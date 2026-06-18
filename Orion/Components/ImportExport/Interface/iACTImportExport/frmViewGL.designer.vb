<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewGL
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
        Me.statusStrip = New System.Windows.Forms.StatusStrip
        Me.toolStripStatusLabelMain = New System.Windows.Forms.ToolStripStatusLabel
        Me.toolStripStatusLabelChildCount = New System.Windows.Forms.ToolStripStatusLabel
        Me.toolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar
        Me.tabControl = New System.Windows.Forms.TabControl
        Me.statusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'statusStrip
        '
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolStripStatusLabelMain, Me.toolStripStatusLabelChildCount, Me.toolStripProgressBar})
        Me.statusStrip.Location = New System.Drawing.Point(0, 567)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(805, 22)
        Me.statusStrip.TabIndex = 7
        '
        'toolStripStatusLabelMain
        '
        Me.toolStripStatusLabelMain.Name = "toolStripStatusLabelMain"
        Me.toolStripStatusLabelMain.Size = New System.Drawing.Size(790, 17)
        Me.toolStripStatusLabelMain.Spring = True
        Me.toolStripStatusLabelMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'toolStripStatusLabelChildCount
        '
        Me.toolStripStatusLabelChildCount.Name = "toolStripStatusLabelChildCount"
        Me.toolStripStatusLabelChildCount.Size = New System.Drawing.Size(0, 17)
        '
        'toolStripProgressBar
        '
        Me.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.toolStripProgressBar.Name = "toolStripProgressBar"
        Me.toolStripProgressBar.Size = New System.Drawing.Size(100, 16)
        Me.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.toolStripProgressBar.Visible = False
        '
        'tabControl
        '
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Location = New System.Drawing.Point(0, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.ShowToolTips = True
        Me.tabControl.Size = New System.Drawing.Size(805, 567)
        Me.tabControl.TabIndex = 8
        '
        'frmViewGL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 589)
        Me.Controls.Add(Me.tabControl)
        Me.Controls.Add(Me.statusStrip)
        Me.Name = "frmViewGL"
        Me.Text = "frmViewGL"
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Private WithEvents toolStripStatusLabelMain As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents toolStripStatusLabelChildCount As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents toolStripProgressBar As System.Windows.Forms.ToolStripProgressBar
    Private WithEvents tabControl As System.Windows.Forms.TabControl
End Class
