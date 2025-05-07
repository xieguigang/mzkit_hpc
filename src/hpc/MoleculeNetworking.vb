Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
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
End Module
