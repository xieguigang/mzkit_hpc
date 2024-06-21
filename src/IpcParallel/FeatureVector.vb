Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.SingleCells
Imports Microsoft.VisualBasic.Data.GraphTheory.GridGraph
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Information
Imports Microsoft.VisualBasic.Math.Quantile
Imports Microsoft.VisualBasic.Math.SIMD
Imports Microsoft.VisualBasic.Math.Statistics.Hypothesis
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Point = System.Drawing.Point

Public Class FeatureVector

    Public Property cell_labels As String()
    Public Property mz As Double
    Public Property mzmin As Double
    Public Property mzmax As Double
    Public Property intensity As Double()

    Public Function MeasureMSIIon(grid_size As Integer) As IonStat
        Dim mean As Double = intensity.Average
        Dim max_i As Integer = which.Max(intensity)
        Dim maxo As Integer = intensity(max_i)
        Dim max_spot As Integer() = cell_labels(max_i).Split(","c).AsInteger
        Dim spots As (xyz As String, intensity As Double)() = (
            From cell As (String, Double)
            In cell_labels.Zip(intensity)
            Where cell.Second > 0).ToArray
        Dim points = spots.Select(Function(cell) cell.xyz.Split(","c).AsInteger).ToArray
        Dim pixels = points.Select(Function(a) New Point(a(0), a(1))).CreateReadOnlyGrid
        Dim moran_test As MoranTest
        Dim quartile As DataQuartile = intensity.Quartile

        If points.Length < 3 Then
            moran_test = New MoranTest With {
                .df = 0, .Expected = 0, .Observed = 0,
                .prob2 = 1, .pvalue = 1, .SD = 0,
                .t = 0, .z = 0
            }
        Else
            moran_test = MoranTest.moran_test(
                x:=spots.Select(Function(c) c.intensity).ToArray,
                c1:=points.Select(Function(p) CDbl(p(0))).ToArray,
                c2:=points.Select(Function(p) CDbl(p(1))).ToArray,
                parallel:=False,
                throwMaxIterError:=False
            )
        End If

        Dim counts As New List(Of Double)
        Dim density As Double = 0

        For Each top As (xyz As String, intensity As Double) In (
            From cell
            In spots
            Order By cell.intensity Descending
            Take 30
        )
            Dim spatial As Integer() = top.xyz.Split(","c).AsInteger

            Call counts.Add(Aggregate cell As Point
                            In pixels.Query(spatial(0), spatial(1), grid_size)
                            Where Not cell.IsEmpty
                            Into Count)
        Next

        If counts.Count > 0 Then
            density = SIMD.Divide.f64_op_divide_f64_scalar(counts.ToArray, grid_size ^ 2).Average
        End If

        Dim total_cells As Integer = intensity.Length
        Dim intensity_vec = Divide.f64_op_divide_f64_scalar(intensity, intensity.Sum)

        Return New IonStat With {
            .averageIntensity = mean,
            .basePixelX = max_spot(0),
            .basePixelY = max_spot(1),
            .density = density,
            .maxIntensity = maxo,
            .moran = moran_test.Observed,
            .mz = mz,
            .mzmin = mzmin,
            .mzmax = mzmax,
            .mzwidth = MassWindow.ToString(.mzmax, .mzmin),
            .pixels = spots.Length,
            .pvalue = moran_test.pvalue,
            .Q1Intensity = quartile.Q1,
            .Q2Intensity = quartile.Q2,
            .Q3Intensity = quartile.Q3,
            .entropy = intensity_vec.ShannonEntropy,
            .rsd = intensity_vec.RSD,
            .sparsity = 1 - .pixels / total_cells
        }
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
            .mz_error = MassWindow.ToString(.mzmax, .mzmin)
        }
    End Function
End Class