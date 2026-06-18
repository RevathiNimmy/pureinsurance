<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptMandatory()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents lblMandatory As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblCaption As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblDispOrd As System.Windows.Forms.Label
	Public WithEvents lblLookUp As System.Windows.Forms.Label
	Public WithEvents lblReadOnly As System.Windows.Forms.Label
	Public WithEvents lblManualEntry As System.Windows.Forms.Label
	Public WithEvents lblTab As System.Windows.Forms.Label
	Public WithEvents lvwDefnFlds As System.Windows.Forms.ListView
	Public WithEvents txtCaption As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdModify As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cboType As System.Windows.Forms.ComboBox
	Public WithEvents txtDispOrd As System.Windows.Forms.TextBox
	Public WithEvents cboLookUp As System.Windows.Forms.ComboBox
	Private WithEvents _optMandatory_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optMandatory_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optMandatory_2 As System.Windows.Forms.RadioButton
	Public WithEvents cboTab As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public optMandatory(2) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblMandatory = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblCaption = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblDispOrd = New System.Windows.Forms.Label
        Me.lblLookUp = New System.Windows.Forms.Label
        Me.lblReadOnly = New System.Windows.Forms.Label
        Me.lblManualEntry = New System.Windows.Forms.Label
        Me.lblTab = New System.Windows.Forms.Label
        Me.lvwDefnFlds = New System.Windows.Forms.ListView
        Me.txtCaption = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdModify = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.txtDispOrd = New System.Windows.Forms.TextBox
        Me.cboLookUp = New System.Windows.Forms.ComboBox
        Me._optMandatory_0 = New System.Windows.Forms.RadioButton
        Me._optMandatory_1 = New System.Windows.Forms.RadioButton
        Me._optMandatory_2 = New System.Windows.Forms.RadioButton
        Me.cboTab = New System.Windows.Forms.ComboBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 471)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(352, 471)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(434, 471)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(54, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(503, 461)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblMandatory)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCaption)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDispOrd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblLookUp)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReadOnly)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblManualEntry)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTab)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwDefnFlds)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCaption)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdModify)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDispOrd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboLookUp)
        Me._tabMainTab_TabPage0.Controls.Add(Me._optMandatory_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me._optMandatory_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._optMandatory_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTab)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(495, 435)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblMandatory
        '
        Me.lblMandatory.AutoSize = True
        Me.lblMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.lblMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMandatory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMandatory.Location = New System.Drawing.Point(10, 172)
        Me.lblMandatory.Name = "lblMandatory"
        Me.lblMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMandatory.Size = New System.Drawing.Size(0, 13)
        Me.lblMandatory.TabIndex = 12
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(10, 43)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(0, 13)
        Me.lblDescription.TabIndex = 2
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCaption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCaption.Location = New System.Drawing.Point(10, 19)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCaption.Size = New System.Drawing.Size(0, 13)
        Me.lblCaption.TabIndex = 0
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(10, 67)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(0, 13)
        Me.lblType.TabIndex = 4
        '
        'lblDispOrd
        '
        Me.lblDispOrd.AutoSize = True
        Me.lblDispOrd.BackColor = System.Drawing.SystemColors.Control
        Me.lblDispOrd.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDispOrd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDispOrd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDispOrd.Location = New System.Drawing.Point(10, 93)
        Me.lblDispOrd.Name = "lblDispOrd"
        Me.lblDispOrd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDispOrd.Size = New System.Drawing.Size(0, 13)
        Me.lblDispOrd.TabIndex = 6
        '
        'lblLookUp
        '
        Me.lblLookUp.AutoSize = True
        Me.lblLookUp.BackColor = System.Drawing.SystemColors.Control
        Me.lblLookUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLookUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLookUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLookUp.Location = New System.Drawing.Point(10, 117)
        Me.lblLookUp.Name = "lblLookUp"
        Me.lblLookUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLookUp.Size = New System.Drawing.Size(0, 13)
        Me.lblLookUp.TabIndex = 8
        '
        'lblReadOnly
        '
        Me.lblReadOnly.AutoSize = True
        Me.lblReadOnly.BackColor = System.Drawing.SystemColors.Control
        Me.lblReadOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReadOnly.Location = New System.Drawing.Point(150, 172)
        Me.lblReadOnly.Name = "lblReadOnly"
        Me.lblReadOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadOnly.Size = New System.Drawing.Size(0, 13)
        Me.lblReadOnly.TabIndex = 14
        '
        'lblManualEntry
        '
        Me.lblManualEntry.AutoSize = True
        Me.lblManualEntry.BackColor = System.Drawing.SystemColors.Control
        Me.lblManualEntry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblManualEntry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManualEntry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblManualEntry.Location = New System.Drawing.Point(279, 172)
        Me.lblManualEntry.Name = "lblManualEntry"
        Me.lblManualEntry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblManualEntry.Size = New System.Drawing.Size(0, 13)
        Me.lblManualEntry.TabIndex = 16
        '
        'lblTab
        '
        Me.lblTab.BackColor = System.Drawing.SystemColors.Control
        Me.lblTab.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTab.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTab.Location = New System.Drawing.Point(10, 140)
        Me.lblTab.Name = "lblTab"
        Me.lblTab.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTab.Size = New System.Drawing.Size(81, 25)
        Me.lblTab.TabIndex = 10
        Me.lblTab.Text = "Tab :"
        Me.lblTab.Visible = False
        '
        'lvwDefnFlds
        '
        Me.lvwDefnFlds.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwDefnFlds, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwDefnFlds, True)
        Me.lvwDefnFlds.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDefnFlds.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwDefnFlds.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwDefnFlds, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwDefnFlds, "")
        Me.lvwDefnFlds.Location = New System.Drawing.Point(8, 196)
        Me.lvwDefnFlds.Name = "lvwDefnFlds"
        Me.lvwDefnFlds.Size = New System.Drawing.Size(481, 233)
        Me.listViewHelper1.SetSmallIcons(Me.lvwDefnFlds, "")
        Me.listViewHelper1.SetSorted(Me.lvwDefnFlds, False)
        Me.listViewHelper1.SetSortKey(Me.lvwDefnFlds, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwDefnFlds, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwDefnFlds.TabIndex = 21
        Me.lvwDefnFlds.UseCompatibleStateImageBehavior = False
        Me.lvwDefnFlds.View = System.Windows.Forms.View.Details
        '
        'txtCaption
        '
        Me.txtCaption.AcceptsReturn = True
        Me.txtCaption.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaption.Location = New System.Drawing.Point(120, 16)
        Me.txtCaption.MaxLength = 25
        Me.txtCaption.Name = "txtCaption"
        Me.txtCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaption.Size = New System.Drawing.Size(217, 21)
        Me.txtCaption.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(119, 40)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(217, 21)
        Me.txtDescription.TabIndex = 3
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(400, 92)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 20
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdModify
        '
        Me.cmdModify.BackColor = System.Drawing.SystemColors.Control
        Me.cmdModify.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdModify.Enabled = False
        Me.cmdModify.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdModify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdModify.Location = New System.Drawing.Point(400, 60)
        Me.cmdModify.Name = "cmdModify"
        Me.cmdModify.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdModify.Size = New System.Drawing.Size(73, 22)
        Me.cmdModify.TabIndex = 19
        Me.cmdModify.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdModify.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Enabled = False
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(400, 28)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 18
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cboType
        '
        Me.cboType.BackColor = System.Drawing.SystemColors.Window
        Me.cboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboType.Location = New System.Drawing.Point(119, 64)
        Me.cboType.Name = "cboType"
        Me.cboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboType.Size = New System.Drawing.Size(153, 21)
        Me.cboType.TabIndex = 5
        '
        'txtDispOrd
        '
        Me.txtDispOrd.AcceptsReturn = True
        Me.txtDispOrd.BackColor = System.Drawing.SystemColors.Window
        Me.txtDispOrd.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDispOrd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDispOrd.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDispOrd.Location = New System.Drawing.Point(119, 90)
        Me.txtDispOrd.MaxLength = 0
        Me.txtDispOrd.Name = "txtDispOrd"
        Me.txtDispOrd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDispOrd.Size = New System.Drawing.Size(81, 21)
        Me.txtDispOrd.TabIndex = 7
        '
        'cboLookUp
        '
        Me.cboLookUp.BackColor = System.Drawing.SystemColors.Window
        Me.cboLookUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLookUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLookUp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLookUp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLookUp.Location = New System.Drawing.Point(119, 114)
        Me.cboLookUp.Name = "cboLookUp"
        Me.cboLookUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLookUp.Size = New System.Drawing.Size(153, 21)
        Me.cboLookUp.TabIndex = 9
        '
        '_optMandatory_0
        '
        Me._optMandatory_0.BackColor = System.Drawing.SystemColors.Control
        Me._optMandatory_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMandatory_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMandatory_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMandatory_0.Location = New System.Drawing.Point(254, 172)
        Me._optMandatory_0.Name = "_optMandatory_0"
        Me._optMandatory_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMandatory_0.Size = New System.Drawing.Size(19, 16)
        Me._optMandatory_0.TabIndex = 15
        Me._optMandatory_0.TabStop = True
        Me._optMandatory_0.UseVisualStyleBackColor = False
        '
        '_optMandatory_1
        '
        Me._optMandatory_1.BackColor = System.Drawing.SystemColors.Control
        Me._optMandatory_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMandatory_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMandatory_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMandatory_1.Location = New System.Drawing.Point(120, 172)
        Me._optMandatory_1.Name = "_optMandatory_1"
        Me._optMandatory_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMandatory_1.Size = New System.Drawing.Size(19, 16)
        Me._optMandatory_1.TabIndex = 13
        Me._optMandatory_1.TabStop = True
        Me._optMandatory_1.UseVisualStyleBackColor = False
        '
        '_optMandatory_2
        '
        Me._optMandatory_2.BackColor = System.Drawing.SystemColors.Control
        Me._optMandatory_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optMandatory_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optMandatory_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optMandatory_2.Location = New System.Drawing.Point(387, 172)
        Me._optMandatory_2.Name = "_optMandatory_2"
        Me._optMandatory_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optMandatory_2.Size = New System.Drawing.Size(19, 16)
        Me._optMandatory_2.TabIndex = 17
        Me._optMandatory_2.TabStop = True
        Me._optMandatory_2.UseVisualStyleBackColor = False
        '
        'cboTab
        '
        Me.cboTab.BackColor = System.Drawing.SystemColors.Window
        Me.cboTab.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTab.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTab.Location = New System.Drawing.Point(120, 140)
        Me.cboTab.Name = "cboTab"
        Me.cboTab.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTab.Size = New System.Drawing.Size(153, 21)
        Me.cboTab.TabIndex = 11
        Me.cboTab.Visible = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(514, 500)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(332, 129)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptMandatory()
		Me.optMandatory(2) = _optMandatory_2
		Me.optMandatory(1) = _optMandatory_1
		Me.optMandatory(0) = _optMandatory_0
	End Sub
#End Region 
End Class