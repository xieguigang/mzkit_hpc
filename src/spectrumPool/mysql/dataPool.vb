Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports spectrumPool.clusterModels

Public Class dataPool : Inherits clusterModels.db_models

    Public ReadOnly Property spectrum_pool As TableModel(Of spectrum_pool)
        Get
            Return m_spectrum_pool
        End Get
    End Property

    Public ReadOnly Property cluster As TableModel(Of cluster)
        Get
            Return m_cluster
        End Get
    End Property

    Public ReadOnly Property cluster_data As TableModel(Of cluster_data)
        Get
            Return m_cluster_data
        End Get
    End Property

    Public ReadOnly Property cluster_graph As TableModel(Of cluster_graph)
        Get
            Return m_cluster_graph
        End Get
    End Property

    Public ReadOnly Property cluster_tree As TableModel(Of cluster_tree)
        Get
            Return m_cluster_tree
        End Get
    End Property

    Public ReadOnly Property consensus_model As TableModel(Of consensus_model)
        Get
            Return m_consensus_model
        End Get
    End Property

    Public ReadOnly Property consensus_spectrum As TableModel(Of consensus_spectrum)
        Get
            Return m_consensus_spectrum
        End Get
    End Property

    Public ReadOnly Property graph_model As TableModel(Of graph_model)
        Get
            Return m_graph_model
        End Get
    End Property

    Public ReadOnly Property metadata As TableModel(Of metadata)
        Get
            Return m_metadata
        End Get
    End Property

    Public ReadOnly Property project As TableModel(Of project)
        Get
            Return m_project
        End Get
    End Property

    Public ReadOnly Property rawfiles As TableModel(Of rawfiles)
        Get
            Return m_rawfiles
        End Get
    End Property

    Public ReadOnly Property sample_groups As TableModel(Of sample_groups)
        Get
            Return m_sample_groups
        End Get
    End Property

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class
