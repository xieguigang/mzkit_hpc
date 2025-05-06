Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData

Public Class mysqlFs : Inherits PoolFs

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
