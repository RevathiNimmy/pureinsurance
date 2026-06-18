<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUserMaintenance
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializepnlAgent()
        InitializepnlAccHandler()
        InitializelblAgentYN()
        InitializelblAccHandlerYN()
        InitializecmdPrevious()
        InitializecmdNext()
        InitializecmdAgent()
        InitializecmdAccHandler()
        InitializechkAgent()
        InitializechkAccHandler()
        tabMainPreviousTab = tabMain.SelectedIndex
        Form_Initialize_Renamed()
    End Sub
    Private Sub Ctx_mnuSupervisor_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSupervisor.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuSupervisor.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuSupervisor.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuSupervisor.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuSupervisor_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSupervisor.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSupervisor.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuSupervisor.DropDownItems.Add(item)
        Next item
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public cmdlgSignatureOpen As System.Windows.Forms.OpenFileDialog
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    Private WithEvents _lvwUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwUsers As System.Windows.Forms.ListView
    Public WithEvents chkSiriusUser As System.Windows.Forms.CheckBox
    Public WithEvents txtPercentHoursWorked As System.Windows.Forms.TextBox
    Public WithEvents cboJobBasis As System.Windows.Forms.ComboBox
    Public WithEvents chkPrinterChangable As System.Windows.Forms.CheckBox
    Public WithEvents cboPrinter As System.Windows.Forms.ComboBox
    Public WithEvents lblPrinterName As System.Windows.Forms.Label
    Public WithEvents lblPrinterYN As System.Windows.Forms.Label
    Public WithEvents fraPrinter As System.Windows.Forms.GroupBox
    Public WithEvents txtInitials As System.Windows.Forms.TextBox
    Public WithEvents txtFullName As System.Windows.Forms.TextBox
    Public WithEvents txtEmailAddress As System.Windows.Forms.TextBox
    Public WithEvents txtTelephoneNumber As System.Windows.Forms.TextBox
    Public WithEvents txtMobileNumber As System.Windows.Forms.TextBox
    Public WithEvents txtExtensionNumber As System.Windows.Forms.TextBox
    Public WithEvents txtFaxNumber As System.Windows.Forms.TextBox
    Public WithEvents cboJobTitle As System.Windows.Forms.ComboBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents pnlEffectiveDate As System.Windows.Forms.Panel
    Public WithEvents pnlUsername As System.Windows.Forms.Panel
    Public WithEvents pnlLastLogin As System.Windows.Forms.Panel
    Public WithEvents ddTitle As PMListMgrDropdown.uctDropdown
    Public WithEvents lblSiriusUser As System.Windows.Forms.Label
    Public WithEvents lblPercentageHoursWorked As System.Windows.Forms.Label
    Public WithEvents lblJobBasis As System.Windows.Forms.Label
    Public WithEvents lblEmailAddress As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents lblLastLogin As System.Windows.Forms.Label
    Public WithEvents Frame0 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Public WithEvents cmdSignature As System.Windows.Forms.Button
    Public WithEvents chkOtherParty As System.Windows.Forms.CheckBox
    Public WithEvents cmdOtherParty As System.Windows.Forms.Button
    Private WithEvents _cmdAgent_1 As System.Windows.Forms.Button
    Private WithEvents _chkAgent_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkAgent_0 As System.Windows.Forms.CheckBox
    Private WithEvents _cmdAgent_0 As System.Windows.Forms.Button
    Private WithEvents _cmdAccHandler_0 As System.Windows.Forms.Button
    Private WithEvents _cmdAccHandler_1 As System.Windows.Forms.Button
    Private WithEvents _cmdAccHandler_2 As System.Windows.Forms.Button
    Private WithEvents _chkAccHandler_0 As System.Windows.Forms.CheckBox
    Private WithEvents _chkAccHandler_1 As System.Windows.Forms.CheckBox
    Private WithEvents _chkAccHandler_2 As System.Windows.Forms.CheckBox
    Private WithEvents _pnlAccHandler_0 As System.Windows.Forms.Panel
    Private WithEvents _pnlAccHandler_1 As System.Windows.Forms.Panel
    Private WithEvents _pnlAccHandler_2 As System.Windows.Forms.Panel
    Private WithEvents _pnlAgent_0 As System.Windows.Forms.Panel
    Private WithEvents _pnlAgent_1 As System.Windows.Forms.Panel
    Public WithEvents pnlOtherParty As System.Windows.Forms.Panel
    Public WithEvents lblOtherPartyYN As System.Windows.Forms.Label
    Private WithEvents _lblAgentYN_1 As System.Windows.Forms.Label
    Private WithEvents _lblAgentYN_0 As System.Windows.Forms.Label
    Private WithEvents _lblAccHandlerYN_0 As System.Windows.Forms.Label
    Private WithEvents _lblAccHandlerYN_1 As System.Windows.Forms.Label
    Private WithEvents _lblAccHandlerYN_2 As System.Windows.Forms.Label
    Public WithEvents Frame7 As System.Windows.Forms.GroupBox
    Public WithEvents cmdChangePassword As System.Windows.Forms.Button
    Public WithEvents pnlPasswordChange As System.Windows.Forms.Panel
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents cmdChangeDomainAccount As System.Windows.Forms.Button
    Public WithEvents pnlDomainUserName As System.Windows.Forms.Panel
    Public WithEvents Label15 As System.Windows.Forms.Label
    Public WithEvents fraDomainAccount As System.Windows.Forms.GroupBox
    Public WithEvents imgSignature As System.Windows.Forms.PictureBox
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Public WithEvents cmdAddBranch As System.Windows.Forms.Button
    Public WithEvents cmdDelAllBranches As System.Windows.Forms.Button
    Public WithEvents cmdDelBranch As System.Windows.Forms.Button
    Public WithEvents cmdAddAllBranches As System.Windows.Forms.Button
    Private WithEvents _lvwBranches_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwBranches As System.Windows.Forms.ListView
    Private WithEvents _lvwSelectedBranches_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSelectedBranches As System.Windows.Forms.ListView
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_3 As System.Windows.Forms.Button
    Public WithEvents cmdAddGroup As System.Windows.Forms.Button
    Public WithEvents cmdDelAllGroups As System.Windows.Forms.Button
    Public WithEvents cmdDelGroup As System.Windows.Forms.Button
    Public WithEvents cmdAddAllGroups As System.Windows.Forms.Button
    Private WithEvents _lvwSelectedGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSelectedGroups As System.Windows.Forms.ListView
    Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwGroups As System.Windows.Forms.ListView
    Public WithEvents imgGroup As System.Windows.Forms.ImageList
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents Frame4 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage3 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_3 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_4 As System.Windows.Forms.Button
    Public WithEvents txtTimePeriod As System.Windows.Forms.TextBox
    Public WithEvents chkReverseAllocation As System.Windows.Forms.CheckBox
    Public WithEvents lblTimePeriod As System.Windows.Forms.Label
    Public WithEvents fmeAllocationReversal As System.Windows.Forms.GroupBox
    Public WithEvents chkWriteOffs As System.Windows.Forms.CheckBox
    Public WithEvents txtWriteOff As System.Windows.Forms.TextBox
    Public WithEvents cboWriteOffsCurrency As UserControls.CurrencyLookup
    Public WithEvents lblWriteoffAmount As System.Windows.Forms.Label
    Public WithEvents lblWriteoffText As System.Windows.Forms.Label
    Public WithEvents lblWriteOffsCurrency As System.Windows.Forms.Label
    Public WithEvents fmeWriteOffs As System.Windows.Forms.GroupBox
    Public WithEvents chkOverrideCollectionDate As System.Windows.Forms.CheckBox
    Public WithEvents chkOverrideChequeNumber As System.Windows.Forms.CheckBox
    Public WithEvents chkPostingPeriod As System.Windows.Forms.CheckBox
    Public WithEvents chkDuplicateClaimOverride As System.Windows.Forms.CheckBox
    Public WithEvents chkOverridePrePolicyDate As System.Windows.Forms.CheckBox
    Public WithEvents chkOverridePrePolicyRate As System.Windows.Forms.CheckBox
    Public WithEvents chkOverrideDate As System.Windows.Forms.CheckBox
    Public WithEvents chkOverrideRate As System.Windows.Forms.CheckBox
    Public WithEvents fmeOverride As System.Windows.Forms.GroupBox
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab2_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents txtRecommendAmount As System.Windows.Forms.TextBox
    Public WithEvents chkRecommender As System.Windows.Forms.CheckBox
    Public WithEvents cboRecommandationCurrency As UserControls.CurrencyLookup
    Public WithEvents lblRecommenderAmount As System.Windows.Forms.Label
    Public WithEvents lblRecommenderCurrency As System.Windows.Forms.Label
    Public WithEvents Frame10 As System.Windows.Forms.GroupBox
    Public WithEvents chkPayments As System.Windows.Forms.CheckBox
    Public WithEvents txtPayments As System.Windows.Forms.TextBox
    Public WithEvents cboPaymentsCurrency As UserControls.CurrencyLookup
    Public WithEvents lblPaymentsCurrency As System.Windows.Forms.Label
    Public WithEvents lblPayments As System.Windows.Forms.Label
    Public WithEvents lblPaymentsText As System.Windows.Forms.Label
    Public WithEvents fmePayments As System.Windows.Forms.GroupBox
    Public WithEvents chkClaimPayments As System.Windows.Forms.CheckBox
    Public WithEvents txtClaimPayments As System.Windows.Forms.TextBox
    Public WithEvents chkUserCanChangeReserves As System.Windows.Forms.CheckBox
    Public WithEvents cboClaimPaymentsCurrency As UserControls.CurrencyLookup
    Public WithEvents lblClaimPaymentsCurrency As System.Windows.Forms.Label
    Public WithEvents lblClaimPayments As System.Windows.Forms.Label
    Public WithEvents fmeClaimPayments As System.Windows.Forms.GroupBox
    Public WithEvents Frame8 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab2_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents chkDisplayClaimReinsurance As System.Windows.Forms.CheckBox
    Public WithEvents chkDisplayReinsuranceScreen As System.Windows.Forms.CheckBox
    Public WithEvents fraReinsurance As System.Windows.Forms.GroupBox
    Public WithEvents chkEditRatingSections As System.Windows.Forms.CheckBox
    Public WithEvents chkAddRemoveRatingSections As System.Windows.Forms.CheckBox
    Public WithEvents fraRatingSections As System.Windows.Forms.GroupBox
    Public WithEvents chkEditDefaultCommission As System.Windows.Forms.CheckBox
    Public WithEvents chkCashDeposit As System.Windows.Forms.CheckBox
    Public WithEvents chkBankGuarantee As System.Windows.Forms.CheckBox
    Public WithEvents cboMTAAuthority As System.Windows.Forms.ComboBox
    Public WithEvents txtPaynowWriteOffAmount As System.Windows.Forms.TextBox
    Public WithEvents chkInvoice As System.Windows.Forms.CheckBox
    Public WithEvents chkPayNow As System.Windows.Forms.CheckBox
    Public WithEvents chkInstalments As System.Windows.Forms.CheckBox
    Public WithEvents chkHasPaynowWriteOffAuthority As System.Windows.Forms.CheckBox
    Public WithEvents cboMakeLiveCurrency As UserControls.CurrencyLookup
    Public WithEvents Label16 As System.Windows.Forms.Label
    Public WithEvents LblPaynowAmount As System.Windows.Forms.Label
    Public WithEvents lblPaynowCurrency As System.Windows.Forms.Label
    Public WithEvents fraMakeLive As System.Windows.Forms.GroupBox
    Public WithEvents chkUnrestrictedUpdate As System.Windows.Forms.CheckBox
    Public WithEvents chkUnrestrictedEnquiry As System.Windows.Forms.CheckBox
    Public WithEvents lblUnrestrictedUpdate As System.Windows.Forms.Label
    Public WithEvents lblUnrestrictedEnquiry As System.Windows.Forms.Label
    Public WithEvents fmeAccess As System.Windows.Forms.GroupBox
    Public WithEvents chkCanPerformBrokerTransfer As System.Windows.Forms.CheckBox
    Public WithEvents fraBrokerAgentPortfolioTransfer As System.Windows.Forms.GroupBox
    Public WithEvents cboVoidTransaction As System.Windows.Forms.ComboBox
    Public WithEvents lblVoidPolicyVersion As System.Windows.Forms.Label
    Public WithEvents fraVoidTransaction As System.Windows.Forms.GroupBox
    Public WithEvents Frame9 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab2_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents chkUserServerScriptsRunInDebug As System.Windows.Forms.CheckBox
    Public WithEvents chkUserCanDebugDynamicLogicScripts As System.Windows.Forms.CheckBox
    Public WithEvents Frame11 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab2_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents SSTab2 As System.Windows.Forms.TabControl
    Public WithEvents fmeAuthorities As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage4 As System.Windows.Forms.TabPage
    Private WithEvents _cmdNext_5 As System.Windows.Forms.Button
    Private WithEvents _cmdPrevious_4 As System.Windows.Forms.Button
    Public WithEvents cmdEditRiskDetails As System.Windows.Forms.Button
    Private WithEvents _lvwRiskGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskGroups_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskGroups_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskGroups_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRiskGroups As System.Windows.Forms.ListView
    Public WithEvents Frame6 As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents chkCanReverseReplaceTransactions As System.Windows.Forms.CheckBox
    Public WithEvents chkIsReverseTransactions As System.Windows.Forms.CheckBox
    Private WithEvents _cmdNext_6 As System.Windows.Forms.Button
    Public WithEvents chkIsEditSchemePolicy As System.Windows.Forms.CheckBox
    Public WithEvents chkIsDeletePolicy As System.Windows.Forms.CheckBox
    Public WithEvents chkIsViewClient As System.Windows.Forms.CheckBox
    Public WithEvents chkIsEditClient As System.Windows.Forms.CheckBox
    Public WithEvents chkIsEditPolicy As System.Windows.Forms.CheckBox
    Public WithEvents chkIsViewClaim As System.Windows.Forms.CheckBox
    Public WithEvents chkIsEditFinancePlan As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRaiseDebit As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRaiseCredit As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRaiseCash As System.Windows.Forms.CheckBox
    Public WithEvents chkIsReverseAllocations As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRaiseFee As System.Windows.Forms.CheckBox
    Public WithEvents chkIsRaiseManualDID As System.Windows.Forms.CheckBox
    Public WithEvents chkIsDeleteClient As System.Windows.Forms.CheckBox
    Public WithEvents chkIsPerformAllocations As System.Windows.Forms.CheckBox
    Private WithEvents _cmdPrevious_5 As System.Windows.Forms.Button
    Public WithEvents lblReverseReplaceTransactions As System.Windows.Forms.Label
    Public WithEvents lblEditSchemePolicy As System.Windows.Forms.Label
    Public WithEvents lblDeletePolicy As System.Windows.Forms.Label
    Public WithEvents lblViewClient As System.Windows.Forms.Label
    Public WithEvents lblEditClient As System.Windows.Forms.Label
    Public WithEvents lblEditPolicy As System.Windows.Forms.Label
    Public WithEvents lblViewClaim As System.Windows.Forms.Label
    Public WithEvents lblEditFinancePlan As System.Windows.Forms.Label
    Public WithEvents lblRaiseDebit As System.Windows.Forms.Label
    Public WithEvents lblRaiseCredit As System.Windows.Forms.Label
    Public WithEvents lblRaiseFee As System.Windows.Forms.Label
    Public WithEvents lblRaiseCash As System.Windows.Forms.Label
    Public WithEvents lblReverseTransactions As System.Windows.Forms.Label
    Public WithEvents lblReverseAllocations As System.Windows.Forms.Label
    Public WithEvents lblRaiseManualDID As System.Windows.Forms.Label
    Public WithEvents lblDeleteClient As System.Windows.Forms.Label
    Public WithEvents lblPerformAllocations As System.Windows.Forms.Label
    Public WithEvents frmClientManagerSecurity As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage6 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_6 As System.Windows.Forms.Button
    Public WithEvents chkOtherPartyMaintenance As System.Windows.Forms.CheckBox
    Public WithEvents chkInsurerMaintenance As System.Windows.Forms.CheckBox
    Public WithEvents chkAccountExecutive As System.Windows.Forms.CheckBox
    Public WithEvents chkAccountHandler As System.Windows.Forms.CheckBox
    Public WithEvents chkAgentMaintenance As System.Windows.Forms.CheckBox
    Public WithEvents chkIsViewClientManager As System.Windows.Forms.CheckBox
    Public WithEvents lblIsViewOnly As System.Windows.Forms.Label
    Public WithEvents lblPartyCaption As System.Windows.Forms.Label
    Public WithEvents fmePartyEdit As System.Windows.Forms.GroupBox
    Private WithEvents _SSTab1_TabPage7 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents txtFilter As System.Windows.Forms.TextBox
    Public WithEvents cmdAddUser As System.Windows.Forms.Button
    Public WithEvents cmdDeleteUser As System.Windows.Forms.Button
    Public WithEvents chkHideDeleted As System.Windows.Forms.CheckBox
    Public WithEvents Combo1 As System.Windows.Forms.ComboBox
    Public WithEvents cmdUnmatch As System.Windows.Forms.Button
    Private WithEvents _lvwMatchedUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwMatchedUsers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwMatchedUsers As System.Windows.Forms.ListView
    Public WithEvents fraMatched As System.Windows.Forms.GroupBox
    Public WithEvents cboDomain As System.Windows.Forms.ComboBox
    Public WithEvents cmdMatch As System.Windows.Forms.Button
    Private WithEvents _lvwSiriusUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSiriusUsers_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSiriusUsers As System.Windows.Forms.ListView
    Private WithEvents _lvwDomainUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwDomainUsers As System.Windows.Forms.ListView
    Public WithEvents txtLDAPDomain As System.Windows.Forms.TextBox
    Public WithEvents lblUnmatchedDomain As System.Windows.Forms.Label
    Public WithEvents lblUnmatchedUsers As System.Windows.Forms.Label
    Public WithEvents fraUnmatched As System.Windows.Forms.GroupBox
    Public WithEvents cboSystemSecurity As System.Windows.Forms.ComboBox
    Public WithEvents fraSystemSecurity As System.Windows.Forms.GroupBox
    Private WithEvents _tabMain_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents tabMain As System.Windows.Forms.TabControl
    Public chkAccHandler(2) As System.Windows.Forms.CheckBox
    Public chkAgent(1) As System.Windows.Forms.CheckBox
    Public cmdAccHandler(2) As System.Windows.Forms.Button
    Public cmdAgent(1) As System.Windows.Forms.Button
    Public cmdNext(6) As System.Windows.Forms.Button
    Public cmdPrevious(6) As System.Windows.Forms.Button
    Public lblAccHandlerYN(2) As System.Windows.Forms.Label
    Public lblAgentYN(1) As System.Windows.Forms.Label
    Public pnlAccHandler(2) As System.Windows.Forms.Panel
    Public pnlAgent(1) As System.Windows.Forms.Panel
    Public WithEvents Ctx_mnuSupervisor As System.Windows.Forms.ContextMenuStrip
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserMaintenance))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtInitials = New System.Windows.Forms.TextBox()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.txtEmailAddress = New System.Windows.Forms.TextBox()
        Me.txtTelephoneNumber = New System.Windows.Forms.TextBox()
        Me.txtMobileNumber = New System.Windows.Forms.TextBox()
        Me.txtExtensionNumber = New System.Windows.Forms.TextBox()
        Me.txtFaxNumber = New System.Windows.Forms.TextBox()
        Me.pnlEffectiveDate = New System.Windows.Forms.Panel()
        Me.lblEffectiveDatePanel = New System.Windows.Forms.Label()
        Me.pnlUsername = New System.Windows.Forms.Panel()
        Me.lblUsernamePanel = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.pnlLastLogin = New System.Windows.Forms.Panel()
        Me.lblLastLoginPanel = New System.Windows.Forms.Label()
        Me.pnlPasswordChange = New System.Windows.Forms.Panel()
        Me.lblPasswordChangePanel = New System.Windows.Forms.Label()
        Me.cmdEditRiskDetails = New System.Windows.Forms.Button()
        Me.cmdAddUser = New System.Windows.Forms.Button()
        Me.cmdDeleteUser = New System.Windows.Forms.Button()
        Me.cmdUnmatch = New System.Windows.Forms.Button()
        Me.cmdMatch = New System.Windows.Forms.Button()
        Me.cmdlgSignatureOpen = New System.Windows.Forms.OpenFileDialog()
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lvwUsers = New System.Windows.Forms.ListView()
        Me._lvwUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl()
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.Frame0 = New System.Windows.Forms.GroupBox()
        Me.lblSSOPreferredName = New System.Windows.Forms.Label()
        Me.txtSSOPreferredName = New System.Windows.Forms.TextBox()
        Me.chkSiriusUser = New System.Windows.Forms.CheckBox()
        Me.txtPercentHoursWorked = New System.Windows.Forms.TextBox()
        Me.cboJobBasis = New System.Windows.Forms.ComboBox()
        Me.fraPrinter = New System.Windows.Forms.GroupBox()
        Me.chkPrinterChangable = New System.Windows.Forms.CheckBox()
        Me.cboPrinter = New System.Windows.Forms.ComboBox()
        Me.lblPrinterName = New System.Windows.Forms.Label()
        Me.lblPrinterYN = New System.Windows.Forms.Label()
        Me.cboJobTitle = New System.Windows.Forms.ComboBox()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me.ddTitle = New PMListMgrDropdown.uctDropdown()
        Me.lblSiriusUser = New System.Windows.Forms.Label()
        Me.lblPercentageHoursWorked = New System.Windows.Forms.Label()
        Me.lblJobBasis = New System.Windows.Forms.Label()
        Me.lblEmailAddress = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblLastLogin = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me.cmdSignature = New System.Windows.Forms.Button()
        Me.Frame7 = New System.Windows.Forms.GroupBox()
        Me.chkOtherParty = New System.Windows.Forms.CheckBox()
        Me.cmdOtherParty = New System.Windows.Forms.Button()
        Me._cmdAgent_1 = New System.Windows.Forms.Button()
        Me._chkAgent_1 = New System.Windows.Forms.CheckBox()
        Me._chkAgent_0 = New System.Windows.Forms.CheckBox()
        Me._cmdAgent_0 = New System.Windows.Forms.Button()
        Me._cmdAccHandler_0 = New System.Windows.Forms.Button()
        Me._cmdAccHandler_1 = New System.Windows.Forms.Button()
        Me._cmdAccHandler_2 = New System.Windows.Forms.Button()
        Me._chkAccHandler_0 = New System.Windows.Forms.CheckBox()
        Me._chkAccHandler_1 = New System.Windows.Forms.CheckBox()
        Me._chkAccHandler_2 = New System.Windows.Forms.CheckBox()
        Me._pnlAccHandler_0 = New System.Windows.Forms.Panel()
        Me.lblAccHandlerPanel = New System.Windows.Forms.Label()
        Me._pnlAccHandler_1 = New System.Windows.Forms.Panel()
        Me.lblAccExecutivePanel = New System.Windows.Forms.Label()
        Me._pnlAccHandler_2 = New System.Windows.Forms.Panel()
        Me.lblClaimsHandlerPanel = New System.Windows.Forms.Label()
        Me._pnlAgent_0 = New System.Windows.Forms.Panel()
        Me.lblAgentPanel = New System.Windows.Forms.Label()
        Me._pnlAgent_1 = New System.Windows.Forms.Panel()
        Me.lblInsurerPanel = New System.Windows.Forms.Label()
        Me.pnlOtherParty = New System.Windows.Forms.Panel()
        Me.lblOtherPartypanel = New System.Windows.Forms.Label()
        Me.lblOtherPartyYN = New System.Windows.Forms.Label()
        Me._lblAgentYN_1 = New System.Windows.Forms.Label()
        Me._lblAgentYN_0 = New System.Windows.Forms.Label()
        Me._lblAccHandlerYN_0 = New System.Windows.Forms.Label()
        Me._lblAccHandlerYN_1 = New System.Windows.Forms.Label()
        Me._lblAccHandlerYN_2 = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.cmdChangePassword = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.fraDomainAccount = New System.Windows.Forms.GroupBox()
        Me.cmdChangeDomainAccount = New System.Windows.Forms.Button()
        Me.pnlDomainUserName = New System.Windows.Forms.Panel()
        Me.lblDomainUserNamePanel = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.imgSignature = New System.Windows.Forms.PictureBox()
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me.cmdAddBranch = New System.Windows.Forms.Button()
        Me.cmdDelAllBranches = New System.Windows.Forms.Button()
        Me.cmdDelBranch = New System.Windows.Forms.Button()
        Me.cmdAddAllBranches = New System.Windows.Forms.Button()
        Me.lvwBranches = New System.Windows.Forms.ListView()
        Me._lvwBranches_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwSelectedBranches = New System.Windows.Forms.ListView()
        Me._lvwSelectedBranches_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage3 = New System.Windows.Forms.TabPage()
        Me.Frame4 = New System.Windows.Forms.GroupBox()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me._cmdNext_3 = New System.Windows.Forms.Button()
        Me.cmdAddGroup = New System.Windows.Forms.Button()
        Me.cmdDelAllGroups = New System.Windows.Forms.Button()
        Me.cmdDelGroup = New System.Windows.Forms.Button()
        Me.cmdAddAllGroups = New System.Windows.Forms.Button()
        Me.lvwSelectedGroups = New System.Windows.Forms.ListView()
        Me._lvwSelectedGroups_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwGroups = New System.Windows.Forms.ListView()
        Me._lvwGroups_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fmeAuthorities = New System.Windows.Forms.GroupBox()
        Me.SSTab2 = New System.Windows.Forms.TabControl()
        Me._SSTab2_TabPage0 = New System.Windows.Forms.TabPage()
        Me.Frame5 = New System.Windows.Forms.GroupBox()
        Me.fmeInstalmentDetail = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtEditInstalmentByNoofDays = New System.Windows.Forms.TextBox()
        Me.chkEditInstalment = New System.Windows.Forms.CheckBox()
        Me.fmeReceiptreversal = New System.Windows.Forms.GroupBox()
        Me.chkReceiptReversal = New System.Windows.Forms.CheckBox()
        Me.fmeAllocationReversal = New System.Windows.Forms.GroupBox()
        Me.txtTimePeriod = New System.Windows.Forms.TextBox()
        Me.chkReverseAllocation = New System.Windows.Forms.CheckBox()
        Me.lblTimePeriod = New System.Windows.Forms.Label()
        Me.fmeWriteOffs = New System.Windows.Forms.GroupBox()
        Me.cboLossGainCurrency = New UserControls.CurrencyLookup()
        Me.lblLossGainCurrency = New System.Windows.Forms.Label()
        Me.txtCurrencyLossGainLimit = New System.Windows.Forms.TextBox()
        Me.lblCurrencyAmount = New System.Windows.Forms.Label()
        Me.lblCurrencyWriteoff = New System.Windows.Forms.Label()
        Me.chkWriteOffs = New System.Windows.Forms.CheckBox()
        Me.txtWriteOff = New System.Windows.Forms.TextBox()
        Me.cboWriteOffsCurrency = New UserControls.CurrencyLookup()
        Me.lblWriteoffAmount = New System.Windows.Forms.Label()
        Me.lblWriteoffText = New System.Windows.Forms.Label()
        Me.lblWriteOffsCurrency = New System.Windows.Forms.Label()
        Me.fmeOverride = New System.Windows.Forms.GroupBox()
        Me.chkInstalmentStatus = New System.Windows.Forms.CheckBox()
        Me.chkOverrideCollectionDate = New System.Windows.Forms.CheckBox()
        Me.chkOverrideChequeNumber = New System.Windows.Forms.CheckBox()
        Me.chkPostingPeriod = New System.Windows.Forms.CheckBox()
        Me.chkDuplicateClaimOverride = New System.Windows.Forms.CheckBox()
        Me.chkOverridePrePolicyDate = New System.Windows.Forms.CheckBox()
        Me.chkOverridePrePolicyRate = New System.Windows.Forms.CheckBox()
        Me.chkOverrideDate = New System.Windows.Forms.CheckBox()
        Me.chkOverrideRate = New System.Windows.Forms.CheckBox()
        Me._SSTab2_TabPage1 = New System.Windows.Forms.TabPage()
        Me.Frame8 = New System.Windows.Forms.GroupBox()
        Me.Frame10 = New System.Windows.Forms.GroupBox()
        Me.txtRecommendAmount = New System.Windows.Forms.TextBox()
        Me.chkRecommender = New System.Windows.Forms.CheckBox()
        Me.cboRecommandationCurrency = New UserControls.CurrencyLookup()
        Me.lblRecommenderAmount = New System.Windows.Forms.Label()
        Me.lblRecommenderCurrency = New System.Windows.Forms.Label()
        Me.fmePayments = New System.Windows.Forms.GroupBox()
        Me.chkPayments = New System.Windows.Forms.CheckBox()
        Me.txtPayments = New System.Windows.Forms.TextBox()
        Me.cboPaymentsCurrency = New UserControls.CurrencyLookup()
        Me.lblPaymentsCurrency = New System.Windows.Forms.Label()
        Me.lblPayments = New System.Windows.Forms.Label()
        Me.lblPaymentsText = New System.Windows.Forms.Label()
        Me.fmeClaimPayments = New System.Windows.Forms.GroupBox()
        Me.chkClaimPayments = New System.Windows.Forms.CheckBox()
        Me.txtClaimPayments = New System.Windows.Forms.TextBox()
        Me.chkUserCanChangeReserves = New System.Windows.Forms.CheckBox()
        Me.cboClaimPaymentsCurrency = New UserControls.CurrencyLookup()
        Me.lblClaimPaymentsCurrency = New System.Windows.Forms.Label()
        Me.lblClaimPayments = New System.Windows.Forms.Label()
        Me._sstab2_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fmeManualjournal = New System.Windows.Forms.GroupBox()
        Me.ChkManualJournal = New System.Windows.Forms.CheckBox()
        Me.txtjournalAmount = New System.Windows.Forms.TextBox()
        Me.cboJournalCurrency = New UserControls.CurrencyLookup()
        Me.lblJournalCurrency = New System.Windows.Forms.Label()
        Me.lblJournalAmount = New System.Windows.Forms.Label()
        Me.lblmanulJournal = New System.Windows.Forms.Label()
        Me._SSTab2_TabPage2 = New System.Windows.Forms.TabPage()
        Me.Frame9 = New System.Windows.Forms.GroupBox()
        Me.fraReinsurance = New System.Windows.Forms.GroupBox()
        Me.chkDisplayClaimReinsurance = New System.Windows.Forms.CheckBox()
        Me.chkDisplayReinsuranceScreen = New System.Windows.Forms.CheckBox()
        Me.fraRatingSections = New System.Windows.Forms.GroupBox()
        Me.chkEditRatingSections = New System.Windows.Forms.CheckBox()
        Me.chkAddRemoveRatingSections = New System.Windows.Forms.CheckBox()
        Me.fraMakeLive = New System.Windows.Forms.GroupBox()
        Me.fraCommSettings = New System.Windows.Forms.GroupBox()
        Me.chkEditDefaultCommissionMTR = New System.Windows.Forms.CheckBox()
        Me.chkEditDefaultCommissionMTC = New System.Windows.Forms.CheckBox()
        Me.chkEditDefaultCommissionMTA = New System.Windows.Forms.CheckBox()
        Me.chkEditDefaultCommissionNBRN = New System.Windows.Forms.CheckBox()
        Me.chkEditDefaultCommission = New System.Windows.Forms.CheckBox()
        Me.chkCanChangeInstalmentPlanDefaultCurrecny = New System.Windows.Forms.CheckBox()
        Me.chkEditAgentDuringMTAMTC = New System.Windows.Forms.CheckBox()
        Me.chkCashDeposit = New System.Windows.Forms.CheckBox()
        Me.chkBankGuarantee = New System.Windows.Forms.CheckBox()
        Me.cboMTAAuthority = New System.Windows.Forms.ComboBox()
        Me.txtPaynowWriteOffAmount = New System.Windows.Forms.TextBox()
        Me.chkInvoice = New System.Windows.Forms.CheckBox()
        Me.chkPayNow = New System.Windows.Forms.CheckBox()
        Me.chkInstalments = New System.Windows.Forms.CheckBox()
        Me.chkHasPaynowWriteOffAuthority = New System.Windows.Forms.CheckBox()
        Me.cboMakeLiveCurrency = New UserControls.CurrencyLookup()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.LblPaynowAmount = New System.Windows.Forms.Label()
        Me.lblPaynowCurrency = New System.Windows.Forms.Label()
        Me.fmeAccess = New System.Windows.Forms.GroupBox()
        Me.chkCanExtractClientData = New System.Windows.Forms.CheckBox()
        Me.lblCanExtractClientData = New System.Windows.Forms.Label()
        
        Me.chkViewBatchProcessStatus = New System.Windows.Forms.CheckBox()
        Me.lblViewBatchProcessStatus = New System.Windows.Forms.Label()
        Me.chkUnrestrictedUpdate = New System.Windows.Forms.CheckBox()
        Me.chkUnrestrictedEnquiry = New System.Windows.Forms.CheckBox()
        Me.lblUnrestrictedUpdate = New System.Windows.Forms.Label()
        Me.lblUnrestrictedEnquiry = New System.Windows.Forms.Label()
        Me.fraBrokerAgentPortfolioTransfer = New System.Windows.Forms.GroupBox()
        Me.chkCanPerformBrokerTransfer = New System.Windows.Forms.CheckBox()
        Me.fraVoidTransaction = New System.Windows.Forms.GroupBox()
        Me.cboVoidTransaction = New System.Windows.Forms.ComboBox()
        Me.lblVoidPolicyVersion = New System.Windows.Forms.Label()

        Me._SSTab2_TabPage3 = New System.Windows.Forms.TabPage()
        Me.Frame11 = New System.Windows.Forms.GroupBox()
        Me.chkUserServerScriptsRunInDebug = New System.Windows.Forms.CheckBox()
        Me.chkUserCanDebugDynamicLogicScripts = New System.Windows.Forms.CheckBox()
        Me._cmdPrevious_3 = New System.Windows.Forms.Button()
        Me._cmdNext_4 = New System.Windows.Forms.Button()
        Me._SSTab1_TabPage5 = New System.Windows.Forms.TabPage()
        Me.Frame6 = New System.Windows.Forms.GroupBox()
        Me._cmdNext_5 = New System.Windows.Forms.Button()
        Me._cmdPrevious_4 = New System.Windows.Forms.Button()
        Me.lvwRiskGroups = New System.Windows.Forms.ListView()
        Me._lvwRiskGroups_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskGroups_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskGroups_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskGroups_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._SSTab1_TabPage6 = New System.Windows.Forms.TabPage()
        Me.frmClientManagerSecurity = New System.Windows.Forms.GroupBox()
        Me.chkCanReverseReplaceTransactions = New System.Windows.Forms.CheckBox()
        Me.chkIsReverseTransactions = New System.Windows.Forms.CheckBox()
        Me._cmdNext_6 = New System.Windows.Forms.Button()
        Me.chkIsEditSchemePolicy = New System.Windows.Forms.CheckBox()
        Me.chkIsDeletePolicy = New System.Windows.Forms.CheckBox()
        Me.chkIsViewClient = New System.Windows.Forms.CheckBox()
        Me.chkIsEditClient = New System.Windows.Forms.CheckBox()
        Me.chkIsEditPolicy = New System.Windows.Forms.CheckBox()
        Me.chkIsViewClaim = New System.Windows.Forms.CheckBox()
        Me.chkIsEditFinancePlan = New System.Windows.Forms.CheckBox()
        Me.chkIsRaiseDebit = New System.Windows.Forms.CheckBox()
        Me.chkIsRaiseCredit = New System.Windows.Forms.CheckBox()
        Me.chkIsRaiseCash = New System.Windows.Forms.CheckBox()
        Me.chkIsReverseAllocations = New System.Windows.Forms.CheckBox()
        Me.chkIsRaiseFee = New System.Windows.Forms.CheckBox()
        Me.chkIsRaiseManualDID = New System.Windows.Forms.CheckBox()
        Me.chkIsDeleteClient = New System.Windows.Forms.CheckBox()
        Me.chkIsPerformAllocations = New System.Windows.Forms.CheckBox()
        Me._cmdPrevious_5 = New System.Windows.Forms.Button()
        Me.lblReverseReplaceTransactions = New System.Windows.Forms.Label()
        Me.lblEditSchemePolicy = New System.Windows.Forms.Label()
        Me.lblDeletePolicy = New System.Windows.Forms.Label()
        Me.lblViewClient = New System.Windows.Forms.Label()
        Me.lblEditClient = New System.Windows.Forms.Label()
        Me.lblEditPolicy = New System.Windows.Forms.Label()
        Me.lblViewClaim = New System.Windows.Forms.Label()
        Me.lblEditFinancePlan = New System.Windows.Forms.Label()
        Me.lblRaiseDebit = New System.Windows.Forms.Label()
        Me.lblRaiseCredit = New System.Windows.Forms.Label()
        Me.lblRaiseFee = New System.Windows.Forms.Label()
        Me.lblRaiseCash = New System.Windows.Forms.Label()
        Me.lblReverseTransactions = New System.Windows.Forms.Label()
        Me.lblReverseAllocations = New System.Windows.Forms.Label()
        Me.lblRaiseManualDID = New System.Windows.Forms.Label()
        Me.lblDeleteClient = New System.Windows.Forms.Label()
        Me.lblPerformAllocations = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage7 = New System.Windows.Forms.TabPage()
        Me.fmePartyEdit = New System.Windows.Forms.GroupBox()
        Me._cmdPrevious_6 = New System.Windows.Forms.Button()
        Me.chkOtherPartyMaintenance = New System.Windows.Forms.CheckBox()
        Me.chkInsurerMaintenance = New System.Windows.Forms.CheckBox()
        Me.chkAccountExecutive = New System.Windows.Forms.CheckBox()
        Me.chkAccountHandler = New System.Windows.Forms.CheckBox()
        Me.chkAgentMaintenance = New System.Windows.Forms.CheckBox()
        Me.chkIsViewClientManager = New System.Windows.Forms.CheckBox()
        Me.lblIsViewOnly = New System.Windows.Forms.Label()
        Me.lblPartyCaption = New System.Windows.Forms.Label()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.chkHideDeleted = New System.Windows.Forms.CheckBox()
        Me.Combo1 = New System.Windows.Forms.ComboBox()
        Me._tabMain_TabPage1 = New System.Windows.Forms.TabPage()
        Me.fraMatched = New System.Windows.Forms.GroupBox()
        Me.lvwMatchedUsers = New System.Windows.Forms.ListView()
        Me._lvwMatchedUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatchedUsers_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fraUnmatched = New System.Windows.Forms.GroupBox()
        Me.cboDomain = New System.Windows.Forms.ComboBox()
        Me.lvwSiriusUsers = New System.Windows.Forms.ListView()
        Me._lvwSiriusUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwSiriusUsers_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwDomainUsers = New System.Windows.Forms.ListView()
        Me._lvwDomainUsers_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtLDAPDomain = New System.Windows.Forms.TextBox()
        Me.lblUnmatchedDomain = New System.Windows.Forms.Label()
        Me.lblUnmatchedUsers = New System.Windows.Forms.Label()
        Me.fraSystemSecurity = New System.Windows.Forms.GroupBox()
        Me.cboSystemSecurity = New System.Windows.Forms.ComboBox()
        Me.Ctx_mnuSupervisor = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSupervisor = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSuper = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.txtCurrencyAmount = New System.Windows.Forms.TextBox()
        Me.CurrencyLookup1 = New UserControls.CurrencyLookup()
        Me.pnlEffectiveDate.SuspendLayout()
        Me.pnlUsername.SuspendLayout()
        Me.pnlLastLogin.SuspendLayout()
        Me.pnlPasswordChange.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.Frame0.SuspendLayout()
        Me.fraPrinter.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.Frame7.SuspendLayout()
        Me._pnlAccHandler_0.SuspendLayout()
        Me._pnlAccHandler_1.SuspendLayout()
        Me._pnlAccHandler_2.SuspendLayout()
        Me._pnlAgent_0.SuspendLayout()
        Me._pnlAgent_1.SuspendLayout()
        Me.pnlOtherParty.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.fraDomainAccount.SuspendLayout()
        Me.pnlDomainUserName.SuspendLayout()
        CType(Me.imgSignature, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me._SSTab1_TabPage3.SuspendLayout()
        Me.Frame4.SuspendLayout()
        Me._SSTab1_TabPage4.SuspendLayout()
        Me.fmeAuthorities.SuspendLayout()
        Me.SSTab2.SuspendLayout()
        Me._SSTab2_TabPage0.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me.fmeInstalmentDetail.SuspendLayout()
        Me.fmeReceiptreversal.SuspendLayout()
        Me.fmeAllocationReversal.SuspendLayout()
        Me.fmeWriteOffs.SuspendLayout()
        Me.fmeOverride.SuspendLayout()
        Me._SSTab2_TabPage1.SuspendLayout()
        Me.Frame8.SuspendLayout()
        Me.Frame10.SuspendLayout()
        Me.fmePayments.SuspendLayout()
        Me.fmeClaimPayments.SuspendLayout()
        Me._sstab2_TabPage4.SuspendLayout()
        Me.fmeManualjournal.SuspendLayout()
        Me._SSTab2_TabPage2.SuspendLayout()
        Me.Frame9.SuspendLayout()
        Me.fraReinsurance.SuspendLayout()
        Me.fraRatingSections.SuspendLayout()
        Me.fraMakeLive.SuspendLayout()
        Me.fraCommSettings.SuspendLayout()
        Me.fmeAccess.SuspendLayout()
        Me.fraBrokerAgentPortfolioTransfer.SuspendLayout()
        Me.fraVoidTransaction.SuspendLayout()
        Me._SSTab2_TabPage3.SuspendLayout()
        Me.Frame11.SuspendLayout()
        Me._SSTab1_TabPage5.SuspendLayout()
        Me.Frame6.SuspendLayout()
        Me._SSTab1_TabPage6.SuspendLayout()
        Me.frmClientManagerSecurity.SuspendLayout()
        Me._SSTab1_TabPage7.SuspendLayout()
        Me.fmePartyEdit.SuspendLayout()
        Me._tabMain_TabPage1.SuspendLayout()
        Me.fraMatched.SuspendLayout()
        Me.fraUnmatched.SuspendLayout()
        Me.fraSystemSecurity.SuspendLayout()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(853, 560)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 22)
        Me.cmdApply.TabIndex = 39
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdApply, "Apply changes to Database")
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(771, 560)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 38
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept any changes and exit")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(934, 560)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 40
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel any changes that have not been applied to the db and exit")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtInitials
        '
        Me.txtInitials.AcceptsReturn = True
        Me.txtInitials.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitials.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitials.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitials.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitials.Location = New System.Drawing.Point(369, 77)
        Me.txtInitials.MaxLength = 255
        Me.txtInitials.Name = "txtInitials"
        Me.txtInitials.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitials.Size = New System.Drawing.Size(72, 20)
        Me.txtInitials.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.txtInitials, "User Name")
        '
        'txtFullName
        '
        Me.txtFullName.AcceptsReturn = True
        Me.txtFullName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFullName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFullName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFullName.Location = New System.Drawing.Point(137, 101)
        Me.txtFullName.MaxLength = 255
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFullName.Size = New System.Drawing.Size(304, 20)
        Me.txtFullName.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.txtFullName, "User Name")
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.AcceptsReturn = True
        Me.txtEmailAddress.BackColor = System.Drawing.SystemColors.Window
        Me.txtEmailAddress.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEmailAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmailAddress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEmailAddress.Location = New System.Drawing.Point(137, 221)
        Me.txtEmailAddress.MaxLength = 255
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEmailAddress.Size = New System.Drawing.Size(304, 20)
        Me.txtEmailAddress.TabIndex = 27
        Me.ToolTip1.SetToolTip(Me.txtEmailAddress, "User Name")
        '
        'txtTelephoneNumber
        '
        Me.txtTelephoneNumber.AcceptsReturn = True
        Me.txtTelephoneNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtTelephoneNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTelephoneNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTelephoneNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTelephoneNumber.Location = New System.Drawing.Point(137, 149)
        Me.txtTelephoneNumber.MaxLength = 255
        Me.txtTelephoneNumber.Name = "txtTelephoneNumber"
        Me.txtTelephoneNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTelephoneNumber.Size = New System.Drawing.Size(144, 20)
        Me.txtTelephoneNumber.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.txtTelephoneNumber, "User Name")
        '
        'txtMobileNumber
        '
        Me.txtMobileNumber.AcceptsReturn = True
        Me.txtMobileNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtMobileNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMobileNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobileNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMobileNumber.Location = New System.Drawing.Point(137, 173)
        Me.txtMobileNumber.MaxLength = 255
        Me.txtMobileNumber.Name = "txtMobileNumber"
        Me.txtMobileNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMobileNumber.Size = New System.Drawing.Size(144, 20)
        Me.txtMobileNumber.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.txtMobileNumber, "User Name")
        '
        'txtExtensionNumber
        '
        Me.txtExtensionNumber.AcceptsReturn = True
        Me.txtExtensionNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtExtensionNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExtensionNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExtensionNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExtensionNumber.Location = New System.Drawing.Point(353, 149)
        Me.txtExtensionNumber.MaxLength = 255
        Me.txtExtensionNumber.Name = "txtExtensionNumber"
        Me.txtExtensionNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExtensionNumber.Size = New System.Drawing.Size(88, 20)
        Me.txtExtensionNumber.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.txtExtensionNumber, "User Name")
        '
        'txtFaxNumber
        '
        Me.txtFaxNumber.AcceptsReturn = True
        Me.txtFaxNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtFaxNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFaxNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFaxNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFaxNumber.Location = New System.Drawing.Point(137, 197)
        Me.txtFaxNumber.MaxLength = 255
        Me.txtFaxNumber.Name = "txtFaxNumber"
        Me.txtFaxNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFaxNumber.Size = New System.Drawing.Size(144, 20)
        Me.txtFaxNumber.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.txtFaxNumber, "User Name")
        '
        'pnlEffectiveDate
        '
        Me.pnlEffectiveDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlEffectiveDate.Controls.Add(Me.lblEffectiveDatePanel)
        Me.pnlEffectiveDate.Enabled = False
        Me.pnlEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlEffectiveDate.Location = New System.Drawing.Point(137, 245)
        Me.pnlEffectiveDate.Name = "pnlEffectiveDate"
        Me.pnlEffectiveDate.Size = New System.Drawing.Size(143, 22)
        Me.pnlEffectiveDate.TabIndex = 29
        Me.ToolTip1.SetToolTip(Me.pnlEffectiveDate, "Date when current changes will be effective")
        '
        'lblEffectiveDatePanel
        '
        Me.lblEffectiveDatePanel.AutoSize = True
        Me.lblEffectiveDatePanel.Location = New System.Drawing.Point(0, 0)
        Me.lblEffectiveDatePanel.Name = "lblEffectiveDatePanel"
        Me.lblEffectiveDatePanel.Size = New System.Drawing.Size(0, 13)
        Me.lblEffectiveDatePanel.TabIndex = 0
        '
        'pnlUsername
        '
        Me.pnlUsername.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlUsername.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlUsername.Controls.Add(Me.lblUsernamePanel)
        Me.pnlUsername.Controls.Add(Me.lblUsername)
        Me.pnlUsername.Enabled = False
        Me.pnlUsername.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlUsername.Location = New System.Drawing.Point(137, 15)
        Me.pnlUsername.Name = "pnlUsername"
        Me.pnlUsername.Size = New System.Drawing.Size(144, 21)
        Me.pnlUsername.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.pnlUsername, "Date when current changes will be effective")
        '
        'lblUsernamePanel
        '
        Me.lblUsernamePanel.AutoSize = True
        Me.lblUsernamePanel.Location = New System.Drawing.Point(0, 0)
        Me.lblUsernamePanel.Name = "lblUsernamePanel"
        Me.lblUsernamePanel.Size = New System.Drawing.Size(0, 13)
        Me.lblUsernamePanel.TabIndex = 1
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(1, 3)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(0, 13)
        Me.lblUsername.TabIndex = 0
        '
        'pnlLastLogin
        '
        Me.pnlLastLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlLastLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLastLogin.Controls.Add(Me.lblLastLoginPanel)
        Me.pnlLastLogin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLastLogin.Location = New System.Drawing.Point(137, 269)
        Me.pnlLastLogin.Name = "pnlLastLogin"
        Me.pnlLastLogin.Size = New System.Drawing.Size(144, 22)
        Me.pnlLastLogin.TabIndex = 31
        Me.ToolTip1.SetToolTip(Me.pnlLastLogin, "Last Login date for this user")
        '
        'lblLastLoginPanel
        '
        Me.lblLastLoginPanel.AutoSize = True
        Me.lblLastLoginPanel.Location = New System.Drawing.Point(0, 0)
        Me.lblLastLoginPanel.Name = "lblLastLoginPanel"
        Me.lblLastLoginPanel.Size = New System.Drawing.Size(0, 13)
        Me.lblLastLoginPanel.TabIndex = 0
        '
        'pnlPasswordChange
        '
        Me.pnlPasswordChange.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlPasswordChange.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPasswordChange.Controls.Add(Me.lblPasswordChangePanel)
        Me.pnlPasswordChange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPasswordChange.Location = New System.Drawing.Point(128, 18)
        Me.pnlPasswordChange.Name = "pnlPasswordChange"
        Me.pnlPasswordChange.Size = New System.Drawing.Size(224, 21)
        Me.pnlPasswordChange.TabIndex = 45
        Me.ToolTip1.SetToolTip(Me.pnlPasswordChange, "Last password change date for this user")
        '
        'lblPasswordChangePanel
        '
        Me.lblPasswordChangePanel.AutoSize = True
        Me.lblPasswordChangePanel.Location = New System.Drawing.Point(0, 0)
        Me.lblPasswordChangePanel.Name = "lblPasswordChangePanel"
        Me.lblPasswordChangePanel.Size = New System.Drawing.Size(0, 13)
        Me.lblPasswordChangePanel.TabIndex = 0
        '
        'cmdEditRiskDetails
        '
        Me.cmdEditRiskDetails.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRiskDetails.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRiskDetails.Enabled = False
        Me.cmdEditRiskDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditRiskDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRiskDetails.Location = New System.Drawing.Point(568, 24)
        Me.cmdEditRiskDetails.Name = "cmdEditRiskDetails"
        Me.cmdEditRiskDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRiskDetails.Size = New System.Drawing.Size(73, 22)
        Me.cmdEditRiskDetails.TabIndex = 102
        Me.cmdEditRiskDetails.Text = "&Edit"
        Me.cmdEditRiskDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdEditRiskDetails, "Apply changes to Database")
        Me.cmdEditRiskDetails.UseVisualStyleBackColor = False
        '
        'cmdAddUser
        '
        Me.cmdAddUser.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddUser.Location = New System.Drawing.Point(161, 474)
        Me.cmdAddUser.Name = "cmdAddUser"
        Me.cmdAddUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddUser.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddUser.TabIndex = 4
        Me.cmdAddUser.Text = "&Add"
        Me.cmdAddUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddUser, "Accept any changes and exit")
        Me.cmdAddUser.UseVisualStyleBackColor = False
        '
        'cmdDeleteUser
        '
        Me.cmdDeleteUser.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteUser.Location = New System.Drawing.Point(241, 474)
        Me.cmdDeleteUser.Name = "cmdDeleteUser"
        Me.cmdDeleteUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteUser.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteUser.TabIndex = 5
        Me.cmdDeleteUser.Text = "&Delete"
        Me.cmdDeleteUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteUser, "Accept any changes and exit")
        Me.cmdDeleteUser.UseVisualStyleBackColor = False
        '
        'cmdUnmatch
        '
        Me.cmdUnmatch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUnmatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUnmatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUnmatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUnmatch.Location = New System.Drawing.Point(648, 16)
        Me.cmdUnmatch.Name = "cmdUnmatch"
        Me.cmdUnmatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUnmatch.Size = New System.Drawing.Size(73, 25)
        Me.cmdUnmatch.TabIndex = 146
        Me.cmdUnmatch.Text = "U&nMatch"
        Me.cmdUnmatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdUnmatch, "Unmap a Sirius user from a Domain Account")
        Me.cmdUnmatch.UseVisualStyleBackColor = False
        '
        'cmdMatch
        '
        Me.cmdMatch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMatch.Location = New System.Drawing.Point(648, 40)
        Me.cmdMatch.Name = "cmdMatch"
        Me.cmdMatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMatch.Size = New System.Drawing.Size(73, 25)
        Me.cmdMatch.TabIndex = 143
        Me.cmdMatch.Text = "&Match"
        Me.cmdMatch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdMatch, "Map a Sirius user to a Domain Account")
        Me.cmdMatch.UseVisualStyleBackColor = False
        '
        'cmdlgSignatureOpen
        '
        Me.cmdlgSignatureOpen.Filter = "Signature Files (*.bmp;*.jpg;*.gif)|*.bmp;*.jpg,*.gif"
        Me.cmdlgSignatureOpen.InitialDirectory = "app.path"
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(336, 528)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 41
        Me.uctPMResizer1.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage1)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(500, 18)
        Me.tabMain.Location = New System.Drawing.Point(3, 12)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1006, 542)
        Me.tabMain.TabIndex = 0
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lvwUsers)
        Me._tabMain_TabPage0.Controls.Add(Me.SSTab1)
        Me._tabMain_TabPage0.Controls.Add(Me.txtFilter)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdAddUser)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdDeleteUser)
        Me._tabMain_TabPage0.Controls.Add(Me.chkHideDeleted)
        Me._tabMain_TabPage0.Controls.Add(Me.Combo1)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(998, 516)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Users"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'lvwUsers
        '
        Me.lvwUsers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwUsers_ColumnHeader_1})
        Me.lvwUsers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwUsers.HideSelection = False
        Me.lvwUsers.LargeImageList = Me.imgGroup
        Me.lvwUsers.Location = New System.Drawing.Point(16, 44)
        Me.lvwUsers.Name = "lvwUsers"
        Me.lvwUsers.Size = New System.Drawing.Size(137, 427)
        Me.lvwUsers.SmallImageList = Me.imgGroup
        Me.lvwUsers.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwUsers.TabIndex = 2
        Me.lvwUsers.UseCompatibleStateImageBehavior = False
        Me.lvwUsers.View = System.Windows.Forms.View.Details
        '
        '_lvwUsers_ColumnHeader_1
        '
        Me._lvwUsers_ColumnHeader_1.Text = "User List"
        Me._lvwUsers_ColumnHeader_1.Width = 97
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "user")
        Me.imgGroup.Images.SetKeyName(1, "group")
        Me.imgGroup.Images.SetKeyName(2, "supervisor")
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage3)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage4)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage5)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage6)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage7)
        Me.SSTab1.Enabled = False
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(102, 18)
        Me.SSTab1.Location = New System.Drawing.Point(166, 20)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(832, 448)
        Me.SSTab1.TabIndex = 6
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame0)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - Details"
        Me._SSTab1_TabPage0.UseVisualStyleBackColor = True
        '
        'Frame0
        '
        Me.Frame0.BackColor = System.Drawing.SystemColors.Control
        Me.Frame0.Controls.Add(Me.lblSSOPreferredName)
        Me.Frame0.Controls.Add(Me.txtSSOPreferredName)
        Me.Frame0.Controls.Add(Me.chkSiriusUser)
        Me.Frame0.Controls.Add(Me.txtPercentHoursWorked)
        Me.Frame0.Controls.Add(Me.cboJobBasis)
        Me.Frame0.Controls.Add(Me.fraPrinter)
        Me.Frame0.Controls.Add(Me.txtInitials)
        Me.Frame0.Controls.Add(Me.txtFullName)
        Me.Frame0.Controls.Add(Me.txtEmailAddress)
        Me.Frame0.Controls.Add(Me.txtTelephoneNumber)
        Me.Frame0.Controls.Add(Me.txtMobileNumber)
        Me.Frame0.Controls.Add(Me.txtExtensionNumber)
        Me.Frame0.Controls.Add(Me.txtFaxNumber)
        Me.Frame0.Controls.Add(Me.cboJobTitle)
        Me.Frame0.Controls.Add(Me._cmdNext_0)
        Me.Frame0.Controls.Add(Me.pnlEffectiveDate)
        Me.Frame0.Controls.Add(Me.pnlUsername)
        Me.Frame0.Controls.Add(Me.pnlLastLogin)
        Me.Frame0.Controls.Add(Me.ddTitle)
        Me.Frame0.Controls.Add(Me.lblSiriusUser)
        Me.Frame0.Controls.Add(Me.lblPercentageHoursWorked)
        Me.Frame0.Controls.Add(Me.lblJobBasis)
        Me.Frame0.Controls.Add(Me.lblEmailAddress)
        Me.Frame0.Controls.Add(Me.lblEffectiveDate)
        Me.Frame0.Controls.Add(Me.Label1)
        Me.Frame0.Controls.Add(Me.Label2)
        Me.Frame0.Controls.Add(Me.Label3)
        Me.Frame0.Controls.Add(Me.Label6)
        Me.Frame0.Controls.Add(Me.Label13)
        Me.Frame0.Controls.Add(Me.Label4)
        Me.Frame0.Controls.Add(Me.Label14)
        Me.Frame0.Controls.Add(Me.Label12)
        Me.Frame0.Controls.Add(Me.Label5)
        Me.Frame0.Controls.Add(Me.lblLastLogin)
        Me.Frame0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame0.Location = New System.Drawing.Point(12, 10)
        Me.Frame0.Name = "Frame0"
        Me.Frame0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame0.Size = New System.Drawing.Size(794, 394)
        Me.Frame0.TabIndex = 7
        Me.Frame0.TabStop = False
        '
        'lblSSOPreferredName
        '
        Me.lblSSOPreferredName.AutoSize = True
        Me.lblSSOPreferredName.Location = New System.Drawing.Point(15, 52)
        Me.lblSSOPreferredName.Name = "lblSSOPreferredName"
        Me.lblSSOPreferredName.Size = New System.Drawing.Size(131, 13)
        Me.lblSSOPreferredName.TabIndex = 159
        Me.lblSSOPreferredName.Text = "SSO Preferred User Name"
        '
        'txtSSOPreferredName
        '
        Me.txtSSOPreferredName.Location = New System.Drawing.Point(150, 48)
        Me.txtSSOPreferredName.Name = "txtSSOPreferredName"
        Me.txtSSOPreferredName.Size = New System.Drawing.Size(292, 20)
        Me.txtSSOPreferredName.TabIndex = 158
        '
        'chkSiriusUser
        '
        Me.chkSiriusUser.BackColor = System.Drawing.SystemColors.Control
        Me.chkSiriusUser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSiriusUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSiriusUser.Enabled = False
        Me.chkSiriusUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSiriusUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSiriusUser.Location = New System.Drawing.Point(424, 16)
        Me.chkSiriusUser.Name = "chkSiriusUser"
        Me.chkSiriusUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSiriusUser.Size = New System.Drawing.Size(17, 13)
        Me.chkSiriusUser.TabIndex = 153
        Me.chkSiriusUser.UseVisualStyleBackColor = False
        '
        'txtPercentHoursWorked
        '
        Me.txtPercentHoursWorked.AcceptsReturn = True
        Me.txtPercentHoursWorked.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentHoursWorked.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentHoursWorked.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentHoursWorked.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentHoursWorked.Location = New System.Drawing.Point(376, 295)
        Me.txtPercentHoursWorked.MaxLength = 255
        Me.txtPercentHoursWorked.Name = "txtPercentHoursWorked"
        Me.txtPercentHoursWorked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentHoursWorked.Size = New System.Drawing.Size(64, 20)
        Me.txtPercentHoursWorked.TabIndex = 154
        Me.txtPercentHoursWorked.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboJobBasis
        '
        Me.cboJobBasis.BackColor = System.Drawing.SystemColors.Window
        Me.cboJobBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboJobBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboJobBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboJobBasis.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboJobBasis.Location = New System.Drawing.Point(136, 295)
        Me.cboJobBasis.Name = "cboJobBasis"
        Me.cboJobBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboJobBasis.Size = New System.Drawing.Size(145, 21)
        Me.cboJobBasis.TabIndex = 155
        '
        'fraPrinter
        '
        Me.fraPrinter.BackColor = System.Drawing.SystemColors.Control
        Me.fraPrinter.Controls.Add(Me.chkPrinterChangable)
        Me.fraPrinter.Controls.Add(Me.cboPrinter)
        Me.fraPrinter.Controls.Add(Me.lblPrinterName)
        Me.fraPrinter.Controls.Add(Me.lblPrinterYN)
        Me.fraPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPrinter.Location = New System.Drawing.Point(17, 327)
        Me.fraPrinter.Name = "fraPrinter"
        Me.fraPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPrinter.Size = New System.Drawing.Size(431, 60)
        Me.fraPrinter.TabIndex = 32
        Me.fraPrinter.TabStop = False
        Me.fraPrinter.Text = "Printer"
        '
        'chkPrinterChangable
        '
        Me.chkPrinterChangable.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrinterChangable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPrinterChangable.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrinterChangable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrinterChangable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrinterChangable.Location = New System.Drawing.Point(280, 27)
        Me.chkPrinterChangable.Name = "chkPrinterChangable"
        Me.chkPrinterChangable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrinterChangable.Size = New System.Drawing.Size(104, 17)
        Me.chkPrinterChangable.TabIndex = 35
        Me.chkPrinterChangable.Text = "Changeable?"
        Me.chkPrinterChangable.UseVisualStyleBackColor = False
        '
        'cboPrinter
        '
        Me.cboPrinter.BackColor = System.Drawing.SystemColors.Window
        Me.cboPrinter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrinter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPrinter.Location = New System.Drawing.Point(67, 23)
        Me.cboPrinter.Name = "cboPrinter"
        Me.cboPrinter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPrinter.Size = New System.Drawing.Size(205, 21)
        Me.cboPrinter.TabIndex = 34
        '
        'lblPrinterName
        '
        Me.lblPrinterName.AutoSize = True
        Me.lblPrinterName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrinterName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrinterName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinterName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrinterName.Location = New System.Drawing.Point(19, 27)
        Me.lblPrinterName.Name = "lblPrinterName"
        Me.lblPrinterName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrinterName.Size = New System.Drawing.Size(38, 13)
        Me.lblPrinterName.TabIndex = 33
        Me.lblPrinterName.Text = "Name:"
        '
        'lblPrinterYN
        '
        Me.lblPrinterYN.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrinterYN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrinterYN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinterYN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrinterYN.Location = New System.Drawing.Point(392, 27)
        Me.lblPrinterYN.Name = "lblPrinterYN"
        Me.lblPrinterYN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrinterYN.Size = New System.Drawing.Size(26, 13)
        Me.lblPrinterYN.TabIndex = 36
        Me.lblPrinterYN.Text = "No"
        '
        'cboJobTitle
        '
        Me.cboJobTitle.BackColor = System.Drawing.SystemColors.Window
        Me.cboJobTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboJobTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboJobTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboJobTitle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboJobTitle.Location = New System.Drawing.Point(137, 125)
        Me.cboJobTitle.Name = "cboJobTitle"
        Me.cboJobTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboJobTitle.Size = New System.Drawing.Size(305, 21)
        Me.cboJobTitle.TabIndex = 17
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(604, 350)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_0.TabIndex = 37
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'ddTitle
        '
        Me.ddTitle.AllowAbiCodeEntry = False
        Me.ddTitle.AutoCompleteText = False
        Me.ddTitle.DataModel = "GIIM"
        Me.ddTitle.ListIndex = -1
        Me.ddTitle.ListManager = Nothing
        Me.ddTitle.Location = New System.Drawing.Point(137, 77)
        Me.ddTitle.Login = False
        Me.ddTitle.LongList = False
        Me.ddTitle.MousePointer = System.Windows.Forms.Cursors.Default
        Me.ddTitle.Name = "ddTitle"
        Me.ddTitle.PropertyId = "131085"
        Me.ddTitle.ReadOnly_Renamed = False
        Me.ddTitle.SelLength = 0
        Me.ddTitle.SelStart = 0
        Me.ddTitle.SelText = ""
        Me.ddTitle.Size = New System.Drawing.Size(145, 21)
        Me.ddTitle.TabIndex = 11
        Me.ddTitle.ToolTipText = ""
        Me.ddTitle.VehicleListId = ""
        Me.ddTitle.VehicleMake = ""
        '
        'lblSiriusUser
        '
        Me.lblSiriusUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblSiriusUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSiriusUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSiriusUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSiriusUser.Location = New System.Drawing.Point(344, 16)
        Me.lblSiriusUser.Name = "lblSiriusUser"
        Me.lblSiriusUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSiriusUser.Size = New System.Drawing.Size(76, 13)
        Me.lblSiriusUser.TabIndex = 152
        Me.lblSiriusUser.Text = "Sirius User:"
        '
        'lblPercentageHoursWorked
        '
        Me.lblPercentageHoursWorked.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentageHoursWorked.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentageHoursWorked.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentageHoursWorked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentageHoursWorked.Location = New System.Drawing.Point(288, 275)
        Me.lblPercentageHoursWorked.Name = "lblPercentageHoursWorked"
        Me.lblPercentageHoursWorked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentageHoursWorked.Size = New System.Drawing.Size(92, 37)
        Me.lblPercentageHoursWorked.TabIndex = 156
        Me.lblPercentageHoursWorked.Text = "Percentage of Normal Hours Worked:"
        '
        'lblJobBasis
        '
        Me.lblJobBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblJobBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblJobBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJobBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblJobBasis.Location = New System.Drawing.Point(16, 299)
        Me.lblJobBasis.Name = "lblJobBasis"
        Me.lblJobBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblJobBasis.Size = New System.Drawing.Size(116, 13)
        Me.lblJobBasis.TabIndex = 157
        Me.lblJobBasis.Text = "Job Basis:"
        '
        'lblEmailAddress
        '
        Me.lblEmailAddress.BackColor = System.Drawing.SystemColors.Control
        Me.lblEmailAddress.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEmailAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmailAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmailAddress.Location = New System.Drawing.Point(17, 225)
        Me.lblEmailAddress.Name = "lblEmailAddress"
        Me.lblEmailAddress.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEmailAddress.Size = New System.Drawing.Size(153, 14)
        Me.lblEmailAddress.TabIndex = 26
        Me.lblEmailAddress.Text = "Email Address:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(17, 249)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(116, 14)
        Me.lblEffectiveDate.TabIndex = 28
        Me.lblEffectiveDate.Text = "Effective date:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(17, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(116, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "User Name:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(297, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(68, 14)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Initials/Ref:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(17, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(116, 14)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Full Name:"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(17, 81)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(116, 14)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Title:"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(17, 129)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(116, 14)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "Job Title:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(17, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(116, 14)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Direct Dial Number:"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(17, 177)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(100, 14)
        Me.Label14.TabIndex = 22
        Me.Label14.Text = "Mobile Number:"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(313, 153)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(36, 14)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Extn:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(17, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(92, 14)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Fax Number:"
        '
        'lblLastLogin
        '
        Me.lblLastLogin.BackColor = System.Drawing.SystemColors.Control
        Me.lblLastLogin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLastLogin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastLogin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLastLogin.Location = New System.Drawing.Point(17, 273)
        Me.lblLastLogin.Name = "lblLastLogin"
        Me.lblLastLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLastLogin.Size = New System.Drawing.Size(116, 14)
        Me.lblLastLogin.TabIndex = 30
        Me.lblLastLogin.Text = "Last login:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "2 - Security"
        Me._SSTab1_TabPage1.UseVisualStyleBackColor = True
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._cmdPrevious_0)
        Me.Frame1.Controls.Add(Me._cmdNext_1)
        Me.Frame1.Controls.Add(Me.cmdSignature)
        Me.Frame1.Controls.Add(Me.Frame7)
        Me.Frame1.Controls.Add(Me.Frame2)
        Me.Frame1.Controls.Add(Me.fraDomainAccount)
        Me.Frame1.Controls.Add(Me.imgSignature)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(12, 10)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(650, 373)
        Me.Frame1.TabIndex = 41
        Me.Frame1.TabStop = False
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_0.TabIndex = 76
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(604, 350)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_1.TabIndex = 77
        Me._cmdNext_1.Text = ">>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'cmdSignature
        '
        Me.cmdSignature.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSignature.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSignature.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSignature.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSignature.Location = New System.Drawing.Point(102, 21)
        Me.cmdSignature.Name = "cmdSignature"
        Me.cmdSignature.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSignature.Size = New System.Drawing.Size(119, 21)
        Me.cmdSignature.TabIndex = 42
        Me.cmdSignature.Text = "Digital Signature:"
        Me.cmdSignature.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSignature.UseVisualStyleBackColor = False
        '
        'Frame7
        '
        Me.Frame7.BackColor = System.Drawing.SystemColors.Control
        Me.Frame7.Controls.Add(Me.chkOtherParty)
        Me.Frame7.Controls.Add(Me.cmdOtherParty)
        Me.Frame7.Controls.Add(Me._cmdAgent_1)
        Me.Frame7.Controls.Add(Me._chkAgent_1)
        Me.Frame7.Controls.Add(Me._chkAgent_0)
        Me.Frame7.Controls.Add(Me._cmdAgent_0)
        Me.Frame7.Controls.Add(Me._cmdAccHandler_0)
        Me.Frame7.Controls.Add(Me._cmdAccHandler_1)
        Me.Frame7.Controls.Add(Me._cmdAccHandler_2)
        Me.Frame7.Controls.Add(Me._chkAccHandler_0)
        Me.Frame7.Controls.Add(Me._chkAccHandler_1)
        Me.Frame7.Controls.Add(Me._chkAccHandler_2)
        Me.Frame7.Controls.Add(Me._pnlAccHandler_0)
        Me.Frame7.Controls.Add(Me._pnlAccHandler_1)
        Me.Frame7.Controls.Add(Me._pnlAccHandler_2)
        Me.Frame7.Controls.Add(Me._pnlAgent_0)
        Me.Frame7.Controls.Add(Me._pnlAgent_1)
        Me.Frame7.Controls.Add(Me.pnlOtherParty)
        Me.Frame7.Controls.Add(Me.lblOtherPartyYN)
        Me.Frame7.Controls.Add(Me._lblAgentYN_1)
        Me.Frame7.Controls.Add(Me._lblAgentYN_0)
        Me.Frame7.Controls.Add(Me._lblAccHandlerYN_0)
        Me.Frame7.Controls.Add(Me._lblAccHandlerYN_1)
        Me.Frame7.Controls.Add(Me._lblAccHandlerYN_2)
        Me.Frame7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame7.Location = New System.Drawing.Point(102, 168)
        Me.Frame7.Name = "Frame7"
        Me.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame7.Size = New System.Drawing.Size(439, 177)
        Me.Frame7.TabIndex = 51
        Me.Frame7.TabStop = False
        Me.Frame7.Text = "User Type"
        '
        'chkOtherParty
        '
        Me.chkOtherParty.BackColor = System.Drawing.SystemColors.Control
        Me.chkOtherParty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOtherParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOtherParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOtherParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOtherParty.Location = New System.Drawing.Point(8, 148)
        Me.chkOtherParty.Name = "chkOtherParty"
        Me.chkOtherParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOtherParty.Size = New System.Drawing.Size(144, 17)
        Me.chkOtherParty.TabIndex = 72
        Me.chkOtherParty.Text = "Other Party?"
        Me.chkOtherParty.UseVisualStyleBackColor = False
        '
        'cmdOtherParty
        '
        Me.cmdOtherParty.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOtherParty.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOtherParty.Enabled = False
        Me.cmdOtherParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOtherParty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOtherParty.Location = New System.Drawing.Point(360, 144)
        Me.cmdOtherParty.Name = "cmdOtherParty"
        Me.cmdOtherParty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOtherParty.Size = New System.Drawing.Size(65, 21)
        Me.cmdOtherParty.TabIndex = 75
        Me.cmdOtherParty.Text = "Update:"
        Me.cmdOtherParty.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOtherParty.UseVisualStyleBackColor = False
        '
        '_cmdAgent_1
        '
        Me._cmdAgent_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAgent_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAgent_1.Enabled = False
        Me._cmdAgent_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAgent_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAgent_1.Location = New System.Drawing.Point(360, 120)
        Me._cmdAgent_1.Name = "_cmdAgent_1"
        Me._cmdAgent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAgent_1.Size = New System.Drawing.Size(65, 21)
        Me._cmdAgent_1.TabIndex = 71
        Me._cmdAgent_1.Text = "Update:"
        Me._cmdAgent_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAgent_1.UseVisualStyleBackColor = False
        '
        '_chkAgent_1
        '
        Me._chkAgent_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkAgent_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkAgent_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAgent_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAgent_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAgent_1.Location = New System.Drawing.Point(8, 124)
        Me._chkAgent_1.Name = "_chkAgent_1"
        Me._chkAgent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAgent_1.Size = New System.Drawing.Size(144, 18)
        Me._chkAgent_1.TabIndex = 68
        Me._chkAgent_1.Text = "Insurer?"
        Me._chkAgent_1.UseVisualStyleBackColor = False
        '
        '_chkAgent_0
        '
        Me._chkAgent_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkAgent_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkAgent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAgent_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAgent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAgent_0.Location = New System.Drawing.Point(8, 100)
        Me._chkAgent_0.Name = "_chkAgent_0"
        Me._chkAgent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAgent_0.Size = New System.Drawing.Size(144, 18)
        Me._chkAgent_0.TabIndex = 64
        Me._chkAgent_0.Text = "Agent?"
        Me._chkAgent_0.UseVisualStyleBackColor = False
        '
        '_cmdAgent_0
        '
        Me._cmdAgent_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAgent_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAgent_0.Enabled = False
        Me._cmdAgent_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAgent_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAgent_0.Location = New System.Drawing.Point(360, 96)
        Me._cmdAgent_0.Name = "_cmdAgent_0"
        Me._cmdAgent_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAgent_0.Size = New System.Drawing.Size(65, 21)
        Me._cmdAgent_0.TabIndex = 67
        Me._cmdAgent_0.Text = "Update:"
        Me._cmdAgent_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAgent_0.UseVisualStyleBackColor = False
        '
        '_cmdAccHandler_0
        '
        Me._cmdAccHandler_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAccHandler_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAccHandler_0.Enabled = False
        Me._cmdAccHandler_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAccHandler_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAccHandler_0.Location = New System.Drawing.Point(360, 24)
        Me._cmdAccHandler_0.Name = "_cmdAccHandler_0"
        Me._cmdAccHandler_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAccHandler_0.Size = New System.Drawing.Size(65, 21)
        Me._cmdAccHandler_0.TabIndex = 55
        Me._cmdAccHandler_0.Text = "Update:"
        Me._cmdAccHandler_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAccHandler_0.UseVisualStyleBackColor = False
        '
        '_cmdAccHandler_1
        '
        Me._cmdAccHandler_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAccHandler_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAccHandler_1.Enabled = False
        Me._cmdAccHandler_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAccHandler_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAccHandler_1.Location = New System.Drawing.Point(360, 48)
        Me._cmdAccHandler_1.Name = "_cmdAccHandler_1"
        Me._cmdAccHandler_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAccHandler_1.Size = New System.Drawing.Size(65, 21)
        Me._cmdAccHandler_1.TabIndex = 59
        Me._cmdAccHandler_1.Text = "Update:"
        Me._cmdAccHandler_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAccHandler_1.UseVisualStyleBackColor = False
        '
        '_cmdAccHandler_2
        '
        Me._cmdAccHandler_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAccHandler_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAccHandler_2.Enabled = False
        Me._cmdAccHandler_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAccHandler_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAccHandler_2.Location = New System.Drawing.Point(360, 72)
        Me._cmdAccHandler_2.Name = "_cmdAccHandler_2"
        Me._cmdAccHandler_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAccHandler_2.Size = New System.Drawing.Size(65, 21)
        Me._cmdAccHandler_2.TabIndex = 63
        Me._cmdAccHandler_2.Text = "Update:"
        Me._cmdAccHandler_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAccHandler_2.UseVisualStyleBackColor = False
        '
        '_chkAccHandler_0
        '
        Me._chkAccHandler_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkAccHandler_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkAccHandler_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAccHandler_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAccHandler_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAccHandler_0.Location = New System.Drawing.Point(8, 28)
        Me._chkAccHandler_0.Name = "_chkAccHandler_0"
        Me._chkAccHandler_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAccHandler_0.Size = New System.Drawing.Size(144, 17)
        Me._chkAccHandler_0.TabIndex = 52
        Me._chkAccHandler_0.Text = "Account Handler?"
        Me._chkAccHandler_0.UseVisualStyleBackColor = False
        '
        '_chkAccHandler_1
        '
        Me._chkAccHandler_1.BackColor = System.Drawing.SystemColors.Control
        Me._chkAccHandler_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkAccHandler_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAccHandler_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAccHandler_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAccHandler_1.Location = New System.Drawing.Point(8, 52)
        Me._chkAccHandler_1.Name = "_chkAccHandler_1"
        Me._chkAccHandler_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAccHandler_1.Size = New System.Drawing.Size(144, 18)
        Me._chkAccHandler_1.TabIndex = 56
        Me._chkAccHandler_1.Text = "Account Executive?"
        Me._chkAccHandler_1.UseVisualStyleBackColor = False
        '
        '_chkAccHandler_2
        '
        Me._chkAccHandler_2.BackColor = System.Drawing.SystemColors.Control
        Me._chkAccHandler_2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkAccHandler_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkAccHandler_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkAccHandler_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkAccHandler_2.Location = New System.Drawing.Point(8, 76)
        Me._chkAccHandler_2.Name = "_chkAccHandler_2"
        Me._chkAccHandler_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkAccHandler_2.Size = New System.Drawing.Size(144, 17)
        Me._chkAccHandler_2.TabIndex = 60
        Me._chkAccHandler_2.Text = "Claims Handler?"
        Me._chkAccHandler_2.UseVisualStyleBackColor = False
        '
        '_pnlAccHandler_0
        '
        Me._pnlAccHandler_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me._pnlAccHandler_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlAccHandler_0.Controls.Add(Me.lblAccHandlerPanel)
        Me._pnlAccHandler_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlAccHandler_0.Location = New System.Drawing.Point(194, 24)
        Me._pnlAccHandler_0.Name = "_pnlAccHandler_0"
        Me._pnlAccHandler_0.Size = New System.Drawing.Size(160, 21)
        Me._pnlAccHandler_0.TabIndex = 54
        '
        'lblAccHandlerPanel
        '
        Me.lblAccHandlerPanel.AutoSize = True
        Me.lblAccHandlerPanel.Location = New System.Drawing.Point(0, 0)
        Me.lblAccHandlerPanel.Name = "lblAccHandlerPanel"
        Me.lblAccHandlerPanel.Size = New System.Drawing.Size(0, 13)
        Me.lblAccHandlerPanel.TabIndex = 0
        '
        '_pnlAccHandler_1
        '
        Me._pnlAccHandler_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me._pnlAccHandler_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlAccHandler_1.Controls.Add(Me.lblAccExecutivePanel)
        Me._pnlAccHandler_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlAccHandler_1.Location = New System.Drawing.Point(194, 48)
        Me._pnlAccHandler_1.Name = "_pnlAccHandler_1"
        Me._pnlAccHandler_1.Size = New System.Drawing.Size(160, 21)
        Me._pnlAccHandler_1.TabIndex = 58
        '
        'lblAccExecutivePanel
        '
        Me.lblAccExecutivePanel.AutoSize = True
        Me.lblAccExecutivePanel.Location = New System.Drawing.Point(0, 0)
        Me.lblAccExecutivePanel.Name = "lblAccExecutivePanel"
        Me.lblAccExecutivePanel.Size = New System.Drawing.Size(0, 13)
        Me.lblAccExecutivePanel.TabIndex = 0
        '
        '_pnlAccHandler_2
        '
        Me._pnlAccHandler_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me._pnlAccHandler_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlAccHandler_2.Controls.Add(Me.lblClaimsHandlerPanel)
        Me._pnlAccHandler_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlAccHandler_2.Location = New System.Drawing.Point(194, 72)
        Me._pnlAccHandler_2.Name = "_pnlAccHandler_2"
        Me._pnlAccHandler_2.Size = New System.Drawing.Size(160, 21)
        Me._pnlAccHandler_2.TabIndex = 62
        '
        'lblClaimsHandlerPanel
        '
        Me.lblClaimsHandlerPanel.AutoSize = True
        Me.lblClaimsHandlerPanel.Location = New System.Drawing.Point(0, 0)
        Me.lblClaimsHandlerPanel.Name = "lblClaimsHandlerPanel"
        Me.lblClaimsHandlerPanel.Size = New System.Drawing.Size(0, 13)
        Me.lblClaimsHandlerPanel.TabIndex = 0
        '
        '_pnlAgent_0
        '
        Me._pnlAgent_0.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me._pnlAgent_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlAgent_0.Controls.Add(Me.lblAgentPanel)
        Me._pnlAgent_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlAgent_0.Location = New System.Drawing.Point(194, 96)
        Me._pnlAgent_0.Name = "_pnlAgent_0"
        Me._pnlAgent_0.Size = New System.Drawing.Size(160, 21)
        Me._pnlAgent_0.TabIndex = 66
        '
        'lblAgentPanel
        '
        Me.lblAgentPanel.AutoSize = True
        Me.lblAgentPanel.Location = New System.Drawing.Point(0, 0)
        Me.lblAgentPanel.Name = "lblAgentPanel"
        Me.lblAgentPanel.Size = New System.Drawing.Size(0, 13)
        Me.lblAgentPanel.TabIndex = 0
        '
        '_pnlAgent_1
        '
        Me._pnlAgent_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me._pnlAgent_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlAgent_1.Controls.Add(Me.lblInsurerPanel)
        Me._pnlAgent_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlAgent_1.Location = New System.Drawing.Point(194, 120)
        Me._pnlAgent_1.Name = "_pnlAgent_1"
        Me._pnlAgent_1.Size = New System.Drawing.Size(160, 21)
        Me._pnlAgent_1.TabIndex = 70
        '
        'lblInsurerPanel
        '
        Me.lblInsurerPanel.AutoSize = True
        Me.lblInsurerPanel.Location = New System.Drawing.Point(0, 0)
        Me.lblInsurerPanel.Name = "lblInsurerPanel"
        Me.lblInsurerPanel.Size = New System.Drawing.Size(0, 13)
        Me.lblInsurerPanel.TabIndex = 0
        '
        'pnlOtherParty
        '
        Me.pnlOtherParty.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlOtherParty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlOtherParty.Controls.Add(Me.lblOtherPartypanel)
        Me.pnlOtherParty.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlOtherParty.Location = New System.Drawing.Point(194, 144)
        Me.pnlOtherParty.Name = "pnlOtherParty"
        Me.pnlOtherParty.Size = New System.Drawing.Size(160, 21)
        Me.pnlOtherParty.TabIndex = 74
        '
        'lblOtherPartypanel
        '
        Me.lblOtherPartypanel.AutoSize = True
        Me.lblOtherPartypanel.Location = New System.Drawing.Point(0, 0)
        Me.lblOtherPartypanel.Name = "lblOtherPartypanel"
        Me.lblOtherPartypanel.Size = New System.Drawing.Size(0, 13)
        Me.lblOtherPartypanel.TabIndex = 0
        '
        'lblOtherPartyYN
        '
        Me.lblOtherPartyYN.BackColor = System.Drawing.SystemColors.Control
        Me.lblOtherPartyYN.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOtherPartyYN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherPartyYN.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOtherPartyYN.Location = New System.Drawing.Point(160, 148)
        Me.lblOtherPartyYN.Name = "lblOtherPartyYN"
        Me.lblOtherPartyYN.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOtherPartyYN.Size = New System.Drawing.Size(26, 13)
        Me.lblOtherPartyYN.TabIndex = 73
        Me.lblOtherPartyYN.Text = "No"
        '
        '_lblAgentYN_1
        '
        Me._lblAgentYN_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblAgentYN_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAgentYN_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAgentYN_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAgentYN_1.Location = New System.Drawing.Point(160, 124)
        Me._lblAgentYN_1.Name = "_lblAgentYN_1"
        Me._lblAgentYN_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAgentYN_1.Size = New System.Drawing.Size(26, 13)
        Me._lblAgentYN_1.TabIndex = 69
        Me._lblAgentYN_1.Text = "No"
        '
        '_lblAgentYN_0
        '
        Me._lblAgentYN_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblAgentYN_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAgentYN_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAgentYN_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAgentYN_0.Location = New System.Drawing.Point(160, 100)
        Me._lblAgentYN_0.Name = "_lblAgentYN_0"
        Me._lblAgentYN_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAgentYN_0.Size = New System.Drawing.Size(26, 13)
        Me._lblAgentYN_0.TabIndex = 65
        Me._lblAgentYN_0.Text = "No"
        '
        '_lblAccHandlerYN_0
        '
        Me._lblAccHandlerYN_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblAccHandlerYN_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAccHandlerYN_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAccHandlerYN_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAccHandlerYN_0.Location = New System.Drawing.Point(160, 28)
        Me._lblAccHandlerYN_0.Name = "_lblAccHandlerYN_0"
        Me._lblAccHandlerYN_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAccHandlerYN_0.Size = New System.Drawing.Size(26, 13)
        Me._lblAccHandlerYN_0.TabIndex = 53
        Me._lblAccHandlerYN_0.Text = "No"
        '
        '_lblAccHandlerYN_1
        '
        Me._lblAccHandlerYN_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblAccHandlerYN_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAccHandlerYN_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAccHandlerYN_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAccHandlerYN_1.Location = New System.Drawing.Point(160, 52)
        Me._lblAccHandlerYN_1.Name = "_lblAccHandlerYN_1"
        Me._lblAccHandlerYN_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAccHandlerYN_1.Size = New System.Drawing.Size(26, 13)
        Me._lblAccHandlerYN_1.TabIndex = 57
        Me._lblAccHandlerYN_1.Text = "No"
        '
        '_lblAccHandlerYN_2
        '
        Me._lblAccHandlerYN_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblAccHandlerYN_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAccHandlerYN_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAccHandlerYN_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAccHandlerYN_2.Location = New System.Drawing.Point(160, 76)
        Me._lblAccHandlerYN_2.Name = "_lblAccHandlerYN_2"
        Me._lblAccHandlerYN_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAccHandlerYN_2.Size = New System.Drawing.Size(26, 13)
        Me._lblAccHandlerYN_2.TabIndex = 61
        Me._lblAccHandlerYN_2.Text = "No"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cmdChangePassword)
        Me.Frame2.Controls.Add(Me.pnlPasswordChange)
        Me.Frame2.Controls.Add(Me.Label7)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(104, 56)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(437, 49)
        Me.Frame2.TabIndex = 43
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Password"
        '
        'cmdChangePassword
        '
        Me.cmdChangePassword.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangePassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangePassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangePassword.Location = New System.Drawing.Point(360, 16)
        Me.cmdChangePassword.Name = "cmdChangePassword"
        Me.cmdChangePassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangePassword.Size = New System.Drawing.Size(57, 21)
        Me.cmdChangePassword.TabIndex = 46
        Me.cmdChangePassword.Text = "Change"
        Me.cmdChangePassword.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangePassword.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(16, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(116, 13)
        Me.Label7.TabIndex = 44
        Me.Label7.Text = "Last Changed:"
        '
        'fraDomainAccount
        '
        Me.fraDomainAccount.BackColor = System.Drawing.SystemColors.Control
        Me.fraDomainAccount.Controls.Add(Me.cmdChangeDomainAccount)
        Me.fraDomainAccount.Controls.Add(Me.pnlDomainUserName)
        Me.fraDomainAccount.Controls.Add(Me.Label15)
        Me.fraDomainAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDomainAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDomainAccount.Location = New System.Drawing.Point(104, 112)
        Me.fraDomainAccount.Name = "fraDomainAccount"
        Me.fraDomainAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDomainAccount.Size = New System.Drawing.Size(437, 49)
        Me.fraDomainAccount.TabIndex = 47
        Me.fraDomainAccount.TabStop = False
        Me.fraDomainAccount.Text = "Domain Account"
        '
        'cmdChangeDomainAccount
        '
        Me.cmdChangeDomainAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdChangeDomainAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdChangeDomainAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdChangeDomainAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdChangeDomainAccount.Location = New System.Drawing.Point(360, 16)
        Me.cmdChangeDomainAccount.Name = "cmdChangeDomainAccount"
        Me.cmdChangeDomainAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdChangeDomainAccount.Size = New System.Drawing.Size(57, 21)
        Me.cmdChangeDomainAccount.TabIndex = 50
        Me.cmdChangeDomainAccount.Text = "Change"
        Me.cmdChangeDomainAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdChangeDomainAccount.UseVisualStyleBackColor = False
        '
        'pnlDomainUserName
        '
        Me.pnlDomainUserName.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.pnlDomainUserName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlDomainUserName.Controls.Add(Me.lblDomainUserNamePanel)
        Me.pnlDomainUserName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDomainUserName.Location = New System.Drawing.Point(128, 18)
        Me.pnlDomainUserName.Name = "pnlDomainUserName"
        Me.pnlDomainUserName.Size = New System.Drawing.Size(224, 21)
        Me.pnlDomainUserName.TabIndex = 49
        '
        'lblDomainUserNamePanel
        '
        Me.lblDomainUserNamePanel.AutoSize = True
        Me.lblDomainUserNamePanel.Location = New System.Drawing.Point(0, 0)
        Me.lblDomainUserNamePanel.Name = "lblDomainUserNamePanel"
        Me.lblDomainUserNamePanel.Size = New System.Drawing.Size(0, 13)
        Me.lblDomainUserNamePanel.TabIndex = 0
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label15.Location = New System.Drawing.Point(16, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label15.Size = New System.Drawing.Size(116, 13)
        Me.Label15.TabIndex = 48
        Me.Label15.Text = "User Name:"
        '
        'imgSignature
        '
        Me.imgSignature.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.imgSignature.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgSignature.Location = New System.Drawing.Point(230, 16)
        Me.imgSignature.Name = "imgSignature"
        Me.imgSignature.Size = New System.Drawing.Size(307, 41)
        Me.imgSignature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgSignature.TabIndex = 78
        Me.imgSignature.TabStop = False
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.Frame3)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "3 - Branches"
        Me._SSTab1_TabPage2.UseVisualStyleBackColor = True
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me._cmdPrevious_1)
        Me.Frame3.Controls.Add(Me._cmdNext_2)
        Me.Frame3.Controls.Add(Me.cmdAddBranch)
        Me.Frame3.Controls.Add(Me.cmdDelAllBranches)
        Me.Frame3.Controls.Add(Me.cmdDelBranch)
        Me.Frame3.Controls.Add(Me.cmdAddAllBranches)
        Me.Frame3.Controls.Add(Me.lvwBranches)
        Me.Frame3.Controls.Add(Me.lvwSelectedBranches)
        Me.Frame3.Controls.Add(Me.Label9)
        Me.Frame3.Controls.Add(Me.Label8)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(12, 10)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(650, 373)
        Me.Frame3.TabIndex = 78
        Me.Frame3.TabStop = False
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_1.TabIndex = 87
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
        Me._cmdNext_2.Location = New System.Drawing.Point(604, 350)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_2.TabIndex = 88
        Me._cmdNext_2.Text = ">>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        'cmdAddBranch
        '
        Me.cmdAddBranch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddBranch.Location = New System.Drawing.Point(294, 56)
        Me.cmdAddBranch.Name = "cmdAddBranch"
        Me.cmdAddBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddBranch.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddBranch.TabIndex = 81
        Me.cmdAddBranch.Text = "--&>"
        Me.cmdAddBranch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddBranch.UseVisualStyleBackColor = False
        '
        'cmdDelAllBranches
        '
        Me.cmdDelAllBranches.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelAllBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelAllBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelAllBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelAllBranches.Location = New System.Drawing.Point(294, 296)
        Me.cmdDelAllBranches.Name = "cmdDelAllBranches"
        Me.cmdDelAllBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelAllBranches.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelAllBranches.TabIndex = 84
        Me.cmdDelAllBranches.Text = "<<-"
        Me.cmdDelAllBranches.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelAllBranches.UseVisualStyleBackColor = False
        '
        'cmdDelBranch
        '
        Me.cmdDelBranch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelBranch.Location = New System.Drawing.Point(294, 264)
        Me.cmdDelBranch.Name = "cmdDelBranch"
        Me.cmdDelBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelBranch.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelBranch.TabIndex = 83
        Me.cmdDelBranch.Text = "&<--"
        Me.cmdDelBranch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelBranch.UseVisualStyleBackColor = False
        '
        'cmdAddAllBranches
        '
        Me.cmdAddAllBranches.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllBranches.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllBranches.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllBranches.Location = New System.Drawing.Point(294, 88)
        Me.cmdAddAllBranches.Name = "cmdAddAllBranches"
        Me.cmdAddAllBranches.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllBranches.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddAllBranches.TabIndex = 82
        Me.cmdAddAllBranches.Text = "->>"
        Me.cmdAddAllBranches.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAllBranches.UseVisualStyleBackColor = False
        '
        'lvwBranches
        '
        Me.lvwBranches.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBranches.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBranches_ColumnHeader_1})
        Me.lvwBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBranches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBranches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwBranches.HideSelection = False
        Me.lvwBranches.Location = New System.Drawing.Point(24, 48)
        Me.lvwBranches.Name = "lvwBranches"
        Me.lvwBranches.Size = New System.Drawing.Size(257, 287)
        Me.lvwBranches.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwBranches.TabIndex = 80
        Me.lvwBranches.UseCompatibleStateImageBehavior = False
        Me.lvwBranches.View = System.Windows.Forms.View.Details
        '
        '_lvwBranches_ColumnHeader_1
        '
        Me._lvwBranches_ColumnHeader_1.Width = 167
        '
        'lvwSelectedBranches
        '
        Me.lvwSelectedBranches.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSelectedBranches.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSelectedBranches_ColumnHeader_1})
        Me.lvwSelectedBranches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSelectedBranches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSelectedBranches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwSelectedBranches.HideSelection = False
        Me.lvwSelectedBranches.Location = New System.Drawing.Point(360, 48)
        Me.lvwSelectedBranches.Name = "lvwSelectedBranches"
        Me.lvwSelectedBranches.Size = New System.Drawing.Size(257, 287)
        Me.lvwSelectedBranches.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSelectedBranches.TabIndex = 86
        Me.lvwSelectedBranches.UseCompatibleStateImageBehavior = False
        Me.lvwSelectedBranches.View = System.Windows.Forms.View.Details
        '
        '_lvwSelectedBranches_ColumnHeader_1
        '
        Me._lvwSelectedBranches_ColumnHeader_1.Width = 167
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(360, 30)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(116, 21)
        Me.Label9.TabIndex = 85
        Me.Label9.Text = "Allow Access:"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(24, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(116, 21)
        Me.Label8.TabIndex = 79
        Me.Label8.Text = "Prevent Access:"
        '
        '_SSTab1_TabPage3
        '
        Me._SSTab1_TabPage3.Controls.Add(Me.Frame4)
        Me._SSTab1_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage3.Name = "_SSTab1_TabPage3"
        Me._SSTab1_TabPage3.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage3.TabIndex = 3
        Me._SSTab1_TabPage3.Text = "4 - User Groups"
        Me._SSTab1_TabPage3.UseVisualStyleBackColor = True
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me._cmdPrevious_2)
        Me.Frame4.Controls.Add(Me._cmdNext_3)
        Me.Frame4.Controls.Add(Me.cmdAddGroup)
        Me.Frame4.Controls.Add(Me.cmdDelAllGroups)
        Me.Frame4.Controls.Add(Me.cmdDelGroup)
        Me.Frame4.Controls.Add(Me.cmdAddAllGroups)
        Me.Frame4.Controls.Add(Me.lvwSelectedGroups)
        Me.Frame4.Controls.Add(Me.lvwGroups)
        Me.Frame4.Controls.Add(Me.Label10)
        Me.Frame4.Controls.Add(Me.Label11)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(12, 10)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(650, 373)
        Me.Frame4.TabIndex = 89
        Me.Frame4.TabStop = False
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_2.TabIndex = 98
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
        Me._cmdNext_3.Location = New System.Drawing.Point(604, 350)
        Me._cmdNext_3.Name = "_cmdNext_3"
        Me._cmdNext_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_3.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_3.TabIndex = 99
        Me._cmdNext_3.Text = ">>"
        Me._cmdNext_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_3.UseVisualStyleBackColor = False
        '
        'cmdAddGroup
        '
        Me.cmdAddGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddGroup.Location = New System.Drawing.Point(294, 56)
        Me.cmdAddGroup.Name = "cmdAddGroup"
        Me.cmdAddGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddGroup.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddGroup.TabIndex = 92
        Me.cmdAddGroup.Text = "--&>"
        Me.cmdAddGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddGroup.UseVisualStyleBackColor = False
        '
        'cmdDelAllGroups
        '
        Me.cmdDelAllGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelAllGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelAllGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelAllGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelAllGroups.Location = New System.Drawing.Point(294, 296)
        Me.cmdDelAllGroups.Name = "cmdDelAllGroups"
        Me.cmdDelAllGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelAllGroups.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelAllGroups.TabIndex = 95
        Me.cmdDelAllGroups.Text = "<<-"
        Me.cmdDelAllGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelAllGroups.UseVisualStyleBackColor = False
        '
        'cmdDelGroup
        '
        Me.cmdDelGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelGroup.Location = New System.Drawing.Point(294, 264)
        Me.cmdDelGroup.Name = "cmdDelGroup"
        Me.cmdDelGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelGroup.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelGroup.TabIndex = 94
        Me.cmdDelGroup.Text = "&<--"
        Me.cmdDelGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelGroup.UseVisualStyleBackColor = False
        '
        'cmdAddAllGroups
        '
        Me.cmdAddAllGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllGroups.Location = New System.Drawing.Point(294, 88)
        Me.cmdAddAllGroups.Name = "cmdAddAllGroups"
        Me.cmdAddAllGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllGroups.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddAllGroups.TabIndex = 93
        Me.cmdAddAllGroups.Text = "->>"
        Me.cmdAddAllGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAllGroups.UseVisualStyleBackColor = False
        '
        'lvwSelectedGroups
        '
        Me.lvwSelectedGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSelectedGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSelectedGroups_ColumnHeader_1})
        Me.lvwSelectedGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSelectedGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSelectedGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwSelectedGroups.HideSelection = False
        Me.lvwSelectedGroups.LargeImageList = Me.imgGroup
        Me.lvwSelectedGroups.Location = New System.Drawing.Point(360, 48)
        Me.lvwSelectedGroups.Name = "lvwSelectedGroups"
        Me.lvwSelectedGroups.Size = New System.Drawing.Size(257, 287)
        Me.lvwSelectedGroups.SmallImageList = Me.imgGroup
        Me.lvwSelectedGroups.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSelectedGroups.TabIndex = 97
        Me.lvwSelectedGroups.UseCompatibleStateImageBehavior = False
        Me.lvwSelectedGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwSelectedGroups_ColumnHeader_1
        '
        Me._lvwSelectedGroups_ColumnHeader_1.Width = 167
        '
        'lvwGroups
        '
        Me.lvwGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwGroups_ColumnHeader_1})
        Me.lvwGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwGroups.HideSelection = False
        Me.lvwGroups.Location = New System.Drawing.Point(24, 48)
        Me.lvwGroups.Name = "lvwGroups"
        Me.lvwGroups.Size = New System.Drawing.Size(257, 287)
        Me.lvwGroups.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwGroups.TabIndex = 91
        Me.lvwGroups.UseCompatibleStateImageBehavior = False
        Me.lvwGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwGroups_ColumnHeader_1
        '
        Me._lvwGroups_ColumnHeader_1.Width = 167
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(360, 30)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(116, 21)
        Me.Label10.TabIndex = 96
        Me.Label10.Text = "Selected Groups:"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(24, 30)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(116, 21)
        Me.Label11.TabIndex = 90
        Me.Label11.Text = "Available Groups:"
        '
        '_SSTab1_TabPage4
        '
        Me._SSTab1_TabPage4.Controls.Add(Me.fmeAuthorities)
        Me._SSTab1_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage4.Name = "_SSTab1_TabPage4"
        Me._SSTab1_TabPage4.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage4.TabIndex = 4
        Me._SSTab1_TabPage4.Text = "5 - Authorities"
        Me._SSTab1_TabPage4.UseVisualStyleBackColor = True
        '
        'fmeAuthorities
        '
        Me.fmeAuthorities.BackColor = System.Drawing.SystemColors.Control
        Me.fmeAuthorities.Controls.Add(Me.SSTab2)
        Me.fmeAuthorities.Controls.Add(Me._cmdPrevious_3)
        Me.fmeAuthorities.Controls.Add(Me._cmdNext_4)
        Me.fmeAuthorities.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeAuthorities.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeAuthorities.Location = New System.Drawing.Point(6, 8)
        Me.fmeAuthorities.Name = "fmeAuthorities"
        Me.fmeAuthorities.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeAuthorities.Size = New System.Drawing.Size(810, 401)
        Me.fmeAuthorities.TabIndex = 169
        Me.fmeAuthorities.TabStop = False
        '
        'SSTab2
        '
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage0)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage1)
        Me.SSTab2.Controls.Add(Me._sstab2_TabPage4)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage2)
        Me.SSTab2.Controls.Add(Me._SSTab2_TabPage3)
        Me.SSTab2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab2.ItemSize = New System.Drawing.Size(264, 18)
        Me.SSTab2.Location = New System.Drawing.Point(4, 10)
        Me.SSTab2.Multiline = True
        Me.SSTab2.Name = "SSTab2"
        Me.SSTab2.SelectedIndex = 2
        Me.SSTab2.Size = New System.Drawing.Size(801, 360)
        Me.SSTab2.TabIndex = 171
        '
        '_SSTab2_TabPage0
        '
        Me._SSTab2_TabPage0.Controls.Add(Me.Frame5)
        Me._SSTab2_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage0.Name = "_SSTab2_TabPage0"
        Me._SSTab2_TabPage0.Size = New System.Drawing.Size(793, 334)
        Me._SSTab2_TabPage0.TabIndex = 0
        Me._SSTab2_TabPage0.Text = "Accounts"
        Me._SSTab2_TabPage0.UseVisualStyleBackColor = True
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.fmeInstalmentDetail)
        Me.Frame5.Controls.Add(Me.fmeReceiptreversal)
        Me.Frame5.Controls.Add(Me.fmeAllocationReversal)
        Me.Frame5.Controls.Add(Me.fmeWriteOffs)
        Me.Frame5.Controls.Add(Me.fmeOverride)
        Me.Frame5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(8, 2)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(785, 342)
        Me.Frame5.TabIndex = 211
        Me.Frame5.TabStop = False
        '
        'fmeInstalmentDetail
        '
        Me.fmeInstalmentDetail.Controls.Add(Me.Label17)
        Me.fmeInstalmentDetail.Controls.Add(Me.txtEditInstalmentByNoofDays)
        Me.fmeInstalmentDetail.Controls.Add(Me.chkEditInstalment)
        Me.fmeInstalmentDetail.Location = New System.Drawing.Point(307, 254)
        Me.fmeInstalmentDetail.Name = "fmeInstalmentDetail"
        Me.fmeInstalmentDetail.Size = New System.Drawing.Size(294, 71)
        Me.fmeInstalmentDetail.TabIndex = 234
        Me.fmeInstalmentDetail.TabStop = False
        Me.fmeInstalmentDetail.Text = "Edit Instalment Details"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 48)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(81, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Number of days"
        '
        'txtEditInstalmentByNoofDays
        '
        Me.txtEditInstalmentByNoofDays.AcceptsReturn = True
        Me.txtEditInstalmentByNoofDays.Enabled = False
        Me.txtEditInstalmentByNoofDays.Location = New System.Drawing.Point(190, 48)
        Me.txtEditInstalmentByNoofDays.Name = "txtEditInstalmentByNoofDays"
        Me.txtEditInstalmentByNoofDays.Size = New System.Drawing.Size(93, 20)
        Me.txtEditInstalmentByNoofDays.TabIndex = 1
        '
        'chkEditInstalment
        '
        Me.chkEditInstalment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditInstalment.Location = New System.Drawing.Point(8, 20)
        Me.chkEditInstalment.Name = "chkEditInstalment"
        Me.chkEditInstalment.Size = New System.Drawing.Size(275, 22)
        Me.chkEditInstalment.TabIndex = 0
        Me.chkEditInstalment.Text = "User can edit instalment due dates"
        Me.chkEditInstalment.UseVisualStyleBackColor = True
        '
        'fmeReceiptreversal
        '
        Me.fmeReceiptreversal.BackColor = System.Drawing.SystemColors.Control
        Me.fmeReceiptreversal.Controls.Add(Me.chkReceiptReversal)
        Me.fmeReceiptreversal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeReceiptreversal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeReceiptreversal.Location = New System.Drawing.Point(7, 266)
        Me.fmeReceiptreversal.Name = "fmeReceiptreversal"
        Me.fmeReceiptreversal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeReceiptreversal.Size = New System.Drawing.Size(294, 60)
        Me.fmeReceiptreversal.TabIndex = 234
        Me.fmeReceiptreversal.TabStop = False
        Me.fmeReceiptreversal.Text = "Receipt Reversal"
        '
        'chkReceiptReversal
        '
        Me.chkReceiptReversal.BackColor = System.Drawing.SystemColors.Control
        Me.chkReceiptReversal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReceiptReversal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReceiptReversal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReceiptReversal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReceiptReversal.Location = New System.Drawing.Point(8, 28)
        Me.chkReceiptReversal.Name = "chkReceiptReversal"
        Me.chkReceiptReversal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReceiptReversal.Size = New System.Drawing.Size(275, 17)
        Me.chkReceiptReversal.TabIndex = 197
        Me.chkReceiptReversal.Text = "User has Authority to Reverse the Receipts (SRP)"
        Me.chkReceiptReversal.UseVisualStyleBackColor = False
        '
        'fmeAllocationReversal
        '
        Me.fmeAllocationReversal.BackColor = System.Drawing.SystemColors.Control
        Me.fmeAllocationReversal.Controls.Add(Me.txtTimePeriod)
        Me.fmeAllocationReversal.Controls.Add(Me.chkReverseAllocation)
        Me.fmeAllocationReversal.Controls.Add(Me.lblTimePeriod)
        Me.fmeAllocationReversal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeAllocationReversal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeAllocationReversal.Location = New System.Drawing.Point(6, 186)
        Me.fmeAllocationReversal.Name = "fmeAllocationReversal"
        Me.fmeAllocationReversal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeAllocationReversal.Size = New System.Drawing.Size(295, 80)
        Me.fmeAllocationReversal.TabIndex = 233
        Me.fmeAllocationReversal.TabStop = False
        Me.fmeAllocationReversal.Text = "Allocation Reversal"
        '
        'txtTimePeriod
        '
        Me.txtTimePeriod.AcceptsReturn = True
        Me.txtTimePeriod.BackColor = System.Drawing.SystemColors.Window
        Me.txtTimePeriod.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTimePeriod.Enabled = False
        Me.txtTimePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTimePeriod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTimePeriod.Location = New System.Drawing.Point(190, 52)
        Me.txtTimePeriod.MaxLength = 0
        Me.txtTimePeriod.Name = "txtTimePeriod"
        Me.txtTimePeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTimePeriod.Size = New System.Drawing.Size(93, 20)
        Me.txtTimePeriod.TabIndex = 198
        '
        'chkReverseAllocation
        '
        Me.chkReverseAllocation.BackColor = System.Drawing.SystemColors.Control
        Me.chkReverseAllocation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkReverseAllocation.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReverseAllocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReverseAllocation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReverseAllocation.Location = New System.Drawing.Point(8, 28)
        Me.chkReverseAllocation.Name = "chkReverseAllocation"
        Me.chkReverseAllocation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReverseAllocation.Size = New System.Drawing.Size(275, 17)
        Me.chkReverseAllocation.TabIndex = 197
        Me.chkReverseAllocation.Text = "User has Authority to Reverse Allocations"
        Me.chkReverseAllocation.UseVisualStyleBackColor = False
        '
        'lblTimePeriod
        '
        Me.lblTimePeriod.BackColor = System.Drawing.SystemColors.Control
        Me.lblTimePeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTimePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimePeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTimePeriod.Location = New System.Drawing.Point(8, 54)
        Me.lblTimePeriod.Name = "lblTimePeriod"
        Me.lblTimePeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTimePeriod.Size = New System.Drawing.Size(177, 17)
        Me.lblTimePeriod.TabIndex = 234
        Me.lblTimePeriod.Text = "Time Period (Number of Days)"
        '
        'fmeWriteOffs
        '
        Me.fmeWriteOffs.BackColor = System.Drawing.SystemColors.Control
        Me.fmeWriteOffs.Controls.Add(Me.cboLossGainCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.lblLossGainCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.txtCurrencyLossGainLimit)
        Me.fmeWriteOffs.Controls.Add(Me.lblCurrencyAmount)
        Me.fmeWriteOffs.Controls.Add(Me.lblCurrencyWriteoff)
        Me.fmeWriteOffs.Controls.Add(Me.chkWriteOffs)
        Me.fmeWriteOffs.Controls.Add(Me.txtWriteOff)
        Me.fmeWriteOffs.Controls.Add(Me.cboWriteOffsCurrency)
        Me.fmeWriteOffs.Controls.Add(Me.lblWriteoffAmount)
        Me.fmeWriteOffs.Controls.Add(Me.lblWriteoffText)
        Me.fmeWriteOffs.Controls.Add(Me.lblWriteOffsCurrency)
        Me.fmeWriteOffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeWriteOffs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeWriteOffs.Location = New System.Drawing.Point(6, 10)
        Me.fmeWriteOffs.Name = "fmeWriteOffs"
        Me.fmeWriteOffs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeWriteOffs.Size = New System.Drawing.Size(295, 174)
        Me.fmeWriteOffs.TabIndex = 220
        Me.fmeWriteOffs.TabStop = False
        Me.fmeWriteOffs.Text = "Write-offs"
        '
        'cboLossGainCurrency
        '
        Me.cboLossGainCurrency.CompanyId = 0
        Me.cboLossGainCurrency.CurrencyId = 0
        Me.cboLossGainCurrency.DefaultCurrencyId = 0
        Me.cboLossGainCurrency.FirstItem = ""
        Me.cboLossGainCurrency.ListIndex = -1
        Me.cboLossGainCurrency.Location = New System.Drawing.Point(92, 118)
        Me.cboLossGainCurrency.Name = "cboLossGainCurrency"
        Me.cboLossGainCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboLossGainCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboLossGainCurrency.TabIndex = 233
        Me.cboLossGainCurrency.ToolTipText = ""
        Me.cboLossGainCurrency.WhatsThisHelpID = 0
        '
        'lblLossGainCurrency
        '
        Me.lblLossGainCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblLossGainCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLossGainCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLossGainCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLossGainCurrency.Location = New System.Drawing.Point(12, 122)
        Me.lblLossGainCurrency.Name = "lblLossGainCurrency"
        Me.lblLossGainCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLossGainCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblLossGainCurrency.TabIndex = 232
        Me.lblLossGainCurrency.Text = "Currency:"
        '
        'txtCurrencyLossGainLimit
        '
        Me.txtCurrencyLossGainLimit.AcceptsReturn = True
        Me.txtCurrencyLossGainLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrencyLossGainLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyLossGainLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyLossGainLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyLossGainLimit.Location = New System.Drawing.Point(92, 145)
        Me.txtCurrencyLossGainLimit.MaxLength = 0
        Me.txtCurrencyLossGainLimit.Name = "txtCurrencyLossGainLimit"
        Me.txtCurrencyLossGainLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyLossGainLimit.Size = New System.Drawing.Size(101, 20)
        Me.txtCurrencyLossGainLimit.TabIndex = 231
        '
        'lblCurrencyAmount
        '
        Me.lblCurrencyAmount.AutoSize = True
        Me.lblCurrencyAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrencyAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrencyAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrencyAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrencyAmount.Location = New System.Drawing.Point(12, 145)
        Me.lblCurrencyAmount.Name = "lblCurrencyAmount"
        Me.lblCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrencyAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblCurrencyAmount.TabIndex = 230
        Me.lblCurrencyAmount.Text = "A&mount:"
        '
        'lblCurrencyWriteoff
        '
        Me.lblCurrencyWriteoff.AutoSize = True
        Me.lblCurrencyWriteoff.Location = New System.Drawing.Point(10, 99)
        Me.lblCurrencyWriteoff.Name = "lblCurrencyWriteoff"
        Me.lblCurrencyWriteoff.Size = New System.Drawing.Size(168, 13)
        Me.lblCurrencyWriteoff.TabIndex = 229
        Me.lblCurrencyWriteoff.Text = "Currency Loss/Gain Write-off Limit"
        '
        'chkWriteOffs
        '
        Me.chkWriteOffs.BackColor = System.Drawing.SystemColors.Control
        Me.chkWriteOffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkWriteOffs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkWriteOffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWriteOffs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWriteOffs.Location = New System.Drawing.Point(10, 21)
        Me.chkWriteOffs.Name = "chkWriteOffs"
        Me.chkWriteOffs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkWriteOffs.Size = New System.Drawing.Size(240, 17)
        Me.chkWriteOffs.TabIndex = 187
        Me.chkWriteOffs.Text = "Use&r can perform allocation write-offs"
        Me.chkWriteOffs.UseVisualStyleBackColor = False
        '
        'txtWriteOff
        '
        Me.txtWriteOff.AcceptsReturn = True
        Me.txtWriteOff.BackColor = System.Drawing.SystemColors.Window
        Me.txtWriteOff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWriteOff.Enabled = False
        Me.txtWriteOff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWriteOff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWriteOff.Location = New System.Drawing.Point(92, 72)
        Me.txtWriteOff.MaxLength = 0
        Me.txtWriteOff.Name = "txtWriteOff"
        Me.txtWriteOff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWriteOff.Size = New System.Drawing.Size(101, 20)
        Me.txtWriteOff.TabIndex = 189
        '
        'cboWriteOffsCurrency
        '
        Me.cboWriteOffsCurrency.CompanyId = 0
        Me.cboWriteOffsCurrency.CurrencyId = 0
        Me.cboWriteOffsCurrency.DefaultCurrencyId = 0
        Me.cboWriteOffsCurrency.FirstItem = ""
        Me.cboWriteOffsCurrency.ListIndex = -1
        Me.cboWriteOffsCurrency.Location = New System.Drawing.Point(92, 46)
        Me.cboWriteOffsCurrency.Name = "cboWriteOffsCurrency"
        Me.cboWriteOffsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboWriteOffsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboWriteOffsCurrency.TabIndex = 188
        Me.cboWriteOffsCurrency.ToolTipText = ""
        Me.cboWriteOffsCurrency.WhatsThisHelpID = 0
        '
        'lblWriteoffAmount
        '
        Me.lblWriteoffAmount.AutoSize = True
        Me.lblWriteoffAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteoffAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteoffAmount.Enabled = False
        Me.lblWriteoffAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteoffAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteoffAmount.Location = New System.Drawing.Point(12, 76)
        Me.lblWriteoffAmount.Name = "lblWriteoffAmount"
        Me.lblWriteoffAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteoffAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblWriteoffAmount.TabIndex = 228
        Me.lblWriteoffAmount.Text = "A&mount:"
        '
        'lblWriteoffText
        '
        Me.lblWriteoffText.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteoffText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteoffText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteoffText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteoffText.Location = New System.Drawing.Point(262, 22)
        Me.lblWriteoffText.Name = "lblWriteoffText"
        Me.lblWriteoffText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteoffText.Size = New System.Drawing.Size(26, 13)
        Me.lblWriteoffText.TabIndex = 227
        Me.lblWriteoffText.Text = "No"
        '
        'lblWriteOffsCurrency
        '
        Me.lblWriteOffsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffsCurrency.Location = New System.Drawing.Point(12, 48)
        Me.lblWriteOffsCurrency.Name = "lblWriteOffsCurrency"
        Me.lblWriteOffsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblWriteOffsCurrency.TabIndex = 224
        Me.lblWriteOffsCurrency.Text = "Currency:"
        '
        'fmeOverride
        '
        Me.fmeOverride.BackColor = System.Drawing.SystemColors.Control
        Me.fmeOverride.Controls.Add(Me.chkInstalmentStatus)
        Me.fmeOverride.Controls.Add(Me.chkOverrideCollectionDate)
        Me.fmeOverride.Controls.Add(Me.chkOverrideChequeNumber)
        Me.fmeOverride.Controls.Add(Me.chkPostingPeriod)
        Me.fmeOverride.Controls.Add(Me.chkDuplicateClaimOverride)
        Me.fmeOverride.Controls.Add(Me.chkOverridePrePolicyDate)
        Me.fmeOverride.Controls.Add(Me.chkOverridePrePolicyRate)
        Me.fmeOverride.Controls.Add(Me.chkOverrideDate)
        Me.fmeOverride.Controls.Add(Me.chkOverrideRate)
        Me.fmeOverride.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeOverride.Location = New System.Drawing.Point(304, 10)
        Me.fmeOverride.Name = "fmeOverride"
        Me.fmeOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeOverride.Size = New System.Drawing.Size(317, 235)
        Me.fmeOverride.TabIndex = 213
        Me.fmeOverride.TabStop = False
        Me.fmeOverride.Text = "User can override"
        '
        'chkInstalmentStatus
        '
        Me.chkInstalmentStatus.BackColor = System.Drawing.SystemColors.Control
        Me.chkInstalmentStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInstalmentStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInstalmentStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInstalmentStatus.Location = New System.Drawing.Point(16, 204)
        Me.chkInstalmentStatus.Name = "chkInstalmentStatus"
        Me.chkInstalmentStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInstalmentStatus.Size = New System.Drawing.Size(285, 25)
        Me.chkInstalmentStatus.TabIndex = 243
        Me.chkInstalmentStatus.Text = "Instalment Status"
        Me.chkInstalmentStatus.UseVisualStyleBackColor = False
        '
        'chkOverrideCollectionDate
        '
        Me.chkOverrideCollectionDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideCollectionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideCollectionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideCollectionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideCollectionDate.Location = New System.Drawing.Point(16, 182)
        Me.chkOverrideCollectionDate.Name = "chkOverrideCollectionDate"
        Me.chkOverrideCollectionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideCollectionDate.Size = New System.Drawing.Size(285, 25)
        Me.chkOverrideCollectionDate.TabIndex = 243
        Me.chkOverrideCollectionDate.Text = "Collection Date on Cash/Cheque receipt"
        Me.chkOverrideCollectionDate.UseVisualStyleBackColor = False
        '
        'chkOverrideChequeNumber
        '
        Me.chkOverrideChequeNumber.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideChequeNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideChequeNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideChequeNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideChequeNumber.Location = New System.Drawing.Point(16, 160)
        Me.chkOverrideChequeNumber.Name = "chkOverrideChequeNumber"
        Me.chkOverrideChequeNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideChequeNumber.Size = New System.Drawing.Size(217, 25)
        Me.chkOverrideChequeNumber.TabIndex = 196
        Me.chkOverrideChequeNumber.Text = "Cheque Numbers"
        Me.chkOverrideChequeNumber.UseVisualStyleBackColor = False
        '
        'chkPostingPeriod
        '
        Me.chkPostingPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.chkPostingPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPostingPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPostingPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPostingPeriod.Location = New System.Drawing.Point(16, 136)
        Me.chkPostingPeriod.Name = "chkPostingPeriod"
        Me.chkPostingPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPostingPeriod.Size = New System.Drawing.Size(217, 25)
        Me.chkPostingPeriod.TabIndex = 195
        Me.chkPostingPeriod.Text = "Posting Period"
        Me.chkPostingPeriod.UseVisualStyleBackColor = False
        '
        'chkDuplicateClaimOverride
        '
        Me.chkDuplicateClaimOverride.BackColor = System.Drawing.SystemColors.Control
        Me.chkDuplicateClaimOverride.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDuplicateClaimOverride.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDuplicateClaimOverride.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDuplicateClaimOverride.Location = New System.Drawing.Point(16, 112)
        Me.chkDuplicateClaimOverride.Name = "chkDuplicateClaimOverride"
        Me.chkDuplicateClaimOverride.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDuplicateClaimOverride.Size = New System.Drawing.Size(217, 25)
        Me.chkDuplicateClaimOverride.TabIndex = 194
        Me.chkDuplicateClaimOverride.Text = "Duplicate Claim Override"
        Me.chkDuplicateClaimOverride.UseVisualStyleBackColor = False
        '
        'chkOverridePrePolicyDate
        '
        Me.chkOverridePrePolicyDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverridePrePolicyDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverridePrePolicyDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverridePrePolicyDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverridePrePolicyDate.Location = New System.Drawing.Point(16, 14)
        Me.chkOverridePrePolicyDate.Name = "chkOverridePrePolicyDate"
        Me.chkOverridePrePolicyDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverridePrePolicyDate.Size = New System.Drawing.Size(217, 25)
        Me.chkOverridePrePolicyDate.TabIndex = 190
        Me.chkOverridePrePolicyDate.Text = "Exchange Date on Policy Screen"
        Me.chkOverridePrePolicyDate.UseVisualStyleBackColor = False
        '
        'chkOverridePrePolicyRate
        '
        Me.chkOverridePrePolicyRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverridePrePolicyRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverridePrePolicyRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverridePrePolicyRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverridePrePolicyRate.Location = New System.Drawing.Point(16, 37)
        Me.chkOverridePrePolicyRate.Name = "chkOverridePrePolicyRate"
        Me.chkOverridePrePolicyRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverridePrePolicyRate.Size = New System.Drawing.Size(217, 25)
        Me.chkOverridePrePolicyRate.TabIndex = 191
        Me.chkOverridePrePolicyRate.Text = "Exchange Rate on Policy Screen"
        Me.chkOverridePrePolicyRate.UseVisualStyleBackColor = False
        '
        'chkOverrideDate
        '
        Me.chkOverrideDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideDate.Location = New System.Drawing.Point(16, 62)
        Me.chkOverrideDate.Name = "chkOverrideDate"
        Me.chkOverrideDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideDate.Size = New System.Drawing.Size(293, 25)
        Me.chkOverrideDate.TabIndex = 192
        Me.chkOverrideDate.Text = "Exchange Date on other multi-currency screens"
        Me.chkOverrideDate.UseVisualStyleBackColor = False
        '
        'chkOverrideRate
        '
        Me.chkOverrideRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOverrideRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOverrideRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOverrideRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOverrideRate.Location = New System.Drawing.Point(16, 87)
        Me.chkOverrideRate.Name = "chkOverrideRate"
        Me.chkOverrideRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOverrideRate.Size = New System.Drawing.Size(297, 25)
        Me.chkOverrideRate.TabIndex = 193
        Me.chkOverrideRate.Text = "Exchange Rate on other multi-currency screens"
        Me.chkOverrideRate.UseVisualStyleBackColor = False
        '
        '_SSTab2_TabPage1
        '
        Me._SSTab2_TabPage1.Controls.Add(Me.Frame8)
        Me._SSTab2_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage1.Name = "_SSTab2_TabPage1"
        Me._SSTab2_TabPage1.Size = New System.Drawing.Size(793, 334)
        Me._SSTab2_TabPage1.TabIndex = 1
        Me._SSTab2_TabPage1.Text = "Payments"
        Me._SSTab2_TabPage1.UseVisualStyleBackColor = True
        '
        'Frame8
        '
        Me.Frame8.BackColor = System.Drawing.SystemColors.Control
        Me.Frame8.Controls.Add(Me.Frame10)
        Me.Frame8.Controls.Add(Me.fmePayments)
        Me.Frame8.Controls.Add(Me.fmeClaimPayments)
        Me.Frame8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame8.Location = New System.Drawing.Point(6, 4)
        Me.Frame8.Name = "Frame8"
        Me.Frame8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame8.Size = New System.Drawing.Size(787, 327)
        Me.Frame8.TabIndex = 183
        Me.Frame8.TabStop = False
        '
        'Frame10
        '
        Me.Frame10.BackColor = System.Drawing.SystemColors.Control
        Me.Frame10.Controls.Add(Me.txtRecommendAmount)
        Me.Frame10.Controls.Add(Me.chkRecommender)
        Me.Frame10.Controls.Add(Me.cboRecommandationCurrency)
        Me.Frame10.Controls.Add(Me.lblRecommenderAmount)
        Me.Frame10.Controls.Add(Me.lblRecommenderCurrency)
        Me.Frame10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame10.Location = New System.Drawing.Point(6, 132)
        Me.Frame10.Name = "Frame10"
        Me.Frame10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame10.Size = New System.Drawing.Size(293, 112)
        Me.Frame10.TabIndex = 230
        Me.Frame10.TabStop = False
        Me.Frame10.Text = "Recommend Payments"
        '
        'txtRecommendAmount
        '
        Me.txtRecommendAmount.AcceptsReturn = True
        Me.txtRecommendAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecommendAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRecommendAmount.Enabled = False
        Me.txtRecommendAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecommendAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRecommendAmount.Location = New System.Drawing.Point(108, 74)
        Me.txtRecommendAmount.MaxLength = 0
        Me.txtRecommendAmount.Name = "txtRecommendAmount"
        Me.txtRecommendAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRecommendAmount.Size = New System.Drawing.Size(175, 20)
        Me.txtRecommendAmount.TabIndex = 205
        '
        'chkRecommender
        '
        Me.chkRecommender.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecommender.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRecommender.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecommender.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecommender.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecommender.Location = New System.Drawing.Point(16, 22)
        Me.chkRecommender.Name = "chkRecommender"
        Me.chkRecommender.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecommender.Size = New System.Drawing.Size(265, 25)
        Me.chkRecommender.TabIndex = 204
        Me.chkRecommender.Text = "Recommender"
        Me.chkRecommender.UseVisualStyleBackColor = False
        '
        'cboRecommandationCurrency
        '
        Me.cboRecommandationCurrency.CompanyId = 0
        Me.cboRecommandationCurrency.CurrencyId = 0
        Me.cboRecommandationCurrency.DefaultCurrencyId = 0
        Me.cboRecommandationCurrency.FirstItem = ""
        Me.cboRecommandationCurrency.ListIndex = -1
        Me.cboRecommandationCurrency.Location = New System.Drawing.Point(108, 50)
        Me.cboRecommandationCurrency.Name = "cboRecommandationCurrency"
        Me.cboRecommandationCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboRecommandationCurrency.Size = New System.Drawing.Size(177, 21)
        Me.cboRecommandationCurrency.TabIndex = 206
        Me.cboRecommandationCurrency.ToolTipText = ""
        Me.cboRecommandationCurrency.WhatsThisHelpID = 0
        '
        'lblRecommenderAmount
        '
        Me.lblRecommenderAmount.AutoSize = True
        Me.lblRecommenderAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecommenderAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecommenderAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecommenderAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecommenderAmount.Location = New System.Drawing.Point(16, 79)
        Me.lblRecommenderAmount.Name = "lblRecommenderAmount"
        Me.lblRecommenderAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecommenderAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblRecommenderAmount.TabIndex = 232
        Me.lblRecommenderAmount.Text = "Amount:"
        '
        'lblRecommenderCurrency
        '
        Me.lblRecommenderCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblRecommenderCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRecommenderCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecommenderCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRecommenderCurrency.Location = New System.Drawing.Point(16, 53)
        Me.lblRecommenderCurrency.Name = "lblRecommenderCurrency"
        Me.lblRecommenderCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRecommenderCurrency.Size = New System.Drawing.Size(57, 17)
        Me.lblRecommenderCurrency.TabIndex = 231
        Me.lblRecommenderCurrency.Text = "Currency:"
        '
        'fmePayments
        '
        Me.fmePayments.BackColor = System.Drawing.SystemColors.Control
        Me.fmePayments.Controls.Add(Me.chkPayments)
        Me.fmePayments.Controls.Add(Me.txtPayments)
        Me.fmePayments.Controls.Add(Me.cboPaymentsCurrency)
        Me.fmePayments.Controls.Add(Me.lblPaymentsCurrency)
        Me.fmePayments.Controls.Add(Me.lblPayments)
        Me.fmePayments.Controls.Add(Me.lblPaymentsText)
        Me.fmePayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmePayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmePayments.Location = New System.Drawing.Point(6, 8)
        Me.fmePayments.Name = "fmePayments"
        Me.fmePayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmePayments.Size = New System.Drawing.Size(293, 124)
        Me.fmePayments.TabIndex = 199
        Me.fmePayments.TabStop = False
        Me.fmePayments.Text = "Payments"
        '
        'chkPayments
        '
        Me.chkPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkPayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPayments.Location = New System.Drawing.Point(16, 21)
        Me.chkPayments.Name = "chkPayments"
        Me.chkPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPayments.Size = New System.Drawing.Size(173, 17)
        Me.chkPayments.TabIndex = 200
        Me.chkPayments.Text = "User has a &Payment Limit"
        Me.chkPayments.UseVisualStyleBackColor = False
        '
        'txtPayments
        '
        Me.txtPayments.AcceptsReturn = True
        Me.txtPayments.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayments.Location = New System.Drawing.Point(96, 72)
        Me.txtPayments.MaxLength = 0
        Me.txtPayments.Name = "txtPayments"
        Me.txtPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayments.Size = New System.Drawing.Size(157, 20)
        Me.txtPayments.TabIndex = 203
        '
        'cboPaymentsCurrency
        '
        Me.cboPaymentsCurrency.CompanyId = 0
        Me.cboPaymentsCurrency.CurrencyId = 0
        Me.cboPaymentsCurrency.DefaultCurrencyId = 0
        Me.cboPaymentsCurrency.FirstItem = ""
        Me.cboPaymentsCurrency.ListIndex = -1
        Me.cboPaymentsCurrency.Location = New System.Drawing.Point(96, 48)
        Me.cboPaymentsCurrency.Name = "cboPaymentsCurrency"
        Me.cboPaymentsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboPaymentsCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboPaymentsCurrency.TabIndex = 201
        Me.cboPaymentsCurrency.ToolTipText = ""
        Me.cboPaymentsCurrency.WhatsThisHelpID = 0
        '
        'lblPaymentsCurrency
        '
        Me.lblPaymentsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentsCurrency.Location = New System.Drawing.Point(16, 50)
        Me.lblPaymentsCurrency.Name = "lblPaymentsCurrency"
        Me.lblPaymentsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentsCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblPaymentsCurrency.TabIndex = 209
        Me.lblPaymentsCurrency.Text = "Currency:"
        '
        'lblPayments
        '
        Me.lblPayments.AutoSize = True
        Me.lblPayments.BackColor = System.Drawing.SystemColors.Control
        Me.lblPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPayments.Location = New System.Drawing.Point(16, 74)
        Me.lblPayments.Name = "lblPayments"
        Me.lblPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPayments.Size = New System.Drawing.Size(46, 13)
        Me.lblPayments.TabIndex = 207
        Me.lblPayments.Text = "Amount:"
        '
        'lblPaymentsText
        '
        Me.lblPaymentsText.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentsText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentsText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentsText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentsText.Location = New System.Drawing.Point(202, 22)
        Me.lblPaymentsText.Name = "lblPaymentsText"
        Me.lblPaymentsText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentsText.Size = New System.Drawing.Size(26, 13)
        Me.lblPaymentsText.TabIndex = 202
        Me.lblPaymentsText.Text = "No"
        '
        'fmeClaimPayments
        '
        Me.fmeClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.fmeClaimPayments.Controls.Add(Me.chkClaimPayments)
        Me.fmeClaimPayments.Controls.Add(Me.txtClaimPayments)
        Me.fmeClaimPayments.Controls.Add(Me.chkUserCanChangeReserves)
        Me.fmeClaimPayments.Controls.Add(Me.cboClaimPaymentsCurrency)
        Me.fmeClaimPayments.Controls.Add(Me.lblClaimPaymentsCurrency)
        Me.fmeClaimPayments.Controls.Add(Me.lblClaimPayments)
        Me.fmeClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeClaimPayments.Location = New System.Drawing.Point(302, 8)
        Me.fmeClaimPayments.Name = "fmeClaimPayments"
        Me.fmeClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeClaimPayments.Size = New System.Drawing.Size(293, 124)
        Me.fmeClaimPayments.TabIndex = 184
        Me.fmeClaimPayments.TabStop = False
        Me.fmeClaimPayments.Text = "Claim Payments"
        '
        'chkClaimPayments
        '
        Me.chkClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimPayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimPayments.Location = New System.Drawing.Point(16, 19)
        Me.chkClaimPayments.Name = "chkClaimPayments"
        Me.chkClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimPayments.Size = New System.Drawing.Size(273, 25)
        Me.chkClaimPayments.TabIndex = 208
        Me.chkClaimPayments.Text = "User has &Claim Payments authority"
        Me.chkClaimPayments.UseVisualStyleBackColor = False
        '
        'txtClaimPayments
        '
        Me.txtClaimPayments.AcceptsReturn = True
        Me.txtClaimPayments.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaimPayments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaimPayments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaimPayments.Location = New System.Drawing.Point(108, 75)
        Me.txtClaimPayments.MaxLength = 0
        Me.txtClaimPayments.Name = "txtClaimPayments"
        Me.txtClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaimPayments.Size = New System.Drawing.Size(175, 20)
        Me.txtClaimPayments.TabIndex = 212
        '
        'chkUserCanChangeReserves
        '
        Me.chkUserCanChangeReserves.BackColor = System.Drawing.SystemColors.Control
        Me.chkUserCanChangeReserves.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUserCanChangeReserves.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUserCanChangeReserves.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUserCanChangeReserves.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUserCanChangeReserves.Location = New System.Drawing.Point(16, 101)
        Me.chkUserCanChangeReserves.Name = "chkUserCanChangeReserves"
        Me.chkUserCanChangeReserves.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUserCanChangeReserves.Size = New System.Drawing.Size(217, 17)
        Me.chkUserCanChangeReserves.TabIndex = 214
        Me.chkUserCanChangeReserves.Text = "User Can Change Reserves"
        Me.chkUserCanChangeReserves.UseVisualStyleBackColor = False
        '
        'cboClaimPaymentsCurrency
        '
        Me.cboClaimPaymentsCurrency.CompanyId = 0
        Me.cboClaimPaymentsCurrency.CurrencyId = 0
        Me.cboClaimPaymentsCurrency.DefaultCurrencyId = 0
        Me.cboClaimPaymentsCurrency.FirstItem = ""
        Me.cboClaimPaymentsCurrency.ListIndex = -1
        Me.cboClaimPaymentsCurrency.Location = New System.Drawing.Point(110, 50)
        Me.cboClaimPaymentsCurrency.Name = "cboClaimPaymentsCurrency"
        Me.cboClaimPaymentsCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboClaimPaymentsCurrency.Size = New System.Drawing.Size(175, 21)
        Me.cboClaimPaymentsCurrency.TabIndex = 210
        Me.cboClaimPaymentsCurrency.ToolTipText = ""
        Me.cboClaimPaymentsCurrency.WhatsThisHelpID = 0
        '
        'lblClaimPaymentsCurrency
        '
        Me.lblClaimPaymentsCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimPaymentsCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimPaymentsCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimPaymentsCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimPaymentsCurrency.Location = New System.Drawing.Point(16, 53)
        Me.lblClaimPaymentsCurrency.Name = "lblClaimPaymentsCurrency"
        Me.lblClaimPaymentsCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimPaymentsCurrency.Size = New System.Drawing.Size(57, 17)
        Me.lblClaimPaymentsCurrency.TabIndex = 186
        Me.lblClaimPaymentsCurrency.Text = "Currency:"
        '
        'lblClaimPayments
        '
        Me.lblClaimPayments.AutoSize = True
        Me.lblClaimPayments.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimPayments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimPayments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaimPayments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimPayments.Location = New System.Drawing.Point(16, 79)
        Me.lblClaimPayments.Name = "lblClaimPayments"
        Me.lblClaimPayments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimPayments.Size = New System.Drawing.Size(46, 13)
        Me.lblClaimPayments.TabIndex = 185
        Me.lblClaimPayments.Text = "Amount:"
        '
        '_sstab2_TabPage4
        '
        Me._sstab2_TabPage4.Controls.Add(Me.fmeManualjournal)
        Me._sstab2_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._sstab2_TabPage4.Name = "_sstab2_TabPage4"
        Me._sstab2_TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me._sstab2_TabPage4.Size = New System.Drawing.Size(793, 334)
        Me._sstab2_TabPage4.TabIndex = 4
        Me._sstab2_TabPage4.Text = "Manual Journal"
        Me._sstab2_TabPage4.UseVisualStyleBackColor = True
        '
        'fmeManualjournal
        '
        Me.fmeManualjournal.BackColor = System.Drawing.SystemColors.Control
        Me.fmeManualjournal.Controls.Add(Me.ChkManualJournal)
        Me.fmeManualjournal.Controls.Add(Me.txtjournalAmount)
        Me.fmeManualjournal.Controls.Add(Me.cboJournalCurrency)
        Me.fmeManualjournal.Controls.Add(Me.lblJournalCurrency)
        Me.fmeManualjournal.Controls.Add(Me.lblJournalAmount)
        Me.fmeManualjournal.Controls.Add(Me.lblmanulJournal)
        Me.fmeManualjournal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeManualjournal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeManualjournal.Location = New System.Drawing.Point(6, 6)
        Me.fmeManualjournal.Name = "fmeManualjournal"
        Me.fmeManualjournal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeManualjournal.Size = New System.Drawing.Size(790, 328)
        Me.fmeManualjournal.TabIndex = 200
        Me.fmeManualjournal.TabStop = False
        Me.fmeManualjournal.Text = "Manual Journal"
        '
        'ChkManualJournal
        '
        Me.ChkManualJournal.BackColor = System.Drawing.SystemColors.Control
        Me.ChkManualJournal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkManualJournal.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkManualJournal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkManualJournal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkManualJournal.Location = New System.Drawing.Point(16, 21)
        Me.ChkManualJournal.Name = "ChkManualJournal"
        Me.ChkManualJournal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkManualJournal.Size = New System.Drawing.Size(208, 28)
        Me.ChkManualJournal.TabIndex = 200
        Me.ChkManualJournal.Text = "Manual Journal Authorisation Limit"
        Me.ChkManualJournal.UseVisualStyleBackColor = False
        '
        'txtjournalAmount
        '
        Me.txtjournalAmount.AcceptsReturn = True
        Me.txtjournalAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtjournalAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtjournalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtjournalAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtjournalAmount.Location = New System.Drawing.Point(96, 72)
        Me.txtjournalAmount.MaxLength = 0
        Me.txtjournalAmount.Name = "txtjournalAmount"
        Me.txtjournalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtjournalAmount.Size = New System.Drawing.Size(157, 20)
        Me.txtjournalAmount.TabIndex = 203
        '
        'cboJournalCurrency
        '
        Me.cboJournalCurrency.CompanyId = 0
        Me.cboJournalCurrency.CurrencyId = 0
        Me.cboJournalCurrency.DefaultCurrencyId = 0
        Me.cboJournalCurrency.FirstItem = ""
        Me.cboJournalCurrency.ListIndex = -1
        Me.cboJournalCurrency.Location = New System.Drawing.Point(96, 48)
        Me.cboJournalCurrency.Name = "cboJournalCurrency"
        Me.cboJournalCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboJournalCurrency.Size = New System.Drawing.Size(161, 21)
        Me.cboJournalCurrency.TabIndex = 201
        Me.cboJournalCurrency.ToolTipText = ""
        Me.cboJournalCurrency.WhatsThisHelpID = 0
        '
        'lblJournalCurrency
        '
        Me.lblJournalCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblJournalCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblJournalCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJournalCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblJournalCurrency.Location = New System.Drawing.Point(16, 50)
        Me.lblJournalCurrency.Name = "lblJournalCurrency"
        Me.lblJournalCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblJournalCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblJournalCurrency.TabIndex = 209
        Me.lblJournalCurrency.Text = "Currency:"
        '
        'lblJournalAmount
        '
        Me.lblJournalAmount.AutoSize = True
        Me.lblJournalAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblJournalAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblJournalAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblJournalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblJournalAmount.Location = New System.Drawing.Point(16, 74)
        Me.lblJournalAmount.Name = "lblJournalAmount"
        Me.lblJournalAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblJournalAmount.Size = New System.Drawing.Size(46, 13)
        Me.lblJournalAmount.TabIndex = 207
        Me.lblJournalAmount.Text = "Amount:"
        '
        'lblmanulJournal
        '
        Me.lblmanulJournal.BackColor = System.Drawing.SystemColors.Control
        Me.lblmanulJournal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblmanulJournal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmanulJournal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblmanulJournal.Location = New System.Drawing.Point(231, 24)
        Me.lblmanulJournal.Name = "lblmanulJournal"
        Me.lblmanulJournal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblmanulJournal.Size = New System.Drawing.Size(26, 13)
        Me.lblmanulJournal.TabIndex = 202
        Me.lblmanulJournal.Text = "No"
        '
        '_SSTab2_TabPage2
        '
        Me._SSTab2_TabPage2.Controls.Add(Me.Frame9)
        Me._SSTab2_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage2.Name = "_SSTab2_TabPage2"
        Me._SSTab2_TabPage2.Size = New System.Drawing.Size(793, 334)
        Me._SSTab2_TabPage2.TabIndex = 2
        Me._SSTab2_TabPage2.Text = "Policy"
        Me._SSTab2_TabPage2.UseVisualStyleBackColor = True
        '
        'Frame9
        '
        Me.Frame9.BackColor = System.Drawing.SystemColors.Control
        Me.Frame9.Controls.Add(Me.fraReinsurance)
        Me.Frame9.Controls.Add(Me.fraRatingSections)
        Me.Frame9.Controls.Add(Me.fraMakeLive)
        Me.Frame9.Controls.Add(Me.fmeAccess)
        Me.Frame9.Controls.Add(Me.fraBrokerAgentPortfolioTransfer)
        Me.Frame9.Controls.Add(Me.fraVoidTransaction)
        Me.Frame9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame9.Location = New System.Drawing.Point(8, 4)
        Me.Frame9.Name = "Frame9"
        Me.Frame9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame9.Size = New System.Drawing.Size(785, 327)
        Me.Frame9.TabIndex = 172
        Me.Frame9.TabStop = False
        '
        'fraReinsurance
        '
        Me.fraReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.fraReinsurance.Controls.Add(Me.chkDisplayClaimReinsurance)
        Me.fraReinsurance.Controls.Add(Me.chkDisplayReinsuranceScreen)
        Me.fraReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReinsurance.Location = New System.Drawing.Point(6, 240)
        Me.fraReinsurance.Name = "fraReinsurance"
        Me.fraReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReinsurance.Size = New System.Drawing.Size(295, 80)
        Me.fraReinsurance.TabIndex = 239
        Me.fraReinsurance.TabStop = False
        Me.fraReinsurance.Text = "Reinsurance"
        '
        'chkDisplayClaimReinsurance
        '
        Me.chkDisplayClaimReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayClaimReinsurance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisplayClaimReinsurance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayClaimReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayClaimReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayClaimReinsurance.Location = New System.Drawing.Point(8, 40)
        Me.chkDisplayClaimReinsurance.Name = "chkDisplayClaimReinsurance"
        Me.chkDisplayClaimReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayClaimReinsurance.Size = New System.Drawing.Size(273, 25)
        Me.chkDisplayClaimReinsurance.TabIndex = 241
        Me.chkDisplayClaimReinsurance.Text = "Display Claim Reinsurance"
        Me.chkDisplayClaimReinsurance.UseVisualStyleBackColor = False
        '
        'chkDisplayReinsuranceScreen
        '
        Me.chkDisplayReinsuranceScreen.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayReinsuranceScreen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisplayReinsuranceScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayReinsuranceScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisplayReinsuranceScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayReinsuranceScreen.Location = New System.Drawing.Point(8, 16)
        Me.chkDisplayReinsuranceScreen.Name = "chkDisplayReinsuranceScreen"
        Me.chkDisplayReinsuranceScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayReinsuranceScreen.Size = New System.Drawing.Size(273, 25)
        Me.chkDisplayReinsuranceScreen.TabIndex = 240
        Me.chkDisplayReinsuranceScreen.Text = "Display Reinsurance Screen"
        Me.chkDisplayReinsuranceScreen.UseVisualStyleBackColor = False
        '
                ' fraVoidTransaction
        '
        Me.fraVoidTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.fraVoidTransaction.Controls.Add(Me.lblVoidPolicyVersion)
        Me.fraVoidTransaction.Controls.Add(Me.cboVoidTransaction)
        Me.fraVoidTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!,System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraVoidTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraVoidTransaction.Location = New System.Drawing.Point(6, 280) ' Placed right below Reinsurance frame
        Me.fraVoidTransaction.Name = "fraVoidTransaction"
        Me.fraVoidTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraVoidTransaction.Size = New System.Drawing.Size(295, 40) ' Smaller height, fits checkbox neatly
        Me.fraVoidTransaction.TabIndex = 173
        Me.fraVoidTransaction.TabStop = False
        Me.fraVoidTransaction.Text = ""
        '
        ' lblVoidPolicyVersion
        '
        Me.lblVoidPolicyVersion.BackColor = System.Drawing.SystemColors.Control
        Me.lblVoidPolicyVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!,System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVoidPolicyVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVoidPolicyVersion.Location = New System.Drawing.Point(10, 10) ' left side
        Me.lblVoidPolicyVersion.Name = "lblVoidPolicyVersion"
        Me.lblVoidPolicyVersion.Size = New System.Drawing.Size(120, 20) ' roughly half the frame width
        Me.lblVoidPolicyVersion.TabIndex = 221
        Me.lblVoidPolicyVersion.Text = "Void Policy Version"
        Me.lblVoidPolicyVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' cboVoidTransaction
        '
        Me.cboVoidTransaction.BackColor = System.Drawing.SystemColors.Window
        Me.cboVoidTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboVoidTransaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVoidTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!,System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboVoidTransaction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboVoidTransaction.Location = New System.Drawing.Point(140, 10) ' right side of label
        Me.cboVoidTransaction.Name = "cboVoidTransaction"
        Me.cboVoidTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboVoidTransaction.Size = New System.Drawing.Size(145, 21) ' remaining space in frame
        Me.cboVoidTransaction.TabIndex = 220
        Me.cboVoidTransaction.Items.AddRange(New Object() {"Not Allowed", "Current Period Only", "Current Period + 1", "Unrestricted"})
        '
        'fraRatingSections
        '
        Me.fraRatingSections.BackColor = System.Drawing.SystemColors.Control
        Me.fraRatingSections.Controls.Add(Me.chkEditRatingSections)
        Me.fraRatingSections.Controls.Add(Me.chkAddRemoveRatingSections)
        Me.fraRatingSections.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRatingSections.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRatingSections.Location = New System.Drawing.Point(6, 156)
        Me.fraRatingSections.Name = "fraRatingSections"
        Me.fraRatingSections.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRatingSections.Size = New System.Drawing.Size(295, 79)
        Me.fraRatingSections.TabIndex = 182
        Me.fraRatingSections.TabStop = False
        Me.fraRatingSections.Text = "Rating Sections"
        Me.fraRatingSections.Visible = False
        '
        'chkEditRatingSections
        '
        Me.chkEditRatingSections.BackColor = System.Drawing.SystemColors.Control
        Me.chkEditRatingSections.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditRatingSections.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEditRatingSections.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEditRatingSections.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEditRatingSections.Location = New System.Drawing.Point(8, 40)
        Me.chkEditRatingSections.Name = "chkEditRatingSections"
        Me.chkEditRatingSections.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEditRatingSections.Size = New System.Drawing.Size(273, 25)
        Me.chkEditRatingSections.TabIndex = 219
        Me.chkEditRatingSections.Text = "User can Edit existing  Rating Sections"
        Me.chkEditRatingSections.UseVisualStyleBackColor = False
        '
        'chkAddRemoveRatingSections
        '
        Me.chkAddRemoveRatingSections.BackColor = System.Drawing.SystemColors.Control
        Me.chkAddRemoveRatingSections.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAddRemoveRatingSections.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAddRemoveRatingSections.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAddRemoveRatingSections.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAddRemoveRatingSections.Location = New System.Drawing.Point(8, 16)
        Me.chkAddRemoveRatingSections.Name = "chkAddRemoveRatingSections"
        Me.chkAddRemoveRatingSections.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAddRemoveRatingSections.Size = New System.Drawing.Size(273, 25)
        Me.chkAddRemoveRatingSections.TabIndex = 218
        Me.chkAddRemoveRatingSections.Text = "User can Add/Remove Rating Sections"
        Me.chkAddRemoveRatingSections.UseVisualStyleBackColor = False
        '
        'fraMakeLive
        '
        Me.fraMakeLive.BackColor = System.Drawing.SystemColors.Control
        Me.fraMakeLive.Controls.Add(Me.fraCommSettings)
        Me.fraMakeLive.Controls.Add(Me.chkCanChangeInstalmentPlanDefaultCurrecny)
        Me.fraMakeLive.Controls.Add(Me.chkEditAgentDuringMTAMTC)
        Me.fraMakeLive.Controls.Add(Me.chkCashDeposit)
        Me.fraMakeLive.Controls.Add(Me.chkBankGuarantee)
        Me.fraMakeLive.Controls.Add(Me.cboMTAAuthority)
        Me.fraMakeLive.Controls.Add(Me.txtPaynowWriteOffAmount)
        Me.fraMakeLive.Controls.Add(Me.chkInvoice)
        Me.fraMakeLive.Controls.Add(Me.chkPayNow)
        Me.fraMakeLive.Controls.Add(Me.chkInstalments)
        Me.fraMakeLive.Controls.Add(Me.chkHasPaynowWriteOffAuthority)
        Me.fraMakeLive.Controls.Add(Me.cboMakeLiveCurrency)
        Me.fraMakeLive.Controls.Add(Me.Label16)
        Me.fraMakeLive.Controls.Add(Me.LblPaynowAmount)
        Me.fraMakeLive.Controls.Add(Me.lblPaynowCurrency)
        Me.fraMakeLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMakeLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMakeLive.Location = New System.Drawing.Point(306, 8)
        Me.fraMakeLive.Name = "fraMakeLive"
        Me.fraMakeLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMakeLive.Size = New System.Drawing.Size(473, 313)
        Me.fraMakeLive.TabIndex = 177
        Me.fraMakeLive.TabStop = False
        Me.fraMakeLive.Text = "Make Live"
        '
        'fraCommSettings
        '
        Me.fraCommSettings.Controls.Add(Me.chkEditDefaultCommissionMTR)
        Me.fraCommSettings.Controls.Add(Me.chkEditDefaultCommissionMTC)
        Me.fraCommSettings.Controls.Add(Me.chkEditDefaultCommissionMTA)
        Me.fraCommSettings.Controls.Add(Me.chkEditDefaultCommissionNBRN)
        Me.fraCommSettings.Controls.Add(Me.chkEditDefaultCommission)
        Me.fraCommSettings.Location = New System.Drawing.Point(11, 168)
        Me.fraCommSettings.Name = "fraCommSettings"
        Me.fraCommSettings.Size = New System.Drawing.Size(428, 91)
        Me.fraCommSettings.TabIndex = 240
        Me.fraCommSettings.TabStop = False
        Me.fraCommSettings.Text = "Default Commission Settings"
        '
        'chkEditDefaultCommissionMTR
        '
        Me.chkEditDefaultCommissionMTR.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditDefaultCommissionMTR.Location = New System.Drawing.Point(224, 61)
        Me.chkEditDefaultCommissionMTR.Name = "chkEditDefaultCommissionMTR"
        Me.chkEditDefaultCommissionMTR.Size = New System.Drawing.Size(183, 24)
        Me.chkEditDefaultCommissionMTR.TabIndex = 249
        Me.chkEditDefaultCommissionMTR.Text = "Transaction Type MTR"
        Me.chkEditDefaultCommissionMTR.UseVisualStyleBackColor = True
        '
        'chkEditDefaultCommissionMTC
        '
        Me.chkEditDefaultCommissionMTC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditDefaultCommissionMTC.Location = New System.Drawing.Point(224, 42)
        Me.chkEditDefaultCommissionMTC.Name = "chkEditDefaultCommissionMTC"
        Me.chkEditDefaultCommissionMTC.Size = New System.Drawing.Size(183, 24)
        Me.chkEditDefaultCommissionMTC.TabIndex = 248
        Me.chkEditDefaultCommissionMTC.Text = "Transaction Type MTC"
        Me.chkEditDefaultCommissionMTC.UseVisualStyleBackColor = True
        '
        'chkEditDefaultCommissionMTA
        '
        Me.chkEditDefaultCommissionMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditDefaultCommissionMTA.Location = New System.Drawing.Point(7, 61)
        Me.chkEditDefaultCommissionMTA.Name = "chkEditDefaultCommissionMTA"
        Me.chkEditDefaultCommissionMTA.Size = New System.Drawing.Size(195, 24)
        Me.chkEditDefaultCommissionMTA.TabIndex = 247
        Me.chkEditDefaultCommissionMTA.Text = "Transaction Type MTA"
        Me.chkEditDefaultCommissionMTA.UseVisualStyleBackColor = True
        '
        'chkEditDefaultCommissionNBRN
        '
        Me.chkEditDefaultCommissionNBRN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditDefaultCommissionNBRN.Location = New System.Drawing.Point(6, 42)
        Me.chkEditDefaultCommissionNBRN.Name = "chkEditDefaultCommissionNBRN"
        Me.chkEditDefaultCommissionNBRN.Size = New System.Drawing.Size(196, 24)
        Me.chkEditDefaultCommissionNBRN.TabIndex = 246
        Me.chkEditDefaultCommissionNBRN.Text = "Transaction Type NB/RN"
        Me.chkEditDefaultCommissionNBRN.UseVisualStyleBackColor = True
        '
        'chkEditDefaultCommission
        '
        Me.chkEditDefaultCommission.BackColor = System.Drawing.SystemColors.Control
        Me.chkEditDefaultCommission.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditDefaultCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEditDefaultCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEditDefaultCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEditDefaultCommission.Location = New System.Drawing.Point(7, 24)
        Me.chkEditDefaultCommission.Name = "chkEditDefaultCommission"
        Me.chkEditDefaultCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEditDefaultCommission.Size = New System.Drawing.Size(195, 17)
        Me.chkEditDefaultCommission.TabIndex = 245
        Me.chkEditDefaultCommission.Text = "Edit Default Commission"
        Me.chkEditDefaultCommission.UseVisualStyleBackColor = False
        '
        'chkCanChangeInstalmentPlanDefaultCurrecny
        '
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.BackColor = System.Drawing.SystemColors.Control
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Location = New System.Drawing.Point(9, 287)
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Name = "chkCanChangeInstalmentPlanDefaultCurrecny"
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Size = New System.Drawing.Size(297, 17)
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.TabIndex = 246
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.Text = "Can  Change Instalment Plan Default Currency:"
        Me.chkCanChangeInstalmentPlanDefaultCurrecny.UseVisualStyleBackColor = False
        '
        'chkEditAgentDuringMTAMTC
        '
        Me.chkEditAgentDuringMTAMTC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEditAgentDuringMTAMTC.Location = New System.Drawing.Point(8, 265)
        Me.chkEditAgentDuringMTAMTC.Name = "chkEditAgentDuringMTAMTC"
        Me.chkEditAgentDuringMTAMTC.Size = New System.Drawing.Size(195, 17)
        Me.chkEditAgentDuringMTAMTC.TabIndex = 246
        Me.chkEditAgentDuringMTAMTC.Text = "Agent Editable During MTA/MTC"
        Me.chkEditAgentDuringMTAMTC.UseVisualStyleBackColor = True
        '
        'chkCashDeposit
        '
        Me.chkCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.chkCashDeposit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCashDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCashDeposit.Location = New System.Drawing.Point(209, 46)
        Me.chkCashDeposit.Name = "chkCashDeposit"
        Me.chkCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCashDeposit.Size = New System.Drawing.Size(199, 17)
        Me.chkCashDeposit.TabIndex = 244
        Me.chkCashDeposit.Text = "Cash Deposit"
        Me.chkCashDeposit.UseVisualStyleBackColor = False
        '
        'chkBankGuarantee
        '
        Me.chkBankGuarantee.BackColor = System.Drawing.SystemColors.Control
        Me.chkBankGuarantee.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkBankGuarantee.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkBankGuarantee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBankGuarantee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBankGuarantee.Location = New System.Drawing.Point(209, 22)
        Me.chkBankGuarantee.Name = "chkBankGuarantee"
        Me.chkBankGuarantee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkBankGuarantee.Size = New System.Drawing.Size(199, 17)
        Me.chkBankGuarantee.TabIndex = 242
        Me.chkBankGuarantee.Text = "Bank Guarantee"
        Me.chkBankGuarantee.UseVisualStyleBackColor = False
        '
        'cboMTAAuthority
        '
        Me.cboMTAAuthority.BackColor = System.Drawing.SystemColors.Window
        Me.cboMTAAuthority.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMTAAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMTAAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMTAAuthority.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMTAAuthority.Location = New System.Drawing.Point(194, 145)
        Me.cboMTAAuthority.Name = "cboMTAAuthority"
        Me.cboMTAAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMTAAuthority.Size = New System.Drawing.Size(227, 21)
        Me.cboMTAAuthority.TabIndex = 226
        '
        'txtPaynowWriteOffAmount
        '
        Me.txtPaynowWriteOffAmount.AcceptsReturn = True
        Me.txtPaynowWriteOffAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtPaynowWriteOffAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPaynowWriteOffAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaynowWriteOffAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPaynowWriteOffAmount.Location = New System.Drawing.Point(96, 119)
        Me.txtPaynowWriteOffAmount.MaxLength = 0
        Me.txtPaynowWriteOffAmount.Name = "txtPaynowWriteOffAmount"
        Me.txtPaynowWriteOffAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPaynowWriteOffAmount.Size = New System.Drawing.Size(209, 20)
        Me.txtPaynowWriteOffAmount.TabIndex = 225
        '
        'chkInvoice
        '
        Me.chkInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.chkInvoice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInvoice.Location = New System.Drawing.Point(8, 21)
        Me.chkInvoice.Name = "chkInvoice"
        Me.chkInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInvoice.Size = New System.Drawing.Size(195, 17)
        Me.chkInvoice.TabIndex = 179
        Me.chkInvoice.Text = "Invoice"
        Me.chkInvoice.UseVisualStyleBackColor = False
        '
        'chkPayNow
        '
        Me.chkPayNow.BackColor = System.Drawing.SystemColors.Control
        Me.chkPayNow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPayNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPayNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPayNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPayNow.Location = New System.Drawing.Point(8, 44)
        Me.chkPayNow.Name = "chkPayNow"
        Me.chkPayNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPayNow.Size = New System.Drawing.Size(195, 17)
        Me.chkPayNow.TabIndex = 221
        Me.chkPayNow.Text = "Pay Now"
        Me.chkPayNow.UseVisualStyleBackColor = False
        '
        'chkInstalments
        '
        Me.chkInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.chkInstalments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInstalments.Location = New System.Drawing.Point(8, 66)
        Me.chkInstalments.Name = "chkInstalments"
        Me.chkInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInstalments.Size = New System.Drawing.Size(195, 17)
        Me.chkInstalments.TabIndex = 222
        Me.chkInstalments.Text = "Instalments"
        Me.chkInstalments.UseVisualStyleBackColor = False
        '
        'chkHasPaynowWriteOffAuthority
        '
        Me.chkHasPaynowWriteOffAuthority.BackColor = System.Drawing.SystemColors.Control
        Me.chkHasPaynowWriteOffAuthority.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkHasPaynowWriteOffAuthority.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHasPaynowWriteOffAuthority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHasPaynowWriteOffAuthority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHasPaynowWriteOffAuthority.Location = New System.Drawing.Point(209, 66)
        Me.chkHasPaynowWriteOffAuthority.Name = "chkHasPaynowWriteOffAuthority"
        Me.chkHasPaynowWriteOffAuthority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHasPaynowWriteOffAuthority.Size = New System.Drawing.Size(199, 17)
        Me.chkHasPaynowWriteOffAuthority.TabIndex = 178
        Me.chkHasPaynowWriteOffAuthority.Text = "User can perform pay now write-offs"
        Me.chkHasPaynowWriteOffAuthority.UseVisualStyleBackColor = False
        '
        'cboMakeLiveCurrency
        '
        Me.cboMakeLiveCurrency.CompanyId = 0
        Me.cboMakeLiveCurrency.CurrencyId = 0
        Me.cboMakeLiveCurrency.DefaultCurrencyId = 0
        Me.cboMakeLiveCurrency.FirstItem = ""
        Me.cboMakeLiveCurrency.ListIndex = -1
        Me.cboMakeLiveCurrency.Location = New System.Drawing.Point(96, 90)
        Me.cboMakeLiveCurrency.Name = "cboMakeLiveCurrency"
        Me.cboMakeLiveCurrency.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.cboMakeLiveCurrency.Size = New System.Drawing.Size(209, 21)
        Me.cboMakeLiveCurrency.TabIndex = 223
        Me.cboMakeLiveCurrency.ToolTipText = ""
        Me.cboMakeLiveCurrency.WhatsThisHelpID = 0
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.SystemColors.Control
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label16.Location = New System.Drawing.Point(8, 145)
        Me.Label16.Name = "Label16"
        Me.Label16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label16.Size = New System.Drawing.Size(185, 13)
        Me.Label16.TabIndex = 238
        Me.Label16.Text = "BackDate MTA/MTC/MTR Authority:"
        '
        'LblPaynowAmount
        '
        Me.LblPaynowAmount.AutoSize = True
        Me.LblPaynowAmount.BackColor = System.Drawing.SystemColors.Control
        Me.LblPaynowAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblPaynowAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPaynowAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblPaynowAmount.Location = New System.Drawing.Point(8, 119)
        Me.LblPaynowAmount.Name = "LblPaynowAmount"
        Me.LblPaynowAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LblPaynowAmount.Size = New System.Drawing.Size(46, 13)
        Me.LblPaynowAmount.TabIndex = 181
        Me.LblPaynowAmount.Text = "Amount:"
        '
        'lblPaynowCurrency
        '
        Me.lblPaynowCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaynowCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaynowCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaynowCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaynowCurrency.Location = New System.Drawing.Point(8, 90)
        Me.lblPaynowCurrency.Name = "lblPaynowCurrency"
        Me.lblPaynowCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaynowCurrency.Size = New System.Drawing.Size(73, 17)
        Me.lblPaynowCurrency.TabIndex = 180
        Me.lblPaynowCurrency.Text = "Currency:"
        '
        'fmeAccess
        '
        Me.fmeAccess.BackColor = System.Drawing.SystemColors.Control
        Me.fmeAccess.Controls.Add(Me.chkCanExtractClientData)
        Me.fmeAccess.Controls.Add(Me.lblCanExtractClientData)
        Me.fmeAccess.Controls.Add(Me.chkViewBatchProcessStatus)
        Me.fmeAccess.Controls.Add(Me.lblViewBatchProcessStatus)
        Me.fmeAccess.Controls.Add(Me.chkUnrestrictedUpdate)
        Me.fmeAccess.Controls.Add(Me.chkUnrestrictedEnquiry)
        Me.fmeAccess.Controls.Add(Me.lblUnrestrictedUpdate)
        Me.fmeAccess.Controls.Add(Me.lblUnrestrictedEnquiry)
        Me.fmeAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmeAccess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeAccess.Location = New System.Drawing.Point(6, 8)
        Me.fmeAccess.Name = "fmeAccess"
        Me.fmeAccess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeAccess.Size = New System.Drawing.Size(295, 101)
        Me.fmeAccess.TabIndex = 174
        Me.fmeAccess.TabStop = False
        Me.fmeAccess.Text = "Access"
        '
        'chkViewBatchProcessStatus
        '
        Me.chkViewBatchProcessStatus.BackColor = System.Drawing.SystemColors.Control
        Me.chkViewBatchProcessStatus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkViewBatchProcessStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkViewBatchProcessStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkViewBatchProcessStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkViewBatchProcessStatus.Location = New System.Drawing.Point(8, 58)
        Me.chkViewBatchProcessStatus.Name = "chkViewBatchProcessStatus"
        Me.chkViewBatchProcessStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkViewBatchProcessStatus.Size = New System.Drawing.Size(243, 17)
        Me.chkViewBatchProcessStatus.TabIndex = 220
        Me.chkViewBatchProcessStatus.Text = "User can view batch process status"
        Me.chkViewBatchProcessStatus.UseVisualStyleBackColor = False
        '
        'chkCanExtractClientData
        '
        Me.chkCanExtractClientData.BackColor = System.Drawing.SystemColors.Control
        Me.chkCanExtractClientData.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCanExtractClientData.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCanExtractClientData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCanExtractClientData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCanExtractClientData.Location = New System.Drawing.Point(8, 78)
        Me.chkCanExtractClientData.Name = "chkCanExtractClientData"
        Me.chkCanExtractClientData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCanExtractClientData.Size = New System.Drawing.Size(243, 17)
        Me.chkCanExtractClientData.TabIndex = 221
        Me.chkCanExtractClientData.Text = "Can extract client data"
        Me.chkCanExtractClientData.UseVisualStyleBackColor = False
        '
        'lblCanExtractClientData
        '
        Me.lblCanExtractClientData.BackColor = System.Drawing.SystemColors.Control
        Me.lblCanExtractClientData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCanExtractClientData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCanExtractClientData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCanExtractClientData.Location = New System.Drawing.Point(259, 78)
        Me.lblCanExtractClientData.Name = "lblCanExtractClientData"
        Me.lblCanExtractClientData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCanExtractClientData.Size = New System.Drawing.Size(26, 13)
        Me.lblCanExtractClientData.TabIndex = 222
        Me.lblCanExtractClientData.Text = "No"
        '
        'lblViewBatchProcessStatus
        '
        Me.lblViewBatchProcessStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblViewBatchProcessStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblViewBatchProcessStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblViewBatchProcessStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblViewBatchProcessStatus.Location = New System.Drawing.Point(260, 60)
        Me.lblViewBatchProcessStatus.Name = "lblViewBatchProcessStatus"
        Me.lblViewBatchProcessStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblViewBatchProcessStatus.Size = New System.Drawing.Size(26, 13)
        Me.lblViewBatchProcessStatus.TabIndex = 219
        Me.lblViewBatchProcessStatus.Text = "No"
        '
        'chkUnrestrictedUpdate
        '
        Me.chkUnrestrictedUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnrestrictedUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUnrestrictedUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnrestrictedUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnrestrictedUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnrestrictedUpdate.Location = New System.Drawing.Point(8, 38)
        Me.chkUnrestrictedUpdate.Name = "chkUnrestrictedUpdate"
        Me.chkUnrestrictedUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnrestrictedUpdate.Size = New System.Drawing.Size(243, 17)
        Me.chkUnrestrictedUpdate.TabIndex = 216
        Me.chkUnrestrictedUpdate.Text = "User has unrestricted update access"
        Me.chkUnrestrictedUpdate.UseVisualStyleBackColor = False
        '
        'chkUnrestrictedEnquiry
        '
        Me.chkUnrestrictedEnquiry.BackColor = System.Drawing.SystemColors.Control
        Me.chkUnrestrictedEnquiry.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUnrestrictedEnquiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUnrestrictedEnquiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUnrestrictedEnquiry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUnrestrictedEnquiry.Location = New System.Drawing.Point(8, 19)
        Me.chkUnrestrictedEnquiry.Name = "chkUnrestrictedEnquiry"
        Me.chkUnrestrictedEnquiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUnrestrictedEnquiry.Size = New System.Drawing.Size(243, 17)
        Me.chkUnrestrictedEnquiry.TabIndex = 215
        Me.chkUnrestrictedEnquiry.Text = "User has unrestricted enquiry access"
        Me.chkUnrestrictedEnquiry.UseVisualStyleBackColor = False
        '
        'lblUnrestrictedUpdate
        '
        Me.lblUnrestrictedUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnrestrictedUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnrestrictedUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnrestrictedUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnrestrictedUpdate.Location = New System.Drawing.Point(260, 40)
        Me.lblUnrestrictedUpdate.Name = "lblUnrestrictedUpdate"
        Me.lblUnrestrictedUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnrestrictedUpdate.Size = New System.Drawing.Size(26, 13)
        Me.lblUnrestrictedUpdate.TabIndex = 176
        Me.lblUnrestrictedUpdate.Text = "No"
        '
        'lblUnrestrictedEnquiry
        '
        Me.lblUnrestrictedEnquiry.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnrestrictedEnquiry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnrestrictedEnquiry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnrestrictedEnquiry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnrestrictedEnquiry.Location = New System.Drawing.Point(260, 22)
        Me.lblUnrestrictedEnquiry.Name = "lblUnrestrictedEnquiry"
        Me.lblUnrestrictedEnquiry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnrestrictedEnquiry.Size = New System.Drawing.Size(26, 13)
        Me.lblUnrestrictedEnquiry.TabIndex = 175
        Me.lblUnrestrictedEnquiry.Text = "No"
        '
        'fraBrokerAgentPortfolioTransfer
        '
        Me.fraBrokerAgentPortfolioTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.fraBrokerAgentPortfolioTransfer.Controls.Add(Me.chkCanPerformBrokerTransfer)
        Me.fraBrokerAgentPortfolioTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBrokerAgentPortfolioTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBrokerAgentPortfolioTransfer.Location = New System.Drawing.Point(6, 114)
        Me.fraBrokerAgentPortfolioTransfer.Name = "fraBrokerAgentPortfolioTransfer"
        Me.fraBrokerAgentPortfolioTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBrokerAgentPortfolioTransfer.Size = New System.Drawing.Size(295, 41)
        Me.fraBrokerAgentPortfolioTransfer.TabIndex = 173
        Me.fraBrokerAgentPortfolioTransfer.TabStop = False
        Me.fraBrokerAgentPortfolioTransfer.Text = "Broker/Agent Portfolio Transfer"
        '
        'chkCanPerformBrokerTransfer
        '
        Me.chkCanPerformBrokerTransfer.BackColor = System.Drawing.SystemColors.Control
        Me.chkCanPerformBrokerTransfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCanPerformBrokerTransfer.Enabled = False
        Me.chkCanPerformBrokerTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCanPerformBrokerTransfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCanPerformBrokerTransfer.Location = New System.Drawing.Point(15, 21)
        Me.chkCanPerformBrokerTransfer.Name = "chkCanPerformBrokerTransfer"
        Me.chkCanPerformBrokerTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCanPerformBrokerTransfer.Size = New System.Drawing.Size(217, 18)
        Me.chkCanPerformBrokerTransfer.TabIndex = 217
        Me.chkCanPerformBrokerTransfer.Text = "User can perform broker transfer"
        Me.chkCanPerformBrokerTransfer.UseVisualStyleBackColor = False
        '
        '_SSTab2_TabPage3
        '
        Me._SSTab2_TabPage3.Controls.Add(Me.Frame11)
        Me._SSTab2_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab2_TabPage3.Name = "_SSTab2_TabPage3"
        Me._SSTab2_TabPage3.Size = New System.Drawing.Size(793, 334)
        Me._SSTab2_TabPage3.TabIndex = 3
        Me._SSTab2_TabPage3.Text = "Debugging"
        '
        'Frame11
        '
        Me.Frame11.BackColor = System.Drawing.SystemColors.Control
        Me.Frame11.Controls.Add(Me.chkUserServerScriptsRunInDebug)
        Me.Frame11.Controls.Add(Me.chkUserCanDebugDynamicLogicScripts)
        Me.Frame11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame11.Location = New System.Drawing.Point(14, 16)
        Me.Frame11.Name = "Frame11"
        Me.Frame11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame11.Size = New System.Drawing.Size(354, 84)
        Me.Frame11.TabIndex = 246
        Me.Frame11.TabStop = False
        Me.Frame11.Text = "Access"
        '
        'chkUserServerScriptsRunInDebug
        '
        Me.chkUserServerScriptsRunInDebug.BackColor = System.Drawing.SystemColors.Control
        Me.chkUserServerScriptsRunInDebug.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUserServerScriptsRunInDebug.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUserServerScriptsRunInDebug.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUserServerScriptsRunInDebug.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUserServerScriptsRunInDebug.Location = New System.Drawing.Point(9, 48)
        Me.chkUserServerScriptsRunInDebug.Name = "chkUserServerScriptsRunInDebug"
        Me.chkUserServerScriptsRunInDebug.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUserServerScriptsRunInDebug.Size = New System.Drawing.Size(317, 17)
        Me.chkUserServerScriptsRunInDebug.TabIndex = 248
        Me.chkUserServerScriptsRunInDebug.Text = "Server Rules run in debug mode for this User"
        Me.chkUserServerScriptsRunInDebug.UseVisualStyleBackColor = False
        '
        'chkUserCanDebugDynamicLogicScripts
        '
        Me.chkUserCanDebugDynamicLogicScripts.BackColor = System.Drawing.SystemColors.Control
        Me.chkUserCanDebugDynamicLogicScripts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUserCanDebugDynamicLogicScripts.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUserCanDebugDynamicLogicScripts.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUserCanDebugDynamicLogicScripts.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUserCanDebugDynamicLogicScripts.Location = New System.Drawing.Point(9, 21)
        Me.chkUserCanDebugDynamicLogicScripts.Name = "chkUserCanDebugDynamicLogicScripts"
        Me.chkUserCanDebugDynamicLogicScripts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUserCanDebugDynamicLogicScripts.Size = New System.Drawing.Size(316, 17)
        Me.chkUserCanDebugDynamicLogicScripts.TabIndex = 247
        Me.chkUserCanDebugDynamicLogicScripts.Text = "User can debug Dynamic Logic with CTRL-ALT-F12"
        Me.chkUserCanDebugDynamicLogicScripts.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_3
        '
        Me._cmdPrevious_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_3.Location = New System.Drawing.Point(5, 376)
        Me._cmdPrevious_3.Name = "_cmdPrevious_3"
        Me._cmdPrevious_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_3.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_3.TabIndex = 229
        Me._cmdPrevious_3.Text = "<<"
        Me._cmdPrevious_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_3.UseVisualStyleBackColor = False
        '
        '_cmdNext_4
        '
        Me._cmdNext_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_4.Location = New System.Drawing.Point(761, 376)
        Me._cmdNext_4.Name = "_cmdNext_4"
        Me._cmdNext_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_4.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_4.TabIndex = 170
        Me._cmdNext_4.Text = ">>"
        Me._cmdNext_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_4.UseVisualStyleBackColor = False
        '
        '_SSTab1_TabPage5
        '
        Me._SSTab1_TabPage5.Controls.Add(Me.Frame6)
        Me._SSTab1_TabPage5.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage5.Name = "_SSTab1_TabPage5"
        Me._SSTab1_TabPage5.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage5.TabIndex = 5
        Me._SSTab1_TabPage5.Text = "6 - FSA"
        Me._SSTab1_TabPage5.UseVisualStyleBackColor = True
        '
        'Frame6
        '
        Me.Frame6.BackColor = System.Drawing.SystemColors.Control
        Me.Frame6.Controls.Add(Me._cmdNext_5)
        Me.Frame6.Controls.Add(Me._cmdPrevious_4)
        Me.Frame6.Controls.Add(Me.cmdEditRiskDetails)
        Me.Frame6.Controls.Add(Me.lvwRiskGroups)
        Me.Frame6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame6.Location = New System.Drawing.Point(12, 10)
        Me.Frame6.Name = "Frame6"
        Me.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame6.Size = New System.Drawing.Size(650, 374)
        Me.Frame6.TabIndex = 100
        Me.Frame6.TabStop = False
        '
        '_cmdNext_5
        '
        Me._cmdNext_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_5.Location = New System.Drawing.Point(604, 350)
        Me._cmdNext_5.Name = "_cmdNext_5"
        Me._cmdNext_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_5.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_5.TabIndex = 104
        Me._cmdNext_5.Text = ">>"
        Me._cmdNext_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_5.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_4
        '
        Me._cmdPrevious_4.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_4.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_4.Name = "_cmdPrevious_4"
        Me._cmdPrevious_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_4.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_4.TabIndex = 103
        Me._cmdPrevious_4.Text = "<<"
        Me._cmdPrevious_4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_4.UseVisualStyleBackColor = False
        '
        'lvwRiskGroups
        '
        Me.lvwRiskGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRiskGroups_ColumnHeader_1, Me._lvwRiskGroups_ColumnHeader_2, Me._lvwRiskGroups_ColumnHeader_3, Me._lvwRiskGroups_ColumnHeader_4})
        Me.lvwRiskGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRiskGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskGroups.FullRowSelect = True
        Me.lvwRiskGroups.HideSelection = False
        Me.lvwRiskGroups.LabelEdit = True
        Me.lvwRiskGroups.Location = New System.Drawing.Point(19, 24)
        Me.lvwRiskGroups.Name = "lvwRiskGroups"
        Me.lvwRiskGroups.Size = New System.Drawing.Size(537, 305)
        Me.lvwRiskGroups.TabIndex = 101
        Me.lvwRiskGroups.UseCompatibleStateImageBehavior = False
        Me.lvwRiskGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwRiskGroups_ColumnHeader_1
        '
        Me._lvwRiskGroups_ColumnHeader_1.Text = "Risk Group"
        Me._lvwRiskGroups_ColumnHeader_1.Width = 167
        '
        '_lvwRiskGroups_ColumnHeader_2
        '
        Me._lvwRiskGroups_ColumnHeader_2.Text = "User Status"
        Me._lvwRiskGroups_ColumnHeader_2.Width = 97
        '
        '_lvwRiskGroups_ColumnHeader_3
        '
        Me._lvwRiskGroups_ColumnHeader_3.Text = "Passed Exam"
        Me._lvwRiskGroups_ColumnHeader_3.Width = 97
        '
        '_lvwRiskGroups_ColumnHeader_4
        '
        Me._lvwRiskGroups_ColumnHeader_4.Text = "Date Passed Exam"
        Me._lvwRiskGroups_ColumnHeader_4.Width = 137
        '
        '_SSTab1_TabPage6
        '
        Me._SSTab1_TabPage6.Controls.Add(Me.frmClientManagerSecurity)
        Me._SSTab1_TabPage6.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage6.Name = "_SSTab1_TabPage6"
        Me._SSTab1_TabPage6.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage6.TabIndex = 6
        Me._SSTab1_TabPage6.Text = "7 - Client Manager Security"
        Me._SSTab1_TabPage6.UseVisualStyleBackColor = True
        '
        'frmClientManagerSecurity
        '
        Me.frmClientManagerSecurity.BackColor = System.Drawing.SystemColors.Control
        Me.frmClientManagerSecurity.Controls.Add(Me.chkCanReverseReplaceTransactions)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsReverseTransactions)
        Me.frmClientManagerSecurity.Controls.Add(Me._cmdNext_6)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsEditSchemePolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsDeletePolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsViewClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsEditClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsEditPolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsViewClaim)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsEditFinancePlan)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsRaiseDebit)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsRaiseCredit)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsRaiseCash)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsReverseAllocations)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsRaiseFee)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsRaiseManualDID)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsDeleteClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.chkIsPerformAllocations)
        Me.frmClientManagerSecurity.Controls.Add(Me._cmdPrevious_5)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblReverseReplaceTransactions)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblEditSchemePolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblDeletePolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblViewClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblEditClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblEditPolicy)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblViewClaim)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblEditFinancePlan)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblRaiseDebit)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblRaiseCredit)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblRaiseFee)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblRaiseCash)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblReverseTransactions)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblReverseAllocations)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblRaiseManualDID)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblDeleteClient)
        Me.frmClientManagerSecurity.Controls.Add(Me.lblPerformAllocations)
        Me.frmClientManagerSecurity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmClientManagerSecurity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmClientManagerSecurity.Location = New System.Drawing.Point(12, 10)
        Me.frmClientManagerSecurity.Name = "frmClientManagerSecurity"
        Me.frmClientManagerSecurity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmClientManagerSecurity.Size = New System.Drawing.Size(788, 395)
        Me.frmClientManagerSecurity.TabIndex = 149
        Me.frmClientManagerSecurity.TabStop = False
        '
        'chkCanReverseReplaceTransactions
        '
        Me.chkCanReverseReplaceTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.chkCanReverseReplaceTransactions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCanReverseReplaceTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCanReverseReplaceTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCanReverseReplaceTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCanReverseReplaceTransactions.Location = New System.Drawing.Point(67, 328)
        Me.chkCanReverseReplaceTransactions.Name = "chkCanReverseReplaceTransactions"
        Me.chkCanReverseReplaceTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCanReverseReplaceTransactions.Size = New System.Drawing.Size(233, 17)
        Me.chkCanReverseReplaceTransactions.TabIndex = 237
        Me.chkCanReverseReplaceTransactions.Text = "Can Reverse/Replace Transactions:"
        Me.chkCanReverseReplaceTransactions.UseVisualStyleBackColor = False
        '
        'chkIsReverseTransactions
        '
        Me.chkIsReverseTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReverseTransactions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsReverseTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReverseTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReverseTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReverseTransactions.Location = New System.Drawing.Point(67, 303)
        Me.chkIsReverseTransactions.Name = "chkIsReverseTransactions"
        Me.chkIsReverseTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReverseTransactions.Size = New System.Drawing.Size(233, 17)
        Me.chkIsReverseTransactions.TabIndex = 236
        Me.chkIsReverseTransactions.Text = "Can Reverse Transactions:"
        Me.chkIsReverseTransactions.UseVisualStyleBackColor = False
        '
        '_cmdNext_6
        '
        Me._cmdNext_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_6.Location = New System.Drawing.Point(676, 350)
        Me._cmdNext_6.Name = "_cmdNext_6"
        Me._cmdNext_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_6.Size = New System.Drawing.Size(40, 19)
        Me._cmdNext_6.TabIndex = 167
        Me._cmdNext_6.Text = ">>"
        Me._cmdNext_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_6.UseVisualStyleBackColor = False
        '
        'chkIsEditSchemePolicy
        '
        Me.chkIsEditSchemePolicy.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsEditSchemePolicy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsEditSchemePolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsEditSchemePolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsEditSchemePolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsEditSchemePolicy.Location = New System.Drawing.Point(339, 63)
        Me.chkIsEditSchemePolicy.Name = "chkIsEditSchemePolicy"
        Me.chkIsEditSchemePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsEditSchemePolicy.Size = New System.Drawing.Size(233, 17)
        Me.chkIsEditSchemePolicy.TabIndex = 113
        Me.chkIsEditSchemePolicy.Text = "Can Edit Scheme Policies:"
        Me.chkIsEditSchemePolicy.UseVisualStyleBackColor = False
        '
        'chkIsDeletePolicy
        '
        Me.chkIsDeletePolicy.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDeletePolicy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDeletePolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDeletePolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDeletePolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDeletePolicy.Location = New System.Drawing.Point(67, 112)
        Me.chkIsDeletePolicy.Name = "chkIsDeletePolicy"
        Me.chkIsDeletePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDeletePolicy.Size = New System.Drawing.Size(233, 17)
        Me.chkIsDeletePolicy.TabIndex = 115
        Me.chkIsDeletePolicy.Text = "Can Delete Policies:"
        Me.chkIsDeletePolicy.UseVisualStyleBackColor = False
        '
        'chkIsViewClient
        '
        Me.chkIsViewClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsViewClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsViewClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsViewClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsViewClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsViewClient.Location = New System.Drawing.Point(68, 18)
        Me.chkIsViewClient.Name = "chkIsViewClient"
        Me.chkIsViewClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsViewClient.Size = New System.Drawing.Size(232, 17)
        Me.chkIsViewClient.TabIndex = 105
        Me.chkIsViewClient.Text = "Can View Clients:"
        Me.chkIsViewClient.UseVisualStyleBackColor = False
        '
        'chkIsEditClient
        '
        Me.chkIsEditClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsEditClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsEditClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsEditClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsEditClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsEditClient.Location = New System.Drawing.Point(67, 39)
        Me.chkIsEditClient.Name = "chkIsEditClient"
        Me.chkIsEditClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsEditClient.Size = New System.Drawing.Size(233, 17)
        Me.chkIsEditClient.TabIndex = 107
        Me.chkIsEditClient.Text = "Can Edit Clients:"
        Me.chkIsEditClient.UseVisualStyleBackColor = False
        '
        'chkIsEditPolicy
        '
        Me.chkIsEditPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsEditPolicy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsEditPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsEditPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsEditPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsEditPolicy.Location = New System.Drawing.Point(67, 87)
        Me.chkIsEditPolicy.Name = "chkIsEditPolicy"
        Me.chkIsEditPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsEditPolicy.Size = New System.Drawing.Size(233, 17)
        Me.chkIsEditPolicy.TabIndex = 111
        Me.chkIsEditPolicy.Text = "Can Edit Policies:"
        Me.chkIsEditPolicy.UseVisualStyleBackColor = False
        '
        'chkIsViewClaim
        '
        Me.chkIsViewClaim.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsViewClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsViewClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsViewClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsViewClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsViewClaim.Location = New System.Drawing.Point(67, 135)
        Me.chkIsViewClaim.Name = "chkIsViewClaim"
        Me.chkIsViewClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsViewClaim.Size = New System.Drawing.Size(233, 17)
        Me.chkIsViewClaim.TabIndex = 117
        Me.chkIsViewClaim.Text = "Can View Claims:"
        Me.chkIsViewClaim.UseVisualStyleBackColor = False
        '
        'chkIsEditFinancePlan
        '
        Me.chkIsEditFinancePlan.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsEditFinancePlan.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsEditFinancePlan.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsEditFinancePlan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsEditFinancePlan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsEditFinancePlan.Location = New System.Drawing.Point(67, 159)
        Me.chkIsEditFinancePlan.Name = "chkIsEditFinancePlan"
        Me.chkIsEditFinancePlan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsEditFinancePlan.Size = New System.Drawing.Size(233, 17)
        Me.chkIsEditFinancePlan.TabIndex = 119
        Me.chkIsEditFinancePlan.Text = "Can Add\Edit Finance Plans:"
        Me.chkIsEditFinancePlan.UseVisualStyleBackColor = False
        '
        'chkIsRaiseDebit
        '
        Me.chkIsRaiseDebit.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRaiseDebit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRaiseDebit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRaiseDebit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRaiseDebit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRaiseDebit.Location = New System.Drawing.Point(67, 183)
        Me.chkIsRaiseDebit.Name = "chkIsRaiseDebit"
        Me.chkIsRaiseDebit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRaiseDebit.Size = New System.Drawing.Size(233, 17)
        Me.chkIsRaiseDebit.TabIndex = 121
        Me.chkIsRaiseDebit.Text = "Can Raise Debit:"
        Me.chkIsRaiseDebit.UseVisualStyleBackColor = False
        '
        'chkIsRaiseCredit
        '
        Me.chkIsRaiseCredit.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRaiseCredit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRaiseCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRaiseCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRaiseCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRaiseCredit.Location = New System.Drawing.Point(67, 207)
        Me.chkIsRaiseCredit.Name = "chkIsRaiseCredit"
        Me.chkIsRaiseCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRaiseCredit.Size = New System.Drawing.Size(233, 17)
        Me.chkIsRaiseCredit.TabIndex = 123
        Me.chkIsRaiseCredit.Text = "Can Raise Credit:"
        Me.chkIsRaiseCredit.UseVisualStyleBackColor = False
        '
        'chkIsRaiseCash
        '
        Me.chkIsRaiseCash.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRaiseCash.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRaiseCash.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRaiseCash.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRaiseCash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRaiseCash.Location = New System.Drawing.Point(67, 255)
        Me.chkIsRaiseCash.Name = "chkIsRaiseCash"
        Me.chkIsRaiseCash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRaiseCash.Size = New System.Drawing.Size(233, 17)
        Me.chkIsRaiseCash.TabIndex = 127
        Me.chkIsRaiseCash.Text = "Can Raise Cash:"
        Me.chkIsRaiseCash.UseVisualStyleBackColor = False
        '
        'chkIsReverseAllocations
        '
        Me.chkIsReverseAllocations.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsReverseAllocations.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsReverseAllocations.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsReverseAllocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsReverseAllocations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsReverseAllocations.Location = New System.Drawing.Point(339, 39)
        Me.chkIsReverseAllocations.Name = "chkIsReverseAllocations"
        Me.chkIsReverseAllocations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsReverseAllocations.Size = New System.Drawing.Size(233, 17)
        Me.chkIsReverseAllocations.TabIndex = 134
        Me.chkIsReverseAllocations.Text = "Can Reverse Allocations:"
        Me.chkIsReverseAllocations.UseVisualStyleBackColor = False
        '
        'chkIsRaiseFee
        '
        Me.chkIsRaiseFee.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRaiseFee.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRaiseFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRaiseFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRaiseFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRaiseFee.Location = New System.Drawing.Point(67, 231)
        Me.chkIsRaiseFee.Name = "chkIsRaiseFee"
        Me.chkIsRaiseFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRaiseFee.Size = New System.Drawing.Size(233, 17)
        Me.chkIsRaiseFee.TabIndex = 125
        Me.chkIsRaiseFee.Text = "Can Raise Fee:"
        Me.chkIsRaiseFee.UseVisualStyleBackColor = False
        '
        'chkIsRaiseManualDID
        '
        Me.chkIsRaiseManualDID.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsRaiseManualDID.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsRaiseManualDID.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsRaiseManualDID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsRaiseManualDID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsRaiseManualDID.Location = New System.Drawing.Point(67, 279)
        Me.chkIsRaiseManualDID.Name = "chkIsRaiseManualDID"
        Me.chkIsRaiseManualDID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsRaiseManualDID.Size = New System.Drawing.Size(233, 17)
        Me.chkIsRaiseManualDID.TabIndex = 129
        Me.chkIsRaiseManualDID.Text = "Can Raise Manual DID:"
        Me.chkIsRaiseManualDID.UseVisualStyleBackColor = False
        '
        'chkIsDeleteClient
        '
        Me.chkIsDeleteClient.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDeleteClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDeleteClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDeleteClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDeleteClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDeleteClient.Location = New System.Drawing.Point(67, 63)
        Me.chkIsDeleteClient.Name = "chkIsDeleteClient"
        Me.chkIsDeleteClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDeleteClient.Size = New System.Drawing.Size(233, 17)
        Me.chkIsDeleteClient.TabIndex = 109
        Me.chkIsDeleteClient.Text = "Can Delete Clients:"
        Me.chkIsDeleteClient.UseVisualStyleBackColor = False
        '
        'chkIsPerformAllocations
        '
        Me.chkIsPerformAllocations.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsPerformAllocations.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsPerformAllocations.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsPerformAllocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsPerformAllocations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsPerformAllocations.Location = New System.Drawing.Point(339, 18)
        Me.chkIsPerformAllocations.Name = "chkIsPerformAllocations"
        Me.chkIsPerformAllocations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsPerformAllocations.Size = New System.Drawing.Size(233, 17)
        Me.chkIsPerformAllocations.TabIndex = 132
        Me.chkIsPerformAllocations.Text = "Can Perform Allocations:"
        Me.chkIsPerformAllocations.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_5
        '
        Me._cmdPrevious_5.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_5.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_5.Name = "_cmdPrevious_5"
        Me._cmdPrevious_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_5.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_5.TabIndex = 150
        Me._cmdPrevious_5.Text = "<<"
        Me._cmdPrevious_5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_5.UseVisualStyleBackColor = False
        '
        'lblReverseReplaceTransactions
        '
        Me.lblReverseReplaceTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.lblReverseReplaceTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReverseReplaceTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReverseReplaceTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReverseReplaceTransactions.Location = New System.Drawing.Point(307, 328)
        Me.lblReverseReplaceTransactions.Name = "lblReverseReplaceTransactions"
        Me.lblReverseReplaceTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReverseReplaceTransactions.Size = New System.Drawing.Size(25, 17)
        Me.lblReverseReplaceTransactions.TabIndex = 235
        Me.lblReverseReplaceTransactions.Text = "No"
        '
        'lblEditSchemePolicy
        '
        Me.lblEditSchemePolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblEditSchemePolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEditSchemePolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditSchemePolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEditSchemePolicy.Location = New System.Drawing.Point(576, 64)
        Me.lblEditSchemePolicy.Name = "lblEditSchemePolicy"
        Me.lblEditSchemePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEditSchemePolicy.Size = New System.Drawing.Size(19, 17)
        Me.lblEditSchemePolicy.TabIndex = 114
        Me.lblEditSchemePolicy.Text = " No"
        '
        'lblDeletePolicy
        '
        Me.lblDeletePolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeletePolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeletePolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeletePolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeletePolicy.Location = New System.Drawing.Point(307, 112)
        Me.lblDeletePolicy.Name = "lblDeletePolicy"
        Me.lblDeletePolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeletePolicy.Size = New System.Drawing.Size(25, 17)
        Me.lblDeletePolicy.TabIndex = 116
        Me.lblDeletePolicy.Text = "No"
        '
        'lblViewClient
        '
        Me.lblViewClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblViewClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblViewClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblViewClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblViewClient.Location = New System.Drawing.Point(307, 20)
        Me.lblViewClient.Name = "lblViewClient"
        Me.lblViewClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblViewClient.Size = New System.Drawing.Size(25, 18)
        Me.lblViewClient.TabIndex = 106
        Me.lblViewClient.Text = "No"
        '
        'lblEditClient
        '
        Me.lblEditClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblEditClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEditClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEditClient.Location = New System.Drawing.Point(307, 39)
        Me.lblEditClient.Name = "lblEditClient"
        Me.lblEditClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEditClient.Size = New System.Drawing.Size(25, 17)
        Me.lblEditClient.TabIndex = 108
        Me.lblEditClient.Text = "No"
        '
        'lblEditPolicy
        '
        Me.lblEditPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblEditPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEditPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEditPolicy.Location = New System.Drawing.Point(307, 87)
        Me.lblEditPolicy.Name = "lblEditPolicy"
        Me.lblEditPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEditPolicy.Size = New System.Drawing.Size(25, 17)
        Me.lblEditPolicy.TabIndex = 112
        Me.lblEditPolicy.Text = "No"
        '
        'lblViewClaim
        '
        Me.lblViewClaim.BackColor = System.Drawing.SystemColors.Control
        Me.lblViewClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblViewClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblViewClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblViewClaim.Location = New System.Drawing.Point(307, 135)
        Me.lblViewClaim.Name = "lblViewClaim"
        Me.lblViewClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblViewClaim.Size = New System.Drawing.Size(25, 17)
        Me.lblViewClaim.TabIndex = 118
        Me.lblViewClaim.Text = "No"
        '
        'lblEditFinancePlan
        '
        Me.lblEditFinancePlan.BackColor = System.Drawing.SystemColors.Control
        Me.lblEditFinancePlan.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEditFinancePlan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditFinancePlan.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEditFinancePlan.Location = New System.Drawing.Point(307, 159)
        Me.lblEditFinancePlan.Name = "lblEditFinancePlan"
        Me.lblEditFinancePlan.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEditFinancePlan.Size = New System.Drawing.Size(25, 17)
        Me.lblEditFinancePlan.TabIndex = 120
        Me.lblEditFinancePlan.Text = "No"
        '
        'lblRaiseDebit
        '
        Me.lblRaiseDebit.BackColor = System.Drawing.SystemColors.Control
        Me.lblRaiseDebit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaiseDebit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaiseDebit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaiseDebit.Location = New System.Drawing.Point(307, 183)
        Me.lblRaiseDebit.Name = "lblRaiseDebit"
        Me.lblRaiseDebit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaiseDebit.Size = New System.Drawing.Size(25, 17)
        Me.lblRaiseDebit.TabIndex = 122
        Me.lblRaiseDebit.Text = "No"
        '
        'lblRaiseCredit
        '
        Me.lblRaiseCredit.BackColor = System.Drawing.SystemColors.Control
        Me.lblRaiseCredit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaiseCredit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaiseCredit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaiseCredit.Location = New System.Drawing.Point(307, 207)
        Me.lblRaiseCredit.Name = "lblRaiseCredit"
        Me.lblRaiseCredit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaiseCredit.Size = New System.Drawing.Size(25, 17)
        Me.lblRaiseCredit.TabIndex = 124
        Me.lblRaiseCredit.Text = "No"
        '
        'lblRaiseFee
        '
        Me.lblRaiseFee.BackColor = System.Drawing.SystemColors.Control
        Me.lblRaiseFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaiseFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaiseFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaiseFee.Location = New System.Drawing.Point(307, 231)
        Me.lblRaiseFee.Name = "lblRaiseFee"
        Me.lblRaiseFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaiseFee.Size = New System.Drawing.Size(25, 17)
        Me.lblRaiseFee.TabIndex = 126
        Me.lblRaiseFee.Text = "No"
        '
        'lblRaiseCash
        '
        Me.lblRaiseCash.BackColor = System.Drawing.SystemColors.Control
        Me.lblRaiseCash.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaiseCash.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaiseCash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaiseCash.Location = New System.Drawing.Point(307, 255)
        Me.lblRaiseCash.Name = "lblRaiseCash"
        Me.lblRaiseCash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaiseCash.Size = New System.Drawing.Size(25, 17)
        Me.lblRaiseCash.TabIndex = 128
        Me.lblRaiseCash.Text = "No"
        '
        'lblReverseTransactions
        '
        Me.lblReverseTransactions.BackColor = System.Drawing.SystemColors.Control
        Me.lblReverseTransactions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReverseTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReverseTransactions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReverseTransactions.Location = New System.Drawing.Point(307, 303)
        Me.lblReverseTransactions.Name = "lblReverseTransactions"
        Me.lblReverseTransactions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReverseTransactions.Size = New System.Drawing.Size(25, 17)
        Me.lblReverseTransactions.TabIndex = 131
        Me.lblReverseTransactions.Text = "No"
        '
        'lblReverseAllocations
        '
        Me.lblReverseAllocations.BackColor = System.Drawing.SystemColors.Control
        Me.lblReverseAllocations.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReverseAllocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReverseAllocations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReverseAllocations.Location = New System.Drawing.Point(579, 39)
        Me.lblReverseAllocations.Name = "lblReverseAllocations"
        Me.lblReverseAllocations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReverseAllocations.Size = New System.Drawing.Size(25, 17)
        Me.lblReverseAllocations.TabIndex = 135
        Me.lblReverseAllocations.Text = "No"
        '
        'lblRaiseManualDID
        '
        Me.lblRaiseManualDID.BackColor = System.Drawing.SystemColors.Control
        Me.lblRaiseManualDID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRaiseManualDID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRaiseManualDID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRaiseManualDID.Location = New System.Drawing.Point(307, 279)
        Me.lblRaiseManualDID.Name = "lblRaiseManualDID"
        Me.lblRaiseManualDID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRaiseManualDID.Size = New System.Drawing.Size(25, 17)
        Me.lblRaiseManualDID.TabIndex = 130
        Me.lblRaiseManualDID.Text = "No"
        '
        'lblDeleteClient
        '
        Me.lblDeleteClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblDeleteClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDeleteClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDeleteClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDeleteClient.Location = New System.Drawing.Point(307, 63)
        Me.lblDeleteClient.Name = "lblDeleteClient"
        Me.lblDeleteClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDeleteClient.Size = New System.Drawing.Size(25, 17)
        Me.lblDeleteClient.TabIndex = 110
        Me.lblDeleteClient.Text = "No"
        '
        'lblPerformAllocations
        '
        Me.lblPerformAllocations.BackColor = System.Drawing.SystemColors.Control
        Me.lblPerformAllocations.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPerformAllocations.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPerformAllocations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPerformAllocations.Location = New System.Drawing.Point(579, 18)
        Me.lblPerformAllocations.Name = "lblPerformAllocations"
        Me.lblPerformAllocations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPerformAllocations.Size = New System.Drawing.Size(25, 17)
        Me.lblPerformAllocations.TabIndex = 133
        Me.lblPerformAllocations.Text = "No"
        '
        '_SSTab1_TabPage7
        '
        Me._SSTab1_TabPage7.Controls.Add(Me.fmePartyEdit)
        Me._SSTab1_TabPage7.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage7.Name = "_SSTab1_TabPage7"
        Me._SSTab1_TabPage7.Size = New System.Drawing.Size(824, 422)
        Me._SSTab1_TabPage7.TabIndex = 7
        Me._SSTab1_TabPage7.Text = "8 - Party Edit"
        Me._SSTab1_TabPage7.UseVisualStyleBackColor = True
        '
        'fmePartyEdit
        '
        Me.fmePartyEdit.BackColor = System.Drawing.SystemColors.Control
        Me.fmePartyEdit.Controls.Add(Me._cmdPrevious_6)
        Me.fmePartyEdit.Controls.Add(Me.chkOtherPartyMaintenance)
        Me.fmePartyEdit.Controls.Add(Me.chkInsurerMaintenance)
        Me.fmePartyEdit.Controls.Add(Me.chkAccountExecutive)
        Me.fmePartyEdit.Controls.Add(Me.chkAccountHandler)
        Me.fmePartyEdit.Controls.Add(Me.chkAgentMaintenance)
        Me.fmePartyEdit.Controls.Add(Me.chkIsViewClientManager)
        Me.fmePartyEdit.Controls.Add(Me.lblIsViewOnly)
        Me.fmePartyEdit.Controls.Add(Me.lblPartyCaption)
        Me.fmePartyEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fmePartyEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmePartyEdit.Location = New System.Drawing.Point(12, 10)
        Me.fmePartyEdit.Name = "fmePartyEdit"
        Me.fmePartyEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmePartyEdit.Size = New System.Drawing.Size(650, 373)
        Me.fmePartyEdit.TabIndex = 158
        Me.fmePartyEdit.TabStop = False
        '
        '_cmdPrevious_6
        '
        Me._cmdPrevious_6.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_6.Location = New System.Drawing.Point(4, 350)
        Me._cmdPrevious_6.Name = "_cmdPrevious_6"
        Me._cmdPrevious_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_6.Size = New System.Drawing.Size(40, 19)
        Me._cmdPrevious_6.TabIndex = 168
        Me._cmdPrevious_6.Text = "<<"
        Me._cmdPrevious_6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_6.UseVisualStyleBackColor = False
        '
        'chkOtherPartyMaintenance
        '
        Me.chkOtherPartyMaintenance.BackColor = System.Drawing.SystemColors.Control
        Me.chkOtherPartyMaintenance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOtherPartyMaintenance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOtherPartyMaintenance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOtherPartyMaintenance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOtherPartyMaintenance.Location = New System.Drawing.Point(72, 176)
        Me.chkOtherPartyMaintenance.Name = "chkOtherPartyMaintenance"
        Me.chkOtherPartyMaintenance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOtherPartyMaintenance.Size = New System.Drawing.Size(299, 17)
        Me.chkOtherPartyMaintenance.TabIndex = 166
        Me.chkOtherPartyMaintenance.Text = "Other Party Information"
        Me.chkOtherPartyMaintenance.UseVisualStyleBackColor = False
        '
        'chkInsurerMaintenance
        '
        Me.chkInsurerMaintenance.BackColor = System.Drawing.SystemColors.Control
        Me.chkInsurerMaintenance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkInsurerMaintenance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInsurerMaintenance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInsurerMaintenance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInsurerMaintenance.Location = New System.Drawing.Point(72, 104)
        Me.chkInsurerMaintenance.Name = "chkInsurerMaintenance"
        Me.chkInsurerMaintenance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInsurerMaintenance.Size = New System.Drawing.Size(299, 17)
        Me.chkInsurerMaintenance.TabIndex = 163
        Me.chkInsurerMaintenance.Text = "Reinsurers/Insurers Information"
        Me.chkInsurerMaintenance.UseVisualStyleBackColor = False
        '
        'chkAccountExecutive
        '
        Me.chkAccountExecutive.BackColor = System.Drawing.SystemColors.Control
        Me.chkAccountExecutive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAccountExecutive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAccountExecutive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAccountExecutive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAccountExecutive.Location = New System.Drawing.Point(72, 152)
        Me.chkAccountExecutive.Name = "chkAccountExecutive"
        Me.chkAccountExecutive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAccountExecutive.Size = New System.Drawing.Size(299, 17)
        Me.chkAccountExecutive.TabIndex = 165
        Me.chkAccountExecutive.Text = "Account Executives Information"
        Me.chkAccountExecutive.UseVisualStyleBackColor = False
        '
        'chkAccountHandler
        '
        Me.chkAccountHandler.BackColor = System.Drawing.SystemColors.Control
        Me.chkAccountHandler.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAccountHandler.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAccountHandler.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAccountHandler.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAccountHandler.Location = New System.Drawing.Point(72, 128)
        Me.chkAccountHandler.Name = "chkAccountHandler"
        Me.chkAccountHandler.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAccountHandler.Size = New System.Drawing.Size(299, 17)
        Me.chkAccountHandler.TabIndex = 164
        Me.chkAccountHandler.Text = "Account Handler Information"
        Me.chkAccountHandler.UseVisualStyleBackColor = False
        '
        'chkAgentMaintenance
        '
        Me.chkAgentMaintenance.BackColor = System.Drawing.SystemColors.Control
        Me.chkAgentMaintenance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAgentMaintenance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAgentMaintenance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgentMaintenance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAgentMaintenance.Location = New System.Drawing.Point(72, 80)
        Me.chkAgentMaintenance.Name = "chkAgentMaintenance"
        Me.chkAgentMaintenance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAgentMaintenance.Size = New System.Drawing.Size(299, 17)
        Me.chkAgentMaintenance.TabIndex = 162
        Me.chkAgentMaintenance.Text = "Agents Information"
        Me.chkAgentMaintenance.UseVisualStyleBackColor = False
        '
        'chkIsViewClientManager
        '
        Me.chkIsViewClientManager.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsViewClientManager.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsViewClientManager.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsViewClientManager.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsViewClientManager.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsViewClientManager.Location = New System.Drawing.Point(72, 56)
        Me.chkIsViewClientManager.Name = "chkIsViewClientManager"
        Me.chkIsViewClientManager.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsViewClientManager.Size = New System.Drawing.Size(299, 17)
        Me.chkIsViewClientManager.TabIndex = 161
        Me.chkIsViewClientManager.Text = "Clients (via Client Manager)"
        Me.chkIsViewClientManager.UseVisualStyleBackColor = False
        '
        'lblIsViewOnly
        '
        Me.lblIsViewOnly.AutoSize = True
        Me.lblIsViewOnly.BackColor = System.Drawing.SystemColors.Control
        Me.lblIsViewOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIsViewOnly.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsViewOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIsViewOnly.Location = New System.Drawing.Point(352, 32)
        Me.lblIsViewOnly.Name = "lblIsViewOnly"
        Me.lblIsViewOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIsViewOnly.Size = New System.Drawing.Size(87, 13)
        Me.lblIsViewOnly.TabIndex = 160
        Me.lblIsViewOnly.Text = "Is View Only"
        '
        'lblPartyCaption
        '
        Me.lblPartyCaption.AutoSize = True
        Me.lblPartyCaption.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCaption.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCaption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCaption.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCaption.Location = New System.Drawing.Point(72, 32)
        Me.lblPartyCaption.Name = "lblPartyCaption"
        Me.lblPartyCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCaption.Size = New System.Drawing.Size(42, 13)
        Me.lblPartyCaption.TabIndex = 159
        Me.lblPartyCaption.Text = "Party"
        '
        'txtFilter
        '
        Me.txtFilter.AcceptsReturn = True
        Me.txtFilter.BackColor = System.Drawing.SystemColors.Window
        Me.txtFilter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFilter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFilter.Location = New System.Drawing.Point(16, 20)
        Me.txtFilter.MaxLength = 0
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFilter.Size = New System.Drawing.Size(137, 20)
        Me.txtFilter.TabIndex = 1
        '
        'chkHideDeleted
        '
        Me.chkHideDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkHideDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkHideDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHideDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHideDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHideDeleted.Location = New System.Drawing.Point(17, 477)
        Me.chkHideDeleted.Name = "chkHideDeleted"
        Me.chkHideDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHideDeleted.Size = New System.Drawing.Size(136, 17)
        Me.chkHideDeleted.TabIndex = 3
        Me.chkHideDeleted.Text = "Show Deleted Users"
        Me.chkHideDeleted.UseVisualStyleBackColor = False
        '
        'Combo1
        '
        Me.Combo1.BackColor = System.Drawing.SystemColors.Window
        Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Combo1.Location = New System.Drawing.Point(2168, 164)
        Me.Combo1.Name = "Combo1"
        Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Combo1.Size = New System.Drawing.Size(137, 21)
        Me.Combo1.TabIndex = 138
        Me.Combo1.Text = "Combo1"
        '
        '_tabMain_TabPage1
        '
        Me._tabMain_TabPage1.Controls.Add(Me.fraMatched)
        Me._tabMain_TabPage1.Controls.Add(Me.fraUnmatched)
        Me._tabMain_TabPage1.Controls.Add(Me.fraSystemSecurity)
        Me._tabMain_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage1.Name = "_tabMain_TabPage1"
        Me._tabMain_TabPage1.Size = New System.Drawing.Size(998, 516)
        Me._tabMain_TabPage1.TabIndex = 1
        Me._tabMain_TabPage1.Text = "System Security"
        Me._tabMain_TabPage1.UseVisualStyleBackColor = True
        '
        'fraMatched
        '
        Me.fraMatched.BackColor = System.Drawing.SystemColors.Control
        Me.fraMatched.Controls.Add(Me.cmdUnmatch)
        Me.fraMatched.Controls.Add(Me.lvwMatchedUsers)
        Me.fraMatched.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMatched.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMatched.Location = New System.Drawing.Point(24, 308)
        Me.fraMatched.Name = "fraMatched"
        Me.fraMatched.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMatched.Size = New System.Drawing.Size(729, 161)
        Me.fraMatched.TabIndex = 144
        Me.fraMatched.TabStop = False
        Me.fraMatched.Text = "Matched"
        '
        'lvwMatchedUsers
        '
        Me.lvwMatchedUsers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwMatchedUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMatchedUsers_ColumnHeader_1, Me._lvwMatchedUsers_ColumnHeader_2})
        Me.lvwMatchedUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMatchedUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwMatchedUsers.FullRowSelect = True
        Me.lvwMatchedUsers.HideSelection = False
        Me.lvwMatchedUsers.LargeImageList = Me.imgGroup
        Me.lvwMatchedUsers.Location = New System.Drawing.Point(8, 16)
        Me.lvwMatchedUsers.Name = "lvwMatchedUsers"
        Me.lvwMatchedUsers.Size = New System.Drawing.Size(633, 137)
        Me.lvwMatchedUsers.SmallImageList = Me.imgGroup
        Me.lvwMatchedUsers.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwMatchedUsers.TabIndex = 145
        Me.lvwMatchedUsers.UseCompatibleStateImageBehavior = False
        Me.lvwMatchedUsers.View = System.Windows.Forms.View.Details
        '
        '_lvwMatchedUsers_ColumnHeader_1
        '
        Me._lvwMatchedUsers_ColumnHeader_1.Text = "Sirius User"
        Me._lvwMatchedUsers_ColumnHeader_1.Width = 201
        '
        '_lvwMatchedUsers_ColumnHeader_2
        '
        Me._lvwMatchedUsers_ColumnHeader_2.Text = "Domain User"
        Me._lvwMatchedUsers_ColumnHeader_2.Width = 334
        '
        'fraUnmatched
        '
        Me.fraUnmatched.BackColor = System.Drawing.SystemColors.Control
        Me.fraUnmatched.Controls.Add(Me.cboDomain)
        Me.fraUnmatched.Controls.Add(Me.cmdMatch)
        Me.fraUnmatched.Controls.Add(Me.lvwSiriusUsers)
        Me.fraUnmatched.Controls.Add(Me.lvwDomainUsers)
        Me.fraUnmatched.Controls.Add(Me.txtLDAPDomain)
        Me.fraUnmatched.Controls.Add(Me.lblUnmatchedDomain)
        Me.fraUnmatched.Controls.Add(Me.lblUnmatchedUsers)
        Me.fraUnmatched.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraUnmatched.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraUnmatched.Location = New System.Drawing.Point(24, 84)
        Me.fraUnmatched.Name = "fraUnmatched"
        Me.fraUnmatched.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraUnmatched.Size = New System.Drawing.Size(729, 217)
        Me.fraUnmatched.TabIndex = 139
        Me.fraUnmatched.TabStop = False
        Me.fraUnmatched.Text = "Unmatched"
        '
        'cboDomain
        '
        Me.cboDomain.BackColor = System.Drawing.SystemColors.Window
        Me.cboDomain.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDomain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDomain.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDomain.Location = New System.Drawing.Point(480, 16)
        Me.cboDomain.Name = "cboDomain"
        Me.cboDomain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDomain.Size = New System.Drawing.Size(161, 21)
        Me.cboDomain.TabIndex = 140
        '
        'lvwSiriusUsers
        '
        Me.lvwSiriusUsers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSiriusUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSiriusUsers_ColumnHeader_1, Me._lvwSiriusUsers_ColumnHeader_2})
        Me.lvwSiriusUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSiriusUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSiriusUsers.FullRowSelect = True
        Me.lvwSiriusUsers.HideSelection = False
        Me.lvwSiriusUsers.LargeImageList = Me.imgGroup
        Me.lvwSiriusUsers.Location = New System.Drawing.Point(8, 40)
        Me.lvwSiriusUsers.Name = "lvwSiriusUsers"
        Me.lvwSiriusUsers.Size = New System.Drawing.Size(313, 169)
        Me.lvwSiriusUsers.SmallImageList = Me.imgGroup
        Me.lvwSiriusUsers.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSiriusUsers.TabIndex = 141
        Me.lvwSiriusUsers.UseCompatibleStateImageBehavior = False
        Me.lvwSiriusUsers.View = System.Windows.Forms.View.Details
        '
        '_lvwSiriusUsers_ColumnHeader_1
        '
        Me._lvwSiriusUsers_ColumnHeader_1.Text = "Sirius User list"
        Me._lvwSiriusUsers_ColumnHeader_1.Width = 334
        '
        '_lvwSiriusUsers_ColumnHeader_2
        '
        Me._lvwSiriusUsers_ColumnHeader_2.Text = "UserId"
        Me._lvwSiriusUsers_ColumnHeader_2.Width = 0
        '
        'lvwDomainUsers
        '
        Me.lvwDomainUsers.BackColor = System.Drawing.SystemColors.Window
        Me.lvwDomainUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwDomainUsers_ColumnHeader_1})
        Me.lvwDomainUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDomainUsers.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwDomainUsers.FullRowSelect = True
        Me.lvwDomainUsers.HideSelection = False
        Me.lvwDomainUsers.LargeImageList = Me.imgGroup
        Me.lvwDomainUsers.Location = New System.Drawing.Point(328, 40)
        Me.lvwDomainUsers.Name = "lvwDomainUsers"
        Me.lvwDomainUsers.Size = New System.Drawing.Size(313, 169)
        Me.lvwDomainUsers.SmallImageList = Me.imgGroup
        Me.lvwDomainUsers.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDomainUsers.TabIndex = 142
        Me.lvwDomainUsers.UseCompatibleStateImageBehavior = False
        Me.lvwDomainUsers.View = System.Windows.Forms.View.Details
        '
        '_lvwDomainUsers_ColumnHeader_1
        '
        Me._lvwDomainUsers_ColumnHeader_1.Text = "Domain User list"
        Me._lvwDomainUsers_ColumnHeader_1.Width = 334
        '
        'txtLDAPDomain
        '
        Me.txtLDAPDomain.AcceptsReturn = True
        Me.txtLDAPDomain.BackColor = System.Drawing.SystemColors.Window
        Me.txtLDAPDomain.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLDAPDomain.Enabled = False
        Me.txtLDAPDomain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLDAPDomain.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLDAPDomain.Location = New System.Drawing.Point(480, 16)
        Me.txtLDAPDomain.MaxLength = 0
        Me.txtLDAPDomain.Name = "txtLDAPDomain"
        Me.txtLDAPDomain.ReadOnly = True
        Me.txtLDAPDomain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLDAPDomain.Size = New System.Drawing.Size(161, 20)
        Me.txtLDAPDomain.TabIndex = 151
        '
        'lblUnmatchedDomain
        '
        Me.lblUnmatchedDomain.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnmatchedDomain.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnmatchedDomain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnmatchedDomain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnmatchedDomain.Location = New System.Drawing.Point(416, 20)
        Me.lblUnmatchedDomain.Name = "lblUnmatchedDomain"
        Me.lblUnmatchedDomain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnmatchedDomain.Size = New System.Drawing.Size(81, 17)
        Me.lblUnmatchedDomain.TabIndex = 148
        Me.lblUnmatchedDomain.Text = "Domain:"
        '
        'lblUnmatchedUsers
        '
        Me.lblUnmatchedUsers.BackColor = System.Drawing.SystemColors.Control
        Me.lblUnmatchedUsers.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUnmatchedUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnmatchedUsers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnmatchedUsers.Location = New System.Drawing.Point(8, 24)
        Me.lblUnmatchedUsers.Name = "lblUnmatchedUsers"
        Me.lblUnmatchedUsers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnmatchedUsers.Size = New System.Drawing.Size(129, 17)
        Me.lblUnmatchedUsers.TabIndex = 147
        Me.lblUnmatchedUsers.Text = "Sirius Users:"
        '
        'fraSystemSecurity
        '
        Me.fraSystemSecurity.BackColor = System.Drawing.SystemColors.Control
        Me.fraSystemSecurity.Controls.Add(Me.cboSystemSecurity)
        Me.fraSystemSecurity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSystemSecurity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSystemSecurity.Location = New System.Drawing.Point(24, 20)
        Me.fraSystemSecurity.Name = "fraSystemSecurity"
        Me.fraSystemSecurity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSystemSecurity.Size = New System.Drawing.Size(729, 57)
        Me.fraSystemSecurity.TabIndex = 136
        Me.fraSystemSecurity.TabStop = False
        Me.fraSystemSecurity.Text = "System Security Model"
        '
        'cboSystemSecurity
        '
        Me.cboSystemSecurity.BackColor = System.Drawing.SystemColors.Window
        Me.cboSystemSecurity.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSystemSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSystemSecurity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSystemSecurity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSystemSecurity.Location = New System.Drawing.Point(8, 24)
        Me.cboSystemSecurity.Name = "cboSystemSecurity"
        Me.cboSystemSecurity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSystemSecurity.Size = New System.Drawing.Size(265, 21)
        Me.cboSystemSecurity.TabIndex = 137
        '
        'Ctx_mnuSupervisor
        '
        Me.Ctx_mnuSupervisor.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.Ctx_mnuSupervisor.Name = "Ctx_mnuSupervisor"
        Me.Ctx_mnuSupervisor.Size = New System.Drawing.Size(61, 4)
        '
        'mnuSupervisor
        '
        Me.mnuSupervisor.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSuper})
        Me.mnuSupervisor.Enabled = False
        Me.mnuSupervisor.Name = "mnuSupervisor"
        Me.mnuSupervisor.Size = New System.Drawing.Size(74, 32)
        Me.mnuSupervisor.Text = "Supervisor"
        Me.mnuSupervisor.Visible = False
        '
        'mnuSuper
        '
        Me.mnuSuper.Name = "mnuSuper"
        Me.mnuSuper.Size = New System.Drawing.Size(129, 22)
        Me.mnuSuper.Text = "Supervisor"
        '
        'MainMenu1
        '
        Me.MainMenu1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSupervisor})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(1018, 24)
        Me.MainMenu1.TabIndex = 42
        Me.MainMenu1.Visible = False
        '
        'txtCurrencyAmount
        '
        Me.txtCurrencyAmount.AcceptsReturn = True
        Me.txtCurrencyAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrencyAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrencyAmount.Enabled = False
        Me.txtCurrencyAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrencyAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrencyAmount.Location = New System.Drawing.Point(92, 119)
        Me.txtCurrencyAmount.MaxLength = 0
        Me.txtCurrencyAmount.Name = "txtCurrencyAmount"
        Me.txtCurrencyAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrencyAmount.Size = New System.Drawing.Size(101, 20)
        Me.txtCurrencyAmount.TabIndex = 231
        '
        'CurrencyLookup1
        '
        Me.CurrencyLookup1.CompanyId = 0
        Me.CurrencyLookup1.CurrencyId = 0
        Me.CurrencyLookup1.DefaultCurrencyId = 0
        Me.CurrencyLookup1.FirstItem = ""
        Me.CurrencyLookup1.ListIndex = -1
        Me.CurrencyLookup1.Location = New System.Drawing.Point(92, 118)
        Me.CurrencyLookup1.Name = "CurrencyLookup1"
        Me.CurrencyLookup1.RestrictTo = UserControls.CurrencyLookup.RestrictToCurrency.actBaseCurrencies
        Me.CurrencyLookup1.Size = New System.Drawing.Size(161, 21)
        Me.CurrencyLookup1.TabIndex = 233
        Me.CurrencyLookup1.ToolTipText = ""
        Me.CurrencyLookup1.WhatsThisHelpID = 0
        '
        'frmUserMaintenance
        '
        Me.AcceptButton = Me._cmdNext_0
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1034, 603)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUserMaintenance"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User Maintenance"
        Me.pnlEffectiveDate.ResumeLayout(False)
        Me.pnlEffectiveDate.PerformLayout()
        Me.pnlUsername.ResumeLayout(False)
        Me.pnlUsername.PerformLayout()
        Me.pnlLastLogin.ResumeLayout(False)
        Me.pnlLastLogin.PerformLayout()
        Me.pnlPasswordChange.ResumeLayout(False)
        Me.pnlPasswordChange.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.Frame0.ResumeLayout(False)
        Me.Frame0.PerformLayout()
        Me.fraPrinter.ResumeLayout(False)
        Me.fraPrinter.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame7.ResumeLayout(False)
        Me._pnlAccHandler_0.ResumeLayout(False)
        Me._pnlAccHandler_0.PerformLayout()
        Me._pnlAccHandler_1.ResumeLayout(False)
        Me._pnlAccHandler_1.PerformLayout()
        Me._pnlAccHandler_2.ResumeLayout(False)
        Me._pnlAccHandler_2.PerformLayout()
        Me._pnlAgent_0.ResumeLayout(False)
        Me._pnlAgent_0.PerformLayout()
        Me._pnlAgent_1.ResumeLayout(False)
        Me._pnlAgent_1.PerformLayout()
        Me.pnlOtherParty.ResumeLayout(False)
        Me.pnlOtherParty.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.fraDomainAccount.ResumeLayout(False)
        Me.pnlDomainUserName.ResumeLayout(False)
        Me.pnlDomainUserName.PerformLayout()
        CType(Me.imgSignature, System.ComponentModel.ISupportInitialize).EndInit()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.Frame3.ResumeLayout(False)
        Me._SSTab1_TabPage3.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me._SSTab1_TabPage4.ResumeLayout(False)
        Me.fmeAuthorities.ResumeLayout(False)
        Me.SSTab2.ResumeLayout(False)
        Me._SSTab2_TabPage0.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.fmeInstalmentDetail.ResumeLayout(False)
        Me.fmeInstalmentDetail.PerformLayout()
        Me.fmeReceiptreversal.ResumeLayout(False)
        Me.fmeAllocationReversal.ResumeLayout(False)
        Me.fmeAllocationReversal.PerformLayout()
        Me.fmeWriteOffs.ResumeLayout(False)
        Me.fmeWriteOffs.PerformLayout()
        Me.fmeOverride.ResumeLayout(False)
        Me._SSTab2_TabPage1.ResumeLayout(False)
        Me.Frame8.ResumeLayout(False)
        Me.Frame10.ResumeLayout(False)
        Me.Frame10.PerformLayout()
        Me.fmePayments.ResumeLayout(False)
        Me.fmePayments.PerformLayout()
        Me.fmeClaimPayments.ResumeLayout(False)
        Me.fmeClaimPayments.PerformLayout()
        Me._sstab2_TabPage4.ResumeLayout(False)
        Me.fmeManualjournal.ResumeLayout(False)
        Me.fmeManualjournal.PerformLayout()
        Me._SSTab2_TabPage2.ResumeLayout(False)
        Me.Frame9.ResumeLayout(False)
        Me.fraReinsurance.ResumeLayout(False)
        Me.fraRatingSections.ResumeLayout(False)
        Me.fraMakeLive.ResumeLayout(False)
        Me.fraMakeLive.PerformLayout()
        Me.fraCommSettings.ResumeLayout(False)
        Me.fmeAccess.ResumeLayout(False)
        Me.fraBrokerAgentPortfolioTransfer.ResumeLayout(False)
        Me.fraVoidTransaction.ResumeLayout(False)
        Me._SSTab2_TabPage3.ResumeLayout(False)
        Me.Frame11.ResumeLayout(False)
        Me._SSTab1_TabPage5.ResumeLayout(False)
        Me.Frame6.ResumeLayout(False)
        Me._SSTab1_TabPage6.ResumeLayout(False)
        Me.frmClientManagerSecurity.ResumeLayout(False)
        Me._SSTab1_TabPage7.ResumeLayout(False)
        Me.fmePartyEdit.ResumeLayout(False)
        Me.fmePartyEdit.PerformLayout()
        Me._tabMain_TabPage1.ResumeLayout(False)
        Me.fraMatched.ResumeLayout(False)
        Me.fraUnmatched.ResumeLayout(False)
        Me.fraUnmatched.PerformLayout()
        Me.fraSystemSecurity.ResumeLayout(False)
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializepnlAgent()
        Me.pnlAgent(0) = _pnlAgent_0
        Me.pnlAgent(1) = _pnlAgent_1
    End Sub
    Sub InitializepnlAccHandler()
        Me.pnlAccHandler(0) = _pnlAccHandler_0
        Me.pnlAccHandler(1) = _pnlAccHandler_1
        Me.pnlAccHandler(2) = _pnlAccHandler_2
    End Sub
    Sub InitializelblAgentYN()
        Me.lblAgentYN(1) = _lblAgentYN_1
        Me.lblAgentYN(0) = _lblAgentYN_0
    End Sub
    Sub InitializelblAccHandlerYN()
        Me.lblAccHandlerYN(0) = _lblAccHandlerYN_0
        Me.lblAccHandlerYN(1) = _lblAccHandlerYN_1
        Me.lblAccHandlerYN(2) = _lblAccHandlerYN_2
    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(3) = _cmdPrevious_3
        Me.cmdPrevious(6) = _cmdPrevious_6
        Me.cmdPrevious(5) = _cmdPrevious_5
        Me.cmdPrevious(4) = _cmdPrevious_4
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(4) = _cmdNext_4
        Me.cmdNext(6) = _cmdNext_6
        Me.cmdNext(0) = _cmdNext_0
        Me.cmdNext(5) = _cmdNext_5
        Me.cmdNext(3) = _cmdNext_3
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
    End Sub
    Sub InitializecmdAgent()
        Me.cmdAgent(1) = _cmdAgent_1
        Me.cmdAgent(0) = _cmdAgent_0
    End Sub
    Sub InitializecmdAccHandler()
        Me.cmdAccHandler(0) = _cmdAccHandler_0
        Me.cmdAccHandler(1) = _cmdAccHandler_1
        Me.cmdAccHandler(2) = _cmdAccHandler_2
    End Sub
    Sub InitializechkAgent()
        Me.chkAgent(1) = _chkAgent_1
        Me.chkAgent(0) = _chkAgent_0
    End Sub
    Sub InitializechkAccHandler()
        Me.chkAccHandler(0) = _chkAccHandler_0
        Me.chkAccHandler(1) = _chkAccHandler_1
        Me.chkAccHandler(2) = _chkAccHandler_2
    End Sub
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblEffectiveDatePanel As System.Windows.Forms.Label
    Friend WithEvents lblUsernamePanel As System.Windows.Forms.Label
    Friend WithEvents lblLastLoginPanel As System.Windows.Forms.Label
    Friend WithEvents lblPasswordChangePanel As System.Windows.Forms.Label
    Friend WithEvents lblDomainUserNamePanel As System.Windows.Forms.Label
    Friend WithEvents lblAccHandlerPanel As System.Windows.Forms.Label
    Friend WithEvents lblAccExecutivePanel As System.Windows.Forms.Label
    Friend WithEvents lblClaimsHandlerPanel As System.Windows.Forms.Label
    Friend WithEvents lblAgentPanel As System.Windows.Forms.Label
    Friend WithEvents lblInsurerPanel As System.Windows.Forms.Label
    Friend WithEvents lblOtherPartypanel As System.Windows.Forms.Label
    Public WithEvents mnuSupervisor As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSuper As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents chkEditAgentDuringMTAMTC As System.Windows.Forms.CheckBox
    Friend WithEvents fraCommSettings As System.Windows.Forms.GroupBox
    Friend WithEvents chkEditDefaultCommissionMTR As System.Windows.Forms.CheckBox
    Friend WithEvents chkEditDefaultCommissionMTC As System.Windows.Forms.CheckBox
    Friend WithEvents chkEditDefaultCommissionMTA As System.Windows.Forms.CheckBox
    Friend WithEvents chkEditDefaultCommissionNBRN As System.Windows.Forms.CheckBox
    Public WithEvents fmeReceiptreversal As System.Windows.Forms.GroupBox
    Public WithEvents chkReceiptReversal As System.Windows.Forms.CheckBox
    Public WithEvents chkViewBatchProcessStatus As CheckBox
    Public WithEvents lblViewBatchProcessStatus As Label
    Public WithEvents chkCanExtractClientData As System.Windows.Forms.CheckBox
    Public WithEvents lblCanExtractClientData As System.Windows.Forms.Label
    Public WithEvents chkCanChangeInstalmentPlanDefaultCurrecny As System.Windows.Forms.CheckBox
    Public WithEvents chkInstalmentStatus As System.Windows.Forms.CheckBox
    Friend WithEvents fmeInstalmentDetail As System.Windows.Forms.GroupBox
    Public WithEvents chkEditInstalment As System.Windows.Forms.CheckBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Public WithEvents txtEditInstalmentByNoofDays As System.Windows.Forms.TextBox
    Public WithEvents txtCurrencyLossGainLimit As TextBox
    Public WithEvents lblCurrencyAmount As Label
    Friend WithEvents lblCurrencyWriteoff As Label
    Public WithEvents txtCurrencyAmount As TextBox
    Public WithEvents cboLossGainCurrency As UserControls.CurrencyLookup
    Public WithEvents lblLossGainCurrency As Label
    Public WithEvents CurrencyLookup1 As UserControls.CurrencyLookup
    Friend WithEvents _sstab2_TabPage4 As TabPage
    Public WithEvents fmeManualjournal As GroupBox
    Public WithEvents ChkManualJournal As CheckBox
    Public WithEvents txtjournalAmount As TextBox
    Public WithEvents cboJournalCurrency As UserControls.CurrencyLookup
    Public WithEvents lblJournalCurrency As Label
    Public WithEvents lblJournalAmount As Label
    Public WithEvents lblmanulJournal As Label
    Friend WithEvents txtSSOPreferredName As TextBox
    Friend WithEvents lblSSOPreferredName As Label
#End Region
End Class