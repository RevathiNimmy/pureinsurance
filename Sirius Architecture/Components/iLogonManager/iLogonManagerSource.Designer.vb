<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSource
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
	Public WithEvents lstSource As System.Windows.Forms.ListBox
	Public WithEvents lblUserName As System.Windows.Forms.Label
	Public WithEvents lblLogonDesc As System.Windows.Forms.Label
	Public WithEvents imgLogonDetails As System.Windows.Forms.PictureBox
	Public WithEvents fraLogonDetails As System.Windows.Forms.GroupBox
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSource))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.fraLogonDetails = New System.Windows.Forms.GroupBox
		Me.lstSource = New System.Windows.Forms.ListBox
		Me.lblUserName = New System.Windows.Forms.Label
		Me.lblLogonDesc = New System.Windows.Forms.Label
		Me.imgLogonDetails = New System.Windows.Forms.PictureBox
		Me.fraLogonDetails.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(234, 196)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 4
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraLogonDetails
		' 
		Me.fraLogonDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraLogonDetails.Controls.Add(Me.lstSource)
		Me.fraLogonDetails.Controls.Add(Me.lblUserName)
		Me.fraLogonDetails.Controls.Add(Me.lblLogonDesc)
		Me.fraLogonDetails.Controls.Add(Me.imgLogonDetails)
		Me.fraLogonDetails.Enabled = True
		Me.fraLogonDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraLogonDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraLogonDetails.Location = New System.Drawing.Point(8, 4)
		Me.fraLogonDetails.Name = "fraLogonDetails"
		Me.fraLogonDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraLogonDetails.Size = New System.Drawing.Size(299, 185)
		Me.fraLogonDetails.TabIndex = 0
		Me.fraLogonDetails.Text = "Branch"
		Me.fraLogonDetails.Visible = True
		' 
		' lstSource
		' 
		Me.lstSource.BackColor = System.Drawing.SystemColors.Window
		Me.lstSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstSource.CausesValidation = True
		Me.lstSource.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstSource.Enabled = True
		Me.lstSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstSource.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstSource.IntegralHeight = True
		Me.lstSource.Location = New System.Drawing.Point(72, 68)
		Me.lstSource.MultiColumn = False
		Me.lstSource.Name = "lstSource"
		Me.lstSource.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstSource.Size = New System.Drawing.Size(197, 85)
		Me.lstSource.Sorted = False
		Me.lstSource.TabIndex = 3
		Me.lstSource.TabStop = True
		Me.lstSource.Visible = True
		' 
		' lblUserName
		' 
		Me.lblUserName.AutoSize = False
		Me.lblUserName.BackColor = System.Drawing.SystemColors.Control
		Me.lblUserName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUserName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUserName.Enabled = True
		Me.lblUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUserName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblUserName.Location = New System.Drawing.Point(16, 66)
		Me.lblUserName.Name = "lblUserName"
		Me.lblUserName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUserName.Size = New System.Drawing.Size(57, 17)
		Me.lblUserName.TabIndex = 2
		Me.lblUserName.Text = "Branch:"
		Me.lblUserName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUserName.UseMnemonic = True
		Me.lblUserName.Visible = True
		' 
		' lblLogonDesc
		' 
		Me.lblLogonDesc.AutoSize = False
		Me.lblLogonDesc.BackColor = System.Drawing.SystemColors.Control
		Me.lblLogonDesc.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLogonDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLogonDesc.Enabled = True
		Me.lblLogonDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLogonDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLogonDesc.Location = New System.Drawing.Point(56, 24)
		Me.lblLogonDesc.Name = "lblLogonDesc"
		Me.lblLogonDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLogonDesc.Size = New System.Drawing.Size(217, 33)
		Me.lblLogonDesc.TabIndex = 1
		Me.lblLogonDesc.Text = "Please select your branch"
		Me.lblLogonDesc.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLogonDesc.UseMnemonic = True
		Me.lblLogonDesc.Visible = True
		' 
		' imgLogonDetails
		' 
		Me.imgLogonDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgLogonDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgLogonDetails.Enabled = True
		Me.imgLogonDetails.Image = CType(resources.GetObject("imgLogonDetails.Image"), System.Drawing.Image)
		Me.imgLogonDetails.Location = New System.Drawing.Point(8, 24)
		Me.imgLogonDetails.Name = "imgLogonDetails"
		Me.imgLogonDetails.Size = New System.Drawing.Size(32, 32)
		Me.imgLogonDetails.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgLogonDetails.Visible = True
		' 
		' frmSource
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(316, 225)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.fraLogonDetails)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmSource.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSource"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "User logon - branch selection"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstSource, System.Windows.Forms.SelectionMode.One)
		Me.fraLogonDetails.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class