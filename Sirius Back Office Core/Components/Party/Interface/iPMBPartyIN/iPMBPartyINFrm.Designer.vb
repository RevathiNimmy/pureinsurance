<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Private WithEvents _Toolbar1_Button1 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button2 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button3 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _Toolbar1_Button4 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button5 As System.Windows.Forms.ToolStripButton
	Private WithEvents _Toolbar1_Button6 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _Toolbar1_Button7 As System.Windows.Forms.ToolStripButton
	Public WithEvents Toolbar1 As System.Windows.Forms.ToolStrip
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents txtBureauAccount As System.Windows.Forms.TextBox
	Public WithEvents cmdBureauAccount As System.Windows.Forms.Button
	Public WithEvents cmdStargate As System.Windows.Forms.Button
	Public WithEvents cmdIPTExtras As System.Windows.Forms.Button
	Public WithEvents cmdRates As System.Windows.Forms.Button
	Public WithEvents cmdPaymentGroups As System.Windows.Forms.Button
	Public WithEvents txtAgencyNumber As System.Windows.Forms.TextBox
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtIDReference As System.Windows.Forms.TextBox
	Public WithEvents cboInsurerType As PMLookupControl.cboPMLookup
	Public WithEvents lblInsurerType As System.Windows.Forms.Label
	Public WithEvents lblAgencyNumber As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblIDReference As System.Windows.Forms.Label
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents chkRTA As System.Windows.Forms.CheckBox
	Public WithEvents chkRTAEditable As System.Windows.Forms.CheckBox
	Public WithEvents txtDefaultCommissionRate As System.Windows.Forms.TextBox
	Public WithEvents cboInsurerStatus As PMLookupControl.cboPMLookup
	Public WithEvents cboInsurerCreditRating As PMLookupControl.cboPMLookup
	Public WithEvents lblReInsuranceType As System.Windows.Forms.Label
	Public WithEvents lblInsurerCreditRating As System.Windows.Forms.Label
	Public WithEvents lblCommission As System.Windows.Forms.Label
	Public WithEvents fraReinsurance As System.Windows.Forms.GroupBox
	Public WithEvents cboLocking As PMLookupControl.cboPMLookup
	Public WithEvents uctBranch As System.Windows.Forms.ComboBox
	Public WithEvents cboSubBranch As System.Windows.Forms.ComboBox
	Public WithEvents uctABINumber As PMListMgrDropdown.uctDropdown
	Public WithEvents cboReportIndicator As System.Windows.Forms.ComboBox
	Public WithEvents cboBinderIndicator As System.Windows.Forms.ComboBox
	Public WithEvents cboTermsOfPayment As System.Windows.Forms.ComboBox
	Public WithEvents cboCurrency As UserControls.CurrencyLookup
	Public WithEvents lblInsurerPaymentLocking As System.Windows.Forms.Label
	Public WithEvents lblBinderIndicator As System.Windows.Forms.Label
	Public WithEvents lblReportIndicator As System.Windows.Forms.Label
	Public WithEvents lblSubBranch As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents lblABINumber As System.Windows.Forms.Label
	Public WithEvents lblTermsOfPayment As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraAppointment As System.Windows.Forms.GroupBox
	Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents cmdRiskGroup As System.Windows.Forms.Button
	Private WithEvents _lvwAddresses_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAddresses_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAddresses As System.Windows.Forms.ListView
	Public WithEvents cmdAddAd As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAd As System.Windows.Forms.Button
	Public WithEvents cmdEditAd As System.Windows.Forms.Button
	Public WithEvents fraAddress As System.Windows.Forms.GroupBox
	Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
	Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents cmdEditCon As System.Windows.Forms.Button
	Public WithEvents cmdDeleteCon As System.Windows.Forms.Button
	Public WithEvents cmdAddCon As System.Windows.Forms.Button
	Private WithEvents _lvwContacts_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwContacts_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwContacts As System.Windows.Forms.ListView
	Public WithEvents fraContact As System.Windows.Forms.GroupBox
	Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
	Public WithEvents uctPartyTax1 As uctPartyTaxControl.uctPartyTax
	Public WithEvents fraTax As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents txtClaimsRatingDescription As System.Windows.Forms.TextBox
	Public WithEvents txtClaimsRatingGrading As System.Windows.Forms.TextBox
	Public WithEvents txtClaimsRatingDate As System.Windows.Forms.TextBox
	Public WithEvents cboClaimsRatingAgency As PMLookupControl.cboPMLookup
	Public WithEvents lblClaimsRatingDescription As System.Windows.Forms.Label
	Public WithEvents lblClaimsRatingAgency As System.Windows.Forms.Label
	Public WithEvents lblClaimsRatingGrading As System.Windows.Forms.Label
	Public WithEvents lblClaimsRatingDate As System.Windows.Forms.Label
	Public WithEvents fraClaimRating As System.Windows.Forms.GroupBox
	Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
	Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents cboBrokerlinkSubAccount As System.Windows.Forms.ComboBox
	Public WithEvents txtBrokerlinkUnderwritingId As System.Windows.Forms.TextBox
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraBrokerlink As System.Windows.Forms.GroupBox
	Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
	Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
	Public WithEvents cmdEditRisk As System.Windows.Forms.Button
	Private WithEvents _lvwRiskCodes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRiskCodes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRiskCodes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRiskCodes As System.Windows.Forms.ListView
	Public WithEvents fraFSA As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList2 As System.Windows.Forms.ImageList
	Public cmdNext(5) As System.Windows.Forms.Button
    Public cmdPrevious(5) As System.Windows.Forms.Button
    Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button
        Me.Toolbar1 = New System.Windows.Forms.ToolStrip
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._Toolbar1_Button1 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button2 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button3 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button4 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button5 = New System.Windows.Forms.ToolStripButton
        Me._Toolbar1_Button6 = New System.Windows.Forms.ToolStripSeparator
        Me._Toolbar1_Button7 = New System.Windows.Forms.ToolStripButton
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
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.Frame3 = New System.Windows.Forms.GroupBox
        Me.txtBureauAccount = New System.Windows.Forms.TextBox
        Me.cmdBureauAccount = New System.Windows.Forms.Button
        Me.cmdStargate = New System.Windows.Forms.Button
        Me.cmdIPTExtras = New System.Windows.Forms.Button
        Me.cmdRates = New System.Windows.Forms.Button
        Me.cmdPaymentGroups = New System.Windows.Forms.Button
        Me.txtAgencyNumber = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtIDReference = New System.Windows.Forms.TextBox
        Me.cboInsurerType = New PMLookupControl.cboPMLookup
        Me.lblInsurerType = New System.Windows.Forms.Label
        Me.lblAgencyNumber = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblIDReference = New System.Windows.Forms.Label
        Me.fraReinsurance = New System.Windows.Forms.GroupBox
        Me.chkRTA = New System.Windows.Forms.CheckBox
        Me.chkRTAEditable = New System.Windows.Forms.CheckBox
        Me.txtDefaultCommissionRate = New System.Windows.Forms.TextBox
        Me.cboInsurerStatus = New PMLookupControl.cboPMLookup
        Me.cboInsurerCreditRating = New PMLookupControl.cboPMLookup
        Me.lblReInsuranceType = New System.Windows.Forms.Label
        Me.lblInsurerCreditRating = New System.Windows.Forms.Label
        Me.lblCommission = New System.Windows.Forms.Label
        Me.fraAppointment = New System.Windows.Forms.GroupBox
        Me.cboLocking = New PMLookupControl.cboPMLookup
        Me.uctBranch = New System.Windows.Forms.ComboBox
        Me.cboSubBranch = New System.Windows.Forms.ComboBox
        Me.uctABINumber = New PMListMgrDropdown.uctDropdown
        Me.cboReportIndicator = New System.Windows.Forms.ComboBox
        Me.cboBinderIndicator = New System.Windows.Forms.ComboBox
        Me.cboTermsOfPayment = New System.Windows.Forms.ComboBox
        Me.cboCurrency = New UserControls.CurrencyLookup
        Me.lblInsurerPaymentLocking = New System.Windows.Forms.Label
        Me.lblBinderIndicator = New System.Windows.Forms.Label
        Me.lblReportIndicator = New System.Windows.Forms.Label
        Me.lblSubBranch = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lblABINumber = New System.Windows.Forms.Label
        Me.lblTermsOfPayment = New System.Windows.Forms.Label
        Me.lblCurrency = New System.Windows.Forms.Label
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraAddress = New System.Windows.Forms.GroupBox
        Me.cmdRiskGroup = New System.Windows.Forms.Button
        Me.lvwAddresses = New System.Windows.Forms.ListView
        Me._lvwAddresses_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwAddresses_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me.cmdAddAd = New System.Windows.Forms.Button
        Me.cmdDeleteAd = New System.Windows.Forms.Button
        Me.cmdEditAd = New System.Windows.Forms.Button
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraContact = New System.Windows.Forms.GroupBox
        Me.cmdEditCon = New System.Windows.Forms.Button
        Me.cmdDeleteCon = New System.Windows.Forms.Button
        Me.cmdAddCon = New System.Windows.Forms.Button
        Me.lvwContacts = New System.Windows.Forms.ListView
        Me._lvwContacts_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwContacts_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me.fraTax = New System.Windows.Forms.GroupBox
        Me.uctPartyTax1 = New uctPartyTaxControl.uctPartyTax
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me.fraClaimRating = New System.Windows.Forms.GroupBox
        Me.txtClaimsRatingDescription = New System.Windows.Forms.TextBox
        Me.txtClaimsRatingGrading = New System.Windows.Forms.TextBox
        Me.txtClaimsRatingDate = New System.Windows.Forms.TextBox
        Me.cboClaimsRatingAgency = New PMLookupControl.cboPMLookup
        Me.lblClaimsRatingDescription = New System.Windows.Forms.Label
        Me.lblClaimsRatingAgency = New System.Windows.Forms.Label
        Me.lblClaimsRatingGrading = New System.Windows.Forms.Label
        Me.lblClaimsRatingDate = New System.Windows.Forms.Label
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.fraBrokerlink = New System.Windows.Forms.GroupBox
        Me.cboBrokerlinkSubAccount = New System.Windows.Forms.ComboBox
        Me.txtBrokerlinkUnderwritingId = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me._cmdPrevious_4 = New System.Windows.Forms.Button
        Me._cmdNext_5 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me._cmdPrevious_5 = New System.Windows.Forms.Button
        Me.fraFSA = New System.Windows.Forms.GroupBox
        Me.cmdEditRisk = New System.Windows.Forms.Button
        Me.lvwRiskCodes = New System.Windows.Forms.ListView
        Me._lvwRiskCodes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwRiskCodes_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwRiskCodes_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.Toolbar1.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Frame3.SuspendLayout()
        Me.fraReinsurance.SuspendLayout()
        Me.fraAppointment.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraAddress.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraContact.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.fraTax.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraClaimRating.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me.fraBrokerlink.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.fraFSA.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(296, 519)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 24)
        Me.cmdApply.TabIndex = 49
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'Toolbar1
        '
        Me.Toolbar1.ImageList = Me.ImageList2
        Me.Toolbar1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._Toolbar1_Button1, Me._Toolbar1_Button2, Me._Toolbar1_Button3, Me._Toolbar1_Button4, Me._Toolbar1_Button5, Me._Toolbar1_Button6, Me._Toolbar1_Button7})
        Me.Toolbar1.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar1.Name = "Toolbar1"
        Me.Toolbar1.Size = New System.Drawing.Size(615, 25)
        Me.Toolbar1.TabIndex = 0
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "FINANCIAL")
        Me.ImageList2.Images.SetKeyName(1, "BLANK")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "NOTE")
        Me.ImageList2.Images.SetKeyName(4, "LETTER")
        Me.ImageList2.Images.SetKeyName(5, "COMMISSION")
        Me.ImageList2.Images.SetKeyName(6, "AddressImage")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "ContactImage")
        '
        '_Toolbar1_Button1
        '
        Me._Toolbar1_Button1.AutoSize = False
        Me._Toolbar1_Button1.ImageIndex = 0
        Me._Toolbar1_Button1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button1.Name = "_Toolbar1_Button1"
        Me._Toolbar1_Button1.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button1.ToolTipText = "Financial"
        '
        '_Toolbar1_Button2
        '
        Me._Toolbar1_Button2.AutoSize = False
        Me._Toolbar1_Button2.ImageIndex = 3
        Me._Toolbar1_Button2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button2.Name = "_Toolbar1_Button2"
        Me._Toolbar1_Button2.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button2.ToolTipText = "Notes"
        '
        '_Toolbar1_Button3
        '
        Me._Toolbar1_Button3.AutoSize = False
        Me._Toolbar1_Button3.Name = "_Toolbar1_Button3"
        Me._Toolbar1_Button3.Size = New System.Drawing.Size(6, 22)
        '
        '_Toolbar1_Button4
        '
        Me._Toolbar1_Button4.AutoSize = False
        Me._Toolbar1_Button4.ImageIndex = 4
        Me._Toolbar1_Button4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button4.Name = "_Toolbar1_Button4"
        Me._Toolbar1_Button4.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button4.ToolTipText = "Letter"
        '
        '_Toolbar1_Button5
        '
        Me._Toolbar1_Button5.AutoSize = False
        Me._Toolbar1_Button5.ImageIndex = 6
        Me._Toolbar1_Button5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button5.Name = "_Toolbar1_Button5"
        Me._Toolbar1_Button5.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button5.ToolTipText = "E-Mail"
        '
        '_Toolbar1_Button6
        '
        Me._Toolbar1_Button6.AutoSize = False
        Me._Toolbar1_Button6.Name = "_Toolbar1_Button6"
        Me._Toolbar1_Button6.Size = New System.Drawing.Size(6, 22)
        '
        '_Toolbar1_Button7
        '
        Me._Toolbar1_Button7.AutoSize = False
        Me._Toolbar1_Button7.ImageIndex = 7
        Me._Toolbar1_Button7.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me._Toolbar1_Button7.Name = "_Toolbar1_Button7"
        Me._Toolbar1_Button7.Size = New System.Drawing.Size(24, 22)
        Me._Toolbar1_Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._Toolbar1_Button7.ToolTipText = "Web "
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 519)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 24)
        Me.cmdHelp.TabIndex = 52
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 519)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 24)
        Me.cmdCancel.TabIndex = 51
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
        Me.cmdOK.Location = New System.Drawing.Point(376, 519)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 24)
        Me.cmdOK.TabIndex = 50
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
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage5)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage6)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(84, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(9, 32)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 481)
        Me.tabMainTab.TabIndex = 1
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraReinsurance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraAppointment)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Insurer"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Location = New System.Drawing.Point(5627, 24)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.txtBureauAccount)
        Me.Frame3.Controls.Add(Me.cmdBureauAccount)
        Me.Frame3.Controls.Add(Me.cmdStargate)
        Me.Frame3.Controls.Add(Me.cmdIPTExtras)
        Me.Frame3.Controls.Add(Me.cmdRates)
        Me.Frame3.Controls.Add(Me.cmdPaymentGroups)
        Me.Frame3.Controls.Add(Me.txtAgencyNumber)
        Me.Frame3.Controls.Add(Me.txtName)
        Me.Frame3.Controls.Add(Me.txtIDReference)
        Me.Frame3.Controls.Add(Me.cboInsurerType)
        Me.Frame3.Controls.Add(Me.lblInsurerType)
        Me.Frame3.Controls.Add(Me.lblAgencyNumber)
        Me.Frame3.Controls.Add(Me.lblName)
        Me.Frame3.Controls.Add(Me.lblIDReference)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(5008, 4)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(585, 143)
        Me.Frame3.TabIndex = 13
        Me.Frame3.TabStop = False
        '
        'txtBureauAccount
        '
        Me.txtBureauAccount.AcceptsReturn = True
        Me.txtBureauAccount.BackColor = System.Drawing.SystemColors.Window
        Me.txtBureauAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBureauAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBureauAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBureauAccount.Location = New System.Drawing.Point(136, 112)
        Me.txtBureauAccount.MaxLength = 0
        Me.txtBureauAccount.Name = "txtBureauAccount"
        Me.txtBureauAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBureauAccount.Size = New System.Drawing.Size(153, 19)
        Me.txtBureauAccount.TabIndex = 87
        '
        'cmdBureauAccount
        '
        Me.cmdBureauAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBureauAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBureauAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBureauAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBureauAccount.Location = New System.Drawing.Point(8, 112)
        Me.cmdBureauAccount.Name = "cmdBureauAccount"
        Me.cmdBureauAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBureauAccount.Size = New System.Drawing.Size(113, 19)
        Me.cmdBureauAccount.TabIndex = 25
        Me.cmdBureauAccount.Text = "&Bureau Account"
        Me.cmdBureauAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBureauAccount.UseVisualStyleBackColor = False
        '
        'cmdStargate
        '
        Me.cmdStargate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdStargate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdStargate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStargate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdStargate.Location = New System.Drawing.Point(416, 64)
        Me.cmdStargate.Name = "cmdStargate"
        Me.cmdStargate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdStargate.Size = New System.Drawing.Size(137, 22)
        Me.cmdStargate.TabIndex = 23
        Me.cmdStargate.Text = "&Stargate Config."
        Me.cmdStargate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdStargate.UseVisualStyleBackColor = False
        Me.cmdStargate.Visible = False
        '
        'cmdIPTExtras
        '
        Me.cmdIPTExtras.BackColor = System.Drawing.SystemColors.Control
        Me.cmdIPTExtras.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdIPTExtras.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdIPTExtras.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdIPTExtras.Location = New System.Drawing.Point(416, 64)
        Me.cmdIPTExtras.Name = "cmdIPTExtras"
        Me.cmdIPTExtras.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdIPTExtras.Size = New System.Drawing.Size(137, 22)
        Me.cmdIPTExtras.TabIndex = 22
        Me.cmdIPTExtras.Text = "&IPT rates"
        Me.cmdIPTExtras.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdIPTExtras.UseVisualStyleBackColor = False
        Me.cmdIPTExtras.Visible = False
        '
        'cmdRates
        '
        Me.cmdRates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRates.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRates.Location = New System.Drawing.Point(416, 16)
        Me.cmdRates.Name = "cmdRates"
        Me.cmdRates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRates.Size = New System.Drawing.Size(137, 22)
        Me.cmdRates.TabIndex = 16
        Me.cmdRates.Text = "&Commission rates"
        Me.cmdRates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRates.UseVisualStyleBackColor = False
        '
        'cmdPaymentGroups
        '
        Me.cmdPaymentGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPaymentGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPaymentGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPaymentGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPaymentGroups.Location = New System.Drawing.Point(416, 40)
        Me.cmdPaymentGroups.Name = "cmdPaymentGroups"
        Me.cmdPaymentGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPaymentGroups.Size = New System.Drawing.Size(137, 22)
        Me.cmdPaymentGroups.TabIndex = 19
        Me.cmdPaymentGroups.Text = "&Payment Groups"
        Me.cmdPaymentGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPaymentGroups.UseVisualStyleBackColor = False
        '
        'txtAgencyNumber
        '
        Me.txtAgencyNumber.AcceptsReturn = True
        Me.txtAgencyNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgencyNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgencyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgencyNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgencyNumber.Location = New System.Drawing.Point(136, 64)
        Me.txtAgencyNumber.MaxLength = 0
        Me.txtAgencyNumber.Name = "txtAgencyNumber"
        Me.txtAgencyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgencyNumber.Size = New System.Drawing.Size(153, 19)
        Me.txtAgencyNumber.TabIndex = 20
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(136, 40)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(265, 19)
        Me.txtName.TabIndex = 18
        '
        'txtIDReference
        '
        Me.txtIDReference.AcceptsReturn = True
        Me.txtIDReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtIDReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIDReference.Location = New System.Drawing.Point(136, 18)
        Me.txtIDReference.MaxLength = 0
        Me.txtIDReference.Name = "txtIDReference"
        Me.txtIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIDReference.Size = New System.Drawing.Size(153, 19)
        Me.txtIDReference.TabIndex = 15
        '
        'cboInsurerType
        '
        Me.cboInsurerType.DefaultItemId = 0
        Me.cboInsurerType.FirstItem = "(none)"
        Me.cboInsurerType.ItemId = 0
        Me.cboInsurerType.ListIndex = -1
        Me.cboInsurerType.Location = New System.Drawing.Point(136, 88)
        Me.cboInsurerType.Name = "cboInsurerType"
        Me.cboInsurerType.PMLookupProductFamily = 9
        Me.cboInsurerType.SingleItemId = 0
        Me.cboInsurerType.Size = New System.Drawing.Size(153, 21)
        Me.cboInsurerType.Sorted = True
        Me.cboInsurerType.TabIndex = 24
        Me.cboInsurerType.TableName = "Insurer_Type"
        Me.cboInsurerType.ToolTipText = ""
        Me.cboInsurerType.WhereClause = ""
        '
        'lblInsurerType
        '
        Me.lblInsurerType.AutoSize = True
        Me.lblInsurerType.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerType.Location = New System.Drawing.Point(16, 88)
        Me.lblInsurerType.Name = "lblInsurerType"
        Me.lblInsurerType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerType.Size = New System.Drawing.Size(69, 13)
        Me.lblInsurerType.TabIndex = 88
        Me.lblInsurerType.Text = "Insurer Type:"
        '
        'lblAgencyNumber
        '
        Me.lblAgencyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgencyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgencyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgencyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgencyNumber.Location = New System.Drawing.Point(16, 67)
        Me.lblAgencyNumber.Name = "lblAgencyNumber"
        Me.lblAgencyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgencyNumber.Size = New System.Drawing.Size(121, 17)
        Me.lblAgencyNumber.TabIndex = 21
        Me.lblAgencyNumber.Text = "Agency number:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(16, 43)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(71, 13)
        Me.lblName.TabIndex = 17
        Me.lblName.Text = "Insurer name:"
        '
        'lblIDReference
        '
        Me.lblIDReference.AutoSize = True
        Me.lblIDReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblIDReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIDReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIDReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIDReference.Location = New System.Drawing.Point(16, 20)
        Me.lblIDReference.Name = "lblIDReference"
        Me.lblIDReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIDReference.Size = New System.Drawing.Size(69, 13)
        Me.lblIDReference.TabIndex = 14
        Me.lblIDReference.Text = "Insurer code:"
        '
        'fraReinsurance
        '
        Me.fraReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.fraReinsurance.Controls.Add(Me.chkRTA)
        Me.fraReinsurance.Controls.Add(Me.chkRTAEditable)
        Me.fraReinsurance.Controls.Add(Me.txtDefaultCommissionRate)
        Me.fraReinsurance.Controls.Add(Me.cboInsurerStatus)
        Me.fraReinsurance.Controls.Add(Me.cboInsurerCreditRating)
        Me.fraReinsurance.Controls.Add(Me.lblReInsuranceType)
        Me.fraReinsurance.Controls.Add(Me.lblInsurerCreditRating)
        Me.fraReinsurance.Controls.Add(Me.lblCommission)
        Me.fraReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReinsurance.Location = New System.Drawing.Point(5008, 324)
        Me.fraReinsurance.Name = "fraReinsurance"
        Me.fraReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReinsurance.Size = New System.Drawing.Size(585, 96)
        Me.fraReinsurance.TabIndex = 38
        Me.fraReinsurance.TabStop = False
        Me.fraReinsurance.Text = "FSA"
        '
        'chkRTA
        '
        Me.chkRTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkRTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRTA.Location = New System.Drawing.Point(304, 40)
        Me.chkRTA.Name = "chkRTA"
        Me.chkRTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRTA.Size = New System.Drawing.Size(261, 26)
        Me.chkRTA.TabIndex = 86
        Me.chkRTA.Text = "Default Risk Transfer Agreement:"
        Me.chkRTA.UseVisualStyleBackColor = False
        '
        'chkRTAEditable
        '
        Me.chkRTAEditable.BackColor = System.Drawing.SystemColors.Control
        Me.chkRTAEditable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRTAEditable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRTAEditable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRTAEditable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRTAEditable.Location = New System.Drawing.Point(304, 64)
        Me.chkRTAEditable.Name = "chkRTAEditable"
        Me.chkRTAEditable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRTAEditable.Size = New System.Drawing.Size(261, 26)
        Me.chkRTAEditable.TabIndex = 79
        Me.chkRTAEditable.Text = "Allow Risk Transfer Flag to be changed on policy:"
        Me.chkRTAEditable.UseVisualStyleBackColor = False
        '
        'txtDefaultCommissionRate
        '
        Me.txtDefaultCommissionRate.AcceptsReturn = True
        Me.txtDefaultCommissionRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDefaultCommissionRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDefaultCommissionRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefaultCommissionRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDefaultCommissionRate.Location = New System.Drawing.Point(136, 43)
        Me.txtDefaultCommissionRate.MaxLength = 0
        Me.txtDefaultCommissionRate.Name = "txtDefaultCommissionRate"
        Me.txtDefaultCommissionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDefaultCommissionRate.Size = New System.Drawing.Size(153, 19)
        Me.txtDefaultCommissionRate.TabIndex = 44
        '
        'cboInsurerStatus
        '
        Me.cboInsurerStatus.DefaultItemId = 0
        Me.cboInsurerStatus.FirstItem = "(none)"
        Me.cboInsurerStatus.ItemId = 0
        Me.cboInsurerStatus.ListIndex = -1
        Me.cboInsurerStatus.Location = New System.Drawing.Point(136, 16)
        Me.cboInsurerStatus.Name = "cboInsurerStatus"
        Me.cboInsurerStatus.PMLookupProductFamily = 9
        Me.cboInsurerStatus.SingleItemId = 0
        Me.cboInsurerStatus.Size = New System.Drawing.Size(153, 21)
        Me.cboInsurerStatus.Sorted = True
        Me.cboInsurerStatus.TabIndex = 40
        Me.cboInsurerStatus.TableName = "FSA_InsurerStatus"
        Me.cboInsurerStatus.ToolTipText = ""
        Me.cboInsurerStatus.WhereClause = ""
        '
        'cboInsurerCreditRating
        '
        Me.cboInsurerCreditRating.DefaultItemId = 0
        Me.cboInsurerCreditRating.FirstItem = "(none)"
        Me.cboInsurerCreditRating.ItemId = 0
        Me.cboInsurerCreditRating.ListIndex = -1
        Me.cboInsurerCreditRating.Location = New System.Drawing.Point(416, 16)
        Me.cboInsurerCreditRating.Name = "cboInsurerCreditRating"
        Me.cboInsurerCreditRating.PMLookupProductFamily = 9
        Me.cboInsurerCreditRating.SingleItemId = 0
        Me.cboInsurerCreditRating.Size = New System.Drawing.Size(153, 21)
        Me.cboInsurerCreditRating.Sorted = True
        Me.cboInsurerCreditRating.TabIndex = 42
        Me.cboInsurerCreditRating.TableName = "FSA_InsurerCreditRating"
        Me.cboInsurerCreditRating.ToolTipText = ""
        Me.cboInsurerCreditRating.WhereClause = ""
        '
        'lblReInsuranceType
        '
        Me.lblReInsuranceType.AutoSize = True
        Me.lblReInsuranceType.BackColor = System.Drawing.SystemColors.Control
        Me.lblReInsuranceType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReInsuranceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReInsuranceType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReInsuranceType.Location = New System.Drawing.Point(16, 22)
        Me.lblReInsuranceType.Name = "lblReInsuranceType"
        Me.lblReInsuranceType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReInsuranceType.Size = New System.Drawing.Size(40, 13)
        Me.lblReInsuranceType.TabIndex = 39
        Me.lblReInsuranceType.Text = "Status:"
        '
        'lblInsurerCreditRating
        '
        Me.lblInsurerCreditRating.AutoSize = True
        Me.lblInsurerCreditRating.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerCreditRating.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerCreditRating.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerCreditRating.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerCreditRating.Location = New System.Drawing.Point(305, 22)
        Me.lblInsurerCreditRating.Name = "lblInsurerCreditRating"
        Me.lblInsurerCreditRating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerCreditRating.Size = New System.Drawing.Size(71, 13)
        Me.lblInsurerCreditRating.TabIndex = 41
        Me.lblInsurerCreditRating.Text = "Credit Rating:"
        '
        'lblCommission
        '
        Me.lblCommission.AutoSize = True
        Me.lblCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommission.Location = New System.Drawing.Point(16, 40)
        Me.lblCommission.Name = "lblCommission"
        Me.lblCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommission.Size = New System.Drawing.Size(106, 13)
        Me.lblCommission.TabIndex = 43
        Me.lblCommission.Text = "Registration Number:"
        '
        'fraAppointment
        '
        Me.fraAppointment.BackColor = System.Drawing.SystemColors.Control
        Me.fraAppointment.Controls.Add(Me.cboLocking)
        Me.fraAppointment.Controls.Add(Me.uctBranch)
        Me.fraAppointment.Controls.Add(Me.cboSubBranch)
        Me.fraAppointment.Controls.Add(Me.uctABINumber)
        Me.fraAppointment.Controls.Add(Me.cboReportIndicator)
        Me.fraAppointment.Controls.Add(Me.cboBinderIndicator)
        Me.fraAppointment.Controls.Add(Me.cboTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.cboCurrency)
        Me.fraAppointment.Controls.Add(Me.lblInsurerPaymentLocking)
        Me.fraAppointment.Controls.Add(Me.lblBinderIndicator)
        Me.fraAppointment.Controls.Add(Me.lblReportIndicator)
        Me.fraAppointment.Controls.Add(Me.lblSubBranch)
        Me.fraAppointment.Controls.Add(Me.lblBranch)
        Me.fraAppointment.Controls.Add(Me.lblABINumber)
        Me.fraAppointment.Controls.Add(Me.lblTermsOfPayment)
        Me.fraAppointment.Controls.Add(Me.lblCurrency)
        Me.fraAppointment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAppointment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAppointment.Location = New System.Drawing.Point(5008, 152)
        Me.fraAppointment.Name = "fraAppointment"
        Me.fraAppointment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAppointment.Size = New System.Drawing.Size(585, 169)
        Me.fraAppointment.TabIndex = 26
        Me.fraAppointment.TabStop = False
        '
        'cboLocking
        '
        Me.cboLocking.DefaultItemId = 0
        Me.cboLocking.FirstItem = ""
        Me.cboLocking.ItemId = 0
        Me.cboLocking.ListIndex = -1
        Me.cboLocking.Location = New System.Drawing.Point(416, 142)
        Me.cboLocking.Name = "cboLocking"
        Me.cboLocking.PMLookupProductFamily = 1
        Me.cboLocking.SingleItemId = 0
        Me.cboLocking.Size = New System.Drawing.Size(153, 21)
        Me.cboLocking.Sorted = True
        Me.cboLocking.TabIndex = 76
        Me.cboLocking.TableName = "insurer_locking_type"
        Me.cboLocking.ToolTipText = ""
        Me.cboLocking.WhereClause = ""
        '
        'uctBranch
        '
        Me.uctBranch.BackColor = System.Drawing.SystemColors.Window
        Me.uctBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.uctBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uctBranch.Location = New System.Drawing.Point(136, 16)
        Me.uctBranch.Name = "uctBranch"
        Me.uctBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uctBranch.Size = New System.Drawing.Size(153, 21)
        Me.uctBranch.TabIndex = 27
        Me.uctBranch.Text = " "
        '
        'cboSubBranch
        '
        Me.cboSubBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSubBranch.Location = New System.Drawing.Point(416, 16)
        Me.cboSubBranch.Name = "cboSubBranch"
        Me.cboSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubBranch.Size = New System.Drawing.Size(153, 21)
        Me.cboSubBranch.TabIndex = 28
        '
        'uctABINumber
        '
        Me.uctABINumber.AllowAbiCodeEntry = False
        Me.uctABINumber.AutoCompleteText = False
        Me.uctABINumber.DataModel = "GIIM"
        Me.uctABINumber.ListIndex = -1
        Me.uctABINumber.ListManager = Nothing
        Me.uctABINumber.Location = New System.Drawing.Point(136, 112)
        Me.uctABINumber.Login = False
        Me.uctABINumber.LongList = False
        Me.uctABINumber.MousePointer = System.Windows.Forms.Cursors.Default
        Me.uctABINumber.Name = "uctABINumber"
        Me.uctABINumber.PropertyId = "2818081"
        Me.uctABINumber.ReadOnly_Renamed = False
        Me.uctABINumber.SelLength = 0
        Me.uctABINumber.SelStart = 0
        Me.uctABINumber.SelText = ""
        Me.uctABINumber.Size = New System.Drawing.Size(433, 21)
        Me.uctABINumber.TabIndex = 37
        Me.uctABINumber.ToolTipText = ""
        Me.uctABINumber.VehicleListId = ""
        Me.uctABINumber.VehicleMake = ""
        '
        'cboReportIndicator
        '
        Me.cboReportIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboReportIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReportIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReportIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReportIndicator.Location = New System.Drawing.Point(416, 80)
        Me.cboReportIndicator.Name = "cboReportIndicator"
        Me.cboReportIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReportIndicator.Size = New System.Drawing.Size(153, 21)
        Me.cboReportIndicator.TabIndex = 35
        '
        'cboBinderIndicator
        '
        Me.cboBinderIndicator.BackColor = System.Drawing.SystemColors.Window
        Me.cboBinderIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBinderIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBinderIndicator.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBinderIndicator.Location = New System.Drawing.Point(416, 48)
        Me.cboBinderIndicator.Name = "cboBinderIndicator"
        Me.cboBinderIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBinderIndicator.Size = New System.Drawing.Size(153, 21)
        Me.cboBinderIndicator.TabIndex = 32
        '
        'cboTermsOfPayment
        '
        Me.cboTermsOfPayment.BackColor = System.Drawing.SystemColors.Window
        Me.cboTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTermsOfPayment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTermsOfPayment.Location = New System.Drawing.Point(136, 80)
        Me.cboTermsOfPayment.Name = "cboTermsOfPayment"
        Me.cboTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTermsOfPayment.Size = New System.Drawing.Size(153, 21)
        Me.cboTermsOfPayment.TabIndex = 33
        '
        'cboCurrency
        '
        Me.cboCurrency.CompanyId = 0
        Me.cboCurrency.CurrencyId = 0
        Me.cboCurrency.DefaultCurrencyId = 0
        Me.cboCurrency.FirstItem = ""
        Me.cboCurrency.ListIndex = -1
        Me.cboCurrency.Location = New System.Drawing.Point(136, 48)
        Me.cboCurrency.Name = "cboCurrency"
        Me.cboCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actAllCurrencies
        Me.cboCurrency.Size = New System.Drawing.Size(153, 21)
        Me.cboCurrency.TabIndex = 64
        Me.cboCurrency.ToolTipText = ""
        Me.cboCurrency.WhatsThisHelpID = 0
        '
        'lblInsurerPaymentLocking
        '
        Me.lblInsurerPaymentLocking.AutoSize = True
        Me.lblInsurerPaymentLocking.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerPaymentLocking.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerPaymentLocking.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerPaymentLocking.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerPaymentLocking.Location = New System.Drawing.Point(251, 144)
        Me.lblInsurerPaymentLocking.Name = "lblInsurerPaymentLocking"
        Me.lblInsurerPaymentLocking.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerPaymentLocking.Size = New System.Drawing.Size(127, 13)
        Me.lblInsurerPaymentLocking.TabIndex = 80
        Me.lblInsurerPaymentLocking.Text = "Insurer Payment Locking:"
        '
        'lblBinderIndicator
        '
        Me.lblBinderIndicator.AutoSize = True
        Me.lblBinderIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.lblBinderIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBinderIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBinderIndicator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBinderIndicator.Location = New System.Drawing.Point(301, 51)
        Me.lblBinderIndicator.Name = "lblBinderIndicator"
        Me.lblBinderIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBinderIndicator.Size = New System.Drawing.Size(83, 13)
        Me.lblBinderIndicator.TabIndex = 78
        Me.lblBinderIndicator.Text = "Binder indicator:"
        '
        'lblReportIndicator
        '
        Me.lblReportIndicator.AutoSize = True
        Me.lblReportIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportIndicator.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportIndicator.Location = New System.Drawing.Point(300, 83)
        Me.lblReportIndicator.Name = "lblReportIndicator"
        Me.lblReportIndicator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportIndicator.Size = New System.Drawing.Size(85, 13)
        Me.lblReportIndicator.TabIndex = 77
        Me.lblReportIndicator.Text = "Report indicator:"
        '
        'lblSubBranch
        '
        Me.lblSubBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubBranch.Location = New System.Drawing.Point(301, 20)
        Me.lblSubBranch.Name = "lblSubBranch"
        Me.lblSubBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubBranch.Size = New System.Drawing.Size(93, 17)
        Me.lblSubBranch.TabIndex = 30
        Me.lblSubBranch.Text = "Sub-branch:"
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(16, 20)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(89, 17)
        Me.lblBranch.TabIndex = 29
        Me.lblBranch.Text = "Branch:"
        '
        'lblABINumber
        '
        Me.lblABINumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblABINumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblABINumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblABINumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblABINumber.Location = New System.Drawing.Point(16, 115)
        Me.lblABINumber.Name = "lblABINumber"
        Me.lblABINumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblABINumber.Size = New System.Drawing.Size(105, 17)
        Me.lblABINumber.TabIndex = 36
        Me.lblABINumber.Text = "ABI number:"
        '
        'lblTermsOfPayment
        '
        Me.lblTermsOfPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblTermsOfPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTermsOfPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTermsOfPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTermsOfPayment.Location = New System.Drawing.Point(16, 83)
        Me.lblTermsOfPayment.Name = "lblTermsOfPayment"
        Me.lblTermsOfPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTermsOfPayment.Size = New System.Drawing.Size(121, 17)
        Me.lblTermsOfPayment.TabIndex = 34
        Me.lblTermsOfPayment.Text = "Terms of payment:"
        '
        'lblCurrency
        '
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(16, 51)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(97, 17)
        Me.lblCurrency.TabIndex = 31
        Me.lblCurrency.Text = "Currency:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(5552, 428)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 47
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraAddress)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "Address"
        '
        'fraAddress
        '
        Me.fraAddress.BackColor = System.Drawing.SystemColors.Control
        Me.fraAddress.Controls.Add(Me.cmdRiskGroup)
        Me.fraAddress.Controls.Add(Me.lvwAddresses)
        Me.fraAddress.Controls.Add(Me.cmdAddAd)
        Me.fraAddress.Controls.Add(Me.cmdDeleteAd)
        Me.fraAddress.Controls.Add(Me.cmdEditAd)
        Me.fraAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAddress.Location = New System.Drawing.Point(8, 4)
        Me.fraAddress.Name = "fraAddress"
        Me.fraAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAddress.Size = New System.Drawing.Size(585, 369)
        Me.fraAddress.TabIndex = 2
        Me.fraAddress.TabStop = False
        '
        'cmdRiskGroup
        '
        Me.cmdRiskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRiskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRiskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRiskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRiskGroup.Location = New System.Drawing.Point(8, 336)
        Me.cmdRiskGroup.Name = "cmdRiskGroup"
        Me.cmdRiskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRiskGroup.Size = New System.Drawing.Size(73, 22)
        Me.cmdRiskGroup.TabIndex = 4
        Me.cmdRiskGroup.Text = "&Risk"
        Me.cmdRiskGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRiskGroup.UseVisualStyleBackColor = False
        '
        'lvwAddresses
        '
        Me.lvwAddresses.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAddresses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAddresses_ColumnHeader_1, Me._lvwAddresses_ColumnHeader_2, Me._lvwAddresses_ColumnHeader_3, Me._lvwAddresses_ColumnHeader_4, Me._lvwAddresses_ColumnHeader_5, Me._lvwAddresses_ColumnHeader_6, Me._lvwAddresses_ColumnHeader_7})
        Me.lvwAddresses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAddresses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAddresses.FullRowSelect = True
        Me.lvwAddresses.Location = New System.Drawing.Point(8, 16)
        Me.lvwAddresses.Name = "lvwAddresses"
        Me.lvwAddresses.Size = New System.Drawing.Size(569, 309)
        Me.lvwAddresses.TabIndex = 3
        Me.lvwAddresses.UseCompatibleStateImageBehavior = False
        Me.lvwAddresses.View = System.Windows.Forms.View.Details
        '
        '_lvwAddresses_ColumnHeader_1
        '
        Me._lvwAddresses_ColumnHeader_1.Text = "Post Code"
        Me._lvwAddresses_ColumnHeader_1.Width = 67
        '
        '_lvwAddresses_ColumnHeader_2
        '
        Me._lvwAddresses_ColumnHeader_2.Text = "Address Usage"
        Me._lvwAddresses_ColumnHeader_2.Width = 167
        '
        '_lvwAddresses_ColumnHeader_3
        '
        Me._lvwAddresses_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwAddresses_ColumnHeader_3.Width = 97
        '
        '_lvwAddresses_ColumnHeader_4
        '
        Me._lvwAddresses_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwAddresses_ColumnHeader_4.Width = 97
        '
        '_lvwAddresses_ColumnHeader_5
        '
        Me._lvwAddresses_ColumnHeader_5.Text = "Address Line 3"
        Me._lvwAddresses_ColumnHeader_5.Width = 97
        '
        '_lvwAddresses_ColumnHeader_6
        '
        Me._lvwAddresses_ColumnHeader_6.Text = "Address Line 4"
        Me._lvwAddresses_ColumnHeader_6.Width = 97
        '
        '_lvwAddresses_ColumnHeader_7
        '
        Me._lvwAddresses_ColumnHeader_7.Width = 0
        '
        'cmdAddAd
        '
        Me.cmdAddAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAd.Location = New System.Drawing.Point(344, 336)
        Me.cmdAddAd.Name = "cmdAddAd"
        Me.cmdAddAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAd.TabIndex = 5
        Me.cmdAddAd.Text = "&Add"
        Me.cmdAddAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAd.UseVisualStyleBackColor = False
        '
        'cmdDeleteAd
        '
        Me.cmdDeleteAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAd.Location = New System.Drawing.Point(504, 336)
        Me.cmdDeleteAd.Name = "cmdDeleteAd"
        Me.cmdDeleteAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAd.TabIndex = 7
        Me.cmdDeleteAd.Text = "&Delete"
        Me.cmdDeleteAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteAd.UseVisualStyleBackColor = False
        '
        'cmdEditAd
        '
        Me.cmdEditAd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditAd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditAd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditAd.Location = New System.Drawing.Point(424, 336)
        Me.cmdEditAd.Name = "cmdEditAd"
        Me.cmdEditAd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditAd.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditAd.TabIndex = 6
        Me.cmdEditAd.Text = "&Edit"
        Me.cmdEditAd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditAd.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(552, 380)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 48
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 380)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 45
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraContact)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "Contacts"
        '
        'fraContact
        '
        Me.fraContact.BackColor = System.Drawing.SystemColors.Control
        Me.fraContact.Controls.Add(Me.cmdEditCon)
        Me.fraContact.Controls.Add(Me.cmdDeleteCon)
        Me.fraContact.Controls.Add(Me.cmdAddCon)
        Me.fraContact.Controls.Add(Me.lvwContacts)
        Me.fraContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraContact.Location = New System.Drawing.Point(-4992, 4)
        Me.fraContact.Name = "fraContact"
        Me.fraContact.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraContact.Size = New System.Drawing.Size(585, 369)
        Me.fraContact.TabIndex = 8
        Me.fraContact.TabStop = False
        '
        'cmdEditCon
        '
        Me.cmdEditCon.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditCon.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditCon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditCon.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditCon.Location = New System.Drawing.Point(424, 336)
        Me.cmdEditCon.Name = "cmdEditCon"
        Me.cmdEditCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditCon.TabIndex = 11
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
        Me.cmdDeleteCon.Location = New System.Drawing.Point(504, 336)
        Me.cmdDeleteCon.Name = "cmdDeleteCon"
        Me.cmdDeleteCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteCon.TabIndex = 12
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
        Me.cmdAddCon.Location = New System.Drawing.Point(344, 336)
        Me.cmdAddCon.Name = "cmdAddCon"
        Me.cmdAddCon.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddCon.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddCon.TabIndex = 10
        Me.cmdAddCon.Text = "&Add"
        Me.cmdAddCon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddCon.UseVisualStyleBackColor = False
        '
        'lvwContacts
        '
        Me.lvwContacts.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContacts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwContacts_ColumnHeader_1, Me._lvwContacts_ColumnHeader_2, Me._lvwContacts_ColumnHeader_3, Me._lvwContacts_ColumnHeader_4, Me._lvwContacts_ColumnHeader_5})
        Me.lvwContacts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContacts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContacts.FullRowSelect = True
        Me.lvwContacts.Location = New System.Drawing.Point(8, 16)
        Me.lvwContacts.Name = "lvwContacts"
        Me.lvwContacts.Size = New System.Drawing.Size(569, 310)
        Me.lvwContacts.TabIndex = 9
        Me.lvwContacts.UseCompatibleStateImageBehavior = False
        Me.lvwContacts.View = System.Windows.Forms.View.Details
        '
        '_lvwContacts_ColumnHeader_1
        '
        Me._lvwContacts_ColumnHeader_1.Text = "Area Code"
        Me._lvwContacts_ColumnHeader_1.Width = 67
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
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(-4992, 380)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 46
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(-4448, 380)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 70
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdNext_3)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fraTax)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "Tax"
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 380)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 73
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(552, 380)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_3.TabIndex = 71
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'fraTax
        '
        Me.fraTax.BackColor = System.Drawing.SystemColors.Control
        Me.fraTax.Controls.Add(Me.uctPartyTax1)
        Me.fraTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTax.Location = New System.Drawing.Point(8, 4)
        Me.fraTax.Name = "fraTax"
        Me.fraTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTax.Size = New System.Drawing.Size(585, 369)
        Me.fraTax.TabIndex = 53
        Me.fraTax.TabStop = False
        Me.fraTax.Text = "Party Tax"
        '
        'uctPartyTax1
        '
        Me.uctPartyTax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyTax1.FrameOn = False
        Me.uctPartyTax1.IsDomiciledForTax = False
        Me.uctPartyTax1.Location = New System.Drawing.Point(8, 24)
        Me.uctPartyTax1.Name = "uctPartyTax1"
        Me.uctPartyTax1.Size = New System.Drawing.Size(569, 209)
        Me.uctPartyTax1.SizeHorizontal = False
        Me.uctPartyTax1.TabIndex = 63
        Me.uctPartyTax1.TaxExempt = False
        Me.uctPartyTax1.TaxNumber = ""
        Me.uctPartyTax1.TaxPercentage = 0
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraClaimRating)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdPrevious_3)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "Claims Rating"
        '
        'fraClaimRating
        '
        Me.fraClaimRating.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimRating.Controls.Add(Me.txtClaimsRatingDescription)
        Me.fraClaimRating.Controls.Add(Me.txtClaimsRatingGrading)
        Me.fraClaimRating.Controls.Add(Me.txtClaimsRatingDate)
        Me.fraClaimRating.Controls.Add(Me.cboClaimsRatingAgency)
        Me.fraClaimRating.Controls.Add(Me.lblClaimsRatingDescription)
        Me.fraClaimRating.Controls.Add(Me.lblClaimsRatingAgency)
        Me.fraClaimRating.Controls.Add(Me.lblClaimsRatingGrading)
        Me.fraClaimRating.Controls.Add(Me.lblClaimsRatingDate)
        Me.fraClaimRating.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimRating.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimRating.Location = New System.Drawing.Point(8, 4)
        Me.fraClaimRating.Name = "fraClaimRating"
        Me.fraClaimRating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimRating.Size = New System.Drawing.Size(585, 369)
        Me.fraClaimRating.TabIndex = 59
        Me.fraClaimRating.TabStop = False
        '
        'txtClaimsRatingDescription
        '
        Me.txtClaimsRatingDescription.AcceptsReturn = True
        Me.txtClaimsRatingDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimsRatingDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimsRatingDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimsRatingDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimsRatingDescription.Location = New System.Drawing.Point(128, 112)
        Me.txtClaimsRatingDescription.MaxLength = 4000
        Me.txtClaimsRatingDescription.Multiline = True
        Me.txtClaimsRatingDescription.Name = "txtClaimsRatingDescription"
        Me.txtClaimsRatingDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimsRatingDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtClaimsRatingDescription.Size = New System.Drawing.Size(449, 139)
        Me.txtClaimsRatingDescription.TabIndex = 58
        Me.txtClaimsRatingDescription.Text = " "
        '
        'txtClaimsRatingGrading
        '
        Me.txtClaimsRatingGrading.AcceptsReturn = True
        Me.txtClaimsRatingGrading.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimsRatingGrading.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimsRatingGrading.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimsRatingGrading.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimsRatingGrading.Location = New System.Drawing.Point(128, 48)
        Me.txtClaimsRatingGrading.MaxLength = 0
        Me.txtClaimsRatingGrading.Name = "txtClaimsRatingGrading"
        Me.txtClaimsRatingGrading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimsRatingGrading.Size = New System.Drawing.Size(193, 19)
        Me.txtClaimsRatingGrading.TabIndex = 56
        Me.txtClaimsRatingGrading.Text = " "
        '
        'txtClaimsRatingDate
        '
        Me.txtClaimsRatingDate.AcceptsReturn = True
        Me.txtClaimsRatingDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimsRatingDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimsRatingDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimsRatingDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimsRatingDate.Location = New System.Drawing.Point(128, 80)
        Me.txtClaimsRatingDate.MaxLength = 0
        Me.txtClaimsRatingDate.Name = "txtClaimsRatingDate"
        Me.txtClaimsRatingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimsRatingDate.Size = New System.Drawing.Size(193, 19)
        Me.txtClaimsRatingDate.TabIndex = 57
        Me.txtClaimsRatingDate.Text = " "
        '
        'cboClaimsRatingAgency
        '
        Me.cboClaimsRatingAgency.DefaultItemId = 0
        Me.cboClaimsRatingAgency.FirstItem = "(None)"
        Me.cboClaimsRatingAgency.ItemId = 0
        Me.cboClaimsRatingAgency.ListIndex = -1
        Me.cboClaimsRatingAgency.Location = New System.Drawing.Point(128, 16)
        Me.cboClaimsRatingAgency.Name = "cboClaimsRatingAgency"
        Me.cboClaimsRatingAgency.PMLookupProductFamily = 1
        Me.cboClaimsRatingAgency.SingleItemId = 0
        Me.cboClaimsRatingAgency.Size = New System.Drawing.Size(193, 21)
        Me.cboClaimsRatingAgency.Sorted = True
        Me.cboClaimsRatingAgency.TabIndex = 55
        Me.cboClaimsRatingAgency.TableName = "Claims_Rating_Agency"
        Me.cboClaimsRatingAgency.ToolTipText = ""
        Me.cboClaimsRatingAgency.WhereClause = ""
        '
        'lblClaimsRatingDescription
        '
        Me.lblClaimsRatingDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsRatingDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsRatingDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimsRatingDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsRatingDescription.Location = New System.Drawing.Point(8, 112)
        Me.lblClaimsRatingDescription.Name = "lblClaimsRatingDescription"
        Me.lblClaimsRatingDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsRatingDescription.Size = New System.Drawing.Size(113, 17)
        Me.lblClaimsRatingDescription.TabIndex = 54
        Me.lblClaimsRatingDescription.Text = "Rating Description:"
        '
        'lblClaimsRatingAgency
        '
        Me.lblClaimsRatingAgency.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsRatingAgency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsRatingAgency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimsRatingAgency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsRatingAgency.Location = New System.Drawing.Point(8, 16)
        Me.lblClaimsRatingAgency.Name = "lblClaimsRatingAgency"
        Me.lblClaimsRatingAgency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsRatingAgency.Size = New System.Drawing.Size(73, 17)
        Me.lblClaimsRatingAgency.TabIndex = 62
        Me.lblClaimsRatingAgency.Text = "Agency:"
        '
        'lblClaimsRatingGrading
        '
        Me.lblClaimsRatingGrading.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsRatingGrading.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsRatingGrading.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimsRatingGrading.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsRatingGrading.Location = New System.Drawing.Point(8, 48)
        Me.lblClaimsRatingGrading.Name = "lblClaimsRatingGrading"
        Me.lblClaimsRatingGrading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsRatingGrading.Size = New System.Drawing.Size(81, 17)
        Me.lblClaimsRatingGrading.TabIndex = 61
        Me.lblClaimsRatingGrading.Text = "Grading:"
        '
        'lblClaimsRatingDate
        '
        Me.lblClaimsRatingDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsRatingDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsRatingDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimsRatingDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsRatingDate.Location = New System.Drawing.Point(8, 80)
        Me.lblClaimsRatingDate.Name = "lblClaimsRatingDate"
        Me.lblClaimsRatingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsRatingDate.Size = New System.Drawing.Size(97, 17)
        Me.lblClaimsRatingDate.TabIndex = 60
        Me.lblClaimsRatingDate.Text = "Date:"
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(552, 380)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_4.TabIndex = 72
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(8, 380)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_3.TabIndex = 74
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraBrokerlink)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "Brokerlink"
        '
        'fraBrokerlink
        '
        Me.fraBrokerlink.BackColor = System.Drawing.SystemColors.Control
        Me.fraBrokerlink.Controls.Add(Me.cboBrokerlinkSubAccount)
        Me.fraBrokerlink.Controls.Add(Me.txtBrokerlinkUnderwritingId)
        Me.fraBrokerlink.Controls.Add(Me.Label2)
        Me.fraBrokerlink.Controls.Add(Me.Label1)
        Me.fraBrokerlink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBrokerlink.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBrokerlink.Location = New System.Drawing.Point(8, 4)
        Me.fraBrokerlink.Name = "fraBrokerlink"
        Me.fraBrokerlink.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBrokerlink.Size = New System.Drawing.Size(585, 369)
        Me.fraBrokerlink.TabIndex = 65
        Me.fraBrokerlink.TabStop = False
        '
        'cboBrokerlinkSubAccount
        '
        Me.cboBrokerlinkSubAccount.BackColor = System.Drawing.SystemColors.Window
        Me.cboBrokerlinkSubAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBrokerlinkSubAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBrokerlinkSubAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBrokerlinkSubAccount.Location = New System.Drawing.Point(160, 64)
        Me.cboBrokerlinkSubAccount.Name = "cboBrokerlinkSubAccount"
        Me.cboBrokerlinkSubAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBrokerlinkSubAccount.Size = New System.Drawing.Size(153, 21)
        Me.cboBrokerlinkSubAccount.TabIndex = 67
        '
        'txtBrokerlinkUnderwritingId
        '
        Me.txtBrokerlinkUnderwritingId.AcceptsReturn = True
        Me.txtBrokerlinkUnderwritingId.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerlinkUnderwritingId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerlinkUnderwritingId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerlinkUnderwritingId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerlinkUnderwritingId.Location = New System.Drawing.Point(160, 32)
        Me.txtBrokerlinkUnderwritingId.MaxLength = 0
        Me.txtBrokerlinkUnderwritingId.Name = "txtBrokerlinkUnderwritingId"
        Me.txtBrokerlinkUnderwritingId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerlinkUnderwritingId.Size = New System.Drawing.Size(153, 19)
        Me.txtBrokerlinkUnderwritingId.TabIndex = 66
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(32, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 69
        Me.Label2.Text = "SubAccount:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(32, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 68
        Me.Label1.Text = "Underwriting ID:"
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(8, 380)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_4.TabIndex = 75
        Me._cmdPrevious_4.Text = "<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(552, 380)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_5.TabIndex = 82
        Me._cmdNext_5.Text = ">>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me._cmdPrevious_5)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraFSA)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(597, 455)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "FSA"
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(8, 380)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_5.TabIndex = 81
        Me._cmdPrevious_5.Text = "<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        'fraFSA
        '
        Me.fraFSA.BackColor = System.Drawing.SystemColors.Control
        Me.fraFSA.Controls.Add(Me.cmdEditRisk)
        Me.fraFSA.Controls.Add(Me.lvwRiskCodes)
        Me.fraFSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFSA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFSA.Location = New System.Drawing.Point(8, 4)
        Me.fraFSA.Name = "fraFSA"
        Me.fraFSA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFSA.Size = New System.Drawing.Size(585, 369)
        Me.fraFSA.TabIndex = 83
        Me.fraFSA.TabStop = False
        '
        'cmdEditRisk
        '
        Me.cmdEditRisk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRisk.Location = New System.Drawing.Point(504, 16)
        Me.cmdEditRisk.Name = "cmdEditRisk"
        Me.cmdEditRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRisk.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditRisk.TabIndex = 85
        Me.cmdEditRisk.Text = "&Edit"
        Me.cmdEditRisk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRisk.UseVisualStyleBackColor = False
        '
        'lvwRiskCodes
        '
        Me.lvwRiskCodes.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskCodes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRiskCodes_ColumnHeader_1, Me._lvwRiskCodes_ColumnHeader_2, Me._lvwRiskCodes_ColumnHeader_3})
        Me.lvwRiskCodes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRiskCodes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskCodes.FullRowSelect = True
        Me.lvwRiskCodes.Location = New System.Drawing.Point(8, 16)
        Me.lvwRiskCodes.Name = "lvwRiskCodes"
        Me.lvwRiskCodes.Size = New System.Drawing.Size(489, 345)
        Me.lvwRiskCodes.TabIndex = 84
        Me.lvwRiskCodes.UseCompatibleStateImageBehavior = False
        Me.lvwRiskCodes.View = System.Windows.Forms.View.Details
        '
        '_lvwRiskCodes_ColumnHeader_1
        '
        Me._lvwRiskCodes_ColumnHeader_1.Text = "Risk Code"
        Me._lvwRiskCodes_ColumnHeader_1.Width = 193
        '
        '_lvwRiskCodes_ColumnHeader_2
        '
        Me._lvwRiskCodes_ColumnHeader_2.Text = "Risk Transfer"
        Me._lvwRiskCodes_ColumnHeader_2.Width = 134
        '
        '_lvwRiskCodes_ColumnHeader_3
        '
        Me._lvwRiskCodes_ColumnHeader_3.Text = "Delegated Authority"
        Me._lvwRiskCodes_ColumnHeader_3.Width = 134
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(615, 549)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.Toolbar1)
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
        Me.Text = "Insurer"
        Me.Toolbar1.ResumeLayout(False)
        Me.Toolbar1.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me.fraReinsurance.ResumeLayout(False)
        Me.fraReinsurance.PerformLayout()
        Me.fraAppointment.ResumeLayout(False)
        Me.fraAppointment.PerformLayout()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.fraAddress.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraContact.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.fraTax.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraClaimRating.ResumeLayout(False)
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraBrokerlink.ResumeLayout(False)
        Me.fraBrokerlink.PerformLayout()
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraFSA.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializecmdPrevious()
		Me.cmdPrevious(5) = _cmdPrevious_5
		Me.cmdPrevious(4) = _cmdPrevious_4
		Me.cmdPrevious(3) = _cmdPrevious_3
		Me.cmdPrevious(2) = _cmdPrevious_2
		Me.cmdPrevious(1) = _cmdPrevious_1
		Me.cmdPrevious(0) = _cmdPrevious_0
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(5) = _cmdNext_5
		Me.cmdNext(4) = _cmdNext_4
		Me.cmdNext(3) = _cmdNext_3
		Me.cmdNext(2) = _cmdNext_2
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(0) = _cmdNext_0
	End Sub
#End Region 
End Class