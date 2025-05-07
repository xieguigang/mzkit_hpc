Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.Xml
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Net.Http
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class mysqlRepository : Inherits MetadataProxy

    ''' <summary>
    ''' metadata cache of current cluster node
    ''' </summary>
    Dim local_cache As Dictionary(Of String, Metadata)
    Dim hash_index As String
    Dim cluster_data As clusterModels.cluster
    Dim model_id As String
    Dim fs As mysqlFs

    Dim m_depth As Integer = 0
    Dim m_rootId As String = Nothing

    ''' <summary>
    ''' the cluster id in the database
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property guid As UInteger
        Get
            Return cluster_data.id
        End Get
    End Property

    Default Public Overrides ReadOnly Property GetById(id As String) As Metadata
        Get
            If Not local_cache.ContainsKey(id) Then
                Call local_cache.Add(id, GetMetadataByHashKey(id))
            End If

            Return local_cache.TryGetValue(id)
        End Get
    End Property

    Public Overrides ReadOnly Property AllClusterMembers As IEnumerable(Of Metadata)
        Get
            Return FetchClusterData(hash_index, model_id)
        End Get
    End Property

    ''' <summary>
    ''' the root spectrum id of current cluster
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property RootId As String
        Get
            If m_rootId.StringEmpty Then
                Dim root = cluster_data.root

                If root = 0 Then
                    Return Nothing
                Else
                    m_rootId = CStr(root)
                End If
            End If

            Return m_rootId
        End Get
    End Property

    Public Overrides ReadOnly Property Depth As Integer
        Get
            Return m_depth
        End Get
    End Property

    ''' <summary>
    ''' open existed or create new cluster node
    ''' </summary>
    ''' <param name="http"></param>
    ''' <param name="path"></param>
    ''' <param name="parentId"></param>
    Sub New(http As mysqlFs, path As String, parentId As Long)
        Me.New(http)
        Me.hash_index = HttpTreeFs.ClusterHashIndex(path)

        cluster_data = http.mysql.cluster _
            .where(field("model_id") = http.model_id,
                   field("hash_index") = hash_index) _
            .find(Of clusterModels.cluster)

        If cluster_data Is Nothing Then
            Dim split As String() = path.Split("/"c)

            ' create new?
            http.mysql.cluster.add(
                field("model_id") = http.model_id,
                field("key") = path.BaseName,
                field("parent_id") = parentId,
                field("n_childs") = 0,
                field("n_spectrum") = 0,
                field("root") = 0,
                field("hash_index") = hash_index,
                field("depth") = split.Length - 1
            )

            cluster_data = http.mysql.cluster _
                .where(field("model_id") = http.model_id,
                       field("hash_index") = hash_index) _
                .order_by("id", desc:=True) _
                .find(Of clusterModels.cluster)
        End If

        If cluster_data Is Nothing Then
            Throw New InvalidProgramException("initialize of the cluster data error!")
        Else
            Me.m_depth = cluster_data.depth
        End If
    End Sub

    ''' <summary>
    ''' common pathway for initialize the cluster node data pool
    ''' </summary>
    ''' <param name="fs"></param>
    Private Sub New(fs As mysqlFs)
        Me.model_id = fs.model.id.ToString
        Me.local_cache = New Dictionary(Of String, Metadata)
        Me.fs = fs
    End Sub

    ''' <summary>
    ''' open existsed cluster node
    ''' </summary>
    ''' <param name="http"></param>
    ''' <param name="cluster_id"></param>
    Sub New(http As mysqlFs, cluster_id As UInteger)
        Me.New(http)

        Dim obj As clusterModels.cluster = http.mysql.cluster _
            .where(field("id") = cluster_id) _
            .find(Of clusterModels.cluster)

        If obj Is Nothing Then
            Throw New MissingMemberException($"No cluster which its id is: '{cluster_id}'!")
        Else
            Me.cluster_data = obj
            Me.m_depth = cluster_data.depth
            Me.hash_index = cluster_data.hash_index
        End If
    End Sub

    ''' <summary>
    ''' get metabolite ion data from a specific spectrum cluster
    ''' </summary>
    ''' <param name="url_get"></param>
    ''' <param name="hash_index"></param>
    ''' <param name="model_id"></param>
    ''' <returns></returns>
    Public Iterator Function FetchClusterData(hash_index As String, model_id As String) As IEnumerable(Of Metadata)
        'Dim url As String = $"{url_get}?id={hash_index}&is_cluster=true&model_id={model_id}"
        'Dim json As String = url.GET
        'Dim list As Restful = Restful.ParseJSON(json)

        'If list.code <> 0 Then
        '    Return
        'End If

        'Dim info As JavaScriptObject = list.info
        'Dim array As Array = info!metabolites

        'For i As Integer = 0 To array.Length - 1
        '    Yield ParseMetadata(array(i))
        'Next
    End Function

    Public Function GetMetadataByHashKey(hash As String) As Metadata
        Dim q = fs.mysql.metadata _
            .where(field("model_id") = model_id,
                   field("hashcode") = hash) _
            .find(Of clusterModels.metadata)

        If q Is Nothing Then
            Return Nothing
        Else
            Return New Metadata With {
                .adducts = q.adducts,
                .biodeep_id = q.xref_id,
                .formula = q.formula,
                .guid = q.hashcode,
                .instrument = q.instrument,
                .intensity = q.intensity,
                .mz = q.mz,
                .name = q.name,
                .project = q.project,
                .organism = q.organism,
                .rt = q.rt,
                .sample_source = q.biosample,
                .source_file = q.filename,
                .block = New BufferRegion(q.spectral_id, 0)
            }
        End If
    End Function

    Public Overrides Sub Add(id As String, metadata As Metadata)
        Call fs.mysql.metadata.add(
            field("hashcode") = id,
            field("mz") = metadata.mz,
            field("rt") = metadata.rt,
            field("intensity") = metadata.intensity,
            field("filename") = metadata.source_file,
            field("cluster_id") = cluster_data.id,
            field("rawfile") = fs.mysql.getFileReference(metadata.source_file),
            field("spectral_id") = metadata.block.position,
            field("model_id") = model_id,
            field("project_id") = If(fs.mysql.project_data Is Nothing, 0, fs.mysql.project_data.id),
            field("project") = metadata.project,
            field("biosample") = metadata.sample_source,
            field("organism") = metadata.organism,
            field("xref_id") = metadata.biodeep_id,
            field("name") = metadata.name,
            field("formula") = metadata.formula,
            field("adducts") = metadata.adducts,
            field("instrument") = metadata.instrument
        )

        local_cache(id) = metadata
    End Sub

    Public Overrides Sub Add(id As String, score As Double, align As AlignmentOutput, pval As Double)
        Dim metadata As Metadata = local_cache(id)
        Dim data As New List(Of FieldAssert)
        Dim meta_id = fs.mysql.metadata.where(field("hashcode") = metadata.guid, field("model_id") = model_id).find(Of clusterModels.metadata)

        If align Is Nothing Then
            ' config for root
            Call data.Add("n_hits", 0)
            Call data.Add("consensus", "*")
            Call data.Add("forward", 1)
            Call data.Add("reverse", 1)
            Call data.Add("jaccard", 1)
            Call data.Add("entropy", 1)
        Else
            ' member spectrum align with root spectrum 
            ' of current cluster
            Dim consensus As Double() = align.alignments _
                .Where(Function(a) a.query > 0 AndAlso a.ref > 0) _
                .Select(Function(a) a.mz) _
                .ToArray

            Call data.Add("n_hits", consensus.Length)
            Call data.Add("consensus", consensus.Select(AddressOf NetworkByteOrderBitConvertor.GetBytes).IteratesALL.ToBase64String)
            Call data.Add("forward", align.forward)
            Call data.Add("reverse", align.reverse)
            Call data.Add("jaccard", align.jaccard)
            Call data.Add("entropy", align.entropy)
        End If

        Call data.Add("p_value", pval)
        Call data.Add("score", score)
        Call data.Add("spectral_id", metadata.block.position)
        Call data.Add("cluster_id", Me.guid)
        Call data.Add("model_id", model_id)
        Call data.Add("metadata_id", meta_id.id)

        Call fs.mysql.cluster_data.add(data.ToArray)
    End Sub

    Public Overrides Sub SetRootId(hashcode As String)
        m_rootId = hashcode


    End Sub

    Public Overrides Function HasGuid(id As String) As Boolean
        Return Me(id) IsNot Nothing
    End Function
End Class
