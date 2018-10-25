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
        chars(Keys.A) = "a"
        chars(Keys.B) = "b"
        chars(Keys.C) = "c"
        chars(Keys.D) = "d"
        chars(Keys.E) = "e"
        chars(Keys.F) = "f"
        chars(Keys.G) = "g"
        chars(Keys.H) = "h"
        chars(Keys.I) = "i"
        chars(Keys.J) = "j"
        chars(Keys.K) = "k"
        chars(Keys.L) = "l"
        chars(Keys.M) = "m"
        chars(Keys.N) = "n"
        chars(Keys.O) = "o"
        chars(Keys.P) = "p"
        chars(Keys.Q) = "q"
        chars(Keys.R) = "r"
        chars(Keys.S) = "s"
        chars(Keys.T) = "t"
        chars(Keys.U) = "u"
        chars(Keys.V) = "v"
        chars(Keys.W) = "w"
        chars(Keys.X) = "x"
        chars(Keys.Y) = "y"
        chars(Keys.Z) = "z"
        chars(Keys.D0) = "0" : chars(Keys.NumPad0) = "0"
        chars(Keys.D0) = "1" : chars(Keys.NumPad0) = "1"
        chars(Keys.D0) = "2" : chars(Keys.NumPad0) = "2"
        chars(Keys.D0) = "3" : chars(Keys.NumPad0) = "3"
        chars(Keys.D0) = "4" : chars(Keys.NumPad0) = "4"
        chars(Keys.D0) = "5" : chars(Keys.NumPad0) = "5"
        chars(Keys.D0) = "6" : chars(Keys.NumPad0) = "6"
        chars(Keys.D0) = "7" : chars(Keys.NumPad0) = "7"
        chars(Keys.D0) = "8" : chars(Keys.NumPad0) = "8"
        chars(Keys.D0) = "9" : chars(Keys.NumPad0) = "9"
    End Sub
End Class
