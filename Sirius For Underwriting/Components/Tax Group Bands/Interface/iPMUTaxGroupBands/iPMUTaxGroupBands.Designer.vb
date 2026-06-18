<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptRule()
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents lblTaxBand As System.Windows.Forms.Label
	Public WithEvents lblSequence As System.Windows.Forms.Label
	Public WithEvents lblAllocationSequence As System.Windows.Forms.Label
	Public WithEvents lblRule As System.Windows.Forms.Label
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents cboTaxband As PMLookupControl.cboPMLookup
	Public WithEvents txtSequence As System.Windows.Forms.TextBox
	Public WithEvents cboSequence As System.Windows.Forms.ComboBox
	Public WithEvents txtAllocationSequence As System.Windows.Forms.TextBox
	Private WithEvents _optRule_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optRule_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optRule_2 As System.Windows.Forms.RadioButton
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
	Public WithEvents lblTaxGroup As System.Windows.Forms.Label
	Public WithEvents pnlTaxGroup As System.Windows.Forms.Label
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _lvwTaxBands_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBands_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBands_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTaxBands_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTaxBands As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public optRule(2) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblTaxBand = New System.Windows.Forms.Label
        Me.lblSequence = New System.Windows.Forms.Label
        Me.lblAllocationSequence = New System.Windows.Forms.Label
        Me.lblRule = New System.Windows.Forms.Label
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.cboTaxband = New PMLookupControl.cboPMLookup
        Me.txtSequence = New System.Windows.Forms.TextBox
        Me.cboSequence = New System.Windows.Forms.ComboBox
        Me.txtAllocationSequence = New System.Windows.Forms.TextBox
        Me._optRule_0 = New System.Windows.Forms.RadioButton
        Me._optRule_1 = New System.Windows.Forms.RadioButton
        Me._optRule_2 = New System.Windows.Forms.RadioButton
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblTaxGroup = New System.Windows.Forms.Label
        Me.pnlTaxGroup = New System.Windows.Forms.Label
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwTaxBands = New System.Windows.Forms.ListView
        Me._lvwTaxBands_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTaxBands_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTaxBands_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwTaxBands_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 288)
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
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 288)
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
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 288)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(12, 8)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 277)
        Me.tabDetailTab.TabIndex = 10
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblTaxBand)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblSequence)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblAllocationSequence)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRule)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboTaxband)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtSequence)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboSequence)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtAllocationSequence)
        Me._tabDetailTab_TabPage0.Controls.Add(Me._optRule_0)
        Me._tabDetailTab_TabPage0.Controls.Add(Me._optRule_1)
        Me._tabDetailTab_TabPage0.Controls.Add(Me._optRule_2)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 251)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "2 - Tax Band"
        Me._tabDetailTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblTaxBand
        '
        Me.lblTaxBand.AutoSize = True
        Me.lblTaxBand.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxBand.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxBand.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxBand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxBand.Location = New System.Drawing.Point(80, 28)
        Me.lblTaxBand.Name = "lblTaxBand"
        Me.lblTaxBand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxBand.Size = New System.Drawing.Size(56, 13)
        Me.lblTaxBand.TabIndex = 12
        Me.lblTaxBand.Text = "Tax Band:"
        Me.lblTaxBand.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblSequence
        '
        Me.lblSequence.AutoSize = True
        Me.lblSequence.BackColor = System.Drawing.SystemColors.Control
        Me.lblSequence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSequence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSequence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSequence.Location = New System.Drawing.Point(78, 56)
        Me.lblSequence.Name = "lblSequence"
        Me.lblSequence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSequence.Size = New System.Drawing.Size(59, 13)
        Me.lblSequence.TabIndex = 15
        Me.lblSequence.Text = "Sequence:"
        Me.lblSequence.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAllocationSequence
        '
        Me.lblAllocationSequence.AutoSize = True
        Me.lblAllocationSequence.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocationSequence.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocationSequence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocationSequence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocationSequence.Location = New System.Drawing.Point(19, 110)
        Me.lblAllocationSequence.Name = "lblAllocationSequence"
        Me.lblAllocationSequence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocationSequence.Size = New System.Drawing.Size(108, 13)
        Me.lblAllocationSequence.TabIndex = 18
        Me.lblAllocationSequence.Text = "Allocation Sequence:"
        Me.lblAllocationSequence.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblRule
        '
        Me.lblRule.AutoSize = True
        Me.lblRule.BackColor = System.Drawing.SystemColors.Control
        Me.lblRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRule.Location = New System.Drawing.Point(50, 84)
        Me.lblRule.Name = "lblRule"
        Me.lblRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRule.Size = New System.Drawing.Size(81, 13)
        Me.lblRule.TabIndex = 19
        Me.lblRule.Text = "Allocation Rule:"
        Me.lblRule.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(440, 222)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 8
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(520, 222)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 9
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'cboTaxband
        '
        Me.cboTaxband.DefaultItemId = 0
        Me.cboTaxband.FirstItem = ""
        Me.cboTaxband.ItemId = 0
        Me.cboTaxband.ListIndex = -1
        Me.cboTaxband.Location = New System.Drawing.Point(168, 24)
        Me.cboTaxband.Name = "cboTaxband"
        Me.cboTaxband.PMLookupProductFamily = 9
        Me.cboTaxband.SingleItemId = 0
        Me.cboTaxband.Size = New System.Drawing.Size(364, 21)
        Me.cboTaxband.Sorted = True
        Me.cboTaxband.TabIndex = 6
        Me.cboTaxband.TableName = "tax_band"
        Me.cboTaxband.ToolTipText = ""
        Me.cboTaxband.WhereClause = ""
        '
        'txtSequence
        '
        Me.txtSequence.AcceptsReturn = True
        Me.txtSequence.BackColor = System.Drawing.SystemColors.Window
        Me.txtSequence.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSequence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSequence.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSequence.Location = New System.Drawing.Point(168, 52)
        Me.txtSequence.MaxLength = 0
        Me.txtSequence.Name = "txtSequence"
        Me.txtSequence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSequence.Size = New System.Drawing.Size(140, 20)
        Me.txtSequence.TabIndex = 16
        Me.txtSequence.Visible = False
        '
        'cboSequence
        '
        Me.cboSequence.BackColor = System.Drawing.SystemColors.Window
        Me.cboSequence.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSequence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSequence.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSequence.Location = New System.Drawing.Point(168, 52)
        Me.cboSequence.Name = "cboSequence"
        Me.cboSequence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSequence.Size = New System.Drawing.Size(140, 21)
        Me.cboSequence.TabIndex = 17
        '
        'txtAllocationSequence
        '
        Me.txtAllocationSequence.AcceptsReturn = True
        Me.txtAllocationSequence.BackColor = System.Drawing.SystemColors.Window
        Me.txtAllocationSequence.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAllocationSequence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllocationSequence.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAllocationSequence.Location = New System.Drawing.Point(168, 106)
        Me.txtAllocationSequence.MaxLength = 0
        Me.txtAllocationSequence.Name = "txtAllocationSequence"
        Me.txtAllocationSequence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllocationSequence.Size = New System.Drawing.Size(137, 20)
        Me.txtAllocationSequence.TabIndex = 20
        '
        '_optRule_0
        '
        Me._optRule_0.BackColor = System.Drawing.SystemColors.Control
        Me._optRule_0.Checked = True
        Me._optRule_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRule_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRule_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRule_0.Location = New System.Drawing.Point(166, 84)
        Me._optRule_0.Name = "_optRule_0"
        Me._optRule_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRule_0.Size = New System.Drawing.Size(115, 16)
        Me._optRule_0.TabIndex = 21
        Me._optRule_0.TabStop = True
        Me._optRule_0.Text = "Before Premium"
        Me._optRule_0.UseVisualStyleBackColor = False
        '
        '_optRule_1
        '
        Me._optRule_1.BackColor = System.Drawing.SystemColors.Control
        Me._optRule_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRule_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRule_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRule_1.Location = New System.Drawing.Point(288, 84)
        Me._optRule_1.Name = "_optRule_1"
        Me._optRule_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRule_1.Size = New System.Drawing.Size(115, 20)
        Me._optRule_1.TabIndex = 22
        Me._optRule_1.TabStop = True
        Me._optRule_1.Text = "With Premium"
        Me._optRule_1.UseVisualStyleBackColor = False
        '
        '_optRule_2
        '
        Me._optRule_2.BackColor = System.Drawing.SystemColors.Control
        Me._optRule_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRule_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRule_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRule_2.Location = New System.Drawing.Point(412, 84)
        Me._optRule_2.Name = "_optRule_2"
        Me._optRule_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRule_2.Size = New System.Drawing.Size(115, 20)
        Me._optRule_2.TabIndex = 23
        Me._optRule_2.TabStop = True
        Me._optRule_2.Text = "After Premium"
        Me._optRule_2.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 277)
        Me.tabMainTab.TabIndex = 11
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaxGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlTaxGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwTaxBands)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 251)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Tax Group Tax Bands"
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.AutoSize = True
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(30, 16)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(58, 13)
        Me.lblTaxGroup.TabIndex = 13
        Me.lblTaxGroup.Text = "Tax group:"
        Me.lblTaxGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlTaxGroup
        '
        Me.pnlTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.pnlTaxGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlTaxGroup.Location = New System.Drawing.Point(106, 14)
        Me.pnlTaxGroup.Name = "pnlTaxGroup"
        Me.pnlTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlTaxGroup.Size = New System.Drawing.Size(479, 19)
        Me.pnlTaxGroup.TabIndex = 14
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(520, 222)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 3
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(360, 222)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 1
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwTaxBands
        '
        Me.lvwTaxBands.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTaxBands.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwTaxBands, "")
        Me.lvwTaxBands.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTaxBands_ColumnHeader_1, Me._lvwTaxBands_ColumnHeader_2, Me._lvwTaxBands_ColumnHeader_3, Me._lvwTaxBands_ColumnHeader_4})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTaxBands, True)
        Me.lvwTaxBands.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTaxBands.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTaxBands.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwTaxBands, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwTaxBands, "")
        Me.lvwTaxBands.Location = New System.Drawing.Point(6, 40)
        Me.lvwTaxBands.Name = "lvwTaxBands"
        Me.lvwTaxBands.Size = New System.Drawing.Size(585, 173)
        Me.listViewHelper1.SetSmallIcons(Me.lvwTaxBands, "")
        Me.listViewHelper1.SetSorted(Me.lvwTaxBands, False)
        Me.listViewHelper1.SetSortKey(Me.lvwTaxBands, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwTaxBands, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwTaxBands.TabIndex = 0
        Me.lvwTaxBands.UseCompatibleStateImageBehavior = False
        Me.lvwTaxBands.View = System.Windows.Forms.View.Details
        '
        '_lvwTaxBands_ColumnHeader_1
        '
        Me._lvwTaxBands_ColumnHeader_1.Text = "Tax Band"
        Me._lvwTaxBands_ColumnHeader_1.Width = 201
        '
        '_lvwTaxBands_ColumnHeader_2
        '
        Me._lvwTaxBands_ColumnHeader_2.Text = "Sequence"
        Me._lvwTaxBands_ColumnHeader_2.Width = 97
        '
        '_lvwTaxBands_ColumnHeader_3
        '
        Me._lvwTaxBands_ColumnHeader_3.Text = "Allocation Rule"
        Me._lvwTaxBands_ColumnHeader_3.Width = 131
        '
        '_lvwTaxBands_ColumnHeader_4
        '
        Me._lvwTaxBands_ColumnHeader_4.Text = "Allocation Sequence"
        Me._lvwTaxBands_ColumnHeader_4.Width = 141
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(440, 222)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(615, 316)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Peril Type Usage"
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptRule()
		Me.optRule(2) = _optRule_2
		Me.optRule(1) = _optRule_1
		Me.optRule(0) = _optRule_0
	End Sub
#End Region 
End Class