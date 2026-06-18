<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopy
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
	Public WithEvents chkComments As System.Windows.Forms.CheckBox
	Public WithEvents txtFile As System.Windows.Forms.TextBox
	Public WithEvents chkOutputScript As System.Windows.Forms.CheckBox
	Public WithEvents lblFileName As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtDestination As System.Windows.Forms.TextBox
	Public WithEvents SSPanel1 As System.Windows.Forms.PictureBox
	Public WithEvents cboMap As System.Windows.Forms.ComboBox
	Public WithEvents cboProcess As System.Windows.Forms.ComboBox
	Public WithEvents cboDSN As System.Windows.Forms.ComboBox
	Public WithEvents lblDestination As System.Windows.Forms.Label
	Public WithEvents lblProcess As System.Windows.Forms.Label
	Public WithEvents lblMap As System.Windows.Forms.Label
	Public WithEvents lblDSN As System.Windows.Forms.Label
	Public WithEvents fraSource As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCopy))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.chkComments = New System.Windows.Forms.CheckBox
		Me.txtFile = New System.Windows.Forms.TextBox
		Me.chkOutputScript = New System.Windows.Forms.CheckBox
		Me.lblFileName = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraSource = New System.Windows.Forms.GroupBox
		Me.txtDestination = New System.Windows.Forms.TextBox
		Me.SSPanel1 = New System.Windows.Forms.PictureBox
		Me.cboMap = New System.Windows.Forms.ComboBox
		Me.cboProcess = New System.Windows.Forms.ComboBox
		Me.cboDSN = New System.Windows.Forms.ComboBox
		Me.lblDestination = New System.Windows.Forms.Label
		Me.lblProcess = New System.Windows.Forms.Label
		Me.lblMap = New System.Windows.Forms.Label
		Me.lblDSN = New System.Windows.Forms.Label
		Me.Frame1.SuspendLayout()
		Me.fraSource.SuspendLayout()
		Me.SuspendLayout()
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.chkComments)
		Me.Frame1.Controls.Add(Me.txtFile)
		Me.Frame1.Controls.Add(Me.chkOutputScript)
		Me.Frame1.Controls.Add(Me.lblFileName)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 208)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(393, 89)
		Me.Frame1.TabIndex = 14
		Me.Frame1.Text = "Scripting"
		Me.Frame1.Visible = True
		' 
		' chkComments
		' 
		Me.chkComments.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkComments.BackColor = System.Drawing.SystemColors.Control
		Me.chkComments.CausesValidation = True
		Me.chkComments.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkComments.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkComments.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkComments.Enabled = False
		Me.chkComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkComments.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkComments.Location = New System.Drawing.Point(128, 16)
		Me.chkComments.Name = "chkComments"
		Me.chkComments.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkComments.Size = New System.Drawing.Size(105, 25)
		Me.chkComments.TabIndex = 5
		Me.chkComments.TabStop = False
		Me.chkComments.Text = "Use Comments"
		Me.chkComments.Visible = True
		' 
		' txtFile
		' 
		Me.txtFile.AcceptsReturn = True
		Me.txtFile.AutoSize = False
		Me.txtFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFile.CausesValidation = True
		Me.txtFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFile.Enabled = False
		Me.txtFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFile.HideSelection = True
		Me.txtFile.Location = New System.Drawing.Point(64, 56)
		Me.txtFile.MaxLength = 0
		Me.txtFile.Multiline = False
		Me.txtFile.Name = "txtFile"
		Me.txtFile.ReadOnly = False
		Me.txtFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFile.Size = New System.Drawing.Size(313, 19)
		Me.txtFile.TabIndex = 6
		Me.txtFile.TabStop = True
		Me.txtFile.Text = "C:\InsertScript.sql"
		Me.txtFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFile.Visible = True
		' 
		' chkOutputScript
		' 
		Me.chkOutputScript.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkOutputScript.BackColor = System.Drawing.SystemColors.Control
		Me.chkOutputScript.CausesValidation = True
		Me.chkOutputScript.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkOutputScript.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkOutputScript.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkOutputScript.Enabled = True
		Me.chkOutputScript.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkOutputScript.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkOutputScript.Location = New System.Drawing.Point(8, 16)
		Me.chkOutputScript.Name = "chkOutputScript"
		Me.chkOutputScript.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkOutputScript.Size = New System.Drawing.Size(121, 25)
		Me.chkOutputScript.TabIndex = 4
		Me.chkOutputScript.TabStop = False
		Me.chkOutputScript.Text = "Make Copy Script"
		Me.chkOutputScript.Visible = True
		' 
		' lblFileName
		' 
		Me.lblFileName.AutoSize = False
		Me.lblFileName.BackColor = System.Drawing.SystemColors.Control
		Me.lblFileName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFileName.Enabled = True
		Me.lblFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFileName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFileName.Location = New System.Drawing.Point(8, 56)
		Me.lblFileName.Name = "lblFileName"
		Me.lblFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFileName.Size = New System.Drawing.Size(57, 17)
		Me.lblFileName.TabIndex = 15
		Me.lblFileName.Text = "File Name"
		Me.lblFileName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFileName.UseMnemonic = True
		Me.lblFileName.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(248, 304)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 9
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Text = "&Exit"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = False
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(328, 304)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 7
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&Apply"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraSource
		' 
		Me.fraSource.BackColor = System.Drawing.SystemColors.Control
		Me.fraSource.Controls.Add(Me.txtDestination)
		Me.fraSource.Controls.Add(Me.SSPanel1)
		Me.fraSource.Controls.Add(Me.cboMap)
		Me.fraSource.Controls.Add(Me.cboProcess)
		Me.fraSource.Controls.Add(Me.cboDSN)
		Me.fraSource.Controls.Add(Me.lblDestination)
		Me.fraSource.Controls.Add(Me.lblProcess)
		Me.fraSource.Controls.Add(Me.lblMap)
		Me.fraSource.Controls.Add(Me.lblDSN)
		Me.fraSource.Enabled = True
		Me.fraSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraSource.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSource.Location = New System.Drawing.Point(8, 8)
		Me.fraSource.Name = "fraSource"
		Me.fraSource.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraSource.Size = New System.Drawing.Size(393, 193)
		Me.fraSource.TabIndex = 8
		Me.fraSource.Text = "Source"
		Me.fraSource.Visible = True
		' 
		' txtDestination
		' 
		Me.txtDestination.AcceptsReturn = True
		Me.txtDestination.AutoSize = False
		Me.txtDestination.BackColor = System.Drawing.SystemColors.Window
		Me.txtDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDestination.CausesValidation = True
		Me.txtDestination.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDestination.Enabled = True
		Me.txtDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDestination.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDestination.HideSelection = True
		Me.txtDestination.Location = New System.Drawing.Point(72, 160)
		Me.txtDestination.MaxLength = 0
		Me.txtDestination.Multiline = False
		Me.txtDestination.Name = "txtDestination"
		Me.txtDestination.ReadOnly = False
		Me.txtDestination.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDestination.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDestination.Size = New System.Drawing.Size(305, 19)
		Me.txtDestination.TabIndex = 3
		Me.txtDestination.TabStop = True
		Me.txtDestination.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDestination.Visible = True
		' 
		' SSPanel1
		' 
		Me.SSPanel1.BackColor = System.Drawing.Color.Silver
		Me.SSPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.SSPanel1.CausesValidation = True
		Me.SSPanel1.Cursor = System.Windows.Forms.Cursors.Default
		Me.SSPanel1.Dock = System.Windows.Forms.DockStyle.None
		Me.SSPanel1.Enabled = True
		Me.SSPanel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.SSPanel1.Location = New System.Drawing.Point(8, 64)
		Me.SSPanel1.Name = "SSPanel1"
		Me.SSPanel1.Size = New System.Drawing.Size(377, 2)
		Me.SSPanel1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.SSPanel1.TabIndex = 10
		Me.SSPanel1.TabStop = True
		Me.SSPanel1.Visible = True
		' 
		' cboMap
		' 
		Me.cboMap.BackColor = System.Drawing.SystemColors.Window
		Me.cboMap.CausesValidation = True
		Me.cboMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboMap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboMap.Enabled = True
		Me.cboMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboMap.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboMap.IntegralHeight = True
		Me.cboMap.Location = New System.Drawing.Point(72, 120)
		Me.cboMap.Name = "cboMap"
		Me.cboMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboMap.Size = New System.Drawing.Size(305, 21)
		Me.cboMap.Sorted = False
		Me.cboMap.TabIndex = 2
		Me.cboMap.TabStop = True
		Me.cboMap.Visible = True
		' 
		' cboProcess
		' 
		Me.cboProcess.BackColor = System.Drawing.SystemColors.Window
		Me.cboProcess.CausesValidation = True
		Me.cboProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboProcess.Enabled = True
		Me.cboProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboProcess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboProcess.IntegralHeight = True
		Me.cboProcess.Location = New System.Drawing.Point(72, 80)
		Me.cboProcess.Name = "cboProcess"
		Me.cboProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboProcess.Size = New System.Drawing.Size(305, 21)
		Me.cboProcess.Sorted = False
		Me.cboProcess.TabIndex = 1
		Me.cboProcess.TabStop = True
		Me.cboProcess.Visible = True
		' 
		' cboDSN
		' 
		Me.cboDSN.BackColor = System.Drawing.SystemColors.Window
		Me.cboDSN.CausesValidation = True
		Me.cboDSN.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDSN.Enabled = True
		Me.cboDSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDSN.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDSN.IntegralHeight = True
		Me.cboDSN.Location = New System.Drawing.Point(72, 32)
		Me.cboDSN.Name = "cboDSN"
		Me.cboDSN.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDSN.Size = New System.Drawing.Size(305, 21)
		Me.cboDSN.Sorted = False
		Me.cboDSN.TabIndex = 0
		Me.cboDSN.TabStop = True
		Me.cboDSN.Visible = True
		' 
		' lblDestination
		' 
		Me.lblDestination.AutoSize = False
		Me.lblDestination.BackColor = System.Drawing.SystemColors.Control
		Me.lblDestination.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDestination.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDestination.Enabled = True
		Me.lblDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDestination.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDestination.Location = New System.Drawing.Point(16, 160)
		Me.lblDestination.Name = "lblDestination"
		Me.lblDestination.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDestination.Size = New System.Drawing.Size(57, 17)
		Me.lblDestination.TabIndex = 16
		Me.lblDestination.Text = "Destination"
		Me.lblDestination.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDestination.UseMnemonic = True
		Me.lblDestination.Visible = True
		' 
		' lblProcess
		' 
		Me.lblProcess.AutoSize = False
		Me.lblProcess.BackColor = System.Drawing.SystemColors.Control
		Me.lblProcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProcess.Enabled = True
		Me.lblProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProcess.Location = New System.Drawing.Point(16, 80)
		Me.lblProcess.Name = "lblProcess"
		Me.lblProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProcess.Size = New System.Drawing.Size(57, 17)
		Me.lblProcess.TabIndex = 13
		Me.lblProcess.Text = "Process"
		Me.lblProcess.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProcess.UseMnemonic = True
		Me.lblProcess.Visible = True
		' 
		' lblMap
		' 
		Me.lblMap.AutoSize = False
		Me.lblMap.BackColor = System.Drawing.SystemColors.Control
		Me.lblMap.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMap.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMap.Enabled = True
		Me.lblMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMap.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMap.Location = New System.Drawing.Point(16, 120)
		Me.lblMap.Name = "lblMap"
		Me.lblMap.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMap.Size = New System.Drawing.Size(57, 17)
		Me.lblMap.TabIndex = 12
		Me.lblMap.Text = "Map"
		Me.lblMap.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMap.UseMnemonic = True
		Me.lblMap.Visible = True
		' 
		' lblDSN
		' 
		Me.lblDSN.AutoSize = False
		Me.lblDSN.BackColor = System.Drawing.SystemColors.Control
		Me.lblDSN.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDSN.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDSN.Enabled = True
		Me.lblDSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDSN.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDSN.Location = New System.Drawing.Point(16, 32)
		Me.lblDSN.Name = "lblDSN"
		Me.lblDSN.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDSN.Size = New System.Drawing.Size(81, 17)
		Me.lblDSN.TabIndex = 11
		Me.lblDSN.Text = "Database"
		Me.lblDSN.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDSN.UseMnemonic = True
		Me.lblDSN.Visible = True
		' 
		' frmCopy
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(410, 333)
		Me.ControlBox = True
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.fraSource)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmCopy.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmCopy"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Copy"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame1.ResumeLayout(False)
		Me.fraSource.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class