Imports System.Data
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Reflection.DbAttributes

Public Module GraphMatrix

    ReadOnly network As New NetworkByteOrderBuffer

    <Extension>
    Public Sub ResolveMatrix(tree As molecule_tree,
                             Optional page_size As Integer = 100,
                             Optional fast_check As Boolean = False)

        Dim page As Integer = 1
        Dim atoms As treeModel.atoms() = tree.atoms.select(Of treeModel.atoms) _
            .OrderBy(Function(a) a.unique_id) _
            .ToArray
        Dim atom_id As String() = atoms.Select(Function(a) a.id) _
            .AsCharacter _
            .ToArray

        Do While True
            Dim offset As UInteger = (page - 1) * page_size
            Dim pull As treeModel.graph() = tree.graph.limit(offset, page_size).select(Of treeModel.graph)()

            If pull.IsNullOrEmpty Then
                Exit Do
            Else
                page += 1
            End If

            For Each json As treeModel.graph In TqdmWrapper.Wrap(pull)
                Dim keys As SparseGraph.Edge() = json.graph.LoadJSON(Of SparseGraph.Edge())
                Dim graph As New SparseGraph(keys)
                Dim matrix As NumericMatrix = graph.CreateMatrix(atom_id)
                Dim v As Double() = matrix.RowPackedCopy
                Dim bytes As String = network.Base64String(v)

                ' make matrix data updates
                Call tree.graph.where(field("id") = json.id).save(
                    field("matrix") = bytes
                )
            Next
        Loop
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="graph_id">
    ''' id reference to the graph data table
    ''' </param>
    ''' <returns></returns>
    <Extension>
    Public Function DecodeMatrix(tree As molecule_tree, graph_id As UInteger) As Double()
        Dim graph As treeModel.graph = tree.graph.where(field("id") = graph_id).find(Of treeModel.graph)
        Dim v As Double() = network.ParseDouble(graph.matrix)
        Return v
    End Function

    <Extension>
    Public Function FetchTree(tree As molecule_tree, model As String, Optional page_size As Integer = 1000) As NetworkGraph
        Dim m As treeModel.models = tree.models.where(field("name") = model).find(Of treeModel.models)
        Dim g As New NetworkGraph
        Dim page As Integer = 1
        Dim offset As UInteger = 0

        If m Is Nothing Then
            Throw New MissingPrimaryKeyException($"there is no cluster tree mode which is named '{model}' inside database!")
        Else
            Call g.CreateNode("0", New NodeData With {
                .label = "root",
                .origID = "root"
            })
        End If

        Dim data As TreeNode()

        Do While True
            offset = (page - 1) * page_size
            page += 1
            data = tree.tree _
                .left_join("graph").on(field("graph.id") = field("graph_id")) _
                .left_join("molecules").on(field("molecule_id") = field("molecules.id")) _
                .where(field("model_id") = m.id) _
                .limit(offset, page_size) _
                .select(Of TreeNode)(
                    "tree.id",
                    "parent_id",
                    "cosine",
                    "jaccard",
                    "t",
                    "pvalue",
                    "`left`",
                    "`right`",
                    "graph.smiles",
                    "db_xref",
                    "name",
                    "formula",
                    "exact_mass")

            If data.IsNullOrEmpty Then
                Exit Do
            Else
                Call VBDebugger.EchoLine($"fetch data -> offset[{offset},{offset + page_size}]!")
            End If

            For Each i As TreeNode In data
                If g.GetElementByID(i.id.ToString) Is Nothing Then
                    Call g.CreateNode(i.id, New NodeData With {
                        .label = i.name,
                        .mass = i.exact_mass,
                        .origID = i.db_xref,
                        .Properties = New Dictionary(Of String, String) From {
                            {"formula", i.formula},
                            {"exact_mass", i.exact_mass},
                            {"smiles", i.smiles},
                            {"parent_id", i.parent_id},
                            {"left", i.left},
                            {"right", i.right}
                        }
                    })
                End If
            Next

            For Each i As TreeNode In data
                Dim u = g.GetElementByID(i.parent_id.ToString)
                Dim v = g.GetElementByID(i.id.ToString)

                If Not g.GetEdges(u, v).Any Then
                    Call g.CreateEdge(u, v, i.cosine * i.jaccard, New EdgeData With {
                        .Properties = New Dictionary(Of String, String) From {
                            {"cosine", i.cosine},
                            {"jaccard", i.jaccard},
                            {"t", i.t},
                            {"pvalue", i.pvalue}
                        }
                    })
                End If
            Next
        Loop

        Return g
    End Function
End Module

Public Class TreeNode

    <DatabaseField> Public Property id As UInteger
    <DatabaseField> Public Property parent_id As UInteger
    <DatabaseField> Public Property cosine As Double
    <DatabaseField> Public Property jaccard As Double
    <DatabaseField> Public Property t As Double
    <DatabaseField> Public Property pvalue As Double
    <DatabaseField> Public Property left As UInteger
    <DatabaseField> Public Property right As UInteger
    <DatabaseField> Public Property smiles As String
    <DatabaseField> Public Property db_xref As String
    <DatabaseField> Public Property name As String
    <DatabaseField> Public Property formula As String
    <DatabaseField> Public Property exact_mass As Double

End Class