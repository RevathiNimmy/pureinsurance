<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccounts
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
	Public WithEvents optUnallocatedCash As System.Windows.Forms.RadioButton
	Public WithEvents optMisallocations As System.Windows.Forms.RadioButton
	Public WithEvents fraDisplay As System.Windows.Forms.GroupBox
	Public WithEvents cmdCopyToClipboard As System.Windows.Forms.Button
	Public WithEvents cmdWriteoff As System.Windows.Forms.Button
	Public WithEvents cmdAddOther As System.Windows.Forms.Button
	Public WithEvents cmdMergeAllocations As System.Windows.Forms.Button
	Public WithEvents cmdAddMissing As System.Windows.Forms.Button
	Public WithEvents cmdAddRelated As System.Windows.Forms.Button
	Public WithEvents cmdEditAmount As System.Windows.Forms.Button
	Public WithEvents cmdDeleteSingle As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAll As System.Windows.Forms.Button
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblAllocation As System.Windows.Forms.Label
	Public WithEvents lblAccounts As System.Windows.Forms.Label
	Private WithEvents grdTransactions As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents grdAllocations As Artinsoft.Windows.Forms.ExtendedDataGridView
	Private WithEvents grdAccounts As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAccounts))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraDisplay = New System.Windows.Forms.GroupBox
		Me.optUnallocatedCash = New System.Windows.Forms.RadioButton
		Me.optMisallocations = New System.Windows.Forms.RadioButton
		Me.cmdCopyToClipboard = New System.Windows.Forms.Button
		Me.cmdWriteoff = New System.Windows.Forms.Button
		Me.cmdAddOther = New System.Windows.Forms.Button
		Me.cmdMergeAllocations = New System.Windows.Forms.Button
		Me.cmdAddMissing = New System.Windows.Forms.Button
		Me.cmdAddRelated = New System.Windows.Forms.Button
		Me.cmdEditAmount = New System.Windows.Forms.Button
		Me.cmdDeleteSingle = New System.Windows.Forms.Button
		Me.cmdDeleteAll = New System.Windows.Forms.Button
		Me.cmdRefresh = New System.Windows.Forms.Button
		Me.cmdExit = New System.Windows.Forms.Button
		Me.lblCurrency = New System.Windows.Forms.Label
		Me.lblAllocation = New System.Windows.Forms.Label
		Me.lblAccounts = New System.Windows.Forms.Label
		Me.fraDisplay.SuspendLayout()
		Me.SuspendLayout()
		Me.grdTransactions = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
		CType(Me.grdTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grdAllocations = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
		CType(Me.grdAllocations, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grdAccounts = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
		CType(Me.grdAccounts, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraDisplay
		' 
		Me.fraDisplay.BackColor = System.Drawing.SystemColors.Control
		Me.fraDisplay.Controls.Add(Me.optUnallocatedCash)
		Me.fraDisplay.Controls.Add(Me.optMisallocations)
		Me.fraDisplay.Enabled = True
		Me.fraDisplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDisplay.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDisplay.Location = New System.Drawing.Point(712, 144)
		Me.fraDisplay.Name = "fraDisplay"
		Me.fraDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDisplay.Size = New System.Drawing.Size(113, 73)
		Me.fraDisplay.TabIndex = 17
		Me.fraDisplay.Text = "Display"
		Me.fraDisplay.Visible = True
		' 
		' optUnallocatedCash
		' 
		Me.optUnallocatedCash.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optUnallocatedCash.BackColor = System.Drawing.SystemColors.Control
		Me.optUnallocatedCash.CausesValidation = True
		Me.optUnallocatedCash.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optUnallocatedCash.Checked = False
		Me.optUnallocatedCash.Cursor = System.Windows.Forms.Cursors.Default
		Me.optUnallocatedCash.Enabled = True
		Me.optUnallocatedCash.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.optUnallocatedCash.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optUnallocatedCash.Location = New System.Drawing.Point(8, 40)
		Me.optUnallocatedCash.Name = "optUnallocatedCash"
		Me.optUnallocatedCash.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optUnallocatedCash.Size = New System.Drawing.Size(97, 25)
		Me.optUnallocatedCash.TabIndex = 19
		Me.optUnallocatedCash.TabStop = True
		Me.optUnallocatedCash.Text = "Unallocated Cash"
		Me.optUnallocatedCash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optUnallocatedCash.Visible = True
		' 
		' optMisallocations
		' 
		Me.optMisallocations.Appearance = System.Windows.Forms.Appearance.Normal
		Me.optMisallocations.BackColor = System.Drawing.SystemColors.Control
		Me.optMisallocations.CausesValidation = True
		Me.optMisallocations.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optMisallocations.Checked = True
		Me.optMisallocations.Cursor = System.Windows.Forms.Cursors.Default
		Me.optMisallocations.Enabled = True
		Me.optMisallocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.optMisallocations.ForeColor = System.Drawing.SystemColors.ControlText
		Me.optMisallocations.Location = New System.Drawing.Point(8, 16)
		Me.optMisallocations.Name = "optMisallocations"
		Me.optMisallocations.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.optMisallocations.Size = New System.Drawing.Size(97, 17)
		Me.optMisallocations.TabIndex = 18
		Me.optMisallocations.TabStop = True
		Me.optMisallocations.Text = "Mis-Allocations"
		Me.optMisallocations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.optMisallocations.Visible = True
		' 
		' cmdCopyToClipboard
		' 
		Me.cmdCopyToClipboard.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCopyToClipboard.CausesValidation = True
		Me.cmdCopyToClipboard.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCopyToClipboard.Enabled = True
		Me.cmdCopyToClipboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCopyToClipboard.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCopyToClipboard.Location = New System.Drawing.Point(712, 432)
		Me.cmdCopyToClipboard.Name = "cmdCopyToClipboard"
		Me.cmdCopyToClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCopyToClipboard.Size = New System.Drawing.Size(113, 33)
		Me.cmdCopyToClipboard.TabIndex = 16
		Me.cmdCopyToClipboard.TabStop = True
		Me.cmdCopyToClipboard.Text = "Copy To Clipboard"
		Me.cmdCopyToClipboard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCopyToClipboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdWriteoff
		' 
		Me.cmdWriteoff.BackColor = System.Drawing.SystemColors.Control
		Me.cmdWriteoff.CausesValidation = True
		Me.cmdWriteoff.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdWriteoff.Enabled = True
		Me.cmdWriteoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdWriteoff.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdWriteoff.Location = New System.Drawing.Point(712, 104)
		Me.cmdWriteoff.Name = "cmdWriteoff"
		Me.cmdWriteoff.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdWriteoff.Size = New System.Drawing.Size(113, 33)
		Me.cmdWriteoff.TabIndex = 15
		Me.cmdWriteoff.TabStop = True
		Me.cmdWriteoff.Text = "Write Off"
		Me.cmdWriteoff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdWriteoff.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddOther
		' 
		Me.cmdAddOther.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddOther.CausesValidation = True
		Me.cmdAddOther.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddOther.Enabled = True
		Me.cmdAddOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddOther.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddOther.Location = New System.Drawing.Point(712, 392)
		Me.cmdAddOther.Name = "cmdAddOther"
		Me.cmdAddOther.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddOther.Size = New System.Drawing.Size(113, 33)
		Me.cmdAddOther.TabIndex = 14
		Me.cmdAddOther.TabStop = True
		Me.cmdAddOther.Text = "Add Other Transaction Lines"
		Me.cmdAddOther.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddOther.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdMergeAllocations
		' 
		Me.cmdMergeAllocations.BackColor = System.Drawing.SystemColors.Control
		Me.cmdMergeAllocations.CausesValidation = True
		Me.cmdMergeAllocations.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdMergeAllocations.Enabled = True
		Me.cmdMergeAllocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdMergeAllocations.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdMergeAllocations.Location = New System.Drawing.Point(712, 24)
		Me.cmdMergeAllocations.Name = "cmdMergeAllocations"
		Me.cmdMergeAllocations.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdMergeAllocations.Size = New System.Drawing.Size(113, 33)
		Me.cmdMergeAllocations.TabIndex = 13
		Me.cmdMergeAllocations.TabStop = True
		Me.cmdMergeAllocations.Text = "Merge Allocations"
		Me.cmdMergeAllocations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdMergeAllocations.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddMissing
		' 
		Me.cmdAddMissing.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddMissing.CausesValidation = True
		Me.cmdAddMissing.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddMissing.Enabled = True
		Me.cmdAddMissing.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddMissing.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddMissing.Location = New System.Drawing.Point(712, 352)
		Me.cmdAddMissing.Name = "cmdAddMissing"
		Me.cmdAddMissing.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddMissing.Size = New System.Drawing.Size(113, 33)
		Me.cmdAddMissing.TabIndex = 12
		Me.cmdAddMissing.TabStop = True
		Me.cmdAddMissing.Text = "Add Missing Transaction Lines"
		Me.cmdAddMissing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddMissing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddRelated
		' 
		Me.cmdAddRelated.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddRelated.CausesValidation = True
		Me.cmdAddRelated.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddRelated.Enabled = True
		Me.cmdAddRelated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddRelated.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddRelated.Location = New System.Drawing.Point(712, 312)
		Me.cmdAddRelated.Name = "cmdAddRelated"
		Me.cmdAddRelated.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddRelated.Size = New System.Drawing.Size(113, 33)
		Me.cmdAddRelated.TabIndex = 11
		Me.cmdAddRelated.TabStop = True
		Me.cmdAddRelated.Text = "Add Related Transaction Lines"
		Me.cmdAddRelated.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddRelated.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdEditAmount
		' 
		Me.cmdEditAmount.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEditAmount.CausesValidation = True
		Me.cmdEditAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEditAmount.Enabled = True
		Me.cmdEditAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEditAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEditAmount.Location = New System.Drawing.Point(712, 272)
		Me.cmdEditAmount.Name = "cmdEditAmount"
		Me.cmdEditAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEditAmount.Size = New System.Drawing.Size(113, 33)
		Me.cmdEditAmount.TabIndex = 9
		Me.cmdEditAmount.TabStop = True
		Me.cmdEditAmount.Text = "Edit Allocated Amount"
		Me.cmdEditAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEditAmount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDeleteSingle
		' 
		Me.cmdDeleteSingle.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeleteSingle.CausesValidation = True
		Me.cmdDeleteSingle.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeleteSingle.Enabled = True
		Me.cmdDeleteSingle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDeleteSingle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeleteSingle.Location = New System.Drawing.Point(712, 232)
		Me.cmdDeleteSingle.Name = "cmdDeleteSingle"
		Me.cmdDeleteSingle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeleteSingle.Size = New System.Drawing.Size(113, 33)
		Me.cmdDeleteSingle.TabIndex = 8
		Me.cmdDeleteSingle.TabStop = True
		Me.cmdDeleteSingle.Text = "Remove Transaction"
		Me.cmdDeleteSingle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeleteSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDeleteAll
		' 
		Me.cmdDeleteAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeleteAll.CausesValidation = True
		Me.cmdDeleteAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeleteAll.Enabled = True
		Me.cmdDeleteAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDeleteAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeleteAll.Location = New System.Drawing.Point(712, 64)
		Me.cmdDeleteAll.Name = "cmdDeleteAll"
		Me.cmdDeleteAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeleteAll.Size = New System.Drawing.Size(113, 33)
		Me.cmdDeleteAll.TabIndex = 7
		Me.cmdDeleteAll.TabStop = True
		Me.cmdDeleteAll.Text = "Delete Allocation"
		Me.cmdDeleteAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRefresh
		' 
		Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRefresh.CausesValidation = True
		Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRefresh.Enabled = True
		Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRefresh.Location = New System.Drawing.Point(712, 512)
		Me.cmdRefresh.Name = "cmdRefresh"
		Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRefresh.Size = New System.Drawing.Size(113, 33)
		Me.cmdRefresh.TabIndex = 2
		Me.cmdRefresh.TabStop = True
		Me.cmdRefresh.Text = "&Refresh"
		Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(712, 552)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(113, 33)
		Me.cmdExit.TabIndex = 1
		Me.cmdExit.TabStop = True
		Me.cmdExit.Text = "E&xit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lblCurrency
		' 
		Me.lblCurrency.AutoSize = False
		Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrency.Enabled = True
		Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrency.Location = New System.Drawing.Point(8, 216)
		Me.lblCurrency.Name = "lblCurrency"
		Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrency.Size = New System.Drawing.Size(289, 17)
		Me.lblCurrency.TabIndex = 10
		Me.lblCurrency.Text = "All amounts are in base currency."
		Me.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrency.UseMnemonic = True
		Me.lblCurrency.Visible = True
		' 
		' lblAllocation
		' 
		Me.lblAllocation.AutoSize = False
		Me.lblAllocation.BackColor = System.Drawing.SystemColors.Control
		Me.lblAllocation.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAllocation.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAllocation.Enabled = True
		Me.lblAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAllocation.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAllocation.Location = New System.Drawing.Point(440, 8)
		Me.lblAllocation.Name = "lblAllocation"
		Me.lblAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllocation.Size = New System.Drawing.Size(265, 17)
		Me.lblAllocation.TabIndex = 4
		Me.lblAllocation.Text = "Click an allocation to display the transactions involved."
		Me.lblAllocation.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAllocation.UseMnemonic = True
		Me.lblAllocation.Visible = True
		' 
		' lblAccounts
		' 
		Me.lblAccounts.AutoSize = False
		Me.lblAccounts.BackColor = System.Drawing.SystemColors.Control
		Me.lblAccounts.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAccounts.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAccounts.Enabled = True
		Me.lblAccounts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAccounts.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAccounts.Location = New System.Drawing.Point(8, 8)
		Me.lblAccounts.Name = "lblAccounts"
		Me.lblAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAccounts.Size = New System.Drawing.Size(297, 17)
		Me.lblAccounts.TabIndex = 3
		Me.lblAccounts.Text = "Click an account to display the mis-allocations."
		Me.lblAccounts.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAccounts.UseMnemonic = True
		Me.lblAccounts.Visible = True
		' 
		' frmAccounts
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(833, 589)
		Me.grdAccounts.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
		Me.grdAllocations.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
		Me.grdTransactions.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
		Me.ControlBox = True
		Me.Controls.Add(Me.fraDisplay)
		Me.Controls.Add(Me.cmdCopyToClipboard)
		Me.Controls.Add(Me.cmdWriteoff)
		Me.Controls.Add(Me.cmdAddOther)
		Me.Controls.Add(Me.cmdMergeAllocations)
		Me.Controls.Add(Me.cmdAddMissing)
		Me.Controls.Add(Me.cmdAddRelated)
		Me.Controls.Add(Me.cmdEditAmount)
		Me.Controls.Add(Me.cmdDeleteSingle)
		Me.Controls.Add(Me.cmdDeleteAll)
		Me.Controls.Add(Me.cmdRefresh)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.grdAccounts)
		Me.Controls.Add(Me.grdAllocations)
		Me.Controls.Add(Me.grdTransactions)
		Me.Controls.Add(Me.lblCurrency)
		Me.Controls.Add(Me.lblAllocation)
		Me.Controls.Add(Me.lblAccounts)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.grdAccounts.Location = New System.Drawing.Point(8, 24)
		Me.grdAllocations.Location = New System.Drawing.Point(440, 24)
		Me.grdTransactions.Location = New System.Drawing.Point(8, 232)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAccounts"
		Me.grdAccounts.Name = "grdAccounts"
		Me.grdAllocations.Name = "grdAllocations"
		Me.grdTransactions.Name = "grdTransactions"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.grdAccounts.Size = New System.Drawing.Size(425, 185)
		Me.grdAllocations.Size = New System.Drawing.Size(265, 185)
		Me.grdTransactions.Size = New System.Drawing.Size(697, 353)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.grdAccounts.TabIndex = 0
		Me.grdAllocations.TabIndex = 5
		Me.grdTransactions.TabIndex = 6
		Me.Text = "Mis-Allocations Helper"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ToolTip1.SetToolTip(Me.cmdCopyToClipboard, "Copies all records into the clipboard.")
		Me.ToolTip1.SetToolTip(Me.cmdWriteoff, "Creates a write off transaction and allocates it to the current allocation.")
		Me.ToolTip1.SetToolTip(Me.cmdAddOther, "Allows you to add transaction lines to the allocation.")
		Me.ToolTip1.SetToolTip(Me.cmdMergeAllocations, "Merges all of the allocations selected.")
		Me.ToolTip1.SetToolTip(Me.cmdAddMissing, "Allows you to add transaction lines to the allocation that were meant to be in the allocation but were missed off (e.g. 500 issue).")
		Me.ToolTip1.SetToolTip(Me.cmdAddRelated, "Allows you to add transaction lines to the allocation that are from the same transactions as the existing transaction lines.")
		Me.ToolTip1.SetToolTip(Me.cmdEditAmount, "Edits the amount allocated for the transaction selected.")
		Me.ToolTip1.SetToolTip(Me.cmdDeleteSingle, "Removes the selected transaction from the allocation.")
		Me.ToolTip1.SetToolTip(Me.cmdDeleteAll, "Deletes the allocation selected.")
		CType(Me.grdTransactions, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdAllocations, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grdAccounts, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraDisplay.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class