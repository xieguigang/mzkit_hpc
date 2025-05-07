Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports spectrumPool.clusterModels

Public Class dataPool : Inherits clusterModels.db_models

    Public ReadOnly Property spectrum_pool As TableModel(Of spectrum_pool)
        Get
            Return m_spectrum_pool
        End Get
    End Property

    Public ReadOnly Property cluster As TableModel(Of cluster)
        Get
            Return m_cluster
        End Get
    End Property

    Public ReadOnly Property cluster_data As TableModel(Of cluster_data)
        Get
            Return m_cluster_data
        End Get
    End Property

    Public ReadOnly Property cluster_graph As TableModel(Of cluster_graph)
        Get
            Return m_cluster_graph
        End Get
    End Property

    Public ReadOnly Property cluster_tree As TableModel(Of cluster_tree)
        Get
            Return m_cluster_tree
        End Get
    End Property

    Public ReadOnly Property consensus_model As TableModel(Of consensus_model)
        Get
            Return m_consensus_model
        End Get
    End Property

    Public ReadOnly Property consensus_spectrum As TableModel(Of consensus_spectrum)
        Get
            Return m_consensus_spectrum
        End Get
    End Property

    Public ReadOnly Property graph_model As TableModel(Of graph_model)
        Get
            Return m_graph_model
        End Get
    End Property

    Public ReadOnly Property metadata As TableModel(Of clusterModels.metadata)
        Get
            Return m_metadata
        End Get
    End Property

    Public ReadOnly Property project As TableModel(Of project)
        Get
            Return m_project
        End Get
    End Property

    Public ReadOnly Property rawfiles As TableModel(Of rawfiles)
        Get
            Return m_rawfiles
        End Get
    End Property

    Public ReadOnly Property sample_groups As TableModel(Of sample_groups)
        Get
            Return m_sample_groups
        End Get
    End Property

    Public ReadOnly Property project_data As clusterModels.project

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub

    Public Sub setProjectReference(project_id As String, name As String, desc As String)
        _project_data = project _
            .where(field("project_id") = project_id) _
            .find(Of clusterModels.project)

        If project_data Is Nothing Then
            project.add(
                field("project_id") = project_id,
                field("project_name") = name,
                field("note") = desc,
                field("sample_groups") = 0,
                field("sample_files") = 0
            )
        Else
            project.where(field("id") = project_data.id).save(
                field("project_name") = name,
                field("note") = desc
            )
        End If
    End Sub

    Public Sub setGroupReference(group As String, organism As String, bio_sample As String, repo_dir As String)
        Dim q As FieldAssert()

        If project_data Is Nothing Then
            q = {field("group_name") = group}
        Else
            q = {field("group_name") = group, field("project_id") = project_data.id}
        End If

        Dim group_data As clusterModels.sample_groups = sample_groups.where(q).find(Of clusterModels.sample_groups)

        If group_data Is Nothing Then
            sample_groups.add(q)
            group_data = sample_groups.where(q).find(Of clusterModels.sample_groups)
        End If

        If group_data IsNot Nothing Then
            sample_groups.where(field("id") = group_data.id).save(
                field("organism") = organism,
                field("bio_sample") = bio_sample,
                field("repo_path") = repo_dir
            )
        End If
    End Sub

    Public Function getGroupID(group As String) As clusterModels.sample_groups
        Dim q As FieldAssert()

        If group.StringEmpty Then
            Return Nothing
        End If

        If project_data Is Nothing Then
            q = {field("group_name") = group}
        Else
            q = {field("group_name") = group, field("project_id") = project_data.id}
        End If

        Return sample_groups.where(q).find(Of clusterModels.sample_groups)
    End Function

    Public Sub setFileReference(filepath As String, Optional sample_group As String = Nothing)
        Dim ref As UInteger = getFileReference(filepath.BaseName)

        If ref = 0 Then
            Dim groupId As clusterModels.sample_groups = getGroupID(sample_group)

            ' create new
            rawfiles.add(
                field("filename") = filepath.BaseName,
                field("size_bytes") = filepath.FileLength,
                field("project_id") = If(project_data Is Nothing, 0, project_data.id),
                field("sample_group") = If(groupId Is Nothing, 0, groupId.id)
            )
        End If
    End Sub

    Public Function getFileReference(filename As String) As UInteger
        Dim q As FieldAssert()

        If project_data Is Nothing Then
            q = {field("filename") = filename}
        Else
            q = {field("filename") = filename, field("project_id") = project_data.id}
        End If

        Dim file As clusterModels.rawfiles = rawfiles.where(q).find(Of clusterModels.rawfiles)

        If file Is Nothing Then
            Return 0
        Else
            Return file.id
        End If
    End Function

    Private Overloads Function CreateModel(name$, desc$, level As Double, split As Integer) As UInteger
        Dim args As New Dictionary(Of String, String) From {
            {NameOf(level), level},
            {NameOf(split), split}
        }

        Call graph_model.add(
            field("name") = name,
            field("parameters") = args.GetJson,
            field("description") = desc,
            field("flag") = 0
        )

        Dim ctor As clusterModels.graph_model = graph_model _
            .where(field("name") = name) _
            .order_by("id", desc:=True) _
            .find(Of clusterModels.graph_model)

        If ctor Is Nothing Then
            Throw New InvalidOperationException("Create spectrum cluster model error: " & graph_model.GetLastErrorMessage)
        Else
            Return ctor.id
        End If
    End Function

    ''' <summary>
    ''' open root folder
    ''' </summary>
    ''' <param name="level"></param>
    ''' <param name="split"></param>
    ''' <returns></returns>
    Public Function Create(Optional level As Double = 0.85,
                           Optional split As Integer = 3,
                           Optional name As String = "no_named",
                           Optional desc As String = "no_information") As BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool

        Dim fs As New mysqlFs(Me, CreateModel(name, desc, level, split))
        Dim pool As New BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool(fs, "/")

        Call fs.SetLevel(level, split)
        Call fs.SetScore(0.3, 0.05)

        Return pool
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="model_id"></param>
    ''' <param name="score">
    ''' WARNING: this optional parameter will overrides the mode score 
    ''' level when this parameter has a positive numeric value in 
    ''' range ``(0,1]``.
    ''' </param>
    ''' <returns></returns>
    Public Function Open(Optional model_id As String = Nothing, Optional score As Double? = Nothing) As BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool
        Dim fs As New mysqlFs(Me, model_id)
        Dim pool As New BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool(fs, "/")

        If score IsNot Nothing AndAlso
            score > 0 AndAlso
            score < 1 Then

            Call fs.SetLevel(score, fs.split)
        Else
            Call fs.SetLevel(fs.level, fs.split)
        End If

        Call fs.SetScore(0.3, 0.05)

        Return pool
    End Function
End Class
