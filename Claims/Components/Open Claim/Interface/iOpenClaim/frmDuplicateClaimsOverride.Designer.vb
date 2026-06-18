<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDuplicateClaimsOverride
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
	Public WithEvents lvwDuplicateClaims As System.Windows.Forms.ListView
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents cboApprovedUsers As System.Windows.Forms.ComboBox
	Public WithEvents lblApprovedUserDetail As System.Windows.Forms.Label
	Public WithEvents lblApprovedUserPassword As System.Windows.Forms.Label
	Public WithEvents lblApprovedUser As System.Windows.Forms.Label
	Public WithEvents fraApproverDetails As System.Windows.Forms.GroupBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraDuplicateClaims As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDuplicateClaimsOverride))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraDuplicateClaims = New System.Windows.Forms.GroupBox
		Me.lvwDuplicateClaims = New System.Windows.Forms.ListView
		Me.fraApproverDetails = New System.Windows.Forms.GroupBox
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.cboApprovedUsers = New System.Windows.Forms.ComboBox
		Me.lblApprovedUserDetail = New System.Windows.Forms.Label
		Me.lblApprovedUserPassword = New System.Windows.Forms.Label
		Me.lblApprovedUser = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.fraDuplicateClaims.SuspendLayout()
		Me.fraApproverDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraDuplicateClaims
		' 
		Me.fraDuplicateClaims.BackColor = System.Drawing.SystemColors.Control
		Me.fraDuplicateClaims.Controls.Add(Me.lvwDuplicateClaims)
		Me.fraDuplicateClaims.Controls.Add(Me.fraApproverDetails)
		Me.fraDuplicateClaims.Controls.Add(Me.Label1)
		Me.fraDuplicateClaims.Enabled = True
		Me.fraDuplicateClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDuplicateClaims.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDuplicateClaims.Location = New System.Drawing.Point(8, 8)
		Me.fraDuplicateClaims.Name = "fraDuplicateClaims"
		Me.fraDuplicateClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDuplicateClaims.Size = New System.Drawing.Size(673, 361)
		Me.fraDuplicateClaims.TabIndex = 5
		Me.fraDuplicateClaims.Text = "Potential Duplicate Claims"
		Me.fraDuplicateClaims.Visible = True
		' 
		' lvwDuplicateClaims
		' 
		Me.lvwDuplicateClaims.BackColor = System.Drawing.SystemColors.Window
		Me.lvwDuplicateClaims.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwDuplicateClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwDuplicateClaims.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwDuplicateClaims.HideSelection = True
		Me.lvwDuplicateClaims.LabelEdit = True
		Me.lvwDuplicateClaims.LabelWrap = True
		Me.lvwDuplicateClaims.Location = New System.Drawing.Point(8, 48)
		Me.lvwDuplicateClaims.Name = "lvwDuplicateClaims"
		Me.lvwDuplicateClaims.Size = New System.Drawing.Size(657, 233)
		Me.lvwDuplicateClaims.TabIndex = 0
		Me.lvwDuplicateClaims.View = System.Windows.Forms.View.Details
		' 
		' fraApproverDetails
		' 
		Me.fraApproverDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraApproverDetails.Controls.Add(Me.txtPassword)
		Me.fraApproverDetails.Controls.Add(Me.cboApprovedUsers)
		Me.fraApproverDetails.Controls.Add(Me.lblApprovedUserDetail)
		Me.fraApproverDetails.Controls.Add(Me.lblApprovedUserPassword)
		Me.fraApproverDetails.Controls.Add(Me.lblApprovedUser)
		Me.fraApproverDetails.Enabled = True
		Me.fraApproverDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraApproverDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraApproverDetails.Location = New System.Drawing.Point(8, 280)
		Me.fraApproverDetails.Name = "fraApproverDetails"
		Me.fraApproverDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraApproverDetails.Size = New System.Drawing.Size(657, 73)
		Me.fraApproverDetails.TabIndex = 7
		Me.fraApproverDetails.Text = "Approver Details"
		Me.fraApproverDetails.Visible = True
		' 
		' txtPassword
		' 
		Me.txtPassword.AcceptsReturn = True
		Me.txtPassword.AutoSize = False
		Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPassword.CausesValidation = True
		Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPassword.Enabled = True
		Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPassword.HideSelection = True
		Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtPassword.Location = New System.Drawing.Point(408, 44)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(225, 21)
		Me.txtPassword.TabIndex = 2
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' cboApprovedUsers
		' 
		Me.cboApprovedUsers.BackColor = System.Drawing.SystemColors.Window
		Me.cboApprovedUsers.CausesValidation = True
		Me.cboApprovedUsers.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboApprovedUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboApprovedUsers.Enabled = True
		Me.cboApprovedUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboApprovedUsers.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboApprovedUsers.IntegralHeight = True
		Me.cboApprovedUsers.Location = New System.Drawing.Point(56, 44)
		Me.cboApprovedUsers.Name = "cboApprovedUsers"
		Me.cboApprovedUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboApprovedUsers.Size = New System.Drawing.Size(249, 21)
		Me.cboApprovedUsers.Sorted = False
		Me.cboApprovedUsers.TabIndex = 1
		Me.cboApprovedUsers.TabStop = True
		Me.cboApprovedUsers.Visible = True
		' 
		' lblApprovedUserDetail
		' 
		Me.lblApprovedUserDetail.AutoSize = True
		Me.lblApprovedUserDetail.BackColor = System.Drawing.SystemColors.Control
		Me.lblApprovedUserDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblApprovedUserDetail.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblApprovedUserDetail.Enabled = True
		Me.lblApprovedUserDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblApprovedUserDetail.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblApprovedUserDetail.Location = New System.Drawing.Point(16, 19)
		Me.lblApprovedUserDetail.Name = "lblApprovedUserDetail"
		Me.lblApprovedUserDetail.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblApprovedUserDetail.Size = New System.Drawing.Size(626, 13)
		Me.lblApprovedUserDetail.TabIndex = 10
		Me.lblApprovedUserDetail.Text = "Override details required before claim entry can continue.  Please select overriding user and enter password."
		Me.lblApprovedUserDetail.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblApprovedUserDetail.UseMnemonic = True
		Me.lblApprovedUserDetail.Visible = True
		' 
		' lblApprovedUserPassword
		' 
		Me.lblApprovedUserPassword.AutoSize = True
		Me.lblApprovedUserPassword.BackColor = System.Drawing.SystemColors.Control
		Me.lblApprovedUserPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblApprovedUserPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblApprovedUserPassword.Enabled = True
		Me.lblApprovedUserPassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblApprovedUserPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblApprovedUserPassword.Location = New System.Drawing.Point(336, 48)
		Me.lblApprovedUserPassword.Name = "lblApprovedUserPassword"
		Me.lblApprovedUserPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblApprovedUserPassword.Size = New System.Drawing.Size(66, 13)
		Me.lblApprovedUserPassword.TabIndex = 9
		Me.lblApprovedUserPassword.Text = "Password:"
		Me.lblApprovedUserPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblApprovedUserPassword.UseMnemonic = True
		Me.lblApprovedUserPassword.Visible = True
		' 
		' lblApprovedUser
		' 
		Me.lblApprovedUser.AutoSize = True
		Me.lblApprovedUser.BackColor = System.Drawing.SystemColors.Control
		Me.lblApprovedUser.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblApprovedUser.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblApprovedUser.Enabled = True
		Me.lblApprovedUser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblApprovedUser.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblApprovedUser.Location = New System.Drawing.Point(16, 48)
		Me.lblApprovedUser.Name = "lblApprovedUser"
		Me.lblApprovedUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblApprovedUser.Size = New System.Drawing.Size(34, 13)
		Me.lblApprovedUser.TabIndex = 8
		Me.lblApprovedUser.Text = "User:"
		Me.lblApprovedUser.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblApprovedUser.UseMnemonic = True
		Me.lblApprovedUser.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(473, 13)
		Me.Label1.TabIndex = 6
		Me.Label1.Text = "Warning: The folllowing list of claim(s) are possible duplicates of the current claim."
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(608, 376)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
		Me.cmdCancel.TabIndex = 4
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(528, 376)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(73, 23)
		Me.cmdOk.TabIndex = 3
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&OK"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmDuplicateClaimsOverride
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(687, 404)
		Me.ControlBox = False
		Me.Controls.Add(Me.fraDuplicateClaims)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDuplicateClaimsOverride"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Duplicate Claim Override"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraDuplicateClaims.ResumeLayout(False)
		Me.fraApproverDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class