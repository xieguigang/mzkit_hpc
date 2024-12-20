
Imports BioNovoGene.BioDeep.Chemoinformatics
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Scripting.MetaData
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
    Public Sub updateMatrix(tree As molecule_tree, Optional page_size As Integer = 100)
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
