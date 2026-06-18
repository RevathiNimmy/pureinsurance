<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializepnlPeriod()
        InitializelblLedger()
        InitializelblPeriod()
		InitializecmdAdvance()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblCurrentPeriodIn As System.Windows.Forms.Label
	Private WithEvents _lblLedger_1 As System.Windows.Forms.Label
	Private WithEvents _lblLedger_2 As System.Windows.Forms.Label
	Private WithEvents _lblLedger_3 As System.Windows.Forms.Label
	Public WithEvents lblNote As System.Windows.Forms.Label
	Public WithEvents lblSubBranch As System.Windows.Forms.Label
	Private WithEvents _pnlPeriod_2 As System.Windows.Forms.Panel
	Private WithEvents _pnlPeriod_3 As System.Windows.Forms.Panel
	Public WithEvents pnlCurrentYear As System.Windows.Forms.Panel
	Private WithEvents _pnlPeriod_1 As System.Windows.Forms.Panel
	Public WithEvents cmdPeriodEnd As System.Windows.Forms.Button
	Private WithEvents _cmdAdvance_3 As System.Windows.Forms.Button
	Private WithEvents _cmdAdvance_1 As System.Windows.Forms.Button
	Private WithEvents _cmdAdvance_2 As System.Windows.Forms.Button
	Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public cmdAdvance(3) As System.Windows.Forms.Button
	Public lblLedger(3) As System.Windows.Forms.Label
	Public pnlPeriod(3) As System.Windows.Forms.Panel
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblCurrentPeriodIn = New System.Windows.Forms.Label
        Me._lblLedger_1 = New System.Windows.Forms.Label
        Me._lblLedger_2 = New System.Windows.Forms.Label
        Me._lblLedger_3 = New System.Windows.Forms.Label
        Me.lblNote = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me._pnlPeriod_2 = New System.Windows.Forms.Panel
        Me._lblPeriod_2 = New System.Windows.Forms.Label
        Me._pnlPeriod_3 = New System.Windows.Forms.Panel
        Me._lblPeriod_3 = New System.Windows.Forms.Label
        Me.pnlCurrentYear = New System.Windows.Forms.Panel
        Me.lblCurrentYear = New System.Windows.Forms.Label
        Me._pnlPeriod_1 = New System.Windows.Forms.Panel
        Me._lblPeriod_1 = New System.Windows.Forms.Label
        Me.cmdPeriodEnd = New System.Windows.Forms.Button
        Me._cmdAdvance_3 = New System.Windows.Forms.Button
        Me._cmdAdvance_1 = New System.Windows.Forms.Button
        Me._cmdAdvance_2 = New System.Windows.Forms.Button
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmdSchedule = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._pnlPeriod_2.SuspendLayout()
        Me._pnlPeriod_3.SuspendLayout()
        Me.pnlCurrentYear.SuspendLayout()
        Me._pnlPeriod_1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(360, 288)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 6
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(440, 288)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 7
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(280, 288)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(200, 288)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(100, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(509, 279)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdSchedule)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrentPeriodIn)
        Me._tabMainTab_TabPage0.Controls.Add(Me._lblLedger_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._lblLedger_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me._lblLedger_3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNote)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSubBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me._pnlPeriod_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me._pnlPeriod_3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlCurrentYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me._pnlPeriod_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPeriodEnd)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdAdvance_3)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdAdvance_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdAdvance_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSubBranch)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(501, 253)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(456, 24)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblCurrentPeriodIn
        '
        Me.lblCurrentPeriodIn.AutoSize = True
        Me.lblCurrentPeriodIn.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentPeriodIn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentPeriodIn.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentPeriodIn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentPeriodIn.Location = New System.Drawing.Point(122, 46)
        Me.lblCurrentPeriodIn.Name = "lblCurrentPeriodIn"
        Me.lblCurrentPeriodIn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentPeriodIn.Size = New System.Drawing.Size(88, 13)
        Me.lblCurrentPeriodIn.TabIndex = 9
        Me.lblCurrentPeriodIn.Text = "Current Period in"
        '
        '_lblLedger_1
        '
        Me._lblLedger_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblLedger_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLedger_1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLedger_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLedger_1.Location = New System.Drawing.Point(16, 86)
        Me._lblLedger_1.Name = "_lblLedger_1"
        Me._lblLedger_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLedger_1.Size = New System.Drawing.Size(92, 21)
        Me._lblLedger_1.TabIndex = 10
        Me._lblLedger_1.Text = "Ledger:"
        '
        '_lblLedger_2
        '
        Me._lblLedger_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblLedger_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLedger_2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLedger_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLedger_2.Location = New System.Drawing.Point(16, 126)
        Me._lblLedger_2.Name = "_lblLedger_2"
        Me._lblLedger_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLedger_2.Size = New System.Drawing.Size(95, 21)
        Me._lblLedger_2.TabIndex = 11
        Me._lblLedger_2.Text = "Ledger:"
        '
        '_lblLedger_3
        '
        Me._lblLedger_3.AutoSize = True
        Me._lblLedger_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblLedger_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblLedger_3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblLedger_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblLedger_3.Location = New System.Drawing.Point(16, 166)
        Me._lblLedger_3.Name = "_lblLedger_3"
        Me._lblLedger_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblLedger_3.Size = New System.Drawing.Size(44, 13)
        Me._lblLedger_3.TabIndex = 12
        Me._lblLedger_3.Text = "Ledger:"
        '
        'lblNote
        '
        Me.lblNote.BackColor = System.Drawing.SystemColors.Control
        Me.lblNote.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNote.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNote.Location = New System.Drawing.Point(120, 214)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNote.Size = New System.Drawing.Size(290, 39)
        Me.lblNote.TabIndex = 15
        Me.lblNote.Text = "Once all Ledger periods have been advanced, Period End should be run."
        '
        'lblSubBranch
        '
        Me.lblSubBranch.AutoSize = True
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(122, 22)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(106, 13)
        Me.lblSubBranch.TabIndex = 20
        Me.lblSubBranch.Text = "Current Sub-Branch:"
        '
        '_pnlPeriod_2
        '
        Me._pnlPeriod_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPeriod_2.Controls.Add(Me._lblPeriod_2)
        Me._pnlPeriod_2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPeriod_2.Location = New System.Drawing.Point(120, 126)
        Me._pnlPeriod_2.Name = "_pnlPeriod_2"
        Me._pnlPeriod_2.Size = New System.Drawing.Size(281, 17)
        Me._pnlPeriod_2.TabIndex = 17
        '
        '_lblPeriod_2
        '
        Me._lblPeriod_2.AutoSize = True
        Me._lblPeriod_2.Location = New System.Drawing.Point(1, 0)
        Me._lblPeriod_2.Name = "_lblPeriod_2"
        Me._lblPeriod_2.Size = New System.Drawing.Size(0, 13)
        Me._lblPeriod_2.TabIndex = 22
        '
        '_pnlPeriod_3
        '
        Me._pnlPeriod_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPeriod_3.Controls.Add(Me._lblPeriod_3)
        Me._pnlPeriod_3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPeriod_3.Location = New System.Drawing.Point(120, 166)
        Me._pnlPeriod_3.Name = "_pnlPeriod_3"
        Me._pnlPeriod_3.Size = New System.Drawing.Size(281, 17)
        Me._pnlPeriod_3.TabIndex = 14
        '
        '_lblPeriod_3
        '
        Me._lblPeriod_3.AutoSize = True
        Me._lblPeriod_3.Location = New System.Drawing.Point(1, 0)
        Me._lblPeriod_3.Name = "_lblPeriod_3"
        Me._lblPeriod_3.Size = New System.Drawing.Size(0, 13)
        Me._lblPeriod_3.TabIndex = 21
        '
        'pnlCurrentYear
        '
        Me.pnlCurrentYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlCurrentYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCurrentYear.Controls.Add(Me.lblCurrentYear)
        Me.pnlCurrentYear.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCurrentYear.Location = New System.Drawing.Point(232, 46)
        Me.pnlCurrentYear.Name = "pnlCurrentYear"
        Me.pnlCurrentYear.Size = New System.Drawing.Size(169, 17)
        Me.pnlCurrentYear.TabIndex = 8
        '
        'lblCurrentYear
        '
        Me.lblCurrentYear.AutoSize = True
        Me.lblCurrentYear.Location = New System.Drawing.Point(1, 1)
        Me.lblCurrentYear.Name = "lblCurrentYear"
        Me.lblCurrentYear.Size = New System.Drawing.Size(0, 13)
        Me.lblCurrentYear.TabIndex = 0
        '
        '_pnlPeriod_1
        '
        Me._pnlPeriod_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPeriod_1.Controls.Add(Me._lblPeriod_1)
        Me._pnlPeriod_1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPeriod_1.Location = New System.Drawing.Point(120, 86)
        Me._pnlPeriod_1.Name = "_pnlPeriod_1"
        Me._pnlPeriod_1.Size = New System.Drawing.Size(281, 17)
        Me._pnlPeriod_1.TabIndex = 13
        '
        '_lblPeriod_1
        '
        Me._lblPeriod_1.AutoSize = True
        Me._lblPeriod_1.Location = New System.Drawing.Point(1, 0)
        Me._lblPeriod_1.Name = "_lblPeriod_1"
        Me._lblPeriod_1.Size = New System.Drawing.Size(0, 13)
        Me._lblPeriod_1.TabIndex = 21
        '
        'cmdPeriodEnd
        '
        Me.cmdPeriodEnd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPeriodEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPeriodEnd.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPeriodEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPeriodEnd.Location = New System.Drawing.Point(416, 214)
        Me.cmdPeriodEnd.Name = "cmdPeriodEnd"
        Me.cmdPeriodEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPeriodEnd.Size = New System.Drawing.Size(73, 22)
        Me.cmdPeriodEnd.TabIndex = 16
        Me.cmdPeriodEnd.Text = "Period End"
        Me.cmdPeriodEnd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPeriodEnd.UseVisualStyleBackColor = False
        '
        '_cmdAdvance_3
        '
        Me._cmdAdvance_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAdvance_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAdvance_3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAdvance_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAdvance_3.Location = New System.Drawing.Point(416, 166)
        Me._cmdAdvance_3.Name = "_cmdAdvance_3"
        Me._cmdAdvance_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAdvance_3.Size = New System.Drawing.Size(73, 22)
        Me._cmdAdvance_3.TabIndex = 3
        Me._cmdAdvance_3.Text = "Advance"
        Me._cmdAdvance_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAdvance_3.UseVisualStyleBackColor = False
        '
        '_cmdAdvance_1
        '
        Me._cmdAdvance_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAdvance_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAdvance_1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAdvance_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAdvance_1.Location = New System.Drawing.Point(416, 86)
        Me._cmdAdvance_1.Name = "_cmdAdvance_1"
        Me._cmdAdvance_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAdvance_1.Size = New System.Drawing.Size(73, 22)
        Me._cmdAdvance_1.TabIndex = 1
        Me._cmdAdvance_1.Text = "Advance"
        Me._cmdAdvance_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAdvance_1.UseVisualStyleBackColor = False
        '
        '_cmdAdvance_2
        '
        Me._cmdAdvance_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAdvance_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAdvance_2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAdvance_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAdvance_2.Location = New System.Drawing.Point(416, 126)
        Me._cmdAdvance_2.Name = "_cmdAdvance_2"
        Me._cmdAdvance_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAdvance_2.Size = New System.Drawing.Size(73, 22)
        Me._cmdAdvance_2.TabIndex = 2
        Me._cmdAdvance_2.Text = "Advance"
        Me._cmdAdvance_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAdvance_2.UseVisualStyleBackColor = False
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(232, 18)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(171, 21)
        Me.cboSubBranch.TabIndex = 19
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblStatus.Location = New System.Drawing.Point(8, 288)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(153, 25)
        Me.lblStatus.TabIndex = 18
        '
        'cmdSchedule
        '
        Me.cmdSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchedule.Location = New System.Drawing.Point(19, 215)
        Me.cmdSchedule.Name = "cmdSchedule"
        Me.cmdSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchedule.Size = New System.Drawing.Size(128, 22)
        Me.cmdSchedule.TabIndex = 19
        Me.cmdSchedule.Text = "&Schedule Period End"
        Me.cmdSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchedule.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(521, 316)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.lblStatus)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Period End"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._pnlPeriod_2.ResumeLayout(False)
        Me._pnlPeriod_2.PerformLayout()
        Me._pnlPeriod_3.ResumeLayout(False)
        Me._pnlPeriod_3.PerformLayout()
        Me.pnlCurrentYear.ResumeLayout(False)
        Me.pnlCurrentYear.PerformLayout()
        Me._pnlPeriod_1.ResumeLayout(False)
        Me._pnlPeriod_1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializepnlPeriod()
        Me.pnlPeriod(1) = _pnlPeriod_1
        Me.pnlPeriod(3) = _pnlPeriod_3
        Me.pnlPeriod(2) = _pnlPeriod_2
    End Sub
    Sub InitializelblLedger()
        Me.lblLedger(3) = _lblLedger_3
        Me.lblLedger(2) = _lblLedger_2
        Me.lblLedger(1) = _lblLedger_1
    End Sub
    Sub InitializecmdAdvance()
        Me.cmdAdvance(2) = _cmdAdvance_2
        Me.cmdAdvance(1) = _cmdAdvance_1
        Me.cmdAdvance(3) = _cmdAdvance_3
    End Sub
    Sub InitializelblPeriod()
        Me.lblPeriod(3) = _lblPeriod_3
        Me.lblPeriod(2) = _lblPeriod_2
        Me.lblPeriod(1) = _lblPeriod_1
    End Sub
    Friend WithEvents lblCurrentYear As System.Windows.Forms.Label
    Friend WithEvents _lblPeriod_1 As System.Windows.Forms.Label
    Friend WithEvents _lblPeriod_2 As System.Windows.Forms.Label
    Friend WithEvents _lblPeriod_3 As System.Windows.Forms.Label
    Friend lblPeriod(3) As System.Windows.Forms.Label
    Public WithEvents cmdSchedule As Button
#End Region
End Class