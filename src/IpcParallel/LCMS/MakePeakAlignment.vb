﻿Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.MIME.application.json

Public Class MakePeakAlignment

    Public Property errors As String
    Public Property rt_range As Double()
    Public Property baseline As Double
    Public Property joint As Boolean
    Public Property dtw As Boolean

    Public Function CreatePeaktable(peak_groups As XICPool, features_mz As Double(),
                                    errors As Tolerance,
                                    rtRange As DoubleRange,
                                    baseline As Double,
                                    joint As Boolean,
                                    dtw As Boolean, ByRef rt_shifts As RtShift()) As xcms2()

        Dim pack As String = New MakePeakAlignment With {
            .baseline = baseline,
            .dtw = dtw,
            .errors = errors.ToScript,
            .joint = joint,
            .rt_range = rtRange.MinMax
        }.GetJson
        Dim pool As FeatureXic() = features_mz.AsParallel _
            .Select(Function(mz)
                        Dim pick_features = peak_groups.GetXICMatrix(mz, errors).ToArray
                        Dim sources As String() = pick_features.Select(Function(a) a.Name).ToArray
                        Dim peaks As MzGroup() = pick_features.Select(Function(a) a.Value).ToArray

                        Return New FeatureXic With {
                            .mz = mz,
                            .peak_groups = peaks,
                            .samples = sources
                        }
                    End Function) _
            .ToArray

    End Function

    Private Shared Function MakePeakTable(peak_groups As FeatureXic(), args As MakePeakAlignment) As PeakTablePack
        Dim peaktable As New List(Of xcms2)
        Dim rt_shifts As New List(Of RtShift)

        For Each peak As FeatureXic In peak_groups
            With peak.GetPeakTable(args)
                Call peaktable.AddRange(.peaks)
                Call rt_shifts.AddRange(.rt_shifts)
            End With
        Next

        Return New PeakTablePack With {
            .peaktable = peaktable.ToArray,
            .rt_shifts = rt_shifts.ToArray
        }
    End Function

End Class

Public Class PeakTablePack

    Public Property peaktable As xcms2()
    Public Property rt_shifts As RtShift()

End Class

Public Class FeatureXic

    Public Property mz As Double
    Public Property samples As String()
    ''' <summary>
    ''' the xic features vector is keeps the same dimension size with the 
    ''' <see cref="samples"/> character vector, each sample name has a 
    ''' corresponding xic feature data from this vector in the same index
    ''' offset position.
    ''' </summary>
    ''' <returns></returns>
    Public Property peak_groups As MzGroup()

    Public Function GetPeakTable(pars As MakePeakAlignment) As (peaks As xcms2(), rt_shifts As RtShift())
        Dim samples_xic = samples _
            .Select(Function(name, i)
                        Return New NamedValue(Of MzGroup)(name, peak_groups(i))
                    End Function) _
            .ToArray
        Dim shifts As New List(Of RtShift)

        If pars.dtw Then
            samples_xic = XICPool.DtwXIC(samples_xic).ToArray
        End If

        Dim result As xcms2() = xic_deco_task.extractAlignedPeaks(samples_xic,
            rtRange:=New DoubleRange(pars.rt_range),
            baseline:=pars.baseline,
            joint:=pars.joint, xic_align:=True,
            rt_shifts:=shifts)

        Return (result, shifts.ToArray)
    End Function

End Class
