Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class dataPool : Inherits clusterModels.db_models

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class
