<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
	Public WithEvents cmdTotalTax As System.Windows.Forms.Button
	Public WithEvents txtTask As System.Windows.Forms.TextBox
	Public WithEvents txtTransactionType As System.Windows.Forms.TextBox
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cmdRecalc As System.Windows.Forms.Button
	Public WithEvents Text3 As System.Windows.Forms.TextBox
	Public WithEvents txtReadOnly As System.Windows.Forms.TextBox
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents txtRiskCnt As System.Windows.Forms.TextBox
	Public WithEvents cmdLoad As System.Windows.Forms.Button
	Public WithEvents uctPMURITax1 As uctPMURITaxControl.uctPMURITax
	Public WithEvents lblTask As System.Windows.Forms.Label
	Public WithEvents lblTransactionType As System.Windows.Forms.Label
	Public WithEvents lblTotalTax As System.Windows.Forms.Label
	Public WithEvents lblReadOnly As System.Windows.Forms.Label
	Public WithEvents lblInsFileCnt As System.Windows.Forms.Label
	Public WithEvents lblRiskCnt As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdTotalTax = New System.Windows.Forms.Button
		Me.txtTask = New System.Windows.Forms.TextBox
		Me.txtTransactionType = New System.Windows.Forms.TextBox
		Me.cmdSave = New System.Windows.Forms.Button
		Me.cmdRecalc = New System.Windows.Forms.Button
		Me.Text3 = New System.Windows.Forms.TextBox
		Me.txtReadOnly = New System.Windows.Forms.TextBox
		Me.Text1 = New System.Windows.Forms.TextBox
		Me.txtRiskCnt = New System.Windows.Forms.TextBox
		Me.cmdLoad = New System.Windows.Forms.Button
		Me.uctPMURITax1 = New uctPMURITaxControl.uctPMURITax
		Me.lblTask = New System.Windows.Forms.Label
		Me.lblTransactionType = New System.Windows.Forms.Label
		Me.lblTotalTax = New System.Windows.Forms.Label
		Me.lblReadOnly = New System.Windows.Forms.Label
		Me.lblInsFileCnt = New System.Windows.Forms.Label
		Me.lblRiskCnt = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' cmdTotalTax
		' 
		Me.cmdTotalTax.BackColor = System.Drawing.SystemColors.Control
		Me.cmdTotalTax.CausesValidation = True
		Me.cmdTotalTax.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdTotalTax.Enabled = True
		Me.cmdTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdTotalTax.Location = New System.Drawing.Point(112, 192)
		Me.cmdTotalTax.Name = "cmdTotalTax"
		Me.cmdTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdTotalTax.Size = New System.Drawing.Size(89, 33)
		Me.cmdTotalTax.TabIndex = 16
		Me.cmdTotalTax.TabStop = True
		Me.cmdTotalTax.Text = "Get Total Tax"
		Me.cmdTotalTax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdTotalTax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtTask
		' 
		Me.txtTask.AcceptsReturn = True
		Me.txtTask.AutoSize = False
		Me.txtTask.BackColor = System.Drawing.SystemColors.Window
		Me.txtTask.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTask.CausesValidation = True
		Me.txtTask.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTask.Enabled = True
		Me.txtTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTask.HideSelection = True
		Me.txtTask.Location = New System.Drawing.Point(680, 224)
		Me.txtTask.MaxLength = 0
		Me.txtTask.Multiline = False
		Me.txtTask.Name = "txtTask"
		Me.txtTask.ReadOnly = False
		Me.txtTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTask.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTask.Size = New System.Drawing.Size(33, 21)
		Me.txtTask.TabIndex = 14
		Me.txtTask.TabStop = True
		Me.txtTask.Text = "2"
		Me.txtTask.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTask.Visible = True
		' 
		' txtTransactionType
		' 
		Me.txtTransactionType.AcceptsReturn = True
		Me.txtTransactionType.AutoSize = False
		Me.txtTransactionType.BackColor = System.Drawing.SystemColors.Window
		Me.txtTransactionType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTransactionType.CausesValidation = True
		Me.txtTransactionType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTransactionType.Enabled = True
		Me.txtTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTransactionType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTransactionType.HideSelection = True
		Me.txtTransactionType.Location = New System.Drawing.Point(584, 196)
		Me.txtTransactionType.MaxLength = 0
		Me.txtTransactionType.Multiline = False
		Me.txtTransactionType.Name = "txtTransactionType"
		Me.txtTransactionType.ReadOnly = False
		Me.txtTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTransactionType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTransactionType.Size = New System.Drawing.Size(113, 21)
		Me.txtTransactionType.TabIndex = 12
		Me.txtTransactionType.TabStop = True
		Me.txtTransactionType.Text = "NB"
		Me.txtTransactionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTransactionType.Visible = True
		' 
		' cmdSave
		' 
		Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSave.CausesValidation = True
		Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSave.Enabled = True
		Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSave.Location = New System.Drawing.Point(16, 272)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSave.Size = New System.Drawing.Size(89, 33)
		Me.cmdSave.TabIndex = 11
		Me.cmdSave.TabStop = True
		Me.cmdSave.Text = "Save"
		Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRecalc
		' 
		Me.cmdRecalc.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRecalc.CausesValidation = True
		Me.cmdRecalc.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRecalc.Enabled = True
		Me.cmdRecalc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRecalc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRecalc.Location = New System.Drawing.Point(16, 232)
		Me.cmdRecalc.Name = "cmdRecalc"
		Me.cmdRecalc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRecalc.Size = New System.Drawing.Size(89, 33)
		Me.cmdRecalc.TabIndex = 10
		Me.cmdRecalc.TabStop = True
		Me.cmdRecalc.Text = "Recalculate"
		Me.cmdRecalc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRecalc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Text3
		' 
		Me.Text3.AcceptsReturn = True
		Me.Text3.AutoSize = False
		Me.Text3.BackColor = System.Drawing.SystemColors.Window
		Me.Text3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Text3.CausesValidation = True
		Me.Text3.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.Text3.Enabled = True
		Me.Text3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Text3.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Text3.HideSelection = True
		Me.Text3.Location = New System.Drawing.Point(352, 280)
		Me.Text3.MaxLength = 0
		Me.Text3.Multiline = False
		Me.Text3.Name = "Text3"
		Me.Text3.ReadOnly = False
		Me.Text3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Text3.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.Text3.Size = New System.Drawing.Size(113, 21)
		Me.Text3.TabIndex = 7
		Me.Text3.TabStop = True
		Me.Text3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.Text3.Visible = True
		' 
		' txtReadOnly
		' 
		Me.txtReadOnly.AcceptsReturn = True
		Me.txtReadOnly.AutoSize = False
		Me.txtReadOnly.BackColor = System.Drawing.SystemColors.Window
		Me.txtReadOnly.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReadOnly.CausesValidation = True
		Me.txtReadOnly.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReadOnly.Enabled = True
		Me.txtReadOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReadOnly.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReadOnly.HideSelection = True
		Me.txtReadOnly.Location = New System.Drawing.Point(352, 252)
		Me.txtReadOnly.MaxLength = 0
		Me.txtReadOnly.Multiline = False
		Me.txtReadOnly.Name = "txtReadOnly"
		Me.txtReadOnly.ReadOnly = False
		Me.txtReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReadOnly.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReadOnly.Size = New System.Drawing.Size(113, 21)
		Me.txtReadOnly.TabIndex = 6
		Me.txtReadOnly.TabStop = True
		Me.txtReadOnly.Text = "false"
		Me.txtReadOnly.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReadOnly.Visible = True
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
		Me.Text1.Location = New System.Drawing.Point(352, 224)
		Me.Text1.MaxLength = 0
		Me.Text1.Multiline = False
		Me.Text1.Name = "Text1"
		Me.Text1.ReadOnly = False
		Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Text1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.Text1.Size = New System.Drawing.Size(113, 21)
		Me.Text1.TabIndex = 4
		Me.Text1.TabStop = True
		Me.Text1.Text = "85261"
		Me.Text1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.Text1.Visible = True
		' 
		' txtRiskCnt
		' 
		Me.txtRiskCnt.AcceptsReturn = True
		Me.txtRiskCnt.AutoSize = False
		Me.txtRiskCnt.BackColor = System.Drawing.SystemColors.Window
		Me.txtRiskCnt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtRiskCnt.CausesValidation = True
		Me.txtRiskCnt.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRiskCnt.Enabled = True
		Me.txtRiskCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtRiskCnt.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRiskCnt.HideSelection = True
		Me.txtRiskCnt.Location = New System.Drawing.Point(352, 196)
		Me.txtRiskCnt.MaxLength = 0
		Me.txtRiskCnt.Multiline = False
		Me.txtRiskCnt.Name = "txtRiskCnt"
		Me.txtRiskCnt.ReadOnly = False
		Me.txtRiskCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRiskCnt.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRiskCnt.Size = New System.Drawing.Size(113, 21)
		Me.txtRiskCnt.TabIndex = 2
		Me.txtRiskCnt.TabStop = True
		Me.txtRiskCnt.Text = "121757"
		Me.txtRiskCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRiskCnt.Visible = True
		' 
		' cmdLoad
		' 
		Me.cmdLoad.BackColor = System.Drawing.SystemColors.Control
		Me.cmdLoad.CausesValidation = True
		Me.cmdLoad.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdLoad.Enabled = True
		Me.cmdLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdLoad.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdLoad.Location = New System.Drawing.Point(16, 192)
		Me.cmdLoad.Name = "cmdLoad"
		Me.cmdLoad.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdLoad.Size = New System.Drawing.Size(89, 33)
		Me.cmdLoad.TabIndex = 1
		Me.cmdLoad.TabStop = True
		Me.cmdLoad.Text = "Load"
		Me.cmdLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMURITax1
		' 
		Me.uctPMURITax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.uctPMURITax1.Location = New System.Drawing.Point(8, 8)
		Me.uctPMURITax1.Name = "uctPMURITax1"
		Me.uctPMURITax1.Size = New System.Drawing.Size(713, 177)
		Me.uctPMURITax1.TabIndex = 0
		' 
		' lblTask
		' 
		Me.lblTask.AutoSize = True
		Me.lblTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTask.Enabled = True
		Me.lblTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTask.Location = New System.Drawing.Point(480, 228)
		Me.lblTask.Name = "lblTask"
		Me.lblTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTask.Size = New System.Drawing.Size(187, 13)
		Me.lblTask.TabIndex = 15
		Me.lblTask.Text = "Task (Edit =2, Add =1, View =0)"
		Me.lblTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTask.UseMnemonic = True
		Me.lblTask.Visible = True
		' 
		' lblTransactionType
		' 
		Me.lblTransactionType.AutoSize = True
		Me.lblTransactionType.BackColor = System.Drawing.SystemColors.Control
		Me.lblTransactionType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTransactionType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTransactionType.Enabled = True
		Me.lblTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTransactionType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTransactionType.Location = New System.Drawing.Point(480, 200)
		Me.lblTransactionType.Name = "lblTransactionType"
		Me.lblTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTransactionType.Size = New System.Drawing.Size(94, 13)
		Me.lblTransactionType.TabIndex = 13
		Me.lblTransactionType.Text = "TransactionType"
		Me.lblTransactionType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTransactionType.UseMnemonic = True
		Me.lblTransactionType.Visible = True
		' 
		' lblTotalTax
		' 
		Me.lblTotalTax.AutoSize = True
		Me.lblTotalTax.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotalTax.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotalTax.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotalTax.Enabled = True
		Me.lblTotalTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotalTax.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotalTax.Location = New System.Drawing.Point(291, 284)
		Me.lblTotalTax.Name = "lblTotalTax"
		Me.lblTotalTax.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotalTax.Size = New System.Drawing.Size(53, 13)
		Me.lblTotalTax.TabIndex = 9
		Me.lblTotalTax.Text = "Total Tax"
		Me.lblTotalTax.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTotalTax.UseMnemonic = True
		Me.lblTotalTax.Visible = True
		' 
		' lblReadOnly
		' 
		Me.lblReadOnly.AutoSize = True
		Me.lblReadOnly.BackColor = System.Drawing.SystemColors.Control
		Me.lblReadOnly.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReadOnly.Enabled = True
		Me.lblReadOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblReadOnly.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReadOnly.Location = New System.Drawing.Point(286, 256)
		Me.lblReadOnly.Name = "lblReadOnly"
		Me.lblReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReadOnly.Size = New System.Drawing.Size(59, 13)
		Me.lblReadOnly.TabIndex = 8
		Me.lblReadOnly.Text = "Read Only"
		Me.lblReadOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReadOnly.UseMnemonic = True
		Me.lblReadOnly.Visible = True
		' 
		' lblInsFileCnt
		' 
		Me.lblInsFileCnt.AutoSize = True
		Me.lblInsFileCnt.BackColor = System.Drawing.SystemColors.Control
		Me.lblInsFileCnt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInsFileCnt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInsFileCnt.Enabled = True
		Me.lblInsFileCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInsFileCnt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInsFileCnt.Location = New System.Drawing.Point(287, 228)
		Me.lblInsFileCnt.Name = "lblInsFileCnt"
		Me.lblInsFileCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInsFileCnt.Size = New System.Drawing.Size(62, 13)
		Me.lblInsFileCnt.TabIndex = 5
		Me.lblInsFileCnt.Text = "InsFileCnt:"
		Me.lblInsFileCnt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInsFileCnt.UseMnemonic = True
		Me.lblInsFileCnt.Visible = True
		' 
		' lblRiskCnt
		' 
		Me.lblRiskCnt.AutoSize = True
		Me.lblRiskCnt.BackColor = System.Drawing.SystemColors.Control
		Me.lblRiskCnt.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblRiskCnt.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblRiskCnt.Enabled = True
		Me.lblRiskCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblRiskCnt.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblRiskCnt.Location = New System.Drawing.Point(296, 200)
		Me.lblRiskCnt.Name = "lblRiskCnt"
		Me.lblRiskCnt.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblRiskCnt.Size = New System.Drawing.Size(40, 13)
		Me.lblRiskCnt.TabIndex = 3
		Me.lblRiskCnt.Text = "Risk Cnt"
		Me.lblRiskCnt.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblRiskCnt.UseMnemonic = True
		Me.lblRiskCnt.Visible = True
		' 
		' Form1
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(729, 314)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdTotalTax)
		Me.Controls.Add(Me.txtTask)
		Me.Controls.Add(Me.txtTransactionType)
		Me.Controls.Add(Me.cmdSave)
		Me.Controls.Add(Me.cmdRecalc)
		Me.Controls.Add(Me.Text3)
		Me.Controls.Add(Me.txtReadOnly)
		Me.Controls.Add(Me.Text1)
		Me.Controls.Add(Me.txtRiskCnt)
		Me.Controls.Add(Me.cmdLoad)
		Me.Controls.Add(Me.uctPMURITax1)
		Me.Controls.Add(Me.lblTask)
		Me.Controls.Add(Me.lblTransactionType)
		Me.Controls.Add(Me.lblTotalTax)
		Me.Controls.Add(Me.lblReadOnly)
		Me.Controls.Add(Me.lblInsFileCnt)
		Me.Controls.Add(Me.lblRiskCnt)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "Form1"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class