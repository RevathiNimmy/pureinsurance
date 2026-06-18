<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessage
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
	Public WithEvents cmdNo As System.Windows.Forms.Button
	Public WithEvents cmdYes As System.Windows.Forms.Button
	Public WithEvents rtbReadme As System.Windows.Forms.RichTextBox
	Public WithEvents lblMessage As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessage))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdNo = New System.Windows.Forms.Button
		Me.cmdYes = New System.Windows.Forms.Button
		Me.rtbReadme = New System.Windows.Forms.RichTextBox
		Me.lblMessage = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdNo
		' 
		Me.cmdNo.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNo.CausesValidation = True
		Me.cmdNo.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNo.Enabled = True
		Me.cmdNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNo.Location = New System.Drawing.Point(232, 304)
		Me.cmdNo.Name = "cmdNo"
		Me.cmdNo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNo.Size = New System.Drawing.Size(73, 22)
		Me.cmdNo.TabIndex = 3
		Me.cmdNo.TabStop = True
		Me.cmdNo.Text = "&No"
		Me.cmdNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdYes
		' 
		Me.cmdYes.BackColor = System.Drawing.SystemColors.Control
		Me.cmdYes.CausesValidation = True
		Me.cmdYes.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdYes.Enabled = True
		Me.cmdYes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdYes.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdYes.Location = New System.Drawing.Point(152, 304)
		Me.cmdYes.Name = "cmdYes"
		Me.cmdYes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdYes.Size = New System.Drawing.Size(73, 22)
		Me.cmdYes.TabIndex = 2
		Me.cmdYes.TabStop = True
		Me.cmdYes.Text = "&Yes"
		Me.cmdYes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdYes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' rtbReadme
		' 
		Me.rtbReadme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.rtbReadme.Enabled = True
		Me.rtbReadme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.rtbReadme.Location = New System.Drawing.Point(8, 136)
		Me.rtbReadme.Name = "rtbReadme"
		Me.rtbReadme.ReadOnly = -1
		Me.rtbReadme.RTF = resources.GetString("rtbReadme.TextRTF")
		Me.rtbReadme.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both
		Me.rtbReadme.Size = New System.Drawing.Size(297, 160)
		Me.rtbReadme.TabIndex = 1
		' 
		' lblMessage
		' 
		Me.lblMessage.AutoSize = False
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.Enabled = True
		Me.lblMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Location = New System.Drawing.Point(16, 16)
		Me.lblMessage.Name = "lblMessage"
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.Size = New System.Drawing.Size(281, 105)
		Me.lblMessage.TabIndex = 0
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		' 
		' frmMessage
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(315, 334)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNo)
		Me.Controls.Add(Me.cmdYes)
		Me.Controls.Add(Me.rtbReadme)
		Me.Controls.Add(Me.lblMessage)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmMessage"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class