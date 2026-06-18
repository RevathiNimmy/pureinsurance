<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBadScan
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
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents cmdScanAgain As System.Windows.Forms.Button
	Public WithEvents lblWarning As System.Windows.Forms.Label
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents lblWarningTitle As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBadScan))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Command1 = New System.Windows.Forms.Button
		Me.cmdScanAgain = New System.Windows.Forms.Button
		Me.lblWarning = New System.Windows.Forms.Label
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.lblWarningTitle = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' Command1
		' 
		Me.Command1.BackColor = System.Drawing.SystemColors.Control
		Me.Command1.CausesValidation = True
		Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Command1.Enabled = True
		Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Command1.Location = New System.Drawing.Point(56, 96)
		Me.Command1.Name = "Command1"
		Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Command1.Size = New System.Drawing.Size(73, 22)
		Me.Command1.TabIndex = 1
		Me.Command1.TabStop = True
		Me.Command1.Text = "&Contiune"
		Me.Command1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdScanAgain
		' 
		Me.cmdScanAgain.BackColor = System.Drawing.SystemColors.Control
		Me.cmdScanAgain.CausesValidation = True
		Me.cmdScanAgain.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdScanAgain.Enabled = True
		Me.cmdScanAgain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdScanAgain.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdScanAgain.Location = New System.Drawing.Point(216, 96)
		Me.cmdScanAgain.Name = "cmdScanAgain"
		Me.cmdScanAgain.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdScanAgain.Size = New System.Drawing.Size(73, 22)
		Me.cmdScanAgain.TabIndex = 0
		Me.cmdScanAgain.TabStop = True
		Me.cmdScanAgain.Text = "&Re-Scan"
		Me.cmdScanAgain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdScanAgain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblWarning
		' 
		Me.lblWarning.AutoSize = False
		Me.lblWarning.BackColor = System.Drawing.SystemColors.Control
		Me.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWarning.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWarning.Enabled = True
		Me.lblWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWarning.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWarning.Location = New System.Drawing.Point(64, 40)
		Me.lblWarning.Name = "lblWarning"
		Me.lblWarning.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWarning.Size = New System.Drawing.Size(225, 41)
		Me.lblWarning.TabIndex = 3
		Me.lblWarning.Text = "The size of the image that has just been scanned is less than normally expected. Do you wish to :"
		Me.lblWarning.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWarning.UseMnemonic = True
		Me.lblWarning.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(16, 16)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' lblWarningTitle
		' 
		Me.lblWarningTitle.AutoSize = True
		Me.lblWarningTitle.BackColor = System.Drawing.SystemColors.Control
		Me.lblWarningTitle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWarningTitle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWarningTitle.Enabled = True
		Me.lblWarningTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWarningTitle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWarningTitle.Location = New System.Drawing.Point(64, 16)
		Me.lblWarningTitle.Name = "lblWarningTitle"
		Me.lblWarningTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWarningTitle.Size = New System.Drawing.Size(62, 16)
		Me.lblWarningTitle.TabIndex = 2
		Me.lblWarningTitle.Text = "Warning!"
		Me.lblWarningTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWarningTitle.UseMnemonic = True
		Me.lblWarningTitle.Visible = True
		' 
		' frmBadScan
		' 
		Me.AcceptButton = Me.cmdScanAgain
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(309, 130)
		Me.ControlBox = True
		Me.Controls.Add(Me.Command1)
		Me.Controls.Add(Me.cmdScanAgain)
		Me.Controls.Add(Me.lblWarning)
		Me.Controls.Add(Me.Image1)
		Me.Controls.Add(Me.lblWarningTitle)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmBadScan.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmBadScan"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "ScanStation"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class