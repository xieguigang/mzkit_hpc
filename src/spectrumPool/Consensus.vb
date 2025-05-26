Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Module Consensus

    <Extension>
    Public Function CreateModelParameters(mysql As dataPool,
                                          model As String,
                                          dimensions As Integer,
                                          knn As Integer,
                                          Optional cutoff As Double = 0.8) As clusterModels.consensus_model

        Dim modelObj As clusterModels.graph_model = mysql.graph_model _
            .where(field("name") = model) _
            .find(Of clusterModels.graph_model)

        If modelObj Is Nothing Then
            Throw New ArgumentException($"Model '{model}' not found in the database.")
        End If

        Call mysql.consensus_model.add(
            field("model_id") = modelObj.id,
            field("consensus_cutoff") = cutoff,
            field("umap_dimension") = dimensions,
            field("umap_neighbors") = knn,
            field("umap_others") = "{}"
        )

        Return mysql.consensus_model _
            .where(field("model_id") = modelObj.id) _
            .order_by("id", desc:=True) _
            .find(Of clusterModels.consensus_model)
    End Function

    <Extension>
    Public Sub ScanConsensus(mysql As dataPool, args As clusterModels.consensus_model, Optional page_size As Integer = 1000, Optional top As Integer = 30)
        Dim mzdiff As Tolerance = Tolerance.DeltaMass(0.1)

        For page As Integer = 0 To Integer.MaxValue
            Dim offset As UInteger = (page - 1) * page_size
            Dim pagedata = mysql.cluster.where(field("model_id") = args.model_id).limit(offset, page_size).select(Of clusterModels.cluster)

            If pagedata.IsNullOrEmpty Then
                Exit For
            End If

            For Each cluster As clusterModels.cluster In TqdmWrapper.Wrap(pagedata)
                Dim spectrumData As clusterSpectrumData() = mysql.cluster_data _
                    .left_join("metadata").on(field("`metadata`.id") = field("metadata_id")) _
                    .left_join("spectrum_pool").on(field("`spectrum_pool`.id") = field("`metadata`.spectral_id")) _
                    .where(field("`cluster_data`.cluster_id") = cluster.id) _
                    .select(Of clusterSpectrumData)(
                        "metadata.mz as precursor",
                        "rt",
                        "intensity AS `into`",
                        "spectrum_pool.mz",
                        "`into` AS intensity")
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
                Dim consens As NamedCollection(Of ms2)() = decodeSpectrum _
                    .Select(Function(s) s.mzInto) _
                    .IteratesALL _
                    .GroupBy(mzdiff) _
                    .OrderByDescending(Function(peak) peak.Length) _
                    .Take(top) _
                    .ToArray
                Dim mz_str As String = HttpTreeFs.encode(consens.Select(Function(m) Val(m.name)))
                Dim intensity_str As String = HttpTreeFs.encode(consens.Select(Function(m) m.Average(Function(i) i.intensity)))
                Dim peak_ranking As ms2() = consens _
                    .Select(Function(m) Val(m.name)) _
                    .consens_peakRanking(decodeSpectrum, mzdiff) _
                    .ToArray

            Next
        Next
    End Sub

    <Extension>
    Private Iterator Function consens_peakRanking(consensus_mz As IEnumerable(Of Double), clusterdata As PeakMs2(), mzdiff As Tolerance) As IEnumerable(Of ms2)
        For Each mz As Double In consensus_mz
            Dim count As Integer = clusterdata _
                .AsParallel _
                .Where(Function(s) s.GetIntensity(mz, mzdiff) > 0) _
                .Count

            Yield New ms2 With {
                .mz = mz,
                .intensity = count / clusterdata.Length,
                .Annotation = ""
            }
        Next
    End Function
End Module

Public Class clusterSpectrumData

    <DatabaseField> Public Property precursor As Double
    <DatabaseField> Public Property rt As Double
    <DatabaseField> Public Property into As Double
    <DatabaseField> Public Property mz As String
    <DatabaseField> Public Property intensity As String

End Class