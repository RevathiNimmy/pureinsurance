<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRIModel
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptRIModelType()
		InitializeoptFACPremiums()
		InitializeoptClaimAllocation()
		InitializeoptTreatyPremium()
		tabRIModelPreviousTab = tabRIModel.SelectedIndex
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblExpiryDate As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblRIModelType As System.Windows.Forms.Label
	Public WithEvents lblFACPremiums As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblClaimAllocation As System.Windows.Forms.Label
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents dtpExpiryDate As System.Windows.Forms.DateTimePicker
	Public WithEvents dtpEffectiveDate As System.Windows.Forms.DateTimePicker
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Private WithEvents _optRIModelType_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optRIModelType_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optRIModelType_2 As System.Windows.Forms.RadioButton
	Private WithEvents _optRIModelType_3 As System.Windows.Forms.RadioButton
	Public WithEvents Picture1 As System.Windows.Forms.PictureBox
	Private WithEvents _optFACPremiums_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optFACPremiums_1 As System.Windows.Forms.RadioButton
	Public WithEvents Picture2 As System.Windows.Forms.PictureBox
	Private WithEvents _optClaimAllocation_2 As System.Windows.Forms.RadioButton
	Private WithEvents _optClaimAllocation_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optClaimAllocation_0 As System.Windows.Forms.RadioButton
	Public WithEvents Picture3 As System.Windows.Forms.PictureBox
	Private WithEvents _optTreatyPremium_0 As System.Windows.Forms.RadioButton
	Private WithEvents _optTreatyPremium_1 As System.Windows.Forms.RadioButton
	Public WithEvents Picture4 As System.Windows.Forms.PictureBox
	Public WithEvents lblTreatyPremium As System.Windows.Forms.Label
	Private WithEvents _tabRIModel_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents lvwRIModelLine As System.Windows.Forms.ListView
	Private WithEvents _tabRIModel_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents chkCatXOL As System.Windows.Forms.CheckBox
	Public WithEvents chkClmXOL As System.Windows.Forms.CheckBox
	Public WithEvents txtClmXOLLimit As System.Windows.Forms.TextBox
	Public WithEvents cboClmXOLModel As PMLookupControl.cboPMLookup
	Public WithEvents lblClmXOLModel As System.Windows.Forms.Label
	Public WithEvents lblClmXOLLimit As System.Windows.Forms.Label
	Public WithEvents fraClmXOL As System.Windows.Forms.GroupBox
	Public WithEvents txtCatXOLLimit As System.Windows.Forms.TextBox
	Public WithEvents chkCatXOLAutoReins As System.Windows.Forms.CheckBox
	Public WithEvents txtCatXOLReinstatements As System.Windows.Forms.TextBox
	Public WithEvents cboCatXOLModel As PMLookupControl.cboPMLookup
	Public WithEvents lblCatXOLLimit As System.Windows.Forms.Label
	Public WithEvents lblCatXOLModel As System.Windows.Forms.Label
	Public WithEvents lblCatXOLReinstatements As System.Windows.Forms.Label
	Public WithEvents fraCatXOL As System.Windows.Forms.GroupBox
	Private WithEvents _tabRIModel_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents uctSummary As cSIRRIControls.uctRIModelControl
	Private WithEvents _tabRIModel_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents lvwHistory As System.Windows.Forms.ListView
	Private WithEvents _tabRIModel_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents tabRIModel As System.Windows.Forms.TabControl
	Public optClaimAllocation(2) As System.Windows.Forms.RadioButton
	Public optFACPremiums(1) As System.Windows.Forms.RadioButton
    Public optRIModelType(4) As System.Windows.Forms.RadioButton
    Public optTreatyPremium(1) As System.Windows.Forms.RadioButton
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabRIModelPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabRIModel = New System.Windows.Forms.TabControl()
        Me._tabRIModel_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmdConverionRates = New System.Windows.Forms.Button()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblRIModelType = New System.Windows.Forms.Label()
        Me.lblFACPremiums = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblClaimAllocation = New System.Windows.Forms.Label()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.dtpExpiryDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpEffectiveDate = New System.Windows.Forms.DateTimePicker()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.Picture1 = New System.Windows.Forms.PictureBox()
        Me._optRIModelType_0 = New System.Windows.Forms.RadioButton()
        Me._optRIModelType_1 = New System.Windows.Forms.RadioButton()
        Me._optRIModelType_2 = New System.Windows.Forms.RadioButton()
        Me._optRIModelType_3 = New System.Windows.Forms.RadioButton()
        Me._optRIModelType_4 = New System.Windows.Forms.RadioButton()
        Me.Picture2 = New System.Windows.Forms.PictureBox()
        Me._optFACPremiums_0 = New System.Windows.Forms.RadioButton()
        Me._optFACPremiums_1 = New System.Windows.Forms.RadioButton()
        Me.Picture3 = New System.Windows.Forms.PictureBox()
        Me._optClaimAllocation_2 = New System.Windows.Forms.RadioButton()
        Me._optClaimAllocation_1 = New System.Windows.Forms.RadioButton()
        Me._optClaimAllocation_0 = New System.Windows.Forms.RadioButton()
        Me.Picture4 = New System.Windows.Forms.PictureBox()
        Me._optTreatyPremium_0 = New System.Windows.Forms.RadioButton()
        Me._optTreatyPremium_1 = New System.Windows.Forms.RadioButton()
        Me.lblTreatyPremium = New System.Windows.Forms.Label()
        Me._tabRIModel_TabPage1 = New System.Windows.Forms.TabPage()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.lvwRIModelLine = New System.Windows.Forms.ListView()
        Me._tabRIModel_TabPage2 = New System.Windows.Forms.TabPage()
        Me.chkCatXOL = New System.Windows.Forms.CheckBox()
        Me.chkClmXOL = New System.Windows.Forms.CheckBox()
        Me.fraClmXOL = New System.Windows.Forms.GroupBox()
        Me.txtClmXOLLimit = New System.Windows.Forms.TextBox()
        Me.cboClmXOLModel = New PMLookupControl.cboPMLookup()
        Me.lblClmXOLModel = New System.Windows.Forms.Label()
        Me.lblClmXOLLimit = New System.Windows.Forms.Label()
        Me.fraCatXOL = New System.Windows.Forms.GroupBox()
        Me.txtCatXOLLimit = New System.Windows.Forms.TextBox()
        Me.chkCatXOLAutoReins = New System.Windows.Forms.CheckBox()
        Me.txtCatXOLReinstatements = New System.Windows.Forms.TextBox()
        Me.cboCatXOLModel = New PMLookupControl.cboPMLookup()
        Me.lblCatXOLLimit = New System.Windows.Forms.Label()
        Me.lblCatXOLModel = New System.Windows.Forms.Label()
        Me.lblCatXOLReinstatements = New System.Windows.Forms.Label()
        Me._tabRIModel_TabPage3 = New System.Windows.Forms.TabPage()
        Me.uctSummary = New cSIRRIControls.uctRIModelControl()
        Me._tabRIModel_TabPage4 = New System.Windows.Forms.TabPage()
        Me.lvwHistory = New System.Windows.Forms.ListView()
        Me.tabRIModel.SuspendLayout()
        Me._tabRIModel_TabPage0.SuspendLayout()
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Picture1.SuspendLayout()
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Picture2.SuspendLayout()
        CType(Me.Picture3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Picture3.SuspendLayout()
        CType(Me.Picture4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Picture4.SuspendLayout()
        Me._tabRIModel_TabPage1.SuspendLayout()
        Me._tabRIModel_TabPage2.SuspendLayout()
        Me.fraClmXOL.SuspendLayout()
        Me.fraCatXOL.SuspendLayout()
        Me._tabRIModel_TabPage3.SuspendLayout()
        Me._tabRIModel_TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(499, 302)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 35
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
        Me.cmdOK.Location = New System.Drawing.Point(419, 302)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 34
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabRIModel
        '
        Me.tabRIModel.Controls.Add(Me._tabRIModel_TabPage0)
        Me.tabRIModel.Controls.Add(Me._tabRIModel_TabPage1)
        Me.tabRIModel.Controls.Add(Me._tabRIModel_TabPage2)
        Me.tabRIModel.Controls.Add(Me._tabRIModel_TabPage3)
        Me.tabRIModel.Controls.Add(Me._tabRIModel_TabPage4)
        Me.tabRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabRIModel.ItemSize = New System.Drawing.Size(112, 18)
        Me.tabRIModel.Location = New System.Drawing.Point(6, 6)
        Me.tabRIModel.Multiline = True
        Me.tabRIModel.Name = "tabRIModel"
        Me.tabRIModel.SelectedIndex = 0
        Me.tabRIModel.Size = New System.Drawing.Size(570, 293)
        Me.tabRIModel.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabRIModel.TabIndex = 0
        '
        '_tabRIModel_TabPage0
        '
        Me._tabRIModel_TabPage0.Controls.Add(Me.cmdConverionRates)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblExpiryDate)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblCode)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblRIModelType)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblFACPremiums)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblCurrency)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblClaimAllocation)
        Me._tabRIModel_TabPage0.Controls.Add(Me.cboCurrency)
        Me._tabRIModel_TabPage0.Controls.Add(Me.dtpExpiryDate)
        Me._tabRIModel_TabPage0.Controls.Add(Me.dtpEffectiveDate)
        Me._tabRIModel_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabRIModel_TabPage0.Controls.Add(Me.txtCode)
        Me._tabRIModel_TabPage0.Controls.Add(Me.Picture1)
        Me._tabRIModel_TabPage0.Controls.Add(Me.Picture2)
        Me._tabRIModel_TabPage0.Controls.Add(Me.Picture3)
        Me._tabRIModel_TabPage0.Controls.Add(Me.Picture4)
        Me._tabRIModel_TabPage0.Controls.Add(Me.lblTreatyPremium)
        Me._tabRIModel_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabRIModel_TabPage0.Name = "_tabRIModel_TabPage0"
        Me._tabRIModel_TabPage0.Size = New System.Drawing.Size(562, 267)
        Me._tabRIModel_TabPage0.TabIndex = 0
        Me._tabRIModel_TabPage0.Text = "General"
        '
        'cmdConverionRates
        '
        Me.cmdConverionRates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdConverionRates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdConverionRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdConverionRates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdConverionRates.Location = New System.Drawing.Point(409, 232)
        Me.cmdConverionRates.Name = "cmdConverionRates"
        Me.cmdConverionRates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdConverionRates.Size = New System.Drawing.Size(135, 22)
        Me.cmdConverionRates.TabIndex = 45
        Me.cmdConverionRates.Text = "&Conversion Rates"
        Me.cmdConverionRates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdConverionRates.UseVisualStyleBackColor = False
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 42)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 3
        Me.lblDescription.Text = "&Description:"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(16, 98)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(64, 13)
        Me.lblExpiryDate.TabIndex = 7
        Me.lblExpiryDate.Text = "E&xpiry Date:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 69)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(78, 13)
        Me.lblEffectiveDate.TabIndex = 5
        Me.lblEffectiveDate.Text = "E&ffective Date:"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(16, 17)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(35, 13)
        Me.lblCode.TabIndex = 1
        Me.lblCode.Text = "Cod&e:"
        '
        'lblRIModelType
        '
        Me.lblRIModelType.AutoSize = True
        Me.lblRIModelType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIModelType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIModelType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIModelType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIModelType.Location = New System.Drawing.Point(16, 127)
        Me.lblRIModelType.Name = "lblRIModelType"
        Me.lblRIModelType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIModelType.Size = New System.Drawing.Size(80, 13)
        Me.lblRIModelType.TabIndex = 9
        Me.lblRIModelType.Text = "RI Model &Type:"
        '
        'lblFACPremiums
        '
        Me.lblFACPremiums.AutoSize = True
        Me.lblFACPremiums.BackColor = System.Drawing.SystemColors.Control
        Me.lblFACPremiums.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFACPremiums.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFACPremiums.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFACPremiums.Location = New System.Drawing.Point(16, 153)
        Me.lblFACPremiums.Name = "lblFACPremiums"
        Me.lblFACPremiums.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFACPremiums.Size = New System.Drawing.Size(78, 13)
        Me.lblFACPremiums.TabIndex = 10
        Me.lblFACPremiums.Text = "FAC &Premiums:"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 236)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(52, 13)
        Me.lblCurrency.TabIndex = 14
        Me.lblCurrency.Text = "C&urrency:"
        '
        'lblClaimAllocation
        '
        Me.lblClaimAllocation.AutoSize = True
        Me.lblClaimAllocation.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimAllocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimAllocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimAllocation.Location = New System.Drawing.Point(16, 181)
        Me.lblClaimAllocation.Name = "lblClaimAllocation"
        Me.lblClaimAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimAllocation.Size = New System.Drawing.Size(84, 13)
        Me.lblClaimAllocation.TabIndex = 11
        Me.lblClaimAllocation.Text = "Claim &Allocation:"
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(144, 232)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(240, 21)
        Me.cboCurrency.TabIndex = 15
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'dtpExpiryDate
        '
        Me.dtpExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpExpiryDate.Location = New System.Drawing.Point(144, 93)
        Me.dtpExpiryDate.MinDate = New Date(2005, 8, 10, 0, 0, 0, 0)
        Me.dtpExpiryDate.Name = "dtpExpiryDate"
        Me.dtpExpiryDate.ShowCheckBox = True
        Me.dtpExpiryDate.Size = New System.Drawing.Size(140, 20)
        Me.dtpExpiryDate.TabIndex = 8
        Me.dtpExpiryDate.Value = New Date(2005, 8, 10, 0, 0, 0, 0)
        '
        'dtpEffectiveDate
        '
        Me.dtpEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEffectiveDate.Location = New System.Drawing.Point(144, 64)
        Me.dtpEffectiveDate.Name = "dtpEffectiveDate"
        Me.dtpEffectiveDate.Size = New System.Drawing.Size(140, 20)
        Me.dtpEffectiveDate.TabIndex = 6
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(144, 39)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(400, 20)
        Me.txtDescription.TabIndex = 4
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(144, 14)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(140, 20)
        Me.txtCode.TabIndex = 2
        '
        'Picture1
        '
        Me.Picture1.BackColor = System.Drawing.SystemColors.Control
        Me.Picture1.Controls.Add(Me._optRIModelType_0)
        Me.Picture1.Controls.Add(Me._optRIModelType_1)
        Me.Picture1.Controls.Add(Me._optRIModelType_2)
        Me.Picture1.Controls.Add(Me._optRIModelType_3)
        Me.Picture1.Controls.Add(Me._optRIModelType_4)
        Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture1.Location = New System.Drawing.Point(142, 122)
        Me.Picture1.Name = "Picture1"
        Me.Picture1.Size = New System.Drawing.Size(415, 21)
        Me.Picture1.TabIndex = 36
        Me.Picture1.TabStop = False
        '
        '_optRIModelType_0
        '
        Me._optRIModelType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optRIModelType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRIModelType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRIModelType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRIModelType_0.Location = New System.Drawing.Point(2, 2)
        Me._optRIModelType_0.Name = "_optRIModelType_0"
        Me._optRIModelType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRIModelType_0.Size = New System.Drawing.Size(73, 17)
        Me._optRIModelType_0.TabIndex = 40
        Me._optRIModelType_0.TabStop = True
        Me._optRIModelType_0.Text = "Standard"
        Me._optRIModelType_0.UseVisualStyleBackColor = False
        '
        '_optRIModelType_1
        '
        Me._optRIModelType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optRIModelType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRIModelType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRIModelType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRIModelType_1.Location = New System.Drawing.Point(101, 2)
        Me._optRIModelType_1.Name = "_optRIModelType_1"
        Me._optRIModelType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRIModelType_1.Size = New System.Drawing.Size(61, 17)
        Me._optRIModelType_1.TabIndex = 39
        Me._optRIModelType_1.TabStop = True
        Me._optRIModelType_1.Text = "Default"
        Me._optRIModelType_1.UseVisualStyleBackColor = False
        '
        '_optRIModelType_2
        '
        Me._optRIModelType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optRIModelType_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRIModelType_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRIModelType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRIModelType_2.Location = New System.Drawing.Point(200, 2)
        Me._optRIModelType_2.Name = "_optRIModelType_2"
        Me._optRIModelType_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRIModelType_2.Size = New System.Drawing.Size(71, 17)
        Me._optRIModelType_2.TabIndex = 38
        Me._optRIModelType_2.TabStop = True
        Me._optRIModelType_2.Text = "Deferred"
        Me._optRIModelType_2.UseVisualStyleBackColor = False
        '
        '_optRIModelType_3
        '
        Me._optRIModelType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optRIModelType_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRIModelType_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRIModelType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRIModelType_3.Location = New System.Drawing.Point(299, 2)
        Me._optRIModelType_3.Name = "_optRIModelType_3"
        Me._optRIModelType_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRIModelType_3.Size = New System.Drawing.Size(118, 17)
        Me._optRIModelType_3.TabIndex = 37
        Me._optRIModelType_3.TabStop = True
        Me._optRIModelType_3.Text = "Excess of Loss"
        Me._optRIModelType_3.UseVisualStyleBackColor = False
        '
        '_optRIModelType_4
        '
        Me._optRIModelType_4.BackColor = System.Drawing.SystemColors.Control
        Me._optRIModelType_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._optRIModelType_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optRIModelType_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optRIModelType_4.Location = New System.Drawing.Point(299, 2)
        Me._optRIModelType_4.Name = "_optRIModelType_4"
        Me._optRIModelType_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optRIModelType_4.Size = New System.Drawing.Size(118, 17)
        Me._optRIModelType_4.TabIndex = 37
        Me._optRIModelType_4.TabStop = True
        Me._optRIModelType_4.Text = "Cloned"
        Me._optRIModelType_4.UseVisualStyleBackColor = False
        '
        'Picture2
        '
        Me.Picture2.BackColor = System.Drawing.SystemColors.Control
        Me.Picture2.Controls.Add(Me._optFACPremiums_0)
        Me.Picture2.Controls.Add(Me._optFACPremiums_1)
        Me.Picture2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture2.Location = New System.Drawing.Point(142, 150)
        Me.Picture2.Name = "Picture2"
        Me.Picture2.Size = New System.Drawing.Size(415, 21)
        Me.Picture2.TabIndex = 41
        Me.Picture2.TabStop = False
        '
        '_optFACPremiums_0
        '
        Me._optFACPremiums_0.BackColor = System.Drawing.SystemColors.Control
        Me._optFACPremiums_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFACPremiums_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFACPremiums_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFACPremiums_0.Location = New System.Drawing.Point(2, 2)
        Me._optFACPremiums_0.Name = "_optFACPremiums_0"
        Me._optFACPremiums_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFACPremiums_0.Size = New System.Drawing.Size(89, 17)
        Me._optFACPremiums_0.TabIndex = 43
        Me._optFACPremiums_0.TabStop = True
        Me._optFACPremiums_0.Text = "Proportional"
        Me._optFACPremiums_0.UseVisualStyleBackColor = False
        '
        '_optFACPremiums_1
        '
        Me._optFACPremiums_1.BackColor = System.Drawing.SystemColors.Control
        Me._optFACPremiums_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFACPremiums_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFACPremiums_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFACPremiums_1.Location = New System.Drawing.Point(101, 2)
        Me._optFACPremiums_1.Name = "_optFACPremiums_1"
        Me._optFACPremiums_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFACPremiums_1.Size = New System.Drawing.Size(117, 17)
        Me._optFACPremiums_1.TabIndex = 42
        Me._optFACPremiums_1.TabStop = True
        Me._optFACPremiums_1.Text = "Non-Proportional"
        Me._optFACPremiums_1.UseVisualStyleBackColor = False
        '
        'Picture3
        '
        Me.Picture3.BackColor = System.Drawing.SystemColors.Control
        Me.Picture3.Controls.Add(Me._optClaimAllocation_2)
        Me.Picture3.Controls.Add(Me._optClaimAllocation_1)
        Me.Picture3.Controls.Add(Me._optClaimAllocation_0)
        Me.Picture3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture3.Location = New System.Drawing.Point(142, 178)
        Me.Picture3.Name = "Picture3"
        Me.Picture3.Size = New System.Drawing.Size(415, 21)
        Me.Picture3.TabIndex = 44
        Me.Picture3.TabStop = False
        '
        '_optClaimAllocation_2
        '
        Me._optClaimAllocation_2.BackColor = System.Drawing.SystemColors.Control
        Me._optClaimAllocation_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optClaimAllocation_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optClaimAllocation_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optClaimAllocation_2.Location = New System.Drawing.Point(100, 2)
        Me._optClaimAllocation_2.Name = "_optClaimAllocation_2"
        Me._optClaimAllocation_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optClaimAllocation_2.Size = New System.Drawing.Size(117, 17)
        Me._optClaimAllocation_2.TabIndex = 47
        Me._optClaimAllocation_2.TabStop = True
        Me._optClaimAllocation_2.Text = "Non-Proportional"
        Me._optClaimAllocation_2.UseVisualStyleBackColor = False
        '
        '_optClaimAllocation_1
        '
        Me._optClaimAllocation_1.BackColor = System.Drawing.SystemColors.Control
        Me._optClaimAllocation_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optClaimAllocation_1.Enabled = False
        Me._optClaimAllocation_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optClaimAllocation_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optClaimAllocation_1.Location = New System.Drawing.Point(226, 2)
        Me._optClaimAllocation_1.Name = "_optClaimAllocation_1"
        Me._optClaimAllocation_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optClaimAllocation_1.Size = New System.Drawing.Size(171, 17)
        Me._optClaimAllocation_1.TabIndex = 46
        Me._optClaimAllocation_1.TabStop = True
        Me._optClaimAllocation_1.Text = "By Priority (Unsupported)"
        Me._optClaimAllocation_1.UseVisualStyleBackColor = False
        Me._optClaimAllocation_1.Visible = False
        '
        '_optClaimAllocation_0
        '
        Me._optClaimAllocation_0.BackColor = System.Drawing.SystemColors.Control
        Me._optClaimAllocation_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optClaimAllocation_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optClaimAllocation_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optClaimAllocation_0.Location = New System.Drawing.Point(2, 2)
        Me._optClaimAllocation_0.Name = "_optClaimAllocation_0"
        Me._optClaimAllocation_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optClaimAllocation_0.Size = New System.Drawing.Size(89, 17)
        Me._optClaimAllocation_0.TabIndex = 45
        Me._optClaimAllocation_0.TabStop = True
        Me._optClaimAllocation_0.Text = "Proportional"
        Me._optClaimAllocation_0.UseVisualStyleBackColor = False
        '
        'lblTreatyPremium
        '
        Me.lblTreatyPremium.AutoSize = True
        Me.lblTreatyPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblTreatyPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTreatyPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTreatyPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTreatyPremium.Location = New System.Drawing.Point(16, 208)
        Me.lblTreatyPremium.Name = "lblTreatyPremium"
        Me.lblTreatyPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTreatyPremium.Size = New System.Drawing.Size(84, 13)
        Me.lblTreatyPremium.TabIndex = 12
        Me.lblTreatyPremium.Text = "Treaty Premium:"
        '
        'Picture4
        '
        Me.Picture4.BackColor = System.Drawing.SystemColors.Control
        Me.Picture4.Controls.Add(Me._optTreatyPremium_0)
        Me.Picture4.Controls.Add(Me._optTreatyPremium_1)
        Me.Picture4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Picture4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Picture4.Location = New System.Drawing.Point(142, 204)
        Me.Picture4.Name = "Picture4"
        Me.Picture4.Size = New System.Drawing.Size(415, 21)
        Me.Picture4.TabIndex = 13
        Me.Picture4.TabStop = False
        '
        '_optTreatyPremium_0
        '
        Me._optTreatyPremium_0.BackColor = System.Drawing.SystemColors.Control
        Me._optTreatyPremium_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTreatyPremium_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTreatyPremium_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optTreatyPremium_0.Location = New System.Drawing.Point(2, 2)
        Me._optTreatyPremium_0.Name = "_optTreatyPremium_0"
        Me._optTreatyPremium_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTreatyPremium_0.Size = New System.Drawing.Size(73, 17)
        Me._optTreatyPremium_0.TabIndex = 48
        Me._optTreatyPremium_0.TabStop = True
        Me._optTreatyPremium_0.Text = "Standard"
        Me._optTreatyPremium_0.UseVisualStyleBackColor = False
        '
        '_optTreatyPremium_1
        '
        Me._optTreatyPremium_1.BackColor = System.Drawing.SystemColors.Control
        Me._optTreatyPremium_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optTreatyPremium_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optTreatyPremium_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optTreatyPremium_1.Location = New System.Drawing.Point(101, 2)
        Me._optTreatyPremium_1.Name = "_optTreatyPremium_1"
        Me._optTreatyPremium_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optTreatyPremium_1.Size = New System.Drawing.Size(150, 17)
        Me._optTreatyPremium_1.TabIndex = 49
        Me._optTreatyPremium_1.TabStop = True
        Me._optTreatyPremium_1.Text = "Variable Cession Order"
        Me._optTreatyPremium_1.UseVisualStyleBackColor = False
        '
        '_tabRIModel_TabPage1
        '
        Me._tabRIModel_TabPage1.Controls.Add(Me.cmdAdd)
        Me._tabRIModel_TabPage1.Controls.Add(Me.cmdEdit)
        Me._tabRIModel_TabPage1.Controls.Add(Me.cmdDelete)
        Me._tabRIModel_TabPage1.Controls.Add(Me.lvwRIModelLine)
        Me._tabRIModel_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabRIModel_TabPage1.Name = "_tabRIModel_TabPage1"
        Me._tabRIModel_TabPage1.Size = New System.Drawing.Size(562, 239)
        Me._tabRIModel_TabPage1.TabIndex = 1
        Me._tabRIModel_TabPage1.Text = "Reinsurance Lines"
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(324, 210)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 15
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(404, 210)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 16
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(484, 210)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 17
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'lvwRIModelLine
        '
        Me.lvwRIModelLine.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRIModelLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRIModelLine.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRIModelLine.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRIModelLine.FullRowSelect = True
        Me.lvwRIModelLine.HideSelection = False
        Me.lvwRIModelLine.Location = New System.Drawing.Point(8, 8)
        Me.lvwRIModelLine.Name = "lvwRIModelLine"
        Me.lvwRIModelLine.Size = New System.Drawing.Size(549, 195)
        Me.lvwRIModelLine.TabIndex = 14
        Me.lvwRIModelLine.UseCompatibleStateImageBehavior = False
        Me.lvwRIModelLine.View = System.Windows.Forms.View.Details
        '
        '_tabRIModel_TabPage2
        '
        Me._tabRIModel_TabPage2.Controls.Add(Me.chkCatXOL)
        Me._tabRIModel_TabPage2.Controls.Add(Me.chkClmXOL)
        Me._tabRIModel_TabPage2.Controls.Add(Me.fraClmXOL)
        Me._tabRIModel_TabPage2.Controls.Add(Me.fraCatXOL)
        Me._tabRIModel_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabRIModel_TabPage2.Name = "_tabRIModel_TabPage2"
        Me._tabRIModel_TabPage2.Size = New System.Drawing.Size(562, 239)
        Me._tabRIModel_TabPage2.TabIndex = 2
        Me._tabRIModel_TabPage2.Text = "Excess of Loss"
        '
        'chkCatXOL
        '
        Me.chkCatXOL.BackColor = System.Drawing.SystemColors.Control
        Me.chkCatXOL.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCatXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCatXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCatXOL.Location = New System.Drawing.Point(18, 98)
        Me.chkCatXOL.Name = "chkCatXOL"
        Me.chkCatXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCatXOL.Size = New System.Drawing.Size(179, 14)
        Me.chkCatXOL.TabIndex = 24
        Me.chkCatXOL.Text = "Catastrophe Excess of Loss"
        Me.chkCatXOL.UseVisualStyleBackColor = False
        '
        'chkClmXOL
        '
        Me.chkClmXOL.BackColor = System.Drawing.SystemColors.Control
        Me.chkClmXOL.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClmXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClmXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClmXOL.Location = New System.Drawing.Point(18, 8)
        Me.chkClmXOL.Name = "chkClmXOL"
        Me.chkClmXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClmXOL.Size = New System.Drawing.Size(165, 14)
        Me.chkClmXOL.TabIndex = 18
        Me.chkClmXOL.Text = "Per Claim Excess of Loss"
        Me.chkClmXOL.UseVisualStyleBackColor = False
        '
        'fraClmXOL
        '
        Me.fraClmXOL.BackColor = System.Drawing.SystemColors.Control
        Me.fraClmXOL.Controls.Add(Me.txtClmXOLLimit)
        Me.fraClmXOL.Controls.Add(Me.cboClmXOLModel)
        Me.fraClmXOL.Controls.Add(Me.lblClmXOLModel)
        Me.fraClmXOL.Controls.Add(Me.lblClmXOLLimit)
        Me.fraClmXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClmXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClmXOL.Location = New System.Drawing.Point(8, 7)
        Me.fraClmXOL.Name = "fraClmXOL"
        Me.fraClmXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClmXOL.Size = New System.Drawing.Size(549, 83)
        Me.fraClmXOL.TabIndex = 19
        Me.fraClmXOL.TabStop = False
        '
        'txtClmXOLLimit
        '
        Me.txtClmXOLLimit.AcceptsReturn = True
        Me.txtClmXOLLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtClmXOLLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClmXOLLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClmXOLLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClmXOLLimit.Location = New System.Drawing.Point(136, 46)
        Me.txtClmXOLLimit.MaxLength = 0
        Me.txtClmXOLLimit.Name = "txtClmXOLLimit"
        Me.txtClmXOLLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClmXOLLimit.Size = New System.Drawing.Size(140, 20)
        Me.txtClmXOLLimit.TabIndex = 23
        Me.txtClmXOLLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboClmXOLModel
        '
        Me.cboClmXOLModel.DefaultItemId = 0
        Me.cboClmXOLModel.FirstItem = ""
        Me.cboClmXOLModel.ItemId = 0
        Me.cboClmXOLModel.ListIndex = -1
        Me.cboClmXOLModel.Location = New System.Drawing.Point(136, 20)
        Me.cboClmXOLModel.Name = "cboClmXOLModel"
        Me.cboClmXOLModel.PMLookupProductFamily = 1
        Me.cboClmXOLModel.SingleItemId = 0
        Me.cboClmXOLModel.Size = New System.Drawing.Size(400, 21)
        Me.cboClmXOLModel.SortColumnName = ""
        Me.cboClmXOLModel.Sorted = True
        Me.cboClmXOLModel.TabIndex = 21
        Me.cboClmXOLModel.TableName = "ri_model"
        Me.cboClmXOLModel.ToolTipText = ""
        Me.cboClmXOLModel.WhereClause = "ri_model_type = 3"
        '
        'lblClmXOLModel
        '
        Me.lblClmXOLModel.AutoSize = True
        Me.lblClmXOLModel.BackColor = System.Drawing.SystemColors.Control
        Me.lblClmXOLModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClmXOLModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClmXOLModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClmXOLModel.Location = New System.Drawing.Point(12, 24)
        Me.lblClmXOLModel.Name = "lblClmXOLModel"
        Me.lblClmXOLModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClmXOLModel.Size = New System.Drawing.Size(63, 13)
        Me.lblClmXOLModel.TabIndex = 20
        Me.lblClmXOLModel.Text = "XOL Model:"
        '
        'lblClmXOLLimit
        '
        Me.lblClmXOLLimit.AutoSize = True
        Me.lblClmXOLLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblClmXOLLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClmXOLLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClmXOLLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClmXOLLimit.Location = New System.Drawing.Point(12, 49)
        Me.lblClmXOLLimit.Name = "lblClmXOLLimit"
        Me.lblClmXOLLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClmXOLLimit.Size = New System.Drawing.Size(55, 13)
        Me.lblClmXOLLimit.TabIndex = 22
        Me.lblClmXOLLimit.Text = "XOL Limit:"
        '
        'fraCatXOL
        '
        Me.fraCatXOL.BackColor = System.Drawing.SystemColors.Control
        Me.fraCatXOL.Controls.Add(Me.txtCatXOLLimit)
        Me.fraCatXOL.Controls.Add(Me.chkCatXOLAutoReins)
        Me.fraCatXOL.Controls.Add(Me.txtCatXOLReinstatements)
        Me.fraCatXOL.Controls.Add(Me.cboCatXOLModel)
        Me.fraCatXOL.Controls.Add(Me.lblCatXOLLimit)
        Me.fraCatXOL.Controls.Add(Me.lblCatXOLModel)
        Me.fraCatXOL.Controls.Add(Me.lblCatXOLReinstatements)
        Me.fraCatXOL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCatXOL.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCatXOL.Location = New System.Drawing.Point(8, 98)
        Me.fraCatXOL.Name = "fraCatXOL"
        Me.fraCatXOL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCatXOL.Size = New System.Drawing.Size(549, 133)
        Me.fraCatXOL.TabIndex = 25
        Me.fraCatXOL.TabStop = False
        '
        'txtCatXOLLimit
        '
        Me.txtCatXOLLimit.AcceptsReturn = True
        Me.txtCatXOLLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtCatXOLLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCatXOLLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCatXOLLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCatXOLLimit.Location = New System.Drawing.Point(136, 46)
        Me.txtCatXOLLimit.MaxLength = 0
        Me.txtCatXOLLimit.Name = "txtCatXOLLimit"
        Me.txtCatXOLLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCatXOLLimit.Size = New System.Drawing.Size(140, 20)
        Me.txtCatXOLLimit.TabIndex = 29
        Me.txtCatXOLLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkCatXOLAutoReins
        '
        Me.chkCatXOLAutoReins.BackColor = System.Drawing.SystemColors.Control
        Me.chkCatXOLAutoReins.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCatXOLAutoReins.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCatXOLAutoReins.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCatXOLAutoReins.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCatXOLAutoReins.Location = New System.Drawing.Point(10, 74)
        Me.chkCatXOLAutoReins.Name = "chkCatXOLAutoReins"
        Me.chkCatXOLAutoReins.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCatXOLAutoReins.Size = New System.Drawing.Size(139, 15)
        Me.chkCatXOLAutoReins.TabIndex = 30
        Me.chkCatXOLAutoReins.Text = "Auto Reinstatement:"
        Me.chkCatXOLAutoReins.UseVisualStyleBackColor = False
        '
        'txtCatXOLReinstatements
        '
        Me.txtCatXOLReinstatements.AcceptsReturn = True
        Me.txtCatXOLReinstatements.BackColor = System.Drawing.SystemColors.Window
        Me.txtCatXOLReinstatements.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCatXOLReinstatements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCatXOLReinstatements.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCatXOLReinstatements.Location = New System.Drawing.Point(136, 96)
        Me.txtCatXOLReinstatements.MaxLength = 0
        Me.txtCatXOLReinstatements.Name = "txtCatXOLReinstatements"
        Me.txtCatXOLReinstatements.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCatXOLReinstatements.Size = New System.Drawing.Size(140, 20)
        Me.txtCatXOLReinstatements.TabIndex = 32
        Me.txtCatXOLReinstatements.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboCatXOLModel
        '
        Me.cboCatXOLModel.DefaultItemId = 0
        Me.cboCatXOLModel.FirstItem = ""
        Me.cboCatXOLModel.ItemId = 0
        Me.cboCatXOLModel.ListIndex = -1
        Me.cboCatXOLModel.Location = New System.Drawing.Point(136, 20)
        Me.cboCatXOLModel.Name = "cboCatXOLModel"
        Me.cboCatXOLModel.PMLookupProductFamily = 1
        Me.cboCatXOLModel.SingleItemId = 0
        Me.cboCatXOLModel.Size = New System.Drawing.Size(400, 21)
        Me.cboCatXOLModel.SortColumnName = ""
        Me.cboCatXOLModel.Sorted = True
        Me.cboCatXOLModel.TabIndex = 27
        Me.cboCatXOLModel.TableName = "ri_model"
        Me.cboCatXOLModel.ToolTipText = ""
        Me.cboCatXOLModel.WhereClause = "ri_model_type = 3"
        '
        'lblCatXOLLimit
        '
        Me.lblCatXOLLimit.AutoSize = True
        Me.lblCatXOLLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblCatXOLLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCatXOLLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCatXOLLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCatXOLLimit.Location = New System.Drawing.Point(12, 49)
        Me.lblCatXOLLimit.Name = "lblCatXOLLimit"
        Me.lblCatXOLLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCatXOLLimit.Size = New System.Drawing.Size(82, 13)
        Me.lblCatXOLLimit.TabIndex = 28
        Me.lblCatXOLLimit.Text = "XOL Total Limit:"
        '
        'lblCatXOLModel
        '
        Me.lblCatXOLModel.AutoSize = True
        Me.lblCatXOLModel.BackColor = System.Drawing.SystemColors.Control
        Me.lblCatXOLModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCatXOLModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCatXOLModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCatXOLModel.Location = New System.Drawing.Point(12, 24)
        Me.lblCatXOLModel.Name = "lblCatXOLModel"
        Me.lblCatXOLModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCatXOLModel.Size = New System.Drawing.Size(63, 13)
        Me.lblCatXOLModel.TabIndex = 26
        Me.lblCatXOLModel.Text = "XOL Model:"
        '
        'lblCatXOLReinstatements
        '
        Me.lblCatXOLReinstatements.AutoSize = True
        Me.lblCatXOLReinstatements.BackColor = System.Drawing.SystemColors.Control
        Me.lblCatXOLReinstatements.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCatXOLReinstatements.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCatXOLReinstatements.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCatXOLReinstatements.Location = New System.Drawing.Point(12, 99)
        Me.lblCatXOLReinstatements.Name = "lblCatXOLReinstatements"
        Me.lblCatXOLReinstatements.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCatXOLReinstatements.Size = New System.Drawing.Size(83, 13)
        Me.lblCatXOLReinstatements.TabIndex = 31
        Me.lblCatXOLReinstatements.Text = "Reinstatements:"
        '
        '_tabRIModel_TabPage3
        '
        Me._tabRIModel_TabPage3.Controls.Add(Me.uctSummary)
        Me._tabRIModel_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabRIModel_TabPage3.Name = "_tabRIModel_TabPage3"
        Me._tabRIModel_TabPage3.Size = New System.Drawing.Size(562, 239)
        Me._tabRIModel_TabPage3.TabIndex = 3
        Me._tabRIModel_TabPage3.Text = "Summary"
        '
        'uctSummary
        '
        Me.uctSummary.FilterType = 0
        Me.uctSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctSummary.Location = New System.Drawing.Point(8, 8)
        Me.uctSummary.Name = "uctSummary"
        Me.uctSummary.RIArrangementID = 0
        Me.uctSummary.Size = New System.Drawing.Size(549, 223)
        Me.uctSummary.TabIndex = 33
        '
        '_tabRIModel_TabPage4
        '
        Me._tabRIModel_TabPage4.Controls.Add(Me.lvwHistory)
        Me._tabRIModel_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabRIModel_TabPage4.Name = "_tabRIModel_TabPage4"
        Me._tabRIModel_TabPage4.Size = New System.Drawing.Size(562, 239)
        Me._tabRIModel_TabPage4.TabIndex = 4
        Me._tabRIModel_TabPage4.Text = "History"
        '
        'lvwHistory
        '
        Me.lvwHistory.BackColor = System.Drawing.SystemColors.Window
        Me.lvwHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwHistory.FullRowSelect = True
        Me.lvwHistory.HideSelection = False
        Me.lvwHistory.Location = New System.Drawing.Point(8, 4)
        Me.lvwHistory.Name = "lvwHistory"
        Me.lvwHistory.Size = New System.Drawing.Size(553, 233)
        Me.lvwHistory.TabIndex = 48
        Me.lvwHistory.UseCompatibleStateImageBehavior = False
        Me.lvwHistory.View = System.Windows.Forms.View.Details
        '
        'frmRIModel
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(579, 331)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabRIModel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRIModel"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RI Model Maintenance"
        Me.tabRIModel.ResumeLayout(False)
        Me._tabRIModel_TabPage0.ResumeLayout(False)
        Me._tabRIModel_TabPage0.PerformLayout()
        CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Picture1.ResumeLayout(False)
        CType(Me.Picture2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Picture2.ResumeLayout(False)
        CType(Me.Picture3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Picture3.ResumeLayout(False)
        CType(Me.Picture4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Picture4.ResumeLayout(False)
        Me._tabRIModel_TabPage1.ResumeLayout(False)
        Me._tabRIModel_TabPage2.ResumeLayout(False)
        Me.fraClmXOL.ResumeLayout(False)
        Me.fraClmXOL.PerformLayout()
        Me.fraCatXOL.ResumeLayout(False)
        Me.fraCatXOL.PerformLayout()
        Me._tabRIModel_TabPage3.ResumeLayout(False)
        Me._tabRIModel_TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptRIModelType()
		Me.optRIModelType(0) = _optRIModelType_0
		Me.optRIModelType(1) = _optRIModelType_1
		Me.optRIModelType(2) = _optRIModelType_2
        Me.optRIModelType(3) = _optRIModelType_3
        Me.optRIModelType(4) = _optRIModelType_4
	End Sub
	Sub InitializeoptFACPremiums()
		Me.optFACPremiums(0) = _optFACPremiums_0
		Me.optFACPremiums(1) = _optFACPremiums_1
	End Sub
	Sub InitializeoptClaimAllocation()
		Me.optClaimAllocation(2) = _optClaimAllocation_2
		Me.optClaimAllocation(1) = _optClaimAllocation_1
		Me.optClaimAllocation(0) = _optClaimAllocation_0
    End Sub
    Sub InitializeoptTreatyPremium()
        Me.optTreatyPremium(0) = _optTreatyPremium_0
        Me.optTreatyPremium(1) = _optTreatyPremium_1
    End Sub
    Private WithEvents _optRIModelType_4 As System.Windows.Forms.RadioButton
    Public WithEvents cmdConverionRates As Button
#End Region
End Class