<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_Renamed()
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
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents cboAccess As System.Windows.Forms.ComboBox
	Public WithEvents txtAccess As System.Windows.Forms.TextBox
	Public WithEvents lblNewAccess As System.Windows.Forms.Label
	Public WithEvents lblDocFoldName As System.Windows.Forms.Label
	Public WithEvents imgSafe As System.Windows.Forms.PictureBox
	Public WithEvents lblCurrentAccess As System.Windows.Forms.Label
	Public WithEvents fraAccess As System.Windows.Forms.GroupBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraAccess = New System.Windows.Forms.GroupBox
		Me.txtName = New System.Windows.Forms.TextBox
		Me.cboAccess = New System.Windows.Forms.ComboBox
		Me.txtAccess = New System.Windows.Forms.TextBox
		Me.lblNewAccess = New System.Windows.Forms.Label
		Me.lblDocFoldName = New System.Windows.Forms.Label
		Me.imgSafe = New System.Windows.Forms.PictureBox
		Me.lblCurrentAccess = New System.Windows.Forms.Label
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraAccess.SuspendLayout()
		Me.SuspendLayout()
		' 
		' fraAccess
		' 
		Me.fraAccess.BackColor = System.Drawing.SystemColors.Control
		Me.fraAccess.Controls.Add(Me.txtName)
		Me.fraAccess.Controls.Add(Me.cboAccess)
		Me.fraAccess.Controls.Add(Me.txtAccess)
		Me.fraAccess.Controls.Add(Me.lblNewAccess)
		Me.fraAccess.Controls.Add(Me.lblDocFoldName)
		Me.fraAccess.Controls.Add(Me.imgSafe)
		Me.fraAccess.Controls.Add(Me.lblCurrentAccess)
		Me.fraAccess.Enabled = True
		Me.fraAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraAccess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraAccess.Location = New System.Drawing.Point(8, 8)
		Me.fraAccess.Name = "fraAccess"
		Me.fraAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraAccess.Size = New System.Drawing.Size(345, 121)
		Me.fraAccess.TabIndex = 3
		Me.fraAccess.Visible = True
		' 
		' txtName
		' 
		Me.txtName.AcceptsReturn = True
		Me.txtName.AutoSize = False
		Me.txtName.BackColor = System.Drawing.SystemColors.Menu
		Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtName.CausesValidation = True
		Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtName.Enabled = True
		Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtName.HideSelection = True
		Me.txtName.Location = New System.Drawing.Point(152, 24)
		Me.txtName.MaxLength = 0
		Me.txtName.Multiline = False
		Me.txtName.Name = "txtName"
		Me.txtName.ReadOnly = True
		Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtName.Size = New System.Drawing.Size(121, 19)
		Me.txtName.TabIndex = 9
		Me.txtName.TabStop = True
		Me.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtName.Visible = True
		' 
		' cboAccess
		' 
		Me.cboAccess.BackColor = System.Drawing.SystemColors.Window
		Me.cboAccess.CausesValidation = True
		Me.cboAccess.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboAccess.Enabled = True
		Me.cboAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAccess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboAccess.IntegralHeight = True
		Me.cboAccess.Location = New System.Drawing.Point(152, 88)
		Me.cboAccess.Name = "cboAccess"
		Me.cboAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboAccess.Size = New System.Drawing.Size(49, 21)
		Me.cboAccess.Sorted = True
		Me.cboAccess.TabIndex = 7
		Me.cboAccess.TabStop = True
		Me.cboAccess.Visible = True
		' 
		' txtAccess
		' 
		Me.txtAccess.AcceptsReturn = True
		Me.txtAccess.AutoSize = False
		Me.txtAccess.BackColor = System.Drawing.SystemColors.Menu
		Me.txtAccess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAccess.CausesValidation = True
		Me.txtAccess.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAccess.Enabled = True
		Me.txtAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAccess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAccess.HideSelection = True
		Me.txtAccess.Location = New System.Drawing.Point(152, 56)
		Me.txtAccess.MaxLength = 0
		Me.txtAccess.Multiline = False
		Me.txtAccess.Name = "txtAccess"
		Me.txtAccess.ReadOnly = True
		Me.txtAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAccess.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAccess.Size = New System.Drawing.Size(49, 19)
		Me.txtAccess.TabIndex = 5
		Me.txtAccess.TabStop = True
		Me.txtAccess.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAccess.Visible = True
		' 
		' lblNewAccess
		' 
		Me.lblNewAccess.AutoSize = True
		Me.lblNewAccess.BackColor = System.Drawing.SystemColors.Control
		Me.lblNewAccess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNewAccess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNewAccess.Enabled = True
		Me.lblNewAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblNewAccess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNewAccess.Location = New System.Drawing.Point(16, 88)
		Me.lblNewAccess.Name = "lblNewAccess"
		Me.lblNewAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNewAccess.Size = New System.Drawing.Size(89, 13)
		Me.lblNewAccess.TabIndex = 8
		Me.lblNewAccess.Text = "New Access Level"
		Me.lblNewAccess.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNewAccess.UseMnemonic = True
		Me.lblNewAccess.Visible = True
		' 
		' lblDocFoldName
		' 
		Me.lblDocFoldName.AutoSize = True
		Me.lblDocFoldName.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocFoldName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocFoldName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocFoldName.Enabled = True
		Me.lblDocFoldName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocFoldName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocFoldName.Location = New System.Drawing.Point(16, 24)
		Me.lblDocFoldName.Name = "lblDocFoldName"
		Me.lblDocFoldName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocFoldName.Size = New System.Drawing.Size(118, 13)
		Me.lblDocFoldName.TabIndex = 6
		Me.lblDocFoldName.Text = "docu/folder Name"
		Me.lblDocFoldName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocFoldName.UseMnemonic = True
		Me.lblDocFoldName.Visible = True
		' 
		' imgSafe
		' 
		Me.imgSafe.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgSafe.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgSafe.Enabled = True
		Me.imgSafe.Image = CType(resources.GetObject("imgSafe.Image"), System.Drawing.Image)
		Me.imgSafe.Location = New System.Drawing.Point(296, 24)
		Me.imgSafe.Name = "imgSafe"
		Me.imgSafe.Size = New System.Drawing.Size(32, 32)
		Me.imgSafe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgSafe.Visible = True
		' 
		' lblCurrentAccess
		' 
		Me.lblCurrentAccess.AutoSize = True
		Me.lblCurrentAccess.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrentAccess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrentAccess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrentAccess.Enabled = True
		Me.lblCurrentAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrentAccess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrentAccess.Location = New System.Drawing.Point(16, 56)
		Me.lblCurrentAccess.Name = "lblCurrentAccess"
		Me.lblCurrentAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrentAccess.Size = New System.Drawing.Size(101, 13)
		Me.lblCurrentAccess.TabIndex = 4
		Me.lblCurrentAccess.Text = "Current Access Level"
		Me.lblCurrentAccess.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrentAccess.UseMnemonic = True
		Me.lblCurrentAccess.Visible = True
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(280, 144)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 2
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(200, 144)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(120, 144)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(361, 176)
		Me.ControlBox = True
		Me.Controls.Add(Me.fraAccess)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Access Level"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.fraAccess.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class