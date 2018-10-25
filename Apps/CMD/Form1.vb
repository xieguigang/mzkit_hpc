Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Windows.Forms

Public Class Form1

    Dim commandBuffer As New List(Of Char)

    Private Sub boxTerminal_KeyDown(sender As Object, e As KeyEventArgs) Handles boxTerminal.KeyDown
        If e.KeyCode = Keys.Enter Then
            If commandBuffer > 0 Then
                Call FireCommand(commandBuffer.CharString)
                Call commandBuffer.Clear()
            End If

            Call boxTerminal.AppendText("# ")
        ElseIf Keys.KeyCode.IsPrintableCharacter Then
            commandBuffer += e.KeyCode.ToChar
        End If
    End Sub

    Private Sub FireCommand(cmd As String)

    End Sub
End Class
