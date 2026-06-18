<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSecurityQuestion
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
	Public WithEvents chkPasswordOK As System.Windows.Forms.CheckBox
	Public WithEvents chkKnowVoice As System.Windows.Forms.CheckBox
	Public WithEvents chkKnowPhoneNo As System.Windows.Forms.CheckBox
	Public WithEvents pnlAnswers As System.Windows.Forms.GroupBox
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtPostcode As System.Windows.Forms.TextBox
	Public WithEvents lblPassword As System.Windows.Forms.Label
	Public WithEvents lblPostcode As System.Windows.Forms.Label
	Public WithEvents pnlDetails As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSecurityQuestion))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pnlAnswers = New System.Windows.Forms.GroupBox
		Me.chkPasswordOK = New System.Windows.Forms.CheckBox
		Me.chkKnowVoice = New System.Windows.Forms.CheckBox
		Me.chkKnowPhoneNo = New System.Windows.Forms.CheckBox
		Me.pnlDetails = New System.Windows.Forms.GroupBox
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtPostcode = New System.Windows.Forms.TextBox
		Me.lblPassword = New System.Windows.Forms.Label
		Me.lblPostcode = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.pnlAnswers.SuspendLayout()
		Me.pnlDetails.SuspendLayout()
		Me.SuspendLayout()
		' 
		' pnlAnswers
		' 
		Me.pnlAnswers.BackColor = System.Drawing.SystemColors.Control
		Me.pnlAnswers.Controls.Add(Me.chkPasswordOK)
		Me.pnlAnswers.Controls.Add(Me.chkKnowVoice)
		Me.pnlAnswers.Controls.Add(Me.chkKnowPhoneNo)
		Me.pnlAnswers.Enabled = True
		Me.pnlAnswers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlAnswers.ForeColor = System.Drawing.SystemColors.ControlText
		Me.pnlAnswers.Location = New System.Drawing.Point(6, 92)
		Me.pnlAnswers.Name = "pnlAnswers"
		Me.pnlAnswers.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.pnlAnswers.Size = New System.Drawing.Size(313, 105)
		Me.pnlAnswers.TabIndex = 5
		Me.pnlAnswers.Visible = True
		' 
		' chkPasswordOK
		' 
		Me.chkPasswordOK.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPasswordOK.BackColor = System.Drawing.SystemColors.Control
		Me.chkPasswordOK.CausesValidation = True
		Me.chkPasswordOK.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkPasswordOK.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPasswordOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPasswordOK.Enabled = True
		Me.chkPasswordOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPasswordOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPasswordOK.Location = New System.Drawing.Point(10, 22)
		Me.chkPasswordOK.Name = "chkPasswordOK"
		Me.chkPasswordOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPasswordOK.Size = New System.Drawing.Size(287, 13)
		Me.chkPasswordOK.TabIndex = 6
		Me.chkPasswordOK.TabStop = True
		Me.chkPasswordOK.Text = "Password && Postal Code Correctly Supplied"
		Me.chkPasswordOK.Visible = True
		' 
		' chkKnowVoice
		' 
		Me.chkKnowVoice.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkKnowVoice.BackColor = System.Drawing.SystemColors.Control
		Me.chkKnowVoice.CausesValidation = True
		Me.chkKnowVoice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkKnowVoice.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkKnowVoice.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkKnowVoice.Enabled = True
		Me.chkKnowVoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkKnowVoice.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkKnowVoice.Location = New System.Drawing.Point(10, 74)
		Me.chkKnowVoice.Name = "chkKnowVoice"
		Me.chkKnowVoice.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkKnowVoice.Size = New System.Drawing.Size(287, 13)
		Me.chkKnowVoice.TabIndex = 8
		Me.chkKnowVoice.TabStop = True
		Me.chkKnowVoice.Text = "Voice Recognised:"
		Me.chkKnowVoice.Visible = True
		' 
		' chkKnowPhoneNo
		' 
		Me.chkKnowPhoneNo.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkKnowPhoneNo.BackColor = System.Drawing.SystemColors.Control
		Me.chkKnowPhoneNo.CausesValidation = True
		Me.chkKnowPhoneNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkKnowPhoneNo.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkKnowPhoneNo.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkKnowPhoneNo.Enabled = True
		Me.chkKnowPhoneNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkKnowPhoneNo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkKnowPhoneNo.Location = New System.Drawing.Point(10, 48)
		Me.chkKnowPhoneNo.Name = "chkKnowPhoneNo"
		Me.chkKnowPhoneNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkKnowPhoneNo.Size = New System.Drawing.Size(287, 13)
		Me.chkKnowPhoneNo.TabIndex = 7
		Me.chkKnowPhoneNo.TabStop = True
		Me.chkKnowPhoneNo.Text = "Telephone Number Recognised:"
		Me.chkKnowPhoneNo.Visible = True
		' 
		' pnlDetails
		' 
		Me.pnlDetails.BackColor = System.Drawing.SystemColors.Control
		Me.pnlDetails.Controls.Add(Me.txtPassword)
		Me.pnlDetails.Controls.Add(Me.txtPostcode)
		Me.pnlDetails.Controls.Add(Me.lblPassword)
		Me.pnlDetails.Controls.Add(Me.lblPostcode)
		Me.pnlDetails.Enabled = True
		Me.pnlDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.pnlDetails.Location = New System.Drawing.Point(6, 4)
		Me.pnlDetails.Name = "pnlDetails"
		Me.pnlDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.pnlDetails.Size = New System.Drawing.Size(311, 79)
		Me.pnlDetails.TabIndex = 0
		Me.pnlDetails.Text = "Details"
		Me.pnlDetails.Visible = True
		' 
		' txtPassword
		' 
		Me.txtPassword.AcceptsReturn = True
		Me.txtPassword.AutoSize = False
		Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPassword.CausesValidation = True
		Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPassword.Enabled = False
		Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPassword.HideSelection = True
		Me.txtPassword.Location = New System.Drawing.Point(128, 46)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(167, 21)
		Me.txtPassword.TabIndex = 4
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' txtPostcode
		' 
		Me.txtPostcode.AcceptsReturn = True
		Me.txtPostcode.AutoSize = False
		Me.txtPostcode.BackColor = System.Drawing.SystemColors.Window
		Me.txtPostcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPostcode.CausesValidation = True
		Me.txtPostcode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPostcode.Enabled = False
		Me.txtPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPostcode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPostcode.HideSelection = True
		Me.txtPostcode.Location = New System.Drawing.Point(128, 18)
		Me.txtPostcode.MaxLength = 0
		Me.txtPostcode.Multiline = False
		Me.txtPostcode.Name = "txtPostcode"
		Me.txtPostcode.ReadOnly = False
		Me.txtPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPostcode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPostcode.Size = New System.Drawing.Size(167, 21)
		Me.txtPostcode.TabIndex = 2
		Me.txtPostcode.TabStop = True
		Me.txtPostcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPostcode.Visible = True
		' 
		' lblPassword
		' 
		Me.lblPassword.AutoSize = False
		Me.lblPassword.BackColor = System.Drawing.Color.Transparent
		Me.lblPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.Enabled = True
		Me.lblPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Location = New System.Drawing.Point(10, 50)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPassword.Size = New System.Drawing.Size(103, 19)
		Me.lblPassword.TabIndex = 3
		Me.lblPassword.Text = "Password:"
		Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPassword.UseMnemonic = True
		Me.lblPassword.Visible = True
		' 
		' lblPostcode
		' 
		Me.lblPostcode.AutoSize = False
		Me.lblPostcode.BackColor = System.Drawing.Color.Transparent
		Me.lblPostcode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPostcode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPostcode.Enabled = True
		Me.lblPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPostcode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPostcode.Location = New System.Drawing.Point(10, 22)
		Me.lblPostcode.Name = "lblPostcode"
		Me.lblPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPostcode.Size = New System.Drawing.Size(103, 19)
		Me.lblPostcode.TabIndex = 1
		Me.lblPostcode.Text = "Postal Code:"
		Me.lblPostcode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPostcode.UseMnemonic = True
		Me.lblPostcode.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(246, 207)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 10
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
		Me.cmdOK.Location = New System.Drawing.Point(166, 207)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 9
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmSecurityQuestion
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(327, 238)
		Me.ControlBox = False
		Me.Controls.Add(Me.pnlAnswers)
		Me.Controls.Add(Me.pnlDetails)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSecurityQuestion"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Security Question"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.pnlAnswers.ResumeLayout(False)
		Me.pnlDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class