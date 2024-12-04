
Imports BioNovoGene.BioDeep.Chemoinformatics
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MoleculeTree
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 
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
Module MoleculeCluster

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
    Public Sub updateMatrix(tree As molecule_tree, Optional page_size As Integer = 100)
        Call GraphMatrix.ResolveMatrix(tree, page_size)
    End Sub

    <ExportAPI("make_clusterTree")>
    Public Sub makeClusterTree(tree As molecule_tree)
        Call New Cluster(tree).BuildTree()
    End Sub

End Module
