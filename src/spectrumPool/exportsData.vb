Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports Oracle.LinuxCompatibility.MySQL.MySqlBuilder

Public Module exportsData

    <Extension>
    Public Iterator Function ExportClusterSpectrum(data As dataPool, cluster_id As UInteger) As IEnumerable(Of PeakMs2)
        Dim spectral_id As UInteger() = data.cluster_data _
            .where(field("cluster_id") = cluster_id) _
            .project(Of UInteger)("spectral_id")

    End Function
End Module
