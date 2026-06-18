Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports PMLookupControl

Imports SharedFiles

<System.Runtime.InteropServices.ProgId("uctPMUPolicyControl_NET.uctPMUPolicyControl")>
Partial Public Class uctPMUPolicyControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event EventRaisedChange()
    Public Event FromEventChange()
    Public Event BorderStyleChange()
    Public Event BackStyleChange()
    Public Event FontChange()
    Public Event EnabledChange()
    Public Event ForeColorChange()
    Public Event BackColorChange()
    Public Event BusinessTypeIdChange()
    Public Event ProductIdChange()
    Public Event RiskGroupIdChange()
    Public Event RiskCodeIdChange()
    Public Event SourceIdChange()
    Public Event PolicyTypeIdChange()
    Public Event PartyCntChange()
    Public Event InsuranceFolderCntChange()
    Public Event InsuranceFileCntChange()
    Public Event EffectiveDateChange()
    Public Event TransactionTypeChange()
    Public Event ProcessModeChange()
    Public Event NavigateChange()
    Public Event TaskChange()
    Public Event StatusChange()
    Public Event CallingAppNameChange()

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
    '               MEvans : 03-12-2004 : PN17232 -
    '                   show multi currency dialogue for MTA, MTC, MTR
    '               VB 04/04/2005 PN44 : Commission button disabled for "direct" business only.
    '                       Reinsurer button hidden for all business types at policy level.
    '                       S4I doesn�t support Reinsurance at policy level.
    '               VB 19/04/2005 PN19896: In NB it displays only active currency. And for
    '                       MTA's and revisiting Quotes for that it displays the
    '                       deleted currency also for selection with '(Deleted)' suffix.
    '               PN 21210: BusinessTypeChange Event added
    '               RKS 16/09/2005 Policy Discount
    ' ***************************************************************** '


#Region "Private Constants"
    Private Const ACClass As String = "uctPMUPolicyControl"
    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_InsuranceFileCnt As Integer = 0
#End Region

#Region "Private Variables"
    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As BorderStyle
    Dim m_InsuranceFileCnt As Integer
#End Region

    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)

    ' Object parameter members.
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

    Private m_oBusinessListRisk As bSIRListRisks.Business
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAddressCount As Integer

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the system Business object.

    Private m_oSystemBusiness As bSIRInsuranceFileSystem.Business

    ' Declare an instance of the folder Business object.

    Private m_oFolderBusiness As bSIRInsuranceFolder.Business

    ' Declare an instance of the Lock object.

    Private m_oPMLock As bPMLock.User

    ' Declare an instance of the Cover Note Business object.

    Private m_oCoverNoteBusiness As bSIRCoverNote.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the fees interface.
    Private m_oFees As Object

    ' Declare an instance of the policy wording interface.
    Private m_oPolicyWording As Object

    ' Declare an instance of the shares interface.
    Private m_oShares As Object

    ' Declare an instance of the IPT interface.
    Private m_oIPT As Object

    ' Declare an instance of the Insurer Rate interface.
    Private m_oInsurerRate As Object

    Private m_oPolicyNumbering As Object

    ' Party

    Private m_oPartyAG As bSIRPartyAG.Business
    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'Policy Numbering
    Dim m_oPolicyNumber As Object

    ' PW311002 - Declare an instance of the Policy Business object.
    Private m_oBusinessPolicy As Object

    Private m_oUserAuthorities As Object  'bACTUserAuthorities.Business

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Store last selected currency id
    Private m_lCurrencyID As Integer
    Private m_bAgentEditable As Boolean
    ' constants for above
    Private Const CHANGECURRENCYTRUE As Integer = 1
    Private Const CHANGECURRENCYFALSE As Integer = 0

    ' Stores the details from the business object.

    Private Const vbScrollBars As String = "0x80000000"
    Private Const vbDesktop As String = "0x80000001"
    Private Const vbActiveTitleBar As String = "0x80000002"
    Private Const vbInactiveTitleBar As String = "0x80000003"
    Private Const vbMenuBar As String = "0x80000004"
    Private Const vbWindowBackground As String = "H00FFFFC0"
    Private Const vbWindowFrame As String = "0x80000006"
    Private Const vbMenuText As String = "0x80000007"
    Private Const vbWindowText As String = "0x80000008"
    Private Const vbTitleBarText As String = "0x80000009"
    Private Const vbActiveBorder As String = "0x8000000A"
    Private Const vbInactiveBorder As String = "0x8000000B"
    Private Const vbApplicationWorkspace As String = "0x8000000C"
    Private Const vbHighlight As String = "0x8000000D"
    Private Const vbHighlightText As String = "0x8000000E"
    Private Const vbButtonFace As String = "0x8000000F"
    Private Const vbButtonShadow As String = "0x80000010"
    Private Const vbGrayText As String = "0x80000011"
    Private Const vbButtonText As String = "0x80000012"
    Private Const vbInactiveCaptionText As String = "0x80000013"
    Private Const vb3DHighlight As String = "0x80000014"
    Private Const vb3DDKShadow As String = "0x80000015"
    Private Const vb3DLight As String = "0x80000016"
    Private Const vbInfoText As String = "0x80000017"
    Private Const vbInfoBackground As String = "0x80000018"

    Private Const m_lRiskDiscount As Integer = 0
    Private Const kEnableDoNotMergeClause As Integer = 5206

    ' {* USER DEFINED CODE (Begin) *}

    Private m_lPartyCnt As Integer
    Private m_lFinancePlancnt As Integer
    Private m_lFinancePlanVersion As Integer
    'Fields from Insurance File
    Private m_vFieldArray As Object

    'TN20001027 (Start)
    Private m_vKeyArray(,) As Object
    'TN20001027 (End)

    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFileStructureID As Integer
    Private m_lInsuranceFileTypeID As Integer
    Private m_vInsuranceFileStatusID As Object
    Private m_lInsuranceFileID As Integer
    Private m_iSourceId As Integer
    'Private m_lInsuranceFolderCnt As Long
    Private m_sInsuranceRef As String = ""
    Private m_lProductID As Integer
    Private m_vLeadInsurerCnt As Object
    Private m_vLeadAgentCnt As Object
    Private m_vLeadAgentPercent As Object
    'EK 18/11/99
    Private m_sAgentRef As String = ""
    Private m_vAccountHandlerCnt As Object
    Private m_lInsuredCnt As Integer
    Private m_vBusinessTypeId As Object
    Private m_vCollectTypeID As Object
    Private m_vCollectionFromCnt As Object

    Private v_Result As Object
    Private m_vSubBranchId As Object
    Private m_iCurrencyID As Integer
    Private m_iLanguageID As Integer
    Private m_vDateIssued As Object
    Private m_dtCoverStartDate As Date
    Private m_dtExpiryDate As Date
    Private m_dtRenewalDate As Date
    Private m_vRenewalMethodID As Object
    Private m_iRenewalFrequencyID As Integer
    Private m_iIsReferredAtRenewal As Integer
    Private m_vLapsedReasonID As Object
    Private m_vLapsedDate As Object
    Private m_vLapsedDescription As Object
    Private m_iIsReferredOnMta As Object
    Private m_iPolicyVersion As Integer
    Private m_vGeminiPolicyStatus As Object
    Private m_vGeminiBusinessType As Object
    Private m_vDeferredInd As Object
    Private m_vPolicyIgnore As Object
    Private m_vBrokerCnt As Object
    Private m_vRiskCodeID As Object
    Private m_vRiskGroupID As Object
    Private m_vAnalysisCodeID As Object
    Private m_vPolicyDeductiblesID As Object
    Private m_vPolicyLimitsID As Object
    Private m_vProposalDate As Object
    Private m_vDiaryDate As Object
    Private m_vReviewDate As Object
    Private m_vRenewalDayNumber As Object
    Private m_vPolicyTypeId As Object
    Private m_vIndicator As Object
    Private m_vClause As Object
    Private m_vCover As Object
    Private m_vArea As Object
    Private m_vLongTermUndertakingDate As Object
    Private m_vRenewalStopCodeID As Object
    Private m_vVBSType As Object
    Private m_vVBSStatus As Object
    Private m_vIsInsurerRateTable As Object
    Private m_vIsRelatedPolicies As Object
    Private m_vIsRetainedDocuments As Object
    Private m_vSchemesPostcode As Object
    Private m_vPaidDirect As Object
    Private m_vScheme As Object
    Private m_vBrokerageAmount As Object
    Private m_vIsMinimumBrokerageFlag As Object
    Private m_vAnnualPremium As Object
    Private m_vThisPremium As Object
    Private m_vNetPremium As Object

    Private m_vCommissionAmount As Object
    Private m_vIPTableAmount As Object
    Private m_vIPTPercentage As Object
    Private m_vIsIPTOverridden As Object
    Private m_vTaxAmount As Object
    Private m_vVatableAmount As Object
    Private m_vVatPercentage As Object
    Private m_vVatAmount As Object
    Private m_vPaymentMethod As Object
    Private m_vUserDefinedDataID As Object
    'EK 300300
    Private m_vExemptAmount As Object
    'ECK 14/7/99
    Private m_vCommissionPercentage As Object
    'TN20000815 - Doc Ref 10
    Private m_vInsuredName As Object
    Private m_vAlternateReference As Object
    Private m_vIsClientInvoiced As Object
    Private m_vOldPolicyNumber As Object
    Private m_vQuoteExpiryDate As Object
    Private m_vAlternateAccountCnt As Object
    Private m_vUnderwritingYearID As Object
    Private m_bUnderwritingYearID As Boolean 'Is option switched on/off
    Private m_vPolicyStatusID As Object

    'References from Party Lookups
    Private m_sInsurerName As String = ""
    Private m_sBrokerName As String = ""
    Private m_sRiskDesc As String = ""
    Private m_sAnalysisDesc As String = ""
    Private m_vPolicyDeductibles As Object
    Private m_vPolicyLimits As Object
    Private m_sHandlerName As String = ""
    Private m_sScheme As String = ""
    Private m_sCurrency As String = ""
    Private m_sAgentName As String = ""
    Private m_iAgentAllowedCommission As Integer
    Private m_sRelatedPolicyCode As String = ""
    Private m_sRelationshipType As String = ""
    Private m_sPolicyTypeDesc As String = ""
    Private m_sProductDesc As String = ""

    Private m_bIgnoreHandler As Boolean
    Private m_iOldSelectedBranchIndex As Integer

    'TN20000817 - Doc Ref 10
    Private m_sAlternateAccountName As String = ""

    'Fields from Insurance Folder
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFolderId As Integer
    Private m_iFolderSourceId As Integer
    Private m_lInsuranceHolderCnt As Integer
    Private m_sCode As String = ""
    Private m_vDescription As Object
    Private m_vInceptionDate As Object
    Private m_vInceptionTPI As Object
    Private m_vArcArchiveFolderId As Object
    Private m_vQuoteInsuranceRef As Object
    Private m_vNextInsuranceRef As Object
    Private m_vLastInsuranceRef As Object
    Private m_vRenewalCount As Object



    'Fields from Insurance File System
    Private m_lEndorsementCount As Integer
    Private m_iCreatedById As Integer
    Private m_dtDateCreated As Date
    Private m_vModifiedById As Object
    Private m_vLastModified As Object
    Private m_vLastTransDate As Object
    Private m_vLastTransType As Object
    Private m_vLastTransDescription As Object
    Private m_vLastTransDebitCredit As Object
    Private m_vLastTransDocumentRef As Object
    Private m_vLastTransCoverStartDate As Object
    Private m_vLastTransExpiryDate As Object


    'Tax fields - currencies for accuracy
    Private m_cPremiumExcTax As Decimal
    Private m_cPremiumIncTax As Decimal
    Private m_cIPTRate As Decimal
    Private m_cIPTAmount As Decimal
    Private m_cVATRate As Decimal
    Private m_cVATableAmount As Decimal
    Private m_cVATAmount As Decimal
    Private m_cIPTableAmount As Decimal
    'EK 303000
    Private m_cExemptAmount As Decimal

    'Fields from Relationship Table
    Private m_vRelationship As Integer
    Private m_vRelatedPolicyCnt As Object

    'Don't do business logic when business to interface
    Private m_bSetUp As Boolean
    'EK 17/11/99 Flag to indicate whether we need to check the  agent
    Private m_bVerifyAgentCnt As Boolean

    'Arrays
    Private m_vFees(,) As Object
    Private m_vFeeTypes As Object
    Private m_vStandardWordings As Object

    'TN20000818 - Doc Ref 10
    Private m_vAgentList(,) As Object 'array to store sub agents
    Private m_bIsSubAgentAdded As Boolean 'Subagent status

    'Total fees
    Private m_cTotalFees As Decimal

    Private m_iLine As Integer
    Private m_lPolicyWordingCnt As Integer
    Private m_lFeeCnt As Integer

    Private m_bEvent As Boolean

    Private m_bEventRaised As Boolean

    'TN20000807
    Private m_bPMRaiseEvent As Boolean 'set to true when data has been modified (even from sub modules)

    'RWH(23/04/2001)
    Private m_sAdvanceMonthsAllowed As String = ""

    ' PW311002
    Private m_sCoverToMonthsAllowed As String = ""

    Private m_iMidnightRenewal As Integer

    'JMK 13/11/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""

    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
    Private m_vClientList(,) As Object
    Private m_vClientOrig As Object

    'sj 19/07/2002 - start
    Private m_bUserMode As Boolean
    'sj 19/07/2002 - end

    'sj 23/09/2002 - start
    Private m_bMultiTreeAccounting As Boolean
    'sj 23/09/2002 - end

    Private m_bUseClientPolicyLinkage As Boolean
    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

    'PM 26/09/2006
    Private m_bIsAgentAttachedWithClient As Boolean

    ' PW311002
    Private m_dtInitialCoverToDate As Date

    Private m_dtInitialCoverFromDate As Date

    ' PW311002
    Private m_bIsNRMA As Boolean

    'CMG/PB
    Private m_sCrspnd As String = ""
    Private m_sLead As String = ""

    'CMG KR
    Private m_bIsRenewal As Boolean
    Private m_bSetQuoteToLapsed As Boolean

    Private m_lSchemeID As Integer
    Private m_sSchemeDesc As String = ""

    Private m_lPolicyStyleID As Integer
    Private m_vPolicyStyleMandatory As String = ""

    Private m_lCurrencyChange As Integer
    Private m_bAllowOverride As Boolean

    Private m_dBaseExchangeRate As Double
    Private m_dSystemExchangeRate As Double
    Private m_dtEffectiveDateOfExchange As Date
    Private m_iRateOverrideReasonID As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_sSystemOption1040 As String = ""

    Private m_dtCommonRenewalDate As Date
    Private m_sPartyCategoryCode As String = ""
    Private m_bIsSingleInstalmentPlan As Boolean

    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean
    Private m_bIsExit As Boolean
    Private m_bEnableDoNotMergeClause As Boolean
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    '**********************
    ' policy discount fields

    ' interface fields
    Private m_dPrevDiscountPercentage As Double
    Private m_crPrevDiscountedPremium As Decimal
    Private m_crTotalPremiumDiscountOrig As Decimal
    Private m_lPrevDiscountReasonId As Integer
    Private m_bIsPolicyDiscountActive As Boolean
    Private m_bDiscountCalculatedPreviously As Boolean
    Private m_iPreviousBusinessType As Integer

    ' insurance file
    Private m_iDiscountReasonId As Integer
    Private m_crDiscountedPremium As Decimal
    Private m_dDiscountPercentage As Double
    Private m_iMatchDiscountedPremiumFlag As Integer

    ' insurance folder
    Private m_lDiscountRecurringTypeID As Integer
    Private m_lDiscountInsuranceFileCnt As Integer
    Private m_vDiscountTermEndDate As Date

    '**********************

    Private m_vProductDetails(,) As Object
    Private m_bIsTrueMonthlyPolicy As Boolean
    Private m_lPutOnNextInstalmentRenewal As Integer
    Private m_lAnniversaryCopy As Integer
    Private m_dtAnniversaryDate As Date
    Private m_lRenewalDayNumber As Integer
    Private m_bProcessingTMPDates As Boolean
    'TMP Added Five Variables
    Private m_bISProductConfAllowLeadConsolidatedCommission As Boolean
    Private m_bISProductConfAllowSubConsolidatedCommission As Boolean
    Private m_bIsAgentConfAllowConsolidatedcommission As Boolean

    Private m_iLeadAllowConsolidatedCommission As Integer
    Private m_iSubAllowConsolidatedCommission As Integer

    Public Event BusinessTypeChange(ByVal Sender As Object, ByVal e As BusinessTypeChangeEventArgs) 'Event for Business Type
    Public Event SubAgentChange(ByVal Sender As Object, ByVal e As EventArgs) 'Event fired when subagent added or  deleted

    Private Const ACCoInsuranceLinktoAgent As Integer = 5026
    Private m_sCoInsuranceLinktoAgent As String = ""

    Private m_vGetAssociatedSubAgent(,) As Object
    Private m_iBusinessType_Orig As Integer
    Private m_sAgentCode_Orig As String = ""
    Private Const PMKeyNameFinancePlanEditAuthority As String = "FinancePlanEditAuthority"
    Private m_bIsBackdatedMTAVersion As Boolean

    Private m_oAutoMTA As bSIRAutoMTA.Business
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    Private m_bBackDatedMTAsAllowed As Boolean
    Private m_sSelectedPolicyStatus As String = ""
    Private m_bIsMTATemp As Boolean
    Private m_dRenewaldate As Date
    Private m_bIsPriorDate As Boolean
    Private m_bIsRenewed As Boolean
    Private m_oDefaultClauses As Object
    Private m_dtLapsedDate As Date
    Private m_vDefaultClauses As Object
    'Start - Sankar - PN 53104
    Private m_bLapseAQuote As Boolean
    Private m_bRenCalDate As Boolean
    Private m_dManualDiscountPercentage As Double
    Private m_vContactuserId As Integer
    Private m_sCoInsPlacement As String = ""
    Private m_iMTAReasonId As Integer
    Private m_sMTAReasonDesc As String = String.Empty
    Private m_bUnifiedRenewalDateIsReadOnly As Boolean
    Private m_oDefaultCoverToDateToLastDay As Object
    Private m_bAgentReceiveCorrespondenceFlag As Boolean = False
    Private m_iCorrespondenceType As Integer = 0
    Private m_vClientDefaultPreferredCorrespondence(2, 0) As Object
    Private m_vAgentDefaultPreferredCorrespondence(2, 0) As Object
    Private m_bPolicyIsInRenewal As Boolean
    Private m_bDoNotDeleteRenewalQuoteOnMTA As Boolean
    Private m_bDeletePolicyFromRenewal As Boolean
    Private m_sRenewalVersionStartDate As String
    Private m_sRequiredCoverEndDate As String
    Private m_lRenewalInsuranceFileKey As Integer

#Region "Public Properties"
    <Browsable(True)>
    Public Property LapseAQuote() As Boolean
        Get
            Return m_bLapseAQuote
        End Get
        Set(ByVal Value As Boolean)
            m_bLapseAQuote = Value
        End Set
    End Property
    'End - Sankar - PN 53104
    <Browsable(True)>
    Public Property BackDatedMTAsAllowed() As Boolean
        Get
            Return m_bBackDatedMTAsAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_bBackDatedMTAsAllowed = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property SelectedPolicyStatus() As String
        Get
            Return m_sSelectedPolicyStatus
        End Get
        Set(ByVal Value As String)
            m_sSelectedPolicyStatus = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property IsMTATemp() As String
        Get
            Return CStr(m_bIsMTATemp)
        End Get
        Set(ByVal Value As String)
            m_bIsMTATemp = CBool(Value)
        End Set
    End Property
    <Browsable(True)>
    Public Property Renewaldate() As Date
        Get
            Return m_dRenewaldate
        End Get
        Set(ByVal Value As Date)
            m_dRenewaldate = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property IsPriorDate() As Boolean
        Get
            Return m_bIsPriorDate
        End Get
        Set(ByVal Value As Boolean)
            m_bIsPriorDate = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property IsRenewed() As Boolean
        Get
            Return m_bIsRenewed
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewed = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property LapsedDate() As Date
        Get
            Return m_dtLapsedDate
        End Get
        Set(ByVal Value As Date)
            m_dtLapsedDate = Value
        End Set
    End Property
    'End  (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
    <Browsable(False)>
    Public ReadOnly Property Name_Renamed() As String
        Get
            Return MyBase.Name
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property

    'TN20010126 End

    <Browsable(True)>
    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value
            RaiseEvent StatusChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value
            RaiseEvent NavigateChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()
        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}

    <Browsable(True)>
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
            RaiseEvent InsuranceFileCntChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
            RaiseEvent InsuranceFolderCntChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
            RaiseEvent PartyCntChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property PolicyTypeId() As Object
        Get
            Return m_vPolicyTypeId
        End Get
        Set(ByVal Value As Object)


            m_vPolicyTypeId = Value
            RaiseEvent PolicyTypeIdChange()
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
    End Property

    'eck180500
    <Browsable(True)>
    Public Property SourceId() As Object
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Object)

            m_iSourceId = CInt(Value)
            RaiseEvent SourceIdChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskCodeId() As Object
        Get
            Return m_vRiskCodeID
        End Get
        Set(ByVal Value As Object)


            m_vRiskCodeID = Value
            RaiseEvent RiskCodeIdChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property RiskGroupId() As Object
        Get
            Return m_vRiskGroupID
        End Get
        Set(ByVal Value As Object)


            m_vRiskGroupID = Value
            RaiseEvent RiskGroupIdChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property ProductId() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
            RaiseEvent ProductIdChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property BusinessTypeId() As Object
        Get
            Return m_vBusinessTypeId
        End Get
        Set(ByVal Value As Object)


            m_vBusinessTypeId = Value
            RaiseEvent BusinessTypeIdChange()
        End Set
    End Property

    'TN20010126 Start
    <Browsable(False)>
    Public ReadOnly Property InsuranceFileStatusID() As Object
        Get
            Return m_vInsuranceFileStatusID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            'Return Me.Controls_Renamed
            Return Me.Controls

        End Get
    End Property

    ' {* USER DEFINED CODE (End) *}


    <Browsable(True)>
    Public Shadows Property BackColor() As Integer
        Get
            Return m_BackColor
        End Get
        Set(ByVal Value As Integer)
            m_BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property ForeColor() As Integer
        Get
            Return m_ForeColor
        End Get
        Set(ByVal Value As Integer)
            m_ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property


    <Browsable(True)>
    Public Overrides Property Font() As Font
        Get
            Return m_Font
        End Get
        Set(ByVal Value As Font)
            m_Font = Value
            RaiseEvent FontChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property BackStyle() As Integer
        Get
            Return m_BackStyle
        End Get
        Set(ByVal Value As Integer)
            m_BackStyle = Value
            RaiseEvent BackStyleChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property BorderStyle() As Integer
        Get
            Return m_BorderStyle
        End Get
        Set(ByVal Value As Integer)
            m_BorderStyle = Value
            RaiseEvent BorderStyleChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property FromEvent() As Boolean
        Get
            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bEvent = Value
            RaiseEvent FromEventChange()
        End Set
    End Property

    <Browsable(True)>
    Public Property EventRaised() As Boolean
        Get
            Return m_bEventRaised
        End Get
        Set(ByVal Value As Boolean)
            m_bEventRaised = Value
            RaiseEvent EventRaisedChange()
        End Set
    End Property

    'TN20000807
    <Browsable(True)>
    Public Property PMRaiseEvent() As Boolean
        Get
            Return m_bPMRaiseEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bPMRaiseEvent = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property IsRenewal() As Boolean
        Get
            Return m_bIsRenewal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsRenewal = Value
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property IsSubAgentAdded() As Boolean
        Get
            'Check for subagent included or not
            m_bIsSubAgentAdded = lvwAgents.Items.Count > 0

            Return m_bIsSubAgentAdded
        End Get
    End Property
    <Browsable(True)>
    Public Property SetQuoteToLapsed() As Boolean
        Get
            Return m_bSetQuoteToLapsed
        End Get
        Set(ByVal Value As Boolean)
            m_bSetQuoteToLapsed = Value
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property IsSingleInstalmentPlan() As Boolean
        Get
            Return m_bIsSingleInstalmentPlan
        End Get
    End Property
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    <Browsable(False)>
    Public ReadOnly Property IsTrueMonthlypolicyandNextInstalmentRenewal() As Boolean
        Get
            Return m_bIsTrueMonthlypolicyandNextInstalmentRenewal
        End Get
    End Property
    <Browsable(True)>
    Public Property IsExit() As Boolean
        Get
            Return m_bIsExit
        End Get
        Set(ByVal Value As Boolean)
            m_bIsExit = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

    <Browsable(False)>
    Public ReadOnly Property LeadAgentCnt() As Object
        Get
            Return m_vLeadAgentCnt
        End Get
    End Property

    <Browsable(True)>
    Public Property EnableDoNotMergeClause() As Boolean
        Get
            Return m_bEnableDoNotMergeClause
        End Get
        Set(ByVal Value As Boolean)
            m_bEnableDoNotMergeClause = Value
        End Set
    End Property
#End Region
    '
    ' PW311002 - function to default the quote expiry date using the grace
    '            period from the product table
    '
    Private Function DefaultQuoteExpiryDate() As Integer

        Dim result As Integer = 0
        Dim lGracePeriodDays As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If cboProduct.SelectedIndex <> -1 Then
                m_lReturn = m_oBusinessPolicy.GetGracePeriod(v_lProductID:=VB6.GetItemData(cboProduct, cboProduct.SelectedIndex), r_lGracePeriodDays:=lGracePeriodDays)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_vQuoteExpiryDate = DateTime.Today.AddDays(lGracePeriodDays)
            m_lReturn = m_oFormFields.FormatControl(txtQuoteExpiryDate, m_vQuoteExpiryDate)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultQuoteExpiryDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DefaultQuoteExpiryDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name : RenewalPolicy
    '
    ' Desc : Renewal this policy
    '
    ' Hist : 13/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function RenewalPolicy(ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oObject As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            result = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURenSelection.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                System.Windows.Forms.MessageBox.Show("Failed To Create iPMURenSelection object", ACApp, MessageBoxButtons.OK)
                Return result
            End If


            oObject.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oObject.RenewalMode = 1 'silent mode


            result = oObject.Start()


            oObject.Dispose()

            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function IsPolicyInRenewal(ByVal v_lInsuranceFolderCnt As Integer) As Integer
        Dim vResultArray(,) As Object = Nothing
        Return IsPolicyInRenewal(v_lInsuranceFolderCnt, vResultArray)
    End Function


    ' ***************************************************************** '
    ' Name : IsPolicyInRenewal
    '
    ' Desc : return PMTRUE if a version of this policy is in renewal
    '           return PMNOTFOUND if policy is not in renewal
    '
    ' Hist : 13/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function IsPolicyInRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim bSIRRenSelection As Object


        Dim oObject As bSIRRenSelection.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object = Nothing
            result = g_oObjectManager.GetInstance(temp_oObject, "bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oObject = temp_oObject

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                System.Windows.Forms.MessageBox.Show("Failed To Create bSIRRenSelection object", ACApp, MessageBoxButtons.OK)
                Return result
            End If


            result = oObject.GetRenewalVersion(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vResultArray)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If Not Information.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If


            oObject.Dispose()

            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsPolicyInRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPolicyInRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelClick
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function CancelClick() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As Integer
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If tabMainTab.Visible Then
                Return CancelPolicy()
            End If

            'TN20000824 - Doc Ref 10
            If (tabAgent.Visible) Or (tabCommissionTab.Visible) Then
                'hide sub tabs and show main tab
                tabAgent.Visible = False
                tabCommissionTab.Visible = False
                tabMainTab.Visible = True
            End If

            'And this stops us exiting.

            Return gPMConstants.PMEReturnCode.PMFalse

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelPolicy
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'TN20000808 create event if flag is set
                If PMRaiseEvent Then
                    m_lReturn = CreateEvent()
                End If

                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicy
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    'If in edit mode, lock the party
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                '  m_lReturn = LockPolicy

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView Then

                ' Get the interface details from the
                ' business object.
                m_lReturn = GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            ' If we are adding we need to retrieve the default associated client
            If m_bUseClientPolicyLinkage Then

                If Task = gPMConstants.PMEComponentAction.PMAdd Then

                    ' Get associated clients for listview

                    m_lReturn = m_oBusiness.GetPolicyClient(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lPartyCnt:=m_lPartyCnt, r_vResultArray:=m_vClientList)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Load insured clients into the listview
                    m_lReturn = LoadInsuredClients()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

            m_lReturn = SetFieldValidationAfterGetPolicy()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "MTA" And Task <> gPMConstants.PMEComponentAction.PMAdd And m_sUnderwritingType = "U" Then
                m_iBusinessType_Orig = VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex)
                m_sAgentCode_Orig = pnlAgentCode.Text.Trim()
                If m_iBusinessType_Orig = 3 OrElse m_iBusinessType_Orig = 4 Then
                    If pnlAgentCode.Text.Trim() = "" Then
                        cmdAgentCode.Enabled = False
                    End If
                End If
            End If
            m_lReturn = SetupTrueMonthlyPolicy()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMView Then

                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                ' disable Add button on Agent tab
                'RWH(07/06/01) Added cmdAddAgent into list to be enabled.
                'RKS 01/03/2005 Added txtOldPolicyNo & lblOldPolicyNo into list to be enabled
                m_lReturn = SetControlEnable(v_iState:=1, v_sContainerName:="fraAgents", v_sException:="cmdEditAgent;cmdPrevious;tabMainTab;" &
                            "cmdDeleteAgent;tabAgent;cmdAddAgent;chkConsolidatedSubCommission;chkConsolidatedLeadCommission;")

                Select Case m_sTransactionType
                    Case "NB", "MTA"

                        m_lReturn = SetControlEnable(v_iState:=0, v_sContainerName:="fraPremium", v_sException:="cmdNext;cmdPrevious;tabMainTab;" &
                                    "cboTransactionGroup;lblTransactionGroup;cboCurrency;" &
                                    "lblCurrency;txtOldPolicyNo;lblOldPolicyNo;tabCommissionTab;tabAgent;chkConsolidatedSubCommission;chkConsolidatedLeadCommission;")

                    Case "EDIT"

                        m_lReturn = DisableForm(lDisabled:=True)

                        cboAnalysisCode.Enabled = True
                        cboPolicyDeductible.Enabled = True
                        cboPolicyLimits.Enabled = True
                        chkHandler.Enabled = True
                        If chkHandler.Checked Then
                            cmdHandler.Enabled = True
                        End If
                        cboPolicyStatus.Enabled = True
                        cboRenewalMethod.Enabled = True
                        cboRenewalStop.Enabled = True
                        lblReferredAtRenewal.Enabled = True
                        lblReferredOnMTA.Enabled = True
                        chkReferredAtRenewal.Enabled = True
                        chkReferredOnMTA.Enabled = True
                        cmdAgentCode.Enabled = False
                        txtAlternateReference.Enabled = True
                        txtRegarding.Enabled = True
                        cmdCoInsurer.Enabled = False
                        txtOldPolicyNo.Enabled = True

                    Case Else

                        If IsRenewal Then

                            m_lReturn = SetControlEnable(v_iState:=0, v_sContainerName:="fraPremium", v_sException:="cmdNext;cmdPrevious;tabMainTab;" &
                                        "cmdReInsurer;cmdCoInsurer;cmdCommission;" &
                                        "tabCommissionTab;tabAgent;fraPremium;cboCurrency;")

                        Else

                            m_lReturn = SetControlEnable(v_iState:=0, v_sContainerName:="fraPremium", v_sException:="cmdNext;cmdPrevious;tabMainTab;" &
                                        "cmdReInsurer;cmdCoInsurer;cmdCommission;" &
                                        "tabCommissionTab;tabAgent;fraPremium;")

                        End If

                End Select

            End If

            ' PW311002 - default the Quote Expiry date
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or txtQuoteExpiryDate.Text.Trim() = "" Then

                m_lReturn = DefaultQuoteExpiryDate()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String

        Dim sHelpFile As String = ""
        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sValue As String = ""
        Dim vReturn As String = ""
        Dim sEnableDoNotMerge As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_iCurrencyID = .CurrencyID
                g_sUserName = .UserName
                g_sPassword = .Password
                g_iLogLevel = .LogLevel
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                System.Windows.Forms.MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If
            'Developer Guide No solution no 39 (no solution)
            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                'devloper guide no.
                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            sEnableDoNotMerge = "0"
            m_lReturn = GetSystemOption(kEnableDoNotMergeClause, sEnableDoNotMerge)

            If String.IsNullOrEmpty(sEnableDoNotMerge) OrElse sEnableDoNotMerge = "0" Then
                m_bEnableDoNotMergeClause = False
            Else
                m_bEnableDoNotMergeClause = True
            End If
            'Arul-Bug Fixing
            If m_lInsuranceFileCnt = 0 Then


                m_lReturn = m_oBusiness.GetStandardWordings(m_lInsuranceFileCnt, m_vStandardWordings, m_lProductID, m_iSourceId)

                PopulateStandardWordings()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("Initilize", "PopulateStandardWordings failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                'End Arul PN63627
            End If
            'End Arul
            'Stephen-Bug Fixing
            ' PW311002 - Get an instance of the policy business object via
            ' the public object manager.
            Dim temp_m_oBusinessPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusinessPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusinessPolicy = temp_m_oBusinessPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' PW311002 - Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' PW311002 - Display error stating the problem.
                ' PW311002 - Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' PW311002 - Display message.
                System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'Get InsuranceFileSystem
            Dim temp_m_oSystemBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oSystemBusiness, "bSIRInsuranceFileSystem.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oSystemBusiness = temp_m_oSystemBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the system business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'Get InsuranceFolder
            Dim temp_m_oFolderBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFolderBusiness, "bSIRInsuranceFolder.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFolderBusiness = temp_m_oFolderBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get folder business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'Get list manager
            Dim temp_g_oListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oListManager, sClassName:="iGEMListManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            PMBGeneralFunc.g_oListManager = temp_g_oListManager

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGEMListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oPartyAG As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyAG, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPartyAG = temp_m_oPartyAG

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            'Get InsuranceFolder
            Dim temp_m_oCoverNoteBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCoverNoteBusiness, "bSIRCoverNote.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCoverNoteBusiness = temp_m_oCoverNoteBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Cover Note business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'Backdated MTA
            Dim temp_m_oAutoMTA As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAutoMTA, "bSIRAutoMTA.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAutoMTA = temp_m_oAutoMTA

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSirAutoMTA business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'm_lReturn = g_oListManager.Initialise()


            m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get latest list manager files.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            ' Get the client policy linkage product option
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClientPolicyLinkage, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vReturn)

            'sj 24/07/2002 - start
            m_bUseClientPolicyLinkage = Conversion.Val(vReturn) = 1

            'sj 23/09/2002 - start
            'See if we are multi tree accounting
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Multi Tree Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                'Multi tree accounting
                m_bMultiTreeAccounting = True
                Return result
            End If

            If m_lProductID <> 0 Then

                ' Get "allow currency change"
                m_lReturn = GetAllowCurrencyChange()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get allow currency change", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                End If

                ' get the product details
                m_lReturn = GetProductDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetProductDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                End If

            End If

            If GetSystemOption(1040, m_sSystemOption1040) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option 1040 (validate cancelled agent/broker)", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            End If
            m_lReturn = GetSystemOption(ACCoInsuranceLinktoAgent, m_sCoInsuranceLinktoAgent)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetAllowCurrencyChange
    ' Description:  RDC 10052004 get "allow currency change" from
    '               Product table via bSIRProduct.Business
    ' ***************************************************************** '
    Private Function GetAllowCurrencyChange() As Integer
        Dim result As Integer = 0
        Dim oProduct As bSIRProduct.Business

        Dim sTitle, sMessage As String

        Try

            ' RDC 10052004 get "allow currency change" from
            ' Product table via bSIRProduct.Business
            Dim temp_oProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProduct = temp_oProduct

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Go get the value and populate the module variable

            'Developer Guide No. 8
            m_lReturn = oProduct.GetAllowCurrencyChange(m_lProductID, m_lCurrencyChange, 0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                oProduct.Dispose()
                oProduct = Nothing

                ' Display error stating the problem.
                ' Get description from the resource file.

                'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' done with bSIRProduct.Business

            oProduct.Dispose()

            oProduct = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowCurrencyChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowCurrencyChange", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim i As Integer

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt

            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt
            ' {* USER DEFINED CODE (End) *}

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            'JMK 13/11/2001 - get hidden option

            m_sUnderwritingType = m_oBusiness.UnderwritingType

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Arul
            m_lReturn = SetupSelectClausesListView()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACSelectClasueLoadControl, "SetupSelectClausesListView failed to setup the columns", gPMConstants.PMELogLevel.PMLogError)
            End If
            AddHandler lvwPolicyWording.ItemChecked, AddressOf lvwPolicyWording_ItemChecked
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RecalculateFees
    '
    ' Description: We've changed the premium, so let's recalculate the fees
    '
    ' ***************************************************************** '
    Public Function RecalculateFees(ByRef cPremium As Decimal) As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim oListItem As ListViewItem
        Dim cWork As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iTemp = 1

            m_cTotalFees = 0
            Do

                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    'percentage
                    txtPercentage.Text = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

                    cWork = CDec(m_oFormFields.UnformatControl(txtPercentage))
                    If cWork = 0 Then
                        'No percentage, flat fee
                        txtAmount.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text

                        cWork = CDec(m_oFormFields.UnformatControl(txtAmount))
                    Else
                        'Amount
                        cWork = cPremium * cWork / 100.0#
                        m_lReturn = m_oFormFields.FormatControl(txtAmount, cWork)
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtAmount.Text
                    End If
                    m_cTotalFees += cWork
                End If
                iTemp += 1
            Loop

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recalculate fees", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateFees", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: What is this supposed to do?
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

    End Sub

    ''' <summary>
    ''' SavePolicy-Saves the displayed policy details
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function SavePolicy() As Integer
        Dim objfrmDialogMsg As New frmDialogMsg
        Dim nResult As Integer = 0
        Dim oReturnArray As Object
        Dim nRecordsAffected As Integer

        Dim dtDefaultDate As Date
        Dim oDateCancelled As Object
        Dim rMsgResult As DialogResult
        Dim oArray As Object

        Dim dtTransactionDate As Date
        Dim oMTAProrata As Object
        Dim oChangePolicyStatus As bSIRChangePolicyStatus.Business

        Try

            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Don't forget to put this back when form control can handle user controls
            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Validate Policy Discount
            If cboDiscountReason.ItemId > 0 Then
                If cboDiscountRecurringType.ItemId = 0 Then
                    System.Windows.Forms.MessageBox.Show("You have not entered a discount / loading recurring type",
                                                         "Discount Validation", MessageBoxButtons.OK,
                                                         MessageBoxIcon.Information)
                    cboDiscountRecurringType.Focus()
                    Return nResult
                End If

                If CDbl(txtDiscountPercentage.Text.Substring(0, Strings.Len(txtDiscountPercentage.Text) - 1)) = 0 Then
                    System.Windows.Forms.MessageBox.Show("You have not entered a discount / loading percentage",
                                                         "Discount Validation", MessageBoxButtons.OK,
                                                         MessageBoxIcon.Information)
                    txtDiscountPercentage.Focus()
                    Return nResult
                End If
            End If

            If m_sTransactionType <> "MTA" Then
                m_lReturn = IsClosedBranch(VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex))
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    System.Windows.Forms.MessageBox.Show("Selected branch is Closed.Please choose another.", ACApp,
                                                         MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cboBranchCode.Focus()
                    Return nResult
                End If
            End If

            'Validate the policy number - only if it has been changed!
            If m_sInsuranceRef.Trim() <> txtPolicyNumber.Text.Trim() Then
                m_lReturn = ValidatePolicyNumber()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                ElseIf m_sTransactionType = "NB" Then 'PM049747 Policy Number should be updated only during NB not during MTA, cancellation, Reinstatement etc.
                    'PM039561 Update the variable too if we have validated the new policy number
                    m_sInsuranceRef = CStr(m_oFormFields.UnformatControl(ctlControl:=txtPolicyNumber))
                End If
            End If
            Dim sOptionValue As String = String.Empty
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue,
                                                v_iSourceID:=g_oObjectManager.SourceID)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then

                If txtPolicyNumber.Text <> "" And m_sTransactionType = "NB" Then
                    If IsValidString(txtPolicyNumber.Text) = False Then
                        System.Windows.Forms.MessageBox.Show("Policy Number can't contain any of the following characters. " & vbNewLine &
                            ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
                        If txtPolicyNumber.Visible Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                            txtPolicyNumber.Focus()
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        Return nResult
                    End If
                End If
            End If

            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 1 Then


                m_vLeadAgentCnt = DBNull.Value
            End If

            If Not (Convert.IsDBNull(m_vLeadAgentCnt) Or IsNothing(m_vLeadAgentCnt)) Then

                m_lReturn = m_oBusinessPolicy.ValidateLeadAgent(v_vLeadAgentCnt:=m_vLeadAgentCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    System.Windows.Forms.MessageBox.Show("Unable to add a sub agent as the leading agent", ACApp,
                                                         MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return nResult
                End If
            End If

            Dim dtInitialCoverFromDate As Date
            Dim nBusinessTypeId As Integer
            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "REN" Then

                '  - Check if quote has expired
                If DateTime.Today > CDate(txtQuoteExpiryDate.Text) And m_sTransactionType <> "REN" Then
                    If cboStatus.Text <> VB6.GetItemString(cboStatus, 3) Then
                        System.Windows.Forms.MessageBox.Show("The quote has expired. Current rates will " &
                            "need to be applied now, and this may affect the " & "premium of the quote.", ACApp,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        '  - set all risks to "unquoted"
                    End If


                    m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                   r_lRecordsAffected:=nRecordsAffected)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'", ACApp,
                                                             MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    '  - reset the quote expiry date
                    m_lReturn = DefaultQuoteExpiryDate()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        System.Windows.Forms.MessageBox.Show("Unable to default the Quote Expiry Date.", ACApp,
                                                             MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Else
                    If m_lInsuranceFileTypeID = 0 Then

                        dtInitialCoverFromDate = DateTime.Parse(DateTime.Now)
                    Else
                        dtInitialCoverFromDate = m_dtInitialCoverFromDate
                    End If
                    If CDate(txtCoverFromDate.Text) <> m_dtInitialCoverFromDate And
                        (m_lInsuranceFileTypeID = 1 Or m_lInsuranceFileTypeID = 0) Then

                        nBusinessTypeId = 1

                        Dim temp_oChangePolicyStatus As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oChangePolicyStatus,
                                                                 "bSIRChangePolicyStatus.Business",
                                                                 vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oChangePolicyStatus = temp_oChangePolicyStatus

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("Save Policy",
                                                    "Failed to Initialize the component  bSIRChangePolicyStatus.Business",
                                                    gPMConstants.PMELogLevel.PMLogError)
                        Else


                            If Object.Equals(m_vLeadAgentCnt, Nothing) Or Convert.IsDBNull(m_vLeadAgentCnt) Or
                                IsNothing(m_vLeadAgentCnt) Then

                                m_vLeadAgentCnt = 0
                            End If


                            m_lReturn = oChangePolicyStatus.CheckPeriodStatus(v_lBusinessType:=nBusinessTypeId,
                                                                              v_iBranch:=m_iSourceId,
                                                                              v_lProductId:=m_lProductID,
                                                                              v_lAgent:=m_vLeadAgentCnt,
                                                                              r_sGeneratedPolicyNumber:=m_sInsuranceRef,
                                                                              v_dtInitialCoverStartDate:=dtInitialCoverFromDate,
                                                                              v_dtCurrentStartDate:=gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim()))


                            If CDbl(m_vLeadAgentCnt) = 0 Then

                                m_vLeadAgentCnt = DBNull.Value
                            End If

                        End If

                        oChangePolicyStatus.Dispose()

                        oChangePolicyStatus = Nothing

                    End If


                    If m_sInsuranceRef <> "" And m_sInsuranceRef.Trim() <> "" AndAlso txtPolicyNumber.Text.Trim = "" Then
                        txtPolicyNumber.Text = m_sInsuranceRef
                        m_sCode = m_sInsuranceRef
                    End If

                    '  - check if cover to date has changed
                    If CDate(txtCoverToDate.Text) <> m_dtInitialCoverToDate Then

                        '  - set all risks to "unquoted"

                        m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                       r_lRecordsAffected:=nRecordsAffected)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'",
                                                                 ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        ' - if some risks were changed we may affect the premium
                        If nRecordsAffected > 0 Then

                            If (System.Windows.Forms.MessageBox.Show(("The 'Cover To' date has changed. This may " +
                                                                      "affect the premium of the quote."), ACApp,
                                                                     MessageBoxButtons.OKCancel,
                                                                     MessageBoxIcon.Exclamation) = vbCancel) Then
                                nResult = vbCancel
                                Return nResult
                            Else
                                m_lReturn = m_oBusinessPolicy.DeleteRisksRI(nInsuranceFileCnt:=m_lInsuranceFileCnt, r_nRecordsAffected:=nRecordsAffected)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    MsgBox("Unable to delete the risks RI", vbOKOnly + vbInformation, ACApp)
                                End If
                            End If
                        End If

                        DeleteRenewalVersion()

                    ElseIf CDate(txtCoverToDate.Text) < CDate(DateTime.Parse(m_dtInitialCoverToDate).ToString("d")) And
                        m_sTransactionType <> "NB" Then

                        m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Product", v_vFieldName:="mta_prorata",
                                                             v_vKeyField:="product_id", v_vKeyID:=m_lProductID,
                                                             r_vResult:=oMTAProrata)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                               sMsg:="Failed to get MTA Prorata product option", vApp:=ACApp,
                                               vClass:=ACClass, vMethod:="SavePolicy")
                            Return nResult
                        End If

                        If gPMFunctions.ToSafeBoolean(oMTAProrata) Then

                            objfrmDialogMsg.ShowDialog()

                            If objfrmDialogMsg.ClickAction = 1 Then

                                ' - set all risk to unquoted

                                m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                       r_lRecordsAffected:=nRecordsAffected)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'", ACApp, MessageBoxButtons.OK,
                                        MessageBoxIcon.Information)
                                End If
                            Else

                                objfrmDialogMsg.Close()
                                Return nResult

                            End If

                            objfrmDialogMsg.Close()

                        End If

                        DeleteRenewalVersion()

                    ElseIf (CDate(txtCoverFromDate.Text) <> m_dtInitialCoverFromDate) And m_sTransactionType = "NB" Then

                        '  - set all risks to "unquoted"

                        m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                       r_lRecordsAffected:=nRecordsAffected)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'",
                                                                 ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        ' - if some risks were changed we may affect the premium
                        If nRecordsAffected > 0 Then
                            System.Windows.Forms.MessageBox.Show("The 'Cover from' date has changed. This may " & "affect the premium of the quote.",
                                ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    End If
                End If
            End If
            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "REN" Then
                If (m_vPolicyDeductiblesID IsNot Nothing AndAlso cboPolicyDeductible.ItemId.ToString <> m_vPolicyDeductiblesID.ToString) OrElse (m_vPolicyDeductiblesID Is Nothing AndAlso cboPolicyDeductible.ItemCaption <> "(None)") Then

                    m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                       r_lRecordsAffected:=nRecordsAffected)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'",
                                                                 ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                ElseIf Convert.ToDouble(txtManualDiscountPercentage.Text) <> m_dManualDiscountPercentage Then
                    m_lReturn = m_oBusinessPolicy.SetRisksUnquoted(v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                                                                       r_lRecordsAffected:=nRecordsAffected)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        System.Windows.Forms.MessageBox.Show("Unable to reset the risks status' to 'unquoted'",
                                                                 ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            If Not (Convert.IsDBNull(m_vLeadAgentCnt) Or IsNothing(m_vLeadAgentCnt)) Then

                dtTransactionDate = DateTime.Today

                If m_sSystemOption1040 = "1" Then
                    dtTransactionDate = CDate(txtCoverFromDate.Text)
                End If


                m_lReturn = m_oBusinessPolicy.GetAgentCancellationDetails(AgentCnt:=m_vLeadAgentCnt,
                                                                          r_vResultArray:=oArray)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Information.IsArray(oArray) Then

                    oDateCancelled = "29/12/1899"
                Else


                    oDateCancelled = oArray(0, 0)
                End If

                If m_sTransactionType = "NB" Then
                    If (gPMFunctions.ToSafeDate(oDateCancelled) <= gPMFunctions.ToSafeDate(dtTransactionDate)) And
                        gPMFunctions.ToSafeDate(oDateCancelled).Year <> 1899 Then
                        System.Windows.Forms.MessageBox.Show(
                            "Agency cancelled - No new transactions can be placed through this agent.",
                            "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return nResult
                    End If

                End If

                If m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "MTC" Then
                    ' Need to check whether the agent has been cancelled or not
                    dtDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

                    If (ToSafeDate(oDateCancelled, #12/29/1899#) <= dtTransactionDate) And
                        (ToSafeDate(oDateCancelled, #12/29/1899#) <> dtDefaultDate) Then
                        rMsgResult = System.Windows.Forms.MessageBox.Show("Agency cancelled - Do you still wish to proceed ?",
                                                                 "Agency Cancelled", MessageBoxButtons.YesNo)
                        If rMsgResult = System.Windows.Forms.DialogResult.No Then
                            Return nResult
                        End If
                    End If
                Else
                    ' Need to check whether the agent has been cancelled or not
                    dtDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

                    If (ToSafeDate(oDateCancelled, #12/29/1899#) <= dtTransactionDate) And
                        (ToSafeDate(oDateCancelled, #12/29/1899#) <> dtDefaultDate) Or
                        ((CDate(txtInceptionDate.Text) >= ToSafeDate(oDateCancelled, #12/29/1899#)) And
                         (ToSafeDate(oDateCancelled, #12/29/1899#) <> dtDefaultDate)) Then
                        System.Windows.Forms.MessageBox.Show(
                            "Agency cancelled - no new transactions can be placed through this " &
                                                             "agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        pnlAgentCode.Text = ""

                        m_vLeadAgentCnt = 0
                        Return nResult
                    End If
                End If
            End If
            If (CDate(txtCoverFromDate.Text) <> m_dtInitialCoverFromDate) And m_sTransactionType = "NB" And
                m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ' - Change inception date for already added risk in NB quote's effective date is changed
                m_lReturn = m_oBusinessPolicy.SetRisksInceptionDate(
                    v_lInsuranceFileCnt:=m_lInsuranceFileCnt,
                    v_dtCoverFromDate:=CDate(txtCoverFromDate.Text))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(
                        iType:=gPMConstants.PMELogLevel.PMLogError,
                        sMsg:="Failed to set risk inception date.",
                        vApp:=ACApp,
                        vClass:=ACClass,
                        vMethod:="SavePolicy")
                    Return nResult
                End If
            End If


            'If we're editing, we want to check the fees and StandardWordings first.  We must create
            'an event if either has changed.  And an event hasn't already been created.

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                ' Delete any existing co-insurer if business type is not 'co-insurer lead'
                If Not (VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 3 Or VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 4) Then

                    If m_oBusiness.DelUnderWriterCoInsurer(v_lInsuranceFileCnt:=m_lInsuranceFileCnt) <>
                        gPMConstants.PMEReturnCode.PMTrue Then

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                           sMsg:="Failed to Delete CoInsurers", vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="SavePolicy")

                        Return nResult

                    End If
                End If
            End If

            If CInt(Conversion.Val(txtDiscountPercentage.Text)) <> 0 Then
                m_lReturn = UpdateRiskDiscountFlag(m_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to Update Discount Flag", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SavePolicy")

                    Return nResult
                End If
            End If

            'update the MTA Reason ID 
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If m_sTransactionType <> "NB" Then
                    m_lReturn = ProcessMTAReasons(m_sMTAReasonDesc, m_iMTAReasonId)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to Update MTA Reasons", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SavePolicy")

                    Return nResult
                End If
            End If

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If Not PMRaiseEvent Then
                    PMRaiseEvent = m_oBusiness.PMRaiseEvent
                End If

                'update the party cnt property

                m_lInsuranceFileCnt = m_oBusiness.InsuranceFileCnt

                m_lInsuranceFolderCnt = m_oBusiness.InsuranceFolderCnt

                'Update fees
                m_lReturn = UpdateFees()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to Update Fees Details", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SavePolicy")

                    Return nResult
                End If

                'Update StandardWordings
                m_lReturn = UpdateStandardWordings()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to Update Standard Wording Details", vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="SavePolicy")

                    Return nResult
                End If

                'update sub agents
                If Task = gPMConstants.PMEComponentAction.PMAdd Then


                    m_lReturn = m_oBusiness.AddSubAgents(v_vInsuranceFileCnt:=m_oBusiness.InsuranceFileCnt,
                                                         r_vValueArray:=m_vAgentList)
                Else

                    m_lReturn = m_oBusiness.AddSubAgents(v_vInsuranceFileCnt:=InsuranceFileCnt,
                                                         r_vValueArray:=m_vAgentList)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to Update Sub-Agents Details", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SavePolicy")

                    Return nResult
                End If

                If m_bUseClientPolicyLinkage Then
                    ' Update client policy links (the change of lead/correspond can be done there as well)
                    If Task = gPMConstants.PMEComponentAction.PMAdd Then


                        m_lReturn = m_oBusiness.AddPolicyClient(v_lInsuranceFolderCnt:=m_oBusiness.InsuranceFolderCnt,
                                                                v_lInsuranceFileCnt:=m_oBusiness.InsuranceFileCnt,
                                                                v_vValueArray:=m_vClientList,
                                                                r_vReturnArray:=oReturnArray)
                    Else

                        m_lReturn = m_oBusiness.AddPolicyClient(v_lInsuranceFolderCnt:=InsuranceFolderCnt,
                                                                v_lInsuranceFileCnt:=InsuranceFileCnt,
                                                                v_vValueArray:=m_vClientList,
                                                                r_vReturnArray:=oReturnArray)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                           sMsg:="Failed to Update Client Policy Links", vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="SavePolicy")

                        Return nResult
                    Else
                        ' Check for return messages
                        If Information.IsArray(oReturnArray) Then

                            For iLen As Integer = oReturnArray.GetLowerBound(0) To oReturnArray.GetUpperBound(0)

                                System.Windows.Forms.MessageBox.Show(CStr(oReturnArray(iLen)), "Insured Clients",
                                                                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Next
                        End If
                    End If
                End If

                'Filter out all other transaction type All other validation has been done already.
                If m_sTransactionType = "NB" And
                    (m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit) _
                    Then
                    m_lReturn = AssignCoverNote()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMCancel
                    End If
                End If

                m_lReturn = CreateEvent()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMCancel

                End If

                nResult = gPMConstants.PMEReturnCode.PMTrue

            End If

            Return nResult

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Function DeleteRenewalVersion() As Integer
        Dim nResult As Integer

        Try
            If (m_bDeletePolicyFromRenewal AndAlso m_lRenewalInsuranceFileKey > 0) Then
                Dim oRenewalProcess As New bSIRRenewalProcess.Business
                Dim sClientName As String = ""

                Dim oBusiness As bSIRRenewalProcess.Business

                nResult = m_lReturn
                Dim temp_oBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRRenewalProcess.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Instance of bSIRListRisks.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskDiscountFlag", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                oBusiness = temp_oBusiness

                nResult = oBusiness.DeletePolicyFromRenewal(m_lRenewalInsuranceFileKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete renewal version", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalVersion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                m_bDeletePolicyFromRenewal = False
            End If
        Catch excep As System.Exception

            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewalVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
        Return nResult
    End Function


    Public Function UpdateRiskDiscountFlag(ByVal lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRListRisks As Object
        '    ' Get an instance of the business object via
        '    ' the public object manager.
        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        Dim oBusiness As bSIRListRisks.Business
        Dim vInsuranceFileDetails As Object


        result = m_lReturn
        Dim temp_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oBusiness = temp_oBusiness

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Instance of bSIRListRisks.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskDiscountFlag", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        ' Get the Insurance file details

        m_lReturn = oBusiness.GetInsuranceFileDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResults:=vInsuranceFileDetails)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Insurance File Detils", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskDiscountFlag", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        ' A check to find out whether discount percentage or value is changed
        ' We will update the discount status and make it No only if the discount
        ' value is changed

        If Conversion.Val(txtDiscountedPremium.Text) <> Conversion.Val(CStr(vInsuranceFileDetails(20, 0))) Or Conversion.Val(txtDiscountPercentage.Text) <> Conversion.Val(CStr(vInsuranceFileDetails(18, 0))) Then


            m_lReturn = oBusiness.UpdateRiskDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lIsDiscounted:=m_lRiskDiscount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to add the details
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
        End If
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer
        ' Fire up the help screen

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)


    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                Application.DoEvents()
                If PMBGeneralFunc.g_oListManager IsNot Nothing Then
                    PMBGeneralFunc.g_oListManager.Dispose()
                    PMBGeneralFunc.g_oListManager = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If Not (m_oPMLock Is Nothing) Then

                    'If in edit mode, unlock the policy
                    If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                        UnlockPolicy()



                    End If

                    m_oPMLock = Nothing
                End If
                If m_oPartyAG IsNot Nothing Then
                    m_oPartyAG.Dispose()
                    m_oPartyAG = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oBusinessPolicy IsNot Nothing Then
                    m_oBusinessPolicy.Dispose()
                    m_oBusinessPolicy = Nothing
                End If
                If m_oSystemBusiness IsNot Nothing Then
                    m_oSystemBusiness.Dispose()
                    m_oSystemBusiness = Nothing
                End If
                If m_oFolderBusiness IsNot Nothing Then
                    m_oFolderBusiness.Dispose()
                    m_oFolderBusiness = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                    m_oFormFields = Nothing
                End If
                If m_oFees IsNot Nothing Then
                    m_oFees.Dispose()
                    m_oFees = Nothing
                End If
                If m_oPolicyWording IsNot Nothing Then
                    m_oPolicyWording.Dispose()
                    m_oPolicyWording = Nothing
                End If
                If m_oShares IsNot Nothing Then
                    m_oShares.Dispose()
                    m_oShares = Nothing
                End If
                If m_oIPT IsNot Nothing Then
                    m_oIPT.Dispose()
                    m_oIPT = Nothing
                End If
                If m_oInsurerRate IsNot Nothing Then
                    m_oInsurerRate.Dispose()
                    m_oInsurerRate = Nothing
                End If
                If m_oPolicyNumber IsNot Nothing Then
                    m_oPolicyNumber.Dispose()
                    m_oPolicyNumber = Nothing
                End If
                If m_oUserAuthorities IsNot Nothing Then
                    m_oUserAuthorities.Dispose()
                    m_oUserAuthorities = Nothing
                End If
                If m_oAutoMTA IsNot Nothing Then
                    m_oAutoMTA.Dispose()
                    m_oAutoMTA = Nothing
                End If


            End If
        End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = 0
    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        ' Forms query unload event.

        Dim result As Integer = 0
        Debug.WriteLine("unload control")

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

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Return result
                End If
            End If

            'If in edit mode, unlock the party
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                m_lReturn = UnlockPolicy()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Terminate the general object.
            Dispose()

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidatePolicy
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidatePolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return ValidateOK()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' CreateEvent
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateEvent() As Integer

        Dim nResult As Integer = 0
        Dim oGetChangeReason As Object

        Dim sDescription As String = ""


        Const kMethodName As String = "CreateEvent"
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "EDIT" Then
                Return nResult
            End If

            'DN 17/10/02 - Add description for policy quote created
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                sDescription = "Quotation record created"
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                Select Case UCase(m_sTransactionType.Trim())
                    Case "MTA", "MTC", "MTR", "REN"
                        sDescription = m_sMTAReasonDesc
                    Case "NB"
                        sDescription = "Policy requoted"


                        If Not (Convert.IsDBNull(m_vInsuranceFileStatusID) Or IsNothing(m_vInsuranceFileStatusID)) Then

                            If CStr(m_vInsuranceFileStatusID) = "2" Then
                                sDescription = "Quote Lapsed"
                            End If
                        End If
                End Select
            End If

            'Tomo(17/07/2001) #905, #1144, Reinstated if necessary.
            'If sDescription = "" Then
            '    'sDescription = InputBox("What is the description for this change of policy?", "Policy")

            '    Dim temp_oGetChangeReason As Object
            '    m_lReturn = g_oObjectManager.GetInstance(temp_oGetChangeReason, sClassName:="iPMBGetChangeReason.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            '    oGetChangeReason = temp_oGetChangeReason
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        gPMFunctions.RaiseError(kMethodName, "oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            '    End If


            '    oGetChangeReason.ProductId = m_lProductID

            '    oGetChangeReason.TransactionType = "MTA"

            '    oGetChangeReason.FormCaption = "Policy " & m_sTransactionType

            '    oGetChangeReason.Start()

            '    If oGetChangeReason.Status <> gPMConstants.PMEReturnCode.PMCancel Then

            '        sDescription = oGetChangeReason.ReasonDescription

            '        m_oBusiness.IsManualDescription = 1
            '    Else

            '        sDescription = ""
            '        Return gPMConstants.PMEReturnCode.PMCancel

            '    End If

            '    oGetChangeReason.Terminate()
            '    oGetChangeReason = Nothing

            'End If

            If sDescription.Trim() = "" Then

                sDescription = Nothing
            Else
                sDescription = sDescription
            End If


            m_oBusiness.EventDescription = sDescription


            m_lReturn = m_oBusiness.MakeEvent

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                nResult = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent")
            End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer
        Dim result As Integer = 0
        Dim bSIRFindInsurance As Object

        Dim m_oTransactionBusiness As bSIRFindInsurance.Form
        Try

            Dim vResults As Object
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetNext(r_vFieldArray:=m_vFieldArray)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            m_lReturn = SetValuesFromArray(v_vFieldArray:=m_vFieldArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the values from the array", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' get product details
            m_lReturn = GetProductDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetProductDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            ' Get "allow currency change"
            m_lReturn = GetAllowCurrencyChange()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get allow currency change", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If



            m_lReturn = m_oFolderBusiness.GetNext(vInsuranceFolderCnt:=m_lInsuranceFolderCnt, vInsuranceFolderID:=m_lInsuranceFolderId, vSourceId:=m_iFolderSourceId, vInsuranceHolderCnt:=m_lInsuranceHolderCnt, vCode:=m_sCode, vDescription:=m_vDescription, vInceptionDate:=m_vInceptionDate, vArcArchiveFolderID:=m_vArcArchiveFolderId, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_vRenewalCount)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the folder business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            m_lReturn = m_oSystemBusiness.GetNext(vInsuranceFileCnt:=m_lInsuranceFileCnt, vEndorsementCount:=m_lEndorsementCount, vCreatedByID:=m_iCreatedById, vDateCreated:=m_dtDateCreated, vModifiedById:=m_vModifiedById, vLastModified:=m_vLastModified, vLastTransDate:=m_vLastTransDate, vLastTransTypeID:=m_vLastTransType, vLastTransDescription:=m_vLastTransDescription, vLastTransDebitCredit:=m_vLastTransDebitCredit, vLastTransDocumentRef:=m_vLastTransDocumentRef, vLastTransCoverStartDate:=m_vLastTransCoverStartDate, vLastTransExpiryDate:=m_vLastTransExpiryDate)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the system business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'We may also want to get the policy relationship stuff...

            'Get additional details required for display that not stored on this record


            m_lReturn = m_oBusiness.GetOtherDetails(vInsurerCnt:=DBNull.Value, vInsurerName:=DBNull.Value, VBrokerCnt:=m_vBrokerCnt, vBrokerName:=m_sBrokerName, vRiskId:=m_vRiskCodeID, vRiskDesc:=m_sRiskDesc, vRiskGroupId:=m_vRiskGroupID, vAnalysisId:=m_vAnalysisCodeID, vAnalysisDesc:=m_sAnalysisDesc, vHandlerCnt:=m_vAccountHandlerCnt, vHandlerName:=m_sHandlerName, vAgentCnt:=m_vLeadAgentCnt, vAgentName:=m_sAgentName, vInsuranceFileCnt:=m_lInsuranceFileCnt, vRelatedPolicyCnt:=m_vRelatedPolicyCnt, vRelatedPolicyCode:=m_sRelatedPolicyCode, vRelationshipType:=m_sRelationshipType, vPolicyTypeId:=m_vPolicyTypeId, vPolicyTypeDesc:=m_sPolicyTypeDesc, vSchemeId:=m_lSchemeID, vSchemeDesc:=m_sSchemeDesc, vPolicyDeductiblesId:=m_vPolicyDeductiblesID, vPolicyDeductibles:=m_vPolicyDeductibles, vPolicyLimitsId:=m_vPolicyLimitsID, vPolicyLimits:=m_vPolicyLimits)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'TMP

            If Not (Convert.IsDBNull(m_vLeadAgentCnt) Or IsNothing(m_vLeadAgentCnt)) Then

                m_lReturn = m_oBusiness.IsAgentAllowCommissionUsingAgentCnt(v_vPartyCnt:=m_vLeadAgentCnt, r_vAgentAllowedCommission:=m_iAgentAllowedCommission)
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve whether Agent is allowed Commission or Not ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If
            End If


            If Not (Convert.IsDBNull(m_vAlternateAccountCnt) Or IsNothing(m_vAlternateAccountCnt)) Then
                'get alternative account name

                m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Party", v_vFieldName:="resolved_name", v_vKeyField:="party_cnt", v_vKeyID:=m_vAlternateAccountCnt, r_vResult:=m_sAlternateAccountName)


                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If
            End If

            'get sub agents for listview

            If m_oBusiness.GetSubAgents(v_vInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResultArray:=m_vAgentList) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'TN20000816 - Doc Ref 10 (End)


            'Get fees for the Policy

            m_lReturn = m_oBusiness.GetFeeDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt, vFee:=m_vFees)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the fee details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get StandardWordings for the Policy
            ' Start - Sankar - PN 61172

            m_lReturn = m_oBusiness.GetStandardWordings(vInsuranceFileCnt:=m_lInsuranceFileCnt, vStandardWordings:=m_vStandardWordings, v_lProductID:=m_lProductID, v_lSoruceId:=m_iSourceId)
            ' End - Sankar - PN 61172
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the standard wording details from the business object " &
                                   "", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            If m_bUseClientPolicyLinkage Then
                ' Get associated clients for listview

                m_lReturn = m_oBusiness.GetPolicyClient(v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lPartyCnt:=m_lPartyCnt, r_vResultArray:=m_vClientList)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the insured clients.", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
                End If
            End If

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

            m_bIsBackdatedMTAVersion = m_oAutoMTA.IsBackdatedMTARequired(m_lInsuranceFolderCnt, m_dtCoverStartDate, m_lInsuranceFileCnt)
            ''PN70860
            ''Note This was done for PN 46466 and condition should be specific for MTA only not for all like renewal
            If m_sTransactionType = "MTA" And m_bIsBackdatedMTAVersion Then
                txtCoverFromDate.Enabled = False
                txtCoverToDate.Enabled = False
            End If

            If CDbl(m_vLeadAgentCnt) <> 0 Then

                m_lReturn = m_oBusiness.GetAgentDetail(v_lAgentCnt:=m_vLeadAgentCnt, r_vResults:=vResults)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get default Lead Agent", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If
            If Information.IsArray(vResults) Then
                m_dtCommonRenewalDate = gPMFunctions.ToSafeDate(vResults(0, 0), DateTime.MinValue)
                m_sPartyCategoryCode = gPMFunctions.ToSafeString(vResults(1, 0))
                m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(vResults(2, 0))
            End If
            m_lReturn = AgentChangeLogic()
            'Start(Sriram P)PN 56443
            Dim vTransactionArray(,) As Object
            Dim bExpirydate As Boolean
            If m_sTransactionType = "MTA" And m_bIsMTATemp Then

                Dim temp_m_oTransactionBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTransactionBusiness, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oTransactionBusiness = temp_m_oTransactionBusiness

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRFindInsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                If m_lInsuranceFileTypeID <> 7 Then
                    m_lReturn = m_oTransactionBusiness.GetAllTransactionDates(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sUsername:=g_sUserName, r_vTransactionArray:=vTransactionArray, v_iSourceID:=m_iSourceId, v_iLanguageID:=m_iLanguageID)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get values from GetAllTransactionDates method", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                    If Information.IsArray(vTransactionArray) Then

                        For lCount As Integer = vTransactionArray.GetLowerBound(1) To vTransactionArray.GetUpperBound(1)
                            If m_dtCoverStartDate < gPMFunctions.ToSafeDate(vTransactionArray(0, lCount)) And Not bExpirydate Then
                                m_dtExpiryDate = gPMFunctions.ToSafeDate(vTransactionArray(0, lCount))
                                bExpirydate = True
                            End If
                        Next lCount
                    End If
                End If
            End If
            'End(Sriram P)PN 56443
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim cTemp As Decimal
        Dim sValue As String = ""
        Dim bIsDiscountApplied As Boolean
        Dim sSysOptValue As String = ""
        Const kMethodName As String = "BusinessToInterface"
        Dim v_PaymentMethod As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateShort, _
            ''        vFieldValue:=m_dtDDate)
            '
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            ' {* GENERATED CODE (Begin) *}

            m_bSetUp = True

            'TN20000817 - Doc Ref 10 (Start)

            'load sub agents onto listview
            If LoadAgents() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInsuredName, vControlValue:=m_vInsuredName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAlternateReference, vControlValue:=m_vAlternateReference)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOldPolicyNo, vControlValue:=m_vOldPolicyNumber)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtQuoteExpiryDate, vControlValue:=m_vQuoteExpiryDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtProposalDate, vControlValue:=m_vProposalDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20000817 - Doc Ref 10 (End)

            'This is what we SHOULD be using.
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyNumber, vControlValue:=m_sInsuranceRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't forget to set the status properly
            cboStatus.SelectedIndex = -1

            'This one comes from the insurance_file_system.
            If (m_lInsuranceFileTypeID = 2) Or (m_lInsuranceFileTypeID = 4) Or (m_lInsuranceFileTypeID = 7) Then
                chkQuote.CheckState = CheckState.Unchecked
            Else
                chkQuote.CheckState = CheckState.Checked
            End If

            If CDbl(m_iIsReferredOnMta) = 1 Then
                chkReferredOnMTA.CheckState = CheckState.Checked
            Else
                chkReferredOnMTA.CheckState = CheckState.Unchecked
            End If

            If CDbl(m_iIsReferredAtRenewal) = 1 Then
                chkReferredAtRenewal.CheckState = CheckState.Checked
            Else
                chkReferredAtRenewal.CheckState = CheckState.Unchecked
            End If


            lblCommAccount.Text = m_sBrokerName

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCoverFromDate, vControlValue:=m_dtCoverStartDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCoverToDate, vControlValue:=m_dtExpiryDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set txtInceptionDate same as txtInceptionTPI if business object value for txtInception date is nothing

            If Not (Convert.IsDBNull(m_vInceptionDate) Or IsNothing(m_vInceptionDate)) Then
                'This one comes from the Folder.
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInceptionDate,
                                                        vControlValue:=m_vInceptionDate)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf Not (Convert.IsDBNull(m_vInceptionTPI) Or IsNothing(m_vInceptionTPI)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInceptionDate, vControlValue:=m_vInceptionTPI)
            End If


            If Not (Convert.IsDBNull(m_vInceptionTPI) Or IsNothing(m_vInceptionTPI)) Then
                'This one comes from the Folder.
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInceptionTPI, vControlValue:=m_vInceptionTPI)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' If we are here, it means this is a quote which was created
                ' before this change, and the field in the DB is empty.
                ' We set it to the same as above (inception date).
                txtInceptionTPI.Text = txtInceptionDate.Text
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRenewalDate, vControlValue:=m_dtRenewalDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRegarding, vControlValue:=m_vLastTransDescription)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (Convert.IsDBNull(m_vDateIssued) Or IsNothing(m_vDateIssued)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIssuedDate, vControlValue:=m_vDateIssued)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'Developer Guide No. 51
            lblHandler.Text = m_sHandlerName
            If m_sHandlerName = "" Then
                chkHandler.Checked = False
            End If
            pnlPolicyType.Text = m_sPolicyTypeDesc
            pnlScheme.Text = m_sProductDesc
            'Start- Written Status
            If m_sTransactionType = "NB" Then
                If m_lInsuranceFileTypeID = 0 Then
                    pnlPolicyType.Text = ""
                ElseIf m_lInsuranceFileTypeID = 1 Then
                    pnlPolicyType.Text = "Quotation"
                ElseIf m_lInsuranceFileTypeID = 11 Then
                    pnlPolicyType.Text = "Written"
                End If
            End If
            'End Written Status
            pnlScheme.Text = m_sProductDesc
            ' Alix Bergeret - 11/12/2002 - Issue 1574

            pnlPolicyType.Text = m_sPolicyTypeDesc
            pnlScheme.Text = m_sProductDesc

            ' Alix Bergeret - 11/12/2002 - Issue 1574

            m_vNetPremium = gPMFunctions.NullToDouble(m_vNetPremium)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumExcTax, vControlValue:=m_vNetPremium)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            pnlAgentCode.Text = m_sAgentName
            pnlAgentCode.Tag = CStr(gPMFunctions.ToSafeLong(m_vLeadAgentCnt))
            '(RC) QBENZ014 ----------------------Start
            Dim vAltRefForEachTrans As Object
            If gPMFunctions.ToSafeString(pnlAgentCode.Text.Trim()) <> "" Then


                m_lReturn = m_oBusiness.GetFromTable("party_agent", "alternate_reference_for_each_transaction",
                                                     "party_cnt", m_vLeadAgentCnt, vAltRefForEachTrans)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If _
                    gPMFunctions.ToSafeBoolean(vAltRefForEachTrans, 0) And
                    (m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "MTC") And
                    m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    'If policy is edited before, dont remove previous AltRef
                    If gPMFunctions.ToSafeLong(m_vModifiedById) = 0 Then
                        m_oFormFields.Item("txtAlternateReference-0").Text = ""
                    End If
                Else
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAlternateReference,
                                                            vControlValue:=m_vAlternateReference)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If
            '(RC) QBENZ014 ----------------------End


            If Not (Convert.IsDBNull(m_vAnnualPremium) Or IsNothing(m_vAnnualPremium)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFutureAnnualPremium,
                                                        vControlValue:=m_vAnnualPremium)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Convert.IsDBNull(m_vLongTermUndertakingDate) Or IsNothing(m_vLongTermUndertakingDate)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPolicyLTUExpiryDate,
                                                        vControlValue:=m_vLongTermUndertakingDate)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'This one comes from the Folder.

            If Not (Convert.IsDBNull(m_vRenewalCount) Or IsNothing(m_vRenewalCount)) Then


                lblRenCount.Text = m_vRenewalCount
            End If

            'This one comes from the Policy Relationship table.


            lblRelatedPolicy.Text = m_sRelatedPolicyCode

            'Don't forget to set the relationship properly.  Which also comes from the
            'Policy relationship table.
            If m_sRelatedPolicyCode.Trim() = "" Then
                cboRelationship.SelectedIndex = -1
                cboRelationship.Enabled = False
            End If


            If Not (Convert.IsDBNull(m_vLapsedDate) Or IsNothing(m_vLapsedDate)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLapsedDate, vControlValue:=m_vLapsedDate)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' If cancellation, start date needs to be in this field
            If m_sTransactionType = "MTC" Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLapsedDate, vControlValue:=m_dtCoverStartDate)
            End If
            'PN 72680
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.2.1.1)
            If m_sTransactionType = "MTA" Then
                If (m_sSelectedPolicyStatus = "Cancelled" Or m_sSelectedPolicyStatus = "Lapsed") Then
                    cboPolicyStatus.Enabled = False
                    txtLapsedDate.Enabled = False
                End If
            End If
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.2.1.1)

            'Fill the Fee list view
            'PopulateFees


            If Convert.IsDBNull(m_vThisPremium) Or IsNothing(m_vThisPremium) Then
                cTemp = 0
            Else

                cTemp = CDec(m_vThisPremium)
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=cTemp + m_cTotalFees)


            'Fill the Narrative list view
            PopulateStandardWordings()

            'ECK 14/7/99 Populate hidden commission tab


            lblCommAccount.Text = m_sBrokerName


            If Not (Convert.IsDBNull(m_vCommissionPercentage) Or IsNothing(m_vCommissionPercentage)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage,
                                                        vControlValue:=m_vCommissionPercentage)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Convert.IsDBNull(m_vBrokerageAmount) Or IsNothing(m_vBrokerageAmount)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionCharge,
                                                        vControlValue:=m_vBrokerageAmount)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Convert.IsDBNull(m_vCommissionAmount) Or IsNothing(m_vCommissionAmount)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPayable,
                                                        vControlValue:=m_vCommissionAmount)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Convert.IsDBNull(m_vIsMinimumBrokerageFlag) Or IsNothing(m_vIsMinimumBrokerageFlag)) Then
                If (m_vIsMinimumBrokerageFlag) = gPMConstants.PMEReturnCode.PMTrue Then
                    txtCommissionPayable.Font = VB6.FontChangeBold(txtCommissionPayable.Font, True)
                Else
                    txtCommissionPayable.Font = VB6.FontChangeBold(txtCommissionPayable.Font,
                                                                   gPMConstants.PMEReturnCode.PMFalse)
                End If
            Else
                txtCommissionPayable.Font = VB6.FontChangeBold(txtCommissionPayable.Font,
                                                               gPMConstants.PMEReturnCode.PMFalse)
            End If


            If CDbl(m_vIsInsurerRateTable) = 1 Then
                chkOverrideRateTable.CheckState = CheckState.Checked
            Else
                chkOverrideRateTable.CheckState = CheckState.Unchecked
            End If

            If chkOverrideRateTable.CheckState = CheckState.Checked Then
                txtCommissionPercentage.Enabled = True
                txtCommissionCharge.Enabled = True
            Else
                txtCommissionPercentage.Enabled = False
                txtCommissionCharge.Enabled = False
            End If

            ' if this is new business ignore the
            ' product risk option for currency change
            If m_sTransactionType <> "NB" Then

                cboCurrency.Enabled = m_lCurrencyChange = CHANGECURRENCYTRUE

            Else

                cboCurrency.Enabled = True

            End If

            txtAnniversaryDate.Text = DateTime.Parse(m_dtAnniversaryDate).ToString("D")
            If m_lRenewalDayNumber <> 0 Then
                SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
            End If

            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
            m_lReturn = GetSystemOption(v_iOptionNumber:=ACSysOptionNextInstalmentRenewal, r_sResult:=sSysOptValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName,
                                        "GetSystemOption Failed to retrieve NextInstalmentRenewal Enabled:" &
                                        ACSysOptionNextInstalmentRenewal, gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_m_oBusinessListRisk As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusinessListRisk, "bSIRListRisks.Business",
                                                     vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusinessListRisk = temp_m_oBusinessListRisk
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRListRisks.Business",
                                        gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oBusinessListRisk.GetPaymentMethod(m_lInsuranceFileCnt, v_PaymentMethod)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPaymentMethod Failed for:" & m_lInsuranceFileCnt,
                                        gPMConstants.PMELogLevel.PMLogError)
            End If


            m_oBusinessListRisk.Dispose()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRListRisks.Business Failed",
                                        gPMConstants.PMELogLevel.PMLogError)
            End If

            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

            If m_lPutOnNextInstalmentRenewal = 1 Then
                chkPutOnNextInstalmentRenewal.CheckState = CheckState.Checked
                'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
            Else

                If Not gPMFunctions.IsArrayEmpty(v_PaymentMethod) Then

                    If _
                        sSysOptValue = "1" And m_bIsTrueMonthlyPolicy And m_sTransactionType = "MTA" And
                        CStr(v_PaymentMethod(0, 0)) <> "Invoice" Then
                        m_bIsTrueMonthlypolicyandNextInstalmentRenewal = True
                        chkPutOnNextInstalmentRenewal.CheckState = CheckState.Checked
                        chkPutOnNextInstalmentRenewal.Visible = True
                        lblPutOnNextInstalmentRenewal.Visible = True
                    End If
                End If
                'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
            End If

            m_bSetUp = False

            ' {* GENERATED CODE (End) *}

            ' {* USER DEFINED CODE (Start) *}

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            If m_bUseClientPolicyLinkage Then
                ' Load insured clients into the listview
                m_lReturn = LoadInsuredClients()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)


            ' Assign policy status to interface, default if not in DB

            If Not (Convert.IsDBNull(m_vPolicyStatusID) Or IsNothing(m_vPolicyStatusID)) Then

                cboPolicyStatus.ItemId = CInt(m_vPolicyStatusID)
            End If


            If Not (Convert.IsDBNull(m_vPolicyDeductiblesID) Or IsNothing(m_vPolicyDeductiblesID)) Then

                cboPolicyDeductible.ItemId = CInt(m_vPolicyDeductiblesID)
            End If


            If Not (Convert.IsDBNull(m_vPolicyLimitsID) Or IsNothing(m_vPolicyLimitsID)) Then

                cboPolicyLimits.ItemId = CInt(m_vPolicyLimitsID)
            End If

            m_lReturn = SetUnderwritingYear()

            ' determine if policy discount is active
            m_lReturn = GetPolicyDiscountActive(r_bPolicyDiscountEnabled:=m_bIsPolicyDiscountActive)
            'Enable the Policy Discount Frame
            If Not m_bIsPolicyDiscountActive Then
                'disable policy discount functionality
                SetupPolicyDiscountInterface(v_bEnabled:=False)
            Else
                If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Then

                    m_lReturn = IsDiscountApplied(m_lInsuranceFileCnt, bIsDiscountApplied)

                    If bIsDiscountApplied Then
                        ' enable policy discount functionality
                        SetupPolicyDiscountInterface(v_bEnabled:=False)
                        cboDiscountReason.ItemId = m_iDiscountReasonId
                        txtDiscountedPremium.Text = StringsHelper.Format(m_crDiscountedPremium, kGenericCurrencyFormat)
                        txtDiscountPercentage.Text = StringsHelper.Format(m_dDiscountPercentage,
                                                                          kDiscountPercentageFormat)
                        cboDiscountRecurringType.ItemId = m_lDiscountRecurringTypeID
                    Else
                        ' enable policy discount functionality
                        SetupPolicyDiscountInterface(v_bEnabled:=True)
                        ' setup policy discount defaults / values
                        SetupPolicyDiscount()
                    End If

                Else
                    ' enable policy discount functionality
                    SetupPolicyDiscountInterface(v_bEnabled:=True)
                    ' setup policy discount defaults / values
                    SetupPolicyDiscount()
                End If
            End If

            '--RFC-PLICO14- Amit

            If Not (Convert.IsDBNull(m_dManualDiscountPercentage) Or IsNothing(m_dManualDiscountPercentage)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtManualDiscountPercentage,
                                                        vControlValue:=m_dManualDiscountPercentage)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If Not (Convert.IsDBNull(m_sCoInsPlacement) Or IsNothing(m_sCoInsPlacement)) Then
                If m_sCoInsPlacement.Trim = "GROSS" Then
                    optGross.Checked = True
                ElseIf m_sCoInsPlacement.Trim = "NETT" Then
                    optNet.Checked = True
                End If
            End If

            If m_sTransactionType = "MTA" Then
                Dim vResultArray(,) As Object = Nothing

                m_bPolicyIsInRenewal = IsPolicyInRenewal(m_lInsuranceFolderCnt, vResultArray)
                m_bPolicyIsInRenewal = Not vResultArray Is Nothing

                If m_bPolicyIsInRenewal Then

                    If Information.IsArray(vResultArray) AndAlso vResultArray.Length > 0 Then
                        m_lRenewalInsuranceFileKey = vResultArray(0, 0)
                        m_sRenewalVersionStartDate = vResultArray(1, 0)
                    End If

                    If (m_sRenewalVersionStartDate <> String.Empty) Then
                        m_sRequiredCoverEndDate = If(m_iMidnightRenewal, DateAdd(DateInterval.Day, -1, CDate(m_sRenewalVersionStartDate)), CDate(m_sRenewalVersionStartDate))
                    End If

                End If
                Dim oDoNotDeleteRenQuoteOnMta As Object = Nothing
                m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Product", v_vFieldName:="do_not_delete_renQuote_on_mta",
                                                                    v_vKeyField:="product_id", v_vKeyID:=m_lProductID,
                                                                    r_vResult:=oDoNotDeleteRenQuoteOnMta)

                If Not IsNothing(oDoNotDeleteRenQuoteOnMta) Then
                    m_bDoNotDeleteRenewalQuoteOnMTA = ToSafeBoolean(oDoNotDeleteRenQuoteOnMta)
                Else
                    m_bDoNotDeleteRenewalQuoteOnMTA = False
                End If
            End If


            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function IsDiscountApplied(ByVal v_lInsuranceFileCnt As Integer, ByRef bIsDiscountApplied As Boolean) As Integer
        Dim result As Integer = 0
        Dim bSIRListRisks As Object
        Const kMethodName As String = "IsDiscountApplied"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            Dim oBusiness As bSIRListRisks.Business
            Dim vIsdiscountApplied As Object
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "IsDiscountFailed Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = oBusiness.IsDiscountApplied(v_lInsuranceFileCnt, vIsdiscountApplied)

            bIsDiscountApplied = gPMFunctions.ToSafeInteger(vIsdiscountApplied, 0) = 1

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "IsDiscountApplied Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    'EK 20/10/99
    ' ***************************************************************** '
    ' Name: CheckCoInsurers
    '
    ' Description: checks whether a MULTI policy has been               '
    ' made single insurer
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckCoInsurers) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckCoInsurers() As Integer
    '
    'Dim result As Integer = 0
    'Dim sDescription As String = ""
    'Dim vDescription As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_sInsurerName <> "MULTI" Then
    'Return result
    'End If
    '

    'm_lReturn = m_oBusiness.DeleteCoInsurers(vInsuranceFileCnt:=m_lInsuranceFileCnt)
    '
    'TN20000807 set flag to create event
    'PMRaiseEvent = True
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCoInsurers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCoInsurers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: DeleteClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DeleteClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = DeletePolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Need to keep locked, so let's get out
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeletePolicy
    '
    ' Description:
    '
    ' History: 17/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DeletePolicy() As Integer

        Dim result As Integer = 0
        Dim bOKToDelete As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt


            bOKToDelete = m_oBusiness.OKToDelete()

            If Not bOKToDelete Then


                System.Windows.Forms.MessageBox.Show("Cannot delete -" & Strings.Chr(13) & Strings.Chr(10) & m_oBusiness.NoDeleteReasons, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            m_lReturn = m_oBusiness.DeletePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.

            Dim ctlFormControl As System.Windows.Forms.Control
            For Each ctlFormControl In Me.Controls
                If (TypeOf ctlFormControl Is System.Windows.Forms.TabControl) Then
                    For Each ctlTabPage As Control In ctlFormControl.Controls
                        If (TypeOf ctlTabPage Is System.Windows.Forms.TabPage) Then
                            For Each ctl As Control In ctlTabPage.Controls
                                If (TypeOf ctl Is GroupBox) Then
                                    For Each ctlChild As Control In ctl.Controls
                                        If (TypeOf ctlChild Is System.Windows.Forms.TextBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.ComboBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.CheckBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.RadioButton) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is PMLookupControl.cboPMLookup) Then
                                            ctlChild.Enabled = Not lDisabled
                                        End If
                                    Next
                                End If
                                'Check the type of the control.
                                If (TypeOf ctl Is System.Windows.Forms.TextBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.ComboBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.CheckBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.RadioButton) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is PMLookupControl.cboPMLookup) Then
                                    ctl.Enabled = Not lDisabled
                                End If
                            Next
                        End If
                    Next

                End If
            Next

            'Now the command buttons...
            'TN20000814 - Doc Ref 10    cmdInsurerLookup.Enabled = Not lDisabled
            'TN20010219 cmdCommission.Enabled = Not lDisabled
            cmdHandler.Enabled = Not lDisabled
            'TN20000814 - Doc Ref 10    cmdAgent.Enabled = Not lDisabled
            cmdAddPolicyWording.Enabled = Not lDisabled
            cmdUpNarrative.Enabled = Not lDisabled
            cmdDownNarrative.Enabled = Not lDisabled
            cmdRelatedPolicy.Enabled = Not lDisabled
            lvwPolicyWording.Enabled = Not lDisabled
            'TN20000823 - Doc Ref 10

            'JMK 15/05/2001 - Prevent Adding SubAgents
            cmdAddAgent.Enabled = Not lDisabled

            cmdDeleteAgent.Enabled = Not lDisabled
            cmdEditAgent.Enabled = Not lDisabled

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' DisplayCaptions
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function DisplayCaptions() As Integer

        Dim nResult As Integer = 0
        Dim vValue As String = ""

        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.7.1.1)
        Dim iLanguageId As Integer
        'In all instances where the GetResData function is called the g_iLanguageID% is replaced with iLanguageId%
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.7.1.1)
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.7.1.1)
            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.7.1.1)

            ' Display all language specific captions.

            'This is how it should be done.
            'Don't forget to update it with all the controls on this control

            'Developer Guide No. 55
            SSTabHelper.SetTabCaption(tabMainTab, 0,
                                      iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. 55
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 2,
                                      iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle4,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))

            'TN20000816 - Doc Ref 10 (Start)


            'Developer Guide No. 26
            lblInsuredName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapInsuredName,
                                                          iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                          bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblProduct.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapProduct,
                                                      iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblBranchCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapBranchCode,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            cmdAgentCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAgentCode,
                                                        iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                        bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblAlternateReference.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapAlternateReference,
                                                                 iDataType:=
                                                                    gPMConstants.PMEResourseFileDataType.PMResString,
                                                                 bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblAnalysisCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapAnalysisCode,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))

            'Developer Guide No. 26
            lblQuoteExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapQuoteExpiryDate,
                                                              iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                              bResFile:=My.Resources.ResourceManager))

            ' Check which label to use here
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:=
                                      "Failed to get product option " &
                                      gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If

            If vValue = "1" Then

                'Developer Guide No. 26
                lblProposalDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapHoldCoverExpiryDate,
                                                               iDataType:=
                                                                  gPMConstants.PMEResourseFileDataType.PMResString,
                                                               bResFile:=My.Resources.ResourceManager))
            Else

                'Developer Guide No. 26
                lblProposalDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapProposalDate,
                                                               iDataType:=
                                                                  gPMConstants.PMEResourseFileDataType.PMResString,
                                                               bResFile:=My.Resources.ResourceManager))
            End If


            'Developer Guide No. 26
            lblInceptionTPI.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapInceptionTPI,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraAgents.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapFrameAgent,
                                                     iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                     bResFile:=My.Resources.ResourceManager))

            'TN20000816 - Doc Ref 10 (End)


            'Developer Guide No. 26
            fraRisks.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRisks,
                                                    iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                    bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPolicyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicyNo,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRegarding.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRegarding,
                                                        iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                        bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStatus,
                                                     iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                     bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            chkQuote.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACQuote,
                                                    iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                    bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraDates.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDates,
                                                    iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                    bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPolicyDeductible.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicyDeductible,
                                                               iDataType:=
                                                                  gPMConstants.PMEResourseFileDataType.PMResString,
                                                               bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPolicyLimits.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicyLimits,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCoverFromDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCoverFrom,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblInceptionDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInception,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCoverToDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCoverTo,
                                                          iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                          bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRenewalDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRenewal,
                                                          iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                          bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPaymentMethodText,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStatus2,
                                                     iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                     bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPolicyType.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicyType,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblScheme.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACSchemeText,
                                                     iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                     bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraSource.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACSourceRenewalInformation,
                                                     iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                     bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblFrequency.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFrequency,
                                                        iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                        bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblPolicyLTUExpiryDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLTUExpiry,
                                                                  iDataType:=
                                                                     gPMConstants.PMEResourseFileDataType.PMResString,
                                                                  bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRenewalStop.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStopReason,
                                                          iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                          bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRelationship.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRelationship,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblLapsedReason.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLapseReason,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblLapsedDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLapsedDateText,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraNarrations.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStandardPolicyWording,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            cmdNext(0).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNext,
                                                      iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                      bResFile:=My.Resources.ResourceManager))

            cmdNext(1).Text = cmdNext(0).Text


            'Developer Guide No. 26
            cmdPrevious(0).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPrevious,
                                                          iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                          bResFile:=My.Resources.ResourceManager))

            cmdPrevious(1).Text = cmdPrevious(0).Text


            'Developer Guide No. 26
            cmdHandler.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACHandler,
                                                      iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            cmdRelatedPolicy.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRelatedPolicy,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCommissionAccount.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommAccount,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCommissionPremium.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommPremium,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCommissionPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommPercentage,
                                                                   iDataType:=
                                                                      gPMConstants.PMEResourseFileDataType.PMResString,
                                                                   bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCommissionCharge.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommCharge,
                                                               iDataType:=
                                                                  gPMConstants.PMEResourseFileDataType.PMResString,
                                                               bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblCommissionPayable.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommPayable,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblOverride.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCommOverride,
                                                       iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                       bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblReferredAtRenewal.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACReferredAtRenewal,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblReferredOnMTA.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACReferredOnMTA,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRenewalMethod.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACRenewalMethod,
                                                            iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                            bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblBusinessType.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACBusinessType,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblIssuedDate.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACIssuedDate,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            ' Set tab and frame

            'Developer Guide No. 26
            SSTabHelper.SetTabCaption(tabMainTab, 4,
                                      iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle5,
                                                         iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                         bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            fraClients.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPFrame,
                                                      iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                      bResFile:=My.Resources.ResourceManager))

            ' Set listview column headers


            'Developer Guide No. 26
            lvwClients.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPLVCol0,
                                                                      iDataType:=
                                                                         gPMConstants.PMEResourseFileDataType.PMResString,
                                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lvwClients.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPLVCol1,
                                                                      iDataType:=
                                                                         gPMConstants.PMEResourseFileDataType.PMResString,
                                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lvwClients.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPLVCol2,
                                                                      iDataType:=
                                                                         gPMConstants.PMEResourseFileDataType.PMResString,
                                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lvwClients.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPLVCol3,
                                                                      iDataType:=
                                                                         gPMConstants.PMEResourseFileDataType.PMResString,
                                                                      bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lvwClients.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPLVCol4,
                                                                      iDataType:=
                                                                         gPMConstants.PMEResourseFileDataType.PMResString,
                                                                      bResFile:=My.Resources.ResourceManager))

            ' Set command buttons

            'Developer Guide No. 26
            cmdAddClient.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddButton,
                                                        iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                        bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            cmdDeleteClient.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDeleteButton,
                                                           iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                           bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            cmdSetCorrespondence.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCPSetCrspnd,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))

            ' Simple one for these
            cmdNext(3).Text = cmdNext(0).Text
            cmdPrevious(4).Text = cmdPrevious(1).Text
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

            'Policy Discount

            'Developer Guide No. 26
            fraDiscount.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapFrameDiscount,
                                                       iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                       bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblDiscountReason.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapDiscountReason,
                                                             iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                             bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblDiscountedPremium.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapDiscountedPremium,
                                                                iDataType:=
                                                                   gPMConstants.PMEResourseFileDataType.PMResString,
                                                                bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblRecurring.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapRecurring,
                                                        iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                        bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblDiscountPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCapDiscountPercentage,
                                                                 iDataType:=
                                                                    gPMConstants.PMEResourseFileDataType.PMResString,
                                                                 bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 26
            lblManualDiscountPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId,
                                                                       lId:=ACCapManualDiscountPercentage,
                                                                       iDataType:=
                                                                          gPMConstants.PMEResourseFileDataType.PMResString,
                                                                       bResFile:=My.Resources.ResourceManager))

            optGross.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=kCoInsHandlingGross,
                                                  iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                  bResFile:=My.Resources.ResourceManager))

            optNet.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=kCoInsHandlingNet,
                                                  iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                  bResFile:=My.Resources.ResourceManager))
            lblCoinsurancePlacement.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=kCoinsurancePlacement,
                                                             iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                             bResFile:=My.Resources.ResourceManager))

            Return nResult

        Catch excep As System.Exception

            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Displays all of the lookup details using the lookup values/details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayLookupDetails() As Integer
        Dim nResult As Integer
        Dim nRow As Integer
        Dim oDefaultCurrency As Object
        Dim oBusinessSourceDef As bPMUSourceDefaults.Business
        Dim nDirectBusiness As Integer
        Dim nAgentID As Integer
        Dim oAgentName As Object
        Dim sDefaultPolicyStyleID As String = ""
        Dim oIsRenewable As Object
        Dim oResults As Object
        Dim oPartyAG As bSIRPartyAG.Business
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = g_oObjectManager.GetInstance(oPartyAG, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            'For this one null means live.  So let's see...
            Dim cboStatus_NewIndex As Integer = -1
            cboStatus_NewIndex = cboStatus.Items.Add("Current")
            VB6.SetItemData(cboStatus, cboStatus_NewIndex, 0)

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupInsuranceFileStatus, ctlLookup:=cboStatus)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bSetQuoteToLapsed Then
                For nRow = 0 To cboStatus.Items.Count - 1
                    If VB6.GetItemData(cboStatus, nRow) = 2 Then
                        cboStatus.SelectedIndex = nRow
                        Exit For
                    End If
                Next nRow

            ElseIf (cboStatus.SelectedIndex = -1) Then
                cboStatus.SelectedIndex = 0
            End If

            ' Default "policy status" value to "status" value (when new policy)

            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or Convert.IsDBNull(m_vPolicyStatusID) Or
                IsNothing(m_vPolicyStatusID) Then
                For nRow = 0 To cboPolicyStatus.ListCount - 1
                    If cboPolicyStatus.ItemCaption(cboPolicyStatus.ItemData(nRow)) = cboStatus.Text Then
                        cboPolicyStatus.ListIndex = nRow
                        Exit For
                    End If
                Next nRow
            End If


            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or Convert.IsDBNull(m_vPolicyDeductiblesID) Or
                IsNothing(m_vPolicyDeductiblesID) Then
                cboPolicyDeductible.FirstItem = "None"
                cboPolicyDeductible.ListIndex = 0
            End If


            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or Convert.IsDBNull(m_vPolicyLimitsID) Or
                IsNothing(m_vPolicyLimitsID) Then
                cboPolicyLimits.FirstItem = "None"
                cboPolicyLimits.ListIndex = 0

            End If

            nRow = 0

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalFrequency, ctlLookup:=cboFrequency)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Default the renewal frequency to annual
            'EK 12/12/99 - Not any more we don't
            'CF 08/02/00 - We do again, but just for PMAdd

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                For nRow = 0 To cboFrequency.Items.Count
                    If VB6.GetItemString(cboFrequency, nRow) = "Annually" Or
                        VB6.GetItemString(cboFrequency, nRow).ToUpper() = "ANNUAL" Then
                        cboFrequency.SelectedIndex = nRow
                    End If
                Next nRow
            Else
                'PN: 56793, Set the saved Frequency
                For nRow = 0 To cboFrequency.Items.Count - 1
                    If VB6.GetItemData(cboFrequency, nRow) = gPMFunctions.ToSafeInteger(m_iRenewalFrequencyID) Then
                        cboFrequency.SelectedIndex = nRow
                        Exit For
                    End If
                Next nRow
                If cboFrequency.SelectedIndex = -1 Then
                    cboFrequency.SelectedIndex = 0
                End If
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalStopCode, ctlLookup:=cboRenewalStop)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalMethod, ctlLookup:=cboRenewalMethod)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupBusinessType, ctlLookup:=cboBusinessType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck230500

            'TN20010125 Start
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'default Business type to Agency Business
                For nRow = 0 To cboBusinessType.Items.Count - 1
                    If VB6.GetItemData(cboBusinessType, nRow) = 7 Then
                        cboBusinessType.SelectedIndex = nRow
                        cboBusinessType_SelectionChangeCommitted(Nothing, Nothing)
                        If (cboBusinessType.Text.Trim() = "Co-Insurance Lead") OrElse (cboBusinessType.Text.Trim() = "Co-Insurance Follow") Then
                            grpCoInsuranceLead.Visible = True
                            lblCoinsurancePlacement.Visible = True
                            If m_sCoInsPlacement = "GROSS" Then
                                optGross.Checked = True
                            ElseIf m_sCoInsPlacement = "NETT" Then
                                optNet.Checked = True
                            End If
                        Else
                            grpCoInsuranceLead.Visible = False
                            lblCoinsurancePlacement.Visible = False
                        End If
                    End If
                Next nRow
                '        cboBusinessType.ListIndex = 6
            Else
                'PN: 56220, Set the saved business type
                For nRow = 0 To cboBusinessType.Items.Count - 1
                    If VB6.GetItemData(cboBusinessType, nRow) = gPMFunctions.ToSafeInteger(m_vBusinessTypeId) Then
                        cboBusinessType.SelectedIndex = nRow
                        cboBusinessType_SelectionChangeCommitted(Nothing, Nothing)
                        If (cboBusinessType.Text.Trim() = "Co-Insurance Lead") OrElse (cboBusinessType.Text.Trim() = "Co-Insurance Follow") Then
                            grpCoInsuranceLead.Visible = True
                            lblCoinsurancePlacement.Visible = True
                            If m_sCoInsPlacement = "GROSS" Then
                                optGross.Checked = True
                            ElseIf m_sCoInsPlacement = "NETT" Then
                                optNet.Checked = True
                            End If
                        Else
                            grpCoInsuranceLead.Visible = False
                            lblCoinsurancePlacement.Visible = False
                        End If
                    End If
                Next nRow
                If cboBusinessType.SelectedIndex = -1 Then
                    cboBusinessType.SelectedIndex = 0
                    cboBusinessType_SelectionChangeCommitted(Nothing, Nothing)
                    If (cboBusinessType.Text.Trim() = "Co-Insurance Lead") OrElse (cboBusinessType.Text.Trim() = "Co-Insurance Follow") Then
                        grpCoInsuranceLead.Visible = True
                        lblCoinsurancePlacement.Visible = True
                        If m_sCoInsPlacement = "GROSS" Then
                            optGross.Checked = True
                        ElseIf m_sCoInsPlacement = "NETT" Then
                            optNet.Checked = True
                        End If
                    Else
                        grpCoInsuranceLead.Visible = False
                        lblCoinsurancePlacement.Visible = False
                    End If
                End If
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPolicyRelationshipType,
                                         ctlLookup:=cboRelationship)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupLapsedReason, ctlLookup:=cboLapsedReason)

            'Set default value to cboLapseReason if IsRenewable is true in product
            'Task : Renewal Back Office changes

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Product", v_vFieldName:="is_renewable",
                                                     v_vKeyField:="product_id", v_vKeyID:=m_lProductID,
                                                     r_vResult:=oIsRenewable)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If oIsRenewable = 0 Then
                    For iCnt As Integer = 0 To cboLapsedReason.Items.Count - 1
                        If VB6.GetItemString(cboLapsedReason, iCnt).Trim().ToUpper() = ("Non Renewable").ToUpper() Then
                            cboLapsedReason.SelectedIndex = iCnt
                            Exit For
                        End If
                    Next
                End If
            End If



            If m_sTransactionType = "MTR" Then
                cboLapsedReason.Text = ""
                txtLapsedDate.Text = ""
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCurrency, ctlLookup:=cboCurrency)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20000815 - Doc Ref 10 (Start)

            'setting default for currency
            If Task = gPMConstants.PMEComponentAction.PMAdd Then

                'default to party currency or system currency

                m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Party", v_vFieldName:="currency_id",
                                                     v_vKeyField:="party_cnt", v_vKeyID:=m_lPartyCnt,
                                                     r_vResult:=oDefaultCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get default currency from Party table", vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="DisplayLookupDetails",
                                       vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return nResult

                End If

                'default to system currency if not found one in party

                If Convert.IsDBNull(oDefaultCurrency) Or IsNothing(oDefaultCurrency) Then

                    oDefaultCurrency = g_iCurrencyID
                End If
            Else
                'use currency from insurance file

                oDefaultCurrency = m_iCurrencyID
            End If

            'highlight default currency
            For nRow = 0 To cboCurrency.Items.Count - 1

                If CInt(oDefaultCurrency) = VB6.GetItemData(cboCurrency, nRow) Then
                    cboCurrency.SelectedIndex = nRow
                    Exit For
                End If
            Next


            ' ----------- get more lookups ------------

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupAnalysisCode, ctlLookup:=cboAnalysisCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' When doing an MTA, the current policy could be against a closed
            ' branch (marked as deleted in the database). The normal GetLookupDetails
            ' function doesn't return deleted branches, so we need a new one
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = GetBranches()
            Else
                m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSource, ctlLookup:=cboBranchCode)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002 default branch to user's branch if adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                For nRow = 0 To cboBranchCode.Items.Count - 1
                    If VB6.GetItemData(cboBranchCode, nRow) = m_iSourceId Then
                        cboBranchCode.SelectedIndex = nRow
                        Exit For
                    End If
                Next nRow
            End If

            'sj 19/07/2002 - start
            m_bUserMode = True
            'sj 19/07/2002 - end

            'sj 19/07/2002 - start
            m_lReturn = GetSubBranchDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 19/07/2002 - end

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupProduct, ctlLookup:=cboProduct)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                For nRow = 0 To cboProduct.Items.Count - 1
                    If VB6.GetItemData(cboProduct, nRow) = m_lProductID Then
                        cboProduct.SelectedIndex = nRow
                        Exit For
                    End If
                Next nRow
            End If

            'setting default for analysis code, branch code and product code
            If Task <> gPMConstants.PMEComponentAction.PMAdd Then

                'analysis code
                For nRow = 0 To cboAnalysisCode.Items.Count - 1

                    If CDbl(m_vAnalysisCodeID) = VB6.GetItemData(cboAnalysisCode, nRow) Then
                        cboAnalysisCode.SelectedIndex = nRow
                        Exit For
                    End If
                Next


                'branch code
                For nRow = 0 To cboBranchCode.Items.Count - 1
                    'Branch isn't branch - it's source
                    '            If m_iBranchID = cboBranchCode.ItemData(i%) Then
                    If m_iSourceId = VB6.GetItemData(cboBranchCode, nRow) Then

                        m_bIgnoreHandler = True

                        cboBranchCode.SelectedIndex = nRow
                        Exit For
                    End If
                Next

                'Renewal Frequency
                For nRow = 0 To cboFrequency.Items.Count - 1
                    If m_iRenewalFrequencyID = VB6.GetItemData(cboFrequency, nRow) Then
                        cboFrequency.SelectedIndex = nRow
                        Exit For
                    End If
                Next

                'Renewal Method
                For nRow = 0 To cboRenewalMethod.Items.Count - 1

                    If CDbl(m_vRenewalMethodID) = VB6.GetItemData(cboRenewalMethod, nRow) Then
                        cboRenewalMethod.SelectedIndex = nRow
                        Exit For
                    End If
                Next

                'Lapsed Reason
                For nRow = 0 To cboLapsedReason.Items.Count - 1

                    If CDbl(m_vLapsedReasonID) = VB6.GetItemData(cboLapsedReason, nRow) Then
                        cboLapsedReason.SelectedIndex = nRow
                        Exit For
                    End If
                Next

                'Renewal Stop Reason
                For nRow = 0 To cboRenewalStop.Items.Count - 1

                    If CDbl(m_vRenewalStopCodeID) = VB6.GetItemData(cboRenewalStop, nRow) Then
                        cboRenewalStop.SelectedIndex = nRow
                        Exit For
                    End If
                Next

                If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 1 Then
                    pnlAgentCode.Text = "" & m_sAgentName
                End If

                If m_sUnderwritingType = "U" Then

                    If CStr(m_vLeadAgentCnt) <> "" Then

                        m_lReturn = AddAssociatedSubAgent(v_lLeadAgentCnt:=CInt(m_vLeadAgentCnt))
                    End If
                End If

                'product code
                For nRow = 0 To cboProduct.Items.Count - 1
                    If m_lProductID = VB6.GetItemData(cboProduct, nRow) Then
                        cboProduct.SelectedIndex = nRow
                        Exit For
                    End If
                Next
                '
                'PW311002 - Get the branch defaults - start
                '
            Else
                ' Get an instance of the source defaults business object via
                ' the public object manager.
                Dim temp_oBusinessSourceDef As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusinessSourceDef, "bPMUSourceDefaults.Business",
                                                         vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusinessSourceDef = temp_oBusinessSourceDef

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    System.Windows.Forms.MessageBox.Show("Failed to get the Branch Defaults - could not create business object.", ACApp,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return nResult
                End If

                ' Get the branch defaults

                m_lReturn = oBusinessSourceDef.GetSourceDefaults(v_lSourceID:=m_iSourceId,
                                                                 r_iDirectBusiness:=nDirectBusiness,
                                                                 r_lAgentID:=nAgentID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oBusinessSourceDef.Dispose()
                    oBusinessSourceDef = Nothing
                    System.Windows.Forms.MessageBox.Show("Failed to get the Branch Defaults - GetSourceDefaults method failed.", ACApp, MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation)
                    Return nResult
                End If

                ' Terminate the source defaults business object

                oBusinessSourceDef.Dispose()
                oBusinessSourceDef = Nothing

                ' Set the business type combo to direct if the defaults say so
                If nDirectBusiness = 1 Then
                    For nRow = 0 To cboBusinessType.Items.Count - 1
                        If VB6.GetItemData(cboBusinessType, nRow) = 1 Then
                            cboBusinessType.SelectedIndex = nRow
                            cboBusinessType_SelectionChangeCommitted(Nothing, Nothing)
                        End If
                    Next nRow
                Else
                    'Set the default agent if there is one on the source defaults
                    'TMP - if there is no default lead agent for the product then default it with branch default agent

                    If CDbl(m_vLeadAgentCnt) = 0 Then
                        If nAgentID <> 0 Then
                            ' Get the agent name

                            m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Party", v_vFieldName:="name",
                                                                 v_vKeyField:="party_cnt", v_vKeyID:=nAgentID,
                                                                 r_vResult:=oAgentName)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="Failed to get agent name from Party table", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="DisplayLookupDetails",
                                                   vErrNo:=Information.Err().Number,
                                                   vErrDesc:=Information.Err().Description)
                                Return nResult
                            End If
                            ' Set the agent panel up
                            pnlAgentCode.Tag = CStr(nAgentID)

                            m_vLeadAgentCnt = nAgentID

                            If oAgentName IsNot Nothing Then
                                m_sAgentName = gPMFunctions.ToSafeString(oAgentName).Trim()
                            End If

                            pnlAgentCode.Text = m_sAgentName

                            pnlAgentCode.Text = CStr(oAgentName).Trim()

                            'Add Associated Sub-Agents in Listview if business type is Agency Business
                            If m_sUnderwritingType = "U" Then

                                If CStr(m_vLeadAgentCnt) <> "" Then

                                    m_lReturn = AddAssociatedSubAgent(v_lLeadAgentCnt:=CInt(m_vLeadAgentCnt))
                                End If
                            End If

                        End If
                    End If
                End If
            End If

            'DJM 29/03/2004
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or m_lPolicyStyleID = -1 Then

                m_lReturn = m_oBusiness.GetProductDetails(v_lProductID:=m_lProductID, v_lOption:=34,
                                                          r_vValue:=sDefaultPolicyStyleID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sDefaultPolicyStyleID <> "" Then
                    m_lPolicyStyleID = CInt(sDefaultPolicyStyleID)
                Else
                    m_lPolicyStyleID = 0
                End If
            End If

            cboPolicyStyle.ItemId = m_lPolicyStyleID

            'TMP Setting the user interface
            chkConsolidatedLeadCommission.CheckState = m_iLeadAllowConsolidatedCommission
            chkConsolidatedSubCommission.CheckState = m_iSubAllowConsolidatedCommission
            Dim iSourceId As Integer
            If cboBranchCode.SelectedIndex >= 0 Then
                iSourceId = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'default Business type to Agency Business
                For nRow = 0 To cboBusinessType.Items.Count - 1
                    If VB6.GetItemData(cboBusinessType, nRow) = 7 Then
                        m_oBusinessPolicy.GetAssociatedAgent(r_iUserID:=g_iUserId, m_vAgentArray:=oResults)
                        If Information.IsArray(oResults) Then

                            cboBusinessType.SelectedIndex = nRow
                            pnlAgentCode.Tag = CStr(oResults(0, 0))

                            pnlAgentCode.Text = CStr(oResults(1, 0))
                            pnlAgentCode.Enabled = False
                            cboBusinessType.Enabled = False
                            cmdAgentCode.Enabled = False
                        ElseIf oAgentName = "" And m_vLeadAgentCnt = 0 Then
                            pnlAgentCode.Text = ""
                            pnlAgentCode.Tag = CStr(0)
                            pnlAgentCode.Enabled = True
                            cboBusinessType.Enabled = True

                        End If
                    End If
                Next nRow
            End If

            For nRow = 0 To cboBusinessType.Items.Count - 1
                If VB6.GetItemData(cboBusinessType, nRow) = 7 Then
                    m_oBusinessPolicy.GetAssociatedAgent(r_iUserID:=g_iUserId, m_vAgentArray:=oResults)
                    If Information.IsArray(oResults) Then
                        m_oBusinessPolicy.GetAssociatedAgentWithBranch(r_iUserID:=g_iUserId, m_vAgentArray:=oResults,
                                                                       r_iSourceID:=iSourceId)
                        If Information.IsArray(oResults) Then
                            cboBusinessType.SelectedIndex = nRow
                            cboBusinessType_SelectionChangeCommitted(Nothing, Nothing)
                            pnlAgentCode.Tag = CStr(oResults(0, 0))

                            pnlAgentCode.Text = CStr(oResults(1, 0))
                            pnlAgentCode.Enabled = False
                            cboBusinessType.Enabled = False
                            cmdAgentCode.Enabled = False
                            m_vLeadAgentCnt = CStr(oResults(0, 0))
                        End If
                    ElseIf oAgentName <> "" And m_vLeadAgentCnt <> 0 Then
                        cmdAgentCode.Enabled = True
                        m_lReturn = GetBranchDefaultAgent()
                    End If

                End If
            Next
            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 1 Then
                cmdAgentCode.Enabled = False
            ElseIf (m_sTransactionType = "MTA" Or m_sTransactionType = "MTC") Then
                If m_bAgentEditable = True Then
                    cmdAgentCode.Enabled = True
                Else
                    cmdAgentCode.Enabled = False
                End If
            End If

            If m_sTransactionType = "NB" AndAlso m_iCorrespondenceType = 0 Then
                For i As Integer = 0 To cboCorrespondenceMethod.ListCount - 1
                    If cboCorrespondenceMethod.ItemCaption(cboCorrespondenceMethod.ItemData(i)) = "Use Default" Then
                        cboCorrespondenceMethod.ListIndex = i
                        Exit For
                    End If
                Next i
            Else
                cboCorrespondenceMethod.ItemId = m_iCorrespondenceType
            End If
            Return nResult

        Catch excep As System.Exception


            ' Error Section

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try
            Dim vResult(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            'Are we from events?

            m_oBusiness.FromEvent = FromEvent



            m_lReturn = m_oBusiness.getdetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}

            m_lInsuranceFolderCnt = m_oBusiness.InsuranceFolderCnt

            'Are we from events?

            m_oFolderBusiness.FromEvent = FromEvent

            'If coming from events, then the folder has the same number as the file
            If FromEvent Then


                m_lReturn = m_oFolderBusiness.GetDetails(vInsuranceFolderCnt:=m_lInsuranceFileCnt)
            Else



                m_lReturn = m_oFolderBusiness.GetDetails(vInsuranceFolderCnt:=m_lInsuranceFolderCnt)
            End If
            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the folder business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' {* USER DEFINED CODE (Begin) *}
            'Are we from events?

            m_oSystemBusiness.FromEvent = FromEvent



            m_lReturn = m_oSystemBusiness.GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            'If cboBusinessType.ItemData(cboBusinessType.ListIndex) = 7 Then
            'if agency businesss check for cover note's presence


            'Developer Guide No. 8
            m_lReturn = m_oCoverNoteBusiness.GetDetails(m_lInsuranceFileCnt, vResult)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            If Information.IsArray(vResult) Then
                txtCoverNoteBook.Text = gPMFunctions.ToSafeString(vResult(0, 0))
                txtCoverNoteSheet.Text = gPMFunctions.ToSafeString(vResult(1, 0))
            End If
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetInsurerRate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetInsurerRate() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'If Convert.IsDBNull(m_vLeadInsurerCnt) Or IsNothing(m_vLeadInsurerCnt) Then
    'Return result
    'End If
    '
    'Create coinsurer object if not already done so
    'If m_oInsurerRate Is Nothing Then
    '
    ' Get an instance of the Insurer Rate business object via
    ' the public object manager.
    'Dim temp_m_oInsurerRate As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_m_oInsurerRate, "bPIRLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'm_oInsurerRate = temp_m_oInsurerRate
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Insurer Rate object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerRate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'End If
    '
    'The insurer...


    'm_oInsurerRate.Insurer = m_vLeadInsurerCnt
    '
    'The scheme is...
    'ECK 14/07/99

    'If Convert.IsDBNull(m_vScheme) Or IsNothing(m_vScheme) Then

    'm_oInsurerRate.Scheme = 0
    'Else


    'm_oInsurerRate.Scheme = m_vScheme
    'End If
    '


    'm_oInsurerRate.FromDate = m_oFormFields.UnformatControl(txtCoverFromDate)
    '

    'm_lReturn = m_oInsurerRate.Start()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'If Not m_oInsurerRate.RateFound Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerRate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetIPT) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetIPT() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'If Convert.IsDBNull(m_vLeadInsurerCnt) Or IsNothing(m_vLeadInsurerCnt) Then
    'm_cIPTRate = 0
    'm_cIPTAmount = 0
    'Return result
    'End If
    '
    'Create coinsurer object if not already done so
    'If m_oIPT Is Nothing Then
    '
    ' Get an instance of the IPT business object via
    ' the public object manager.
    'Dim temp_m_oIPT As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_m_oIPT, "bIptLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'm_oIPT = temp_m_oIPT
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get IPT object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIPT", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'End If
    '
    'The account is the insurer...


    'm_oIPT.Account = m_vLeadInsurerCnt
    '
    'The effective date is when the policy commences


    'm_oIPT.IPTEffectiveDate = m_oFormFields.UnformatControl(txtCoverFromDate)
    '
    'Check the normal table

    'm_oIPT.IPTType = IPTConsts.PMIPTTableLookup
    '

    'm_oIPT.PostCode = ""
    '

    'm_lReturn = m_oIPT.Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Now what do we do with the values?

    'm_cIPTRate = m_oIPT.Rate

    'm_cIPTAmount = m_oIPT.IPTAmount
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIPT Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIPT", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox, Optional ByRef bSecondary As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
        Dim bFoundMatch As Boolean
        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            sLookupTable = sLookupTable.Trim().ToUpper()
            bFoundMatch = False
            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim().ToUpper() = sLookupTable Then
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

            '    ctlLookup.Clear

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.


                Dim listindex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem((m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If

                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
                    If m_vLookupValues(ACValueID, lRow2) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                        'ReflectionHelper.SetMember(ctlLookup, "ListIndex", ReflectionHelper.GetMember(ctlLookup, "NewIndex"))'tt
                        ctlLookup.SelectedIndex = listindex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            ' No - to minus 1...
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then
                ctlLookup.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck200400
    ' ***************************************************************** '
    ' Name: GetLookupRiskCodes
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupRiskCodes) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupRiskCodes(ByRef vRiskCodes( ,  ) As Object, ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByRef bSecondary As Boolean = False) As Integer
    'Dim result As Integer = 0
    'Dim bValidRiskCode As Boolean
    'Dim lRow, lRow2 As Integer
    'Dim bFoundMatch As Boolean
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRiskCodes")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    'eck200400
    'Check That the code is valid
    'bValidRiskCode = False
    'For 'i As Integer = 0 To vRiskCodes.GetUpperBound(1)

    'If CInt(m_vLookupDetails(ACDetailKey, lCntr)) = CInt(vRiskCodes(0, i)) Then
    'bValidRiskCode = True
    'End If
    'Next i
    'If bValidRiskCode Then
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Check if this is the selected index.
    'If bSecondary Then
    'lRow2 = lRow + 1
    'Else
    'lRow2 = lRow
    'End If
    '
    '
    'If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    '
    'End If
    'End If
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    ' No - to minus 1...
    'If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then

    'ctlLookup.ListIndex = -1
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupRiskCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    'Update History:
    ' Changed the LookupType to LookupAllwithDeleted for EDIT AND VIEW
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails, vPolicyRelationshipType:=m_sRelationshipType)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.
                    'Now this will fetch all Branches even Closed ones

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails, vPolicyRelationshipType:=m_sRelationshipType)

                Case gPMConstants.PMEComponentAction.PMView
                    'Note that lookupsingle doesn't work, has never worked and never will work...
                    'Get lookup values for viewing only.
                    'Now this will fetch all Branches even Closed ones

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllWithDeleted, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails, vPolicyRelationshipType:=m_sRelationshipType)
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
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    ' Edit History :
    '   MEvans : 03-12-2004 : PN17232
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Dim bShowMultiCurrencyDialogue As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RDC 12052004 for multi-currency

            'JT**05/10/04**PN-15258-Screen shouldnt come while editing
            'RDT 01/12/2004 - PN 16558 - Need to be allowed view screen when editing an
            '                            existing quote through New Business roadmap.
            ' Need to show currency dialog for MTA, MTC, MTR as well as NB. PN17232
            ' Determine whether or not to show currency dialogue
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                Select Case m_sTransactionType
                    Case "NB", "MTA", "MTC", "MTR", "REN"
                        bShowMultiCurrencyDialogue = True
                    Case Else
                        bShowMultiCurrencyDialogue = False
                End Select
            Else
                bShowMultiCurrencyDialogue = True
            End If

            If bShowMultiCurrencyDialogue Then
                m_lReturn = ShowMultiCurrencyDialogue()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to show multi-currency form
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, r_vFieldArray:=m_vFieldArray)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If


                    m_lReturn = m_oFolderBusiness.EditAdd(lRow:=lBusinessDataID, vInsuranceHolderCnt:=m_lPartyCnt, vCode:=m_sInsuranceRef, vDescription:=m_vDescription, vInceptionDate:=m_vInceptionDate, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_vRenewalCount)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to folder business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If


                    m_lReturn = m_oSystemBusiness.EditAdd(lRow:=lBusinessDataID, vInsuranceFileCnt:=m_lInsuranceFileCnt, vEndorsementCount:=0, vLastTransDescription:=m_vLastTransDescription)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to system business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If


                Case gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, r_vFieldArray:=m_vFieldArray)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If


                    m_lReturn = m_oFolderBusiness.EditUpdate(lRow:=lBusinessDataID, vInsuranceHolderCnt:=m_lInsuranceHolderCnt, vCode:=m_sCode, vDescription:=m_vDescription, vInceptionDate:=m_vInceptionDate, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_vRenewalCount)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to folder business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If


                    m_lReturn = m_oSystemBusiness.EditUpdate(lRow:=lBusinessDataID, vLastTransDescription:=m_vLastTransDescription)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to system business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                    End If

            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ''' <summary>
    ''' InterfaceToData: Updates the data storage from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InterfaceToData() As Integer

        Dim nResult As Integer = 0
        Dim sMsg As String = ""
        Dim vMinimumBrokerage As Object
        Dim lTemp As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            'Don't forget - we need some business logic if the policy number changes...

            'Validate the policy number - only if it has been changed!
            If m_sInsuranceRef.Trim() <> txtPolicyNumber.Text.Trim() Then
                m_lReturn = ValidatePolicyNumber()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                ElseIf m_sTransactionType = "NB" Then 'PM049747 Policy Number should be updated only during NB not during MTA, cancellation, Reinstatement etc.
                    'PM039561 Update the variable too if we have validated the new policy number
                    m_sInsuranceRef = CStr(m_oFormFields.UnformatControl(ctlControl:=txtPolicyNumber))
                End If
            End If

            'm_sInsuranceRef = CStr(m_oFormFields.UnformatControl(ctlControl:=txtPolicyNumber))

            ' If this is an add then check for duplicate references

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    'If the returned reference is an empty string, then the reference exists
                    If m_sInsuranceRef = "" Then


                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefExists,
                                                       iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,
                                                       bResFile:=My.Resources.ResourceManager))

                        System.Windows.Forms.MessageBox.Show(sMsg, "Policy", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Exclamation)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If
            End If


            m_vLastTransDescription = m_oFormFields.UnformatControl(ctlControl:=txtRegarding)

            m_dtCoverStartDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCoverFromDate))

            m_dtExpiryDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCoverToDate))


            m_vInceptionDate = m_oFormFields.UnformatControl(ctlControl:=txtInceptionDate)


            m_vInceptionTPI = m_oFormFields.UnformatControl(ctlControl:=txtInceptionTPI)

            m_dtRenewalDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtRenewalDate))


            m_vDateIssued = m_oFormFields.UnformatControl(ctlControl:=txtIssuedDate)
            'If m_vDateIssued = ToSafeDate("29/12/1899", #12/29/1899#) Then
            '    m_vDateIssued = Date.Now
            'End If
            'These are coming in as 1,000.00, so let's force a conversion


            m_vNetPremium = m_oFormFields.UnformatControl(ctlControl:=txtPremiumExcTax)


            m_vAnnualPremium = m_oFormFields.UnformatControl(ctlControl:=txtFutureAnnualPremium)


            m_vLongTermUndertakingDate = m_oFormFields.UnformatControl(ctlControl:=txtPolicyLTUExpiryDate)


            m_vLapsedDate = m_oFormFields.UnformatControl(ctlControl:=txtLapsedDate)


            If pnlRelatedPolicy.Name.Trim() = "" Then

                m_vIsRelatedPolicies = 0
            Else

                m_vIsRelatedPolicies = 1
            End If

            'TN20000816 - Doc Ref 10 (Start)


            m_vInsuredName = m_oFormFields.UnformatControl(ctlControl:=txtInsuredName)


            m_vAlternateReference = m_oFormFields.UnformatControl(ctlControl:=txtAlternateReference)


            m_vOldPolicyNumber = m_oFormFields.UnformatControl(ctlControl:=txtOldPolicyNo)


            m_vProposalDate = m_oFormFields.UnformatControl(ctlControl:=txtProposalDate)


            m_vQuoteExpiryDate = m_oFormFields.UnformatControl(ctlControl:=txtQuoteExpiryDate)
            'TN20000816 - Doc Ref 10 (End)


            'From Lookup Combos

            If cboAnalysisCode.SelectedIndex = -1 Then


                m_vAnalysisCodeID = DBNull.Value
            Else

                m_vAnalysisCodeID = VB6.GetItemData(cboAnalysisCode, cboAnalysisCode.SelectedIndex)
            End If

            'Branch isn't branch - it's source
            If cboBranchCode.SelectedIndex = -1 Then
                m_iSourceId = 0
            Else
                m_iSourceId = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
            End If

            If cboProduct.SelectedIndex = -1 Then
                m_lProductID = 0
            Else
                m_lProductID = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)
            End If

            If (cboStatus.SelectedIndex = -1) Or (cboStatus.SelectedIndex = 0) Then


                m_vInsuranceFileStatusID = DBNull.Value
            Else

                m_vInsuranceFileStatusID = VB6.GetItemData(cboStatus, cboStatus.SelectedIndex)

            End If

            If cboCurrency.SelectedIndex = -1 Then

                m_iCurrencyID = Nothing
            Else
                m_iCurrencyID = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)
            End If

            If cboFrequency.SelectedIndex = -1 Then

                m_iRenewalFrequencyID = Nothing
            Else
                m_iRenewalFrequencyID = VB6.GetItemData(cboFrequency, cboFrequency.SelectedIndex)
            End If

            If cboRenewalMethod.SelectedIndex = -1 Then


                m_vRenewalMethodID = DBNull.Value
            Else

                m_vRenewalMethodID = VB6.GetItemData(cboRenewalMethod, cboRenewalMethod.SelectedIndex)
            End If

            If cboBusinessType.SelectedIndex = -1 Then


                m_vBusinessTypeId = DBNull.Value
            Else

                m_vBusinessTypeId = VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex)
            End If

            If cboRenewalStop.SelectedIndex = -1 Then


                m_vRenewalStopCodeID = DBNull.Value
            Else

                m_vRenewalStopCodeID = VB6.GetItemData(cboRenewalStop, cboRenewalStop.SelectedIndex)
            End If

            If cboRelationship.SelectedIndex = -1 Then

                m_vRelationship = Nothing
            Else
                m_vRelationship = VB6.GetItemData(cboRelationship, cboRelationship.SelectedIndex)
            End If

            If cboLapsedReason.SelectedIndex = -1 Then


                m_vLapsedReasonID = DBNull.Value
            Else

                m_vLapsedReasonID = VB6.GetItemData(cboLapsedReason, cboLapsedReason.SelectedIndex)
            End If


            If chkReferredAtRenewal.CheckState = CheckState.Checked Then
                m_iIsReferredAtRenewal = 1
            Else
                m_iIsReferredAtRenewal = 0
            End If

            If chkReferredOnMTA.CheckState = CheckState.Checked Then

                m_iIsReferredOnMta = 1
            Else

                m_iIsReferredOnMta = 0
            End If

            'TN20000816 - Doc Ref 10 (Start)
            'TN20000816 - Doc Ref 10 (End)

            'This is the extra stuff for when we are adding...
            If Task = gPMConstants.PMEComponentAction.PMAdd Then


                m_vLeadInsurerCnt = DBNull.Value
                m_lInsuredCnt = m_lPartyCnt
                'This is the code for Underwriting.  In the future we'll be passing it in...

                If Object.Equals(m_vPolicyTypeId, Nothing) Then

                    m_vPolicyTypeId = 5
                End If
            End If


            'TN20000818 - Doc Ref 10 (Start)
            If UnloadAgents() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20000818 - Doc Ref 10 (End)

            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            If m_bUseClientPolicyLinkage Then
                m_lReturn = UnloadInsuredClients()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)


            'Validation of following not required if we are cancelling out

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}
            'sj 19/07/2002 - start
            If cboSubBranch.SelectedIndex >= 0 Then

                m_vSubBranchId = Conversion.Val(CStr(VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)))
            Else

                m_vSubBranchId = 0
            End If
            'sj 19/07/2002 - end

            'DJM 24/03/2004
            m_lPolicyStyleID = cboPolicyStyle.ItemId

            m_vUnderwritingYearID = cboUnderwritingYearID.ItemId

            m_vPolicyStatusID = cboPolicyStatus.ItemId
            'PLICO 29
            If cboPolicyDeductible.ItemCaption = "(None)" Then


                m_vPolicyDeductiblesID = DBNull.Value
            Else

                m_vPolicyDeductiblesID = cboPolicyDeductible.ItemId
            End If

            If cboPolicyLimits.ItemCaption = "(None)" Then


                m_vPolicyLimitsID = DBNull.Value
            Else

                m_vPolicyLimitsID = cboPolicyLimits.ItemId
            End If
            '   m_vPolicyDeductibleID = cboPolicyDeductible.ItemId
            '   m_vPolicyLimitsID = cboPolicyLimits.ItemId

            ' *******************************************
            ' discount / loading  "insurance file details"
            ' if a discount reason hasnt been provided
            ' pass discount reason id as null
            If cboDiscountReason.ItemId < 1 Then
                m_iDiscountReasonId = 0
            Else
                m_iDiscountReasonId = cboDiscountReason.ItemId
            End If

            ' save premium and percentage
            m_crDiscountedPremium = gPMFunctions.ToSafeCurrency(txtDiscountedPremium.Text, 0)
            m_dDiscountPercentage = gPMFunctions.ToSafeDouble(txtDiscountPercentage.Text, 0)

            ' set match discount premium indicator
            If m_crDiscountedPremium <> 0 Then
                m_iMatchDiscountedPremiumFlag = 1
            Else
                m_iMatchDiscountedPremiumFlag = 0
            End If

            ' discount / loading  "insurance folder details"
            m_lDiscountRecurringTypeID = cboDiscountRecurringType.ItemId
            m_lDiscountInsuranceFileCnt = m_lInsuranceFileCnt
            m_vDiscountTermEndDate = m_dtExpiryDate
            If cboAgencyContact.SelectedIndex >= 0 Then
                m_vContactuserId = VB6.GetItemData(cboAgencyContact, cboAgencyContact.SelectedIndex)
            End If
            ' *******************************************

            '--RFC-PLICO14- Amit
            If txtManualDiscountPercentage.Text.IndexOf("%"c) >= 0 Then
                m_dManualDiscountPercentage = gPMFunctions.ToSafeDouble(
                    Mid(txtManualDiscountPercentage.Text, 1, Strings.Len(txtManualDiscountPercentage.Text) - 1), 0)
            Else
                m_dManualDiscountPercentage = gPMFunctions.ToSafeDouble(txtManualDiscountPercentage.Text, 0)
            End If


            ' if this is true monthly policy then
            If m_bIsTrueMonthlyPolicy Then

                ' new business tmp are classed as anniversary copies
                If m_sTransactionType = "NB" Then
                    m_lAnniversaryCopy = 1
                ElseIf m_sTransactionType <> "REN" Then 'PN28331 fix
                    m_lAnniversaryCopy = 0
                End If

                If chkPutOnNextInstalmentRenewal.CheckState = CheckState.Checked Then
                    m_lPutOnNextInstalmentRenewal = 1
                Else
                    m_lPutOnNextInstalmentRenewal = 0
                End If

                If txtAnniversaryDate.Text = "" Then
                    MsgBox("Invalid anniversary date", vbOKOnly, "Anniversary Date")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                Else
                    m_dtAnniversaryDate = CDate(txtAnniversaryDate.Text)
                End If

                If cboRenewalDayNumber.SelectedIndex <> -1 Then
                    m_lRenewalDayNumber = VB6.GetItemData(cboRenewalDayNumber, cboRenewalDayNumber.SelectedIndex)
                Else
                    m_lRenewalDayNumber = 0
                End If

                'TMP
                m_iLeadAllowConsolidatedCommission = chkConsolidatedLeadCommission.CheckState
                m_iSubAllowConsolidatedCommission = chkConsolidatedSubCommission.CheckState

                If DatePart("d", m_dtAnniversaryDate) <> m_lRenewalDayNumber Then
                    m_dtAnniversaryDate = DateSerial(Year(m_dtAnniversaryDate), Month(m_dtAnniversaryDate),
                                                 m_lRenewalDayNumber)
                    txtAnniversaryDate.Text =
                   Format(
                       GetClosestDate(Microsoft.VisualBasic.DateAndTime.Day(m_dtRenewalDate),
                                      Month(m_vInceptionDate), Year(m_vInceptionDate) + 1), "Long Date")
                    MsgBox("Anniversary day has to be the same as the monthly renewal day", vbOKOnly, "Anniversary Date")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                ElseIf m_dtAnniversaryDate < m_dtRenewalDate Then
                    txtAnniversaryDate.Text =
                    Format(
                        GetClosestDate(Microsoft.VisualBasic.DateAndTime.Day(m_dtRenewalDate),
                                       Month(m_vInceptionDate), Year(m_vInceptionDate) + 1), "Long Date")
                    MsgBox("Anniversary Date must be after Renewal Date", vbOKOnly, "Anniversary Date")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

            Else

                m_lPutOnNextInstalmentRenewal = 0
                m_lAnniversaryCopy = 0
                m_dtAnniversaryDate = m_dtRenewalDate
                m_lRenewalDayNumber = DateAndTime.Day(m_dtRenewalDate)

            End If

            If (optGross.Checked) Then
                m_sCoInsPlacement = "GROSS"
            End If
            If (optNet.Checked) Then
                m_sCoInsPlacement = "NETT"
            End If

            m_iCorrespondenceType = cboCorrespondenceMethod.ItemId
            m_lReturn = SetArrayFromValues(r_vFieldArray:=m_vFieldArray)

            Return nResult

        Catch excep As System.Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LockPolicy
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (LockPolicy) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function LockPolicy() As Integer
    '
    'Dim result As Integer = 0
    'Dim sLockedBy As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.LockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)
    '
    '
    'Select Case m_lReturn
    'Case gPMConstants.PMEReturnCode.PMTrue
    'OK
    '
    'Case gPMConstants.PMEReturnCode.PMFalse
    'Locked or error
    'If sLockedBy = "ERROR" Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'System.Windows.Forms.MessageBox.Show("Policy currently locked by " & sLockedBy &  _
    '                                     Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Policy Lock")
    'Return result
    'End If
    '
    '
    'Case Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Select
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ''' <summary>
    ''' OKClick
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OKClick() As Integer
        Dim nResult As Integer
        Dim sDiscountPerc As String
        Dim oChangePolicyStatus As bSIRChangePolicyStatus.Business
        Dim bValidBranch As Boolean
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim nPeriodID As Integer
            Dim nBusinessTypeId As Integer
            If tabMainTab.Visible Then

                ' Don't want to do anything if we are viewing
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    If m_sTransactionType = "PT" Or m_sTransactionType = "DRI" Then
                        m_lReturn = CreateEvent()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            OKClick = gPMConstants.PMEReturnCode.PMCancel
                            Return nResult
                        End If
                    End If
                    Return nResult
                End If

                If m_iTask <> gPMConstants.PMEComponentAction.PMView And Trim(pnlAgentCode.Text) <> "" Then
                    m_lReturn = GetAssosiatedAgentBranch(bValidBranch)
                    If bValidBranch = False Then
                        OKClick = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                Else
                    m_sAgentName = ""
                    m_vLeadAgentCnt = 0
                End If

                ' Start - Check to see if discount % falls in the numeric(11,8) range
                sDiscountPerc = CStr(gPMFunctions.ToSafeDouble(txtManualDiscountPercentage.Text))

                If StringsHelper.ToDoubleSafe(sDiscountPerc) > 999.99999999 Then
                    System.Windows.Forms.MessageBox.Show("Manual Discount/Surcharge Percentage has a value which is not in the valid range. Please re-enter.",
                        "Manual Discount/Surcharge Percentage", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtManualDiscountPercentage.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' End - Check to see if discount % falls in the numeric(11,8) range


                ' Check that the Underwriting Year is valid before rest of validation, as we
                ' want to display a specific message
                If m_bUnderwritingYearID Then
                    If cboUnderwritingYearID.ItemId = 0 Then
                        System.Windows.Forms.MessageBox.Show("Warning - no Underwriting Year exists for this date so the roadmap cannot continue. Please contact your System Administrator",
                            "Underwriting Year", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'Not allow renewal with previous dates
                If m_sTransactionType = "REN" Then
                    If CDate(txtCoverFromDate.Text) < m_dtCoverStartDate Then
                        System.Windows.Forms.MessageBox.Show("Cover Start Date cannot be prior to the Renewal Date of the Current Term.",
                                                             Application.ProductName, MessageBoxButtons.OK,
                                                             MessageBoxIcon.Information)
                        txtCoverFromDate.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'force lost focus for free-form combo
                If cboBusinessType.SelectedIndex = -1 Then
                    m_lReturn = PMBGeneralFunc.ControlLostFocus(cboBusinessType)
                End If
                If cboCurrency.SelectedIndex = -1 Then
                    m_lReturn = PMBGeneralFunc.ControlLostFocus(cboCurrency)
                End If

                If m_bIsTrueMonthlypolicyandNextInstalmentRenewal And m_sTransactionType = "MTA" Then
                    If chkPutOnNextInstalmentRenewal.CheckState = CheckState.Unchecked Then
                        If System.Windows.Forms.MessageBox.Show("Policy will change from Instalment to Invoice. Do you wish to continue?", "Policy",
                                MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                            m_bIsExit = False
                            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = False
                        Else
                            m_bIsExit = True
                            Return nResult
                        End If

                    End If
                End If

                If m_sTransactionType = "MTA" And m_bIsMTATemp Then

                    If gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim) >
                        gPMFunctions.ToSafeDate(txtRenewalDate.Text.Trim) Then
                        System.Windows.Forms.MessageBox.Show("Temporary MTA tried to extend cover beyond expiry date. This function is not permited. Click ok to continue.",
                            "Policy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
                'check if this is a temporary mta and a cancelled policy
                If m_sSelectedPolicyStatus = "Cancelled" And m_bIsMTATemp Then
                    If m_sTransactionType = "MTA" Then
                        If DateAndTime.DateDiff("D", CDate(txtCoverToDate.Text), m_dtLapsedDate, FirstDayOfWeek.Sunday,
                                                 FirstWeekOfYear.Jan1) < 0 And m_sTransactionType = "MTA" Then
                            System.Windows.Forms.MessageBox.Show("Temporary MTA 'Cover To' date greater than effective date of cancellation of the policy. This function is not permitted. Click OK to continue.",
                                "Policy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                'Check if this is a temporary MTA and a lapsed policy - If so, then :
                If m_sSelectedPolicyStatus = "Lapsed" And m_bIsMTATemp Then
                    If m_sTransactionType = "MTA" Then
                        If DateAndTime.DateDiff("D", CDate(txtCoverToDate.Text), m_dtLapsedDate, FirstDayOfWeek.Sunday,
                                                 FirstWeekOfYear.Jan1) < 0 And m_sTransactionType = "MTA" Then
                            System.Windows.Forms.MessageBox.Show("Temporary MTA 'Cover To' date greater than effective date of lapse of the policy. This function is not permitted. Click OK to continue.",
                                "Policy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'txtCoverToDate.SetFocus
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                If txtCoverFromDate.Text.Trim() <> "" Then
                    Dim temp_oChangePolicyStatus As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oChangePolicyStatus, "bSIRChangePolicyStatus.Business",
                                                             vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oChangePolicyStatus = temp_oChangePolicyStatus

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("OKClick",
                                                "Failed to Initilize the component bSIRChangePolicyStatus.Business",
                                                gPMConstants.PMELogLevel.PMLogError)
                    Else
                        'This code is only for Quote. So set the business type to 1
                        nBusinessTypeId = 1

                        m_lReturn =
                            oChangePolicyStatus.GetAccountingPeriodForCoverStartDate(v_lBusinessType:=nBusinessTypeId,
                                                                                     v_iSubBranchID:=m_iSourceId,
                                                                                     v_lProductId:=m_lProductID,
                                                                                     r_lPeriodID:=nPeriodID,
                                                                                     v_dtCoverStartDate:=
                                                                                        gPMFunctions.ToSafeDate(
                                                                                            txtCoverFromDate.Text.Trim,
                                                                                            DateTime.Now))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("OKClick", "Method GetAccountingPeriodForCoverStartDate Failed ",
                                                    gPMConstants.PMELogLevel.PMLogError)
                        Else
                            If nPeriodID = -1 Then
                                System.Windows.Forms.MessageBox.Show("No Accounting Period Defined for the selected dates, contact system administrator",
                                    "Accounting Period", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If
                    End If
                End If
                ' Save quote
                m_lReturn = SavePolicy()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                End If


            End If

            If tabAgent.Visible Then
                cmdAgentMain_Click(cmdAgentMain, New EventArgs())
            End If

            If tabCommissionTab.Visible Then
                cmdMain_Click(cmdMain, New EventArgs())
            End If

            'And this stops us exiting.

            Return m_lReturn
        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : MTACancellation
    '
    ' Desc : cancel this MTA
    '
    ' Hist : 19 April 2001 Created - Tinny
    ' ***************************************************************** '
    Public Function MTACancellation(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, Optional ByVal v_sDesc As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return m_oBusiness.MTACancellation(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_sDesc:=v_sDesc)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTACancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTACancellation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : LapseQuote
    '
    ' Desc : cancel this MTA
    '
    ' Hist : 19 April 2001 Created - Tinny
    ' ***************************************************************** '
    Public Function LapseQuote() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboLapsedReason.SelectedIndex = -1 Then
                System.Windows.Forms.MessageBox.Show("Lapse reason has not been entered", "Lapse Quote", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                cboLapsedReason.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If txtLapsedDate.Text.Trim() = "" Then
                System.Windows.Forms.MessageBox.Show("Lapse date has not been entered", "Lapse Quote", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                txtLapsedDate.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lTemp As Integer = 0 To cboStatus.Items.Count - 1
                If VB6.GetItemData(cboStatus, lTemp) = 2 Then
                    cboStatus.SelectedIndex = lTemp
                    Exit For
                End If
            Next lTemp

            tabMainTab.Visible = True
            'Start - Sankar - PN 53104
            m_bLapseAQuote = True
            'Start - Sankar - PN 53104

            m_lReturn = OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateFees
    '
    ' Description: Fills the Fees grid control with some details
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PopulateFees) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub PopulateFees()
    '
    'Dim j, k As Integer
    'Dim sDesc, sTmp As String
    'Dim oListItem As ListViewItem
    '
    'Try 
    '
    '
    'Just go if no fees
    'If Not Information.IsArray(m_vFees) Then
    'Exit Sub
    'End If
    '
    'm_cTotalFees = 0
    '
    ' Assign the details to the interface.
    'For 'i As Integer = m_vFees.GetLowerBound(1) To m_vFees.GetUpperBound(1)
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' Assign the details to the first column.
    ' Fee type
    '
    ' Assign details to other the columns
    ' Shortname
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vFees(1, i)).Trim()
    '
    ' Percentage
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=m_vFees(2, i))
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to assign the data.
    'Exit Sub
    'End If
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtPercentage.Text
    '
    ' Value
    'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=m_vFees(3, i))
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to assign the data.
    'Exit Sub
    'End If
    '
    'If CStr(m_vFees(0, i)).Trim() = "Discount Account" Then
    'm_cTotalFees -= CDec(ReplaceNullWithDefault(CStr(m_vFees(3, i)), CStr(0)))
    'Else
    'm_cTotalFees += CDec(ReplaceNullWithDefault(CStr(m_vFees(3, i)), CStr(0)))
    'End If
    '
    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtAmount.Text
    '
    ' Store the Fee Party_cnt
    'oListItem.Tag = CStr(m_vFees(4, i)).Trim()
    '
    'Next i
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateFees", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub


    'End Try
    '
    'End Sub

    '*********************************************************************************'
    ' Name          :   PopulateStandardWordings
    ' Description   :   To Populate the StandardWordings
    ' Modified by   :   Arul Stephen
    '*********************************************************************************'
    'Start (Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)
    Private Function PopulateStandardWordings() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Const kMethodName As String = "PopulateStandardWordings"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vStandardWordings) Then
                Return result
            End If

            lvwPolicyWording.Items.Clear()

            If Not Object.Equals(m_vStandardWordings(kSelectClauseId, ACSelectClasueArrayIndex), Nothing) Then
                If m_bEnableDoNotMergeClause Then
                    lvwPolicyWording.CheckBoxes = True
                    For iRowCount As Integer = m_vStandardWordings.GetLowerBound(ACSelectClasueRowIndex - 1) To m_vStandardWordings.GetUpperBound(ACSelectClasueRowIndex - 1)

                        oListItem = lvwPolicyWording.Items.Add(CStr(m_vStandardWordings(kSelectClauseCode, iRowCount)).Trim())

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vStandardWordings(kSelectClauseDescription, iRowCount)).Trim()

                        oListItem.Tag = CStr(m_vStandardWordings(kSelectClauseId, iRowCount)).Trim()

                        oListItem.Text = Space(Len(CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDoNotMerge, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))) + " " + CStr(m_vStandardWordings(kSelectClauseCode, iRowCount)).Trim()

                        If m_vStandardWordings.GetUpperBound(0) = kSelectClauseDoNotMerge Then
                            If CStr(m_vStandardWordings(kSelectClauseDoNotMerge, iRowCount)).Trim() = "1" Then
                                oListItem.Checked = True
                                oListItem.ImageIndex = 14
                            Else
                                oListItem.Checked = False
                                oListItem.ImageIndex = 13
                            End If
                        End If
                    Next iRowCount
                Else
                    lvwPolicyWording.CheckBoxes = False
                    For iRowCount As Integer = m_vStandardWordings.GetLowerBound(ACSelectClasueRowIndex - 1) To m_vStandardWordings.GetUpperBound(ACSelectClasueRowIndex - 1)

                        oListItem = lvwPolicyWording.Items.Add(CStr(m_vStandardWordings(kSelectClauseId, iRowCount)).Trim(), "history")

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vStandardWordings(kSelectClauseDescription, iRowCount)).Trim()


                        oListItem.Tag = CStr(m_vStandardWordings(kSelectClauseId, iRowCount)).Trim()

                        oListItem.Text = CStr(m_vStandardWordings(kSelectClauseCode, iRowCount)).Trim()
                        'End
                    Next iRowCount
                End If
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Return result
    End Function

    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)
    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)
    'This is to setup the columns
    Private Function SetupSelectClausesListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupSelectClausesListView"
        Dim lColWidth As Integer
        Dim sCaption As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwPolicyWording.Columns.Clear()
            If m_bEnableDoNotMergeClause Then
                sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDoNotMerge, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) + "|" + CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                lColWidth = CInt((VB6.PixelsToTwipsX(lvwPolicyWording.Width) - 100) / 10)
                sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            lvwPolicyWording.Columns.Insert(kSelectClauseColHIndexCode - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseColWidthCode)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwPolicyWording.Columns.Insert(kSelectClauseColHIndexDescription - 1, "", sCaption, CInt(VB6.TwipsToPixelsX(kSelectClauseColWidthDescription)), HorizontalAlignment.Left, -1)

            Me._lvwPolicyWording_ColumnHeader_1.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
        Return result
    End Function

    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)


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
        Dim oCurrencyConvert As bACTCurrencyConvert.Form


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                Select Case Task
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                        ' Update the business from the interface.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select

            End If

            ' Check the task.
            Select Case Task
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        'We need to do this in the right order...
                        ' Add the details using the folder business object.

                        m_lReturn = m_oFolderBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the folder details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")

                            Return result

                        End If

                        'We've assigned the folder key, we now need this for the file


                        m_oBusiness.InsuranceFolderCnt = m_oFolderBusiness.InsuranceFolderCnt

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")

                            Return result

                        End If

                        'We've assigned the file key, we now need this for the system


                        m_oSystemBusiness.InsuranceFileCnt = m_oBusiness.InsuranceFileCnt

                        ' Add the details using the system business object.

                        m_lReturn = m_oSystemBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the system details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")

                            Return result

                        End If

                        Dim temp_oCurrencyConvert As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oCurrencyConvert = temp_oCurrencyConvert



                        'Developer Guide No 257. 
                        m_lReturn = oCurrencyConvert.UpdateInsuranceFile(v_lInsuranceFileCnt:=m_oBusiness.InsuranceFileCnt, v_dCurrencyBaseXrate:=m_dBaseExchangeRate, v_dtCurrencyBaseDate:=m_dtEffectiveDateOfExchange, v_dAccountBaseXrate:=0, v_dtAccountBaseDate:=DateTime.MinValue, v_dSystemBaseXrate:=m_dSystemExchangeRate, v_dtSystemBaseDate:=m_dtEffectiveDateOfExchange, v_lRateOverrideReasonID:=m_iRateOverrideReasonID, v_iBaseCurrencyID:=m_iBaseCurrencyID, v_iAccountCurrencyID:=0)


                        oCurrencyConvert.Dispose()

                        oCurrencyConvert = Nothing

                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then

                        ' Check the details haven't changed.

                        m_lReturn = m_oBusiness.Cancel()

                        'MH Request - Always confirm cancellation
                        '                If (m_lReturn = PMDataChanged) Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = System.Windows.Forms.MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Else

                        ' Update the details using the business object.
                        'And do the insurance file folder and system...

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If


                        m_lReturn = m_oFolderBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the folder details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If


                        m_oSystemBusiness.InsuranceFileCnt = InsuranceFileCnt


                        m_lReturn = m_oSystemBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the system details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

                        Dim temp_oCurrencyConvert2 As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oCurrencyConvert2, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oCurrencyConvert = temp_oCurrencyConvert2



                        'Developer Guide No 257. 
                        m_lReturn = oCurrencyConvert.UpdateInsuranceFile(v_lInsuranceFileCnt:=m_oBusiness.InsuranceFileCnt, v_dCurrencyBaseXrate:=m_dBaseExchangeRate, v_dtCurrencyBaseDate:=m_dtEffectiveDateOfExchange, v_dAccountBaseXrate:=0, v_dtAccountBaseDate:=DateTime.MinValue, v_dSystemBaseXrate:=m_dSystemExchangeRate, v_dtSystemBaseDate:=m_dtEffectiveDateOfExchange, v_lRateOverrideReasonID:=m_iRateOverrideReasonID, v_iBaseCurrencyID:=m_iBaseCurrencyID, v_iAccountCurrencyID:=0)


                        oCurrencyConvert.Dispose()

                        oCurrencyConvert = Nothing

                    End If
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function ProcessDetails() As Integer

        Dim result As Integer = 0
        Dim oDetail As frmDetails
        Dim iIndex As Integer
        Dim sTag As String = ""
        Dim lResult As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            iIndex = Convert.ToString(lvwClients.FocusedItem.Tag)

            ' Get a new instance of the form
            oDetail = New frmDetails()

            ' Load it



            oDetail.ShowDialog()
            ' Pass the array through for the combo box
            'TODO
            'oDetail.set_DataArray(m_vClientList)
            oDetail.DataArray = m_vClientList
            oDetail.DeleteClientID = iIndex

            ' Initialise it
            m_lReturn = CType(oDetail, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse's pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Show it
            oDetail.ShowDialog()

            ' Get the values
            If oDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Check for selected record
                If lvwClients.FocusedItem.Index + 1 <> -1 Then
                    ' Confirm delete
                    'TODO
                    lResult = MessageBox(ACCPDlgDeleteConfirm, ACCPDlgTitle, MsgBoxStyle.Question + MsgBoxStyle.OkCancel, ListViewHelper.GetListViewSubItem(lvwClients.FocusedItem, 1).Text)

                    If lResult = System.Windows.Forms.DialogResult.OK Then
                        For Each oListItem As ListViewItem In lvwClients.Items
                            'If this listitem matches the chosen new lead client, then set to Y
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = IIf((Convert.ToString(oListItem.Tag) = oDetail.DeleteClientID), "Y", "N")
                        Next oListItem
                        lvwClients.Items.RemoveAt(lvwClients.FocusedItem.Index)
                    End If
                End If
            End If


            ' Terminate it
            oDetail.Dispose()
            oDetail.Close()

            oDetail = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' PW190802 - allow to suppress sub agents
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vAddress1 As String = "", Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef vDateCancelled As Object = Nothing) As Integer 'CT 19/07/00 added vResolvedName parameter


        Dim result As Integer = 0
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindParty = New iPMBFindParty.Interface_Renamed()

            oFindParty.BranchID = m_iSourceId
            m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = ACApp
            ''Start(Saurabh Agrawal) Tech Spec LOA008 Account Handlers
            If vSpecialParty = "AH" Then

                oFindParty.BranchID = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
            Else
                'oFindParty.BranchID = g_iSourceID 'Agent Filtering
            End If
            ''End(Saurabh Agrawal) Tech Spec LOA008 Account Handlers

            'SD 31/07/2002
            m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=m_sTransactionType, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If (vSpecialParty = "AG") Or (vSpecialParty = "UB") Or (vSpecialParty = "AH") Then
                    oFindParty.NotEditable = 1
                End If

                ' PW190802 - suppress sub agents if applicable
                oFindParty.SuppressSubAgents = bSuppressSubAgents
            End If

            m_lErrorNumber = oFindParty.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                'MSB 03/03/2003


                vDateCancelled = oFindParty.DateCancelled


                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                'TN20000823 - fix CT

                If Not Information.IsNothing(vResolvedName) Then
                    vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
                End If

                ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
                ' Return address line1 if requested

                If Not Information.IsNothing(vAddress1) Then
                    ' Get the key array (only place it's stored)
                    m_lReturn = oFindParty.GetKeys(vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Walk the array to find the value

                    lLower = vKeyArray.GetLowerBound(1)

                    lUpper = vKeyArray.GetUpperBound(1)
                    For lCount As Integer = lLower To lUpper

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)) = PMNavKeyConst.PMKeyNameAddLine1 Then

                            vAddress1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))
                            Exit For
                        End If
                    Next
                End If


                m_lReturn = GetAgencyContactDetails(vPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GetAgentUserDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgencyContactDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectPolicy
    '
    ' Description: Call Find Insurance File component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectPolicy(ByRef vInsuranceFileCnt As Object, ByRef vInsuranceFileRef As Object) As Integer

        Dim result As Integer = 0
        Dim oFindInsuranceFile As Object

        Try
            'ECK 15/7/99 Try this
            System.Windows.Forms.MessageBox.Show("This option is not yet implemented", Application.ProductName)
            Return result

            m_lErrorNumber = CType(oFindInsuranceFile, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindInsuranceFile.Dispose()
                oFindInsuranceFile = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsuranceFile.CallingAppName = "PolicyControl"

            'SD 31/07/2002

            m_lErrorNumber = oFindInsuranceFile.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindInsuranceFile.Dispose()
                oFindInsuranceFile = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsuranceFile.PartyCnt = m_lPartyCnt

            oFindInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt


            m_lErrorNumber = oFindInsuranceFile.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

                oFindInsuranceFile.Dispose()
                oFindInsuranceFile = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oFindInsuranceFile.Status = gPMConstants.PMEReturnCode.PMOK Then



                vInsuranceFileCnt = oFindInsuranceFile.InsuranceFileCnt


                vInsuranceFileRef = oFindInsuranceFile.InsuranceFileRef
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsuranceFile.Dispose()

            oFindInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' SetArrayFromValues (Public) - Sets up the array from the values. HIstory : JT PN 18794 Added Code for Getting base currency
    ''' </summary>
    ''' <param name="r_vFieldArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetArrayFromValues(ByRef r_vFieldArray() As Object) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            If Not Information.IsArray(r_vFieldArray) Then
                ReDim r_vFieldArray(InsuranceFileConst.ACFieldArraySize)
            End If
            m_lReturn = GetBaseCurrency()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetArrayFromValues failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="SetArrayFromValues")

                Return m_lReturn
            End If

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt) = m_lInsuranceFileCnt

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID) = m_lInsuranceFileStructureID

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID) = m_lInsuranceFileTypeID

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) = m_vInsuranceFileStatusID

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileID) = m_lInsuranceFileID

            r_vFieldArray(InsuranceFileConst.ACSourceID) = m_iSourceId

            r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt) = m_lInsuranceFolderCnt

            r_vFieldArray(InsuranceFileConst.ACInsuranceRef) = m_sInsuranceRef

            r_vFieldArray(InsuranceFileConst.ACProductID) = m_lProductID

            r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) = m_vLeadInsurerCnt

            r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) = m_vLeadAgentCnt

            r_vFieldArray(InsuranceFileConst.ACLeadAgentPercent) = m_vLeadAgentPercent

            r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) = m_vAccountHandlerCnt

            r_vFieldArray(InsuranceFileConst.ACInsuredCnt) = m_lInsuredCnt

            r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) = m_vBusinessTypeId

            r_vFieldArray(InsuranceFileConst.ACCollectTypeID) = m_vCollectTypeID

            r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) = m_vCollectionFromCnt

            r_vFieldArray(InsuranceFileConst.ACSubBranchID) = m_vSubBranchId

            r_vFieldArray(InsuranceFileConst.ACCurrencyID) = m_iCurrencyID

            r_vFieldArray(InsuranceFileConst.ACBaseCurrencyID) = m_iBaseCurrencyID

            r_vFieldArray(InsuranceFileConst.ACLanguageID) = m_iLanguageID

            r_vFieldArray(InsuranceFileConst.ACDateIssued) = m_vDateIssued

            r_vFieldArray(InsuranceFileConst.ACCoverStartDate) = m_dtCoverStartDate

            r_vFieldArray(InsuranceFileConst.ACExpiryDate) = m_dtExpiryDate

            r_vFieldArray(InsuranceFileConst.ACRenewalDate) = m_dtRenewalDate

            r_vFieldArray(InsuranceFileConst.ACRenewalMethodID) = m_vRenewalMethodID

            r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) = m_iRenewalFrequencyID

            r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal) = m_iIsReferredAtRenewal

            r_vFieldArray(InsuranceFileConst.ACLapsedReasonID) = m_vLapsedReasonID

            r_vFieldArray(InsuranceFileConst.ACLapsedDate) = m_vLapsedDate

            r_vFieldArray(InsuranceFileConst.ACLapsedDescription) = m_vLapsedDescription

            r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta) = m_iIsReferredOnMta

            r_vFieldArray(InsuranceFileConst.ACPolicyVersion) = m_iPolicyVersion

            r_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus) = m_vGeminiPolicyStatus

            r_vFieldArray(InsuranceFileConst.ACGeminiBusinessType) = m_vGeminiBusinessType

            r_vFieldArray(InsuranceFileConst.ACDeferredInd) = m_vDeferredInd

            r_vFieldArray(InsuranceFileConst.ACPolicyIgnore) = m_vPolicyIgnore

            r_vFieldArray(InsuranceFileConst.ACBrokerCnt) = m_vBrokerCnt

            r_vFieldArray(InsuranceFileConst.ACRiskCodeId) = m_vRiskCodeID

            r_vFieldArray(InsuranceFileConst.ACAnalysisCodeId) = m_vAnalysisCodeID

            r_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId) = m_vPolicyDeductiblesID

            r_vFieldArray(InsuranceFileConst.ACPolicyLimitsId) = m_vPolicyLimitsID

            r_vFieldArray(InsuranceFileConst.ACProposalDate) = m_vProposalDate

            r_vFieldArray(InsuranceFileConst.ACDiaryDate) = m_vDiaryDate

            r_vFieldArray(InsuranceFileConst.ACReviewDate) = m_vReviewDate

            r_vFieldArray(InsuranceFileConst.ACPolicyTypeId) = m_vPolicyTypeId

            r_vFieldArray(InsuranceFileConst.ACIndicator) = m_vIndicator

            r_vFieldArray(InsuranceFileConst.ACClause) = m_vClause

            r_vFieldArray(InsuranceFileConst.ACCover) = m_vCover

            r_vFieldArray(InsuranceFileConst.ACArea) = m_vArea

            r_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate) = m_vLongTermUndertakingDate

            r_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID) = m_vRenewalStopCodeID

            r_vFieldArray(InsuranceFileConst.ACVBSType) = m_vVBSType

            r_vFieldArray(InsuranceFileConst.ACVBSStatus) = m_vVBSStatus

            r_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable) = m_vIsInsurerRateTable

            r_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies) = m_vIsRelatedPolicies

            r_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments) = m_vIsRetainedDocuments

            r_vFieldArray(InsuranceFileConst.ACSchemesPostcode) = m_vSchemesPostcode

            r_vFieldArray(InsuranceFileConst.ACPaidDirect) = m_vPaidDirect

            r_vFieldArray(InsuranceFileConst.ACScheme) = m_vScheme

            r_vFieldArray(InsuranceFileConst.ACBrokerageAmount) = m_vBrokerageAmount

            r_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag) = m_vIsMinimumBrokerageFlag

            r_vFieldArray(InsuranceFileConst.ACAnnualPremium) = m_vAnnualPremium

            r_vFieldArray(InsuranceFileConst.ACThisPremium) = m_vThisPremium

            r_vFieldArray(InsuranceFileConst.ACNetPremium) = m_vNetPremium

            r_vFieldArray(InsuranceFileConst.ACCommissionAmount) = m_vCommissionAmount

            r_vFieldArray(InsuranceFileConst.ACIPTableAmount) = m_vIPTableAmount

            r_vFieldArray(InsuranceFileConst.ACIPTPercentage) = m_vIPTPercentage

            r_vFieldArray(InsuranceFileConst.ACIsIPTOverridden) = m_vIsIPTOverridden

            r_vFieldArray(InsuranceFileConst.ACTaxAmount) = m_vTaxAmount

            r_vFieldArray(InsuranceFileConst.ACVatableAmount) = m_vVatableAmount

            r_vFieldArray(InsuranceFileConst.ACVatPercentage) = m_vVatPercentage

            r_vFieldArray(InsuranceFileConst.ACVatAmount) = m_vVatAmount

            r_vFieldArray(InsuranceFileConst.ACPaymentMethod) = m_vPaymentMethod

            r_vFieldArray(InsuranceFileConst.ACUserDefinedDataID) = m_vUserDefinedDataID

            r_vFieldArray(InsuranceFileConst.ACCommissionPercentage) = m_vCommissionPercentage

            r_vFieldArray(InsuranceFileConst.ACInvariantKey) = 0

            r_vFieldArray(InsuranceFileConst.ACInsuredName) = m_vInsuredName

            r_vFieldArray(InsuranceFileConst.ACAlternateReference) = m_vAlternateReference

            r_vFieldArray(InsuranceFileConst.ACIsClientInvoiced) = m_vIsClientInvoiced

            r_vFieldArray(InsuranceFileConst.ACOldPolicyNumber) = m_vOldPolicyNumber

            If CStr(m_vQuoteExpiryDate) = "" Then
                r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate) = DBNull.Value
            Else
                r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate) = m_vQuoteExpiryDate
            End If

            r_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt) = m_vAlternateAccountCnt

            r_vFieldArray(InsuranceFileConst.ACPolicyStyleID) = m_lPolicyStyleID

            r_vFieldArray(InsuranceFileConst.ACUnderwritingYearID) = m_vUnderwritingYearID

            r_vFieldArray(InsuranceFileConst.ACPolicyStatusID) = m_vPolicyStatusID

            r_vFieldArray(InsuranceFileConst.ACCCInceptionDate) = m_vInceptionDate
            'Policy Discount Details

            r_vFieldArray(InsuranceFileConst.ACDiscountReasonID) = m_iDiscountReasonId

            r_vFieldArray(InsuranceFileConst.ACDiscountedPremium) = m_crDiscountedPremium

            r_vFieldArray(InsuranceFileConst.ACDiscountPercentage) = m_dDiscountPercentage

            r_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag) = m_iMatchDiscountedPremiumFlag

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy) = m_lAnniversaryCopy

            r_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal) = m_lPutOnNextInstalmentRenewal

            r_vFieldArray(InsuranceFileConst.ACAnniversaryDate) = m_dtAnniversaryDate

            r_vFieldArray(InsuranceFileConst.ACRenewalDayNumber) = m_lRenewalDayNumber

            r_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId) = m_lDiscountRecurringTypeID

            r_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission) = m_iLeadAllowConsolidatedCommission

            r_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission) = m_iSubAllowConsolidatedCommission

            r_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) = m_dManualDiscountPercentage

            r_vFieldArray(InsuranceFileConst.ACContactuserId) = m_vContactuserId

            r_vFieldArray(InsuranceFileConst.kCoInsPlacement) = m_sCoInsPlacement

            r_vFieldArray(InsuranceFileConst.ACMTAReasonId) = m_iMTAReasonId

            r_vFieldArray(InsuranceFileConst.ACCorrespondenceType) = m_iCorrespondenceType

            r_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence) = m_bAgentReceiveCorrespondenceFlag

            If cboCorrespondenceMethod.ItemCode.Trim.ToUpper = "DEFAULT" Then
                If m_bAgentReceiveCorrespondenceFlag Then
                    If IsArray(m_vAgentDefaultPreferredCorrespondence) Then
                        r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = m_vAgentDefaultPreferredCorrespondence(0, 0)
                    Else
                        r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = 0
                    End If
                Else
                    If IsArray(m_vClientDefaultPreferredCorrespondence) Then
                        r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = m_vClientDefaultPreferredCorrespondence(0, 0)
                    Else
                        r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = 0
                    End If
                End If
            Else
                r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = 0
            End If
            r_vFieldArray(InsuranceFileConst.ACCCInceptionDate) = m_vInceptionDate

            r_vFieldArray(InsuranceFileConst.ACInceptionTPI) = m_vInceptionTPI
            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetArrayFromValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetArrayFromValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function


    ''' <summary>
    ''' SetFieldValidation - Sets the rules for validating fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFieldValidation() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            'TN20000814 - Doc Ref 10    (Start)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsuredName,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboProduct,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBranchCode,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAlternateReference,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAnalysisCode,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOldPolicyNo,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtProposalDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002 make mandatory if NB/MTA
            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtQuoteExpiryDate,
                                                          lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtQuoteExpiryDate,
                                                          lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Agent Edit tab
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentCode,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentName,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentAmount,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'TN20000814 - Doc Ref 10 (End)

            'Policy number must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyNumber,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Regarding must be entered
            'ECK 13/07/99 Not it musn't
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRegarding,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            '                             lMandatory:=PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Cover From Date must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCoverFromDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Cover To Date must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCoverToDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Inception Date must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInceptionDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Inception TPI (this period of insurance) must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInceptionTPI,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Renewal Date must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRenewalDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Premium Exc Tax
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremiumExcTax,
                                                      lFieldType:=gPMConstants.PMEDataType.PMCurrency,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Future Annual Premium
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFutureAnnualPremium,
                                                      lFieldType:=gPMConstants.PMEDataType.PMCurrency,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Frequency
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboFrequency,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'LTU Expiry Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPolicyLTUExpiryDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Renewal Stop Reason
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRenewalStop,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Relationship
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRelationship,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Lapsed Reason
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboLapsedReason,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Lapsed Date - Mandatory when doing a cancellation
            If m_sTransactionType = "MTC" Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLapsedDate,
                                                          lFieldType:=gPMConstants.PMEDataType.PMString,
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLapsedDate,
                                                          lFieldType:=gPMConstants.PMEDataType.PMString,
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Percentage for fees
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Amount for fees
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAmount,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'ECK 14/09/99 New Fields for Extras
            'Commission Percentage for fees
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFeeCommissionPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Commission Amount for fees
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFeeCommissionAmount,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'IPTable for fees
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFeeIPTable,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'ECK 14/09/99 End
            'ECK 14/07/99
            'Commission Percentage
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Commission Charge
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionCharge,
                                                      lFieldType:=gPMConstants.PMEDataType.PMCurrency,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Commission Payable
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionPayable,
                                                      lFieldType:=gPMConstants.PMEDataType.PMCurrency,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Renewal Method
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRenewalMethod,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Business Type must be added
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBusinessType,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Issued Date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIssuedDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002 - add sub branch as mandatory
            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or IsRenewal Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSubBranch,
                                                          lFieldType:=gPMConstants.PMEDataType.PMString,
                                                          lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                          lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Policy Discount
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboDiscountReason,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboDiscountRecurringType,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDiscountedPremium,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDiscountPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' anniversary renewal date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAnniversaryDate,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDate,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' renewal day number
            'm_lReturn = m_oFormFields.AddNewFormField( _
            'ctlControl:=cboRenewalDayNumber, _
            'lFieldType:=PMLong, _
            'lFormat:=PMFormatLong, _
            'lMandatory:=PMNonMandatory)

            'If (m_lReturn <> PMTrue) Then
            '    SetFieldValidation = PMFalse
            '    Exit Function
            'End If

            ' {* USER DEFINED CODE (End) *}

            '--RFC-PLICO14- Amit
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtManualDiscountPercentage,
                                                      lFieldType:=gPMConstants.PMEDataType.PMDouble,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory,
                                                      lDecimalPlaces:=8)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=optGross,
                                                      lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=optNet, lFieldType:=gPMConstants.PMEDataType.PMString,
                                                      lFormat:=gPMConstants.PMEFormatStyle.PMFormatString,
                                                      lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception


            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            If m_bUseClientPolicyLinkage Then
                ReDim m_ctlTabFirstLast(1, 4)
            Else
                ' Set smaller array and remove tab
                ReDim m_ctlTabFirstLast(1, 3)
                SSTabHelper.SetTabVisible(tabMainTab, 4, False)
            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            'TN20000814 - Doc Ref 10
            m_ctlTabFirstLast(ACControlStart, 0) = txtInsuredName
            m_ctlTabFirstLast(ACControlEnd, 0) = cmdNext(0)
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdNext(1)
            m_ctlTabFirstLast(ACControlStart, 2) = cboFrequency
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdNext(2)
            m_ctlTabFirstLast(ACControlStart, 3) = cmdAddAgent
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
            If m_bUseClientPolicyLinkage Then
                m_ctlTabFirstLast(ACControlEnd, 3) = cmdNext(3)
                m_ctlTabFirstLast(ACControlStart, 4) = cmdAddClient
                m_ctlTabFirstLast(ACControlEnd, 4) = cmdPrevious(4)
            Else
                m_ctlTabFirstLast(ACControlEnd, 3) = cmdPrevious(3)
            End If
            ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Sets all of the interface default values</remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer
        Dim sPolicyRef As String = ""
        Dim bIsMidnightRenewal As Boolean
        Dim sValue As String
        Dim oResults(,) As Object = Nothing
        Dim oIsNRMA As Object
        Dim dtDefaultDate As Date
        Dim oDateCancelled As Object = Nothing
        Dim obFindParty As bSIRFindParty.Business
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_bSetUp = True
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            tabMainTab.Visible = True
            tabMainTab.Top = 0
            tabCommissionTab.Visible = False
            tabAgent.Visible = False
            SSTabHelper.SetTabVisible(tabMainTab, 3, False)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetupCboRenewalDayNumber()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lvwPolicyWording.FullRowSelect = True

            lvwAgents.FullRowSelect = True

            lvwClients.FullRowSelect = True
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwPolicyWording.Handle.ToInt32(),
                                                   v_vShowRowSelect:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAgents.Handle.ToInt32(), v_vShowRowSelect:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwClients.Handle.ToInt32(), v_vShowRowSelect:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdAddClient.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
            cmdDeleteClient.Enabled = False
            cmdDeleteAgent.Enabled = False
            cmdSetCorrespondence.Enabled = False

            'Agent Edit Tab
            txtAgentCode.Enabled = False
            txtAgentName.Enabled = False

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                m_bUnderwritingYearID = True
                lblUnderwritingYearID.Visible = True
                cboUnderwritingYearID.Visible = True
                cboUnderwritingYearID.Enabled = True
            Else
                cboPolicyLimits.Top = cboUnderwritingYearID.Top
                lblPolicyLimits.Top = lblUnderwritingYearID.Top
                m_bUnderwritingYearID = False
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = m_oBusiness.GetInsuredName(m_lPartyCnt, m_vInsuredName)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get default insured name", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)

                    Return nResult
                End If

                txtInsuredName.Text = "" & CStr(m_vInsuredName)

                'get default lead agent details

                m_lReturn = m_oBusiness.GetLeadAgentUsingAgentCnt(m_lPartyCnt, m_sAgentName, m_vLeadAgentCnt, oDateCancelled)

                m_bIsAgentAttachedWithClient = m_sAgentName.Trim() <> ""

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get default Lead Agent", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)

                    Return nResult
                End If

                pnlAgentCode.Text = "" & m_sAgentName

                If CInt(m_vLeadAgentCnt) <> 0 Then

                    m_lReturn = m_oBusiness.IsAgentAllowCommissionUsingAgentCnt(v_vPartyCnt:=m_vLeadAgentCnt,
                                                                                r_vAgentAllowedCommission:=
                                                                                   m_iAgentAllowedCommission)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="Failed to get default Lead Agent", vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)

                        Return nResult
                    End If
                End If

                If CStr(m_vLeadAgentCnt) <> "" Then
                    ' Need to check whether the agent has been cancelled or not
                    dtDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

                    If (oDateCancelled <> "" AndAlso CDate(oDateCancelled) <= DateTime.Today) AndAlso (CDate(oDateCancelled) <> dtDefaultDate) Then
                        System.Windows.Forms.MessageBox.Show("Agency cancelled - no new transactions can be placed through this " &
                                                             "agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        pnlAgentCode.Text = ""
                        m_sAgentName = ""

                        m_vLeadAgentCnt = 0
                    End If
                End If
                m_lReturn = GetPolicyNumber(sPolicyRef:=sPolicyRef)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sPolicyRef = "") Then
                    txtPolicyNumber.Text = ""
                Else
                    txtPolicyNumber.Text = sPolicyRef
                End If
                Dim oIsMidnightRenewal As Object = Nothing

                'default the dates

                m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Product", v_vFieldName:="is_midnight_renewal",
                                                     v_vKeyField:="product_id", v_vKeyID:=m_lProductID,
                                                     r_vResult:=oIsMidnightRenewal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If IsNothing(oIsMidnightRenewal) = False Then
                    bIsMidnightRenewal = ToSafeBoolean(oIsMidnightRenewal)
                Else
                    bIsMidnightRenewal = False
                End If

                ' as the midnight renewal indicator has already been retrieved use it.
                If bIsMidnightRenewal Then
                    m_iMidnightRenewal = gPMFunctions.ToSafeInteger(bIsMidnightRenewal)
                Else
                    m_iMidnightRenewal = 0
                End If

                m_lReturn = m_oFormFields.FormatControl(txtCoverFromDate, DateTime.Today)
                m_lReturn = m_oFormFields.FormatControl(txtInceptionDate, DateTime.Today)
                m_lReturn = m_oFormFields.FormatControl(txtInceptionTPI, DateTime.Today)

                m_lReturn = SetUnderwritingYear()

                If m_bIsTrueMonthlyPolicy Then

                    If bIsMidnightRenewal Then
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateTime.Today.AddMonths(1).AddDays(-1))
                    Else
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateTime.Today.AddMonths(1))
                    End If

                    m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, DateTime.Today.AddMonths(1))

                Else

                    If (bIsMidnightRenewal) Then
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateTime.Today.AddYears(1).AddDays(-1))
                    Else
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateTime.Today.AddYears(1))
                    End If
                    m_lReturn = m_oBusiness.GetFromTable(v_vTableName:="Product",
                                                v_vFieldName:="default_cover_to_date_to_last_day",
                                                v_vKeyField:="product_id",
                                                v_vKeyID:=m_lProductID,
                                                r_vResult:=m_oDefaultCoverToDateToLastDay)
                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        SetInterfaceDefaults = PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    If m_oDefaultCoverToDateToLastDay IsNot Nothing AndAlso ToSafeBoolean(m_oDefaultCoverToDateToLastDay) Then
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateAdd("d", -1 * (DateAndTime.Day(CDate(txtCoverToDate.Text).AddDays(m_iMidnightRenewal)) - CLng(m_iMidnightRenewal)), txtCoverToDate.Text))
                        If CLng(bIsMidnightRenewal) = 1 Then
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, DateAdd("d", 1, txtCoverToDate))
                        Else
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, txtCoverToDate.Text)
                        End If
                    Else
                        m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, DateTime.Today.AddYears(1))

                    End If

                    HandleLeapYearExtraDay()
                End If

                ' set the renewal day number
                If m_lRenewalDayNumber <> 0 Then
                    SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                    ProcessTMPDates(kProcessRenewalDatesFromRenewalDayNumber)
                Else
                    SelectcboItem(cboRenewalDayNumber, DateAndTime.Day(CDate(txtRenewalDate.Text)))
                End If

                chkQuote.CheckState = CheckState.Checked

            End If

            cmdDeletePolicyWording.Enabled = False

            cboStatus.Enabled = Not (m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "REN" Or m_sTransactionType = "")

            If m_sTransactionType = "" Then
                txtPolicyNumber.Enabled = False
            End If

            If m_bMultiTreeAccounting Then
                cboBranchCode.Enabled = False
            End If
            'sj 23/09/2002 - end

            ' PW311002 - Get the NRMA flag

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, v_vBranch:=g_iSourceID, r_vUnderwriting:=oIsNRMA)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If
            m_bIsNRMA = (oIsNRMA = "1")

            If m_bIsNRMA And (m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR") Then
                cboBranchCode.Enabled = False
            End If

            txtRenewalDate.Enabled = False

            txtInceptionDate.Enabled = False

            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Or m_sTransactionType = "MTC" Then
                txtQuoteExpiryDate.Enabled = False
            End If

            If m_sTransactionType = "MTA" Then
                txtCoverFromDate.Enabled = False
            End If

            If m_sTransactionType <> "NB" Then
                If IsRenewal Then
                    cboCurrency.Enabled = m_lCurrencyChange = CHANGECURRENCYTRUE
                End If
            Else
                cboCurrency.Enabled = True
            End If

            If m_sTransactionType = "MTC" Then
                txtCoverFromDate.Enabled = False
                txtCoverToDate.Enabled = False
                ' Also this field becomes mandatory
                lblLapsedDate.Font = VB6.FontChangeBold(lblLapsedDate.Font, True)
                txtInsuredName.Enabled = False
                cboAnalysisCode.Enabled = False
                cboSubBranch.Enabled = False
                cboBusinessType.Enabled = False
                cmdHandler.Enabled = False
                txtPolicyNumber.Enabled = False
                cboBranchCode.Enabled = False
                cboStatus.Enabled = False
                cboPolicyStatus.Enabled = False
            End If

            m_bSetUp = False

            m_dPrevDiscountPercentage = 0
            m_crPrevDiscountedPremium = 0

            If CInt(m_vLeadAgentCnt) <> 0 Then


                nResult = m_oBusiness.GetAgentDetail(m_vLeadAgentCnt, oResults)

                If nResult = gPMConstants.PMEReturnCode.PMFalse Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get default Lead Agent", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)

                    Return nResult
                End If
            End If
            If Information.IsArray(oResults) Then
                m_dtCommonRenewalDate = gPMFunctions.ToSafeDate(oResults(0, 0), DateTime.MinValue)
                m_sPartyCategoryCode = gPMFunctions.ToSafeString(oResults(1, 0))
                m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(oResults(2, 0))
            End If

            nResult = AgentChangeLogic()

            nResult = GetAgentChangeAuthority()
            If nResult = gPMConstants.PMEReturnCode.PMFalse Then
                SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to get authority to change Lead Agent", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="SetInterfaceDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                Exit Function
            End If
            If (m_sTransactionType = "MTA" Or m_sTransactionType = "MTC") And
                m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdAgentCode.Enabled = m_bAgentEditable
            End If

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                nResult = m_oBusiness.GetDefaultPreferredCorrespondence(m_lPartyCnt, m_vClientDefaultPreferredCorrespondence)
                If nResult = gPMConstants.PMEReturnCode.PMFalse Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get default Client Preferred Correspondence ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
            End If
            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyNumber
    '
    ' Description:
    '
    ' History: 20/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetPolicyNumber(ByRef sPolicyRef As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumber Is Nothing Then
                Dim temp_m_oPolicyNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPolicyNumber = temp_m_oPolicyNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



            'Developer Guide No. 8
            m_lReturn = m_oPolicyNumber.GeneratePolicyNumber(v_lBusinessType:=1, v_iBranch:=m_iSourceId, v_lProductId:=m_lProductID, v_lAgent:=CInt(m_vLeadAgentCnt), r_sGeneratedPolicyNumber:=sPolicyRef, v_lPartyCnt:=PartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' SetValuesFromArray - ets the values from the array.
    ''' </summary>
    ''' <param name="v_vFieldArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetValuesFromArray(ByVal v_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lInsuranceFileCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt))

            m_lInsuranceFileStructureID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID))

            m_lInsuranceFileTypeID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID))


            m_vInsuranceFileStatusID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)

            m_lInsuranceFileID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileID))

            m_iSourceId = CInt(v_vFieldArray(InsuranceFileConst.ACSourceID))

            m_lInsuranceFolderCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt))

            m_sInsuranceRef = CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceRef))

            m_lProductID = CInt(v_vFieldArray(InsuranceFileConst.ACProductID))


            m_vLeadInsurerCnt = v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)


            m_vLeadAgentCnt = v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)


            m_vLeadAgentPercent = v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)


            m_vAccountHandlerCnt = v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)

            m_lInsuredCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuredCnt))


            m_vBusinessTypeId = v_vFieldArray(InsuranceFileConst.ACBusinessTypeID)


            m_vCollectTypeID = v_vFieldArray(InsuranceFileConst.ACCollectTypeID)


            m_vCollectionFromCnt = v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)
            'sj 19/07/2002 - start
            'm_iBranchID = v_vFieldArray(ACBranchID)


            m_vSubBranchId = v_vFieldArray(InsuranceFileConst.ACSubBranchID)
            'sj 19/07/2002 - end

            m_iCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACCurrencyID))

            m_iBaseCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACBaseCurrencyID))

            m_iLanguageID = CInt(v_vFieldArray(InsuranceFileConst.ACLanguageID))


            m_vDateIssued = v_vFieldArray(InsuranceFileConst.ACDateIssued)

            m_dtCoverStartDate = CDate(v_vFieldArray(InsuranceFileConst.ACCoverStartDate))

            m_dtExpiryDate = CDate(v_vFieldArray(InsuranceFileConst.ACExpiryDate))
            ' PW311002 - store the expiry date so we can see if it has been
            ' changed later on. heh heh.
            m_dtInitialCoverFromDate = m_dtCoverStartDate
            m_dtInitialCoverToDate = m_dtExpiryDate

            m_dtRenewalDate = CDate(v_vFieldArray(InsuranceFileConst.ACRenewalDate))


            m_vRenewalMethodID = v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)

            m_iRenewalFrequencyID = CInt(v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID))

            m_iIsReferredAtRenewal = CInt(v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal))


            m_vLapsedReasonID = v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)


            m_vLapsedDate = v_vFieldArray(InsuranceFileConst.ACLapsedDate)


            m_vLapsedDescription = v_vFieldArray(InsuranceFileConst.ACLapsedDescription)


            m_iIsReferredOnMta = v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)

            m_iPolicyVersion = CInt(v_vFieldArray(InsuranceFileConst.ACPolicyVersion))


            m_vGeminiPolicyStatus = v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)


            m_vGeminiBusinessType = v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)


            m_vDeferredInd = v_vFieldArray(InsuranceFileConst.ACDeferredInd)


            m_vPolicyIgnore = v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)


            m_vBrokerCnt = v_vFieldArray(InsuranceFileConst.ACBrokerCnt)


            m_vRiskCodeID = v_vFieldArray(InsuranceFileConst.ACRiskCodeId)


            m_vAnalysisCodeID = v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)


            m_vPolicyDeductiblesID = v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)


            m_vPolicyLimitsID = v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)
            'TN20000823 - Doc Ref 10

            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACProposalDate)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACProposalDate)) Then

                m_vProposalDate = ""
            Else


                m_vProposalDate = v_vFieldArray(InsuranceFileConst.ACProposalDate)
            End If


            m_vDiaryDate = v_vFieldArray(InsuranceFileConst.ACDiaryDate)


            m_vReviewDate = v_vFieldArray(InsuranceFileConst.ACReviewDate)


            m_vRenewalDayNumber = v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)


            m_vPolicyTypeId = v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)


            m_vIndicator = v_vFieldArray(InsuranceFileConst.ACIndicator)


            m_vClause = v_vFieldArray(InsuranceFileConst.ACClause)


            m_vCover = v_vFieldArray(InsuranceFileConst.ACCover)


            m_vArea = v_vFieldArray(InsuranceFileConst.ACArea)


            m_vLongTermUndertakingDate = v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)


            m_vRenewalStopCodeID = v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)


            m_vVBSType = v_vFieldArray(InsuranceFileConst.ACVBSType)


            m_vVBSStatus = v_vFieldArray(InsuranceFileConst.ACVBSStatus)


            m_vIsInsurerRateTable = v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)


            m_vIsRelatedPolicies = v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)


            m_vIsRetainedDocuments = v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)


            m_vSchemesPostcode = v_vFieldArray(InsuranceFileConst.ACSchemesPostcode)


            m_vPaidDirect = v_vFieldArray(InsuranceFileConst.ACPaidDirect)


            m_vScheme = v_vFieldArray(InsuranceFileConst.ACScheme)


            m_vBrokerageAmount = v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)


            m_vIsMinimumBrokerageFlag = v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)


            m_vAnnualPremium = v_vFieldArray(InsuranceFileConst.ACAnnualPremium)


            m_vThisPremium = v_vFieldArray(InsuranceFileConst.ACThisPremium)


            m_vNetPremium = v_vFieldArray(InsuranceFileConst.ACNetPremium)


            m_vCommissionAmount = v_vFieldArray(InsuranceFileConst.ACCommissionAmount)


            m_vIPTableAmount = v_vFieldArray(InsuranceFileConst.ACIPTableAmount)


            m_vIPTPercentage = v_vFieldArray(InsuranceFileConst.ACIPTPercentage)
            'Tomo190100
            'CTAF080200 - Check for null

            If Not (Convert.IsDBNull(m_vIPTPercentage) Or IsNothing(m_vIPTPercentage)) Then

                m_cIPTRate = CDec(m_vIPTPercentage)
            Else
                m_cIPTRate = 0
            End If


            m_vIsIPTOverridden = v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)


            m_vTaxAmount = v_vFieldArray(InsuranceFileConst.ACTaxAmount)


            m_vVatableAmount = v_vFieldArray(InsuranceFileConst.ACVatableAmount)


            m_vVatPercentage = v_vFieldArray(InsuranceFileConst.ACVatPercentage)


            m_vVatAmount = v_vFieldArray(InsuranceFileConst.ACVatAmount)


            m_vPaymentMethod = v_vFieldArray(InsuranceFileConst.ACPaymentMethod)


            m_vUserDefinedDataID = v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)
            'ECK 14/7/99


            m_vCommissionPercentage = v_vFieldArray(InsuranceFileConst.ACCommissionPercentage)

            'TN20000815 - Doc Ref 10 (Start)

            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACInsuredName)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACInsuredName)) Then

                m_vInsuredName = ""
            Else


                m_vInsuredName = v_vFieldArray(InsuranceFileConst.ACInsuredName)
            End If


            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAlternateReference)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACAlternateReference)) Then

                m_vAlternateReference = ""
            Else


                m_vAlternateReference = v_vFieldArray(InsuranceFileConst.ACAlternateReference)
            End If


            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)) Then

                m_vIsClientInvoiced = CheckState.Unchecked
            Else


                m_vIsClientInvoiced = v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)
            End If


            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)) Then

                m_vOldPolicyNumber = ""
            Else


                m_vOldPolicyNumber = v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)
            End If


            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)) Then

                m_vQuoteExpiryDate = ""
            Else


                m_vQuoteExpiryDate = v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)
            End If


            m_vAlternateAccountCnt = v_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt)
            'TN20000815 - Doc Ref 10 (End)

            'DJM 29/03/2004

            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)) Then
                m_lPolicyStyleID = -1
            Else

                m_lPolicyStyleID = CInt(v_vFieldArray(InsuranceFileConst.ACPolicyStyleID))
            End If

            'EK 300300


            'TODO
            m_vExemptAmount = (CDbl(CDbl(m_vNetPremium) - CDbl(m_vIPTableAmount)) - CDbl(m_vVatableAmount))


            m_vUnderwritingYearID = v_vFieldArray(InsuranceFileConst.ACUnderwritingYearID)


            m_vPolicyStatusID = v_vFieldArray(InsuranceFileConst.ACPolicyStatusID)


            m_vInceptionTPI = v_vFieldArray(InsuranceFileConst.ACInceptionTPI)

            'Policy Discount

            m_iDiscountReasonId = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountReasonID))
            m_crDiscountedPremium = gPMFunctions.ToSafeCurrency(v_vFieldArray(InsuranceFileConst.ACDiscountedPremium), 0)
            m_dDiscountPercentage = gPMFunctions.ToSafeDouble(v_vFieldArray(InsuranceFileConst.ACDiscountPercentage), 0)

            m_iMatchDiscountedPremiumFlag = CInt(v_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag))

            ' true monthly policies

            If _
                Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)) Or
                IsNothing(v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)) Then
                m_dtAnniversaryDate = DateTime.MinValue
            Else

                m_dtAnniversaryDate = CDate(v_vFieldArray(InsuranceFileConst.ACAnniversaryDate))
            End If


            m_lRenewalDayNumber = CInt(v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber))

            m_lPutOnNextInstalmentRenewal =
                CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal))

            m_lAnniversaryCopy = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy))


            m_lDiscountRecurringTypeID = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId))
            'TMP

            m_iLeadAllowConsolidatedCommission = CInt(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission))

            m_iSubAllowConsolidatedCommission = CInt(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission))

            m_dManualDiscountPercentage = gPMFunctions.ToSafeDouble(v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage), 0)

            m_vContactuserId = CInt(v_vFieldArray(InsuranceFileConst.ACContactuserId))

            m_sCoInsPlacement = gPMFunctions.ToSafeString(v_vFieldArray(InsuranceFileConst.kCoInsPlacement), "")
            m_iMTAReasonId = CInt(v_vFieldArray(InsuranceFileConst.ACMTAReasonId))
            m_iCorrespondenceType = CInt(v_vFieldArray(InsuranceFileConst.ACCorrespondenceType))
            m_bAgentReceiveCorrespondenceFlag = CBool(v_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence))
            If m_iCorrespondenceType = 1 Then
                If m_bAgentReceiveCorrespondenceFlag Then
                    m_lReturn = m_oBusiness.GetExistingPreferredCorrespondence(m_lInsuranceFileCnt, m_vAgentDefaultPreferredCorrespondence)
                Else
                    m_lReturn = m_oBusiness.GetExistingPreferredCorrespondence(m_lInsuranceFileCnt, m_vClientDefaultPreferredCorrespondence)
                End If
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get default Agent Preferred Correspondence ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            cboCorrespondenceMethod_ItemCodeChange()
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetValuesFromArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetValuesFromArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ''' <summary>
    ''' UnlockPolicy
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnlockPolicy() As Integer

        Dim result As Integer = 0
        ' Declare an instance of the Lock object.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_oPMLock IsNot Nothing Then

                m_lReturn = m_oPMLock.UnLockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=g_iUserId)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result

                End If
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateFees
    '
    ' Description: This deletes all the fee records then recreates
    ' them based on what's in the array
    '
    ' ***************************************************************** '
    Private Function UpdateFees() As Integer
        ' UNDERWRITING DOES NOT USE FEES HERE DO NOTHING
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: UpdateStandardWordings
    '
    ' Description: This deletes all the narrative records then recreates
    ' them based on what's in the array
    '
    ' ***************************************************************** '
    Private Function UpdateStandardWordings() As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim bFirst As Boolean
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Loop round the listview
            i = 1
            bFirst = True

            m_vStandardWordings = Nothing

            Do
                If i > lvwPolicyWording.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwPolicyWording.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim m_vStandardWordings(3, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve m_vStandardWordings(3, i - 1)
                    End If

                    'Code

                    m_vStandardWordings(0, i - 1) = oListItem.Text
                    'Caption

                    m_vStandardWordings(1, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text

                    'Narrative Code id


                    m_vStandardWordings.SetValue(Convert.ToString(oListItem.Tag), 2, i - 1)
                    If oListItem.Checked Then
                        m_vStandardWordings(3, i - 1) = Convert.ToString(1)
                    Else
                        m_vStandardWordings(3, i - 1) = Convert.ToString(0)
                    End If
                End If
                i += 1
            Loop

            'Delete old fees from database

            m_lReturn = m_oBusiness.DeleteStandardWordings(vInsuranceFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add new fees to database
            If Information.IsArray(m_vStandardWordings) Then

                m_lReturn = m_oBusiness.AddStandardWordings(vInsuranceFileCnt:=m_lInsuranceFileCnt, vStandardWordings:=m_vStandardWordings)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStandardWordings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' ValidateOK
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function ValidateOK() As Integer

        Dim nResult As Integer = 0
        Dim i As Integer
        Dim j As Integer
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean
        Dim nAddressCnt As Integer
        Dim dtSysDate As Date
        Dim dtOldDate As Date
        Dim oListItem As ListViewItem
        Dim oListItem2 As ListViewItem

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'TN20010126 Start
            If cboStatus.Enabled Then
                '        Select Case cboStatus.ListIndex
                If cboStatus.SelectedIndex > -1 Then
                    Select Case VB6.GetItemData(cboStatus, cboStatus.SelectedIndex)
                        Case 0, 1, 2
                        Case Else
                            System.Windows.Forms.MessageBox.Show("Only (Current, Lapsed, Cancelled) are valid", ACApp,
                                                                 MessageBoxButtons.OK)
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                    End Select
                End If
            End If
            'TN20010126 End

            If cboCurrency.SelectedIndex = -1 Then
                System.Windows.Forms.MessageBox.Show("Currency must be entered", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(23/04/2001) Get system option to limit Cover From Date months in advance.
            If m_sAdvanceMonthsAllowed = "" Then
                m_lReturn = GetSystemOption(v_iOptionNumber:=ACSysOptionCoverFromAdvanceMonthsAllowed,
                                            r_sResult:=m_sAdvanceMonthsAllowed)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="Failed to retrieve the System Option (1008) from the business object",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK")
                End If
            End If

            'RWH(23/04/2001) Check 'Cover From Date' against system option.
            If gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim, DateTime.Now) > DateTime.Now.AddMonths(Conversion.Val(m_sAdvanceMonthsAllowed)) Then
                If System.Windows.Forms.MessageBox.Show("'Cover From Date' more than " & m_sAdvanceMonthsAllowed & " months in the future. Please confirm this is correct.", "Policy", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            'If _
            '    gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim, DateTime.Now) >
            '    DateTime.Now.AddMonths(Conversion.Val(m_sAdvanceMonthsAllowed)) Then
            '    If _
            '        System.Windows.Forms.MessageBox.Show(
            '            "'Cover From Date' more than " & m_sAdvanceMonthsAllowed &
            '            " months in the future. Please confirm this is correct.", "Policy", MessageBoxButtons.YesNo,
            '            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) =
            '        System.Windows.Forms.DialogResult.No Then
            '        Return gPMConstants.PMEReturnCode.PMFalse
            '    End If
            'End If

            If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Then
                ' PW311002 Get system option to limit Cover To Date months in advance.
                If m_sCoverToMonthsAllowed = "" Then
                    m_lReturn = GetSystemOption(v_iOptionNumber:=ACSysOptionCoverToAdvanceMonthsAllowed,
                                                r_sResult:=m_sCoverToMonthsAllowed)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError,
                                           sMsg:=
                                              "Failed to retrieve the System Option (1009) from the business object",
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK")
                    End If
                End If

                ' PW311002 Check 'Cover To Date' against system option.
                If _
                    gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim, DateTime.Now) >
                    gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim, DateTime.Now).AddMonths(
                        Conversion.Val(m_sCoverToMonthsAllowed)) Then
                    System.Windows.Forms.MessageBox.Show("'Cover To' Date cannot be greater than 'Cover From' Date + " &
                                                         m_sCoverToMonthsAllowed & " months.", Application.ProductName,
                                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'If DateAndTime.DateDiff("d", gPMFunctions.ToSafeDate(txtCoverFromDate), gPMFunctions.ToSafeDate(txtCoverToDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
            If _
                DateAndTime.DateDiff("d", CDate(txtCoverFromDate.Text.Trim), CDate(txtCoverToDate.Text.Trim),
                                     FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                System.Windows.Forms.MessageBox.Show("Cover To Date cannot be less than Cover From Date", "Invalid date",
                                                     MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim vValue As Object
            If m_sUnderwritingType = "U" Then

                m_lReturn = GetSystemOption(ACCoInsuranceLinktoAgent, m_sCoInsuranceLinktoAgent)


                'Developer Guide No. 98
                m_lReturn =
                    iPMFunc.getProductOptionValue(
                        v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions,
                        v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)

                If pnlAgentCode.Text.Trim() = "" Then
                    If m_sCoInsuranceLinktoAgent = "1" Then

                        Select Case cboBusinessType.SelectedItem.ItemData
                            Case 1 'Direct Business

                            Case 3, 4
                                If m_sTransactionType = "NB" OrElse m_sTransactionType = "MTA" Then
                                    If System.Windows.Forms.MessageBox.Show("No Agent Selected. Do you still wish to proceed further?", "Co Insurance with agent", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.Cancel Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                End If
                            Case Else
                                System.Windows.Forms.MessageBox.Show(
                                    "An agent must be assigned when business is not direct.", "Agent Validation",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return gPMConstants.PMEReturnCode.PMFalse
                        End Select

                    ElseIf VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 1 Then
                        System.Windows.Forms.MessageBox.Show("An agent must be assigned when business is not direct.",
                                                             "Agent Validation", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
                If m_sPartyCategoryCode.Trim().ToUpper() = "NONB" And m_sTransactionType = "NB" Then
                    System.Windows.Forms.MessageBox.Show("This agent can no longer accept New Business.",
                                                         "No New Business allowed", MessageBoxButtons.OK,
                                                         MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If _
                    (pnlAgentCode.Text.Trim() = "") And
                    VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 1 Then
                    System.Windows.Forms.MessageBox.Show("An agent must be assigned when business is not direct.",
                                                         "Agent Validation", MessageBoxButtons.OK,
                                                         MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If CBool(m_vPolicyStyleMandatory) And cboPolicyStyle.ItemId = 0 Then
                System.Windows.Forms.MessageBox.Show("This is a mandatory field. You must enter data in this field.",
                                                     "Mandatory Field - Policy Style", MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If either one is filled validate them
            If _
                (txtCoverNoteBook.Text.Trim() <> "" Or txtCoverNoteSheet.Text.Trim() <> "") And
                VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 7 And m_sTransactionType = "NB" And
                m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                If ValidateCoverNote() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Start Gaurav - PN 56442 -
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Adjustments Prior to Latest MTA Effective Date.doc) - (5.2.1.1)
            'Check if backdating MTAs are allowed

            If m_sTransactionType = "MTA" And m_bIsMTATemp Then
                If (gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim) = gPMFunctions.ToSafeDate(m_dtExpiryDate) Or
                     gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim) = gPMFunctions.ToSafeDate(m_dtInitialCoverFromDate)) OrElse
                    (gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim) < gPMFunctions.ToSafeDate(m_dtExpiryDate) AndAlso
                    gPMFunctions.ToSafeDate(txtCoverToDate.Text.Trim) > gPMFunctions.ToSafeDate(m_dtInitialCoverFromDate)) Then
                    System.Windows.Forms.MessageBox.Show(
                        "This is a Temporary MTA. The change is not permanent. Another permanent change may be required if the change is to be automatically applied at the next renewal, permanent MTA or cancellation.",
                        "Policy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Adjustments Prior to Latest MTA Effective Date.doc) - (5.2.1.1)
            Dim bIsValid As Boolean = False
            Dim sValue As String = ""
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, 1,
                                                      sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:=
                                      "Failed to get product option " &
                                      gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
            End If
            If m_bIsSubAgentAdded AndAlso sValue = "1" Then
                ValidateCertificateYear(bIsValid, sValue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'validate agent and client preferred correspondence
            Dim temp_oBusiness As Object
            Dim obSIRParty As Object
            Dim v_resultArray As Object(,)
            Dim sPreferredCorrespondence As String = String.Empty
            Dim bValid As Boolean = False

            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            obSIRParty = temp_oBusiness

            If (m_sTransactionType = "NB") OrElse (m_sTransactionType = "REN") OrElse (m_sTransactionType = "MTR") Then
                If _
                    (cboBusinessType.Text.Trim() = "Co-Insurance Lead") Or
                    (cboBusinessType.Text.Trim() = "Co-Insurance Follow") Then
                    If (optGross.Checked = False) And (optNet.Checked = False) Then
                        System.Windows.Forms.MessageBox.Show("Please Select GROSS or NET.",
                                                             "Co-Insurance Placement Validation", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Information)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'EH016736 Start
            If m_iMidnightRenewal = 0 AndAlso DateAndTime.DateDiff("d", CDate(txtCoverFromDate.Text.Trim), CDate(txtCoverToDate.Text.Trim), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0 Then
                System.Windows.Forms.MessageBox.Show("The Effective date and the Expiration date are the same.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EH016736 End

            obSIRParty.Dispose()
            obSIRParty = Nothing
            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOK Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidatePolicyNumber
    '
    ' Description:
    '
    ' History: 27/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ValidatePolicyNumber() As Integer
        Dim result As Integer = 0

        Dim sFailureReason As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumber Is Nothing Then
                Dim temp_m_oPolicyNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPolicyNumber = temp_m_oPolicyNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oPolicyNumber.ValidatePolicyNumber(sEnteredNumber:=txtPolicyNumber.Text, sFailureReason:=sFailureReason, lInsuranceFolderCnt:=m_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                System.Windows.Forms.MessageBox.Show(sFailureReason, "Policy Control", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePolicyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    '
    ' Name      : UnLoadAgents
    ' Desc      : unload sub-agents from listview into m_vAgentList
    ' History   : 18/08/2000 Tinny (Created)
    '
    '*******************************************************************************
    Private Function UnloadAgents() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwAgents.Items.Count > 0 Then
                ReDim m_vAgentList(4, 0)

                oListItem = lvwAgents.Items.Item(0)


                m_vAgentList(0, 0) = Convert.ToString(oListItem.Tag)
                m_vAgentList(1, 0) = oListItem.Text
                m_vAgentList(2, 0) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text

                m_vAgentList(3, 0) = 0
                m_vAgentList(4, 0) = 0

                For lCount As Integer = 2 To lvwAgents.Items.Count
                    ReDim Preserve m_vAgentList(4, m_vAgentList.GetUpperBound(1) + 1)

                    oListItem = lvwAgents.Items.Item(lCount - 1)


                    m_vAgentList(0, m_vAgentList.GetUpperBound(1)) = Convert.ToString(oListItem.Tag)
                    m_vAgentList(1, m_vAgentList.GetUpperBound(1)) = oListItem.Text
                    m_vAgentList(2, m_vAgentList.GetUpperBound(1)) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text

                    m_vAgentList(3, m_vAgentList.GetUpperBound(1)) = 0
                    m_vAgentList(4, m_vAgentList.GetUpperBound(1)) = 0
                Next
                'AG - 28/10/2004 - PN 15991
                'Added Else condition if subagent does not exist.
            Else
                m_vAgentList = VB6.CopyArray(Nothing)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadAgents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*******************************************************************************
    '
    ' Name      : LoadAgents
    ' Desc      : load sub-agents onto listview
    ' History   : 18/08/2000 Tinny (Created)
    '
    '*******************************************************************************
    Private Function LoadAgents() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwAgents.Items.Clear()

            If Information.IsArray(m_vAgentList) Then
                For lCount As Integer = 0 To m_vAgentList.GetUpperBound(1)

                    'Code

                    oListItem = lvwAgents.Items.Add(CStr(m_vAgentList(1, lCount)).Trim(), "AgentImage")

                    'Name
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAgentList(2, lCount)).Trim()

                    ' Percentage
                    If m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=m_vAgentList(3, lCount)) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtPercentage.Text

                    'Value
                    If m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=m_vAgentList(4, lCount)) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtAmount.Text

                    'party_cnt
                    oListItem.Tag = CStr(m_vAgentList(0, lCount))

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lCount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwAgents.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwAgents.Refresh()
                    End If
                    If lvwAgents.Items.Count = 1 Then
                        lvwAgents.Sorting = SortOrder.Ascending
                        lvwAgents.Sorting = SortOrder.None
                    End If

                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadAgents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCoinsurance
    '
    ' Description:
    '
    ' History: 06/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetCoinsurance() As Integer

        Dim result As Integer = 0
        Dim oObject As iPMUCoinsurance.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if its not co-insurance lead then we are not interested
            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 3 And VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 4 Then
                Return result
            End If

            oObject = New iPMUCoinsurance.Interface_Renamed()

            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                System.Windows.Forms.MessageBox.Show("error init", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            m_lReturn = SetKeyArray()

            m_lReturn = oObject.SetKeys(vKeyArray:=m_vKeyArray)

            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                System.Windows.Forms.MessageBox.Show("error start", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status

            m_lReturn = oObject.GetKeys(vKeyArray:=m_vKeyArray)

            m_lReturn = GetKeyArray()

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeyArray
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SetKeyArray() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim m_vKeyArray(1, 5)

            ' Party Cnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            ' Insurance Folder Cnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lInsuranceFolderCnt

            ' Insurance File Cnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsuranceFileCnt
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lInsuranceFileCnt

            ' Insurance File Ref
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsReference
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sInsuranceRef

            ' Product Id
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductID
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductID

            ' Business Type Id
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameBusinessTypeId
            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex)
            'TN20001027    m_vKeyArray(PMKeyValue, 5) = m_lBusinessTypeId

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeyArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeyArray
    '
    ' Description:
    '
    ' History: 26/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetKeyArray() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vKeyArray) Then
                Return result
            End If

            For iLoop1 As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)


                Select Case m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
                    Case PMNavKeyConst.PMKeyNamePartyCnt
                        m_lPartyCnt = CInt(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt
                        m_lInsuranceFolderCnt = CInt(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        m_lInsuranceFileCnt = CInt(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameInsReference
                        m_sInsuranceRef = CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameProductID
                        m_lProductID = CInt(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                    Case PMNavKeyConst.PMKeyNameBusinessTypeId

                        m_vBusinessTypeId = m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)

                End Select

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetAgentCommission
    '
    ' Desc : get agent commission in view mode
    '
    ' Hist : 19/02/2001 Created - Tinny
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetAgentCommission) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetAgentCommission() As Integer
    '
    'Dim result As Integer = 0

    'Dim oObject As ClassInterface
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim temp_oObject As Object
    'result = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUAgentCommission.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oObject = temp_oObject
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return result
    'End If
    '

    'result = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
    '

    'oObject.InsuranceFileCnt = m_lInsuranceFileCnt
    '

    'result = oObject.Start()
    '

    'm_lReturn = oObject.Terminate()
    '
    'oObject = Nothing
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentCommission", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    '
    ' Name: GetRiskReinsurance
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskReinsurance() As Integer

        Dim result As Integer = 0


        Dim oObject As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUReinsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vTransactionType:="EDIT")


            oObject.InsuranceFileCnt = m_lInsuranceFileCnt


            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lStatus = oObject.Status

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***************************************************************************
    ' Name : SetControlEnable
    '
    ' Desc  : set control enabled status to v_iState for all control in the specified container
    '            except for those in the v_sException (eg v_sException = "tabMainTab,fraMain;")
    '            make sure it ends with ;
    ' Hist : 14/01/2001 Tinny - Created
    '        04/12/2002 PF - Amended to handle controls not in containers
    '***************************************************************************
    Private Function SetControlEnable(ByVal v_iState As Integer, Optional ByVal v_sContainerName As String = "", Optional ByVal v_sException As String = "") As Integer

        Dim result As Integer = 0
        Dim lFound As Integer
        Dim bInContainer As Boolean
        Dim sContainerName As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        v_sContainerName = v_sContainerName.ToUpper()
        v_sException = v_sException.ToUpper()

        'loop thru and set enabled property to v_iState
        For Each ctlControl As Control In Me.Controls_Renamed
            ' Check for container state
            If v_sContainerName.Length Then
                ' Default container name
                sContainerName = MyBase.Name

                ' Get control container, if available (if not stick with default)
                Try
                    sContainerName = ctlControl.Parent.Name

                Catch
                End Try



                ' Check for match
                bInContainer = (sContainerName.ToUpper() = v_sContainerName)
            Else
                bInContainer = True
            End If

            ' Process if in container
            If bInContainer Then
                ' Default found flag
                lFound = 0

                'check to see if control name is in our exception list
                If v_sException.Length Then
                    lFound = (v_sException.IndexOf(ctlControl.Name.ToUpper()) + 1)

                    'if we found one make sure its the correct one, and not just part of a longer control name
                    If lFound <> 0 Then
                        If v_sException.Substring(lFound + Strings.Len(ctlControl.Name) - 1, 1) <> ";" Then
                            lFound = 0 'its part of a longer control name....not interested
                        End If
                    End If
                End If

                'set enabled state if its not in our exception list
                If lFound = 0 Then
                    ControlHelper.SetEnabled(ctlControl, v_iState)
                End If
            End If
        Next ctlControl

        Return result

Err_SetControlEnable:

        'if a control hasn't got the enabled property then carry on to the next one in the collection
        If Information.Err().Number = 438 Then


        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetControlEnable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetControlEnable", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranchDetails
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '          04/12/2002 APS- Amended to used member variable when getting
    '                          branch details
    ' ***************************************************************** '
    Public Function GetSubBranchDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACSubBranchID As Integer = 0
            Const ACSubBranchDescription As Integer = 3

            Dim lIndex As Integer
            Dim vSubBranchArray(,) As Object

            cboSubBranch.Items.Clear()

            lIndex = cboBranchCode.SelectedIndex
            If lIndex < 0 Then
                Return result
            End If

            m_iSourceId = VB6.GetItemData(cboBranchCode, lIndex)


            m_lReturn = m_oBusiness.GetSubBranches(v_lSourceID:=m_iSourceId, r_vSubBranchArray:=vSubBranchArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If
            If Not Information.IsArray(vSubBranchArray) Then
                Return result
            End If

            For i As Integer = 0 To vSubBranchArray.GetUpperBound(1)
                Dim cboSubBranch_NewIndex As Integer = -1

                cboSubBranch_NewIndex = cboSubBranch.Items.Add(CStr(vSubBranchArray(ACSubBranchDescription, i)))

                VB6.SetItemData(cboSubBranch, cboSubBranch_NewIndex, CInt(vSubBranchArray(ACSubBranchID, i)))


                If CInt(vSubBranchArray(ACSubBranchID, i)) = CDbl(m_vSubBranchId) Then
                    cboSubBranch.SelectedIndex = cboSubBranch_NewIndex
                End If
            Next i


            If CDbl(m_vSubBranchId) = 0 Then
                cboSubBranch.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboAnalysisCode_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAnalysisCode.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
    End Sub

    Private Sub cboAnalysisCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAnalysisCode.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboAnalysisCode)
    End Sub

    Private Sub cboAnalysisCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAnalysisCode.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboAnalysisCode)
    End Sub

    Private Sub cboAnalysisCode_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboAnalysisCode.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
    End Sub

    Private Sub cboBranchCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranchCode.GotFocus

    End Sub

    ''' <summary>
    ''' cboBranchCode_SelectedIndexChanged
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cboBranchCode_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles cboBranchCode.SelectedIndexChanged
        If m_iOldSelectedBranchIndex <> VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex) Then
            Dim bValidBranch As Boolean
            pnlAgentCode.Text = ""
            m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
            If (m_sTransactionType = "MTA" Or m_sTransactionType = "MTC") And m_bAgentEditable = False And
                ToSafeInteger(m_vLeadAgentCnt) <> 0 Then
                m_lReturn = GetAssosiatedAgentBranch(bValidBranch)
                If bValidBranch = False Then
                    cboBranchCode.SelectedIndex = m_iOldSelectedBranchIndex
                    Exit Sub
                Else
                    m_iOldSelectedBranchIndex = cboBranchCode.SelectedIndex
                End If
            Else
                m_iOldSelectedBranchIndex = cboBranchCode.SelectedIndex
            End If
            If m_bUserMode Then
                m_lReturn = GetSubBranchDetails()
            End If

            m_lReturn = GetCurrenciesForBranch()

            'Agent Filtering
            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 1 And Not m_bIsTrueMonthlyPolicy Then
                If m_iTask = gPMConstants.PMEComponentAction.PMAdd And Not m_bIsAgentAttachedWithClient Then
                    If String.IsNullOrEmpty(m_vLeadAgentCnt) = False Then
                        m_lReturn = GetAssosiatedAgentBranch(bValidBranch)
                        If bValidBranch = False Then
                            m_lReturn = GetBranchDefaultAgent()
                        Else
                            pnlAgentCode.Text = m_sAgentName
                        End If
                    Else
                        m_lReturn = GetBranchDefaultAgent()
                    End If
                Else
                    pnlAgentCode.Text = m_sAgentName
                End If
            End If

            If m_bIgnoreHandler Then
                m_bIgnoreHandler = False
            Else
                If m_iTask <> PMEComponentAction.PMView Then
                    m_vAccountHandlerCnt = 0
                    pnlHandler.Tag = ""
                    lblHandler.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub cboBranchCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranchCode.Enter
        If cboBranchCode.SelectedIndex >= 0 Then
            m_iOldSelectedBranchIndex = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
            m_lReturn = PMBGeneralFunc.ControlGotFocus(cboBranchCode)
        End If
    End Sub

    Private Sub cboBranchCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranchCode.Leave
        If cboBranchCode.SelectedIndex >= 0 Then
            If m_iOldSelectedBranchIndex <> VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex) Then
                m_lReturn = PMBGeneralFunc.ControlLostFocus(cboBranchCode)
            End If
        End If
    End Sub

    Private Sub cboBranchCode_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboBranchCode.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
    End Sub
    ''' <summary>
    ''' cboBusinessType_DropDown
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cboBusinessType_DropDown()
        m_iPreviousBusinessType = cboBusinessType.SelectedIndex
    End Sub
    ''' <summary>
    ''' cboBusinessType_GotFocus
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboBusinessType_GotFocus(sender As Object, e As EventArgs) Handles cboBusinessType.GotFocus
        m_lReturn = ControlGotFocus(cboBusinessType)
        m_iPreviousBusinessType = cboBusinessType.SelectedIndex
    End Sub

    ''' <summary>
    ''' cboBusinessType_SelectionChangeCommitted
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cboBusinessType_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) _
        Handles cboBusinessType.SelectionChangeCommitted

        Dim oResults As Object

        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

        'can't do co-insurance if we are adding
        If Task <> gPMConstants.PMEComponentAction.PMAdd Then
            'only enable button if its 'co-insurer lead'
            cmdCoInsurer.Enabled = VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 3
        End If

        ' if it is Direct Business We dodn't need the Agent Code.
        If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 1 Then
            If (m_sTransactionType = "MTA" Or m_sTransactionType = "MTC") And m_bAgentEditable = False And
                m_vLeadAgentCnt <> 0 Then
                RemoveHandler cboBusinessType.GotFocus, AddressOf cboBusinessType_GotFocus
                MsgBox(
                    "Cannot Change to Direct Business this will remove the Agent. Insufficient User Authority to remove Agent",
                    vbOKOnly, "Remove")
                AddHandler cboBusinessType.GotFocus, AddressOf cboBusinessType_GotFocus
                cboBusinessType.SelectedIndex = m_iPreviousBusinessType
                cmdAgentCode.Enabled = False ' Disable the Command Button
                Exit Sub
            Else
                cmdAgentCode.Enabled = False
            End If
            m_lReturn = SetTrueMonthlyPolicy(True)
            pnlAgentCode.Text = ""

            cboAgencyContact.Enabled = False 'Disable the agency contact combo

            ' Removes associated sub-agents
            If m_sUnderwritingType = "U" Then
                If lvwAgents.Items.Count > 0 And Information.IsArray(m_vGetAssociatedSubAgent) Then

                    ' m_vGetAssociatedSubAgent array has the list of associated sub agents.
                    For iLen As Integer = m_vGetAssociatedSubAgent.GetLowerBound(1) To m_vGetAssociatedSubAgent.GetUpperBound(1)
                        ' lvwAgents has all the sub agents.
                        For iIndex As Integer = 1 To lvwAgents.Items.Count
                            If lvwAgents.Items.Count >= iIndex Then
                                ' Check for the associated sub agents.
                                If Convert.ToString(lvwAgents.Items.Item(iIndex - 1).Tag) =
                                    gPMFunctions.ToSafeLong(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, iLen)) Then
                                    lvwAgents.Items.RemoveAt(iIndex - 1)
                                    lvwAgents.Refresh()
                                End If
                            End If
                        Next
                    Next

                    ' Clears the array as no associated sub agents is in the lvwAgents.
                    If Information.IsArray(m_vGetAssociatedSubAgent) Then
                        m_vGetAssociatedSubAgent = Nothing
                    End If

                End If
            End If

        Else
            If pnlAgentCode.Text.Trim() = "" And Not m_bIsTrueMonthlyPolicy And cboBranchCode.SelectedIndex <> -1 Then _
                cboBranchCode_SelectedIndexChanged(cboBranchCode, New EventArgs())
            cmdAgentCode.Enabled = Not (m_iTask = gPMConstants.PMEComponentAction.PMView)
            cboAgencyContact.Enabled = True
            If (m_sTransactionType = "MTA" Or m_sTransactionType = "MTC") And m_bAgentEditable = False And
                ToSafeInteger(m_vLeadAgentCnt) <> 0 Then
                cmdAgentCode.Enabled = False ' Disable the Command Button
            End If
            ' TMP Set the Interface For Business Other than Agency
            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 7 Then
                m_lReturn = SetTrueMonthlyPolicy(False)
            Else
                m_lReturn = SetTrueMonthlyPolicy(True)
            End If
        End If
        If (m_iBusinessType_Orig = 3 Or m_iBusinessType_Orig = 4) And
            (VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 4 Or
             VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 3) Then
            If m_sAgentCode_Orig = "" Then
                cmdAgentCode.Enabled = False
                pnlAgentCode.Text = ""
            End If
        End If

        'Event raise when Business Type is change
        If cboBusinessType.SelectedIndex <> -1 Then
            RaiseEvent BusinessTypeChange(Me, New BusinessTypeChangeEventArgs(VB6.GetItemData(cboBusinessType,
                                                                                   cboBusinessType.SelectedIndex)))
        End If

        SetAltRefMandatory()

        'Cover Note Validation
        m_lReturn = SetCoverNoteInterface()

        If pnlAgentCode.Text.Trim() = "" Then
            m_dtCommonRenewalDate = DateTime.MinValue
            m_sPartyCategoryCode = ""
            m_bIsSingleInstalmentPlan = False
            cboAgencyContact.SelectedIndex = -1
        Else

            If CDbl(m_vLeadAgentCnt) <> 0 Then
                GetAgencyContactDetails(m_vLeadAgentCnt)
                m_lReturn = m_oBusiness.GetAgentDetail(v_lAgentCnt:=m_vLeadAgentCnt, r_vResults:=oResults)
            End If
            If Information.IsArray(oResults) Then
                m_dtCommonRenewalDate = gPMFunctions.ToSafeDate(oResults(0, 0), DateTime.MinValue)
                m_sPartyCategoryCode = gPMFunctions.ToSafeString(oResults(1, 0))
                m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(oResults(2, 0))
            End If
        End If

        If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 3 Or VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 4 Then
            cmdCoInsurer.Enabled = True
        Else
            cmdCoInsurer.Enabled = False
        End If
        m_lReturn = AgentChangeLogic()

    End Sub

    Private Sub cboBusinessType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBusinessType.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboBusinessType)

    End Sub


    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)
    Private Sub chkPutOnNextInstalmentRenewal_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPutOnNextInstalmentRenewal.CheckStateChanged

    End Sub
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1)

    Private Sub cmdCoInsurer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCoInsurer.Click

        Try

            m_lReturn = GetCoinsurance()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdCoInsurer_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCoInsurer_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cboBusinessType_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboBusinessType.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboBusinessType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBusinessType.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboBusinessType)

    End Sub



    Private Sub txtFutureAnnualPremium_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFutureAnnualPremium.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFutureAnnualPremium)
    End Sub

    Private Sub txtFutureAnnualPremium_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFutureAnnualPremium.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFutureAnnualPremium)
    End Sub

    Private Sub cboCurrency_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboCurrency_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboCurrency)

    End Sub

    Private Sub cboCurrency_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCurrency.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboCurrency_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboCurrency)

    End Sub

    ' ***************************************************************** '
    ' Name: cboDiscountReason_Click
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Sub cboDiscountReason_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDiscountReason.Click

        Const kMethodName As String = "cboDiscountReason_Click"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            lReturn = ActionDiscountReasonChange()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ActionDiscountReasonChange Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub cboFrequency_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFrequency.TextChanged

        ' To avoid executing all the process no change is done by user in Frequency in case of Requote
        If tabMainTab.TabPages(tabMainTab.SelectedIndex).Controls.Find("cboFrequency", True).Length = 0 Then
            If m_lInsuranceFileCnt <> 0 Then
                Exit Sub
            End If
        End If

        'Select item while text is being typed in Frequency and exit in case of garbage value (that is not a part the combo values)
        If cboFrequency.FindStringExact(cboFrequency.Text) = -1 Then
            Exit Sub
        Else
            cboFrequency.SelectedIndex = cboFrequency.FindStringExact(cboFrequency.Text)
        End If

        If isInitializingComponent Then
            Exit Sub
        End If

        Dim dNumber As Double

        If m_bSetUp Then
            Exit Sub
        End If

        If cboFrequency.SelectedIndex = -1 Then
            Exit Sub
        End If

        'Now we take the start date and rejig everything else
        If txtCoverFromDate.Text.Trim() = "" Then
            Exit Sub
        End If

        If m_sTransactionType = "MTA" Or m_sTransactionType = "MTR" Then
            Exit Sub
        End If

        Dim dtDate As Date = CDate(m_oFormFields.UnformatControl(txtCoverFromDate))
        m_oBusiness.GetRenewalFrequencyMonths(cboFrequency.SelectedItem.ItemData, dNumber)
        If m_bIsTrueMonthlyPolicy AndAlso gPMFunctions.ToSafeInteger(m_lRenewalDayNumber) > 0 Then
            m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, CDate(m_oFormFields.UnformatControl(txtCoverToDate)))
        Else
            dtDate = DateAndTime.DateAdd("m", dNumber, dtDate)
            If m_iMidnightRenewal = 1 Then
                dtDate = dtDate.AddDays(-1)
                m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, dtDate)
                dtDate = dtDate.AddDays(1)
            Else
                m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, dtDate)
            End If
            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
        End If

    End Sub

    Private Sub cboLapsedReason_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLapsedReason.SelectionChangeCommitted

        Dim lReturn As gPMConstants.PMEReturnCode = CType(PMBGeneralFunc.FieldOnControlChange(Me), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub cboLapsedReason_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLapsedReason.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboLapsedReason)

    End Sub

    Private Sub cboLapsedReason_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboLapsedReason.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboLapsedReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLapsedReason.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboLapsedReason)

    End Sub

    Private Sub cboProduct_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
    End Sub

    Private Sub cboProduct_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboProduct)
    End Sub

    Private Sub cboProduct_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProduct.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboProduct)
    End Sub

    Private Sub cboProduct_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboProduct.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
    End Sub

    Private Sub cboRelationship_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRelationship.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboRelationship_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRelationship.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboRelationship)

    End Sub

    Private Sub cboRelationship_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboRelationship.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboRelationship_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRelationship.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboRelationship)
    End Sub

    Private Sub cboRenewalDayNumber_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalDayNumber.SelectionChangeCommitted
        ' process true monthly policy dates
        ProcessTMPDates(kProcessRenewalDatesFromRenewalDayNumber)
        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, CDate(m_oFormFields.UnformatControl(txtCoverToDate)))

    End Sub

    Private Sub cboRenewalMethod_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalMethod.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboRenewalMethod_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalMethod.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboRenewalMethod)

    End Sub

    Private Sub cboRenewalMethod_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboRenewalMethod.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboRenewalMethod_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalMethod.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboRenewalMethod)

    End Sub

    Private Sub cboRenewalStop_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalStop.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboRenewalStop_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalStop.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboRenewalStop)

    End Sub

    Private Sub cboRenewalStop_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboRenewalStop.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboRenewalStop_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalStop.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboRenewalStop)

    End Sub

    Private Sub cboStatus_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStatus.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboStatus_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStatus.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboStatus)

    End Sub

    Private Sub cboStatus_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboStatus.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboStatus_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStatus.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboStatus)

    End Sub



    Private Sub chkOverrideRateTable_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOverrideRateTable.CheckStateChanged
        If chkOverrideRateTable.CheckState = CheckState.Checked Then
            txtCommissionPercentage.Enabled = True
            txtCommissionCharge.Enabled = True
        End If
        If chkOverrideRateTable.CheckState = CheckState.Unchecked Then
            txtCommissionPercentage.Enabled = False
            txtCommissionCharge.Enabled = False
        End If
    End Sub

    Private Sub cmdAddAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAgent.Click

        Dim vCnt As Integer
        Dim vShortName As Object
        Dim vResolvedName As String = ""
        Dim oListItem As ListViewItem

        ' Ram 10-01-2001
        Dim vLongName As String = ""

        Try



            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vLongName, vResolvedName:=vResolvedName, vSpecialParty:="UB")

            Dim i_CommissionFlag As Integer
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'add to list view if not already in the list
                If IsInListView(vCnt, lvwAgents) = gPMConstants.PMEReturnCode.PMFalse Then

                    'Code


                    oListItem = lvwAgents.Items.Add(CStr(vShortName).Trim(), "AgentImage")

                    'Name
                    ' Ram 10-01-2001 (Use Long Name, if ResolvedName is Empty String)
                    If vResolvedName.Trim() <> "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = vResolvedName.Trim()
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = vLongName.Trim()
                    End If

                    'party_cnt
                    oListItem.Tag = CStr(vCnt)

                    ' refrest the list
                    lvwAgents.Refresh()
                    'TMP To check SubAgent is Allowed Consolidated Commision or not
                    If m_bISProductConfAllowSubConsolidatedCommission Then
                        If lvwAgents.Items.Count > 0 Then
                            For Each lvwItem As ListViewItem In lvwAgents.Items

                                m_lReturn = GetAllowConsolidatedCommissionValue(lvwItem.Text, i_CommissionFlag)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    ''
                                End If

                                If i_CommissionFlag = 1 Then
                                    chkConsolidatedSubCommission.CheckState = CheckState.Unchecked
                                    chkConsolidatedSubCommission.Enabled = True
                                    Exit For
                                End If
                                chkConsolidatedSubCommission.Enabled = False


                            Next lvwItem

                        End If
                    End If

                End If

            End If
            If lvwAgents.Items.Count = 1 Then
                lvwAgents.Sorting = SortOrder.Ascending
                lvwAgents.Sorting = SortOrder.None
            End If
            RaiseEvent SubAgentChange(Me, Nothing)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAgent_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    '*******************************************************************************
    ' Name : cmdAddClient_Click
    '
    ' Description : Add a new client to the list. We can assume they are not the
    '               lead or correspondence client as this can be set easily later.
    '
    ' History :
    '   21/07/2002 PWF (Created)
    '*******************************************************************************
    Private Sub cmdAddClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddClient.Click

        Dim lPartyCnt As Integer
        Dim sShortname, sResolved, sAddress As String

        Try

            ' Select a new insured client
            m_lReturn = SelectParty(vPartyCnt:=lPartyCnt, vShortName:=sShortname, vName:=sResolved, vAddress1:=sAddress)

            ' Did we get one?
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Check party is not already in list
                For Each oListItem As ListViewItem In lvwClients.Items
                    If Convert.ToString(oListItem.Tag) = lPartyCnt Then
                        ' Display error stating the problem.
                        MessageBox(ACCPDlgAddExists, ACCPDlgTitle, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, sResolved)

                        Exit Sub
                    End If
                Next oListItem

                ' Add new client to the list
                With lvwClients.Items.Add(sShortname, "AgentImage")

                    .Tag = CStr(lPartyCnt)
                    ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(sShortname, "AgentImage"), 1).Text = sResolved
                    ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(sShortname, "AgentImage"), 2).Text = sAddress
                    ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(sShortname, "AgentImage"), 3).Text = "N"
                    ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(sShortname, "AgentImage"), 4).Text = "N"
                End With
            End If

            ' Fire a fake click event to update button states
            'TODO
            lvwClients_ItemClick(lvwClients.CheckedItems(0))

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddClient_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub



    '*********************************************************************************'
    ' Name          :   cmdAddPolicyWording_Click
    ' Description   :   To Load the "iSIRPickDocTemplate.Interface"
    ' Created by    :   Arul Stephen
    '*********************************************************************************'

    'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)
    Private Sub cmdAddPolicyWording_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPolicyWording.Click

        Const kMethodName As String = "cmdAddPolicyWording_Click"
        Try


            Dim oListItem As ListViewItem
            'Arul
            Dim lNewRowCount As Integer
            'End

            'Start Arul PN63627
            Dim lDefaultClausesCount As Long
            Dim lNewClausesRowCount As Long
            Dim lRowCount As Long
            'End Arul PN63627

            'Create policy wording object if not already done so
            If m_oPolicyWording Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oPolicyWording As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyWording, sClassName:="iSIRPickDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oPolicyWording = temp_m_oPolicyWording

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    gPMFunctions.RaiseError(kMethodName, "iSIRPickDocTemplate.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
                    Exit Sub
                End If

            End If


            m_lReturn = m_oPolicyWording.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oPolicyWording.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If


            m_oPolicyWording.SourceId = m_iSourceId

            m_oPolicyWording.ProductId = m_lProductID

            m_oPolicyWording.ClauseId = MainModule.ENClauseType.ProductType



            'Start Arul PN63627
            m_oPolicyWording.Searchable = True
            ReDim Preserve m_vDefaultClauses(ENSelectClause.Description, 0)

            If lvwPolicyWording.Items.Count > 0 Then
                If IsArray(m_vStandardWordings) = True Then
                    For lDefaultClausesCount = 0 To lvwPolicyWording.Items.Count - 1
                        ReDim Preserve m_vDefaultClauses(ENSelectClause.Description, lNewClausesRowCount)
                        For lRowCount = LBound(m_vStandardWordings, ACSelectClasueRowIndex) To UBound(m_vStandardWordings, ACSelectClasueRowIndex)
                            If ToSafeString(lvwPolicyWording.Items(Integer.Parse(lDefaultClausesCount)).Text) = Trim$(m_vStandardWordings(kSelectClauseCode, lRowCount)) Then
                                m_vDefaultClauses(ENSelectClause.Description, lNewClausesRowCount) = m_vStandardWordings(kSelectClauseDescription, lRowCount)
                                Exit For
                            End If
                        Next lRowCount


                        m_vDefaultClauses(ENSelectClause.Code, lNewClausesRowCount) = lvwPolicyWording.Items(Integer.Parse(lDefaultClausesCount)).Text
                        m_vDefaultClauses(ENSelectClause.Id, lNewClausesRowCount) = lvwPolicyWording.Items(Integer.Parse(lDefaultClausesCount)).Tag
                        lNewClausesRowCount = lNewClausesRowCount + 1
                    Next
                    m_oPolicyWording.DefaultClauses = m_vDefaultClauses
                End If
            Else
                m_oPolicyWording.DefaultClauses = Nothing
            End If
            'End Arul PN63627

            Dim oSelectedStandardWording As Object
            If (lvwPolicyWording.Items.Count) > 0 Then
                ReDim oSelectedStandardWording(MainModule.ENSelectClause.Description, lvwPolicyWording.Items.Count - 1)
                For iCount As Integer = 0 To lvwPolicyWording.Items.Count - 1
                    oSelectedStandardWording(0, iCount) = lvwPolicyWording.Items(iCount).Tag
                    oSelectedStandardWording(1, iCount) = lvwPolicyWording.Items(iCount).SubItems(0).Text
                    oSelectedStandardWording(2, iCount) = lvwPolicyWording.Items(iCount).SubItems(1).Text
                Next
            End If
            m_oPolicyWording.DefaultClauses = oSelectedStandardWording
            m_oPolicyWording.CoverToDate = Convert.ToDateTime(txtCoverToDate.Text).ToString("yyyy-MM-dd")

            m_lReturn = m_oPolicyWording.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oPolicyWording.Start Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oPolicyWording.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Get the chosen clauses


            m_vStandardWordings = m_oPolicyWording.DocumentTemplate
            If IsArray(m_vStandardWordings) = False Then
                'Start Arul PN 63627
                lvwPolicyWording.Items.Clear()
                m_vStandardWordings = Nothing
                'End Arul PN 63627
                Exit Sub
            End If
            'Arul
            Dim m_vResultArray(MainModule.ENSelectClause.Description, 0) As Object


            For lSelectedItemcount As Integer = m_vStandardWordings.GetLowerBound(ACSelectClasueRowIndex - 1) To m_vStandardWordings.GetUpperBound(ACSelectClasueRowIndex - 1)
                ReDim Preserve m_vResultArray(MainModule.ENSelectClause.Description, lNewRowCount)


                m_vResultArray(MainModule.ENSelectClause.Id, lNewRowCount) = m_vStandardWordings(MainModule.ENSelectClause.Code, lSelectedItemcount)


                m_vResultArray(MainModule.ENSelectClause.Code, lNewRowCount) = m_vStandardWordings(MainModule.ENSelectClause.Description, lSelectedItemcount)


                m_vResultArray(MainModule.ENSelectClause.Description, lNewRowCount) = m_vStandardWordings(MainModule.ENSelectClause.Id, lSelectedItemcount)
                lNewRowCount += 1

            Next lSelectedItemcount

            m_vStandardWordings = m_vResultArray
            'End
            m_lReturn = PopulateStandardWordings()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateStandardWordings load Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.3.1.1)

    'UPGRADE_NOTE: (7001) The following declaration (PopulateStandardWordingsToGrid) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub PopulateStandardWordingsToGrid()
    '
    'Dim j, k As Integer
    'Dim sDesc, sTmp As String
    'Dim oListItem As ListViewItem
    '
    'An example.
    'Try 
    '
    'Just go if no fees
    'If Not Information.IsArray(v_Result) Then
    'Exit Sub
    'End If
    '
    'lvwPolicyWording.Items.Clear()
    '
    ' Assign the details to the interface.

    'For 'i As Integer = v_Result.GetLowerBound(0) To v_Result.GetUpperBound(0)
    '
    '

    'oListItem = lvwPolicyWording.Items.Add(CStr(v_Result(i)).Trim())
    '                Trim$(m_vStandardWordings(0, i)), , NarrativeImage)
    ''
    '        ' Assign details to other the columns
    '        ' Column 2
    '        oListItem.SubItems(1) = Trim$(m_vStandardWordings(1, i))
    ''
    '        oListItem.Tag = Trim$(m_vStandardWordings(2, i))
    '
    ' {* USER DEFINED CODE (End) *}
    ' Set the tag property with the index of
    ' the search data storage.
    '
    'Next i
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateStandardWordings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub cmdAgentCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentCode.Click

        Dim vCnt, vName, vShortName As Object
        Dim vResolvedName As String = ""
        Dim dDefaultDate As Date
        Dim vDateCancelled As Object
        Dim dTransactionDate As Date
        Dim vResults As Object

        Try

            ' PW190802 - suppress sub agents




            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="AG", bSuppressSubAgents:=True, vDateCancelled:=vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            dDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

            dTransactionDate = DateTime.Today

            If m_sSystemOption1040 = "1" Then
                dTransactionDate = CDate(txtCoverFromDate.Text)
            End If


            If (ToSafeDate(vDateCancelled, #12/29/1899#) <= dTransactionDate) And (ToSafeDate(vDateCancelled, #12/29/1899#) <> dDefaultDate) Then
                System.Windows.Forms.MessageBox.Show("Agency cancelled - no new transactions can be placed through this " &
                                                     "agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                pnlAgentCode.Text = ""

                m_vLeadAgentCnt = 0

                If m_sUnderwritingType = "U" Then
                    ' Removes associated sub-agents
                    If lvwAgents.Items.Count > 0 And Information.IsArray(m_vGetAssociatedSubAgent) Then

                        ' m_vGetAssociatedSubAgent array has the list of associated sub agents.
                        For iLen As Integer = m_vGetAssociatedSubAgent.GetLowerBound(1) To m_vGetAssociatedSubAgent.GetUpperBound(1)
                            ' lvwAgents has all the sub agents.
                            For iIndex As Integer = 1 To lvwAgents.Items.Count
                                If lvwAgents.Items.Count >= iIndex Then
                                    ' Check for the associated sub agents.
                                    If Convert.ToString(lvwAgents.Items.Item(iIndex - 1).Tag) = gPMFunctions.ToSafeLong(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, iLen)) Then
                                        lvwAgents.Items.RemoveAt(iIndex - 1)
                                        lvwAgents.Refresh()
                                    End If
                                End If
                            Next
                        Next

                        ' Clears the array as no associated sub agents is in the lvwAgents.
                        If Information.IsArray(m_vGetAssociatedSubAgent) Then
                            m_vGetAssociatedSubAgent = Nothing
                        End If

                    End If
                End If

                '(RC) QBENZ014
                SetAltRefMandatory()

                Exit Sub
            End If

            'save the count in the tag and update controls

            pnlAgentCode.Tag = CStr(vCnt)



            m_vLeadAgentCnt = vCnt

            If vResolvedName = "" Then

                pnlAgentCode.Text = CStr(vName)
            Else
                pnlAgentCode.Text = vResolvedName
            End If

            '(RC) QBENZ014
            SetAltRefMandatory()

            'Is agent Allow Commision

            m_lReturn = m_oBusiness.IsAgentAllowCommissionUsingAgentCnt(v_vPartyCnt:=vCnt, r_vAgentAllowedCommission:=m_iAgentAllowedCommission)

            If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 7 Then
                m_lReturn = SetTrueMonthlyPolicy(False)
            Else
                m_lReturn = SetTrueMonthlyPolicy(True)
            End If

            If m_sUnderwritingType = "U" Then
                ' Removes associated sub-agents
                If lvwAgents.Items.Count > 0 And Information.IsArray(m_vGetAssociatedSubAgent) Then

                    ' m_vGetAssociatedSubAgent array has the list of associated sub agents.
                    For iLen As Integer = m_vGetAssociatedSubAgent.GetLowerBound(1) To m_vGetAssociatedSubAgent.GetUpperBound(1)
                        ' lvwAgents has all the sub agents.
                        For iIndex As Integer = 1 To lvwAgents.Items.Count
                            If lvwAgents.Items.Count >= iIndex Then
                                ' Check for the associated sub agents.
                                If Convert.ToString(lvwAgents.Items.Item(iIndex - 1).Tag) = gPMFunctions.ToSafeLong(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, iLen)) Then
                                    lvwAgents.Items.RemoveAt(iIndex - 1)
                                    lvwAgents.Refresh()
                                End If
                            End If
                        Next
                    Next

                    ' Clears the array as no associated sub agents is in the lvwAgents.
                    If Information.IsArray(m_vGetAssociatedSubAgent) Then
                        m_vGetAssociatedSubAgent = Nothing
                    End If

                    System.Windows.Forms.MessageBox.Show("Lead Agent has been changed - any associated Sub-Agents will" &
                                                         " also be removed", "Policy", MessageBoxButtons.OK)
                End If

                ' Add Associated Sub-Agents in Listview if business type is Agency Business

                If CStr(m_vLeadAgentCnt) <> "" Then

                    m_lReturn = AddAssociatedSubAgent(v_lLeadAgentCnt:=CInt(m_vLeadAgentCnt))
                End If


                If CDbl(m_vLeadAgentCnt) <> 0 Then

                    m_lReturn = m_oBusiness.GetAgentDetail(v_lAgentCnt:=m_vLeadAgentCnt, r_vResults:=vResults)
                End If
                If Information.IsArray(vResults) Then
                    m_dtCommonRenewalDate = gPMFunctions.ToSafeDate(vResults(0, 0), DateTime.MinValue)
                    m_sPartyCategoryCode = gPMFunctions.ToSafeString(vResults(1, 0))
                    m_bIsSingleInstalmentPlan = gPMFunctions.ToSafeBoolean(vResults(2, 0))
                End If
                m_lReturn = m_oBusiness.GetDefaultPreferredCorrespondence(m_vLeadAgentCnt, m_vAgentDefaultPreferredCorrespondence)
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get default Agent Preferred Correspondence ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                cboCorrespondenceMethod_ItemCodeChange()
                m_lReturn = AgentChangeLogic()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentCode_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAgentMain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentMain.Click


        'put data back to agent listview
        Dim lSelectedItem As Integer = lvwAgents.FocusedItem.Index + 1

        lvwAgents.Refresh()

        'Position of main tab should already be ok
        tabAgent.Visible = False
        tabMainTab.Visible = True


    End Sub

    Private Sub cmdDeleteAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAgent.Click
        If lvwAgents.Items.Count = 0 Then
            Exit Sub
        End If
        If lvwAgents.FocusedItem Is Nothing Then
            lvwAgents.Items(0).Focused = True
        End If
        'do we have any record on the listview
        If lvwAgents.Items.Count > 0 Then
            'are we selecting any record
            If lvwAgents.FocusedItem.Index + 1 <> -1 Then
                lvwAgents.Items.RemoveAt(lvwAgents.FocusedItem.Index)
            End If
        End If
        'TMP
        m_lReturn = SetTrueMonthlyPolicy()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAgent_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If
        RaiseEvent SubAgentChange(Me, Nothing)
    End Sub

    '*******************************************************************************
    ' Name : cmdDeleteClient_Click
    '
    ' Description : Delete the selected client from the listview. Make sure that we
    '               only delete a client that is not lead or correspondence
    '
    ' History :
    '   21/07/2002 PWF (Created)
    '*******************************************************************************
    Private Sub cmdDeleteClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteClient.Click

        Dim bLead, bCorrespondence As Boolean
        Dim lResult As DialogResult

        Try

            ' Do we have any record on the listview
            If lvwClients.Items.Count > 0 Then
                ' Check for lead or correspondence
                'TODO
                bLead = ListViewHelper.GetListViewSubItem(lvwClients.FocusedItem, 3).Text = "Y"
                bCorrespondence = ListViewHelper.GetListViewSubItem(lvwClients.FocusedItem, 4).Text = "Y"
                If bCorrespondence Then
                    ' Display error stating the problem.
                    MessageBox(ACCPDlgDeleteCrspnd, ACCPDlgTitle, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                    Exit Sub
                ElseIf bLead Then
                    'Trying to delete lead client
                    'Ask for a new one before deleting
                    If lvwClients.Items.Count < 2 Then
                        MessageBox(ACCPDlgDeleteLead, ACCPDlgTitle, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
                        Exit Sub
                    Else
                        m_lReturn = UnloadInsuredClients()
                        m_lReturn = ProcessDetails()
                    End If
                Else
                    ' Check for selected record
                    If lvwClients.FocusedItem.Index + 1 <> -1 Then
                        ' Confirm delete
                        'TODO
                        lResult = MessageBox(ACCPDlgDeleteConfirm, ACCPDlgTitle, MsgBoxStyle.Question + MsgBoxStyle.OkCancel, ListViewHelper.GetListViewSubItem(lvwClients.FocusedItem, 1).Text)

                        If lResult = System.Windows.Forms.DialogResult.OK Then
                            lvwClients.Items.RemoveAt(lvwClients.FocusedItem.Index)
                        End If
                    End If
                End If
            End If

            ' Fire a fake click event to update button states
            'TODO
            lvwClients_ItemClick(lvwClients.CheckedItems(0))

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteClient_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub



    Private Sub cmdDeletePolicyWording_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeletePolicyWording.Click

        Dim iLine As Integer

        Try

            'Set row to be deleted - if a valid one selected
            If lvwPolicyWording.Items.Count < 1 Or lvwPolicyWording.FocusedItem Is Nothing Then
                Exit Sub
            End If

            iLine = lvwPolicyWording.FocusedItem.Index + 1

            If iLine = -1 Then
                Exit Sub
            End If

            'We may want to display something here, rather than just deleting it
            lvwPolicyWording.Items.RemoveAt(iLine - 1)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeletePolicyWording_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeletePolicyWording_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDownNarrative_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDownNarrative.Click

        Dim iLine As Integer
        Dim sCode, sDescription As String
        Dim lNarrative As Integer
        Dim bChecked As Boolean
        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwPolicyWording.Items.Count < 1 Then
                Exit Sub
            End If

            iLine = lvwPolicyWording.FocusedItem.Index + 1

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwPolicyWording.Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at the last one, there's no need to do anything
            If iLine = lvwPolicyWording.Items.Count Then
                Exit Sub
            End If

            'So let's swap them
            'iLine + 1 goes to temporary storage
            sCode = lvwPolicyWording.Items.Item(iLine).Text
            sDescription = ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine), 1).Text
            bChecked = lvwPolicyWording.Items.Item(iLine).Checked
            lNarrative = Convert.ToString(lvwPolicyWording.Items.Item(iLine).Tag)

            'iLine goes to iLine + 1
            lvwPolicyWording.Items.Item(iLine).Text = lvwPolicyWording.Items.Item(iLine - 1).Text
            ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine), 1).Text = ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 1), 1).Text
            lvwPolicyWording.Items.Item(iLine).Checked = lvwPolicyWording.Items.Item(iLine - 1).Checked
            lvwPolicyWording.Items.Item(iLine).Tag = Convert.ToString(lvwPolicyWording.Items.Item(iLine - 1).Tag)

            'temporary storage goes to iLine
            lvwPolicyWording.Items.Item(iLine - 1).Text = sCode
            ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 1), 1).Text = sDescription
            lvwPolicyWording.Items.Item(iLine - 1).Tag = CStr(lNarrative)
            lvwPolicyWording.Items.Item(iLine - 1).Checked = bChecked
            'Reset the selected item, automatically deselects the previous one
            lvwPolicyWording.Items.Item(iLine).Selected = True

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDownNarrative_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDownNarrative_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    Private Sub cmdHandler_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHandler.Click

        Dim vCnt, vName, vShortName As Object
        Dim vResolvedName As String = "" 'CT 19/07/00

        Try





            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="AH", vResolvedName:=vResolvedName) 'CT 19/07/00 added extra resolvedName parameter


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            pnlHandler.Tag = CStr(vCnt)



            m_vAccountHandlerCnt = vCnt

            'pnlHandler.Caption = CStr(vName)

            If vResolvedName = "" Then




                lblHandler.Text = CStr(vName)
            Else



                lblHandler.Text = vResolvedName
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHandler_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'ECK 14/07/99
    Private Sub cmdMain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMain.Click

        tabCommissionTab.Visible = False
        tabMainTab.Top = 0
        tabMainTab.Visible = True

    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click, _cmdPrevious_2.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try


            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                'TN20000823        tabMainTab.Tab = Index
                SSTabHelper.SetSelectedIndex(tabMainTab, Index - 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub



    Private Sub cmdRelatedPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRelatedPolicy.Click

        Dim vCnt, vRef As Object

        Try



            m_lReturn = SelectPolicy(vInsuranceFileCnt:=vCnt, vInsuranceFileRef:=vRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            pnlRelatedPolicy.Tag = CStr(vCnt)



            m_vRelatedPolicyCnt = vCnt





            lblRelatedPolicy.Text = CStr(vRef)
            'If we're here we must have a related party so we can set up the relationship
            cboRelationship.Enabled = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRelatedPolicy_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    '*******************************************************************************
    ' Name : cmdSetCorrespondence_Click
    '
    ' Description : Set a new correspondence client. But only in the listview control
    '
    ' History :
    '   21/07/2002 PWF (Created)
    '*******************************************************************************
    Private Sub cmdSetCorrespondence_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSetCorrespondence.Click


        Try

            ' At this point just change the list to reflect the change
            ' We'll pick up and do the full thing later
            For Each oListItem As ListViewItem In lvwClients.Items
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = IIf(oListItem.Index + 1 = lvwClients.FocusedItem.Index + 1, "Y", "N")
            Next oListItem

            ' Fire a fake click event to update button states
            'TODO
            lvwClients_ItemClick(lvwClients.CheckedItems(0))

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSetCorrespondence_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdUpNarrative_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUpNarrative.Click

        Dim iLine As Integer
        Dim sCode, sDescription As String
        Dim lNarrative As Integer
        Dim bChecked As Boolean
        Try

            'Set row to be moved - if a valid one selected
            'Is this list view empty
            If lvwPolicyWording.Items.Count < 1 Then
                Exit Sub
            End If

            iLine = lvwPolicyWording.FocusedItem.Index + 1

            'Have we selected any
            If iLine = -1 Then
                Exit Sub
            End If

            'Is it really, really selected?
            If Not lvwPolicyWording.Items.Item(iLine - 1).Selected Then
                Exit Sub
            End If

            'If we're at number 1, there's no need to do anything
            If iLine = 1 Then
                Exit Sub
            End If

            'So let's swap them
            'iLine - 1 goes to temporary storage
            sCode = lvwPolicyWording.Items.Item(iLine - 2).Text
            sDescription = ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 2), 1).Text
            bChecked = lvwPolicyWording.Items.Item(iLine - 2).Checked
            lNarrative = Convert.ToString(lvwPolicyWording.Items.Item(iLine - 2).Tag)

            'iLine goes to iLine - 1
            lvwPolicyWording.Items.Item(iLine - 2).Text = lvwPolicyWording.Items.Item(iLine - 1).Text
            ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 2), 1).Text = ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 1), 1).Text
            lvwPolicyWording.Items.Item(iLine - 2).Checked = lvwPolicyWording.Items.Item(iLine - 1).Checked
            lvwPolicyWording.Items.Item(iLine - 2).Tag = Convert.ToString(lvwPolicyWording.Items.Item(iLine - 1).Tag)

            'temporary storage goes to iLine
            lvwPolicyWording.Items.Item(iLine - 1).Text = sCode
            ListViewHelper.GetListViewSubItem(lvwPolicyWording.Items.Item(iLine - 1), 1).Text = sDescription
            lvwPolicyWording.Items.Item(iLine - 1).Tag = CStr(lNarrative)
            lvwPolicyWording.Items.Item(iLine - 1).Checked = bChecked
            'Reset the selected item, automatically deselects the previous one
            lvwPolicyWording.Items.Item(iLine - 2).Selected = True

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdUpNarrative_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUpNarrative_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwAgents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAgents.Click

        If Not (lvwAgents.FocusedItem Is Nothing) Then
            If lvwAgents.FocusedItem.Text.Trim() = "SSPSUBAGENT" Then
                cmdDeleteAgent.Enabled = False
            End If
        End If

    End Sub

    Private Sub lvwAgents_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAgents.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'not if we are viewing
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            cmdEditAgent.Enabled = True
            cmdDeleteAgent.Enabled = True

            If lvwAgents.GetItemAt(x, y) Is Nothing Then
                cmdEditAgent.Enabled = False
                cmdDeleteAgent.Enabled = False
            End If
        End If

    End Sub

    Private Sub lvwClients_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwClients.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwClients.Columns(eventArgs.Column)

        Try

            ' Check current sort order
            If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwClients) Then
                ' Current column, reverse order
                ListViewHelper.SetSortOrderProperty(lvwClients, (ListViewHelper.GetSortOrderProperty(lvwClients) + 1) Mod 2)
            Else
                ' Different column, change ordering
                ListViewHelper.SetSortedProperty(lvwClients, False)
                ListViewHelper.SetSortOrderProperty(lvwClients, SortOrder.Ascending)
                ListViewHelper.SetSortKeyProperty(lvwClients, ColumnHeader.Index + 1 - 1)
                ListViewHelper.SetSortedProperty(lvwClients, True)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwFees_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub lvwClients_ItemClick(ByVal Item As ListViewItem)

        Dim bLead, bCorrespondence As Boolean

        Try

            ' Only set if we are not viewing
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                bLead = ListViewHelper.GetListViewSubItem(Item, 3).Text = "Y"
                bCorrespondence = ListViewHelper.GetListViewSubItem(Item, 4).Text = "Y"

                'CMG/PB 27082002 Can delete leads now
                cmdDeleteClient.Enabled = Not bCorrespondence
                cmdSetCorrespondence.Enabled = Not bCorrespondence
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the item selection", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwFees_ItemClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Disable buttons to avoid unexpected processing due to the error
            cmdSetCorrespondence.Enabled = False

        End Try

    End Sub
    Private Sub lvwClients_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwClients.ItemChecked

        Try

            lvwClients_ItemClick(lvwClients.CheckedItems(0))
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the item selection", ACApp, ACClass, "lvwFees_ItemClick", Information.Err().Number, excep.Message, excep:=excep)

            ' Disable buttons to avoid unexpected processing due to the error
            cmdSetCorrespondence.Enabled = False

        End Try
    End Sub
    Private Sub lvwPolicyWording_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPolicyWording.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwPolicyWording.Columns(eventArgs.Column)

        ' Column click event for the StandardWordings

        'Don't
        Try

            With lvwPolicyWording
                ' If current sort column header is
                ' pressed.

                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicyWording) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPolicyWording, (ListViewHelper.GetSortOrderProperty(lvwPolicyWording) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPolicyWording, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPolicyWording, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwPolicyWording, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwPolicyWording, True)
                End If
            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPolicyWording_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwPolicyWording_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPolicyWording.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000


        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwPolicyWording.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdDeletePolicyWording.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdDeletePolicyWording.Enabled = True
            End If
        End If

    End Sub


    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    If Not (cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)).FindForm() Is Nothing) Then
                        VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                    End If
                Else
                    '            cmdOK.Default = True
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub txtAgentPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentPercentage.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentPercentage)
    End Sub

    Private Sub txtAgentPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentPercentage)
    End Sub

    Private Sub txtAgentAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentAmount.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentAmount)
    End Sub

    Private Sub txtAgentAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentAmount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentAmount)
    End Sub


    Private Sub txtAlternateReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternateReference.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAlternateReference)
    End Sub

    Private Sub txtAlternateReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternateReference.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAlternateReference)
    End Sub


    Private Sub txtCommissionCharge_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCommissionCharge.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        'CT 1/8/00 Bugfix 293 - disallow character entry
        Select Case KeyAscii
            Case 32 To 45, 47, Is > 57
                KeyAscii = 0
            Case Else
                'valid - do nothing
        End Select
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    Private Sub txtCommissionPercentage_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCommissionPercentage.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        'CT 1/8/00 Bugfix 293 - disallow character entry
        Select Case KeyAscii
            Case 32 To 45, 47, Is > 57
                KeyAscii = 0
            Case Else
                'valid - do nothing
        End Select
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    Private Sub txtCoverFromDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCoverFromDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCoverFromDate)

    End Sub

    Private Sub txtCoverFromDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCoverFromDate.Leave
        Dim dtDate As Date
        m_bRenCalDate = False
        If Strings.Len(txtCoverFromDate.Text) > 0 Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCoverFromDate)
            m_bRenCalDate = True
        End If
        ' PM Sumit Kumar interchanged below 2 statements
        m_lReturn = AgentChangeLogic()
        m_lReturn = SetUnderwritingYear()

        If m_lInsuranceFileTypeID <> 6 Then
            If m_lInsuranceFileTypeID <> 7 Then
                If m_iMidnightRenewal = 1 Then

                    dtDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCoverToDate))
                    dtDate = dtDate.AddDays(1)
                    m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
                Else
                    txtRenewalDate.Text = txtCoverToDate.Text
                End If
            End If
        End If
    End Sub

    Private Sub txtCoverToDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCoverToDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCoverToDate)

    End Sub

    Private Sub txtCoverToDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCoverToDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCoverToDate)
        FillRenewalDate()

        If m_bPolicyIsInRenewal Then
            Dim msg As String = "Renewal quote version exists on this policy, this will delete your existing renewal quote version from the system, do you still want to continue ?"

            If (m_bDoNotDeleteRenewalQuoteOnMTA AndAlso Convert.ToDateTime(txtRenewalDate.Text) <> Convert.ToDateTime(m_sRenewalVersionStartDate)) Then

                Dim objfrmDialogMsgRenewalConflict As New frmDialogMsg

                objfrmDialogMsgRenewalConflict.Text = "Policy Is In Renewal Conflict"
                objfrmDialogMsgRenewalConflict.Label1.Text = msg

                objfrmDialogMsgRenewalConflict.ShowDialog()

                If objfrmDialogMsgRenewalConflict.ClickAction = 1 Then
                    m_bDeletePolicyFromRenewal = True
                Else
                    m_bDeletePolicyFromRenewal = False
                    txtCoverToDate.Text = DateTime.Parse(m_sRequiredCoverEndDate).ToString("D")
                    FillRenewalDate()
                End If

            End If
        End If

        ' process true monthly policy dates
        ProcessTMPDates(kProcessRenewalDatesFromCoverTo)

    End Sub

    Private Sub FillRenewalDate()
        Dim dtDate As Date
        If m_lInsuranceFileTypeID <> 6 Then
            If m_lInsuranceFileTypeID <> 7 Then
                If m_iMidnightRenewal = 1 Then
                    dtDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCoverToDate))
                    dtDate = dtDate.AddDays(1)
                    m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
                Else
                    txtRenewalDate.Text = txtCoverToDate.Text
                End If
            End If
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: txtDiscountedPremium_LostFocus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Sub txtDiscountedPremium_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDiscountedPremium.Leave

        Const kMethodName As String = "txtDiscountedPremium_LostFocus"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            lReturn = ActionDiscountPremiumChange()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ActionDiscountPremiumChange Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub




    Private Sub txtInceptionTPI_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInceptionTPI.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInceptionTPI)
    End Sub
    Private Sub txtInceptionTPI_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInceptionTPI.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInceptionTPI)
    End Sub
    'TODO
    Private Sub txtInceptionDate_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInceptionDate.Enter
        m_lReturn = m_oFormFields.GotFocus(txtInceptionDate)
    End Sub
    Private Sub txtInceptionDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInceptionDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInceptionDate)

    End Sub

    Private Sub txtInsuredName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsuredName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsuredName)
    End Sub

    Private Sub txtInsuredName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsuredName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsuredName)
    End Sub

    Private Sub txtIssuedDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIssuedDate.Enter
        'RKS PN13281
        If Strings.Len(txtIssuedDate.Text) > 0 Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIssuedDate)
        End If
    End Sub


    Private Sub txtIssuedDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIssuedDate.Leave
        'RKS PN13281
        If Strings.Len(txtIssuedDate.Text) > 0 Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIssuedDate)
        End If
    End Sub



    Private Sub txtLapsedDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLapsedDate.Enter
        'RKS PN13281
        If Strings.Len(txtLapsedDate.Text) > 0 Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtLapsedDate)
        End If
    End Sub

    Private Sub txtLapsedDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLapsedDate.Leave
        'RKS PN13281
        If Strings.Len(txtLapsedDate.Text) > 0 Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtLapsedDate)
            'txtCoverFromDate = txtLapsedDate --PN 64015
        End If
        m_lReturn = SetUnderwritingYear()
    End Sub

    Private Sub txtManualDiscountPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtManualDiscountPercentage.Enter
        txtManualDiscountPercentage.MaxLength = 12
    End Sub
    Private Sub txtManualDiscountPercentage_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtManualDiscountPercentage.KeyPress

        If Not Char.IsDigit(eventArgs.KeyChar) Then eventArgs.Handled = True

        If eventArgs.KeyChar = Chr(8) Then eventArgs.Handled = False 'allow Backspace

        If eventArgs.KeyChar = "-" And txtManualDiscountPercentage.SelectionStart = 0 Then eventArgs.Handled = False 'allow negative number

        If eventArgs.KeyChar = "." And txtManualDiscountPercentage.Text.IndexOf(".") = -1 Then eventArgs.Handled = False 'allow single decimal point

        If eventArgs.KeyChar = Chr(13) Then txtManualDiscountPercentage.Focus() 'Enter key moves to specified control


        'Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 8 Then
        '    KeyAscii = 0
        'End If
        'If KeyAscii = 0 Then
        '    eventArgs.Handled = True
        'End If
        'eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtPolicyLTUExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyLTUExpiryDate.Enter
        'RKS PN13281
        If Strings.Len(txtPolicyLTUExpiryDate.Text) > 0 Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPolicyLTUExpiryDate)
        End If
    End Sub

    Private Sub txtPolicyLTUExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyLTUExpiryDate.Leave
        'RKS PN13281
        If Strings.Len(txtPolicyLTUExpiryDate.Text) > 0 Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPolicyLTUExpiryDate)
        End If
    End Sub

    Private Sub txtPolicyNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyNumber.Enter
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPolicyNumber)
        End If
        'If we're here by tabbing (or back-tabbing) we're on tab 0
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

    End Sub

    Private Sub txtPolicyNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPolicyNumber.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPolicyNumber)
        End If
    End Sub





    Private Sub txtProposalDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProposalDate.Enter
        'RKS PN13281
        If Strings.Len(txtProposalDate.Text) > 0 Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtProposalDate)
        End If
    End Sub

    Private Sub txtProposalDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtProposalDate.Leave
        'RKS PN13281
        If Strings.Len(txtProposalDate.Text) > 0 Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtProposalDate)
        End If
    End Sub


    Private Sub txtQuoteExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtQuoteExpiryDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtQuoteExpiryDate)
    End Sub

    Private Sub txtQuoteExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtQuoteExpiryDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtQuoteExpiryDate)
    End Sub

    Private Sub txtRegarding_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegarding.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRegarding)

    End Sub

    Private Sub txtRegarding_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegarding.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRegarding)

    End Sub

    Private Sub txtRenewalDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRenewalDate)

    End Sub

    Private Sub txtRenewalDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRenewalDate)

    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'Developer Guide No solution 2
        m_Font = MyBase.Font
        m_BackStyle = m_def_BackStyle


        m_BorderStyle = Windows.Forms.BorderStyle.None
        m_InsuranceFileCnt = m_def_InsuranceFileCnt
    End Sub

    Private Sub uctPMUPolicyControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    'Load property values from storage


    'Developer Guide No solution 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'Developer Guide No solution 2
        m_Font = PropBag.ReadProperty("Font", Me.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = PropBag.ReadProperty("BorderStyle", m_def_BorderStyle)


        m_InsuranceFileCnt = CInt(PropBag.ReadProperty("InsuranceFileCnt", m_def_InsuranceFileCnt))

    End Sub

    Private Sub uctPMUPolicyControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Maintain minimum width
        If VB6.PixelsToTwipsX(Width) < 9090 Then Width = VB6.TwipsToPixelsX(9090)
        ' and height width
        If VB6.PixelsToTwipsY(Height) < 4950 Then Height = VB6.TwipsToPixelsY(4950)

    End Sub

    'Write property values to storage


    'Developer Guide No solution 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)


        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'Developer Guide No solution 2
        PropBag.WriteProperty("Font", m_Font, Me.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("InsuranceFileCnt", m_InsuranceFileCnt, m_def_InsuranceFileCnt)
    End Sub

    '********************************************************************************
    ' Name : GetSystemOption
    '
    ' Desc : get system option
    '
    ' Hist : 23/04/2001 RWH Created
    '
    '********************************************************************************
    Public Function GetSystemOption(ByVal v_iOptionNumber As Integer, ByRef r_sResult As String) As Integer
        Dim result As Integer = 0

        Dim oObject As bSIROptions.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oObject = temp_oObject


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If



            result = oObject.GetOption(v_iOptionNumber, r_sResult)


            oObject.Dispose()

            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '*******************************************************************************
    ' Name : LoadInsuredClients
    '
    ' Description : Load the list of insured clients into the listview
    '
    ' History :
    '   21/07/2002 PWF (Created)
    '*******************************************************************************
    Private Function LoadInsuredClients() As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwClients.Items.Clear()

            ' Ensure we have an array of clients
            If Information.IsArray(m_vClientList) Then
                ' Get array bounds
                lLower = m_vClientList.GetLowerBound(1)
                lUpper = m_vClientList.GetUpperBound(1)

                ' Walk the array
                For lCount As Integer = lLower To lUpper
                    ' Create the listitem (with shortname)
                    With lvwClients.Items.Add(CStr(m_vClientList(3, lCount)).Trim(), "AgentImage")
                        ' Set party_cnt for matching later and duplicate checks

                        .Tag = CStr(m_vClientList(0, lCount))

                        ' Set item properties
                        ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientList(3, lCount)).Trim(), "AgentImage"), 1).Text = CStr(m_vClientList(4, lCount)).Trim()
                        ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientList(3, lCount)).Trim(), "AgentImage"), 2).Text = CStr(m_vClientList(5, lCount)).Trim()
                        ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientList(3, lCount)).Trim(), "AgentImage"), 3).Text = IIf(CBool(CStr(m_vClientList(1, lCount)).Trim()), "Y", "N")
                        ListViewHelper.GetListViewSubItem(lvwClients.Items.Add(CStr(m_vClientList(3, lCount)).Trim(), "AgentImage"), 4).Text = IIf(CBool(CStr(m_vClientList(2, lCount)).Trim()), "Y", "N")
                    End With
                Next
            End If

            ' We need to keep the old lead/correspond client details (use a funky string)
            lLower = m_vClientList.GetLowerBound(1)
            lUpper = m_vClientList.GetUpperBound(1)
            For lCount As Integer = lLower To lUpper
                ' Check for lead
                If CDbl(m_vClientList(1, lCount)) = 1 Then
                    ' Store lead details
                    m_sLead = CStr(m_vClientList(0, lCount)) & "|" &
                              CStr(m_vClientList(3, lCount)) & "|" &
                              CStr(m_vClientList(4, lCount))
                End If

                ' Check for correspond
                If CDbl(m_vClientList(2, lCount)) = 1 Then
                    ' Store correspondence details
                    m_sCrspnd = CStr(m_vClientList(0, lCount)) & "|" &
                                CStr(m_vClientList(3, lCount)) & "|" &
                                CStr(m_vClientList(4, lCount))
                End If
            Next

            ' Now, there must be at least on insured client...but
            ' This has been resolved in the stored procedure. If an empty resultset
            ' is detected the procedure will automatically return details from the
            ' insurance holder.

            ' Select the first item.
            lvwClients.Items.Item(0).Selected = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInsuredClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInsuredClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    '*******************************************************************************
    ' Name : UnloadInsuredClients
    '
    ' Description : Load the list of insured clients from the listview to the array
    '
    ' History :
    '   21/07/2002 PWF (Created)
    '*******************************************************************************
    Private Function UnloadInsuredClients() As Integer

        Dim result As Integer = 0
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lCount = 0

            ' Check we have some insured clients
            If lvwClients.Items.Count > 0 Then
                ' Enumerate all items
                For Each oListItem As ListViewItem In lvwClients.Items
                    ' Redimension the array appropriately
                    If lCount Then
                        ReDim Preserve m_vClientList(7, lCount)
                    Else
                        ReDim m_vClientList(7, 0)
                    End If

                    ' Load information

                    m_vClientList(0, lCount) = Convert.ToString(oListItem.Tag)
                    m_vClientList(1, lCount) = IIf(ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Y", 1, 0)
                    m_vClientList(2, lCount) = IIf(ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "Y", 1, 0)
                    m_vClientList(3, lCount) = oListItem.Text
                    m_vClientList(4, lCount) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                    m_vClientList(5, lCount) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
                    m_vClientList(6, lCount) = m_sLead
                    m_vClientList(7, lCount) = m_sCrspnd
                    lCount += 1
                Next oListItem
            Else
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Insured Clients Found", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadInsuredClients")

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadInsuredClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadInsuredClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function



    Private Function MessageBox(ByVal MessageResource As Integer, ByVal TitleResource As Integer, ByVal style As MsgBoxStyle, ByVal ParamArray Parameters() As Object) As DialogResult

        Dim lLower, lUpper As Integer
        Dim sMessage, sTitle As String

        Try

            ' Get description from the resource file.

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=MessageResource, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=TitleResource, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Insert any parameters
            lLower = Parameters.GetLowerBound(0)
            lUpper = Parameters.GetUpperBound(0)
            For lCount As Integer = lLower To lUpper
                sMessage = Strings.Replace(sMessage, "%", Parameters(lCount), , 1)
            Next

            ' Display message.


            Return Interaction.MsgBox(sMessage, style, sTitle)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to display message box for id's: " & MessageResource & " and " & TitleResource, vApp:=ACApp, vClass:=ACClass, vMethod:="MessageBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Function

    'Moh 15-05-2003
    Private Function GetCurrenciesForBranch() As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object
        Dim iSourceId As Integer

        Const ACCurrencyID As Integer = 0 'Currency Id
        Const ACCurrencyDescription As Integer = 1 'Currency Description

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If a currency is selected store it's value
            If cboCurrency.SelectedIndex > -1 Then
                m_lCurrencyID = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)
            End If

            ' Clear the list
            cboCurrency.Items.Clear()
            iSourceId = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)

            'Retrieve the currencies for this branch only
            If m_sTransactionType = "NB" And Task = gPMConstants.PMEComponentAction.PMAdd Then
                'Show only active currency for New Business

                m_lReturn = m_oBusiness.RetrieveCurrenciesForBranch(iSourceId:=iSourceId, vReturnArray:=vResult, bRestrictDeletedCurrency:=True)
            Else

                m_lReturn = m_oBusiness.RetrieveCurrenciesForBranch(iSourceId:=iSourceId, vReturnArray:=vResult)
            End If

            If Information.IsArray(vResult) Then

                For lCount As Integer = vResult.GetLowerBound(1) To vResult.GetUpperBound(1)
                    'Need to retrieve the currencies for the branch selected
                    Dim cboCurrency_NewIndex As Integer = -1

                    cboCurrency_NewIndex = cboCurrency.Items.Add(CStr(vResult(ACCurrencyDescription, lCount)).Trim())

                    VB6.SetItemData(cboCurrency, cboCurrency_NewIndex, CInt(vResult(ACCurrencyID, lCount)))

                    If CInt(vResult(ACCurrencyID, lCount)) = m_lCurrencyID Then
                        cboCurrency.SelectedIndex = cboCurrency_NewIndex
                    End If
                Next lCount
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in GetCurrenciesForBranch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesForBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'DJM 29/03/2004 : Created so that I can set cboPolicyStyle to mandatory or not based on retrieved policy.
    Public Function SetFieldValidationAfterGetPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetProductDetails(v_lProductID:=m_lProductID, v_lOption:=35, r_vValue:=m_vPolicyStyleMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vPolicyStyleMandatory = "" Then m_vPolicyStyleMandatory = CStr(0)

            'Policy Style
            If CBool(m_vPolicyStyleMandatory) Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPolicyStyle, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPolicyStyle, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.GetMidnightRenewal(v_lProductID:=m_lProductID, r_iMidnightRenewal:=m_iMidnightRenewal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidationAfterGetPolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidationAfterGetPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name : ShowMultiCurrencyDialogue
    '
    ' Description : Displays the multi-currency dialogue if required:
    '               If policy and base currency are different AND ((User cannot
    '               change rates AND System Option 156 enabled) OR user
    '               can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '*******************************************************************************
    Private Function ShowMultiCurrencyDialogue() As Integer
        Dim result As Integer = 0
        Dim iBaseCurrencyID As Integer
        Dim lStatus As gPMConstants.PMEReturnCode
        Dim sResult As String = ""
        Dim oForm As frmMultiCurrency

        Dim oSource As bPMSource.Business
        Dim iSourceId As Integer
        Dim vOverrideDate, vOverrideRate As Object
        Dim bTranCurMatchesBaseCur As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If cboBranchCode.SelectedIndex < 0 Then
                Return result
            End If

            'Get Base Currency for Policy Branch
            Dim temp_oSource As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSource = temp_oSource

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")


                oSource.Dispose()
                oSource = Nothing
                Return result
            End If

            iSourceId = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)

            m_lReturn = oSource.GetDetails(vSourceID:=iSourceId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMSource.Business.GetDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")

                result = m_lReturn

                oSource.Dispose()
                oSource = Nothing
                Return result
            End If


            m_lReturn = oSource.GetNext(vBaseCurrency:=iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMSource.Business.GetNext failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")

                result = m_lReturn

                oSource.Dispose()
                oSource = Nothing
                Return result
            End If


            oSource.Dispose()
            oSource = Nothing

            'Get UserAuthorities business object
            m_lReturn = CreateUserAuthorities()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, CreateUserAuthorities")
            End If

            'Get authority details for current user.

            m_lReturn = m_oUserAuthorities.getdetails(vUserID:=g_iUserId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                           "m_oUserAuthorities.GetDetails")
            End If

            'Get override options for current user.
            'DD 27/09/2004 - use pre-policy settings

            m_lReturn = m_oUserAuthorities.GetNext(vOverridePrePolicyDate:=vOverrideDate, vOverridePrePolicyRate:=vOverrideRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                           "m_oUserAuthorities.GetNext")
            End If

            ' if the user does have authority


            m_bAllowOverride = CInt(vOverrideDate) = 1 Or CInt(vOverrideRate) = 1

            ' option 156 (show multi-currency form on Policy)
            m_lReturn = GetSystemOption(v_iOptionNumber:=156, r_sResult:=sResult)

            bTranCurMatchesBaseCur = m_iCurrencyID = iBaseCurrencyID

            oForm = New frmMultiCurrency()



            'Load(oForm)
            'set up data

            'Fixed PN-15728***  JT***11/10/2004
            oForm.SourceId = m_iSourceId
            oForm.TransactionCurrencyID = m_iCurrencyID
            oForm.PartyCnt = m_lPartyCnt

            m_lReturn = oForm.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the multi-currency dialogue", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue")
                    Return result
                End If
            End If

            iPMFunc.CenterForm(oForm)

            'If user has got the authority to override rates and show currency screen

            ' if the user has got the authority to override the rates
            ' or the system option is set to display the transaction screen
            ' and the transaction currency doesnt match the base currency
            If (m_bAllowOverride Or sResult.Trim() = "1") And Not bTranCurMatchesBaseCur Then
                oForm.ShowDialog()
            Else
                'if not then do not show the currency screen - perform process silently.

                ' move the values on the form into the properties that
                ' are read from the form.
                oForm.InterfaceToProperties()

                ' set the status on the form to PMOK so that
                ' the code in queryunload doesnt display a message box
                oForm.Status = gPMConstants.PMEReturnCode.PMOK
            End If

            lStatus = oForm.Status

            If lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_dBaseExchangeRate = oForm.BaseExchangeRate
                m_dSystemExchangeRate = oForm.SystemExchangeRate
                If (Not Information.IsNothing(oForm.EffectiveDateOfExchange)) AndAlso oForm.EffectiveDateOfExchange <> Date.MinValue Then
                    m_dtEffectiveDateOfExchange = oForm.EffectiveDateOfExchange
                Else
                    m_dtEffectiveDateOfExchange = Now
                End If
                m_iRateOverrideReasonID = oForm.RateOverrideReasonID
                m_iBaseCurrencyID = oForm.BaseCurrencyID

                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            oForm.Dispose()

            oForm.Close()

            oForm = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowMultiCurrencyDialogue failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMultiCurrencyDialogue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetBranches() As Integer

        Dim result As Integer = 0
        Dim vBranches(,) As Object

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the branch values.

            m_lReturn = m_oBusiness.GetBranches(v_lUserID:=g_oObjectManager.UserID, v_lIncludeThisBranchID:=m_iSourceId, r_vResultArray:=vBranches, v_lProductId:=m_lProductID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get branches from business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Loop through branches
            If Information.IsArray(vBranches) Then

                For lCount As Integer = vBranches.GetLowerBound(1) To vBranches.GetUpperBound(1)
                    ' Add the details to the control
                    Dim cboBranchCode_NewIndex As Integer = -1

                    cboBranchCode_NewIndex = cboBranchCode.Items.Add(CStr(vBranches(ACDetailDesc, lCount)))

                    VB6.SetItemData(cboBranchCode, cboBranchCode_NewIndex, CInt(vBranches(ACDetailKey, lCount)))
                Next lCount
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get branches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function SetUnderwritingYear() As Integer

        'RKS PN13438
        Dim result As Integer = 0
        Dim lUnderwritingYearID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get the underwriting year for the cover from date
            If m_bUnderwritingYearID Then
                If m_sTransactionType = "MTA" Or m_sTransactionType = "MTC" Or m_sTransactionType = "REN" Or m_bIsRenewal Then
                    'RDT 15/12/2004 - PN17393 - cancellations should always use the Inception underwriting year.
                    'RKS 01/12/2004 PN17215

                    m_lReturn = m_oBusiness.GetUnderwritingYear(txtInceptionTPI.Text, lUnderwritingYearID)
                ElseIf gPMFunctions.ToSafeLong(m_vUnderwritingYearID, 0) = 0 Or m_lInsuranceFileTypeID = 1 Then
                    'TFS PN 6350
                    m_lReturn = m_oBusiness.GetUnderwritingYear(txtCoverFromDate.Text, lUnderwritingYearID)
                Else

                    lUnderwritingYearID = CInt(m_vUnderwritingYearID)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Underwriting Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUnderwritingYear")
                Else
                    cboUnderwritingYearID.ItemId = lUnderwritingYearID
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set UnderwritingYear", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUnderwritingYear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            'RKS PN13438
        End Try
    End Function

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUserAuthorities failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserAuthorities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name : IsClosedBranch
    '
    ' Description : Checkes if the branch seleted is closed one than flash a msg and cancel the action
    '
    ' History :
    ' JT created for PN-18031
    '*******************************************************************************
    Private Function IsClosedBranch(ByRef iBranchCode As Integer) As Integer
        Dim result As Integer = 0
        Try

            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3
            Dim lRow As Integer
            Dim isDeleted As Integer

            result = gPMConstants.PMEReturnCode.PMFalse
            lRow = 9 '**For Source Table

            For i As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt(CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow)) - 1)
                If CDbl(m_vLookupDetails(0, i)) = iBranchCode Then
                    isDeleted = CInt(m_vLookupDetails(3, i))
                    If isDeleted = 1 Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                    Return result
                End If
            Next

            '****
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsClosedBranch failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsClosedBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    '************************************************************************
    ' Name : GetBaseCurrency
    '
    ' Description : Gets the Base currency
    '
    ' JT created for PN-18794
    '***********************************************************************
    Private Function GetBaseCurrency() As Integer
        Dim result As Integer = 0


        Dim oSource As bPMSource.Business
        result = gPMConstants.PMEReturnCode.PMTrue
        'Get Base Currency for Policy Branch
        Dim temp_oSource As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oSource = temp_oSource

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrency")
            result = gPMConstants.PMEReturnCode.PMFalse

            oSource.Dispose()
            oSource = Nothing
            Return result
        End If
        Dim iSourceId As Integer = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)

        m_lReturn = oSource.GetDetails(vSourceID:=iSourceId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrency")

            result = m_lReturn

            oSource.Dispose()
            oSource = Nothing
            Return result
        End If


        m_lReturn = oSource.GetNext(vBaseCurrency:=m_iBaseCurrencyID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrency")

            result = m_lReturn

            oSource.Dispose()
            oSource = Nothing
            Return result
        End If


        oSource.Dispose()
        oSource = Nothing

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseCurrency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2004 : CQ4740
    ' ***************************************************************** '
    Private Function ReplaceNullWithDefault(ByRef v_vValue As String, ByVal v_vDefault As String) As String

        Dim result As String = String.Empty
        Const sFunctionName As String = "ReplaceNullWithDefault"

        Try

            result = CStr(gPMConstants.PMEReturnCode.PMTrue)


            If v_vValue = "" Or v_vValue Is DBNull.Value.ToString() Or Convert.IsDBNull(v_vValue) Or IsNothing(v_vValue) Or StringsHelper.ToDoubleSafe(v_vValue) = 0 Then

                v_vValue = v_vDefault

            End If


            Return v_vValue

        Catch excep As System.Exception



            result = CStr(gPMConstants.PMEReturnCode.PMError)

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HasInstalment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Jitendra
    ' ***************************************************************** '
    Public Function HasInstalment(ByRef v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vGetArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the instalment plan.


            m_lReturn = m_oBusiness.HasInstalment(v_lInsuranceFileCnt, vGetArray)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            If Information.IsArray(vGetArray) Then

                m_lFinancePlancnt = CInt(vGetArray(0, 0))

                m_lFinancePlanVersion = CInt(vGetArray(1, 0))
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to HasInstalment", vApp:=ACApp, vClass:=ACClass, vMethod:="HasInstalment", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get HasInstalment", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetFeeDetail() As Integer

        Dim result As Integer = 0


        Dim oObject As Object
        Dim vKeyArray(1, 4) As Object
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUListRisks.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            'ReDim vKeyArray(1, 4)

            vKeyArray(0, 0) = "party_cnt"

            vKeyArray(0, 1) = "shortname"

            vKeyArray(0, 2) = "insurance_file_cnt"

            vKeyArray(0, 3) = "insurance_folder_cnt"

            vKeyArray(0, 4) = "specifiedtab"


            vKeyArray(1, 0) = m_lPartyCnt

            vKeyArray(1, 1) = ""

            vKeyArray(1, 2) = m_lInsuranceFileCnt

            vKeyArray(1, 3) = m_lInsuranceFolderCnt
            'just for policy Fee

            vKeyArray(1, 4) = 1

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)


            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status

            oObject.Dispose()
            'm_lReturn = oObject.Terminate()
            oObject = Nothing

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFeeDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetPolicyTaxDetail() As Integer
        Dim result As Integer = 0


        Dim oObject As Object
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUListRisks.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            ReDim vKeyArray(1, 4)

            vKeyArray(0, 0) = "party_cnt"

            vKeyArray(0, 1) = "shortname"

            vKeyArray(0, 2) = "insurance_file_cnt"

            vKeyArray(0, 3) = "insurance_folder_cnt"

            vKeyArray(0, 4) = "specifiedtab"


            vKeyArray(1, 0) = m_lPartyCnt

            vKeyArray(1, 1) = ""

            vKeyArray(1, 2) = m_lInsuranceFileCnt

            vKeyArray(1, 3) = m_lInsuranceFolderCnt
            'just for policy tax

            vKeyArray(1, 4) = 2


            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)


            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyTaxDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTaxDetial", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetCommissionDetail() As Integer
        Dim result As Integer = 0


        Dim oObject As Object
        Dim vKeyArray(,) As Object
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUListRisks.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            ReDim vKeyArray(1, 4)

            vKeyArray(0, 0) = "party_cnt"

            vKeyArray(0, 1) = "shortname"

            vKeyArray(0, 2) = "insurance_file_cnt"

            vKeyArray(0, 3) = "insurance_folder_cnt"

            vKeyArray(0, 4) = "specifiedtab"


            vKeyArray(1, 0) = m_lPartyCnt

            vKeyArray(1, 1) = ""

            vKeyArray(1, 2) = m_lInsuranceFileCnt

            vKeyArray(1, 3) = m_lInsuranceFolderCnt
            'just for policy Comm

            vKeyArray(1, 4) = 3

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)


            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status


            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed GetCommissionDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetInstalmentDetail() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            Dim oMaint As Object
            Dim vKeyArray(1, 5) As Object


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClientCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = ""

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClientName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = ""

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameFinancePlanCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lFinancePlancnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lFinancePlanVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMKeyNameFinancePlanEditAuthority

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = False

            Dim temp_oMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oMaint, sClassName:="iPMBFinancePlanMaint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oMaint = temp_oMaint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = CType(oMaint, SSP.S4I.Interfaces.ILocalInterface).Initialise()


            m_lReturn = oMaint.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vEffectiveDate:=DateTime.Now)


            m_lReturn = oMaint.SetKeys(vKeyArray)

            m_lReturn = oMaint.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oMaint.Status


            oMaint.Dispose()
            oMaint = Nothing

            Return result

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed GetCommissionDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' GetProductDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductDetails() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetProductDetails"
        Const kAllProductDetails As Integer = -1
        Const kProductDetailsIsMidnightRenewal As Integer = 11
        Const kProductDetailIsTrueMonthlyPolicy As Integer = 41
        Const kProductDetailAnniversaryRenewalWeeks As Integer = 42
        'TMP Added two constants.
        Const kProductDetailIsLeadAllowAgentCommission As Integer = 47
        Const kProductDetailIsSubAllowAgentCommission As Integer = 50
        Const kProductDetailRenewalDayNumber As Integer = 46
        Const kProductDetailsUnifiedRenewalReadOnly As Integer = 176
        Dim nReturn As gPMConstants.PMEReturnCode
        nResult = gPMConstants.PMEReturnCode.PMTrue
        Try

            nReturn = m_oBusiness.GetProductDetails(m_lProductID, kAllProductDetails, m_vProductDetails)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(m_vProductDetails) Then
                m_bIsTrueMonthlyPolicy =
                    gPMFunctions.ToSafeBoolean(CDbl(m_vProductDetails(kProductDetailIsTrueMonthlyPolicy, 0)) = 1, False)
                If m_bIsTrueMonthlyPolicy Then
                    m_bUnifiedRenewalDateIsReadOnly =
                        ToSafeBoolean(m_vProductDetails(kProductDetailsUnifiedRenewalReadOnly, 0) = 1, False)
                End If
                m_iMidnightRenewal = gPMFunctions.ToSafeLong(m_vProductDetails(kProductDetailsIsMidnightRenewal, 0))
                'TMP
                m_bISProductConfAllowLeadConsolidatedCommission =
                    gPMFunctions.ToSafeBoolean(CDbl(m_vProductDetails(kProductDetailIsLeadAllowAgentCommission, 0)) = 1,
                                               False)
                m_bISProductConfAllowSubConsolidatedCommission =
                    gPMFunctions.ToSafeBoolean(CDbl(m_vProductDetails(kProductDetailIsSubAllowAgentCommission, 0)) = 1,
                                               False)
                If m_sTransactionType = "NB" OrElse m_bIsTrueMonthlyPolicy Then
                    m_lRenewalDayNumber = gPMFunctions.ToSafeLong(m_vProductDetails(kProductDetailRenewalDayNumber, 0))
                End If
            End If
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' SetupTrueMonthlyPolicy
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupTrueMonthlyPolicy() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SetupTrueMonthlyPolicy"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim nItemId As Integer
        Dim sItemDesc As String = ""

        nResult = gPMConstants.PMEReturnCode.PMTrue
        Try
            If m_bIsTrueMonthlyPolicy Then

                ' default control settings
                txtRenewalDate.Width = txtRenewalDate.Width
                lblAnniversaryDate.Visible = True
                txtAnniversaryDate.Visible = True
                If m_bIsTrueMonthlypolicyandNextInstalmentRenewal Then
                    chkPutOnNextInstalmentRenewal.Visible = True
                    lblPutOnNextInstalmentRenewal.Visible = True
                End If
                cboRenewalDayNumber.Enabled = False
                If m_sTransactionType = "NB" Or m_sTransactionType = "MTR" Then
                    txtAnniversaryDate.Enabled = True
                Else
                    txtAnniversaryDate.Enabled = False
                End If
                If m_bUnifiedRenewalDateIsReadOnly Then
                    txtCoverToDate.Enabled = False
                End If


                Select Case m_sTransactionType
                    Case "NB"
                        m_lAnniversaryCopy = 1
                        If m_lRenewalDayNumber <> 0 Then
                            SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        Else
                            SelectcboItem(cboRenewalDayNumber, DateAndTime.Day(CDate(txtRenewalDate.Text)))
                        End If

                        cboRenewalDayNumber.Enabled = Not (m_bUnifiedRenewalDateIsReadOnly)

                    Case "MTA"
                        m_lAnniversaryCopy = 0
                        SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        chkPutOnNextInstalmentRenewal.Visible = True
                        lblPutOnNextInstalmentRenewal.Visible = True
                        cboRenewalDayNumber.Enabled = Not (m_bUnifiedRenewalDateIsReadOnly)

                    Case "REN"
                        SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        cboRenewalDayNumber.Enabled = Not (m_bUnifiedRenewalDateIsReadOnly)

                    Case Else
                        If m_lRenewalDayNumber <> 0 Then
                            SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        End If
                End Select

                nItemId = 0

                ' get the id of renewal frequency monthly
                nReturn = CType(GetLookupItem(v_sLookupTable:=gSIRLibrary.SIRLookupRenewalFrequency,
                                              r_sItemDesc:=sItemDesc,
                                              r_sItemCode:=kLookupCodeRenewalFrequencyMonthly, r_lItemId:=nItemId),
                            gPMConstants.PMEReturnCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetLookupItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if we found the monthly frequency
                If nItemId <> 0 Then

                    ' select the monthly frequency item
                    nReturn = CType(SelectcboItem(cboFrequency, nItemId), gPMConstants.PMEReturnCode)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' disable the frequency combo
                    cboFrequency.Enabled = False

                End If

            Else

                txtRenewalDate.Width =
                    VB6.TwipsToPixelsX(
                        VB6.PixelsToTwipsX(txtRenewalDate.Width) + VB6.PixelsToTwipsX(cboRenewalDayNumber.Width) + 10)
                cboRenewalDayNumber.Visible = False
                lblAnniversaryDate.Visible = False
                txtAnniversaryDate.Visible = False
                cboPolicyDeductible.Top = txtAnniversaryDate.Top
                lblPolicyDeductible.Top = lblAnniversaryDate.Top
                chkPutOnNextInstalmentRenewal.Visible = False
                lblPutOnNextInstalmentRenewal.Visible = False

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupItem
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created : MEvans : 06-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookupItem(ByVal v_sLookupTable As String, ByRef r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupItem"

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim sCode As String = ""
        Dim llBound, lUBound As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lookup value contants.
            Const ACValueTableName As Integer = 0
            Const ACValueID As Integer = 1
            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3

            Const ACDetailKey As Integer = 0
            Const ACDetailDesc As Integer = 1
            Const ACDetailCode As Integer = 2

            ' Initilisation
            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)

                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
                    bFoundMatch = True
                    Exit For
                End If

            Next lRow

            If bFoundMatch Then

                ' get array boundaries for specified table
                llBound = CInt(m_vLookupValues(ACValueStartPos, lRow))

                lUBound = CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                ' set lookup properties
                If r_lItemId <> 0 Then
                    v_vLookupItem = CStr(r_lItemId)
                    lLookupItem = 0

                ElseIf r_sItemDesc <> "" Then
                    v_vLookupItem = r_sItemDesc
                    lLookupItem = 1

                ElseIf r_sItemCode <> "" Then
                    v_vLookupItem = r_sItemCode
                    lLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = llBound To lUBound

                    ' get the code for the specified lookup items key
                    If CStr(m_vLookupDetails(lLookupItem, lCntr)).Trim() = v_vLookupItem Then

                        ' return the requested code, id, description
                        r_sItemDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
                        r_sItemCode = CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim()
                        r_lItemId = CInt(CStr(m_vLookupDetails(ACDetailKey, lCntr)).Trim())

                        Exit For
                    End If

                Next lCntr

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
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound Then

                ' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessTMPDates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-10-2005 : Process ID
    ' ***************************************************************** '
    Public Function ProcessTMPDates(ByVal v_lControl As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessTMPDates"

        Dim lReturn As Integer
        Dim dtSerial As Date
        Dim lMonth, lYear, lDay, lNewMonth As Integer

        Dim dtCoverFromDate, dtInceptionDate, dtInceptionDateTPI As Date
        Dim vRenewalFrequency As Object
        Dim sInterval As String = ""
        Dim lRenewalFrequencyMonths As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' only process this routine if this product is a true monthly policy
            If Not m_bIsTrueMonthlyPolicy Then
                Return result
            End If

            ' only process this routine if its not already running
            If m_bProcessingTMPDates Then
                Return result
            End If

            m_bProcessingTMPDates = True


            Select Case v_lControl
            ' if the procedure was called from cover from change
                Case kProcessRenewalDatesFromCoverFrom

                    If m_sTransactionType = "NB" Then
                        lDay = DateAndTime.Day(gPMFunctions.ToSafeDate(txtRenewalDate.Text.Trim))
                        lMonth = gPMFunctions.ToSafeDate(txtInceptionDate.Text.Trim).Month
                        lYear = gPMFunctions.ToSafeDate(txtInceptionDate.Text.Trim).Year
                        If m_lRenewalDayNumber > lDay Then
                            SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        Else
                            SelectcboItem(cboRenewalDayNumber, lDay)
                        End If
                        txtAnniversaryDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear + 1)).ToString("D")

                        If DateAndTime.DateSerial(lYear, lMonth, lDay) > CDate(txtCoverFromDate.Text) Then
                            txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")
                        Else
                            txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth + 1, lYear)).ToString("D")
                        End If

                        If m_iMidnightRenewal Then
                            ' cover to date should be the day before the renewal date
                            txtCoverToDate.Text = DateTime.Parse(CDate(txtRenewalDate.Text).AddDays(-1)).ToString("D")
                        Else
                            ' cover to date
                            txtCoverToDate.Text = DateTime.Parse(txtRenewalDate.Text).ToString("D")
                        End If

                    End If

                ' update the renewal date
                Case kProcessRenewalDatesFromCoverTo

                    If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Then

                        lDay = DateAndTime.Day(CDate(txtRenewalDate.Text))
                        lMonth = CDate(txtInceptionDate.Text).Month
                        lYear = CDate(txtInceptionDate.Text).Year + 1

                        If m_lRenewalDayNumber > lDay Then
                            SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                        Else
                            SelectcboItem(cboRenewalDayNumber, lDay)
                        End If

                        txtAnniversaryDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")

                    End If

                Case kProcessRenewalDatesFromRenewalDayNumber

                    If m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Then

                        lDay = VB6.GetItemData(cboRenewalDayNumber, cboRenewalDayNumber.SelectedIndex)
                        lMonth = CDate(txtCoverFromDate.Text).Month
                        lYear = CDate(txtCoverFromDate.Text).Year

                        ' renewal date
                        If m_bIsTrueMonthlyPolicy And m_lRenewalDayNumber > 0 And m_sTransactionType = "REN" Then
                            If VB6.GetItemData(cboRenewalDayNumber, cboRenewalDayNumber.SelectedIndex) >= DateAndTime.Day(DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 1, 0)) Then
                                txtRenewalDate.Text = DateTime.Parse(DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 2, 0)).ToString("D")
                            Else
                                If gPMFunctions.ToSafeDate(DateAndTime.DateSerial(lYear, lMonth, lDay)) > gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim) Then
                                    txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")
                                Else
                                    txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth + 1, lYear)).ToString("D")
                                End If
                            End If
                        ElseIf m_bIsTrueMonthlyPolicy And m_lRenewalDayNumber > 0 And m_sTransactionType = "NB" Then
                            If VB6.GetItemData(cboRenewalDayNumber, cboRenewalDayNumber.SelectedIndex) >= DateAndTime.Day(DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 1, 0)) Then
                                txtRenewalDate.Text = DateTime.Parse(DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 1, 0)).ToString("D")
                            Else
                                If gPMFunctions.ToSafeDate(DateAndTime.DateSerial(lYear, lMonth, lDay)) > gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim) Then
                                    txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")
                                Else
                                    txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth + 1, lYear)).ToString("D")
                                End If
                            End If
                        Else
                            If gPMFunctions.ToSafeDate(DateAndTime.DateSerial(lYear, lMonth, lDay)) > gPMFunctions.ToSafeDate(txtCoverFromDate.Text.Trim) Then
                                txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")
                            Else
                                txtRenewalDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth + 1, lYear)).ToString("D")
                            End If
                        End If
                        ' if this is a midnight renewal
                        If m_iMidnightRenewal Then
                            ' cover to date should be the day before the renewal date

                            txtCoverToDate.Text = CDate(txtRenewalDate.Text).AddDays(-1)
                        Else
                            ' cover to date
                            txtCoverToDate.Text = txtRenewalDate.Text
                        End If

                        lMonth = CDate(txtInceptionDate.Text).Month
                        lYear = CDate(txtInceptionDate.Text).Year + 1

                        ' anniversary date
                        If m_sTransactionType <> "REN" Then
                            txtAnniversaryDate.Text = DateTime.Parse(GetClosestDate(lDay, lMonth, lYear)).ToString("D")
                        End If

                    End If

                Case Else

                    ' do nothing yet

            End Select

            ' do not change cover to date if its a BDTMP
            If m_bIsBackdatedMTAVersion Then

                txtCoverToDate.Text = m_dtExpiryDate
            End If

            m_bProcessingTMPDates = False


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClosestDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-10-2005 : Process ID
    ' ***************************************************************** '
    Public Function GetClosestDate(ByVal v_lDay As Integer, ByVal v_lMonth As Integer, ByVal v_lYear As Integer) As Date

        Dim result As Date = DateTime.FromOADate(0)
        Const kMethodName As String = "GetClosestDate"

        Dim lReturn, lNewMonth As Integer
        Dim dtSerial As Date
        Dim lMonth, lYear As Integer
        Dim dtMonthStart As Date



        Try



            result = DateTime.FromOADate(gPMConstants.PMEReturnCode.PMTrue)

            ' serialise the date from the passed data
            dtSerial = DateAndTime.DateSerial(v_lYear, v_lMonth, v_lDay)

            ' get the month from the newly serialised date
            lNewMonth = dtSerial.Month

            v_lYear = dtSerial.Year
            v_lDay = DateAndTime.Day(dtSerial)
            v_lMonth = dtSerial.Month

            ' if the month of the new date doesnt match the
            ' month passed in then
            If lNewMonth <> v_lMonth Then

                dtMonthStart = DateAndTime.DateSerial(v_lYear, lNewMonth, 1)

                dtSerial = dtMonthStart.AddDays(-1)

            End If

            ' return the serialised date
            ' or the last day in the specified month
            result = dtSerial


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetupCboRenewalDayNumber
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetupCboRenewalDayNumber() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupCboRenewalDayNumber"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For lItem As Integer = 1 To 31
                Dim cboRenewalDayNumber_NewIndex As Integer = -1
                cboRenewalDayNumber_NewIndex = cboRenewalDayNumber.Items.Add(CStr(lItem))
                VB6.SetItemData(cboRenewalDayNumber, cboRenewalDayNumber_NewIndex, lItem)
            Next


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupPolicyDiscountInterface
    '
    ' Parameters: n/a
    '
    ' Description: sets up the discount frame either enabled or disabled
    '
    ' History:
    '           Created : MEvans : 23-11-2005 : PN25452 (Discount / Loading)
    ' ***************************************************************** '
    Private Function SetupPolicyDiscountInterface(ByVal v_bEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyDiscountInterface"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            fraDiscount.Enabled = v_bEnabled

            cboDiscountReason.Enabled = v_bEnabled
            cboDiscountRecurringType.Enabled = v_bEnabled
            txtDiscountedPremium.Enabled = v_bEnabled
            txtDiscountPercentage.Enabled = v_bEnabled

            lblDiscountReason.Enabled = v_bEnabled
            lblDiscountedPremium.Enabled = v_bEnabled
            lblDiscountPercentage.Enabled = v_bEnabled
            lblRecurring.Enabled = v_bEnabled



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountActive
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function GetPolicyDiscountActive(ByRef r_bPolicyDiscountEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountActive"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bPolicyDiscountActive As Boolean
        Dim sSysOptValue As String = ""
        Dim oSIRRiskData As Object
        Dim vResultArray, vSelectedRiskCount As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get system option - policy discount enabled
            lReturn = CType(GetSystemOption(v_iOptionNumber:=ACSysOptionPolicyDiscount, r_sResult:=sSysOptValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed to retrieve PolicyDiscount Enabled:" & ACSysOptionPolicyDiscount, gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if the system option is enabled
            If sSysOptValue = "1" Then
                ' policy discount is active
                bPolicyDiscountActive = True
            Else
                ' policy discount is disabled
                bPolicyDiscountActive = False
            End If

            ' if this policy already has a specified discount reason
            If gPMFunctions.ToSafeLong(m_iDiscountReasonId, 0) > 0 Then
                m_bDiscountCalculatedPreviously = True
                ' even if the policy discount system option is disabled
                ' activate the policy discount for this policy
                bPolicyDiscountActive = True
            End If

            ' if the policy discount is active
            If bPolicyDiscountActive Then

                ' only if there are any selected risks associated with this policy should we classify
                ' policy discount as being active

                lReturn = m_oBusiness.GetSelectedRiskCount(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=vSelectedRiskCount)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetSelectedRiskCount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if there are no associated risks
                If Information.IsArray(vSelectedRiskCount) Then

                    If CDbl(vSelectedRiskCount(0, 0)) <> 0 Then
                        ' enabled Policy discount for this policy
                        bPolicyDiscountActive = True
                    Else
                        ' disable Policy discount for this policy
                        bPolicyDiscountActive = False
                    End If
                Else
                    ' disable Policy discount for this policy
                    bPolicyDiscountActive = False
                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy object instance
            oSIRRiskData = Nothing

            ' return the policy discount active status
            r_bPolicyDiscountEnabled = bPolicyDiscountActive




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
    '           Created : MEvans : 25-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function SetupPolicyDiscount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the total policy premium for the policy
            lReturn = CType(GetPolicyDiscountTotalPremium(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' initially default all discount values
            cboDiscountRecurringType.ItemId = 0
            cboDiscountReason.ItemId = 0
            txtDiscountedPremium.Text = ""
            txtDiscountPercentage.Text = ""

            txtDiscountedPremium.Enabled = False
            txtDiscountPercentage.Enabled = False
            cboDiscountRecurringType.Enabled = False

            ' determine if the the stored discount details still apply
            ' based on transaction type and discount recurring type
            ' NB           - stored discount values always apply
            ' REN           - only discount specified as "This Policy" apply
            ' ALL MTA Types - only discounts specified as "This Policy", "This Term" apply

            Select Case m_sTransactionType
                Case "NB" ' New Business

                    ' always use the specified details
                    cboDiscountRecurringType.ItemId = m_lDiscountRecurringTypeID
                    cboDiscountReason.ItemId = m_iDiscountReasonId

                    ' if there is a reason selected
                    If cboDiscountReason.ListIndex > 0 Then

                        ' if there is a previously saved discounted premium use it
                        If m_crDiscountedPremium > 0 Then
                            txtDiscountedPremium.Text = CStr(m_crDiscountedPremium)
                        Else
                            ' otherwise use the total policy premium
                            txtDiscountedPremium.Text = CStr(m_crTotalPremiumDiscountOrig)
                        End If

                    End If

                    txtDiscountPercentage.Text = CStr(m_dDiscountPercentage)

                    If cboDiscountReason.ListIndex > 0 Then
                        txtDiscountedPremium.Enabled = True
                        txtDiscountPercentage.Enabled = True
                        cboDiscountRecurringType.Enabled = True
                    End If

                Case "REN"

                    ' in the case of renewals only set the discount values
                    ' if the recurring type is "Policy"
                    If m_lDiscountRecurringTypeID = kDiscountRecurringTypeIdPolicy Then

                        cboDiscountRecurringType.ItemId = m_lDiscountRecurringTypeID
                        cboDiscountReason.ItemId = m_iDiscountReasonId
                        txtDiscountPercentage.Text = CStr(m_dDiscountPercentage)

                        If cboDiscountReason.ListIndex > 0 Then
                            txtDiscountedPremium.Enabled = True
                            txtDiscountPercentage.Enabled = True
                            cboDiscountRecurringType.Enabled = True
                        End If

                    Else
                        If m_crTotalPremiumDiscountOrig = 0 Then
                            cboDiscountReason.Enabled = False
                        End If
                    End If

                Case ""

                    ' view from client manager

                    ' show all details
                    txtDiscountedPremium.Text = CStr(m_crDiscountedPremium)
                    cboDiscountRecurringType.ItemId = m_lDiscountRecurringTypeID
                    cboDiscountReason.ItemId = m_iDiscountReasonId
                    txtDiscountPercentage.Text = CStr(m_dDiscountPercentage)

                Case Else

                    ' in the case of all forms of MTA only set the discount values
                    ' when the discount recurring type is "Policy" or "This Term"

                    ' for mta we never want to match a discounted premium ever ever ever...
                    m_crDiscountedPremium = 0

                    If (m_lDiscountRecurringTypeID = kDiscountRecurringTypeIdPolicy) Or (m_lDiscountRecurringTypeID = kDiscountRecurringTypeIdTerm) Then

                        cboDiscountRecurringType.ItemId = m_lDiscountRecurringTypeID
                        m_lPrevDiscountReasonId = m_iDiscountReasonId
                        cboDiscountReason.ItemId = m_iDiscountReasonId
                        txtDiscountPercentage.Text = CStr(m_dDiscountPercentage)

                        If cboDiscountReason.ListIndex > 0 Then
                            txtDiscountedPremium.Enabled = True
                            txtDiscountPercentage.Enabled = True
                            cboDiscountRecurringType.Enabled = True
                        End If
                    Else
                        If m_crTotalPremiumDiscountOrig = 0 Then
                            cboDiscountReason.Enabled = False
                        End If
                    End If

            End Select



            If m_crDiscountedPremium <> 0 Then
                txtDiscountedPremium.Text = StringsHelper.Format(m_crDiscountedPremium, kGenericCurrencyFormat)
            Else
                txtDiscountedPremium.Text = ""
                txtDiscountedPremium.Enabled = False
            End If
            txtDiscountPercentage.Text = StringsHelper.Format(txtDiscountPercentage.Text, kDiscountPercentageFormat)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountTotalPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function GetPolicyDiscountTotalPremium() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountTotalPremium"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPremiumTotals As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the total premium hasnt already been retrieved
            If m_crTotalPremiumDiscountOrig = 0 Then

                ' get the total premium

                lReturn = m_oBusiness.GetPolicyDiscountTotalPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vResults:=vPremiumTotals)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' get the total amount of premium for this policy

                m_crTotalPremiumDiscountOrig = CDec(vPremiumTotals(0, 0))

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
    ' Name: ActionDiscountReasonChange
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ActionDiscountReasonChange() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionDiscountReasonChange"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if no discount reason has been selected
            ' default the values
            If cboDiscountReason.ListIndex < 1 Then

                txtDiscountedPremium.Text = CStr(0)
                txtDiscountPercentage.Text = CStr(0)
                cboDiscountRecurringType.ListIndex = 0

                txtDiscountedPremium.Enabled = False
                txtDiscountPercentage.Enabled = False
                cboDiscountRecurringType.Enabled = False

                ' recurring is no longer mandatory
                lblRecurring.Font = VB6.FontChangeBold(lblRecurring.Font, False)
                lblRecurring.Left = VB6.TwipsToPixelsX(120)

                m_crPrevDiscountedPremium = 0

            Else

                ' get the total policy premium
                lReturn = GetPolicyDiscountTotalPremium()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' if we have changed from discount reason "<none>" to a different
                If m_lPrevDiscountReasonId = kDiscountReasonNone Then

                    ' default the initial values
                    txtDiscountedPremium.Text = CStr(m_crTotalPremiumDiscountOrig)
                    txtDiscountPercentage.Text = CStr(0)

                    ' enable the fields for user input
                    txtDiscountedPremium.Enabled = True
                    txtDiscountPercentage.Enabled = True
                    cboDiscountRecurringType.Enabled = True

                    ' recurring is now mandatory
                    lblRecurring.Font = VB6.FontChangeBold(lblRecurring.Font, True)
                    lblRecurring.Left = VB6.TwipsToPixelsX(120)

                End If

            End If

            ' save the currently selected discount reason id
            m_lPrevDiscountReasonId = cboDiscountReason.ItemData(cboDiscountReason.ListIndex)

            ' apply format to policy discount premium and percentage fields
            txtDiscountedPremium.Text = StringsHelper.Format(txtDiscountedPremium.Text, "0.00")
            txtDiscountPercentage.Text = StringsHelper.Format(txtDiscountPercentage.Text, "0.00000000")



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionDiscountPremiumChange
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ActionDiscountPremiumChange() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionDiscountPremiumChange"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' if the user has entered incorrect data reset and  reformat
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtDiscountedPremium.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                txtDiscountedPremium.Text = StringsHelper.Format(m_crPrevDiscountedPremium, kGenericCurrencyFormat)
            End If

            ' if the actual premium has changed
            If m_crPrevDiscountedPremium <> CDec(txtDiscountedPremium.Text) Then

                ' recalculate the discount percentage
                lReturn = RecalculateDiscountPercentage()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "RecalculateDiscountPercentage Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' format the discount premium
            txtDiscountedPremium.Text = StringsHelper.Format(txtDiscountedPremium.Text, kGenericCurrencyFormat)

            ' store current discount premium in case it changes
            m_crPrevDiscountedPremium = gPMFunctions.ToSafeCurrency(txtDiscountedPremium.Text, 0)
            m_dPrevDiscountPercentage = gPMFunctions.ToSafeDouble(txtDiscountPercentage.Text, 0)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RecalculateDiscountPercentage
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function RecalculateDiscountPercentage() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateDiscountPercentage"

        Dim lReturn As Integer
        Dim crPremium As Decimal
        Dim dPercentage As Double
        Dim crOriginalPremium As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the new premium
            crPremium = gPMFunctions.ToSafeCurrency(txtDiscountedPremium.Text, 0)
            crOriginalPremium = gPMFunctions.ToSafeCurrency(m_crTotalPremiumDiscountOrig, 0)

            ' calculate the percentage (discount / loading)
            ' in comparison with the original premium
            dPercentage = ((crPremium / (crOriginalPremium)) * 100) - 100

            ' check for invalid discount / loading percentages
            If dPercentage > 999 Or dPercentage < -99 Then
                ' its invalid as a policy cannot be discounted by more than 99%
                ' and cannot be loaded by more than 999%.
                dPercentage = 0
            End If

            txtDiscountPercentage.Text = StringsHelper.Format(dPercentage, kDiscountPercentageFormat)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ActionDiscountPercentageChange
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ActionDiscountPercentageChange() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ActionDiscountPercentageChange"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the user has entered invalid data
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtDiscountPercentage.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                ' reset to the last known valid discount percentage
                txtDiscountPercentage.Text = StringsHelper.Format(m_dPrevDiscountPercentage, kDiscountPercentageFormat)
            End If

            ' if the discount percentage has been changed manually
            If m_dPrevDiscountPercentage <> CDbl(txtDiscountPercentage.Text) Then

                ' if the user has entered 0 as a percentage
                ' reset the premium to the original premium
                ' and enable the premium field for editing
                If CDbl(txtDiscountPercentage.Text) = 0 Then
                    txtDiscountedPremium.Text = StringsHelper.Format(m_crTotalPremiumDiscountOrig, kGenericCurrencyFormat)
                    txtDiscountedPremium.Enabled = True
                Else

                    result = RecalculateDiscountedPremium()

                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateDiscountedPremium Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' disable the discount premium field
                    txtDiscountedPremium.Enabled = False
                End If

            End If

            ' format the percentage
            txtDiscountPercentage.Text = StringsHelper.Format(txtDiscountPercentage.Text, kDiscountPercentageFormat)

            ' save the current discount percentage
            m_dPrevDiscountPercentage = gPMFunctions.ToSafeDouble(txtDiscountPercentage.Text, 0)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: txtDiscountPercentage_LostFocus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-11-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Sub txtDiscountPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDiscountPercentage.Leave

        Const kMethodName As String = "txtDiscountPercentage_LostFocus"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSubValue As Integer

        Try



            lReturn = ActionDiscountPercentageChange()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub


    ' ***************************************************************** '
    ' Name: SetTrueMonthlyPolicy
    '
    ' Parameters: bDirectBusiness Optional
    '
    ' Description: Sets the User Interface for the True Monthly Policy
    '
    ' History:
    'Created : Deepak : 29-06-2006
    ' ***************************************************************** '
    Private Function SetTrueMonthlyPolicy(Optional ByRef bDirectBusiness As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetTrueMonthlyPolicy"
        Dim i_CommissionFlag As Integer

        Try

            chkConsolidatedLeadCommission.Enabled = False
            chkConsolidatedSubCommission.Enabled = False

            If m_bIsTrueMonthlyPolicy Then

                Select Case m_sTransactionType
                    Case "NB"
                        If m_bISProductConfAllowLeadConsolidatedCommission Then
                            chkConsolidatedLeadCommission.Enabled = m_iAgentAllowedCommission = 1

                            If m_bISProductConfAllowSubConsolidatedCommission Then
                                chkConsolidatedSubCommission.CheckState = CheckState.Unchecked
                                If lvwAgents.Items.Count > 0 Then
                                    For Each lvwItem As ListViewItem In lvwAgents.Items

                                        m_lReturn = GetAllowConsolidatedCommissionValue(lvwItem.Text, i_CommissionFlag)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAgent_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                        End If
                                        If i_CommissionFlag = 1 Then
                                            chkConsolidatedSubCommission.CheckState = CheckState.Unchecked
                                            chkConsolidatedSubCommission.Enabled = True
                                            Exit For
                                        End If
                                        chkConsolidatedSubCommission.Enabled = False

                                    Next lvwItem
                                End If

                            End If
                        Else
                            chkConsolidatedLeadCommission.Enabled = False
                        End If
                    Case "MTA"
                        chkConsolidatedLeadCommission.Enabled = False
                        chkConsolidatedSubCommission.Enabled = False
                    Case "EDIT"

                    Case Else
                        If IsRenewal Then
                            If m_lAnniversaryCopy = 0 Then
                                chkConsolidatedLeadCommission.Enabled = False
                                chkConsolidatedSubCommission.Enabled = False
                            Else
                                chkConsolidatedLeadCommission.Visible = False
                                chkConsolidatedSubCommission.Visible = False
                            End If
                        End If
                End Select
            Else
                chkConsolidatedLeadCommission.Visible = False
                chkConsolidatedSubCommission.Visible = False
                lblConsolidatedLeadCommission.Visible = False
                lblConsolidatedSubCommission.Visible = False
            End If
            If pnlAgentCode.Text.Trim() = "" Then
                chkConsolidatedLeadCommission.CheckState = CheckState.Unchecked
            End If
            If bDirectBusiness Then
                chkConsolidatedLeadCommission.Enabled = False
                chkConsolidatedLeadCommission.CheckState = CheckState.Unchecked
            End If
            result = 1


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    ' ***************************************************************** '

    ' Name: GetAllowConsolidatedCommissionValue
    '
    ' Parameters: sShortName,i_rCommissionFlag
    '
    ' Description: Get the Allow Consolidated Commission Flag Value
    '
    ' History:
    '    True Monthly Policy
    '    Created : Deepak : 29-06-2006
    ' ***************************************************************** '


    Private Function GetAllowConsolidatedCommissionValue(ByRef sShortname As String, ByRef i_rCommissionFlag As Integer) As Integer
        Dim result As Integer = 0

        Dim oFindParty As bSIRFindParty.Business

        'Const kMethodName As String = "GetAllowConsolidatedCommissionValue"        ''Unused Local Variables

        result = gPMConstants.PMEReturnCode.PMTrue
        'Get an instance of the business object via
        ' the public object manager.
        Dim temp_oFindParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oFindParty = temp_oFindParty
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowConsolidatedCommissionValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If


        m_lReturn = oFindParty.GetCommissionFlag(sShortName:=sShortname, i_rAllow_Commission_Flag:=i_rCommissionFlag)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowConsolidatedCommissionValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result
    End Function

    ''' <summary>
    ''' GetBranchDefaultAgent
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBranchDefaultAgent() As Integer


        Dim nSourceId As Integer = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)
        Dim oResultArray(,) As Object = Nothing


        m_lReturn = m_oBusinessPolicy.GetDefaultBranchAgent(nSourceId, oResultArray)

        If Information.IsArray(oResultArray) Then

            pnlAgentCode.Tag = CStr(oResultArray(0, 0))
            m_vLeadAgentCnt = pnlAgentCode.Tag
            pnlAgentCode.Text = CStr(oResultArray(1, 0))
            SetAltRefMandatory()
        Else
            pnlAgentCode.Text = ""
            pnlAgentCode.Tag = CStr(0)
        End If
        m_vLeadAgentCnt = pnlAgentCode.Tag
        Return m_lReturn
    End Function

    ' ***************************************************************** '
    ' Name: AddAssociatedSubAgent
    '
    ' Description: Adds associated sub agents to the list view if
    ' raise commission transactions against this associated agent
    ' is checked in the iPMBAssociates.
    ' ***************************************************************** '
    Private Function AddAssociatedSubAgent(ByVal v_lLeadAgentCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRFindParty As Object

        Const kMethodName As String = "AddAssociatedSubAgent"


        Dim oFindParty As bSIRFindParty.Business
        Dim vSubAgentCnt As Integer
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get business object of the component bSIRFindParty.
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business object", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Information.IsArray(m_vGetAssociatedSubAgent) Then
                m_vGetAssociatedSubAgent = Nothing
            End If

            ' Get associated sub agents to lead agent having "raise commission transaction
            ' against this associated agent" check box active in iPMBAssociates.

            m_lReturn = oFindParty.GetAssociatedSubAgent(v_lLeadPartyCnt:=v_lLeadAgentCnt, r_vGetAssociatedSubAgent:=m_vGetAssociatedSubAgent)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.RaiseError(kMethodName, "Failed to get associated sub-agents", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Add Associated SubAgents to list view.
            If Information.IsArray(m_vGetAssociatedSubAgent) Then
                For lRow As Integer = m_vGetAssociatedSubAgent.GetLowerBound(1) To m_vGetAssociatedSubAgent.GetUpperBound(1)

                    ' add to list view if not already in the list
                    If IsInListView(gPMFunctions.ToSafeLong(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, lRow)), lvwAgents) = gPMConstants.PMEReturnCode.PMFalse Then

                        ' Sub Agent short Name

                        oListItem = lvwAgents.Items.Add(gPMFunctions.ToSafeString(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentShortName, lRow)).Trim()), "AgentImage")

                        ' Sub Agent Name
                        If gPMFunctions.ToSafeString(m_vGetAssociatedSubAgent(kAssociatedSubAgentResolvedName, lRow)) <> "" Then
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(m_vGetAssociatedSubAgent(kAssociatedSubAgentResolvedName, lRow))
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(m_vGetAssociatedSubAgent(kAssociatedSubAgentName, lRow))
                        End If

                        'Sub Agent party_cnt
                        oListItem.Tag = CStr(gPMFunctions.ToSafeLong(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, lRow)))

                        ' refreshes the list
                        lvwAgents.Refresh()

                    End If
                Next
                RaiseEvent SubAgentChange(Me, Nothing)
            End If
            m_lReturn = SetTrueMonthlyPolicy()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally

            ' Terminate the business object
            If Not (oFindParty Is Nothing) Then

                oFindParty.Dispose()
                oFindParty = Nothing
            End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetAltRefMandatory                          '(RC) QBENZ014
    '
    ' Description: Sets AlternateReference field mandatory if
    ' all conditions are met
    ' ***************************************************************** '
    Private Sub SetAltRefMandatory()

        Dim bAgentBusiness As Boolean

        Dim vAltRefMandatory As Object = False

        If cboBusinessType.SelectedIndex < 0 Then
            Exit Sub
        End If

        If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 1 Then
            bAgentBusiness = True
            If gPMFunctions.ToSafeString(pnlAgentCode.Text.Trim()) <> "" Then

                m_lReturn = m_oBusiness.GetFromTable("party_agent", "alternate_reference_mandatory", "party_cnt", m_vLeadAgentCnt, vAltRefMandatory)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        Else
            bAgentBusiness = False
        End If

        If bAgentBusiness And gPMFunctions.ToSafeBoolean(vAltRefMandatory, 0) Then
            m_oFormFields.Item("txtAlternateReference-0").IsMandatory = True
            lblAlternateReference.Font = VB6.FontChangeName(lblAlternateReference.Font, "Arial")
            lblAlternateReference.Font = VB6.FontChangeBold(lblAlternateReference.Font, True)
        Else
            m_oFormFields.Item("txtAlternateReference-0").IsMandatory = False
            lblAlternateReference.Font = VB6.FontChangeName(lblAlternateReference.Font, "Verdana")
            lblAlternateReference.Font = VB6.FontChangeBold(lblAlternateReference.Font, False)
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: SetCoverNoteMandatory
    '
    ' Description: Setup interface for Cover Note Validation (REL001)
    ' ***************************************************************** '
    Private Function SetCoverNoteInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "SetCoverNoteInterface"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sTransactionType = "NB" And m_iTask <> gPMConstants.PMEComponentAction.PMView And VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) = 7 Then
                txtCoverNoteBook.Enabled = True
                txtCoverNoteSheet.Enabled = True
            Else
                'Clear and disable
                If VB6.GetItemData(cboBusinessType, cboBusinessType.SelectedIndex) <> 7 Then
                    txtCoverNoteBook.Text = ""
                    txtCoverNoteSheet.Text = ""
                End If
                txtCoverNoteBook.Enabled = False
                txtCoverNoteSheet.Enabled = False
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally

            'Clean up code will go here


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateCoverNote
    '
    ' Description: This will validate Cover Note
    '
    ' ***************************************************************** '
    Private Function ValidateCoverNote() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lResult As Integer
        Dim sSheetStatus As String = ""

        Const kMethodName As String = "ValidateCoverNote"

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If txtCoverNoteBook.Text.Trim() <> "" And txtCoverNoteSheet.Text.Trim() = "" Then
                System.Windows.Forms.MessageBox.Show("Please enter cover note sheet number to proceed.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            ElseIf txtCoverNoteBook.Text.Trim() = "" And txtCoverNoteSheet.Text.Trim() <> "" Then
                System.Windows.Forms.MessageBox.Show("Please enter cover note book number to proceed.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            ElseIf txtCoverNoteSheet.Text.IndexOf("."c) >= 0 Then
                System.Windows.Forms.MessageBox.Show("Cover note sheet number entered is not a valid number.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            ElseIf gPMFunctions.ToSafeLong(txtCoverNoteSheet.Text.Trim()) <= 0 Then
                System.Windows.Forms.MessageBox.Show("Cover note sheet number entered is not a valid number.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            Else


                lReturn = m_oCoverNoteBusiness.ValidateCoverNoteSheet(lInsuranceFileCnt:=m_lInsuranceFileCnt, sCoverNoteBookNumber:=txtCoverNoteBook.Text.Trim(), lCoverSheetNumber:=gPMFunctions.ToSafeLong(txtCoverNoteSheet.Text), lBranchId:=gPMFunctions.ToSafeLong(VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)), lAgentCnt:=gPMFunctions.ToSafeLong(Convert.ToString(pnlAgentCode.Tag)), lProductId:=gPMFunctions.ToSafeLong(VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)), r_lResult:=lResult, r_sSheetStatus:=sSheetStatus)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to validate cover note sheet", gPMConstants.PMELogLevel.PMLogError)
                End If

                Select Case lResult
                    Case 1
                        result = gPMConstants.PMEReturnCode.PMTrue

                    Case 2
                        System.Windows.Forms.MessageBox.Show("Please check the status/availability of the book.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result

                    Case 3
                        System.Windows.Forms.MessageBox.Show("Cover note number not assigned to selected agent.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result

                    Case 4
                        System.Windows.Forms.MessageBox.Show("Cover note number not assigned to selected product.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result

                    Case 5
                        System.Windows.Forms.MessageBox.Show("Cover note number not assigned to selected branch.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result

                    Case 6
                    'TBD as and when specification will come

                    Case 7
                        'PN76917
                        System.Windows.Forms.MessageBox.Show("Cover Note Sheet entered is not valid.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    Case 8
                        If sSheetStatus.Trim().ToUpper() = "MISSING" Then
                            System.Windows.Forms.MessageBox.Show("Cover note status missing, policy cannot be issued." & Strings.Chr(13) & Strings.Chr(10) &
                                                             "Please change the status before issuing policy.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return result

                        ElseIf sSheetStatus.Trim().ToUpper() = "CANCEL" Then
                            System.Windows.Forms.MessageBox.Show("Cover note status cancelled, policy cannot be issued." & Strings.Chr(13) & Strings.Chr(10) &
                                                             "Please change the status before issuing policy.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return result

                        ElseIf sSheetStatus.Trim().ToUpper() = "ISSUED" Then
                            System.Windows.Forms.MessageBox.Show("Cover note number already converted to policy.", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return result

                        Else
                            System.Windows.Forms.MessageBox.Show("Cover note number entered is not a valid number", "Cover Note Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return result

                        End If

                End Select
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AssignCoverNote
    '
    ' Description: This will Assign Cover Note
    '
    ' ***************************************************************** '
    Private Function AssignCoverNote() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lResult As Integer
        Dim sSheetStatus As String = ""

        Const kMethodName As String = "AssignCoverNote"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oCoverNoteBusiness.AssignCoverNoteSheet(lInsuranceFileCnt:=m_lInsuranceFileCnt, sCoverSheetReference:=txtCoverNoteBook.Text.Trim(), lCoverSheetNumber:=gPMFunctions.ToSafeLong(txtCoverNoteSheet.Text)) ', |                                r_lResult:=lResult, |                                r_sSheetStatus:=sSheetStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to assign cover note sheet", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function
    ''' <summary>
    ''' CalculateRenewalDate
    ''' </summary>
    ''' <param name="v_bCalcRenewalDateFromCoverEndDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalculateRenewalDate(ByVal v_bCalcRenewalDateFromCoverEndDate As Boolean) As Integer

        Const kMethodName As String = "CalculateRenewalDate"

        Dim nResult As Integer
        Dim dtDate As Date
        Dim sInterval As String = ""
        Dim dNumber As Double
        Dim oRenewalFrequency As Object = Nothing

        If v_bCalcRenewalDateFromCoverEndDate Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCoverFromDate)
            If m_bSetUp Then
                Return nResult
            End If

            'Now we rejig everything else
            If txtCoverFromDate.Text.Trim() = "" Then
                Return nResult
            End If

            ' Only set this if we have a frequency
            If cboFrequency.SelectedIndex <> -1 Then

                'And if we're not doing an MTA
                If m_sTransactionType <> "MTA" And m_sTransactionType <> "MTR" And m_sTransactionType <> "MTC" And m_sTransactionType <> "DRI" And m_sTransactionType <> "PT" Then

                    dtDate = CDate(m_oFormFields.UnformatControl(txtCoverFromDate))

                    If m_sTransactionType = "NB" Then
                        m_lReturn = m_oFormFields.FormatControl(txtInceptionDate, dtDate)
                    End If

                    m_lReturn = m_oFormFields.FormatControl(txtInceptionTPI, dtDate)

                    'get renewal frequency details
                    m_lReturn = m_oBusinessPolicy.GetRenewalFrequencyDetail(v_lFrequencyID:=VB6.GetItemData(cboFrequency, cboFrequency.SelectedIndex), r_vResult:=oRenewalFrequency)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get renewal frequency details", gPMConstants.PMELogLevel.PMLogError)
                        txtCoverFromDate.Focus()
                    End If

                    sInterval = "m"

                    dNumber = CDbl(oRenewalFrequency(2, 0))
                    If m_bIsTrueMonthlyPolicy And gPMFunctions.ToSafeLong(m_lRenewalDayNumber) > 0 Then
                        If m_lRenewalDayNumber >= DateAndTime.Day(DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 1, 0)) Then
                            dtDate = DateAndTime.DateSerial(CDate(txtCoverFromDate.Text).Year, CDate(txtCoverFromDate.Text).Month + 1, 0)
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
                        Else
                            dtDate = DateAndTime.DateAdd(sInterval, dNumber, dtDate)
                        End If
                    Else
                        If dtDate.Month = 2 And dtDate.Day = 29 And dNumber = 12 Then
                            dtDate = DateAndTime.DateAdd("d", 1, DateAndTime.DateAdd(sInterval, dNumber, dtDate))
                        Else
                            dtDate = DateAndTime.DateAdd(sInterval, dNumber, dtDate)
                        End If
                    End If

                    'If we're renewing at midnight, cover ends the day before...
                    If m_iMidnightRenewal = 1 Then
                        dtDate = dtDate.AddDays(-1)
                    End If

                    If m_oDefaultCoverToDateToLastDay IsNot Nothing AndAlso IsArray(m_oDefaultCoverToDateToLastDay) AndAlso ToSafeBoolean(m_oDefaultCoverToDateToLastDay(0, 0)) AndAlso Not m_bIsTrueMonthlyPolicy Then
                        m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, DateAdd("d", -1 * (DateAndTime.Day(dtDate.AddDays(m_iMidnightRenewal)) - m_iMidnightRenewal), dtDate))

                        If CLng(m_iMidnightRenewal) = 1 Then
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, DateAdd("d", 1, txtCoverToDate))
                        Else
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, txtCoverToDate)
                        End If
                    ElseIf Not m_bIsTrueMonthlyPolicy Or (gPMFunctions.ToSafeLong(m_lRenewalDayNumber) = 0 And m_bIsTrueMonthlyPolicy) Then

                        If m_sSelectedPolicyStatus <> "Quote" And m_sSelectedPolicyStatus.Trim().Length > 0 AndAlso (m_sTransactionType = "REN" OrElse m_lInsuranceFileCnt = 0) Then
                            m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, dtDate)
                        End If

                        'renewal date is cover from date plus the period of the policy
                        'Insurance File count check is to restrict this set of statement for only new quote (not requote and not save quote)                    
                        If ((m_sTransactionType = "REN" OrElse m_sTransactionType = "NB") OrElse m_lInsuranceFileCnt = 0) AndAlso m_bRenCalDate Then
                            m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, dtDate)
                            dtDate = CDate(m_oFormFields.UnformatControl(txtCoverFromDate))
                            dtDate = DateAndTime.DateAdd(sInterval, dNumber, dtDate)
                            m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
                            m_bRenCalDate = False
                        End If
                    Else
                        SelectcboItem(cboRenewalDayNumber, m_lRenewalDayNumber)
                    End If

                    ' process true monthly policy dates
                    If m_bIsTrueMonthlyPolicy AndAlso gPMFunctions.ToSafeInteger(m_lRenewalDayNumber) > 0 Then
                        ProcessTMPDates(kProcessRenewalDatesFromRenewalDayNumber)
                    Else
                        ProcessTMPDates(kProcessRenewalDatesFromCoverFrom)
                    End If

                End If
            End If
        Else
            'Now we rejig everything else
            If txtCoverFromDate.Text.Trim() = "" Then
                Return nResult
            End If

            dtDate = CDate(m_oFormFields.UnformatControl(txtCoverFromDate))
            If (m_sTransactionType = "NB") Then
                m_lReturn = m_oFormFields.FormatControl(txtInceptionDate, dtDate)
            End If

            m_lReturn = m_oFormFields.FormatControl(txtInceptionTPI, dtDate)
            If dtDate < m_dtCommonRenewalDate Then
                dtDate = m_dtCommonRenewalDate
            Else

                If cboFrequency.SelectedIndex >= 0 Then

                    m_lReturn = m_oBusinessPolicy.GetRenewalFrequencyDetail(v_lFrequencyID:=VB6.GetItemData(cboFrequency, cboFrequency.SelectedIndex), r_vResult:=oRenewalFrequency)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to get renewal frequency details", gPMConstants.PMELogLevel.PMLogError)
                        txtCoverFromDate.Focus()
                    End If

                    sInterval = "m"

                    dNumber = CDbl(oRenewalFrequency(2, 0))
                    dtDate = DateAndTime.DateAdd(sInterval, dNumber, dtDate)
                    m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, dtDate)
                End If
            End If

            'If we're renewing at midnight, cover ends the day before...
            If m_iMidnightRenewal = 1 Then
                dtDate = dtDate.AddDays(-1)
            End If

            If (m_sTransactionType = "REN" OrElse m_lInsuranceFileCnt = 0) AndAlso m_bRenCalDate Then
                m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, dtDate)
            End If
        End If
        HandleLeapYearExtraDay()
        Return nResult

    End Function

    ''' <summary>
    ''' AgentChangeLogic
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AgentChangeLogic() As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "AgentChangeLogic"
        nResult = True

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            If m_dtCommonRenewalDate <> DateTime.MinValue Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRenewalDate,
                                                        vControlValue:=m_dtCommonRenewalDate)

                m_lReturn = CalculateRenewalDate(False)
                If m_bIsTrueMonthlyPolicy And m_bUnifiedRenewalDateIsReadOnly Then
                    txtCoverToDate.Enabled = False
                Else
                    txtCoverToDate.Enabled = True
                End If
            Else
                CalculateRenewalDate(True)
                If m_bIsTrueMonthlyPolicy And m_bUnifiedRenewalDateIsReadOnly Then
                    txtCoverToDate.Enabled = False
                Else
                    txtCoverToDate.Enabled = True
                End If
            End If
        End If
        Return nResult
    End Function


    'Developer Guide No 38. 
    Private Sub uctPMUPolicyControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cboPolicyStatus.FirstItem = ""
        Me.cboPolicyLimits.FirstItem = ""
        Me.cboPolicyDeductible.FirstItem = ""
        Me.cboUnderwritingYearID.FirstItem = "(None)"
        Me.cboDiscountReason.FirstItem = "(None)"
        Me.cboDiscountRecurringType.FirstItem = "(None)"
        Me.cboPolicyStyle.FirstItem = "(N/A)"
        Me.cboCorrespondenceMethod.FirstItem = ""
    End Sub

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabMainTab.SelectedIndex = 2
        End If
        If tabMainTab.TabPages.Count > 3 Then
            If e.Alt And e.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
        End If
        If tabMainTab.TabPages.Count > 4 Then
            If e.Alt And e.KeyCode = Keys.D5 Then
                tabMainTab.SelectedIndex = 4
            End If
        End If
    End Sub

    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, Optional ByVal sValue As String = "") As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oPartyAG As bSIRPartyAG.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = g_oObjectManager.GetInstance(oPartyAG, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result)
            End If
            If m_bIsSubAgentAdded AndAlso sValue = "1" Then
                Dim iTotalAgent As Integer = lvwAgents.Items.Count
                Dim iRow As Integer
                Dim sPartyCode As String = ""
                For iRow = 0 To lvwAgents.Items.Count - 1
                    oListItem = lvwAgents.Items.Item(iRow)
                    sPartyCode = CStr(ListViewHelper.GetListViewSubItem(oListItem, 0).Text)

                    If oPartyAG.GetAndValidateSubAgentDetails(v_sPartyCode:=sPartyCode, v_dtCoverStartDate:=CDate(txtCoverFromDate.Text), v_dtCoverEndDate:=CDate(txtCoverToDate.Text), r_bIsValid:=bIsValid) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If bIsValid = False Then
                        System.Windows.Forms.MessageBox.Show("You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent-" + CStr(sPartyCode), ACApp, MessageBoxButtons.OK)
                        Return result
                    End If
                Next
            End If
            Return result
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally
            oPartyAG.Dispose()
            oPartyAG = Nothing
        End Try

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetAgencyContactDetails
    '
    ' Description: WPR73-74
    '
    ' History: 1/11/2011 SJ - Created.
    '          Amended to get details of agency contact
    ' ***************************************************************** '
    Public Function GetAgencyContactDetails(ByVal vPartyCnt As Integer) As Integer

        Dim result As Integer = 0

        Dim obFindParty As bSIRFindParty.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Const ACAgencyContactID As Integer = 0                 ''Unused Local Variables
            'Const ACAgencyContactDescription As Integer = 1        ''Unused Local Variables
            Dim vGetAgentUserDetails(,) As Object
            cboAgencyContact.Items.Clear()


            cboAgencyContact.Text = "Select"

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obFindParty = temp_oFindParty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GetAgentUserDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowConsolidatedCommissionValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = obFindParty.GetAgentUserDetails(vPartyCnt, vGetAgentUserDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                obFindParty.Dispose()
                obFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GetAgentUserDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowConsolidatedCommissionValue", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
            m_lReturn = obFindParty.CheckAgentReceiveCorrespondenceFlag(vPartyCnt, m_bAgentReceiveCorrespondenceFlag)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                obFindParty.Dispose()
                obFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get CheckAgentReceiveCorrespondenceFlag", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredCorrespondence", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If m_bAgentReceiveCorrespondenceFlag Then
                lblCorrespondenceType.Text = "Agent Correspondence"
            Else
                lblCorrespondenceType.Text = "Client Correspondence"
            End If

            obFindParty.Dispose()

            obFindParty = Nothing


            If Not Information.IsArray(vGetAgentUserDetails) Then
                Return result
            End If

            If vGetAgentUserDetails IsNot Nothing Then
                Dim cboAgency_NewIndex1 As Integer = -1
                cboAgency_NewIndex1 = cboAgencyContact.Items.Add("Select")
                VB6.SetItemData(cboAgencyContact, cboAgency_NewIndex1, 0)
                For i As Integer = 0 To vGetAgentUserDetails.GetUpperBound(1)
                    cboAgencyContact.Items.Add(New VB6.ListBoxItem(vGetAgentUserDetails(1, i), vGetAgentUserDetails(0, i)))
                Next i
            End If

            If CDbl(m_vContactuserId) = 0 Then
                cboAgencyContact.SelectedIndex = 0
            End If

            If vGetAgentUserDetails IsNot Nothing Then
                For i As Integer = 0 To vGetAgentUserDetails.GetUpperBound(1)
                    If m_vContactuserId = vGetAgentUserDetails(0, i) Then
                        cboAgencyContact.SelectedIndex = i + 1
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgencyContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgencyContactDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboAgencyContact_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAgencyContact.SelectedIndexChanged
        If cboAgencyContact.SelectedIndex <> -1 Then
            m_vContactuserId = VB6.GetItemData(cboAgencyContact, cboAgencyContact.SelectedIndex)
        End If
    End Sub

    Private Function IsValidString(ByRef str As String) As Boolean
        Dim illegalChars As Char() = ":~""#%&*:<>?/\{}|".ToCharArray()

        For Each ch As Char In str
            If Array.IndexOf(illegalChars, ch) > -1 Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function ProcessMTAReasons(ByRef r_sMTAReasonDesc As String, ByRef r_iMTAReasonId As Integer) As Integer

        Const kMethodName As String = "ProcessMTAReasons"

        Dim oGetChangeReason As Object

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            Dim temp_oGetChangeReason As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGetChangeReason, sClassName:="iPMBGetChangeReason.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oGetChangeReason = temp_oGetChangeReason
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            oGetChangeReason.ProductId = m_lProductID
            oGetChangeReason.TransactionType = "MTA"
            oGetChangeReason.FormCaption = "Policy " & m_sTransactionType
            oGetChangeReason.Start()

            If oGetChangeReason.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                r_sMTAReasonDesc = oGetChangeReason.ReasonDescription
                r_iMTAReasonId = oGetChangeReason.ReasonId
                m_oBusiness.IsManualDescription = 1
            Else
                r_sMTAReasonDesc = ""
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            oGetChangeReason.Dispose()
            oGetChangeReason = Nothing
            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

    ''' <summary>
    ''' Recalculate discounted premium
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecalculateDiscountedPremium() As Integer

        Dim nResult As Integer = 0
        Dim crPremium As Decimal = 0
        Dim dPercentage As Double = 0
        Dim crOriginalPremium As Decimal = 0

        Try
            'Discounted Premium should be calculated from the original premium

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' get the original premium
            crOriginalPremium = ToSafeCurrency(m_crTotalPremiumDiscountOrig, 0)

            'get the new discount / loading percentage Format(m_dPrevDiscountPercentage, kDiscountPercentageFormat)
            dPercentage = CDbl(txtDiscountPercentage.Text)

            'calculate the discounted premium
            'in comparision with the original premium
            'First check for valid percentage

            ' check for invalid discount / loading percentages
            If dPercentage > 999 Or dPercentage < -99 Then
                ' its invalid as a policy cannot be discounted by more than 99%
                ' and cannot be loaded by more than 999%.
                dPercentage = 0
            Else
                crPremium = ((CDbl(txtDiscountPercentage.Text) + 100) / 100) * crOriginalPremium
            End If

            txtDiscountedPremium.Text = Format(crPremium, kGenericCurrencyFormat)

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to RecalculateDiscountedPremium", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateDiscountedPremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try

    End Function

    ''' <summary>
    ''' GetAgentChangeAuthority
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAgentChangeAuthority() As Long
        Dim nResult As Integer
        Const kMethodName As String = "GetAgentChangeAuthority"
        Dim oParams As Object
        ReDim oParams(kParamsCount)
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Get UserAuthorities Busines Object
            m_lReturn = CreateUserAuthorities()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Run CreateUserAuthorities", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get Authority details for Current User.
            m_lReturn = m_oUserAuthorities.getdetails(vUserID:=g_iUserId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Run m_oUserAuthorities.getdetails", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oUserAuthorities.getnext(vParams:=oParams)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Run m_oUserAuthorities.getdetails", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bAgentEditable = ToSafeBoolean(oParams(kEditAgentDuringMTAMTC), False)

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetAgentChangeAuthority", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' gets associated branch with the agent
    ''' </summary>
    ''' <param name="r_bValidBranch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAssosiatedAgentBranch(ByRef r_bValidBranch As Boolean) As Long

        Dim oResultArray(,) As Object
        Dim nSourceId As Integer = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)

        oResultArray = Nothing
        If m_vLeadAgentCnt <> 0 Then
            m_lReturn = m_oBusinessPolicy.GetAssosiatedAgentBranch(nSourceId, m_vLeadAgentCnt, m_sTransactionType, oResultArray)

            If Not (IsArray(oResultArray)) Then
                MsgBox("The Agent does not have access to the selected branch", vbOKOnly, "Branch Access")
            Else
                r_bValidBranch = True
            End If
        End If
        GetAssosiatedAgentBranch = m_lReturn
    End Function

    Private Sub fraRisks_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fraRisks.Enter

    End Sub

    Private Sub cboBusinessType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
       Handles cboBusinessType.SelectedIndexChanged
        optGross.Checked = False
        optNet.Checked = False
        If (m_sTransactionType = "NB") OrElse (m_sTransactionType = "REN") OrElse (m_sTransactionType = "MTR") Then
            If (cboBusinessType.Text.Trim() = "Co-Insurance Lead") OrElse (cboBusinessType.Text.Trim() = "Co-Insurance Follow") _
                Then
                grpCoInsuranceLead.Visible = True
                lblCoinsurancePlacement.Visible = True
            Else
                grpCoInsuranceLead.Visible = False
                lblCoinsurancePlacement.Visible = False
            End If
        ElseIf (m_sTransactionType = "MTA") OrElse (m_sTransactionType = "MTC") Then
            grpCoInsuranceLead.Visible = True
            lblCoinsurancePlacement.Visible = True
            grpCoInsuranceLead.Enabled = False

        End If
        If cboBusinessType.Text.Trim() = "Direct Business" Then
            cboCorrespondenceMethod_ItemCodeChange()
            m_bAgentReceiveCorrespondenceFlag = False
        Else
            cboCorrespondenceMethod_ItemCodeChange()
        End If
    End Sub

    Private Sub HandleLeapYearExtraDay()
        If ((m_sTransactionType = "NB") OrElse (m_sTransactionType = "REN")) AndAlso txtCoverFromDate.Text.Trim <> "" AndAlso DateTime.IsLeapYear(Year(txtCoverFromDate.Text)) = True AndAlso Month(txtCoverFromDate.Text) = 2 AndAlso Convert.ToDateTime(txtCoverFromDate.Text).Day = 29 Then
            If m_bIsTrueMonthlyPolicy = False Then
                txtCoverToDate.Text = String.Empty

                If CInt(m_iMidnightRenewal) = 1 Then
                    m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, Convert.ToDateTime(txtCoverFromDate.Text).AddYears(1))
                    m_lReturn = m_oFormFields.FormatControl(txtRenewalDate, Convert.ToDateTime(txtCoverToDate.Text).AddDays(1))
                Else
                    m_lReturn = m_oFormFields.FormatControl(txtCoverToDate, Convert.ToDateTime(txtCoverFromDate.Text).AddYears(1).AddDays(1))
                    txtRenewalDate.Text = txtCoverToDate.Text
                End If
                'm_lReturn = m_oFormFields.FormatControl(txtRenewalDate, DateTime.Today.AddYears(1).AddDays(1))

            End If

        End If

    End Sub

    Private Sub chkHandler_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHandler.CheckedChanged
        If Not chkHandler.Checked Then
            pnlHandler.Tag = "0"
            m_vAccountHandlerCnt = 0
            lblHandler.Text = ""
            cmdHandler.Enabled = False
        Else
            cmdHandler.Enabled = True
        End If
    End Sub

    Private Sub cboCorrespondenceMethod_ItemCodeChange() Handles cboCorrespondenceMethod.ItemCodeChange

        If cboBusinessType.Text.Trim = "Direct Business" OrElse String.IsNullOrEmpty(cboBusinessType.Text) Then
            lblCorrespondenceType.Text = "Client Correspondence"
            If cboCorrespondenceMethod.ItemCode.Trim.ToUpper = "DEFAULT" Then
                txtCorrespondenceType.Visible = True
                If IsArray(m_vClientDefaultPreferredCorrespondence) Then
                    txtCorrespondenceType.Text = ToSafeString(m_vClientDefaultPreferredCorrespondence(2, 0))
                End If
            Else
                txtCorrespondenceType.Visible = False
                txtCorrespondenceType.Text = String.Empty
            End If
        Else

            If m_bAgentReceiveCorrespondenceFlag Then
                lblCorrespondenceType.Text = "Agent Correspondence"
                If cboCorrespondenceMethod.ItemCode.Trim.ToUpper = "DEFAULT" Then
                    txtCorrespondenceType.Visible = True
                    If IsArray(m_vAgentDefaultPreferredCorrespondence) Then
                        txtCorrespondenceType.Text = ToSafeString(m_vAgentDefaultPreferredCorrespondence(2, 0))
                    End If
                Else
                    txtCorrespondenceType.Visible = False
                    txtCorrespondenceType.Text = String.Empty
                End If
            Else
                lblCorrespondenceType.Text = "Client Correspondence"
                If cboCorrespondenceMethod.ItemCode.Trim.ToUpper = "DEFAULT" Then
                    txtCorrespondenceType.Visible = True
                    If IsArray(m_vClientDefaultPreferredCorrespondence) Then
                        txtCorrespondenceType.Text = ToSafeString(m_vClientDefaultPreferredCorrespondence(2, 0))
                    End If
                Else
                    txtCorrespondenceType.Visible = False
                    txtCorrespondenceType.Text = String.Empty
                End If
            End If
        End If
    End Sub
    Private Sub lvwPolicyWording_ItemChecked(sender As Object, e As ItemCheckedEventArgs)
        If e.Item.Checked Then
            e.Item.ImageIndex = 14
        Else
            e.Item.ImageIndex = 13
        End If
    End Sub

End Class
