<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddEditComments
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
	Public WithEvents txtComment As System.Windows.Forms.TextBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblComment As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddEditComments))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtComment = New System.Windows.Forms.TextBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lblComment = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtComment
		' 
		Me.txtComment.AcceptsReturn = True
		Me.txtComment.AutoSize = False
		Me.txtComment.BackColor = System.Drawing.SystemColors.Window
		Me.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtComment.CausesValidation = True
		Me.txtComment.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtComment.Enabled = True
		Me.txtComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtComment.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtComment.HideSelection = True
		Me.txtComment.Location = New System.Drawing.Point(96, 14)
		Me.txtComment.MaxLength = 500
		Me.txtComment.Multiline = True
		Me.txtComment.Name = "txtComment"
		Me.txtComment.ReadOnly = False
		Me.txtComment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtComment.Size = New System.Drawing.Size(433, 99)
		Me.txtComment.TabIndex = 3
		Me.txtComment.TabStop = True
		Me.txtComment.Tag = "F;"
		Me.txtComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtComment.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(376, 120)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 23)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = False
		Me.cmdOK.Tag = "CAP;200"
		Me.cmdOK.Text = "{&OK}"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(456, 120)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = False
		Me.cmdCancel.Tag = "CAP;201"
		Me.cmdCancel.Text = "{&Cancel}"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblComment
		' 
		Me.lblComment.AutoSize = False
		Me.lblComment.BackColor = System.Drawing.SystemColors.Control
		Me.lblComment.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblComment.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblComment.Enabled = True
		Me.lblComment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblComment.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblComment.Location = New System.Drawing.Point(16, 16)
		Me.lblComment.Name = "lblComment"
		Me.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblComment.Size = New System.Drawing.Size(80, 19)
		Me.lblComment.TabIndex = 2
		Me.lblComment.Tag = "CAP;809"
		Me.lblComment.Text = "{Comment:}"
		Me.lblComment.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblComment.UseMnemonic = True
		Me.lblComment.Visible = True
		' 
		' frmAddEditComments
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(542, 154)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtComment)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lblComment)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmAddEditComments"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Tag = "CAP;810"
		Me.Text = "{Add/Edit Comment}"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class