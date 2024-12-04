Imports BioNovoGene.BioDeep.Chemoinformatics

Public Class Cluster

    ReadOnly tree As molecule_tree
    ''' <summary>
    ''' the root matrix
    ''' </summary>
    ReadOnly root As Double()

    Sub New(tree As molecule_tree)
        Me.tree = tree
    End Sub

    Public Sub Add(meta As MetaboliteAnnotation, smiles As String)

    End Sub

End Class
