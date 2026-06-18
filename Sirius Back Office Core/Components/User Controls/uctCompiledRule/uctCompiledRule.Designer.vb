<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCompiledRule
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lbSuggestions = New System.Windows.Forms.ListBox()
        Me.txtCompiledDll = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lbSuggestions
        '
        Me.lbSuggestions.FormattingEnabled = True
        Me.lbSuggestions.Location = New System.Drawing.Point(0, 20)
        Me.lbSuggestions.Name = "lbSuggestions"
        Me.lbSuggestions.Size = New System.Drawing.Size(167, 95)
        Me.lbSuggestions.TabIndex = 3
        Me.lbSuggestions.Visible = False
        '
        'txtCompiledDll
        '
        Me.txtCompiledDll.Location = New System.Drawing.Point(0, 3)
        Me.txtCompiledDll.Name = "txtCompiledDll"
        Me.txtCompiledDll.Size = New System.Drawing.Size(223, 20)
        Me.txtCompiledDll.TabIndex = 0
        '
        'uctCompiledRule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtCompiledDll)
        Me.Name = "uctCompiledRule"
        Me.Size = New System.Drawing.Size(226, 26)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCompiledDll As System.Windows.Forms.TextBox
    Friend WithEvents lbSuggestions As System.Windows.Forms.ListBox

End Class
