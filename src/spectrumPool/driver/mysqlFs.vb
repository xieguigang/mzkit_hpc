Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.My.JavaScript
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class mysqlFs : Inherits PoolFs

    ReadOnly db As dataPool
    Friend ReadOnly model_id As clusterModels.graph_model

    Friend ReadOnly root_id As UInteger
    Friend ReadOnly metadata_pool As New Dictionary(Of String, mysqlRepository)
    Friend ReadOnly cluster_data As New Dictionary(Of String, JavaScriptObject)

    Sub New(db As dataPool, model_id As UInteger)
        Me.db = db
        Me.model_id = db.graph_model _
            .where(field("id") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Sub New(db As dataPool, model_id As String)
        Me.db = db
        Me.model_id = db.graph_model _
            .where(field("name") = model_id Or field("name") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Private Sub init(ByRef root_id As UInteger)

    End Sub

    Public Overrides Sub CommitMetadata(path As String, data As MetadataProxy)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub SetRootId(path As String, id As String)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub Close()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function CheckExists(spectral As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetTreeChilds(path As String) As IEnumerable(Of String)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function LoadMetadata(path As String) As MetadataProxy
        Throw New NotImplementedException()
    End Function

    Public Overrides Function LoadMetadata(id As Integer) As MetadataProxy
        Throw New NotImplementedException()
    End Function

    Public Overrides Function FindRootId(path As String) As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function ReadSpectrum(p As Metadata) As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2
        Throw New NotImplementedException()
    End Function

    Public Overrides Function WriteSpectrum(spectral As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2) As Metadata
        Throw New NotImplementedException()
    End Function
End Class
