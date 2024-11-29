Imports MoleculeTree.treeModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class molecule_tree : Inherits db_models

    Public ReadOnly Property atoms As Model
        Get
            Return m_atoms
        End Get
    End Property

    Public ReadOnly Property tree As Model
        Get
            Return m_tree
        End Get
    End Property

    Public ReadOnly Property graph As Model
        Get
            Return m_graph
        End Get
    End Property

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class
