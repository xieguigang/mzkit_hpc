#Region "Microsoft.VisualBasic::e8aae5003b2f7a45dff7144513a029e8, Rscript\Library\mzkit_hpc\src\hpc\deconvolution.vb"

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

    '   Total Lines: 63
    '    Code Lines: 41 (65.08%)
    ' Comment Lines: 16 (25.40%)
    '    - Xml Docs: 93.75%
    ' 
    '   Blank Lines: 6 (9.52%)
    '     File Size: 2.58 KB


    ' Module deconvolution
    ' 
    '     Function: peak_alignments
    ' 
    ' /********************************************************************************/

#End Region

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

''' <summary>
''' LCMS rawdata processor
''' </summary>
<Package("deconvolution")>
Module deconvolution

    ''' <summary>
    ''' Make peak group alignments and export peaktable
    ''' </summary>
    ''' <param name="peak_groups"></param>
    ''' <param name="features_mz"></param>
    ''' <returns>
    ''' this function returns a vector of the <see cref="xcms2"/> peaks data, andalso 
    ''' the <see cref="RtShift"/> value is attached in the value vector via R# object
    ''' attribute named ``rt.shift``.
    ''' </returns>
    ''' <remarks></remarks>
    ''' <keywords>lc-ms</keywords>
    ''' 
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
