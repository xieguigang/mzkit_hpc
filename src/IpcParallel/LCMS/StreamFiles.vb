Imports System.IO
Imports BioNovoGene.Analytical.MassSpectrometry.Math
Imports Darwinism.HPC.Parallel

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
        Throw New NotImplementedException()
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Throw New NotImplementedException()
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Throw New NotImplementedException()
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Throw New NotImplementedException()
    End Function
End Class