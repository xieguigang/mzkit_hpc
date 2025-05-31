#Region "Microsoft.VisualBasic::04baf5fd7e0c93e14dd59433b4c3e6cd, Rscript\Library\mzkit_hpc\src\spectrumPool\mysql\models\db_models.vb"

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

    '   Total Lines: 38
    '    Code Lines: 34 (89.47%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 4 (10.53%)
    '     File Size: 1.65 KB


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

Namespace clusterModels

Public MustInherit Class db_models : Inherits IDatabase
Protected ReadOnly m_cluster As TableModel(Of cluster)
Protected ReadOnly m_cluster_data As TableModel(Of cluster_data)
Protected ReadOnly m_cluster_graph As TableModel(Of cluster_graph)
Protected ReadOnly m_cluster_tree As TableModel(Of cluster_tree)
Protected ReadOnly m_consensus_model As TableModel(Of consensus_model)
Protected ReadOnly m_consensus_spectrum As TableModel(Of consensus_spectrum)
Protected ReadOnly m_graph_model As TableModel(Of graph_model)
Protected ReadOnly m_metadata As TableModel(Of metadata)
Protected ReadOnly m_project As TableModel(Of project)
Protected ReadOnly m_rawfiles As TableModel(Of rawfiles)
Protected ReadOnly m_sample_groups As TableModel(Of sample_groups)
Protected ReadOnly m_spectrum_pool As TableModel(Of spectrum_pool)
Protected Sub New(mysqli As ConnectionUri)
Call MyBase.New(mysqli)

Me.m_cluster = model(Of cluster)()
Me.m_cluster_data = model(Of cluster_data)()
Me.m_cluster_graph = model(Of cluster_graph)()
Me.m_cluster_tree = model(Of cluster_tree)()
Me.m_consensus_model = model(Of consensus_model)()
Me.m_consensus_spectrum = model(Of consensus_spectrum)()
Me.m_graph_model = model(Of graph_model)()
Me.m_metadata = model(Of metadata)()
Me.m_project = model(Of project)()
Me.m_rawfiles = model(Of rawfiles)()
Me.m_sample_groups = model(Of sample_groups)()
Me.m_spectrum_pool = model(Of spectrum_pool)()
End Sub
End Class

End Namespace

