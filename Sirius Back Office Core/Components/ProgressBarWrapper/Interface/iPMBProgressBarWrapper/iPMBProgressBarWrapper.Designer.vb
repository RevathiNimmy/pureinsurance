<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLoadAVI
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents UserControl11 As uctPMBProgressBar.UserControl1

	Public WithEvents lblText As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLoadAVI))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.UserControl11 = New uctPMBProgressBar.UserControl1

		Me.lblText = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' UserControl11
		' 
		Me.UserControl11.Location = New System.Drawing.Point(4, 36)
		Me.UserControl11.Name = "UserControl11"
		Me.UserControl11.Size = New System.Drawing.Size(249, 21)
		Me.UserControl11.TabIndex = 1
		' 
		' lblText
		' 
		Me.lblText.AutoSize = False
		Me.lblText.BackColor = System.Drawing.SystemColors.Control
		Me.lblText.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblText.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblText.Enabled = True
		Me.lblText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblText.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblText.Location = New System.Drawing.Point(8, 4)
		Me.lblText.Name = "lblText"
		Me.lblText.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblText.Size = New System.Drawing.Size(249, 25)
		Me.lblText.TabIndex = 0
		Me.lblText.Text = "Reports may take several minutes to process depending on how much data is returned. "
		Me.lblText.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblText.UseMnemonic = True
		Me.lblText.Visible = True
		' 
		' frmLoadAVI
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(256, 62)
		Me.ControlBox = False
		Me.Controls.Add(Me.UserControl11)
		Me.Controls.Add(Me.lblText)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLoadAVI"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Processing Report . . ."
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class