Imports System.Data
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

''' <summary>
''' the molecule data information pool
''' </summary>
Public Module MoleculePool

    <Extension>
    Public Function TreeMoleculeSet(tree As molecule_tree, model As String) As IEnumerable(Of treeModel.molecules)
        Dim pars As treeModel.models = tree.models.where(field("name") = model).find(Of treeModel.models)

        If pars Is Nothing Then
            Throw New MissingPrimaryKeyException($"missing model which is referenced by name '{model}' inside database!")
        End If

        Return tree.TreeMoleculeSet(pars)
    End Function

    <Extension>
    Public Iterator Function TreeMoleculeSet(tree As molecule_tree, model As treeModel.models) As IEnumerable(Of treeModel.molecules)
        Dim page_size As Integer = 1000
        Dim page As Integer = 1

        Do While True
            Dim offset As UInteger = (page - 1) * page_size
            Dim data_sql = $"
SELECT 
    *
FROM
    molecules
WHERE
    id IN (SELECT DISTINCT
            molecule_id
        FROM
            molecule_tree.tree
                LEFT JOIN
            graph ON graph.id = graph_id
        WHERE
            model_id = {model.id})
LIMIT {offset},{page_size}
"
            Dim data As treeModel.molecules() = tree.getDriver.Query(Of treeModel.molecules)(data_sql)

            If data.IsNullOrEmpty Then
                Exit Do
            End If

            For Each mol As treeModel.molecules In data
                Yield mol
            Next
        Loop
    End Function

End Module
