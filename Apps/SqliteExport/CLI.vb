Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Data.IO.ManagedSqlite.Core.SQLSchema
Imports Microsoft.VisualBasic.Net.Http

<CLI> Module CLI

    <ExportAPI("/export")>
    <Usage("/export /db <database.sqlite3> /table <tableName> [/out <table.csv>]")>
    Public Function exportTable(args As CommandLine) As Integer
        Dim dbFile$ = args <= "/db"
        Dim tableName$ = args <= "/table"
        Dim out$ = args("/out") Or $"{dbFile.TrimSuffix}_{tableName}.csv"
        Dim fields As String() = Nothing

        Call Program.SelectQueryTable(
            dbFile, $"SELECT * FROM sqlite_master;",
            Function(rid, names)
                ' CREATE TABLE sqlite_master (type TEXT, name TEXT, tbl_name TEXT, rootpage INTEGER, sql TEXT);
                If CStr(names(Scan0)) = "table" AndAlso CStr(names(1)) = tableName Then
                    Dim schema As New Schema(names(4))

                    fields = schema.columns.Keys
                    Return True
                Else
                    Return False
                End If
            End Function)

        If fields.IsNullOrEmpty Then
            Call $"Could not found table '{tableName}' in database: {dbFile}".PrintException
            Return 500
        End If

        Dim SQL$ = $"SELECT {fields.JoinBy(", ")} FROM {tableName};"
        Dim table As New List(Of EntityObject)
        Dim haveIDCol As Integer = fields.IndexOf("ID")

        Call Program.SelectQueryTable(
            dbFile, SQL,
            Function(rid, row)
                Dim id As String
                Dim cols As New Dictionary(Of String, String)

                If haveIDCol > -1 Then
                    id = row(haveIDCol)

                    For i As Integer = 0 To fields.Length - 1
                        If i = haveIDCol Then
                            Continue For
                        End If

                        If row(i).GetType Is GetType(Byte()) Then
                            ' base64
                            cols.Add(fields(i), DirectCast(row(i), Byte()).ToBase64String)
                        Else
                            cols.Add(fields(i), row(i))
                        End If
                    Next
                Else
                    id = "#" & rid

                    For i As Integer = 0 To fields.Length - 1
                        If row(i).GetType Is GetType(Byte()) Then
                            ' base64
                            cols.Add(fields(i), DirectCast(row(i), Byte()).ToBase64String)
                        Else
                            cols.Add(fields(i), row(i))
                        End If
                    Next
                End If

                table.Add(New EntityObject With {.ID = id, .Properties = cols})

                Return False
            End Function)

        Return table.SaveDataSet(out).CLICode
    End Function
End Module
