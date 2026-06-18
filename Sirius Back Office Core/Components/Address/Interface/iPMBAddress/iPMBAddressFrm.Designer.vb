<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptAddress()
		InitializecmdPrevious()
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents Image1 As System.Windows.Forms.PictureBox
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblClearFutureAddress As System.Windows.Forms.Label
	Public WithEvents cboAddUsageType As PMLookupControl.cboPMLookup
	Public WithEvents pnlAdPostcode As System.Windows.Forms.Label
	Public WithEvents pnlAdReference As System.Windows.Forms.Label
	Public WithEvents lblAddressUsageType As System.Windows.Forms.Label
	Public WithEvents lbAdReference As System.Windows.Forms.Label
	Public WithEvents lblAdPostcode As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Private WithEvents _optAddress_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optAddress_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraFutureDateAddressChanges As System.Windows.Forms.GroupBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents cmdClearFutureAddress As System.Windows.Forms.Button
	Public WithEvents uctadd As PMAddressControl.uctPMAddressControl
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwContacts As System.Windows.Forms.ListView
	Public WithEvents cmdEditCon As System.Windows.Forms.Button
	Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
	Public WithEvents cmdAddCon As System.Windows.Forms.Button
	Public WithEvents Frame4 As System.Windows.Forms.GroupBox
	Public WithEvents pnlConPostCode As System.Windows.Forms.Label
	Public WithEvents pnlConReference As System.Windows.Forms.Label
	Public WithEvents lblConPostCode As System.Windows.Forms.Label
	Public WithEvents lblConReference As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(0) As System.Windows.Forms.Button
	Public cmdPrevious(1) As System.Windows.Forms.Button
	Public optAddress(1) As System.Windows.Forms.RadioButton
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
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblClearFutureAddress = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboAddUsageType = New PMLookupControl.cboPMLookup
        Me.pnlAdPostcode = New System.Windows.Forms.Label
        Me.pnlAdReference = New System.Windows.Forms.Label
        Me.lblAddressUsageType = New System.Windows.Forms.Label
        Me.lbAdReference = New System.Windows.Forms.Label
        Me.lblAdPostcode = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.fraFutureDateAddressChanges = New System.Windows.Forms.GroupBox
        Me._optAddress_1 = New System.Windows.Forms.RadioButton
        Me._optAddress_0 = New System.Windows.Forms.RadioButton
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.cmdClearFutureAddress = New System.Windows.Forms.Button
        Me.uctadd = New PMAddressControl.uctPMAddressControl
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me.Frame4 = New System.Windows.Forms.GroupBox
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.pnlConPostCode = New System.Windows.Forms.Label
        Me.pnlConReference = New System.Windows.Forms.Label
        Me.lblConPostCode = New System.Windows.Forms.Label
        Me.lblConReference = New System.Windows.Forms.Label
        Me.Image1 = New System.Windows.Forms.PictureBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraFutureDateAddressChanges.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.Frame4.SuspendLayout()
        Me.Frame2.SuspendLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(632, 309)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 30
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
        Me.cmdCancel.Location = New System.Drawing.Point(552, 309)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 29
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
        Me.cmdOK.Location = New System.Drawing.Point(472, 309)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 28
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(138, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 10)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(701, 292)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClearFutureAddress)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFutureDateAddressChanges)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdClearFutureAddress)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctadd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(693, 266)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Address"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(477, 171)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(66, 29)
        Me.lblEffectiveDate.TabIndex = 12
        Me.lblEffectiveDate.Text = "Effective Date:"
        Me.lblEffectiveDate.Visible = False
        '
        'lblClearFutureAddress
        '
        Me.lblClearFutureAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblClearFutureAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClearFutureAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClearFutureAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClearFutureAddress.Location = New System.Drawing.Point(480, 204)
        Me.lblClearFutureAddress.Name = "lblClearFutureAddress"
        Me.lblClearFutureAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClearFutureAddress.Size = New System.Drawing.Size(81, 41)
        Me.lblClearFutureAddress.TabIndex = 14
        Me.lblClearFutureAddress.Text = "Clear Future Dated Address"
        Me.lblClearFutureAddress.Visible = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboAddUsageType)
        Me.Frame1.Controls.Add(Me.pnlAdPostcode)
        Me.Frame1.Controls.Add(Me.pnlAdReference)
        Me.Frame1.Controls.Add(Me.lblAddressUsageType)
        Me.Frame1.Controls.Add(Me.lbAdReference)
        Me.Frame1.Controls.Add(Me.lblAdPostcode)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 12)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(681, 79)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        '
        'cboAddUsageType
        '
        Me.cboAddUsageType.DefaultItemId = 0
        Me.cboAddUsageType.FirstItem = ""
        Me.cboAddUsageType.ItemId = 0
        Me.cboAddUsageType.ListIndex = -1
        Me.cboAddUsageType.Location = New System.Drawing.Point(136, 48)
        Me.cboAddUsageType.Name = "cboAddUsageType"
        Me.cboAddUsageType.PMLookupProductFamily = 9
        Me.cboAddUsageType.SingleItemId = 0
        Me.cboAddUsageType.Size = New System.Drawing.Size(177, 21)
        Me.cboAddUsageType.Sorted = True
        Me.cboAddUsageType.TabIndex = 7
        Me.cboAddUsageType.TableName = "address_usage_type"
        Me.cboAddUsageType.ToolTipText = ""
        Me.cboAddUsageType.WhereClause = ""
        '
        'pnlAdPostcode
        '
        Me.pnlAdPostcode.BackColor = System.Drawing.SystemColors.Control
        Me.pnlAdPostcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAdPostcode.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlAdPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAdPostcode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlAdPostcode.Location = New System.Drawing.Point(427, 18)
        Me.pnlAdPostcode.Name = "pnlAdPostcode"
        Me.pnlAdPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlAdPostcode.Size = New System.Drawing.Size(81, 21)
        Me.pnlAdPostcode.TabIndex = 5
        '
        'pnlAdReference
        '
        Me.pnlAdReference.BackColor = System.Drawing.SystemColors.Control
        Me.pnlAdReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlAdReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlAdReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAdReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlAdReference.Location = New System.Drawing.Point(136, 18)
        Me.pnlAdReference.Name = "pnlAdReference"
        Me.pnlAdReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlAdReference.Size = New System.Drawing.Size(177, 21)
        Me.pnlAdReference.TabIndex = 3
        Me.pnlAdReference.UseMnemonic = False
        '
        'lblAddressUsageType
        '
        Me.lblAddressUsageType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddressUsageType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddressUsageType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddressUsageType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddressUsageType.Location = New System.Drawing.Point(16, 48)
        Me.lblAddressUsageType.Name = "lblAddressUsageType"
        Me.lblAddressUsageType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddressUsageType.Size = New System.Drawing.Size(81, 17)
        Me.lblAddressUsageType.TabIndex = 6
        Me.lblAddressUsageType.Text = "Type:"
        '
        'lbAdReference
        '
        Me.lbAdReference.AutoSize = True
        Me.lbAdReference.BackColor = System.Drawing.SystemColors.Control
        Me.lbAdReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbAdReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAdReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbAdReference.Location = New System.Drawing.Point(16, 20)
        Me.lbAdReference.Name = "lbAdReference"
        Me.lbAdReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbAdReference.Size = New System.Drawing.Size(60, 13)
        Me.lbAdReference.TabIndex = 2
        Me.lbAdReference.Text = "Reference:"
        '
        'lblAdPostcode
        '
        Me.lblAdPostcode.AutoSize = True
        Me.lblAdPostcode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdPostcode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdPostcode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdPostcode.Location = New System.Drawing.Point(340, 20)
        Me.lblAdPostcode.Name = "lblAdPostcode"
        Me.lblAdPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdPostcode.Size = New System.Drawing.Size(55, 13)
        Me.lblAdPostcode.TabIndex = 4
        Me.lblAdPostcode.Text = "Postcode:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(656, 236)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 16
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraFutureDateAddressChanges
        '
        Me.fraFutureDateAddressChanges.BackColor = System.Drawing.SystemColors.Control
        Me.fraFutureDateAddressChanges.Controls.Add(Me._optAddress_1)
        Me.fraFutureDateAddressChanges.Controls.Add(Me._optAddress_0)
        Me.fraFutureDateAddressChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFutureDateAddressChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFutureDateAddressChanges.Location = New System.Drawing.Point(480, 95)
        Me.fraFutureDateAddressChanges.Name = "fraFutureDateAddressChanges"
        Me.fraFutureDateAddressChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFutureDateAddressChanges.Size = New System.Drawing.Size(209, 73)
        Me.fraFutureDateAddressChanges.TabIndex = 9
        Me.fraFutureDateAddressChanges.TabStop = False
        Me.fraFutureDateAddressChanges.Visible = False
        '
        '_optAddress_1
        '
        Me._optAddress_1.BackColor = System.Drawing.SystemColors.Control
        Me._optAddress_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optAddress_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAddress_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAddress_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAddress_1.Location = New System.Drawing.Point(8, 32)
        Me._optAddress_1.Name = "_optAddress_1"
        Me._optAddress_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAddress_1.Size = New System.Drawing.Size(185, 33)
        Me._optAddress_1.TabIndex = 11
        Me._optAddress_1.TabStop = True
        Me._optAddress_1.Text = "Future Dated Address Change"
        Me._optAddress_1.UseVisualStyleBackColor = False
        '
        '_optAddress_0
        '
        Me._optAddress_0.BackColor = System.Drawing.SystemColors.Control
        Me._optAddress_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optAddress_0.Checked = True
        Me._optAddress_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAddress_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAddress_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAddress_0.Location = New System.Drawing.Point(8, 16)
        Me._optAddress_0.Name = "_optAddress_0"
        Me._optAddress_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAddress_0.Size = New System.Drawing.Size(185, 17)
        Me._optAddress_0.TabIndex = 10
        Me._optAddress_0.TabStop = True
        Me._optAddress_0.Text = "Current Address"
        Me._optAddress_0.UseVisualStyleBackColor = False
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(544, 172)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(145, 20)
        Me.txtEffectiveDate.TabIndex = 13
        Me.txtEffectiveDate.Visible = False
        '
        'cmdClearFutureAddress
        '
        Me.cmdClearFutureAddress.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearFutureAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearFutureAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearFutureAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearFutureAddress.Location = New System.Drawing.Point(560, 204)
        Me.cmdClearFutureAddress.Name = "cmdClearFutureAddress"
        Me.cmdClearFutureAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearFutureAddress.Size = New System.Drawing.Size(41, 17)
        Me.cmdClearFutureAddress.TabIndex = 15
        Me.cmdClearFutureAddress.Text = "..."
        Me.cmdClearFutureAddress.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClearFutureAddress.UseVisualStyleBackColor = False
        Me.cmdClearFutureAddress.Visible = False
        '
        'uctadd
        '
        Me.uctadd.AddressLine1 = ""
        Me.uctadd.AddressLine2 = ""
        Me.uctadd.AddressLine3 = ""
        Me.uctadd.AddressLine4 = ""
        Me.uctadd.Caption = ""
        Me.uctadd.CaptionAddress1 = "No. && street name:"
        Me.uctadd.CaptionAddress2 = "Locality:"
        Me.uctadd.CaptionAddress3 = "Town:"
        Me.uctadd.CaptionAddress4 = "County:"
        Me.uctadd.CaptionCountry = "Country:"
        Me.uctadd.CaptionFontBoldAddress1 = False
        Me.uctadd.CaptionFontBoldPostCode = False
        Me.uctadd.CaptionPostCode = "Postcode:"
        Me.uctadd.ClearButtonCaption = "X"
        Me.uctadd.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctadd.ClearButtonLeft = 6420
        Me.uctadd.ClearButtonWidth = 360
        Me.uctadd.CountryId = 1
        Me.uctadd.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctadd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctadd.IsCountryRequired = 1
        Me.uctadd.IsPostCodeRequired = 1
        Me.uctadd.Location = New System.Drawing.Point(8, 95)
        Me.uctadd.Name = "uctadd"
        Me.uctadd.Organisation = ""
        Me.uctadd.PMAddressCnt = 0
        Me.uctadd.PMDatabaseID = 0
        Me.uctadd.PostCode = ""
        Me.uctadd.QAS2PMAddress1 = "3,4,2,5,6"
        Me.uctadd.QAS2PMAddress2 = "8,7"
        Me.uctadd.QAS2PMAddress3 = "9"
        Me.uctadd.QAS2PMAddress4 = ""
        Me.uctadd.QASDatabaseID = 3
        Me.uctadd.SearchButtonCaption = "..."
        Me.uctadd.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctadd.SearchButtonHeight = 285
        Me.uctadd.SearchButtonLeft = 6000
        Me.uctadd.SearchButtonTop = 1545
        Me.uctadd.SearchButtonWidth = 360
        Me.uctadd.Size = New System.Drawing.Size(453, 152)
        Me.uctadd.TabIndex = 8
        Me.uctadd.WarningMessage = ""
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame4)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(693, 266)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Contact"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(16, 236)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(30, 19)
        Me._cmdPrevious_1.TabIndex = 27
        Me._cmdPrevious_1.Text = "&<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.lvwContacts)
        Me.Frame4.Controls.Add(Me.cmdEditCon)
        Me.Frame4.Controls.Add(Me.cmdDeleteCon)
        Me.Frame4.Controls.Add(Me.cmdAddCon)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(16, 71)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(673, 160)
        Me.Frame4.TabIndex = 22
        Me.Frame4.TabStop = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.FullRowSelect = True
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(649, 105)
        Me.lvwContacts.TabIndex = 23
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 74
        '
        '_lvwContacts_ColumnHeader_2
        '
        Me._lvwContacts_ColumnHeader_2.Text = "Number"
        Me._lvwContacts_ColumnHeader_2.Width = 67
        '
        '_lvwContacts_ColumnHeader_3
        '
        Me._lvwContacts_ColumnHeader_3.Text = "Extension"
        Me._lvwContacts_ColumnHeader_3.Width = 67
        '
        '_lvwContacts_ColumnHeader_4
        '
        Me._lvwContacts_ColumnHeader_4.Text = "Type"
        Me._lvwContacts_ColumnHeader_4.Width = 97
        '
        '_lvwContacts_ColumnHeader_5
        '
        Me._lvwContacts_ColumnHeader_5.Text = "Description"
        Me._lvwContacts_ColumnHeader_5.Width = 134
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(168, 128)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 26
        Me.cmdEditCon.Text = "&Edit"
        Me.cmdEditCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditCon.UseVisualStyleBackColor = False
        '
        'cmdDeleteCon
        '
        Me.cmdDeleteCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteCon.Location = New System.Drawing.Point(88, 128)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 25
        Me.cmdDeleteCon.Text = "&Delete"
        Me.cmdDeleteCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteCon.UseVisualStyleBackColor = False
        '
        'cmdAddCon
        '
        Me.cmdAddCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddCon.Location = New System.Drawing.Point(8, 128)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 24
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.pnlConPostCode)
        Me.Frame2.Controls.Add(Me.pnlConReference)
        Me.Frame2.Controls.Add(Me.lblConPostCode)
        Me.Frame2.Controls.Add(Me.lblConReference)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(16, 12)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(673, 55)
        Me.Frame2.TabIndex = 17
        Me.Frame2.TabStop = False
        '
        'pnlConPostCode
        '
        Me.pnlConPostCode.BackColor = System.Drawing.SystemColors.Control
        Me.pnlConPostCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConPostCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlConPostCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConPostCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlConPostCode.Location = New System.Drawing.Point(427, 18)
        Me.pnlConPostCode.Name = "pnlConPostCode"
        Me.pnlConPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlConPostCode.Size = New System.Drawing.Size(81, 21)
        Me.pnlConPostCode.TabIndex = 21
        '
        'pnlConReference
        '
        Me.pnlConReference.BackColor = System.Drawing.SystemColors.Control
        Me.pnlConReference.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlConReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlConReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlConReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlConReference.Location = New System.Drawing.Point(136, 18)
        Me.pnlConReference.Name = "pnlConReference"
        Me.pnlConReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlConReference.Size = New System.Drawing.Size(177, 21)
        Me.pnlConReference.TabIndex = 19
        Me.pnlConReference.UseMnemonic = False
        '
        'lblConPostCode
        '
        Me.lblConPostCode.AutoSize = True
        Me.lblConPostCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblConPostCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConPostCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConPostCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConPostCode.Location = New System.Drawing.Point(340, 20)
        Me.lblConPostCode.Name = "lblConPostCode"
        Me.lblConPostCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConPostCode.Size = New System.Drawing.Size(55, 13)
        Me.lblConPostCode.TabIndex = 20
        Me.lblConPostCode.Text = "Postcode:"
        '
        'lblConReference
        '
        Me.lblConReference.AutoSize = True
        Me.lblConReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblConReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblConReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblConReference.Location = New System.Drawing.Point(16, 20)
        Me.lblConReference.Name = "lblConReference"
        Me.lblConReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblConReference.Size = New System.Drawing.Size(60, 13)
        Me.lblConReference.TabIndex = 18
        Me.lblConReference.Text = "Reference:"
        '
        'Image1
        '
        Me.Image1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Image1.Image = CType(resources.GetObject("Image1.Image"), System.Drawing.Image)
        Me.Image1.Location = New System.Drawing.Point(-4440, 32)
        Me.Image1.Name = "Image1"
        Me.Image1.Size = New System.Drawing.Size(32, 32)
        Me.Image1.TabIndex = 0
        Me.Image1.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(712, 339)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Address"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraFutureDateAddressChanges.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        CType(Me.Image1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub InitializeoptAddress()
		Me.optAddress(1) = _optAddress_1
		Me.optAddress(0) = _optAddress_0
	End Sub
	Sub InitializecmdPrevious()
		Me.cmdPrevious(1) = _cmdPrevious_1
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class
