Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class Cluster

    ReadOnly tree As molecule_tree
    ReadOnly model As treeModel.models

    ''' <summary>
    ''' the molecule tree root node
    ''' </summary>
    ReadOnly root As treeModel.tree

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

        root = getRoot()
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
            root = getRoot()
        End If
    End Sub

    ''' <summary>
    ''' get tree root node its strcutre data
    ''' </summary>
    ''' <returns></returns>
    Private Function getRoot() As treeModel.tree
        Dim rootNode As treeModel.tree = tree.tree _
            .where(field("model_id") = model.id) _
            .find(Of treeModel.tree)

        If Not rootNode Is Nothing Then
            Return rootNode
        End If

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
            Return rootNode
        Else
            Throw New InvalidProgramException($"create root node of the tree model error: {tree.tree.GetLastErrorMessage}")
        End If
    End Function

    ''' <summary>
    ''' scan all molecule graph data and run tree clustering
    ''' </summary>
    Public Sub BuildTree(Optional page_size As Integer = 100)
        Dim offset As UInteger = 0
        Dim page As Integer = 1
        Dim molecules As treeModel.molecules()

        Do While True
            offset = (page - 1) * page_size
            molecules = tree.molecules.limit(offset, page_size).select(Of treeModel.molecules)
            page += 1

            If molecules.IsNullOrEmpty Then
                Exit Do
            End If

            For Each molecule As treeModel.molecules In TqdmWrapper.Wrap(molecules)
                For Each graph As treeModel.graph In tree.graph _
                    .where(field("molecule_id") = molecule.id) _
                    .select(Of treeModel.graph)

                    ' check of the graph is already been existed
                    If tree.tree.where(
                            field("model_id") = model.id,
                            field("graph_id") = graph.id
                        ) _
                        .find(Of treeModel.tree) Is Nothing Then

                        Call BuildTreePage(graph)
                    End If
                Next
            Next
        Loop
    End Sub

    Private Sub BuildTreePage(page As treeModel.graph)
        Dim root As treeModel.tree = Me.root
        Dim u As Double() = tree.DecodeMatrix(page.id)

        u = SIMD.Divide.f64_op_divide_f64_scalar(u, u.Max)

        Do While True
            Dim v As Double() = tree.DecodeMatrix(root.graph_id)
            ' compares with current
            Dim cos As Double = SSM_SIMD(u, v)
            Dim jac As Double = LinearAlgebra.JaccardIndex(u, v)

            v = SIMD.Divide.f64_op_divide_f64_scalar(v, v.Max)

            ' u is not equsls to v
            ' but we check similarity at here
            ' so pvalue is 1 - t-test pvalue
            Dim test = t.Test(u, v, alternative:=Hypothesis.TwoSided)
            Dim pval As Double = 1 - test.Pvalue
            Dim score As Double = cos * jac

            If score > model.cluster_cutoff Then
                ' is member of current node 
                ' create tree node and exit
                Call CreateNode(root, page, cos, jac, test.TestValue, pval)
                Exit Do
            ElseIf score > model.right Then
                ' visit right
                If root.right = 0 Then
                    ' no right
                    ' create node as right based on current graph data
                    Call tree.tree.where(
                        field("id") = root.id
                    ).save(field("right") = CreateNode(root, page, cos, jac, test.TestValue, pval))

                    Exit Do
                Else
                    root = tree.tree.where(field("id") = root.right).find(Of treeModel.tree)
                End If
            Else
                ' visit left
                If root.left = 0 Then
                    ' no left
                    ' create node as left based on current graph data
                    Call tree.tree.where(
                        field("id") = root.id
                    ).save(field("left") = CreateNode(root, page, cos, jac, test.TestValue, pval))

                    Exit Do
                Else
                    root = tree.tree.where(field("id") = root.left).find(Of treeModel.tree)
                End If
            End If
        Loop
    End Sub

    Private Function CreateNode(root As treeModel.tree, page As treeModel.graph, cos As Double, jac As Double, t As Double, pval As Double) As UInteger
        Call tree.tree.add(
            field("model_id") = model.id,
            field("parent_id") = root.id,
            field("graph_id") = page.id,
            field("cosine") = cos,
            field("jaccard") = jac,
            field("t") = t,
            field("pvalue") = pval,
            field("left") = 0,
            field("right") = 0
        )

        root = tree.tree.where(
                field("model_id") = model.id,
                field("graph_id") = page.id
            ).find(Of treeModel.tree)

        Return root.id
    End Function
End Class
