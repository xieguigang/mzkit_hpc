Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.Xml
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.My.JavaScript

Public Class mysqlRepository : Inherits MetadataProxy

    ''' <summary>
    ''' metadata cache of current cluster node
    ''' </summary>
    Dim local_cache As Dictionary(Of String, Metadata)
    Dim hash_index As String
    Dim cluster_data As JavaScriptObject
    Dim model_id As String

    Dim m_depth As Integer = 0
    Dim m_rootId As String = Nothing


    ''' <summary>
    ''' the cluster id in the database
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property guid As Long
        Get
            Return Val(cluster_data!id)
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
            Return FetchClusterData(url_get, hash_index, model_id)
        End Get
    End Property

    ''' <summary>
    ''' the root spectrum id of current cluster
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property RootId As String
        Get
            If m_rootId.StringEmpty Then
                Dim root = cluster_data!root

                If root Is Nothing Then
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


    End Sub

    ''' <summary>
    ''' common pathway for initialize the cluster node data pool
    ''' </summary>
    ''' <param name="http"></param>
    Private Sub New(http As mysqlFs)
        Me.model_id = http.model_id.id.ToString
        Me.local_cache = New Dictionary(Of String, Metadata)
    End Sub

    ''' <summary>
    ''' open existsed cluster node
    ''' </summary>
    ''' <param name="http"></param>
    ''' <param name="cluster_id"></param>
    Sub New(http As mysqlFs, cluster_id As Integer)
        Me.New(http)

        Dim url As String = $"{http.base}/get/cluster/?id={cluster_id}&model_id={http.model_id}"
        Dim json As String = url.GET
        Dim obj As Restful = Restful.ParseJSON(json)

        If obj.code <> 0 Then
            Throw New MissingMemberException($"No cluster which its id is: '{cluster_id}'!")
        Else
            Me.cluster_data = obj.info
            Me.m_depth = Val((cluster_data!depth).ToString)
            Me.hash_index = CStr(cluster_data!hash_index)
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
        Dim url As String = $"{url_get}?id={hash_index}&is_cluster=true&model_id={model_id}"
        Dim json As String = url.GET
        Dim list As Restful = Restful.ParseJSON(json)

        If list.code <> 0 Then
            Return
        End If

        Dim info As JavaScriptObject = list.info
        Dim array As Array = info!metabolites

        For i As Integer = 0 To array.Length - 1
            Yield ParseMetadata(array(i))
        Next
    End Function

    Public Function GetMetadataByHashKey(hash As String) As Metadata
        Dim url As String = $"{url_get}?id={hash}&model_id={model_id}&cluster_id={guid}"
        Dim json As String = url.GET
        Dim obj As Restful = Restful.ParseJSON(json)

        If obj.code <> 0 Then
            Call VBDebugger.EchoLine(obj.debug)
            Return Nothing
        Else
            Return ParseMetadata(fetch:=obj.info)
        End If
    End Function

    Public Overrides Sub Add(id As String, metadata As Metadata)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Add(id As String, score As Double, align As AlignmentOutput, pval As Double)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub SetRootId(hashcode As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function HasGuid(id As String) As Boolean
        Return Me(id) IsNot Nothing
    End Function
End Class
