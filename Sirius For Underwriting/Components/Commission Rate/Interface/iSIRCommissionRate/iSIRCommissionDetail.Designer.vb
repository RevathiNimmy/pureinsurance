'developer guide no.129
Imports SharedFiles
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetail
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Added the code as the controls to be initialised befor assigning properties to it.
        m_oFormfields = New iPMFormControl.FormFields()
        m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtRate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If chkIsvalue.CheckState = CheckState.Checked Then

                'start
                'm_oFormfields.Item(CStr(1)).set_FieldType(gPMConstants.PMEDataType.PMCurrency)
                m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMCurrency
                'm_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatCurrency)
                m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                m_oFormfields.Item(1).DecimalPlaces = 0
            Else
                'm_oFormfields.Item(CStr(1)).set_FieldType(gPMConstants.PMEDataType.PMDouble)
                m_oFormfields.Item(1).FieldType = gPMConstants.PMEDataType.PMDouble
                'm_oFormfields.Item(CStr(1)).set_FieldFormat(gPMConstants.PMEFormatStyle.PMFormatPercent)
                m_oFormfields.Item(1).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                'end
                m_oFormfields.Item(1).DecimalPlaces = 10
            End If
        End If

        m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtEffectiveDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

        'Start - Renuka - (WPR64 Paralleling)
        m_lReturn = m_oFormfields.AddNewFormField(ctlControl:=txtMaximumRate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
        'End - Renuka - (WPR64 Paralleling)
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
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents lblMaximumRate As System.Windows.Forms.Label
    Public WithEvents lblPartyType As System.Windows.Forms.Label
    Public WithEvents lblParty As System.Windows.Forms.Label
    Public WithEvents lblProduct As System.Windows.Forms.Label
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents lblTransactionType As System.Windows.Forms.Label
    Public WithEvents lblCommissionband As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblRate As System.Windows.Forms.Label
    Public WithEvents lblCommisionGroup As System.Windows.Forms.Label
    Public WithEvents lblCommissionLevel As System.Windows.Forms.Label
    Public WithEvents lblTaxGroup As System.Windows.Forms.Label
    Public WithEvents cboPartyType As System.Windows.Forms.ComboBox
    Public WithEvents cboParty As System.Windows.Forms.ComboBox
    Public WithEvents cboProduct As System.Windows.Forms.ComboBox
    Public WithEvents cboRiskType As System.Windows.Forms.ComboBox
    Public WithEvents cboTransactionType As System.Windows.Forms.ComboBox
    Public WithEvents cboCommissionband As System.Windows.Forms.ComboBox
    Public WithEvents cboCommissionLevel As System.Windows.Forms.ComboBox
    Public WithEvents chkIsvalue As System.Windows.Forms.CheckBox
    Public WithEvents txtRate As System.Windows.Forms.TextBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public WithEvents cboCommissionGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboTaxGroup As System.Windows.Forms.ComboBox
    Public WithEvents cmdFindParty As System.Windows.Forms.Button
    Public WithEvents txtMaximumRate As System.Windows.Forms.TextBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblPartyType = New System.Windows.Forms.Label
        Me.lblParty = New System.Windows.Forms.Label
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblTransactionType = New System.Windows.Forms.Label
        Me.lblCommissionband = New System.Windows.Forms.Label
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblCommisionGroup = New System.Windows.Forms.Label
        Me.lblTaxGroup = New System.Windows.Forms.Label
        Me.lblMaximumRate = New System.Windows.Forms.Label
        Me.cboPartyType = New System.Windows.Forms.ComboBox
        Me.cboParty = New System.Windows.Forms.ComboBox
        Me.cboProduct = New System.Windows.Forms.ComboBox
        Me.cboRiskType = New System.Windows.Forms.ComboBox
        Me.cboTransactionType = New System.Windows.Forms.ComboBox
        Me.cboCommissionband = New System.Windows.Forms.ComboBox
        Me.chkIsvalue = New System.Windows.Forms.CheckBox
        Me.txtRate = New System.Windows.Forms.TextBox
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.cboCommissionGroup = New System.Windows.Forms.ComboBox
        Me.cboTaxGroup = New System.Windows.Forms.ComboBox
        Me.cmdFindParty = New System.Windows.Forms.Button
        Me.txtMaximumRate = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lblCommissionLevel = New System.Windows.Forms.Label
        Me.cboCommissionLevel = New System.Windows.Forms.ComboBox
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(345, 326)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 20
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(499, 326)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 19
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
        Me.cmdCancel.Location = New System.Drawing.Point(424, 326)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 18
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(560, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 16)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(565, 304)
        Me.SSTab1.TabIndex = 9
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionLevel)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionLevel)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblPartyType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblParty)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblProduct)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTransactionType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionband)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommisionGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTaxGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblMaximumRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboPartyType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboParty)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboProduct)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboTransactionType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionband)
        Me._SSTab1_TabPage0.Controls.Add(Me.chkIsvalue)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboTaxGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdFindParty)
        Me._SSTab1_TabPage0.Controls.Add(Me.txtMaximumRate)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(557, 278)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1- General"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'lblPartyType
        '
        Me.lblPartyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyType.Location = New System.Drawing.Point(8, 23)
        Me.lblPartyType.Name = "lblPartyType"
        Me.lblPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyType.Size = New System.Drawing.Size(65, 17)
        Me.lblPartyType.TabIndex = 10
        Me.lblPartyType.Text = "Party type:"
        '
        'lblParty
        '
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(8, 119)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(65, 17)
        Me.lblParty.TabIndex = 11
        Me.lblParty.Text = "Party:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(8, 87)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(73, 17)
        Me.lblProduct.TabIndex = 12
        Me.lblProduct.Text = "Product:"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(272, 23)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(81, 17)
        Me.lblRiskType.TabIndex = 13
        Me.lblRiskType.Text = "Risk type:"
        '
        'lblTransactionType
        '
        Me.lblTransactionType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionType.Location = New System.Drawing.Point(272, 55)
        Me.lblTransactionType.Name = "lblTransactionType"
        Me.lblTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionType.Size = New System.Drawing.Size(105, 17)
        Me.lblTransactionType.TabIndex = 14
        Me.lblTransactionType.Text = "Transaction type:"
        '
        'lblCommissionband
        '
        Me.lblCommissionband.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionband.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionband.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionband.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionband.Location = New System.Drawing.Point(272, 87)
        Me.lblCommissionband.Name = "lblCommissionband"
        Me.lblCommissionband.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionband.Size = New System.Drawing.Size(113, 17)
        Me.lblCommissionband.TabIndex = 15
        Me.lblCommissionband.Text = "Commission band:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(272, 119)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(105, 17)
        Me.lblEffectiveDate.TabIndex = 16
        Me.lblEffectiveDate.Text = "Effective date:"
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(8, 149)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(73, 17)
        Me.lblRate.TabIndex = 17
        Me.lblRate.Text = "Rate:"
        '
        'lblCommisionGroup
        '
        Me.lblCommisionGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommisionGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommisionGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommisionGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommisionGroup.Location = New System.Drawing.Point(8, 51)
        Me.lblCommisionGroup.Name = "lblCommisionGroup"
        Me.lblCommisionGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommisionGroup.Size = New System.Drawing.Size(81, 29)
        Me.lblCommisionGroup.TabIndex = 22
        Me.lblCommisionGroup.Text = "Commission Group:"
        '
        'lblTaxGroup
        '
        Me.lblTaxGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxGroup.Location = New System.Drawing.Point(272, 150)
        Me.lblTaxGroup.Name = "lblTaxGroup"
        Me.lblTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxGroup.Size = New System.Drawing.Size(113, 17)
        Me.lblTaxGroup.TabIndex = 24
        Me.lblTaxGroup.Text = "Tax Group:"
        '
        'lblMaximumRate
        '
        Me.lblMaximumRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaximumRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaximumRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaximumRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaximumRate.Location = New System.Drawing.Point(273, 179)
        Me.lblMaximumRate.Name = "lblMaximumRate"
        Me.lblMaximumRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaximumRate.Size = New System.Drawing.Size(112, 18)
        Me.lblMaximumRate.TabIndex = 27
        Me.lblMaximumRate.Text = "Maximum Rate:"
        '
        'cboPartyType
        '
        Me.cboPartyType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPartyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPartyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPartyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPartyType.Location = New System.Drawing.Point(88, 20)
        Me.cboPartyType.Name = "cboPartyType"
        Me.cboPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPartyType.Size = New System.Drawing.Size(169, 21)
        Me.cboPartyType.TabIndex = 0
        '
        'cboParty
        '
        Me.cboParty.BackColor = System.Drawing.SystemColors.Window
        Me.cboParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboParty.Location = New System.Drawing.Point(88, 114)
        Me.cboParty.Name = "cboParty"
        Me.cboParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboParty.Size = New System.Drawing.Size(145, 21)
        Me.cboParty.TabIndex = 4
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(88, 84)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(169, 21)
        Me.cboProduct.TabIndex = 2
        '
        'cboRiskType
        '
        Me.cboRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRiskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRiskType.Location = New System.Drawing.Point(384, 20)
        Me.cboRiskType.Name = "cboRiskType"
        Me.cboRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRiskType.Size = New System.Drawing.Size(169, 21)
        Me.cboRiskType.TabIndex = 1
        '
        'cboTransactionType
        '
        Me.cboTransactionType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTransactionType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTransactionType.Location = New System.Drawing.Point(384, 52)
        Me.cboTransactionType.Name = "cboTransactionType"
        Me.cboTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransactionType.Size = New System.Drawing.Size(169, 21)
        Me.cboTransactionType.TabIndex = 3
        '
        'cboCommissionband
        '
        Me.cboCommissionband.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionband.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionband.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionband.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionband.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionband.Location = New System.Drawing.Point(384, 84)
        Me.cboCommissionband.Name = "cboCommissionband"
        Me.cboCommissionband.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionband.Size = New System.Drawing.Size(169, 21)
        Me.cboCommissionband.TabIndex = 5
        '
        'chkIsvalue
        '
        Me.chkIsvalue.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsvalue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsvalue.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsvalue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsvalue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsvalue.Location = New System.Drawing.Point(8, 180)
        Me.chkIsvalue.Name = "chkIsvalue"
        Me.chkIsvalue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsvalue.Size = New System.Drawing.Size(97, 17)
        Me.chkIsvalue.TabIndex = 8
        Me.chkIsvalue.Text = "Is Value ?"
        Me.chkIsvalue.UseVisualStyleBackColor = False
        '
        'txtRate
        '
        Me.txtRate.AcceptsReturn = True
        Me.txtRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate.Location = New System.Drawing.Point(88, 146)
        Me.txtRate.MaxLength = 0
        Me.txtRate.Name = "txtRate"
        Me.txtRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate.Size = New System.Drawing.Size(169, 20)
        Me.txtRate.TabIndex = 6
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(384, 116)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(169, 20)
        Me.txtEffectiveDate.TabIndex = 7
        '
        'cboCommissionGroup
        '
        Me.cboCommissionGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionGroup.Enabled = False
        Me.cboCommissionGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionGroup.Location = New System.Drawing.Point(88, 52)
        Me.cboCommissionGroup.Name = "cboCommissionGroup"
        Me.cboCommissionGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboCommissionGroup.TabIndex = 21
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxGroup.Location = New System.Drawing.Point(384, 146)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxGroup.TabIndex = 23
        '
        'cmdFindParty
        '
        Me.cmdFindParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindParty.Location = New System.Drawing.Point(232, 114)
        Me.cmdFindParty.Name = "cmdFindParty"
        Me.cmdFindParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindParty.Size = New System.Drawing.Size(24, 19)
        Me.cmdFindParty.TabIndex = 25
        Me.cmdFindParty.Text = "..."
        Me.cmdFindParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindParty.UseVisualStyleBackColor = False
        '
        'txtMaximumRate
        '
        Me.txtMaximumRate.AcceptsReturn = True
        Me.txtMaximumRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaximumRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaximumRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaximumRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaximumRate.Location = New System.Drawing.Point(384, 176)
        Me.txtMaximumRate.MaxLength = 0
        Me.txtMaximumRate.Name = "txtMaximumRate"
        Me.txtMaximumRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaximumRate.Size = New System.Drawing.Size(169, 20)
        Me.txtMaximumRate.TabIndex = 26
        '
        'lblCommissionLevel
        '
        Me.lblCommissionLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionLevel.Location = New System.Drawing.Point(8, 217)
        Me.lblCommissionLevel.Name = "lblCommissionLevel"
        Me.lblCommissionLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionLevel.Size = New System.Drawing.Size(97, 17)
        Me.lblCommissionLevel.TabIndex = 28
        Me.lblCommissionLevel.Text = "Commission Level:"
        '
        'cboCommissionLevel
        '
        Me.cboCommissionLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionLevel.Location = New System.Drawing.Point(106, 213)
        Me.cboCommissionLevel.Name = "cboCommissionLevel"
        Me.cboCommissionLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionLevel.Size = New System.Drawing.Size(150, 21)
        Me.cboCommissionLevel.TabIndex = 29
        '
        'frmDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(578, 360)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Commission rate details"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class