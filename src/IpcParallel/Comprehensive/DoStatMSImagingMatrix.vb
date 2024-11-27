#Region "Microsoft.VisualBasic::1c3f0e93ad8113ef5552693ba0c2e744, Rscript\Library\mzkit_hpc\src\IpcParallel\Comprehensive\DoStatMSImagingMatrix.vb"

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

    '   Total Lines: 52
    '    Code Lines: 44 (84.62%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (15.38%)
    '     File Size: 2.27 KB


    '     Module DoStatMSImagingMatrix
    ' 
    '         Function: getFeatures, MeasureIonFeatures, MeasureIonsFeaturesTask
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports batch
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Darwinism.DataScience.DataMining
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream

Namespace Comprehensive

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
End Namespace
