<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportFolder
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
	Public commDlgOpen As System.Windows.Forms.OpenFileDialog
	Public commDlgSave As System.Windows.Forms.SaveFileDialog
	Public commDlgFont As System.Windows.Forms.FontDialog
	Public commDlgColor As System.Windows.Forms.ColorDialog
	Public commDlgPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdBrowse As System.Windows.Forms.Button
	Public WithEvents txtFolderName As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblFolderName As System.Windows.Forms.Label
	Public WithEvents lblTitle As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExportFolder))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.commDlgOpen = New System.Windows.Forms.OpenFileDialog
		Me.commDlgSave = New System.Windows.Forms.SaveFileDialog
		Me.commDlgFont = New System.Windows.Forms.FontDialog
		Me.commDlgColor = New System.Windows.Forms.ColorDialog
		Me.commDlgPrint = New System.Windows.Forms.PrintDialog
		Me.cmdBrowse = New System.Windows.Forms.Button
		Me.txtFolderName = New System.Windows.Forms.TextBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lblFolderName = New System.Windows.Forms.Label
		Me.lblTitle = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdBrowse
		' 
		Me.cmdBrowse.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBrowse.CausesValidation = True
		Me.cmdBrowse.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBrowse.Enabled = True
		Me.cmdBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdBrowse.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBrowse.Location = New System.Drawing.Point(416, 44)
		Me.cmdBrowse.Name = "cmdBrowse"
		Me.cmdBrowse.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdBrowse.Size = New System.Drawing.Size(81, 25)
		Me.cmdBrowse.TabIndex = 1
		Me.cmdBrowse.TabStop = True
		Me.cmdBrowse.Text = "&Browse..."
		Me.cmdBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtFolderName
		' 
		Me.txtFolderName.AcceptsReturn = True
		Me.txtFolderName.AutoSize = False
		Me.txtFolderName.BackColor = System.Drawing.SystemColors.Window
		Me.txtFolderName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFolderName.CausesValidation = True
		Me.txtFolderName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFolderName.Enabled = True
		Me.txtFolderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFolderName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFolderName.HideSelection = True
		Me.txtFolderName.Location = New System.Drawing.Point(112, 47)
		Me.txtFolderName.MaxLength = 0
		Me.txtFolderName.Multiline = False
		Me.txtFolderName.Name = "txtFolderName"
		Me.txtFolderName.ReadOnly = False
		Me.txtFolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFolderName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFolderName.Size = New System.Drawing.Size(297, 19)
		Me.txtFolderName.TabIndex = 0
		Me.txtFolderName.TabStop = True
		Me.txtFolderName.Text = " "
		Me.txtFolderName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFolderName.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(272, 88)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
		Me.cmdCancel.TabIndex = 3
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
		Me.cmdOK.Location = New System.Drawing.Point(176, 88)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblFolderName
		' 
		Me.lblFolderName.AutoSize = False
		Me.lblFolderName.BackColor = System.Drawing.SystemColors.Control
		Me.lblFolderName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFolderName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFolderName.Enabled = True
		Me.lblFolderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFolderName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFolderName.Location = New System.Drawing.Point(16, 48)
		Me.lblFolderName.Name = "lblFolderName"
		Me.lblFolderName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFolderName.Size = New System.Drawing.Size(89, 17)
		Me.lblFolderName.TabIndex = 5
		Me.lblFolderName.Text = "Folder Name:"
		Me.lblFolderName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFolderName.UseMnemonic = True
		Me.lblFolderName.Visible = True
		' 
		' lblTitle
		' 
		Me.lblTitle.AutoSize = False
		Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
		Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTitle.Enabled = True
		Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTitle.Location = New System.Drawing.Point(16, 16)
		Me.lblTitle.Name = "lblTitle"
		Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTitle.Size = New System.Drawing.Size(185, 17)
		Me.lblTitle.TabIndex = 4
		Me.lblTitle.Text = "Specify the destination folder name"
		Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTitle.UseMnemonic = True
		Me.lblTitle.Visible = True
		' 
		' frmExportFolder
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(511, 120)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdBrowse)
		Me.Controls.Add(Me.txtFolderName)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lblFolderName)
		Me.Controls.Add(Me.lblTitle)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmExportFolder.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmExportFolder"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Export"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class