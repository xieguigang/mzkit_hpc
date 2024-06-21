Imports System.Runtime.CompilerServices
Imports batch
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Darwinism.DataScience.DataMining
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Linq

Public Module DoStatSingleCellsMatrix

    <Extension>
    Public Iterator Function MeasureIonFeatures(matrix As MzMatrix) As IEnumerable(Of SingleCellIonStat)
        Dim labels As String() = matrix.matrix _
            .Select(Function(si) si.label) _
            .ToArray
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim vectorPack = matrix.getFeatures.Split(CInt(matrix.featureSize / env.n_threads / 2))
        Dim task As New Func(Of FeatureVector(), SingleCellIonStat())(AddressOf MeasureIonsFeaturesTask)

        For Each batch As SingleCellIonStat() In Host.ParallelFor(Of FeatureVector(), SingleCellIonStat())(env, task, vectorPack)
            For Each ion_stat As SingleCellIonStat In batch
                Yield ion_stat
            Next
        Next
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

    <EmitStream(GetType(FeatureVectorPackFile), Target:=GetType(FeatureVector()))>
    Public Function MeasureIonsFeaturesTask(packs As FeatureVector()) As SingleCellIonStat()
        Return (From i As FeatureVector In packs Select i.MeasureStat).ToArray
    End Function

End Module
