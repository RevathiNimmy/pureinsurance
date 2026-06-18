<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPMSiriusSupport
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
	Public WithEvents lblWait As System.Windows.Forms.Label
	Public WithEvents lblLaunching As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPMSiriusSupport))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lblWait = New System.Windows.Forms.Label
		Me.lblLaunching = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' lblWait
		' 
		Me.lblWait.AutoSize = False
		Me.lblWait.BackColor = System.Drawing.SystemColors.Control
		Me.lblWait.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblWait.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblWait.Enabled = True
		Me.lblWait.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblWait.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblWait.Location = New System.Drawing.Point(56, 8)
		Me.lblWait.Name = "lblWait"
		Me.lblWait.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblWait.Size = New System.Drawing.Size(89, 17)
		Me.lblWait.TabIndex = 1
		Me.lblWait.Text = "Please Wait"
		Me.lblWait.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblWait.UseMnemonic = True
		Me.lblWait.Visible = True
		' 
		' lblLaunching
		' 
		Me.lblLaunching.AutoSize = False
		Me.lblLaunching.BackColor = System.Drawing.SystemColors.Control
		Me.lblLaunching.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLaunching.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLaunching.Enabled = True
		Me.lblLaunching.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblLaunching.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLaunching.Location = New System.Drawing.Point(8, 48)
		Me.lblLaunching.Name = "lblLaunching"
		Me.lblLaunching.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLaunching.Size = New System.Drawing.Size(193, 33)
		Me.lblLaunching.TabIndex = 0
		Me.lblLaunching.Text = "Launching Internet Explorer to display Policy Master Sirius Support Web Page."
		Me.lblLaunching.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLaunching.UseMnemonic = True
		Me.lblLaunching.Visible = True
		' 
		' frmPMSiriusSupport
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(215, 93)
		Me.ControlBox = True
		Me.Controls.Add(Me.lblWait)
		Me.Controls.Add(Me.lblLaunching)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmPMSiriusSupport.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(403, 328)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmPMSiriusSupport"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "PM Sirius Support"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class