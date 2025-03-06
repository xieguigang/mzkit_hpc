#Region "Microsoft.VisualBasic::1261e60f64cc49bd27a24f5e0956c9e9, Rscript\Library\mzkit_hpc\src\MoleculeTree\models\db_models.vb"

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

    '   Total Lines: 26
    '    Code Lines: 22 (84.62%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 4 (15.38%)
    '     File Size: 783 B


    ' Class db_models
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Oracle.LinuxCompatibility.MySQL
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder
Imports Oracle.LinuxCompatibility.MySQL.Uri

Namespace treeModel

Public MustInherit Class db_models : Inherits IDatabase
Protected ReadOnly m_atoms As Model
Protected ReadOnly m_graph As Model
Protected ReadOnly m_models As Model
Protected ReadOnly m_molecule_atoms As Model
Protected ReadOnly m_molecules As Model
Protected ReadOnly m_tree As Model
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_atoms = model(Of atoms)()
Me.m_graph = model(Of graph)()
Me.m_models = model(Of models)()
Me.m_molecule_atoms = model(Of molecule_atoms)()
Me.m_molecules = model(Of molecules)()
Me.m_tree = model(Of tree)()
End Sub
End Class

End Namespace

