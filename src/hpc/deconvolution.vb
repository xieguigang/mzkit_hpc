
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MZKit.IpcParallel
Imports SMRUCC.Rsharp
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop
Imports vec = SMRUCC.Rsharp.Runtime.Internal.Object.vector

<Package("deconvolution")>
Module deconvolution

    ''' <summary>
    ''' Make peak group alignments and export peaktable
    ''' </summary>
    ''' <param name="peak_groups"></param>
    ''' <param name="features_mz"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <keywords>lc-ms</keywords>
    <ExportAPI("peak_alignments")>
    Public Function peak_alignments(peak_groups As XICPool, features_mz As Double(),
                                    Optional tolerance As Object = "da:0.005",
                                    <RRawVectorArgument>
                                    Optional peak_width As Object = "3,15",
                                    Optional baseline# = 0.65,
                                    Optional joint As Boolean = False,
                                    Optional dtw As Boolean = False,
                                    Optional env As Environment = Nothing) As Object

        Dim errors As [Variant](Of Tolerance, Message) = Math.getTolerance(tolerance, env)
        Dim rtRange = ApiArgumentHelpers.GetDoubleRange(peak_width, env, [default]:="3,15")

        If errors Like GetType(Message) Then
            Return errors.TryCast(Of Message)
        ElseIf rtRange Like GetType(Message) Then
            Return rtRange.TryCast(Of Message)
        End If

        Dim rt_shifts As RtShift() = Nothing
        Dim peaktable As xcms2() = MakePeakAlignment.CreatePeaktable(
            peak_groups, features_mz,
            errors,
            rtRange,
            baseline, joint, dtw,
            rt_shifts).ToArray
        Dim vec As New vec(peaktable, RType.GetRSharpType(GetType(xcms2)))
        Call vec.setAttribute("rt.shift", rt_shifts)
        Return vec
    End Function

End Module
