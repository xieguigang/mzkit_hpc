#Region "Microsoft.VisualBasic::182844c8351266bc468b311f3704436d, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\dataPool.vb"

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

    '   Total Lines: 260
    '    Code Lines: 197 (75.77%)
    ' Comment Lines: 17 (6.54%)
    '    - Xml Docs: 88.24%
    ' 
    '   Blank Lines: 46 (17.69%)
    '     File Size: 8.70 KB


    ' Class dataPool
    ' 
    '     Properties: cluster, cluster_data, cluster_graph, cluster_tree, consensus_model
    '                 consensus_spectrum, graph_model, metadata, project, project_data
    '                 rawfiles, sample_groups, spectrum_pool
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: Create, CreateModel, getFileReference, getGroupID, Open
    ' 
    '     Sub: setFileReference, setGroupReference, setProjectReference
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Linq
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

            _project_data = project _
                .where(field("project_id") = project_id) _
                .order_by("id", desc:=True) _
                .find(Of clusterModels.project)
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
            sample_groups.add(q.JoinIterates({field("organism") = organism,
                field("bio_samples") = bio_sample,
                field("repo_path") = repo_dir}).ToArray)
        Else
            sample_groups.where(field("id") = group_data.id).save(
                field("organism") = organism,
                field("bio_samples") = bio_sample,
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
