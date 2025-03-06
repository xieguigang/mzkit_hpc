#Region "Microsoft.VisualBasic::a22baacb882f0c045be965353e50be6f, Rscript\Library\mzkit_hpc\src\hpc\MsImaging.vb"

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
    '    Code Lines: 25 (45.45%)
    ' Comment Lines: 24 (43.64%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (10.91%)
    '     File Size: 2.03 KB


    ' Module MsImaging
    ' 
    '     Function: HEstain_tissueReader, msiIonsStats, singleCellsStats
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells.Deconvolute
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MZKit.IpcParallel.Comprehensive
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' MS-imaging rawdata processing helper in HPC parallel
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

    ''' <summary>
    ''' image processor for huge HE-stain bitmap file 
    ''' </summary>
    ''' <param name="file">
    ''' the file path to the HE-stain image file, should be processed as bitmap 
    ''' file at first via image processing software like photoshop.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("HEstain_tissue_reader")>
    Public Function HEstain_tissueReader(file As String) As Object

    End Function
End Module
