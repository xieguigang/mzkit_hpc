Imports System.Data.SQLite
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module CLI

    <ExportAPI("/export")>
    <Usage("/export /db <database.sqlite3> /table <tableName> [/out <table.csv>]")>
    Public Function exportTable(args As CommandLine) As Integer
        Dim dbFile$ = args <= "/db"
        Dim tableName$ = args <= "/table"
        Dim out$ = args("/out") Or $"{dbFile.TrimSuffix}_{tableName}.csv"
        Dim fields As String()

        Call Program.SelectQueryTable(
            dbFile, "sqlite_master",
            Function(names)
                ' CREATE TABLE sqlite_master (type TEXT, name TEXT, tbl_name TEXT, rootpage INTEGER, sql TEXT);
                If CStr(names(Scan0)) = "table" AndAlso CStr(names(1)) Then

                End If
            End Function)

    End Function
End Module
