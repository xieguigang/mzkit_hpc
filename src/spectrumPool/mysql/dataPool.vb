Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.Uri
Imports spectrumPool.clusterModels

Public Class dataPool : Inherits clusterModels.db_models

    Public ReadOnly Property spectrum_pool As TableModel(Of spectrum_pool)
        Get
            Return m_spectrum_pool
        End Get
    End Property

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class
