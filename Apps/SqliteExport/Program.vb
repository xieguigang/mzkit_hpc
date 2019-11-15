Imports System.Data.SQLite
Imports Microsoft.VisualBasic.Language

Module Program

    Public Function Main() As Integer
        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function

    Public Sub SelectQueryTable(dbFile$, selectQuery$, rowAction As Func(Of Integer, Object(), Boolean))
        Dim connStr$ = $"data source={dbFile.GetFullPath};version=3;"
        Dim cn As New SQLiteConnection(connStr)
        Call cn.Open()

        Dim cmd = cn.CreateCommand()
        cmd.CommandText = selectQuery

        Dim row As New List(Of Object)
        Dim rid As i32 = Scan0

        Using reader As SQLiteDataReader = cmd.ExecuteReader()
            Do While reader.Read()
                For i As Integer = 0 To reader.FieldCount - 1
                    Call row.Add(reader(i))
                Next

                If rowAction(++rid, row.ToArray) Then
                    Exit Do
                Else
                    Call row.Clear()
                End If
            Loop
        End Using
    End Sub

End Module
