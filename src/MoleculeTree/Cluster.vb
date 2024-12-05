Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class Cluster

    ReadOnly tree As molecule_tree
    ''' <summary>
    ''' the root matrix
    ''' </summary>
    ReadOnly root As Double()
    ReadOnly model As treeModel.model

    Sub New(tree As molecule_tree, model As String)
        Me.tree = tree
        Me.model = tree.model.where(field("name") = model).find(Of treeModel.model)
        Me.root = getRoot()
    End Sub

    ''' <summary>
    ''' get tree root node its strcutre data
    ''' </summary>
    ''' <returns></returns>
    Private Function getRoot() As Double()

    End Function

    Public Sub BuildTree()
        ' the first
    End Sub

End Class
