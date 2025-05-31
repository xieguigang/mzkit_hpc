#Region "Microsoft.VisualBasic::f6ae693c87ad4feb73bc91fd30983c50, Rscript\Library\mzkit_hpc\src\spectrumPool\driver\mysqlFs.vb"

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

    '   Total Lines: 206
    '    Code Lines: 160 (77.67%)
    ' Comment Lines: 1 (0.49%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 45 (21.84%)
    '     File Size: 6.91 KB


    ' Class mysqlFs
    ' 
    '     Properties: model_id, mysql
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    '     Function: CheckExists, FindRootId, getParentId, GetTreeChilds, (+2 Overloads) LoadMetadata
    '               ReadSpectrum, WriteSpectrum
    ' 
    '     Sub: Close, CommitMetadata, init, SetRootId
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra.SplashID
Imports BioNovoGene.BioDeep.MassSpectrometry.MoleculeNetworking.PoolData
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Class mysqlFs : Inherits PoolFs
    Implements IReadOnlyId

    ReadOnly db As dataPool

    Friend ReadOnly model As clusterModels.graph_model

    Friend ReadOnly root_id As UInteger
    Friend ReadOnly metadata_pool As New Dictionary(Of String, mysqlRepository)
    Friend ReadOnly cluster_data As New Dictionary(Of String, JavaScriptObject)

    Public ReadOnly Property model_id As String Implements IReadOnlyId.Identity
        Get
            Return model.id.ToString
        End Get
    End Property

    Public ReadOnly Property mysql As dataPool
        Get
            Return db
        End Get
    End Property

    Sub New(db As dataPool, model_id As UInteger)
        Me.db = db
        Me.model = db.graph_model _
            .where(field("id") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Sub New(db As dataPool, model_id As String)
        Me.db = db
        Me.model = db.graph_model _
            .where(field("name") = model_id Or field("id") = model_id) _
            .find(Of clusterModels.graph_model)

        Call init(root_id)
    End Sub

    Private Sub init(ByRef root_id As UInteger)
        Dim root = db.cluster _
            .where(field("model_id") = model.id,
                   field("key") = "/") _
            .find(Of clusterModels.cluster)

        If root Is Nothing Then
            root_id = 0
        Else
            root_id = root.id
        End If

        Dim pars As Dictionary(Of String, Double) = model.parameters _
            .LoadJSON(Of Dictionary(Of String, Double))

        m_level = pars!level
        m_split = pars!split
    End Sub

    Public Overrides Sub CommitMetadata(path As String, data As MetadataProxy)

    End Sub

    Public Overrides Sub SetRootId(path As String, id As String)

    End Sub

    Protected Overrides Sub Close()

    End Sub

    Public Overrides Function CheckExists(spectral As PeakMs2) As Boolean
        Dim hashcode As String = spectral.lib_guid
        Dim filename As String = spectral.file
        Dim model_id As String = Me.model_id
        Dim project As String = spectral.meta.TryGetValue("project", [default]:="unknown project")
        Dim biodeep_id As String = spectral.meta.TryGetValue("biodeep_id", [default]:="unknown conserved")
        ' check of the metadata
        Dim check As clusterModels.metadata = mysql.metadata _
            .where(field("hashcode") = hashcode,
                   field("filename") = filename,
                   field("model_id") = model_id,
                   field("project") = project,
                   field("xref_id") = biodeep_id) _
            .find(Of clusterModels.metadata)

        Return Not check Is Nothing
    End Function

    Public Overrides Iterator Function GetTreeChilds(path As String) As IEnumerable(Of String)

    End Function

    Public Overrides Function LoadMetadata(path As String) As MetadataProxy
        Dim key As String = HttpTreeFs.ClusterHashIndex(path)

        If Not metadata_pool.ContainsKey(key) Then
            Dim meta As New mysqlRepository(Me, path, getParentId(path))
            metadata_pool.Add(key, meta)
            Return meta
        Else
            Return metadata_pool(key)
        End If
    End Function

    Private Function getParentId(path As String) As UInteger
        If path = "/" Then
            Return 0
        Else
            Dim parent As String = path.ParentPath(full:=False)
            Dim parentHashKey As String = HttpTreeFs.ClusterHashIndex(parent)
            Dim meta = metadata_pool(parentHashKey)

            Return meta.guid
        End If
    End Function

    Public Overrides Function LoadMetadata(id As Integer) As MetadataProxy
        Return New mysqlRepository(Me, id)
    End Function

    Public Overrides Function FindRootId(path As String) As String
        Dim key As String = HttpTreeFs.ClusterHashIndex(path)

        If Not metadata_pool.ContainsKey(key) Then
            Return Nothing
        End If

        Dim spec_id = metadata_pool(key).RootId

        If spec_id.StringEmpty Then
            Return Nothing
        End If

        Dim spec = mysql.spectrum_pool _
            .where(field("id") = spec_id) _
            .find(Of clusterModels.spectrum_pool)

        Return spec.hashcode
    End Function

    Public Overrides Function ReadSpectrum(p As Metadata) As PeakMs2
        Dim q = mysql.spectrum_pool _
            .where(field("id") = p.block.position) _
            .find(Of clusterModels.spectrum_pool)

        If q Is Nothing Then
            Return Nothing
        End If

        Dim spectral As ms2() = HttpTreeFs.decodeSpectrum(q.mz, q.into, q.npeaks) _
            .SafeQuery _
            .ToArray

        If spectral.Length <> q.npeaks Then
            Return Nothing
        End If

        Return New PeakMs2 With {
            .lib_guid = q.hashcode,
            .mzInto = spectral
        }
    End Function

    Public Overrides Function WriteSpectrum(spectral As PeakMs2) As Metadata
        Dim metadata As Metadata = TreeFs.GetMetadata(spectral)
        Dim mz As String = HttpTreeFs.encode(spectral.mzInto.Select(Function(m) m.mz))
        Dim into As String = HttpTreeFs.encode(spectral.mzInto.Select(Function(m) m.intensity))

        Call mysql.spectrum_pool.add(
            field("npeaks") = spectral.mzInto.Length,
            field("entropy") = SpectralEntropy.Entropy(spectral),
            field("splash_id") = Splash.MSSplash.CalcSplashID(spectral),
            field("hashcode") = spectral.lib_guid,
            field("model_id") = model.id,
            field("mz") = mz,
            field("into") = into
        )

        Dim insert = mysql.spectrum_pool _
            .where(field("hashcode") = spectral.lib_guid, field("model_id") = model.id) _
            .order_by("id", desc:=True) _
            .find(Of clusterModels.spectrum_pool)

        If insert Is Nothing Then
            Throw New InvalidProgramException("add spectrum data into database error!")
        Else
            metadata.block = New BufferRegion With {
                .position = insert.id
            }
        End If

        Return metadata
    End Function
End Class

