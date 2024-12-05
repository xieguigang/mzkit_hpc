Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module GraphMatrix

    ReadOnly network As New NetworkByteOrderBuffer

    <Extension>
    Public Sub ResolveMatrix(tree As molecule_tree, Optional page_size As Integer = 100)
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
End Module
