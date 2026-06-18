<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdViewReport As System.Windows.Forms.Button
	Public WithEvents proOther As System.Windows.Forms.ProgressBar
	Public WithEvents lblOtherFailed As System.Windows.Forms.Label
	Public WithEvents lblOtherDone As System.Windows.Forms.Label
	Public WithEvents lblOtherTotal As System.Windows.Forms.Label
	Public WithEvents Label19 As System.Windows.Forms.Label
	Public WithEvents Label18 As System.Windows.Forms.Label
	Public WithEvents Label17 As System.Windows.Forms.Label
	Public WithEvents Frame5 As System.Windows.Forms.GroupBox
	Public WithEvents proFold As System.Windows.Forms.ProgressBar
	Public WithEvents lblFoldFailed As System.Windows.Forms.Label
	Public WithEvents lblFoldDone As System.Windows.Forms.Label
	Public WithEvents lblFoldTotal As System.Windows.Forms.Label
	Public WithEvents Label13 As System.Windows.Forms.Label
	Public WithEvents Label12 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents proDraw As System.Windows.Forms.ProgressBar
	Public WithEvents lblDrawFailed As System.Windows.Forms.Label
	Public WithEvents lblDrawDone As System.Windows.Forms.Label
	Public WithEvents lblDrawTotal As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents proCab As System.Windows.Forms.ProgressBar
	Public WithEvents lblCabFailed As System.Windows.Forms.Label
	Public WithEvents lblCabDone As System.Windows.Forms.Label
	Public WithEvents lblCabTotal As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblCabName As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdAbort As System.Windows.Forms.Button
	Public WithEvents cmdStart As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdViewReport = New System.Windows.Forms.Button
		Me.Frame5 = New System.Windows.Forms.GroupBox
		Me.proOther = New System.Windows.Forms.ProgressBar
		Me.lblOtherFailed = New System.Windows.Forms.Label
		Me.lblOtherDone = New System.Windows.Forms.Label
		Me.lblOtherTotal = New System.Windows.Forms.Label
		Me.Label19 = New System.Windows.Forms.Label
		Me.Label18 = New System.Windows.Forms.Label
		Me.Label17 = New System.Windows.Forms.Label
		Me.Frame3 = New System.Windows.Forms.GroupBox
		Me.proFold = New System.Windows.Forms.ProgressBar
		Me.lblFoldFailed = New System.Windows.Forms.Label
		Me.lblFoldDone = New System.Windows.Forms.Label
		Me.lblFoldTotal = New System.Windows.Forms.Label
		Me.Label13 = New System.Windows.Forms.Label
		Me.Label12 = New System.Windows.Forms.Label
		Me.Label11 = New System.Windows.Forms.Label
		Me.Frame2 = New System.Windows.Forms.GroupBox
		Me.proDraw = New System.Windows.Forms.ProgressBar
		Me.lblDrawFailed = New System.Windows.Forms.Label
		Me.lblDrawDone = New System.Windows.Forms.Label
		Me.lblDrawTotal = New System.Windows.Forms.Label
		Me.Label10 = New System.Windows.Forms.Label
		Me.Label9 = New System.Windows.Forms.Label
		Me.Label8 = New System.Windows.Forms.Label
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.proCab = New System.Windows.Forms.ProgressBar
		Me.lblCabFailed = New System.Windows.Forms.Label
		Me.lblCabDone = New System.Windows.Forms.Label
		Me.lblCabTotal = New System.Windows.Forms.Label
		Me.Label7 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.lblCabName = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.cmdAbort = New System.Windows.Forms.Button
		Me.cmdStart = New System.Windows.Forms.Button
		Me.Frame5.SuspendLayout()
		Me.Frame3.SuspendLayout()
		Me.Frame2.SuspendLayout()
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdViewReport
		' 
		Me.cmdViewReport.BackColor = System.Drawing.SystemColors.Control
		Me.cmdViewReport.CausesValidation = True
		Me.cmdViewReport.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdViewReport.Enabled = False
		Me.cmdViewReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdViewReport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdViewReport.Location = New System.Drawing.Point(16, 368)
		Me.cmdViewReport.Name = "cmdViewReport"
		Me.cmdViewReport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdViewReport.Size = New System.Drawing.Size(81, 33)
		Me.cmdViewReport.TabIndex = 12
		Me.cmdViewReport.TabStop = True
		Me.cmdViewReport.Text = "View Report"
		Me.cmdViewReport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdViewReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Frame5
		' 
		Me.Frame5.BackColor = System.Drawing.SystemColors.Control
		Me.Frame5.Controls.Add(Me.proOther)
		Me.Frame5.Controls.Add(Me.lblOtherFailed)
		Me.Frame5.Controls.Add(Me.lblOtherDone)
		Me.Frame5.Controls.Add(Me.lblOtherTotal)
		Me.Frame5.Controls.Add(Me.Label19)
		Me.Frame5.Controls.Add(Me.Label18)
		Me.Frame5.Controls.Add(Me.Label17)
		Me.Frame5.Enabled = True
		Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame5.Location = New System.Drawing.Point(8, 272)
		Me.Frame5.Name = "Frame5"
		Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame5.Size = New System.Drawing.Size(465, 73)
		Me.Frame5.TabIndex = 10
		Me.Frame5.Text = "Document Names and Keywords"
		Me.Frame5.Visible = True
		' 
		' proOther
		' 
		Me.proOther.Location = New System.Drawing.Point(16, 32)
		Me.proOther.Name = "proOther"
		Me.proOther.Size = New System.Drawing.Size(321, 17)
		Me.proOther.TabIndex = 11
		' 
		' lblOtherFailed
		' 
		Me.lblOtherFailed.AutoSize = False
		Me.lblOtherFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblOtherFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOtherFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOtherFailed.Enabled = True
		Me.lblOtherFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOtherFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOtherFailed.Location = New System.Drawing.Point(400, 48)
		Me.lblOtherFailed.Name = "lblOtherFailed"
		Me.lblOtherFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOtherFailed.Size = New System.Drawing.Size(57, 17)
		Me.lblOtherFailed.TabIndex = 36
		Me.lblOtherFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOtherFailed.UseMnemonic = True
		Me.lblOtherFailed.Visible = True
		' 
		' lblOtherDone
		' 
		Me.lblOtherDone.AutoSize = False
		Me.lblOtherDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblOtherDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOtherDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOtherDone.Enabled = True
		Me.lblOtherDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOtherDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOtherDone.Location = New System.Drawing.Point(400, 32)
		Me.lblOtherDone.Name = "lblOtherDone"
		Me.lblOtherDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOtherDone.Size = New System.Drawing.Size(57, 17)
		Me.lblOtherDone.TabIndex = 35
		Me.lblOtherDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOtherDone.UseMnemonic = True
		Me.lblOtherDone.Visible = True
		' 
		' lblOtherTotal
		' 
		Me.lblOtherTotal.AutoSize = False
		Me.lblOtherTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblOtherTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOtherTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOtherTotal.Enabled = True
		Me.lblOtherTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOtherTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOtherTotal.Location = New System.Drawing.Point(400, 16)
		Me.lblOtherTotal.Name = "lblOtherTotal"
		Me.lblOtherTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOtherTotal.Size = New System.Drawing.Size(57, 17)
		Me.lblOtherTotal.TabIndex = 34
		Me.lblOtherTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOtherTotal.UseMnemonic = True
		Me.lblOtherTotal.Visible = True
		' 
		' Label19
		' 
		Me.Label19.AutoSize = False
		Me.Label19.BackColor = System.Drawing.SystemColors.Control
		Me.Label19.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label19.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label19.Enabled = True
		Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label19.Location = New System.Drawing.Point(360, 48)
		Me.Label19.Name = "Label19"
		Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label19.Size = New System.Drawing.Size(49, 17)
		Me.Label19.TabIndex = 24
		Me.Label19.Text = "Failed  :"
		Me.Label19.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label19.UseMnemonic = True
		Me.Label19.Visible = True
		' 
		' Label18
		' 
		Me.Label18.AutoSize = False
		Me.Label18.BackColor = System.Drawing.SystemColors.Control
		Me.Label18.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label18.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label18.Enabled = True
		Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label18.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label18.Location = New System.Drawing.Point(360, 32)
		Me.Label18.Name = "Label18"
		Me.Label18.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label18.Size = New System.Drawing.Size(49, 17)
		Me.Label18.TabIndex = 23
		Me.Label18.Text = "Done  :"
		Me.Label18.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label18.UseMnemonic = True
		Me.Label18.Visible = True
		' 
		' Label17
		' 
		Me.Label17.AutoSize = False
		Me.Label17.BackColor = System.Drawing.SystemColors.Control
		Me.Label17.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label17.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label17.Enabled = True
		Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label17.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label17.Location = New System.Drawing.Point(360, 16)
		Me.Label17.Name = "Label17"
		Me.Label17.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label17.Size = New System.Drawing.Size(49, 17)
		Me.Label17.TabIndex = 22
		Me.Label17.Text = "Total   :"
		Me.Label17.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label17.UseMnemonic = True
		Me.Label17.Visible = True
		' 
		' Frame3
		' 
		Me.Frame3.BackColor = System.Drawing.SystemColors.Control
		Me.Frame3.Controls.Add(Me.proFold)
		Me.Frame3.Controls.Add(Me.lblFoldFailed)
		Me.Frame3.Controls.Add(Me.lblFoldDone)
		Me.Frame3.Controls.Add(Me.lblFoldTotal)
		Me.Frame3.Controls.Add(Me.Label13)
		Me.Frame3.Controls.Add(Me.Label12)
		Me.Frame3.Controls.Add(Me.Label11)
		Me.Frame3.Enabled = True
		Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame3.Location = New System.Drawing.Point(8, 184)
		Me.Frame3.Name = "Frame3"
		Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame3.Size = New System.Drawing.Size(465, 73)
		Me.Frame3.TabIndex = 8
		Me.Frame3.Text = "Folders"
		Me.Frame3.Visible = True
		' 
		' proFold
		' 
		Me.proFold.Location = New System.Drawing.Point(16, 32)
		Me.proFold.Name = "proFold"
		Me.proFold.Size = New System.Drawing.Size(321, 17)
		Me.proFold.TabIndex = 9
		' 
		' lblFoldFailed
		' 
		Me.lblFoldFailed.AutoSize = False
		Me.lblFoldFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblFoldFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFoldFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFoldFailed.Enabled = True
		Me.lblFoldFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFoldFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFoldFailed.Location = New System.Drawing.Point(400, 48)
		Me.lblFoldFailed.Name = "lblFoldFailed"
		Me.lblFoldFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFoldFailed.Size = New System.Drawing.Size(57, 17)
		Me.lblFoldFailed.TabIndex = 33
		Me.lblFoldFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFoldFailed.UseMnemonic = True
		Me.lblFoldFailed.Visible = True
		' 
		' lblFoldDone
		' 
		Me.lblFoldDone.AutoSize = False
		Me.lblFoldDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblFoldDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFoldDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFoldDone.Enabled = True
		Me.lblFoldDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFoldDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFoldDone.Location = New System.Drawing.Point(400, 32)
		Me.lblFoldDone.Name = "lblFoldDone"
		Me.lblFoldDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFoldDone.Size = New System.Drawing.Size(57, 17)
		Me.lblFoldDone.TabIndex = 32
		Me.lblFoldDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFoldDone.UseMnemonic = True
		Me.lblFoldDone.Visible = True
		' 
		' lblFoldTotal
		' 
		Me.lblFoldTotal.AutoSize = False
		Me.lblFoldTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblFoldTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFoldTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFoldTotal.Enabled = True
		Me.lblFoldTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFoldTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFoldTotal.Location = New System.Drawing.Point(400, 16)
		Me.lblFoldTotal.Name = "lblFoldTotal"
		Me.lblFoldTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFoldTotal.Size = New System.Drawing.Size(57, 17)
		Me.lblFoldTotal.TabIndex = 31
		Me.lblFoldTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFoldTotal.UseMnemonic = True
		Me.lblFoldTotal.Visible = True
		' 
		' Label13
		' 
		Me.Label13.AutoSize = False
		Me.Label13.BackColor = System.Drawing.SystemColors.Control
		Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label13.Enabled = True
		Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label13.Location = New System.Drawing.Point(360, 16)
		Me.Label13.Name = "Label13"
		Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label13.Size = New System.Drawing.Size(49, 17)
		Me.Label13.TabIndex = 21
		Me.Label13.Text = "Total   :"
		Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label13.UseMnemonic = True
		Me.Label13.Visible = True
		' 
		' Label12
		' 
		Me.Label12.AutoSize = False
		Me.Label12.BackColor = System.Drawing.SystemColors.Control
		Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label12.Enabled = True
		Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label12.Location = New System.Drawing.Point(360, 32)
		Me.Label12.Name = "Label12"
		Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label12.Size = New System.Drawing.Size(49, 17)
		Me.Label12.TabIndex = 20
		Me.Label12.Text = "Done  :"
		Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label12.UseMnemonic = True
		Me.Label12.Visible = True
		' 
		' Label11
		' 
		Me.Label11.AutoSize = False
		Me.Label11.BackColor = System.Drawing.SystemColors.Control
		Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label11.Enabled = True
		Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label11.Location = New System.Drawing.Point(360, 48)
		Me.Label11.Name = "Label11"
		Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label11.Size = New System.Drawing.Size(49, 17)
		Me.Label11.TabIndex = 19
		Me.Label11.Text = "Failed  :"
		Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label11.UseMnemonic = True
		Me.Label11.Visible = True
		' 
		' Frame2
		' 
		Me.Frame2.BackColor = System.Drawing.SystemColors.Control
		Me.Frame2.Controls.Add(Me.proDraw)
		Me.Frame2.Controls.Add(Me.lblDrawFailed)
		Me.Frame2.Controls.Add(Me.lblDrawDone)
		Me.Frame2.Controls.Add(Me.lblDrawTotal)
		Me.Frame2.Controls.Add(Me.Label10)
		Me.Frame2.Controls.Add(Me.Label9)
		Me.Frame2.Controls.Add(Me.Label8)
		Me.Frame2.Enabled = True
		Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame2.Location = New System.Drawing.Point(8, 96)
		Me.Frame2.Name = "Frame2"
		Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame2.Size = New System.Drawing.Size(465, 73)
		Me.Frame2.TabIndex = 6
		Me.Frame2.Text = "Drawers"
		Me.Frame2.Visible = True
		' 
		' proDraw
		' 
		Me.proDraw.Location = New System.Drawing.Point(16, 32)
		Me.proDraw.Name = "proDraw"
		Me.proDraw.Size = New System.Drawing.Size(321, 17)
		Me.proDraw.TabIndex = 7
		' 
		' lblDrawFailed
		' 
		Me.lblDrawFailed.AutoSize = False
		Me.lblDrawFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblDrawFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDrawFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDrawFailed.Enabled = True
		Me.lblDrawFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDrawFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDrawFailed.Location = New System.Drawing.Point(400, 48)
		Me.lblDrawFailed.Name = "lblDrawFailed"
		Me.lblDrawFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDrawFailed.Size = New System.Drawing.Size(57, 17)
		Me.lblDrawFailed.TabIndex = 30
		Me.lblDrawFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDrawFailed.UseMnemonic = True
		Me.lblDrawFailed.Visible = True
		' 
		' lblDrawDone
		' 
		Me.lblDrawDone.AutoSize = False
		Me.lblDrawDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblDrawDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDrawDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDrawDone.Enabled = True
		Me.lblDrawDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDrawDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDrawDone.Location = New System.Drawing.Point(400, 32)
		Me.lblDrawDone.Name = "lblDrawDone"
		Me.lblDrawDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDrawDone.Size = New System.Drawing.Size(57, 17)
		Me.lblDrawDone.TabIndex = 29
		Me.lblDrawDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDrawDone.UseMnemonic = True
		Me.lblDrawDone.Visible = True
		' 
		' lblDrawTotal
		' 
		Me.lblDrawTotal.AutoSize = False
		Me.lblDrawTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblDrawTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDrawTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDrawTotal.Enabled = True
		Me.lblDrawTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDrawTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDrawTotal.Location = New System.Drawing.Point(400, 16)
		Me.lblDrawTotal.Name = "lblDrawTotal"
		Me.lblDrawTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDrawTotal.Size = New System.Drawing.Size(57, 17)
		Me.lblDrawTotal.TabIndex = 28
		Me.lblDrawTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDrawTotal.UseMnemonic = True
		Me.lblDrawTotal.Visible = True
		' 
		' Label10
		' 
		Me.Label10.AutoSize = False
		Me.Label10.BackColor = System.Drawing.SystemColors.Control
		Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label10.Enabled = True
		Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label10.Location = New System.Drawing.Point(360, 48)
		Me.Label10.Name = "Label10"
		Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label10.Size = New System.Drawing.Size(49, 17)
		Me.Label10.TabIndex = 18
		Me.Label10.Text = "Failed  :"
		Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label10.UseMnemonic = True
		Me.Label10.Visible = True
		' 
		' Label9
		' 
		Me.Label9.AutoSize = False
		Me.Label9.BackColor = System.Drawing.SystemColors.Control
		Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label9.Enabled = True
		Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label9.Location = New System.Drawing.Point(360, 32)
		Me.Label9.Name = "Label9"
		Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label9.Size = New System.Drawing.Size(49, 17)
		Me.Label9.TabIndex = 17
		Me.Label9.Text = "Done  :"
		Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label9.UseMnemonic = True
		Me.Label9.Visible = True
		' 
		' Label8
		' 
		Me.Label8.AutoSize = False
		Me.Label8.BackColor = System.Drawing.SystemColors.Control
		Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.Enabled = True
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label8.Location = New System.Drawing.Point(360, 16)
		Me.Label8.Name = "Label8"
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.Size = New System.Drawing.Size(49, 17)
		Me.Label8.TabIndex = 16
		Me.Label8.Text = "Total   :"
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label8.UseMnemonic = True
		Me.Label8.Visible = True
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.proCab)
		Me.Frame1.Controls.Add(Me.lblCabFailed)
		Me.Frame1.Controls.Add(Me.lblCabDone)
		Me.Frame1.Controls.Add(Me.lblCabTotal)
		Me.Frame1.Controls.Add(Me.Label7)
		Me.Frame1.Controls.Add(Me.Label6)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Controls.Add(Me.lblCabName)
		Me.Frame1.Controls.Add(Me.Label2)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(465, 73)
		Me.Frame1.TabIndex = 2
		Me.Frame1.Text = "Cabinets"
		Me.Frame1.Visible = True
		' 
		' proCab
		' 
		Me.proCab.Location = New System.Drawing.Point(16, 48)
		Me.proCab.Name = "proCab"
		Me.proCab.Size = New System.Drawing.Size(321, 17)
		Me.proCab.TabIndex = 4
		' 
		' lblCabFailed
		' 
		Me.lblCabFailed.AutoSize = False
		Me.lblCabFailed.BackColor = System.Drawing.SystemColors.Control
		Me.lblCabFailed.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCabFailed.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCabFailed.Enabled = True
		Me.lblCabFailed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCabFailed.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCabFailed.Location = New System.Drawing.Point(400, 48)
		Me.lblCabFailed.Name = "lblCabFailed"
		Me.lblCabFailed.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCabFailed.Size = New System.Drawing.Size(57, 17)
		Me.lblCabFailed.TabIndex = 27
		Me.lblCabFailed.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCabFailed.UseMnemonic = True
		Me.lblCabFailed.Visible = True
		' 
		' lblCabDone
		' 
		Me.lblCabDone.AutoSize = False
		Me.lblCabDone.BackColor = System.Drawing.SystemColors.Control
		Me.lblCabDone.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCabDone.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCabDone.Enabled = True
		Me.lblCabDone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCabDone.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCabDone.Location = New System.Drawing.Point(400, 32)
		Me.lblCabDone.Name = "lblCabDone"
		Me.lblCabDone.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCabDone.Size = New System.Drawing.Size(57, 17)
		Me.lblCabDone.TabIndex = 26
		Me.lblCabDone.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCabDone.UseMnemonic = True
		Me.lblCabDone.Visible = True
		' 
		' lblCabTotal
		' 
		Me.lblCabTotal.AutoSize = False
		Me.lblCabTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblCabTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCabTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCabTotal.Enabled = True
		Me.lblCabTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCabTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCabTotal.Location = New System.Drawing.Point(400, 16)
		Me.lblCabTotal.Name = "lblCabTotal"
		Me.lblCabTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCabTotal.Size = New System.Drawing.Size(57, 17)
		Me.lblCabTotal.TabIndex = 25
		Me.lblCabTotal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCabTotal.UseMnemonic = True
		Me.lblCabTotal.Visible = True
		' 
		' Label7
		' 
		Me.Label7.AutoSize = False
		Me.Label7.BackColor = System.Drawing.SystemColors.Control
		Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.Enabled = True
		Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label7.Location = New System.Drawing.Point(360, 32)
		Me.Label7.Name = "Label7"
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.Size = New System.Drawing.Size(49, 17)
		Me.Label7.TabIndex = 15
		Me.Label7.Text = "Done  :"
		Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label7.UseMnemonic = True
		Me.Label7.Visible = True
		' 
		' Label6
		' 
		Me.Label6.AutoSize = False
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.Enabled = True
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Location = New System.Drawing.Point(360, 48)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(49, 17)
		Me.Label6.TabIndex = 14
		Me.Label6.Text = "Failed  :"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(360, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(49, 17)
		Me.Label1.TabIndex = 13
		Me.Label1.Text = "Total   :"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' lblCabName
		' 
		Me.lblCabName.AutoSize = False
		Me.lblCabName.BackColor = System.Drawing.SystemColors.Control
		Me.lblCabName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCabName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCabName.Enabled = True
		Me.lblCabName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCabName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCabName.Location = New System.Drawing.Point(136, 24)
		Me.lblCabName.Name = "lblCabName"
		Me.lblCabName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCabName.Size = New System.Drawing.Size(201, 17)
		Me.lblCabName.TabIndex = 5
		Me.lblCabName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCabName.UseMnemonic = True
		Me.lblCabName.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(16, 24)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(97, 17)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Current Cabinet :"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' cmdAbort
		' 
		Me.cmdAbort.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAbort.CausesValidation = True
		Me.cmdAbort.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAbort.Enabled = True
		Me.cmdAbort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAbort.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAbort.Location = New System.Drawing.Point(280, 368)
		Me.cmdAbort.Name = "cmdAbort"
		Me.cmdAbort.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAbort.Size = New System.Drawing.Size(81, 33)
		Me.cmdAbort.TabIndex = 1
		Me.cmdAbort.TabStop = True
		Me.cmdAbort.Text = "Cancel"
		Me.cmdAbort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAbort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdStart
		' 
		Me.cmdStart.BackColor = System.Drawing.SystemColors.Control
		Me.cmdStart.CausesValidation = True
		Me.cmdStart.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdStart.Enabled = True
		Me.cmdStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdStart.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdStart.Location = New System.Drawing.Point(376, 368)
		Me.cmdStart.Name = "cmdStart"
		Me.cmdStart.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdStart.Size = New System.Drawing.Size(81, 33)
		Me.cmdStart.TabIndex = 0
		Me.cmdStart.TabStop = True
		Me.cmdStart.Text = "Start"
		Me.cmdStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(481, 415)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdViewReport)
		Me.Controls.Add(Me.Frame5)
		Me.Controls.Add(Me.Frame3)
		Me.Controls.Add(Me.Frame2)
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdAbort)
		Me.Controls.Add(Me.cmdStart)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "DocuMaster Enterprise - Data Transfer"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame5.ResumeLayout(False)
		Me.Frame3.ResumeLayout(False)
		Me.Frame2.ResumeLayout(False)
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class