Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.Xml
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData

Public Class mysqlRepository : Inherits MetadataProxy

    Default Public Overrides ReadOnly Property GetById(id As String) As Metadata
        Get
        End Get
    End Property

    Public Overrides ReadOnly Property AllClusterMembers As IEnumerable(Of Metadata)
    Public Overrides ReadOnly Property Depth As Integer
    Public Overrides ReadOnly Property RootId As String

    Public Overrides Sub Add(id As String, metadata As Metadata)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Add(id As String, score As Double, align As AlignmentOutput, pval As Double)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub SetRootId(hashcode As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function HasGuid(id As String) As Boolean
        Throw New NotImplementedException()
    End Function
End Class
