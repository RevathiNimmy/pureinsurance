Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmList
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmList
    '
    ' Date: 11th July 1997
    '
    ' Description: Main interface. Shows List of available details.
    '
    ' Edit History:
    ' 12/01/05 CJB - Perkins Slade Retail Logic integration
    '                Added processing of CCname, CCcustomer, CCmanualauthcode,
    '                CCtransactioncode, MediaTypeIssuerID in DetailsFormToData,
    '                BusinessToData, DataToBusiness and DataToDetailsForm.
    ' 18/02/05 CJB - PN18882 Do not set PaymentExpiryDate on frmDetails (not used yet)
    ' 29/07/05 CJB - PN22728 Changed SetDefaultAccountProperties to cater for no account selected.
    ' 16/09/05 CJB - PN24096 Changed ProcessLetters to check if template has not been specified in
    '                Maintain System Options before proceeding and assuming there was!
    ' 18/10/05 CJB - PN24911 Changed all places where cmdAllocate.Visible was set to True as we
    '                shouldn't do this if a) We have come via Client Manager b) Client Manager
    '                Security System Option has been turned on AND c) the user does not have
    '                allocate priviledges.
    ' ***************************************************************** '

    Dim frmDetails As frmDetails

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmList"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    Public bStopCheque As Boolean

    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Process status of navigator
    Private m_bProcessComplete As Boolean
    Private m_lAllocationProcessID As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sNavigatorTitle As String = ""
    Private m_lAccountID As Integer
    ' Stores the last business data ID used
    Private m_lLastDataID As Integer
    Private m_lCashlistID As Integer
    Private m_lSelectedRow As Integer
    Private m_lCashListItemID As Integer
    Private m_lCashListTypeID As Integer
    Private m_sAccountShortCode As String = ""
    Private m_iCurrencyID As Integer
    Private m_lAllocationID As Integer
    Private m_vAllocationIDs() As Object
    Private m_sCashListRoadmap As String = ""
    Private m_lBatchID As Integer
    Private m_cAmount As Decimal
    Private m_cWriteOffAmount As Decimal
    Private m_bWOFF As Boolean
    Private m_bAllocated As Boolean
    Private m_iCompanyID As Integer
    Private m_bLetter As Boolean
    Private m_vLetters(,) As Object
    Private m_bViaInsurerPayment As Boolean
    Private m_bViaFinancePlan As Boolean
    Private m_bViaClaimPayment As Boolean 'AR20050125 - PN18271
    Private m_sLedgerCode As String = "" 'AR20070222 - PN33413
    Private m_bCashDrawerType As Boolean
    Private m_vBatchID As Integer
    Private m_vCashDrawerID As Integer
    Private m_vFutureChequeDays As Integer
    Private m_vDrawerReceiptType As String = ""
    Private m_vDrawerMediaType As String = ""
    Private m_vAllowReversals As String = ""
    Private m_vCashDrawerClosed As Integer
    Private m_vCashDrawerDescription As String = ""
    Private m_vTaskIsUrgent As Object
    Private m_vTaskStatus As Object
    Private m_vTaskDueDays As Object
    Private m_vPMUserGroupId As String = ""
    Private m_vGenerateTask As Object
    Private m_sActionkey As String = ""
    Private m_bViaBanking As Boolean
    Private m_vInsurerPaymentInstArray As Object
    Private m_iCashListItemReceiptTypeID As Integer
    ' SplitReceipt = True , ViaInsurerPayment = True 
    ' This will be the default InsurerAccountID when we come through InsurerPayment
    Private m_iInsurerAccountID As Integer

    Private m_iDocumentsID As Integer
    Private m_bOnLoad As Boolean = False
    ' {* USER DEFINED CODE (End) *}
    ' Instance of Navigator
    Private WithEvents m_oNav As iPMNavigator.NavigateControl

    'EK 20/12/99

    Private m_oInsurerPaymentAllocateBusiness As bACTInsurerPaymentAllocate.Business
    '
    'EK 100100
    ' Currency conversion instance
    Private m_oCCYConvert As Object

    ' Declare an instance of the Business object.

    Private m_oBusiness As bACTCashlistitem.Form
    Private m_oCashListPost As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the List data from the business object.
    Private m_vListData(,) As Object

    Private m_lDisplayState As FormShowConstants

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' Private variable
    Private m_lPMAuthorityLevel As Integer

    Private m_bAllowAllocateButton As Boolean
    Private m_bPosted As Boolean
    Private m_bCashListCreated As Boolean
    Private m_cTotalAmount As Decimal
    Private m_lCashTransDetailID As Integer
    Private m_oStepAuthorization As Object

    'Required for Credit Card processing
    Private m_lInsuranceFileCnt As Integer
    Private m_lGISSchemeID As Integer
    Private m_sGISDataModelCode As String = ""

    Private m_bLossSchedule As Boolean
    Private m_lCashlistitemPaymentStatusId As Integer

    'Added for Manage Debtors development

    Private m_iCashListItemPaymentTypeID As Integer '' PN 3733

    Private m_iCashListStatusId As Integer

    'sw 30/01/2003
    Private m_sReceiptPolicyRef As String = ""

    Private bMessageDisplayed As Boolean

    ' AMB 21/02/2003: PS220 - added keys for Manage Debtors development
    Private m_sPaymentName As String = ""
    Private m_lMediaTypeID As Integer
    Private m_sOurRef As String
    Private m_sTheirRef As String

    'SMJB CQ850 - 11/07/03
    Private m_iSourceForm As Integer
    Const ACSourceFindCashList As Integer = 1

    'DD 25/09/2003: CQ850
    Private m_bHasSecurityAccess As Boolean

    '***********
    ' MEvans : 14-05-2003 : CQ 709
    Private m_lApprovalType As Integer
    '***********

    'SMJB CQ2155 02/09/03
    Private m_sMediaRef As String = ""

    Private m_sDocumentRef As String = ""

    ' START CHANGES - Changed By: AAB  - Changed On: 03-Dec-2003 11:25
    Private Const ACClaimPaymentsType As Integer = 1
    Private Const ACPaymentsType As Integer = 2
    ' END CHANGES - Changed By: AAB  - Changed On: 03-Dec-2003 11:25

    'AR20050203 - PN18491 Constants for AutoAllocation
    Private Const cAUTO_ALLOCATE_BUTTON As Integer = 1
    Private Const cAUTO_ALLOCATE_AUTOMATIC As Integer = 2

    'AR20050125 - PN18271
    Private m_sPayeeName As String = ""
    Private m_sPayeeAccountCode As String = ""
    Private m_sPayeeSortCode As String = ""
    Private m_sPayeeComments As String = ""
    Private m_sForRecommendation As String = ""
    Private m_vClaimPaymentIDs As Object

    Private m_oAllocation As iACTAllocation.Interface_Renamed

    'DD Performance change
    Private m_oUserAuthorities As Object

    Private m_oAccount As bACTAccount.Form
    Private m_vDocumentIds As Object
    Private m_sScreenType As String = ""
    Private m_lClaimPaymentId As Integer

    Private m_iDocumentsCurrencyID As Integer

    Private m_bSilentMultiCurrencyScreen As Boolean
    Private m_bApprovalMsg As Boolean
    Private m_vSelectdBGPoliciesItem(,) As Object
    Private m_vBGAllPoliciesForReceipt As Object

    'WPR12- Enhancement Quote Collection Process
    Private m_sCashListActualCalledFrom As String = ""
    Private m_bMultiplePoliciesSelected As Boolean
    Private m_lQuoteClientCnt As Integer
    Private m_lQuoteAgentCnt As Integer
    Private m_sQuoteAgentType As String = ""
    Dim m_oPMAutoNumber As bACTAutoNumber.Business
    Dim m_oDocumentPost As bACTDocumentPost.Form

    ' WPR 51
    Private m_cOriginalPNLTotal As Decimal = 0.0
    Private m_iLeadAccountBranchId As Integer
    Private m_cUnallocatedAmountForPost As Decimal
    Private m_bIsUnallocatedAmountForPost As Boolean
    Private m_lFirstTransdetailID As Integer
    Dim oCache As Hashtable = New Hashtable

    Private m_nTaxTransdetailId As Integer
    Private m_sIncludeInsurerPaymentMultiStep As String
    Private m_bIsInsurerePaymentRoadMap As Boolean
    Private m_nTaxBandId As Integer
    Private m_crTaxAmount As Decimal
    Private m_sBIC As String
    Private m_sIBAN As String
    Private m_dtTransactionDate As Date
    Private m_sPlanref As String

    Public Property TaxTransDetailID() As Integer
        Get
            Return m_nTaxTransdetailId
        End Get
        Set(ByVal Value As Integer)
            m_nTaxTransdetailId = Value
        End Set
    End Property

    Public Property IsInsurerePaymentRoadMap() As Boolean
        Get
            Return m_bIsInsurerePaymentRoadMap
        End Get
        Set(ByVal Value As Boolean)
            m_bIsInsurerePaymentRoadMap = Value
        End Set
    End Property

    Public Property TaxAmount() As Decimal
        Get
            Return m_crTaxAmount
        End Get
        Set(ByVal value As Decimal)
            m_crTaxAmount = value
        End Set
    End Property

    Public Property TaxBandID() As Integer
        Get
            Return m_nTaxBandId
        End Get
        Set(ByVal Value As Integer)
            m_nTaxBandId = Value
        End Set
    End Property

    Public Enum ENMatchSplitLead
        MediaTypeID = 2
        MediaRef = 5
        PaymentName = 16
        ChequeDate = 34
        CCNumber = 35
        CCExpiryDate = 36
        CCStartDate = 37
        CCIssue = 38
        CCPin = 39
        CCName = 66
        CCCustomer = 67
        CCManualAuthCode = 68
        SplitTotal = 84
    End Enum

    'Start of PN: 62476
    Public m_bMediaType As Boolean
    'End of PN: 62476
    ' Start - Sankar - PN 56728
    Private m_lPartyBankId As Integer

    Public Property PartyBankId() As Integer
        Get
            Return m_lPartyBankId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyBankId = Value
        End Set
    End Property
    ' End - Sankar - PN 56728

    Public WriteOnly Property SilentMultiCurrencyScreen() As Boolean
        Set(ByVal Value As Boolean)
            m_bSilentMultiCurrencyScreen = Value
        End Set
    End Property
    Public Property InsurerPaymentInstArray() As Object
        Get
            Return m_vInsurerPaymentInstArray
        End Get
        Set(ByVal vInsurerPaymentInstArray As Object)
            m_vInsurerPaymentInstArray = vInsurerPaymentInstArray
        End Set
    End Property
    Public Property Cashlistitemreceipttypeid() As Integer
        Get
            Return m_iCashListItemReceiptTypeID
        End Get
        Set(ByVal iCashListItemReceiptTypeID As Integer)
            m_iCashListItemReceiptTypeID = iCashListItemReceiptTypeID
        End Set
    End Property

    Public Property BGAllPoliciesForReceipt() As Object
        Get
            Return m_vBGAllPoliciesForReceipt
        End Get
        Set(ByVal Value As Object)

            m_vBGAllPoliciesForReceipt = Value
        End Set
    End Property

    Public Property SelectdBGPoliciesItem() As Object
        Get
            Return m_vSelectdBGPoliciesItem
        End Get
        Set(ByVal Value As Object)

            m_vSelectdBGPoliciesItem = Value
        End Set
    End Property

    Public WriteOnly Property DocumentsCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iDocumentsCurrencyID = Value
        End Set
    End Property

    Public WriteOnly Property ClaimPaymentId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimPaymentId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenType() As String
        Set(ByVal Value As String)
            m_sScreenType = Value
        End Set
    End Property

    Public Property DocumentIds() As Object
        Get
            Return m_vDocumentIds
        End Get
        Set(ByVal Value As Object)

            m_vDocumentIds = Value
        End Set
    End Property

    '****************
    ' MEvans : 14-05-2003 : CQ 709
    Public WriteOnly Property ApprovalType() As Integer
        Set(ByVal Value As Integer)
            m_lApprovalType = Value
        End Set
    End Property
    '****************
    Public Property ListData() As Object
        Get
            'DD 28/08/2003: Added for Validation
            Return VB6.CopyArray(m_vListData)
        End Get
        Set(ByVal Value As Object)
            'DD 28/08/2003: Added for Validation
            m_vListData = Value
        End Set
    End Property

    Public Property MediaTypeID() As Integer
        Get
            ' AMB 21/02/2003: PS220 - added for manage debtors development
            Return m_lMediaTypeID

        End Get
        Set(ByVal Value As Integer)
            ' AMB 21/02/2003: PS220 - added for manage debtors development
            m_lMediaTypeID = Value

        End Set
    End Property
    Public Property OurRef() As String
        Get
            Return m_sOurRef

        End Get
        Set(ByVal Value As String)
            m_sOurRef = Value

        End Set
    End Property
    Public Property TheirRef() As String
        Get
            Return m_sTheirRef

        End Get
        Set(ByVal Value As String)
            m_sTheirRef = Value

        End Set
    End Property


    Public Property PaymentName() As String
        Get
            ' AMB 21/02/2003: PS220 - added for manage debtors development
            Return m_sPaymentName

        End Get
        Set(ByVal Value As String)
            ' AMB 21/02/2003: PS220 - added for manage debtors development
            m_sPaymentName = Value

        End Set
    End Property

    Public WriteOnly Property AllowAllocateButton() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllowAllocateButton = Value
        End Set
    End Property

    Public Property AllocationID() As Integer
        Get
            Return m_lAllocationID
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationID = Value
        End Set
    End Property
    'EK 17/12/99

    Public Property Amount() As Decimal
        Get
            Return m_cAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cAmount = Value
        End Set
    End Property

    Public Property WriteOffAmount() As Decimal
        Get
            Return m_cWriteOffAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cWriteOffAmount = Value
        End Set
    End Property

    Public Property IsWOFF() As Boolean
        Get
            Return m_bWOFF
        End Get
        Set(ByVal Value As Boolean)
            m_bWOFF = Value
        End Set
    End Property

    Public Property SourceForm() As Integer
        Get
            Return m_iSourceForm
        End Get
        Set(ByVal Value As Integer)
            m_iSourceForm = Value
        End Set
    End Property

    'DC231001 -start -for checking if run via Insurer Payment

    Public Property ViaInsurerPayment() As Boolean
        Get

            Return m_bViaInsurerPayment

        End Get
        Set(ByVal Value As Boolean)

            m_bViaInsurerPayment = Value

        End Set
    End Property
    'DC231001 -end
    'AR20050125 - PN18271

    Public Property ViaClaimPayment() As Boolean
        Get
            Return m_bViaClaimPayment
        End Get
        Set(ByVal Value As Boolean)
            m_bViaClaimPayment = Value
        End Set
    End Property

    'PN11263

    Public Property ViaFinancePlan() As Boolean
        Get

            Return m_bViaFinancePlan
        End Get
        Set(ByVal Value As Boolean)

            m_bViaFinancePlan = Value

        End Set
    End Property
    'PN11263end

    ' Property Let

    ' Property Get
    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    ' Property Get
    Public ReadOnly Property AllocationIDs() As Object
        Get
            Return VB6.CopyArray(m_vAllocationIDs)
        End Get
    End Property

    ' Property Let
    Public WriteOnly Property CashListAllocationRoadmap() As String
        Set(ByVal Value As String)
            m_sCashListRoadmap = Value
        End Set
    End Property
    'eck100500
    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)

            m_iCompanyID = Value

        End Set
    End Property
    'eck100701
    Public Property Letter() As Boolean
        Get

            Return m_bLetter

        End Get
        Set(ByVal Value As Boolean)

            m_bLetter = Value

        End Set
    End Property
    Public Property Letters() As Object
        Get

            Return VB6.CopyArray(m_vLetters)

        End Get
        Set(ByVal Value As Object)

            m_vLetters = Value
        End Set
    End Property

    Public ReadOnly Property CashTransDetailID() As Integer
        Get
            Return m_lCashTransDetailID
        End Get
    End Property
    Public WriteOnly Property ForRecommendation() As String
        Set(ByVal Value As String)
            m_sForRecommendation = Value
        End Set
    End Property
    Public WriteOnly Property ClaimPaymentIDs() As Object
        Set(ByVal Value As Object)

            m_vClaimPaymentIDs = Value
        End Set
    End Property

    'WPR12- Enhancement Quote Collection Process
    Public WriteOnly Property CashListActualCalledFrom() As String
        Set(ByVal Value As String)

            m_sCashListActualCalledFrom = Value

        End Set
    End Property

    Public WriteOnly Property MultipleQuoteSelected() As Boolean
        Set(ByVal Value As Boolean)

            m_bMultiplePoliciesSelected = Value

        End Set
    End Property

    Public WriteOnly Property QuotePartyCnt() As Integer
        Set(ByVal Value As Integer)

            m_lQuoteClientCnt = Value

        End Set
    End Property

    Public WriteOnly Property QuoteAgentCnt() As Integer
        Set(ByVal Value As Integer)

            m_lQuoteAgentCnt = Value

        End Set
    End Property

    Public WriteOnly Property QuoteAgentType() As String
        Set(ByVal Value As String)

            m_sQuoteAgentType = Value

        End Set
    End Property

    'eck100701 end

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    Public WriteOnly Property UnallocatedAmountForPost() As Decimal
        Set(ByVal cUnallocatedAmountForPost As Decimal)
            m_cUnallocatedAmountForPost = cUnallocatedAmountForPost
        End Set
    End Property
    Public WriteOnly Property IsUnallocatedAmountForPost() As Boolean
        Set(ByVal bIsUnallocatedAmountForPost As Boolean)
            m_bIsUnallocatedAmountForPost = bIsUnallocatedAmountForPost
        End Set
    End Property

    Public Property PlanRef() As String

        Get

            Return m_sPlanref

        End Get
        Set(ByVal Value As String)

            m_sPlanref = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property
    Public WriteOnly Property CashlistID() As Integer
        Set(ByVal Value As Integer)

            ' Set the Cash List ID
            m_lCashlistID = Value

        End Set
    End Property

    ' CF150199
    Public Property AccountID() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)

            ' Set the AccountID
            m_lAccountID = Value

        End Set
    End Property

    Public Property CashlistitemID() As Integer
        Get

            Return m_lCashListItemID

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItemID = Value

        End Set
    End Property

    Public Property CashlistTypeID() As Integer
        Get

            Return m_lCashListTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lCashListTypeID = Value

        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property

    Public Property BatchID() As Integer
        Get
            Return m_lBatchID
        End Get
        Set(ByVal Value As Integer)
            m_lBatchID = Value
        End Set
    End Property

    'sw added payment maintenance 06-11-2002

    Public Property ActionKey() As String
        Get
            Return m_sActionkey
        End Get
        Set(ByVal Value As String)
            m_sActionkey = Value
        End Set
    End Property

    'TF230902

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property GISSchemeID() As Integer
        Set(ByVal Value As Integer)
            m_lGISSchemeID = Value
        End Set
    End Property
    Public WriteOnly Property GISDataModelCode() As String
        Set(ByVal Value As String)
            m_sGISDataModelCode = Value
        End Set
    End Property
    'CMG/PB 08012002 PS202
    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property

    Public Property CashListItemPaymentStatusID() As Integer
        Get
            Return m_lCashlistitemPaymentStatusId
        End Get
        Set(ByVal Value As Integer)
            m_lCashlistitemPaymentStatusId = Value
        End Set
    End Property
    'End CMG
    Public Property CashListItemPaymentTypeID() As Integer
        Get
            ' AMB 24/02/2003: PS220 - added for Manage Debtors 'refunds' development
            Return m_iCashListItemPaymentTypeID '' PN 3733
        End Get
        Set(ByVal Value As Integer)
            ' AMB 24/02/2003: PS220 - added for Manage Debtors 'refunds' development
            m_iCashListItemPaymentTypeID = Value '' PN 3733
        End Set
    End Property
    'sw added 30/01/2003

    Public Property ReceiptPolicyRef() As String
        Get
            Return m_sReceiptPolicyRef
        End Get
        Set(ByVal Value As String)
            m_sReceiptPolicyRef = Value
        End Set
    End Property
    '30/04/2003 - PWC - ENDVR00000850
    Public WriteOnly Property CashListStatusId() As Integer
        Set(ByVal Value As Integer)
            m_iCashListStatusId = Value
        End Set
    End Property

    'Added for Letter printing
    Public ReadOnly Property CashDrawerID() As Integer
        Get
            Return m_vCashDrawerID
        End Get
    End Property

    Public Property DocumentRef() As String
        Get
            Return m_sDocumentRef
        End Get
        Set(ByVal Value As String)
            m_sDocumentRef = Value
        End Set
    End Property

    'AR20050125 - PN18271

    Public Property PayeeName() As String
        Get
            Return m_sPayeeName
        End Get
        Set(ByVal Value As String)
            m_sPayeeName = Value
        End Set
    End Property

    Public Property PayeeAccountCode() As String
        Get
            Return m_sPayeeAccountCode
        End Get
        Set(ByVal Value As String)
            m_sPayeeAccountCode = Value
        End Set
    End Property

    Public Property PayeeSortCode() As String
        Get
            Return m_sPayeeSortCode
        End Get
        Set(ByVal Value As String)
            m_sPayeeSortCode = Value
        End Set
    End Property

    Public Property PayeeComments() As String
        Get
            Return m_sPayeeComments
        End Get
        Set(ByVal Value As String)
            m_sPayeeComments = Value
        End Set
    End Property

    Public Property LedgerCode() As String
        Get
            Return m_sLedgerCode
        End Get
        Set(ByVal Value As String)
            m_sLedgerCode = Value
        End Set
    End Property

    Public Property ViaBanking() As Boolean
        Get
            Return m_bViaBanking
        End Get
        Set(ByVal Value As Boolean)
            m_bViaBanking = Value
        End Set
    End Property
    Public Property MediaRef() As String
        Get
            Return m_sMediaRef
        End Get
        Set(ByVal Value As String)
            m_sMediaRef = Value
        End Set
    End Property

    Public Property InsurerAccountID() As Integer
        Get
            Return m_iInsurerAccountID
        End Get
        Set(ByVal value As Integer)
            m_iInsurerAccountID = value
        End Set
    End Property

    Public Property DocumentID() As Integer
        Get
            Return m_iDocumentsID
        End Get
        Set(ByVal value As Integer)
            m_iDocumentsID = value
        End Set
    End Property

    Public Property LeadAccountBranchId() As Integer
        Get
            Return m_iLeadAccountBranchId
        End Get
        Set(ByVal value As Integer)
            m_iLeadAccountBranchId = value
        End Set
    End Property

    Public Property TransactionDate() As Date
        Get
            Return m_dtTransactionDate
        End Get
        Set(ByVal value As Date)
            m_dtTransactionDate = value
        End Set

    End Property

    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sMessage, sTitle As String
        Dim oLookup As bPMLookup.Business
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object

            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashlistitem.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            Dim temp_m_oPMAutoNumber As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMAutoNumber, "bACTAutoNumber.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMAutoNumber = temp_m_oPMAutoNumber

            Dim temp_m_oDocumentPost As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oDocumentPost, "bACTDocumentPost.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oDocumentPost = temp_m_oDocumentPost

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTCashListItem.Form" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            Dim temp_oLookup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oLookup = temp_oLookup
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bPMLookup.Business" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            Dim temp_g_oSirConfig As Object

            m_lReturn = g_oObjectManager.GetInstance(temp_g_oSirConfig, "bSIRSolutionConfig.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oSirConfig = temp_g_oSirConfig
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bSirSolutionConfig.Business" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Look up on the architecture table
            oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

            'if no road map set then default to the allocation roadmap
            If m_sCashListRoadmap = "" Then
                m_sCashListRoadmap = "ACTRCTV22P"
            End If

            'Tracy Richards - Get the ProcessID. If this is the PFCash roadmap,
            'get the normal Allocation process ID, otherwise get the one from
            'the roadmap passed in
            If m_sCashListRoadmap = "PFCASH" Then
                'leave as normal (get ID from m_sCashListRoadmap)
                m_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="PMNav_Process", v_sCode:="ACTRCTV22P", v_dtEffectiveDate:=DateTime.Now, r_lID:=m_lAllocationProcessID)
            Else
                ' Get the processID for the specified allocation navigator map
                m_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="PMNav_Process", v_sCode:=m_sCashListRoadmap, v_dtEffectiveDate:=DateTime.Now, r_lID:=m_lAllocationProcessID)
            End If
            ' Remove the instance of bPMLookup
            oLookup.Dispose()
            oLookup = Nothing

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' CTAF 290200 - Not allocated yet
            m_bAllocated = False

            m_vBGAllPoliciesForReceipt = Nothing
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Load_Renamed() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Split Receipt
            m_bOnLoad = True

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
        Dim result As Integer = 0
        Dim vCashListItem As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lDisplayState = lDisplayState

            'PSL 03/03/2003 added ACTViewCheque
            If lDisplayState = gACTLibrary.ACTUseListHidden Then
                If (m_sActionkey = gACTLibrary.ACTEditCheque) Or (m_sActionkey = gACTLibrary.ACTCancelCheque) Or (m_sActionkey = gACTLibrary.ACTStopCheque) Or (m_sActionkey = gACTLibrary.ACTApprove) Or (m_sActionkey = gACTLibrary.ACTViewCheque) Or (m_iTask) = gPMConstants.PMEComponentAction.PMView Then
                    'select the correct listitem
                    'first find the cash list item in the array
                    For lRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                        If CInt(m_vListData(ACSubCashListItemID, lRow)) = m_lCashListItemID Then
                            Me.Visible = True
                            lvwListDetails.FocusedItem = lvwListDetails.Items.Item(lRow)
                            Me.Visible = False
                            Exit For
                        End If
                    Next

                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        cmdEdit_Click(cmdEdit, New EventArgs())
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        cmdView_Click(cmdView, New EventArgs())
                    End If

                    Return result

                Else
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit And m_sForRecommendation = "F" Then
                        cmdEdit_Click(cmdEdit, New EventArgs())
                    Else
                        cmdAdd_Click(cmdAdd, New EventArgs())
                    End If
                    Application.DoEvents()

                    If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                        cmdCancel_Click(cmdCancel, New EventArgs())
                    ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMCancel
                    Else
                        If m_sScreenType = "CLP" Then
                            'm_sRecommended = "Y" - Payment is sent for Recommendation only. So, only posting will be done
                            'm_sRecommended = "N" - Payment is sent for Authorisation - Posting is already done.
                            'else both posting and allocation will be performed
                            If m_sForRecommendation = "T" Then
                                m_lReturn = FinaliseRecommend()
                            ElseIf m_sForRecommendation = "F" Then
                                cmdPost_Click(cmdPost, New EventArgs())
                                cmdAllocate_Click(cmdAllocate, New EventArgs())
                            Else
                                cmdPost_Click(cmdPost, New EventArgs())
                                cmdAllocate_Click(cmdAllocate, New EventArgs())
                                m_lDisplayState = FormShowConstants.Modeless
                                cmdOK_Click(cmdOK, New EventArgs())
                            End If

                        Else

                            cmdOK_Click(cmdOK, New EventArgs())

                            m_lReturn = GetBusiness()

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                                m_lReturn = m_oBusiness.GetNext(vCashListItem)

                                m_lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))
                            End If

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to get details.
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                End If
            Else

                If lvwListDetails.Items.Count > 0 Then

                    If (lvwListDetails.SelectedItems.Count > 0) Then
                        lvwListDetails_ItemClick(lvwListDetails.SelectedItems.Item(0))
                    End If
                End If

                ' Show the the form, allow user input etc.
                VB6.ShowForm(Me, lDisplayState)
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function FinaliseRecommend() As Long
        Const kMethodName As String = "FinaliseRecommend"
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Update the business object so the cash list id's are on the database
            m_lReturn = m_oBusiness.Update()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error Message
                RaiseError(kMethodName, "FinaliseRecommend Failed", gPMConstants.PMELogLevel.PMLogOnError)
            End If

            'Start at beginning of collection
            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "m_oBusiness.GetDetails Failed", gPMConstants.PMELogLevel.PMLogOnError)
            End If


            If (m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) Or
                (m_lCashListTypeID = ACTCashListTypeClaimPayments) And m_sForRecommendation = "F" Then
                m_bApprovalMsg = True
                MsgBox("This payment requires further approval. It cannot be allocated until fully approved.",
                        vbExclamation, "Allocation not allowed")
            End If
            Return result

        Catch ex As Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLetterDetails (Standard Method)
    '
    ' Description: Gets the Letter Processing Details
    '
    ' ***************************************************************** '
    Public Function GetLetterDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oBusiness Is Nothing Then
                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_m_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCashlistitem.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusiness = temp_m_oBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = LetterProcessing(m_lCashListItemID)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the letter details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLetterDetails")

                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Letter Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLetterDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Public Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByVal v_sSelectedItemCode As String = "") As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim lSelectedItemIndex As Integer

        Const ACLedgerInsurer As String = "Insurer"
        Const ACLedgerAgent As String = "Agent"

        ' Look  up value constants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1
        Const ACDetailCode As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim iNewIndex As Integer = -1
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                ' Add the details to the control.

                'iNewIndex = ReflectionHelper.Invoke(ReflectionHelper.GetMember(ctlLookup, "Items"), "Add", New Object() {New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), m_vLookupDetails(ACDetailKey, lCntr))})
                iNewIndex = CType(ctlLookup, ComboBox).Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), m_vLookupDetails(ACDetailKey, lCntr)))

                ' if a code has been specified that needs matching
                If v_sSelectedItemCode <> "" Then
                    ' check if the current items code matches the specified one
                    If v_sSelectedItemCode = CStr(m_vLookupDetails(ACDetailCode, lCntr)) Then
                        ' save items Index

                        lSelectedItemIndex = iNewIndex
                    End If
                End If

                ' Check if this is the selected index.
                If CBool(CStr((CStr(m_vLookupValues(ACValueID, lRow))) = CStr(m_vLookupDetails(ACDetailKey, lCntr)))) Then
                    'ReflectionHelper.SetMember(ctlLookup, "SelectedIndex", lSelectedItemIndex)
                    CType(ctlLookup, ComboBox).SelectedIndex = lSelectedItemIndex
                End If

                If (sLookupTable = gACTLibrary.ACTLookupPaymentTypeTable) And m_bViaInsurerPayment Then
                    If m_sLedgerCode = ACLedgerInsurer And CStr(m_vLookupDetails(ACDetailCode, lCntr)).ToUpper() = "INSPAY" Then

                        lSelectedItemIndex = iNewIndex
                    ElseIf m_sLedgerCode = ACLedgerAgent And CStr(m_vLookupDetails(ACDetailCode, lCntr)).ToUpper() = "AGPAY" Then

                        lSelectedItemIndex = iNewIndex
                    End If
                End If

            Next lCntr

            ' If nothing yet selected (index = -1)
            ' select first item (index = 0)

            If (ReflectionHelper.GetMember(ctlLookup, "SelectedIndex") < 0) And ReflectionHelper.GetMember(ReflectionHelper.GetMember(ctlLookup, "Items"), "Count") > 0 Then
                'ReflectionHelper.SetMember(ctlLookup, "SelectedIndex", 0)
                CType(ctlLookup, ComboBox).SelectedIndex = 0
            End If

            ' if a selected item has been found
            ' select it.
            If lSelectedItemIndex <> 0 Then
                'ReflectionHelper.SetMember(ctlLookup, "SelectedIndex", lSelectedItemIndex)
                CType(ctlLookup, ComboBox).SelectedIndex = lSelectedItemIndex
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupSingle
    '
    ' Description: Gets a single lookup caption
    '
    ' ***************************************************************** '
    Public Function GetLookupSingle(ByRef sLookupTable As String, ByRef iLookUpID As Integer, Optional ByRef sLookUpCaption As String = "", Optional ByRef sLookUpcode As String = "") As Integer
        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        Dim oValidation As bSIRMediaTypeValidation.Business
        Dim vValue As Object
        Dim sKey As String = ""
        ' Lookup value constants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1
        'Added SW front office receipting, return code as this is more generic
        Const ACDetailCode As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get caption from the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupSingle")

                Return result
            End If

            ' Using the lookup values, match the ID and return the Caption

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                ' Check if this is the selected index.
                If CInt(m_vLookupDetails(ACDetailKey, lCntr)) = iLookUpID Then
                    sLookUpCaption = CStr(m_vLookupDetails(ACDetailDesc, lCntr))
                    If sLookupTable = gACTLibrary.ACTLookupMediaType Then

                        Dim temp_oValidation As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oValidation = temp_oValidation
                        sKey = sLookupTable & "_" & iLookUpID

                        vValue = oCache.Item(sKey)

                        If Object.Equals(vValue, Nothing) Then
                            sLookUpcode = oValidation.GetValidationCode(iLookUpID).Trim()
                            ' Add them to the Cache
                            oCache.Add(sKey, sLookUpcode)
                        Else
                            sLookUpcode = vValue.ToString
                        End If

                        If sLookUpcode = "" And Not bMessageDisplayed Then
                            MessageBox.Show("The media type " & sLookUpCaption.Trim() & " does not have an appropriate Media Validation Code associated with it, please correct this before you continue", "Invalid Media Validation Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            bMessageDisplayed = True
                            Return gPMConstants.PMEReturnCode.PMFalse
                        ElseIf sLookUpcode = "" And bMessageDisplayed Then
                            Return gPMConstants.PMEReturnCode.PMNotFound
                        End If
                    Else
                        sLookUpcode = CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim()
                    End If
                    Exit For
                End If

            Next lCntr

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the lookup caption", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupSingle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DetailsCommit
    '
    ' Description: Used to by List & Details Form to commit all changes to
    '   business collection so far by updating the business with most
    '   recent change and then calling Update to commit to DB.
    '
    ' ***************************************************************** '
    Public Function DetailsCommit(ByRef iTaskType As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business collection with the details
            m_lReturn = DetailsFormReturn(lRow:=m_lSelectedRow, iTaskType:=iTaskType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Commit the details using the business object.

            m_lReturn = m_oBusiness.Update()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                Return result
            End If

            ' Refresh local data, done to get any added ID's
            ' and ensure that business is in sync after commit.
            ' No need to do a GetDetails as all still in business collection, just need to:
            ' Set current record to start of collection

            m_oBusiness.CurrentRecord = 0

            ' Get the details back from the business object to the List data storage
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details from the List data storage
            ' to the interface.
            m_lReturn = DataToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to commit database changes", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsCommit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the
            ' business object.
            m_lReturn = GetBusiness()

            ' If we have some records populate array and list view
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the List data storage.
                m_lReturn = BusinessToData()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'BB Used here to get the Media Types & Allocation Status array
                m_lReturn = DisplayLookupDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the List data storage
                ' to the interface.
                m_lReturn = DataToInterface()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If no records exist we still need Types array
            Else

                'BB Used here to get the Media Types & Allocation Status array
                m_lReturn = DisplayLookupDetails()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' AMB 06/03/2003: IS2495 - if there are no items in the listview, disable the appropriate buttons
                If lvwListDetails.Items.Count = 0 Then
                    'SMJB:cmdView.Enabled = False
                    'SMJB:cmdReverse.Enabled = False
                    'SMJB:cmdAllocate.Enabled = False
                    m_lReturn = EnableButtons()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            ' Pass the Cash List ID to be used in selection
            ' as we only want the Items belonging to a given list

            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)

            ' {* USER DEFINED CODE (End) *}

            ' If no records found return NotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Check for other errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates the data storage from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BusinessToData() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCashListItem As Object = Nothing

        Try
            If Not (m_vListData Is Nothing) Then
                ReDim Preserve m_vListData(ACLastItem, 0)
            Else
                ReDim m_vListData(ACLastItem, 0)
            End If
            ' Retrieve all of the details from the business object.
            Dim nCount As Integer = 0
            Do While m_oBusiness.GetNext(oCashListItem) = PMEReturnCode.PMTrue
                nCount = m_vListData.GetUpperBound(1)

                m_vListData(ACSubCashListItemID, nCount) = oCashListItem(eCashListItem.CashlistitemID)
                m_vListData(ACSubAllocationstatusID, nCount) = oCashListItem(eCashListItem.AllocationstatusID)
                m_vListData(ACSubMediatypeID, nCount) = oCashListItem(eCashListItem.MediaTypeID)
                m_vListData(ACSubCashListID, nCount) = oCashListItem(eCashListItem.CashlistID)
                m_vListData(ACSubAccountID, nCount) = oCashListItem(eCashListItem.AccountID)
                m_vListData(ACSubMediaRef, nCount) = oCashListItem(eCashListItem.MediaRef)
                m_vListData(ACSubOurRef, nCount) = oCashListItem(eCashListItem.OurRef)
                m_vListData(ACSubTheirRef, nCount) = oCashListItem(eCashListItem.TheirRef)
                m_vListData(ACSubAmount, nCount) = oCashListItem(eCashListItem.Amount)
                m_vListData(ACSubContactName, nCount) = oCashListItem(eCashListItem.ContactName)
                m_vListData(ACSubAddress1, nCount) = oCashListItem(eCashListItem.Address1)
                m_vListData(ACSubAddress2, nCount) = oCashListItem(eCashListItem.Address2)
                m_vListData(ACSubAddress3, nCount) = oCashListItem(eCashListItem.Address3)
                m_vListData(ACSubAddress4, nCount) = oCashListItem(eCashListItem.Address4)
                m_vListData(ACSubPostalCode, nCount) = oCashListItem(eCashListItem.PostalCode)
                m_vListData(ACSubAddressCountry, nCount) = oCashListItem(eCashListItem.AddressCountry)
                m_vListData(ACSubPaymentName, nCount) = oCashListItem(eCashListItem.PaymentName)
                m_vListData(ACSubPaymentAccountCode, nCount) = oCashListItem(eCashListItem.PaymentAccountCode)
                m_vListData(ACSubPaymentBranchCode, nCount) = oCashListItem(eCashListItem.PaymentBranchCode)
                m_vListData(ACSubPaymentExpiryDate, nCount) = oCashListItem(eCashListItem.PaymentExpiryDate)
                m_vListData(ACSubPaymentReference1, nCount) = oCashListItem(eCashListItem.PaymentReference1)
                m_vListData(ACSubPaymentReference2, nCount) = oCashListItem(eCashListItem.PaymentReference2)
                m_vListData(ACSubLetter, nCount) = oCashListItem(eCashListItem.Letter)
                m_vListData(ACSubTransdetailId, nCount) = oCashListItem(eCashListItem.TransdetailID)
                m_vListData(ACSubRealAllocationID, nCount) = oCashListItem(eCashListItem.AllocationstatusID)
                m_vListData(ACBatchid, nCount) = oCashListItem(eCashListItem.Batch_id)
                m_vListData(ACPMUserid, nCount) = oCashListItem(eCashListItem.pmuser_id)
                m_vListData(ACTransactiondate, nCount) = oCashListItem(eCashListItem.Transaction_Date)
                m_vListData(ACOriginalamount, nCount) = oCashListItem(eCashListItem.Original_Amount)
                m_vListData(ACAmounttendered, nCount) = oCashListItem(eCashListItem.Amount_Tendered)
                m_vListData(ACChange, nCount) = oCashListItem(eCashListItem.Change)
                m_vListData(ACvCashlistitemreceipttypeid, nCount) = oCashListItem(eCashListItem.CashListItem_receipt_type_id)
                m_vListData(ACCashlistitemreceiptstatusid, nCount) = oCashListItem(eCashListItem.CashListItem_receipt_status_id)
                m_vListData(ACCashlistitembankid, nCount) = oCashListItem(eCashListItem.CashListItem_bank_id)
                m_vListData(ACChequedate, nCount) = oCashListItem(eCashListItem.Cheque_Date)
                m_vListData(ACCCnumber, nCount) = oCashListItem(eCashListItem.CC_Number)
                m_vListData(ACCCexpirydate, nCount) = oCashListItem(eCashListItem.CC_Expiry_Date)
                m_vListData(ACCCstartdate, nCount) = oCashListItem(eCashListItem.CC_Start_Date)
                m_vListData(ACCCissue, nCount) = oCashListItem(eCashListItem.CC_Issue)
                m_vListData(ACCCpin, nCount) = oCashListItem(eCashListItem.CC_Pin)
                m_vListData(ACCCauthcode, nCount) = oCashListItem(eCashListItem.CC_Auth_Code)
                m_vListData(ACCCname, nCount) = oCashListItem(eCashListItem.CC_Name)
                m_vListData(ACCCcustomer, nCount) = oCashListItem(eCashListItem.CC_Customer)
                m_vListData(ACCCmanualauthcode, nCount) = oCashListItem(eCashListItem.CC_Manual_Auth_Code)
                m_vListData(ACCCtransactioncode, nCount) = oCashListItem(eCashListItem.CC_Transaction_Code)
                m_vListData(ACMediatype_IssuerID, nCount) = oCashListItem(eCashListItem.MediaTypeIssuerID)
                m_vListData(ACReceiptdetails, nCount) = oCashListItem(eCashListItem.Receipt_Details)
                m_vListData(ACCashlistitemreversepmuserid, nCount) = oCashListItem(eCashListItem.CashListItem_Reverse_PMUser_id)
                m_vListData(ACCashlistitemreversereasonid, nCount) = oCashListItem(eCashListItem.CashListItem_Reverse_Reason_id)
                m_vListData(ACCashListItemPaymentTypeID, nCount) = oCashListItem(eCashListItem.CashListItem_Payment_Type_id)
                m_vListData(ACCashListItemPaymentStatusID, nCount) = oCashListItem(eCashListItem.CashListItem_Payment_Status_id)
                m_vListData(ACDatepresented, nCount) = oCashListItem(eCashListItem.Date_Presented)
                m_vListData(ACChequeinpossession, nCount) = oCashListItem(eCashListItem.Cheque_in_Possession)
                m_vListData(ACStoprequesteddate, nCount) = oCashListItem(eCashListItem.Stop_Requested_Date)
                m_vListData(ACStopprinteddate, nCount) = oCashListItem(eCashListItem.Stop_Printed_Date)
                m_vListData(ACStopconfirmationdate, nCount) = oCashListItem(eCashListItem.Stop_Confirmation_Date)
                m_vListData(ACReason, nCount) = oCashListItem(eCashListItem.Reason)
                m_vListData(ACReplacescashlistitemid, nCount) = oCashListItem(eCashListItem.Replaces_CashListItem_id)
                m_vListData(ACCurrencyBaseDate, nCount) = oCashListItem(eCashListItem.CurrencyBaseDate)
                m_vListData(ACCurrencyBaseXrate, nCount) = oCashListItem(eCashListItem.CurrencyBaseXrate)
                m_vListData(ACAccountBaseDate, nCount) = oCashListItem(eCashListItem.AccountBaseDate)
                m_vListData(ACAccountBaseXrate, nCount) = oCashListItem(eCashListItem.AccountBaseXrate)
                m_vListData(ACSystemBaseDate, nCount) = oCashListItem(eCashListItem.SystemBaseDate)
                m_vListData(ACSystemBaseXrate, nCount) = oCashListItem(eCashListItem.SystemBaseXrate)
                m_vListData(ACOverrideReason, nCount) = oCashListItem(eCashListItem.OverrideReason)
                m_vListData(ACBankPaymentTypeId, nCount) = oCashListItem(eCashListItem.PartyBankId)
                m_vListData(ACCollectionDate, nCount) = oCashListItem(eCashListItem.CollectionDate)
                m_vListData(ACComments, nCount) = oCashListItem(eCashListItem.Comments)

                If CInt(oCashListItem(eCashListItem.AllocationstatusID)) = gACTLibrary.ACTAllocationStatusUnallocated Then
                    m_bAllocated = False
                ElseIf CInt(oCashListItem(eCashListItem.AllocationstatusID)) = gACTLibrary.ACTAllocationStatusPosted Then
                    m_bAllocated = True
                    m_bPosted = True
                End If

                'WPR12- Enhancement Quote Collection Process
                m_vListData(ACBankLocation, nCount) = oCashListItem(eCashListItem.BankLocation)
                m_vListData(ACBankBranch, nCount) = oCashListItem(eCashListItem.BankBranch)
                m_vListData(ACChequeTypeId, nCount) = oCashListItem(eCashListItem.ChequeTypeId)
                m_vListData(ACCCBankId, nCount) = oCashListItem(eCashListItem.CCBankId)
                m_vListData(ACCardTypeId, nCount) = oCashListItem(eCashListItem.CardTypeId)
                m_vListData(ACCardTransSlipNo, nCount) = oCashListItem(eCashListItem.CardTransSlipNo)
                m_vListData(ACChequeClearingTypeId, nCount) = oCashListItem(eCashListItem.ChequeClearingTypeId)
                m_vListData(ACIsLeadAccount, nCount) = oCashListItem(eCashListItem.IsLeadAccount)
                m_vListData(ACSplitTotal, nCount) = oCashListItem(eCashListItem.SplitTotal)
                m_vListData(kTaxBandID, nCount) = oCashListItem(eCashListItem.TaxBandId)
                m_vListData(kTaxAmount, nCount) = oCashListItem(eCashListItem.TaxAmount)
                m_vListData(kPMNavBatchKey, nCount) = oCashListItem(eCashListItem.PMNavBatchKey)
                m_vListData(kBIC, nCount) = oCashListItem(eCashListItem.BIC)
                m_vListData(kIBAN, nCount) = oCashListItem(eCashListItem.IBAN)
                m_vListData(ACInsuranceRef, m_vListData.GetUpperBound(1)) = oCashListItem(eCashListItem.InsuranceRef)

                ' Store unique key for this row.
                m_vListData(m_vListData.GetUpperBound(0), nCount) = nCount + 1
                ' Increment the data array.
                ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), nCount + 1)
            Loop

            ' Check if we have data in the List array.
            If Information.IsArray(m_vListData) Then
                If m_vListData.GetUpperBound(1) > 0 Then
                    ' Decrement the data array.
                    ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
                End If
            End If

            ' Store the last Data ID used by the business
            m_lLastDataID = m_vListData.GetUpperBound(1) + 1
            m_lReturn = EnableButtons()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Updates all business members from the data storage.
    ''' </summary>
    ''' <param name="lMode"></param>
    ''' <param name="lRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToBusiness(ByRef lMode As Integer,
                                    ByRef lRow As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nBusinessDataID As Integer
        Dim oCashListItem As Object = Nothing

        Try

            'Transfer data from list array into cashlistitem array
            ReDim oCashListItem(eCashListItem.LastItem)
            oCashListItem(eCashListItem.CashlistitemID) = m_vListData(ACSubCashListItemID, lRow)
            oCashListItem(eCashListItem.AllocationstatusID) = m_vListData(ACSubRealAllocationID, lRow)
            oCashListItem(eCashListItem.MediaTypeID) = m_vListData(ACSubMediatypeID, lRow)
            oCashListItem(eCashListItem.CashlistID) = m_vListData(ACSubCashListID, lRow)
            oCashListItem(eCashListItem.AccountID) = m_vListData(ACSubAccountID, lRow)
            oCashListItem(eCashListItem.MediaRef) = m_vListData(ACSubMediaRef, lRow)
            oCashListItem(eCashListItem.OurRef) = m_vListData(ACSubOurRef, lRow)
            oCashListItem(eCashListItem.TheirRef) = m_vListData(ACSubTheirRef, lRow)
            oCashListItem(eCashListItem.Amount) = m_vListData(ACSubAmount, lRow)
            oCashListItem(eCashListItem.ContactName) = m_vListData(ACSubContactName, lRow)
            oCashListItem(eCashListItem.Address1) = m_vListData(ACSubAddress1, lRow)
            oCashListItem(eCashListItem.Address2) = m_vListData(ACSubAddress2, lRow)
            oCashListItem(eCashListItem.Address3) = m_vListData(ACSubAddress3, lRow)
            oCashListItem(eCashListItem.Address4) = m_vListData(ACSubAddress4, lRow)
            oCashListItem(eCashListItem.PostalCode) = m_vListData(ACSubPostalCode, lRow)
            oCashListItem(eCashListItem.AddressCountry) = m_vListData(ACSubAddressCountry, lRow)
            oCashListItem(eCashListItem.PaymentName) = m_vListData(ACSubPaymentName, lRow)
            oCashListItem(eCashListItem.PaymentAccountCode) = m_vListData(ACSubPaymentAccountCode, lRow)
            oCashListItem(eCashListItem.PaymentBranchCode) = m_vListData(ACSubPaymentBranchCode, lRow)
            oCashListItem(eCashListItem.PaymentExpiryDate) = m_vListData(ACSubPaymentExpiryDate, lRow)
            oCashListItem(eCashListItem.PaymentReference1) = m_vListData(ACSubPaymentReference1, lRow)
            oCashListItem(eCashListItem.PaymentReference2) = m_vListData(ACSubPaymentReference2, lRow)
            oCashListItem(eCashListItem.Letter) = m_vListData(ACSubLetter, lRow)
            oCashListItem(eCashListItem.Batch_id) = m_vListData(ACBatchid, lRow)
            oCashListItem(eCashListItem.pmuser_id) = m_vListData(ACPMUserid, lRow)
            oCashListItem(eCashListItem.Transaction_Date) = m_vListData(ACTransactiondate, lRow)
            oCashListItem(eCashListItem.Original_Amount) = m_vListData(ACOriginalamount, lRow)
            oCashListItem(eCashListItem.Amount_Tendered) = m_vListData(ACAmounttendered, lRow)
            oCashListItem(eCashListItem.Change) = m_vListData(ACChange, lRow)
            oCashListItem(eCashListItem.CashListItem_receipt_type_id) = m_vListData(ACvCashlistitemreceipttypeid, lRow)
            oCashListItem(eCashListItem.CashListItem_receipt_status_id) = m_vListData(ACCashlistitemreceiptstatusid, lRow)
            oCashListItem(eCashListItem.CashListItem_bank_id) = m_vListData(ACCashlistitembankid, lRow)
            oCashListItem(eCashListItem.Cheque_Date) = m_vListData(ACChequedate, lRow)
            oCashListItem(eCashListItem.CC_Number) = m_vListData(ACCCnumber, lRow)
            oCashListItem(eCashListItem.CC_Expiry_Date) = m_vListData(ACCCexpirydate, lRow)
            oCashListItem(eCashListItem.CC_Start_Date) = m_vListData(ACCCstartdate, lRow)
            oCashListItem(eCashListItem.CC_Issue) = m_vListData(ACCCissue, lRow)
            oCashListItem(eCashListItem.CC_Pin) = m_vListData(ACCCpin, lRow)
            oCashListItem(eCashListItem.CC_Auth_Code) = m_vListData(ACCCauthcode, lRow)
            oCashListItem(eCashListItem.CC_Name) = m_vListData(ACCCname, lRow)
            oCashListItem(eCashListItem.CC_Customer) = m_vListData(ACCCcustomer, lRow)
            oCashListItem(eCashListItem.CC_Manual_Auth_Code) = m_vListData(ACCCmanualauthcode, lRow)
            oCashListItem(eCashListItem.CC_Transaction_Code) = m_vListData(ACCCtransactioncode, lRow)
            oCashListItem(eCashListItem.MediaTypeIssuerID) = m_vListData(ACMediatype_IssuerID, lRow)
            oCashListItem(eCashListItem.Receipt_Details) = m_vListData(ACReceiptdetails, lRow)
            oCashListItem(eCashListItem.CashListItem_Reverse_PMUser_id) = m_vListData(ACCashlistitemreversepmuserid, lRow)
            oCashListItem(eCashListItem.CashListItem_Reverse_Reason_id) = m_vListData(ACCashlistitemreversereasonid, lRow)
            oCashListItem(eCashListItem.CashListItem_Payment_Type_id) = m_vListData(ACCashListItemPaymentTypeID, lRow)
            oCashListItem(eCashListItem.CashListItem_Payment_Status_id) = m_vListData(ACCashListItemPaymentStatusID, lRow)
            oCashListItem(eCashListItem.Date_Presented) = m_vListData(ACDatepresented, lRow)
            oCashListItem(eCashListItem.Cheque_in_Possession) = m_vListData(ACChequeinpossession, lRow)
            oCashListItem(eCashListItem.Stop_Requested_Date) = m_vListData(ACStoprequesteddate, lRow)
            oCashListItem(eCashListItem.Stop_Printed_Date) = m_vListData(ACStopprinteddate, lRow)
            oCashListItem(eCashListItem.Stop_Confirmation_Date) = m_vListData(ACStopconfirmationdate, lRow)
            oCashListItem(eCashListItem.Reason) = m_vListData(ACReason, lRow)
            oCashListItem(eCashListItem.Replaces_CashListItem_id) = m_vListData(ACReplacescashlistitemid, lRow)
            oCashListItem(eCashListItem.InstalmentArray) = m_vListData(ACInstalmentArray, lRow)
            oCashListItem(eCashListItem.SalvageArray) = m_vListData(ACSalvageArray, lRow)
            oCashListItem(eCashListItem.CLMUSRecoveryArray) = m_vListData(ACCLMUSRecoveryArray, lRow)
            oCashListItem(eCashListItem.CLMRVRecoveryArray) = m_vListData(ACCLMRVRecoveryArray, lRow)
            oCashListItem(eCashListItem.UnderwritingYearID) = m_vListData(ACUnderwritingYearID, lRow)
            oCashListItem(eCashListItem.CurrencyBaseDate) = m_vListData(ACCurrencyBaseDate, lRow)
            oCashListItem(eCashListItem.CurrencyBaseXrate) = m_vListData(ACCurrencyBaseXrate, lRow)
            oCashListItem(eCashListItem.AccountBaseDate) = m_vListData(ACAccountBaseDate, lRow)
            oCashListItem(eCashListItem.AccountBaseXrate) = m_vListData(ACAccountBaseXrate, lRow)
            oCashListItem(eCashListItem.SystemBaseDate) = m_vListData(ACSystemBaseDate, lRow)
            oCashListItem(eCashListItem.SystemBaseXrate) = m_vListData(ACSystemBaseXrate, lRow)
            oCashListItem(eCashListItem.OverrideReason) = m_vListData(ACOverrideReason, lRow)
            oCashListItem(eCashListItem.TransdetailID) = m_vListData(ACSubTransdetailId, lRow)
            'Party Bank Details

            oCashListItem(eCashListItem.PartyBankId) = m_vListData(ACBankPaymentTypeId, lRow)
            oCashListItem(eCashListItem.CollectionDate) = m_vListData(ACCollectionDate, lRow)
            oCashListItem(eCashListItem.Comments) = m_vListData(ACComments, lRow)
            oCashListItem(eCashListItem.BGPolicies) = m_vListData(ACBGPolicies, lRow)
            'WPR12- Enhancement Quote Collection Process

            oCashListItem(eCashListItem.BankLocation) = m_vListData(ACBankLocation, lRow)
            oCashListItem(eCashListItem.BankBranch) = m_vListData(ACBankBranch, lRow)
            oCashListItem(eCashListItem.ChequeTypeId) = m_vListData(ACChequeTypeId, lRow)
            oCashListItem(eCashListItem.CCBankId) = m_vListData(ACCCBankId, lRow)
            oCashListItem(eCashListItem.CardTypeId) = m_vListData(ACCardTypeId, lRow)
            oCashListItem(eCashListItem.CardTransSlipNo) = m_vListData(ACCardTransSlipNo, lRow)
            oCashListItem(eCashListItem.ChequeClearingTypeId) = m_vListData(ACChequeClearingTypeId, lRow)
            oCashListItem(eCashListItem.IsLeadAccount) = m_vListData(ACIsLeadAccount, lRow)
            oCashListItem(eCashListItem.SplitTotal) = m_vListData(ACSplitTotal, lRow)
            oCashListItem(eCashListItem.TaxBandId) = m_vListData(kTaxBandID, lRow)
            oCashListItem(eCashListItem.TaxAmount) = m_vListData(kTaxAmount, lRow)
            oCashListItem(eCashListItem.PMNavBatchKey) = m_vListData(kPMNavBatchKey, lRow)
            oCashListItem(eCashListItem.BIC) = m_vListData(kBIC, lRow)
            oCashListItem(eCashListItem.IBAN) = m_vListData(kIBAN, lRow)
            oCashListItem(eCashListItem.InsuranceRef) = m_vListData(ACInsuranceRef, lRow)
            ' Store unique key for this row.
            nBusinessDataID = CInt(m_vListData(m_vListData.GetUpperBound(0), lRow))

            ' Check the task.
            Select Case (lMode)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    m_lReturn = m_oBusiness.EditAdd(lRow:=nBusinessDataID, r_vCashListItem:=oCashListItem)
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMReverse
                    ' Inform the business object with an updated data item.
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=nBusinessDataID, v_vCashListItem:=oCashListItem)
                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Inform the business object with a deleted data item.
                    m_lReturn = m_oBusiness.EditDelete(lRow:=nBusinessDataID)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="Failed to assign the data details to business object",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="DataToBusiness")
            End If
            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the List data.
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Dim iMediaTypeID As Integer
        Dim sMediaType As String = ""
        Dim iAllocationStatusID As Integer
        Dim sAllocationStatus As String

        'DC290704 PN13006 to distinguish between instalment related line and others

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the List details.
            lvwListDetails.Items.Clear()

            ' Disable the Remove/Edit buttons by default
            ' Enabled in List.ItemClick when a deletable item is clicked
            'SMJB:cmdRemove.Enabled = False
            'SMJB:cmdEdit.Enabled = False

            ' Check that List details are present before
            ' continuing.
            If Not Information.IsArray(m_vListData) Then
                ' No details so disable Edit
                'uday
                m_cTotalAmount = 0
                pnlTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=m_cTotalAmount)
                Return result
            End If

            ' We have some details so we can enable Edit

            ' Assign the details to the interface.
            'eck250800
            m_cTotalAmount = 0

            For lRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}
                ' ACSubCashListItemID = 0
                ' ACSubAllocationstatusID = 1
                ' ACSubMediatypeID = 2
                ' ACSubCashListID = 3
                ' ACSubAccountID = 4
                ' ACSubMediaRef = 5
                ' ACSubOurRef = 6
                ' ACSubTheirRef = 7

                ' We are coming from FindTransaction 
                If m_sCallingAppName = "SplitReceiptsFromFindTransaction" And m_bOnLoad = True Then

                    If m_vListData.GetUpperBound(1) = 0 And CDec(m_vListData(ACSplitTotal, 0)) = CDec(0.0) Then
                        'SplitReceipt Button
                        m_vListData(ACSplitTotal, lRow) = m_vListData(ACSubAmount, lRow)
                        m_vListData(ACSubRealAllocationID, lRow) = 1
                        m_vListData(ACIsLeadAccount, lRow) = True
                        m_bAllocated = False
                        m_dSplitTotal = m_vListData(ACSplitTotal, lRow)
                        m_sMediaRefLead = CStr(m_vListData(ACSubMediaRef, 0))
                    ElseIf lRow = 0 Then
                        'EditSplit Button
                        m_vListData(ACSubRealAllocationID, lRow) = 1
                        m_bAllocated = False
                        m_dSplitTotal = m_vListData(ACSplitTotal, lRow)
                    Else
                        'EditSplit Button
                        m_vListData(ACSubRealAllocationID, lRow) = 1
                        m_bAllocated = False
                    End If

                End If

                ' Column 1 Media Ref
                'DC290704 PN13006 do distinguish between installment related line and others

                If Not Object.Equals(m_vListData(ACInstalmentArray, lRow), Nothing) Then
                    ' Column 1 Media Ref

                    oListItem = lvwListDetails.Items.Add(CStr(m_vListData(ACSubMediaRef, lRow)).Trim(), "CashListImage")
                Else
                    ' Column 1 Media Ref

                    oListItem = lvwListDetails.Items.Add(CStr(m_vListData(ACSubMediaRef, lRow)).Trim(), "CashListImage")
                End If

                ' Column 2 Media Type
                ' Get the Media Type from the Lookup table using ID
                iMediaTypeID = CInt(m_vListData(ACSubMediatypeID, lRow))

                m_lReturn = GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iMediaTypeID, sLookUpCaption:=sMediaType)

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sMediaType.Trim()

                'Column 3 Amount
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=m_vListData(ACSubAmount, lRow))
                'eck150800 Accumulate the amount
                'sw iif added to ensure that reversed items are not added to the total
                m_cTotalAmount += (IIf(CDbl(m_vListData(ACCashlistitemreversereasonid, lRow)) = 0, CDec(m_vListData(ACSubAmount, lRow)), 0))
                ' Column 4 Account Code
                m_lReturn = SetDefaultAccountProperties(CInt(m_vListData(ACSubAccountID, lRow)), False)

                If m_bIsLeadAccount Then
                    LeadAccountBranchId = m_iCompanyID
                End If

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_sAccountShortCode.Trim()
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = ""
                End If

                ' Column 5 Allocation Status
                ' Get the Allocation Status from the Lookup table using ID
                iAllocationStatusID = CInt(m_vListData(ACSubRealAllocationID, lRow))

                m_lReturn = GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupAllocationStatus, iLookUpID:=iAllocationStatusID, sLookUpCaption:=sAllocationStatus)

                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sAllocationStatus.Trim()

                If CDbl(m_vListData(ACCashlistitemreversereasonid, lRow)) <> 0 Then
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Reversed"
                End If

                'eck100701 Produce a letter
                m_bLetter = CBool(m_vListData(ACSubLetter, lRow))
                If m_bLetter Then
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "Y"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "N"
                End If
                ' {* USER DEFINED CODE (End) *}

                If chkSplitReceipt.Checked = True Then

                    If m_sCallingAppName = "SplitReceiptsFromFindTransaction" And m_vListData.GetUpperBound(1) = 0 And CDec(m_vListData(ACSplitTotal, 0)) = CDec(0.0) Then

                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "Lead"
                        m_dSplitTotal = CDec(m_vListData(ACSplitTotal, 0))
                        m_sMediaRefLead = CStr(m_vListData(ACSubMediaRef, 0))

                    ElseIf CBool(m_vListData(ACIsLeadAccount, lRow) = True) Then

                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "Lead"
                        m_dSplitTotal = CDec(m_vListData(ACSplitTotal, lRow))
                        m_sMediaRefLead = CStr(m_vListData(ACSubMediaRef, 0))

                    Else

                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = "Split"

                    End If
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = ""
                End If

                ' Set the tag property with the index of
                ' the List data storage.
                oListItem.Tag = CStr(lRow)

            Next lRow
            m_bOnLoad = False
            'eck250800

            If chkSplitReceipt.Checked = True Then
                pnlTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=m_dSplitTotal)
            Else
                pnlTotal.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=m_cTotalAmount)
            End If

            If m_cTotalAmount <> m_dSplitTotal Then
                cmdPost.Enabled = False
            End If
            ' Select the first item.
            lvwListDetails.FocusedItem = lvwListDetails.Items.Item(0)
            If (lvwListDetails.Items.Count > 0) Then
                lvwListDetails.FullRowSelect = True
                lvwListDetails.Items(0).Selected = True
                lvwListDetails.Select()
            End If

            ' AMB 06/03/2003: IS2495 - pseudo-click the first item

            'lvwListDetails_ItemClick(lvwListDetails, New EventArgs())

            If Not Information.IsNothing(lvwListDetails.FocusedItem) Then
                lvwListDetails_ItemClick(lvwListDetails.FocusedItem)
            ElseIf (lvwListDetails.SelectedItems.Count > 0) Then
                lvwListDetails_ItemClick(lvwListDetails.SelectedItems.Item(0))
            End If
            ' CF 040199 - Added to show short account code on form's caption
            'Me.Caption = Me.Caption & " (" & Trim(m_sAccountShortCode) & ")"

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the List data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            'BB No combo to populate but we need the list

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Const kAutoAllocateIfAble As Integer = 5059
        Const kSplitReceipt As Integer = 5091

        Dim sResult, sAppName, sSection As String
        Dim vDefault As Object

        Dim oCashListDrawer As bACTCashListDrawer.Business
        Dim sAutoAllocateIfAble As String = ""
        Dim sSplitReceipt As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Get what solution we're part of

            m_lReturn = g_oSirConfig.GetRegSettings(sResult:=sResult, sAppName:=sAppName, sSection:=sSection, sKey:=gACTLibrary.ACTOrionSolutionValue, vDefault:=vDefault)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMOK) Or (sResult = "0") Then
                ' Default to MBP style of solution
                sResult = CStr(gACTLibrary.ACTOrionSolutionMBP)
            End If

            g_iSolutionConfig = CInt(sResult)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            'SMJB:Don't know what to do with this
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select
            'eck100700
            m_bPosted = False

            ' {* USER DEFINED CODE (Begin) *}
            ' Set the column widths for the List headers.
            lvwListDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwListDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwListDetails.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1000))
            lvwListDetails.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwListDetails.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1020))
            lvwListDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1000))

            ' Enable/Disable the allocate button
            AllowAllocateButton = m_bAllowAllocateButton

            'Front Office receipting changes SW 08-10-2002

            m_lReturn = GetCashListDetails(v_lCashListID:=m_lCashlistID, r_vCashDrawerID:=m_vCashDrawerID, r_vBatchID:=m_vBatchID, r_bHasSecurityAccess:=m_bHasSecurityAccess, r_vListDate:=m_dtEffectiveDate)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vCashDrawerID = 0 Then
                'we are dealing with the old style cash list
                m_bCashDrawerType = False
            Else
                m_bCashDrawerType = True

                'set this to true as items will automatically post when they are created
                m_bPosted = True
                'we are dealing with the new cash drawers

                If Task = gPMConstants.PMEComponentAction.PMView Then
                    m_bAllowAllocateButton = False
                Else
                    'Allow allocation will be automatic for Cash Drawers sw front office receipting 26-11-2002
                    m_bAllowAllocateButton = True
                End If

                ' Create an instance of bACTCashListDrawer
                Dim temp_oCashListDrawer As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashListDrawer, "bACTCashListDrawer.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oCashListDrawer = temp_oCashListDrawer
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails Failed to create instance of bACTCashListdrawer", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    ' Remove the instance.

                    oCashListDrawer.Dispose()
                    oCashListDrawer = Nothing
                    Return result
                End If

                ' Getdetails

                m_lReturn = oCashListDrawer.GetDetails(v_lCashlistDrawerId:=m_vCashDrawerID, r_vDescription:=m_vCashDrawerDescription, r_vGenerateTask:=m_vGenerateTask, r_vFutureChequeDays:=m_vFutureChequeDays, r_vCashlistItemReceiptTypeId:=m_vDrawerReceiptType, r_vMediaTypeId:=m_vDrawerMediaType, r_vAllowReversals:=m_vAllowReversals, r_vClosed:=m_vCashDrawerClosed, r_vTaskIsUrgent:=m_vTaskIsUrgent, r_vTaskStatus:=m_vTaskStatus, r_vTaskDueDays:=m_vTaskDueDays, r_vPMUserGroupId:=m_vPMUserGroupId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for cashlistdrawerid " & m_vCashDrawerID, vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    ' Remove the instance.

                    oCashListDrawer.Dispose()
                    oCashListDrawer = Nothing
                    Return result
                End If

                ' Remove the instance.

                oCashListDrawer.Dispose()
                oCashListDrawer = Nothing

            End If

            m_lReturn = EnableButtons()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set default Auto Allocate If able

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kAutoAllocateIfAble, r_sOptionValue:=sAutoAllocateIfAble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If gPMFunctions.ToSafeInteger(sAutoAllocateIfAble) = 1 Then
                'Me.chkAutoAllocateIfAble.CheckState = CheckState.Checked
                Me.chkAutoAllocateIfAble.Checked = True
            Else
                'Me.chkAutoAllocateIfAble.CheckState = CheckState.Unchecked
                Me.chkAutoAllocateIfAble.Checked = False
            End If

            If (m_lCashListTypeID = 2) Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSplitReceipt, r_sOptionValue:=sSplitReceipt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeInteger(sSplitReceipt) = 1 Then
                    Me.chkSplitReceipt.Visible = True
                    lvwListDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1000))
                Else
                    Me.chkSplitReceipt.Visible = False
                    Me.chkSplitReceipt.Checked = False
                    lvwListDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(0))
                End If

                If m_sCallingAppName = "SplitReceiptsFromFindTransaction" Then
                    Me.chkSplitReceipt.Checked = True
                    Me.chkSplitReceipt.Enabled = False
                End If
            Else
                lvwListDetails.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(0))
            End If

            Return result

        Catch excep As System.Exception

            ' {* USER DEFINED CODE (End) *}

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdRemove.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRemoveButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            lvwListDetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwListDetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwListDetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwListDetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwListDetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'eck250800

            lblCashTotal.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            ' Set the icon to S4B if part of s4b...
            If g_iSolutionConfig = gACTLibrary.ACTOrionSolutionSFORB Then

                'imgIcon.Picture = imgSFORB.Picture

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCashListDetails
    '
    ' Description: Creates an instance of bACTCashList and gets the
    '              required details for the passed cashlist_id.
    '
    ' ***************************************************************** '
    Private Function GetCashListDetails(ByVal v_lCashListID As Integer, Optional ByRef r_vCashListStatusID As Object = Nothing, Optional ByRef r_vCashListRef As Object = Nothing, Optional ByRef r_vCompanyID As Object = Nothing, Optional ByRef r_vBankAccountID As Object = Nothing, Optional ByRef r_vCurrencyID As Object = Nothing, Optional ByRef r_vListDate As Object = Nothing, Optional ByRef r_vControlTotal As Object = Nothing, Optional ByRef r_vItemCount As Object = Nothing, Optional ByRef r_vCashDrawerID As Double = 0, Optional ByRef r_vBatchID As Object = Nothing, Optional ByRef r_bHasSecurityAccess As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oCashList As bACTCashList.Form
        Dim vResultArray(,) As Object

        Const ACCashDrawerId As Integer = 1

        Try

            'SMJB CQ1966 06/08/03: If cash drawer was locked then we won't have an ID here
            '(it will be 0, so test for it and quietly exit)
            If v_lCashListID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bACTCashList
            Dim temp_oCashList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashList = temp_oCashList

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails Failed to create instance of bACTCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Getdetails

            m_lReturn = oCashList.GetDetails(vCashListID:=v_lCashListID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for cashlistid " & v_lCashListID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oCashList.GetNext(vCashListID:=v_lCashListID, vCashListStatusID:=r_vCashListStatusID, vCashListRef:=r_vCashListRef, vCompanyID:=r_vCompanyID, vBankAccountID:=r_vBankAccountID, vCurrencyID:=r_vCurrencyID, vListDate:=r_vListDate, vControlTotal:=r_vControlTotal, vItemCount:=r_vItemCount, vCashlist_drawer_id:=r_vCashDrawerID, vBatch_id:=r_vBatchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to getnext for cashlistid " & v_lCashListID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Remove the instance.

                oCashList.Dispose()
                oCashList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 13/10/2003 : Also check to see if r_vCashDrawerID has passed into or not.
            ' DD 25/09/2003
            ' Moved security check here. Essentially any user can view a cash drawer
            ' but only those with security rights can add/reverse/allocate

            If Not Information.IsNothing(r_vCashDrawerID) Then
                If r_vCashDrawerID > 0 Then

                    r_bHasSecurityAccess = False

                    ' Get CashLists User has access to

                    m_lReturn = oCashList.GetAllUserCashListDrawer(v_lUserId:=g_oObjectManager.UserID, r_vResultArray:=vResultArray)

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get details.
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run GetAllUserCashListDrawer in business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails")

                        Return result

                    Else
                        ' Scan all rows in array data
                        If Information.IsArray(vResultArray) Then

                            For iRowCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                                ' Check Cash Drawer is in Users available list

                                If Conversion.Val(CStr(vResultArray(ACCashDrawerId, iRowCount))) = r_vCashDrawerID Then
                                    r_bHasSecurityAccess = True
                                    Exit For
                                End If
                            Next iRowCount
                        End If
                    End If
                Else
                    'Not a cash drawer so everyone has access
                    r_bHasSecurityAccess = True
                End If
            Else
                'Not a cash drawer so everyone has access
                r_bHasSecurityAccess = True
            End If

            ' Remove the instance.

            oCashList.Dispose()
            oCashList = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' CreateWorkManagerTask
    ''' </summary>
    ''' <param name="dPlanAmount"></param>
    ''' <param name="dOutstandingAmount"></param>
    ''' <param name="m_sPlanref"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateWorkManagerTask(ByVal dPlanAmount As Decimal, dOutstandingAmount As Decimal, ByVal m_sPlanref As String) As Integer
        Dim result As Integer = 0
        Dim lTaskInstanceCnt As Integer
        Dim sTaskDesc, vCashListRef As String
        Dim vListDate As Date
        Dim vKeyArray(,) As Object

        Dim sCashItemMediaRef, sCashItemOurRef, sCashItemTheirRef As String
        Dim cCashItemAmount As Decimal
        Dim sTaskDescComplete As New StringBuilder
        Dim vOptionValue As Object
        Dim sUserGroup As String = ""

        Dim oAccount As bACTAccount.Form
        Dim sAccountShortCode As String = ""
        Dim dReceiptOS As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetCashListDetails(v_lCashListID:=m_lCashlistID, r_vCashListRef:=vCashListRef, r_vListDate:=vListDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListDetails.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            m_lReturn = m_oBusiness.GetCashListType(v_lCashListTypeID:=m_lCashListTypeID, r_sCashListType:=sTaskDesc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListType.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            'Creating Account Business Object
            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
            oAccount = temp_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                Return result
            End If

            If Not Information.IsArray(m_vListData) Then
                Return result
            End If
            For iLoop As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                If m_lCashListItemID = 0 Then
                    m_lCashListItemID = CInt(m_vListData(ACSubCashListItemID, iLoop))
                End If
                sCashItemMediaRef = gPMFunctions.NullToString(m_vListData(ACSubMediaRef, iLoop))
                sCashItemOurRef = gPMFunctions.NullToString(m_vListData(ACSubOurRef, iLoop))
                sCashItemTheirRef = gPMFunctions.NullToString(m_vListData(ACSubTheirRef, iLoop))
                cCashItemAmount = Math.Abs(gPMFunctions.NullToCurrency(m_vListData(ACSubAmount, iLoop)))
                dReceiptOS = cCashItemAmount - dPlanAmount

                'Getting Account Short Code

                m_lReturn = oAccount.GetDetails(vAccountID:=m_vListData(ACSubAccountID, iLoop))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                    Return result
                End If

                m_lReturn = oAccount.GetNext(vShortCode:=sAccountShortCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                End If

                ReDim vKeyArray(2, 5)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCashListItemID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lCashlistID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lCashListTypeID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameActionKey

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = gACTLibrary.ACTApprove

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = gACTLibrary.ACTUseListHidden

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClaimPaymentId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lClaimPaymentId

                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                    Return result
                End If

                If gPMFunctions.NullToString(vOptionValue) = "1" Then
                    'Get the user group for first step
                    m_lReturn = GetStepGroupCode(r_sGroupCode:=sUserGroup)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                        sUserGroup = "SLACS"
                    End If
                Else
                    sUserGroup = "SLACS"
                End If

                If cCashItemAmount > dPlanAmount Then
                    sTaskDescComplete = New StringBuilder("Premium fully allocated to" & New String(" "c, 1))
                    sTaskDescComplete.Append("plan no: " & m_sPlanref & New String("."c, 1))
                    sTaskDescComplete.Append(". Extra Premium on the receipt is " & StringsHelper.Format(dReceiptOS, "#,##0.00"))
                Else
                    sTaskDescComplete = New StringBuilder("Premium partially allocated to" & New String(" "c, 1))
                    sTaskDescComplete.Append("plan no: " & m_sPlanref & New String("."c, 1))
                    sTaskDescComplete.Append(" Outstanding premium is " & StringsHelper.Format(dOutstandingAmount, "#,##0.00"))
                End If

                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=sAccountShortCode, v_sDescription:=sTaskDescComplete.ToString(), v_dtTaskDueDate:=DateTime.Today.AddDays(1).AddSeconds(-1), v_sTaskCode:="ACTRCTV2", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:="", v_vKeyArray:=vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process AddTaskToWorkManager.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                    Return result
                End If

            Next iLoop

            oAccount = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                        If m_lDisplayState <> gACTLibrary.ACTUseListHidden Then
                            ' Check the details havn't changed.

                            m_lReturn = m_oBusiness.Cancel()

                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                            ' Check message result.
                            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                                ' Set return to false, meaning
                                ' don't cancel.
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    Else
                        If Not m_bPosted And m_vCashDrawerID = 0 Then
                            'Need to skip updation of cashlistitem for underwriting
                            'Everything should be in place by now
                        Else
                            ' Update the details using the business object.

                            m_lReturn = m_oBusiness.Update()

                            ' Check for errors.
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' Failed to update the details
                                result = gPMConstants.PMEReturnCode.PMFalse

                                ' Log Error.
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                            End If
                        End If
                    End If

            End Select

            ' Remove the allocationid

            m_lReturn = m_oBusiness.DeleteUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_bDeleteAll:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' Display details form and Add, Edit or View
    ''' </summary>
    ''' <param name="iTaskType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetailsFormProcess(ByRef iTaskType As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            ' MEvans : 22-05-2003 : CQ 709.
            Dim lIssuedID As Integer
            Dim bProceed As Boolean

            If frmDetails Is Nothing Then
                frmDetails = New frmDetails
            End If
            frmDetails.Business = m_oBusiness

            ' Call the initialise method passing a business ref
            ' and a ref to this List form
            m_lReturn = frmDetails.Initialise(oListForm:=Me)

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return nResult
            End If

            ' Pass standard details to form properties
            With frmDetails

                .PMUserGroupId = CInt(Conversion.Val(m_vPMUserGroupId))
                .CallingAppName = m_sCallingAppName
                .Task = iTaskType
                .Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                'WPR12- Enhancement Quote Collection Process
                .CashListActualCalledFrom = m_sCashListActualCalledFrom
                .MultipleQuoteSelected = m_bMultiplePoliciesSelected
                .QuotePartyCnt = m_lQuoteClientCnt
                .QuoteAgentCnt = m_lQuoteAgentCnt
                .QuoteAgentType = m_sQuoteAgentType

                'AR20050210 - PN18698/PN18699
                'Only pass in AccountId if set by calling application
                If g_bHasAccountContext Then
                    .AccountID = m_lAccountID
                Else
                    .AccountID = 0
                End If

                .Amount = m_cAmount
                .Amounttendered = m_cAmount
                .Letter = m_bLetter
                .ViaInsurerPayment = m_bViaInsurerPayment
                .ViaClaimPayment = m_bViaClaimPayment 'AR20050125 - PN18271
                .ViaFinancePlan = m_bViaFinancePlan
                .ApprovalType = m_lApprovalType
                .CashListRoadmap = m_sCashListRoadmap
                .CurrencyID = m_iCurrencyID
                .CompanyID = m_iCompanyID
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .LeadAccountBranchId = LeadAccountBranchId
                'AR20050125 - PN18271
                If m_lMediaTypeID > 0 Then
                    m_iMediaTypeID = m_lMediaTypeID
                    .MediaTypeID = m_lMediaTypeID
                End If
                .OurRef = m_sOurRef
                .TheirRef = m_sTheirRef
                .ScreenType = m_sScreenType
                .ClaimPaymentId = m_lClaimPaymentId
                .SilentMultiCurrencyScreen = m_bSilentMultiCurrencyScreen
                .WriteOffAmount = m_cWriteOffAmount
                .IsWOFF = m_bWOFF
                .ViaBanking = m_bViaBanking
                .ForRecommendation = m_sForRecommendation
                If chkSplitReceipt.Checked = True Then
                    .SplitTotal = m_dSplitTotal
                    .IsLeadAccount = m_bIsLeadAccount
                    .IsSplitReceipt = True
                    .CollectionHasLead = m_bCollectionHasLead
                    .MediaRefLead = m_sMediaRefLead
                    .SplitReceiptRunningTotal = m_dSplitReceiptRunningTotal
                Else
                    .IsSplitReceipt = False
                End If
                If iTaskType = PMEComponentAction.PMEdit Then
                    If CDbl(m_vListData(ACvCashlistitemreceipttypeid, m_lSelectedRow)) = 8 Then
                        bProceed = MessageBox.Show("All Policies attached with this Receipt will be removed." &
                                                   Strings.Chr(13) & Strings.Chr(10) &
                                                   "Do you want to continue?",
                                                   Application.ProductName, MessageBoxButtons.YesNo)
                        m_lReturn = RebuildBGArray(RemoveAt:=Convert.ToString(lvwListDetails.FocusedItem.Tag))
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("RebuildBGArray", "RebuildBGArray Failed",
                                                    PMELogLevel.PMLogError)
                        End If

                    End If
                End If

                If Information.IsArray(m_vBGAllPoliciesForReceipt) Then
                    .SelectdBGPolicies = m_vBGAllPoliciesForReceipt
                End If
                'PN 56728
                If m_lPartyBankId > 0 Then
                    .PartyBank_Id = m_lPartyBankId
                End If
                .TaxBandID = m_nTaxBandId
                .TaxAmount = m_crTaxAmount
                .BIC = m_sBIC
                .IBAN = m_sIBAN
                .PlanRef = m_sPlanref
            End With

            ' If Edit or View populate Details Form properties
            ' from List data array
            Select Case (iTaskType)
                Case PMEComponentAction.PMEdit,
                    PMEComponentAction.PMView,
                    PMEComponentAction.PMReplace

                    'sw payment maintenance 11-05-2002
                    If m_sActionkey = gACTLibrary.ACTEditCheque Then
                        frmDetails.ActionKey = gACTLibrary.ACTEditCheque
                    ElseIf m_sActionkey = gACTLibrary.ACTCancelCheque Then
                        frmDetails.ActionKey = gACTLibrary.ACTCancelCheque
                        frmDetails.m_lTransDetailID =
                            CInt(m_vListData(ACSubTransdetailId, Convert.ToString(Me.lvwListDetails.FocusedItem.Tag)))
                    ElseIf m_sActionkey = gACTLibrary.ACTApprove Then
                        frmDetails.ActionKey = gACTLibrary.ACTApprove
                    ElseIf m_sActionkey = gACTLibrary.ACTStopCheque Then
                        frmDetails.ActionKey = gACTLibrary.ACTStopCheque
                    End If

                    ' Set the row to the selected item
                    If Not IsNothing(lvwListDetails.FocusedItem) Then
                        m_lSelectedRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)
                    ElseIf lvwListDetails.SelectedItems.Count > 0 Then
                        m_lSelectedRow = Convert.ToString(lvwListDetails.SelectedItems(0).Tag)
                    Else
                        m_lSelectedRow = Convert.ToString(lvwListDetails.Items(0).Tag)
                    End If

                    ' Populate form properties
                    m_lReturn = DataToDetailsForm(lRow:=m_lSelectedRow)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = PMEReturnCode.PMFalse
                        frmDetails = Nothing
                        Return nResult
                    End If

                    'sw front office receipting, if we are replacing a reversed receipt
                    'then change some of the properties so that the new details are added
                    If iTaskType = PMEComponentAction.PMReplace Then
                        With frmDetails
                            .Replacescashlistitemid = .CashlistitemID
                            .CashListItemReverseReasonID = 0
                            .Reason = ""
                            .Stopconfirmationdate = Nothing
                            .CashlistitemID = 0
                            .Task = PMEComponentAction.PMAdd
                        End With
                        iTaskType = PMEComponentAction.PMAdd
                        m_lSelectedRow = 0
                        frmDetails.m_bReverseCashDrawerListItem = False
                    Else
                        frmDetails.m_bReverseCashDrawerListItem = False
                        ' Inform the business of the current record
                        ' so that the lookup ID match works
                        m_oBusiness.CurrentRecord = m_vListData(m_vListData.GetUpperBound(0), m_lSelectedRow)
                    End If

                Case PMEComponentAction.PMAdd
                    ' Set the row to zero
                    m_lSelectedRow = 0
                    ' Populate default form properties unless a replacement, 
                    'then populate with existing details
                    m_lReturn = DetailsFormDefaults()
                    'SMJB CQ2155 02/09/03
                    frmDetails.MediaRef = m_sMediaRef
                    'CMG/PB 08012003 LossSchedule PS202
                    If m_bLossSchedule Then
                        frmDetails.AccountID = m_lAccountID
                        'Set tendered to the same as amount else
                        'we get an error box which does this anyway
                        frmDetails.Amounttendered = Amount
                        frmDetails.AllocationStatusID = 2
                        frmDetails.CashListItemPaymentStatusID = CashListItemPaymentStatusID
                    End If
                    'End CMG

                    ' AMB 21/02/2003: PS220 - added for Manage Debtors development
                    If m_sActionkey = gACTLibrary.ACTRefund Then
                        With frmDetails
                            'KG 03/09/03
                            'CQ 2102 - Disable cashlist Item for Refunds
                            .Task = PMEComponentAction.PMAdd
                            .ActionKey = gACTLibrary.ACTRefund
                            .AccountID = m_lAccountID
                            .PaymentName = m_sPaymentName
                            .MediaTypeID = m_lMediaTypeID
                            .CashListItemPaymentTypeID = m_iCashListItemPaymentTypeID '' PN 3733
                            .CashListItemPaymentStatusID = m_lCashlistitemPaymentStatusId
                        End With
                    End If

                    If m_sActionkey = gACTLibrary.ACTUnderwritingDirect Then
                        frmDetails.ActionKey = gACTLibrary.ACTUnderwritingDirect
                    End If

                    frmDetails.m_bReverseCashDrawerListItem = False
                    ' Check for errors.
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = PMEReturnCode.PMFalse
                        frmDetails = Nothing
                        Return nResult
                    End If

                Case PMEComponentAction.PMReverse
                    ' Set the row to the selected item
                    m_lSelectedRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)
                    ' Populate form properties
                    m_lReturn = DataToDetailsForm(lRow:=m_lSelectedRow)
                    frmDetails.m_bReverseCashDrawerListItem = True
                    frmDetails.m_lTransDetailID =
                        CInt(m_vListData(ACSubTransdetailId, Convert.ToString(Me.lvwListDetails.FocusedItem.Tag)))
                    ' Check for errors.
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = PMEReturnCode.PMFalse
                        frmDetails = Nothing
                        Return nResult
                    End If
                    ' Inform the business of the current record
                    ' so that the lookup ID match works
                    m_oBusiness.CurrentRecord = m_vListData(m_vListData.GetUpperBound(0), m_lSelectedRow)
                Case Else
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                       sMsg:="Unknown Case condition",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="DetailsFormProcess")
            End Select

            ' Call the Load method to setup the interface details
            m_lReturn = frmDetails.LoadForm()

            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return nResult
            End If

            ' Offset this form relative to parent
            m_lReturn = iACTFunc.SetChildFormPosition(frmParent:=Me, frmChild:=frmDetails)
            ' Call the ShowForm method to show the form, allow user input etc.
            m_lReturn = frmDetails.ShowForm(lDisplayState:=FormShowConstants.Modal)
            ' Check for errors.
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                frmDetails = Nothing
                Return nResult
            End If

            If (ActionKey = gACTLibrary.ACTEditCheque) OrElse
                (ActionKey = gACTLibrary.ACTCancelCheque) OrElse
                (ActionKey = gACTLibrary.ACTStopCheque) Then
                'exit the function as the neccessary updates have been completed
                frmDetails.Close()
                frmDetails = Nothing
                Return nResult
            End If

            ' If OK was pressed and Edit or Add return List data
            ' and update Business from Details Form properties
            If frmDetails.Status = PMEReturnCode.PMOK Then

                ' MEvans : 22-05-2003 : CQ 709
                ' Although task has been a view ( so user cant edit any details)
                ' we want to update the payment status to "issued". To enable this
                ' change task type to edit and frmdetails.payment status to issued,
                ' and let existing update process take place.

                'update the Status
                m_lStatus = PMEReturnCode.PMOK
                If m_lApprovalType = ACApprovalTypeClaimPayment Then
                    If m_oBusiness.GetPaymentStatusIDFromCode("ISS", lIssuedID) = PMEReturnCode.PMTrue Then
                        CashListItemPaymentStatusID = lIssuedID
                        ' need to reload the details from the business so we dont lose any data..
                        m_lReturn = DataToDetailsForm(0)
                        frmDetails.CashListItemPaymentStatusID = lIssuedID
                        ' commit the change to the items status to the database
                        m_lReturn = DetailsCommit(PMEComponentAction.PMEdit)
                        ' default the status to OK as we want to complete our task
                        ' and we have successfully returned from frmdetails
                        m_lStatus = PMEReturnCode.PMOK
                    End If
                End If

                Select Case (iTaskType)
                    Case PMEComponentAction.PMEdit,
                        PMEComponentAction.PMAdd,
                        PMEComponentAction.PMReverse
                        ' Use Details properties to update data array and business
                        m_lReturn = DetailsFormReturn(lRow:=m_lSelectedRow, iTaskType:=iTaskType)
                        ' Check for errors.
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            nResult = PMEReturnCode.PMFalse
                            frmDetails.Close()
                            frmDetails = Nothing
                            Return nResult
                        End If

                        If m_vCashDrawerID = 0 Or iTaskType = PMEComponentAction.PMReverse Then
                            ' Refresh list from data if we are dealing with old style cash list
                            'for cash drawers, refresh done from cmd_Add Click event
                            m_lReturn = DetailsFormList(lRow:=m_lSelectedRow)
                            ' Check for errors.
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                nResult = PMEReturnCode.PMFalse
                                frmDetails.Close()
                                frmDetails = Nothing
                                Return nResult
                            End If
                        End If
                        m_lReturn = MergeBGPolicies()
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("MergeBGPolicies", "MergeBGPolicies Failed", PMELogLevel.PMLogError)
                        End If
                End Select
            Else
                If frmDetails.Status = PMEReturnCode.PMCancel Then
                    nResult = PMEReturnCode.PMCancel
                End If
            End If

            frmDetails.Close()
            frmDetails = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    Private Function MergeBGPolicies() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeBGPolicies"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim uboundBankDetails As Integer

            If Not gPMFunctions.IsArrayEmpty(m_vBGAllPoliciesForReceipt) Then

                uboundBankDetails = m_vBGAllPoliciesForReceipt.GetUpperBound(1) + 1

                ReDim Preserve m_vBGAllPoliciesForReceipt(m_vBGAllPoliciesForReceipt.GetUpperBound(0), uboundBankDetails)
            Else
                ReDim m_vBGAllPoliciesForReceipt(MainModule.ENBankGuarantee.LastItem, 0)
            End If

            uboundBankDetails = m_vBGAllPoliciesForReceipt.GetUpperBound(1)

            If Information.IsArray(m_vSelectdBGPoliciesItem) Then

                For lRowCount As Integer = m_vSelectdBGPoliciesItem.GetLowerBound(1) To m_vSelectdBGPoliciesItem.GetUpperBound(1)

                    For lColCount As Integer = m_vSelectdBGPoliciesItem.GetLowerBound(0) To m_vSelectdBGPoliciesItem.GetUpperBound(0)

                        m_vBGAllPoliciesForReceipt(lColCount, m_vBGAllPoliciesForReceipt.GetUpperBound(1)) = m_vSelectdBGPoliciesItem(lColCount, lRowCount)
                    Next

                    'lSelRowCount = lSelRowCount + 1

                    ReDim Preserve m_vBGAllPoliciesForReceipt(m_vBGAllPoliciesForReceipt.GetUpperBound(0), m_vBGAllPoliciesForReceipt.GetUpperBound(1) + 1)
                Next

                ReDim Preserve m_vBGAllPoliciesForReceipt(m_vBGAllPoliciesForReceipt.GetUpperBound(0), m_vBGAllPoliciesForReceipt.GetUpperBound(1) - 1)

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
    ' Name: DetailsDelete (Standard Method)
    '
    ' Description: Deletes a newly added set of details
    '
    ' ***************************************************************** '
    Private Function DetailsDelete() As Integer

        Dim result As Integer = 0
        Dim lSelRow As Integer
        Dim iRes As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    ' If item has non zero ID then it has already been committed to the DB
            '    ' so we won't allow a delete
            '    ' This can be removed to allow deletion of committed records
            '    If m_vListData(ACSubCashListItemID, (lvwListDetails.SelectedItem.Tag)) <> 0 Then
            '        DetailsDelete = PMFalse
            '        Exit Function
            '    End If

            '    Let's replace the above with a check to see if it's been posted
            '    ' if so we won't allow a delete
            If CDbl(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) > gACTLibrary.ACTAllocationStatusUnallocated Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'check that the user really wants to delete the item
            'AR20050210 - Improve message
            iRes = MessageBox.Show("Are you sure you want to delete the selected item?", "Cash List Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If iRes = System.Windows.Forms.DialogResult.No Then
                Return result
            End If

            ' Set the row to the selected item
            lSelRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)

            ' Check the row is in range
            If lSelRow < 0 Or lSelRow > m_vListData.GetUpperBound(1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Delete details from Business object
            m_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMDelete, lRow:=lSelRow)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If array has only one entry(ie 0) delete the array
            ' otherwise delete the entry
            If m_vListData.GetUpperBound(1) = 0 Then
                'Set to Nothing will have knock on effect
                m_vListData = Nothing
            Else
                ' Shift the data in the data array to fill the empty space
                ' vacated by the deleted row.
                For lRow As Integer = lSelRow To m_vListData.GetUpperBound(1) - 1
                    For iCol As Integer = 0 To m_vListData.GetUpperBound(0)
                        m_vListData(iCol, lRow) = m_vListData(iCol, lRow + 1)
                    Next iCol
                Next lRow

                ' Resize array to free storage space used by
                ' deleted row.
                ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) - 1)
            End If

            ' Refresh contents of List box
            m_lReturn = DataToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete details", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DetailsFormReturn (Standard Method)
    '
    ' Description: Return List data and update Business
    '              from Details Form properties
    '
    ' ***************************************************************** '
    Private Function DetailsFormReturn(ByRef lRow As Integer, ByRef iTaskType As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add expand List array to take new entry
            If iTaskType = gPMConstants.PMEComponentAction.PMAdd And m_sForRecommendation <> "F" Then

                ' If the array exists increment it
                ' otherwise must be the first entry so set it up
                If Information.IsArray(m_vListData) Then
                    ReDim Preserve m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1) + 1)
                Else
                    'eck100701
                    ReDim m_vListData(ACLastItem, 0)
                End If

                ' Store unique key for this row used to point to business collection.
                ' This must always be last key added + 1, as deletion does not actually
                ' remove the record from business collection it just flags for deletion
                m_lLastDataID += 1
                m_vListData(m_vListData.GetUpperBound(0), m_vListData.GetUpperBound(1)) = m_lLastDataID

                ' Set row to point to newly added List data array row
                lRow = m_vListData.GetUpperBound(1)

            End If

            ' Populate List array from Details Form properties
            m_lReturn = DetailsFormToData(lRow:=lRow)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update Business from List array
            m_lReturn = DataToBusiness(lMode:=iTaskType, lRow:=lRow)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update data from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormReturn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DetailsFormList
    '
    ' Description: Redisplay List after Details Form return
    '
    ' ***************************************************************** '
    Private Function DetailsFormList(ByRef lRow As Integer) As Integer

        Dim result As Integer = 0
        ' Dim lstItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Refresh contents of List box

            m_lReturn = DataToInterface()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sScreenType = "CLP" Then
                Me.Visible = True
                lvwListDetails.FocusedItem = lvwListDetails.Items.Item(lRow)
                Me.Visible = False
            End If
            lvwListDetails.FocusedItem = lvwListDetails.Items.Item(lRow)
            ' Simulate a click so the relevant buttons are activated/de-activated
            '10/03/2003 - PWC - Issue (ref:2876)
            'Rob noticed the following line of code and asked me to change it
            '(lstItem was not being set)
            'lvwListDetails_ItemClick Item:=lstItem

            If Not Information.IsNothing(lvwListDetails.FocusedItem) Then
                lvwListDetails_ItemClick(lvwListDetails.FocusedItem)
            ElseIf (lvwListDetails.SelectedItems.Count > 0) Then
                lvwListDetails_ItemClick(lvwListDetails.SelectedItems.Item(0))
            End If

            ' If item has allocation status of 'unallocated' then allow delete/edit
            ' This code also in List.ItemClick
            'sw front office receipting, added in clause to check item has not been reversed

            If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                If CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) = gACTLibrary.ACTAllocationStatusUnallocated Or CInt(m_vListData(ACCashlistitemreversereasonid, lRow)) <> 0 Then
                    ' Enable the Remove button
                    cmdRemove.Enabled = True
                    cmdEdit.Enabled = True
                    cmdAllocate.Enabled = m_bAllowAllocateButton
                    cmdPost.Enabled = True
                Else
                    ' Disable the Remove button
                    cmdRemove.Enabled = False
                    cmdEdit.Enabled = False
                    cmdAllocate.Enabled = False
                    cmdPost.Enabled = False
                End If

            End If
            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to refresh list from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Use data array to populate details form properties
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToDetailsForm(ByRef lRow As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try
            ' Pass details to form properties
            With frmDetails
                ' {* USER DEFINED CODE (Begin) *}

                If m_bCashDrawerType Then
                    .CDFutureChequeDays = m_vFutureChequeDays
                    .CashDrawerID = m_vCashDrawerID
                    .CDBatchID = m_vBatchID
                    .CashDrawerDescription = m_vCashDrawerDescription
                Else
                    .CashDrawerID = 0
                    .CDBatchID = 0
                    .CashDrawerDescription = ""
                End If

                .CashlistitemID = CInt(m_vListData(ACSubCashListItemID, lRow))
                'DC300704 PN13747 show correct status
                .AllocationStatusID = CInt(m_vListData(ACSubRealAllocationID, lRow))
                .MediaTypeID = CInt(m_vListData(ACSubMediatypeID, lRow))
                .CashlistID = CInt(m_vListData(ACSubCashListID, lRow))
                .AccountID = CInt(m_vListData(ACSubAccountID, lRow))
                .MediaRef = CStr(m_vListData(ACSubMediaRef, lRow))
                .OurRef = CStr(m_vListData(ACSubOurRef, lRow))
                .TheirRef = CStr(m_vListData(ACSubTheirRef, lRow))
                .Amount = CDec(m_vListData(ACSubAmount, lRow))
                .ContactName = CStr(m_vListData(ACSubContactName, lRow))
                .Address1 = CStr(m_vListData(ACSubAddress1, lRow))
                .Address2 = CStr(m_vListData(ACSubAddress2, lRow))
                .Address3 = CStr(m_vListData(ACSubAddress3, lRow))
                .Address4 = CStr(m_vListData(ACSubAddress4, lRow))
                .PostalCode = CStr(m_vListData(ACSubPostalCode, lRow))
                .AddressCountry = CInt(m_vListData(ACSubAddressCountry, lRow))
                .PaymentName = CStr(m_vListData(ACSubPaymentName, lRow))
                .PaymentAccountCode = CStr(m_vListData(ACSubPaymentAccountCode, lRow))
                .PaymentBranchCode = CStr(m_vListData(ACSubPaymentBranchCode, lRow))
                .PaymentExpiryDate = CDate(m_vListData(ACSubPaymentExpiryDate, lRow))
                .PaymentReference1 = CStr(m_vListData(ACSubPaymentReference1, lRow))
                .PaymentReference2 = CStr(m_vListData(ACSubPaymentReference2, lRow))
                .CashlistTypeID = m_lCashListTypeID
                'eck100701
                .Letter = CBool(m_vListData(ACSubLetter, lRow))
                'front office receipting changes
                .BatchID = CInt(m_vListData(ACBatchid, lRow))
                .PMUserid = CInt(m_vListData(ACPMUserid, lRow))
                .Transactiondate = CDate(m_vListData(ACTransactiondate, lRow))
                .Originalamount = CDec(m_vListData(ACOriginalamount, lRow))
                .Amounttendered = CDec(m_vListData(ACAmounttendered, lRow))
                .Change = CDec(m_vListData(ACChange, lRow))
                .Cashlistitemreceipttypeid = CInt(m_vListData(ACvCashlistitemreceipttypeid, lRow))
                .Cashlistitemreceiptstatusid = CInt(m_vListData(ACCashlistitemreceiptstatusid, lRow))
                .Cashlistitembankid = CInt(m_vListData(ACCashlistitembankid, lRow))
                .Chequedate = m_vListData(ACChequedate, lRow)
                .CCnumber = CStr(m_vListData(ACCCnumber, lRow))
                .CCexpirydate = m_vListData(ACCCexpirydate, lRow)
                .CCstartdate = m_vListData(ACCCstartdate, lRow)
                .CCissue = CStr(m_vListData(ACCCissue, lRow))
                .CCpin = CStr(m_vListData(ACCCpin, lRow))
                .CCauthcode = CStr(m_vListData(ACCCauthcode, lRow))
                .CCname = CStr(m_vListData(ACCCname, lRow))
                .CCcustomer = CStr(m_vListData(ACCCcustomer, lRow))
                .CCmanualauthcode = CStr(m_vListData(ACCCmanualauthcode, lRow))
                .CCtransactioncode = CStr(m_vListData(ACCCtransactioncode, lRow))
                .MediaTypeIssuerID = CInt(m_vListData(ACMediatype_IssuerID, lRow))
                .Receiptdetails = CStr(m_vListData(ACReceiptdetails, lRow))
                .CashListItemReversePMUserID = CInt(m_vListData(ACCashlistitemreversepmuserid, lRow))
                .CashListItemReverseReasonID = CInt(m_vListData(ACCashlistitemreversereasonid, lRow))
                .CashListItemPaymentTypeID = CInt(m_vListData(ACCashListItemPaymentTypeID, lRow))
                .CashListItemPaymentStatusID = CInt(m_vListData(ACCashListItemPaymentStatusID, lRow))
                .Datepresented = m_vListData(ACDatepresented, lRow)
                .Chequeinpossession = CBool(m_vListData(ACChequeinpossession, lRow))
                .Stoprequesteddate = m_vListData(ACStoprequesteddate, lRow)
                .Stopprinteddate = m_vListData(ACStopprinteddate, lRow)
                .Stopconfirmationdate = m_vListData(ACStopconfirmationdate, lRow)
                .Reason = CStr(m_vListData(ACReason, lRow))
                .Replacescashlistitemid = CInt(m_vListData(ACReplacescashlistitemid, lRow))
                .UnderwritingYearID = m_vListData(ACUnderwritingYearID, lRow)
                .CurrencyBaseDate = CDate(m_vListData(ACCurrencyBaseDate, lRow))
                .CurrencyBaseXRate = CDbl(m_vListData(ACCurrencyBaseXrate, lRow))
                .AccountBaseDate = CDate(m_vListData(ACAccountBaseDate, lRow))
                .AccountBaseXrate = CDbl(m_vListData(ACAccountBaseXrate, lRow))
                .SystemBaseDate = CDate(m_vListData(ACSystemBaseDate, lRow))
                .SystemBaseXrate = CDbl(m_vListData(ACSystemBaseXrate, lRow))
                .PartyBankPaymentTypeId = m_vListData(ACBankPaymentTypeId, lRow)
                .CollectionDate = CDate(m_vListData(ACCollectionDate, lRow))
                .Comments = CStr(m_vListData(ACComments, lRow))
                .BankLocation = CStr(m_vListData(ACBankLocation, lRow))
                .BBankBranch = CStr(m_vListData(ACBankBranch, lRow))
                .ChequeTypeId = CInt(m_vListData(ACChequeTypeId, lRow))
                .CCBankId = CInt(m_vListData(ACCCBankId, lRow))
                .CardTypeId = CInt(m_vListData(ACCardTypeId, lRow))
                .CardTransSlipNo = CStr(m_vListData(ACCardTransSlipNo, lRow))
                .ChequeClearingTypeId = CInt(m_vListData(ACChequeClearingTypeId, lRow))
                .LeadAccountBranchId = LeadAccountBranchId
                .TaxBandID = m_vListData(kTaxBandID, lRow)
                .TaxAmount = m_vListData(kTaxAmount, lRow)
                .sBIC = m_vListData(kBIC, lRow)
                .sIBAN = m_vListData(kIBAN, lRow)
                .InsuranceRef = CStr(m_vListData(ACInsuranceRef, lRow))
            End With

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update details form from data", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetailsForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Use details form properties to populate data array
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetailsFormToData(ByRef lRow As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            With frmDetails
                m_lCashListItemID = .CashlistitemID
                m_vListData(ACSubCashListItemID, lRow) = .CashlistitemID
                m_vListData(ACSubAllocationstatusID, lRow) = .AllocationStatusID
                m_vListData(ACSubMediatypeID, lRow) = .MediaTypeID
                m_vListData(ACSubCashListID, lRow) = .CashlistID
                m_vListData(ACSubAccountID, lRow) = .AccountID
                m_vListData(ACSubRealAllocationID, lRow) = .AllocationStatusID
                ' CF150199 - This doesn't seem right, but it was requested...
                m_lAccountID = .AccountID
                m_vListData(ACSubMediaRef, lRow) = .MediaRef
                m_vListData(ACSubOurRef, lRow) = .OurRef
                m_vListData(ACSubTheirRef, lRow) = .TheirRef
                If (m_lCashListTypeID = 1) Or (m_lCashListTypeID = 3) Then
                    ' Payment
                    m_vListData(ACSubAmount, lRow) = Math.Abs(.Amount) * -1
                Else
                    ' Receipt
                    m_vListData(ACSubAmount, lRow) = Math.Abs(.Amount)
                End If
                m_vListData(ACSubContactName, lRow) = .ContactName
                m_vListData(ACSubAddress1, lRow) = .Address1
                m_vListData(ACSubAddress2, lRow) = .Address2
                m_vListData(ACSubAddress3, lRow) = .Address3
                m_vListData(ACSubAddress4, lRow) = .Address4
                m_vListData(ACSubPostalCode, lRow) = .PostalCode
                m_vListData(ACSubAddressCountry, lRow) = .AddressCountry
                m_vListData(ACSubPaymentName, lRow) = .PaymentName
                m_vListData(ACSubPaymentAccountCode, lRow) = .PaymentAccountCode
                m_vListData(ACSubPaymentBranchCode, lRow) = .PaymentBranchCode
                m_vListData(ACSubPaymentExpiryDate, lRow) = .PaymentExpiryDate
                m_vListData(ACSubPaymentReference1, lRow) = .PaymentReference1
                m_vListData(ACSubPaymentReference2, lRow) = .PaymentReference2
                'eck10701
                m_vListData(ACSubLetter, lRow) = .Letter
                'front office receipting changes

                'don't update if approval
                If LCase(frmDetails.cmdOK.Text) <> "approve" Then
                    m_vListData(ACBatchid, lRow) = .BatchID
                End If
                m_vListData(ACPMUserid, lRow) = .PMUserid
                m_vListData(ACTransactiondate, lRow) = .Transactiondate
                m_vListData(ACOriginalamount, lRow) = .Originalamount
                m_vListData(ACAmounttendered, lRow) = .Amounttendered
                m_vListData(ACChange, lRow) = .Change
                m_vListData(ACvCashlistitemreceipttypeid, lRow) = .Cashlistitemreceipttypeid
                m_vListData(ACCashlistitemreceiptstatusid, lRow) = .Cashlistitemreceiptstatusid
                m_vListData(ACCashlistitembankid, lRow) = .Cashlistitembankid
                m_vListData(ACChequedate, lRow) = .Chequedate
                m_vListData(ACCCnumber, lRow) = .CCnumber
                m_vListData(ACCCexpirydate, lRow) = .CCexpirydate
                m_vListData(ACCCstartdate, lRow) = .CCstartdate
                m_vListData(ACCCissue, lRow) = .CCissue
                m_vListData(ACCCpin, lRow) = .CCpin
                m_vListData(ACCCauthcode, lRow) = .CCauthcode
                m_vListData(ACCCname, lRow) = .CCname
                m_vListData(ACCCcustomer, lRow) = .CCcustomer
                m_vListData(ACCCmanualauthcode, lRow) = .CCmanualauthcode
                m_vListData(ACCCtransactioncode, lRow) = .CCtransactioncode
                m_vListData(ACMediatype_IssuerID, lRow) = .MediaTypeIssuerID
                m_vListData(ACReceiptdetails, lRow) = .Receiptdetails
                m_vListData(ACCashlistitemreversepmuserid, lRow) = .CashListItemReversePMUserID
                m_vListData(ACCashlistitemreversereasonid, lRow) = .CashListItemReverseReasonID
                m_vListData(ACCashListItemPaymentStatusID, lRow) = .CashListItemPaymentStatusID
                m_vListData(ACCashListItemPaymentTypeID, lRow) = .CashListItemPaymentTypeID
                m_vListData(ACDatepresented, lRow) = .Datepresented
                m_vListData(ACChequeinpossession, lRow) = .Chequeinpossession
                m_vListData(ACStoprequesteddate, lRow) = .Stoprequesteddate
                m_vListData(ACStopprinteddate, lRow) = .Stopprinteddate
                m_vListData(ACStopconfirmationdate, lRow) = .Stopconfirmationdate
                m_vListData(ACReason, lRow) = .Reason
                m_vListData(ACReplacescashlistitemid, lRow) = .Replacescashlistitemid
                m_vListData(ACInstalmentArray, lRow) = .InstalmentArray
                m_vListData(ACSalvageArray, lRow) = .SalvageArray
                m_vListData(ACCLMUSRecoveryArray, lRow) = .m_vUnsavedRBIItems
                m_vListData(ACCLMRVRecoveryArray, lRow) = .m_vReversedRBIItems
                m_vListData(ACUnderwritingYearID, lRow) = .UnderwritingYearID
                m_vListData(ACCurrencyBaseDate, lRow) = .CurrencyBaseDate
                m_vListData(ACCurrencyBaseXrate, lRow) = .CurrencyBaseXRate
                m_vListData(ACAccountBaseDate, lRow) = .AccountBaseDate
                m_vListData(ACAccountBaseXrate, lRow) = .AccountBaseXrate
                m_vListData(ACSystemBaseDate, lRow) = .SystemBaseDate
                m_vListData(ACSystemBaseXrate, lRow) = .SystemBaseXrate
                m_vListData(ACOverrideReason, lRow) = .OverrideReason
                'Party Bank Details
                m_vListData(ACBankPaymentTypeId, lRow) = .m_vBankPaymentTypeId
                m_vListData(ACBaseAmount, lRow) = .BaseAmount
                m_vListData(ACCollectionDate, lRow) = .CollectionDate
                m_vListData(ACComments, lRow) = .Comments
                m_vListData(ACBGPolicies, lRow) = .SelBGPoliciesItemForReceipt
                m_vSelectdBGPoliciesItem = .SelBGPoliciesItemForReceipt

                'WPR12- Enhancement Quote Collection Process
                m_vListData(ACBankLocation, lRow) = .BankLocation
                m_vListData(ACBankBranch, lRow) = .BBankBranch
                m_vListData(ACChequeTypeId, lRow) = .ChequeTypeId
                m_vListData(ACCCBankId, lRow) = .CCBankId
                m_vListData(ACCardTypeId, lRow) = .CardTypeId
                m_vListData(ACCardTransSlipNo, lRow) = .CardTransSlipNo
                m_vListData(ACChequeClearingTypeId, lRow) = .ChequeClearingTypeId
                m_vListData(ACIsLeadAccount, lRow) = .IsLeadAccount
                m_vListData(ACSplitTotal, lRow) = .SplitTotal
                If .IsLeadAccount Then
                    LeadAccountBranchId = .LeadAccountBranchId
                End If
                m_vListData(ACReceiptTypeIsInstalmentBased, lRow) = .ReceiptTypeIsInstalmentBased
                m_vListData(kTaxBandID, lRow) = .TaxBandID
                m_vListData(kTaxAmount, lRow) = .TaxAmount
                m_vListData(kBIC, lRow) = .sBIC
                m_vListData(kIBAN, lRow) = .sIBAN
                m_vListData(ACInsuranceRef, lRow) = .InsuranceRef
            End With

            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update data from details form", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DetailsFormDefaults (Standard Method)
    '
    ' Description: Set default values for a Details Add
    '
    ' ***************************************************************** '
    Private Function DetailsFormDefaults() As Integer

        Dim result As Integer = 0
        Dim lIssuedID As Integer
        'Const kPaymentTab As Integer = 3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Setup default details form properties
            With frmDetails
                ' {* USER DEFINED CODE (Begin) *}
                If m_bCashDrawerType Then
                    .CDFutureChequeDays = m_vFutureChequeDays
                    .CashDrawerID = m_vCashDrawerID
                    .CDBatchID = m_vBatchID
                    .Cashlistitemreceipttypeid = IIf(m_vDrawerReceiptType = "", 0, CInt(m_vDrawerReceiptType))
                    .MediaTypeID = IIf(m_vDrawerMediaType = "", 0, CInt(m_vDrawerMediaType))
                    .CashDrawerDescription = m_vCashDrawerDescription
                Else
                    .CashDrawerID = 0
                    .CDBatchID = 0
                    .CashDrawerDescription = ""
                    '.MediaTypeID = ACTMediaTypeCheque
                End If

                .CashlistitemID = 0
                .AllocationStatusID = gACTLibrary.ACTAllocationStatusUnallocated
                .CashlistID = m_lCashlistID

                'AR20050210 - PN18698/PN18699
                If g_bHasAccountContext Then
                    If m_lAccountID <> 0 Or m_bViaClaimPayment Then
                        .AccountID = m_lAccountID
                    Else
                        .AccountID = 0
                    End If
                Else
                    .AccountID = 0
                End If

                'If the policy ref is passed in through set keys then
                'populate our ref field on the details form
                If m_sReceiptPolicyRef.Trim() <> "" Then
                    .OurRef = m_sReceiptPolicyRef
                Else
                    .OurRef = m_sOurRef
                End If

                .TheirRef = m_sTheirRef

                .CashlistTypeID = m_lCashListTypeID
                .CurrencyID = m_iCurrencyID
                .Transactiondate = DateTime.Today
                .CollectionDate = DateTime.Today
                .Comments = ""

                If m_lCashListTypeID = ACReceiptType Then
                    .Cashlistitemreceiptstatusid = gPMConstants.PMEComponentAction.PMAdd
                    'don't default receipt type for cash drawers as this is defined in the drawer set up
                    If m_vCashDrawerID = 0 Then
                        .Cashlistitemreceipttypeid = 1 'premium
                    End If
                Else
                    If m_bLossSchedule Then
                        'If this is loss schedule, we are passing in the payment status
                        'because the user may not be authorised to create payments immediatley
                        .CashListItemPaymentStatusID = m_lCashlistitemPaymentStatusId
                    Else
                        'look up the ID for an issued status

                        m_oBusiness.GetPaymentStatusIDFromCode("ISS", lIssuedID)
                        'set to ID
                        .CashListItemPaymentStatusID = lIssuedID 'issued

                    End If
                End If

                'Required for Auto-Allocation
                .InsuranceFileCnt = m_lInsuranceFileCnt

                .GISSchemeID = m_lGISSchemeID
                .GISDataModelCode = m_sGISDataModelCode

                If m_cAmount <> 0 Then
                    .Amount = m_cAmount
                End If

                If m_cWriteOffAmount <> 0 Then
                    .WriteOffAmount = m_cWriteOffAmount
                End If

                .ViaInsurerPayment = m_bViaInsurerPayment
                .ViaClaimPayment = m_bViaClaimPayment 'AR20050125 - PN18271

                '' ''PN 3733
                If m_bViaInsurerPayment Then
                    .CashListItemPaymentTypeID = m_iCashListItemPaymentTypeID
                    .InsurerPaymentInstArray = m_vInsurerPaymentInstArray
                    If m_iCashListItemReceiptTypeID <> 0 Then
                        .Cashlistitemreceipttypeid = m_iCashListItemReceiptTypeID
                    End If
                End If
                ''''''''''''''
                'AR20050210 - PN18698/PN18699
                'Only populate bank details if have an account id
                If m_lAccountID <> 0 And (g_bHasAccountContext) Then
                    m_lReturn = SetDefaultAccountProperties(v_lAccountId:=m_lAccountID, v_bCompanyChangeDefault:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    .ContactName = ""
                    .Address1 = ""
                    .Address2 = ""
                    .Address3 = ""
                    .Address4 = ""
                    .PostalCode = ""
                    .AddressCountry = 0
                    .PaymentName = ""
                    .PaymentAccountCode = ""
                    .PaymentBranchCode = ""
                    '.PaymentExpiryDate = Date    'PN18882
                    .PaymentReference1 = ""
                    .PaymentReference2 = ""
                    .Receiptdetails = ""
                    .BIC = ""
                    .IBAN = ""
                End If

                .Letter = False

                'WPR12- Enhancement Quote Collection Process
                .BankLocation = ""
                .BBankBranch = ""
                .ChequeTypeId = -1
                .CCBankId = -1
                .CardTypeId = -1
                .CardTransSlipNo = ""
                .ChequeClearingTypeId = -1

                ' 

                'All values that have to be same for lead & split. Set from SetSplitReceiptDefaults()
                'Use these values only for IsLeadAccount = False
                .MediaTypeID = m_iMediaTypeID
                .CCname = m_sCCNameLead
                .CCnumber = m_sCCNumberLead
                .CCexpirydate = m_sCCExpiryDateLead
                .CCissue = m_sCCissueLead
                .CCpin = m_sCCpinLead
                .CCstartdate = m_sCCstartdateLead
                .CCauthcode = m_sCCauthcodeLead
                .CCmanualauthcode = m_sCCmanualauthcodeLead
                .CCcustomer = m_sCCcustomerLead
                .CCtransactioncode = m_sCCtransactioncodeLead
                If ViaInsurerPayment And InsurerAccountID = 0 Then
                    ' Local field
                    InsurerAccountID = m_lAccountID
                End If
                .InsurerAccountID = InsurerAccountID

                'Also tab5 only visible on batch receipts

                ' {* USER DEFINED CODE (End) *}
            End With

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to setup details form defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetDefaultAccountProperties(ByVal v_lAccountId As Integer, Optional ByVal v_bCompanyChangeDefault As Boolean = True) As Integer
        Dim result As Integer = 0

        Dim oAccount As bACTAccount.Form
        Dim sContactName As New FixedLengthString(60)
        Dim sAddress1 As New FixedLengthString(40)
        Dim sAddress2 As New FixedLengthString(40)
        Dim sAddress3 As New FixedLengthString(40)
        Dim sAddress4 As New FixedLengthString(40)
        Dim sPostalCode As New FixedLengthString(20)
        Dim iAddressCountry As Integer
        Dim sPaymentName As New FixedLengthString(60)
        Dim sPaymentAccountCode As New FixedLengthString(60)
        Dim sPaymentBranchCode As New FixedLengthString(30)
        Dim dtPaymentExpiryDate As Date
        Dim sPaymentReference1 As New FixedLengthString(30)
        Dim sPaymentReference2 As New FixedLengthString(30)
        Dim sTitle, sMessage As String
        Dim sBIC As String
        Dim sIBAN As String
        ' RDC 14112003
        Dim vAllowElectronicPayment As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lAccountId <> 0 Then 'PN22728

                Dim temp_oAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
                oAccount = temp_oAccount

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Display error stating the problem.
                    ' Get description from the resource file.

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Display message.
                    MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTAccount", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                m_lReturn = oAccount.GetDetails(vAccountID:=v_lAccountId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultAccountProperties")

                    Return result
                End If
                If v_bCompanyChangeDefault Then
                    m_lReturn = oAccount.GetNext(vShortCode:=m_sAccountShortCode, vContactName:=sContactName.Value, vAddress1:=sAddress1.Value, vAddress2:=sAddress2.Value, vAddress3:=sAddress3.Value, vAddress4:=sAddress4.Value, vPostalCode:=sPostalCode.Value, vAddressCountry:=iAddressCountry, vPaymentName:=sPaymentName.Value, vPaymentAccountCode:=sPaymentAccountCode.Value, vPaymentBranchCode:=sPaymentBranchCode.Value, vPaymentExpiryDate:=dtPaymentExpiryDate, vPaymentReference1:=sPaymentReference1.Value, vPaymentReference2:=sPaymentReference2.Value, vAllowElectronicPayment:=vAllowElectronicPayment, vCompanyId:=m_iCompanyID)
                Else
                    m_lReturn = oAccount.GetNext(vShortCode:=m_sAccountShortCode, vContactName:=sContactName.Value, vAddress1:=sAddress1.Value, vAddress2:=sAddress2.Value, vAddress3:=sAddress3.Value, vAddress4:=sAddress4.Value, vPostalCode:=sPostalCode.Value, vAddressCountry:=iAddressCountry, vPaymentName:=sPaymentName.Value, vPaymentAccountCode:=sPaymentAccountCode.Value, vPaymentBranchCode:=sPaymentBranchCode.Value, vPaymentExpiryDate:=dtPaymentExpiryDate, vPaymentReference1:=sPaymentReference1.Value, vPaymentReference2:=sPaymentReference2.Value, vAllowElectronicPayment:=vAllowElectronicPayment)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultAccountProperties")
                End If

                oAccount = Nothing

            Else
                ' Cater for no account match  PN22728
                sContactName.Value = ""
                sAddress1.Value = ""
                sAddress2.Value = ""
                sAddress3.Value = ""
                sAddress4.Value = ""
                sPostalCode.Value = ""
                iAddressCountry = 0
                sPaymentName.Value = ""
                sPaymentAccountCode.Value = ""
                sPaymentBranchCode.Value = ""
                sPaymentReference1.Value = ""
                sPaymentReference2.Value = ""

            End If

            If frmDetails Is Nothing Then
                frmDetails = New frmDetails
            End If

            With frmDetails
                .ContactName = sContactName.Value
                .Address1 = sAddress1.Value
                .Address2 = sAddress2.Value
                .Address3 = sAddress3.Value
                .Address4 = sAddress4.Value
                .PostalCode = sPostalCode.Value
                .AddressCountry = iAddressCountry

                'AR20050125 -PN18271
                If m_sPayeeName.Length = 0 Then
                    .PaymentName = sPaymentName.Value
                Else
                    .PaymentName = m_sPayeeName
                End If

                If m_sPayeeAccountCode.Length = 0 Then
                    .PaymentAccountCode = sPaymentAccountCode.Value
                Else
                    .PaymentAccountCode = m_sPayeeAccountCode
                End If

                If m_sPayeeSortCode.Length = 0 Then
                    .PaymentBranchCode = sPaymentBranchCode.Value
                Else
                    .PaymentBranchCode = m_sPayeeSortCode
                End If
                If Not m_sAccountShortCode = "CLMPAYABLE" Then
                    .PaymentExpiryDate = dtPaymentExpiryDate
                End If
                '.PaymentExpiryDate = dtPaymentExpiryDate 'PN18882
                .PaymentReference1 = sPaymentReference1.Value
                .PaymentReference2 = sPaymentReference2.Value

                .AllowElectronicPayment = vAllowElectronicPayment

                'AR20050125 -PN18271
                If m_sPayeeComments.Length > 0 Then
                    .Reason = m_sPayeeComments
                End If
                .CompanyID = m_iCompanyID

            End With
            'End (Sriram P)PN 54063
            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to setup account defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDefaultAccountProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AllocateCashListItem
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function AllocateCashListItem(ByVal v_lRow As Integer) As Integer
        Dim result As Integer = 0
        Dim vAllocationArray(0, gACTLibrary.k_ACAllocationArraySize) As Object
        Dim vExtraInformation As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = m_oBusiness.GetPostedTransaction(lTransdetailId:=m_vListData(ACSubTransdetailId, v_lRow), r_vResultArray:=vExtraInformation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vExtraInformation) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to call GetPostedTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateCashListItem")
                Return result
            End If

            vAllocationArray(0, gACTLibrary.k_ACTransDetail_id) = m_vListData(ACSubTransdetailId, v_lRow)

            vAllocationArray(0, gACTLibrary.k_ACDocument_Ref) = CStr(vExtraInformation(0, 0)).Trim()

            vAllocationArray(0, gACTLibrary.k_ACTransactionCurrencyID) = vExtraInformation(1, 0)

            vAllocationArray(0, gACTLibrary.k_ACTransactionCurrency) = vExtraInformation(2, 0)

            vAllocationArray(0, gACTLibrary.k_ACTransactionAmount) = CDbl(m_vListData(ACSubAmount, v_lRow)) * -1

            vAllocationArray(0, gACTLibrary.k_ACBaseCurrencyID) = vExtraInformation(3, 0)

            vAllocationArray(0, gACTLibrary.k_ACBaseCurrency) = vExtraInformation(4, 0)

            vAllocationArray(0, gACTLibrary.k_ACBaseOutstanding) = CDbl(vExtraInformation(6, 0))

            If m_oAllocation Is Nothing Then
                Dim temp_m_oAllocation As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAllocation, sClassName:="iACTAllocation.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAllocation = temp_m_oAllocation
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of iACTAllocation.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateCashListItem")
                    Return result
                End If
            End If

            'Call the Allocation UI

            m_oAllocation.AllocationArray = vAllocationArray

            m_oAllocation.CallingAppName = "iACTCashListItem"

            m_oAllocation.AccountID = m_vListData(ACSubAccountID, v_lRow)

            m_oAllocation.CompanyID = g_iSourceID

            m_oAllocation.CashListTypeID = m_lCashListTypeID

            m_oAllocation.CashListItemID = CInt(m_vListData(ACSubCashListItemID, v_lRow))

            m_oAllocation.SelectedCurrencyId = m_iCurrencyID

            m_oAllocation.SelectedSourceId = m_iCompanyID

            m_oAllocation.Start()

            If m_oAllocation.Status = gPMConstants.PMEReturnCode.PMOK Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllocateCashListItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' AutomaticAllocate
    ''' </summary>
    ''' <param name="v_lRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AutomaticAllocate(ByVal v_lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim vKeys As Object
        Dim lAllocationID As Integer

        Try

            result = PMEReturnCode.PMTrue

            ' CF 170999
            ' If we have a batch id then pass that as well
            If m_lBatchID <> 0 Then
                ReDim vKeys(1, 4)
            Else
                ReDim vKeys(1, 3)
            End If

            ' AccountID
            vKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID
            vKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID 'm_vListData(ACSubAccountID, v_lRow)

            ' AllocationID
            vKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAllocationId
            vKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = lAllocationID

            ' CashListItemID
            vKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListItemId
            vKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lCashListItemID 'm_vListData(ACSubCashListItemID, v_lRow)

            ' CashListID
            vKeys(PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListId
            vKeys(PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lCashlistID 'm_vListData(ACSubCashListID, v_lRow)

            ' Add batch id if we have one
            If m_lBatchID <> 0 Then
                vKeys(PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameBatchID
                vKeys(PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lBatchID
            End If

            Dim temp_m_oInsurerPaymentAllocateBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsurerPaymentAllocateBusiness,
                                                     "bACTInsurerPaymentAllocate.Business",
                                                     vInstanceManager:=PMGetViaClientManager)
            m_oInsurerPaymentAllocateBusiness = temp_m_oInsurerPaymentAllocateBusiness
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
            ' Set the keys
            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to set navigator keys.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="AutomaticAllocate",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.Start()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to start Insurer Payment Allocate Business.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="AutomaticAllocate",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.GetKeys(vKeyArray:=vKeys)
            m_lAllocationID = CInt(vKeys(1, 0))
            m_oInsurerPaymentAllocateBusiness.Dispose()


            m_oInsurerPaymentAllocateBusiness = Nothing
            m_vListData(ACSubRealAllocationID, v_lRow) = gACTLibrary.ACTAllocationStatusAllocated
            m_lReturn = DetailsFormList(lRow:=m_lSelectedRow)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'eck230801 Wasn't updating the Cash List
            m_vListData(ACSubRealAllocationID, m_lSelectedRow) = gACTLibrary.ACTAllocationStatusAllocated
            m_lReturn = DataToBusiness(lMode:=PMEComponentAction.PMEdit, lRow:=m_lSelectedRow)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to update the business.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="AutoAllocate",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If

            m_lReturn = m_oBusiness.Update()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to update the business.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="AutoAllocate",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If

            m_lReturn = DetailsFormList(m_lSelectedRow)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to display details on the list.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="m_oNav_NavigatorClose",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If




            m_lReturn = DisableForm(bEnabled:=True)
            Return result
        Catch excep As System.Exception
            result = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="AutomaticAllocate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutomaticAllocate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Disables the form for when the Navigator process
    '              is running.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef bEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Return result
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is ListView Then
                    oControl.Enabled = bEnabled
                ElseIf TypeOf oControl Is Button Then
                    oControl.Enabled = bEnabled
                End If
            Next oControl

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *********************************************************************** '
    '
    ' Name: AutoAllocateCash
    '
    ' Description: Automatically allocates cash. Called from Client Manager.
    '
    ' History: 29/02/2000 CTAF - Created.
    '
    ' *********************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AutoAllocateCash) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AutoAllocateCash(ByVal v_lRow As Integer) As Integer
    'Dim result As Integer = 0
    'Dim iACTCashReceipt As Object
    '

    'Dim oCashReceipt As iACTCashReceipt.Interface_Renamed
    'Dim lAllocationID As Integer
    'Dim vAllocationArray(0, gACTLibrary.k_ACAllocationArraySize) As Object 'eck130904
    'Dim vExtraInformation As Object 'eck130904
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'DOUBLING TRANSMATCH FIX
    'EK 23/12/99 set the allocation id to 0
    'before running the road map
    'lAllocationID = 0

    'm_lReturn = m_oBusiness.UpdateUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vPropertyValue:=lAllocationID)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'lAllocationID = 0
    'End If
    'EK 23/12/99 END
    'eck130904

    'm_lReturn = m_oBusiness.GetPostedTransaction(lTransDetailID:=m_vListData(ACSubTransdetailId, v_lRow), r_vResultArray:=vExtraInformation)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vExtraInformation) Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to call GetPostedTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateCashListItem")
    'Return result
    'End If
    '

    'vAllocationArray(0, gACTLibrary.k_ACTransDetail_id) = m_vListData(ACSubTransdetailId, v_lRow)

    'vAllocationArray(0, gACTLibrary.k_ACDocument_Ref) = CStr(vExtraInformation(0, 0)).Trim()

    'vAllocationArray(0, gACTLibrary.k_ACTransactionCurrencyID) = vExtraInformation(1, 0)

    'vAllocationArray(0, gACTLibrary.k_ACTransactionCurrency) = vExtraInformation(2, 0)

    'vAllocationArray(0, gACTLibrary.k_ACTransactionAmount) = CDbl(m_vListData(ACSubAmount, v_lRow)) * -1

    'vAllocationArray(0, gACTLibrary.k_ACBaseCurrencyID) = vExtraInformation(3, 0)

    'vAllocationArray(0, gACTLibrary.k_ACBaseCurrency) = vExtraInformation(4, 0)

    'vAllocationArray(0, gACTLibrary.k_ACBaseOutstanding) = vExtraInformation(5, 0)
    'eck130904End
    '
    '
    '
    'Dim temp_oCashReceipt As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oCashReceipt, sClassName:="iACTCashReceipt.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oCashReceipt = temp_oCashReceipt
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iACTCashReceipt.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocateCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    '
    ' set the keys
    '

    'oCashReceipt.AccountID = m_vListData(ACSubAccountID, v_lRow)

    'oCashReceipt.CashlistID = m_vListData(ACSubCashListID, v_lRow)

    'oCashReceipt.CashlistitemID = m_vListData(ACSubCashListItemID, v_lRow)

    'oCashReceipt.AllocationID = lAllocationID

    'oCashReceipt.BatchID = m_lBatchID
    'eck130904

    'oCashReceipt.AllocationArray = vAllocationArray

    'oCashReceipt.CallingAppName = "iACTCashListItem"

    'oCashReceipt.CompanyID = g_iSourceID
    'eck130904End
    '
    '
    'DN 10/01/03 ISS 1731 - Pass Cash List Type

    'oCashReceipt.CashlistTypeID = m_lCashListTypeID
    '

    'm_lReturn = oCashReceipt.Start()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start oCashReceipt.Start()", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocateCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    '

    'm_lReturn = oCashReceipt.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate oCashReceipt.Terminate()", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocateCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'End If
    '
    'oCashReceipt = Nothing
    '
    ' Say its complete
    'm_oNav_SetProcessStatus(v_bProcessComplete:=True)
    '
    ' Process the rest of the bits
    'm_oNav_NavigatorClose()
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoAllocateCash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocateCash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdAllocate_Click(ByVal eventSender As Object,
                                  ByVal eventArgs As EventArgs) Handles cmdAllocate.Click

        'eck230501
        Dim lAccountID, lTransDetailID, lPaymentTypeID As Integer
        Dim sPurchInvNo As String = ""
        Dim vDocument As Object

        ' AMB 06/03/2003: IS2495 - check that something's actually been selected
        If lvwListDetails.FocusedItem Is Nothing Then
            MessageBox.Show("Please select an item to allocate.",
                            "Allocate",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Get the CashListItemID that was selected
        If chkSplitReceipt.Checked Then
            m_lSelectedRow = Convert.ToString(lvwListDetails.Items(0).Tag)
            lvwListDetails.FocusedItem = lvwListDetails.Items(0)
        Else
            m_lSelectedRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)
        End If


        m_lReturn = GetBusiness()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                               sMsg:="Failed to GetBusiness.",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="cmdAllocate_Click",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description)
            m_lReturn = DisableForm(bEnabled:=True)
            Exit Sub
        End If

        m_lReturn = BusinessToData()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                               sMsg:="Failed to perform BusinessToData.",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="cmdAllocate_Click",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description)
            m_lReturn = DisableForm(bEnabled:=True)
            Exit Sub
        End If

        'eck250800 Don't let them allocate the same row twice
        If CDbl(m_vListData(ACSubRealAllocationID, m_lSelectedRow)) = gACTLibrary.ACTAllocationStatusAllocated Then
            MessageBox.Show("This item has already been allocated. Please re-select.",
                            "Allocation already done",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim sOptionValue As String = ""
        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                  v_vBranch:=g_oObjectManager.SourceID,
                                                  r_vUnderwriting:=sOptionValue)

        If (m_lReturn <> PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="Failed to process GetProductOptionValue.",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                               vErrNo:=Err.Number, vErrDesc:=Err.Description)
            Exit Sub
        End If

        If sOptionValue = "1" AndAlso m_sCashListRoadmap = ACTInsurerPaymentRoadMap Then
            m_lReturn = GetOption(v_iOptionNumber:=kSysOptIncludeInsurerPaymentMultiStep,
                  r_sOptionValue:=m_sIncludeInsurerPaymentMultiStep)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then

                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                        sMsg:="Failed to process GetOption.",
                        vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                        vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Sub
            End If
        End If

        If (m_sCashListRoadmap <> PMNavKeyConst.ACTInsurerPaymentRoadMap) OrElse
            (m_sCashListRoadmap = ACTInsurerPaymentRoadMap And m_sIncludeInsurerPaymentMultiStep = "1") Then
            'check to see that the item has posted
            If m_sForRecommendation <> "F" Then
                If CStr(m_vListData(ACSubTransdetailId, m_lSelectedRow)).Trim() = "" OrElse
                    gPMFunctions.NullToLong(CStr(m_vListData(ACSubTransdetailId, m_lSelectedRow)).Trim()) = 0 Then

                    If m_lCashListTypeID = 1 Or m_lCashListTypeID = 3 Then '1 = Payment , 2 = Receipt
                        ' At this stage the item is not posted.
                        If gPMFunctions.NullToLong(CStr(m_vListData(ACCashListItemPaymentStatusID, m_lSelectedRow)).Trim()) = ACStatusPendingID Then
                            If Not m_bApprovalMsg Then
                                ''Start(Saurabh Agrawal)PN 58035
                                If m_sScreenType <> "CLP" Then
                                    MessageBox.Show("This payment requires further approval. It cannot be allocated until fully approved.",
                                                    "Allocation not allowed",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Exclamation)
                                End If
                                ''End(Saurabh Agrawal)PN 58035
                            End If
                        Else
                            MessageBox.Show("Selected Item has not been posted. Please re-select.",
                                            "Item not posted",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation)
                        End If
                    Else
                        MessageBox.Show("Selected Item has not been posted. Please re-select.",
                                        "Item not posted",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                    End If

                    Exit Sub
                End If
            End If
        End If
        'EK 20/12/99 If we are in Insurer Payment then allocate automatically
        If m_sCashListRoadmap = PMNavKeyConst.ACTInsurerPaymentRoadMap Then
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            'WriteOff DocumentRef
            m_lReturn = UpdateWriteOffDocumentRef()
            If m_lReturn = PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError("CmdAllocate_Click",
                                        "UpdateWriteOffDocumentRef Failed",
                                        PMEReturnCode.PMLogError1)
                Exit Sub
            End If
            m_lReturn = AutomaticAllocate(v_lRow:=m_lSelectedRow)
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
        Else
            'eck230501 Try to auto Allocate first
            'eck230501 If this allocation clears the client balance - auto allocate
            lAccountID = CInt(m_vListData(ACSubAccountID, m_lSelectedRow))
            lTransDetailID = CInt(m_vListData(ACSubTransdetailId, m_lSelectedRow))
            lPaymentTypeID = CInt(m_vListData(ACCashListItemPaymentTypeID, m_lSelectedRow))
            sPurchInvNo = CStr(m_vListData(ACSubOurRef, m_lSelectedRow)).Trim()

            m_lReturn = m_oBusiness.GetDocumentFromTransdetail(v_lTransdetailId:=lTransDetailID, r_vResults:=vDocument)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to get document details for the transaction.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="cmdAllocate_Click",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If

            If Information.IsArray(vDocument) Then
                m_sDocumentRef = CStr(vDocument(1, 0))
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            'sw 30/01/2003, pass in the optional parameters, only ever used for claim payments though
            m_lReturn = AutoAllocate(v_lAccountId:=lAccountID,
                                     v_lTransdetailID:=lTransDetailID,
                                     v_lPaymentType:=lPaymentTypeID)

            'If AutoAllocate fails then try to match manually
            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lReturn = AllocateCashListItem(v_lRow:=m_lSelectedRow)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    MessageBox.Show("Cash list item has not been allocated.", "Not " &
                                    "Allocated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            'If either way returns PMTrue then update screen.
            If m_lReturn = PMEReturnCode.PMTrue Then
                m_vListData(ACSubRealAllocationID, m_lSelectedRow) = gACTLibrary.ACTAllocationStatusAllocated
                'eck temp
                m_vListData(ACSubAllocationstatusID, m_lSelectedRow) = gACTLibrary.ACTAllocationStatusAllocated
                'ecktempend
                m_lReturn = DetailsFormList(m_lSelectedRow)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                       sMsg:="Failed to display details on the list.",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="cmdAllocate_Click",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    'Set back to true to show that allocation worked.
                    m_lReturn = PMEReturnCode.PMTrue
                End If
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

        End If

        If m_lReturn <> PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                               sMsg:="Failed to allocate cash list item.",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="cmdAllocate_Click",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description)
            m_lReturn = DisableForm(bEnabled:=True)
            Exit Sub
        End If

        'Refresh to prevent the list from looking funny when there are scrollbars.
        lvwListDetails.Refresh()
        lvwListDetails.FocusedItem.EnsureVisible()
        'Start at beginning of collection
        If m_sForRecommendation = "F" Then
            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)
            If m_lReturn = PMEReturnCode.PMTrue Then
                m_lReturn = LetterProcessing(m_lCashListItemID)
            End If
        End If

        'Remove entries from list which have been allocated
        'If Me.chkAutoAllocateIfAble.CheckState = CheckState.Checked Then
        If Me.chkAutoAllocateIfAble.Checked = True Then
            For lRow As Integer = 1 To lvwListDetails.Items.Count
                If lRow > lvwListDetails.Items.Count Then
                    Exit For
                End If
                If ListViewHelper.GetListViewSubItem(lvwListDetails.Items.Item(lRow - 1), 4).Text.Trim().ToUpper() = ("Allocated").Trim().ToUpper() Then
                    lvwListDetails.Items.RemoveAt(lRow - 1)
                    lRow -= 1
                End If
            Next
        End If
        ' Allocated now
        m_bAllocated = True
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID2)

    End Sub

    ''' <summary>
    ''' to post a cash list or individual cash list item dpdt on whether the
    ''' cashlistitemID is passed.
    ''' </summary>
    ''' <param name="v_lCashlistItemID"></param>
    ''' <param name="r_iAllocationStatus"></param>
    ''' <remarks></remarks>
    Public Sub Post(Optional ByVal v_lCashlistItemID As Integer = 0, Optional ByRef r_iAllocationStatus As Integer = 0)

        Const kMethodName As String = "Post"
        Dim oKeyArray(,) As Object
        Dim nWriteOffReasonID As Integer
        Dim crWriteOffAmount As Decimal
        Dim bCurrencyWriteOff As Boolean
        Dim nAccountID As Integer
        Dim oAutoAllocationState As PMEReturnCode
        Dim bPartPayment As Boolean
        Dim bDoAllocation As Boolean
        Dim nResponse As DialogResult

        Dim crBaseAmount As Decimal
        Dim crCurrAmount As Decimal
        Dim crRate As Decimal
        Dim sFailureReason As String = ""
        Dim sReceiptType As String = ""
        Dim nCount As Integer
        Dim vCashListItem As Object
        Dim bReceiptTypeIsInstalmentBased As Boolean
        Dim bThirdPartyOnly As Boolean
        Const klUserError As Long = 32767
        Dim oResultArray(,) As Object = Nothing
        Dim oClaimPaymentIDs() As Object = Nothing
        Dim vInstalmentArrayTP(,) As Object
        Dim vInstalmentIDArrayTP(,) As Object
        Dim lIDTPArrayCount As Integer
        Dim lUBound As Integer
        Dim dInsAmount As Decimal
        Dim dPartialInsAmount As Decimal
        Dim oInstalments As bSIRPFInstalments.Business
        Dim nresult As Integer = 0
        Dim dOutstandingAmount As Decimal
        Dim nPremiumFinanceVersion As Integer
        Dim dPlanAmount As Decimal

        Try

            'Update the business object so the cash list id's are on the database
            m_lReturn = m_oBusiness.Update()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to update the database with the details in the business object",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Post",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                m_lReturn = DisableForm(bEnabled:=True)
                Exit Sub
            End If

            If frmDetails Is Nothing Then
                frmDetails = New frmDetails
            End If
            frmDetails.Business = m_oBusiness

            ' Call the initialise method passing a business ref
            ' and a ref to this List form
            m_lReturn = frmDetails.Initialise(oListForm:=Me)

            ' Check for errors.
            m_sPlanref = frmDetails.PlanRef


            For nCount = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                If ToSafeInteger(m_vListData(ACvCashlistitemreceipttypeid, nCount)) <> 0 Then
                    bReceiptTypeIsInstalmentBased = ToSafeBoolean(m_vListData(ACReceiptTypeIsInstalmentBased, nCount))
                End If

                m_lReturn = m_oBusiness.GetNext(vCashListItem)
                If m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMEOF Then
                    RaiseError("Post", "Failed to get next cash list item details.", klUserError)
                End If

                If m_vListData(ACInstalmentArray, nCount) IsNot Nothing AndAlso m_vListData(ACInstalmentArray, nCount)(0, 0) = -1 Then
                    bReceiptTypeIsInstalmentBased = False
                    bThirdPartyOnly = True
                End If

                If bReceiptTypeIsInstalmentBased Then
                    'first find the cashlistitem ID of the new listitem
                    'Only process the current line

                    'Post the Instalment Receipts


                    If IsArray(m_vListData(ACInstalmentArray, nCount)) Then

                        If InstalmentsAlreadyPosted(nCount) Then
                            m_vListData(ACSubTransdetailId, nCount) = vCashListItem(eCashListItem.TransdetailID)
                            m_vListData(ACSubCashListItemID, nCount) = vCashListItem(eCashListItem.CashlistitemID)

                            If PostInstalments(nCount) <> PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                   sMsg:="Failed to Post Instalments",
                                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Post")
                            End If

                            m_vListData(ACSubRealAllocationID, nCount) = gACTLibrary.ACTAllocationStatusAllocated
                            If DataToBusiness(lMode:=PMEComponentAction.PMEdit, lRow:=nCount) <> PMEReturnCode.PMTrue Then
                                RaiseError("Post", "Failed to update the status to allocated.", klUserError)
                            End If

                            m_oBusiness.CurrentRecord = m_vListData(m_vListData.GetUpperBound(0), nCount)
                            If m_oBusiness.Update() <> PMEReturnCode.PMTrue Then
                                RaiseError("Post", "Failed to update the database.", klUserError)
                            End If

                            If m_lFirstTransdetailID > 0 Then
                                If UpdateCashListItem(m_vListData(ACSubCashListItemID, nCount)) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update CashlistItem", vApp:=ACApp, vClass:=ACClass, vMethod:="Post")
                                End If
                            End If
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Post Instalments. Selected installments already posted ", vApp:=ACApp, vClass:=ACClass, vMethod:="Post")
                        End If
                    End If
                End If
                vInstalmentArrayTP = m_vListData(ACInstalmentArray, nCount)
            Next nCount

            CreateCashListPost()
            ReDim oKeyArray(1, 5)
            oKeyArray(0, 0) = PMNavKeyConst.ACTKeyNameCashListId
            oKeyArray(1, 0) = m_lCashlistID

            'tax band id
            oKeyArray(0, 1) = PMNavKeyConst.kACTKeyNameTaxBandId
            oKeyArray(1, 1) = m_nTaxBandId
            'tax amount
            oKeyArray(0, 2) = PMNavKeyConst.PMKeyNameTaxAmount
            oKeyArray(1, 2) = m_crTaxAmount
            'insurance file cnt
            oKeyArray(0, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt
            oKeyArray(1, 3) = m_lInsuranceFileCnt

            'batch Id
            oKeyArray(0, 4) = PMNavKeyConst.PMKeyNameBatchID
            oKeyArray(1, 4) = m_lBatchID

            'InsurerPaymentRoadmap
            oKeyArray(0, 5) = PMNavKeyConst.kPMKeyNameInsurerPaymentRoadMap
            If IsInsurerePaymentRoadMap = True Then
                oKeyArray(1, 5) = "1"
            Else
                oKeyArray(1, 5) = "0"
            End If

            'This has been added so that we can post specific cashlistitems
            'SW Front office receipting
            If v_lCashlistItemID <> 0 Then
                ReDim Preserve oKeyArray(1, 5)
                oKeyArray(0, 1) = PMNavKeyConst.ACTKeyNameCashListItemId
                oKeyArray(1, 1) = v_lCashlistItemID

                'batch Id
                oKeyArray(0, 2) = PMNavKeyConst.PMKeyNameBatchID
                oKeyArray(1, 2) = m_lBatchID

                'InsurerPaymentRoadmap
                oKeyArray(0, 3) = PMNavKeyConst.kPMKeyNameInsurerPaymentRoadMap
                If IsInsurerePaymentRoadMap = True Then
                    oKeyArray(1, 3) = "1"
                Else
                    oKeyArray(1, 3) = "0"
                End If

                oKeyArray(0, 4) = PMNavKeyConst.kACTKeyNameTaxBandId
                oKeyArray(1, 4) = m_nTaxBandId

                oKeyArray(0, 5) = PMNavKeyConst.PMKeyNameTaxAmount
                oKeyArray(1, 5) = m_crTaxAmount


            End If

            m_lReturn = m_oCashListPost.SetKeys(vKeyArray:=oKeyArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to set keys" & Strings.Chr(13) & Strings.Chr(10) &
                                "bACTCashListPost.Automated" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")",
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_oCashListPost.ChequeProduction = g_bChequeProduction

            'For TP Financer
            If bThirdPartyOnly Then
                Dim temp_oInstalments As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oInstalments, "bSIRPFInstalments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oInstalments = temp_oInstalments

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nresult = gPMConstants.PMEReturnCode.PMFalse
                    oInstalments = Nothing
                    Return
                End If

                ' To support partial payments.
                ReDim vInstalmentIDArrayTP(4, 0)
                'loop through the instalments that are paid by the current receipt

                For lCount As Integer = 0 To vInstalmentArrayTP.GetUpperBound(1)

                    If gPMFunctions.ToSafeInteger(vInstalmentArrayTP(ACInstalmentFlagElement, lCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                        'increase size of array by one (unless first time through)
                        ReDim Preserve vInstalmentIDArrayTP(4, lIDTPArrayCount)

                        vInstalmentIDArrayTP(0, lIDTPArrayCount) = gPMFunctions.ToSafeInteger(vInstalmentArrayTP(ACInstalmentPlanRef, lCount))

                        vInstalmentIDArrayTP(1, lIDTPArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArrayTP(ACPartialPaymentAmount, lCount))

                        vInstalmentIDArrayTP(2, lIDTPArrayCount) = gPMFunctions.ToSafeInteger(vInstalmentArrayTP(ACReceiptDifferenceOption, lCount))

                        vInstalmentIDArrayTP(3, lIDTPArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArrayTP(ACInstalmentReceiptAmount, lCount))

                        vInstalmentIDArrayTP(4, lIDTPArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArrayTP(ACInstalmentBaseAmmount, lCount))

                        'increment count by one
                        lIDTPArrayCount += 1
                    End If
                Next
            End If

            If v_lCashlistItemID = 0 Then
                'Post the rest of the list
                If bThirdPartyOnly Then
                    sFailureReason = "error"
                    'While (sFailureReason.Trim() <> "")
                    If Information.IsArray(vInstalmentIDArrayTP) Then
                        lUBound = vInstalmentIDArrayTP.GetUpperBound(1)
                    End If
                    For lInstCount As Integer = 0 To lUBound
                        sFailureReason = ""
                        If vInstalmentIDArrayTP(0, lInstCount) <> 0 Then
                            m_sPlanref = vInstalmentIDArrayTP(0, lInstCount)
                        End If
                        dInsAmount = ToSafeDecimal(vInstalmentIDArrayTP(3, lInstCount))
                        dPlanAmount = dInsAmount
                        'For Partial receipting 
                        If lUBound = 0 Then
                            dPartialInsAmount = ToSafeDecimal(vInstalmentIDArrayTP(1, lInstCount))
                            If dPartialInsAmount <> 0 AndAlso dInsAmount <> dPartialInsAmount Then
                                dInsAmount = dPartialInsAmount
                            End If
                        End If

                        m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                            sFailureReason:=sFailureReason, sInsurence_Ref:=m_sMediaRef, dtTransactionDate:=m_dtEffectiveDate, bThirdPartyOnly:=bThirdPartyOnly, sPlanRef:=m_sPlanref, dInsAmount:=dInsAmount, dOutstandingAmount:=dOutstandingAmount, nPremiumFinanceVersion:=nPremiumFinanceVersion)
                        If (m_lReturn <> PMEReturnCode.PMTrue) And sFailureReason.Trim() <> "" Then
                            MessageBox.Show(sFailureReason & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "Click OK to Try Again",
                                            Application.ProductName,
                                            MessageBoxButtons.OK)
                        End If
                        If dOutstandingAmount = 0 Then
                            m_lReturn = oInstalments.PlanStatusUpdate(vPremiumFinanceCnt:=Convert.ToInt32(m_sPlanref), vPremiumFinanceVersion:=nPremiumFinanceVersion, vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted)
                        End If
                        m_vListData(ACSubRealAllocationID, 0) = gACTLibrary.ACTAllocationStatusAllocated
                        If dOutstandingAmount <> 0 Then
                            CreateWorkManagerTask(dPlanAmount, dOutstandingAmount, m_sPlanref)
                        End If
                    Next
                    'End While

                Else
                    sFailureReason = "error"
                    While (sFailureReason.Trim() <> "")
                        sFailureReason = ""
                        m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                        sFailureReason:=sFailureReason, sInsurence_Ref:=m_sMediaRef, v_dTransactionDate:=m_dtEffectiveDate)
                        If (m_lReturn <> PMEReturnCode.PMTrue) And sFailureReason.Trim() <> "" Then
                            MessageBox.Show(sFailureReason & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "Click OK to Try Again",
                                            Application.ProductName,
                                            MessageBoxButtons.OK)
                        End If
                    End While
                    For lCount1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                        'Start - Sankar - PN 55288
                        m_lReturn = m_oBusiness.GetCashListReceiptTypeFromID(gPMFunctions.ToSafeLong(m_vListData(ACvCashlistitemreceipttypeid, lCount1)),
                                                                             r_sCashListReceiptType:=sReceiptType)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName,
                                                    "GetCashListReceiptTypeFromID Failed",
                                                    PMELogLevel.PMLogError)
                        End If

                        If kCashListItemReceiptTypeCode = sReceiptType Then
                            ' Post Cash List Item and allocate against all the selected policies
                            SinglePostAndMultiAllocate()
                            Exit For
                        End If
                    Next
                End If

            ElseIf m_lInsuranceFileCnt <> 0 And m_sDocumentRef <> "" Then
                ' Get the CashListItemID that was selected
                m_lSelectedRow = gPMFunctions.ToSafeInteger(lvwListDetails.FocusedItem.Tag)
                nAccountID = gPMFunctions.ToSafeInteger(m_vListData(ACSubAccountID, m_lSelectedRow))

                'DC090205 : PN18569 : Check amount after converting to base currency
                crRate = gPMFunctions.ToSafeCurrency(m_vListData(ACCurrencyBaseXrate, m_lSelectedRow))
                crBaseAmount = gPMFunctions.ToSafeCurrency(m_vListData(ACSubAmount, m_lSelectedRow))
                crCurrAmount = Math.Round(crBaseAmount * crRate, 2)

                'AR20050203 - PN18491 - Pass in Write Off mode and Status
                m_lReturn = CheckWriteOff(lAccountID:=nAccountID,
                                          lCurrencyID:=m_iCurrencyID,
                                          cAmount:=crCurrAmount,
                                          lMode:=cAUTO_ALLOCATE_AUTOMATIC,
                                          r_lStatus:=oAutoAllocationState,
                                          r_lWriteOffReasonID:=nWriteOffReasonID,
                                          r_cWriteOffAmount:=crWriteOffAmount,
                                          r_bPartPayment:=bPartPayment)

                'AR20050203 - PN18491 - Handle error
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                       sMsg:="Failed to check Write Off limits.",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="Post",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                bDoAllocation = True
                'AR20050203 - PN18491 - If not auto-allocating, reset variables
                'AR20050208 - PN18582 - Alternative message if user has chosen not to write off
                If oAutoAllocationState <> PMEReturnCode.PMTrue AndAlso
                    crRate = 1 AndAlso (m_iCurrencyID = m_iDocumentsCurrencyID Or m_iDocumentsCurrencyID = 0) Then
                    nWriteOffReasonID = 0
                    crWriteOffAmount = 0
                    If bPartPayment Then
                        'AR20050211 - PN18727 Give user option not to part pay.
                        If oAutoAllocationState = PMEReturnCode.PMCancel Then
                            nResponse = MessageBox.Show("You have chosen not to write off the difference." & Strings.Chr(13) & Strings.Chr(10) &
                                                        "Click OK to make a partial payment. Click Cancel to leave the item unallocated.",
                                                        "Cash List Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                        ElseIf oAutoAllocationState = PMEReturnCode.PMFalse Then
                            nResponse = MessageBox.Show("Amount is outside the write-off limits." & Strings.Chr(13) & Strings.Chr(10) &
                                                        "Click OK to make a partial payment. Click Cancel to leave the item unallocated.",
                                                        "Cash List Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                        End If
                        bDoAllocation = nResponse = System.Windows.Forms.DialogResult.OK
                    End If
                End If

                'AR20050211 - PN18727
                If bDoAllocation Then
                    'Post and allocate the individual receipt
                    'Passed bSpecificCashListItemId parameter as TRUE to allocate the cashlistitem.
                    m_lReturn = m_oCashListPost.PostAllocatedCashListItem(lCashlistID:=m_lCashlistID,
                                                                          lCashListItemID:=v_lCashlistItemID,
                                                                          lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                          sDocumentRef:=m_sDocumentRef,
                                                                          lWriteOffReasonID:=nWriteOffReasonID,
                                                                          cWriteOffAmount:=crWriteOffAmount,
                                                                          bCurrencyWriteOff:=bCurrencyWriteOff,
                                                                          r_iAllocationStatus:=r_iAllocationStatus,
                                                                          bSpecificCashListItemId:=True)
                Else
                    'Post the individual receipt
                    sFailureReason = "error"
                    While (sFailureReason.Trim() <> "")
                        sFailureReason = ""

                        'PostUnallocatedCash : Changes in this as well.
                        If m_lCashListItemID <> 0 Then
                            m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                            sFailureReason:=sFailureReason,
                                                                            r_nTaxTransdetailID:=m_nTaxTransdetailId)
                        Else
                            m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                            sFailureReason:=sFailureReason)
                        End If
                        If (m_lReturn <> PMEReturnCode.PMTrue) And sFailureReason.Trim() <> "" Then
                            MessageBox.Show(sFailureReason & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "Click OK to Try Again",
                                            Application.ProductName,
                                            MessageBoxButtons.OK)
                        End If


                    End While
                End If


                If m_lReturn <> PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                       sMsg:="Failed to Post Cash List items.",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="Post",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            Else
                'Post the individual receipt
                sFailureReason = "error"
                While (sFailureReason.Trim() <> "")
                    sFailureReason = ""
                    If m_lCashListItemID <> 0 Then

                        m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                        v_vCashListItemID:=m_lCashListItemID,
                                                                        sFailureReason:=sFailureReason,
                                                                        r_nTaxTransdetailID:=m_nTaxTransdetailId)
                    Else
                        m_lReturn = m_oCashListPost.PostUnallocatedCash(v_vCashListID:=m_lCashlistID,
                                                                        sFailureReason:=sFailureReason)
                    End If
                    If (m_lReturn <> PMEReturnCode.PMTrue) AndAlso sFailureReason.Trim() <> "" Then
                        MessageBox.Show(sFailureReason & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                        "Click OK to Try Again",
                                        Application.ProductName,
                                        MessageBoxButtons.OK)
                    End If
                End While

                If ((m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse (m_lCashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) AndAlso
                    m_sActionkey = gACTLibrary.ACTApprove AndAlso m_lCashListItemID > 0 AndAlso m_lClaimPaymentId = 0 AndAlso m_vClaimPaymentIDs Is Nothing Then

                    m_lReturn = m_oBusiness.GetClaimPaymentDetailsByCashListItem(m_lCashListItemID, oResultArray)
                    If m_lReturn = PMEReturnCode.PMTrue AndAlso oResultArray IsNot Nothing Then

                        ReDim m_vDocumentIds(oResultArray.GetUpperBound(1))
                        ReDim oClaimPaymentIDs(oResultArray.GetUpperBound(1))

                        For nCount = 0 To oResultArray.GetUpperBound(1)
                            m_vDocumentIds(nCount) = oResultArray(0, nCount)
                            oClaimPaymentIDs(nCount) = oResultArray(1, nCount)
                        Next nCount

                        If oClaimPaymentIDs IsNot Nothing Then
                            cmdAllocate_Click(cmdAllocate, New EventArgs())
                        End If
                    End If
                End If

            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                ' Display message.
                MessageBox.Show("Failed to post unallocated cash" & Strings.Chr(13) & Strings.Chr(10) &
                                "bACTCashListPost.Automated" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")",
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lCashTransDetailID = m_oCashListPost.CashTransId
            m_lReturn = m_oBusiness.GetDetails(vCashListID:=m_lCashlistID)

            If m_lReturn = PMEReturnCode.PMTrue Then
                m_lReturn = LetterProcessing(m_lCashListItemID)
            End If

            If (m_lCashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                (m_lCashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then

                If Information.IsArray(m_vClaimPaymentIDs) Then
                    For iVar As Integer = 0 To m_vClaimPaymentIDs.GetUpperBound(0)
                        m_lReturn = m_oBusiness.AddCashListItemClaimLink(v_lClaim_payment_Id:=m_vClaimPaymentIDs(iVar),
                                                                         v_lClaim_receipt_id:=0,
                                                                         v_lCashListItem_id:=m_lCashListItemID)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("Post", "AddCashListItemClaimLink Failed")
                        End If
                    Next
                Else
                    If m_lClaimPaymentId > 0 Then

                        m_lReturn = m_oBusiness.AddCashListItemClaimLink(v_lClaim_payment_Id:=m_lClaimPaymentId,
                                                                         v_lClaim_receipt_id:=0,
                                                                         v_lCashListItem_id:=m_lCashListItemID)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("Post", "AddCashListItemClaimLink Failed")
                        End If
                    End If
                End If
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                   sMsg:="Failed to store cash for letter printing.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Post",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If

            m_bPosted = True
            If lvwListDetails.Items.Count > 0 Then
                For lRow As Integer = 1 To lvwListDetails.Items.Count
                    'Only update if previously unallocated.'Update status as Posted, in both listview and array

                    m_lReturn = m_oBusiness.GetCashListReceiptTypeFromID(gPMFunctions.ToSafeLong(m_vListData(ACvCashlistitemreceipttypeid, lRow - 1)),
                                                                         r_sCashListReceiptType:=sReceiptType)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName,
                                                "GetCashListReceiptTypeFromID Failed",
                                                PMELogLevel.PMLogError)
                    End If

                    If kCashListItemReceiptTypeCode = sReceiptType Then
                        m_vListData(ACSubRealAllocationID, lRow - 1) = gACTLibrary.ACTAllocationStatusAllocated
                    ElseIf CDbl(m_vListData(ACSubRealAllocationID, lRow - 1)) = gACTLibrary.ACTAllocationStatusUnallocated Then
                        m_vListData(ACSubRealAllocationID, lRow - 1) = gACTLibrary.ACTAllocationStatusPosted
                    End If
                Next lRow
                'Instead of Refreshing everytime for Each Entry - Refresh only once
                'Refresh the list correctly afterwards.
                m_lReturn = DetailsFormList(0)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                       sMsg:="Failed to display details on the list.",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAllocate_Click",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                End If

            End If

            m_bCashListCreated = True
        Catch excep As System.Exception

            Select Case Information.Err().Number
                Case Else
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Post", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End Select

        End Try

    End Sub


    ''' <summary>
    ''' PostInstalments - to post instalments applicable to the most recent receipt
    ''' </summary>
    ''' <param name="lIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostInstalments(ByVal lIndex As Integer) As Integer
        Dim result As Integer = 0

        Dim oInstalments As bSIRPFInstalments.Business
        Dim lNewRowID As Integer
        Dim vInstalmentArray(,) As Object
        Dim vInstalmentIDArray(,) As Object
        Dim lIDArrayCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'lNewRowID = m_vListData.GetUpperBound(1)

            vInstalmentArray = m_vListData(ACInstalmentArray, lIndex)

            Dim temp_oInstalments As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInstalments, "bSIRPFInstalments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oInstalments = temp_oInstalments

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oInstalments = Nothing
                Return result
            End If

            lIDArrayCount = 0

            ' To support partial payments.
            ReDim vInstalmentIDArray(5, 0)
            'loop through the instalments that are paid by the current receipt

            For lCount As Integer = 0 To vInstalmentArray.GetUpperBound(1)

                If gPMFunctions.ToSafeInteger(vInstalmentArray(ACInstalmentFlagElement, lCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    'increase size of array by one (unless first time through)
                    ReDim Preserve vInstalmentIDArray(5, lIDArrayCount)

                    vInstalmentIDArray(0, lIDArrayCount) = gPMFunctions.ToSafeInteger(vInstalmentArray(ACInstalmentsID, lCount))

                    vInstalmentIDArray(1, lIDArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArray(ACPartialPaymentAmount, lCount))

                    vInstalmentIDArray(2, lIDArrayCount) = gPMFunctions.ToSafeInteger(vInstalmentArray(ACReceiptDifferenceOption, lCount))

                    vInstalmentIDArray(3, lIDArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArray(ACInstalmentReceiptAmount, lCount))

                    vInstalmentIDArray(4, lIDArrayCount) = gPMFunctions.ToSafeCurrency(vInstalmentArray(ACInstalmentBaseAmmount, lCount))

                    If CDec(vInstalmentIDArray(1, lIDArrayCount)) = CDec(vInstalmentIDArray(3, lIDArrayCount)) Then
                        vInstalmentIDArray(5, lIDArrayCount) = 0
                    Else
                        vInstalmentIDArray(5, lIDArrayCount) = gPMFunctions.ToSafeInteger(vInstalmentArray(kWriteOffReasonId, lCount))
                    End If

                    'increment count by one
                    lIDArrayCount += 1
                    End If
            Next

            'the instalment is part of the receipt

            m_lReturn = oInstalments.PostMultipleInstalments(v_vInstalmentID:=vInstalmentIDArray, v_lCashDrawerID:=gPMFunctions.ToSafeInteger(m_vCashDrawerID), v_dtTransactionDate:=gPMFunctions.ToSafeDate(m_vListData(ACTransactiondate, lNewRowID)), v_vCashListItemID:=m_vListData(ACSubCashListItemID, lIndex), v_sSpare:=m_vListData(ACSubMediaRef, lIndex), r_vTransDetailID:=m_vListData(ACSubTransdetailId, lIndex), v_iCashListCurrencyID:=CurrencyID, v_lFirstTransDetailID:=m_lFirstTransdetailID)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch ex As Exception


            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            End Select
        End Try
        Return result
    End Function

    Public Function SinglePostAndMultiAllocate() As Integer
        Dim result As Integer = 0
        Dim bSIRBankGuarantee As Object
        Const kMethodName As String = "SinglePostAndMultiAllocate"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lAccountID As Integer
            Dim lCashListItemID As Integer
            Dim lWriteOffReasonID As Integer
            Dim cWriteOffAmount As Decimal
            Dim r_iAllocationStatus As Integer
            Dim r_vCLBankGuarantee(,) As Object
            Dim lCount As Integer

            Dim oBankGuarantee As bSIRBankGuarantee.Business
            Dim bSpecificCashListItemId As Boolean

            lAccountID = CInt(m_vListData(ACSubAccountID, m_lSelectedRow))

            Dim temp_oBankGuarantee As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankGuarantee, "bSIRBankGuarantee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBankGuarantee = temp_oBankGuarantee
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'm_lReturn = oBankGuarantee.Initialise(g_sUserName$, g_sPassword$, g_iUserID%, g_iSourceID%, g_iLanguageID%, _
            ''                                g_iCurrencyID%, g_iLogLevel%, m_sCallingAppName$, g_oDatabase)
            'If m_lReturn <> PMTrue Then
            '    RaiseError kMethodName, "oBankGuarantee.Initialise Failed", PMLogError
            'End If

            m_lReturn = oBankGuarantee.GetCashListItemForBG(lCashListId:=m_lCashlistID, vCashListItemsForBg:=r_vCLBankGuarantee)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oBankGuarantee.GetCashListItemForBG Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            For i As Integer = r_vCLBankGuarantee.GetLowerBound(1) To r_vCLBankGuarantee.GetUpperBound(1)

                m_lReturn = m_oCashListPost.PostAllocatedCashListItem(lCashlistID:=r_vCLBankGuarantee(1, i), lCashListItemID:=r_vCLBankGuarantee(2, i), lInsuranceFileCnt:=r_vCLBankGuarantee(3, i), sDocumentRef:=m_sDocumentRef, lWriteOffReasonID:=lWriteOffReasonID, cWriteOffAmount:=cWriteOffAmount, bCurrencyWriteOff:=False, r_iAllocationStatus:=r_iAllocationStatus, bIsPosted:=True, cAmtToBePosted:=r_vCLBankGuarantee(4, i), bSpecificCashListItemId:=True, lPostAccountId:=lAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oCashListPost.PostAllocatedCashListItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Start - Sankar - Bank Guarantee Bug Fixing

                m_lReturn = m_oCashListPost.UpdateBGAvailableBalance(r_vCLBankGuarantee(0, i), r_vCLBankGuarantee(4, i))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateBGAvailableBalance Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                'End - Sankar - Bank Guarantee Bug Fixing
            Next

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Cash List Post
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdPost_Click(ByVal eventSender As Object,
                              ByVal eventArgs As EventArgs) Handles cmdPost.Click

        Dim oOptionValue As Object = Nothing
        Dim bLeadFlag As Boolean = False
        Dim bIsIncludePaymentTypeClaimPayment As Boolean = False
        Dim nApprovalSteps As Integer = 0
        Dim aoResultArray(,) As Object = Nothing
        Dim nPos As Integer = 0
        If chkSplitReceipt.Checked = True Then
            m_lReturn = RefreshListData()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                m_lErrorNumber = PMEReturnCode.PMFalse
                Exit Sub
            End If
            For nRow As Integer = 0 To lvwListDetails.Items.Count - 1
                If lvwListDetails.Items(nRow).SubItems(6).Text = "Lead" Then
                    bLeadFlag = True
                    Exit For
                End If
            Next
            If bLeadFlag <> True Then
                MsgBox("You must specify a Lead Account in order to split the receipt.", MsgBoxStyle.OkOnly, "Cash List Items")
                Exit Sub
            End If
            If m_cTotalAmount <> CDec(pnlTotal.Text) Then
                MsgBox("The total amount of the receipt has not been fully split, please amend in order to proceed.", MsgBoxStyle.OkOnly, "Cash List Items")
                Exit Sub
            End If
            m_oBusiness.UpdateCashListForSplitReceipt(v_iCashListId:=m_lCashlistID, v_bStatus:=True)
        End If

        m_lReturn = g_oObjectManager.GetInstance(
                  oObject:=m_oStepAuthorization,
                  sClassName:="bACTCashlistitem.StepAuthorization",
                  vInstanceManager:=PMGetViaClientManager)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("cboPaymentType_Click", "Failed to create instance of Cashlistitem")
        End If
        nApprovalSteps = 1
        If CashlistTypeID = ACTCashListTypeClaimPayments Then
            m_oStepAuthorization.PaymentType = 2
        End If

        m_lReturn = m_oStepAuthorization.GetStepDetails(v_lApprovalStep:=nApprovalSteps, r_vStepDetails:=aoResultArray)

        If IsArray(aoResultArray) Then
            For nPos = 0 To UBound(aoResultArray, 2)
                If aoResultArray(2, nPos) = "1" Then
                    bIsIncludePaymentTypeClaimPayment = True
                    Exit For
                Else
                    bIsIncludePaymentTypeClaimPayment = False
                End If
            Next
        End If
        ' call the post routines
        Post()

        If chkSplitReceipt.Checked = True Then
            chkSplitReceipt.Enabled = False
            If m_sCallingAppName = "SplitReceiptsFromFindTransaction" AndAlso CInt(DocumentID) <> CInt(0) Then
                m_lReturn = ReverseCashList(DocumentID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    m_lErrorNumber = PMEReturnCode.PMFail
                    Exit Sub
                End If
            End If
        End If

        m_lReturn = EnableButtons()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            m_lErrorNumber = PMEReturnCode.PMFalse
            Exit Sub
        End If

        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                  v_vBranch:=g_oObjectManager.SourceID,
                                                  r_vUnderwriting:=oOptionValue)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="Failed to process GetProductOptionValue.",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description)
            Exit Sub
        End If

        If oOptionValue = "1" AndAlso m_sCashListRoadmap = ACTInsurerPaymentRoadMap Then
            m_lReturn = GetOption(v_iOptionNumber:=kSysOptIncludeInsurerPaymentMultiStep,
                  r_sOptionValue:=m_sIncludeInsurerPaymentMultiStep)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                        sMsg:="Failed to process GetOption.",
                        vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                        vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Sub
            End If
        End If

        If gPMFunctions.NullToString(oOptionValue) = "1" AndAlso
            (m_bViaInsurerPayment = False OrElse m_sIncludeInsurerPaymentMultiStep = "1") Then
            If m_cTotalAmount <> 0 Then
                If (m_lCashListTypeID = SharedFiles.ACTConst.ACTCashListTypePayments) OrElse
            (m_lCashListTypeID = ACTCashListTypeClaimPayments _
            AndAlso bIsIncludePaymentTypeClaimPayment = True) Then
                    MessageBox.Show("This payment requires further approval. It cannot be allocated until fully approved.",
                                    "Allocation not allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cmdAllocate.Enabled = False
                    m_bApprovalMsg = True
                    m_lReturn = m_oBusiness.UpdateTransMatchCashListItemID(m_lCashListItemID, m_lBatchID, "")
                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                sMsg:="Failed to UpdateTransMatchCashListItemID.",
                                vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                                vErrNo:=Err.Number, vErrDesc:=Err.Description)
                        Exit Sub
                    End If
                    m_lReturn = m_oBusiness.UpdateCashListBatchID(m_lBatchID, m_lCashlistID)
                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                sMsg:="Failed to UpdateCashListBatchID.",
                                vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                                vErrNo:=Err.Number, vErrDesc:=Err.Description)
                        Exit Sub
                    End If
                End If
            End If
        End If

        lvwListDetails.Refresh()
        m_oStepAuthorization.Dispose()
        m_oStepAuthorization = Nothing
    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: cmdReverse_Click
    ' PURPOSE: Reverse the selected receipt
    ' AUTHOR: Steve Watton
    ' DATE: 25 October 2002, 15:19:52
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Sub cmdReverse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReverse.Click
        Dim lRow As Integer
        Dim sLookUpcode As String = ""

        ' Click event of the reverse Button.

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' AMB 06/03/2003: IS2495 - check that something's actually been selected
            If lvwListDetails.FocusedItem Is Nothing Then
                MessageBox.Show("Please select an item to reverse", "Reverse", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            lRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)

            If lvwListDetails.FocusedItem.SubItems(4).Text = "Reversed" Then
                MessageBox.Show("This Receipt has already been reversed", "Warning: Unable to reverse", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMReverse)

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'update the database with the reverse details

            m_lReturn = m_oBusiness.Update()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to update the details
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the database with the details in the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click")
            End If

            'perform replace options
            If MessageBox.Show("Do you wish to replace this receipt?", "Warning: Replace Receipt?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMReplace)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    ' Log Error.
                    If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the replacement item", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click")
                    Exit Sub
                End If
                ProcessCashDrawerFunctionalityForAdd()
            End If

            'Refresh to prevent the list from looking funny when their are scrollbars.
            lvwListDetails.Refresh()

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Reverse button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdReverse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: cmdReverse_Click
    ' PURPOSE: Reverse the selected receipt
    ' AUTHOR: Steve Watton
    ' DATE: 25 October 2002, 15:19:52
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim lRow As Integer

        ' Click event of the reverse Button.

        Try

            ' AMB 06/03/2003: IS2495 - check that something's actually been selected
            'If lvwListDetails.FocusedItem Is Nothing Then
            '    MessageBox.Show("Please select an item to view", "View", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '    Exit Sub
            'End If

            If lvwListDetails.Items.Count = 0 Then
                Exit Sub
            End If

            'lRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)

            ' Edit Details form
            m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMView)

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Refresh to prevent the list from looking funny when their are scrollbars.
            lvwListDetails.Refresh()

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the View button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub frmList_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            ' Terminate business

            m_oBusiness.Dispose()
            m_oBusiness = Nothing

            If Not (m_oInsurerPaymentAllocateBusiness Is Nothing) Then

                m_oInsurerPaymentAllocateBusiness.Dispose()
                m_oInsurerPaymentAllocateBusiness = Nothing
            End If

            g_oSirConfig.Dispose()
            g_oSirConfig = Nothing

            If Not (m_oCashListPost Is Nothing) Then

                m_oCashListPost.Dispose()
                m_oCashListPost = Nothing
            End If
            oCache = Nothing
            'Reset the stored Bank Reference
            If gPMFunctions.SetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMERegSettingLevel.pmeRSLClient, "LastBankReference", "") <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reset the Bank Reference in the local Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckAllocated
    '
    ' Description: Checks that all the cash list items have been
    '              allocated.
    '
    ' ***************************************************************** '
    Private Function CheckAllocated(ByRef v_bAllocated As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_bAllocated = True

            ' Check each list item
            For iLoop1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                'PN15806
                '        If (m_vListData(ACSubRealAllocationID, iLoop1%) = ACTAllocationStatusUnallocated) Then
                If CDbl(m_vListData(ACSubRealAllocationID, iLoop1)) <> gACTLibrary.ACTAllocationStatusAllocated Then
                    v_bAllocated = False
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAllocated Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllocated", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck230501
    ' ***************************************************************** '
    ' Name: IsAccountPaid
    '
    ' Description: Checks that all the cash list items have been
    '              allocated.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (IsAccountPaid) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsAccountPaid(ByVal vAccount_id As Object, ByRef vAccount_Balance As Object) As Integer
    'Dim result As Integer = 0
    'Dim bACTAccount As Object
    'Dim sTitle, sMessage As String

    'Dim oAccount As bACTAccount.Form
    'Dim vBalance As Object
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    'Dim temp_oAccount As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
    'oAccount = temp_oAccount
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to get an instance of the business object.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Display error stating the problem.
    ' Get description from the resource file.

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' Display message.
    'MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTAccount", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
    'Return result
    'End If
    '

    'm_lReturn = oAccount.GetAccountBalance(r_vdAccountBalance:=vBalance, v_vAccountId:=vAccount_id)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New Exception()
    'End If

    'If CDec(vBalance) = CDec(vAccount_Balance) Then
    'result = gPMConstants.PMEReturnCode.PMTrue
    'End If

    'm_lReturn = oAccount.Terminate()
    'oAccount = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Check Account Balance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAccountPaid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name:
    '
    ' Description: Store Cash for Letter Run
    '
    ' DD 10/08/2003: Changed for Cash Drawers, this will produce results
    ' on each receipt closing.
    ' ***************************************************************** '
    Private Function LetterProcessing(ByVal lCashListItemID As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lAccountID, lTransDetailID, lPartyCnt, lInsuranceFileCnt As Integer
        Dim sShortName, sDocumentRef As String
        Dim bLetter As Boolean
        Dim lCashListItemReceiptType As Integer
        Dim sCashLIstItemReceiptTypeCode As String = ""
        Dim lCashListItemPaymentType As Integer
        Dim sCashListItemPaymentTypeCode As String = ""
        Dim vCashListItem As Object
        Dim bMessageShown, bReceiptTypeIsInstalmentBased As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetNext(vCashListItem)

            'Transfer data from retrieved array into local variables

            lAccountID = CInt(vCashListItem(gACTLibrary.eCashListItem.AccountID))

            lTransDetailID = CInt(vCashListItem(gACTLibrary.eCashListItem.TransdetailID))

            bLetter = CBool(vCashListItem(gACTLibrary.eCashListItem.Letter))

            lCashListItemReceiptType = CInt(vCashListItem(gACTLibrary.eCashListItem.CashListItem_receipt_type_id))

            lCashListItemPaymentType = CInt(vCashListItem(gACTLibrary.eCashListItem.CashListItem_Payment_Type_id))

            lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))

            m_lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))
            bMessageShown = False

            Do While m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                If bLetter Then
                    'DC101003 -PN7293 -payment letters
                    If CashlistTypeID = ACReceiptType Then

                        'Get the up to date transdetail as it hasn't been updated in the business object yet.

                        m_lReturn = m_oBusiness.GetReceiptTypeCodeAndTransDetailID(lCashListItemID, lTransDetailID, sCashLIstItemReceiptTypeCode, bReceiptTypeIsInstalmentBased)

                        If bReceiptTypeIsInstalmentBased Then
                            'Get letter detail for instalment receipts (got no Transaction ID)

                            m_lReturn = m_oBusiness.GetLetterDetailsForInstalment(lCashlistitemID:=lCashListItemID, lPartyCnt:=lPartyCnt, sShortName:=sShortName, sDocumentRef:=sDocumentRef, lInsuranceFileCnt:=lInsuranceFileCnt)
                        Else
                            'Get Letter details for other receipts

                            m_lReturn = m_oBusiness.GetLetterDetails(lAccountId:=lAccountID, lTransdetailId:=lTransDetailID, lPartyCnt:=lPartyCnt, sShortName:=sShortName, sDocumentRef:=sDocumentRef)
                        End If

                        'DC101003 -PN7393 -payment letters
                    Else

                        m_lReturn = m_oBusiness.GetPaymentTypeCodeAndTransDetailID(lCashListItemID, lTransDetailID, sCashListItemPaymentTypeCode)

                        'Get Letter details for payments

                        m_lReturn = m_oBusiness.GetLetterDetails(lAccountId:=lAccountID, lTransdetailId:=lTransDetailID, lPartyCnt:=lPartyCnt, sShortName:=sShortName, sDocumentRef:=sDocumentRef)
                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        Throw New Exception()
                    End If

                    'Skip if this Account does not have a Party
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                        If m_bPosted Then ' Gaurav DOUBT
                            If Information.IsArray(m_vLetters) Then
                                ReDim Preserve m_vLetters(3, m_vLetters.GetUpperBound(1) + 1)
                            Else
                                ReDim m_vLetters(3, 0)
                            End If

                            m_vLetters(0, m_vLetters.GetUpperBound(1)) = lPartyCnt
                            m_vLetters(1, m_vLetters.GetUpperBound(1)) = sShortName
                            m_vLetters(2, m_vLetters.GetUpperBound(1)) = sDocumentRef
                            m_vLetters(3, m_vLetters.GetUpperBound(1)) = lInsuranceFileCnt
                        End If
                        'Cash Drawer letter print now
                        If m_vCashDrawerID <> 0 Then
                            ProcessLetters()

                            'PSL 28/08/2003 Issue6451
                            'Moved end if down, so we don't set letters printed if we are not printing,
                            'this will make it work on 186
                            'Set CashListItem.Letter to Zero to mstop recurrent printing

                            m_lReturn = m_oBusiness.SetLetterPrinted(lCashListItemID)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse

                                ' Log Error Message
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set letter to printed", vApp:=ACApp, vClass:=ACClass, vMethod:="LetterProcessing", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                Return result
                            End If
                        End If
                    ElseIf bMessageShown Then
                        MessageBox.Show("Document printing is not available for System Accounts.", "Document Printing not available", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        bMessageShown = True
                    End If
                End If

                m_lReturn = m_oBusiness.GetNext(vCashListItem)

                'Transfer data from retrieved array into local variables

                lAccountID = CInt(vCashListItem(gACTLibrary.eCashListItem.AccountID))

                lTransDetailID = CInt(vCashListItem(gACTLibrary.eCashListItem.TransdetailID))

                bLetter = CBool(vCashListItem(gACTLibrary.eCashListItem.Letter))

                lCashListItemReceiptType = CInt(vCashListItem(gACTLibrary.eCashListItem.CashListItem_receipt_type_id))

                lCashListItemPaymentType = CInt(vCashListItem(gACTLibrary.eCashListItem.CashListItem_Payment_Type_id))

                lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))

            Loop

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to store cash for letter processing", vApp:=ACApp, vClass:=ACClass, vMethod:="LetterPRocessing", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessLetters(Standard Method)
    '
    ' Description: Calls the appropriate methods to print letters
    '
    ' DD 10/08/2003: Moved here from the Interface Class
    ' DD 23/03/2005: Rewritten to call iPMBDocTemplate
    ' ***************************************************************** '
    Public Function ProcessLetters() As Integer

        Dim result As Integer = 0
        Dim sDocumentTemplateID As String = ""
        Dim lDocumentTemplateID, lDocumentTypeID As Integer
        Dim oDocTemplate As iPMBDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Get Default Letter template for type
            If m_lCashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                m_lReturn = GetOption(v_iOptionNumber:=61, r_sOptionValue:=sDocumentTemplateID)
            Else
                If m_sActionkey = gACTLibrary.ACTStopCheque Then
                    m_lReturn = GetOption(v_iOptionNumber:=75, r_sOptionValue:=sDocumentTemplateID)
                Else
                    m_lReturn = GetOption(v_iOptionNumber:=63, r_sOptionValue:=sDocumentTemplateID)
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to read system option for Letter Template.")
                Throw New System.Exception("Failed to read system option for Letter Template.")
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return m_lReturn
            End If

            lDocumentTemplateID = gPMFunctions.ToSafeLong(sDocumentTemplateID, 0)

            ' If template has not been specified in Maintain System Options do not proceed!  PN24096
            If lDocumentTemplateID = 0 Then
                'Throw New System.Exception(m_lReturn.ToString() + ", " + +", The letter cannot be produced since the relevant document template for this process has not been configured in Maintain System Options.")
                Throw New System.Exception("The letter cannot be produced since the relevant document template for this process has not been configured in Maintain System Options.")
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return m_lReturn
            End If

            m_lReturn = GetTemplateType(lDocumentTemplateID:=lDocumentTemplateID, r_lDocumentTypeID:=lDocumentTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to Get Template Type.")
                Throw New System.Exception("Failed to Get Template Type.")
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            End If

            oDocTemplate = New iPMBDocTemplate.Interface_Renamed()

            For iIndex As Integer = 0 To m_vLetters.GetUpperBound(1)

                With oDocTemplate

                    .PartyCnt = ToSafeInteger(m_vLetters(0, iIndex))
                    .DocumentRef = ToSafeString(m_vLetters(2, iIndex)).Trim()
                    .InsuranceFileCnt = ToSafeInteger(m_vLetters(3, iIndex))

                    .DocumentTemplateId = lDocumentTemplateID
                    .DocumentTypeId = lDocumentTypeID
                    .Mode = 1
                    .ArchiveMode = False

                    m_lReturn = oDocTemplate.Initialise()
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        oDocTemplate.AutoGenerateDocumentTask = False
                        m_lReturn = .Start()
                    End If

                End With

            Next iIndex

            oDocTemplate.Dispose()
            oDocTemplate = Nothing

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process letters:" & Strings.Chr(13) & Strings.Chr(10) & excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLetters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTemplateType
    '
    ' Description: Get the Document Template Type from the Template
    '
    ' History: 24/03/2005 DD (taken from original frmDocument)
    '
    ' ***************************************************************** '
    Private Function GetTemplateType(ByVal lDocumentTemplateID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer
        Dim result As Integer = 0

        Dim oDocTemplate As bSIRDocTemplate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oDocTemplate Is Nothing Then
                Dim temp_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oDocTemplate = temp_oDocTemplate

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If

            m_lReturn = oDocTemplate.GetDetails(vDocumentTemplateId:=lDocumentTemplateID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = oDocTemplate.GetNext(vDocumentTypeId:=r_lDocumentTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            oDocTemplate.Dispose()

            oDocTemplate = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoAllocate
    '
    ' Description: Auto Allocate the Outstanding Balance
    '
    '
    ' ***************************************************************** '
    Private Function AutoAllocate(ByRef v_lAccountId As Integer, ByRef v_lTransdetailID As Integer, Optional ByRef v_lPaymentType As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String
        Dim vOSTransactions As Object
        Dim cWriteOffAmount As Decimal
        Dim lWriteOffReasonID As Integer
        Dim lAutoAllocationState As gPMConstants.PMEReturnCode 'AR20050203 - PN18491
        Dim bPartPayment As Boolean 'AR20050203 - PN18491
        Dim sLookUpcode As String = ""
        Dim cCurGainLossAutoAllocateLimitAmount As Decimal
        Dim lCurExchangeRateGainLossReasonID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_oAccount Is Nothing Then
                Dim temp_m_oAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
                m_oAccount = temp_m_oAccount

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get an instance of the business object.

                    ' Display error stating the problem.
                    ' Get description from the resource file.

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Display message.
                    MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTAccount", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If

            ' if the documents to auto-allocate to have been passed in
            If Information.IsArray(m_vDocumentIds) Then

                ' get the relevant transactions to allocate to

                m_lReturn = m_oAccount.GetAccountOSTransForDocuments(v_lAccountId:=v_lAccountId, v_vDocumentIds:=m_vDocumentIds, r_vOSTransactions:=vOSTransactions)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                End If

                lAutoAllocationState = gPMConstants.PMEReturnCode.PMTrue

            Else
                ' If Me.chkAutoAllocateIfAble.CheckState = CheckState.Unchecked Then
                If Me.chkAutoAllocateIfAble.Checked = False Then

                    m_lReturn = CheckWriteOff(lAccountID:=v_lAccountId, lCurrencyID:=m_iCurrencyID, cAmount:=CDec(m_vListData(ACSubAmount, m_lSelectedRow)), lMode:=cAUTO_ALLOCATE_BUTTON, r_lStatus:=lAutoAllocationState, r_lWriteOffReasonID:=lWriteOffReasonID, r_cWriteOffAmount:=cWriteOffAmount, r_bPartPayment:=bPartPayment, r_vOSTransactions:=vOSTransactions)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to check write off limits.", vApp:=ACApp, vClass:=ACClass, vMethod:="frmList.AutoAllocate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If

                'If Me.chkAutoAllocateIfAble.CheckState = CheckState.Checked Then
                If Me.chkAutoAllocateIfAble.Checked = True Then
                    m_lReturn = CheckWriteOffAndExchangeRateGainLoss(lAccountID:=v_lAccountId, lCurrencyID:=m_iCurrencyID, cAmount:=CDec(m_vListData(ACSubAmount, m_lSelectedRow)), dCurrencyBaseXRate:=gPMFunctions.ToSafeDouble(m_vListData(ACCurrencyBaseXrate, m_lSelectedRow)), cCurrencyBaseAmount:=gPMFunctions.ToSafeCurrency(m_vListData(ACBaseAmount, m_lSelectedRow)), lMode:=cAUTO_ALLOCATE_BUTTON, r_lStatus:=lAutoAllocationState, r_lWriteOffReasonID:=lWriteOffReasonID, r_cWriteOffAmount:=cWriteOffAmount, r_lCurExchangeRateGainLossReasonID:=lCurExchangeRateGainLossReasonID, r_cCurGainLossAutoAllocationLimitAmount:=cCurGainLossAutoAllocateLimitAmount, r_bPartPayment:=bPartPayment, r_vOSTransactions:=vOSTransactions)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to check write off limits.", vApp:=ACApp, vClass:=ACClass, vMethod:="frmList.AutoAllocate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
            End If

            'AR20050208 - PN18582
            If lAutoAllocationState <> gPMConstants.PMEReturnCode.PMTrue Then
                'Do not auto allocate
                Return result
            End If

            'Do the auto allocation
            If DoAutoAllocation(r_lAccountId:=v_lAccountId, r_lTransdetailid:=v_lTransdetailID, r_vOSTransactions:=vOSTransactions, lWriteOffReasonID:=lWriteOffReasonID, cCurrencyWriteOff:=cWriteOffAmount, v_lCurExchangeRateGainLossReasonID:=lCurExchangeRateGainLossReasonID, v_cCurGainLossAutoAllocationLimitAmount:=cCurGainLossAutoAllocateLimitAmount) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New Exception()
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = DetailsFormList(m_lSelectedRow)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display details on the list.", vApp:=ACApp, vClass:=ACClass, vMethod:="m_oNav_NavigatorClose", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            ' Re-enable the form
            m_lReturn = DisableForm(bEnabled:=True)

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Auto allocation failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoAllocate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object,
                            ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bAllocated As Boolean
        Dim sMsg As String = ""
        Dim bCancel As Boolean
        Dim nCashListItemID As Integer
        Dim bAllPending As Boolean
        Dim oCashListItem As Object

        ' Click event of the OK button.
        Try
            If Me.Task = PMEComponentAction.PMView Then 'If we're in view only mode then just hide the form
                m_lStatus = PMEReturnCode.PMOK 'Set the interface status
                Me.Hide() 'No data changes can be made so no warnings should
                Exit Sub 'appear and no work should be done
            End If

            ' CTAF 290200 - Make sure allocation is clicked in part of Insurer Payments
            If m_sCashListRoadmap = PMNavKeyConst.ACTInsurerPaymentRoadMap OrElse m_sCashListRoadmap = "PFCASHSETTLEMENT" Then
                If m_bApprovalMsg = False Then
                    If Not m_bAllocated Then
                        If MessageBox.Show("You must allocate before continuing. Do you wish to do so now?",
                                           "Allocate",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                            cmdAllocate_Click(cmdAllocate, New EventArgs())
                            Exit Sub
                        Else
                            Exit Sub
                        End If
                    End If
                End If
            End If

            'eck061200
            If m_sCashListRoadmap = "PFCASH" OrElse m_sCashListRoadmap = "PFCASHSETTLEMENT" Then
                'AK 240702 - use business object to save this property value
                m_lReturn = m_oBusiness.UpdateUserProperty(v_sPropertyName:="Cash Transaction",
                                                           v_vPropertyValue:=m_lCashTransDetailID)
                'MKW PN15725 Return Total amount to user properties.
                m_lReturn = m_oBusiness.UpdateUserProperty(v_sPropertyName:="Cash Amount",
                                                           v_vPropertyValue:=m_cTotalAmount)
            End If

            ' Set the interface status.
            m_lStatus = PMEReturnCode.PMOK
            ' Cant continue if theres no cash list items
            'EK 230200 Not sure why
            If lvwListDetails.Items.Count < 1 Then
                '        sMsg = "You cannot proceed without any cash list items."
                '        MsgBox sMsg, vbCritical, "Error"
                '        Exit Sub
            Else
                'CMG/PB 08012003 Loss Schedule PS202
                'If this is a loss schedule payment then dont check for allocation
                If Not m_bLossSchedule And m_lDisplayState <> gACTLibrary.ACTUseListHidden Then
                    m_lReturn = CheckAllocated(v_bAllocated:=bAllocated)
                ElseIf m_lDisplayState = gACTLibrary.ACTUseListHidden Then
                    m_lReturn = m_oBusiness.Update()
                    ' Check for errors.
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        ' Failed to update the details
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                           sMsg:="Failed to update the database with the details in the business object",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="cmdOK_Click")
                    End If

                    'first find the cashlistitem ID of the new listitem
                    m_lReturn = m_oBusiness.GetNext(oCashListItem)
                    nCashListItemID = CInt(oCashListItem(gACTLibrary.eCashListItem.CashlistitemID))
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        'Failed to update the details
                        'Log Error.
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                           sMsg:="Failed to get the new item ID from the business object",
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    End If

                    Post(nCashListItemID)
                    m_bAllowAllocateButton = False
                End If
                'End CMG
            End If

            bCancel = False
            'eck140700 No longer valid for work manager task
            If (Me.Task <> PMEComponentAction.PMView) AndAlso (lvwListDetails.Items.Count > 0) Then
                If (m_bAllowAllocateButton) AndAlso
                    (m_sActionkey <> gACTLibrary.ACTUnderwritingDirect) AndAlso
                    (m_lDisplayState <> gACTLibrary.ACTUseListHidden) Then
                    'Check that all the status is pending or not
                    m_lReturn = CheckAllPending(r_bAllIsPending:=bAllPending)

                    'PN: 45790
                    m_lReturn = CheckAllocated(v_bAllocated:=bAllocated)

                    If Not bAllPending Or Not bAllocated Then
                        If Not bAllocated Then
                            'Set the question
                            Select Case m_bPosted
                                Case True
                                    sMsg = "You have not allocated all of the items." & Environment.NewLine &
                                           "Do you wish to exit the program ?"
                                Case False
                                    sMsg = "You have not posted the cash list items these items will be lost" &
                                           Environment.NewLine &
                                           "Do you wish to exit the program ?"
                            End Select

                            m_lReturn = MessageBox.Show(sMsg, "Cash List Items", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                                ' Create a work manager task
                                'TN20010907 - add task to work manager for underwriting
                                'DD 10/08/2003: Do not do this for Cash Drawers
                                If m_bPosted And m_vCashDrawerID = 0 Then

                                    ' START CHANGES - Changed By: AAB  - Changed On: 09-Mar-2004 15:21
                                    ' Added this to ensure that everything is in place updated before we
                                    ' AddTaskToWorkManager
                                    'Update the business object so the cash list id's are on the database

                                    m_lReturn = m_oBusiness.Update()
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                           sMsg:="Failed during the Update process.",
                                                           vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                                        Exit Sub
                                    End If

                                    'Thinh Nguyen 09/03/2004 - make sure we start at top of collection

                                    m_oBusiness.CurrentRecord = 0

                                    'Update the list object
                                    m_lReturn = BusinessToData()
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                           sMsg:="Failed BusinessToData.",
                                                           vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                                        Exit Sub
                                    End If
                                    ' END CHANGES - Changed By: AAB  - Changed On: 09-Mar-2004 15:21
                                    m_lReturn = AddTaskToWorkManager()
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                           sMsg:="Failed AddTaskToWorkManager.",
                                                           vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                                        Exit Sub
                                    End If
                                End If
                            Else
                                Exit Sub
                            End If
                        End If
                    Else
                        If m_vCashDrawerID = 0 Then
                            'Update the business object so the cash list id's are on the database
                            m_lReturn = m_oBusiness.Update()
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                   sMsg:="Failed during the Update process.",
                                                   vApp:=ACApp, vClass:=ACClass,
                                                   vMethod:="cmdOK_Click")
                                Exit Sub
                            End If

                            'Thinh Nguyen 09/03/2004 - make sure we start at top of collection

                            m_oBusiness.CurrentRecord = 0

                            'Update the list object
                            m_lReturn = BusinessToData()
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                   sMsg:="Failed BusinessToData.",
                                                   vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                                Exit Sub
                            End If
                            'Add a new work task item to each item on the list
                            m_lReturn = AddTaskToWorkManager()
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                                   sMsg:="Failed AddTaskToWorkManager.",
                                                   vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                                Exit Sub
                            End If
                        End If
                    End If
                Else
                    'Add a work task if called as a part of Claim nevigator Process
                    m_lReturn = AddTaskToWorkManager()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogError,
                                           sMsg:="Failed AddTaskToWorkManager.",
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                End If
            End If

            ' Gaurav DOUBT
            If Not m_bPosted And m_oBusiness.UnderwritingOrAgency = "U" And bAllPending = False And m_vCashDrawerID = 0 Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                m_lReturn = AddTaskToWorkManager()
            End If



            ' Process the next set of actions.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = PMEReturnCode.PMTrue Then
                If (m_sCashListRoadmap = ACTInsurerPaymentRoadMap AndAlso m_sIncludeInsurerPaymentMultiStep = "1") AndAlso m_bApprovalMsg = True Then
                    m_lStatus = PMEReturnCode.PMOK
                    Me.Hide()
                    Exit Sub
                End If

                ' If the user is creating a work manager task, then cancel out, so
                ' we dont go to the next navigator step.

                If (bCancel) And (m_vAllocationIDs Is Nothing) Then
                    m_lStatus = PMEReturnCode.PMCancel
                    Me.Hide()
                    Exit Sub
                End If

                ' Everything OK, so we can hide the interface.
                If m_lDisplayState <> gACTLibrary.ACTUseListHidden Then
                    Me.Hide()
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim sMsg As String = ""
        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            If Me.Task = gPMConstants.PMEComponentAction.PMView Then 'If we're in view only mode then just hide the form
                Me.Hide() 'no data changes can be made so no warnings should
                Exit Sub 'appear and no work should be done
            End If

            If lvwListDetails.Items.Count > 0 And m_sActionkey <> gACTLibrary.ACTUnderwritingDirect Then
                Select Case m_bPosted
                    Case True
                        sMsg = "You have not allocated all of the items." & Environment.NewLine &
                               "Do you wish to exit the program ?"
                    Case False
                        sMsg = "You have not posted the cash list items these items will be lost" & Environment.NewLine &
                               "Do you wish to exit the program ?"
                End Select

                m_lReturn = MessageBox.Show(sMsg, "Cash List Items", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)

                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                Else
                    'TN20010907 - add task to work manager for underwriting
                    'DD 10/08/2003: Do not do this for Cash Drawers
                    If m_bPosted And m_vCashDrawerID = 0 Then
                        m_lReturn = AddTaskToWorkManager()
                    End If

                End If
            End If

            ' Process the next set of actions.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        ' Click event of the Add Button.
        Try

            Dim iRow As Integer
            ' {* USER DEFINED CODE (Begin) *}

            If lvwListDetails.Items.Count >= 250 Then
                MessageBox.Show("You can only add a maximum of 250 items to a Cash List.", "Cash List limit reached", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else

                ' Add Details form

                If chkSplitReceipt.Checked = True And lvwListDetails.Items.Count >= 1 Then
                    m_bIsLeadAccount = False

                    'More than one row in the collection & no lead entry.The next receipt has to be Lead.
                    'Check if LeadAccount = True & CollectionHasLead = False in CashListItemDetails
                    m_bCollectionHasLead = False
                    For iRow = 0 To lvwListDetails.Items.Count - 1
                        If lvwListDetails.Items(iRow).SubItems(6).Text = "Lead" Then
                            m_bCollectionHasLead = True
                        End If
                    Next
                    If m_bCollectionHasLead = False Then
                        m_bIsLeadAccount = True
                    End If

                Else
                    m_bCollectionHasLead = True
                    m_bIsLeadAccount = True
                End If

                ' SetValues for ReceiptTab , will only be used when LeadAccount = False
                If chkSplitReceipt.Checked = True Then
                    SetSplitReceiptDefaults()
                End If

                m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMAdd)

                bMessageDisplayed = False

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If

                'DC PN13006 Don't Do this anymore
                ProcessCashDrawerFunctionalityForAdd()

                'DC191203 PN6318 -do not give option to Add anymore
                'AR20050125 - PN18271
                If (ViaInsurerPayment And chkSplitReceipt.Checked = False) Or m_bViaClaimPayment Or ViaBanking Then

                    cmdAdd.Enabled = False

                End If

                If chkSplitReceipt.Checked = True Then
                    m_lReturn = EnableButtons()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If
                    m_lReturn = Validate("SplitReceipt", gPMConstants.PMEComponentAction.PMAdd)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If
                End If

                'Refresh to prevent the list from looking funny when their are scrollbars.
                lvwListDetails.Refresh()
                If (m_sScreenType <> "PAYNOW") Then
                    lvwListDetails.Items(Convert.ToInt32(Convert.ToString(lvwListDetails.FocusedItem.Tag))).Selected = True
                End If
                lvwListDetails.Select()

            End If
            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub ProcessCashDrawerFunctionalityForAdd()

        Dim lCashListItemID, lTransDetailID, lReceiptTypeID As Integer
        Dim sLookUpcode As String = ""
        Dim sAccountShortCode As String = ""
        Dim sUserGroupCode As String = ""
        Dim lNewRowID As Integer 'The index of the newly added m_vListData array element
        Dim bTransactionStarted As Boolean
        Dim vCashListItem As Object
        Dim lMediaTypeIssuerID As Integer
        Dim sAuthCode, sTransactionCode As String
        Dim bReceiptTypeIsInstalmentBased As Boolean

        Const klUserError As Integer = 32767

        Try

            bTransactionStarted = False

            'Get the index of the newly added array element
            lNewRowID = m_vListData.GetUpperBound(1)

            If gPMFunctions.ToSafeInteger(m_vListData(ACvCashlistitemreceipttypeid, lNewRowID)) <> 0 Then
                m_lReturn = GetReceiptTypeDetails(v_lReceiptTypeId:=gPMFunctions.ToSafeInteger(m_vListData(ACvCashlistitemreceipttypeid, lNewRowID)), r_sReceiptTypeAlias:=sLookUpcode, r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
            End If

            lMediaTypeIssuerID = gPMFunctions.ToSafeInteger(m_vListData(ACMediatype_IssuerID, lNewRowID))
            sAuthCode = gPMFunctions.ToSafeString(m_vListData(ACCCauthcode, lNewRowID))
            sTransactionCode = gPMFunctions.ToSafeString(m_vListData(ACCCtransactioncode, lNewRowID))

            'Automatically post Instalments and Authorised Credit Cards, and when the list is not hidden
            If (lMediaTypeIssuerID <> 0 And sAuthCode <> "" And sTransactionCode <> "" And m_lDisplayState <> gACTLibrary.ACTUseListHidden) Then

                'added sw front office receipting
                'we need to call process command to add the cash list item to the db this will allocate
                'a cashlistitemID for the new item, this is required to enable us to add instalments

                m_lReturn = m_oBusiness.Update()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update the details
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the database with the details in the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashDrawerFunctionalityForAdd")
                End If

                'first find the cashlistitem ID of the new listitem
                'PN13006
                'Only process the current line
                While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF And m_lReturn <> gPMConstants.PMEReturnCode.PMFalse

                    m_lReturn = m_oBusiness.GetNext(vCashListItem)
                End While
                If m_lReturn = gPMConstants.PMEReturnCode.PMEOF Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                End If
                'PN13006

                lCashListItemID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))

                lReceiptTypeID = CInt(vCashListItem(gACTLibrary.eCashListItem.CashListItem_receipt_type_id))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Failed to update the details
                    'Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the new item ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashDrawerFunctionalityForAdd")
                End If

                If PMNavKeyConst.ACTKeyNameActionKey = m_sActionkey Or m_lCashListTypeID = ACPaymentType Then

                    Post(lCashListItemID)

                    'first find the cashlistitem ID of the new listitem
                    lTransDetailID = m_lCashTransDetailID

                    'Update the row Status and in the database
                    m_vListData(ACSubRealAllocationID, lNewRowID) = gACTLibrary.ACTAllocationStatusPosted
                    m_vListData(ACSubCashListItemID, lNewRowID) = lCashListItemID

                    If DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=lNewRowID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Raise the error
                        Throw New System.Exception(klUserError.ToString() + ", " + +", Failed to update the business object.")
                    End If

                    ' Inform the business of the current record
                    ' so that the lookup ID match works

                    m_oBusiness.CurrentRecord = m_vListData(m_vListData.GetUpperBound(0), lNewRowID)

                    'Update the database

                    If m_oBusiness.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Raise the error
                        Throw New System.Exception(klUserError.ToString() + ", " + +", Failed to update the database.")
                    End If

                End If

                DataToInterface()
                'PM044947 If transaction is already posted and allocated, use this to refresh button status.
                If m_vListData(ACSubRealAllocationID, lNewRowID) = gACTLibrary.ACTAllocationStatusAllocated Then
                    DetailsFormList(lNewRowID)
                    m_bPosted = True
                End If



                m_vListData(ACSubTransdetailId, lNewRowID) = lTransDetailID
                m_vListData(ACSubCashListItemID, lNewRowID) = lCashListItemID
            End If

            If bTransactionStarted Then

                If m_oBusiness.CommitTrans <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Raise the error
                    Throw New System.Exception(klUserError.ToString() + ", " + +", Failed to commit a transaction")
                End If
            End If

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception

            'Rollback any transactions started
            If bTransactionStarted Then

                m_oBusiness.RollbackTrans()
            End If

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process ProcessCashDrawerFunctionalityForAdd Method", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCashDrawerAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        ' Click event of the Edit Button.
        Dim iRow As Integer
        Try

            ' {* USER DEFINED CODE (Begin) *}

            If chkSplitReceipt.Checked = True Then

                m_lReturn = SetIsLeadAccount()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
                m_bCollectionHasLead = False
                For iRow = 0 To lvwListDetails.Items.Count - 1
                    If lvwListDetails.Items(iRow).SubItems(6).Text = "Lead" Then
                        m_bCollectionHasLead = True
                    End If
                Next
            End If
            If (lvwListDetails.FocusedItem) Is Nothing Then
                m_lSelectedRow = 0
            Else
                m_lSelectedRow = Convert.ToInt32(Convert.ToString(lvwListDetails.FocusedItem.Tag))
            End If

            ' Edit Details form
            m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            If ActionKey = gACTLibrary.ACTStopCheque And bStopCheque Then
                'we need to produce the the stopcheque letter
                PopulateLetterArrayForStopCheque()
            End If

            If chkSplitReceipt.Checked Then
                m_lReturn = EnableButtons()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            If chkSplitReceipt.Checked = True Then
                m_lReturn = Validate("SplitReceipt")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            'Refresh to prevent the list from looking funny when their are scrollbars.
            lvwListDetails.Refresh()

            ' {* USER DEFINED CODE (End) *}

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' Remove Details
            m_lReturn = DetailsDelete()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' If its part of the cash list
            If Not m_bAllowAllocateButton Then
                cmdAdd.Enabled = Not (lvwListDetails.Items.Count >= 1)
            Else
                cmdPost.Enabled = True
            End If
            m_lReturn = RebuildBGArray()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RebuildBGArray", "RebuildBGArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' {* USER DEFINED CODE (End) *}
            m_lReturn = EnableButtons()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Refresh to prevent the list from looking funny when their are scrollbars.
            lvwListDetails.Refresh()

        Catch excep As System.Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Remove button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRemove_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Function RebuildBGArray(Optional ByRef RemoveAt As Integer = -1) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RebuildBGArray"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Dim uboundBankDetails As Long

            m_vBGAllPoliciesForReceipt = Nothing
            'uboundBankDetails = UBound(m_vBGAllPoliciesForReceipt, 2)

            If Information.IsArray(m_vListData) Then
                For lRowCount As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                    If lRowCount <> RemoveAt Then
                        If Information.IsArray(m_vListData(73, lRowCount)) Then
                            For i As Integer = m_vListData(73, lRowCount).GetLowerBound(1) To m_vListData(73, lRowCount).GetUpperBound(1) 'm_vListData(73, 0)(5, i), _ m_vListData(73, 0)(17, i))
                                If Not Information.IsArray(m_vBGAllPoliciesForReceipt) Then
                                    ReDim m_vBGAllPoliciesForReceipt(MainModule.ENBankGuarantee.LastItem, 0)
                                Else

                                    ReDim Preserve m_vBGAllPoliciesForReceipt(m_vBGAllPoliciesForReceipt.GetUpperBound(0), m_vBGAllPoliciesForReceipt.GetUpperBound(1) + 1)
                                End If
                                For lColCount As Integer = 0 To m_vListData(73, lRowCount).GetUpperBound(0)

                                    m_vBGAllPoliciesForReceipt(lColCount, m_vBGAllPoliciesForReceipt.GetUpperBound(1)) = m_vListData(73, lRowCount)(lColCount, i) 'm_vSelectdBGPoliciesItem(lColCount, lRowCount)
                                Next

                                'lSelRowCount = lSelRowCount + 1
                            Next
                        End If
                    End If
                Next
                'ReDim Preserve m_vBGAllPoliciesForReceipt(UBound(m_vBGAllPoliciesForReceipt, 1), UBound(m_vBGAllPoliciesForReceipt, 2) - 1)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmList_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        'RKS PN14255
        If Not (m_oUserAuthorities Is Nothing) Then

            m_oUserAuthorities.Dispose()
            m_oUserAuthorities = Nothing
        End If
        If Not (m_oAccount Is Nothing) Then

            m_oAccount.Dispose()
            m_oAccount = Nothing
        End If

    End Sub

    Private Sub lvwListDetails_ItemClick(ByVal Item As ListViewItem)

        ' Single ItemClick event for the List details.

        Try

            '01/05/2003 - PWC - ENDVR00000850
            Select Case m_iCashListStatusId
                Case gACTLibrary.ACTCashListStatusClosed, gACTLibrary.ACTCashListStatusInBanking
                    'Disable the form
                    lvwListDetails.Enabled = True
                    m_lReturn = EnableButtons()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If
                    Exit Sub
            End Select

            ' If its part of the cash list
            'eck130700 added extra if to check if transaction have been posted
            m_lReturn = EnableButtons()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.DoubleClick

        ' Double click event for the List details.

        Try

            ' Check if there are any items available.
            If lvwListDetails.Items.Count = 0 Then
                Exit Sub
            End If

            If chkSplitReceipt.Checked = True Then

                m_lReturn = SetIsLeadAccount()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

            m_lSelectedRow = Convert.ToInt32(Convert.ToString(lvwListDetails.FocusedItem.Tag))
            ' Only edit if Edit is enabled
            ' SET 28102002 - PN Ref. 731 - only allow edit if edit button
            '                is both visible and enabled.
            If (cmdEdit.Enabled) And (cmdEdit.Visible) Then

                ' Bring up Edit Details form
                m_lReturn = DetailsFormProcess(gPMConstants.PMEComponentAction.PMEdit)

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then Exit Sub

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If

            End If

            If chkSplitReceipt.Checked = True Then
                m_lReturn = Validate("SplitReceipt")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Enter

        ' GotFocus Event for the List details

        Try

            ' Check if there are any items available.
            'eck130700
            If m_bPosted Then
                VB6.SetDefault(cmdAllocate, True)
            Else
                VB6.SetDefault(cmdAdd, True)
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwListDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwListDetails.Leave

        ' LostFocus Event for the List details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdOK, True)
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwListDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwListDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwListDetails.Columns(eventArgs.Column)

        ' Column click event for the List details

        Try

            With lvwListDetails
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwListDetails) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwListDetails, (ListViewHelper.GetSortOrderProperty(lvwListDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwListDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwListDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwListDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwListDetails, True)
                End If
            End With

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' *************************************************************************
    '
    ' Name:         NavigatorClose
    '
    ' Description:  This is called when navigator is closed. We can re-enable
    '               the form at this time.
    '
    ' *************************************************************************
    Private Sub m_oNav_NavigatorClose() Handles m_oNav.NavigatorClose
        Dim lStatus As Integer
        Dim cCashListAmt, cAllocAmt As Decimal

        ' Exit if we have no business.
        If m_oBusiness Is Nothing Then
            Exit Sub
        End If
        ' Read the value

        m_lReturn = m_oBusiness.GetUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationId, r_vPropertyValue:=m_lAllocationID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lAllocationID = 0
        End If

        'TRANSMATCH DOUBLING BUG
        'EK 23/12/99 Check that an allocation was made
        If m_lAllocationID <> 0 Then
            ' Has the array been initialised yet?
            If Not Information.IsArray(m_vAllocationIDs) Then
                ReDim m_vAllocationIDs(0)
            Else
                ReDim Preserve m_vAllocationIDs(m_vAllocationIDs.GetUpperBound(0) + 1)
            End If

            ' Save the allocation_id in an array
            m_vAllocationIDs(m_vAllocationIDs.GetUpperBound(0)) = m_lAllocationID
            'EK 230200
        Else
            m_bProcessComplete = False
        End If

        If m_bProcessComplete Then

            ' CF ----- Check here if full or partial allocation TODO

            ' Get the amount of the cash list selected
            cCashListAmt = CDec(m_vListData(ACSubAmount, m_lSelectedRow))

            'EK 100100 Get Cash List currency and pass to the business
            m_lReturn = GetCashListDetails(v_lCashListID:=m_lCashlistID, r_vCurrencyID:=m_iCurrencyID)

            m_lReturn = m_oBusiness.GetMultiAllocationStatus(v_lCashListItemID:=m_vListData(ACSubCashListItemID, m_lSelectedRow), v_iCashListCurrency:=m_iCurrencyID, v_cCashListAmt:=cCashListAmt, r_lAllocationStatus:=lStatus, r_cAllocAmt:=cAllocAmt)
            'eck 030701 More friendly error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Warning - Cash has not been allocated", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' Set the status to allocated
            m_vListData(ACSubRealAllocationID, m_lSelectedRow) = lStatus
            'eck250800

            ' Update the business
            m_lReturn = DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=m_lSelectedRow)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business.", vApp:=ACApp, vClass:=ACClass, vMethod:="m_oNav_NavigatorClose", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
        End If
        'eck250800 Update status Now

        m_lReturn = m_oBusiness.Update()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business.", vApp:=ACApp, vClass:=ACClass, vMethod:="m_oNav_NavigatorClose", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If ' Re-display the information
        m_lReturn = DetailsFormList(m_lSelectedRow)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to display details on the list.", vApp:=ACApp, vClass:=ACClass, vMethod:="m_oNav_NavigatorClose", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        ' Re-enable the form
        m_lReturn = DisableForm(bEnabled:=True)

        ' CTAF 020300 - Only terminate if it exists
        If Not (m_oNav Is Nothing) Then
            m_oNav.Dispose()
            m_oNav = Nothing
        End If

    End Sub
    ' PRIVATE Events (End)
    Private Sub m_oNav_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNav.SetProcessStatus

        ' Store it so we can check in Close event
        m_bProcessComplete = v_bProcessComplete

    End Sub

    ''' <summary>
    ''' Enables and disables buttons based on current form status, data, and entry point
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EnableButtons() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            Dim bItemsInListView As Boolean
            'Move the View and Reverse buttons up a touch
            cmdView.Top = cmdEdit.Top
            cmdReverse.Top = cmdRemove.Top
            If lvwListDetails.Items.Count > 0 Then
                bItemsInListView = True
            Else
                bItemsInListView = False
            End If

            If m_vCashDrawerID = 0 Then 'We are on a cash list
                cmdReverse.Visible = False 'Reverse is never available here
                If Me.SourceForm = ACSourceFindCashList Then 'We have come from the find screen
                    cmdView.Enabled = bItemsInListView
                    'We may or may not have loaded the array yet depending on
                    'when we're being called
                    If Information.IsArray(m_vListData) Then
                        'If the status of the current item is already allocated then disable allocate button
                        If CInt(m_vListData(ACSubRealAllocationID, m_lSelectedRow)) = gACTLibrary.ACTAllocationStatusAllocated Then
                            cmdAllocate.Enabled = False
                        Else
                            If lvwListDetails.Items.Count > 0 Then
                                If CInt(m_vListData(ACCashlistitemreversereasonid, Convert.ToString(lvwListDetails.FocusedItem.Tag))) <> 0 Then
                                    cmdAllocate.Enabled = False
                                Else
                                    cmdAllocate.Enabled = bItemsInListView
                                    If bItemsInListView Then
                                        SetAllocateButtonVisibility() 'PN24911
                                    Else
                                        cmdAllocate.Visible = False
                                        Me.chkAutoAllocateIfAble.Enabled = False
                                    End If
                                End If
                            Else
                                cmdAllocate.Enabled = False
                            End If
                        End If
                    Else
                        'If we haven't loaded the array yet then just disable it
                        cmdAllocate.Enabled = False
                        cmdAllocate.Visible = False
                        Me.chkAutoAllocateIfAble.Enabled = False
                    End If

                    cmdOK.Enabled = True
                    cmdCancel.Enabled = True
                    cmdRemove.Enabled = False
                    cmdAdd.Enabled = False
                    cmdEdit.Enabled = False
                    cmdPost.Visible = False
                Else
                    'We've come through the 'Cash / Cheque Receipt' route, but we don't know if this is the first time
                    'we've been called or not so check to see what's in the list view
                    cmdOK.Enabled = True
                    cmdCancel.Enabled = True
                    'If insurer payment then don't allow the adding of more than one cashlistitem.
                    If (ViaInsurerPayment Or m_bViaClaimPayment Or ViaBanking Or UCase(m_sCallingAppName) = "IPMWRKCOMPONENTSTARTER") And lvwListDetails.Items.Count >= 1 Then
                        cmdAdd.Enabled = False
                    Else
                        cmdAdd.Enabled = True
                    End If


                    cmdEdit.Enabled = bItemsInListView

                    cmdRemove.Enabled = bItemsInListView
                    If Not bItemsInListView Then 'If there's nothing in the list view
                        cmdAllocate.Visible = False 'then we can't allocate or post
                        cmdPost.Visible = True 'But make post visible because
                        cmdPost.Enabled = False 'we'll do that before we allocate
                        'DC300704 PN13006 only enabled if items in list
                        cmdView.Enabled = False
                        Me.chkAutoAllocateIfAble.Enabled = False

                    Else
                        'DC300704 PN13006 only enabled if items in list
                        cmdView.Enabled = True
                        'We have items in the list view
                        If m_bPosted Then 'so check if we've posted
                            cmdPost.Visible = False
                            'Has this item been allocated?
                            If chkSplitReceipt.Checked Then
                                If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                                    If ToSafeInteger(m_vListData(ACSubRealAllocationID, m_lSelectedRow)) = gACTLibrary.ACTAllocationStatusUnallocated AndAlso m_sCallingAppName = "SplitReceiptsFromFindTransaction" Then
                                        'not Posted
                                        m_bPosted = False
                                        cmdPost.Visible = True
                                    End If
                                    If CInt(m_vListData(ACSubRealAllocationID, m_lSelectedRow)) = gACTLibrary.ACTAllocationStatusAllocated Then
                                        cmdAllocate.Enabled = False
                                        cmdEdit.Enabled = False
                                        cmdRemove.Enabled = False
                                    Else
                                        If m_bApprovalMsg Then
                                            cmdAllocate.Enabled = False
                                        Else
                                            cmdAllocate.Enabled = True
                                            SetAllocateButtonVisibility() 'PN24911
                                        End If
                                    End If
                                End If
                            Else

                                If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                                    If CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) = gACTLibrary.ACTAllocationStatusAllocated Then
                                        cmdAllocate.Enabled = False
                                        cmdEdit.Enabled = False
                                        cmdRemove.Enabled = False
                                    Else
                                        If m_bApprovalMsg Then
                                            cmdAllocate.Enabled = False
                                        Else
                                            cmdAllocate.Enabled = True
                                            SetAllocateButtonVisibility() 'PN24911
                                        End If
                                    End If
                                End If
                            End If
                            cmdAdd.Enabled = False
                            cmdEdit.Visible = False
                            cmdRemove.Enabled = False
                        Else
                            'We haven't posted yet so we can't allocate
                            cmdAllocate.Visible = False
                            Me.chkAutoAllocateIfAble.Enabled = False
                            'DC290704 PN13006 do not allow instalment line to be editted/removed
                            If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                                If CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) = gACTLibrary.ACTAllocationStatusUnallocated _
                                     And UCase(m_sCallingAppName) <> "IPMWRKCOMPONENTSTARTER" Then
                                    cmdEdit.Visible = bItemsInListView
                                    cmdEdit.Enabled = bItemsInListView
                                    cmdRemove.Visible = bItemsInListView
                                    cmdRemove.Enabled = bItemsInListView
                                    cmdView.Visible = False
                                    cmdView.Enabled = False
                                Else
                                    cmdView.Visible = bItemsInListView
                                    cmdView.Enabled = bItemsInListView
                                    cmdRemove.Visible = False
                                    cmdRemove.Enabled = False
                                End If

                                If Not Object.Equals(m_vListData(ACInstalmentArray, Convert.ToString(lvwListDetails.FocusedItem.Tag)), Nothing) Then
                                    cmdEdit.Visible = False
                                    cmdEdit.Enabled = False
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                'We are on a cash drawer
                cmdPost.Visible = False 'We can never post here
                cmdRemove.Visible = False
                cmdEdit.Visible = False
                If Me.SourceForm = ACSourceFindCashList Then 'We have come from the find screen
                    Select Case m_iCashListStatusId 'Get the status of this cash drawer
                        Case gACTLibrary.ACTCashListStatusClosed
                            cmdView.Enabled = bItemsInListView
                            'DD 25/09/2003: Cannot allocate/reverse on closed drawers
                            cmdReverse.Enabled = False
                            cmdReverse.Visible = True
                            cmdAllocate.Enabled = False
                            SetAllocateButtonVisibility() 'PN24911
                            cmdAdd.Enabled = False
                        Case gACTLibrary.ACTCashListStatusInBanking
                            cmdView.Enabled = bItemsInListView
                            cmdEdit.Visible = False
                            cmdAdd.Enabled = False
                            cmdRemove.Enabled = False
                            cmdAllocate.Enabled = False
                            cmdReverse.Enabled = False
                        Case Else 'We're Open or Entered
                            cmdView.Enabled = bItemsInListView
                            cmdRemove.Enabled = False
                            If m_bHasSecurityAccess Then
                                'DJM 14/01/2004 : If insurer payment then don't allow the adding of more than one cashlistitem.
                                'AR20050125 - PN18271
                                cmdAdd.Enabled = Not ((ViaInsurerPayment OrElse m_bViaClaimPayment OrElse ViaBanking) AndAlso
                                                      lvwListDetails.Items.Count >= 1)

                                If Information.IsArray(m_vListData) AndAlso lvwListDetails.Items.Count > 0 Then
                                    If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                                        If CInt(m_vListData(ACCashlistitemreversereasonid, Convert.ToString(lvwListDetails.FocusedItem.Tag))) <> 0 Then
                                            cmdAllocate.Enabled = False
                                            cmdReverse.Enabled = False
                                        Else
                                            'Check the status of this item, if it's Unallocated, Partial
                                            'or Posted then enable the allocate button
                                            If CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                                gACTLibrary.ACTAllocationStatusUnallocated OrElse
                                                CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                                gACTLibrary.ACTAllocationStatusPosted OrElse
                                                CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                                gACTLibrary.ACTAllocationStatusPartial Then
                                                cmdAllocate.Enabled = bItemsInListView
                                            Else
                                                cmdAllocate.Enabled = False
                                            End If
                                        End If
                                    End If
                                    cmdReverse.Enabled = (bItemsInListView AndAlso (Conversion.Val(m_vAllowReversals) = PMEReturnCode.PMTrue))
                                End If
                            Else
                                'No security access, can only view
                                cmdAdd.Enabled = False
                                cmdReverse.Enabled = False
                                cmdAllocate.Enabled = False
                            End If
                    End Select
                Else
                    'We're on a cash drawer
                    'but we must have come through the
                    'Open Cash Drawer' route
                    'DJM 14/01/2004 : If insurer payment then don't allow the adding of more than one cashlistitem.
                    cmdAdd.Enabled = Not ((ViaInsurerPayment OrElse m_bViaClaimPayment) AndAlso lvwListDetails.Items.Count >= 1)
                    cmdView.Enabled = bItemsInListView
                    SetAllocateButtonVisibility() 'PN24911
                    'If we've loaded the array and we've
                    'got a row selected in the list view
                    If Information.IsArray(m_vListData) AndAlso lvwListDetails.Items.Count > 0 Then
                        'Since there is no status in the DB for Reversed
                        'We need to check the reverse reason.
                        'If it is reversed it is effectively read-only
                        If (Not Information.IsNothing(lvwListDetails.FocusedItem)) Then
                            If CInt(m_vListData(ACCashlistitemreversereasonid, Convert.ToString(lvwListDetails.FocusedItem.Tag))) <> 0 Then
                                cmdReverse.Enabled = False
                                cmdAllocate.Enabled = False
                            Else
                                'Otherwise we need to check if we're Allocated, Partial or Posted
                                If CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                    gACTLibrary.ACTAllocationStatusUnallocated OrElse
                                    CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                    gACTLibrary.ACTAllocationStatusPosted OrElse
                                    CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) =
                                    gACTLibrary.ACTAllocationStatusPartial Then
                                    cmdAllocate.Enabled = bItemsInListView
                                    cmdReverse.Enabled = (bItemsInListView AndAlso (Conversion.Val(m_vAllowReversals) = PMEReturnCode.PMTrue))
                                    'If we're allocated then disable allocate
                                    'and enable reverse (if there's data present)
                                ElseIf CInt(m_vListData(ACSubRealAllocationID, Convert.ToString(lvwListDetails.FocusedItem.Tag))) = gACTLibrary.ACTAllocationStatusAllocated Then
                                    cmdAllocate.Enabled = False
                                    cmdReverse.Enabled = (bItemsInListView AndAlso (Conversion.Val(m_vAllowReversals) = PMEReturnCode.PMTrue))
                                End If
                            End If
                        End If
                    Else
                        'DJM 14/01/2004 : If insurer payment then don't allow the adding of more than one cashlistitem.
                        cmdAdd.Enabled = Not ((ViaInsurerPayment OrElse m_bViaClaimPayment) AndAlso lvwListDetails.Items.Count >= 1)
                        cmdView.Enabled = False
                        cmdAllocate.Enabled = False
                        cmdReverse.Enabled = False
                    End If
                End If
            End If
            'PN11263
            'PN16135 Moved so that this is always executed
            If ViaFinancePlan And m_sCashListRoadmap <> "PFCASHSETTLEMENT" Then
                cmdAllocate.Enabled = False
                cmdAllocate.Visible = False
                Me.chkAutoAllocateIfAble.Enabled = False
            ElseIf ViaFinancePlan And m_sCashListRoadmap = "PFCASHSETTLEMENT" Then
                Me.chkAutoAllocateIfAble.Enabled = False
            End If

            If m_sCallingAppName = "SplitReceiptsFromFindTransaction" Then
                cmdView.Enabled = False
                cmdView.Visible = False
                cmdEdit.Visible = True
                cmdEdit.Enabled = True
            End If

            If chkSplitReceipt.Visible = True AndAlso
                chkSplitReceipt.Checked = True AndAlso ViaInsurerPayment Then
                cmdAdd.Enabled = True
            End If

            If chkSplitReceipt.Visible = True AndAlso chkSplitReceipt.Checked = True Then
                If ((m_cTotalAmount <> CDec(pnlTotal.Text)) OrElse CDec(pnlTotal.Text) = CDec(0.0)) AndAlso (lvwListDetails.Items.Count > -1) Then
                    cmdAdd.Enabled = True
                    cmdPost.Enabled = False
                Else
                    cmdAdd.Enabled = False
                    cmdPost.Enabled = True
                End If
            End If

            If IsArray(m_vListData) Then
                If m_vListData(kTaxBandID, 0) > 0 Then
                    TaxBandID = m_vListData(kTaxBandID, 0)
                    TaxAmount = m_vListData(kTaxAmount, 0)
                End If
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set button configuration", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    'Steve watton 10-11-2002
    'populate the letter array for a stop cheque letter
    Private Sub PopulateLetterArrayForStopCheque()
        Dim lRow, lAccountID, lTransDetailID, lPartyCnt As Integer
        Dim sShortName, sDocumentRef As String

        Try

            ' AMB 06/03/2003: IS2495 - check that something's actually been selected
            If lvwListDetails.FocusedItem Is Nothing Then
                MessageBox.Show("Please select an item in the list", "Select", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            lRow = Convert.ToString(lvwListDetails.FocusedItem.Tag)

            lAccountID = CInt(m_vListData(ACSubAccountID, lRow))
            lTransDetailID = CInt(m_vListData(ACSubTransdetailId, lRow))

            ReDim m_vLetters(2, 0)

            m_lReturn = m_oBusiness.GetLetterDetails(lAccountId:=lAccountID, lTransdetailId:=lTransDetailID, lPartyCnt:=lPartyCnt, sShortName:=sShortName, sDocumentRef:=sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            m_vLetters(0, 0) = lPartyCnt
            m_vLetters(1, 0) = sShortName.Trim()
            m_vLetters(2, 0) = sDocumentRef.Trim()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Letter Array For Stop Cheque", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLetterArrayForStopCheque", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: AddTaskToWorkManager
    '
    ' Description: add task to work manager
    '
    ' History: 07/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager() As Integer
        Dim result As Integer = 0
        Dim lTaskInstanceCnt As Integer
        Dim sTaskDesc, vCashListRef As String
        Dim vListDate As Date
        Dim vKeyArray(,) As Object

        Dim sCashItemMediaRef, sCashItemOurRef, sCashItemTheirRef As String
        Dim cCashItemAmount As Decimal
        Dim sTaskDescComplete As New StringBuilder
        Dim vOptionValue As Object
        Dim sUserGroup As String = ""

        Dim oAccount As bACTAccount.Form
        Dim sAccountShortCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AAB-02-Feb-2004 15:05 - We only need to do this if we are doing payments
            If m_lCashListTypeID = 2 Then
                'm_lCashListTypeID = 1 Payment
                'm_lCashListTypeID = 2 Receipt
                Return result
            End If

            m_lReturn = GetCashListDetails(v_lCashListID:=m_lCashlistID, r_vCashListRef:=vCashListRef, r_vListDate:=vListDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListDetails.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            m_lReturn = m_oBusiness.GetCashListType(v_lCashListTypeID:=m_lCashListTypeID, r_sCashListType:=sTaskDesc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetCashListType.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                Return result
            End If

            'Creating Account Business Object
            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:="ClientManager")
            oAccount = temp_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                Return result
            End If

            If Not Information.IsArray(m_vListData) Then
                Return result
            End If
            For iLoop As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                'Only create items for CashListItems wiht status Pending
                If gPMFunctions.NullToLong(m_vListData(ACCashListItemPaymentStatusID, iLoop)) = ACStatusPendingID Then

                    m_lCashListItemID = CInt(m_vListData(ACSubCashListItemID, iLoop))                  
                    sCashItemMediaRef = gPMFunctions.NullToString(m_vListData(ACSubMediaRef, iLoop))
                    sCashItemOurRef = gPMFunctions.NullToString(m_vListData(ACSubOurRef, iLoop))
                    sCashItemTheirRef = gPMFunctions.NullToString(m_vListData(ACSubTheirRef, iLoop))
                    cCashItemAmount = Math.Abs(gPMFunctions.NullToCurrency(m_vListData(ACSubAmount, iLoop)))

                    'Getting Account Short Code

                    m_lReturn = oAccount.GetDetails(vAccountID:=m_vListData(ACSubAccountID, iLoop))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")

                        Return result
                    End If

                    m_lReturn = oAccount.GetNext(vShortCode:=sAccountShortCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the Account business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                    End If

                    ReDim vKeyArray(2, 5)

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCashListItemID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lCashlistID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lCashListTypeID

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameActionKey

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = gACTLibrary.ACTApprove

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemMode

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = gACTLibrary.ACTUseListHidden

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClaimPaymentId

                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lClaimPaymentId

                    m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                        Return result
                    End If

                    If gPMFunctions.NullToString(vOptionValue) = "1" Then
                        'Get the user group for first step
                        m_lReturn = GetStepGroupCode(r_sGroupCode:=sUserGroup)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                            sUserGroup = "SLACS"
                        End If
                    Else
                        sUserGroup = "SLACS"
                    End If

                    sTaskDescComplete = New StringBuilder(sTaskDesc & " - Cash / Cheque" & New String(" "c, 1))
                    sTaskDescComplete.Append(" - Reference: " & sCashItemOurRef.Trim() & New String(" "c, 1))
                    'sTaskDesc = sTaskDesc & "List Date :" & Trim$(vListDate)
                    sTaskDescComplete.Append(" - The Amount: " & StringsHelper.Format(cCashItemAmount, "#,##0.00"))
                    'eck 130901 change for Tinny - pass in short code

                    m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=sAccountShortCode, v_sDescription:=sTaskDescComplete.ToString(), v_dtTaskDueDate:=DateTime.Today.AddDays(1).AddSeconds(-1), v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:=sUserGroup, v_vKeyArray:=vKeyArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process AddTaskToWorkManager.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager")
                        Return result
                    End If

                End If
            Next iLoop

            oAccount = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function DoAutoAllocation(ByRef r_lAccountId As Integer, ByRef r_lTransdetailid As Integer, ByRef r_vOSTransactions As Object, ByVal lWriteOffReasonID As Integer, ByVal cCurrencyWriteOff As Decimal, ByVal v_lCurExchangeRateGainLossReasonID As Integer, ByVal v_cCurGainLossAutoAllocationLimitAmount As Decimal) As Integer
        Dim result As Integer = 0
        Dim bACTAllocate As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DoAutoAllocation
        ' PURPOSE: Wraps the calls to PerformAllocation and the updating of the AllocationStaus on
        '          the the allocated CashListItems inside a transaction.
        ' AUTHOR: Paul Cunningham
        ' DATE: 03 December 2002, 12:04:41
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim oAllocate As bACTAllocate.Business
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Perform the auto allocation using the new auto allocate routine in the Allocate component
            Dim temp_oAllocate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAllocate, "bACTAllocate.Business", vInstanceManager:="ClientManager")
            oAllocate = temp_oAllocate

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.

                ' Display error stating the problem.
                ' Get description from the resource file

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If m_oBusiness.BeginTrans <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to begin a transaction", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation")

                Return result
            End If

            'Do the auto allocation

            If oAllocate.PerformAutoAllocation(r_lAccountId:=r_lAccountId, r_lTransDetailId:=r_lTransdetailid, v_vOSTransactions:=r_vOSTransactions, lWriteOffReasonID:=lWriteOffReasonID, cCurrencyWriteOff:=cCurrencyWriteOff, v_lCashListItemID:=CInt(m_vListData(ACSubCashListItemID, m_lSelectedRow)), v_lCurExchangeRateGainLossReasonID:=v_lCurExchangeRateGainLossReasonID, v_cCurGainLossAutoAllocationLimitAmount:=v_cCurGainLossAutoAllocationLimitAmount) <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusiness.RollbackTrans()

                oAllocate.Dispose()

                oAllocate = Nothing

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to perform the auto allocation", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation")

                Return result
            End If

            'Set the allocation status column to Allocated for allocated transactions

            If SetItemsAllocated(r_vAllocatedItems:=r_vOSTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusiness.RollbackTrans()

                Return result
            End If

            'Update the database

            If m_oBusiness.Update() <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusiness.RollbackTrans()

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation")
                Return result
            End If

            If m_oBusiness.CommitTrans <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oBusiness.RollbackTrans()

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to commit a transaction", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation")
                Return result
            End If

            oAllocate.Dispose()

            oAllocate = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DoAutoAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    m_oBusiness.RollbackTrans()

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function SetItemsAllocated(ByRef r_vAllocatedItems(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SetItemsAllocated
        ' PURPOSE: Update the AllocationStatus to Allocated for allocated items
        ' AUTHOR: Paul Cunningham
        ' DATE: 03 December 2002, 10:43:57
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const klACAllocatedTransId As Integer = 0 'indicates the column in the array that represents the TransDetailId
        Const klUserError As Integer = 32767

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If Information.IsArray(r_vAllocatedItems) Then

                'Loop through all the items in the allocated array and find the corresponding
                'item in the m_vListData array.  Update the business object if found
                For lAllocatedItemRow As Integer = r_vAllocatedItems.GetLowerBound(1) To r_vAllocatedItems.GetUpperBound(1)

                    For lListItemRow As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)

                        'Test for a match against the TransDetailIds

                        If Conversion.Val(CStr(r_vAllocatedItems(klACAllocatedTransId, lAllocatedItemRow))) = CDbl(m_vListData(ACSubTransdetailId, lListItemRow)) Then

                            'Do the update
                            m_vListData(ACSubRealAllocationID, lListItemRow) = gACTLibrary.ACTAllocationStatusAllocated

                            If DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lRow:=lListItemRow) <> gPMConstants.PMEReturnCode.PMTrue Then
                                'Raise the error
                                Throw New System.Exception(klUserError.ToString() + ", " + +", Failed to update the business.")
                            End If

                            Exit For
                        End If

                    Next lListItemRow

                Next lAllocatedItemRow

            End If

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SetItemsAllocated", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally


        End Try
        Return result
    End Function
    Private Function CheckAllPending(ByRef r_bAllIsPending As Boolean) As Integer

        Dim result As Integer = 0
        Dim sLookUpcode As String
        Dim bReceiptTypeIsInstalmentBased As Boolean
        Try
            Dim vOptionValue As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllPending")
                Return result
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" Then

                r_bAllIsPending = True

                ' Check each list item
                For iLoop1 As Integer = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                    If ToSafeInteger(m_vListData(ACvCashlistitemreceipttypeid, iLoop1)) <> 0 Then
                        m_lReturn = GetReceiptTypeDetails(v_lReceiptTypeId:=ToSafeInteger(m_vListData(ACvCashlistitemreceipttypeid, iLoop1)), r_sReceiptTypeAlias:=sLookUpcode, r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
                    End If
                    If bReceiptTypeIsInstalmentBased Then
                        r_bAllIsPending = False
                        Exit For
                    End If
                    If (m_vListData(ACCashListItemPaymentStatusID, iLoop1)) <> ACStatusPendingID Then
                        r_bAllIsPending = False
                        Exit For
                    End If
                Next iLoop1
            Else
                r_bAllIsPending = False
            End If

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAllPending Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllPending", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function GetStepGroupCode(ByRef r_sGroupCode As String) As Integer
        Dim result As Integer = 0
        Try

            Dim oStepAuthorization As bACTCashlistitem.StepAuthorization
            'RKS PN14253
            Dim sErrorMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oStepAuthorization As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization, "bACTCashlistitem.StepAuthorization", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oStepAuthorization = temp_oStepAuthorization

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode failed to Create StepAuthorization Class", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = oStepAuthorization.Initialise(g_sUserName$, g_sPassword$, g_iUserID%,
                                                    m_iCompanyID, g_iLanguageID%, g_iCurrencyID%,
                                                                        g_iLogLevel%, m_sCallingAppName$)
            'set the properties of the object.

            oStepAuthorization.PaymentType = ACPaymentsType

            m_lReturn = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=r_sGroupCode)

            'RKS PN14253
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                'Check for any error message return

                sErrorMessage = oStepAuthorization.ErrorMessage
                If sErrorMessage <> "" Then
                    MessageBox.Show(sErrorMessage, "Cash List Items", MessageBoxButtons.OK)

                    oStepAuthorization.ErrorMessage = ""
                End If
            End If

            'Display Error only if function call unsuccessful
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApproveCashListItem failed to Process User Authroization", vApp:=ACApp, vClass:=ACClass, vMethod:="ApproveCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            oStepAuthorization.Dispose()
            oStepAuthorization = Nothing

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStepGroupCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepGroupCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    'RKS PN14255 25/08/2004
    Private Function CreateUserAuthorities() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oUserAuthorities Is Nothing Then
                Dim temp_m_oUserAuthorities As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oUserAuthorities = temp_m_oUserAuthorities
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create bACTUserAuthorities.Business")
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUserAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'RKS PN14255 25/08/2004
    Private Function CreateCashListPost() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oCashListPost Is Nothing Then
                Dim temp_m_oCashListPost As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCashListPost, "bACTCashListPost.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oCashListPost = temp_m_oCashListPost
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create bACTCashListPost.Automated")
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateCashListPost failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCashListPost", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ''' <summary>
    ''' CheckWriteOff
    ''' </summary>
    ''' <param name="lAccountID"></param>
    ''' <param name="lCurrencyID"></param>
    ''' <param name="cAmount"></param>
    ''' <param name="lMode"></param>
    ''' <param name="r_lStatus"></param>
    ''' <param name="r_lWriteOffReasonID"></param>
    ''' <param name="r_cWriteOffAmount"></param>
    ''' <param name="r_bPartPayment"></param>
    ''' <param name="r_vOSTransactions"></param>
    ''' <returns></returns>
    ''' <remarks>Checks the users write off ability and asks them whether they wish to write-off a difference. For standard cashlistitems (outside of NB/ MTA, the write off can only be a currency one for the moment</remarks>
    Private Function CheckWriteOff(ByVal lAccountID As Integer, ByVal lCurrencyID As Integer, ByVal cAmount As Decimal, ByVal lMode As Integer, ByRef r_lStatus As Integer, ByRef r_lWriteOffReasonID As Integer, ByRef r_cWriteOffAmount As Decimal, ByRef r_bPartPayment As Boolean, Optional ByRef r_vOSTransactions(,) As Object = Nothing) As Integer

        Const kMethodName As String = "CheckWriteOff"
        Dim nResult As Integer = 0
        Dim vWriteOffValid As Boolean
        Dim cBaseBalance As Decimal
        Dim cAuthorityAmount As Decimal
        Dim sCurrency As String = ""
        Dim cAccountBalance As Decimal
        Dim nAccountCount As Integer
        Dim cWriteOffAmount As Decimal 'AR20050203 - PN18491
        Dim bCheckUserWriteOff As Boolean 'AR20050203 - PN18491

        Dim bNormalWriteOff As Boolean
        Dim bCurrencyWriteOff As Boolean
        Dim nRow As Integer
        Dim nStart As Integer
        Dim nEnd As Integer
        Dim nTransDetailID As Integer
        Dim frmWriteOff As frmWriteOffReason
        Dim vOSTransactions(,) As Object
        Dim bHasClaimTransaction As Boolean
        Dim lCashListCount As Long
        Dim bIsOtherTransaction As Boolean
        Dim sOptionValue As String
        Dim bIsSingleCashListItemAllocation As Boolean

        Try
            nResult = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("CheckWriteOff", "GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed", vbObjectError)
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            If lMode = cAUTO_ALLOCATE_BUTTON Then
                'Get the allocation status of the account

                m_lReturn = m_oAccount.GetAccountOSTransactions(vAccount_id:=lAccountID, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=cBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=cAccountBalance, r_iAccountCount:=nAccountCount, r_iDNGIND:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        nResult = gPMConstants.PMEReturnCode.PMTrue
                        r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    Else
                        Return m_lReturn
                    End If
                End If

                If bIsSingleCashListItemAllocation = True Then
                    nStart = LBound(vOSTransactions, 2)
                    nEnd = UBound(vOSTransactions, 2)
                    For nRow = nStart To nEnd
                        If UCase(Strings.Left(vOSTransactions(6, nRow), 3)) = "SRP" Or UCase(Strings.Left(vOSTransactions(6, nRow), 3)) = "SPY" Then
                            lCashListCount = lCashListCount + 1
                        Else
                            bIsOtherTransaction = True
                        End If
                    Next nRow
                End If


                If ((bIsOtherTransaction = True) And (lCashListCount > 1)) And bIsSingleCashListItemAllocation = True Then

                    'MsgBox("You can only allocate 1 SRP or 1 SPY in a single allocation with other transaction types ", vbOKOnly)
                    CheckWriteOff = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                cWriteOffAmount = cBaseBalance

            ElseIf lMode = cAUTO_ALLOCATE_AUTOMATIC Then

                CreateCashListPost()

                m_lReturn = m_oCashListPost.GetTransactionsForAllocatedCashListItem(lAccountID:=lAccountID, lInsuranceFileCnt:=m_lInsuranceFileCnt, sDocumentRef:=m_sDocumentRef, r_vResultArray:=vOSTransactions, v_bUseDocumentRef:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                If IsArray(vOSTransactions) Then

                    nStart = vOSTransactions.GetLowerBound(1)

                    nEnd = vOSTransactions.GetUpperBound(1)

                    cBaseBalance = 0
                    For nRow = nStart To nEnd

                        cBaseBalance += CDbl(vOSTransactions(1, nRow))
                    Next nRow

                    cWriteOffAmount = cBaseBalance - cAmount
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            r_vOSTransactions = vOSTransactions

            If lMode = cAUTO_ALLOCATE_BUTTON Then

                If cBaseBalance <> 0 And cAccountBalance = 0 And nAccountCount = 1 Then
                    bCheckUserWriteOff = True
                ElseIf cBaseBalance <> 0 Then
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                Else
                    If MessageBox.Show("The Account will balance to zero. Do you want to Auto Allocate?", "Auto Allocate", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                        r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    Else
                        r_lStatus = gPMConstants.PMEReturnCode.PMTrue
                    End If
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

            ElseIf lMode = cAUTO_ALLOCATE_AUTOMATIC Then

                If cWriteOffAmount = 0 Then
                    'Nothing to do
                    r_bPartPayment = False
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                Else
                    r_bPartPayment = True
                    bCheckUserWriteOff = True
                End If

            End If

            If bCheckUserWriteOff Then

                'The allocation can go ahead with a currency write off, if the user has rights
                CreateUserAuthorities()

                ' Validate the user's write off limit

                m_lReturn = m_oUserAuthorities.ValidateAmounts(v_iCurrencyID:=CInt(vOSTransactions(2, 0)), v_cAmount:=cWriteOffAmount, v_lCompanyID:=g_iSourceID, r_vWriteOffValid:=vWriteOffValid, r_cAuthorityAmount:=cAuthorityAmount, r_sCurrency:=sCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Write Off amount for user.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return m_lReturn
                End If

                If Not vWriteOffValid Then
                    'user doesn't have rights to auto-write off
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    r_cWriteOffAmount = cWriteOffAmount
                    Return nResult
                End If

                If lMode = cAUTO_ALLOCATE_AUTOMATIC Then

                    'AR20050209 - PN18582
                    bNormalWriteOff = Math.Abs(cWriteOffAmount) <= cAuthorityAmount
                    bCurrencyWriteOff = False

                ElseIf lMode = cAUTO_ALLOCATE_BUTTON Then

                    bNormalWriteOff = False
                    bCurrencyWriteOff = True

                End If

                If bNormalWriteOff Or bCurrencyWriteOff Then

                    'REASON FOR THIS DESIGN - THERE IS A LIMITATION HERE
                    'Now we need to select the first suitable candidate for writing off against.
                    'Sirius only allows an allocation write off against a SINGLE transactions within
                    'the allocation. If we cannot find one single one to write-off against then we
                    'cannot auto-allocate here. Essentially it means that the write-off needs to be split
                    'between a number of transactions and so needs to be handled manually by the user.

                    'Loop through the transaction array
                    If bCurrencyWriteOff Then

                        nStart = vOSTransactions.GetLowerBound(1)

                        nEnd = vOSTransactions.GetUpperBound(1)
                        For nRow = nStart To nEnd

                            If (CDbl(vOSTransactions(1, nRow)) < 0 And cWriteOffAmount < 0 And Math.Abs(CDbl(vOSTransactions(1, nRow))) > Math.Abs(cWriteOffAmount)) Or (CDbl(vOSTransactions(1, nRow)) > 0 And cWriteOffAmount > 0 And Math.Abs(CDbl(vOSTransactions(1, nRow))) > Math.Abs(cWriteOffAmount)) Then
                                Exit For
                            End If
                        Next nRow

                        If nRow <= nEnd Then

                            nTransDetailID = CInt(vOSTransactions(0, nRow))
                        Else
                            'We cannot find one so quit quietly here
                            nResult = gPMConstants.PMEReturnCode.PMTrue
                            r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                            Return nResult
                        End If
                    End If

                    'Let the user decide if they want to go ahead
                    frmWriteOff = New frmWriteOffReason()

                    If bCurrencyWriteOff Then

                        frmWriteOff.message = "The Account will balance to zero with a currency " &
                                              "write-off of " &
                                                  cWriteOffAmount.ToString("N2") & " " & CStr(vOSTransactions(5, 0)).Trim() & "." &
                                          Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Auto-Allocate and Write-Off Difference?"
                    Else

                        frmWriteOff.message = "The Posting will balance to zero with a " &
                                              "write-off of " &
                                                  cWriteOffAmount.ToString("N2") & " " & CStr(vOSTransactions(5, 0)).Trim() & "." &
                                          Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Auto-Allocate and Write-Off Difference?"
                    End If

                    frmWriteOff.ShowDialog()
                    If frmWriteOff.ReturnValue = gPMConstants.PMEReturnCode.PMCancel Then
                        nResult = gPMConstants.PMEReturnCode.PMTrue
                        'AR20050208 - PN18582 Set status to cancel if user chosen not to write-off
                        r_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        Return nResult
                    Else
                        r_lWriteOffReasonID = frmWriteOff.WriteOffReasonID
                        r_cWriteOffAmount = cWriteOffAmount
                    End If
                    frmWriteOff.Close()
                    frmWriteOff = Nothing
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMTrue

                Else
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                nResult = gPMConstants.PMEReturnCode.PMTrue
                r_lStatus = gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " function failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return nResult

    End Function
    ''' <summary>
    ''' CheckWriteOffAndExchangeRateGainLoss
    ''' </summary>
    ''' <param name="lAccountID"></param>
    ''' <param name="lCurrencyID"></param>
    ''' <param name="cAmount"></param>
    ''' <param name="dCurrencyBaseXRate"></param>
    ''' <param name="cCurrencyBaseAmount"></param>
    ''' <param name="lMode"></param>
    ''' <param name="r_lStatus"></param>
    ''' <param name="r_lWriteOffReasonID"></param>
    ''' <param name="r_cWriteOffAmount"></param>
    ''' <param name="r_lCurExchangeRateGainLossReasonID"></param>
    ''' <param name="r_cCurGainLossAutoAllocationLimitAmount"></param>
    ''' <param name="r_bPartPayment"></param>
    ''' <param name="r_vOSTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckWriteOffAndExchangeRateGainLoss(ByVal lAccountID As Integer, ByVal lCurrencyID As Integer, ByVal cAmount As Decimal, ByVal dCurrencyBaseXRate As Double, ByVal cCurrencyBaseAmount As Decimal, ByVal lMode As Integer, ByRef r_lStatus As Integer, ByRef r_lWriteOffReasonID As Integer, ByRef r_cWriteOffAmount As Decimal, ByRef r_lCurExchangeRateGainLossReasonID As Integer, ByRef r_cCurGainLossAutoAllocationLimitAmount As Decimal, ByRef r_bPartPayment As Boolean, Optional ByRef r_vOSTransactions As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "CheckWriteOffAndExchangeRateGainLoss"
        Const kCurGainLossAllocationLimit As Integer = 5060

        Dim vWriteOffValid As Boolean
        Dim cBaseBalance As Decimal
        Dim cAuthorityAmount As Decimal
        Dim sCurrency As String = ""
        Dim cAccountBalance As Decimal
        Dim nAccountCount As Integer
        Dim cWriteOffAmount As Decimal
        Dim nTransDetailID As Integer
        Dim vOSTransactions As Object
        Dim cCurGainLossAutoAlloctateLimit As Decimal
        Dim nBaseCurrencyID As Integer
        Dim sCurGainLossAllocateLimit As String = ""
        Dim lRow As Long
        Dim lStart As Long
        Dim lEnd As Long
        Dim bHasClaimTransaction As Boolean
        Dim lCashListCount As Long
        Dim bIsOtherTransaction As Boolean
        Dim sOptionValue As String
        Dim bIsSingleCashListItemAllocation As Boolean

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("CheckWriteOffAndExchangeRateGainLoss", "GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed", vbObjectError)
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            If lMode = cAUTO_ALLOCATE_BUTTON Then
                'Get the allocation status of the account

                m_lReturn = m_oAccount.GetAccountOSTransactions(vAccount_id:=lAccountID, vOSTransactions:=vOSTransactions, r_cAccountBaseBalance:=cBaseBalance, r_iBaseCount:=0, r_cAccountBalance:=cAccountBalance, r_iAccountCount:=nAccountCount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        nResult = gPMConstants.PMEReturnCode.PMTrue
                        r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    Else
                        nResult = m_lReturn
                        Return nResult
                    End If
                End If

                If bIsSingleCashListItemAllocation = True Then
                    lStart = LBound(vOSTransactions, 2)
                    lEnd = UBound(vOSTransactions, 2)
                    For lRow = lStart To lEnd
                        If UCase(Strings.Left(vOSTransactions(6, lRow), 3)) = "SRP" Or UCase(Strings.Left(vOSTransactions(6, lRow), 3)) = "SPY" Then
                            lCashListCount = lCashListCount + 1
                        Else
                            bIsOtherTransaction = True
                        End If
                    Next lRow
                End If
                'Check whether we have only SRPs
                Dim bHasContraEntriesForAllocation As Boolean = False
                If chkSplitReceipt.Checked = True And m_sCallingAppName = "SplitReceiptsFromFindTransaction" Then
                    If UBound(vOSTransactions, 2) = 0 Then
                        lStart = LBound(vOSTransactions, 2)
                        lEnd = UBound(vOSTransactions, 2)
                        For lRow = lStart To lEnd
                            If ToSafeDecimal(vOSTransactions(3, lRow)) > 0 Then
                                bHasContraEntriesForAllocation = True
                            End If
                        Next
                    End If
                End If

                If chkSplitReceipt.Checked = True And bHasContraEntriesForAllocation = False Then
                    nResult = gPMConstants.PMEReturnCode.PMTrue
                    Return nResult
                End If


                If ((bIsOtherTransaction = True) And
                   (lCashListCount > 1)) And bIsSingleCashListItemAllocation = True Then

                    'MsgBox("You can only allocate 1 SRP or 1 SPY in a single allocation with other transaction types ", vbOKOnly)
                    CheckWriteOffAndExchangeRateGainLoss = gPMConstants.PMEReturnCode.PMTrue
                    r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                r_cWriteOffAmount = cBaseBalance
                'r_cCurGainLossAutoAllocationLimitAmount = cAccountBalance

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    Return nResult
                End If

                r_vOSTransactions = vOSTransactions

                'Case 1. When OS bal is zero and a transaction can be allocated fully - no hesitation
                If cAccountBalance = 0 Then
                    r_lStatus = gPMConstants.PMEReturnCode.PMTrue
                    Return nResult
                End If

                'Case 2. When Transaction is in Base Currency and (- +)Bal is with in user's write off limit
                'Check the transaction currency, is it base? if yes, then fetch the write off limit and go ahead

                m_lReturn = m_oBusiness.GetBranchBaseCurrency(v_lSourceID:=m_iCompanyID, v_lBaseCurrencyID:=nBaseCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetReceiptTypeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If nBaseCurrencyID = m_iCurrencyID Then
                    'The allocation can go ahead with a currency write off, if the user has rights
                    CreateUserAuthorities()
                    ' Validate the user's write off limit

                    m_lReturn = m_oUserAuthorities.ValidateAmounts(v_iCurrencyID:=vOSTransactions(2, 0), v_cAmount:=r_cWriteOffAmount, v_lCompanyID:=g_iSourceID, r_vWriteOffValid:=vWriteOffValid, r_cAuthorityAmount:=cAuthorityAmount, r_sCurrency:=sCurrency)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Fail to get writeoff limit for the user", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Not vWriteOffValid Then
                        'user doesn't have rights to auto-write off
                        nResult = gPMConstants.PMEReturnCode.PMTrue
                        r_lStatus = gPMConstants.PMEReturnCode.PMFalse
                        'r_cWriteOffAmount = cWriteOffAmount
                        Return nResult
                    End If

                    If Math.Abs(r_cWriteOffAmount) <= cAuthorityAmount Then
                        r_lStatus = gPMConstants.PMEReturnCode.PMTrue
                        'Set the Currency Gain Loss Alloction Amoun to zero As only one can adjust at time to the
                        ' account
                        r_cCurGainLossAutoAllocationLimitAmount = 0
                        Return nResult
                    End If
                Else
                    'When transaction is other than base currency
                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kCurGainLossAllocationLimit, r_sOptionValue:=sCurGainLossAllocateLimit)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Fail to Get System Option", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If gPMFunctions.ToSafeDouble(sCurGainLossAllocateLimit) = 0 Then
                        Return nResult
                    End If

                    ' Here we apply Currency Gain Loss Allocation Limit percentage on the transaction base amount
                    ' and check this with the o/s balance in base currency
                    If cBaseBalance <> 0 Then
                        If Math.Abs(cBaseBalance) <= Math.Abs((cCurrencyBaseAmount * gPMFunctions.ToSafeDouble(sCurGainLossAllocateLimit)) / 100) Then
                            'Set write off amount to zero as only one can be adujsted at a time to the account
                            r_cCurGainLossAutoAllocationLimitAmount = cBaseBalance
                            r_cWriteOffAmount = 0
                            r_lStatus = gPMConstants.PMEReturnCode.PMTrue
                        End If
                    End If

                End If

            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " function failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return nResult

    End Function

    Private Sub SetAllocateButtonVisibility()
        ' If we have come via Client Manager... PN24911
        If m_sCashListRoadmap = "CLIENTCASH" Then

            'If Client Manager Security System Option has been turned on AND
            'the user has NOT been granted allocation priviliedge then hide the Allocate button
            If Not (g_bCanPerformAllocationsAuthority) Then
                cmdAllocate.Visible = False
                Me.chkAutoAllocateIfAble.Enabled = False
            Else
                cmdAllocate.Visible = True
                Me.chkAutoAllocateIfAble.Enabled = True
            End If
        Else
            cmdAllocate.Visible = True
            Me.chkAutoAllocateIfAble.Enabled = True
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: GetReceiptTypeDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 07-02-2006 : Receipt Type Maintenance
    ' ***************************************************************** '
    Public Function GetReceiptTypeDetails(ByVal v_lReceiptTypeId As Integer,
                                          Optional ByRef r_sReceiptTypeDescription As String = "",
                                          Optional ByRef r_sReceiptTypeCode As String = "",
                                          Optional ByRef r_bIsInstalmentsBased As Boolean = False,
                                          Optional ByRef r_sReceiptTypeAlias As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptTypeDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vReceiptTypeDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the cashlistitem receipt type details

            lReturn = m_oBusiness.GetReceiptTypeDetails(v_lReceiptTypeId, vReceiptTypeDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetReceiptTypeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vReceiptTypeDetails) Then
                gPMFunctions.RaiseError(kMethodName, "GetReceiptTypeDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get receipt type details

            r_sReceiptTypeCode = CStr(vReceiptTypeDetails(kReceiptTypeCode, 0)).Trim()

            r_sReceiptTypeDescription = CStr(vReceiptTypeDetails(kReceiptTypeDescription, 0)).Trim()
            r_bIsInstalmentsBased = gPMFunctions.ToSafeBoolean(vReceiptTypeDetails(kReceiptTypeIsInstalmentBased, 0), False)

            ' slightly dodgy but to avoid doing some of the work....
            If r_bIsInstalmentsBased Then
                r_sReceiptTypeAlias = ACInstalmentReceiptType
            Else
                r_sReceiptTypeAlias = ACStandardReceiptType
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function UpdateWriteOffDocumentRef() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateWriteOffDocumentRef"
        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            Dim vResultArray(,) As Object
            Dim sDocumentRef As String = ""
            Dim lNumberRangeID, lNumber As Integer
            Dim sRangeCode As String = ""
            Dim lDocumentType, lDocumentID As Integer
            Dim sGroupCode As String = ""
            Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign

            m_lReturn = m_oBusiness.GetTransDetailsFromBatch(v_lBatchID:=m_lBatchID, r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetTransactionsFromBatchFailed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vResultArray) Then

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)
                    ' Create the autonumber object using component services

                    lDocumentType = gACTLibrary.ACTDocTypeWriteOff
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14

                    ' Get the number range

                    m_lReturn = m_oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetNumberRange Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ''Generate the next number

                    m_lReturn = m_oPMAutoNumber.GenerateNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=g_iUserID, v_iCompanyID:=g_iSourceID, r_lNumber:=lNumber)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GenerateNumber Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' Format the number
                    sDocumentRef = StringsHelper.Format(lNumber, "00000000")
                    sDocumentRef = sRangeCode & sDocumentRef

                    m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=DateTime.Now, v_sComment:="WRITEOFF", r_vDocumentID:=lDocumentID, r_vDocSourceID:=g_iSourceID)

                    m_lReturn = m_oBusiness.UpdateWriteOffDocumentRef(v_lOldDocumentId:=vResultArray(1, iCount), v_lNewDocumentId:=lDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateWriteOffDocumentRef Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally



        End Try
        Return result
    End Function

    Private Sub frmList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

    'Fix for Bug 582
    Private Sub lvwListDetails_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwListDetails.SelectedIndexChanged
        ' This is event fired before the ItemClick event (if mousedown on an item)
        If lvwListDetails.SelectedItems.Count > 0 Then
            If (lvwListDetails.FocusedItem) Is Nothing Then
                lvwListDetails.Items(0).Focused = True
            End If
            m_lReturn = EnableButtons()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
        End If
    End Sub

    Private Sub lvwListDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwListDetails.Click
        ' Single ItemClick event for the List details.

        Try

            m_lSelectedRow = Convert.ToInt32(Convert.ToString(lvwListDetails.FocusedItem.Tag))
            '01/05/2003 - PWC - ENDVR00000850
            Select Case m_iCashListStatusId
                Case gACTLibrary.ACTCashListStatusClosed, gACTLibrary.ACTCashListStatusInBanking
                    'Disable the form
                    lvwListDetails.Enabled = True
                    m_lReturn = EnableButtons()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                        Exit Sub
                    End If
                    Exit Sub
            End Select

            ' If its part of the cash list
            'eck130700 added extra if to check if transaction have been posted
            m_lReturn = EnableButtons()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ItemClick event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwListDetails_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function RefreshListData() As Integer
        Dim kMethodName As String = "RefreshListData"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim iRow As Integer
            Dim iItem As Integer
            Dim iLeadRow As Integer
            Dim iCashListItemID As Integer

            ' Find the Lead Row
            For iRow = 0 To m_vListData.GetUpperBound(1)
                If Convert.ToDouble(m_vListData(83, iRow)) = True Then
                    iLeadRow = iRow
                    Exit For
                End If
            Next

            For iRow = m_vListData.GetLowerBound(0) To m_vListData.GetUpperBound(0)

                ' Row count exists in Enum,
                ' Then Split value = Lead Value for all splits

                If [Enum].IsDefined(GetType(ENMatchSplitLead), iRow) Then
                    For iItem = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)
                        If iItem <> iLeadRow Then
                            m_vListData(iRow, iItem) = m_vListData(iRow, iLeadRow)
                        End If
                    Next
                End If

            Next

            'Case when Posted cash list has to be treated as  Unallocated.
            'Lead will always be posted when coming through FindTransaction
            'For the Post button to be enabled in this case original posted amount[Lead] has to be reduced and a split receipt added.
            If chkSplitReceipt.Checked = True And m_sCallingAppName = "SplitReceiptsFromFindTransaction" Then

                For iRow = m_vListData.GetLowerBound(1) To m_vListData.GetUpperBound(1)

                    'Get the Posting status, if it's already posted set TransDetailID = 0
                    iCashListItemID = m_vListData(iRow, 0)

                    'Check only for existing cash list items
                    If iCashListItemID <> 0 Then

                        m_vListData(ACSubTransdetailId, iRow) = 0

                    End If

                Next

            End If

            iLeadRow = 0
            'Find The Lead row
            For iRow = 0 To lvwListDetails.Items.Count - 1
                If lvwListDetails.Items(iRow).SubItems(6).Text = "Lead" Then
                    iLeadRow = iRow
                    Exit For
                End If
            Next

            For iRow = 0 To lvwListDetails.Items.Count - 1
                If iRow <> iLeadRow Then
                    lvwListDetails.Items(iRow).SubItems(1).Text = lvwListDetails.Items(iLeadRow).SubItems(1).Text
                    lvwListDetails.Items(iRow).SubItems(0).Text = lvwListDetails.Items(iLeadRow).SubItems(0).Text
                End If
            Next

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try

        Return iResult

    End Function

    Public Function SetSplitReceiptDefaults() As Integer
        Dim KMethodName As String = "SetSplitReceiptDefaults"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim iLeadRow As Integer
            Dim iRow As Integer
            m_dSplitReceiptRunningTotal = CDec(0.0)

            If Not m_vListData Is Nothing Then

                'Find the Lead Row
                For iRow = 0 To m_vListData.GetUpperBound(1)
                    If m_vListData(83, iRow) = True Then
                        iLeadRow = iRow
                    End If
                    m_dSplitReceiptRunningTotal = m_dSplitReceiptRunningTotal + m_vListData(ACSubAmount, iRow)
                Next

                m_iMediaTypeID = m_vListData(ENMatchSplitLead.MediaTypeID, iLeadRow)
                m_sCCNameLead = m_vListData(ENMatchSplitLead.CCName, iLeadRow)
                m_sCCNumberLead = m_vListData(ENMatchSplitLead.CCNumber, iLeadRow)
                m_sCCExpiryDateLead = m_vListData(ENMatchSplitLead.CCExpiryDate, iLeadRow)
                m_sCCissueLead = m_vListData(ENMatchSplitLead.CCIssue, iLeadRow)
                m_sCCpinLead = m_vListData(ENMatchSplitLead.CCPin, iLeadRow)
                m_sCCstartdateLead = m_vListData(ENMatchSplitLead.CCStartDate, iLeadRow)
                m_sCCmanualauthcodeLead = m_vListData(ENMatchSplitLead.CCManualAuthCode, iLeadRow)
                m_vListData(ENMatchSplitLead.CCCustomer, iLeadRow) = m_sCCcustomerLead
                m_sMediaRefLead = m_vListData(ENMatchSplitLead.MediaRef, iLeadRow)

            Else
                m_dSplitTotal = 0.0
                m_sMediaRefLead = ""
            End If

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=KMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try

        Return iResult

    End Function

    Private Function SetIsLeadAccount() As Integer
        Const KMethodName As String = "SetIsLeadAccount"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim iRow As Integer = 0
            m_dSplitReceiptRunningTotal = CDec(0.0)

            If lvwListDetails.SelectedItems.Item(iRow).SubItems(6).Text = "Lead" Or lvwListDetails.SelectedItems.Item(iRow).SubItems(6).Text = "" Then
                m_bIsLeadAccount = True
            Else
                m_bIsLeadAccount = False
            End If

            For iRow = 0 To m_vListData.GetUpperBound(1)
                If Convert.ToInt32(Convert.ToString(lvwListDetails.FocusedItem.Tag)) <> iRow Then
                    m_dSplitReceiptRunningTotal = m_dSplitReceiptRunningTotal + m_vListData(ACSubAmount, iRow)
                End If

            Next

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=KMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try

        Return iResult

    End Function

    Private Function Validate(ByVal v_sCode As String, Optional ByVal v_iTaskType As Integer = 0) As Integer
        Dim KMethodName As String = "Validate"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim bRowHighlighted As Boolean = False
        Try
            Select Case (v_sCode)
                Case "SplitReceipt"

                    'pnlTotal can only be zero if there are no cash list item
                    'm_cOriginalPNLTotal default value is 0.0.
                    If m_cOriginalPNLTotal = CDec(0.0) Then
                        m_cOriginalPNLTotal = pnlTotal.Text
                    End If

                    If chkSplitReceipt.Checked = True And m_bIsLeadAccount And lvwListDetails.Items.Count > 1 Then
                        lvwListDetails.MultiSelect = True
                        For lRow As Integer = 0 To lvwListDetails.Items.Count - 1
                            If gPMFunctions.ToSafeDecimal(lvwListDetails.Items(lRow).SubItems(2).Text) > gPMFunctions.ToSafeDecimal(m_dSplitTotal) Then
                                lvwListDetails.Items.Item(lRow).Selected = True
                                bRowHighlighted = True
                            End If
                        Next
                        lvwListDetails.MultiSelect = False
                    End If

                    If chkSplitReceipt.Checked Then

                        If bRowHighlighted Then
                            MsgBox("Please amend the amounts for the accounts higlighted so that they do not exceed the total amount.", MsgBoxStyle.OkOnly, "Cash List Items")
                        End If

                        ' 1st Condition : Msg should be displayed based on this condition
                        ' 2nd Condition : Removed and then added a lead row
                        ' 3rd Condition : Similar to a 2nd, we are amending a LeadAccount item or removing & then adding a new Lead entry
                        ' 4th Condition : Ensures that there is atleast 1 Split and 1 Lead entry.
                        If (m_cTotalAmount <> CDec(pnlTotal.Text)) And (v_iTaskType = 1) And m_bIsLeadAccount And lvwListDetails.Items.Count > 1 Then
                            MsgBox("Please review the Amounts specified for the Accounts so that the sum of the Amounts equals Total.", MsgBoxStyle.OkOnly, "Cash List Items")
                        End If

                        m_cOriginalPNLTotal = CDec(pnlTotal.Text)
                    End If

            End Select

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=KMethodName, r_lFunctionReturn:=iResult, excep:=ex)
        End Try

        Return iResult

    End Function

    Private Function ReverseCashList(ByVal v_lDocumentId As Integer) As Integer
        Const kMethodName As String = "ReverseCashList"
        Dim lResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oDocumentReversal As bACTDocumentReversal.Business

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            If oDocumentReversal Is Nothing Then
                Dim temp_oDocumentReversal As Object

                m_lReturn = g_oObjectManager.GetInstance(temp_oDocumentReversal, "bACTDocumentReversal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oDocumentReversal = temp_oDocumentReversal

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    lResult = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'bACTDocumentReversal.Business'", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return lResult
                End If

            End If

            'lSelectedItem = Convert.ToString(lvwSearchResults.Items.Item(lvwSearchResults.FocusedItem.Index).Tag)

            oDocumentReversal.DocumentId = v_lDocumentId

            m_lReturn = oDocumentReversal.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lResult = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("oDocumentReversal.Start", "Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch excep As Exception

            lResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lResult, excep:=excep)

        Finally

            If Not (oDocumentReversal Is Nothing) Then
                oDocumentReversal.Dispose()
                oDocumentReversal = Nothing
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try

        Return lResult

    End Function

    Private Sub frmList_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        If cmdAdd.Enabled Then
            cmdAdd.Focus()
        End If
    End Sub
    Private Function UpdateCashListItem(ByVal v_lCashListItemId As Integer) As Integer
        Const kMethodName As String = "UpdateCashListItem"
        Dim lResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oInstalments As bSIRPFInstalments.Business

        Dim temp_oInstalments As Object

        Try

            If m_lFirstTransdetailID > 0 Then

                lResult = g_oObjectManager.GetInstance(temp_oInstalments, "bSIRPFInstalments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oInstalments = temp_oInstalments

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    lResult = gPMConstants.PMEReturnCode.PMError
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'bSIRPFInstalments.Business'", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return lResult
                End If

                lResult = CType(oInstalments.UpdateCashListItemRecord(m_lFirstTransdetailID, v_lCashListItemId), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
                m_lFirstTransdetailID = 0
            End If
        Catch excep As Exception

            lResult = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lResult)

        Finally

            If Not (oInstalments Is Nothing) Then
                oInstalments = Nothing
            End If
        End Try
        Return lResult
    End Function

    Public Function InstalmentsAlreadyPosted(ByVal lIndex As Integer) As Integer
        Dim result As Integer = 0

        Dim oInstalments As bSIRPFInstalments.Business
        Dim vInstalmentArray(,) As Object
        Dim vInstalmentIDArray(,) As Object
        Dim lIDArrayCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            vInstalmentArray = m_vListData(ACInstalmentArray, lIndex)

            Dim temp_oInstalments As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInstalments, "bSIRPFInstalments.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oInstalments = temp_oInstalments

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oInstalments = Nothing
                Return result
            End If

            lIDArrayCount = 0

            ' To support partial payments.
            ReDim vInstalmentIDArray(0, 0)
            'loop through the instalments that are paid by the current receipt

            For lCount As Integer = 0 To vInstalmentArray.GetUpperBound(1)

                If gPMFunctions.ToSafeInteger(vInstalmentArray(ACInstalmentFlagElement, lCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    'increase size of array by one (unless first time through)
                    ReDim Preserve vInstalmentIDArray(0, lIDArrayCount)

                    m_lReturn = oInstalments.IsInstalmentsPosted(gPMFunctions.ToSafeInteger(vInstalmentArray(ACInstalmentsID, lCount)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'increment count by one
                    lIDArrayCount += 1
                End If
            Next

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception


            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="InstalmentsAlreadyPosted", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            End Select
        End Try
        Return result
    End Function
End Class
