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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents chkQuick As System.Windows.Forms.CheckBox
	Public WithEvents chkLogExists As System.Windows.Forms.CheckBox
	Public WithEvents chkLogMissing As System.Windows.Forms.CheckBox
	Public WithEvents fraFile As System.Windows.Forms.GroupBox
	Public WithEvents chkScript1 As System.Windows.Forms.CheckBox
	Public WithEvents chkScript2 As System.Windows.Forms.CheckBox
	Public WithEvents chkScript3 As System.Windows.Forms.CheckBox
	Public WithEvents chkScript4 As System.Windows.Forms.CheckBox
	Public WithEvents fraDB As System.Windows.Forms.GroupBox
	Public WithEvents chkFiles As System.Windows.Forms.CheckBox
	Public WithEvents chkDB As System.Windows.Forms.CheckBox
	Public WithEvents cmdSetMainDefaults As System.Windows.Forms.Button
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtUsername As System.Windows.Forms.TextBox
	Public WithEvents chkWindowsAuth As System.Windows.Forms.CheckBox
	Public WithEvents cmdDatabaseRefresh As System.Windows.Forms.Button
	Public WithEvents cmdServerRefresh As System.Windows.Forms.Button
	Public WithEvents cboDatabase As System.Windows.Forms.ComboBox
	Public WithEvents cboServer As System.Windows.Forms.ComboBox
	Public WithEvents txtOutputFile As System.Windows.Forms.TextBox
	Public WithEvents cmdSetDefaults As System.Windows.Forms.Button
	Public WithEvents txtLogFile As System.Windows.Forms.TextBox
	Public WithEvents cboVersion As System.Windows.Forms.ComboBox
	Public WithEvents txtPath As System.Windows.Forms.TextBox
	Public WithEvents lblPassword As System.Windows.Forms.Label
	Public WithEvents lblUsername As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents lblLogFile As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents fmeAdvanced As System.Windows.Forms.GroupBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents lstStatus As System.Windows.Forms.ListBox
	Public WithEvents fraStatus As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdRun As System.Windows.Forms.Button
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.fraFile = New System.Windows.Forms.GroupBox
		Me.chkQuick = New System.Windows.Forms.CheckBox
		Me.chkLogExists = New System.Windows.Forms.CheckBox
		Me.chkLogMissing = New System.Windows.Forms.CheckBox
		Me.fraDB = New System.Windows.Forms.GroupBox
		Me.chkScript1 = New System.Windows.Forms.CheckBox
		Me.chkScript2 = New System.Windows.Forms.CheckBox
		Me.chkScript3 = New System.Windows.Forms.CheckBox
		Me.chkScript4 = New System.Windows.Forms.CheckBox
		Me.chkFiles = New System.Windows.Forms.CheckBox
		Me.chkDB = New System.Windows.Forms.CheckBox
		Me.cmdSetMainDefaults = New System.Windows.Forms.Button
		Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage
		Me.fmeAdvanced = New System.Windows.Forms.GroupBox
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtUsername = New System.Windows.Forms.TextBox
		Me.chkWindowsAuth = New System.Windows.Forms.CheckBox
		Me.cmdDatabaseRefresh = New System.Windows.Forms.Button
		Me.cmdServerRefresh = New System.Windows.Forms.Button
		Me.cboDatabase = New System.Windows.Forms.ComboBox
		Me.cboServer = New System.Windows.Forms.ComboBox
		Me.txtOutputFile = New System.Windows.Forms.TextBox
		Me.cmdSetDefaults = New System.Windows.Forms.Button
		Me.txtLogFile = New System.Windows.Forms.TextBox
		Me.cboVersion = New System.Windows.Forms.ComboBox
		Me.txtPath = New System.Windows.Forms.TextBox
		Me.lblPassword = New System.Windows.Forms.Label
		Me.lblUsername = New System.Windows.Forms.Label
		Me.Label8 = New System.Windows.Forms.Label
		Me.Label7 = New System.Windows.Forms.Label
		Me.lblLogFile = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.fraStatus = New System.Windows.Forms.GroupBox
		Me.lstStatus = New System.Windows.Forms.ListBox
		Me.cmdExit = New System.Windows.Forms.Button
		Me.cmdRun = New System.Windows.Forms.Button
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.fraFile.SuspendLayout()
		Me.fraDB.SuspendLayout()
		Me._tabMain_TabPage1.SuspendLayout()
		Me.fmeAdvanced.SuspendLayout()
		Me.fraStatus.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
		Me.tabMain.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(160, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(487, 333)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 26
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.fraFile)
		Me._tabMain_TabPage0.Controls.Add(Me.fraDB)
		Me._tabMain_TabPage0.Controls.Add(Me.chkFiles)
		Me._tabMain_TabPage0.Controls.Add(Me.chkDB)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdSetMainDefaults)
		Me._tabMain_TabPage0.Text = "&1 - Main"
		' 
		' fraFile
		' 
		Me.fraFile.BackColor = System.Drawing.SystemColors.Control
		Me.fraFile.Controls.Add(Me.chkQuick)
		Me.fraFile.Controls.Add(Me.chkLogExists)
		Me.fraFile.Controls.Add(Me.chkLogMissing)
		Me.fraFile.Enabled = True
		Me.fraFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraFile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraFile.Location = New System.Drawing.Point(8, 36)
		Me.fraFile.Name = "fraFile"
		Me.fraFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraFile.Size = New System.Drawing.Size(465, 89)
		Me.fraFile.TabIndex = 28
		Me.fraFile.Text = "Missing File Check Options (1.6 and 1.8.6+)"
		Me.fraFile.Visible = True
		' 
		' chkQuick
		' 
		Me.chkQuick.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkQuick.BackColor = System.Drawing.SystemColors.Control
		Me.chkQuick.CausesValidation = True
		Me.chkQuick.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkQuick.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkQuick.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkQuick.Enabled = True
		Me.chkQuick.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkQuick.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkQuick.Location = New System.Drawing.Point(16, 64)
		Me.chkQuick.Name = "chkQuick"
		Me.chkQuick.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkQuick.Size = New System.Drawing.Size(409, 17)
		Me.chkQuick.TabIndex = 3
		Me.chkQuick.TabStop = True
		Me.chkQuick.Text = "Only check 1 page per document. For quicker (but less thorough) checking"
		Me.chkQuick.Visible = True
		' 
		' chkLogExists
		' 
		Me.chkLogExists.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkLogExists.BackColor = System.Drawing.SystemColors.Control
		Me.chkLogExists.CausesValidation = True
		Me.chkLogExists.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkLogExists.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkLogExists.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkLogExists.Enabled = True
		Me.chkLogExists.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkLogExists.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkLogExists.Location = New System.Drawing.Point(16, 44)
		Me.chkLogExists.Name = "chkLogExists"
		Me.chkLogExists.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkLogExists.Size = New System.Drawing.Size(101, 17)
		Me.chkLogExists.TabIndex = 2
		Me.chkLogExists.TabStop = True
		Me.chkLogExists.Text = "Log existing files"
		Me.chkLogExists.Visible = True
		' 
		' chkLogMissing
		' 
		Me.chkLogMissing.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkLogMissing.BackColor = System.Drawing.SystemColors.Control
		Me.chkLogMissing.CausesValidation = True
		Me.chkLogMissing.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkLogMissing.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkLogMissing.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkLogMissing.Enabled = True
		Me.chkLogMissing.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkLogMissing.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkLogMissing.Location = New System.Drawing.Point(16, 24)
		Me.chkLogMissing.Name = "chkLogMissing"
		Me.chkLogMissing.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkLogMissing.Size = New System.Drawing.Size(101, 17)
		Me.chkLogMissing.TabIndex = 1
		Me.chkLogMissing.TabStop = True
		Me.chkLogMissing.Text = "Log missing files"
		Me.chkLogMissing.Visible = True
		' 
		' fraDB
		' 
		Me.fraDB.BackColor = System.Drawing.SystemColors.Control
		Me.fraDB.Controls.Add(Me.chkScript1)
		Me.fraDB.Controls.Add(Me.chkScript2)
		Me.fraDB.Controls.Add(Me.chkScript3)
		Me.fraDB.Controls.Add(Me.chkScript4)
		Me.fraDB.Enabled = True
		Me.fraDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDB.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDB.Location = New System.Drawing.Point(8, 156)
		Me.fraDB.Name = "fraDB"
		Me.fraDB.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDB.Size = New System.Drawing.Size(465, 109)
		Me.fraDB.TabIndex = 29
		Me.fraDB.Text = "Data Structure Check Options (1.8.6 only)"
		Me.fraDB.Visible = True
		' 
		' chkScript1
		' 
		Me.chkScript1.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkScript1.BackColor = System.Drawing.SystemColors.Control
		Me.chkScript1.CausesValidation = True
		Me.chkScript1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkScript1.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkScript1.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkScript1.Enabled = True
		Me.chkScript1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkScript1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkScript1.Location = New System.Drawing.Point(16, 24)
		Me.chkScript1.Name = "chkScript1"
		Me.chkScript1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkScript1.Size = New System.Drawing.Size(253, 17)
		Me.chkScript1.TabIndex = 5
		Me.chkScript1.TabStop = True
		Me.chkScript1.Text = "Folders with identical parent_num and ex_codes"
		Me.chkScript1.Visible = True
		' 
		' chkScript2
		' 
		Me.chkScript2.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkScript2.BackColor = System.Drawing.SystemColors.Control
		Me.chkScript2.CausesValidation = True
		Me.chkScript2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkScript2.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkScript2.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkScript2.Enabled = True
		Me.chkScript2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkScript2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkScript2.Location = New System.Drawing.Point(16, 44)
		Me.chkScript2.Name = "chkScript2"
		Me.chkScript2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkScript2.Size = New System.Drawing.Size(137, 17)
		Me.chkScript2.TabIndex = 6
		Me.chkScript2.TabStop = True
		Me.chkScript2.Text = "Missing ""Regarding Line"""
		Me.chkScript2.Visible = True
		' 
		' chkScript3
		' 
		Me.chkScript3.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkScript3.BackColor = System.Drawing.SystemColors.Control
		Me.chkScript3.CausesValidation = True
		Me.chkScript3.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkScript3.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkScript3.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkScript3.Enabled = True
		Me.chkScript3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkScript3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkScript3.Location = New System.Drawing.Point(16, 64)
		Me.chkScript3.Name = "chkScript3"
		Me.chkScript3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkScript3.Size = New System.Drawing.Size(201, 17)
		Me.chkScript3.TabIndex = 7
		Me.chkScript3.TabStop = True
		Me.chkScript3.Text = "Client folders with incorrect ex_codes"
		Me.chkScript3.Visible = True
		' 
		' chkScript4
		' 
		Me.chkScript4.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkScript4.BackColor = System.Drawing.SystemColors.Control
		Me.chkScript4.CausesValidation = True
		Me.chkScript4.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkScript4.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkScript4.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkScript4.Enabled = True
		Me.chkScript4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkScript4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkScript4.Location = New System.Drawing.Point(16, 84)
		Me.chkScript4.Name = "chkScript4"
		Me.chkScript4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkScript4.Size = New System.Drawing.Size(309, 17)
		Me.chkScript4.TabIndex = 8
		Me.chkScript4.TabStop = True
		Me.chkScript4.Text = "Duplicate client folders with identical parent and ex_codes"
		Me.chkScript4.Visible = True
		' 
		' chkFiles
		' 
		Me.chkFiles.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkFiles.BackColor = System.Drawing.SystemColors.Control
		Me.chkFiles.CausesValidation = True
		Me.chkFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkFiles.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkFiles.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkFiles.Enabled = True
		Me.chkFiles.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkFiles.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkFiles.Location = New System.Drawing.Point(16, 16)
		Me.chkFiles.Name = "chkFiles"
		Me.chkFiles.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkFiles.Size = New System.Drawing.Size(133, 17)
		Me.chkFiles.TabIndex = 0
		Me.chkFiles.TabStop = True
		Me.chkFiles.Text = "Check File Structure"
		Me.chkFiles.Visible = True
		' 
		' chkDB
		' 
		Me.chkDB.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDB.BackColor = System.Drawing.SystemColors.Control
		Me.chkDB.CausesValidation = True
		Me.chkDB.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkDB.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkDB.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDB.Enabled = True
		Me.chkDB.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDB.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDB.Location = New System.Drawing.Point(16, 136)
		Me.chkDB.Name = "chkDB"
		Me.chkDB.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDB.Size = New System.Drawing.Size(141, 17)
		Me.chkDB.TabIndex = 4
		Me.chkDB.TabStop = True
		Me.chkDB.Text = "Check Data Structure"
		Me.chkDB.Visible = True
		' 
		' cmdSetMainDefaults
		' 
		Me.cmdSetMainDefaults.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSetMainDefaults.CausesValidation = True
		Me.cmdSetMainDefaults.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSetMainDefaults.Enabled = True
		Me.cmdSetMainDefaults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSetMainDefaults.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSetMainDefaults.Location = New System.Drawing.Point(384, 276)
		Me.cmdSetMainDefaults.Name = "cmdSetMainDefaults"
		Me.cmdSetMainDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSetMainDefaults.Size = New System.Drawing.Size(89, 21)
		Me.cmdSetMainDefaults.TabIndex = 9
		Me.cmdSetMainDefaults.TabStop = True
		Me.cmdSetMainDefaults.Text = "&Set defaults"
		Me.cmdSetMainDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSetMainDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' _tabMain_TabPage1
		' 
		Me._tabMain_TabPage1.Controls.Add(Me.fmeAdvanced)
		Me._tabMain_TabPage1.Controls.Add(Me.Label1)
		Me._tabMain_TabPage1.Text = "&2 - Advanced"
		' 
		' fmeAdvanced
		' 
		Me.fmeAdvanced.BackColor = System.Drawing.SystemColors.Control
		Me.fmeAdvanced.Controls.Add(Me.txtPassword)
		Me.fmeAdvanced.Controls.Add(Me.txtUsername)
		Me.fmeAdvanced.Controls.Add(Me.chkWindowsAuth)
		Me.fmeAdvanced.Controls.Add(Me.cmdDatabaseRefresh)
		Me.fmeAdvanced.Controls.Add(Me.cmdServerRefresh)
		Me.fmeAdvanced.Controls.Add(Me.cboDatabase)
		Me.fmeAdvanced.Controls.Add(Me.cboServer)
		Me.fmeAdvanced.Controls.Add(Me.txtOutputFile)
		Me.fmeAdvanced.Controls.Add(Me.cmdSetDefaults)
		Me.fmeAdvanced.Controls.Add(Me.txtLogFile)
		Me.fmeAdvanced.Controls.Add(Me.cboVersion)
		Me.fmeAdvanced.Controls.Add(Me.txtPath)
		Me.fmeAdvanced.Controls.Add(Me.lblPassword)
		Me.fmeAdvanced.Controls.Add(Me.lblUsername)
		Me.fmeAdvanced.Controls.Add(Me.Label8)
		Me.fmeAdvanced.Controls.Add(Me.Label7)
		Me.fmeAdvanced.Controls.Add(Me.lblLogFile)
		Me.fmeAdvanced.Controls.Add(Me.Label4)
		Me.fmeAdvanced.Controls.Add(Me.Label5)
		Me.fmeAdvanced.Controls.Add(Me.Label6)
		Me.fmeAdvanced.Enabled = True
		Me.fmeAdvanced.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fmeAdvanced.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fmeAdvanced.Location = New System.Drawing.Point(8, 44)
		Me.fmeAdvanced.Name = "fmeAdvanced"
		Me.fmeAdvanced.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fmeAdvanced.Size = New System.Drawing.Size(465, 245)
		Me.fmeAdvanced.TabIndex = 31
		Me.fmeAdvanced.Visible = True
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
		Me.txtPassword.Location = New System.Drawing.Point(108, 92)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(85, 19)
		Me.txtPassword.TabIndex = 14
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' txtUsername
		' 
		Me.txtUsername.AcceptsReturn = True
		Me.txtUsername.AutoSize = False
		Me.txtUsername.BackColor = System.Drawing.SystemColors.Window
		Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtUsername.CausesValidation = True
		Me.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtUsername.Enabled = True
		Me.txtUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtUsername.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtUsername.HideSelection = True
		Me.txtUsername.Location = New System.Drawing.Point(108, 68)
		Me.txtUsername.MaxLength = 0
		Me.txtUsername.Multiline = False
		Me.txtUsername.Name = "txtUsername"
		Me.txtUsername.ReadOnly = False
		Me.txtUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUsername.Size = New System.Drawing.Size(85, 19)
		Me.txtUsername.TabIndex = 12
		Me.txtUsername.TabStop = True
		Me.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUsername.Visible = True
		' 
		' chkWindowsAuth
		' 
		Me.chkWindowsAuth.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkWindowsAuth.BackColor = System.Drawing.SystemColors.Control
		Me.chkWindowsAuth.CausesValidation = True
		Me.chkWindowsAuth.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkWindowsAuth.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkWindowsAuth.Enabled = False
		Me.chkWindowsAuth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkWindowsAuth.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkWindowsAuth.Location = New System.Drawing.Point(216, 68)
		Me.chkWindowsAuth.Name = "chkWindowsAuth"
		Me.chkWindowsAuth.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkWindowsAuth.Size = New System.Drawing.Size(141, 21)
		Me.chkWindowsAuth.TabIndex = 13
		Me.chkWindowsAuth.TabStop = True
		Me.chkWindowsAuth.Text = "Windows Authentication"
		Me.chkWindowsAuth.Visible = True
		' 
		' cmdDatabaseRefresh
		' 
		Me.cmdDatabaseRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDatabaseRefresh.CausesValidation = True
		Me.cmdDatabaseRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDatabaseRefresh.Enabled = True
		Me.cmdDatabaseRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDatabaseRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDatabaseRefresh.Location = New System.Drawing.Point(320, 140)
		Me.cmdDatabaseRefresh.Name = "cmdDatabaseRefresh"
		Me.cmdDatabaseRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDatabaseRefresh.Size = New System.Drawing.Size(49, 21)
		Me.cmdDatabaseRefresh.TabIndex = 18
		Me.cmdDatabaseRefresh.TabStop = True
		Me.cmdDatabaseRefresh.Text = "Refresh"
		Me.cmdDatabaseRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDatabaseRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdServerRefresh
		' 
		Me.cmdServerRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdServerRefresh.CausesValidation = True
		Me.cmdServerRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdServerRefresh.Enabled = True
		Me.cmdServerRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdServerRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdServerRefresh.Location = New System.Drawing.Point(320, 116)
		Me.cmdServerRefresh.Name = "cmdServerRefresh"
		Me.cmdServerRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdServerRefresh.Size = New System.Drawing.Size(49, 21)
		Me.cmdServerRefresh.TabIndex = 16
		Me.cmdServerRefresh.TabStop = True
		Me.cmdServerRefresh.Text = "Refresh"
		Me.cmdServerRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdServerRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
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
		Me.cboDatabase.Location = New System.Drawing.Point(108, 140)
		Me.cboDatabase.Name = "cboDatabase"
		Me.cboDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDatabase.Size = New System.Drawing.Size(205, 21)
		Me.cboDatabase.Sorted = False
		Me.cboDatabase.TabIndex = 17
		Me.cboDatabase.TabStop = True
		Me.cboDatabase.Visible = True
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
		Me.cboServer.Location = New System.Drawing.Point(108, 116)
		Me.cboServer.Name = "cboServer"
		Me.cboServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboServer.Size = New System.Drawing.Size(205, 21)
		Me.cboServer.Sorted = False
		Me.cboServer.TabIndex = 15
		Me.cboServer.TabStop = True
		Me.cboServer.Visible = True
		' 
		' txtOutputFile
		' 
		Me.txtOutputFile.AcceptsReturn = True
		Me.txtOutputFile.AutoSize = False
		Me.txtOutputFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtOutputFile.CausesValidation = True
		Me.txtOutputFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtOutputFile.Enabled = False
		Me.txtOutputFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtOutputFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtOutputFile.HideSelection = True
		Me.txtOutputFile.Location = New System.Drawing.Point(108, 44)
		Me.txtOutputFile.MaxLength = 0
		Me.txtOutputFile.Multiline = False
		Me.txtOutputFile.Name = "txtOutputFile"
		Me.txtOutputFile.ReadOnly = True
		Me.txtOutputFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtOutputFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtOutputFile.Size = New System.Drawing.Size(261, 19)
		Me.txtOutputFile.TabIndex = 11
		Me.txtOutputFile.TabStop = True
		Me.txtOutputFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtOutputFile.Visible = True
		' 
		' cmdSetDefaults
		' 
		Me.cmdSetDefaults.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSetDefaults.CausesValidation = True
		Me.cmdSetDefaults.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSetDefaults.Enabled = True
		Me.cmdSetDefaults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSetDefaults.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSetDefaults.Location = New System.Drawing.Point(364, 216)
		Me.cmdSetDefaults.Name = "cmdSetDefaults"
		Me.cmdSetDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSetDefaults.Size = New System.Drawing.Size(89, 21)
		Me.cmdSetDefaults.TabIndex = 21
		Me.cmdSetDefaults.TabStop = True
		Me.cmdSetDefaults.Text = "&Set defaults"
		Me.cmdSetDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSetDefaults.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtLogFile
		' 
		Me.txtLogFile.AcceptsReturn = True
		Me.txtLogFile.AutoSize = False
		Me.txtLogFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtLogFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLogFile.CausesValidation = True
		Me.txtLogFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLogFile.Enabled = True
		Me.txtLogFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLogFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLogFile.HideSelection = True
		Me.txtLogFile.Location = New System.Drawing.Point(108, 20)
		Me.txtLogFile.MaxLength = 0
		Me.txtLogFile.Multiline = False
		Me.txtLogFile.Name = "txtLogFile"
		Me.txtLogFile.ReadOnly = False
		Me.txtLogFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLogFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLogFile.Size = New System.Drawing.Size(261, 19)
		Me.txtLogFile.TabIndex = 10
		Me.txtLogFile.TabStop = True
		Me.txtLogFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLogFile.Visible = True
		' 
		' cboVersion
		' 
		Me.cboVersion.BackColor = System.Drawing.SystemColors.Window
		Me.cboVersion.CausesValidation = True
		Me.cboVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboVersion.Enabled = True
		Me.cboVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboVersion.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboVersion.IntegralHeight = True
		Me.cboVersion.Location = New System.Drawing.Point(108, 164)
		Me.cboVersion.Name = "cboVersion"
		Me.cboVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboVersion.Size = New System.Drawing.Size(169, 21)
		Me.cboVersion.Sorted = False
		Me.cboVersion.TabIndex = 19
		Me.cboVersion.TabStop = True
		Me.cboVersion.Visible = True
		' 
		' txtPath
		' 
		Me.txtPath.AcceptsReturn = True
		Me.txtPath.AutoSize = False
		Me.txtPath.BackColor = System.Drawing.SystemColors.Window
		Me.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPath.CausesValidation = True
		Me.txtPath.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPath.Enabled = False
		Me.txtPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPath.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPath.HideSelection = True
		Me.txtPath.Location = New System.Drawing.Point(108, 192)
		Me.txtPath.MaxLength = 0
		Me.txtPath.Multiline = False
		Me.txtPath.Name = "txtPath"
		Me.txtPath.ReadOnly = True
		Me.txtPath.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPath.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPath.Size = New System.Drawing.Size(345, 19)
		Me.txtPath.TabIndex = 20
		Me.txtPath.TabStop = True
		Me.txtPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPath.Visible = True
		' 
		' lblPassword
		' 
		Me.lblPassword.AutoSize = True
		Me.lblPassword.BackColor = System.Drawing.SystemColors.Control
		Me.lblPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.Enabled = True
		Me.lblPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Location = New System.Drawing.Point(12, 92)
		Me.lblPassword.Name = "lblPassword"
		Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPassword.Size = New System.Drawing.Size(72, 13)
		Me.lblPassword.TabIndex = 39
		Me.lblPassword.Text = "SQL password:"
		Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPassword.UseMnemonic = True
		Me.lblPassword.Visible = True
		' 
		' lblUsername
		' 
		Me.lblUsername.AutoSize = True
		Me.lblUsername.BackColor = System.Drawing.SystemColors.Control
		Me.lblUsername.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUsername.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUsername.Enabled = True
		Me.lblUsername.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUsername.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUsername.Location = New System.Drawing.Point(12, 68)
		Me.lblUsername.Name = "lblUsername"
		Me.lblUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUsername.Size = New System.Drawing.Size(73, 13)
		Me.lblUsername.TabIndex = 38
		Me.lblUsername.Text = "SQL username:"
		Me.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUsername.UseMnemonic = True
		Me.lblUsername.Visible = True
		' 
		' Label8
		' 
		Me.Label8.AutoSize = True
		Me.Label8.BackColor = System.Drawing.SystemColors.Control
		Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.Enabled = True
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label8.Location = New System.Drawing.Point(12, 140)
		Me.Label8.Name = "Label8"
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.Size = New System.Drawing.Size(50, 13)
		Me.Label8.TabIndex = 37
		Me.Label8.Text = "Database:"
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label8.UseMnemonic = True
		Me.Label8.Visible = True
		' 
		' Label7
		' 
		Me.Label7.AutoSize = True
		Me.Label7.BackColor = System.Drawing.SystemColors.Control
		Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.Enabled = True
		Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label7.Location = New System.Drawing.Point(12, 44)
		Me.Label7.Name = "Label7"
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.Size = New System.Drawing.Size(75, 13)
		Me.Label7.TabIndex = 36
		Me.Label7.Text = "SQL output file:"
		Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label7.UseMnemonic = True
		Me.Label7.Visible = True
		' 
		' lblLogFile
		' 
		Me.lblLogFile.AutoSize = True
		Me.lblLogFile.BackColor = System.Drawing.SystemColors.Control
		Me.lblLogFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLogFile.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLogFile.Enabled = True
		Me.lblLogFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLogFile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLogFile.Location = New System.Drawing.Point(12, 20)
		Me.lblLogFile.Name = "lblLogFile"
		Me.lblLogFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLogFile.Size = New System.Drawing.Size(38, 13)
		Me.lblLogFile.TabIndex = 35
		Me.lblLogFile.Text = "Log file:"
		Me.lblLogFile.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLogFile.UseMnemonic = True
		Me.lblLogFile.Visible = True
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
		Me.Label4.Location = New System.Drawing.Point(12, 116)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(58, 13)
		Me.Label4.TabIndex = 34
		Me.Label4.Text = "SQL Server:"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
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
		Me.Label5.Location = New System.Drawing.Point(12, 164)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(39, 13)
		Me.Label5.TabIndex = 33
		Me.Label5.Text = "Version:"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
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
		Me.Label6.Location = New System.Drawing.Point(12, 192)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(73, 13)
		Me.Label6.TabIndex = 32
		Me.Label6.Text = "Path to scripts:"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(43, 20)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(395, 13)
		Me.Label1.TabIndex = 30
		Me.Label1.Text = "Do not change these values unless Sirius Support provide instructions"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(304, 464)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(89, 25)
		Me.cmdHelp.TabIndex = 23
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraStatus
		' 
		Me.fraStatus.BackColor = System.Drawing.SystemColors.Control
		Me.fraStatus.Controls.Add(Me.lstStatus)
		Me.fraStatus.Enabled = True
		Me.fraStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraStatus.Location = New System.Drawing.Point(8, 344)
		Me.fraStatus.Name = "fraStatus"
		Me.fraStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraStatus.Size = New System.Drawing.Size(481, 113)
		Me.fraStatus.TabIndex = 27
		Me.fraStatus.Text = "Status"
		Me.fraStatus.Visible = True
		' 
		' lstStatus
		' 
		Me.lstStatus.BackColor = System.Drawing.SystemColors.Window
		Me.lstStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstStatus.CausesValidation = True
		Me.lstStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstStatus.Enabled = True
		Me.lstStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstStatus.IntegralHeight = True
		Me.lstStatus.Location = New System.Drawing.Point(8, 16)
		Me.lstStatus.MultiColumn = False
		Me.lstStatus.Name = "lstStatus"
		Me.lstStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstStatus.Size = New System.Drawing.Size(465, 85)
		Me.lstStatus.Sorted = False
		Me.lstStatus.TabIndex = 25
		Me.lstStatus.TabStop = True
		Me.lstStatus.Visible = True
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(400, 464)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(89, 25)
		Me.cmdExit.TabIndex = 24
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "E&xit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRun
		' 
		Me.cmdRun.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRun.CausesValidation = True
		Me.cmdRun.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRun.Enabled = True
		Me.cmdRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRun.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRun.Location = New System.Drawing.Point(8, 464)
		Me.cmdRun.Name = "cmdRun"
		Me.cmdRun.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRun.Size = New System.Drawing.Size(89, 25)
		Me.cmdRun.TabIndex = 22
		Me.cmdRun.TabStop = True
		Me.cmdRun.Text = "&Run"
		Me.cmdRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmMain
		' 
		Me.AcceptButton = Me.cmdExit
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdExit
		Me.ClientSize = New System.Drawing.Size(498, 499)
		Me.ControlBox = True
		Me.Controls.Add(Me.tabMain)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.fraStatus)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.cmdRun)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocuMaster Enterprise Harmoniser"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstStatus, System.Windows.Forms.SelectionMode.One)
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 2)
		Me.ToolTip1.SetToolTip(Me.txtOutputFile, "Read-only. Depends on log file name")
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.fraFile.ResumeLayout(False)
		Me.fraDB.ResumeLayout(False)
		Me._tabMain_TabPage1.ResumeLayout(False)
		Me.fmeAdvanced.ResumeLayout(False)
		Me.fraStatus.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class