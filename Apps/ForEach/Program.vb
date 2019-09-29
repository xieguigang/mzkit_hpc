#Region "Microsoft.VisualBasic::74f8093a9d4e15d82034734e94cab12c, CLI_tools\ls\Program.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

' Module Program
' 
'     Function: Main
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Parallel.Threads

Module Program

    ReadOnly noParallel As [Default](Of String) = "1"

    ' foreach [*.txt] do cli_tool command_argvs
    ' 使用 $file 作为文件路径的占位符

    Public Function Main() As Integer
        Dim argv$() = App.CommandLine.Tokens

        If argv.IsNullOrEmpty Then
            Call Console.WriteLine(" Syntax:")
            Call Console.WriteLine()
            Call Console.WriteLine(" ForEach [*.ext/dir] [In <Folder>] Do <Action> <Arguments, use '$file' as placeholder>")

            Return 0
        Else
            Return argv.doForeach
        End If
    End Function

    <Extension>
    Private Function doForeach(argv$()) As Integer
        Dim filter$ = ""
        Dim appName$
        Dim cli$
        Dim dir$ = App.CurrentDirectory

        If argv(1).TextEquals("do") Then
            filter = argv(0)
            appName = argv(2)
            cli = CLITools.Join(argv.Skip(3))
        ElseIf argv(0).TextEquals("do") Then
            filter = "*.*"
            appName = argv(1)
            cli = CLITools.Join(argv.Skip(2))
        ElseIf argv(3).TextEquals("do") Then
            filter = argv(0)
            appName = argv(4)
            cli = CLITools.Join(argv.Skip(5))
            dir = argv(2)
        Else
            Throw New NotImplementedException()
        End If

        Dim commandLines As New List(Of String)
        Dim environment As Dictionary(Of String, String) = CType(appName & " " & cli, CommandLine).EnvironmentVariables
        Dim parallels% = environment.TryGetValue("/parallel") Or noParallel
        Dim isCLR As Boolean = environment.TryGetValue("/clr", [default]:="false").ParseBoolean
        Dim appShell As Func(Of String, Integer) =
            Function(task)
                Return App.Shell(appName, task, CLR:=True).Run
            End Function

        If Not isCLR Then
            cli = CLITools.GetTokens(cli) _
                .TakeWhile(Function(t)
                               Return Not t.TextEquals("/@set")
                           End Function) _
                .DoCall(AddressOf CLITools.Join)
        End If

        If filter.TextEquals("dir") Then
            For Each file As String In dir.ListDirectory
                commandLines += cli.Replace("$file", file).Replace("$basename", file.DirectoryName)
            Next
        Else
            For Each file As String In dir.EnumerateFiles(filter)
                commandLines += cli.Replace("$file", file).Replace("$basename", file.BaseName)
            Next
        End If

        Call commandLines.BatchTask(appShell, numThreads:=parallels)

        Return 0
    End Function
End Module
