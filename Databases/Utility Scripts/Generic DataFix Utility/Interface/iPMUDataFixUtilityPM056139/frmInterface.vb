Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports System.Text.RegularExpressions
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	' Stores the return value for the a function call.
	Private m_lReturn As Integer
	
	'failed transaction list
	Private Const ACFieldExportFolderCnt As Integer = 0
	Private Const ACFieldInsuranceFileCnt As Integer = 1
	Private Const ACFieldDocumentRef As Integer = 2
	Private Const ACFieldInsuranceRef As Integer = 3
	Private Const ACFieldPolicyStart As Integer = 4
	Private Const ACFieldPolicyEnd As Integer = 5
	Private Const ACFieldClient As Integer = 6
	Private Const ACFieldPostingStatus As Integer = 7
	Private Const ACFieldDeleted As Integer = 8
	
	'policy version list
	Private Const ACFieldPVInsuranceFileCnt As Integer = 0
    Private Const ACFieldPVInsuranceRef As Integer = 1
    Private Const ACFieldPVPolicyTypeID As Integer = 2
    Private Const ACFieldPVPolicyType As Integer = 3
    Private Const ACFieldPVDocumentRef As Integer = 4
    Private Const ACFieldPVPolicyStart As Integer = 5
    Private Const ACFieldPVDocumentDate As Integer = 6

    'Allocation detail list
    Private Const ACFieldPVAllocationId As Integer = 0
    Private Const ACFieldPVOriginalDocRef As Integer = 1
    Private Const ACFieldPVAllocatedDocRef As Integer = 2
    Private Const ACFieldPVAssociatedDocRef As Integer = 3
    Private Const ACFieldPVFACAccountId As Integer = 4
    Private Const ACFieldPVFACAccount As Integer = 5

	'failed claim list
	Private Const ACFieldFCClaimNumber As Integer = 0
	Private Const ACFieldFCDocRef As Integer = 1
	Private Const ACFieldFCDocDate As Integer = 2
	Private Const ACFieldFCPaymentDate As Integer = 3
	Private Const ACFieldFCPremiumTotal As Integer = 4
	Private Const ACFieldFCPaymentAmount As Integer = 5
	Private Const ACFieldFCPaymentPartyCode As Integer = 6
	Private Const ACFieldFCPostingStatus As Integer = 7
	Private Const ACFieldFCExportFolderCnt As Integer = 8
	Private Const ACFieldFCInsuranceFileCnt As Integer = 9
	Private Const ACFieldFCClaimID As Integer = 10
	Private Const ACFieldFCPaymentPartyCnt As Integer = 11
	Private Const ACFieldFCOriginalReserveID As Integer = 12
	Private Const ACFieldFCPaymentID As Integer = 13
	
	'imbalance closed claim
	Private Const ACFieldICCClaimID As Integer = 0
	Private Const ACFieldICCClaimNumber As Integer = 1
	Private Const ACFieldICCCLO As Integer = 2
	Private Const ACFieldICCInitReserve As Integer = 3
	Private Const ACFieldICCCLA As Integer = 4
	Private Const ACFieldICCRevisedRes As Integer = 5
	Private Const ACFieldICCCLP As Integer = 6
	Private Const ACFieldICCPaidToDate As Integer = 7
	Private Const ACFieldICCPaymentTable As Integer = 8
	Private Const ACFieldICCTotalStats As Integer = 9 'only on listview
	Private Const ACFieldICCTotalClaim As Integer = 10 'only on listview
	
	'Reserve details
	Private Const ACFieldRReserveID As Integer = 0
	Private Const ACFieldRClaimPerilID As Integer = 1
	Private Const ACFieldRReserveTypeID As Integer = 2
	Private Const ACFieldRInitReserve As Integer = 3
	Private Const ACFieldRRevisedReserve As Integer = 4
	Private Const ACFieldRPaidToDate As Integer = 5
	Private Const ACFieldRThisRevision As Integer = 6
	Private Const ACFieldRThisPayment As Integer = 7
	Private Const ACFieldRReserveType As Integer = 8
	Private Const ACFieldRClaimPeril As Integer = 9
	
	'Payment details
	Private Const ACFieldPPaymentID As Integer = 0
	Private Const ACFieldPReserveID As Integer = 1
	Private Const ACFieldPClaimPerilID As Integer = 2
	Private Const ACFieldPAmount As Integer = 3
	Private Const ACFieldPPaymentDate As Integer = 4
	Private Const ACFieldPPartyCode As Integer = 5
	
	'risk details
	Private Const ACFieldRDInsuranceFileCnt As Integer = 0
	Private Const ACFieldRDRiskCnt As Integer = 1
	Private Const ACFieldRDStatusFlag As Integer = 2
	Private Const ACFieldRDDesc As Integer = 3
    Private Const ACFieldRDRiskStatus As Integer = 4
    Private Const ACFieldRDInsuranceFolderCnt As Integer = 5

    'transaction export
    Private Const ACFieldTETransactionExportFolderCnt As Integer = 0
	Private Const ACFieldTEInsuranceFileCnt As Integer = 1
	Private Const ACFieldTEInsuranceRef As Integer = 2
	Private Const ACFieldTEDocumentRef As Integer = 3
	Private Const ACFieldTEDocumentDate As Integer = 4
	Private Const ACFieldTEAccountExportStatus As Integer = 5
	
	'claim postings
	Private Const ACFieldCPPolicyNumber As Integer = 0
	Private Const ACFieldCPClaimNumber As Integer = 1
	Private Const ACFieldCPDocumentRef As Integer = 2
	Private Const ACFieldCPDocumentDate As Integer = 3
	Private Const ACFieldCPStatsDetailType As Integer = 4
	Private Const ACFieldCPThisPremiumHome As Integer = 5
	Private Const ACFieldCPPeriodID As Integer = 6
	
	'claim tab constant
	Private Const ACClaimTabFailedClaimTransaction As Integer = 0
	Private Const ACClaimTabImbalancedClosedClaim As Integer = 1
	Private Const ACClaimTabClaimPosting As Integer = 2
	Private Const ACClaimTabReservePaymentDetail As Integer = 3
    Private Const ACClaimTabMiscellaneous As Integer = 4

    ' claim list  Reverse and re generate
    Private Const ACFieldRevClaimid As Integer = 0
    Private Const ACFieldRevClaimNumber As Integer = 1
    Private Const ACFieldRevDocumentRef As Integer = 2
    Private Const ACFieldRevTransactionTypeCode As Integer = 3

    Private Const ACFieldRevClaimTransid As Integer = 0
    Private Const ACFieldRevClaimTransNumber As Integer = 1
    Private Const ACFieldClaimTransInsuranceref As Integer = 2
    Private Const ACFieldRevClaimTransDocumentRef As Integer = 3
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	Private m_oDocTemplate As Object
	
	Private m_lWidth As Integer
    Private m_lHeight As Integer
    Private bIsSucesfullyCompleted As Boolean

    'lvwSearchBordeau
    Private Const KBordereauId As Integer = 0
    Private Const KReceivedFromAccount As Integer = 1
    Private Const KReceivedFrom As Integer = 2
    Private Const KReceivedFromPartyCnt As Integer = 3
    Private Const KAccountCode As Integer = 4
    Private Const KAccount As Integer = 5
    Private Const KAgentPartyCnt As Integer = 6
    Private Const KBranchAmount As Integer = 7
    Private Const KBordereauTransactionTypeId As Integer = 8
    Private Const KBordereauTransactionId As Integer = 9
    Private Const KTransactionType As Integer = 10
    Private Const KBranchSourceId As Integer = 11
    Private Const KBordereauChannel As Integer = 12
    Private Const KBordereauChannelId As Integer = 13
    Private Const KBordereauStatus As Integer = 14
    Private Const KBordereauStatusId As Integer = 15
    Private Const KAmount As Integer = 16
    Private Const KBordereauReference As Integer = 17
    Private Const KDepositNumber As Integer = 18
    Private Const KCapturedDate As Integer = 19
    Private Const KBordereauCCurrencyId = 20
    Private m_bisFirstSelect As Boolean = False
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public Property StepStatus() As String
		Get
			
			Return m_sStepStatus
			
		End Get
		Set(ByVal Value As String)
			
			m_sStepStatus = Value
			
		End Set
	End Property
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	' ***********************************************************
	' Set the resizing anchors
	' ***********************************************************
	Private Sub SetResize()
		
		Try 
			
			' Set start dimensions
			m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
			m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
			
			'controls on form
			uctAnchor.Add(cmdOK, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(cmdExit, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(stbMain, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(tabMain, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'failed transaction tab
            'uctAnchor.Add(fraMain1, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            'uctAnchor.Add(fraMain2, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            'uctAnchor.Add(lvwSelectPolicy, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'********************single policy tab*************************
			'single policy tab
			uctAnchor.Add(tabPolicyVersion, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
            'uctAnchor.Add(fraPolicyVersion, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwPolicyVersion, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(fraRisk, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwRisk, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(fraTransactionExport, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwTransactionExport, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'******************claim tab***********************
			'claim sub tab
			uctAnchor.Add(tabClaim, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			'failed claim's transactions
			uctAnchor.Add(fraFailedClaimTransaction, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwClaim, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'imbalanced closed claims
			uctAnchor.Add(fraImbalancedClosedClaim, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwImbalancedClosedClaim, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'claim postings
			uctAnchor.Add(fraClaimPosting, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwClaimPosting, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'reserve/payment details
			uctAnchor.Add(fraReserve, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
			uctAnchor.Add(lvwReserve, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
			
			uctAnchor.Add(fraPayment, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwPayment, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			
			'miscelleneous
			uctAnchor.Add(fraClaimMisc, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
			uctAnchor.Add(lvwClaimMisc, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorBottom)
		
		Catch excep As System.Exception
			
			MessageBox.Show(excep.Message, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
			
			
		End Try
		
	End Sub
	
	Private Sub chkReserve_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _chkReserve_1.MouseUp, _chkReserve_0.MouseUp
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		Dim Index As Integer = Array.IndexOf(chkReserve, eventSender)
		
		If Index = 0 Then
			chkReserve(Index + 1).CheckState = IIf(chkReserve(Index).CheckState = CheckState.Unchecked, 1, 0)
		Else
			chkReserve(Index - 1).CheckState = IIf(chkReserve(Index).CheckState = CheckState.Unchecked, 1, 0)
		End If
		
	End Sub
	
	Private Sub cmdCPRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCPRefresh.Click
		PopulateClaimPosting()
	End Sub
	
	Private Sub cmdReservePaymentRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReservePaymentRefresh.Click
		PopulateReservePaymentDetail()
	End Sub
	
	Private Sub cmdRiskRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRiskRefresh.Click
		
		On Error GoTo Catch_Renamed
		
		If txtInsuranceFileCnt.Text.Trim() = "" Then
			MessageBox.Show("Please enter policy ID", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
			txtInsuranceFileCnt.Focus()
			GoTo Finally_Renamed
		End If
		
		PopulateRiskDetail()

        GoTo Finally_Renamed
Catch_Renamed: 
		
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get risk details for policy_id = " & txtInsuranceFileCnt.Text, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRiskRefresh_Click", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Exit Sub
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        iPMFunc.ShowFormInTaskBar_Detach()
        SSTabHelper.SetSelectedIndex(Me.tabMain, 0)
        SetResize()
        tabMain.TabPages.Remove(_tabMain_TabPage2)
        tabMain.TabPages.Remove(_tabMain_TabPage3)
        tabMain.TabPages.Remove(TabPage1)
        tabMain.TabPages.Remove(TabPage3)
        tabMain.TabPages.Remove(tabTasks)

        tabPolicyVersion.TabPages.Remove(TabPage2)
        tabPolicyVersion.TabPages.Remove(_tabPolicyVersion_TabPage1)
        tabPolicyVersion.TabPages.Remove(_tabPolicyVersion_TabPage2)
        tabPolicyVersion.TabPages.Remove(TabClaimTransaction)
        tabPolicyVersion.TabPages.Remove(TabDuplicateVersions)
        tabPolicyVersion.TabPages.Remove(TabClonePolicyVersion)

        ChkUpdatePerilandRaringData.Checked = True
        chkRIRefresh.Checked = True
        ChkUpdatePerilandRaringData.Enabled = False
        chkRIRefresh.Enabled = False
        _optSinglePolicy_4.Enabled = True
        _optSinglePolicy_3.Enabled = True
        'txtPolicyNumber.Enabled = False
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Enforce minimum sizes
        If Me.WindowState = FormWindowState.Normal Then
            If VB6.PixelsToTwipsX(Me.Width) < 5445 Then Me.Width = VB6.TwipsToPixelsX(5445)
            If VB6.PixelsToTwipsY(Me.Height) < 7000 Then Me.Height = VB6.TwipsToPixelsY(7000)

            If VB6.PixelsToTwipsX(txtClaimNumber.Width) < 135 Then txtClaimNumber.Width = VB6.TwipsToPixelsX(135)
            If VB6.PixelsToTwipsX(txtPolicyNumber.Width) < 135 Then txtPolicyNumber.Width = VB6.TwipsToPixelsX(135)
            If VB6.PixelsToTwipsX(cmdGetPolicyVersion.Width) < 135 Then cmdGetPolicyVersion.Width = VB6.TwipsToPixelsX(135)
            If VB6.PixelsToTwipsX(cmdReservePaymentRefresh.Width) < 135 Then cmdReservePaymentRefresh.Width = VB6.TwipsToPixelsX(135)
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            ' Resize the screen
            uctAnchor.Resize_Renamed(m_lWidth, m_lHeight, CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width)), CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height)))

            ' Store last sizes
            m_lWidth = CInt(VB6.PixelsToTwipsX(ClientRectangle.Width))
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
        End If

    End Sub

    Private Sub lvwClaim_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClaim.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwClaim.Columns(eventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwClaim
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwClaim, False)
                    ListViewHelper.SetSortOrderProperty(lvwClaim, (ListViewHelper.GetSortOrderProperty(lvwClaim) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:05:38 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaim))
                    ListViewFunc.ListViewSortByDate(lvwClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaim))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwClaim, False)
                    ListViewHelper.SetSortOrderProperty(lvwClaim, (ListViewHelper.GetSortOrderProperty(lvwClaim) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:06:14 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaim))
                    ListViewFunc.ListViewSortByValue(lvwClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaim))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwClaim)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwClaim, (ListViewHelper.GetSortOrderProperty(lvwClaim) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwClaim) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwClaim, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwClaim, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwClaim, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwClaim, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwClaim, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwClaim_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwClaim_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwClaim_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwClaim.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Button = MouseButtonConstants.RightButton Then
            ListViewPopUpMenu()
        End If

    End Sub

    Private Sub lvwClaimMisc_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwClaimMisc.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim vExtraMenu(0) As Object

        If Button = MouseButtonConstants.RightButton Then

            vExtraMenu(0) = "Copy Policy's Reinsurance Model To Claims Without Reinsurance"
            ListViewPopUpMenu(v_vExtraMenu:=vExtraMenu)
        End If

    End Sub

    Private Sub lvwClaimPosting_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClaimPosting.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwClaimPosting.Columns(eventArgs.Column)
        ' Column click event for the search details
        Try

            With lvwClaimPosting
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwClaimPosting, False)
                    ListViewHelper.SetSortOrderProperty(lvwClaimPosting, (ListViewHelper.GetSortOrderProperty(lvwClaimPosting) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:07:22 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwClaimPosting, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaimPosting))
                    ListViewFunc.ListViewSortByDate(lvwClaimPosting, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaimPosting))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwClaimPosting, False)
                    ListViewHelper.SetSortOrderProperty(lvwClaimPosting, (ListViewHelper.GetSortOrderProperty(lvwClaimPosting) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:09:04 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwClaimPosting, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaimPosting))
                    ListViewFunc.ListViewSortByDate(lvwClaimPosting, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwClaimPosting))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwClaimPosting)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwClaimPosting, (ListViewHelper.GetSortOrderProperty(lvwClaimPosting) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwClaimPosting) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwClaimPosting, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwClaimPosting, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwClaimPosting, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwClaimPosting, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwClaimPosting, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwClaimPosting_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwClaimPosting_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwClaimPosting_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwClaimPosting.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim vExtraMenu(1) As Object

        If Button = MouseButtonConstants.RightButton Then

            vExtraMenu(0) = "Add Posting"

            vExtraMenu(1) = "Change Document Date And Period ID"
            ListViewPopUpMenu(v_vExtraMenu:=vExtraMenu)
        End If

    End Sub

    Private Sub lvwImbalancedClosedClaim_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwImbalancedClosedClaim.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwImbalancedClosedClaim.Columns(eventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwImbalancedClosedClaim
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwImbalancedClosedClaim, False)
                    ListViewHelper.SetSortOrderProperty(lvwImbalancedClosedClaim, (ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:09:35 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwImbalancedClosedClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim))
                    ListViewFunc.ListViewSortByDate(lvwImbalancedClosedClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwImbalancedClosedClaim, False)
                    ListViewHelper.SetSortOrderProperty(lvwImbalancedClosedClaim, (ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:09:51 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwImbalancedClosedClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim))
                    ListViewFunc.ListViewSortByValue(lvwImbalancedClosedClaim, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwImbalancedClosedClaim)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwImbalancedClosedClaim, (ListViewHelper.GetSortOrderProperty(lvwImbalancedClosedClaim) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwImbalancedClosedClaim) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwImbalancedClosedClaim, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwImbalancedClosedClaim, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwImbalancedClosedClaim, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwImbalancedClosedClaim, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwImbalancedClosedClaim, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwImbalancedClosedClaim_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwImbalancedClosedClaim_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwImbalancedClosedClaim_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwImbalancedClosedClaim.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = MouseButtonConstants.RightButton Then
            ListViewPopUpMenu()
        End If
    End Sub

    Private Sub lvwPayment_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPayment.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwPayment.Columns(eventArgs.Column)
        ' Column click event for the search details
        Try

            With lvwPayment
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPayment, False)
                    ListViewHelper.SetSortOrderProperty(lvwPayment, (ListViewHelper.GetSortOrderProperty(lvwPayment) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:10:05 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwPayment, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPayment))
                    ListViewFunc.ListViewSortByDate(lvwPayment, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPayment))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPayment, False)
                    ListViewHelper.SetSortOrderProperty(lvwPayment, (ListViewHelper.GetSortOrderProperty(lvwPayment) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:10:20 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwPayment, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPayment))
                    ListViewFunc.ListViewSortByValue(lvwPayment, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPayment))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPayment)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPayment, (ListViewHelper.GetSortOrderProperty(lvwPayment) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwPayment) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwPayment, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPayment, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPayment, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPayment, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPayment, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwPayment_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPayment_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwPolicyVersion_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwPolicyVersion.Columns(eventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwPolicyVersion
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:10:35 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))
                    ListViewFunc.ListViewSortByDate(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:10:47 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))
                    ListViewFunc.ListViewSortByValue(lvwPolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwPolicyVersion))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicyVersion)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwPolicyVersion) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwPolicyVersion) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwPolicyVersion, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPolicyVersion, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPolicyVersion, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPolicyVersion, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwPolicyVersion_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPolicyVersion_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwPolicyVersion_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        If Button = MouseButtonConstants.RightButton Then
            ListViewPopUpMenu()
        End If
    End Sub

    Private Sub lvwReserve_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwReserve.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwReserve.Columns(eventArgs.Column)
        ' Column click event for the search details
        Try

            With lvwReserve
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwReserve, False)
                    ListViewHelper.SetSortOrderProperty(lvwReserve, (ListViewHelper.GetSortOrderProperty(lvwReserve) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:11:00 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwReserve, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwReserve))
                    ListViewFunc.ListViewSortByDate(lvwReserve, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwReserve))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwReserve, False)
                    ListViewHelper.SetSortOrderProperty(lvwReserve, (ListViewHelper.GetSortOrderProperty(lvwReserve) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:11:23 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwReserve, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwReserve))
                    ListViewFunc.ListViewSortByValue(lvwReserve, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwReserve))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwReserve)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwReserve, (ListViewHelper.GetSortOrderProperty(lvwReserve) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwReserve) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwReserve, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwReserve, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwReserve, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwReserve, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwReserve, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwReserve_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwReserve_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    Private Sub lvwRisk_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRisk.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRisk.Columns(eventArgs.Column)
        ' Column click event for the search details
        Try

            With lvwRisk
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwRisk, False)
                    ListViewHelper.SetSortOrderProperty(lvwRisk, (ListViewHelper.GetSortOrderProperty(lvwRisk) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:11:36 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwRisk, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwRisk))
                    ListViewFunc.ListViewSortByDate(lvwRisk, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwRisk))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwRisk, False)
                    ListViewHelper.SetSortOrderProperty(lvwRisk, (ListViewHelper.GetSortOrderProperty(lvwRisk) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:12:05 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwRisk, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwRisk))
                    ListViewFunc.ListViewSortByValue(lvwRisk, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwRisk))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwRisk)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwRisk, (ListViewHelper.GetSortOrderProperty(lvwRisk) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwRisk) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwRisk, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwRisk, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwRisk, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwRisk, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwRisk, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwRisk_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRisk_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)


        End Try

    End Sub

    'Private Sub lvwSelectPolicy_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs)
    '    Dim Item As ListViewItem = lvwSelectPolicy.Items(eventArgs.Index)
    '    If Item.Checked Then
    '        Select Case ListViewHelper.GetListViewSubItem(Item, ACFieldDocumentRef).Text.Substring(0, 3)
    '            Case "SDD", "SEC", "SED", "SIC", "SID", "SNC", "SND", "SPD", "SRC", "SRD"
    '            Case Else
    '                MessageBox.Show("Can't repost this type of document." & Strings.Chr(13) & Strings.Chr(10) & "Can only repost policy level document.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                Item.Checked = False
    '        End Select

    '    End If
    'End Sub

    Private Sub optReservePayment_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles _optReservePayment_1.MouseUp, _optReservePayment_0.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim Index As Integer = Array.IndexOf(optReservePayment, eventSender)

        chkReserve(0).Enabled = (Index = 0)
        chkReserve(1).Enabled = (Index = 0)

    End Sub

    Private Sub optSinglePolicy_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optSinglePolicy_2.CheckedChanged, _optSinglePolicy_0.CheckedChanged, _optSinglePolicy_1.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optSinglePolicy, eventSender)

            Select Case Index
                Case 0 'repost
                    ToolTip1.SetToolTip(cmdOK, "Repost selected versions of this policy")
                Case 1 'delete
                    ToolTip1.SetToolTip(cmdOK, "Delete selected versions of this policy")
                Case 2 'change status
                    ToolTip1.SetToolTip(cmdOK, "Change status for selected versions of this policy")
            End Select

            cboPolicyStatus.Enabled = (Index = 2)
            lblPolicyStatus.Enabled = (Index = 2)

        End If
    End Sub

    Private Sub tabClaim_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabClaim.SelectedIndexChanged

        cmdOK.Enabled = (SSTabHelper.GetSelectedIndex(tabClaim) <> ACClaimTabReservePaymentDetail And SSTabHelper.GetSelectedIndex(tabClaim) <> ACClaimTabClaimPosting)
        CmdSelectAllPolicy.Enabled = (SSTabHelper.GetSelectedIndex(tabClaim) <> ACClaimTabReservePaymentDetail And SSTabHelper.GetSelectedIndex(tabClaim) <> ACClaimTabClaimPosting)
        Select Case SSTabHelper.GetSelectedIndex(tabClaim)
            Case ACClaimTabFailedClaimTransaction 'failed claim's transactions
                stbMain.Items.Item("COUNT").Text = CStr(lvwClaim.Items.Count)
                ToolTip1.SetToolTip(cmdOK, "Repost selected failed claims")
            Case ACClaimTabImbalancedClosedClaim 'imbalance closed claims
                stbMain.Items.Item("COUNT").Text = CStr(lvwImbalancedClosedClaim.Items.Count)
                ToolTip1.SetToolTip(cmdOK, "Repost selected imbalanced closed claims")
            Case ACClaimTabClaimPosting
                stbMain.Items.Item("COUNT").Text = CStr(lvwClaimPosting.Items.Count)
            Case ACClaimTabReservePaymentDetail 'reserve/payment details

                'TODO : TO be checked later
                'Select Case tabClaimPreviousTab
                '    Case ACClaimTabFailedClaimTransaction
                '        If lvwClaim.Items.Count > 0 Then
                '            txtClaimNumber.Text = lvwClaim.Items.Item(lvwClaim.FocusedItem.Index).Text
                '        End If
                '    Case ACClaimTabImbalancedClosedClaim
                '        If lvwImbalancedClosedClaim.Items.Count > 0 Then
                '            txtClaimNumber.Text = lvwImbalancedClosedClaim.Items.Item(lvwImbalancedClosedClaim.FocusedItem.Index).SubItems.Item(ACFieldICCClaimNumber - 1).Text
                '        End If
                '    Case ACClaimTabClaimPosting
                '        If txtCPClaimNumber.Text.Trim() <> "" Then
                '            txtClaimNumber.Text = txtCPClaimNumber.Text.Trim()
                '        End If
                'End Select

                If txtClaimNumber.Text.Trim() <> "" Then
                    PopulateReservePaymentDetail()
                End If
        End Select
        'TODO : TO be checked later
        'tabClaimPreviousTab = tabClaim.SelectedIndex
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMain.SelectedIndexChanged

        Dim sPolicyRef As String = ""
        Dim vResultArray(,) As Object

        cmdOK.Enabled = True
        CmdSelectAllPolicy.Enabled = True
        Select Case SSTabHelper.GetSelectedIndex(tabMain)
            Case 0 'Failed Transaction
                Label6.Visible = True
                txtPMNumber.Visible = True
                'stbMain.Items.Item("COUNT").Text = CStr(lvwSelectPolicy.Items.Count)
                'ToolTip1.SetToolTip(cmdOK, "Repost selected Failed transactions")
            Case 1 'Single Policy
                'If lvwSelectPolicy.Items.Count > 0 Then
                '    sPolicyRef = ListViewHelper.GetListViewSubItem(lvwSelectPolicy.Items.Item(lvwSelectPolicy.FocusedItem.Index), ACFieldInsuranceRef).Text.Trim()

                '    If sPolicyRef <> txtPolicyNumber.Text.Trim() Then
                '        txtPolicyNumber.Text = sPolicyRef
                '        cmdGetPolicyVersion_Click(cmdGetPolicyVersion, New EventArgs())
                '    End If
                'End If

                'If cboPolicyStatus.Items.Count < 1 Then

                '    If m_oBusiness.PopulatePolicyStatus(r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                '        MessageBox.Show("Failed to populate policy status", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    Else
                '        If Information.IsArray(vResultArray) Then
                '            Dim cboPolicyStatus_NewIndex As Integer = -1
                '            cboPolicyStatus_NewIndex = cboPolicyStatus.Items.Add("Live")
                '            VB6.SetItemData(cboPolicyStatus, cboPolicyStatus_NewIndex, 0)


                '            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                '                cboPolicyStatus_NewIndex = cboPolicyStatus.Items.Add(CStr(vResultArray(1, lCount)))

                '                VB6.SetItemData(cboPolicyStatus, cboPolicyStatus_NewIndex, CInt(vResultArray(0, lCount)))
                '            Next lCount

                '            cboPolicyStatus.SelectedIndex = 0
                '        Else
                '            MessageBox.Show("No policy status are found", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                '        End If
                '    End If

                'End If

                SSTabHelper.SetSelectedIndex(tabPolicyVersion, 0)
                Me.txtPolicyNumber.Focus()
                stbMain.Items.Item("COUNT").Text = CStr(lvwPolicyVersion.Items.Count)
                ToolTip1.SetToolTip(cmdOK, "Repost selected versions of this policy")
                Label6.Visible = True
                txtPMNumber.Visible = True
            Case 2 'Claim
                SSTabHelper.SetSelectedIndex(tabClaim, ACClaimTabFailedClaimTransaction)
                ToolTip1.SetToolTip(cmdOK, "Repost selected failed claims")
                Label6.Visible = False
                txtPMNumber.Visible = False
            Case 3 'Miscellaneous
                stbMain.Items.Item("COUNT").Text = ""
                Me.optMiscellaneous(0).Checked = True
                Label6.Visible = True
                txtPMNumber.Visible = True

        End Select
        'TODO : TO be checked later
        'tabMainPreviousTab = tabMain.SelectedIndex
    End Sub

    ''' <summary>
    ''' cmdGetPolicyVersion_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdGetPolicyVersion_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGetPolicyVersion.Click


        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.cmdGetPolicyVersion.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all versions of this policy"

            If txtPolicyNumber.Text <> "" Then
                If (Me.ChkRepost_RepostedTrans.CheckState = CheckState.Unchecked) Then
                    sSql = "SELECT i.insurance_file_cnt,i.insurance_ref,i.insurance_file_type_id,ift.description,d.document_ref,i.cover_start_date,document_date FROM insurance_file i "
                    sSql = sSql & " INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id=i.insurance_file_type_id "
                    sSql = sSql & "LEFT JOIN document d ON d.insurance_file_cnt = i.insurance_file_cnt WHERE document_ref LIKE 'S%' AND  (I.insurance_file_cnt NOT IN (select  insurance_file_cnt from   datafixutility_log) ) AND "
                    sSql = sSql & " i.insurance_ref= " & "'" & txtPolicyNumber.Text & "' ORDER  BY i.insurance_ref,i.insurance_file_cnt"
                Else
                    sSql = "SELECT i.insurance_file_cnt,i.insurance_ref,i.insurance_file_type_id,ift.description,d.document_ref,i.cover_start_date,document_date,d.document_id FROM insurance_file i "
                    sSql = sSql & " INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id=i.insurance_file_type_id "
                    sSql = sSql & "LEFT JOIN document d ON d.insurance_file_cnt = i.insurance_file_cnt "
                    sSql = sSql & "LEFT JOIN datafixutility_log dtl on dtl.new_document_id= d.document_id WHERE document_ref LIKE 'S%' AND  dtl.comment like '%Reposting of%' AND "
                    sSql = sSql & " i.insurance_ref= " & "'" & txtPolicyNumber.Text & "' ORDER  BY  i.insurance_ref,i.insurance_file_cnt asc ,d.document_id  desc"
                End If
            Else
                    sSql = txtSqlQuery.Text
            End If



            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                lvwPolicyVersion.Items.Clear()
                Exit Sub
            End If

            lvwPolicyVersion.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'insurance file cnt
                oListItem = Me.lvwPolicyVersion.Items.Add(CStr(aoResultArray(ACFieldPVInsuranceFileCnt, lCount)))

                'insurance ref               
                oListItem.SubItems.Insert(ACFieldPVInsuranceRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVInsuranceRef, lCount).Trim()))

                'policy type id                
                oListItem.SubItems.Insert(ACFieldPVPolicyTypeID, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()))

                'policy type                
                oListItem.SubItems.Insert(ACFieldPVPolicyType, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyType, lCount).Trim()))

                oListItem.SubItems(ACFieldPVPolicyType).Tag = aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()

                'Document Ref               
                oListItem.SubItems.Insert(ACFieldPVDocumentRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentRef, lCount).Trim()))

                'policy start                
                oListItem.SubItems.Insert(ACFieldPVPolicyStart, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyStart, lCount).Trim()))

                'document Date               
                oListItem.SubItems.Insert(ACFieldPVDocumentDate, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentDate, lCount).Trim()))


            Next

            If lvwPolicyVersion.Items.Count > 0 Then
                lvwPolicyVersion.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)
            CmdSelectAllPolicy.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed cmdGetPolicyVersion_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="cmdGetPolicyVersion_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.cmdGetPolicyVersion.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()


        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of the business object via
            'the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRepostTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display message.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSirRepostTransaction.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If Not (m_oBusiness Is Nothing) Then

                m_lReturn = m_oBusiness.Terminate
                m_oBusiness = Nothing
            End If

            If Not (m_oDocTemplate Is Nothing) Then

                m_lReturn = m_oDocTemplate.Terminate
                m_oDocTemplate = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    '*********************************************************************************************
    ' we only recreate stats details and transaction exports if required.
    ' we do not recreate stats folder because we still want the old document ref
    ' if we need to recreate stats folder in the future then this can be added easily
    '*********************************************************************************************
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Me.cmdOK.Enabled = False
        Me.cmdExit.Enabled = False
        CmdSelectAllPolicy.Enabled = False
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Select Case SSTabHelper.GetSelectedIndex(Me.tabMain)
            'Case 0 'Multi transaction
            'ProcessFailedTransaction()
            'stbMain.Items.Item("COUNT").Text = CStr(Me.lvwSelectPolicy.Items.Count)
            Case 0 'Single policy
                If (txtPMNumber.Text.Trim() = "") Then
                    MsgBox("Please Enter Valid IM/PM number ", vbInformation)
                    Me.cmdOK.Enabled = True
                    Me.cmdExit.Enabled = True
                    CmdSelectAllPolicy.Enabled = True
                    txtPMNumber.Focus()
                    Exit Sub
                End If

                If MsgBox("Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                    Me.cmdOK.Enabled = True
                    Me.cmdExit.Enabled = True
                    CmdSelectAllPolicy.Enabled = True
                    Exit Sub
                End If
                bIsSucesfullyCompleted = True
                ProcessSinglePolicy()
                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)
            Case 1 'Claim
                bIsSucesfullyCompleted = True
                Select Case SSTabHelper.GetSelectedIndex(tabClaim)
                    Case ACClaimTabFailedClaimTransaction 'failed claim's transactions
                        ProcessFailedClaimTransaction()
                    Case ACClaimTabImbalancedClosedClaim 'imbalanced claims
                        RePostClaim()
                End Select
            Case 2 'Miscellaneous
                bIsSucesfullyCompleted = True
                Miscellaneous()
        End Select
        If bIsSucesfullyCompleted Then
            MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
        End If

        Me.cmdOK.Enabled = True
        Me.cmdExit.Enabled = True
        CmdSelectAllPolicy.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Call cmdGetPolicyVersion_Click(Nothing, Nothing)

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_Click", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    '    Private Function PopulateFailedPolicy() As Integer

    '        Dim result As Integer = 0
    '        Dim oListItem As ListViewItem
    '        Dim lMax As Integer
    '        Dim vFailedPolicy(,) As Object
    '        Dim sPostingStatus As String = ""
    '        Dim lExcludeOtherDoc As DialogResult

    '        On Error GoTo Catch_Renamed

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '        lvwSelectPolicy.Items.Clear()

    '        lExcludeOtherDoc = MessageBox.Show("Exclude other type of document?" & Strings.Chr(13) & Strings.Chr(10) & "(Only select policy level documents eg SND, SRD etc)", ACApp, MessageBoxButtons.YesNo)

    '        stbMain.Items.Item("MESSAGE").Text = "Getting failed transactions please wait"

    '        'get transaction which need reposting (status <> 'C')

    '        m_lReturn = m_oBusiness.GetFailedTransaction(r_vResult:=vFailedPolicy, v_lExcludeOtherDoc:=IIf(lExcludeOtherDoc = System.Windows.Forms.DialogResult.Yes, 1, 0))

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get transactions which need reposting", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFailedPolicy", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '            result = gPMConstants.PMEReturnCode.PMFalse
    '            GoTo Finally_Renamed
    '        End If


    '        If Not Information.IsArray(vFailedPolicy) Then
    '            GoTo Finally_Renamed
    '        End If


    '        lMax = vFailedPolicy.GetUpperBound(1)

    '        stbMain.Items.Item("MESSAGE").Text = "Displaying failed transactions please wait"

    '        For lCount As Integer = 0 To lMax
    '            'transaction export folder cnt

    '            oListItem = Me.lvwSelectPolicy.Items.Add(CStr(vFailedPolicy(ACFieldExportFolderCnt, lCount)).Trim())

    '            'insurance file cnt


    '            'Modified by Sumeet Singh on 5/25/2010 12:49:13 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldInsuranceFileCnt,  , vFailedPolicy(ACFieldInsuranceFileCnt, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldInsuranceFileCnt, vFailedPolicy(ACFieldInsuranceFileCnt, lCount).Trim())

    '            'document ref


    '            'Modified by Sumeet Singh on 5/25/2010 12:49:51 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldDocumentRef,  , vFailedPolicy(ACFieldDocumentRef, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldDocumentRef, vFailedPolicy(ACFieldDocumentRef, lCount).Trim())

    '            'insurance ref


    '            'Modified by Sumeet Singh on 5/25/2010 12:50:08 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldInsuranceRef,  , vFailedPolicy(ACFieldInsuranceRef, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldInsuranceRef, vFailedPolicy(ACFieldInsuranceRef, lCount).Trim())

    '            'cover start date


    '            'Modified by Sumeet Singh on 5/25/2010 12:50:25 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldPolicyStart,  , vFailedPolicy(ACFieldPolicyStart, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldPolicyStart, vFailedPolicy(ACFieldPolicyStart, lCount).Trim())

    '            'cover end date


    '            'Modified by Sumeet Singh on 5/25/2010 12:50:38 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldPolicyEnd,  , vFailedPolicy(ACFieldPolicyEnd, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldPolicyEnd, vFailedPolicy(ACFieldPolicyEnd, lCount).Trim())

    '            'client short_name


    '            'Modified by Sumeet Singh on 5/25/2010 12:50:56 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldClient,  , vFailedPolicy(ACFieldClient, lCount).Trim())
    '            oListItem.SubItems.Insert(ACFieldClient, vFailedPolicy(ACFieldClient, lCount).Trim())

    '            'posting status


    '            Select Case CStr(vFailedPolicy(ACFieldPostingStatus, lCount)).Trim().ToUpper()
    '                Case "P"
    '                    sPostingStatus = "Pending (p)"
    '                Case "S"
    '                    sPostingStatus = "Sending (s)"
    '                Case "F"
    '                    sPostingStatus = "Failed (f)"
    '                Case "N"
    '                    sPostingStatus = "No Exports (n)"
    '            End Select


    '            'Modified by Sumeet Singh on 5/25/2010 12:51:12 PM refer developer guide no. 215
    '            'oListItem.SubItems.Add(ACFieldPostingStatus,  , sPostingStatus)
    '            oListItem.SubItems.Insert(ACFieldPostingStatus, CObj(sPostingStatus))
    '        Next

    '        GoTo Finally_Renamed

    'Catch_Renamed:

    '        result = gPMConstants.PMEReturnCode.PMFalse

    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed PopulateFailedPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFailedPolicy", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    'Finally_Renamed:
    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
    '        stbMain.Items.Item("MESSAGE").Text = "Ready"

    '        If lvwSelectPolicy.Items.Count > 0 Then
    '            stbMain.Items.Item("COUNT").Text = CStr(lvwSelectPolicy.Items.Count)
    '        End If

    '        Return result
    '    End Function

    Private Sub CreateTransactionExport(ByVal v_lStatsFolderCnt As Integer, ByRef r_lTransactionExportFolderCnt As Integer, ByVal v_sDocumentRef As String)

        On Error GoTo Err_CreateTransactionExport

        stbMain.Items.Item("MESSAGE").Text = "Creating transaction export folder"

        m_lReturn = m_oBusiness.CreateExportFolder(v_lStatsFolderCnt:=v_lStatsFolderCnt, r_lExportFolderCnt:=r_lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create transaction export folder", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionExport", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            GoTo End_CreateTransactionExport

        End If

        stbMain.Items.Item("MESSAGE").Text = "Creating transaction export detail"

        m_lReturn = m_oBusiness.CreateTransExportReverse(v_lStatsFolderCnt:=v_lStatsFolderCnt, v_lTransactionExportFolderCnt:=r_lTransactionExportFolderCnt, v_sDocumentRef:=v_sDocumentRef)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create transaction export details", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionExport", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            GoTo End_CreateTransactionExport

        End If

        GoTo End_CreateTransactionExport

Err_CreateTransactionExport:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed CreateTransactionExport", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionExport", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        GoTo End_CreateTransactionExport

End_CreateTransactionExport:

        stbMain.Items.Item("MESSAGE").Text = "Ready"

    End Sub

    'Private Sub lvwSelectPolicy_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
    '    Dim ColumnHeader As ColumnHeader = lvwSelectPolicy.Columns(eventArgs.Column)

    '    ' Column click event for the search details
    '    Try

    '        With lvwSelectPolicy
    '            'Identify the Date type columns
    '            If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
    '                ListViewHelper.SetSortedProperty(lvwSelectPolicy, False)
    '                ListViewHelper.SetSortOrderProperty(lvwSelectPolicy, (ListViewHelper.GetSortOrderProperty(lvwSelectPolicy) + 1) Mod 2)
    '                'Special Sort function for Dates
    '                'Modified by Sumeet Singh on 5/25/2010 11:24:59 AM refer developer guide no. 178
    '                'ListViewSortByDate(lvwSelectPolicy, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSelectPolicy))
    '                ListViewFunc.ListViewSortByDate(lvwSelectPolicy, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSelectPolicy))

    '                'Identify the Value type columns (to sort numerics correctly)
    '            ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
    '                ListViewHelper.SetSortedProperty(lvwSelectPolicy, False)
    '                ListViewHelper.SetSortOrderProperty(lvwSelectPolicy, (ListViewHelper.GetSortOrderProperty(lvwSelectPolicy) + 1) Mod 2)
    '                'Use the special sort function for numerics
    '                'Modified by Sumeet Singh on 5/25/2010 11:25:12 AM refer developer guide no. 178
    '                'ListViewSortByValue(lvwSelectPolicy, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSelectPolicy))
    '                ListViewFunc.ListViewSortByValue(lvwSelectPolicy, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSelectPolicy))

    '                'See if this the column already sorted on
    '            ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSelectPolicy)) Then
    '                ' Set sort order opposite of current direction.
    '                ListViewHelper.SetSortOrderProperty(lvwSelectPolicy, (ListViewHelper.GetSortOrderProperty(lvwSelectPolicy) + 1) Mod 2)
    '                'If this is the very first time that a header is clicked then
    '                'if clicking on the first column we need to refresh. Otherwise we don't
    '                If Not ListViewHelper.GetSortedProperty(lvwSelectPolicy) Then
    '                    'Do the refresh
    '                    ListViewHelper.SetSortedProperty(lvwSelectPolicy, True)
    '                End If

    '            Else
    '                ' Sort by this column (ascending).
    '                ListViewHelper.SetSortedProperty(lvwSelectPolicy, False)
    '                ' Turn off sorting so that the list is not sorted twice
    '                ListViewHelper.SetSortOrderProperty(lvwSelectPolicy, SortOrder.Ascending)
    '                ListViewHelper.SetSortKeyProperty(lvwSelectPolicy, ColumnHeader.Index + 1 - 1)
    '                ListViewHelper.SetSortedProperty(lvwSelectPolicy, True)
    '            End If

    '        End With

    '    Catch excep As System.Exception



    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwSelectPolicy_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSelectPolicy_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

    '    End Try

    'End Sub

    Private Sub lvwSelectPolicy_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)


        If Button = MouseButtonConstants.RightButton Then
            ListViewPopUpMenu()
        End If

    End Sub

    Public Sub mnuPopUpItem_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(mnuPopUpItem, eventSender)

        Select Case SSTabHelper.GetSelectedIndex(Me.tabMain)
            Case 0 'Multi transaction
                'Select Case mnuPopUpItem(Index).Text.ToUpper()
                '    Case "SEARCH LIST"
                '        SearchListView(v_oListView:=lvwSelectPolicy)
                '    Case "REFRESH LIST"
                '        m_lReturn = PopulateFailedPolicy()
                '    Case "UNSELECT ALL"
                '        TickListView(r_oListView:=lvwSelectPolicy, v_lAll:=1, v_bValue:=False)
                '    Case "SELECT ALL"
                '        TickListView(r_oListView:=lvwSelectPolicy, v_lAll:=1)
                '    Case "SELECT HIGHLIGHTED"
                '        TickListView(r_oListView:=lvwSelectPolicy, v_lAll:=0)
                '    Case "DELETE SELECTED"
                '        DeleteSelected(r_oListView:=lvwSelectPolicy)
                '    Case "COPY LISTVIEW COLUMN'S VALUE"
                '        CopyListViewColumnValue(lvwSelectPolicy)
                'End Select
            Case 1 'Single Policy
                Select Case mnuPopUpItem(Index).Text.ToUpper()
                    Case "SEARCH LIST"
                        SearchListView(v_oListView:=lvwPolicyVersion)
                    Case "REFRESH LIST"
                        cmdGetPolicyVersion_Click(cmdGetPolicyVersion, New EventArgs())
                    Case "UNSELECT ALL"
                        TickListView(r_oListView:=lvwPolicyVersion, v_lAll:=1, v_bValue:=False)
                    Case "SELECT ALL"
                        TickListView(r_oListView:=lvwPolicyVersion, v_lAll:=1)
                    Case "SELECT HIGHLIGHTED"
                        TickListView(r_oListView:=lvwPolicyVersion, v_lAll:=0)
                    Case "DELETE SELECTED"
                        DeleteSelected(r_oListView:=lvwPolicyVersion)
                    Case "COPY LISTVIEW COLUMN'S VALUE"
                        CopyListViewColumnValue(lvwPolicyVersion)
                End Select
            Case 2 'Claim
                Select Case SSTabHelper.GetSelectedIndex(tabClaim)
                    Case ACClaimTabFailedClaimTransaction 'failed claim's transactions
                        Select Case mnuPopUpItem(Index).Text.ToUpper()
                            Case "SEARCH LIST"
                                SearchListView(v_oListView:=lvwClaim)
                            Case "REFRESH LIST"
                                PopulateFailedClaimTransaction()
                            Case "UNSELECT ALL"
                                TickListView(r_oListView:=lvwClaim, v_lAll:=1, v_bValue:=False)
                            Case "SELECT ALL"
                                TickListView(r_oListView:=lvwClaim, v_lAll:=1)
                            Case "SELECT HIGHLIGHTED"
                                TickListView(r_oListView:=lvwClaim, v_lAll:=0)
                            Case "DELETE SELECTED"
                                DeleteSelected(r_oListView:=lvwClaim)
                            Case "COPY LISTVIEW COLUMN'S VALUE"
                                CopyListViewColumnValue(lvwClaim)
                        End Select
                    Case ACClaimTabImbalancedClosedClaim 'imbalanced closed claim
                        Select Case mnuPopUpItem(Index).Text.ToUpper()
                            Case "SEARCH LIST"
                                SearchListView(v_oListView:=lvwImbalancedClosedClaim)
                            Case "REFRESH LIST"
                                PopulateImbalanceClosedClaim()
                            Case "UNSELECT ALL"
                                TickListView(r_oListView:=lvwImbalancedClosedClaim, v_lAll:=1, v_bValue:=False)
                            Case "SELECT ALL"
                                TickListView(r_oListView:=lvwImbalancedClosedClaim, v_lAll:=1)
                            Case "SELECT HIGHLIGHTED"
                                TickListView(r_oListView:=lvwImbalancedClosedClaim, v_lAll:=0)
                            Case "DELETE SELECTED"
                                DeleteSelected(r_oListView:=lvwImbalancedClosedClaim)
                            Case "COPY LISTVIEW COLUMN'S VALUE"
                                CopyListViewColumnValue(lvwImbalancedClosedClaim)
                        End Select
                    Case ACClaimTabClaimPosting 'Claim postings
                        Select Case mnuPopUpItem(Index).Text.ToUpper()
                            Case "SEARCH LIST"
                                SearchListView(v_oListView:=lvwClaimPosting)
                            Case "REFRESH LIST"
                                PopulateClaimPosting()
                            Case "UNSELECT ALL"
                                TickListView(r_oListView:=lvwClaimPosting, v_lAll:=1, v_bValue:=False)
                            Case "SELECT ALL"
                                TickListView(r_oListView:=lvwClaimPosting, v_lAll:=1)
                            Case "SELECT HIGHLIGHTED"
                                TickListView(r_oListView:=lvwClaimPosting, v_lAll:=0)
                            Case "DELETE SELECTED"
                                DeleteSelected(r_oListView:=lvwClaimPosting)
                            Case "COPY LISTVIEW COLUMN'S VALUE"
                                CopyListViewColumnValue(lvwClaimPosting)
                            Case "ADD POSTING"
                                AddClaimPosting()
                            Case "CHANGE DOCUMENT DATE AND PERIOD ID"
                                ChangeDateAndPeriodID()
                            Case "DELETE DOCUMENT FROM ACCOUNT"
                                DeletePostingDocument(lvwClaimPosting, 3)
                        End Select
                    Case ACClaimTabMiscellaneous 'misellaneous tab
                        Select Case mnuPopUpItem(Index).Text.ToUpper()
                            Case "SEARCH LIST"
                                SearchListView(v_oListView:=lvwClaimMisc)
                            Case "REFRESH LIST"
                                PopulateClaimMisc()
                            Case "UNSELECT ALL"
                                TickListView(r_oListView:=lvwClaimMisc, v_lAll:=1, v_bValue:=False)
                            Case "SELECT ALL"
                                TickListView(r_oListView:=lvwClaimMisc, v_lAll:=1)
                            Case "SELECT HIGHLIGHTED"
                                TickListView(r_oListView:=lvwClaimMisc, v_lAll:=0)
                            Case "DELETE SELECTED"
                                DeleteSelected(r_oListView:=lvwClaimMisc)
                            Case "COPY LISTVIEW COLUMN'S VALUE"
                                CopyListViewColumnValue(lvwClaimMisc)
                            Case "COPY POLICY'S REINSURANCE MODEL TO CLAIMS WITHOUT REINSURANCE"
                                CopyRIAndRepostClaim()
                        End Select

                End Select
        End Select

    End Sub

    Private Sub SearchListView(ByVal v_oListView As ListView)

        Dim oSearch As iPMListViewSearch6.Interface_Renamed
        Dim lReturn As gPMConstants.PMEReturnCode

        On Error GoTo Err_SearchListView

        oSearch = New iPMListViewSearch6.Interface_Renamed()

        If Not Information.IsReference(oSearch) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create search object", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchListView", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            Exit Sub
        End If

        lReturn = oSearch.LvwSearch(oSearchList:=v_oListView)

        Exit Sub

Err_SearchListView:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed SearchListView", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchListView", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        Exit Sub




    End Sub

    '    '*************************************************************************************
    '    ' recreate stats detail and transaction export folder/detail if required and repost to account
    '    '*************************************************************************************
    '    Private Sub ProcessFailedTransaction()


    '        Dim lStatsFolderCnt, lInsuranceFileCnt, lTransactionExportFolderCnt, lCount As Integer
    '        Dim vDeletedIndex As Object
    '        Dim lInAccount As gPMConstants.PMEReturnCode
    '        Dim sDocumentRef As String = ""

    '        On Error GoTo Err_ProcessFailedTransaction

    '        ' Set the interface status.
    '        m_lStatus = gPMConstants.PMEReturnCode.PMOK


    '        For Each oListItem As ListViewItem In Me.lvwSelectPolicy.Items
    '            If oListItem.Checked Then

    '                sDocumentRef = oListItem.SubItems.Item(ACFieldDocumentRef - 1).Text.Trim()
    '                lInsuranceFileCnt = CInt(oListItem.SubItems.Item(ACFieldInsuranceFileCnt - 1).Text)

    '                'check to see if this document_ref is in account

    '                m_lReturn = m_oBusiness.IsDocumentInAccount(v_sDocumentRef:=sDocumentRef, r_lStatus:=lInAccount)

    '                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if document is in account", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                    GoTo End_ProcessFailedTransaction

    '                End If

    '                If lInAccount = gPMConstants.PMEReturnCode.PMTrue Then
    '                    MessageBox.Show("Document : " & sDocumentRef & " Is Already In Account." & Strings.Chr(13) & Strings.Chr(10) & " Can't Repost", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                    GoTo End_ProcessFailedTransaction
    '                End If

    '                lCount += 1

    '                stbMain.Items.Item("COUNT").Text = CStr(lCount)

    '                stbMain.Items.Item("MESSAGE").Text = "Getting stats folder count for this policy"

    '                m_lReturn = m_oBusiness.GetStatsFolderCnt(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sDocumentRef:=sDocumentRef, r_lStatsFolderCnt:=lStatsFolderCnt)

    '                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get stats folder count", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                    GoTo End_ProcessFailedTransaction

    '                End If

    '                'recreate stats details? (this will also recreate transaction exports)
    '                If Me.chkRecreateStats.CheckState = CheckState.Checked Then

    '                    stbMain.Items.Item("MESSAGE").Text = "Clearing existing transaction export details"


    '                    m_lReturn = m_oBusiness.DeleteTransactionExport(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sDocumentRef:=sDocumentRef)

    '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear transaction export details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                        GoTo End_ProcessFailedTransaction

    '                    End If

    '                    stbMain.Items.Item("MESSAGE").Text = "Clearing existing stats details"


    '                    m_lReturn = m_oBusiness.DeleteStatsDetail(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sDocumentRef:=sDocumentRef)

    '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear stats details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                        GoTo End_ProcessFailedTransaction

    '                    End If

    '                    stbMain.Items.Item("MESSAGE").Text = "Recreating stats details"

    '                    m_lReturn = m_oBusiness.CreateStatsDetail(v_lStatsFolderCnt:=lStatsFolderCnt)

    '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create stats details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                        GoTo End_ProcessFailedTransaction

    '                    End If

    '                    'now create transaction export folder and details
    '                    CreateTransactionExport(v_lStatsFolderCnt:=lStatsFolderCnt, r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)
    '                Else
    '                    'recreate transaction exports? (at the moment this is the default option)
    '                    If Me.chkRecreateTransExport.CheckState = CheckState.Checked Then

    '                        stbMain.Items.Item("MESSAGE").Text = "Clearing existing transaction export details"


    '                        m_lReturn = m_oBusiness.DeleteTransactionExport(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sDocumentRef:=sDocumentRef)

    '                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear transaction exports", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                            GoTo End_ProcessFailedTransaction

    '                        End If

    '                        'now create transaction export folder and details
    '                        CreateTransactionExport(v_lStatsFolderCnt:=lStatsFolderCnt, r_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)
    '                    Else

    '                        stbMain.Items.Item("MESSAGE").Text = "Getting transaction export folder count for this policy"

    '                        'we need to get the existing transaction export folder count if we are not regenerating it
    '                        lTransactionExportFolderCnt = CInt(oListItem.Text)

    '                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get transaction export folder count", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                            GoTo End_ProcessFailedTransaction

    '                        End If


    '                    End If
    '                End If

    '                'post transactions to accounts
    '                If lTransactionExportFolderCnt <> 0 Then
    '                    stbMain.Items.Item("MESSAGE").Text = "Posting transactions please wait"

    '                    m_lReturn = m_oBusiness.SendToOrion(v_lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

    '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '                        GoTo End_ProcessFailedTransaction

    '                    End If

    '                    If Not Information.IsArray(vDeletedIndex) Then

    '                        ReDim vDeletedIndex(0)
    '                    Else
    '                        'stored index into an arrray so we can delete it from the list

    '                        ReDim Preserve vDeletedIndex(vDeletedIndex.GetUpperBound(0) + 1)
    '                    End If



    '                    vDeletedIndex(vDeletedIndex.GetUpperBound(0)) = oListItem.Index + 1
    '                End If
    '            End If
    '        Next oListItem

    '        GoTo End_ProcessFailedTransaction

    'Err_ProcessFailedTransaction:

    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ProcessFailedTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

    '        GoTo End_ProcessFailedTransaction
    '        Resume
    'End_ProcessFailedTransaction:

    '        If Information.IsArray(vDeletedIndex) Then
    '            'delete from list view - do it this way because index changes as we delete from listview

    '            For lCount = vDeletedIndex.GetUpperBound(0) To 0 Step -1
    '                'delete from list view

    '                Me.lvwSelectPolicy.Items.RemoveAt(CInt(vDeletedIndex(lCount)) - 1)
    '            Next

    '            Me.lvwSelectPolicy.Refresh()
    '        End If

    '        If lvwSelectPolicy.Items.Count > 0 Then
    '            stbMain.Items.Item("COUNT").Text = CStr(lvwSelectPolicy.Items.Count)
    '        End If

    '        stbMain.Items.Item("MESSAGE").Text = "Ready"

    '    End Sub

    Private Sub ProcessSinglePolicy()

        Dim lCount As Integer



        For lCount = Me.optSinglePolicy.GetLowerBound(0) To Me.optSinglePolicy.GetUpperBound(0)
            If Me.optSinglePolicy(lCount).Checked Then
                Exit For
            End If
        Next

        Select Case lCount
            'Case 0 'repost
            '    RepostPolicyVersion()
            'Case 1 'delete
            '    DeletePolicyVersion()
            'Case 2 'set status
            '    SetStatusPolicyVersion()
            Case 0 'Reverse Transaction
                ReversePolicyTransaction()
            Case 1 'Reverse & Regenerate Transaction
                ReverseAndRegenerateTransaction()
        End Select

    End Sub

    '*************************************************************************************
    ' This will recreate stats folder, details, transaction exoprt folder and detail
    ' and repost to account.
    ' if posting is done for this policy version then give user the option to delete
    ' document and all its allocation and repost.
    ' also give user the option to recalculate reinsurance
    '*************************************************************************************
    Private Sub RepostPolicyVersion()
        Dim bControlTrans, bSIRReinsurance As Object


        'Modified by Sumeet Singh on 5/25/2010 11:38:09 AM to do list (Iteration 3)
        'Dim oReinsurance As bSIRReinsurance.Form
        Dim oReinsurance As Object

        Dim oControlTrans As bControlTrans.Automated
        Dim sTransactionType, sDocumentRef As String
        Dim vDeletedIndex As Object
        Dim lCount As Integer
        Dim sRiskCnt As String = ""
        Dim lValidRIBand, lReinsBand As Integer

        On Error GoTo Catch_Renamed

        Dim temp_oReinsurance As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oReinsurance, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oReinsurance = temp_oReinsurance
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRReinsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            GoTo Finally_Renamed
        End If

        Dim temp_oControlTrans As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oControlTrans, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oControlTrans = temp_oControlTrans

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bControlTrans", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            GoTo Finally_Renamed
        End If

        For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
            If oListItem.Checked Then

                lCount += 1
                stbMain.Items.Item("COUNT").Text = CStr(lCount)

                'work out transaction type
                Select Case Convert.ToString(oListItem.SubItems.Item(ACFieldPVPolicyType).Tag)
                    Case 2 'new business
                        sTransactionType = "NB"
                    Case 3 'renewal
                        sTransactionType = "REN"
                    Case 8
                        sTransactionType = "MTC"
                    Case 5 'MTA permenant
                        sTransactionType = "MTA"
                    Case 9 'MTA reinstatement
                        sTransactionType = "MTA"
                    Case Else
                        MessageBox.Show("Selected version is not valid for reposting", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                End Select

                'ask user for risk id - default with selected one from risk list view if its populated
                sRiskCnt = "0"
                If oListItem.Text.Trim() = txtInsuranceFileCnt.Text.Trim() Then
                    If lvwRisk.Items.Count > 0 Then
                        sRiskCnt = lvwRisk.Items.Item(lvwRisk.FocusedItem.Index).SubItems.Item(ACFieldRDRiskCnt - 1).Text
                    End If
                End If

                sRiskCnt = GetUserInput(v_sMessage:="Enter Risk ID", v_lIsNumeric:=gPMConstants.PMEReturnCode.PMTrue, v_sDefault:=sRiskCnt)

                'user must have cancelled or enter a empty string
                If CInt(sRiskCnt) = 0 Then
                    GoTo Finally_Renamed
                End If

                stbMain.Items.Item("MESSAGE").Text = "Check to see if this version of policy is in account - " & "(" & CStr(CInt(oListItem.Text)) & ")"

                m_lReturn = m_oBusiness.IsPolicyVersionInAccount(v_lInsuranceFileCnt:=CInt(oListItem.Text), r_sDocumentRef:=sDocumentRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to for account details of this policy version - " & oListItem.SubItems.Item(ACFieldPVInsuranceRef - 1).Text, vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed
                End If

                sDocumentRef = sDocumentRef.Trim()

                If sDocumentRef <> "" Then
                    If MessageBox.Show("This version of the policy has been posted to account." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to delete all postings, allocations and repost?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                        GoTo Finally_Renamed
                    Else
                        stbMain.Items.Item("MESSAGE").Text = "Deleting document " & sDocumentRef & " please wait"

                        If m_oBusiness.DeleteDocument(v_sDocumentRef:=sDocumentRef) <> gPMConstants.PMEReturnCode.PMTrue Then
                            GoTo Finally_Renamed
                        End If
                    End If
                End If

                If MessageBox.Show("Do you want to recalculate reinsurance?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    stbMain.Items.Item("MESSAGE").Text = "Recalculating reinsurance please wait"


                    m_lReturn = oReinsurance.SetProcessModes(vTransactionType:=sTransactionType, vEffectiveDate:=oListItem.SubItems.Item(ACFieldPVPolicyStart - 1).Text)

                    ' Get ready to do reinsurance (risk level)

                    oReinsurance.InsuranceFileCnt = CInt(oListItem.Text)

                    oReinsurance.RiskId = CInt(sRiskCnt)

                    ' recalculate reinsurance

                    If oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to recalculate reinsurance", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                    End If

                    ' Note : load and save reinsurance details to fix any roundings
                    ' Get reinsurance details

                    If oReinsurance.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get reinsurance details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                    End If

                    ' Save new reinsurance details

                    If oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to save reinsurance details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                    End If

                    ' Do we have valid reinsurance bands ie adds up to 100%

                    If oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to validate reinsurance bands", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                    End If

                    If lValidRIBand <> 0 Then
                        MessageBox.Show("Reinsurance bands do not add to 100%", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        GoTo Finally_Renamed
                    End If
                End If

                stbMain.Items.Item("MESSAGE").Text = "Clear existing statistics for " & "(" & CStr(CInt(oListItem.Text)) & ")"

                m_lReturn = m_oBusiness.DeleteStatsFolder(v_lInsuranceFileCnt:=CInt(oListItem.Text))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear existing statistics details", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed
                End If


                m_lReturn = oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed bControlTrans.SetProcessModes()", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed
                End If


                stbMain.Items.Item("MESSAGE").Text = "Creating statistics and posting to account for " & "(" & CStr(CInt(oListItem.Text)) & ")"

                oControlTrans.InsuranceFileCnt = CInt(oListItem.Text)

                m_lReturn = oControlTrans.Start()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recreate statistics and repost", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed
                End If

                If Not Information.IsArray(vDeletedIndex) Then

                    ReDim vDeletedIndex(0)
                Else
                    'stored index into an arrray so we can delete it from the list

                    ReDim Preserve vDeletedIndex(vDeletedIndex.GetUpperBound(0) + 1)
                End If



                vDeletedIndex(vDeletedIndex.GetUpperBound(0)) = oListItem.Index + 1

            End If
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed RepostPolicyVersion", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        stbMain.Items.Item("MESSAGE").Text = "Remove reposted policy versions from list"
        If Information.IsArray(vDeletedIndex) Then
            'loop backward to make sure we get the correct ones as index on listview changes as we delete

            For lCount = vDeletedIndex.GetUpperBound(0) To 0 Step -1

                Me.lvwPolicyVersion.Items.RemoveAt(CInt(vDeletedIndex(lCount)) - 1)
            Next

            Me.lvwPolicyVersion.Refresh()
        End If

        If Not (oReinsurance Is Nothing) Then

            m_lReturn = oReinsurance.Terminate()
            oReinsurance = Nothing
        End If

        If Not (oControlTrans Is Nothing) Then

            oControlTrans.Dispose()
            oControlTrans = Nothing
        End If

        stbMain.Items.Item("COUNT").Text = ""
        stbMain.Items.Item("MESSAGE").Text = "Ready"
    End Sub

    '********************************************************************************************
    ' delete this policy version and all allocations
    '********************************************************************************************
    Private Sub DeletePolicyVersion()

        Dim lCount As Integer
        Dim vDeletedIndex, vDocumentArray(,) As Object
        Dim sDocumentRef As String = ""

        On Error GoTo Err_DeletePolicyVersion

        'confirm that user really want to do this
        If MessageBox.Show("Selected policy versions and all their dependancies will be deleted." & Strings.Chr(13) & Strings.Chr(10) & "Are you sure?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
            If oListItem.Checked Then
                lCount += 1
                stbMain.Items.Item("COUNT").Text = CStr(lCount)

                stbMain.Items.Item("MESSAGE").Text = "Getting document reference for policy version " & "(" & oListItem.Text & ")"


                m_lReturn = m_oBusiness.GetPolicyVersionDocument(v_lInsuranceFileCnt:=CInt(oListItem.Text), r_vDocumentRef:=vDocumentArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to get document ref for policy version" & "(" & oListItem.Text & ")", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    GoTo End_DeletePolicyVersion
                End If

                If Information.IsArray(vDocumentArray) Then
                    'delete this document from account

                    For lDocCount As Integer = 0 To vDocumentArray.GetUpperBound(1)

                        sDocumentRef = CStr(vDocumentArray(0, lDocCount))

                        stbMain.Items.Item("MESSAGE").Text = "Deleting document " & "(" & sDocumentRef & ")" & " from account for policy version " & "(" & oListItem.Text & ")"


                        m_lReturn = m_oBusiness.DeleteDocument(v_sDocumentRef:=sDocumentRef)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to delete document " & sDocumentRef & " for policy version " & "(" & oListItem.Text & ")", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            GoTo End_DeletePolicyVersion
                        End If
                    Next lDocCount
                End If

                'delete stats detail for this policy version
                stbMain.Items.Item("MESSAGE").Text = "Deleting statistics for policy version (" & oListItem.Text & ")"


                m_lReturn = m_oBusiness.DeleteStatsFolder(v_lInsuranceFileCnt:=CInt(oListItem.Text))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to delete statistics for policy version " & "(" & oListItem.Text & ")", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    GoTo End_DeletePolicyVersion
                End If

                'now delete this version of policy

                m_lReturn = m_oBusiness.DeletePolicyVersion(v_lInsuranceFileCnt:=CInt(oListItem.Text))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to delete policy version " & "(" & oListItem.Text & ")", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    GoTo End_DeletePolicyVersion
                End If

                If Not Information.IsArray(vDeletedIndex) Then

                    ReDim vDeletedIndex(0)
                Else
                    'stored index into an arrray so we can delete it from the list

                    ReDim Preserve vDeletedIndex(vDeletedIndex.GetUpperBound(0) + 1)
                End If



                vDeletedIndex(vDeletedIndex.GetUpperBound(0)) = oListItem.Index + 1

            End If
        Next oListItem

        GoTo End_DeletePolicyVersion

Err_DeletePolicyVersion:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed DeletePolicyVersion", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        GoTo End_DeletePolicyVersion

End_DeletePolicyVersion:

        stbMain.Items.Item("MESSAGE").Text = "Remove deleted policy versions from list"
        If Information.IsArray(vDeletedIndex) Then
            'loop backward to make sure we get the correct ones as index on listview changes as we delete

            For lCount = vDeletedIndex.GetUpperBound(0) To 0 Step -1

                Me.lvwPolicyVersion.Items.RemoveAt(CInt(vDeletedIndex(lCount)) - 1)
            Next

            Me.lvwPolicyVersion.Refresh()
        End If

        stbMain.Items.Item("COUNT").Text = ""
        stbMain.Items.Item("MESSAGE").Text = "Ready"

    End Sub

    '********************************************************************************************
    ' set status for selected versions
    '********************************************************************************************
    Private Sub SetStatusPolicyVersion()

        Dim lCount As Integer
        Dim sMessage As String = ""
        Dim bRedisplay As Boolean

        On Error GoTo Catch_Renamed
        bRedisplay = False

        If IsTick(oListView:=lvwPolicyVersion) > 0 Then
            'confirm that user really want to do this
            If MessageBox.Show("Selected policy versions will be set to new status." & Strings.Chr(13) & Strings.Chr(10) & "Are you sure?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.Yes Then
                GoTo Finally_Renamed
            End If
        End If

        stbMain.Items.Item("MESSAGE").Text = "Setting status for selected policy versions please wait"

        For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
            If oListItem.Checked Then
                lCount += 1
                stbMain.Items.Item("COUNT").Text = CStr(lCount)

                If m_oBusiness.SetStatusPolicyVersion(v_lInsuranceFileCnt:=CInt(oListItem.Text), v_lInsuranceFileStatusID:=VB6.GetItemData(cboPolicyStatus, cboPolicyStatus.SelectedIndex), r_sMessage:=sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit For
                End If

                bRedisplay = True
            End If
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed SetStatusPolicyVersion", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusPolicyVersion()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        stbMain.Items.Item("MESSAGE").Text = "Ready"
        stbMain.Items.Item("COUNT").Text = ""

        If bRedisplay Then
            stbMain.Items.Item("MESSAGE").Text = "Redisplaying details please wait"
            cmdGetPolicyVersion_Click(cmdGetPolicyVersion, New EventArgs())
        End If



    End Sub

    Private Sub ReversePolicyTransaction()
        Try

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                If oListItem.Checked Then
                    Dim sDocumentRef As String = oListItem.SubItems(4).Text
                    If sDocumentRef <> "" Then
                        m_lReturn = ReversePolicyVersionTransaction(v_nInsuranceFileCnt:=CInt(oListItem.Text), v_sDocumentRef:=sDocumentRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReversePolicyVersionTransaction()", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If
                End If
            Next oListItem

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ReversePolicyTransaction ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Function ReversePolicyVersionTransaction(ByVal v_nInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String, Optional ByVal v_bIsCalledFromDuplicateVersionTab As Boolean = False) As Integer
        Try
            Dim nStatsFolderCnt, nTransactionExportFolderCnt As Integer
            Dim nDocumentId As Integer
            Dim nStatus As Integer

            m_oBusiness.BeginTrans()

            m_lReturn = m_oBusiness.IsDocumentInStats(v_sDocumentRef:=v_sDocumentRef, r_lStatus:=nStatus)

            If nStatus = gPMConstants.PMEReturnCode.PMNotFound Then
                stbMain.Items.Item("MESSAGE").Text = "Reversing transactions please wait"
                m_lReturn = m_oBusiness.ReverseDocument(v_sDocumentRef:=v_sDocumentRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Reversse Stats ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not _optSinglePolicy_4.Checked Or v_bIsCalledFromDuplicateVersionTab Or optClonePolicyReverse.Checked Or optClonePolicyReverseRegenerate.Checked Then
                    m_lReturn = m_oBusiness.AddDataFixUtilityLog(v_sPMNumber:=txtPMNumber.Text, v_sCreatedBy:="DataFixUtility", v_lInsuranceFileCnt:=v_nInsuranceFileCnt, _
                         v_sOldDocumentRef:=v_sDocumentRef, v_sNewDocumentid:=0, v_bIsReversal:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else

                m_lReturn = m_oBusiness.CreateReverseStats(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, r_nStatsFolderCnt:=nStatsFolderCnt, v_sDocumentRef:=v_sDocumentRef)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Reversse Stats ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oBusiness.AddReversalDocument(v_lOldDocumentRef:=v_sDocumentRef, v_nStatsFolderCnt:=nStatsFolderCnt, r_vDocumentID:=nDocumentId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Reversse Stats ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                CreateTransactionExport(v_lStatsFolderCnt:=nStatsFolderCnt, r_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, v_sDocumentRef:=v_sDocumentRef)

                If nDocumentId <> 0 Then
                    stbMain.Items.Item("MESSAGE").Text = "Reversing transactions please wait"

                    ' m_lReturn = m_oBusiness.SendToOrion(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_lDocumentId:=nDocumentId)
                    m_lReturn = m_oBusiness.AddReversalTransdetail(v_lOldDocumentRef:=v_sDocumentRef, r_vDocumentID:=nDocumentId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'Update the comment of reverse document
                    m_lReturn = m_oBusiness.UpdateTransDetailComment(nDocumentId, v_sDocumentRef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update transdetail", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTransDetailComment", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Update the comment of Original document

                    m_lReturn = m_oBusiness.AddTransdetailEx(nDocumentId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not _optSinglePolicy_4.Checked Or v_bIsCalledFromDuplicateVersionTab Or optClonePolicyReverse.Checked Or optClonePolicyReverseRegenerate.Checked Then

                        m_lReturn = m_oBusiness.AddDataFixUtilityLog(v_sPMNumber:=txtPMNumber.Text, v_sCreatedBy:="DataFixUtility", v_lInsuranceFileCnt:=v_nInsuranceFileCnt, _
                                             v_sOldDocumentRef:=v_sDocumentRef, v_sNewDocumentid:=nDocumentId, v_bIsReversal:=True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If
            stbMain.Items.Item("MESSAGE").Text = "Done"

            m_oBusiness.CommitTrans()

            Return m_lReturn

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            m_oBusiness.RollbackTrans()
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReversePolicyVersionTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function


    Private Sub ReverseAndRegenerateTransaction()
        Try
            Dim oControlTrans As bControlTrans.Automated
            Dim bFirstItem As Boolean = True
            Dim oAgentCommission As Object
            Dim temp_oControlTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oControlTrans, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oControlTrans = temp_oControlTrans
            Dim sTransactionType As String = ""
            Dim sDocumentRef As String = ""

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                If oListItem.Checked Then
                    m_oBusiness.BeginTrans()
                    sDocumentRef = oListItem.SubItems(4).Text

                    Select Case Convert.ToString(oListItem.SubItems.Item(ACFieldPVPolicyType).Tag)
                        Case 2 'new business
                            If sDocumentRef.StartsWith("SND") Then
                                sTransactionType = "NB"
                            Else
                                If sDocumentRef.StartsWith("SED") Then
                                    sTransactionType = "MTA"
                                Else
                                    sTransactionType = "REN"
                                End If
                            End If
                        Case 3 'renewal
                            sTransactionType = "REN"
                        Case 8
                            sTransactionType = "MTC"
                        Case 5 'MTA permenant
                            sTransactionType = "MTA"
                        Case 9 'MTA reinstatement
                            sTransactionType = "MTA"
                        Case Else
                            MessageBox.Show("Selected version is not valid for reposting", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Throw New System.Exception
                    End Select

                    If sDocumentRef.StartsWith("SID") Then
                        sTransactionType = "MTR"
                    End If
                    If chkRatingRefresh.Checked Then
                        txtInsuranceFileCnt.Text = CInt(oListItem.Text)
                        PopulateRiskDetail()
                        GetRiskRating(sTransactionType)

                        If MessageBox.Show("Do you want to proceed with transaction posting?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then


                            m_lReturn = m_oBusiness.UpdateInsuranceFilePremium(nInsuranceFileCnt:=CInt(oListItem.Text))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Insurance_file Premium", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFilePremium", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                                Throw New System.Exception
                            End If
                            m_lReturn = m_oBusiness.CalculateAgentCommission(nInsuranceFileCnt:=CInt(oListItem.Text), sTransactionType:=sTransactionType, r_vntResult:=oAgentCommission)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Risk", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                                Throw New System.Exception
                            End If

                            For Each oriskListItem As ListViewItem In Me.lvwRisk.Items

                                m_lReturn = m_oBusiness.RecalculateRiskTaxes(nInsuranceFileCnt:=CInt(oListItem.Text), nRiskCnt:=CInt(oriskListItem.SubItems(1).Text), v_lTask:=2, sTransactionType:=sTransactionType)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Risk", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                                    Throw New System.Exception
                                End If
                            Next oriskListItem
                        Else
                            Exit Sub
                        End If
                    End If
                    If chkRIRefresh.Checked Then
                        m_lReturn = m_oBusiness.RIRefresh(nInsuranceFileCnt:=CInt(oListItem.Text), sTransactionType:=sTransactionType, lIsUpdatePerilData:=ChkUpdatePerilandRaringData.Checked)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Risk", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If

                    If sDocumentRef <> "" Then
                        'Reverse Transaction
                        m_lReturn = ReversePolicyVersionTransaction(v_nInsuranceFileCnt:=CInt(oListItem.Text), v_sDocumentRef:=sDocumentRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReversePolicyVersionTransaction()", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                        'Regenerate Transaction
                        stbMain.Items.Item("MESSAGE").Text = "Document reference Reversed. Regenerating transactions in progress please wait"

                        m_lReturn = oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed bControlTrans.SetProcessModes()", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If



                        oControlTrans.InsuranceFileCnt = CInt(oListItem.Text)
                        m_lReturn = oControlTrans.Start()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recreate statistics and repost", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If

                    bFirstItem = False


                    m_lReturn = m_oBusiness.AddDataFixUtilityLog(v_sPMNumber:=txtPMNumber.Text, v_sCreatedBy:="DataFixUtility", v_lInsuranceFileCnt:=CInt(oListItem.Text),
                                      v_sOldDocumentRef:=sDocumentRef, v_sNewDocumentid:=0, v_bIsReversal:=False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception
                    End If
                    m_oBusiness.CommitTrans()
                End If


            Next oListItem



            stbMain.Items.Item("MESSAGE").Text = ""

            If Not (oControlTrans Is Nothing) Then
                'oControlTrans.Dispose()
                oControlTrans = Nothing
            End If

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            m_oBusiness.RollbackTrans()
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReverseAndRegenerateTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAndRegenerateTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try
    End Sub

    '********************************************************************************************
    ' process the selection from Miscellaneous tab
    '********************************************************************************************
    Private Sub Miscellaneous()

        Dim lCount As Integer

        'loop to see which one is selected
        For lCount = 0 To Me.optMiscellaneous.Length
            If Me.optMiscellaneous(lCount).Checked Then
                Exit For
            End If
        Next

        Select Case lCount
            Case 0 'delete document and all its allocations
                DeleteDocument()
            Case 1 'delete all allocation for this document
                DeleteDocumentAllocation()
            Case 2 'create stats and posting for closed claims without transactions when these claims were zeroised.
                'Dim sMessage As String
                'm_lReturn = m_oBusiness.ReprocessClaim(r_sMessage:=sMessage, v_lClaimID:=23)
                ClosedClaimRepost()
            Case 3 'add RI to policy risk level
                AddRIToPolicy()
            Case 4 'delete claim and all associated postings including stats
                DeleteClaim()
            Case 5 'delete claim and all associated postings including stats
                ReverseDocumentRef()
        End Select
    End Sub

    '********************************************************************************************
    ' delete document and all its allocations from account
    ' including stats folder and detail
    '********************************************************************************************
    Private Sub DeleteDocument(Optional ByVal v_sDocumentRef As String = "", Optional ByVal v_bGetDocumentRef As Boolean = True, Optional ByRef r_sMessage As String = "")

        Dim sMessage, sDocumentRef As String

        On Error GoTo Err_DeleteDocument
        sMessage = ""

        stbMain.Items.Item("MESSAGE").Text = "Getting document ref"

        If v_bGetDocumentRef Then
            sDocumentRef = GetUserInput(v_sMessage:="Please enter document ref", v_sDefault:=v_sDocumentRef)
        Else
            sDocumentRef = v_sDocumentRef
        End If

        If sDocumentRef = "" Then
            GoTo End_DeleteDocument
        End If

        stbMain.Items.Item("MESSAGE").Text = "Deleting document - " & sDocumentRef & " from account"

        m_lReturn = m_oBusiness.DeleteDocument(v_sDocumentRef:=sDocumentRef)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to delete document - (" & sDocumentRef & ") from account"
            GoTo End_DeleteDocument
        End If

        GoTo End_DeleteDocument

Err_DeleteDocument:

        If sMessage = "" Then
            sMessage = "Failed DeleteDocument"
        End If

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        GoTo End_DeleteDocument

End_DeleteDocument:

        stbMain.Items.Item("MESSAGE").Text = "Ready"

        If Not False Then
            r_sMessage = sMessage
        End If
    End Sub

    '********************************************************************************************
    ' delete document and all its allocations from account
    '********************************************************************************************
    Private Sub DeleteDocumentAllocation()

        Dim sDocumentRef As String = ""

        On Error GoTo Err_DeleteDocumentAllocation

        stbMain.Items.Item("MESSAGE").Text = "Getting document ref"
        sDocumentRef = GetUserInput(v_sMessage:="Please enter document ref")

        If sDocumentRef = "" Then
            GoTo End_DeleteDocumentAllocation
        End If

        stbMain.Items.Item("MESSAGE").Text = "Deleting all allocations for document (" & sDocumentRef & ") from account"

        m_lReturn = m_oBusiness.DeleteDocumentAllocation(v_sDocumentRef:=sDocumentRef)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete allocation for document (" & sDocumentRef & ") from account", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocumentAllocation", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

            GoTo End_DeleteDocumentAllocation
        End If

        GoTo End_DeleteDocumentAllocation

Err_DeleteDocumentAllocation:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed DeleteDocumentAllocation", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocumentAllocation", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        GoTo End_DeleteDocumentAllocation

End_DeleteDocumentAllocation:

        stbMain.Items.Item("MESSAGE").Text = "Ready"
    End Sub


    '********************************************************************************************
    ' loop through until we get a value and check to see if its numeric if required
    ' return "0" or empty string if user cancelled
    '********************************************************************************************
    Private Function GetUserInput(ByVal v_sMessage As String, Optional ByVal v_lIsNumeric As Integer = 0, Optional ByVal v_sDefault As String = "0") As String


        Dim sValue As String = "AREYOUHAVINGALAUGH"
        Do While sValue = "AREYOUHAVINGALAUGH"
            sValue = Interaction.InputBox(v_sMessage, "Repost Transaction", v_sDefault)

            'user cancelled or ok with empty string
            If sValue = "" Then
                If v_lIsNumeric = 1 Then
                    sValue = "0"
                End If
            Else
                If v_lIsNumeric = 1 Then
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        MessageBox.Show("Please enter a numeric value", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        sValue = "AREYOUHAVINGALAUGH"
                    End If
                End If
            End If
        Loop

        Return sValue

    End Function

    '********************************************************************************************
    ' tick record on list view
    ' v_lAll = 1 for all records, v_lAll = 0 for high lighted records
    '********************************************************************************************
    Private Sub TickListView(ByRef r_oListView As ListView, Optional ByVal v_lAll As Integer = 0, Optional ByVal v_bValue As Boolean = True)

        Try

            'loop though all records and tick all if v_lall = 1 or just tick selected ones
            For Each oListItem As ListViewItem In r_oListView.Items
                oListItem.Checked = (v_lAll = 1 Or oListItem.Selected Or oListItem.Checked) And v_bValue
            Next oListItem

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed tick list view", vApp:=ACApp, vClass:=ACClass, vMethod:="TickListView()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try

    End Sub

    '********************************************************************************************
    'return number of items ticked on list view
    '
    '********************************************************************************************
    Private Function IsTick(ByVal oListView As ListView) As Integer

        Dim lCount As Integer

        On Error GoTo Catch_Renamed
        lCount = 0

        For Each oListItem As ListViewItem In oListView.Items
            If oListItem.Checked Then
                lCount += 1
            End If
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:
        lCount = -1

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for ticked items on listview", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTick()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return lCount
    End Function


    '*************************************************************************************************************
    'get all closed claims which match criteria below
    '-get all closed claims where the last transaction is not 'CLA' or 'CLP'
    '-or last transaction is 'CLA' or 'CLP' and transaction amount <> this_revision or transaction amount <> this_payment
    '-and its in the imbalanced closed claims set
    '-exclude claims with multiple perils and those with last posting is a clp with amount = this_revision
    ' Note: user can enter a claim number or leave default to get failed claims
    '*************************************************************************************************************
    Private Sub ClosedClaimRepost()

        Dim sMessage As String = ""
        Dim vResultArray(,) As Object
        Dim lMax As Integer
        Dim sClaimNumber As String = ""

        On Error GoTo Catch_Renamed

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        sClaimNumber = Interaction.InputBox("Enter claim number", "Repost Transaction", "Get All").Trim()

        If sClaimNumber = "Get All" Then
            stbMain.Items.Item(0).Text = "Getting imbalance closed claims where last transaction is not CLA/CLP or is but transaction amount <> this_revision/this_payment, excluding multi perils and those with last posting is CLP with amount=this_revision"


            If m_oBusiness.GetClosedClaimWithNoPosting(r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get closed claims without transactions when zeroised", vApp:=ACApp, vClass:=ACClass, vMethod:="ClosedClaimRepost()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        ElseIf sClaimNumber = "" Then
            GoTo Finally_Renamed
        Else
            ReDim vResultArray(1, 0)

            vResultArray(0, 0) = 0

            vResultArray(1, 0) = sClaimNumber
        End If

        If Information.IsArray(vResultArray) Then

            lMax = vResultArray.GetUpperBound(1)
            If MessageBox.Show("There are " & lMax + 1 & " claims." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                GoTo Finally_Renamed
            End If

            'repost reserve.this_revision for each claim
            For lCount As Integer = 0 To lMax

                stbMain.Items.Item(0).Text = "Processing claim number " & CStr(vResultArray(1, lCount))
                stbMain.Items.Item(1).Text = CStr(lCount + 1) & "/" & CStr(lMax + 1)

                m_lReturn = m_oBusiness.ReprocessClaim(r_sMessage:=sMessage, v_sClaimNumber:=vResultArray(1, lCount), v_lClaimID:=vResultArray(0, lCount))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    MessageBox.Show("Failed to repost claim " & CStr(vResultArray(1, lCount)) & Strings.Chr(13) & Strings.Chr(10) & sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Next lCount
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        If sMessage = "" Then
            sMessage = "Failed to repost transactions for closed claims"
        End If

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ClosedClaimRepost()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item(0).Text = "Ready"
        stbMain.Items.Item(1).Text = ""
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Exit Sub
    End Sub

    '*************************************************************************************************************
    'get failed claim transactions.
    'for failed claim payment we are guessing the payment using amount and payment date.
    'user will have to make a decision whether it is correct for reposting
    '*************************************************************************************************************
    Private Sub PopulateFailedClaimTransaction()

        Dim oListItem As ListViewItem
        Dim vFailedClaim(,) As Object
        Dim lMax As Integer
        Dim sPostingStatus As String = ""

        Const ACSQLDefaultDate As String = "1900-Jan-01"

        On Error GoTo Catch_Renamed
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        stbMain.Items.Item("MESSAGE").Text = "Getting failed claim transactions please wait"

        'get all failed transactions

        If m_oBusiness.GetFailedClaimTransaction(r_vResultArray:=vFailedClaim) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get failed claim transactions", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        If Not Information.IsArray(vFailedClaim) Then
            GoTo Finally_Renamed
        End If


        lMax = vFailedClaim.GetUpperBound(1)

        stbMain.Items.Item("MESSAGE").Text = "Displaying failed claim transactions please wait"

        lvwClaim.Items.Clear()
        For lCount As Integer = 0 To lMax

            'claim number

            oListItem = lvwClaim.Items.Add(CStr(vFailedClaim(ACFieldFCClaimNumber, lCount)).Trim())

            'doc ref


            'Modified by Sumeet Singh on 5/25/2010 1:00:16 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCDocRef,  , CStr(vFailedClaim(ACFieldFCDocRef, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCDocRef, vFailedClaim(ACFieldFCDocRef, lCount).Trim())

            'doc date


            'Modified by Sumeet Singh on 5/25/2010 1:01:00 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCDocDate,  , CStr(vFailedClaim(ACFieldFCDocDate, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCDocDate, vFailedClaim(ACFieldFCDocDate, lCount).Trim())

            'payment date



            'Modified by Sumeet Singh on 5/25/2010 1:01:17 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPaymentDate,  , IIf(DateAndTime.DateDiff("d", CDate(vFailedClaim(ACFieldFCPaymentDate, lCount)), CDate(ACSQLDefaultDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0, "", CStr(vFailedClaim(ACFieldFCPaymentDate, lCount))))
            oListItem.SubItems.Insert(ACFieldFCPaymentDate, IIf(DateAndTime.DateDiff("d", CDate(vFailedClaim(ACFieldFCPaymentDate, lCount)), CDate(ACSQLDefaultDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0, "", vFailedClaim(ACFieldFCPaymentDate, lCount)))

            'premium total (value from stats_folder)


            'Modified by Sumeet Singh on 5/25/2010 1:03:58 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPremiumTotal,  , CStr(vFailedClaim(ACFieldFCPremiumTotal, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCPremiumTotal, vFailedClaim(ACFieldFCPremiumTotal, lCount).Trim())

            'payment amount (value from payment table)


            'Modified by Sumeet Singh on 5/25/2010 1:04:24 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPaymentAmount,  , CStr(vFailedClaim(ACFieldFCPaymentAmount, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCPaymentAmount, vFailedClaim(ACFieldFCPaymentAmount, lCount).Trim())

            'payment party code


            'Modified by Sumeet Singh on 5/25/2010 1:04:43 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPaymentPartyCode,  , CStr(vFailedClaim(ACFieldFCPaymentPartyCode, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCPaymentPartyCode, vFailedClaim(ACFieldFCPaymentPartyCode, lCount).Trim())

            'posting status

            Select Case CStr(vFailedClaim(ACFieldFCPostingStatus, lCount)).Trim().ToUpper()
                Case "P"
                    sPostingStatus = "Pending (p)"
                Case "S"
                    sPostingStatus = "Sending (s)"
                Case "F"
                    sPostingStatus = "Failed (f)"
                Case "N"
                    sPostingStatus = "No Exports (n)"
            End Select


            'Modified by Sumeet Singh on 5/25/2010 1:05:06 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPostingStatus,  , sPostingStatus)
            oListItem.SubItems.Insert(ACFieldFCPostingStatus, CObj(sPostingStatus))

            'export folder cnt


            'Modified by Sumeet Singh on 5/25/2010 1:05:20 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCExportFolderCnt,  , CStr(vFailedClaim(ACFieldFCExportFolderCnt, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCExportFolderCnt, vFailedClaim(ACFieldFCExportFolderCnt, lCount).Trim())

            'insurance file cnt


            'Modified by Sumeet Singh on 5/25/2010 1:05:40 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCInsuranceFileCnt,  , CStr(vFailedClaim(ACFieldFCInsuranceFileCnt, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCInsuranceFileCnt, vFailedClaim(ACFieldFCInsuranceFileCnt, lCount).Trim())

            'claim id


            'Modified by Sumeet Singh on 5/25/2010 1:06:33 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCClaimID,  , CStr(vFailedClaim(ACFieldFCClaimID, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCClaimID, vFailedClaim(ACFieldFCClaimID, lCount).Trim())

            'payment party cnt


            'Modified by Sumeet Singh on 5/25/2010 1:06:56 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPaymentPartyCnt,  , CStr(vFailedClaim(ACFieldFCPaymentPartyCnt, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCPaymentPartyCnt, vFailedClaim(ACFieldFCPaymentPartyCnt, lCount).Trim())

            'original reserve id


            'Modified by Sumeet Singh on 5/25/2010 1:07:23 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCOriginalReserveID,  , CStr(vFailedClaim(ACFieldFCOriginalReserveID, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCOriginalReserveID, vFailedClaim(ACFieldFCOriginalReserveID, lCount).Trim())

            'payment id


            'Modified by Sumeet Singh on 5/25/2010 1:08:03 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldFCPaymentID,  , CStr(vFailedClaim(ACFieldFCPaymentID, lCount)).Trim())
            oListItem.SubItems.Insert(ACFieldFCPaymentID, vFailedClaim(ACFieldFCPaymentID, lCount).Trim())

            'display claim payment in green

            If CStr(vFailedClaim(ACFieldFCDocRef, lCount)).Trim().Substring(0, 3) = "CLP" Then
                lvwClaim.Items.Item(oListItem.Index).ForeColor = Color.Lime
                For lCol As Integer = 1 To lvwClaim.Columns.Count - 1
                    lvwClaim.Items.Item(oListItem.Index).SubItems.Item(lCol - 1).ForeColor = Color.Lime
                Next lCol
            End If

        Next lCount

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate failed claim transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFailedClaimTransaction()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
        stbMain.Items.Item("COUNT").Text = CStr(lvwClaim.Items.Count)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Exit Sub
    End Sub

    '*************************************************************************************************************
    ' reposting failed claim transaction
    'Note: user will have to decide which failed transaction is legitimate for reposting.
    '*************************************************************************************************************
    Private Sub ProcessFailedClaimTransaction()

        Dim sMessage As String = ""
        Dim lInAccount As gPMConstants.PMEReturnCode
        Dim sDocumentRef As String = ""
        Dim lInsuranceFileCnt As Integer
        Dim vDeletedIndex As Object
        Dim lCount As Integer
        Dim cThisRevision, cThisPayment As Decimal
        Dim sOriginalReserveID As String = ""
        Dim vResultArray(,) As Object
        Dim sTransactionAmount, sTransactionType As String

        On Error GoTo Catch_Renamed
        lCount = 0

        For Each oListItem As ListViewItem In lvwClaim.Items
            If oListItem.Checked Then
                lCount += 1
                sOriginalReserveID = "0"
                sTransactionAmount = "0"
                sMessage = ""

                sDocumentRef = oListItem.SubItems.Item(ACFieldFCDocRef - 1).Text.Trim()
                lInsuranceFileCnt = CInt(oListItem.SubItems.Item(ACFieldFCInsuranceFileCnt - 1).Text)

                stbMain.Items.Item("COUNT").Text = CStr(lCount)
                stbMain.Items.Item("MESSAGE").Text = "Check to see if document is already in account please wait"

                'check to see if this document_ref is in account

                m_lReturn = m_oBusiness.IsDocumentInAccount(v_sDocumentRef:=sDocumentRef, r_lStatus:=lInAccount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if document is in account", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedClaimTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed

                End If

                If lInAccount = gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Document : " & sDocumentRef & " Is Already In Account." & Strings.Chr(13) & Strings.Chr(10) & " Can't Repost", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    GoTo Finally_Renamed
                End If

                'give user the chance to change the amount to be repost
                If MessageBox.Show("Do you want to enter new transaction amount?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    sTransactionAmount = GetUserInput(v_sMessage:="Enter New Transaction Amount", v_lIsNumeric:=1)

                    If sTransactionAmount = "" Then
                        GoTo Finally_Renamed
                    End If
                End If

                cThisRevision = 0
                cThisPayment = 0

                If MessageBox.Show("Do you want to use selected reserve_id from Reserve/Payment tab?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    If lvwReserve.Items.Count > 0 Then
                        sOriginalReserveID = lvwReserve.Items.Item(lvwReserve.FocusedItem.Index).Text
                    End If
                Else
                    sOriginalReserveID = oListItem.SubItems.Item(ACFieldFCOriginalReserveID - 1).Text
                End If

                If sDocumentRef.Substring(0, 3) = "CLP" Then
                    sTransactionType = "C_CP"

                    If sTransactionAmount <> "0" Then
                        cThisPayment = CDec(sTransactionAmount)
                    Else
                        cThisPayment = CDec(oListItem.SubItems.Item(ACFieldFCPremiumTotal - 1).Text)
                    End If
                Else
                    If sDocumentRef.Substring(0, 3) = "CLO" Then
                        sTransactionType = "C_CO"
                    Else
                        sTransactionType = "C_CR"
                    End If

                    If sTransactionAmount <> "0" Then
                        cThisRevision = CDec(sTransactionAmount)
                    Else
                        cThisRevision = CDec(oListItem.SubItems.Item(ACFieldFCPremiumTotal - 1).Text)
                    End If
                End If

                'if we don't have original reserve id then give user the option to enter it
                Dim dbNumericTemp As Double
                If sOriginalReserveID = "0" Or Not Double.TryParse(sOriginalReserveID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    sOriginalReserveID = GetUserInput(v_sMessage:="Enter Original ReserveID", v_lIsNumeric:=1)

                    If sOriginalReserveID = "" Then
                        GoTo Finally_Renamed
                    End If
                End If

                'only try to repost if we have valid original reserve id
                If CInt(sOriginalReserveID) > 0 Then

                    m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="Reserve", v_vReturnColumn:="reserve_id", v_sKeyColumn:="reserve_id", v_sKeyValue:=sOriginalReserveID, v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)

                    'exit loop if we can't find it or failed
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            MessageBox.Show("OriginalReserveID = " & sOriginalReserveID & " does not exist", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Failed to check OriginalReserveID", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        GoTo Finally_Renamed
                    End If

                    stbMain.Items.Item("MESSAGE").Text = "Processing claim please wait"


                    If m_oBusiness.ReprocessClaim(r_sMessage:=sMessage, v_sClaimNumber:=oListItem.Text, v_lClaimID:=CInt(oListItem.SubItems.Item(ACFieldFCClaimID - 1).Text), v_cThisRevision:=cThisRevision, v_cThisPayment:=cThisPayment, v_lOriginalReserveID:=CInt(sOriginalReserveID), v_lPaymentID:=CInt(oListItem.SubItems.Item(ACFieldFCPaymentID - 1).Text), v_sTransactionType:=sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                        GoTo Finally_Renamed
                    End If

                    If chkDeleteStats.CheckState = CheckState.Checked Then
                        'delete the original failed document ref now that we've reposted with new one

                        If m_oBusiness.DeleteStatsFolder(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sDocumentRef:=sDocumentRef) <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete stats details for InsuranceFileCnt = " & lInsuranceFileCnt & "and DocumentRef = " & sDocumentRef, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedClaimTransaction", vErrNo:=CStr(0), vErrDesc:="")
                        End If
                    End If

                    If Not Information.IsArray(vDeletedIndex) Then

                        ReDim vDeletedIndex(0)
                    Else
                        'stored index into an arrray so we can delete it from the list

                        ReDim Preserve vDeletedIndex(vDeletedIndex.GetUpperBound(0) + 1)
                    End If



                    vDeletedIndex(vDeletedIndex.GetUpperBound(0)) = oListItem.Index + 1
                End If
            End If
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to repost failed claim transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedClaimTransaction()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If sMessage <> "" Then
            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        If Information.IsArray(vDeletedIndex) Then
            'delete from list view - do it this way because index changes as we delete from listview

            For lCount = vDeletedIndex.GetUpperBound(0) To 0 Step -1
                'delete from list view

                lvwClaim.Items.RemoveAt(CInt(vDeletedIndex(lCount)) - 1)
            Next

            Me.lvwClaim.Refresh()
        End If

        If lvwClaim.Items.Count > 0 Then
            stbMain.Items.Item("COUNT").Text = CStr(lvwClaim.Items.Count)
        End If

        stbMain.Items.Item("MESSAGE").Text = "Ready"

        Exit Sub
    End Sub

    '*************************************************************************************************************
    ' deleted selected records from listview
    '*************************************************************************************************************
    Private Sub DeleteSelected(ByRef r_oListView As ListView)


        On Error GoTo Catch_Renamed

        For lCount As Integer = r_oListView.Items.Count To 1 Step -1
            If r_oListView.Items.Item(lCount - 1).Checked Then
                r_oListView.Items.RemoveAt(lCount - 1)
            End If
        Next

        If r_oListView.Items.Count > 0 Then
            stbMain.Items.Item("COUNT").Text = CStr(r_oListView.Items.Count)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to deleted selected records from list", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSelected()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Exit Sub
    End Sub

    '***********************************************************************************************************
    'build popup menu for list view
    '***********************************************************************************************************
    Private Sub ListViewPopUpMenu(Optional ByVal v_vExtraMenu() As Object = Nothing)


        Try

            'refresh list
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Refresh List"

            'separator
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "-"

            'Search
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Search List"

            'separator
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "-"

            'select high lighted
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Select Highlighted"

            'select all
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Select All"

            'unselect all
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "UnSelect All"

            'separator
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "-"

            'delete selected
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Delete Selected"

            'add extra menus if we have them
            If Information.IsArray(v_vExtraMenu) Then
                For Each v_vExtraMenu_item As Object In v_vExtraMenu
                    'separator
                    ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
                    mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "-"

                    ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)

                    mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = CStr(v_vExtraMenu_item)
                Next v_vExtraMenu_item
            End If

            'separator
            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "-"

            ContainerHelper.LoadControl(Me, "mnuPopUpItem", mnuPopUpItem.GetUpperBound(0) + 1)
            mnuPopUpItem(mnuPopUpItem.GetUpperBound(0)).Text = "Copy Listview Column's Value"

            'hide temporary menu
            mnuPopUpItem(0).Available = False

            Ctx_mnuPopUp.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

            'unhide temporary menu
            mnuPopUpItem(0).Available = True

            'unload all added menus
            For lCount As Integer = 1 To mnuPopUpItem.GetUpperBound(0)
                ContainerHelper.UnloadControl(Me, "mnuPopUpItem", lCount)
            Next

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to build listview popup menu", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewPopUpMenu()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)


        End Try

    End Sub

    '***********************************************************************************************************
    ' populate imbalanced closed claims ie those which do not sum to zero in stats_detail
    '***********************************************************************************************************
    Private Sub PopulateImbalanceClosedClaim()

        Dim oListItem As ListViewItem
        Dim sClaimNumber As String = ""
        Dim vResultArray(,) As Object
        Dim lMax As Integer
        Dim lRowColour As Color

        On Error GoTo Catch_Renamed
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        lMax = -1

        sClaimNumber = GetUserInput(v_sMessage:="Enter Claim Number", v_sDefault:="Get All")

        If sClaimNumber = "" Then
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Getting imbalance closed claim's details please wait"


        If m_oBusiness.GetImbalanceClosedClaim(r_vResultArray:=vResultArray, v_sClaimNumber:=IIf(sClaimNumber = "Get All", "", sClaimNumber)) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get imbalance closed claims", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Displaying imbalance closed claims please wait"

        If Information.IsArray(vResultArray) Then

            lMax = vResultArray.GetUpperBound(1)
        End If

        lvwImbalancedClosedClaim.Items.Clear()
        For lCount As Integer = 0 To lMax
            'claim_id

            oListItem = lvwImbalancedClosedClaim.Items.Add(CStr(vResultArray(ACFieldICCClaimID, lCount)).Trim())

            'claim number


            'Modified by Sumeet Singh on 5/25/2010 1:08:34 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCClaimNumber,  , vResultArray(ACFieldICCClaimNumber, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCClaimNumber, vResultArray(ACFieldICCClaimNumber, lCount).Trim())

            'clo


            'Modified by Sumeet Singh on 5/25/2010 1:09:31 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCCLO,  , vResultArray(ACFieldICCCLO, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCCLO, vResultArray(ACFieldICCCLO, lCount).Trim())

            'initial reserve


            'Modified by Sumeet Singh on 5/25/2010 1:09:45 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCInitReserve,  , vResultArray(ACFieldICCInitReserve, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCInitReserve, vResultArray(ACFieldICCInitReserve, lCount).Trim())

            'cla


            'Modified by Sumeet Singh on 5/25/2010 1:09:59 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCCLA,  , vResultArray(ACFieldICCCLA, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCCLA, vResultArray(ACFieldICCCLA, lCount).Trim())

            'revised reserve


            'Modified by Sumeet Singh on 5/25/2010 1:10:16 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCRevisedRes,  , vResultArray(ACFieldICCRevisedRes, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCRevisedRes, vResultArray(ACFieldICCRevisedRes, lCount).Trim())

            'clp

            'Modified by Sumeet Singh on 5/25/2010 1:10:31 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCCLP,  , vResultArray(ACFieldICCCLP, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCCLP, vResultArray(ACFieldICCCLP, lCount).Trim())

            'paid to date

            'Modified by Sumeet Singh on 5/25/2010 1:10:43 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCPaidToDate,  , vResultArray(ACFieldICCPaidToDate, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCPaidToDate, vResultArray(ACFieldICCPaidToDate, lCount).Trim())

            'payment table

            'Modified by Sumeet Singh on 5/25/2010 1:10:56 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCPaymentTable,  , vResultArray(ACFieldICCPaymentTable, lCount).Trim())
            oListItem.SubItems.Insert(ACFieldICCPaymentTable, vResultArray(ACFieldICCPaymentTable, lCount).Trim())

            'total stats

            'Modified by Sumeet Singh on 5/25/2010 1:11:08 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCTotalStats,  , CStr(CDec(vResultArray(ACFieldICCCLO, lCount)) + CDec(vResultArray(ACFieldICCCLA, lCount)) - CDec(vResultArray(ACFieldICCCLP, lCount))))
            oListItem.SubItems.Insert(ACFieldICCTotalStats, CObj(CDec(vResultArray(ACFieldICCCLO, lCount)) + CDec(vResultArray(ACFieldICCCLA, lCount)) - CDec(vResultArray(ACFieldICCCLP, lCount))))

            'total reserve

            'Modified by Sumeet Singh on 5/25/2010 1:16:29 PM refer developer guide no. 215
            'oListItem.SubItems.Add(ACFieldICCTotalClaim,  , CStr(CDec(vResultArray(ACFieldICCInitReserve, lCount)) + CDec(vResultArray(ACFieldICCRevisedRes, lCount)) - CDec(vResultArray(ACFieldICCPaidToDate, lCount))))
            oListItem.SubItems.Insert(ACFieldICCTotalClaim, CObj(CDec(vResultArray(ACFieldICCInitReserve, lCount)) + CDec(vResultArray(ACFieldICCRevisedRes, lCount)) - CDec(vResultArray(ACFieldICCPaidToDate, lCount))))

            lRowColour = Color.Black
            'reserves are wrong




            If CDec(vResultArray(ACFieldICCInitReserve, lCount)) <> CDec(vResultArray(ACFieldICCCLO, lCount)) Or CDec(vResultArray(ACFieldICCRevisedRes, lCount)) <> CDec(vResultArray(ACFieldICCCLA, lCount)) Then
                lRowColour = Color.Lime
            End If

            'payments are wrong


            If CDec(vResultArray(ACFieldICCCLP, lCount)) <> CDec(vResultArray(ACFieldICCPaidToDate, lCount)) Then
                lRowColour = Color.Cyan
            End If

            'both stats and reserves are imbalance
            If CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text) <> 0 And CDec(oListItem.SubItems.Item(ACFieldICCTotalClaim - 1).Text) <> 0 Then
                lRowColour = Color.Red
            End If

            'both reserve and payment are wrong
            If (CDec(oListItem.SubItems.Item(ACFieldICCInitReserve - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLO - 1).Text) Or CDec(oListItem.SubItems.Item(ACFieldICCRevisedRes - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLA - 1).Text)) And CDec(oListItem.SubItems.Item(ACFieldICCPaidToDate - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLP - 1).Text) Then
                lRowColour = Color.Magenta
            End If

            If Not lRowColour.Equals(Color.Black) Then
                oListItem.ForeColor = lRowColour
                For lCol As Integer = 1 To oListItem.SubItems.Count
                    oListItem.SubItems.Item(lCol - 1).ForeColor = lRowColour
                Next lCol
            End If

        Next lCount

        GoTo Finally_Renamed

Catch_Renamed:
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate imbalance closed claims", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateImbalanceClosedClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
        stbMain.Items.Item("COUNT").Text = CStr(lvwImbalancedClosedClaim.Items.Count)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Exit Sub

        Resume Next
    End Sub

    '***********************************************************************************************************
    ' repost an amount either as payment or adjustment. this is normally done to make the stats info balance
    ' Note: reserve and payment table won't be modified, only stats info will
    '***********************************************************************************************************
    Private Sub RePostClaim()

        Dim vResultArray(,) As Object
        Dim sAmount, sOrigReserveID, sPaymentID, sMessage As String
        Dim cThisRevision, cThisPayment As Decimal
        Dim vDeletedIndex As Object
        Dim lCount, lRepostCount As Integer
        Dim sClaimNumber As String = ""
        Dim bContinue As Boolean
        Dim sReport As New StringBuilder
        Dim bIsReserve As Boolean

        On Error GoTo Catch_Renamed
        lCount = 0
        lRepostCount = 0

        For Each oListItem As ListViewItem In lvwImbalancedClosedClaim.Items
            If oListItem.Checked Then
                bIsReserve = True
                bContinue = True
                sPaymentID = "0"
                sOrigReserveID = "0"
                cThisRevision = 0
                cThisPayment = 0

                lCount += 1

                sClaimNumber = oListItem.SubItems.Item(ACFieldICCClaimNumber - 1).Text.Trim()

                stbMain.Items.Item("MESSAGE").Text = "Processing please wait"
                stbMain.Items.Item("COUNT").Text = CStr(lRepostCount) & "/" & CStr(lCount)

                If chkAutoProcess.CheckState = CheckState.Checked Then
                    'if both reserve and payment are imbalance or both stats and reserve are imbalance then we can't do it automatically
                    If ((CDec(oListItem.SubItems.Item(ACFieldICCInitReserve - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLO - 1).Text) Or CDec(oListItem.SubItems.Item(ACFieldICCRevisedRes - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLA - 1).Text)) And CDec(oListItem.SubItems.Item(ACFieldICCPaidToDate - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLP - 1).Text)) Or (CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text) <> 0 And CDec(oListItem.SubItems.Item(ACFieldICCTotalClaim - 1).Text) <> 0) Then

                        sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "both reserve and payment are imbalance and or both stats and reserve are imbalance - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                        bContinue = False
                    End If
                End If

                If bContinue Then

                    'are we processing automatically?
                    If chkAutoProcess.CheckState = CheckState.Unchecked Then

                        bIsReserve = (MessageBox.Show("Is it an adjustment (CLA)?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes)

                        If bIsReserve Then
                            'default amount to outstanding in stats ie total (reserve-payment)
                            sAmount = "" & CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text) * -1
                        Else
                            'default amount to outstanding in stats ie total (reserve-payment)
                            sAmount = "" & CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text)
                        End If

                        'get transaction amount
                        sAmount = GetUserInput(v_sMessage:="Enter Amount To Repost", v_lIsNumeric:=1, v_sDefault:=sAmount)

                        If CDec(sAmount) = 0 Then
                            GoTo Finally_Renamed
                        End If

                        'default original reserve id to selected row on the reserve listview
                        If txtClaimNumber.Text.Trim() = sClaimNumber Then
                            If lvwReserve.Items.Count > 0 Then
                                sOrigReserveID = lvwReserve.Items.Item(lvwReserve.FocusedItem.Index).Text
                            End If
                        End If

                        'we need this to know which reserve we are adjusting
                        sOrigReserveID = GetUserInput(v_sMessage:="Enter Original ReserveID", v_lIsNumeric:=1, v_sDefault:=sOrigReserveID)

                        If CInt(sOrigReserveID) = 0 Then
                            GoTo Finally_Renamed
                        End If
                    Else
                        'is it reserve thats not balance or payment?
                        If CDec(oListItem.SubItems.Item(ACFieldICCInitReserve - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLO - 1).Text) Or CDec(oListItem.SubItems.Item(ACFieldICCRevisedRes - 1).Text) <> CDec(oListItem.SubItems.Item(ACFieldICCCLA - 1).Text) Then

                            bIsReserve = True

                            'default amount to outstanding in stats ie total (reserve-payment)
                            sAmount = "" & CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text) * -1
                        Else
                            bIsReserve = False
                            'default amount to outstanding in stats ie total (reserve-payment)
                            sAmount = "" & CDec(oListItem.SubItems.Item(ACFieldICCTotalStats - 1).Text)
                        End If

                        'check to see if we have more than one perils

                        If m_oBusiness.GetClaimPeril(v_lClaimID:=CInt(oListItem.Text), r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                            'MsgBox "Failed to get claim perils for claim number " & sClaimNumber, vbInformation + vbOKOnly, "Repost Transaction"
                            sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Failed to get claim perils for claim number " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                            bContinue = False
                        End If

                        If bContinue Then
                            If Information.IsArray(vResultArray) Then

                                If vResultArray.GetUpperBound(1) > 0 Then
                                    sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "This claim is on multi perils - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                    bContinue = False 'can't process this one automatically as we don't know which peril to set the transaction to
                                End If
                            End If
                        End If

                        If bContinue Then
                            'check to see if we have more than one reserves set

                            If m_oBusiness.GetReserveDetail(v_sClaimNumber:=sClaimNumber, r_vResultArray:=vResultArray, v_lNoneZeroReserve:=1) <> gPMConstants.PMEReturnCode.PMTrue Then
                                'MsgBox "Failed to get none zero reserve details for claim number " & sClaimNumber, vbOKOnly + vbInformation, "Repost Transaction"
                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Failed to get none zero reserve details for claim number " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                bContinue = False
                            End If

                            If bContinue Then
                                If Information.IsArray(vResultArray) Then

                                    If vResultArray.GetUpperBound(1) > 0 Then
                                        sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "This claim has multi reserve types - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                        bContinue = False 'can't process this one automatically as we don't know which reserve to set the transaction to
                                    Else

                                        sOrigReserveID = CStr(vResultArray(ACFieldRReserveID, 0))
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If bContinue Then
                        'only try to repost if we have valid original reserve id
                        If CInt(sOrigReserveID) > 0 Then

                            m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="Reserve", v_vReturnColumn:="reserve_id", v_sKeyColumn:="reserve_id", v_sKeyValue:=sOrigReserveID, v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)

                            'exit loop if we can't find it or failed
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If chkAutoProcess.CheckState = CheckState.Checked Then
                                    sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Original reserve_id " & sOrigReserveID & " does not exist. - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                    bContinue = False
                                Else
                                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                        MessageBox.Show("OriginalReserveID = " & sOrigReserveID & " does not exist", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        MessageBox.Show("Failed to check OriginalReserveID", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If

                                    GoTo Finally_Renamed
                                End If
                            End If
                        Else
                            If chkAutoProcess.CheckState = CheckState.Checked Then
                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Original reserve_id is zero - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                bContinue = False
                            Else
                                GoTo Finally_Renamed
                            End If
                        End If

                        If bContinue Then
                            'is it reserve thats not balance or payment?
                            If bIsReserve Then
                                cThisRevision = CDec(sAmount) 'its a reserve thats wrong
                            Else
                                cThisPayment = CDec(sAmount)

                                'are we processing automatically?
                                If chkAutoProcess.CheckState = CheckState.Unchecked Then
                                    'default payment_id to the row selected on payment listview
                                    If txtClaimNumber.Text.Trim() = sClaimNumber Then
                                        If lvwPayment.Items.Count > 0 Then
                                            sPaymentID = lvwPayment.Items.Item(lvwPayment.FocusedItem.Index).Text
                                        End If
                                    End If

                                    'we need this to work out who we are paying to
                                    sPaymentID = GetUserInput(v_sMessage:="Enter Original PaymentID", v_lIsNumeric:=1, v_sDefault:=sPaymentID)

                                    If CInt(sPaymentID) = 0 Then
                                        GoTo Finally_Renamed
                                    End If
                                Else

                                    If m_oBusiness.GetPaymentDetail(v_sClaimNumber:=sClaimNumber, r_vResultArray:=vResultArray, v_lUniquePaymentPartyCode:=1) <> gPMConstants.PMEReturnCode.PMTrue Then
                                        'MsgBox "Failed to get unique payment party code for claim number " & sClaimNumber, vbOKOnly + vbInformation, "Repost Transaction"
                                        sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Failed to get unique payment party code for claim number " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                        bContinue = False
                                    End If

                                    If bContinue Then
                                        If Information.IsArray(vResultArray) Then

                                            If vResultArray.GetUpperBound(1) > 0 Then
                                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Claim has multi payment party code - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                                bContinue = False 'can't process this automatically as we don't know which payment party code to post to
                                            Else

                                                sPaymentID = CStr(vResultArray(ACFieldPPaymentID, 0))
                                            End If
                                        End If
                                    End If
                                End If

                                If bContinue Then
                                    If CInt(sPaymentID) > 0 Then

                                        m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="Payment", v_vReturnColumn:="payment_id", v_sKeyColumn:="payment_id", v_sKeyValue:=sPaymentID, v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)

                                        'exit loop if we can't find it or failed
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            If chkAutoProcess.CheckState = CheckState.Checked Then
                                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "PaymentID " & sPaymentID & " does not exist - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                                bContinue = False
                                            Else
                                                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                                    MessageBox.Show("OriginalPaymentID = " & sPaymentID & " does not exist", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                Else
                                                    MessageBox.Show("Failed to check OriginalPaymentID", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                End If

                                                GoTo Finally_Renamed
                                            End If
                                        End If
                                    Else
                                        If chkAutoProcess.CheckState = CheckState.Checked Then
                                            sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "PaymentID is zero - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                            bContinue = False
                                        Else
                                            GoTo Finally_Renamed
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If bContinue Then
                            stbMain.Items.Item("MESSAGE").Text = "Reposting claim " & sClaimNumber & " please wait"

                            If m_oBusiness.ReprocessClaim(r_sMessage:=sMessage, v_sClaimNumber:=sClaimNumber, v_lClaimID:=CInt(oListItem.Text), v_cThisRevision:=cThisRevision, v_cThisPayment:=cThisPayment, v_lOriginalReserveID:=CInt(sOrigReserveID), v_lPaymentID:=CInt(sPaymentID)) <> gPMConstants.PMEReturnCode.PMTrue Then

                                If chkAutoProcess.CheckState = CheckState.Unchecked Then
                                    MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If

                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & sMessage & " - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                                bContinue = False
                            End If

                            If bContinue Then
                                If Not Information.IsArray(vDeletedIndex) Then

                                    ReDim vDeletedIndex(0)
                                Else
                                    'stored index into an arrray so we can delete it from the list

                                    ReDim Preserve vDeletedIndex(vDeletedIndex.GetUpperBound(0) + 1)
                                End If



                                vDeletedIndex(vDeletedIndex.GetUpperBound(0)) = oListItem.Index + 1

                                lRepostCount += 1
                                sReport.Append(Strings.Chr(13) & Strings.Chr(10) & "Successful - " & sClaimNumber & Strings.Chr(13) & Strings.Chr(10))
                            End If
                        End If
                    End If
                End If 'is both stats and reserve imbalance?
            End If 'is row checked
        Next oListItem

        GoTo Finally_Renamed

Catch_Renamed:
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to repost claim", vApp:=ACApp, vClass:=ACClass, vMethod:="RePostClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If chkAutoProcess.CheckState = CheckState.Checked Then
            If sReport.ToString() <> "" Then
                stbMain.Items.Item("MESSAGE").Text = "Spooling report please wait"
                m_lReturn = RepostTransactionReport(v_sReportTitle:="**************** RepostClaim **************", v_sReportText:=sReport.ToString())
            End If
        End If

        If Information.IsArray(vDeletedIndex) Then
            'delete from list view - do it this way because index changes as we delete from listview

            For lCount = vDeletedIndex.GetUpperBound(0) To 0 Step -1
                'delete from list view

                lvwImbalancedClosedClaim.Items.RemoveAt(CInt(vDeletedIndex(lCount)) - 1)
            Next

            Me.lvwImbalancedClosedClaim.Refresh()
        End If

        If lRepostCount > 0 Then
            MessageBox.Show("Reposted " & lRepostCount, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        stbMain.Items.Item("COUNT").Text = CStr(lvwImbalancedClosedClaim.Items.Count)
        stbMain.Items.Item("MESSAGE").Text = "Ready"

        Exit Sub
    End Sub

    '***********************************************************************************************************
    'populate reserve and payment details for selected claim
    '***********************************************************************************************************
    Private Sub PopulateReservePaymentDetail()

        Dim vReserve, vPayment(,) As Object
        Dim sClaimNumber As String = ""
        Dim oListItem As ListViewItem

        On Error GoTo Catch_Renamed
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        sClaimNumber = txtClaimNumber.Text.Trim()

        If sClaimNumber = "" Then
            MessageBox.Show("Please provide claim number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtClaimNumber.Focus()
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Getting reserve details please wait"
        'get reserve details

        If m_oBusiness.GetReserveDetail(v_sClaimNumber:=sClaimNumber, r_vResultArray:=vReserve) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get reserve details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Getting payment details please wait"
        'get payment details

        If m_oBusiness.GetPaymentDetail(v_sClaimNumber:=sClaimNumber, r_vResultArray:=vPayment) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get payment details", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Displaying reserve details please wait"
        'display reserve details
        lvwReserve.Items.Clear()
        If Information.IsArray(vReserve) Then

            For lCount As Integer = 0 To vReserve.GetUpperBound(1)
                'reserve id

                oListItem = lvwReserve.Items.Add(CStr(vReserve(ACFieldRReserveID, lCount)).Trim())

                'claim peril id


                'Modified by Sumeet Singh on 5/25/2010 1:17:18 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRClaimPerilID,  , vReserve(ACFieldRClaimPerilID, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRClaimPerilID, vReserve(ACFieldRClaimPerilID, lCount).Trim())

                'reserve type id

                'Modified by Sumeet Singh on 5/25/2010 1:17:59 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRReserveTypeID,  , vReserve(ACFieldRReserveTypeID, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRReserveTypeID, vReserve(ACFieldRReserveTypeID, lCount).Trim())

                'initial reserve

                'Modified by Sumeet Singh on 5/25/2010 1:18:52 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRInitReserve,  , vReserve(ACFieldRInitReserve, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRInitReserve, vReserve(ACFieldRInitReserve, lCount).Trim())

                'revised reserve

                'Modified by Sumeet Singh on 5/25/2010 1:19:07 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRRevisedReserve,  , vReserve(ACFieldRRevisedReserve, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRRevisedReserve, vReserve(ACFieldRRevisedReserve, lCount).Trim())

                'paid to date

                'Modified by Sumeet Singh on 5/25/2010 1:19:21 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRPaidToDate,  , vReserve(ACFieldRPaidToDate, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRPaidToDate, vReserve(ACFieldRPaidToDate, lCount).Trim())

                'this revision

                'Modified by Sumeet Singh on 5/25/2010 1:19:35 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRThisRevision,  , vReserve(ACFieldRThisRevision, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRThisRevision, vReserve(ACFieldRThisRevision, lCount).Trim())

                'this payment

                'Modified by Sumeet Singh on 5/25/2010 1:19:54 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRThisPayment,  , vReserve(ACFieldRThisPayment, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRThisPayment, vReserve(ACFieldRThisPayment, lCount).Trim())

                'reserve type

                'Modified by Sumeet Singh on 5/25/2010 1:20:08 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRReserveType,  , vReserve(ACFieldRReserveType, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRReserveType, vReserve(ACFieldRReserveType, lCount).Trim())

                'claim peril

                'Modified by Sumeet Singh on 5/25/2010 1:20:21 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldRClaimPeril,  , vReserve(ACFieldRClaimPeril, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldRClaimPeril, vReserve(ACFieldRClaimPeril, lCount).Trim())

            Next lCount

            lvwReserve.Items.Item(0).Selected = True
        End If

        stbMain.Items.Item("MESSAGE").Text = "Displaying payment details please wait"
        'display payment details
        lvwPayment.Items.Clear()
        If Information.IsArray(vPayment) Then

            For lCount As Integer = 0 To vPayment.GetUpperBound(1)
                'payment id

                oListItem = lvwPayment.Items.Add(CStr(vPayment(ACFieldPPaymentID, lCount)).Trim())

                'reserve id


                'Modified by Sumeet Singh on 5/25/2010 1:21:16 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldPReserveID,  , vPayment(ACFieldPReserveID, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldPReserveID, vPayment(ACFieldPReserveID, lCount).Trim())

                'claim peril id

                'Modified by Sumeet Singh on 5/25/2010 1:21:29 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldPClaimPerilID,  , vPayment(ACFieldPClaimPerilID, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldPClaimPerilID, vPayment(ACFieldPClaimPerilID, lCount).Trim())

                'amount

                'Modified by Sumeet Singh on 5/25/2010 1:21:42 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldPAmount,  , vPayment(ACFieldPAmount, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldPAmount, vPayment(ACFieldPAmount, lCount).Trim())

                'date of payment

                'Modified by Sumeet Singh on 5/25/2010 1:21:55 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldPPaymentDate,  , vPayment(ACFieldPPaymentDate, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldPPaymentDate, vPayment(ACFieldPPaymentDate, lCount).Trim())

                'payment party code

                'Modified by Sumeet Singh on 5/25/2010 1:22:09 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldPPartyCode,  , vPayment(ACFieldPPartyCode, lCount).Trim())
                oListItem.SubItems.Insert(ACFieldPPartyCode, vPayment(ACFieldPPartyCode, lCount).Trim())

            Next lCount

            lvwPayment.Items.Item(0).Selected = True
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate reserve and payment details", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateReservePaymentDetail()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        stbMain.Items.Item("COUNT").Text = CStr(lvwReserve.Items.Count) & " - " & CStr(lvwPayment.Items.Count)
        stbMain.Items.Item("MESSAGE").Text = "Ready"

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Exit Sub
    End Sub

    '*******************************************************************************************************
    'Desc: produce a text file and spool it
    '*******************************************************************************************************
    Private Function RepostTransactionReport(ByVal v_sReportTitle As String, ByVal v_sReportText As String, Optional ByVal v_sFileName As String = "", Optional ByVal v_sPath As String = "", Optional ByVal v_bDeleteFile As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        'get path from registry if its not passed in
        If v_sPath = "" Then
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=v_sPath)
        End If

        'make sure we have a backslash at the end
        If Not v_sPath.EndsWith("\") Then
            v_sPath = v_sPath & "\"
        End If

        If v_sFileName = "" Then
            v_sFileName = "RepostTransaction_" & g_oObjectManager.UserName & "_" & DateTime.Now.ToString("yyyyMMddHHMMss") & ".log"
        End If

        If AppendText(v_sFile:=v_sPath & v_sFileName, v_sTextLine:=v_sReportTitle & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & v_sReportText) <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If

        If SpoolDoc(v_sFileName:=v_sPath & v_sFileName, v_sSpoolDesc:=v_sReportTitle) <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If

        'delete original file
        If v_bDeleteFile Then
            File.Delete(v_sPath & v_sFileName)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool renewal report", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostTransactionReport()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        GoTo Finally_Renamed

Finally_Renamed:

        Return result
    End Function

    '*******************************************************************************************************
    'Desc: write to text file depend on mode.
    '*******************************************************************************************************
    Private Function AppendText(ByVal v_sFile As String, ByVal v_sTextLine As String, Optional ByVal v_sMode As String = "Output") As Integer

        Dim result As Integer = 0
        Dim lFileNo As Integer

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        'get free handle
        lFileNo = FileSystem.FreeFile()

        Select Case v_sMode.ToUpper()
            Case "APPEND"
                FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Append)
            Case "BINARY"
                FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Binary)
            Case "INPUT"
                FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Input)
            Case "OUTPUT"
                FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Output)
            Case "RANDOM"
                FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Random)
        End Select

        FileSystem.PrintLine(lFileNo, v_sTextLine)

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write to " & v_sFile, vApp:=ACApp, vClass:=ACClass, vMethod:="AppendText()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
Finally_Renamed:

        FileSystem.FileClose(lFileNo)

        Return result

    End Function

    '*****************************************************************************************
    ' Desc : send document to document spooler (just a text file not a normal merge doc)
    '*****************************************************************************************
    Private Function SpoolDoc(ByVal v_sFileName As String, ByVal v_sSpoolDesc As String) As Integer

        Dim result As Integer = 0
        Dim sDocTypeID As String = ""

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oDocTemplate Is Nothing Then
            Dim temp_m_oDocTemplate As Object
            If g_oObjectManager.GetInstance(temp_m_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDocTemplate = temp_m_oDocTemplate
                MessageBox.Show("Failed to create an instance of iPMBDocTemplate.Interface", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                GoTo Finally_Renamed
            Else
                m_oDocTemplate = temp_m_oDocTemplate
            End If
        End If


        m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="Document_Type", v_vReturnColumn:="document_type_id", v_sKeyColumn:="Code", v_sKeyValue:="REPORT", v_lDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=sDocTypeID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If


        m_oDocTemplate.DocName = v_sFileName

        m_oDocTemplate.SpoolDesc = v_sSpoolDesc

        m_oDocTemplate.DocumentTypeId = CInt(sDocTypeID)

        m_oDocTemplate.Mode = 5 'spool report


        result = m_oDocTemplate.Start()

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    Private Sub tabPolicyVersion_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabPolicyVersion.SelectedIndexChanged

        Dim sInsuranceFileCnt As String = ""

        'If SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 0 Then
        cmdOK.Enabled = (SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 0)
        CmdSelectAllPolicy.Visible = (SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 0)
        'ElseIf SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 5 Then
        '    cmdOK.Enabled = (SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 5)
        '    CmdSelectAllPolicy.Visible = (SSTabHelper.GetSelectedIndex(tabPolicyVersion) = 5)
        'End If
        If lvwPolicyVersion.Items.Count > 0 Then
            Try
                Dim i As Integer
                For i = 0 To lvwPolicyVersion.Items.Count - 1
                    If lvwPolicyVersion.Items(i).Checked Then
                        sInsuranceFileCnt = lvwPolicyVersion.Items.Item(i).Text.Trim()
                        Exit For
                    End If
                Next
                If sInsuranceFileCnt = "" Then
                    sInsuranceFileCnt = 0
                End If
                'sInsuranceFileCnt = lvwPolicyVersion.Items.Item(lvwPolicyVersion.FocusedItem.Index).Text.Trim()
            Catch ex As Exception
                sInsuranceFileCnt = lvwPolicyVersion.Items.Item(0).Text.Trim()
            End Try
        End If

        Select Case SSTabHelper.GetSelectedIndex(tabPolicyVersion)
            Case 0 'policy version
                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)

            Case 1 'display all risk details for this policy verion if user clicked on risk tab


                If txtInsuranceFileCnt.Text <> sInsuranceFileCnt And sInsuranceFileCnt <> "" Then
                    txtInsuranceFileCnt.Text = sInsuranceFileCnt
                    PopulateRiskDetail()
                End If

                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwRisk.Items.Count)

            Case 2 'transaction export

                If txtTransactionExportPolicyID.Text <> sInsuranceFileCnt Then
                    txtTransactionExportPolicyID.Text = sInsuranceFileCnt
                    PopulateTransactionExport()
                End If

                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwTransactionExport.Items.Count)

        End Select

        tabPolicyVersionPreviousTab = tabPolicyVersion.SelectedIndex
    End Sub

    '*****************************************************************************************
    ' get and display risk details for policy id
    '*****************************************************************************************
    Private Sub PopulateRiskDetail()

        Dim oListItem As ListViewItem
        Dim aoResultArray(,) As Object

        Try
            If m_oBusiness.GetRiskDetail(v_lInsuranceFileCnt:=CInt(txtInsuranceFileCnt.Text), r_vResultArray:=aoResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get risk details for policy id = " & txtInsuranceFileCnt.Text, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            lvwRisk.Items.Clear()

            If Information.IsArray(aoResultArray) Then
                For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)
                    oListItem = lvwRisk.Items.Add(CStr(aoResultArray(ACFieldRDInsuranceFileCnt, lCount)).Trim())
                    oListItem.SubItems.Insert(ACFieldRDRiskCnt, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRDRiskCnt, lCount).Trim()))
                    oListItem.SubItems.Insert(ACFieldRDStatusFlag, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRDStatusFlag, lCount).Trim()))
                    oListItem.SubItems.Insert(ACFieldRDDesc, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRDDesc, lCount).Trim()))
                    oListItem.SubItems.Insert(ACFieldRDRiskStatus, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRDRiskStatus, lCount).Trim()))
                    oListItem.SubItems.Insert(ACFieldRDInsuranceFolderCnt, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRDInsuranceFolderCnt, lCount).Trim()))
                Next

                If lvwRisk.Items.Count > 0 Then
                    lvwRisk.Items.Item(0).Selected = True
                End If
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateRiskDetail()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

    End Sub

    '*****************************************************************************************
    ' get and display transaction export details for policy id
    '*****************************************************************************************
    Private Sub PopulateTransactionExport()

        Dim oListItem As ListViewItem
        Dim aoResultArray(,) As Object
        Dim sPostingStatus As String = ""

        Try
            If m_oBusiness.GetTransactionExport(v_lInsuranceFileCnt:=CInt(txtTransactionExportPolicyID.Text), r_vResultArray:=aoResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get transaction export details for policy id = " & txtTransactionExportPolicyID.Text, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            lvwTransactionExport.Items.Clear()

            If Information.IsArray(aoResultArray) Then
                For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)
                    oListItem = lvwTransactionExport.Items.Add(CStr(aoResultArray(ACFieldTETransactionExportFolderCnt, lCount)).Trim())

                    'insurance file cnt - display this in case its differ from policy_id text box


                    'Modified by Sumeet Singh on 5/25/2010 1:24:02 PM refer developer guide no. 215
                    'oListItem.SubItems.Add(ACFieldTEInsuranceFileCnt,  , vResultArray(ACFieldTEInsuranceFileCnt, lCount).Trim())
                    oListItem.SubItems.Insert(ACFieldTEInsuranceFileCnt, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldTEInsuranceFileCnt, lCount).Trim()))

                    'insurance ref

                    'Modified by Sumeet Singh on 5/25/2010 1:24:16 PM refer developer guide no. 215
                    'oListItem.SubItems.Add(ACFieldTEInsuranceRef,  , vResultArray(ACFieldTEInsuranceRef, lCount).Trim())
                    oListItem.SubItems.Insert(ACFieldTEInsuranceRef, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldTEInsuranceRef, lCount).Trim()))

                    'document ref

                    'Modified by Sumeet Singh on 5/25/2010 1:24:31 PM refer developer guide no. 215
                    'oListItem.SubItems.Add(ACFieldTEDocumentRef,  , vResultArray(ACFieldTEDocumentRef, lCount).Trim())
                    oListItem.SubItems.Insert(ACFieldTEDocumentRef, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldTEDocumentRef, lCount).Trim()))

                    'document date

                    'Modified by Sumeet Singh on 5/25/2010 1:24:45 PM refer developer guide no. 215
                    'oListItem.SubItems.Add(ACFieldTEDocumentDate,  , vResultArray(ACFieldTEDocumentDate, lCount).Trim())
                    oListItem.SubItems.Insert(ACFieldTEDocumentDate, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldTEDocumentDate, lCount).Trim()))

                    Select Case CStr(aoResultArray(ACFieldTEAccountExportStatus, lCount)).Trim().ToUpper()
                        Case "P"
                            sPostingStatus = "Pending (p)"
                        Case "S"
                            sPostingStatus = "Sending (s)"
                        Case "F"
                            sPostingStatus = "Failed (f)"
                        Case "C"
                            sPostingStatus = "Completed"
                    End Select

                    oListItem.SubItems.Insert(ACFieldTEAccountExportStatus, New ListViewItem.ListViewSubItem(oListItem, CObj(sPostingStatus)))
                Next

                'highlight first row
                If lvwTransactionExport.Items.Count > 0 Then
                    lvwTransactionExport.Items.Item(0).Selected = True
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateTransactionExport()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End Try


    End Sub

    '*****************************************************************************
    ' add RI to policy at risk level
    ' Note : this will only work if RI model supplied is 100% retained
    '*****************************************************************************
    Private Sub AddRIToPolicy()

        Dim sRIModelCode, sInsuranceRef, sMessage As String

        On Error GoTo Catch_Renamed

        stbMain.Items.Item("MESSAGE").Text = "Adding RI to policy please wait"
        stbMain.Items.Item("COUNT").Text = ""

        sRIModelCode = GetUserInput(v_sMessage:="Enter RI Model Code", v_lIsNumeric:=0, v_sDefault:="RET")

        If sRIModelCode = "" Then
            GoTo Finally_Renamed
        End If

        sInsuranceRef = GetUserInput(v_sMessage:="Enter Policy Number", v_lIsNumeric:=0, v_sDefault:="ALL")

        If sInsuranceRef = "" Then
            GoTo Finally_Renamed
        End If


        m_lReturn = m_oBusiness.AddRIToPolicy(v_sRIModelCode:=sRIModelCode, v_sInsuranceRef:=IIf(sInsuranceRef.ToUpper() = "ALL", "", sInsuranceRef), r_sMessage:=sMessage)

        If sMessage <> "Successful" Then
            If sInsuranceRef.ToUpper() = "ALL" Then
                MessageBox.Show("Failed to add RI model (" & sRIModelCode & ") to policies with no reinsurance" & Strings.Chr(13) & Strings.Chr(10) & _
                                sMessage, Application.ProductName)
            Else
                MessageBox.Show("Failed to add RI model (" & sRIModelCode & ") to policy " & sInsuranceRef & Strings.Chr(13) & Strings.Chr(10) & _
                                sMessage, Application.ProductName)
            End If
        Else
            MessageBox.Show("Process completed successfully", Application.ProductName)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed AddRIToPolicy()", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRIToPolicy()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
        Exit Sub
    End Sub

    '*****************************************************************************
    ' Delete claim and all associated posting including stats
    '*****************************************************************************
    Private Sub DeleteClaim()

        Dim sClaimNumber As String = ""

        On Error GoTo Catch_Renamed

        stbMain.Items.Item("MESSAGE").Text = "Getting claim number"
        sClaimNumber = GetUserInput(v_sMessage:="Please enter claim number", v_sDefault:=sClaimNumber)

        If sClaimNumber = "" Then
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Deleting claim number " & sClaimNumber & " please wait"

        If m_oBusiness.DeleteClaim(v_sClaimNumber:=sClaimNumber, v_lClaimID:=0) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to delete claim", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete claim", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
        Exit Sub
    End Sub

    '*****************************************************************************
    ' populate claim postings
    '*****************************************************************************
    Private Sub PopulateClaimPosting()

        Dim sClaimNumber As String = ""
        Dim aoResultArray(,) As Object
        Dim oListItem As ListViewItem

        On Error GoTo Catch_Renamed
        sClaimNumber = txtCPClaimNumber.Text.Trim()

        If sClaimNumber = "" Then
            MessageBox.Show("Please enter claim number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtCPClaimNumber.Focus()
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Getting claim postings please wait"


        If m_oBusiness.GetClaimPosting(v_sClaimNumber:=sClaimNumber, r_vResultArray:=aoResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get claim postings", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        If Information.IsArray(aoResultArray) Then
            lvwClaimPosting.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                oListItem = lvwClaimPosting.Items.Add(CStr(aoResultArray(ACFieldCPPolicyNumber, lCount)))


                'Modified by Sumeet Singh on 5/25/2010 1:25:39 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPClaimNumber,  , CStr(vResultArray(ACFieldCPClaimNumber, lCount)))
                oListItem.SubItems.Insert(ACFieldCPClaimNumber, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPClaimNumber, lCount).Trim()))


                'Modified by Sumeet Singh on 5/25/2010 1:26:01 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPDocumentRef,  , CStr(vResultArray(ACFieldCPDocumentRef, lCount)))
                oListItem.SubItems.Insert(ACFieldCPDocumentRef, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPDocumentRef, lCount).Trim()))


                'Modified by Sumeet Singh on 5/25/2010 1:26:35 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPDocumentDate,  , DateTime.Parse(vResultArray(ACFieldCPDocumentDate, lCount)).ToString("D"))
                oListItem.SubItems.Insert(ACFieldCPDocumentDate, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPDocumentDate, lCount).Trim()))


                'Modified by Sumeet Singh on 5/25/2010 1:27:31 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPStatsDetailType,  , CStr(vResultArray(ACFieldCPStatsDetailType, lCount)))
                oListItem.SubItems.Insert(ACFieldCPStatsDetailType, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPStatsDetailType, lCount).Trim()))


                'Modified by Sumeet Singh on 5/25/2010 1:27:50 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPThisPremiumHome,  , CStr(vResultArray(ACFieldCPThisPremiumHome, lCount)))
                oListItem.SubItems.Insert(ACFieldCPThisPremiumHome, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPThisPremiumHome, lCount).Trim()))


                'Modified by Sumeet Singh on 5/25/2010 1:28:09 PM refer developer guide no. 215
                'oListItem.SubItems.Add(ACFieldCPPeriodID,  , CStr(vResultArray(ACFieldCPPeriodID, lCount)))
                oListItem.SubItems.Insert(ACFieldCPPeriodID, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldCPPeriodID, lCount).Trim()))
            Next lCount
        Else
            MessageBox.Show("No postings are found for claim number " & sClaimNumber, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate claim postings", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateClaimPosting()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("COUNT").Text = CStr(lvwClaimPosting.Items.Count)
        stbMain.Items.Item("MESSAGE").Text = "Ready"
        Exit Sub

    End Sub

    '*****************************************************************************
    ' add claim posting
    '*****************************************************************************
    Private Sub AddClaimPosting()

        Dim sReserveID, sPaymentID, sAmount, sMessage, sClaimNumber As String
        Dim cThisRevision, cThisPayment As Decimal
        Dim sTransactionType As String = ""

        On Error GoTo Catch_Renamed
        stbMain.Items.Item("MESSAGE").Text = "Adding extra posting please wait"

        sReserveID = "0"
        sPaymentID = "0"
        sClaimNumber = txtCPClaimNumber.Text.Trim()

        sAmount = GetUserInput(v_sMessage:="Enter transaction amount", v_lIsNumeric:=1, v_sDefault:=sAmount)

        If CInt(sAmount) = 0 Then
            GoTo Finally_Renamed
        End If

        'default reserve id to selected row on the reserve listview
        If txtClaimNumber.Text.Trim() = sClaimNumber Then
            If lvwReserve.Items.Count > 0 Then
                sReserveID = lvwReserve.Items.Item(lvwReserve.FocusedItem.Index).Text
            End If
        End If

        'we need this to know which reserve we are adjusting
        sReserveID = GetUserInput(v_sMessage:="Enter ReserveID", v_lIsNumeric:=1, v_sDefault:=sReserveID)

        If CInt(sReserveID) = 0 Then
            GoTo Finally_Renamed
        End If

        If optReservePayment(1).Checked Then
            sTransactionType = "C_CP"

            'default payment_id to the row selected on payment listview
            If txtClaimNumber.Text.Trim() = sClaimNumber Then
                If lvwPayment.Items.Count > 0 Then
                    sPaymentID = lvwPayment.Items.Item(lvwPayment.FocusedItem.Index).Text
                End If
            End If

            'we need this to work out who we are paying to
            sPaymentID = GetUserInput(v_sMessage:="Enter PaymentID", v_lIsNumeric:=1, v_sDefault:=sPaymentID)

            If CInt(sPaymentID) = 0 Then
                GoTo Finally_Renamed
            End If

            cThisPayment = CDec(sAmount)
        Else

            sTransactionType = IIf(chkReserve(0).CheckState = CheckState.Checked, "C_CO", "C_CR")
            cThisRevision = CDec(sAmount)
        End If


        If m_oBusiness.ReprocessClaim(r_sMessage:=sMessage, v_sClaimNumber:=sClaimNumber, v_lClaimID:=0, v_cThisRevision:=cThisRevision, v_cThisPayment:=cThisPayment, v_lOriginalReserveID:=CInt(sReserveID), v_lPaymentID:=CInt(sPaymentID), v_sTransactionType:=sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then

            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        PopulateClaimPosting()

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add claim posting", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPosting()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
    End Sub

    Private Sub txtCPClaimNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCPClaimNumber.Enter
        If Strings.Len(txtCPClaimNumber.Text) > 0 Then
            txtCPClaimNumber.SelectionStart = 0
            txtCPClaimNumber.SelectionLength = Strings.Len(txtCPClaimNumber.Text)
        End If
    End Sub

    '*****************************************************************************
    ' change document date and period_id
    '*****************************************************************************
    Private Sub ChangeDateAndPeriodID()

        Dim sPeriodID, sDocDate, sDocRef, sMessage As String

        On Error GoTo Catch_Renamed
        stbMain.Items.Item("MESSAGE").Text = "Changing document date and period_id please wait"

        sPeriodID = "0"
        sDocDate = DateTime.Parse(DateTime.Today).ToString("d")
        sMessage = ""

        sDocRef = ListViewHelper.GetListViewSubItem(lvwClaimPosting.Items.Item(lvwClaimPosting.FocusedItem.Index), 2).Text.Trim()

        sPeriodID = GetUserInput(v_sMessage:="Enter New PeriodID", v_lIsNumeric:=1, v_sDefault:=sPeriodID)

        If CInt(sPeriodID) = 0 Then
            GoTo Finally_Renamed
        End If

        sDocDate = GetUserInput(v_sMessage:="Enter New Document Date", v_lIsNumeric:=0, v_sDefault:=sDocDate)

        If Not Information.IsDate(sDocDate) Then
            GoTo Finally_Renamed
        End If


        If m_oBusiness.ChangeDateAndPeriodID(v_sDocumentRef:=sDocRef, v_dDocumentDate:=CDate(sDocDate), v_lPeriodID:=CInt(sPeriodID), r_sMessage:=sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            GoTo Finally_Renamed
        End If

        PopulateClaimPosting()

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to change document date and period_id", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeDateAndPeriodID()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
    End Sub

    '*****************************************************************************
    ' copy value from listview column into clip board
    '*****************************************************************************
    Private Sub CopyListViewColumnValue(ByVal v_oListView As ListView)

        Dim sColumn, sColumnValue As String
        Dim lColumn As Integer

        On Error GoTo Catch_Renamed
        stbMain.Items.Item("MESSAGE").Text = "Copying value from specified column please wait"

        If v_oListView.Items.Count < 1 Then
            GoTo Finally_Renamed
        End If

        If v_oListView.FocusedItem.Index + 1 < 1 Then
            GoTo Finally_Renamed
        End If

        sColumn = GetUserInput(v_sMessage:="Enter Column Number (1-" & v_oListView.Columns.Count & ")", v_lIsNumeric:=1, v_sDefault:="1")

        lColumn = CInt(sColumn)
        If lColumn = 0 Then
            GoTo Finally_Renamed
        End If

        If lColumn = 1 Then
            sColumnValue = v_oListView.Items.Item(v_oListView.FocusedItem.Index).Text
        Else
            sColumnValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(v_oListView.FocusedItem.Index), lColumn - 1).Text
        End If

        My.Computer.Clipboard.Clear()

        My.Computer.Clipboard.SetText(sColumnValue)

        GoTo Finally_Renamed

Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy value from listview", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyListViewColumnValue()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        stbMain.Items.Item("MESSAGE").Text = "Ready"
    End Sub

    '*****************************************************************************
    ' delete selected document on listview from account
    ' v_lDocumentColumn = (1 to v_oListView.ColumnHeaders.Count) column with document_ref
    '*****************************************************************************
    Private Sub DeletePostingDocument(ByVal v_oListView As ListView, ByVal v_lDocumentColumn As Integer)

        Dim sDocumentRef, sMessage As String

        Try

            If v_oListView.Items.Count < 1 Then
                Exit Sub
            End If

            If v_oListView.FocusedItem.Index + 1 < 1 Then
                Exit Sub
            End If

            If v_lDocumentColumn = 1 Then
                sDocumentRef = v_oListView.Items.Item(v_oListView.FocusedItem.Index).Text
            Else
                sDocumentRef = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(v_oListView.FocusedItem.Index), v_lDocumentColumn - 1).Text
            End If

            DeleteDocument(sDocumentRef.Trim(), False, sMessage)

            If sMessage = "" Then
                PopulateClaimPosting()
            Else
                MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed delete selected on listview from account", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePostingDocument()", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)



        End Try

    End Sub

    '*****************************************************************************
    ' populate list view with claim data
    ' NOTE: the idea is we can populate this listview with any data we want
    '*****************************************************************************
    Private Sub PopulateClaimMisc()

        Dim sMessage As String = ""
        Dim oListItem As ListViewItem
        Dim oColumnHeader As ColumnHeader
        Dim vResultArray(,) As Object

        On Error GoTo Catch_Renamed
        sMessage = ""

        stbMain.Items.Item("MESSAGE").Text = "Getting all claims without reinsurance please wait"

        'get ready for new set of data
        lvwClaimMisc.Columns.Clear()
        lvwClaimMisc.Items.Clear()

        'get all claims with no reinsurance ie no claim_risk_ri_band

        If m_oBusiness.GetNoRIClaim(vResultArray, sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        stbMain.Items.Item("MESSAGE").Text = "Displaying claims with no reinsurance please wait"

        'let's build columns headers
        oColumnHeader = lvwClaimMisc.Columns.Add("ClaimID", CInt(VB6.TwipsToPixelsX(1000)))
        oColumnHeader = lvwClaimMisc.Columns.Add("ClaimNumber", CInt(VB6.TwipsToPixelsX(2000)))


        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

            oListItem = lvwClaimMisc.Items.Add(CStr(vResultArray(0, lCount)))

            oListItem.SubItems.Add(CStr(vResultArray(1, lCount)))
        Next lCount

        lvwClaimMisc.Refresh()

        GoTo Finally_Renamed
Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate miscellaneous claim list view", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateClaimMisc()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        If sMessage <> "" Then
            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        stbMain.Items.Item("COUNT").Text = CStr(lvwClaimMisc.Items.Count)
        stbMain.Items.Item("MESSAGE").Text = "Ready"

    End Sub


    '*****************************************************************************
    'copy reinsurance model from policy and repost all transaction
    'Note: we won't repost any transaction automatically for now
    '*****************************************************************************
    Private Sub CopyRIAndRepostClaim()

        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed

        If IsTick(lvwClaimMisc) = 0 Then
            sMessage = "No items selected"
            GoTo Finally_Renamed
        End If

        For lCount As Integer = 1 To lvwClaimMisc.Items.Count
            If lvwClaimMisc.Items.Item(lCount - 1).Checked Then
                stbMain.Items.Item("MESSAGE").Text = "Copy reinsurance from policy for claim " & lvwClaimMisc.Items.Item(lCount - 1).SubItems.Item(0).Text
                'copy reinsurance model from policy

                If m_oBusiness.CopyRIToClaim(v_sClaimNumber:=lvwClaimMisc.Items.Item(lCount - 1).SubItems.Item(0), v_lClaimID:=CInt(lvwClaimMisc.Items.Item(lCount - 1).Text), r_sMessage:=sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                    GoTo Finally_Renamed
                End If
            End If
        Next lCount

        GoTo Finally_Renamed
Catch_Renamed:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy reinsurance model from policy to claim and repost transactions ", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRIAndRepostClaim()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:
        If sMessage <> "" Then
            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        'repopulate to ensure user won't try to re-copy reinsurance from policy
        PopulateClaimMisc()

        stbMain.Items.Item("COUNT").Text = CStr(lvwClaimMisc.Items.Count)
        stbMain.Items.Item("MESSAGE").Text = "Ready"

    End Sub

    Private Sub ReverseDocumentRef()

        Dim sMessage, sDocumentRef As String
        Try
            sMessage = ""

            stbMain.Items.Item("MESSAGE").Text = "Getting document ref"

            sDocumentRef = GetUserInput(v_sMessage:="Please enter document ref", v_sDefault:="")

            If sDocumentRef = "" Then
                MsgBox("Choose valid document ref", , "Repost Transaction")
                Exit Sub
            End If

            stbMain.Items.Item("MESSAGE").Text = "Reversing document - " & sDocumentRef & " from account"

            m_lReturn = m_oBusiness.ReverseDocument(v_sDocumentRef:=sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to reverse document - (" & sDocumentRef & ") "
                Throw New Exception
            End If
            stbMain.Items.Item("MESSAGE").Text = "Document Reversed"

        Catch ex As Exception
            If sMessage = "" Then
                sMessage = "Failed ReverseDocumentRef"
            End If
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        End Try
    End Sub


    Private Sub txtSqlQuery_TextChanged(sender As Object, e As EventArgs) Handles txtSqlQuery.TextChanged
        txtPolicyNumber.Text = ""
    End Sub

    Private Sub txtPolicyNumber_TextChanged(sender As Object, e As EventArgs) Handles txtPolicyNumber.TextChanged
        txtSqlQuery.Text = ""
    End Sub

    Private Sub btnReverseTrans_Click(sender As Object, e As EventArgs) Handles btnReverseTrans.Click
        Dim aoResultArray(,) As Object
        Dim iCnt As Integer
        Dim sDocumentRef As String = ""
        Try
            For Each oListItem As ListViewItem In Me.lvwSRPDcouments.Items
                If oListItem.Checked Then
                    sDocumentRef = oListItem.SubItems(1).Text

                    m_lReturn = m_oBusiness.ValidateDocumentRef(sDocumentRef:=sDocumentRef, vResultArray:=aoResultArray)
                    If IsArray(aoResultArray) Then
                        For iCnt = 0 To UBound(aoResultArray, 2)
                            m_lReturn = m_oBusiness.ReverseAllocation(lTransDetailId:=aoResultArray(0, iCnt))
                        Next
                        If chkReverseAllocation.CheckState = CheckState.Unchecked Then
                            m_lReturn = m_oBusiness.ReverseDocument(v_sDocumentRef:=sDocumentRef)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MsgBox("Failed to reverse document - (" & txtDocumentRef.Text & ") ")
                                Throw New Exception
                            End If
                        End If
                    End If
                End If
            Next
            If chkReverseAllocation.CheckState = CheckState.Unchecked Then
                MsgBox("Reversal of allocation and transaction is done successfully", , "Data Fix Utility")
                btnGetSRPDetails_Click(Nothing, Nothing)
            Else
                MsgBox("Reverse Allocation is done successfully", , "Data Fix Utility")
                btnGetSRPDetails_Click(Nothing, Nothing)
            End If
        Catch excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reverse selected document", vApp:=ACApp, vClass:=ACClass, vMethod:="ReveseDocument()", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try

    End Sub

    Private Sub CmdGetClaimVersions_Click(sender As Object, e As EventArgs) Handles CmdGetClaimVersions.Click

        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.CmdGetClaimVersions.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all versions of this Claims"


            sSql = TxtSqlClaim.Text


            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                LvwClaimVersion.Items.Clear()
                Exit Sub
            End If

            LvwClaimVersion.Items.Clear()



            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'Cliam Id
                oListItem = Me.LvwClaimVersion.Items.Add(CStr(aoResultArray(ACFieldRevClaimid, lCount)))

                'Claim Number            
                oListItem.SubItems.Insert(ACFieldRevClaimNumber, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRevClaimNumber, lCount).Trim()))

                'Document Ref               
                oListItem.SubItems.Insert(ACFieldRevDocumentRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRevDocumentRef, lCount).Trim()))

                'Transaction Type Code               
                oListItem.SubItems.Insert(ACFieldRevTransactionTypeCode, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRevTransactionTypeCode, lCount).Trim()))


            Next

            If LvwClaimVersion.Items.Count > 0 Then
                LvwClaimVersion.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.LvwClaimVersion.Items.Count)
            CmdSelectAllCloneClaim.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed CmdGetClaimVersions_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="CmdGetClaimVersions_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.CmdGetClaimVersions.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub CmdClaiimFix_Click(sender As Object, e As EventArgs) Handles CmdClaiimFix.Click
        ProcessMultiClaim()
    End Sub

    Private Sub ProcessMultiClaim()

        '  Dim oListItem As ListViewItem
        Dim lRecordCount As Long
        Dim lCount As Long
        Dim bProceed As Boolean = True
        Try
            lRecordCount = 0
            'For lCount = Me.optMultiPolicy.LBound To Me.optMultiPolicy.UBound
            '    If Me.optMultiPolicy(lCount).Value = True Then
            '        Exit For
            '    End If
            'Next

            If OptReverse.Checked Then
                lCount = 0
            ElseIf OptReverseReg.Checked Then
                lCount = 1
            End If

            If IsTick(oListView:=LvwClaimVersion) > 0 Then
                If txtPMNumber.Text.Trim() = "" Then
                    MsgBox("Please enter valid PM/IM number", vbInformation, ACApp)
                    txtPMNumber.Focus()
                    Exit Sub
                End If
                'confirm that user really want to do this
                If MsgBox("Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                    Exit Sub
                End If
            End If

            Select Case lCount
                Case 0 'Reverse
                    For Each oListItem As ListViewItem In Me.LvwClaimVersion.Items

                        If oListItem.Checked = True Then
                            lRecordCount = lRecordCount + 1
                            stbMain.Items.Item("COUNT").Text = lRecordCount
                            stbMain.Items.Item("MESSAGE").Text = "Processing Claim " & CStr(oListItem.SubItems(1).Text) & "; Document Ref " & CStr(oListItem.SubItems(2).Text)
                            stbMain.Refresh()
                            If m_oBusiness.ProcessClaimTransactions(v_lClaimId:=CLng(oListItem.Text), _
                                                                v_sDocumentRef:=(oListItem.SubItems(2).Text), v_sRefNumber:=txtPMNumber.Text, v_sTransactionTypeCode:=oListItem.SubItems(3).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New Exception
                                Exit For
                            End If
                        End If
                    Next
                Case 1 'Reverse and Reraise
                    For Each oListItem As ListViewItem In Me.LvwClaimVersion.Items
                        If oListItem.Checked = True Then
                            lRecordCount = lRecordCount + 1
                            stbMain.Items.Item("COUNT").Text = lRecordCount
                            stbMain.Items.Item("MESSAGE").Text = "Processing Claim " & CStr(oListItem.SubItems(1).Text) & "; Document Ref " & CStr(oListItem.SubItems(2).Text)
                            stbMain.Refresh()
                            If m_oBusiness.ProcessClaimTransactions(v_lClaimId:=CLng(oListItem.Text), _
                                                                v_sDocumentRef:=oListItem.SubItems(2).Text, v_sRefNumber:=txtPMNumber.Text, v_bRePost:=True, v_sTransactionTypeCode:=oListItem.SubItems(3).Text) <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception
                                Exit For
                            End If
                        End If
                    Next
            End Select
            stbMain.Items.Item("MESSAGE").Text = "Process Completed "
            ' cmdGetDetail_Click()
            MsgBox("Item Processed successfully", , "Data Fix Utility")

            CmdGetClaimVersions_Click(CmdGetClaimVersions, New EventArgs())

            'stbMain.Panels("COUNT").Text = lRecordCount
            'stbMain.Panels("MESSAGE").Text = "Processed " & lRecordCount & " Records"

        Catch excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMultiClaim", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMultiClaim()", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub


    '    '********************************************************************************************
    '    'return number of items ticked on list view
    '    '
    '    '********************************************************************************************
    '    Private Function IsTick(ByVal oListView As ListView) As Long

    '        Dim oListItem As ListItem
    '        Dim lCount As Long

    '    On Error GoTo Catch
    '        lCount = 0

    '        For Each oListItem In oListView.ListItems
    '            If oListItem.Checked Then
    '                lCount = lCount + 1
    '            End If
    '        Next

    '    GoTo Finally

    'Catch:
    '            lCount = -1

    '            LogMessagePopup( _
    '                iType:=PMLogOnError, _
    '                sMsg:="Failed to check for ticked items on listview", _
    '                vApp:=ACApp, _
    '                vClass:=ACClass, _
    '                vMethod:="IsTick()", _
    '                vErrNo:=Err.Number, _
    '                vErrDesc:=Err.Description)

    '        Finally
    '            IsTick = lCount
    '            Exit Function
    '    End Function




    Private Sub btnGetSRPDetails_Click(sender As Object, e As EventArgs) Handles btnGetSRPDetails.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.btnGetSRPDetails.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting Document Details"

            If txtDocumentRef.Text <> "" Then
                sSql = "SELECT document_id,document_ref FROM document WHERE Document_ref= " & "'" & txtDocumentRef.Text & "'"

            Else
                sSql = txtSRPSQL.Text
            End If



            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                lvwSRPDcouments.Items.Clear()
                Exit Sub
            End If

            lvwSRPDcouments.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'insurance file cnt
                oListItem = Me.lvwSRPDcouments.Items.Add(CStr(aoResultArray(0, lCount)))

                'dDocument ref               
                oListItem.SubItems.Insert(1, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(1, lCount).Trim()))


            Next

            If lvwSRPDcouments.Items.Count > 0 Then
                lvwSRPDcouments.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed btnGetSRPDetails_Click_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="btnGetSRPDetails_Click_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.btnGetSRPDetails.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try

    End Sub

    Private Sub txtSRPSQL_TextChanged(sender As Object, e As EventArgs) Handles txtSRPSQL.TextChanged
        txtDocumentRef.Text = ""
    End Sub

    Private Sub txtDocumentRef_TextChanged(sender As Object, e As EventArgs) Handles txtDocumentRef.TextChanged
        txtSRPSQL.Text = ""
    End Sub


    Private Sub cmdExit_Click_1(sender As Object, e As EventArgs) Handles cmdExit.Click
        End
    End Sub

    Private Sub chkRIRefresh_CheckedChanged(sender As Object, e As EventArgs) Handles chkRIRefresh.CheckedChanged

    End Sub

    Private Sub CmdGetClaimVersionsTrans_Click(sender As Object, e As EventArgs) Handles CmdGetClaimVersionsTrans.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Dim m_lReturn As Integer
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.CmdGetClaimVersionsTrans.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all versions of this Claims"


            If txtClaimNo.Text <> "" Then
                sSql = "SELECT sf.loss_id , sf.loss_code ,sf.insurance_ref,sf.document_ref FROM stats_folder sf "
                sSql = sSql & " where document_ref = " & "'" & txtClaimNo.Text & "'"
                sSql = sSql & " and loss_id not in (select claim_id from DataFixUtility_log where claim_id is not null )"
            Else
                sSql = txtSqlQueryClaim.Text
            End If

            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical)
                Exit Sub
            End If


            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)
            'm_lReturn = GetAllEntries(aoResultArray, sSql)


            If Not Information.IsArray(aoResultArray) Then
                LvwClaimVersionTrans.Items.Clear()
                Exit Sub
            End If

            LvwClaimVersionTrans.Items.Clear()


            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'Cliam Id
                oListItem = Me.LvwClaimVersionTrans.Items.Add(CStr(aoResultArray(ACFieldRevClaimTransid, lCount)))

                'Claim Number            
                oListItem.SubItems.Insert(ACFieldRevClaimTransNumber, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRevClaimTransNumber, lCount).Trim()))

                'Insurance Ref               
                oListItem.SubItems.Insert(ACFieldClaimTransInsuranceref, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldClaimTransInsuranceref, lCount).Trim()))

                'Document Ref
                oListItem.SubItems.Insert(ACFieldRevClaimTransDocumentRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldRevClaimTransDocumentRef, lCount).Trim()))

            Next

            If LvwClaimVersionTrans.Items.Count > 0 Then
                LvwClaimVersionTrans.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.LvwClaimVersionTrans.Items.Count)
            CmdSelectAllClaim.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed CmdGetClaimVersionsTrans", vApp:=ACApp, _
                                       vClass:=ACClass, vMethod:="CmdGetClaimVersionsTrans", vErrNo:=CStr(Information.Err().Number), _
                                       vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.CmdGetClaimVersionsTrans.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub CmdClaimTrans_Click(sender As Object, e As EventArgs) Handles CmdClaimTrans.Click
        Dim lReturn As Integer
        Dim lCount As Integer
        Dim lRecordCount As Integer

        lRecordCount = 0

        'For lCount = Me.optMultiPolicy.LBound To Me.optMultiPolicy.UBound
        '    If Me.optMultiPolicy(lCount).Value = True Then
        '        Exit For
        '    End If
        'Next



        If OptReverseClmTrans.Checked Then
            lCount = 0
        ElseIf OptReverseRegClmTrans.Checked Then
            lCount = 1
        End If

        If IsTick(oListView:=LvwClaimVersionTrans) > 0 Then
            If txtPMNumber.Text.Trim() = "" Then

                MsgBox("Please enter valid PM/IM number", vbInformation, ACApp)
                txtPMNumber.Focus()
                Exit Sub
            End If
            'confirm that user really want to do this

            If MsgBox("Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                Exit Sub
            End If
        End If

        CmdClaimTrans.Enabled = False
        CmdGetClaimVersionsTrans.Enabled = False

        Select Case lCount
            Case 0 'Reverse
                For Each oListItem As ListViewItem In Me.LvwClaimVersionTrans.Items

                    If oListItem.Checked = True Then

                        lRecordCount = lRecordCount + 1
                        stbMain.Items.Item("COUNT").Text = lRecordCount
                        stbMain.Items.Item("MESSAGE").Text = "Processing Claim " & CStr(oListItem.SubItems(1).Text) & "; Document Ref " & CStr(oListItem.SubItems(3).Text)
                        stbMain.Refresh()
                        If m_oBusiness.ProcessTransactions(CLng(oListItem.Text), oListItem.SubItems(3).Text, v_bRePost:=False, v_bRIRefresh:=chkRIRefreshClmTrans.Checked, v_sRefNumber:=txtPMNumber.Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                            'lReturn = ProcessTransactions(CLng(oListItem.Text), oListItem.SubItems(3).Text, v_bRePost:=False, v_bRIRefresh:=chkRIRefresh.Checked, v_sRefNumber:=txtPMNumber.Text)


                            Throw New Exception
                            Exit For
                        End If
                    End If
                Next
            Case 1 'Reverse and Reraise
                For Each oListItem As ListViewItem In Me.LvwClaimVersionTrans.Items
                    If oListItem.Checked = True Then

                        lRecordCount = lRecordCount + 1
                        stbMain.Items.Item("COUNT").Text = lRecordCount
                        stbMain.Items.Item("MESSAGE").Text = "Processing Claim " & CStr(oListItem.SubItems(1).Text) & "; Document Ref " & CStr(oListItem.SubItems(3).Text)
                        stbMain.Refresh()

                        stbMain.Refresh()
                        If m_oBusiness.ProcessTransactions(CLng(oListItem.Text), oListItem.SubItems(3).Text, v_bRePost:=True, v_bRIRefresh:=chkRIRefreshClmTrans.Checked, v_sRefNumber:=txtPMNumber.Text) <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' lReturn = ProcessTransactions(CLng(oListItem.Text), oListItem.SubItems(3).Text, v_bRePost:=True, v_bRIRefresh:=chkRIRefresh.Checked, v_sRefNumber:=txtPMNumber.Text)
                            Throw New Exception
                            Exit For
                        End If
                    End If
                Next
        End Select


        stbMain.Items.Item("MESSAGE").Text = "Process Completed "
        stbMain.Refresh()
        MsgBox("Item Processed successfully", , "Data Fix Utility")
        'SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Call CmdGetClaimVersionsTrans_Click(Nothing, Nothing)
        CmdClaimTrans.Enabled = True
        CmdGetClaimVersionsTrans.Enabled = True
    End Sub

    Private Sub CmdSelectAllPolicy_Click(sender As Object, e As EventArgs) Handles CmdSelectAllPolicy.Click
        Dim i As Integer
        m_bisFirstSelect = True
        If CmdSelectAllPolicy.Text = "Select All" Then
            For i = 0 To lvwPolicyVersion.Items.Count - 1
                lvwPolicyVersion.Items(i).Checked = True
            Next
            CmdSelectAllPolicy.Text = "UnSelect All"
            Exit Sub
        End If
        If CmdSelectAllPolicy.Text = "UnSelect All" Then
            For i = 0 To lvwPolicyVersion.Items.Count - 1
                lvwPolicyVersion.Items(i).Checked = False
            Next
            CmdSelectAllPolicy.Text = "Select All"
        End If
        m_bisFirstSelect = False
    End Sub

    Private Sub CmdSelectAllCloneClaim_Click(sender As Object, e As EventArgs) Handles CmdSelectAllCloneClaim.Click
        Dim i As Integer
        If CmdSelectAllCloneClaim.Text = "Select All" Then
            For i = 0 To LvwClaimVersion.Items.Count - 1
                LvwClaimVersion.Items(i).Checked = True
            Next
            CmdSelectAllCloneClaim.Text = "UnSelect All"
            Exit Sub
        End If
        If CmdSelectAllCloneClaim.Text = "UnSelect All" Then
            For i = 0 To LvwClaimVersion.Items.Count - 1
                LvwClaimVersion.Items(i).Checked = False
            Next
            CmdSelectAllCloneClaim.Text = "Select All"
        End If
    End Sub

    Private Sub CmdSelectAllClaim_Click(sender As Object, e As EventArgs) Handles CmdSelectAllClaim.Click

        Dim i As Integer
        If CmdSelectAllClaim.Text = "Select All" Then
            For i = 0 To LvwClaimVersionTrans.Items.Count - 1
                LvwClaimVersionTrans.Items(i).Checked = True
            Next
            CmdSelectAllClaim.Text = "UnSelect All"
            Exit Sub
        End If
        If CmdSelectAllClaim.Text = "UnSelect All" Then
            For i = 0 To LvwClaimVersionTrans.Items.Count - 1
                LvwClaimVersionTrans.Items(i).Checked = False
            Next
            CmdSelectAllClaim.Text = "Select All"
        End If
    End Sub

    Private Sub CmdSRP_Click(sender As Object, e As EventArgs) Handles CmdSRP.Click
        Dim i As Integer
        If CmdSRP.Text = "Select All" Then
            For i = 0 To lvwSRPDcouments.Items.Count - 1
                lvwSRPDcouments.Items(i).Checked = True
            Next
            CmdSRP.Text = "UnSelect All"
            Exit Sub
        End If
        If CmdSRP.Text = "UnSelect All" Then
            For i = 0 To lvwSRPDcouments.Items.Count - 1
                lvwSRPDcouments.Items(i).Checked = False
            Next
            CmdSRP.Text = "Select All"
        End If
    End Sub

    Private Sub _optSinglePolicy_4_CheckedChanged(sender As Object, e As EventArgs) Handles _optSinglePolicy_4.CheckedChanged
        chkRIRefresh.Enabled = True
        chkRatingRefresh.Enabled = True
        ChkRepost_RepostedTrans.Enabled = True

    End Sub

    Private Sub TabClaimTransaction_Click(sender As Object, e As EventArgs) Handles TabClaimTransaction.Click

    End Sub

    Private Sub _tabMain_TabPage1_Click(sender As Object, e As EventArgs) Handles _tabMain_TabPage1.Click

    End Sub

    Private Sub _optSinglePolicy_3_CheckedChanged(sender As Object, e As EventArgs) Handles _optSinglePolicy_3.CheckedChanged
        chkRIRefresh.Enabled = False
        chkRIRefresh.Checked = False
        chkRatingRefresh.Enabled = False
        chkRatingRefresh.Checked = False
        ChkRepost_RepostedTrans.Enabled = False
        ChkRepost_RepostedTrans.Checked = False
    End Sub

    Private Sub OptReverseRegClmTrans_CheckedChanged(sender As Object, e As EventArgs) Handles OptReverseRegClmTrans.CheckedChanged
        chkRIRefreshClmTrans.Enabled = True
    End Sub

    Private Sub OptReverseClmTrans_CheckedChanged(sender As Object, e As EventArgs) Handles OptReverseClmTrans.CheckedChanged
        chkRIRefreshClmTrans.Enabled = False
        chkRIRefreshClmTrans.Checked = False
    End Sub

    Private Sub btnGetAllocationDetails_Click(sender As Object, e As EventArgs) Handles btnGetAllocationDetails.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.btnGetAllocationDetails.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all allocation details"

            sSql = txtSQL.Text

            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                lvlAllocationDetails.Items.Clear()
                Exit Sub
            End If

            lvlAllocationDetails.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'Allocation Id
                oListItem = Me.lvlAllocationDetails.Items.Add(CStr(aoResultArray(ACFieldPVAllocationId, lCount)))

                'Original Doc Ref               
                oListItem.SubItems.Insert(ACFieldPVOriginalDocRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVOriginalDocRef, lCount).Trim()))

                'Allocated Doc Ref               
                oListItem.SubItems.Insert(ACFieldPVAllocatedDocRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVAllocatedDocRef, lCount).Trim()))

                'Associated CLD/SDD               
                oListItem.SubItems.Insert(ACFieldPVAssociatedDocRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVAssociatedDocRef, lCount).Trim()))

                'oListItem.SubItems(ACFieldPVPolicyType).Tag = aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()

                'Document Ref               
                oListItem.SubItems.Insert(ACFieldPVFACAccountId, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVFACAccountId, lCount).Trim()))

                'policy start                
                oListItem.SubItems.Insert(ACFieldPVFACAccount, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVFACAccount, lCount).Trim()))

            Next

            If lvlAllocationDetails.Items.Count > 0 Then
                lvlAllocationDetails.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvlAllocationDetails.Items.Count)
            btnSelectAll.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed btnGetAllocationDetails_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="btnGetAllocationDetails_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.btnGetAllocationDetails.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click

        If (Me.lvlAllocationDetails.Items.Count > 0) Then
            Dim i As Integer
            If btnSelectAll.Text = "Select All" Then
                For i = 0 To lvlAllocationDetails.Items.Count - 1
                    lvlAllocationDetails.Items(i).Checked = True
                Next
                btnSelectAll.Text = "UnSelect All"
                Exit Sub
            End If
            If btnSelectAll.Text = "UnSelect All" Then
                For i = 0 To lvlAllocationDetails.Items.Count - 1
                    lvlAllocationDetails.Items(i).Checked = False
                Next
                btnSelectAll.Text = "Select All"
            End If
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If (Me.lvlAllocationDetails.Items.Count > 0) Then
            Me.btnOK.Enabled = False
            Me.btnExit.Enabled = False
            btnSelectAll.Enabled = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If MsgBox("Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                Me.btnOK.Enabled = True
                Me.btnExit.Enabled = True
                btnSelectAll.Enabled = True
                Exit Sub
            End If
            bIsSucesfullyCompleted = True

            ReverseAndAllocateDocument()

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwPolicyVersion.Items.Count)
            If bIsSucesfullyCompleted Then
                MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
            End If

            Me.btnOK.Enabled = True
            Me.btnExit.Enabled = True
            btnSelectAll.Enabled = True
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Call btnGetAllocationDetails_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub ReverseAndAllocateDocument()
        Try
            For Each oListItem As ListViewItem In Me.lvlAllocationDetails.Items
                If oListItem.Checked Then
                    Dim sAllocatedDocRef As String = oListItem.SubItems(2).Text
                    Dim sAssociatedDocRef As String = oListItem.SubItems(3).Text
                    ReverseAndAllocateSingleDocument(AllocationId:=CInt(oListItem.Text), AllocatedDocRef:=sAllocatedDocRef, AssociatedDocRef:=sAssociatedDocRef)
                End If
            Next oListItem

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ReverseAndAllocateDocument ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAndAllocateDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try
    End Sub

    Private Sub ReverseAndAllocateSingleDocument(ByVal AllocationId As Integer, ByVal AllocatedDocRef As String, ByVal AssociatedDocRef As String)
        Try
            m_oBusiness.BeginTrans()

            m_lReturn = m_oBusiness.AllocateDocuments(AllocationId:=AllocationId, AllocatedDocRef:=AllocatedDocRef, AssociatedDocRef:=AssociatedDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Reversse Stats ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                Throw New System.Exception
            End If

            stbMain.Items.Item("MESSAGE").Text = "Done"

            m_oBusiness.CommitTrans()
        Catch ex As Exception
            bIsSucesfullyCompleted = False
            m_oBusiness.RollbackTrans()
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReverseAndAllocateSingleDocument", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAndAllocateSingleDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try
    End Sub

    Private Sub btnSearchBordereau_Click(sender As Object, e As EventArgs) Handles btnSearchBordereau.Click

        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            stbMain.Items.Item("MESSAGE").Text = "Getting Bordereau Details"

            Dim sRegexUserName As Regex = New Regex("[a-zA-Z]")
            Dim sRegexDepositNumber As Regex = New Regex("[a-zA-Z0-9]")
            Dim sRegexBordereauReference As Regex = New Regex("[a-zA-z0-9/]")
            Dim bIsValid As Boolean = False
            Dim nUserID As Integer = 0
            Dim sDepositNumber As String = ""
            Dim sBordereauReference As String = ""
            If txtUsername.Text <> "" Then
                bIsValid = sRegexUserName.IsMatch(txtUsername.Text.Trim())
            Else
                MsgBox("Please enter a valid user name", vbCritical, ACApp)
                Exit Sub
            End If

            If bIsValid Then
                sSql = "SELECT user_id FROM PMUSER WHERE USERNAME = " & "'" & txtUsername.Text.Trim() & "'"
            Else
                MsgBox("User name can have a-z or A-Z characters", vbCritical, ACApp)
                Exit Sub
            End If

            If InStr(1, sSql, "delete") <> 0 OrElse InStr(1, sSql, "update") OrElse InStr(1, sSql, "insert") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)

            If Not Information.IsArray(aoResultArray) Then
                MsgBox("User not found", vbCritical, ACApp)
                Exit Sub
            Else
                nUserID = aoResultArray(0, 0)
            End If

            If txtDepositNumber.Text <> "" Then
                bIsValid = sRegexDepositNumber.IsMatch(txtDepositNumber.Text.Trim())
                If bIsValid Then
                    sDepositNumber = txtDepositNumber.Text.Trim()
                Else
                    MsgBox("Deposit number can have a-z or A-Z or 0-9 characters", vbCritical, ACApp)
                    Exit Sub
                End If
            End If


            If txtBordereauReference.Text <> "" Then
                bIsValid = sRegexBordereauReference.IsMatch(txtBordereauReference.Text.Trim())
                If bIsValid Then
                    sBordereauReference = ToSafeString(txtBordereauReference.Text.Trim())
                Else
                    MsgBox("Bordereau Reference can have a-z or A-Z or 0-9 or / characters", vbCritical, ACApp)
                    Exit Sub
                End If
            End If

            aoResultArray = Nothing
            m_lReturn = m_oBusiness.SearchBordereau(sDepositNumber, sBordereauReference, nUserID, aoResultArray)

            If Not Information.IsArray(aoResultArray) Then
                MsgBox("No Bordereau records found", vbCritical, ACApp)
                Exit Sub
            End If

            btnSearchBordereau.Enabled = False
            lvwSearchBordereau.Items.Clear()
            lvwSearchBordereau.Columns(0).Text = "BordereauId"
            lvwSearchBordereau.Columns(1).Text = "ReceivedFromAccount"
            lvwSearchBordereau.Columns(2).Text = "ReceivedFrom"
            lvwSearchBordereau.Columns(3).Text = "ReceivedFrom_party_cnt"
            lvwSearchBordereau.Columns(4).Text = "AccountCode"
            lvwSearchBordereau.Columns(5).Text = "Account"
            lvwSearchBordereau.Columns(6).Text = "Agent_party_cnt"
            lvwSearchBordereau.Columns(7).Text = "BranchAmount"
            lvwSearchBordereau.Columns(8).Text = "BordereauTransactionTypeId"
            lvwSearchBordereau.Columns(9).Text = "BordereauTransactionId"
            lvwSearchBordereau.Columns(10).Text = "TransactionType"
            lvwSearchBordereau.Columns(11).Text = "Branch_source_id"
            lvwSearchBordereau.Columns(12).Text = "BordereauChannel"
            lvwSearchBordereau.Columns(13).Text = "BordereauChannelId"
            lvwSearchBordereau.Columns(14).Text = "BordereauStatus"
            lvwSearchBordereau.Columns(15).Text = "BordereauStatusId"
            lvwSearchBordereau.Columns(16).Text = "Amount"
            lvwSearchBordereau.Columns(17).Text = "BordereauReference"
            lvwSearchBordereau.Columns(18).Text = "DepositNumber"
            lvwSearchBordereau.Columns(19).Text = "CapturedDate"
            lvwSearchBordereau.Columns(20).Text = "Bordereau_Currency_Id"


            For nCount As Integer = 0 To aoResultArray.GetUpperBound(1)
                oListItem = Me.lvwSearchBordereau.Items.Add(aoResultArray(KBordereauId, nCount))
                oListItem.SubItems.Insert(KReceivedFromAccount, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KReceivedFromAccount, nCount)))
                oListItem.SubItems.Insert(KReceivedFrom, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KReceivedFrom, nCount)))
                oListItem.SubItems.Insert(KReceivedFromPartyCnt, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KReceivedFromPartyCnt, nCount)))
                oListItem.SubItems.Insert(KAccountCode, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KAccountCode, nCount)))
                oListItem.SubItems.Insert(KAccount, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KAccount, nCount)))
                oListItem.SubItems.Insert(KAgentPartyCnt, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KAgentPartyCnt, nCount)))
                oListItem.SubItems.Insert(KBranchAmount, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBranchAmount, nCount)))
                oListItem.SubItems.Insert(KBordereauTransactionTypeId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauTransactionTypeId, nCount)))
                oListItem.SubItems.Insert(KBordereauTransactionId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauTransactionId, nCount)))
                oListItem.SubItems.Insert(KTransactionType, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KTransactionType, nCount)))
                oListItem.SubItems.Insert(KBranchSourceId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBranchSourceId, nCount)))
                oListItem.SubItems.Insert(KBordereauChannel, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauChannel, nCount)))
                oListItem.SubItems.Insert(KBordereauChannelId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauChannelId, nCount)))
                oListItem.SubItems.Insert(KBordereauStatus, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauStatus, nCount)))
                oListItem.SubItems.Insert(KBordereauStatusId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauStatusId, nCount)))
                oListItem.SubItems.Insert(KAmount, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KAmount, nCount)))
                oListItem.SubItems.Insert(KBordereauReference, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauReference, nCount)))
                oListItem.SubItems.Insert(KDepositNumber, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KDepositNumber, nCount)))
                oListItem.SubItems.Insert(KCapturedDate, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KCapturedDate, nCount)))
                oListItem.SubItems.Insert(KBordereauCCurrencyId, New ListViewItem.ListViewSubItem(oListItem, aoResultArray(KBordereauCCurrencyId, nCount)))
            Next

            If lvwSearchBordereau.Items.Count > 0 Then
                lvwSearchBordereau.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(lvwSearchBordereau.Items.Count)
            btnAddTask.Enabled = True
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed btnSearchBordereau_Click", vApp:=ACApp,
                                         vClass:=ACClass, vMethod:="btnSearchBordereau_Click", vErrNo:=CStr(Information.Err().Number),
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        lvwSearchBordereau.Items.Clear()
        btnSearchBordereau.Enabled = True
        txtDepositNumber.Text = ""
        txtUsername.Text = ""
        txtBordereauReference.Text = ""
        btnAddTask.Enabled = False
    End Sub

    Private Sub btnAddTask_Click(sender As Object, e As EventArgs) Handles btnAddTask.Click

        Try
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            btnAddTask.Enabled = False
            Dim oListItem As ListViewItem = Nothing
            Dim nSelectedRow As Integer = lvwSearchBordereau.SelectedIndices(0)

            If MsgBox("Are you sure you have selected the correct record ?", MsgBoxStyle.YesNo, "Warning") = MsgBoxResult.Yes Then
                m_lReturn = m_oBusiness.AddMissingTask(CInt(lvwSearchBordereau.Items(nSelectedRow).SubItems(9).Text))
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    MsgBox("Failed to add missing bordereau task. Please do not retry contact support team.")
                End If

                m_lReturn = m_oBusiness.AddDataFixUtilityLog(txtUsername.Text, txtBordereauReference.Text,
                                                             txtDepositNumber.Text)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    MsgBox("Failed to add missing bordereau task. Please do not retry contact support team.")
                End If

            Else
                Exit Sub
            End If
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Missing Bordereau Task", vApp:=ACApp,
                                         vClass:=ACClass, vMethod:="btnAddTask_Click", vErrNo:=CStr(Information.Err().Number),
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseReset)
            btnAddTask.Enabled = False
            btnSearchBordereau.Enabled = True
        End Try
    End Sub

    'Private Sub TabPage3_Click(sender As Object, e As EventArgs)
    '    Label6.Visible = False
    '    txtPMNumber.Visible = False
    'End Sub

    Private Sub btnGetDuplicatePolicyVersions_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnGetDuplicatePolicyVersions.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            'If Me.txtPolicyNumber.Text = "" Then
            '    MessageBox.Show("Please enter policy number", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    txtPolicyNumber.Focus()
            '    Exit Sub
            'End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.cmdGetPolicyVersion.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all versions of this policy"

            If txtPolicyNo.Text <> "" Then
                sSql = "SELECT i.insurance_file_cnt,i.insurance_ref,i.insurance_file_type_id,ift.description,d.document_ref,i.cover_start_date,document_date FROM insurance_file i "
                sSql = sSql & " INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id=i.insurance_file_type_id "
                sSql = sSql & "LEFT JOIN document d ON d.insurance_file_cnt = i.insurance_file_cnt WHERE document_ref LIKE 'S%' AND  (I.insurance_file_cnt NOT IN (select  insurance_file_cnt from   datafixutility_log) ) AND "
                sSql = sSql & " i.insurance_ref= " & "'" & txtPolicyNo.Text & "' ORDER  BY i.insurance_ref,i.insurance_file_cnt"
            Else
                sSql = txtsqlDuplicateVersions.Text
            End If



            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                lvwDuplicatePolicyVersion.Items.Clear()
                Exit Sub
            End If

            lvwDuplicatePolicyVersion.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'insurance file cnt
                oListItem = Me.lvwDuplicatePolicyVersion.Items.Add(CStr(aoResultArray(ACFieldPVInsuranceFileCnt, lCount)))

                'insurance ref               
                oListItem.SubItems.Insert(ACFieldPVInsuranceRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVInsuranceRef, lCount).Trim()))

                'policy type id                
                oListItem.SubItems.Insert(ACFieldPVPolicyTypeID, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()))

                'policy type                
                oListItem.SubItems.Insert(ACFieldPVPolicyType, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyType, lCount).Trim()))

                oListItem.SubItems(ACFieldPVPolicyType).Tag = aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()

                'Document Ref               
                oListItem.SubItems.Insert(ACFieldPVDocumentRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentRef, lCount).Trim()))

                'policy start                
                oListItem.SubItems.Insert(ACFieldPVPolicyStart, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyStart, lCount).Trim()))

                'document Date               
                oListItem.SubItems.Insert(ACFieldPVDocumentDate, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentDate, lCount).Trim()))


            Next

            If lvwDuplicatePolicyVersion.Items.Count > 0 Then
                lvwDuplicatePolicyVersion.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwDuplicatePolicyVersion.Items.Count)
            CmdSelectAllPolicy.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed btnGetDuplicatePolicyVersions_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="btnGetDuplicatePolicyVersions_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.btnGetDuplicatePolicyVersions.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub btnSelectAllDuplicateVersions_Click(sender As Object, e As EventArgs) Handles btnSelectAllDuplicateVersions.Click
        Dim i As Integer
        If btnSelectAllDuplicateVersions.Text = "Select All" Then
            For i = 0 To lvwDuplicatePolicyVersion.Items.Count - 1
                lvwDuplicatePolicyVersion.Items(i).Checked = True
            Next
            btnSelectAllDuplicateVersions.Text = "UnSelect All"
            Exit Sub
        End If
        If btnSelectAllDuplicateVersions.Text = "UnSelect All" Then
            For i = 0 To lvwDuplicatePolicyVersion.Items.Count - 1
                lvwDuplicatePolicyVersion.Items(i).Checked = False
            Next
            btnSelectAllDuplicateVersions.Text = "Select All"
        End If
    End Sub

    Private Sub btnDuplicatePolicyVersionsOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnDuplicatePolicyVersionsOk.Click
        Me.btnDuplicatePolicyVersionsOk.Enabled = False
        Me.cmdExit.Enabled = False
        btnSelectAllDuplicateVersions.Enabled = False
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Select Case SSTabHelper.GetSelectedIndex(Me.tabMain)
            'Case 0 'Multi transaction
            'ProcessFailedTransaction()
            'stbMain.Items.Item("COUNT").Text = CStr(Me.lvwSelectPolicy.Items.Count)
            Case 0 'Single policy
                If (txtPMNumber.Text.Trim() = "") Then
                    MsgBox("Please Enter Valid IM/PM number ", vbInformation)
                    Me.btnDuplicatePolicyVersionsOk.Enabled = True
                    Me.cmdExit.Enabled = True
                    btnSelectAllDuplicateVersions.Enabled = True
                    txtPMNumber.Focus()
                    Exit Sub
                End If

                If MsgBox("This will reverse the transaction and selected version will mark as cancelled quote. Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                    Me.btnDuplicatePolicyVersionsOk.Enabled = True
                    Me.cmdExit.Enabled = True
                    btnSelectAllDuplicateVersions.Enabled = True
                    Exit Sub
                End If

                bIsSucesfullyCompleted = True
                ReverseDuplicatePolicyTransaction()
                stbMain.Items.Item("COUNT").Text = CStr(Me.lvwDuplicatePolicyVersion.Items.Count)
        End Select
        If bIsSucesfullyCompleted Then
            MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
        End If

        Me.btnDuplicatePolicyVersionsOk.Enabled = True
        Me.cmdExit.Enabled = True
        btnSelectAllDuplicateVersions.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        btnGetDuplicatePolicyVersions_Click(Nothing, Nothing)
    End Sub

    Private Sub lvwDuplicatePolicyVersion_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwDuplicatePolicyVersion.Columns(EventArgs.Column)

        ' Column click event for the search details
        Try

            With lvwDuplicatePolicyVersion
                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwDuplicatePolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwDuplicatePolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion) + 1) Mod 2)
                    'Special Sort function for Dates
                    'Modified by Sumeet Singh on 5/25/2010 11:10:35 AM refer developer guide no. 178
                    'ListViewSortByDate(lvwDuplicatePolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion))
                    ListViewFunc.ListViewSortByDate(lvwDuplicatePolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion))

                    'Identify the Value type columns (to sort numerics correctly)
                ElseIf Convert.ToString(ColumnHeader.Tag).ToUpper() = "VALUESORT" Then
                    ListViewHelper.SetSortedProperty(lvwDuplicatePolicyVersion, False)
                    ListViewHelper.SetSortOrderProperty(lvwDuplicatePolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion) + 1) Mod 2)
                    'Use the special sort function for numerics
                    'Modified by Sumeet Singh on 5/25/2010 11:10:47 AM refer developer guide no. 178
                    'ListViewSortByValue(lvwDuplicatePolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion))
                    ListViewFunc.ListViewSortByValue(lvwDuplicatePolicyVersion, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion))

                    'See if this the column already sorted on
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwDuplicatePolicyVersion)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwDuplicatePolicyVersion, (ListViewHelper.GetSortOrderProperty(lvwDuplicatePolicyVersion) + 1) Mod 2)
                    'If this is the very first time that a header is clicked then
                    'if clicking on the first column we need to refresh. Otherwise we don't
                    If Not ListViewHelper.GetSortedProperty(lvwDuplicatePolicyVersion) Then
                        'Do the refresh
                        ListViewHelper.SetSortedProperty(lvwDuplicatePolicyVersion, True)
                    End If

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwDuplicatePolicyVersion, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwDuplicatePolicyVersion, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwDuplicatePolicyVersion, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwDuplicatePolicyVersion, True)
                End If

            End With

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed lvwDuplicatePolicyVersion_ColumnClick", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDuplicatePolicyVersion_ColumnClick", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

        End Try
    End Sub

    Private Sub ReverseDuplicatePolicyTransaction()
        Try
            Dim sPolicyHasClaim As String = ""
            For Each oListItem As ListViewItem In Me.lvwDuplicatePolicyVersion.Items
                If oListItem.Checked Then
                    Dim sDocumentRef As String = oListItem.SubItems(4).Text
                    If sDocumentRef <> "" Then

                        If m_oBusiness.GetClaimOnPolicyVersion(CInt(oListItem.Text)) = gPMConstants.PMEReturnCode.PMTrue Then
                            sPolicyHasClaim = sPolicyHasClaim & " " & CInt(oListItem.Text) & ","
                            Continue For
                        End If

                        m_lReturn = ReversePolicyVersionTransaction(v_nInsuranceFileCnt:=CInt(oListItem.Text), v_sDocumentRef:=sDocumentRef, v_bIsCalledFromDuplicateVersionTab:=True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReversePolicyVersionTransaction()", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If

                        m_lReturn = m_oBusiness.UpdatePolicyToQuote(CInt(oListItem.Text))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception
                        End If

                    End If
                End If
            Next oListItem
            If sPolicyHasClaim <> "" Then
                sPolicyHasClaim = sPolicyHasClaim.Trim(",") & " Claim has been loaded for this policy version"
                MsgBox(sPolicyHasClaim, vbInformation)
            End If

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ReverseDuplicatePolicyTransaction ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseDuplicatePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub cmdTransactionExportRefresh_Click(sender As Object, e As EventArgs) Handles cmdTransactionExportRefresh.Click
        PopulateTransactionExport()
    End Sub

    Private Sub btnOkClonePolicyVersionsOk_Click(sender As Object, e As EventArgs) Handles btnOkClonePolicyVersionsOk.Click
        Me.btnOkClonePolicyVersionsOk.Enabled = False
        Me.cmdExit.Enabled = False
        btnSelectAllClonePolicyVersions.Enabled = False
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                If (txtPMNumber.Text.Trim() = "") Then
                    MsgBox("Please Enter Valid IM/PM number ", vbInformation)
                    Me.btnOkClonePolicyVersionsOk.Enabled = True
                    Me.cmdExit.Enabled = True
                    btnSelectAllClonePolicyVersions.Enabled = True
                    txtPMNumber.Focus()
                    Exit Sub
                End If

                If MsgBox("Are you sure you want to proceed?", vbYesNo + vbQuestion, ACApp) <> vbYes Then
                    Me.btnOkClonePolicyVersionsOk.Enabled = True
                    Me.cmdExit.Enabled = True
                    btnSelectAllClonePolicyVersions.Enabled = True
                    Exit Sub
                End If
                bIsSucesfullyCompleted = True
        ProcessClonePolicy()
        stbMain.Items.Item("COUNT").Text = CStr(Me.lvwClonePolicyVersion.Items.Count)

        If bIsSucesfullyCompleted Then
            MsgBox("Item Processed successfully", vbInformation, "Data Fix Utility")
        End If

        Me.btnOkClonePolicyVersionsOk.Enabled = True
        Me.cmdExit.Enabled = True
        btnSelectAllClonePolicyVersions.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Call btnGetClonePolicyVersion_Click(Nothing, Nothing)
    End Sub

    Private Sub btnSelectAllClonePolicyVersions_Click(sender As Object, e As EventArgs) Handles btnSelectAllClonePolicyVersions.Click
        Dim i As Integer
        If btnSelectAllClonePolicyVersions.Text = "Select All" Then
            For i = 0 To lvwClonePolicyVersion.Items.Count - 1
                lvwClonePolicyVersion.Items(i).Checked = True
            Next
            btnSelectAllClonePolicyVersions.Text = "UnSelect All"
            Exit Sub
        End If
        If btnSelectAllClonePolicyVersions.Text = "UnSelect All" Then
            For i = 0 To lvwClonePolicyVersion.Items.Count - 1
                lvwClonePolicyVersion.Items(i).Checked = False
            Next
            btnSelectAllClonePolicyVersions.Text = "Select All"
        End If
    End Sub

    Private Sub optClonePolicyReverse_CheckedChanged(sender As Object, e As EventArgs) Handles optClonePolicyReverse.CheckedChanged
        chkRIRefreshClonePolicyVersion.Enabled = False
        chkRIRefreshClonePolicyVersion.Checked = False
    End Sub

    Private Sub optClonePolicyReverseRegenerate_CheckedChanged(sender As Object, e As EventArgs) Handles optClonePolicyReverseRegenerate.CheckedChanged
        chkRIRefreshClonePolicyVersion.Enabled = True
    End Sub

    Private Sub btnGetClonePolicyVersion_Click(sender As Object, e As EventArgs) Handles btnGetClonePolicyVersion.Click
        Dim aoResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem = Nothing
        Dim sSql As String = ""
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.btnGetClonePolicyVersion.Enabled = False

            stbMain.Items.Item("MESSAGE").Text = "Getting all clone versions of this policy"

            If txtClonePolicyNumber.Text <> "" Then
                sSql = "SELECT i.insurance_file_cnt,i.insurance_ref,i.insurance_file_type_id,ift.description,d.document_ref,i.cover_start_date,document_date FROM insurance_file i "
                sSql = sSql & " INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id=i.insurance_file_type_id "
                sSql = sSql & "LEFT JOIN document d ON d.insurance_file_cnt = i.insurance_file_cnt WHERE (document_ref LIKE 'SDD%' OR document_ref LIKE 'SDR%') AND  (I.insurance_file_cnt NOT IN (select  insurance_file_cnt from   datafixutility_log) ) AND "
                sSql = sSql & "I.insurance_file_cnt NOT IN ( SELECT Insurance_File_cnt From (select insurance_file_cnt,MIN(SUBSTRING(document_ref,1,3)) AS Doc from Stats_Folder where (document_ref Like 'SDD%' OR document_ref Like 'SDR%') and insurance_file_cnt not in ("
                sSql = sSql & "SELECT DISTINCT insurance_file_cnt FROM Stats_Folder  WHERE loss_id IS not NULL) and (created_by_username = 'RIREGEN' OR created_by_username = 'sirius') Group by insurance_file_cnt Having Count(Distinct SUBSTRING(document_ref,1,3))= 1) Insurance ) AND"
                sSql = sSql & " i.insurance_ref= " & "'" & txtClonePolicyNumber.Text & "' ORDER  BY i.insurance_ref,i.insurance_file_cnt"
            Else
                sSql = txtSqlQueryForClonePolicyVersion.Text
            End If



            If InStr(1, sSql, "delete") <> 0 Or InStr(1, sSql, "update") Then
                MsgBox("Update/Delete is not allowed ", vbCritical, ACApp)
                Exit Sub
            End If

            m_lReturn = m_oBusiness.ExecuteSql(v_sSql:=sSql, r_vResultArray:=aoResultArray)


            If Not Information.IsArray(aoResultArray) Then
                lvwClonePolicyVersion.Items.Clear()
                Exit Sub
            End If

            lvwClonePolicyVersion.Items.Clear()

            For lCount As Integer = 0 To aoResultArray.GetUpperBound(1)

                'insurance file cnt
                oListItem = Me.lvwClonePolicyVersion.Items.Add(CStr(aoResultArray(ACFieldPVInsuranceFileCnt, lCount)))

                'insurance ref               
                oListItem.SubItems.Insert(ACFieldPVInsuranceRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVInsuranceRef, lCount).Trim()))

                'policy type id                
                oListItem.SubItems.Insert(ACFieldPVPolicyTypeID, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()))

                'policy type                
                oListItem.SubItems.Insert(ACFieldPVPolicyType, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyType, lCount).Trim()))

                oListItem.SubItems(ACFieldPVPolicyType).Tag = aoResultArray(ACFieldPVPolicyTypeID, lCount).Trim()

                'Document Ref               
                oListItem.SubItems.Insert(ACFieldPVDocumentRef, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentRef, lCount).Trim()))

                'policy start                
                oListItem.SubItems.Insert(ACFieldPVPolicyStart, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVPolicyStart, lCount).Trim()))

                'document Date               
                oListItem.SubItems.Insert(ACFieldPVDocumentDate, _
                                          New ListViewItem.ListViewSubItem(oListItem, aoResultArray(ACFieldPVDocumentDate, lCount).Trim()))


            Next

            If lvwClonePolicyVersion.Items.Count > 0 Then
                lvwClonePolicyVersion.Items.Item(0).Selected = True
            End If

            stbMain.Items.Item("COUNT").Text = CStr(Me.lvwClonePolicyVersion.Items.Count)
            btnSelectAllClonePolicyVersions.Text = "Select All"

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed btnGetClonePolicyVersion_Click", vApp:=ACApp, _
                                         vClass:=ACClass, vMethod:="btnGetClonePolicyVersion_Click", vErrNo:=CStr(Information.Err().Number), _
                                         vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

            Me.btnGetClonePolicyVersion.Enabled = True
            stbMain.Items.Item("MESSAGE").Text = "Ready"
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub ProcessClonePolicy()

        Dim lCount As Integer



        'For lCount = Me.optSinglePolicy.GetLowerBound(0) To Me.optSinglePolicy.GetUpperBound(0)
        '    If Me.optSinglePolicy(lCount).Checked Then
        '        Exit For
        '    End If
        'Next

        If optClonePolicyReverse.Checked Then
            lCount = 0
        ElseIf optClonePolicyReverseRegenerate.Checked Then
            lCount = 1
        End If

        Select Case lCount
            Case 0 'Reverse Transaction
                ReverseClonePolicyTransaction()
            Case 1 'Reverse & Regenerate Transaction
                ReverseAndRegenerateCloneTransaction()
        End Select

    End Sub

    Private Sub ReverseClonePolicyTransaction()
        Try

            For Each oListItem As ListViewItem In Me.lvwClonePolicyVersion.Items
                If oListItem.Checked Then
                    Dim sDocumentRef As String = oListItem.SubItems(4).Text
                    If sDocumentRef <> "" Then
                        m_lReturn = ReversePolicyVersionTransaction(v_nInsuranceFileCnt:=CInt(oListItem.Text), v_sDocumentRef:=sDocumentRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReverseClonePolicyTransaction()", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseClonePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If
                End If
            Next oListItem

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ReverseClonePolicyTransaction ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseClonePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub ReverseAndRegenerateCloneTransaction()
        Try
            Dim oControlTrans As bControlTrans.Automated
            Dim bFirstItem As Boolean = True

            Dim temp_oControlTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oControlTrans, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oControlTrans = temp_oControlTrans



            For Each oListItem As ListViewItem In Me.lvwClonePolicyVersion.Items
                If oListItem.Checked Then
                    m_oBusiness.BeginTrans()
                    Dim sDocumentRef As String = oListItem.SubItems(4).Text

                    Dim sTransactionType As String = ""
                    Select Case Convert.ToString(oListItem.SubItems.Item(ACFieldPVPolicyType).Tag)
                        Case 2 'new business
                            If sDocumentRef.StartsWith("SND") Then
                                sTransactionType = "NB"
                            Else
                                If sDocumentRef.StartsWith("SED") Then
                                    sTransactionType = "MTA"
                                Else
                                    sTransactionType = "REN"
                                End If
                            End If
                        Case 3 'renewal
                            sTransactionType = "REN"
                        Case 8
                            sTransactionType = "MTC"
                        Case 5 'MTA permenant
                            sTransactionType = "MTA"
                        Case 9 'MTA reinstatement
                            sTransactionType = "MTA"
                        Case Else
                            MessageBox.Show("Selected version is not valid for reposting", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Throw New System.Exception
                    End Select

                    If sDocumentRef.StartsWith("SID") Then
                        sTransactionType = "MTR"
                    End If

                    If chkRIRefreshClonePolicyVersion.Checked Then
                        m_lReturn = m_oBusiness.RIRefresh(nInsuranceFileCnt:=CInt(oListItem.Text), sTransactionType:=sTransactionType)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Risk", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If

                    If sDocumentRef <> "" Then
                        'Reverse Transaction
                        m_lReturn = ReversePolicyVersionTransaction(v_nInsuranceFileCnt:=CInt(oListItem.Text), v_sDocumentRef:=sDocumentRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReversePolicyVersionTransaction()", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyVersionTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                        'Regenerate Transaction
                        stbMain.Items.Item("MESSAGE").Text = "Document reference Reversed. Regenerating transactions in progress please wait"

                        m_lReturn = oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed bControlTrans.SetProcessModes()", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If

                        oControlTrans.InsuranceFileCnt = CInt(oListItem.Text)
                        '' m_lReturn = oControlTrans.Start()
                        RegenerateClonePolicyVersionTransaction(CInt(oListItem.Text), sDocumentRef)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recreate statistics and repost", vApp:=ACApp, vClass:=ACClass, vMethod:="RepostPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                            Throw New System.Exception
                        End If
                    End If

                    bFirstItem = False


                    m_lReturn = m_oBusiness.AddDataFixUtilityLog(v_sPMNumber:=txtPMNumber.Text, v_sCreatedBy:="DataFixUtility", v_lInsuranceFileCnt:=CInt(oListItem.Text), _
                                      v_sOldDocumentRef:=sDocumentRef, v_sNewDocumentid:=0, v_bIsReversal:=False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception
                    End If
                    m_oBusiness.CommitTrans()
                End If


            Next oListItem



            stbMain.Items.Item("MESSAGE").Text = ""

            If Not (oControlTrans Is Nothing) Then
                'oControlTrans.Dispose()
                oControlTrans = Nothing
            End If

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            m_oBusiness.RollbackTrans()
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed ReverseAndRegenerateCloneTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAndRegenerateCloneTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try
    End Sub

    Private Sub RegenerateClonePolicyVersionTransaction(ByVal nInsuranceFileCnt As Integer, ByVal sDocumentRef As String)
        Try
            Dim sTransactionType As String = "DRI"
            Dim sInsuranceFileType As String = "POLICY"
            Dim sDocumentType As String
            Dim IsSDDTransaction As Boolean
            If Not String.IsNullOrEmpty(sDocumentRef) Then
                sDocumentType = sDocumentRef.Substring(0, 3)
                If sDocumentType = "SDD" Then
                    IsSDDTransaction = True
                ElseIf sDocumentType = "SDR" Then
                    IsSDDTransaction = False
                End If
            End If
            If nInsuranceFileCnt > 0 Then
                m_lReturn = CreateAndPostStats(nInsuranceFileCnt:=nInsuranceFileCnt, IsSDDTransaction:=IsSDDTransaction, nClonedInsuranceFileCnt:=nInsuranceFileCnt, bReverseCloned:=sInsuranceFileType <> "POLICY", sTransactionType:=sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed CreateMissingSDR()", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMissingSDR", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                    Throw New System.Exception
                End If
            End If


        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateMissingSDR ", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMissingSDR", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        End Try
    End Sub

    Public Function CreateAndPostStats(ByVal nInsuranceFileCnt As Integer, ByVal IsSDDTransaction As Boolean, Optional ByVal nClonedInsuranceFileCnt As Integer = 0, Optional ByVal bReverseCloned As Boolean = False, Optional ByVal sTransactionType As String = "", Optional ByRef r_sMessage As String = "") As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CreateAndPostStats"
        Try
            r_sMessage = ""
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oControlTrans As bControlTrans.Automated
            Dim temp_oControlTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oControlTrans, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oControlTrans = temp_oControlTrans

            oControlTrans.InsuranceFileCnt = nInsuranceFileCnt
            oControlTrans.ClonedInsuranceFileCnt = nClonedInsuranceFileCnt
            oControlTrans.ReverseCloned = bReverseCloned
            oControlTrans.IsCloned = True
            m_lReturn = oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oBusiness.InsuranceFileCnt = nInsuranceFileCnt
            m_oBusiness.ClonedInsuranceFileCnt = nClonedInsuranceFileCnt
            m_oBusiness.ReverseCloned = bReverseCloned
            m_oBusiness.IsCloned = True
            m_oBusiness.IsSDDTransaction = IsSDDTransaction
            Dim nStatsFolderCnt As Decimal

            ' Create the Stats tables
            m_lReturn = m_oBusiness.CreateStats(nStatsFolderCnt:=nStatsFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create Statistics"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CreateAndPostStats", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function


    Private Sub CheckedDuplicateInsuranceRef(ch As Boolean)
        Try

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                If oListItem.Checked Then
                    Dim InsuranceRef As String = oListItem.SubItems(1).Text
                    CheckedOtherRefrence(InsuranceRef, ch)
                    ' Exit Sub
                End If
            Next oListItem

        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedDuplicateInsuranceRef ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub CheckedOtherRefrence(sInsurance As String, ch As Boolean)
        Try

            For Each oListItem As ListViewItem In Me.lvwPolicyVersion.Items
                Dim InsuranceRef As String = oListItem.SubItems(1).Text
                If sInsurance = InsuranceRef Then
                    If ch = True Then
                        oListItem.Checked = True
                    Else
                        oListItem.Checked = False
                    End If
                End If
            Next oListItem

        Catch ex As Exception
            bIsSucesfullyCompleted = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckedOtherRefrence ", vApp:=ACApp, vClass:=ACClass, vMethod:="ReversePolicyTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
        End Try

    End Sub

    Private Sub lvwPolicyVersion_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwPolicyVersion.ItemChecked
        'Dim x As Boolean
        'x = e.Item.Checked
        'If Not m_bisFirstSelect Then
        '    m_bisFirstSelect = True
        '    '  CheckedDuplicateInsuranceRef(x)
        '    m_bisFirstSelect = False
        'End If

    End Sub

    ' ***************************************************************** '
    ' Name: GetRiskRating
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'WPR 33-75 added commented as per WPR
    'Private Function GetRiskRating(ByRef iTask As Integer) As Integer
    'WPR 33-75 added
    Public Function GetRiskRating(ByVal sTransactionType As String) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "GetRiskRating"

        Dim lReturn As Integer
        Dim oPerilAllocation As iPMUPerilAllocation.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of peril allocation
            Dim temp_oPerilAllocation As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oPerilAllocation, sClassName:="iPMUPerilAllocation.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPerilAllocation = temp_oPerilAllocation
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of iPMUPerilAllocation.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = oPerilAllocation.SetProcessModes(vTask:=2, vTransactionType:=sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUPerilAllocation.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set peril allocation properties
            For Each oListItem As ListViewItem In Me.lvwRisk.Items
                oPerilAllocation.InsuranceFolderCnt = oListItem.SubItems(5).Text
                oPerilAllocation.InsuranceFileCnt = oListItem.SubItems(0).Text
                oPerilAllocation.RiskId = oListItem.SubItems(1).Text
                oPerilAllocation.CallingAppName = "iPMUDataFixUtility"


                oPerilAllocation.IsBackDatedMTA = False
                ' start peril allocation

                lReturn = oPerilAllocation.Start()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMUPerilAllocation.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' get the return status

                m_lStatus = oPerilAllocation.Status

                ' terminate peril allocation

                oPerilAllocation.Dispose()

                '  End If
            Next oListItem

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oPerilAllocation = Nothing


        End Try
        Return result
    End Function
End Class
