<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public CommonDialog1Save As System.Windows.Forms.SaveFileDialog
	Public CommonDialog1Font As System.Windows.Forms.FontDialog
	Public CommonDialog1Color As System.Windows.Forms.ColorDialog
	Public CommonDialog1Print As System.Windows.Forms.PrintDialog
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents txtServer As System.Windows.Forms.TextBox
	Public WithEvents txtDatabase As System.Windows.Forms.TextBox
	Public WithEvents txtUsername As System.Windows.Forms.TextBox
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents cmdProcess As System.Windows.Forms.Button
	Public WithEvents txtProvider As System.Windows.Forms.TextBox
	Public WithEvents txtMsg As System.Windows.Forms.RichTextBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.stbStatus = New System.Windows.Forms.StatusStrip
		Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
		Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
		Me.CommonDialog1Save = New System.Windows.Forms.SaveFileDialog
		Me.CommonDialog1Font = New System.Windows.Forms.FontDialog
		Me.CommonDialog1Color = New System.Windows.Forms.ColorDialog
		Me.CommonDialog1Print = New System.Windows.Forms.PrintDialog
		Me.cmdSave = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.Label1 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.txtServer = New System.Windows.Forms.TextBox
		Me.txtDatabase = New System.Windows.Forms.TextBox
		Me.txtUsername = New System.Windows.Forms.TextBox
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.cmdProcess = New System.Windows.Forms.Button
		Me.txtProvider = New System.Windows.Forms.TextBox
		Me.txtMsg = New System.Windows.Forms.RichTextBox
		Me.stbStatus.SuspendLayout()
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' stbStatus
		' 
		Me.stbStatus.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.stbStatus.Location = New System.Drawing.Point(0, 423)
		Me.stbStatus.Name = "stbStatus"
		Me.stbStatus.ShowItemToolTips = True
		Me.stbStatus.Size = New System.Drawing.Size(422, 17)
		Me.stbStatus.TabIndex = 16
		Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me._stbStatus_Panel1})
		' 
		' _stbStatus_Panel1
		' 
		Me._stbStatus_Panel1.AutoSize = True
		Me._stbStatus_Panel1.AutoSize = False
		Me._stbStatus_Panel1.BorderSides = CType(System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom, System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
		Me._stbStatus_Panel1.DoubleClickEnabled = True
		Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
		Me._stbStatus_Panel1.Size = New System.Drawing.Size(404, 17)
		Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._stbStatus_Panel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
		' 
		' cmdSave
		' 
		Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSave.CausesValidation = True
		Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSave.Enabled = True
		Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSave.Location = New System.Drawing.Point(8, 396)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSave.Size = New System.Drawing.Size(69, 25)
		Me.cmdSave.TabIndex = 14
		Me.cmdSave.TabStop = True
		Me.cmdSave.Text = "Save log"
		Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(346, 396)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(69, 25)
		Me.cmdExit.TabIndex = 6
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(134, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(411, 385)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 7
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.Label1)
		Me._tabMain_TabPage0.Controls.Add(Me.Label2)
		Me._tabMain_TabPage0.Controls.Add(Me.Label3)
		Me._tabMain_TabPage0.Controls.Add(Me.Label4)
		Me._tabMain_TabPage0.Controls.Add(Me.Label5)
		Me._tabMain_TabPage0.Controls.Add(Me.Label6)
		Me._tabMain_TabPage0.Controls.Add(Me.txtServer)
		Me._tabMain_TabPage0.Controls.Add(Me.txtDatabase)
		Me._tabMain_TabPage0.Controls.Add(Me.txtUsername)
		Me._tabMain_TabPage0.Controls.Add(Me.txtPassword)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdProcess)
		Me._tabMain_TabPage0.Controls.Add(Me.txtProvider)
		Me._tabMain_TabPage0.Controls.Add(Me.txtMsg)
		Me._tabMain_TabPage0.Text = "Claims Folders"
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
		Me.Label1.Location = New System.Drawing.Point(20, 48)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(53, 13)
		Me.Label1.TabIndex = 8
		Me.Label1.Text = "Server"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(20, 72)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(57, 13)
		Me.Label2.TabIndex = 9
		Me.Label2.Text = "Database"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(20, 120)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(61, 13)
		Me.Label3.TabIndex = 10
		Me.Label3.Text = "Username"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = False
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(20, 144)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(65, 13)
		Me.Label4.TabIndex = 11
		Me.Label4.Text = "Password"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Label5
		' 
		Me.Label5.AutoSize = False
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Location = New System.Drawing.Point(20, 96)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(61, 13)
		Me.Label5.TabIndex = 12
		Me.Label5.Text = "Provider"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Label6
		' 
		Me.Label6.AutoSize = False
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.Enabled = True
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Location = New System.Drawing.Point(20, 12)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(361, 29)
		Me.Label6.TabIndex = 13
		Me.Label6.Text = "The database used by Sirius has been selected by default. You can change the following values to fix claims folders on another database if required."
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		' 
		' txtServer
		' 
		Me.txtServer.AcceptsReturn = True
		Me.txtServer.AutoSize = False
		Me.txtServer.BackColor = System.Drawing.SystemColors.Window
		Me.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtServer.CausesValidation = True
		Me.txtServer.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtServer.Enabled = True
		Me.txtServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtServer.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtServer.HideSelection = True
		Me.txtServer.Location = New System.Drawing.Point(104, 48)
		Me.txtServer.MaxLength = 0
		Me.txtServer.Multiline = False
		Me.txtServer.Name = "txtServer"
		Me.txtServer.ReadOnly = False
		Me.txtServer.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtServer.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtServer.Size = New System.Drawing.Size(189, 19)
		Me.txtServer.TabIndex = 0
		Me.txtServer.TabStop = True
		Me.txtServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtServer.Visible = True
		' 
		' txtDatabase
		' 
		Me.txtDatabase.AcceptsReturn = True
		Me.txtDatabase.AutoSize = False
		Me.txtDatabase.BackColor = System.Drawing.SystemColors.Window
		Me.txtDatabase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDatabase.CausesValidation = True
		Me.txtDatabase.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDatabase.Enabled = True
		Me.txtDatabase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDatabase.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDatabase.HideSelection = True
		Me.txtDatabase.Location = New System.Drawing.Point(104, 72)
		Me.txtDatabase.MaxLength = 0
		Me.txtDatabase.Multiline = False
		Me.txtDatabase.Name = "txtDatabase"
		Me.txtDatabase.ReadOnly = False
		Me.txtDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDatabase.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDatabase.Size = New System.Drawing.Size(189, 19)
		Me.txtDatabase.TabIndex = 1
		Me.txtDatabase.TabStop = True
		Me.txtDatabase.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDatabase.Visible = True
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
		Me.txtUsername.Location = New System.Drawing.Point(104, 120)
		Me.txtUsername.MaxLength = 0
		Me.txtUsername.Multiline = False
		Me.txtUsername.Name = "txtUsername"
		Me.txtUsername.ReadOnly = False
		Me.txtUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtUsername.Size = New System.Drawing.Size(189, 19)
		Me.txtUsername.TabIndex = 3
		Me.txtUsername.TabStop = True
		Me.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtUsername.Visible = True
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
		Me.txtPassword.Location = New System.Drawing.Point(104, 144)
		Me.txtPassword.MaxLength = 0
		Me.txtPassword.Multiline = False
		Me.txtPassword.Name = "txtPassword"
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.Size = New System.Drawing.Size(189, 19)
		Me.txtPassword.TabIndex = 4
		Me.txtPassword.TabStop = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.Visible = True
		' 
		' cmdProcess
		' 
		Me.cmdProcess.BackColor = System.Drawing.SystemColors.Control
		Me.cmdProcess.CausesValidation = True
		Me.cmdProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdProcess.Enabled = True
		Me.cmdProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdProcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdProcess.Location = New System.Drawing.Point(308, 128)
		Me.cmdProcess.Name = "cmdProcess"
		Me.cmdProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdProcess.Size = New System.Drawing.Size(89, 33)
		Me.cmdProcess.TabIndex = 5
		Me.cmdProcess.TabStop = True
		Me.cmdProcess.Text = "Process claims folders"
		Me.cmdProcess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdProcess.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtProvider
		' 
		Me.txtProvider.AcceptsReturn = True
		Me.txtProvider.AutoSize = False
		Me.txtProvider.BackColor = System.Drawing.SystemColors.Window
		Me.txtProvider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtProvider.CausesValidation = True
		Me.txtProvider.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtProvider.Enabled = True
		Me.txtProvider.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtProvider.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtProvider.HideSelection = True
		Me.txtProvider.ImeMode = System.Windows.Forms.ImeMode.Disable
		Me.txtProvider.Location = New System.Drawing.Point(104, 96)
		Me.txtProvider.MaxLength = 0
		Me.txtProvider.Multiline = False
		Me.txtProvider.Name = "txtProvider"
		Me.txtProvider.ReadOnly = False
		Me.txtProvider.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtProvider.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtProvider.Size = New System.Drawing.Size(189, 19)
		Me.txtProvider.TabIndex = 2
		Me.txtProvider.TabStop = True
		Me.txtProvider.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtProvider.Visible = True
		' 
		' txtMsg
		' 
		Me.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtMsg.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtMsg.Location = New System.Drawing.Point(8, 172)
		Me.txtMsg.Name = "txtMsg"
		Me.txtMsg.RTF = resources.GetString("txtMsg.TextRTF")
		Me.txtMsg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
		Me.txtMsg.Size = New System.Drawing.Size(389, 181)
		Me.txtMsg.TabIndex = 15
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(422, 440)
		Me.ControlBox = True
		Me.Controls.Add(Me.stbStatus)
		Me.Controls.Add(Me.cmdSave)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DME Claims Folders"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.stbStatus.ResumeLayout(False)
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class