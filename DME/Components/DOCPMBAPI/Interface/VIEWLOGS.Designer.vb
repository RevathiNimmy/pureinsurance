<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewLogFiles
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
	Public WithEvents vsbView As System.Windows.Forms.VScrollBar
	Public WithEvents lstLogFilesSorted As System.Windows.Forms.ListBox
	Public WithEvents txtLogFile As System.Windows.Forms.TextBox
	Public WithEvents pan3LogFile As System.Windows.Forms.Panel
	Public WithEvents cmbLogFiles As System.Windows.Forms.ComboBox
	Public WithEvents pan3LogFiles As System.Windows.Forms.Panel
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents pan3ViewLogFiles As System.Windows.Forms.Panel
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmViewLogFiles))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pan3ViewLogFiles = New System.Windows.Forms.Panel
		Me.vsbView = New System.Windows.Forms.VScrollBar
		Me.lstLogFilesSorted = New System.Windows.Forms.ListBox
		Me.pan3LogFile = New System.Windows.Forms.Panel
		Me.txtLogFile = New System.Windows.Forms.TextBox
		Me.pan3LogFiles = New System.Windows.Forms.Panel
		Me.cmbLogFiles = New System.Windows.Forms.ComboBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.pan3ViewLogFiles.SuspendLayout()
		Me.pan3LogFile.SuspendLayout()
		Me.pan3LogFiles.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' pan3ViewLogFiles
		' 
		Me.pan3ViewLogFiles.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.pan3ViewLogFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3ViewLogFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3ViewLogFiles.Controls.Add(Me.vsbView)
		Me.pan3ViewLogFiles.Controls.Add(Me.lstLogFilesSorted)
		Me.pan3ViewLogFiles.Controls.Add(Me.pan3LogFile)
		Me.pan3ViewLogFiles.Controls.Add(Me.pan3LogFiles)
		Me.pan3ViewLogFiles.Controls.Add(Me.cmdOK)
		Me.pan3ViewLogFiles.Controls.Add(Me.Label1)
		Me.pan3ViewLogFiles.Dock = System.Windows.Forms.DockStyle.Top
		Me.pan3ViewLogFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3ViewLogFiles.Location = New System.Drawing.Point(0, 0)
		Me.pan3ViewLogFiles.Name = "pan3ViewLogFiles"
		Me.pan3ViewLogFiles.Size = New System.Drawing.Size(666, 349)
		Me.pan3ViewLogFiles.TabIndex = 2
		' 
		' vsbView
		' 
		Me.vsbView.CausesValidation = True
		Me.vsbView.Cursor = System.Windows.Forms.Cursors.Default
		Me.vsbView.Enabled = True
		Me.vsbView.LargeChange = 1
		Me.vsbView.Location = New System.Drawing.Point(620, 56)
		Me.vsbView.Maximum = 32767
		Me.vsbView.Minimum = -32000
		Me.vsbView.Name = "vsbView"
		Me.vsbView.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.vsbView.Size = New System.Drawing.Size(17, 241)
		Me.vsbView.SmallChange = 1
		Me.vsbView.TabIndex = 8
		Me.vsbView.TabStop = True
		Me.vsbView.Value = -32000
		Me.vsbView.Visible = True
		' 
		' lstLogFilesSorted
		' 
		Me.lstLogFilesSorted.BackColor = System.Drawing.SystemColors.Window
		Me.lstLogFilesSorted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lstLogFilesSorted.CausesValidation = True
		Me.lstLogFilesSorted.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstLogFilesSorted.Enabled = True
		Me.lstLogFilesSorted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstLogFilesSorted.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstLogFilesSorted.IntegralHeight = True
		Me.lstLogFilesSorted.Location = New System.Drawing.Point(208, 20)
		Me.lstLogFilesSorted.MultiColumn = False
		Me.lstLogFilesSorted.Name = "lstLogFilesSorted"
		Me.lstLogFilesSorted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstLogFilesSorted.Size = New System.Drawing.Size(97, 18)
		Me.lstLogFilesSorted.Sorted = True
		Me.lstLogFilesSorted.TabIndex = 7
		Me.lstLogFilesSorted.TabStop = True
		Me.lstLogFilesSorted.Visible = False
		' 
		' pan3LogFile
		' 
		Me.pan3LogFile.BackColor = System.Drawing.SystemColors.Control
		Me.pan3LogFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3LogFile.Controls.Add(Me.txtLogFile)
		Me.pan3LogFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3LogFile.Location = New System.Drawing.Point(16, 52)
		Me.pan3LogFile.Name = "pan3LogFile"
		Me.pan3LogFile.Size = New System.Drawing.Size(633, 253)
		Me.pan3LogFile.TabIndex = 5
		' 
		' txtLogFile
		' 
		Me.txtLogFile.AcceptsReturn = True
		Me.txtLogFile.AutoSize = False
		Me.txtLogFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtLogFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.txtLogFile.CausesValidation = True
		Me.txtLogFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLogFile.Enabled = True
		Me.txtLogFile.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLogFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLogFile.HideSelection = True
		Me.txtLogFile.Location = New System.Drawing.Point(3, 3)
		Me.txtLogFile.MaxLength = 0
		Me.txtLogFile.Multiline = True
		Me.txtLogFile.Name = "txtLogFile"
		Me.txtLogFile.ReadOnly = False
		Me.txtLogFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLogFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLogFile.Size = New System.Drawing.Size(627, 247)
		Me.txtLogFile.TabIndex = 6
		Me.txtLogFile.TabStop = True
		Me.txtLogFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLogFile.Visible = True
		' 
		' pan3LogFiles
		' 
		Me.pan3LogFiles.BackColor = System.Drawing.SystemColors.Control
		Me.pan3LogFiles.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3LogFiles.Controls.Add(Me.cmbLogFiles)
		Me.pan3LogFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3LogFiles.Location = New System.Drawing.Point(72, 16)
		Me.pan3LogFiles.Name = "pan3LogFiles"
		Me.pan3LogFiles.Size = New System.Drawing.Size(129, 26)
		Me.pan3LogFiles.TabIndex = 3
		' 
		' cmbLogFiles
		' 
		Me.cmbLogFiles.BackColor = System.Drawing.SystemColors.Window
		Me.cmbLogFiles.CausesValidation = True
		Me.cmbLogFiles.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbLogFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbLogFiles.Enabled = True
		Me.cmbLogFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbLogFiles.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbLogFiles.IntegralHeight = True
		Me.cmbLogFiles.Location = New System.Drawing.Point(3, 3)
		Me.cmbLogFiles.Name = "cmbLogFiles"
		Me.cmbLogFiles.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbLogFiles.Size = New System.Drawing.Size(123, 20)
		Me.cmbLogFiles.Sorted = False
		Me.cmbLogFiles.TabIndex = 0
		Me.cmbLogFiles.TabStop = True
		Me.cmbLogFiles.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(584, 312)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 25)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.Color.Transparent
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Label1.Location = New System.Drawing.Point(16, 20)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(57, 17)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Log File"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmViewLogFiles
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(666, 349)
		Me.ControlBox = False
		Me.Controls.Add(Me.pan3ViewLogFiles)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(51, 133)
		Me.MaximizeBox = True
		Me.MinimizeBox = False
		Me.Name = "frmViewLogFiles"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "View Log Files"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstLogFilesSorted, System.Windows.Forms.SelectionMode.One)
		Me.pan3ViewLogFiles.ResumeLayout(False)
		Me.pan3LogFile.ResumeLayout(False)
		Me.pan3LogFiles.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class