Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model

Public Module MakePeakAlignment

    <Extension>
    Public Function CreatePeaktable(pool As XICPool, features_mz As Double(),
                                    errors As Tolerance,
                                    rtRange As DoubleRange,
                                    baseline As Double,
                                    joint As Boolean,
                                    dtw As Boolean, ByRef rt_shifts As RtShift()) As xcms2()

    End Function
End Module
