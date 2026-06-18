<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDialogMsg
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
	Public WithEvents OKButton As System.Windows.Forms.Button
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents Picture1 As System.Windows.Forms.PictureBox
	Public WithEvents CancelButton_Renamed As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDialogMsg))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.OKButton = New System.Windows.Forms.Button
		Me.Text1 = New System.Windows.Forms.TextBox
		Me.Picture1 = New System.Windows.Forms.PictureBox
		Me.CancelButton_Renamed = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' OKButton
		' 
		Me.OKButton.BackColor = System.Drawing.SystemColors.Control
		Me.OKButton.CausesValidation = True
		Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
		Me.OKButton.Enabled = True
		Me.OKButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
		Me.OKButton.Location = New System.Drawing.Point(112, 112)
		Me.OKButton.Name = "OKButton"
		Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.OKButton.Size = New System.Drawing.Size(81, 25)
		Me.OKButton.TabIndex = 2
		Me.OKButton.TabStop = True
		Me.OKButton.Text = "&OK"
		Me.OKButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.OKButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Text1
		' 
		Me.Text1.AcceptsReturn = True
		Me.Text1.AutoSize = False
		Me.Text1.BackColor = System.Drawing.SystemColors.Window
		Me.Text1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Text1.CausesValidation = True
		Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.Text1.Enabled = True
		Me.Text1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Text1.HideSelection = True
		Me.Text1.Location = New System.Drawing.Point(136, 112)
		Me.Text1.MaxLength = 0
		Me.Text1.Multiline = False
		Me.Text1.Name = "Text1"
		Me.Text1.ReadOnly = False
		Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Text1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.Text1.Size = New System.Drawing.Size(41, 19)
		Me.Text1.TabIndex = 0
		Me.Text1.TabStop = True
		Me.Text1.Text = "Text1"
		Me.Text1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.Text1.Visible = True
		' 
		' Picture1
		' 
		Me.Picture1.BackColor = System.Drawing.SystemColors.Control
		Me.Picture1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Picture1.CausesValidation = True
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.Dock = System.Windows.Forms.DockStyle.None
		Me.Picture1.Enabled = True
		Me.Picture1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Picture1.Image = CType(resources.GetObject("Picture1.Image"), System.Drawing.Image)
		Me.Picture1.Location = New System.Drawing.Point(16, 24)
		Me.Picture1.Name = "Picture1"
		Me.Picture1.Size = New System.Drawing.Size(32, 32)
		Me.Picture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.Picture1.TabIndex = 4
		Me.Picture1.TabStop = True
		Me.Picture1.Visible = True
		' 
		' CancelButton_Renamed
		' 
		Me.CancelButton_Renamed.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton_Renamed.CausesValidation = True
		Me.CancelButton_Renamed.Cursor = System.Windows.Forms.Cursors.Default
		Me.CancelButton_Renamed.Enabled = True
		Me.CancelButton_Renamed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CancelButton_Renamed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CancelButton_Renamed.Location = New System.Drawing.Point(240, 112)
		Me.CancelButton_Renamed.Name = "CancelButton_Renamed"
		Me.CancelButton_Renamed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CancelButton_Renamed.Size = New System.Drawing.Size(81, 25)
		Me.CancelButton_Renamed.TabIndex = 3
		Me.CancelButton_Renamed.TabStop = True
		Me.CancelButton_Renamed.Text = "&Cancel"
		Me.CancelButton_Renamed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton_Renamed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(72, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(361, 73)
		Me.Label1.TabIndex = 1
		Me.Label1.Text = "As you have reduced the 'cover to' date then to ensure that the premium is adjusted correctly you should not amend any risk screen details as it may result in a change to the premium calculation. You will need to edit each risk on the policy to calculate the change in premium due to this change in 'cover to' date."
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmDialogMsg
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(449, 152)
		Me.ControlBox = True
		Me.Controls.Add(Me.OKButton)
		Me.Controls.Add(Me.Text1)
		Me.Controls.Add(Me.Picture1)
		Me.Controls.Add(Me.CancelButton_Renamed)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(184, 250)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDialogMsg"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Policy cover to date reduced"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class