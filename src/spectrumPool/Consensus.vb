﻿#Region "Microsoft.VisualBasic::7c1b745625eca827f77e891fc0510c7e, Rscript\Library\mzkit_hpc\src\spectrumPool\Consensus.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 400
    '    Code Lines: 351 (87.75%)
    ' Comment Lines: 3 (0.75%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 46 (11.50%)
    '     File Size: 17.82 KB


    ' Module Consensus
    ' 
    '     Function: buildClusterName, ClusterAnnotation, consens_peakRanking, CreateModelParameters, RankFormula
    ' 
    '     Sub: ScanConsensus
    ' 
    ' Class clusterSpectrumData
    ' 
    '     Properties: adducts, formula, intensity, into, mz
    '                 name, precursor, rt, xref_id
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1.PrecursorType
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula
Imports BioNovoGene.BioDeep.Chemoinformatics.Formula.MS
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports BioNovoGene.BioDeep.MSFinder
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Distributions
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes
Imports spectrumPool.clusterModels

Public Module Consensus

    <Extension>
    Public Function CreateModelParameters(mysql As dataPool,
                                          model As String,
                                          Optional dimensions As Integer = 10,
                                          Optional knn As Integer = 64,
                                          Optional cutoff As Double = 0.8) As clusterModels.consensus_model

        Dim modelObj As clusterModels.graph_model = mysql.graph_model _
            .where(field("name") = model) _
            .find(Of clusterModels.graph_model)

        If modelObj Is Nothing Then
            Throw New ArgumentException($"Model '{model}' not found in the database.")
        End If

        Dim json As New Dictionary(Of String, String) From {
            {"model_id", modelObj.id},
            {"cutoff", cutoff.ToString("F4")},
            {"dims", dimensions},
            {"knn", knn},
            {"others", "{}"}
        }
        Dim hashcode As String = json.GetJson.MD5
        Dim check = mysql.consensus_model _
            .where(field("model_id") = modelObj.id,
                   field("hashcode") = hashcode) _
            .find(Of clusterModels.consensus_model)

        If Not check Is Nothing Then
            Return check
        Else
            Call mysql.consensus_model.add(
                field("model_id") = modelObj.id,
                field("consensus_cutoff") = cutoff,
                field("umap_dimension") = dimensions,
                field("umap_neighbors") = knn,
                field("umap_others") = "{}",
                field("hashcode") = hashcode
            )
        End If

        Return mysql.consensus_model _
            .where(field("model_id") = modelObj.id) _
            .order_by("id", desc:=True) _
            .find(Of clusterModels.consensus_model)
    End Function

    <Extension>
    Public Sub ScanConsensus(mysql As dataPool, args As clusterModels.consensus_model,
                             Optional page_size As Integer = 1000,
                             Optional top As Integer = 30)

        For page As Integer = 1 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim pagedata As clusterModels.cluster() = mysql.cluster _
                .where(field("model_id") = args.model_id) _
                .limit(offset, page_size) _
                .select(Of clusterModels.cluster)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each cluster As clusterModels.cluster In TqdmWrapper.Wrap(pagedata)
                Call mysql.ClusterAnnotation(args, cluster, , top)
            Next
        Next
    End Sub

    <Extension>
    Public Function ClusterAnnotation(mysql As dataPool,
                                      args As clusterModels.consensus_model,
                                      cluster As clusterModels.cluster,
                                      Optional ms2da As Double = 0.1,
                                      Optional top As Integer = 30) As clusterModels.consensus_spectrum

        Dim checkSpectrum = mysql.consensus_spectrum _
            .where(field("cluster_id") = cluster.id,
                   field("parameter_id") = args.id) _
            .find(Of clusterModels.consensus_spectrum)
        Dim spectrumData As clusterSpectrumData() = mysql.cluster_data _
            .left_join("metadata").on(field("`metadata`.id") = field("metadata_id")) _
            .left_join("spectrum_pool").on(field("`spectrum_pool`.id") = field("`metadata`.spectral_id")) _
            .where(field("`cluster_data`.cluster_id") = cluster.id) _
            .select(Of clusterSpectrumData)(
                "metadata.mz as precursor",
                "rt",
                "formula",
                "xref_id",
                "name",
                "adducts",
                "intensity AS `into`",
                "spectrum_pool.mz",
                "`into` AS intensity")

        If checkSpectrum IsNot Nothing AndAlso spectrumData.Length = checkSpectrum.source_size Then
            Return checkSpectrum
        End If

        Dim decodeSpectrum As PeakMs2() = spectrumData _
            .Select(Function(sdata)
                        Dim spectrum As ms2() = HttpTreeFs.decodeSpectrum(sdata.mz, sdata.intensity) _
                            .SafeQuery _
                            .ToArray

                        If spectrum.Length = 0 Then
                            Return Nothing
                        Else
                            Return New PeakMs2 With {
                                .mzInto = spectrum,
                                .mz = sdata.precursor,
                                .rt = sdata.rt
                            }
                        End If
                    End Function) _
            .Where(Function(s) Not s Is Nothing) _
            .ToArray
        Dim precursor_groups = spectrumData.Select(Function(s) s.precursor) _
            .GroupBy(Tolerance.DeltaMass(0.3)) _
            .OrderByDescending(Function(p) p.Length) _
            .ToArray
        Dim precursor_mz As Double = Val(precursor_groups(0).name)
        Dim mzdiff As Tolerance = Tolerance.DeltaMass(ms2da)
        Dim consens As NamedCollection(Of ms2)() = decodeSpectrum _
            .Select(Function(s) s.mzInto) _
            .IteratesALL _
            .GroupBy(mzdiff) _
            .OrderByDescending(Function(peak) peak.Length) _
            .Take(top) _
            .ToArray
        Dim mz_str As String = HttpTreeFs.encode(consens.Select(Function(m) Val(m.name)))
        Dim intensity_str As String = HttpTreeFs.encode(consens.Select(Function(m) m.Average(Function(i) i.intensity)))
        Dim metabolites = spectrumData _
            .Where(Function(s) s.formula <> "NA") _
            .ToArray
        Dim formulaSet As Dictionary(Of String, (src As NamedValue(Of String)(), size%)) = metabolites _
            .GroupBy(Function(s) s.formula) _
            .ToDictionary(Function(s) s.Key,
                          Function(s)
                              Return (s _
                                  .GroupBy(Function(i) i.xref_id) _
                                  .Select(Function(a)
                                              Return New NamedValue(Of String)(a.Key, a.First.name)
                                          End Function) _
                                  .ToArray, s.Count)
                          End Function)
        Dim adducts As String() = metabolites _
            .Select(Function(s) s.adducts) _
            .Distinct _
            .ToArray
        Dim topFormula = formulaSet _
            .Select(Function(f_str)
                        Return RankFormula(f_str, decodeSpectrum, precursor_mz, adducts)
                    End Function) _
            .Where(Function(c) Not c.formula Is Nothing) _
            .OrderByDescending(Function(f)
                                   Return f.replicates * f.scores.Average
                               End Function) _
            .FirstOrDefault
        Dim ref_rt As Double = spectrumData.Select(Function(a) a.rt).Where(Function(rt) rt > 0).TabulateMode(topBin:=True, bags:=9)
        Dim peak_ranking As ms2() = consens _
            .Select(Function(m) Val(m.name)) _
            .consens_peakRanking(decodeSpectrum, precursor_mz, topFormula.adducts, topFormula.formula, mzdiff) _
            .ToArray
        Dim precursor_entropy As Double = New PeakMs2("", precursor_groups.Select(Function(i) New ms2(Val(i.name), Val(i.Length)))).Entropy
        Dim consensusSpectrum As New PeakMs2("", consens.Select(Function(i) New ms2(Val(i.name), i.Average(Function(a) a.intensity))))
        Dim entropy As Double = consensusSpectrum.Entropy
        Dim splash_id As String = SplashID.MsSplashId(consensusSpectrum)
        Dim cluster_name As String = metabolites.buildClusterName(topFormula.formula,
                                                                  topFormula.adducts?.AdductIonName,
                                                                  consensusSpectrum)

        Dim formula_str As String
        Dim adducts_str As String
        Dim xref_str As String

        If topFormula.formula Is Nothing OrElse topFormula.adducts.AdductIonName Is Nothing Then
            ' deal wit hthe missing formula data
            formula_str = "*"
            adducts_str = "*"
            xref_str = "-"
        Else
            formula_str = topFormula.formula.EmpiricalFormula
            adducts_str = topFormula.adducts.AdductIonName
            xref_str = topFormula.src.Select(Function(s) s.Name).JoinBy(", ")
        End If

        If checkSpectrum Is Nothing Then
            ' add new
            Call mysql.consensus_spectrum.add(
                field("cluster_id") = cluster.id,
                field("parameter_id") = args.id,
                field("source_size") = spectrumData.Length,
                field("precursor") = precursor_mz,
                field("precursor_entropy") = precursor_entropy,
                field("consensus_entropy") = entropy,
                field("splash_id") = splash_id,
                field("name") = cluster_name,
                field("formula") = formula_str,
                field("adducts") = adducts_str,
                field("rt") = ref_rt,
                field("mz") = mz_str,
                field("idset") = xref_str,
                field("intensity") = intensity_str,
                field("peak_ranking") = peak_ranking.GetJson,
                field("umap") = ""
            )
        Else
            ' make updates
            Call mysql.consensus_spectrum.where(field("id") = checkSpectrum.id).save(
                field("source_size") = spectrumData.Length,
                field("precursor") = precursor_mz,
                field("precursor_entropy") = precursor_entropy,
                field("consensus_entropy") = entropy,
                field("splash_id") = splash_id,
                field("name") = cluster_name,
                field("formula") = formula_str,
                field("adducts") = adducts_str,
                field("rt") = ref_rt,
                field("mz") = mz_str,
                field("intensity") = intensity_str,
                field("peak_ranking") = peak_ranking.GetJson,
                field("idset") = xref_str
            )
        End If

        Return mysql.consensus_spectrum _
            .where(field("cluster_id") = cluster.id,
                   field("parameter_id") = args.id) _
            .find(Of clusterModels.consensus_spectrum)
    End Function

    <Extension>
    Private Function buildClusterName(metabolites As clusterSpectrumData(), topFormula As Formula, adducts As String, consensusSpectrum As PeakMs2) As String
        If topFormula IsNot Nothing AndAlso adducts IsNot Nothing Then
            Dim names = metabolites _
                .Where(Function(a) FormulaScanner.ScanFormula(a.formula) = topFormula AndAlso adducts = a.adducts) _
                .Select(Function(a) a.name) _
                .Distinct _
                .ToArray

            If Not names.IsNullOrEmpty Then
                If names.Length = 1 Then
                    Return names(0)
                Else
                    Return $"Isomer[{names.JoinBy("; ")}]"
                End If
            End If

            If names.IsNullOrEmpty Then
                names = metabolites _
                    .Where(Function(a) FormulaScanner.ScanFormula(a.formula) = topFormula) _
                    .Select(Function(a) a.name) _
                    .Distinct _
                    .ToArray
            End If

            If Not names.IsNullOrEmpty Then
                If names.Length = 1 Then
                    Return $"{names(0)}*"
                Else
                    Return $"Candidate Isomer[{names.JoinBy("; ")}]"
                End If
            End If
        End If

        Return "unknown conserved[" & consensusSpectrum.mzInto _
            .OrderByDescending(Function(m) m.intensity) _
            .Take(5) _
            .Select(Function(mzi) mzi.mz.ToString("F2")) _
            .JoinBy("/") & "]"
    End Function

    <Extension>
    Private Iterator Function consens_peakRanking(consensus_mz As IEnumerable(Of Double),
                                                  clusterdata As PeakMs2(),
                                                  precursor As Double,
                                                  adducts As AdductIon,
                                                  formula As Formula,
                                                  mzdiff As Tolerance) As IEnumerable(Of ms2)

        Dim annotation As FragmentAssigner = FragmentAssigner.Default
        Dim peaks As New List(Of SpectrumPeak)

        For Each mz As Double In consensus_mz
            Dim count As Integer = clusterdata _
                .AsParallel _
                .Where(Function(s) s.GetIntensity(mz, mzdiff) > 0) _
                .Count

            Call peaks.Add(New SpectrumPeak(New ms2 With {
                .mz = mz,
                .intensity = count / clusterdata.Length,
                .Annotation = ""
            }))
        Next

        If formula Is Nothing OrElse adducts Is Nothing Then
            For Each peak As SpectrumPeak In peaks
                Yield New ms2(peak)
            Next

            Return
        End If

        Dim checkPpm As Tolerance = Tolerance.PPM(15)
        Dim annotated = annotation.FastFragmnetAssigner(peaks, formula, adducts)

        For Each peak As SpectrumPeak In peaks
            Dim find As ProductIon = annotated _
                .Where(Function(a) checkPpm(a.Mass, peak.mz)) _
                .OrderBy(Function(a) PPMmethod.PPM(a.Mass, peak.mz)) _
                .FirstOrDefault

            If find Is Nothing Then
                Yield New ms2(peak)
            Else
                Dim name As String = find.Name

                If find.Comment <> "FragmentFormula" Then
                    name = {find.Name, find.ShortName}.Distinct.JoinBy(" ") & "(" & find.Formula.EmpiricalFormula & ")"
                End If

                Yield New ms2(peak) With {
                    .Annotation = name
                }
            End If
        Next
    End Function

    Private Function RankFormula(f_str As KeyValuePair(Of String, (src As NamedValue(Of String)(), size%)),
                                 clusterdata As PeakMs2(),
                                 precursor As Double,
                                 adducts As String()) As (formula As Formula, replicates As Integer, src As NamedValue(Of String)(), scores As Double(), adducts As AdductIon)

        Dim formula As Formula = FormulaScanner.ScanFormula(f_str.Key)
        Dim rank As New AdductsRanking
        Dim filter_adducts = rank.RankAdducts(formula, adducts).ToArray
        Dim adduct_type As TypeMatch = filter_adducts.FindPrecursorType(formula.ExactMass, precursor, tolerance:=Tolerance.DeltaMass(0.5))

        If adduct_type.errors.IsNaNImaginary Then
            Return Nothing
        End If

        Static annotation As FragmentAssigner = FragmentAssigner.Default

        Dim precursor_type As New AdductIon(adduct_type.adducts)
        Dim annotations As Double() = clusterdata _
            .AsParallel _
            .Select(Function(spec)
                        Dim result = annotation.FastFragmnetAssigner(spec.GetPeaks.AsList, formula, precursor_type)
                        Dim fragments = result.Where(Function(s) Not s.Name.StringEmpty(, True)).Count

                        Return fragments / spec.mzInto.Length
                    End Function) _
            .ToArray

        Return (formula,
            replicates:=f_str.Value.size,
            src:=f_str.Value.src,
            scores:=annotations,
            adducts:=precursor_type
        )
    End Function
End Module

Public Class clusterSpectrumData

    <DatabaseField> Public Property precursor As Double
    <DatabaseField> Public Property rt As Double
    <DatabaseField> Public Property into As Double
    <DatabaseField> Public Property mz As String
    <DatabaseField> Public Property intensity As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property adducts As String
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property xref_id As String

End Class
