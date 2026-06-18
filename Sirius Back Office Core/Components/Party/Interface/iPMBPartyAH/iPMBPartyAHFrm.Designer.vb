<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializecmdPrev()
		InitializecmdNext()
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdTransfer As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboCommissionAccount As System.Windows.Forms.ComboBox
	Public WithEvents cboDepartment As PMLookupControl.cboPMLookup
	Public WithEvents txtInitials As System.Windows.Forms.TextBox
	Public WithEvents cboTitle As System.Windows.Forms.ComboBox
	Public WithEvents txtForename As System.Windows.Forms.TextBox
	Public WithEvents txtLastname As System.Windows.Forms.TextBox
	Public WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public WithEvents cboCurrency As PMLookupControl.cboPMLookup
	Public WithEvents pnlType As System.Windows.Forms.Label
	Public WithEvents lblCommissionAccount As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblInitials As System.Windows.Forms.Label
	Public WithEvents lblTitle As System.Windows.Forms.Label
	Public WithEvents lblDepartment As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblForename As System.Windows.Forms.Label
	Public WithEvents lblLastname As System.Windows.Forms.Label
	Public WithEvents lblClientCode As System.Windows.Forms.Label
	Public WithEvents fraFrame As System.Windows.Forms.GroupBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
	Private WithEvents _cmdPrev_0 As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwContacts As System.Windows.Forms.ListView
	Public WithEvents fraContact As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents cmdEditAd As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
	Public WithEvents cmdAddAd As System.Windows.Forms.Button
	Private WithEvents _lvwAddress_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddress_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddress_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddress_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddress_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddress_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAddress As System.Windows.Forms.ListView
	Public WithEvents fraAddress As System.Windows.Forms.GroupBox
	Private WithEvents _cmdPrev_1 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrev_2 As System.Windows.Forms.Button
	Public WithEvents uctPickListBranches As uctPickList.PickList
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList2 As System.Windows.Forms.ImageList
	Public cmdNext(1) As System.Windows.Forms.Button
	Public cmdPrev(2) As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdTransfer = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraFrame = New System.Windows.Forms.GroupBox
        Me.cboCommissionAccount = New System.Windows.Forms.ComboBox
        Me.cboDepartment = New PMLookupControl.cboPMLookup
        Me.txtInitials = New System.Windows.Forms.TextBox
        Me.cboTitle = New System.Windows.Forms.ComboBox
        Me.txtForename = New System.Windows.Forms.TextBox
        Me.txtLastname = New System.Windows.Forms.TextBox
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.cboCurrency = New PMLookupControl.cboPMLookup
        Me.pnlType = New System.Windows.Forms.Label
        Me.lblCommissionAccount = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblInitials = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblDepartment = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me.lblForename = New System.Windows.Forms.Label
        Me.lblLastname = New System.Windows.Forms.Label
        Me.lblClientCode = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._cmdPrev_0 = New System.Windows.Forms.Button
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.lvwAddress = New System.Windows.Forms.ListView
        Me._lvwAddress_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddress_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._cmdPrev_1 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrev_2 = New System.Windows.Forms.Button
        Me.uctPickListBranches = New uctPickList.PickList
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraFrame.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        ' CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(320, 280)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 39
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdTransfer
        '
        Me.cmdTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTransfer.Location = New System.Drawing.Point(8, 280)
        Me.cmdTransfer.Name = "cmdTransfer"
        Me.cmdTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTransfer.Size = New System.Drawing.Size(73, 22)
        Me.cmdTransfer.TabIndex = 20
        Me.cmdTransfer.Text = "&Transfer"
        Me.cmdTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTransfer.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(88, 280)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 21
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(560, 280)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 24
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
        Me.cmdCancel.Location = New System.Drawing.Point(480, 280)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 23
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
        Me.cmdOK.Location = New System.Drawing.Point(400, 280)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 22
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(124, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(629, 269)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFrame)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(621, 243)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Client"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraFrame
        '
        Me.fraFrame.BackColor = System.Drawing.SystemColors.Control
        Me.fraFrame.Controls.Add(Me.cboCommissionAccount)
        Me.fraFrame.Controls.Add(Me.cboDepartment)
        Me.fraFrame.Controls.Add(Me.txtInitials)
        Me.fraFrame.Controls.Add(Me.cboTitle)
        Me.fraFrame.Controls.Add(Me.txtForename)
        Me.fraFrame.Controls.Add(Me.txtLastname)
        Me.fraFrame.Controls.Add(Me.txtClientCode)
        Me.fraFrame.Controls.Add(Me.cboCurrency)
        Me.fraFrame.Controls.Add(Me.pnlType)
        Me.fraFrame.Controls.Add(Me.lblCommissionAccount)
        Me.fraFrame.Controls.Add(Me.lblType)
        Me.fraFrame.Controls.Add(Me.lblInitials)
        Me.fraFrame.Controls.Add(Me.lblTitle)
        Me.fraFrame.Controls.Add(Me.lblDepartment)
        Me.fraFrame.Controls.Add(Me.lblCurrency)
        Me.fraFrame.Controls.Add(Me.lblForename)
        Me.fraFrame.Controls.Add(Me.lblLastname)
        Me.fraFrame.Controls.Add(Me.lblClientCode)
        Me.fraFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFrame.Location = New System.Drawing.Point(16, 12)
        Me.fraFrame.Name = "fraFrame"
        Me.fraFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFrame.Size = New System.Drawing.Size(577, 185)
        Me.fraFrame.TabIndex = 38
        Me.fraFrame.TabStop = False
        '
        'cboCommissionAccount
        '
        Me.cboCommissionAccount.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionAccount.Location = New System.Drawing.Point(152, 152)
        Me.cboCommissionAccount.Name = "cboCommissionAccount"
        Me.cboCommissionAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionAccount.Size = New System.Drawing.Size(153, 21)
        Me.cboCommissionAccount.TabIndex = 18
        '
        'cboDepartment
        '
        Me.cboDepartment.DefaultItemId = 0
        'Me.cboDepartment.FirstItem = ""
        Me.cboDepartment.ItemId = 0
        Me.cboDepartment.ListIndex = -1
        Me.cboDepartment.Location = New System.Drawing.Point(408, 56)
        Me.cboDepartment.Name = "cboDepartment"
        Me.cboDepartment.PMLookupProductFamily = 9
        Me.cboDepartment.SingleItemId = 0
        Me.cboDepartment.Size = New System.Drawing.Size(153, 21)
        Me.cboDepartment.Sorted = True
        Me.cboDepartment.TabIndex = 8
        Me.cboDepartment.TableName = "Department"
        Me.cboDepartment.ToolTipText = ""
        Me.cboDepartment.WhereClause = ""
        '
        'txtInitials
        '
        Me.txtInitials.AcceptsReturn = True
        Me.txtInitials.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitials.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitials.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitials.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitials.Location = New System.Drawing.Point(408, 120)
        Me.txtInitials.MaxLength = 0
        Me.txtInitials.Name = "txtInitials"
        Me.txtInitials.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitials.Size = New System.Drawing.Size(153, 20)
        Me.txtInitials.TabIndex = 16
        '
        'cboTitle
        '
        Me.cboTitle.BackColor = System.Drawing.SystemColors.Window
        Me.cboTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTitle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTitle.Location = New System.Drawing.Point(408, 88)
        Me.cboTitle.Name = "cboTitle"
        Me.cboTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTitle.Size = New System.Drawing.Size(153, 21)
        Me.cboTitle.TabIndex = 12
        '
        'txtForename
        '
        Me.txtForename.AcceptsReturn = True
        Me.txtForename.BackColor = System.Drawing.SystemColors.Window
        Me.txtForename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtForename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtForename.Location = New System.Drawing.Point(152, 88)
        Me.txtForename.MaxLength = 0
        Me.txtForename.Name = "txtForename"
        Me.txtForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtForename.Size = New System.Drawing.Size(153, 20)
        Me.txtForename.TabIndex = 10
        '
        'txtLastname
        '
        Me.txtLastname.AcceptsReturn = True
        Me.txtLastname.BackColor = System.Drawing.SystemColors.Window
        Me.txtLastname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLastname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLastname.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLastname.Location = New System.Drawing.Point(152, 56)
        Me.txtLastname.MaxLength = 0
        Me.txtLastname.Name = "txtLastname"
        Me.txtLastname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLastname.Size = New System.Drawing.Size(153, 20)
        Me.txtLastname.TabIndex = 6
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(152, 24)
        Me.txtClientCode.MaxLength = 0
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(153, 20)
        Me.txtClientCode.TabIndex = 2
        '
        'cboCurrency
        '
        Me.cboCurrency.DefaultItemId = 0
        ' Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ItemId = 0
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(152, 120)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.PMLookupProductFamily = 1
        Me.cboCurrency.SingleItemId = 0
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.Sorted = True
        Me.cboCurrency.TabIndex = 14
        Me.cboCurrency.TableName = "Currency"
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhereClause = ""
        '
        'pnlType
        '
        Me.pnlType.BackColor = System.Drawing.SystemColors.Control
        Me.pnlType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlType.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlType.Location = New System.Drawing.Point(408, 24)
        Me.pnlType.Name = "pnlType"
        Me.pnlType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlType.Size = New System.Drawing.Size(153, 21)
        Me.pnlType.TabIndex = 4
        '
        'lblCommissionAccount
        '
        Me.lblCommissionAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionAccount.Location = New System.Drawing.Point(8, 156)
        Me.lblCommissionAccount.Name = "lblCommissionAccount"
        Me.lblCommissionAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionAccount.Size = New System.Drawing.Size(145, 21)
        Me.lblCommissionAccount.TabIndex = 17
        Me.lblCommissionAccount.Text = "Commission Account:"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(320, 28)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(73, 17)
        Me.lblType.TabIndex = 3
        Me.lblType.Text = "Type:"
        '
        'lblInitials
        '
        Me.lblInitials.BackColor = System.Drawing.SystemColors.Control
        Me.lblInitials.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInitials.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInitials.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInitials.Location = New System.Drawing.Point(320, 123)
        Me.lblInitials.Name = "lblInitials"
        Me.lblInitials.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInitials.Size = New System.Drawing.Size(81, 17)
        Me.lblInitials.TabIndex = 15
        Me.lblInitials.Text = "Initials:"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(320, 90)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(81, 17)
        Me.lblTitle.TabIndex = 11
        Me.lblTitle.Text = "Title:"
        '
        'lblDepartment
        '
        Me.lblDepartment.BackColor = System.Drawing.SystemColors.Control
        Me.lblDepartment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepartment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepartment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepartment.Location = New System.Drawing.Point(320, 60)
        Me.lblDepartment.Name = "lblDepartment"
        Me.lblDepartment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepartment.Size = New System.Drawing.Size(81, 17)
        Me.lblDepartment.TabIndex = 7
        Me.lblDepartment.Text = "Department:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(8, 124)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(81, 17)
        Me.lblCurrency.TabIndex = 13
        Me.lblCurrency.Text = "Currency:"
        '
        'lblForename
        '
        Me.lblForename.BackColor = System.Drawing.SystemColors.Control
        Me.lblForename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblForename.Location = New System.Drawing.Point(8, 90)
        Me.lblForename.Name = "lblForename"
        Me.lblForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblForename.Size = New System.Drawing.Size(81, 17)
        Me.lblForename.TabIndex = 9
        Me.lblForename.Text = "Forename:"
        '
        'lblLastname
        '
        Me.lblLastname.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastname.Location = New System.Drawing.Point(8, 58)
        Me.lblLastname.Name = "lblLastname"
        Me.lblLastname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastname.Size = New System.Drawing.Size(81, 17)
        Me.lblLastname.TabIndex = 5
        Me.lblLastname.Text = "Lastname:"
        '
        'lblClientCode
        '
        Me.lblClientCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientCode.Location = New System.Drawing.Point(8, 25)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientCode.Size = New System.Drawing.Size(137, 17)
        Me.lblClientCode.TabIndex = 1
        Me.lblClientCode.Text = "Client code:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(576, 212)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 19
        Me._cmdNext_0.TabStop = False
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrev_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(621, 243)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Contact"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(576, 212)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 31
        Me._cmdNext_1.TabStop = False
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_cmdPrev_0
        '
        Me._cmdPrev_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_0.Location = New System.Drawing.Point(8, 212)
        Me._cmdPrev_0.Name = "_cmdPrev_0"
        Me._cmdPrev_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_0.TabIndex = 30
        Me._cmdPrev_0.TabStop = False
        Me._cmdPrev_0.Text = "<<"
        Me._cmdPrev_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_0.UseVisualStyleBackColor = False
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdDelete)
        Me.fraContact.Controls.Add(Me.cmdEdit)
        Me.fraContact.Controls.Add(Me.cmdAdd)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(8, 12)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(607, 189)
        Me.fraContact.TabIndex = 25
        Me.fraContact.TabStop = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(164, 160)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 29
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
        Me.cmdEdit.Location = New System.Drawing.Point(86, 160)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 28
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(8, 160)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 27
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwContacts, "")
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwContacts, True)
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        ' Me.listViewHelper1.SetItemClickMethod(Me.lvwContacts, "")
        'Me.listViewHelper1.SetLargeIcons(Me.lvwContacts, "")
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(591, 137)
        'Me.listViewHelper1.SetSmallIcons(Me.lvwContacts, "")
        'Me.listViewHelper1.SetSorted(Me.lvwContacts, False)
        'Me.listViewHelper1.SetSortKey(Me.lvwContacts, 0)
        ' Me.listViewHelper1.SetSortOrder(Me.lvwContacts, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwContacts.TabIndex = 26
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 97
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Text = "Number"
        Me._lvwContacts_ColumnHeader_2.Width = 97
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Text = "Extension"
        Me._lvwContacts_ColumnHeader_3.Width = 97
        '
        '_lvwContacts_ColumnHeader_4
        '
        Me._lvwContacts_ColumnHeader_4.Text = "Contact Type"
        Me._lvwContacts_ColumnHeader_4.Width = 97
        '
        '_lvwContacts_ColumnHeader_5
        '
        Me._lvwContacts_ColumnHeader_5.Text = "Description"
        Me._lvwContacts_ColumnHeader_5.Width = 97
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrev_1)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(621, 243)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Address"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.lvwAddress)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 12)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(607, 189)
        Me.fraAddress.TabIndex = 32
        Me.fraAddress.TabStop = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Enabled = False
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(86, 160)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 35
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Enabled = False
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(164, 160)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 36
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(8, 160)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 34
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'lvwAddress
        '
        Me.lvwAddress.BackColor = System.Drawing.SystemColors.Window
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwAddress, "")
        Me.lvwAddress.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddress_ColumnHeader_1, Me._lvwAddress_ColumnHeader_2, Me._lvwAddress_ColumnHeader_3, Me._lvwAddress_ColumnHeader_4, Me._lvwAddress_ColumnHeader_5, Me._lvwAddress_ColumnHeader_6})
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwAddress, True)
        Me.lvwAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddress.ForeColor = System.Drawing.SystemColors.WindowText
        'Me.listViewHelper1.SetItemClickMethod(Me.lvwAddress, "")
        ' Me.listViewHelper1.SetLargeIcons(Me.lvwAddress, "")
        Me.lvwAddress.LargeImageList = Me.ImageList2
        Me.lvwAddress.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddress.Name = "lvwAddress"
        Me.lvwAddress.Size = New System.Drawing.Size(591, 137)
        'Me.listViewHelper1.SetSmallIcons(Me.lvwAddress, "")
        Me.lvwAddress.SmallImageList = Me.ImageList2
        'Me.listViewHelper1.SetSorted(Me.lvwAddress, False)
        ' Me.listViewHelper1.SetSortKey(Me.lvwAddress, 0)
        ' Me.listViewHelper1.SetSortOrder(Me.lvwAddress, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwAddress.TabIndex = 33
        Me.lvwAddress.UseCompatibleStateImageBehavior = False
        Me.lvwAddress.View = System.Windows.Forms.View.Details
        '
        '_lvwAddress_ColumnHeader_1
        '
        Me._lvwAddress_ColumnHeader_1.Width = 97
        '
        '_lvwAddress_ColumnHeader_2
        '
        Me._lvwAddress_ColumnHeader_2.Width = 97
        '
        '_lvwAddress_ColumnHeader_3
        '
        Me._lvwAddress_ColumnHeader_3.Width = 97
        '
        '_lvwAddress_ColumnHeader_4
        '
        Me._lvwAddress_ColumnHeader_4.Width = 97
        '
        '_lvwAddress_ColumnHeader_5
        '
        Me._lvwAddress_ColumnHeader_5.Width = 97
        '
        '_lvwAddress_ColumnHeader_6
        '
        Me._lvwAddress_ColumnHeader_6.Width = 97
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "NOTE")
        Me.ImageList2.Images.SetKeyName(3, "LETTER")
        Me.ImageList2.Images.SetKeyName(4, "COMMISSION")
        Me.ImageList2.Images.SetKeyName(5, "ADDRESS")
        Me.ImageList2.Images.SetKeyName(6, "")
        '
        '_cmdPrev_1
        '
        Me._cmdPrev_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_1.Location = New System.Drawing.Point(8, 212)
        Me._cmdPrev_1.Name = "_cmdPrev_1"
        Me._cmdPrev_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_1.TabIndex = 37
        Me._cmdPrev_1.TabStop = False
        Me._cmdPrev_1.Text = "<<"
        Me._cmdPrev_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_1.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrev_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me.uctPickListBranches)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(621, 243)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "3 - Branches"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        '_cmdPrev_2
        '
        Me._cmdPrev_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrev_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrev_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrev_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrev_2.Location = New System.Drawing.Point(18, 218)
        Me._cmdPrev_2.Name = "_cmdPrev_2"
        Me._cmdPrev_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrev_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrev_2.TabIndex = 41
        Me._cmdPrev_2.TabStop = False
        Me._cmdPrev_2.Text = "<<"
        Me._cmdPrev_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrev_2.UseVisualStyleBackColor = False
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Prevent Access"
        Me.uctPickListBranches.BusinessObject = "bSIRPartyAH.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(8, 12)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "party_handler_branch"
        Me.uctPickListBranches.Size = New System.Drawing.Size(609, 205)
        Me.uctPickListBranches.TabIndex = 40
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(641, 312)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdTransfer)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(-10, -4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Account Handler"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraFrame.ResumeLayout(False)
        Me.fraFrame.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializecmdPrev()
		Me.cmdPrev(2) = _cmdPrev_2
		Me.cmdPrev(1) = _cmdPrev_1
		Me.cmdPrev(0) = _cmdPrev_0
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class