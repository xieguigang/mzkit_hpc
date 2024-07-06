Imports System.IO
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class PeakResultPack : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim result As PeakTablePack = obj

        Call SaveXcms.DumpSample(result.peaktable, result.npeaks, result.sampleNames, file)
        Call SaveRtShifts.DumpShiftsData(result.rt_shifts, file)
        Call file.Flush()

        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim file As New MemoryStream
        Call WriteBuffer(obj, file)
        Return file
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim peak As xcms2() = SaveXcms.ReadSamplePeaks(file)
        Dim rtshifts As RtShift() = SaveRtShifts.ParseRtShifts(file).ToArray

        Return New PeakTablePack With {
            .peaktable = peak,
            .rt_shifts = rtshifts
        }
    End Function
End Class

Public Class XicPack : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim xicdata As FeatureXic = obj
        Dim bin As New BinaryWriter(file)

        bin.Write(xicdata.samples.Length)
        bin.Write(xicdata.mz)
        bin.Write(xicdata.samples.GetJson)
        bin.Flush()

        SaveXIC.DumpSample(xicdata.peak_groups, file)
        file.Flush()

        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim file As New MemoryStream
        Call WriteBuffer(obj, file)
        Return file
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim bin As New BinaryReader(file)
        Dim size As Integer = bin.ReadInt32
        Dim mz As Double = bin.ReadDouble
        Dim samples As String() = bin.ReadString.LoadJSON(Of String())
        Dim xicdata As MzGroup() = SaveXIC.ReadSample(file).ToArray

        Return New FeatureXic With {
            .mz = mz,
            .samples = samples,
            .peak_groups = xicdata
        }
    End Function
End Class