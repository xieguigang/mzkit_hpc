#Region "Microsoft.VisualBasic::fa3d680aeb24bdb241be10efbe35d2b1, Rscript\Library\mzkit_hpc\src\hpc\MoleculeNetworking.vb"

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

    '   Total Lines: 122
    '    Code Lines: 82 (67.21%)
    ' Comment Lines: 22 (18.03%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 18 (14.75%)
    '     File Size: 5.48 KB


    ' Module MoleculeNetworking
    ' 
    '     Function: consensus_annotation, consensus_model, createPool, getClusterSpectrum, openPool
    '               setFileInfo, setGroupInfo, setProjectContext
    ' 
    '     Sub: ScanConsensus
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Interop
Imports spectrumPool

''' <summary>
''' make clustering and networking of the lc-ms ms2 spectrum data
''' </summary>
<Package("molecule_networking")>
<RTypeExport("sample_pool", GetType(dataPool))>
Module MoleculeNetworking

    ''' <summary>
    ''' open the spectrum pool from a given resource link
    ''' </summary>
    ''' <param name="model_id">
    ''' the model id, this parameter works for open the model in the cloud service
    ''' </param>
    ''' <param name="score_overrides">
    ''' WARNING: this optional parameter will overrides the mode score 
    ''' level when this parameter has a positive numeric value in 
    ''' range ``(0,1]``. it is dangers to overrides the score parameter
    ''' in the exists model.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("openMysqlPool")>
    Public Function openPool(repo As dataPool,
                             Optional model_id As String = Nothing,
                             Optional score_overrides As Double? = Nothing,
                             Optional env As Environment = Nothing) As BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool

        If score_overrides IsNot Nothing AndAlso
            score_overrides > 0 AndAlso
            score_overrides < 1 Then

            Call env.AddMessage($"NOTICE: the score level of the spectrum graph model has been overrides to {score_overrides}!", MSG_TYPES.WRN)
        End If

        Return repo.Open(model_id:=model_id, score:=score_overrides)
    End Function

    ''' <summary>
    ''' create a new spectrum clustering data pool
    ''' </summary>
    ''' <param name="level"></param>
    ''' <param name="split">hex, max=15</param>
    ''' <returns></returns>
    <ExportAPI("createMysqlPool")>
    <RApiReturn(GetType(BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool))>
    Public Function createPool(repo As dataPool,
                               Optional level As Double = 0.9,
                               Optional split As Integer = 9,
                               Optional name As String = "no_named",
                               Optional desc As String = "no_information") As Object

        Return repo.Create(level, split:=split, name:=name, desc:=desc)
    End Function

    <ExportAPI("project_context")>
    <RApiReturn(GetType(dataPool))>
    Public Function setProjectContext(repo As dataPool, project_id As String, Optional name As String = Nothing, Optional desc As String = Nothing) As Object
        Call repo.setProjectReference(project_id, If(name, project_id), desc)
        Return repo
    End Function

    <ExportAPI("group_info")>
    <RApiReturn(GetType(dataPool))>
    Public Function setGroupInfo(repo As dataPool, group As String,
                                 Optional organism As String = "Unknown",
                                 Optional bio_sample As String = "Unknown",
                                 Optional repo_dir As String = "") As Object
        Call repo.setGroupReference(group, organism, bio_sample, repo_dir)
        Return repo
    End Function

    <ExportAPI("file_info")>
    <RApiReturn(GetType(dataPool))>
    Public Function setFileInfo(repo As dataPool, filepath As String, Optional group As String = Nothing) As Object
        Call repo.setFileReference(filepath, group)
        Return repo
    End Function

    <ExportAPI("cluster_spectrum")>
    <RApiReturn(GetType(PeakMs2))>
    Public Function getClusterSpectrum(repo As dataPool, cluster_id As String) As Object
        Return repo.ExportClusterSpectrum(UInteger.Parse(cluster_id)).ToArray
    End Function

    <ExportAPI("consensus_annotation")>
    Public Function consensus_annotation(repo As dataPool, model As clusterModels.consensus_model, cluster_id As String) As Object
        Dim cluster As clusterModels.cluster = repo.cluster _
           .where(field("model_id") = model.model_id And field("id") = cluster_id) _
           .find(Of clusterModels.cluster)

        If cluster Is Nothing Then
            Throw New ArgumentException($"Cluster with ID '{cluster_id}' not found in model '{model.model_id}'.")
        End If

        Return repo.ClusterAnnotation(model, cluster)
    End Function

    <ExportAPI("consensus_model")>
    Public Function consensus_model(repo As dataPool, model_id As String,
                                    Optional dims As Integer = 9,
                                    Optional knn As Integer = 64,
                                    Optional cutoff As Double = 0.8) As clusterModels.consensus_model

        Return Consensus.CreateModelParameters(repo, model_id, dims, knn, cutoff)
    End Function

    <ExportAPI("scan_consensus")>
    Public Sub ScanConsensus(mysql As dataPool, args As clusterModels.consensus_model,
                             Optional page_size As Integer = 1000,
                             Optional top As Integer = 30)

        Call Consensus.ScanConsensus(mysql, args, page_size, top)
    End Sub
End Module
