Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.MIME.application.json

Public Class MakePeakAlignment

    Public Property features_mz As Double()
    Public Property errors As String
    Public Property rt_range As Double()
    Public Property baseline As Double
    Public Property joint As Boolean
    Public Property dtw As Boolean

    Public Function CreatePeaktable(pool As XICPool, features_mz As Double(),
                                    errors As Tolerance,
                                    rtRange As DoubleRange,
                                    baseline As Double,
                                    joint As Boolean,
                                    dtw As Boolean, ByRef rt_shifts As RtShift()) As xcms2()

        Dim pack As String = New MakePeakAlignment With {
            .baseline = baseline,
            .dtw = dtw,
            .errors = errors.ToScript,
            .features_mz = features_mz,
            .joint = joint,
            .rt_range = rtRange.MinMax
        }.GetJson

    End Function

End Class
