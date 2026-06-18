<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptFieldType()
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _optFieldType_20 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_19 As System.Windows.Forms.RadioButton
	Public WithEvents chkIsInMISExport As System.Windows.Forms.CheckBox
	Public WithEvents cboPMLookupList As System.Windows.Forms.ComboBox
	Public WithEvents cboGISListId As System.Windows.Forms.ComboBox
	Public WithEvents cboProductId As System.Windows.Forms.ComboBox
	Public WithEvents cboGISUserDefHeaderId As System.Windows.Forms.ComboBox
	Public WithEvents cboPMUSumInsuredType As System.Windows.Forms.ComboBox
	Public WithEvents cboPartyTypeId As System.Windows.Forms.ComboBox
	Public WithEvents cboDocumentFilter As System.Windows.Forms.ComboBox
	Private WithEvents _optFieldType_15 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_14 As System.Windows.Forms.RadioButton
	Public WithEvents chkIsMandatory As System.Windows.Forms.CheckBox
	Private WithEvents _optFieldType_9 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_8 As System.Windows.Forms.RadioButton
	Public WithEvents txtPropertyName As System.Windows.Forms.TextBox
	Public WithEvents txtColumnName As System.Windows.Forms.TextBox
	Public WithEvents cboDataType As System.Windows.Forms.ComboBox
	Public WithEvents chkIsInputProperty As System.Windows.Forms.CheckBox
	Public WithEvents chkIsIdentifyingProperty As System.Windows.Forms.CheckBox
	Public WithEvents chkIsPrimaryKey As System.Windows.Forms.CheckBox
	Public WithEvents chkIsDeleted As System.Windows.Forms.CheckBox
	Public WithEvents chkIsSearchProperty As System.Windows.Forms.CheckBox
	Private WithEvents _optFieldType_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_2 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_3 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_4 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_6 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_5 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_0 As System.Windows.Forms.RadioButton
	Public WithEvents txtPolarisPropertyId As System.Windows.Forms.TextBox
	Private WithEvents _optFieldType_7 As System.Windows.Forms.RadioButton
	Public WithEvents cboIndexLinking As System.Windows.Forms.ComboBox
	Public WithEvents lblProductId As System.Windows.Forms.Label
	Public WithEvents lblGISUserDefHeaderId As System.Windows.Forms.Label
	Public WithEvents lblPMUSumInsuredType As System.Windows.Forms.Label
	Public WithEvents lblPartyTypeId As System.Windows.Forms.Label
	Public WithEvents lblPMLookupTableName As System.Windows.Forms.Label
	Public WithEvents lblGISListId As System.Windows.Forms.Label
	Public WithEvents lblDocumentFilter As System.Windows.Forms.Label
	Public WithEvents lblPropertyName As System.Windows.Forms.Label
	Public WithEvents lblColumnName As System.Windows.Forms.Label
	Public WithEvents lblDataType As System.Windows.Forms.Label
	Public WithEvents lblPolarisPropertyId As System.Windows.Forms.Label
	Public WithEvents lblIndexLinking As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents lblComboLookupTableName As System.Windows.Forms.Label
	Public WithEvents lblCommonCodeType As System.Windows.Forms.Label
	Public WithEvents lblSwiftListViewType As System.Windows.Forms.Label
	Private WithEvents _optFieldType_11 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_12 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_10 As System.Windows.Forms.RadioButton
	Public WithEvents cboCommonCode As System.Windows.Forms.ComboBox
	Private WithEvents _optFieldType_13 As System.Windows.Forms.RadioButton
	Public WithEvents cboSpecialsSelector As System.Windows.Forms.ComboBox
	Private WithEvents _optFieldType_16 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_17 As System.Windows.Forms.RadioButton
	Private WithEvents _optFieldType_18 As System.Windows.Forms.RadioButton
	Public WithEvents txtComboLookupTableName As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public optFieldType(20) As System.Windows.Forms.RadioButton
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraGeneral = New System.Windows.Forms.GroupBox()
        Me.chkIsChaseCycleProperty = New System.Windows.Forms.CheckBox()
        Me.chkIsFormattedText = New System.Windows.Forms.CheckBox()
        Me._optFieldType_20 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_19 = New System.Windows.Forms.RadioButton()
        Me.chkIsInMISExport = New System.Windows.Forms.CheckBox()
        Me.cboPMLookupList = New System.Windows.Forms.ComboBox()
        Me.cboGISListId = New System.Windows.Forms.ComboBox()
        Me.cboProductId = New System.Windows.Forms.ComboBox()
        Me.cboGISUserDefHeaderId = New System.Windows.Forms.ComboBox()
        Me.cboPMUSumInsuredType = New System.Windows.Forms.ComboBox()
        Me.cboPartyTypeId = New System.Windows.Forms.ComboBox()
        Me.cboDocumentFilter = New System.Windows.Forms.ComboBox()
        Me._optFieldType_15 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_14 = New System.Windows.Forms.RadioButton()
        Me.chkIsMandatory = New System.Windows.Forms.CheckBox()
        Me._optFieldType_9 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_8 = New System.Windows.Forms.RadioButton()
        Me.txtPropertyName = New System.Windows.Forms.TextBox()
        Me.txtColumnName = New System.Windows.Forms.TextBox()
        Me.cboDataType = New System.Windows.Forms.ComboBox()
        Me.chkIsInputProperty = New System.Windows.Forms.CheckBox()
        Me.chkIsIdentifyingProperty = New System.Windows.Forms.CheckBox()
        Me.chkIsPrimaryKey = New System.Windows.Forms.CheckBox()
        Me.chkIsDeleted = New System.Windows.Forms.CheckBox()
        Me.chkIsSearchProperty = New System.Windows.Forms.CheckBox()
        Me._optFieldType_1 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_2 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_3 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_4 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_6 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_5 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_0 = New System.Windows.Forms.RadioButton()
        Me.txtPolarisPropertyId = New System.Windows.Forms.TextBox()
        Me._optFieldType_7 = New System.Windows.Forms.RadioButton()
        Me.cboIndexLinking = New System.Windows.Forms.ComboBox()
        Me.lblProductId = New System.Windows.Forms.Label()
        Me.lblGISUserDefHeaderId = New System.Windows.Forms.Label()
        Me.lblPMUSumInsuredType = New System.Windows.Forms.Label()
        Me.lblPartyTypeId = New System.Windows.Forms.Label()
        Me.lblPMLookupTableName = New System.Windows.Forms.Label()
        Me.lblGISListId = New System.Windows.Forms.Label()
        Me.lblDocumentFilter = New System.Windows.Forms.Label()
        Me.lblPropertyName = New System.Windows.Forms.Label()
        Me.lblColumnName = New System.Windows.Forms.Label()
        Me.lblDataType = New System.Windows.Forms.Label()
        Me.lblPolarisPropertyId = New System.Windows.Forms.Label()
        Me.lblIndexLinking = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblComboLookupTableName = New System.Windows.Forms.Label()
        Me.lblCommonCodeType = New System.Windows.Forms.Label()
        Me.lblSwiftListViewType = New System.Windows.Forms.Label()
        Me._optFieldType_11 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_12 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_10 = New System.Windows.Forms.RadioButton()
        Me.cboCommonCode = New System.Windows.Forms.ComboBox()
        Me._optFieldType_13 = New System.Windows.Forms.RadioButton()
        Me.cboSpecialsSelector = New System.Windows.Forms.ComboBox()
        Me._optFieldType_16 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_17 = New System.Windows.Forms.RadioButton()
        Me._optFieldType_18 = New System.Windows.Forms.RadioButton()
        Me.txtComboLookupTableName = New System.Windows.Forms.TextBox()
        Me.chkIsClaim360Display = New System.Windows.Forms.CheckBox()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraGeneral.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 548)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 58
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
        Me.cmdHelp.Location = New System.Drawing.Point(472, 548)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 61
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
        Me.cmdCancel.Location = New System.Drawing.Point(392, 548)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 60
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
        Me.cmdOK.Location = New System.Drawing.Point(312, 548)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 59
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(106, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 9)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(541, 536)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraGeneral)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(533, 510)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - General"
        '
        'fraGeneral
        '
        Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.fraGeneral.Controls.Add(Me.chkIsClaim360Display)
        Me.fraGeneral.Controls.Add(Me.chkIsChaseCycleProperty)
        Me.fraGeneral.Controls.Add(Me.chkIsFormattedText)
        Me.fraGeneral.Controls.Add(Me._optFieldType_20)
        Me.fraGeneral.Controls.Add(Me._optFieldType_19)
        Me.fraGeneral.Controls.Add(Me.chkIsInMISExport)
        Me.fraGeneral.Controls.Add(Me.cboPMLookupList)
        Me.fraGeneral.Controls.Add(Me.cboGISListId)
        Me.fraGeneral.Controls.Add(Me.cboProductId)
        Me.fraGeneral.Controls.Add(Me.cboGISUserDefHeaderId)
        Me.fraGeneral.Controls.Add(Me.cboPMUSumInsuredType)
        Me.fraGeneral.Controls.Add(Me.cboPartyTypeId)
        Me.fraGeneral.Controls.Add(Me.cboDocumentFilter)
        Me.fraGeneral.Controls.Add(Me._optFieldType_15)
        Me.fraGeneral.Controls.Add(Me._optFieldType_14)
        Me.fraGeneral.Controls.Add(Me.chkIsMandatory)
        Me.fraGeneral.Controls.Add(Me._optFieldType_9)
        Me.fraGeneral.Controls.Add(Me._optFieldType_8)
        Me.fraGeneral.Controls.Add(Me.txtPropertyName)
        Me.fraGeneral.Controls.Add(Me.txtColumnName)
        Me.fraGeneral.Controls.Add(Me.cboDataType)
        Me.fraGeneral.Controls.Add(Me.chkIsInputProperty)
        Me.fraGeneral.Controls.Add(Me.chkIsIdentifyingProperty)
        Me.fraGeneral.Controls.Add(Me.chkIsPrimaryKey)
        Me.fraGeneral.Controls.Add(Me.chkIsDeleted)
        Me.fraGeneral.Controls.Add(Me.chkIsSearchProperty)
        Me.fraGeneral.Controls.Add(Me._optFieldType_1)
        Me.fraGeneral.Controls.Add(Me._optFieldType_2)
        Me.fraGeneral.Controls.Add(Me._optFieldType_3)
        Me.fraGeneral.Controls.Add(Me._optFieldType_4)
        Me.fraGeneral.Controls.Add(Me._optFieldType_6)
        Me.fraGeneral.Controls.Add(Me._optFieldType_5)
        Me.fraGeneral.Controls.Add(Me._optFieldType_0)
        Me.fraGeneral.Controls.Add(Me.txtPolarisPropertyId)
        Me.fraGeneral.Controls.Add(Me._optFieldType_7)
        Me.fraGeneral.Controls.Add(Me.cboIndexLinking)
        Me.fraGeneral.Controls.Add(Me.lblProductId)
        Me.fraGeneral.Controls.Add(Me.lblGISUserDefHeaderId)
        Me.fraGeneral.Controls.Add(Me.lblPMUSumInsuredType)
        Me.fraGeneral.Controls.Add(Me.lblPartyTypeId)
        Me.fraGeneral.Controls.Add(Me.lblPMLookupTableName)
        Me.fraGeneral.Controls.Add(Me.lblGISListId)
        Me.fraGeneral.Controls.Add(Me.lblDocumentFilter)
        Me.fraGeneral.Controls.Add(Me.lblPropertyName)
        Me.fraGeneral.Controls.Add(Me.lblColumnName)
        Me.fraGeneral.Controls.Add(Me.lblDataType)
        Me.fraGeneral.Controls.Add(Me.lblPolarisPropertyId)
        Me.fraGeneral.Controls.Add(Me.lblIndexLinking)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(8, 4)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(521, 498)
        Me.fraGeneral.TabIndex = 14
        Me.fraGeneral.TabStop = False
        '
        'chkIsChaseCycleProperty
        '
        Me.chkIsChaseCycleProperty.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsChaseCycleProperty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsChaseCycleProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsChaseCycleProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsChaseCycleProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsChaseCycleProperty.Location = New System.Drawing.Point(344, 173)
        Me.chkIsChaseCycleProperty.Name = "chkIsChaseCycleProperty"
        Me.chkIsChaseCycleProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsChaseCycleProperty.Size = New System.Drawing.Size(161, 17)
        Me.chkIsChaseCycleProperty.TabIndex = 65
        Me.chkIsChaseCycleProperty.Text = "Is Chase Cycle Property"
        Me.chkIsChaseCycleProperty.UseVisualStyleBackColor = False
        '
        'chkIsFormattedText
        '
        Me.chkIsFormattedText.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsFormattedText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsFormattedText.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsFormattedText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsFormattedText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsFormattedText.Location = New System.Drawing.Point(345, 156)
        Me.chkIsFormattedText.Name = "chkIsFormattedText"
        Me.chkIsFormattedText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsFormattedText.Size = New System.Drawing.Size(161, 17)
        Me.chkIsFormattedText.TabIndex = 64
        Me.chkIsFormattedText.Text = "Is Formatted Text?"
        Me.chkIsFormattedText.UseVisualStyleBackColor = False
        '
        '_optFieldType_20
        '
        Me._optFieldType_20.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_20.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_20.Enabled = False
        Me._optFieldType_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_20.Location = New System.Drawing.Point(17, 446)
        Me._optFieldType_20.Name = "_optFieldType_20"
        Me._optFieldType_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_20.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_20.TabIndex = 63
        Me._optFieldType_20.TabStop = True
        Me._optFieldType_20.Text = "Case Claim Links"
        Me._optFieldType_20.UseVisualStyleBackColor = False
        Me._optFieldType_20.Visible = False
        '
        '_optFieldType_19
        '
        Me._optFieldType_19.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_19.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_19.Enabled = False
        Me._optFieldType_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_19.Location = New System.Drawing.Point(17, 425)
        Me._optFieldType_19.Name = "_optFieldType_19"
        Me._optFieldType_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_19.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_19.TabIndex = 62
        Me._optFieldType_19.TabStop = True
        Me._optFieldType_19.Text = "Case Header"
        Me._optFieldType_19.UseVisualStyleBackColor = False
        Me._optFieldType_19.Visible = False
        '
        'chkIsInMISExport
        '
        Me.chkIsInMISExport.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsInMISExport.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsInMISExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsInMISExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsInMISExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsInMISExport.Location = New System.Drawing.Point(345, 136)
        Me.chkIsInMISExport.Name = "chkIsInMISExport"
        Me.chkIsInMISExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsInMISExport.Size = New System.Drawing.Size(161, 17)
        Me.chkIsInMISExport.TabIndex = 29
        Me.chkIsInMISExport.Text = "Is Export Property?"
        Me.chkIsInMISExport.UseVisualStyleBackColor = False
        '
        'cboPMLookupList
        '
        Me.cboPMLookupList.BackColor = System.Drawing.SystemColors.Window
        Me.cboPMLookupList.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPMLookupList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPMLookupList.Enabled = False
        Me.cboPMLookupList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPMLookupList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPMLookupList.Location = New System.Drawing.Point(345, 217)
        Me.cboPMLookupList.Name = "cboPMLookupList"
        Me.cboPMLookupList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPMLookupList.Size = New System.Drawing.Size(161, 21)
        Me.cboPMLookupList.Sorted = True
        Me.cboPMLookupList.TabIndex = 35
        '
        'cboGISListId
        '
        Me.cboGISListId.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISListId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISListId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGISListId.Enabled = False
        Me.cboGISListId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGISListId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISListId.Location = New System.Drawing.Point(345, 193)
        Me.cboGISListId.Name = "cboGISListId"
        Me.cboGISListId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISListId.Size = New System.Drawing.Size(161, 21)
        Me.cboGISListId.Sorted = True
        Me.cboGISListId.TabIndex = 32
        '
        'cboProductId
        '
        Me.cboProductId.BackColor = System.Drawing.SystemColors.Window
        Me.cboProductId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProductId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProductId.Enabled = False
        Me.cboProductId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProductId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProductId.Location = New System.Drawing.Point(345, 338)
        Me.cboProductId.Name = "cboProductId"
        Me.cboProductId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProductId.Size = New System.Drawing.Size(161, 21)
        Me.cboProductId.Sorted = True
        Me.cboProductId.TabIndex = 50
        '
        'cboGISUserDefHeaderId
        '
        Me.cboGISUserDefHeaderId.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISUserDefHeaderId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISUserDefHeaderId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGISUserDefHeaderId.Enabled = False
        Me.cboGISUserDefHeaderId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGISUserDefHeaderId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISUserDefHeaderId.Location = New System.Drawing.Point(345, 312)
        Me.cboGISUserDefHeaderId.Name = "cboGISUserDefHeaderId"
        Me.cboGISUserDefHeaderId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISUserDefHeaderId.Size = New System.Drawing.Size(161, 21)
        Me.cboGISUserDefHeaderId.Sorted = True
        Me.cboGISUserDefHeaderId.TabIndex = 47
        '
        'cboPMUSumInsuredType
        '
        Me.cboPMUSumInsuredType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPMUSumInsuredType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPMUSumInsuredType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPMUSumInsuredType.Enabled = False
        Me.cboPMUSumInsuredType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPMUSumInsuredType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPMUSumInsuredType.Location = New System.Drawing.Point(345, 265)
        Me.cboPMUSumInsuredType.Name = "cboPMUSumInsuredType"
        Me.cboPMUSumInsuredType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPMUSumInsuredType.Size = New System.Drawing.Size(161, 21)
        Me.cboPMUSumInsuredType.Sorted = True
        Me.cboPMUSumInsuredType.TabIndex = 41
        '
        'cboPartyTypeId
        '
        Me.cboPartyTypeId.BackColor = System.Drawing.SystemColors.Window
        Me.cboPartyTypeId.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPartyTypeId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPartyTypeId.Enabled = False
        Me.cboPartyTypeId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPartyTypeId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPartyTypeId.Location = New System.Drawing.Point(345, 241)
        Me.cboPartyTypeId.Name = "cboPartyTypeId"
        Me.cboPartyTypeId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPartyTypeId.Size = New System.Drawing.Size(161, 21)
        Me.cboPartyTypeId.Sorted = True
        Me.cboPartyTypeId.TabIndex = 38
        '
        'cboDocumentFilter
        '
        Me.cboDocumentFilter.BackColor = System.Drawing.SystemColors.Window
        Me.cboDocumentFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDocumentFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocumentFilter.Enabled = False
        Me.cboDocumentFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDocumentFilter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDocumentFilter.Location = New System.Drawing.Point(345, 289)
        Me.cboDocumentFilter.Name = "cboDocumentFilter"
        Me.cboDocumentFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDocumentFilter.Size = New System.Drawing.Size(161, 21)
        Me.cboDocumentFilter.Sorted = True
        Me.cboDocumentFilter.TabIndex = 44
        '
        '_optFieldType_15
        '
        Me._optFieldType_15.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_15.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_15.Enabled = False
        Me._optFieldType_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_15.Location = New System.Drawing.Point(344, 439)
        Me._optFieldType_15.Name = "_optFieldType_15"
        Me._optFieldType_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_15.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_15.TabIndex = 55
        Me._optFieldType_15.TabStop = True
        Me._optFieldType_15.Text = "Documaster???"
        Me._optFieldType_15.UseVisualStyleBackColor = False
        Me._optFieldType_15.Visible = False
        '
        '_optFieldType_14
        '
        Me._optFieldType_14.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_14.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_14.Enabled = False
        Me._optFieldType_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_14.Location = New System.Drawing.Point(17, 404)
        Me._optFieldType_14.Name = "_optFieldType_14"
        Me._optFieldType_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_14.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_14.TabIndex = 54
        Me._optFieldType_14.TabStop = True
        Me._optFieldType_14.Text = "Swift Special"
        Me._optFieldType_14.UseVisualStyleBackColor = False
        Me._optFieldType_14.Visible = False
        '
        'chkIsMandatory
        '
        Me.chkIsMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsMandatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsMandatory.Location = New System.Drawing.Point(345, 116)
        Me.chkIsMandatory.Name = "chkIsMandatory"
        Me.chkIsMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsMandatory.Size = New System.Drawing.Size(161, 17)
        Me.chkIsMandatory.TabIndex = 28
        Me.chkIsMandatory.Text = "Is Mandatory?"
        Me.chkIsMandatory.UseVisualStyleBackColor = False
        '
        '_optFieldType_9
        '
        Me._optFieldType_9.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_9.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_9.Enabled = False
        Me._optFieldType_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_9.Location = New System.Drawing.Point(17, 382)
        Me._optFieldType_9.Name = "_optFieldType_9"
        Me._optFieldType_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_9.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_9.TabIndex = 53
        Me._optFieldType_9.TabStop = True
        Me._optFieldType_9.Text = "Claim Payment"
        Me._optFieldType_9.UseVisualStyleBackColor = False
        '
        '_optFieldType_8
        '
        Me._optFieldType_8.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_8.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_8.Enabled = False
        Me._optFieldType_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_8.Location = New System.Drawing.Point(16, 361)
        Me._optFieldType_8.Name = "_optFieldType_8"
        Me._optFieldType_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_8.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_8.TabIndex = 52
        Me._optFieldType_8.TabStop = True
        Me._optFieldType_8.Text = "Claim Reserve"
        Me._optFieldType_8.UseVisualStyleBackColor = False
        '
        'txtPropertyName
        '
        Me.txtPropertyName.AcceptsReturn = True
        Me.txtPropertyName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPropertyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPropertyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPropertyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPropertyName.Location = New System.Drawing.Point(144, 16)
        Me.txtPropertyName.MaxLength = 70
        Me.txtPropertyName.Name = "txtPropertyName"
        Me.txtPropertyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPropertyName.Size = New System.Drawing.Size(177, 20)
        Me.txtPropertyName.TabIndex = 15
        '
        'txtColumnName
        '
        Me.txtColumnName.AcceptsReturn = True
        Me.txtColumnName.BackColor = System.Drawing.SystemColors.Window
        Me.txtColumnName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtColumnName.Enabled = False
        Me.txtColumnName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColumnName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtColumnName.Location = New System.Drawing.Point(144, 40)
        Me.txtColumnName.MaxLength = 70
        Me.txtColumnName.Name = "txtColumnName"
        Me.txtColumnName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtColumnName.Size = New System.Drawing.Size(177, 20)
        Me.txtColumnName.TabIndex = 18
        '
        'cboDataType
        '
        Me.cboDataType.BackColor = System.Drawing.SystemColors.Window
        Me.cboDataType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDataType.Enabled = False
        Me.cboDataType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDataType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDataType.Location = New System.Drawing.Point(144, 64)
        Me.cboDataType.Name = "cboDataType"
        Me.cboDataType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDataType.Size = New System.Drawing.Size(177, 21)
        Me.cboDataType.TabIndex = 21
        '
        'chkIsInputProperty
        '
        Me.chkIsInputProperty.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsInputProperty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsInputProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsInputProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsInputProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsInputProperty.Location = New System.Drawing.Point(345, 16)
        Me.chkIsInputProperty.Name = "chkIsInputProperty"
        Me.chkIsInputProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsInputProperty.Size = New System.Drawing.Size(161, 17)
        Me.chkIsInputProperty.TabIndex = 17
        Me.chkIsInputProperty.Text = "Is Input Property?"
        Me.chkIsInputProperty.UseVisualStyleBackColor = False
        '
        'chkIsIdentifyingProperty
        '
        Me.chkIsIdentifyingProperty.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsIdentifyingProperty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsIdentifyingProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsIdentifyingProperty.Enabled = False
        Me.chkIsIdentifyingProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsIdentifyingProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsIdentifyingProperty.Location = New System.Drawing.Point(345, 36)
        Me.chkIsIdentifyingProperty.Name = "chkIsIdentifyingProperty"
        Me.chkIsIdentifyingProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsIdentifyingProperty.Size = New System.Drawing.Size(161, 17)
        Me.chkIsIdentifyingProperty.TabIndex = 20
        Me.chkIsIdentifyingProperty.Text = "Is Identifier?"
        Me.chkIsIdentifyingProperty.UseVisualStyleBackColor = False
        '
        'chkIsPrimaryKey
        '
        Me.chkIsPrimaryKey.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsPrimaryKey.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsPrimaryKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsPrimaryKey.Enabled = False
        Me.chkIsPrimaryKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsPrimaryKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsPrimaryKey.Location = New System.Drawing.Point(345, 56)
        Me.chkIsPrimaryKey.Name = "chkIsPrimaryKey"
        Me.chkIsPrimaryKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsPrimaryKey.Size = New System.Drawing.Size(161, 17)
        Me.chkIsPrimaryKey.TabIndex = 23
        Me.chkIsPrimaryKey.Text = "Is Primary Key?"
        Me.chkIsPrimaryKey.UseVisualStyleBackColor = False
        '
        'chkIsDeleted
        '
        Me.chkIsDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDeleted.Enabled = False
        Me.chkIsDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDeleted.Location = New System.Drawing.Point(345, 76)
        Me.chkIsDeleted.Name = "chkIsDeleted"
        Me.chkIsDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDeleted.Size = New System.Drawing.Size(161, 17)
        Me.chkIsDeleted.TabIndex = 26
        Me.chkIsDeleted.Text = "Is Deleted?"
        Me.chkIsDeleted.UseVisualStyleBackColor = False
        '
        'chkIsSearchProperty
        '
        Me.chkIsSearchProperty.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSearchProperty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSearchProperty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSearchProperty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSearchProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSearchProperty.Location = New System.Drawing.Point(345, 96)
        Me.chkIsSearchProperty.Name = "chkIsSearchProperty"
        Me.chkIsSearchProperty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSearchProperty.Size = New System.Drawing.Size(161, 17)
        Me.chkIsSearchProperty.TabIndex = 27
        Me.chkIsSearchProperty.Text = "Is Search Property?"
        Me.chkIsSearchProperty.UseVisualStyleBackColor = False
        '
        '_optFieldType_1
        '
        Me._optFieldType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_1.Enabled = False
        Me._optFieldType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_1.Location = New System.Drawing.Point(16, 196)
        Me._optFieldType_1.Name = "_optFieldType_1"
        Me._optFieldType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_1.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_1.TabIndex = 31
        Me._optFieldType_1.TabStop = True
        Me._optFieldType_1.Text = "GIS List"
        Me._optFieldType_1.UseVisualStyleBackColor = False
        '
        '_optFieldType_2
        '
        Me._optFieldType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_2.Enabled = False
        Me._optFieldType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_2.Location = New System.Drawing.Point(16, 221)
        Me._optFieldType_2.Name = "_optFieldType_2"
        Me._optFieldType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_2.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_2.TabIndex = 34
        Me._optFieldType_2.TabStop = True
        Me._optFieldType_2.Text = "PM Lookup"
        Me._optFieldType_2.UseVisualStyleBackColor = False
        '
        '_optFieldType_3
        '
        Me._optFieldType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_3.Enabled = False
        Me._optFieldType_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_3.Location = New System.Drawing.Point(16, 243)
        Me._optFieldType_3.Name = "_optFieldType_3"
        Me._optFieldType_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_3.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_3.TabIndex = 37
        Me._optFieldType_3.TabStop = True
        Me._optFieldType_3.Text = "Party"
        Me._optFieldType_3.UseVisualStyleBackColor = False
        '
        '_optFieldType_4
        '
        Me._optFieldType_4.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_4.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_4.Enabled = False
        Me._optFieldType_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_4.Location = New System.Drawing.Point(16, 267)
        Me._optFieldType_4.Name = "_optFieldType_4"
        Me._optFieldType_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_4.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_4.TabIndex = 40
        Me._optFieldType_4.TabStop = True
        Me._optFieldType_4.Text = "Sum Insured"
        Me._optFieldType_4.UseVisualStyleBackColor = False
        '
        '_optFieldType_6
        '
        Me._optFieldType_6.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_6.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_6.Enabled = False
        Me._optFieldType_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_6.Location = New System.Drawing.Point(16, 315)
        Me._optFieldType_6.Name = "_optFieldType_6"
        Me._optFieldType_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_6.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_6.TabIndex = 46
        Me._optFieldType_6.TabStop = True
        Me._optFieldType_6.Text = "User Defined"
        Me._optFieldType_6.UseVisualStyleBackColor = False
        '
        '_optFieldType_5
        '
        Me._optFieldType_5.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_5.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_5.Enabled = False
        Me._optFieldType_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_5.Location = New System.Drawing.Point(16, 291)
        Me._optFieldType_5.Name = "_optFieldType_5"
        Me._optFieldType_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_5.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_5.TabIndex = 43
        Me._optFieldType_5.TabStop = True
        Me._optFieldType_5.Text = "Standard Wording"
        Me._optFieldType_5.UseVisualStyleBackColor = False
        '
        '_optFieldType_0
        '
        Me._optFieldType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_0.Enabled = False
        Me._optFieldType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_0.Location = New System.Drawing.Point(16, 172)
        Me._optFieldType_0.Name = "_optFieldType_0"
        Me._optFieldType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_0.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_0.TabIndex = 30
        Me._optFieldType_0.TabStop = True
        Me._optFieldType_0.Text = "Normal"
        Me._optFieldType_0.UseVisualStyleBackColor = False
        '
        'txtPolarisPropertyId
        '
        Me.txtPolarisPropertyId.AcceptsReturn = True
        Me.txtPolarisPropertyId.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolarisPropertyId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolarisPropertyId.Enabled = False
        Me.txtPolarisPropertyId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolarisPropertyId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolarisPropertyId.Location = New System.Drawing.Point(144, 88)
        Me.txtPolarisPropertyId.MaxLength = 70
        Me.txtPolarisPropertyId.Name = "txtPolarisPropertyId"
        Me.txtPolarisPropertyId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolarisPropertyId.Size = New System.Drawing.Size(81, 20)
        Me.txtPolarisPropertyId.TabIndex = 24
        '
        '_optFieldType_7
        '
        Me._optFieldType_7.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_7.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_7.Enabled = False
        Me._optFieldType_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_7.Location = New System.Drawing.Point(16, 339)
        Me._optFieldType_7.Name = "_optFieldType_7"
        Me._optFieldType_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_7.Size = New System.Drawing.Size(153, 17)
        Me._optFieldType_7.TabIndex = 49
        Me._optFieldType_7.TabStop = True
        Me._optFieldType_7.Text = "Policy"
        Me._optFieldType_7.UseVisualStyleBackColor = False
        '
        'cboIndexLinking
        '
        Me.cboIndexLinking.BackColor = System.Drawing.SystemColors.Window
        Me.cboIndexLinking.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboIndexLinking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIndexLinking.Enabled = False
        Me.cboIndexLinking.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboIndexLinking.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboIndexLinking.Location = New System.Drawing.Point(156, 468)
        Me.cboIndexLinking.Name = "cboIndexLinking"
        Me.cboIndexLinking.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboIndexLinking.Size = New System.Drawing.Size(153, 21)
        Me.cboIndexLinking.TabIndex = 56
        '
        'lblProductId
        '
        Me.lblProductId.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductId.Location = New System.Drawing.Point(209, 342)
        Me.lblProductId.Name = "lblProductId"
        Me.lblProductId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductId.Size = New System.Drawing.Size(137, 17)
        Me.lblProductId.TabIndex = 51
        Me.lblProductId.Text = "Product Id:"
        '
        'lblGISUserDefHeaderId
        '
        Me.lblGISUserDefHeaderId.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISUserDefHeaderId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISUserDefHeaderId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGISUserDefHeaderId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISUserDefHeaderId.Location = New System.Drawing.Point(209, 317)
        Me.lblGISUserDefHeaderId.Name = "lblGISUserDefHeaderId"
        Me.lblGISUserDefHeaderId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISUserDefHeaderId.Size = New System.Drawing.Size(137, 17)
        Me.lblGISUserDefHeaderId.TabIndex = 48
        Me.lblGISUserDefHeaderId.Text = "User Defined Table:"
        '
        'lblPMUSumInsuredType
        '
        Me.lblPMUSumInsuredType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMUSumInsuredType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMUSumInsuredType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMUSumInsuredType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMUSumInsuredType.Location = New System.Drawing.Point(209, 268)
        Me.lblPMUSumInsuredType.Name = "lblPMUSumInsuredType"
        Me.lblPMUSumInsuredType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMUSumInsuredType.Size = New System.Drawing.Size(137, 17)
        Me.lblPMUSumInsuredType.TabIndex = 42
        Me.lblPMUSumInsuredType.Text = "Sum Insured Type:"
        '
        'lblPartyTypeId
        '
        Me.lblPartyTypeId.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyTypeId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyTypeId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyTypeId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyTypeId.Location = New System.Drawing.Point(209, 244)
        Me.lblPartyTypeId.Name = "lblPartyTypeId"
        Me.lblPartyTypeId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyTypeId.Size = New System.Drawing.Size(137, 17)
        Me.lblPartyTypeId.TabIndex = 39
        Me.lblPartyTypeId.Text = "Party Type:"
        '
        'lblPMLookupTableName
        '
        Me.lblPMLookupTableName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMLookupTableName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMLookupTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMLookupTableName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMLookupTableName.Location = New System.Drawing.Point(209, 220)
        Me.lblPMLookupTableName.Name = "lblPMLookupTableName"
        Me.lblPMLookupTableName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMLookupTableName.Size = New System.Drawing.Size(137, 17)
        Me.lblPMLookupTableName.TabIndex = 36
        Me.lblPMLookupTableName.Text = "Lookup Table Name:"
        '
        'lblGISListId
        '
        Me.lblGISListId.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISListId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISListId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGISListId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISListId.Location = New System.Drawing.Point(209, 197)
        Me.lblGISListId.Name = "lblGISListId"
        Me.lblGISListId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISListId.Size = New System.Drawing.Size(137, 17)
        Me.lblGISListId.TabIndex = 33
        Me.lblGISListId.Text = "GIS List Id:"
        '
        'lblDocumentFilter
        '
        Me.lblDocumentFilter.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocumentFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocumentFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumentFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocumentFilter.Location = New System.Drawing.Point(209, 292)
        Me.lblDocumentFilter.Name = "lblDocumentFilter"
        Me.lblDocumentFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocumentFilter.Size = New System.Drawing.Size(137, 17)
        Me.lblDocumentFilter.TabIndex = 45
        Me.lblDocumentFilter.Text = "Document Filter:"
        '
        'lblPropertyName
        '
        Me.lblPropertyName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPropertyName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPropertyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPropertyName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPropertyName.Location = New System.Drawing.Point(16, 19)
        Me.lblPropertyName.Name = "lblPropertyName"
        Me.lblPropertyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPropertyName.Size = New System.Drawing.Size(145, 17)
        Me.lblPropertyName.TabIndex = 16
        Me.lblPropertyName.Text = "Property Name:"
        '
        'lblColumnName
        '
        Me.lblColumnName.BackColor = System.Drawing.SystemColors.Control
        Me.lblColumnName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblColumnName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblColumnName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblColumnName.Location = New System.Drawing.Point(16, 43)
        Me.lblColumnName.Name = "lblColumnName"
        Me.lblColumnName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblColumnName.Size = New System.Drawing.Size(145, 17)
        Me.lblColumnName.TabIndex = 19
        Me.lblColumnName.Text = "Column Name:"
        '
        'lblDataType
        '
        Me.lblDataType.BackColor = System.Drawing.SystemColors.Control
        Me.lblDataType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDataType.Location = New System.Drawing.Point(16, 67)
        Me.lblDataType.Name = "lblDataType"
        Me.lblDataType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataType.Size = New System.Drawing.Size(145, 17)
        Me.lblDataType.TabIndex = 22
        Me.lblDataType.Text = "Data Type:"
        '
        'lblPolarisPropertyId
        '
        Me.lblPolarisPropertyId.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolarisPropertyId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolarisPropertyId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolarisPropertyId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolarisPropertyId.Location = New System.Drawing.Point(16, 91)
        Me.lblPolarisPropertyId.Name = "lblPolarisPropertyId"
        Me.lblPolarisPropertyId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolarisPropertyId.Size = New System.Drawing.Size(145, 17)
        Me.lblPolarisPropertyId.TabIndex = 25
        Me.lblPolarisPropertyId.Text = "Polaris Property:"
        '
        'lblIndexLinking
        '
        Me.lblIndexLinking.BackColor = System.Drawing.SystemColors.Control
        Me.lblIndexLinking.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIndexLinking.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIndexLinking.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIndexLinking.Location = New System.Drawing.Point(18, 472)
        Me.lblIndexLinking.Name = "lblIndexLinking"
        Me.lblIndexLinking.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndexLinking.Size = New System.Drawing.Size(137, 17)
        Me.lblIndexLinking.TabIndex = 57
        Me.lblIndexLinking.Text = "Index Linking:"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblComboLookupTableName)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblCommonCodeType)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblSwiftListViewType)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_11)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_12)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_10)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cboCommonCode)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_13)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cboSpecialsSelector)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_16)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_17)
        Me._tabMainTab_TabPage1.Controls.Add(Me._optFieldType_18)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtComboLookupTableName)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(533, 510)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Swift Specials"
        '
        'lblComboLookupTableName
        '
        Me.lblComboLookupTableName.BackColor = System.Drawing.SystemColors.Control
        Me.lblComboLookupTableName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComboLookupTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComboLookupTableName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComboLookupTableName.Location = New System.Drawing.Point(224, 287)
        Me.lblComboLookupTableName.Name = "lblComboLookupTableName"
        Me.lblComboLookupTableName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComboLookupTableName.Size = New System.Drawing.Size(128, 17)
        Me.lblComboLookupTableName.TabIndex = 13
        Me.lblComboLookupTableName.Text = "Lookup Table Name:"
        '
        'lblCommonCodeType
        '
        Me.lblCommonCodeType.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommonCodeType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommonCodeType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommonCodeType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommonCodeType.Location = New System.Drawing.Point(224, 76)
        Me.lblCommonCodeType.Name = "lblCommonCodeType"
        Me.lblCommonCodeType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommonCodeType.Size = New System.Drawing.Size(136, 17)
        Me.lblCommonCodeType.TabIndex = 2
        Me.lblCommonCodeType.Text = "Code Type:"
        '
        'lblSwiftListViewType
        '
        Me.lblSwiftListViewType.BackColor = System.Drawing.SystemColors.Control
        Me.lblSwiftListViewType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSwiftListViewType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwiftListViewType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSwiftListViewType.Location = New System.Drawing.Point(224, 220)
        Me.lblSwiftListViewType.Name = "lblSwiftListViewType"
        Me.lblSwiftListViewType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSwiftListViewType.Size = New System.Drawing.Size(136, 17)
        Me.lblSwiftListViewType.TabIndex = 8
        Me.lblSwiftListViewType.Text = "List Type:"
        '
        '_optFieldType_11
        '
        Me._optFieldType_11.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_11.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_11.Enabled = False
        Me._optFieldType_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_11.Location = New System.Drawing.Point(14, 109)
        Me._optFieldType_11.Name = "_optFieldType_11"
        Me._optFieldType_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_11.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_11.TabIndex = 4
        Me._optFieldType_11.TabStop = True
        Me._optFieldType_11.Text = "Client selector (button only)"
        Me._optFieldType_11.UseVisualStyleBackColor = False
        '
        '_optFieldType_12
        '
        Me._optFieldType_12.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_12.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_12.Enabled = False
        Me._optFieldType_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_12.Location = New System.Drawing.Point(14, 147)
        Me._optFieldType_12.Name = "_optFieldType_12"
        Me._optFieldType_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_12.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_12.TabIndex = 5
        Me._optFieldType_12.TabStop = True
        Me._optFieldType_12.Text = "Address selector (button only)"
        Me._optFieldType_12.UseVisualStyleBackColor = False
        '
        '_optFieldType_10
        '
        Me._optFieldType_10.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_10.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_10.Enabled = False
        Me._optFieldType_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_10.Location = New System.Drawing.Point(14, 76)
        Me._optFieldType_10.Name = "_optFieldType_10"
        Me._optFieldType_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_10.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_10.TabIndex = 1
        Me._optFieldType_10.TabStop = True
        Me._optFieldType_10.Text = "Common Code"
        Me._optFieldType_10.UseVisualStyleBackColor = False
        '
        'cboCommonCode
        '
        Me.cboCommonCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommonCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommonCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommonCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommonCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommonCode.Location = New System.Drawing.Point(368, 74)
        Me.cboCommonCode.Name = "cboCommonCode"
        Me.cboCommonCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommonCode.Size = New System.Drawing.Size(153, 21)
        Me.cboCommonCode.TabIndex = 3
        '
        '_optFieldType_13
        '
        Me._optFieldType_13.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_13.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_13.Enabled = False
        Me._optFieldType_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_13.Location = New System.Drawing.Point(14, 217)
        Me._optFieldType_13.Name = "_optFieldType_13"
        Me._optFieldType_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_13.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_13.TabIndex = 6
        Me._optFieldType_13.TabStop = True
        Me._optFieldType_13.Text = "List View"
        Me._optFieldType_13.UseVisualStyleBackColor = False
        '
        'cboSpecialsSelector
        '
        Me.cboSpecialsSelector.BackColor = System.Drawing.SystemColors.Window
        Me.cboSpecialsSelector.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSpecialsSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSpecialsSelector.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSpecialsSelector.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSpecialsSelector.Location = New System.Drawing.Point(368, 216)
        Me.cboSpecialsSelector.Name = "cboSpecialsSelector"
        Me.cboSpecialsSelector.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSpecialsSelector.Size = New System.Drawing.Size(153, 21)
        Me.cboSpecialsSelector.TabIndex = 7
        '
        '_optFieldType_16
        '
        Me._optFieldType_16.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_16.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_16.Enabled = False
        Me._optFieldType_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_16.Location = New System.Drawing.Point(14, 252)
        Me._optFieldType_16.Name = "_optFieldType_16"
        Me._optFieldType_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_16.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_16.TabIndex = 9
        Me._optFieldType_16.TabStop = True
        Me._optFieldType_16.Text = "Notes"
        Me._optFieldType_16.UseVisualStyleBackColor = False
        '
        '_optFieldType_17
        '
        Me._optFieldType_17.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_17.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_17.Enabled = False
        Me._optFieldType_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_17.Location = New System.Drawing.Point(14, 180)
        Me._optFieldType_17.Name = "_optFieldType_17"
        Me._optFieldType_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_17.Size = New System.Drawing.Size(196, 18)
        Me._optFieldType_17.TabIndex = 10
        Me._optFieldType_17.TabStop = True
        Me._optFieldType_17.Text = "Address selector (In Full)"
        Me._optFieldType_17.UseVisualStyleBackColor = False
        '
        '_optFieldType_18
        '
        Me._optFieldType_18.BackColor = System.Drawing.SystemColors.Control
        Me._optFieldType_18.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optFieldType_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFieldType_18.Enabled = False
        Me._optFieldType_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFieldType_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFieldType_18.Location = New System.Drawing.Point(16, 286)
        Me._optFieldType_18.Name = "_optFieldType_18"
        Me._optFieldType_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFieldType_18.Size = New System.Drawing.Size(193, 17)
        Me._optFieldType_18.TabIndex = 12
        Me._optFieldType_18.TabStop = True
        Me._optFieldType_18.Text = "Combo Lookup"
        Me._optFieldType_18.UseVisualStyleBackColor = False
        '
        'txtComboLookupTableName
        '
        Me.txtComboLookupTableName.AcceptsReturn = True
        Me.txtComboLookupTableName.BackColor = System.Drawing.SystemColors.Window
        Me.txtComboLookupTableName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComboLookupTableName.Enabled = False
        Me.txtComboLookupTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComboLookupTableName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComboLookupTableName.Location = New System.Drawing.Point(366, 284)
        Me.txtComboLookupTableName.MaxLength = 70
        Me.txtComboLookupTableName.Name = "txtComboLookupTableName"
        Me.txtComboLookupTableName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComboLookupTableName.Size = New System.Drawing.Size(153, 20)
        Me.txtComboLookupTableName.TabIndex = 11
        '
        'chkIsClaim360Display
        '
        Me.chkIsClaim360Display.AutoSize = True
        Me.chkIsClaim360Display.Location = New System.Drawing.Point(355, 405)
        Me.chkIsClaim360Display.MaximumSize = New System.Drawing.Size(200, 0)
        Me.chkIsClaim360Display.MinimumSize = New System.Drawing.Size(150, 0)
        Me.chkIsClaim360Display.Name = "chkIsClaim360Display"
        Me.chkIsClaim360Display.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkIsClaim360Display.Size = New System.Drawing.Size(150, 17)
        Me.chkIsClaim360Display.TabIndex = 66
        Me.chkIsClaim360Display.Text = "Claims 360 Display"
        Me.chkIsClaim360Display.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.chkIsClaim360Display.UseVisualStyleBackColor = True
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(553, 577)
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
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "GIS Property"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.fraGeneral.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptFieldType()
        Me.optFieldType(18) = _optFieldType_18
        Me.optFieldType(17) = _optFieldType_17
        Me.optFieldType(16) = _optFieldType_16
        Me.optFieldType(13) = _optFieldType_13
        Me.optFieldType(10) = _optFieldType_10
        Me.optFieldType(12) = _optFieldType_12
        Me.optFieldType(11) = _optFieldType_11
        Me.optFieldType(20) = _optFieldType_20
        Me.optFieldType(19) = _optFieldType_19
        Me.optFieldType(15) = _optFieldType_15
        Me.optFieldType(14) = _optFieldType_14
        Me.optFieldType(9) = _optFieldType_9
        Me.optFieldType(8) = _optFieldType_8
        Me.optFieldType(1) = _optFieldType_1
        Me.optFieldType(2) = _optFieldType_2
        Me.optFieldType(3) = _optFieldType_3
        Me.optFieldType(4) = _optFieldType_4
        Me.optFieldType(6) = _optFieldType_6
        Me.optFieldType(5) = _optFieldType_5
        Me.optFieldType(0) = _optFieldType_0
        Me.optFieldType(7) = _optFieldType_7
    End Sub
    Public WithEvents chkIsFormattedText As System.Windows.Forms.CheckBox
    Public WithEvents chkIsChaseCycleProperty As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsClaim360Display As System.Windows.Forms.CheckBox
#End Region 
End Class