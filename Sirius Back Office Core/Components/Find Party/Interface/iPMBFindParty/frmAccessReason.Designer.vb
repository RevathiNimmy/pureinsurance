<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccessReason
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtReason As System.Windows.Forms.TextBox
	Public WithEvents lblReason As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAccessReason))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.txtReason = New System.Windows.Forms.TextBox
		Me.lblReason = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(182, 180)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtReason
		' 
		Me.txtReason.AcceptsReturn = True
		Me.txtReason.AutoSize = False
		Me.txtReason.BackColor = System.Drawing.SystemColors.Window
		Me.txtReason.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReason.CausesValidation = True
		Me.txtReason.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReason.Enabled = True
		Me.txtReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReason.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReason.HideSelection = True
		Me.txtReason.Location = New System.Drawing.Point(8, 24)
		Me.txtReason.MaxLength = 0
		Me.txtReason.Multiline = True
		Me.txtReason.Name = "txtReason"
		Me.txtReason.ReadOnly = False
		Me.txtReason.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReason.Size = New System.Drawing.Size(425, 145)
		Me.txtReason.TabIndex = 0
		Me.txtReason.TabStop = True
		Me.txtReason.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReason.Visible = True
		' 
		' lblReason
		' 
		Me.lblReason.AutoSize = False
		Me.lblReason.BackColor = System.Drawing.SystemColors.Control
		Me.lblReason.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReason.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReason.Enabled = True
		Me.lblReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReason.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReason.Location = New System.Drawing.Point(8, 8)
		Me.lblReason.Name = "lblReason"
		Me.lblReason.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReason.Size = New System.Drawing.Size(429, 23)
		Me.lblReason.TabIndex = 1
		Me.lblReason.Text = "Please fill in the reason as to why is the Client being accessed?"
		Me.lblReason.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReason.UseMnemonic = True
		Me.lblReason.Visible = True
		' 
		' frmAccessReason
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(439, 210)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.txtReason)
		Me.Controls.Add(Me.lblReason)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAccessReason"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Why is the Client being accessed?"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class