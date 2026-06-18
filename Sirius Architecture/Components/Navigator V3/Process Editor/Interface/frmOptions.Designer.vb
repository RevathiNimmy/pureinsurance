<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
	Public WithEvents cboDSN As System.Windows.Forms.ComboBox
	Public WithEvents lblDSN As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cboDSN = New System.Windows.Forms.ComboBox
		Me.lblDSN = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(224, 64)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 25)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cboDSN
		' 
		Me.cboDSN.BackColor = System.Drawing.SystemColors.Window
		Me.cboDSN.CausesValidation = True
		Me.cboDSN.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDSN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDSN.Enabled = True
		Me.cboDSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDSN.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDSN.IntegralHeight = True
		Me.cboDSN.Location = New System.Drawing.Point(8, 24)
		Me.cboDSN.Name = "cboDSN"
		Me.cboDSN.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDSN.Size = New System.Drawing.Size(289, 21)
		Me.cboDSN.Sorted = False
		Me.cboDSN.TabIndex = 0
		Me.cboDSN.TabStop = True
		Me.cboDSN.Visible = True
		' 
		' lblDSN
		' 
		Me.lblDSN.AutoSize = False
		Me.lblDSN.BackColor = System.Drawing.SystemColors.Control
		Me.lblDSN.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDSN.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDSN.Enabled = True
		Me.lblDSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDSN.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDSN.Location = New System.Drawing.Point(8, 8)
		Me.lblDSN.Name = "lblDSN"
		Me.lblDSN.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDSN.Size = New System.Drawing.Size(81, 17)
		Me.lblDSN.TabIndex = 2
		Me.lblDSN.Text = "System DSN"
		Me.lblDSN.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDSN.UseMnemonic = True
		Me.lblDSN.Visible = True
		' 
		' frmOptions
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(307, 111)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cboDSN)
		Me.Controls.Add(Me.lblDSN)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmOptions.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmOptions"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Navigator Editor Options"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class