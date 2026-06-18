<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDMSMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents mnuLogsView As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuLogsPurge As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuLogs As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuLine1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuFileOptions As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep4 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuAccelerateAPI As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep2 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuErrorsRetryImport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuErrorsRetryExport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuErrors As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents drvMDI As Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
	Public WithEvents tmrDMSAPI As System.Windows.Forms.Timer
	Public WithEvents cmdRunNow As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtTimerInterval As System.Windows.Forms.TextBox
	Public WithEvents pan3TimerInterval As System.Windows.Forms.Panel
	Public WithEvents lblMinutes As System.Windows.Forms.Label
	Public WithEvents fra3JournalPro As System.Windows.Forms.GroupBox
	Public WithEvents pan3JournalPro As System.Windows.Forms.Panel
	Public WithEvents pan3PasswordStatus As System.Windows.Forms.Panel
	Public WithEvents pan3StatusBar As System.Windows.Forms.Panel
	Public cdbDDBOpen As System.Windows.Forms.OpenFileDialog
	Public cdbDDBSave As System.Windows.Forms.SaveFileDialog
	Public cdbDDBFont As System.Windows.Forms.FontDialog
	Public cdbDDBColor As System.Windows.Forms.ColorDialog
	Public cdbDDBPrint As System.Windows.Forms.PrintDialog
	Public WithEvents imgMedia As System.Windows.Forms.PictureBox
	Public WithEvents imgProcess As System.Windows.Forms.PictureBox
	Public WithEvents imgProcess2 As System.Windows.Forms.PictureBox
	Public WithEvents imgProcess1 As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDMSMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuLogs = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuLogsView = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuLogsPurge = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuLine1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFileOptions = New System.Windows.Forms.ToolStripMenuItem
		Me.sep4 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuAccelerateAPI = New System.Windows.Forms.ToolStripMenuItem
		Me.sep2 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuErrors = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuErrorsRetryImport = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuErrorsRetryExport = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
		Me.drvMDI = New Microsoft.VisualBasic.Compatibility.VB6.DriveListBox
		Me.tmrDMSAPI = New System.Windows.Forms.Timer(components)
		Me.pan3JournalPro = New System.Windows.Forms.Panel
		Me.cmdRunNow = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fra3JournalPro = New System.Windows.Forms.GroupBox
		Me.pan3TimerInterval = New System.Windows.Forms.Panel
		Me.txtTimerInterval = New System.Windows.Forms.TextBox
		Me.lblMinutes = New System.Windows.Forms.Label
		Me.pan3PasswordStatus = New System.Windows.Forms.Panel
		Me.pan3StatusBar = New System.Windows.Forms.Panel
		Me.cdbDDBOpen = New System.Windows.Forms.OpenFileDialog
		Me.cdbDDBSave = New System.Windows.Forms.SaveFileDialog
		Me.cdbDDBFont = New System.Windows.Forms.FontDialog
		Me.cdbDDBColor = New System.Windows.Forms.ColorDialog
		Me.cdbDDBPrint = New System.Windows.Forms.PrintDialog
		Me.imgMedia = New System.Windows.Forms.PictureBox
		Me.imgProcess = New System.Windows.Forms.PictureBox
		Me.imgProcess2 = New System.Windows.Forms.PictureBox
		Me.imgProcess1 = New System.Windows.Forms.PictureBox
		Me.pan3JournalPro.SuspendLayout()
		Me.fra3JournalPro.SuspendLayout()
		Me.pan3TimerInterval.SuspendLayout()
		Me.SuspendLayout()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile, Me.mnuErrors, Me.mnuHelp})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuLogs, Me.mnuLine1, Me.mnuFileOptions, Me.sep4, Me.mnuAccelerateAPI, Me.sep2, Me.mnuFileExit})
		' 
		' mnuLogs
		' 
		Me.mnuLogs.Available = True
		Me.mnuLogs.Checked = False
		Me.mnuLogs.Enabled = True
		Me.mnuLogs.Name = "mnuLogs"
		Me.mnuLogs.Text = "&Logs"
		Me.mnuLogs.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuLogsView, Me.mnuLogsPurge})
		' 
		' mnuLogsView
		' 
		Me.mnuLogsView.Available = True
		Me.mnuLogsView.Checked = False
		Me.mnuLogsView.Enabled = True
		Me.mnuLogsView.Name = "mnuLogsView"
		Me.mnuLogsView.Text = "&View..."
		' 
		' mnuLogsPurge
		' 
		Me.mnuLogsPurge.Available = True
		Me.mnuLogsPurge.Checked = False
		Me.mnuLogsPurge.Enabled = True
		Me.mnuLogsPurge.Name = "mnuLogsPurge"
		Me.mnuLogsPurge.Text = "&Purge..."
		' 
		' mnuLine1
		' 
		Me.mnuLine1.Available = True
		Me.mnuLine1.Enabled = True
		Me.mnuLine1.Name = "mnuLine1"
		' 
		' mnuFileOptions
		' 
		Me.mnuFileOptions.Available = True
		Me.mnuFileOptions.Checked = False
		Me.mnuFileOptions.Enabled = True
		Me.mnuFileOptions.Name = "mnuFileOptions"
		Me.mnuFileOptions.Text = "&Options..."
		' 
		' sep4
		' 
		Me.sep4.Available = True
		Me.sep4.Enabled = True
		Me.sep4.Name = "sep4"
		' 
		' mnuAccelerateAPI
		' 
		Me.mnuAccelerateAPI.Available = False
		Me.mnuAccelerateAPI.Checked = False
		Me.mnuAccelerateAPI.Enabled = True
		Me.mnuAccelerateAPI.Name = "mnuAccelerateAPI"
		Me.mnuAccelerateAPI.Text = "Accelerate AddIndex"
		' 
		' sep2
		' 
		Me.sep2.Available = False
		Me.sep2.Enabled = True
		Me.sep2.Name = "sep2"
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "E&xit"
		' 
		' mnuErrors
		' 
		Me.mnuErrors.Available = True
		Me.mnuErrors.Checked = False
		Me.mnuErrors.Enabled = True
		Me.mnuErrors.Name = "mnuErrors"
		Me.mnuErrors.Text = "&Errors"
		Me.mnuErrors.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuErrorsRetryImport, Me.mnuErrorsRetryExport})
		' 
		' mnuErrorsRetryImport
		' 
		Me.mnuErrorsRetryImport.Available = True
		Me.mnuErrorsRetryImport.Checked = False
		Me.mnuErrorsRetryImport.Enabled = True
		Me.mnuErrorsRetryImport.Name = "mnuErrorsRetryImport"
		Me.mnuErrorsRetryImport.Text = "Retry &Import"
		' 
		' mnuErrorsRetryExport
		' 
		Me.mnuErrorsRetryExport.Available = True
		Me.mnuErrorsRetryExport.Checked = False
		Me.mnuErrorsRetryExport.Enabled = True
		Me.mnuErrorsRetryExport.Name = "mnuErrorsRetryExport"
		Me.mnuErrorsRetryExport.Text = "Retry &Export"
		' 
		' mnuHelp
		' 
		Me.mnuHelp.Available = True
		Me.mnuHelp.Checked = False
		Me.mnuHelp.Enabled = True
		Me.mnuHelp.Name = "mnuHelp"
		Me.mnuHelp.Text = "&Help"
		Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuHelpAbout})
		' 
		' mnuHelpAbout
		' 
		Me.mnuHelpAbout.Available = True
		Me.mnuHelpAbout.Checked = False
		Me.mnuHelpAbout.Enabled = True
		Me.mnuHelpAbout.Name = "mnuHelpAbout"
		Me.mnuHelpAbout.Text = "&About..."
		' 
		' drvMDI
		' 
		Me.drvMDI.BackColor = System.Drawing.SystemColors.Window
		Me.drvMDI.CausesValidation = True
		Me.drvMDI.Cursor = System.Windows.Forms.Cursors.Default
		Me.drvMDI.Enabled = True
		Me.drvMDI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.drvMDI.ForeColor = System.Drawing.SystemColors.WindowText
		Me.drvMDI.Location = New System.Drawing.Point(164, 164)
		Me.drvMDI.Name = "drvMDI"
		Me.drvMDI.Size = New System.Drawing.Size(41, 21)
		Me.drvMDI.TabIndex = 10
		Me.drvMDI.TabStop = True
		Me.drvMDI.Visible = True
		' 
		' tmrDMSAPI
		' 
		Me.tmrDMSAPI.Enabled = False
		Me.tmrDMSAPI.Interval = 60000
		' 
		' pan3JournalPro
		' 
		Me.pan3JournalPro.BackColor = System.Drawing.SystemColors.Menu
		Me.pan3JournalPro.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3JournalPro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3JournalPro.Controls.Add(Me.cmdRunNow)
		Me.pan3JournalPro.Controls.Add(Me.cmdCancel)
		Me.pan3JournalPro.Controls.Add(Me.cmdOK)
		Me.pan3JournalPro.Controls.Add(Me.fra3JournalPro)
		Me.pan3JournalPro.Dock = System.Windows.Forms.DockStyle.Top
		Me.pan3JournalPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3JournalPro.Location = New System.Drawing.Point(0, 24)
		Me.pan3JournalPro.Name = "pan3JournalPro"
		Me.pan3JournalPro.Size = New System.Drawing.Size(232, 109)
		Me.pan3JournalPro.TabIndex = 5
		' 
		' cmdRunNow
		' 
		Me.cmdRunNow.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRunNow.CausesValidation = True
		Me.cmdRunNow.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRunNow.Enabled = True
		Me.cmdRunNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRunNow.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRunNow.Location = New System.Drawing.Point(151, 72)
		Me.cmdRunNow.Name = "cmdRunNow"
		Me.cmdRunNow.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRunNow.Size = New System.Drawing.Size(65, 24)
		Me.cmdRunNow.TabIndex = 9
		Me.cmdRunNow.TabStop = True
		Me.cmdRunNow.Text = "&Run Now"
		Me.cmdRunNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRunNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(151, 44)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 24)
		Me.cmdCancel.TabIndex = 2
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
		Me.cmdOK.Location = New System.Drawing.Point(151, 16)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 24)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fra3JournalPro
		' 
		Me.fra3JournalPro.Controls.Add(Me.pan3TimerInterval)
		Me.fra3JournalPro.Controls.Add(Me.lblMinutes)
		Me.fra3JournalPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fra3JournalPro.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0)
		Me.fra3JournalPro.Location = New System.Drawing.Point(16, 8)
		Me.fra3JournalPro.Name = "fra3JournalPro"
		Me.fra3JournalPro.Size = New System.Drawing.Size(121, 89)
		Me.fra3JournalPro.TabIndex = 6
		Me.fra3JournalPro.Text = "Process Interval"
		' 
		' pan3TimerInterval
		' 
		Me.pan3TimerInterval.BackColor = System.Drawing.SystemColors.Control
		Me.pan3TimerInterval.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3TimerInterval.Controls.Add(Me.txtTimerInterval)
		Me.pan3TimerInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3TimerInterval.Location = New System.Drawing.Point(16, 36)
		Me.pan3TimerInterval.Name = "pan3TimerInterval"
		Me.pan3TimerInterval.Size = New System.Drawing.Size(33, 25)
		Me.pan3TimerInterval.TabIndex = 7
		' 
		' txtTimerInterval
		' 
		Me.txtTimerInterval.AcceptsReturn = True
		Me.txtTimerInterval.AutoSize = False
		Me.txtTimerInterval.BackColor = System.Drawing.SystemColors.Window
		Me.txtTimerInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtTimerInterval.CausesValidation = True
		Me.txtTimerInterval.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTimerInterval.Enabled = True
		Me.txtTimerInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTimerInterval.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTimerInterval.HideSelection = True
		Me.txtTimerInterval.Location = New System.Drawing.Point(3, 3)
		Me.txtTimerInterval.MaxLength = 2
		Me.txtTimerInterval.Multiline = False
		Me.txtTimerInterval.Name = "txtTimerInterval"
		Me.txtTimerInterval.ReadOnly = False
		Me.txtTimerInterval.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTimerInterval.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTimerInterval.Size = New System.Drawing.Size(27, 19)
		Me.txtTimerInterval.TabIndex = 0
		Me.txtTimerInterval.TabStop = True
		Me.txtTimerInterval.Text = "15"
		Me.txtTimerInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTimerInterval.Visible = True
		' 
		' lblMinutes
		' 
		Me.lblMinutes.AutoSize = False
		Me.lblMinutes.BackColor = System.Drawing.Color.Transparent
		Me.lblMinutes.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMinutes.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMinutes.Enabled = True
		Me.lblMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMinutes.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lblMinutes.Location = New System.Drawing.Point(56, 40)
		Me.lblMinutes.Name = "lblMinutes"
		Me.lblMinutes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMinutes.Size = New System.Drawing.Size(49, 17)
		Me.lblMinutes.TabIndex = 8
		Me.lblMinutes.Text = "Minutes"
		Me.lblMinutes.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMinutes.UseMnemonic = True
		Me.lblMinutes.Visible = True
		' 
		' pan3PasswordStatus
		' 
		Me.pan3PasswordStatus.BackColor = System.Drawing.SystemColors.Control
		Me.pan3PasswordStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3PasswordStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3PasswordStatus.Location = New System.Drawing.Point(24, 24)
		Me.pan3PasswordStatus.Name = "pan3PasswordStatus"
		Me.pan3PasswordStatus.Size = New System.Drawing.Size(41, 17)
		Me.pan3PasswordStatus.TabIndex = 4
		Me.pan3PasswordStatus.Visible = False
		' 
		' pan3StatusBar
		' 
		Me.pan3StatusBar.BackColor = System.Drawing.SystemColors.Control
		Me.pan3StatusBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3StatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3StatusBar.Location = New System.Drawing.Point(64, 24)
		Me.pan3StatusBar.Name = "pan3StatusBar"
		Me.pan3StatusBar.Size = New System.Drawing.Size(41, 17)
		Me.pan3StatusBar.TabIndex = 3
		Me.pan3StatusBar.Visible = False
		' 
		' imgMedia
		' 
		Me.imgMedia.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgMedia.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgMedia.Enabled = True
		Me.imgMedia.Location = New System.Drawing.Point(44, 155)
		Me.imgMedia.Name = "imgMedia"
		Me.imgMedia.Size = New System.Drawing.Size(32, 32)
		Me.imgMedia.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgMedia.Visible = True
		' 
		' imgProcess
		' 
		Me.imgProcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgProcess.Enabled = True
		Me.imgProcess.Location = New System.Drawing.Point(8, 155)
		Me.imgProcess.Name = "imgProcess"
		Me.imgProcess.Size = New System.Drawing.Size(32, 32)
		Me.imgProcess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgProcess.Visible = True
		' 
		' imgProcess2
		' 
		Me.imgProcess2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgProcess2.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgProcess2.Enabled = True
		Me.imgProcess2.Location = New System.Drawing.Point(8, 192)
		Me.imgProcess2.Name = "imgProcess2"
		Me.imgProcess2.Size = New System.Drawing.Size(32, 32)
		Me.imgProcess2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgProcess2.Visible = True
		' 
		' imgProcess1
		' 
		Me.imgProcess1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgProcess1.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgProcess1.Enabled = True
		Me.imgProcess1.Location = New System.Drawing.Point(44, 192)
		Me.imgProcess1.Name = "imgProcess1"
		Me.imgProcess1.Size = New System.Drawing.Size(32, 32)
		Me.imgProcess1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgProcess1.Visible = True
		' 
		' frmDMSMain
		' 
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.ClientSize = New System.Drawing.Size(232, 134)
		Me.ControlBox = True
		Me.Controls.Add(Me.drvMDI)
		Me.Controls.Add(Me.pan3JournalPro)
		Me.Controls.Add(Me.pan3PasswordStatus)
		Me.Controls.Add(Me.pan3StatusBar)
		Me.Controls.Add(Me.imgMedia)
		Me.Controls.Add(Me.imgProcess)
		Me.Controls.Add(Me.imgProcess2)
		Me.Controls.Add(Me.imgProcess1)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmDMSMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(140, 229)
		Me.MaximizeBox = False
		Me.MinimizeBox = True
		Me.Name = "frmDMSMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DocuMaster API Daemon"
		Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
		Me.pan3JournalPro.ResumeLayout(False)
		Me.fra3JournalPro.ResumeLayout(False)
		Me.pan3TimerInterval.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class