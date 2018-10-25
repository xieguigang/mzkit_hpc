<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.boxTerminal = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'boxTerminal
        '
        Me.boxTerminal.BackColor = System.Drawing.Color.Black
        Me.boxTerminal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.boxTerminal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.boxTerminal.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxTerminal.ForeColor = System.Drawing.Color.White
        Me.boxTerminal.Location = New System.Drawing.Point(0, 0)
        Me.boxTerminal.Name = "boxTerminal"
        Me.boxTerminal.Size = New System.Drawing.Size(809, 451)
        Me.boxTerminal.TabIndex = 0
        Me.boxTerminal.Text = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(809, 451)
        Me.Controls.Add(Me.boxTerminal)
        Me.Name = "Form1"
        Me.Text = "CMD"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents boxTerminal As RichTextBox
End Class
