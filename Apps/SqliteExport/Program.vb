Imports System.Data.SQLite

Module Program

    Public Function Main() As Integer
        Return GetType(CLI).RunCLI(App.CommandLine)
    End Function

    Public Sub SelectQueryTable(dbFile$, tableName$, rowAction As Func(Of Object(), Boolean))
        Dim connStr$ = $"data source={dbFile.GetFullPath};version=3;"
        Dim cn As New SQLiteConnection(connStr)
        Call cn.Open()

        Dim cmd = cn.CreateCommand()
        cmd.CommandText = $"SELECT * FROM {tableName};"

        Dim row As New List(Of Object)

        Using reader As SQLiteDataReader = cmd.ExecuteReader()
            Do While reader.Read()
                For i As Integer = 0 To reader.FieldCount - 1
                    Call row.Add(reader(i))
                Next

                If rowAction(row.ToArray) Then
                    Exit Do
                End If
            Loop
        End Using
    End Sub

End Module
