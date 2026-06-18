<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaintenance
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents txtABICode As System.Windows.Forms.TextBox
	Public WithEvents chkDeleted As System.Windows.Forms.CheckBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtText As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblText As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMaintenance))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtABICode = New System.Windows.Forms.TextBox
		Me.chkDeleted = New System.Windows.Forms.CheckBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.txtText = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.lblText = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtABICode
		' 
		Me.txtABICode.AcceptsReturn = True
		Me.txtABICode.AutoSize = False
		Me.txtABICode.BackColor = System.Drawing.SystemColors.Window
		Me.txtABICode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtABICode.CausesValidation = True
		Me.txtABICode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtABICode.Enabled = False
		Me.txtABICode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtABICode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtABICode.HideSelection = True
		Me.txtABICode.Location = New System.Drawing.Point(16, 80)
		Me.txtABICode.MaxLength = 0
		Me.txtABICode.Multiline = False
		Me.txtABICode.Name = "txtABICode"
		Me.txtABICode.ReadOnly = False
		Me.txtABICode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtABICode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtABICode.Size = New System.Drawing.Size(129, 19)
		Me.txtABICode.TabIndex = 1
		Me.txtABICode.TabStop = True
		Me.txtABICode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtABICode.Visible = True
		' 
		' chkDeleted
		' 
		Me.chkDeleted.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDeleted.BackColor = System.Drawing.SystemColors.Control
		Me.chkDeleted.CausesValidation = True
		Me.chkDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkDeleted.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkDeleted.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDeleted.Enabled = True
		Me.chkDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDeleted.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDeleted.Location = New System.Drawing.Point(296, 72)
		Me.chkDeleted.Name = "chkDeleted"
		Me.chkDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDeleted.Size = New System.Drawing.Size(81, 33)
		Me.chkDeleted.TabIndex = 2
		Me.chkDeleted.TabStop = True
		Me.chkDeleted.Text = "Deleted"
		Me.chkDeleted.Visible = False
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(224, 120)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(304, 120)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtText
		' 
		Me.txtText.AcceptsReturn = True
		Me.txtText.AutoSize = False
		Me.txtText.BackColor = System.Drawing.SystemColors.Window
		Me.txtText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtText.CausesValidation = True
		Me.txtText.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtText.Enabled = True
		Me.txtText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtText.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtText.HideSelection = True
		Me.txtText.Location = New System.Drawing.Point(16, 32)
		Me.txtText.MaxLength = 0
		Me.txtText.Multiline = False
		Me.txtText.Name = "txtText"
		Me.txtText.ReadOnly = False
		Me.txtText.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtText.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtText.Size = New System.Drawing.Size(361, 19)
		Me.txtText.TabIndex = 0
		Me.txtText.TabStop = True
		Me.txtText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtText.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 64)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(65, 17)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "ABI Code"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
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
		Me.lblText.Location = New System.Drawing.Point(16, 8)
		Me.lblText.Name = "lblText"
		Me.lblText.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblText.Size = New System.Drawing.Size(81, 17)
		Me.lblText.TabIndex = 5
		Me.lblText.Text = "Text"
		Me.lblText.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblText.UseMnemonic = True
		Me.lblText.Visible = True
		' 
		' frmMaintenance
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(386, 153)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtABICode)
		Me.Controls.Add(Me.chkDeleted)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.txtText)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.lblText)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 19)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMaintenance"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Maintenance"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class