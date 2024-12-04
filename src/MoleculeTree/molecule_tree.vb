Imports BioNovoGene.BioDeep.Chemoinformatics
Imports BioNovoGene.BioDeep.Chemoinformatics.SMILES
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Serialization.JSON
Imports MoleculeTree.treeModel
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class molecule_tree : Inherits db_models

    Public ReadOnly Property atoms As Model
        Get
            Return m_atoms
        End Get
    End Property

    Public ReadOnly Property tree As Model
        Get
            Return m_tree
        End Get
    End Property

    Public ReadOnly Property graph As Model
        Get
            Return m_graph
        End Get
    End Property

    Public ReadOnly Property molecules As Model
        Get
            Return m_molecules
        End Get
    End Property

    Public ReadOnly Property molecule_atoms As Model
        Get
            Return m_molecule_atoms
        End Get
    End Property

    Dim atoms_cache As New Dictionary(Of String, treeModel.atoms)

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub

    Public Sub AddMolecule(meta As MetaboliteAnnotation, smiles As String)
        ' check molecule is existed?
        Dim check = molecules.where(field("db_xref") = meta.Id).find(Of treeModel.molecules)
        Dim hashcode As String = smiles.MD5

        If check Is Nothing Then
            ' create new molecule data
            Call molecules.add(
                field("db_xref") = meta.Id,
                field("name") = meta.CommonName,
                field("formula") = meta.Formula,
                field("exact_mass") = meta.ExactMass,
                field("smiles") = smiles
            )

            check = molecules.where(field("db_xref") = meta.Id).order_by("id", desc:=True).find(Of treeModel.molecules)

            If check Is Nothing Then
                Call $"create molecule failure: {molecules.GetLastErrorMessage} [{molecules.GetLastMySql}]".Warning
                Return
            End If
        End If

        ' check of the graph data
        Dim check_graph = Me.graph.where(field("molecule_id") = check.id, field("hashcode") = hashcode).find(Of treeModel.graph)

        If check_graph IsNot Nothing Then
            ' alreay have
            Return
        End If

        Dim graph As ChemicalFormula = ParseChain.ParseGraph(smiles, strict:=False)

        If graph Is Nothing Then
            Return
        Else
            For Each atom As ChemicalElement In graph.AllElements
                Dim aid As UInteger = GetAtom(atom)
            Next
        End If

        Dim spares As SparseGraph = SparseGraph.Copy(graph)
        Dim graph_json As String = spares.GetJson

        Call Me.graph.add(
            field("molecule_id") = check.id,
            field("hashcode") = hashcode,
            field("graph") = graph_json,
            field("smiles") = smiles,
            field("matrix") = ""
        )
    End Sub

    Public Function GetAtom(atom As ChemicalElement) As UInteger
        Dim refer As String = atom.elementName & If(atom.hydrogen > 0, "H" & atom.hydrogen, "") & If(atom.aromatic, "@aromatic", "")

        Return atoms_cache.ComputeIfAbsent(refer,
            lazyValue:=Function()
                           Call atoms.add(
                               field("unique_id") = refer,
                               field("atom_group") = atom.group,
                               field("element") = atom.elementName,
                               field("aromatic") = If(atom.aromatic, 1, 0),
                               field("hydrogen") = atom.hydrogen,
                               field("charge") = atom.charge
                           )

                           Return atoms.where(field("unique_id") = refer) _
                               .order_by("id", desc:=True) _
                               .find(Of treeModel.atoms)()
                       End Function).id
    End Function
End Class
