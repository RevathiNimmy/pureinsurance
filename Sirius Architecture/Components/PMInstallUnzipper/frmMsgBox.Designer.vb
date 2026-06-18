<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMsgBox
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents tmrCountdown As System.Windows.Forms.Timer
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents lblMessage As System.Windows.Forms.Label
	Public WithEvents imgInformation As System.Windows.Forms.PictureBox
	Public WithEvents imgExclamation As System.Windows.Forms.PictureBox
	Public WithEvents imgQuestion As System.Windows.Forms.PictureBox
	Public WithEvents imgCritical As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMsgBox))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tmrCountdown = New System.Windows.Forms.Timer(components)
		Me.cmdOk = New System.Windows.Forms.Button
		Me.lblMessage = New System.Windows.Forms.Label
		Me.imgInformation = New System.Windows.Forms.PictureBox
		Me.imgExclamation = New System.Windows.Forms.PictureBox
		Me.imgQuestion = New System.Windows.Forms.PictureBox
		Me.imgCritical = New System.Windows.Forms.PictureBox
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(136, 48)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tmrCountdown
		' 
		Me.tmrCountdown.Enabled = False
		Me.tmrCountdown.Interval = 990
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = True
		Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(64, 48)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(65, 25)
		Me.cmdOk.TabIndex = 1
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&Ok ... 99"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblMessage
		' 
		Me.lblMessage.AutoSize = True
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Enabled = True
		Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(64, 12)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(32, 13)
		Me.lblMessage.TabIndex = 0
		Me.lblMessage.Text = "Label1"
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		' 
		' imgInformation
		' 
		Me.imgInformation.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgInformation.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgInformation.Enabled = True
		Me.imgInformation.Image = CType(resources.GetObject("imgInformation.Image"), System.Drawing.Image)
		Me.imgInformation.Location = New System.Drawing.Point(200, 116)
		Me.imgInformation.Name = "imgInformation"
		Me.imgInformation.Size = New System.Drawing.Size(32, 32)
		Me.imgInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgInformation.Visible = False
		' 
		' imgExclamation
		' 
		Me.imgExclamation.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgExclamation.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgExclamation.Enabled = True
		Me.imgExclamation.Image = CType(resources.GetObject("imgExclamation.Image"), System.Drawing.Image)
		Me.imgExclamation.Location = New System.Drawing.Point(92, 116)
		Me.imgExclamation.Name = "imgExclamation"
		Me.imgExclamation.Size = New System.Drawing.Size(32, 32)
		Me.imgExclamation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgExclamation.Visible = False
		' 
		' imgQuestion
		' 
		Me.imgQuestion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgQuestion.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgQuestion.Enabled = True
		Me.imgQuestion.Image = CType(resources.GetObject("imgQuestion.Image"), System.Drawing.Image)
		Me.imgQuestion.Location = New System.Drawing.Point(128, 116)
		Me.imgQuestion.Name = "imgQuestion"
		Me.imgQuestion.Size = New System.Drawing.Size(32, 32)
		Me.imgQuestion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgQuestion.Visible = False
		' 
		' imgCritical
		' 
		Me.imgCritical.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgCritical.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgCritical.Enabled = True
		Me.imgCritical.Image = CType(resources.GetObject("imgCritical.Image"), System.Drawing.Image)
		Me.imgCritical.Location = New System.Drawing.Point(164, 116)
		Me.imgCritical.Name = "imgCritical"
		Me.imgCritical.Size = New System.Drawing.Size(32, 32)
		Me.imgCritical.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgCritical.Visible = False
		' 
		' frmMsgBox
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(312, 179)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.lblMessage)
		Me.Controls.Add(Me.imgInformation)
		Me.Controls.Add(Me.imgExclamation)
		Me.Controls.Add(Me.imgQuestion)
		Me.Controls.Add(Me.imgCritical)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMsgBox"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class