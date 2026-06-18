<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwCommissionRate_InitializeColumnKeys()
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
    Public WithEvents txtMaximumRate1 As System.Windows.Forms.TextBox
    Public WithEvents txtDate As System.Windows.Forms.TextBox
    Public WithEvents txtRate1 As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents lblPartyType As System.Windows.Forms.Label
    Public WithEvents lblProduct As System.Windows.Forms.Label
    Public WithEvents lblCommissionGroup As System.Windows.Forms.Label
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents lblTransactionType As System.Windows.Forms.Label
    Public WithEvents lblCommissionband As System.Windows.Forms.Label
    Public WithEvents lblParty As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents _lvwCommissionRate_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwCommissionRate As System.Windows.Forms.ListView
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cboPartyType As System.Windows.Forms.ComboBox
    Public WithEvents cboProduct As System.Windows.Forms.ComboBox
    Public WithEvents cboCommissionGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboRiskType As System.Windows.Forms.ComboBox
    Public WithEvents cboTransactionType As System.Windows.Forms.ComboBox
    Public WithEvents cboCommissionband As System.Windows.Forms.ComboBox
    Public WithEvents cboParty As System.Windows.Forms.ComboBox
    Public WithEvents cboTaxGroup As System.Windows.Forms.ComboBox
    Public WithEvents cmdFindParty As System.Windows.Forms.Button
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtMaximumRate1 = New System.Windows.Forms.TextBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.txtRate1 = New System.Windows.Forms.TextBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.cboCommissionLevel = New System.Windows.Forms.ComboBox
        Me.lblCommissionLevel = New System.Windows.Forms.Label
        Me.lblPartyType = New System.Windows.Forms.Label
        Me.lblProduct = New System.Windows.Forms.Label
        Me.lblCommissionGroup = New System.Windows.Forms.Label
        Me.lblRiskType = New System.Windows.Forms.Label
        Me.lblTransactionType = New System.Windows.Forms.Label
        Me.lblCommissionband = New System.Windows.Forms.Label
        Me.lblParty = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lvwCommissionRate = New System.Windows.Forms.ListView
        Me._lvwCommissionRate_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_16 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_17 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_18 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_19 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_20 = New System.Windows.Forms.ColumnHeader
        Me._lvwCommissionRate_ColumnHeader_21 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cboPartyType = New System.Windows.Forms.ComboBox
        Me.cboProduct = New System.Windows.Forms.ComboBox
        Me.cboCommissionGroup = New System.Windows.Forms.ComboBox
        Me.cboRiskType = New System.Windows.Forms.ComboBox
        Me.cboTransactionType = New System.Windows.Forms.ComboBox
        Me.cboCommissionband = New System.Windows.Forms.ComboBox
        Me.cboParty = New System.Windows.Forms.ComboBox
        Me.cboTaxGroup = New System.Windows.Forms.ComboBox
        Me.cmdFindParty = New System.Windows.Forms.Button
        Me._lvwCommissionRate_ColumnHeader_22 = New System.Windows.Forms.ColumnHeader
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMaximumRate1
        '
        Me.txtMaximumRate1.AcceptsReturn = True
        Me.txtMaximumRate1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaximumRate1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaximumRate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaximumRate1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaximumRate1.Location = New System.Drawing.Point(258, 376)
        Me.txtMaximumRate1.MaxLength = 0
        Me.txtMaximumRate1.Name = "txtMaximumRate1"
        Me.txtMaximumRate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaximumRate1.Size = New System.Drawing.Size(97, 20)
        Me.txtMaximumRate1.TabIndex = 27
        Me.txtMaximumRate1.Visible = False
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(24, 376)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(65, 20)
        Me.txtDate.TabIndex = 20
        Me.txtDate.Text = "Text1"
        Me.txtDate.Visible = False
        '
        'txtRate1
        '
        Me.txtRate1.AcceptsReturn = True
        Me.txtRate1.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate1.Location = New System.Drawing.Point(144, 376)
        Me.txtRate1.MaxLength = 0
        Me.txtRate1.Name = "txtRate1"
        Me.txtRate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate1.Size = New System.Drawing.Size(97, 20)
        Me.txtRate1.TabIndex = 21
        Me.txtRate1.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(390, 376)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 22
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
        Me.cmdHelp.Location = New System.Drawing.Point(546, 376)
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
        Me.cmdCancel.Location = New System.Drawing.Point(468, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 23
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(610, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(615, 365)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionLevel)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionLevel)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblPartyType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblProduct)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblTransactionType)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblCommissionband)
        Me._SSTab1_TabPage0.Controls.Add(Me.lblParty)
        Me._SSTab1_TabPage0.Controls.Add(Me.Label1)
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwCommissionRate)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdAdd)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdDelete)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdEdit)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboPartyType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboProduct)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRiskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboTransactionType)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboCommissionband)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboParty)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboTaxGroup)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdFindParty)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(607, 339)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - General"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'cboCommissionLevel
        '
        Me.cboCommissionLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionLevel.Location = New System.Drawing.Point(136, 153)
        Me.cboCommissionLevel.Name = "cboCommissionLevel"
        Me.cboCommissionLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionLevel.Size = New System.Drawing.Size(169, 21)
        Me.cboCommissionLevel.TabIndex = 28
        '
        'lblCommissionLevel
        '
        Me.lblCommissionLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionLevel.Location = New System.Drawing.Point(16, 157)
        Me.lblCommissionLevel.Name = "lblCommissionLevel"
        Me.lblCommissionLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionLevel.Size = New System.Drawing.Size(113, 17)
        Me.lblCommissionLevel.TabIndex = 27
        Me.lblCommissionLevel.Text = "Commission Level:"
        '
        'lblPartyType
        '
        Me.lblPartyType.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyType.Location = New System.Drawing.Point(16, 31)
        Me.lblPartyType.Name = "lblPartyType"
        Me.lblPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyType.Size = New System.Drawing.Size(65, 17)
        Me.lblPartyType.TabIndex = 1
        Me.lblPartyType.Text = "Party type:"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(16, 95)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(65, 17)
        Me.lblProduct.TabIndex = 9
        Me.lblProduct.Text = "Product:"
        '
        'lblCommissionGroup
        '
        Me.lblCommissionGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionGroup.Location = New System.Drawing.Point(16, 63)
        Me.lblCommissionGroup.Name = "lblCommissionGroup"
        Me.lblCommissionGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionGroup.Size = New System.Drawing.Size(114, 18)
        Me.lblCommissionGroup.TabIndex = 6
        Me.lblCommissionGroup.Text = "Commission Group:"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(320, 31)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(81, 17)
        Me.lblRiskType.TabIndex = 3
        Me.lblRiskType.Text = "Risk type:"
        '
        'lblTransactionType
        '
        Me.lblTransactionType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransactionType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransactionType.Location = New System.Drawing.Point(320, 63)
        Me.lblTransactionType.Name = "lblTransactionType"
        Me.lblTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransactionType.Size = New System.Drawing.Size(105, 17)
        Me.lblTransactionType.TabIndex = 7
        Me.lblTransactionType.Text = "Transaction type:"
        '
        'lblCommissionband
        '
        Me.lblCommissionband.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionband.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionband.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionband.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionband.Location = New System.Drawing.Point(320, 95)
        Me.lblCommissionband.Name = "lblCommissionband"
        Me.lblCommissionband.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionband.Size = New System.Drawing.Size(113, 17)
        Me.lblCommissionband.TabIndex = 12
        Me.lblCommissionband.Text = "Commission band:"
        '
        'lblParty
        '
        Me.lblParty.BackColor = System.Drawing.SystemColors.Control
        Me.lblParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParty.Location = New System.Drawing.Point(16, 124)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParty.Size = New System.Drawing.Size(57, 17)
        Me.lblParty.TabIndex = 13
        Me.lblParty.Text = "Party:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(320, 127)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(113, 17)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Tax Group:"
        '
        'lvwCommissionRate
        '
        Me.lvwCommissionRate.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCommissionRate.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCommissionRate_ColumnHeader_1, Me._lvwCommissionRate_ColumnHeader_2, Me._lvwCommissionRate_ColumnHeader_3, Me._lvwCommissionRate_ColumnHeader_4, Me._lvwCommissionRate_ColumnHeader_5, Me._lvwCommissionRate_ColumnHeader_6, Me._lvwCommissionRate_ColumnHeader_7, Me._lvwCommissionRate_ColumnHeader_8, Me._lvwCommissionRate_ColumnHeader_9, Me._lvwCommissionRate_ColumnHeader_10, Me._lvwCommissionRate_ColumnHeader_11, Me._lvwCommissionRate_ColumnHeader_12, Me._lvwCommissionRate_ColumnHeader_13, Me._lvwCommissionRate_ColumnHeader_14, Me._lvwCommissionRate_ColumnHeader_15, Me._lvwCommissionRate_ColumnHeader_16, Me._lvwCommissionRate_ColumnHeader_17, Me._lvwCommissionRate_ColumnHeader_18, Me._lvwCommissionRate_ColumnHeader_19, Me._lvwCommissionRate_ColumnHeader_20, Me._lvwCommissionRate_ColumnHeader_21, Me._lvwCommissionRate_ColumnHeader_22})
        Me.lvwCommissionRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCommissionRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCommissionRate.FullRowSelect = True
        Me.lvwCommissionRate.Location = New System.Drawing.Point(8, 177)
        Me.lvwCommissionRate.Name = "lvwCommissionRate"
        Me.lvwCommissionRate.Size = New System.Drawing.Size(593, 124)
        Me.lvwCommissionRate.SmallImageList = Me.ImageList1
        Me.lvwCommissionRate.TabIndex = 16
        Me.lvwCommissionRate.UseCompatibleStateImageBehavior = False
        Me.lvwCommissionRate.View = System.Windows.Forms.View.Details
        '
        '_lvwCommissionRate_ColumnHeader_1
        '
        Me._lvwCommissionRate_ColumnHeader_1.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_1.Text = "Party type"
        Me._lvwCommissionRate_ColumnHeader_1.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_2
        '
        Me._lvwCommissionRate_ColumnHeader_2.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_2.Text = "Party"
        Me._lvwCommissionRate_ColumnHeader_2.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_3
        '
        Me._lvwCommissionRate_ColumnHeader_3.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_3.Text = "Product"
        Me._lvwCommissionRate_ColumnHeader_3.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_4
        '
        Me._lvwCommissionRate_ColumnHeader_4.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_4.Text = "Risk Type"
        Me._lvwCommissionRate_ColumnHeader_4.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_5
        '
        Me._lvwCommissionRate_ColumnHeader_5.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_5.Text = "Transaction Type"
        Me._lvwCommissionRate_ColumnHeader_5.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_6
        '
        Me._lvwCommissionRate_ColumnHeader_6.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_6.Text = "Commission band"
        Me._lvwCommissionRate_ColumnHeader_6.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_7
        '
        Me._lvwCommissionRate_ColumnHeader_7.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_7.Text = "Commission Group"
        Me._lvwCommissionRate_ColumnHeader_7.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_8
        '
        Me._lvwCommissionRate_ColumnHeader_8.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_8.Text = "Rate"
        Me._lvwCommissionRate_ColumnHeader_8.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_9
        '
        Me._lvwCommissionRate_ColumnHeader_9.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_9.Text = "Is Value "
        Me._lvwCommissionRate_ColumnHeader_9.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_10
        '
        Me._lvwCommissionRate_ColumnHeader_10.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_10.Text = "EffectiveDate"
        Me._lvwCommissionRate_ColumnHeader_10.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_11
        '
        Me._lvwCommissionRate_ColumnHeader_11.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_11.Text = "Party_type_id"
        Me._lvwCommissionRate_ColumnHeader_11.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_12
        '
        Me._lvwCommissionRate_ColumnHeader_12.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_12.Text = "Party_id"
        Me._lvwCommissionRate_ColumnHeader_12.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_13
        '
        Me._lvwCommissionRate_ColumnHeader_13.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_13.Text = "Product_id"
        Me._lvwCommissionRate_ColumnHeader_13.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_14
        '
        Me._lvwCommissionRate_ColumnHeader_14.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_14.Text = "risk_type_id"
        Me._lvwCommissionRate_ColumnHeader_14.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_15
        '
        Me._lvwCommissionRate_ColumnHeader_15.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_15.Text = "Transaction_type_id"
        Me._lvwCommissionRate_ColumnHeader_15.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_16
        '
        Me._lvwCommissionRate_ColumnHeader_16.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_16.Text = "commission_band_id"
        Me._lvwCommissionRate_ColumnHeader_16.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_17
        '
        Me._lvwCommissionRate_ColumnHeader_17.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_17.Text = "commission_group_id"
        Me._lvwCommissionRate_ColumnHeader_17.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_18
        '
        Me._lvwCommissionRate_ColumnHeader_18.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_18.Text = "tax_group_id"
        Me._lvwCommissionRate_ColumnHeader_18.Width = 0
        '
        '_lvwCommissionRate_ColumnHeader_19
        '
        Me._lvwCommissionRate_ColumnHeader_19.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_19.Text = "Tax Group"
        Me._lvwCommissionRate_ColumnHeader_19.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_20
        '
        Me._lvwCommissionRate_ColumnHeader_20.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_20.Text = "Maximum Rate"
        Me._lvwCommissionRate_ColumnHeader_20.Width = 97
        '
        '_lvwCommissionRate_ColumnHeader_21
        '
        Me._lvwCommissionRate_ColumnHeader_21.Tag = ""
        Me._lvwCommissionRate_ColumnHeader_21.Text = "commission_level_id"
        Me._lvwCommissionRate_ColumnHeader_21.Width = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(366, 308)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 17
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(526, 308)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 19
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
        Me.cmdEdit.Location = New System.Drawing.Point(446, 308)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 18
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cboPartyType
        '
        Me.cboPartyType.BackColor = System.Drawing.SystemColors.Window
        Me.cboPartyType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPartyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPartyType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPartyType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPartyType.Location = New System.Drawing.Point(136, 28)
        Me.cboPartyType.Name = "cboPartyType"
        Me.cboPartyType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPartyType.Size = New System.Drawing.Size(169, 21)
        Me.cboPartyType.TabIndex = 2
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(136, 92)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(169, 21)
        Me.cboProduct.TabIndex = 10
        '
        'cboCommissionGroup
        '
        Me.cboCommissionGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionGroup.Enabled = False
        Me.cboCommissionGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionGroup.Location = New System.Drawing.Point(136, 60)
        Me.cboCommissionGroup.Name = "cboCommissionGroup"
        Me.cboCommissionGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboCommissionGroup.TabIndex = 5
        '
        'cboRiskType
        '
        Me.cboRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRiskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRiskType.Location = New System.Drawing.Point(432, 28)
        Me.cboRiskType.Name = "cboRiskType"
        Me.cboRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRiskType.Size = New System.Drawing.Size(169, 21)
        Me.cboRiskType.TabIndex = 4
        '
        'cboTransactionType
        '
        Me.cboTransactionType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransactionType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransactionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTransactionType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTransactionType.Location = New System.Drawing.Point(432, 60)
        Me.cboTransactionType.Name = "cboTransactionType"
        Me.cboTransactionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransactionType.Size = New System.Drawing.Size(169, 21)
        Me.cboTransactionType.TabIndex = 8
        '
        'cboCommissionband
        '
        Me.cboCommissionband.BackColor = System.Drawing.SystemColors.Window
        Me.cboCommissionband.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCommissionband.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCommissionband.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCommissionband.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCommissionband.Location = New System.Drawing.Point(432, 92)
        Me.cboCommissionband.Name = "cboCommissionband"
        Me.cboCommissionband.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCommissionband.Size = New System.Drawing.Size(169, 21)
        Me.cboCommissionband.TabIndex = 11
        '
        'cboParty
        '
        Me.cboParty.BackColor = System.Drawing.SystemColors.Window
        Me.cboParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboParty.Location = New System.Drawing.Point(136, 124)
        Me.cboParty.Name = "cboParty"
        Me.cboParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboParty.Size = New System.Drawing.Size(145, 21)
        Me.cboParty.TabIndex = 14
        '
        'cboTaxGroup
        '
        Me.cboTaxGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaxGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaxGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaxGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaxGroup.Location = New System.Drawing.Point(432, 124)
        Me.cboTaxGroup.Name = "cboTaxGroup"
        Me.cboTaxGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaxGroup.Size = New System.Drawing.Size(169, 21)
        Me.cboTaxGroup.TabIndex = 25
        '
        'cmdFindParty
        '
        Me.cmdFindParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindParty.Location = New System.Drawing.Point(280, 124)
        Me.cmdFindParty.Name = "cmdFindParty"
        Me.cmdFindParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindParty.Size = New System.Drawing.Size(24, 19)
        Me.cmdFindParty.TabIndex = 26
        Me.cmdFindParty.Text = "..."
        Me.cmdFindParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindParty.UseVisualStyleBackColor = False
        '
        '_lvwCommissionRate_ColumnHeader_22
        '
        Me._lvwCommissionRate_ColumnHeader_22.Text = "Commission Level"
        Me._lvwCommissionRate_ColumnHeader_22.Width = 95
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(626, 404)
        Me.Controls.Add(Me.txtMaximumRate1)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.txtRate1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Commission Rate Maintenance"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwCommissionRate_InitializeColumnKeys()
        Me._lvwCommissionRate_ColumnHeader_1.Name = ""
        Me._lvwCommissionRate_ColumnHeader_2.Name = ""
        Me._lvwCommissionRate_ColumnHeader_3.Name = ""
        Me._lvwCommissionRate_ColumnHeader_4.Name = ""
        Me._lvwCommissionRate_ColumnHeader_5.Name = ""
        Me._lvwCommissionRate_ColumnHeader_6.Name = ""
        Me._lvwCommissionRate_ColumnHeader_7.Name = ""
        Me._lvwCommissionRate_ColumnHeader_8.Name = ""
        Me._lvwCommissionRate_ColumnHeader_9.Name = ""
        Me._lvwCommissionRate_ColumnHeader_10.Name = ""
        Me._lvwCommissionRate_ColumnHeader_11.Name = ""
        Me._lvwCommissionRate_ColumnHeader_12.Name = ""
        Me._lvwCommissionRate_ColumnHeader_13.Name = ""
        Me._lvwCommissionRate_ColumnHeader_14.Name = ""
        Me._lvwCommissionRate_ColumnHeader_15.Name = ""
        Me._lvwCommissionRate_ColumnHeader_16.Name = ""
        Me._lvwCommissionRate_ColumnHeader_17.Name = ""
        Me._lvwCommissionRate_ColumnHeader_18.Name = ""
        Me._lvwCommissionRate_ColumnHeader_19.Name = ""
        Me._lvwCommissionRate_ColumnHeader_20.Name = ""
        Me._lvwCommissionRate_ColumnHeader_21.Name = ""
    End Sub
    Public WithEvents cboCommissionLevel As System.Windows.Forms.ComboBox
    Public WithEvents lblCommissionLevel As System.Windows.Forms.Label
    Friend WithEvents _lvwCommissionRate_ColumnHeader_21 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwCommissionRate_ColumnHeader_22 As System.Windows.Forms.ColumnHeader
#End Region
End Class