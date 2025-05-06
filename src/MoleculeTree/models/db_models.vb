Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace treeModel

Public MustInherit Class db_models : Inherits IDatabase
Protected ReadOnly m_atoms As TableModel(Of atoms)
Protected ReadOnly m_graph As TableModel(Of graph)
Protected ReadOnly m_models As TableModel(Of models)
Protected ReadOnly m_molecule_atoms As TableModel(Of molecule_atoms)
Protected ReadOnly m_molecules As TableModel(Of molecules)
Protected ReadOnly m_tree As TableModel(Of tree)
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_atoms = model(Of atoms)()
Me.m_graph = model(Of graph)()
Me.m_models = model(Of models)()
Me.m_molecule_atoms = model(Of molecule_atoms)()
Me.m_molecules = model(Of molecules)()
Me.m_tree = model(Of tree)()
End Sub
End Class

End Namespace
