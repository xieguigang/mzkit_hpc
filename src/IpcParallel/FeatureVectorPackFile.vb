Imports System.IO
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Text

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

