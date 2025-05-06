Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
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

    Public ReadOnly Property metadata As TableModel(Of clusterModels.metadata)
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

    Private Function CreateModel(name$, desc$, level As Double, split As Integer) As UInteger

    End Function

    ''' <summary>
    ''' open root folder
    ''' </summary>
    ''' <param name="level"></param>
    ''' <param name="split"></param>
    ''' <returns></returns>
    Public Function Create(Optional level As Double = 0.85,
                           Optional split As Integer = 3,
                           Optional name As String = "no_named",
                           Optional desc As String = "no_information") As BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool

        Dim fs As New mysqlFs(Me, CreateModel(name, desc, level, split))
        Dim pool As New BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool(fs, "/")

        Call fs.SetLevel(level, split)
        Call fs.SetScore(0.3, 0.05)

        Return pool
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="model_id"></param>
    ''' <param name="score">
    ''' WARNING: this optional parameter will overrides the mode score 
    ''' level when this parameter has a positive numeric value in 
    ''' range ``(0,1]``.
    ''' </param>
    ''' <returns></returns>
    Public Function Open(Optional model_id As String = Nothing, Optional score As Double? = Nothing) As BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool
        Dim fs As New mysqlFs(Me, model_id)
        Dim pool As New BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData.SpectrumPool(fs, "/")

        If score IsNot Nothing AndAlso
            score > 0 AndAlso
            score < 1 Then

            Call fs.SetLevel(score, fs.split)
        Else
            Call fs.SetLevel(fs.level, fs.split)
        End If

        Call fs.SetScore(0.3, 0.05)

        Return pool
    End Function
End Class
