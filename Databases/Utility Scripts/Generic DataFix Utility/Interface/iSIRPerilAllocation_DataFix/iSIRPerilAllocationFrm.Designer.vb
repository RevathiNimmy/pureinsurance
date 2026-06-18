<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtGrossTotal As System.Windows.Forms.TextBox
	Public WithEvents txtTaxTotal As System.Windows.Forms.TextBox
	Public WithEvents txtFeeTotal As System.Windows.Forms.TextBox
	Public WithEvents txtNetTotal As System.Windows.Forms.TextBox
	Public WithEvents txtCurrencyDesc As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents panPolicyHolderFull As System.Windows.Forms.TextBox
	Public WithEvents panPolicyHolder As System.Windows.Forms.TextBox
	Public WithEvents panPolicyRef As System.Windows.Forms.TextBox
	Public WithEvents panPolicyDesc As System.Windows.Forms.TextBox
	Private WithEvents _lvwOriginal_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwOriginal_ColumnHeader_21 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwOriginal As System.Windows.Forms.ListView
	Public WithEvents fraOriginal As System.Windows.Forms.GroupBox
	Public WithEvents cmdUpSection As System.Windows.Forms.Button
	Public WithEvents cmdDownSection As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _lvwRatingSection_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRatingSection_ColumnHeader_21 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRatingSection As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents lblMove As System.Windows.Forms.Label
	Public WithEvents fraRatingSection As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents uctPMUFees1 As uctPMUFeesControl.uctPMUFees
	Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents uctPMURITax1 As uctPMURITaxControl.uctPMURITax
	Private WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents txtOldAnnPremNet As System.Windows.Forms.TextBox
	Public WithEvents txtNewAnnPremNet As System.Windows.Forms.TextBox
	Public WithEvents txtNewPremiumNet As System.Windows.Forms.TextBox
	Public WithEvents txtPremiumDueNet As System.Windows.Forms.TextBox
	Public WithEvents txtOldPremiumNet As System.Windows.Forms.TextBox
	Public WithEvents lblOldPremium As System.Windows.Forms.Label
	Public WithEvents lblNewPremium As System.Windows.Forms.Label
	Public WithEvents lblOldAnnualPremium As System.Windows.Forms.Label
	Public WithEvents lblNewAnnualPremium As System.Windows.Forms.Label
	Public WithEvents lblPremiumDue As System.Windows.Forms.Label
	Public WithEvents fraSummary As System.Windows.Forms.GroupBox
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Private WithEvents _StatusBar1_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents StatusBar1 As System.Windows.Forms.StatusStrip
	Public WithEvents lblGrossTotal As System.Windows.Forms.Label
	Public WithEvents lblTaxTotal As System.Windows.Forms.Label
	Public WithEvents lblFeeTotal As System.Windows.Forms.Label
	Public WithEvents lblNetTotal As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents lblPolicyRef As System.Windows.Forms.Label
	Public WithEvents lblPolicyHolderShort As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtGrossTotal = New System.Windows.Forms.TextBox()
        Me.txtTaxTotal = New System.Windows.Forms.TextBox()
        Me.txtFeeTotal = New System.Windows.Forms.TextBox()
        Me.txtNetTotal = New System.Windows.Forms.TextBox()
        Me.txtCurrencyDesc = New System.Windows.Forms.TextBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.panPolicyHolderFull = New System.Windows.Forms.TextBox()
        Me.panPolicyHolder = New System.Windows.Forms.TextBox()
        Me.panPolicyRef = New System.Windows.Forms.TextBox()
        Me.panPolicyDesc = New System.Windows.Forms.TextBox()
        Me.SSTab1 = New System.Windows.Forms.TabControl()
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraOriginal = New System.Windows.Forms.GroupBox()
        Me.lvwOriginal = New System.Windows.Forms.ListView()
        Me._lvwOriginal_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_20 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwOriginal_ColumnHeader_21 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraRatingSection = New System.Windows.Forms.GroupBox()
        Me.cmdUpSection = New System.Windows.Forms.Button()
        Me.cmdDownSection = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.lvwRatingSection = New System.Windows.Forms.ListView()
        Me._lvwRatingSection_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_20 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRatingSection_ColumnHeader_21 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.lblMove = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.uctPMUFees1 = New uctPMUFeesControl.uctPMUFees()
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage()
        Me.uctPMURITax1 = New uctPMURITaxControl.uctPMURITax()
        Me.fraSummary = New System.Windows.Forms.GroupBox()
        Me.txtOldAnnPremNet = New System.Windows.Forms.TextBox()
        Me.txtNewAnnPremNet = New System.Windows.Forms.TextBox()
        Me.txtNewPremiumNet = New System.Windows.Forms.TextBox()
        Me.txtPremiumDueNet = New System.Windows.Forms.TextBox()
        Me.txtOldPremiumNet = New System.Windows.Forms.TextBox()
        Me.lblOldPremium = New System.Windows.Forms.Label()
        Me.lblNewPremium = New System.Windows.Forms.Label()
        Me.lblOldAnnualPremium = New System.Windows.Forms.Label()
        Me.lblNewAnnualPremium = New System.Windows.Forms.Label()
        Me.lblPremiumDue = New System.Windows.Forms.Label()
        Me.cboCurrency = New UserControls.CurrencyLookup()
        Me.txtCurrency = New System.Windows.Forms.TextBox()
        Me.StatusBar1 = New System.Windows.Forms.StatusStrip()
        Me._StatusBar1_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblGrossTotal = New System.Windows.Forms.Label()
        Me.lblTaxTotal = New System.Windows.Forms.Label()
        Me.lblFeeTotal = New System.Windows.Forms.Label()
        Me.lblNetTotal = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.lblPolicyRef = New System.Windows.Forms.Label()
        Me.lblPolicyHolderShort = New System.Windows.Forms.Label()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.fraOriginal.SuspendLayout()
        Me.fraRatingSection.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.fraSummary.SuspendLayout()
        Me.StatusBar1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtGrossTotal
        '
        Me.txtGrossTotal.AcceptsReturn = True
        Me.txtGrossTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtGrossTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGrossTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGrossTotal.Location = New System.Drawing.Point(648, 440)
        Me.txtGrossTotal.MaxLength = 0
        Me.txtGrossTotal.Name = "txtGrossTotal"
        Me.txtGrossTotal.ReadOnly = True
        Me.txtGrossTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGrossTotal.Size = New System.Drawing.Size(105, 20)
        Me.txtGrossTotal.TabIndex = 18
        Me.txtGrossTotal.TabStop = False
        '
        'txtTaxTotal
        '
        Me.txtTaxTotal.AcceptsReturn = True
        Me.txtTaxTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxTotal.Location = New System.Drawing.Point(536, 440)
        Me.txtTaxTotal.MaxLength = 0
        Me.txtTaxTotal.Name = "txtTaxTotal"
        Me.txtTaxTotal.ReadOnly = True
        Me.txtTaxTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxTotal.Size = New System.Drawing.Size(105, 20)
        Me.txtTaxTotal.TabIndex = 17
        Me.txtTaxTotal.TabStop = False
        '
        'txtFeeTotal
        '
        Me.txtFeeTotal.AcceptsReturn = True
        Me.txtFeeTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtFeeTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeTotal.Location = New System.Drawing.Point(424, 440)
        Me.txtFeeTotal.MaxLength = 0
        Me.txtFeeTotal.Name = "txtFeeTotal"
        Me.txtFeeTotal.ReadOnly = True
        Me.txtFeeTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeTotal.Size = New System.Drawing.Size(105, 20)
        Me.txtFeeTotal.TabIndex = 16
        Me.txtFeeTotal.TabStop = False
        '
        'txtNetTotal
        '
        Me.txtNetTotal.AcceptsReturn = True
        Me.txtNetTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtNetTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNetTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNetTotal.Location = New System.Drawing.Point(312, 440)
        Me.txtNetTotal.MaxLength = 0
        Me.txtNetTotal.Name = "txtNetTotal"
        Me.txtNetTotal.ReadOnly = True
        Me.txtNetTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNetTotal.Size = New System.Drawing.Size(105, 20)
        Me.txtNetTotal.TabIndex = 15
        Me.txtNetTotal.TabStop = False
        '
        'txtCurrencyDesc
        '
        Me.txtCurrencyDesc.AcceptsReturn = True
        Me.txtCurrencyDesc.BackColor = System.Drawing.SystemColors.Control
        Me.txtCurrencyDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyDesc.Location = New System.Drawing.Point(16, 440)
        Me.txtCurrencyDesc.MaxLength = 0
        Me.txtCurrencyDesc.Name = "txtCurrencyDesc"
        Me.txtCurrencyDesc.ReadOnly = True
        Me.txtCurrencyDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyDesc.Size = New System.Drawing.Size(177, 20)
        Me.txtCurrencyDesc.TabIndex = 14
        Me.txtCurrencyDesc.TabStop = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(496, 469)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        Me.cmdCancel.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(669, 469)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 21
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(584, 469)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'panPolicyHolderFull
        '
        Me.panPolicyHolderFull.AcceptsReturn = True
        Me.panPolicyHolderFull.BackColor = System.Drawing.SystemColors.Control
        Me.panPolicyHolderFull.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.panPolicyHolderFull.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPolicyHolderFull.ForeColor = System.Drawing.SystemColors.WindowText
        Me.panPolicyHolderFull.Location = New System.Drawing.Point(259, 9)
        Me.panPolicyHolderFull.MaxLength = 0
        Me.panPolicyHolderFull.Name = "panPolicyHolderFull"
        Me.panPolicyHolderFull.ReadOnly = True
        Me.panPolicyHolderFull.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPolicyHolderFull.Size = New System.Drawing.Size(498, 20)
        Me.panPolicyHolderFull.TabIndex = 1
        Me.panPolicyHolderFull.TabStop = False
        '
        'panPolicyHolder
        '
        Me.panPolicyHolder.AcceptsReturn = True
        Me.panPolicyHolder.BackColor = System.Drawing.SystemColors.Control
        Me.panPolicyHolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.panPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPolicyHolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.panPolicyHolder.Location = New System.Drawing.Point(98, 9)
        Me.panPolicyHolder.MaxLength = 0
        Me.panPolicyHolder.Name = "panPolicyHolder"
        Me.panPolicyHolder.ReadOnly = True
        Me.panPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPolicyHolder.Size = New System.Drawing.Size(154, 20)
        Me.panPolicyHolder.TabIndex = 0
        Me.panPolicyHolder.TabStop = False
        '
        'panPolicyRef
        '
        Me.panPolicyRef.AcceptsReturn = True
        Me.panPolicyRef.BackColor = System.Drawing.SystemColors.Control
        Me.panPolicyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.panPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPolicyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.panPolicyRef.Location = New System.Drawing.Point(98, 35)
        Me.panPolicyRef.MaxLength = 0
        Me.panPolicyRef.Name = "panPolicyRef"
        Me.panPolicyRef.ReadOnly = True
        Me.panPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPolicyRef.Size = New System.Drawing.Size(154, 20)
        Me.panPolicyRef.TabIndex = 2
        Me.panPolicyRef.TabStop = False
        '
        'panPolicyDesc
        '
        Me.panPolicyDesc.AcceptsReturn = True
        Me.panPolicyDesc.BackColor = System.Drawing.SystemColors.Control
        Me.panPolicyDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.panPolicyDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panPolicyDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.panPolicyDesc.Location = New System.Drawing.Point(259, 35)
        Me.panPolicyDesc.MaxLength = 0
        Me.panPolicyDesc.Name = "panPolicyDesc"
        Me.panPolicyDesc.ReadOnly = True
        Me.panPolicyDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.panPolicyDesc.Size = New System.Drawing.Size(498, 20)
        Me.panPolicyDesc.TabIndex = 3
        Me.panPolicyDesc.TabStop = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(249, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 64)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(754, 355)
        Me.SSTab1.TabIndex = 4
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.fraOriginal)
        Me._SSTab1_TabPage0.Controls.Add(Me.fraRatingSection)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(746, 329)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - General"
        '
        'fraOriginal
        '
        Me.fraOriginal.BackColor = System.Drawing.SystemColors.Control
        Me.fraOriginal.Controls.Add(Me.lvwOriginal)
        Me.fraOriginal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOriginal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOriginal.Location = New System.Drawing.Point(8, 2)
        Me.fraOriginal.Name = "fraOriginal"
        Me.fraOriginal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOriginal.Size = New System.Drawing.Size(737, 145)
        Me.fraOriginal.TabIndex = 37
        Me.fraOriginal.TabStop = False
        Me.fraOriginal.Text = "Original"
        '
        'lvwOriginal
        '
        Me.lvwOriginal.BackColor = System.Drawing.SystemColors.Window
        Me.lvwOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwOriginal, "")
        Me.lvwOriginal.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwOriginal_ColumnHeader_1, Me._lvwOriginal_ColumnHeader_2, Me._lvwOriginal_ColumnHeader_3, Me._lvwOriginal_ColumnHeader_4, Me._lvwOriginal_ColumnHeader_5, Me._lvwOriginal_ColumnHeader_6, Me._lvwOriginal_ColumnHeader_7, Me._lvwOriginal_ColumnHeader_8, Me._lvwOriginal_ColumnHeader_9, Me._lvwOriginal_ColumnHeader_10, Me._lvwOriginal_ColumnHeader_11, Me._lvwOriginal_ColumnHeader_12, Me._lvwOriginal_ColumnHeader_13, Me._lvwOriginal_ColumnHeader_14, Me._lvwOriginal_ColumnHeader_15, Me._lvwOriginal_ColumnHeader_16, Me._lvwOriginal_ColumnHeader_17, Me._lvwOriginal_ColumnHeader_18, Me._lvwOriginal_ColumnHeader_19, Me._lvwOriginal_ColumnHeader_20, Me._lvwOriginal_ColumnHeader_21})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwOriginal, True)
        Me.lvwOriginal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwOriginal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwOriginal.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwOriginal, "lvwOriginal_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwOriginal, "")
        Me.lvwOriginal.Location = New System.Drawing.Point(8, 16)
        Me.lvwOriginal.MultiSelect = False
        Me.lvwOriginal.Name = "lvwOriginal"
        Me.lvwOriginal.Size = New System.Drawing.Size(681, 121)
        Me.listViewHelper1.SetSmallIcons(Me.lvwOriginal, "")
        Me.listViewHelper1.SetSorted(Me.lvwOriginal, False)
        Me.listViewHelper1.SetSortKey(Me.lvwOriginal, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwOriginal, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwOriginal.TabIndex = 5
        Me.lvwOriginal.UseCompatibleStateImageBehavior = False
        Me.lvwOriginal.View = System.Windows.Forms.View.Details
        '
        '_lvwOriginal_ColumnHeader_1
        '
        Me._lvwOriginal_ColumnHeader_1.Text = "Rating Section Type"
        Me._lvwOriginal_ColumnHeader_1.Width = 212
        '
        '_lvwOriginal_ColumnHeader_2
        '
        Me._lvwOriginal_ColumnHeader_2.Text = "Policy Section Type"
        Me._lvwOriginal_ColumnHeader_2.Width = 122
        '
        '_lvwOriginal_ColumnHeader_3
        '
        Me._lvwOriginal_ColumnHeader_3.Text = "Rate Type"
        Me._lvwOriginal_ColumnHeader_3.Width = 79
        '
        '_lvwOriginal_ColumnHeader_4
        '
        Me._lvwOriginal_ColumnHeader_4.Text = "Rate"
        Me._lvwOriginal_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwOriginal_ColumnHeader_4.Width = 51
        '
        '_lvwOriginal_ColumnHeader_5
        '
        Me._lvwOriginal_ColumnHeader_5.Text = "Sum Insured"
        Me._lvwOriginal_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwOriginal_ColumnHeader_5.Width = 110
        '
        '_lvwOriginal_ColumnHeader_6
        '
        Me._lvwOriginal_ColumnHeader_6.Text = "Premium"
        Me._lvwOriginal_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwOriginal_ColumnHeader_6.Width = 110
        '
        '_lvwOriginal_ColumnHeader_7
        '
        Me._lvwOriginal_ColumnHeader_7.Text = "This Premium"
        Me._lvwOriginal_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwOriginal_ColumnHeader_7.Width = 115
        '
        '_lvwOriginal_ColumnHeader_8
        '
        Me._lvwOriginal_ColumnHeader_8.Text = "Country"
        Me._lvwOriginal_ColumnHeader_8.Width = 97
        '
        '_lvwOriginal_ColumnHeader_9
        '
        Me._lvwOriginal_ColumnHeader_9.Text = "State"
        Me._lvwOriginal_ColumnHeader_9.Width = 97
        '
        '_lvwOriginal_ColumnHeader_10
        '
        Me._lvwOriginal_ColumnHeader_10.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_10.Width = 0
        '
        '_lvwOriginal_ColumnHeader_11
        '
        Me._lvwOriginal_ColumnHeader_11.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_11.Width = 0
        '
        '_lvwOriginal_ColumnHeader_12
        '
        Me._lvwOriginal_ColumnHeader_12.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_12.Width = 0
        '
        '_lvwOriginal_ColumnHeader_13
        '
        Me._lvwOriginal_ColumnHeader_13.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_13.Width = 0
        '
        '_lvwOriginal_ColumnHeader_14
        '
        Me._lvwOriginal_ColumnHeader_14.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_14.Width = 0
        '
        '_lvwOriginal_ColumnHeader_15
        '
        Me._lvwOriginal_ColumnHeader_15.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_15.Width = 0
        '
        '_lvwOriginal_ColumnHeader_16
        '
        Me._lvwOriginal_ColumnHeader_16.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_16.Width = 0
        '
        '_lvwOriginal_ColumnHeader_17
        '
        Me._lvwOriginal_ColumnHeader_17.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_17.Width = 0
        '
        '_lvwOriginal_ColumnHeader_18
        '
        Me._lvwOriginal_ColumnHeader_18.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_18.Width = 0
        '
        '_lvwOriginal_ColumnHeader_19
        '
        Me._lvwOriginal_ColumnHeader_19.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_19.Width = 0
        '
        '_lvwOriginal_ColumnHeader_20
        '
        Me._lvwOriginal_ColumnHeader_20.Text = "Blank"
        Me._lvwOriginal_ColumnHeader_20.Width = 0
        '
        '_lvwOriginal_ColumnHeader_21
        '
        Me._lvwOriginal_ColumnHeader_21.Width = 0
        '
        'fraRatingSection
        '
        Me.fraRatingSection.BackColor = System.Drawing.SystemColors.Control
        Me.fraRatingSection.Controls.Add(Me.cmdUpSection)
        Me.fraRatingSection.Controls.Add(Me.cmdDownSection)
        Me.fraRatingSection.Controls.Add(Me.cmdDelete)
        Me.fraRatingSection.Controls.Add(Me.cmdEdit)
        Me.fraRatingSection.Controls.Add(Me.lvwRatingSection)
        Me.fraRatingSection.Controls.Add(Me.cmdAdd)
        Me.fraRatingSection.Controls.Add(Me.lblMove)
        Me.fraRatingSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRatingSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRatingSection.Location = New System.Drawing.Point(8, 146)
        Me.fraRatingSection.Name = "fraRatingSection"
        Me.fraRatingSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRatingSection.Size = New System.Drawing.Size(737, 177)
        Me.fraRatingSection.TabIndex = 38
        Me.fraRatingSection.TabStop = False
        Me.fraRatingSection.Text = "Rating Sections"
        '
        'cmdUpSection
        '
        Me.cmdUpSection.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpSection.Image = CType(resources.GetObject("cmdUpSection.Image"), System.Drawing.Image)
        Me.cmdUpSection.Location = New System.Drawing.Point(705, 40)
        Me.cmdUpSection.Name = "cmdUpSection"
        Me.cmdUpSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpSection.Size = New System.Drawing.Size(17, 17)
        Me.cmdUpSection.TabIndex = 12
        Me.cmdUpSection.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdUpSection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUpSection.UseVisualStyleBackColor = False
        '
        'cmdDownSection
        '
        Me.cmdDownSection.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDownSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDownSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDownSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDownSection.Image = CType(resources.GetObject("cmdDownSection.Image"), System.Drawing.Image)
        Me.cmdDownSection.Location = New System.Drawing.Point(705, 88)
        Me.cmdDownSection.Name = "cmdDownSection"
        Me.cmdDownSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDownSection.Size = New System.Drawing.Size(17, 17)
        Me.cmdDownSection.TabIndex = 13
        Me.cmdDownSection.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdDownSection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDownSection.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(166, 147)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 23)
        Me.cmdDelete.TabIndex = 11
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
        Me.cmdEdit.Location = New System.Drawing.Point(88, 147)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 23)
        Me.cmdEdit.TabIndex = 10
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwRatingSection
        '
        Me.lvwRatingSection.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRatingSection.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwRatingSection, "")
        Me.lvwRatingSection.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRatingSection_ColumnHeader_1, Me._lvwRatingSection_ColumnHeader_2, Me._lvwRatingSection_ColumnHeader_3, Me._lvwRatingSection_ColumnHeader_4, Me._lvwRatingSection_ColumnHeader_5, Me._lvwRatingSection_ColumnHeader_6, Me._lvwRatingSection_ColumnHeader_7, Me._lvwRatingSection_ColumnHeader_8, Me._lvwRatingSection_ColumnHeader_9, Me._lvwRatingSection_ColumnHeader_10, Me._lvwRatingSection_ColumnHeader_11, Me._lvwRatingSection_ColumnHeader_12, Me._lvwRatingSection_ColumnHeader_13, Me._lvwRatingSection_ColumnHeader_14, Me._lvwRatingSection_ColumnHeader_15, Me._lvwRatingSection_ColumnHeader_16, Me._lvwRatingSection_ColumnHeader_17, Me._lvwRatingSection_ColumnHeader_18, Me._lvwRatingSection_ColumnHeader_19, Me._lvwRatingSection_ColumnHeader_20, Me._lvwRatingSection_ColumnHeader_21})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwRatingSection, True)
        Me.lvwRatingSection.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRatingSection.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRatingSection.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwRatingSection, "lvwRatingSection_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwRatingSection, "")
        Me.lvwRatingSection.Location = New System.Drawing.Point(8, 16)
        Me.lvwRatingSection.MultiSelect = False
        Me.lvwRatingSection.Name = "lvwRatingSection"
        Me.lvwRatingSection.Size = New System.Drawing.Size(681, 129)
        Me.listViewHelper1.SetSmallIcons(Me.lvwRatingSection, "")
        Me.listViewHelper1.SetSorted(Me.lvwRatingSection, False)
        Me.listViewHelper1.SetSortKey(Me.lvwRatingSection, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwRatingSection, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwRatingSection.TabIndex = 6
        Me.lvwRatingSection.UseCompatibleStateImageBehavior = False
        Me.lvwRatingSection.View = System.Windows.Forms.View.Details
        '
        '_lvwRatingSection_ColumnHeader_1
        '
        Me._lvwRatingSection_ColumnHeader_1.Text = "Rating Section Type"
        Me._lvwRatingSection_ColumnHeader_1.Width = 212
        '
        '_lvwRatingSection_ColumnHeader_2
        '
        Me._lvwRatingSection_ColumnHeader_2.Text = "Earning Pattern"
        Me._lvwRatingSection_ColumnHeader_2.Width = 122
        '
        '_lvwRatingSection_ColumnHeader_3
        '
        Me._lvwRatingSection_ColumnHeader_3.Text = "Rate Type"
        Me._lvwRatingSection_ColumnHeader_3.Width = 79
        '
        '_lvwRatingSection_ColumnHeader_4
        '
        Me._lvwRatingSection_ColumnHeader_4.Text = "Rate"
        Me._lvwRatingSection_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRatingSection_ColumnHeader_4.Width = 51
        '
        '_lvwRatingSection_ColumnHeader_5
        '
        Me._lvwRatingSection_ColumnHeader_5.Text = "Sum Insured"
        Me._lvwRatingSection_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRatingSection_ColumnHeader_5.Width = 110
        '
        '_lvwRatingSection_ColumnHeader_6
        '
        Me._lvwRatingSection_ColumnHeader_6.Text = "Premium"
        Me._lvwRatingSection_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRatingSection_ColumnHeader_6.Width = 110
        '
        '_lvwRatingSection_ColumnHeader_7
        '
        Me._lvwRatingSection_ColumnHeader_7.Text = "This Premium"
        Me._lvwRatingSection_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwRatingSection_ColumnHeader_7.Width = 115
        '
        '_lvwRatingSection_ColumnHeader_8
        '
        Me._lvwRatingSection_ColumnHeader_8.Text = "Country"
        Me._lvwRatingSection_ColumnHeader_8.Width = 97
        '
        '_lvwRatingSection_ColumnHeader_9
        '
        Me._lvwRatingSection_ColumnHeader_9.Text = "State"
        Me._lvwRatingSection_ColumnHeader_9.Width = 97
        '
        '_lvwRatingSection_ColumnHeader_10
        '
        Me._lvwRatingSection_ColumnHeader_10.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_10.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_11
        '
        Me._lvwRatingSection_ColumnHeader_11.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_11.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_12
        '
        Me._lvwRatingSection_ColumnHeader_12.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_12.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_13
        '
        Me._lvwRatingSection_ColumnHeader_13.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_13.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_14
        '
        Me._lvwRatingSection_ColumnHeader_14.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_14.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_15
        '
        Me._lvwRatingSection_ColumnHeader_15.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_15.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_16
        '
        Me._lvwRatingSection_ColumnHeader_16.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_16.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_17
        '
        Me._lvwRatingSection_ColumnHeader_17.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_17.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_18
        '
        Me._lvwRatingSection_ColumnHeader_18.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_18.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_19
        '
        Me._lvwRatingSection_ColumnHeader_19.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_19.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_20
        '
        Me._lvwRatingSection_ColumnHeader_20.Text = "Blank"
        Me._lvwRatingSection_ColumnHeader_20.Width = 0
        '
        '_lvwRatingSection_ColumnHeader_21
        '
        Me._lvwRatingSection_ColumnHeader_21.Width = 0
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(8, 147)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 23)
        Me.cmdAdd.TabIndex = 9
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lblMove
        '
        Me.lblMove.BackColor = System.Drawing.SystemColors.Control
        Me.lblMove.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMove.Location = New System.Drawing.Point(697, 64)
        Me.lblMove.Name = "lblMove"
        Me.lblMove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMove.Size = New System.Drawing.Size(33, 17)
        Me.lblMove.TabIndex = 39
        Me.lblMove.Text = "Move"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.uctPMUFees1)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(746, 329)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "2 - Risk / Peril Fees"
        '
        'uctPMUFees1
        '
        Me.uctPMUFees1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMUFees1.Location = New System.Drawing.Point(2, 4)
        Me.uctPMUFees1.Name = "uctPMUFees1"
        Me.uctPMUFees1.Size = New System.Drawing.Size(729, 321)
        Me.uctPMUFees1.TabIndex = 7
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.uctPMURITax1)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(746, 329)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "3 - Risk Tax"
        '
        'uctPMURITax1
        '
        Me.uctPMURITax1.CurrencyId = 0
        Me.uctPMURITax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMURITax1.Location = New System.Drawing.Point(8, 4)
        Me.uctPMURITax1.Name = "uctPMURITax1"
        Me.uctPMURITax1.Size = New System.Drawing.Size(735, 321)
        Me.uctPMURITax1.TabIndex = 8
        '
        'fraSummary
        '
        Me.fraSummary.BackColor = System.Drawing.SystemColors.Control
        Me.fraSummary.Controls.Add(Me.txtOldAnnPremNet)
        Me.fraSummary.Controls.Add(Me.txtNewAnnPremNet)
        Me.fraSummary.Controls.Add(Me.txtNewPremiumNet)
        Me.fraSummary.Controls.Add(Me.txtPremiumDueNet)
        Me.fraSummary.Controls.Add(Me.txtOldPremiumNet)
        Me.fraSummary.Controls.Add(Me.lblOldPremium)
        Me.fraSummary.Controls.Add(Me.lblNewPremium)
        Me.fraSummary.Controls.Add(Me.lblOldAnnualPremium)
        Me.fraSummary.Controls.Add(Me.lblNewAnnualPremium)
        Me.fraSummary.Controls.Add(Me.lblPremiumDue)
        Me.fraSummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSummary.Location = New System.Drawing.Point(16, 312)
        Me.fraSummary.Name = "fraSummary"
        Me.fraSummary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSummary.Size = New System.Drawing.Size(729, 75)
        Me.fraSummary.TabIndex = 24
        Me.fraSummary.TabStop = False
        Me.fraSummary.Text = "Premium Summary"
        Me.fraSummary.Visible = False
        '
        'txtOldAnnPremNet
        '
        Me.txtOldAnnPremNet.AcceptsReturn = True
        Me.txtOldAnnPremNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtOldAnnPremNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOldAnnPremNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldAnnPremNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOldAnnPremNet.Location = New System.Drawing.Point(147, 19)
        Me.txtOldAnnPremNet.MaxLength = 0
        Me.txtOldAnnPremNet.Name = "txtOldAnnPremNet"
        Me.txtOldAnnPremNet.ReadOnly = True
        Me.txtOldAnnPremNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOldAnnPremNet.Size = New System.Drawing.Size(105, 20)
        Me.txtOldAnnPremNet.TabIndex = 29
        Me.txtOldAnnPremNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNewAnnPremNet
        '
        Me.txtNewAnnPremNet.AcceptsReturn = True
        Me.txtNewAnnPremNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtNewAnnPremNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewAnnPremNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewAnnPremNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewAnnPremNet.Location = New System.Drawing.Point(147, 43)
        Me.txtNewAnnPremNet.MaxLength = 0
        Me.txtNewAnnPremNet.Name = "txtNewAnnPremNet"
        Me.txtNewAnnPremNet.ReadOnly = True
        Me.txtNewAnnPremNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewAnnPremNet.Size = New System.Drawing.Size(105, 20)
        Me.txtNewAnnPremNet.TabIndex = 28
        Me.txtNewAnnPremNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNewPremiumNet
        '
        Me.txtNewPremiumNet.AcceptsReturn = True
        Me.txtNewPremiumNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtNewPremiumNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewPremiumNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewPremiumNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewPremiumNet.Location = New System.Drawing.Point(362, 43)
        Me.txtNewPremiumNet.MaxLength = 0
        Me.txtNewPremiumNet.Name = "txtNewPremiumNet"
        Me.txtNewPremiumNet.ReadOnly = True
        Me.txtNewPremiumNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewPremiumNet.Size = New System.Drawing.Size(105, 20)
        Me.txtNewPremiumNet.TabIndex = 27
        Me.txtNewPremiumNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPremiumDueNet
        '
        Me.txtPremiumDueNet.AcceptsReturn = True
        Me.txtPremiumDueNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtPremiumDueNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPremiumDueNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPremiumDueNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPremiumDueNet.Location = New System.Drawing.Point(583, 43)
        Me.txtPremiumDueNet.MaxLength = 0
        Me.txtPremiumDueNet.Name = "txtPremiumDueNet"
        Me.txtPremiumDueNet.ReadOnly = True
        Me.txtPremiumDueNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPremiumDueNet.Size = New System.Drawing.Size(105, 20)
        Me.txtPremiumDueNet.TabIndex = 26
        Me.txtPremiumDueNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOldPremiumNet
        '
        Me.txtOldPremiumNet.AcceptsReturn = True
        Me.txtOldPremiumNet.BackColor = System.Drawing.SystemColors.Control
        Me.txtOldPremiumNet.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOldPremiumNet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPremiumNet.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOldPremiumNet.Location = New System.Drawing.Point(362, 19)
        Me.txtOldPremiumNet.MaxLength = 0
        Me.txtOldPremiumNet.Name = "txtOldPremiumNet"
        Me.txtOldPremiumNet.ReadOnly = True
        Me.txtOldPremiumNet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOldPremiumNet.Size = New System.Drawing.Size(105, 20)
        Me.txtOldPremiumNet.TabIndex = 25
        Me.txtOldPremiumNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblOldPremium
        '
        Me.lblOldPremium.AutoSize = True
        Me.lblOldPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblOldPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOldPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOldPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOldPremium.Location = New System.Drawing.Point(271, 21)
        Me.lblOldPremium.Name = "lblOldPremium"
        Me.lblOldPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOldPremium.Size = New System.Drawing.Size(69, 13)
        Me.lblOldPremium.TabIndex = 34
        Me.lblOldPremium.Text = "Old Premium:"
        '
        'lblNewPremium
        '
        Me.lblNewPremium.AutoSize = True
        Me.lblNewPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblNewPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewPremium.Location = New System.Drawing.Point(271, 45)
        Me.lblNewPremium.Name = "lblNewPremium"
        Me.lblNewPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewPremium.Size = New System.Drawing.Size(75, 13)
        Me.lblNewPremium.TabIndex = 33
        Me.lblNewPremium.Text = "New Premium:"
        '
        'lblOldAnnualPremium
        '
        Me.lblOldAnnualPremium.AutoSize = True
        Me.lblOldAnnualPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblOldAnnualPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOldAnnualPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOldAnnualPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOldAnnualPremium.Location = New System.Drawing.Point(10, 21)
        Me.lblOldAnnualPremium.Name = "lblOldAnnualPremium"
        Me.lblOldAnnualPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOldAnnualPremium.Size = New System.Drawing.Size(105, 13)
        Me.lblOldAnnualPremium.TabIndex = 32
        Me.lblOldAnnualPremium.Text = "Old Annual Premium:"
        '
        'lblNewAnnualPremium
        '
        Me.lblNewAnnualPremium.AutoSize = True
        Me.lblNewAnnualPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblNewAnnualPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewAnnualPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewAnnualPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewAnnualPremium.Location = New System.Drawing.Point(10, 45)
        Me.lblNewAnnualPremium.Name = "lblNewAnnualPremium"
        Me.lblNewAnnualPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewAnnualPremium.Size = New System.Drawing.Size(111, 13)
        Me.lblNewAnnualPremium.TabIndex = 31
        Me.lblNewAnnualPremium.Text = "New Annual Premium:"
        '
        'lblPremiumDue
        '
        Me.lblPremiumDue.AutoSize = True
        Me.lblPremiumDue.BackColor = System.Drawing.SystemColors.Control
        Me.lblPremiumDue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPremiumDue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPremiumDue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPremiumDue.Location = New System.Drawing.Point(489, 45)
        Me.lblPremiumDue.Name = "lblPremiumDue"
        Me.lblPremiumDue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPremiumDue.Size = New System.Drawing.Size(73, 13)
        Me.lblPremiumDue.TabIndex = 30
        Me.lblPremiumDue.Text = "Premium Due:"
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.Enabled = False
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(56, 304)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(129, 21)
        Me.cboCurrency.TabIndex = 36
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.Visible = False
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(16, 464)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(177, 20)
        Me.txtCurrency.TabIndex = 44
        Me.txtCurrency.Text = "Text1"
        Me.txtCurrency.Visible = False
        '
        'StatusBar1
        '
        Me.StatusBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar1_Panel1})
        Me.StatusBar1.Location = New System.Drawing.Point(0, 491)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.ShowItemToolTips = True
        Me.StatusBar1.Size = New System.Drawing.Size(760, 22)
        Me.StatusBar1.TabIndex = 45
        '
        '_StatusBar1_Panel1
        '
        Me._StatusBar1_Panel1.AutoSize = False
        Me._StatusBar1_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar1_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar1_Panel1.DoubleClickEnabled = True
        Me._StatusBar1_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar1_Panel1.Name = "_StatusBar1_Panel1"
        Me._StatusBar1_Panel1.Size = New System.Drawing.Size(760, 21)
        Me._StatusBar1_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblGrossTotal
        '
        Me.lblGrossTotal.AutoSize = True
        Me.lblGrossTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossTotal.Location = New System.Drawing.Point(648, 424)
        Me.lblGrossTotal.Name = "lblGrossTotal"
        Me.lblGrossTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossTotal.Size = New System.Drawing.Size(61, 13)
        Me.lblGrossTotal.TabIndex = 43
        Me.lblGrossTotal.Text = "Gross Total"
        '
        'lblTaxTotal
        '
        Me.lblTaxTotal.AutoSize = True
        Me.lblTaxTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxTotal.Location = New System.Drawing.Point(536, 424)
        Me.lblTaxTotal.Name = "lblTaxTotal"
        Me.lblTaxTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxTotal.Size = New System.Drawing.Size(52, 13)
        Me.lblTaxTotal.TabIndex = 42
        Me.lblTaxTotal.Text = "Tax Total"
        '
        'lblFeeTotal
        '
        Me.lblFeeTotal.AutoSize = True
        Me.lblFeeTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeTotal.Location = New System.Drawing.Point(424, 424)
        Me.lblFeeTotal.Name = "lblFeeTotal"
        Me.lblFeeTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeTotal.Size = New System.Drawing.Size(52, 13)
        Me.lblFeeTotal.TabIndex = 41
        Me.lblFeeTotal.Text = "Fee Total"
        '
        'lblNetTotal
        '
        Me.lblNetTotal.AutoSize = True
        Me.lblNetTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetTotal.Location = New System.Drawing.Point(312, 424)
        Me.lblNetTotal.Name = "lblNetTotal"
        Me.lblNetTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetTotal.Size = New System.Drawing.Size(51, 13)
        Me.lblNetTotal.TabIndex = 40
        Me.lblNetTotal.Text = "Net Total"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 424)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(49, 13)
        Me.lblCurrency.TabIndex = 35
        Me.lblCurrency.Text = "Currency"
        '
        'lblPolicyRef
        '
        Me.lblPolicyRef.AutoSize = True
        Me.lblPolicyRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyRef.Location = New System.Drawing.Point(10, 38)
        Me.lblPolicyRef.Name = "lblPolicyRef"
        Me.lblPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyRef.Size = New System.Drawing.Size(58, 13)
        Me.lblPolicyRef.TabIndex = 23
        Me.lblPolicyRef.Text = "Policy Ref:"
        '
        'lblPolicyHolderShort
        '
        Me.lblPolicyHolderShort.AutoSize = True
        Me.lblPolicyHolderShort.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyHolderShort.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyHolderShort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyHolderShort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyHolderShort.Location = New System.Drawing.Point(10, 12)
        Me.lblPolicyHolderShort.Name = "lblPolicyHolderShort"
        Me.lblPolicyHolderShort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyHolderShort.Size = New System.Drawing.Size(72, 13)
        Me.lblPolicyHolderShort.TabIndex = 22
        Me.lblPolicyHolderShort.Text = "Policy Holder:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(760, 513)
        Me.Controls.Add(Me.txtGrossTotal)
        Me.Controls.Add(Me.txtTaxTotal)
        Me.Controls.Add(Me.txtFeeTotal)
        Me.Controls.Add(Me.txtNetTotal)
        Me.Controls.Add(Me.txtCurrencyDesc)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.panPolicyHolderFull)
        Me.Controls.Add(Me.panPolicyHolder)
        Me.Controls.Add(Me.panPolicyRef)
        Me.Controls.Add(Me.panPolicyDesc)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.fraSummary)
        Me.Controls.Add(Me.cboCurrency)
        Me.Controls.Add(Me.txtCurrency)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.lblGrossTotal)
        Me.Controls.Add(Me.lblTaxTotal)
        Me.Controls.Add(Me.lblFeeTotal)
        Me.Controls.Add(Me.lblNetTotal)
        Me.Controls.Add(Me.lblCurrency)
        Me.Controls.Add(Me.lblPolicyRef)
        Me.Controls.Add(Me.lblPolicyHolderShort)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Peril Allocation"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.fraOriginal.ResumeLayout(False)
        Me.fraRatingSection.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.fraSummary.ResumeLayout(False)
        Me.fraSummary.PerformLayout()
        Me.StatusBar1.ResumeLayout(False)
        Me.StatusBar1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class