Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports MSComCtl2

Imports SharedFiles
Imports System.Data
Partial Friend Class frmRenewal
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmRenewal
    '
    ' Date: 26/09/00
    '
    ' Description: Interface for Renewals processing.
    '
    ' Edit History: CT 26/09/2000 - Created
    ' PN 16325  AG  25/11/2004  Change in ProcessAmendment function to process
    '                           iPMUChangePolicyStatus after the iPMUAgentCommission
    '                           process on the policy roadmap.
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmRenewal"

    'Thinh Nguyen 08/10/2002 start - constants for different stages of the renewal acceptance process
    Private Const ACRenFailedMakePolicyLive As String = "Failed to update policy status to " &
                                                        "live"
    Private Const ACRenFailedStatistics As String = "Failed to generate statistics"
    Private Const ACRenFailedAccumulation As String = "Failed to generate accumulation"
    Private Const ACRenFailedQuotePlan As String = "Failed to create quote plan"
    Private Const ACRenFailedScheduleDoc As String = "Failed to generate schedule document"
    Private Const ACRenFailedCertificateDoc As String = "Failed to generate certificate " &
                                                        "document"
    Private Const ACRenFailedCreateEvent As String = "Failed to create event"
    'Thinh Nguyen 08/10/2002 end - constants for different stages of the renewal acceptance process
    'DN 21/02/03
    Private Const ACRenFailedDebitNoteDoc As String = "Failed to generate debit note"
    Private Const ACPolicyLockName As String = "Insurance_folder_cnt"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    'Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_sInsuranceRef As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_vRenewalsData(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer
    ' IsRenewalAmmend
    Private m_lRunMode As Integer
    Private m_lProductId As Integer
    Private m_lAgentId As Integer 'Deepak
    Private m_sRenewalDate As String = ""
    Private m_bMultiSelect As Boolean
    Private m_sRenewalPolicyNumber As String = ""
    Private m_sRenewalStartDate As String = ""
    Private m_sRenewalExpiryDate As String = ""
    Private m_bPolicyDetailsChanged As Boolean

    Private m_lLeadAgentCnt As Integer 'if this is set we will only pick up those records in renewal with lead_agent = m_lLeadAgentCnt and renewal_status_type_id = awaiting broker transfer

    Private m_lBusinessTypeId As Integer

    'Thinh Nguyen 20/02/2002(start)
    Private m_lSourceID As Integer
    'Thinh Nguyen 20/02/2002(end)
    Private m_sRenSchedulePrinting As String = "" 'option number 1036
    Private m_sRenCertificatePrinting As String = "" 'option number 1037
    Private m_sRenDebitNotePrinting As String = "" 'option number 1038

    Private m_bCanTransferBroker As Boolean

    'Error return for renewal that has already been accepted
    Const PM_FAILED_RENEWAL_STATUS As Integer = 60132

    Dim frmChangeStatus As frmChangeStatus
    Dim frmFilterRenewal As frmFilterRenewal
    Dim frmLapseRenewal As frmLapseRenewal
    Dim frmChangePolicyDetails As frmChangePolicyDetails
    Private m_oPremiumFinance As Object

    Dim m_oAccount As Object
    Dim m_oBusiness As Object
    Dim m_sUnderwritingOrAgency As String = "" 'deepak
    Dim sSimpleTest As String = ""
    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions(,) As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Dim lNewPolicyCnt As Integer
    Private m_vAllowPayNowOption As String = ""
    Private m_lSelNewPolicyCnt As Integer
    Private m_lSelPartyCnt As Integer
    Private m_bDontDeleteScheme As Boolean
    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    Dim m_cRoundOffAmount As Decimal
    Private m_bRoundOff As Boolean

    Private m_oProduct As Object
    Dim m_oPrepayment(,) As Object

    Public WriteOnly Property Source() As Integer
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    Public Property RenewalDate() As String
        Get
            Return m_sRenewalDate
        End Get
        Set(ByVal Value As String)
            m_sRenewalDate = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property AgentId() As Integer
        Get
            Return m_lAgentId
        End Get
        Set(ByVal Value As Integer)
            m_lAgentId = Value
        End Set
    End Property

    Public WriteOnly Property LeadAgentCnt() As Integer
        Set(ByVal Value As Integer)
            m_lLeadAgentCnt = Value
        End Set
    End Property

    Public WriteOnly Property RunMode() As Integer
        Set(ByVal Value As Integer)
            m_lRunMode = Value
        End Set
    End Property

    Private Sub cmdAccept_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAccept.Click

        Dim vHideSummaryAtRenewalAcceptance As String = ""
        Dim lNumberSelected As Integer
        Dim bAllowed As Boolean

        Dim m_iAllPolicyPayNow As Integer
        Try

            'Check if there are any items selected.
            lNumberSelected = ListViewSelected(lvwRenewals)

            If lNumberSelected < 1 Then
                If lNumberSelected = -1 Then
                    MessageBox.Show("Failed to count selected items on listview", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No item selected", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Exit Sub
            End If
            m_lReturn = CheckJobBatchRenewalInProcess()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            m_bMultiSelect = (lNumberSelected > 1)
            If m_bMultiSelect Then
                ' Header is changed from Application.ProductName to "Renewal"
                ' MessageBox.Show("Agent Selected is a Commission Account. Policies must be selected individually.", "Renewal", MessageBoxButtons.OK)
                m_lReturn = AllPolicyPayNow(m_iAllPolicyPayNow)
                If m_iAllPolicyPayNow = 2 Then
                    MessageBox.Show("Selected Batch have Different Pay term Options. " &
                                    "Choose Another Batch.", "Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                'Start - Prakash - WPR85_Paralleling
                If m_iAllPolicyPayNow = 3 Then
                    MessageBox.Show("Batch renewal is not supported for Cash Deposit." &
                                    " Choose Another Batch.", "Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                'End - Prakash - WPR85_Paralleling

                If m_iAllPolicyPayNow = 1 Then
                    'PN:71818

                    If m_lAgentId = 0 And lvwRenewals.FocusedItem.SubItems(3).Text <> "Direct" Then
                        MessageBox.Show("Renewal Batch Acceptance can't be done as Agent filtering not applied.", "Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                    ' End If

                    'Check for Agent Type (if Commission Agent Donot allow to proceed
                    'Check for Currency (if different currentcy used then donot allow to proceed

                    m_lReturn = CheckCurrencyAndAgentType(bAllowed:=bAllowed)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    If Not bAllowed Then
                        Exit Sub
                    End If

                End If
            End If
            'If we are accepting a single renewal, then offer chance to
            'change some policy details.
            m_bPolicyDetailsChanged = False
            If Not m_bMultiSelect Then

                If lvwRenewals.FocusedItem.SubItems(9).Text = "1" Then
                    'Policy is attached to a closed branch and cannot be accepted

                    MessageBox.Show("Unable to proceed - this Policy is attached to a branch that" & " is closed. Please Amend " & lvwRenewals.FocusedItem.SubItems(2).Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, Convert.ToString(lvwRenewals.FocusedItem.Tag))), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    MessageBox.Show("Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                    "Please contact the System Administrator", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                m_lReturn = g_oRenewal.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="hide_summary_at_renewal_acceptance", v_sKeyColumn:="product_id", v_sKeyValue:=m_vRenewalsData(ACIRenewalProductId, Convert.ToString(lvwRenewals.FocusedItem.Tag)), v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vHideSummaryAtRenewalAcceptance)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to get (Hide_Summary_At_Renewal_Acceptance) flag", Application.ProductName, MessageBoxButtons.OK)
                    Exit Sub
                End If

                If vHideSummaryAtRenewalAcceptance <> "1" Then
                    m_lReturn = ChangePolicyDetails()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If
            End If

            m_lReturn = ProcessAccept()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept the renwal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If
            m_lReturn = DisplayAccountDetails()

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAccept", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdAmmend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAmmend.Click

        Try

            ' Check if there are any items available.
            If lvwRenewals.Items.Count = 0 Then
                Exit Sub
            End If
            m_lReturn = ProcessAmendment()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to amend the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmmend", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ammend the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmmend", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdChangeStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChangeStatus.Click

        Dim lInsuranceFileCnt, lSelected, lSelectedRecord As Integer
        Dim bRedisplay As Boolean
        Dim iNewRenewalStatusTypeID As Integer
        Dim sFailmsg, sCreditControlEnabled As String

        Try

            If Me.lvwRenewals.Items.Count < 1 Then
                Exit Sub
            End If

            'how many did we select?
            lSelectedRecord = ListViewSelected(lvwRenewals)

            If lSelectedRecord < 1 Then
                Exit Sub
            End If

            'get new status
            frmChangeStatus.ShowDialog()

            If frmChangeStatus.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            If MessageBox.Show("All selected records will be set to new status." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            iNewRenewalStatusTypeID = frmChangeStatus.RenewalStatusTypeID
            bRedisplay = False

            For lCount As Integer = 1 To lvwRenewals.Items.Count
                If lvwRenewals.Items.Item(lCount - 1).Selected Then
                    lSelected = Convert.ToString(lvwRenewals.Items.Item(lCount - 1).Tag)
                    lInsuranceFileCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected))

                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lSelected)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                        MessageBox.Show("Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Please contact the System Administrator", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return
                        End If
                        If SetStatus(lRenewalCnt:=lInsuranceFileCnt, iRenewalStatus:=iNewRenewalStatusTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                            sFailmsg = "Failed to set policy " & CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)) & " to new renewal status type id"
                            Throw New Exception()
                        End If

                        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5001, r_sOptionValue:=sCreditControlEnabled)

                        If sCreditControlEnabled = "1" Then
                            'If credit control enabled
                            If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lSelected)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitUpdate And iNewRenewalStatusTypeID <> gPMConstants.PMBRenewalStatusTypeAwaitUpdate Then

                                If m_oBusiness.DeleteCreditControlItem(v_lInsuranceFileCnt:=lInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    sFailmsg = ""
                                    Throw New Exception()
                                End If
                            End If
                        End If
                        ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                        bRedisplay = True
                    End If
                End If
            Next

            If bRedisplay Then
                m_lReturn = ShowRenewals()
            End If

        Catch excep As System.Exception

            If sFailmsg = "" Then
                sFailmsg = "cmdChangeStatus_Click Failed"
            End If

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailmsg, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdChangeStatus_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim lSelectedRecord, lSelected As Integer
        Dim bRedisplay As Boolean

        Try

            ' Check if there are any items available.
            If lvwRenewals.Items.Count = 0 Then
                Exit Sub
            End If

            'how many did we select?
            lSelectedRecord = ListViewSelected(lvwRenewals)

            If lSelectedRecord < 1 Then
                Exit Sub
            End If

            If MessageBox.Show("Delete the selected " & (IIf(lSelectedRecord > 1, "renewals.", "renewal.")) & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to Continue?", "Renewals", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If

            bRedisplay = False
            For lCount As Integer = 1 To lvwRenewals.Items.Count
                If lvwRenewals.Items.Item(lCount - 1).Selected Then
                    lSelected = Convert.ToString(lvwRenewals.FocusedItem.Tag)
                    bRedisplay = True
                    m_lReturn = ProcessDelete(lSelected)
                End If
            Next

            If bRedisplay Then
                m_lReturn = ShowRenewals()
            End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFilter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFilter.Click

        frmFilterRenewal.ShowDialog()

        'If filter params have changed then refresh.
        'sj 07/11/2002 - Add filter for source id
        If frmFilterRenewal.Status <> gPMConstants.PMEReturnCode.PMCancel Then
            If (m_lProductId <> frmFilterRenewal.ProductId) Or (m_sRenewalDate <> frmFilterRenewal.RenewalDate) Or (m_lSourceID <> frmFilterRenewal.SourceID) Or (m_lAgentId <> frmFilterRenewal.AgentId) Then

                m_lProductId = frmFilterRenewal.ProductId
                m_sRenewalDate = frmFilterRenewal.RenewalDate
                m_lAgentId = frmFilterRenewal.AgentId

                'Thinh Nguyen 20/03/2002 (start)
                m_lSourceID = frmFilterRenewal.SourceID
                'Thinh Nguyen 20/03/2002 (end)
                m_lReturn = ShowRenewals()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            End If
        End If

    End Sub

    Private Sub cmdLapse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLapse.Click
        Try

            ' Check if there are any items available.
            If lvwRenewals.Items.Count = 0 Then
                Exit Sub
            End If

            m_lReturn = ProcessLapse()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLapse_click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If Not frmLapseRenewal Is Nothing Then
                frmLapseRenewal.Close()
                frmLapseRenewal = Nothing
            End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdLapse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    'Private Sub Command4_Click()
    '
    'End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'nothing to commit or cancel as we have been doing that as we went
        'when we choose ammend, lapse or delete.

        Me.Close()

    End Sub

    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click

        For iItemCount As Integer = 1 To lvwRenewals.Items.Count
            lvwRenewals.Items.Item(iItemCount - 1).Selected = True
        Next iItemCount
        m_bMultiSelect = True
        'sj 07/11/2002 - start
        EnableDisableButtons()
        'sj 07/11/2002 - end
    End Sub

    Private Sub cmdTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransfer.Click

        Dim lSelected As Integer
        Dim vTransferPolicy(,) As Object
        Dim lTransferPolicyMax As Integer
        Dim sReportTitle, sRegPath, TransferBrokerReportFileName As String
        Dim sReportText As New StringBuilder
        Dim lArrayIndex As Integer

        Try

            If lvwRenewals.Items.Count = 0 Then
                Exit Sub
            End If

            'get number of records selected on listview
            lSelected = ListViewSelected(lvwRenewals)

            If lSelected < 1 Then
                Exit Sub
            End If

            'get path from registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Not sRegPath.EndsWith("\") Then
                sRegPath = sRegPath & "\"
            End If

            'get report file name including path
            TransferBrokerReportFileName = sRegPath & DateTime.Now.ToString("yyyyMMddHHMMss") &
                                           "Renewal_Broker_XFER_" & g_oObjectManager.UserName & ".log"

            'report title
            sReportTitle = "Renewal Broker Transfer Status Report - " & StringsHelper.Format(DateTime.Now, "dd mmmm " &
                           "yyyy hh:mm:ss AMPM") & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            lTransferPolicyMax = -1
            For lCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(lCount - 1).Selected Then

                    lArrayIndex = Convert.ToString(lvwRenewals.Items.Item(lCount - 1).Tag)

                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lArrayIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                        m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lArrayIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lArrayIndex)).Trim())
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Continue For
                        End If

                        If g_oRenewal.TransferBroker(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, lArrayIndex)), v_lTransferToPartyCnt:=gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalTransferToPartyCnt, lArrayIndex)))) = gPMConstants.PMEReturnCode.PMTrue Then

                            If Not Information.IsArray(vTransferPolicy) Then
                                ReDim vTransferPolicy(2, 0)
                            Else

                                ReDim Preserve vTransferPolicy(2, vTransferPolicy.GetUpperBound(1) + 1)
                            End If


                            lTransferPolicyMax = vTransferPolicy.GetUpperBound(1)


                            vTransferPolicy(0, lTransferPolicyMax) = m_vRenewalsData(ACIRenewalPolicyCnt, lArrayIndex)

                            vTransferPolicy(1, lTransferPolicyMax) = lCount

                            vTransferPolicy(2, lTransferPolicyMax) = lArrayIndex

                            sReportText.Append(CStr(m_vRenewalsData(ACIRenewalLivePolicy, lArrayIndex)).Trim() & " - successfuly transfer broker" & Strings.Chr(13) & Strings.Chr(10))
                        Else
                            sReportText.Append(CStr(m_vRenewalsData(ACIRenewalLivePolicy, lArrayIndex)).Trim() & " - failed to transfer broker" & Strings.Chr(13) & Strings.Chr(10))
                        End If
                        ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lArrayIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lArrayIndex)).Trim())
                    End If
                End If
                If m_lAgentId > 0 Then
                    'developer guide no. 26
                    stbStatus.Items.Item(0).Text = "Transferred " & (IIf(lTransferPolicyMax = -1, CStr(0), CStr(lTransferPolicyMax + 1))) & " of " & CStr(lSelected) & (IIf(lSelected > 1, " policies", " policy"))
                    lblStatus.Text = "Transferred " & (IIf(lTransferPolicyMax = -1, CStr(0), CStr(lTransferPolicyMax + 1))) & " of " & CStr(lSelected) & (IIf(lSelected > 1, " policies", " policy"))
                    'stbStatus.Refresh()
                    lblStatus.Refresh()
                Else
                    stbStatus.Text = "Transferred " & (IIf(lTransferPolicyMax = -1, CStr(0), CStr(lTransferPolicyMax + 1))) & " of " & CStr(lSelected) & (IIf(lSelected > 1, " policies", " policy"))
                    lblStatus.Text = "Transferred " & (IIf(lTransferPolicyMax = -1, CStr(0), CStr(lTransferPolicyMax + 1))) & " of " & CStr(lSelected) & (IIf(lSelected > 1, " policies", " policy"))
                    'stbStatus.Refresh()
                    lblStatus.Refresh()
                End If
            Next lCount

            'redisplay listview with updated details
            If lTransferPolicyMax <> -1 Then
                For lCount As Integer = 0 To lTransferPolicyMax



                    RepopulateRenewal(CInt(vTransferPolicy(0, lCount)), CInt(vTransferPolicy(1, lCount)), CInt(vTransferPolicy(2, lCount)))
                Next

                lvwRenewals.Refresh()
                lvwRenewals.FullRowSelect = True
            End If

            If sReportText.ToString() <> "" Then
                'add in report title
                sReportText = New StringBuilder(sReportTitle & sReportText.ToString())

                'create text file of policies in this batch
                m_lReturn = AppendText(v_sFile:=TransferBrokerReportFileName, v_sTextLine:=sReportText.ToString())

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create renewal acceptance report file", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If

                m_lReturn = SpoolDoc(v_sFileName:=TransferBrokerReportFileName, v_sSpoolDesc:="Renewal Broker Transfer Status Report")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool renewal broker transfer report file", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub
                End If
            End If

            If lTransferPolicyMax <> -1 Then
                MessageBox.Show("Successfully transferred " & (IIf(lTransferPolicyMax = -1, CStr(0), CStr(lTransferPolicyMax + 1))) & (IIf(lSelected > 1, " policies", " policy")), "Policy Transfer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to transfer broker", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTransfer_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            '    ' Create business  object
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''            oObject:=g_oRenewal, _
            ''            sClassName:="bSirRenewal.Business", _
            ''            vInstanceManager:=PMGetViaClientManager)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get an instance of the business object.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Display error stating the problem.
            '
            '        ' Get description from the resource file.
            '        sTitle$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lId:=ACBusinessFailTitle, _
            ''                iDataType:=PMResString)
            '
            '        sMessage$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lId:=ACBusinessFail, _
            ''                iDataType:=PMResString)
            '
            '        ' Display message.
            '        MsgBox sMessage$, vbCritical, sTitle$
            '
            '        Exit Sub
            '    End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmRenewal_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            frmChangeStatus = New frmChangeStatus
            frmFilterRenewal = New frmFilterRenewal
            frmLapseRenewal = New frmLapseRenewal

            iPMFunc.ShowFormInTaskBar_Detach()
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'RWH(03/11/2000) Moved into interface class.
            '    'RWH(04/10/2000)
            '    If (m_bIsRenewalAmmend = False) Then
            '        SetMousePointer PMMouseNormal
            '        frmFilterRenewal.Show vbModal
            '        SetMousePointer PMMouseBusy
            '
            '        m_lProductId = frmFilterRenewal.ProductId
            '        m_sRenewalDate = frmFilterRenewal.RenewalDate
            '
            '    End If

            m_lReturn = ShowRenewals()

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwRenewals.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vRenewalsData) Then
                Return result
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vRenewalsData.GetLowerBound(1) To m_vRenewalsData.GetUpperBound(1)
                ' Assign the details to the first column.
                m_lItemsFound += 1

                'Col 1 branch
                oListItem = lvwRenewals.Items.Add(CStr(m_vRenewalsData(ACIRenewalSourceCode, lRow)).Trim())

                'col 2 client
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRenewalsData(ACIRenewalShortname, lRow)).Trim()

                'col 3 policy no
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRenewalsData(ACIRenewalLivePolicy, lRow)).Trim()

                'col 4 agent
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vRenewalsData(ACIRenewalLeadAgentCode, lRow)).Trim()

                'col 5 account handler
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vRenewalsData(ACIRenewalAccHandlerCode, lRow)).Trim()

                'col 6 renewal date
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vRenewalsData(ACIRenewalCoverStartDate, lRow)).Trim()

                'col 7 status
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vRenewalsData(ACIRenewalStatusType, lRow)).Trim()

                'col 8 Exception Reason
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vRenewalsData(ACIRenewalExceptionReason, lRow)).Trim()

                'col 9 product
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(m_vRenewalsData(ACIRenewalProduct, lRow)).Trim()

                'col 10 claim indicator
                ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(m_vRenewalsData(ACIRenewalClaimsIndicator, lRow)).Trim()

                'col 11 close branch
                ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(m_vRenewalsData(ACIRenewalClosedBranch, lRow)).Trim()

                'col 12 transfer broker to
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lRow)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalTransferToPartyCnt, lRow)), 0) = 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, 11).Text = "Direct"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 11).Text = CStr(m_vRenewalsData(ACIRenewalTransferToShortname, lRow)).Trim()
                    End If
                End If

                ' indicate which versions of the renewals are anniversary copies
                If gPMFunctions.ToSafeDouble(m_vRenewalsData(ACIRenewalAnniversaryCopy, lRow)) = 1 Then
                    'developer guide no. 274
                    ListViewHelper.SetListItemSmallIconProperty(oListItem, "Anniversary")
                Else

                    ListViewHelper.SetListItemSmallIconProperty(oListItem, "Policy")
                End If

                oListItem.Tag = CStr(lRow)

            Next lRow

            'Refresh the initial results.

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowRenewals
    '
    ' Description: shows/refreshes renewals
    ' ***************************************************************** '
    Public Function ShowRenewals() As Integer
        Dim result As Integer = 0
        Try

            'Display a searching message.
            DisplayStatusSearching()

            m_lReturn = GetBusiness(m_vRenewalsData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst obtaining the renewal details", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured whilst displaying the renewal details", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'sj 07/11/2002 - start
            EnableDisableButtons(v_bDisableAll:=True)
            'sj 07/11/2002 - end

            'Display a searching message.
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' return success unless failed...
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            SetStatusBar()
            DisplayAccountDetails()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessAmmendment
    '
    ' Description: Displays renewal for ammendment and then goes though subsequent
    '              renewal ammendment processing.
    ' ***************************************************************** '
    Public Function ProcessAmendment() As Integer
        Dim result As Integer = 0
        Dim lSelected As Integer
        Dim sMessage As String = ""
        Dim sPartyType As String = ""
        Dim lPolicycnt As Integer
        Dim sRerateFailReason As String = ""
        Dim iStatus As gPMConstants.PMEReturnCode
        Dim vKeys(,) As Object
        Dim sFailureMessage As String = ""
        Dim vPlanArray(,) As Object
        Dim bContinue, bReDisplayList As Boolean
        Dim vRenewal_Product_id As String = ""
        Dim oFindInsuranceBusiness As Object
        Dim bInsuranceFileExists As Boolean
        Dim sLockedBy As String = ""
        Dim bLocked As Boolean = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            bReDisplayList = False
            Dim sPaymentMethod As String = ""
            For lCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                bContinue = True

                If lvwRenewals.Items.Item(lCount - 1).Selected Then

                    lSelected = Convert.ToString(lvwRenewals.Items.Item(lCount - 1).Tag)
                    lPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected))

                    If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lSelected)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                        MessageBox.Show("Agent/Broker for this policy " & CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim() & " is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Please contact the System Administrator", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        bContinue = False
                    End If

                    If bContinue Then
                        If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, lSelected)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitUpdate Then
                            bContinue = (MessageBox.Show("Policy is ready to be accepted." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to amend this policy?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes)
                        End If
                    End If
                    If bContinue Then
                        'Check for the existance of Policy versions.
                        m_lReturn = g_oObjectManager.GetInstance(oObject:=oFindInsuranceBusiness,
                                            sClassName:="bSIRFindInsurance.Form",
                                            vInstanceManager:=PMGetViaClientManager)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            ProcessAmendment = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRFindInsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmmendment", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Exit Function
                        End If

                        m_lReturn = oFindInsuranceBusiness.CheckInsuranceFileExistance(v_lInsuranceFileCnt:=ToSafeLong(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected), 0),
                                                          bInsuranceFileExists:=bInsuranceFileExists)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And bInsuranceFileExists = False Then
                            MsgBox("Policy version has been removed.", vbOKOnly + vbCritical, "Policy Version not Found")
                            bContinue = False
                        End If
                    End If

                    If bContinue Then
                        m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                        bLocked = True
                        'RWH(14/02/2001) Run Policy details component which will also return
                        'business_type_id for use in coinsurance.
                        m_lReturn = RunPolicy(iStatus, lSelected)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage("iPMUPolicy")
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If (iStatus = gPMConstants.PMEReturnCode.PMCancel) Or (iStatus = gPMConstants.PMEReturnCode.PMError) Then
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return result
                        End If

                        'make sure the agent_commission link is deleted if this version is direct business

                        If g_oRenewal.DeleteAgentcommission(v_lInsuranceFileCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, lSelected), r_sFailMessage:=sFailureMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show(sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'user might have changed policy details...we need to update Renewal Status table with these info

                        If g_oRenewal.UpdateRenewalStatus(v_lRenewalStatusCnt:=m_vRenewalsData(ACIRenewalStatusId, lSelected), r_sMessage:=sFailureMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show(sFailureMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ReDim vKeys(1, 3)

                        vKeys(0, 0) = "insurance_file_cnt"

                        vKeys(1, 0) = m_vRenewalsData(ACIRenewalPolicyCnt, lSelected)

                        vKeys(0, 1) = "insurance_folder_cnt"

                        vKeys(1, 1) = m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected)

                        vKeys(0, 2) = "insurance_ref"

                        vKeys(1, 2) = m_vRenewalsData(ACIRenewalPolicy, lSelected)

                        vKeys(0, 3) = "business_type_id"

                        vKeys(1, 3) = m_lBusinessTypeId


                        m_lReturn = RunProcess("iPMUCoinsurance.NavigatorV3", vKeys, iStatus)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage("iPMUCoinsurance")
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If (iStatus = gPMConstants.PMEReturnCode.PMCancel) Or (iStatus = gPMConstants.PMEReturnCode.PMError) Then
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return result
                        End If

                        'Start Renuka PN 61715
                        ReDim vKeys(1, 3)
                        'End Renuka PN 61715

                        vKeys(0, 0) = "insurance_file_cnt"

                        vKeys(1, 0) = m_vRenewalsData(ACIRenewalPolicyCnt, lSelected)

                        vKeys(0, 1) = "insurance_folder_cnt"

                        vKeys(1, 1) = m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected)

                        vKeys(0, 2) = "shortname"

                        vKeys(1, 2) = gPMFunctions.ToSafeString(CStr(m_vRenewalsData(ACIRenewalShortname, lSelected)), "").Trim()
                        'Start Renuka PN 61715

                        vKeys(0, 3) = "original_insurance_file_cnt"

                        vKeys(1, 3) = m_vRenewalsData(ACIRenewalLivePolicyCnt, lSelected)
                        'End Renuka PN 61715
                        '5-Nov
                        Dim vResultArray(,) As Object
                        Dim m_iPolicyMakeLiveStatus As Integer
                        'End 5-Nov
                        m_lReturn = RunProcess("iPMUListRisks.NavigatorV3", vKeys, iStatus, , vResultArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            DisplayMessage("iPMUListRisks")
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If (iStatus = gPMConstants.PMEReturnCode.PMCancel) Or (iStatus = gPMConstants.PMEReturnCode.PMError) Then
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return result
                        End If

                        '5-nov
                        If Information.IsArray(vResultArray) Then

                            For iLoop As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                                If CStr(vResultArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)) = PMNavKeyConst.PMKeyNamePolicyMakeLiveStaus Then

                                    m_iPolicyMakeLiveStatus = gPMFunctions.ToSafeInteger(CStr(vResultArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)), 0)
                                End If
                            Next iLoop
                        End If
                        'end 5-nov
                        'wr25 1.12
                        'Fetch the value for Renewal product

                        m_lReturn = g_oRenewal.GetValueFromTable(v_sTableName:="Insurance_File", v_vReturnColumn:="Renewal_product_id", v_sKeyColumn:="insurance_file_cnt", v_sKeyValue:=m_vRenewalsData(ACIRenewalPolicyCnt, lSelected), v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vRenewal_Product_id)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            MessageBox.Show("Failed to get (Renewal_product_id) value", "Renewal Amendment Process", MessageBoxButtons.OK)
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                            Return result
                        End If

                        'if renewal product got changed at renewal process then delete the policy from renewal
                        If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalRENProductId, lSelected))) <> gPMFunctions.ToSafeLong(vRenewal_Product_id) And gPMFunctions.ToSafeString(vRenewal_Product_id).Trim() <> "" Then

                            m_lReturn = g_oRenewal.DeletePolicyFromRenewal(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Failed to delete policy from renewal", "Renewal Amendment Process", MessageBoxButtons.OK)
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                Return result
                            End If

                        Else

                            ReDim vKeys(1, 1)

                            vKeys(0, 0) = "insurance_file_cnt"

                            vKeys(1, 0) = m_vRenewalsData(ACIRenewalPolicyCnt, lSelected)

                            vKeys(0, 1) = "ShowInterface"

                            vKeys(1, 1) = gPMConstants.PMEReturnCode.PMFalse

                            m_lReturn = RunProcess("iPMUChangePolicyStatus.NavigatorV3", vKeys, iStatus)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                DisplayMessage("iPMUChangePolicyStatus")
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            If (iStatus = gPMConstants.PMEReturnCode.PMCancel) Or (iStatus = gPMConstants.PMEReturnCode.PMError) Then
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                Return result
                            End If
                            'Start-(Arul Stephen)-(PN Fixing-PN 59278)

                            m_lReturn = m_oBusiness.GetRenewalPaymentMethod(v_lInsuranceFileCnt:=vKeys(1, 0), r_sPaymentMethod:=sPaymentMethod)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                DisplayMessage("GetRenewalPaymentMethod Method Failed")
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If sPaymentMethod.Trim().ToUpper() <> "INVOICE" And sPaymentMethod.Trim().ToUpper() <> "PAYNOW" And sPaymentMethod.Trim().ToUpper() <> "BANKGUARANTEE" Then

                                'End-(Arul Stephen)-(PN Fixing-PN 59278)

                                'quote version should have been created at renewal selection, but just in case something went wrong
                                'do we have instalment on current policy version

                                m_lReturn = g_oRenewal.IsInstalment(v_lInsuranceFileCnt:=m_vRenewalsData(ACIRenewalLivePolicyCnt, lSelected))

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    m_bDontDeleteScheme = False
                                    'create quote plan for renewal version ( if its not already there) now that we have plan on current version
                                    m_lReturn = CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, lSelected)), v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected)), v_lPartyCnt:=CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, lSelected)), r_vPlanArray:=vPlanArray, r_sFailureMessage:=sFailureMessage)
                                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                        'launch plan maintenance to get bank account number and bank account name (Transaction button is disable - can only save details)
                                        'run agent commission - (use same keyarray() as change policy status)

                                        If CStr(vPlanArray(43, 0)).Trim() = "" Then


                                            m_lReturn = RunPlanMaintenance(v_lFinancePlanCnt:=CInt(vPlanArray(0, 0)), v_lFinancePlanVersion:=CInt(vPlanArray(1, 0)), r_sFailureMessage:=sFailureMessage)
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                result = gPMConstants.PMEReturnCode.PMFalse
                                                MessageBox.Show(sFailureMessage, "Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                                Return result
                                            End If
                                        End If
                                        ' if there is no valid instalment plan available the renewal will be quoted as
                                        ' pay by invoice - find out if the user wants to continue on instalments
                                        ' or if pay by invoice is acceptable...
                                    Else
                                        If MessageBox.Show("There is no valid renewal based instalment scheme for this policy. If you choose to continue this policy will be paid by invoice." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to Continue?", "Renewal", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                                            ' carry on as per normal
                                        Else
                                            '    ProcessAmendment = PMFalse
                                            '    MsgBox sFailureMessage, vbOKOnly + vbInformation, "Renewal"
                                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                            Return result
                                        End If
                                    End If

                                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    MessageBox.Show("Failed to check if current policy version has instalment Policy ID " & CStr(m_vRenewalsData(ACIRenewalPolicyCnt, lSelected)), "Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                    Return result
                                End If
                                'Start-(Arul Stephen)-(PN Fixing-PN 59278)
                            End If
                            'End-(Arul Stephen)-(PN Fixing-PN 59278)
                            'Niit 22 oct 2012
                            Dim vBindRenewalWithoutInvitation As String = ""

                            m_lReturn = g_oRenewal.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="bind_renewal_without_invitation", v_sKeyColumn:="product_id", v_sKeyValue:=m_vRenewalsData(ACIRenewalProductId, lSelected), v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vBindRenewalWithoutInvitation)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("Failed to get (bind_renewal_without_invitation) value", "Renewal Amendment Process", MessageBoxButtons.OK)
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                Return result
                            End If
                            '5-nov
                            If m_iPolicyMakeLiveStatus <> 0 Then
                                'end 5-nov
                                If vBindRenewalWithoutInvitation = "1" Then
                                    'rerated and was successful so now awaiting printing
                                    m_lReturn = SetStatus(lPolicycnt, iRenewalStatus:=ACAwaitUpdate)
                                Else
                                    'Niit 22 oct 2012 end
                                    'rerated and was successful so now awaiting printing
                                    m_lReturn = SetStatus(lPolicycnt, iRenewalStatus:=ACAwaitRenewalPrint)
                                    'Niit 22 oct 2012
                                End If
                                'Niit 22 oct 2012 End
                                '5-nov
                            End If
                            'end 5-nov
                            'Niit 22 oct 2012                          

                            ' Check for errors.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                        bReDisplayList = True
                        ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, lSelected)).Trim())
                    End If
                End If
            Next

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If bReDisplayList Then
                m_lReturn = ShowRenewals()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the renewal ammendment", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmendment", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' ProcessLapse
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessLapse() As Integer
        Dim nResult As Integer = 0

        Dim nSelected As Integer
        Dim sMessage As String = ""
        Dim nStatusID As Integer
        Dim nPolicycnt As Integer
        Dim sInsuranceRef As String = ""
        Dim nRenewalStatus As Integer
        Dim nLivePolicyCnt As Integer
        Dim nPartyCnt As Integer
        Dim nInsuranceFolderCnt As Integer
        Dim dtRenPolDetails As New DataTable

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
            If IsNothing(frmLapseRenewal) Then
                frmLapseRenewal = New frmLapseRenewal
            End If

            frmLapseRenewal.ShowDialog()

            'only carry on if they chose a reason
            If frmLapseRenewal.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return nResult
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If (Not m_bMultiSelect) And (lvwRenewals.FocusedItem.Index >= 0) Then

                If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, Convert.ToString(lvwRenewals.FocusedItem.Tag))), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    MessageBox.Show("Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                    "Please contact the System Administrator", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Return nResult
                End If

                'get selected values

                nSelected = Convert.ToString(lvwRenewals.FocusedItem.Tag)
                nStatusID = CInt(m_vRenewalsData(ACIRenewalStatusId, nSelected))
                nPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, nSelected))
                nRenewalStatus = CInt(m_vRenewalsData(ACIRenewalStatusTypeId, nSelected))
                nLivePolicyCnt = CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, nSelected))
                nPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, nSelected))
                nInsuranceFolderCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected))

                m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
                m_lReturn = g_oRenewal.GetRenewalPolicyDetails(v_lInsuranceFileCnt:=nPolicycnt, dtResult:=dtRenPolDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessLapse = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                If (dtRenPolDetails IsNot Nothing AndAlso dtRenPolDetails.Rows.Count > 0) Then
                    If MsgBox("Please confirm you wish to lapse this policy. You will need to recapture or re-migrate it if required later.",
                             vbOKCancel, "Lapsed Policy") = vbCancel Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Function
                    End If
                End If

                m_lReturn = g_oRenewal.LapseRenewal(v_lRenewalCnt:=nPolicycnt, v_lLivePolicyCnt:=nLivePolicyCnt, v_lStatusId:=nStatusID, v_lReasonID:=g_lReasonID, v_sReasonDesc:=g_sReasonDesc, v_lPartyCnt:=nPartyCnt, v_lInsFolderCnt:=nInsuranceFolderCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return nResult
                End If

                m_lReturn = GenerateDocument(ACDocTypeLapse, ACSpoolSilentMode, nLivePolicyCnt, nInsuranceFolderCnt, nPartyCnt, "Lapse Renewal")
                ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())

            ElseIf (m_bMultiSelect) Then

                'Remove old details for each renewal to be accepted.
                'Step backwards thru' list so we can remove items as we go.
                For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                    If lvwRenewals.Items.Item(iListCount - 1).Selected Then

                        nSelected = Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag)

                        If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, nSelected)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                            MessageBox.Show("Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please contact the System Administrator", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return nResult
                        End If

                        nStatusID = CInt(m_vRenewalsData(ACIRenewalStatusId, nSelected))
                        nPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, nSelected))
                        nRenewalStatus = CInt(m_vRenewalsData(ACIRenewalStatusTypeId, nSelected))
                        nLivePolicyCnt = CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, nSelected))
                        nPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, nSelected))
                        nInsuranceFolderCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected))

                        m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If
                        m_lReturn = g_oRenewal.LapseRenewal(v_lRenewalCnt:=nPolicycnt, v_lLivePolicyCnt:=nLivePolicyCnt, v_lStatusId:=nStatusID, v_lReasonID:=g_lReasonID, v_sReasonDesc:=g_sReasonDesc, v_lInsFolderCnt:=nInsuranceFolderCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        m_lReturn = GenerateDocument(ACDocTypeLapse, ACSpoolSilentMode, nLivePolicyCnt, nInsuranceFolderCnt, nPartyCnt, "Lapse Renewal")
                        ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, nSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, nSelected)).Trim())


                    End If
                Next iListCount
            End If

            ShowRenewals()
            g_lReasonID = 0

            frmLapseRenewal = Nothing

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLapse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessDelete
    '
    ' Description: deletes renewal from insurance file table and renewal status table
    ' ***************************************************************** '
    Public Function ProcessDelete(ByVal v_lSelected As Integer) As Integer
        Dim result As Integer = 0
        Dim lStatusID, lPolicycnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lRenewalStatus As Integer
        Dim sLivePolicyRef As String = ""
        Dim lLivePolicyCnt As Integer

        Dim lPartyCnt, lInsuranceFolderCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get selected values
            lStatusID = CInt(m_vRenewalsData(ACIRenewalStatusId, v_lSelected))
            lPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, v_lSelected))
            lLivePolicyCnt = CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, v_lSelected))
            lRenewalStatus = CInt(m_vRenewalsData(ACIRenewalStatusTypeId, v_lSelected))
            sLivePolicyRef = CStr(m_vRenewalsData(ACIRenewalLivePolicy, v_lSelected))

            lPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, v_lSelected))
            lInsuranceFolderCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceFolder, v_lSelected))

            m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, v_lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, v_lSelected)).Trim())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = g_oRenewal.DeleteRenewal(v_lRenewalCnt:=lPolicycnt, v_lLivePolicyCnt:=lLivePolicyCnt, v_lStatusId:=lStatusID)
            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, v_lSelected), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, v_lSelected)).Trim())

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'create event

                m_lReturn = g_oRenewal.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolderCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Delete Renewal - " & CStr(m_vRenewalsData(ACIRenewalPolicy, v_lSelected)))

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus
    '
    ' Description: Sets the renewal status
    ' ***************************************************************** '
    Public Function SetStatus(ByRef lRenewalCnt As Integer, ByRef iRenewalStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oRenewal.SetRenewalStatus(v_lRenewalCnt:=lRenewalCnt, v_iRenewalStatus:=iRenewalStatus)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set status", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAmmendment", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the renewal status", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Rerate
    '
    ' Description: Rerates the renewal
    '
    ' Changes: RWH(16/11/2000) Changed to pass array to ReRate
    ' ***************************************************************** '
    Public Function Rerate(ByRef v_vlInsuranceFileCnt As Integer, ByRef v_vRiskIDArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oRenewal.ReRatePolicy(v_vlInsuranceFileCnt, v_vRiskIDArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    r_sRerateFailReason = r_sRerateFailReason

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to rerate the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="Rerate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:GetBusiness
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetBusiness(ByRef r_vResultArray(,) As Object, Optional ByVal v_lRenewalInsuranceFileCnt As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    ' Check for errors
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get details.
            '        GetBusiness = PMFalse
            '        Exit Function
            '    End If

            ' Set component properties and start interface

            'Thinh Nguyen 20/03/2002 - add branchID/SourceID

            m_lReturn = g_oRenewal.GetRenewals(r_vResultArray:=r_vResultArray, v_lRunMode:=m_lRunMode, v_lRenewalInsFileCnt:=IIf(v_lRenewalInsuranceFileCnt = 0, g_lInsuranceFileCnt, v_lRenewalInsuranceFileCnt), v_sRenewalDate:=IIf(v_lRenewalInsuranceFileCnt = 0, m_sRenewalDate, ""), v_lProductId:=IIf(v_lRenewalInsuranceFileCnt = 0, m_lProductId, 0), v_lSourceID:=IIf(v_lRenewalInsuranceFileCnt = 0, m_lSourceID, 0), v_lLeadAgentCnt:=IIf(v_lRenewalInsuranceFileCnt = 0, m_lLeadAgentCnt, 0), v_lPartyCnt:=IIf(v_lRenewalInsuranceFileCnt = 0, m_lAgentId, 0))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lButtonGap As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lButtonGap = 105
            'changes to correct the display of button
            'cmdTransfer.Top = VB6.TwipsToPixelsY(6360)
            cmdTransfer.Top = VB6.TwipsToPixelsY(6720)
            cmdTransfer.Left = VB6.TwipsToPixelsX(90)

            If GetBrokerTransferAuthority() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case m_lRunMode
                Case gPMConstants.ACRenewalModeAccept
                    cmdTransfer.Visible = False
                    cmdAmmend.Visible = False
                    cmdLapse.Visible = False
                    cmdDelete.Visible = False

                    cmdAccept.Top = cmdTransfer.Top
                    cmdAccept.Left = cmdTransfer.Left
                    cmdAccept.Visible = True

                    cmdSelectAll.Top = cmdTransfer.Top
                    cmdSelectAll.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdAccept.Left) + VB6.PixelsToTwipsX(cmdAccept.Width) + lButtonGap)
                    cmdSelectAll.Visible = True

                    cmdChangeStatus.Top = cmdTransfer.Top
                    cmdChangeStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdSelectAll.Left) + VB6.PixelsToTwipsX(cmdSelectAll.Width) + lButtonGap)
                    cmdChangeStatus.Visible = True

                    cmdFilter.Top = cmdTransfer.Top
                    cmdFilter.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdChangeStatus.Left) + VB6.PixelsToTwipsX(cmdChangeStatus.Width) + lButtonGap)
                    cmdFilter.Visible = True

                Case gPMConstants.ACRenewalModeAmend
                    cmdTransfer.Visible = m_bCanTransferBroker

                    cmdAccept.Visible = False

                    cmdAmmend.Top = cmdTransfer.Top
                    If m_bCanTransferBroker Then
                        cmdAmmend.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdTransfer.Left) + VB6.PixelsToTwipsX(cmdTransfer.Width) + lButtonGap)
                    Else
                        cmdAmmend.Left = cmdTransfer.Left
                    End If
                    cmdAmmend.Visible = True

                    cmdLapse.Top = cmdTransfer.Top
                    cmdLapse.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdAmmend.Left) + VB6.PixelsToTwipsX(cmdAmmend.Width) + lButtonGap)
                    cmdLapse.Visible = True

                    cmdDelete.Top = cmdTransfer.Top
                    cmdDelete.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdLapse.Left) + VB6.PixelsToTwipsX(cmdLapse.Width) + lButtonGap)
                    cmdDelete.Visible = True

                    cmdChangeStatus.Top = cmdTransfer.Top
                    cmdChangeStatus.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdDelete.Left) + VB6.PixelsToTwipsX(cmdDelete.Width) + lButtonGap)
                    cmdChangeStatus.Visible = True

                    cmdSelectAll.Top = cmdTransfer.Top
                    cmdSelectAll.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdChangeStatus.Left) + VB6.PixelsToTwipsX(cmdChangeStatus.Width) + lButtonGap)
                    cmdSelectAll.Visible = True

                    cmdFilter.Top = cmdTransfer.Top
                    cmdFilter.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdSelectAll.Left) + VB6.PixelsToTwipsX(cmdSelectAll.Width) + lButtonGap)
                    cmdFilter.Visible = True

                Case gPMConstants.ACRenewalModeTransfer
                    cmdAccept.Visible = False
                    cmdFilter.Visible = False
                    cmdChangeStatus.Visible = False

                    cmdTransfer.Visible = True

                    cmdAmmend.Top = cmdTransfer.Top
                    cmdAmmend.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdTransfer.Left) + VB6.PixelsToTwipsX(cmdTransfer.Width) + lButtonGap)
                    cmdAmmend.Visible = True

                    cmdLapse.Top = cmdTransfer.Top
                    cmdLapse.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdAmmend.Left) + VB6.PixelsToTwipsX(cmdAmmend.Width) + lButtonGap)
                    cmdLapse.Visible = True

                    cmdDelete.Top = cmdTransfer.Top
                    cmdDelete.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdLapse.Left) + VB6.PixelsToTwipsX(cmdLapse.Width) + lButtonGap)
                    cmdDelete.Visible = True

                    cmdSelectAll.Top = cmdTransfer.Top
                    cmdSelectAll.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdDelete.Left) + VB6.PixelsToTwipsX(cmdDelete.Width) + lButtonGap)
                    cmdSelectAll.Visible = True
            End Select

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'Made full row select on list views
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwRenewals.Handle.ToInt32(), v_vShowRowSelect:=True)

            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwRenewals.FullRowSelect = True

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            If m_lAgentId > 0 Then
                stbStatus.Text = ""

                stbStatus.Items.Item(0).Text = " " & sMessage
                lblStatus.Text = " " & sMessage
                lblStatus.Refresh()
                'stbStatus.Refresh()
            Else
                stbStatus.Text = " " & sMessage
                lblStatus.Text = " " & sMessage
                lblStatus.Refresh()
                'stbStatus.Refresh()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            If m_lAgentId > 0 Then

                stbStatus.Items.Item(0).Text = " " & m_lItemsFound & " " & sMessage
                lblStatus.Text = " " & m_lItemsFound & " " & sMessage
                lblStatus.Refresh()
            Else
                stbStatus.Text = " " & m_lItemsFound & " " & sMessage
                lblStatus.Text = " " & m_lItemsFound & " " & sMessage
                lblStatus.Refresh()

            End If

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub frmRenewal_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        ' Terminate the business object

        g_oRenewal.Dispose()

        ' Destroy the instance of the renewals business object
        ' from memory.
        g_oRenewal = Nothing

        If Not (m_oPremiumFinance Is Nothing) Then

            m_oPremiumFinance.Dispose()
            m_oPremiumFinance = Nothing
        End If

    End Sub

    '' ***************************************************************** '
    '' Name: ShowPolicyDetail
    ''
    '' Description: Displays policy information.
    ''
    '' ***************************************************************** '
    'Public Function ShowPolicyDetail(ByVal v_lPartyCnt As Long, _
    ''                                 ByVal v_sPartyType As String, _
    ''                                 ByVal v_lInsuranceFolderCnt As Long, _
    ''                                 ByVal v_lInsFileCnt As Long, _
    ''                                 ByVal v_lInsuranceFileStructureId As Long, _
    ''                                 ByVal v_sShortName As String, _
    ''                                 ByVal v_sInsReference As String, _
    ''                                 ByVal v_bFromEvent As Boolean, _
    ''                                 ByVal v_lPolicyTypeId As Long, _
    ''                                 ByVal v_vGeminiPolicyStatus As Variant)
    '
    'Dim fIndex As Integer
    'Dim iLoop1 As Integer
    'Dim sFormName As String
    '
    ''eck180500
    'Dim iSourceID As Integer
    '
    '    On Error GoTo Err_ShowPolicyDetail
    '
    '    ShowPolicyDetail = PMTrue
    '
    '
    '        sFormName = "frmPolicyRenewal"
    '
    '
    '    frmPolicyRenewal.PartyCnt = v_lPartyCnt&
    '    frmPolicyRenewal.PartyType = v_sPartyType
    '    frmPolicyRenewal.InsuranceFolderCnt = v_lInsuranceFolderCnt&
    '    frmPolicyRenewal.InsFileCnt = v_lInsFileCnt&
    '    frmPolicyRenewal.ShortName = v_sShortName$
    '    frmPolicyRenewal.InsReference = v_sInsReference
    '
    '    frmPolicyRenewal.SourceID = iSourceID
    '    frmPolicyRenewal.FromEvent = False
    '    'frmPolicyRenewal.Tag = fIndex%
    '
    '    frmPolicyRenewal.LoadInterface
    '    frmPolicyRenewal.Show (1)
    '
    '
    '    Exit Function
    '
    'Err_ShowPolicyDetail:
    '
    '    ShowPolicyDetail = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="ShowPolicyDetail Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="ShowPolicy", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    Private Sub lvwRenewals_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRenewals.Click

        'Reset data changed flag.
        m_bPolicyDetailsChanged = False

        'Only enable Change button if a single item is selected.
        Dim bOneAlreadySelected As Boolean = False
        m_bMultiSelect = False
        For iItemCount As Integer = 1 To lvwRenewals.Items.Count
            If lvwRenewals.Items.Item(iItemCount - 1).Selected Then
                If bOneAlreadySelected Then
                    m_bMultiSelect = True
                    Exit For
                End If
                bOneAlreadySelected = True
                lvwRenewals.FocusedItem = lvwRenewals.Items.Item(iItemCount - 1)
            End If
        Next iItemCount

        'sj 07/11/2002 - start
        If Not (lvwRenewals.FocusedItem Is Nothing) Then
            EnableDisableButtons()
        Else
            EnableDisableButtons(v_bDisableAll:=True)
        End If

        'sj 07/11/2002 - end

        '    cmdChange.Enabled = (m_bMultiSelect = False)

    End Sub

    Private Sub lvwRenewals_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRenewals.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRenewals.Columns(eventArgs.Column)
        'RWH(06/02/2001) Store previous SortKey in static variable as when sorting date
        'an extra column is created as SortKey and then removed. SortKey reverts to 0. If then
        'set SortKey to correct column, list resorts incorrectly.
        Static iPreviousSortKey As Integer

        ' Column click event for the search details

        Try

            With lvwRenewals
                ' VB 21/02/2005 PN-18895 Replaced 'IF' statment With 'Select Case' statment and
                '                        Added ACRenewalDate for sorting 'RenewalDate Column'
                Select Case ColumnHeader.Index + 1 - 1
                    Case ACDateColumn

                        'iDirection = (ListViewHelper.GetSortOrderProperty(lvwRenewals) + 1) Mod 2
                        '            End If
                        If ListViewHelper.GetSortOrderProperty(lvwRenewals) = SortOrder.Ascending Then
                            m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwRenewals, v_iSourceColumn:=ACDateColumn, v_iDirection:=SortOrder.Descending)
                        Else
                            m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwRenewals, v_iSourceColumn:=ACDateColumn, v_iDirection:=SortOrder.Ascending)
                        End If
                        'm_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwRenewals, v_iSourceColumn:=ACDateColumn, v_iDirection:=iDirection)

                        iPreviousSortKey = ACDateColumn

                        ' If current sort column header is
                        ' pressed.
                    Case ACRenewalDate

                        'iDirection = (ListViewHelper.GetSortOrderProperty(lvwRenewals) + 1) Mod 2
                        If ListViewHelper.GetSortOrderProperty(lvwRenewals) = SortOrder.Ascending Then
                            m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwRenewals, v_iSourceColumn:=ACRenewalDate, v_iDirection:=SortOrder.Descending)
                        Else
                            m_lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=lvwRenewals, v_iSourceColumn:=ACRenewalDate, v_iDirection:=SortOrder.Ascending)
                        End If

                        iPreviousSortKey = ACRenewalDate
                    Case ListViewHelper.GetSortKeyProperty(lvwRenewals)
                        'If ListViewHelper.GetSortedProperty(lvwRenewals) Then
                        '        ElseIf (ColumnHeader.Index - 1 = iPreviousSortKey) Then
                        ' Set sort order opposite of
                        ' current direction.
                        If ListViewHelper.GetSortOrderProperty(lvwRenewals) = SortOrder.Ascending Then
                            'ListViewHelper.SetSortOrderProperty(lvwRenewals, (ListViewHelper.GetSortOrderProperty(lvwRenewals) + 1) Mod 2)
                            ListViewHelper.SetSortOrderProperty(lvwRenewals, SortOrder.Descending)
                        Else
                            ListViewHelper.SetSortOrderProperty(lvwRenewals, SortOrder.Ascending)
                        End If
                    Case Else
                        ' Sort by this column (ascending).
                        ListViewHelper.SetSortedProperty(lvwRenewals, False)

                        ' Turn off sorting so that the list
                        ' is not sorted twice
                        ListViewHelper.SetSortOrderProperty(lvwRenewals, SortOrder.Ascending)
                        ListViewHelper.SetSortKeyProperty(lvwRenewals, ColumnHeader.Index + 1 - 1)
                        ListViewHelper.SetSortedProperty(lvwRenewals, True)
                        iPreviousSortKey = ListViewHelper.GetSortKeyProperty(lvwRenewals)

                End Select
            End With

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRenewals_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwRenewals_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRenewals.DoubleClick

        Try

            ' Check if there are any items available.
            If lvwRenewals.Items.Count = 0 Then
                Exit Sub
            End If

            If m_lRunMode = gPMConstants.ACRenewalModeAmend Then
                m_lReturn = ProcessAmendment()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ammend the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRenewals_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
            Else
                cmdAccept_Click(cmdAccept, New EventArgs())
            End If

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ammend the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAmmend", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ProcessAccept
    '
    ' Description: Oversees the process of Accepting a renewal.
    '
    ' History: 03/10/2000 RWH - Created.
    '          Thinh Nguyen 08/10/2002 - modify and restructure this function slightly
    '          to produce and spool a report on policies selected for acceptance
    '
    '          Thinh Nguyen 23/10/2002 - reset status of unquoted policy back to awaiting manual review
    ' ***************************************************************** '
    Public Function ProcessAccept() As Integer
        Dim result As Integer = 0
        Dim lSelectedCount As Integer

        Dim sReportTitle, sAcceptanceReportFileName, sRegPath As String
        Dim sReportText As New StringBuilder
        Dim sFailedText As New StringBuilder
        Dim sFailedReason As String = ""
        Dim iAllPaynow, iAllInvoice As Integer
        Dim lExceptionCount As Integer
        Dim sSpoolRenewalReport As String = ""
        Dim bInvalidTMPFound As Boolean
        Dim lInvalidTMPCount As Integer
        Dim bValid As Boolean
        Dim iInsuranceFolder As Integer
        Dim iProductId As Integer
        Dim bPolicyNumberToChange, bNoRenewalInstalmentPlan, bPrepaymentRequired As Boolean
        Dim iSelectedIndex As Integer
        Dim bLocked As Boolean 

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lSelPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
            m_lSelNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
            iInsuranceFolder = ToSafeInteger(m_vRenewalsData(ACIRenewalInsuranceFolder, Convert.ToString(lvwRenewals.FocusedItem.Tag)))

            If sSpoolRenewalReport = "" Then
                If iPMFunc.GetSystemOption(v_iOptionNumber:=1012, r_sOptionValue:=sSpoolRenewalReport) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Sunil
            Dim bIsvalid As Boolean

            If ToSafeInteger(m_vRenewalsData(ACIRenewalAgentCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag))) <> 0 Then

                m_lReturn = ValidateCertificateYear(bIsvalid, m_lSelNewPolicyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to getCertificate Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                End If
                If bIsvalid = False Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If
            End If

            'Return result
            If m_sRenSchedulePrinting = "" Then
                If iPMFunc.GetSystemOption(v_iOptionNumber:=1036, r_sOptionValue:=m_sRenSchedulePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_sRenCertificatePrinting = "" Then
                If iPMFunc.GetSystemOption(v_iOptionNumber:=1037, r_sOptionValue:=m_sRenCertificatePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_sRenDebitNotePrinting = "" Then
                If iPMFunc.GetSystemOption(v_iOptionNumber:=1038, r_sOptionValue:=m_sRenDebitNotePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'get path from registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not sRegPath.EndsWith("\") Then
                sRegPath = sRegPath & "\"
            End If

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, g_iSourceID, m_vAllowPayNowOption)

            'Ashwani - (RFC_Enable_PrePayment_functionality)
            iProductId = CStr(m_vRenewalsData(ACIRenewalProductId, Convert.ToString(lvwRenewals.FocusedItem.Tag))).Trim()
            m_lReturn = m_oBusiness.GetPrePaymentOptionValue(iProductId, m_oPrepayment)



            'get report file name including path
            sAcceptanceReportFileName = sRegPath & DateTime.Now.ToString("yyyyMMddHHMMss") &
                                        "Renewal_" & g_oObjectManager.UserName & ".log"

            'report title
            sReportTitle = "Renewal Acceptance Status Report - " & StringsHelper.Format(DateTime.Now, "dd mmmm " &
                           "yyyy hh:mm:ss AMPM") & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            'accept just one policy
            Dim bAllowPayNow As Boolean
            Dim iReturnedValue, iReturnedValue_Inv As Integer
            If Not m_bMultiSelect Then

                ' only a single item has been selected
                lSelectedCount = 1

                ' determine if this is a true monthly policy anniversary copy
                ' if it is only allow acceptance if the previous cycle has been completed
                m_lReturn = ValidateTMPPolicy(v_lSelectedIndex:=Convert.ToString(lvwRenewals.FocusedItem.Tag), r_bIsValid:=bValid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not bValid Then
                    bInvalidTMPFound = True
                    lInvalidTMPCount += 1
                    bValid = False
                End If

                m_lReturn = AllPolicyPayNow(iAllPaynow)

                'Start - Prakash - WPR85_Paralleling
                If Not m_bMultiSelect Then
                    If iAllPaynow = 1 Then
                        m_lReturn = ShowPayNow(sPaymentMethod:="PayNow")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If
                    ElseIf iAllPaynow = 3 Then
                        m_lReturn = ProcessCashDeposit()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If
                    End If
                End If
                'End - Prakash - WPR85_Paralleling

                If m_vAllowPayNowOption = "1" Or m_oPrepayment(0, 0) = "1" Then 'Ashwani - (RFC_Enable_PrePayment_functionality)Then
                    m_lReturn = AllPolicyInvoice(iAllInvoice)

                    If Not m_bMultiSelect And iAllInvoice = 1 Then
                        m_lReturn = ShowPayNow(sPaymentMethod:="Invoice")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_lReturn
                        End If
                    End If
                End If

                If bValid Then

                    If lvwRenewals.FocusedItem.SubItems(9).Text = "1" Then
                        sFailedText.Append(
                                           ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(lvwRenewals.FocusedItem.Index), 2).Text & " - " &
                                           "Branch Closed" & Strings.Chr(13) & Strings.Chr(10))
                    Else
                        m_lReturn = IsQuoted(v_lSelectedIndex:=Convert.ToString(lvwRenewals.FocusedItem.Tag))

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = AcceptRenewal(iSelectedIndex:=Convert.ToString(lvwRenewals.FocusedItem.Tag), r_sFailedText:=sFailedReason)

                            If sFailedReason.ToString() <> "" Then
                                sFailedText = New StringBuilder(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(lvwRenewals.FocusedItem.Index), 2).Text & " - " & sFailedReason.ToString() & Strings.Chr(13) & Strings.Chr(10))

                            End If
                        Else
                            sFailedText.Append(
                                               ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(lvwRenewals.FocusedItem.Index), 2).Text & " - " &
                                               "Unquoted" & Strings.Chr(13) & Strings.Chr(10))

                            m_lReturn = SetStatus(lRenewalCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag))), iRenewalStatus:=1)
                        End If

                        If sFailedText.ToString() = "" Then
                            sReportText = New StringBuilder(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(lvwRenewals.FocusedItem.Index), 2).Text & " - Successful" & Strings.Chr(13) & Strings.Chr(10))
                        Else
                            sReportText = New StringBuilder(sFailedText.ToString())
                        End If

                        'remove this policy from listview
                        If Not sFailedText.ToString().Contains("Policy Already Locked !!") Then
                            Me.lvwRenewals.Items.RemoveAt(Me.lvwRenewals.FocusedItem.Index)
                        End If

                        m_lReturn = CancelMTAQuotes(m_lSelNewPolicyCnt, iInsuranceFolder, m_lSelPartyCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Business must have logged it.
                        End If
                    End If

                End If

            Else
                'accept multiple policies

                'Start Renuka PN 61755
                'End Renuka PN 61755

                'Step backwards thru' list so we can remove items as we go.
                For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                    If lvwRenewals.Items.Item(iListCount - 1).Selected Then

                        lSelectedCount += 1
                        m_lSelPartyCnt = ToSafeInteger(m_vRenewalsData(ACIRenewalInsuranceHolder, Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag)))
                        m_lSelNewPolicyCnt = ToSafeInteger(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag)))
                        iInsuranceFolder = ToSafeInteger(m_vRenewalsData(ACIRenewalInsuranceFolder, Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag)))

                        ' determine if this is a true monthly policy anniversary copy
                        ' if it is only allow acceptance if the previous cycle has been completed
                        m_lReturn = ValidateTMPPolicy(v_lSelectedIndex:=Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag), r_bIsValid:=bValid)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bInvalidTMPFound = True
                            lInvalidTMPCount += 1
                            bValid = False
                            'MsgBox "The Anniversary Renewal cannot be accepted until " & _
                            '" the last monthly cycle has been accepted", _
                            'vbInformation, "True Monthly Policy Validation"
                        End If

                        If bValid Then
                            m_lReturn = ValidateRenewalAcceptance(v_lInsuranceFileCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag))), r_bPolicyNumberToChange:=bPolicyNumberToChange, r_bNoRenewalInstalmentPlan:=bNoRenewalInstalmentPlan, r_bPrepaymentRequired:=bPrepaymentRequired)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate renewal acceptance", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Return result
                            End If

                            If Not bPolicyNumberToChange And Not bNoRenewalInstalmentPlan And Not bPrepaymentRequired Then
                                'Start Renuka PN 61755
                                If Not bAllowPayNow Then

                                    m_lReturn = AllPolicyPayNow(iReturnedValue)
                                    m_lReturn = AllPolicyInvoice(iReturnedValue_Inv)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        sFailedText = New StringBuilder("AllPolicyPayNow Failed")
                                        Return result
                                    End If

                                    If iReturnedValue = 1 Or iReturnedValue_Inv = 1 Then
                                        If m_lAgentId <> 0 Then 'Filtered by Agent
                                            'Show the PayNow if All the Selected policies have used Paynow as the Payment Option
                                            ' and Filtering Criteria By Agent is Also Applied.
                                            If iReturnedValue = 1 Then
                                                m_lReturn = ShowPayNow(sPaymentMethod:="PayNow")
                                            Else
                                                m_lReturn = ShowPayNow(sPaymentMethod:="Invoice")
                                            End If

                                            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                                Return m_lReturn
                                            End If
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                result = m_lReturn
                                                sFailedText = New StringBuilder("PayNow Failed")
                                                Return result
                                            End If
                                        End If
                                    End If
                                    bAllowPayNow = True
                                End If
                                'End Renuka PN 61755

                                If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 9).Text = "1" Then
                                    sFailedText.Append(
                                                       ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(lvwRenewals.FocusedItem.Index), 2).Text & " - " &
                                                       "Branch Closed" & Strings.Chr(13) & Strings.Chr(10))
                                    'Policy is still attached to a closed branch and cannot be accepted
                                    MessageBox.Show("Unable to proceed - this Policy is attached to a branch that" & " is closed. Please Amend " & ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                                ElseIf gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, Convert.ToString(lvwRenewals.FocusedItem.Tag))), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                                    sReportText.Append(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text & " - " &
                                                       "Agent/Broker for this policy is in transfer mode. Please contact the System Administrator")
                                Else
                                    If IsQuoted(v_lSelectedIndex:=Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag), v_lDisplay:=0) = gPMConstants.PMEReturnCode.PMTrue Then

                                        sFailedText = New StringBuilder("")
                                        m_lReturn = AcceptRenewal(iSelectedIndex:=Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag), r_sFailedText:=sFailedText.ToString())

                                        If sFailedText.ToString() <> "" Then
                                            sReportText.Append(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text & " - " &
                                                               "" & sFailedText.ToString() & Strings.Chr(13) & Strings.Chr(10))
                                        Else
                                            sReportText.Append(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text & " - " &
                                                               "Successful" & Strings.Chr(13) & Strings.Chr(10))
                                        End If

                                    Else

                                        sReportText.Append(ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text & " - " &
                                                           "Unquoted" & Strings.Chr(13) & Strings.Chr(10))

                                        m_lReturn = SetStatus(lRenewalCnt:=CInt(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.Items.Item(iListCount - 1).Tag))), iRenewalStatus:=1)

                                    End If

                                    'remove this policy from listview
                                    lvwRenewals.Items.RemoveAt(iListCount - 1)

                                    m_lReturn = CancelMTAQuotes(m_lSelNewPolicyCnt, iInsuranceFolder, m_lSelPartyCnt)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        'Business must have logged it.
                                    End If
                                End If
                            Else
                                'remove this policy from listview
                                lvwRenewals.Items.RemoveAt(iListCount - 1)
                                'Start Renuka PN 61755
                                lExceptionCount += 1
                                'End Renuka PN 61755
                            End If
                        Else
                            bInvalidTMPFound = True
                            lInvalidTMPCount += 1
                        End If
                    End If

                Next iListCount
            End If

            If bInvalidTMPFound Then
                MessageBox.Show(CStr(lInvalidTMPCount) & " anniversary renewal/s could not be processed." &
                                " Anniversary Renewals cannot be accepted until " &
                                " the last monthly cycle has been accepted", "True Monthly Policy Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Me.lvwRenewals.Refresh()
            lvwRenewals.FullRowSelect = True
            m_lItemsFound = lvwRenewals.Items.Count
            DisplayStatusFound()

            ' dont bother to spool the report if all the selected items are invalid
            'Start Renuka PN 61755
            If lSelectedCount <> lInvalidTMPCount And sSpoolRenewalReport = "1" And lSelectedCount <> lExceptionCount Then
                'End Renuka PN 61755

                'add in report title
                sReportText = New StringBuilder(sReportTitle & sReportText.ToString())

                'create text file of policies in this batch
                m_lReturn = AppendText(v_sFile:=sAcceptanceReportFileName, v_sTextLine:=sReportText.ToString())

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create renewal acceptance report file", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                m_lReturn = SpoolDoc(v_sFileName:=sAcceptanceReportFileName, v_sSpoolDesc:="Renewal Acceptance Status Report")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool renewal acceptance report file", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                MessageBox.Show("Acceptance report has been spooled", "Renewal Acceptance", MessageBoxButtons.OK)

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccept Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ChangePolicyDetails
    '
    ' Description:
    '
    ' History: 05/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ChangePolicyDetails() As Integer
        Dim result As Integer = 0

        Dim oList As ListViewItem
        Dim sNewPolicyNumber As String = ""
        Dim vValue As String = ""
        Dim oBusiness As Object
        Dim oPolicyNumberGeneration As Object
        Dim iPartyCnt As Integer
        Const OPTChangePolicyNumberAtRenewalAutomatically As Integer = 161
        'End - Renuka - (WPR87 Paralleling)
        Dim iPolicySourceId As Integer
        frmChangePolicyDetails = New frmChangePolicyDetails

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oList = lvwRenewals.FocusedItem
            If oList Is Nothing Then
                Return result
            End If

            m_sRenewalPolicyNumber = CStr(m_vRenewalsData(ACIRenewalPolicy, Convert.ToString(lvwRenewals.FocusedItem.Tag)))

            'convert to date using regional format rather than hardcoded
            'm_sRenewalStartDate = CDate(m_vRenewalsData(ACIRenewalCoverStartDate))
            'm_sRenewalExpiryDate = CDate(m_vRenewalsData(ACIRenewalExpiryDate))
            'PN 25582 V.G.
            m_sRenewalStartDate = CDate(m_vRenewalsData(ACIRenewalCoverStartDate, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
            m_sRenewalExpiryDate = CDate(m_vRenewalsData(ACIRenewalExpiryDate, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
            iPartyCnt = ToSafeInteger(m_vRenewalsData(ACIRenewalInsuranceHolder, Convert.ToString(lvwRenewals.FocusedItem.Tag)))

            'Start - Renuka - (WPR87 Paralleling)
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ChangePolicyDetails", "Failed to initilize the Component bSIRInsuranceFile.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oBusiness.GetProductDetails(v_lProductId:=m_vRenewalsData(ACIRenewalProductId, Convert.ToString(lvwRenewals.FocusedItem.Tag)).Trim(), v_lOption:=OPTChangePolicyNumberAtRenewalAutomatically, r_vValue:=vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ChangePolicyDetails", "Method GetProductdetails failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oBusiness.GetFromTable(v_vTableName:="Insurance_File", v_vFieldName:="Source_Id", v_vKeyField:="Insurance_File_Cnt", v_vKeyID:=CStr(m_vRenewalsData(ACIRenewalPolicyCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag))).Trim(), r_vResult:=iPolicySourceId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ChangePolicyDetails", "Method GetProductdetails failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If vValue = "1" Then
                If oPolicyNumberGeneration Is Nothing Then
                    Dim temp_oPolicyNumberGeneration As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oPolicyNumberGeneration, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oPolicyNumberGeneration = temp_oPolicyNumberGeneration

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ChangePolicyDetails", "Failed to initilize the Component bSIRPolicyNumMaint.Business", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                m_lReturn = oPolicyNumberGeneration.GeneratePolicyNumber(v_lBusinessType:=ACBusinessTypePolicy, v_iBranch:=iPolicySourceId, v_lProductId:=CStr(m_vRenewalsData(ACIRenewalProductId, Convert.ToString(lvwRenewals.FocusedItem.Tag))).Trim(), v_lAgent:=m_lAgentId, r_sGeneratedPolicyNumber:=sNewPolicyNumber, v_dtTransactionDate:=m_sRenewalStartDate, v_lPartyCnt:=iPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ChangePolicyDetails", "Methoed GeneratePolicyNumber Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_sRenewalPolicyNumber.Trim() <> sNewPolicyNumber.Trim() And sNewPolicyNumber.Trim() <> "" Then
                    m_bPolicyDetailsChanged = True
                    m_sRenewalPolicyNumber = sNewPolicyNumber
                Else
                    m_bPolicyDetailsChanged = False
                End If
            Else
                With frmChangePolicyDetails
                    .PolicyNumber = m_sRenewalPolicyNumber
                    .CoverStartDate = m_sRenewalStartDate
                    .CoverExpiryDate = m_sRenewalExpiryDate
                    .ProductId = CInt(m_vRenewalsData(ACIRenewalProductId, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
                    'TN20001218 - Start
                    ' The BusinessType specified here is a positional index for the return
                    'array from spu_get_prod_auto_num_ids.
                    'RWH(18/11/2000) Set BusinessType to Quote as this is what is used when we create a
                    'new policy from New Business in Underwriting.
                    '.BusinessType = ACBusinessTypeQuote
                    .BusinessType = ACBusinessTypePolicy
                    'TN20001218 - End9
                    .PartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
                    'RWH(24/05/01) Protect AgentId against Nulls and Blanks.

                    If (CStr(m_vRenewalsData(ACIRenewalAgentCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag))) = "") Or (Convert.IsDBNull(m_vRenewalsData(ACIRenewalAgentCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag))) Or IsNothing(m_vRenewalsData(ACIRenewalAgentCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag)))) Then
                        .AgentId = 0
                    Else
                        .AgentId = CInt(m_vRenewalsData(ACIRenewalAgentCnt, Convert.ToString(lvwRenewals.FocusedItem.Tag)))
                    End If
                    .BranchId = g_iSourceID

                    frmChangePolicyDetails.ShowDialog()
                    If frmChangePolicyDetails.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                        'If details have changed then update member variables.
                        If m_sRenewalPolicyNumber <> .PolicyNumber Then
                            m_bPolicyDetailsChanged = True
                            m_sRenewalPolicyNumber = .PolicyNumber
                        End If
                        If m_sRenewalStartDate <> .CoverStartDate Then
                            m_bPolicyDetailsChanged = True
                            m_sRenewalStartDate = .CoverStartDate
                        End If
                        If m_sRenewalExpiryDate <> .CoverExpiryDate Then
                            m_bPolicyDetailsChanged = True
                            m_sRenewalExpiryDate = .CoverExpiryDate
                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                frmChangePolicyDetails.Close()
            End If
            'End - Renuka - (WPR87 Paralleling)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangePolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangePolicyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateDocument
    '
    ' Description: Retrieves document id from FindDocTemplate component for
    '               given type and Policy Id and generates document via
    '               Document Template component.
    '
    ' History: 12/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateDocument(ByRef v_iDocType As Integer, ByRef v_iMode As Integer, ByRef v_lInsuranceFileCnt As Integer, ByRef v_lInsuranceFolderCnt As Integer, ByRef v_lPartyCnt As Integer, ByRef v_sSpoolDesc As String) As Integer
        Dim result As Integer = 0


        Dim oGetDocument As Object
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 4)

            'Generate document.
            Dim obPMBDocLink As bPMBDocLink.Business
            Dim oResultArray As Object
            Dim temp_obPMBDocLink As Object
            Dim m_iFuntionalArea As Integer


            Dim temp_oGetDocument As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGetDocument, sClassName:="iPMUGetDocument.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oGetDocument = temp_oGetDocument

            If oGetDocument Is Nothing Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = g_oObjectManager.GetInstance(temp_obPMBDocLink, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obPMBDocLink = temp_obPMBDocLink


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Doc Link object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            'CType(oGetDocument, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            oGetDocument.Initialise()


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_iDocType


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lInsuranceFolderCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductId

            m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

            oGetDocument.FunctionalArea = 1

            oGetDocument.TransactionType = "RN"


            m_lReturn = oGetDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' don't exit; try to release COM refs
            End If

            'Get Values from
            'For time being funtional area is set to 1 i.e. document linking for policy
            m_iFuntionalArea = 1

            m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=m_iFuntionalArea, v_lInsurance_File_Cnt:=v_lInsuranceFileCnt, v_lProcessType_Docs_ID:=v_iDocType, v_lProcess_Type_Code:="RN", v_dtEffectiveDate:=DateTime.Now, r_vResultarray:=oResultArray, v_bCalledFromSAM:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(oResultArray) Then
                If (oResultArray(10, 0) = "Lapse") Then
                    MessageBox.Show("Lapse Document(s) spooled." & Strings.Chr(13) & Strings.Chr(10) & "Process complete", "Lapse Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            oGetDocument.Dispose()
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocID (Private)
    '
    ' Description: Get document template id and document type id
    '
    ' ***************************************************************** '

    'Private Function GetDocID(ByVal v_lInsuranceFileCnt As Integer, ByVal v_iDocType As Integer, ByRef r_lDocTemplateID As Integer, ByRef r_lDocTypeID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oFindDocTemplate = New iPMBFindDocTemplate.Interface_Renamed()
    '
    'If oFindDocTemplate Is Nothing Then
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBFindDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'initialise the object
    'If CType(oFindDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindDocTemplate = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'oFindDocTemplate.InsuranceFileCnt = v_lInsuranceFileCnt
    'oFindDocTemplate.ProcessType = v_iDocType
    'oFindDocTemplate.Mode = 2 'invisible merge
    '
    'oFindDocTemplate.SetProcessModes(vTransactionType:="RN")
    'If oFindDocTemplate.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindDocTemplate = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'r_lDocTemplateID = oFindDocTemplate.DocumentTemplateId
    'r_lDocTypeID = oFindDocTemplate.DocumentTypeId
    '
    'm_lReturn = oFindDocTemplate.Terminate()
    '
    'oFindDocTemplate = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document template id and document type id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: RunPolicy
    '
    ' Description:
    '
    ' History: 13/02/2001 RWH - Created.
    '
    ' Kevin Renshaw (CMG) 26/2/2003. New renewals key
    ' ***************************************************************** '
    Private Function RunPolicy(ByRef r_iStatus As Integer, ByVal v_lSelectedIndex As Integer) As Integer
        Dim result As Integer = 0

        Dim oPolicy As Object
        Dim vKeys(,) As Object
        Dim sTitle, sMessage As String
        Const sPROCESS As String = "iPMUPolicy"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Policy interface object
            'RWH(16/05/2001) It appears we were launching the wrong object here.
            'So how did it work before ?!?!?!
            '        sClassName:="iPMBFindInsurance.NavigatorV3",
            Dim temp_oPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPolicy, "iPMUPolicy.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPolicy = temp_oPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessLaunchFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & sPROCESS, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim vKeys(1, 5)

            vKeys(0, 0) = "party_cnt"

            vKeys(1, 0) = m_vRenewalsData(ACIRenewalInsuranceHolder, v_lSelectedIndex)

            vKeys(0, 1) = "insurance_file_cnt"

            vKeys(1, 1) = m_vRenewalsData(ACIRenewalPolicyCnt, v_lSelectedIndex)

            vKeys(0, 2) = "insurance_folder_cnt"

            vKeys(1, 2) = m_vRenewalsData(ACIRenewalInsuranceFolder, v_lSelectedIndex)

            vKeys(0, 3) = "shortname"

            vKeys(1, 3) = m_vRenewalsData(ACIRenewalShortname, v_lSelectedIndex)

            vKeys(0, 4) = "Product_id"

            vKeys(1, 4) = m_vRenewalsData(ACIRenewalProductId, v_lSelectedIndex)

            vKeys(0, 5) = "renewals"

            vKeys(1, 5) = True

            m_lReturn = oPolicy.NavigatorV3_SetKeys(vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oPolicy.Dispose()
                oPolicy = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendSetKeysFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & sPROCESS, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPolicy.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="REN")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oPolicy.Dispose()
                oPolicy = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendSetProcessModesFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & sPROCESS, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPolicy.NavigatorV3_Start
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                r_iStatus = oPolicy.NavigatorV3_Status

                'Retrieve BusinessTypeId to pass into coinsurance.
                If r_iStatus = gPMConstants.PMEReturnCode.PMOK Then
                    m_lReturn = oPolicy.NavigatorV3_GetKeys(vKeys)
                    If Information.IsArray(vKeys) Then

                        For iCount As Integer = 0 To vKeys.GetUpperBound(1)

                            If CStr(vKeys(0, iCount)) = PMNavKeyConst.PMKeyNameBusinessTypeId Then

                                m_lBusinessTypeId = CInt(vKeys(1, iCount))
                                Exit For
                            End If
                        Next iCount
                    End If

                End If
            Else

                oPolicy.Dispose()
                oPolicy = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & sPROCESS, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oPolicy.Dispose()

            oPolicy = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RunProcess
    '
    ' Description:
    '
    ' History: 13/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    'Added one Parameter
    Private Function RunProcess(ByRef v_sComponentName As String, ByRef v_vKeys(,) As Object, ByRef r_iStatus As Integer, Optional ByVal v_sTransactionType As String = "REN", Optional ByRef r_vGetKeyArray(,) As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oComponent As Object
        Dim sTitle, sMessage, sClass As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sClass = v_sComponentName

            ' Create Policy interface object

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oComponent, sClassName:=sClass, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessLaunchFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If v_sComponentName = "iPMBPartyFee.RMStepUInterface" Then

                m_lReturn = oComponent.SetKeys(v_vKeys)
            Else

                m_lReturn = oComponent.NavigatorV3_SetKeys(v_vKeys)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oComponent.Dispose()
                oComponent = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendSetKeysFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_sComponentName = "iPMBPartyFee.RMStepUInterface" Then

                m_lReturn = oComponent.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            Else

                m_lReturn = oComponent.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=v_sTransactionType)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oComponent.Dispose()
                oComponent = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendSetProcessModesFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_sComponentName = "iPMBPartyFee.RMStepUInterface" Then

                m_lReturn = oComponent.Start
            Else

                m_lReturn = oComponent.NavigatorV3_Start

            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If v_sComponentName = "iPMBPartyFee.RMStepUInterface" Then

                    r_iStatus = oComponent.Status
                Else

                    r_iStatus = oComponent.NavigatorV3_Status

                    '5-nov
                    If Not Information.IsArray(r_vGetKeyArray) Then

                        If oComponent.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            sMessage = "Failed to get keys from " & v_sComponentName
                            'If v_bDisplayMessage Then
                            MessageBox.Show(sMessage, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'End If
                            'GoTo Finally_Renamed
                        End If
                    End If
                    'end 5-nov
                End If
            Else

                oComponent.Dispose()
                oComponent = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oComponent.Dispose()

            oComponent = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunProcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DisplayMessage
    '
    ' Description: Displays message to report failure of child process.
    '
    ' History: 14/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function DisplayMessage(ByVal v_sComponentName As String) As Integer
        Dim result As Integer = 0
        Dim sTitle, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get description from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display message.
            MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AcceptRenewal
    '
    ' Description:
    '
    ' History: 26/02/2001 RWH - Created.
    '
    ' Thinh Nguyen 01/10/2002 - modify to report failed message
    ' ***************************************************************** '
    Private Function AcceptRenewal(ByRef iSelectedIndex As Integer, ByRef r_sFailedText As String) As Integer
        Dim result As Integer = 0
        Dim lOldPolicyCnt, lRenewalStatusCnt, lNewPolicyCnt, lInsuranceFolder, lPartyCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim bProduceSchedule, bProduceDebitNote, bProduceCertificate As Boolean
        Dim vPrintOptions As Object
        Dim vResultArray(,) As Object
        Dim lProductId As Integer
        Dim cTemp As Decimal
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim lAnniversaryCopy As Integer
        Dim bGenerateDocuments As Boolean
        Dim v_sPaymentMethod As String = ""
        Dim bLocked As Boolean

        Const ACProcessing As String = "Processing Acceptance ... "

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            lOldPolicyCnt = CInt(m_vRenewalsData(ACIRenewalLivePolicyCnt, iSelectedIndex))
            lRenewalStatusCnt = CInt(m_vRenewalsData(ACIRenewalStatusId, iSelectedIndex))
            lNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, iSelectedIndex))
            lInsuranceFolder = CInt(m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex))
            lPartyCnt = CInt(m_vRenewalsData(ACIRenewalInsuranceHolder, iSelectedIndex))
            sInsuranceRef = CStr(m_vRenewalsData(ACIRenewalPolicy, iSelectedIndex))
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductIsTrueMonthlyPolicy, iSelectedIndex)), 0) = 1
            lAnniversaryCopy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalAnniversaryCopy, iSelectedIndex)), 0)
            lProductId = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductId, iSelectedIndex)), 0)

            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            Dim temp_m_oProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oProduct = temp_m_oProduct

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ChangePolicyDetails", "Failed to get instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oProduct.GetProductValue(v_lProductId:=lProductId, v_sColumnName:="is_roundoff_to_zero", r_vProductArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ChangePolicyDetails", "Failed To retreive Product Risk Maintainence option for Roundoff", gPMConstants.PMELogLevel.PMLogError)

            Else
                If Information.IsArray(vResultArray) Then

                    m_bRoundOff = IIf(CDbl(vResultArray(0, 0)) = 1, 1, 0)
                End If

            End If
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            'lock this renewal status count to stop others from processing it
            m_lReturn = ProcessLock(v_bAcquireLock:=True, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, iSelectedIndex)).Trim())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailedText = "Policy Already Locked !!"
                Return result
            End If
            bLocked = True
            'Carry out updates for Accept.
            If m_bPolicyDetailsChanged Then
                If m_lAgentId > 0 Then

                    stbStatus.Items.Item(0).Text = ACProcessing & m_sRenewalPolicyNumber
                    lblStatus.Text = ACProcessing & m_sRenewalPolicyNumber
                    lblStatus.Refresh()
                    'stbStatus.Refresh()
                Else
                    stbStatus.Text = ACProcessing & m_sRenewalPolicyNumber
                    lblStatus.Text = ACProcessing & m_sRenewalPolicyNumber
                    lblStatus.Refresh()
                    'stbStatus.Refresh()
                End If

                m_lReturn = g_oRenewal.AcceptRenewal(lOldPolicyCnt, lNewPolicyCnt, lRenewalStatusCnt, v_sNewPolicyRef:=m_sRenewalPolicyNumber, v_sNewStartDate:=m_sRenewalStartDate, v_sNewExpiryDate:=m_sRenewalExpiryDate, r_sFailureMessage:=r_sFailedText) '(RC) QBENZ014

                '(RC) QBENZ014
                If gPMFunctions.ToSafeString(r_sFailedText) <> "" Then
                    MessageBox.Show(r_sFailedText, "Renewal Acceptance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'PSL 5842 17/9/2003
                    If m_lReturn = PM_FAILED_RENEWAL_STATUS Then
                        MessageBox.Show("Policy " & m_sRenewalPolicyNumber & " has Already Been Accepted", "Renewal Acceptance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    r_sFailedText = ACRenFailedMakePolicyLive
                    Return result
                End If

            Else
                If m_lAgentId > 0 Then

                    stbStatus.Items.Item(0).Text = ACProcessing & sInsuranceRef
                    lblStatus.Text = ACProcessing & sInsuranceRef
                    lblStatus.Refresh()
                    'stbStatus.Refresh()
                Else
                    stbStatus.Text = ACProcessing & sInsuranceRef
                    lblStatus.Text = ACProcessing & sInsuranceRef
                    lblStatus.Refresh()
                    'stbStatus.Refresh()
                End If

                m_lReturn = g_oRenewal.AcceptRenewal(lOldPolicyCnt, lNewPolicyCnt, lRenewalStatusCnt, r_sFailureMessage:=r_sFailedText) '(RC) QBENZ014
                '(RC) QBENZ014
                If gPMFunctions.ToSafeString(r_sFailedText) <> "" Then
                    MessageBox.Show(r_sFailedText, "Renewal Acceptance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailedText = ACRenFailedMakePolicyLive
                    Return result
                End If

            End If

            m_lReturn = g_oRenewal.GetPaymentMethod(v_lInsuranceFileCnt:=lNewPolicyCnt, r_sPaymentMethod:=v_sPaymentMethod)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            If v_sPaymentMethod = "Instalments" Then
                GetGrossTotal(cTemp)
            End If
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            'Create Stats (Needs insurance_file_cnt)
            m_lReturn = GetStats(lNewPolicyCnt, bIsTrueMonthlyPolicy)
            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailedText = ACRenFailedStatistics

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create Accumulations
            m_lReturn = GetAccumulations(lNewPolicyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailedText = ACRenFailedAccumulation
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = g_oRenewal.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolder, v_vInsuranceFileCnt:=lNewPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Accept Renewal - " & CStr(m_vRenewalsData(ACIRenewalPolicy, iSelectedIndex)))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailedText = ACRenFailedCreateEvent
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalIsInTransferMode, iSelectedIndex)), 0) <> 0 Then

                m_lReturn = g_oRenewal.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=lInsuranceFolder, v_vInsuranceFileCnt:=lNewPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Renewal Accepted - Broker Transfer From " & CStr(m_vRenewalsData(ACIRenewalLeadAgentCode, iSelectedIndex)) &
                            " to " & CStr(m_vRenewalsData(ACIRenewalTransferToShortname, iSelectedIndex)))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailedText = ACRenFailedCreateEvent
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            bGenerateDocuments = True

            If bIsTrueMonthlyPolicy Then
                If lAnniversaryCopy <> 1 Then

                    m_lReturn = m_oBusiness.GenerateCustomerRenewalEmail(v_lPartyCnt:=lPartyCnt, v_lInsuranceFileCnt:=lNewPolicyCnt, v_sType:="acceptance")

                    bGenerateDocuments = False
                End If
            End If

            If bGenerateDocuments Then

                m_lReturn = m_oBusiness.GenerateCustomerRenewalEmail(v_lPartyCnt:=lPartyCnt, v_lInsuranceFileCnt:=lNewPolicyCnt, v_sType:="acceptance")
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                    m_lReturn = g_oRenewal.GetProdPrintOptions(lProductId, vPrintOptions)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vPrintOptions) Then
                        bProduceSchedule = gPMFunctions.ToSafeBoolean(vPrintOptions(0, 0))
                        bProduceCertificate = gPMFunctions.ToSafeBoolean(vPrintOptions(1, 0))
                        bProduceDebitNote = gPMFunctions.ToSafeBoolean(vPrintOptions(2, 0))
                    End If

                    If (m_sRenSchedulePrinting = "1" Or m_sRenSchedulePrinting = "0") And bProduceSchedule Then
                        'Generate schedule document.
                        m_lReturn = GenerateDocument(ACDocTypeSchedule, ACSpoolSilentMode, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal - Schedule Document")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sFailedText = ACRenFailedScheduleDoc
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, iSelectedIndex)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    If (m_sRenCertificatePrinting = "1" Or m_sRenCertificatePrinting = "0") And bProduceCertificate Then
                        'Generate certificate document.
                        m_lReturn = GenerateDocument(ACDocTypeCertificate, ACSpoolSilentMode, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal -  Certificate Document")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sFailedText = ACRenFailedCertificateDoc
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, iSelectedIndex)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    If (m_sRenDebitNotePrinting = "1" Or m_sRenDebitNotePrinting = "0") And bProduceDebitNote Then
                        'DN 21/02/03 - Generate debit note.
                        'PSL 10/09/2003 Iss 6535 needs a seperate debit note to new business
                        m_lReturn = GenerateDocument(ACDOCTypeDebitNote, ACSpoolSilentMode, lNewPolicyCnt, lInsuranceFolder, lPartyCnt, "Accept Renewal -  Debit Note Document")

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sFailedText = ACRenFailedDebitNoteDoc
                            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, iSelectedIndex)).Trim())
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If
            End If

            'unlock renewal policy

            ProcessLock(v_bAcquireLock:=False, v_sKeyName:=ACPolicyLockName, v_nKeyValue:=m_vRenewalsData(ACIRenewalInsuranceFolder, iSelectedIndex), v_nUserID:=g_oObjectManager.UserID, v_sInsuranceRef:=CStr(m_vRenewalsData(ACIRenewalLivePolicy, iSelectedIndex)).Trim())


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AcceptRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsQuoted
    '
    ' Description: is policy quoted
    '
    ' History: 09/07/2001 TN - Created.
    '
    ' Thinh Nguyen 27/09/2002 - add optional parameter to display message
    ' ***************************************************************** '
    Public Function IsQuoted(ByVal v_lSelectedIndex As Integer, Optional ByVal v_lDisplay As Integer = 1) As Integer

        Dim result As Integer = 0
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim lInsuranceFileCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lInsuranceFileCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, v_lSelectedIndex))

            result = g_oRenewal.IsQuoted(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lResult:=lIsQuoted)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If lIsQuoted <> gPMConstants.PMEReturnCode.PMTrue Then
                'Thinh Nguyen 27/09/2002 start - only display if mode is set
                If v_lDisplay = 1 Then
                    MessageBox.Show("This Policy Is Not Quoted!" & Strings.Chr(13) & Strings.Chr(10) & "Cannot Accept This Policy", "Accept Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                'Thinh Nguyen 27/09/2002 end - only display if mode is set
            End If

            Return lIsQuoted

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsQuoted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsQuoted", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetStats
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetStats(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef v_bIsTrueMonthlyPolicy As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim iAllPaynow As Integer
        Dim vCreditTransactions(,) As Object
        Dim cGrossTotalForSelectedPolicy As Decimal
        Dim itemp As Integer
        Dim vKeys(1, 9) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oObject = CreateLateBoundObject("iPMUStats.Interface_Renamed")

            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error init", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:="REN")

            vKeys(0, 0) = "insurance_file_cnt"

            vKeys(1, 0) = v_lInsuranceFileCnt
            'In case only Single Policy is Selected and PayNow option is used for Payment
            m_lReturn = AllPolicyPayNow(iAllPaynow)

            'Start - Prakash - WPR85_Paralleling
            If Not m_bMultiSelect And (iAllPaynow = 1 Or iAllPaynow = 3) Then

                vCreditTransactions = VB6.CopyArray(m_vCreditTransactions)
            End If
            'End - Prakash - WPR85_Paralleling

            If m_bMultiSelect And iAllPaynow = 1 Then
                'Get the Policy GrossTotal
                If Information.IsArray(m_vCreditTransactions) Then
                    m_lReturn = GetPolicyGrossTotal(lInsuranceFileCnt:=v_lInsuranceFileCnt, cValue:=cGrossTotalForSelectedPolicy)
                    itemp = 0
                    For iLV As Integer = 0 To m_vCreditTransactions.GetUpperBound(1)
                        If CDbl(m_vCreditTransactions(2, iLV)) > 0 Then
                            If Information.IsArray(vCreditTransactions) Then

                                ReDim Preserve vCreditTransactions(2, vCreditTransactions.GetUpperBound(1) + 1)
                            Else

                                ReDim vCreditTransactions(2, 0)
                            End If

                            vCreditTransactions(0, itemp) = m_vCreditTransactions(0, iLV)

                            vCreditTransactions(1, itemp) = m_vCreditTransactions(1, iLV)

                            If cGrossTotalForSelectedPolicy >= m_vCreditTransactions(2, iLV) Then

                                vCreditTransactions(2, itemp) = m_vCreditTransactions(2, iLV)

                                cGrossTotalForSelectedPolicy -= gPMFunctions.ToSafeCurrency(CDbl(m_vCreditTransactions(2, iLV)))
                                m_vCreditTransactions(2, iLV) = 0
                            Else

                                vCreditTransactions(2, itemp) = cGrossTotalForSelectedPolicy
                                m_vCreditTransactions(2, iLV) = CDbl(m_vCreditTransactions(2, iLV)) - cGrossTotalForSelectedPolicy
                                cGrossTotalForSelectedPolicy = 0

                            End If
                            If cGrossTotalForSelectedPolicy <= 0 Then Exit For
                            itemp += 1
                        End If

                    Next
                End If
            Else
                m_lReturn = GetPolicyGrossTotal(lInsuranceFileCnt:=v_lInsuranceFileCnt, cValue:=cGrossTotalForSelectedPolicy)
            End If
            'If iAllPaynow Then

            vKeys(0, 1) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeys(1, 1) = v_lInsuranceFileCnt

            vKeys(0, 2) = PMNavKeyConst.PMKeyNamePaymentAccountID

            vKeys(1, 2) = m_lPaymentAccountID

            vKeys(0, 3) = PMNavKeyConst.PMKeyNameDebitAgainst

            vKeys(1, 3) = m_iDebitAgainst

            vKeys(0, 4) = PMNavKeyConst.PMKeyNameCreditTransactions

            vKeys(1, 4) = vCreditTransactions

            vKeys(0, 5) = PMNavKeyConst.PMKeyNameCashListID

            vKeys(1, 5) = m_lCashListID

            vKeys(0, 6) = PMNavKeyConst.PMKeyNameCashListItemID

            vKeys(1, 6) = m_lCashListItemID

            vKeys(0, 7) = PMNavKeyConst.PMKeyNameTransactionID

            vKeys(1, 7) = m_lTransactionID

            vKeys(0, 8) = PMNavKeyConst.PMKeyNameIsTrueMonthlyPolicy

            vKeys(1, 8) = v_bIsTrueMonthlyPolicy
            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

            vKeys(0, 9) = PMNavKeyConst.PMKeyNameRoundOffAmount

            vKeys(1, 9) = m_cRoundOffAmount
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

            'End If
            m_lReturn = oObject.SetKeys(vKeyArray:=vKeys)

            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error start", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lStatus = oObject.Status

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccumulations
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetAccumulations(ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        'commented as less rows and columns required
        'Dim vKeys(2, 1) As Object
        Dim vKeys(1, 0) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oObject = CreateLateBoundObject("iPMUAccumulationValues.Interface_Renamed")

            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error init", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            vKeys(0, 0) = "insurance_file_cnt"

            vKeys(1, 0) = v_lNewInsuranceFileCnt
            m_lReturn = oObject.SetKeys(vKeyArray:=vKeys)

            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error start", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lStatus = oObject.Status

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccumulations Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccumulations", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: EnableDisableButtons
    '
    ' Description:
    '
    ' History: 07/11/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Sub EnableDisableButtons(Optional ByVal v_bDisableAll As Boolean = False)

        Try

            If lvwRenewals.Items.Count = 0 Then
                cmdSelectAll.Enabled = False
                v_bDisableAll = True
            Else
                cmdSelectAll.Enabled = True
            End If

            If v_bDisableAll Then
                cmdTransfer.Enabled = False
                cmdAccept.Enabled = False
                cmdLapse.Enabled = False
                cmdDelete.Enabled = False
                cmdAmmend.Enabled = False
                cmdChangeStatus.Enabled = False
            Else
                If m_bMultiSelect Then
                    cmdTransfer.Enabled = True
                    cmdAccept.Enabled = True
                    cmdLapse.Enabled = True
                    cmdDelete.Enabled = False
                    cmdAmmend.Enabled = True
                    cmdChangeStatus.Enabled = True
                Else
                    cmdTransfer.Enabled = True
                    cmdAccept.Enabled = True
                    cmdLapse.Enabled = True
                    cmdDelete.Enabled = True
                    cmdAmmend.Enabled = True
                    cmdChangeStatus.Enabled = True
                End If
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    '*****************************************************************
    ' Name : CreateInstalmentQuote
    '
    ' Desc : create an instalment quote if current policy version has instalment plan
    '
    '*****************************************************************
    Private Function CreateInstalmentQuote(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_vPlanArray(,) As Object, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Dim lPremiumFinanceVer As Integer
        Dim lpfschemenoorg, lpfschemeversionorg, lpfschemenoren, lpfschemeversionren, lpfpremfinancecntren, lpfpremfinanceversionren As Integer
        Dim vPreviousSchemeDetails(,) As Object
        Dim lPremiumFinanceCnt As Long

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPremiumFinance Is Nothing Then
                Dim temp_m_oPremiumFinance As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPremiumFinance = temp_m_oPremiumFinance
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    r_sFailureMessage = "Failed to instantiate bSIRPremiumFinance.Business"

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRPremiumFinance.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInstalmentQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oPremiumFinance.GetPreviousPlanSelectedFromInsuranceFile(v_lInsuranceFileCnt:=v_lOriginalInsuranceFileCnt, r_vPreviousSchemeDetails:=vPreviousSchemeDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateInstalmentQuote", "Failed to get Previous Scheme Details")
            End If
            If Information.IsArray(vPreviousSchemeDetails) Then
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfschemenoorg = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(3, 0)))
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfschemeversionorg = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(4, 0)))
            End If
            vPreviousSchemeDetails = Nothing

            m_lReturn = m_oPremiumFinance.GetPreviousPlanSelectedFromInsuranceFile(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, r_vPreviousSchemeDetails:=vPreviousSchemeDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateInstalmentQuote", "Failed to get Previous Scheme Details")
            End If
            If Information.IsArray(vPreviousSchemeDetails) Then
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfpremfinancecntren = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(0, 0)))
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfpremfinanceversionren = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(1, 0)))
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfschemenoren = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(3, 0)))
                'UPGRADE_WARNING: (1068) vPreviousSchemeDetails() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lpfschemeversionren = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(4, 0)))
            End If

            ' Donot delete/insert plan if already changed during renewal amendment
            If lpfschemenoorg = lpfschemenoren And lpfschemeversionorg = lpfschemeversionren Then
                'Delete the original quote and create a new one

                m_lReturn = m_oPremiumFinance.DeletePlanForOneInsFile(v_lRenewalInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to delete quote plan for renewal version Policy ID " & v_lRenewalInsuranceFileCnt
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'create quote plan if we don't have one for renewal version

                m_lReturn = m_oPremiumFinance.CopyInstalmentPlanForRenewals(v_lOriginalInsuranceFileCnt:=v_lOriginalInsuranceFileCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lPartyCnt:=v_lPartyCnt, r_lPremiumFinanceCnt:=lPremiumFinanceCnt, r_lPremiumFinanceVer:=lPremiumFinanceVer)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to create quote plan for renewal version Policy ID " & v_lRenewalInsuranceFileCnt
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Don't delete but update status Or just mark the status to PFStatusIndSaved
                m_bDontDeleteScheme = True
            End If

            m_lReturn = m_oPremiumFinance.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, r_vPFPremiumFinance:=r_vPlanArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to get instalment quote details for Policy ID " & v_lRenewalInsuranceFileCnt
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            r_sFailureMessage = excep.Message

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateInstalmentQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInstalmentQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*****************************************************************
    ' Name : AppendText
    '
    ' Desc : write to text file depend on mode.
    '
    ' Thinh Nguyen 27/09/2002 - created
    '
    '*****************************************************************
    Private Function AppendText(ByVal v_sFile As String, ByVal v_sTextLine As String, Optional ByVal v_sMode As String = "Output") As Integer

        Dim result As Integer = 0
        Dim lFileNo As Integer

        Try

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

            FileSystem.FileClose(lFileNo)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AppendText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AppendText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*****************************************************************
    ' Name : SpoolDoc
    '
    ' Desc : send document to document spooler (just a text file not a normal merge doc)
    '
    ' Thinh Nguyen 01/10/2002 - created
    '
    '*****************************************************************
    Private Function SpoolDoc(ByVal v_sFileName As String, ByVal v_sSpoolDesc As String) As Integer
        Dim result As Integer = 0


        Dim oDocTemplate As Object
        Dim lDocTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oRenewal.GetDocTypeID(v_sDocCode:="LETTER", r_lDocTypeID:=lDocTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document type id", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim temp_oDocTemplate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oDocTemplate = temp_oDocTemplate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            oDocTemplate.PartyCnt = m_lSelPartyCnt

            oDocTemplate.InsuranceFileCnt = m_lSelNewPolicyCnt

            oDocTemplate.DocName = v_sFileName

            oDocTemplate.SpoolDesc = v_sSpoolDesc

            oDocTemplate.DocumentTypeId = lDocTypeId

            oDocTemplate.Mode = 5 'spool report

            result = oDocTemplate.Start()

            'delete original file after spooling
            If result = gPMConstants.PMEReturnCode.PMTrue Then

                'File.Delete(v_sFileName.Substring(0, v_sFileName.Length - 3) & "*")
                For Each FileFound As String In Directory.GetFiles(v_sFileName.Substring(0, v_sFileName.LastIndexOf("\")), v_sFileName.Substring(v_sFileName.LastIndexOf("\") + 1, v_sFileName.Length - v_sFileName.LastIndexOf("\") - 5) & ".*")
                    File.Delete(FileFound)
                Next
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Sub mnuSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSearch.Click

        Dim oSearch As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            oSearch = CreateLateBoundObject("iPMListViewSearch5.Interface_Renamed")

            If Not Information.IsReference(oSearch) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create search object", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            lReturn = oSearch.LvwSearch(oSearchList:=Me.lvwRenewals)
            lvwRenewals.Focus()
            lvwRenewals_Click(lvwRenewals, New EventArgs())

            oSearch = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="mnuSearch_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    '*****************************************************************
    ' run plan maintenance
    '*****************************************************************
    Private Function RunPlanMaintenance(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_sFailureMessage As String) As Integer
        Dim result As Integer = 0
        Dim iPMBFinancePlanMaint As Object

        Dim oObject As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            If g_oObjectManager.GetInstance(temp_oObject, "iPMBFinancePlanMaint.Interface_Renamed", gPMConstants.PMGetLocalInterface) <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = temp_oObject
                r_sFailureMessage = "Failed to create an instance of iPMBFinancePlanMaint.Interface"
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            Else
                oObject = temp_oObject
            End If


            If oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="NB") <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to set process mode for iPMBFinancePlanMaint.Interface"
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            oObject.FinancePlanCnt = v_lFinancePlanCnt

            oObject.FinancePlanVersion = v_lFinancePlanVersion

            oObject.Spawned = True
            oObject.DontDeleteScheme = m_bDontDeleteScheme

            If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to start iPMBFinancePlanMaint.Interface"
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = Information.Err().Description

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="mnuSearch_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If


        End Try
        Return result
    End Function

    Private Function GetBrokerTransferAuthority() As Integer

        Dim result As Integer = 0
        Dim vResult As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If IsNothing(g_oObjectManager) Then
                g_oObjectManager = New bObjectManager.ObjectManager
            End If

            result = g_oRenewal.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="can_perform_broker_transfer", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMInteger, r_vResult:=vResult)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                m_bCanTransferBroker = (gPMFunctions.ToSafeLong(vResult, 0) = 1)
            End If



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get user authority for broker transfer portfolio", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBrokerTransferAuthority()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Function ListViewSelected(ByVal v_oListView As ListView) As Integer

        Dim lSelected As Integer

        Try

            lSelected = 0
            For lCount As Integer = 1 To v_oListView.Items.Count
                If v_oListView.Items.Item(lCount - 1).Selected Then
                    lSelected += 1
                End If
            Next



        Catch ex As Exception
            lSelected = -1

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to count selected items on listview", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSelected()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
        Return lSelected
    End Function

    Private Sub RepopulateRenewal(ByVal v_lRenewalInsuranceFileCnt As Integer, ByRef v_lListViewIndex As Integer, ByRef v_lArrayIndex As Integer)

        Dim vResultArray(,) As Object
        Dim oListItem As ListViewItem
        Dim sFailmsg As String = ""

        Try


            If GetBusiness(vResultArray, v_lRenewalInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailmsg = "Failed to get new data for policy Insurance File Cnt = " & v_lRenewalInsuranceFileCnt
                Throw New Exception(sFailmsg)
            End If

            'update renewal array with new data

            For lCount As Integer = 0 To vResultArray.GetUpperBound(0)

                m_vRenewalsData(lCount, v_lArrayIndex) = vResultArray(lCount, 0)
            Next lCount

            'update list view with new data
            oListItem = lvwRenewals.Items.Item(v_lListViewIndex - 1)

            'Col 1 branch
            oListItem.Text = CStr(m_vRenewalsData(ACIRenewalSourceCode, v_lArrayIndex)).Trim()

            'col 2 client
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRenewalsData(ACIRenewalShortname, v_lArrayIndex)).Trim()

            'col 3 policy no
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRenewalsData(ACIRenewalLivePolicy, v_lArrayIndex)).Trim()

            'col 4 agent
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vRenewalsData(ACIRenewalLeadAgentCode, v_lArrayIndex)).Trim()

            'col 5 account handler
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vRenewalsData(ACIRenewalAccHandlerCode, v_lArrayIndex)).Trim()

            'col 6 renewal date
            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vRenewalsData(ACIRenewalCoverStartDate, v_lArrayIndex)).Trim()

            'col 7 status
            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vRenewalsData(ACIRenewalStatusType, v_lArrayIndex)).Trim()

            'Start PN: 71757
            'col 8 Exception Reason
            ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vRenewalsData(ACIRenewalExceptionReason, v_lArrayIndex)).Trim()

            'col 9 product
            ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(m_vRenewalsData(ACIRenewalProduct, v_lArrayIndex)).Trim()

            'col 10 claim indicator
            ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(m_vRenewalsData(ACIRenewalClaimsIndicator, v_lArrayIndex)).Trim()

            'col 11 closed branched
            ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(m_vRenewalsData(ACIRenewalClosedBranch, v_lArrayIndex)).Trim()

            'col 12 transfer broker to
            If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalStatusTypeId, v_lArrayIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalTransferToPartyCnt, v_lArrayIndex)), 0) = 0 Then
                    ListViewHelper.GetListViewSubItem(oListItem, 11).Text = "Direct"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 11).Text = CStr(m_vRenewalsData(ACIRenewalTransferToShortname, v_lArrayIndex)).Trim()
                End If
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 11).Text = ""
            End If
            'End PN: 71757


        Catch ex As Exception
            If sFailmsg = "" Then
                sFailmsg = "Failed to repopulate listview with new data"
            End If

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lRenewalInsuranceFileCnt", v_lRenewalInsuranceFileCnt)
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailmsg, vApp:=ACApp, vClass:=ACClass, vMethod:="RepopulateRenewal()", excep:=ex, oDicParms:=oDict)

        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: ValidateTMPPolicy
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-10-2005 : True Monthly Policy
    ' ***************************************************************** '
    Public Function ValidateTMPPolicy(ByVal v_lSelectedIndex As Integer, ByRef r_bIsValid As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateTMPPolicy"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNewPolicyCnt, lAnniversaryCopy As Integer
        Dim sInsuranceRef As String = ""
        Dim vValidationResults(,) As Object
        Dim bIsTrueMonthlyPolicy, bAcceptIsValid As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get renewal details
            lNewPolicyCnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, v_lSelectedIndex))
            sInsuranceRef = CStr(m_vRenewalsData(ACIRenewalPolicy, v_lSelectedIndex))
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalProductIsTrueMonthlyPolicy, v_lSelectedIndex)), 0) = 1
            lAnniversaryCopy = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalAnniversaryCopy, v_lSelectedIndex)), 0)

            ' default to accept being a valid action
            bAcceptIsValid = True

            ' if this is an "anniversary copy" version of a policy
            ' based on a "true monthly policy" based product
            If bIsTrueMonthlyPolicy Then

                If lAnniversaryCopy Then

                    ' then validate that the last item of the previous cycle
                    ' has already been accepted if not then this item cannot be accepted

                    lReturn = g_oRenewal.ValidateAcceptTMPIsValidAction(v_lInsuranceFileCnt:=lNewPolicyCnt, v_sInsuranceRef:=sInsuranceRef, r_vResults:=vValidationResults)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "IsAcceptTMPValid Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Information.IsArray(vValidationResults) Then

                        If gPMFunctions.ToSafeLong(CStr(vValidationResults(0, 0)), 0) = 0 Then
                            bAcceptIsValid = False
                        End If
                    Else
                        bAcceptIsValid = False
                    End If

                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            r_bIsValid = bAcceptIsValid

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function SetStatusBar() As Integer
        Dim result As Integer = 0
        Dim s As String = ""
        Const kMethodName As String = "SetStatusBar"
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            With stbStatus

                If m_lAgentId > 0 Then
                    s = .Items.Item(0).Text
                    .Items.Clear()


                    '.style = MSComctlLib.SbarStyleConstants.sbrNormal

                    .Items.Add(New ToolStripStatusLabel(1))

                    .Items.Add(New ToolStripStatusLabel(2))

                    .Items.Add(New ToolStripStatusLabel(3))

                    .Items.Add(New ToolStripStatusLabel(4))
                    .Items.Item(0).Text = s
                Else


                    '.style = MSComctlLib.SbarStyleConstants.sbrSimple
                End If
            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Private Function DisplayAccountDetails() As Integer
        Dim result As Integer = 0

        Dim lAccountID As Integer
        Dim sFormattedBalance As String = ""
        Dim vResultArray(,) As Object
        Dim sFormattedAccountBalance, sFormattedFloatBalance, sFormattedOverDraftBalance As String

        Dim sErrMsg As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness

        m_lReturn = m_oBusiness.getUnderwritingOrAgency

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "Failed to get bSIRRenewal.Business"
            GoTo Catch_Renamed
        End If

        Dim m_oCurrencyConvert As Object
        'Get Currency Convert Object.
        Dim temp_m_oCurrencyConvert As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oCurrencyConvert = temp_m_oCurrencyConvert

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "Failed to Get bACTCurrencyConvert.Form"
            GoTo Catch_Renamed
        End If

        m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

        'Get Account Object.
        Dim temp_m_oAccount As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oAccount = temp_m_oAccount
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sErrMsg = "Failed to get bActAccount Instance"
            GoTo Catch_Renamed
        End If

        If m_lAgentId > 0 Then

            m_lReturn = m_oBusiness.GetAccountID(m_lAgentId, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sErrMsg = "Failed to Execute GetAccountID"
                GoTo Catch_Renamed
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sErrMsg = "Account Datails not Found for Agent"
                GoTo Catch_Renamed
            End If


            lAccountID = gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0)))


            m_lReturn = m_oAccount.GetAccountBalance(v_vAccountID:=lAccountID, v_vAccountingDate:=DateTime.Today, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sErrMsg = "Failed to execute GetAccountBalance"
                GoTo Catch_Renamed
            End If
            If Information.IsArray(vResultArray) Then

                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vResultArray(1, 0), vCurrencyAmount:=vResultArray(0, 0), vFormattedCurrency:=sFormattedAccountBalance)


                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vResultArray(1, 0), vCurrencyAmount:=vResultArray(2, 0), vFormattedCurrency:=sFormattedFloatBalance)


                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=vResultArray(1, 0), vCurrencyAmount:=vResultArray(3, 0), vFormattedCurrency:=sFormattedOverDraftBalance)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sErrMsg = "Failed to Execute FormatCurrency"
                GoTo Catch_Renamed
            End If

            With stbStatus
                .Items.Item(1).Text = "Account Balance: " & sFormattedAccountBalance.Trim()
                .Items.Item(2).Text = "Float Balance: " & sFormattedFloatBalance.Trim()
                .Items.Item(3).Text = "OverDraft Balance: " & sFormattedOverDraftBalance.Trim()

                .Items.Item(0).Width = VB6.TwipsToPixelsX(3000)

                If CStr(vResultArray(0, 0)).Trim() = "" Then
                    .Items.Item(1).Visible = False
                Else
                    .Items.Item(1).Width = VB6.TwipsToPixelsX(3200)
                End If


                If CStr(vResultArray(2, 0)).Trim() = "" Then
                    .Items.Item(2).Visible = False
                Else
                    .Items.Item(2).Width = VB6.TwipsToPixelsX(2900)
                End If

                If CStr(vResultArray(3, 0)).Trim() = "" Then
                    .Items.Item(3).Visible = False
                Else
                    .Items.Item(3).Width = VB6.TwipsToPixelsX(3400)
                End If

            End With

        End If
        Return result
Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAccountDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    'Deepak
    Private Function AllPolicyPayNow(ByRef value As Integer) As Integer

        'If return value=0 : Payment Method of Selected Policies are either Invoice/Instalment
        'If return value=1 : Payment Method of Selected Policies is PayNow
        'If return value=2 : Payment Method of Selected Policies are either Paynow/Instalment/Invoice
        'Start - Prakash - WPR85_Paralleling
        'If return value=3 : Payment Method of any one of selected policies is Cash Deposit
        Dim result As Integer = 0
        Dim bCashDeposit As Boolean
        'End - Prakash - WPR85_Paralleling
        Dim lPolicycnt As Integer
        Dim sPaymentMethod As String = ""
        Dim bPayNow, bInvoiceorInstalment As Boolean
        Dim iPosInArray As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then
                    'Get the Payment Method

                    'Find item position in array
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                        End If
                    Next

                    lPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray))
                    sPaymentMethod = gPMFunctions.ToSafeString(CStr(m_vRenewalsData(ACIRenewalPaymentMethod, iPosInArray))).Trim()

                    'm_lReturn = g_oRenewal.GetPaymentMethod(v_lInsuranceFileCnt:=lPolicycnt, r_sPaymentMethod:=sPaymentMethod)
                    If sPaymentMethod = "PayNow" Then bPayNow = True
                    If sPaymentMethod = "Invoice" Or sPaymentMethod = "" Or sPaymentMethod = "Instalment" Then bInvoiceorInstalment = True
                    'Start - Prakash - WPR85_Paralleling
                    If sPaymentMethod = "CashDeposit" Then bCashDeposit = True
                    'End - Prakash - WPR85_Paralleling
                End If
            Next
            'Start - Prakash - WPR85_Paralleling
            If bCashDeposit Then
                value = 3
            Else
                If Not bPayNow And Not bInvoiceorInstalment Then value = 0
                If Not bPayNow And bInvoiceorInstalment Then value = 0
                If bPayNow And Not bInvoiceorInstalment Then value = 1
                If bPayNow And bInvoiceorInstalment Then value = 2
            End If
            'End - Prakash - WPR85_Paralleling
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="All Policy PayNow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllPolicypaynow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function AllPolicyInvoice(ByRef value As Integer) As Integer

        'If return value=1 : Payment Method of Selected Policies is Invoice
        'If return value=0: Payment Method of Selected Policies are either Paynow/Instalment/Invoice

        Dim result As Integer = 0
        Dim lPolicycnt As Integer
        Dim sPaymentMethod As String = ""
        Dim bInvoice, bPayNoworInstalment As Boolean
        Dim iPosInArray As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then
                    'Get the Payment Method

                    'Find item position in array
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                        End If
                    Next

                    lPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray))

                    m_lReturn = g_oRenewal.GetPaymentMethod(v_lInsuranceFileCnt:=lPolicycnt, r_sPaymentMethod:=sPaymentMethod)
                    If sPaymentMethod = "Invoice" Then bInvoice = True
                    If sPaymentMethod = "PayNow" Or sPaymentMethod = "" Or sPaymentMethod = "Instalment" Or sPaymentMethod = "Direct Debit" Then bPayNoworInstalment = True

                End If
            Next
            If bInvoice And Not bPayNoworInstalment Then
                value = 1
            Else
                value = 0
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllPolicyInvoice Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllPolicyInvoice", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    Private Function CheckCurrencyAndAgentType(ByRef bAllowed As Boolean) As Integer

        'Checks for the Cuurency Type and Agent Type.
        'If Selected policies have different currency or made through commission agent, it does not allow to proceed.
        Dim result As Integer = 0
        Dim lPolicycnt As Integer
        Dim iPosInArray As Integer
        Dim lCurrency, lCurrency_prev As Integer
        Dim sAgentType, sPaymentMethod As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bAllowed = True

            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then

                    'Find item position in array
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                        End If
                    Next

                    lPolicycnt = CInt(m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray))

                    m_lReturn = g_oRenewal.GetCurrencyAndAgentType(v_lInsuranceFileCnt:=lPolicycnt, r_lCurrency:=lCurrency, r_sAgentType:=sAgentType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCurrencyAndAgentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCurrencyAndAgentType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    m_lReturn = g_oRenewal.GetPaymentMethod(v_lInsuranceFileCnt:=lPolicycnt, r_sPaymentMethod:=sPaymentMethod)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCurrencyAndAgentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCurrencyAndAgentType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    If lCurrency_prev = 0 Then lCurrency_prev = lCurrency
                    If sPaymentMethod = "PayNow" Then 'Parallel PN 74558
                        If lCurrency_prev <> lCurrency Then
                            MessageBox.Show("Selected policies have different currency. Policies must be renewed individually", Application.ProductName)
                            bAllowed = False
                            Return result
                        End If
                    End If
                    If sAgentType.Trim() = "Comm Acc" Then
                        MessageBox.Show("Agent Selected is a Commission Account. Policies must be renewed individually", Application.ProductName, MessageBoxButtons.OK)
                        bAllowed = False
                        Return result
                    End If

                End If
            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCurrencyAndAgentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCurrencyAndAgentType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function GetPolicyGrossTotal(ByVal lInsuranceFileCnt As Integer, ByRef cValue As Decimal) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult(,) As Object
        Dim vGrosstotal As Decimal
        Dim sErrMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = g_oRenewal.GetPolicyGrossTotal(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResults:=vResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetPolicyGrossTotal"
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then

                vGrosstotal = CDec(vResult(4, 0))
            End If
            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            If m_bRoundOff Then
                m_cRoundOffAmount = gPMMaths.PMRoundupValueCurrency(vGrosstotal, gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero, gPMConstants.PMERoundupFactor.pmeRFactor50Up) - vGrosstotal
            End If
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            cValue = vGrosstotal

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyGrossTotal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    'Gives the grossTotal of all the Selected Policies
    Private Function GetGrossTotal(ByRef value As Decimal) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult(,) As Object
        Dim vGrosstotal As Decimal
        Dim sErrMsg As String = ""
        Dim iPosInArray As Integer
        Dim sAgentType As String = ""
        Dim cAgentcommission As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then

                    'Find item position in array
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                        End If
                    Next


                    lReturn = g_oRenewal.GetPolicyGrossTotal(v_lInsuranceFileCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray), r_vResults:=vResult)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sErrMsg = "Failed to Get GetPolicyGrossTotal"
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If Information.IsArray(vResult) Then

                        vGrosstotal += CDec(vResult(4, 0))
                    End If
                    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
                    If m_bRoundOff Then
                        m_cRoundOffAmount = gPMMaths.PMRoundupValueCurrency(vGrosstotal, gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero, gPMConstants.PMERoundupFactor.pmeRFactor50Up) - vGrosstotal
                    End If
                    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

                    vResult = Nothing


                    lReturn = g_oRenewal.GetAgentCommission(v_lInsuranceFileCnt:=m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray), r_vResults:=vResult)

                    If Information.IsArray(vResult) Then

                        sAgentType = CStr(vResult(1, 0)).Trim()
                        If sAgentType = "Broker" Then

                            cAgentcommission = gPMFunctions.ToSafeCurrency(CDbl(vResult(6, 0))) + gPMFunctions.ToSafeCurrency(gPMFunctions.ToSafeDouble(vResult(16, 0), 0))
                            vGrosstotal -= cAgentcommission
                        End If
                    End If


                End If

            Next
            value = vGrosstotal + m_cRoundOffAmount 'Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGrossTotal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function
    Private Function ShowPayNow(ByRef sPaymentMethod As String) As Integer
        Dim result As Integer = 0
        Dim iPMUPaynowOptions As Object

        Dim oPayNow As Object
        Dim sErrMsg As String = ""
        Dim lGrossTotal As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oPayNow As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPayNow, sClassName:="iPMUPayNowOptions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPayNow = temp_oPayNow
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to get PayNow Instance"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find item position in array
            Dim iPosInArray As Integer

            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                        End If
                    Next
                End If
            Next


            oPayNow.PaymentOption = sPaymentMethod

            If Not m_bMultiSelect Then
                m_lReturn = GetGrossTotal(lGrossTotal)

                oPayNow.InsuranceFileCnt = m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray)

                oPayNow.AmountDue = lGrossTotal
            Else
                m_lReturn = GetGrossTotal(lGrossTotal)

                oPayNow.MultiplePoliciesSelected = True

                oPayNow.Agentcnt = m_lAgentId

                oPayNow.AmountDue = lGrossTotal
            End If

            oPayNow.PrePayment = m_oPrepayment(0, 0)
            m_lReturn = oPayNow.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "PayNow.Start Failed"
                Return m_lReturn
            End If


            If oPayNow.OKClick Then
                'Get Values from iPMUPaynowOptions for GetKeys()

                m_lPaymentAccountID = oPayNow.PaymentAccountID

                m_iDebitAgainst = oPayNow.DebitAgainst

                m_vCreditTransactions = VB6.CopyArray(oPayNow.CreditTransactions)

                m_lCashListID = oPayNow.CashListID

                m_lCashListItemID = oPayNow.CashListItemID

                m_lTransactionID = oPayNow.CashTransDetailID
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPayNow", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateRenewalAcceptance
    '
    ' Parameters: Insurance_file_cnt
    '
    ' Description:
    '
    ' History:
    '           Created : Pankaj : 10-01-2008 : Unattended Renewals
    ' ***************************************************************** '
    Private Function ValidateRenewalAcceptance(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bPolicyNumberToChange As Boolean, ByRef r_bNoRenewalInstalmentPlan As Boolean, ByRef r_bPrepaymentRequired As Boolean) As Integer

        Dim nResult As Integer = 0
        Dim sExceptionNote, sMsg As String

        Try



            nResult = g_oRenewal.ValidateRenewalAcceptance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_bPolicyNumberToChange:=r_bPolicyNumberToChange, r_bNoRenewalInstalmentPlan:=r_bNoRenewalInstalmentPlan, r_bPrepaymentRequired:=r_bPrepaymentRequired)


            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                sMsg = "Failed to validate renewal acceptance"
                Throw New Exception(sMsg)
            End If

            sExceptionNote = ""

            '1. Set Exception Reason when policy flagged for policy number change
            If r_bPolicyNumberToChange Then
                'Start [Nitesh Dwivedi] PN 71271
                sExceptionNote = "Product flagged for policy number change ? manual acceptance required"
                'End  [Nitesh Dwivedi] PN 71271

                nResult = g_oRenewal.UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=1, v_sRenewalExceptionNote:=sExceptionNote)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If

                Return nResult
            End If

            '2. Set Exception Reason when bank details are missing
            If r_bNoRenewalInstalmentPlan Then
                'Start [Nitesh Dwivedi] PN 71271
                sExceptionNote = "Insufficient Instalment details"
                'End [Nitesh Dwivedi] PN 71271

                nResult = g_oRenewal.UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=2, v_sRenewalExceptionNote:=sExceptionNote)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If

                Return nResult
            End If

            '3. Set Exception Reason when Prepayment Required
            If r_bPrepaymentRequired Then
                'Start Renuka PN 61755
                sExceptionNote = "Prepayment Required"
                'End Renuka PN 61755

                nResult = g_oRenewal.UpdateRenewalExceptions(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalExceptionReasonID:=3, v_sRenewalExceptionNote:=sExceptionNote)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Update Renewal Exceptions"
                    Throw New Exception(sMsg)
                End If

                Return nResult
            End If



        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateRenewalAcceptance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
    'Start - Prakash - WPR85_Paralleling
    Private Function ProcessCashDeposit() As Integer
        Dim result As Integer = 0
        Dim iSIRPolicyCashDeposit As Object

        Dim oCashDeposit As Object
        Dim sErrMsg As String = ""
        'Start - PN 65531
        Dim crGrossTotal, crLeadAgentCommission, crLeadAgentTax As Decimal
        'End - PN 65531

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oCashDeposit As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRPolicyCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oCashDeposit = temp_oCashDeposit
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to get CashDeposit Instance"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find item position in array
            Dim iPosInArray As Integer
            Dim bFlag As Boolean

            bFlag = False
            For iListCount As Integer = lvwRenewals.Items.Count To 1 Step -1
                If lvwRenewals.Items.Item(iListCount - 1).Selected Then
                    For iVar As Integer = 0 To m_vRenewalsData.GetUpperBound(1)
                        If ListViewHelper.GetListViewSubItem(lvwRenewals.Items.Item(iListCount - 1), 2).Text = CStr(m_vRenewalsData(5, iVar)).Trim() Then
                            iPosInArray = iVar
                            bFlag = True
                            Exit For
                        End If
                    Next
                End If
                If bFlag Then Exit For
            Next

            If Not m_bMultiSelect Then


                oCashDeposit.InsuranceFileCnt = gPMFunctions.ToSafeLong(CStr(m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray)), 0)
                'developer guide no. 40
                oCashDeposit.PolicyIssueDate = DateTime.Parse(DateTime.Now)

                oCashDeposit.PrePayment = m_vAllowPayNowOption

                oCashDeposit.CoverFromDate = gPMFunctions.ToSafeDate(m_sRenewalStartDate)

                'Start - Prakash - PN 65531
                m_lReturn = GetPremiumDetails(crGrossTotal:=crGrossTotal, crLeadAgentCommission:=crLeadAgentCommission, crLeadAgentTax:=crLeadAgentTax, lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(m_vRenewalsData(ACIRenewalPolicyCnt, iPosInArray), 0))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = "GetPremiumDetails Failed"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oCashDeposit.LeadAgentCommission = crLeadAgentCommission

                oCashDeposit.LeadAgentTax = crLeadAgentTax

                oCashDeposit.TotalPremium = crGrossTotal
                'End - Prakash - PN 65531

            Else
                'Not currently supporting batch renewal
            End If


            m_lReturn = oCashDeposit.Start

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "CashDeposit.Start Failed"
                Return m_lReturn
            End If


            If oCashDeposit.OKClick Then


                m_lPaymentAccountID = oCashDeposit.PaymentAccountID

                m_iDebitAgainst = oCashDeposit.DebitAgainst

                m_vCreditTransactions = VB6.CopyArray(oCashDeposit.CreditTransactions)
                'Start - PN 65531
                If m_lPaymentAccountID <= 0 Or (Not Information.IsArray(m_vCreditTransactions)) Or gPMFunctions.IsArrayEmpty(m_vCreditTransactions) Then
                    MessageBox.Show("Payment via Cash Deposit Failed", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If
                'End - PN 65531
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
            'Start - PN 65531

            oCashDeposit.Dispose()
            oCashDeposit = Nothing
            'End - PN 65531

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashDeposit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    'Start - PN 61554
    Private Function GetPremiumDetails(ByRef crGrossTotal As Decimal, ByRef crLeadAgentCommission As Decimal, ByRef crLeadAgentTax As Decimal, ByVal lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSirRenewalprocess As Object

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult(,) As Object
        Dim sErrMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim g_oRenewal As Object
            Dim sAgentType As String = ""

            Dim temp_g_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oRenewal, "bSIRRenewalProcess.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oRenewal = temp_g_oRenewal


            lReturn = g_oRenewal.GetPolicyGrossTotal(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResults:=vResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetPolicyGrossTotal"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then

                crGrossTotal = gPMFunctions.ToSafeCurrency(CDbl(vResult(4, 0)), 0)
            End If

            vResult = Nothing


            lReturn = g_oRenewal.GetAgentCommission(lInsuranceFileCnt, vResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrMsg = "Failed to Get GetAgentCommission"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Information.IsArray(vResult) Then

                crLeadAgentCommission = gPMFunctions.ToSafeCurrency(CDbl(vResult(6, 0)))

                crLeadAgentTax = gPMFunctions.ToSafeCurrency(CDbl(vResult(16, 0)))
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPremiumDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function
    'End - PN 61554
    'End - Prakash - WPR85_Paralleling

    Private Function CancelMTAQuotes(ByVal v_lInsuranceFileCnt As Integer,
                                                    ByVal v_lInsuranceFolderCnt As Integer,
                                                                ByVal v_lPartyCnt As Integer) As Integer
        Const kMethodName As String = "CancelMTAQuotes"

        Dim lReturn As Long
        Dim oRenewal As Object
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=temp_oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)
            oRenewal = temp_oRenewal
            ' apply policy discount
            lReturn = oRenewal.CancelMTAQuotes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                                   v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                                   v_lPartyCnt:=v_lPartyCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CancelMTAQuotes, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return result

        Finally

            If Not oRenewal Is Nothing Then
                oRenewal.Dispose()
                oRenewal = Nothing
            End If

        End Try
    End Function


    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sValue As String = ""
        Dim r_sMessage As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            If sValue = "1" Then
                m_lReturn = g_oRenewal.GetAndValidateSubAgentDetailsViaInsFile(bIsValid, lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    g_oRenewal.UpdateRenewalStatus(gPMConstants.PMBRenewalStatusTypeManualReview, r_sMessage)
                    System.Windows.Forms.MessageBox.Show("You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", ACApp, MessageBoxButtons.OK)
                    Return result
                End If
            Else
                bIsValid = True
            End If
            Return result
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function CheckJobBatchRenewalInProcess() As Integer
        Const kMethodName As String = "CheckJobBatchRenewalInProcess"

        Dim iReturn As Integer
        Dim bIsJobBatchRenewalInProcess As Boolean
        Dim oRenewal As Object
        Try

            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=temp_oRenewal,
                sClassName:="bSIRRenewal.Business",
                vInstanceManager:=PMGetViaClientManager)
            oRenewal = temp_oRenewal

            iReturn = oRenewal.CheckJobBatchRenewalInProcess(v_sKey:="ACC",
                                                        r_bIsJobBatchRenewalInProcess:=bIsJobBatchRenewalInProcess)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bIsJobBatchRenewalInProcess Then
                MessageBox.Show("There is a Batch Renewal Accept Run in progress. Please try later.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=CheckJobBatchRenewalInProcess)
            CheckJobBatchRenewalInProcess = gPMConstants.PMEReturnCode.PMFalse
            ' If you want to rollback a transaction or something, do it here
        Finally

            If Not oRenewal Is Nothing Then
                oRenewal = Nothing
            End If

        End Try

    End Function

    ''' <summary>
    ''' Process Lock
    ''' </summary>
    ''' <param name="v_sKeyName"></param>
    ''' <param name="v_nKeyValue"></param>
    ''' <param name="v_nUserID"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_bAcquireLock"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessLock(ByVal v_sKeyName As String, ByVal v_nKeyValue As Integer, ByVal v_nUserID As Integer, ByVal v_sInsuranceRef As String, ByVal v_bAcquireLock As Boolean) As Integer
        Dim nResult As Integer
        Dim sLockedBy As String = ""
        nResult = gPMConstants.PMEReturnCode.PMTrue

        If v_bAcquireLock Then
            m_lReturn = m_oBusiness.LockKey(v_sKeyName:=v_sKeyName, v_nKeyValue:=CInt(v_nKeyValue),
                        v_nUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If sLockedBy = "ERROR" Then
                    MessageBox.Show("Failed to lock policy for, Insurance Folder count : " &
                                    CStr(v_nKeyValue) & Strings.Chr(13) & Strings.Chr(10) &
                                    "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Current policy " & v_sInsuranceRef &
                                    " is being locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) &
                                    "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            If m_oBusiness.UnLockKey(v_sKeyName:=v_sKeyName, v_nKeyValue:=CInt(v_nKeyValue), v_nUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to unlock KeyName: Insurance_folder_cnt" & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(v_nKeyValue) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return nResult
    End Function
End Class
