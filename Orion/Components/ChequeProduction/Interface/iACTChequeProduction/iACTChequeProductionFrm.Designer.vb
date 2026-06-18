<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializecmdPrint()
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
	Public WithEvents cmdUnselect As System.Windows.Forms.Button
	Private WithEvents _cmdPrint_1 As System.Windows.Forms.RadioButton
	Private WithEvents _cmdPrint_0 As System.Windows.Forms.RadioButton
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdAction As System.Windows.Forms.Button
	Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblAccountCode As System.Windows.Forms.Label
	Public WithEvents lblDateTo As System.Windows.Forms.Label
	Public WithEvents lblSource As System.Windows.Forms.Label
	Public WithEvents txtDateTo As System.Windows.Forms.TextBox
	Public WithEvents uctBankAccount As UserControls.BankAccount
	Public WithEvents cboSource As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public cmdPrint(1) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer


	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdUnselect = New System.Windows.Forms.Button
        Me._cmdPrint_1 = New System.Windows.Forms.RadioButton
        Me._cmdPrint_0 = New System.Windows.Forms.RadioButton
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdAction = New System.Windows.Forms.Button
        Me.uctPMResizer = New PMResizerControl.uctPMResizer
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblAccountCode = New System.Windows.Forms.Label
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.lblSource = New System.Windows.Forms.Label
        Me.txtDateTo = New System.Windows.Forms.TextBox
        Me.uctBankAccount = New UserControls.BankAccount
        Me.cboSource = New System.Windows.Forms.ComboBox
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdUnselect
        '
        Me.cmdUnselect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnselect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnselect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnselect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnselect.Location = New System.Drawing.Point(90, 478)
        Me.cmdUnselect.Name = "cmdUnselect"
        Me.cmdUnselect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnselect.Size = New System.Drawing.Size(83, 22)
        Me.cmdUnselect.TabIndex = 10
        Me.cmdUnselect.Text = "&Unselect All"
        Me.cmdUnselect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUnselect.UseVisualStyleBackColor = False
        '
        '_cmdPrint_1
        '
        Me._cmdPrint_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrint_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrint_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrint_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrint_1.Location = New System.Drawing.Point(413, 480)
        Me._cmdPrint_1.Name = "_cmdPrint_1"
        Me._cmdPrint_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrint_1.Size = New System.Drawing.Size(67, 19)
        Me._cmdPrint_1.TabIndex = 14
        Me._cmdPrint_1.TabStop = True
        Me._cmdPrint_1.Text = "Spool"
        Me._cmdPrint_1.UseVisualStyleBackColor = False
        '
        '_cmdPrint_0
        '
        Me._cmdPrint_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrint_0.Checked = True
        Me._cmdPrint_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrint_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrint_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrint_0.Location = New System.Drawing.Point(343, 480)
        Me._cmdPrint_0.Name = "_cmdPrint_0"
        Me._cmdPrint_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrint_0.Size = New System.Drawing.Size(67, 19)
        Me._cmdPrint_0.TabIndex = 13
        Me._cmdPrint_0.TabStop = True
        Me._cmdPrint_0.Text = "Print"
        Me._cmdPrint_0.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(259, 478)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 12
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdAction
        '
        Me.cmdAction.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAction.Location = New System.Drawing.Point(179, 478)
        Me.cmdAction.Name = "cmdAction"
        Me.cmdAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAction.Size = New System.Drawing.Size(73, 22)
        Me.cmdAction.TabIndex = 11
        Me.cmdAction.Text = "&Print"
        Me.cmdAction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAction.UseVisualStyleBackColor = False
        '
        'uctPMResizer
        '
        Me.uctPMResizer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer.Location = New System.Drawing.Point(674, 0)
        Me.uctPMResizer.Name = "uctPMResizer"
        Me.uctPMResizer.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer.TabIndex = 15
        Me.uctPMResizer.Visible = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Location = New System.Drawing.Point(8, 478)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(77, 22)
        Me.cmdSelect.TabIndex = 9
        Me.cmdSelect.Text = "&Select All"
        Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(674, 63)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(85, 22)
        Me.cmdNewSearch.TabIndex = 7
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(674, 33)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(85, 22)
        Me.cmdFindNow.TabIndex = 6
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(674, 478)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 17
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(594, 478)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 16
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(514, 478)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(215, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(15, 3)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(653, 123)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccountCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSource)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctBankAccount)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSource)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(645, 97)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = " &1 - Details"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(24, 42)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(105, 17)
        Me.lblAccountCode.TabIndex = 3
        Me.lblAccountCode.Text = "&Bank Account:"
        '
        'lblDateTo
        '
        Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateTo.Location = New System.Drawing.Point(24, 72)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateTo.Size = New System.Drawing.Size(89, 17)
        Me.lblDateTo.TabIndex = 5
        Me.lblDateTo.Text = "Date To:"
        '
        'lblSource
        '
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(24, 12)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(57, 13)
        Me.lblSource.TabIndex = 19
        Me.lblSource.Text = "Branch:"
        '
        'txtDateTo
        '
        Me.txtDateTo.AcceptsReturn = True
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.Location = New System.Drawing.Point(144, 68)
        Me.txtDateTo.MaxLength = 0
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.Size = New System.Drawing.Size(161, 21)
        Me.txtDateTo.TabIndex = 4
        '
        'uctBankAccount
        '
        Me.uctBankAccount.DefaultId = "0"
        Me.uctBankAccount.FirstItem = ""
        Me.uctBankAccount.Id = 0
        Me.uctBankAccount.ListIndex = -1
        Me.uctBankAccount.Location = New System.Drawing.Point(144, 38)
        Me.uctBankAccount.Name = "uctBankAccount"
        Me.uctBankAccount.Size = New System.Drawing.Size(161, 21)
        Me.uctBankAccount.TabIndex = 2
        Me.uctBankAccount.ToolTipText = ""
        Me.uctBankAccount.WhatsThisHelpID = 0
        '
        'cboSource
        '
        Me.cboSource.BackColor = System.Drawing.SystemColors.Window
        Me.cboSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSource.Location = New System.Drawing.Point(144, 8)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSource.Size = New System.Drawing.Size(161, 21)
        Me.cboSource.TabIndex = 1
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 505)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(771, 22)
        Me.stbStatus.TabIndex = 18
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(736, 22)
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 132)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(737, 341)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetSorted(Me.lvwSearchDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSearchDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSearchDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSearchDetails.TabIndex = 8
        Me.lvwSearchDetails.TabStop = False
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Text = "1"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 92
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Text = "2"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Text = "3"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Text = "4"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Text = "5"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Text = "6"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Text = "7"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Text = "8"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(771, 527)
        Me.Controls.Add(Me.cmdUnselect)
        Me.Controls.Add(Me._cmdPrint_1)
        Me.Controls.Add(Me._cmdPrint_0)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdAction)
        Me.Controls.Add(Me.uctPMResizer)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchDetails)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(191, 280)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cheque Production"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializecmdPrint()
		Me.cmdPrint(1) = _cmdPrint_1
		Me.cmdPrint(0) = _cmdPrint_0
	End Sub
#End Region 
End Class