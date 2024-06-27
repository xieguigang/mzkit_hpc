Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MZKit.IpcParallel.Comprehensive
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' MS-imaging rawdata processing in HPC parallel
''' </summary>
<Package("MsImaging")>
Public Module MsImaging

    Sub Main()
    End Sub

    ''' <summary>
    ''' run measure of the ion features in IPC parallel for a huge single cells rawdata matrix
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    ''' <keywords>single cells;parallel;features</keywords>
    <ExportAPI("SCMs_ionStat_parallel")>
    <RApiReturn(GetType(SingleCellIonStat))>
    Public Function singleCellsStats(x As MzMatrix) As Object
        Return x.MeasureIonFeatures.ToArray
    End Function

    ''' <summary>
    ''' run measure of the ion features in IPC parallel for a huge ms-imaging rawdata matrix
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="grid_size"></param>
    ''' <returns></returns>
    ''' <keywords>spatial;parallel;features</keywords>
    <ExportAPI("MSI_ionStat_parallel")>
    <RApiReturn(GetType(IonStat))>
    Public Function msiIonsStats(x As MzMatrix, Optional grid_size As Integer = 5) As Object
        Return x.MeasureIonFeatures(grid_size).ToArray
    End Function
End Module
