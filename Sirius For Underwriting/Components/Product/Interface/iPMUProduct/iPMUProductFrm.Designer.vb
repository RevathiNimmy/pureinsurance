<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializetxtEmailComm()
		InitializelblEmailComm()
		InitializelblClaimSystemOptions()
		InitializecmdEmailCommSelect()
		InitializecmdEmailCommDeSelect()
		InitializechkPaymentClaimWorkflow()
		InitializechkOpenClaimWorkflow()
		InitializechkMaintainClaimWorkflow()
		InitializecboUDT()
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public cboUDT(4) As System.Windows.Forms.ComboBox
    Public chkMaintainClaimWorkflow(26) As System.Windows.Forms.CheckBox
    Public chkOpenClaimWorkflow(25) As System.Windows.Forms.CheckBox
    Public chkPaymentClaimWorkflow(16) As System.Windows.Forms.CheckBox
    Public cmdEmailCommDeSelect(20) As System.Windows.Forms.Button
    Public cmdEmailCommSelect(20) As System.Windows.Forms.Button
    Public lblClaimSystemOptions(14) As System.Windows.Forms.Label
    Public lblEmailComm(23) As System.Windows.Forms.Label
    Public txtEmailComm(20) As System.Windows.Forms.TextBox
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    ' <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.cboAgentReviewAttachment = New System.Windows.Forms.ComboBox()
        Me.cboAgentInviteAttachment = New System.Windows.Forms.ComboBox()
        Me.cboAgentUpdateAttachment = New System.Windows.Forms.ComboBox()
        Me.cboCoinsurerInclusion = New System.Windows.Forms.ComboBox()
        Me.cboClaimUserGroup = New System.Windows.Forms.ComboBox()
        Me.cboClaimTaskGroup = New System.Windows.Forms.ComboBox()
        Me._cboUDT_4 = New System.Windows.Forms.ComboBox()
        Me._cboUDT_2 = New System.Windows.Forms.ComboBox()
        Me._cboUDT_3 = New System.Windows.Forms.ComboBox()
        Me._cboUDT_0 = New System.Windows.Forms.ComboBox()
        Me._cboUDT_1 = New System.Windows.Forms.ComboBox()
        Me.cboallocation = New System.Windows.Forms.ComboBox()
        Me.cbodtAllowed = New System.Windows.Forms.ComboBox()
        Me.cboOSMTAUserGroup = New System.Windows.Forms.ComboBox()
        Me.cboOSMTATaskGroup = New System.Windows.Forms.ComboBox()
        Me.cboMonthInCycleSA = New System.Windows.Forms.ComboBox()
        Me.cboMonthInCycleLA = New System.Windows.Forms.ComboBox()
        Me.cboProvClaimAutoNumberingID = New System.Windows.Forms.ComboBox()
        Me.cboFullClaimAutoNumberingID = New System.Windows.Forms.ComboBox()
        Me.cboQuoteAutoNumberingID = New System.Windows.Forms.ComboBox()
        Me.cboPolicyAutoNumberingID = New System.Windows.Forms.ComboBox()
        Me.cboCNAutoNumberingID = New System.Windows.Forms.ComboBox()
        Me.cboPosValues = New System.Windows.Forms.ComboBox()
        Me.cboReminderUserGroup = New System.Windows.Forms.ComboBox()
        Me.cboReminderTaskGroup = New System.Windows.Forms.ComboBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.ComboBox4 = New System.Windows.Forms.ComboBox()
        Me.ComboBox5 = New System.Windows.Forms.ComboBox()
        Me.ComboBox6 = New System.Windows.Forms.ComboBox()
        Me.ComboBox7 = New System.Windows.Forms.ComboBox()
        Me.ComboBox8 = New System.Windows.Forms.ComboBox()
        Me.ComboBox9 = New System.Windows.Forms.ComboBox()
        Me.ComboBox10 = New System.Windows.Forms.ComboBox()
        Me.ComboBox11 = New System.Windows.Forms.ComboBox()
        Me.ComboBox12 = New System.Windows.Forms.ComboBox()
        Me.ComboBox13 = New System.Windows.Forms.ComboBox()
        Me.ComboBox14 = New System.Windows.Forms.ComboBox()
        Me.ComboBox15 = New System.Windows.Forms.ComboBox()
        Me.ComboBox16 = New System.Windows.Forms.ComboBox()
        Me.ComboBox17 = New System.Windows.Forms.ComboBox()
        Me.ComboBox18 = New System.Windows.Forms.ComboBox()
        Me.ComboBox19 = New System.Windows.Forms.ComboBox()
        Me.ComboBox20 = New System.Windows.Forms.ComboBox()
        Me.ComboBox21 = New System.Windows.Forms.ComboBox()
        Me.ComboBox22 = New System.Windows.Forms.ComboBox()
        Me.ComboBox23 = New System.Windows.Forms.ComboBox()
        Me.chkDefaultCovertoDatetolastday = New System.Windows.Forms.CheckBox()
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.fraBranches = New System.Windows.Forms.GroupBox()
        Me.uctPickListBranches = New uctPickList.PickList()
        Me._tabMainTab_TabPage10 = New System.Windows.Forms.TabPage()
        Me.GroupBox40 = New System.Windows.Forms.GroupBox()
        Me.GroupBox42 = New System.Windows.Forms.GroupBox()
        Me.PickList6 = New uctPickList.PickList()
        Me.PickList4 = New uctPickList.PickList()
        Me._tabMainTab_TabPage9 = New System.Windows.Forms.TabPage()
        Me.uctSIRSelectClauses = New uctSCControl.uctSIRSelectClauses()
        Me._tabMainTab_TabPage8 = New System.Windows.Forms.TabPage()
        Me.DocumentLinkClaim = New uctPMUDocumentLink.uctDocumentLink()
        Me.DocumentLinkClaimPayment = New uctPMUDocumentLink.uctDocumentLink()
        Me._tabMainTab_TabPage7 = New System.Windows.Forms.TabPage()
        Me.uctDocumentLink1 = New uctPMUDocumentLink.uctDocumentLink()
        Me._tabMainTab_TabPage6 = New System.Windows.Forms.TabPage()
        Me.fraAgentEmailComm = New System.Windows.Forms.GroupBox()
        Me.chkAgentRenInvite = New System.Windows.Forms.CheckBox()
        Me.chkAgentRenUpdate = New System.Windows.Forms.CheckBox()
        Me._cmdEmailCommDeSelect_20 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_20 = New System.Windows.Forms.Button()
        Me._txtEmailComm_20 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_19 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_19 = New System.Windows.Forms.Button()
        Me._txtEmailComm_19 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_18 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_18 = New System.Windows.Forms.Button()
        Me._txtEmailComm_18 = New System.Windows.Forms.TextBox()
        Me.chkAgentRenSelection = New System.Windows.Forms.CheckBox()
        Me._lblEmailComm_23 = New System.Windows.Forms.Label()
        Me._lblEmailComm_20 = New System.Windows.Forms.Label()
        Me._lblEmailComm_22 = New System.Windows.Forms.Label()
        Me._lblEmailComm_19 = New System.Windows.Forms.Label()
        Me._lblEmailComm_21 = New System.Windows.Forms.Label()
        Me._lblEmailComm_18 = New System.Windows.Forms.Label()
        Me.fraTrueMonthlyPolFreqOfComm = New System.Windows.Forms.GroupBox()
        Me.optRenewalProcessRun = New System.Windows.Forms.RadioButton()
        Me.optAnniversaryDate = New System.Windows.Forms.RadioButton()
        Me.fraDirectConsumerComm = New System.Windows.Forms.GroupBox()
        Me.fraRenewalUpdate = New System.Windows.Forms.GroupBox()
        Me.chkEnabledRenUpdate = New System.Windows.Forms.CheckBox()
        Me._txtEmailComm_12 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_12 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_12 = New System.Windows.Forms.Button()
        Me._txtEmailComm_13 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_13 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_13 = New System.Windows.Forms.Button()
        Me._txtEmailComm_14 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_14 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_14 = New System.Windows.Forms.Button()
        Me._txtEmailComm_15 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_15 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_15 = New System.Windows.Forms.Button()
        Me._txtEmailComm_16 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_16 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_16 = New System.Windows.Forms.Button()
        Me._txtEmailComm_17 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_17 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_17 = New System.Windows.Forms.Button()
        Me._lblEmailComm_12 = New System.Windows.Forms.Label()
        Me._lblEmailComm_15 = New System.Windows.Forms.Label()
        Me._lblEmailComm_13 = New System.Windows.Forms.Label()
        Me._lblEmailComm_16 = New System.Windows.Forms.Label()
        Me._lblEmailComm_14 = New System.Windows.Forms.Label()
        Me._lblEmailComm_17 = New System.Windows.Forms.Label()
        Me.fraRenewalInvite = New System.Windows.Forms.GroupBox()
        Me.chkEnabledRenInvite = New System.Windows.Forms.CheckBox()
        Me._txtEmailComm_6 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_6 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_6 = New System.Windows.Forms.Button()
        Me._txtEmailComm_7 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_7 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_7 = New System.Windows.Forms.Button()
        Me._txtEmailComm_8 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_8 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_8 = New System.Windows.Forms.Button()
        Me._txtEmailComm_9 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_9 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_9 = New System.Windows.Forms.Button()
        Me._txtEmailComm_10 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_10 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_10 = New System.Windows.Forms.Button()
        Me._txtEmailComm_11 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommSelect_11 = New System.Windows.Forms.Button()
        Me._cmdEmailCommDeSelect_11 = New System.Windows.Forms.Button()
        Me._lblEmailComm_6 = New System.Windows.Forms.Label()
        Me._lblEmailComm_9 = New System.Windows.Forms.Label()
        Me._lblEmailComm_7 = New System.Windows.Forms.Label()
        Me._lblEmailComm_10 = New System.Windows.Forms.Label()
        Me._lblEmailComm_8 = New System.Windows.Forms.Label()
        Me._lblEmailComm_11 = New System.Windows.Forms.Label()
        Me.fraRenewalSelection = New System.Windows.Forms.GroupBox()
        Me._cmdEmailCommDeSelect_5 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_5 = New System.Windows.Forms.Button()
        Me._txtEmailComm_5 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_4 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_4 = New System.Windows.Forms.Button()
        Me._txtEmailComm_4 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_3 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_3 = New System.Windows.Forms.Button()
        Me._txtEmailComm_3 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_2 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_2 = New System.Windows.Forms.Button()
        Me._txtEmailComm_2 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_1 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_1 = New System.Windows.Forms.Button()
        Me._txtEmailComm_1 = New System.Windows.Forms.TextBox()
        Me._cmdEmailCommDeSelect_0 = New System.Windows.Forms.Button()
        Me._cmdEmailCommSelect_0 = New System.Windows.Forms.Button()
        Me._txtEmailComm_0 = New System.Windows.Forms.TextBox()
        Me.chkEnabledRenSelection = New System.Windows.Forms.CheckBox()
        Me._lblEmailComm_5 = New System.Windows.Forms.Label()
        Me._lblEmailComm_2 = New System.Windows.Forms.Label()
        Me._lblEmailComm_4 = New System.Windows.Forms.Label()
        Me._lblEmailComm_1 = New System.Windows.Forms.Label()
        Me._lblEmailComm_3 = New System.Windows.Forms.Label()
        Me._lblEmailComm_0 = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage5 = New System.Windows.Forms.TabPage()
        Me.fraClaimEventDesc = New System.Windows.Forms.GroupBox()
        Me.uctPickListClaimEvent = New uctPickList.PickList()
        Me.fraMTAEventDesc = New System.Windows.Forms.GroupBox()
        Me.uctPickListMTAEvent = New uctPickList.PickList()
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fraRateMultipleRisks = New System.Windows.Forms.GroupBox()
        Me.chkDisplayRerateForCancellationsAndReinstatments = New System.Windows.Forms.CheckBox()
        Me.chkDisplayRerateForMTA = New System.Windows.Forms.CheckBox()
        Me.chkDisplayRerateForQuoteAndNB = New System.Windows.Forms.CheckBox()
        Me.chkDisplayRerateForRenewal = New System.Windows.Forms.CheckBox()
        Me.fraMediaTypeStatusValidation = New System.Windows.Forms.GroupBox()
        Me.chkValidateMediaTypeStatusAtClaimPayment = New System.Windows.Forms.CheckBox()
        Me.chkValidateMediaTypeStatusAtPolicyRefund = New System.Windows.Forms.CheckBox()
        Me.fraCoverNote = New System.Windows.Forms.GroupBox()
        Me.cmdCNDocTemplate = New System.Windows.Forms.Button()
        Me.txtCNDocTemplate = New System.Windows.Forms.TextBox()
        Me.txtCNMaxNo = New System.Windows.Forms.TextBox()
        Me.txtCNDefaultPeriod = New System.Windows.Forms.TextBox()
        Me.lblCNDocTemplate = New System.Windows.Forms.Label()
        Me.lblCNMaxNo = New System.Windows.Forms.Label()
        Me.lblCNDefaultPeriod = New System.Windows.Forms.Label()
        Me.FraOutofSeqMTA = New System.Windows.Forms.GroupBox()
        Me.lblTaskGroup = New System.Windows.Forms.Label()
        Me.lblUserGroup = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FraVoidTransaction = New System.Windows.Forms.GroupBox()
        Me.chkVoidPolicyVersion = New System.Windows.Forms.CheckBox()
        Me.fraClaimSystemOptions = New System.Windows.Forms.GroupBox()
        Me.Frame10 = New System.Windows.Forms.GroupBox()
        Me.txtDeleteQuoteAfter = New System.Windows.Forms.TextBox()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.chkQuoteVersioning = New System.Windows.Forms.CheckBox()
        Me.Frame9 = New System.Windows.Forms.GroupBox()
        Me.chkBackdatedMTA = New System.Windows.Forms.CheckBox()
        Me.chkBackdatedCan = New System.Windows.Forms.CheckBox()
        Me.Frame8 = New System.Windows.Forms.GroupBox()
        Me.chkDuplicateClaim = New System.Windows.Forms.CheckBox()
        Me.chkAdvanceTaxScript = New System.Windows.Forms.CheckBox()
        Me.chkPaymentRefCheck = New System.Windows.Forms.CheckBox()
        Me.Frame7 = New System.Windows.Forms.GroupBox()
        Me._lblClaimSystemOptions_12 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_13 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_10 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_11 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_14 = New System.Windows.Forms.Label()
        Me.fraExtClaimHandler = New System.Windows.Forms.GroupBox()
        Me.txtAckTaskAllowedTime = New System.Windows.Forms.TextBox()
        Me.txtPreReportAllowedTime = New System.Windows.Forms.TextBox()
        Me._lblClaimSystemOptions_8 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_9 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_6 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_7 = New System.Windows.Forms.Label()
        Me.fraUWClaims = New System.Windows.Forms.GroupBox()
        Me.chkPaymentCannotExceedReserve = New System.Windows.Forms.CheckBox()
        Me.chkAllowNegativeReserve = New System.Windows.Forms.CheckBox()
        Me.chkValidPolicyAtLossDate = New System.Windows.Forms.CheckBox()
        Me.chkClaimPaymentGross = New System.Windows.Forms.CheckBox()
        Me.txtLargeLossAdviceValue = New System.Windows.Forms.TextBox()
        Me._lblClaimSystemOptions_0 = New System.Windows.Forms.Label()
        Me._lblClaimSystemOptions_4 = New System.Windows.Forms.Label()
        Me.fraNexus = New System.Windows.Forms.GroupBox()
        Me.txtOnlineCommencedOn = New System.Windows.Forms.TextBox()
        Me.chkTradeRnlOnline = New System.Windows.Forms.CheckBox()
        Me.chkTradeMtaOnline = New System.Windows.Forms.CheckBox()
        Me.chkTradeNbOnline = New System.Windows.Forms.CheckBox()
        Me.lblOnlineCommencedOn = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.chkProduceDebitNote = New System.Windows.Forms.CheckBox()
        Me.chkProduceCertificate = New System.Windows.Forms.CheckBox()
        Me.chkProduceSchedule = New System.Windows.Forms.CheckBox()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.chkCashDeposit = New System.Windows.Forms.CheckBox()
        Me.chkBankGuarantee = New System.Windows.Forms.CheckBox()
        Me.chkInvoice = New System.Windows.Forms.CheckBox()
        Me.chkInstalments = New System.Windows.Forms.CheckBox()
        Me.chkPayNow = New System.Windows.Forms.CheckBox()
        Me.lblCashDeposit = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkRecoveryInstalmentsEnabled = New System.Windows.Forms.CheckBox()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.frmRenewals = New System.Windows.Forms.GroupBox()
        Me.chkEditAnnivDate = New System.Windows.Forms.CheckBox()
        Me.lblAnnivDateEditableMonthlyPolicy = New System.Windows.Forms.Label()
        Me.chkAutoRenBDMonthlyPol = New System.Windows.Forms.CheckBox()
        Me.lblAutoRenBDMonthlyPolicy = New System.Windows.Forms.Label()
        Me.chkUnifiedRenewalDateIsReadOnly = New System.Windows.Forms.CheckBox()
        Me.lblUnifiedRenewalDateIsReadOnly = New System.Windows.Forms.Label()
        Me.chkTMPAutoRenFAC = New System.Windows.Forms.CheckBox()
        Me.lblMonthlyAutoRenWithFac = New System.Windows.Forms.Label()
        Me.txtUnifiedRenewalDay = New System.Windows.Forms.TextBox()
        Me.txtAnniversaryRenewalWeeks = New System.Windows.Forms.TextBox()
        Me.lblUnifiedRenewalDay = New System.Windows.Forms.Label()
        Me.lblAnniversaryRenewalWeeks = New System.Windows.Forms.Label()
        Me.frmLeadAgentCommission = New System.Windows.Forms.GroupBox()
        Me.actSuspenseAcc = New UserControls.AccountLookup()
        Me.chkAllowConsolidateCommissionLA = New System.Windows.Forms.CheckBox()
        Me.lblLeadAgentCommSuspenseLA = New System.Windows.Forms.Label()
        Me.lblMonthInCycleLA = New System.Windows.Forms.Label()
        Me.frmSubAgentCommission = New System.Windows.Forms.GroupBox()
        Me.actSuspenseAcc1 = New UserControls.AccountLookup()
        Me.chkAllowConsolidateCommissionSA = New System.Windows.Forms.CheckBox()
        Me.lblSubAgentCommSuspense = New System.Windows.Forms.Label()
        Me.lblMonthInCycleSA = New System.Windows.Forms.Label()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me.OptInstalments = New System.Windows.Forms.RadioButton()
        Me.OptInvoice = New System.Windows.Forms.RadioButton()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.fraOpenClaim = New System.Windows.Forms.GroupBox()
        Me._chkOpenClaimWorkflow_25 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_24 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_23 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_22 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_21 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_20 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_19 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_18 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_17 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_16 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_15 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_14 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_13 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_12 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_11 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_10 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_9 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_8 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_7 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_6 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_5 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_4 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_3 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_2 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_1 = New System.Windows.Forms.CheckBox()
        Me._chkOpenClaimWorkflow_0 = New System.Windows.Forms.CheckBox()
        Me.Frame5 = New System.Windows.Forms.GroupBox()
        Me._chkPaymentClaimWorkflow_16 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_15 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_14 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_13 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_12 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_11 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_10 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_9 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_8 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_7 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_6 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_5 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_4 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_3 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_2 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_1 = New System.Windows.Forms.CheckBox()
        Me._chkPaymentClaimWorkflow_0 = New System.Windows.Forms.CheckBox()
        Me.Frame6 = New System.Windows.Forms.GroupBox()
        Me._chkMaintainClaimWorkflow_26 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_25 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_24 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_23 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_22 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_21 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_20 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_19 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_18 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_17 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_16 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_15 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_14 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_13 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_12 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_11 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_10 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_8 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_7 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_6 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_5 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_4 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_3 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_2 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_1 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_0 = New System.Windows.Forms.CheckBox()
        Me._chkMaintainClaimWorkflow_9 = New System.Windows.Forms.CheckBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.grpClaimScripts = New System.Windows.Forms.GroupBox()
        Me.chkIsPaymentsReadonly = New System.Windows.Forms.CheckBox()
        Me.chkIsReservesReadonly = New System.Windows.Forms.CheckBox()
        Me.chkIsRecoveriesReadonly = New System.Windows.Forms.CheckBox()
        Me.fraCausations = New System.Windows.Forms.GroupBox()
        Me.uctPickListCausations = New uctPickList.PickList()
        Me.fraLimits = New System.Windows.Forms.GroupBox()
        Me.txtAllowedClaims = New System.Windows.Forms.TextBox()
        Me.txtClaimYear = New System.Windows.Forms.TextBox()
        Me.txtSingleClaimValue = New System.Windows.Forms.TextBox()
        Me.txtTotalClaimsValue = New System.Windows.Forms.TextBox()
        Me.chkCheckAgent = New System.Windows.Forms.CheckBox()
        Me.chkMediaTypeMandatory = New System.Windows.Forms.CheckBox()
        Me.chkLossCurrencyChange = New System.Windows.Forms.CheckBox()
        Me.cboBankAccount = New UserControls.BankAccount()
        Me.lblBankAccount = New System.Windows.Forms.Label()
        Me.lblAllowedClaims = New System.Windows.Forms.Label()
        Me.lblClaimYear = New System.Windows.Forms.Label()
        Me.lblSingleClaimValue = New System.Windows.Forms.Label()
        Me.lblTotalClaimsValue = New System.Windows.Forms.Label()
        Me.fraNumbering = New System.Windows.Forms.GroupBox()
        Me.lblFullClaimAutoNumberingID = New System.Windows.Forms.Label()
        Me.lblProvClaimAutoNumberingID = New System.Windows.Forms.Label()
        Me.fraSupression = New System.Windows.Forms.GroupBox()
        Me.chkRecoveries = New System.Windows.Forms.CheckBox()
        Me.chkPayments = New System.Windows.Forms.CheckBox()
        Me.chkReserve = New System.Windows.Forms.CheckBox()
        Me.Frame4 = New System.Windows.Forms.GroupBox()
        Me.txtAuthorisationThreshold = New System.Windows.Forms.TextBox()
        Me.lblAuthorisationThreshold = New System.Windows.Forms.Label()
        Me.chkRecommender = New System.Windows.Forms.CheckBox()
        Me.txtMaxNoofUnauthorisedClaimPayments = New System.Windows.Forms.TextBox()
        Me.txtMaxUnauthorisedClaimsValue = New System.Windows.Forms.TextBox()
        Me.chkMultiplePayments = New System.Windows.Forms.CheckBox()
        Me.chkRunAuthorisationScriptsClaimPayments = New System.Windows.Forms.CheckBox()
        Me.lblMaxNoofUnauthorisedClaimPayments = New System.Windows.Forms.Label()
        Me.lblMaxUnauthorisedClaimsValue = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraWrittenStatus = New System.Windows.Forms.GroupBox()
        Me.lblReminderTaskGroup = New System.Windows.Forms.Label()
        Me.txtTaskManagerDays = New System.Windows.Forms.TextBox()
        Me.lblReminderUserGroup = New System.Windows.Forms.Label()
        Me.lblTaskManagerDays = New System.Windows.Forms.Label()
        Me.fraOtherOptions = New System.Windows.Forms.GroupBox()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.cboApplyMandatoryRisk = New PMLookupControl.cboPMLookup()
        Me.lblApplyMandatoryRisk = New System.Windows.Forms.Label()
        Me.cmdRIModel = New System.Windows.Forms.Button()
        Me.chkAllowStandardWordingEdit = New System.Windows.Forms.CheckBox()
        Me.lblPositiveValues = New System.Windows.Forms.Label()
        Me.lblAllowStandardWordingEdit = New System.Windows.Forms.Label()
        Me.fraPremium = New System.Windows.Forms.GroupBox()
        Me.chkRoundOffToZero = New System.Windows.Forms.CheckBox()
        Me.chkRoundPremium = New System.Windows.Forms.CheckBox()
        Me.chkCurrencyChange = New System.Windows.Forms.CheckBox()
        Me.chkTaxSuppressed = New System.Windows.Forms.CheckBox()
        Me.chkAccumulation = New System.Windows.Forms.CheckBox()
        Me.cboRoundingSection = New PMLookupControl.cboPMLookup()
        Me.lblRoundingSection = New System.Windows.Forms.Label()
        Me.lblRoundPremium = New System.Windows.Forms.Label()
        Me.lblCurrencyChange = New System.Windows.Forms.Label()
        Me.lblAccumulation = New System.Windows.Forms.Label()
        Me.lblTaxSuppressed = New System.Windows.Forms.Label()
        Me.fraProRata = New System.Windows.Forms.GroupBox()
        Me.chkMTCRatingRules = New System.Windows.Forms.CheckBox()
        Me.cmdSPR = New System.Windows.Forms.Button()
        Me.chkMTAProRata = New System.Windows.Forms.CheckBox()
        Me.chkNBProRata = New System.Windows.Forms.CheckBox()
        Me.chkShortPeriodRated = New System.Windows.Forms.CheckBox()
        Me.lblMTCRatingRules = New System.Windows.Forms.Label()
        Me.lblMTAProRata = New System.Windows.Forms.Label()
        Me.lblNBProRata = New System.Windows.Forms.Label()
        Me.lblShortPeriodRated = New System.Windows.Forms.Label()
        Me.fraRenewals = New System.Windows.Forms.GroupBox()
        Me.chkDisableCoverStartDateonREN = New System.Windows.Forms.CheckBox()
        Me.chkDoNotDeleteRenewalQuoteOnMTA = New System.Windows.Forms.CheckBox()
        Me.chkDeleteRenewalQuoteReRunOnMTA = New System.Windows.Forms.CheckBox()
        Me.chkBindRenewalWOInvitation = New System.Windows.Forms.CheckBox()
        Me.chkUsePriorSchemeAtRenewal = New System.Windows.Forms.CheckBox()
        Me.chkUseNBRenPaymentTermsAtSelection = New System.Windows.Forms.CheckBox()
        Me.chkChangePolicyNumberAtRenewalAutomatically = New System.Windows.Forms.CheckBox()
        Me.txtdefaultRenMth = New System.Windows.Forms.TextBox()
        Me.chkRenewable = New System.Windows.Forms.CheckBox()
        Me.chkHideSummaryAtRenewalAcceptance = New System.Windows.Forms.CheckBox()
        Me.chkChangePolicyNumberAtRenewal = New System.Windows.Forms.CheckBox()
        Me.chkMidNightRenewal = New System.Windows.Forms.CheckBox()
        Me.chkAutoRenewal = New System.Windows.Forms.CheckBox()
        Me.txtRenewalPeriod = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblAutoRenewal = New System.Windows.Forms.Label()
        Me.lblRenewalPeriod = New System.Windows.Forms.Label()
        Me.rbPolicyInceptionDate = New System.Windows.Forms.RadioButton()
        Me.rbEffectiveDate = New System.Windows.Forms.RadioButton()
        Me.fraPolicyCreation = New System.Windows.Forms.GroupBox()
        Me.chkRetainPolicyNumber = New System.Windows.Forms.CheckBox()
        Me.chkEnablePrePayment = New System.Windows.Forms.CheckBox()
        Me.chkWrittenPolicy = New System.Windows.Forms.CheckBox()
        Me.chkReinsuranceManualPremiumAdjustment = New System.Windows.Forms.CheckBox()
        Me.chkTrueMonthlyPolicy = New System.Windows.Forms.CheckBox()
        Me.txtGracePeriod = New System.Windows.Forms.TextBox()
        Me.chkPolicyNumberAtQuote = New System.Windows.Forms.CheckBox()
        Me.chkPolicyStyleMandatory = New System.Windows.Forms.CheckBox()
        Me.cboPolicyStyle = New PMLookupControl.cboPMLookup()
        Me.lbCNNumbering = New System.Windows.Forms.Label()
        Me.lblGracePeriod = New System.Windows.Forms.Label()
        Me.lblPolicyNumberAtQuote = New System.Windows.Forms.Label()
        Me.lblPolicyAutoNumberingID = New System.Windows.Forms.Label()
        Me.lblQuoteAutoNumberingID = New System.Windows.Forms.Label()
        Me.lblPolicyStyle = New System.Windows.Forms.Label()
        Me.lblPolicyStyleMandatory = New System.Windows.Forms.Label()
        Me.fraDetails = New System.Windows.Forms.GroupBox()
        Me.txtBlockNo = New System.Windows.Forms.TextBox()
        Me.txtRIPointer = New System.Windows.Forms.TextBox()
        Me.txtReportPointer = New System.Windows.Forms.TextBox()
        Me.txtSchemeAgencyRef = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.cboRiskTypeGroup = New PMLookupControl.cboPMLookup()
        Me.lblBlockNo = New System.Windows.Forms.Label()
        Me.lblRIPointer = New System.Windows.Forms.Label()
        Me.lblReportPointer = New System.Windows.Forms.Label()
        Me.lblRiskTypeGroup = New System.Windows.Forms.Label()
        Me.lblSchemeAgencyRef = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox5 = New System.Windows.Forms.CheckBox()
        Me.CheckBox6 = New System.Windows.Forms.CheckBox()
        Me.CboPMLookup1 = New PMLookupControl.cboPMLookup()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CheckBox7 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.CheckBox8 = New System.Windows.Forms.CheckBox()
        Me.CheckBox9 = New System.Windows.Forms.CheckBox()
        Me.CheckBox10 = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.CheckBox11 = New System.Windows.Forms.CheckBox()
        Me.CheckBox12 = New System.Windows.Forms.CheckBox()
        Me.CheckBox13 = New System.Windows.Forms.CheckBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CheckBox14 = New System.Windows.Forms.CheckBox()
        Me.CheckBox15 = New System.Windows.Forms.CheckBox()
        Me.CheckBox16 = New System.Windows.Forms.CheckBox()
        Me.CheckBox17 = New System.Windows.Forms.CheckBox()
        Me.CheckBox18 = New System.Windows.Forms.CheckBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.CheckBox19 = New System.Windows.Forms.CheckBox()
        Me.CheckBox20 = New System.Windows.Forms.CheckBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.CheckBox21 = New System.Windows.Forms.CheckBox()
        Me.CheckBox22 = New System.Windows.Forms.CheckBox()
        Me.CboPMLookup2 = New PMLookupControl.cboPMLookup()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.TextBox10 = New System.Windows.Forms.TextBox()
        Me.CboPMLookup3 = New PMLookupControl.cboPMLookup()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.PickList1 = New uctPickList.PickList()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.TextBox12 = New System.Windows.Forms.TextBox()
        Me.TextBox13 = New System.Windows.Forms.TextBox()
        Me.TextBox14 = New System.Windows.Forms.TextBox()
        Me.CheckBox23 = New System.Windows.Forms.CheckBox()
        Me.CheckBox24 = New System.Windows.Forms.CheckBox()
        Me.CheckBox25 = New System.Windows.Forms.CheckBox()
        Me.BankAccount1 = New UserControls.BankAccount()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.CheckBox26 = New System.Windows.Forms.CheckBox()
        Me.CheckBox27 = New System.Windows.Forms.CheckBox()
        Me.CheckBox28 = New System.Windows.Forms.CheckBox()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.CboPMLookup4 = New PMLookupControl.cboPMLookup()
        Me.CboPMLookup5 = New PMLookupControl.cboPMLookup()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.CheckBox29 = New System.Windows.Forms.CheckBox()
        Me.TextBox15 = New System.Windows.Forms.TextBox()
        Me.TextBox16 = New System.Windows.Forms.TextBox()
        Me.CheckBox30 = New System.Windows.Forms.CheckBox()
        Me.CheckBox31 = New System.Windows.Forms.CheckBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox13 = New System.Windows.Forms.GroupBox()
        Me.CheckBox32 = New System.Windows.Forms.CheckBox()
        Me.CheckBox33 = New System.Windows.Forms.CheckBox()
        Me.CheckBox34 = New System.Windows.Forms.CheckBox()
        Me.CheckBox35 = New System.Windows.Forms.CheckBox()
        Me.CheckBox36 = New System.Windows.Forms.CheckBox()
        Me.CheckBox37 = New System.Windows.Forms.CheckBox()
        Me.CheckBox38 = New System.Windows.Forms.CheckBox()
        Me.CheckBox39 = New System.Windows.Forms.CheckBox()
        Me.CheckBox40 = New System.Windows.Forms.CheckBox()
        Me.CheckBox41 = New System.Windows.Forms.CheckBox()
        Me.CheckBox42 = New System.Windows.Forms.CheckBox()
        Me.CheckBox43 = New System.Windows.Forms.CheckBox()
        Me.CheckBox44 = New System.Windows.Forms.CheckBox()
        Me.CheckBox45 = New System.Windows.Forms.CheckBox()
        Me.CheckBox46 = New System.Windows.Forms.CheckBox()
        Me.CheckBox47 = New System.Windows.Forms.CheckBox()
        Me.CheckBox48 = New System.Windows.Forms.CheckBox()
        Me.CheckBox49 = New System.Windows.Forms.CheckBox()
        Me.CheckBox50 = New System.Windows.Forms.CheckBox()
        Me.CheckBox51 = New System.Windows.Forms.CheckBox()
        Me.CheckBox52 = New System.Windows.Forms.CheckBox()
        Me.CheckBox53 = New System.Windows.Forms.CheckBox()
        Me.CheckBox54 = New System.Windows.Forms.CheckBox()
        Me.CheckBox55 = New System.Windows.Forms.CheckBox()
        Me.CheckBox56 = New System.Windows.Forms.CheckBox()
        Me.CheckBox57 = New System.Windows.Forms.CheckBox()
        Me.GroupBox14 = New System.Windows.Forms.GroupBox()
        Me.CheckBox58 = New System.Windows.Forms.CheckBox()
        Me.CheckBox59 = New System.Windows.Forms.CheckBox()
        Me.CheckBox60 = New System.Windows.Forms.CheckBox()
        Me.CheckBox61 = New System.Windows.Forms.CheckBox()
        Me.CheckBox62 = New System.Windows.Forms.CheckBox()
        Me.CheckBox63 = New System.Windows.Forms.CheckBox()
        Me.CheckBox64 = New System.Windows.Forms.CheckBox()
        Me.CheckBox65 = New System.Windows.Forms.CheckBox()
        Me.CheckBox66 = New System.Windows.Forms.CheckBox()
        Me.CheckBox67 = New System.Windows.Forms.CheckBox()
        Me.CheckBox68 = New System.Windows.Forms.CheckBox()
        Me.CheckBox69 = New System.Windows.Forms.CheckBox()
        Me.CheckBox70 = New System.Windows.Forms.CheckBox()
        Me.CheckBox71 = New System.Windows.Forms.CheckBox()
        Me.CheckBox72 = New System.Windows.Forms.CheckBox()
        Me.CheckBox73 = New System.Windows.Forms.CheckBox()
        Me.CheckBox74 = New System.Windows.Forms.CheckBox()
        Me.GroupBox15 = New System.Windows.Forms.GroupBox()
        Me.CheckBox75 = New System.Windows.Forms.CheckBox()
        Me.CheckBox76 = New System.Windows.Forms.CheckBox()
        Me.CheckBox77 = New System.Windows.Forms.CheckBox()
        Me.CheckBox78 = New System.Windows.Forms.CheckBox()
        Me.CheckBox79 = New System.Windows.Forms.CheckBox()
        Me.CheckBox80 = New System.Windows.Forms.CheckBox()
        Me.CheckBox81 = New System.Windows.Forms.CheckBox()
        Me.CheckBox82 = New System.Windows.Forms.CheckBox()
        Me.CheckBox83 = New System.Windows.Forms.CheckBox()
        Me.CheckBox84 = New System.Windows.Forms.CheckBox()
        Me.CheckBox85 = New System.Windows.Forms.CheckBox()
        Me.CheckBox86 = New System.Windows.Forms.CheckBox()
        Me.CheckBox87 = New System.Windows.Forms.CheckBox()
        Me.CheckBox88 = New System.Windows.Forms.CheckBox()
        Me.CheckBox89 = New System.Windows.Forms.CheckBox()
        Me.CheckBox90 = New System.Windows.Forms.CheckBox()
        Me.CheckBox91 = New System.Windows.Forms.CheckBox()
        Me.CheckBox92 = New System.Windows.Forms.CheckBox()
        Me.CheckBox93 = New System.Windows.Forms.CheckBox()
        Me.CheckBox94 = New System.Windows.Forms.CheckBox()
        Me.CheckBox95 = New System.Windows.Forms.CheckBox()
        Me.CheckBox96 = New System.Windows.Forms.CheckBox()
        Me.CheckBox97 = New System.Windows.Forms.CheckBox()
        Me.CheckBox98 = New System.Windows.Forms.CheckBox()
        Me.CheckBox99 = New System.Windows.Forms.CheckBox()
        Me.CheckBox100 = New System.Windows.Forms.CheckBox()
        Me.CheckBox101 = New System.Windows.Forms.CheckBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBox16 = New System.Windows.Forms.GroupBox()
        Me.TextBox17 = New System.Windows.Forms.TextBox()
        Me.TextBox18 = New System.Windows.Forms.TextBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.GroupBox17 = New System.Windows.Forms.GroupBox()
        Me.AccountLookup1 = New UserControls.AccountLookup()
        Me.CheckBox102 = New System.Windows.Forms.CheckBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.GroupBox18 = New System.Windows.Forms.GroupBox()
        Me.AccountLookup2 = New UserControls.AccountLookup()
        Me.CheckBox103 = New System.Windows.Forms.CheckBox()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.GroupBox19 = New System.Windows.Forms.GroupBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.GroupBox20 = New System.Windows.Forms.GroupBox()
        Me.CheckBox104 = New System.Windows.Forms.CheckBox()
        Me.CheckBox105 = New System.Windows.Forms.CheckBox()
        Me.GroupBox21 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TextBox19 = New System.Windows.Forms.TextBox()
        Me.TextBox20 = New System.Windows.Forms.TextBox()
        Me.TextBox21 = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.GroupBox22 = New System.Windows.Forms.GroupBox()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.GroupBox23 = New System.Windows.Forms.GroupBox()
        Me.GroupBox24 = New System.Windows.Forms.GroupBox()
        Me.CheckBox106 = New System.Windows.Forms.CheckBox()
        Me.CheckBox107 = New System.Windows.Forms.CheckBox()
        Me.GroupBox25 = New System.Windows.Forms.GroupBox()
        Me.CheckBox108 = New System.Windows.Forms.CheckBox()
        Me.CheckBox109 = New System.Windows.Forms.CheckBox()
        Me.CheckBox110 = New System.Windows.Forms.CheckBox()
        Me.GroupBox26 = New System.Windows.Forms.GroupBox()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.GroupBox27 = New System.Windows.Forms.GroupBox()
        Me.TextBox22 = New System.Windows.Forms.TextBox()
        Me.TextBox23 = New System.Windows.Forms.TextBox()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.GroupBox28 = New System.Windows.Forms.GroupBox()
        Me.CheckBox111 = New System.Windows.Forms.CheckBox()
        Me.CheckBox112 = New System.Windows.Forms.CheckBox()
        Me.CheckBox113 = New System.Windows.Forms.CheckBox()
        Me.CheckBox114 = New System.Windows.Forms.CheckBox()
        Me.TextBox24 = New System.Windows.Forms.TextBox()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.GroupBox29 = New System.Windows.Forms.GroupBox()
        Me.TextBox25 = New System.Windows.Forms.TextBox()
        Me.CheckBox115 = New System.Windows.Forms.CheckBox()
        Me.CheckBox116 = New System.Windows.Forms.CheckBox()
        Me.CheckBox117 = New System.Windows.Forms.CheckBox()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.GroupBox30 = New System.Windows.Forms.GroupBox()
        Me.CheckBox118 = New System.Windows.Forms.CheckBox()
        Me.CheckBox119 = New System.Windows.Forms.CheckBox()
        Me.CheckBox120 = New System.Windows.Forms.CheckBox()
        Me.GroupBox31 = New System.Windows.Forms.GroupBox()
        Me.CheckBox121 = New System.Windows.Forms.CheckBox()
        Me.CheckBox122 = New System.Windows.Forms.CheckBox()
        Me.CheckBox123 = New System.Windows.Forms.CheckBox()
        Me.CheckBox124 = New System.Windows.Forms.CheckBox()
        Me.CheckBox125 = New System.Windows.Forms.CheckBox()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.GroupBox32 = New System.Windows.Forms.GroupBox()
        Me.PickList2 = New uctPickList.PickList()
        Me.GroupBox33 = New System.Windows.Forms.GroupBox()
        Me.PickList3 = New uctPickList.PickList()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.GroupBox34 = New System.Windows.Forms.GroupBox()
        Me.CheckBox126 = New System.Windows.Forms.CheckBox()
        Me.CheckBox127 = New System.Windows.Forms.CheckBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.TextBox26 = New System.Windows.Forms.TextBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.TextBox27 = New System.Windows.Forms.TextBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.TextBox28 = New System.Windows.Forms.TextBox()
        Me.CheckBox128 = New System.Windows.Forms.CheckBox()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.GroupBox35 = New System.Windows.Forms.GroupBox()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.GroupBox36 = New System.Windows.Forms.GroupBox()
        Me.GroupBox37 = New System.Windows.Forms.GroupBox()
        Me.CheckBox129 = New System.Windows.Forms.CheckBox()
        Me.TextBox29 = New System.Windows.Forms.TextBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.TextBox30 = New System.Windows.Forms.TextBox()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.TextBox31 = New System.Windows.Forms.TextBox()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.TextBox32 = New System.Windows.Forms.TextBox()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button17 = New System.Windows.Forms.Button()
        Me.TextBox33 = New System.Windows.Forms.TextBox()
        Me.Button18 = New System.Windows.Forms.Button()
        Me.Button19 = New System.Windows.Forms.Button()
        Me.TextBox34 = New System.Windows.Forms.TextBox()
        Me.Button20 = New System.Windows.Forms.Button()
        Me.Button21 = New System.Windows.Forms.Button()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.GroupBox38 = New System.Windows.Forms.GroupBox()
        Me.CheckBox130 = New System.Windows.Forms.CheckBox()
        Me.TextBox35 = New System.Windows.Forms.TextBox()
        Me.Button22 = New System.Windows.Forms.Button()
        Me.Button23 = New System.Windows.Forms.Button()
        Me.TextBox36 = New System.Windows.Forms.TextBox()
        Me.Button24 = New System.Windows.Forms.Button()
        Me.Button25 = New System.Windows.Forms.Button()
        Me.TextBox37 = New System.Windows.Forms.TextBox()
        Me.Button26 = New System.Windows.Forms.Button()
        Me.Button27 = New System.Windows.Forms.Button()
        Me.TextBox38 = New System.Windows.Forms.TextBox()
        Me.Button28 = New System.Windows.Forms.Button()
        Me.Button29 = New System.Windows.Forms.Button()
        Me.TextBox39 = New System.Windows.Forms.TextBox()
        Me.Button30 = New System.Windows.Forms.Button()
        Me.Button31 = New System.Windows.Forms.Button()
        Me.TextBox40 = New System.Windows.Forms.TextBox()
        Me.Button32 = New System.Windows.Forms.Button()
        Me.Button33 = New System.Windows.Forms.Button()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.GroupBox39 = New System.Windows.Forms.GroupBox()
        Me.Button34 = New System.Windows.Forms.Button()
        Me.Button35 = New System.Windows.Forms.Button()
        Me.TextBox41 = New System.Windows.Forms.TextBox()
        Me.Button36 = New System.Windows.Forms.Button()
        Me.Button37 = New System.Windows.Forms.Button()
        Me.TextBox42 = New System.Windows.Forms.TextBox()
        Me.Button38 = New System.Windows.Forms.Button()
        Me.Button39 = New System.Windows.Forms.Button()
        Me.TextBox43 = New System.Windows.Forms.TextBox()
        Me.Button40 = New System.Windows.Forms.Button()
        Me.Button41 = New System.Windows.Forms.Button()
        Me.TextBox44 = New System.Windows.Forms.TextBox()
        Me.Button42 = New System.Windows.Forms.Button()
        Me.Button43 = New System.Windows.Forms.Button()
        Me.TextBox45 = New System.Windows.Forms.TextBox()
        Me.Button44 = New System.Windows.Forms.Button()
        Me.Button45 = New System.Windows.Forms.Button()
        Me.TextBox46 = New System.Windows.Forms.TextBox()
        Me.CheckBox131 = New System.Windows.Forms.CheckBox()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.UctDocumentLink2 = New uctPMUDocumentLink.uctDocumentLink()
        Me.TabPage9 = New System.Windows.Forms.TabPage()
        Me.UctDocumentLink3 = New uctPMUDocumentLink.uctDocumentLink()
        Me.UctDocumentLink4 = New uctPMUDocumentLink.uctDocumentLink()
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.UctSIRSelectClauses1 = New uctSCControl.uctSIRSelectClauses()
        Me.GroupBox41 = New System.Windows.Forms.GroupBox()
        Me.PickList5 = New uctPickList.PickList()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit
        Me._tabMainTab_TabPage10.SuspendLayout
        Me.GroupBox40.SuspendLayout
        Me.GroupBox42.SuspendLayout
        Me._tabMainTab_TabPage9.SuspendLayout
        Me._tabMainTab_TabPage8.SuspendLayout
        Me._tabMainTab_TabPage7.SuspendLayout
        Me._tabMainTab_TabPage6.SuspendLayout
        Me.fraAgentEmailComm.SuspendLayout
        Me.fraTrueMonthlyPolFreqOfComm.SuspendLayout
        Me.fraDirectConsumerComm.SuspendLayout
        Me.fraRenewalUpdate.SuspendLayout
        Me.fraRenewalInvite.SuspendLayout
        Me.fraRenewalSelection.SuspendLayout
        Me._tabMainTab_TabPage5.SuspendLayout
        Me.fraClaimEventDesc.SuspendLayout
        Me.fraMTAEventDesc.SuspendLayout
        Me._tabMainTab_TabPage4.SuspendLayout
        Me.fraRateMultipleRisks.SuspendLayout
        Me.fraMediaTypeStatusValidation.SuspendLayout
        Me.fraCoverNote.SuspendLayout
        Me.FraOutofSeqMTA.SuspendLayout
        Me.FraVoidTransaction.SuspendLayout
        Me.fraClaimSystemOptions.SuspendLayout
        Me.Frame10.SuspendLayout
        Me.Frame9.SuspendLayout
        Me.Frame8.SuspendLayout
        Me.Frame7.SuspendLayout
        Me.fraExtClaimHandler.SuspendLayout
        Me.fraUWClaims.SuspendLayout
        Me.fraNexus.SuspendLayout
        Me.Frame2.SuspendLayout
        Me.Frame1.SuspendLayout
        Me._tabMainTab_TabPage3.SuspendLayout
        Me.frmRenewals.SuspendLayout
        Me.frmLeadAgentCommission.SuspendLayout
        Me.frmSubAgentCommission.SuspendLayout
        Me.Frame3.SuspendLayout
        Me._tabMainTab_TabPage2.SuspendLayout
        Me.fraOpenClaim.SuspendLayout
        Me.Frame5.SuspendLayout
        Me.Frame6.SuspendLayout
        Me._tabMainTab_TabPage1.SuspendLayout
        Me.grpClaimScripts.SuspendLayout
        Me.fraCausations.SuspendLayout
        Me.fraLimits.SuspendLayout
        Me.fraNumbering.SuspendLayout
        Me.fraSupression.SuspendLayout
        Me.Frame4.SuspendLayout
        Me._tabMainTab_TabPage0.SuspendLayout
        Me.fraWrittenStatus.SuspendLayout
        Me.fraOtherOptions.SuspendLayout
        Me.fraPremium.SuspendLayout
        Me.fraProRata.SuspendLayout
        Me.fraRenewals.SuspendLayout
        Me.fraPolicyCreation.SuspendLayout
        Me.fraDetails.SuspendLayout
        Me.tabMainTab.SuspendLayout
        Me.TabPage1.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.GroupBox2.SuspendLayout
        Me.GroupBox3.SuspendLayout
        Me.GroupBox4.SuspendLayout
        Me.GroupBox5.SuspendLayout
        Me.GroupBox6.SuspendLayout
        Me.TabPage2.SuspendLayout
        Me.GroupBox7.SuspendLayout
        Me.GroupBox8.SuspendLayout
        Me.GroupBox9.SuspendLayout
        Me.GroupBox10.SuspendLayout
        Me.GroupBox11.SuspendLayout
        Me.GroupBox12.SuspendLayout
        Me.TabPage3.SuspendLayout
        Me.GroupBox13.SuspendLayout
        Me.GroupBox14.SuspendLayout
        Me.GroupBox15.SuspendLayout
        Me.TabPage4.SuspendLayout
        Me.GroupBox16.SuspendLayout
        Me.GroupBox17.SuspendLayout
        Me.GroupBox18.SuspendLayout
        Me.GroupBox19.SuspendLayout
        Me.TabPage5.SuspendLayout
        Me.GroupBox20.SuspendLayout
        Me.GroupBox21.SuspendLayout
        Me.GroupBox22.SuspendLayout
        Me.GroupBox23.SuspendLayout
        Me.GroupBox24.SuspendLayout
        Me.GroupBox25.SuspendLayout
        Me.GroupBox26.SuspendLayout
        Me.GroupBox27.SuspendLayout
        Me.GroupBox28.SuspendLayout
        Me.GroupBox29.SuspendLayout
        Me.GroupBox30.SuspendLayout
        Me.GroupBox31.SuspendLayout
        Me.TabPage6.SuspendLayout
        Me.GroupBox32.SuspendLayout
        Me.GroupBox33.SuspendLayout
        Me.TabPage7.SuspendLayout
        Me.GroupBox34.SuspendLayout
        Me.GroupBox35.SuspendLayout
        Me.GroupBox36.SuspendLayout
        Me.GroupBox37.SuspendLayout
        Me.GroupBox38.SuspendLayout
        Me.GroupBox39.SuspendLayout
        Me.TabPage8.SuspendLayout
        Me.TabPage9.SuspendLayout
        Me.TabPage10.SuspendLayout
        Me.GroupBox41.SuspendLayout
        Me.SuspendLayout
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(688, 711)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 44
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(607, 711)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 43
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(767, 711)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 45
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(527, 711)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 42
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        Me.cmdHelp.Visible = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "checked")
        Me.ImageList1.Images.SetKeyName(1, "causation")
        '
        'cboAgentReviewAttachment
        '
        Me.cboAgentReviewAttachment.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentReviewAttachment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentReviewAttachment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentReviewAttachment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboAgentReviewAttachment, New Integer(-1) {})
        Me.cboAgentReviewAttachment.Location = New System.Drawing.Point(513, 17)
        Me.cboAgentReviewAttachment.Name = "cboAgentReviewAttachment"
        Me.cboAgentReviewAttachment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentReviewAttachment.Size = New System.Drawing.Size(180, 21)
        Me.cboAgentReviewAttachment.TabIndex = 287
        Me.cboAgentReviewAttachment.Text = "Combo1"
        '
        'cboAgentInviteAttachment
        '
        Me.cboAgentInviteAttachment.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentInviteAttachment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentInviteAttachment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentInviteAttachment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboAgentInviteAttachment, New Integer(-1) {})
        Me.cboAgentInviteAttachment.Location = New System.Drawing.Point(513, 42)
        Me.cboAgentInviteAttachment.Name = "cboAgentInviteAttachment"
        Me.cboAgentInviteAttachment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentInviteAttachment.Size = New System.Drawing.Size(180, 21)
        Me.cboAgentInviteAttachment.TabIndex = 288
        Me.cboAgentInviteAttachment.Text = "Combo1"
        '
        'cboAgentUpdateAttachment
        '
        Me.cboAgentUpdateAttachment.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentUpdateAttachment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentUpdateAttachment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentUpdateAttachment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboAgentUpdateAttachment, New Integer(-1) {})
        Me.cboAgentUpdateAttachment.Location = New System.Drawing.Point(513, 68)
        Me.cboAgentUpdateAttachment.Name = "cboAgentUpdateAttachment"
        Me.cboAgentUpdateAttachment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentUpdateAttachment.Size = New System.Drawing.Size(180, 21)
        Me.cboAgentUpdateAttachment.TabIndex = 289
        Me.cboAgentUpdateAttachment.Text = "Combo1"
        '
        'cboCoinsurerInclusion
        '
        Me.cboCoinsurerInclusion.BackColor = System.Drawing.SystemColors.Window
        Me.cboCoinsurerInclusion.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCoinsurerInclusion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCoinsurerInclusion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCoinsurerInclusion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCoinsurerInclusion, New Integer(-1) {})
        Me.cboCoinsurerInclusion.Location = New System.Drawing.Point(276, 127)
        Me.cboCoinsurerInclusion.Name = "cboCoinsurerInclusion"
        Me.cboCoinsurerInclusion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCoinsurerInclusion.Size = New System.Drawing.Size(192, 21)
        Me.cboCoinsurerInclusion.TabIndex = 142
        '
        'cboClaimUserGroup
        '
        Me.cboClaimUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboClaimUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClaimUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClaimUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClaimUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboClaimUserGroup, New Integer(-1) {})
        Me.cboClaimUserGroup.Location = New System.Drawing.Point(281, 92)
        Me.cboClaimUserGroup.Name = "cboClaimUserGroup"
        Me.cboClaimUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClaimUserGroup.Size = New System.Drawing.Size(210, 21)
        Me.cboClaimUserGroup.TabIndex = 162
        '
        'cboClaimTaskGroup
        '
        Me.cboClaimTaskGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboClaimTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClaimTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClaimTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClaimTaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboClaimTaskGroup, New Integer(-1) {})
        Me.cboClaimTaskGroup.Location = New System.Drawing.Point(281, 65)
        Me.cboClaimTaskGroup.Name = "cboClaimTaskGroup"
        Me.cboClaimTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClaimTaskGroup.Size = New System.Drawing.Size(210, 21)
        Me.cboClaimTaskGroup.TabIndex = 160
        '
        '_cboUDT_4
        '
        Me._cboUDT_4.BackColor = System.Drawing.SystemColors.Window
        Me._cboUDT_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboUDT_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboUDT_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboUDT_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me._cboUDT_4, New Integer(-1) {})
        Me._cboUDT_4.Location = New System.Drawing.Point(78, 131)
        Me._cboUDT_4.Name = "_cboUDT_4"
        Me._cboUDT_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboUDT_4.Size = New System.Drawing.Size(214, 21)
        Me._cboUDT_4.TabIndex = 153
        '
        '_cboUDT_2
        '
        Me._cboUDT_2.BackColor = System.Drawing.SystemColors.Window
        Me._cboUDT_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboUDT_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboUDT_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboUDT_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me._cboUDT_2, New Integer(-1) {})
        Me._cboUDT_2.Location = New System.Drawing.Point(78, 77)
        Me._cboUDT_2.Name = "_cboUDT_2"
        Me._cboUDT_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboUDT_2.Size = New System.Drawing.Size(214, 21)
        Me._cboUDT_2.TabIndex = 149
        '
        '_cboUDT_3
        '
        Me._cboUDT_3.BackColor = System.Drawing.SystemColors.Window
        Me._cboUDT_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboUDT_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboUDT_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboUDT_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me._cboUDT_3, New Integer(-1) {})
        Me._cboUDT_3.Location = New System.Drawing.Point(78, 104)
        Me._cboUDT_3.Name = "_cboUDT_3"
        Me._cboUDT_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboUDT_3.Size = New System.Drawing.Size(214, 21)
        Me._cboUDT_3.TabIndex = 151
        '
        '_cboUDT_0
        '
        Me._cboUDT_0.BackColor = System.Drawing.SystemColors.Window
        Me._cboUDT_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboUDT_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboUDT_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboUDT_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me._cboUDT_0, New Integer(-1) {})
        Me._cboUDT_0.Location = New System.Drawing.Point(78, 23)
        Me._cboUDT_0.Name = "_cboUDT_0"
        Me._cboUDT_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboUDT_0.Size = New System.Drawing.Size(214, 21)
        Me._cboUDT_0.TabIndex = 145
        '
        '_cboUDT_1
        '
        Me._cboUDT_1.BackColor = System.Drawing.SystemColors.Window
        Me._cboUDT_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboUDT_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboUDT_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboUDT_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me._cboUDT_1, New Integer(-1) {})
        Me._cboUDT_1.Location = New System.Drawing.Point(78, 50)
        Me._cboUDT_1.Name = "_cboUDT_1"
        Me._cboUDT_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboUDT_1.Size = New System.Drawing.Size(214, 21)
        Me._cboUDT_1.TabIndex = 147
        '
        'cboallocation
        '
        Me.cboallocation.BackColor = System.Drawing.SystemColors.Window
        Me.cboallocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboallocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboallocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboallocation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboallocation, New Integer(-1) {})
        Me.cboallocation.Location = New System.Drawing.Point(112, 48)
        Me.cboallocation.Name = "cboallocation"
        Me.cboallocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboallocation.Size = New System.Drawing.Size(177, 21)
        Me.cboallocation.TabIndex = 368
        '
        'cbodtAllowed
        '
        Me.cbodtAllowed.BackColor = System.Drawing.SystemColors.Window
        Me.cbodtAllowed.Cursor = System.Windows.Forms.Cursors.Default
        Me.cbodtAllowed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbodtAllowed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbodtAllowed.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cbodtAllowed, New Integer(-1) {})
        Me.cbodtAllowed.Location = New System.Drawing.Point(112, 24)
        Me.cbodtAllowed.Name = "cbodtAllowed"
        Me.cbodtAllowed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbodtAllowed.Size = New System.Drawing.Size(177, 21)
        Me.cbodtAllowed.TabIndex = 369
        '
        'cboOSMTAUserGroup
        '
        Me.cboOSMTAUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboOSMTAUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOSMTAUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOSMTAUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOSMTAUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOSMTAUserGroup, New Integer(-1) {})
        Me.cboOSMTAUserGroup.Location = New System.Drawing.Point(112, 72)
        Me.cboOSMTAUserGroup.Name = "cboOSMTAUserGroup"
        Me.cboOSMTAUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOSMTAUserGroup.Size = New System.Drawing.Size(177, 21)
        Me.cboOSMTAUserGroup.TabIndex = 388
        '
        'cboOSMTATaskGroup
        '
        Me.cboOSMTATaskGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboOSMTATaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOSMTATaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOSMTATaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOSMTATaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOSMTATaskGroup, New Integer(-1) {})
        Me.cboOSMTATaskGroup.Location = New System.Drawing.Point(112, 96)
        Me.cboOSMTATaskGroup.Name = "cboOSMTATaskGroup"
        Me.cboOSMTATaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOSMTATaskGroup.Size = New System.Drawing.Size(177, 21)
        Me.cboOSMTATaskGroup.TabIndex = 390
        '
        'cboMonthInCycleSA
        '
        Me.cboMonthInCycleSA.BackColor = System.Drawing.SystemColors.Window
        Me.cboMonthInCycleSA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMonthInCycleSA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonthInCycleSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonthInCycleSA.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMonthInCycleSA, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.cboMonthInCycleSA.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"})
        Me.cboMonthInCycleSA.Location = New System.Drawing.Point(232, 48)
        Me.cboMonthInCycleSA.Name = "cboMonthInCycleSA"
        Me.cboMonthInCycleSA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMonthInCycleSA.Size = New System.Drawing.Size(65, 21)
        Me.cboMonthInCycleSA.TabIndex = 114
        Me.cboMonthInCycleSA.Visible = False
        '
        'cboMonthInCycleLA
        '
        Me.cboMonthInCycleLA.BackColor = System.Drawing.SystemColors.Window
        Me.cboMonthInCycleLA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMonthInCycleLA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonthInCycleLA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonthInCycleLA.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboMonthInCycleLA, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.cboMonthInCycleLA.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"})
        Me.cboMonthInCycleLA.Location = New System.Drawing.Point(232, 48)
        Me.cboMonthInCycleLA.Name = "cboMonthInCycleLA"
        Me.cboMonthInCycleLA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMonthInCycleLA.Size = New System.Drawing.Size(65, 21)
        Me.cboMonthInCycleLA.TabIndex = 105
        Me.cboMonthInCycleLA.Visible = False
        '
        'cboProvClaimAutoNumberingID
        '
        Me.cboProvClaimAutoNumberingID.BackColor = System.Drawing.SystemColors.Window
        Me.cboProvClaimAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProvClaimAutoNumberingID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProvClaimAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProvClaimAutoNumberingID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboProvClaimAutoNumberingID, New Integer(-1) {})
        Me.cboProvClaimAutoNumberingID.Location = New System.Drawing.Point(231, 18)
        Me.cboProvClaimAutoNumberingID.Name = "cboProvClaimAutoNumberingID"
        Me.cboProvClaimAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProvClaimAutoNumberingID.Size = New System.Drawing.Size(177, 21)
        Me.cboProvClaimAutoNumberingID.TabIndex = 14
        '
        'cboFullClaimAutoNumberingID
        '
        Me.cboFullClaimAutoNumberingID.BackColor = System.Drawing.SystemColors.Window
        Me.cboFullClaimAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFullClaimAutoNumberingID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFullClaimAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFullClaimAutoNumberingID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboFullClaimAutoNumberingID, New Integer(-1) {})
        Me.cboFullClaimAutoNumberingID.Location = New System.Drawing.Point(231, 48)
        Me.cboFullClaimAutoNumberingID.Name = "cboFullClaimAutoNumberingID"
        Me.cboFullClaimAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFullClaimAutoNumberingID.Size = New System.Drawing.Size(176, 21)
        Me.cboFullClaimAutoNumberingID.TabIndex = 15
        '
        'cboQuoteAutoNumberingID
        '
        Me.cboQuoteAutoNumberingID.BackColor = System.Drawing.SystemColors.Window
        Me.cboQuoteAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboQuoteAutoNumberingID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboQuoteAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboQuoteAutoNumberingID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboQuoteAutoNumberingID, New Integer(-1) {})
        Me.cboQuoteAutoNumberingID.Location = New System.Drawing.Point(157, 44)
        Me.cboQuoteAutoNumberingID.Name = "cboQuoteAutoNumberingID"
        Me.cboQuoteAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboQuoteAutoNumberingID.Size = New System.Drawing.Size(173, 21)
        Me.cboQuoteAutoNumberingID.TabIndex = 11
        '
        'cboPolicyAutoNumberingID
        '
        Me.cboPolicyAutoNumberingID.BackColor = System.Drawing.SystemColors.Window
        Me.cboPolicyAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPolicyAutoNumberingID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPolicyAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPolicyAutoNumberingID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboPolicyAutoNumberingID, New Integer(-1) {})
        Me.cboPolicyAutoNumberingID.Location = New System.Drawing.Point(157, 72)
        Me.cboPolicyAutoNumberingID.Name = "cboPolicyAutoNumberingID"
        Me.cboPolicyAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPolicyAutoNumberingID.Size = New System.Drawing.Size(173, 21)
        Me.cboPolicyAutoNumberingID.TabIndex = 12
        '
        'cboCNAutoNumberingID
        '
        Me.cboCNAutoNumberingID.BackColor = System.Drawing.SystemColors.Window
        Me.cboCNAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCNAutoNumberingID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCNAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCNAutoNumberingID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboCNAutoNumberingID, New Integer(-1) {})
        Me.cboCNAutoNumberingID.Location = New System.Drawing.Point(157, 124)
        Me.cboCNAutoNumberingID.Name = "cboCNAutoNumberingID"
        Me.cboCNAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCNAutoNumberingID.Size = New System.Drawing.Size(173, 21)
        Me.cboCNAutoNumberingID.TabIndex = 14
        '
        'cboPosValues
        '
        Me.cboPosValues.BackColor = System.Drawing.SystemColors.Window
        Me.cboPosValues.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPosValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPosValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPosValues.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboPosValues, New Integer(-1) {})
        Me.cboPosValues.Location = New System.Drawing.Point(200, 16)
        Me.cboPosValues.Name = "cboPosValues"
        Me.cboPosValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPosValues.Size = New System.Drawing.Size(134, 21)
        Me.cboPosValues.TabIndex = 30
        '
        'cboReminderUserGroup
        '
        Me.cboReminderUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReminderUserGroup.FormattingEnabled = True
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboReminderUserGroup, New Integer(-1) {})
        Me.cboReminderUserGroup.Location = New System.Drawing.Point(341, 16)
        Me.cboReminderUserGroup.Name = "cboReminderUserGroup"
        Me.cboReminderUserGroup.Size = New System.Drawing.Size(159, 21)
        Me.cboReminderUserGroup.TabIndex = 4
        '
        'cboReminderTaskGroup
        '
        Me.cboReminderTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReminderTaskGroup.FormattingEnabled = True
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboReminderTaskGroup, New Integer(-1) {})
        Me.cboReminderTaskGroup.Location = New System.Drawing.Point(644, 16)
        Me.cboReminderTaskGroup.Name = "cboReminderTaskGroup"
        Me.cboReminderTaskGroup.Size = New System.Drawing.Size(164, 21)
        Me.cboReminderTaskGroup.TabIndex = 5
        '
        'ComboBox1
        '
        Me.ComboBox1.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox1.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox1, New Integer(-1) {})
        Me.ComboBox1.Location = New System.Drawing.Point(200, 16)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox1.Size = New System.Drawing.Size(134, 21)
        Me.ComboBox1.TabIndex = 30
        '
        'ComboBox2
        '
        Me.ComboBox2.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox2.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox2, New Integer(-1) {})
        Me.ComboBox2.Location = New System.Drawing.Point(157, 124)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox2.Size = New System.Drawing.Size(173, 21)
        Me.ComboBox2.TabIndex = 14
        '
        'ComboBox3
        '
        Me.ComboBox3.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox3.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox3, New Integer(-1) {})
        Me.ComboBox3.Location = New System.Drawing.Point(157, 72)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox3.Size = New System.Drawing.Size(173, 21)
        Me.ComboBox3.TabIndex = 12
        '
        'ComboBox4
        '
        Me.ComboBox4.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox4.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox4, New Integer(-1) {})
        Me.ComboBox4.Location = New System.Drawing.Point(157, 44)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox4.Size = New System.Drawing.Size(173, 21)
        Me.ComboBox4.TabIndex = 11
        '
        'ComboBox5
        '
        Me.ComboBox5.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox5.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox5, New Integer(-1) {})
        Me.ComboBox5.Location = New System.Drawing.Point(231, 48)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox5.Size = New System.Drawing.Size(176, 21)
        Me.ComboBox5.TabIndex = 15
        '
        'ComboBox6
        '
        Me.ComboBox6.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox6.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox6, New Integer(-1) {})
        Me.ComboBox6.Location = New System.Drawing.Point(231, 18)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox6.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox6.TabIndex = 14
        '
        'ComboBox7
        '
        Me.ComboBox7.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox7.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox7, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.ComboBox7.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"})
        Me.ComboBox7.Location = New System.Drawing.Point(232, 48)
        Me.ComboBox7.Name = "ComboBox7"
        Me.ComboBox7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox7.Size = New System.Drawing.Size(65, 21)
        Me.ComboBox7.TabIndex = 105
        Me.ComboBox7.Visible = False
        '
        'ComboBox8
        '
        Me.ComboBox8.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox8.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox8, New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
        Me.ComboBox8.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11"})
        Me.ComboBox8.Location = New System.Drawing.Point(232, 48)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox8.Size = New System.Drawing.Size(65, 21)
        Me.ComboBox8.TabIndex = 114
        Me.ComboBox8.Visible = False
        '
        'ComboBox9
        '
        Me.ComboBox9.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox9.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox9, New Integer(-1) {})
        Me.ComboBox9.Location = New System.Drawing.Point(112, 96)
        Me.ComboBox9.Name = "ComboBox9"
        Me.ComboBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox9.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox9.TabIndex = 390
        '
        'ComboBox10
        '
        Me.ComboBox10.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox10.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox10, New Integer(-1) {})
        Me.ComboBox10.Location = New System.Drawing.Point(112, 72)
        Me.ComboBox10.Name = "ComboBox10"
        Me.ComboBox10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox10.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox10.TabIndex = 388
        '
        'ComboBox11
        '
        Me.ComboBox11.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox11.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox11, New Integer(-1) {})
        Me.ComboBox11.Location = New System.Drawing.Point(112, 24)
        Me.ComboBox11.Name = "ComboBox11"
        Me.ComboBox11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox11.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox11.TabIndex = 369
        '
        'ComboBox12
        '
        Me.ComboBox12.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox12.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox12, New Integer(-1) {})
        Me.ComboBox12.Location = New System.Drawing.Point(112, 48)
        Me.ComboBox12.Name = "ComboBox12"
        Me.ComboBox12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox12.Size = New System.Drawing.Size(177, 21)
        Me.ComboBox12.TabIndex = 368
        '
        'ComboBox13
        '
        Me.ComboBox13.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox13.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox13, New Integer(-1) {})
        Me.ComboBox13.Location = New System.Drawing.Point(78, 50)
        Me.ComboBox13.Name = "ComboBox13"
        Me.ComboBox13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox13.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox13.TabIndex = 147
        '
        'ComboBox14
        '
        Me.ComboBox14.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox14.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox14, New Integer(-1) {})
        Me.ComboBox14.Location = New System.Drawing.Point(78, 23)
        Me.ComboBox14.Name = "ComboBox14"
        Me.ComboBox14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox14.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox14.TabIndex = 145
        '
        'ComboBox15
        '
        Me.ComboBox15.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox15.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox15.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox15, New Integer(-1) {})
        Me.ComboBox15.Location = New System.Drawing.Point(78, 104)
        Me.ComboBox15.Name = "ComboBox15"
        Me.ComboBox15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox15.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox15.TabIndex = 151
        '
        'ComboBox16
        '
        Me.ComboBox16.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox16.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox16.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox16, New Integer(-1) {})
        Me.ComboBox16.Location = New System.Drawing.Point(78, 77)
        Me.ComboBox16.Name = "ComboBox16"
        Me.ComboBox16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox16.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox16.TabIndex = 149
        '
        'ComboBox17
        '
        Me.ComboBox17.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox17.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox17.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox17.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox17, New Integer(-1) {})
        Me.ComboBox17.Location = New System.Drawing.Point(78, 131)
        Me.ComboBox17.Name = "ComboBox17"
        Me.ComboBox17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox17.Size = New System.Drawing.Size(214, 21)
        Me.ComboBox17.TabIndex = 153
        '
        'ComboBox18
        '
        Me.ComboBox18.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox18.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox18.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox18, New Integer(-1) {})
        Me.ComboBox18.Location = New System.Drawing.Point(281, 65)
        Me.ComboBox18.Name = "ComboBox18"
        Me.ComboBox18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox18.Size = New System.Drawing.Size(210, 21)
        Me.ComboBox18.TabIndex = 160
        '
        'ComboBox19
        '
        Me.ComboBox19.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox19.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox19.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox19, New Integer(-1) {})
        Me.ComboBox19.Location = New System.Drawing.Point(281, 92)
        Me.ComboBox19.Name = "ComboBox19"
        Me.ComboBox19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox19.Size = New System.Drawing.Size(210, 21)
        Me.ComboBox19.TabIndex = 162
        '
        'ComboBox20
        '
        Me.ComboBox20.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox20.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox20.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox20, New Integer(-1) {})
        Me.ComboBox20.Location = New System.Drawing.Point(277, 127)
        Me.ComboBox20.Name = "ComboBox20"
        Me.ComboBox20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox20.Size = New System.Drawing.Size(192, 21)
        Me.ComboBox20.TabIndex = 142
        '
        'ComboBox21
        '
        Me.ComboBox21.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox21.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox21, New Integer(-1) {})
        Me.ComboBox21.Location = New System.Drawing.Point(513, 68)
        Me.ComboBox21.Name = "ComboBox21"
        Me.ComboBox21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox21.Size = New System.Drawing.Size(180, 21)
        Me.ComboBox21.TabIndex = 289
        Me.ComboBox21.Text = "Combo1"
        '
        'ComboBox22
        '
        Me.ComboBox22.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox22.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox22.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox22, New Integer(-1) {})
        Me.ComboBox22.Location = New System.Drawing.Point(513, 42)
        Me.ComboBox22.Name = "ComboBox22"
        Me.ComboBox22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox22.Size = New System.Drawing.Size(180, 21)
        Me.ComboBox22.TabIndex = 288
        Me.ComboBox22.Text = "Combo1"
        '
        'ComboBox23
        '
        Me.ComboBox23.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox23.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.ComboBox23, New Integer(-1) {})
        Me.ComboBox23.Location = New System.Drawing.Point(513, 17)
        Me.ComboBox23.Name = "ComboBox23"
        Me.ComboBox23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox23.Size = New System.Drawing.Size(180, 21)
        Me.ComboBox23.TabIndex = 287
        Me.ComboBox23.Text = "Combo1"
        '
        'chkDefaultCovertoDatetolastday
        '
        Me.chkDefaultCovertoDatetolastday.BackColor = System.Drawing.SystemColors.Control
        Me.chkDefaultCovertoDatetolastday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDefaultCovertoDatetolastday.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDefaultCovertoDatetolastday.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDefaultCovertoDatetolastday.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDefaultCovertoDatetolastday.Location = New System.Drawing.Point(270, 67)
        Me.chkDefaultCovertoDatetolastday.Name = "chkDefaultCovertoDatetolastday"
        Me.chkDefaultCovertoDatetolastday.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDefaultCovertoDatetolastday.Size = New System.Drawing.Size(205, 32)
        Me.chkDefaultCovertoDatetolastday.TabIndex = 388
        Me.chkDefaultCovertoDatetolastday.Text = "Default Cover to Date to last day of 12th month?"
        Me.chkDefaultCovertoDatetolastday.UseVisualStyleBackColor = False
        '
        'fraBranches
        '
        Me.fraBranches.BackColor = System.Drawing.SystemColors.Control
        Me.fraBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBranches.Location = New System.Drawing.Point(3, 6)
        Me.fraBranches.Name = "fraBranches"
        Me.fraBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBranches.Size = New System.Drawing.Size(819, 195)
        Me.fraBranches.TabIndex = 186
        Me.fraBranches.TabStop = False
        Me.fraBranches.Text = "Branches"
        '
        'uctPickListBranches
        '
        Me.uctPickListBranches.AvailableCaption = "Restrict to Branches"
        Me.uctPickListBranches.BusinessObject = "bSIRProduct.Business"
        Me.uctPickListBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListBranches.ForeignKeys = CType(resources.GetObject("uctPickListBranches.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListBranches.IsSearchable = False
        Me.uctPickListBranches.Location = New System.Drawing.Point(8, 11)
        Me.uctPickListBranches.Name = "uctPickListBranches"
        Me.uctPickListBranches.PickListType = "Source"
        Me.uctPickListBranches.Size = New System.Drawing.Size(805, 174)
        Me.uctPickListBranches.TabIndex = 21
        '
        '_tabMainTab_TabPage10
        '
        Me._tabMainTab_TabPage10.Controls.Add(Me.GroupBox40)
        Me._tabMainTab_TabPage10.Controls.Add(Me.GroupBox42)
        Me._tabMainTab_TabPage10.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage10.Name = "_tabMainTab_TabPage10"
        Me._tabMainTab_TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage10.Size = New System.Drawing.Size(842, 656)
        Me._tabMainTab_TabPage10.TabIndex = 10
        Me._tabMainTab_TabPage10.Text = "6-Branches"
        Me._tabMainTab_TabPage10.UseVisualStyleBackColor = True
        '
        'GroupBox40
        '
        Me.GroupBox40.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox40.Controls.Add(Me.uctPickListBranches)
        Me.GroupBox40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox40.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox40.Location = New System.Drawing.Point(3, 6)
        Me.GroupBox40.Name = "GroupBox40"
        Me.GroupBox40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox40.Size = New System.Drawing.Size(819, 195)
        Me.GroupBox40.TabIndex = 186
        Me.GroupBox40.TabStop = False
        Me.GroupBox40.Text = "Branches"
        '
        'GroupBox42
        '
        Me.GroupBox42.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox42.Controls.Add(Me.PickList6)
        Me.GroupBox42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox42.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox42.Location = New System.Drawing.Point(5, 3)
        Me.GroupBox42.Name = "GroupBox42"
        Me.GroupBox42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox42.Size = New System.Drawing.Size(819, 195)
        Me.GroupBox42.TabIndex = 187
        Me.GroupBox42.TabStop = False
        Me.GroupBox42.Text = "Branches"
        '
        'PickList6
        '
        Me.PickList6.AvailableCaption = "Restrict to Branches"
        Me.PickList6.BusinessObject = "bSIRProduct.Business"
        Me.PickList6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList6.ForeignKeys = CType(resources.GetObject("PickList6.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList6.IsSearchable = False
        Me.PickList6.Location = New System.Drawing.Point(8, 11)
        Me.PickList6.Name = "PickList6"
        Me.PickList6.PickListType = "Source"
        Me.PickList6.Size = New System.Drawing.Size(805, 174)
        Me.PickList6.TabIndex = 21
        '
        'PickList4
        '
        Me.PickList4.AvailableCaption = "Restrict to Branches"
        Me.PickList4.BusinessObject = "bSIRProduct.Business"
        Me.PickList4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList4.ForeignKeys = CType(resources.GetObject("PickList4.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList4.IsSearchable = False
        Me.PickList4.Location = New System.Drawing.Point(8, 11)
        Me.PickList4.Name = "PickList4"
        Me.PickList4.PickListType = "Source"
        Me.PickList4.Size = New System.Drawing.Size(805, 174)
        Me.PickList4.TabIndex = 21
        '
        '_tabMainTab_TabPage9
        '
        Me._tabMainTab_TabPage9.Controls.Add(Me.uctSIRSelectClauses)
        Me._tabMainTab_TabPage9.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage9.Name = "_tabMainTab_TabPage9"
        Me._tabMainTab_TabPage9.Size = New System.Drawing.Size(842, 656)
        Me._tabMainTab_TabPage9.TabIndex = 9
        Me._tabMainTab_TabPage9.Text = "6-Clauses"
        Me._tabMainTab_TabPage9.UseVisualStyleBackColor = True
        '
        'uctSIRSelectClauses
        '
        Me.uctSIRSelectClauses.ClauseId = 0
        Me.uctSIRSelectClauses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctSIRSelectClauses.imglImages = Nothing
        Me.uctSIRSelectClauses.Location = New System.Drawing.Point(8, 12)
        Me.uctSIRSelectClauses.Name = "uctSIRSelectClauses"
        Me.uctSIRSelectClauses.ProductId = 0
        Me.uctSIRSelectClauses.RiskId = 0
        Me.uctSIRSelectClauses.ScreenHierarchy = ""
        Me.uctSIRSelectClauses.Size = New System.Drawing.Size(819, 519)
        Me.uctSIRSelectClauses.SystemCurrency = 0
        Me.uctSIRSelectClauses.TabIndex = 382
        Me.uctSIRSelectClauses.Task = 0
        Me.uctSIRSelectClauses.UniqueId = ""
        '
        '_tabMainTab_TabPage8
        '
        Me._tabMainTab_TabPage8.Controls.Add(Me.DocumentLinkClaim)
        Me._tabMainTab_TabPage8.Controls.Add(Me.DocumentLinkClaimPayment)
        Me._tabMainTab_TabPage8.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage8.Name = "_tabMainTab_TabPage8"
        Me._tabMainTab_TabPage8.Size = New System.Drawing.Size(842, 656)
        Me._tabMainTab_TabPage8.TabIndex = 8
        Me._tabMainTab_TabPage8.Text = "9-Claims Documents"
        Me._tabMainTab_TabPage8.UseVisualStyleBackColor = True
        '
        'DocumentLinkClaim
        '
        Me.DocumentLinkClaim.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocumentLinkClaim.Location = New System.Drawing.Point(10, 14)
        Me.DocumentLinkClaim.Name = "DocumentLinkClaim"
        Me.DocumentLinkClaim.ScreenHierarchy = ""
        Me.DocumentLinkClaim.Size = New System.Drawing.Size(817, 243)
        Me.DocumentLinkClaim.Status = 0
        Me.DocumentLinkClaim.TabIndex = 381
        Me.DocumentLinkClaim.Task = 0
        Me.DocumentLinkClaim.UniqueId = ""
        Me.DocumentLinkClaim.WrittenPolicyStatus = False
        '
        'DocumentLinkClaimPayment
        '
        Me.DocumentLinkClaimPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocumentLinkClaimPayment.Location = New System.Drawing.Point(10, 270)
        Me.DocumentLinkClaimPayment.Name = "DocumentLinkClaimPayment"
        Me.DocumentLinkClaimPayment.ScreenHierarchy = ""
        Me.DocumentLinkClaimPayment.Size = New System.Drawing.Size(817, 243)
        Me.DocumentLinkClaimPayment.Status = 0
        Me.DocumentLinkClaimPayment.TabIndex = 386
        Me.DocumentLinkClaimPayment.Task = 0
        Me.DocumentLinkClaimPayment.UniqueId = ""
        Me.DocumentLinkClaimPayment.WrittenPolicyStatus = False
        '
        '_tabMainTab_TabPage7
        '
        Me._tabMainTab_TabPage7.Controls.Add(Me.uctDocumentLink1)
        Me._tabMainTab_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage7.Name = "_tabMainTab_TabPage7"
        Me._tabMainTab_TabPage7.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage7.TabIndex = 7
        Me._tabMainTab_TabPage7.Text = "8-Document Links"
        Me._tabMainTab_TabPage7.UseVisualStyleBackColor = True
        '
        'uctDocumentLink1
        '
        Me.uctDocumentLink1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctDocumentLink1.Location = New System.Drawing.Point(8, 14)
        Me.uctDocumentLink1.Name = "uctDocumentLink1"
        Me.uctDocumentLink1.ScreenHierarchy = ""
        Me.uctDocumentLink1.Size = New System.Drawing.Size(824, 499)
        Me.uctDocumentLink1.Status = 0
        Me.uctDocumentLink1.TabIndex = 293
        Me.uctDocumentLink1.Task = 0
        Me.uctDocumentLink1.UniqueId = ""
        Me.uctDocumentLink1.WrittenPolicyStatus = False
        '
        '_tabMainTab_TabPage6
        '
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraAgentEmailComm)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraTrueMonthlyPolFreqOfComm)
        Me._tabMainTab_TabPage6.Controls.Add(Me.fraDirectConsumerComm)
        Me._tabMainTab_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage6.Name = "_tabMainTab_TabPage6"
        Me._tabMainTab_TabPage6.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage6.TabIndex = 6
        Me._tabMainTab_TabPage6.Text = "7-Renewals On-Line"
        Me._tabMainTab_TabPage6.UseVisualStyleBackColor = True
        '
        'fraAgentEmailComm
        '
        Me.fraAgentEmailComm.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgentEmailComm.Controls.Add(Me.cboAgentUpdateAttachment)
        Me.fraAgentEmailComm.Controls.Add(Me.cboAgentInviteAttachment)
        Me.fraAgentEmailComm.Controls.Add(Me.cboAgentReviewAttachment)
        Me.fraAgentEmailComm.Controls.Add(Me.chkAgentRenInvite)
        Me.fraAgentEmailComm.Controls.Add(Me.chkAgentRenUpdate)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommDeSelect_20)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommSelect_20)
        Me.fraAgentEmailComm.Controls.Add(Me._txtEmailComm_20)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommDeSelect_19)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommSelect_19)
        Me.fraAgentEmailComm.Controls.Add(Me._txtEmailComm_19)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommDeSelect_18)
        Me.fraAgentEmailComm.Controls.Add(Me._cmdEmailCommSelect_18)
        Me.fraAgentEmailComm.Controls.Add(Me._txtEmailComm_18)
        Me.fraAgentEmailComm.Controls.Add(Me.chkAgentRenSelection)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_23)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_20)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_22)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_19)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_21)
        Me.fraAgentEmailComm.Controls.Add(Me._lblEmailComm_18)
        Me.fraAgentEmailComm.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgentEmailComm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgentEmailComm.Location = New System.Drawing.Point(8, 376)
        Me.fraAgentEmailComm.Name = "fraAgentEmailComm"
        Me.fraAgentEmailComm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgentEmailComm.Size = New System.Drawing.Size(701, 95)
        Me.fraAgentEmailComm.TabIndex = 268
        Me.fraAgentEmailComm.TabStop = False
        Me.fraAgentEmailComm.Text = "Agent - Email Communication"
        '
        'chkAgentRenInvite
        '
        Me.chkAgentRenInvite.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgentRenInvite.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgentRenInvite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgentRenInvite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgentRenInvite.Location = New System.Drawing.Point(6, 40)
        Me.chkAgentRenInvite.Name = "chkAgentRenInvite"
        Me.chkAgentRenInvite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgentRenInvite.Size = New System.Drawing.Size(118, 22)
        Me.chkAgentRenInvite.TabIndex = 286
        Me.chkAgentRenInvite.Text = "Renewal Invite"
        Me.chkAgentRenInvite.UseVisualStyleBackColor = False
        '
        'chkAgentRenUpdate
        '
        Me.chkAgentRenUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgentRenUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgentRenUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgentRenUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgentRenUpdate.Location = New System.Drawing.Point(6, 64)
        Me.chkAgentRenUpdate.Name = "chkAgentRenUpdate"
        Me.chkAgentRenUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgentRenUpdate.Size = New System.Drawing.Size(118, 25)
        Me.chkAgentRenUpdate.TabIndex = 285
        Me.chkAgentRenUpdate.Text = "Renewal Update"
        Me.chkAgentRenUpdate.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_20
        '
        Me._cmdEmailCommDeSelect_20.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_20.Location = New System.Drawing.Point(415, 69)
        Me._cmdEmailCommDeSelect_20.Name = "_cmdEmailCommDeSelect_20"
        Me._cmdEmailCommDeSelect_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_20.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_20.TabIndex = 278
        Me._cmdEmailCommDeSelect_20.Text = "X"
        Me._cmdEmailCommDeSelect_20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_20.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_20
        '
        Me._cmdEmailCommSelect_20.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_20.Location = New System.Drawing.Point(390, 69)
        Me._cmdEmailCommSelect_20.Name = "_cmdEmailCommSelect_20"
        Me._cmdEmailCommSelect_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_20.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_20.TabIndex = 277
        Me._cmdEmailCommSelect_20.Text = "..."
        Me._cmdEmailCommSelect_20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_20.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_20
        '
        Me._txtEmailComm_20.AcceptsReturn = True
        Me._txtEmailComm_20.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_20.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_20.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_20.Location = New System.Drawing.Point(261, 69)
        Me._txtEmailComm_20.MaxLength = 0
        Me._txtEmailComm_20.Name = "_txtEmailComm_20"
        Me._txtEmailComm_20.ReadOnly = True
        Me._txtEmailComm_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_20.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_20.TabIndex = 276
        '
        '_cmdEmailCommDeSelect_19
        '
        Me._cmdEmailCommDeSelect_19.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_19.Location = New System.Drawing.Point(415, 44)
        Me._cmdEmailCommDeSelect_19.Name = "_cmdEmailCommDeSelect_19"
        Me._cmdEmailCommDeSelect_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_19.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_19.TabIndex = 275
        Me._cmdEmailCommDeSelect_19.Text = "X"
        Me._cmdEmailCommDeSelect_19.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_19.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_19
        '
        Me._cmdEmailCommSelect_19.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_19.Location = New System.Drawing.Point(390, 44)
        Me._cmdEmailCommSelect_19.Name = "_cmdEmailCommSelect_19"
        Me._cmdEmailCommSelect_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_19.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_19.TabIndex = 274
        Me._cmdEmailCommSelect_19.Text = "..."
        Me._cmdEmailCommSelect_19.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_19.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_19
        '
        Me._txtEmailComm_19.AcceptsReturn = True
        Me._txtEmailComm_19.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_19.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_19.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_19.Location = New System.Drawing.Point(261, 44)
        Me._txtEmailComm_19.MaxLength = 0
        Me._txtEmailComm_19.Name = "_txtEmailComm_19"
        Me._txtEmailComm_19.ReadOnly = True
        Me._txtEmailComm_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_19.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_19.TabIndex = 273
        '
        '_cmdEmailCommDeSelect_18
        '
        Me._cmdEmailCommDeSelect_18.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_18.Location = New System.Drawing.Point(415, 20)
        Me._cmdEmailCommDeSelect_18.Name = "_cmdEmailCommDeSelect_18"
        Me._cmdEmailCommDeSelect_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_18.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_18.TabIndex = 272
        Me._cmdEmailCommDeSelect_18.Text = "X"
        Me._cmdEmailCommDeSelect_18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_18.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_18
        '
        Me._cmdEmailCommSelect_18.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_18.Location = New System.Drawing.Point(390, 20)
        Me._cmdEmailCommSelect_18.Name = "_cmdEmailCommSelect_18"
        Me._cmdEmailCommSelect_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_18.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_18.TabIndex = 271
        Me._cmdEmailCommSelect_18.Text = "..."
        Me._cmdEmailCommSelect_18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_18.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_18
        '
        Me._txtEmailComm_18.AcceptsReturn = True
        Me._txtEmailComm_18.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_18.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_18.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_18.Location = New System.Drawing.Point(261, 20)
        Me._txtEmailComm_18.MaxLength = 0
        Me._txtEmailComm_18.Name = "_txtEmailComm_18"
        Me._txtEmailComm_18.ReadOnly = True
        Me._txtEmailComm_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_18.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_18.TabIndex = 270
        '
        'chkAgentRenSelection
        '
        Me.chkAgentRenSelection.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgentRenSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgentRenSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgentRenSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgentRenSelection.Location = New System.Drawing.Point(6, 15)
        Me.chkAgentRenSelection.Name = "chkAgentRenSelection"
        Me.chkAgentRenSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgentRenSelection.Size = New System.Drawing.Size(125, 25)
        Me.chkAgentRenSelection.TabIndex = 269
        Me.chkAgentRenSelection.Text = "Renewal Selection"
        Me.chkAgentRenSelection.UseVisualStyleBackColor = False
        '
        '_lblEmailComm_23
        '
        Me._lblEmailComm_23.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_23.Location = New System.Drawing.Point(443, 70)
        Me._lblEmailComm_23.Name = "_lblEmailComm_23"
        Me._lblEmailComm_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_23.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_23.TabIndex = 284
        Me._lblEmailComm_23.Text = "Attachment"
        '
        '_lblEmailComm_20
        '
        Me._lblEmailComm_20.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_20.Location = New System.Drawing.Point(136, 70)
        Me._lblEmailComm_20.Name = "_lblEmailComm_20"
        Me._lblEmailComm_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_20.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_20.TabIndex = 283
        Me._lblEmailComm_20.Text = "Awaiting Update"
        '
        '_lblEmailComm_22
        '
        Me._lblEmailComm_22.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_22.Location = New System.Drawing.Point(443, 45)
        Me._lblEmailComm_22.Name = "_lblEmailComm_22"
        Me._lblEmailComm_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_22.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_22.TabIndex = 282
        Me._lblEmailComm_22.Text = "Attachment"
        '
        '_lblEmailComm_19
        '
        Me._lblEmailComm_19.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_19.Location = New System.Drawing.Point(136, 45)
        Me._lblEmailComm_19.Name = "_lblEmailComm_19"
        Me._lblEmailComm_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_19.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_19.TabIndex = 281
        Me._lblEmailComm_19.Text = "Awaiting Rnl Invite"
        '
        '_lblEmailComm_21
        '
        Me._lblEmailComm_21.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_21.Location = New System.Drawing.Point(443, 21)
        Me._lblEmailComm_21.Name = "_lblEmailComm_21"
        Me._lblEmailComm_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_21.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_21.TabIndex = 280
        Me._lblEmailComm_21.Text = "Attachment"
        '
        '_lblEmailComm_18
        '
        Me._lblEmailComm_18.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_18.Location = New System.Drawing.Point(136, 21)
        Me._lblEmailComm_18.Name = "_lblEmailComm_18"
        Me._lblEmailComm_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_18.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_18.TabIndex = 279
        Me._lblEmailComm_18.Text = "Awaiting Man Review"
        '
        'fraTrueMonthlyPolFreqOfComm
        '
        Me.fraTrueMonthlyPolFreqOfComm.BackColor = System.Drawing.SystemColors.Control
        Me.fraTrueMonthlyPolFreqOfComm.Controls.Add(Me.optRenewalProcessRun)
        Me.fraTrueMonthlyPolFreqOfComm.Controls.Add(Me.optAnniversaryDate)
        Me.fraTrueMonthlyPolFreqOfComm.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTrueMonthlyPolFreqOfComm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTrueMonthlyPolFreqOfComm.Location = New System.Drawing.Point(8, 7)
        Me.fraTrueMonthlyPolFreqOfComm.Name = "fraTrueMonthlyPolFreqOfComm"
        Me.fraTrueMonthlyPolFreqOfComm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTrueMonthlyPolFreqOfComm.Size = New System.Drawing.Size(701, 43)
        Me.fraTrueMonthlyPolFreqOfComm.TabIndex = 187
        Me.fraTrueMonthlyPolFreqOfComm.TabStop = False
        Me.fraTrueMonthlyPolFreqOfComm.Text = "True Monthly Policies - Frequency of Communication"
        '
        'optRenewalProcessRun
        '
        Me.optRenewalProcessRun.BackColor = System.Drawing.SystemColors.Control
        Me.optRenewalProcessRun.Cursor = System.Windows.Forms.Cursors.Default
        Me.optRenewalProcessRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optRenewalProcessRun.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optRenewalProcessRun.Location = New System.Drawing.Point(9, 14)
        Me.optRenewalProcessRun.Name = "optRenewalProcessRun"
        Me.optRenewalProcessRun.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optRenewalProcessRun.Size = New System.Drawing.Size(252, 26)
        Me.optRenewalProcessRun.TabIndex = 189
        Me.optRenewalProcessRun.TabStop = True
        Me.optRenewalProcessRun.Text = "As Renewal Process run"
        Me.optRenewalProcessRun.UseVisualStyleBackColor = False
        '
        'optAnniversaryDate
        '
        Me.optAnniversaryDate.BackColor = System.Drawing.SystemColors.Control
        Me.optAnniversaryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAnniversaryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAnniversaryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAnniversaryDate.Location = New System.Drawing.Point(287, 14)
        Me.optAnniversaryDate.Name = "optAnniversaryDate"
        Me.optAnniversaryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAnniversaryDate.Size = New System.Drawing.Size(231, 26)
        Me.optAnniversaryDate.TabIndex = 188
        Me.optAnniversaryDate.TabStop = True
        Me.optAnniversaryDate.Text = "Anniversary Date"
        Me.optAnniversaryDate.UseVisualStyleBackColor = False
        '
        'fraDirectConsumerComm
        '
        Me.fraDirectConsumerComm.BackColor = System.Drawing.SystemColors.Control
        Me.fraDirectConsumerComm.Controls.Add(Me.fraRenewalUpdate)
        Me.fraDirectConsumerComm.Controls.Add(Me.fraRenewalInvite)
        Me.fraDirectConsumerComm.Controls.Add(Me.fraRenewalSelection)
        Me.fraDirectConsumerComm.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDirectConsumerComm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDirectConsumerComm.Location = New System.Drawing.Point(8, 53)
        Me.fraDirectConsumerComm.Name = "fraDirectConsumerComm"
        Me.fraDirectConsumerComm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDirectConsumerComm.Size = New System.Drawing.Size(701, 318)
        Me.fraDirectConsumerComm.TabIndex = 186
        Me.fraDirectConsumerComm.TabStop = False
        Me.fraDirectConsumerComm.Text = "Direct Consumer - Email Communication"
        '
        'fraRenewalUpdate
        '
        Me.fraRenewalUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.fraRenewalUpdate.Controls.Add(Me.chkEnabledRenUpdate)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_12)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_12)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_12)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_13)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_13)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_13)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_14)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_14)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_14)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_15)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_15)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_15)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_16)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_16)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_16)
        Me.fraRenewalUpdate.Controls.Add(Me._txtEmailComm_17)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommSelect_17)
        Me.fraRenewalUpdate.Controls.Add(Me._cmdEmailCommDeSelect_17)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_12)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_15)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_13)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_16)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_14)
        Me.fraRenewalUpdate.Controls.Add(Me._lblEmailComm_17)
        Me.fraRenewalUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRenewalUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRenewalUpdate.Location = New System.Drawing.Point(8, 216)
        Me.fraRenewalUpdate.Name = "fraRenewalUpdate"
        Me.fraRenewalUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRenewalUpdate.Size = New System.Drawing.Size(688, 95)
        Me.fraRenewalUpdate.TabIndex = 242
        Me.fraRenewalUpdate.TabStop = False
        Me.fraRenewalUpdate.Text = "Renewal Update"
        '
        'chkEnabledRenUpdate
        '
        Me.chkEnabledRenUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnabledRenUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnabledRenUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnabledRenUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnabledRenUpdate.Location = New System.Drawing.Point(8, 14)
        Me.chkEnabledRenUpdate.Name = "chkEnabledRenUpdate"
        Me.chkEnabledRenUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnabledRenUpdate.Size = New System.Drawing.Size(78, 25)
        Me.chkEnabledRenUpdate.TabIndex = 261
        Me.chkEnabledRenUpdate.Text = "Enabled"
        Me.chkEnabledRenUpdate.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_12
        '
        Me._txtEmailComm_12.AcceptsReturn = True
        Me._txtEmailComm_12.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_12.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_12.Location = New System.Drawing.Point(253, 20)
        Me._txtEmailComm_12.MaxLength = 0
        Me._txtEmailComm_12.Name = "_txtEmailComm_12"
        Me._txtEmailComm_12.ReadOnly = True
        Me._txtEmailComm_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_12.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_12.TabIndex = 260
        '
        '_cmdEmailCommSelect_12
        '
        Me._cmdEmailCommSelect_12.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_12.Location = New System.Drawing.Point(382, 20)
        Me._cmdEmailCommSelect_12.Name = "_cmdEmailCommSelect_12"
        Me._cmdEmailCommSelect_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_12.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_12.TabIndex = 259
        Me._cmdEmailCommSelect_12.Text = "..."
        Me._cmdEmailCommSelect_12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_12.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_12
        '
        Me._cmdEmailCommDeSelect_12.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_12.Location = New System.Drawing.Point(407, 20)
        Me._cmdEmailCommDeSelect_12.Name = "_cmdEmailCommDeSelect_12"
        Me._cmdEmailCommDeSelect_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_12.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_12.TabIndex = 258
        Me._cmdEmailCommDeSelect_12.Text = "X"
        Me._cmdEmailCommDeSelect_12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_12.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_13
        '
        Me._txtEmailComm_13.AcceptsReturn = True
        Me._txtEmailComm_13.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_13.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_13.Location = New System.Drawing.Point(504, 20)
        Me._txtEmailComm_13.MaxLength = 0
        Me._txtEmailComm_13.Name = "_txtEmailComm_13"
        Me._txtEmailComm_13.ReadOnly = True
        Me._txtEmailComm_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_13.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_13.TabIndex = 257
        '
        '_cmdEmailCommSelect_13
        '
        Me._cmdEmailCommSelect_13.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_13.Location = New System.Drawing.Point(632, 20)
        Me._cmdEmailCommSelect_13.Name = "_cmdEmailCommSelect_13"
        Me._cmdEmailCommSelect_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_13.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_13.TabIndex = 256
        Me._cmdEmailCommSelect_13.Text = "..."
        Me._cmdEmailCommSelect_13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_13.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_13
        '
        Me._cmdEmailCommDeSelect_13.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_13.Location = New System.Drawing.Point(657, 20)
        Me._cmdEmailCommDeSelect_13.Name = "_cmdEmailCommDeSelect_13"
        Me._cmdEmailCommDeSelect_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_13.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_13.TabIndex = 255
        Me._cmdEmailCommDeSelect_13.Text = "X"
        Me._cmdEmailCommDeSelect_13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_13.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_14
        '
        Me._txtEmailComm_14.AcceptsReturn = True
        Me._txtEmailComm_14.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_14.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_14.Location = New System.Drawing.Point(253, 44)
        Me._txtEmailComm_14.MaxLength = 0
        Me._txtEmailComm_14.Name = "_txtEmailComm_14"
        Me._txtEmailComm_14.ReadOnly = True
        Me._txtEmailComm_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_14.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_14.TabIndex = 254
        '
        '_cmdEmailCommSelect_14
        '
        Me._cmdEmailCommSelect_14.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_14.Location = New System.Drawing.Point(382, 44)
        Me._cmdEmailCommSelect_14.Name = "_cmdEmailCommSelect_14"
        Me._cmdEmailCommSelect_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_14.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_14.TabIndex = 253
        Me._cmdEmailCommSelect_14.Text = "..."
        Me._cmdEmailCommSelect_14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_14.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_14
        '
        Me._cmdEmailCommDeSelect_14.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_14.Location = New System.Drawing.Point(407, 44)
        Me._cmdEmailCommDeSelect_14.Name = "_cmdEmailCommDeSelect_14"
        Me._cmdEmailCommDeSelect_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_14.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_14.TabIndex = 252
        Me._cmdEmailCommDeSelect_14.Text = "X"
        Me._cmdEmailCommDeSelect_14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_14.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_15
        '
        Me._txtEmailComm_15.AcceptsReturn = True
        Me._txtEmailComm_15.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_15.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_15.Location = New System.Drawing.Point(504, 44)
        Me._txtEmailComm_15.MaxLength = 0
        Me._txtEmailComm_15.Name = "_txtEmailComm_15"
        Me._txtEmailComm_15.ReadOnly = True
        Me._txtEmailComm_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_15.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_15.TabIndex = 251
        '
        '_cmdEmailCommSelect_15
        '
        Me._cmdEmailCommSelect_15.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_15.Location = New System.Drawing.Point(632, 44)
        Me._cmdEmailCommSelect_15.Name = "_cmdEmailCommSelect_15"
        Me._cmdEmailCommSelect_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_15.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_15.TabIndex = 250
        Me._cmdEmailCommSelect_15.Text = "..."
        Me._cmdEmailCommSelect_15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_15.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_15
        '
        Me._cmdEmailCommDeSelect_15.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_15.Location = New System.Drawing.Point(658, 44)
        Me._cmdEmailCommDeSelect_15.Name = "_cmdEmailCommDeSelect_15"
        Me._cmdEmailCommDeSelect_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_15.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_15.TabIndex = 249
        Me._cmdEmailCommDeSelect_15.Text = "X"
        Me._cmdEmailCommDeSelect_15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_15.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_16
        '
        Me._txtEmailComm_16.AcceptsReturn = True
        Me._txtEmailComm_16.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_16.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_16.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_16.Location = New System.Drawing.Point(253, 69)
        Me._txtEmailComm_16.MaxLength = 0
        Me._txtEmailComm_16.Name = "_txtEmailComm_16"
        Me._txtEmailComm_16.ReadOnly = True
        Me._txtEmailComm_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_16.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_16.TabIndex = 248
        '
        '_cmdEmailCommSelect_16
        '
        Me._cmdEmailCommSelect_16.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_16.Location = New System.Drawing.Point(382, 69)
        Me._cmdEmailCommSelect_16.Name = "_cmdEmailCommSelect_16"
        Me._cmdEmailCommSelect_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_16.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_16.TabIndex = 247
        Me._cmdEmailCommSelect_16.Text = "..."
        Me._cmdEmailCommSelect_16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_16.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_16
        '
        Me._cmdEmailCommDeSelect_16.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_16.Location = New System.Drawing.Point(407, 69)
        Me._cmdEmailCommDeSelect_16.Name = "_cmdEmailCommDeSelect_16"
        Me._cmdEmailCommDeSelect_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_16.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_16.TabIndex = 246
        Me._cmdEmailCommDeSelect_16.Text = "X"
        Me._cmdEmailCommDeSelect_16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_16.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_17
        '
        Me._txtEmailComm_17.AcceptsReturn = True
        Me._txtEmailComm_17.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_17.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_17.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_17.Location = New System.Drawing.Point(504, 69)
        Me._txtEmailComm_17.MaxLength = 0
        Me._txtEmailComm_17.Name = "_txtEmailComm_17"
        Me._txtEmailComm_17.ReadOnly = True
        Me._txtEmailComm_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_17.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_17.TabIndex = 245
        '
        '_cmdEmailCommSelect_17
        '
        Me._cmdEmailCommSelect_17.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_17.Location = New System.Drawing.Point(632, 69)
        Me._cmdEmailCommSelect_17.Name = "_cmdEmailCommSelect_17"
        Me._cmdEmailCommSelect_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_17.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_17.TabIndex = 244
        Me._cmdEmailCommSelect_17.Text = "..."
        Me._cmdEmailCommSelect_17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_17.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_17
        '
        Me._cmdEmailCommDeSelect_17.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_17.Location = New System.Drawing.Point(657, 69)
        Me._cmdEmailCommDeSelect_17.Name = "_cmdEmailCommDeSelect_17"
        Me._cmdEmailCommDeSelect_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_17.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_17.TabIndex = 243
        Me._cmdEmailCommDeSelect_17.Text = "X"
        Me._cmdEmailCommDeSelect_17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_17.UseVisualStyleBackColor = False
        '
        '_lblEmailComm_12
        '
        Me._lblEmailComm_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_12.Location = New System.Drawing.Point(128, 20)
        Me._lblEmailComm_12.Name = "_lblEmailComm_12"
        Me._lblEmailComm_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_12.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_12.TabIndex = 267
        Me._lblEmailComm_12.Text = "Awaiting Man Review"
        '
        '_lblEmailComm_15
        '
        Me._lblEmailComm_15.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_15.Location = New System.Drawing.Point(435, 21)
        Me._lblEmailComm_15.Name = "_lblEmailComm_15"
        Me._lblEmailComm_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_15.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_15.TabIndex = 266
        Me._lblEmailComm_15.Text = "Attachment"
        '
        '_lblEmailComm_13
        '
        Me._lblEmailComm_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_13.Location = New System.Drawing.Point(128, 45)
        Me._lblEmailComm_13.Name = "_lblEmailComm_13"
        Me._lblEmailComm_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_13.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_13.TabIndex = 265
        Me._lblEmailComm_13.Text = "Awaiting Rnl Invite"
        '
        '_lblEmailComm_16
        '
        Me._lblEmailComm_16.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_16.Location = New System.Drawing.Point(435, 45)
        Me._lblEmailComm_16.Name = "_lblEmailComm_16"
        Me._lblEmailComm_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_16.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_16.TabIndex = 264
        Me._lblEmailComm_16.Text = "Attachment"
        '
        '_lblEmailComm_14
        '
        Me._lblEmailComm_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_14.Location = New System.Drawing.Point(128, 70)
        Me._lblEmailComm_14.Name = "_lblEmailComm_14"
        Me._lblEmailComm_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_14.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_14.TabIndex = 263
        Me._lblEmailComm_14.Text = "Awaiting Update"
        '
        '_lblEmailComm_17
        '
        Me._lblEmailComm_17.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_17.Location = New System.Drawing.Point(435, 70)
        Me._lblEmailComm_17.Name = "_lblEmailComm_17"
        Me._lblEmailComm_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_17.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_17.TabIndex = 262
        Me._lblEmailComm_17.Text = "Attachment"
        '
        'fraRenewalInvite
        '
        Me.fraRenewalInvite.BackColor = System.Drawing.SystemColors.Control
        Me.fraRenewalInvite.Controls.Add(Me.chkEnabledRenInvite)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_6)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_6)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_6)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_7)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_7)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_7)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_8)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_8)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_8)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_9)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_9)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_9)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_10)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_10)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_10)
        Me.fraRenewalInvite.Controls.Add(Me._txtEmailComm_11)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommSelect_11)
        Me.fraRenewalInvite.Controls.Add(Me._cmdEmailCommDeSelect_11)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_6)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_9)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_7)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_10)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_8)
        Me.fraRenewalInvite.Controls.Add(Me._lblEmailComm_11)
        Me.fraRenewalInvite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRenewalInvite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRenewalInvite.Location = New System.Drawing.Point(8, 117)
        Me.fraRenewalInvite.Name = "fraRenewalInvite"
        Me.fraRenewalInvite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRenewalInvite.Size = New System.Drawing.Size(688, 95)
        Me.fraRenewalInvite.TabIndex = 216
        Me.fraRenewalInvite.TabStop = False
        Me.fraRenewalInvite.Text = "Renewal Invite"
        '
        'chkEnabledRenInvite
        '
        Me.chkEnabledRenInvite.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnabledRenInvite.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnabledRenInvite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnabledRenInvite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnabledRenInvite.Location = New System.Drawing.Point(8, 14)
        Me.chkEnabledRenInvite.Name = "chkEnabledRenInvite"
        Me.chkEnabledRenInvite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnabledRenInvite.Size = New System.Drawing.Size(78, 25)
        Me.chkEnabledRenInvite.TabIndex = 235
        Me.chkEnabledRenInvite.Text = "Enabled"
        Me.chkEnabledRenInvite.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_6
        '
        Me._txtEmailComm_6.AcceptsReturn = True
        Me._txtEmailComm_6.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_6.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_6.Location = New System.Drawing.Point(253, 20)
        Me._txtEmailComm_6.MaxLength = 0
        Me._txtEmailComm_6.Name = "_txtEmailComm_6"
        Me._txtEmailComm_6.ReadOnly = True
        Me._txtEmailComm_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_6.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_6.TabIndex = 234
        '
        '_cmdEmailCommSelect_6
        '
        Me._cmdEmailCommSelect_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_6.Location = New System.Drawing.Point(382, 20)
        Me._cmdEmailCommSelect_6.Name = "_cmdEmailCommSelect_6"
        Me._cmdEmailCommSelect_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_6.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_6.TabIndex = 233
        Me._cmdEmailCommSelect_6.Text = "..."
        Me._cmdEmailCommSelect_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_6.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_6
        '
        Me._cmdEmailCommDeSelect_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_6.Location = New System.Drawing.Point(407, 20)
        Me._cmdEmailCommDeSelect_6.Name = "_cmdEmailCommDeSelect_6"
        Me._cmdEmailCommDeSelect_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_6.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_6.TabIndex = 232
        Me._cmdEmailCommDeSelect_6.Text = "X"
        Me._cmdEmailCommDeSelect_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_6.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_7
        '
        Me._txtEmailComm_7.AcceptsReturn = True
        Me._txtEmailComm_7.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_7.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_7.Location = New System.Drawing.Point(504, 20)
        Me._txtEmailComm_7.MaxLength = 0
        Me._txtEmailComm_7.Name = "_txtEmailComm_7"
        Me._txtEmailComm_7.ReadOnly = True
        Me._txtEmailComm_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_7.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_7.TabIndex = 231
        '
        '_cmdEmailCommSelect_7
        '
        Me._cmdEmailCommSelect_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_7.Location = New System.Drawing.Point(632, 20)
        Me._cmdEmailCommSelect_7.Name = "_cmdEmailCommSelect_7"
        Me._cmdEmailCommSelect_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_7.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_7.TabIndex = 230
        Me._cmdEmailCommSelect_7.Text = "..."
        Me._cmdEmailCommSelect_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_7.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_7
        '
        Me._cmdEmailCommDeSelect_7.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_7.Location = New System.Drawing.Point(657, 20)
        Me._cmdEmailCommDeSelect_7.Name = "_cmdEmailCommDeSelect_7"
        Me._cmdEmailCommDeSelect_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_7.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_7.TabIndex = 229
        Me._cmdEmailCommDeSelect_7.Text = "X"
        Me._cmdEmailCommDeSelect_7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_7.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_8
        '
        Me._txtEmailComm_8.AcceptsReturn = True
        Me._txtEmailComm_8.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_8.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_8.Location = New System.Drawing.Point(254, 44)
        Me._txtEmailComm_8.MaxLength = 0
        Me._txtEmailComm_8.Name = "_txtEmailComm_8"
        Me._txtEmailComm_8.ReadOnly = True
        Me._txtEmailComm_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_8.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_8.TabIndex = 228
        '
        '_cmdEmailCommSelect_8
        '
        Me._cmdEmailCommSelect_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_8.Location = New System.Drawing.Point(382, 44)
        Me._cmdEmailCommSelect_8.Name = "_cmdEmailCommSelect_8"
        Me._cmdEmailCommSelect_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_8.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_8.TabIndex = 227
        Me._cmdEmailCommSelect_8.Text = "..."
        Me._cmdEmailCommSelect_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_8.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_8
        '
        Me._cmdEmailCommDeSelect_8.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_8.Location = New System.Drawing.Point(407, 44)
        Me._cmdEmailCommDeSelect_8.Name = "_cmdEmailCommDeSelect_8"
        Me._cmdEmailCommDeSelect_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_8.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_8.TabIndex = 226
        Me._cmdEmailCommDeSelect_8.Text = "X"
        Me._cmdEmailCommDeSelect_8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_8.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_9
        '
        Me._txtEmailComm_9.AcceptsReturn = True
        Me._txtEmailComm_9.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_9.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_9.Location = New System.Drawing.Point(504, 44)
        Me._txtEmailComm_9.MaxLength = 0
        Me._txtEmailComm_9.Name = "_txtEmailComm_9"
        Me._txtEmailComm_9.ReadOnly = True
        Me._txtEmailComm_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_9.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_9.TabIndex = 225
        '
        '_cmdEmailCommSelect_9
        '
        Me._cmdEmailCommSelect_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_9.Location = New System.Drawing.Point(632, 44)
        Me._cmdEmailCommSelect_9.Name = "_cmdEmailCommSelect_9"
        Me._cmdEmailCommSelect_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_9.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_9.TabIndex = 224
        Me._cmdEmailCommSelect_9.Text = "..."
        Me._cmdEmailCommSelect_9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_9.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_9
        '
        Me._cmdEmailCommDeSelect_9.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_9.Location = New System.Drawing.Point(657, 44)
        Me._cmdEmailCommDeSelect_9.Name = "_cmdEmailCommDeSelect_9"
        Me._cmdEmailCommDeSelect_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_9.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_9.TabIndex = 223
        Me._cmdEmailCommDeSelect_9.Text = "X"
        Me._cmdEmailCommDeSelect_9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_9.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_10
        '
        Me._txtEmailComm_10.AcceptsReturn = True
        Me._txtEmailComm_10.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_10.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_10.Location = New System.Drawing.Point(254, 69)
        Me._txtEmailComm_10.MaxLength = 0
        Me._txtEmailComm_10.Name = "_txtEmailComm_10"
        Me._txtEmailComm_10.ReadOnly = True
        Me._txtEmailComm_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_10.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_10.TabIndex = 222
        '
        '_cmdEmailCommSelect_10
        '
        Me._cmdEmailCommSelect_10.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_10.Location = New System.Drawing.Point(382, 69)
        Me._cmdEmailCommSelect_10.Name = "_cmdEmailCommSelect_10"
        Me._cmdEmailCommSelect_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_10.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_10.TabIndex = 221
        Me._cmdEmailCommSelect_10.Text = "..."
        Me._cmdEmailCommSelect_10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_10.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_10
        '
        Me._cmdEmailCommDeSelect_10.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_10.Location = New System.Drawing.Point(407, 69)
        Me._cmdEmailCommDeSelect_10.Name = "_cmdEmailCommDeSelect_10"
        Me._cmdEmailCommDeSelect_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_10.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_10.TabIndex = 220
        Me._cmdEmailCommDeSelect_10.Text = "X"
        Me._cmdEmailCommDeSelect_10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_10.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_11
        '
        Me._txtEmailComm_11.AcceptsReturn = True
        Me._txtEmailComm_11.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_11.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_11.Location = New System.Drawing.Point(504, 69)
        Me._txtEmailComm_11.MaxLength = 0
        Me._txtEmailComm_11.Name = "_txtEmailComm_11"
        Me._txtEmailComm_11.ReadOnly = True
        Me._txtEmailComm_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_11.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_11.TabIndex = 219
        '
        '_cmdEmailCommSelect_11
        '
        Me._cmdEmailCommSelect_11.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_11.Location = New System.Drawing.Point(632, 69)
        Me._cmdEmailCommSelect_11.Name = "_cmdEmailCommSelect_11"
        Me._cmdEmailCommSelect_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_11.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_11.TabIndex = 218
        Me._cmdEmailCommSelect_11.Text = "..."
        Me._cmdEmailCommSelect_11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_11.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommDeSelect_11
        '
        Me._cmdEmailCommDeSelect_11.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_11.Location = New System.Drawing.Point(657, 69)
        Me._cmdEmailCommDeSelect_11.Name = "_cmdEmailCommDeSelect_11"
        Me._cmdEmailCommDeSelect_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_11.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_11.TabIndex = 217
        Me._cmdEmailCommDeSelect_11.Text = "X"
        Me._cmdEmailCommDeSelect_11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_11.UseVisualStyleBackColor = False
        '
        '_lblEmailComm_6
        '
        Me._lblEmailComm_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_6.Location = New System.Drawing.Point(129, 21)
        Me._lblEmailComm_6.Name = "_lblEmailComm_6"
        Me._lblEmailComm_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_6.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_6.TabIndex = 241
        Me._lblEmailComm_6.Text = "Awaiting Man Review"
        '
        '_lblEmailComm_9
        '
        Me._lblEmailComm_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_9.Location = New System.Drawing.Point(435, 21)
        Me._lblEmailComm_9.Name = "_lblEmailComm_9"
        Me._lblEmailComm_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_9.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_9.TabIndex = 240
        Me._lblEmailComm_9.Text = "Attachment"
        '
        '_lblEmailComm_7
        '
        Me._lblEmailComm_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_7.Location = New System.Drawing.Point(129, 45)
        Me._lblEmailComm_7.Name = "_lblEmailComm_7"
        Me._lblEmailComm_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_7.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_7.TabIndex = 239
        Me._lblEmailComm_7.Text = "Awaiting Rnl Invite"
        '
        '_lblEmailComm_10
        '
        Me._lblEmailComm_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_10.Location = New System.Drawing.Point(435, 45)
        Me._lblEmailComm_10.Name = "_lblEmailComm_10"
        Me._lblEmailComm_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_10.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_10.TabIndex = 238
        Me._lblEmailComm_10.Text = "Attachment"
        '
        '_lblEmailComm_8
        '
        Me._lblEmailComm_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_8.Location = New System.Drawing.Point(129, 70)
        Me._lblEmailComm_8.Name = "_lblEmailComm_8"
        Me._lblEmailComm_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_8.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_8.TabIndex = 237
        Me._lblEmailComm_8.Text = "Awaiting Update"
        '
        '_lblEmailComm_11
        '
        Me._lblEmailComm_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_11.Location = New System.Drawing.Point(435, 70)
        Me._lblEmailComm_11.Name = "_lblEmailComm_11"
        Me._lblEmailComm_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_11.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_11.TabIndex = 236
        Me._lblEmailComm_11.Text = "Attachment"
        '
        'fraRenewalSelection
        '
        Me.fraRenewalSelection.BackColor = System.Drawing.SystemColors.Control
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_5)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_5)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_5)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_4)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_4)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_4)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_3)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_3)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_3)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_2)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_2)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_2)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_1)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_1)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_1)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommDeSelect_0)
        Me.fraRenewalSelection.Controls.Add(Me._cmdEmailCommSelect_0)
        Me.fraRenewalSelection.Controls.Add(Me._txtEmailComm_0)
        Me.fraRenewalSelection.Controls.Add(Me.chkEnabledRenSelection)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_5)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_2)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_4)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_1)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_3)
        Me.fraRenewalSelection.Controls.Add(Me._lblEmailComm_0)
        Me.fraRenewalSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRenewalSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRenewalSelection.Location = New System.Drawing.Point(8, 19)
        Me.fraRenewalSelection.Name = "fraRenewalSelection"
        Me.fraRenewalSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRenewalSelection.Size = New System.Drawing.Size(686, 95)
        Me.fraRenewalSelection.TabIndex = 190
        Me.fraRenewalSelection.TabStop = False
        Me.fraRenewalSelection.Text = "Renewal Selection"
        '
        '_cmdEmailCommDeSelect_5
        '
        Me._cmdEmailCommDeSelect_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_5.Location = New System.Drawing.Point(655, 69)
        Me._cmdEmailCommDeSelect_5.Name = "_cmdEmailCommDeSelect_5"
        Me._cmdEmailCommDeSelect_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_5.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_5.TabIndex = 215
        Me._cmdEmailCommDeSelect_5.Text = "X"
        Me._cmdEmailCommDeSelect_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_5.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_5
        '
        Me._cmdEmailCommSelect_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_5.Location = New System.Drawing.Point(630, 69)
        Me._cmdEmailCommSelect_5.Name = "_cmdEmailCommSelect_5"
        Me._cmdEmailCommSelect_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_5.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_5.TabIndex = 214
        Me._cmdEmailCommSelect_5.Text = "..."
        Me._cmdEmailCommSelect_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_5.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_5
        '
        Me._txtEmailComm_5.AcceptsReturn = True
        Me._txtEmailComm_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_5.Location = New System.Drawing.Point(503, 69)
        Me._txtEmailComm_5.MaxLength = 0
        Me._txtEmailComm_5.Name = "_txtEmailComm_5"
        Me._txtEmailComm_5.ReadOnly = True
        Me._txtEmailComm_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_5.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_5.TabIndex = 212
        '
        '_cmdEmailCommDeSelect_4
        '
        Me._cmdEmailCommDeSelect_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_4.Location = New System.Drawing.Point(406, 69)
        Me._cmdEmailCommDeSelect_4.Name = "_cmdEmailCommDeSelect_4"
        Me._cmdEmailCommDeSelect_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_4.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_4.TabIndex = 211
        Me._cmdEmailCommDeSelect_4.Text = "X"
        Me._cmdEmailCommDeSelect_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_4.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_4
        '
        Me._cmdEmailCommSelect_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_4.Location = New System.Drawing.Point(380, 69)
        Me._cmdEmailCommSelect_4.Name = "_cmdEmailCommSelect_4"
        Me._cmdEmailCommSelect_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_4.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_4.TabIndex = 210
        Me._cmdEmailCommSelect_4.Text = "..."
        Me._cmdEmailCommSelect_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_4.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_4
        '
        Me._txtEmailComm_4.AcceptsReturn = True
        Me._txtEmailComm_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_4.Location = New System.Drawing.Point(253, 69)
        Me._txtEmailComm_4.MaxLength = 0
        Me._txtEmailComm_4.Name = "_txtEmailComm_4"
        Me._txtEmailComm_4.ReadOnly = True
        Me._txtEmailComm_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_4.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_4.TabIndex = 208
        '
        '_cmdEmailCommDeSelect_3
        '
        Me._cmdEmailCommDeSelect_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_3.Location = New System.Drawing.Point(655, 44)
        Me._cmdEmailCommDeSelect_3.Name = "_cmdEmailCommDeSelect_3"
        Me._cmdEmailCommDeSelect_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_3.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_3.TabIndex = 207
        Me._cmdEmailCommDeSelect_3.Text = "X"
        Me._cmdEmailCommDeSelect_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_3.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_3
        '
        Me._cmdEmailCommSelect_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_3.Location = New System.Drawing.Point(630, 44)
        Me._cmdEmailCommSelect_3.Name = "_cmdEmailCommSelect_3"
        Me._cmdEmailCommSelect_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_3.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_3.TabIndex = 206
        Me._cmdEmailCommSelect_3.Text = "..."
        Me._cmdEmailCommSelect_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_3.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_3
        '
        Me._txtEmailComm_3.AcceptsReturn = True
        Me._txtEmailComm_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_3.Location = New System.Drawing.Point(503, 44)
        Me._txtEmailComm_3.MaxLength = 0
        Me._txtEmailComm_3.Name = "_txtEmailComm_3"
        Me._txtEmailComm_3.ReadOnly = True
        Me._txtEmailComm_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_3.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_3.TabIndex = 204
        '
        '_cmdEmailCommDeSelect_2
        '
        Me._cmdEmailCommDeSelect_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_2.Location = New System.Drawing.Point(406, 44)
        Me._cmdEmailCommDeSelect_2.Name = "_cmdEmailCommDeSelect_2"
        Me._cmdEmailCommDeSelect_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_2.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_2.TabIndex = 203
        Me._cmdEmailCommDeSelect_2.Text = "X"
        Me._cmdEmailCommDeSelect_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_2.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_2
        '
        Me._cmdEmailCommSelect_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_2.Location = New System.Drawing.Point(381, 44)
        Me._cmdEmailCommSelect_2.Name = "_cmdEmailCommSelect_2"
        Me._cmdEmailCommSelect_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_2.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_2.TabIndex = 202
        Me._cmdEmailCommSelect_2.Text = "..."
        Me._cmdEmailCommSelect_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_2.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_2
        '
        Me._txtEmailComm_2.AcceptsReturn = True
        Me._txtEmailComm_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_2.Location = New System.Drawing.Point(253, 43)
        Me._txtEmailComm_2.MaxLength = 0
        Me._txtEmailComm_2.Name = "_txtEmailComm_2"
        Me._txtEmailComm_2.ReadOnly = True
        Me._txtEmailComm_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_2.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_2.TabIndex = 200
        '
        '_cmdEmailCommDeSelect_1
        '
        Me._cmdEmailCommDeSelect_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_1.Location = New System.Drawing.Point(655, 20)
        Me._cmdEmailCommDeSelect_1.Name = "_cmdEmailCommDeSelect_1"
        Me._cmdEmailCommDeSelect_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_1.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_1.TabIndex = 199
        Me._cmdEmailCommDeSelect_1.Text = "X"
        Me._cmdEmailCommDeSelect_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_1.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_1
        '
        Me._cmdEmailCommSelect_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_1.Location = New System.Drawing.Point(631, 20)
        Me._cmdEmailCommSelect_1.Name = "_cmdEmailCommSelect_1"
        Me._cmdEmailCommSelect_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_1.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_1.TabIndex = 198
        Me._cmdEmailCommSelect_1.Text = "..."
        Me._cmdEmailCommSelect_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_1.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_1
        '
        Me._txtEmailComm_1.AcceptsReturn = True
        Me._txtEmailComm_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_1.Location = New System.Drawing.Point(503, 20)
        Me._txtEmailComm_1.MaxLength = 0
        Me._txtEmailComm_1.Name = "_txtEmailComm_1"
        Me._txtEmailComm_1.ReadOnly = True
        Me._txtEmailComm_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_1.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_1.TabIndex = 196
        '
        '_cmdEmailCommDeSelect_0
        '
        Me._cmdEmailCommDeSelect_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommDeSelect_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommDeSelect_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommDeSelect_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommDeSelect_0.Location = New System.Drawing.Point(406, 20)
        Me._cmdEmailCommDeSelect_0.Name = "_cmdEmailCommDeSelect_0"
        Me._cmdEmailCommDeSelect_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommDeSelect_0.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommDeSelect_0.TabIndex = 195
        Me._cmdEmailCommDeSelect_0.Text = "X"
        Me._cmdEmailCommDeSelect_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommDeSelect_0.UseVisualStyleBackColor = False
        '
        '_cmdEmailCommSelect_0
        '
        Me._cmdEmailCommSelect_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdEmailCommSelect_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdEmailCommSelect_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdEmailCommSelect_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdEmailCommSelect_0.Location = New System.Drawing.Point(381, 20)
        Me._cmdEmailCommSelect_0.Name = "_cmdEmailCommSelect_0"
        Me._cmdEmailCommSelect_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdEmailCommSelect_0.Size = New System.Drawing.Size(23, 18)
        Me._cmdEmailCommSelect_0.TabIndex = 194
        Me._cmdEmailCommSelect_0.Text = "..."
        Me._cmdEmailCommSelect_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdEmailCommSelect_0.UseVisualStyleBackColor = False
        '
        '_txtEmailComm_0
        '
        Me._txtEmailComm_0.AcceptsReturn = True
        Me._txtEmailComm_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtEmailComm_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtEmailComm_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtEmailComm_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtEmailComm_0.Location = New System.Drawing.Point(253, 20)
        Me._txtEmailComm_0.MaxLength = 0
        Me._txtEmailComm_0.Name = "_txtEmailComm_0"
        Me._txtEmailComm_0.ReadOnly = True
        Me._txtEmailComm_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtEmailComm_0.Size = New System.Drawing.Size(128, 20)
        Me._txtEmailComm_0.TabIndex = 192
        '
        'chkEnabledRenSelection
        '
        Me.chkEnabledRenSelection.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnabledRenSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnabledRenSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnabledRenSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnabledRenSelection.Location = New System.Drawing.Point(8, 13)
        Me.chkEnabledRenSelection.Name = "chkEnabledRenSelection"
        Me.chkEnabledRenSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnabledRenSelection.Size = New System.Drawing.Size(78, 25)
        Me.chkEnabledRenSelection.TabIndex = 191
        Me.chkEnabledRenSelection.Text = "Enabled"
        Me.chkEnabledRenSelection.UseVisualStyleBackColor = False
        '
        '_lblEmailComm_5
        '
        Me._lblEmailComm_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_5.Location = New System.Drawing.Point(434, 70)
        Me._lblEmailComm_5.Name = "_lblEmailComm_5"
        Me._lblEmailComm_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_5.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_5.TabIndex = 213
        Me._lblEmailComm_5.Text = "Attachment"
        '
        '_lblEmailComm_2
        '
        Me._lblEmailComm_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_2.Location = New System.Drawing.Point(128, 70)
        Me._lblEmailComm_2.Name = "_lblEmailComm_2"
        Me._lblEmailComm_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_2.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_2.TabIndex = 209
        Me._lblEmailComm_2.Text = "Awaiting Update"
        '
        '_lblEmailComm_4
        '
        Me._lblEmailComm_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_4.Location = New System.Drawing.Point(434, 45)
        Me._lblEmailComm_4.Name = "_lblEmailComm_4"
        Me._lblEmailComm_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_4.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_4.TabIndex = 205
        Me._lblEmailComm_4.Text = "Attachment"
        '
        '_lblEmailComm_1
        '
        Me._lblEmailComm_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_1.Location = New System.Drawing.Point(128, 45)
        Me._lblEmailComm_1.Name = "_lblEmailComm_1"
        Me._lblEmailComm_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_1.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_1.TabIndex = 201
        Me._lblEmailComm_1.Text = "Awaiting Rnl Invite"
        '
        '_lblEmailComm_3
        '
        Me._lblEmailComm_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_3.Location = New System.Drawing.Point(434, 21)
        Me._lblEmailComm_3.Name = "_lblEmailComm_3"
        Me._lblEmailComm_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_3.Size = New System.Drawing.Size(70, 21)
        Me._lblEmailComm_3.TabIndex = 197
        Me._lblEmailComm_3.Text = "Attachment"
        '
        '_lblEmailComm_0
        '
        Me._lblEmailComm_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblEmailComm_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblEmailComm_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblEmailComm_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblEmailComm_0.Location = New System.Drawing.Point(128, 21)
        Me._lblEmailComm_0.Name = "_lblEmailComm_0"
        Me._lblEmailComm_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblEmailComm_0.Size = New System.Drawing.Size(120, 20)
        Me._lblEmailComm_0.TabIndex = 193
        Me._lblEmailComm_0.Text = "Awaiting Man Review"
        '
        '_tabMainTab_TabPage5
        '
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraClaimEventDesc)
        Me._tabMainTab_TabPage5.Controls.Add(Me.fraMTAEventDesc)
        Me._tabMainTab_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage5.Name = "_tabMainTab_TabPage5"
        Me._tabMainTab_TabPage5.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage5.TabIndex = 5
        Me._tabMainTab_TabPage5.Text = "6-Event"
        Me._tabMainTab_TabPage5.UseVisualStyleBackColor = True
        '
        'fraClaimEventDesc
        '
        Me.fraClaimEventDesc.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimEventDesc.Controls.Add(Me.uctPickListClaimEvent)
        Me.fraClaimEventDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimEventDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimEventDesc.Location = New System.Drawing.Point(10, 263)
        Me.fraClaimEventDesc.Name = "fraClaimEventDesc"
        Me.fraClaimEventDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimEventDesc.Size = New System.Drawing.Size(815, 250)
        Me.fraClaimEventDesc.TabIndex = 177
        Me.fraClaimEventDesc.TabStop = False
        Me.fraClaimEventDesc.Text = "Claim Event Descriptions"
        '
        'uctPickListClaimEvent
        '
        Me.uctPickListClaimEvent.AvailableCaption = ""
        Me.uctPickListClaimEvent.BusinessObject = "bSIRProduct.Business"
        Me.uctPickListClaimEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListClaimEvent.ForeignKeys = CType(resources.GetObject("uctPickListClaimEvent.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListClaimEvent.IsSearchable = False
        Me.uctPickListClaimEvent.Location = New System.Drawing.Point(8, 16)
        Me.uctPickListClaimEvent.Name = "uctPickListClaimEvent"
        Me.uctPickListClaimEvent.PickListType = "Claim"
        Me.uctPickListClaimEvent.Size = New System.Drawing.Size(793, 226)
        Me.uctPickListClaimEvent.TabIndex = 178
        '
        'fraMTAEventDesc
        '
        Me.fraMTAEventDesc.BackColor = System.Drawing.SystemColors.Control
        Me.fraMTAEventDesc.Controls.Add(Me.uctPickListMTAEvent)
        Me.fraMTAEventDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMTAEventDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMTAEventDesc.Location = New System.Drawing.Point(8, 4)
        Me.fraMTAEventDesc.Name = "fraMTAEventDesc"
        Me.fraMTAEventDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMTAEventDesc.Size = New System.Drawing.Size(819, 251)
        Me.fraMTAEventDesc.TabIndex = 175
        Me.fraMTAEventDesc.TabStop = False
        Me.fraMTAEventDesc.Text = "MTA Event Descriptions"
        '
        'uctPickListMTAEvent
        '
        Me.uctPickListMTAEvent.AvailableCaption = ""
        Me.uctPickListMTAEvent.BusinessObject = "bSIRProduct.Business"
        Me.uctPickListMTAEvent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListMTAEvent.ForeignKeys = CType(resources.GetObject("uctPickListMTAEvent.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListMTAEvent.IsSearchable = False
        Me.uctPickListMTAEvent.Location = New System.Drawing.Point(8, 16)
        Me.uctPickListMTAEvent.Name = "uctPickListMTAEvent"
        Me.uctPickListMTAEvent.PickListType = "MTA"
        Me.uctPickListMTAEvent.Size = New System.Drawing.Size(800, 219)
        Me.uctPickListMTAEvent.TabIndex = 176
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraRateMultipleRisks)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraMediaTypeStatusValidation)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraCoverNote)
        Me._tabMainTab_TabPage4.Controls.Add(Me.FraOutofSeqMTA)
        Me._tabMainTab_TabPage4.Controls.Add(Me.FraVoidTransaction)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraClaimSystemOptions)
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraNexus)
        Me._tabMainTab_TabPage4.Controls.Add(Me.Frame2)
        Me._tabMainTab_TabPage4.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5-Additional Options"
        Me._tabMainTab_TabPage4.UseVisualStyleBackColor = True
        '
        'fraRateMultipleRisks
        '
        Me.fraRateMultipleRisks.BackColor = System.Drawing.SystemColors.Control
        Me.fraRateMultipleRisks.Controls.Add(Me.chkDisplayRerateForCancellationsAndReinstatments)
        Me.fraRateMultipleRisks.Controls.Add(Me.chkDisplayRerateForMTA)
        Me.fraRateMultipleRisks.Controls.Add(Me.chkDisplayRerateForQuoteAndNB)
        Me.fraRateMultipleRisks.Controls.Add(Me.chkDisplayRerateForRenewal)
        Me.fraRateMultipleRisks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRateMultipleRisks.Location = New System.Drawing.Point(345, 148)
        Me.fraRateMultipleRisks.Name = "fraRateMultipleRisks"
        Me.fraRateMultipleRisks.Size = New System.Drawing.Size(189, 95)
        Me.fraRateMultipleRisks.TabIndex = 393
        Me.fraRateMultipleRisks.TabStop = False
        Me.fraRateMultipleRisks.Text = "Rate Multiple Risks"
        '
        'chkDisplayRerateForCancellationsAndReinstatments
        '
        Me.chkDisplayRerateForCancellationsAndReinstatments.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayRerateForCancellationsAndReinstatments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayRerateForCancellationsAndReinstatments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayRerateForCancellationsAndReinstatments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayRerateForCancellationsAndReinstatments.Location = New System.Drawing.Point(6, 47)
        Me.chkDisplayRerateForCancellationsAndReinstatments.Name = "chkDisplayRerateForCancellationsAndReinstatments"
        Me.chkDisplayRerateForCancellationsAndReinstatments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayRerateForCancellationsAndReinstatments.Size = New System.Drawing.Size(176, 30)
        Me.chkDisplayRerateForCancellationsAndReinstatments.TabIndex = 332
        Me.chkDisplayRerateForCancellationsAndReinstatments.Text = "Display rerate for Cancellations && Reinstatments"
        Me.chkDisplayRerateForCancellationsAndReinstatments.UseVisualStyleBackColor = False
        '
        'chkDisplayRerateForMTA
        '
        Me.chkDisplayRerateForMTA.AutoSize = True
        Me.chkDisplayRerateForMTA.Location = New System.Drawing.Point(6, 31)
        Me.chkDisplayRerateForMTA.Name = "chkDisplayRerateForMTA"
        Me.chkDisplayRerateForMTA.Size = New System.Drawing.Size(131, 17)
        Me.chkDisplayRerateForMTA.TabIndex = 1
        Me.chkDisplayRerateForMTA.Text = "Display rerate for MTA"
        Me.chkDisplayRerateForMTA.UseVisualStyleBackColor = True
        '
        'chkDisplayRerateForQuoteAndNB
        '
        Me.chkDisplayRerateForQuoteAndNB.AutoSize = True
        Me.chkDisplayRerateForQuoteAndNB.Location = New System.Drawing.Point(6, 15)
        Me.chkDisplayRerateForQuoteAndNB.Name = "chkDisplayRerateForQuoteAndNB"
        Me.chkDisplayRerateForQuoteAndNB.Size = New System.Drawing.Size(164, 17)
        Me.chkDisplayRerateForQuoteAndNB.TabIndex = 0
        Me.chkDisplayRerateForQuoteAndNB.Text = "Display rerate for Quote && NB"
        Me.chkDisplayRerateForQuoteAndNB.UseVisualStyleBackColor = True
        '
        'chkDisplayRerateForRenewal
        '
        Me.chkDisplayRerateForRenewal.AutoSize = True
        Me.chkDisplayRerateForRenewal.Location = New System.Drawing.Point(6, 75)
        Me.chkDisplayRerateForRenewal.Name = "chkDisplayRerateForRenewal"
        Me.chkDisplayRerateForRenewal.Size = New System.Drawing.Size(150, 17)
        Me.chkDisplayRerateForRenewal.TabIndex = 0
        Me.chkDisplayRerateForRenewal.Text = "Display rerate for Renewal"
        Me.chkDisplayRerateForRenewal.UseVisualStyleBackColor = True
        '
        'fraMediaTypeStatusValidation
        '
        Me.fraMediaTypeStatusValidation.BackColor = System.Drawing.SystemColors.Control
        Me.fraMediaTypeStatusValidation.Controls.Add(Me.chkValidateMediaTypeStatusAtClaimPayment)
        Me.fraMediaTypeStatusValidation.Controls.Add(Me.chkValidateMediaTypeStatusAtPolicyRefund)
        Me.fraMediaTypeStatusValidation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMediaTypeStatusValidation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMediaTypeStatusValidation.Location = New System.Drawing.Point(8, 148)
        Me.fraMediaTypeStatusValidation.Name = "fraMediaTypeStatusValidation"
        Me.fraMediaTypeStatusValidation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMediaTypeStatusValidation.Size = New System.Drawing.Size(331, 72)
        Me.fraMediaTypeStatusValidation.TabIndex = 392
        Me.fraMediaTypeStatusValidation.TabStop = False
        Me.fraMediaTypeStatusValidation.Text = "Media Type Status Validation"
        '
        'chkValidateMediaTypeStatusAtClaimPayment
        '
        Me.chkValidateMediaTypeStatusAtClaimPayment.BackColor = System.Drawing.SystemColors.Control
        Me.chkValidateMediaTypeStatusAtClaimPayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkValidateMediaTypeStatusAtClaimPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkValidateMediaTypeStatusAtClaimPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkValidateMediaTypeStatusAtClaimPayment.Location = New System.Drawing.Point(21, 19)
        Me.chkValidateMediaTypeStatusAtClaimPayment.Name = "chkValidateMediaTypeStatusAtClaimPayment"
        Me.chkValidateMediaTypeStatusAtClaimPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkValidateMediaTypeStatusAtClaimPayment.Size = New System.Drawing.Size(294, 17)
        Me.chkValidateMediaTypeStatusAtClaimPayment.TabIndex = 394
        Me.chkValidateMediaTypeStatusAtClaimPayment.Text = "Claim Payment - Check Media Type Status as Cleared"
        Me.chkValidateMediaTypeStatusAtClaimPayment.UseVisualStyleBackColor = False
        '
        'chkValidateMediaTypeStatusAtPolicyRefund
        '
        Me.chkValidateMediaTypeStatusAtPolicyRefund.BackColor = System.Drawing.SystemColors.Control
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkValidateMediaTypeStatusAtPolicyRefund.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Location = New System.Drawing.Point(21, 42)
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Name = "chkValidateMediaTypeStatusAtPolicyRefund"
        Me.chkValidateMediaTypeStatusAtPolicyRefund.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Size = New System.Drawing.Size(300, 16)
        Me.chkValidateMediaTypeStatusAtPolicyRefund.TabIndex = 393
        Me.chkValidateMediaTypeStatusAtPolicyRefund.Text = "Refund MTA/MTC - Check Media Type Status as Cleared"
        Me.chkValidateMediaTypeStatusAtPolicyRefund.UseVisualStyleBackColor = False
        '
        'fraCoverNote
        '
        Me.fraCoverNote.BackColor = System.Drawing.SystemColors.Control
        Me.fraCoverNote.Controls.Add(Me.cmdCNDocTemplate)
        Me.fraCoverNote.Controls.Add(Me.txtCNDocTemplate)
        Me.fraCoverNote.Controls.Add(Me.txtCNMaxNo)
        Me.fraCoverNote.Controls.Add(Me.txtCNDefaultPeriod)
        Me.fraCoverNote.Controls.Add(Me.lblCNDocTemplate)
        Me.fraCoverNote.Controls.Add(Me.lblCNMaxNo)
        Me.fraCoverNote.Controls.Add(Me.lblCNDefaultPeriod)
        Me.fraCoverNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCoverNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCoverNote.Location = New System.Drawing.Point(8, 553)
        Me.fraCoverNote.Name = "fraCoverNote"
        Me.fraCoverNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCoverNote.Size = New System.Drawing.Size(483, 84)
        Me.fraCoverNote.TabIndex = 167
        Me.fraCoverNote.TabStop = False
        Me.fraCoverNote.Text = "Cover Note"
        '
        'cmdCNDocTemplate
        '
        Me.cmdCNDocTemplate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCNDocTemplate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCNDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCNDocTemplate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCNDocTemplate.Location = New System.Drawing.Point(431, 55)
        Me.cmdCNDocTemplate.Name = "cmdCNDocTemplate"
        Me.cmdCNDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCNDocTemplate.Size = New System.Drawing.Size(35, 19)
        Me.cmdCNDocTemplate.TabIndex = 174
        Me.cmdCNDocTemplate.Text = "..."
        Me.cmdCNDocTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCNDocTemplate.UseVisualStyleBackColor = False
        '
        'txtCNDocTemplate
        '
        Me.txtCNDocTemplate.AcceptsReturn = True
        Me.txtCNDocTemplate.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNDocTemplate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNDocTemplate.Enabled = False
        Me.txtCNDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNDocTemplate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNDocTemplate.Location = New System.Drawing.Point(287, 55)
        Me.txtCNDocTemplate.MaxLength = 0
        Me.txtCNDocTemplate.Name = "txtCNDocTemplate"
        Me.txtCNDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNDocTemplate.Size = New System.Drawing.Size(137, 20)
        Me.txtCNDocTemplate.TabIndex = 173
        '
        'txtCNMaxNo
        '
        Me.txtCNMaxNo.AcceptsReturn = True
        Me.txtCNMaxNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNMaxNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNMaxNo.Enabled = False
        Me.txtCNMaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNMaxNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNMaxNo.Location = New System.Drawing.Point(287, 33)
        Me.txtCNMaxNo.MaxLength = 0
        Me.txtCNMaxNo.Name = "txtCNMaxNo"
        Me.txtCNMaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNMaxNo.Size = New System.Drawing.Size(111, 20)
        Me.txtCNMaxNo.TabIndex = 171
        '
        'txtCNDefaultPeriod
        '
        Me.txtCNDefaultPeriod.AcceptsReturn = True
        Me.txtCNDefaultPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.txtCNDefaultPeriod.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCNDefaultPeriod.Enabled = False
        Me.txtCNDefaultPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCNDefaultPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCNDefaultPeriod.Location = New System.Drawing.Point(287, 11)
        Me.txtCNDefaultPeriod.MaxLength = 0
        Me.txtCNDefaultPeriod.Name = "txtCNDefaultPeriod"
        Me.txtCNDefaultPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCNDefaultPeriod.Size = New System.Drawing.Size(111, 20)
        Me.txtCNDefaultPeriod.TabIndex = 169
        '
        'lblCNDocTemplate
        '
        Me.lblCNDocTemplate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNDocTemplate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNDocTemplate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNDocTemplate.Location = New System.Drawing.Point(21, 58)
        Me.lblCNDocTemplate.Name = "lblCNDocTemplate"
        Me.lblCNDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNDocTemplate.Size = New System.Drawing.Size(209, 25)
        Me.lblCNDocTemplate.TabIndex = 172
        Me.lblCNDocTemplate.Text = "Cover Note Doc Template"
        '
        'lblCNMaxNo
        '
        Me.lblCNMaxNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNMaxNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNMaxNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNMaxNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNMaxNo.Location = New System.Drawing.Point(20, 37)
        Me.lblCNMaxNo.Name = "lblCNMaxNo"
        Me.lblCNMaxNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNMaxNo.Size = New System.Drawing.Size(247, 21)
        Me.lblCNMaxNo.TabIndex = 170
        Me.lblCNMaxNo.Text = "Maximum No. of Cover Notes"
        '
        'lblCNDefaultPeriod
        '
        Me.lblCNDefaultPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblCNDefaultPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCNDefaultPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNDefaultPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCNDefaultPeriod.Location = New System.Drawing.Point(21, 17)
        Me.lblCNDefaultPeriod.Name = "lblCNDefaultPeriod"
        Me.lblCNDefaultPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCNDefaultPeriod.Size = New System.Drawing.Size(235, 20)
        Me.lblCNDefaultPeriod.TabIndex = 168
        Me.lblCNDefaultPeriod.Text = "Cover Note default period"
        '
        'FraOutofSeqMTA
        '
        Me.FraOutofSeqMTA.BackColor = System.Drawing.SystemColors.Control
        Me.FraOutofSeqMTA.Controls.Add(Me.cboOSMTATaskGroup)
        Me.FraOutofSeqMTA.Controls.Add(Me.cboOSMTAUserGroup)
        Me.FraOutofSeqMTA.Controls.Add(Me.cbodtAllowed)
        Me.FraOutofSeqMTA.Controls.Add(Me.cboallocation)
        Me.FraOutofSeqMTA.Controls.Add(Me.lblTaskGroup)
        Me.FraOutofSeqMTA.Controls.Add(Me.lblUserGroup)
        Me.FraOutofSeqMTA.Controls.Add(Me.Label4)
        Me.FraOutofSeqMTA.Controls.Add(Me.Label5)
        Me.FraOutofSeqMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FraOutofSeqMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FraOutofSeqMTA.Location = New System.Drawing.Point(536, 65)
        Me.FraOutofSeqMTA.Name = "FraOutofSeqMTA"
        Me.FraOutofSeqMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FraOutofSeqMTA.Size = New System.Drawing.Size(297, 135)
        Me.FraOutofSeqMTA.TabIndex = 367
        Me.FraOutofSeqMTA.TabStop = False
        Me.FraOutofSeqMTA.Text = "Out of Sequence MTAs"
        '
        'lblTaskGroup
        '
        Me.lblTaskGroup.AutoSize = True
        Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroup.Location = New System.Drawing.Point(16, 104)
        Me.lblTaskGroup.Name = "lblTaskGroup"
        Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroup.Size = New System.Drawing.Size(66, 13)
        Me.lblTaskGroup.TabIndex = 389
        Me.lblTaskGroup.Text = "Task Group:"
        '
        'lblUserGroup
        '
        Me.lblUserGroup.AutoSize = True
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(16, 80)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(64, 13)
        Me.lblUserGroup.TabIndex = 387
        Me.lblUserGroup.Text = "User Group:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(77, 13)
        Me.Label4.TabIndex = 371
        Me.Label4.Text = "Dates allowed:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(16, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 370
        Me.Label5.Text = "Allocation:"
        '
        'FraVoidTransaction
        '
        Me.FraVoidTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.FraVoidTransaction.Controls.Add(Me.chkVoidPolicyVersion)
        Me.FraVoidTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FraVoidTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FraVoidTransaction.Location = New System.Drawing.Point(536, 198)
        Me.FraVoidTransaction.Name = "FraVoidTransaction"
        Me.FraVoidTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FraVoidTransaction.Size = New System.Drawing.Size(297, 45)
        Me.FraVoidTransaction.TabIndex = 368
        Me.FraVoidTransaction.TabStop = False
        Me.FraVoidTransaction.Text = "Void Policy Version"
        '
        'chkVoidPolicyVersion
        '
        Me.chkVoidPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.chkVoidPolicyVersion.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkVoidPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVoidPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkVoidPolicyVersion.Location = New System.Drawing.Point(16, 18)
        Me.chkVoidPolicyVersion.Name = "chkVoidPolicyVersion"
        Me.chkVoidPolicyVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkVoidPolicyVersion.Size = New System.Drawing.Size(209, 17)
        Me.chkVoidPolicyVersion.TabIndex = 391
        Me.chkVoidPolicyVersion.Text = "Void Policy Version"
        Me.chkVoidPolicyVersion.UseVisualStyleBackColor = False
        '
        'fraClaimSystemOptions
        '
        Me.fraClaimSystemOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraClaimSystemOptions.Controls.Add(Me.Frame10)
        Me.fraClaimSystemOptions.Controls.Add(Me.Frame9)
        Me.fraClaimSystemOptions.Controls.Add(Me.Frame8)
        Me.fraClaimSystemOptions.Controls.Add(Me.Frame7)
        Me.fraClaimSystemOptions.Controls.Add(Me.fraExtClaimHandler)
        Me.fraClaimSystemOptions.Controls.Add(Me.fraUWClaims)
        Me.fraClaimSystemOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraClaimSystemOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraClaimSystemOptions.Location = New System.Drawing.Point(7, 240)
        Me.fraClaimSystemOptions.Name = "fraClaimSystemOptions"
        Me.fraClaimSystemOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraClaimSystemOptions.Size = New System.Drawing.Size(826, 408)
        Me.fraClaimSystemOptions.TabIndex = 134
        Me.fraClaimSystemOptions.TabStop = False
        Me.fraClaimSystemOptions.Text = "Claim System Configuration Options"
        '
        'Frame10
        '
        Me.Frame10.Controls.Add(Me.txtDeleteQuoteAfter)
        Me.Frame10.Controls.Add(Me.Label103)
        Me.Frame10.Controls.Add(Me.chkQuoteVersioning)
        Me.Frame10.Location = New System.Drawing.Point(517, 336)
        Me.Frame10.Name = "Frame10"
        Me.Frame10.Size = New System.Drawing.Size(302, 60)
        Me.Frame10.TabIndex = 373
        Me.Frame10.TabStop = False
        Me.Frame10.Text = "Quote Versioning"
        '
        'txtDeleteQuoteAfter
        '
        Me.txtDeleteQuoteAfter.Location = New System.Drawing.Point(192, 37)
        Me.txtDeleteQuoteAfter.Name = "txtDeleteQuoteAfter"
        Me.txtDeleteQuoteAfter.Size = New System.Drawing.Size(45, 20)
        Me.txtDeleteQuoteAfter.TabIndex = 2
        '
        'Label103
        '
        Me.Label103.AutoSize = True
        Me.Label103.Location = New System.Drawing.Point(13, 41)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(168, 13)
        Me.Label103.TabIndex = 1
        Me.Label103.Text = "Delete Quote Versions After(Days)"
        '
        'chkQuoteVersioning
        '
        Me.chkQuoteVersioning.AutoSize = True
        Me.chkQuoteVersioning.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkQuoteVersioning.Location = New System.Drawing.Point(12, 19)
        Me.chkQuoteVersioning.Name = "chkQuoteVersioning"
        Me.chkQuoteVersioning.Size = New System.Drawing.Size(194, 17)
        Me.chkQuoteVersioning.TabIndex = 0
        Me.chkQuoteVersioning.Text = "Quote Versioning Enabled               "
        Me.chkQuoteVersioning.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.chkQuoteVersioning.UseVisualStyleBackColor = True
        '
        'Frame9
        '
        Me.Frame9.BackColor = System.Drawing.SystemColors.Control
        Me.Frame9.Controls.Add(Me.chkBackdatedMTA)
        Me.Frame9.Controls.Add(Me.chkBackdatedCan)
        Me.Frame9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame9.Location = New System.Drawing.Point(516, 279)
        Me.Frame9.Name = "Frame9"
        Me.Frame9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame9.Size = New System.Drawing.Size(303, 53)
        Me.Frame9.TabIndex = 372
        Me.Frame9.TabStop = False
        Me.Frame9.Text = "Backdated MTA"
        '
        'chkBackdatedMTA
        '
        Me.chkBackdatedMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkBackdatedMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBackdatedMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBackdatedMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBackdatedMTA.Location = New System.Drawing.Point(16, 17)
        Me.chkBackdatedMTA.Name = "chkBackdatedMTA"
        Me.chkBackdatedMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBackdatedMTA.Size = New System.Drawing.Size(214, 17)
        Me.chkBackdatedMTA.TabIndex = 373
        Me.chkBackdatedMTA.Text = "Backdated MTA's Allowed"
        Me.chkBackdatedMTA.UseVisualStyleBackColor = False
        '
        'chkBackdatedCan
        '
        Me.chkBackdatedCan.BackColor = System.Drawing.SystemColors.Control
        Me.chkBackdatedCan.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBackdatedCan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBackdatedCan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBackdatedCan.Location = New System.Drawing.Point(16, 34)
        Me.chkBackdatedCan.Name = "chkBackdatedCan"
        Me.chkBackdatedCan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBackdatedCan.Size = New System.Drawing.Size(209, 17)
        Me.chkBackdatedCan.TabIndex = 391
        Me.chkBackdatedCan.Text = "Backdated Cancellation Allowed"
        Me.chkBackdatedCan.UseVisualStyleBackColor = False
        '
        'Frame8
        '
        Me.Frame8.BackColor = System.Drawing.SystemColors.Control
        Me.Frame8.Controls.Add(Me.chkDuplicateClaim)
        Me.Frame8.Controls.Add(Me.chkAdvanceTaxScript)
        Me.Frame8.Controls.Add(Me.chkPaymentRefCheck)
        Me.Frame8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame8.Location = New System.Drawing.Point(516, 193)
        Me.Frame8.Name = "Frame8"
        Me.Frame8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame8.Size = New System.Drawing.Size(303, 83)
        Me.Frame8.TabIndex = 163
        Me.Frame8.TabStop = False
        Me.Frame8.Text = "Claim Numbering"
        '
        'chkDuplicateClaim
        '
        Me.chkDuplicateClaim.BackColor = System.Drawing.SystemColors.Control
        Me.chkDuplicateClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDuplicateClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDuplicateClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDuplicateClaim.Location = New System.Drawing.Point(16, 15)
        Me.chkDuplicateClaim.Name = "chkDuplicateClaim"
        Me.chkDuplicateClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDuplicateClaim.Size = New System.Drawing.Size(214, 20)
        Me.chkDuplicateClaim.TabIndex = 164
        Me.chkDuplicateClaim.Text = "Duplicate Claim Check Enabled"
        Me.chkDuplicateClaim.UseVisualStyleBackColor = False
        '
        'chkAdvanceTaxScript
        '
        Me.chkAdvanceTaxScript.BackColor = System.Drawing.SystemColors.Control
        Me.chkAdvanceTaxScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAdvanceTaxScript.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAdvanceTaxScript.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAdvanceTaxScript.Location = New System.Drawing.Point(16, 35)
        Me.chkAdvanceTaxScript.Name = "chkAdvanceTaxScript"
        Me.chkAdvanceTaxScript.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAdvanceTaxScript.Size = New System.Drawing.Size(214, 20)
        Me.chkAdvanceTaxScript.TabIndex = 165
        Me.chkAdvanceTaxScript.Text = "Advanced Tax Script"
        Me.chkAdvanceTaxScript.UseVisualStyleBackColor = False
        '
        'chkPaymentRefCheck
        '
        Me.chkPaymentRefCheck.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaymentRefCheck.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaymentRefCheck.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaymentRefCheck.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaymentRefCheck.Location = New System.Drawing.Point(16, 55)
        Me.chkPaymentRefCheck.Name = "chkPaymentRefCheck"
        Me.chkPaymentRefCheck.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaymentRefCheck.Size = New System.Drawing.Size(214, 20)
        Me.chkPaymentRefCheck.TabIndex = 166
        Me.chkPaymentRefCheck.TabStop = False
        Me.chkPaymentRefCheck.Text = "Payment Reference Check"
        Me.chkPaymentRefCheck.UseVisualStyleBackColor = False
        '
        'Frame7
        '
        Me.Frame7.BackColor = System.Drawing.SystemColors.Control
        Me.Frame7.Controls.Add(Me._cboUDT_1)
        Me.Frame7.Controls.Add(Me._cboUDT_0)
        Me.Frame7.Controls.Add(Me._cboUDT_3)
        Me.Frame7.Controls.Add(Me._cboUDT_2)
        Me.Frame7.Controls.Add(Me._cboUDT_4)
        Me.Frame7.Controls.Add(Me._lblClaimSystemOptions_12)
        Me.Frame7.Controls.Add(Me._lblClaimSystemOptions_13)
        Me.Frame7.Controls.Add(Me._lblClaimSystemOptions_10)
        Me.Frame7.Controls.Add(Me._lblClaimSystemOptions_11)
        Me.Frame7.Controls.Add(Me._lblClaimSystemOptions_14)
        Me.Frame7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame7.Location = New System.Drawing.Point(514, 16)
        Me.Frame7.Name = "Frame7"
        Me.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame7.Size = New System.Drawing.Size(303, 175)
        Me.Frame7.TabIndex = 143
        Me.Frame7.TabStop = False
        Me.Frame7.Text = "User Defined Tables"
        '
        '_lblClaimSystemOptions_12
        '
        Me._lblClaimSystemOptions_12.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_12.Location = New System.Drawing.Point(14, 80)
        Me._lblClaimSystemOptions_12.Name = "_lblClaimSystemOptions_12"
        Me._lblClaimSystemOptions_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_12.Size = New System.Drawing.Size(67, 13)
        Me._lblClaimSystemOptions_12.TabIndex = 148
        Me._lblClaimSystemOptions_12.Text = "Table C :"
        '
        '_lblClaimSystemOptions_13
        '
        Me._lblClaimSystemOptions_13.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_13.Location = New System.Drawing.Point(14, 107)
        Me._lblClaimSystemOptions_13.Name = "_lblClaimSystemOptions_13"
        Me._lblClaimSystemOptions_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_13.Size = New System.Drawing.Size(67, 13)
        Me._lblClaimSystemOptions_13.TabIndex = 150
        Me._lblClaimSystemOptions_13.Text = "Table D :"
        '
        '_lblClaimSystemOptions_10
        '
        Me._lblClaimSystemOptions_10.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_10.Location = New System.Drawing.Point(14, 27)
        Me._lblClaimSystemOptions_10.Name = "_lblClaimSystemOptions_10"
        Me._lblClaimSystemOptions_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_10.Size = New System.Drawing.Size(67, 13)
        Me._lblClaimSystemOptions_10.TabIndex = 144
        Me._lblClaimSystemOptions_10.Text = "Table A :"
        '
        '_lblClaimSystemOptions_11
        '
        Me._lblClaimSystemOptions_11.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_11.Location = New System.Drawing.Point(14, 53)
        Me._lblClaimSystemOptions_11.Name = "_lblClaimSystemOptions_11"
        Me._lblClaimSystemOptions_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_11.Size = New System.Drawing.Size(67, 13)
        Me._lblClaimSystemOptions_11.TabIndex = 146
        Me._lblClaimSystemOptions_11.Text = "Table B :"
        '
        '_lblClaimSystemOptions_14
        '
        Me._lblClaimSystemOptions_14.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_14.Location = New System.Drawing.Point(14, 134)
        Me._lblClaimSystemOptions_14.Name = "_lblClaimSystemOptions_14"
        Me._lblClaimSystemOptions_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_14.Size = New System.Drawing.Size(67, 13)
        Me._lblClaimSystemOptions_14.TabIndex = 152
        Me._lblClaimSystemOptions_14.Text = "Table E :"
        '
        'fraExtClaimHandler
        '
        Me.fraExtClaimHandler.BackColor = System.Drawing.SystemColors.Control
        Me.fraExtClaimHandler.Controls.Add(Me.cboClaimTaskGroup)
        Me.fraExtClaimHandler.Controls.Add(Me.cboClaimUserGroup)
        Me.fraExtClaimHandler.Controls.Add(Me.txtAckTaskAllowedTime)
        Me.fraExtClaimHandler.Controls.Add(Me.txtPreReportAllowedTime)
        Me.fraExtClaimHandler.Controls.Add(Me._lblClaimSystemOptions_8)
        Me.fraExtClaimHandler.Controls.Add(Me._lblClaimSystemOptions_9)
        Me.fraExtClaimHandler.Controls.Add(Me._lblClaimSystemOptions_6)
        Me.fraExtClaimHandler.Controls.Add(Me._lblClaimSystemOptions_7)
        Me.fraExtClaimHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraExtClaimHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraExtClaimHandler.Location = New System.Drawing.Point(7, 196)
        Me.fraExtClaimHandler.Name = "fraExtClaimHandler"
        Me.fraExtClaimHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraExtClaimHandler.Size = New System.Drawing.Size(496, 118)
        Me.fraExtClaimHandler.TabIndex = 154
        Me.fraExtClaimHandler.TabStop = False
        Me.fraExtClaimHandler.Text = "External Claim Handler"
        '
        'txtAckTaskAllowedTime
        '
        Me.txtAckTaskAllowedTime.AcceptsReturn = True
        Me.txtAckTaskAllowedTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtAckTaskAllowedTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAckTaskAllowedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAckTaskAllowedTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAckTaskAllowedTime.Location = New System.Drawing.Point(282, 15)
        Me.txtAckTaskAllowedTime.MaxLength = 3
        Me.txtAckTaskAllowedTime.Name = "txtAckTaskAllowedTime"
        Me.txtAckTaskAllowedTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAckTaskAllowedTime.Size = New System.Drawing.Size(52, 20)
        Me.txtAckTaskAllowedTime.TabIndex = 156
        '
        'txtPreReportAllowedTime
        '
        Me.txtPreReportAllowedTime.AcceptsReturn = True
        Me.txtPreReportAllowedTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtPreReportAllowedTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPreReportAllowedTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPreReportAllowedTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPreReportAllowedTime.Location = New System.Drawing.Point(282, 40)
        Me.txtPreReportAllowedTime.MaxLength = 3
        Me.txtPreReportAllowedTime.Name = "txtPreReportAllowedTime"
        Me.txtPreReportAllowedTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPreReportAllowedTime.Size = New System.Drawing.Size(52, 20)
        Me.txtPreReportAllowedTime.TabIndex = 158
        '
        '_lblClaimSystemOptions_8
        '
        Me._lblClaimSystemOptions_8.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_8.Location = New System.Drawing.Point(15, 67)
        Me._lblClaimSystemOptions_8.Name = "_lblClaimSystemOptions_8"
        Me._lblClaimSystemOptions_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_8.Size = New System.Drawing.Size(210, 13)
        Me._lblClaimSystemOptions_8.TabIndex = 159
        Me._lblClaimSystemOptions_8.Text = "Claim Task Group:"
        '
        '_lblClaimSystemOptions_9
        '
        Me._lblClaimSystemOptions_9.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_9.Location = New System.Drawing.Point(16, 92)
        Me._lblClaimSystemOptions_9.Name = "_lblClaimSystemOptions_9"
        Me._lblClaimSystemOptions_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_9.Size = New System.Drawing.Size(210, 13)
        Me._lblClaimSystemOptions_9.TabIndex = 161
        Me._lblClaimSystemOptions_9.Text = "Claim User Group:"
        '
        '_lblClaimSystemOptions_6
        '
        Me._lblClaimSystemOptions_6.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_6.Location = New System.Drawing.Point(14, 18)
        Me._lblClaimSystemOptions_6.Name = "_lblClaimSystemOptions_6"
        Me._lblClaimSystemOptions_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_6.Size = New System.Drawing.Size(210, 13)
        Me._lblClaimSystemOptions_6.TabIndex = 155
        Me._lblClaimSystemOptions_6.Text = "Acknowledged Task Allowed Time"
        '
        '_lblClaimSystemOptions_7
        '
        Me._lblClaimSystemOptions_7.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_7.Location = New System.Drawing.Point(14, 42)
        Me._lblClaimSystemOptions_7.Name = "_lblClaimSystemOptions_7"
        Me._lblClaimSystemOptions_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_7.Size = New System.Drawing.Size(261, 20)
        Me._lblClaimSystemOptions_7.TabIndex = 157
        Me._lblClaimSystemOptions_7.Text = "Supply Preliminary Report Task Allowed Time"
        '
        'fraUWClaims
        '
        Me.fraUWClaims.BackColor = System.Drawing.SystemColors.Control
        Me.fraUWClaims.Controls.Add(Me.chkPaymentCannotExceedReserve)
        Me.fraUWClaims.Controls.Add(Me.chkAllowNegativeReserve)
        Me.fraUWClaims.Controls.Add(Me.chkValidPolicyAtLossDate)
        Me.fraUWClaims.Controls.Add(Me.chkClaimPaymentGross)
        Me.fraUWClaims.Controls.Add(Me.txtLargeLossAdviceValue)
        Me.fraUWClaims.Controls.Add(Me.cboCoinsurerInclusion)
        Me.fraUWClaims.Controls.Add(Me._lblClaimSystemOptions_0)
        Me.fraUWClaims.Controls.Add(Me._lblClaimSystemOptions_4)
        Me.fraUWClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUWClaims.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUWClaims.Location = New System.Drawing.Point(8, 16)
        Me.fraUWClaims.Name = "fraUWClaims"
        Me.fraUWClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUWClaims.Size = New System.Drawing.Size(494, 175)
        Me.fraUWClaims.TabIndex = 135
        Me.fraUWClaims.TabStop = False
        Me.fraUWClaims.Text = "Underwriting Claims"
        '
        'chkPaymentCannotExceedReserve
        '
        Me.chkPaymentCannotExceedReserve.BackColor = System.Drawing.SystemColors.Control
        Me.chkPaymentCannotExceedReserve.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPaymentCannotExceedReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPaymentCannotExceedReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPaymentCannotExceedReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPaymentCannotExceedReserve.Location = New System.Drawing.Point(10, 151)
        Me.chkPaymentCannotExceedReserve.Name = "chkPaymentCannotExceedReserve"
        Me.chkPaymentCannotExceedReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPaymentCannotExceedReserve.Size = New System.Drawing.Size(279, 20)
        Me.chkPaymentCannotExceedReserve.TabIndex = 375
        Me.chkPaymentCannotExceedReserve.Text = "Payment Cannot Exceed Reserve"
        Me.chkPaymentCannotExceedReserve.UseVisualStyleBackColor = False
        '
        'chkAllowNegativeReserve
        '
        Me.chkAllowNegativeReserve.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowNegativeReserve.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowNegativeReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowNegativeReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowNegativeReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowNegativeReserve.Location = New System.Drawing.Point(10, 60)
        Me.chkAllowNegativeReserve.Name = "chkAllowNegativeReserve"
        Me.chkAllowNegativeReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowNegativeReserve.Size = New System.Drawing.Size(279, 20)
        Me.chkAllowNegativeReserve.TabIndex = 138
        Me.chkAllowNegativeReserve.Text = "Allow Negative Reserves"
        Me.chkAllowNegativeReserve.UseVisualStyleBackColor = False
        '
        'chkValidPolicyAtLossDate
        '
        Me.chkValidPolicyAtLossDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkValidPolicyAtLossDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkValidPolicyAtLossDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkValidPolicyAtLossDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkValidPolicyAtLossDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkValidPolicyAtLossDate.Location = New System.Drawing.Point(10, 83)
        Me.chkValidPolicyAtLossDate.Name = "chkValidPolicyAtLossDate"
        Me.chkValidPolicyAtLossDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkValidPolicyAtLossDate.Size = New System.Drawing.Size(279, 20)
        Me.chkValidPolicyAtLossDate.TabIndex = 139
        Me.chkValidPolicyAtLossDate.Text = "Display only valid policy version at loss date"
        Me.chkValidPolicyAtLossDate.UseVisualStyleBackColor = False
        '
        'chkClaimPaymentGross
        '
        Me.chkClaimPaymentGross.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimPaymentGross.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkClaimPaymentGross.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimPaymentGross.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimPaymentGross.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimPaymentGross.Location = New System.Drawing.Point(10, 106)
        Me.chkClaimPaymentGross.Name = "chkClaimPaymentGross"
        Me.chkClaimPaymentGross.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimPaymentGross.Size = New System.Drawing.Size(279, 20)
        Me.chkClaimPaymentGross.TabIndex = 140
        Me.chkClaimPaymentGross.Text = "Claim Payment Amount is Gross"
        Me.chkClaimPaymentGross.UseVisualStyleBackColor = False
        '
        'txtLargeLossAdviceValue
        '
        Me.txtLargeLossAdviceValue.AcceptsReturn = True
        Me.txtLargeLossAdviceValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtLargeLossAdviceValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLargeLossAdviceValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLargeLossAdviceValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLargeLossAdviceValue.Location = New System.Drawing.Point(276, 28)
        Me.txtLargeLossAdviceValue.MaxLength = 20
        Me.txtLargeLossAdviceValue.Name = "txtLargeLossAdviceValue"
        Me.txtLargeLossAdviceValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLargeLossAdviceValue.Size = New System.Drawing.Size(168, 20)
        Me.txtLargeLossAdviceValue.TabIndex = 137
        '
        '_lblClaimSystemOptions_0
        '
        Me._lblClaimSystemOptions_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_0.Location = New System.Drawing.Point(12, 22)
        Me._lblClaimSystemOptions_0.Name = "_lblClaimSystemOptions_0"
        Me._lblClaimSystemOptions_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_0.Size = New System.Drawing.Size(214, 27)
        Me._lblClaimSystemOptions_0.TabIndex = 136
        Me._lblClaimSystemOptions_0.Text = "Reinsurance Large Loss Advice Required When Claim Value Exceeds"
        '
        '_lblClaimSystemOptions_4
        '
        Me._lblClaimSystemOptions_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblClaimSystemOptions_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblClaimSystemOptions_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblClaimSystemOptions_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblClaimSystemOptions_4.Location = New System.Drawing.Point(12, 130)
        Me._lblClaimSystemOptions_4.Name = "_lblClaimSystemOptions_4"
        Me._lblClaimSystemOptions_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblClaimSystemOptions_4.Size = New System.Drawing.Size(212, 13)
        Me._lblClaimSystemOptions_4.TabIndex = 141
        Me._lblClaimSystemOptions_4.Text = "Inclusion of Co-Insurers on Claims"
        '
        'fraNexus
        '
        Me.fraNexus.BackColor = System.Drawing.SystemColors.Control
        Me.fraNexus.Controls.Add(Me.txtOnlineCommencedOn)
        Me.fraNexus.Controls.Add(Me.chkTradeRnlOnline)
        Me.fraNexus.Controls.Add(Me.chkTradeMtaOnline)
        Me.fraNexus.Controls.Add(Me.chkTradeNbOnline)
        Me.fraNexus.Controls.Add(Me.lblOnlineCommencedOn)
        Me.fraNexus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNexus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNexus.Location = New System.Drawing.Point(8, 68)
        Me.fraNexus.Name = "fraNexus"
        Me.fraNexus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNexus.Size = New System.Drawing.Size(519, 79)
        Me.fraNexus.TabIndex = 128
        Me.fraNexus.TabStop = False
        Me.fraNexus.Text = "Nexus"
        '
        'txtOnlineCommencedOn
        '
        Me.txtOnlineCommencedOn.AcceptsReturn = True
        Me.txtOnlineCommencedOn.BackColor = System.Drawing.SystemColors.Window
        Me.txtOnlineCommencedOn.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOnlineCommencedOn.Enabled = False
        Me.txtOnlineCommencedOn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOnlineCommencedOn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOnlineCommencedOn.Location = New System.Drawing.Point(176, 52)
        Me.txtOnlineCommencedOn.MaxLength = 0
        Me.txtOnlineCommencedOn.Name = "txtOnlineCommencedOn"
        Me.txtOnlineCommencedOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOnlineCommencedOn.Size = New System.Drawing.Size(177, 20)
        Me.txtOnlineCommencedOn.TabIndex = 133
        '
        'chkTradeRnlOnline
        '
        Me.chkTradeRnlOnline.BackColor = System.Drawing.SystemColors.Control
        Me.chkTradeRnlOnline.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTradeRnlOnline.Enabled = False
        Me.chkTradeRnlOnline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTradeRnlOnline.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTradeRnlOnline.Location = New System.Drawing.Point(384, 21)
        Me.chkTradeRnlOnline.Name = "chkTradeRnlOnline"
        Me.chkTradeRnlOnline.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTradeRnlOnline.Size = New System.Drawing.Size(129, 17)
        Me.chkTradeRnlOnline.TabIndex = 131
        Me.chkTradeRnlOnline.Text = "Trade RNL on-line"
        Me.chkTradeRnlOnline.UseVisualStyleBackColor = False
        '
        'chkTradeMtaOnline
        '
        Me.chkTradeMtaOnline.BackColor = System.Drawing.SystemColors.Control
        Me.chkTradeMtaOnline.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTradeMtaOnline.Enabled = False
        Me.chkTradeMtaOnline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTradeMtaOnline.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTradeMtaOnline.Location = New System.Drawing.Point(208, 21)
        Me.chkTradeMtaOnline.Name = "chkTradeMtaOnline"
        Me.chkTradeMtaOnline.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTradeMtaOnline.Size = New System.Drawing.Size(177, 17)
        Me.chkTradeMtaOnline.TabIndex = 130
        Me.chkTradeMtaOnline.Text = "Trade MTA on-line"
        Me.chkTradeMtaOnline.UseVisualStyleBackColor = False
        '
        'chkTradeNbOnline
        '
        Me.chkTradeNbOnline.BackColor = System.Drawing.SystemColors.Control
        Me.chkTradeNbOnline.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTradeNbOnline.Enabled = False
        Me.chkTradeNbOnline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTradeNbOnline.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTradeNbOnline.Location = New System.Drawing.Point(32, 21)
        Me.chkTradeNbOnline.Name = "chkTradeNbOnline"
        Me.chkTradeNbOnline.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTradeNbOnline.Size = New System.Drawing.Size(145, 17)
        Me.chkTradeNbOnline.TabIndex = 129
        Me.chkTradeNbOnline.Text = "Trade NB on-line"
        Me.chkTradeNbOnline.UseVisualStyleBackColor = False
        '
        'lblOnlineCommencedOn
        '
        Me.lblOnlineCommencedOn.BackColor = System.Drawing.SystemColors.Control
        Me.lblOnlineCommencedOn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOnlineCommencedOn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOnlineCommencedOn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOnlineCommencedOn.Location = New System.Drawing.Point(32, 52)
        Me.lblOnlineCommencedOn.Name = "lblOnlineCommencedOn"
        Me.lblOnlineCommencedOn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOnlineCommencedOn.Size = New System.Drawing.Size(153, 25)
        Me.lblOnlineCommencedOn.TabIndex = 132
        Me.lblOnlineCommencedOn.Text = "On-line commenced On:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.chkProduceDebitNote)
        Me.Frame2.Controls.Add(Me.chkProduceCertificate)
        Me.Frame2.Controls.Add(Me.chkProduceSchedule)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(398, 12)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(434, 57)
        Me.Frame2.TabIndex = 124
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Produce Documents"
        '
        'chkProduceDebitNote
        '
        Me.chkProduceDebitNote.BackColor = System.Drawing.SystemColors.Control
        Me.chkProduceDebitNote.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProduceDebitNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProduceDebitNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProduceDebitNote.Location = New System.Drawing.Point(290, 19)
        Me.chkProduceDebitNote.Name = "chkProduceDebitNote"
        Me.chkProduceDebitNote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProduceDebitNote.Size = New System.Drawing.Size(133, 25)
        Me.chkProduceDebitNote.TabIndex = 127
        Me.chkProduceDebitNote.Text = "Produce Debit Note"
        Me.chkProduceDebitNote.UseVisualStyleBackColor = False
        '
        'chkProduceCertificate
        '
        Me.chkProduceCertificate.BackColor = System.Drawing.SystemColors.Control
        Me.chkProduceCertificate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProduceCertificate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProduceCertificate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProduceCertificate.Location = New System.Drawing.Point(154, 19)
        Me.chkProduceCertificate.Name = "chkProduceCertificate"
        Me.chkProduceCertificate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProduceCertificate.Size = New System.Drawing.Size(161, 25)
        Me.chkProduceCertificate.TabIndex = 126
        Me.chkProduceCertificate.Text = "Produce Certificate"
        Me.chkProduceCertificate.UseVisualStyleBackColor = False
        '
        'chkProduceSchedule
        '
        Me.chkProduceSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.chkProduceSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkProduceSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProduceSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkProduceSchedule.Location = New System.Drawing.Point(19, 19)
        Me.chkProduceSchedule.Name = "chkProduceSchedule"
        Me.chkProduceSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkProduceSchedule.Size = New System.Drawing.Size(161, 25)
        Me.chkProduceSchedule.TabIndex = 125
        Me.chkProduceSchedule.Text = "Produce Schedule"
        Me.chkProduceSchedule.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.chkCashDeposit)
        Me.Frame1.Controls.Add(Me.chkBankGuarantee)
        Me.Frame1.Controls.Add(Me.chkInvoice)
        Me.Frame1.Controls.Add(Me.chkInstalments)
        Me.Frame1.Controls.Add(Me.chkPayNow)
        Me.Frame1.Controls.Add(Me.lblCashDeposit)
        Me.Frame1.Controls.Add(Me.Label7)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label3)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 12)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(385, 57)
        Me.Frame1.TabIndex = 117
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Options at Make Live"
        '
        'chkCashDeposit
        '
        Me.chkCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.chkCashDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCashDeposit.Location = New System.Drawing.Point(356, 17)
        Me.chkCashDeposit.Name = "chkCashDeposit"
        Me.chkCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCashDeposit.Size = New System.Drawing.Size(24, 13)
        Me.chkCashDeposit.TabIndex = 396
        Me.chkCashDeposit.UseVisualStyleBackColor = False
        '
        'chkBankGuarantee
        '
        Me.chkBankGuarantee.BackColor = System.Drawing.SystemColors.Control
        Me.chkBankGuarantee.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBankGuarantee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBankGuarantee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBankGuarantee.Location = New System.Drawing.Point(324, 35)
        Me.chkBankGuarantee.Name = "chkBankGuarantee"
        Me.chkBankGuarantee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBankGuarantee.Size = New System.Drawing.Size(29, 13)
        Me.chkBankGuarantee.TabIndex = 383
        Me.chkBankGuarantee.UseVisualStyleBackColor = False
        '
        'chkInvoice
        '
        Me.chkInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.chkInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInvoice.Location = New System.Drawing.Point(130, 17)
        Me.chkInvoice.Name = "chkInvoice"
        Me.chkInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInvoice.Size = New System.Drawing.Size(25, 13)
        Me.chkInvoice.TabIndex = 119
        Me.chkInvoice.UseVisualStyleBackColor = False
        '
        'chkInstalments
        '
        Me.chkInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.chkInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInstalments.Location = New System.Drawing.Point(130, 35)
        Me.chkInstalments.Name = "chkInstalments"
        Me.chkInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInstalments.Size = New System.Drawing.Size(21, 13)
        Me.chkInstalments.TabIndex = 121
        Me.chkInstalments.UseVisualStyleBackColor = False
        '
        'chkPayNow
        '
        Me.chkPayNow.BackColor = System.Drawing.SystemColors.Control
        Me.chkPayNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPayNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPayNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPayNow.Location = New System.Drawing.Point(232, 17)
        Me.chkPayNow.Name = "chkPayNow"
        Me.chkPayNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPayNow.Size = New System.Drawing.Size(29, 13)
        Me.chkPayNow.TabIndex = 123
        Me.chkPayNow.UseVisualStyleBackColor = False
        '
        'lblCashDeposit
        '
        Me.lblCashDeposit.AutoSize = True
        Me.lblCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.lblCashDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCashDeposit.Location = New System.Drawing.Point(263, 17)
        Me.lblCashDeposit.Name = "lblCashDeposit"
        Me.lblCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCashDeposit.Size = New System.Drawing.Size(70, 13)
        Me.lblCashDeposit.TabIndex = 397
        Me.lblCashDeposit.Text = "Cash Deposit"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(220, 34)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(85, 13)
        Me.Label7.TabIndex = 384
        Me.Label7.Text = "Bank Guarantee"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(52, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 118
        Me.Label1.Text = "Invoice"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(52, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 120
        Me.Label2.Text = "Instalments"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(168, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 122
        Me.Label3.Text = "Pay Now"
        '
        'chkRecoveryInstalmentsEnabled
        '
        Me.chkRecoveryInstalmentsEnabled.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecoveryInstalmentsEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRecoveryInstalmentsEnabled.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecoveryInstalmentsEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecoveryInstalmentsEnabled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecoveryInstalmentsEnabled.Location = New System.Drawing.Point(11, 83)
        Me.chkRecoveryInstalmentsEnabled.Name = "chkRecoveryInstalmentsEnabled"
        Me.chkRecoveryInstalmentsEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecoveryInstalmentsEnabled.Size = New System.Drawing.Size(279, 20)
        Me.chkRecoveryInstalmentsEnabled.TabIndex = 398
        Me.chkRecoveryInstalmentsEnabled.Text = "Recovery Receipts on Instalments"
        Me.chkRecoveryInstalmentsEnabled.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmRenewals)
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmLeadAgentCommission)
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmSubAgentCommission)
        Me._tabMainTab_TabPage3.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4-True Monthly Policies"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'frmRenewals
        '
        Me.frmRenewals.BackColor = System.Drawing.SystemColors.Control
        Me.frmRenewals.Controls.Add(Me.chkEditAnnivDate)
        Me.frmRenewals.Controls.Add(Me.lblAnnivDateEditableMonthlyPolicy)
        Me.frmRenewals.Controls.Add(Me.chkAutoRenBDMonthlyPol)
        Me.frmRenewals.Controls.Add(Me.lblAutoRenBDMonthlyPolicy)
        Me.frmRenewals.Controls.Add(Me.chkUnifiedRenewalDateIsReadOnly)
        Me.frmRenewals.Controls.Add(Me.lblUnifiedRenewalDateIsReadOnly)
        Me.frmRenewals.Controls.Add(Me.chkTMPAutoRenFAC)
        Me.frmRenewals.Controls.Add(Me.lblMonthlyAutoRenWithFac)
        Me.frmRenewals.Controls.Add(Me.txtUnifiedRenewalDay)
        Me.frmRenewals.Controls.Add(Me.txtAnniversaryRenewalWeeks)
        Me.frmRenewals.Controls.Add(Me.lblUnifiedRenewalDay)
        Me.frmRenewals.Controls.Add(Me.lblAnniversaryRenewalWeeks)
        Me.frmRenewals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmRenewals.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmRenewals.Location = New System.Drawing.Point(8, 16)
        Me.frmRenewals.Name = "frmRenewals"
        Me.frmRenewals.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmRenewals.Size = New System.Drawing.Size(400, 192)
        Me.frmRenewals.TabIndex = 97
        Me.frmRenewals.TabStop = False
        Me.frmRenewals.Text = "Renewals"
        '
        'chkEditAnnivDate
        '
        Me.chkEditAnnivDate.AutoSize = True
        Me.chkEditAnnivDate.Location = New System.Drawing.Point(295, 165)
        Me.chkEditAnnivDate.Name = "chkEditAnnivDate"
        Me.chkEditAnnivDate.Size = New System.Drawing.Size(15, 14)
        Me.chkEditAnnivDate.TabIndex = 108
        Me.chkEditAnnivDate.UseVisualStyleBackColor = True
        '
        'lblAnnivDateEditableMonthlyPolicy
        '
        Me.lblAnnivDateEditableMonthlyPolicy.Location = New System.Drawing.Point(9, 165)
        Me.lblAnnivDateEditableMonthlyPolicy.Name = "lblAnnivDateEditableMonthlyPolicy"
        Me.lblAnnivDateEditableMonthlyPolicy.Size = New System.Drawing.Size(190, 13)
        Me.lblAnnivDateEditableMonthlyPolicy.TabIndex = 107
        Me.lblAnnivDateEditableMonthlyPolicy.Text = "Anniversary Date Editable"
        '
        'chkAutoRenBDMonthlyPol
        '
        Me.chkAutoRenBDMonthlyPol.AutoSize = True
        Me.chkAutoRenBDMonthlyPol.Location = New System.Drawing.Point(295, 145)
        Me.chkAutoRenBDMonthlyPol.Name = "chkAutoRenBDMonthlyPol"
        Me.chkAutoRenBDMonthlyPol.Size = New System.Drawing.Size(15, 14)
        Me.chkAutoRenBDMonthlyPol.TabIndex = 106
        Me.chkAutoRenBDMonthlyPol.UseVisualStyleBackColor = True
        '
        'lblAutoRenBDMonthlyPolicy
        '
        Me.lblAutoRenBDMonthlyPolicy.Location = New System.Drawing.Point(9, 145)
        Me.lblAutoRenBDMonthlyPolicy.Name = "lblAutoRenBDMonthlyPolicy"
        Me.lblAutoRenBDMonthlyPolicy.Size = New System.Drawing.Size(190, 13)
        Me.lblAutoRenBDMonthlyPolicy.TabIndex = 100
        Me.lblAutoRenBDMonthlyPolicy.Text = "Auto renew backdated monthly policy"
        '
        'chkUnifiedRenewalDateIsReadOnly
        '
        Me.chkUnifiedRenewalDateIsReadOnly.AutoSize = True
        Me.chkUnifiedRenewalDateIsReadOnly.Location = New System.Drawing.Point(295, 82)
        Me.chkUnifiedRenewalDateIsReadOnly.Name = "chkUnifiedRenewalDateIsReadOnly"
        Me.chkUnifiedRenewalDateIsReadOnly.Size = New System.Drawing.Size(15, 14)
        Me.chkUnifiedRenewalDateIsReadOnly.TabIndex = 105
        Me.chkUnifiedRenewalDateIsReadOnly.UseVisualStyleBackColor = True
        '
        'lblUnifiedRenewalDateIsReadOnly
        '
        Me.lblUnifiedRenewalDateIsReadOnly.AutoSize = True
        Me.lblUnifiedRenewalDateIsReadOnly.Location = New System.Drawing.Point(8, 84)
        Me.lblUnifiedRenewalDateIsReadOnly.Name = "lblUnifiedRenewalDateIsReadOnly"
        Me.lblUnifiedRenewalDateIsReadOnly.Size = New System.Drawing.Size(174, 13)
        Me.lblUnifiedRenewalDateIsReadOnly.TabIndex = 104
        Me.lblUnifiedRenewalDateIsReadOnly.Text = "Unified Renewal Date is Read Only"
        '
        'chkTMPAutoRenFAC
        '
        Me.chkTMPAutoRenFAC.AutoSize = True
        Me.chkTMPAutoRenFAC.Location = New System.Drawing.Point(295, 110)
        Me.chkTMPAutoRenFAC.Name = "chkTMPAutoRenFAC"
        Me.chkTMPAutoRenFAC.Size = New System.Drawing.Size(15, 14)
        Me.chkTMPAutoRenFAC.TabIndex = 103
        Me.chkTMPAutoRenFAC.UseVisualStyleBackColor = True
        '
        'lblMonthlyAutoRenWithFac
        '
        Me.lblMonthlyAutoRenWithFac.AutoSize = True
        Me.lblMonthlyAutoRenWithFac.Location = New System.Drawing.Point(9, 113)
        Me.lblMonthlyAutoRenWithFac.Name = "lblMonthlyAutoRenWithFac"
        Me.lblMonthlyAutoRenWithFac.Size = New System.Drawing.Size(165, 13)
        Me.lblMonthlyAutoRenWithFac.TabIndex = 102
        Me.lblMonthlyAutoRenWithFac.Text = "Monthly Auto Renewal with FAC :"
        '
        'txtUnifiedRenewalDay
        '
        Me.txtUnifiedRenewalDay.AcceptsReturn = True
        Me.txtUnifiedRenewalDay.BackColor = System.Drawing.SystemColors.Window
        Me.txtUnifiedRenewalDay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUnifiedRenewalDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUnifiedRenewalDay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUnifiedRenewalDay.Location = New System.Drawing.Point(294, 52)
        Me.txtUnifiedRenewalDay.MaxLength = 0
        Me.txtUnifiedRenewalDay.Name = "txtUnifiedRenewalDay"
        Me.txtUnifiedRenewalDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUnifiedRenewalDay.Size = New System.Drawing.Size(83, 20)
        Me.txtUnifiedRenewalDay.TabIndex = 101
        '
        'txtAnniversaryRenewalWeeks
        '
        Me.txtAnniversaryRenewalWeeks.AcceptsReturn = True
        Me.txtAnniversaryRenewalWeeks.BackColor = System.Drawing.SystemColors.Window
        Me.txtAnniversaryRenewalWeeks.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAnniversaryRenewalWeeks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAnniversaryRenewalWeeks.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAnniversaryRenewalWeeks.Location = New System.Drawing.Point(294, 24)
        Me.txtAnniversaryRenewalWeeks.MaxLength = 0
        Me.txtAnniversaryRenewalWeeks.Name = "txtAnniversaryRenewalWeeks"
        Me.txtAnniversaryRenewalWeeks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAnniversaryRenewalWeeks.Size = New System.Drawing.Size(83, 20)
        Me.txtAnniversaryRenewalWeeks.TabIndex = 99
        '
        'lblUnifiedRenewalDay
        '
        Me.lblUnifiedRenewalDay.AutoSize = True
        Me.lblUnifiedRenewalDay.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnifiedRenewalDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnifiedRenewalDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnifiedRenewalDay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnifiedRenewalDay.Location = New System.Drawing.Point(8, 54)
        Me.lblUnifiedRenewalDay.Name = "lblUnifiedRenewalDay"
        Me.lblUnifiedRenewalDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnifiedRenewalDay.Size = New System.Drawing.Size(110, 13)
        Me.lblUnifiedRenewalDay.TabIndex = 100
        Me.lblUnifiedRenewalDay.Text = "Unified Renewal Day:"
        '
        'lblAnniversaryRenewalWeeks
        '
        Me.lblAnniversaryRenewalWeeks.AutoSize = True
        Me.lblAnniversaryRenewalWeeks.BackColor = System.Drawing.SystemColors.Control
        Me.lblAnniversaryRenewalWeeks.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAnniversaryRenewalWeeks.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAnniversaryRenewalWeeks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAnniversaryRenewalWeeks.Location = New System.Drawing.Point(8, 24)
        Me.lblAnniversaryRenewalWeeks.Name = "lblAnniversaryRenewalWeeks"
        Me.lblAnniversaryRenewalWeeks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAnniversaryRenewalWeeks.Size = New System.Drawing.Size(196, 13)
        Me.lblAnniversaryRenewalWeeks.TabIndex = 98
        Me.lblAnniversaryRenewalWeeks.Text = "Anniversary Renewal Weeks:"
        '
        'frmLeadAgentCommission
        '
        Me.frmLeadAgentCommission.BackColor = System.Drawing.SystemColors.Control
        Me.frmLeadAgentCommission.Controls.Add(Me.actSuspenseAcc)
        Me.frmLeadAgentCommission.Controls.Add(Me.cboMonthInCycleLA)
        Me.frmLeadAgentCommission.Controls.Add(Me.chkAllowConsolidateCommissionLA)
        Me.frmLeadAgentCommission.Controls.Add(Me.lblLeadAgentCommSuspenseLA)
        Me.frmLeadAgentCommission.Controls.Add(Me.lblMonthInCycleLA)
        Me.frmLeadAgentCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmLeadAgentCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmLeadAgentCommission.Location = New System.Drawing.Point(421, 14)
        Me.frmLeadAgentCommission.Name = "frmLeadAgentCommission"
        Me.frmLeadAgentCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmLeadAgentCommission.Size = New System.Drawing.Size(402, 113)
        Me.frmLeadAgentCommission.TabIndex = 102
        Me.frmLeadAgentCommission.TabStop = False
        Me.frmLeadAgentCommission.Text = "Lead Agent Commission"
        '
        'actSuspenseAcc
        '
        Me.actSuspenseAcc.AccountId = 0
        Me.actSuspenseAcc.AllowStoppedAccounts = False
        Me.actSuspenseAcc.BackStyle = 0
        Me.actSuspenseAcc.CompanyId = 0
        Me.actSuspenseAcc.Default_Renamed = False
        Me.actSuspenseAcc.Location = New System.Drawing.Point(232, 80)
        Me.actSuspenseAcc.LookupCaption = "..."
        Me.actSuspenseAcc.LookupHeight = 285
        Me.actSuspenseAcc.LookupLeft = 2070
        Me.actSuspenseAcc.LookupTextLeft = 0
        Me.actSuspenseAcc.LookupTextWidth = 2070
        Me.actSuspenseAcc.LookupWidth = 360
        Me.actSuspenseAcc.Name = "actSuspenseAcc"
        Me.actSuspenseAcc.OnlyUpdatableAccounts = False
        Me.actSuspenseAcc.SelLength = 0
        Me.actSuspenseAcc.SelStart = 0
        Me.actSuspenseAcc.SelText = ""
        Me.actSuspenseAcc.ShowEditOnFindAccount = False
        Me.actSuspenseAcc.Size = New System.Drawing.Size(162, 19)
        Me.actSuspenseAcc.TabIndex = 107
        Me.actSuspenseAcc.ToolTipText = ""
        Me.actSuspenseAcc.Visible = False
        '
        'chkAllowConsolidateCommissionLA
        '
        Me.chkAllowConsolidateCommissionLA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowConsolidateCommissionLA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowConsolidateCommissionLA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowConsolidateCommissionLA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowConsolidateCommissionLA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowConsolidateCommissionLA.Location = New System.Drawing.Point(8, 24)
        Me.chkAllowConsolidateCommissionLA.Name = "chkAllowConsolidateCommissionLA"
        Me.chkAllowConsolidateCommissionLA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowConsolidateCommissionLA.Size = New System.Drawing.Size(237, 17)
        Me.chkAllowConsolidateCommissionLA.TabIndex = 103
        Me.chkAllowConsolidateCommissionLA.Text = "Allow Consolidate Commission:"
        Me.chkAllowConsolidateCommissionLA.UseVisualStyleBackColor = False
        '
        'lblLeadAgentCommSuspenseLA
        '
        Me.lblLeadAgentCommSuspenseLA.BackColor = System.Drawing.SystemColors.Control
        Me.lblLeadAgentCommSuspenseLA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLeadAgentCommSuspenseLA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeadAgentCommSuspenseLA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLeadAgentCommSuspenseLA.Location = New System.Drawing.Point(8, 80)
        Me.lblLeadAgentCommSuspenseLA.Name = "lblLeadAgentCommSuspenseLA"
        Me.lblLeadAgentCommSuspenseLA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLeadAgentCommSuspenseLA.Size = New System.Drawing.Size(225, 17)
        Me.lblLeadAgentCommSuspenseLA.TabIndex = 106
        Me.lblLeadAgentCommSuspenseLA.Text = "Lead Agent Commission Suspense a/c:"
        Me.lblLeadAgentCommSuspenseLA.Visible = False
        '
        'lblMonthInCycleLA
        '
        Me.lblMonthInCycleLA.BackColor = System.Drawing.SystemColors.Control
        Me.lblMonthInCycleLA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMonthInCycleLA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonthInCycleLA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonthInCycleLA.Location = New System.Drawing.Point(8, 52)
        Me.lblMonthInCycleLA.Name = "lblMonthInCycleLA"
        Me.lblMonthInCycleLA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMonthInCycleLA.Size = New System.Drawing.Size(105, 17)
        Me.lblMonthInCycleLA.TabIndex = 104
        Me.lblMonthInCycleLA.Text = "Month in Cycle:"
        Me.lblMonthInCycleLA.Visible = False
        '
        'frmSubAgentCommission
        '
        Me.frmSubAgentCommission.BackColor = System.Drawing.SystemColors.Control
        Me.frmSubAgentCommission.Controls.Add(Me.actSuspenseAcc1)
        Me.frmSubAgentCommission.Controls.Add(Me.cboMonthInCycleSA)
        Me.frmSubAgentCommission.Controls.Add(Me.chkAllowConsolidateCommissionSA)
        Me.frmSubAgentCommission.Controls.Add(Me.lblSubAgentCommSuspense)
        Me.frmSubAgentCommission.Controls.Add(Me.lblMonthInCycleSA)
        Me.frmSubAgentCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmSubAgentCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmSubAgentCommission.Location = New System.Drawing.Point(423, 132)
        Me.frmSubAgentCommission.Name = "frmSubAgentCommission"
        Me.frmSubAgentCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmSubAgentCommission.Size = New System.Drawing.Size(399, 105)
        Me.frmSubAgentCommission.TabIndex = 111
        Me.frmSubAgentCommission.TabStop = False
        Me.frmSubAgentCommission.Text = "Sub-Agent Commission"
        '
        'actSuspenseAcc1
        '
        Me.actSuspenseAcc1.AccountId = 0
        Me.actSuspenseAcc1.AllowStoppedAccounts = False
        Me.actSuspenseAcc1.BackStyle = 0
        Me.actSuspenseAcc1.CompanyId = 0
        Me.actSuspenseAcc1.Default_Renamed = False
        Me.actSuspenseAcc1.Location = New System.Drawing.Point(232, 79)
        Me.actSuspenseAcc1.LookupCaption = "..."
        Me.actSuspenseAcc1.LookupHeight = 285
        Me.actSuspenseAcc1.LookupLeft = 2025
        Me.actSuspenseAcc1.LookupTextLeft = 0
        Me.actSuspenseAcc1.LookupTextWidth = 2025
        Me.actSuspenseAcc1.LookupWidth = 360
        Me.actSuspenseAcc1.Name = "actSuspenseAcc1"
        Me.actSuspenseAcc1.OnlyUpdatableAccounts = False
        Me.actSuspenseAcc1.SelLength = 0
        Me.actSuspenseAcc1.SelStart = 0
        Me.actSuspenseAcc1.SelText = ""
        Me.actSuspenseAcc1.ShowEditOnFindAccount = False
        Me.actSuspenseAcc1.Size = New System.Drawing.Size(159, 19)
        Me.actSuspenseAcc1.TabIndex = 116
        Me.actSuspenseAcc1.ToolTipText = ""
        Me.actSuspenseAcc1.Visible = False
        '
        'chkAllowConsolidateCommissionSA
        '
        Me.chkAllowConsolidateCommissionSA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowConsolidateCommissionSA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAllowConsolidateCommissionSA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowConsolidateCommissionSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowConsolidateCommissionSA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowConsolidateCommissionSA.Location = New System.Drawing.Point(7, 24)
        Me.chkAllowConsolidateCommissionSA.Name = "chkAllowConsolidateCommissionSA"
        Me.chkAllowConsolidateCommissionSA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowConsolidateCommissionSA.Size = New System.Drawing.Size(237, 17)
        Me.chkAllowConsolidateCommissionSA.TabIndex = 112
        Me.chkAllowConsolidateCommissionSA.Text = "Allow Consolidate Commission:"
        Me.chkAllowConsolidateCommissionSA.UseVisualStyleBackColor = False
        '
        'lblSubAgentCommSuspense
        '
        Me.lblSubAgentCommSuspense.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubAgentCommSuspense.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubAgentCommSuspense.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubAgentCommSuspense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubAgentCommSuspense.Location = New System.Drawing.Point(8, 80)
        Me.lblSubAgentCommSuspense.Name = "lblSubAgentCommSuspense"
        Me.lblSubAgentCommSuspense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubAgentCommSuspense.Size = New System.Drawing.Size(225, 17)
        Me.lblSubAgentCommSuspense.TabIndex = 115
        Me.lblSubAgentCommSuspense.Text = "Sub-Agent Commission Suspense a/c:"
        Me.lblSubAgentCommSuspense.Visible = False
        '
        'lblMonthInCycleSA
        '
        Me.lblMonthInCycleSA.BackColor = System.Drawing.SystemColors.Control
        Me.lblMonthInCycleSA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMonthInCycleSA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonthInCycleSA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonthInCycleSA.Location = New System.Drawing.Point(8, 48)
        Me.lblMonthInCycleSA.Name = "lblMonthInCycleSA"
        Me.lblMonthInCycleSA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMonthInCycleSA.Size = New System.Drawing.Size(177, 17)
        Me.lblMonthInCycleSA.TabIndex = 113
        Me.lblMonthInCycleSA.Text = "Month in Cycle:"
        Me.lblMonthInCycleSA.Visible = False
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.OptInstalments)
        Me.Frame3.Controls.Add(Me.OptInvoice)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 214)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(397, 65)
        Me.Frame3.TabIndex = 108
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "Payment Method"
        '
        'OptInstalments
        '
        Me.OptInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.OptInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptInstalments.Location = New System.Drawing.Point(144, 24)
        Me.OptInstalments.Name = "OptInstalments"
        Me.OptInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptInstalments.Size = New System.Drawing.Size(89, 25)
        Me.OptInstalments.TabIndex = 110
        Me.OptInstalments.TabStop = True
        Me.OptInstalments.Text = "Instalments"
        Me.OptInstalments.UseVisualStyleBackColor = False
        '
        'OptInvoice
        '
        Me.OptInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.OptInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptInvoice.Location = New System.Drawing.Point(16, 24)
        Me.OptInvoice.Name = "OptInvoice"
        Me.OptInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptInvoice.Size = New System.Drawing.Size(73, 25)
        Me.OptInvoice.TabIndex = 109
        Me.OptInvoice.TabStop = True
        Me.OptInvoice.Text = "Invoice"
        Me.OptInvoice.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraOpenClaim)
        Me._tabMainTab_TabPage2.Controls.Add(Me.Frame5)
        Me._tabMainTab_TabPage2.Controls.Add(Me.Frame6)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(842, 674)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3-Claims Workflow"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        'fraOpenClaim
        '
        Me.fraOpenClaim.BackColor = System.Drawing.SystemColors.Control
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_25)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_24)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_23)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_22)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_21)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_20)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_19)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_18)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_17)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_16)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_15)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_14)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_13)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_12)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_11)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_10)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_9)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_8)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_7)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_6)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_5)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_4)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_3)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_2)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_1)
        Me.fraOpenClaim.Controls.Add(Me._chkOpenClaimWorkflow_0)
        Me.fraOpenClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOpenClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOpenClaim.Location = New System.Drawing.Point(10, 9)
        Me.fraOpenClaim.Name = "fraOpenClaim"
        Me.fraOpenClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOpenClaim.Size = New System.Drawing.Size(266, 528)
        Me.fraOpenClaim.TabIndex = 294
        Me.fraOpenClaim.TabStop = False
        Me.fraOpenClaim.Text = "Open Claim"
        '
        '_chkOpenClaimWorkflow_25
        '
        Me._chkOpenClaimWorkflow_25.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_25.Checked = True
        Me._chkOpenClaimWorkflow_25.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_25.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_25.Enabled = False
        Me._chkOpenClaimWorkflow_25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_25.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_25.Location = New System.Drawing.Point(6, 430)
        Me._chkOpenClaimWorkflow_25.Name = "_chkOpenClaimWorkflow_25"
        Me._chkOpenClaimWorkflow_25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_25.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_25.TabIndex = 376
        Me._chkOpenClaimWorkflow_25.Text = "Close Claim Message"
        Me._chkOpenClaimWorkflow_25.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_24
        '
        Me._chkOpenClaimWorkflow_24.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_24.Checked = True
        Me._chkOpenClaimWorkflow_24.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_24.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_24.Enabled = False
        Me._chkOpenClaimWorkflow_24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_24.Location = New System.Drawing.Point(6, 511)
        Me._chkOpenClaimWorkflow_24.Name = "_chkOpenClaimWorkflow_24"
        Me._chkOpenClaimWorkflow_24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_24.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_24.TabIndex = 319
        Me._chkOpenClaimWorkflow_24.Text = "Unlock Claim"
        Me._chkOpenClaimWorkflow_24.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_23
        '
        Me._chkOpenClaimWorkflow_23.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_23.Location = New System.Drawing.Point(6, 494)
        Me._chkOpenClaimWorkflow_23.Name = "_chkOpenClaimWorkflow_23"
        Me._chkOpenClaimWorkflow_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_23.Size = New System.Drawing.Size(255, 23)
        Me._chkOpenClaimWorkflow_23.TabIndex = 318
        Me._chkOpenClaimWorkflow_23.Text = "Do you wish to make further payments?"
        Me._chkOpenClaimWorkflow_23.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_22
        '
        Me._chkOpenClaimWorkflow_22.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_22.Location = New System.Drawing.Point(6, 471)
        Me._chkOpenClaimWorkflow_22.Name = "_chkOpenClaimWorkflow_22"
        Me._chkOpenClaimWorkflow_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_22.Size = New System.Drawing.Size(255, 20)
        Me._chkOpenClaimWorkflow_22.TabIndex = 317
        Me._chkOpenClaimWorkflow_22.Text = "Produce Claim Payment Documents"
        Me._chkOpenClaimWorkflow_22.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_21
        '
        Me._chkOpenClaimWorkflow_21.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_21.Location = New System.Drawing.Point(6, 446)
        Me._chkOpenClaimWorkflow_21.Name = "_chkOpenClaimWorkflow_21"
        Me._chkOpenClaimWorkflow_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_21.Size = New System.Drawing.Size(255, 31)
        Me._chkOpenClaimWorkflow_21.TabIndex = 316
        Me._chkOpenClaimWorkflow_21.Text = "Display Generate Claim Payment Documents Message"
        Me._chkOpenClaimWorkflow_21.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_20
        '
        Me._chkOpenClaimWorkflow_20.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_20.Checked = True
        Me._chkOpenClaimWorkflow_20.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_20.Enabled = False
        Me._chkOpenClaimWorkflow_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_20.Location = New System.Drawing.Point(6, 411)
        Me._chkOpenClaimWorkflow_20.Name = "_chkOpenClaimWorkflow_20"
        Me._chkOpenClaimWorkflow_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_20.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_20.TabIndex = 315
        Me._chkOpenClaimWorkflow_20.Text = "Check Status"
        Me._chkOpenClaimWorkflow_20.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_19
        '
        Me._chkOpenClaimWorkflow_19.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_19.Location = New System.Drawing.Point(6, 392)
        Me._chkOpenClaimWorkflow_19.Name = "_chkOpenClaimWorkflow_19"
        Me._chkOpenClaimWorkflow_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_19.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_19.TabIndex = 314
        Me._chkOpenClaimWorkflow_19.Text = "Cash Payments Process"
        Me._chkOpenClaimWorkflow_19.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_18
        '
        Me._chkOpenClaimWorkflow_18.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_18.Checked = True
        Me._chkOpenClaimWorkflow_18.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_18.Enabled = False
        Me._chkOpenClaimWorkflow_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_18.Location = New System.Drawing.Point(6, 374)
        Me._chkOpenClaimWorkflow_18.Name = "_chkOpenClaimWorkflow_18"
        Me._chkOpenClaimWorkflow_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_18.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_18.TabIndex = 313
        Me._chkOpenClaimWorkflow_18.Text = "Update Claim Details"
        Me._chkOpenClaimWorkflow_18.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_17
        '
        Me._chkOpenClaimWorkflow_17.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_17.Location = New System.Drawing.Point(6, 355)
        Me._chkOpenClaimWorkflow_17.Name = "_chkOpenClaimWorkflow_17"
        Me._chkOpenClaimWorkflow_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_17.Size = New System.Drawing.Size(255, 23)
        Me._chkOpenClaimWorkflow_17.TabIndex = 312
        Me._chkOpenClaimWorkflow_17.Text = "Enter Description for Change"
        Me._chkOpenClaimWorkflow_17.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_16
        '
        Me._chkOpenClaimWorkflow_16.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_16.Location = New System.Drawing.Point(6, 336)
        Me._chkOpenClaimWorkflow_16.Name = "_chkOpenClaimWorkflow_16"
        Me._chkOpenClaimWorkflow_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_16.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_16.TabIndex = 311
        Me._chkOpenClaimWorkflow_16.Text = "Reinsurance Payment"
        Me._chkOpenClaimWorkflow_16.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_15
        '
        Me._chkOpenClaimWorkflow_15.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_15.Checked = True
        Me._chkOpenClaimWorkflow_15.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_15.Enabled = False
        Me._chkOpenClaimWorkflow_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_15.Location = New System.Drawing.Point(6, 318)
        Me._chkOpenClaimWorkflow_15.Name = "_chkOpenClaimWorkflow_15"
        Me._chkOpenClaimWorkflow_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_15.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_15.TabIndex = 310
        Me._chkOpenClaimWorkflow_15.Text = "Coinsurance Payment"
        Me._chkOpenClaimWorkflow_15.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_14
        '
        Me._chkOpenClaimWorkflow_14.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_14.Checked = True
        Me._chkOpenClaimWorkflow_14.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_14.Enabled = False
        Me._chkOpenClaimWorkflow_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_14.Location = New System.Drawing.Point(6, 299)
        Me._chkOpenClaimWorkflow_14.Name = "_chkOpenClaimWorkflow_14"
        Me._chkOpenClaimWorkflow_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_14.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_14.TabIndex = 309
        Me._chkOpenClaimWorkflow_14.Text = "Risk Details"
        Me._chkOpenClaimWorkflow_14.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_13
        '
        Me._chkOpenClaimWorkflow_13.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_13.Location = New System.Drawing.Point(6, 280)
        Me._chkOpenClaimWorkflow_13.Name = "_chkOpenClaimWorkflow_13"
        Me._chkOpenClaimWorkflow_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_13.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_13.TabIndex = 308
        Me._chkOpenClaimWorkflow_13.Text = "Fast Track Claims Payments"
        Me._chkOpenClaimWorkflow_13.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_12
        '
        Me._chkOpenClaimWorkflow_12.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_12.Checked = True
        Me._chkOpenClaimWorkflow_12.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_12.Enabled = False
        Me._chkOpenClaimWorkflow_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_12.Location = New System.Drawing.Point(6, 262)
        Me._chkOpenClaimWorkflow_12.Name = "_chkOpenClaimWorkflow_12"
        Me._chkOpenClaimWorkflow_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_12.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_12.TabIndex = 307
        Me._chkOpenClaimWorkflow_12.Text = "Proceed to Claim Payments Message"
        Me._chkOpenClaimWorkflow_12.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_11
        '
        Me._chkOpenClaimWorkflow_11.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_11.Checked = True
        Me._chkOpenClaimWorkflow_11.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_11.Location = New System.Drawing.Point(6, 243)
        Me._chkOpenClaimWorkflow_11.Name = "_chkOpenClaimWorkflow_11"
        Me._chkOpenClaimWorkflow_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_11.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_11.TabIndex = 306
        Me._chkOpenClaimWorkflow_11.Text = "Claim Payments Process"
        Me._chkOpenClaimWorkflow_11.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_10
        '
        Me._chkOpenClaimWorkflow_10.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_10.Location = New System.Drawing.Point(6, 224)
        Me._chkOpenClaimWorkflow_10.Name = "_chkOpenClaimWorkflow_10"
        Me._chkOpenClaimWorkflow_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_10.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_10.TabIndex = 305
        Me._chkOpenClaimWorkflow_10.Text = "Produce Claim Notification Documents"
        Me._chkOpenClaimWorkflow_10.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_9
        '
        Me._chkOpenClaimWorkflow_9.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_9.Location = New System.Drawing.Point(6, 195)
        Me._chkOpenClaimWorkflow_9.Name = "_chkOpenClaimWorkflow_9"
        Me._chkOpenClaimWorkflow_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_9.Size = New System.Drawing.Size(255, 30)
        Me._chkOpenClaimWorkflow_9.TabIndex = 304
        Me._chkOpenClaimWorkflow_9.Text = "Display Generate Claim Notification Documents Message"
        Me._chkOpenClaimWorkflow_9.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_8
        '
        Me._chkOpenClaimWorkflow_8.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_8.Checked = True
        Me._chkOpenClaimWorkflow_8.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_8.Enabled = False
        Me._chkOpenClaimWorkflow_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_8.Location = New System.Drawing.Point(6, 176)
        Me._chkOpenClaimWorkflow_8.Name = "_chkOpenClaimWorkflow_8"
        Me._chkOpenClaimWorkflow_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_8.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_8.TabIndex = 303
        Me._chkOpenClaimWorkflow_8.Text = "Update Claim Details"
        Me._chkOpenClaimWorkflow_8.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_7
        '
        Me._chkOpenClaimWorkflow_7.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_7.Location = New System.Drawing.Point(6, 157)
        Me._chkOpenClaimWorkflow_7.Name = "_chkOpenClaimWorkflow_7"
        Me._chkOpenClaimWorkflow_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_7.Size = New System.Drawing.Size(255, 23)
        Me._chkOpenClaimWorkflow_7.TabIndex = 302
        Me._chkOpenClaimWorkflow_7.Text = "External Claim Handling"
        Me._chkOpenClaimWorkflow_7.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_6
        '
        Me._chkOpenClaimWorkflow_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_6.Location = New System.Drawing.Point(6, 137)
        Me._chkOpenClaimWorkflow_6.Name = "_chkOpenClaimWorkflow_6"
        Me._chkOpenClaimWorkflow_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_6.Size = New System.Drawing.Size(255, 23)
        Me._chkOpenClaimWorkflow_6.TabIndex = 301
        Me._chkOpenClaimWorkflow_6.Text = "Third Party Recovery"
        Me._chkOpenClaimWorkflow_6.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_5
        '
        Me._chkOpenClaimWorkflow_5.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_5.Location = New System.Drawing.Point(6, 118)
        Me._chkOpenClaimWorkflow_5.Name = "_chkOpenClaimWorkflow_5"
        Me._chkOpenClaimWorkflow_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_5.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_5.TabIndex = 300
        Me._chkOpenClaimWorkflow_5.Text = "Salvage Recovery"
        Me._chkOpenClaimWorkflow_5.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_4
        '
        Me._chkOpenClaimWorkflow_4.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_4.Location = New System.Drawing.Point(6, 98)
        Me._chkOpenClaimWorkflow_4.Name = "_chkOpenClaimWorkflow_4"
        Me._chkOpenClaimWorkflow_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_4.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_4.TabIndex = 299
        Me._chkOpenClaimWorkflow_4.Text = "Reinsurance Recoveries"
        Me._chkOpenClaimWorkflow_4.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_3
        '
        Me._chkOpenClaimWorkflow_3.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_3.Checked = True
        Me._chkOpenClaimWorkflow_3.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_3.Enabled = False
        Me._chkOpenClaimWorkflow_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_3.Location = New System.Drawing.Point(6, 79)
        Me._chkOpenClaimWorkflow_3.Name = "_chkOpenClaimWorkflow_3"
        Me._chkOpenClaimWorkflow_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_3.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_3.TabIndex = 298
        Me._chkOpenClaimWorkflow_3.Text = "Risk Details"
        Me._chkOpenClaimWorkflow_3.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_2
        '
        Me._chkOpenClaimWorkflow_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_2.Location = New System.Drawing.Point(6, 59)
        Me._chkOpenClaimWorkflow_2.Name = "_chkOpenClaimWorkflow_2"
        Me._chkOpenClaimWorkflow_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_2.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_2.TabIndex = 297
        Me._chkOpenClaimWorkflow_2.Text = "Check Unpaid Status"
        Me._chkOpenClaimWorkflow_2.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_1
        '
        Me._chkOpenClaimWorkflow_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_1.Checked = True
        Me._chkOpenClaimWorkflow_1.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_1.Enabled = False
        Me._chkOpenClaimWorkflow_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_1.Location = New System.Drawing.Point(6, 40)
        Me._chkOpenClaimWorkflow_1.Name = "_chkOpenClaimWorkflow_1"
        Me._chkOpenClaimWorkflow_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_1.Size = New System.Drawing.Size(255, 13)
        Me._chkOpenClaimWorkflow_1.TabIndex = 296
        Me._chkOpenClaimWorkflow_1.Text = "Claim Details"
        Me._chkOpenClaimWorkflow_1.UseVisualStyleBackColor = False
        '
        '_chkOpenClaimWorkflow_0
        '
        Me._chkOpenClaimWorkflow_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkOpenClaimWorkflow_0.Checked = True
        Me._chkOpenClaimWorkflow_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkOpenClaimWorkflow_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOpenClaimWorkflow_0.Enabled = False
        Me._chkOpenClaimWorkflow_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOpenClaimWorkflow_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOpenClaimWorkflow_0.Location = New System.Drawing.Point(6, 20)
        Me._chkOpenClaimWorkflow_0.Name = "_chkOpenClaimWorkflow_0"
        Me._chkOpenClaimWorkflow_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOpenClaimWorkflow_0.Size = New System.Drawing.Size(255, 17)
        Me._chkOpenClaimWorkflow_0.TabIndex = 295
        Me._chkOpenClaimWorkflow_0.Text = "Find Policy"
        Me._chkOpenClaimWorkflow_0.UseVisualStyleBackColor = False
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_16)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_15)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_14)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_13)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_12)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_11)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_10)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_9)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_8)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_7)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_6)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_5)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_4)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_3)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_2)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_1)
        Me.Frame5.Controls.Add(Me._chkPaymentClaimWorkflow_0)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(565, 9)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(264, 361)
        Me.Frame5.TabIndex = 347
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Claim Payment"
        '
        '_chkPaymentClaimWorkflow_16
        '
        Me._chkPaymentClaimWorkflow_16.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_16.Checked = True
        Me._chkPaymentClaimWorkflow_16.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_16.Enabled = False
        Me._chkPaymentClaimWorkflow_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_16.Location = New System.Drawing.Point(6, 252)
        Me._chkPaymentClaimWorkflow_16.Name = "_chkPaymentClaimWorkflow_16"
        Me._chkPaymentClaimWorkflow_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_16.Size = New System.Drawing.Size(255, 17)
        Me._chkPaymentClaimWorkflow_16.TabIndex = 378
        Me._chkPaymentClaimWorkflow_16.Text = "Close Claim Message"
        Me._chkPaymentClaimWorkflow_16.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_15
        '
        Me._chkPaymentClaimWorkflow_15.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_15.Checked = True
        Me._chkPaymentClaimWorkflow_15.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_15.Enabled = False
        Me._chkPaymentClaimWorkflow_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_15.Location = New System.Drawing.Point(6, 341)
        Me._chkPaymentClaimWorkflow_15.Name = "_chkPaymentClaimWorkflow_15"
        Me._chkPaymentClaimWorkflow_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_15.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_15.TabIndex = 363
        Me._chkPaymentClaimWorkflow_15.Text = "Unlock Claim"
        Me._chkPaymentClaimWorkflow_15.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_14
        '
        Me._chkPaymentClaimWorkflow_14.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_14.Location = New System.Drawing.Point(6, 321)
        Me._chkPaymentClaimWorkflow_14.Name = "_chkPaymentClaimWorkflow_14"
        Me._chkPaymentClaimWorkflow_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_14.Size = New System.Drawing.Size(255, 17)
        Me._chkPaymentClaimWorkflow_14.TabIndex = 362
        Me._chkPaymentClaimWorkflow_14.Text = "Do you wish to make further payments?"
        Me._chkPaymentClaimWorkflow_14.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_13
        '
        Me._chkPaymentClaimWorkflow_13.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_13.Location = New System.Drawing.Point(6, 300)
        Me._chkPaymentClaimWorkflow_13.Name = "_chkPaymentClaimWorkflow_13"
        Me._chkPaymentClaimWorkflow_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_13.Size = New System.Drawing.Size(255, 17)
        Me._chkPaymentClaimWorkflow_13.TabIndex = 361
        Me._chkPaymentClaimWorkflow_13.Text = "Produce Claim Payment Documents"
        Me._chkPaymentClaimWorkflow_13.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_12
        '
        Me._chkPaymentClaimWorkflow_12.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_12.Location = New System.Drawing.Point(6, 266)
        Me._chkPaymentClaimWorkflow_12.Name = "_chkPaymentClaimWorkflow_12"
        Me._chkPaymentClaimWorkflow_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_12.Size = New System.Drawing.Size(255, 32)
        Me._chkPaymentClaimWorkflow_12.TabIndex = 360
        Me._chkPaymentClaimWorkflow_12.Text = "Display Generate Claim Payment Documents Message"
        Me._chkPaymentClaimWorkflow_12.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_11
        '
        Me._chkPaymentClaimWorkflow_11.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_11.Checked = True
        Me._chkPaymentClaimWorkflow_11.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_11.Enabled = False
        Me._chkPaymentClaimWorkflow_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_11.Location = New System.Drawing.Point(6, 231)
        Me._chkPaymentClaimWorkflow_11.Name = "_chkPaymentClaimWorkflow_11"
        Me._chkPaymentClaimWorkflow_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_11.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_11.TabIndex = 359
        Me._chkPaymentClaimWorkflow_11.Text = "Check Status"
        Me._chkPaymentClaimWorkflow_11.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_10
        '
        Me._chkPaymentClaimWorkflow_10.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_10.Location = New System.Drawing.Point(6, 212)
        Me._chkPaymentClaimWorkflow_10.Name = "_chkPaymentClaimWorkflow_10"
        Me._chkPaymentClaimWorkflow_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_10.Size = New System.Drawing.Size(255, 23)
        Me._chkPaymentClaimWorkflow_10.TabIndex = 358
        Me._chkPaymentClaimWorkflow_10.Text = "Cash Payments Process"
        Me._chkPaymentClaimWorkflow_10.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_9
        '
        Me._chkPaymentClaimWorkflow_9.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_9.Checked = True
        Me._chkPaymentClaimWorkflow_9.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_9.Enabled = False
        Me._chkPaymentClaimWorkflow_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_9.Location = New System.Drawing.Point(6, 193)
        Me._chkPaymentClaimWorkflow_9.Name = "_chkPaymentClaimWorkflow_9"
        Me._chkPaymentClaimWorkflow_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_9.Size = New System.Drawing.Size(255, 17)
        Me._chkPaymentClaimWorkflow_9.TabIndex = 357
        Me._chkPaymentClaimWorkflow_9.Text = "Update Claim Details"
        Me._chkPaymentClaimWorkflow_9.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_8
        '
        Me._chkPaymentClaimWorkflow_8.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_8.Location = New System.Drawing.Point(6, 174)
        Me._chkPaymentClaimWorkflow_8.Name = "_chkPaymentClaimWorkflow_8"
        Me._chkPaymentClaimWorkflow_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_8.Size = New System.Drawing.Size(255, 23)
        Me._chkPaymentClaimWorkflow_8.TabIndex = 356
        Me._chkPaymentClaimWorkflow_8.Text = "Enter Description for Change"
        Me._chkPaymentClaimWorkflow_8.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_7
        '
        Me._chkPaymentClaimWorkflow_7.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_7.Location = New System.Drawing.Point(6, 154)
        Me._chkPaymentClaimWorkflow_7.Name = "_chkPaymentClaimWorkflow_7"
        Me._chkPaymentClaimWorkflow_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_7.Size = New System.Drawing.Size(255, 23)
        Me._chkPaymentClaimWorkflow_7.TabIndex = 355
        Me._chkPaymentClaimWorkflow_7.Text = "Reinsurance Payment"
        Me._chkPaymentClaimWorkflow_7.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_6
        '
        Me._chkPaymentClaimWorkflow_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_6.Checked = True
        Me._chkPaymentClaimWorkflow_6.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_6.Enabled = False
        Me._chkPaymentClaimWorkflow_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_6.Location = New System.Drawing.Point(6, 135)
        Me._chkPaymentClaimWorkflow_6.Name = "_chkPaymentClaimWorkflow_6"
        Me._chkPaymentClaimWorkflow_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_6.Size = New System.Drawing.Size(255, 17)
        Me._chkPaymentClaimWorkflow_6.TabIndex = 354
        Me._chkPaymentClaimWorkflow_6.Text = "Coinsurance Payment"
        Me._chkPaymentClaimWorkflow_6.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_5
        '
        Me._chkPaymentClaimWorkflow_5.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_5.Checked = True
        Me._chkPaymentClaimWorkflow_5.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_5.Enabled = False
        Me._chkPaymentClaimWorkflow_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_5.Location = New System.Drawing.Point(6, 116)
        Me._chkPaymentClaimWorkflow_5.Name = "_chkPaymentClaimWorkflow_5"
        Me._chkPaymentClaimWorkflow_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_5.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_5.TabIndex = 353
        Me._chkPaymentClaimWorkflow_5.Text = "Risk Details"
        Me._chkPaymentClaimWorkflow_5.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_4
        '
        Me._chkPaymentClaimWorkflow_4.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_4.Location = New System.Drawing.Point(6, 97)
        Me._chkPaymentClaimWorkflow_4.Name = "_chkPaymentClaimWorkflow_4"
        Me._chkPaymentClaimWorkflow_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_4.Size = New System.Drawing.Size(255, 23)
        Me._chkPaymentClaimWorkflow_4.TabIndex = 352
        Me._chkPaymentClaimWorkflow_4.Text = "Fast Track Claims Payments"
        Me._chkPaymentClaimWorkflow_4.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_3
        '
        Me._chkPaymentClaimWorkflow_3.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_3.Location = New System.Drawing.Point(6, 78)
        Me._chkPaymentClaimWorkflow_3.Name = "_chkPaymentClaimWorkflow_3"
        Me._chkPaymentClaimWorkflow_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_3.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_3.TabIndex = 351
        Me._chkPaymentClaimWorkflow_3.Text = "Check Deferred Reinsurance"
        Me._chkPaymentClaimWorkflow_3.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_2
        '
        Me._chkPaymentClaimWorkflow_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_2.Location = New System.Drawing.Point(6, 59)
        Me._chkPaymentClaimWorkflow_2.Name = "_chkPaymentClaimWorkflow_2"
        Me._chkPaymentClaimWorkflow_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_2.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_2.TabIndex = 350
        Me._chkPaymentClaimWorkflow_2.Text = "Check Unpaid Status"
        Me._chkPaymentClaimWorkflow_2.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_1
        '
        Me._chkPaymentClaimWorkflow_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_1.Checked = True
        Me._chkPaymentClaimWorkflow_1.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_1.Enabled = False
        Me._chkPaymentClaimWorkflow_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_1.Location = New System.Drawing.Point(6, 40)
        Me._chkPaymentClaimWorkflow_1.Name = "_chkPaymentClaimWorkflow_1"
        Me._chkPaymentClaimWorkflow_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_1.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_1.TabIndex = 349
        Me._chkPaymentClaimWorkflow_1.Text = "Claim Details"
        Me._chkPaymentClaimWorkflow_1.UseVisualStyleBackColor = False
        '
        '_chkPaymentClaimWorkflow_0
        '
        Me._chkPaymentClaimWorkflow_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkPaymentClaimWorkflow_0.Checked = True
        Me._chkPaymentClaimWorkflow_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkPaymentClaimWorkflow_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkPaymentClaimWorkflow_0.Enabled = False
        Me._chkPaymentClaimWorkflow_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkPaymentClaimWorkflow_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkPaymentClaimWorkflow_0.Location = New System.Drawing.Point(6, 20)
        Me._chkPaymentClaimWorkflow_0.Name = "_chkPaymentClaimWorkflow_0"
        Me._chkPaymentClaimWorkflow_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkPaymentClaimWorkflow_0.Size = New System.Drawing.Size(255, 13)
        Me._chkPaymentClaimWorkflow_0.TabIndex = 348
        Me._chkPaymentClaimWorkflow_0.Text = "Find Claim"
        Me._chkPaymentClaimWorkflow_0.UseVisualStyleBackColor = False
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_26)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_25)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_24)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_23)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_22)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_21)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_20)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_19)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_18)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_17)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_16)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_15)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_14)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_13)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_12)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_11)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_10)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_8)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_7)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_6)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_5)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_4)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_3)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_2)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_1)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_0)
        Me.Frame6.Controls.Add(Me._chkMaintainClaimWorkflow_9)
        Me.Frame6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(288, 9)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(264, 528)
        Me.Frame6.TabIndex = 320
        Me.Frame6.TabStop = False
        Me.Frame6.Text = "Maintain Claim"
        '
        '_chkMaintainClaimWorkflow_26
        '
        Me._chkMaintainClaimWorkflow_26.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_26.Checked = True
        Me._chkMaintainClaimWorkflow_26.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_26.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_26.Enabled = False
        Me._chkMaintainClaimWorkflow_26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_26.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_26.Location = New System.Drawing.Point(6, 412)
        Me._chkMaintainClaimWorkflow_26.Name = "_chkMaintainClaimWorkflow_26"
        Me._chkMaintainClaimWorkflow_26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_26.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_26.TabIndex = 377
        Me._chkMaintainClaimWorkflow_26.Text = "Close Claim Message"
        Me._chkMaintainClaimWorkflow_26.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_25
        '
        Me._chkMaintainClaimWorkflow_25.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_25.Checked = True
        Me._chkMaintainClaimWorkflow_25.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_25.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_25.Enabled = False
        Me._chkMaintainClaimWorkflow_25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_25.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_25.Location = New System.Drawing.Point(6, 511)
        Me._chkMaintainClaimWorkflow_25.Name = "_chkMaintainClaimWorkflow_25"
        Me._chkMaintainClaimWorkflow_25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_25.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_25.TabIndex = 346
        Me._chkMaintainClaimWorkflow_25.Text = "Unlock Claim"
        Me._chkMaintainClaimWorkflow_25.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_24
        '
        Me._chkMaintainClaimWorkflow_24.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_24.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_24.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_24.Location = New System.Drawing.Point(6, 494)
        Me._chkMaintainClaimWorkflow_24.Name = "_chkMaintainClaimWorkflow_24"
        Me._chkMaintainClaimWorkflow_24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_24.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_24.TabIndex = 345
        Me._chkMaintainClaimWorkflow_24.Text = "Do you wish to make further payments?"
        Me._chkMaintainClaimWorkflow_24.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_23
        '
        Me._chkMaintainClaimWorkflow_23.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_23.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_23.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_23.Location = New System.Drawing.Point(6, 475)
        Me._chkMaintainClaimWorkflow_23.Name = "_chkMaintainClaimWorkflow_23"
        Me._chkMaintainClaimWorkflow_23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_23.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_23.TabIndex = 344
        Me._chkMaintainClaimWorkflow_23.Text = "Produce Claim Payment Documents"
        Me._chkMaintainClaimWorkflow_23.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_22
        '
        Me._chkMaintainClaimWorkflow_22.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_22.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_22.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_22.Location = New System.Drawing.Point(6, 446)
        Me._chkMaintainClaimWorkflow_22.Name = "_chkMaintainClaimWorkflow_22"
        Me._chkMaintainClaimWorkflow_22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_22.Size = New System.Drawing.Size(255, 30)
        Me._chkMaintainClaimWorkflow_22.TabIndex = 343
        Me._chkMaintainClaimWorkflow_22.Text = "Display Generate Claim Payment Documents Message"
        Me._chkMaintainClaimWorkflow_22.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_21
        '
        Me._chkMaintainClaimWorkflow_21.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_21.Checked = True
        Me._chkMaintainClaimWorkflow_21.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_21.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_21.Enabled = False
        Me._chkMaintainClaimWorkflow_21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_21.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_21.Location = New System.Drawing.Point(6, 430)
        Me._chkMaintainClaimWorkflow_21.Name = "_chkMaintainClaimWorkflow_21"
        Me._chkMaintainClaimWorkflow_21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_21.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_21.TabIndex = 342
        Me._chkMaintainClaimWorkflow_21.Text = "Check Status"
        Me._chkMaintainClaimWorkflow_21.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_20
        '
        Me._chkMaintainClaimWorkflow_20.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_20.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_20.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_20.Location = New System.Drawing.Point(6, 395)
        Me._chkMaintainClaimWorkflow_20.Name = "_chkMaintainClaimWorkflow_20"
        Me._chkMaintainClaimWorkflow_20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_20.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_20.TabIndex = 341
        Me._chkMaintainClaimWorkflow_20.Text = "Cash Payments Process"
        Me._chkMaintainClaimWorkflow_20.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_19
        '
        Me._chkMaintainClaimWorkflow_19.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_19.Checked = True
        Me._chkMaintainClaimWorkflow_19.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_19.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_19.Enabled = False
        Me._chkMaintainClaimWorkflow_19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_19.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_19.Location = New System.Drawing.Point(6, 378)
        Me._chkMaintainClaimWorkflow_19.Name = "_chkMaintainClaimWorkflow_19"
        Me._chkMaintainClaimWorkflow_19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_19.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_19.TabIndex = 340
        Me._chkMaintainClaimWorkflow_19.Text = "Update Claim Details"
        Me._chkMaintainClaimWorkflow_19.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_18
        '
        Me._chkMaintainClaimWorkflow_18.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_18.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_18.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_18.Location = New System.Drawing.Point(6, 361)
        Me._chkMaintainClaimWorkflow_18.Name = "_chkMaintainClaimWorkflow_18"
        Me._chkMaintainClaimWorkflow_18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_18.Size = New System.Drawing.Size(255, 23)
        Me._chkMaintainClaimWorkflow_18.TabIndex = 339
        Me._chkMaintainClaimWorkflow_18.Text = "Enter Description for Change"
        Me._chkMaintainClaimWorkflow_18.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_17
        '
        Me._chkMaintainClaimWorkflow_17.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_17.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_17.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_17.Location = New System.Drawing.Point(6, 344)
        Me._chkMaintainClaimWorkflow_17.Name = "_chkMaintainClaimWorkflow_17"
        Me._chkMaintainClaimWorkflow_17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_17.Size = New System.Drawing.Size(255, 23)
        Me._chkMaintainClaimWorkflow_17.TabIndex = 338
        Me._chkMaintainClaimWorkflow_17.Text = "Reinsurance Payment"
        Me._chkMaintainClaimWorkflow_17.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_16
        '
        Me._chkMaintainClaimWorkflow_16.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_16.Checked = True
        Me._chkMaintainClaimWorkflow_16.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_16.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_16.Enabled = False
        Me._chkMaintainClaimWorkflow_16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_16.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_16.Location = New System.Drawing.Point(6, 327)
        Me._chkMaintainClaimWorkflow_16.Name = "_chkMaintainClaimWorkflow_16"
        Me._chkMaintainClaimWorkflow_16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_16.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_16.TabIndex = 337
        Me._chkMaintainClaimWorkflow_16.Text = "Coinsurance Payment"
        Me._chkMaintainClaimWorkflow_16.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_15
        '
        Me._chkMaintainClaimWorkflow_15.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_15.Checked = True
        Me._chkMaintainClaimWorkflow_15.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_15.Enabled = False
        Me._chkMaintainClaimWorkflow_15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_15.Location = New System.Drawing.Point(6, 310)
        Me._chkMaintainClaimWorkflow_15.Name = "_chkMaintainClaimWorkflow_15"
        Me._chkMaintainClaimWorkflow_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_15.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_15.TabIndex = 336
        Me._chkMaintainClaimWorkflow_15.Text = "Risk Details"
        Me._chkMaintainClaimWorkflow_15.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_14
        '
        Me._chkMaintainClaimWorkflow_14.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_14.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_14.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_14.Location = New System.Drawing.Point(6, 293)
        Me._chkMaintainClaimWorkflow_14.Name = "_chkMaintainClaimWorkflow_14"
        Me._chkMaintainClaimWorkflow_14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_14.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_14.TabIndex = 335
        Me._chkMaintainClaimWorkflow_14.Text = "Fast Track Claims Payments"
        Me._chkMaintainClaimWorkflow_14.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_13
        '
        Me._chkMaintainClaimWorkflow_13.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_13.Checked = True
        Me._chkMaintainClaimWorkflow_13.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_13.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_13.Enabled = False
        Me._chkMaintainClaimWorkflow_13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_13.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_13.Location = New System.Drawing.Point(6, 276)
        Me._chkMaintainClaimWorkflow_13.Name = "_chkMaintainClaimWorkflow_13"
        Me._chkMaintainClaimWorkflow_13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_13.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_13.TabIndex = 334
        Me._chkMaintainClaimWorkflow_13.Text = "Proceed to Claim Payments Message"
        Me._chkMaintainClaimWorkflow_13.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_12
        '
        Me._chkMaintainClaimWorkflow_12.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_12.Checked = True
        Me._chkMaintainClaimWorkflow_12.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_12.Location = New System.Drawing.Point(6, 259)
        Me._chkMaintainClaimWorkflow_12.Name = "_chkMaintainClaimWorkflow_12"
        Me._chkMaintainClaimWorkflow_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_12.Size = New System.Drawing.Size(255, 23)
        Me._chkMaintainClaimWorkflow_12.TabIndex = 333
        Me._chkMaintainClaimWorkflow_12.Text = "Claim Payments Process"
        Me._chkMaintainClaimWorkflow_12.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_11
        '
        Me._chkMaintainClaimWorkflow_11.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_11.Location = New System.Drawing.Point(6, 241)
        Me._chkMaintainClaimWorkflow_11.Name = "_chkMaintainClaimWorkflow_11"
        Me._chkMaintainClaimWorkflow_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_11.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_11.TabIndex = 332
        Me._chkMaintainClaimWorkflow_11.Text = "Produce Claim Notification Documents"
        Me._chkMaintainClaimWorkflow_11.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_10
        '
        Me._chkMaintainClaimWorkflow_10.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_10.Location = New System.Drawing.Point(6, 212)
        Me._chkMaintainClaimWorkflow_10.Name = "_chkMaintainClaimWorkflow_10"
        Me._chkMaintainClaimWorkflow_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_10.Size = New System.Drawing.Size(255, 30)
        Me._chkMaintainClaimWorkflow_10.TabIndex = 331
        Me._chkMaintainClaimWorkflow_10.Text = "Display Generate Claim Notification Documents Message"
        Me._chkMaintainClaimWorkflow_10.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_8
        '
        Me._chkMaintainClaimWorkflow_8.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_8.Checked = True
        Me._chkMaintainClaimWorkflow_8.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_8.Enabled = False
        Me._chkMaintainClaimWorkflow_8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_8.Location = New System.Drawing.Point(6, 173)
        Me._chkMaintainClaimWorkflow_8.Name = "_chkMaintainClaimWorkflow_8"
        Me._chkMaintainClaimWorkflow_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_8.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_8.TabIndex = 329
        Me._chkMaintainClaimWorkflow_8.Text = "Update Claim Details"
        Me._chkMaintainClaimWorkflow_8.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_7
        '
        Me._chkMaintainClaimWorkflow_7.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_7.Location = New System.Drawing.Point(6, 154)
        Me._chkMaintainClaimWorkflow_7.Name = "_chkMaintainClaimWorkflow_7"
        Me._chkMaintainClaimWorkflow_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_7.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_7.TabIndex = 328
        Me._chkMaintainClaimWorkflow_7.Text = "Enter Description for Change"
        Me._chkMaintainClaimWorkflow_7.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_6
        '
        Me._chkMaintainClaimWorkflow_6.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_6.Location = New System.Drawing.Point(6, 135)
        Me._chkMaintainClaimWorkflow_6.Name = "_chkMaintainClaimWorkflow_6"
        Me._chkMaintainClaimWorkflow_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_6.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_6.TabIndex = 327
        Me._chkMaintainClaimWorkflow_6.Text = "Third Party Recovery"
        Me._chkMaintainClaimWorkflow_6.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_5
        '
        Me._chkMaintainClaimWorkflow_5.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_5.Location = New System.Drawing.Point(6, 116)
        Me._chkMaintainClaimWorkflow_5.Name = "_chkMaintainClaimWorkflow_5"
        Me._chkMaintainClaimWorkflow_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_5.Size = New System.Drawing.Size(255, 17)
        Me._chkMaintainClaimWorkflow_5.TabIndex = 326
        Me._chkMaintainClaimWorkflow_5.Text = "Salvage Recovery"
        Me._chkMaintainClaimWorkflow_5.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_4
        '
        Me._chkMaintainClaimWorkflow_4.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_4.Location = New System.Drawing.Point(6, 97)
        Me._chkMaintainClaimWorkflow_4.Name = "_chkMaintainClaimWorkflow_4"
        Me._chkMaintainClaimWorkflow_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_4.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_4.TabIndex = 325
        Me._chkMaintainClaimWorkflow_4.Text = "Reinsurance Recoveries"
        Me._chkMaintainClaimWorkflow_4.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_3
        '
        Me._chkMaintainClaimWorkflow_3.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_3.Checked = True
        Me._chkMaintainClaimWorkflow_3.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_3.Enabled = False
        Me._chkMaintainClaimWorkflow_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_3.Location = New System.Drawing.Point(6, 78)
        Me._chkMaintainClaimWorkflow_3.Name = "_chkMaintainClaimWorkflow_3"
        Me._chkMaintainClaimWorkflow_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_3.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_3.TabIndex = 324
        Me._chkMaintainClaimWorkflow_3.Text = "Coinsurance Recoveries"
        Me._chkMaintainClaimWorkflow_3.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_2
        '
        Me._chkMaintainClaimWorkflow_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_2.Checked = True
        Me._chkMaintainClaimWorkflow_2.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_2.Enabled = False
        Me._chkMaintainClaimWorkflow_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_2.Location = New System.Drawing.Point(6, 59)
        Me._chkMaintainClaimWorkflow_2.Name = "_chkMaintainClaimWorkflow_2"
        Me._chkMaintainClaimWorkflow_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_2.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_2.TabIndex = 323
        Me._chkMaintainClaimWorkflow_2.Text = "Risk Details"
        Me._chkMaintainClaimWorkflow_2.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_1
        '
        Me._chkMaintainClaimWorkflow_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_1.Checked = True
        Me._chkMaintainClaimWorkflow_1.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_1.Enabled = False
        Me._chkMaintainClaimWorkflow_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_1.Location = New System.Drawing.Point(6, 40)
        Me._chkMaintainClaimWorkflow_1.Name = "_chkMaintainClaimWorkflow_1"
        Me._chkMaintainClaimWorkflow_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_1.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_1.TabIndex = 322
        Me._chkMaintainClaimWorkflow_1.Text = "Claim Details"
        Me._chkMaintainClaimWorkflow_1.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_0
        '
        Me._chkMaintainClaimWorkflow_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_0.Checked = True
        Me._chkMaintainClaimWorkflow_0.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_0.Enabled = False
        Me._chkMaintainClaimWorkflow_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_0.Location = New System.Drawing.Point(6, 20)
        Me._chkMaintainClaimWorkflow_0.Name = "_chkMaintainClaimWorkflow_0"
        Me._chkMaintainClaimWorkflow_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_0.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_0.TabIndex = 321
        Me._chkMaintainClaimWorkflow_0.Text = "Find Claim"
        Me._chkMaintainClaimWorkflow_0.UseVisualStyleBackColor = False
        '
        '_chkMaintainClaimWorkflow_9
        '
        Me._chkMaintainClaimWorkflow_9.BackColor = System.Drawing.SystemColors.Control
        Me._chkMaintainClaimWorkflow_9.Checked = True
        Me._chkMaintainClaimWorkflow_9.CheckState = System.Windows.Forms.CheckState.Checked
        Me._chkMaintainClaimWorkflow_9.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkMaintainClaimWorkflow_9.Enabled = False
        Me._chkMaintainClaimWorkflow_9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkMaintainClaimWorkflow_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkMaintainClaimWorkflow_9.Location = New System.Drawing.Point(6, 192)
        Me._chkMaintainClaimWorkflow_9.Name = "_chkMaintainClaimWorkflow_9"
        Me._chkMaintainClaimWorkflow_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkMaintainClaimWorkflow_9.Size = New System.Drawing.Size(255, 13)
        Me._chkMaintainClaimWorkflow_9.TabIndex = 330
        Me._chkMaintainClaimWorkflow_9.Text = "Check Status"
        Me._chkMaintainClaimWorkflow_9.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.grpClaimScripts)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraCausations)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraLimits)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraNumbering)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraSupression)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Frame4)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(842, 656)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2-Claim"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        'grpClaimScripts
        '
        Me.grpClaimScripts.BackColor = System.Drawing.SystemColors.Control
        Me.grpClaimScripts.Controls.Add(Me.chkIsPaymentsReadonly)
        Me.grpClaimScripts.Controls.Add(Me.chkIsReservesReadonly)
        Me.grpClaimScripts.Controls.Add(Me.chkIsRecoveriesReadonly)
        Me.grpClaimScripts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpClaimScripts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpClaimScripts.Location = New System.Drawing.Point(8, 243)
        Me.grpClaimScripts.Name = "grpClaimScripts"
        Me.grpClaimScripts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.grpClaimScripts.Size = New System.Drawing.Size(821, 68)
        Me.grpClaimScripts.TabIndex = 187
        Me.grpClaimScripts.TabStop = False
        Me.grpClaimScripts.Text = "Claim Scripts"
        '
        'chkIsPaymentsReadonly
        '
        Me.chkIsPaymentsReadonly.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsPaymentsReadonly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsPaymentsReadonly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsPaymentsReadonly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsPaymentsReadonly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsPaymentsReadonly.Location = New System.Drawing.Point(14, 41)
        Me.chkIsPaymentsReadonly.Name = "chkIsPaymentsReadonly"
        Me.chkIsPaymentsReadonly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsPaymentsReadonly.Size = New System.Drawing.Size(276, 21)
        Me.chkIsPaymentsReadonly.TabIndex = 20
        Me.chkIsPaymentsReadonly.Text = "Run Claim Scripts for Payments"
        Me.chkIsPaymentsReadonly.UseVisualStyleBackColor = False
        '
        'chkIsReservesReadonly
        '
        Me.chkIsReservesReadonly.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReservesReadonly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsReservesReadonly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReservesReadonly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReservesReadonly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReservesReadonly.Location = New System.Drawing.Point(14, 18)
        Me.chkIsReservesReadonly.Name = "chkIsReservesReadonly"
        Me.chkIsReservesReadonly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReservesReadonly.Size = New System.Drawing.Size(276, 17)
        Me.chkIsReservesReadonly.TabIndex = 16
        Me.chkIsReservesReadonly.Text = "Run Claim Scripts for reserves"
        Me.chkIsReservesReadonly.UseVisualStyleBackColor = False
        '
        'chkIsRecoveriesReadonly
        '
        Me.chkIsRecoveriesReadonly.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRecoveriesReadonly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRecoveriesReadonly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRecoveriesReadonly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRecoveriesReadonly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRecoveriesReadonly.Location = New System.Drawing.Point(405, 14)
        Me.chkIsRecoveriesReadonly.Name = "chkIsRecoveriesReadonly"
        Me.chkIsRecoveriesReadonly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRecoveriesReadonly.Size = New System.Drawing.Size(351, 21)
        Me.chkIsRecoveriesReadonly.TabIndex = 19
        Me.chkIsRecoveriesReadonly.Text = "Run Claim Scripts for Recoveries"
        Me.chkIsRecoveriesReadonly.UseVisualStyleBackColor = False
        '
        'fraCausations
        '
        Me.fraCausations.BackColor = System.Drawing.SystemColors.Control
        Me.fraCausations.Controls.Add(Me.uctPickListCausations)
        Me.fraCausations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCausations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCausations.Location = New System.Drawing.Point(8, 416)
        Me.fraCausations.Name = "fraCausations"
        Me.fraCausations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCausations.Size = New System.Drawing.Size(819, 195)
        Me.fraCausations.TabIndex = 185
        Me.fraCausations.TabStop = False
        Me.fraCausations.Text = "Causations"
        '
        'uctPickListCausations
        '
        Me.uctPickListCausations.AvailableCaption = ""
        Me.uctPickListCausations.BusinessObject = "bSIRProduct.Business"
        Me.uctPickListCausations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPickListCausations.ForeignKeys = CType(resources.GetObject("uctPickListCausations.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.uctPickListCausations.IsSearchable = False
        Me.uctPickListCausations.Location = New System.Drawing.Point(8, 11)
        Me.uctPickListCausations.Name = "uctPickListCausations"
        Me.uctPickListCausations.PickListType = "Causation"
        Me.uctPickListCausations.Size = New System.Drawing.Size(805, 174)
        Me.uctPickListCausations.TabIndex = 21
        '
        'fraLimits
        '
        Me.fraLimits.BackColor = System.Drawing.SystemColors.Control
        Me.fraLimits.Controls.Add(Me.txtAllowedClaims)
        Me.fraLimits.Controls.Add(Me.txtClaimYear)
        Me.fraLimits.Controls.Add(Me.txtSingleClaimValue)
        Me.fraLimits.Controls.Add(Me.txtTotalClaimsValue)
        Me.fraLimits.Controls.Add(Me.chkCheckAgent)
        Me.fraLimits.Controls.Add(Me.chkMediaTypeMandatory)
        Me.fraLimits.Controls.Add(Me.chkLossCurrencyChange)
        Me.fraLimits.Controls.Add(Me.cboBankAccount)
        Me.fraLimits.Controls.Add(Me.lblBankAccount)
        Me.fraLimits.Controls.Add(Me.lblAllowedClaims)
        Me.fraLimits.Controls.Add(Me.lblClaimYear)
        Me.fraLimits.Controls.Add(Me.lblSingleClaimValue)
        Me.fraLimits.Controls.Add(Me.lblTotalClaimsValue)
        Me.fraLimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLimits.Location = New System.Drawing.Point(8, 4)
        Me.fraLimits.Name = "fraLimits"
        Me.fraLimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLimits.Size = New System.Drawing.Size(823, 151)
        Me.fraLimits.TabIndex = 181
        Me.fraLimits.TabStop = False
        Me.fraLimits.Text = "Limits"
        '
        'txtAllowedClaims
        '
        Me.txtAllowedClaims.AcceptsReturn = True
        Me.txtAllowedClaims.BackColor = System.Drawing.SystemColors.Window
        Me.txtAllowedClaims.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAllowedClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAllowedClaims.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAllowedClaims.Location = New System.Drawing.Point(645, 14)
        Me.txtAllowedClaims.MaxLength = 10
        Me.txtAllowedClaims.Name = "txtAllowedClaims"
        Me.txtAllowedClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAllowedClaims.Size = New System.Drawing.Size(167, 20)
        Me.txtAllowedClaims.TabIndex = 7
        Me.txtAllowedClaims.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtClaimYear
        '
        Me.txtClaimYear.AcceptsReturn = True
        Me.txtClaimYear.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimYear.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimYear.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimYear.Location = New System.Drawing.Point(136, 17)
        Me.txtClaimYear.MaxLength = 10
        Me.txtClaimYear.Name = "txtClaimYear"
        Me.txtClaimYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimYear.Size = New System.Drawing.Size(169, 20)
        Me.txtClaimYear.TabIndex = 3
        '
        'txtSingleClaimValue
        '
        Me.txtSingleClaimValue.AcceptsReturn = True
        Me.txtSingleClaimValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtSingleClaimValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSingleClaimValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSingleClaimValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSingleClaimValue.Location = New System.Drawing.Point(136, 40)
        Me.txtSingleClaimValue.MaxLength = 10
        Me.txtSingleClaimValue.Name = "txtSingleClaimValue"
        Me.txtSingleClaimValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSingleClaimValue.Size = New System.Drawing.Size(169, 20)
        Me.txtSingleClaimValue.TabIndex = 4
        '
        'txtTotalClaimsValue
        '
        Me.txtTotalClaimsValue.AcceptsReturn = True
        Me.txtTotalClaimsValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtTotalClaimsValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalClaimsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalClaimsValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalClaimsValue.Location = New System.Drawing.Point(645, 36)
        Me.txtTotalClaimsValue.MaxLength = 10
        Me.txtTotalClaimsValue.Name = "txtTotalClaimsValue"
        Me.txtTotalClaimsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalClaimsValue.Size = New System.Drawing.Size(167, 20)
        Me.txtTotalClaimsValue.TabIndex = 8
        Me.txtTotalClaimsValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkCheckAgent
        '
        Me.chkCheckAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkCheckAgent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCheckAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCheckAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCheckAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCheckAgent.Location = New System.Drawing.Point(8, 64)
        Me.chkCheckAgent.Name = "chkCheckAgent"
        Me.chkCheckAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCheckAgent.Size = New System.Drawing.Size(297, 17)
        Me.chkCheckAgent.TabIndex = 5
        Me.chkCheckAgent.Text = "Prevent Claim Payments on Cancelled Agents"
        Me.chkCheckAgent.UseVisualStyleBackColor = False
        '
        'chkMediaTypeMandatory
        '
        Me.chkMediaTypeMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkMediaTypeMandatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMediaTypeMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMediaTypeMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMediaTypeMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMediaTypeMandatory.Location = New System.Drawing.Point(402, 80)
        Me.chkMediaTypeMandatory.Name = "chkMediaTypeMandatory"
        Me.chkMediaTypeMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMediaTypeMandatory.Size = New System.Drawing.Size(297, 25)
        Me.chkMediaTypeMandatory.TabIndex = 10
        Me.chkMediaTypeMandatory.Text = "Media Type Field Mandatory On Claim Payments"
        Me.chkMediaTypeMandatory.UseVisualStyleBackColor = False
        '
        'chkLossCurrencyChange
        '
        Me.chkLossCurrencyChange.BackColor = System.Drawing.SystemColors.Control
        Me.chkLossCurrencyChange.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkLossCurrencyChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLossCurrencyChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLossCurrencyChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLossCurrencyChange.Location = New System.Drawing.Point(8, 88)
        Me.chkLossCurrencyChange.Name = "chkLossCurrencyChange"
        Me.chkLossCurrencyChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLossCurrencyChange.Size = New System.Drawing.Size(297, 17)
        Me.chkLossCurrencyChange.TabIndex = 6
        Me.chkLossCurrencyChange.Text = "Allow Loss Currency Change"
        Me.chkLossCurrencyChange.UseVisualStyleBackColor = False
        '
        'cboBankAccount
        '
        Me.cboBankAccount.DefaultId = "0"
        Me.cboBankAccount.FirstItem = ""
        Me.cboBankAccount.Id = 0
        Me.cboBankAccount.ListIndex = -1
        Me.cboBankAccount.Location = New System.Drawing.Point(645, 58)
        Me.cboBankAccount.Name = "cboBankAccount"
        Me.cboBankAccount.Size = New System.Drawing.Size(168, 21)
        Me.cboBankAccount.TabIndex = 9
        Me.cboBankAccount.ToolTipText = ""
        Me.cboBankAccount.WhatsThisHelpID = 0
        '
        'lblBankAccount
        '
        Me.lblBankAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccount.Location = New System.Drawing.Point(402, 60)
        Me.lblBankAccount.Name = "lblBankAccount"
        Me.lblBankAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccount.Size = New System.Drawing.Size(118, 17)
        Me.lblBankAccount.TabIndex = 365
        Me.lblBankAccount.Text = "Bank Account :"
        '
        'lblAllowedClaims
        '
        Me.lblAllowedClaims.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllowedClaims.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllowedClaims.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllowedClaims.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllowedClaims.Location = New System.Drawing.Point(402, 15)
        Me.lblAllowedClaims.Name = "lblAllowedClaims"
        Me.lblAllowedClaims.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllowedClaims.Size = New System.Drawing.Size(104, 17)
        Me.lblAllowedClaims.TabIndex = 99
        Me.lblAllowedClaims.Text = "Allowed Claims :"
        '
        'lblClaimYear
        '
        Me.lblClaimYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimYear.Location = New System.Drawing.Point(8, 17)
        Me.lblClaimYear.Name = "lblClaimYear"
        Me.lblClaimYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimYear.Size = New System.Drawing.Size(119, 17)
        Me.lblClaimYear.TabIndex = 999
        Me.lblClaimYear.Text = "No. of Years Back :"
        '
        'lblSingleClaimValue
        '
        Me.lblSingleClaimValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblSingleClaimValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSingleClaimValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSingleClaimValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSingleClaimValue.Location = New System.Drawing.Point(8, 41)
        Me.lblSingleClaimValue.Name = "lblSingleClaimValue"
        Me.lblSingleClaimValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSingleClaimValue.Size = New System.Drawing.Size(121, 17)
        Me.lblSingleClaimValue.TabIndex = 99
        Me.lblSingleClaimValue.Text = "Single Claim Value :"
        '
        'lblTotalClaimsValue
        '
        Me.lblTotalClaimsValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalClaimsValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalClaimsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalClaimsValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalClaimsValue.Location = New System.Drawing.Point(402, 37)
        Me.lblTotalClaimsValue.Name = "lblTotalClaimsValue"
        Me.lblTotalClaimsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalClaimsValue.Size = New System.Drawing.Size(118, 17)
        Me.lblTotalClaimsValue.TabIndex = 99
        Me.lblTotalClaimsValue.Text = "Total Claims Value :"
        '
        'fraNumbering
        '
        Me.fraNumbering.BackColor = System.Drawing.SystemColors.Control
        Me.fraNumbering.Controls.Add(Me.cboFullClaimAutoNumberingID)
        Me.fraNumbering.Controls.Add(Me.cboProvClaimAutoNumberingID)
        Me.fraNumbering.Controls.Add(Me.lblFullClaimAutoNumberingID)
        Me.fraNumbering.Controls.Add(Me.lblProvClaimAutoNumberingID)
        Me.fraNumbering.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNumbering.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNumbering.Location = New System.Drawing.Point(410, 156)
        Me.fraNumbering.Name = "fraNumbering"
        Me.fraNumbering.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNumbering.Size = New System.Drawing.Size(419, 81)
        Me.fraNumbering.TabIndex = 183
        Me.fraNumbering.TabStop = False
        Me.fraNumbering.Text = "Numbering"
        '
        'lblFullClaimAutoNumberingID
        '
        Me.lblFullClaimAutoNumberingID.BackColor = System.Drawing.SystemColors.Control
        Me.lblFullClaimAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFullClaimAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullClaimAutoNumberingID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFullClaimAutoNumberingID.Location = New System.Drawing.Point(8, 48)
        Me.lblFullClaimAutoNumberingID.Name = "lblFullClaimAutoNumberingID"
        Me.lblFullClaimAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFullClaimAutoNumberingID.Size = New System.Drawing.Size(173, 29)
        Me.lblFullClaimAutoNumberingID.TabIndex = 99
        Me.lblFullClaimAutoNumberingID.Text = "Full Claim Auto Numbering ID"
        '
        'lblProvClaimAutoNumberingID
        '
        Me.lblProvClaimAutoNumberingID.BackColor = System.Drawing.SystemColors.Control
        Me.lblProvClaimAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProvClaimAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProvClaimAutoNumberingID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProvClaimAutoNumberingID.Location = New System.Drawing.Point(8, 22)
        Me.lblProvClaimAutoNumberingID.Name = "lblProvClaimAutoNumberingID"
        Me.lblProvClaimAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProvClaimAutoNumberingID.Size = New System.Drawing.Size(214, 26)
        Me.lblProvClaimAutoNumberingID.TabIndex = 88
        Me.lblProvClaimAutoNumberingID.Text = "Provisional Claim Auto Numbering ID"
        '
        'fraSupression
        '
        Me.fraSupression.BackColor = System.Drawing.SystemColors.Control
        Me.fraSupression.Controls.Add(Me.chkRecoveries)
        Me.fraSupression.Controls.Add(Me.chkPayments)
        Me.fraSupression.Controls.Add(Me.chkReserve)
        Me.fraSupression.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSupression.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSupression.Location = New System.Drawing.Point(8, 156)
        Me.fraSupression.Name = "fraSupression"
        Me.fraSupression.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSupression.Size = New System.Drawing.Size(395, 81)
        Me.fraSupression.TabIndex = 182
        Me.fraSupression.TabStop = False
        Me.fraSupression.Text = "Claims Transaction Suppression"
        '
        'chkRecoveries
        '
        Me.chkRecoveries.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecoveries.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecoveries.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecoveries.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecoveries.Location = New System.Drawing.Point(8, 53)
        Me.chkRecoveries.Name = "chkRecoveries"
        Me.chkRecoveries.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecoveries.Size = New System.Drawing.Size(153, 17)
        Me.chkRecoveries.TabIndex = 13
        Me.chkRecoveries.Text = "Recovery"
        Me.chkRecoveries.UseVisualStyleBackColor = False
        '
        'chkPayments
        '
        Me.chkPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPayments.Location = New System.Drawing.Point(8, 35)
        Me.chkPayments.Name = "chkPayments"
        Me.chkPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPayments.Size = New System.Drawing.Size(153, 17)
        Me.chkPayments.TabIndex = 12
        Me.chkPayments.Text = "Payment"
        Me.chkPayments.UseVisualStyleBackColor = False
        '
        'chkReserve
        '
        Me.chkReserve.BackColor = System.Drawing.SystemColors.Control
        Me.chkReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReserve.Location = New System.Drawing.Point(8, 16)
        Me.chkReserve.Name = "chkReserve"
        Me.chkReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReserve.Size = New System.Drawing.Size(185, 17)
        Me.chkReserve.TabIndex = 11
        Me.chkReserve.Text = "Reserve"
        Me.chkReserve.UseVisualStyleBackColor = False
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.chkRecoveryInstalmentsEnabled)
        Me.Frame4.Controls.Add(Me.txtAuthorisationThreshold)
        Me.Frame4.Controls.Add(Me.lblAuthorisationThreshold)
        Me.Frame4.Controls.Add(Me.chkRecommender)
        Me.Frame4.Controls.Add(Me.txtMaxNoofUnauthorisedClaimPayments)
        Me.Frame4.Controls.Add(Me.txtMaxUnauthorisedClaimsValue)
        Me.Frame4.Controls.Add(Me.chkMultiplePayments)
        Me.Frame4.Controls.Add(Me.chkRunAuthorisationScriptsClaimPayments)
        Me.Frame4.Controls.Add(Me.lblMaxNoofUnauthorisedClaimPayments)
        Me.Frame4.Controls.Add(Me.lblMaxUnauthorisedClaimsValue)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(8, 317)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(821, 200)
        Me.Frame4.TabIndex = 184
        Me.Frame4.TabStop = False
        Me.Frame4.Text = "Claim Payment"
        '
        'txtAuthorisationThreshold
        '
        Me.txtAuthorisationThreshold.AcceptsReturn = True
        Me.txtAuthorisationThreshold.Location = New System.Drawing.Point(631, 62)
        Me.txtAuthorisationThreshold.MaxLength = 10
        Me.txtAuthorisationThreshold.Name = "txtAuthorisationThreshold"
        Me.txtAuthorisationThreshold.Size = New System.Drawing.Size(175, 20)
        Me.txtAuthorisationThreshold.TabIndex = 21
        Me.txtAuthorisationThreshold.Visible = False
        '
        'lblAuthorisationThreshold
        '
        Me.lblAuthorisationThreshold.Location = New System.Drawing.Point(407, 64)
        Me.lblAuthorisationThreshold.Name = "lblAuthorisationThreshold"
        Me.lblAuthorisationThreshold.Size = New System.Drawing.Size(174, 23)
        Me.lblAuthorisationThreshold.TabIndex = 293
        Me.lblAuthorisationThreshold.Text = "Authorisation Threshold"
        '
        'chkRecommender
        '
        Me.chkRecommender.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecommender.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRecommender.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecommender.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecommender.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecommender.Location = New System.Drawing.Point(406, 32)
        Me.chkRecommender.Name = "chkRecommender"
        Me.chkRecommender.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecommender.Size = New System.Drawing.Size(351, 25)
        Me.chkRecommender.TabIndex = 20
        Me.chkRecommender.Text = "Recommender Steps for Claim Payments"
        Me.chkRecommender.UseVisualStyleBackColor = False
        '
        'txtMaxNoofUnauthorisedClaimPayments
        '
        Me.txtMaxNoofUnauthorisedClaimPayments.AcceptsReturn = True
        Me.txtMaxNoofUnauthorisedClaimPayments.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxNoofUnauthorisedClaimPayments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxNoofUnauthorisedClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxNoofUnauthorisedClaimPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxNoofUnauthorisedClaimPayments.Location = New System.Drawing.Point(278, 62)
        Me.txtMaxNoofUnauthorisedClaimPayments.MaxLength = 3
        Me.txtMaxNoofUnauthorisedClaimPayments.Name = "txtMaxNoofUnauthorisedClaimPayments"
        Me.txtMaxNoofUnauthorisedClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxNoofUnauthorisedClaimPayments.Size = New System.Drawing.Size(77, 20)
        Me.txtMaxNoofUnauthorisedClaimPayments.TabIndex = 18
        Me.txtMaxNoofUnauthorisedClaimPayments.Visible = False
        '
        'txtMaxUnauthorisedClaimsValue
        '
        Me.txtMaxUnauthorisedClaimsValue.AcceptsReturn = True
        Me.txtMaxUnauthorisedClaimsValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxUnauthorisedClaimsValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxUnauthorisedClaimsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxUnauthorisedClaimsValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxUnauthorisedClaimsValue.Location = New System.Drawing.Point(278, 38)
        Me.txtMaxUnauthorisedClaimsValue.MaxLength = 10
        Me.txtMaxUnauthorisedClaimsValue.Name = "txtMaxUnauthorisedClaimsValue"
        Me.txtMaxUnauthorisedClaimsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxUnauthorisedClaimsValue.Size = New System.Drawing.Size(77, 20)
        Me.txtMaxUnauthorisedClaimsValue.TabIndex = 17
        Me.txtMaxUnauthorisedClaimsValue.Visible = False
        '
        'chkMultiplePayments
        '
        Me.chkMultiplePayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkMultiplePayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMultiplePayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMultiplePayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMultiplePayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMultiplePayments.Location = New System.Drawing.Point(12, 18)
        Me.chkMultiplePayments.Name = "chkMultiplePayments"
        Me.chkMultiplePayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMultiplePayments.Size = New System.Drawing.Size(279, 17)
        Me.chkMultiplePayments.TabIndex = 16
        Me.chkMultiplePayments.Text = "Multiple Claim Payments"
        Me.chkMultiplePayments.UseVisualStyleBackColor = False
        '
        'chkRunAuthorisationScriptsClaimPayments
        '
        Me.chkRunAuthorisationScriptsClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkRunAuthorisationScriptsClaimPayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRunAuthorisationScriptsClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRunAuthorisationScriptsClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunAuthorisationScriptsClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRunAuthorisationScriptsClaimPayments.Location = New System.Drawing.Point(406, 12)
        Me.chkRunAuthorisationScriptsClaimPayments.Name = "chkRunAuthorisationScriptsClaimPayments"
        Me.chkRunAuthorisationScriptsClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRunAuthorisationScriptsClaimPayments.Size = New System.Drawing.Size(351, 25)
        Me.chkRunAuthorisationScriptsClaimPayments.TabIndex = 19
        Me.chkRunAuthorisationScriptsClaimPayments.Text = "Run Authorisation Scripts for Claims Payments"
        Me.chkRunAuthorisationScriptsClaimPayments.UseVisualStyleBackColor = False
        '
        'lblMaxNoofUnauthorisedClaimPayments
        '
        Me.lblMaxNoofUnauthorisedClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxNoofUnauthorisedClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxNoofUnauthorisedClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxNoofUnauthorisedClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxNoofUnauthorisedClaimPayments.Location = New System.Drawing.Point(12, 62)
        Me.lblMaxNoofUnauthorisedClaimPayments.Name = "lblMaxNoofUnauthorisedClaimPayments"
        Me.lblMaxNoofUnauthorisedClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxNoofUnauthorisedClaimPayments.Size = New System.Drawing.Size(243, 29)
        Me.lblMaxNoofUnauthorisedClaimPayments.TabIndex = 292
        Me.lblMaxNoofUnauthorisedClaimPayments.Text = "Max. No. of Unauthorised Claim Payments"
        Me.lblMaxNoofUnauthorisedClaimPayments.Visible = False
        '
        'lblMaxUnauthorisedClaimsValue
        '
        Me.lblMaxUnauthorisedClaimsValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxUnauthorisedClaimsValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxUnauthorisedClaimsValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxUnauthorisedClaimsValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxUnauthorisedClaimsValue.Location = New System.Drawing.Point(12, 40)
        Me.lblMaxUnauthorisedClaimsValue.Name = "lblMaxUnauthorisedClaimsValue"
        Me.lblMaxUnauthorisedClaimsValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxUnauthorisedClaimsValue.Size = New System.Drawing.Size(223, 29)
        Me.lblMaxUnauthorisedClaimsValue.TabIndex = 291
        Me.lblMaxUnauthorisedClaimsValue.Text = "Max. Unauthorised Claims Value"
        Me.lblMaxUnauthorisedClaimsValue.Visible = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraWrittenStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraOtherOptions)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPremium)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraProRata)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraRenewals)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraPolicyCreation)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDetails)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 40)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(842, 656)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Product"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'fraWrittenStatus
        '
        Me.fraWrittenStatus.Controls.Add(Me.cboReminderTaskGroup)
        Me.fraWrittenStatus.Controls.Add(Me.cboReminderUserGroup)
        Me.fraWrittenStatus.Controls.Add(Me.lblReminderTaskGroup)
        Me.fraWrittenStatus.Controls.Add(Me.txtTaskManagerDays)
        Me.fraWrittenStatus.Controls.Add(Me.lblReminderUserGroup)
        Me.fraWrittenStatus.Controls.Add(Me.lblTaskManagerDays)
        Me.fraWrittenStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.fraWrittenStatus.Location = New System.Drawing.Point(8, 608)
        Me.fraWrittenStatus.Name = "fraWrittenStatus"
        Me.fraWrittenStatus.Size = New System.Drawing.Size(817, 45)
        Me.fraWrittenStatus.TabIndex = 59
        Me.fraWrittenStatus.TabStop = False
        Me.fraWrittenStatus.Text = "Written Policy Status"
        '
        'lblReminderTaskGroup
        '
        Me.lblReminderTaskGroup.AutoSize = True
        Me.lblReminderTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.lblReminderTaskGroup.Location = New System.Drawing.Point(527, 21)
        Me.lblReminderTaskGroup.Name = "lblReminderTaskGroup"
        Me.lblReminderTaskGroup.Size = New System.Drawing.Size(111, 13)
        Me.lblReminderTaskGroup.TabIndex = 3
        Me.lblReminderTaskGroup.Text = "Reminder Task Group"
        '
        'txtTaskManagerDays
        '
        Me.txtTaskManagerDays.Location = New System.Drawing.Point(142, 15)
        Me.txtTaskManagerDays.Name = "txtTaskManagerDays"
        Me.txtTaskManagerDays.Size = New System.Drawing.Size(41, 20)
        Me.txtTaskManagerDays.TabIndex = 2
        '
        'lblReminderUserGroup
        '
        Me.lblReminderUserGroup.AutoSize = True
        Me.lblReminderUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.lblReminderUserGroup.Location = New System.Drawing.Point(226, 20)
        Me.lblReminderUserGroup.Name = "lblReminderUserGroup"
        Me.lblReminderUserGroup.Size = New System.Drawing.Size(109, 13)
        Me.lblReminderUserGroup.TabIndex = 1
        Me.lblReminderUserGroup.Text = "Reminder User Group"
        '
        'lblTaskManagerDays
        '
        Me.lblTaskManagerDays.AutoSize = True
        Me.lblTaskManagerDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.lblTaskManagerDays.Location = New System.Drawing.Point(11, 18)
        Me.lblTaskManagerDays.Name = "lblTaskManagerDays"
        Me.lblTaskManagerDays.Size = New System.Drawing.Size(103, 13)
        Me.lblTaskManagerDays.TabIndex = 0
        Me.lblTaskManagerDays.Text = "Task Manager Days"
        '
        'fraOtherOptions
        '
        Me.fraOtherOptions.BackColor = System.Drawing.SystemColors.Control
        Me.fraOtherOptions.Controls.Add(Me.Label102)
        Me.fraOtherOptions.Controls.Add(Me.cboApplyMandatoryRisk)
        Me.fraOtherOptions.Controls.Add(Me.lblApplyMandatoryRisk)
        Me.fraOtherOptions.Controls.Add(Me.cmdRIModel)
        Me.fraOtherOptions.Controls.Add(Me.cboPosValues)
        Me.fraOtherOptions.Controls.Add(Me.chkAllowStandardWordingEdit)
        Me.fraOtherOptions.Controls.Add(Me.lblPositiveValues)
        Me.fraOtherOptions.Controls.Add(Me.lblAllowStandardWordingEdit)
        Me.fraOtherOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOtherOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOtherOptions.Location = New System.Drawing.Point(485, 349)
        Me.fraOtherOptions.Name = "fraOtherOptions"
        Me.fraOtherOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOtherOptions.Size = New System.Drawing.Size(341, 108)
        Me.fraOtherOptions.TabIndex = 52
        Me.fraOtherOptions.TabStop = False
        Me.fraOtherOptions.Text = "Other Options"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(117, 89)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(215, 13)
        Me.Label102.TabIndex = 61
        Me.Label102.Text = "NB:MTC Rating rules enabled (with pro-rata)"
        '
        'cboApplyMandatoryRisk
        '
        Me.cboApplyMandatoryRisk.DefaultItemId = 0
        Me.cboApplyMandatoryRisk.FirstItem = ""
        Me.cboApplyMandatoryRisk.ItemId = 0
        Me.cboApplyMandatoryRisk.ListIndex = -1
        Me.cboApplyMandatoryRisk.Location = New System.Drawing.Point(200, 64)
        Me.cboApplyMandatoryRisk.Name = "cboApplyMandatoryRisk"
        Me.cboApplyMandatoryRisk.PMLookupProductFamily = 9
        Me.cboApplyMandatoryRisk.SingleItemId = 0
        Me.cboApplyMandatoryRisk.Size = New System.Drawing.Size(132, 21)
        Me.cboApplyMandatoryRisk.SortColumnName = ""
        Me.cboApplyMandatoryRisk.Sorted = True
        Me.cboApplyMandatoryRisk.TabIndex = 60
        Me.cboApplyMandatoryRisk.TableName = "Risk_Type"
        Me.cboApplyMandatoryRisk.ToolTipText = ""
        Me.cboApplyMandatoryRisk.WhereClause = ""
        '
        'lblApplyMandatoryRisk
        '
        Me.lblApplyMandatoryRisk.AutoSize = True
        Me.lblApplyMandatoryRisk.Location = New System.Drawing.Point(8, 70)
        Me.lblApplyMandatoryRisk.Name = "lblApplyMandatoryRisk"
        Me.lblApplyMandatoryRisk.Size = New System.Drawing.Size(109, 13)
        Me.lblApplyMandatoryRisk.TabIndex = 59
        Me.lblApplyMandatoryRisk.Text = "Apply mandatory Risk"
        '
        'cmdRIModel
        '
        Me.cmdRIModel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRIModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRIModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRIModel.Location = New System.Drawing.Point(248, 42)
        Me.cmdRIModel.Name = "cmdRIModel"
        Me.cmdRIModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRIModel.Size = New System.Drawing.Size(84, 20)
        Me.cmdRIModel.TabIndex = 32
        Me.cmdRIModel.TabStop = False
        Me.cmdRIModel.Text = "&RI Model"
        Me.cmdRIModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRIModel.UseVisualStyleBackColor = False
        Me.cmdRIModel.Visible = False
        '
        'chkAllowStandardWordingEdit
        '
        Me.chkAllowStandardWordingEdit.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowStandardWordingEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowStandardWordingEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowStandardWordingEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowStandardWordingEdit.Location = New System.Drawing.Point(200, 42)
        Me.chkAllowStandardWordingEdit.Name = "chkAllowStandardWordingEdit"
        Me.chkAllowStandardWordingEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowStandardWordingEdit.Size = New System.Drawing.Size(16, 19)
        Me.chkAllowStandardWordingEdit.TabIndex = 31
        Me.chkAllowStandardWordingEdit.UseVisualStyleBackColor = False
        '
        'lblPositiveValues
        '
        Me.lblPositiveValues.AutoSize = True
        Me.lblPositiveValues.BackColor = System.Drawing.SystemColors.Control
        Me.lblPositiveValues.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPositiveValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPositiveValues.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPositiveValues.Location = New System.Drawing.Point(8, 20)
        Me.lblPositiveValues.Name = "lblPositiveValues"
        Me.lblPositiveValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPositiveValues.Size = New System.Drawing.Size(157, 13)
        Me.lblPositiveValues.TabIndex = 53
        Me.lblPositiveValues.Text = "Positive Values in Cancel Policy"
        '
        'lblAllowStandardWordingEdit
        '
        Me.lblAllowStandardWordingEdit.AutoSize = True
        Me.lblAllowStandardWordingEdit.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllowStandardWordingEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllowStandardWordingEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllowStandardWordingEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllowStandardWordingEdit.Location = New System.Drawing.Point(8, 45)
        Me.lblAllowStandardWordingEdit.Name = "lblAllowStandardWordingEdit"
        Me.lblAllowStandardWordingEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllowStandardWordingEdit.Size = New System.Drawing.Size(142, 13)
        Me.lblAllowStandardWordingEdit.TabIndex = 55
        Me.lblAllowStandardWordingEdit.Text = "Allow Standard Wording Edit"
        '
        'fraPremium
        '
        Me.fraPremium.BackColor = System.Drawing.SystemColors.Control
        Me.fraPremium.Controls.Add(Me.chkRoundOffToZero)
        Me.fraPremium.Controls.Add(Me.chkRoundPremium)
        Me.fraPremium.Controls.Add(Me.chkCurrencyChange)
        Me.fraPremium.Controls.Add(Me.chkTaxSuppressed)
        Me.fraPremium.Controls.Add(Me.chkAccumulation)
        Me.fraPremium.Controls.Add(Me.cboRoundingSection)
        Me.fraPremium.Controls.Add(Me.lblRoundingSection)
        Me.fraPremium.Controls.Add(Me.lblRoundPremium)
        Me.fraPremium.Controls.Add(Me.lblCurrencyChange)
        Me.fraPremium.Controls.Add(Me.lblAccumulation)
        Me.fraPremium.Controls.Add(Me.lblTaxSuppressed)
        Me.fraPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPremium.Location = New System.Drawing.Point(587, 169)
        Me.fraPremium.Name = "fraPremium"
        Me.fraPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPremium.Size = New System.Drawing.Size(241, 176)
        Me.fraPremium.TabIndex = 33
        Me.fraPremium.TabStop = False
        Me.fraPremium.Text = "Premium"
        '
        'chkRoundOffToZero
        '
        Me.chkRoundOffToZero.BackColor = System.Drawing.SystemColors.Control
        Me.chkRoundOffToZero.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRoundOffToZero.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRoundOffToZero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRoundOffToZero.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRoundOffToZero.Location = New System.Drawing.Point(8, 128)
        Me.chkRoundOffToZero.Name = "chkRoundOffToZero"
        Me.chkRoundOffToZero.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRoundOffToZero.Size = New System.Drawing.Size(225, 25)
        Me.chkRoundOffToZero.TabIndex = 24
        Me.chkRoundOffToZero.Text = "Gross Total Round Off upto 0 Decimals"
        Me.chkRoundOffToZero.UseVisualStyleBackColor = False
        '
        'chkRoundPremium
        '
        Me.chkRoundPremium.BackColor = System.Drawing.SystemColors.Control
        Me.chkRoundPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRoundPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRoundPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRoundPremium.Location = New System.Drawing.Point(218, 76)
        Me.chkRoundPremium.Name = "chkRoundPremium"
        Me.chkRoundPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRoundPremium.Size = New System.Drawing.Size(16, 19)
        Me.chkRoundPremium.TabIndex = 22
        Me.chkRoundPremium.UseVisualStyleBackColor = False
        '
        'chkCurrencyChange
        '
        Me.chkCurrencyChange.BackColor = System.Drawing.SystemColors.Control
        Me.chkCurrencyChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCurrencyChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCurrencyChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCurrencyChange.Location = New System.Drawing.Point(218, 56)
        Me.chkCurrencyChange.Name = "chkCurrencyChange"
        Me.chkCurrencyChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCurrencyChange.Size = New System.Drawing.Size(16, 19)
        Me.chkCurrencyChange.TabIndex = 21
        Me.chkCurrencyChange.UseVisualStyleBackColor = False
        '
        'chkTaxSuppressed
        '
        Me.chkTaxSuppressed.BackColor = System.Drawing.SystemColors.Control
        Me.chkTaxSuppressed.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTaxSuppressed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTaxSuppressed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTaxSuppressed.Location = New System.Drawing.Point(218, 36)
        Me.chkTaxSuppressed.Name = "chkTaxSuppressed"
        Me.chkTaxSuppressed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTaxSuppressed.Size = New System.Drawing.Size(16, 19)
        Me.chkTaxSuppressed.TabIndex = 20
        Me.chkTaxSuppressed.UseVisualStyleBackColor = False
        '
        'chkAccumulation
        '
        Me.chkAccumulation.BackColor = System.Drawing.SystemColors.Control
        Me.chkAccumulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAccumulation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAccumulation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAccumulation.Location = New System.Drawing.Point(218, 16)
        Me.chkAccumulation.Name = "chkAccumulation"
        Me.chkAccumulation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAccumulation.Size = New System.Drawing.Size(16, 19)
        Me.chkAccumulation.TabIndex = 19
        Me.chkAccumulation.UseVisualStyleBackColor = False
        '
        'cboRoundingSection
        '
        Me.cboRoundingSection.DefaultItemId = 0
        Me.cboRoundingSection.Enabled = False
        Me.cboRoundingSection.FirstItem = ""
        Me.cboRoundingSection.ItemId = 0
        Me.cboRoundingSection.ListIndex = -1
        Me.cboRoundingSection.Location = New System.Drawing.Point(126, 100)
        Me.cboRoundingSection.Name = "cboRoundingSection"
        Me.cboRoundingSection.PMLookupProductFamily = 9
        Me.cboRoundingSection.SingleItemId = 0
        Me.cboRoundingSection.Size = New System.Drawing.Size(108, 21)
        Me.cboRoundingSection.SortColumnName = ""
        Me.cboRoundingSection.Sorted = True
        Me.cboRoundingSection.TabIndex = 23
        Me.cboRoundingSection.TableName = "rating_section_type"
        Me.cboRoundingSection.ToolTipText = ""
        Me.cboRoundingSection.WhereClause = ""
        '
        'lblRoundingSection
        '
        Me.lblRoundingSection.BackColor = System.Drawing.SystemColors.Control
        Me.lblRoundingSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRoundingSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRoundingSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRoundingSection.Location = New System.Drawing.Point(8, 102)
        Me.lblRoundingSection.Name = "lblRoundingSection"
        Me.lblRoundingSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRoundingSection.Size = New System.Drawing.Size(112, 25)
        Me.lblRoundingSection.TabIndex = 544545
        Me.lblRoundingSection.Text = "Rounding Section:"
        '
        'lblRoundPremium
        '
        Me.lblRoundPremium.AutoSize = True
        Me.lblRoundPremium.BackColor = System.Drawing.SystemColors.Control
        Me.lblRoundPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRoundPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRoundPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRoundPremium.Location = New System.Drawing.Point(8, 81)
        Me.lblRoundPremium.Name = "lblRoundPremium"
        Me.lblRoundPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRoundPremium.Size = New System.Drawing.Size(82, 13)
        Me.lblRoundPremium.TabIndex = 66
        Me.lblRoundPremium.Text = "Round Premium"
        '
        'lblCurrencyChange
        '
        Me.lblCurrencyChange.AutoSize = True
        Me.lblCurrencyChange.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyChange.Location = New System.Drawing.Point(8, 59)
        Me.lblCurrencyChange.Name = "lblCurrencyChange"
        Me.lblCurrencyChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyChange.Size = New System.Drawing.Size(89, 13)
        Me.lblCurrencyChange.TabIndex = 66
        Me.lblCurrencyChange.Text = "Currency Change"
        '
        'lblAccumulation
        '
        Me.lblAccumulation.AutoSize = True
        Me.lblAccumulation.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccumulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccumulation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccumulation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccumulation.Location = New System.Drawing.Point(8, 19)
        Me.lblAccumulation.Name = "lblAccumulation"
        Me.lblAccumulation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccumulation.Size = New System.Drawing.Size(71, 13)
        Me.lblAccumulation.TabIndex = 66
        Me.lblAccumulation.Text = "Accumulation"
        '
        'lblTaxSuppressed
        '
        Me.lblTaxSuppressed.AutoSize = True
        Me.lblTaxSuppressed.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxSuppressed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxSuppressed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxSuppressed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxSuppressed.Location = New System.Drawing.Point(8, 39)
        Me.lblTaxSuppressed.Name = "lblTaxSuppressed"
        Me.lblTaxSuppressed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxSuppressed.Size = New System.Drawing.Size(84, 13)
        Me.lblTaxSuppressed.TabIndex = 66
        Me.lblTaxSuppressed.Text = "Tax Suppressed"
        '
        'fraProRata
        '
        Me.fraProRata.BackColor = System.Drawing.SystemColors.Control
        Me.fraProRata.Controls.Add(Me.chkMTCRatingRules)
        Me.fraProRata.Controls.Add(Me.cmdSPR)
        Me.fraProRata.Controls.Add(Me.chkMTAProRata)
        Me.fraProRata.Controls.Add(Me.chkNBProRata)
        Me.fraProRata.Controls.Add(Me.chkShortPeriodRated)
        Me.fraProRata.Controls.Add(Me.lblMTCRatingRules)
        Me.fraProRata.Controls.Add(Me.lblMTAProRata)
        Me.fraProRata.Controls.Add(Me.lblNBProRata)
        Me.fraProRata.Controls.Add(Me.lblShortPeriodRated)
        Me.fraProRata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraProRata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraProRata.Location = New System.Drawing.Point(8, 349)
        Me.fraProRata.Name = "fraProRata"
        Me.fraProRata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraProRata.Size = New System.Drawing.Size(468, 107)
        Me.fraProRata.TabIndex = 44
        Me.fraProRata.TabStop = False
        Me.fraProRata.Text = "Pro-Rata"
        '
        'chkMTCRatingRules
        '
        Me.chkMTCRatingRules.BackColor = System.Drawing.SystemColors.Control
        Me.chkMTCRatingRules.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMTCRatingRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMTCRatingRules.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMTCRatingRules.Location = New System.Drawing.Point(124, 64)
        Me.chkMTCRatingRules.Name = "chkMTCRatingRules"
        Me.chkMTCRatingRules.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMTCRatingRules.Size = New System.Drawing.Size(17, 17)
        Me.chkMTCRatingRules.TabIndex = 27
        Me.chkMTCRatingRules.UseVisualStyleBackColor = False
        '
        'cmdSPR
        '
        Me.cmdSPR.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSPR.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSPR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSPR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSPR.Location = New System.Drawing.Point(189, 34)
        Me.cmdSPR.Name = "cmdSPR"
        Me.cmdSPR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSPR.Size = New System.Drawing.Size(117, 22)
        Me.cmdSPR.TabIndex = 29
        Me.cmdSPR.Text = "&Short Period Rate"
        Me.cmdSPR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSPR.UseVisualStyleBackColor = False
        '
        'chkMTAProRata
        '
        Me.chkMTAProRata.BackColor = System.Drawing.SystemColors.Control
        Me.chkMTAProRata.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMTAProRata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMTAProRata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMTAProRata.Location = New System.Drawing.Point(290, 12)
        Me.chkMTAProRata.Name = "chkMTAProRata"
        Me.chkMTAProRata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMTAProRata.Size = New System.Drawing.Size(16, 19)
        Me.chkMTAProRata.TabIndex = 28
        Me.chkMTAProRata.UseVisualStyleBackColor = False
        '
        'chkNBProRata
        '
        Me.chkNBProRata.BackColor = System.Drawing.SystemColors.Control
        Me.chkNBProRata.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkNBProRata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNBProRata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNBProRata.Location = New System.Drawing.Point(124, 16)
        Me.chkNBProRata.Name = "chkNBProRata"
        Me.chkNBProRata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkNBProRata.Size = New System.Drawing.Size(16, 19)
        Me.chkNBProRata.TabIndex = 25
        Me.chkNBProRata.UseVisualStyleBackColor = False
        '
        'chkShortPeriodRated
        '
        Me.chkShortPeriodRated.BackColor = System.Drawing.SystemColors.Control
        Me.chkShortPeriodRated.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShortPeriodRated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShortPeriodRated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShortPeriodRated.Location = New System.Drawing.Point(124, 40)
        Me.chkShortPeriodRated.Name = "chkShortPeriodRated"
        Me.chkShortPeriodRated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShortPeriodRated.Size = New System.Drawing.Size(16, 19)
        Me.chkShortPeriodRated.TabIndex = 26
        Me.chkShortPeriodRated.UseVisualStyleBackColor = False
        '
        'lblMTCRatingRules
        '
        Me.lblMTCRatingRules.BackColor = System.Drawing.SystemColors.Control
        Me.lblMTCRatingRules.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMTCRatingRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMTCRatingRules.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMTCRatingRules.Location = New System.Drawing.Point(8, 64)
        Me.lblMTCRatingRules.Name = "lblMTCRatingRules"
        Me.lblMTCRatingRules.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMTCRatingRules.Size = New System.Drawing.Size(113, 25)
        Me.lblMTCRatingRules.TabIndex = 379
        Me.lblMTCRatingRules.Text = "MTC Rating Rules Enabled"
        '
        'lblMTAProRata
        '
        Me.lblMTAProRata.BackColor = System.Drawing.SystemColors.Control
        Me.lblMTAProRata.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMTAProRata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMTAProRata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMTAProRata.Location = New System.Drawing.Point(189, 13)
        Me.lblMTAProRata.Name = "lblMTAProRata"
        Me.lblMTAProRata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMTAProRata.Size = New System.Drawing.Size(91, 17)
        Me.lblMTAProRata.TabIndex = 47
        Me.lblMTAProRata.Text = "MTA Prorata"
        '
        'lblNBProRata
        '
        Me.lblNBProRata.BackColor = System.Drawing.SystemColors.Control
        Me.lblNBProRata.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNBProRata.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNBProRata.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNBProRata.Location = New System.Drawing.Point(8, 17)
        Me.lblNBProRata.Name = "lblNBProRata"
        Me.lblNBProRata.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNBProRata.Size = New System.Drawing.Size(91, 17)
        Me.lblNBProRata.TabIndex = 45
        Me.lblNBProRata.Text = "NB Prorata"
        '
        'lblShortPeriodRated
        '
        Me.lblShortPeriodRated.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortPeriodRated.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortPeriodRated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortPeriodRated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortPeriodRated.Location = New System.Drawing.Point(8, 41)
        Me.lblShortPeriodRated.Name = "lblShortPeriodRated"
        Me.lblShortPeriodRated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortPeriodRated.Size = New System.Drawing.Size(111, 17)
        Me.lblShortPeriodRated.TabIndex = 49
        Me.lblShortPeriodRated.Text = "Short Period Rated"
        '
        'fraRenewals
        '
        Me.fraRenewals.BackColor = System.Drawing.SystemColors.Control
        Me.fraRenewals.Controls.Add(Me.chkDisableCoverStartDateonREN)
        Me.fraRenewals.Controls.Add(Me.chkDefaultCovertoDatetolastday)
        Me.fraRenewals.Controls.Add(Me.chkDoNotDeleteRenewalQuoteOnMTA)
        Me.fraRenewals.Controls.Add(Me.chkDeleteRenewalQuoteReRunOnMTA)
        Me.fraRenewals.Controls.Add(Me.chkBindRenewalWOInvitation)
        Me.fraRenewals.Controls.Add(Me.chkUsePriorSchemeAtRenewal)
        Me.fraRenewals.Controls.Add(Me.chkUseNBRenPaymentTermsAtSelection)
        Me.fraRenewals.Controls.Add(Me.chkChangePolicyNumberAtRenewalAutomatically)
        Me.fraRenewals.Controls.Add(Me.txtdefaultRenMth)
        Me.fraRenewals.Controls.Add(Me.chkRenewable)
        Me.fraRenewals.Controls.Add(Me.chkHideSummaryAtRenewalAcceptance)
        Me.fraRenewals.Controls.Add(Me.chkChangePolicyNumberAtRenewal)
        Me.fraRenewals.Controls.Add(Me.chkMidNightRenewal)
        Me.fraRenewals.Controls.Add(Me.chkAutoRenewal)
        Me.fraRenewals.Controls.Add(Me.txtRenewalPeriod)
        Me.fraRenewals.Controls.Add(Me.Label6)
        Me.fraRenewals.Controls.Add(Me.lblAutoRenewal)
        Me.fraRenewals.Controls.Add(Me.lblRenewalPeriod)
        Me.fraRenewals.Controls.Add(Me.rbPolicyInceptionDate)
        Me.fraRenewals.Controls.Add(Me.rbEffectiveDate)
        Me.fraRenewals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRenewals.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRenewals.Location = New System.Drawing.Point(9, 460)
        Me.fraRenewals.Name = "fraRenewals"
        Me.fraRenewals.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRenewals.Size = New System.Drawing.Size(817, 151)
        Me.fraRenewals.TabIndex = 58
        Me.fraRenewals.TabStop = False
        Me.fraRenewals.Text = "Renewals"
        '
        'chkDisableCoverStartDateonREN
        '
        Me.chkDisableCoverStartDateonREN.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisableCoverStartDateonREN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisableCoverStartDateonREN.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisableCoverStartDateonREN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisableCoverStartDateonREN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisableCoverStartDateonREN.Location = New System.Drawing.Point(484, 125)
        Me.chkDisableCoverStartDateonREN.Name = "chkDisableCoverStartDateonREN"
        Me.chkDisableCoverStartDateonREN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisableCoverStartDateonREN.Size = New System.Drawing.Size(324, 19)
        Me.chkDisableCoverStartDateonREN.TabIndex = 390
        Me.chkDisableCoverStartDateonREN.Text = "Disable Cover Start Date on Renewal"
        Me.chkDisableCoverStartDateonREN.UseVisualStyleBackColor = False
        '
        'chkDoNotDeleteRenewalQuoteOnMTA
        '
        Me.chkDoNotDeleteRenewalQuoteOnMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkDoNotDeleteRenewalQuoteOnMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDoNotDeleteRenewalQuoteOnMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Location = New System.Drawing.Point(5, 125)
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Name = "chkDoNotDeleteRenewalQuoteOnMTA"
        Me.chkDoNotDeleteRenewalQuoteOnMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Size = New System.Drawing.Size(216, 19)
        Me.chkDoNotDeleteRenewalQuoteOnMTA.TabIndex = 388
        Me.chkDoNotDeleteRenewalQuoteOnMTA.Text = "Do not delete Renewal Quote on MTA"
        Me.chkDoNotDeleteRenewalQuoteOnMTA.UseVisualStyleBackColor = False
        '
        'chkDeleteRenewalQuoteReRunOnMTA
        '
        Me.chkDeleteRenewalQuoteReRunOnMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkDeleteRenewalQuoteReRunOnMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDeleteRenewalQuoteReRunOnMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDeleteRenewalQuoteReRunOnMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDeleteRenewalQuoteReRunOnMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDeleteRenewalQuoteReRunOnMTA.Location = New System.Drawing.Point(235, 125)
        Me.chkDeleteRenewalQuoteReRunOnMTA.Name = "chkDeleteRenewalQuoteReRunOnMTA"
        Me.chkDeleteRenewalQuoteReRunOnMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDeleteRenewalQuoteReRunOnMTA.Size = New System.Drawing.Size(240, 19)
        Me.chkDeleteRenewalQuoteReRunOnMTA.TabIndex = 389
        Me.chkDeleteRenewalQuoteReRunOnMTA.Text = "Delete and Auto Select Renewal Upon MTA"
        Me.chkDeleteRenewalQuoteReRunOnMTA.UseVisualStyleBackColor = False
        '
        'chkBindRenewalWOInvitation
        '
        Me.chkBindRenewalWOInvitation.BackColor = System.Drawing.SystemColors.Control
        Me.chkBindRenewalWOInvitation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkBindRenewalWOInvitation.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBindRenewalWOInvitation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBindRenewalWOInvitation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBindRenewalWOInvitation.Location = New System.Drawing.Point(5, 103)
        Me.chkBindRenewalWOInvitation.Name = "chkBindRenewalWOInvitation"
        Me.chkBindRenewalWOInvitation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBindRenewalWOInvitation.Size = New System.Drawing.Size(216, 19)
        Me.chkBindRenewalWOInvitation.TabIndex = 387
        Me.chkBindRenewalWOInvitation.Text = "Bind Manual Renewal without invitation"
        Me.chkBindRenewalWOInvitation.UseVisualStyleBackColor = False
        '
        'chkUsePriorSchemeAtRenewal
        '
        Me.chkUsePriorSchemeAtRenewal.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar
        Me.chkUsePriorSchemeAtRenewal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUsePriorSchemeAtRenewal.Location = New System.Drawing.Point(235, 100)
        Me.chkUsePriorSchemeAtRenewal.Name = "chkUsePriorSchemeAtRenewal"
        Me.chkUsePriorSchemeAtRenewal.Size = New System.Drawing.Size(240, 25)
        Me.chkUsePriorSchemeAtRenewal.TabIndex = 386
        Me.chkUsePriorSchemeAtRenewal.Text = "Always Use Prior Term Scheme At Renewal"
        Me.chkUsePriorSchemeAtRenewal.UseVisualStyleBackColor = True
        '
        'chkUseNBRenPaymentTermsAtSelection
        '
        Me.chkUseNBRenPaymentTermsAtSelection.BackColor = System.Drawing.SystemColors.Control
        Me.chkUseNBRenPaymentTermsAtSelection.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUseNBRenPaymentTermsAtSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUseNBRenPaymentTermsAtSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseNBRenPaymentTermsAtSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseNBRenPaymentTermsAtSelection.Location = New System.Drawing.Point(484, 100)
        Me.chkUseNBRenPaymentTermsAtSelection.Name = "chkUseNBRenPaymentTermsAtSelection"
        Me.chkUseNBRenPaymentTermsAtSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUseNBRenPaymentTermsAtSelection.Size = New System.Drawing.Size(324, 19)
        Me.chkUseNBRenPaymentTermsAtSelection.TabIndex = 41
        Me.chkUseNBRenPaymentTermsAtSelection.Text = "Use NB/Renewal Payment Terms At Selection"
        Me.chkUseNBRenPaymentTermsAtSelection.UseVisualStyleBackColor = False
        '
        'chkChangePolicyNumberAtRenewalAutomatically
        '
        Me.chkChangePolicyNumberAtRenewalAutomatically.BackColor = System.Drawing.SystemColors.Control
        Me.chkChangePolicyNumberAtRenewalAutomatically.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkChangePolicyNumberAtRenewalAutomatically.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkChangePolicyNumberAtRenewalAutomatically.Enabled = False
        Me.chkChangePolicyNumberAtRenewalAutomatically.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkChangePolicyNumberAtRenewalAutomatically.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkChangePolicyNumberAtRenewalAutomatically.Location = New System.Drawing.Point(484, 44)
        Me.chkChangePolicyNumberAtRenewalAutomatically.Name = "chkChangePolicyNumberAtRenewalAutomatically"
        Me.chkChangePolicyNumberAtRenewalAutomatically.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkChangePolicyNumberAtRenewalAutomatically.Size = New System.Drawing.Size(324, 19)
        Me.chkChangePolicyNumberAtRenewalAutomatically.TabIndex = 39
        Me.chkChangePolicyNumberAtRenewalAutomatically.Text = "Change Policy Number At Renewal Automatically"
        Me.chkChangePolicyNumberAtRenewalAutomatically.UseVisualStyleBackColor = False
        '
        'txtdefaultRenMth
        '
        Me.txtdefaultRenMth.AcceptsReturn = True
        Me.txtdefaultRenMth.BackColor = System.Drawing.SystemColors.Window
        Me.txtdefaultRenMth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtdefaultRenMth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtdefaultRenMth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtdefaultRenMth.Location = New System.Drawing.Point(196, 60)
        Me.txtdefaultRenMth.MaxLength = 0
        Me.txtdefaultRenMth.Name = "txtdefaultRenMth"
        Me.txtdefaultRenMth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtdefaultRenMth.Size = New System.Drawing.Size(41, 20)
        Me.txtdefaultRenMth.TabIndex = 35
        '
        'chkRenewable
        '
        Me.chkRenewable.BackColor = System.Drawing.SystemColors.Control
        Me.chkRenewable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRenewable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRenewable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRenewable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRenewable.Location = New System.Drawing.Point(309, 19)
        Me.chkRenewable.Name = "chkRenewable"
        Me.chkRenewable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRenewable.Size = New System.Drawing.Size(166, 19)
        Me.chkRenewable.TabIndex = 36
        Me.chkRenewable.Text = "Renewable"
        Me.chkRenewable.UseVisualStyleBackColor = False
        '
        'chkHideSummaryAtRenewalAcceptance
        '
        Me.chkHideSummaryAtRenewalAcceptance.BackColor = System.Drawing.SystemColors.Control
        Me.chkHideSummaryAtRenewalAcceptance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkHideSummaryAtRenewalAcceptance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHideSummaryAtRenewalAcceptance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHideSummaryAtRenewalAcceptance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHideSummaryAtRenewalAcceptance.Location = New System.Drawing.Point(484, 73)
        Me.chkHideSummaryAtRenewalAcceptance.Name = "chkHideSummaryAtRenewalAcceptance"
        Me.chkHideSummaryAtRenewalAcceptance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHideSummaryAtRenewalAcceptance.Size = New System.Drawing.Size(324, 19)
        Me.chkHideSummaryAtRenewalAcceptance.TabIndex = 40
        Me.chkHideSummaryAtRenewalAcceptance.Text = "Hide Summary At Renewal Acceptance"
        Me.chkHideSummaryAtRenewalAcceptance.UseVisualStyleBackColor = False
        '
        'chkChangePolicyNumberAtRenewal
        '
        Me.chkChangePolicyNumberAtRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkChangePolicyNumberAtRenewal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkChangePolicyNumberAtRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkChangePolicyNumberAtRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkChangePolicyNumberAtRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkChangePolicyNumberAtRenewal.Location = New System.Drawing.Point(484, 14)
        Me.chkChangePolicyNumberAtRenewal.Name = "chkChangePolicyNumberAtRenewal"
        Me.chkChangePolicyNumberAtRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkChangePolicyNumberAtRenewal.Size = New System.Drawing.Size(324, 19)
        Me.chkChangePolicyNumberAtRenewal.TabIndex = 38
        Me.chkChangePolicyNumberAtRenewal.Text = "Change Policy Number at Renewal"
        Me.chkChangePolicyNumberAtRenewal.UseVisualStyleBackColor = False
        '
        'chkMidNightRenewal
        '
        Me.chkMidNightRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkMidNightRenewal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkMidNightRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMidNightRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMidNightRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMidNightRenewal.Location = New System.Drawing.Point(309, 44)
        Me.chkMidNightRenewal.Name = "chkMidNightRenewal"
        Me.chkMidNightRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMidNightRenewal.Size = New System.Drawing.Size(166, 19)
        Me.chkMidNightRenewal.TabIndex = 37
        Me.chkMidNightRenewal.Text = "Midnight Renewal"
        Me.chkMidNightRenewal.UseVisualStyleBackColor = False
        '
        'chkAutoRenewal
        '
        Me.chkAutoRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoRenewal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAutoRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoRenewal.Location = New System.Drawing.Point(6, 39)
        Me.chkAutoRenewal.Name = "chkAutoRenewal"
        Me.chkAutoRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoRenewal.Size = New System.Drawing.Size(205, 19)
        Me.chkAutoRenewal.TabIndex = 34
        Me.chkAutoRenewal.Text = "Auto Renewal"
        Me.chkAutoRenewal.UseVisualStyleBackColor = False
        '
        'txtRenewalPeriod
        '
        Me.txtRenewalPeriod.AcceptsReturn = True
        Me.txtRenewalPeriod.BackColor = System.Drawing.SystemColors.Window
        Me.txtRenewalPeriod.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRenewalPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRenewalPeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRenewalPeriod.Location = New System.Drawing.Point(196, 16)
        Me.txtRenewalPeriod.MaxLength = 0
        Me.txtRenewalPeriod.Name = "txtRenewalPeriod"
        Me.txtRenewalPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRenewalPeriod.Size = New System.Drawing.Size(41, 20)
        Me.txtRenewalPeriod.TabIndex = 33
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(139, 13)
        Me.Label6.TabIndex = 385
        Me.Label6.Text = "Default Months for Renewal"
        '
        'lblAutoRenewal
        '
        Me.lblAutoRenewal.AutoSize = True
        Me.lblAutoRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.lblAutoRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAutoRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAutoRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAutoRenewal.Location = New System.Drawing.Point(24, 42)
        Me.lblAutoRenewal.Name = "lblAutoRenewal"
        Me.lblAutoRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAutoRenewal.Size = New System.Drawing.Size(74, 13)
        Me.lblAutoRenewal.TabIndex = 366
        Me.lblAutoRenewal.Text = "Auto Renewal"
        '
        'lblRenewalPeriod
        '
        Me.lblRenewalPeriod.AutoSize = True
        Me.lblRenewalPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblRenewalPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalPeriod.Location = New System.Drawing.Point(8, 19)
        Me.lblRenewalPeriod.Name = "lblRenewalPeriod"
        Me.lblRenewalPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalPeriod.Size = New System.Drawing.Size(82, 13)
        Me.lblRenewalPeriod.TabIndex = 59
        Me.lblRenewalPeriod.Text = "Renewal Period"
        '
        'rbPolicyInceptionDate
        '
        Me.rbPolicyInceptionDate.AutoSize = True
        Me.rbPolicyInceptionDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbPolicyInceptionDate.Location = New System.Drawing.Point(117, 84)
        Me.rbPolicyInceptionDate.Name = "rbPolicyInceptionDate"
        Me.rbPolicyInceptionDate.Size = New System.Drawing.Size(126, 17)
        Me.rbPolicyInceptionDate.TabIndex = 389
        Me.rbPolicyInceptionDate.Text = "Policy Inception Date"
        Me.rbPolicyInceptionDate.UseVisualStyleBackColor = False
        '
        'rbEffectiveDate
        '
        Me.rbEffectiveDate.AutoSize = True
        Me.rbEffectiveDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbEffectiveDate.Checked = True
        Me.rbEffectiveDate.Location = New System.Drawing.Point(7, 84)
        Me.rbEffectiveDate.Name = "rbEffectiveDate"
        Me.rbEffectiveDate.Size = New System.Drawing.Size(93, 17)
        Me.rbEffectiveDate.TabIndex = 388
        Me.rbEffectiveDate.TabStop = True
        Me.rbEffectiveDate.Text = "Effective Date"
        Me.rbEffectiveDate.UseVisualStyleBackColor = False
        '
        'fraPolicyCreation
        '
        Me.fraPolicyCreation.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicyCreation.Controls.Add(Me.chkRetainPolicyNumber)
        Me.fraPolicyCreation.Controls.Add(Me.chkEnablePrePayment)
        Me.fraPolicyCreation.Controls.Add(Me.chkWrittenPolicy)
        Me.fraPolicyCreation.Controls.Add(Me.chkReinsuranceManualPremiumAdjustment)
        Me.fraPolicyCreation.Controls.Add(Me.cboCNAutoNumberingID)
        Me.fraPolicyCreation.Controls.Add(Me.chkTrueMonthlyPolicy)
        Me.fraPolicyCreation.Controls.Add(Me.txtGracePeriod)
        Me.fraPolicyCreation.Controls.Add(Me.chkPolicyNumberAtQuote)
        Me.fraPolicyCreation.Controls.Add(Me.cboPolicyAutoNumberingID)
        Me.fraPolicyCreation.Controls.Add(Me.cboQuoteAutoNumberingID)
        Me.fraPolicyCreation.Controls.Add(Me.chkPolicyStyleMandatory)
        Me.fraPolicyCreation.Controls.Add(Me.cboPolicyStyle)
        Me.fraPolicyCreation.Controls.Add(Me.lbCNNumbering)
        Me.fraPolicyCreation.Controls.Add(Me.lblGracePeriod)
        Me.fraPolicyCreation.Controls.Add(Me.lblPolicyNumberAtQuote)
        Me.fraPolicyCreation.Controls.Add(Me.lblPolicyAutoNumberingID)
        Me.fraPolicyCreation.Controls.Add(Me.lblQuoteAutoNumberingID)
        Me.fraPolicyCreation.Controls.Add(Me.lblPolicyStyle)
        Me.fraPolicyCreation.Controls.Add(Me.lblPolicyStyleMandatory)
        Me.fraPolicyCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicyCreation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicyCreation.Location = New System.Drawing.Point(8, 170)
        Me.fraPolicyCreation.Name = "fraPolicyCreation"
        Me.fraPolicyCreation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicyCreation.Size = New System.Drawing.Size(572, 176)
        Me.fraPolicyCreation.TabIndex = 17
        Me.fraPolicyCreation.TabStop = False
        Me.fraPolicyCreation.Text = "Policy Creation"
        '
        'chkRetainPolicyNumber
        '
        Me.chkRetainPolicyNumber.BackColor = System.Drawing.SystemColors.Control
        Me.chkRetainPolicyNumber.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRetainPolicyNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRetainPolicyNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRetainPolicyNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRetainPolicyNumber.Location = New System.Drawing.Point(8, 150)
        Me.chkRetainPolicyNumber.Name = "chkRetainPolicyNumber"
        Me.chkRetainPolicyNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRetainPolicyNumber.Size = New System.Drawing.Size(227, 20)
        Me.chkRetainPolicyNumber.TabIndex = 70
        Me.chkRetainPolicyNumber.Text = "Retain Policy Number on Copy "
        Me.chkRetainPolicyNumber.UseVisualStyleBackColor = False
        '
        'chkEnablePrePayment
        '
        Me.chkEnablePrePayment.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnablePrePayment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEnablePrePayment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnablePrePayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnablePrePayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnablePrePayment.Location = New System.Drawing.Point(341, 147)
        Me.chkEnablePrePayment.Name = "chkEnablePrePayment"
        Me.chkEnablePrePayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnablePrePayment.Size = New System.Drawing.Size(227, 20)
        Me.chkEnablePrePayment.TabIndex = 69
        Me.chkEnablePrePayment.Text = "Enable PrePayment      "
        Me.chkEnablePrePayment.UseVisualStyleBackColor = False
        '
        'chkWrittenPolicy
        '
        Me.chkWrittenPolicy.AutoSize = True
        Me.chkWrittenPolicy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkWrittenPolicy.Location = New System.Drawing.Point(343, 123)
        Me.chkWrittenPolicy.Name = "chkWrittenPolicy"
        Me.chkWrittenPolicy.Size = New System.Drawing.Size(226, 17)
        Me.chkWrittenPolicy.TabIndex = 67
        Me.chkWrittenPolicy.Text = "Written Policy                                             "
        Me.chkWrittenPolicy.UseVisualStyleBackColor = True
        '
        'chkReinsuranceManualPremiumAdjustment
        '
        Me.chkReinsuranceManualPremiumAdjustment.BackColor = System.Drawing.SystemColors.Control
        Me.chkReinsuranceManualPremiumAdjustment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReinsuranceManualPremiumAdjustment.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReinsuranceManualPremiumAdjustment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReinsuranceManualPremiumAdjustment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReinsuranceManualPremiumAdjustment.Location = New System.Drawing.Point(341, 88)
        Me.chkReinsuranceManualPremiumAdjustment.Name = "chkReinsuranceManualPremiumAdjustment"
        Me.chkReinsuranceManualPremiumAdjustment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReinsuranceManualPremiumAdjustment.Size = New System.Drawing.Size(224, 29)
        Me.chkReinsuranceManualPremiumAdjustment.TabIndex = 18
        Me.chkReinsuranceManualPremiumAdjustment.Text = "Reinsurance Manual Premium Adjustment"
        Me.chkReinsuranceManualPremiumAdjustment.UseVisualStyleBackColor = False
        '
        'chkTrueMonthlyPolicy
        '
        Me.chkTrueMonthlyPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.chkTrueMonthlyPolicy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTrueMonthlyPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTrueMonthlyPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrueMonthlyPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTrueMonthlyPolicy.Location = New System.Drawing.Point(343, 69)
        Me.chkTrueMonthlyPolicy.Name = "chkTrueMonthlyPolicy"
        Me.chkTrueMonthlyPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTrueMonthlyPolicy.Size = New System.Drawing.Size(223, 17)
        Me.chkTrueMonthlyPolicy.TabIndex = 17
        Me.chkTrueMonthlyPolicy.Text = "True Monthly Policy"
        Me.chkTrueMonthlyPolicy.UseVisualStyleBackColor = False
        '
        'txtGracePeriod
        '
        Me.txtGracePeriod.AcceptsReturn = True
        Me.txtGracePeriod.BackColor = System.Drawing.SystemColors.Window
        Me.txtGracePeriod.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGracePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGracePeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGracePeriod.Location = New System.Drawing.Point(157, 100)
        Me.txtGracePeriod.MaxLength = 3
        Me.txtGracePeriod.Name = "txtGracePeriod"
        Me.txtGracePeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGracePeriod.Size = New System.Drawing.Size(41, 20)
        Me.txtGracePeriod.TabIndex = 13
        '
        'chkPolicyNumberAtQuote
        '
        Me.chkPolicyNumberAtQuote.BackColor = System.Drawing.SystemColors.Control
        Me.chkPolicyNumberAtQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPolicyNumberAtQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPolicyNumberAtQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPolicyNumberAtQuote.Location = New System.Drawing.Point(552, 41)
        Me.chkPolicyNumberAtQuote.Name = "chkPolicyNumberAtQuote"
        Me.chkPolicyNumberAtQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPolicyNumberAtQuote.Size = New System.Drawing.Size(15, 19)
        Me.chkPolicyNumberAtQuote.TabIndex = 16
        Me.chkPolicyNumberAtQuote.UseVisualStyleBackColor = False
        '
        'chkPolicyStyleMandatory
        '
        Me.chkPolicyStyleMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkPolicyStyleMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPolicyStyleMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPolicyStyleMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPolicyStyleMandatory.Location = New System.Drawing.Point(552, 13)
        Me.chkPolicyStyleMandatory.Name = "chkPolicyStyleMandatory"
        Me.chkPolicyStyleMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPolicyStyleMandatory.Size = New System.Drawing.Size(16, 19)
        Me.chkPolicyStyleMandatory.TabIndex = 15
        Me.chkPolicyStyleMandatory.UseVisualStyleBackColor = False
        '
        'cboPolicyStyle
        '
        Me.cboPolicyStyle.DefaultItemId = 0
        Me.cboPolicyStyle.FirstItem = ""
        Me.cboPolicyStyle.ItemId = 0
        Me.cboPolicyStyle.ListIndex = -1
        Me.cboPolicyStyle.Location = New System.Drawing.Point(157, 16)
        Me.cboPolicyStyle.Name = "cboPolicyStyle"
        Me.cboPolicyStyle.PMLookupProductFamily = 9
        Me.cboPolicyStyle.SingleItemId = 0
        Me.cboPolicyStyle.Size = New System.Drawing.Size(173, 21)
        Me.cboPolicyStyle.SortColumnName = ""
        Me.cboPolicyStyle.Sorted = True
        Me.cboPolicyStyle.TabIndex = 10
        Me.cboPolicyStyle.TableName = "Policy_Style"
        Me.cboPolicyStyle.ToolTipText = ""
        Me.cboPolicyStyle.WhereClause = ""
        '
        'lbCNNumbering
        '
        Me.lbCNNumbering.AutoSize = True
        Me.lbCNNumbering.BackColor = System.Drawing.SystemColors.Control
        Me.lbCNNumbering.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbCNNumbering.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCNNumbering.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbCNNumbering.Location = New System.Drawing.Point(8, 128)
        Me.lbCNNumbering.Name = "lbCNNumbering"
        Me.lbCNNumbering.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbCNNumbering.Size = New System.Drawing.Size(115, 13)
        Me.lbCNNumbering.TabIndex = 66
        Me.lbCNNumbering.Text = "Cover Note Numbering"
        '
        'lblGracePeriod
        '
        Me.lblGracePeriod.AutoSize = True
        Me.lblGracePeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblGracePeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGracePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGracePeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGracePeriod.Location = New System.Drawing.Point(8, 103)
        Me.lblGracePeriod.Name = "lblGracePeriod"
        Me.lblGracePeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGracePeriod.Size = New System.Drawing.Size(101, 13)
        Me.lblGracePeriod.TabIndex = 66
        Me.lblGracePeriod.Text = "Quote Expiry (days):"
        '
        'lblPolicyNumberAtQuote
        '
        Me.lblPolicyNumberAtQuote.AutoSize = True
        Me.lblPolicyNumberAtQuote.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyNumberAtQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyNumberAtQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyNumberAtQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyNumberAtQuote.Location = New System.Drawing.Point(346, 42)
        Me.lblPolicyNumberAtQuote.Name = "lblPolicyNumberAtQuote"
        Me.lblPolicyNumberAtQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyNumberAtQuote.Size = New System.Drawing.Size(119, 13)
        Me.lblPolicyNumberAtQuote.TabIndex = 66
        Me.lblPolicyNumberAtQuote.Text = "Policy Number at Quote"
        Me.lblPolicyNumberAtQuote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPolicyAutoNumberingID
        '
        Me.lblPolicyAutoNumberingID.AutoSize = True
        Me.lblPolicyAutoNumberingID.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyAutoNumberingID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyAutoNumberingID.Location = New System.Drawing.Point(8, 76)
        Me.lblPolicyAutoNumberingID.Name = "lblPolicyAutoNumberingID"
        Me.lblPolicyAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyAutoNumberingID.Size = New System.Drawing.Size(89, 13)
        Me.lblPolicyAutoNumberingID.TabIndex = 66
        Me.lblPolicyAutoNumberingID.Text = "Policy Numbering"
        '
        'lblQuoteAutoNumberingID
        '
        Me.lblQuoteAutoNumberingID.AutoSize = True
        Me.lblQuoteAutoNumberingID.BackColor = System.Drawing.SystemColors.Control
        Me.lblQuoteAutoNumberingID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblQuoteAutoNumberingID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQuoteAutoNumberingID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblQuoteAutoNumberingID.Location = New System.Drawing.Point(8, 48)
        Me.lblQuoteAutoNumberingID.Name = "lblQuoteAutoNumberingID"
        Me.lblQuoteAutoNumberingID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblQuoteAutoNumberingID.Size = New System.Drawing.Size(90, 13)
        Me.lblQuoteAutoNumberingID.TabIndex = 66
        Me.lblQuoteAutoNumberingID.Text = "Quote Numbering"
        '
        'lblPolicyStyle
        '
        Me.lblPolicyStyle.AutoSize = True
        Me.lblPolicyStyle.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStyle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStyle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStyle.Location = New System.Drawing.Point(8, 20)
        Me.lblPolicyStyle.Name = "lblPolicyStyle"
        Me.lblPolicyStyle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStyle.Size = New System.Drawing.Size(101, 13)
        Me.lblPolicyStyle.TabIndex = 66
        Me.lblPolicyStyle.Text = "Default Policy Style:"
        '
        'lblPolicyStyleMandatory
        '
        Me.lblPolicyStyleMandatory.AutoSize = True
        Me.lblPolicyStyleMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyStyleMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyStyleMandatory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyStyleMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyStyleMandatory.Location = New System.Drawing.Point(348, 16)
        Me.lblPolicyStyleMandatory.Name = "lblPolicyStyleMandatory"
        Me.lblPolicyStyleMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyStyleMandatory.Size = New System.Drawing.Size(125, 13)
        Me.lblPolicyStyleMandatory.TabIndex = 66
        Me.lblPolicyStyleMandatory.Text = "Policy Style Is Mandatory"
        Me.lblPolicyStyleMandatory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'fraDetails
        '
        Me.fraDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraDetails.Controls.Add(Me.txtBlockNo)
        Me.fraDetails.Controls.Add(Me.txtRIPointer)
        Me.fraDetails.Controls.Add(Me.txtReportPointer)
        Me.fraDetails.Controls.Add(Me.txtSchemeAgencyRef)
        Me.fraDetails.Controls.Add(Me.txtDescription)
        Me.fraDetails.Controls.Add(Me.txtEffectiveDate)
        Me.fraDetails.Controls.Add(Me.txtCode)
        Me.fraDetails.Controls.Add(Me.cboRiskTypeGroup)
        Me.fraDetails.Controls.Add(Me.lblBlockNo)
        Me.fraDetails.Controls.Add(Me.lblRIPointer)
        Me.fraDetails.Controls.Add(Me.lblReportPointer)
        Me.fraDetails.Controls.Add(Me.lblRiskTypeGroup)
        Me.fraDetails.Controls.Add(Me.lblSchemeAgencyRef)
        Me.fraDetails.Controls.Add(Me.lblDescription)
        Me.fraDetails.Controls.Add(Me.lblEffectiveDate)
        Me.fraDetails.Controls.Add(Me.lblCode)
        Me.fraDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDetails.Location = New System.Drawing.Point(8, 8)
        Me.fraDetails.Name = "fraDetails"
        Me.fraDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDetails.Size = New System.Drawing.Size(820, 157)
        Me.fraDetails.TabIndex = 0
        Me.fraDetails.TabStop = False
        Me.fraDetails.Text = "Details"
        '
        'txtBlockNo
        '
        Me.txtBlockNo.AcceptsReturn = True
        Me.txtBlockNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtBlockNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBlockNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBlockNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBlockNo.Location = New System.Drawing.Point(584, 104)
        Me.txtBlockNo.MaxLength = 0
        Me.txtBlockNo.Multiline = True
        Me.txtBlockNo.Name = "txtBlockNo"
        Me.txtBlockNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBlockNo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBlockNo.Size = New System.Drawing.Size(211, 37)
        Me.txtBlockNo.TabIndex = 9
        '
        'txtRIPointer
        '
        Me.txtRIPointer.AcceptsReturn = True
        Me.txtRIPointer.BackColor = System.Drawing.SystemColors.Window
        Me.txtRIPointer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRIPointer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRIPointer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRIPointer.Location = New System.Drawing.Point(128, 99)
        Me.txtRIPointer.MaxLength = 0
        Me.txtRIPointer.Name = "txtRIPointer"
        Me.txtRIPointer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRIPointer.Size = New System.Drawing.Size(41, 20)
        Me.txtRIPointer.TabIndex = 5
        '
        'txtReportPointer
        '
        Me.txtReportPointer.AcceptsReturn = True
        Me.txtReportPointer.BackColor = System.Drawing.SystemColors.Window
        Me.txtReportPointer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReportPointer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReportPointer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReportPointer.Location = New System.Drawing.Point(128, 127)
        Me.txtReportPointer.MaxLength = 0
        Me.txtReportPointer.Name = "txtReportPointer"
        Me.txtReportPointer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReportPointer.Size = New System.Drawing.Size(41, 20)
        Me.txtReportPointer.TabIndex = 6
        '
        'txtSchemeAgencyRef
        '
        Me.txtSchemeAgencyRef.AcceptsReturn = True
        Me.txtSchemeAgencyRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtSchemeAgencyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSchemeAgencyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSchemeAgencyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSchemeAgencyRef.Location = New System.Drawing.Point(584, 60)
        Me.txtSchemeAgencyRef.MaxLength = 0
        Me.txtSchemeAgencyRef.Multiline = True
        Me.txtSchemeAgencyRef.Name = "txtSchemeAgencyRef"
        Me.txtSchemeAgencyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSchemeAgencyRef.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSchemeAgencyRef.Size = New System.Drawing.Size(211, 37)
        Me.txtSchemeAgencyRef.TabIndex = 8
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(584, 16)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(211, 37)
        Me.txtDescription.TabIndex = 7
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(128, 42)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(173, 20)
        Me.txtEffectiveDate.TabIndex = 3
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(128, 16)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(173, 20)
        Me.txtCode.TabIndex = 2
        '
        'cboRiskTypeGroup
        '
        Me.cboRiskTypeGroup.DefaultItemId = 0
        Me.cboRiskTypeGroup.FirstItem = ""
        Me.cboRiskTypeGroup.ItemId = 0
        Me.cboRiskTypeGroup.ListIndex = -1
        Me.cboRiskTypeGroup.Location = New System.Drawing.Point(128, 68)
        Me.cboRiskTypeGroup.Name = "cboRiskTypeGroup"
        Me.cboRiskTypeGroup.PMLookupProductFamily = 9
        Me.cboRiskTypeGroup.SingleItemId = 0
        Me.cboRiskTypeGroup.Size = New System.Drawing.Size(173, 21)
        Me.cboRiskTypeGroup.SortColumnName = ""
        Me.cboRiskTypeGroup.Sorted = True
        Me.cboRiskTypeGroup.TabIndex = 4
        Me.cboRiskTypeGroup.TableName = "Risk_type_group"
        Me.cboRiskTypeGroup.ToolTipText = ""
        Me.cboRiskTypeGroup.WhereClause = ""
        '
        'lblBlockNo
        '
        Me.lblBlockNo.AutoSize = True
        Me.lblBlockNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBlockNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBlockNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBlockNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBlockNo.Location = New System.Drawing.Point(440, 114)
        Me.lblBlockNo.Name = "lblBlockNo"
        Me.lblBlockNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBlockNo.Size = New System.Drawing.Size(54, 13)
        Me.lblBlockNo.TabIndex = 15
        Me.lblBlockNo.Text = "Block No."
        '
        'lblRIPointer
        '
        Me.lblRIPointer.AutoSize = True
        Me.lblRIPointer.BackColor = System.Drawing.SystemColors.Control
        Me.lblRIPointer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRIPointer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRIPointer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRIPointer.Location = New System.Drawing.Point(8, 102)
        Me.lblRIPointer.Name = "lblRIPointer"
        Me.lblRIPointer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRIPointer.Size = New System.Drawing.Size(59, 13)
        Me.lblRIPointer.TabIndex = 7
        Me.lblRIPointer.Text = "R/I Pointer"
        '
        'lblReportPointer
        '
        Me.lblReportPointer.AutoSize = True
        Me.lblReportPointer.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportPointer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportPointer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportPointer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportPointer.Location = New System.Drawing.Point(8, 130)
        Me.lblReportPointer.Name = "lblReportPointer"
        Me.lblReportPointer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportPointer.Size = New System.Drawing.Size(75, 13)
        Me.lblReportPointer.TabIndex = 9
        Me.lblReportPointer.Text = "Report Pointer"
        '
        'lblRiskTypeGroup
        '
        Me.lblRiskTypeGroup.AutoSize = True
        Me.lblRiskTypeGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskTypeGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskTypeGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskTypeGroup.Location = New System.Drawing.Point(8, 72)
        Me.lblRiskTypeGroup.Name = "lblRiskTypeGroup"
        Me.lblRiskTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskTypeGroup.Size = New System.Drawing.Size(117, 13)
        Me.lblRiskTypeGroup.TabIndex = 5
        Me.lblRiskTypeGroup.Text = "Risk Type Group:"
        '
        'lblSchemeAgencyRef
        '
        Me.lblSchemeAgencyRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblSchemeAgencyRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSchemeAgencyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSchemeAgencyRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSchemeAgencyRef.Location = New System.Drawing.Point(440, 66)
        Me.lblSchemeAgencyRef.Name = "lblSchemeAgencyRef"
        Me.lblSchemeAgencyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSchemeAgencyRef.Size = New System.Drawing.Size(123, 25)
        Me.lblSchemeAgencyRef.TabIndex = 13
        Me.lblSchemeAgencyRef.Text = "Scheme Agency Ref"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(440, 26)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(60, 13)
        Me.lblDescription.TabIndex = 11
        Me.lblDescription.Text = "Description"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(8, 45)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(75, 13)
        Me.lblEffectiveDate.TabIndex = 3
        Me.lblEffectiveDate.Text = "Effective Date"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(8, 19)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(32, 13)
        Me.lblCode.TabIndex = 1
        Me.lblCode.Text = "Code"
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
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage7)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage8)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage9)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage10)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(83, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 7)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(850, 700)
        Me.tabMainTab.TabIndex = 179
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox5)
        Me.TabPage1.Controls.Add(Me.GroupBox6)
        Me.TabPage1.Location = New System.Drawing.Point(4, 40)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(836, 624)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "1-Product"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(485, 333)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(341, 101)
        Me.GroupBox1.TabIndex = 52
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Other Options"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(248, 44)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button1.Size = New System.Drawing.Size(84, 20)
        Me.Button1.TabIndex = 32
        Me.Button1.TabStop = False
        Me.Button1.Text = "&RI Model"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'CheckBox1
        '
        Me.CheckBox1.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox1.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox1.Location = New System.Drawing.Point(200, 44)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox1.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox1.TabIndex = 31
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(8, 20)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(157, 13)
        Me.Label8.TabIndex = 53
        Me.Label8.Text = "Positive Values in Cancel Policy"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(8, 47)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(142, 13)
        Me.Label9.TabIndex = 55
        Me.Label9.Text = "Allow Standard Wording Edit"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox2.Controls.Add(Me.CheckBox2)
        Me.GroupBox2.Controls.Add(Me.CheckBox3)
        Me.GroupBox2.Controls.Add(Me.CheckBox4)
        Me.GroupBox2.Controls.Add(Me.CheckBox5)
        Me.GroupBox2.Controls.Add(Me.CheckBox6)
        Me.GroupBox2.Controls.Add(Me.CboPMLookup1)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox2.Location = New System.Drawing.Point(587, 173)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox2.Size = New System.Drawing.Size(241, 157)
        Me.GroupBox2.TabIndex = 33
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Premium"
        '
        'CheckBox2
        '
        Me.CheckBox2.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox2.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox2.Location = New System.Drawing.Point(8, 128)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox2.Size = New System.Drawing.Size(225, 25)
        Me.CheckBox2.TabIndex = 24
        Me.CheckBox2.Text = "Gross Total Round Off upto 0 Decimals"
        Me.CheckBox2.UseVisualStyleBackColor = False
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox3.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox3.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox3.Size = New System.Drawing.Size(305, 17)
        Me.CheckBox3.TabIndex = 0
        Me.CheckBox3.Text = "Display Rerate option for Cancellations and Reinstatements"
        Me.CheckBox3.UseVisualStyleBackColor = False
        '
        'CheckBox4
        '
        Me.CheckBox4.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox4.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox4.Location = New System.Drawing.Point(218, 56)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox4.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox4.TabIndex = 21
        Me.CheckBox4.UseVisualStyleBackColor = False
        '
        'CheckBox5
        '
        Me.CheckBox5.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox5.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox5.Location = New System.Drawing.Point(218, 36)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox5.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox5.TabIndex = 20
        Me.CheckBox5.UseVisualStyleBackColor = False
        '
        'CheckBox6
        '
        Me.CheckBox6.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox6.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox6.Location = New System.Drawing.Point(218, 16)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox6.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox6.TabIndex = 19
        Me.CheckBox6.UseVisualStyleBackColor = False
        '
        'CboPMLookup1
        '
        Me.CboPMLookup1.DefaultItemId = 0
        Me.CboPMLookup1.Enabled = False
        Me.CboPMLookup1.FirstItem = ""
        Me.CboPMLookup1.ItemId = 0
        Me.CboPMLookup1.ListIndex = -1
        Me.CboPMLookup1.Location = New System.Drawing.Point(126, 100)
        Me.CboPMLookup1.Name = "CboPMLookup1"
        Me.CboPMLookup1.PMLookupProductFamily = 9
        Me.CboPMLookup1.SingleItemId = 0
        Me.CboPMLookup1.Size = New System.Drawing.Size(108, 21)
        Me.CboPMLookup1.SortColumnName = ""
        Me.CboPMLookup1.Sorted = True
        Me.CboPMLookup1.TabIndex = 23
        Me.CboPMLookup1.TableName = "rating_section_type"
        Me.CboPMLookup1.ToolTipText = ""
        Me.CboPMLookup1.WhereClause = ""
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(8, 102)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(112, 25)
        Me.Label10.TabIndex = 544545
        Me.Label10.Text = "Rounding Section:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(8, 81)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(82, 13)
        Me.Label11.TabIndex = 66
        Me.Label11.Text = "Round Premium"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(8, 59)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(89, 13)
        Me.Label12.TabIndex = 66
        Me.Label12.Text = "Currency Change"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(8, 19)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(71, 13)
        Me.Label13.TabIndex = 66
        Me.Label13.Text = "Accumulation"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(8, 39)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(84, 13)
        Me.Label14.TabIndex = 66
        Me.Label14.Text = "Tax Suppressed"
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox3.Controls.Add(Me.CheckBox7)
        Me.GroupBox3.Controls.Add(Me.Button2)
        Me.GroupBox3.Controls.Add(Me.CheckBox8)
        Me.GroupBox3.Controls.Add(Me.CheckBox9)
        Me.GroupBox3.Controls.Add(Me.CheckBox10)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.Label16)
        Me.GroupBox3.Controls.Add(Me.Label17)
        Me.GroupBox3.Controls.Add(Me.Label18)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox3.Location = New System.Drawing.Point(8, 333)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox3.Size = New System.Drawing.Size(468, 101)
        Me.GroupBox3.TabIndex = 44
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Pro-Rata"
        '
        'CheckBox7
        '
        Me.CheckBox7.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox7.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox7.Location = New System.Drawing.Point(124, 64)
        Me.CheckBox7.Name = "CheckBox7"
        Me.CheckBox7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox7.Size = New System.Drawing.Size(17, 17)
        Me.CheckBox7.TabIndex = 27
        Me.CheckBox7.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button2.Location = New System.Drawing.Point(189, 34)
        Me.Button2.Name = "Button2"
        Me.Button2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button2.Size = New System.Drawing.Size(117, 22)
        Me.Button2.TabIndex = 29
        Me.Button2.Text = "&Short Period Rate"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button2.UseVisualStyleBackColor = False
        '
        'CheckBox8
        '
        Me.CheckBox8.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox8.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox8.Location = New System.Drawing.Point(290, 12)
        Me.CheckBox8.Name = "CheckBox8"
        Me.CheckBox8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox8.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox8.TabIndex = 28
        Me.CheckBox8.UseVisualStyleBackColor = False
        '
        'CheckBox9
        '
        Me.CheckBox9.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox9.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox9.Location = New System.Drawing.Point(124, 16)
        Me.CheckBox9.Name = "CheckBox9"
        Me.CheckBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox9.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox9.TabIndex = 25
        Me.CheckBox9.UseVisualStyleBackColor = False
        '
        'CheckBox10
        '
        Me.CheckBox10.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox10.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox10.Location = New System.Drawing.Point(124, 40)
        Me.CheckBox10.Name = "CheckBox10"
        Me.CheckBox10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox10.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox10.TabIndex = 26
        Me.CheckBox10.UseVisualStyleBackColor = False
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label15.Location = New System.Drawing.Point(8, 64)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label15.Size = New System.Drawing.Size(113, 25)
        Me.Label15.TabIndex = 379
        Me.Label15.Text = "MTC Rating Rules Enabled"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.SystemColors.Control
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label16.Location = New System.Drawing.Point(189, 13)
        Me.Label16.Name = "Label16"
        Me.Label16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label16.Size = New System.Drawing.Size(91, 17)
        Me.Label16.TabIndex = 47
        Me.Label16.Text = "MTA Prorata"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.SystemColors.Control
        Me.Label17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label17.Location = New System.Drawing.Point(8, 17)
        Me.Label17.Name = "Label17"
        Me.Label17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label17.Size = New System.Drawing.Size(91, 17)
        Me.Label17.TabIndex = 45
        Me.Label17.Text = "NB Prorata"
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.SystemColors.Control
        Me.Label18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label18.Location = New System.Drawing.Point(8, 41)
        Me.Label18.Name = "Label18"
        Me.Label18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label18.Size = New System.Drawing.Size(111, 17)
        Me.Label18.TabIndex = 49
        Me.Label18.Text = "Short Period Rated"
        '
        'GroupBox4
        '
        Me.GroupBox4.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox4.Controls.Add(Me.CheckBox11)
        Me.GroupBox4.Controls.Add(Me.CheckBox12)
        Me.GroupBox4.Controls.Add(Me.CheckBox13)
        Me.GroupBox4.Controls.Add(Me.TextBox1)
        Me.GroupBox4.Controls.Add(Me.CheckBox14)
        Me.GroupBox4.Controls.Add(Me.CheckBox15)
        Me.GroupBox4.Controls.Add(Me.CheckBox16)
        Me.GroupBox4.Controls.Add(Me.CheckBox17)
        Me.GroupBox4.Controls.Add(Me.CheckBox18)
        Me.GroupBox4.Controls.Add(Me.TextBox2)
        Me.GroupBox4.Controls.Add(Me.Label19)
        Me.GroupBox4.Controls.Add(Me.Label20)
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox4.Location = New System.Drawing.Point(8, 436)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox4.Size = New System.Drawing.Size(817, 137)
        Me.GroupBox4.TabIndex = 58
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Renewals"
        '
        'CheckBox11
        '
        Me.CheckBox11.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox11.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox11.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox11.Location = New System.Drawing.Point(7, 99)
        Me.CheckBox11.Name = "CheckBox11"
        Me.CheckBox11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox11.Size = New System.Drawing.Size(216, 19)
        Me.CheckBox11.TabIndex = 386
        Me.CheckBox11.Text = "Bind Manual Renewal without invitation"
        Me.CheckBox11.UseVisualStyleBackColor = False
        '
        'CheckBox12
        '
        Me.CheckBox12.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox12.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox12.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox12.Location = New System.Drawing.Point(484, 100)
        Me.CheckBox12.Name = "CheckBox12"
        Me.CheckBox12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox12.Size = New System.Drawing.Size(324, 19)
        Me.CheckBox12.TabIndex = 41
        Me.CheckBox12.Text = "Use NB/Renewal Payment Terms At Selection"
        Me.CheckBox12.UseVisualStyleBackColor = False
        '
        'CheckBox13
        '
        Me.CheckBox13.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox13.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox13.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox13.Enabled = False
        Me.CheckBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox13.Location = New System.Drawing.Point(484, 44)
        Me.CheckBox13.Name = "CheckBox13"
        Me.CheckBox13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox13.Size = New System.Drawing.Size(324, 19)
        Me.CheckBox13.TabIndex = 39
        Me.CheckBox13.Text = "Change Policy Number At Renewal Automatically"
        Me.CheckBox13.UseVisualStyleBackColor = False
        '
        'TextBox1
        '
        Me.TextBox1.AcceptsReturn = True
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox1.Location = New System.Drawing.Point(209, 71)
        Me.TextBox1.MaxLength = 0
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox1.Size = New System.Drawing.Size(41, 20)
        Me.TextBox1.TabIndex = 35
        '
        'CheckBox14
        '
        Me.CheckBox14.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox14.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox14.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox14.Location = New System.Drawing.Point(295, 17)
        Me.CheckBox14.Name = "CheckBox14"
        Me.CheckBox14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox14.Size = New System.Drawing.Size(166, 19)
        Me.CheckBox14.TabIndex = 36
        Me.CheckBox14.Text = "Renewable"
        Me.CheckBox14.UseVisualStyleBackColor = False
        '
        'CheckBox15
        '
        Me.CheckBox15.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox15.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox15.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox15.Location = New System.Drawing.Point(484, 73)
        Me.CheckBox15.Name = "CheckBox15"
        Me.CheckBox15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox15.Size = New System.Drawing.Size(324, 19)
        Me.CheckBox15.TabIndex = 40
        Me.CheckBox15.Text = "Hide Summary At Renewal Acceptance"
        Me.CheckBox15.UseVisualStyleBackColor = False
        '
        'CheckBox16
        '
        Me.CheckBox16.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox16.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox16.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox16.Location = New System.Drawing.Point(484, 14)
        Me.CheckBox16.Name = "CheckBox16"
        Me.CheckBox16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox16.Size = New System.Drawing.Size(324, 19)
        Me.CheckBox16.TabIndex = 38
        Me.CheckBox16.Text = "Change Policy Number at Renewal"
        Me.CheckBox16.UseVisualStyleBackColor = False
        '
        'CheckBox17
        '
        Me.CheckBox17.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox17.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox17.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox17.Location = New System.Drawing.Point(295, 46)
        Me.CheckBox17.Name = "CheckBox17"
        Me.CheckBox17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox17.Size = New System.Drawing.Size(166, 19)
        Me.CheckBox17.TabIndex = 37
        Me.CheckBox17.Text = "Midnight Renewal"
        Me.CheckBox17.UseVisualStyleBackColor = False
        '
        'CheckBox18
        '
        Me.CheckBox18.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox18.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox18.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox18.Location = New System.Drawing.Point(6, 46)
        Me.CheckBox18.Name = "CheckBox18"
        Me.CheckBox18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox18.Size = New System.Drawing.Size(217, 19)
        Me.CheckBox18.TabIndex = 34
        Me.CheckBox18.Text = "Auto Renewal"
        Me.CheckBox18.UseVisualStyleBackColor = False
        '
        'TextBox2
        '
        Me.TextBox2.AcceptsReturn = True
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox2.Location = New System.Drawing.Point(209, 16)
        Me.TextBox2.MaxLength = 0
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox2.Size = New System.Drawing.Size(41, 20)
        Me.TextBox2.TabIndex = 33
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.SystemColors.Control
        Me.Label19.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label19.Location = New System.Drawing.Point(8, 76)
        Me.Label19.Name = "Label19"
        Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label19.Size = New System.Drawing.Size(139, 13)
        Me.Label19.TabIndex = 385
        Me.Label19.Text = "Default Months for Renewal"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.SystemColors.Control
        Me.Label20.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label20.Location = New System.Drawing.Point(24, 51)
        Me.Label20.Name = "Label20"
        Me.Label20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label20.Size = New System.Drawing.Size(74, 13)
        Me.Label20.TabIndex = 366
        Me.Label20.Text = "Auto Renewal"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.SystemColors.Control
        Me.Label21.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label21.Location = New System.Drawing.Point(8, 19)
        Me.Label21.Name = "Label21"
        Me.Label21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label21.Size = New System.Drawing.Size(82, 13)
        Me.Label21.TabIndex = 59
        Me.Label21.Text = "Renewal Period"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox5.Controls.Add(Me.CheckBox19)
        Me.GroupBox5.Controls.Add(Me.ComboBox2)
        Me.GroupBox5.Controls.Add(Me.CheckBox20)
        Me.GroupBox5.Controls.Add(Me.TextBox3)
        Me.GroupBox5.Controls.Add(Me.CheckBox21)
        Me.GroupBox5.Controls.Add(Me.ComboBox3)
        Me.GroupBox5.Controls.Add(Me.ComboBox4)
        Me.GroupBox5.Controls.Add(Me.CheckBox22)
        Me.GroupBox5.Controls.Add(Me.CboPMLookup2)
        Me.GroupBox5.Controls.Add(Me.Label22)
        Me.GroupBox5.Controls.Add(Me.Label23)
        Me.GroupBox5.Controls.Add(Me.Label24)
        Me.GroupBox5.Controls.Add(Me.Label25)
        Me.GroupBox5.Controls.Add(Me.Label26)
        Me.GroupBox5.Controls.Add(Me.Label27)
        Me.GroupBox5.Controls.Add(Me.Label28)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox5.Location = New System.Drawing.Point(8, 173)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox5.Size = New System.Drawing.Size(572, 157)
        Me.GroupBox5.TabIndex = 17
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Policy Creation"
        '
        'CheckBox19
        '
        Me.CheckBox19.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox19.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox19.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox19.Location = New System.Drawing.Point(364, 100)
        Me.CheckBox19.Name = "CheckBox19"
        Me.CheckBox19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox19.Size = New System.Drawing.Size(175, 29)
        Me.CheckBox19.TabIndex = 18
        Me.CheckBox19.Text = "Reinsurance Manual Premium Adjustment"
        Me.CheckBox19.UseVisualStyleBackColor = False
        '
        'CheckBox20
        '
        Me.CheckBox20.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox20.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox20.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox20.Location = New System.Drawing.Point(364, 75)
        Me.CheckBox20.Name = "CheckBox20"
        Me.CheckBox20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox20.Size = New System.Drawing.Size(175, 17)
        Me.CheckBox20.TabIndex = 17
        Me.CheckBox20.Text = "True Monthly Policy"
        Me.CheckBox20.UseVisualStyleBackColor = False
        '
        'TextBox3
        '
        Me.TextBox3.AcceptsReturn = True
        Me.TextBox3.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox3.Location = New System.Drawing.Point(157, 100)
        Me.TextBox3.MaxLength = 3
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox3.Size = New System.Drawing.Size(41, 20)
        Me.TextBox3.TabIndex = 13
        '
        'CheckBox21
        '
        Me.CheckBox21.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox21.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox21.Location = New System.Drawing.Point(525, 43)
        Me.CheckBox21.Name = "CheckBox21"
        Me.CheckBox21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox21.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox21.TabIndex = 16
        Me.CheckBox21.UseVisualStyleBackColor = False
        '
        'CheckBox22
        '
        Me.CheckBox22.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox22.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox22.Location = New System.Drawing.Point(525, 13)
        Me.CheckBox22.Name = "CheckBox22"
        Me.CheckBox22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox22.Size = New System.Drawing.Size(16, 19)
        Me.CheckBox22.TabIndex = 15
        Me.CheckBox22.UseVisualStyleBackColor = False
        '
        'CboPMLookup2
        '
        Me.CboPMLookup2.DefaultItemId = 0
        Me.CboPMLookup2.FirstItem = ""
        Me.CboPMLookup2.ItemId = 0
        Me.CboPMLookup2.ListIndex = -1
        Me.CboPMLookup2.Location = New System.Drawing.Point(157, 16)
        Me.CboPMLookup2.Name = "CboPMLookup2"
        Me.CboPMLookup2.PMLookupProductFamily = 9
        Me.CboPMLookup2.SingleItemId = 0
        Me.CboPMLookup2.Size = New System.Drawing.Size(173, 21)
        Me.CboPMLookup2.SortColumnName = ""
        Me.CboPMLookup2.Sorted = True
        Me.CboPMLookup2.TabIndex = 10
        Me.CboPMLookup2.TableName = "Policy_Style"
        Me.CboPMLookup2.ToolTipText = ""
        Me.CboPMLookup2.WhereClause = ""
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.BackColor = System.Drawing.SystemColors.Control
        Me.Label22.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label22.Location = New System.Drawing.Point(8, 128)
        Me.Label22.Name = "Label22"
        Me.Label22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label22.Size = New System.Drawing.Size(115, 13)
        Me.Label22.TabIndex = 66
        Me.Label22.Text = "Cover Note Numbering"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.SystemColors.Control
        Me.Label23.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label23.Location = New System.Drawing.Point(8, 103)
        Me.Label23.Name = "Label23"
        Me.Label23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label23.Size = New System.Drawing.Size(101, 13)
        Me.Label23.TabIndex = 66
        Me.Label23.Text = "Quote Expiry (days):"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.BackColor = System.Drawing.SystemColors.Control
        Me.Label24.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label24.Location = New System.Drawing.Point(367, 46)
        Me.Label24.Name = "Label24"
        Me.Label24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label24.Size = New System.Drawing.Size(119, 13)
        Me.Label24.TabIndex = 66
        Me.Label24.Text = "Policy Number at Quote"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.BackColor = System.Drawing.SystemColors.Control
        Me.Label25.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label25.Location = New System.Drawing.Point(8, 76)
        Me.Label25.Name = "Label25"
        Me.Label25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label25.Size = New System.Drawing.Size(89, 13)
        Me.Label25.TabIndex = 66
        Me.Label25.Text = "Policy Numbering"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.SystemColors.Control
        Me.Label26.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label26.Location = New System.Drawing.Point(8, 48)
        Me.Label26.Name = "Label26"
        Me.Label26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label26.Size = New System.Drawing.Size(90, 13)
        Me.Label26.TabIndex = 66
        Me.Label26.Text = "Quote Numbering"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.SystemColors.Control
        Me.Label27.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label27.Location = New System.Drawing.Point(8, 20)
        Me.Label27.Name = "Label27"
        Me.Label27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label27.Size = New System.Drawing.Size(101, 13)
        Me.Label27.TabIndex = 66
        Me.Label27.Text = "Default Policy Style:"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.SystemColors.Control
        Me.Label28.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label28.Location = New System.Drawing.Point(367, 16)
        Me.Label28.Name = "Label28"
        Me.Label28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label28.Size = New System.Drawing.Size(125, 13)
        Me.Label28.TabIndex = 66
        Me.Label28.Text = "Policy Style Is Mandatory"
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox6.Controls.Add(Me.TextBox4)
        Me.GroupBox6.Controls.Add(Me.TextBox5)
        Me.GroupBox6.Controls.Add(Me.TextBox6)
        Me.GroupBox6.Controls.Add(Me.TextBox7)
        Me.GroupBox6.Controls.Add(Me.TextBox8)
        Me.GroupBox6.Controls.Add(Me.TextBox9)
        Me.GroupBox6.Controls.Add(Me.TextBox10)
        Me.GroupBox6.Controls.Add(Me.CboPMLookup3)
        Me.GroupBox6.Controls.Add(Me.Label29)
        Me.GroupBox6.Controls.Add(Me.Label30)
        Me.GroupBox6.Controls.Add(Me.Label31)
        Me.GroupBox6.Controls.Add(Me.Label32)
        Me.GroupBox6.Controls.Add(Me.Label33)
        Me.GroupBox6.Controls.Add(Me.Label34)
        Me.GroupBox6.Controls.Add(Me.Label35)
        Me.GroupBox6.Controls.Add(Me.Label36)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox6.Location = New System.Drawing.Point(8, 12)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox6.Size = New System.Drawing.Size(820, 157)
        Me.GroupBox6.TabIndex = 0
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Details"
        '
        'TextBox4
        '
        Me.TextBox4.AcceptsReturn = True
        Me.TextBox4.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox4.Location = New System.Drawing.Point(584, 104)
        Me.TextBox4.MaxLength = 0
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox4.Size = New System.Drawing.Size(211, 37)
        Me.TextBox4.TabIndex = 9
        '
        'TextBox5
        '
        Me.TextBox5.AcceptsReturn = True
        Me.TextBox5.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox5.Location = New System.Drawing.Point(128, 99)
        Me.TextBox5.MaxLength = 0
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox5.Size = New System.Drawing.Size(41, 20)
        Me.TextBox5.TabIndex = 5
        '
        'TextBox6
        '
        Me.TextBox6.AcceptsReturn = True
        Me.TextBox6.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox6.Location = New System.Drawing.Point(128, 127)
        Me.TextBox6.MaxLength = 0
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox6.Size = New System.Drawing.Size(41, 20)
        Me.TextBox6.TabIndex = 6
        '
        'TextBox7
        '
        Me.TextBox7.AcceptsReturn = True
        Me.TextBox7.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox7.Location = New System.Drawing.Point(584, 60)
        Me.TextBox7.MaxLength = 0
        Me.TextBox7.Multiline = True
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox7.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox7.Size = New System.Drawing.Size(211, 37)
        Me.TextBox7.TabIndex = 8
        '
        'TextBox8
        '
        Me.TextBox8.AcceptsReturn = True
        Me.TextBox8.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox8.Location = New System.Drawing.Point(584, 16)
        Me.TextBox8.MaxLength = 0
        Me.TextBox8.Multiline = True
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox8.Size = New System.Drawing.Size(211, 37)
        Me.TextBox8.TabIndex = 7
        '
        'TextBox9
        '
        Me.TextBox9.AcceptsReturn = True
        Me.TextBox9.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox9.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox9.Location = New System.Drawing.Point(128, 42)
        Me.TextBox9.MaxLength = 0
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox9.Size = New System.Drawing.Size(173, 20)
        Me.TextBox9.TabIndex = 3
        '
        'TextBox10
        '
        Me.TextBox10.AcceptsReturn = True
        Me.TextBox10.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox10.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox10.Location = New System.Drawing.Point(128, 16)
        Me.TextBox10.MaxLength = 10
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox10.Size = New System.Drawing.Size(173, 20)
        Me.TextBox10.TabIndex = 2
        '
        'CboPMLookup3
        '
        Me.CboPMLookup3.DefaultItemId = 0
        Me.CboPMLookup3.FirstItem = ""
        Me.CboPMLookup3.ItemId = 0
        Me.CboPMLookup3.ListIndex = -1
        Me.CboPMLookup3.Location = New System.Drawing.Point(128, 68)
        Me.CboPMLookup3.Name = "CboPMLookup3"
        Me.CboPMLookup3.PMLookupProductFamily = 9
        Me.CboPMLookup3.SingleItemId = 0
        Me.CboPMLookup3.Size = New System.Drawing.Size(173, 21)
        Me.CboPMLookup3.SortColumnName = ""
        Me.CboPMLookup3.Sorted = True
        Me.CboPMLookup3.TabIndex = 4
        Me.CboPMLookup3.TableName = "Risk_type_group"
        Me.CboPMLookup3.ToolTipText = ""
        Me.CboPMLookup3.WhereClause = ""
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.SystemColors.Control
        Me.Label29.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label29.Location = New System.Drawing.Point(440, 114)
        Me.Label29.Name = "Label29"
        Me.Label29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label29.Size = New System.Drawing.Size(54, 13)
        Me.Label29.TabIndex = 15
        Me.Label29.Text = "Block No."
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.BackColor = System.Drawing.SystemColors.Control
        Me.Label30.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label30.Location = New System.Drawing.Point(8, 102)
        Me.Label30.Name = "Label30"
        Me.Label30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label30.Size = New System.Drawing.Size(59, 13)
        Me.Label30.TabIndex = 7
        Me.Label30.Text = "R/I Pointer"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.BackColor = System.Drawing.SystemColors.Control
        Me.Label31.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label31.Location = New System.Drawing.Point(8, 130)
        Me.Label31.Name = "Label31"
        Me.Label31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label31.Size = New System.Drawing.Size(75, 13)
        Me.Label31.TabIndex = 9
        Me.Label31.Text = "Report Pointer"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.SystemColors.Control
        Me.Label32.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label32.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label32.Location = New System.Drawing.Point(8, 72)
        Me.Label32.Name = "Label32"
        Me.Label32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label32.Size = New System.Drawing.Size(117, 13)
        Me.Label32.TabIndex = 5
        Me.Label32.Text = "Risk Type Group:"
        '
        'Label33
        '
        Me.Label33.BackColor = System.Drawing.SystemColors.Control
        Me.Label33.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label33.Location = New System.Drawing.Point(440, 66)
        Me.Label33.Name = "Label33"
        Me.Label33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label33.Size = New System.Drawing.Size(123, 25)
        Me.Label33.TabIndex = 13
        Me.Label33.Text = "Scheme Agency Ref"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.BackColor = System.Drawing.SystemColors.Control
        Me.Label34.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label34.Location = New System.Drawing.Point(440, 26)
        Me.Label34.Name = "Label34"
        Me.Label34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label34.Size = New System.Drawing.Size(60, 13)
        Me.Label34.TabIndex = 11
        Me.Label34.Text = "Description"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.BackColor = System.Drawing.SystemColors.Control
        Me.Label35.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label35.Location = New System.Drawing.Point(8, 45)
        Me.Label35.Name = "Label35"
        Me.Label35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label35.Size = New System.Drawing.Size(75, 13)
        Me.Label35.TabIndex = 3
        Me.Label35.Text = "Effective Date"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.BackColor = System.Drawing.SystemColors.Control
        Me.Label36.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label36.Location = New System.Drawing.Point(8, 19)
        Me.Label36.Name = "Label36"
        Me.Label36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label36.Size = New System.Drawing.Size(32, 13)
        Me.Label36.TabIndex = 1
        Me.Label36.Text = "Code"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox7)
        Me.TabPage2.Controls.Add(Me.GroupBox8)
        Me.TabPage2.Controls.Add(Me.GroupBox9)
        Me.TabPage2.Controls.Add(Me.GroupBox10)
        Me.TabPage2.Controls.Add(Me.GroupBox11)
        Me.TabPage2.Controls.Add(Me.GroupBox12)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(836, 642)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "2-Claim"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox7.Controls.Add(Me.PickList1)
        Me.GroupBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox7.Location = New System.Drawing.Point(8, 328)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox7.Size = New System.Drawing.Size(819, 195)
        Me.GroupBox7.TabIndex = 185
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Causations"
        '
        'PickList1
        '
        Me.PickList1.AvailableCaption = ""
        Me.PickList1.BusinessObject = "bSIRProduct.Business"
        Me.PickList1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList1.ForeignKeys = CType(resources.GetObject("PickList1.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList1.IsSearchable = False
        Me.PickList1.Location = New System.Drawing.Point(8, 11)
        Me.PickList1.Name = "PickList1"
        Me.PickList1.PickListType = "Causation"
        Me.PickList1.Size = New System.Drawing.Size(805, 174)
        Me.PickList1.TabIndex = 21
        '
        'GroupBox8
        '
        Me.GroupBox8.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox8.Controls.Add(Me.TextBox11)
        Me.GroupBox8.Controls.Add(Me.TextBox12)
        Me.GroupBox8.Controls.Add(Me.TextBox13)
        Me.GroupBox8.Controls.Add(Me.TextBox14)
        Me.GroupBox8.Controls.Add(Me.CheckBox23)
        Me.GroupBox8.Controls.Add(Me.CheckBox24)
        Me.GroupBox8.Controls.Add(Me.CheckBox25)
        Me.GroupBox8.Controls.Add(Me.BankAccount1)
        Me.GroupBox8.Controls.Add(Me.Label37)
        Me.GroupBox8.Controls.Add(Me.Label38)
        Me.GroupBox8.Controls.Add(Me.Label39)
        Me.GroupBox8.Controls.Add(Me.Label40)
        Me.GroupBox8.Controls.Add(Me.Label41)
        Me.GroupBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox8.Location = New System.Drawing.Point(8, 46)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox8.Size = New System.Drawing.Size(823, 109)
        Me.GroupBox8.TabIndex = 181
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Limits"
        '
        'TextBox11
        '
        Me.TextBox11.AcceptsReturn = True
        Me.TextBox11.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox11.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox11.Location = New System.Drawing.Point(645, 14)
        Me.TextBox11.MaxLength = 10
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox11.Size = New System.Drawing.Size(167, 20)
        Me.TextBox11.TabIndex = 7
        Me.TextBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBox12
        '
        Me.TextBox12.AcceptsReturn = True
        Me.TextBox12.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox12.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox12.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox12.Location = New System.Drawing.Point(136, 17)
        Me.TextBox12.MaxLength = 10
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox12.Size = New System.Drawing.Size(169, 20)
        Me.TextBox12.TabIndex = 3
        '
        'TextBox13
        '
        Me.TextBox13.AcceptsReturn = True
        Me.TextBox13.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox13.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox13.Location = New System.Drawing.Point(136, 40)
        Me.TextBox13.MaxLength = 10
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox13.Size = New System.Drawing.Size(169, 20)
        Me.TextBox13.TabIndex = 4
        '
        'TextBox14
        '
        Me.TextBox14.AcceptsReturn = True
        Me.TextBox14.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox14.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox14.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox14.Location = New System.Drawing.Point(645, 36)
        Me.TextBox14.MaxLength = 10
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox14.Size = New System.Drawing.Size(167, 20)
        Me.TextBox14.TabIndex = 8
        Me.TextBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CheckBox23
        '
        Me.CheckBox23.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox23.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox23.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox23.Location = New System.Drawing.Point(8, 64)
        Me.CheckBox23.Name = "CheckBox23"
        Me.CheckBox23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox23.Size = New System.Drawing.Size(297, 17)
        Me.CheckBox23.TabIndex = 5
        Me.CheckBox23.Text = "Prevent Claim Payments on Cancelled Agents"
        Me.CheckBox23.UseVisualStyleBackColor = False
        '
        'CheckBox24
        '
        Me.CheckBox24.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox24.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox24.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox24.Location = New System.Drawing.Point(400, 80)
        Me.CheckBox24.Name = "CheckBox24"
        Me.CheckBox24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox24.Size = New System.Drawing.Size(258, 25)
        Me.CheckBox24.TabIndex = 10
        Me.CheckBox24.Text = "Media Type Field Mandatory On Claim Payments"
        Me.CheckBox24.UseVisualStyleBackColor = False
        '
        'CheckBox25
        '
        Me.CheckBox25.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox25.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox25.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox25.Location = New System.Drawing.Point(8, 88)
        Me.CheckBox25.Name = "CheckBox25"
        Me.CheckBox25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox25.Size = New System.Drawing.Size(297, 17)
        Me.CheckBox25.TabIndex = 6
        Me.CheckBox25.Text = "Allow Loss Currency Change"
        Me.CheckBox25.UseVisualStyleBackColor = False
        '
        'BankAccount1
        '
        Me.BankAccount1.DefaultId = "0"
        Me.BankAccount1.FirstItem = ""
        Me.BankAccount1.Id = 0
        Me.BankAccount1.ListIndex = -1
        Me.BankAccount1.Location = New System.Drawing.Point(645, 58)
        Me.BankAccount1.Name = "BankAccount1"
        Me.BankAccount1.Size = New System.Drawing.Size(168, 21)
        Me.BankAccount1.TabIndex = 9
        Me.BankAccount1.ToolTipText = ""
        Me.BankAccount1.WhatsThisHelpID = 0
        '
        'Label37
        '
        Me.Label37.BackColor = System.Drawing.SystemColors.Control
        Me.Label37.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label37.Location = New System.Drawing.Point(402, 60)
        Me.Label37.Name = "Label37"
        Me.Label37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label37.Size = New System.Drawing.Size(118, 17)
        Me.Label37.TabIndex = 365
        Me.Label37.Text = "Bank Account :"
        '
        'Label38
        '
        Me.Label38.BackColor = System.Drawing.SystemColors.Control
        Me.Label38.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label38.Location = New System.Drawing.Point(402, 15)
        Me.Label38.Name = "Label38"
        Me.Label38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label38.Size = New System.Drawing.Size(104, 17)
        Me.Label38.TabIndex = 99
        Me.Label38.Text = "Allowed Claims :"
        '
        'Label39
        '
        Me.Label39.BackColor = System.Drawing.SystemColors.Control
        Me.Label39.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label39.Location = New System.Drawing.Point(8, 17)
        Me.Label39.Name = "Label39"
        Me.Label39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label39.Size = New System.Drawing.Size(119, 17)
        Me.Label39.TabIndex = 999
        Me.Label39.Text = "No. of Years Back :"
        '
        'Label40
        '
        Me.Label40.BackColor = System.Drawing.SystemColors.Control
        Me.Label40.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label40.Location = New System.Drawing.Point(8, 41)
        Me.Label40.Name = "Label40"
        Me.Label40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label40.Size = New System.Drawing.Size(121, 17)
        Me.Label40.TabIndex = 99
        Me.Label40.Text = "Single Claim Value :"
        '
        'Label41
        '
        Me.Label41.BackColor = System.Drawing.SystemColors.Control
        Me.Label41.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label41.Location = New System.Drawing.Point(402, 37)
        Me.Label41.Name = "Label41"
        Me.Label41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label41.Size = New System.Drawing.Size(118, 17)
        Me.Label41.TabIndex = 99
        Me.Label41.Text = "Total Claims Value :"
        '
        'GroupBox9
        '
        Me.GroupBox9.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox9.Controls.Add(Me.ComboBox5)
        Me.GroupBox9.Controls.Add(Me.ComboBox6)
        Me.GroupBox9.Controls.Add(Me.Label42)
        Me.GroupBox9.Controls.Add(Me.Label43)
        Me.GroupBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox9.Location = New System.Drawing.Point(410, 156)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox9.Size = New System.Drawing.Size(419, 81)
        Me.GroupBox9.TabIndex = 183
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Numbering"
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.SystemColors.Control
        Me.Label42.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label42.Location = New System.Drawing.Point(8, 48)
        Me.Label42.Name = "Label42"
        Me.Label42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label42.Size = New System.Drawing.Size(173, 29)
        Me.Label42.TabIndex = 99
        Me.Label42.Text = "Full Claim Auto Numbering ID"
        '
        'Label43
        '
        Me.Label43.BackColor = System.Drawing.SystemColors.Control
        Me.Label43.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label43.Location = New System.Drawing.Point(8, 22)
        Me.Label43.Name = "Label43"
        Me.Label43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label43.Size = New System.Drawing.Size(214, 26)
        Me.Label43.TabIndex = 88
        Me.Label43.Text = "Provisional Claim Auto Numbering ID"
        '
        'GroupBox10
        '
        Me.GroupBox10.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox10.Controls.Add(Me.CheckBox26)
        Me.GroupBox10.Controls.Add(Me.CheckBox27)
        Me.GroupBox10.Controls.Add(Me.CheckBox28)
        Me.GroupBox10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox10.Location = New System.Drawing.Point(8, 156)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox10.Size = New System.Drawing.Size(395, 81)
        Me.GroupBox10.TabIndex = 182
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Claims Transaction Suppression"
        '
        'CheckBox26
        '
        Me.CheckBox26.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox26.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox26.Location = New System.Drawing.Point(8, 53)
        Me.CheckBox26.Name = "CheckBox26"
        Me.CheckBox26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox26.Size = New System.Drawing.Size(153, 17)
        Me.CheckBox26.TabIndex = 13
        Me.CheckBox26.Text = "Recovery"
        Me.CheckBox26.UseVisualStyleBackColor = False
        '
        'CheckBox27
        '
        Me.CheckBox27.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox27.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox27.Location = New System.Drawing.Point(8, 35)
        Me.CheckBox27.Name = "CheckBox27"
        Me.CheckBox27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox27.Size = New System.Drawing.Size(153, 17)
        Me.CheckBox27.TabIndex = 12
        Me.CheckBox27.Text = "Payment"
        Me.CheckBox27.UseVisualStyleBackColor = False
        '
        'CheckBox28
        '
        Me.CheckBox28.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox28.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox28.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox28.Location = New System.Drawing.Point(8, 16)
        Me.CheckBox28.Name = "CheckBox28"
        Me.CheckBox28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox28.Size = New System.Drawing.Size(185, 17)
        Me.CheckBox28.TabIndex = 11
        Me.CheckBox28.Text = "Reserve"
        Me.CheckBox28.UseVisualStyleBackColor = False
        '
        'GroupBox11
        '
        Me.GroupBox11.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox11.Controls.Add(Me.CboPMLookup4)
        Me.GroupBox11.Controls.Add(Me.CboPMLookup5)
        Me.GroupBox11.Controls.Add(Me.Label44)
        Me.GroupBox11.Controls.Add(Me.Label45)
        Me.GroupBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox11.Location = New System.Drawing.Point(8, 4)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox11.Size = New System.Drawing.Size(822, 41)
        Me.GroupBox11.TabIndex = 180
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Cover"
        '
        'CboPMLookup4
        '
        Me.CboPMLookup4.DefaultItemId = 0
        Me.CboPMLookup4.FirstItem = ""
        Me.CboPMLookup4.ItemId = 0
        Me.CboPMLookup4.ListIndex = -1
        Me.CboPMLookup4.Location = New System.Drawing.Point(643, 13)
        Me.CboPMLookup4.Name = "CboPMLookup4"
        Me.CboPMLookup4.PMLookupProductFamily = 1
        Me.CboPMLookup4.SingleItemId = 0
        Me.CboPMLookup4.Size = New System.Drawing.Size(164, 21)
        Me.CboPMLookup4.SortColumnName = ""
        Me.CboPMLookup4.Sorted = True
        Me.CboPMLookup4.TabIndex = 2
        Me.CboPMLookup4.TableName = "claims_cover_basis"
        Me.CboPMLookup4.ToolTipText = ""
        Me.CboPMLookup4.WhereClause = ""
        '
        'CboPMLookup5
        '
        Me.CboPMLookup5.DefaultItemId = 0
        Me.CboPMLookup5.FirstItem = ""
        Me.CboPMLookup5.ItemId = 0
        Me.CboPMLookup5.ListIndex = -1
        Me.CboPMLookup5.Location = New System.Drawing.Point(136, 14)
        Me.CboPMLookup5.Name = "CboPMLookup5"
        Me.CboPMLookup5.PMLookupProductFamily = 1
        Me.CboPMLookup5.SingleItemId = 0
        Me.CboPMLookup5.Size = New System.Drawing.Size(169, 21)
        Me.CboPMLookup5.SortColumnName = ""
        Me.CboPMLookup5.Sorted = True
        Me.CboPMLookup5.TabIndex = 1
        Me.CboPMLookup5.TableName = "claims_type_basis"
        Me.CboPMLookup5.ToolTipText = ""
        Me.CboPMLookup5.WhereClause = ""
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.BackColor = System.Drawing.SystemColors.Control
        Me.Label44.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label44.Location = New System.Drawing.Point(402, 16)
        Me.Label44.Name = "Label44"
        Me.Label44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label44.Size = New System.Drawing.Size(124, 13)
        Me.Label44.TabIndex = 183
        Me.Label44.Text = "Cover Verification Basis :"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.BackColor = System.Drawing.SystemColors.Control
        Me.Label45.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label45.Location = New System.Drawing.Point(8, 16)
        Me.Label45.Name = "Label45"
        Me.Label45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label45.Size = New System.Drawing.Size(98, 13)
        Me.Label45.TabIndex = 181
        Me.Label45.Text = "Claims Type Basis :"
        '
        'GroupBox12
        '
        Me.GroupBox12.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox12.Controls.Add(Me.CheckBox29)
        Me.GroupBox12.Controls.Add(Me.TextBox15)
        Me.GroupBox12.Controls.Add(Me.TextBox16)
        Me.GroupBox12.Controls.Add(Me.CheckBox30)
        Me.GroupBox12.Controls.Add(Me.CheckBox31)
        Me.GroupBox12.Controls.Add(Me.Label46)
        Me.GroupBox12.Controls.Add(Me.Label47)
        Me.GroupBox12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox12.Location = New System.Drawing.Point(8, 236)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox12.Size = New System.Drawing.Size(821, 93)
        Me.GroupBox12.TabIndex = 184
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Claim Payment"
        '
        'CheckBox29
        '
        Me.CheckBox29.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox29.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox29.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox29.Location = New System.Drawing.Point(406, 32)
        Me.CheckBox29.Name = "CheckBox29"
        Me.CheckBox29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox29.Size = New System.Drawing.Size(351, 25)
        Me.CheckBox29.TabIndex = 20
        Me.CheckBox29.Text = "Recommender Steps for Claim Payments"
        Me.CheckBox29.UseVisualStyleBackColor = False
        '
        'TextBox15
        '
        Me.TextBox15.AcceptsReturn = True
        Me.TextBox15.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox15.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox15.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox15.Location = New System.Drawing.Point(278, 62)
        Me.TextBox15.MaxLength = 3
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox15.Size = New System.Drawing.Size(77, 20)
        Me.TextBox15.TabIndex = 18
        Me.TextBox15.Visible = False
        '
        'TextBox16
        '
        Me.TextBox16.AcceptsReturn = True
        Me.TextBox16.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox16.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox16.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox16.Location = New System.Drawing.Point(278, 38)
        Me.TextBox16.MaxLength = 10
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox16.Size = New System.Drawing.Size(77, 20)
        Me.TextBox16.TabIndex = 17
        Me.TextBox16.Visible = False
        '
        'CheckBox30
        '
        Me.CheckBox30.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox30.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox30.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox30.Location = New System.Drawing.Point(12, 18)
        Me.CheckBox30.Name = "CheckBox30"
        Me.CheckBox30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox30.Size = New System.Drawing.Size(279, 17)
        Me.CheckBox30.TabIndex = 16
        Me.CheckBox30.Text = "Multiple Claim Payments"
        Me.CheckBox30.UseVisualStyleBackColor = False
        '
        'CheckBox31
        '
        Me.CheckBox31.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox31.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox31.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox31.Location = New System.Drawing.Point(406, 12)
        Me.CheckBox31.Name = "CheckBox31"
        Me.CheckBox31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox31.Size = New System.Drawing.Size(351, 25)
        Me.CheckBox31.TabIndex = 19
        Me.CheckBox31.Text = "Run Authorisation Scripts for Claims Payments"
        Me.CheckBox31.UseVisualStyleBackColor = False
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.SystemColors.Control
        Me.Label46.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label46.Location = New System.Drawing.Point(12, 62)
        Me.Label46.Name = "Label46"
        Me.Label46.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label46.Size = New System.Drawing.Size(243, 29)
        Me.Label46.TabIndex = 292
        Me.Label46.Text = "Max. No. of Unauthorised Claim Payments"
        Me.Label46.Visible = False
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.SystemColors.Control
        Me.Label47.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label47.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label47.Location = New System.Drawing.Point(12, 40)
        Me.Label47.Name = "Label47"
        Me.Label47.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label47.Size = New System.Drawing.Size(223, 29)
        Me.Label47.TabIndex = 291
        Me.Label47.Text = "Max. Unauthorised Claims Value"
        Me.Label47.Visible = False
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox13)
        Me.TabPage3.Controls.Add(Me.GroupBox14)
        Me.TabPage3.Controls.Add(Me.GroupBox15)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(836, 642)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "3-Claims Workflow"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox13
        '
        Me.GroupBox13.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox13.Controls.Add(Me.CheckBox32)
        Me.GroupBox13.Controls.Add(Me.CheckBox33)
        Me.GroupBox13.Controls.Add(Me.CheckBox34)
        Me.GroupBox13.Controls.Add(Me.CheckBox35)
        Me.GroupBox13.Controls.Add(Me.CheckBox36)
        Me.GroupBox13.Controls.Add(Me.CheckBox37)
        Me.GroupBox13.Controls.Add(Me.CheckBox38)
        Me.GroupBox13.Controls.Add(Me.CheckBox39)
        Me.GroupBox13.Controls.Add(Me.CheckBox40)
        Me.GroupBox13.Controls.Add(Me.CheckBox41)
        Me.GroupBox13.Controls.Add(Me.CheckBox42)
        Me.GroupBox13.Controls.Add(Me.CheckBox43)
        Me.GroupBox13.Controls.Add(Me.CheckBox44)
        Me.GroupBox13.Controls.Add(Me.CheckBox45)
        Me.GroupBox13.Controls.Add(Me.CheckBox46)
        Me.GroupBox13.Controls.Add(Me.CheckBox47)
        Me.GroupBox13.Controls.Add(Me.CheckBox48)
        Me.GroupBox13.Controls.Add(Me.CheckBox49)
        Me.GroupBox13.Controls.Add(Me.CheckBox50)
        Me.GroupBox13.Controls.Add(Me.CheckBox51)
        Me.GroupBox13.Controls.Add(Me.CheckBox52)
        Me.GroupBox13.Controls.Add(Me.CheckBox53)
        Me.GroupBox13.Controls.Add(Me.CheckBox54)
        Me.GroupBox13.Controls.Add(Me.CheckBox55)
        Me.GroupBox13.Controls.Add(Me.CheckBox56)
        Me.GroupBox13.Controls.Add(Me.CheckBox57)
        Me.GroupBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox13.Location = New System.Drawing.Point(10, 9)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox13.Size = New System.Drawing.Size(266, 528)
        Me.GroupBox13.TabIndex = 294
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Open Claim"
        '
        'CheckBox32
        '
        Me.CheckBox32.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox32.Checked = True
        Me.CheckBox32.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox32.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox32.Enabled = False
        Me.CheckBox32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox32.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox32.Location = New System.Drawing.Point(6, 430)
        Me.CheckBox32.Name = "CheckBox32"
        Me.CheckBox32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox32.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox32.TabIndex = 376
        Me.CheckBox32.Text = "Close Claim Message"
        Me.CheckBox32.UseVisualStyleBackColor = False
        '
        'CheckBox33
        '
        Me.CheckBox33.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox33.Checked = True
        Me.CheckBox33.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox33.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox33.Enabled = False
        Me.CheckBox33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox33.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox33.Location = New System.Drawing.Point(6, 511)
        Me.CheckBox33.Name = "CheckBox33"
        Me.CheckBox33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox33.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox33.TabIndex = 319
        Me.CheckBox33.Text = "Unlock Claim"
        Me.CheckBox33.UseVisualStyleBackColor = False
        '
        'CheckBox34
        '
        Me.CheckBox34.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox34.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox34.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox34.Location = New System.Drawing.Point(6, 494)
        Me.CheckBox34.Name = "CheckBox34"
        Me.CheckBox34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox34.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox34.TabIndex = 318
        Me.CheckBox34.Text = "Do you wish to make further payments?"
        Me.CheckBox34.UseVisualStyleBackColor = False
        '
        'CheckBox35
        '
        Me.CheckBox35.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox35.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox35.Location = New System.Drawing.Point(6, 471)
        Me.CheckBox35.Name = "CheckBox35"
        Me.CheckBox35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox35.Size = New System.Drawing.Size(255, 20)
        Me.CheckBox35.TabIndex = 317
        Me.CheckBox35.Text = "Produce Claim Payment Documents"
        Me.CheckBox35.UseVisualStyleBackColor = False
        '
        'CheckBox36
        '
        Me.CheckBox36.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox36.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox36.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox36.Location = New System.Drawing.Point(6, 446)
        Me.CheckBox36.Name = "CheckBox36"
        Me.CheckBox36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox36.Size = New System.Drawing.Size(255, 31)
        Me.CheckBox36.TabIndex = 316
        Me.CheckBox36.Text = "Display Generate Claim Payment Documents Message"
        Me.CheckBox36.UseVisualStyleBackColor = False
        '
        'CheckBox37
        '
        Me.CheckBox37.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox37.Checked = True
        Me.CheckBox37.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox37.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox37.Enabled = False
        Me.CheckBox37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox37.Location = New System.Drawing.Point(6, 411)
        Me.CheckBox37.Name = "CheckBox37"
        Me.CheckBox37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox37.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox37.TabIndex = 315
        Me.CheckBox37.Text = "Check Status"
        Me.CheckBox37.UseVisualStyleBackColor = False
        '
        'CheckBox38
        '
        Me.CheckBox38.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox38.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox38.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox38.Location = New System.Drawing.Point(6, 392)
        Me.CheckBox38.Name = "CheckBox38"
        Me.CheckBox38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox38.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox38.TabIndex = 314
        Me.CheckBox38.Text = "Cash Payments Process"
        Me.CheckBox38.UseVisualStyleBackColor = False
        '
        'CheckBox39
        '
        Me.CheckBox39.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox39.Checked = True
        Me.CheckBox39.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox39.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox39.Enabled = False
        Me.CheckBox39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox39.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox39.Location = New System.Drawing.Point(6, 374)
        Me.CheckBox39.Name = "CheckBox39"
        Me.CheckBox39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox39.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox39.TabIndex = 313
        Me.CheckBox39.Text = "Update Claim Details"
        Me.CheckBox39.UseVisualStyleBackColor = False
        '
        'CheckBox40
        '
        Me.CheckBox40.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox40.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox40.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox40.Location = New System.Drawing.Point(6, 355)
        Me.CheckBox40.Name = "CheckBox40"
        Me.CheckBox40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox40.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox40.TabIndex = 312
        Me.CheckBox40.Text = "Enter Description for Change"
        Me.CheckBox40.UseVisualStyleBackColor = False
        '
        'CheckBox41
        '
        Me.CheckBox41.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox41.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox41.Location = New System.Drawing.Point(6, 336)
        Me.CheckBox41.Name = "CheckBox41"
        Me.CheckBox41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox41.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox41.TabIndex = 311
        Me.CheckBox41.Text = "Reinsurance Payment"
        Me.CheckBox41.UseVisualStyleBackColor = False
        '
        'CheckBox42
        '
        Me.CheckBox42.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox42.Checked = True
        Me.CheckBox42.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox42.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox42.Enabled = False
        Me.CheckBox42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox42.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox42.Location = New System.Drawing.Point(6, 318)
        Me.CheckBox42.Name = "CheckBox42"
        Me.CheckBox42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox42.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox42.TabIndex = 310
        Me.CheckBox42.Text = "Coinsurance Payment"
        Me.CheckBox42.UseVisualStyleBackColor = False
        '
        'CheckBox43
        '
        Me.CheckBox43.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox43.Checked = True
        Me.CheckBox43.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox43.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox43.Enabled = False
        Me.CheckBox43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox43.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox43.Location = New System.Drawing.Point(6, 299)
        Me.CheckBox43.Name = "CheckBox43"
        Me.CheckBox43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox43.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox43.TabIndex = 309
        Me.CheckBox43.Text = "Risk Details"
        Me.CheckBox43.UseVisualStyleBackColor = False
        '
        'CheckBox44
        '
        Me.CheckBox44.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox44.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox44.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox44.Location = New System.Drawing.Point(6, 280)
        Me.CheckBox44.Name = "CheckBox44"
        Me.CheckBox44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox44.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox44.TabIndex = 308
        Me.CheckBox44.Text = "Fast Track Claims Payments"
        Me.CheckBox44.UseVisualStyleBackColor = False
        '
        'CheckBox45
        '
        Me.CheckBox45.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox45.Checked = True
        Me.CheckBox45.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox45.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox45.Enabled = False
        Me.CheckBox45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox45.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox45.Location = New System.Drawing.Point(6, 262)
        Me.CheckBox45.Name = "CheckBox45"
        Me.CheckBox45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox45.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox45.TabIndex = 307
        Me.CheckBox45.Text = "Proceed to Claim Payments Message"
        Me.CheckBox45.UseVisualStyleBackColor = False
        '
        'CheckBox46
        '
        Me.CheckBox46.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox46.Checked = True
        Me.CheckBox46.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox46.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox46.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox46.Location = New System.Drawing.Point(6, 243)
        Me.CheckBox46.Name = "CheckBox46"
        Me.CheckBox46.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox46.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox46.TabIndex = 306
        Me.CheckBox46.Text = "Claim Payments Process"
        Me.CheckBox46.UseVisualStyleBackColor = False
        '
        'CheckBox47
        '
        Me.CheckBox47.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox47.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox47.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox47.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox47.Location = New System.Drawing.Point(6, 224)
        Me.CheckBox47.Name = "CheckBox47"
        Me.CheckBox47.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox47.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox47.TabIndex = 305
        Me.CheckBox47.Text = "Produce Claim Notification Documents"
        Me.CheckBox47.UseVisualStyleBackColor = False
        '
        'CheckBox48
        '
        Me.CheckBox48.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox48.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox48.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox48.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox48.Location = New System.Drawing.Point(6, 195)
        Me.CheckBox48.Name = "CheckBox48"
        Me.CheckBox48.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox48.Size = New System.Drawing.Size(255, 30)
        Me.CheckBox48.TabIndex = 304
        Me.CheckBox48.Text = "Display Generate Claim Notification Documents Message"
        Me.CheckBox48.UseVisualStyleBackColor = False
        '
        'CheckBox49
        '
        Me.CheckBox49.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox49.Checked = True
        Me.CheckBox49.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox49.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox49.Enabled = False
        Me.CheckBox49.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox49.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox49.Location = New System.Drawing.Point(6, 176)
        Me.CheckBox49.Name = "CheckBox49"
        Me.CheckBox49.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox49.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox49.TabIndex = 303
        Me.CheckBox49.Text = "Update Claim Details"
        Me.CheckBox49.UseVisualStyleBackColor = False
        '
        'CheckBox50
        '
        Me.CheckBox50.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox50.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox50.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox50.Location = New System.Drawing.Point(6, 157)
        Me.CheckBox50.Name = "CheckBox50"
        Me.CheckBox50.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox50.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox50.TabIndex = 302
        Me.CheckBox50.Text = "External Claim Handling"
        Me.CheckBox50.UseVisualStyleBackColor = False
        '
        'CheckBox51
        '
        Me.CheckBox51.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox51.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox51.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox51.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox51.Location = New System.Drawing.Point(6, 137)
        Me.CheckBox51.Name = "CheckBox51"
        Me.CheckBox51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox51.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox51.TabIndex = 301
        Me.CheckBox51.Text = "Third Party Recovery"
        Me.CheckBox51.UseVisualStyleBackColor = False
        '
        'CheckBox52
        '
        Me.CheckBox52.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox52.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox52.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox52.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox52.Location = New System.Drawing.Point(6, 118)
        Me.CheckBox52.Name = "CheckBox52"
        Me.CheckBox52.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox52.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox52.TabIndex = 300
        Me.CheckBox52.Text = "Salvage Recovery"
        Me.CheckBox52.UseVisualStyleBackColor = False
        '
        'CheckBox53
        '
        Me.CheckBox53.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox53.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox53.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox53.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox53.Location = New System.Drawing.Point(6, 98)
        Me.CheckBox53.Name = "CheckBox53"
        Me.CheckBox53.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox53.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox53.TabIndex = 299
        Me.CheckBox53.Text = "Reinsurance Recoveries"
        Me.CheckBox53.UseVisualStyleBackColor = False
        '
        'CheckBox54
        '
        Me.CheckBox54.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox54.Checked = True
        Me.CheckBox54.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox54.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox54.Enabled = False
        Me.CheckBox54.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox54.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox54.Location = New System.Drawing.Point(6, 79)
        Me.CheckBox54.Name = "CheckBox54"
        Me.CheckBox54.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox54.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox54.TabIndex = 298
        Me.CheckBox54.Text = "Risk Details"
        Me.CheckBox54.UseVisualStyleBackColor = False
        '
        'CheckBox55
        '
        Me.CheckBox55.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox55.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox55.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox55.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox55.Location = New System.Drawing.Point(6, 59)
        Me.CheckBox55.Name = "CheckBox55"
        Me.CheckBox55.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox55.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox55.TabIndex = 297
        Me.CheckBox55.Text = "Check Unpaid Status"
        Me.CheckBox55.UseVisualStyleBackColor = False
        '
        'CheckBox56
        '
        Me.CheckBox56.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox56.Checked = True
        Me.CheckBox56.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox56.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox56.Enabled = False
        Me.CheckBox56.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox56.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox56.Location = New System.Drawing.Point(6, 40)
        Me.CheckBox56.Name = "CheckBox56"
        Me.CheckBox56.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox56.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox56.TabIndex = 296
        Me.CheckBox56.Text = "Claim Details"
        Me.CheckBox56.UseVisualStyleBackColor = False
        '
        'CheckBox57
        '
        Me.CheckBox57.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox57.Checked = True
        Me.CheckBox57.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox57.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox57.Enabled = False
        Me.CheckBox57.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox57.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox57.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox57.Name = "CheckBox57"
        Me.CheckBox57.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox57.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox57.TabIndex = 295
        Me.CheckBox57.Text = "Find Policy"
        Me.CheckBox57.UseVisualStyleBackColor = False
        '
        'GroupBox14
        '
        Me.GroupBox14.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox14.Controls.Add(Me.CheckBox58)
        Me.GroupBox14.Controls.Add(Me.CheckBox59)
        Me.GroupBox14.Controls.Add(Me.CheckBox60)
        Me.GroupBox14.Controls.Add(Me.CheckBox61)
        Me.GroupBox14.Controls.Add(Me.CheckBox62)
        Me.GroupBox14.Controls.Add(Me.CheckBox63)
        Me.GroupBox14.Controls.Add(Me.CheckBox64)
        Me.GroupBox14.Controls.Add(Me.CheckBox65)
        Me.GroupBox14.Controls.Add(Me.CheckBox66)
        Me.GroupBox14.Controls.Add(Me.CheckBox67)
        Me.GroupBox14.Controls.Add(Me.CheckBox68)
        Me.GroupBox14.Controls.Add(Me.CheckBox69)
        Me.GroupBox14.Controls.Add(Me.CheckBox70)
        Me.GroupBox14.Controls.Add(Me.CheckBox71)
        Me.GroupBox14.Controls.Add(Me.CheckBox72)
        Me.GroupBox14.Controls.Add(Me.CheckBox73)
        Me.GroupBox14.Controls.Add(Me.CheckBox74)
        Me.GroupBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox14.Location = New System.Drawing.Point(565, 9)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox14.Size = New System.Drawing.Size(264, 361)
        Me.GroupBox14.TabIndex = 347
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "Claim Payment"
        '
        'CheckBox58
        '
        Me.CheckBox58.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox58.Checked = True
        Me.CheckBox58.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox58.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox58.Enabled = False
        Me.CheckBox58.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox58.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox58.Location = New System.Drawing.Point(6, 252)
        Me.CheckBox58.Name = "CheckBox58"
        Me.CheckBox58.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox58.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox58.TabIndex = 378
        Me.CheckBox58.Text = "Close Claim Message"
        Me.CheckBox58.UseVisualStyleBackColor = False
        '
        'CheckBox59
        '
        Me.CheckBox59.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox59.Checked = True
        Me.CheckBox59.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox59.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox59.Enabled = False
        Me.CheckBox59.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox59.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox59.Location = New System.Drawing.Point(6, 341)
        Me.CheckBox59.Name = "CheckBox59"
        Me.CheckBox59.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox59.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox59.TabIndex = 363
        Me.CheckBox59.Text = "Unlock Claim"
        Me.CheckBox59.UseVisualStyleBackColor = False
        '
        'CheckBox60
        '
        Me.CheckBox60.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox60.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox60.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox60.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox60.Location = New System.Drawing.Point(6, 321)
        Me.CheckBox60.Name = "CheckBox60"
        Me.CheckBox60.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox60.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox60.TabIndex = 362
        Me.CheckBox60.Text = "Do you wish to make further payments?"
        Me.CheckBox60.UseVisualStyleBackColor = False
        '
        'CheckBox61
        '
        Me.CheckBox61.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox61.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox61.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox61.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox61.Location = New System.Drawing.Point(6, 300)
        Me.CheckBox61.Name = "CheckBox61"
        Me.CheckBox61.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox61.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox61.TabIndex = 361
        Me.CheckBox61.Text = "Produce Claim Payment Documents"
        Me.CheckBox61.UseVisualStyleBackColor = False
        '
        'CheckBox62
        '
        Me.CheckBox62.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox62.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox62.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox62.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox62.Location = New System.Drawing.Point(6, 266)
        Me.CheckBox62.Name = "CheckBox62"
        Me.CheckBox62.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox62.Size = New System.Drawing.Size(255, 32)
        Me.CheckBox62.TabIndex = 360
        Me.CheckBox62.Text = "Display Generate Claim Payment Documents Message"
        Me.CheckBox62.UseVisualStyleBackColor = False
        '
        'CheckBox63
        '
        Me.CheckBox63.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox63.Checked = True
        Me.CheckBox63.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox63.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox63.Enabled = False
        Me.CheckBox63.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox63.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox63.Location = New System.Drawing.Point(6, 231)
        Me.CheckBox63.Name = "CheckBox63"
        Me.CheckBox63.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox63.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox63.TabIndex = 359
        Me.CheckBox63.Text = "Check Status"
        Me.CheckBox63.UseVisualStyleBackColor = False
        '
        'CheckBox64
        '
        Me.CheckBox64.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox64.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox64.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox64.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox64.Location = New System.Drawing.Point(6, 212)
        Me.CheckBox64.Name = "CheckBox64"
        Me.CheckBox64.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox64.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox64.TabIndex = 358
        Me.CheckBox64.Text = "Cash Payments Process"
        Me.CheckBox64.UseVisualStyleBackColor = False
        '
        'CheckBox65
        '
        Me.CheckBox65.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox65.Checked = True
        Me.CheckBox65.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox65.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox65.Enabled = False
        Me.CheckBox65.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox65.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox65.Location = New System.Drawing.Point(6, 193)
        Me.CheckBox65.Name = "CheckBox65"
        Me.CheckBox65.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox65.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox65.TabIndex = 357
        Me.CheckBox65.Text = "Update Claim Details"
        Me.CheckBox65.UseVisualStyleBackColor = False
        '
        'CheckBox66
        '
        Me.CheckBox66.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox66.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox66.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox66.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox66.Location = New System.Drawing.Point(6, 174)
        Me.CheckBox66.Name = "CheckBox66"
        Me.CheckBox66.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox66.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox66.TabIndex = 356
        Me.CheckBox66.Text = "Enter Description for Change"
        Me.CheckBox66.UseVisualStyleBackColor = False
        '
        'CheckBox67
        '
        Me.CheckBox67.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox67.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox67.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox67.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox67.Location = New System.Drawing.Point(6, 154)
        Me.CheckBox67.Name = "CheckBox67"
        Me.CheckBox67.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox67.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox67.TabIndex = 355
        Me.CheckBox67.Text = "Reinsurance Payment"
        Me.CheckBox67.UseVisualStyleBackColor = False
        '
        'CheckBox68
        '
        Me.CheckBox68.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox68.Checked = True
        Me.CheckBox68.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox68.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox68.Enabled = False
        Me.CheckBox68.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox68.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox68.Location = New System.Drawing.Point(6, 135)
        Me.CheckBox68.Name = "CheckBox68"
        Me.CheckBox68.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox68.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox68.TabIndex = 354
        Me.CheckBox68.Text = "Coinsurance Payment"
        Me.CheckBox68.UseVisualStyleBackColor = False
        '
        'CheckBox69
        '
        Me.CheckBox69.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox69.Checked = True
        Me.CheckBox69.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox69.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox69.Enabled = False
        Me.CheckBox69.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox69.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox69.Location = New System.Drawing.Point(6, 116)
        Me.CheckBox69.Name = "CheckBox69"
        Me.CheckBox69.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox69.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox69.TabIndex = 353
        Me.CheckBox69.Text = "Risk Details"
        Me.CheckBox69.UseVisualStyleBackColor = False
        '
        'CheckBox70
        '
        Me.CheckBox70.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox70.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox70.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox70.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox70.Location = New System.Drawing.Point(6, 97)
        Me.CheckBox70.Name = "CheckBox70"
        Me.CheckBox70.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox70.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox70.TabIndex = 352
        Me.CheckBox70.Text = "Fast Track Claims Payments"
        Me.CheckBox70.UseVisualStyleBackColor = False
        '
        'CheckBox71
        '
        Me.CheckBox71.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox71.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox71.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox71.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox71.Location = New System.Drawing.Point(6, 78)
        Me.CheckBox71.Name = "CheckBox71"
        Me.CheckBox71.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox71.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox71.TabIndex = 351
        Me.CheckBox71.Text = "Check Deferred Reinsurance"
        Me.CheckBox71.UseVisualStyleBackColor = False
        '
        'CheckBox72
        '
        Me.CheckBox72.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox72.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox72.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox72.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox72.Location = New System.Drawing.Point(6, 59)
        Me.CheckBox72.Name = "CheckBox72"
        Me.CheckBox72.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox72.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox72.TabIndex = 350
        Me.CheckBox72.Text = "Check Unpaid Status"
        Me.CheckBox72.UseVisualStyleBackColor = False
        '
        'CheckBox73
        '
        Me.CheckBox73.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox73.Checked = True
        Me.CheckBox73.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox73.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox73.Enabled = False
        Me.CheckBox73.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox73.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox73.Location = New System.Drawing.Point(6, 40)
        Me.CheckBox73.Name = "CheckBox73"
        Me.CheckBox73.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox73.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox73.TabIndex = 349
        Me.CheckBox73.Text = "Claim Details"
        Me.CheckBox73.UseVisualStyleBackColor = False
        '
        'CheckBox74
        '
        Me.CheckBox74.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox74.Checked = True
        Me.CheckBox74.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox74.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox74.Enabled = False
        Me.CheckBox74.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox74.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox74.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox74.Name = "CheckBox74"
        Me.CheckBox74.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox74.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox74.TabIndex = 348
        Me.CheckBox74.Text = "Find Claim"
        Me.CheckBox74.UseVisualStyleBackColor = False
        '
        'GroupBox15
        '
        Me.GroupBox15.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox15.Controls.Add(Me.CheckBox75)
        Me.GroupBox15.Controls.Add(Me.CheckBox76)
        Me.GroupBox15.Controls.Add(Me.CheckBox77)
        Me.GroupBox15.Controls.Add(Me.CheckBox78)
        Me.GroupBox15.Controls.Add(Me.CheckBox79)
        Me.GroupBox15.Controls.Add(Me.CheckBox80)
        Me.GroupBox15.Controls.Add(Me.CheckBox81)
        Me.GroupBox15.Controls.Add(Me.CheckBox82)
        Me.GroupBox15.Controls.Add(Me.CheckBox83)
        Me.GroupBox15.Controls.Add(Me.CheckBox84)
        Me.GroupBox15.Controls.Add(Me.CheckBox85)
        Me.GroupBox15.Controls.Add(Me.CheckBox86)
        Me.GroupBox15.Controls.Add(Me.CheckBox87)
        Me.GroupBox15.Controls.Add(Me.CheckBox88)
        Me.GroupBox15.Controls.Add(Me.CheckBox89)
        Me.GroupBox15.Controls.Add(Me.CheckBox90)
        Me.GroupBox15.Controls.Add(Me.CheckBox91)
        Me.GroupBox15.Controls.Add(Me.CheckBox92)
        Me.GroupBox15.Controls.Add(Me.CheckBox93)
        Me.GroupBox15.Controls.Add(Me.CheckBox94)
        Me.GroupBox15.Controls.Add(Me.CheckBox95)
        Me.GroupBox15.Controls.Add(Me.CheckBox96)
        Me.GroupBox15.Controls.Add(Me.CheckBox97)
        Me.GroupBox15.Controls.Add(Me.CheckBox98)
        Me.GroupBox15.Controls.Add(Me.CheckBox99)
        Me.GroupBox15.Controls.Add(Me.CheckBox100)
        Me.GroupBox15.Controls.Add(Me.CheckBox101)
        Me.GroupBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox15.Location = New System.Drawing.Point(288, 9)
        Me.GroupBox15.Name = "GroupBox15"
        Me.GroupBox15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox15.Size = New System.Drawing.Size(264, 528)
        Me.GroupBox15.TabIndex = 320
        Me.GroupBox15.TabStop = False
        Me.GroupBox15.Text = "Maintain Claim"
        '
        'CheckBox75
        '
        Me.CheckBox75.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox75.Checked = True
        Me.CheckBox75.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox75.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox75.Enabled = False
        Me.CheckBox75.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox75.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox75.Location = New System.Drawing.Point(6, 412)
        Me.CheckBox75.Name = "CheckBox75"
        Me.CheckBox75.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox75.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox75.TabIndex = 377
        Me.CheckBox75.Text = "Close Claim Message"
        Me.CheckBox75.UseVisualStyleBackColor = False
        '
        'CheckBox76
        '
        Me.CheckBox76.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox76.Checked = True
        Me.CheckBox76.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox76.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox76.Enabled = False
        Me.CheckBox76.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox76.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox76.Location = New System.Drawing.Point(6, 511)
        Me.CheckBox76.Name = "CheckBox76"
        Me.CheckBox76.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox76.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox76.TabIndex = 346
        Me.CheckBox76.Text = "Unlock Claim"
        Me.CheckBox76.UseVisualStyleBackColor = False
        '
        'CheckBox77
        '
        Me.CheckBox77.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox77.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox77.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox77.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox77.Location = New System.Drawing.Point(6, 494)
        Me.CheckBox77.Name = "CheckBox77"
        Me.CheckBox77.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox77.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox77.TabIndex = 345
        Me.CheckBox77.Text = "Do you wish to make further payments?"
        Me.CheckBox77.UseVisualStyleBackColor = False
        '
        'CheckBox78
        '
        Me.CheckBox78.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox78.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox78.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox78.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox78.Location = New System.Drawing.Point(6, 475)
        Me.CheckBox78.Name = "CheckBox78"
        Me.CheckBox78.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox78.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox78.TabIndex = 344
        Me.CheckBox78.Text = "Produce Claim Payment Documents"
        Me.CheckBox78.UseVisualStyleBackColor = False
        '
        'CheckBox79
        '
        Me.CheckBox79.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox79.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox79.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox79.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox79.Location = New System.Drawing.Point(6, 446)
        Me.CheckBox79.Name = "CheckBox79"
        Me.CheckBox79.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox79.Size = New System.Drawing.Size(255, 30)
        Me.CheckBox79.TabIndex = 343
        Me.CheckBox79.Text = "Display Generate Claim Payment Documents Message"
        Me.CheckBox79.UseVisualStyleBackColor = False
        '
        'CheckBox80
        '
        Me.CheckBox80.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox80.Checked = True
        Me.CheckBox80.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox80.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox80.Enabled = False
        Me.CheckBox80.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox80.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox80.Location = New System.Drawing.Point(6, 430)
        Me.CheckBox80.Name = "CheckBox80"
        Me.CheckBox80.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox80.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox80.TabIndex = 342
        Me.CheckBox80.Text = "Check Status"
        Me.CheckBox80.UseVisualStyleBackColor = False
        '
        'CheckBox81
        '
        Me.CheckBox81.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox81.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox81.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox81.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox81.Location = New System.Drawing.Point(6, 395)
        Me.CheckBox81.Name = "CheckBox81"
        Me.CheckBox81.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox81.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox81.TabIndex = 341
        Me.CheckBox81.Text = "Cash Payments Process"
        Me.CheckBox81.UseVisualStyleBackColor = False
        '
        'CheckBox82
        '
        Me.CheckBox82.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox82.Checked = True
        Me.CheckBox82.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox82.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox82.Enabled = False
        Me.CheckBox82.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox82.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox82.Location = New System.Drawing.Point(6, 378)
        Me.CheckBox82.Name = "CheckBox82"
        Me.CheckBox82.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox82.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox82.TabIndex = 340
        Me.CheckBox82.Text = "Update Claim Details"
        Me.CheckBox82.UseVisualStyleBackColor = False
        '
        'CheckBox83
        '
        Me.CheckBox83.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox83.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox83.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox83.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox83.Location = New System.Drawing.Point(6, 361)
        Me.CheckBox83.Name = "CheckBox83"
        Me.CheckBox83.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox83.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox83.TabIndex = 339
        Me.CheckBox83.Text = "Enter Description for Change"
        Me.CheckBox83.UseVisualStyleBackColor = False
        '
        'CheckBox84
        '
        Me.CheckBox84.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox84.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox84.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox84.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox84.Location = New System.Drawing.Point(6, 344)
        Me.CheckBox84.Name = "CheckBox84"
        Me.CheckBox84.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox84.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox84.TabIndex = 338
        Me.CheckBox84.Text = "Reinsurance Payment"
        Me.CheckBox84.UseVisualStyleBackColor = False
        '
        'CheckBox85
        '
        Me.CheckBox85.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox85.Checked = True
        Me.CheckBox85.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox85.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox85.Enabled = False
        Me.CheckBox85.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox85.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox85.Location = New System.Drawing.Point(6, 327)
        Me.CheckBox85.Name = "CheckBox85"
        Me.CheckBox85.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox85.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox85.TabIndex = 337
        Me.CheckBox85.Text = "Coinsurance Payment"
        Me.CheckBox85.UseVisualStyleBackColor = False
        '
        'CheckBox86
        '
        Me.CheckBox86.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox86.Checked = True
        Me.CheckBox86.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox86.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox86.Enabled = False
        Me.CheckBox86.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox86.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox86.Location = New System.Drawing.Point(6, 310)
        Me.CheckBox86.Name = "CheckBox86"
        Me.CheckBox86.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox86.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox86.TabIndex = 336
        Me.CheckBox86.Text = "Risk Details"
        Me.CheckBox86.UseVisualStyleBackColor = False
        '
        'CheckBox87
        '
        Me.CheckBox87.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox87.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox87.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox87.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox87.Location = New System.Drawing.Point(6, 293)
        Me.CheckBox87.Name = "CheckBox87"
        Me.CheckBox87.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox87.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox87.TabIndex = 335
        Me.CheckBox87.Text = "Fast Track Claims Payments"
        Me.CheckBox87.UseVisualStyleBackColor = False
        '
        'CheckBox88
        '
        Me.CheckBox88.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox88.Checked = True
        Me.CheckBox88.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox88.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox88.Enabled = False
        Me.CheckBox88.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox88.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox88.Location = New System.Drawing.Point(6, 276)
        Me.CheckBox88.Name = "CheckBox88"
        Me.CheckBox88.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox88.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox88.TabIndex = 334
        Me.CheckBox88.Text = "Proceed to Claim Payments Message"
        Me.CheckBox88.UseVisualStyleBackColor = False
        '
        'CheckBox89
        '
        Me.CheckBox89.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox89.Checked = True
        Me.CheckBox89.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox89.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox89.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox89.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox89.Location = New System.Drawing.Point(6, 259)
        Me.CheckBox89.Name = "CheckBox89"
        Me.CheckBox89.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox89.Size = New System.Drawing.Size(255, 23)
        Me.CheckBox89.TabIndex = 333
        Me.CheckBox89.Text = "Claim Payments Process"
        Me.CheckBox89.UseVisualStyleBackColor = False
        '
        'CheckBox90
        '
        Me.CheckBox90.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox90.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox90.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox90.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox90.Location = New System.Drawing.Point(6, 241)
        Me.CheckBox90.Name = "CheckBox90"
        Me.CheckBox90.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox90.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox90.TabIndex = 332
        Me.CheckBox90.Text = "Produce Claim Notification Documents"
        Me.CheckBox90.UseVisualStyleBackColor = False
        '
        'CheckBox91
        '
        Me.CheckBox91.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox91.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox91.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox91.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox91.Location = New System.Drawing.Point(6, 212)
        Me.CheckBox91.Name = "CheckBox91"
        Me.CheckBox91.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox91.Size = New System.Drawing.Size(255, 30)
        Me.CheckBox91.TabIndex = 331
        Me.CheckBox91.Text = "Display Generate Claim Notification Documents Message"
        Me.CheckBox91.UseVisualStyleBackColor = False
        '
        'CheckBox92
        '
        Me.CheckBox92.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox92.Checked = True
        Me.CheckBox92.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox92.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox92.Enabled = False
        Me.CheckBox92.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox92.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox92.Location = New System.Drawing.Point(6, 173)
        Me.CheckBox92.Name = "CheckBox92"
        Me.CheckBox92.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox92.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox92.TabIndex = 329
        Me.CheckBox92.Text = "Update Claim Details"
        Me.CheckBox92.UseVisualStyleBackColor = False
        '
        'CheckBox93
        '
        Me.CheckBox93.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox93.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox93.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox93.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox93.Location = New System.Drawing.Point(6, 154)
        Me.CheckBox93.Name = "CheckBox93"
        Me.CheckBox93.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox93.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox93.TabIndex = 328
        Me.CheckBox93.Text = "Enter Description for Change"
        Me.CheckBox93.UseVisualStyleBackColor = False
        '
        'CheckBox94
        '
        Me.CheckBox94.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox94.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox94.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox94.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox94.Location = New System.Drawing.Point(6, 135)
        Me.CheckBox94.Name = "CheckBox94"
        Me.CheckBox94.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox94.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox94.TabIndex = 327
        Me.CheckBox94.Text = "Third Party Recovery"
        Me.CheckBox94.UseVisualStyleBackColor = False
        '
        'CheckBox95
        '
        Me.CheckBox95.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox95.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox95.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox95.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox95.Location = New System.Drawing.Point(6, 116)
        Me.CheckBox95.Name = "CheckBox95"
        Me.CheckBox95.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox95.Size = New System.Drawing.Size(255, 17)
        Me.CheckBox95.TabIndex = 326
        Me.CheckBox95.Text = "Salvage Recovery"
        Me.CheckBox95.UseVisualStyleBackColor = False
        '
        'CheckBox96
        '
        Me.CheckBox96.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox96.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox96.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox96.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox96.Location = New System.Drawing.Point(6, 97)
        Me.CheckBox96.Name = "CheckBox96"
        Me.CheckBox96.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox96.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox96.TabIndex = 325
        Me.CheckBox96.Text = "Reinsurance Recoveries"
        Me.CheckBox96.UseVisualStyleBackColor = False
        '
        'CheckBox97
        '
        Me.CheckBox97.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox97.Checked = True
        Me.CheckBox97.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox97.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox97.Enabled = False
        Me.CheckBox97.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox97.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox97.Location = New System.Drawing.Point(6, 78)
        Me.CheckBox97.Name = "CheckBox97"
        Me.CheckBox97.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox97.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox97.TabIndex = 324
        Me.CheckBox97.Text = "Coinsurance Recoveries"
        Me.CheckBox97.UseVisualStyleBackColor = False
        '
        'CheckBox98
        '
        Me.CheckBox98.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox98.Checked = True
        Me.CheckBox98.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox98.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox98.Enabled = False
        Me.CheckBox98.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox98.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox98.Location = New System.Drawing.Point(6, 59)
        Me.CheckBox98.Name = "CheckBox98"
        Me.CheckBox98.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox98.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox98.TabIndex = 323
        Me.CheckBox98.Text = "Risk Details"
        Me.CheckBox98.UseVisualStyleBackColor = False
        '
        'CheckBox99
        '
        Me.CheckBox99.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox99.Checked = True
        Me.CheckBox99.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox99.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox99.Enabled = False
        Me.CheckBox99.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox99.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox99.Location = New System.Drawing.Point(6, 40)
        Me.CheckBox99.Name = "CheckBox99"
        Me.CheckBox99.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox99.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox99.TabIndex = 322
        Me.CheckBox99.Text = "Claim Details"
        Me.CheckBox99.UseVisualStyleBackColor = False
        '
        'CheckBox100
        '
        Me.CheckBox100.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox100.Checked = True
        Me.CheckBox100.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox100.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox100.Enabled = False
        Me.CheckBox100.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox100.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox100.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox100.Name = "CheckBox100"
        Me.CheckBox100.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox100.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox100.TabIndex = 321
        Me.CheckBox100.Text = "Find Claim"
        Me.CheckBox100.UseVisualStyleBackColor = False
        '
        'CheckBox101
        '
        Me.CheckBox101.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox101.Checked = True
        Me.CheckBox101.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox101.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox101.Enabled = False
        Me.CheckBox101.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox101.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox101.Location = New System.Drawing.Point(6, 192)
        Me.CheckBox101.Name = "CheckBox101"
        Me.CheckBox101.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox101.Size = New System.Drawing.Size(255, 13)
        Me.CheckBox101.TabIndex = 330
        Me.CheckBox101.Text = "Check Status"
        Me.CheckBox101.UseVisualStyleBackColor = False
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.GroupBox16)
        Me.TabPage4.Controls.Add(Me.GroupBox17)
        Me.TabPage4.Controls.Add(Me.GroupBox18)
        Me.TabPage4.Controls.Add(Me.GroupBox19)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(836, 642)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "4-True Monthly Policies"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'GroupBox16
        '
        Me.GroupBox16.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox16.Controls.Add(Me.TextBox17)
        Me.GroupBox16.Controls.Add(Me.TextBox18)
        Me.GroupBox16.Controls.Add(Me.Label48)
        Me.GroupBox16.Controls.Add(Me.Label49)
        Me.GroupBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox16.Location = New System.Drawing.Point(8, 16)
        Me.GroupBox16.Name = "GroupBox16"
        Me.GroupBox16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox16.Size = New System.Drawing.Size(400, 109)
        Me.GroupBox16.TabIndex = 97
        Me.GroupBox16.TabStop = False
        Me.GroupBox16.Text = "Renewals"
        '
        'TextBox17
        '
        Me.TextBox17.AcceptsReturn = True
        Me.TextBox17.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox17.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox17.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox17.Location = New System.Drawing.Point(294, 56)
        Me.TextBox17.MaxLength = 0
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox17.Size = New System.Drawing.Size(83, 20)
        Me.TextBox17.TabIndex = 101
        '
        'TextBox18
        '
        Me.TextBox18.AcceptsReturn = True
        Me.TextBox18.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox18.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox18.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox18.Location = New System.Drawing.Point(294, 24)
        Me.TextBox18.MaxLength = 0
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox18.Size = New System.Drawing.Size(83, 20)
        Me.TextBox18.TabIndex = 99
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.BackColor = System.Drawing.SystemColors.Control
        Me.Label48.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label48.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label48.Location = New System.Drawing.Point(8, 58)
        Me.Label48.Name = "Label48"
        Me.Label48.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label48.Size = New System.Drawing.Size(110, 13)
        Me.Label48.TabIndex = 100
        Me.Label48.Text = "Unified Renewal Day:"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.BackColor = System.Drawing.SystemColors.Control
        Me.Label49.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label49.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label49.Location = New System.Drawing.Point(8, 24)
        Me.Label49.Name = "Label49"
        Me.Label49.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label49.Size = New System.Drawing.Size(196, 13)
        Me.Label49.TabIndex = 98
        Me.Label49.Text = "Anniversary Renewal Weeks:"
        '
        'GroupBox17
        '
        Me.GroupBox17.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox17.Controls.Add(Me.AccountLookup1)
        Me.GroupBox17.Controls.Add(Me.ComboBox7)
        Me.GroupBox17.Controls.Add(Me.CheckBox102)
        Me.GroupBox17.Controls.Add(Me.Label50)
        Me.GroupBox17.Controls.Add(Me.Label51)
        Me.GroupBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox17.Location = New System.Drawing.Point(421, 14)
        Me.GroupBox17.Name = "GroupBox17"
        Me.GroupBox17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox17.Size = New System.Drawing.Size(402, 113)
        Me.GroupBox17.TabIndex = 102
        Me.GroupBox17.TabStop = False
        Me.GroupBox17.Text = "Lead Agent Commission"
        '
        'AccountLookup1
        '
        Me.AccountLookup1.AccountId = 0
        Me.AccountLookup1.AllowStoppedAccounts = False
        Me.AccountLookup1.BackStyle = 0
        Me.AccountLookup1.CompanyId = 0
        Me.AccountLookup1.Default_Renamed = False
        Me.AccountLookup1.Location = New System.Drawing.Point(232, 80)
        Me.AccountLookup1.LookupCaption = "..."
        Me.AccountLookup1.LookupHeight = 285
        Me.AccountLookup1.LookupLeft = 2070
        Me.AccountLookup1.LookupTextLeft = 0
        Me.AccountLookup1.LookupTextWidth = 2070
        Me.AccountLookup1.LookupWidth = 360
        Me.AccountLookup1.Name = "AccountLookup1"
        Me.AccountLookup1.OnlyUpdatableAccounts = False
        Me.AccountLookup1.SelLength = 0
        Me.AccountLookup1.SelStart = 0
        Me.AccountLookup1.SelText = ""
        Me.AccountLookup1.ShowEditOnFindAccount = False
        Me.AccountLookup1.Size = New System.Drawing.Size(162, 19)
        Me.AccountLookup1.TabIndex = 107
        Me.AccountLookup1.ToolTipText = ""
        Me.AccountLookup1.Visible = False
        '
        'CheckBox102
        '
        Me.CheckBox102.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox102.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox102.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox102.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox102.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox102.Location = New System.Drawing.Point(8, 24)
        Me.CheckBox102.Name = "CheckBox102"
        Me.CheckBox102.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox102.Size = New System.Drawing.Size(237, 17)
        Me.CheckBox102.TabIndex = 103
        Me.CheckBox102.Text = "Allow Consolidate Commission:"
        Me.CheckBox102.UseVisualStyleBackColor = False
        '
        'Label50
        '
        Me.Label50.BackColor = System.Drawing.SystemColors.Control
        Me.Label50.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label50.Location = New System.Drawing.Point(8, 80)
        Me.Label50.Name = "Label50"
        Me.Label50.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label50.Size = New System.Drawing.Size(225, 17)
        Me.Label50.TabIndex = 106
        Me.Label50.Text = "Lead Agent Commission Suspense a/c:"
        Me.Label50.Visible = False
        '
        'Label51
        '
        Me.Label51.BackColor = System.Drawing.SystemColors.Control
        Me.Label51.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label51.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label51.Location = New System.Drawing.Point(8, 52)
        Me.Label51.Name = "Label51"
        Me.Label51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label51.Size = New System.Drawing.Size(105, 17)
        Me.Label51.TabIndex = 104
        Me.Label51.Text = "Month in Cycle:"
        Me.Label51.Visible = False
        '
        'GroupBox18
        '
        Me.GroupBox18.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox18.Controls.Add(Me.AccountLookup2)
        Me.GroupBox18.Controls.Add(Me.ComboBox8)
        Me.GroupBox18.Controls.Add(Me.CheckBox103)
        Me.GroupBox18.Controls.Add(Me.Label52)
        Me.GroupBox18.Controls.Add(Me.Label53)
        Me.GroupBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox18.Location = New System.Drawing.Point(423, 132)
        Me.GroupBox18.Name = "GroupBox18"
        Me.GroupBox18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox18.Size = New System.Drawing.Size(399, 105)
        Me.GroupBox18.TabIndex = 111
        Me.GroupBox18.TabStop = False
        Me.GroupBox18.Text = "Sub-Agent Commission"
        '
        'AccountLookup2
        '
        Me.AccountLookup2.AccountId = 0
        Me.AccountLookup2.AllowStoppedAccounts = False
        Me.AccountLookup2.BackStyle = 0
        Me.AccountLookup2.CompanyId = 0
        Me.AccountLookup2.Default_Renamed = False
        Me.AccountLookup2.Location = New System.Drawing.Point(232, 79)
        Me.AccountLookup2.LookupCaption = "..."
        Me.AccountLookup2.LookupHeight = 285
        Me.AccountLookup2.LookupLeft = 2025
        Me.AccountLookup2.LookupTextLeft = 0
        Me.AccountLookup2.LookupTextWidth = 2025
        Me.AccountLookup2.LookupWidth = 360
        Me.AccountLookup2.Name = "AccountLookup2"
        Me.AccountLookup2.OnlyUpdatableAccounts = False
        Me.AccountLookup2.SelLength = 0
        Me.AccountLookup2.SelStart = 0
        Me.AccountLookup2.SelText = ""
        Me.AccountLookup2.ShowEditOnFindAccount = False
        Me.AccountLookup2.Size = New System.Drawing.Size(159, 19)
        Me.AccountLookup2.TabIndex = 116
        Me.AccountLookup2.ToolTipText = ""
        Me.AccountLookup2.Visible = False
        '
        'CheckBox103
        '
        Me.CheckBox103.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox103.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox103.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox103.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox103.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox103.Location = New System.Drawing.Point(7, 24)
        Me.CheckBox103.Name = "CheckBox103"
        Me.CheckBox103.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox103.Size = New System.Drawing.Size(237, 17)
        Me.CheckBox103.TabIndex = 112
        Me.CheckBox103.Text = "Allow Consolidate Commission:"
        Me.CheckBox103.UseVisualStyleBackColor = False
        '
        'Label52
        '
        Me.Label52.BackColor = System.Drawing.SystemColors.Control
        Me.Label52.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label52.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label52.Location = New System.Drawing.Point(8, 80)
        Me.Label52.Name = "Label52"
        Me.Label52.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label52.Size = New System.Drawing.Size(225, 17)
        Me.Label52.TabIndex = 115
        Me.Label52.Text = "Sub-Agent Commission Suspense a/c:"
        Me.Label52.Visible = False
        '
        'Label53
        '
        Me.Label53.BackColor = System.Drawing.SystemColors.Control
        Me.Label53.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label53.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label53.Location = New System.Drawing.Point(8, 48)
        Me.Label53.Name = "Label53"
        Me.Label53.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label53.Size = New System.Drawing.Size(177, 17)
        Me.Label53.TabIndex = 113
        Me.Label53.Text = "Month in Cycle:"
        Me.Label53.Visible = False
        '
        'GroupBox19
        '
        Me.GroupBox19.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox19.Controls.Add(Me.RadioButton1)
        Me.GroupBox19.Controls.Add(Me.RadioButton2)
        Me.GroupBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox19.Location = New System.Drawing.Point(8, 134)
        Me.GroupBox19.Name = "GroupBox19"
        Me.GroupBox19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox19.Size = New System.Drawing.Size(397, 65)
        Me.GroupBox19.TabIndex = 108
        Me.GroupBox19.TabStop = False
        Me.GroupBox19.Text = "Payment Method"
        '
        'RadioButton1
        '
        Me.RadioButton1.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton1.Location = New System.Drawing.Point(144, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton1.Size = New System.Drawing.Size(89, 25)
        Me.RadioButton1.TabIndex = 110
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Instalments"
        Me.RadioButton1.UseVisualStyleBackColor = False
        '
        'RadioButton2
        '
        Me.RadioButton2.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton2.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton2.Location = New System.Drawing.Point(16, 24)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton2.Size = New System.Drawing.Size(73, 25)
        Me.RadioButton2.TabIndex = 109
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Invoice"
        Me.RadioButton2.UseVisualStyleBackColor = False
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.GroupBox20)
        Me.TabPage5.Controls.Add(Me.GroupBox21)
        Me.TabPage5.Controls.Add(Me.GroupBox22)
        Me.TabPage5.Controls.Add(Me.GroupBox23)
        Me.TabPage5.Controls.Add(Me.GroupBox29)
        Me.TabPage5.Controls.Add(Me.GroupBox30)
        Me.TabPage5.Controls.Add(Me.GroupBox31)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(836, 642)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "5-Additional Options"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'GroupBox20
        '
        Me.GroupBox20.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox20.Controls.Add(Me.CheckBox104)
        Me.GroupBox20.Controls.Add(Me.CheckBox105)
        Me.GroupBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox20.Location = New System.Drawing.Point(8, 148)
        Me.GroupBox20.Name = "GroupBox20"
        Me.GroupBox20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox20.Size = New System.Drawing.Size(519, 65)
        Me.GroupBox20.TabIndex = 392
        Me.GroupBox20.TabStop = False
        Me.GroupBox20.Text = "Media Type Status Validation"
        '
        'CheckBox104
        '
        Me.CheckBox104.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox104.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox104.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox104.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox104.Location = New System.Drawing.Point(32, 19)
        Me.CheckBox104.Name = "CheckBox104"
        Me.CheckBox104.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox104.Size = New System.Drawing.Size(359, 17)
        Me.CheckBox104.TabIndex = 394
        Me.CheckBox104.Text = "Claim Payment - Check Media Type Status as Cleared"
        Me.CheckBox104.UseVisualStyleBackColor = False
        '
        'CheckBox105
        '
        Me.CheckBox105.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox105.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox105.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox105.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox105.Location = New System.Drawing.Point(32, 42)
        Me.CheckBox105.Name = "CheckBox105"
        Me.CheckBox105.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox105.Size = New System.Drawing.Size(405, 17)
        Me.CheckBox105.TabIndex = 393
        Me.CheckBox105.Text = "Refund MTA/MTC - Check Media Type Status as Cleared"
        Me.CheckBox105.UseVisualStyleBackColor = False
        '
        'GroupBox21
        '
        Me.GroupBox21.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox21.Controls.Add(Me.Button3)
        Me.GroupBox21.Controls.Add(Me.TextBox19)
        Me.GroupBox21.Controls.Add(Me.TextBox20)
        Me.GroupBox21.Controls.Add(Me.TextBox21)
        Me.GroupBox21.Controls.Add(Me.Label54)
        Me.GroupBox21.Controls.Add(Me.Label55)
        Me.GroupBox21.Controls.Add(Me.Label56)
        Me.GroupBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox21.Location = New System.Drawing.Point(6, 558)
        Me.GroupBox21.Name = "GroupBox21"
        Me.GroupBox21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox21.Size = New System.Drawing.Size(827, 63)
        Me.GroupBox21.TabIndex = 167
        Me.GroupBox21.TabStop = False
        Me.GroupBox21.Text = "Cover Note"
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.SystemColors.Control
        Me.Button3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button3.Location = New System.Drawing.Point(770, 16)
        Me.Button3.Name = "Button3"
        Me.Button3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button3.Size = New System.Drawing.Size(37, 19)
        Me.Button3.TabIndex = 174
        Me.Button3.Text = "..."
        Me.Button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button3.UseVisualStyleBackColor = False
        '
        'TextBox19
        '
        Me.TextBox19.AcceptsReturn = True
        Me.TextBox19.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox19.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox19.Enabled = False
        Me.TextBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox19.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox19.Location = New System.Drawing.Point(630, 16)
        Me.TextBox19.MaxLength = 0
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox19.Size = New System.Drawing.Size(137, 20)
        Me.TextBox19.TabIndex = 173
        '
        'TextBox20
        '
        Me.TextBox20.AcceptsReturn = True
        Me.TextBox20.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox20.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox20.Enabled = False
        Me.TextBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox20.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox20.Location = New System.Drawing.Point(262, 38)
        Me.TextBox20.MaxLength = 0
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox20.Size = New System.Drawing.Size(111, 20)
        Me.TextBox20.TabIndex = 171
        '
        'TextBox21
        '
        Me.TextBox21.AcceptsReturn = True
        Me.TextBox21.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox21.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox21.Enabled = False
        Me.TextBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox21.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox21.Location = New System.Drawing.Point(262, 16)
        Me.TextBox21.MaxLength = 0
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox21.Size = New System.Drawing.Size(111, 20)
        Me.TextBox21.TabIndex = 169
        '
        'Label54
        '
        Me.Label54.BackColor = System.Drawing.SystemColors.Control
        Me.Label54.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label54.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label54.Location = New System.Drawing.Point(420, 18)
        Me.Label54.Name = "Label54"
        Me.Label54.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label54.Size = New System.Drawing.Size(209, 25)
        Me.Label54.TabIndex = 172
        Me.Label54.Text = "Cover Note Doc Template"
        '
        'Label55
        '
        Me.Label55.BackColor = System.Drawing.SystemColors.Control
        Me.Label55.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label55.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label55.Location = New System.Drawing.Point(10, 40)
        Me.Label55.Name = "Label55"
        Me.Label55.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label55.Size = New System.Drawing.Size(247, 21)
        Me.Label55.TabIndex = 170
        Me.Label55.Text = "Maximum No. of Cover Notes"
        '
        'Label56
        '
        Me.Label56.BackColor = System.Drawing.SystemColors.Control
        Me.Label56.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label56.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label56.Location = New System.Drawing.Point(10, 18)
        Me.Label56.Name = "Label56"
        Me.Label56.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label56.Size = New System.Drawing.Size(235, 20)
        Me.Label56.TabIndex = 168
        Me.Label56.Text = "Cover Note default period"
        '
        'GroupBox22
        '
        Me.GroupBox22.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox22.Controls.Add(Me.ComboBox9)
        Me.GroupBox22.Controls.Add(Me.ComboBox10)
        Me.GroupBox22.Controls.Add(Me.ComboBox11)
        Me.GroupBox22.Controls.Add(Me.ComboBox12)
        Me.GroupBox22.Controls.Add(Me.Label57)
        Me.GroupBox22.Controls.Add(Me.Label58)
        Me.GroupBox22.Controls.Add(Me.Label59)
        Me.GroupBox22.Controls.Add(Me.Label60)
        Me.GroupBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox22.Location = New System.Drawing.Point(536, 68)
        Me.GroupBox22.Name = "GroupBox22"
        Me.GroupBox22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox22.Size = New System.Drawing.Size(297, 139)
        Me.GroupBox22.TabIndex = 367
        Me.GroupBox22.TabStop = False
        Me.GroupBox22.Text = "Out of Sequence MTAs"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.BackColor = System.Drawing.SystemColors.Control
        Me.Label57.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label57.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label57.Location = New System.Drawing.Point(16, 104)
        Me.Label57.Name = "Label57"
        Me.Label57.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label57.Size = New System.Drawing.Size(66, 13)
        Me.Label57.TabIndex = 389
        Me.Label57.Text = "Task Group:"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.BackColor = System.Drawing.SystemColors.Control
        Me.Label58.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label58.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label58.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label58.Location = New System.Drawing.Point(16, 80)
        Me.Label58.Name = "Label58"
        Me.Label58.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label58.Size = New System.Drawing.Size(64, 13)
        Me.Label58.TabIndex = 387
        Me.Label58.Text = "User Group:"
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.BackColor = System.Drawing.SystemColors.Control
        Me.Label59.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label59.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label59.Location = New System.Drawing.Point(16, 28)
        Me.Label59.Name = "Label59"
        Me.Label59.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label59.Size = New System.Drawing.Size(77, 13)
        Me.Label59.TabIndex = 371
        Me.Label59.Text = "Dates allowed:"
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.BackColor = System.Drawing.SystemColors.Control
        Me.Label60.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label60.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label60.Location = New System.Drawing.Point(16, 52)
        Me.Label60.Name = "Label60"
        Me.Label60.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label60.Size = New System.Drawing.Size(56, 13)
        Me.Label60.TabIndex = 370
        Me.Label60.Text = "Allocation:"
        '
        'GroupBox23
        '
        Me.GroupBox23.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox23.Controls.Add(Me.GroupBox24)
        Me.GroupBox23.Controls.Add(Me.GroupBox25)
        Me.GroupBox23.Controls.Add(Me.GroupBox26)
        Me.GroupBox23.Controls.Add(Me.GroupBox27)
        Me.GroupBox23.Controls.Add(Me.GroupBox28)
        Me.GroupBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox23.Location = New System.Drawing.Point(7, 220)
        Me.GroupBox23.Name = "GroupBox23"
        Me.GroupBox23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox23.Size = New System.Drawing.Size(826, 319)
        Me.GroupBox23.TabIndex = 134
        Me.GroupBox23.TabStop = False
        Me.GroupBox23.Text = "Claim System Configuration Options"
        '
        'GroupBox24
        '
        Me.GroupBox24.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox24.Controls.Add(Me.CheckBox106)
        Me.GroupBox24.Controls.Add(Me.CheckBox107)
        Me.GroupBox24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox24.Location = New System.Drawing.Point(516, 282)
        Me.GroupBox24.Name = "GroupBox24"
        Me.GroupBox24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox24.Size = New System.Drawing.Size(303, 71)
        Me.GroupBox24.TabIndex = 372
        Me.GroupBox24.TabStop = False
        Me.GroupBox24.Text = "Backdated MTA"
        '
        'CheckBox106
        '
        Me.CheckBox106.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox106.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox106.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox106.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox106.Location = New System.Drawing.Point(16, 17)
        Me.CheckBox106.Name = "CheckBox106"
        Me.CheckBox106.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox106.Size = New System.Drawing.Size(214, 13)
        Me.CheckBox106.TabIndex = 373
        Me.CheckBox106.Text = "Backdated MTA's Allowed"
        Me.CheckBox106.UseVisualStyleBackColor = False
        '
        'CheckBox107
        '
        Me.CheckBox107.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox107.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox107.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox107.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox107.Location = New System.Drawing.Point(16, 35)
        Me.CheckBox107.Name = "CheckBox107"
        Me.CheckBox107.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox107.Size = New System.Drawing.Size(209, 17)
        Me.CheckBox107.TabIndex = 391
        Me.CheckBox107.Text = "Backdated Cancellation Allowed"
        Me.CheckBox107.UseVisualStyleBackColor = False
        '
        'GroupBox25
        '
        Me.GroupBox25.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox25.Controls.Add(Me.CheckBox108)
        Me.GroupBox25.Controls.Add(Me.CheckBox109)
        Me.GroupBox25.Controls.Add(Me.CheckBox110)
        Me.GroupBox25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox25.Location = New System.Drawing.Point(516, 193)
        Me.GroupBox25.Name = "GroupBox25"
        Me.GroupBox25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox25.Size = New System.Drawing.Size(303, 87)
        Me.GroupBox25.TabIndex = 163
        Me.GroupBox25.TabStop = False
        Me.GroupBox25.Text = "Claim Numbering"
        '
        'CheckBox108
        '
        Me.CheckBox108.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox108.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox108.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox108.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox108.Location = New System.Drawing.Point(16, 23)
        Me.CheckBox108.Name = "CheckBox108"
        Me.CheckBox108.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox108.Size = New System.Drawing.Size(214, 20)
        Me.CheckBox108.TabIndex = 164
        Me.CheckBox108.Text = "Duplicate Claim Check Enabled"
        Me.CheckBox108.UseVisualStyleBackColor = False
        '
        'CheckBox109
        '
        Me.CheckBox109.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox109.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox109.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox109.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox109.Location = New System.Drawing.Point(16, 43)
        Me.CheckBox109.Name = "CheckBox109"
        Me.CheckBox109.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox109.Size = New System.Drawing.Size(214, 20)
        Me.CheckBox109.TabIndex = 165
        Me.CheckBox109.Text = "Advanced Tax Script"
        Me.CheckBox109.UseVisualStyleBackColor = False
        '
        'CheckBox110
        '
        Me.CheckBox110.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox110.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox110.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox110.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox110.Location = New System.Drawing.Point(16, 63)
        Me.CheckBox110.Name = "CheckBox110"
        Me.CheckBox110.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox110.Size = New System.Drawing.Size(214, 20)
        Me.CheckBox110.TabIndex = 166
        Me.CheckBox110.TabStop = False
        Me.CheckBox110.Text = "Payment Reference Check"
        Me.CheckBox110.UseVisualStyleBackColor = False
        '
        'GroupBox26
        '
        Me.GroupBox26.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox26.Controls.Add(Me.ComboBox13)
        Me.GroupBox26.Controls.Add(Me.ComboBox14)
        Me.GroupBox26.Controls.Add(Me.ComboBox15)
        Me.GroupBox26.Controls.Add(Me.ComboBox16)
        Me.GroupBox26.Controls.Add(Me.ComboBox17)
        Me.GroupBox26.Controls.Add(Me.Label61)
        Me.GroupBox26.Controls.Add(Me.Label62)
        Me.GroupBox26.Controls.Add(Me.Label63)
        Me.GroupBox26.Controls.Add(Me.Label64)
        Me.GroupBox26.Controls.Add(Me.Label65)
        Me.GroupBox26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox26.Location = New System.Drawing.Point(514, 16)
        Me.GroupBox26.Name = "GroupBox26"
        Me.GroupBox26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox26.Size = New System.Drawing.Size(303, 175)
        Me.GroupBox26.TabIndex = 143
        Me.GroupBox26.TabStop = False
        Me.GroupBox26.Text = "User Defined Tables"
        '
        'Label61
        '
        Me.Label61.BackColor = System.Drawing.SystemColors.Control
        Me.Label61.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label61.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label61.Location = New System.Drawing.Point(14, 80)
        Me.Label61.Name = "Label61"
        Me.Label61.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label61.Size = New System.Drawing.Size(67, 13)
        Me.Label61.TabIndex = 148
        Me.Label61.Text = "Table C :"
        '
        'Label62
        '
        Me.Label62.BackColor = System.Drawing.SystemColors.Control
        Me.Label62.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label62.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label62.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label62.Location = New System.Drawing.Point(14, 107)
        Me.Label62.Name = "Label62"
        Me.Label62.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label62.Size = New System.Drawing.Size(67, 13)
        Me.Label62.TabIndex = 150
        Me.Label62.Text = "Table D :"
        '
        'Label63
        '
        Me.Label63.BackColor = System.Drawing.SystemColors.Control
        Me.Label63.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label63.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label63.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label63.Location = New System.Drawing.Point(14, 27)
        Me.Label63.Name = "Label63"
        Me.Label63.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label63.Size = New System.Drawing.Size(67, 13)
        Me.Label63.TabIndex = 144
        Me.Label63.Text = "Table A :"
        '
        'Label64
        '
        Me.Label64.BackColor = System.Drawing.SystemColors.Control
        Me.Label64.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label64.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label64.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label64.Location = New System.Drawing.Point(14, 53)
        Me.Label64.Name = "Label64"
        Me.Label64.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label64.Size = New System.Drawing.Size(67, 13)
        Me.Label64.TabIndex = 146
        Me.Label64.Text = "Table B :"
        '
        'Label65
        '
        Me.Label65.BackColor = System.Drawing.SystemColors.Control
        Me.Label65.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label65.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label65.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label65.Location = New System.Drawing.Point(14, 134)
        Me.Label65.Name = "Label65"
        Me.Label65.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label65.Size = New System.Drawing.Size(67, 13)
        Me.Label65.TabIndex = 152
        Me.Label65.Text = "Table E :"
        '
        'GroupBox27
        '
        Me.GroupBox27.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox27.Controls.Add(Me.ComboBox18)
        Me.GroupBox27.Controls.Add(Me.ComboBox19)
        Me.GroupBox27.Controls.Add(Me.TextBox22)
        Me.GroupBox27.Controls.Add(Me.TextBox23)
        Me.GroupBox27.Controls.Add(Me.Label66)
        Me.GroupBox27.Controls.Add(Me.Label67)
        Me.GroupBox27.Controls.Add(Me.Label68)
        Me.GroupBox27.Controls.Add(Me.Label69)
        Me.GroupBox27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox27.Location = New System.Drawing.Point(7, 193)
        Me.GroupBox27.Name = "GroupBox27"
        Me.GroupBox27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox27.Size = New System.Drawing.Size(496, 119)
        Me.GroupBox27.TabIndex = 154
        Me.GroupBox27.TabStop = False
        Me.GroupBox27.Text = "External Claim Handler"
        '
        'TextBox22
        '
        Me.TextBox22.AcceptsReturn = True
        Me.TextBox22.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox22.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox22.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox22.Location = New System.Drawing.Point(282, 15)
        Me.TextBox22.MaxLength = 3
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox22.Size = New System.Drawing.Size(52, 20)
        Me.TextBox22.TabIndex = 156
        '
        'TextBox23
        '
        Me.TextBox23.AcceptsReturn = True
        Me.TextBox23.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox23.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox23.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox23.Location = New System.Drawing.Point(282, 40)
        Me.TextBox23.MaxLength = 3
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox23.Size = New System.Drawing.Size(52, 20)
        Me.TextBox23.TabIndex = 158
        '
        'Label66
        '
        Me.Label66.BackColor = System.Drawing.SystemColors.Control
        Me.Label66.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label66.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label66.Location = New System.Drawing.Point(15, 67)
        Me.Label66.Name = "Label66"
        Me.Label66.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label66.Size = New System.Drawing.Size(210, 13)
        Me.Label66.TabIndex = 159
        Me.Label66.Text = "Claim Task Group:"
        '
        'Label67
        '
        Me.Label67.BackColor = System.Drawing.SystemColors.Control
        Me.Label67.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label67.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label67.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label67.Location = New System.Drawing.Point(16, 92)
        Me.Label67.Name = "Label67"
        Me.Label67.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label67.Size = New System.Drawing.Size(210, 13)
        Me.Label67.TabIndex = 161
        Me.Label67.Text = "Claim User Group:"
        '
        'Label68
        '
        Me.Label68.BackColor = System.Drawing.SystemColors.Control
        Me.Label68.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label68.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label68.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label68.Location = New System.Drawing.Point(14, 18)
        Me.Label68.Name = "Label68"
        Me.Label68.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label68.Size = New System.Drawing.Size(210, 13)
        Me.Label68.TabIndex = 155
        Me.Label68.Text = "Acknowledged Task Allowed Time"
        '
        'Label69
        '
        Me.Label69.BackColor = System.Drawing.SystemColors.Control
        Me.Label69.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label69.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label69.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label69.Location = New System.Drawing.Point(14, 42)
        Me.Label69.Name = "Label69"
        Me.Label69.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label69.Size = New System.Drawing.Size(261, 20)
        Me.Label69.TabIndex = 157
        Me.Label69.Text = "Supply Preliminary Report Task Allowed Time"
        '
        'GroupBox28
        '
        Me.GroupBox28.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox28.Controls.Add(Me.CheckBox111)
        Me.GroupBox28.Controls.Add(Me.CheckBox112)
        Me.GroupBox28.Controls.Add(Me.CheckBox113)
        Me.GroupBox28.Controls.Add(Me.CheckBox114)
        Me.GroupBox28.Controls.Add(Me.TextBox24)
        Me.GroupBox28.Controls.Add(Me.ComboBox20)
        Me.GroupBox28.Controls.Add(Me.Label70)
        Me.GroupBox28.Controls.Add(Me.Label71)
        Me.GroupBox28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox28.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox28.Location = New System.Drawing.Point(8, 16)
        Me.GroupBox28.Name = "GroupBox28"
        Me.GroupBox28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox28.Size = New System.Drawing.Size(494, 175)
        Me.GroupBox28.TabIndex = 135
        Me.GroupBox28.TabStop = False
        Me.GroupBox28.Text = "Underwriting Claims"
        '
        'CheckBox111
        '
        Me.CheckBox111.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox111.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox111.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox111.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox111.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox111.Location = New System.Drawing.Point(10, 154)
        Me.CheckBox111.Name = "CheckBox111"
        Me.CheckBox111.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox111.Size = New System.Drawing.Size(279, 20)
        Me.CheckBox111.TabIndex = 375
        Me.CheckBox111.Text = "Payment Cannot Exceed Reserve"
        Me.CheckBox111.UseVisualStyleBackColor = False
        '
        'CheckBox112
        '
        Me.CheckBox112.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox112.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox112.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox112.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox112.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox112.Location = New System.Drawing.Point(10, 60)
        Me.CheckBox112.Name = "CheckBox112"
        Me.CheckBox112.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox112.Size = New System.Drawing.Size(279, 20)
        Me.CheckBox112.TabIndex = 138
        Me.CheckBox112.Text = "Allow Negative Reserves"
        Me.CheckBox112.UseVisualStyleBackColor = False
        '
        'CheckBox113
        '
        Me.CheckBox113.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox113.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox113.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox113.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox113.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox113.Location = New System.Drawing.Point(10, 83)
        Me.CheckBox113.Name = "CheckBox113"
        Me.CheckBox113.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox113.Size = New System.Drawing.Size(279, 20)
        Me.CheckBox113.TabIndex = 139
        Me.CheckBox113.Text = "Display only valid policy version at loss date"
        Me.CheckBox113.UseVisualStyleBackColor = False
        '
        'CheckBox114
        '
        Me.CheckBox114.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox114.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox114.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox114.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox114.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox114.Location = New System.Drawing.Point(10, 106)
        Me.CheckBox114.Name = "CheckBox114"
        Me.CheckBox114.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox114.Size = New System.Drawing.Size(279, 20)
        Me.CheckBox114.TabIndex = 140
        Me.CheckBox114.Text = "Claim Payment Amount is Gross"
        Me.CheckBox114.UseVisualStyleBackColor = False
        '
        'TextBox24
        '
        Me.TextBox24.AcceptsReturn = True
        Me.TextBox24.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox24.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox24.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox24.Location = New System.Drawing.Point(276, 28)
        Me.TextBox24.MaxLength = 20
        Me.TextBox24.Name = "TextBox24"
        Me.TextBox24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox24.Size = New System.Drawing.Size(168, 20)
        Me.TextBox24.TabIndex = 137
        '
        'Label70
        '
        Me.Label70.BackColor = System.Drawing.SystemColors.Control
        Me.Label70.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label70.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label70.Location = New System.Drawing.Point(12, 22)
        Me.Label70.Name = "Label70"
        Me.Label70.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label70.Size = New System.Drawing.Size(214, 27)
        Me.Label70.TabIndex = 136
        Me.Label70.Text = "Reinsurance Large Loss Advice Required When Claim Value Exceeds"
        '
        'Label71
        '
        Me.Label71.BackColor = System.Drawing.SystemColors.Control
        Me.Label71.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label71.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label71.Location = New System.Drawing.Point(12, 130)
        Me.Label71.Name = "Label71"
        Me.Label71.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label71.Size = New System.Drawing.Size(212, 13)
        Me.Label71.TabIndex = 141
        Me.Label71.Text = "Inclusion of Co-Insurers on Claims"
        '
        'GroupBox29
        '
        Me.GroupBox29.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox29.Controls.Add(Me.TextBox25)
        Me.GroupBox29.Controls.Add(Me.CheckBox115)
        Me.GroupBox29.Controls.Add(Me.CheckBox116)
        Me.GroupBox29.Controls.Add(Me.CheckBox117)
        Me.GroupBox29.Controls.Add(Me.Label72)
        Me.GroupBox29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox29.Location = New System.Drawing.Point(8, 68)
        Me.GroupBox29.Name = "GroupBox29"
        Me.GroupBox29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox29.Size = New System.Drawing.Size(519, 79)
        Me.GroupBox29.TabIndex = 128
        Me.GroupBox29.TabStop = False
        Me.GroupBox29.Text = "Nexus"
        '
        'TextBox25
        '
        Me.TextBox25.AcceptsReturn = True
        Me.TextBox25.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox25.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox25.Enabled = False
        Me.TextBox25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox25.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox25.Location = New System.Drawing.Point(176, 52)
        Me.TextBox25.MaxLength = 0
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox25.Size = New System.Drawing.Size(177, 20)
        Me.TextBox25.TabIndex = 133
        '
        'CheckBox115
        '
        Me.CheckBox115.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox115.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox115.Enabled = False
        Me.CheckBox115.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox115.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox115.Location = New System.Drawing.Point(384, 21)
        Me.CheckBox115.Name = "CheckBox115"
        Me.CheckBox115.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox115.Size = New System.Drawing.Size(129, 17)
        Me.CheckBox115.TabIndex = 131
        Me.CheckBox115.Text = "Trade RNL on-line"
        Me.CheckBox115.UseVisualStyleBackColor = False
        '
        'CheckBox116
        '
        Me.CheckBox116.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox116.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox116.Enabled = False
        Me.CheckBox116.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox116.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox116.Location = New System.Drawing.Point(208, 21)
        Me.CheckBox116.Name = "CheckBox116"
        Me.CheckBox116.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox116.Size = New System.Drawing.Size(177, 17)
        Me.CheckBox116.TabIndex = 130
        Me.CheckBox116.Text = "Trade MTA on-line"
        Me.CheckBox116.UseVisualStyleBackColor = False
        '
        'CheckBox117
        '
        Me.CheckBox117.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox117.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox117.Enabled = False
        Me.CheckBox117.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox117.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox117.Location = New System.Drawing.Point(32, 21)
        Me.CheckBox117.Name = "CheckBox117"
        Me.CheckBox117.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox117.Size = New System.Drawing.Size(145, 17)
        Me.CheckBox117.TabIndex = 129
        Me.CheckBox117.Text = "Trade NB on-line"
        Me.CheckBox117.UseVisualStyleBackColor = False
        '
        'Label72
        '
        Me.Label72.BackColor = System.Drawing.SystemColors.Control
        Me.Label72.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label72.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label72.Location = New System.Drawing.Point(32, 52)
        Me.Label72.Name = "Label72"
        Me.Label72.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label72.Size = New System.Drawing.Size(153, 25)
        Me.Label72.TabIndex = 132
        Me.Label72.Text = "On-line commenced On:"
        '
        'GroupBox30
        '
        Me.GroupBox30.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox30.Controls.Add(Me.CheckBox118)
        Me.GroupBox30.Controls.Add(Me.CheckBox119)
        Me.GroupBox30.Controls.Add(Me.CheckBox120)
        Me.GroupBox30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox30.Location = New System.Drawing.Point(398, 12)
        Me.GroupBox30.Name = "GroupBox30"
        Me.GroupBox30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox30.Size = New System.Drawing.Size(434, 57)
        Me.GroupBox30.TabIndex = 124
        Me.GroupBox30.TabStop = False
        Me.GroupBox30.Text = "Produce Documents"
        '
        'CheckBox118
        '
        Me.CheckBox118.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox118.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox118.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox118.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox118.Location = New System.Drawing.Point(290, 19)
        Me.CheckBox118.Name = "CheckBox118"
        Me.CheckBox118.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox118.Size = New System.Drawing.Size(133, 25)
        Me.CheckBox118.TabIndex = 127
        Me.CheckBox118.Text = "Produce Debit Note"
        Me.CheckBox118.UseVisualStyleBackColor = False
        '
        'CheckBox119
        '
        Me.CheckBox119.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox119.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox119.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox119.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox119.Location = New System.Drawing.Point(154, 19)
        Me.CheckBox119.Name = "CheckBox119"
        Me.CheckBox119.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox119.Size = New System.Drawing.Size(161, 25)
        Me.CheckBox119.TabIndex = 126
        Me.CheckBox119.Text = "Produce Certificate"
        Me.CheckBox119.UseVisualStyleBackColor = False
        '
        'CheckBox120
        '
        Me.CheckBox120.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox120.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox120.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox120.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox120.Location = New System.Drawing.Point(19, 19)
        Me.CheckBox120.Name = "CheckBox120"
        Me.CheckBox120.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox120.Size = New System.Drawing.Size(161, 25)
        Me.CheckBox120.TabIndex = 125
        Me.CheckBox120.Text = "Produce Schedule"
        Me.CheckBox120.UseVisualStyleBackColor = False
        '
        'GroupBox31
        '
        Me.GroupBox31.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox31.Controls.Add(Me.CheckBox121)
        Me.GroupBox31.Controls.Add(Me.CheckBox122)
        Me.GroupBox31.Controls.Add(Me.CheckBox123)
        Me.GroupBox31.Controls.Add(Me.CheckBox124)
        Me.GroupBox31.Controls.Add(Me.CheckBox125)
        Me.GroupBox31.Controls.Add(Me.Label73)
        Me.GroupBox31.Controls.Add(Me.Label74)
        Me.GroupBox31.Controls.Add(Me.Label75)
        Me.GroupBox31.Controls.Add(Me.Label76)
        Me.GroupBox31.Controls.Add(Me.Label77)
        Me.GroupBox31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox31.Location = New System.Drawing.Point(8, 12)
        Me.GroupBox31.Name = "GroupBox31"
        Me.GroupBox31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox31.Size = New System.Drawing.Size(385, 57)
        Me.GroupBox31.TabIndex = 117
        Me.GroupBox31.TabStop = False
        Me.GroupBox31.Text = "Options at Make Live"
        '
        'CheckBox121
        '
        Me.CheckBox121.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox121.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox121.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox121.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox121.Location = New System.Drawing.Point(356, 17)
        Me.CheckBox121.Name = "CheckBox121"
        Me.CheckBox121.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox121.Size = New System.Drawing.Size(24, 13)
        Me.CheckBox121.TabIndex = 396
        Me.CheckBox121.UseVisualStyleBackColor = False
        '
        'CheckBox122
        '
        Me.CheckBox122.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox122.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox122.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox122.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox122.Location = New System.Drawing.Point(324, 35)
        Me.CheckBox122.Name = "CheckBox122"
        Me.CheckBox122.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox122.Size = New System.Drawing.Size(29, 13)
        Me.CheckBox122.TabIndex = 383
        Me.CheckBox122.UseVisualStyleBackColor = False
        '
        'CheckBox123
        '
        Me.CheckBox123.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox123.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox123.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox123.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox123.Location = New System.Drawing.Point(130, 17)
        Me.CheckBox123.Name = "CheckBox123"
        Me.CheckBox123.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox123.Size = New System.Drawing.Size(25, 13)
        Me.CheckBox123.TabIndex = 119
        Me.CheckBox123.UseVisualStyleBackColor = False
        '
        'CheckBox124
        '
        Me.CheckBox124.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox124.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox124.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox124.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox124.Location = New System.Drawing.Point(130, 35)
        Me.CheckBox124.Name = "CheckBox124"
        Me.CheckBox124.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox124.Size = New System.Drawing.Size(21, 13)
        Me.CheckBox124.TabIndex = 121
        Me.CheckBox124.UseVisualStyleBackColor = False
        '
        'CheckBox125
        '
        Me.CheckBox125.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox125.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox125.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox125.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox125.Location = New System.Drawing.Point(232, 17)
        Me.CheckBox125.Name = "CheckBox125"
        Me.CheckBox125.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox125.Size = New System.Drawing.Size(29, 13)
        Me.CheckBox125.TabIndex = 123
        Me.CheckBox125.UseVisualStyleBackColor = False
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.BackColor = System.Drawing.SystemColors.Control
        Me.Label73.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label73.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label73.Location = New System.Drawing.Point(263, 17)
        Me.Label73.Name = "Label73"
        Me.Label73.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label73.Size = New System.Drawing.Size(70, 13)
        Me.Label73.TabIndex = 397
        Me.Label73.Text = "Cash Deposit"
        '
        'Label74
        '
        Me.Label74.AutoSize = True
        Me.Label74.BackColor = System.Drawing.SystemColors.Control
        Me.Label74.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label74.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label74.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label74.Location = New System.Drawing.Point(220, 34)
        Me.Label74.Name = "Label74"
        Me.Label74.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label74.Size = New System.Drawing.Size(85, 13)
        Me.Label74.TabIndex = 384
        Me.Label74.Text = "Bank Guarantee"
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.BackColor = System.Drawing.SystemColors.Control
        Me.Label75.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label75.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label75.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label75.Location = New System.Drawing.Point(52, 17)
        Me.Label75.Name = "Label75"
        Me.Label75.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label75.Size = New System.Drawing.Size(42, 13)
        Me.Label75.TabIndex = 118
        Me.Label75.Text = "Invoice"
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.BackColor = System.Drawing.SystemColors.Control
        Me.Label76.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label76.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label76.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label76.Location = New System.Drawing.Point(52, 35)
        Me.Label76.Name = "Label76"
        Me.Label76.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label76.Size = New System.Drawing.Size(60, 13)
        Me.Label76.TabIndex = 120
        Me.Label76.Text = "Instalments"
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.BackColor = System.Drawing.SystemColors.Control
        Me.Label77.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label77.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label77.Location = New System.Drawing.Point(168, 17)
        Me.Label77.Name = "Label77"
        Me.Label77.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label77.Size = New System.Drawing.Size(50, 13)
        Me.Label77.TabIndex = 122
        Me.Label77.Text = "Pay Now"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.GroupBox32)
        Me.TabPage6.Controls.Add(Me.GroupBox33)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(836, 642)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "6-Event"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBox32
        '
        Me.GroupBox32.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox32.Controls.Add(Me.PickList2)
        Me.GroupBox32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox32.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox32.Location = New System.Drawing.Point(10, 263)
        Me.GroupBox32.Name = "GroupBox32"
        Me.GroupBox32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox32.Size = New System.Drawing.Size(815, 250)
        Me.GroupBox32.TabIndex = 177
        Me.GroupBox32.TabStop = False
        Me.GroupBox32.Text = "Claim Event Descriptions"
        '
        'PickList2
        '
        Me.PickList2.AvailableCaption = ""
        Me.PickList2.BusinessObject = "bSIRProduct.Business"
        Me.PickList2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList2.ForeignKeys = CType(resources.GetObject("PickList2.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList2.IsSearchable = False
        Me.PickList2.Location = New System.Drawing.Point(8, 16)
        Me.PickList2.Name = "PickList2"
        Me.PickList2.PickListType = "Claim"
        Me.PickList2.Size = New System.Drawing.Size(793, 226)
        Me.PickList2.TabIndex = 178
        '
        'GroupBox33
        '
        Me.GroupBox33.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox33.Controls.Add(Me.PickList3)
        Me.GroupBox33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox33.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox33.Location = New System.Drawing.Point(8, 4)
        Me.GroupBox33.Name = "GroupBox33"
        Me.GroupBox33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox33.Size = New System.Drawing.Size(819, 251)
        Me.GroupBox33.TabIndex = 175
        Me.GroupBox33.TabStop = False
        Me.GroupBox33.Text = "MTA Event Descriptions"
        '
        'PickList3
        '
        Me.PickList3.AvailableCaption = ""
        Me.PickList3.BusinessObject = "bSIRProduct.Business"
        Me.PickList3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList3.ForeignKeys = CType(resources.GetObject("PickList3.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList3.IsSearchable = False
        Me.PickList3.Location = New System.Drawing.Point(8, 16)
        Me.PickList3.Name = "PickList3"
        Me.PickList3.PickListType = "MTA"
        Me.PickList3.Size = New System.Drawing.Size(800, 219)
        Me.PickList3.TabIndex = 176
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.GroupBox34)
        Me.TabPage7.Controls.Add(Me.GroupBox35)
        Me.TabPage7.Controls.Add(Me.GroupBox36)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(836, 642)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "7-Renewals On-Line"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'GroupBox34
        '
        Me.GroupBox34.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox34.Controls.Add(Me.ComboBox21)
        Me.GroupBox34.Controls.Add(Me.ComboBox22)
        Me.GroupBox34.Controls.Add(Me.ComboBox23)
        Me.GroupBox34.Controls.Add(Me.CheckBox126)
        Me.GroupBox34.Controls.Add(Me.CheckBox127)
        Me.GroupBox34.Controls.Add(Me.Button4)
        Me.GroupBox34.Controls.Add(Me.Button5)
        Me.GroupBox34.Controls.Add(Me.TextBox26)
        Me.GroupBox34.Controls.Add(Me.Button6)
        Me.GroupBox34.Controls.Add(Me.Button7)
        Me.GroupBox34.Controls.Add(Me.TextBox27)
        Me.GroupBox34.Controls.Add(Me.Button8)
        Me.GroupBox34.Controls.Add(Me.Button9)
        Me.GroupBox34.Controls.Add(Me.TextBox28)
        Me.GroupBox34.Controls.Add(Me.CheckBox128)
        Me.GroupBox34.Controls.Add(Me.Label78)
        Me.GroupBox34.Controls.Add(Me.Label79)
        Me.GroupBox34.Controls.Add(Me.Label80)
        Me.GroupBox34.Controls.Add(Me.Label81)
        Me.GroupBox34.Controls.Add(Me.Label82)
        Me.GroupBox34.Controls.Add(Me.Label83)
        Me.GroupBox34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox34.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox34.Location = New System.Drawing.Point(8, 376)
        Me.GroupBox34.Name = "GroupBox34"
        Me.GroupBox34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox34.Size = New System.Drawing.Size(701, 95)
        Me.GroupBox34.TabIndex = 268
        Me.GroupBox34.TabStop = False
        Me.GroupBox34.Text = "Agent - Email Communication"
        '
        'CheckBox126
        '
        Me.CheckBox126.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox126.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox126.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox126.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox126.Location = New System.Drawing.Point(6, 40)
        Me.CheckBox126.Name = "CheckBox126"
        Me.CheckBox126.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox126.Size = New System.Drawing.Size(118, 22)
        Me.CheckBox126.TabIndex = 286
        Me.CheckBox126.Text = "Renewal Invite"
        Me.CheckBox126.UseVisualStyleBackColor = False
        '
        'CheckBox127
        '
        Me.CheckBox127.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox127.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox127.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox127.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox127.Location = New System.Drawing.Point(6, 64)
        Me.CheckBox127.Name = "CheckBox127"
        Me.CheckBox127.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox127.Size = New System.Drawing.Size(118, 25)
        Me.CheckBox127.TabIndex = 285
        Me.CheckBox127.Text = "Renewal Update"
        Me.CheckBox127.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.SystemColors.Control
        Me.Button4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button4.Location = New System.Drawing.Point(415, 69)
        Me.Button4.Name = "Button4"
        Me.Button4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button4.Size = New System.Drawing.Size(23, 18)
        Me.Button4.TabIndex = 278
        Me.Button4.Text = "X"
        Me.Button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button4.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.Control
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button5.Location = New System.Drawing.Point(390, 69)
        Me.Button5.Name = "Button5"
        Me.Button5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button5.Size = New System.Drawing.Size(23, 18)
        Me.Button5.TabIndex = 277
        Me.Button5.Text = "..."
        Me.Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button5.UseVisualStyleBackColor = False
        '
        'TextBox26
        '
        Me.TextBox26.AcceptsReturn = True
        Me.TextBox26.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox26.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox26.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox26.Location = New System.Drawing.Point(261, 69)
        Me.TextBox26.MaxLength = 0
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.ReadOnly = True
        Me.TextBox26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox26.Size = New System.Drawing.Size(128, 20)
        Me.TextBox26.TabIndex = 276
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.Control
        Me.Button6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button6.Location = New System.Drawing.Point(415, 44)
        Me.Button6.Name = "Button6"
        Me.Button6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button6.Size = New System.Drawing.Size(23, 18)
        Me.Button6.TabIndex = 275
        Me.Button6.Text = "X"
        Me.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button6.UseVisualStyleBackColor = False
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.SystemColors.Control
        Me.Button7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button7.Location = New System.Drawing.Point(390, 44)
        Me.Button7.Name = "Button7"
        Me.Button7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button7.Size = New System.Drawing.Size(23, 18)
        Me.Button7.TabIndex = 274
        Me.Button7.Text = "..."
        Me.Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button7.UseVisualStyleBackColor = False
        '
        'TextBox27
        '
        Me.TextBox27.AcceptsReturn = True
        Me.TextBox27.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox27.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox27.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox27.Location = New System.Drawing.Point(261, 44)
        Me.TextBox27.MaxLength = 0
        Me.TextBox27.Name = "TextBox27"
        Me.TextBox27.ReadOnly = True
        Me.TextBox27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox27.Size = New System.Drawing.Size(128, 20)
        Me.TextBox27.TabIndex = 273
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.SystemColors.Control
        Me.Button8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button8.Location = New System.Drawing.Point(415, 20)
        Me.Button8.Name = "Button8"
        Me.Button8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button8.Size = New System.Drawing.Size(23, 18)
        Me.Button8.TabIndex = 272
        Me.Button8.Text = "X"
        Me.Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button8.UseVisualStyleBackColor = False
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.SystemColors.Control
        Me.Button9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button9.Location = New System.Drawing.Point(390, 20)
        Me.Button9.Name = "Button9"
        Me.Button9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button9.Size = New System.Drawing.Size(23, 18)
        Me.Button9.TabIndex = 271
        Me.Button9.Text = "..."
        Me.Button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button9.UseVisualStyleBackColor = False
        '
        'TextBox28
        '
        Me.TextBox28.AcceptsReturn = True
        Me.TextBox28.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox28.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox28.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox28.Location = New System.Drawing.Point(261, 20)
        Me.TextBox28.MaxLength = 0
        Me.TextBox28.Name = "TextBox28"
        Me.TextBox28.ReadOnly = True
        Me.TextBox28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox28.Size = New System.Drawing.Size(128, 20)
        Me.TextBox28.TabIndex = 270
        '
        'CheckBox128
        '
        Me.CheckBox128.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox128.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox128.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox128.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox128.Location = New System.Drawing.Point(6, 15)
        Me.CheckBox128.Name = "CheckBox128"
        Me.CheckBox128.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox128.Size = New System.Drawing.Size(125, 25)
        Me.CheckBox128.TabIndex = 269
        Me.CheckBox128.Text = "Renewal Selection"
        Me.CheckBox128.UseVisualStyleBackColor = False
        '
        'Label78
        '
        Me.Label78.BackColor = System.Drawing.SystemColors.Control
        Me.Label78.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label78.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label78.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label78.Location = New System.Drawing.Point(443, 70)
        Me.Label78.Name = "Label78"
        Me.Label78.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label78.Size = New System.Drawing.Size(70, 21)
        Me.Label78.TabIndex = 284
        Me.Label78.Text = "Attachment"
        '
        'Label79
        '
        Me.Label79.BackColor = System.Drawing.SystemColors.Control
        Me.Label79.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label79.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label79.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label79.Location = New System.Drawing.Point(136, 70)
        Me.Label79.Name = "Label79"
        Me.Label79.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label79.Size = New System.Drawing.Size(120, 20)
        Me.Label79.TabIndex = 283
        Me.Label79.Text = "Awaiting Update"
        '
        'Label80
        '
        Me.Label80.BackColor = System.Drawing.SystemColors.Control
        Me.Label80.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label80.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label80.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label80.Location = New System.Drawing.Point(443, 45)
        Me.Label80.Name = "Label80"
        Me.Label80.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label80.Size = New System.Drawing.Size(70, 21)
        Me.Label80.TabIndex = 282
        Me.Label80.Text = "Attachment"
        '
        'Label81
        '
        Me.Label81.BackColor = System.Drawing.SystemColors.Control
        Me.Label81.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label81.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label81.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label81.Location = New System.Drawing.Point(136, 45)
        Me.Label81.Name = "Label81"
        Me.Label81.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label81.Size = New System.Drawing.Size(120, 20)
        Me.Label81.TabIndex = 281
        Me.Label81.Text = "Awaiting Rnl Invite"
        '
        'Label82
        '
        Me.Label82.BackColor = System.Drawing.SystemColors.Control
        Me.Label82.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label82.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label82.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label82.Location = New System.Drawing.Point(443, 21)
        Me.Label82.Name = "Label82"
        Me.Label82.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label82.Size = New System.Drawing.Size(70, 21)
        Me.Label82.TabIndex = 280
        Me.Label82.Text = "Attachment"
        '
        'Label83
        '
        Me.Label83.BackColor = System.Drawing.SystemColors.Control
        Me.Label83.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label83.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label83.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label83.Location = New System.Drawing.Point(136, 21)
        Me.Label83.Name = "Label83"
        Me.Label83.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label83.Size = New System.Drawing.Size(120, 20)
        Me.Label83.TabIndex = 279
        Me.Label83.Text = "Awaiting Man Review"
        '
        'GroupBox35
        '
        Me.GroupBox35.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox35.Controls.Add(Me.RadioButton3)
        Me.GroupBox35.Controls.Add(Me.RadioButton4)
        Me.GroupBox35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox35.Location = New System.Drawing.Point(8, 7)
        Me.GroupBox35.Name = "GroupBox35"
        Me.GroupBox35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox35.Size = New System.Drawing.Size(701, 43)
        Me.GroupBox35.TabIndex = 187
        Me.GroupBox35.TabStop = False
        Me.GroupBox35.Text = "True Monthly Policies - Frequency of Communication"
        '
        'RadioButton3
        '
        Me.RadioButton3.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton3.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton3.Location = New System.Drawing.Point(9, 14)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton3.Size = New System.Drawing.Size(252, 26)
        Me.RadioButton3.TabIndex = 189
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "As Renewal Process run"
        Me.RadioButton3.UseVisualStyleBackColor = False
        '
        'RadioButton4
        '
        Me.RadioButton4.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton4.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton4.Location = New System.Drawing.Point(287, 14)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton4.Size = New System.Drawing.Size(231, 26)
        Me.RadioButton4.TabIndex = 188
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "Anniversary Date"
        Me.RadioButton4.UseVisualStyleBackColor = False
        '
        'GroupBox36
        '
        Me.GroupBox36.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox36.Controls.Add(Me.GroupBox37)
        Me.GroupBox36.Controls.Add(Me.GroupBox38)
        Me.GroupBox36.Controls.Add(Me.GroupBox39)
        Me.GroupBox36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox36.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox36.Location = New System.Drawing.Point(8, 53)
        Me.GroupBox36.Name = "GroupBox36"
        Me.GroupBox36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox36.Size = New System.Drawing.Size(701, 318)
        Me.GroupBox36.TabIndex = 186
        Me.GroupBox36.TabStop = False
        Me.GroupBox36.Text = "Direct Consumer - Email Communication"
        '
        'GroupBox37
        '
        Me.GroupBox37.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox37.Controls.Add(Me.CheckBox129)
        Me.GroupBox37.Controls.Add(Me.TextBox29)
        Me.GroupBox37.Controls.Add(Me.Button10)
        Me.GroupBox37.Controls.Add(Me.Button11)
        Me.GroupBox37.Controls.Add(Me.TextBox30)
        Me.GroupBox37.Controls.Add(Me.Button12)
        Me.GroupBox37.Controls.Add(Me.Button13)
        Me.GroupBox37.Controls.Add(Me.TextBox31)
        Me.GroupBox37.Controls.Add(Me.Button14)
        Me.GroupBox37.Controls.Add(Me.Button15)
        Me.GroupBox37.Controls.Add(Me.TextBox32)
        Me.GroupBox37.Controls.Add(Me.Button16)
        Me.GroupBox37.Controls.Add(Me.Button17)
        Me.GroupBox37.Controls.Add(Me.TextBox33)
        Me.GroupBox37.Controls.Add(Me.Button18)
        Me.GroupBox37.Controls.Add(Me.Button19)
        Me.GroupBox37.Controls.Add(Me.TextBox34)
        Me.GroupBox37.Controls.Add(Me.Button20)
        Me.GroupBox37.Controls.Add(Me.Button21)
        Me.GroupBox37.Controls.Add(Me.Label84)
        Me.GroupBox37.Controls.Add(Me.Label85)
        Me.GroupBox37.Controls.Add(Me.Label86)
        Me.GroupBox37.Controls.Add(Me.Label87)
        Me.GroupBox37.Controls.Add(Me.Label88)
        Me.GroupBox37.Controls.Add(Me.Label89)
        Me.GroupBox37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox37.Location = New System.Drawing.Point(8, 216)
        Me.GroupBox37.Name = "GroupBox37"
        Me.GroupBox37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox37.Size = New System.Drawing.Size(688, 95)
        Me.GroupBox37.TabIndex = 242
        Me.GroupBox37.TabStop = False
        Me.GroupBox37.Text = "Renewal Update"
        '
        'CheckBox129
        '
        Me.CheckBox129.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox129.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox129.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox129.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox129.Location = New System.Drawing.Point(8, 14)
        Me.CheckBox129.Name = "CheckBox129"
        Me.CheckBox129.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox129.Size = New System.Drawing.Size(78, 25)
        Me.CheckBox129.TabIndex = 261
        Me.CheckBox129.Text = "Enabled"
        Me.CheckBox129.UseVisualStyleBackColor = False
        '
        'TextBox29
        '
        Me.TextBox29.AcceptsReturn = True
        Me.TextBox29.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox29.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox29.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox29.Location = New System.Drawing.Point(253, 20)
        Me.TextBox29.MaxLength = 0
        Me.TextBox29.Name = "TextBox29"
        Me.TextBox29.ReadOnly = True
        Me.TextBox29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox29.Size = New System.Drawing.Size(128, 20)
        Me.TextBox29.TabIndex = 260
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.SystemColors.Control
        Me.Button10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button10.Location = New System.Drawing.Point(382, 20)
        Me.Button10.Name = "Button10"
        Me.Button10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button10.Size = New System.Drawing.Size(23, 18)
        Me.Button10.TabIndex = 259
        Me.Button10.Text = "..."
        Me.Button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button10.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.SystemColors.Control
        Me.Button11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button11.Location = New System.Drawing.Point(407, 20)
        Me.Button11.Name = "Button11"
        Me.Button11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button11.Size = New System.Drawing.Size(23, 18)
        Me.Button11.TabIndex = 258
        Me.Button11.Text = "X"
        Me.Button11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button11.UseVisualStyleBackColor = False
        '
        'TextBox30
        '
        Me.TextBox30.AcceptsReturn = True
        Me.TextBox30.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox30.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox30.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox30.Location = New System.Drawing.Point(504, 20)
        Me.TextBox30.MaxLength = 0
        Me.TextBox30.Name = "TextBox30"
        Me.TextBox30.ReadOnly = True
        Me.TextBox30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox30.Size = New System.Drawing.Size(128, 20)
        Me.TextBox30.TabIndex = 257
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.SystemColors.Control
        Me.Button12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button12.Location = New System.Drawing.Point(632, 20)
        Me.Button12.Name = "Button12"
        Me.Button12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button12.Size = New System.Drawing.Size(23, 18)
        Me.Button12.TabIndex = 256
        Me.Button12.Text = "..."
        Me.Button12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button13
        '
        Me.Button13.BackColor = System.Drawing.SystemColors.Control
        Me.Button13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button13.Location = New System.Drawing.Point(657, 20)
        Me.Button13.Name = "Button13"
        Me.Button13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button13.Size = New System.Drawing.Size(23, 18)
        Me.Button13.TabIndex = 255
        Me.Button13.Text = "X"
        Me.Button13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button13.UseVisualStyleBackColor = False
        '
        'TextBox31
        '
        Me.TextBox31.AcceptsReturn = True
        Me.TextBox31.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox31.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox31.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox31.Location = New System.Drawing.Point(253, 44)
        Me.TextBox31.MaxLength = 0
        Me.TextBox31.Name = "TextBox31"
        Me.TextBox31.ReadOnly = True
        Me.TextBox31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox31.Size = New System.Drawing.Size(128, 20)
        Me.TextBox31.TabIndex = 254
        '
        'Button14
        '
        Me.Button14.BackColor = System.Drawing.SystemColors.Control
        Me.Button14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button14.Location = New System.Drawing.Point(382, 44)
        Me.Button14.Name = "Button14"
        Me.Button14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button14.Size = New System.Drawing.Size(23, 18)
        Me.Button14.TabIndex = 253
        Me.Button14.Text = "..."
        Me.Button14.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button14.UseVisualStyleBackColor = False
        '
        'Button15
        '
        Me.Button15.BackColor = System.Drawing.SystemColors.Control
        Me.Button15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button15.Location = New System.Drawing.Point(407, 44)
        Me.Button15.Name = "Button15"
        Me.Button15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button15.Size = New System.Drawing.Size(23, 18)
        Me.Button15.TabIndex = 252
        Me.Button15.Text = "X"
        Me.Button15.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button15.UseVisualStyleBackColor = False
        '
        'TextBox32
        '
        Me.TextBox32.AcceptsReturn = True
        Me.TextBox32.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox32.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox32.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox32.Location = New System.Drawing.Point(504, 44)
        Me.TextBox32.MaxLength = 0
        Me.TextBox32.Name = "TextBox32"
        Me.TextBox32.ReadOnly = True
        Me.TextBox32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox32.Size = New System.Drawing.Size(128, 20)
        Me.TextBox32.TabIndex = 251
        '
        'Button16
        '
        Me.Button16.BackColor = System.Drawing.SystemColors.Control
        Me.Button16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button16.Location = New System.Drawing.Point(632, 44)
        Me.Button16.Name = "Button16"
        Me.Button16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button16.Size = New System.Drawing.Size(23, 18)
        Me.Button16.TabIndex = 250
        Me.Button16.Text = "..."
        Me.Button16.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button16.UseVisualStyleBackColor = False
        '
        'Button17
        '
        Me.Button17.BackColor = System.Drawing.SystemColors.Control
        Me.Button17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button17.Location = New System.Drawing.Point(658, 44)
        Me.Button17.Name = "Button17"
        Me.Button17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button17.Size = New System.Drawing.Size(23, 18)
        Me.Button17.TabIndex = 249
        Me.Button17.Text = "X"
        Me.Button17.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button17.UseVisualStyleBackColor = False
        '
        'TextBox33
        '
        Me.TextBox33.AcceptsReturn = True
        Me.TextBox33.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox33.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox33.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox33.Location = New System.Drawing.Point(253, 69)
        Me.TextBox33.MaxLength = 0
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.ReadOnly = True
        Me.TextBox33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox33.Size = New System.Drawing.Size(128, 20)
        Me.TextBox33.TabIndex = 248
        '
        'Button18
        '
        Me.Button18.BackColor = System.Drawing.SystemColors.Control
        Me.Button18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button18.Location = New System.Drawing.Point(382, 69)
        Me.Button18.Name = "Button18"
        Me.Button18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button18.Size = New System.Drawing.Size(23, 18)
        Me.Button18.TabIndex = 247
        Me.Button18.Text = "..."
        Me.Button18.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button18.UseVisualStyleBackColor = False
        '
        'Button19
        '
        Me.Button19.BackColor = System.Drawing.SystemColors.Control
        Me.Button19.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button19.Location = New System.Drawing.Point(407, 69)
        Me.Button19.Name = "Button19"
        Me.Button19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button19.Size = New System.Drawing.Size(23, 18)
        Me.Button19.TabIndex = 246
        Me.Button19.Text = "X"
        Me.Button19.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button19.UseVisualStyleBackColor = False
        '
        'TextBox34
        '
        Me.TextBox34.AcceptsReturn = True
        Me.TextBox34.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox34.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox34.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox34.Location = New System.Drawing.Point(504, 69)
        Me.TextBox34.MaxLength = 0
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.ReadOnly = True
        Me.TextBox34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox34.Size = New System.Drawing.Size(128, 20)
        Me.TextBox34.TabIndex = 245
        '
        'Button20
        '
        Me.Button20.BackColor = System.Drawing.SystemColors.Control
        Me.Button20.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button20.Location = New System.Drawing.Point(632, 69)
        Me.Button20.Name = "Button20"
        Me.Button20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button20.Size = New System.Drawing.Size(23, 18)
        Me.Button20.TabIndex = 244
        Me.Button20.Text = "..."
        Me.Button20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button20.UseVisualStyleBackColor = False
        '
        'Button21
        '
        Me.Button21.BackColor = System.Drawing.SystemColors.Control
        Me.Button21.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button21.Location = New System.Drawing.Point(657, 69)
        Me.Button21.Name = "Button21"
        Me.Button21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button21.Size = New System.Drawing.Size(23, 18)
        Me.Button21.TabIndex = 243
        Me.Button21.Text = "X"
        Me.Button21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button21.UseVisualStyleBackColor = False
        '
        'Label84
        '
        Me.Label84.BackColor = System.Drawing.SystemColors.Control
        Me.Label84.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label84.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label84.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label84.Location = New System.Drawing.Point(128, 20)
        Me.Label84.Name = "Label84"
        Me.Label84.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label84.Size = New System.Drawing.Size(120, 20)
        Me.Label84.TabIndex = 267
        Me.Label84.Text = "Awaiting Man Review"
        '
        'Label85
        '
        Me.Label85.BackColor = System.Drawing.SystemColors.Control
        Me.Label85.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label85.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label85.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label85.Location = New System.Drawing.Point(435, 21)
        Me.Label85.Name = "Label85"
        Me.Label85.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label85.Size = New System.Drawing.Size(70, 21)
        Me.Label85.TabIndex = 266
        Me.Label85.Text = "Attachment"
        '
        'Label86
        '
        Me.Label86.BackColor = System.Drawing.SystemColors.Control
        Me.Label86.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label86.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label86.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label86.Location = New System.Drawing.Point(128, 45)
        Me.Label86.Name = "Label86"
        Me.Label86.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label86.Size = New System.Drawing.Size(120, 20)
        Me.Label86.TabIndex = 265
        Me.Label86.Text = "Awaiting Rnl Invite"
        '
        'Label87
        '
        Me.Label87.BackColor = System.Drawing.SystemColors.Control
        Me.Label87.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label87.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label87.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label87.Location = New System.Drawing.Point(435, 45)
        Me.Label87.Name = "Label87"
        Me.Label87.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label87.Size = New System.Drawing.Size(70, 21)
        Me.Label87.TabIndex = 264
        Me.Label87.Text = "Attachment"
        '
        'Label88
        '
        Me.Label88.BackColor = System.Drawing.SystemColors.Control
        Me.Label88.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label88.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label88.Location = New System.Drawing.Point(128, 70)
        Me.Label88.Name = "Label88"
        Me.Label88.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label88.Size = New System.Drawing.Size(120, 20)
        Me.Label88.TabIndex = 263
        Me.Label88.Text = "Awaiting Update"
        '
        'Label89
        '
        Me.Label89.BackColor = System.Drawing.SystemColors.Control
        Me.Label89.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label89.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label89.Location = New System.Drawing.Point(435, 70)
        Me.Label89.Name = "Label89"
        Me.Label89.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label89.Size = New System.Drawing.Size(70, 21)
        Me.Label89.TabIndex = 262
        Me.Label89.Text = "Attachment"
        '
        'GroupBox38
        '
        Me.GroupBox38.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox38.Controls.Add(Me.CheckBox130)
        Me.GroupBox38.Controls.Add(Me.TextBox35)
        Me.GroupBox38.Controls.Add(Me.Button22)
        Me.GroupBox38.Controls.Add(Me.Button23)
        Me.GroupBox38.Controls.Add(Me.TextBox36)
        Me.GroupBox38.Controls.Add(Me.Button24)
        Me.GroupBox38.Controls.Add(Me.Button25)
        Me.GroupBox38.Controls.Add(Me.TextBox37)
        Me.GroupBox38.Controls.Add(Me.Button26)
        Me.GroupBox38.Controls.Add(Me.Button27)
        Me.GroupBox38.Controls.Add(Me.TextBox38)
        Me.GroupBox38.Controls.Add(Me.Button28)
        Me.GroupBox38.Controls.Add(Me.Button29)
        Me.GroupBox38.Controls.Add(Me.TextBox39)
        Me.GroupBox38.Controls.Add(Me.Button30)
        Me.GroupBox38.Controls.Add(Me.Button31)
        Me.GroupBox38.Controls.Add(Me.TextBox40)
        Me.GroupBox38.Controls.Add(Me.Button32)
        Me.GroupBox38.Controls.Add(Me.Button33)
        Me.GroupBox38.Controls.Add(Me.Label90)
        Me.GroupBox38.Controls.Add(Me.Label91)
        Me.GroupBox38.Controls.Add(Me.Label92)
        Me.GroupBox38.Controls.Add(Me.Label93)
        Me.GroupBox38.Controls.Add(Me.Label94)
        Me.GroupBox38.Controls.Add(Me.Label95)
        Me.GroupBox38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox38.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox38.Location = New System.Drawing.Point(8, 117)
        Me.GroupBox38.Name = "GroupBox38"
        Me.GroupBox38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox38.Size = New System.Drawing.Size(688, 95)
        Me.GroupBox38.TabIndex = 216
        Me.GroupBox38.TabStop = False
        Me.GroupBox38.Text = "Renewal Invite"
        '
        'CheckBox130
        '
        Me.CheckBox130.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox130.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox130.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox130.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox130.Location = New System.Drawing.Point(8, 14)
        Me.CheckBox130.Name = "CheckBox130"
        Me.CheckBox130.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox130.Size = New System.Drawing.Size(78, 25)
        Me.CheckBox130.TabIndex = 235
        Me.CheckBox130.Text = "Enabled"
        Me.CheckBox130.UseVisualStyleBackColor = False
        '
        'TextBox35
        '
        Me.TextBox35.AcceptsReturn = True
        Me.TextBox35.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox35.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox35.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox35.Location = New System.Drawing.Point(253, 20)
        Me.TextBox35.MaxLength = 0
        Me.TextBox35.Name = "TextBox35"
        Me.TextBox35.ReadOnly = True
        Me.TextBox35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox35.Size = New System.Drawing.Size(128, 20)
        Me.TextBox35.TabIndex = 234
        '
        'Button22
        '
        Me.Button22.BackColor = System.Drawing.SystemColors.Control
        Me.Button22.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button22.Location = New System.Drawing.Point(382, 20)
        Me.Button22.Name = "Button22"
        Me.Button22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button22.Size = New System.Drawing.Size(23, 18)
        Me.Button22.TabIndex = 233
        Me.Button22.Text = "..."
        Me.Button22.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button22.UseVisualStyleBackColor = False
        '
        'Button23
        '
        Me.Button23.BackColor = System.Drawing.SystemColors.Control
        Me.Button23.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button23.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button23.Location = New System.Drawing.Point(407, 20)
        Me.Button23.Name = "Button23"
        Me.Button23.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button23.Size = New System.Drawing.Size(23, 18)
        Me.Button23.TabIndex = 232
        Me.Button23.Text = "X"
        Me.Button23.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button23.UseVisualStyleBackColor = False
        '
        'TextBox36
        '
        Me.TextBox36.AcceptsReturn = True
        Me.TextBox36.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox36.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox36.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox36.Location = New System.Drawing.Point(504, 20)
        Me.TextBox36.MaxLength = 0
        Me.TextBox36.Name = "TextBox36"
        Me.TextBox36.ReadOnly = True
        Me.TextBox36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox36.Size = New System.Drawing.Size(128, 20)
        Me.TextBox36.TabIndex = 231
        '
        'Button24
        '
        Me.Button24.BackColor = System.Drawing.SystemColors.Control
        Me.Button24.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button24.Location = New System.Drawing.Point(632, 20)
        Me.Button24.Name = "Button24"
        Me.Button24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button24.Size = New System.Drawing.Size(23, 18)
        Me.Button24.TabIndex = 230
        Me.Button24.Text = "..."
        Me.Button24.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button24.UseVisualStyleBackColor = False
        '
        'Button25
        '
        Me.Button25.BackColor = System.Drawing.SystemColors.Control
        Me.Button25.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button25.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button25.Location = New System.Drawing.Point(657, 20)
        Me.Button25.Name = "Button25"
        Me.Button25.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button25.Size = New System.Drawing.Size(23, 18)
        Me.Button25.TabIndex = 229
        Me.Button25.Text = "X"
        Me.Button25.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button25.UseVisualStyleBackColor = False
        '
        'TextBox37
        '
        Me.TextBox37.AcceptsReturn = True
        Me.TextBox37.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox37.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox37.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox37.Location = New System.Drawing.Point(254, 44)
        Me.TextBox37.MaxLength = 0
        Me.TextBox37.Name = "TextBox37"
        Me.TextBox37.ReadOnly = True
        Me.TextBox37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox37.Size = New System.Drawing.Size(128, 20)
        Me.TextBox37.TabIndex = 228
        '
        'Button26
        '
        Me.Button26.BackColor = System.Drawing.SystemColors.Control
        Me.Button26.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button26.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button26.Location = New System.Drawing.Point(382, 44)
        Me.Button26.Name = "Button26"
        Me.Button26.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button26.Size = New System.Drawing.Size(23, 18)
        Me.Button26.TabIndex = 227
        Me.Button26.Text = "..."
        Me.Button26.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button26.UseVisualStyleBackColor = False
        '
        'Button27
        '
        Me.Button27.BackColor = System.Drawing.SystemColors.Control
        Me.Button27.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button27.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button27.Location = New System.Drawing.Point(407, 44)
        Me.Button27.Name = "Button27"
        Me.Button27.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button27.Size = New System.Drawing.Size(23, 18)
        Me.Button27.TabIndex = 226
        Me.Button27.Text = "X"
        Me.Button27.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button27.UseVisualStyleBackColor = False
        '
        'TextBox38
        '
        Me.TextBox38.AcceptsReturn = True
        Me.TextBox38.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox38.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox38.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox38.Location = New System.Drawing.Point(504, 44)
        Me.TextBox38.MaxLength = 0
        Me.TextBox38.Name = "TextBox38"
        Me.TextBox38.ReadOnly = True
        Me.TextBox38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox38.Size = New System.Drawing.Size(128, 20)
        Me.TextBox38.TabIndex = 225
        '
        'Button28
        '
        Me.Button28.BackColor = System.Drawing.SystemColors.Control
        Me.Button28.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button28.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button28.Location = New System.Drawing.Point(632, 44)
        Me.Button28.Name = "Button28"
        Me.Button28.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button28.Size = New System.Drawing.Size(23, 18)
        Me.Button28.TabIndex = 224
        Me.Button28.Text = "..."
        Me.Button28.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button28.UseVisualStyleBackColor = False
        '
        'Button29
        '
        Me.Button29.BackColor = System.Drawing.SystemColors.Control
        Me.Button29.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button29.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button29.Location = New System.Drawing.Point(657, 44)
        Me.Button29.Name = "Button29"
        Me.Button29.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button29.Size = New System.Drawing.Size(23, 18)
        Me.Button29.TabIndex = 223
        Me.Button29.Text = "X"
        Me.Button29.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button29.UseVisualStyleBackColor = False
        '
        'TextBox39
        '
        Me.TextBox39.AcceptsReturn = True
        Me.TextBox39.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox39.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox39.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox39.Location = New System.Drawing.Point(254, 69)
        Me.TextBox39.MaxLength = 0
        Me.TextBox39.Name = "TextBox39"
        Me.TextBox39.ReadOnly = True
        Me.TextBox39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox39.Size = New System.Drawing.Size(128, 20)
        Me.TextBox39.TabIndex = 222
        '
        'Button30
        '
        Me.Button30.BackColor = System.Drawing.SystemColors.Control
        Me.Button30.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button30.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button30.Location = New System.Drawing.Point(382, 69)
        Me.Button30.Name = "Button30"
        Me.Button30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button30.Size = New System.Drawing.Size(23, 18)
        Me.Button30.TabIndex = 221
        Me.Button30.Text = "..."
        Me.Button30.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button30.UseVisualStyleBackColor = False
        '
        'Button31
        '
        Me.Button31.BackColor = System.Drawing.SystemColors.Control
        Me.Button31.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button31.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button31.Location = New System.Drawing.Point(407, 69)
        Me.Button31.Name = "Button31"
        Me.Button31.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button31.Size = New System.Drawing.Size(23, 18)
        Me.Button31.TabIndex = 220
        Me.Button31.Text = "X"
        Me.Button31.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button31.UseVisualStyleBackColor = False
        '
        'TextBox40
        '
        Me.TextBox40.AcceptsReturn = True
        Me.TextBox40.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox40.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox40.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox40.Location = New System.Drawing.Point(504, 69)
        Me.TextBox40.MaxLength = 0
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.ReadOnly = True
        Me.TextBox40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox40.Size = New System.Drawing.Size(128, 20)
        Me.TextBox40.TabIndex = 219
        '
        'Button32
        '
        Me.Button32.BackColor = System.Drawing.SystemColors.Control
        Me.Button32.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button32.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button32.Location = New System.Drawing.Point(632, 69)
        Me.Button32.Name = "Button32"
        Me.Button32.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button32.Size = New System.Drawing.Size(23, 18)
        Me.Button32.TabIndex = 218
        Me.Button32.Text = "..."
        Me.Button32.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button32.UseVisualStyleBackColor = False
        '
        'Button33
        '
        Me.Button33.BackColor = System.Drawing.SystemColors.Control
        Me.Button33.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button33.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button33.Location = New System.Drawing.Point(657, 69)
        Me.Button33.Name = "Button33"
        Me.Button33.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button33.Size = New System.Drawing.Size(23, 18)
        Me.Button33.TabIndex = 217
        Me.Button33.Text = "X"
        Me.Button33.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button33.UseVisualStyleBackColor = False
        '
        'Label90
        '
        Me.Label90.BackColor = System.Drawing.SystemColors.Control
        Me.Label90.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label90.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label90.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label90.Location = New System.Drawing.Point(129, 21)
        Me.Label90.Name = "Label90"
        Me.Label90.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label90.Size = New System.Drawing.Size(120, 20)
        Me.Label90.TabIndex = 241
        Me.Label90.Text = "Awaiting Man Review"
        '
        'Label91
        '
        Me.Label91.BackColor = System.Drawing.SystemColors.Control
        Me.Label91.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label91.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label91.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label91.Location = New System.Drawing.Point(435, 21)
        Me.Label91.Name = "Label91"
        Me.Label91.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label91.Size = New System.Drawing.Size(70, 21)
        Me.Label91.TabIndex = 240
        Me.Label91.Text = "Attachment"
        '
        'Label92
        '
        Me.Label92.BackColor = System.Drawing.SystemColors.Control
        Me.Label92.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label92.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label92.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label92.Location = New System.Drawing.Point(129, 45)
        Me.Label92.Name = "Label92"
        Me.Label92.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label92.Size = New System.Drawing.Size(120, 20)
        Me.Label92.TabIndex = 239
        Me.Label92.Text = "Awaiting Rnl Invite"
        '
        'Label93
        '
        Me.Label93.BackColor = System.Drawing.SystemColors.Control
        Me.Label93.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label93.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label93.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label93.Location = New System.Drawing.Point(435, 45)
        Me.Label93.Name = "Label93"
        Me.Label93.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label93.Size = New System.Drawing.Size(70, 21)
        Me.Label93.TabIndex = 238
        Me.Label93.Text = "Attachment"
        '
        'Label94
        '
        Me.Label94.BackColor = System.Drawing.SystemColors.Control
        Me.Label94.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label94.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label94.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label94.Location = New System.Drawing.Point(129, 70)
        Me.Label94.Name = "Label94"
        Me.Label94.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label94.Size = New System.Drawing.Size(120, 20)
        Me.Label94.TabIndex = 237
        Me.Label94.Text = "Awaiting Update"
        '
        'Label95
        '
        Me.Label95.BackColor = System.Drawing.SystemColors.Control
        Me.Label95.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label95.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label95.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label95.Location = New System.Drawing.Point(435, 70)
        Me.Label95.Name = "Label95"
        Me.Label95.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label95.Size = New System.Drawing.Size(70, 21)
        Me.Label95.TabIndex = 236
        Me.Label95.Text = "Attachment"
        '
        'GroupBox39
        '
        Me.GroupBox39.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox39.Controls.Add(Me.Button34)
        Me.GroupBox39.Controls.Add(Me.Button35)
        Me.GroupBox39.Controls.Add(Me.TextBox41)
        Me.GroupBox39.Controls.Add(Me.Button36)
        Me.GroupBox39.Controls.Add(Me.Button37)
        Me.GroupBox39.Controls.Add(Me.TextBox42)
        Me.GroupBox39.Controls.Add(Me.Button38)
        Me.GroupBox39.Controls.Add(Me.Button39)
        Me.GroupBox39.Controls.Add(Me.TextBox43)
        Me.GroupBox39.Controls.Add(Me.Button40)
        Me.GroupBox39.Controls.Add(Me.Button41)
        Me.GroupBox39.Controls.Add(Me.TextBox44)
        Me.GroupBox39.Controls.Add(Me.Button42)
        Me.GroupBox39.Controls.Add(Me.Button43)
        Me.GroupBox39.Controls.Add(Me.TextBox45)
        Me.GroupBox39.Controls.Add(Me.Button44)
        Me.GroupBox39.Controls.Add(Me.Button45)
        Me.GroupBox39.Controls.Add(Me.TextBox46)
        Me.GroupBox39.Controls.Add(Me.CheckBox131)
        Me.GroupBox39.Controls.Add(Me.Label96)
        Me.GroupBox39.Controls.Add(Me.Label97)
        Me.GroupBox39.Controls.Add(Me.Label98)
        Me.GroupBox39.Controls.Add(Me.Label99)
        Me.GroupBox39.Controls.Add(Me.Label100)
        Me.GroupBox39.Controls.Add(Me.Label101)
        Me.GroupBox39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox39.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox39.Location = New System.Drawing.Point(8, 19)
        Me.GroupBox39.Name = "GroupBox39"
        Me.GroupBox39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox39.Size = New System.Drawing.Size(686, 95)
        Me.GroupBox39.TabIndex = 190
        Me.GroupBox39.TabStop = False
        Me.GroupBox39.Text = "Renewal Selection"
        '
        'Button34
        '
        Me.Button34.BackColor = System.Drawing.SystemColors.Control
        Me.Button34.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button34.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button34.Location = New System.Drawing.Point(655, 69)
        Me.Button34.Name = "Button34"
        Me.Button34.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button34.Size = New System.Drawing.Size(23, 18)
        Me.Button34.TabIndex = 215
        Me.Button34.Text = "X"
        Me.Button34.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button34.UseVisualStyleBackColor = False
        '
        'Button35
        '
        Me.Button35.BackColor = System.Drawing.SystemColors.Control
        Me.Button35.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button35.Location = New System.Drawing.Point(630, 69)
        Me.Button35.Name = "Button35"
        Me.Button35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button35.Size = New System.Drawing.Size(23, 18)
        Me.Button35.TabIndex = 214
        Me.Button35.Text = "..."
        Me.Button35.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button35.UseVisualStyleBackColor = False
        '
        'TextBox41
        '
        Me.TextBox41.AcceptsReturn = True
        Me.TextBox41.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox41.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox41.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox41.Location = New System.Drawing.Point(503, 69)
        Me.TextBox41.MaxLength = 0
        Me.TextBox41.Name = "TextBox41"
        Me.TextBox41.ReadOnly = True
        Me.TextBox41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox41.Size = New System.Drawing.Size(128, 20)
        Me.TextBox41.TabIndex = 212
        '
        'Button36
        '
        Me.Button36.BackColor = System.Drawing.SystemColors.Control
        Me.Button36.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button36.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button36.Location = New System.Drawing.Point(406, 69)
        Me.Button36.Name = "Button36"
        Me.Button36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button36.Size = New System.Drawing.Size(23, 18)
        Me.Button36.TabIndex = 211
        Me.Button36.Text = "X"
        Me.Button36.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button36.UseVisualStyleBackColor = False
        '
        'Button37
        '
        Me.Button37.BackColor = System.Drawing.SystemColors.Control
        Me.Button37.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button37.Location = New System.Drawing.Point(380, 69)
        Me.Button37.Name = "Button37"
        Me.Button37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button37.Size = New System.Drawing.Size(23, 18)
        Me.Button37.TabIndex = 210
        Me.Button37.Text = "..."
        Me.Button37.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button37.UseVisualStyleBackColor = False
        '
        'TextBox42
        '
        Me.TextBox42.AcceptsReturn = True
        Me.TextBox42.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox42.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox42.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox42.Location = New System.Drawing.Point(253, 69)
        Me.TextBox42.MaxLength = 0
        Me.TextBox42.Name = "TextBox42"
        Me.TextBox42.ReadOnly = True
        Me.TextBox42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox42.Size = New System.Drawing.Size(128, 20)
        Me.TextBox42.TabIndex = 208
        '
        'Button38
        '
        Me.Button38.BackColor = System.Drawing.SystemColors.Control
        Me.Button38.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button38.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button38.Location = New System.Drawing.Point(655, 44)
        Me.Button38.Name = "Button38"
        Me.Button38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button38.Size = New System.Drawing.Size(23, 18)
        Me.Button38.TabIndex = 207
        Me.Button38.Text = "X"
        Me.Button38.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button38.UseVisualStyleBackColor = False
        '
        'Button39
        '
        Me.Button39.BackColor = System.Drawing.SystemColors.Control
        Me.Button39.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button39.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button39.Location = New System.Drawing.Point(630, 44)
        Me.Button39.Name = "Button39"
        Me.Button39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button39.Size = New System.Drawing.Size(23, 18)
        Me.Button39.TabIndex = 206
        Me.Button39.Text = "..."
        Me.Button39.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button39.UseVisualStyleBackColor = False
        '
        'TextBox43
        '
        Me.TextBox43.AcceptsReturn = True
        Me.TextBox43.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox43.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox43.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox43.Location = New System.Drawing.Point(503, 44)
        Me.TextBox43.MaxLength = 0
        Me.TextBox43.Name = "TextBox43"
        Me.TextBox43.ReadOnly = True
        Me.TextBox43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox43.Size = New System.Drawing.Size(128, 20)
        Me.TextBox43.TabIndex = 204
        '
        'Button40
        '
        Me.Button40.BackColor = System.Drawing.SystemColors.Control
        Me.Button40.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button40.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button40.Location = New System.Drawing.Point(406, 44)
        Me.Button40.Name = "Button40"
        Me.Button40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button40.Size = New System.Drawing.Size(23, 18)
        Me.Button40.TabIndex = 203
        Me.Button40.Text = "X"
        Me.Button40.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button40.UseVisualStyleBackColor = False
        '
        'Button41
        '
        Me.Button41.BackColor = System.Drawing.SystemColors.Control
        Me.Button41.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button41.Location = New System.Drawing.Point(381, 44)
        Me.Button41.Name = "Button41"
        Me.Button41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button41.Size = New System.Drawing.Size(23, 18)
        Me.Button41.TabIndex = 202
        Me.Button41.Text = "..."
        Me.Button41.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button41.UseVisualStyleBackColor = False
        '
        'TextBox44
        '
        Me.TextBox44.AcceptsReturn = True
        Me.TextBox44.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox44.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox44.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox44.Location = New System.Drawing.Point(253, 43)
        Me.TextBox44.MaxLength = 0
        Me.TextBox44.Name = "TextBox44"
        Me.TextBox44.ReadOnly = True
        Me.TextBox44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox44.Size = New System.Drawing.Size(128, 20)
        Me.TextBox44.TabIndex = 200
        '
        'Button42
        '
        Me.Button42.BackColor = System.Drawing.SystemColors.Control
        Me.Button42.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button42.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button42.Location = New System.Drawing.Point(655, 20)
        Me.Button42.Name = "Button42"
        Me.Button42.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button42.Size = New System.Drawing.Size(23, 18)
        Me.Button42.TabIndex = 199
        Me.Button42.Text = "X"
        Me.Button42.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button42.UseVisualStyleBackColor = False
        '
        'Button43
        '
        Me.Button43.BackColor = System.Drawing.SystemColors.Control
        Me.Button43.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button43.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button43.Location = New System.Drawing.Point(631, 20)
        Me.Button43.Name = "Button43"
        Me.Button43.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button43.Size = New System.Drawing.Size(23, 18)
        Me.Button43.TabIndex = 198
        Me.Button43.Text = "..."
        Me.Button43.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button43.UseVisualStyleBackColor = False
        '
        'TextBox45
        '
        Me.TextBox45.AcceptsReturn = True
        Me.TextBox45.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox45.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox45.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox45.Location = New System.Drawing.Point(503, 20)
        Me.TextBox45.MaxLength = 0
        Me.TextBox45.Name = "TextBox45"
        Me.TextBox45.ReadOnly = True
        Me.TextBox45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox45.Size = New System.Drawing.Size(128, 20)
        Me.TextBox45.TabIndex = 196
        '
        'Button44
        '
        Me.Button44.BackColor = System.Drawing.SystemColors.Control
        Me.Button44.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button44.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button44.Location = New System.Drawing.Point(406, 20)
        Me.Button44.Name = "Button44"
        Me.Button44.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button44.Size = New System.Drawing.Size(23, 18)
        Me.Button44.TabIndex = 195
        Me.Button44.Text = "X"
        Me.Button44.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button44.UseVisualStyleBackColor = False
        '
        'Button45
        '
        Me.Button45.BackColor = System.Drawing.SystemColors.Control
        Me.Button45.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button45.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button45.Location = New System.Drawing.Point(381, 20)
        Me.Button45.Name = "Button45"
        Me.Button45.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button45.Size = New System.Drawing.Size(23, 18)
        Me.Button45.TabIndex = 194
        Me.Button45.Text = "..."
        Me.Button45.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button45.UseVisualStyleBackColor = False
        '
        'TextBox46
        '
        Me.TextBox46.AcceptsReturn = True
        Me.TextBox46.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox46.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox46.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox46.Location = New System.Drawing.Point(253, 20)
        Me.TextBox46.MaxLength = 0
        Me.TextBox46.Name = "TextBox46"
        Me.TextBox46.ReadOnly = True
        Me.TextBox46.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox46.Size = New System.Drawing.Size(128, 20)
        Me.TextBox46.TabIndex = 192
        '
        'CheckBox131
        '
        Me.CheckBox131.BackColor = System.Drawing.SystemColors.Control
        Me.CheckBox131.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckBox131.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox131.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckBox131.Location = New System.Drawing.Point(8, 13)
        Me.CheckBox131.Name = "CheckBox131"
        Me.CheckBox131.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox131.Size = New System.Drawing.Size(78, 25)
        Me.CheckBox131.TabIndex = 191
        Me.CheckBox131.Text = "Enabled"
        Me.CheckBox131.UseVisualStyleBackColor = False
        '
        'Label96
        '
        Me.Label96.BackColor = System.Drawing.SystemColors.Control
        Me.Label96.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label96.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label96.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label96.Location = New System.Drawing.Point(434, 70)
        Me.Label96.Name = "Label96"
        Me.Label96.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label96.Size = New System.Drawing.Size(70, 21)
        Me.Label96.TabIndex = 213
        Me.Label96.Text = "Attachment"
        '
        'Label97
        '
        Me.Label97.BackColor = System.Drawing.SystemColors.Control
        Me.Label97.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label97.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label97.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label97.Location = New System.Drawing.Point(128, 70)
        Me.Label97.Name = "Label97"
        Me.Label97.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label97.Size = New System.Drawing.Size(120, 20)
        Me.Label97.TabIndex = 209
        Me.Label97.Text = "Awaiting Update"
        '
        'Label98
        '
        Me.Label98.BackColor = System.Drawing.SystemColors.Control
        Me.Label98.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label98.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label98.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label98.Location = New System.Drawing.Point(434, 45)
        Me.Label98.Name = "Label98"
        Me.Label98.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label98.Size = New System.Drawing.Size(70, 21)
        Me.Label98.TabIndex = 205
        Me.Label98.Text = "Attachment"
        '
        'Label99
        '
        Me.Label99.BackColor = System.Drawing.SystemColors.Control
        Me.Label99.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label99.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label99.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label99.Location = New System.Drawing.Point(128, 45)
        Me.Label99.Name = "Label99"
        Me.Label99.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label99.Size = New System.Drawing.Size(120, 20)
        Me.Label99.TabIndex = 201
        Me.Label99.Text = "Awaiting Rnl Invite"
        '
        'Label100
        '
        Me.Label100.BackColor = System.Drawing.SystemColors.Control
        Me.Label100.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label100.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label100.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label100.Location = New System.Drawing.Point(434, 21)
        Me.Label100.Name = "Label100"
        Me.Label100.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label100.Size = New System.Drawing.Size(70, 21)
        Me.Label100.TabIndex = 197
        Me.Label100.Text = "Attachment"
        '
        'Label101
        '
        Me.Label101.BackColor = System.Drawing.SystemColors.Control
        Me.Label101.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label101.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label101.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label101.Location = New System.Drawing.Point(128, 21)
        Me.Label101.Name = "Label101"
        Me.Label101.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label101.Size = New System.Drawing.Size(120, 20)
        Me.Label101.TabIndex = 193
        Me.Label101.Text = "Awaiting Man Review"
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.UctDocumentLink2)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(836, 642)
        Me.TabPage8.TabIndex = 7
        Me.TabPage8.Text = "8-Document Links"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'UctDocumentLink2
        '
        Me.UctDocumentLink2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UctDocumentLink2.Location = New System.Drawing.Point(8, 14)
        Me.UctDocumentLink2.Name = "UctDocumentLink2"
        Me.UctDocumentLink2.ScreenHierarchy = ""
        Me.UctDocumentLink2.Size = New System.Drawing.Size(824, 499)
        Me.UctDocumentLink2.Status = 0
        Me.UctDocumentLink2.TabIndex = 293
        Me.UctDocumentLink2.Task = 0
        Me.UctDocumentLink2.UniqueId = ""
        Me.UctDocumentLink2.WrittenPolicyStatus = False
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.UctDocumentLink3)
        Me.TabPage9.Controls.Add(Me.UctDocumentLink4)
        Me.TabPage9.Location = New System.Drawing.Point(4, 40)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Size = New System.Drawing.Size(836, 624)
        Me.TabPage9.TabIndex = 8
        Me.TabPage9.Text = "9-Claims Documents"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'UctDocumentLink3
        '
        Me.UctDocumentLink3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UctDocumentLink3.Location = New System.Drawing.Point(10, 14)
        Me.UctDocumentLink3.Name = "UctDocumentLink3"
        Me.UctDocumentLink3.ScreenHierarchy = ""
        Me.UctDocumentLink3.Size = New System.Drawing.Size(817, 243)
        Me.UctDocumentLink3.Status = 0
        Me.UctDocumentLink3.TabIndex = 381
        Me.UctDocumentLink3.Task = 0
        Me.UctDocumentLink3.UniqueId = ""
        Me.UctDocumentLink3.WrittenPolicyStatus = False
        '
        'UctDocumentLink4
        '
        Me.UctDocumentLink4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UctDocumentLink4.Location = New System.Drawing.Point(10, 270)
        Me.UctDocumentLink4.Name = "UctDocumentLink4"
        Me.UctDocumentLink4.ScreenHierarchy = ""
        Me.UctDocumentLink4.Size = New System.Drawing.Size(817, 243)
        Me.UctDocumentLink4.Status = 0
        Me.UctDocumentLink4.TabIndex = 386
        Me.UctDocumentLink4.Task = 0
        Me.UctDocumentLink4.UniqueId = ""
        Me.UctDocumentLink4.WrittenPolicyStatus = False
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.UctSIRSelectClauses1)
        Me.TabPage10.Location = New System.Drawing.Point(4, 40)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Size = New System.Drawing.Size(836, 624)
        Me.TabPage10.TabIndex = 9
        Me.TabPage10.Text = "6-Clauses"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'UctSIRSelectClauses1
        '
        Me.UctSIRSelectClauses1.ClauseId = 0
        Me.UctSIRSelectClauses1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UctSIRSelectClauses1.imglImages = Nothing
        Me.UctSIRSelectClauses1.Location = New System.Drawing.Point(8, 12)
        Me.UctSIRSelectClauses1.Name = "UctSIRSelectClauses1"
        Me.UctSIRSelectClauses1.ProductId = 0
        Me.UctSIRSelectClauses1.RiskId = 0
        Me.UctSIRSelectClauses1.ScreenHierarchy = ""
        Me.UctSIRSelectClauses1.Size = New System.Drawing.Size(819, 519)
        Me.UctSIRSelectClauses1.SystemCurrency = 0
        Me.UctSIRSelectClauses1.TabIndex = 382
        Me.UctSIRSelectClauses1.Task = 0
        Me.UctSIRSelectClauses1.UniqueId = ""
        '
        'GroupBox41
        '
        Me.GroupBox41.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox41.Controls.Add(Me.PickList5)
        Me.GroupBox41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox41.Location = New System.Drawing.Point(3, 6)
        Me.GroupBox41.Name = "GroupBox41"
        Me.GroupBox41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox41.Size = New System.Drawing.Size(819, 195)
        Me.GroupBox41.TabIndex = 186
        Me.GroupBox41.TabStop = False
        Me.GroupBox41.Text = "Branches"
        '
        'PickList5
        '
        Me.PickList5.AvailableCaption = "Restrict to Branches"
        Me.PickList5.BusinessObject = "bSIRProduct.Business"
        Me.PickList5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PickList5.ForeignKeys = CType(resources.GetObject("PickList5.ForeignKeys"), Microsoft.VisualBasic.Collection)
        Me.PickList5.IsSearchable = False
        Me.PickList5.Location = New System.Drawing.Point(8, 11)
        Me.PickList5.Name = "PickList5"
        Me.PickList5.PickListType = "Source"
        Me.PickList5.Size = New System.Drawing.Size(805, 174)
        Me.PickList5.TabIndex = 21
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(853, 739)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Product"
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit
        Me._tabMainTab_TabPage10.ResumeLayout(False)
        Me.GroupBox40.ResumeLayout(False)
        Me.GroupBox42.ResumeLayout(False)
        Me._tabMainTab_TabPage9.ResumeLayout(False)
        Me._tabMainTab_TabPage8.ResumeLayout(False)
        Me._tabMainTab_TabPage7.ResumeLayout(False)
        Me._tabMainTab_TabPage6.ResumeLayout(False)
        Me.fraAgentEmailComm.ResumeLayout(False)
        Me.fraAgentEmailComm.PerformLayout
        Me.fraTrueMonthlyPolFreqOfComm.ResumeLayout(False)
        Me.fraDirectConsumerComm.ResumeLayout(False)
        Me.fraRenewalUpdate.ResumeLayout(False)
        Me.fraRenewalUpdate.PerformLayout
        Me.fraRenewalInvite.ResumeLayout(False)
        Me.fraRenewalInvite.PerformLayout
        Me.fraRenewalSelection.ResumeLayout(False)
        Me.fraRenewalSelection.PerformLayout
        Me._tabMainTab_TabPage5.ResumeLayout(False)
        Me.fraClaimEventDesc.ResumeLayout(False)
        Me.fraMTAEventDesc.ResumeLayout(False)
        Me._tabMainTab_TabPage4.ResumeLayout(False)
        Me.fraRateMultipleRisks.ResumeLayout(False)
        Me.fraRateMultipleRisks.PerformLayout
        Me.fraMediaTypeStatusValidation.ResumeLayout(False)
        Me.fraCoverNote.ResumeLayout(False)
        Me.fraCoverNote.PerformLayout
        Me.FraOutofSeqMTA.ResumeLayout(False)
        Me.FraOutofSeqMTA.PerformLayout
        Me.FraVoidTransaction.ResumeLayout(False)
        Me.fraClaimSystemOptions.ResumeLayout(False)
        Me.Frame10.ResumeLayout(False)
        Me.Frame10.PerformLayout
        Me.Frame9.ResumeLayout(False)
        Me.Frame8.ResumeLayout(False)
        Me.Frame7.ResumeLayout(False)
        Me.fraExtClaimHandler.ResumeLayout(False)
        Me.fraExtClaimHandler.PerformLayout
        Me.fraUWClaims.ResumeLayout(False)
        Me.fraUWClaims.PerformLayout
        Me.fraNexus.ResumeLayout(False)
        Me.fraNexus.PerformLayout
        Me.Frame2.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me.frmRenewals.ResumeLayout(False)
        Me.frmRenewals.PerformLayout
        Me.frmLeadAgentCommission.ResumeLayout(False)
        Me.frmSubAgentCommission.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraOpenClaim.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.Frame6.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me.grpClaimScripts.ResumeLayout(False)
        Me.fraCausations.ResumeLayout(False)
        Me.fraLimits.ResumeLayout(False)
        Me.fraLimits.PerformLayout
        Me.fraNumbering.ResumeLayout(False)
        Me.fraSupression.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me.Frame4.PerformLayout
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraWrittenStatus.ResumeLayout(False)
        Me.fraWrittenStatus.PerformLayout
        Me.fraOtherOptions.ResumeLayout(False)
        Me.fraOtherOptions.PerformLayout
        Me.fraPremium.ResumeLayout(False)
        Me.fraPremium.PerformLayout
        Me.fraProRata.ResumeLayout(False)
        Me.fraRenewals.ResumeLayout(False)
        Me.fraRenewals.PerformLayout
        Me.fraPolicyCreation.ResumeLayout(False)
        Me.fraPolicyCreation.PerformLayout
        Me.fraDetails.ResumeLayout(False)
        Me.fraDetails.PerformLayout
        Me.tabMainTab.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox13.ResumeLayout(False)
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox15.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.GroupBox16.ResumeLayout(False)
        Me.GroupBox16.PerformLayout
        Me.GroupBox17.ResumeLayout(False)
        Me.GroupBox18.ResumeLayout(False)
        Me.GroupBox19.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.GroupBox20.ResumeLayout(False)
        Me.GroupBox21.ResumeLayout(False)
        Me.GroupBox21.PerformLayout
        Me.GroupBox22.ResumeLayout(False)
        Me.GroupBox22.PerformLayout
        Me.GroupBox23.ResumeLayout(False)
        Me.GroupBox24.ResumeLayout(False)
        Me.GroupBox25.ResumeLayout(False)
        Me.GroupBox26.ResumeLayout(False)
        Me.GroupBox27.ResumeLayout(False)
        Me.GroupBox27.PerformLayout
        Me.GroupBox28.ResumeLayout(False)
        Me.GroupBox28.PerformLayout
        Me.GroupBox29.ResumeLayout(False)
        Me.GroupBox29.PerformLayout
        Me.GroupBox30.ResumeLayout(False)
        Me.GroupBox31.ResumeLayout(False)
        Me.GroupBox31.PerformLayout
        Me.TabPage6.ResumeLayout(False)
        Me.GroupBox32.ResumeLayout(False)
        Me.GroupBox33.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.GroupBox34.ResumeLayout(False)
        Me.GroupBox34.PerformLayout
        Me.GroupBox35.ResumeLayout(False)
        Me.GroupBox36.ResumeLayout(False)
        Me.GroupBox37.ResumeLayout(False)
        Me.GroupBox37.PerformLayout
        Me.GroupBox38.ResumeLayout(False)
        Me.GroupBox38.PerformLayout
        Me.GroupBox39.ResumeLayout(False)
        Me.GroupBox39.PerformLayout
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage10.ResumeLayout(False)
        Me.GroupBox41.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializetxtEmailComm()
        Me.txtEmailComm(20) = _txtEmailComm_20
        Me.txtEmailComm(19) = _txtEmailComm_19
        Me.txtEmailComm(18) = _txtEmailComm_18
        Me.txtEmailComm(12) = _txtEmailComm_12
        Me.txtEmailComm(13) = _txtEmailComm_13
        Me.txtEmailComm(14) = _txtEmailComm_14
        Me.txtEmailComm(15) = _txtEmailComm_15
        Me.txtEmailComm(16) = _txtEmailComm_16
        Me.txtEmailComm(17) = _txtEmailComm_17
        Me.txtEmailComm(6) = _txtEmailComm_6
        Me.txtEmailComm(7) = _txtEmailComm_7
        Me.txtEmailComm(8) = _txtEmailComm_8
        Me.txtEmailComm(9) = _txtEmailComm_9
        Me.txtEmailComm(10) = _txtEmailComm_10
        Me.txtEmailComm(11) = _txtEmailComm_11
        Me.txtEmailComm(5) = _txtEmailComm_5
        Me.txtEmailComm(4) = _txtEmailComm_4
        Me.txtEmailComm(3) = _txtEmailComm_3
        Me.txtEmailComm(2) = _txtEmailComm_2
        Me.txtEmailComm(1) = _txtEmailComm_1
        Me.txtEmailComm(0) = _txtEmailComm_0
    End Sub
    Sub InitializelblEmailComm()
        Me.lblEmailComm(23) = _lblEmailComm_23
        Me.lblEmailComm(20) = _lblEmailComm_20
        Me.lblEmailComm(22) = _lblEmailComm_22
        Me.lblEmailComm(19) = _lblEmailComm_19
        Me.lblEmailComm(21) = _lblEmailComm_21
        Me.lblEmailComm(18) = _lblEmailComm_18
        Me.lblEmailComm(12) = _lblEmailComm_12
        Me.lblEmailComm(15) = _lblEmailComm_15
        Me.lblEmailComm(13) = _lblEmailComm_13
        Me.lblEmailComm(16) = _lblEmailComm_16
        Me.lblEmailComm(14) = _lblEmailComm_14
        Me.lblEmailComm(17) = _lblEmailComm_17
        Me.lblEmailComm(6) = _lblEmailComm_6
        Me.lblEmailComm(9) = _lblEmailComm_9
        Me.lblEmailComm(7) = _lblEmailComm_7
        Me.lblEmailComm(10) = _lblEmailComm_10
        Me.lblEmailComm(8) = _lblEmailComm_8
        Me.lblEmailComm(11) = _lblEmailComm_11
        Me.lblEmailComm(5) = _lblEmailComm_5
        Me.lblEmailComm(2) = _lblEmailComm_2
        Me.lblEmailComm(4) = _lblEmailComm_4
        Me.lblEmailComm(1) = _lblEmailComm_1
        Me.lblEmailComm(3) = _lblEmailComm_3
        Me.lblEmailComm(0) = _lblEmailComm_0
    End Sub
    Sub InitializelblClaimSystemOptions()
        Me.lblClaimSystemOptions(12) = _lblClaimSystemOptions_12
        Me.lblClaimSystemOptions(13) = _lblClaimSystemOptions_13
        Me.lblClaimSystemOptions(10) = _lblClaimSystemOptions_10
        Me.lblClaimSystemOptions(11) = _lblClaimSystemOptions_11
        Me.lblClaimSystemOptions(14) = _lblClaimSystemOptions_14
        Me.lblClaimSystemOptions(8) = _lblClaimSystemOptions_8
        Me.lblClaimSystemOptions(9) = _lblClaimSystemOptions_9
        Me.lblClaimSystemOptions(6) = _lblClaimSystemOptions_6
        Me.lblClaimSystemOptions(7) = _lblClaimSystemOptions_7
        Me.lblClaimSystemOptions(0) = _lblClaimSystemOptions_0
        Me.lblClaimSystemOptions(4) = _lblClaimSystemOptions_4
    End Sub
    Sub InitializecmdEmailCommSelect()
        Me.cmdEmailCommSelect(20) = _cmdEmailCommSelect_20
        Me.cmdEmailCommSelect(19) = _cmdEmailCommSelect_19
        Me.cmdEmailCommSelect(18) = _cmdEmailCommSelect_18
        Me.cmdEmailCommSelect(12) = _cmdEmailCommSelect_12
        Me.cmdEmailCommSelect(13) = _cmdEmailCommSelect_13
        Me.cmdEmailCommSelect(14) = _cmdEmailCommSelect_14
        Me.cmdEmailCommSelect(15) = _cmdEmailCommSelect_15
        Me.cmdEmailCommSelect(16) = _cmdEmailCommSelect_16
        Me.cmdEmailCommSelect(17) = _cmdEmailCommSelect_17
        Me.cmdEmailCommSelect(6) = _cmdEmailCommSelect_6
        Me.cmdEmailCommSelect(7) = _cmdEmailCommSelect_7
        Me.cmdEmailCommSelect(8) = _cmdEmailCommSelect_8
        Me.cmdEmailCommSelect(9) = _cmdEmailCommSelect_9
        Me.cmdEmailCommSelect(10) = _cmdEmailCommSelect_10
        Me.cmdEmailCommSelect(11) = _cmdEmailCommSelect_11
        Me.cmdEmailCommSelect(5) = _cmdEmailCommSelect_5
        Me.cmdEmailCommSelect(4) = _cmdEmailCommSelect_4
        Me.cmdEmailCommSelect(3) = _cmdEmailCommSelect_3
        Me.cmdEmailCommSelect(2) = _cmdEmailCommSelect_2
        Me.cmdEmailCommSelect(1) = _cmdEmailCommSelect_1
        Me.cmdEmailCommSelect(0) = _cmdEmailCommSelect_0
    End Sub
    Sub InitializecmdEmailCommDeSelect()
        Me.cmdEmailCommDeSelect(20) = _cmdEmailCommDeSelect_20
        Me.cmdEmailCommDeSelect(19) = _cmdEmailCommDeSelect_19
        Me.cmdEmailCommDeSelect(18) = _cmdEmailCommDeSelect_18
        Me.cmdEmailCommDeSelect(12) = _cmdEmailCommDeSelect_12
        Me.cmdEmailCommDeSelect(13) = _cmdEmailCommDeSelect_13
        Me.cmdEmailCommDeSelect(14) = _cmdEmailCommDeSelect_14
        Me.cmdEmailCommDeSelect(15) = _cmdEmailCommDeSelect_15
        Me.cmdEmailCommDeSelect(16) = _cmdEmailCommDeSelect_16
        Me.cmdEmailCommDeSelect(17) = _cmdEmailCommDeSelect_17
        Me.cmdEmailCommDeSelect(6) = _cmdEmailCommDeSelect_6
        Me.cmdEmailCommDeSelect(7) = _cmdEmailCommDeSelect_7
        Me.cmdEmailCommDeSelect(8) = _cmdEmailCommDeSelect_8
        Me.cmdEmailCommDeSelect(9) = _cmdEmailCommDeSelect_9
        Me.cmdEmailCommDeSelect(10) = _cmdEmailCommDeSelect_10
        Me.cmdEmailCommDeSelect(11) = _cmdEmailCommDeSelect_11
        Me.cmdEmailCommDeSelect(5) = _cmdEmailCommDeSelect_5
        Me.cmdEmailCommDeSelect(4) = _cmdEmailCommDeSelect_4
        Me.cmdEmailCommDeSelect(3) = _cmdEmailCommDeSelect_3
        Me.cmdEmailCommDeSelect(2) = _cmdEmailCommDeSelect_2
        Me.cmdEmailCommDeSelect(1) = _cmdEmailCommDeSelect_1
        Me.cmdEmailCommDeSelect(0) = _cmdEmailCommDeSelect_0
    End Sub
    Sub InitializechkPaymentClaimWorkflow()
        Me.chkPaymentClaimWorkflow(16) = _chkPaymentClaimWorkflow_16
        Me.chkPaymentClaimWorkflow(15) = _chkPaymentClaimWorkflow_15
        Me.chkPaymentClaimWorkflow(14) = _chkPaymentClaimWorkflow_14
        Me.chkPaymentClaimWorkflow(13) = _chkPaymentClaimWorkflow_13
        Me.chkPaymentClaimWorkflow(12) = _chkPaymentClaimWorkflow_12
        Me.chkPaymentClaimWorkflow(11) = _chkPaymentClaimWorkflow_11
        Me.chkPaymentClaimWorkflow(10) = _chkPaymentClaimWorkflow_10
        Me.chkPaymentClaimWorkflow(9) = _chkPaymentClaimWorkflow_9
        Me.chkPaymentClaimWorkflow(8) = _chkPaymentClaimWorkflow_8
        Me.chkPaymentClaimWorkflow(7) = _chkPaymentClaimWorkflow_7
        Me.chkPaymentClaimWorkflow(6) = _chkPaymentClaimWorkflow_6
        Me.chkPaymentClaimWorkflow(5) = _chkPaymentClaimWorkflow_5
        Me.chkPaymentClaimWorkflow(4) = _chkPaymentClaimWorkflow_4
        Me.chkPaymentClaimWorkflow(3) = _chkPaymentClaimWorkflow_3
        Me.chkPaymentClaimWorkflow(2) = _chkPaymentClaimWorkflow_2
        Me.chkPaymentClaimWorkflow(1) = _chkPaymentClaimWorkflow_1
        Me.chkPaymentClaimWorkflow(0) = _chkPaymentClaimWorkflow_0
    End Sub
    Sub InitializechkOpenClaimWorkflow()
        Me.chkOpenClaimWorkflow(25) = _chkOpenClaimWorkflow_25
        Me.chkOpenClaimWorkflow(24) = _chkOpenClaimWorkflow_24
        Me.chkOpenClaimWorkflow(23) = _chkOpenClaimWorkflow_23
        Me.chkOpenClaimWorkflow(22) = _chkOpenClaimWorkflow_22
        Me.chkOpenClaimWorkflow(21) = _chkOpenClaimWorkflow_21
        Me.chkOpenClaimWorkflow(20) = _chkOpenClaimWorkflow_20
        Me.chkOpenClaimWorkflow(19) = _chkOpenClaimWorkflow_19
        Me.chkOpenClaimWorkflow(18) = _chkOpenClaimWorkflow_18
        Me.chkOpenClaimWorkflow(17) = _chkOpenClaimWorkflow_17
        Me.chkOpenClaimWorkflow(16) = _chkOpenClaimWorkflow_16
        Me.chkOpenClaimWorkflow(15) = _chkOpenClaimWorkflow_15
        Me.chkOpenClaimWorkflow(14) = _chkOpenClaimWorkflow_14
        Me.chkOpenClaimWorkflow(13) = _chkOpenClaimWorkflow_13
        Me.chkOpenClaimWorkflow(12) = _chkOpenClaimWorkflow_12
        Me.chkOpenClaimWorkflow(11) = _chkOpenClaimWorkflow_11
        Me.chkOpenClaimWorkflow(10) = _chkOpenClaimWorkflow_10
        Me.chkOpenClaimWorkflow(9) = _chkOpenClaimWorkflow_9
        Me.chkOpenClaimWorkflow(8) = _chkOpenClaimWorkflow_8
        Me.chkOpenClaimWorkflow(7) = _chkOpenClaimWorkflow_7
        Me.chkOpenClaimWorkflow(6) = _chkOpenClaimWorkflow_6
        Me.chkOpenClaimWorkflow(5) = _chkOpenClaimWorkflow_5
        Me.chkOpenClaimWorkflow(4) = _chkOpenClaimWorkflow_4
        Me.chkOpenClaimWorkflow(3) = _chkOpenClaimWorkflow_3
        Me.chkOpenClaimWorkflow(2) = _chkOpenClaimWorkflow_2
        Me.chkOpenClaimWorkflow(1) = _chkOpenClaimWorkflow_1
        Me.chkOpenClaimWorkflow(0) = _chkOpenClaimWorkflow_0
    End Sub
    Sub InitializechkMaintainClaimWorkflow()
        Me.chkMaintainClaimWorkflow(26) = _chkMaintainClaimWorkflow_26
        Me.chkMaintainClaimWorkflow(25) = _chkMaintainClaimWorkflow_25
        Me.chkMaintainClaimWorkflow(24) = _chkMaintainClaimWorkflow_24
        Me.chkMaintainClaimWorkflow(23) = _chkMaintainClaimWorkflow_23
        Me.chkMaintainClaimWorkflow(22) = _chkMaintainClaimWorkflow_22
        Me.chkMaintainClaimWorkflow(21) = _chkMaintainClaimWorkflow_21
        Me.chkMaintainClaimWorkflow(20) = _chkMaintainClaimWorkflow_20
        Me.chkMaintainClaimWorkflow(19) = _chkMaintainClaimWorkflow_19
        Me.chkMaintainClaimWorkflow(18) = _chkMaintainClaimWorkflow_18
        Me.chkMaintainClaimWorkflow(17) = _chkMaintainClaimWorkflow_17
        Me.chkMaintainClaimWorkflow(16) = _chkMaintainClaimWorkflow_16
        Me.chkMaintainClaimWorkflow(15) = _chkMaintainClaimWorkflow_15
        Me.chkMaintainClaimWorkflow(14) = _chkMaintainClaimWorkflow_14
        Me.chkMaintainClaimWorkflow(13) = _chkMaintainClaimWorkflow_13
        Me.chkMaintainClaimWorkflow(12) = _chkMaintainClaimWorkflow_12
        Me.chkMaintainClaimWorkflow(11) = _chkMaintainClaimWorkflow_11
        Me.chkMaintainClaimWorkflow(10) = _chkMaintainClaimWorkflow_10
        Me.chkMaintainClaimWorkflow(8) = _chkMaintainClaimWorkflow_8
        Me.chkMaintainClaimWorkflow(7) = _chkMaintainClaimWorkflow_7
        Me.chkMaintainClaimWorkflow(6) = _chkMaintainClaimWorkflow_6
        Me.chkMaintainClaimWorkflow(5) = _chkMaintainClaimWorkflow_5
        Me.chkMaintainClaimWorkflow(4) = _chkMaintainClaimWorkflow_4
        Me.chkMaintainClaimWorkflow(3) = _chkMaintainClaimWorkflow_3
        Me.chkMaintainClaimWorkflow(2) = _chkMaintainClaimWorkflow_2
        Me.chkMaintainClaimWorkflow(1) = _chkMaintainClaimWorkflow_1
        Me.chkMaintainClaimWorkflow(0) = _chkMaintainClaimWorkflow_0
        Me.chkMaintainClaimWorkflow(9) = _chkMaintainClaimWorkflow_9
    End Sub
    Sub InitializecboUDT()
        Me.cboUDT(1) = _cboUDT_1
        Me.cboUDT(0) = _cboUDT_0
        Me.cboUDT(3) = _cboUDT_3
        Me.cboUDT(2) = _cboUDT_2
        Me.cboUDT(4) = _cboUDT_4
    End Sub
    Public WithEvents fraBranches As System.Windows.Forms.GroupBox
    Public WithEvents uctPickListBranches As uctPickList.PickList
    Private WithEvents _tabMainTab_TabPage9 As System.Windows.Forms.TabPage
    Public WithEvents uctSIRSelectClauses As uctSCControl.uctSIRSelectClauses
    Private WithEvents _tabMainTab_TabPage8 As System.Windows.Forms.TabPage
    Public WithEvents DocumentLinkClaim As uctPMUDocumentLink.uctDocumentLink
    Public WithEvents DocumentLinkClaimPayment As uctPMUDocumentLink.uctDocumentLink
    Private WithEvents _tabMainTab_TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents uctDocumentLink1 As uctPMUDocumentLink.uctDocumentLink
    Private WithEvents _tabMainTab_TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents fraAgentEmailComm As System.Windows.Forms.GroupBox
    Public WithEvents cboAgentUpdateAttachment As System.Windows.Forms.ComboBox
    Public WithEvents cboAgentInviteAttachment As System.Windows.Forms.ComboBox
    Public WithEvents cboAgentReviewAttachment As System.Windows.Forms.ComboBox
    Public WithEvents chkAgentRenInvite As System.Windows.Forms.CheckBox
    Public WithEvents chkAgentRenUpdate As System.Windows.Forms.CheckBox
    Private WithEvents _cmdEmailCommDeSelect_20 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_20 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_20 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_19 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_19 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_19 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_18 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_18 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_18 As System.Windows.Forms.TextBox
    Public WithEvents chkAgentRenSelection As System.Windows.Forms.CheckBox
    Private WithEvents _lblEmailComm_23 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_20 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_22 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_19 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_21 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_18 As System.Windows.Forms.Label
    Public WithEvents fraTrueMonthlyPolFreqOfComm As System.Windows.Forms.GroupBox
    Public WithEvents optRenewalProcessRun As System.Windows.Forms.RadioButton
    Public WithEvents optAnniversaryDate As System.Windows.Forms.RadioButton
    Public WithEvents fraDirectConsumerComm As System.Windows.Forms.GroupBox
    Public WithEvents fraRenewalUpdate As System.Windows.Forms.GroupBox
    Public WithEvents chkEnabledRenUpdate As System.Windows.Forms.CheckBox
    Private WithEvents _txtEmailComm_12 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_12 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_12 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_13 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_13 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_13 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_14 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_14 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_14 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_15 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_15 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_15 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_16 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_16 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_16 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_17 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_17 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_17 As System.Windows.Forms.Button
    Private WithEvents _lblEmailComm_12 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_15 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_13 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_16 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_14 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_17 As System.Windows.Forms.Label
    Public WithEvents fraRenewalInvite As System.Windows.Forms.GroupBox
    Public WithEvents chkEnabledRenInvite As System.Windows.Forms.CheckBox
    Private WithEvents _txtEmailComm_6 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_6 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_6 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_7 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_7 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_7 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_8 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_8 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_8 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_9 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_9 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_9 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_10 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_10 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_10 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_11 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommSelect_11 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommDeSelect_11 As System.Windows.Forms.Button
    Private WithEvents _lblEmailComm_6 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_9 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_7 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_10 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_8 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_11 As System.Windows.Forms.Label
    Public WithEvents fraRenewalSelection As System.Windows.Forms.GroupBox
    Private WithEvents _cmdEmailCommDeSelect_5 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_5 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_5 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_4 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_4 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_4 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_3 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_3 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_3 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_2 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_2 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_2 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_1 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_1 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_1 As System.Windows.Forms.TextBox
    Private WithEvents _cmdEmailCommDeSelect_0 As System.Windows.Forms.Button
    Private WithEvents _cmdEmailCommSelect_0 As System.Windows.Forms.Button
    Private WithEvents _txtEmailComm_0 As System.Windows.Forms.TextBox
    Public WithEvents chkEnabledRenSelection As System.Windows.Forms.CheckBox
    Private WithEvents _lblEmailComm_5 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_2 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_4 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_1 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_3 As System.Windows.Forms.Label
    Private WithEvents _lblEmailComm_0 As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents fraClaimEventDesc As System.Windows.Forms.GroupBox
    Public WithEvents uctPickListClaimEvent As uctPickList.PickList
    Public WithEvents fraMTAEventDesc As System.Windows.Forms.GroupBox
    Public WithEvents uctPickListMTAEvent As uctPickList.PickList
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents fraMediaTypeStatusValidation As System.Windows.Forms.GroupBox
    Public WithEvents chkValidateMediaTypeStatusAtClaimPayment As System.Windows.Forms.CheckBox
    Public WithEvents chkValidateMediaTypeStatusAtPolicyRefund As System.Windows.Forms.CheckBox
    Public WithEvents fraCoverNote As System.Windows.Forms.GroupBox
    Public WithEvents cmdCNDocTemplate As System.Windows.Forms.Button
    Public WithEvents txtCNDocTemplate As System.Windows.Forms.TextBox
    Public WithEvents txtCNMaxNo As System.Windows.Forms.TextBox
    Public WithEvents txtCNDefaultPeriod As System.Windows.Forms.TextBox
    Public WithEvents lblCNDocTemplate As System.Windows.Forms.Label
    Public WithEvents lblCNMaxNo As System.Windows.Forms.Label
    Public WithEvents lblCNDefaultPeriod As System.Windows.Forms.Label
    Public WithEvents FraOutofSeqMTA As System.Windows.Forms.GroupBox
    Public WithEvents FraVoidTransaction As System.Windows.Forms.GroupBox
    Public WithEvents chkVoidPolicyVersion As System.Windows.Forms.CheckBox
    Public WithEvents cboOSMTATaskGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboOSMTAUserGroup As System.Windows.Forms.ComboBox
    Public WithEvents cbodtAllowed As System.Windows.Forms.ComboBox
    Public WithEvents cboallocation As System.Windows.Forms.ComboBox
    Public WithEvents lblTaskGroup As System.Windows.Forms.Label
    Public WithEvents lblUserGroup As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents fraClaimSystemOptions As System.Windows.Forms.GroupBox
    Public WithEvents Frame9 As System.Windows.Forms.GroupBox
    Public WithEvents chkBackdatedMTA As System.Windows.Forms.CheckBox
    Public WithEvents chkBackdatedCan As System.Windows.Forms.CheckBox
    Public WithEvents Frame8 As System.Windows.Forms.GroupBox
    Public WithEvents chkDuplicateClaim As System.Windows.Forms.CheckBox
    Public WithEvents chkAdvanceTaxScript As System.Windows.Forms.CheckBox
    Public WithEvents chkPaymentRefCheck As System.Windows.Forms.CheckBox
    Public WithEvents Frame7 As System.Windows.Forms.GroupBox
    Private WithEvents _cboUDT_1 As System.Windows.Forms.ComboBox
    Private WithEvents _cboUDT_0 As System.Windows.Forms.ComboBox
    Private WithEvents _cboUDT_3 As System.Windows.Forms.ComboBox
    Private WithEvents _cboUDT_2 As System.Windows.Forms.ComboBox
    Private WithEvents _cboUDT_4 As System.Windows.Forms.ComboBox
    Private WithEvents _lblClaimSystemOptions_12 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_13 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_10 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_11 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_14 As System.Windows.Forms.Label
    Public WithEvents fraExtClaimHandler As System.Windows.Forms.GroupBox
    Public WithEvents cboClaimTaskGroup As System.Windows.Forms.ComboBox
    Public WithEvents cboClaimUserGroup As System.Windows.Forms.ComboBox
    Public WithEvents txtAckTaskAllowedTime As System.Windows.Forms.TextBox
    Public WithEvents txtPreReportAllowedTime As System.Windows.Forms.TextBox
    Private WithEvents _lblClaimSystemOptions_8 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_9 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_6 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_7 As System.Windows.Forms.Label
    Public WithEvents fraUWClaims As System.Windows.Forms.GroupBox
    Public WithEvents chkPaymentCannotExceedReserve As System.Windows.Forms.CheckBox
    Public WithEvents chkRecoveryInstalmentsEnabled As System.Windows.Forms.CheckBox
    Public WithEvents chkAllowNegativeReserve As System.Windows.Forms.CheckBox
    Public WithEvents chkValidPolicyAtLossDate As System.Windows.Forms.CheckBox
    Public WithEvents chkClaimPaymentGross As System.Windows.Forms.CheckBox
    Public WithEvents txtLargeLossAdviceValue As System.Windows.Forms.TextBox
    Public WithEvents cboCoinsurerInclusion As System.Windows.Forms.ComboBox
    Private WithEvents _lblClaimSystemOptions_0 As System.Windows.Forms.Label
    Private WithEvents _lblClaimSystemOptions_4 As System.Windows.Forms.Label
    Public WithEvents fraNexus As System.Windows.Forms.GroupBox
    Public WithEvents txtOnlineCommencedOn As System.Windows.Forms.TextBox
    Public WithEvents chkTradeRnlOnline As System.Windows.Forms.CheckBox
    Public WithEvents chkTradeMtaOnline As System.Windows.Forms.CheckBox
    Public WithEvents chkTradeNbOnline As System.Windows.Forms.CheckBox
    Public WithEvents lblOnlineCommencedOn As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents chkProduceDebitNote As System.Windows.Forms.CheckBox
    Public WithEvents chkProduceCertificate As System.Windows.Forms.CheckBox
    Public WithEvents chkProduceSchedule As System.Windows.Forms.CheckBox
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents chkCashDeposit As System.Windows.Forms.CheckBox
    Public WithEvents chkBankGuarantee As System.Windows.Forms.CheckBox
    Public WithEvents chkInvoice As System.Windows.Forms.CheckBox
    Public WithEvents chkInstalments As System.Windows.Forms.CheckBox
    Public WithEvents chkPayNow As System.Windows.Forms.CheckBox
    Public WithEvents lblCashDeposit As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents frmRenewals As System.Windows.Forms.GroupBox
    Friend WithEvents chkTMPAutoRenFAC As System.Windows.Forms.CheckBox
    Friend WithEvents lblMonthlyAutoRenWithFac As System.Windows.Forms.Label
    Public WithEvents txtUnifiedRenewalDay As System.Windows.Forms.TextBox
    Public WithEvents txtAnniversaryRenewalWeeks As System.Windows.Forms.TextBox
    Public WithEvents lblUnifiedRenewalDay As System.Windows.Forms.Label
    Public WithEvents lblAnniversaryRenewalWeeks As System.Windows.Forms.Label
    Public WithEvents frmLeadAgentCommission As System.Windows.Forms.GroupBox
    Public WithEvents actSuspenseAcc As UserControls.AccountLookup
    Public WithEvents cboMonthInCycleLA As System.Windows.Forms.ComboBox
    Public WithEvents chkAllowConsolidateCommissionLA As System.Windows.Forms.CheckBox
    Public WithEvents lblLeadAgentCommSuspenseLA As System.Windows.Forms.Label
    Public WithEvents lblMonthInCycleLA As System.Windows.Forms.Label
    Public WithEvents frmSubAgentCommission As System.Windows.Forms.GroupBox
    Public WithEvents actSuspenseAcc1 As UserControls.AccountLookup
    Public WithEvents cboMonthInCycleSA As System.Windows.Forms.ComboBox
    Public WithEvents chkAllowConsolidateCommissionSA As System.Windows.Forms.CheckBox
    Public WithEvents lblSubAgentCommSuspense As System.Windows.Forms.Label
    Public WithEvents lblMonthInCycleSA As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Public WithEvents OptInstalments As System.Windows.Forms.RadioButton
    Public WithEvents OptInvoice As System.Windows.Forms.RadioButton
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents fraOpenClaim As System.Windows.Forms.GroupBox
    Private WithEvents _chkOpenClaimWorkflow_25 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_24 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_23 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_22 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_21 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_20 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_19 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_18 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_17 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_16 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_15 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_14 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_13 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_12 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_11 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_10 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_9 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_8 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_7 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_6 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_5 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_4 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_3 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_2 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkOpenClaimWorkflow_0 As System.Windows.Forms.CheckBox
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Private WithEvents _chkPaymentClaimWorkflow_16 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_15 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_14 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_13 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_12 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_11 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_10 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_9 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_8 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_7 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_6 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_5 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_4 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_3 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_2 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkPaymentClaimWorkflow_0 As System.Windows.Forms.CheckBox
    Public WithEvents Frame6 As System.Windows.Forms.GroupBox
    Private WithEvents _chkMaintainClaimWorkflow_26 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_25 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_24 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_23 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_22 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_21 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_20 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_19 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_18 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_17 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_16 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_15 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_14 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_13 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_12 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_11 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_10 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_8 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_7 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_6 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_5 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_4 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_3 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_2 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_0 As System.Windows.Forms.CheckBox
    Private WithEvents _chkMaintainClaimWorkflow_9 As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents fraCausations As System.Windows.Forms.GroupBox
    Public WithEvents uctPickListCausations As uctPickList.PickList
    Public WithEvents fraLimits As System.Windows.Forms.GroupBox
    Public WithEvents txtAllowedClaims As System.Windows.Forms.TextBox
    Public WithEvents txtClaimYear As System.Windows.Forms.TextBox
    Public WithEvents txtSingleClaimValue As System.Windows.Forms.TextBox
    Public WithEvents txtTotalClaimsValue As System.Windows.Forms.TextBox
    Public WithEvents chkCheckAgent As System.Windows.Forms.CheckBox
    Public WithEvents chkMediaTypeMandatory As System.Windows.Forms.CheckBox
    Public WithEvents chkLossCurrencyChange As System.Windows.Forms.CheckBox
    Public WithEvents cboBankAccount As UserControls.BankAccount
    Public WithEvents lblBankAccount As System.Windows.Forms.Label
    Public WithEvents lblAllowedClaims As System.Windows.Forms.Label
    Public WithEvents lblClaimYear As System.Windows.Forms.Label
    Public WithEvents lblSingleClaimValue As System.Windows.Forms.Label
    Public WithEvents lblTotalClaimsValue As System.Windows.Forms.Label
    Public WithEvents fraNumbering As System.Windows.Forms.GroupBox
    Public WithEvents cboFullClaimAutoNumberingID As System.Windows.Forms.ComboBox
    Public WithEvents cboProvClaimAutoNumberingID As System.Windows.Forms.ComboBox
    Public WithEvents lblFullClaimAutoNumberingID As System.Windows.Forms.Label
    Public WithEvents lblProvClaimAutoNumberingID As System.Windows.Forms.Label
    Public WithEvents fraSupression As System.Windows.Forms.GroupBox
    Public WithEvents chkRecoveries As System.Windows.Forms.CheckBox
    Public WithEvents chkPayments As System.Windows.Forms.CheckBox
    Public WithEvents chkReserve As System.Windows.Forms.CheckBox
    Public WithEvents Frame4 As System.Windows.Forms.GroupBox
    Public WithEvents chkRecommender As System.Windows.Forms.CheckBox
    Public WithEvents txtMaxNoofUnauthorisedClaimPayments As System.Windows.Forms.TextBox
    Public WithEvents txtMaxUnauthorisedClaimsValue As System.Windows.Forms.TextBox
    Public WithEvents chkMultiplePayments As System.Windows.Forms.CheckBox
    Public WithEvents chkRunAuthorisationScriptsClaimPayments As System.Windows.Forms.CheckBox
    Public WithEvents lblMaxNoofUnauthorisedClaimPayments As System.Windows.Forms.Label
    Public WithEvents lblMaxUnauthorisedClaimsValue As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents fraWrittenStatus As System.Windows.Forms.GroupBox
    Friend WithEvents cboReminderTaskGroup As System.Windows.Forms.ComboBox
    Friend WithEvents cboReminderUserGroup As System.Windows.Forms.ComboBox
    Friend WithEvents lblReminderTaskGroup As System.Windows.Forms.Label
    Friend WithEvents txtTaskManagerDays As System.Windows.Forms.TextBox
    Friend WithEvents lblReminderUserGroup As System.Windows.Forms.Label
    Friend WithEvents lblTaskManagerDays As System.Windows.Forms.Label
    Public WithEvents fraOtherOptions As System.Windows.Forms.GroupBox
    Public WithEvents cmdRIModel As System.Windows.Forms.Button
    Public WithEvents cboPosValues As System.Windows.Forms.ComboBox
    Public WithEvents chkAllowStandardWordingEdit As System.Windows.Forms.CheckBox
    Public WithEvents lblPositiveValues As System.Windows.Forms.Label
    Public WithEvents lblAllowStandardWordingEdit As System.Windows.Forms.Label
    Public WithEvents fraPremium As System.Windows.Forms.GroupBox
    Public WithEvents chkRoundOffToZero As System.Windows.Forms.CheckBox
    Public WithEvents chkRoundPremium As System.Windows.Forms.CheckBox
    Public WithEvents chkCurrencyChange As System.Windows.Forms.CheckBox
    Public WithEvents chkTaxSuppressed As System.Windows.Forms.CheckBox
    Public WithEvents chkAccumulation As System.Windows.Forms.CheckBox
    Public WithEvents cboRoundingSection As PMLookupControl.cboPMLookup
    Public WithEvents lblRoundingSection As System.Windows.Forms.Label
    Public WithEvents lblRoundPremium As System.Windows.Forms.Label
    Public WithEvents lblCurrencyChange As System.Windows.Forms.Label
    Public WithEvents lblAccumulation As System.Windows.Forms.Label
    Public WithEvents lblTaxSuppressed As System.Windows.Forms.Label
    Public WithEvents fraProRata As System.Windows.Forms.GroupBox
    Public WithEvents chkMTCRatingRules As System.Windows.Forms.CheckBox
    Public WithEvents cmdSPR As System.Windows.Forms.Button
    Public WithEvents chkMTAProRata As System.Windows.Forms.CheckBox
    Public WithEvents chkNBProRata As System.Windows.Forms.CheckBox
    Public WithEvents chkShortPeriodRated As System.Windows.Forms.CheckBox
    Public WithEvents lblMTCRatingRules As System.Windows.Forms.Label
    Public WithEvents lblMTAProRata As System.Windows.Forms.Label
    Public WithEvents lblNBProRata As System.Windows.Forms.Label
    Public WithEvents lblShortPeriodRated As System.Windows.Forms.Label
    Public WithEvents fraRenewals As System.Windows.Forms.GroupBox
    Public WithEvents chkBindRenewalWOInvitation As System.Windows.Forms.CheckBox
    Friend WithEvents chkUsePriorSchemeAtRenewal As System.Windows.Forms.CheckBox
    Public WithEvents chkUseNBRenPaymentTermsAtSelection As System.Windows.Forms.CheckBox
    Public WithEvents chkChangePolicyNumberAtRenewalAutomatically As System.Windows.Forms.CheckBox
    Public WithEvents txtdefaultRenMth As System.Windows.Forms.TextBox
    Public WithEvents chkRenewable As System.Windows.Forms.CheckBox
    Public WithEvents chkHideSummaryAtRenewalAcceptance As System.Windows.Forms.CheckBox
    Public WithEvents chkChangePolicyNumberAtRenewal As System.Windows.Forms.CheckBox
    Public WithEvents chkMidNightRenewal As System.Windows.Forms.CheckBox
    Public WithEvents chkAutoRenewal As System.Windows.Forms.CheckBox
    Public WithEvents txtRenewalPeriod As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents lblAutoRenewal As System.Windows.Forms.Label
    Public WithEvents lblRenewalPeriod As System.Windows.Forms.Label
    Public WithEvents fraPolicyCreation As System.Windows.Forms.GroupBox
    Friend WithEvents chkWrittenPolicy As System.Windows.Forms.CheckBox
    Public WithEvents chkReinsuranceManualPremiumAdjustment As System.Windows.Forms.CheckBox
    Public WithEvents cboCNAutoNumberingID As System.Windows.Forms.ComboBox
    Public WithEvents chkTrueMonthlyPolicy As System.Windows.Forms.CheckBox
    Public WithEvents txtGracePeriod As System.Windows.Forms.TextBox
    Public WithEvents chkPolicyNumberAtQuote As System.Windows.Forms.CheckBox
    Public WithEvents cboPolicyAutoNumberingID As System.Windows.Forms.ComboBox
    Public WithEvents cboQuoteAutoNumberingID As System.Windows.Forms.ComboBox
    Public WithEvents chkPolicyStyleMandatory As System.Windows.Forms.CheckBox
    Public WithEvents cboPolicyStyle As PMLookupControl.cboPMLookup
    Public WithEvents lbCNNumbering As System.Windows.Forms.Label
    Public WithEvents lblGracePeriod As System.Windows.Forms.Label
    Public WithEvents lblPolicyNumberAtQuote As System.Windows.Forms.Label
    Public WithEvents lblPolicyAutoNumberingID As System.Windows.Forms.Label
    Public WithEvents lblQuoteAutoNumberingID As System.Windows.Forms.Label
    Public WithEvents lblPolicyStyle As System.Windows.Forms.Label
    Public WithEvents lblPolicyStyleMandatory As System.Windows.Forms.Label
    Public WithEvents fraDetails As System.Windows.Forms.GroupBox
    Public WithEvents txtBlockNo As System.Windows.Forms.TextBox
    Public WithEvents txtRIPointer As System.Windows.Forms.TextBox
    Public WithEvents txtReportPointer As System.Windows.Forms.TextBox
    Public WithEvents txtSchemeAgencyRef As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents cboRiskTypeGroup As PMLookupControl.cboPMLookup
    Public WithEvents lblBlockNo As System.Windows.Forms.Label
    Public WithEvents lblRIPointer As System.Windows.Forms.Label
    Public WithEvents lblReportPointer As System.Windows.Forms.Label
    Public WithEvents lblRiskTypeGroup As System.Windows.Forms.Label
    Public WithEvents lblSchemeAgencyRef As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents Button1 As System.Windows.Forms.Button
    Public WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Public WithEvents CboPMLookup1 As PMLookupControl.cboPMLookup
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox7 As System.Windows.Forms.CheckBox
    Public WithEvents Button2 As System.Windows.Forms.Button
    Public WithEvents CheckBox8 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox9 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox10 As System.Windows.Forms.CheckBox
    Public WithEvents Label15 As System.Windows.Forms.Label
    Public WithEvents Label16 As System.Windows.Forms.Label
    Public WithEvents Label17 As System.Windows.Forms.Label
    Public WithEvents Label18 As System.Windows.Forms.Label
    Public WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox11 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox12 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox13 As System.Windows.Forms.CheckBox
    Public WithEvents TextBox1 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox14 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox15 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox16 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox17 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox18 As System.Windows.Forms.CheckBox
    Public WithEvents TextBox2 As System.Windows.Forms.TextBox
    Public WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents Label20 As System.Windows.Forms.Label
    Public WithEvents Label21 As System.Windows.Forms.Label
    Public WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox19 As System.Windows.Forms.CheckBox
    Public WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox20 As System.Windows.Forms.CheckBox
    Public WithEvents TextBox3 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox21 As System.Windows.Forms.CheckBox
    Public WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox22 As System.Windows.Forms.CheckBox
    Public WithEvents CboPMLookup2 As PMLookupControl.cboPMLookup
    Public WithEvents Label22 As System.Windows.Forms.Label
    Public WithEvents Label23 As System.Windows.Forms.Label
    Public WithEvents Label24 As System.Windows.Forms.Label
    Public WithEvents Label25 As System.Windows.Forms.Label
    Public WithEvents Label26 As System.Windows.Forms.Label
    Public WithEvents Label27 As System.Windows.Forms.Label
    Public WithEvents Label28 As System.Windows.Forms.Label
    Public WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Public WithEvents TextBox4 As System.Windows.Forms.TextBox
    Public WithEvents TextBox5 As System.Windows.Forms.TextBox
    Public WithEvents TextBox6 As System.Windows.Forms.TextBox
    Public WithEvents TextBox7 As System.Windows.Forms.TextBox
    Public WithEvents TextBox8 As System.Windows.Forms.TextBox
    Public WithEvents TextBox9 As System.Windows.Forms.TextBox
    Public WithEvents TextBox10 As System.Windows.Forms.TextBox
    Public WithEvents CboPMLookup3 As PMLookupControl.cboPMLookup
    Public WithEvents Label29 As System.Windows.Forms.Label
    Public WithEvents Label30 As System.Windows.Forms.Label
    Public WithEvents Label31 As System.Windows.Forms.Label
    Public WithEvents Label32 As System.Windows.Forms.Label
    Public WithEvents Label33 As System.Windows.Forms.Label
    Public WithEvents Label34 As System.Windows.Forms.Label
    Public WithEvents Label35 As System.Windows.Forms.Label
    Public WithEvents Label36 As System.Windows.Forms.Label
    Private WithEvents TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Public WithEvents PickList1 As uctPickList.PickList
    Public WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Public WithEvents TextBox11 As System.Windows.Forms.TextBox
    Public WithEvents TextBox12 As System.Windows.Forms.TextBox
    Public WithEvents TextBox13 As System.Windows.Forms.TextBox
    Public WithEvents TextBox14 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox23 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox24 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox25 As System.Windows.Forms.CheckBox
    Public WithEvents BankAccount1 As UserControls.BankAccount
    Public WithEvents Label37 As System.Windows.Forms.Label
    Public WithEvents Label38 As System.Windows.Forms.Label
    Public WithEvents Label39 As System.Windows.Forms.Label
    Public WithEvents Label40 As System.Windows.Forms.Label
    Public WithEvents Label41 As System.Windows.Forms.Label
    Public WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Public WithEvents ComboBox5 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox6 As System.Windows.Forms.ComboBox
    Public WithEvents Label42 As System.Windows.Forms.Label
    Public WithEvents Label43 As System.Windows.Forms.Label
    Public WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox26 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox27 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox28 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Public WithEvents CboPMLookup4 As PMLookupControl.cboPMLookup
    Public WithEvents CboPMLookup5 As PMLookupControl.cboPMLookup
    Public WithEvents Label44 As System.Windows.Forms.Label
    Public WithEvents Label45 As System.Windows.Forms.Label
    Public WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox29 As System.Windows.Forms.CheckBox
    Public WithEvents TextBox15 As System.Windows.Forms.TextBox
    Public WithEvents TextBox16 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox30 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox31 As System.Windows.Forms.CheckBox
    Public WithEvents Label46 As System.Windows.Forms.Label
    Public WithEvents Label47 As System.Windows.Forms.Label
    Private WithEvents TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox32 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox33 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox34 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox35 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox36 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox37 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox38 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox39 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox40 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox41 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox42 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox43 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox44 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox45 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox46 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox47 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox48 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox49 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox50 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox51 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox52 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox53 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox54 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox55 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox56 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox57 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox58 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox59 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox60 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox61 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox62 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox63 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox64 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox65 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox66 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox67 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox68 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox69 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox70 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox71 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox72 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox73 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox74 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox15 As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox75 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox76 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox77 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox78 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox79 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox80 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox81 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox82 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox83 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox84 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox85 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox86 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox87 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox88 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox89 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox90 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox91 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox92 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox93 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox94 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox95 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox96 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox97 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox98 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox99 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox100 As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox101 As System.Windows.Forms.CheckBox
    Private WithEvents TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox16 As System.Windows.Forms.GroupBox
    Public WithEvents TextBox17 As System.Windows.Forms.TextBox
    Public WithEvents TextBox18 As System.Windows.Forms.TextBox
    Public WithEvents Label48 As System.Windows.Forms.Label
    Public WithEvents Label49 As System.Windows.Forms.Label
    Public WithEvents GroupBox17 As System.Windows.Forms.GroupBox
    Public WithEvents AccountLookup1 As UserControls.AccountLookup
    Public WithEvents ComboBox7 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox102 As System.Windows.Forms.CheckBox
    Public WithEvents Label50 As System.Windows.Forms.Label
    Public WithEvents Label51 As System.Windows.Forms.Label
    Public WithEvents GroupBox18 As System.Windows.Forms.GroupBox
    Public WithEvents AccountLookup2 As UserControls.AccountLookup
    Public WithEvents ComboBox8 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox103 As System.Windows.Forms.CheckBox
    Public WithEvents Label52 As System.Windows.Forms.Label
    Public WithEvents Label53 As System.Windows.Forms.Label
    Public WithEvents GroupBox19 As System.Windows.Forms.GroupBox
    Public WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Private WithEvents TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox20 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox104 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox105 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox21 As System.Windows.Forms.GroupBox
    Public WithEvents Button3 As System.Windows.Forms.Button
    Public WithEvents TextBox19 As System.Windows.Forms.TextBox
    Public WithEvents TextBox20 As System.Windows.Forms.TextBox
    Public WithEvents TextBox21 As System.Windows.Forms.TextBox
    Public WithEvents Label54 As System.Windows.Forms.Label
    Public WithEvents Label55 As System.Windows.Forms.Label
    Public WithEvents Label56 As System.Windows.Forms.Label
    Public WithEvents GroupBox22 As System.Windows.Forms.GroupBox
    Public WithEvents ComboBox9 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox10 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox11 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox12 As System.Windows.Forms.ComboBox
    Public WithEvents Label57 As System.Windows.Forms.Label
    Public WithEvents Label58 As System.Windows.Forms.Label
    Public WithEvents Label59 As System.Windows.Forms.Label
    Public WithEvents Label60 As System.Windows.Forms.Label
    Public WithEvents GroupBox23 As System.Windows.Forms.GroupBox
    Public WithEvents GroupBox24 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox106 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox107 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox25 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox108 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox109 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox110 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox26 As System.Windows.Forms.GroupBox
    Private WithEvents ComboBox13 As System.Windows.Forms.ComboBox
    Private WithEvents ComboBox14 As System.Windows.Forms.ComboBox
    Private WithEvents ComboBox15 As System.Windows.Forms.ComboBox
    Private WithEvents ComboBox16 As System.Windows.Forms.ComboBox
    Private WithEvents ComboBox17 As System.Windows.Forms.ComboBox
    Private WithEvents Label61 As System.Windows.Forms.Label
    Private WithEvents Label62 As System.Windows.Forms.Label
    Private WithEvents Label63 As System.Windows.Forms.Label
    Private WithEvents Label64 As System.Windows.Forms.Label
    Private WithEvents Label65 As System.Windows.Forms.Label
    Public WithEvents GroupBox27 As System.Windows.Forms.GroupBox
    Public WithEvents ComboBox18 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox19 As System.Windows.Forms.ComboBox
    Public WithEvents TextBox22 As System.Windows.Forms.TextBox
    Public WithEvents TextBox23 As System.Windows.Forms.TextBox
    Private WithEvents Label66 As System.Windows.Forms.Label
    Private WithEvents Label67 As System.Windows.Forms.Label
    Private WithEvents Label68 As System.Windows.Forms.Label
    Private WithEvents Label69 As System.Windows.Forms.Label
    Public WithEvents GroupBox28 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox111 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox112 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox113 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox114 As System.Windows.Forms.CheckBox
    Public WithEvents TextBox24 As System.Windows.Forms.TextBox
    Public WithEvents ComboBox20 As System.Windows.Forms.ComboBox
    Private WithEvents Label70 As System.Windows.Forms.Label
    Private WithEvents Label71 As System.Windows.Forms.Label
    Public WithEvents GroupBox29 As System.Windows.Forms.GroupBox
    Public WithEvents TextBox25 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox115 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox116 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox117 As System.Windows.Forms.CheckBox
    Public WithEvents Label72 As System.Windows.Forms.Label
    Public WithEvents GroupBox30 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox118 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox119 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox120 As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox31 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox121 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox122 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox123 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox124 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox125 As System.Windows.Forms.CheckBox
    Public WithEvents Label73 As System.Windows.Forms.Label
    Public WithEvents Label74 As System.Windows.Forms.Label
    Public WithEvents Label75 As System.Windows.Forms.Label
    Public WithEvents Label76 As System.Windows.Forms.Label
    Public WithEvents Label77 As System.Windows.Forms.Label
    Private WithEvents TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox32 As System.Windows.Forms.GroupBox
    Public WithEvents PickList2 As uctPickList.PickList
    Public WithEvents GroupBox33 As System.Windows.Forms.GroupBox
    Public WithEvents PickList3 As uctPickList.PickList
    Private WithEvents TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox34 As System.Windows.Forms.GroupBox
    Public WithEvents ComboBox21 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox22 As System.Windows.Forms.ComboBox
    Public WithEvents ComboBox23 As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox126 As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox127 As System.Windows.Forms.CheckBox
    Private WithEvents Button4 As System.Windows.Forms.Button
    Private WithEvents Button5 As System.Windows.Forms.Button
    Private WithEvents TextBox26 As System.Windows.Forms.TextBox
    Private WithEvents Button6 As System.Windows.Forms.Button
    Private WithEvents Button7 As System.Windows.Forms.Button
    Private WithEvents TextBox27 As System.Windows.Forms.TextBox
    Private WithEvents Button8 As System.Windows.Forms.Button
    Private WithEvents Button9 As System.Windows.Forms.Button
    Private WithEvents TextBox28 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox128 As System.Windows.Forms.CheckBox
    Private WithEvents Label78 As System.Windows.Forms.Label
    Private WithEvents Label79 As System.Windows.Forms.Label
    Private WithEvents Label80 As System.Windows.Forms.Label
    Private WithEvents Label81 As System.Windows.Forms.Label
    Private WithEvents Label82 As System.Windows.Forms.Label
    Private WithEvents Label83 As System.Windows.Forms.Label
    Public WithEvents GroupBox35 As System.Windows.Forms.GroupBox
    Public WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Public WithEvents GroupBox36 As System.Windows.Forms.GroupBox
    Public WithEvents GroupBox37 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox129 As System.Windows.Forms.CheckBox
    Private WithEvents TextBox29 As System.Windows.Forms.TextBox
    Private WithEvents Button10 As System.Windows.Forms.Button
    Private WithEvents Button11 As System.Windows.Forms.Button
    Private WithEvents TextBox30 As System.Windows.Forms.TextBox
    Private WithEvents Button12 As System.Windows.Forms.Button
    Private WithEvents Button13 As System.Windows.Forms.Button
    Private WithEvents TextBox31 As System.Windows.Forms.TextBox
    Private WithEvents Button14 As System.Windows.Forms.Button
    Private WithEvents Button15 As System.Windows.Forms.Button
    Private WithEvents TextBox32 As System.Windows.Forms.TextBox
    Private WithEvents Button16 As System.Windows.Forms.Button
    Private WithEvents Button17 As System.Windows.Forms.Button
    Private WithEvents TextBox33 As System.Windows.Forms.TextBox
    Private WithEvents Button18 As System.Windows.Forms.Button
    Private WithEvents Button19 As System.Windows.Forms.Button
    Private WithEvents TextBox34 As System.Windows.Forms.TextBox
    Private WithEvents Button20 As System.Windows.Forms.Button
    Private WithEvents Button21 As System.Windows.Forms.Button
    Private WithEvents Label84 As System.Windows.Forms.Label
    Private WithEvents Label85 As System.Windows.Forms.Label
    Private WithEvents Label86 As System.Windows.Forms.Label
    Private WithEvents Label87 As System.Windows.Forms.Label
    Private WithEvents Label88 As System.Windows.Forms.Label
    Private WithEvents Label89 As System.Windows.Forms.Label
    Public WithEvents GroupBox38 As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox130 As System.Windows.Forms.CheckBox
    Private WithEvents TextBox35 As System.Windows.Forms.TextBox
    Private WithEvents Button22 As System.Windows.Forms.Button
    Private WithEvents Button23 As System.Windows.Forms.Button
    Private WithEvents TextBox36 As System.Windows.Forms.TextBox
    Private WithEvents Button24 As System.Windows.Forms.Button
    Private WithEvents Button25 As System.Windows.Forms.Button
    Private WithEvents TextBox37 As System.Windows.Forms.TextBox
    Private WithEvents Button26 As System.Windows.Forms.Button
    Private WithEvents Button27 As System.Windows.Forms.Button
    Private WithEvents TextBox38 As System.Windows.Forms.TextBox
    Private WithEvents Button28 As System.Windows.Forms.Button
    Private WithEvents Button29 As System.Windows.Forms.Button
    Private WithEvents TextBox39 As System.Windows.Forms.TextBox
    Private WithEvents Button30 As System.Windows.Forms.Button
    Private WithEvents Button31 As System.Windows.Forms.Button
    Private WithEvents TextBox40 As System.Windows.Forms.TextBox
    Private WithEvents Button32 As System.Windows.Forms.Button
    Private WithEvents Button33 As System.Windows.Forms.Button
    Private WithEvents Label90 As System.Windows.Forms.Label
    Private WithEvents Label91 As System.Windows.Forms.Label
    Private WithEvents Label92 As System.Windows.Forms.Label
    Private WithEvents Label93 As System.Windows.Forms.Label
    Private WithEvents Label94 As System.Windows.Forms.Label
    Private WithEvents Label95 As System.Windows.Forms.Label
    Public WithEvents GroupBox39 As System.Windows.Forms.GroupBox
    Private WithEvents Button34 As System.Windows.Forms.Button
    Private WithEvents Button35 As System.Windows.Forms.Button
    Private WithEvents TextBox41 As System.Windows.Forms.TextBox
    Private WithEvents Button36 As System.Windows.Forms.Button
    Private WithEvents Button37 As System.Windows.Forms.Button
    Private WithEvents TextBox42 As System.Windows.Forms.TextBox
    Private WithEvents Button38 As System.Windows.Forms.Button
    Private WithEvents Button39 As System.Windows.Forms.Button
    Private WithEvents TextBox43 As System.Windows.Forms.TextBox
    Private WithEvents Button40 As System.Windows.Forms.Button
    Private WithEvents Button41 As System.Windows.Forms.Button
    Private WithEvents TextBox44 As System.Windows.Forms.TextBox
    Private WithEvents Button42 As System.Windows.Forms.Button
    Private WithEvents Button43 As System.Windows.Forms.Button
    Private WithEvents TextBox45 As System.Windows.Forms.TextBox
    Private WithEvents Button44 As System.Windows.Forms.Button
    Private WithEvents Button45 As System.Windows.Forms.Button
    Private WithEvents TextBox46 As System.Windows.Forms.TextBox
    Public WithEvents CheckBox131 As System.Windows.Forms.CheckBox
    Private WithEvents Label96 As System.Windows.Forms.Label
    Private WithEvents Label97 As System.Windows.Forms.Label
    Private WithEvents Label98 As System.Windows.Forms.Label
    Private WithEvents Label99 As System.Windows.Forms.Label
    Private WithEvents Label100 As System.Windows.Forms.Label
    Private WithEvents Label101 As System.Windows.Forms.Label
    Private WithEvents TabPage8 As System.Windows.Forms.TabPage
    Public WithEvents UctDocumentLink2 As uctPMUDocumentLink.uctDocumentLink
    Private WithEvents TabPage9 As System.Windows.Forms.TabPage
    Public WithEvents UctDocumentLink3 As uctPMUDocumentLink.uctDocumentLink
    Public WithEvents UctDocumentLink4 As uctPMUDocumentLink.uctDocumentLink
    Private WithEvents TabPage10 As System.Windows.Forms.TabPage
    Public WithEvents UctSIRSelectClauses1 As uctSCControl.uctSIRSelectClauses
    Private WithEvents _tabMainTab_TabPage10 As System.Windows.Forms.TabPage
    Public WithEvents GroupBox40 As System.Windows.Forms.GroupBox
    Public WithEvents PickList4 As uctPickList.PickList
    Public WithEvents GroupBox42 As System.Windows.Forms.GroupBox
    Public WithEvents PickList6 As uctPickList.PickList
    Public WithEvents GroupBox41 As System.Windows.Forms.GroupBox
    Public WithEvents PickList5 As uctPickList.PickList
    Public WithEvents chkEnablePrePayment As System.Windows.Forms.CheckBox
    Friend WithEvents Label102 As System.Windows.Forms.Label
    Public WithEvents cboApplyMandatoryRisk As PMLookupControl.cboPMLookup
    Friend WithEvents lblApplyMandatoryRisk As System.Windows.Forms.Label
    Public WithEvents chkDoNotDeleteRenewalQuoteOnMTA As System.Windows.Forms.CheckBox
    Public WithEvents chkDeleteRenewalQuoteReRunOnMTA As System.Windows.Forms.CheckBox
    Public WithEvents chkDefaultCovertoDatetolastday As System.Windows.Forms.CheckBox
    Friend WithEvents chkUnifiedRenewalDateIsReadOnly As System.Windows.Forms.CheckBox
    Friend WithEvents lblUnifiedRenewalDateIsReadOnly As System.Windows.Forms.Label
    Public WithEvents grpClaimScripts As System.Windows.Forms.GroupBox
    Public WithEvents chkIsPaymentsReadonly As System.Windows.Forms.CheckBox
    Public WithEvents chkIsReservesReadonly As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRecoveriesReadonly As System.Windows.Forms.CheckBox
    Friend WithEvents fraRateMultipleRisks As System.Windows.Forms.GroupBox
    Private WithEvents chkDisplayRerateForCancellationsAndReinstatments As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayRerateForMTA As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayRerateForQuoteAndNB As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayRerateForRenewal As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoRenBDMonthlyPol As CheckBox
    Friend WithEvents lblAutoRenBDMonthlyPolicy As Label
    Public WithEvents chkRetainPolicyNumber As CheckBox
    Friend WithEvents chkEditAnnivDate As CheckBox
    Friend WithEvents lblAnnivDateEditableMonthlyPolicy As Label
    Public WithEvents chkDisableCoverStartDateonREN As CheckBox
    Friend WithEvents rbEffectiveDate As RadioButton
    Friend WithEvents rbPolicyInceptionDate As RadioButton
    Friend WithEvents txtAuthorisationThreshold As System.Windows.Forms.TextBox
    Public WithEvents lblAuthorisationThreshold As System.Windows.Forms.Label
    Friend WithEvents Frame10 As GroupBox
    Friend WithEvents Label103 As Label
    Friend WithEvents chkQuoteVersioning As CheckBox
    Friend WithEvents txtDeleteQuoteAfter As TextBox
#End Region
End Class