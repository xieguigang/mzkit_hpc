Imports System.Runtime.CompilerServices
Imports batch
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Darwinism.DataScience.DataMining
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream

Public Module DoStatMSImagingMatrix

    <Extension>
    Public Iterator Function MeasureIonFeatures(matrix As MzMatrix, grid_size As Integer) As IEnumerable(Of IonStat)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim vectorPack = matrix.getFeatures.Split(CInt(matrix.featureSize / env.n_threads / 2))
        Dim task As New Func(Of FeatureVector(), Integer, IonStat())(AddressOf MeasureIonsFeaturesTask)

        For Each batch As IonStat() In Host.ParallelFor(Of FeatureVector(), IonStat())(env, task, vectorPack, SocketRef.WriteBuffer(grid_size))
            For Each ion_stat As IonStat In batch
                Yield ion_stat
            Next
        Next
    End Function

    <Extension>
    Private Iterator Function getFeatures(matrix As MzMatrix) As IEnumerable(Of FeatureVector)
        Dim offset As Integer
        Dim mat = matrix.matrix
        Dim labels As String() = matrix.matrix _
            .Select(Function(si) si.X & "," & si.Y & "," & si.Z) _
            .ToArray

        For i As Integer = 0 To matrix.featureSize - 1
            offset = i

            Yield New FeatureVector With {
                .mz = matrix.mz(i),
                .mzmin = matrix.mzmin(i),
                .mzmax = matrix.mzmax(i),
                .intensity = (From cell In mat Select cell(offset)).ToArray,
                .cell_labels = labels
            }
        Next
    End Function

    <EmitStream(GetType(FeatureVectorPackFile), Target:=GetType(FeatureVector()))>
    Public Function MeasureIonsFeaturesTask(packs As FeatureVector(), grid_size As Integer) As IonStat()
        Return (From i As FeatureVector In packs Select i.MeasureMSIIon(grid_size)).ToArray
    End Function
End Module
