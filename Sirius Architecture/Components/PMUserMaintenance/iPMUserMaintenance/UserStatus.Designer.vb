<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserStatus
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents chkPassedExam As System.Windows.Forms.CheckBox
	Public WithEvents cboUserStatus As System.Windows.Forms.ComboBox
	Public WithEvents txtDatePassedExam As System.Windows.Forms.TextBox
	Public WithEvents lblDatePassed As System.Windows.Forms.Label
	Public WithEvents fraExam As System.Windows.Forms.GroupBox
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserStatus))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.chkPassedExam = New System.Windows.Forms.CheckBox
		Me.cboUserStatus = New System.Windows.Forms.ComboBox
		Me.fraExam = New System.Windows.Forms.GroupBox
		Me.txtDatePassedExam = New System.Windows.Forms.TextBox
		Me.lblDatePassed = New System.Windows.Forms.Label
		Me.Frame1.SuspendLayout()
		Me.fraExam.SuspendLayout()
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
		Me.cmdOK.Location = New System.Drawing.Point(104, 128)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 3
		Me.cmdOK.TabStop = True
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
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(184, 128)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.chkPassedExam)
		Me.Frame1.Controls.Add(Me.cboUserStatus)
		Me.Frame1.Controls.Add(Me.fraExam)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(249, 113)
		Me.Frame1.TabIndex = 0
		Me.Frame1.Text = "User Status"
		Me.Frame1.Visible = True
		' 
		' chkPassedExam
		' 
		Me.chkPassedExam.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkPassedExam.BackColor = System.Drawing.SystemColors.Control
		Me.chkPassedExam.CausesValidation = True
		Me.chkPassedExam.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkPassedExam.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkPassedExam.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkPassedExam.Enabled = True
		Me.chkPassedExam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkPassedExam.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkPassedExam.Location = New System.Drawing.Point(16, 48)
		Me.chkPassedExam.Name = "chkPassedExam"
		Me.chkPassedExam.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkPassedExam.Size = New System.Drawing.Size(97, 17)
		Me.chkPassedExam.TabIndex = 4
		Me.chkPassedExam.TabStop = True
		Me.chkPassedExam.Text = "Passed Exam"
		Me.chkPassedExam.Visible = True
		' 
		' cboUserStatus
		' 
		Me.cboUserStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cboUserStatus.CausesValidation = True
		Me.cboUserStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboUserStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboUserStatus.Enabled = True
		Me.cboUserStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboUserStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboUserStatus.IntegralHeight = True
		Me.cboUserStatus.Location = New System.Drawing.Point(8, 22)
		Me.cboUserStatus.Name = "cboUserStatus"
		Me.cboUserStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboUserStatus.Size = New System.Drawing.Size(233, 21)
		Me.cboUserStatus.Sorted = False
		Me.cboUserStatus.TabIndex = 1
		Me.cboUserStatus.TabStop = True
		Me.cboUserStatus.Visible = True
		' 
		' fraExam
		' 
		Me.fraExam.BackColor = System.Drawing.SystemColors.Control
		Me.fraExam.Controls.Add(Me.txtDatePassedExam)
		Me.fraExam.Controls.Add(Me.lblDatePassed)
		Me.fraExam.Enabled = True
		Me.fraExam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraExam.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraExam.Location = New System.Drawing.Point(8, 48)
		Me.fraExam.Name = "fraExam"
		Me.fraExam.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraExam.Size = New System.Drawing.Size(233, 57)
		Me.fraExam.TabIndex = 6
		Me.fraExam.Visible = True
		' 
		' txtDatePassedExam
		' 
		Me.txtDatePassedExam.AcceptsReturn = True
		Me.txtDatePassedExam.AutoSize = False
		Me.txtDatePassedExam.BackColor = System.Drawing.SystemColors.Window
		Me.txtDatePassedExam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDatePassedExam.CausesValidation = True
		Me.txtDatePassedExam.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDatePassedExam.Enabled = True
		Me.txtDatePassedExam.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDatePassedExam.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDatePassedExam.HideSelection = True
		Me.txtDatePassedExam.Location = New System.Drawing.Point(88, 24)
		Me.txtDatePassedExam.MaxLength = 0
		Me.txtDatePassedExam.Multiline = False
		Me.txtDatePassedExam.Name = "txtDatePassedExam"
		Me.txtDatePassedExam.ReadOnly = False
		Me.txtDatePassedExam.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDatePassedExam.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDatePassedExam.Size = New System.Drawing.Size(137, 21)
		Me.txtDatePassedExam.TabIndex = 5
		Me.txtDatePassedExam.TabStop = True
		Me.txtDatePassedExam.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDatePassedExam.Visible = True
		' 
		' lblDatePassed
		' 
		Me.lblDatePassed.AutoSize = True
		Me.lblDatePassed.BackColor = System.Drawing.SystemColors.Control
		Me.lblDatePassed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDatePassed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDatePassed.Enabled = True
		Me.lblDatePassed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDatePassed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDatePassed.Location = New System.Drawing.Point(8, 24)
		Me.lblDatePassed.Name = "lblDatePassed"
		Me.lblDatePassed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDatePassed.Size = New System.Drawing.Size(76, 13)
		Me.lblDatePassed.TabIndex = 7
		Me.lblDatePassed.Text = "Date Passed:"
		Me.lblDatePassed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDatePassed.UseMnemonic = True
		Me.lblDatePassed.Visible = True
		' 
		' frmUserStatus
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(263, 157)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.Frame1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmUserStatus"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "User Status"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
		Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
		Me.Frame1.ResumeLayout(False)
		Me.fraExam.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class