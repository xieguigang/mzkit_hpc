Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace clusterModels

Public MustInherit Class db_models : Inherits IDatabase
Protected ReadOnly m_cluster As TableModel(Of cluster)
Protected ReadOnly m_cluster_data As TableModel(Of cluster_data)
Protected ReadOnly m_cluster_graph As TableModel(Of cluster_graph)
Protected ReadOnly m_cluster_tree As TableModel(Of cluster_tree)
Protected ReadOnly m_consensus_model As TableModel(Of consensus_model)
Protected ReadOnly m_consensus_spectrum As TableModel(Of consensus_spectrum)
Protected ReadOnly m_graph_model As TableModel(Of graph_model)
Protected ReadOnly m_metadata As TableModel(Of metadata)
Protected ReadOnly m_project As TableModel(Of project)
Protected ReadOnly m_rawfiles As TableModel(Of rawfiles)
Protected ReadOnly m_sample_groups As TableModel(Of sample_groups)
Protected ReadOnly m_spectrum_pool As TableModel(Of spectrum_pool)
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_cluster = model(Of cluster)()
Me.m_cluster_data = model(Of cluster_data)()
Me.m_cluster_graph = model(Of cluster_graph)()
Me.m_cluster_tree = model(Of cluster_tree)()
Me.m_consensus_model = model(Of consensus_model)()
Me.m_consensus_spectrum = model(Of consensus_spectrum)()
Me.m_graph_model = model(Of graph_model)()
Me.m_metadata = model(Of metadata)()
Me.m_project = model(Of project)()
Me.m_rawfiles = model(Of rawfiles)()
Me.m_sample_groups = model(Of sample_groups)()
Me.m_spectrum_pool = model(Of spectrum_pool)()
End Sub
End Class

End Namespace
