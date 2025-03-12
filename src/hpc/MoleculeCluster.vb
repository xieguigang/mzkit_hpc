#Region "Microsoft.VisualBasic::c7e3ef9a3787ea0d3a03de8183fe4613, Rscript\Library\mzkit_hpc\src\hpc\MoleculeCluster.vb"

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

    '   Total Lines: 183
    '    Code Lines: 113 (61.75%)
    ' Comment Lines: 44 (24.04%)
    '    - Xml Docs: 95.45%
    ' 
    '   Blank Lines: 26 (14.21%)
    '     File Size: 7.23 KB


    ' Module MoleculeCluster
    ' 
    '     Function: CastDataframe, fetch_matrix, fetch_tree, molecule_set
    ' 
    '     Sub: addMolecule, Main, makeClusterTree, updateMatrix
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.BioDeep.Chemoinformatics
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization.BinaryDumping
Imports MoleculeTree
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

''' <summary>
''' Create molecule tree via strucutre clustering for run unknown spectrum feature annotation
''' </summary>
''' <remarks>
''' steps for build molecule tree:
''' 
''' 1. push molecule structre data
''' 2. parse structure data as graph
''' 3. push many strcutre data
''' 4. update graph matrix data
''' 5. build molecule tree finally
''' 
''' </remarks>
<Package("molecule_tree")>
<RTypeExport("molecule_tree", GetType(molecule_tree))>
<RTypeExport("metabo_data", GetType(MetaboliteAnnotation))>
Module MoleculeCluster

    Sub Main()
        Call RInternal.Object.Converts.makeDataframe.addHandler(GetType(treeModel.molecules()), AddressOf CastDataframe)
    End Sub

    <RGenericOverloads("as.data.frame")>
    Public Function CastDataframe(pull As treeModel.molecules(), args As list, env As Environment) As dataframe
        Dim dataset As New dataframe With {
            .rownames = pull.Select(Function(a) a.db_xref).ToArray,
            .columns = New Dictionary(Of String, Array)
        }

        Call dataset.add("name", From a As treeModel.molecules In pull Select a.name)
        Call dataset.add("formula", From a As treeModel.molecules In pull Select a.formula)
        Call dataset.add("exact_mass", From a As treeModel.molecules In pull Select a.exact_mass)
        Call dataset.add("smiles", From a As treeModel.molecules In pull Select a.smiles)
        Call dataset.add("add_time", From a As treeModel.molecules In pull Select a.add_time)

        Return dataset
    End Function

    ''' <summary>
    ''' add molecule model data into database pool
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="meta">the brief metabolite annotation information for make cache</param>
    ''' <param name="smiles">molecule structure data</param>
    <ExportAPI("add_molecule")>
    Public Sub addMolecule(tree As molecule_tree,
                           meta As MetaboliteAnnotation,
                           <RRawVectorArgument(TypeCodes.string)>
                           smiles As Object)

        Dim smiles_data As String() = CLRVector.asCharacter(smiles)

        For Each smiles_str As String In smiles_data
            Call tree.AddMolecule(meta, smiles_str)
        Next
    End Sub

    <ExportAPI("update_matrix")>
    Public Sub updateMatrix(tree As molecule_tree,
                            Optional page_size As Integer = 100,
                            Optional fast_check As Boolean = False)

        Call GraphMatrix.ResolveMatrix(tree, page_size)
    End Sub

    <ExportAPI("make_clusterTree")>
    Public Sub makeClusterTree(tree As molecule_tree, model As String,
                               Optional cluster_cutoff As Double? = Nothing,
                               Optional right_cutoff As Double? = Nothing)

        If cluster_cutoff Is Nothing OrElse right_cutoff Is Nothing Then
            Call New Cluster(tree, model).BuildTree()
        Else
            Call New Cluster(tree, model, cluster_cutoff, right_cutoff).BuildTree()
        End If
    End Sub

    ''' <summary>
    ''' Download the molecule tree graph from the database
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="model"></param>
    ''' <returns></returns>
    <ExportAPI("fetch_tree")>
    Public Function fetch_tree(tree As molecule_tree, model As String) As NetworkGraph
        Return tree.FetchTree(model)
    End Function

    ''' <summary>
    ''' get molecule graph matrix data
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="db_xrefs"></param>
    ''' <param name="prefix"></param>
    ''' <param name="scalar">
    ''' the function returns a scalar molecule result: a numeric vector if not base64, or the raw base64 string of the matrix.
    ''' </param>
    ''' <param name="base64"></param>
    ''' <returns></returns>
    <ExportAPI("fetch_matrix")>
    Public Function fetch_matrix(tree As molecule_tree, <RRawVectorArgument> db_xrefs As Object,
                                 Optional prefix As String = Nothing,
                                 Optional scalar As Boolean = False,
                                 Optional base64 As Boolean = False) As Object

        Dim mol_xrefs As String() = CLRVector.asCharacter(db_xrefs)
        Dim list As New list

        Static network As New NetworkByteOrderBuffer

        If mol_xrefs.IsNullOrEmpty Then
            Call "no db_xref reference id for get molecule graph matrix!".Warning
            Return Nothing
        End If

        For Each id As String In mol_xrefs
            Dim fetch As String() = tree.graph _
                .left_join("molecules") _
                .on(field("`molecules`.id") = field("molecule_id")) _
                .where(field("db_xref") = id) _
                .limit(1) _
                .project(Of String)("matrix")

            If fetch.IsNullOrEmpty Then
                fetch = {Nothing}
                Call $"no reference from the external db_xrefs: {id}".Warning
            End If

            If scalar Then
                If base64 Then
                    Return fetch(0)
                ElseIf fetch(0) Is Nothing Then
                    Return Nothing
                Else
                    Return network.ParseDouble(fetch(0))
                End If
            End If

            Dim vec As Object = If(base64, fetch(0), If(fetch(0) Is Nothing, Nothing, network.ParseDouble(fetch(0))))

            If prefix Is Nothing Then
                Call list.add(id, vec)
            Else
                Call list.add(prefix & id, vec)
            End If
        Next

        Return list
    End Function

    ''' <summary>
    ''' get a set of the molecule information in a given model
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="model">
    ''' the name reference to a specific model
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("molecule_set")>
    Public Function molecule_set(tree As molecule_tree, model As String, Optional env As Environment = Nothing) As Object
        Dim pars As treeModel.models = tree.models.where(field("name") = model).find(Of treeModel.models)

        If pars Is Nothing Then
            Return RInternal.debug.stop($"the required model which is referenced by name '{model}' could not be found!", env)
        End If

        Return MoleculeCluster.CastDataframe(tree.TreeMoleculeSet(pars).ToArray, list.empty, env)
    End Function
End Module
