<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelPoliciesFailed
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
	Public WithEvents txtPolicies As System.Windows.Forms.TextBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCancelPoliciesFailed))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.txtPolicies = New System.Windows.Forms.TextBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.Image1 = New System.Windows.Forms.PictureBox
		Me.SuspendLayout()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(125, 175)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(56, 26)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtPolicies
		' 
		Me.txtPolicies.AcceptsReturn = True
		Me.txtPolicies.AutoSize = False
		Me.txtPolicies.BackColor = System.Drawing.SystemColors.Window
		Me.txtPolicies.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPolicies.CausesValidation = True
		Me.txtPolicies.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPolicies.Enabled = True
		Me.txtPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPolicies.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPolicies.HideSelection = True
		Me.txtPolicies.Location = New System.Drawing.Point(10, 50)
		Me.txtPolicies.MaxLength = 0
		Me.txtPolicies.Multiline = True
		Me.txtPolicies.Name = "txtPolicies"
		Me.txtPolicies.ReadOnly = False
		Me.txtPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPolicies.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPolicies.Size = New System.Drawing.Size(291, 121)
		Me.txtPolicies.TabIndex = 1
		Me.txtPolicies.TabStop = True
		Me.txtPolicies.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPolicies.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(45, 5)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(256, 31)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Policy cancellation cannot continue as the following list will note qoute."
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' Image1
		' 
		Me.Image1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Image1.Enabled = True
		Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
		Me.Image1.Location = New System.Drawing.Point(5, 10)
		Me.Image1.Name = "Image1"
		Me.Image1.Size = New System.Drawing.Size(32, 32)
		Me.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.Image1.Visible = True
		' 
		' frmCancelPoliciesFailed
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(312, 206)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.txtPolicies)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Image1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmCancelPoliciesFailed"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Cancel Policies Failed"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class