#Region "Microsoft.VisualBasic::ae4aa016e39ed5094508f97328bf5569, Rscript\Library\mzkit_hpc\src\IpcParallel\Comprehensive\DoStatSingleCellsMatrix.vb"

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

    '   Total Lines: 55
    '    Code Lines: 46 (83.64%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 9 (16.36%)
    '     File Size: 2.30 KB


    '     Module DoStatSingleCellsMatrix
    ' 
    '         Function: getFeatures, MeasureIonFeatures, MeasureIonsFeaturesTask
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports batch
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Darwinism.DataScience.DataMining
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Linq

Namespace Comprehensive

    Public Module DoStatSingleCellsMatrix

        <Extension>
        Public Iterator Function MeasureIonFeatures(matrix As MzMatrix) As IEnumerable(Of SingleCellIonStat)
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
            Dim labels As String() = matrix.matrix _
                .Select(Function(si) si.label) _
                .ToArray

            For i As Integer = 0 To matrix.featureSize - 1
                offset = i

                Yield New FeatureVector With {
                    .mz = matrix.mz(i),
                    .mzmin = matrix.mzmin(i),
                    .mzmax = matrix.mzmax(i),
                    .intensity = (From cell As PixelData
                                  In mat
                                  Select cell(offset)).ToArray,
                    .cell_labels = labels
                }
            Next
        End Function

        <EmitStream(GetType(FeatureVectorPackFile), Target:=GetType(FeatureVector()))>
        Public Function MeasureIonsFeaturesTask(packs As FeatureVector()) As SingleCellIonStat()
            Return (From i As FeatureVector In packs Select i.MeasureStat).ToArray
        End Function

    End Module
End Namespace
