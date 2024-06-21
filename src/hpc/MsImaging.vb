Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MZKit.IpcParallel
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' MS-imaging rawdata processing in HPC parallel
''' </summary>
<Package("MsImaging")>
Public Module MsImaging

    Sub Main()
    End Sub

    ''' <summary>
    ''' run measure of the ion features in IPC parallel
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    <ExportAPI("SCMs_ionStat_parallel")>
    <RApiReturn(GetType(SingleCellIonStat))>
    Public Function singleCellsStats(x As MzMatrix) As Object
        Return x.MeasureIonFeatures.ToArray
    End Function
End Module
