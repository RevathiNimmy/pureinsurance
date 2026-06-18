<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializepnlMain()
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
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents txtMerchantId As System.Windows.Forms.TextBox
    Public WithEvents cboClientBankAccType As System.Windows.Forms.ComboBox
    Public WithEvents cboMoneyCalcAccType As System.Windows.Forms.ComboBox
    Public WithEvents cboAccountStatus As System.Windows.Forms.ComboBox
    Public WithEvents cboPurgefrequencyID As System.Windows.Forms.ComboBox
    Public WithEvents chkRestrictEnquiry As System.Windows.Forms.CheckBox
    Public WithEvents chkRestrictUpdate As System.Windows.Forms.CheckBox
    Public WithEvents chkDeleteAtPurge As System.Windows.Forms.CheckBox
    Public WithEvents cboAccounttypeID As System.Windows.Forms.ComboBox
    Public WithEvents cboLedgerID As System.Windows.Forms.ComboBox
    Public WithEvents txtAccountCode As System.Windows.Forms.TextBox
    Public WithEvents txtAccountName As System.Windows.Forms.TextBox
    Public WithEvents txtShortCode As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents uctAccountLookup As UserControls.AccountLookup
    Public WithEvents lblMerchantId As System.Windows.Forms.Label
    Public WithEvents lblMoneyCalcAccType As System.Windows.Forms.Label
    Public WithEvents lblClientBankAccType As System.Windows.Forms.Label
    Public WithEvents lblNominalCode As System.Windows.Forms.Label
    Public WithEvents lblAccountStatus As System.Windows.Forms.Label
    Public WithEvents lblPurgefrequencyID As System.Windows.Forms.Label
    Public WithEvents lblRestrictEnquiry As System.Windows.Forms.Label
    Public WithEvents lblRestrictUpdate As System.Windows.Forms.Label
    Public WithEvents lblDeleteAtPurge As System.Windows.Forms.Label
    Public WithEvents lblAccounttypeID As System.Windows.Forms.Label
    Public WithEvents lblLedgerID As System.Windows.Forms.Label
    Public WithEvents lblAccountCode As System.Windows.Forms.Label
    Public WithEvents lblAccountName As System.Windows.Forms.Label
    Public WithEvents lblShortCode As System.Windows.Forms.Label
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Private WithEvents _pnlMain_0 As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Public WithEvents txtPhoneExtension As System.Windows.Forms.TextBox
    Public WithEvents txtContactName As System.Windows.Forms.TextBox
    Public WithEvents txtAddress3 As System.Windows.Forms.TextBox
    Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Public WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Public WithEvents txtAddress4 As System.Windows.Forms.TextBox
    Public WithEvents txtPostalCode As System.Windows.Forms.TextBox
    Public WithEvents cboAddressCountry As System.Windows.Forms.ComboBox
    Public WithEvents txtPhoneAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtPhoneNumber As System.Windows.Forms.TextBox
    Public WithEvents txtFaxAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtFaxNumber As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Public WithEvents lblExtension As System.Windows.Forms.Label
    Public WithEvents lblContactName As System.Windows.Forms.Label
    Public WithEvents lblAddress1 As System.Windows.Forms.Label
    Public WithEvents lblAddress2 As System.Windows.Forms.Label
    Public WithEvents lblAddresss3 As System.Windows.Forms.Label
    Public WithEvents lblAddress4 As System.Windows.Forms.Label
    Public WithEvents lblPostalCode As System.Windows.Forms.Label
    Public WithEvents lblAddressCountry As System.Windows.Forms.Label
    Public WithEvents lblTelephone As System.Windows.Forms.Label
    Public WithEvents lblFax As System.Windows.Forms.Label
    Public WithEvents lblAreaCode As System.Windows.Forms.Label
    Public WithEvents lblNumber As System.Windows.Forms.Label
    Private WithEvents _pnlMain_1 As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents chkElectronicPayment As System.Windows.Forms.CheckBox
    Public WithEvents cboPaymentTypeID As PMLookupControl.cboPMLookup
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Public WithEvents cboBordereauReportID As System.Windows.Forms.ComboBox
    Public WithEvents cboProofListReportID As System.Windows.Forms.ComboBox
    Public WithEvents txtSettlementPeriod As System.Windows.Forms.TextBox
    Public WithEvents txtDiscountPercentage As System.Windows.Forms.TextBox
    Public WithEvents txtCreditLimit As System.Windows.Forms.TextBox
    Public WithEvents lblDays As System.Windows.Forms.Label
    Public WithEvents lblSettlementPeriod As System.Windows.Forms.Label
    Public WithEvents lblDiscountPercentage As System.Windows.Forms.Label
    Public WithEvents lblCreditLimit As System.Windows.Forms.Label
    Public WithEvents fraSettlementTerms As System.Windows.Forms.GroupBox
    Public WithEvents txtPaymentExpiryDate As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentReference2 As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentReference1 As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentBranchCode As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentAccountCode As System.Windows.Forms.TextBox
    Public WithEvents txtPaymentName As System.Windows.Forms.TextBox
    Public WithEvents lblElectronicPayment As System.Windows.Forms.Label
    Public WithEvents lblBordereauReportID As System.Windows.Forms.Label
    Public WithEvents lblProofListReportID As System.Windows.Forms.Label
    Public WithEvents lblPaymentAccountCode As System.Windows.Forms.Label
    Public WithEvents lblPaymentBranchCode As System.Windows.Forms.Label
    Public WithEvents lblPaymentExpiryDate As System.Windows.Forms.Label
    Public WithEvents lblPaymentReference2 As System.Windows.Forms.Label
    Public WithEvents lblPaymentReference1 As System.Windows.Forms.Label
    Public WithEvents lblPaymenttypeID As System.Windows.Forms.Label
    Public WithEvents lblPaymentName As System.Windows.Forms.Label
    Private WithEvents _pnlMain_2 As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Public WithEvents txtBankPhoneAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtBankFaxNumber As System.Windows.Forms.TextBox
    Public WithEvents txtBankFaxAreaCode As System.Windows.Forms.TextBox
    Public WithEvents txtBankPhoneNumber As System.Windows.Forms.TextBox
    Public WithEvents cboBankCountry As System.Windows.Forms.ComboBox
    Public WithEvents txtBankPostalCode As System.Windows.Forms.TextBox
    Public WithEvents txtBankAddress4 As System.Windows.Forms.TextBox
    Public WithEvents txtBankAddress2 As System.Windows.Forms.TextBox
    Public WithEvents txtBankAddress1 As System.Windows.Forms.TextBox
    Public WithEvents txtBankAddress3 As System.Windows.Forms.TextBox
    Public WithEvents txtBankName As System.Windows.Forms.TextBox
    Public WithEvents txtBankPhoneExtension As System.Windows.Forms.TextBox
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Public WithEvents lblBankAreaCode As System.Windows.Forms.Label
    Public WithEvents lblBankNumber As System.Windows.Forms.Label
    Public WithEvents lblBankFax As System.Windows.Forms.Label
    Public WithEvents lblBankPhone As System.Windows.Forms.Label
    Public WithEvents lblBankCountry As System.Windows.Forms.Label
    Public WithEvents lblBankPostalCode As System.Windows.Forms.Label
    Public WithEvents lblBankAddress4 As System.Windows.Forms.Label
    Public WithEvents lblBankAddress3 As System.Windows.Forms.Label
    Public WithEvents lblBankAddress2 As System.Windows.Forms.Label
    Public WithEvents lblBankAddress1 As System.Windows.Forms.Label
    Public WithEvents lblBankName As System.Windows.Forms.Label
    Public WithEvents lblBankExtension As System.Windows.Forms.Label
    Private WithEvents _pnlMain_3 As System.Windows.Forms.Panel
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Public WithEvents txtComments As System.Windows.Forms.TextBox
    Private WithEvents _pnlMain_4 As System.Windows.Forms.Panel
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
	Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents uctLinkedAccountsControl As uctLinkedAccountsCtrl.uctLinkedAccounts
	Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
	Public WithEvents fraCashDeposit As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public cmdNext(5) As System.Windows.Forms.Button
	Public cmdPrevious(5) As System.Windows.Forms.Button
    Public pnlMain(4) As System.Windows.Forms.Panel

	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._pnlMain_0 = New System.Windows.Forms.Panel
        Me.txtMerchantId = New System.Windows.Forms.TextBox
        Me.cboClientBankAccType = New System.Windows.Forms.ComboBox
        Me.cboMoneyCalcAccType = New System.Windows.Forms.ComboBox
        Me.cboAccountStatus = New System.Windows.Forms.ComboBox
        Me.cboPurgefrequencyID = New System.Windows.Forms.ComboBox
        Me.chkRestrictEnquiry = New System.Windows.Forms.CheckBox
        Me.chkRestrictUpdate = New System.Windows.Forms.CheckBox
        Me.chkDeleteAtPurge = New System.Windows.Forms.CheckBox
        Me.cboAccounttypeID = New System.Windows.Forms.ComboBox
        Me.cboLedgerID = New System.Windows.Forms.ComboBox
        Me.txtAccountCode = New System.Windows.Forms.TextBox
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.txtShortCode = New System.Windows.Forms.TextBox
        Me._cmdNext_0 = New System.Windows.Forms.Button
        Me.uctAccountLookup = New UserControls.AccountLookup
        Me.lblMerchantId = New System.Windows.Forms.Label
        Me.lblMoneyCalcAccType = New System.Windows.Forms.Label
        Me.lblClientBankAccType = New System.Windows.Forms.Label
        Me.lblNominalCode = New System.Windows.Forms.Label
        Me.lblAccountStatus = New System.Windows.Forms.Label
        Me.lblPurgefrequencyID = New System.Windows.Forms.Label
        Me.lblRestrictEnquiry = New System.Windows.Forms.Label
        Me.lblRestrictUpdate = New System.Windows.Forms.Label
        Me.lblDeleteAtPurge = New System.Windows.Forms.Label
        Me.lblAccounttypeID = New System.Windows.Forms.Label
        Me.lblLedgerID = New System.Windows.Forms.Label
        Me.lblAccountCode = New System.Windows.Forms.Label
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblShortCode = New System.Windows.Forms.Label
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage
        Me._pnlMain_1 = New System.Windows.Forms.Panel
        Me._cmdPrevious_0 = New System.Windows.Forms.Button
        Me.txtPhoneExtension = New System.Windows.Forms.TextBox
        Me.txtContactName = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.txtPostalCode = New System.Windows.Forms.TextBox
        Me.cboAddressCountry = New System.Windows.Forms.ComboBox
        Me.txtPhoneAreaCode = New System.Windows.Forms.TextBox
        Me.txtPhoneNumber = New System.Windows.Forms.TextBox
        Me.txtFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtFaxNumber = New System.Windows.Forms.TextBox
        Me._cmdNext_1 = New System.Windows.Forms.Button
        Me.lblExtension = New System.Windows.Forms.Label
        Me.lblContactName = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblAddress2 = New System.Windows.Forms.Label
        Me.lblAddresss3 = New System.Windows.Forms.Label
        Me.lblAddress4 = New System.Windows.Forms.Label
        Me.lblPostalCode = New System.Windows.Forms.Label
        Me.lblAddressCountry = New System.Windows.Forms.Label
        Me.lblTelephone = New System.Windows.Forms.Label
        Me.lblFax = New System.Windows.Forms.Label
        Me.lblAreaCode = New System.Windows.Forms.Label
        Me.lblNumber = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage
        Me._pnlMain_2 = New System.Windows.Forms.Panel
        Me.chkElectronicPayment = New System.Windows.Forms.CheckBox
        Me.cboPaymentTypeID = New PMLookupControl.cboPMLookup
        Me._cmdNext_2 = New System.Windows.Forms.Button
        Me._cmdPrevious_1 = New System.Windows.Forms.Button
        Me.cboBordereauReportID = New System.Windows.Forms.ComboBox
        Me.cboProofListReportID = New System.Windows.Forms.ComboBox
        Me.fraSettlementTerms = New System.Windows.Forms.GroupBox
        Me.txtSettlementPeriod = New System.Windows.Forms.TextBox
        Me.txtDiscountPercentage = New System.Windows.Forms.TextBox
        Me.txtCreditLimit = New System.Windows.Forms.TextBox
        Me.lblDays = New System.Windows.Forms.Label
        Me.lblSettlementPeriod = New System.Windows.Forms.Label
        Me.lblDiscountPercentage = New System.Windows.Forms.Label
        Me.lblCreditLimit = New System.Windows.Forms.Label
        Me.txtPaymentExpiryDate = New System.Windows.Forms.TextBox
        Me.txtPaymentReference2 = New System.Windows.Forms.TextBox
        Me.txtPaymentReference1 = New System.Windows.Forms.TextBox
        Me.txtPaymentBranchCode = New System.Windows.Forms.TextBox
        Me.txtPaymentAccountCode = New System.Windows.Forms.TextBox
        Me.txtPaymentName = New System.Windows.Forms.TextBox
        Me.lblElectronicPayment = New System.Windows.Forms.Label
        Me.lblBordereauReportID = New System.Windows.Forms.Label
        Me.lblProofListReportID = New System.Windows.Forms.Label
        Me.lblPaymentAccountCode = New System.Windows.Forms.Label
        Me.lblPaymentBranchCode = New System.Windows.Forms.Label
        Me.lblPaymentExpiryDate = New System.Windows.Forms.Label
        Me.lblPaymentReference2 = New System.Windows.Forms.Label
        Me.lblPaymentReference1 = New System.Windows.Forms.Label
        Me.lblPaymenttypeID = New System.Windows.Forms.Label
        Me.lblPaymentName = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage
        Me._pnlMain_3 = New System.Windows.Forms.Panel
        Me._cmdPrevious_2 = New System.Windows.Forms.Button
        Me.txtBankPhoneAreaCode = New System.Windows.Forms.TextBox
        Me.txtBankFaxNumber = New System.Windows.Forms.TextBox
        Me.txtBankFaxAreaCode = New System.Windows.Forms.TextBox
        Me.txtBankPhoneNumber = New System.Windows.Forms.TextBox
        Me.cboBankCountry = New System.Windows.Forms.ComboBox
        Me.txtBankPostalCode = New System.Windows.Forms.TextBox
        Me.txtBankAddress4 = New System.Windows.Forms.TextBox
        Me.txtBankAddress2 = New System.Windows.Forms.TextBox
        Me.txtBankAddress1 = New System.Windows.Forms.TextBox
        Me.txtBankAddress3 = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.txtBankPhoneExtension = New System.Windows.Forms.TextBox
        Me._cmdNext_3 = New System.Windows.Forms.Button
        Me.lblBankAreaCode = New System.Windows.Forms.Label
        Me.lblBankNumber = New System.Windows.Forms.Label
        Me.lblBankFax = New System.Windows.Forms.Label
        Me.lblBankPhone = New System.Windows.Forms.Label
        Me.lblBankCountry = New System.Windows.Forms.Label
        Me.lblBankPostalCode = New System.Windows.Forms.Label
        Me.lblBankAddress4 = New System.Windows.Forms.Label
        Me.lblBankAddress3 = New System.Windows.Forms.Label
        Me.lblBankAddress2 = New System.Windows.Forms.Label
        Me.lblBankAddress1 = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblBankExtension = New System.Windows.Forms.Label
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage
        Me._pnlMain_4 = New System.Windows.Forms.Panel
        Me._cmdPrevious_3 = New System.Windows.Forms.Button
        Me.txtComments = New System.Windows.Forms.TextBox
        Me._cmdNext_4 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage
        Me.uctPartyBankControl2 = New uctPartyBank.uctPartyBankControl
        Me._cmdPrevious_4 = New System.Windows.Forms.Button
        Me._cmdNext_5 = New System.Windows.Forms.Button
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage
        Me.fraCashDeposit = New System.Windows.Forms.GroupBox
        Me.uctLinkedAccountsControl = New uctLinkedAccountsCtrl.uctLinkedAccounts
        Me._cmdPrevious_5 = New System.Windows.Forms.Button
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me._pnlMain_0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._pnlMain_1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._pnlMain_2.SuspendLayout()
        Me.fraSettlementTerms.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me._pnlMain_3.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me._pnlMain_4.SuspendLayout()
        Me._tabMainTab_TabPage5.SuspendLayout()
        Me._tabMainTab_TabPage6.SuspendLayout()
        Me.fraCashDeposit.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(422, 522)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 100
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Confirm Changes")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 522)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 99
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(582, 522)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 102
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
        Me.cmdCancel.Location = New System.Drawing.Point(502, 522)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 101
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(91, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(705, 508)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._pnlMain_0)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(697, 464)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Account Details"
        '
        '_pnlMain_0
        '
        Me._pnlMain_0.Controls.Add(Me.txtMerchantId)
        Me._pnlMain_0.Controls.Add(Me.cboClientBankAccType)
        Me._pnlMain_0.Controls.Add(Me.cboMoneyCalcAccType)
        Me._pnlMain_0.Controls.Add(Me.cboAccountStatus)
        Me._pnlMain_0.Controls.Add(Me.cboPurgefrequencyID)
        Me._pnlMain_0.Controls.Add(Me.chkRestrictEnquiry)
        Me._pnlMain_0.Controls.Add(Me.chkRestrictUpdate)
        Me._pnlMain_0.Controls.Add(Me.chkDeleteAtPurge)
        Me._pnlMain_0.Controls.Add(Me.cboAccounttypeID)
        Me._pnlMain_0.Controls.Add(Me.cboLedgerID)
        Me._pnlMain_0.Controls.Add(Me.txtAccountCode)
        Me._pnlMain_0.Controls.Add(Me.txtAccountName)
        Me._pnlMain_0.Controls.Add(Me.txtShortCode)
        Me._pnlMain_0.Controls.Add(Me._cmdNext_0)
        Me._pnlMain_0.Controls.Add(Me.uctAccountLookup)
        Me._pnlMain_0.Controls.Add(Me.lblMerchantId)
        Me._pnlMain_0.Controls.Add(Me.lblMoneyCalcAccType)
        Me._pnlMain_0.Controls.Add(Me.lblClientBankAccType)
        Me._pnlMain_0.Controls.Add(Me.lblNominalCode)
        Me._pnlMain_0.Controls.Add(Me.lblAccountStatus)
        Me._pnlMain_0.Controls.Add(Me.lblPurgefrequencyID)
        Me._pnlMain_0.Controls.Add(Me.lblRestrictEnquiry)
        Me._pnlMain_0.Controls.Add(Me.lblRestrictUpdate)
        Me._pnlMain_0.Controls.Add(Me.lblDeleteAtPurge)
        Me._pnlMain_0.Controls.Add(Me.lblAccounttypeID)
        Me._pnlMain_0.Controls.Add(Me.lblLedgerID)
        Me._pnlMain_0.Controls.Add(Me.lblAccountCode)
        Me._pnlMain_0.Controls.Add(Me.lblAccountName)
        Me._pnlMain_0.Controls.Add(Me.lblShortCode)
        Me._pnlMain_0.Controls.Add(Me.imgIcon)
        Me._pnlMain_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_0.Location = New System.Drawing.Point(62, 10)
        Me._pnlMain_0.Name = "_pnlMain_0"
        Me._pnlMain_0.Size = New System.Drawing.Size(471, 447)
        Me._pnlMain_0.TabIndex = 50
        '
        'txtMerchantId
        '
        Me.txtMerchantId.AcceptsReturn = True
        Me.txtMerchantId.BackColor = System.Drawing.SystemColors.Window
        Me.txtMerchantId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMerchantId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMerchantId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMerchantId.Location = New System.Drawing.Point(164, 80)
        Me.txtMerchantId.MaxLength = 20
        Me.txtMerchantId.Name = "txtMerchantId"
        Me.txtMerchantId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMerchantId.Size = New System.Drawing.Size(171, 21)
        Me.txtMerchantId.TabIndex = 121
        '
        'cboClientBankAccType
        '
        Me.cboClientBankAccType.BackColor = System.Drawing.SystemColors.Window
        Me.cboClientBankAccType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClientBankAccType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClientBankAccType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClientBankAccType.Location = New System.Drawing.Point(164, 392)
        Me.cboClientBankAccType.Name = "cboClientBankAccType"
        Me.cboClientBankAccType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClientBankAccType.Size = New System.Drawing.Size(171, 21)
        Me.cboClientBankAccType.TabIndex = 120
        '
        'cboMoneyCalcAccType
        '
        Me.cboMoneyCalcAccType.BackColor = System.Drawing.SystemColors.Window
        Me.cboMoneyCalcAccType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMoneyCalcAccType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMoneyCalcAccType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMoneyCalcAccType.Location = New System.Drawing.Point(164, 360)
        Me.cboMoneyCalcAccType.Name = "cboMoneyCalcAccType"
        Me.cboMoneyCalcAccType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMoneyCalcAccType.Size = New System.Drawing.Size(171, 21)
        Me.cboMoneyCalcAccType.TabIndex = 119
        Me.cboMoneyCalcAccType.Text = "cboMoneyCalcAccType"
        '
        'cboAccountStatus
        '
        Me.cboAccountStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccountStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccountStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccountStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccountStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAccountStatus.Location = New System.Drawing.Point(164, 424)
        Me.cboAccountStatus.Name = "cboAccountStatus"
        Me.cboAccountStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccountStatus.Size = New System.Drawing.Size(171, 21)
        Me.cboAccountStatus.TabIndex = 104
        '
        'cboPurgefrequencyID
        '
        Me.cboPurgefrequencyID.BackColor = System.Drawing.SystemColors.Window
        Me.cboPurgefrequencyID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPurgefrequencyID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPurgefrequencyID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPurgefrequencyID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPurgefrequencyID.Location = New System.Drawing.Point(164, 271)
        Me.cboPurgefrequencyID.Name = "cboPurgefrequencyID"
        Me.cboPurgefrequencyID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPurgefrequencyID.Size = New System.Drawing.Size(171, 21)
        Me.cboPurgefrequencyID.TabIndex = 8
        '
        'chkRestrictEnquiry
        '
        Me.chkRestrictEnquiry.BackColor = System.Drawing.SystemColors.Control
        Me.chkRestrictEnquiry.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRestrictEnquiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRestrictEnquiry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRestrictEnquiry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.chkRestrictEnquiry.Location = New System.Drawing.Point(164, 208)
        Me.chkRestrictEnquiry.Name = "chkRestrictEnquiry"
        Me.chkRestrictEnquiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRestrictEnquiry.Size = New System.Drawing.Size(15, 21)
        Me.chkRestrictEnquiry.TabIndex = 6
        Me.chkRestrictEnquiry.UseVisualStyleBackColor = False
        '
        'chkRestrictUpdate
        '
        Me.chkRestrictUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.chkRestrictUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRestrictUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRestrictUpdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRestrictUpdate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.chkRestrictUpdate.Location = New System.Drawing.Point(164, 232)
        Me.chkRestrictUpdate.Name = "chkRestrictUpdate"
        Me.chkRestrictUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRestrictUpdate.Size = New System.Drawing.Size(15, 21)
        Me.chkRestrictUpdate.TabIndex = 7
        Me.chkRestrictUpdate.UseVisualStyleBackColor = False
        '
        'chkDeleteAtPurge
        '
        Me.chkDeleteAtPurge.BackColor = System.Drawing.SystemColors.Control
        Me.chkDeleteAtPurge.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDeleteAtPurge.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDeleteAtPurge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDeleteAtPurge.ForeColor = System.Drawing.SystemColors.WindowText
        Me.chkDeleteAtPurge.Location = New System.Drawing.Point(164, 296)
        Me.chkDeleteAtPurge.Name = "chkDeleteAtPurge"
        Me.chkDeleteAtPurge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDeleteAtPurge.Size = New System.Drawing.Size(15, 21)
        Me.chkDeleteAtPurge.TabIndex = 9
        Me.chkDeleteAtPurge.UseVisualStyleBackColor = False
        '
        'cboAccounttypeID
        '
        Me.cboAccounttypeID.BackColor = System.Drawing.SystemColors.Window
        Me.cboAccounttypeID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAccounttypeID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccounttypeID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAccounttypeID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAccounttypeID.Location = New System.Drawing.Point(164, 176)
        Me.cboAccounttypeID.Name = "cboAccounttypeID"
        Me.cboAccounttypeID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAccounttypeID.Size = New System.Drawing.Size(171, 21)
        Me.cboAccounttypeID.TabIndex = 5
        '
        'cboLedgerID
        '
        Me.cboLedgerID.BackColor = System.Drawing.SystemColors.Window
        Me.cboLedgerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLedgerID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLedgerID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLedgerID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLedgerID.Location = New System.Drawing.Point(164, 143)
        Me.cboLedgerID.Name = "cboLedgerID"
        Me.cboLedgerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLedgerID.Size = New System.Drawing.Size(171, 21)
        Me.cboLedgerID.TabIndex = 4
        '
        'txtAccountCode
        '
        Me.txtAccountCode.AcceptsReturn = True
        Me.txtAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountCode.Enabled = False
        Me.txtAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountCode.Location = New System.Drawing.Point(164, 112)
        Me.txtAccountCode.MaxLength = 0
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountCode.Size = New System.Drawing.Size(291, 21)
        Me.txtAccountCode.TabIndex = 3
        '
        'txtAccountName
        '
        Me.txtAccountName.AcceptsReturn = True
        Me.txtAccountName.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountName.Location = New System.Drawing.Point(164, 48)
        Me.txtAccountName.MaxLength = 0
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountName.Size = New System.Drawing.Size(291, 21)
        Me.txtAccountName.TabIndex = 2
        '
        'txtShortCode
        '
        Me.txtShortCode.AcceptsReturn = True
        Me.txtShortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortCode.Location = New System.Drawing.Point(164, 16)
        Me.txtShortCode.MaxLength = 0
        Me.txtShortCode.Name = "txtShortCode"
        Me.txtShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortCode.Size = New System.Drawing.Size(155, 21)
        Me.txtShortCode.TabIndex = 1
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(400, 400)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_0.TabIndex = 10
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'uctAccountLookup
        '
        Me.uctAccountLookup.AccountId = 0
        Me.uctAccountLookup.AllowStoppedAccounts = False
        Me.uctAccountLookup.BackStyle = 0
        Me.uctAccountLookup.CompanyId = 0
        Me.uctAccountLookup.Default_Renamed = False
        Me.uctAccountLookup.Location = New System.Drawing.Point(164, 328)
        Me.uctAccountLookup.LookupCaption = "..."
        Me.uctAccountLookup.LookupHeight = 285
        Me.uctAccountLookup.LookupLeft = 2205
        Me.uctAccountLookup.LookupTextLeft = 0
        Me.uctAccountLookup.LookupTextWidth = 2205
        Me.uctAccountLookup.LookupWidth = 360
        Me.uctAccountLookup.Name = "uctAccountLookup"
        Me.uctAccountLookup.OnlyUpdatableAccounts = False
        Me.uctAccountLookup.SelLength = 0
        Me.uctAccountLookup.SelStart = 0
        Me.uctAccountLookup.SelText = ""
        Me.uctAccountLookup.ShowEditOnFindAccount = False
        Me.uctAccountLookup.Size = New System.Drawing.Size(171, 19)
        Me.uctAccountLookup.TabIndex = 105
        Me.uctAccountLookup.ToolTipText = ""
        '
        'lblMerchantId
        '
        Me.lblMerchantId.BackColor = System.Drawing.SystemColors.Control
        Me.lblMerchantId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMerchantId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMerchantId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMerchantId.Location = New System.Drawing.Point(8, 80)
        Me.lblMerchantId.Name = "lblMerchantId"
        Me.lblMerchantId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMerchantId.Size = New System.Drawing.Size(140, 33)
        Me.lblMerchantId.TabIndex = 122
        Me.lblMerchantId.Text = "&Merchant ID/Sun reference:"
        '
        'lblMoneyCalcAccType
        '
        Me.lblMoneyCalcAccType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMoneyCalcAccType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMoneyCalcAccType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMoneyCalcAccType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMoneyCalcAccType.Location = New System.Drawing.Point(8, 360)
        Me.lblMoneyCalcAccType.Name = "lblMoneyCalcAccType"
        Me.lblMoneyCalcAccType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMoneyCalcAccType.Size = New System.Drawing.Size(147, 33)
        Me.lblMoneyCalcAccType.TabIndex = 118
        Me.lblMoneyCalcAccType.Text = "Client &Money Calculation Accout Type:"
        '
        'lblClientBankAccType
        '
        Me.lblClientBankAccType.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientBankAccType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientBankAccType.Enabled = False
        Me.lblClientBankAccType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientBankAccType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientBankAccType.Location = New System.Drawing.Point(8, 392)
        Me.lblClientBankAccType.Name = "lblClientBankAccType"
        Me.lblClientBankAccType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientBankAccType.Size = New System.Drawing.Size(139, 33)
        Me.lblClientBankAccType.TabIndex = 117
        Me.lblClientBankAccType.Text = "Client &Bank Account Type:"
        '
        'lblNominalCode
        '
        Me.lblNominalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblNominalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNominalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNominalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNominalCode.Location = New System.Drawing.Point(8, 329)
        Me.lblNominalCode.Name = "lblNominalCode"
        Me.lblNominalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNominalCode.Size = New System.Drawing.Size(105, 17)
        Me.lblNominalCode.TabIndex = 106
        Me.lblNominalCode.Text = "&Nominal Code:"
        '
        'lblAccountStatus
        '
        Me.lblAccountStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountStatus.Location = New System.Drawing.Point(8, 426)
        Me.lblAccountStatus.Name = "lblAccountStatus"
        Me.lblAccountStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountStatus.Size = New System.Drawing.Size(115, 17)
        Me.lblAccountStatus.TabIndex = 103
        Me.lblAccountStatus.Text = "&Account Status:"
        '
        'lblPurgefrequencyID
        '
        Me.lblPurgefrequencyID.BackColor = System.Drawing.SystemColors.Control
        Me.lblPurgefrequencyID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurgefrequencyID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurgefrequencyID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPurgefrequencyID.Location = New System.Drawing.Point(8, 273)
        Me.lblPurgefrequencyID.Name = "lblPurgefrequencyID"
        Me.lblPurgefrequencyID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurgefrequencyID.Size = New System.Drawing.Size(118, 17)
        Me.lblPurgefrequencyID.TabIndex = 63
        Me.lblPurgefrequencyID.Text = "&Purge Frequency:"
        '
        'lblRestrictEnquiry
        '
        Me.lblRestrictEnquiry.BackColor = System.Drawing.SystemColors.Control
        Me.lblRestrictEnquiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRestrictEnquiry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRestrictEnquiry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblRestrictEnquiry.Location = New System.Drawing.Point(8, 210)
        Me.lblRestrictEnquiry.Name = "lblRestrictEnquiry"
        Me.lblRestrictEnquiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRestrictEnquiry.Size = New System.Drawing.Size(100, 17)
        Me.lblRestrictEnquiry.TabIndex = 62
        Me.lblRestrictEnquiry.Text = "Restrict &Enquiry:"
        '
        'lblRestrictUpdate
        '
        Me.lblRestrictUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRestrictUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRestrictUpdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRestrictUpdate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblRestrictUpdate.Location = New System.Drawing.Point(8, 234)
        Me.lblRestrictUpdate.Name = "lblRestrictUpdate"
        Me.lblRestrictUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRestrictUpdate.Size = New System.Drawing.Size(100, 17)
        Me.lblRestrictUpdate.TabIndex = 61
        Me.lblRestrictUpdate.Text = "Restrict &Update:"
        '
        'lblDeleteAtPurge
        '
        Me.lblDeleteAtPurge.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeleteAtPurge.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeleteAtPurge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeleteAtPurge.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDeleteAtPurge.Location = New System.Drawing.Point(8, 298)
        Me.lblDeleteAtPurge.Name = "lblDeleteAtPurge"
        Me.lblDeleteAtPurge.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeleteAtPurge.Size = New System.Drawing.Size(100, 17)
        Me.lblDeleteAtPurge.TabIndex = 60
        Me.lblDeleteAtPurge.Text = "&Delete at Purge:"
        '
        'lblAccounttypeID
        '
        Me.lblAccounttypeID.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccounttypeID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccounttypeID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccounttypeID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccounttypeID.Location = New System.Drawing.Point(8, 178)
        Me.lblAccounttypeID.Name = "lblAccounttypeID"
        Me.lblAccounttypeID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccounttypeID.Size = New System.Drawing.Size(100, 17)
        Me.lblAccounttypeID.TabIndex = 59
        Me.lblAccounttypeID.Text = "Account &Type:"
        '
        'lblLedgerID
        '
        Me.lblLedgerID.BackColor = System.Drawing.SystemColors.Control
        Me.lblLedgerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLedgerID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLedgerID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLedgerID.Location = New System.Drawing.Point(8, 145)
        Me.lblLedgerID.Name = "lblLedgerID"
        Me.lblLedgerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLedgerID.Size = New System.Drawing.Size(100, 17)
        Me.lblLedgerID.TabIndex = 58
        Me.lblLedgerID.Text = "&Ledger:"
        '
        'lblAccountCode
        '
        Me.lblAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountCode.Location = New System.Drawing.Point(8, 114)
        Me.lblAccountCode.Name = "lblAccountCode"
        Me.lblAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountCode.Size = New System.Drawing.Size(100, 17)
        Me.lblAccountCode.TabIndex = 57
        Me.lblAccountCode.Text = "&Code:"
        '
        'lblAccountName
        '
        Me.lblAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccountName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccountName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccountName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccountName.Location = New System.Drawing.Point(8, 50)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccountName.Size = New System.Drawing.Size(100, 17)
        Me.lblAccountName.TabIndex = 56
        Me.lblAccountName.Text = "&Name:"
        '
        'lblShortCode
        '
        Me.lblShortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortCode.Location = New System.Drawing.Point(8, 18)
        Me.lblShortCode.Name = "lblShortCode"
        Me.lblShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortCode.Size = New System.Drawing.Size(100, 17)
        Me.lblShortCode.TabIndex = 55
        Me.lblShortCode.Text = "&Short Name:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(430, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 123
        Me.imgIcon.TabStop = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._pnlMain_1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(697, 464)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "&2 - Account Address"
        '
        '_pnlMain_1
        '
        Me._pnlMain_1.Controls.Add(Me._cmdPrevious_0)
        Me._pnlMain_1.Controls.Add(Me.txtPhoneExtension)
        Me._pnlMain_1.Controls.Add(Me.txtContactName)
        Me._pnlMain_1.Controls.Add(Me.txtAddress3)
        Me._pnlMain_1.Controls.Add(Me.txtAddress1)
        Me._pnlMain_1.Controls.Add(Me.txtAddress2)
        Me._pnlMain_1.Controls.Add(Me.txtAddress4)
        Me._pnlMain_1.Controls.Add(Me.txtPostalCode)
        Me._pnlMain_1.Controls.Add(Me.cboAddressCountry)
        Me._pnlMain_1.Controls.Add(Me.txtPhoneAreaCode)
        Me._pnlMain_1.Controls.Add(Me.txtPhoneNumber)
        Me._pnlMain_1.Controls.Add(Me.txtFaxAreaCode)
        Me._pnlMain_1.Controls.Add(Me.txtFaxNumber)
        Me._pnlMain_1.Controls.Add(Me._cmdNext_1)
        Me._pnlMain_1.Controls.Add(Me.lblExtension)
        Me._pnlMain_1.Controls.Add(Me.lblContactName)
        Me._pnlMain_1.Controls.Add(Me.lblAddress1)
        Me._pnlMain_1.Controls.Add(Me.lblAddress2)
        Me._pnlMain_1.Controls.Add(Me.lblAddresss3)
        Me._pnlMain_1.Controls.Add(Me.lblAddress4)
        Me._pnlMain_1.Controls.Add(Me.lblPostalCode)
        Me._pnlMain_1.Controls.Add(Me.lblAddressCountry)
        Me._pnlMain_1.Controls.Add(Me.lblTelephone)
        Me._pnlMain_1.Controls.Add(Me.lblFax)
        Me._pnlMain_1.Controls.Add(Me.lblAreaCode)
        Me._pnlMain_1.Controls.Add(Me.lblNumber)
        Me._pnlMain_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_1.Location = New System.Drawing.Point(62, 10)
        Me._pnlMain_1.Name = "_pnlMain_1"
        Me._pnlMain_1.Size = New System.Drawing.Size(437, 413)
        Me._pnlMain_1.TabIndex = 51
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(368, 384)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_0.TabIndex = 109
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        'txtPhoneExtension
        '
        Me.txtPhoneExtension.AcceptsReturn = True
        Me.txtPhoneExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneExtension.Location = New System.Drawing.Point(320, 200)
        Me.txtPhoneExtension.MaxLength = 0
        Me.txtPhoneExtension.Name = "txtPhoneExtension"
        Me.txtPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneExtension.Size = New System.Drawing.Size(81, 21)
        Me.txtPhoneExtension.TabIndex = 20
        '
        'txtContactName
        '
        Me.txtContactName.AcceptsReturn = True
        Me.txtContactName.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactName.Location = New System.Drawing.Point(112, 16)
        Me.txtContactName.MaxLength = 0
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtContactName.Size = New System.Drawing.Size(289, 21)
        Me.txtContactName.TabIndex = 11
        '
        'txtAddress3
        '
        Me.txtAddress3.AcceptsReturn = True
        Me.txtAddress3.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress3.Location = New System.Drawing.Point(112, 88)
        Me.txtAddress3.MaxLength = 0
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress3.Size = New System.Drawing.Size(289, 21)
        Me.txtAddress3.TabIndex = 14
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(112, 43)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(289, 21)
        Me.txtAddress1.TabIndex = 12
        '
        'txtAddress2
        '
        Me.txtAddress2.AcceptsReturn = True
        Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress2.Location = New System.Drawing.Point(112, 66)
        Me.txtAddress2.MaxLength = 0
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress2.Size = New System.Drawing.Size(289, 21)
        Me.txtAddress2.TabIndex = 13
        '
        'txtAddress4
        '
        Me.txtAddress4.AcceptsReturn = True
        Me.txtAddress4.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress4.Location = New System.Drawing.Point(112, 112)
        Me.txtAddress4.MaxLength = 0
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress4.Size = New System.Drawing.Size(289, 21)
        Me.txtAddress4.TabIndex = 15
        '
        'txtPostalCode
        '
        Me.txtPostalCode.AcceptsReturn = True
        Me.txtPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPostalCode.Location = New System.Drawing.Point(112, 133)
        Me.txtPostalCode.MaxLength = 0
        Me.txtPostalCode.Name = "txtPostalCode"
        Me.txtPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPostalCode.Size = New System.Drawing.Size(153, 21)
        Me.txtPostalCode.TabIndex = 16
        '
        'cboAddressCountry
        '
        Me.cboAddressCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cboAddressCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAddressCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAddressCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAddressCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAddressCountry.Location = New System.Drawing.Point(112, 160)
        Me.cboAddressCountry.Name = "cboAddressCountry"
        Me.cboAddressCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAddressCountry.Size = New System.Drawing.Size(289, 21)
        Me.cboAddressCountry.TabIndex = 17
        '
        'txtPhoneAreaCode
        '
        Me.txtPhoneAreaCode.AcceptsReturn = True
        Me.txtPhoneAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneAreaCode.Location = New System.Drawing.Point(112, 200)
        Me.txtPhoneAreaCode.MaxLength = 0
        Me.txtPhoneAreaCode.Name = "txtPhoneAreaCode"
        Me.txtPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtPhoneAreaCode.TabIndex = 18
        '
        'txtPhoneNumber
        '
        Me.txtPhoneNumber.AcceptsReturn = True
        Me.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhoneNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhoneNumber.Location = New System.Drawing.Point(197, 200)
        Me.txtPhoneNumber.MaxLength = 0
        Me.txtPhoneNumber.Name = "txtPhoneNumber"
        Me.txtPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhoneNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtPhoneNumber.TabIndex = 19
        '
        'txtFaxAreaCode
        '
        Me.txtFaxAreaCode.AcceptsReturn = True
        Me.txtFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxAreaCode.Location = New System.Drawing.Point(112, 224)
        Me.txtFaxAreaCode.MaxLength = 0
        Me.txtFaxAreaCode.Name = "txtFaxAreaCode"
        Me.txtFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtFaxAreaCode.TabIndex = 21
        '
        'txtFaxNumber
        '
        Me.txtFaxNumber.AcceptsReturn = True
        Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxNumber.Location = New System.Drawing.Point(197, 224)
        Me.txtFaxNumber.MaxLength = 0
        Me.txtFaxNumber.Name = "txtFaxNumber"
        Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtFaxNumber.TabIndex = 22
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(400, 384)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_1.TabIndex = 23
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'lblExtension
        '
        Me.lblExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExtension.Location = New System.Drawing.Point(320, 184)
        Me.lblExtension.Name = "lblExtension"
        Me.lblExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExtension.Size = New System.Drawing.Size(73, 17)
        Me.lblExtension.TabIndex = 75
        Me.lblExtension.Text = "Extension"
        '
        'lblContactName
        '
        Me.lblContactName.BackColor = System.Drawing.SystemColors.Control
        Me.lblContactName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContactName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContactName.Location = New System.Drawing.Point(8, 17)
        Me.lblContactName.Name = "lblContactName"
        Me.lblContactName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContactName.Size = New System.Drawing.Size(97, 17)
        Me.lblContactName.TabIndex = 74
        Me.lblContactName.Text = "Contact &Name:"
        '
        'lblAddress1
        '
        Me.lblAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress1.Location = New System.Drawing.Point(8, 47)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress1.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress1.TabIndex = 73
        Me.lblAddress1.Text = "Line &1:"
        '
        'lblAddress2
        '
        Me.lblAddress2.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress2.Location = New System.Drawing.Point(8, 70)
        Me.lblAddress2.Name = "lblAddress2"
        Me.lblAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress2.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress2.TabIndex = 72
        Me.lblAddress2.Text = "Line &2:"
        '
        'lblAddresss3
        '
        Me.lblAddresss3.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddresss3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddresss3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddresss3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddresss3.Location = New System.Drawing.Point(8, 89)
        Me.lblAddresss3.Name = "lblAddresss3"
        Me.lblAddresss3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddresss3.Size = New System.Drawing.Size(97, 17)
        Me.lblAddresss3.TabIndex = 71
        Me.lblAddresss3.Text = "&Town:"
        '
        'lblAddress4
        '
        Me.lblAddress4.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddress4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddress4.Location = New System.Drawing.Point(8, 115)
        Me.lblAddress4.Name = "lblAddress4"
        Me.lblAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddress4.Size = New System.Drawing.Size(97, 17)
        Me.lblAddress4.TabIndex = 70
        Me.lblAddress4.Text = "&Region:"
        '
        'lblPostalCode
        '
        Me.lblPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPostalCode.Location = New System.Drawing.Point(8, 134)
        Me.lblPostalCode.Name = "lblPostalCode"
        Me.lblPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPostalCode.Size = New System.Drawing.Size(97, 17)
        Me.lblPostalCode.TabIndex = 69
        Me.lblPostalCode.Text = "&Postal Code:"
        '
        'lblAddressCountry
        '
        Me.lblAddressCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddressCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddressCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddressCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddressCountry.Location = New System.Drawing.Point(8, 162)
        Me.lblAddressCountry.Name = "lblAddressCountry"
        Me.lblAddressCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddressCountry.Size = New System.Drawing.Size(97, 17)
        Me.lblAddressCountry.TabIndex = 68
        Me.lblAddressCountry.Text = "&Country:"
        '
        'lblTelephone
        '
        Me.lblTelephone.BackColor = System.Drawing.SystemColors.Control
        Me.lblTelephone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTelephone.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTelephone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTelephone.Location = New System.Drawing.Point(8, 201)
        Me.lblTelephone.Name = "lblTelephone"
        Me.lblTelephone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTelephone.Size = New System.Drawing.Size(89, 17)
        Me.lblTelephone.TabIndex = 67
        Me.lblTelephone.Text = "T&elephone No:"
        '
        'lblFax
        '
        Me.lblFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFax.Location = New System.Drawing.Point(8, 225)
        Me.lblFax.Name = "lblFax"
        Me.lblFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFax.Size = New System.Drawing.Size(97, 17)
        Me.lblFax.TabIndex = 66
        Me.lblFax.Text = "&Fax No:"
        '
        'lblAreaCode
        '
        Me.lblAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAreaCode.Location = New System.Drawing.Point(112, 184)
        Me.lblAreaCode.Name = "lblAreaCode"
        Me.lblAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAreaCode.Size = New System.Drawing.Size(73, 17)
        Me.lblAreaCode.TabIndex = 65
        Me.lblAreaCode.Text = "Area Code"
        '
        'lblNumber
        '
        Me.lblNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumber.Location = New System.Drawing.Point(197, 184)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumber.Size = New System.Drawing.Size(73, 17)
        Me.lblNumber.TabIndex = 64
        Me.lblNumber.Text = "Number"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._pnlMain_2)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(697, 464)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "&3 - Payment"
        '
        '_pnlMain_2
        '
        Me._pnlMain_2.Controls.Add(Me.chkElectronicPayment)
        Me._pnlMain_2.Controls.Add(Me.cboPaymentTypeID)
        Me._pnlMain_2.Controls.Add(Me._cmdNext_2)
        Me._pnlMain_2.Controls.Add(Me._cmdPrevious_1)
        Me._pnlMain_2.Controls.Add(Me.cboBordereauReportID)
        Me._pnlMain_2.Controls.Add(Me.cboProofListReportID)
        Me._pnlMain_2.Controls.Add(Me.fraSettlementTerms)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentExpiryDate)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentReference2)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentReference1)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentBranchCode)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentAccountCode)
        Me._pnlMain_2.Controls.Add(Me.txtPaymentName)
        Me._pnlMain_2.Controls.Add(Me.lblElectronicPayment)
        Me._pnlMain_2.Controls.Add(Me.lblBordereauReportID)
        Me._pnlMain_2.Controls.Add(Me.lblProofListReportID)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentAccountCode)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentBranchCode)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentExpiryDate)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentReference2)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentReference1)
        Me._pnlMain_2.Controls.Add(Me.lblPaymenttypeID)
        Me._pnlMain_2.Controls.Add(Me.lblPaymentName)
        Me._pnlMain_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_2.Location = New System.Drawing.Point(62, 10)
        Me._pnlMain_2.Name = "_pnlMain_2"
        Me._pnlMain_2.Size = New System.Drawing.Size(439, 411)
        Me._pnlMain_2.TabIndex = 52
        '
        'chkElectronicPayment
        '
        Me.chkElectronicPayment.BackColor = System.Drawing.SystemColors.Control
        Me.chkElectronicPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkElectronicPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkElectronicPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkElectronicPayment.Location = New System.Drawing.Point(162, 244)
        Me.chkElectronicPayment.Name = "chkElectronicPayment"
        Me.chkElectronicPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkElectronicPayment.Size = New System.Drawing.Size(15, 13)
        Me.chkElectronicPayment.TabIndex = 116
        Me.chkElectronicPayment.UseVisualStyleBackColor = False
        '
        'cboPaymentTypeID
        '
        Me.cboPaymentTypeID.DefaultItemId = 0
        Me.cboPaymentTypeID.FirstItem = ""
        Me.cboPaymentTypeID.ItemId = 0
        Me.cboPaymentTypeID.ListIndex = -1
        Me.cboPaymentTypeID.Location = New System.Drawing.Point(140, 44)
        Me.cboPaymentTypeID.Name = "cboPaymentTypeID"
        Me.cboPaymentTypeID.PMLookupProductFamily = 1
        Me.cboPaymentTypeID.SingleItemId = 0
        Me.cboPaymentTypeID.Size = New System.Drawing.Size(169, 21)
        Me.cboPaymentTypeID.Sorted = True
        Me.cboPaymentTypeID.TabIndex = 114
        Me.cboPaymentTypeID.TableName = "MediaType"
        Me.cboPaymentTypeID.ToolTipText = ""
        Me.cboPaymentTypeID.WhereClause = "is_payment=1"
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(400, 384)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_2.TabIndex = 36
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(368, 384)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_1.TabIndex = 35
        Me._cmdPrevious_1.Text = "<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        'cboBordereauReportID
        '
        Me.cboBordereauReportID.BackColor = System.Drawing.SystemColors.Window
        Me.cboBordereauReportID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBordereauReportID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBordereauReportID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBordereauReportID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBordereauReportID.Location = New System.Drawing.Point(140, 216)
        Me.cboBordereauReportID.Name = "cboBordereauReportID"
        Me.cboBordereauReportID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBordereauReportID.Size = New System.Drawing.Size(289, 21)
        Me.cboBordereauReportID.Sorted = True
        Me.cboBordereauReportID.TabIndex = 31
        '
        'cboProofListReportID
        '
        Me.cboProofListReportID.BackColor = System.Drawing.SystemColors.Window
        Me.cboProofListReportID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProofListReportID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProofListReportID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProofListReportID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProofListReportID.Location = New System.Drawing.Point(140, 192)
        Me.cboProofListReportID.Name = "cboProofListReportID"
        Me.cboProofListReportID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProofListReportID.Size = New System.Drawing.Size(289, 21)
        Me.cboProofListReportID.Sorted = True
        Me.cboProofListReportID.TabIndex = 30
        '
        'fraSettlementTerms
        '
        Me.fraSettlementTerms.BackColor = System.Drawing.SystemColors.Control
        Me.fraSettlementTerms.Controls.Add(Me.txtSettlementPeriod)
        Me.fraSettlementTerms.Controls.Add(Me.txtDiscountPercentage)
        Me.fraSettlementTerms.Controls.Add(Me.txtCreditLimit)
        Me.fraSettlementTerms.Controls.Add(Me.lblDays)
        Me.fraSettlementTerms.Controls.Add(Me.lblSettlementPeriod)
        Me.fraSettlementTerms.Controls.Add(Me.lblDiscountPercentage)
        Me.fraSettlementTerms.Controls.Add(Me.lblCreditLimit)
        Me.fraSettlementTerms.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSettlementTerms.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSettlementTerms.Location = New System.Drawing.Point(8, 268)
        Me.fraSettlementTerms.Name = "fraSettlementTerms"
        Me.fraSettlementTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSettlementTerms.Size = New System.Drawing.Size(265, 109)
        Me.fraSettlementTerms.TabIndex = 94
        Me.fraSettlementTerms.TabStop = False
        Me.fraSettlementTerms.Text = "Settlement Terms"
        '
        'txtSettlementPeriod
        '
        Me.txtSettlementPeriod.AcceptsReturn = True
        Me.txtSettlementPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.txtSettlementPeriod.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSettlementPeriod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSettlementPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSettlementPeriod.Location = New System.Drawing.Point(144, 80)
        Me.txtSettlementPeriod.MaxLength = 0
        Me.txtSettlementPeriod.Name = "txtSettlementPeriod"
        Me.txtSettlementPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSettlementPeriod.Size = New System.Drawing.Size(33, 21)
        Me.txtSettlementPeriod.TabIndex = 34
        '
        'txtDiscountPercentage
        '
        Me.txtDiscountPercentage.AcceptsReturn = True
        Me.txtDiscountPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtDiscountPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDiscountPercentage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscountPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDiscountPercentage.Location = New System.Drawing.Point(144, 56)
        Me.txtDiscountPercentage.MaxLength = 0
        Me.txtDiscountPercentage.Name = "txtDiscountPercentage"
        Me.txtDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDiscountPercentage.Size = New System.Drawing.Size(41, 21)
        Me.txtDiscountPercentage.TabIndex = 33
        '
        'txtCreditLimit
        '
        Me.txtCreditLimit.AcceptsReturn = True
        Me.txtCreditLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtCreditLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCreditLimit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreditLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCreditLimit.Location = New System.Drawing.Point(144, 32)
        Me.txtCreditLimit.MaxLength = 0
        Me.txtCreditLimit.Name = "txtCreditLimit"
        Me.txtCreditLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCreditLimit.Size = New System.Drawing.Size(89, 21)
        Me.txtCreditLimit.TabIndex = 32
        '
        'lblDays
        '
        Me.lblDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDays.Location = New System.Drawing.Point(192, 82)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDays.Size = New System.Drawing.Size(57, 17)
        Me.lblDays.TabIndex = 98
        Me.lblDays.Text = "days"
        '
        'lblSettlementPeriod
        '
        Me.lblSettlementPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblSettlementPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSettlementPeriod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSettlementPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSettlementPeriod.Location = New System.Drawing.Point(8, 82)
        Me.lblSettlementPeriod.Name = "lblSettlementPeriod"
        Me.lblSettlementPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSettlementPeriod.Size = New System.Drawing.Size(129, 17)
        Me.lblSettlementPeriod.TabIndex = 97
        Me.lblSettlementPeriod.Text = "&Settlement Period:"
        '
        'lblDiscountPercentage
        '
        Me.lblDiscountPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblDiscountPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDiscountPercentage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscountPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblDiscountPercentage.Location = New System.Drawing.Point(8, 58)
        Me.lblDiscountPercentage.Name = "lblDiscountPercentage"
        Me.lblDiscountPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDiscountPercentage.Size = New System.Drawing.Size(131, 17)
        Me.lblDiscountPercentage.TabIndex = 96
        Me.lblDiscountPercentage.Text = "&Discount %age:"
        '
        'lblCreditLimit
        '
        Me.lblCreditLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblCreditLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCreditLimit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreditLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCreditLimit.Location = New System.Drawing.Point(8, 34)
        Me.lblCreditLimit.Name = "lblCreditLimit"
        Me.lblCreditLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreditLimit.Size = New System.Drawing.Size(105, 17)
        Me.lblCreditLimit.TabIndex = 95
        Me.lblCreditLimit.Text = "Credit &Limit:"
        '
        'txtPaymentExpiryDate
        '
        Me.txtPaymentExpiryDate.AcceptsReturn = True
        Me.txtPaymentExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentExpiryDate.Location = New System.Drawing.Point(140, 120)
        Me.txtPaymentExpiryDate.MaxLength = 0
        Me.txtPaymentExpiryDate.Name = "txtPaymentExpiryDate"
        Me.txtPaymentExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentExpiryDate.Size = New System.Drawing.Size(105, 21)
        Me.txtPaymentExpiryDate.TabIndex = 27
        '
        'txtPaymentReference2
        '
        Me.txtPaymentReference2.AcceptsReturn = True
        Me.txtPaymentReference2.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentReference2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentReference2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentReference2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentReference2.Location = New System.Drawing.Point(140, 168)
        Me.txtPaymentReference2.MaxLength = 0
        Me.txtPaymentReference2.Name = "txtPaymentReference2"
        Me.txtPaymentReference2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentReference2.Size = New System.Drawing.Size(153, 21)
        Me.txtPaymentReference2.TabIndex = 29
        '
        'txtPaymentReference1
        '
        Me.txtPaymentReference1.AcceptsReturn = True
        Me.txtPaymentReference1.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentReference1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentReference1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentReference1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentReference1.Location = New System.Drawing.Point(140, 144)
        Me.txtPaymentReference1.MaxLength = 0
        Me.txtPaymentReference1.Name = "txtPaymentReference1"
        Me.txtPaymentReference1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentReference1.Size = New System.Drawing.Size(153, 21)
        Me.txtPaymentReference1.TabIndex = 28
        '
        'txtPaymentBranchCode
        '
        Me.txtPaymentBranchCode.AcceptsReturn = True
        Me.txtPaymentBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentBranchCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentBranchCode.Location = New System.Drawing.Point(140, 96)
        Me.txtPaymentBranchCode.MaxLength = 0
        Me.txtPaymentBranchCode.Name = "txtPaymentBranchCode"
        Me.txtPaymentBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentBranchCode.Size = New System.Drawing.Size(153, 21)
        Me.txtPaymentBranchCode.TabIndex = 26
        '
        'txtPaymentAccountCode
        '
        Me.txtPaymentAccountCode.AcceptsReturn = True
        Me.txtPaymentAccountCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentAccountCode.Location = New System.Drawing.Point(140, 72)
        Me.txtPaymentAccountCode.MaxLength = 0
        Me.txtPaymentAccountCode.Name = "txtPaymentAccountCode"
        Me.txtPaymentAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentAccountCode.Size = New System.Drawing.Size(153, 21)
        Me.txtPaymentAccountCode.TabIndex = 25
        '
        'txtPaymentName
        '
        Me.txtPaymentName.AcceptsReturn = True
        Me.txtPaymentName.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaymentName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaymentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaymentName.Location = New System.Drawing.Point(140, 16)
        Me.txtPaymentName.MaxLength = 0
        Me.txtPaymentName.Name = "txtPaymentName"
        Me.txtPaymentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaymentName.Size = New System.Drawing.Size(289, 21)
        Me.txtPaymentName.TabIndex = 24
        '
        'lblElectronicPayment
        '
        Me.lblElectronicPayment.BackColor = System.Drawing.SystemColors.Control
        Me.lblElectronicPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblElectronicPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElectronicPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblElectronicPayment.Location = New System.Drawing.Point(8, 244)
        Me.lblElectronicPayment.Name = "lblElectronicPayment"
        Me.lblElectronicPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblElectronicPayment.Size = New System.Drawing.Size(157, 17)
        Me.lblElectronicPayment.TabIndex = 115
        Me.lblElectronicPayment.Text = "Allow electronic payment"
        '
        'lblBordereauReportID
        '
        Me.lblBordereauReportID.BackColor = System.Drawing.SystemColors.Control
        Me.lblBordereauReportID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBordereauReportID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBordereauReportID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblBordereauReportID.Location = New System.Drawing.Point(8, 219)
        Me.lblBordereauReportID.Name = "lblBordereauReportID"
        Me.lblBordereauReportID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBordereauReportID.Size = New System.Drawing.Size(123, 17)
        Me.lblBordereauReportID.TabIndex = 113
        Me.lblBordereauReportID.Text = "{Bordereau Report}"
        '
        'lblProofListReportID
        '
        Me.lblProofListReportID.BackColor = System.Drawing.SystemColors.Control
        Me.lblProofListReportID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProofListReportID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProofListReportID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblProofListReportID.Location = New System.Drawing.Point(8, 195)
        Me.lblProofListReportID.Name = "lblProofListReportID"
        Me.lblProofListReportID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProofListReportID.Size = New System.Drawing.Size(127, 17)
        Me.lblProofListReportID.TabIndex = 112
        Me.lblProofListReportID.Text = "{Proof List Report}"
        '
        'lblPaymentAccountCode
        '
        Me.lblPaymentAccountCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentAccountCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentAccountCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentAccountCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentAccountCode.Location = New System.Drawing.Point(8, 74)
        Me.lblPaymentAccountCode.Name = "lblPaymentAccountCode"
        Me.lblPaymentAccountCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentAccountCode.Size = New System.Drawing.Size(113, 17)
        Me.lblPaymentAccountCode.TabIndex = 93
        Me.lblPaymentAccountCode.Text = "&Account Code:"
        '
        'lblPaymentBranchCode
        '
        Me.lblPaymentBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentBranchCode.Location = New System.Drawing.Point(8, 98)
        Me.lblPaymentBranchCode.Name = "lblPaymentBranchCode"
        Me.lblPaymentBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentBranchCode.Size = New System.Drawing.Size(121, 17)
        Me.lblPaymentBranchCode.TabIndex = 92
        Me.lblPaymentBranchCode.Text = "&Branch Code:"
        '
        'lblPaymentExpiryDate
        '
        Me.lblPaymentExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentExpiryDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentExpiryDate.Location = New System.Drawing.Point(8, 122)
        Me.lblPaymentExpiryDate.Name = "lblPaymentExpiryDate"
        Me.lblPaymentExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentExpiryDate.Size = New System.Drawing.Size(97, 17)
        Me.lblPaymentExpiryDate.TabIndex = 91
        Me.lblPaymentExpiryDate.Text = "&Expiry Date:"
        '
        'lblPaymentReference2
        '
        Me.lblPaymentReference2.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentReference2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentReference2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentReference2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentReference2.Location = New System.Drawing.Point(8, 170)
        Me.lblPaymentReference2.Name = "lblPaymentReference2"
        Me.lblPaymentReference2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentReference2.Size = New System.Drawing.Size(89, 17)
        Me.lblPaymentReference2.TabIndex = 90
        Me.lblPaymentReference2.Text = "Reference &2:"
        '
        'lblPaymentReference1
        '
        Me.lblPaymentReference1.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentReference1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentReference1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentReference1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymentReference1.Location = New System.Drawing.Point(8, 146)
        Me.lblPaymentReference1.Name = "lblPaymentReference1"
        Me.lblPaymentReference1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentReference1.Size = New System.Drawing.Size(97, 17)
        Me.lblPaymentReference1.TabIndex = 89
        Me.lblPaymentReference1.Text = "Reference &1:"
        '
        'lblPaymenttypeID
        '
        Me.lblPaymenttypeID.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymenttypeID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymenttypeID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymenttypeID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblPaymenttypeID.Location = New System.Drawing.Point(8, 47)
        Me.lblPaymenttypeID.Name = "lblPaymenttypeID"
        Me.lblPaymenttypeID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymenttypeID.Size = New System.Drawing.Size(115, 17)
        Me.lblPaymenttypeID.TabIndex = 88
        Me.lblPaymenttypeID.Text = "&Payment Type:"
        '
        'lblPaymentName
        '
        Me.lblPaymentName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentName.Location = New System.Drawing.Point(8, 17)
        Me.lblPaymentName.Name = "lblPaymentName"
        Me.lblPaymentName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentName.Size = New System.Drawing.Size(110, 17)
        Me.lblPaymentName.TabIndex = 87
        Me.lblPaymentName.Text = "Payment &Name:"
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me._pnlMain_3)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(697, 482)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "&4 - Bank Address"
        '
        '_pnlMain_3
        '
        Me._pnlMain_3.Controls.Add(Me._cmdPrevious_2)
        Me._pnlMain_3.Controls.Add(Me.txtBankPhoneAreaCode)
        Me._pnlMain_3.Controls.Add(Me.txtBankFaxNumber)
        Me._pnlMain_3.Controls.Add(Me.txtBankFaxAreaCode)
        Me._pnlMain_3.Controls.Add(Me.txtBankPhoneNumber)
        Me._pnlMain_3.Controls.Add(Me.cboBankCountry)
        Me._pnlMain_3.Controls.Add(Me.txtBankPostalCode)
        Me._pnlMain_3.Controls.Add(Me.txtBankAddress4)
        Me._pnlMain_3.Controls.Add(Me.txtBankAddress2)
        Me._pnlMain_3.Controls.Add(Me.txtBankAddress1)
        Me._pnlMain_3.Controls.Add(Me.txtBankAddress3)
        Me._pnlMain_3.Controls.Add(Me.txtBankName)
        Me._pnlMain_3.Controls.Add(Me.txtBankPhoneExtension)
        Me._pnlMain_3.Controls.Add(Me._cmdNext_3)
        Me._pnlMain_3.Controls.Add(Me.lblBankAreaCode)
        Me._pnlMain_3.Controls.Add(Me.lblBankNumber)
        Me._pnlMain_3.Controls.Add(Me.lblBankFax)
        Me._pnlMain_3.Controls.Add(Me.lblBankPhone)
        Me._pnlMain_3.Controls.Add(Me.lblBankCountry)
        Me._pnlMain_3.Controls.Add(Me.lblBankPostalCode)
        Me._pnlMain_3.Controls.Add(Me.lblBankAddress4)
        Me._pnlMain_3.Controls.Add(Me.lblBankAddress3)
        Me._pnlMain_3.Controls.Add(Me.lblBankAddress2)
        Me._pnlMain_3.Controls.Add(Me.lblBankAddress1)
        Me._pnlMain_3.Controls.Add(Me.lblBankName)
        Me._pnlMain_3.Controls.Add(Me.lblBankExtension)
        Me._pnlMain_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_3.Location = New System.Drawing.Point(62, 10)
        Me._pnlMain_3.Name = "_pnlMain_3"
        Me._pnlMain_3.Size = New System.Drawing.Size(445, 411)
        Me._pnlMain_3.TabIndex = 53
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(368, 384)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_2.TabIndex = 110
        Me._cmdPrevious_2.Text = "<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'txtBankPhoneAreaCode
        '
        Me.txtBankPhoneAreaCode.AcceptsReturn = True
        Me.txtBankPhoneAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankPhoneAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankPhoneAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankPhoneAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankPhoneAreaCode.Location = New System.Drawing.Point(112, 200)
        Me.txtBankPhoneAreaCode.MaxLength = 0
        Me.txtBankPhoneAreaCode.Name = "txtBankPhoneAreaCode"
        Me.txtBankPhoneAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankPhoneAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtBankPhoneAreaCode.TabIndex = 107
        '
        'txtBankFaxNumber
        '
        Me.txtBankFaxNumber.AcceptsReturn = True
        Me.txtBankFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankFaxNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankFaxNumber.Location = New System.Drawing.Point(197, 224)
        Me.txtBankFaxNumber.MaxLength = 0
        Me.txtBankFaxNumber.Name = "txtBankFaxNumber"
        Me.txtBankFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankFaxNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtBankFaxNumber.TabIndex = 47
        '
        'txtBankFaxAreaCode
        '
        Me.txtBankFaxAreaCode.AcceptsReturn = True
        Me.txtBankFaxAreaCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankFaxAreaCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankFaxAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankFaxAreaCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankFaxAreaCode.Location = New System.Drawing.Point(112, 224)
        Me.txtBankFaxAreaCode.MaxLength = 0
        Me.txtBankFaxAreaCode.Name = "txtBankFaxAreaCode"
        Me.txtBankFaxAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankFaxAreaCode.Size = New System.Drawing.Size(81, 21)
        Me.txtBankFaxAreaCode.TabIndex = 46
        '
        'txtBankPhoneNumber
        '
        Me.txtBankPhoneNumber.AcceptsReturn = True
        Me.txtBankPhoneNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankPhoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankPhoneNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankPhoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankPhoneNumber.Location = New System.Drawing.Point(197, 200)
        Me.txtBankPhoneNumber.MaxLength = 0
        Me.txtBankPhoneNumber.Name = "txtBankPhoneNumber"
        Me.txtBankPhoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankPhoneNumber.Size = New System.Drawing.Size(121, 21)
        Me.txtBankPhoneNumber.TabIndex = 44
        '
        'cboBankCountry
        '
        Me.cboBankCountry.BackColor = System.Drawing.SystemColors.Window
        Me.cboBankCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBankCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBankCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBankCountry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBankCountry.Location = New System.Drawing.Point(112, 148)
        Me.cboBankCountry.Name = "cboBankCountry"
        Me.cboBankCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBankCountry.Size = New System.Drawing.Size(289, 21)
        Me.cboBankCountry.TabIndex = 43
        '
        'txtBankPostalCode
        '
        Me.txtBankPostalCode.AcceptsReturn = True
        Me.txtBankPostalCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankPostalCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankPostalCode.Location = New System.Drawing.Point(112, 128)
        Me.txtBankPostalCode.MaxLength = 0
        Me.txtBankPostalCode.Name = "txtBankPostalCode"
        Me.txtBankPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankPostalCode.Size = New System.Drawing.Size(153, 21)
        Me.txtBankPostalCode.TabIndex = 42
        '
        'txtBankAddress4
        '
        Me.txtBankAddress4.AcceptsReturn = True
        Me.txtBankAddress4.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAddress4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAddress4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAddress4.Location = New System.Drawing.Point(112, 108)
        Me.txtBankAddress4.MaxLength = 0
        Me.txtBankAddress4.Name = "txtBankAddress4"
        Me.txtBankAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAddress4.Size = New System.Drawing.Size(289, 21)
        Me.txtBankAddress4.TabIndex = 41
        '
        'txtBankAddress2
        '
        Me.txtBankAddress2.AcceptsReturn = True
        Me.txtBankAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAddress2.Location = New System.Drawing.Point(112, 68)
        Me.txtBankAddress2.MaxLength = 0
        Me.txtBankAddress2.Name = "txtBankAddress2"
        Me.txtBankAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAddress2.Size = New System.Drawing.Size(289, 21)
        Me.txtBankAddress2.TabIndex = 39
        '
        'txtBankAddress1
        '
        Me.txtBankAddress1.AcceptsReturn = True
        Me.txtBankAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAddress1.Location = New System.Drawing.Point(112, 48)
        Me.txtBankAddress1.MaxLength = 0
        Me.txtBankAddress1.Name = "txtBankAddress1"
        Me.txtBankAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAddress1.Size = New System.Drawing.Size(289, 21)
        Me.txtBankAddress1.TabIndex = 38
        '
        'txtBankAddress3
        '
        Me.txtBankAddress3.AcceptsReturn = True
        Me.txtBankAddress3.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankAddress3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankAddress3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankAddress3.Location = New System.Drawing.Point(112, 88)
        Me.txtBankAddress3.MaxLength = 0
        Me.txtBankAddress3.Name = "txtBankAddress3"
        Me.txtBankAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankAddress3.Size = New System.Drawing.Size(289, 21)
        Me.txtBankAddress3.TabIndex = 40
        '
        'txtBankName
        '
        Me.txtBankName.AcceptsReturn = True
        Me.txtBankName.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankName.Location = New System.Drawing.Point(112, 16)
        Me.txtBankName.MaxLength = 0
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankName.Size = New System.Drawing.Size(289, 21)
        Me.txtBankName.TabIndex = 37
        '
        'txtBankPhoneExtension
        '
        Me.txtBankPhoneExtension.AcceptsReturn = True
        Me.txtBankPhoneExtension.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankPhoneExtension.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankPhoneExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankPhoneExtension.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankPhoneExtension.Location = New System.Drawing.Point(320, 200)
        Me.txtBankPhoneExtension.MaxLength = 0
        Me.txtBankPhoneExtension.Name = "txtBankPhoneExtension"
        Me.txtBankPhoneExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankPhoneExtension.Size = New System.Drawing.Size(81, 21)
        Me.txtBankPhoneExtension.TabIndex = 45
        '
        '_cmdNext_3
        '
        Me._cmdNext_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_3.Location = New System.Drawing.Point(400, 384)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_3.TabIndex = 48
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'lblBankAreaCode
        '
        Me.lblBankAreaCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAreaCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAreaCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAreaCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAreaCode.Location = New System.Drawing.Point(112, 184)
        Me.lblBankAreaCode.Name = "lblBankAreaCode"
        Me.lblBankAreaCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAreaCode.Size = New System.Drawing.Size(73, 17)
        Me.lblBankAreaCode.TabIndex = 108
        Me.lblBankAreaCode.Text = "Area Code"
        '
        'lblBankNumber
        '
        Me.lblBankNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankNumber.Location = New System.Drawing.Point(197, 184)
        Me.lblBankNumber.Name = "lblBankNumber"
        Me.lblBankNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankNumber.Size = New System.Drawing.Size(73, 17)
        Me.lblBankNumber.TabIndex = 86
        Me.lblBankNumber.Text = "Number"
        '
        'lblBankFax
        '
        Me.lblBankFax.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankFax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankFax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankFax.Location = New System.Drawing.Point(8, 225)
        Me.lblBankFax.Name = "lblBankFax"
        Me.lblBankFax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankFax.Size = New System.Drawing.Size(97, 17)
        Me.lblBankFax.TabIndex = 85
        Me.lblBankFax.Text = "&Fax No:"
        '
        'lblBankPhone
        '
        Me.lblBankPhone.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankPhone.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankPhone.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankPhone.Location = New System.Drawing.Point(8, 201)
        Me.lblBankPhone.Name = "lblBankPhone"
        Me.lblBankPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankPhone.Size = New System.Drawing.Size(89, 17)
        Me.lblBankPhone.TabIndex = 84
        Me.lblBankPhone.Text = "T&elephone No:"
        '
        'lblBankCountry
        '
        Me.lblBankCountry.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankCountry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankCountry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankCountry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankCountry.Location = New System.Drawing.Point(8, 150)
        Me.lblBankCountry.Name = "lblBankCountry"
        Me.lblBankCountry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankCountry.Size = New System.Drawing.Size(97, 17)
        Me.lblBankCountry.TabIndex = 83
        Me.lblBankCountry.Text = "&Country:"
        '
        'lblBankPostalCode
        '
        Me.lblBankPostalCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankPostalCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankPostalCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankPostalCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankPostalCode.Location = New System.Drawing.Point(8, 129)
        Me.lblBankPostalCode.Name = "lblBankPostalCode"
        Me.lblBankPostalCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankPostalCode.Size = New System.Drawing.Size(97, 17)
        Me.lblBankPostalCode.TabIndex = 82
        Me.lblBankPostalCode.Text = "&Postal Code:"
        '
        'lblBankAddress4
        '
        Me.lblBankAddress4.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAddress4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAddress4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAddress4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAddress4.Location = New System.Drawing.Point(8, 109)
        Me.lblBankAddress4.Name = "lblBankAddress4"
        Me.lblBankAddress4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAddress4.Size = New System.Drawing.Size(97, 17)
        Me.lblBankAddress4.TabIndex = 81
        Me.lblBankAddress4.Text = "&Region:"
        '
        'lblBankAddress3
        '
        Me.lblBankAddress3.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAddress3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAddress3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAddress3.Location = New System.Drawing.Point(8, 89)
        Me.lblBankAddress3.Name = "lblBankAddress3"
        Me.lblBankAddress3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAddress3.Size = New System.Drawing.Size(97, 17)
        Me.lblBankAddress3.TabIndex = 80
        Me.lblBankAddress3.Text = "&Town:"
        '
        'lblBankAddress2
        '
        Me.lblBankAddress2.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAddress2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAddress2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAddress2.Location = New System.Drawing.Point(8, 69)
        Me.lblBankAddress2.Name = "lblBankAddress2"
        Me.lblBankAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAddress2.Size = New System.Drawing.Size(97, 17)
        Me.lblBankAddress2.TabIndex = 79
        Me.lblBankAddress2.Text = "Line &2:"
        '
        'lblBankAddress1
        '
        Me.lblBankAddress1.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAddress1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAddress1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAddress1.Location = New System.Drawing.Point(8, 49)
        Me.lblBankAddress1.Name = "lblBankAddress1"
        Me.lblBankAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAddress1.Size = New System.Drawing.Size(97, 17)
        Me.lblBankAddress1.TabIndex = 78
        Me.lblBankAddress1.Text = "Line &1:"
        '
        'lblBankName
        '
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(8, 17)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(97, 17)
        Me.lblBankName.TabIndex = 77
        Me.lblBankName.Text = "Bank &Name:"
        '
        'lblBankExtension
        '
        Me.lblBankExtension.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankExtension.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankExtension.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankExtension.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankExtension.Location = New System.Drawing.Point(320, 184)
        Me.lblBankExtension.Name = "lblBankExtension"
        Me.lblBankExtension.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankExtension.Size = New System.Drawing.Size(73, 17)
        Me.lblBankExtension.TabIndex = 76
        Me.lblBankExtension.Text = "Extension"
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me._pnlMain_4)
        Me._tabMainTab_TabPage4.Controls.Add(Me._cmdNext_4)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(697, 482)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "&5 - Comments"
        '
        '_pnlMain_4
        '
        Me._pnlMain_4.Controls.Add(Me._cmdPrevious_3)
        Me._pnlMain_4.Controls.Add(Me.txtComments)
        Me._pnlMain_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_4.Location = New System.Drawing.Point(50, 10)
        Me._pnlMain_4.Name = "_pnlMain_4"
        Me._pnlMain_4.Size = New System.Drawing.Size(447, 411)
        Me._pnlMain_4.TabIndex = 54
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(380, 384)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_3.TabIndex = 111
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(8, 16)
        Me.txtComments.MaxLength = 160
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.Size = New System.Drawing.Size(427, 184)
        Me.txtComments.TabIndex = 49
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(462, 394)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_4.TabIndex = 123
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.uctPartyBankControl2)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdPrevious_4)
        Me._tabMainTab_TabPage5.Controls.Add(Me._cmdNext_5)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(697, 482)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "&6 - Bank"
        '
        'uctPartyBankControl2
        '
        Me.uctPartyBankControl2.AccountId = Nothing
        Me.uctPartyBankControl2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyBankControl2.IsExternalCreditCardProcessing = False
        Me.uctPartyBankControl2.Location = New System.Drawing.Point(2, 38)
        Me.uctPartyBankControl2.Name = "uctPartyBankControl2"
        Me.uctPartyBankControl2.PartyBankDetails = Nothing
        Me.uctPartyBankControl2.PartyBankHistory = Nothing
        Me.uctPartyBankControl2.PartyCnt = Nothing
        Me.uctPartyBankControl2.Size = New System.Drawing.Size(688, 370)
        Me.uctPartyBankControl2.TabIndex = 128
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(430, 414)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_4.TabIndex = 124
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
        Me._cmdNext_5.Location = New System.Drawing.Point(462, 414)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(29, 22)
        Me._cmdNext_5.TabIndex = 126
        Me._cmdNext_5.Text = ">>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraCashDeposit)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(697, 464)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "&7 - Linked Account's"
        '
        'fraCashDeposit
        '
        Me.fraCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.fraCashDeposit.Controls.Add(Me.uctLinkedAccountsControl)
        Me.fraCashDeposit.Controls.Add(Me._cmdPrevious_5)
        Me.fraCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCashDeposit.Location = New System.Drawing.Point(10, 12)
        Me.fraCashDeposit.Name = "fraCashDeposit"
        Me.fraCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCashDeposit.Size = New System.Drawing.Size(631, 451)
        Me.fraCashDeposit.TabIndex = 127
        Me.fraCashDeposit.TabStop = False
        Me.fraCashDeposit.Text = "Cash Deposit"
        '
        'uctLinkedAccountsControl
        '
        Me.uctLinkedAccountsControl.AccountId = 0
        Me.uctLinkedAccountsControl.CDLinkedAccountDetails = Nothing
        Me.uctLinkedAccountsControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctLinkedAccountsControl.ItemCount = 0
        Me.uctLinkedAccountsControl.Location = New System.Drawing.Point(4, 14)
        Me.uctLinkedAccountsControl.Name = "uctLinkedAccountsControl"
        Me.uctLinkedAccountsControl.Size = New System.Drawing.Size(617, 427)
        Me.uctLinkedAccountsControl.TabIndex = 130
        Me.uctLinkedAccountsControl.Task = 0
        Me.uctLinkedAccountsControl.ViewMode = False
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(464, 384)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(29, 22)
        Me._cmdPrevious_5.TabIndex = 129
        Me._cmdPrevious_5.Text = "<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(714, 555)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Account"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._pnlMain_0.ResumeLayout(False)
        Me._pnlMain_0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._pnlMain_1.ResumeLayout(False)
        Me._pnlMain_1.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me._pnlMain_2.ResumeLayout(False)
        Me._pnlMain_2.PerformLayout()
        Me.fraSettlementTerms.ResumeLayout(False)
        Me.fraSettlementTerms.PerformLayout()
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._pnlMain_3.ResumeLayout(False)
        Me._pnlMain_3.PerformLayout()
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me._pnlMain_4.ResumeLayout(False)
        Me._pnlMain_4.PerformLayout()
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraCashDeposit.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializepnlMain()
		Me.pnlMain(0) = _pnlMain_0
		Me.pnlMain(1) = _pnlMain_1
		Me.pnlMain(3) = _pnlMain_3
		Me.pnlMain(4) = _pnlMain_4
		Me.pnlMain(2) = _pnlMain_2
	End Sub
	Sub InitializecmdPrevious()
		Me.cmdPrevious(5) = _cmdPrevious_5
		Me.cmdPrevious(4) = _cmdPrevious_4
		Me.cmdPrevious(0) = _cmdPrevious_0
		Me.cmdPrevious(2) = _cmdPrevious_2
		Me.cmdPrevious(3) = _cmdPrevious_3
		Me.cmdPrevious(1) = _cmdPrevious_1
	End Sub
	Sub InitializecmdNext()
		Me.cmdNext(5) = _cmdNext_5
		Me.cmdNext(4) = _cmdNext_4
		Me.cmdNext(0) = _cmdNext_0
		Me.cmdNext(1) = _cmdNext_1
		Me.cmdNext(3) = _cmdNext_3
		Me.cmdNext(2) = _cmdNext_2
    End Sub
    Public WithEvents uctPartyBankControl2 As uctPartyBank.uctPartyBankControl
#End Region 
End Class