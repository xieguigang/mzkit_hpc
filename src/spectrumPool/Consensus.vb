Imports System.Runtime.CompilerServices
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module Consensus

    <Extension>
    Public Function CreateModelParameters(mysql As dataPool,
                                          model As String,
                                          dimensions As Integer,
                                          knn As Integer,
                                          Optional cutoff As Double = 0.8) As clusterModels.consensus_model

        Dim modelObj As clusterModels.graph_model = mysql.graph_model _
            .where(field("name") = model) _
            .find(Of clusterModels.graph_model)

        If modelObj Is Nothing Then
            Throw New ArgumentException($"Model '{model}' not found in the database.")
        End If

        Call mysql.consensus_model.add(
            field("model_id") = modelObj.id,
            field("consensus_cutoff") = cutoff,
            field("umap_dimension") = dimensions,
            field("umap_neighbors") = knn,
            field("umap_others") = "{}"
        )

        Return mysql.consensus_model _
            .where(field("model_id") = modelObj.id) _
            .order_by("id", desc:=True) _
            .find(Of clusterModels.consensus_model)
    End Function

    Public Function 
End Module
