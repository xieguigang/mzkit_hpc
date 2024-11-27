#Region "Microsoft.VisualBasic::146656a5df05d9b4bbeb81253c19e219, Rscript\Library\mzkit_hpc\src\IpcParallel\LCMS\StreamFiles.vb"

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

    '   Total Lines: 79
    '    Code Lines: 62 (78.48%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 17 (21.52%)
    '     File Size: 2.72 KB


    ' Class PeakResultPack
    ' 
    '     Function: BufferInMemory, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    ' Class XicPack
    ' 
    '     Function: BufferInMemory, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class PeakResultPack : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim result As PeakTablePack = obj

        Call SaveXcms.DumpSample(result.peaktable, result.npeaks, result.sampleNames, file)
        Call SaveRtShifts.DumpShiftsData(result.rt_shifts, file)
        Call file.Flush()

        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim file As New MemoryStream
        Call WriteBuffer(obj, file)
        Return file
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim peak As xcms2() = SaveXcms.ReadSamplePeaks(file)
        Dim rtshifts As RtShift() = SaveRtShifts.ParseRtShifts(file).ToArray

        Return New PeakTablePack With {
            .peaktable = peak,
            .rt_shifts = rtshifts
        }
    End Function
End Class

Public Class XicPack : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim xicdata As FeatureXic = obj
        Dim bin As New BinaryWriter(file)

        bin.Write(xicdata.samples.Length)
        bin.Write(xicdata.mz)
        bin.Write(xicdata.samples.GetJson)
        bin.Flush()

        SaveXIC.DumpSample(xicdata.peak_groups, file)
        file.Flush()

        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim file As New MemoryStream
        Call WriteBuffer(obj, file)
        Return file
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim bin As New BinaryReader(file)
        Dim size As Integer = bin.ReadInt32
        Dim mz As Double = bin.ReadDouble
        Dim samples As String() = bin.ReadString.LoadJSON(Of String())
        Dim xicdata As MzGroup() = SaveXIC.ReadSample(file).ToArray

        Return New FeatureXic With {
            .mz = mz,
            .samples = samples,
            .peak_groups = xicdata
        }
    End Function
End Class
