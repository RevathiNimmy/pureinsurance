<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
	Public WithEvents cmdConvertAll As System.Windows.Forms.Button
	Public WithEvents chkClipboard As System.Windows.Forms.CheckBox
	Public WithEvents cboDatabase As System.Windows.Forms.ComboBox
	Public WithEvents chkDebug As System.Windows.Forms.CheckBox
	Public WithEvents cboServer As System.Windows.Forms.ComboBox
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtUser As System.Windows.Forms.TextBox
	Public WithEvents cboProcessID As System.Windows.Forms.ComboBox
	Public WithEvents txtFolder As System.Windows.Forms.TextBox
	Public WithEvents cmdConvert As System.Windows.Forms.Button
	Public WithEvents lstLog As System.Windows.Forms.ListBox
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdConvertAll = New System.Windows.Forms.Button
		Me.chkClipboard = New System.Windows.Forms.CheckBox
		Me.cboDatabase = New System.Windows.Forms.ComboBox
		Me.chkDebug = New System.Windows.Forms.CheckBox
		Me.cboServer = New System.Windows.Forms.ComboBox
		Me.Command1 = New System.Windows.Forms.Button
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtUser = New System.Windows.Forms.TextBox
		Me.cboProcessID = New System.Windows.Forms.ComboBox
		Me.txtFolder = New System.Windows.Forms.TextBox
		Me.cmdConvert = New System.Windows.Forms.Button
		Me.lstLog = New System.Windows.Forms.ListBox
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' cmdConvertAll
		' 
		Me.cmdConvertAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdConvertAll.CausesValidation = True
		Me.cmdConvertAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdConvertAll.Enabled = False
		Me.cmdConvertAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdConvertAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdConvertAll.Location = New System.Drawing.Point(472, 368)
		Me.cmdConvertAll.Name = "cmdConvertAll"
		Me.cmdConvertAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdConvertAll.Size = New System.Drawing.Size(97, 25)
		Me.cmdConvertAll.TabIndex = 17
		Me.cmdConvertAll.TabStop = True
		Me.cmdConvertAll.Text = "Convert All"
		Me.cmdConvertAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdConvertAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' chkClipboard
		' 
		Me.chkClipboard.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkClipboard.BackColor = System.Drawing.SystemColors.Control
		Me.chkClipboard.CausesValidation = True
		Me.chkClipboard.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkClipboard.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkClipboard.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkClipboard.Enabled = True
		Me.chkClipboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkClipboard.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkClipboard.Location = New System.Drawing.Point(160, 340)
		Me.chkClipboard.Name = "chkClipboard"
		Me.chkClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkClipboard.Size = New System.Drawing.Size(193, 17)
		Me.chkClipboard.TabIndex = 16
		Me.chkClipboard.TabStop = True
		Me.chkClipboard.Text = "Copy start map filename to clipboard"
		Me.chkClipboard.Visible = True
		' 
		' cboDatabase
		' 
		Me.cboDatabase.BackColor = System.Drawing.SystemColors.Window
		Me.cboDatabase.CausesValidation = True
		Me.cboDatabase.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboDatabase.Enabled = True
		Me.cboDatabase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDatabase.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDatabase.IntegralHeight = True
		Me.cboDatabase.Location = New System.Drawing.Point(432, 48)
		Me.cboDatabase.Name = "cboDatabase"
		Me.cboDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDatabase.Size = New System.Drawing.Size(121, 21)
		Me.cboDatabase.Sorted = False
		Me.cboDatabase.TabIndex = 15
		Me.cboDatabase.TabStop = True
		Me.cboDatabase.Text = "Sirius"
		Me.cboDatabase.Visible = True
		' 
		' chkDebug
		' 
		Me.chkDebug.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDebug.BackColor = System.Drawing.SystemColors.Control
		Me.chkDebug.CausesValidation = True
		Me.chkDebug.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkDebug.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkDebug.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDebug.Enabled = True
		Me.chkDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDebug.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDebug.Location = New System.Drawing.Point(8, 340)
		Me.chkDebug.Name = "chkDebug"
		Me.chkDebug.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDebug.Size = New System.Drawing.Size(145, 17)
		Me.chkDebug.TabIndex = 14
		Me.chkDebug.TabStop = True
		Me.chkDebug.Text = "Include Debug Comments"
		Me.chkDebug.Visible = True
		' 
		' cboServer
		' 
		Me.cboServer.BackColor = System.Drawing.SystemColors.Window
		Me.cboServer.CausesValidation = True
		Me.cboServer.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboServer.Enabled = True
		Me.cboServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboServer.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboServer.IntegralHeight = True
		Me.cboServer.Location = New System.Drawing.Point(80, 80)
		Me.cboServer.Name = "cboServer"
		Me.cboServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboServer.Size = New System.Drawing.Size(121, 21)
		Me.cboServer.Sorted = False
		Me.cboServer.TabIndex = 13
		Me.cboServer.TabStop = True
		Me.cboServer.Visible = True
		' 
		' Command1
		' 
		Me.Command1.BackColor = System.Drawing.SystemColors.Control
		Me.Command1.CausesValidation = True
		Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Command1.Enabled = True
		Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Command1.Location = New System.Drawing.Point(368, 336)
		Me.Command1.Name = "Command1"
		Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Command1.Size = New System.Drawing.Size(97, 25)
		Me.Command1.TabIndex = 12
		Me.Command1.TabStop = True
		Me.Command1.Text = "Get Processes"
		Me.Command1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
		Me.txtPassword.Location = New System.Drawing.Point(432, 80)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(121, 19)
		Me.txtPassword.TabIndex = 9
		Me.txtPassword.TabStop = True
        Me.txtPassword.Text = ""
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' txtUser
		' 
		Me.txtUser.AcceptsReturn = True
		Me.txtUser.AutoSize = False
		Me.txtUser.BackColor = System.Drawing.SystemColors.Window
		Me.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtUser.CausesValidation = True
		Me.txtUser.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtUser.Enabled = True
		Me.txtUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtUser.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUser.HideSelection = True
		Me.txtUser.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtUser.Location = New System.Drawing.Point(240, 80)
		Me.txtUser.MaxLength = 0
		Me.txtUser.Multiline = False
		Me.txtUser.Name = "txtUser"
		Me.txtUser.ReadOnly = False
		Me.txtUser.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUser.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUser.Size = New System.Drawing.Size(121, 19)
		Me.txtUser.TabIndex = 7
		Me.txtUser.TabStop = True
		Me.txtUser.Text = "SIRIUS"
		Me.txtUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUser.Visible = True
		' 
		' cboProcessID
		' 
		Me.cboProcessID.BackColor = System.Drawing.SystemColors.Window
		Me.cboProcessID.CausesValidation = True
		Me.cboProcessID.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboProcessID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboProcessID.Enabled = True
		Me.cboProcessID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboProcessID.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboProcessID.IntegralHeight = True
		Me.cboProcessID.Location = New System.Drawing.Point(80, 16)
		Me.cboProcessID.Name = "cboProcessID"
		Me.cboProcessID.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboProcessID.Size = New System.Drawing.Size(489, 21)
		Me.cboProcessID.Sorted = False
		Me.cboProcessID.TabIndex = 5
		Me.cboProcessID.TabStop = True
		Me.cboProcessID.Visible = True
		' 
		' txtFolder
		' 
		Me.txtFolder.AcceptsReturn = True
		Me.txtFolder.AutoSize = False
		Me.txtFolder.BackColor = System.Drawing.SystemColors.Window
		Me.txtFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFolder.CausesValidation = True
		Me.txtFolder.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFolder.Enabled = True
		Me.txtFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFolder.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFolder.HideSelection = True
		Me.txtFolder.Location = New System.Drawing.Point(80, 48)
		Me.txtFolder.MaxLength = 0
		Me.txtFolder.Multiline = False
		Me.txtFolder.Name = "txtFolder"
		Me.txtFolder.ReadOnly = False
		Me.txtFolder.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFolder.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFolder.Size = New System.Drawing.Size(121, 19)
		Me.txtFolder.TabIndex = 4
		Me.txtFolder.TabStop = True
		Me.txtFolder.Text = "C:\scratch"
		Me.txtFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFolder.Visible = True
		' 
		' cmdConvert
		' 
		Me.cmdConvert.BackColor = System.Drawing.SystemColors.Control
		Me.cmdConvert.CausesValidation = True
		Me.cmdConvert.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdConvert.Enabled = False
		Me.cmdConvert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdConvert.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdConvert.Location = New System.Drawing.Point(472, 336)
		Me.cmdConvert.Name = "cmdConvert"
		Me.cmdConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdConvert.Size = New System.Drawing.Size(97, 25)
		Me.cmdConvert.TabIndex = 1
		Me.cmdConvert.TabStop = True
		Me.cmdConvert.Text = "Convert"
		Me.cmdConvert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lstLog
		' 
		Me.lstLog.BackColor = System.Drawing.SystemColors.Window
		Me.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstLog.CausesValidation = True
		Me.lstLog.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstLog.Enabled = True
		Me.lstLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstLog.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstLog.IntegralHeight = True
		Me.lstLog.Location = New System.Drawing.Point(8, 112)
		Me.lstLog.MultiColumn = False
		Me.lstLog.Name = "lstLog"
		Me.lstLog.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstLog.Size = New System.Drawing.Size(561, 215)
		Me.lstLog.Sorted = False
		Me.lstLog.TabIndex = 0
		Me.lstLog.TabStop = True
		Me.lstLog.Visible = True
		' 
		' Label6
		' 
		Me.Label6.AutoSize = True
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.Enabled = True
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Location = New System.Drawing.Point(376, 51)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(46, 13)
		Me.Label6.TabIndex = 11
		Me.Label6.Text = "Database"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		' 
		' Label5
		' 
		Me.Label5.AutoSize = True
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Location = New System.Drawing.Point(376, 83)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(46, 13)
		Me.Label5.TabIndex = 10
		Me.Label5.Text = "Password"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = True
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(208, 83)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(22, 13)
		Me.Label4.TabIndex = 8
		Me.Label4.Text = "User"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = True
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(16, 84)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(31, 13)
		Me.Label3.TabIndex = 6
		Me.Label3.Text = "Server"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(16, 51)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(29, 13)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Folder"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(16, 20)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(52, 13)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Process ID"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmMain
		' 
		Me.AcceptButton = Me.Command1
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(578, 400)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdConvertAll)
		Me.Controls.Add(Me.chkClipboard)
		Me.Controls.Add(Me.cboDatabase)
		Me.Controls.Add(Me.chkDebug)
		Me.Controls.Add(Me.cboServer)
		Me.Controls.Add(Me.Command1)
		Me.Controls.Add(Me.txtPassword)
		Me.Controls.Add(Me.txtUser)
		Me.Controls.Add(Me.cboProcessID)
		Me.Controls.Add(Me.txtFolder)
		Me.Controls.Add(Me.cmdConvert)
		Me.Controls.Add(Me.lstLog)
		Me.Controls.Add(Me.Label6)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "XMConvertor"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstLog, System.Windows.Forms.SelectionMode.One)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class