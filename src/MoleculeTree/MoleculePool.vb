#Region "Microsoft.VisualBasic::484c298a5d0f35a255f81e74bab116bd, Rscript\Library\mzkit_hpc\src\MoleculeTree\MoleculePool.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 56
    '    Code Lines: 44 (78.57%)
    ' Comment Lines: 3 (5.36%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 9 (16.07%)
    '     File Size: 1.67 KB


    ' Module MoleculePool
    ' 
    '     Function: (+2 Overloads) TreeMoleculeSet
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Runtime.CompilerServices
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
