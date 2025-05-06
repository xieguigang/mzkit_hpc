Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.My.JavaScript
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class mysqlFs : Inherits PoolFs
    Implements IReadOnlyId

    ReadOnly db As dataPool

    Friend ReadOnly model As clusterModels.graph_model

    Friend ReadOnly root_id As UInteger
    Friend ReadOnly metadata_pool As New Dictionary(Of String, mysqlRepository)
    Friend ReadOnly cluster_data As New Dictionary(Of String, JavaScriptObject)

    Public ReadOnly Property model_id As String Implements IReadOnlyId.Identity
        Get
            Return model.id.ToString
        End Get
    End Property

    Sub New(db As dataPool, model_id As UInteger)
        Me.db = db
        Me.model = db.graph_model _
            .where(field("id") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Sub New(db As dataPool, model_id As String)
        Me.db = db
        Me.model = db.graph_model _
            .where(field("name") = model_id Or field("name") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Private Sub init(ByRef root_id As UInteger)

    End Sub

    Public Overrides Sub CommitMetadata(path As String, data As MetadataProxy)

    End Sub

    Public Overrides Sub SetRootId(path As String, id As String)

    End Sub

    Protected Overrides Sub Close()

    End Sub

    Public Overrides Function CheckExists(spectral As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2) As Boolean

    End Function

    Public Overrides Function GetTreeChilds(path As String) As IEnumerable(Of String)

    End Function

    Public Overrides Function LoadMetadata(path As String) As MetadataProxy

    End Function

    Public Overrides Function LoadMetadata(id As Integer) As MetadataProxy

    End Function

    Public Overrides Function FindRootId(path As String) As String

    End Function

    Public Overrides Function ReadSpectrum(p As Metadata) As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2

    End Function

    Public Overrides Function WriteSpectrum(spectral As BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.PeakMs2) As Metadata

    End Function
End Class
