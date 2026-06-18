<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_Renamed()
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
	Public WithEvents lblFailed As System.Windows.Forms.Label
	Public WithEvents lblDone As System.Windows.Forms.Label
	Public WithEvents lblTotal As System.Windows.Forms.Label
	Public WithEvents lblDocsFailed As System.Windows.Forms.Label
	Public WithEvents lblDocsDone As System.Windows.Forms.Label
	Public WithEvents lblDocsTotal As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents tmrProg As System.Windows.Forms.Timer
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents proCommit As System.Windows.Forms.ProgressBar
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Frame2 = New System.Windows.Forms.GroupBox
		Me.lblFailed = New System.Windows.Forms.Label
		Me.lblDone = New System.Windows.Forms.Label
		Me.lblTotal = New System.Windows.Forms.Label
		Me.lblDocsFailed = New System.Windows.Forms.Label
		Me.lblDocsDone = New System.Windows.Forms.Label
		Me.lblDocsTotal = New System.Windows.Forms.Label
		Me.tmrProg = New System.Windows.Forms.Timer(components)
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.proCommit = New System.Windows.Forms.ProgressBar
        Me.Frame2.SuspendLayout()
		Me.SuspendLayout()

		' 
		' Frame2
		' 
		Me.Frame2.BackColor = System.Drawing.SystemColors.Control
		Me.Frame2.Controls.Add(Me.lblFailed)
		Me.Frame2.Controls.Add(Me.lblDone)
		Me.Frame2.Controls.Add(Me.lblTotal)
		Me.Frame2.Controls.Add(Me.lblDocsFailed)
		Me.Frame2.Controls.Add(Me.lblDocsDone)
		Me.Frame2.Controls.Add(Me.lblDocsTotal)
		Me.Frame2.Enabled = True
		Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame2.Location = New System.Drawing.Point(16, 8)
		Me.Frame2.Name = "Frame2"
		Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame2.Size = New System.Drawing.Size(129, 81)
		Me.Frame2.TabIndex = 2
		Me.Frame2.Text = "Document Progress"
		Me.Frame2.Visible = True
		' 
		' lblFailed
		' 
		Me.lblFailed.AutoSize = False
		Me.lblFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFailed.Enabled = True
		Me.lblFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFailed.Location = New System.Drawing.Point(64, 56)
		Me.lblFailed.Name = "lblFailed"
		Me.lblFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFailed.Size = New System.Drawing.Size(49, 17)
		Me.lblFailed.TabIndex = 10
		Me.lblFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFailed.UseMnemonic = True
		Me.lblFailed.Visible = True
		' 
		' lblDone
		' 
		Me.lblDone.AutoSize = False
		Me.lblDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDone.Enabled = True
		Me.lblDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDone.Location = New System.Drawing.Point(64, 40)
		Me.lblDone.Name = "lblDone"
		Me.lblDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDone.Size = New System.Drawing.Size(57, 17)
		Me.lblDone.TabIndex = 9
		Me.lblDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDone.UseMnemonic = True
		Me.lblDone.Visible = True
		' 
		' lblTotal
		' 
		Me.lblTotal.AutoSize = False
		Me.lblTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotal.Enabled = True
		Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotal.Location = New System.Drawing.Point(64, 24)
		Me.lblTotal.Name = "lblTotal"
		Me.lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotal.Size = New System.Drawing.Size(49, 17)
		Me.lblTotal.TabIndex = 8
		Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotal.UseMnemonic = True
		Me.lblTotal.Visible = True
		' 
		' lblDocsFailed
		' 
		Me.lblDocsFailed.AutoSize = False
		Me.lblDocsFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocsFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocsFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocsFailed.Enabled = True
		Me.lblDocsFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocsFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocsFailed.Location = New System.Drawing.Point(16, 56)
		Me.lblDocsFailed.Name = "lblDocsFailed"
		Me.lblDocsFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocsFailed.Size = New System.Drawing.Size(41, 17)
		Me.lblDocsFailed.TabIndex = 5
		Me.lblDocsFailed.Text = "Failed   :"
		Me.lblDocsFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocsFailed.UseMnemonic = True
		Me.lblDocsFailed.Visible = True
		' 
		' lblDocsDone
		' 
		Me.lblDocsDone.AutoSize = False
		Me.lblDocsDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocsDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocsDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocsDone.Enabled = True
		Me.lblDocsDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocsDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocsDone.Location = New System.Drawing.Point(16, 40)
		Me.lblDocsDone.Name = "lblDocsDone"
		Me.lblDocsDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocsDone.Size = New System.Drawing.Size(65, 17)
		Me.lblDocsDone.TabIndex = 4
		Me.lblDocsDone.Text = "Done   :"
		Me.lblDocsDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocsDone.UseMnemonic = True
		Me.lblDocsDone.Visible = True
		' 
		' lblDocsTotal
		' 
		Me.lblDocsTotal.AutoSize = False
		Me.lblDocsTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocsTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocsTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocsTotal.Enabled = True
		Me.lblDocsTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocsTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocsTotal.Location = New System.Drawing.Point(16, 24)
		Me.lblDocsTotal.Name = "lblDocsTotal"
		Me.lblDocsTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocsTotal.Size = New System.Drawing.Size(57, 17)
		Me.lblDocsTotal.TabIndex = 3
		Me.lblDocsTotal.Text = "Total    :"
		Me.lblDocsTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocsTotal.UseMnemonic = True
		Me.lblDocsTotal.Visible = True
		' 
		' tmrProg
		' 
		Me.tmrProg.Enabled = False
		Me.tmrProg.Interval = 1000
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(368, 136)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 1
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(280, 136)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' proCommit
		' 
		Me.proCommit.Location = New System.Drawing.Point(16, 104)
		Me.proCommit.Name = "proCommit"
		Me.proCommit.Size = New System.Drawing.Size(433, 17)
		Me.proCommit.TabIndex = 7
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(464, 169)
		Me.ControlBox = True
		Me.Controls.Add(Me.Frame2)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.proCommit)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(673, 603)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Commit Batch"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame2.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class