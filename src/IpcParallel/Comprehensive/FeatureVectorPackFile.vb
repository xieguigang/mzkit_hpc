#Region "Microsoft.VisualBasic::9942810b20b2be41255eea5d6db62408, Rscript\Library\mzkit_hpc\src\IpcParallel\Comprehensive\FeatureVectorPackFile.vb"

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

    '   Total Lines: 80
    '    Code Lines: 63 (78.75%)
    ' Comment Lines: 1 (1.25%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 16 (20.00%)
    '     File Size: 2.94 KB


    '     Class FeatureVectorPackFile
    ' 
    '         Function: BufferInMemory, Load, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    '         Sub: Save
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text

Namespace Comprehensive

    Public Class FeatureVectorPackFile : Implements IEmitStream

        Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
            Return True
        End Function

        Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
            Call Save(DirectCast(obj, FeatureVector()), file)
            Call file.Flush()
            Return True
        End Function

        Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
            Dim file As New MemoryStream
            Call Save(DirectCast(obj, FeatureVector()), file)
            Call file.Flush()
            Call file.Seek(0, SeekOrigin.Begin)
            Return file
        End Function

        Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
            Return Load(file).ToArray
        End Function

        Public Shared Sub Save(pack As FeatureVector(), file As Stream)
            Dim bin As New BinaryDataWriter(file, Encodings.ASCII)

            ' flag n packages
            Call bin.Write(pack.Length)

            For Each vec As FeatureVector In pack
                Call bin.Write(vec.cell_labels.Length)
                Call bin.Write(vec.mz)
                Call bin.Write(vec.mzmin)
                Call bin.Write(vec.mzmax)
                Call bin.Write(vec.intensity)

                For Each str As String In vec.cell_labels
                    Call bin.Write(str, BinaryStringFormat.DwordLengthPrefix)
                Next
            Next

            Call bin.Flush()
        End Sub

        Public Shared Iterator Function Load(file As Stream) As IEnumerable(Of FeatureVector)
            Dim bin As New BinaryDataReader(file, Encodings.ASCII)
            Dim n As Integer = bin.ReadInt32

            For null As Integer = 1 To n
                Dim labels As String() = New String(bin.ReadInt32 - 1) {}
                Dim mz As Double = bin.ReadDouble
                Dim mzmin As Double = bin.ReadDouble
                Dim mzmax As Double = bin.ReadDouble
                Dim into As Double() = bin.ReadDoubles(labels.Length)

                For i As Integer = 0 To labels.Length - 1
                    labels(i) = bin.ReadString(BinaryStringFormat.DwordLengthPrefix)
                Next

                Yield New FeatureVector With {
                    .cell_labels = labels,
                    .mz = mz,
                    .mzmin = mzmin,
                    .mzmax = mzmax,
                    .intensity = into
                }
            Next
        End Function

    End Class
End Namespace
