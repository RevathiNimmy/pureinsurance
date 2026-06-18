<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInputBox
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
	Public WithEvents txtValue As System.Windows.Forms.TextBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblPrompt As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInputBox))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtValue = New System.Windows.Forms.TextBox
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lblPrompt = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtValue
		' 
		Me.txtValue.AcceptsReturn = True
		Me.txtValue.AutoSize = False
		Me.txtValue.BackColor = System.Drawing.SystemColors.Window
		Me.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtValue.CausesValidation = True
		Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtValue.Enabled = True
		Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtValue.HideSelection = True
		Me.txtValue.Location = New System.Drawing.Point(7, 63)
		Me.txtValue.MaxLength = 255
		Me.txtValue.Multiline = True
		Me.txtValue.Name = "txtValue"
		Me.txtValue.ReadOnly = False
		Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtValue.Size = New System.Drawing.Size(295, 113)
		Me.txtValue.TabIndex = 3
		Me.txtValue.TabStop = True
		Me.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtValue.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(148, 184)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
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
		Me.cmdCancel.Location = New System.Drawing.Point(228, 184)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblPrompt
		' 
		Me.lblPrompt.AutoSize = True
		Me.lblPrompt.BackColor = System.Drawing.SystemColors.Control
		Me.lblPrompt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPrompt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPrompt.Enabled = True
		Me.lblPrompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPrompt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPrompt.Location = New System.Drawing.Point(7, 7)
		Me.lblPrompt.Name = "lblPrompt"
		Me.lblPrompt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPrompt.Size = New System.Drawing.Size(296, 13)
		Me.lblPrompt.TabIndex = 2
		Me.lblPrompt.Text = "Label1"
		Me.lblPrompt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPrompt.UseMnemonic = True
		Me.lblPrompt.Visible = True
		' 
		' frmInputBox
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(312, 213)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtValue)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lblPrompt)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInputBox"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Data Entry Requirements"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class