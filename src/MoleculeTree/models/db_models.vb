Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace treeModel

Public MustInherit Class db_models : Inherits IDatabase
Protected ReadOnly m_atoms As Model
Protected ReadOnly m_graph As Model
Protected ReadOnly m_model As Model
Protected ReadOnly m_models As Model
Protected ReadOnly m_molecule_atoms As Model
Protected ReadOnly m_molecules As Model
Protected ReadOnly m_tree As Model
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_atoms = model(Of atoms)()
Me.m_graph = model(Of graph)()
Me.m_model = model(Of model)()
Me.m_models = model(Of models)()
Me.m_molecule_atoms = model(Of molecule_atoms)()
Me.m_molecules = model(Of molecules)()
Me.m_tree = model(Of tree)()
End Sub
End Class

End Namespace
