Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Information
Imports Microsoft.VisualBasic.Math.Quantile
Imports Microsoft.VisualBasic.Math.SIMD

Public Class FeatureVector

    Public Property cell_labels As String()
    Public Property mz As Double
    Public Property mzmin As Double
    Public Property mzmax As Double
    Public Property intensity As Double()

    Public Function MeasureMSIIon(grid_size As Integer) As IonStat

    End Function

    Public Function MeasureStat() As SingleCellIonStat
        Dim max_i As Integer = which.Max(intensity)
        Dim counts As Integer = Aggregate xi As Double
                                In intensity
                                Where xi > 0
                                Into Count
        Dim quartile As DataQuartile = intensity.Quartile
        Dim base_cell = cell_labels(max_i)
        Dim maxo As Double = intensity(max_i)
        Dim intensity_vec = Divide.f64_op_divide_f64_scalar(intensity, intensity.Sum)
        Dim total_cells As Integer = intensity.Length

        Return New SingleCellIonStat With {
            .baseCell = base_cell,
            .cells = counts,
            .maxIntensity = maxo,
            .Q1Intensity = quartile.Q1,
            .Q2Intensity = quartile.Q2,
            .Q3Intensity = quartile.Q3,
            .entropy = intensity_vec.ShannonEntropy,
            .RSD = intensity_vec.RSD,
            .sparsity = 1 - counts / total_cells,
            .mz = mz,
            .mzmin = mzmin,
            .mzmax = mzmax,
            .mz_error = MassWindow.ToString(.mzmin, .mzmax)
        }
    End Function
End Class