<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogDrop
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
	Public WithEvents mnuFileOpen As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sep1 As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public dlgOpenOpen As System.Windows.Forms.OpenFileDialog
	Public dlgOpenSave As System.Windows.Forms.SaveFileDialog
	Public dlgOpenFont As System.Windows.Forms.FontDialog
	Public dlgOpenColor As System.Windows.Forms.ColorDialog
	Public dlgOpenPrint As System.Windows.Forms.PrintDialog
	Public WithEvents imgLogo As System.Windows.Forms.PictureBox
	Public WithEvents lblLabel As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogDrop))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileOpen = New System.Windows.Forms.ToolStripMenuItem
		Me.sep1 = New System.Windows.Forms.ToolStripSeparator
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.dlgOpenOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpenSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgOpenFont = New System.Windows.Forms.FontDialog
		Me.dlgOpenColor = New System.Windows.Forms.ColorDialog
		Me.dlgOpenPrint = New System.Windows.Forms.PrintDialog
		Me.imgLogo = New System.Windows.Forms.PictureBox
		Me.lblLabel = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileOpen, Me.sep1, Me.mnuFileExit})
		' 
		' mnuFileOpen
		' 
		Me.mnuFileOpen.Available = True
		Me.mnuFileOpen.Checked = False
		Me.mnuFileOpen.Enabled = True
		Me.mnuFileOpen.Name = "mnuFileOpen"
		Me.mnuFileOpen.Text = "&Open"
		' 
		' sep1
		' 
		Me.sep1.Available = True
		Me.sep1.Enabled = True
		Me.sep1.Name = "sep1"
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "E&xit"
		' 
		' imgLogo
		' 
		Me.imgLogo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgLogo.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgLogo.Enabled = True
		Me.imgLogo.Image = CType(resources.GetObject("imgLogo.Image"), System.Drawing.Image)
		Me.imgLogo.Location = New System.Drawing.Point(0, 24)
		Me.imgLogo.Name = "imgLogo"
		Me.imgLogo.Size = New System.Drawing.Size(477, 338)
		Me.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.imgLogo.Visible = True
		' 
		' lblLabel
		' 
		Me.lblLabel.AutoSize = True
		Me.lblLabel.BackColor = System.Drawing.Color.Transparent
		Me.lblLabel.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLabel.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLabel.Enabled = True
		Me.lblLabel.Font = New System.Drawing.Font("Tahoma", 12!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLabel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLabel.Location = New System.Drawing.Point(165, 368)
		Me.lblLabel.Name = "lblLabel"
		Me.lblLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLabel.Size = New System.Drawing.Size(177, 19)
		Me.lblLabel.TabIndex = 0
		Me.lblLabel.Text = "Drop .zip file on here."
		Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
		Me.lblLabel.UseMnemonic = True
		Me.lblLabel.Visible = True
		' 
		' frmLogDrop
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
		Me.BackColor = System.Drawing.Color.White
		Me.ControlBox = True
		Me.Controls.Add(Me.imgLogo)
		Me.Controls.Add(Me.lblLabel)
		Me.ClientSize = New System.Drawing.Size(482, 396)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.dlgOpenOpen.Filter = "Zip files (*.zip)|*.zip"
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmLogDrop.Icon"), System.Drawing.Icon)
		Me.dlgOpenOpen.InitialDirectory = "C:\"
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 41)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmLogDrop"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Sirius Log Viewer"
		Me.dlgOpenOpen.Title = "Open .ZIP file"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class