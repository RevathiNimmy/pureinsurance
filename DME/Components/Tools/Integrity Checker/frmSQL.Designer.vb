<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSQL
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
	Public WithEvents txtFilename As System.Windows.Forms.TextBox
	Public WithEvents txtVBCode As System.Windows.Forms.TextBox
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents txtSQL As System.Windows.Forms.TextBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSQL))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtFilename = New System.Windows.Forms.TextBox
		Me.txtVBCode = New System.Windows.Forms.TextBox
		Me.Command1 = New System.Windows.Forms.Button
		Me.txtSQL = New System.Windows.Forms.TextBox
		Me.SuspendLayout()
		' 
		' txtFilename
		' 
		Me.txtFilename.AcceptsReturn = True
		Me.txtFilename.AutoSize = False
		Me.txtFilename.BackColor = System.Drawing.SystemColors.Window
		Me.txtFilename.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFilename.CausesValidation = True
		Me.txtFilename.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFilename.Enabled = True
		Me.txtFilename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFilename.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFilename.HideSelection = True
		Me.txtFilename.Location = New System.Drawing.Point(8, 200)
		Me.txtFilename.MaxLength = 0
		Me.txtFilename.Multiline = False
		Me.txtFilename.Name = "txtFilename"
		Me.txtFilename.ReadOnly = False
		Me.txtFilename.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFilename.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFilename.Size = New System.Drawing.Size(585, 19)
		Me.txtFilename.TabIndex = 1
		Me.txtFilename.TabStop = True
		Me.txtFilename.Text = "C:\Source\DME 1.5.2\Components\Tools\Integrity Checker\SQL\"
		Me.txtFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFilename.Visible = True
		' 
		' txtVBCode
		' 
		Me.txtVBCode.AcceptsReturn = True
		Me.txtVBCode.AutoSize = False
		Me.txtVBCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtVBCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtVBCode.CausesValidation = True
		Me.txtVBCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtVBCode.Enabled = True
		Me.txtVBCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtVBCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtVBCode.HideSelection = True
		Me.txtVBCode.Location = New System.Drawing.Point(8, 232)
		Me.txtVBCode.MaxLength = 0
		Me.txtVBCode.Multiline = True
		Me.txtVBCode.Name = "txtVBCode"
		Me.txtVBCode.ReadOnly = False
		Me.txtVBCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtVBCode.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.txtVBCode.Size = New System.Drawing.Size(809, 273)
		Me.txtVBCode.TabIndex = 2
		Me.txtVBCode.TabStop = True
		Me.txtVBCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtVBCode.Visible = True
		Me.txtVBCode.WordWrap = False
		' 
		' Command1
		' 
		Me.Command1.BackColor = System.Drawing.SystemColors.Control
		Me.Command1.CausesValidation = True
		Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Command1.Enabled = True
		Me.Command1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Command1.Location = New System.Drawing.Point(600, 197)
		Me.Command1.Name = "Command1"
		Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Command1.Size = New System.Drawing.Size(217, 25)
		Me.Command1.TabIndex = 3
		Me.Command1.TabStop = True
		Me.Command1.Text = "Load SQL"
		Me.Command1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Command1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtSQL
		' 
		Me.txtSQL.AcceptsReturn = True
		Me.txtSQL.AutoSize = False
		Me.txtSQL.BackColor = System.Drawing.SystemColors.Window
		Me.txtSQL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSQL.CausesValidation = True
		Me.txtSQL.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSQL.Enabled = True
		Me.txtSQL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSQL.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSQL.HideSelection = True
		Me.txtSQL.Location = New System.Drawing.Point(8, 8)
		Me.txtSQL.MaxLength = 0
		Me.txtSQL.Multiline = True
		Me.txtSQL.Name = "txtSQL"
		Me.txtSQL.ReadOnly = False
		Me.txtSQL.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.txtSQL.Size = New System.Drawing.Size(809, 177)
		Me.txtSQL.TabIndex = 0
		Me.txtSQL.TabStop = True
		Me.txtSQL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSQL.Visible = True
		Me.txtSQL.WordWrap = False
		' 
		' frmSQL
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(824, 517)
		Me.ControlBox = True
		Me.Controls.Add(Me.txtFilename)
		Me.Controls.Add(Me.txtVBCode)
		Me.Controls.Add(Me.Command1)
		Me.Controls.Add(Me.txtSQL)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSQL"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "SQL to VB convertor"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class