Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class Cluster

    ReadOnly tree As molecule_tree
    ReadOnly model As treeModel.models

    ''' <summary>
    ''' the root matrix
    ''' </summary>
    Dim root As Double()

    ''' <summary>
    ''' build model tree
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="model"></param>
    ''' <remarks>
    ''' the model referenced by <paramref name="model"/> name must be existsed 
    ''' </remarks>
    Sub New(tree As molecule_tree, model As String)
        Me.tree = tree
        Me.model = tree.models.where(field("name") = model).find(Of treeModel.models)

        If model Is Nothing Then
            Throw New InvalidProgramException($"the given model with name reference '{model}' is not found in database!")
        End If

        Me.root = getRoot()
    End Sub

    ''' <summary>
    ''' build model tree
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="model"></param>
    ''' <param name="cluster_cutoff"></param>
    ''' <param name="right_cutoff"></param>
    ''' <remarks>
    ''' this constructor function will create the model tree if the referenced <paramref name="model"/> is not found in database
    ''' </remarks>
    Sub New(tree As molecule_tree, model As String, cluster_cutoff As Double, right_cutoff As Double)
        Me.tree = tree
        Me.model = tree.models.where(field("name") = model).find(Of treeModel.models)

        If Not Me.model Is Nothing Then
            If Me.model.cluster_cutoff = cluster_cutoff AndAlso Me.model.right = right_cutoff Then
                ' is valid model parameters
            Else
                Throw New InvalidProgramException($"A model with name referenced '{model}' is already been found in database, and the parameter is different with current given parameter")
            End If
        Else
            ' create new 
            Call tree.models.add(
                field("name") = model,
                field("cluster_cutoff") = cluster_cutoff,
                field("right") = right_cutoff
            )

            Me.model = tree.models _
                .where(field("name") = model) _
                .order_by("id", desc:=True) _
                .find(Of treeModel.models)
        End If

        If Me.model Is Nothing Then
            Throw New InvalidProgramException($"create molecule tree cluster model error: {tree.models.GetLastErrorMessage}")
        Else
            Me.root = getRoot()
        End If
    End Sub

    ''' <summary>
    ''' get tree root node its strcutre data
    ''' </summary>
    ''' <returns></returns>
    Private Function getRoot() As Double()
        Dim rootNode As treeModel.tree = tree.tree _
            .where(field("model_id") = model.id) _
            .find(Of treeModel.tree)

        If Not rootNode Is Nothing Then
            root = tree.DecodeMatrix(rootNode.graph_id)
        Else
            ' try to use the first molecule as root
            Dim firstMolecule = tree.molecules.find(Of treeModel.molecules)

            If firstMolecule Is Nothing Then
                Throw New InvalidProgramException("no molecules data for create tree!")
            End If

            Dim firstGraph = tree.graph _
                .where(field("molecule_id") = firstMolecule.id) _
                .find(Of treeModel.graph)

            If firstGraph Is Nothing Then
                Throw New InvalidProgramException($"in-consist database model: missing of the graph data for the first molecule: {firstMolecule.ToString}")
            End If

            ' add tree root
            Call tree.tree.add(
                field("model_id") = model.id,
                field("parent_id") = 0,
                field("graph_id") = firstGraph.id,
                field("cosine") = 1)

            rootNode = tree.tree _
                .where(field("model_id") = model.id) _
                .order_by("id", desc:=True) _
                .find(Of treeModel.tree)

            If Not rootNode Is Nothing Then
                root = tree.DecodeMatrix(rootNode.graph_id)
            Else
                Throw New InvalidProgramException($"create root node of the tree model error: {tree.tree.GetLastErrorMessage}")
            End If
        End If

        Return root
    End Function

    ''' <summary>
    ''' scan all molecule graph data and run tree clustering
    ''' </summary>
    Public Sub BuildTree()

    End Sub
End Class
