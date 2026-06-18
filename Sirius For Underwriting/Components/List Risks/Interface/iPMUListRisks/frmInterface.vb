Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports uctPMUListRiskControl
Imports System.Data

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer

    Private m_oBusiness As bSIRListRisks.Business
    Private m_oPMUProduct
    Private m_lReturn As Integer

    '********************************
    Private m_lNoOfPoliciesOnSingleInsAgent As Long

    Private m_oRiskData As bSIRRiskData.Business

    Private m_oRenSelection As bSIRRenSelection.Business

    Private m_oChangePolicyStatus As bSIRChangePolicyStatus.Business

    Private m_oRITax As bSIRRITax.Business

    Private m_oPartyFee As bSIRPartyFee.UBusiness

    Private m_oRenewal As bSIRRenewal.Business

    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lRiskId As Integer
    Private m_lRiskNo As Integer
    Private m_lVariationNo As Integer
    Private m_sFollowUpNote As String = ""
    Private m_sReferredTo As String = ""

    Private m_bAdmiralForceRisks As Boolean
    Private m_lScreenId As Integer
    Private m_lProductId As Integer
    Private m_lRiskTypeId As Integer

    Private m_iIsRiAtRiskLevel As Integer
    Private m_lErrorNumber As Integer
    Private m_sTransactionType As String = ""
    Private m_bCopyRisk As Boolean
    Private m_vOriginalSelStatus As Object
    Private m_iSpecifiedTab As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date

    Private m_bInsuranceFileOnInstalments As Boolean
    Private m_sInstalmentProductCode As String = ""
    Private m_sBusinessType As String = ""
    Private m_bPaymentByInstalmentsAllowed As Boolean
    Private m_sAgentType As String = ""

    Private m_lPFPremFinanceCnt As Integer
    Private m_lPFPremFinanceVersion As Integer
    Private m_sPaymentTerms As String = ""
    Private m_sQuoteStatus As String = ""
    Private m_iMode As Integer
    Private m_sOldPolicyNumber As String = ""
    Private m_sNewPolicyNumber As String = ""
    Private m_vRisks As Object
    Private m_bRoadmapDisablesInstalments As Boolean
    Private m_lSubAgentCount As Integer
    Private m_lOriginalInsuranceFileCnt As Integer ' RAM20050825 - PN 23018
    Private m_iNewBusinessNoTrans As Integer ' PN 43045 --SUR
    Private m_iMTAType As Integer ' RAM20050906 - PN 23653
    Private m_bIsTrueMonthlyPolicy As Boolean
    Private m_bPutOnNextInstalmentRenewal As Boolean
    Private m_lDiscountReasonId As Integer
    Private m_dDiscountPercentage As Double
    Private m_lMatchDiscountedPremium As Integer
    Private m_crTotalPolicyAmount As Decimal
    Private m_bCancelAboutToChangeAction As Boolean
    Private m_crDiscountedPremium As Decimal
    Private m_vArray As Object
    Private m_vArray1 As Object
    Private m_vRiskStatus As Object 'list of all risks and statuses for this policy version
    Private b_IsMakeLiveClicked As Boolean
    Private m_dtAnniversaryDate As Date
    Private m_dtCoverStartDate As Date
    Private m_lLeadAgentCnt As Integer
    'Float Balance and Pre-Payment (RC)
    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_vDebitTransactions As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Private m_sUnderwritingOrAgency As Object

    ' Variables to store the Policy Discounts already applied
    Private m_crAppliedDiscountPremium As Decimal
    Private m_dAppliedDiscountPercentage As Double
    Private m_lAppliedMatchDiscountPremium As Integer
    Private m_lAppliedDiscountReasonId As Integer

    Private m_bApplyDiscountClicked As Boolean
    Private bPaymentOptionAlreadyChecked As Boolean

    'BackDate MTA
    Private m_bIsBackdatedMTARequired As Boolean
    Private m_bMTABackdatedOK As Boolean
    Private m_sApplyMTATaxRatesonRen As String = ""

    Private m_oAutoMTA As bSIRAutoMTA.Business
    Private m_vBackdatedMTAVersions As Object
    'WPR 33-75 added
    Private m_bBackdatedQuoteExists As Boolean

    Private m_cTransactionAmount As Decimal
    Private m_bIsRENaffectedbyBackdateMTA As Boolean
    'PN 32935 (RC)
    Private m_sAgentShortCode As String = ""
    Private m_bIsSingleInstalmentPlan As Boolean
    Private m_vLeadAgentCnt As Integer

    Private m_lCurrencyId As Integer
    Private m_sAgentName As String = ""
    Private m_vPremiumDetails(,) As Object
    Private m_cOriginalGrossPremium As Decimal
    Private m_cPremiumPaid As Decimal
    Private m_lCancelFPOnCancelPolicy As Integer
    Private m_bIsNegativeInstallents As Boolean

    Private m_oProduct As bSIRProduct.Business
    Private m_vUserGroupId As Integer
    Private m_vTaskGroupId As Integer
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_bProcessSettleTransactions As Boolean

    Private m_oProgressBar As iPMBProgressBarWrapper.Wrapper
    Private m_iInsuranceFileTypeID As Integer

    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)
    Dim m_iPolicyRenewalStatus As Integer
    Dim m_iPolicyMakeLiveStatus As Integer
    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx)

    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

    Private m_bRoundOff As Boolean
    Private m_cRoundOffAmount As Decimal
    Private m_lRenewalProcessMode As Integer

    'WPR 33-75 added
    Private m_iSourceID As Integer
    Private m_bWrittenStatusPermitted As Boolean
    Private m_sInsuranceFileStatusCode As String
    Private m_iWrittenTaskManagerDays As Integer
    Private m_iWrittenRemUserGroup As Integer
    Private m_iWrittenRemTaskGroup As Integer
    Private m_bSkipBackDatedCloseSave As Boolean
    Private m_iMandatoryRiskTypeId As Integer
    Private m_bMandatoryRisk As Boolean
    Private m_oLookupValues(,) As Object = Nothing
    Private m_oLookUpDetails(,) As Object = Nothing
    Private m_dtInceptionDateTPI As Date
    Private m_bIsMarketPlacePolicy As Boolean = False
    Private m_vRisksForAutoQuote As Object
    Private m_bAllowReRateAllCancRein As Boolean
    Private m_bAllowReRateAllNBQuotation As Boolean
    Private m_bCanAutoQuoteRisks As Boolean
    Private m_bCanReRateAllRisks As Boolean = False

    Private m_sPaymentCashOrDebit As String
    Private m_oEnableDebitOrder As Object
    Private m_bIsReadyToAccept As Boolean
    Private m_bIsRIAmended As Boolean
    Private m_bIsRiskDeleted As Boolean
    Private m_nInsuranceFileStatusID As Integer
    Private m_bBackdatedEditing As Boolean
    Private m_bEditable As Boolean
    Private m_bFormLoading As Boolean
    Private m_bIsPolicyEdited As Boolean
    Private m_bSaveBackdatedQuotes As Boolean
    Private m_oaRisksForAutoQuote As Object = Nothing
    Private m_bAllRiskQuoted As Boolean = True
    Private m_bIsAllRiskQuoted As Boolean = True
    Private m_sEnableRIRegeneration As Object = Nothing
    Private m_iTransCurrencyID As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_sTransISOCode As String
    Private m_sBaseISOCode As String
    Private m_bSoftDeleteInstalmentPlan As Boolean = False
    Dim vbMsg As DialogResult
    Private validateProcess As Integer
    Private m_bBProcessWithAmend As Boolean = False

    Private m_RiskCopyType As Integer
    Public Property RiskCopyType() As Integer
        Get
            Return m_RiskCopyType
        End Get
        Set(ByVal Value As Integer)
            m_RiskCopyType = Value
        End Set
    End Property

    Public Property RoundOffAmount() As Decimal
        Get
            Return m_cRoundOffAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cRoundOffAmount = Value
        End Set
    End Property
    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

    Public Property SelectedPolicyStatus() As String
        Get
            Return m_sSelectedPolicyStatus
        End Get
        Set(ByVal Value As String)
            m_sSelectedPolicyStatus = Value
        End Set
    End Property
    Public Property BackDatedMTAsAllowed() As Boolean
        Get
            Return m_bBackDatedMTAsAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_bBackDatedMTAsAllowed = Value
        End Set
    End Property

    Public Property ApplyMTATaxRatesonRen() As String
        Get
            Return m_sApplyMTATaxRatesonRen
        End Get
        Set(ByVal Value As String)
            m_sApplyMTATaxRatesonRen = Value
        End Set
    End Property
    Public Property IsTrueMonthlypolicyandNextInstalmentRenewal() As Boolean
        Get
            Return m_bIsTrueMonthlypolicyandNextInstalmentRenewal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = Value
        End Set
    End Property

    Public WriteOnly Property LeadAgentCnt() As Integer
        Set(ByVal Value As Integer)
            m_lLeadAgentCnt = Value
        End Set
    End Property

    Public WriteOnly Property RoadmapDisablesInstalments() As Boolean
        Set(ByVal Value As Boolean)
            m_bRoadmapDisablesInstalments = Value
        End Set
    End Property

    Public ReadOnly Property OldPolicyNumber() As String
        Get
            Return m_sOldPolicyNumber
        End Get
    End Property

    Public ReadOnly Property NewPolicyNumber() As String
        Get
            Return m_sNewPolicyNumber
        End Get
    End Property

    Public WriteOnly Property Mode() As Integer
        Set(ByVal Value As Integer)
            m_iMode = Value
        End Set
    End Property

    Public ReadOnly Property QuoteStatus() As String
        Get
            Return m_sQuoteStatus
        End Get
    End Property

    Public ReadOnly Property PFPremFinanceCnt() As Integer
        Get
            Return m_lPFPremFinanceCnt
        End Get
    End Property

    Public ReadOnly Property PFPremFinanceVersion() As Integer
        Get
            Return m_lPFPremFinanceVersion
        End Get
    End Property

    Public ReadOnly Property PaymentTerms() As String
        Get
            Return m_sPaymentTerms
        End Get
    End Property

    Public WriteOnly Property InstalmentProductCode() As String
        Set(ByVal Value As String)
            m_sInstalmentProductCode = Value
        End Set
    End Property
    'WPR 33-75 added
    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property
    '********************************
    ' General Interface Properties
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        'WPR 33-75 added
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property
    '********************************

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property SetSpecifiedTab() As Integer
        Set(ByVal Value As Integer)
            m_iSpecifiedTab = Value
        End Set
    End Property
    'Public Property Get SetSpecifiedTab() As Long
    '    SetSpecifiedTab = m_iSpecifiedTab
    'End Property

    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public ReadOnly Property ProcessSettleTransactions() As Boolean
        Get
            Return m_bProcessSettleTransactions
        End Get
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    ' RAM20050825 - Bug fix for PN 23018
    Public Property OriginalInsuranceFileCnt() As Integer
        Get
            Return m_lOriginalInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lOriginalInsuranceFileCnt = Value
        End Set
    End Property
    'PN 43045 -SUR
    Public Property NewBusinessNoTrans() As Integer
        Get
            Return m_iNewBusinessNoTrans
        End Get
        Set(ByVal Value As Integer)
            m_iNewBusinessNoTrans = Value
        End Set
    End Property

    Public Property MTAType() As Integer
        Get
            Return m_iMTAType
        End Get
        Set(ByVal Value As Integer)
            m_iMTAType = Value
        End Set
    End Property

    'Float Balance and Pre-Payment (RC)
    Public ReadOnly Property PaymentAccountID() As Integer
        Get
            Return m_lPaymentAccountID
        End Get
    End Property
    Public ReadOnly Property DebitAgainst() As Integer
        Get
            Return m_iDebitAgainst
        End Get
    End Property
    Public ReadOnly Property CreditTransactions() As Object
        Get
            Return m_vCreditTransactions
        End Get
    End Property
    Public ReadOnly Property DebitTransactions() As Object
        Get
            Return m_vDebitTransactions
        End Get
    End Property
    Public ReadOnly Property CashListID() As Integer
        Get
            Return m_lCashListID
        End Get
    End Property
    Public ReadOnly Property CashListItemID() As Integer
        Get
            Return m_lCashListItemID
        End Get
    End Property
    Public ReadOnly Property TransactionID() As Integer
        Get
            Return m_lTransactionID
        End Get
    End Property
    Public ReadOnly Property TransactionAmount() As Decimal
        Get
            Return m_cTransactionAmount
        End Get
    End Property

    Public ReadOnly Property IsBackDatedMTA() As Boolean
        Get
            Return m_bIsBackdatedMTARequired
        End Get
    End Property

    'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
    Public WriteOnly Property PolicyRenewalStatus() As Integer
        Set(ByVal Value As Integer)
            m_iPolicyRenewalStatus = Value
        End Set
    End Property

    Public ReadOnly Property PolicyMakeLiveStatus() As Integer
        Get
            Return m_iPolicyMakeLiveStatus
        End Get
    End Property ''m_lRenewalMode
    'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)

    Public Property RenewalProcessMode() As Integer
        Get
            Return m_lRenewalProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lRenewalProcessMode = Value
        End Set
    End Property
    'WPR 33-75 added
    Public ReadOnly Property ScreenID() As Integer
        Get
            Return m_lScreenId
        End Get
    End Property
    'WPR 33-75 added
    Public ReadOnly Property RiskTypeID() As Long
        Get
            Return m_lRiskTypeId
        End Get
    End Property
    ''' <summary>
    ''' cboPaymentTerms_Click
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cboPaymentTerms_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentTerms.SelectedIndexChanged
        SelectPaymentTerms(kPaymentTermsInvoice)
        ToggleMarkForCollection(kMakeLiveOptionsINVOICE)
    End Sub

    Public Property IsReadyToAccept() As Boolean
        Get
            Return m_bIsReadyToAccept
        End Get
        Set(value As Boolean)
            m_bIsReadyToAccept = value
        End Set
    End Property

    Public Property IsAllRiskQuoted() As Boolean
        Get
            Return m_bIsAllRiskQuoted
        End Get
        Set(value As Boolean)
            m_bIsAllRiskQuoted = value
        End Set
    End Property

    ''' <summary>
    ''' BackdatedEditing
    ''' </summary>
    ''' <value></value>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Property BackdatedEditing() As Boolean
        Get
            Return m_bBackdatedEditing
        End Get
        Set(ByVal value As Boolean)
            m_bBackdatedEditing = value
        End Set
    End Property

    Public Property ProcessWithAmend() As Boolean
        Get
            Return m_bBProcessWithAmend
        End Get
        Set(ByVal value As Boolean)
            m_bBProcessWithAmend = value
        End Set
    End Property

    Private Sub cmdAddRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRisk.Click
        ProcessAddRisk()
        EnableDisbleQuoteAllButton()
        'MyBase.BringToFront()

    End Sub

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click
        Try

            m_lReturn = CreateWorkManagerTask()

            'PN - 69368 code start - added for PMCancel by chandra shekhar on 18th March 2010
            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to create work task manager", "Task Window", MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub

            End If

            cmdSaveQuote_Click(cmdSaveQuote, New EventArgs())

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Task Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddTask_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0


        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn As Integer
        Dim dtDueDate As Date
        Dim v_lAction As Integer
        Dim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_lAction = 1

            ' Create the Component
            Dim temp_oTaskInstance As Object
            lReturn = g_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskInstance = temp_oTaskInstance
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Process Modes

            lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'we need to pass keys into the iPMWrkTaskInstance
            'so far just the client party cnt - add others to the local key array
            'I did think of passing iPMUListRisks entire key array but some
            'set values that the interface component already has different values set

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            lReturn = oTaskInstance.SetKeys(vKeyArray)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateWorkManagerTask = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Pass the MultipleTaskInstanceDisplayForm relevant values

            oTaskInstance.Customer = m_sShortName & "  " & txtPolicyRef.Text 'v_sPartyName

            oTaskInstance.DueDate = dtDueDate

            oTaskInstance.Description = txtPolicyRef.Text

            oTaskInstance.DisableCustomer = gPMConstants.PMEReturnCode.PMTrue

            oTaskInstance.CallingAppName = "iPMUListRisks"
            ' Set Task Group Id and Task Id (Memo Task)

            oTaskInstance.PMWrkTaskGroupId = 8
            'WPR 33-75 added
            oTaskInstance.SourceId = m_iSourceID

            If m_sTransactionType.Trim() = "NB" Then

                oTaskInstance.PMWrkTaskId = 58
            ElseIf m_sTransactionType.Trim() = "MTA" Then

                oTaskInstance.PMWrkTaskId = 59
            ElseIf m_sTransactionType.Trim() = "MTR" Then

                oTaskInstance.PMWrkTaskId = 208
            ElseIf m_sTransactionType.Trim() = "MTC" Then

                oTaskInstance.PMWrkTaskId = 75
            End If



            oTaskInstance.TaskInstKeyArray = m_vKeyArray

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:-      iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' If the User Cancelled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                'r_vPMWrkTaskInstanceCntArray = ""

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If

            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask", vApp:=ACApp, vClass:=ACClass, vMethod:=" CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result


            Return result
        End Try
    End Function
    'Start Written Status

    Private Function CreateWrittenWorkTask() As Integer
        Const kMethodName As String = "CreateWrittenWorkTask"
        Const kUserGroupIDSYSADMI As Long = 1
        Const kTaskGroupIDUNDER As Long = 8
        Dim oTaskInstance As Object
        Dim dtDueDate As Date
        Dim iTaskInstanceCnt As Integer
        Dim vNBTaskID As Object
        Dim iNBTaskId As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Create the Component
            '    ' Create an instance of the bPMWrkTaskInstance Component
            CreateWrittenWorkTask = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=oTaskInstance,
                sClassName:="bPMWrkTaskInstance.TaskControl",
                vInstanceManager:=PMGetLocalBusiness)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get instance of bPMWrkTaskInstance.TaskControl", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Start - Prakash Varghese - PN71294
            If m_dtCoverStartDate < Now Then
                dtDueDate = DateAdd("d", m_iWrittenTaskManagerDays, Now)
            Else
                dtDueDate = DateAdd("d", m_iWrittenTaskManagerDays, m_dtCoverStartDate)
            End If
            'End - Prakash Varghese - PN71294

            If m_iWrittenRemTaskGroup = 0 Then
                m_iWrittenRemTaskGroup = kTaskGroupIDUNDER
            End If

            If m_iWrittenRemUserGroup = 0 Then
                m_iWrittenRemUserGroup = kUserGroupIDSYSADMI
            End If

            m_lReturn = m_oBusiness.GetPMWrkTaskID(v_sTaskCode:="UNDERNB",
                                            r_vTaskId:=vNBTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPMWrkTaskID Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If IsArray(vNBTaskID) Then
                iNBTaskId = ToSafeInteger(vNBTaskID(0, 0))
            End If
            'Start  Written Status
            'Task has been created intead of Quotenumber new policy number is passed(m_sNewPolicyNumber)
            'While Creating Written Task.
            m_lReturn = oTaskInstance.CreateNew(
                                         v_lPMWrkTaskGroupID:=ToSafeInteger(m_iWrittenRemTaskGroup),
                                         v_lPMWrkTaskID:=iNBTaskId,
                                         v_sCustomer:=m_sShortName,
                                         v_dtTaskDueDate:=dtDueDate,
                                         v_lPMUserGroupID:=ToSafeInteger(m_iWrittenRemUserGroup),
                                         v_sDescription:="New Business Written Policy - " & m_sNewPolicyNumber,
                                         v_iTaskStatus:=0,
                                         v_iIsUrgent:=1,
                                         r_lPMWrkTaskInstanceCnt:=iTaskInstanceCnt,
                                         v_iIsVisible:=gPMConstants.PMEReturnCode.PMTrue)
            'End  -written Status
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                RaiseError(kMethodName, "Failed To create the Written task", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch excep As System.Exception


            ' DO Not Call any functions before here or the error will be lost
            'LogError( _
            '     v_sClass:=ACClass, _
            '     v_sMethod:=kMethodName, _
            '     r_lFunctionReturn:=CreateWrittenWorkTask)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask", vApp:=ACApp, vClass:=ACClass, vMethod:=" CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
            ' If you want to rollback a transaction or something, do it here

        Finally

            oTaskInstance.Dispose()
            oTaskInstance = Nothing


        End Try
    End Function

    Private Sub cmdApplyDiscount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApplyDiscount.Click
        ProcessApplyDiscount()
        MyBase.Activate()
    End Sub

    Private Sub cmdBackdateMTA_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBackdateMTA.Click
        cmdBackdateMTA.Enabled = False
        ProcessBackDateMTA(txtPolicyRef.Text.Trim()) ''PN-71068
        cmdBackdateMTA.Enabled = True
        MyBase.Activate()
    End Sub

    Private Sub cmdCopyRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopyRisk.Click
        ProcessCopyRisk()
        MyBase.Activate()
    End Sub

    Private Sub cmdDeleteRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteRisk.Click
        ProcessDeleteRisk()
        MyBase.Activate()
    End Sub

    'This button Doc Archive is newly added for implementing the document archive functionality
    Private Sub cmdDocArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDocArchive.Click
        Dim sOption As String = ""
        Dim sSPUrl As String = ""
        Dim sDocLIB As String = ""
        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionDocumentArchive, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return
        End If

        If sOption = "2" Then
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionSharepointserverName, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If


            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If
        End If
        If sOption = "1" Then

            m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=txtPolicyRef.Text.Trim() & "2")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        ElseIf sOption = "2" Then
            If sSPUrl.EndsWith("\") Then
                System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & m_sShortName.Trim() & "\Policy\" & txtPolicyRef.Text.Trim())
            Else
                System.Diagnostics.Process.Start(sSPUrl & "\" & sDocLIB & "\" & m_sShortName.Trim() & "\Policy\" & txtPolicyRef.Text.Trim())
            End If

        End If

    End Sub
    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.4.1)

    Private Sub cmdEditRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRisk.Click
        Try
            ProcessEditRisk()
            EnableDisbleQuoteAllButton()
            MyBase.Activate()
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdEditRisk_Click", r_lFunctionReturn:=0, excep:=ex)
        End Try
    End Sub
    ''' <summary>
    ''' cmdMakeLive_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdMakeLive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMakeLive.Click
        If OptInstalments.Checked AndAlso OptInstalments.Enabled AndAlso Convert.ToDecimal(uctInstalments1.TotalPayableAmount) < 0 Then
            MessageBox.Show("Unable to Proceed - This transaction will cause a negative instalment plan amount.", "Instalment Amount", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        If Trim(m_sTransactionType.ToUpper()) = "REN" AndAlso
            (Trim(m_sPaymentTerms.ToUpper()) = "INSTALMENTS" OrElse Trim(m_sPaymentTerms.ToUpper()) = "PREMIUMFINANCE" OrElse
             Trim(m_sPaymentTerms.ToUpper()) = "DIRECT DEBIT" OrElse Trim(m_sPaymentTerms.ToUpper()) = "CREDIT CARD") AndAlso
            OptInstalments.Checked = False Then
            m_bSoftDeleteInstalmentPlan = True
            Dim nChangePaymentOnRen As DialogResult
            nChangePaymentOnRen = MessageBox.Show(String.Format("You have changed the payment plan from instalments; the old instalment plan will be deleted at renewal acceptance.{0}Do you wish to continue?", Environment.NewLine),
                                                  "Payment Method Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If nChangePaymentOnRen = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        Dim bValidation As Boolean

        m_bSkipBackDatedCloseSave = True

        If m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI" Then
            ProcessAccept()
        Else
            If optMarkForCollection.Checked Then
                Exit Sub
            End If
            ValidateMakeLiveOptions(bValidation)
            If bValidation = False Then Exit Sub

            If OptInstalments.Checked AndAlso uctInstalments1.ValidateData() <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_sTransactionType.Trim().ToUpper() = "MTC" Then
                'If return premiun is greater than billed premium, do not allow cancellation
                Dim dTotalPremiumAmountForAllVerions As Decimal = 0
                Dim dTotalTaxNotAppliedToClient As Decimal = 0
                m_lReturn = m_oBusiness.GetTotalPremiumAmountForALLPolicyVersions(sInsuranceRef:=txtPolicyRef.Text.Trim(), nInsuranceCnt:=InsuranceFileCnt, dTotalPremium:=dTotalPremiumAmountForAllVerions, dTotalTaxNotAppliedToClient:=dTotalTaxNotAppliedToClient)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
                If (Math.Round(dTotalPremiumAmountForAllVerions, 2) + Math.Round(dTotalTaxNotAppliedToClient, 1) < (Math.Round(Convert.ToDecimal(txtGrossTotal.Text), 2) * (-1))) Then
                    MessageBox.Show("Return Premium is greater than Billed Premium. Your transaction could not be completed",
                                    "Cancel Policy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If
            ProcessMakeLive()
            MarkPlanAsSavedOrUpdated()
            Dim vresultarray As Object
            Dim istruemonthly As Integer
            Dim isautorenewbdm As Integer
            m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="is_true_monthly_policy", r_vProductArray:=vresultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("cmdmakelive", "failed to retreive product risk maintainence option for taskgroup", gPMConstants.PMELogLevel.PMLogError)
            Else
                istruemonthly = gPMFunctions.ToSafeLong(vresultarray(0, 0), 0)
            End If

            m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="auto_renew_bdmpolicy", r_vProductArray:=vresultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("cmdmakelive", "failed to retreive product risk maintainence option for taskgroup", gPMConstants.PMELogLevel.PMLogError)
            Else
                isautorenewbdm = gPMFunctions.ToSafeLong(vresultarray(0, 0), 0)
            End If

            If istruemonthly = 1 AndAlso isautorenewbdm = 1 AndAlso (m_sTransactionType.Trim().ToUpper() = "NB" Or m_sTransactionType.Trim().ToUpper() = "MTR") AndAlso validateProcess = kValidationSuccessful Then
                UNLOCKPOLICY()
                Dim frmIrenewalcatch As iPMURenewalCatchUp.frmInterface
                frmIrenewalcatch = New iPMURenewalCatchUp.frmInterface()
                frmIrenewalcatch.PolicyRef = m_sNewPolicyNumber
                frmIrenewalcatch.CoverStart = m_dtCoverStartDate
                frmIrenewalcatch.CoverEnd = Convert.ToDateTime(txtExpiryDate.Text)
                If m_dtCoverStartDate < DateTime.Now.AddMonths(-1) AndAlso Not OptInstalments.Checked Then
                    frmIrenewalcatch.ShowDialog()
                End If
            End If
        End If
    End Sub

    Private Sub cmdPrintDocument_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrintDocument.Click
        ProcessPrintDocument()
    End Sub

    Private Sub cmdPrintProposal_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrintProposal.Click
        ProcessPrintProposal()
    End Sub

    Private Sub cmdPrintQuote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrintQuote.Click
        ProcessPrintQuote()
    End Sub


    Private Sub cmdRequote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRequote.Click
        ProcessRequote()
    End Sub

    Private Sub cmdSaveQuote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSaveQuote.Click
        If Trim(m_sTransactionType.ToUpper()) = "REN" AndAlso
            (Trim(m_sPaymentTerms.ToUpper()) = "INSTALMENTS" OrElse Trim(m_sPaymentTerms.ToUpper()) = "PREMIUMFINANCE" OrElse
             Trim(m_sPaymentTerms.ToUpper()) = "DIRECT DEBIT" OrElse Trim(m_sPaymentTerms.ToUpper()) = "CREDIT CARD") AndAlso
            OptInstalments.Checked = False Then
            m_bSoftDeleteInstalmentPlan = True
            Dim nChangePaymentOnRen As DialogResult
            nChangePaymentOnRen = MessageBox.Show(String.Format("You have changed the payment plan from instalments; the old instalment plan will be deleted at renewal acceptance.{0}Do you wish to continue?", Environment.NewLine),
                                                  "Payment Method Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If nChangePaymentOnRen = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If

        Dim iResponse As DialogResult

        Dim oEvent As bSIREvent.Business

        Dim r_bValidation As Boolean
        Dim nValidationStatus As Integer = 0

        'WPR 33-75 added
        If m_bIsBackdatedMTARequired Then
            iResponse = MsgBox("Do you want to Save Backdated Quotes?" & vbCrLf & vbCrLf, vbExclamation + vbYesNoCancel + vbDefaultButton2, "Backdated Quote")
            If iResponse = vbCancel Then
                m_bCancelAboutToChangeAction = True
                m_bSkipBackDatedCloseSave = False
                Exit Sub
            ElseIf iResponse = vbNo Then
                m_bSkipBackDatedCloseSave = True
                m_bSaveBackdatedQuotes = False
            ElseIf iResponse = vbYes Then
                m_bSkipBackDatedCloseSave = True
                m_bSaveBackdatedQuotes = True
            End If
        End If

        'WPR12- Enhancement Quote Collection Process
        If optMarkForCollection.Checked Then

            m_lReturn = ValidationMarkForCollection(r_bValidation:=r_bValidation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdSaveQuote_Click", "Validation Mark For Collection Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            If Not r_bValidation Then
                'Validation Failed
                Exit Sub
            End If

            iResponse = MessageBox.Show("Quote is being passed to collection process. Do you wish to proceed?", "Mark For Collection", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If iResponse = System.Windows.Forms.DialogResult.Yes Then

                Dim temp_oEvent As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oEvent, "bSIREvent.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oEvent = temp_oEvent


                m_lReturn = oEvent.DirectAdd(vInsuranceFileCnt:=m_lInsuranceFileCnt, vPartyCnt:=m_lPartyCnt, vEventTypeCode:="POLCHANGE", vUserId:=g_iUserID, vDescription:="Quote Marked for Collection")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oEvent.DirectAdd", "Event Add Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            If iResponse = System.Windows.Forms.DialogResult.No Then
                'WPR 33-75 added
                m_bCancelAboutToChangeAction = True

                Exit Sub
            End If
        End If

        'This check is not requried for SaveQuote as the policy is not being made live from here,
        'sp zero peremium should not be a case to validate as premium can be entered later.
        'If OptInstalments.Checked AndAlso uctInstalments1.ValidateData() <> gPMConstants.PMEReturnCode.PMTrue Then
        '    Exit Sub
        'End If
        If Trim(m_sTransactionType.ToUpper()) = "REN" AndAlso m_bBProcessWithAmend Then
            m_lReturn = ProcessValidation(nValidationStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdSaveQuote_Click", "ProcessValidation Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            If nValidationStatus <> kValidationSuccessful Then
                Exit Sub
            End If
        End If

        'Unlock Current Policy 'PN35753 --RC
        If (m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA OrElse m_sTransactionType = "NB" OrElse m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR") And m_bBackdatedEditing = False Then
            UNLOCKPOLICY()
            UnLockSingleInstalmentPlan()
        End If

        ProcessSaveQuote()
        MarkPlanAsSavedOrUpdated()
    End Sub

    Private Sub MarkPlanAsSavedOrUpdated()
        'If policy is in renewal check for payment method
        If Trim(m_sTransactionType.ToUpper()) = "REN" Then
            Dim obSIRPremiumFinance As bSIRPremiumFinance.Business = Nothing
            m_lReturn = g_oObjectManager.GetInstance(obSIRPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SaveQuote", "Failed to get instance of bSIRPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Soft delete instalment plan if changed from instalment
            If OptInstalments.Checked = False AndAlso m_bSoftDeleteInstalmentPlan Then
                m_lReturn = obSIRPremiumFinance.MarkPlanAsDeleted(m_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SaveQuote", "Failed to mark plan as delete - SIRPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf OptInstalments.Checked = True Then
                m_lReturn = obSIRPremiumFinance.MarkPlanAsSaved(m_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SaveQuote", "Failed to mark plan as saved - SIRPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            obSIRPremiumFinance.Dispose()
            obSIRPremiumFinance = Nothing
        End If
    End Sub

    Private Sub Commission1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles Commission1.Change
        Recalculate(kRecalculateModeAgentCommission)
    End Sub


    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Const kMethodName As String = "Form_Initialize"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create an instance of the object manager.
            'g_oObjectManager = New bObjectManager.ObjectManager()
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            'lReturn = g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
            lReturn = MainModule.g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Instance of bObjectManager.ObjectManager Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            'With g_oObjectManager

            With MainModule.g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserID = .UserID
            End With

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRListRisks.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oRiskData As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oRiskData, "bSIRRiskData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskData = temp_m_oRiskData
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSirRiskData.business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oRenSelection As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oRenSelection, "bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRenSelection = temp_m_oRenSelection
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSirRenSelection.business", gPMConstants.PMELogLevel.PMLogError)

            End If


            Dim temp_m_oChangePolicyStatus As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oChangePolicyStatus, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oChangePolicyStatus = temp_m_oChangePolicyStatus

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            Dim temp_m_oRITax As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oRITax, "bSIRRITax.Business", gPMConstants.PMGetViaClientManager)
            m_oRITax = temp_m_oRITax

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRRITax.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_m_oPartyFee As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oPartyFee, "bSIRPartyFee.UBusiness", gPMConstants.PMGetViaClientManager)
            m_oPartyFee = temp_m_oPartyFee

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRPartyFee.UBusiness", gPMConstants.PMELogLevel.PMLogError)
            End If

            ''Backdated MTA
            Dim temp_m_oAutoMTA As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oAutoMTA, "bSIRAutoMTA.Business", gPMConstants.PMGetViaClientManager)
            m_oAutoMTA = temp_m_oAutoMTA

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRAutoMTA.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
            Dim temp_m_oProduct As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oProduct = temp_m_oProduct

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_m_oRenewal As Object
            lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oRenewal, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRenewal = temp_m_oRenewal

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRRenewal.Business", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : Date : Process Id
    ' ***************************************************************** '
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'WPR 33-75 added
        'Don't let the user leave without saving the Backdated MTA status except requote
        If m_bIsBackdatedMTARequired And Not m_bSkipBackDatedCloseSave And m_lStatus <> gPMConstants.PMEReturnCode.PMNavAction1 Then
            cmdSaveQuote_Click(cmdSaveQuote, New EventArgs())
            If m_bCancelAboutToChangeAction Then
                m_bCancelAboutToChangeAction = False
                Cancel = True
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
        End If

        'Unlock Current Policy 'PN35753 --RC

        If (m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "NB" Or m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR") And UnloadMode <> vbFormCode Then
            UNLOCKPOLICY()
            UnLockSingleInstalmentPlan()
        End If

        'Unlock Current Policy if we are creating policy through NB (No Transaction) ' PN 43045 --SUR
        If (m_sTransactionType = "NB" Or m_sTransactionType = "MTA") And m_iNewBusinessNoTrans = 1 Then
            UNLOCKPOLICY()
        End If

        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.


        If UnloadMode <> vbFormCode Then
            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'if we cancel while in cancellation mode then put the risk status back to its original
            If m_sTransactionType = "MTC" Then

                If m_oBusiness.SetRiskStatusArray(m_vRiskStatus) <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to restore risk status to its original", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If

        End If

        ' Terminate the control
        uctPMUListRisk1.Dispose()
        ' Reset the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        eventArgs.Cancel = Cancel <> 0
    End Sub
    Private Sub Form_Terminate_Renamed()

        'Terminate will have done this, but just in case...
        If Not (m_oBusiness Is Nothing) Then
            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

        End If

        'Terminate will have done this, but just in case...
        If Not (m_oRiskData Is Nothing) Then
            ' Terminate the business object

            m_oRiskData.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oRiskData = Nothing

        End If

        'Terminate will have done this, but just in case...
        If Not (m_oRenSelection Is Nothing) Then
            ' Terminate the business object

            m_oRenSelection.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oRenSelection = Nothing

        End If

    End Sub

    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const kMethodName As String = "Form_Unload"

        Dim lReturn, lSubValue As Integer

        Try
            ' Terminate the business object

            If Not (IsNothing(m_oBusiness)) Then
                m_oBusiness.Dispose()
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
        Finally
            ' destroy object reference
            m_oBusiness = Nothing
            'Exit Sub
            'Resume
        End Try
    End Sub

    ''' <summary>
    ''' frmInterface_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Dim nReturn As Integer
        Dim nRiskFolderCnt As Integer
        Dim nRiskCnt As Integer
        Dim vResultArray As Object
        Dim lQuoteAllRiskNB As Integer
        Dim lQuoteAllRiskMTC As Integer
        Dim lQuoteAllRiskMTA As Integer
        Dim bShowQuoteAllButton As Boolean = False
        Try
            nReturn = PMEReturnCode.PMTrue

            m_bFormLoading = True
            uctPMUListRisk1.FormLoading = m_bFormLoading
            ' set up interface
            nReturn = SetupForm()

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Start WPR27
            EnableDisbleQuoteAllButton()
            'End WPR27



            ' WPR53 : Need to add mandatory risk if not already added...
            If m_sTransactionType = "NB" Then
                nReturn = m_oBusiness.AddRisk(v_lRiskTypeId:=m_lRiskTypeId, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lProductID:=m_lProductId, r_lRiskFolderCnt:=nRiskFolderCnt, r_lRiskCnt:=nRiskCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If nRiskCnt <> 0 Then

                    nReturn = uctPMUListRisk1.GetRisks
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetRisks Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    ' get return properties
                    m_vOriginalSelStatus = uctPMUListRisk1.RiskSelectionStatus
                End If
            End If

            'Backdated MTA
            If m_sTransactionType <> "REN" AndAlso m_sTransactionType <> "NB" AndAlso m_sTransactionType <> "" AndAlso m_sTransactionType <> "DRI" AndAlso m_sTransactionType <> "PT" AndAlso m_bBackdatedEditing = False Then
                m_lReturn = m_oAutoMTA.SetProcessModes(vTransactionType:=m_sTransactionType)
                m_bIsBackdatedMTARequired = m_oAutoMTA.IsBackdatedMTARequired(m_lInsuranceFolderCnt, m_dtCoverStartDate, m_lInsuranceFileCnt)
            End If

            If m_bIsBackdatedMTARequired Then
                cmdMakeLive.Enabled = False
                cmdBackdateMTA.Visible = True

                m_bIsRENaffectedbyBackdateMTA = m_oAutoMTA.RenewalAffectedByBDMTA
            Else
                If Not cmdApplyDiscount.Enabled AndAlso m_bBackdatedEditing = False Then
                    If Not m_bIsMarketPlacePolicy Then
                        cmdMakeLive.Enabled = True
                    End If
                End If
                cmdBackdateMTA.Visible = False
            End If

            cmdBackdateMTA.Visible = uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired

            nReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRIRegeneration,
                                                    v_vBranch:=g_iSourceID, r_vUnderwriting:=m_sEnableRIRegeneration)


            If (m_sTransactionType = "PT" Or m_sTransactionType = "DRI") Then
                cmdEditRisk.Text = "&Reinsurance"
                cmdSaveQuote.Visible = True
                cmdSaveQuote.Enabled = True
                cmdSaveQuote.Text = "&Save Quote"
                cmdMakeLive.Enabled = True
                cmdDocArchive.Visible = False
                cmdMakeLive.Visible = True
            End If

            nReturn = CType(GetAgentDetails(), gPMConstants.PMEReturnCode)

            nReturn = CType(EnableDisableMarkForCollection(), gPMConstants.PMEReturnCode)

        Catch ex As Exception
            nReturn = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn, excep:=ex)
        Finally
            m_bFormLoading = False
            uctPMUListRisk1.FormLoading = m_bFormLoading
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Sub

    Private Sub PopulateCollectionFrequency()
        Dim oResult(,) As Object = Nothing
        Dim nUpperBound As Integer
        Dim oResultReturned(,) As Object
        m_lReturn = m_oBusiness.GetLookupsByEffectiveDate("CollectionFrequency", oResult)

        If oResult IsNot Nothing Then
            oResultReturned = DirectCast(oResult, Object(,))
            nUpperBound = oResultReturned.GetUpperBound(1)
            For iCnt As Integer = 0 To nUpperBound
                cboCollectionFrequency.Items.Add(oResultReturned(1, iCnt).ToString)
                VB6.SetItemData(cboCollectionFrequency, iCnt, oResultReturned(0, iCnt).ToString)
            Next
        End If

    End Sub
    Private Sub PopulatePaymentTerm()

        Dim oResult(,) As Object = Nothing
        Dim nUpperBound As Integer
        Dim oResultReturned(,) As Object
        m_lReturn = m_oBusiness.GetLookupsByEffectiveDate("DOPaymentTerms", oResult)

        If oResult IsNot Nothing Then
            oResultReturned = DirectCast(oResult, Object(,))
            nUpperBound = oResultReturned.GetUpperBound(1)
            For iCnt As Integer = 0 To nUpperBound
                cboPaymentTerms.Items.Add(oResultReturned(1, iCnt).ToString)
                VB6.SetItemData(cboPaymentTerms, iCnt, oResultReturned(0, iCnt).ToString)
                VB6.SetItemString(cboPaymentTerms, iCnt, oResultReturned(2, iCnt).ToString)
            Next
        End If

    End Sub
    ' ***************************************************************** '
    ' Name: SetupForm
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupForm() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "SetupForm"

        Dim oResultArray(,) As Object

        Try
            nResult = PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            nResult = DisplayCaptions()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " DisplayCaptions Failed", PMELogLevel.PMLogError)
            End If

            nResult = GetInsuranceFileDetails()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInsuranceFileDetails Failed", PMELogLevel.PMLogError)
            End If

            nResult = GetAgentDetails()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetAgentDetails Failed", PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="is_roundoff_to_zero", r_vProductArray:=oResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for Roundoff", PMELogLevel.PMLogError)
            Else
                If IsArray(oResultArray) Then

                    m_bRoundOff = IIf(CDbl(oResultArray(0, 0)) = 1, 1, 0)
                End If
                If Not m_bRoundOff Then
                    lblRoundOffAmount.Enabled = False
                    lblGrossRoundedTotal.Enabled = False
                End If
            End If
            If m_bIsSingleInstalmentPlan Then
                nResult = m_oBusiness.GetNoOfPoliciesOnAgent(v_lLeadAgentCnt:=m_vLeadAgentCnt,
                                                                        r_lNoOfPolicies:=m_lNoOfPoliciesOnSingleInsAgent)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "CheckInstallmentSchemesforMTA Failed", PMELogLevel.PMLogError)
                End If
            End If

            nResult = SetUpListRisks()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetUpListRisks Failed", PMELogLevel.PMLogError)
            End If
            nResult = SetupFees()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupFees Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupRITaxes()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupRITaxes Failed", PMELogLevel.PMLogError)
            End If

            nResult = PopulateTotals()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "PopulateTotals Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupCommission()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupCommission", PMELogLevel.PMLogError)
            End If

            nResult = SetupControl()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupTabs Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupInstalments()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupInstalments Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupButtons()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupButtons Failed", PMELogLevel.PMLogError)
            End If

            nResult = GetProductOptions()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetProductOptions Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupInViewOnlyMode()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupInViewOnlyMode Failed", PMELogLevel.PMLogError)
            End If

            nResult = SetupPolicyDiscount()
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetupPolicyDiscount Failed", PMELogLevel.PMLogError)
            End If


            'Float Balance and Pre-Payment (RC)
            If m_sTransactionType <> "" Then
                If m_bBackdatedEditing = True Then

                    cmdEditRisk.Enabled = False
                    cmdAddRisk.Enabled = False
                    cmdDeleteRisk.Enabled = False
                    cmdCopyRisk.Enabled = False

                    cmdPrintDocument.Enabled = False
                    cmdPrintProposal.Enabled = False
                    cmdPrintQuote.Enabled = False
                    cmdMakeLive.Enabled = False
                    cmdDocArchive.Enabled = False

                    OptInstalments.Enabled = False
                    OptInvoice.Enabled = False
                    optPayNow.Enabled = False
                    optBankGuarantee.Enabled = False
                    optCashDeposit.Enabled = False
                    optMarkForCollection.Enabled = False

                    cmdAddTask.Enabled = False
                ElseIf Not OptInvoice.Enabled AndAlso Not OptInstalments.Enabled AndAlso Not optPayNow.Enabled AndAlso Not optBankGuarantee.Enabled _
                  AndAlso m_sTransactionType <> "PT" AndAlso m_sTransactionType <> "DRI" Then
                    MessageBox.Show("No Payment Terms are available. Please contact your system administrator.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

            Else
                cmdAddTask.Enabled = False
            End If

            If m_sTransactionType = "NB" Then
                If m_bWrittenStatusPermitted Then
                    If Trim$(m_sInsuranceFileStatusCode) = "WRITTEN" Then
                        cmdWrite.Enabled = False
                    End If
                End If
            End If

            m_bCopyRisk = False
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function







    ' ***************************************************************** '
    ' Name: PrintQuote
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PrintQuote(ByVal v_lDocumentTemplateID As Object, ByVal v_lDocumentTypeID As Object) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "PrintQuote"
        Const ACPrintSilentMode As Integer = 3

        Dim objDocTemplate As iPMBDocTemplate.Interface_Renamed
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' create new instance of doc template
            Dim temp_objDocTemplate As Object
            lReturn = g_oObjectManager.GetInstance(temp_objDocTemplate, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            objDocTemplate = temp_objDocTemplate

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of iPMBDocTemplate.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the appropriate properties


            objDocTemplate.DocumentTemplateId = v_lDocumentTemplateID
            objDocTemplate.DocumentTypeId = v_lDocumentTypeID
            objDocTemplate.PartyCnt = m_lPartyCnt
            objDocTemplate.InsuranceFileCnt = m_lInsuranceFileCnt
            objDocTemplate.Mode = ACPrintSilentMode

            ' Call the Start Method of iPMBDocTemplate.Interface

            lReturn = objDocTemplate.Start

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to print the policy document", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
            result = PMEReturnCode.PMError
            ' If you want to rollback a transaction or something, do it here
        Finally
            ' destroy object
            objDocTemplate = Nothing

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessPrintDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessPrintDocument() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPrintDocument"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lDocumentTemplateID, lDocumentTypeID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document template to use
            lReturn = CType(GetDocTemplate(r_lDocumentTemplateId:=lDocumentTemplateID, r_lDocumentTypeId:=lDocumentTypeID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to get the document template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' PW051102 - Ensure a template has been selected
            If lDocumentTemplateID = 0 And lDocumentTypeID = 0 Then
                Return result
            End If

            ' Print using the selected template
            lReturn = CType(PrintQuote(v_lDocumentTemplateID:=lDocumentTemplateID, v_lDocumentTypeID:=lDocumentTypeID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to print the quote.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetDocTemplate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDocTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBFindDocTemplate As Object

        Const kMethodName As String = "GetDocTemplate"

        Dim lReturn As Integer

        Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find document template object
            Dim temp_oFindDocTemplate As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindDocTemplate, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindDocTemplate = temp_oFindDocTemplate

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of iPMBFindDocTemplate.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oFindDocTemplate.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindDocTemplate.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oFindDocTemplate.Mode = 1

            ' Start the component

            lReturn = oFindDocTemplate.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindDocTemplate.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            r_lDocumentTemplateId = oFindDocTemplate.DocumentTemplateId

            r_lDocumentTypeId = oFindDocTemplate.DocumentTypeId


            oFindDocTemplate.Dispose()



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oFindDocTemplate = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CopyRiskData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CopyRiskData"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bFound As Boolean
        Dim lNewGisPolicyLinkID As Integer
        Dim vGisPolicyLinkArray, vRiskArray(,) As Object
        Dim lCount As Integer
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lNewPolicyBinderId, lOldPolicyBinderId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all risks associate with the InsuranceFileCnt

            lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Getting Risk"
                gPMFunctions.RaiseError(kMethodName, "GetRisk Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check if we have any risks
            If Not Information.IsArray(vRiskArray) Then
                r_sFailureReason = "No risks found"
                gPMFunctions.RaiseError(kMethodName, "GetRisk Returned no data", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Find the risk that matches the passed risk count, i.e. the one we want
            ' to copy
            bFound = False

            For lCount = 0 To vRiskArray.GetUpperBound(1)

                If CDbl(vRiskArray(0, lCount)) = v_lRiskCnt Then
                    bFound = True
                    Exit For
                End If
            Next

            ' Check if we have found the risk to copy
            If Not bFound Then
                r_sFailureReason = "Cannot find risk to copy"
                gPMFunctions.RaiseError(kMethodName, "Risk to Copy Not Found", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Copy risk with same insurance file cnt

            lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=r_lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Copy Risk"
                gPMFunctions.RaiseError(kMethodName, "CopyRisk Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Prepare details to copy GIS Stuff attached to current risk

            ' Get policy link detail

            lReturn = m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=vRiskArray(ACRiskPosCnt, lCount), r_vResultArray:=vGisPolicyLinkArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "GetGISPolicyLink"
                gPMFunctions.RaiseError(kMethodName, "GetGISPolicyLink Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Do we have any data?
            Dim auxVar As Object = vGisPolicyLinkArray(0, 0)


            If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                ' Make sure GIS object present.


                lReturn = m_oRenSelection.GIS_LoadFromDB(CStr(vGisPolicyLinkArray(4, 0)).Trim(), v_lInsuranceFolderCnt, vGisPolicyLinkArray(0, 0), vRiskArray(0, lCount)) 'copy GIS details to NewInsuranceFileCnt

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "LoadFromDB"
                    gPMFunctions.RaiseError(kMethodName, "LoadFromDB Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                ' So we pass existing folder_cnt in for old and new file_cnt.



                lReturn = m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vOldRiskID:=vRiskArray(0, lCount), v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vNewRiskID:=r_lNewRiskCnt)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "CopyDataSet"
                    gPMFunctions.RaiseError(kMethodName, "CopyDataSet Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Initialise the Data Set with the Object/Properties

                m_lReturn = m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "LoadFromXML"
                    gPMFunctions.RaiseError(kMethodName, "LoadFromXML Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'RWH(28/02/2001)


                lReturn = m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "SaveToDB"
                    gPMFunctions.RaiseError(kMethodName, "SaveToDB Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                'START PN    51771
                'Get Policy Binder Ids


                m_lReturn = m_oRenSelection.GetPolicyBinderId(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), v_lGISPolicyLinkId:=lNewGisPolicyLinkID, r_lPolicyBinderId:=lNewPolicyBinderId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "GetPolicyBinderId"
                    gPMFunctions.RaiseError(kMethodName, "GetPolicyBinderId", gPMConstants.PMELogLevel.PMLogError)
                    Return result
                End If



                m_lReturn = m_oRenSelection.GetPolicyBinderId(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), v_lGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), r_lPolicyBinderId:=lOldPolicyBinderId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "GetPolicyBinderId"
                    gPMFunctions.RaiseError(kMethodName, "GetPolicyBinderId", gPMConstants.PMELogLevel.PMLogError)
                    Return result
                End If



                m_lReturn = m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderId:=lOldPolicyBinderId, v_lNewPolicyBinderId:=lNewPolicyBinderId, v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "CopyRiskStandardWordings"
                    gPMFunctions.RaiseError(kMethodName, "CopyRiskStandardWordings", gPMConstants.PMELogLevel.PMLogError)
                    Return result
                End If
                'START PN    51771
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCopyRisk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessCopyRisk() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCopyRisk"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNewRiskCnt As Integer
        Dim sFailureReason As String = ""
        Dim oCopyRisk As frmCopyRisk
        Dim lRiskNumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            lReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " - Save Cover Noted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            If uctPMUListRisk1.DiscountedRiskCount > 0 Then
                PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonCopyRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return result
                End If
            End If
            ' create new instance of copy form
            oCopyRisk = New frmCopyRisk()

            ' display copy form
            oCopyRisk.ShowDialog()

            ' if copy cancelled
            If Not oCopyRisk.OK Then
                oCopyRisk.Close()
                oCopyRisk = Nothing
                Return result
            Else
                ' determine what copy type to perform
                RiskCopyType = oCopyRisk.CopyType
                oCopyRisk.Close()
                oCopyRisk = Nothing
            End If

            ' copy the risk
            lReturn = CType(CopyRiskData(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskCnt:=m_lRiskId, r_lNewRiskCnt:=lNewRiskCnt, r_sFailureReason:=sFailureReason), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to copy the risk. CopyRiskData failed on " & sFailureReason, gPMConstants.PMELogLevel.PMLogError)
            End If

            If RiskCopyType = 0 Then

                'make sure this risk is not selected

                If m_oBusiness.SetRiskSelectedValue(lNewRiskCnt, 0) <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to reset selected flag for copied risk")
                End If

                'update the variation number on the new risk

                lReturn = m_oBusiness.UpdateRiskVarNo(v_lRiskNumber:=m_lRiskNo, v_lRiskCnt:=lNewRiskCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Unable to update the risk variation number.", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                ' Find out the next risk number for this policy

                lReturn = m_oBusiness.GetNextRiskNo(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetNextRiskNo Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Save the risk number to the risk record

                lReturn = m_oBusiness.UpdateRiskNo(v_lRiskCnt:=lNewRiskCnt, v_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateRiskNo Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' Assign new risk folder
                lReturn = m_oBusiness.UpdateRiskFolder(v_lRiskCnt:=lNewRiskCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateRiskFolder Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If



            lReturn = m_oBusiness.UpdateRiskSelection(v_lRiskCnt:=lNewRiskCnt, v_vIsRiskSelected:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateRiskSelection Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' act as if the edit button has been clicked on the new copy
            m_bCopyRisk = True
            m_lRiskId = lNewRiskCnt

            lReturn = ProcessEditRisk()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessEditRisk", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bIsBackdatedMTARequired Then
                ClearBackDatedMTAQuotes()
            End If
            'Start WPR27
            EnableDisbleQuoteAllButton()
            'End WPR27
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
        Return result
    End Function

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
    '(Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    'Added one optional parameter v_bIsSilentQuote
    Public Function GetRiskRating(ByRef iTask As Integer) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "GetRiskRating"

        Dim lReturn As Integer


        Dim oPerilAllocation As iPMUPerilAllocation.Interface_Renamed


        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of peril allocation
            Dim temp_oPerilAllocation As Object
            lReturn = g_oObjectManager.GetInstance(temp_oPerilAllocation, sClassName:="iPMUPerilAllocation.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPerilAllocation = temp_oPerilAllocation
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of iPMUPerilAllocation.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set process modes

            lReturn = oPerilAllocation.SetProcessModes(vTask:=iTask, vTransactionType:=m_sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUPerilAllocation.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set peril allocation properties

            oPerilAllocation.InsuranceFolderCnt = m_lInsuranceFolderCnt

            oPerilAllocation.InsuranceFileCnt = m_lInsuranceFileCnt

            oPerilAllocation.RiskId = m_lRiskId

            oPerilAllocation.ApplyMTATaxRatesonRen = m_sApplyMTATaxRatesonRen

            'WPR 33-75 added
            oPerilAllocation.IsBackDatedMTA = m_bIsBackdatedMTARequired
            ' start peril allocation

            lReturn = oPerilAllocation.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUPerilAllocation.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the return status

            m_lStatus = oPerilAllocation.Status

            ' terminate peril allocation

            oPerilAllocation.Dispose()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oPerilAllocation = Nothing


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetRiskReinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetRiskReinsurance() As Integer
        Dim nReturn As Integer
        Dim oReinsurance As iPMUReinsurance.Interface_Renamed
        Dim nTask As PMEComponentAction

        Const kMethodName As String = "GetRiskReinsurance"
        Try

            nReturn = PMEReturnCode.PMTrue

            ' get instance of reinsurance
            Dim temp_oReinsurance As Object
            nReturn = g_oObjectManager.GetInstance(temp_oReinsurance, sClassName:="iPMUReinsurance.Interface_Renamed", vInstanceManager:=PMGetLocalInterface)
            oReinsurance = temp_oReinsurance

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Get instance of iPMUReinsurance.Interface failed", PMELogLevel.PMLogError)
            End If

            If m_iTask <> PMEComponentAction.PMView OrElse m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI" Then
                nTask = PMEComponentAction.PMAdd
            Else
                nTask = PMEComponentAction.PMView
            End If

            ' set process modes
            If m_sTransactionType = "DRI" AndAlso (m_iInsuranceFileTypeID = kInsuranceFileTypeMTACANID OrElse m_nInsuranceFileStatusID = kInsuranceFileStatusCANID OrElse m_nInsuranceFileStatusID = kInsuranceFileStatusREPBDMTAID) Then
                'set transaction type to MTC so that unallocated amount will be moved to retained
                nReturn = oReinsurance.SetProcessModes(vTask:=nTask, vTransactionType:="MTC")
            Else
                nReturn = oReinsurance.SetProcessModes(vTask:=nTask, vTransactionType:=m_sTransactionType)
            End If

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Get instance of iPMUReinsurance.Interface.setprocessmodes failed", PMELogLevel.PMLogError)
            End If

            ' set properties

            oReinsurance.InsuranceFileCnt = m_lInsuranceFileCnt

            oReinsurance.RiskID = m_lRiskId

            If m_bIsRiskDeleted = True Then
                oReinsurance.IsRiskDeleted = True
            End If
            ' start interface

            nReturn = oReinsurance.Start()
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Get instance of iPMUReinsurance.Interface.start failed", PMELogLevel.PMLogError)
            End If

            ' get status

            m_lStatus = oReinsurance.Status

            If m_lStatus = PMEReturnCode.PMOK AndAlso (m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI") Then
                m_bIsRIAmended = True
                nReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=m_lRiskId, v_sRiskStatusCode:="QUOTED")
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateRiskStatus Failed", PMELogLevel.PMLogError)
                End If

            End If

        Catch excep As Exception
            nReturn = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nReturn, excep:=excep)

        Finally

            oReinsurance.Dispose()
            oReinsurance = Nothing

        End Try
        Return nReturn
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetRiskTax) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'WPR 33-75 added un commented as per WPR
    Public Function GetRiskTax() As Integer
        'Private Function GetRiskTax() As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "GetRiskTax"

        Dim lReturn As Integer

        Dim oRITax As iPMURITax.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of interface
            Dim temp_oRITax As Object
            lReturn = g_oObjectManager.GetInstance(temp_oRITax, sClassName:="iPMURITax.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRITax = temp_oRITax

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Instance of iPMURITax.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            'set process modes

            lReturn = oRITax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=m_sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURITax.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'set properties

            oRITax.InsuranceFileCnt = m_lInsuranceFileCnt

            oRITax.RiskCnt = m_lRiskId


            'start interface

            lReturn = oRITax.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURITax.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'get return status

            m_lStatus = oRITax.Status

            'terminate interface

            oRITax.Dispose()



        Catch ex As Exception

            'DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            'destroy interface
            oRITax = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '

    'WPR 33-75 added commented as per WPR
    'Private Function GetRiskType() As Integer
    'WPR 33-75 added
    Public Function GetRiskType() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskType"

        Dim lReturn As Integer

        Dim oFindRiskType As iPMUFindRiskType.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of interface
            Dim temp_oFindRiskType As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindRiskType, sClassName:="iPMUFindRiskType.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindRiskType = temp_oFindRiskType
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Instance of iPMUFindRiskType.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set process modes

            lReturn = oFindRiskType.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUFindRiskType.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set properties

            'Developer Guide No. 37
            oFindRiskType.ProductTypeId = m_lProductId

            ' start interface

            'Developer Guide No. 37
            oFindRiskType.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUFindRiskType.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return details

            m_lStatus = oFindRiskType.Status

            m_lRiskTypeId = oFindRiskType.RiskTypeId

            m_lScreenId = oFindRiskType.GISScreenId

            ' terminate interface

            oFindRiskType.Dispose()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy interface
            oFindRiskType = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' UpdatePolicyPremium
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdatePolicyPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim nResult As Integer
        Const kMethodName As String = "UpdatePolicyPremium"

        Dim oChangePolicyStatus As bSIRChangePolicyStatus.Business

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of object
            Dim temp_oChangePolicyStatus As Object
            nResult = g_oObjectManager.GetInstance(temp_oChangePolicyStatus, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oChangePolicyStatus = temp_oChangePolicyStatus
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update policy premium

            nResult = oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' terminate object instance

            oChangePolicyStatus.Dispose()
            oChangePolicyStatus = Nothing
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            oChangePolicyStatus = Nothing
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' ProcessEditRisk
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessEditRisk() As Integer
        Dim nResult As Integer
        Dim nReturn As Integer
        Dim oRisk As iPMURisk.Interface_Renamed
        Dim vCurRiskId As Object

        Dim sSysOptionBDMTA As String = ""
        Const kMethodName As String = "ProcessEditRisk"

        nResult = PMEReturnCode.PMTrue

        'get system option "Apply Back-Dated Risk Editing Restrictions"
        nReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=5079, r_sOptionValue:=sSysOptionBDMTA, v_iSourceID:=g_iSourceID), PMEReturnCode)

        If nReturn <> PMEReturnCode.PMTrue Then
            RaiseError("ProcessEditRisk", "Failed to get System Option [5056]", PMELogLevel.PMLogError)
            Return PMEReturnCode.PMFalse

        End If
        If (m_sTransactionType = "PT" OrElse m_sTransactionType = "DRI") Then
            ' get risk reinsurace
            nReturn = GetRiskReinsurance()
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetRiskReinsurance Failed", PMELogLevel.PMLogError)
                Return PMEReturnCode.PMFalse
            End If

        Else
            If sSysOptionBDMTA = "1" AndAlso m_iTask <> PMEComponentAction.PMView Then

                If m_bIsBackdatedMTARequired AndAlso Not (m_sTransactionType = "MTC" OrElse m_iInsuranceFileTypeID = kInsFileTypeMTATempQuote) Then
                    nReturn = CType(IsSubsequentRiskVersionsEdited(v_lRiskID:=m_lRiskId, v_dtMTAEffectiveDate:=m_dtCoverStartDate), PMEReturnCode)
                    If nReturn = PMEReturnCode.PMTrue Then
                        MessageBox.Show("The Risk cannot be edited as the Future versions of the Risk have been edited!!", "Edit Risk", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return nResult
                    End If
                End If
            End If

            nReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote()

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " - Save Cover Noted Failed", PMELogLevel.PMLogError)
            End If

            If m_iTask <> PMEComponentAction.PMView Then

                ' if pay by instalments selected
                ' then save the selected instalments plan
                'PM033055 Sumit K only process payments when Make live is clicked.
                If m_sTransactionType = "REN" AndAlso b_IsMakeLiveClicked Then
                    nReturn = ProcessPaymentTerms()
                    If nReturn = PMEReturnCode.PMFalse Then
                        RaiseError(kMethodName, "ProcessInstalments Failed", PMELogLevel.PMLogError)
                        Return nResult
                    End If
                End If
                If uctPMUListRisk1.DiscountedRiskCount > 0 Then
                    PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonEditRisk)
                    If m_bCancelAboutToChangeAction Then
                        m_bCancelAboutToChangeAction = False
                        Return nResult
                    End If
                End If
            End If

            ' create instance of interface
            Dim temp_oRisk As Object
            nReturn = g_oObjectManager.GetInstance(temp_oRisk, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=PMGetLocalInterface)
            oRisk = temp_oRisk
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get instance of iPMURisk.Interface", PMELogLevel.PMLogError)
            End If

            If b_IsMakeLiveClicked Then

                'If b_IsMakeLiveClicked=True then get the current risk id for the original risk id
                nReturn = m_oBusiness.GetCurRiskIdtForOriginalRiskId(v_lOriginalRiskId:=m_lRiskId, r_vCurRiskId:=vCurRiskId)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to get the current risk id.", PMELogLevel.PMLogError)
                End If

                If IsArray(vCurRiskId) Then
                    m_lRiskId = ToSafeLong(vCurRiskId(0, 0))
                End If

            End If

            If b_IsMakeLiveClicked Then

                'If b_IsMakeLiveClicked=True then get the current risk id for the original risk id
                nReturn = m_oBusiness.GetCurRiskIdtForOriginalRiskId(v_lOriginalRiskId:=m_lRiskId, r_vCurRiskId:=vCurRiskId)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to get the current risk id.", PMELogLevel.PMLogError)
                End If

                If IsArray(vCurRiskId) Then
                    m_lRiskId = ToSafeLong(vCurRiskId(0, 0))
                End If

            End If

            ' set interface properties

            oRisk.PartyCnt = m_lPartyCnt

            oRisk.ShortName = m_sShortName

            oRisk.InsuranceFolderCnt = m_lInsuranceFolderCnt

            oRisk.InsuranceFileCnt = m_lInsuranceFileCnt

            oRisk.RiskId = m_lRiskId

            oRisk.ScreenId = m_lScreenId

            oRisk.ProductId = uctPMUListRisk1.ProductID

            oRisk.RiskTypeId = m_lRiskTypeId

            oRisk.CopyRisk = m_bCopyRisk

            ' set process modes

            nReturn = oRisk.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "IPMURisk.Interface.SetProcessModes Failed", PMELogLevel.PMLogError)
            End If

            ' start interface

            nReturn = oRisk.Start
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "IPMURisk.Interface.Start Failed", PMELogLevel.PMLogError)
            End If

            'Reset the flag
            m_bCopyRisk = False

            ' WPR53
            If m_lRiskId <> 0 And oRisk.RiskId <> 0 And m_bMandatoryRisk And m_lRiskId <> oRisk.RiskId Then
                ' If mandatory risk edited first time to acquire new riskId in the version
                nReturn = m_oBusiness.UpdateMandatoryRisk(v_lRiskId:=oRisk.RiskId)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateMandatoryRisk" & " Failed", PMELogLevel.PMLogError)
                End If
            End If

            ' get return details

            m_lRiskId = oRisk.RiskId

            m_iIsRiAtRiskLevel = oRisk.IsRiAtRiskLevel

            m_lStatus = oRisk.Status

            ' terminate interface

            oRisk.Dispose()

            ' destroy interface
            oRisk = Nothing

            ' if the risk interface was cancelled
            If m_lStatus <> PMEReturnCode.PMCancel Then

                ' get risk ratings
                nReturn = CType(GetRiskRating(iTask:=m_iTask), PMEReturnCode)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetRiskRating Failed", PMELogLevel.PMLogError)
                End If

                ' if the risk rating wasnt cancelled
                If m_lStatus <> PMEReturnCode.PMCancel Then

                    ' get risk reinsurace
                    nReturn = CType(GetRiskReinsurance(), PMEReturnCode)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetRiskReinsurance Failed", PMELogLevel.PMLogError)
                    End If
                    ' if risk reinsurance wasnt cancelled
                    If m_lStatus <> PMEReturnCode.PMCancel Then

                        ' indicate to the the user control that the risk was successfully editted.
                        uctPMUListRisk1.RiskID = m_lRiskId
                        uctPMUListRisk1.Editted = True
                        'Block Risk for posting which are not edited   PN 35099

                        m_lReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)
                        If nReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GetRiskReinsurance Failed", PMELogLevel.PMLogError)
                        End If
                        ' WPR53
                        m_lReturn = m_oBusiness.UnquoteMandatoryRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskId:=m_lRiskId)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "UnquoteMandatoryRisk Failed", PMELogLevel.PMLogError)
                        End If
                        If m_bBackdatedEditing Then
                            m_lReturn = m_oBusiness.UnquoteRisksForward(nRiskCnt:=m_lRiskId)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "UnquoteRisksForward Failed", PMELogLevel.PMLogError)
                            End If
                        End If
                        m_bIsPolicyEdited = True
                    End If
                End If
            End If

            ' get the latest version of the risks before recalculating
            nReturn = uctPMUListRisk1.GetRisks()
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetRisks Failed", PMELogLevel.PMLogError)
            End If
            cmdBackdateMTA.Visible = uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired
            cmdBackdateMTA.Enabled = uctPMUListRisk1.OKToProceed AndAlso m_bIsBackdatedMTARequired

            ' recalculate everything from policy down
            nReturn = CType(Recalculate(kRecalculateModeRisks), PMEReturnCode)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Recalculate Failed", PMELogLevel.PMLogError)
            End If
            If m_bIsBackdatedMTARequired Then
                ClearBackDatedMTAQuotes()
            End If

        End If

        GoTo Finally_Renamed

Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)

        ' If you want to rollback a transaction or something, do it here

Finally_Renamed:

        Try

            ' last thing to do in every circumstance is get the risks again
            uctPMUListRisk1.GetRisks()

        Catch
        End Try

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: ProcessDeleteRisk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessDeleteRisk() As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "ProcessDeleteRisk"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oRisk As iPMURisk.Interface_Renamed

        Dim bRiskHasClaim As Boolean

        Dim sSysOptionBDMTA As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether the current risk has a clim on it
            lReturn = m_oBusiness.CheckClaimOnRisk(v_lRiskId:=m_lRiskId,
                                                 v_bRiskHasClaim:=bRiskHasClaim)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("ProcessDeleteRisk", "Failed to CheckClaimOnRisk", gPMConstants.PMELogLevel.PMLogError)
                ProcessDeleteRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If

            If bRiskHasClaim Then
                If MessageBox.Show("A Claim is linked with this Risk. Are you sure want to delete this Risk?", "Delete Risk", MessageBoxButtons.YesNo) = DialogResult.No Then
                    Return result
                End If
            End If

            '------PLICO Backdate Changes-----------
            'get system option "Apply Back-Dated Risk Editing Restrictions"
            lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=5079, r_sOptionValue:=sSysOptionBDMTA, v_iSourceID:=g_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessDeleteRisk", "Failed to get System Option [5056]", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If sSysOptionBDMTA = "1" Then
                If m_bIsBackdatedMTARequired And Not (m_sTransactionType = "MTC" Or m_iInsuranceFileTypeID = kInsFileTypeMTATempQuote) Then
                    lReturn = CType(IsSubsequentRiskVersionsEdited(v_lRiskID:=m_lRiskId, v_dtMTAEffectiveDate:=m_dtCoverStartDate), gPMConstants.PMEReturnCode)
                    If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("The Risk cannot be deleted as the Future versions of the Risk have been edited!!", "Edit Risk", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    End If
                End If
            End If
            'End

            'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            lReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " - Save Cover Noted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            If uctPMUListRisk1.DiscountedRiskCount > 0 Then
                PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonDeleteRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return result
                End If
            End If
            cmdEditRisk.Enabled = False
            cmdDeleteRisk.Enabled = False
            cmdCopyRisk.Enabled = False

            ' get object instance
            Dim temp_oRisk As Object
            lReturn = g_oObjectManager.GetInstance(temp_oRisk, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRisk = temp_oRisk
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of iPMURisk.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set risk properties

            oRisk.PartyCnt = m_lPartyCnt

            oRisk.ShortName = m_sShortName

            oRisk.InsuranceFolderCnt = m_lInsuranceFolderCnt

            oRisk.InsuranceFileCnt = m_lInsuranceFileCnt

            oRisk.RiskId = m_lRiskId

            oRisk.ScreenId = m_lScreenId

            oRisk.ProductId = uctPMUListRisk1.ProductID

            oRisk.RiskTypeId = m_lRiskTypeId

            ' set process modes

            'Edit so that it generates a "C" record if necessary

            lReturn = oRisk.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=m_sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURisk.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' start interface

            lReturn = oRisk.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURisk.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return values
            'Just in case it's changed..

            m_lRiskId = oRisk.RiskId

            m_iIsRiAtRiskLevel = oRisk.IsRiAtRiskLevel

            m_lStatus = oRisk.Status

            ' terminate the interface

            oRisk.Dispose()

            ' destroy interface
            oRisk = Nothing

            ' if the risk interface was not cancelled by the user
            If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then

                ' set the risk id
                uctPMUListRisk1.RiskID = m_lRiskId

                ' attempt to delete the risk
                lReturn = uctPMUListRisk1.DeleteClick()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "uctPMUListRisk.DeleteClick Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Did we totally delete the risk (not just change the flag to D)
                ' if the risk has actually been deleted the risk id will be set to 0
                ' otherwise a valid risk cnt will be returned
                m_lRiskId = uctPMUListRisk1.RiskID

                ' if the risk wasnt actually deleted
                If m_lRiskId <> 0 Then

                    ' get the risk ratings
                    lReturn = CType(GetRiskRating(iTask:=gPMConstants.PMEComponentAction.PMDelete), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetRiskRating Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' if the user doesnt cancel the risk rating interface
                    If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                        m_bIsRiskDeleted = True
                        ' get the risks reinsurance
                        lReturn = CType(GetRiskReinsurance(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        'Block Risk for posting which are not edited   PN 35099

                        m_lReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        'WPR 33-75 added
                    End If
                End If
                ' Wpr53
                m_lReturn = m_oBusiness.UnquoteMandatoryRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskId:=m_lRiskId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UnquoteMandatoryRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                m_bIsPolicyEdited = True
                'WPR 33-75 added
            End If

            lReturn = uctPMUListRisk1.GetRisks()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ''WPR 33-75 added
            If uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired Then
                cmdBackdateMTA.Enabled = True
            Else
                cmdBackdateMTA.Enabled = False
            End If

            ' recalculate everything from policy down
            lReturn = CType(Recalculate(kRecalculateModeRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If uctPMUListRisk1.RiskCount = 0 Then
                cmdApplyDiscount.Enabled = False
            End If

            If m_bIsBackdatedMTARequired Then
                ClearBackDatedMTAQuotes()
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' last thing to do in every circumstance is get the risks again
            uctPMUListRisk1.GetRisks()


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetUpListRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetUpListRisks() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetUpListRisks"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' set properties
            uctPMUListRisk1.Task = m_iTask
            uctPMUListRisk1.InsFileCnt = m_lInsuranceFileCnt
            uctPMUListRisk1.TransactionType = m_sTransactionType '(RC)Pass TransactionType PN31949
            uctPMUListRisk1.FormLoading = m_bFormLoading
            ' initialise
            ' Developer Guide No. 9
            lReturn = uctPMUListRisk1.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initialise failed on uctPMUListRisk")
            End If

            ' load
            lReturn = uctPMUListRisk1.LoadControl()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadControl Failed on uctPMUListRisk", gPMConstants.PMELogLevel.PMLogError)
            End If

            'if we are cancelling then default risks to unquoted to force user to edit them for return premium
            'WPR 33-75 added
            If m_sTransactionType = "MTC" Or m_bIsBackdatedMTARequired Then

                ' Check that we are not clicking on Apply Discount button
                If Not m_bApplyDiscountClicked Then
                    'get risk status so we can put them back in case user cancel half way through

                    If m_oBusiness.GetRiskStatus(m_lInsuranceFileCnt, m_vRiskStatus) <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to retrieve risk status", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    If m_oBusiness.ResetRiskStatusForPolicyID(m_lInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to reset risk status for this policy", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            ' get risks
            lReturn = uctPMUListRisk1.GetRisks()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRisks failed on uctPMUListRisk", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return properties


            m_vOriginalSelStatus = uctPMUListRisk1.RiskSelectionStatus



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' SetupButtons
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupButtons() As Integer
        Const kMethodName As String = "SetupButtons"
        Dim nResult As Integer
        Dim oPaymentMethod(,) As Object = Nothing
        Dim bCanAutoQuoteRisks As Boolean = False
        Dim bAllRisksQuoted As Boolean = False
        Dim obSIRPremiumFinance As bSIRPremiumFinance.Business
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            cmdAddRisk.Enabled = Not (m_sTransactionType = "DRI")

            cmdEditRisk.Enabled = False
            cmdCopyRisk.Enabled = False
            cmdDeleteRisk.Enabled = False
            cmdWrite.Visible = False
            btnNOChange.Visible = False
            btnNoChangeAll.Visible = False
            nResult = m_oBusiness.GetPaymentMethod(m_lInsuranceFileCnt, oPaymentMethod)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupButtons Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdEditRisk.Text = "&View"
                cmdAddRisk.Visible = False
                cmdDeleteRisk.Visible = False

                If Information.IsArray(oPaymentMethod) Then

                    If CStr(oPaymentMethod(0, 0)) = "Invoice" Then
                        OptInvoice.Checked = True
                    ElseIf CStr(oPaymentMethod(0, 0)) = "Instalments" OrElse CStr(oPaymentMethod(0, 0)) = "Direct Debit" OrElse CStr(oPaymentMethod(0, 0)) = "Credit Card" Then
                        If uctInstalments1.QuoteAvailable Then
                            If m_bIsTrueMonthlyPolicy AndAlso m_sTransactionType <> "NB" Then
                                OptPutMTAOnNextInstalment.Checked = True
                                OptPutMTAOnNextInstalment.Visible = True
                            Else
                                OptInstalments.Checked = True
                            End If
                        Else
                            OptInstalments.Checked = False
                            m_iSpecifiedTab = kTabRisk
                        End If
                    ElseIf CStr(oPaymentMethod(0, 0)) = "PayNow" Then
                        optPayNow.Checked = True
                    ElseIf CStr(oPaymentMethod(0, 0)) = "CashDeposit" Then
                        optCashDeposit.Checked = True
                    End If
                    SSTabHelper.SetSelectedIndex(SSTab1, m_iSpecifiedTab)
                End If
            End If

            If m_bWrittenStatusPermitted Then
                cmdWrite.Visible = True
                If Trim$(m_sInsuranceFileStatusCode) = "WRITTEN" Then
                    cmdWrite.Enabled = False
                    cmdSaveQuote.Text = "&Save"
                End If
                If m_sTransactionType <> "NB" Then
                    cmdWrite.Enabled = False
                End If
            End If
            If m_sTransactionType = "MTC" Then
                cmdAddRisk.Visible = False
            End If

            If (m_sTransactionType = "MTC" OrElse m_sTransactionType = "MTR" OrElse m_sTransactionType = "MTA" OrElse m_sTransactionType = "REN" OrElse m_sTransactionType = "NB") Then
                nResult = CanAutoQuoteRisks(r_bCanAutoQuoteRisks:=bCanAutoQuoteRisks,
                                            r_bAllRisksQuoted:=bAllRisksQuoted)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "CanAutoQuoteRisks Failed", PMEReturnCode.PMLogError1)
                End If

                If bCanAutoQuoteRisks Then
                    cmdQuoteAllRisks.Enabled = True
                Else
                    cmdQuoteAllRisks.Enabled = False
                End If
            End If

            If m_sTransactionType = "MTA" Then
                nResult = gPMConstants.PMEReturnCode.PMTrue

                obSIRPremiumFinance = New bSIRPremiumFinance.Business

                obSIRPremiumFinance.Initialise(sUserName:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
                If obSIRPremiumFinance Is Nothing Then
                    Throw New ApplicationException("Failed to create instance of bSIRPremiumFinance.Business")
                End If

                Dim sDefaultPaymentMethod As String = Nothing
                Dim nDefaultInstalmentPlan As Integer
                Dim nDefaultInstalmentPlanVersion As Integer
                Dim nDefaultSchemeNumber As Integer
                Dim nDefaultSchemeVersion As Integer

                nResult = obSIRPremiumFinance.GetDefaultPaymentTerms(m_lInsuranceFileCnt, sDefaultPaymentMethod, nDefaultInstalmentPlan, nDefaultInstalmentPlanVersion, nDefaultSchemeNumber, nDefaultSchemeVersion)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Failed to execute method bSIRPremiumFinance.GetDefaultPaymentTerms()")
                Else
                    oPaymentMethod(0, 0) = sDefaultPaymentMethod
                End If

                If Information.IsArray(oPaymentMethod) Then
                    If CStr(oPaymentMethod(0, 0)).ToUpper() = "INVOICE" Then
                        OptInvoice.Checked = True
                    ElseIf CStr(oPaymentMethod(0, 0)).ToUpper() = "INSTALMENTS" OrElse CStr(oPaymentMethod(0, 0)).ToUpper() = "INSTALMENT" OrElse CStr(oPaymentMethod(0, 0)).ToUpper() = "DIRECT DEBIT" OrElse CStr(oPaymentMethod(0, 0)).ToUpper() = "CREDIT CARD" Then
                        uctInstalments1.PremiumFinanceCnt = nDefaultInstalmentPlan
                        uctInstalments1.PremiumFinanceVersion = nDefaultInstalmentPlanVersion
                        uctInstalments1.Refresh()
                        If uctInstalments1.QuoteAvailable Then
                            If m_bIsTrueMonthlyPolicy AndAlso m_sTransactionType <> "NB" Then
                                'Default Invoice for PutOnNextInstRenewal=False
                                OptPutMTAOnNextInstalment.Checked = m_bPutOnNextInstalmentRenewal
                            Else
                                OptInstalments.Checked = True
                            End If
                        Else
                            OptInstalments.Checked = False
                            m_iSpecifiedTab = kTabRisk
                        End If
                    ElseIf CStr(oPaymentMethod(0, 0)).ToUpper() = "PAYNOW" Then
                        optPayNow.Checked = True
                    ElseIf CStr(oPaymentMethod(0, 0)).ToUpper() = "BANKGUARANTEE" Then  ' Gaurav
                        optBankGuarantee.Checked = True
                    ElseIf CStr(oPaymentMethod(0, 0)).Trim().ToUpper() = "CASHDEPOSIT" Then
                        optCashDeposit.Checked = True
                    End If
                    SSTabHelper.SetSelectedIndex(SSTab1, m_iSpecifiedTab)
                    If uctInstalments1.HasInstalmentVersions Then
                        OptInstalments.Checked = True
                    End If
                End If

            ElseIf m_sTransactionType = "REN" Then
                If uctInstalments1.HasInstalmentVersions Then
                    OptInstalments.Checked = True
                End If
            End If

            If m_sTransactionType = "MTC" Then
                If m_bIsSingleInstalmentPlan And m_lNoOfPoliciesOnSingleInsAgent > 1 Then
                    OptInstalments.Checked = True
                Else
                    OptInvoice.Checked = True
                End If
            End If

            PopulatePaymentTerm()
            PopulateCollectionFrequency()
            If oPaymentMethod IsNot Nothing AndAlso ToSafeLong(oPaymentMethod(1, 0)) > 0 Then
                For iCnt As Integer = 0 To cboPaymentTerms.Items.Count - 1
                    If CInt(oPaymentMethod(1, 0)) = VB6.GetItemData(cboPaymentTerms, iCnt) Then
                        cboPaymentTerms.SelectedIndex = iCnt
                        Exit For
                    End If
                Next
            End If

            If m_bIsMarketPlacePolicy Then
                cmdMakeLive.Enabled = False
                cmdAddRisk.Enabled = False
                cmdDeleteRisk.Enabled = False
                cmdCopyRisk.Enabled = False
            End If

            If oPaymentMethod IsNot Nothing AndAlso ToSafeLong(oPaymentMethod(2, 0)) > 0 Then
                For iCnt As Integer = 0 To cboCollectionFrequency.Items.Count - 1
                    If CInt(oPaymentMethod(2, 0)) = VB6.GetItemData(cboCollectionFrequency, iCnt) Then
                        cboCollectionFrequency.SelectedIndex = iCnt
                        Exit For
                    End If
                Next
            End If

            If m_bIsTrueMonthlyPolicy Then
                Dim iIndex As Integer
                iIndex = cboCollectionFrequency.FindString("MONTHLY")
                If iIndex >= 0 Then
                    cboCollectionFrequency.SelectedIndex = iIndex
                    cboCollectionFrequency.Enabled = False
                End If
                If m_iTask = PMEComponentAction.PMView Then
                    cboPaymentTerms.Enabled = False
                ElseIf m_sTransactionType = "NB" OrElse m_sTransactionType = "REN" Then
                    cboPaymentTerms.Enabled = True
                ElseIf m_sTransactionType = "MTR" AndAlso ToSafeDate(txtCoverFromDate.Text) = ToSafeDate(m_dtInceptionDateTPI) Then
                    cboPaymentTerms.Enabled = True
                Else
                    cboPaymentTerms.Enabled = False
                End If
            Else
                If m_iTask = PMEComponentAction.PMView Then
                    cboPaymentTerms.Enabled = False
                    cboCollectionFrequency.Enabled = False
                ElseIf m_sTransactionType = "NB" OrElse m_sTransactionType = "REN" Then
                    cboPaymentTerms.Enabled = True
                    cboCollectionFrequency.Enabled = True
                ElseIf m_sTransactionType = "MTR" AndAlso ToSafeDate(txtCoverFromDate.Text) = ToSafeDate(m_dtInceptionDateTPI) Then
                    cboPaymentTerms.Enabled = True
                    cboCollectionFrequency.Enabled = True
                Else
                    cboPaymentTerms.Enabled = False
                    cboCollectionFrequency.Enabled = False
                End If
            End If

            If m_bBackdatedEditing Then
                cmdEditRisk.Enabled = False
                cmdAddRisk.Enabled = False
                cmdDeleteRisk.Enabled = False
                cmdCopyRisk.Enabled = False

                cmdPrintDocument.Enabled = False
                cmdPrintProposal.Enabled = False
                cmdPrintQuote.Enabled = False
                cmdMakeLive.Enabled = False
                cmdDocArchive.Enabled = False

                OptInstalments.Enabled = False
                OptInvoice.Enabled = False
                optPayNow.Enabled = False
                optBankGuarantee.Enabled = False
                optCashDeposit.Enabled = False
                optMarkForCollection.Enabled = False
                btnNOChange.Visible = True
                btnNoChangeAll.Visible = True
            End If
            If Not uctPMUListRisk1.AllRiskStatusQuoted Then
                btnNoChangeAll.Enabled = True
            End If
            If m_iTask = PMEComponentAction.PMView AndAlso OptInstalments.Checked = True Then
                SSTabHelper.SetTabVisible(SSTab1, kTabInstalments, False)
            End If

            Return nResult
        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=excep)
            Return nResult
        Finally
            If Not obSIRPremiumFinance Is Nothing Then
                obSIRPremiumFinance.Dispose()
                obSIRPremiumFinance = Nothing
            End If
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetProductOptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetProductOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductOptions"

        Dim lReturn As Integer
        'Developer Guide No. 101
        Dim vAdmiralForceRisks As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            '**********************************
            ' Get the Admiral Force Risks flag
            '**********************************
            'Developer Guide No. 98
            lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAdmiralForceRisks, v_vBranch:=g_iSourceID, r_vUnderwriting:=vAdmiralForceRisks)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTAdmiralForceRisks, gPMConstants.PMELogLevel.PMLogError)
            End If
            'Developer Guide No. 12
            m_bAdmiralForceRisks = (vAdmiralForceRisks = "1")
            '**********************************
            '**********************************




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessAddRisk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessAddRisk() As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "ProcessAddRisk"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oRisk As iPMURisk.Interface_Renamed
        Dim lRiskNumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " - Save Cover Noted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
            If uctPMUListRisk1.DiscountedRiskCount > 0 Then
                PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonAddRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return result
                End If
            End If
            ' get product id
            m_lProductId = uctPMUListRisk1.ProductID

            ' get risk type to add from user input
            lReturn = CType(GetRiskType(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRiskType Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if the user has chosen to cancel on the get risk type interface
            ' then quit the add process without error
            If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            ' get instance of risk interface
            Dim temp_oRisk As Object
            lReturn = g_oObjectManager.GetInstance(temp_oRisk, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRisk = temp_oRisk
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of iPMURisk.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' set properties

            oRisk.PartyCnt = m_lPartyCnt

            oRisk.ShortName = m_sShortName

            oRisk.InsuranceFolderCnt = m_lInsuranceFolderCnt

            oRisk.InsuranceFileCnt = m_lInsuranceFileCnt

            oRisk.RiskId = 0

            oRisk.ScreenId = m_lScreenId

            oRisk.ProductId = m_lProductId

            oRisk.RiskTypeId = m_lRiskTypeId

            'WPR 33-75 added
            oRisk.SourceId = m_iSourceID

            ' set process modes

            lReturn = oRisk.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=m_sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURisk.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' reset the task ???

            oRisk.Task = gPMConstants.PMEComponentAction.PMAdd

            ' start the interface

            lReturn = oRisk.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMURisk.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return details from the interface
            Application.DoEvents()
            m_lStatus = oRisk.Status

            'WPR 33-75 added commented as per WPR
            'm_iIsRiAtRiskLevel = oRisk.IsRiAtRiskLevel

            m_lRiskId = oRisk.RiskId

            ' terminate the interface

            oRisk.Dispose()

            ' destroy the object instance
            oRisk = Nothing

            ' Find out the next risk number for this policy

            lReturn = m_oBusiness.GetNextRiskNo(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lRiskNumber:=lRiskNumber)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to retrieve Next Risk Number", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Save the risk number to the risk record

            lReturn = m_oBusiness.UpdateRiskNo(v_lRiskCnt:=m_lRiskId, v_lRiskNumber:=lRiskNumber)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to update the Next Risk Number on the Risk record", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' The risk is created anyway on cancel, so have moved this bit
            ' of code to after the previous 2 blocks, so the risk is given a number.
            ' Note: at time of writing, risk ID is not returned if cancelled, so
            ' the risk number will not be written. However a change request has
            ' been made for policybuilder to return the risk ID.
            If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then

                ' get the risk ratings
                lReturn = CType(GetRiskRating(iTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetRiskRating Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then

                    ' get the risk reinsurance
                    lReturn = CType(GetRiskReinsurance(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                        'Tell the user control that the risk was successfully editted.
                        uctPMUListRisk1.RiskID = m_lRiskId
                        uctPMUListRisk1.Editted = True
                        'Block Risk for posting which are not edited at the time of MTA    PN 35099

                        m_lReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        ' Wpr53
                        m_lReturn = m_oBusiness.UnquoteMandatoryRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskId:=m_lRiskId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "UnquoteMandatoryRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        m_bIsPolicyEdited = True
                    End If

                End If

            End If

            ' get the latest version of the risks before recalculating
            lReturn = uctPMUListRisk1.GetRisks()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate everything from policy down
            lReturn = CType(Recalculate(kRecalculateModeRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bIsBackdatedMTARequired Then

                ClearBackDatedMTAQuotes()

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' always get the risks regardless of whatever has happened above
            uctPMUListRisk1.GetRisks()

        End Try
        Return result
    End Function

    Private Sub ClearBackDatedMTAQuotes()
        Dim oOOSResultArray(,) As Object = Nothing
        Dim oAutoMTA As Object
        Dim nResult As Integer

        nResult = g_oObjectManager.GetInstance(oObject:=oAutoMTA,
                                     sClassName:="bSIRAutoMTA.Business",
                                     vInstanceManager:=PMGetViaClientManager)

        If (nResult <> PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the policy business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            Exit Sub
        End If

        m_lReturn = oAutoMTA.ClearBackdateMTAData(CLng(m_lInsuranceFileCnt))
        If (m_lReturn <> PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ClearBackdateMTAData", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
        End If

        'Terminate the Business Policy Object
        m_lReturn = oAutoMTA.Dispose()



    End Sub
    ''' <summary>
    ''' ProcessSaveQuote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessSaveQuote() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "ProcessSaveQuote"

        Dim nRenewalStatusTypeId As Integer
        Dim nIsTrueMonthlyPolicy As Integer
        Dim nDoNotDeleteRenewalQuoteOnMta As Integer
        Dim nAnniversaryCopy As Integer
        Dim oResultArray As Object

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask <> PMEComponentAction.PMView Then
                If m_sTransactionType = "REN" Then
                    nResult = ProcessPaymentTerms()
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessPaymentTerms Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
                If cmdApplyDiscount.Enabled And uctPMUListRisk1.RiskCount > 0 Then
                    MessageBox.Show("You must Apply Discount before a Quote can be saved, Click OK to continue", "Policy Discount Error", MessageBoxButtons.OK)
                    Return nResult
                End If

                ' Set the interface status.
                m_lStatus = PMEReturnCode.PMFail
                If m_bIsBackdatedMTARequired AndAlso Not m_bSaveBackdatedQuotes Then

                    nResult = m_oAutoMTA.ClearBackdateMTAData(m_lInsuranceFileCnt)
                End If
                If m_sTransactionType = "MTA" And m_bBackdatedEditing = False Then
                    m_lReturn = m_oRenewal.GetRenewalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                      r_dtResult:=oResultArray)
                    If (IsArray(oResultArray)) Then
                        nRenewalStatusTypeId = ToSafeInteger(oResultArray(0, 0))
                        nIsTrueMonthlyPolicy = ToSafeInteger(oResultArray(1, 0))
                        nDoNotDeleteRenewalQuoteOnMta = ToSafeInteger(oResultArray(2, 0))
                        nAnniversaryCopy = ToSafeInteger(oResultArray(3, 0))
                        If (nRenewalStatusTypeId <> -1 AndAlso nDoNotDeleteRenewalQuoteOnMta = 1 AndAlso nIsTrueMonthlyPolicy = 1 AndAlso nAnniversaryCopy = 1) Then
                            MsgBox("This Policy is in Renewal. Please Confirm that you will also apply your amendment to the renewal version of this policy manually", vbOKOnly, "Confirm")
                        ElseIf (nRenewalStatusTypeId <> -1 AndAlso nDoNotDeleteRenewalQuoteOnMta = 1 AndAlso nIsTrueMonthlyPolicy = 0) Then
                            MsgBox("This Policy is in Renewal. Please Confirm that you will also apply your amendment to the renewal version of this policy manually", vbOKOnly, "Confirm")
                        End If
                    End If
                End If
                ' update the required policies details
                nResult = UpdatePolicyDetails()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdatePolicyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                nResult = uctPMUListRisk1.SaveAllCoverNotesOnQuote()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdatePolicyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If nResult = gPMConstants.PMEReturnCode.PMTrue Then

                    m_sQuoteStatus = "Quote Saved"
                    RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                    Me.Hide()
                End If
            ElseIf m_sTransactionType = "PT" Or m_sTransactionType = "DRI" Then
                If uctPMUListRisk1.OKToProceed And m_sTransactionType = "DRI" Then
                    m_bIsAllRiskQuoted = True
                End If
                If Not m_bIsRIAmended Then
                    m_lStatus = PMEReturnCode.PMCancel
                Else
                    'set the status as PMOK which is used further in checking Portfolio status as amend or accept
                    m_lStatus = PMEReturnCode.PMOK
                End If
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
            Else
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Me.Close()

            End If

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' This method is used to check wheather system have any attached any live Plan or  Not.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChecklatestValidInstalmentPlanYN() As Integer
        Dim nReturnValue As Integer = gPMConstants.PMEReturnCode.PMTrue

        'Get an instance of the business object via the public object manager
        Dim oTempBusiness As Object = Nothing
        Dim oPFPremiumFinance As Object = Nothing
        nReturnValue = g_oObjectManager.GetInstance(oTempBusiness, "bSIRPremiumFinance.Business", vInstanceManager:="ClientManager")

        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
            'Failed to get an instance of the business object.
            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
            Return nReturnValue
        End If
        nReturnValue = oTempBusiness.GetLatestValidFinancePlan(nInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                               r_oPFPremiumFinance:=oPFPremiumFinance,
                                                               sBusinessType:="MTC")

        If Not IsArray(oPFPremiumFinance) Then
            nReturnValue = nReturnValue
        Else
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue
        End If
        Return nReturnValue
    End Function

    ''' <summary>
    ''' Process make live
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessMakeLive() As Integer
        Dim nResult As Integer

        Const kMethodName As String = "ProcessMakeLive"
        Dim objfrmPostingPeriod As frmPostingPeriod = New frmPostingPeriod
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vSelectionArray As Object
        Dim nValidationStatus As Integer = 0
        Dim m_sFailureReason As String = ""
        Dim oReturnArray(,) As Object = Nothing
        Dim bCanOverride As Boolean
        Dim bAlreadyLive As Boolean
        Dim obSIRInsuranceFile As bSIRInsuranceFile.Business
        Dim lInsuranceFileCnt, lInsuranceFolderCnt As Integer
        Const kCancelPlanOnMTC As Integer = 5076
        Dim sSysOptionValue As String = ""
        Dim bProceed As Boolean
        Dim r_bIsValid As Boolean
        Dim bIsSingleinstalmentPolicy As Boolean

        Dim dtResult As DataTable

        Dim nRenewalStatusTypeId As Integer
        Dim nIsTrueMonthlyPolicy As Integer
        Dim nDoNotDeleteRenewalQuoteOnMta As Integer
        Dim nDeleteRenQuoteReRunRenewal As Integer
        Dim iAnniversaryCopy As Integer
        Dim oRenewalArray As Object
        Dim oActivePlan As Object = Nothing
        Dim sPlanStatusInd As String = String.Empty
        Dim sPlanStatus As String = String.Empty
        Dim vOptionValue As Object = ""
        Dim bCopyRiskOnMTA As Boolean = False

        Try
            nResult = PMEReturnCode.PMTrue
            b_IsMakeLiveClicked = True
            bAlreadyLive = False

            m_lReturn = ValidateCertificateYear(r_bIsValid)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute ValidateCertificateYear.",
                                        gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFail
            End If
            If r_bIsValid = False Then
                Return nResult
            End If
            If m_sTransactionType = "MTC" Or (m_sTransactionType = "MTA" And m_bIsNegativeInstallents = True And OptInstalments.Checked = False) Then
                'get system option "Cancel Instalment Plan on Policy Cancellation"
                nResult = iPMFunc.GetSystemOption(v_iOptionNumber:=kCancelPlanOnMTC, r_sOptionValue:=sSysOptionValue, v_iSourceID:=g_iSourceID)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Could not get System Option: Cancel Instalment Plan on Policy Cancellation",
                                            gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
                m_lCancelFPOnCancelPolicy = gPMFunctions.ToSafeLong(sSysOptionValue, 0)
            End If
            If m_sTransactionType.Trim().ToUpper() = "MTC" Then
                'If plan is not to be cancelled with policy then prompt for message
                If m_lCancelFPOnCancelPolicy <> 1 Then
                    'validate here for current policy which is being cancelled ,if there is any plan for the policy
                    Dim obSIRPremiumFinance As Object = Nothing
                    Dim oPFPremiumFinance As Object = Nothing
                    nResult = g_oObjectManager.GetInstance(obSIRPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:="ClientManager")
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get instance of business object bSIRPremiumFinance.", gPMConstants.PMELogLevel.PMLogError)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                    nResult = obSIRPremiumFinance.GetLatestValidFinancePlan2(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vPFPremiumFinance:=oPFPremiumFinance, v_sBusinessType:="MTC")
                    If nResult <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If Not IsArray(oPFPremiumFinance) Then
                            sPlanStatusInd = ""
                        Else
                            sPlanStatusInd = ToSafeString(oPFPremiumFinance(k_PFPlanStatusInd, 0))
                            Select Case sPlanStatusInd
                                Case bSIRPremFinConst.PFStatusIndLive.Trim() : sPlanStatus = "Live"
                                Case bSIRPremFinConst.PFStatusIndOnHold.Trim() : sPlanStatus = "On Hold"
                            End Select
                        End If
                    End If
                    If sPlanStatusInd <> "" Then
                        MessageBox.Show("You cannot proceed with the cancellation of policy as there is """ & sPlanStatus & """ plan attached to the policy",
                                        "Cancel Policy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                End If
            End If

            If m_sTransactionType = "NB" OrElse m_sTransactionType = "MTA" OrElse m_sTransactionType = "REN" Then

                m_lReturn = m_oBusiness.GetPolicyVersionDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                r_vResults:=oReturnArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileDetails Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
                If Information.IsArray(oReturnArray) Then
                    'if last transaction was not modified by current user
                    'Check if transaction is already made live that can happen in a multi-user environment
                    If gPMFunctions.ToSafeLong(oReturnArray(4, 0)) <> gPMFunctions.ToSafeLong(g_iUserID) Then
                        If (m_sTransactionType = kInsFileTransactionTypeNB OrElse
                             m_sTransactionType = kInsFileTransactionTypeREN) AndAlso
                            gPMFunctions.ToSafeLong(oReturnArray(0, 0)) = kInsFileTypeLive Then
                            bAlreadyLive = True
                        ElseIf m_sTransactionType = kInsFileTransactionTypeMTA AndAlso
                            (gPMFunctions.ToSafeLong(oReturnArray(0, 0)) = kInsFileTypeMTAPerm OrElse
                             gPMFunctions.ToSafeLong(oReturnArray(0, 0)) = kInsFileTypeMTATemp) Then
                            bAlreadyLive = True
                        End If

                        If bAlreadyLive Then
                            m_lStatus = gPMConstants.PMEReturnCode.PMFail
                            m_sQuoteStatus = "Process Cancelled - This policy has already been processed"
                            MessageBox.Show(
                                "This transaction has already been made live by user " &
                                gPMFunctions.ToSafeString(oReturnArray(5, 0)), Application.ProductName)
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                            Me.Hide()
                            If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA OrElse m_sTransactionType = "NB" Then
                                UNLOCKPOLICY()
                                UnLockSingleInstalmentPlan()
                            End If
                            Return nResult
                        End If
                    End If
                    'Reset
                    oReturnArray = Nothing
                End If
            End If

            If m_sTransactionType = "MTA" Then
                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then 'discount
                    If uctPMUListRisk1.DiscountedRiskCount > 0 And cmdApplyDiscount.Enabled Then
                        PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonEditRisk)
                        If m_bCancelAboutToChangeAction Then
                            m_bCancelAboutToChangeAction = False
                            Return nResult
                        End If
                    End If

                End If
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to get the value for SIROPTCopyRiskInMTA", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return m_lReturn
                End If
                If vOptionValue = "1" Then
                    bCopyRiskOnMTA = True
                End If
                If bCopyRiskOnMTA Then
                    m_lReturn = m_oBusiness.CopyRisksMTA(m_lInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Unable to copy Risks for MTA", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return m_lReturn
                    End If
                End If


            End If

            ' Process the OK in the control
            lReturn = uctPMUListRisk1.OKClick()

            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return nResult
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctPMUListRisk1.OKClick Failed",
                                        gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
            If m_sTransactionType = "MTC" Or (m_sTransactionType = "MTA" And ToSafeCurrency(txtGrossTotal.Text) < 0) Then

                m_lReturn = ProcessPolicyReceiptMediatTypeStatus(r_bProceed:=bProceed)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute ProcessPolicyReceiptMediatTypeStatus",
                                            gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If

                If Not bProceed Then
                    MsgBox("Please refer to accounts as the status of receipts is not cleared", vbOKOnly + vbInformation,
                           ACApp)
                    Return nResult
                End If
            End If

            ' process validation rules
            m_lReturn = ProcessValidation(nValidationStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessValidation Failed", gPMConstants.PMELogLevel.PMLogError)
                Return m_lReturn
            End If

            validateProcess = nValidationStatus

            If nValidationStatus = kValidationSuccessful Then

                'BackdateMTA
                If m_bMTABackdatedOK Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


                    Try
                        m_oProgressBar = CreateObject("iPMBProgressBarWrapper.Wrapper")
                    Catch ex As Exception

                    End Try

                    If (m_oProgressBar Is Nothing) = False Then
                        m_oProgressBar.Caption = "  Making Backdated Quotes Live . . ."
                        m_oProgressBar.Text = "It may take several minutes to process. Please wait..."
                        m_oProgressBar.StartBar = True
                    End If

                    m_lReturn = m_oAutoMTA.SetProcessModes(vTransactionType:=m_sTransactionType)

                    If m_sTransactionType = "MTA" Then

                        m_lReturn = m_oAutoMTA.QuoteMTA(v_lPartyCnt:=m_lPartyCnt,
                                                            v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                                            v_dtEffectiveDate:=m_dtCoverStartDate,
                                                            lBaseInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                            r_sFailureMessage:=m_sFailureReason,
                                                            vBackdatedMTAVersions:=m_vBackdatedMTAVersions,
                                                            bUpdateStats:=True)
                    ElseIf m_sTransactionType = "MTC" Then

                        lReturn = m_oAutoMTA.QuoteCancellation(v_lPartyCnt:=m_lPartyCnt,
                                                               v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                                               v_dtEffectiveDate:=m_dtEffectiveDate,
                                                               r_lNewInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                               r_sFailureMessage:=m_sFailureReason,
                                                               lBaseInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                               bUpdateStats:=True, bBackDateMTA:=True)
                    ElseIf m_sTransactionType = "MTR" Then
                        lReturn = m_oAutoMTA.QuoteReinstatement(nPartyCnt:=m_lPartyCnt,
                                                               nInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                                               dtEffectiveDate:=m_dtEffectiveDate,
                                                               nNewInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                               sFailureMessage:=m_sFailureReason,
                                                               nBaseInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                               bUpdateStats:=True,
                                                               bBackDateMTA:=True)

                    End If
                    If (m_oProgressBar Is Nothing) = False Then
                        m_oProgressBar.StopBar = True
                    End If
                    ' Set the mouse pointer to busy.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "QuoteMTA Failed - " & m_sFailureReason,
                                                gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If
                End If

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                'Get UserCanOverride PostingPeriod from UserAuthorities

                m_lReturn = m_oBusiness.GetUserCanOverridePostingPeriod(g_iUserID, bCanOverride)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetUserCanOverridePostingPeriod Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If

                If bCanOverride Then

                    'Get Posting Periods

                    m_lReturn = m_oBusiness.GetOpenPostingPeriods(m_dtCoverStartDate, oReturnArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetOpenPostingPeriods Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    If Information.IsArray(oReturnArray) Then

                        If oReturnArray.GetUpperBound(1) > 0 Then
                            'Show Posting Periods dialog


                            'Developer Guide No. 50
                            objfrmPostingPeriod.m_vOpenPeriodArray = oReturnArray
                            objfrmPostingPeriod.ShowDialog()

                            If objfrmPostingPeriod.PostingPeriod = 0 Then
                                validateProcess = 0
                                Return nResult 'Cancel Make Live call
                            Else
                                'Update Posting PeriodID

                                m_lReturn = m_oBusiness.UpdatePolicyPostingPeriod(InsuranceFileCnt,
                                                                                  v_lPostingPeriodID:=
                                                                                     objfrmPostingPeriod.PostingPeriod)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPostingPeriod Failed",
                                                            gPMConstants.PMELogLevel.PMLogError)
                                    Return nResult
                                End If
                            End If

                            'Update Posting Period
                            objfrmPostingPeriod.Close()
                        End If
                    End If
                End If
                '(RC) IH-UDPP   -END

                m_bIsNegativeInstallents = gPMFunctions.ToSafeCurrency(uctInstalments1.GrossDue - (uctInstalments1.FeeExcluded + uctInstalments1.TaxExcluded), 0) < 0

                If m_sTransactionType = "MTC" OrElse (m_sTransactionType = "MTA" AndAlso m_bIsNegativeInstallents AndAlso
                                                      Not OptInstalments.Checked AndAlso Not OptInvoice.Checked AndAlso
                                                      Not optPayNow.Checked) Then
                    If m_lCancelFPOnCancelPolicy = 1 Then

                        lReturn = ProcessPolicyRefund()
                        If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            gPMFunctions.RaiseError(kMethodName, "ProcessPolicyRefund Failed", gPMConstants.PMELogLevel.PMLogError)
                            Return nResult
                        End If
                    Else

                        lReturn = ProcessPaymentTerms()
                        If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            gPMFunctions.RaiseError(kMethodName, "ProcessInstalments Failed", gPMConstants.PMELogLevel.PMLogError)
                            Return nResult
                        End If
                        'Compare with 3.2 and apply Condition Yogender singh 3 Mar 2014
                        'End - Prakash - WPR85_Paralleling
                    End If
                Else

                    lReturn = ProcessPaymentTerms()
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessInstalments Failed", gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If
                End If

                If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return nResult
                End If
                ' get an array of the the risks selection status

                vSelectionArray = uctPMUListRisk1.RiskSelectionStatus
                ' Update the risk records with the selection status'
                If Information.IsArray(vSelectionArray) Then

                    m_lReturn = m_oBusiness.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Unable to update the risk selection status.", Application.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return nResult
                    End If
                End If

                ' Put in as part of a change for Admiral whist instalments is being
                ' implemented. NOTE this cannot be used for any other implementation
                ' other than Admiral
                If m_bAdmiralForceRisks Then
                    If uctPMUListRisk1.CheckForMandatoryRisks() = gPMConstants.PMEReturnCode.PMFalse Then
                        'MsgBox "Cannot proceed, a mandatory risk has not been added.", vbCritical
                        Return nResult
                    End If
                End If

                lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
                End If



                ' process change policy status
                lReturn = ProcessChangePolicyStatus()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessChangePolicyStatus", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' update policy details
                lReturn = UpdatePolicyDetails()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' reset any applied discount fields
                lReturn = ProcessPolicyMakeLive()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessPolicyDiscountMakeLive Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                End If

                'START PN52000
                'Added code for cancelling all other mta reinstated quotes other than the selected one during MTR
                If _
                    m_iTask = gPMConstants.PMEComponentAction.PMEdit AndAlso
                    (m_sTransactionType = "MTA" OrElse m_sTransactionType = "MTR") Then

                    Dim temp_obSIRInsuranceFile As Object = Nothing
                    m_lReturn = g_oObjectManager.GetInstance(temp_obSIRInsuranceFile, "bSIRInsuranceFile.Business",
                                                             vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    obSIRInsuranceFile = temp_obSIRInsuranceFile
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRInsuranceFile.Business",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = m_oBusiness.GetMTAQuotePolicyVersions(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                      v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                                                      r_vResults:=oReturnArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileDetails Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    If Information.IsArray(oReturnArray) Then

                        For iLoop As Integer = oReturnArray.GetLowerBound(1) To oReturnArray.GetUpperBound(1)
                            lInsuranceFileCnt = gPMFunctions.ToSafeLong(oReturnArray(0, iLoop))
                            lInsuranceFolderCnt = gPMFunctions.ToSafeLong(oReturnArray(1, iLoop))
                            If lInsuranceFileCnt > 0 AndAlso lInsuranceFolderCnt > 0 Then

                                m_lReturn = obSIRInsuranceFile.MTACancellation(v_lInsuranceFileCnt:=lInsuranceFileCnt,
                                                                               v_lInsuranceFolderCnt:=lInsuranceFolderCnt,
                                                                               v_lPartyCnt:=m_lPartyCnt)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    MessageBox.Show("Failed to policy status from MTA Quote to Permanent MTA", ACApp,
                                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            End If
                        Next
                    End If
                    obSIRInsuranceFile = Nothing
                End If

                If m_sTransactionType = "REN" AndAlso m_sPaymentTerms = "Invoice" Then
                    m_lReturn = DeletePFPremiumFinance()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "DeletePFPremiumFinance Failed", gPMConstants.PMELogLevel.PMLogError)
                        Exit Function
                    End If
                End If

                ' Make Live successful. Set the policy make live status
                m_iPolicyMakeLiveStatus = MainModule.EPolicyMakeLiveStatus.PolicyMadeLive
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                If m_sTransactionType = "MTA" Then
                    m_lReturn = m_oRenewal.GetRenewalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                             r_dtResult:=dtResult)
                    If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 1 Then
                        For iLoop As Integer = 0 To dtResult.Rows.Count - 1
                            nRenewalStatusTypeId = CInt(dtResult.Rows(iLoop).Item(0))
                            nIsTrueMonthlyPolicy = CInt(dtResult.Rows(iLoop).Item(1))
                            nDoNotDeleteRenewalQuoteOnMta = CInt(dtResult.Rows(iLoop).Item(2))
                            iAnniversaryCopy = CInt(dtResult.Rows(iLoop).Item(3))
                            If (nRenewalStatusTypeId <> -1 And nDoNotDeleteRenewalQuoteOnMta = 1 And nIsTrueMonthlyPolicy = 1 And iAnniversaryCopy = 0) Then
                                m_lReturn = m_oRenewal.DeletePolicyFromRenewal(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_bRetainAnniversaryCopy:=True)
                                If m_bMTABackdatedOK Then
                                    m_lReturn = m_oRenewal.GetLatestVersionForRenewal(m_lInsuranceFolderCnt, oRenewalArray)
                                    If IsArray(oRenewalArray) Then
                                        m_lReturn = m_oRenSelection.ProcessRenewalSelection(v_lInsuranceFileCnt:=CLng(oRenewalArray(0, 0)),
                                                                                           v_bDoNotCreateTMPAnniversaryVersion:=True)
                                    End If
                                Else
                                    m_lReturn = m_oRenSelection.ProcessRenewalSelection(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                                       v_bDoNotCreateTMPAnniversaryVersion:=True)
                                End If
                            ElseIf (nRenewalStatusTypeId <> -1 And nDoNotDeleteRenewalQuoteOnMta = 1 And nIsTrueMonthlyPolicy = 1 And iAnniversaryCopy = 1) Then
                                MsgBox("This Policy is in Renewal. Please Confirm that you will also apply your amendment to the renewal version of this policy", vbOKOnly, "Confirm")
                                Call AddWorkManagerTask()
                            ElseIf (nRenewalStatusTypeId <> -1 And nDoNotDeleteRenewalQuoteOnMta = 0) Then
                                m_lReturn = m_oRenewal.DeletePolicyFromRenewal(m_lInsuranceFileCnt)
                                Exit For
                            End If
                        Next
                    ElseIf dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                        nRenewalStatusTypeId = CInt(dtResult.Rows(0).Item(0))
                        nIsTrueMonthlyPolicy = CInt(dtResult.Rows(0).Item(1))
                        nDoNotDeleteRenewalQuoteOnMta = CInt(dtResult.Rows(0).Item(2))
                        iAnniversaryCopy = CInt(dtResult.Rows(0).Item(3))
                        nDeleteRenQuoteReRunRenewal = CInt(dtResult.Rows(0).Item(5))
                        If nRenewalStatusTypeId <> -1 Then
                            If (nDoNotDeleteRenewalQuoteOnMta = 0 And nDeleteRenQuoteReRunRenewal = 0) Then
                                m_lReturn = m_oRenewal.DeletePolicyFromRenewal(m_lInsuranceFileCnt)
                            ElseIf ((nDoNotDeleteRenewalQuoteOnMta = 1) AndAlso (nIsTrueMonthlyPolicy = 0)) Then
                                MsgBox(
                                    "This Policy is in Renewal. Please Confirm that you will also apply your amendment to the renewal version of this policy manually",
                                    vbOKOnly, "Confirm")
                                AddWorkManagerTask()
                            ElseIf _
                                ((nDoNotDeleteRenewalQuoteOnMta = 1) AndAlso (nIsTrueMonthlyPolicy = 1) AndAlso
                                 (iAnniversaryCopy = 1)) Then
                                MsgBox(
                                    "This Policy is in Renewal. Please Confirm that you will also apply your amendment to the renewal version of this policy manually",
                                    vbOKOnly, "Confirm")
                                m_lReturn = m_oRenewal.DeletePolicyFromRenewal(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                               v_bRetainAnniversaryCopy:=True)
                                AddWorkManagerTask()
                            ElseIf _
                                ((nDoNotDeleteRenewalQuoteOnMta = 1) AndAlso (nIsTrueMonthlyPolicy = 1) AndAlso
                                 (iAnniversaryCopy = 0)) Then
                                m_lReturn = m_oRenewal.DeletePolicyFromRenewal(m_lInsuranceFileCnt)
                                If m_bMTABackdatedOK Then
                                    m_lReturn = m_oRenewal.GetLatestVersionForRenewal(m_lInsuranceFolderCnt, oRenewalArray)
                                    If IsArray(oRenewalArray) Then
                                        m_lReturn = m_oRenSelection.ProcessRenewalSelection(v_lInsuranceFileCnt:=CLng(oRenewalArray(0, 0)),
                                                                                           v_bDoNotCreateTMPAnniversaryVersion:=True)
                                    End If
                                Else
                                    m_lReturn = m_oRenSelection.ProcessRenewalSelection(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                                       v_bDoNotCreateTMPAnniversaryVersion:=True)
                                End If
                            End If
                            If nDeleteRenQuoteReRunRenewal = 1 And nDoNotDeleteRenewalQuoteOnMta = 0 Then
                                vbMsg = MsgBox("A version of this policy is already in renewal." & vbNewLine & "Do you wish to delete the existing renewal version and re-select the policy?", vbYesNo, "Renewals")
                                If (vbMsg = vbYes) Then
                                    m_lReturn = m_oRenewal.DeletePolicyFromRenewal(m_lInsuranceFileCnt)
                                    ReRunRenewalSelection()
                                End If
                            End If
                        End If
                    End If
                End If

                ' Everything OK, so we can hide the interface.
                RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
                Me.Hide()
                
            Else
                ' validation failed - do not proceed
                ' do not hide interface
            End If

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' ProcessBackDateMTA
    ''' </summary>
    ''' <param name="vPolicyRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessBackDateMTA(Optional ByRef vPolicyRef As String = "") As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessBackDateMTA"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim m_sFailureReason As String = ""
        Dim oBackdateMTA As frmBackdatedMTA
        Dim NetMTAPremium As Decimal

        Dim bShowQuoteMsg As Boolean
        Dim loriginalBaseInsfileCnt As Integer
        Dim bAutoRenewal As Boolean = False
        Dim vResultArray(,) As Object
        Dim bRiskEdited As Boolean

        'WPR 33-75 added commented as per WPR
        Dim lCount As Integer
        result = gPMConstants.PMEReturnCode.PMTrue


        lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the latest version of the risks before recalculating
        lReturn = uctPMUListRisk1.GetRisks
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            cmdBackdateMTA.Enabled = True
            RaiseError(kMethodName, "GetRisks Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        'WPR 33-75 end
        ''(Start) Saurabh Agrawal PN56398
        lReturn = m_oChangePolicyStatus.GetRisksByStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vRisks:=m_vRisks)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            cmdBackdateMTA.Enabled = True
            gPMFunctions.RaiseError(kMethodName, "GetRisksByStatus Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Information.IsArray(m_vRisks) Then
            MessageBox.Show("Cannot Proceed - There should be at least 1 risk", "Backdate MTA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            cmdBackdateMTA.Enabled = True
            Return result
        Else
            For lCount = 0 To UBound(m_vRisks, 2)
                If ToSafeLong(m_vRisks(kRiskIsSelected, lCount), 0) = 0 Then
                    MsgBox("Cannot Proceed - All risks must be selected.", vbExclamation, "Backdate MTA")
                    cmdBackdateMTA.Enabled = True
                    Return result
                End If
                If Trim(UCase(ToSafeString(m_vRisks(5, lCount), 0))) <> "U" Then
                    bRiskEdited = True
                End If
            Next lCount
        End If

        If uctPMUListRisk1.RiskCount > UBound(m_vRisks, 2) + 1 Then
            ' allow if some risk(s) are deleted as well
            bRiskEdited = True
        End If

        If bRiskEdited = False Then
            MsgBox("Cannot Proceed - There should be at least 1 risk edited.", vbExclamation, "Backdate MTA")
            cmdBackdateMTA.Enabled = True
            Return result
        End If
        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="out_of_sequence_MTA_UserGroup", r_vProductArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            cmdBackdateMTA.Enabled = True
            gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for UserGroup", gPMConstants.PMELogLevel.PMLogError)

        Else
            If Information.IsArray(vResultArray) Then
                m_vUserGroupId = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
                If m_vUserGroupId = 0 Then
                    MessageBox.Show("Please set up the User Group under Product Risk configuration. Contact your system administrator", "Backdate MTA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cmdMakeLive.Enabled = False
                    m_bMTABackdatedOK = False
                    cmdBackdateMTA.Enabled = True
                    Return result
                End If
            Else
                cmdBackdateMTA.Enabled = True
                gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for UserGroup", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If


        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="out_of_sequence_MTA_TaskGroup", r_vProductArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            cmdBackdateMTA.Enabled = True
            gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for TaskGroup", gPMConstants.PMELogLevel.PMLogError)
        Else
            If Information.IsArray(vResultArray) Then
                m_vTaskGroupId = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
                If m_vTaskGroupId = 0 Then
                    MessageBox.Show("Please set up the Task Group under Product Risk configuration. Contact your system administrator", "Backdate MTA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cmdMakeLive.Enabled = False
                    m_bMTABackdatedOK = False
                    cmdBackdateMTA.Enabled = True
                    Return result
                End If
            Else
                cmdBackdateMTA.Enabled = True
                gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for TaskGroup", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If
        m_bMTABackdatedOK = True

        m_dtEffectiveDate = m_dtCoverStartDate
        If Not m_bBackdatedQuoteExists OrElse IsArray(m_vBackdatedMTAVersions) = False Then
            lReturn = m_oAutoMTA.GetBackdatedPolicyVersions(m_lInsuranceFileCnt, m_vBackdatedMTAVersions)
            If lReturn <> PMEReturnCode.PMTrue Then
                cmdBackdateMTA.Enabled = True
                RaiseError(kMethodName, "GetBackdatedPolicyVersions Failed", PMELogLevel.PMLogError)
            End If
            If IsArray(m_vBackdatedMTAVersions) Then
                m_bBackdatedQuoteExists = True
            End If
        End If

        'WPR 33-75 added
        'Skip if we already have a quote
        If Not m_bBackdatedQuoteExists OrElse m_bIsPolicyEdited Then
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Try  ' Don't error if it fails
                m_oProgressBar = New iPMBProgressBarWrapper.Wrapper()

            Catch
            End Try

            If Not (m_oProgressBar Is Nothing) Then
                m_oProgressBar.Caption = "  Processing Backdated Quotes . . ."
                m_oProgressBar.Text = "It may take several minutes to process. Please wait..."
                m_oProgressBar.StartBar = True
            End If

            m_vBackdatedMTAVersions = Nothing

            If m_sTransactionType = "MTA" Then
                'WPR 33-75 added
                lReturn = m_oAutoMTA.SetProcessModes(vTransactionType:=m_sTransactionType)
                'WPR 33-75 added  bIsInteractive parameter added
                lReturn = m_oAutoMTA.QuoteMTA(v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_dtEffectiveDate:=m_dtEffectiveDate, lBaseInsuranceFileCnt:=m_lInsuranceFileCnt, r_sFailureMessage:=m_sFailureReason, vBackdatedMTAVersions:=m_vBackdatedMTAVersions, bUpdateStats:=False, r_bShowQuoteMsg:=bShowQuoteMsg, bIsInteractive:=True, bIsDirty:=True)

            ElseIf m_sTransactionType = "MTC" Then
                'retain current insurance file cnt
                loriginalBaseInsfileCnt = m_lInsuranceFileCnt

                lReturn = m_oAutoMTA.QuoteCancellation(v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_dtEffectiveDate:=m_dtEffectiveDate, r_lNewInsuranceFileCnt:=m_lInsuranceFileCnt, r_sFailureMessage:=m_sFailureReason, lBaseInsuranceFileCnt:=m_lInsuranceFileCnt, bUpdateStats:=False, bBackDateMTA:=True, vPolicyRef:=vPolicyRef, bIsDirty:=True) 'PN-71068
                'restore back original file cnt
                m_lInsuranceFileCnt = loriginalBaseInsfileCnt
            ElseIf m_sTransactionType = "MTR" Then
                'retain current insurance file cnt
                loriginalBaseInsfileCnt = m_lInsuranceFileCnt
                lReturn = m_oAutoMTA.SetProcessModes(vTransactionType:=m_sTransactionType)
                lReturn = m_oAutoMTA.QuoteReinstatement(nPartyCnt:=m_lPartyCnt,
                                                       nInsuranceFolderCnt:=m_lInsuranceFolderCnt,
                                                       dtEffectiveDate:=m_dtEffectiveDate,
                                                       nNewInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                       sFailureMessage:=m_sFailureReason,
                                                       nBaseInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                       bUpdateStats:=False,
                                                       bBackDateMTA:=True,
                                                       oPolicyRef:=vPolicyRef,
                                                       bIsDirty:=True)
                'restore back original file cnt
                m_lInsuranceFileCnt = loriginalBaseInsfileCnt
            End If
            If (m_oProgressBar Is Nothing) = False Then
                m_oProgressBar.StopBar = True
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            cmdBackdateMTA.Enabled = True
            gPMFunctions.RaiseError(kMethodName, "QuoteMTA Failed - " & m_sFailureReason, gPMConstants.PMELogLevel.PMLogError)
            m_bMTABackdatedOK = False
            Return result

        Else
            'WPR 33-75 added
            m_bBackdatedQuoteExists = True

            m_bMTABackdatedOK = True
            'load frm Backdate MTA
            oBackdateMTA = New frmBackdatedMTA()
            m_bIsPolicyEdited = False

            oBackdateMTA.oAutoMTA = m_oAutoMTA
            oBackdateMTA.InsuranceFileCnt = m_lInsuranceFileCnt

            'WPR 33-75 added
            oBackdateMTA.TransactionType = m_sTransactionType
            oBackdateMTA.InsuranceFolderCnt = m_lInsuranceFolderCnt
            oBackdateMTA.ProductID = m_lProductId
            oBackdateMTA.PartyCnt = m_lPartyCnt
            oBackdateMTA.Shortname = m_sShortName
            oBackdateMTA.SourceID = m_iSourceID
            oBackdateMTA.BackdatedMTAVersions = m_vBackdatedMTAVersions
            oBackdateMTA.ParentForm = Me
            oBackdateMTA.Business = m_oBusiness
            'WPR 33-75 END
            'Developer Guide No. 68

            oBackdateMTA.ShowDialog()
            NetMTAPremium = oBackdateMTA.TotalPremium
            'WPR 33-75 added
            m_bMTABackdatedOK = oBackdateMTA.AllQuoted



            oBackdateMTA.Close()
            oBackdateMTA = Nothing

            If m_bMTABackdatedOK Then
                If Not m_bIsMarketPlacePolicy Then
                    cmdMakeLive.Enabled = True
                End If
                'WPR 33-75 added commented as per WPR
                'cmdBackdateMTA.Enabled = False
            Else
                cmdMakeLive.Enabled = False
            End If
            cmdBackdateMTA.Enabled = True
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here

Finally_Renamed:

        Return result


        Return result
    End Function


    Private isInitializingComponent As Boolean
    Private Sub optBankGuarantee_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optBankGuarantee.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ToggleMarkForCollection(kMakeLiveOptionsBG)
        End If
    End Sub



    Private Sub optCashDeposit_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optCashDeposit.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ToggleMarkForCollection(kMakeLiveOptionsCD)
        End If
    End Sub

    Private Sub OptInstalments_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptInstalments.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SelectPaymentTerms(kPaymentTermsInstalments)
            ToggleMarkForCollection(kMakeLiveOptionsINST)
        End If
    End Sub

    Private Sub OptInvoice_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptInvoice.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SelectPaymentTerms(kPaymentTermsInvoice)
            ToggleMarkForCollection(kMakeLiveOptionsINVOICE)
        End If
    End Sub



    Private Sub optMarkForCollection_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optMarkForCollection.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            'WPR12- Enhancement Quote Collection Process
            ToggleMarkForCollection(kMakeLiveOptionsMARKED)
        End If
    End Sub

    Private Sub optPayNow_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optPayNow.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SSTabHelper.SetSelectedIndex(SSTab1, kRiskStatus)
            ToggleMarkForCollection(kMakeLiveOptionsPAYNOW)
        End If
    End Sub

    Private Sub OptPutMTAOnNextInstalment_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPutMTAOnNextInstalment.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SelectPaymentTerms(kPaymentTermsPutMTAOnNextInstalmentRenewal)
            ToggleMarkForCollection("OTHER")
        End If
    End Sub

    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.8.1.2)
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0

        Dim iLanguageId As Integer

        Const kMethodName As String = "DisplayCaptions"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.



            'SSTabHelper.SetTabCaption(SSTab1, 4, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(SSTab1, 4, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblCoverFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCoverFromDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblCoverFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCoverFromDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'lblExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACExpiryDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACExpiryDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'OptInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInstalments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            OptInstalments.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInstalments, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Private Sub uctPMUFees1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMUFees1.Change
        Recalculate(kRecalculateModeFees)
        m_bIsPolicyEdited = True
    End Sub

    Private Sub uctPMUListRisk1_AboutToChange(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMUListRisk1.AboutToChange
        If uctPMUListRisk1.DiscountedRiskCount > 0 Then
            PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonRiskSelectOrUnSelect)
        End If
    End Sub

    ' PW311002 - add the risk/variation number to the event parameters
    'WPR 33-75 added commented as per WPR
    Private Sub uctPMUListRisk1_lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As uctPMUListRisk.lvwSearchDetailsClickEventArgs) Handles uctPMUListRisk1.lvwSearchDetailsClick
        'WPR 33-75 added
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then

            If Not e.v_bSelected Then
                cmdEditRisk.Enabled = False
                cmdDeleteRisk.Enabled = False

            Else
                cmdEditRisk.Enabled = True
                cmdDeleteRisk.Enabled = False
            End If

        Else

            If Not e.v_bSelected Then
                cmdEditRisk.Enabled = False
                cmdCopyRisk.Enabled = False
                cmdDeleteRisk.Enabled = False
            Else
                If m_bBackdatedEditing = True Then
                    If e.v_bEditable Then
                        m_bEditable = True
                    Else
                        m_bEditable = False
                    End If

                    If m_bEditable = False AndAlso m_iTask <> PMEComponentAction.PMView Then
                        cmdEditRisk.Enabled = False
                    Else
                        If e.v_bSelected Then
                            cmdEditRisk.Enabled = True
                        End If
                    End If

                    If ToSafeString(e.v_sRiskStatus).ToUpper.Trim = "QUOTED" OrElse ToSafeString(e.v_sRiskStatus).ToUpper.Trim = "DELETED" Then
                        btnNOChange.Enabled = False
                    Else
                        btnNOChange.Enabled = True
                    End If
                    If uctPMUListRisk1.AllRiskStatusQuoted Then
                        btnNoChangeAll.Enabled = False
                    Else
                        btnNoChangeAll.Enabled = True
                    End If
                    cmdAddRisk.Enabled = False
                    cmdDeleteRisk.Enabled = False
                    cmdCopyRisk.Enabled = False

                    cmdPrintDocument.Enabled = False
                    cmdPrintProposal.Enabled = False
                    cmdPrintQuote.Enabled = False
                    cmdMakeLive.Enabled = False
                    cmdDocArchive.Enabled = False

                    OptInstalments.Enabled = False
                    OptInvoice.Enabled = False
                    optPayNow.Enabled = False
                    optBankGuarantee.Enabled = False
                    optCashDeposit.Enabled = False
                    optMarkForCollection.Enabled = False
                    ' if this isnt being called from deferred reinsurance
                ElseIf m_sTransactionType <> "DRI" Then

                    If Not uctPMUListRisk1.Deleted Then
                        cmdEditRisk.Enabled = True
                        cmdCopyRisk.Enabled = True
                    Else
                        cmdEditRisk.Enabled = False
                        cmdCopyRisk.Enabled = False
                    End If

                    'PN-29259  as incase of MTA variation number starts from 1.
                    If m_sTransactionType = "MTA" Then
                        cmdCopyRisk.Enabled = True
                    End If
                    'PSL 29/04/2003 Iss 3797 Put Delete back
                    If m_sTransactionType <> "MTC" Then
                        cmdDeleteRisk.Enabled = True
                        cmdCopyRisk.Enabled = True
                    End If
                Else
                    ' this process is being called from deferred reinsurance
                    cmdDeleteRisk.Enabled = False
                    cmdEditRisk.Enabled = True
                    cmdCopyRisk.Enabled = False
                    cmdAddRisk.Enabled = False
                End If
            End If

        End If

        ' Wpr53 : if v_bMandatoryRisk then disable cmdDeleteRisk and cmdCopyRisk button
        If ToSafeBoolean(e.v_bMandatoryRisk, False) = True Then
            cmdDeleteRisk.Enabled = False
            cmdCopyRisk.Enabled = False
            m_bMandatoryRisk = True
        Else
            m_bMandatoryRisk = False
        End If
        'WPR 33-75 ENDS
        m_lRiskId = e.v_lRiskID
        m_lScreenId = e.v_lScreenId
        m_lRiskTypeId = e.v_lRiskTypeId
        'PW311002 - store the risk and variation numbers
        m_lRiskNo = e.v_lRiskNo
        m_lVariationNo = e.v_lVariationNo

        If m_bIsMarketPlacePolicy Then
            cmdAddRisk.Enabled = False
            cmdDeleteRisk.Enabled = False
            cmdCopyRisk.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' GetInsuranceFileDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceFileDetails() As Integer

        Const kMethodName As String = "GetInsuranceFileDetails"
        Dim nResult As Integer
        Dim oInsuranceFileDetails As Object
        Dim oPaymentMethod As Object

        nResult = PMEReturnCode.PMTrue

        ' get insurance file details

        nResult = m_oBusiness.GetInsuranceFileDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lOriginalInsuranceFileCnt:=m_lOriginalInsuranceFileCnt, r_vResults:=oInsuranceFileDetails)

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetInsuranceFileDetails Failed", PMELogLevel.PMLogError)
        End If

        If Not IsArray(oInsuranceFileDetails) Then
            RaiseError(kMethodName, "GetInsuranceFileDetails returned no data", PMELogLevel.PMLogError)
        End If

        ' populate screen

        txtPolicyHolder.Text = CStr(oInsuranceFileDetails(4, 0))

        txtPolicyHolderFull.Text = CStr(oInsuranceFileDetails(6, 0))

        txtPolicyRef.Text = CStr(oInsuranceFileDetails(0, 0))

        Dim dtExpiryDate As DateTime = DateTime.MinValue
        DateTime.TryParse(CStr(oInsuranceFileDetails(3, 0)), dtExpiryDate)
        txtExpiryDate.Text = IIf(dtExpiryDate.Equals(DateTime.MinValue), String.Empty, dtExpiryDate.ToShortDateString)

        Dim dtInceptionDate As DateTime = DateTime.MinValue
        DateTime.TryParse(CStr(oInsuranceFileDetails(1, 0)), dtInceptionDate)
        txtInceptionDate.Text = IIf(dtInceptionDate.Equals(DateTime.MinValue), String.Empty, dtInceptionDate.ToShortDateString)

        Dim dtCoverFromDate As DateTime = DateTime.MinValue
        DateTime.TryParse(CStr(oInsuranceFileDetails(2, 0)), dtCoverFromDate)
        txtCoverFromDate.Text = IIf(dtCoverFromDate.Equals(DateTime.MinValue), String.Empty, dtCoverFromDate.ToShortDateString)

        txtAgent.Text = CStr(oInsuranceFileDetails(9, 0))

        txtCurrency.Text = CStr(oInsuranceFileDetails(10, 0))

        ' hide the agent field if there is no value
        If txtAgent.Text = "" Then
            txtAgent.Visible = False
            lblAgent.Visible = False
        End If

        m_lPartyCnt = ToSafeLong(oInsuranceFileDetails(23, 0), 0)

        m_lLeadAgentCnt = ToSafeLong(oInsuranceFileDetails(25, 0), 0)
        m_bInsuranceFileOnInstalments = ToSafeBoolean(oInsuranceFileDetails(11, 0), 0)

        m_sBusinessType = CStr(oInsuranceFileDetails(12, 0))

        m_sAgentType = CStr(oInsuranceFileDetails(13, 0))

        m_sAgentShortCode = CStr(oInsuranceFileDetails(7, 0))

        If CStr(oInsuranceFileDetails(14, 0)) = "" Then
            m_lSubAgentCount = 0
        Else

            m_lSubAgentCount = CInt(oInsuranceFileDetails(14, 0))
        End If

        m_bIsTrueMonthlyPolicy = ToSafeBoolean(CInt(oInsuranceFileDetails(kInsFileDetailsProductIsTrueMonthlyPolicy, 0)) = 1, False)

        m_bPutOnNextInstalmentRenewal = ToSafeBoolean(CInt(oInsuranceFileDetails(kInsFileDetailsPutOnNextInstalmentRenewal, 0)) = 1, False)
        m_lDiscountReasonId = ToSafeLong(oInsuranceFileDetails(kInsFileDetailsDiscountReasonId, 0), 0)
        m_dDiscountPercentage = ToSafeDouble(oInsuranceFileDetails(kInsFileDetailsDiscountPercentage, 0), 0)
        m_crDiscountedPremium = ToSafeCurrency(oInsuranceFileDetails(kInsFileDetailsDiscountedPremium, 0), 0)
        m_lProductId = ToSafeLong(oInsuranceFileDetails(kInsFileDetailsProductId, 0), 0)
        m_lMatchDiscountedPremium = ToSafeLong(oInsuranceFileDetails(kInsFileDetailsMatchDiscountedPremium, 0), 0)
        m_dtAnniversaryDate = ToSafeDate(oInsuranceFileDetails(kInsFileDetailsAnniversaryDate, 0), DateTime.Today)

        m_dtCoverStartDate = CDate(oInsuranceFileDetails(2, 0))

        m_lCurrencyId = CInt(oInsuranceFileDetails(33, 0))

        m_sAgentName = CStr(oInsuranceFileDetails(9, 0))

        'Start Written Status
        m_bWrittenStatusPermitted = ToSafeBoolean(oInsuranceFileDetails(kInsFileDetailsWrittenStatusPermitted, 0))
        m_sInsuranceFileStatusCode = ToSafeString(oInsuranceFileDetails(kInsFileDetailsInsuranceFileStatusCode, 0))
        m_iWrittenTaskManagerDays = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsWrittenTaskManagerDays, 0))
        m_iWrittenRemUserGroup = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsWrittenRemUserGroup, 0))
        m_iWrittenRemTaskGroup = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsWrittenRemTaskGroup, 0))
        'End Written Status-
        m_bIsMarketPlacePolicy = ToSafeBoolean(oInsuranceFileDetails(kInsFileDetailsIsMarketPlacePolicy, 0))
        'Get the payment method of the original policy

        nResult = m_oBusiness.GetPaymentMethod(m_lInsuranceFileCnt, oPaymentMethod)

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetInsuranceFileDetails Failed", PMELogLevel.PMLogError)
        End If

        If IsArray(oPaymentMethod) And (m_dtCoverStartDate <= m_dtAnniversaryDate) Then

            m_sPaymentTerms = CStr(oPaymentMethod(0, 0))
        Else
            m_sPaymentTerms = ""
        End If

        m_iInsuranceFileTypeID = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsOnsFileTypeID, 0), 0)
        m_dtInceptionDateTPI = ToSafeDate(oInsuranceFileDetails(kInsFileDetailsInceptionTPI, 0), Date.MinValue)
        m_nInsuranceFileStatusID = ToSafeInteger(oInsuranceFileDetails(39, 0), 0)
        m_iTransCurrencyID = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsTransCurrencyID, 0))
        m_iBaseCurrencyID = ToSafeInteger(oInsuranceFileDetails(kInsFileDetailsBaseCurrencyID, 0))
        m_sTransISOCode = ToSafeString(oInsuranceFileDetails(kInsFileDetailsTransISOCode, 0))
        m_sBaseISOCode = ToSafeString(oInsuranceFileDetails(kInsFileDetailsBaeISOCode, 0))

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: SetupFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFees"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Developer Guide No. 9
            uctPMUFees1.Initialise()

            uctPMUFees1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate, m_sPaymentTerms, m_sPaymentCashOrDebit)

            uctPMUFees1.InsuranceFileCnt = m_lInsuranceFileCnt

            ' if we are viewing the initial values should be loaded.
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                uctPMUFees1.ReadOnly_Renamed = True
                uctPMUFees1.Load_Renamed()
                ' for all other modes the initial values should be recalculated
            Else
                uctPMUFees1.Recalculate()
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupRITaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupRITaxes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupRITaxes"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            ' Developer Guide No. 9
            uctPMURITax1.Initialise()
            uctPMURITax1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)
            uctPMURITax1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctPMURITax1.ApplyMTATaxRatesonRen = m_sApplyMTATaxRatesonRen


            ' if we are viewing the initial values should be loaded.
            If m_iTask = gPMConstants.PMEComponentAction.PMView OrElse m_bBackdatedEditing Then
                uctPMURITax1.ReadOnly_Renamed = True
                uctPMURITax1.Load_Renamed()
                ' for all other modes the initial values should be recalculated
            Else
                uctPMURITax1.Recalculate()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateTotals() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTotals"

        Dim crNetTotal, crFeeTotal, crTaxTotal, crGrossTotal As Decimal



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Risk Tax Totals

            m_lReturn = m_oRITax.GetTaxesTotalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, vArray:=m_vArray)

            'Risk Level Taxes
            txtTEFF.Text = StringsHelper.Format(m_vArray(2, 0), "0.00")
            txtTEF.Text = StringsHelper.Format(m_vArray(4, 0), "0.00")

            txtTotalNonTax.Text = StringsHelper.Format(m_vArray(10, 0), "0.00")
            txtTotalRiskTax.Text = StringsHelper.Format(m_vArray(8, 0), "0.00") 'Format(CLng(m_vArray(4, 0)) + CLng(m_vArray(6, 0)), "0.00")
            'Policy Level taxes
            txtPTEFF.Text = StringsHelper.Format(m_vArray(3, 0), "0.00")
            txtPTEXF.Text = StringsHelper.Format(m_vArray(5, 0), "0.00")


            txtPNCT.Text = StringsHelper.Format(m_vArray(11, 0), "0.00")
            txtPCT.Text = StringsHelper.Format(m_vArray(9, 0), "0.00") 'Format(CLng(m_vArray(5, 0)) + CLng(m_vArray(7, 0)), "0.00")
            'access control

            'Policy fee Totals


            m_lReturn = m_oPartyFee.GetFeesTotalDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, vArray:=m_vArray1)
            'Risk level Fees
            txtPFF.Text = StringsHelper.Format(m_vArray1(2, 0), "0.00")
            txtPEXF.Text = StringsHelper.Format(m_vArray1(4, 0), "0.00")
            txtPFNCT.Text = StringsHelper.Format(CDec(m_vArray1(2, 0)) + CDec(m_vArray1(4, 0)), "0.00")
            'Policy level Fees
            txtPFEFF.Text = StringsHelper.Format(m_vArray1(3, 0), "0.00")
            txtPFEXFF.Text = StringsHelper.Format(m_vArray1(5, 0), "0.00")
            txtPFNCTT.Text = StringsHelper.Format(CDec(m_vArray1(3, 0)) + CDec(m_vArray1(5, 0)), "0.00")


            'Developer Guide No 248. 
            crFeeTotal = uctPMUListRisk1.TotalFeePremium + IIf(Conversion.Val(uctPMUFees1.TotalFees) = Conversion.Val(""), 0.0, uctPMUFees1.TotalFees)

            crTaxTotal = uctPMUListRisk1.TotalTax + uctPMUListRisk1.TotalFeeTax + uctPMUFees1.TotalTax + uctPMURITax1.TotalTax

            crNetTotal = uctPMUListRisk1.TotalPremium

            crGrossTotal = crFeeTotal + crTaxTotal + crNetTotal

            txtNettotal.Text = StringsHelper.Format(crNetTotal, "0.00")
            txtFeeTotal.Text = StringsHelper.Format(crFeeTotal, "0.00")
            txtTaxTotal.Text = StringsHelper.Format(crTaxTotal, "0.00")
            ' take rounded amound to display gross
            txtGrossTotal.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(txtNettotal.Text.Trim) + gPMFunctions.ToSafeCurrency(txtFeeTotal.Text.Trim) + gPMFunctions.ToSafeCurrency(txtTaxTotal.Text.Trim), "0.00")

            'setup control
            uctInstalments1.Initialise_Control()
            uctInstalments1.Product_Code = m_sInstalmentProductCode
            If m_sInstalmentProductCode = "REN" Then
                uctInstalments1.InsuranceFileCnt_Renewal = m_lInsuranceFileCnt
            Else
                uctInstalments1.InsuranceFileCnt = m_lInsuranceFileCnt
            End If

            uctInstalments1.EffectiveDate = m_dtCoverStartDate

            uctInstalments1.Task = m_iTask
            uctInstalments1.TransactionType = m_sTransactionType

            uctInstalments1.FeeExcluded = gPMFunctions.ToSafeCurrency(txtPEXF.Text, 0) + gPMFunctions.ToSafeCurrency(txtPFEXFF.Text, 0)
            uctInstalments1.TaxExcluded = gPMFunctions.ToSafeCurrency(txtTEF.Text, 0) + gPMFunctions.ToSafeCurrency(txtPTEXF.Text, 0)
            uctInstalments1.TransCurrencyID = m_iTransCurrencyID
            uctInstalments1.BaseCurrencyID = m_iBaseCurrencyID
            uctInstalments1.TransISOCode = m_sTransISOCode
            uctInstalments1.BaseISOCode = m_sBaseISOCode
            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
            If m_bRoundOff Then
                If crGrossTotal <> 0 Then
                    m_cRoundOffAmount = gPMMaths.PMRoundupValueCurrency(crGrossTotal, gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero, gPMConstants.PMERoundupFactor.pmeRFactor50Up) - crGrossTotal
                Else
                    m_cRoundOffAmount = 0
                End If
                txtGrossRoundTotal.Text = StringsHelper.Format(crNetTotal + crFeeTotal + crTaxTotal + m_cRoundOffAmount, "0.00")
                txtRoundAmt.Text = StringsHelper.Format(m_cRoundOffAmount, "0.00")
            Else
                m_cRoundOffAmount = 0
                txtGrossRoundTotal.Text = StringsHelper.Format(0, "0.00")
                txtRoundAmt.Text = StringsHelper.Format(0, "0.00")
            End If
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

            If m_sInstalmentProductCode.ToUpper() = "REN" And m_iTask = gPMConstants.PMEComponentAction.PMEdit And m_bIsTrueMonthlyPolicy Then
                'Manual renewal review
                uctInstalments1.PopulateGrossDue()
            Else
                'Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
                uctInstalments1.GrossDue = Math.Round(crNetTotal + crFeeTotal + crTaxTotal + m_cRoundOffAmount, 2) 'ToSafeCurrency(txtGrossTotal.Text, 0)
            End If
            uctInstalments1.FeeDeposit = gPMFunctions.ToSafeCurrency(CDec(m_vArray1(6, 0)) + CDec(m_vArray1(7, 0)), CDec("0.00"))
            uctInstalments1.TaxDeposit = gPMFunctions.ToSafeCurrency(CDec(m_vArray(6, 0)) + CDec(m_vArray(7, 0)), CDec("0.00"))
            uctInstalments1.LoadCurrencyInfo()

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_lRecalculateMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function Recalculate(ByVal v_lRecalculateMode As Integer) As Integer
        Dim nResult As Integer = 0
        Const kMethodName As String = "Recalculate"
        Try
            nResult = Recalculate(v_lRecalculateMode, 0)
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recalculate fees", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function


    ''' <summary>
    ''' Recalculate fees, tax
    ''' </summary>
    ''' <param name="v_lRecalculateMode"></param>
    ''' <param name="v_lLevel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function Recalculate(ByVal v_lRecalculateMode As Integer,
                                Optional ByVal v_lLevel As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "Recalculate"

        Try

            nResult = PMEReturnCode.PMTrue
            If m_iTask = PMEComponentAction.PMView Then
                Return nResult
            End If


            If m_bCanReRateAllRisks Then
                nResult = CanAutoQuoteRisks(r_bCanAutoQuoteRisks:=m_bCanAutoQuoteRisks)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CanAutoQuoteRisks Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If m_bCanAutoQuoteRisks Then
                    cmdQuoteAllRisks.Enabled = True
                Else
                    cmdQuoteAllRisks.Enabled = False
                End If
            Else
                cmdQuoteAllRisks.Enabled = False
            End If
            v_lLevel += 1

            If v_lLevel = 1 Then
                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                StatusBar.Items.Item(0).Text = "Recalculating..."
                StatusBar.Refresh()

            End If

            Select Case v_lRecalculateMode
                Case kRecalculateModeRisks

                    ' update the policy premium now as the premium totals will be based
                    ' on the selected risks so any change in the risks selection status
                    ' or values will have an impact on the policy premium
                    nResult = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt),
                        gPMConstants.PMEReturnCode)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' recalculate fees
                    nResult = uctPMURITax1.Recalculate()

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate Tax Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' call next step in the recalculation routine
                    nResult = CType(Recalculate(kRecalculateModeRITax, v_lLevel), gPMConstants.PMEReturnCode)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate " & kRecalculateModeRITax & " Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case kRecalculateModeFees
                    If m_sAgentType.Trim().ToUpper() <> "" Or m_lSubAgentCount > 0 Then
                        nResult = Commission1.Recalculate()
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Commission Recalculate Failed",
                                                    gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    nResult = CType(Recalculate(kRecalculateModeAgentCommission, v_lLevel),
                  gPMConstants.PMEReturnCode)

                    ' call next step in the recalculation routine
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate " &
                                                kRecalculateModeAgentCommission & " Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If


                Case kRecalculateModeRITax
                    ' only recalculate agent commission if there the
                    ' tab is enabled
                    nResult = uctPMUFees1.Recalculate()
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate Fees Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If
                    ' call next step in the recalculation routine
                    nResult = CType(Recalculate(kRecalculateModeFees, v_lLevel),
                        gPMConstants.PMEReturnCode)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate " &
                                                kRecalculateModeFees & " Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case kRecalculateModeAgentCommission
                    If m_sAgentType.Trim().ToUpper() <> "" Or m_lSubAgentCount > 0 Then
                        Commission1.InsuranceFileCnt = m_lInsuranceFileCnt
                        Commission1.GrossTotal = uctPMUListRisk1.TotalFeePremium +
                            uctPMUFees1.TotalFees + uctPMUListRisk1.TotalTax + uctPMUListRisk1.TotalFeeTax +
                            uctPMUFees1.TotalTax + uctPMURITax1.TotalTax + uctPMUListRisk1.TotalPremium
                    End If

                    ' call next step in the recalculation routine
                    nResult = CType(Recalculate(kRecalculateModeInstalments, v_lLevel),
                        gPMConstants.PMEReturnCode)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate " &
                                                kRecalculateModeInstalments & " Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case kRecalculateModeInstalments

                    ' get the new totals
                    nResult = PopulateTotals()
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If m_bIsBackdatedMTARequired = False And m_bBackdatedEditing = False Then
                        ' recalculate the instalments based on the
                        ' new totals....
                        nResult = CType(SetupInstalments(), gPMConstants.PMEReturnCode)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "RecalculateInstalments Failed",
                                                    gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
            End Select

            If v_lLevel = 1 Then
                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                StatusBar.Items.Item(0).Text = ""
                StatusBar.Refresh()
            End If

            'Backdated MTA
            If m_bMTABackdatedOK Then
                cmdMakeLive.Enabled = False
                cmdBackdateMTA.Enabled = True
            End If
            If m_bIsBackdatedMTARequired = False And m_bBackdatedEditing = False Then

                nResult = CType(CheckRiskStatusMarkForCollection(), gPMConstants.PMEReturnCode)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CheckRiskStatusMarkForCollection Failed",
                                            gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recalculate fees", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupControl() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupControl"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = SetupPaymentTerms()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPaymentTerms Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            SSTabHelper.SetSelectedIndex(SSTab1, m_iSpecifiedTab)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' SetupPaymentTerms
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupPaymentTerms() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SetupPaymentTerms"
        Dim bInvoiceEnabled As Boolean
        Dim bBankGuaranteeEnabled As Boolean
        Dim bInstalmentsEnabled As Boolean
        Dim bCashDepositEnabled As Boolean
        Dim bPayNowEnabled As Boolean
        Dim rPaymentType As Object

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            Select Case m_sTransactionType
                Case "MTA", "MTC", "EDIT"

                    ' by default instalments is not an option
                    m_bPaymentByInstalmentsAllowed = False

                    ' if the policy version is already on a live instalment plan
                    ' then allow it to continue on the existing instalment plan
                    If m_bInsuranceFileOnInstalments Then
                        m_bPaymentByInstalmentsAllowed = True
                    End If

                    ' unless this is a true monthly policy in which case
                    ' disable instalments
                    If m_bIsTrueMonthlyPolicy Then
                        m_bPaymentByInstalmentsAllowed = False
                    End If

                    'allow user to change payment method if any changes are being done on anniversry dt
                    If m_dtCoverStartDate = m_dtAnniversaryDate Then
                        m_bPaymentByInstalmentsAllowed = True
                    End If

                Case "NB", "REN"

                    ' payment by instalments is not allowed if the
                    ' business type is agency business....
                    m_bPaymentByInstalmentsAllowed = True

            End Select

            ' if the user can pay by instalments
            If m_bPaymentByInstalmentsAllowed And Not m_bRoadmapDisablesInstalments Then
                nResult = m_oBusiness.GetPaymentTerms(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPMUserID:=g_iUserID, r_bInvoiceEnabled:=bInvoiceEnabled, r_bInstalmentsEnabled:=bInstalmentsEnabled, r_bPayNowEnabled:=bPayNowEnabled, r_bBankGuaranteeEnabled:=bBankGuaranteeEnabled, r_bCashDepositEnabled:=bCashDepositEnabled)
                ' enable the instalment controls
                If bInstalmentsEnabled Then
                    OptInstalments.Enabled = True
                    uctInstalments1.IsPlanSelected = True
                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, True)
                End If
                Select Case m_sTransactionType
                    Case "MTA"

                        ' if the policy is on a based on a true monthly product
                        If m_bIsTrueMonthlyPolicy Then

                            ' disable the instalment option
                            OptInstalments.Visible = False

                            ' enable the put on next instalment renewal option
                            OptPutMTAOnNextInstalment.Visible = True

                            ' if the policy is already set to "put on next instalment renewal"then
                            If m_bPutOnNextInstalmentRenewal Then

                                ' default to put mta on next instalment
                                OptPutMTAOnNextInstalment.Checked = True
                            Else
                                ' otherwise default to invoice
                                OptInvoice.Checked = True

                            End If

                            If m_dtCoverStartDate = m_dtAnniversaryDate Then

                                nResult = m_oBusiness.GetPaymentType(m_lProductId, rPaymentType)
                                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "GetPayment Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                If CStr(rPaymentType(0, 0)) = "Invoice" Then
                                    OptInvoice.Checked = True
                                    m_sPaymentTerms = "Invoice"
                                ElseIf CStr(rPaymentType(0, 0)) = "Instalments" Then
                                    OptInstalments.Enabled = True
                                    OptInstalments.Checked = True
                                End If
                            End If

                        Else

                            ' if the insurance file is already on instalments
                            ' then select instalments as the default option
                            If m_bInsuranceFileOnInstalments Then
                                OptInstalments.Checked = True
                            Else
                                ' otherwise select pay by invoice as the default
                                OptInvoice.Checked = True
                            End If

                        End If

                    Case "MTC", "EDIT"

                        ' if the insurance file is already on instalments
                        ' then select instalments as the default option
                        If m_bInsuranceFileOnInstalments Then
                            OptInstalments.Checked = True
                        Else
                            ' otherwise select pay by invoice as the default
                            OptInvoice.Checked = True
                        End If

                    Case "NB", "REN"

                        nResult = m_oBusiness.GetPaymentType(m_lProductId, rPaymentType)
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetPayment Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If m_sTransactionType = "NB" Then
                            If CStr(rPaymentType(0, 0)) = "" Then
                                OptInvoice.Checked = True
                                m_sPaymentTerms = "Invoice"
                            End If
                        End If
                        If m_bIsTrueMonthlyPolicy Then

                            If CStr(rPaymentType(0, 0)) = "Invoice" Then
                                OptInvoice.Checked = True
                                m_sPaymentTerms = "Invoice"
                            ElseIf CStr(rPaymentType(0, 0)) = "Instalments" Then
                                OptInstalments.Enabled = True
                                OptInstalments.Checked = True
                            End If
                        End If

                End Select

            Else

                ' the user cant pay by instalments so disable the tab
                ' and the option
                If m_bIsTrueMonthlyPolicy Then
                    If m_sPaymentTerms = "Invoice" Then
                        OptInvoice.Checked = True
                        OptPutMTAOnNextInstalment.Enabled = False
                        OptInstalments.Visible = False
                    Else
                        If Not m_bPutOnNextInstalmentRenewal Then
                            OptPutMTAOnNextInstalment.Enabled = False
                            OptPutMTAOnNextInstalment.Checked = True
                        Else
                            OptInvoice.Enabled = False
                            OptPutMTAOnNextInstalment.Checked = True
                        End If

                    End If
                Else

                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, False)
                    OptInstalments.Enabled = False
                    If m_sPaymentTerms = "Credit Card" OrElse m_sPaymentTerms = "Direct Debit" OrElse m_sPaymentTerms = "Instalments" Then
                        OptInstalments.Checked = True
                        SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, True)
                        OptInstalments.Enabled = True
                    ElseIf m_sPaymentTerms = "PayNow" Then
                        optPayNow.Checked = True
                    ElseIf m_sPaymentTerms = "BankGuarantee" Then
                        optBankGuarantee.Checked = True
                    ElseIf m_sPaymentTerms = "CashDeposit" Then
                        optCashDeposit.Checked = True
                    Else
                        OptInvoice.Checked = True
                    End If

                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try

        Return nResult
    End Function

    Private Sub uctPMUListRisk1_RiskItemCheckChanged(ByVal Sender As Object, ByVal e As uctPMUListRiskControl.uctPMUListRisk.RiskItemCheckChangedEventArgs) Handles uctPMUListRisk1.RiskItemCheckChanged

        ProcessRiskItemCheckChanged(v_lRiskID:=e.RiskID, v_lCheckStatus:=e.CheckStatus, lRiskFolderID:=0)

        If uctPMUListRisk1.SelectedRiskCount = 0 And cmdApplyDiscount.Enabled Then
            cmdApplyDiscount.Enabled = False
        ElseIf uctPMUListRisk1.SelectedRiskCount > 0 And m_lDiscountReasonId <> 0 Then
            cmdApplyDiscount.Enabled = True
        End If

        'WPR12- Enhancement Quote Collection Process
        m_lReturn = EnableDisableMarkForCollection()
        ' Sasria
        m_lReturn = m_oBusiness.UnquoteMandatoryRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                     v_lRiskId:=RiskId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Business must have logged it.
        Else 'ToDo: Return flag from above statement if a risk got unquoted and then only call below one
            m_lReturn = uctPMUListRisk1.GetRisks
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("RiskItemCheckChanged", "GetRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get return properties
            m_vOriginalSelStatus = uctPMUListRisk1.RiskSelectionStatus
        End If
        If uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired Then
            cmdBackdateMTA.Enabled = True
            m_bIsPolicyEdited = True
        Else
            cmdBackdateMTA.Enabled = False
        End If

        'Defect 41718: QBE (Pacific) - Selecting instalment plan at Make Live
        If OptInstalments.Checked = True AndAlso uctPMUListRisk1.SelectedRiskCount = uctPMUListRisk1.RiskCount Then
            SelectPaymentTerms(kPaymentTermsInstalments)
            ToggleMarkForCollection(kMakeLiveOptionsINST)
        End If

    End Sub

    Private Sub uctPMURITax1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMURITax1.Change
        Recalculate(kRecalculateModeRITax)
        m_bIsPolicyEdited = True
    End Sub

    ' ***************************************************************** '
    ' Name: SetupCommission
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupCommission() As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "SetupCommission"

        Dim lReturn As Integer

        Dim oAgentCommission As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' only if there is an associated commission account
            ' show we show the agent commission screen.....
            If m_sAgentType.Trim().ToUpper() <> "" Or m_lSubAgentCount > 0 Then
                ' Developer Guide No. 9
                Commission1.Initialise()
                Commission1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)
                Commission1.InsuranceFileCnt = m_lInsuranceFileCnt

                ' if we are viewing the initial values should be loaded.
                If m_iTask = gPMConstants.PMEComponentAction.PMView OrElse m_bBackdatedEditing = True Then
                    Commission1.GrossTotal = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text, 0)
                    Commission1.ReadOnly_Renamed = True
                    Commission1.Load_Renamed()
                    ' for all other modes the initial values should be recalculated
                Else

                    Commission1.Recalculate()

                End If

                SSTabHelper.SetTabEnabled(SSTab1, kTabAgentCommission, True)

            Else
                Dim temp_oAgentCommission As Object
                lReturn = g_oObjectManager.GetInstance(temp_oAgentCommission, "bSirAgentCommission.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oAgentCommission = temp_oAgentCommission
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRAgentCommission.Business", gPMConstants.PMELogLevel.PMLogError)
                End If


                lReturn = oAgentCommission.DeleteAgentCommission(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DeleteAgentCommission Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                oAgentCommission.Dispose()
                oAgentCommission = Nothing

                SSTabHelper.SetTabEnabled(SSTab1, kTabAgentCommission, False)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' SetupInstalments
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupInstalments() As Integer

        Const kMethodName As String = "SetupInstalments"
        Dim nResult As Integer
        Dim bEnablePutOnNextInstalment As Boolean = False
        Dim crTaxNotApplied As Decimal

        Try
            nResult = PMEReturnCode.PMTrue
            bEnablePutOnNextInstalment = (m_sTransactionType = "MTA" And m_bIsTrueMonthlyPolicy)
            uctInstalments1.IsTrueMonthlypolicyandNextInstalmentRenewal = bEnablePutOnNextInstalment

            If uctPMUListRisk1.TotalNoSelectedQuotedRisks > 0 OrElse ToSafeCurrency(txtGrossTotal.Text, 0) > 0 Then
                ' amount is less than or equal to zero then disable instalments
                'Get the total amount of tax which is not applicable
                nResult = m_oRITax.GetTaxNotAppliedToClient(m_lInsuranceFileCnt, crTaxNotApplied)
                If nResult <> PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", " + +", Call to GetTaxNotAppliedToClient failed.")
                End If

                'Set up the control


                uctInstalments1.Amount = gPMFunctions.ToSafeCurrency(uctInstalments1.GrossDue - (uctInstalments1.FeeExcluded + uctInstalments1.TaxExcluded), 0)
                uctInstalments1.Product_Code = m_sInstalmentProductCode

                uctInstalments1.Initialise_Control()
                uctInstalments1.Task = m_iTask
                ' Send the original InsuranceFileCnt
                If m_sTransactionType = "MTC" AndAlso m_bIsSingleInstalmentPlan Then
                    uctInstalments1.InsuranceFileCnt_Renewal = m_lInsuranceFileCnt
                    uctInstalments1.InsuranceFileCnt = m_lOriginalInsuranceFileCnt
                ElseIf (m_sTransactionType = "MTA") Then
                    uctInstalments1.InsuranceFileCnt_Renewal = m_lInsuranceFileCnt
                    uctInstalments1.InsuranceFileCnt = m_lOriginalInsuranceFileCnt
                Else
                    uctInstalments1.InsuranceFileCnt = m_lInsuranceFileCnt
                End If
                'If m_sTransactionType = "MTC" And m_bIsSingleInstalmentPlan Then
                uctInstalments1.TransactionType = m_sTransactionType
                'End If

                uctInstalments1.Refresh()
            Else
                If Not m_bIsTrueMonthlyPolicy AndAlso m_sTransactionType = "NB" Then
                    OptInstalments.Enabled = False
                End If
            End If
            If bEnablePutOnNextInstalment Then
                If m_sPaymentTerms <> "Invoice" Then
                    OptPutMTAOnNextInstalment.Enabled = True
                    OptInstalments.Visible = False
                    OptPutMTAOnNextInstalment.Visible = True
                End If
            Else
                OptInstalments.Enabled = True
                OptInstalments.Visible = True
                OptPutMTAOnNextInstalment.Visible = False
            End If
            Call EnableDisblePaymentOptions()
            ' if instalments is not enabled then
            If (OptInstalments.Enabled = False OrElse OptPutMTAOnNextInstalment.Enabled = False) _
                AndAlso m_bIsTrueMonthlyPolicy = False Then

                OptInstalments.Enabled = False
                OptPutMTAOnNextInstalment.Enabled = False

            End If
            'Float Balance and Pre-Payment (RC)
            SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, OptInstalments.Enabled)
            Return nResult
        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Installments", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
            Return nResult
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ProcessRiskItemCheckChanged
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'WPR 33-75 added
    'Private Function ProcessRiskItemCheckChanged(ByVal v_lRiskID As Integer, ByVal v_lCheckStatus As Integer) As Integer
    'WPR 33-75 added
    Private Function ProcessRiskItemCheckChanged(ByVal v_lRiskID As Integer, ByVal v_lCheckStatus As Integer, Optional ByVal lRiskFolderID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRiskItemCheckChanged"

        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' update the risk selection status

            lReturn = m_oBusiness.UpdateRiskSelection(v_lRiskID, v_lCheckStatus)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessRiskItemCheckChanged", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' call the recalculate routines....
            lReturn = CType(Recalculate(kRecalculateModeRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectPaymentTerms
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SelectPaymentTerms(ByVal v_lOption As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectPaymentTerms"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ensure we have setup instalments
            lReturn = SetupInstalments()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupInstalments Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_lOption = kPaymentTermsInvoice Then

                ' show the risk tab
                SSTabHelper.SetSelectedIndex(SSTab1, kTabRisk)
                If m_iSpecifiedTab = 0 Then
                    m_iSpecifiedTab = kTabRisk
                Else
                    m_iSpecifiedTab = m_iSpecifiedTab
                End If


            ElseIf v_lOption = kPaymentTermsInstalments Or v_lOption = kPaymentTermsPutMTAOnNextInstalmentRenewal Then

                ' show the instalments tab
                If uctPMUListRisk1.SelectedRiskCount > 0 AndAlso m_sTransactionType <> "" AndAlso Not (m_bFormLoading) Then
                    SSTabHelper.SetSelectedIndex(SSTab1, kTabInstalments)
                    m_iSpecifiedTab = kTabInstalments
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPaymentTerms
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ProcessPaymentTerms() As Integer
        Dim result As Integer = 0

        Dim oPayNow As iPMUPayNowOptions.Interface_Renamed

        Dim oCashDeposit As iSIRPolicyCashDeposit.Interface_Renamed

        Dim oBankGuarantee As iSIRPolicyBankGuarantee.Interface_Renamed

        Const kMethodName As String = "ProcessPaymentTerms"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bByPassPaynow As Boolean 'ByPass the PayNow
        Dim vResultArray(,) As Object
        Dim m_sAgentType As String = ""
        Dim oPrepayment(,) As Object = Nothing

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        'Ashwani - (RFC_Enable_PrePayment_functionality)
        m_lReturn = m_oBusiness.GetPrePaymentOptionValue(m_lProductId, oPrepayment)

        m_lReturn = m_oBusiness.GetAgentType(m_lInsuranceFileCnt, vResultArray)

        If Information.IsArray(vResultArray) Then

            m_sAgentType = CStr(vResultArray(0, 0)).Trim()
        End If

        If (OptInvoice.Checked Or OptInstalments.Checked) And m_sTransactionType = "NB" Then
            Dim oParty As bSIRParty.Business
            Dim temp_oParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oParty, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oParty = temp_oParty
            If (oPrepayment(0, 0) = "1" AndAlso m_sPaymentTerms = "Invoice") Then

            Else
                If m_sAgentType = "Intermed" Then
                    Dim oSelectAccount As frmSelectAccount
                    Dim bSelectAgent As Boolean
                    oSelectAccount = New frmSelectAccount()

                    ' display copy form
                    oSelectAccount.ShowDialog()

                    ' if copy cancelled
                    If Not oSelectAccount.OK Then
                        oSelectAccount.Close()
                        oSelectAccount = Nothing
                        Return gPMConstants.PMEReturnCode.PMCancel
                    Else
                        ' determine what copy type to perform
                        bSelectAgent = oSelectAccount.SelectAgent
                        oSelectAccount.Close()
                        oSelectAccount = Nothing
                    End If
                    If bSelectAgent = True Then
                        'MsgBox("Agent")
                        lReturn = oParty.GetAccountID(vPartyRef:=m_sAgentShortCode, vAccountID:=m_lPaymentAccountID)
                    Else
                        'MsgBox("Client")
                        lReturn = oParty.GetAccountID(vPartyRef:=m_sShortName, vAccountID:=m_lPaymentAccountID)
                    End If
                End If
            End If
        End If
        If m_sAgentType = "Intermed" And m_sTransactionType <> "NB" Then
            m_lReturn = m_oBusiness.GetTransNBAccountId(m_lInsuranceFolderCnt, vResultArray)
            If Information.IsArray(vResultArray) AndAlso vResultArray(0, 0).ToString() <> "" Then
                m_lPaymentAccountID = CInt(vResultArray(0, 0))
            End If
        End If
        Dim cGrossTotal, m_cChargeablePremium As Decimal

        Dim cLeadAgentCommission As Decimal
        If OptInstalments.Checked Then
            If m_sTransactionType = "MTA" And m_bIsBackdatedMTARequired Then
                'Backdated MTA won't transact instalments
                m_sPaymentTerms = "Instalments"
            Else
                If m_sTransactionType = "MTA" Then
                    If m_bIsNegativeInstallents AndAlso uctInstalments1.FirstInstalmentAmt < 0 Then
                        MessageBox.Show("Can't proceed with negative Installments. Select Payment method other then 'Instalment' or 'Bank Guarantee'", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If
                End If

                lReturn = uctInstalments1.SaveQuote()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SaveQuote Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lPFPremFinanceCnt = uctInstalments1.PremiumFinanceCnt
                m_lPFPremFinanceVersion = uctInstalments1.PremiumFinanceVersion
                m_sPaymentTerms = "Instalments"
                m_iMTAType = uctInstalments1.MTAType
            End If
        ElseIf OptInvoice.Checked Then
            m_sPaymentTerms = "Invoice"

            If oPrepayment(0, 0) = "1" Then 'Ashwani - (RFC_Enable_PrePayment_functionality)
                GoTo PayNowFunctionality
            Else
                'PM033055 Plan should be deleted if someone selected OptInstallment before and Now Making Live policy on Invoice
                'Once entry is created in Pfpremiumfinance, should be deleted here if Invoice
                'lReturn = uctInstalments1.DeletePlanForOneInsFile(m_lInsuranceFileCnt)
                m_lPFPremFinanceCnt = 0
                m_lPFPremFinanceVersion = 0
                'm_sPaymentTerms = "Invoice"
            End If
        ElseIf optBankGuarantee.Checked Then
            m_sPaymentTerms = "BankGuarantee"
            Dim temp_oBankGuarantee As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankGuarantee, sClassName:="iSIRPolicyBankGuarantee.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oBankGuarantee = temp_oBankGuarantee
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oBankGuarantee.InsuranceFileCnt = gPMFunctions.ToSafeLong(m_lInsuranceFileCnt)

            oBankGuarantee.PartyCnt = gPMFunctions.ToSafeLong(PartyCnt)

            oBankGuarantee.AgentCnt = gPMFunctions.ToSafeLong(m_lLeadAgentCnt)

            oBankGuarantee.AgentType = m_sAgentType

            oBankGuarantee.BusinessTypeCode = m_sBusinessType

            oBankGuarantee.CoverFromDate = gPMFunctions.ToSafeDate(m_dtCoverStartDate)

            oBankGuarantee.ProductId = gPMFunctions.ToSafeLong(m_lProductId, 0)

            oBankGuarantee.TranCurrencyId = gPMFunctions.ToSafeLong(m_lCurrencyId, 0)
            'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

            oBankGuarantee.TotalPremium = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text, 0) + m_cRoundOffAmount
            'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling



            oBankGuarantee.AgentCode = gPMFunctions.ToSafeString(m_sAgentShortCode.Trim())

            oBankGuarantee.AgentName = gPMFunctions.ToSafeString(m_sAgentName.Trim())

            oBankGuarantee.PartyCode = gPMFunctions.ToSafeString(txtPolicyHolder.Text.Trim())

            oBankGuarantee.PartyName = gPMFunctions.ToSafeString(txtPolicyHolderFull.Text.Trim())

            ' Start the component

            m_lReturn = oBankGuarantee.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            If oBankGuarantee.OKCLICK Then
                ' Process policy as per Invoice since we don't need any transaction for BG
                ' upto now
                'OptInvoice.Value = True
                'm_sPaymentTerms = "Invoice"
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
            End If
        ElseIf optPayNow.Checked Then

            'Float Balance and Pre-Payment (RC)
            '//////////////////// START ///////////////////////
            m_sPaymentTerms = "PayNow"
PayNowFunctionality:
            m_lPFPremFinanceCnt = 0
            m_lPFPremFinanceVersion = 0


            If m_sTransactionType = "MTC" Or (m_sTransactionType = "MTA" And m_bIsNegativeInstallents) And m_lCancelFPOnCancelPolicy = 1 Then
                cGrossTotal = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text) - gPMFunctions.ToSafeCurrency(txtTotalNonTax.Text) - gPMFunctions.ToSafeCurrency(txtPNCT.Text)

                m_cChargeablePremium = m_cOriginalGrossPremium + cGrossTotal
                cGrossTotal = m_cChargeablePremium - m_cPremiumPaid
            Else
                cGrossTotal = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text) - gPMFunctions.ToSafeCurrency(txtTotalNonTax.Text) - gPMFunctions.ToSafeCurrency(txtPNCT.Text)

            End If

            'Needs to byPass the PayNow Screen in case of Renewal Amendment.
            'Bypass the Paynow Screen in NB/MTA if
            ' 1.    Business Type is Co Insurance Follow
            ' 2.    Business Type is Inward Facultative

            bByPassPaynow = False

            If cGrossTotal = 0 Then bByPassPaynow = True
            If m_sTransactionType = "REN" Then bByPassPaynow = True
            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Then
                'If m_sBusinessType.Trim() = "COIN FOLL" Or m_sBusinessType.Trim() = "IN FAC" Then
                If (m_sBusinessType.Trim() = "IN FAC" OrElse m_iNewBusinessNoTrans = 1) Then 'Ashwani - (RFC_Enable_PrePayment_functionality)
                    bByPassPaynow = True
                End If
            End If

            If Not bByPassPaynow Then
                Dim temp_oPayNow As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPayNow, sClassName:="iPMUPayNowOptions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oPayNow = temp_oPayNow
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oPayNow.InsuranceFileCnt = m_lInsuranceFileCnt
                If m_sAgentType = "Broker" Then

                    cLeadAgentCommission = CDec(Commission1.LeadAgentCommission)

                    oPayNow.AmountDue = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text) - cLeadAgentCommission - CDbl(Commission1.LeadAgentTax) + m_cRoundOffAmount

                    m_cTransactionAmount = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text) - cLeadAgentCommission - CDbl(Commission1.LeadAgentTax)
                Else
                    oPayNow.AmountDue = cGrossTotal + m_cRoundOffAmount

                    m_cTransactionAmount = cGrossTotal
                End If

                If optPayNow.Checked Then

                    oPayNow.PaymentOption = "paynow"
                End If
                oPayNow.PrePayment = oPrepayment(0, 0)
                ' Start the component

                m_lReturn = oPayNow.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return m_lReturn
                End If


                If oPayNow.OKClick Then
                    'Get Values from iPMUPaynowOptions for GetKeys()

                    m_lPaymentAccountID = oPayNow.PaymentAccountID

                    m_iDebitAgainst = oPayNow.DebitAgainst


                    m_vCreditTransactions = oPayNow.CreditTransactions

                    m_lCashListID = oPayNow.CashListID

                    m_lCashListItemID = oPayNow.CashListItemID

                    m_lTransactionID = oPayNow.CashTransDetailID
                    'In case other form is shown debitagainst ='' credittransactions=""
                Else
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If


                oPayNow.Dispose()
                oPayNow = Nothing

            End If

            '//////////////////// END ///////////////////////

            'Start - Prakash - WPR85_Paralleling
        ElseIf optCashDeposit.Checked Then

            m_sPaymentTerms = "CashDeposit"

            cGrossTotal = gPMFunctions.ToSafeCurrency(txtGrossTotal.Text) + gPMFunctions.ToSafeCurrency(txtTotalNonTax.Text) - gPMFunctions.ToSafeCurrency(txtTotalNonTax.Text) - gPMFunctions.ToSafeCurrency(txtPNCT.Text)

            If cGrossTotal <> 0 And m_sTransactionType <> "REN" Then

                Dim temp_oCashDeposit As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRPolicyCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oCashDeposit = temp_oCashDeposit
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oCashDeposit.InsuranceFileCnt = gPMFunctions.ToSafeLong(m_lInsuranceFileCnt)
                'developert guide no.40
                oCashDeposit.PolicyIssueDate = DateTime.Parse(DateTime.Now)

                oCashDeposit.CoverFromDate = gPMFunctions.ToSafeDate(m_dtCoverStartDate)

                oCashDeposit.PrePayment = oPrepayment(0, 0)
                'Start - Prakash - PN 65531

                oCashDeposit.LeadAgentCommission = Commission1.LeadAgentCommission

                oCashDeposit.LeadAgentTax = Commission1.LeadAgentTax

                oCashDeposit.TotalPremium = cGrossTotal + m_cRoundOffAmount
                m_cTransactionAmount = cGrossTotal
                'End - Prakash - PN 65531

                ' Start the component

                m_lReturn = oCashDeposit.Start
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If


                If oCashDeposit.OKCLICK Then


                    m_lPaymentAccountID = oCashDeposit.PaymentAccountID

                    m_iDebitAgainst = oCashDeposit.DebitAgainst
                    'Start - Prakash - PN 65554
                    If m_lPaymentAccountID <= 0 Then
                        MessageBox.Show("Payment via Cash Deposit Failed", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        result = gPMConstants.PMEReturnCode.PMCancel
                    End If
                    If m_sTransactionType = "MTC" Or (m_sTransactionType = "MTA" And cGrossTotal < 0) Then


                        m_vCreditTransactions = DBNull.Value
                    Else


                        m_vCreditTransactions = oCashDeposit.CreditTransactions
                        If (Not Information.IsArray(m_vCreditTransactions)) Or gPMFunctions.IsArrayEmpty(m_vCreditTransactions) Then
                            MessageBox.Show("Payment via Cash Deposit Failed", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            result = gPMConstants.PMEReturnCode.PMCancel
                        End If
                    End If
                    'End - Prakash - PN 65554
                Else
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If


                oCashDeposit.Dispose()
                oCashDeposit = Nothing
            End If
            'End - Prakash - WPR85_Paralleling
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        ' If you want to rollback a transaction or something, do it here

Finally_Renamed:

        Return result
        Resume
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessRequote
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessRequote() As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "ProcessRequote"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "MTC" Then

                If m_oBusiness.SetRiskStatusArray(m_vRiskStatus) <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to restore risk status to its original", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If
            If uctPMUListRisk1.DiscountedRiskCount > 0 Then
                PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonRequote)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return result
                End If
            End If
            'WPR 33-75 added
            If m_bIsBackdatedMTARequired Then
                lReturn = m_oAutoMTA.ClearBackdateMTAData(m_lInsuranceFileCnt)
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavAction1
            'Developer Guide No. 231
            RemoveHandler MyBase.FormClosing, AddressOf frmInterface_FormClosing
            Me.Hide()



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPrintQuote
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessPrintQuote() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPrintQuote"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If uctPMUListRisk1.SelectedRiskCount >= 1 Then

                ' set up document keys
                ReDim vKeyArray(1, 4)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFolderCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFolderCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lPartyCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "document_id"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 1


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Product_id"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductId


                lReturn = CType(GetDocument(vKeyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                MessageBox.Show("No risk has been selected on which to base the quote.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPrintProposal
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessPrintProposal() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPrintProposal"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vKeyArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If uctPMUListRisk1.SelectedRiskCount >= 1 Then

                ' set up document keys
                ReDim vKeyArray(1, 4)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFolderCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsuranceFolderCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lPartyCnt



                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "document_id"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = 2


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Product_id"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductId


                lReturn = CType(GetDocument(vKeyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                MessageBox.Show("No risk has been selected on which to base the proposal.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetDocument
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetDocument(ByVal v_vKeys(,) As Object) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "GetDocument"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oGetDocument As iPMUGetDocument.Interface_Renamed

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oGetDocument As Object
            lReturn = g_oObjectManager.GetInstance(temp_oGetDocument, sClassName:="iPMUGetDocument.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oGetDocument = temp_oGetDocument

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oGetDocument.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUGetDocument.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oGetDocument.SetKeys(v_vKeys)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUGetDocument.Interface.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oGetDocument.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUGetDocument.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oGetDocument = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessChangePolicyStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function ProcessChangePolicyStatus(Optional ByVal v_bSetAsWritten As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessChangePolicyStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'if we're really making the policy live...
            If m_iMode <> 1 Then

                ' If going live, delete any unselected risks' link
                '            records

                lReturn = m_oChangePolicyStatus.DeleteRisks(v_vrisks:=m_vRisks)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DeleteRisks Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Re-jig the risk and variation numbers of the remaining
                '            risks on this policy

                lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "RenumberRisks Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                'm_vRisks = ""
                m_vRisks = Nothing

            End If


            m_oChangePolicyStatus.Mode = m_iMode

            m_oChangePolicyStatus.TransactionType = m_sTransactionType
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'Added the optional parameters

            'Start Written Status-
            lReturn = m_oChangePolicyStatus.ChangePolicyStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sSelectedPolicyStatus:=m_sSelectedPolicyStatus, v_bBackDatedMTAsAllowed:=m_bBackDatedMTAsAllowed, v_bSetAsWritten:=v_bSetAsWritten)
            'End- Written Status
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ChangePolicyStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'get back new policy number

            m_sOldPolicyNumber = m_oChangePolicyStatus.OldPolicyNumber

            m_sNewPolicyNumber = m_oChangePolicyStatus.NewPolicyNumber



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' ProcessValidation
    ''' </summary>
    ''' <param name="r_lValidationStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessValidation(ByRef r_lValidationStatus As Integer) As Integer

        Const kMethodName As String = "ProcessValidation"

        Dim nResult As Integer
        Dim oAnswer As DialogResult
        Dim sMessage As String
        Dim iLevel As Integer
        Dim ilBound As Integer
        Dim iUBound As Integer
        Dim bSelectedRisks As Boolean

        Try

            nResult = PMEReturnCode.PMTrue

            ' assume validation failed
            r_lValidationStatus = kValidationFailed

            If uctPMUListRisk1.RiskCount < 1 Then
                If MessageBox.Show("There are no risks attached to this policy" & Chr(13) & Chr(10) & "is this correct ?", ACApp, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                    Return nResult
                End If
            End If

            ' if there is at least one selected risk
            nResult = CType(ProcessPolicyDiscountMakeLiveValidation(), PMEReturnCode)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not uctPMUListRisk1.OKToProceed Then
                MessageBox.Show("Cannot proceed, not all risks have successfully quoted", "Change Policy Status", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            If Not uctPMUListRisk1.CurrenciesUpdated Then
                MessageBox.Show("Currency has changed." & Chr(13) & Chr(10) & "All selected risks must be edited to allow for currency conversions", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            If uctPMUListRisk1.PendingReinsurance Then
                oAnswer = MessageBox.Show("Reinsurance has not been fully assigned" & Chr(13) & Chr(10) & "Do you wish to proceed with quotation?", Application.ProductName, MessageBoxButtons.YesNo)
                If oAnswer = Windows.Forms.DialogResult.No Then
                    Return nResult
                End If
            End If

            If Val(txtNettotal.Text) < 0 AndAlso (m_sTransactionType = "NB" OrElse m_sTransactionType = "REN") Then
                MessageBox.Show("Premium amount on New Business/Renewal cannot be credit type", "Amount type Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            If Not m_bIsTrueMonthlyPolicy Then
                If m_sTransactionType <> "MTC" AndAlso Val(txtNettotal.Text) >= 0 Then
                    If Commission1.TotalCommission > Val(txtNettotal.Text) Then
                        If m_sTransactionType = "MTA" Then 'START PN 37758
                            If MessageBox.Show("Total Commission is more than the Premium" & Chr(13) & Chr(10) & "Do you wish to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                                SSTabHelper.SetSelectedIndex(SSTab1, 3) 'Goto Commission Tab
                                Return nResult
                            End If
                        Else
                            MessageBox.Show("Total Commission is more than the Premium", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            SSTabHelper.SetSelectedIndex(SSTab1, 3) 'Goto Commission Tab
                            Return nResult
                        End If
                    End If
                ElseIf Val(txtNettotal.Text) < 0 AndAlso Commission1.TotalCommission < 0 Then
                    If Math.Abs(Commission1.TotalCommission) > Math.Abs(Val(txtNettotal.Text)) Then
                        MessageBox.Show("Total Commission Return is more than the Premium Return", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        SSTabHelper.SetSelectedIndex(SSTab1, 3) 'Goto Commission Tab
                        Return nResult
                    End If
                End If

                If Not uctInstalments1.QuoteAvailable And OptInstalments.Checked Then
                    MessageBox.Show("No Instalment quote is available.", "Policy Risk Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'Switch back to invoice payment term
                    OptInvoice.Checked = True
                    Return nResult
                End If
            Else
                If Not uctInstalments1.IsPlanSelected And OptInstalments.Checked Then
                    MessageBox.Show("Please select an instalment plan.", "Policy Risk Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return nResult
                End If
            End If

            ' Only run the additional validation rules if we're really making the policy live...
            If m_iMode <> 1 Then

                ' Get risks associated with this insurance file

                nResult = m_oChangePolicyStatus.GetRisksByStatus(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vRisks:=m_vRisks)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetRisksByStatus Failed", PMELogLevel.PMLogError)
                End If

                '*************
                ' risk status
                '*************
                '1 = Referred
                '2 = Declined
                '3 = Quoted
                '4 = Unquoted
                '5 = Purchase question to be answered
                '6 = Post quote questions to be answered
                '7 = Pre quote questions to be answered
                '8 = Pending Reinsurance
                '*************

                'This had better have something in it...
                If Not IsArray(m_vRisks) Then
                    sMessage = "Cannot proceed -" & Chr(13) & Chr(10) &
                           "There are no risks on this policy"
                Else
                    sMessage = ""
                    iLevel = 0

                    ilBound = m_vRisks.GetLowerBound(1)

                    iUBound = m_vRisks.GetUpperBound(1)

                    For iRisk As Integer = ilBound To iUBound

                        If Not String.IsNullOrEmpty(m_vRisks(kRiskIsSelected, iRisk)) Then

                            If CInt(m_vRisks(kRiskIsSelected, iRisk)) = 1 And m_vRisks(kRiskMandatoryrisk, iRisk) <> 1 Then
                                bSelectedRisks = True

                                ' select case risk status
                                Select Case m_vRisks(kRiskStatus, iRisk)
                                    Case 1, 2, 4
                                        If iLevel < 3 Then
                                            iLevel = 3
                                            sMessage = "Cannot proceed -" & Chr(13) & Chr(10) &
                                                   "At least one risk on this policy is unquoted"
                                        End If
                                    Case 8
                                        If iLevel < 2 Then
                                            iLevel = 2
                                            sMessage = "Cannot proceed -" & Chr(13) & Chr(10) &
                                                   "At least one risk on this policy has no reinsurance"
                                        End If
                                    Case 5, 6, 7
                                        If iLevel < 1 Then
                                            iLevel = 1
                                            sMessage = "Cannot proceed -" & Chr(13) & Chr(10) &
                                                   "At least one risk on this policy has questions to be answered"
                                        End If
                                End Select

                            End If
                        End If
                    Next iRisk
                End If
                If Not bSelectedRisks And sMessage.Trim() = "" Then
                    sMessage = "Cannot proceed -" & Chr(13) & Chr(10) & "At least one quoted risk on this policy must be selected to make it live"
                End If
                If sMessage <> "" Then
                    MessageBox.Show(sMessage, "Change Policy Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    r_lValidationStatus = kValidationFailed
                    Return nResult
                End If

            End If

            If Not m_bIsTrueMonthlyPolicy Then
                If Commission1.CheckCommissionAgainstPremium() = PMEReturnCode.PMFalse Then
                    Return nResult
                End If
            End If

            'Float Balance and Pre-Payment (RC)
            If Not (OptInvoice.Enabled And OptInvoice.Checked) AndAlso Not (OptInstalments.Enabled AndAlso OptInstalments.Checked) AndAlso Not (OptPutMTAOnNextInstalment.Enabled AndAlso OptPutMTAOnNextInstalment.Checked) AndAlso Not (optPayNow.Enabled AndAlso optPayNow.Checked) AndAlso Not (optBankGuarantee.Enabled AndAlso optBankGuarantee.Checked) AndAlso Not (optCashDeposit.Enabled AndAlso optCashDeposit.Checked) Then
                MessageBox.Show("Please Select Payment Option", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return nResult
            End If

            nResult = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableDebitOrder,
                                            v_vBranch:=g_iSourceID,
                                            r_vUnderwriting:=m_oEnableDebitOrder)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "getProductOptionValue Failed", PMELogLevel.PMLogError)
            End If

            If ToSafeLong(m_oEnableDebitOrder) = 1 AndAlso OptInvoice.Checked = True Then
                If cboCollectionFrequency.Enabled AndAlso (cboCollectionFrequency.SelectedIndex = -1 OrElse Not VB6.GetItemData(cboCollectionFrequency, cboCollectionFrequency.SelectedIndex) > 0) Then
                    MsgBox("Please Select Collection Frequency.", vbInformation, "Debit Order")
                    cboCollectionFrequency.Focus()
                    Return nResult
                End If

                If cboPaymentTerms.Enabled AndAlso (cboPaymentTerms.SelectedIndex = -1 OrElse Not VB6.GetItemData(cboPaymentTerms, cboPaymentTerms.SelectedIndex) > 0) Then
                    MsgBox("Please Select Payment Term.", vbInformation, "Debit Order")
                    cboPaymentTerms.Focus()
                    Return nResult
                End If
            End If

            Dim oAccount As bACTAccount.Form
            Dim oParty As bSIRParty.Business
            Dim nAgentAccountId As Integer
            Dim nInsuredAccountId As Integer
            Dim nAgentStatusID As Integer
            Dim nInsuredStatusID As Integer

            'Get Account Object.
            oAccount = New bACTAccount.Form
            nResult = oAccount.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInstance Failed", PMELogLevel.PMLogError)
            End If

            'Get Party Object
            oParty = New bSIRParty.Business
            nResult = oParty.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInstance Failed", PMELogLevel.PMLogError)
            End If

            If m_sTransactionType.Trim() = "DRI" And m_sShortName = "" Then
                m_sShortName = txtPolicyHolder.Text.Trim()
            End If

            'get AccountID of Lead Insured

            nResult = oParty.GetAccountID(vPartyRef:=m_sShortName, vAccountID:=nInsuredAccountId)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetAccountID Failed", PMELogLevel.PMLogError)
            End If

            'get StatusID of Lead Insured account

            nResult = oAccount.GetAccountStatus(v_lAccountId:=nInsuredAccountId, r_iAccountStatus:=nInsuredStatusID)
            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetAccountStatus Failed", PMELogLevel.PMLogError)
            End If

            'If account status of Lead Insured is 2(stopped)
            If nInsuredStatusID = 2 Then
                MessageBox.Show("The Policy cannot be made live because the Lead Insured " &
                                "account has been stopped.", "Account Stopped", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf m_sAgentShortCode.Trim() <> "" Then

                'get AccountID of Lead Agent
                nResult = oParty.GetAccountID(vPartyRef:=m_sAgentShortCode, vAccountID:=nAgentAccountId)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetAccountID Failed", PMELogLevel.PMLogError)
                End If

                'get StatusID of Lead Agent account

                nResult = oAccount.GetAccountStatus(v_lAccountId:=nAgentAccountId, r_iAccountStatus:=nAgentStatusID)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetAccountStatus Failed", PMELogLevel.PMLogError)
                End If

                'If account status of Lead Agent is 2(stopped)
                If nAgentStatusID = 2 Then
                    MessageBox.Show("The Policy cannot be made live because the Lead Agent " &
                                    "account has been stopped.", "Account Stopped", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            End If

            oAccount.Dispose()
            oAccount = Nothing

            oParty.Dispose()
            oParty = Nothing

            If nInsuredStatusID = 2 OrElse nAgentStatusID = 2 Then
                Return nResult
            End If

            ' Validate Agent Suspense Account
            If m_sBusinessType.Trim() <> "DIRECT" Then
                m_lReturn = ValidateAgentSuspenseAccount()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    r_lValidationStatus = kValidationFailed
                    Return nResult
                End If
            End If

            nResult = ValidateRoundOffAccount()
            If nResult <> PMEReturnCode.PMTrue Then
                r_lValidationStatus = kValidationFailed
                Return nResult
            End If

            If OptInstalments.Checked AndAlso ToSafeCurrency(txtGrossTotal.Text) = 0 AndAlso Not m_bIsTrueMonthlyPolicy AndAlso (m_sTransactionType = "NB" OrElse m_sTransactionType = "REN") Then
                MessageBox.Show("Cannot make a policy live with Zero Premium", "Zero Premium", MessageBoxButtons.OK, MessageBoxIcon.Error)
                r_lValidationStatus = kValidationFailed
                Return nResult
            End If

            If m_sTransactionType = "REN" AndAlso m_iPolicyRenewalStatus = PMBRenewalStatusTypeAwaitUpdate AndAlso m_lRenewalProcessMode <> 1 AndAlso m_lRenewalProcessMode <> 2 AndAlso m_lRenewalProcessMode <> 3 Then
                If MessageBox.Show("Confirm renewal terms have been Accepted as clicking OK will make policy live.", "Renewal Acceptance", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Cancel Then
                    Return nResult
                End If
            End If

            ' validation has been successful
            r_lValidationStatus = kValidationSuccessful
            Return nResult

        Catch excep As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetTabDefault
    '
    ' Parameters: n/a
    '
    ' Description: makes the passed tab to default
    '
    ' History:
    '           Created : JITENDRA : Date : Process ID
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SetTabDefault) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub SetTabDefault(ByRef sTabName As String)
    'Select Case sTabName
    'Case "Policy Fee"
    'SSTabHelper.SetSelectedIndex(SSTab1, kTabPoliyFees)
    'Case "Policy Tax"
    'SSTabHelper.SetSelectedIndex(SSTab1, kTabPolicyTaxes)
    'Case "Agent Commission"
    'SSTabHelper.SetSelectedIndex(SSTab1, kTabAgentCommission)
    'End Select
    'End Sub

    ' ***************************************************************** '
    ' Name: SetupInViewOnlyMode
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupInViewOnlyMode() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInViewOnlyMode"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then

                cmdEditRisk.Visible = True
                cmdAddRisk.Visible = True
                cmdDeleteRisk.Visible = True
                cmdCopyRisk.Visible = True

                cmdEditRisk.Enabled = False
                cmdAddRisk.Enabled = False
                cmdDeleteRisk.Enabled = False
                cmdCopyRisk.Enabled = False

                cmdPrintDocument.Visible = False
                cmdPrintProposal.Visible = False
                cmdPrintQuote.Visible = False
                cmdMakeLive.Visible = False
                If m_bBackdatedEditing = False Then
                    cmdRequote.Visible = False
                End If
                cmdSaveQuote.Text = "&OK"

                OptInstalments.Enabled = False
                OptInvoice.Enabled = False
                optPayNow.Enabled = False

                'Gaurav
                optBankGuarantee.Enabled = False
                'Start - Prakash - WPR85_Paralleling
                optCashDeposit.Enabled = False
                'End - Prakash - WPR85_Paralleling
                'WPR 33-75 added
                optMarkForCollection.Enabled = False
                'Start Written Status
                cmdWrite.Visible = False
                'End  Written Status
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessApplyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans  : 12-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ProcessApplyDiscount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessApplyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' if match discounted premium is set then
            ' indicate that the premium has changed and therefore
            ' the user must reconfirm the discounted premium / percentage before
            ' a discount can be applied....
            lReturn = ApplyPolicyDiscountMatchDiscountedPremiumLock()
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Apply policy Discount
                lReturn = ActionApplyDiscount()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ActionApplyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' UpdatePolicyDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdatePolicyDetails() As Integer

        Const kMethodName As String = "UpdatePolicyDetails"
        Dim nResult As Integer
        Dim nPutOnNextInstalmentRenewal As Integer
        Dim nMarkedForCollection As Integer

        nResult = PMEReturnCode.PMTrue

        ' get details to update
        If OptPutMTAOnNextInstalment.Checked Then
            nPutOnNextInstalmentRenewal = 1
        Else
            nPutOnNextInstalmentRenewal = 0
        End If

        If optMarkForCollection.Checked Then
            nMarkedForCollection = 1
        Else
            nMarkedForCollection = 0
        End If

        If OptInvoice.Checked Then
            m_sPaymentTerms = "Invoice"
        ElseIf optPayNow.Checked Then
            m_sPaymentTerms = "PayNow"
        ElseIf optBankGuarantee.Checked Then
            m_sPaymentTerms = "BankGuarantee"
        End If

        'cboCollectionFrequency and cboPaymentTerms should only get saved when OptInvoice.Checked = True
        If cboCollectionFrequency.SelectedIndex = -1 AndAlso cboPaymentTerms.SelectedIndex = -1 Then
            'Neither of them is selected

            nResult = m_oBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPutOnNextInstalmentRenewal:=nPutOnNextInstalmentRenewal,
                                                      v_sPaymentMethod:=m_sPaymentTerms, v_lMarkedForCollection:=nMarkedForCollection)

        ElseIf OptInvoice.Checked AndAlso cboCollectionFrequency.SelectedIndex = -1 Then
            'Only CollectionFrequency not selected, 
            nResult = m_oBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPutOnNextInstalmentRenewal:=nPutOnNextInstalmentRenewal,
                                                      v_sPaymentMethod:=m_sPaymentTerms, v_lMarkedForCollection:=nMarkedForCollection,
                                                      v_nDOPaymentTerms:=VB6.GetItemData(cboPaymentTerms, cboPaymentTerms.SelectedIndex))
        ElseIf OptInvoice.Checked AndAlso cboPaymentTerms.SelectedIndex = -1 Then
            'Only cboPaymentTerms not selected

            nResult = m_oBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPutOnNextInstalmentRenewal:=nPutOnNextInstalmentRenewal,
                                                      v_sPaymentMethod:=m_sPaymentTerms, v_lMarkedForCollection:=nMarkedForCollection,
                                                        v_nCollectionFrequency:=VB6.GetItemData(cboCollectionFrequency, cboCollectionFrequency.SelectedIndex))
        ElseIf OptInvoice.Checked Then
            'Both selected
            nResult = m_oBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPutOnNextInstalmentRenewal:=nPutOnNextInstalmentRenewal,
                                                      v_sPaymentMethod:=m_sPaymentTerms, v_lMarkedForCollection:=nMarkedForCollection,
                                                        v_nCollectionFrequency:=VB6.GetItemData(cboCollectionFrequency, cboCollectionFrequency.SelectedIndex),
                                                        v_nDOPaymentTerms:=VB6.GetItemData(cboPaymentTerms, cboPaymentTerms.SelectedIndex))
        End If

        If nResult <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "UpdatePolicyDetails Failed", PMELogLevel.PMLogError)
        End If
        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: ActionApplyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ActionApplyDiscount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionApplyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer
        Dim sFailureReason As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_bApplyDiscountClicked = True

            ' get policy discount status
            lReturn = CType(GetPolicyDiscountStatus(r_lPolicyDiscountStatus:=lPolicyDiscountStatus), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if the policy discount status allows action
            If lPolicyDiscountStatus = kPolicyDiscountStatusAllowApply Then

                ' Display the status bar message.
                StatusBar.Items.Item(0).Text = "Applying Policy Discount.."
                StatusBar.Refresh()

                ' process apply discount

                lReturn = m_oBusiness.ProcessApplyDiscount(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lProductId:=m_lProductId, v_sTransactionType:=m_sTransactionType, v_lTask:=m_iTask, r_sFailureReason:=sFailureReason, crAppliedDiscountPremium:=m_crAppliedDiscountPremium, dAppliedDiscountPercentage:=m_dAppliedDiscountPercentage, lAppliedMatchDiscountPremium:=m_lAppliedMatchDiscountPremium, lAppliedDiscountReasonId:=m_lAppliedDiscountReasonId)


                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If sFailureReason = "" Then
                        ' only raise an error if we have not provided a failure reason
                        gPMFunctions.RaiseError(kMethodName, "ProcessApplyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
                    Else
                        MessageBox.Show("Failed To apply the discount / loading because " & sFailureReason, "Apply Policy Discount Failure", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else

                    lReturn = CType(ReloadRiskTotals(v_lPolicyDiscountAction:=kPolicyDiscountActionApply), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ReloadRiskTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    cmdApplyDiscount.Enabled = False
                    If Not m_bIsMarketPlacePolicy Then
                        cmdMakeLive.Enabled = True
                    End If
                    ' Display the status bar message.
                    StatusBar.Items.Item(0).Text = "Completed"
                    StatusBar.Refresh()
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPolicyMakeLive
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ProcessPolicyMakeLive() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyMakeLive"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' process all policy discount make live steps

            lReturn = m_oBusiness.ProcessPolicyMakeLive(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessPolicyMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RollbackDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function RollbackDiscount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get policy discount status
            lReturn = CType(GetPolicyDiscountStatus(r_lPolicyDiscountStatus:=lPolicyDiscountStatus), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if policy discount status allows action
            If lPolicyDiscountStatus = kPolicyDiscountStatusAllowRollback Then

                ' Display the status bar message.
                StatusBar.Items.Item(0).Text = "Rolling Policy Discount Out..."
                StatusBar.Refresh()


                m_crAppliedDiscountPremium = m_crDiscountedPremium
                m_dAppliedDiscountPercentage = m_dDiscountPercentage
                m_lAppliedMatchDiscountPremium = m_lMatchDiscountedPremium
                m_lAppliedDiscountReasonId = m_lDiscountReasonId

                ' rollback the policy discount

                lReturn = m_oBusiness.ProcessRollbackDiscount(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lProductId:=m_lProductId, v_sTransactionType:=m_sTransactionType, v_lTask:=m_iTask)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ProcessRollbackDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
                Else
                    m_lDiscountReasonId = 0
                End If

                lReturn = CType(ReloadRiskTotals(kPolicyDiscountActionRollback), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "ReloadRiskTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                cmdApplyDiscount.Enabled = False
                If Not m_bIsMarketPlacePolicy Then
                    cmdMakeLive.Enabled = True
                End If

                ' Display the status bar message.
                StatusBar.Items.Item(0).Text = "Completed..."
                StatusBar.Refresh()

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyDiscountStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetPolicyDiscountStatus(ByRef r_lPolicyDiscountStatus As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountStatus"
        Dim bPolicyDiscountOn As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' default to dont allow action
            r_lPolicyDiscountStatus = kPolicyDiscountStatusNotApplicable

            ' determine if a policy discount reason has been
            ' specified against the current version of the policy
            bPolicyDiscountOn = m_lDiscountReasonId <> 0

            ' if policy discount is specified against the current policy
            If bPolicyDiscountOn Then

                ' determine which actions are allowable
                If cmdMakeLive.Enabled Then 'And b_IsMakeLiveClicked = True Then
                    r_lPolicyDiscountStatus = kPolicyDiscountStatusAllowRollback
                Else
                    r_lPolicyDiscountStatus = kPolicyDiscountStatusAllowApply
                End If

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Private Function IsDiscountApplied(ByVal v_lInsuranceFileCnt As Integer, ByRef bIsDiscountApplied As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "IsDiscountApplied"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vIsDiscountApplied As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = m_oBusiness.IsDiscountApplied(v_lInsuranceFileCnt, vIsDiscountApplied)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "IsDiscountApplied Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bIsDiscountApplied = gPMFunctions.ToSafeInteger(vIsDiscountApplied, 1) = 1


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ReloadRiskTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ReloadRiskTotals(ByVal v_lPolicyDiscountAction As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ReloadRiskTotals"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = SetUpListRisks()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetUpListRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate everything from policy down
            lReturn = CType(Recalculate(kRecalculateModeRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function SetupPolicyDiscount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsDiscountApplied As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' on loading enable apply discount button
            ' if a discount reason is specified against the policy
            If m_lDiscountReasonId <> 0 Then

                lReturn = CType(IsDiscountApplied(m_lInsuranceFileCnt, bIsDiscountApplied), gPMConstants.PMEReturnCode)

                cmdApplyDiscount.Visible = True

                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    cmdApplyDiscount.Enabled = False
                Else
                    If Not bIsDiscountApplied Then
                        cmdApplyDiscount.Enabled = True
                    ElseIf bIsDiscountApplied Then
                        cmdApplyDiscount.Enabled = False
                    End If
                End If

                If m_sTransactionType = "REN" And uctPMUListRisk1.DiscountedRiskCount > 0 Then
                    cmdApplyDiscount.Enabled = False
                End If

            End If

            If cmdApplyDiscount.Enabled Then
                cmdMakeLive.Enabled = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PolicyDiscountRollbackWrapper
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function PolicyDiscountRollbackWrapper(ByVal v_lRollbackReason As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PolicyDiscountRollbackWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer
        Dim lCarryOnWithRollback As DialogResult

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get policy discount status
            lReturn = CType(GetPolicyDiscountStatus(r_lPolicyDiscountStatus:=lPolicyDiscountStatus), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' determine if this action is allowable
            If lPolicyDiscountStatus = kPolicyDiscountStatusAllowRollback Then

                ' confirm that user understands that this action will result in discount being removed from all risks
                lCarryOnWithRollback = MessageBox.Show("The action that you are about to take will remove all applied policy discounts / loadings." &
                                       " Do you want to continue?", "Policy Discount Rollback", MessageBoxButtons.YesNo)

                ' if the user chooses not to continue with action
                If lCarryOnWithRollback = System.Windows.Forms.DialogResult.No Then

                    ' determine which action is was

                    Select Case v_lRollbackReason
                    ' cancel fees action
                        Case kPolicyDiscountRollbackReasonFeesChanged
                            uctPMUFees1.CancelAboutToChangeAction = True

                        ' cancel tax action
                        Case kPolicyDiscountRollbackReasonTaxChanged
                            uctPMURITax1.CancelAboutToChangeAction = True

                        ' cancel tax action
                        Case kPolicyDiscountRollbackReasonRiskSelectOrUnSelect
                            uctPMUListRisk1.CancelAboutToChangeAction = True

                            ' cancel other actions
                        Case Else
                            m_bCancelAboutToChangeAction = True

                    End Select

                Else

                    ' rollback discounts
                    lReturn = RollbackDiscount()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RollbackDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPolicyDiscountMakeLiveValidation
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ProcessPolicyDiscountMakeLiveValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyDiscountMakeLiveValidation"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer
        Dim sInvalidRiskDescription As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get policy discount risk this_premium and premium_this_year values
            lReturn = CType(GetPolicyDiscountStatus(r_lPolicyDiscountStatus:=lPolicyDiscountStatus), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Exit because nothing applies here
            If lPolicyDiscountStatus = kPolicyDiscountStatusNotApplicable Then
                Return result
            End If

            ' process pre make live

            lReturn = m_oBusiness.ProcessPolicyPreMakeLive(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lProductId:=m_lProductId, v_sTransactionType:=m_sTransactionType, v_lTask:=m_iTask, r_sInvalidRiskMessage:=sInvalidRiskDescription, v_lPolicyDiscountStatus:=lPolicyDiscountStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessPolicyPreMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If sInvalidRiskDescription <> "" Then

                ReloadRiskTotals(kPolicyDiscountActionRollback)

                cmdApplyDiscount.Enabled = True
                cmdMakeLive.Enabled = False

                MessageBox.Show(sInvalidRiskDescription, "Policy Risk Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ApplyPolicyDiscountMatchDiscountedPremiumLock
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ApplyPolicyDiscountMatchDiscountedPremiumLock() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyPolicyDiscountMatchDiscountedPremiumLock"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer
        Dim crCalculatedOriginalPremium As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' return the policy discount status
            lReturn = CType(GetPolicyDiscountStatus(r_lPolicyDiscountStatus:=lPolicyDiscountStatus), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' if the user is still able to apply a policy discount then
            If lPolicyDiscountStatus = kPolicyDiscountStatusAllowApply Then

                ' get the total selected premium amount at this point in time
                lReturn = GetTotalPremiumAmount()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetTotalPremiumAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if the discount is defined to match a discounted premium
                If m_lMatchDiscountedPremium = 1 Then

                    ' determine what the discounted premium amount would have been using the current discount percentage and current selected premium
                    crCalculatedOriginalPremium = m_crTotalPolicyAmount + (m_dDiscountPercentage * (m_crTotalPolicyAmount / 100))

                    ' if the discounted premium no longer matches the discounted premium that was specified
                    If crCalculatedOriginalPremium <> m_crDiscountedPremium Then
                        MessageBox.Show("The policy premium has changed since the discounted premium was specified." & Strings.Chr(13) & Strings.Chr(10) &
                                        "The policy must be requoted prior to a discount being applied!", "Policy Discount Premium Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalPremiumAmount
    '
    ' Parameters: n/a
    '
    ' Description: When discounting if match discounted premium has been
    '               specified then a percentage will have been calculated
    '               based on the selected total premium at the time of requoting
    '
    '              This function gets the total selected premium at the point
    '               of loading the interface so that the process can tell if
    '               the user has changed the premium up to the point of the discount
    '               being applied.
    '
    ' History:
    '           Created : MEvans : 23-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetTotalPremiumAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTotalPremiumAmount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPolicyPremium As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the total selected premium at the point of loading

            lReturn = m_oBusiness.GetPolicyDiscountTotalPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=vPolicyPremium)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if data was returned then
            If Information.IsArray(vPolicyPremium) Then
                ' get the total selected premium at the point of loading

                m_crTotalPolicyAmount = CDec(vPolicyPremium(0, 0))
            Else
                m_crTotalPolicyAmount = 0
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' Name: MergeKeyArray
    '
    ' Description:
    '
    ' History: 22/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (MergeKeyArray) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function MergeKeyArray(ByVal v_vNewKeyArray( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim bMatch As Boolean
    'Dim iIndex As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Don't merge if we dont have any keys
    'If Not Information.IsArray(m_vKeyArray) Then

    'm_vKeyArray = VB6.CopyArray(v_vNewKeyArray)
    'Return result
    'End If
    '
    ' Dont do anything if we have no keys
    'If Not Information.IsArray(v_vNewKeyArray) Then
    'Return result
    'End If
    '
    'For 'iLoop1 As Integer = v_vNewKeyArray.GetLowerBound(1) To v_vNewKeyArray.GetUpperBound(1)
    '
    'bMatch = False
    '
    ' Check if it's in the master array

    'For 'iLoop2 As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)


    'If m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop2).Equals(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) Then
    ' Update the value
    ' CTAF 071101 - Pass around objects
    'If Information.IsReference(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
    'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
    'Else


    'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
    'End If
    '
    'bMatch = True
    'Exit For
    'End If
    'Next iLoop2
    '
    ' Not found so add it
    'If Not bMatch Then
    '

    'iIndex = m_vKeyArray.GetUpperBound(1) + 1
    ''ReDim Preserve m_vKeyArray(1, iIndex)
    '


    'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
    '
    ' CTAF 071101 - Pass around objects
    'If Information.IsReference(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
    'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
    'Else


    'm_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
    'End If
    '
    'End If
    '
    'Next iLoop1
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeKeyArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetPaymentTerms
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 02 Aug 2006
    ' ***************************************************************** '
    Private Function GetPaymentTerms() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentTerms"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bInvoiceEnabled, bBankGuaranteeEnabled, bInstalmentsEnabled, bPayNowEnabled, bInstallmentSchemeExists, bMTAInstalmentsEnabled As Boolean
        'Start - Prakash - WPR85_Paralleling
        Dim bCashDepositEnabled As Boolean
        'End - Prakash - WPR85_Paralleling
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Start - Prakash - WPR85_Paralleling

            lReturn = m_oBusiness.GetPaymentTerms(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lPMUserID:=g_iUserID, r_bInvoiceEnabled:=bInvoiceEnabled, r_bInstalmentsEnabled:=bInstalmentsEnabled, r_bPayNowEnabled:=bPayNowEnabled, r_bBankGuaranteeEnabled:=bBankGuaranteeEnabled, r_bCashDepositEnabled:=bCashDepositEnabled)
            'End - Prakash - WPR85_Paralleling

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPaymentTerms Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            OptInvoice.Enabled = bInvoiceEnabled
            OptInstalments.Enabled = bInstalmentsEnabled
            optPayNow.Enabled = bPayNowEnabled
            optBankGuarantee.Enabled = bBankGuaranteeEnabled
            'Start - Prakash - WPR85_Paralleling
            optCashDeposit.Enabled = bCashDepositEnabled
            'End - Prakash - WPR85_Paralleling
            If m_sTransactionType = "MTA" Then

                lReturn = m_oBusiness.GetMTAPaymentTerms(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_bInstalmentsEnabled:=bMTAInstalmentsEnabled)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetMTAPaymentTerms Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If bMTAInstalmentsEnabled Then
                    OptInstalments.Enabled = Not bMTAInstalmentsEnabled
                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, Not bMTAInstalmentsEnabled)
                End If


                lReturn = m_oBusiness.CheckInstallmentSchemesforMTA(r_bSchemesExists:=bInstallmentSchemeExists)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CheckInstallmentSchemesforMTA Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not bInstallmentSchemeExists Then
                    OptInstalments.Enabled = False
                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, False)
                End If
            End If

            If m_sTransactionType = "MTA" Then
                If (m_bIsNegativeInstallents Or ToSafeDecimal(uctInstalments1.FirstInstalmentAmt) <= 0 Or
                    ToSafeDecimal(uctInstalments1.TotalPayableAmount) <= 0 Or ToSafeDecimal(uctInstalments1.GrossDue) <= 0) AndAlso txtGrossTotal.Text <= 0 Then
                    OptInvoice.Enabled = True
                End If
            End If

            If m_sTransactionType = "MTC" Then
                If m_bIsSingleInstalmentPlan And m_lNoOfPoliciesOnSingleInsAgent > 1 Then
                    OptInvoice.Checked = False
                    OptInvoice.Enabled = False
                    OptInstalments.Enabled = True
                    OptInstalments.Checked = True
                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, True)
                Else
                    'OptInvoice.Checked = True
                    OptInvoice.Enabled = True
                    OptInstalments.Enabled = False
                    optCashDeposit.Enabled = False
                    SSTabHelper.SetTabEnabled(SSTab1, kTabInstalments, False)
                End If
                optPayNow.Enabled = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' EnableDisblePaymentOptions
    ''' </summary>
    ''' <remarks></remarks>
    Sub EnableDisblePaymentOptions()

        Dim v_PaymentMethod As Object
        m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("iPMUListRisks.frmInterface.EnableDisblePaymentOptions()", "getUnderwritingOrAgency Failed.")
        End If

        GetPaymentTerms()

        'If uctInstalments1.TotalPayableAmount < 0 Then
        '   OptInstalments.Enabled = False
        'End If

        If m_sTransactionType = "REN" Then


            m_lReturn = m_oBusiness.GetPaymentMethod(m_lInsuranceFileCnt, v_PaymentMethod)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(v_PaymentMethod) Then
                gPMFunctions.RaiseError("iPMUListRisks.frmInterface.EnableDisblePaymentOptions()", "GetPaymentMethod Failed.")
            End If

            If Not bPaymentOptionAlreadyChecked Then

                Select Case CStr(v_PaymentMethod(0, 0)).Trim().ToLower()
                    Case "invoice", ""
                        OptInvoice.Checked = True
                    Case "instalments", "direct debit", "credit card"
                        OptInstalments.Checked = True
                    Case "paynow"
                        optPayNow.Checked = True

                    Case "cashdeposit"
                        optCashDeposit.Checked = True
                    Case "bankguarantee"
                        optBankGuarantee.Checked = True
                End Select
            End If

            bPaymentOptionAlreadyChecked = True
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: UNLOCKPOLICY
    '
    ' Description: UnLock Policy  'PN35753 --RC
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 18 Jul 2007
    '' ***************************************************************** '
    Private Function UNLOCKPOLICY() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result

            End If

            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UNLOCKPOLICY Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateAgentSuspenseAccount
    '
    ' Description: Validates Agent Suspense Account  '(RC) PLICO 9-10
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 26 Sep 2007
    '' ***************************************************************** '
    Function ValidateAgentSuspenseAccount() As gPMConstants.PMEReturnCode
        Dim Catch_Renamed As Boolean = False
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        result = gPMConstants.PMEReturnCode.PMTrue



        Try
            Catch_Renamed = True

            'Validate Agent Suspense Account

            Dim oAccount As bACTAccount.Form
            'Dim oExplorer As bACTExplorer.Form

            Dim oExplorer As bACTExplorer.Form
            Dim sShortCode As String = ""
            Dim lASAccountId As Integer

            'Get Account Object
            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAccount = temp_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ValidateAgentSuspenseAccount", "GetInstance Failed - bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get Explorer Object
            Dim temp_oExplorer As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oExplorer, "bACTExplorer.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oExplorer = temp_oExplorer
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ValidateAgentSuspenseAccount", "GetInstance Failed - bACTExplorer.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'get system option "Agent Commission Suspended Postings"
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5037, r_sOptionValue:=sShortCode, v_iSourceID:=g_iSourceID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ValidateAgentSuspenseAccount", "Failed to get System Option [5037]", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If system option "Agent Commission Suspended Postings" is true then
            'check that Agent Suspense account should be valid Account in "Current Liabilities"
            If gPMFunctions.ToSafeLong(sShortCode, 0) = 1 Then

                'get system option "Agent suspense account"
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5039, r_sOptionValue:=sShortCode, v_iSourceID:=g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ValidateAgentSuspenseAccount", "Failed to get System Option [5039]", gPMConstants.PMELogLevel.PMLogError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sShortCode.Trim() = "" Then
                    MessageBox.Show("Enter Agent Suspense Account in System Options -> Accounts -> General", "Blank Agent Suspense Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get account Id

                m_lReturn = oAccount.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=lASAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Enter Valid Agent Suspense Account in System Options -> Accounts -> General", "Invalid Agent Suspense Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                sShortCode = oExplorer.FullKey(lAccountId:=lASAccountId).Split("\"c)(oExplorer.FullKey(lAccountId:=lASAccountId).Split("\"c).GetUpperBound(0) - 1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ValidateAgentSuspenseAccount", "Failed to get details from the explorer object", gPMConstants.PMELogLevel.PMLogError)
                End If

                'AgentSuspenseAccount must be under Current Liability folder
                If sShortCode.Trim().ToLower() <> "current liabilities" Then
                    MessageBox.Show("Agent Suspense Account must be under 'Current Liabilities' folder", "Invalid Agent Suspense Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            oAccount.Dispose()
            oAccount = Nothing

            oExplorer.Dispose()
            oExplorer = Nothing

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



            If Catch_Renamed Then

                Select Case Information.Err().Number
                    Case Else

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAgentSuspenseAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAgentSuspenseAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        result = gPMConstants.PMEReturnCode.PMFalse

                        GoTo Finally_Renamed
                End Select
            End If
Finally_Renamed:
        End Try
    End Function

    Private Function GetAutoRenewalOption(ByRef r_bAutoRENFlag As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAutoRenewalOption"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetAutoRenewalFlag(m_lInsuranceFileCnt, r_bAutoRENFlag)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetAutoRenewalOption", "Failed to fetch Autorenewal flag", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetAgentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    ' ***************************************************************** '
    Private Function GetAgentDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAgentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAgentDetails As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get insurance file details

            lReturn = m_oBusiness.GetAgentDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=vAgentDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetAgentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vAgentDetails) Then
                m_vLeadAgentCnt = gPMFunctions.ToSafeLong(vAgentDetails(0, 0))
                m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(vAgentDetails(1, 0))
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetInsuranceFileDetails(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function UnLockSingleInstalmentPlan() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Find the Business Class
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_bIsSingleInstalmentPlan Then

                m_lReturn = oPMLock.UnLockKey("Single_Instalment_Plan", vKeyValue:=m_vLeadAgentCnt, iUserID:=g_oObjectManager.UserID, vKey2Value:=m_lProductId)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the plan", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UnLockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If
            'Terminate the business object

            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="UnLockSingleInstalmentPlan Failed", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UnLockSingleInstalmentPlan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPolicyRefund
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gautam Poddar : Date : 27 Apr 2009
    ' ***************************************************************** '
    Private Function ProcessPolicyRefund() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyRefund"

        Dim obSIRPremiumFinance As bSIRPremiumFinance.Business
        Dim vPFPremiumFinance, vInstallmentPlan As Object
        Dim vDebitTransDetail As Object


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_cOriginalGrossPremium = 0
            m_cPremiumPaid = 0
            'Get Premium Finance Object.
            Dim temp_obSIRPremiumFinance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_obSIRPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obSIRPremiumFinance = temp_obSIRPremiumFinance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = m_oBusiness.GetPremiumDetailsForAllPolicyVersions(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_vResults:=m_vPremiumDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get premium details for the policy", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            If Information.IsArray(m_vPremiumDetails) Then
                For lRow As Integer = m_vPremiumDetails.GetLowerBound(1) To m_vPremiumDetails.GetUpperBound(1)
                    m_cOriginalGrossPremium = m_cOriginalGrossPremium + gPMFunctions.ToSafeCurrency(m_vPremiumDetails(3, lRow), 0) + gPMFunctions.ToSafeCurrency(m_vPremiumDetails(5, lRow), 0) + gPMFunctions.ToSafeCurrency(m_vPremiumDetails(6, lRow), 0) + gPMFunctions.ToSafeCurrency(m_vPremiumDetails(8, lRow), 0) + gPMFunctions.ToSafeCurrency(m_vPremiumDetails(9, lRow), 0) - gPMFunctions.ToSafeCurrency(m_vPremiumDetails(13, lRow), 0)

                    m_cPremiumPaid += gPMFunctions.ToSafeCurrency(m_vPremiumDetails(10, lRow), 0)
                Next lRow
            End If

            m_lReturn = ProcessPaymentTerms()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Or m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                gPMFunctions.RaiseError(kMethodName, "Process Payment Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If payment process is cancelled don?t show error, just exit
            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            If Information.IsArray(m_vPremiumDetails) Then
                ReDim m_vDebitTransactions(0)
                For nRow As Integer = m_vPremiumDetails.GetLowerBound(1) To m_vPremiumDetails.GetUpperBound(1)

            m_lReturn = obSIRPremiumFinance.GetLatestValidFinancePlan(v_lInsuranceFileCnt:=m_vPremiumDetails(0, nRow), r_vPFPremiumFinance:=vPFPremiumFinance)

            If Information.IsArray(vPFPremiumFinance) Then
                For lRow As Integer = vPFPremiumFinance.GetLowerBound(1) To vPFPremiumFinance.GetUpperBound(1)


                    ReDim vInstallmentPlan(vPFPremiumFinance.GetUpperBound(0), 0)

                    For lCol As Integer = vPFPremiumFinance.GetLowerBound(0) To vPFPremiumFinance.GetUpperBound(0)


                        vInstallmentPlan(lCol, 0) = vPFPremiumFinance(lCol, lRow)
                    Next lCol


                    m_lReturn = obSIRPremiumFinance.CancelPlanInHouse(vPremiumFinance:=vInstallmentPlan, dRefund:=0, r_vDebitTransDetail:=vDebitTransDetail)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then



                        m_vDebitTransactions(m_vDebitTransactions.GetUpperBound(0)) = vDebitTransDetail

                        ReDim Preserve m_vDebitTransactions(m_vDebitTransactions.GetUpperBound(0) + 1)
                    End If
                Next lRow
                m_bProcessSettleTransactions = True
            End If
       Next nRow
    End If
                

                'Remove the extra row from array

                ReDim Preserve m_vDebitTransactions(m_vDebitTransactions.GetUpperBound(0) - 1)

            



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function IsSubsequentRiskVersionsEdited(ByVal v_lRiskID As Integer, ByVal v_dtMTAEffectiveDate As Date) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "IsSubsequentRiskVersionsEdited"



        result = gPMConstants.PMEReturnCode.PMTrue


        Dim lReturn As gPMConstants.PMEReturnCode = m_oBusiness.IsSubsequentRiskVersionsEdited(v_lRiskID:=v_lRiskID, v_dtMTAEffectiveDate:=v_dtMTAEffectiveDate)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        GoTo Finally_Renamed



        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result
        Resume
        Return result
    End Function

    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    Function ValidateRoundOffAccount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateRoundOffAccount"
        Const kSystemOptionRoundOffAccount As Integer = 5080
        'Validate Round Off Account
        Dim sShortCode As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        Try



            If m_bRoundOff Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionRoundOffAccount, r_sOptionValue:=sShortCode, v_iSourceID:=g_iSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get System Option for RoundOff", gPMConstants.PMELogLevel.PMLogError)
                End If

                If sShortCode.Trim() = "" Then
                    MessageBox.Show("Enter Round Off Account in System Options -> Accounts -> General", "Blank Round Off Account", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result

    End Function
    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Private Function ProcessPolicyReceiptMediatTypeStatus(ByRef r_bProceed As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyReceiptMediatTypeStatus"

        Const kCheckMediaTypeStatusAtPolicyRefund As String = "check_mediatype_status_at_policy_refund"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bCheckMediaTypeStatusAtPolicyRefund As Boolean
        Dim vResultArray(,) As Object

        'Dim o_FindClaimBusiness As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:=kCheckMediaTypeStatusAtPolicyRefund, r_vProductArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for media type status validation", gPMConstants.PMELogLevel.PMLogError)
            Else
                If Information.IsArray(vResultArray) Then

                    bCheckMediaTypeStatusAtPolicyRefund = IIf(CDbl(vResultArray(0, 0)) = 1, 1, 0)
                End If
            End If
            If bCheckMediaTypeStatusAtPolicyRefund Then

                lReturn = m_oBusiness.ProcessPolicyReceiptMediaTypeStatus(v_lInsuranceFileId:=m_lInsuranceFileCnt, r_bProceed:=r_bProceed)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRListRisk.ProcessPolicyReceiptMediaTypeStatus", gPMConstants.PMELogLevel.PMLogError)
                End If
                'Start Arul PN 63414
            Else
                r_bProceed = True
                'End Arul PN 63414
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    Private Sub ToggleMarkForCollection()

        'WPR12- Enhancement Quote Collection Process
        Const kMethodName As String = "ToggleMarkForCollection"

        Dim lSubValue As Integer
        Try


            '(PN-67175)Or Condition in if Added by Nitesh as on 03-02-2010(To stop eneable MakeLive Button if doing Backdate MTA)
            '(PN-68508)-Resolved with same above condition by Nitesh (11-02-2010)
            cmdMakeLive.Enabled = Not (optMarkForCollection.Checked Or m_bIsBackdatedMTARequired)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        Finally



        End Try
    End Sub
    ''' <summary>
    ''' ToggleMarkForCollection
    ''' </summary>
    ''' <param name="sMakeLivePaymentTerms"></param>
    ''' <remarks></remarks>
    Private Sub ToggleMarkForCollection(Optional ByVal sMakeLivePaymentTerms As String = "")

        Const kMethodName As String = "ToggleMarkForCollection"
        Dim nSubValue As Integer

        Try

            cmdMakeLive.Enabled = Not (optMarkForCollection.Checked OrElse m_bIsBackdatedMTARequired OrElse cmdApplyDiscount.Enabled)

            If Len(Trim(sMakeLivePaymentTerms)) <> 0 Then
                m_sPaymentCashOrDebit = "0"
                If sMakeLivePaymentTerms = kMakeLiveOptionsINVOICE Then
                    If VB6.GetItemString(cboPaymentTerms, cboPaymentTerms.SelectedIndex).ToString.Trim.Length > 0 Then
                        m_sPaymentCashOrDebit = VB6.GetItemString(cboPaymentTerms, cboPaymentTerms.SelectedIndex).ToString
                    Else
                        m_sPaymentCashOrDebit = "0"
                    End If
                End If
                uctPMUFees1.Initialise()
                uctPMUFees1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate, sMakeLivePaymentTerms, m_sPaymentCashOrDebit)
                uctPMUFees1.InsuranceFileCnt = m_lInsuranceFileCnt
            End If

            nSubValue = Recalculate(kRecalculateModeRisks)

            If nSubValue <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ToggleMarkForCollection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
    End Sub


    Private Function ValidationMarkForCollection(ByRef r_bValidation As Boolean) As Integer

        'WPR12- Enhancement Quote Collection Process
        Dim result As Integer = 0
        Const kMethodName As String = "ValidationMarkForCollection"

        Dim lSubValue As Integer
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            r_bValidation = True

            If optMarkForCollection.Checked Then
                If uctPMUListRisk1.TotalPremium <= 0 Then
                    MessageBox.Show("Premium Value should be greater then zero", "Mark For Collection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    r_bValidation = False
                    Return result
                End If
                If uctPMUListRisk1.TotalNoSelectedQuotedRisks <> uctPMUListRisk1.RiskCount Then
                    MessageBox.Show("Please put all Risks to Quote", "Mark For Collection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    r_bValidation = False
                    Return result
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    'WPR12- Enhancement Quote Collection Process
    Private Function EnableDisableMarkForCollection() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableMarkForCollection"

        Dim lSubValue As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            optMarkForCollection.Enabled = uctPMUListRisk1.TotalNoSelectedQuotedRisks = uctPMUListRisk1.RiskCount And uctPMUListRisk1.RiskCount > 0 And uctPMUListRisk1.TotalPremium > 0

            'WPR 33-75 added
            If m_iTask = gPMConstants.PMEComponentAction.PMView Or m_bBackdatedEditing = True Then
                optMarkForCollection.Enabled = False
            End If

        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally





        End Try
        Return result
    End Function

    'WPR12- Enhancement Quote Collection Process
    Private Function CheckRiskStatusMarkForCollection() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckRiskStatusMarkForCollection"

        Dim lSubValue As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'If optMarkForCollection.Value = True Then

            If uctPMUListRisk1.TotalNoSelectedQuotedRisks > 0 And uctPMUListRisk1.TotalNoSelectedQuotedRisks = uctPMUListRisk1.SelectedRiskCount And uctPMUListRisk1.TotalPremium > 0 Then
                optMarkForCollection.Enabled = True
            Else
                optMarkForCollection.Enabled = False
                optMarkForCollection.Checked = False

                'WPR 33-75 added
                If m_bIsBackdatedMTARequired Then
                    If uctPMUListRisk1.OKToProceed And cmdBackdateMTA.Enabled = False Then

                        If Not m_bIsMarketPlacePolicy Then
                            cmdMakeLive.Enabled = True
                        End If
                        'WPR 33-75 added
                    Else
                        cmdMakeLive.Enabled = False
                    End If
                Else
                    If Not m_bIsMarketPlacePolicy Then
                        cmdMakeLive.Enabled = True
                    End If
                End If
            End If
            'End If
            'WPR 33-75 end


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally





        End Try
        Return result
    End Function

    Private Sub SSTab1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SSTab1.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            SSTab1.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            SSTab1.SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            SSTab1.SelectedIndex = 3
        End If
        If e.Alt And e.KeyCode = Keys.D5 Then
            SSTab1.SelectedIndex = 4
        End If
    End Sub

    Function ValidateCertificateYear(ByRef r_bIsValid As Boolean) As Integer

        Dim iResult As Integer = 0
        Const kMethodName As String = "ValidateCertificateYear"
        Dim vValueSubAgentCertificateYears As String = ""
        Dim lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        iResult = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Read Product options
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValueSubAgentCertificateYears)
            If vValueSubAgentCertificateYears = "1" Then

                lReturn = m_oBusiness.GetAndValidateSubAgentDetailsViaInsFile(r_bIsValid, m_lInsuranceFileCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iResult = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.RaiseError(kMethodName, "Unable to validate certificate year.", gPMConstants.PMELogLevel.PMLogError)
                End If

                If r_bIsValid = False Then
                    If m_sTransactionType = "NB" Or m_sTransactionType = "MTR" Then
                        MessageBox.Show("You cannot make this transaction live - please check Certificate Year configuration.", "Certificate Year", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            Else
                r_bIsValid = True
            End If


        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try


        Return iResult


    End Function

    Function ValidateMakeLiveOptions(ByRef r_bIsValid As Boolean) As Integer
        Dim iResult As Integer = 0
        Const kMethodName As String = "ValidateMakeLiveOptions"

        Dim lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        iResult = gPMConstants.PMEReturnCode.PMTrue
        r_bIsValid = True
        Try
            If m_sTransactionType = "MTC" AndAlso OptInstalments.Checked = True Then

                MessageBox.Show("Cancellation is not allowed on the default selected payment method, please select other desired payment method", "Make Live", MessageBoxButtons.OK)
                r_bIsValid = False

            ElseIf OptInstalments.Checked = True And OptInstalments.Enabled = False Then
                MessageBox.Show("You do not have the authority to make this policy live for the configured ""Method of Payment"".", "Make Live", MessageBoxButtons.OK)
                r_bIsValid = False
            End If
            If OptInvoice.Checked = True And OptInvoice.Enabled = False Then
                MessageBox.Show("You do not have the authority to make this policy live for the configured ""Method of Payment"".", "Make Live", MessageBoxButtons.OK)
                r_bIsValid = False
            End If

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try

        Return iResult

    End Function


    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        Call ProcessWritePolicy()
    End Sub
    'StartWritten Status
    Public Function ProcessWritePolicy() As Long
        Const kMethodName As String = "ProcessWritePolicy"
        Dim lValidationStatus As Long
        Dim vSelectionArray As Object
        Dim lReturn As Long
        Dim iResult As Integer = 0

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' process validation rules
            m_lReturn = ProcessValidation(lValidationStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessValidation Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lValidationStatus <> kValidationSuccessful Then
                Exit Function
            End If

            If OptInvoice.Checked <> True Then
                If MsgBox("Payment option other than Invoice is not supported when the policy has been written so the payment option will be switched to Invoice." & vbNewLine & "Do you want to continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                    OptInvoice.Checked = True
                Else
                    Exit Function
                End If
            End If
            ' get an array of the the risks selection status
            vSelectionArray = uctPMUListRisk1.RiskSelectionStatus

            ' Update the risk records with the selection status'
            If IsArray(vSelectionArray) Then
                m_lReturn = m_oBusiness.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MsgBox("Unable to update the risk selection status.", vbOKOnly + vbCritical)
                    Exit Function
                End If
            End If

            'Recalculate Premium
            lReturn = UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Process the OK in the control
            lReturn = uctPMUListRisk1.OKClick()
            If lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Function
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "uctPMUListRisk1.OKClick Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' process change policy status
            lReturn = ProcessChangePolicyStatus(True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessChangePolicyStatus", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update policy details
            lReturn = UpdatePolicyDetails()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "UpdatePolicyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' The go-live call is still valid in the written process
            lReturn = ProcessPolicyMakeLive()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessPolicyDiscountMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create WM Task as reminder
            lReturn = CreateWrittenWorkTask()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CreateWrittenWorkTask Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Everything OK, so we can hide the interface.
            Me.Hide()

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessWritePolicy, excep:=ex)
        End Try

        Return iResult

    End Function
    'End-Written Status
    ''' <summary>
    ''' GetLookupDetails
    ''' </summary>
    ''' <param name="sLookupTable"></param>
    ''' <param name="ctlLookup"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim nResult As Integer
        Dim iRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const kValueTableName As Integer = 0
        Const kValueID As Integer = 1
        Const kValueStartPos As Integer = 2
        Const kValueNumber As Integer = 3

        ' Lookup detail contants.
        Const kDetailKey As Integer = 0
        Const kDetailDesc As Integer = 1

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For iRow = m_oLookupValues.GetLowerBound(1) To m_oLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_oLookupValues(kValueTableName, iRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next iRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return nResult
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For iCntr As Integer = CInt(m_oLookupValues(kValueStartPos, iRow)) To CInt((CDbl(m_oLookupValues(kValueStartPos, iRow)) + CDbl(m_oLookupValues(kValueNumber, iRow))) - 1)
                ' Add the details to the control.
                Dim listindex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem((m_oLookUpDetails(kDetailDesc, iCntr)), CInt(m_oLookUpDetails(kDetailKey, iCntr))))

                ' Check if this is the selected index.
                If CStr(m_oLookupValues(kValueID, iRow)) <> "" Then
                    If CDbl(m_oLookupValues(kValueID, iRow)) = CInt(m_oLookUpDetails(kDetailKey, iCntr)) Then
                        ctlLookup.SelectedIndex = listindex
                    End If
                End If

            Next iCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_oLookupValues(kValueNumber, iRow)) = "" Then
                ctlLookup.SelectedIndex = 0
            End If

            Return nResult

        Catch excep As System.Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' AddWorkManagerTask
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddWorkManagerTask() As Long
        Dim oTaskInstance As Object
        Dim nReturn As Long
        Dim dtDueDate As Date
        Dim v_lAction As Long
        Dim v_lPMWrkTaskGroupID As Long
        Dim v_lPMWrkTaskID As Long
        Dim v_sDescription As String
        Dim v_iIsUrgent As Integer
        Dim r_lPMWrkTaskInstanceCnt As Long
        Dim v_dtDateCreated As Date
        Dim v_iIsVisible As Integer
        Dim v_sWorkflowInformation As String
        Dim v_iIsTaskReview As Integer
        Dim v_sCustomer As String
        Dim v_dtTaskDueDate As Date
        Dim v_iTaskStatus As Integer
        Dim v_iCreatedByID As Integer
        Dim v_lPMUserGroupID As Long

        nReturn = gPMConstants.PMEReturnCode.PMTrue

        Try
            v_lAction = 1

            ' Create the Component
            nReturn = g_oObjectManager.GetInstance(
                      oObject:=oTaskInstance,
                      sClassName:="bPMWrkTaskInstance.FormClass",
                      vInstanceManager:=PMGetViaClientManager)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nReturn = gPMConstants.PMEReturnCode.PMFalse
                Return nReturn
            End If

            v_sCustomer = m_sShortName
            v_dtTaskDueDate = DateAdd(DateInterval.Weekday, 1, Date.Today) + " " + CDate("23:59:59")
            v_sDescription = "Please also apply your amendment to the renewal version of this policy " & txtPolicyRef.Text
            v_dtDateCreated = Date.Today
            v_lPMWrkTaskGroupID = 5
            v_lPMWrkTaskID = 18
            v_lPMUserGroupID = 15
            v_iIsVisible = 1

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Start the Form
            nReturn = oTaskInstance.CreateNew(
                v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID,
                v_lPMWrkTaskID:=v_lPMWrkTaskID,
                v_sCustomer:=v_sCustomer,
                v_dtTaskDueDate:=v_dtTaskDueDate,
                v_lPMUserGroupID:=v_lPMUserGroupID,
                v_sDescription:=v_sDescription,
                v_iTaskStatus:=v_iTaskStatus,
                v_iIsUrgent:=v_iIsUrgent,
                r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt,
                v_dtDateCreated:=v_dtDateCreated,
                v_iCreatedByID:=v_iCreatedByID,
                v_iUserID:=g_iUserID,
                v_iIsVisible:=v_iIsVisible,
                v_sWorkflowInformation:=v_sWorkflowInformation,
                v_iIsTaskReview:=v_iIsTaskReview)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                ' Log Error Message

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:-      iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Err.Description)
                Return nReturn
            End If


            ' If the User Cancelled then exit as we do not need
            ' to Refresh the Form details.

            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return nReturn

        Catch ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddWorkManagerTask", vApp:=ACApp, vClass:=ACClass, vMethod:="AddWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Err.Description, excep:=ex)

            Return nReturn
        End Try
    End Function

    ''' <summary>
    ''' 'WPR100 (B)- Portfolio And Clone Batch Processing
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessAccept() As Integer

        Const kMethodName As String = "ProcessAccept"

        Dim nReturn As Integer
        Dim iLevel As Integer
        Dim nlBound As Integer
        Dim nUBound As Integer

        Dim bSelectedRisks As Boolean
        Dim sMessage As String
        Dim nResult As PMEReturnCode
        Dim oFindRisk As bSIRFindRisk.Form
        Try
            nResult = PMEReturnCode.PMTrue

            nResult = g_oObjectManager.GetInstance(oFindRisk, sClassName:="bSIRFindRisk.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInstance of bSIRFindRisk.Form Failed", PMELogLevel.PMLogError)
            End If

            nResult = oFindRisk.SearchAll(
                         r_vResultArray:=m_vRisks,
                         v_vInsuranceFileCnt:=m_lInsuranceFileCnt)

            If nResult <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessAccept Failed", PMELogLevel.PMLogError)
            End If

            'This had better have something in it...
            If Not Information.IsArray(m_vRisks) Then
                sMessage = "Cannot proceed -" & vbCrLf &
                           "There are no risks on this policy"
            Else
                sMessage = ""
                iLevel = 0
                nlBound = m_vRisks.GetLowerBound(1)

                nUBound = m_vRisks.GetUpperBound(1)

                For iRisk As Integer = nlBound To nUBound
                    If ToSafeString(m_vRisks(15, iRisk)) = "1" AndAlso ToSafeString(m_vRisks(40, iRisk)) <> "1" Then
                        bSelectedRisks = True
                        If m_vRisks(22, iRisk) <> "QUOTED" Then
                            sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) &
                                       "At least one risk on this policy has no reinsurance"
                        End If
                    End If
                Next iRisk

            End If

            ' PW311002 - Check if any of the risks are flagged as "selected"
            ' PW021202 - Don't muscle in on someone else's message
            If Not bSelectedRisks And sMessage.Trim() = "" Then
                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) &
                           "At least one quoted risk on this policy must be selected to make it live"
            End If

            If (sMessage <> "") Then
                MsgBox(sMessage, vbOK + vbInformation, "Validate Risks")
                m_bIsReadyToAccept = False
                m_bIsAllRiskQuoted = False
                Return PMEReturnCode.PMTrue
            End If

            m_bIsReadyToAccept = True
            m_bIsAllRiskQuoted = True
            m_lStatus = PMEReturnCode.PMOK

            Me.Hide()

        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        Finally
            oFindRisk.Dispose()
        End Try
        Return nResult

    End Function
    ''' <summary>
    ''' GetRiskRating
    ''' </summary>
    ''' <param name="iTask"></param>
    ''' <param name="bIsSilentQuote"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskRating(ByRef iTask As Integer, ByVal bIsSilentQuote As Boolean) As Integer
        Const kMethodName As String = "GetRiskRating"
        Dim nReturn As Integer = 0
        Dim oPerilAllocation As iPMUPerilAllocation.Interface_Renamed
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            nReturn = g_oObjectManager.GetInstance(oPerilAllocation,
                                                   sClassName:="iPMUPerilAllocation.Interface_Renamed",
                                                   vInstanceManager:=PMGetLocalInterface)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetInstance of iPMUPerilAllocation.Interface Failed")
            End If

            ' set process modes
            nReturn = oPerilAllocation.SetProcessModes(vTask:=iTask, vTransactionType:=m_sTransactionType)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "iPMUPerilAllocation.Interface.SetProcessModes Failed")
            End If

            ' set peril allocation properties
            oPerilAllocation.InsuranceFolderCnt = m_lInsuranceFolderCnt
            oPerilAllocation.InsuranceFileCnt = m_lInsuranceFileCnt
            oPerilAllocation.RiskId = m_lRiskId
            oPerilAllocation.IsBackDatedMTA = m_bIsBackdatedMTARequired
            oPerilAllocation.IsSilentQuote = bIsSilentQuote
            oPerilAllocation.ApplyMTATaxRatesonRen = m_sApplyMTATaxRatesonRen

            nReturn = oPerilAllocation.Start()
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "iPMUPerilAllocation.Interface.Start Failed")
            End If
            m_lStatus = oPerilAllocation.Status
            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            oPerilAllocation.Dispose()

            oPerilAllocation = Nothing
        End Try

    End Function
    ''' <summary>
    ''' GetRiskReinsurance
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskReinsurance(ByVal bIsSilentQuote As Boolean) As Integer
        Const kMethodName As String = "GetRiskReinsurance"
        Dim oReinsurance As iPMUReinsurance.Interface_Renamed
        Dim nReturn As Integer = 0
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            nReturn = g_oObjectManager.GetInstance(oReinsurance,
                                                   sClassName:="iPMUReinsurance.Interface_Renamed",
                                                   vInstanceManager:=PMGetLocalInterface)

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "Get instance of iPMUReinsurance.Interface failed")
            End If

            Dim nTask As PMEComponentAction
            If m_iTask = PMEComponentAction.PMView Then
                nTask = PMEComponentAction.PMView
            Else
                nTask = PMEComponentAction.PMAdd
            End If

            ' set process modes
            nReturn = oReinsurance.SetProcessModes(vTask:=nTask, vTransactionType:=m_sTransactionType)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "Get instance of iPMUReinsurance.Interface.setprocessmodes failed")
            End If

            ' set properties
            oReinsurance.InsuranceFileCnt = m_lInsuranceFileCnt
            oReinsurance.RiskID = m_lRiskId
            oReinsurance.IsSilentQuote = bIsSilentQuote

            nReturn = oReinsurance.Start() ' start interface
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "Get instance of iPMUReinsurance.Interface.start failed")
            End If
            m_lStatus = oReinsurance.Status

            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            oReinsurance.Dispose()
            oReinsurance = Nothing

        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_bCanAutoQuoteRisks"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Function CanAutoQuoteRisks(ByRef r_bCanAutoQuoteRisks As Boolean, ByRef r_bAllRisksQuoted As Boolean) As Integer

    '    Const kMethodName As String = "CanAutoQuoteRisks"
    '    Dim bIsAutoRated As Boolean = False
    '    Dim nReturn As Integer = 0
    '    Dim nResult As Integer = PMEReturnCode.PMTrue
    '    Dim bAllRisksQuoted As Boolean

    '    Try
    '        bIsAutoRated = True
    '        bAllRisksQuoted = True

    '        'nReturn = GetPolicyRisksForAutoQuote()
    '        If nReturn <> PMEReturnCode.PMTrue Then
    '            Throw New Exception(kMethodName + " : " + "GetPolicyRisksForAutoQuote Failed")
    '        End If

    '        Dim nLoop As Integer = 0
    '        If IsArray(m_oaRisksForAutoQuote) Then
    '            'Go through the risks and try to quote if it is not already quoted
    '            For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
    '                If ToSafeString(m_oaRisksForAutoQuote(kIsAutoRated, nLoop), "") <> 1 Then
    '                    bIsAutoRated = False
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        'If all risks has rating rules attached, check whether all risks are quoted
    '        If bIsAutoRated Then
    '            For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
    '                If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop), ""))) <> "QUOTED" Then
    '                    bAllRisksQuoted = False
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        Return nResult
    '    Catch ex As Exception
    '        Throw New Exception(kMethodName + " Failed", ex)
    '    Finally
    '        r_bCanAutoQuoteRisks = bIsAutoRated
    '        r_bAllRisksQuoted = bAllRisksQuoted
    '    End Try

    'End Function

    ''' <summary>
    ''' GetPolicyRisksForNoChange
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyRisksForNoChange(Optional ByVal v_nRiskId As Integer = 0) As Integer
        Const kMethodName As String = "GetPolicyRisksForNoChange"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            nReturn = m_oBusiness.GetPolicyRisksForNoChange(nInsuranceFileCnt:=m_lInsuranceFileCnt, v_nRiskId:=v_nRiskId, r_oResults:=m_oaRisksForAutoQuote)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "bSIRListRisks.GetPolicyRisksForNoChange Failed")
            End If
            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try
    End Function

    ''' <summary>
    ''' Tries to auto quote the risks of given policy
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessQuoteSingleRisks() As Integer
        Const kMethodName As String = "ProcessQuoteAllRisks"
        Dim nReturn As Integer = 0
        Dim nLoop As Integer = 0
        Dim bAllRisksQuoted As Boolean = False
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim m_oIsRI2007 As Object
        Dim bIsSilentQuote As Boolean = True
        Dim bDisplayReinsurance As Boolean = False

        Try
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            nReturn = GetPolicyRisksForNoChange(m_lRiskId)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetPolicyRisksForNoChange Failed")
            End If

            nReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + " - Save Cover Notes Failed")
            End If

            If m_iTask <> PMEComponentAction.PMView Then
                Call PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonEditRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return nReturn
                End If
            End If

            nReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=m_oIsRI2007)
            nReturn = m_oBusiness.GetUserAuthorityDisplayReinsurance(v_nUserID:=g_iUserID, r_bDisplayReinsurance:=bDisplayReinsurance)
            If m_oIsRI2007 IsNot Nothing AndAlso m_oIsRI2007 = "1" AndAlso bDisplayReinsurance Then
                bIsSilentQuote = False
            End If

            m_bAllRiskQuoted = True
            If IsArray(m_oaRisksForAutoQuote) Then
                nReturn = QuoteRisk(nArrayIndex:=0, bIsSilentQuote:=bIsSilentQuote)
            End If
            If m_bAllRiskQuoted Then
                btnNOChange.Enabled = False
            End If
            If uctPMUListRisk1.AllRiskStatusQuoted Then
                btnNoChangeAll.Enabled = False
            Else
                btnNoChangeAll.Enabled = True
            End If
            ' get the latest version of the risks before recalculating
            nReturn = uctPMUListRisk1.GetRisks
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetRisks Failed")
            End If
            If uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired Then
                cmdBackdateMTA.Visible = True
            Else
                cmdBackdateMTA.Visible = False
            End If

            ' recalculate everything from policy down
            nReturn = Recalculate(kRecalculateModeRisks)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Recalculate Failed", PMELogLevel.PMLogError)
                Throw New Exception(kMethodName + " : " + "Recalculate Failed")
            End If

            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            Call uctPMUListRisk1.GetRisks()
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Function

    ''' <summary>
    ''' Tries to auto quote a given risk
    ''' </summary>
    ''' <param name="nArrayIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuoteRisk(ByVal nArrayIndex As Integer, ByVal bIsSilentQuote As Boolean) As Integer
        Const kMethodName As String = "QuoteRisk"
        Dim nReturn As Integer = 0
        Dim oRiskInterface As Object = Nothing
        Dim oCurRiskId(,) As Object = Nothing
        Dim sSysOptionBDMTA As String = String.Empty
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            If IsArray(m_oaRisksForAutoQuote) Then

                'Quote the selected risk
                oRiskInterface = Nothing

                m_lRiskId = ToSafeInteger(m_oaRisksForAutoQuote(kRiskID, nArrayIndex), "0")

                If b_IsMakeLiveClicked Then
                    'If b_IsMakeLiveClicked=True then get the current risk id for the original risk id
                    nReturn = m_oBusiness.GetCurRiskIdtForOriginalRiskId(v_lOriginalRiskId:=m_lRiskId,
                                                                r_vCurRiskId:=oCurRiskId)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception(kMethodName + " : " + "Unable to get the current risk id.")
                    End If

                    If IsArray(oCurRiskId) Then
                        m_lRiskId = ToSafeLong(oCurRiskId(0, 0))
                    End If
                End If

                ' create instance of interface
                nReturn = g_oObjectManager.GetInstance(oObject:=oRiskInterface,
                                                       sClassName:="iPMURisk.Interface_Renamed",
                                                       vInstanceManager:=PMGetLocalInterface)
                If nReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kMethodName + " : " + "Failed to get an instance of IPMURisk.Interface")
                End If

                ' set interface properties
                oRiskInterface.PartyCnt = m_lPartyCnt
                oRiskInterface.ShortName = m_sShortName
                oRiskInterface.InsuranceFileCnt = ToSafeLong(m_oaRisksForAutoQuote(kInsuranceFileID, nArrayIndex), "0")
                oRiskInterface.InsuranceFolderCnt = ToSafeLong(m_oaRisksForAutoQuote(kInsuranceFolderID, nArrayIndex), "0")
                oRiskInterface.RiskId = m_lRiskId
                oRiskInterface.ScreenId = ToSafeLong(m_oaRisksForAutoQuote(kScreenID, nArrayIndex), "0")
                oRiskInterface.ProductID = ToSafeLong(m_oaRisksForAutoQuote(kProductID, nArrayIndex), "0")
                oRiskInterface.RiskTypeId = ToSafeLong(m_oaRisksForAutoQuote(kRiskTypeID, nArrayIndex), "0")
                oRiskInterface.CopyRisk = m_bCopyRisk
                oRiskInterface.IsSilentQuote = True

                ' set process modes
                nReturn = oRiskInterface.SetProcessModes(vTask:=m_iTask,
                                        vTransactionType:=m_sTransactionType)
                If nReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.SetProcessModes Failed")
                End If

                ' start interface
                nReturn = oRiskInterface.Start
                If nReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.Start Failed")
                End If

                'Reset the flag
                m_bCopyRisk = False

                ' get return details
                m_lRiskId = oRiskInterface.RiskId
                m_iIsRiAtRiskLevel = oRiskInterface.IsRiAtRiskLevel
                m_lStatus = oRiskInterface.Status

                ' terminate interface
                nReturn = oRiskInterface.Dispose()

                ' destroy interface
                oRiskInterface = Nothing

                ' if the risk interface was cancelled
                If (m_lStatus <> PMEReturnCode.PMCancel) Then

                    ' get risk ratings
                    nReturn = GetRiskRating(m_iTask, bIsSilentQuote)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception(kMethodName + " : " + "GetRiskRating Failed")
                    End If

                    ' if the risk rating wasnt cancelled
                    If (m_lStatus <> PMEReturnCode.PMCancel) Then

                        ' get risk reinsurace
                        nReturn = GetRiskReinsurance(bIsSilentQuote)
                        If nReturn <> PMEReturnCode.PMTrue Then
                            Throw New Exception(kMethodName + " : " + "GetRiskReinsurance Failed")
                        End If

                        ' if risk reinsurance wasnt cancelled
                        If (m_lStatus <> PMEReturnCode.PMCancel) Then

                            ' indicate to the the user control that the risk was successfully editted.
                            uctPMUListRisk1.RiskID = m_lRiskId
                            uctPMUListRisk1.Editted = True

                            If uctPMUListRisk1.RiskStatus.ToUpper = "UNQUOTED" AndAlso m_bAllRiskQuoted Then
                                m_bAllRiskQuoted = False
                            End If

                            'Block Risk for posting which are not edited   PN 35099
                            nReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)

                            If nReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception(kMethodName + " : " + "UpdateIFRLInkRisk Failed")
                            End If
                        End If
                    End If
                End If
            End If
            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            If Not oRiskInterface Is Nothing Then
                nReturn = oRiskInterface.Terminate
                If nReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.Terminate Failed")
                End If
                oRiskInterface = Nothing
            End If
            Call uctPMUListRisk1.GetRisks()
        End Try
    End Function
    Private Sub btnNOChange_Click(sender As Object, e As EventArgs) Handles btnNOChange.Click
        Const kMethodName As String = "btnNOChange_Click"
        Try
            ProcessQuoteSingleRisks()
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName,
                               iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kMethodName + " Failed",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName,
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=ex.Message, excep:=ex)
        End Try

    End Sub
    ''' <summary>
    ''' ProcessQuoteAllRisks
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessQuoteAllRisks() As Integer
        Const kMethodName As String = "ProcessQuoteAllRisks"
        Dim nReturn As Integer = 0
        Dim nLoop As Integer = 0
        Dim bAllRisksQuoted As Boolean = False
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            nReturn = GetPolicyRisksForNoChange()
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetPolicyRisksForNoChange Failed")
            End If

            nReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + " - Save Cover Notes Failed")
            End If

            If m_iTask <> PMEComponentAction.PMView Then
                Call PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonEditRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Return nResult
                End If
            End If

            m_bAllRiskQuoted = True
            If IsArray(m_oaRisksForAutoQuote) Then
                Dim nRiskIndex As Integer
                Dim bMandatoryRiskPresent As Boolean = False
                'Go through the risks and try to quote if it is not already quoted
                For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
                    If ToSafeInteger(m_oaRisksForAutoQuote(kIsMandatoryRisk, nLoop)) = 1 Then
                        nRiskIndex = nLoop
                        bMandatoryRiskPresent = True
                    Else
                        If ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop)).ToUpper.Trim <> "QUOTED" AndAlso ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop)).ToUpper.Trim <> "DELETED" Then
                            nReturn = QuoteRisk(nArrayIndex:=nLoop, bIsSilentQuote:=True)
                        End If
                    End If
                    'even if the QuoteRisk fails, the loop should continue
                Next
                If m_bAllRiskQuoted AndAlso bMandatoryRiskPresent Then
                    nReturn = QuoteRisk(nArrayIndex:=nRiskIndex, bIsSilentQuote:=True)
                End If
            End If
            If m_bAllRiskQuoted AndAlso uctPMUListRisk1.AllRiskStatusQuoted Then
                btnNoChangeAll.Enabled = False
                btnNOChange.Enabled = False
            End If

            ' get the latest version of the risks before recalculating
            nReturn = uctPMUListRisk1.GetRisks
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetRisks Failed")
            End If
            If uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired Then
                cmdBackdateMTA.Visible = True
            Else
                cmdBackdateMTA.Visible = False
            End If

            ' recalculate everything from policy down
            nReturn = Recalculate(kRecalculateModeRisks)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Recalculate Failed", PMELogLevel.PMLogError)
                Throw New Exception(kMethodName + " : " + "Recalculate Failed")
            End If

            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            Call uctPMUListRisk1.GetRisks()
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Function

    Private Sub btnNoChangeAll_Click(sender As Object, e As EventArgs) Handles btnNoChangeAll.Click
        Const kMethodName As String = "btnNoChangeAll_Click"
        Try
            ProcessQuoteAllRisks()
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName,
                               iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kMethodName + " Failed",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName,
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=ex.Message, excep:=ex)
        End Try
    End Sub

    ''' <summary>
    ''' cmdQuoteAllRisks_Click Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdQuoteAllRisks_Click(sender As Object, e As EventArgs) Handles cmdQuoteAllRisks.Click
        Const kMethodName As String = "cmdQuoteAllRisks_Click"
        Try
            ProcessAutoQuoteAllRisks()
            MsgBox("The Quote All Risks process is now complete – please review any unquoted risks (if applicable)", vbOKOnly, "Risk Quoted")
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName,
                               iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kMethodName + " Failed",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName,
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=ex.Message, excep:=ex)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_bCanAutoQuoteRisks"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CanAutoQuoteRisks(ByRef r_bCanAutoQuoteRisks As Boolean, ByRef r_bAllRisksQuoted As Boolean) As Integer

        Const kMethodName As String = "CanAutoQuoteRisks"
        Dim bIsAutoRated As Boolean = False
        Dim nReturn As Integer = 0
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bAllRisksQuoted As Boolean

        Try
            bIsAutoRated = False
            bAllRisksQuoted = True

            nReturn = GetPolicyRisksForAutoQuote()
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetPolicyRisksForAutoQuote Failed")
            End If

            Dim nLoop As Integer = 0
            If IsArray(m_oaRisksForAutoQuote) Then
                'Go through the risks and try to quote if it is not already quoted
                For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
                    If ToSafeString(m_oaRisksForAutoQuote(kIsAutoRated, nLoop), "") = 1 Then
                        bIsAutoRated = True
                        Exit For
                    End If
                Next
            End If

            'If all risks has rating rules attached, check whether all risks are quoted
            If bIsAutoRated Then
                If IsArray(m_oaRisksForAutoQuote) Then
                    For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
                        If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop), ""))) <> "QUOTED" Then
                            bAllRisksQuoted = False
                            Exit For
                        End If
                    Next
                End If
            End If

            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            r_bCanAutoQuoteRisks = bIsAutoRated
            r_bAllRisksQuoted = bAllRisksQuoted
        End Try

    End Function

    ''' <summary>
    ''' GetPolicyRisksForAutoQuote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyRisksForAutoQuote() As Integer
        Const kMethodName As String = "GetPolicyRisksForAutoQuote"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            nReturn = m_oBusiness.GetPolicyRisksForAutoQuote(nInsuranceFileCnt:=m_lInsuranceFileCnt, r_oResults:=m_oaRisksForAutoQuote)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "bSIRListRisks.GetPolicyRisksForAutoQuote Failed")
            End If
            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try
    End Function

    ''' <summary>
    ''' Tries to auto quote the risks of given policy
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessAutoQuoteAllRisks() As Integer
        Const kMethodName As String = "ProcessAutoQuoteAllRisks"
        Dim nReturn As Integer = 0
        Dim nLoop As Integer = 0
        Dim bAllRisksQuoted As Boolean = False
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim bUnquoteMandatoryRisk As Boolean = False
        Try
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            nReturn = GetPolicyRisksForAutoQuote()
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetPolicyRisksForAutoQuote Failed")
            End If

            nReturn = uctPMUListRisk1.SaveAllCoverNotesOnQuote
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + " - Save Cover Notes Failed")
            End If

            If m_iTask <> PMEComponentAction.PMView Then
                Call PolicyDiscountRollbackWrapper(kPolicyDiscountRollbackReasonEditRisk)
                If m_bCancelAboutToChangeAction Then
                    m_bCancelAboutToChangeAction = False
                    Exit Function
                End If
            End If

            If IsArray(m_oaRisksForAutoQuote) Then
                'Go through the risks and try to quote if it is not already quoted
                For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
                    If ToSafeInteger(m_oaRisksForAutoQuote(kIsMandatoryRisk, nLoop)) <> 1 Then
                        nReturn = AutoQuoteRisk(nArrayIndex:=nLoop)
                    End If
                    'even if the QuoteRisk fails, the loop should continue
                Next
            End If

            'Check all risks have been quoted. If yes, disable the quote all risks button.
            bAllRisksQuoted = True
            nReturn = GetPolicyRisksForAutoQuote()

            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetPolicyRisksForAutoQuote Failed")
            End If
            Dim nCountREVIEWFAC As Integer = 0
            Dim nCountUnQuote As Integer = 0
            If IsArray(m_oaRisksForAutoQuote) Then
                'Go through the risks and try to quote if it is not already quoted
                For nLoop = LBound(m_oaRisksForAutoQuote, 2) To UBound(m_oaRisksForAutoQuote, 2)
                    If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop), ""))) = "REVIEWFAC" Then
                        nCountREVIEWFAC = nCountREVIEWFAC + 1
                    End If
                    If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop), ""))) = "UNQUOTED" Then
                        nCountUnQuote = nCountUnQuote + 1
                    End If
                    If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nLoop), ""))) = "QUOTED" Then
                        bAllRisksQuoted = True
                    Else
                        bAllRisksQuoted = False
                    End If
                Next
            End If

            'If all risks are quoted then quote the mandatory risk
            If bAllRisksQuoted = True Then
                AutoQuoteRisk(nArrayIndex:=0)
                nCountUnQuote = 0
            End If

            If bAllRisksQuoted Then
                If nCountUnQuote > 0 Then
                    cmdQuoteAllRisks.Enabled = True
                Else
                    cmdQuoteAllRisks.Enabled = False
                End If
            Else
                If nCountREVIEWFAC > 0 Then
                    If nCountUnQuote > 0 Then
                        cmdQuoteAllRisks.Enabled = True
                    Else
                        cmdQuoteAllRisks.Enabled = False
                    End If
                Else
                    If nCountUnQuote > 0 Then
                        cmdQuoteAllRisks.Enabled = True
                    End If
                End If
            End If

            ' get the latest version of the risks before recalculating
            nReturn = uctPMUListRisk1.GetRisks
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + "GetRisks Failed")
            End If
            If uctPMUListRisk1.OKToProceed And m_bIsBackdatedMTARequired Then
                cmdBackdateMTA.Visible = True
            Else
                cmdBackdateMTA.Visible = False
            End If
            ' recalculate everything from policy down
            nReturn = Recalculate(kRecalculateModeRisks)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Recalculate Failed", PMELogLevel.PMLogError)
                Throw New Exception(kMethodName + " : " + "Recalculate Failed")
            End If

            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            Call uctPMUListRisk1.GetRisks()
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Function

    ''' <summary>
    ''' Tries to auto quote a given risk
    ''' </summary>
    ''' <param name="nArrayIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoQuoteRisk(nArrayIndex As Integer) As Integer
        Const kMethodName As String = "AutoQuoteRisk"
        Const kFACRiskStatusCode As String = "REVIEWFAC"
        Dim nReturn As Integer = 0
        Dim oRiskInterface As Object = Nothing
        Dim oCurRiskId(,) As Object = Nothing
        Dim sSysOptionBDMTA As String = String.Empty
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            If IsArray(m_oaRisksForAutoQuote) Then
                If UCase(Trim(ToSafeString(m_oaRisksForAutoQuote(kRiskStatusCode, nArrayIndex), ""))) = "UNQUOTED" Then

                    '------PLICO Backdate Changes- Start-----------
                    'get system option "Apply Back-Dated Risk Editing Restrictions"
                    nReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5070, r_sOptionValue:=sSysOptionBDMTA, v_iSourceID:=g_iSourceID)
                    If nReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception(kMethodName + " : " + "Failed to get System Option [5070]")
                    End If

                    If sSysOptionBDMTA = "1" Then

                        If m_bIsBackdatedMTARequired And Not (m_sTransactionType = "MTC" Or m_iInsuranceFileTypeID = kInsFileTypeMTATempQuote) Then
                            nReturn = IsSubsequentRiskVersionsEdited(v_lRiskID:=m_lRiskId,
                                                      v_dtMTAEffectiveDate:=m_dtCoverStartDate)
                            If nReturn = PMEReturnCode.PMTrue Then
                                AutoQuoteRisk = PMEReturnCode.PMFalse
                                Exit Function
                            End If
                        End If
                    End If
                    '------PLICO Backdate Changes- End-----------

                    'If the risk has FAC Reinsurer, changes the status to "Unquoted - Review Reinsurance FAC applied"
                    If ToSafeInteger(m_oaRisksForAutoQuote(kIsAutoRated, nArrayIndex), "0") = 1 OrElse (nArrayIndex = 0 AndAlso ToSafeInteger(m_oaRisksForAutoQuote(kIsMandatoryRisk, nArrayIndex)) = 1) Then


                        If ToSafeInteger(m_oaRisksForAutoQuote(kHasFACRI, nArrayIndex), "0") = 1 Then
                            nReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=m_oaRisksForAutoQuote(kRiskID, nArrayIndex),
                                                               v_sRiskStatusCode:=kFACRiskStatusCode)

                            If nReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception(kMethodName + " : " + "bSIRListRisks.UpdateRiskStatus Failed")
                            End If
                        Else
                            'Quote the selected risk
                            oRiskInterface = Nothing

                            m_lRiskId = ToSafeInteger(m_oaRisksForAutoQuote(kRiskID, nArrayIndex), "0")

                            If b_IsMakeLiveClicked Then
                                'If b_IsMakeLiveClicked=True then get the current risk id for the original risk id
                                nReturn = m_oBusiness.GetCurRiskIdtForOriginalRiskId(v_lOriginalRiskId:=m_lRiskId,
                                                                        r_vCurRiskId:=oCurRiskId)
                                If nReturn <> PMEReturnCode.PMTrue Then
                                    Throw New Exception(kMethodName + " : " + "Unable to get the current risk id.")
                                End If

                                If IsArray(oCurRiskId) Then
                                    m_lRiskId = ToSafeLong(oCurRiskId(0, 0))
                                End If
                            End If

                            ' create instance of interface
                            nReturn = g_oObjectManager.GetInstance(oObject:=oRiskInterface,
                                                               sClassName:="iPMURisk.Interface_Renamed",
                                                               vInstanceManager:=PMGetLocalInterface)
                            If nReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception(kMethodName + " : " + "Failed to get an instance of IPMURisk.Interface")
                            End If

                            ' set interface properties
                            oRiskInterface.PartyCnt = m_lPartyCnt
                            oRiskInterface.ShortName = m_sShortName
                            oRiskInterface.InsuranceFileCnt = ToSafeLong(m_oaRisksForAutoQuote(kInsuranceFileID, nArrayIndex), "0")
                            oRiskInterface.InsuranceFolderCnt = ToSafeLong(m_oaRisksForAutoQuote(kInsuranceFolderID, nArrayIndex), "0")
                            oRiskInterface.RiskId = m_lRiskId
                            oRiskInterface.ScreenId = ToSafeLong(m_oaRisksForAutoQuote(kScreenID, nArrayIndex), "0")
                            oRiskInterface.ProductID = ToSafeLong(m_oaRisksForAutoQuote(kProductID, nArrayIndex), "0")
                            oRiskInterface.RiskTypeId = ToSafeLong(m_oaRisksForAutoQuote(kRiskTypeID, nArrayIndex), "0")
                            oRiskInterface.CopyRisk = m_bCopyRisk
                            oRiskInterface.IsSilentQuote = True

                            ' set process modes
                            nReturn = oRiskInterface.SetProcessModes(vTask:=m_iTask,
                                                vTransactionType:=m_sTransactionType)
                            If nReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.SetProcessModes Failed")
                            End If

                            ' start interface
                            If ToSafeInteger(m_oaRisksForAutoQuote(kIsMandatoryRisk, nArrayIndex)) <> 1 Then
                                nReturn = oRiskInterface.Start
                            End If
                            If nReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.Start Failed")
                            End If

                            'Reset the flag
                            m_bCopyRisk = False

                            ' get return details
                            m_lRiskId = oRiskInterface.RiskId
                            m_iIsRiAtRiskLevel = oRiskInterface.IsRiAtRiskLevel
                            m_lStatus = oRiskInterface.Status

                            ' terminate interface
                            nReturn = oRiskInterface.Dispose()

                            ' destroy interface
                            oRiskInterface = Nothing

                            ' if the risk interface was cancelled
                            If (m_lStatus <> PMEReturnCode.PMCancel) Then

                                ' get risk ratings
                                nReturn = GetRiskRating(m_iTask, True)
                                If nReturn <> PMEReturnCode.PMTrue Then
                                    Throw New Exception(kMethodName + " : " + "GetRiskRating Failed")
                                End If

                                ' if the risk rating wasnt cancelled
                                If (m_lStatus <> PMEReturnCode.PMCancel) Then

                                    ' get risk reinsurace
                                    nReturn = GetRiskReinsurance(True)
                                    If nReturn <> PMEReturnCode.PMTrue Then
                                        Throw New Exception(kMethodName + " : " + "GetRiskReinsurance Failed")
                                    End If

                                    ' if risk reinsurance wasnt cancelled
                                    If (m_lStatus <> PMEReturnCode.PMCancel) Then

                                        ' indicate to the the user control that the risk was successfully editted.
                                        uctPMUListRisk1.RiskID = m_lRiskId
                                        uctPMUListRisk1.Editted = True

                                        'Block Risk for posting which are not edited   PN 35099
                                        nReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)

                                        If nReturn <> PMEReturnCode.PMTrue Then
                                            Throw New Exception(kMethodName + " : " + "UpdateIFRLInkRisk Failed")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return nResult
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        Finally
            If Not oRiskInterface Is Nothing Then
                nReturn = oRiskInterface.Terminate
                If nReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kMethodName + " : " + "IPMURisk.Interface.Terminate Failed")
                End If
                oRiskInterface = Nothing
            End If
            Call uctPMUListRisk1.GetRisks()
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: EnableDisbleQuoteAllButton
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    ' Created : Rohit Kumar Mishra
    ' ***************************************************************** '
    Sub EnableDisbleQuoteAllButton()
        Dim vResultArray As Object
        Dim lQuoteAllRiskNB As Integer
        Dim lQuoteAllRiskMTC As Integer
        Dim lQuoteAllRiskMTA As Integer
        Dim lQuoteAllRiskRenewal As Integer
        Dim bShowQuoteAllButton As Boolean = False
        Dim bCanAutoQuoteRisks As Boolean
        Dim bAllRisksQuoted As Boolean

        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="Quote_all_risk_NB", r_vProductArray:=vResultArray)
        If Information.IsArray(vResultArray) Then
            lQuoteAllRiskNB = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
        End If

        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="Quote_all_risk_MTC", r_vProductArray:=vResultArray)

        If Information.IsArray(vResultArray) Then
            lQuoteAllRiskMTC = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
        End If

        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="Quote_all_risk_MTA", r_vProductArray:=vResultArray)

        If Information.IsArray(vResultArray) Then
            lQuoteAllRiskMTA = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
        End If
        m_lReturn = m_oProduct.GetProductValue(v_lProductId:=m_lProductId, v_sColumnName:="Quote_all_risk_RENEWAL", r_vProductArray:=vResultArray)
        If Information.IsArray(vResultArray) Then
            lQuoteAllRiskRenewal = gPMFunctions.ToSafeLong(vResultArray(0, 0), 0)
        End If

        cmdQuoteAllRisks.Enabled = False

        m_lReturn = CanAutoQuoteRisks(r_bCanAutoQuoteRisks:=bCanAutoQuoteRisks,
                                            r_bAllRisksQuoted:=bAllRisksQuoted)
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError("EnableDisbaleQuoteAll", "CanAutoQuoteRisks Failed", PMEReturnCode.PMLogError1)
        End If

        If bCanAutoQuoteRisks Then
            If (m_sTransactionType = "MTC" Or m_sTransactionType = "MTR") AndAlso lQuoteAllRiskMTC = 1 Then
                bShowQuoteAllButton = True
            End If

            If m_sTransactionType = "MTA" AndAlso lQuoteAllRiskMTA = 1 Then
                bShowQuoteAllButton = True
            End If

            If m_sTransactionType = "NB" AndAlso lQuoteAllRiskNB = 1 Then
                bShowQuoteAllButton = True
            End If

            If m_sTransactionType = "REN" AndAlso lQuoteAllRiskRenewal = 1 Then
                bShowQuoteAllButton = True
            End If
        End If
        If bShowQuoteAllButton = True Then
            cmdQuoteAllRisks.Enabled = uctPMUListRisk1.QuoteAll
        End If
    End Sub

    ''' <summary>
    ''' DeletePFPremiumFinance
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function DeletePFPremiumFinance() As Integer
        Const kMethodName As String = "DeletePFPremiumFinance"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse
        Try



            nReturn = m_oBusiness.DeletePFPremiumFinance(nInsuranceFileCnt:=m_lInsuranceFileCnt)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "DeletePFPremiumFinance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nReturn

        Catch excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(sUsername:=g_oObjectManager.UserID, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="DeletePFPremiumFinance Failed.", vClass:=ACClass, vMethod:=kMethodName, excep:=excep)
            Return nReturn
            ' If you want to rollback a transaction or something, do it here
        End Try
        Return nReturn

    End Function
    'Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    'Added one more parameter r_bAllRisksQuoted to indicate whether all risk has status "Quoted"
    Public Function CanAutoQuoteRisks(ByRef r_bCanAutoQuoteRisks As Boolean) As Integer
        Const kMethodName As String = "CanAutoQuoteRisks"
        Dim bIsAutoRated As Boolean
        Dim lLoop As Long
        Dim lReturn As Integer

        Try
            CanAutoQuoteRisks = gPMConstants.PMEReturnCode.PMTrue
            bIsAutoRated = True

            lReturn = GetPolicyRisksForAutoQuote()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPolicyRisksForAutoQuote Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If IsArray(m_vRisksForAutoQuote) Then
                'Go through the risks, Check If all risks has rating rules attached,
                For lLoop = LBound(m_vRisksForAutoQuote, 2) To UBound(m_vRisksForAutoQuote, 2)
                    If ToSafeString(m_vRisksForAutoQuote(ACReRateIsAutoRated, lLoop), "") <> 1 Then
                        bIsAutoRated = False
                        Exit For
                    End If
                Next
            Else
                bIsAutoRated = False
            End If
        Catch
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CanAutoQuoteRisks)
            bIsAutoRated = False
        Finally
            r_bCanAutoQuoteRisks = bIsAutoRated
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: QuoteRisk
    '
    ' Parameters: v_lArrayIndex
    '
    ' Description: Tries to auto quote the a given risk
    ' ***************************************************************** '
    Public Function QuoteRisk(ByVal v_lArrayIndex As Long) As Long
        Const kMethodName As String = "QuoteRisk"
        Const kFACRiskStatusCode As String = "REVIEWFAC"
        Dim lReturn As Long
        Dim oRiskInterface As Object
        Dim vCurRiskId As Object
        Dim sSysOptionBDMTA As String
        Dim sStatusCode As String

        Try
            QuoteRisk = gPMConstants.PMEReturnCode.PMTrue
            If IsArray(m_vRisksForAutoQuote) Then
                sStatusCode = UCase(Trim(ToSafeString(m_vRisksForAutoQuote(ACReRateRiskStatusCode, v_lArrayIndex), "")))
                'EH011765 If sStatusCode = "UNQUOTED" OrElse sStatusCode = "PENDINGRI" Then

                '------PLICO Backdate Changes- Start-----------
                'get system option "Apply Back-Dated Risk Editing Restrictions"
                lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5070, r_sOptionValue:=sSysOptionBDMTA, v_iSourceID:=g_iSourceID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to get System Option [5056]", gPMConstants.PMELogLevel.PMLogError)
                End If

                If sSysOptionBDMTA = "1" Then
                    If m_bIsBackdatedMTARequired And Not (m_sTransactionType = "MTC" Or m_iInsuranceFileTypeID = kInsFileTypeMTATempQuote) Then
                        lReturn = IsSubsequentRiskVersionsEdited(v_lRiskID:=m_lRiskId,
                                                  v_dtMTAEffectiveDate:=m_dtCoverStartDate)
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            QuoteRisk = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    End If
                End If
                '------PLICO Backdate Changes- End-----------
                'If the risk has FAC Reinsurer, changes the status to "Unquoted - Review Reinsurance FAC applied"
                If ToSafeInteger(m_vRisksForAutoQuote(ACReRateHasFACRI, v_lArrayIndex), "0") = 1 Then
                    lReturn = m_oBusiness.UpdateRiskStatus(
                                            v_lRiskCnt:=m_vRisksForAutoQuote(ACReRateRiskID, v_lArrayIndex),
                                            v_sRiskStatusCode:=kFACRiskStatusCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bSIRListRisks.UpdateRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    'Quote the selected risk
                    oRiskInterface = Nothing

                    m_lRiskId = ToSafeLong(m_vRisksForAutoQuote(ACReRateRiskID, v_lArrayIndex), "0")
                    If b_IsMakeLiveClicked Then
                        'If b_IsMakeLiveClicked=True then get the current risk id for the original risk id
                        lReturn = m_oBusiness.GetCurRiskIdtForOriginalRiskId(v_lOriginalRiskId:=m_lRiskId,
                                                                   r_vCurRiskId:=vCurRiskId)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Unable to get the current risk id.", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        If IsArray(vCurRiskId) Then
                            m_lRiskId = ToSafeLong(vCurRiskId(0, 0))
                        End If
                    End If

                    ' create instance of interface
                    lReturn = g_oObjectManager.GetInstance(
                                            oObject:=oRiskInterface,
                                            sClassName:="iPMURisk.Interface_Renamed",
                                            vInstanceManager:=gPMConstants.PMGetLocalInterface)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of IPMURisk.Interface", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' set interface properties
                    oRiskInterface.PartyCnt = m_lPartyCnt
                    oRiskInterface.ShortName = m_sShortName
                    oRiskInterface.InsuranceFileCnt = ToSafeLong(m_vRisksForAutoQuote(ACReRateInsuranceFileID, v_lArrayIndex), "0")
                    oRiskInterface.InsuranceFolderCnt = ToSafeLong(m_vRisksForAutoQuote(ACReRateInsuranceFolderID, v_lArrayIndex), "0")
                    oRiskInterface.RiskId = m_lRiskId
                    oRiskInterface.ScreenId = ToSafeLong(m_vRisksForAutoQuote(ACReRateScreenID, v_lArrayIndex), "0")
                    oRiskInterface.ProductID = ToSafeLong(m_vRisksForAutoQuote(ACReRateProductID, v_lArrayIndex), "0")
                    oRiskInterface.RiskTypeId = ToSafeLong(m_vRisksForAutoQuote(ACReRateRiskTypeID, v_lArrayIndex), "0")
                    oRiskInterface.CopyRisk = m_bCopyRisk
                    oRiskInterface.IsSilentQuote = True

                    ' set process modes
                    lReturn = oRiskInterface.SetProcessModes(vTask:=m_iTask,
                                            vTransactionType:=m_sTransactionType)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "IPMURisk.Interface.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' start interface
                    lReturn = oRiskInterface.Start
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "IPMURisk.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'Reset the flag
                    m_bCopyRisk = False
                    ' get return details
                    m_lRiskId = oRiskInterface.RiskId
                    m_iIsRiAtRiskLevel = oRiskInterface.IsRiAtRiskLevel
                    m_lStatus = oRiskInterface.Status

                    ' terminate interface
                    lReturn = oRiskInterface.Terminate
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "IPMURisk.Interface.Terminate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    ' destroy interface
                    oRiskInterface = Nothing
                    ' if the risk interface was cancelled
                    If (m_lStatus <> gPMConstants.PMEReturnCode.PMCancel) Then

                        ' get risk ratings
                        lReturn = GetRiskRating(iTask:=m_iTask, bIsSilentQuote:=True)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetRiskRating Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' if the risk rating wasnt cancelled
                        If (m_lStatus <> gPMConstants.PMEReturnCode.PMCancel) Then

                            ' get risk reinsurace
                            lReturn = GetRiskReinsurance(bIsSilentQuote:=True)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            ' if risk reinsurance wasnt cancelled
                            If (m_lStatus <> gPMConstants.PMEReturnCode.PMCancel) Then

                                ' indicate to the the user control that the risk was successfully editted.
                                uctPMUListRisk1.RiskID = m_lRiskId
                                uctPMUListRisk1.Editted = True

                                'Block Risk for posting which are not edited   PN 35099
                                lReturn = m_oBusiness.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskID:=m_lRiskId)

                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "UpdateIFRLInkRisk Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=QuoteRisk)
        Finally
            'Terminate objects if any
            If Not oRiskInterface Is Nothing Then
                ' terminate interface
                lReturn = oRiskInterface.Terminate
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "IPMURisk.Interface.Terminate Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' destroy interface
                oRiskInterface = Nothing
            End If
            ' last thing to do in every circumstance is get the risks again
            Call uctPMUListRisk1.GetRisks()
        End Try
    End Function
    ''' <summary>
    ''' Run Renewal Selection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReRunRenewalSelection() As Integer
        Const kMethodName As String = "ReRunRenewalSelection"
        Dim nResult As Integer = 0

        Try
            m_oProgressBar = New iPMBProgressBarWrapper.Wrapper()
        Catch ex As Exception
        End Try

        If (m_oProgressBar Is Nothing) = False Then
            m_oProgressBar.Caption = "  Running Renewal Selection . . ."
            m_oProgressBar.Text = "It may take several minutes to process. Please wait..."
            m_oProgressBar.StartBar = True
        End If

        Try
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oRenSelection.DeleteRenewalsForPolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To Delete Existing Renewal For Selected Policy", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_oRenSelection.DelRenewalReport() <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To Delete Existing Renewal Report", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim vRenewalPolicy(,) As Object
            m_lReturn = m_oRenSelection.GetPolicyForRenewal(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_vResultArray:=vRenewalPolicy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To Get Correct Version Of Policy For Renewal", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim nNewInsuranceFileCnt As Integer
            If Information.IsArray(vRenewalPolicy) Then
                m_lReturn = m_oRenSelection.CreateRenewalPolicyWrapper(r_vRenewalList:=vRenewalPolicy, v_lCount:=0, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, r_sFailureCriterion:="")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed To Create Renewal Policy Wrapper", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = m_oRenSelection.CreateTMPAnniversaryRenewal(r_vRenewalList:=vRenewalPolicy, v_lCount:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To Create TMPAnniversary Renewal", gPMConstants.PMELogLevel.PMLogError)
            End If

            If (m_oProgressBar Is Nothing) = False Then
                m_oProgressBar.StopBar = True
            End If

            m_lReturn = PrintRenewalReport()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To Print Renewal Report", gPMConstants.PMELogLevel.PMLogError)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return nResult

        End Try

    End Function
    ''' <summary>
    ''' Print Renewal Report
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PrintRenewalReport() As Integer
        Dim nResult As Integer = 0
        Dim bManualRenewalsExist As Boolean = False
        Dim bAutoRenewalsExist As Boolean = False
        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vReportKeys As Object
        Const PMReportAutoRenewal As String = "AutomaticRenewal"
        Const PMReportManualRenewal As String = "ManualRenewal"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oReport As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If Not (oReport Is Nothing) Then
                m_lReturn = m_oRenSelection.RenewalsReportExists("ManualRenewal", bManualRenewalsExist)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bManualRenewalsExist Then
                    ReDim vReportKeys(1, 2)

                    vReportKeys.SetValue("report_name", 0, 0)

                    vReportKeys.SetValue(PMReportManualRenewal, 1, 0)

                    vReportKeys.SetValue("report_print_options", 0, 1)

                    vReportKeys.SetValue(0, 1, 1)
                    vReportKeys.SetValue("param_name1", 0, 2)
                    vReportKeys.SetValue(g_oObjectManager.UserName, 1, 2)

                    m_lReturn = oReport.SetKeys(vReportKeys)

                    m_lReturn = oReport.Start

                End If

                m_lReturn = m_oRenSelection.RenewalsReportExists("AutoRenewal", bAutoRenewalsExist)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                If bAutoRenewalsExist Then

                    ReDim vReportKeys(1, 2)

                    vReportKeys.SetValue("report_name", 0, 0)

                    vReportKeys.SetValue(PMReportAutoRenewal, 1, 0)

                    vReportKeys.SetValue("report_print_options", 0, 1)

                    vReportKeys.SetValue(0, 1, 1)
                    vReportKeys.SetValue("param_name1", 0, 2)
                    vReportKeys.SetValue(g_oObjectManager.UserName, 1, 2)

                    m_lReturn = oReport.SetKeys(vReportKeys)
                    m_lReturn = oReport.Start

                End If

            End If

            If Not (bManualRenewalsExist OrElse bAutoRenewalsExist) Then
                MessageBox.Show("No data found for current criteria.", "Renewal Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Print Renewal Report", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try
    End Function
End Class
