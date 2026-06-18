<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClauseSelection
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwSelectClause_InitializeColumnKeys()
		lvwBranches_InitializeColumnKeys()
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
	Public WithEvents chkDefault As System.Windows.Forms.CheckBox
	Private WithEvents _lvwBranches_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwBranches As System.Windows.Forms.ListView
	Public WithEvents lblDefaultclause As System.Windows.Forms.Label
	Public WithEvents framBranch As System.Windows.Forms.GroupBox
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents _lvwSelectClause_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSelectClause_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSelectClause As System.Windows.Forms.ListView
	Public WithEvents framClause As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClauseSelection))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.framBranch = New System.Windows.Forms.GroupBox
		Me.chkDefault = New System.Windows.Forms.CheckBox
		Me.lvwBranches = New System.Windows.Forms.ListView
		Me._lvwBranches_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.lblDefaultclause = New System.Windows.Forms.Label
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.framClause = New System.Windows.Forms.GroupBox
		Me.lvwSelectClause = New System.Windows.Forms.ListView
		Me._lvwSelectClause_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwSelectClause_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.framBranch.SuspendLayout()
		Me.lvwBranches.SuspendLayout()
		Me.framClause.SuspendLayout()
		Me.lvwSelectClause.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' framBranch
		' 
		Me.framBranch.BackColor = System.Drawing.SystemColors.Control
		Me.framBranch.Controls.Add(Me.chkDefault)
		Me.framBranch.Controls.Add(Me.lvwBranches)
		Me.framBranch.Controls.Add(Me.lblDefaultclause)
		Me.framBranch.Enabled = True
		Me.framBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.framBranch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.framBranch.Location = New System.Drawing.Point(322, 0)
		Me.framBranch.Name = "framBranch"
		Me.framBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.framBranch.Size = New System.Drawing.Size(215, 421)
		Me.framBranch.TabIndex = 5
		Me.framBranch.Text = "Branches Access "
		Me.framBranch.Visible = True
		' 
		' chkDefault
		' 
		Me.chkDefault.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkDefault.BackColor = System.Drawing.SystemColors.Control
		Me.chkDefault.CausesValidation = True
		Me.chkDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkDefault.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkDefault.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkDefault.Enabled = True
		Me.chkDefault.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkDefault.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkDefault.Location = New System.Drawing.Point(18, 392)
		Me.chkDefault.Name = "chkDefault"
		Me.chkDefault.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkDefault.Size = New System.Drawing.Size(137, 21)
		Me.chkDefault.TabIndex = 6
		Me.chkDefault.TabStop = True
		Me.chkDefault.Text = "Default Clause(s)"
		Me.chkDefault.Visible = True
		' 
		' lvwBranches
		' 
		Me.lvwBranches.BackColor = System.Drawing.SystemColors.Window
		Me.lvwBranches.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwBranches.CheckBoxes = True
		Me.lvwBranches.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwBranches.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwBranches.FullRowSelect = True
		Me.lvwBranches.HideSelection = True
		Me.lvwBranches.LabelEdit = False
		Me.lvwBranches.LabelWrap = True
		Me.lvwBranches.Location = New System.Drawing.Point(6, 14)
		Me.lvwBranches.MultiSelect = True
		Me.lvwBranches.Name = "lvwBranches"
		Me.lvwBranches.Size = New System.Drawing.Size(203, 361)
		Me.lvwBranches.TabIndex = 7
		Me.lvwBranches.View = System.Windows.Forms.View.Details
		Me.lvwBranches.Columns.Add(Me._lvwBranches_ColumnHeader_1)
		' 
		' _lvwBranches_ColumnHeader_1
		' 
		Me._lvwBranches_ColumnHeader_1.Text = "Description"
		Me._lvwBranches_ColumnHeader_1.Width = 197
		' 
		' lblDefaultclause
		' 
		Me.lblDefaultclause.AutoSize = False
		Me.lblDefaultclause.BackColor = System.Drawing.SystemColors.Control
		Me.lblDefaultclause.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDefaultclause.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDefaultclause.Enabled = True
		Me.lblDefaultclause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDefaultclause.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDefaultclause.Location = New System.Drawing.Point(10, 376)
		Me.lblDefaultclause.Name = "lblDefaultclause"
		Me.lblDefaultclause.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDefaultclause.Size = New System.Drawing.Size(61, 17)
		Me.lblDefaultclause.TabIndex = 8
		Me.lblDefaultclause.Text = "Default"
		Me.lblDefaultclause.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDefaultclause.UseMnemonic = True
		Me.lblDefaultclause.Visible = True
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(468, 428)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(67, 23)
		Me.cmdApply.TabIndex = 3
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "A&pply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOk
		' 
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.Enabled = False
		Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Location = New System.Drawing.Point(324, 428)
		Me.cmdOk.Name = "cmdOk"
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.Size = New System.Drawing.Size(67, 23)
		Me.cmdOk.TabIndex = 1
		Me.cmdOk.TabStop = True
		Me.cmdOk.Text = "&Ok"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(396, 428)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(67, 23)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' framClause
		' 
		Me.framClause.BackColor = System.Drawing.SystemColors.Control
		Me.framClause.Controls.Add(Me.lvwSelectClause)
		Me.framClause.Enabled = True
		Me.framClause.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.framClause.ForeColor = System.Drawing.SystemColors.ControlText
		Me.framClause.Location = New System.Drawing.Point(2, 0)
		Me.framClause.Name = "framClause"
		Me.framClause.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.framClause.Size = New System.Drawing.Size(317, 457)
		Me.framClause.TabIndex = 4
		Me.framClause.Text = "Clauses"
		Me.framClause.Visible = True
		' 
		' lvwSelectClause
		' 
		Me.lvwSelectClause.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSelectClause.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwSelectClause.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSelectClause.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSelectClause.HideSelection = False
		Me.lvwSelectClause.LabelEdit = False
		Me.lvwSelectClause.LabelWrap = False
		Me.lvwSelectClause.Location = New System.Drawing.Point(6, 14)
		Me.lvwSelectClause.Name = "lvwSelectClause"
		Me.lvwSelectClause.Size = New System.Drawing.Size(305, 437)
		Me.lvwSelectClause.TabIndex = 0
		Me.lvwSelectClause.TabStop = False
		Me.lvwSelectClause.View = System.Windows.Forms.View.Details
		Me.lvwSelectClause.Columns.Add(Me._lvwSelectClause_ColumnHeader_1)
		Me.lvwSelectClause.Columns.Add(Me._lvwSelectClause_ColumnHeader_2)
		' 
		' _lvwSelectClause_ColumnHeader_1
		' 
		Me._lvwSelectClause_ColumnHeader_1.Text = "Code"
		Me._lvwSelectClause_ColumnHeader_1.Width = 104
		' 
		' _lvwSelectClause_ColumnHeader_2
		' 
		Me._lvwSelectClause_ColumnHeader_2.Text = "Description"
		Me._lvwSelectClause_ColumnHeader_2.Width = 197
		' 
		' frmClauseSelection
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(542, 461)
		Me.ControlBox = True
		Me.Controls.Add(Me.framBranch)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdOk)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.framClause)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 30)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmClauseSelection"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Clause Selection"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetSorted(Me.lvwBranches, True)
		Me.listViewHelper1.SetSorted(Me.lvwSelectClause, True)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwSelectClause, "lvwSelectClause_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSelectClause, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.framBranch.ResumeLayout(False)
		Me.lvwBranches.ResumeLayout(False)
		Me.framClause.ResumeLayout(False)
		Me.lvwSelectClause.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwSelectClause_InitializeColumnKeys()
		Me._lvwSelectClause_ColumnHeader_1.Name = "Code"
		Me._lvwSelectClause_ColumnHeader_2.Name = "Description"
	End Sub
	Sub lvwBranches_InitializeColumnKeys()
		Me._lvwBranches_ColumnHeader_1.Name = "Description"
	End Sub
#End Region 
End Class