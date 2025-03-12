#Region "Microsoft.VisualBasic::21f70a6da2562a9c198a85fe6a5875f6, Rscript\Library\mzkit_hpc\src\MoleculeTree\molecule_tree.vb"

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

    '   Total Lines: 156
    '    Code Lines: 124 (79.49%)
    ' Comment Lines: 5 (3.21%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 27 (17.31%)
    '     File Size: 5.27 KB


    ' Class molecule_tree
    ' 
    '     Properties: atoms, graph, models, molecule_atoms, molecules
    '                 tree
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetAtom, LazyLoadAtoms
    ' 
    '     Sub: AddMolecule
    ' 
    ' /********************************************************************************/

#End Region

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

    Public ReadOnly Property models As Model
        Get
            Return m_models
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
                Dim index As Integer = atom.ID

                If molecule_atoms.where(
                    field("molecule") = check.id,
                    field("index") = index,
                    field("atom_id") = aid).find(Of treeModel.molecule_atoms) Is Nothing Then

                    molecule_atoms.add(
                        field("molecule") = check.id,
                        field("index") = index,
                        field("atom_id") = aid
                    )
                End If
            Next
        End If

        Dim spares As New List(Of SparseGraph.Edge)

        ' graph data should be link to the internal atom group table
        For Each key As ChemicalKey In graph.AllBonds
            Call spares.Add(New SparseGraph.Edge(
                GetAtom(key.U), GetAtom(key.V)
            ))
        Next

        Call Me.graph.add(
            field("molecule_id") = check.id,
            field("hashcode") = hashcode,
            field("graph") = spares.ToArray.GetJson,
            field("smiles") = smiles,
            field("matrix") = ""
        )
    End Sub

    Public Function GetAtom(atom As ChemicalElement) As UInteger
        Dim refer As String = atom.elementName & If(atom.hydrogen > 0, "H" & atom.hydrogen, "") & If(atom.aromatic, "@aromatic", "")
        Dim group = atoms_cache.ComputeIfAbsent(refer,
            lazyValue:=Function()
                           Return LazyLoadAtoms(refer, atom)
                       End Function)

        Return group.id
    End Function

    Private Function LazyLoadAtoms(refer As String, atom As ChemicalElement) As treeModel.atoms
        Dim check_atom = atoms.where(field("unique_id") = refer).find(Of treeModel.atoms)()

        If check_atom Is Nothing Then
            Call atoms.add(
                field("unique_id") = refer,
                field("atom_group") = atom.group,
                field("element") = atom.elementName,
                field("aromatic") = If(atom.aromatic, 1, 0),
                field("hydrogen") = atom.hydrogen,
                field("charge") = atom.charge
            )
        Else
            Return check_atom
        End If

        Return atoms.where(field("unique_id") = refer) _
            .order_by("id", desc:=True) _
            .find(Of treeModel.atoms)()
    End Function
End Class
