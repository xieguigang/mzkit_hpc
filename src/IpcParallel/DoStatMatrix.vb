Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports batch
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.GraphTheory.KNearNeighbors
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports Darwinism.DataScience.DataMining
Imports System.IO
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Text
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.ASCII.MGF

Public Module DoStatMatrix

    <Extension>
    Public Iterator Function MeasureIonFeatures(matrix As MzMatrix) As IEnumerable(Of SingleCellIonStat)
        Dim labels As String() = matrix.matrix _
            .Select(Function(si) si.label) _
            .ToArray
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim vectorPack = matrix.getFeatures _
            .Split(CInt(matrix.featureSize / env.n_threads / 2)) _
            .Select(Function(part)
                        Return New FeatureVectorPack With {
                            .cell_labels = labels,
                            .ions = part
                        }
                    End Function) _
            .ToArray
        Dim pool As SocketRef = SocketRef.WriteBuffer(vectorPack, StreamEmit.Custom(Of FeatureVectorPack())(New FeatureVectorPackFile))


    End Function

    <Extension>
    Private Iterator Function getFeatures(matrix As MzMatrix) As IEnumerable(Of FeatureVector)
        Dim offset As Integer
        Dim mat As PixelData() = matrix.matrix

        For i As Integer = 0 To matrix.featureSize - 1
            offset = i

            Yield New FeatureVector With {
                .mz = matrix.mz(i),
                .mzmin = matrix.mzmin(i),
                .mzmax = matrix.mzmax(i),
                .intensity = (From cell As PixelData
                              In mat
                              Select cell(offset)).ToArray
            }
        Next
    End Function

    <EmitStream(GetType(FeatureVectorPackFile), Target:=GetType(FeatureVectorPack()))>
    Public Function MeasureIonsFeatures(packs As FeatureVectorPack()) As SingleCellIonStat()

    End Function

End Module

Public Class FeatureVectorPackFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Call FeatureVectorPack.Save(DirectCast(obj, FeatureVectorPack()), file)
        Call file.Flush()
        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim file As New MemoryStream
        Call FeatureVectorPack.Save(DirectCast(obj, FeatureVectorPack()), file)
        Call file.Flush()
        Call file.Seek(0, SeekOrigin.Begin)
        Return file
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Return FeatureVectorPack.Load(file).ToArray
    End Function
End Class

Public Class FeatureVectorPack

    Public Property cell_labels As String()
    Public Property ions As FeatureVector()

    Public Shared Sub Save(pack As FeatureVectorPack(), file As Stream)
        Dim bin As New BinaryDataWriter(file, Encodings.ASCII)

        ' flag n packages
        Call bin.Write(pack.Length)

        For Each vec As FeatureVectorPack In pack
            Call bin.Write(vec.ions.Length)
            Call bin.Write(vec.cell_labels.Length)

            For Each str As String In vec.cell_labels
                Call bin.Write(str, BinaryStringFormat.DwordLengthPrefix)
            Next
            For Each ion As FeatureVector In vec.ions
                Call bin.Write(ion.mz)
                Call bin.Write(ion.mzmin)
                Call bin.Write(ion.mzmax)
                Call bin.Write(ion.intensity)
            Next
        Next

        Call bin.Flush()
    End Sub

    Public Shared Iterator Function Load(file As Stream) As IEnumerable(Of FeatureVectorPack)
        Dim bin As New BinaryDataReader(file, Encodings.ASCII)
        Dim n As Integer = bin.ReadInt32

        For null As Integer = 1 To n
            Dim ions As FeatureVector() = New FeatureVector(bin.ReadInt32 - 1) {}
            Dim labels As String() = New String(bin.ReadInt32 - 1) {}

            For i As Integer = 0 To labels.Length - 1
                labels(i) = bin.ReadString(BinaryStringFormat.DwordLengthPrefix)
            Next
            For i As Integer = 0 To ions.Length - 1
                ions(i) = New FeatureVector With {
                    .mz = bin.ReadDouble,
                    .mzmin = bin.ReadDouble,
                    .mzmax = bin.ReadDouble,
                    .intensity = bin.ReadDoubles(labels.Length)
                }
            Next

            Yield New FeatureVectorPack With {
                .cell_labels = labels,
                .ions = ions
            }
        Next
    End Function

End Class

Public Class FeatureVector

    Public Property mz As Double
    Public Property mzmin As Double
    Public Property mzmax As Double
    Public Property intensity As Double()

End Class