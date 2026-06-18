<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents txtStatus As System.Windows.Forms.TextBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdConvert As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtStatus = New System.Windows.Forms.TextBox
		Me.cmdExit = New System.Windows.Forms.Button
		Me.cmdConvert = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtStatus
		' 
		Me.txtStatus.AcceptsReturn = True
		Me.txtStatus.AutoSize = False
		Me.txtStatus.BackColor = System.Drawing.SystemColors.Window
		Me.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStatus.CausesValidation = True
		Me.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStatus.Enabled = True
		Me.txtStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStatus.HideSelection = True
		Me.txtStatus.Location = New System.Drawing.Point(8, 40)
		Me.txtStatus.MaxLength = 0
		Me.txtStatus.Multiline = True
		Me.txtStatus.Name = "txtStatus"
		Me.txtStatus.ReadOnly = False
		Me.txtStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtStatus.Size = New System.Drawing.Size(393, 133)
		Me.txtStatus.TabIndex = 3
		Me.txtStatus.TabStop = True
		Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStatus.Visible = True
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(332, 184)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(69, 25)
		Me.cmdExit.TabIndex = 2
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdConvert
		' 
		Me.cmdConvert.BackColor = System.Drawing.SystemColors.Control
		Me.cmdConvert.CausesValidation = True
		Me.cmdConvert.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdConvert.Enabled = True
		Me.cmdConvert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdConvert.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdConvert.Location = New System.Drawing.Point(256, 184)
		Me.cmdConvert.Name = "cmdConvert"
		Me.cmdConvert.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdConvert.Size = New System.Drawing.Size(69, 25)
		Me.cmdConvert.TabIndex = 1
		Me.cmdConvert.TabStop = True
		Me.cmdConvert.Text = "Convert"
		Me.cmdConvert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(8, 12)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(293, 17)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Converts ZIP files in DME Store"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(411, 217)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtStatus)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.cmdConvert)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "DME Convert Zips"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class