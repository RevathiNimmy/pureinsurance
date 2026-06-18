<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTestHarness
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
        Me.cmdProcessBackgroundJob = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cmdProcessBackgroundJob
        '
        Me.cmdProcessBackgroundJob.Location = New System.Drawing.Point(57, 26)
        Me.cmdProcessBackgroundJob.Name = "cmdProcessBackgroundJob"
        Me.cmdProcessBackgroundJob.Size = New System.Drawing.Size(266, 23)
        Me.cmdProcessBackgroundJob.TabIndex = 1
        Me.cmdProcessBackgroundJob.Text = "Process Background Job"
        Me.cmdProcessBackgroundJob.UseVisualStyleBackColor = True
        '
        'frmTestHarness
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(373, 173)
        Me.Controls.Add(Me.cmdProcessBackgroundJob)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmTestHarness"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pure Windows Service - Test Harness"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdProcessBackgroundJob As System.Windows.Forms.Button

End Class
