Imports Microsoft.VisualBasic.Language

Public Class Form1

    Dim commandBuffer As New List(Of Char)
    Dim chars As New Dictionary(Of Keys, Char)

    Private Sub boxTerminal_KeyDown(sender As Object, e As KeyEventArgs) Handles boxTerminal.KeyDown
        If e.KeyCode = Keys.Enter Then
            If commandBuffer > 0 Then
                Call FireCommand(commandBuffer.CharString)
                Call commandBuffer.Clear()
            End If

            Call boxTerminal.AppendText("# ")
        ElseIf chars.ContainsKey(Keys.KeyCode) Then
            commandBuffer += chars(e.KeyCode)
        End If
    End Sub

    Private Sub FireCommand(cmd As String)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class
