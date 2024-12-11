#Region "Microsoft.VisualBasic::fea2d839b17fdd6150445b6884751947, Rscript\Library\mzkit_hpc\src\IpcParallel\LCMS\MakePeakAlignment.vb"

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

'   Total Lines: 143
'    Code Lines: 111 (77.62%)
' Comment Lines: 8 (5.59%)
'    - Xml Docs: 87.50%
' 
'   Blank Lines: 24 (16.78%)
'     File Size: 5.45 KB


' Class MakePeakAlignment
' 
'     Properties: baseline, dtw, errors, joint, rt_range
' 
'     Function: CreatePeaktable, MakePeakTable
' 
' Class PeakTablePack
' 
'     Properties: npeaks, peaktable, rt_shifts, sampleNames
' 
' Class FeatureXic
' 
'     Properties: mz, peak_groups, samples
' 
'     Function: GetPeakTable
' 
' /********************************************************************************/

#End Region

Imports batch
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Tasks
Imports Darwinism.DataScience.DataMining
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json

Public Class MakePeakAlignment

    Public Property errors As String
    Public Property rt_range As Double()
    Public Property baseline As Double
    Public Property joint As Boolean
    Public Property dtw As Boolean

    Public Shared Function CreatePeaktable(peak_groups As XICPool, features_mz As Double(),
                                           errors As Tolerance, rtRange As DoubleRange,
                                           baseline As Double,
                                           joint As Boolean,
                                           dtw As Boolean,
                                           ByRef rt_shifts As RtShift()) As IEnumerable(Of xcms2)
        ' deconvolution task parameters
        Dim pars As New MakePeakAlignment With {
            .baseline = baseline,
            .dtw = dtw,
            .errors = errors.ToScript,
            .joint = joint,
            .rt_range = rtRange.MinMax
        }
        Dim pool As FeatureXic() = features_mz.AsParallel _
            .Select(Function(mz)
                        Dim pick_features = peak_groups.GetXICMatrix(mz, errors).ToArray
                        Dim sources As String() = pick_features.Select(Function(a) a.Name).ToArray
                        Dim peaks As MzGroup() = pick_features.Select(Function(a) a.Value).ToArray

                        Return New FeatureXic With {
                            .mz = mz,
                            .peak_groups = peaks,
                            .samples = sources
                        }
                    End Function) _
            .ToArray
        Dim deconv As New Func(Of FeatureXic(), MakePeakAlignment, PeakTablePack)(AddressOf MakePeakTable)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim vectorPack = pool.Split(CInt(pool.Length / env.n_threads / 2))
        Dim shiftList As New List(Of RtShift)
        Dim peaktable As New List(Of xcms2)

        Call VBDebugger.EchoLine("run lc-ms deconvolution in ipc parallel with arguments:")
        Call VBDebugger.EchoLine(pars.GetJson)

        For Each batch As PeakTablePack In Host.ParallelFor(Of FeatureXic(), PeakTablePack)(env, deconv, vectorPack, SocketRef.WriteBuffer(pars))
            Call peaktable.AddRange(batch.peaktable)
            Call shiftList.AddRange(batch.rt_shifts)
        Next

        rt_shifts = shiftList.ToArray

        Return peaktable
    End Function

    <EmitStream(GetType(XicPack), Target:=GetType(FeatureXic()))>
    Private Shared Function MakePeakTable(peak_groups As FeatureXic(), args As MakePeakAlignment) As PeakTablePack
        Dim peaktable As New List(Of xcms2)
        Dim rt_shifts As New List(Of RtShift)

        For Each peak As FeatureXic In peak_groups
            With peak.GetPeakTable(args)
                Call peaktable.AddRange(.peaks)
                Call rt_shifts.AddRange(.rt_shifts)
            End With
        Next

        Return New PeakTablePack With {
            .peaktable = peaktable.ToArray,
            .rt_shifts = rt_shifts.ToArray
        }
    End Function

End Class

Public Class PeakTablePack

    Public Property peaktable As xcms2()
    Public Property rt_shifts As RtShift()

    Public ReadOnly Property npeaks As Integer
        Get
            Return peaktable.TryCount
        End Get
    End Property

    Public ReadOnly Property sampleNames As String()
        Get
            Return peaktable _
                .Select(Function(a) a.Properties.Keys) _
                .IteratesALL _
                .Distinct _
                .ToArray
        End Get
    End Property

End Class

Public Class FeatureXic

    Public Property mz As Double
    Public Property samples As String()
    ''' <summary>
    ''' the xic features vector is keeps the same dimension size with the 
    ''' <see cref="samples"/> character vector, each sample name has a 
    ''' corresponding xic feature data from this vector in the same index
    ''' offset position.
    ''' </summary>
    ''' <returns></returns>
    Public Property peak_groups As MzGroup()

    Public Function GetPeakTable(pars As MakePeakAlignment) As (peaks As xcms2(), rt_shifts As RtShift())
        Dim samples_xic = samples _
            .Select(Function(name, i)
                        Return New NamedValue(Of MzGroup)(name, peak_groups(i))
                    End Function) _
            .ToArray
        Dim shifts As New List(Of RtShift)

        If pars.dtw Then
            samples_xic = XICPool.DtwXIC(samples_xic).ToArray
        End If

        Dim result As xcms2() = xic_deco_task.ExtractAlignedPeaks(samples_xic,
            rtRange:=New DoubleRange(pars.rt_range),
            baseline:=pars.baseline,
            joint:=pars.joint, xic_align:=True,
            rt_shifts:=shifts)

        Return (result, shifts.ToArray)
    End Function

End Class

