#Region "Microsoft.VisualBasic::d5bc4e2010ed35044383a5e887d75c71, Rscript\Library\mzkit_hpc\src\MoleculeTree\Cluster.vb"

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

    '   Total Lines: 343
    '    Code Lines: 227 (66.18%)
    ' Comment Lines: 64 (18.66%)
    '    - Xml Docs: 60.94%
    ' 
    '   Blank Lines: 52 (15.16%)
    '     File Size: 12.73 KB


    ' Class Cluster
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    '     Function: CreateNode, getRoot, Search
    ' 
    '     Sub: BuildTree, BuildTreePage
    ' 
    ' /********************************************************************************/

#End Region

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
            field("cosine") = 1,
            field("jaccard") = 1,
            field("t") = 1,
            field("pvalue") = 0,
            field("left") = 0,
            field("right") = 0)

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
            Else
                Call VBDebugger.EchoLine($"processing data page_{page - 1}")
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

    ''' <summary>
    ''' make molecule structure similarity search 
    ''' </summary>
    ''' <param name="u"></param>
    ''' <returns></returns>
    Public Iterator Function Search(u As Double()) As IEnumerable(Of (molecule As treeModel.molecules, score As Double))
        Dim root As treeModel.tree = Me.root
        Dim max As Double = u.Max

        If max <> 0.0 Then
            u = SIMD.Divide.f64_op_divide_f64_scalar(u, max)
        End If

        Do While True
            Dim v As Double() = tree.DecodeMatrix(root.graph_id)

            max = v.Max

            If max <> 0.0 Then
                v = SIMD.Divide.f64_op_divide_f64_scalar(v, max)
            End If

            ' compares with current
            Dim cos As Double = SSM_SIMD(u, v)
            Dim jac As Double = LinearAlgebra.JaccardIndex(u, v)
            Dim score As Double = cos * jac

            If score > model.cluster_cutoff Then
                ' populate cluster search result and break the loop
                Dim cluster = tree.tree.where(field("parent_id") = root.id).select(Of treeModel.tree) _
                    .Where(Function(a)
                               ' filter [left,right]
                               Return a.parent_id <> root.left AndAlso
                                      a.parent_id <> root.right
                           End Function) _
                    .ToArray

                For Each item In cluster _
                    .Select(Function(t)
                                Dim molecule_id As UInteger = 0
                                Dim v1 As Double() = tree.DecodeMatrix(t.graph_id, molecule_id)
                                Dim max1 = v1.Max

                                If max1 <> 0.0 Then
                                    v1 = SIMD.Divide.f64_op_divide_f64_scalar(v1, max1)
                                End If

                                Dim cos1 As Double = SSM_SIMD(u, v1)
                                Dim jac1 As Double = LinearAlgebra.JaccardIndex(u, v1)
                                Dim score1 As Double = cos1 * jac1

                                Return (tree.molecules.where(field("id") = molecule_id).find(Of treeModel.molecules), score1)
                            End Function) _
                    .OrderByDescending(Function(a) a.score1)

                    Yield item
                Next

                Exit Do
            ElseIf score > model.right Then
                ' visit right
                If root.right = 0 Then
                    ' reach the leaf of tree, break search tree 
                    Exit Do
                Else
                    root = tree.tree.where(field("id") = root.right).find(Of treeModel.tree)
                End If
            Else
                ' break search tree on left
                Exit Do
            End If
        Loop
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="page"></param>
    ''' <remarks>
    ''' the graph vector data has been normalized to [0,1]
    ''' </remarks>
    Private Sub BuildTreePage(page As treeModel.graph)
        Dim root As treeModel.tree = Me.root
        Dim u As Double() = tree.DecodeMatrix(page.id)
        Dim max As Double = u.Max

        If max <> 0.0 Then
            u = SIMD.Divide.f64_op_divide_f64_scalar(u, max)
        Else
            u(0) += randf(0.00000001, 0.000001)
        End If

        Do While True
            Dim v As Double() = tree.DecodeMatrix(root.graph_id)
            ' compares with current
            Dim cos As Double = SSM_SIMD(u, v)
            Dim jac As Double = LinearAlgebra.JaccardIndex(u, v)

            max = v.Max

            If max <> 0.0 Then
                v = SIMD.Divide.f64_op_divide_f64_scalar(v, max)
            Else
                ' 20241206 for avoid the un-expected constant value error
                v(0) += randf(0.00000001, 0.000001)
            End If

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
                    root.right = CreateNode(root, page, cos, jac, test.TestValue, pval)

                    Call tree.tree.where(
                        field("id") = root.id
                    ).save(field("right") = root.right)

                    Exit Do
                Else
                    root = tree.tree.where(field("id") = root.right).find(Of treeModel.tree)
                End If
            Else
                ' visit left
                If root.left = 0 Then
                    ' no left
                    ' create node as left based on current graph data
                    root.left = CreateNode(root, page, cos, jac, test.TestValue, pval)

                    Call tree.tree.where(
                        field("id") = root.id
                    ).save(field("left") = root.left)

                    Exit Do
                Else
                    root = tree.tree.where(field("id") = root.left).find(Of treeModel.tree)
                End If
            End If
        Loop
    End Sub

    Private Function CreateNode(root As treeModel.tree, page As treeModel.graph,
                                cos As Double,
                                jac As Double,
                                t As Double,
                                pval As Double) As UInteger
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

