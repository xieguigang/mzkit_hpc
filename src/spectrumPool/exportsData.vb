#Region "Microsoft.VisualBasic::e40a0501f45362c4380827bbae2f9981, Rscript\Library\mzkit_hpc\src\spectrumPool\exportsData.vb"

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

    '   Total Lines: 73
    '    Code Lines: 67 (91.78%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 6 (8.22%)
    '     File Size: 2.73 KB


    ' Module exportsData
    ' 
    '     Function: ExportClusterSpectrum
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module exportsData

    <Extension>
    Public Iterator Function ExportClusterSpectrum(data As dataPool, cluster_id As UInteger) As IEnumerable(Of PeakMs2)
        Dim spectral_id As UInteger() = data.cluster_data _
            .where(field("cluster_id") = cluster_id) _
            .project(Of UInteger)("spectral_id")
        Dim spectrumData As spectrumData() = data.spectrum_pool _
            .left_join("metadata") _
            .on(field("`metadata`.`spectral_id`") = field("`spectrum_pool`.`id`")) _
            .where(field("`spectrum_pool`.`id`").in(spectral_id)) _
            .select(Of spectrumData)("npeaks",
    "spectrum_pool.mz",
    "`into`",
    "metadata.mz AS precursor",
    "rt",
    "intensity",
    "filename",
    "project",
    "biosample",
    "organism",
    "xref_id",
    "`name`",
    "formula",
    "splash_id",
    "adducts",
    "instrument")

        For Each q As spectrumData In spectrumData
            Dim mz As Double() = HttpTreeFs.decode(q.mz)
            Dim into As Double() = HttpTreeFs.decode(q.into)

            If q.npeaks <> mz.Length Then
                Continue For
            ElseIf q.npeaks <> into.Length Then
                Continue For
            End If

            Dim spectral As ms2() = mz _
                .Select(Function(mzi, i)
                            Return New ms2 With {
                                .mz = mzi,
                                .intensity = into(i)
                            }
                        End Function) _
                .ToArray

            Yield New PeakMs2 With {
                .intensity = q.intensity,
                .mz = q.precursor,
                .rt = q.rt,
                .file = q.filename,
                .mzInto = spectral,
                .precursor_type = q.adducts,
                .lib_guid = q.splash_id & " " & $"{q.precursor.ToString("F4")}@{(q.rt / 60).ToString("F2")}min",
                .meta = New Dictionary(Of String, String) From {
                    {NameOf(q.biosample), q.biosample},
                    {NameOf(q.name), q.name},
                    {NameOf(q.formula), q.formula},
                    {NameOf(q.instrument), q.instrument},
                    {NameOf(q.organism), q.organism},
                    {NameOf(q.project), q.project},
                    {NameOf(q.xref_id), q.xref_id}
                }
            }
        Next
    End Function
End Module
