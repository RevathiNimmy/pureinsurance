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
Imports System.Text
Imports System.Windows.Forms

Imports SharedFiles
'imported the namespace
Imports uctPartyBank

Partial Friend Class frmDetails
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDetails
    '
    ' Date: 8th August 1997
    '
    ' Description: Interface for Item Details Tab.
    '
    ' Edit History:
    ' RAW 06/03/2003 : do not display instalment number 0
    ' MEvans : 06-12-2004 : PN17327
    '              updated cashlistitem so that cashlistitem receipts
    '              are exempt from payment authority checks
    ' CJB 12/01/2005 : Perkins Slade Retail Logic integration
    '                  Added 5 new public vars for additional fields on uctACTCreditCard:
    '                  CCname, CCcustomer, CCmanualauthcode, CCtransactioncode and MediaTypeIssuerID.
    '                  Added a new usercontrol - uctACTCreditCard to Payment & Receipt tabs.
    '                  Properties for it set in PropertiesToInterface and fetched back in
    '                  InterfaceToProperties. Set further new usercontrol properties in
    '                  GetInterfaceDetails, cmbMediaType_LostFocus, ClearCreditCardDetails,
    '                  uctAccountLookup_LostFocus, cmdOK_Click (where we also call the .Validate
    '                  method on the usercontrol) and cboIssuer_LostFocus. Also added a new Issuer
    '                  combo and populate triggers in cmbMediaType_Click and cboPaymentType_Click.
    '                  The combo will only be visible (and mandatory) if CC media type selected and
    '                  if there are any issuers available.
    ' CJB 31/01/2005 : PN18417 Change PropertiesToInterface to NOT allow editing of the amount if
    '                  "Allow Partial Payments" system option set AND we're processing an Insurer Payment.
    ' CJB 03/02/2005 : PN18415 Change DisplayLookupDetails to remove "Instalment Debt" from the Receipt
    '                  Type combo doing a receipt via a roadmap or an Insurer Payment instead of disabling
    '                  the receipt type as as being done previously. Also change PropertiesToInterface to
    '                  NOT disable the PaymentType combo if via Insurer Payment or Claim Payment.
    ' CJB 18/02/2005 : PN18882 If payment expiry date is 00:00:00 then show as ""
    ' ***************************************************************** '
    Private Const vbFormCode As Integer = 0
    'SW front office receipting changes, get rid of public property procedures
    'and replace with public variables
    Public CashlistitemID As Integer
    Public AllocationStatusID As Integer
    Public MediaTypeID As Integer
    Public CashlistID As Integer
    Public AccountID As Integer
    Public MediaRef As String = ""
    Public OurRef As String = ""
    Public TheirRef As String = ""
    Public ChequePartyName As String = ""
    Public ViaFinancePlan As Boolean
    Public ViaInsurerPayment As Boolean
    Public ViaClaimPayment As Boolean 'AR20050125 - PN18271
    Public Amount As Decimal
    Public ContactName As String = ""
    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public PostalCode As String = ""
    Public AddressCountry As Integer
    Public PaymentName As String = ""
    Public PaymentAccountCode As String = ""
    Public PaymentBranchCode As String = ""
    Public PaymentExpiryDate As Date
    Public PaymentReference1 As String = ""
    Public PaymentReference2 As String = ""
    Public Letter As Boolean
    Public CashlistTypeID As Integer
    Public CurrencyID As Integer
    Public CompanyID As Integer
    Public WriteOffAmount As Decimal
    Public IsWOFF As Boolean
    Public sBIC As String = String.Empty
    Public sIBAN As String = String.Empty

    Public BatchID As Integer
    Public PMUserid As Integer
    Public Transactiondate As Date
    Public Originalamount As Decimal
    Public Amounttendered As Decimal
    Public Change As Decimal
    Public Cashlistitemreceipttypeid As Integer
    Public Cashlistitemreceiptstatusid As Integer
    Public Cashlistitembankid As Integer
    Public Chequedate As Object
    Public CCnumber As String = ""
    Public CCexpirydate As Object
    Public CCstartdate As Object
    Public CCissue As String = ""
    Public CCpin As String = ""
    Public CCauthcode As String = ""
    Public CCname As String = ""
    Public CCcustomer As String = ""
    Public CCmanualauthcode As String = ""
    Public CCtransactioncode As String = ""
    Public MediaTypeIssuerID As Integer
    Public Receiptdetails As String = ""
    Public CashListItemReversePMUserID As Integer
    Public CashListItemReverseReasonID As Integer
    Public CashListItemPaymentTypeID As Integer
    Public CashListItemPaymentStatusID As Integer
    Public Datepresented As Object
    Public Chequeinpossession As Boolean
    Public Stoprequesteddate As Object
    Public Stopprinteddate As Object
    Public Stopconfirmationdate As Object
    Public Reason As String = ""
    Public Replacescashlistitemid As Integer
    'used to hold a given clients instalment plan details
    Public InstalmentArray As Object
    Public SalvageArray As Object
    'cash drawer properties
    Public CashDrawerID As Integer
    Public CashDrawerDescription As String = ""
    Public CDBatchID As Integer
    Public CDFutureChequeDays As Integer
    Public CDDrawerReceiptType As Integer
    Public CDDrawerMediaType As Integer
    Public UnderwritingYearID As Object

    'reverse flag sw front office receipting
    Public m_bReverseCashDrawerListItem As Boolean
    Public m_lTransDetailID As Integer

    'Party Bank Details
    Public m_vBankPaymentTypeId As Object
    Public InsurerPaymentInstArray As Object
    Public vStepDetails As Object
    Public nApprovalSteps As Integer
    'electronic receipting
    Public sXML As String = ""

    Public ActionKey As String = ""

    'Currency Conversion
    Public CurrencyBaseDate As Date
    Public CurrencyBaseXRate As Double
    Public AccountBaseDate As Date
    Public AccountBaseXrate As Double
    Public SystemBaseDate As Date
    Public SystemBaseXrate As Double
    Public OverrideReason As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmDetails"

    'Recovery By Instalments changes
    'Pull all the consts for Tab definition together
    Private Const TAB_DETAILS As Integer = 0
    Private Const TAB_RECEIPT As Integer = 1
    Private Const TAB_PAYMENT As Integer = 2
    Private Const TAB_ADDRESS As Integer = 3
    Private Const TAB_INSTALMENT As Integer = 5
    Private Const TAB_BANKGUARANTEE As Integer = 6

    Private m_oFormFields As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lSaveAccountID As Integer
    Private m_sSaveAccountText As String = ""
    Private m_sSaveMediaTypeText As String = ""
    Private m_sSaveIssuerText As String = ""

    ' Declare an instance of the List Form.
    Private m_oListForm As Object

    ' Declare an instance of the CLI business object (set from List Form).

    'Changed as per VB Code
    'Private m_oBusiness As bACTCashlistitem.Form
    Private m_oBusiness As Object
    Private m_oCashlistitem As Object
    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object
    ' Store MediaTypes locally now that they contain more data than lookups
    Private m_vMediaResultArray(,) As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last text box controls for each tab
    Private m_ctlTabFirstLast(,) As Control

    'Required for Credit Card processing
    Private m_lInsuranceFileCnt As Integer
    Private m_lGISSchemeID As Integer
    Private m_sGISDataModelCode As String = ""

    'sw front office receipting
    Private sAmountFieldText As String = ""
    Private m_bInstalmentProcessing As Boolean

    Private m_bRemovingItems As Boolean
    Private m_bAddingItems As Boolean
    Private m_lInstalmentAccountID As Integer

    Private m_bPopulatingLookUps As Boolean
    Private m_bOKClicked As Boolean
    Private m_lPMUserGroupId As Integer

    Private Const ACEnabledColor As Integer = -2147483630
    Private Const ACDisabledColor As Integer = -2147483631
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    Private Const ACMediaNumberingScheme As Integer = 14
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    'Media Type Array Constants
    Private Const ACMediaTypeID As Integer = 0
    Private Const ACMediaDescription As Integer = 2
    Private Const ACMediaCode As Integer = 3
    Private Const ACIsValidationEnabled As Integer = 6
    Private Const ACIsBanking As Integer = 7
    Private Const ACIsStoppable As Integer = 8
    Private Const ACIsManualPayment As Integer = 11
    Private Const ACIsMediaRefMandatory As Integer = 12
    Private Const ACIsPrintedAutomatically As Integer = 13
    'WPR12- Enhancement Quote Collection Process
    Private Const ACIsAdditionalOptions As Integer = 16

    'Safe to hard code these as they won't change
    Private Const ACPaymentStatusIssued As Integer = 1
    Private Const ACPaymentStatusPendingApproval As Integer = 7
    Private Const ACPaymentStatusDeclined As Integer = 10

    'Recovery By Instalments changes
    Public m_vReversedRBIItems As Object
    Public m_vUnsavedRBIItems As Object
    Private m_bOnInstalment As Boolean

    Private m_lApprovalType As Integer

    Public CashListRoadmap As String = ""

    Private m_bOptionElectronicPayment As Boolean
    Private m_bAccountElectronicPayment As Boolean

    Private m_bOptionUnderwritingYear As Boolean
    Private m_bSystemOptionUWYearMandatory As Boolean

    Private Const ACClaimPaymentsType As Integer = 1
    Private Const ACPaymentsType As Integer = 2

    Private m_oCurrencyConvert As Object
    Private m_oUserAuthorities As Object

    Private m_cInstalmentBaseTotal As Decimal
    Private m_bAmountDisabled As Boolean
    Private m_sScreenType As String = ""
    Private m_lClaimPaymentId As Integer

    Private m_sClaimPaymentPayeeName As String = ""
    Private m_sClaimPaymentBankSortCode As String = ""
    Private m_sClaimPaymentBankAccountNo As String = ""
    Private m_lClaimPaymentPayeeCountryId As Integer
    Private m_sClaimPaymentPayeeComments As String = ""
    Private m_sClaimPaymentMediaRef As String = ""
    Private m_sClaimPaymentBIC As String = String.Empty
    Private m_sClaimPaymentIBAN As String = String.Empty

    Private m_sClaimPaymentAddress1 As String = ""
    Private m_sClaimPaymentAddress2 As String = ""
    Private m_sClaimPaymentAddress3 As String = ""
    Private m_sClaimPaymentAddress4 As String = ""
    Private m_sClaimPaymentPostCode As String = ""
    Private m_sClaimPaymentThirdPartyReference As String = ""

    Private m_bIsMediaRefMandatory As Boolean
    Private m_bIsvalidationEnabled As Boolean
    Private m_bSilentMultiCurrencyScreen As Boolean
    Private m_cBaseAmount As Decimal
    Public ViaBanking As Boolean

    Private m_dCollectionDate As Date
    Private m_sComments As String = ""

    Private m_vGetBGPoliciesForReceipt(,) As Object
    Private m_vSelBGPoliciesItemForReceipt As Object
    Private m_vSelectdBGPolicies(,) As Object

    Private m_cBalAmtTobeAllocated As Decimal
    Public Const ACThirdPartyOnly As Integer = 0

    'Start(Saurabh Agrawal) Tech Spec LOA010 Claim Payment Improvement
    Private m_sClaimPaymentOurRef As String = ""
    'End(Saurabh Agrawal) Tech Spec LOA010 Claim Payment Improvement
    Private m_sForRecommendation As String, m_bReceiptTypeIsInstalmentBased As Boolean
    Dim m_oPMAutoNumber As Object = Nothing
    Dim m_oDocumentPost As Object = Nothing
    Dim m_crAuthorityAmount As Decimal
    Public InsuranceRef As String = ""

    'Party Bank Details
    Private Enum ENPartyBank
        RowStatus = 0
        RowIndex = 1
        PartyBankId = 2
        IsBank = 3
        AccountID1 = 4
        BankPaymentTypeId = 5
        BankAccountTypeId = 6
        AccountHolderName = 7
        AccountNumber = 8
        BankNameId = 9
        BankBranch = 10
        BankBranchCode = 11
        BankAdd1 = 12
        BankAdd2 = 13
        BankAdd3 = 14
        BankTown = 15
        BankPCode = 16
        BankRegion = 17
        BankCountry = 18
        CCNum = 19
        CCstartdate1 = 20
        CCexpirydate1 = 21
        CCIssueNum = 22
        CCpin1 = 23
        IsRegistered = 24
        CCAdd1 = 25
        CCAdd2 = 26
        CCAdd3 = 27
        CCTown = 28
        CCPCode = 29
        CCCountry = 30
        IsDeleted1 = 31
        CCNameOnCard = 32
        CCManualAuthNumber = 33
        PFLINKEXISTS = 34
        CLILINKEXISTS = 35
        CPLINKEXISTS = 36
        BIC = 37
        IBAN = 38
    End Enum

    Private Enum ENPMLookups
        Id = 0
        Description = 1
    End Enum

    'Party Bank Details
    Private m_lBankPaymentTypeId As Integer
    Private m_vPartyBankDetails(,) As Object
    Private m_lPartyCnt As Integer
    Private m_lAccountID As Integer
    Private m_lPartyBankId As Integer

    'WPR12- Enhancement Quote Collection Process
    Public BankLocation As String = ""
    Public ChequeTypeId As Integer
    Public BBankBranch As String = ""
    Public CCBankId As Integer
    Public CardTypeId As Integer
    Public CardTransSlipNo As String = ""
    Public ChequeClearingTypeId As Integer
    'WPR12- Enhancement Quote Collection Process
    Private m_sCashListActualCalledFrom As String = ""
    Private m_bMultiplePoliciesSelected As Boolean
    Private m_lQuoteClientCnt As Integer
    Private m_lQuoteAgentCnt As Integer
    Private m_sQuoteAgentType As String = ""
    Private lsFormInitializing As Boolean = True
    Public m_oInsurerPaymentAllocateBusiness As Object

    ' Start - Sankar - PN 56728

    ' WPR 51
    Private m_bIsLeadAccount As Boolean
    Private m_dSplitReceiptRunningTotal As Decimal
    Private m_iInsurerAccountID As Integer
    Private m_bIsSplitReceipt As Boolean
    Private m_dSplitToal As Decimal
    Private m_sCCNameLead As String = ""
    Private m_sCCNumberLead As String = ""
    Private m_sCCExpiryDateLead As String = ""
    Private m_sCCissueLead As String = ""
    Private m_sCCpinLead As String = ""
    Private m_sCCstartdateLead As String = ""
    Private m_sCCauthcodeLead As String = ""
    Private m_sCCmanualauthcodeLead As String = ""
    Private m_sCCcustomerLead As String = ""
    Private m_sCCtransactioncodeLead As String = ""
    Private m_iLeadAccountBranchId As Integer
    Private m_dAuthorityAmount As Decimal

    Private m_sDocumentRef As String
    Private m_oListData As Object
    Public m_bAllocated As Boolean
    Private m_nCashListTypeID As Integer
    Private m_bIsInsurerePaymentRoadMap As Boolean
    Private m_crTaxAmount As Decimal
    Private m_nTaxBandID As Integer
    Private m_bIsReinsurer As Boolean
    Private m_bIsRIPaymentAndRecoveries As Boolean
    Private m_sBIC As String = String.Empty
    Private m_sIBAN As String = String.Empty


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
            Return m_nTaxBandID
        End Get
        Set(ByVal Value As Integer)
            m_nTaxBandID = Value
        End Set
    End Property

    Public Property PartyBank_Id() As Integer
        Get
            Return m_lPartyBankId
        End Get
        Set(ByVal Value As Integer)
            m_lPartyBankId = Value
        End Set
    End Property
    ' End - Sankar - PN 56728

    Public WriteOnly Property PartyBankPaymentTypeId() As Object
        Set(ByVal Value As Object)

            m_vBankPaymentTypeId = Value
        End Set
    End Property

    Public Property CollectionDate() As Date
        Get
            Return m_dCollectionDate
        End Get
        Set(ByVal Value As Date)
            m_dCollectionDate = Value
        End Set
    End Property

    Public ReadOnly Property BalAmtTobeAllocated() As Decimal
        Get
            Return m_cBalAmtTobeAllocated
        End Get
    End Property

    Public Property Comments() As String
        Get
            Return m_sComments
        End Get
        Set(ByVal Value As String)
            m_sComments = Value
        End Set
    End Property

    Public Property GetBGPoliciesForReceipt() As Object
        Get
            Return m_vGetBGPoliciesForReceipt
        End Get
        Set(ByVal Value As Object)

            m_vGetBGPoliciesForReceipt = Value
        End Set
    End Property

    Public Property SelBGPoliciesItemForReceipt() As Object
        Get
            Return m_vSelBGPoliciesItemForReceipt
        End Get
        Set(ByVal Value As Object)

            m_vSelBGPoliciesItemForReceipt = Value
        End Set
    End Property

    Public Property SelectdBGPolicies() As Object
        Get
            Return m_vSelectdBGPolicies
        End Get
        Set(ByVal Value As Object)

            m_vSelectdBGPolicies = Value
        End Set
    End Property

    Public WriteOnly Property SilentMultiCurrencyScreen() As Boolean
        Set(ByVal Value As Boolean)
            m_bSilentMultiCurrencyScreen = Value
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

    '****************
    ' MEvans : 14-05-2003 : CQ 709
    Public WriteOnly Property ApprovalType() As Integer
        Set(ByVal Value As Integer)
            m_lApprovalType = Value
        End Set
    End Property
    '****************

    ' RDC 14112003
    Public WriteOnly Property AllowElectronicPayment() As Boolean
        Set(ByVal Value As Boolean)
            m_bAccountElectronicPayment = Value
        End Set
    End Property

    Public Property PMUserGroupId() As Integer
        Get
            Return m_lPMUserGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lPMUserGroupId = Value
        End Set
    End Property
    Public WriteOnly Property Business() As bACTCashListItem.Form
        Set(ByVal Value As bACTCashListItem.Form)
            m_oBusiness = Value
        End Set
    End Property
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

    Public ReadOnly Property BaseAmount() As Decimal
        Get
            Return m_cBaseAmount
        End Get
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

    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Standard Property.
    ' Set the interface exit status.
    'm_lStatus = Value
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

    'TF230902
    Public WriteOnly Property InsuranceFileCnt() As Integer
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
    Public Property ForRecommendation() As String
        Get
            Return m_sForRecommendation
        End Get
        Set(ByVal Value As String)
            m_sForRecommendation = Value
        End Set
    End Property

    ' WPR 51 , use only for Split Receipt
    Public Property IsLeadAccount() As Boolean
        Get
            Return m_bIsLeadAccount
        End Get
        Set(ByVal value As Boolean)
            m_bIsLeadAccount = value
        End Set
    End Property

    Public Property SplitReceiptRunningTotal() As Decimal
        Get
            Return m_dSplitReceiptRunningTotal
        End Get
        Set(ByVal value As Decimal)
            m_dSplitReceiptRunningTotal = value
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

    Public Property IsSplitReceipt() As Boolean
        Get
            Return m_bIsSplitReceipt
        End Get
        Set(ByVal value As Boolean)
            m_bIsSplitReceipt = value
        End Set
    End Property

    Public Property SplitTotal() As Decimal
        Get
            Return m_dSplitTotal
        End Get
        Set(ByVal value As Decimal)
            m_dSplitTotal = value
        End Set
    End Property

    Public Property CollectionHasLead() As Boolean
        Get
            Return m_bCollectionHasLead
        End Get
        Set(ByVal value As Boolean)
            m_bCollectionHasLead = value
        End Set
    End Property

    Public Property MediaRefLead() As String
        Get
            Return m_sMediaRefLead
        End Get
        Set(ByVal value As String)
            m_sMediaRefLead = value
        End Set
    End Property

    Public Property MediaTypeIDLead() As Integer
        Get
            Return m_lMediaTypeIDLead
        End Get
        Set(ByVal value As Integer)
            m_lMediaTypeIDLead = value
        End Set
    End Property

    Public Property ChequeDateLead() As Date
        Get
            Return m_dChequeDateLead
        End Get
        Set(ByVal value As Date)
            m_dChequeDateLead = value
        End Set
    End Property

    Public Property NameLead() As String
        Get
            Return m_sNameLead
        End Get
        Set(ByVal value As String)
            m_sNameLead = value
        End Set
    End Property

    Public Property CCNameLead() As String
        Get
            Return m_sCCNameLead
        End Get
        Set(ByVal value As String)
            m_sCCNameLead = value
        End Set
    End Property

    Public Property CCNumberLead() As String
        Get
            Return m_sCCNumberLead
        End Get
        Set(ByVal value As String)
            m_sCCNumberLead = value
        End Set
    End Property

    Public Property CCExpiryDateLead() As String
        Get
            Return m_sCCExpiryDateLead
        End Get
        Set(ByVal value As String)
            m_sCCExpiryDateLead = value
        End Set
    End Property

    Public Property CCissueLead() As String
        Get
            Return m_sCCissueLead
        End Get
        Set(ByVal value As String)
            m_sCCissueLead = value
        End Set
    End Property

    Public Property CCpinLead() As String
        Get
            Return m_sCCpinLead
        End Get
        Set(ByVal value As String)
            m_sCCpinLead = value
        End Set
    End Property

    Public Property CCstartdateLead() As String
        Get
            Return m_sCCstartdateLead
        End Get
        Set(ByVal value As String)
            m_sCCstartdateLead = value
        End Set
    End Property

    Public Property CCauthcodeLead() As String
        Get
            Return m_sCCauthcodeLead
        End Get
        Set(ByVal value As String)
            m_sCCauthcodeLead = value
        End Set
    End Property

    Public Property CCmanualauthcodeLead() As String
        Get
            Return m_sCCmanualauthcodeLead
        End Get
        Set(ByVal value As String)
            m_sCCmanualauthcodeLead = value
        End Set
    End Property

    Public Property CCcustomerLead() As String
        Get
            Return m_sCCcustomerLead
        End Get
        Set(ByVal value As String)
            m_sCCcustomerLead = value
        End Set
    End Property

    Public Property CCtransactioncodeLead() As String
        Get
            Return m_sCCtransactioncodeLead
        End Get
        Set(ByVal value As String)
            m_sCCtransactioncodeLead = value
        End Set
    End Property

    Public Property ReceiptTypeIsInstalmentBased() As Boolean '2
        Get
            Return m_bReceiptTypeIsInstalmentBased
        End Get
        Set(ByVal value As Boolean)
            m_bReceiptTypeIsInstalmentBased = value
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

    ''' <summary>
    ''' Business Identifier Code(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return m_sBIC
        End Get
        Set(ByVal value As String)
            m_sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return m_sIBAN
        End Get
        Set(ByVal value As String)
            m_sIBAN = value
        End Set
    End Property
    Public Property PlanRef() As String
        Get
            Return m_sPlanRef
        End Get
        Set(ByVal Value As String)
            m_sPlanRef = Value
        End Set
    End Property

    'Selects a combobox item based on the ItemData value
    'SMJB CQ1210 Must set the payment status
    Function SelectComboItem(ByRef r_ctl As ComboBox, ByVal v_varID As Double) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lCount As Integer = 0 To r_ctl.Items.Count - 1
                If Conversion.Str(VB6.GetItemData(r_ctl, lCount)) = Conversion.Str(v_varID) Then
                    r_ctl.SelectedIndex = lCount
                End If
            Next

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the combo item", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectComboItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef oListForm As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the calling List form
            m_oListForm = oListForm

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormFields)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Load Interface defaults and get details replaces form load event
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadForm() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sLookUpcode As String = ""
        Dim nReturn As PMEReturnCode

        Try
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = GetInterfaceDetails()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If

            cmbMediaType_Leave(cmbMediaType, New EventArgs())

            'SW 30/04/2003 force validation for default receipt type
            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts AndAlso
                m_iTask = PMEComponentAction.PMAdd Then

                cboReceiptType_SelectedIndexChanged(cboReceiptType, New EventArgs())
                SetMandatoryAndEnabledFields(sMediaCode:=cmbMediaType.Text.ToUpper())

            ElseIf ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                    (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) AndAlso
                m_iTask = PMEComponentAction.PMAdd And cmbMediaType.SelectedIndex >= 0 Then
                With cmbMediaType

                    m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                            iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode)
                End With
                SetMandatoryAndEnabledFields(sMediaCode:=sLookUpcode)
            End If

            ' if there is an associated claim payment
            If m_lClaimPaymentId <> 0 Then
                ' use its details as defaults for this cash list item
                nReturn = SetupClaimPaymentDetails()
                If nReturn <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                    Return nResult
                End If
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
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
        Try

            '01/05/2003 - PWC - ENDVR00000850
            'Override all that has gone before - this seemed to be the easiest way to do it
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                DisableForm(True)
                cboInstalment.Enabled = True
                fraInstalments.Enabled = True
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ' Gaurav
                m_lReturn = PopulateBGDetailsList()
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.

            VB6.ShowForm(Me, lDisplayState)

            Return result

        Catch excep As System.Exception

            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates all interface details from the Form properties
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PropertiesToInterface() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sAllocationStatus As String
        Dim sLookUpcode As String
        Dim nLookUpID As Integer
        Dim oCashListBusiness As bACTCashList.Form
        Dim crBaseAmount As Decimal
        Dim crCurrencyAmount As Decimal
        Dim nCashListCurrencyID As Integer
        Dim dtCashListDate As Date
        Dim nCashListCompanyID As Integer
        Dim nBaseCurrencyID As Integer
        Dim sOptionValue As String = ""

        Try

            'Check the option for partial amounts
            iPMFunc.GetSystemOption(128, sOptionValue)
            'PN 32026 (RC)

            'PN #33048 PM
            If m_sScreenType = "CLP" OrElse ViaInsurerPayment Then
                m_bAmountDisabled = True
                Me.txtAmount.Enabled = False 'PN 56758
            Else
                m_bAmountDisabled = False
            End If
            If ViaBanking AndAlso Amount <> 0 Then
                m_bAmountDisabled = True
            End If

            'EK 090300 Enable edit of amount if unallocated
            'DC231001 -amount not editable if run via Insurer payment
            If AllocationStatusID = gACTLibrary.ACTAllocationStatusUnallocated AndAlso
                Not (ViaInsurerPayment OrElse ViaClaimPayment) Then
                'Disable amount if passed in and partial amount not allowed
                If g_bHasAccountContext AndAlso m_bAmountDisabled Then
                    txtAmount.Enabled = False
                    uctAccountLookup.Enabled = False
                    txtTendered.Text = CStr(Amounttendered)
                End If
            Else
                'DJM 08/12/2003
                Dim temp_oCashListBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashListBusiness,
                                                         "bACTCashList.Form",
                                                         vInstanceManager:=PMGetViaClientManager)
                oCashListBusiness = temp_oCashListBusiness
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to create instance of bACTCashList.Form",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="PropertiesToInterface",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = oCashListBusiness.GetDetails(vCashListID:=CashlistID)
                m_lReturn = oCashListBusiness.GetNext(vCompanyID:=nCashListCompanyID,
                                                      vCurrencyID:=nCashListCurrencyID,
                                                      vListDate:=dtCashListDate)

                If CurrencyID <> nCashListCurrencyID Then
                    m_lReturn = CreateCurrencyConvert()
                    ' Get the Company's base currency
                    m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=nCashListCompanyID,
                                                                   r_iBaseCurrencyID:=nBaseCurrencyID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If

                    If CurrencyID = nBaseCurrencyID Then
                        crBaseAmount = Amount
                        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCashListCurrencyID,
                                                                             lCompanyID:=nCashListCompanyID,
                                                                             cBaseAmount:=crBaseAmount,
                                                                             cCurrencyAmount:=crCurrencyAmount,
                                                                             vConversionDate:=dtCashListDate)
                        Amount = crCurrencyAmount
                    Else
                        crCurrencyAmount = Amount
                        m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=CurrencyID,
                                                                             lCompanyID:=nCashListCompanyID,
                                                                             cBaseAmount:=crBaseAmount,
                                                                             cCurrencyAmount:=crCurrencyAmount,
                                                                             vConversionDate:=dtCashListDate)

                        If nCashListCurrencyID <> nBaseCurrencyID Then
                            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=nCashListCurrencyID,
                                                                                 lCompanyID:=nCashListCompanyID,
                                                                                 cBaseAmount:=crBaseAmount,
                                                                                 cCurrencyAmount:=crCurrencyAmount,
                                                                                 vConversionDate:=dtCashListDate)
                            Amount = crCurrencyAmount
                        Else
                            Amount = crBaseAmount
                        End If
                    End If
                    'Set the currency_id of the transaction to the same as was selected on the cash list screen.
                    CurrencyID = nCashListCurrencyID
                End If
            End If

            If IsSplitReceipt AndAlso IsLeadAccount AndAlso ViaInsurerPayment AndAlso Not CollectionHasLead Then
                AccountID = InsurerAccountID
            End If

            If AccountID <> 0 Then
                uctAccountLookup.AccountId = AccountID
                'Ensure other properties are passed on
                uctAccountLookup_LostFocus(uctAccountLookup, Nothing)
                'DD 20/08/2003: Stop Address overwriting from Client
                m_sSaveAccountText = uctAccountLookup.Text
                'AR20050210 - PN18698/PN18699 Do not disable if editing without an account passed in
                If Not (m_iTask = PMEComponentAction.PMEdit AndAlso Not g_bHasAccountContext) Then
                    uctAccountLookup.Enabled = False
                    lblAccount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If
            Else
                uctAccountLookup.AccountId = 0
            End If

            If Amount < 0 Then
                Amount *= -1 'no negatives allowed
            End If
            ' Get the Allocation Status from the Lookup table using ID
            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupAllocationStatus,
                                                    iLookUpID:=AllocationStatusID,
                                                    sLookUpCaption:=sAllocationStatus)
            panAllocationStatus.Text = sAllocationStatus.Trim()
            'SW front Office Receipting Changes
            'summary tab
            m_lReturn = m_oFormFields.FormatControl(chkLetter, Letter)
            'PN30097
            txtTransDate.Text = gPMFunctions.FormatField(PMEFormatStyle.PMFormatDateLong, m_dtEffectiveDate)
            m_lReturn = m_oFormFields.FormatControl(txtMediaRef, MediaRef)
            m_lReturn = m_oFormFields.FormatControl(txtOurRef, OurRef)
            m_lReturn = m_oFormFields.FormatControl(txtTheirRef, TheirRef)
            m_lReturn = m_oFormFields.FormatControl(txtAmount, Amount)
            m_lReturn = m_oFormFields.FormatControl(txtTendered, Amounttendered)
            m_lReturn = m_oFormFields.FormatControl(txtChange, Change)
            m_lReturn = m_oFormFields.FormatControl(cmbMediaType, MediaTypeID)
            m_lReturn = m_oFormFields.FormatControl(cboReceiptType, Cashlistitemreceipttypeid)
            m_lReturn = m_oFormFields.FormatControl(TxtWoffAmt, WriteOffAmount)
            cboInsuranceRef.Text = InsuranceRef
            nLookUpID = MediaTypeID
            If nLookUpID <> 0 Then
                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                        iLookUpID:=nLookUpID,
                                                        sLookUpcode:=sLookUpcode)
            End If

            If sLookUpcode = ACMediaTypeCreditCard Then

                CCnumber = CCnumber.Trim()

                If CashlistTypeID = gACTLibrary.ACTCashListTypePayments OrElse
                    CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments Then
                    With uctPaymentCC
                        .CCName = CStr(CCname.Trim())
                        .CCNumber = CStr(CCnumber.Trim())
                        .CCExpiry = CStr(CStr(CCexpirydate).Trim())
                        .CCIssue = CStr(CCissue.Trim())
                        .CCPIN = CStr(CCpin.Trim())
                        .CCStart = CStr(CStr(CCstartdate).Trim())
                        .CCAutoAuthCode = CStr(CCauthcode.Trim())
                        .CCManualAuthCode = CStr(CCmanualauthcode.Trim())
                        .CCCustomerFlag = CStr(CCcustomer.Trim())
                        .CCTransactionCode = CStr(CCtransactioncode.Trim())
                        .MediaTypeID = MediaTypeID
                        .AccountID = AccountID
                        .MediaTypeIssuerID = MediaTypeIssuerID
                    End With
                    uctPartyBankCombo2.SelectedPaymentID = gPMFunctions.ToSafeInteger(m_vBankPaymentTypeId)
                Else
                    If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                        With uctReceiptCC
                            .CCName = CStr(CCname.Trim())
                            .CCNumber = CStr(CCnumber.Trim())
                            .CCExpiry = CStr(CStr(CCexpirydate).Trim())
                            .CCIssue = CStr(CCissue.Trim())
                            .CCPIN = CStr(CCpin.Trim())
                            .CCStart = CStr(CStr(CCstartdate).Trim())
                            .CCAutoAuthCode = CStr(CCauthcode.Trim())
                            .CCManualAuthCode = CStr(CCmanualauthcode.Trim())
                            .CCCustomerFlag = CStr(CCcustomer.Trim())
                            .CCTransactionCode = CStr(CCtransactioncode.Trim())
                            .MediaTypeID = MediaTypeID
                            .AccountID = AccountID
                            .MediaTypeIssuerID = MediaTypeIssuerID
                            'WPR12- Enhancement Quote Collection Process
                            .CCBankId = CCBankId
                            .CardTypeId = CardTypeId
                            .CardTransSlipNo = CardTransSlipNo
                        End With
                        uctPartyBankCombo2.SelectedPaymentID = gPMFunctions.ToSafeInteger(m_vBankPaymentTypeId)
                    End If
                End If
            Else
                uctPartyBankCombo1.SelectedPaymentID = gPMFunctions.ToSafeInteger(m_vBankPaymentTypeId)
                m_lReturn = m_oFormFields.FormatControl(txtName, PaymentName)
            End If

            m_lReturn = m_oFormFields.FormatControl(txtReverseDate, Stopconfirmationdate)
            If txtReverseDate.Text.Trim() = "00:00:00" Then txtReverseDate.Text = ""
            m_lReturn = m_oFormFields.FormatControl(cboBank, Cashlistitembankid)
            m_lReturn = m_oFormFields.FormatControl(cboReversalReason, CashListItemReverseReasonID)
            m_lReturn = m_oFormFields.FormatControl(txtReason, Reason)
            m_lReturn = m_oFormFields.FormatControl(txtChequeDate, Chequedate)
            If txtChequeDate.Text.Trim() = "00:00:00" Then txtChequeDate.Text = ""
            'WPR12- Enhancement Quote Collection Process
            m_lReturn = m_oFormFields.FormatControl(txtBankLocation, BankLocation)
            m_lReturn = m_oFormFields.FormatControl(cboChequeType, ChequeTypeId)
            m_lReturn = m_oFormFields.FormatControl(txtBankBranch, BBankBranch)
            m_lReturn = m_oFormFields.FormatControl(cboChequeClearingType, ChequeClearingTypeId)
            m_lReturn = m_oFormFields.FormatControl(txtCollectionDate, m_dCollectionDate)
            m_lReturn = m_oFormFields.FormatControl(txtComments, m_sComments)
            'payment tab
            m_lReturn = m_oFormFields.FormatControl(cboPaymentType, CashListItemPaymentTypeID)
            m_lReturn = m_oFormFields.FormatControl(txtPayeeName, PaymentName)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentAccountCode, PaymentAccountCode)
            m_lReturn = m_oFormFields.FormatControl(txtBIC, sBIC)
            m_lReturn = m_oFormFields.FormatControl(txtIBAN, sIBAN)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentBranchCode, PaymentBranchCode)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentExpiryDate, PaymentExpiryDate)
            If txtPaymentExpiryDate.Text.Trim() = "00:00:00" Or txtPaymentExpiryDate.Text.Trim() = DateTime.MinValue Then txtPaymentExpiryDate.Text = "" 'PN18882
            'PN18882
            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference1, PaymentReference1)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference2, PaymentReference2)
            m_lReturn = m_oFormFields.FormatControl(txtDatePresented, Datepresented)
            If txtDatePresented.Text.Trim() = "00:00:00" Then txtDatePresented.Text = ""
            m_lReturn = m_oFormFields.FormatControl(chkInPossession, Chequeinpossession)
            m_lReturn = m_oFormFields.FormatControl(txtStopRequested, Stoprequesteddate)
            If txtStopRequested.Text.Trim() = "00:00:00" Then txtStopRequested.Text = ""
            m_lReturn = m_oFormFields.FormatControl(txtConfirmation, Stopconfirmationdate)
            If txtConfirmation.Text.Trim() = "00:00:00" Then txtConfirmation.Text = ""
            m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, CashListItemPaymentStatusID)
            If Not Me.m_bReverseCashDrawerListItem Then
                m_lReturn = m_oFormFields.FormatControl(txtPaymentReason, Reason)
            End If

            'AR20050125 - PN18271
            If ViaInsurerPayment OrElse ViaClaimPayment OrElse ViaFinancePlan Then
                'Disable items for those have been preset
                If Amount <> 0 Then
                    txtAmount.Enabled = False
                    lblAmount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If

                If MediaTypeID > 0 Then
                    cmbMediaType.Enabled = False
                    lblMediaType.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If
                TxtWoffAmt.Enabled = False
                lblWoff.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            End If

            'AR20050125 - PN18271
            If ViaClaimPayment Then
                txtPayeeName.Enabled = False
                lblPayeeName.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtPaymentAccountCode.Enabled = False
                lblPaymentAccountCode.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtPaymentBranchCode.Enabled = False
                lblPaymentBranchCode.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtPaymentReason.Enabled = False
                lblCancellationReason.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtBIC.Enabled = False
                lblBIC.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                txtIBAN.Enabled = False
                lblBIC.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            End If

            AccountPropertiesToInterface()
            If m_bOptionUnderwritingYear Then
                cboUnderwritingYear.ItemId = gPMFunctions.ToSafeLong(UnderwritingYearID)
            End If

            If Not IsWOFF Then
                TxtWoffAmt.Visible = False
                lblWoff.Visible = False
            End If

            If IsSplitReceipt Then
                SplitReceiptMandatoryAndEnabledFieldsFields()
                If SplitTotal = CDec(0) Then
                    txtSplitTotal.Text = "0.00"
                Else
                    txtSplitTotal.Text = gPMFunctions.ToSafeDecimal(SplitTotal)
                End If
                txtMediaRef.Text = MediaRefLead
                cboReceiptType.Enabled = False
                txtName.Text = NameLead
            End If

            If (ViaInsurerPayment AndAlso txtTendered.Visible = False AndAlso m_bIsReinsurer AndAlso m_bIsRIPaymentAndRecoveries) OrElse
                (UCase(m_sCallingAppName) = "IPMWRKCOMPONENTSTARTER" AndAlso m_crTaxAmount > 0) Then

                m_lReturn = m_oFormFields.FormatControl(cboTaxBand, m_nTaxBandID)
                m_lReturn = m_oFormFields.FormatControl(txtTaxAmount, m_crTaxAmount)
                m_lReturn = m_oFormFields.FormatControl(txtAmount, Amount - m_crTaxAmount)

                lblTaxAmount.Visible = True
                lblTaxBand.Visible = True
                txtTaxAmount.Visible = True
                cboTaxBand.Visible = True
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayAdditionalFields
    '
    ' Description: gets and displays the additional fields when viewing a batch receipt.
    '
    ' Author: Steve Watton, 15/01/2003
    ' ***************************************************************** '
    Private Function DisplayAdditionalFields() As Integer

        Dim result As Integer = 0
        Dim sXML As String = ""
        Dim sElements() As String
        Dim lFieldCount, lUpperBound As Integer
        Dim objLbl As Label
        Dim objText As TextBox
        Dim iCapLength As Integer
        Dim sChar As String = ""
        Dim sCaption As New StringBuilder

        Const cLabelLeft As Integer = 240
        Const cFirstLabelTop As Integer = 405
        Const cLabelWidth As Integer = 3500

        Const cTopDiff As Integer = 480
        Const cControlHeight As Integer = 285
        Const cFirstTextTop As Integer = 360
        Const cTextWidth As Integer = 3000
        Const cTextLeft As Integer = 3500

        Const cBatchTab As Integer = 4

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iPtr As IntPtr = tabMainTab.Handle
            m_lReturn = m_oBusiness.GetAdditionalFields(CashlistitemID, sXML)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sXML.Trim().Length = 0 Then Return result

            sElements = sXML.Split(New String() {", "}, StringSplitOptions.None)

            lUpperBound = sElements.GetUpperBound(0)

            ReDim Preserve sElements(lUpperBound)

            'we now have a single dimension array which contains (label),(data),(label),(data).. etc

            'the number of fields to be displayed
            lFieldCount = CInt((lUpperBound - 1) / 2)

            For lCount As Integer = 0 To lFieldCount

                objLbl.Name = "lbl" & sElements(2 * lCount)
                fraAddFields.Controls.Add(objLbl)
                With objLbl
                    .Left = VB6.TwipsToPixelsX(cLabelLeft)
                    .Top = VB6.TwipsToPixelsY(cFirstLabelTop + (lCount * cTopDiff))
                    .Width = VB6.TwipsToPixelsX(cLabelWidth)
                    .Height = VB6.TwipsToPixelsY(cControlHeight)

                    iCapLength = sElements(2 * lCount).Trim().Length
                    sCaption = New StringBuilder("")

                    'ignore first character as this will be capital and do not want to replace
                    For lCount2 As Integer = 1 To iCapLength
                        sChar = Mid(sElements(2 * lCount), lCount2, 1)
                        If sChar = sChar.ToUpper() Then
                            sCaption.Append(" " & sChar)
                        Else
                            sCaption.Append(sChar)
                        End If
                    Next
                    'add colon
                    sCaption.Append(":")
                    'revert 'I D' back to ID
                    sCaption = New StringBuilder(sCaption.ToString().Replace("I D", "ID"))

                    .Text = sCaption.ToString().Trim()
                End With

                objLbl.Visible = True

                'set font to the same as another control on the form
                objLbl.Font = VB6.FontChangeName(objLbl.Font, txtName.Font.Name)

                objLbl = Nothing

                objText.Name = "txt" & sElements(2 * lCount)
                fraAddFields.Controls.Add(objText)

                With objText
                    .Left = VB6.TwipsToPixelsX(cTextLeft)
                    .Top = VB6.TwipsToPixelsY(cFirstTextTop + (lCount * cTopDiff))
                    .Width = VB6.TwipsToPixelsX(cTextWidth)
                    .Height = VB6.TwipsToPixelsY(cControlHeight)
                    .Text = sElements((2 * lCount) + 1)
                End With

                objText.Visible = True

                'set font to the same as other controls
                objText.Font = VB6.FontChangeName(objText.Font, txtName.Font.Name)

                objText = Nothing

            Next

            SSTabHelper.SetTabVisible(tabMainTab, cBatchTab, True)
            SSTabHelper.SetTabEnabled(tabMainTab, cBatchTab, True)

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the account properties", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAdditionalFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AccountPropertiesToInterface
    '
    ' Description: Updates the Properties from the interface details.
    '
    ' ***************************************************************** '
    Private Function AccountPropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AMENDED for front office receipting sw 15-10-2002

            'Address Tab

            m_lReturn = m_oFormFields.FormatControl(txtContactName, ContactName)
            uctAddress.AddressLine1 = Address1.Trim()
            uctAddress.AddressLine2 = Address2.Trim()
            uctAddress.AddressLine3 = Address3.Trim()
            uctAddress.AddressLine4 = Address4.Trim()
            uctAddress.PostCode = PostalCode.Trim()
            uctAddress.CountryId = AddressCountry

            m_lReturn = m_oFormFields.FormatControl(txtFurtherDetails, Receiptdetails)
            uctReceiptCC.CCName = ContactName.Trim()

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the account properties", vApp:=ACApp, vClass:=ACClass, vMethod:="AccountPropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' Updates the Properties from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InterfaceToProperties() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nLookUpID As Integer
        Dim sLookUpcode As String = ""
        Const kPaymentTab As Integer = 2
        Const kReceiptTab As Integer = 1

        Try

            Dim iPtr As IntPtr = tabMainTab.Handle
            AccountID = uctAccountLookup.AccountId
            MediaTypeID = VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)

            If cboIssuer.Visible Then
                MediaTypeIssuerID = VB6.GetItemData(cboIssuer, cboIssuer.SelectedIndex)
            Else
                MediaTypeIssuerID = 0
            End If

            ' WPR 51
            If chkIsLeadAccount.Checked = True And IsSplitReceipt Then
                IsLeadAccount = True
            Else
                IsLeadAccount = False
            End If

            'summary tab
            Letter = m_oFormFields.UnformatControl(chkLetter)
            Transactiondate = CDate(gPMFunctions.UnFormatField(PMEFormatStyle.PMFormatDateLong,
                                                               PMEFormatStyle.PMFormatDateTimeShort,
                                                               txtTransDate.Text))
            MediaRef = m_oFormFields.UnformatControl(txtMediaRef)
            OurRef = m_oFormFields.UnformatControl(txtOurRef)
            TheirRef = m_oFormFields.UnformatControl(txtTheirRef)
            If (CashListRoadmap = ACTInsurerPaymentRoadMap) Then
                Amount = m_oFormFields.UnformatControl(txtAmount) + ToSafeCurrency(txtTaxAmount.Text)
            Else
                Amount = m_oFormFields.UnformatControl(txtAmount)
            End If
            InsuranceRef = cboInsuranceRef.Text
            If (m_bIsInsurerePaymentRoadMap) OrElse
                (UCase(m_sCallingAppName) = "IPMWRKCOMPONENTSTARTER" AndAlso m_crTaxAmount > 0) Then
                Amount = m_oFormFields.UnformatControl(txtAmount) + ToSafeCurrency(txtTaxAmount.Text)
            End If

            Amounttendered = m_oFormFields.UnformatControl(txtTendered)
            txtChange.Text = CStr(CDec(txtTendered.Text) - CDec(txtAmount.Text))
            Change = m_oFormFields.UnformatControl(txtChange)
            ' Get the media type
            nLookUpID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)
            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                    iLookUpID:=nLookUpID,
                                                    sLookUpcode:=sLookUpcode)
            'receipt tab
            If SSTabHelper.GetTabVisible(tabMainTab, kReceiptTab) Then
                Cashlistitemreceipttypeid = VB6.GetItemData(cboReceiptType,
                                                            cboReceiptType.SelectedIndex)
                If sLookUpcode = ACMediaTypeCreditCard Then
                    With uctReceiptCC
                        PaymentName = .CCName
                        CCname = .CCName
                        CCnumber = .CCNumber
                        CCexpirydate = .CCExpiry
                        CCissue = .CCIssue
                        CCpin = .CCPIN
                        CCstartdate = .CCStart
                        CCauthcode = .CCAutoAuthCode
                        CCmanualauthcode = .CCManualAuthCode
                        CCcustomer = .CCCustomerFlag
                        CCtransactioncode = .CCTransactionCode
                        'WPR12- Enhancement Quote Collection Process
                        CCBankId = .CCBankId
                        CardTypeId = .CardTypeId
                        CardTransSlipNo = .CardTransSlipNo
                    End With
                    m_vBankPaymentTypeId = uctPartyBankCombo2.SelectedPaymentID
                Else
                    If Not SSTabHelper.GetTabVisible(tabMainTab, kPaymentTab) Then
                        PaymentName = m_oFormFields.UnformatControl(txtName)
                        If uctPartyBankCombo1.SelectedPaymentID > 0 Then
                            m_vBankPaymentTypeId = uctPartyBankCombo1.SelectedPaymentID
                        End If
                    End If
                End If

                If Information.IsDate(txtReverseDate.Text) Then
                    Stopconfirmationdate = m_oFormFields.UnformatControl(txtReverseDate)
                End If
                If Information.IsDate(txtChequeDate.Text) Then
                    Chequedate = m_oFormFields.UnformatControl(txtChequeDate)
                End If
                If cboBank.SelectedIndex <> -1 Then
                    Cashlistitembankid = m_oFormFields.UnformatControl(cboBank)
                Else
                    Cashlistitembankid = 0
                End If

                'WPR12- Enhancement Quote Collection Process
                BankLocation = m_oFormFields.UnformatControl(txtBankLocation)
                If cboChequeType.SelectedIndex <> -1 Then
                    ChequeTypeId = m_oFormFields.UnformatControl(cboChequeType)
                Else
                    ChequeTypeId = 0
                End If

                BBankBranch = m_oFormFields.UnformatControl(txtBankBranch)
                If cboChequeClearingType.SelectedIndex <> -1 Then
                    ChequeClearingTypeId = m_oFormFields.UnformatControl(cboChequeClearingType)
                Else
                    ChequeClearingTypeId = 0
                End If

                'ensure that an item is selected
                If cboReversalReason.SelectedIndex <> -1 Then
                    CashListItemReverseReasonID = m_oFormFields.UnformatControl(cboReversalReason)
                End If

                Reason = m_oFormFields.UnformatControl(txtReason)
                If txtCollectionDate.Visible Then
                    If Information.IsDate(txtCollectionDate.Text) Then
                        m_dCollectionDate = m_oFormFields.UnformatControl(txtCollectionDate)
                    End If
                    m_sComments = txtComments.Text
                End If
            End If

            'Address Tab
            ContactName = m_oFormFields.UnformatControl(txtContactName)
            Address1 = uctAddress.AddressLine1
            Address2 = uctAddress.AddressLine2
            Address3 = uctAddress.AddressLine3
            Address4 = uctAddress.AddressLine4
            PostalCode = uctAddress.PostCode
            AddressCountry = uctAddress.CountryId
            Receiptdetails = m_oFormFields.UnformatControl(txtFurtherDetails)

            'payment tab
            If SSTabHelper.GetTabVisible(tabMainTab, kPaymentTab) Then
                CashListItemPaymentStatusID = VB6.GetItemData(cboPaymentStatus, cboPaymentStatus.SelectedIndex)
                CashListItemPaymentTypeID = VB6.GetItemData(cboPaymentType, cboPaymentType.SelectedIndex)
                PaymentAccountCode = m_oFormFields.UnformatControl(txtPaymentAccountCode)
                sBIC = m_oFormFields.UnformatControl(txtBIC)
                sIBAN = m_oFormFields.UnformatControl(txtIBAN)
                PaymentBranchCode = m_oFormFields.UnformatControl(txtPaymentBranchCode)
                If Information.IsDate(txtPaymentExpiryDate.Text) Then 'PN18882
                    PaymentExpiryDate = m_oFormFields.UnformatControl(txtPaymentExpiryDate)
                End If
                PaymentReference1 = m_oFormFields.UnformatControl(txtPaymentReference1)
                PaymentReference2 = m_oFormFields.UnformatControl(txtPaymentReference2)
                Datepresented = m_oFormFields.UnformatControl(txtDatePresented)
                If CStr(Datepresented) = "" Then
                    Datepresented = 0
                End If
                Chequeinpossession = m_oFormFields.UnformatControl(chkInPossession)
                Stoprequesteddate = m_oFormFields.UnformatControl(txtStopRequested)
                If CStr(Stoprequesteddate) = "" Then
                    Stoprequesteddate = 0
                End If
                Stopconfirmationdate = m_oFormFields.UnformatControl(txtConfirmation)
                If CStr(Stopconfirmationdate) = "" Then
                    Stopconfirmationdate = 0
                End If
                Reason = m_oFormFields.UnformatControl(txtPaymentReason)
                If sLookUpcode = ACMediaTypeCreditCard Then
                    With uctPaymentCC
                        PaymentName = .CCName
                        CCname = .CCName
                        CCnumber = .CCNumber
                        CCexpirydate = .CCExpiry
                        CCissue = .CCIssue
                        CCpin = .CCPIN
                        CCstartdate = .CCStart
                        CCauthcode = .CCAutoAuthCode
                        CCmanualauthcode = .CCManualAuthCode
                        CCcustomer = .CCCustomerFlag
                        CCtransactioncode = .CCTransactionCode
                    End With
                    m_vBankPaymentTypeId = uctPartyBankCombo2.SelectedPaymentID
                Else
                    PaymentName = m_oFormFields.UnformatControl(txtPayeeName)
                    m_vBankPaymentTypeId = uctPartyBankCombo1.SelectedPaymentID
                End If
            End If

            If m_bOptionUnderwritingYear Then
                ' if this field is mandatory
                If m_bSystemOptionUWYearMandatory Then
                    ' use the value from the control
                    UnderwritingYearID = cboUnderwritingYear.ItemId
                Else
                    ' otherwise check first that the control is set
                    ' if its not use null
                    If cboUnderwritingYear.ListIndex = 0 Then
                        UnderwritingYearID = DBNull.Value
                    Else
                        UnderwritingYearID = cboUnderwritingYear.ItemId
                    End If
                End If
            Else
                UnderwritingYearID = DBNull.Value
            End If

            ' WPR 51
            SplitTotal = gPMFunctions.ToSafeDecimal(txtSplitTotal.Text)
            MediaRefLead = txtMediaRef.Text
            MediaTypeIDLead = cmbMediaType.SelectedIndex
            NameLead = txtName.Text
            If Information.IsDate(txtChequeDate.Text) Then
                ChequeDateLead = txtChequeDate.Text
            End If


            If cboTaxBand.SelectedIndex >= 0 Then
                If cboTaxBand.SelectedIndex > 0 Then
                    m_crTaxAmount = m_oFormFields.UnformatControl(txtTaxAmount)
                    m_nTaxBandID = VB6.GetItemData(cboTaxBand, cboTaxBand.SelectedIndex)
                Else
                    m_crTaxAmount = 0
                    m_nTaxBandID = 0
                End If
            Else
                m_crTaxAmount = 0
                m_nTaxBandID = 0
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to assign the form properties", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCashList As bACTCashList.Form
        Dim sBatchStatusDesc As String = ""
        Dim oFF As iPMFormControl.FormField
        Dim sValue As String = ""

        ' RDC 14112003 for system option AllowElectronicPayment
        Dim vValue As String = ""
        Dim sSystemOptionValue As String = ""

        Const kAdditionalTab As Integer = 4
        Const kPaymentTab As Integer = 2
        Const kReceiptTab As Integer = 1
        Const kAddressTab As Integer = 3
        Const kInstalmentTab As Integer = 5
        Const kBankGuaranteeTab As Integer = 6
        Try

            Dim iPtr As IntPtr = tabMainTab.Handle
            m_bInstalmentProcessing = False
            m_bRemovingItems = False

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
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
            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.
            VB6.SetDefault(cmdOK, True)

            'When Media Type.manual_payment = 1 then MediaRef field
            'must be editable regardless of Cheque Production setting
            'Is a MediaType selected?
            If cmbMediaType.SelectedIndex <> -1 Then
                'TR - Is the selected Media Type a Manual Payment Type?
                If GetIsManualPaymentFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                    txtMediaRef.Enabled = True
                    lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                    oFF = m_oFormFields.Item("txtMediaRef-0")
                    oFF.IsMandatory = True
                    lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
                Else
                    If g_bChequeProduction And ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then
                        txtMediaRef.Enabled = False
                        lblMediaRef.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    Else
                        txtMediaRef.Enabled = True
                        lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    End If
                End If

            Else
                'PN 31427 (RC)
                If g_bChequeProduction And (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    txtMediaRef.Enabled = False
                    lblMediaRef.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                Else
                    txtMediaRef.Enabled = True
                    lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                End If
            End If

            'front office receipting changes SW 09-10-2002
            'set some form defaults for a receeipt transaction type
            If CashlistTypeID = ACReceiptType Then
                txtTransDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=m_dtEffectiveDate)
                SSTabHelper.SetTabVisible(tabMainTab, kPaymentTab, False)
                SSTabHelper.SetTabEnabled(tabMainTab, kPaymentTab, False)
                SSTabHelper.SetTabVisible(tabMainTab, kReceiptTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kReceiptTab, True)
                SSTabHelper.SetTabVisible(tabMainTab, kAddressTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kAddressTab, True)

                'check for a valid cashlistitemID, if we have one display in the form caption
                If CashlistitemID <> 0 Then
                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        Me.Text = "View Receipt: " & CashlistitemID
                    End If
                End If
                lblReceiptType.Visible = True
                cboReceiptType.Visible = True
                lblPaymentType.Visible = False
                cboPaymentType.Visible = False
            Else
                txtTransDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=m_dtEffectiveDate)
                SSTabHelper.SetTabVisible(tabMainTab, kPaymentTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kPaymentTab, True)
                SSTabHelper.SetTabVisible(tabMainTab, kReceiptTab, False)
                SSTabHelper.SetTabEnabled(tabMainTab, kReceiptTab, False)
                SSTabHelper.SetTabVisible(tabMainTab, kInstalmentTab, False)
                SSTabHelper.SetTabEnabled(tabMainTab, kInstalmentTab, False)
                SSTabHelper.SetTabVisible(tabMainTab, kAddressTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kAddressTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kBankGuaranteeTab, False)
                'check for a valid cashlistitemID, if we have one display in the form caption
                If CashlistitemID <> 0 Then
                    'Should say View if in view mode
                    If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                        Me.Text = "View Payment: " & CashlistitemID
                    End If
                End If
                lblReceiptType.Visible = False
                cboReceiptType.Visible = False
                lblPaymentType.Visible = True
                cboPaymentType.Visible = True
            End If

            'display the cash drawer details if cash drawer type
            If CashDrawerID = 0 Then
                panCashDrawer.Text = ""
            Else
                panCashDrawer.Text = CashDrawerDescription
            End If

            If CDBatchID <> 0 Then

                'Create an instance of bACTCashList
                Dim temp_oCashList As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oCashList = temp_oCashList

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed to create instance of bACTCashList", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    ' Remove the instance.

                    oCashList.Dispose()
                    oCashList = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Getdetails

                m_lReturn = oCashList.GetBatchStatusDetails(CDBatchID, sBatchStatusDesc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get batch details for batchid " & CDBatchID, vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    ' Remove the instance.

                    oCashList.Dispose()
                    oCashList = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'now dispay the batch id and statuds desc in the display pannel
                panBatchNo.Text = CStr(CDBatchID) & ", " & sBatchStatusDesc

                SSTabHelper.SetTabVisible(tabMainTab, kAdditionalTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kAdditionalTab, True)
            Else
                panBatchNo.Text = ""
                SSTabHelper.SetTabVisible(tabMainTab, kAdditionalTab, False)
                SSTabHelper.SetTabEnabled(tabMainTab, kAdditionalTab, False)
            End If

            'Not used yet
            lblBatch.Visible = False
            panBatchNo.Visible = False
            lblCashDrawer.Visible = False
            panCashDrawer.Visible = False
            fraReceipt(2).Visible = False
            fraReceipt(3).Visible = False

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowElectronicPayment, 1, vValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bOptionElectronicPayment = (gPMFunctions.NullToString(vValue) = "1")

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bOptionUnderwritingYear = (gPMFunctions.NullToString(vValue) = "1")

            ' if the product option underwriting year is switched on
            If m_bOptionUnderwritingYear Then

                ' get the uw year mandatory system option
                m_lReturn = iPMFunc.GetSystemOption(kSystemOptionUWYearMandatory, sValue, g_oObjectManager.SourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption UWYear Mandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                    Return nResult
                End If

                ' get the system option for UW Year mandatory
                m_bSystemOptionUWYearMandatory = (sValue = "1")

            End If

            If m_bOptionUnderwritingYear Then
                If m_bSystemOptionUWYearMandatory Then
                    lblUnderwritingYear.Font = VB6.FontChangeBold(lblUnderwritingYear.Font, True)
                Else
                    lblUnderwritingYear.Font = VB6.FontChangeBold(lblUnderwritingYear.Font, False)
                End If
                lblUnderwritingYear.Visible = True
                cboUnderwritingYear.Visible = True
                SSTabHelper.SetTabVisible(tabMainTab, kAdditionalTab, True)
                SSTabHelper.SetTabEnabled(tabMainTab, kAdditionalTab, True)
            End If

            'PN28265 fix
            If m_sScreenType = "CLP" Then
                chkLetter.Enabled = False
            End If

            m_lReturn = m_oBusiness.CheckInsurerPaymentRoadMap(nCashListItemID:=CashlistitemID, r_bIsInsurerPaymentRoadMap:=m_bIsInsurerePaymentRoadMap)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInsurerPaymentRoadMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                Return nResult
            End If
            If m_bIsInsurerePaymentRoadMap Then
                cmdHelp.Enabled = True
            Else
                cmdHelp.Enabled = False
            End If

            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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

            m_ctlTabFirstLast = Array.CreateInstance(GetType(Control), New Integer() {ACControlEnd - ACControlStart + 1, TAB_ADDRESS - TAB_DETAILS + 1}, New Integer() {ACControlStart, TAB_DETAILS})

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************

            m_ctlTabFirstLast(ACControlStart, TAB_DETAILS) = cboReceiptType
            m_ctlTabFirstLast(ACControlEnd, TAB_DETAILS) = txtTendered
            m_ctlTabFirstLast(ACControlStart, TAB_RECEIPT) = txtName
            m_ctlTabFirstLast(ACControlEnd, TAB_RECEIPT) = txtReason
            m_ctlTabFirstLast(ACControlStart, TAB_ADDRESS) = txtContactName
            m_ctlTabFirstLast(ACControlEnd, TAB_ADDRESS) = txtFurtherDetails
            '   Set m_ctlTabFirstLast(ACControlStart, TAB_PAYMENT) = cboPaymentType
            m_ctlTabFirstLast(ACControlEnd, TAB_PAYMENT) = txtPaymentReason

            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                '        tabMainTab.TabEnabled(TAB_ADDRESS) = False
                '        tabMainTab.TabEnabled(TAB_PAYMENT) = False
                '        tabMainTab.TabVisible(TAB_ADDRESS) = False
                '        tabMainTab.TabVisible(TAB_PAYMENT) = False
                'EK091298
                'commented out these 2 lines SW front office receipting
                '        cmdNext(TAB_DETAILS).Visible = False
                '        cmdNext(TAB_ADDRESS).Visible = False
                '
            End If

            'EK091298
            '   cmdNext(TAB_DETAILS).Visible = False

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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

            cmdClaimDebt.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimDebtButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdSalvage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimSalvageButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Alix - 11/11/2003 - Tab labels are different for broking

            ' Underwriting

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDetailsTabTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Gets the interface details and sets the appropriate style.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInterfaceDetails() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nLookUpID As Integer
        Dim sLookUpMediaCode As String
        Dim sLookUpReceiptCode As String
        Dim sLookUpPayStatCode As String
        Dim bReceiptTypeIsInstalmentBased As Boolean
        Dim nPendingID As Integer
        Dim nIssuedID As Integer ' MEvans : 23-05-2003 : CQ 709
        Dim oTaxBandArray As Object(,)
        Dim nCount As Integer

        Try

            m_lReturn = m_oBusiness.GetTaxbandDetailForPaymentRecoveries(r_oResultArray:=oTaxBandArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Get Tax Band details Failed")
                Exit Function
            End If

            m_lReturn = GetReinsurerAndRIPaymentRecoveriesDetail(nAccountID:=AccountID)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                Return PMEReturnCode.PMFalse
            End If

            Dim nNewIndex As Integer = -1

            cboTaxBand.Items.Clear()
            nNewIndex = cboTaxBand.Items.Add("(none)")
            VB6.SetItemData(cboTaxBand, nNewIndex, 0)

            If IsArray(oTaxBandArray) Then
                For nCount = LBound(oTaxBandArray, 2) To UBound(oTaxBandArray, 2)
                    nNewIndex = cboTaxBand.Items.Add(oTaxBandArray(1, nCount))
                    VB6.SetItemData(cboTaxBand, nNewIndex, oTaxBandArray(0, nCount))
                Next
            End If


            Dim nPtr As IntPtr = tabMainTab.Handle
            ' Assign the details from the form properties to the interface.
            m_lReturn = PropertiesToInterface()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_bPopulatingLookUps = True
            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()
            m_bPopulatingLookUps = False
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If ViaInsurerPayment = True AndAlso Amount = 0 Then
                txtAmount.Enabled = False
                cboTaxBand.Enabled = False
                m_lReturn = SelectComboItem(r_ctl:=cboPaymentStatus,
                                            v_varID:=ACPaymentStatusIssued)
            End If

            If m_iTask = PMEComponentAction.PMEdit And CashlistitemID = 0 Then
                'set mandatory behavior for ref fields
                cmbMediaType_SelectedIndexChanged(cmbMediaType, New EventArgs())
            End If

            If m_bReverseCashDrawerListItem Then
                m_lReturn = DisableForm(lDisabled:=True)
                'now enable the reversal controls
                EnableReceiptFrames(ACReversalFrame, True)
                SSTabHelper.SetTabVisible(tabMainTab, TAB_INSTALMENT, False)
                SSTabHelper.SetTabEnabled(tabMainTab, TAB_INSTALMENT, False)
                cboReversalReason.Enabled = True
                txtReverseDate.Enabled = True
                txtReverseDate.Text = DateTime.Today
                txtReverseDate.Enabled = False
                txtReason.Enabled = True
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
            ElseIf CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                EnableReceiptFrames(ACReversalFrame, False)
                'check which media has been selected and enable correct details frame
                'first check that the default exists
                'sw front office receipting 22-11-2002
                With cmbMediaType
                    If .SelectedIndex <> -1 Then
                        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                                iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex),
                                                                sLookUpcode:=sLookUpMediaCode)
                    Else
                        sLookUpMediaCode = ""
                    End If
                End With

                'enable the receipt frames on tab2 according to media type
                Select Case sLookUpMediaCode
                    Case ACMediaTypeCreditCard
                        EnableReceiptFrames(ACCreditCardFrame, True)
                        EnableReceiptFrames(ACChequeFrame, False)
                    Case ACMediaTypeCash
                        EnableReceiptFrames(ACCreditCardFrame, False)
                        EnableReceiptFrames(ACChequeFrame, False)
                    Case ACMediaTypeBank, ACMediaTypeCheque
                        EnableReceiptFrames(ACCreditCardFrame, False)
                        EnableReceiptFrames(ACChequeFrame, True)
                    Case Else
                        EnableReceiptFrames(ACCreditCardFrame, False)
                        EnableReceiptFrames(ACChequeFrame, False)
                End Select

                If cboReceiptType.SelectedIndex <> -1 Then
                    nLookUpID = VB6.GetItemData(Me.cboReceiptType, Me.cboReceiptType.SelectedIndex)
                    m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=nLookUpID,
                                                                  r_sReceiptTypeAlias:=sLookUpReceiptCode,
                                                                  r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
                    If bReceiptTypeIsInstalmentBased Then
                        EnableReceiptFrames(ACClaimsFrame, False)
                        SSTabHelper.SetTabVisible(tabMainTab, TAB_INSTALMENT, True)
                        SSTabHelper.SetTabEnabled(tabMainTab, TAB_INSTALMENT, True)
                    Else
                        EnableReceiptFrames(ACClaimsFrame, False)
                        SSTabHelper.SetTabVisible(tabMainTab, TAB_INSTALMENT, False)
                        SSTabHelper.SetTabEnabled(tabMainTab, TAB_INSTALMENT, False)
                    End If
                    ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
                Else
                    EnableReceiptFrames(ACClaimsFrame, False)
                End If

                If ViaInsurerPayment AndAlso txtTendered.Visible = False AndAlso m_bIsReinsurer AndAlso m_bIsRIPaymentAndRecoveries Then
                    lblTaxBand.Visible = True
                    cboTaxBand.Visible = True
                    lblTaxAmount.Visible = True
                    txtTaxAmount.Visible = True
                End If

                SetMandatoryAndEnabledFields(sLookUpReceiptCode, sLookUpMediaCode)
                ' Signify to uctACTCreditCard to function in RECEIPT mode (as opposed to PAYMENT mode)
                With uctReceiptCC
                    'WPR12- Enhancement Quote Collection Process
                    If cmbMediaType.SelectedIndex <> -1 Then
                        .IsAdditionalDetailOption = GetIsAdditionalDetailsFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex))
                    Else
                        .IsAdditionalDetailOption = False
                    End If
                    .IsPayment = False
                    .Initialise()
                End With

                If (IsSplitReceipt = True) AndAlso (IsLeadAccount = False) Then
                    EnableReceiptFrames(ACCreditCardFrame, False)
                    EnableReceiptFrames(ACChequeFrame, False)
                End If

            ElseIf ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                    (CashlistTypeID = ACTCashListTypeClaimPayments)) Then
                'the payment status is read only
                cboPaymentStatus.Enabled = False
                lblTendered.Visible = False
                txtTendered.Visible = False
                lblChange.Visible = False
                txtChange.Visible = False
                lblCashDrawer.Visible = False
                panCashDrawer.Visible = False

                If (ViaInsurerPayment AndAlso txtTendered.Visible = False AndAlso m_bIsReinsurer = True AndAlso m_bIsRIPaymentAndRecoveries = True) OrElse
                    (UCase(m_sCallingAppName) = "IPMWRKCOMPONENTSTARTER" AndAlso m_crTaxAmount > 0) Then
                    lblTaxBand.Visible = True
                    cboTaxBand.Visible = True
                    lblTaxAmount.Visible = True
                    txtTaxAmount.Visible = True
                Else
                    'reposition the frames accordingly
                    Frame1.Height -= VB6.TwipsToPixelsY(450)
                    Frame1.Top += VB6.TwipsToPixelsY(450)
                    fraTransInfo.Top += VB6.TwipsToPixelsY(166)
                    fraTransInfo.Height += VB6.TwipsToPixelsY(50)
                    fraPostInfo.Top += VB6.TwipsToPixelsY(333)
                    fraPostInfo.Height += VB6.TwipsToPixelsY(50)
                    lblPaymentType.Top += VB6.TwipsToPixelsY(83)
                    cboPaymentType.Top += VB6.TwipsToPixelsY(83)
                    chkLetter.Top += VB6.TwipsToPixelsY(83)
                End If
                If CashlistitemID = 0 Then
                    'disable bank frame for new cashlistitems
                    fraBank.Enabled = False
                    lblDatePresented.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblStopRequested.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblConfirmation.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    chkInPossession.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblCancellationReason.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If

                ' Signify to uctACTCreditCard to function in PAYMENT mode (as opposed to RECEIPT mode)
                With uctPaymentCC
                    .IsPayment = True
                    fraPaymentCreditCard.Visible = False
                    .Initialise()
                End With

                ' Set the status of Payment as per Recommender
                If Trim(UCase(ForRecommendation)) = "F" Then
                    m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode("ISS", nIssuedID)
                    CashListItemPaymentStatusID = nIssuedID
                    m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus,
                         CashListItemPaymentStatusID)
                ElseIf Trim(UCase(ForRecommendation)) = "T" Then
                    m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode("PENDING", nPendingID)
                    CashListItemPaymentStatusID = nPendingID
                    m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus,
                        CashListItemPaymentStatusID)
                End If
            End If

            ' Check the task.
            'CHECK_PSL should it be disabled if edit cheque?
            If (m_iTask = PMEComponentAction.PMView) OrElse
                (m_iTask = PMEComponentAction.PMEdit AndAlso ActionKey = ACTEditCheque) OrElse
                (m_iTask = PMEComponentAction.PMEdit AndAlso ActionKey = ACTCancelCheque) OrElse
                (m_iTask = PMEComponentAction.PMEdit AndAlso ActionKey = ACTApprove) OrElse
                (m_iTask = PMEComponentAction.PMEdit AndAlso ActionKey = ACTStopCheque) Then

                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                End If

                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    'check which receipt type is being displayed
                    nLookUpID = VB6.GetItemData(Me.cboReceiptType, Me.cboReceiptType.SelectedIndex)

                    m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=nLookUpID,
                                                                  r_sReceiptTypeAlias:=sLookUpReceiptCode,
                                                                  r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)

                    'if receipt type is instalment then enable the instalment combo so that
                    'the user can view different instalment plans
                    If bReceiptTypeIsInstalmentBased Then
                        fraInstalments.Enabled = True
                        cboInstalment.Enabled = True
                        fralvInstalments.Enabled = False
                        GetInstalmentDetails()
                        DisplaySelectedInstalmentTotals()
                    End If
                    ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
                End If

                'sw payment maintenance 10-11-2002
                If ActionKey = ACTEditCheque Then
                    'we are displaying from the find form for payments then enable the
                    'media reference textbox and set focus
                    fraTransInfo.Enabled = True
                    txtMediaRef.Enabled = True
                    lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                ElseIf ActionKey = ACTCancelCheque Then
                    'we are displaying from the find form for payments then enable the
                    'reason textbox, change the caption and set focus
                    fraBank.Enabled = True
                    lblCancellationReason.Text = "Cancellation Reason:"
                    lblCancellationReason.Font = VB6.FontChangeBold(lblCancellationReason.Font, True)
                    txtPaymentReason.Enabled = True
                    chkInPossession.Enabled = True
                    lblStopRequested.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblConfirmation.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblDatePresented.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                ElseIf ActionKey = ACTStopCheque Then
                    'we are displaying from the find form for payments then enable the
                    'reason textbox, change the label caption and set focus
                    fraBank.Enabled = True
                    lblCancellationReason.Text = "Stop Reason:"
                    lblCancellationReason.Font = VB6.FontChangeBold(lblCancellationReason.Font, True)
                    cmdOK.Text = "Stop Cheque"
                    'now determine whether the item has already been stop requested
                    nLookUpID = VB6.GetItemData(Me.cboPaymentStatus, Me.cboPaymentStatus.SelectedIndex)
                    m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=ACTLookupPaymentStatus,
                                                            iLookUpID:=nLookUpID,
                                                            sLookUpcode:=sLookUpPayStatCode)
                    If sLookUpPayStatCode = ACStatusIssued Then
                        'no stop requested
                        txtPaymentReason.Enabled = True
                    ElseIf sLookUpPayStatCode = ACStatusStopRequested Then
                        'already stop requested
                        lblConfirmation.Font = VB6.FontChangeBold(lblConfirmation.Font, True)
                        txtConfirmation.Enabled = True
                        txtConfirmation.Text = DateTime.Today
                    End If
                ElseIf ActionKey = ACTApprove Then
                    ' MEvans : 22-05-2003 : CQ 709
                    ' get the payment status id for code "Issued"
                    m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode("ISS", nIssuedID)
                    ' The user shouldnt be able to approve issued items
                    ' so disable the approve button...
                    ' The user also shouldnt be able to decline issue items
                    ' so stop the update of the cancel button caption ( this removes the
                    ' decline functionality but lets us cancel out from the screen
                    ' This check purely to claims payment for now..
                    If m_lApprovalType = 1 And Me.CashListItemPaymentStatusID = nIssuedID Then
                        Me.cmdOK.Enabled = False
                    Else
                        cmdOK.Text = "Approve"
                        cmdCancel.Text = "Decline"
                        cmdHelp.Text = "View"
                    End If
                End If
            End If

            If m_iTask <> PMEComponentAction.PMView AndAlso
                (ActionKey <> ACTEditCheque) AndAlso
                (ActionKey <> ACTCancelCheque) AndAlso
                (ActionKey <> ACTStopCheque) AndAlso (ActionKey <> ACTApprove) Then
                cmbMediaType_Leave(cmbMediaType, New EventArgs())
            End If

            If IsSplitReceipt Then
                If Not IsLeadAccount Then
                    cmbMediaType.SelectedIndex = MediaTypeIDLead
                    txtChequeDate.Text = gPMFunctions.FormatField(iFormatType:=PMEFormatStyle.PMFormatDateLong,
                                                                  vFieldValue:=ChequeDateLead)
                    txtMediaRef.Text = MediaRefLead
                    txtName.Text = NameLead
                End If
                cboReceiptType.Enabled = False
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    '*************************************************************************
    'Name       : DisplayLookupDetails
    'Description: Displays all of the lookup details using the lookup
    '             values/details.
    'History    : TR060203 - Added filtering on MediaType combo based on
    '             whether this CashListItem is a Payment or a Receipt
    '*************************************************************************
    Private Function DisplayLookupDetails() As Integer
        Dim result As Integer = 0

        Const kReceiptType As String = "BANKING"

        Dim oCashDrawer As bACTCashListDrawer.Business
        Dim oCashList As bACTCashList.Form
        Dim vReceiptResultArray, vCDMediaTypes(,) As Object
        Dim lListTotal, lArrayTotal As Integer
        Dim bFound, bReceiptTypeIsInstalmentBased As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            m_lReturn = m_oListForm.GetLookupValues()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - TS220 - Only display Media Types supporting Payment OR Receipt
            Dim temp_oCashList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", gPMConstants.PMGetViaClientManager)
            oCashList = temp_oCashList
            'TR - Make sure that this worked
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Get the relevant Media Types based on Parent List Form's Type

            If m_oListForm.CashlistTypeID = gACTLibrary.ACTCashListTypePayments Or m_oListForm.CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments Then 'Payments
                'TR - Get the Media Types relevant for this Item

                m_lReturn = oCashList.GetPaymentMediaTypeIDs(m_vMediaResultArray)
            ElseIf m_oListForm.CashlistTypeID = 2 Then  'Receipts
                'TR - Get the Media Types relevant for this Item

                m_lReturn = oCashList.GetReceiptMediaTypeIDs(m_vMediaResultArray)
            End If
            'TR - Make sure that this worked
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vMediaResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Now populate the MediaType control
            For lCount As Integer = m_vMediaResultArray.GetLowerBound(1) To m_vMediaResultArray.GetUpperBound(1)
                'TR - Add ALL the Media Types in the array (as they have been
                'filtered by the Stored Procedure)
                With cmbMediaType
                    'TR - Add the Description

                    'TR - Add an Index
                    .Items.Add(New VB6.ListBoxItem(CStr(m_vMediaResultArray(ACMediaDescription, lCount)), CInt(m_vMediaResultArray(ACMediaTypeID, lCount))))
                End With
            Next lCount

            'part of a CashDrawer
            'SW Front office receipting changes
            If CashDrawerID <> 0 Then

                'we need to filter out specific media types for a given cash drawer
                Dim temp_oCashDrawer As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashDrawer, "bACTCashListDrawer.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oCashDrawer = temp_oCashDrawer

                m_lReturn = oCashDrawer.GetCashDrawerMediaTypes(CashDrawerID, vCDMediaTypes)

                lListTotal = cmbMediaType.Items.Count - 1

                lArrayTotal = vCDMediaTypes.GetUpperBound(1)

                'Filter out the media types that are not applicable for a given cash drawer
                For lCount As Integer = lListTotal To 0 Step -1
                    bFound = False
                    For lArrayCount As Integer = 0 To lArrayTotal

                        If VB6.GetItemData(cmbMediaType, lCount) = CInt(vCDMediaTypes(ACMediaTypeID, lArrayCount)) Then
                            bFound = True
                        End If
                    Next
                    If Not bFound Then
                        cmbMediaType.Items.RemoveAt(CShort(lCount))
                    End If
                Next
                oCashDrawer = Nothing
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'No default set so select nothing
            Dim sLookUpcode As String = ""
            If MediaTypeID = 0 Then
                cmbMediaType.SelectedIndex = -1
            Else

                m_lReturn = m_oFormFields.FormatControl(cmbMediaType, MediaTypeID)
                ' If the Media Type combo has a value AND we have an ID for the Issuer then...
                ' If Media is CC then load the Issuer combo so we can then position to the correct value
                If MediaTypeIssuerID <> 0 Then

                    With cmbMediaType

                        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode)
                    End With

                    If sLookUpcode = ACMediaTypeCreditCard Then

                        m_lReturn = LoadIssuerCombo()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadIssuerCombo failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
                            Return result
                        End If

                        m_lReturn = m_oFormFields.FormatControl(cboIssuer, MediaTypeIssuerID)

                    End If
                End If

            End If

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupReceiptType, ctlLookup:=cboReceiptType)

            If CashDrawerID <> 0 Then

                'we need to filter out specific receipt types for a given cash drawer
                Dim temp_oCashDrawer2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashDrawer2, "bACTCashListDrawer.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oCashDrawer = temp_oCashDrawer2

                m_lReturn = oCashDrawer.GetCashDrawerReceiptTypes(CashDrawerID, vReceiptResultArray)

                lListTotal = cboReceiptType.Items.Count - 1

                lArrayTotal = vReceiptResultArray.GetUpperBound(1)

                'Filter out the receipt types that are not applicable for a given cash drawer
                For lCount As Integer = lListTotal To 0 Step -1
                    bFound = False
                    For lArrayCount As Integer = 0 To lArrayTotal

                        If VB6.GetItemData(cboReceiptType, lCount) = CInt(vReceiptResultArray(0, lArrayCount)) Then
                            bFound = True
                        End If
                    Next
                    If Not bFound Then
                        cboReceiptType.Items.RemoveAt(CShort(lCount))
                    End If
                Next
                oCashDrawer = Nothing

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If this is a receipt via a roadmap or an Insurer Payment then do not show
            ' "Instalment Debt" in the Receipt Type combo  PN18415
            'PN: 61320, If coming via PayNow then remove items having "Is Installment" True
            If (m_lInsuranceFileCnt <> 0) Or (ViaInsurerPayment) Or m_sScreenType = "PAYNOW" Then

                For lCount As Integer = cboReceiptType.Items.Count - 1 To 0 Step -1

                    m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=VB6.GetItemData(cboReceiptType, lCount), r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
                    ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
                    If bReceiptTypeIsInstalmentBased Then
                        cboReceiptType.Items.RemoveAt(CShort(lCount))
                    End If
                Next

                If cboReceiptType.Items.Count > 0 And cboReceiptType.SelectedIndex < 0 Then
                    cboReceiptType.SelectedIndex = 0
                End If

            End If

            'no default set so select nothing
            If Me.Cashlistitemreceipttypeid = 0 Or cboReceiptType.Items.Count = 0 Then
                cboReceiptType.SelectedIndex = -1
            Else

                m_lReturn = m_oFormFields.FormatControl(cboReceiptType, Cashlistitemreceipttypeid)
            End If

            '    m_lReturn& = m_oListForm.GetLookupDetails( _
            ''        sLookupTable:=PMLookupCountry, _
            ''        ctlLookup:=cboAddressCountry)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupBankType, ctlLookup:=cboBank)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Cashlistitembankid <> 0 Then

                m_lReturn = m_oFormFields.FormatControl(cboBank, Cashlistitembankid)
            Else
                cboBank.SelectedIndex = -1
            End If

            'WPR12- Enhancement Quote Collection Process

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupChequeType, ctlLookup:=cboChequeType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ChequeTypeId > 0 Then

                m_lReturn = m_oFormFields.FormatControl(cboChequeType, ChequeTypeId)
            Else
                cboChequeType.SelectedIndex = -1
            End If
            If cboChequeType.Items.Count < 1 Then
                cboChequeType.Items.Clear()
            End If

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupChequeClearingType, ctlLookup:=cboChequeClearingType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ChequeClearingTypeId > 0 Then

                m_lReturn = m_oFormFields.FormatControl(cboChequeClearingType, ChequeClearingTypeId)
            Else
                cboChequeClearingType.SelectedIndex = -1
            End If
            If cboChequeClearingType.Items.Count < 1 Then
                cboChequeClearingType.Items.Clear()
            End If

            'check to see if we are dealing with a reversal, if so populate the look up details

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupReverseType, ctlLookup:=cboReversalReason)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CashListItemReverseReasonID <> 0 Then

                m_lReturn = m_oFormFields.FormatControl(cboReversalReason, CashListItemReverseReasonID)
            Else
                cboReversalReason.SelectedIndex = -1
            End If

            If m_sScreenType = "CLP" Then

                'Added, payment maintenance spec

                m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupPaymentTypeTable, ctlLookup:=cboPaymentType, v_sSelectedItemCode:="CLP")
            Else
                'Added, payment maintenance spec

                m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupPaymentTypeTable, ctlLookup:=cboPaymentType)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ViaBanking Then
                For lCount As Integer = 0 To cboReceiptType.Items.Count
                    If VB6.GetItemString(cboReceiptType, lCount).Trim().ToUpper() = kReceiptType Then
                        cboReceiptType.SelectedIndex = lCount
                        Exit For
                    End If
                Next lCount
            End If

            m_lReturn = m_oListForm.GetLookupDetails(sLookupTable:=gACTLibrary.ACTLookupPaymentStatus, ctlLookup:=cboPaymentStatus)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, CashListItemPaymentStatusID)
            ' AMB 24/02/2003: PS220 - set the payment type combo up also

            m_lReturn = m_oFormFields.FormatControl(cboPaymentType, CashListItemPaymentTypeID)

            'end of front office receipting changes sw 18-10-2002
            If m_sScreenType = "CLP" Then
                cboPaymentType.Enabled = False
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetIsValidationEnabledFromMediaArray
    '
    ' Description: determnes whether the IsValidationEnabled flag has been set
    ' for the given MediaTypeID
    '
    ' ***************************************************************** '

    Private Function GetIsValidationEnabledFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False

        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsValidationEnabled, iCount) <> "" AndAlso CInt(m_vMediaResultArray(ACIsValidationEnabled, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    m_bIsvalidationEnabled = True
                    Exit For
                Else
                    result = False
                    m_bIsvalidationEnabled = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetIsManualPaymentFromMediaArray
    '
    ' Description: determnes whether the IsPayment flag has been set
    ' for the given MediaTypeID
    '
    ' ***************************************************************** '

    Private Function GetIsManualPaymentFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False

        'TR - Is the selected Media Type a Manual Payment Type?
        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsManualPayment, iCount) <> "" AndAlso CInt(m_vMediaResultArray(ACIsManualPayment, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    Exit For
                Else
                    result = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetIsStoppableFromMediaArray
    '
    ' Description: determnes whether the IsStoppable flag has been set
    ' for the given MediaTypeCode
    '
    ' ***************************************************************** '

    'UPGRADE_NOTE: (7001) The following declaration (GetIsStoppableFromMediaArray) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetIsStoppableFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean
    '
    'Dim result As Boolean = False
    '
    'TR - Is the selected Media Type a Manual Payment Type?
    'For 'iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
    'If CDbl(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
    'If m_vMediaResultArray(ACIsStoppable, iCount) = gPMConstants.PMEReturnCode.PMTrue Then
    'result = True
    'Exit For
    'Else
    'result = False
    'Exit For
    'End If
    'End If
    'Next iCount
    '
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: GetIsBankingFromMediaArray
    '
    ' Description: determnes whether the IsBanking flag has been set
    ' for the given MediaTypeID
    '
    ' ***************************************************************** '

    Private Function GetIsBankingFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False

        'TR - Is the selected Media Type a Manual Payment Type?
        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsBanking, iCount) <> "" AndAlso CLng(m_vMediaResultArray(ACIsBanking, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    Exit For
                Else
                    result = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetIsMediaRefMandatoryFromMediaArray
    '
    ' Description: determnes whether the IsMediaRefMandatory flag has been set
    ' for the given MediaTypeID
    '
    ' ***************************************************************** '

    Private Function GetIsMediaRefMandatoryFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False

        'TR - Is the selected Media Type a Manual Payment Type?
        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsBanking, iCount) <> "" AndAlso CInt(m_vMediaResultArray(ACIsMediaRefMandatory, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    m_bIsMediaRefMandatory = True
                    Exit For
                Else
                    result = False
                    m_bIsMediaRefMandatory = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetIsPrintedAutomaticallyFromMediaArray
    '
    ' Description: determnes whether the IsMediaRefMandatory flag has been set
    ' for the given MediaTypeID
    '
    ' ***************************************************************** '

    Private Function GetIsPrintedAutomaticallyFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean

        Dim result As Boolean = False

        'TR - Is the selected Media Type printed automatically
        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsPrintedAutomatically, iCount) <> "" AndAlso CInt(m_vMediaResultArray(ACIsPrintedAutomatically, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    Exit For
                Else
                    result = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
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
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMReverse
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'jmf 11/9/2003 - unlock the salvage party id the dialog is cancelled
                        If g_lLockedSalvagePartyId <> 0 Then

                            m_lReturn = m_oBusiness.UnlockSalvageParty(g_lLockedSalvagePartyId)
                        End If

                    Else
                        ' Form hasn't been cancelled
                        ' Update the properties from the interface.
                        m_lReturn = InterfaceToProperties()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                            Return result
                        End If
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

    ' ***************************************************************** '
    ' Name: ProcessCommit
    '
    ' Description: Used to Commit all changes so far to the Database
    '              equivalent to OK on main form without exiting
    '
    ' ***************************************************************** '
    Private Function ProcessCommit() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the properties from the interface.
            m_lReturn = InterfaceToProperties()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommit")
                Return result
            End If

            ' Update the properties from the interface.

            m_lReturn = m_oListForm.DetailsCommit(iTaskType:=m_iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to commit the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommit")
                Return result
            End If

            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'TR - Pass the array back to bCLMDebtAllocate to confirm receipt
            'DD 19/08/2003: Added check around this as this function is also
            'called by standard Approve Payment
            If Information.IsArray(m_vUnsavedRBIItems) Or Information.IsArray(m_vReversedRBIItems) Then
                m_lReturn = ConfirmReceipt()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Confirm Receipt", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If

            ' Reset task to Edit
            m_iTask = gPMConstants.PMEComponentAction.PMEdit

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process commit", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is GroupBox) Then
                    If ctlFormControl.Name <> "fraReceipt" Then
                        ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                    End If
                End If
            Next ctlFormControl

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckChangeOfAccount
    '
    ' ***************************************************************** '
    Private Function CheckChangeOfAccount() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If AccountID <> uctAccountLookup.AccountId Then

                AccountID = uctAccountLookup.AccountId

                m_lReturn = m_oListForm.SetDefaultAccountProperties(AccountID, False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("m_oListForm.SetDefaultAccountProperties failed: " & m_lReturn, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = AccountPropertiesToInterface()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("AccountPropertiesToInterface failed: " & m_lReturn, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckChangeOfAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckChangeOfAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:     GetInstalmentDetails
    ' Author:   Steve Watton
    '
    ' Description:Gets instalment details for instalment receipt type
    '             and populates the list view tab.
    '             Accepts a plan ref to show specific details of a given plan, otehrwise it
    '             just shows the details of the first, with the option to change via the combo
    '
    'Added for front office receipting
    ' ***************************************************************** '

    Private Sub GetInstalmentDetails(Optional ByVal sPlanRef As String = "")

        Dim lRowCount, lListCount As Integer
        Dim oListItem As ListViewItem
        Dim bFound As Boolean
        Dim sTag As String = ""
        Dim iLookUpID As Integer
        Dim sLookUpcode As String = ""
        Dim iBaseCurrencyID As Integer
        Dim iTransCurrencyID As Integer
        Dim cCurrencyAmount As Decimal
        Dim sAmount As String = ""
        Dim bReceiptTypeIsInstalmentBased As Boolean
        Dim bInstalmentAlreadyAdded As Boolean
        Dim iCntr As Integer
        Dim lCashListItemsCount As Long
        Dim vInstalmentArray(,) As Object
        Dim sReceiptTypeCode As String = ""
        Try

            m_bInstalmentProcessing = True

            If cboReceiptType.SelectedIndex = -1 Then Exit Sub

            iLookUpID = VB6.GetItemData(Me.cboReceiptType, Me.cboReceiptType.SelectedIndex)

            m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=iLookUpID, r_sReceiptTypeCode:=sReceiptTypeCode, r_sReceiptTypeAlias:=sLookUpcode, r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)

            ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
            If Not bReceiptTypeIsInstalmentBased Or uctAccountLookup.AccountId = 0 Then
                'we do not have enough details to proceed
                m_lInstalmentAccountID = 0
                ClearInstalmentDetails()
                Exit Sub
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                'get the instalment details logged against the cashlistitem

                m_lReturn = m_oBusiness.SelectCashlistItemInstalments(CashlistitemID, InstalmentArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ClearInstalmentDetails()
                    MessageBox.Show("GetInstalmentDetails failed: " & gPMFunctions.ToSafeString(m_lReturn), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            Else
                If ViaInsurerPayment Then
                    InstalmentArray = InsurerPaymentInstArray

                    If Not IsArray(InstalmentArray) Then
                        MsgBox("No Instalment Plans exist for this client", vbOKOnly + vbExclamation,
                                "Warning: No plans available")
                        m_lInstalmentAccountID = 0
                        ClearInstalmentDetails()
                        Exit Sub
                    End If

                    lRowCount = UBound(InstalmentArray, 2)

                Else
                    If m_lInstalmentAccountID <> uctAccountLookup.AccountId Then
                        'account ID has changed, so we need to get new details

                        m_lReturn = m_oBusiness.GetInstalmentDetails(uctAccountLookup.AccountId, InstalmentArray, sReceiptTypeCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_lInstalmentAccountID = 0
                            ClearInstalmentDetails()
                            MessageBox.Show("GetInstalmentDetails failed: " & gPMFunctions.ToSafeString(m_lReturn), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If

                        If Not Information.IsArray(InstalmentArray) Then
                            MessageBox.Show("No Instalment Plans exist for this client", "Warning: No plans available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            m_lInstalmentAccountID = 0
                            ClearInstalmentDetails()
                            Exit Sub
                        End If

                        lRowCount = InstalmentArray.GetUpperBound(1)

                    End If
                End If
            End If

            lRowCount = InstalmentArray.GetUpperBound(1)

            m_lInstalmentAccountID = uctAccountLookup.AccountId

            'populate the instalment plan cbo with the distinct Plan names
            If False Or sPlanRef = "" Then
                For lRow As Integer = 0 To lRowCount
                    lListCount = cboInstalment.Items.Count

                    If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) <> "" Then
                        If lListCount = 0 Then

                            cboInstalment.Items.Add(CStr(InstalmentArray(ACInstalmentPlanRef, lRow)))
                        Else
                            'Loop through the listitems and see if the item already exists
                            bFound = False
                            For lCount As Integer = 0 To (lListCount - 1)

                                If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) = VB6.GetItemString(cboInstalment, lCount) Then
                                    bFound = True
                                End If
                            Next
                            If Not bFound Then
                                'item not found, so add the plan to the combo box

                                cboInstalment.Items.Add(CStr(InstalmentArray(ACInstalmentPlanRef, lRow)))
                            End If
                        End If
                    End If
                Next
                cboInstalment.SelectedIndex = 0
            End If

            'Create currency convert object if it hasn't been already.
            m_lReturn = CreateCurrencyConvert()

            'Get the Company's base currency

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=CompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lInstalmentAccountID = 0
                ClearInstalmentDetails()
                Exit Sub
            End If

            lvInstalments.Items.Clear()
            For lRow As Integer = 0 To lRowCount

                If cboInstalment.Text = CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) Then

                    ' RAW 06/03/2003 : do not display instalment number 0 - added if test

                    lCashListItemsCount = m_oBusiness.Details.Count

                    bInstalmentAlreadyAdded = False

                    For iCntr = 1 To lCashListItemsCount
                        If (m_oBusiness.Details.Item(iCntr).DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Or m_oBusiness.Details.Item(iCntr).DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit) And m_oBusiness.Details.Item(iCntr).CashListItem_receipt_type_id = 2 Then 'CashListItem_receipt_type_id condition added to loop thourgh for instalment type only

                            vInstalmentArray = InstalmentArray

                            For lCount As Integer = 0 To UBound(vInstalmentArray, 2)
                                'PN59252 : Check whether the Instalment has already been added into another cashlistitem or not
                                If ToSafeLong(vInstalmentArray(ACInstalmentFlagElement, lCount)) = gPMConstants.PMEReturnCode.PMTrue And vInstalmentArray(ACInstalmentPlanRef, lCount) = InstalmentArray(ACInstalmentPlanRef, lRow) And vInstalmentArray(ACInstalmentNumber, lCount) = InstalmentArray(ACInstalmentNumber, lRow) Then
                                    bInstalmentAlreadyAdded = True
                                    Exit For
                                End If
                            Next

                        End If

                        If bInstalmentAlreadyAdded Then
                            Exit For
                        End If
                    Next
                    If (CDbl(InstalmentArray(ACInstalmentNumber, lRow)) = 0 And gPMFunctions.ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow), 0) <> 0 And (CDbl(InstalmentArray(ACTransactionCode, lRow)) <> PFTransactionCreate Or CDbl(InstalmentArray(ACTransactionCode, lRow)) <> PFTransactionCancel)) Or (CDbl(InstalmentArray(ACInstalmentNumber, lRow)) <> 0 And bInstalmentAlreadyAdded = False) Or CDbl(InstalmentArray(ACTransactionCode, lRow)) = PFTransactionDeposit Then
                        'If (gPMFunctions.ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow), 0) <> 0 And (CDbl(InstalmentArray(ACTransactionCode, lRow)) <> PFTransactionCreate Or CDbl(InstalmentArray(ACTransactionCode, lRow)) <> PFTransactionCancel)) And Not bInstalmentAlreadyAdded Then
                        m_bAddingItems = True
                        oListItem = lvInstalments.Items.Add("")

                        m_bAddingItems = False
                        'this will enable us to locate the position of the listitem in the array
                        oListItem.Tag = CStr(lRow)
                        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                                'the list is read only and only contains previously selected items
                                oListItem.Checked = False
                            Else

                                oListItem.Checked = gPMFunctions.ToSafeBoolean(InstalmentArray(ACInstalmentFlagElement, lRow))
                            End If

                            If CDbl(InstalmentArray(ACTransactionCode, lRow)) = PFTransactionDeposit Then
                                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Deposit"
                            ElseIf InstalmentArray(ACInstalmentNumber, lRow) <> -1 Then

                                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(InstalmentArray(ACInstalmentNumber, lRow))
                            Else
                                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
                            End If

                            If InstalmentArray(ACInstalmentDueDate, lRow) <> "" Then
                                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(InstalmentArray(ACInstalmentDueDate, lRow))
                            End If
                            If InstalmentArray(ACTransCurrencyCode, lRow) <> "" Then
                                iTransCurrencyID = ToSafeInteger(InstalmentArray(ACTransCurrencyCode, lRow))
                            End If

                            Dim iUseTransCurrency As Integer = ToSafeInteger(InstalmentArray(ACUseTransCurrencyInPF, lRow))
                            If iTransCurrencyID > 0 Then

                                If iUseTransCurrency = 1 Then
                                    iBaseCurrencyID = iTransCurrencyID
                                End If
                            End If

                            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=iBaseCurrencyID, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(InstalmentArray(ACInstalmentAmount, lRow)), vFormattedCurrency:=sAmount)

                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sAmount

                            If CurrencyID <> iBaseCurrencyID Then

                                m_lReturn = m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=iBaseCurrencyID, v_crCurrencyAmountFrom:=gPMFunctions.ToSafeCurrency(InstalmentArray(ACInstalmentAmount, lRow)),
                                                                    v_lCompanyId:=CompanyID, v_lCurrencyIdTo:=CurrencyID, r_crCurrencyAmountTo:=cCurrencyAmount, dt_EffectiveDate:=Transactiondate)
                                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=CurrencyID, vCurrencyAmount:=cCurrencyAmount, vFormattedCurrency:=sAmount)

                                InstalmentArray(ACInstalmentReceiptAmount, lRow) = cCurrencyAmount
                                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sAmount
                            Else

                                m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=CurrencyID, vCurrencyAmount:=gPMFunctions.ToSafeCurrency(InstalmentArray(ACInstalmentAmount, lRow)), vFormattedCurrency:=sAmount)
                                InstalmentArray(ACInstalmentReceiptAmount, lRow) = gPMFunctions.ToSafeCurrency(InstalmentArray(ACInstalmentAmount, lRow))
                                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = sAmount
                            End If
                        End If
                    End If

            Next

            PlanRef = cboInstalment.Text
            If CurrencyID = iBaseCurrencyID Then
                lvInstalments.Columns.Item(4).Width = CInt(0)
            Else
                lvInstalments.Columns.Item(4).Width = CInt(131)
            End If

            m_bInstalmentProcessing = False
            If ViaInsurerPayment Then
                DisplaySelectedInstalmentTotals()
            End If
        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalmentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'sw front office receipting
    'clear out the instalment details held in memory
    Private Sub ClearInstalmentDetails()

        If Information.IsArray(InstalmentArray) Then

            InstalmentArray = Nothing
        End If

        If lvInstalments.Items.Count > 0 Then
            'MKW030204 PN10136 Do Not Reset for PFCASH Roadmap
            If CashListRoadmap <> "PFCASH" Then
                txtAmount.Text = "0"
            End If
            txtTendered.Text = "0"
            txtChange.Text = "0"
            txtAmount_Leave(txtAmount, New EventArgs())
            txtTendered_Leave(txtTendered, New EventArgs())
            txtChange_Leave(txtChange, New EventArgs())
        End If

        lvInstalments.Items.Clear()
        cboInstalment.Items.Clear()
        m_bInstalmentProcessing = False
        SSThisPlanTotal.Text = "0.00"
        Me.SSOveralPlanTotal.Text = "0.00"

        m_lInstalmentAccountID = 0

    End Sub

    'WPR12- Enhancement Quote Collection Process
    Private Sub cboChequeClearingType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboChequeClearingType.Enter

        m_lReturn = m_oFormFields.GotFocus(cboChequeClearingType)
    End Sub

    Private Sub cboChequeClearingType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboChequeClearingType.Leave

        m_lReturn = m_oFormFields.LostFocus(cboChequeClearingType)
    End Sub

    'WPR12- Enhancement Quote Collection Process
    Private Sub cboChequeType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboChequeType.Enter

        m_lReturn = m_oFormFields.GotFocus(cboChequeType)
    End Sub

    Private Sub cboChequeType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboChequeType.Leave

        m_lReturn = m_oFormFields.LostFocus(cboChequeType)
    End Sub

    Private Sub cboInstalment_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboInstalment.SelectionChangeCommitted

        If Not m_bInstalmentProcessing Then
            GetInstalmentDetails(cboInstalment.Text)
            DisplaySelectedInstalmentTotals()
        End If

    End Sub

    Private Sub cboIssuer_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboIssuer.Enter
        m_sSaveIssuerText = cboIssuer.Text
    End Sub

    Private Sub cboIssuer_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboIssuer.Leave

        Dim lIssuerID As Integer

        ' If Issuer has changed...
        If cboIssuer.Text <> m_sSaveIssuerText Then
            ' For credit card payments/receipts we must let uctACTCreditCard immediately know of
            ' any changes to the Issuer as it will effect display and validation of the control.
            If cboIssuer.SelectedIndex = -1 Then
                lIssuerID = 0
            Else
                lIssuerID = VB6.GetItemData(cboIssuer, cboIssuer.SelectedIndex)
            End If

            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                uctReceiptCC.MediaTypeIssuerID = lIssuerID
            Else
                If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    uctPaymentCC.MediaTypeIssuerID = lIssuerID
                End If
            End If

            m_sSaveIssuerText = cboIssuer.Text

        End If

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub cboPaymentType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentType.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If cboPaymentType.Text = "BankGuarantee" Then
            cmdOK.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cboPaymentType_SelectedIndexChanged(ByVal eventSender As Object,
                                                    ByVal eventArgs As EventArgs) Handles cboPaymentType.SelectedIndexChanged

        Dim nIssuedID As Integer = 0
        Dim oOptionValue As Object = Nothing
        Dim oFF As iPMFormControl.FormField 'DC040804 PN13889
        Dim sLookUpcode As String = ""

        'ICB - 52
        Dim oStepAuthorization As Object
        Dim bPaymentStatus As Boolean

        m_lReturn = g_oObjectManager.GetInstance(oStepAuthorization, "bACTCashlistitem.StepAuthorization", vInstanceManager:=gPMConstants.PMGetViaClientManager)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="failed to Create StepAuthorization Class.", vApp:=ACApp, vClass:=ACClass, vMethod:="cboPaymentType_SelectedIndexChanged")
            Exit Sub
        End If

        oStepAuthorization.PaymentType = ACPaymentsType

        oStepAuthorization.PaymentID = CashlistitemID

        oStepAuthorization.PaymentAmount = Amount

        oStepAuthorization.PaymentCreatorUserID = PMUserid

        m_lReturn = oStepAuthorization.CheckPaymentStepStatus(bPaymentStatus)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPaymentStepStatus failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cboPaymentType_SelectedIndexChanged")
            Exit Sub
        End If

        Dim bIsIncludePaymentTypeClaimPayment As Boolean = False
        Dim aoResultArray(,) As Object = Nothing
        Dim nPos As Integer = 0

        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                  v_vBranch:=g_oObjectManager.SourceID,
                                                  r_vUnderwriting:=oOptionValue)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="Failed to process GetProductOptionValue.",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="cboPaymentType_Click")
            Exit Sub
        End If

        Dim sIncludeInsurerPaymentMultiStep As String
        If oOptionValue = "1" AndAlso CashListRoadmap = ACTInsurerPaymentRoadMap Then
            m_lReturn = GetOption(v_iOptionNumber:=kSysOptIncludeInsurerPaymentMultiStep,
                                  r_sOptionValue:=sIncludeInsurerPaymentMultiStep)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                        sMsg:="Failed to process GetOption.",
                        vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click",
                        vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Sub
            End If
        End If

        m_lReturn = g_oObjectManager.GetInstance(
                   oObject:=m_oCashlistitem,
                   sClassName:="bACTCashlistitem.StepAuthorization",
                   vInstanceManager:=PMGetViaClientManager)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("cboPaymentType_Click", "Failed to create instance of Cashlistitem")
        End If

        nApprovalSteps = 1

        If CashlistTypeID = ACTCashListTypeClaimPayments Then
            m_oCashlistitem.PaymentType = 2
        End If

        m_lReturn = m_oCashlistitem.GetStepDetails(
                                v_lApprovalStep:=nApprovalSteps,
                                r_vStepDetails:=aoResultArray)

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

        'PN64920 If it comes from Insurer Payment RoadMap then ignore Multistep approval
        If gPMFunctions.NullToString(oOptionValue) = "1" AndAlso
            (CashListRoadmap <> ACTInsurerPaymentRoadMap OrElse sIncludeInsurerPaymentMultiStep = "1") Then
            ' We have multi-step approval then set the status to Pending
            If (bIsIncludePaymentTypeClaimPayment = True And CashlistTypeID = ACTCashListTypeClaimPayments) Or CashlistTypeID <> ACTCashListTypeClaimPayments Then
                ' We have multi-step approval then set the status to Pending
                m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode(ACStatusPending, nIssuedID)
            Else
                'set the status of the payment to issued
                m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode(ACStatusIssued, nIssuedID)
            End If

        Else
            'set the status of the payment to issued
            m_lReturn = m_oBusiness.GetPaymentStatusIDFromCode(ACStatusIssued, nIssuedID)
        End If
        ' END CHANGES - Changed By: AAB  - Changed On: 02-Dec-2003 12:22

        If m_lReturn = PMEReturnCode.PMTrue Then
            CashListItemPaymentStatusID = nIssuedID
            m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, CashListItemPaymentStatusID)
        End If

        'DC040804 PN13889 make Amount bold & mandatory
        If Not m_bAmountDisabled Then
            txtAmount.Enabled = True
            oFF = m_oFormFields.Item("txtAmount-0")
            oFF.IsMandatory = True
            lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, True)
        End If

        ' If MediaType has a value of CC already then we need to reload the Issuer combo as whether the Payment
        ' Type is a Claim or not will effect the contents
        If Not m_bPopulatingLookUps Then
            If cmbMediaType.SelectedIndex <> -1 Then
                With cmbMediaType
                    m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                            iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex),
                                                            sLookUpcode:=sLookUpcode)
                End With

                If sLookUpcode = ACMediaTypeCreditCard Then
                    m_lReturn = LoadIssuerCombo()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="LoadIssuerCombo failed.",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="cboPaymentType_Click")
                        Exit Sub
                    End If
                End If
            End If
        End If
        m_oCashlistitem.Dispose()
        m_oCashlistitem = Nothing
    End Sub

    Private Sub cboReceiptType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReceiptType.SelectedIndexChanged
        Dim sLookUpcode, sLookUpcode2 As String
        Dim oFF As iPMFormControl.FormField
        Dim bReceiptTypeIsInstalmentBased As Boolean

        Dim iPtr As IntPtr = tabMainTab.Handle

        If cboReceiptType.Text = "Bank Guarantee Debt" Then
            SSTabHelper.SetTabVisible(tabMainTab, TAB_BANKGUARANTEE, True)
        Else
            SSTabHelper.SetTabVisible(tabMainTab, TAB_BANKGUARANTEE, False)
        End If

        If cboReceiptType.SelectedIndex = -1 Or m_bPopulatingLookUps Then Exit Sub

        Dim iLookUpID As Integer = VB6.GetItemData(Me.cboReceiptType, Me.cboReceiptType.SelectedIndex)

        m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=iLookUpID, r_sReceiptTypeAlias:=sLookUpcode, r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
        ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
        'DC270904 PN15193 -start -this is to ensure mediacode set correctly
        If cmbMediaType.SelectedIndex = -1 Or m_bPopulatingLookUps Then
            sLookUpcode2 = ""
        Else

            With cmbMediaType

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode2)
            End With

        End If
        'DC270904 PN15193 -end

        If Not m_bReverseCashDrawerListItem Then
            If cmbMediaType.SelectedIndex <> -1 Then
                SetMandatoryAndEnabledFields(sReceiptCode:=sLookUpcode, sMediaCode:=sLookUpcode2)
            Else
                SetMandatoryAndEnabledFields(sReceiptCode:=sLookUpcode)
            End If
        End If

        If bReceiptTypeIsInstalmentBased Then
            SSTabHelper.SetTabVisible(tabMainTab, TAB_INSTALMENT, True)
            SSTabHelper.SetTabEnabled(tabMainTab, TAB_INSTALMENT, True)
            'AAB-20-Aug-2003 09:17
            ClearInstalmentDetails()
            GetInstalmentDetails()

            txtAmount.Enabled = True
            lblAmount.ForeColor = Color.Black

            oFF = m_oFormFields.Item("txtAmount-0")
            'DC290704 PN11262 must enter amount if not via Insurer Payment
            If ViaInsurerPayment Then
                oFF.IsMandatory = False
                lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, False)
            Else
                oFF.IsMandatory = True
                lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, True)
            End If
            txtAmount_Leave(txtAmount, New EventArgs())
        Else
            ClearInstalmentDetails()
            SSTabHelper.SetTabVisible(Me.tabMainTab, TAB_INSTALMENT, False)
            SSTabHelper.SetTabEnabled(Me.tabMainTab, TAB_INSTALMENT, False)

            oFF = m_oFormFields.Item("txtAmount-0")

            'AR20050125 - PN18271
            If ViaInsurerPayment Or ViaClaimPayment Then
                oFF.IsMandatory = False
                lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, False)
            Else
                oFF.IsMandatory = True
                lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, True)
            End If

            txtAmount_Leave(txtAmount, New EventArgs())

            'Never enable this for insurer payment
            'If (ViaInsurerPayment Or ViaClaimPayment) Or m_bAmountDisabled Then
            If (ViaInsurerPayment Or ViaClaimPayment Or ViaFinancePlan) Or m_bAmountDisabled Then
                txtAmount.Enabled = False
                lblAmount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                TxtWoffAmt.Enabled = False
                lblWoff.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
            Else
                txtAmount.Enabled = True
                lblAmount.ForeColor = Color.Black
            End If
        End If

        If cboReceiptType.Text = "Bank Guarantee Debt" Then
            cmdOK.Enabled = False
        Else
            cmdOK.Enabled = True
        End If
    End Sub

    Private Sub cboReversalReason_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReversalReason.SelectionChangeCommitted

        If m_bReverseCashDrawerListItem And cboReversalReason.SelectedIndex <> -1 Then
            SetMandatoryAndEnabledFields(sReceiptCode:=ACReversalOfAnyType)
        End If

    End Sub

    Private Sub cmbMediaType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbMediaType.SelectedIndexChanged
        Dim sLookUpcode As String = ""
        '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
        Dim lMediaTypeIndex As Integer
        Const kMethodName As String = "cmbMediaType"

        Dim oPlicyNumMaint As bSIRPolicyNumMaint.Business
        Dim sMediaRef As String = ""

        'If m_bPopulatingLookUps Then Exit Sub

        If cmbMediaType.SelectedIndex = -1 Then
            lblIssuer.Visible = False
            cboIssuer.Visible = False
            Exit Sub
        End If

        With cmbMediaType

            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode)
        End With

        For lCount As Integer = m_vMediaResultArray.GetLowerBound(1) To m_vMediaResultArray.GetUpperBound(1)
            If VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex) = CDbl(m_vMediaResultArray(ACMediaTypeID, lCount)) Then
                lMediaTypeIndex = lCount
                Exit For
            End If
        Next lCount
        GetIsMediaRefMandatoryFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex))
        If CStr(m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex)) <> "" AndAlso CDbl(m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex)) <> 0 Then

            Dim temp_oPlicyNumMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPlicyNumMaint, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPlicyNumMaint = temp_oPlicyNumMaint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initilization of the Component bSIRPolicyNumMaint.Business ", gPMConstants.PMELogLevel.PMLogOnError)
            End If

            m_lReturn = oPlicyNumMaint.GenerateMediaReference(v_iSourceID:=g_iSourceID, v_iNumberingScheme:=m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex), r_sGeneratedMediaRef:=sMediaRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "The method GenerateMediaReference calling failed", gPMConstants.PMELogLevel.PMLogOnError)
            End If
            'Start PN: 62476
            If MediaTypeID = 13 And Task = gPMConstants.PMEComponentAction.PMEdit Then
                txtMediaRef.Text = MediaRef
            Else
                txtMediaRef.Text = sMediaRef
            End If
            'End PN: 62476

            txtMediaRef.ReadOnly = True
            ''Start(Saurabh) PN59044
            m_lReturn = CheckElectronicPayments()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initilization of the Component bSIRPolicyNumMaint.Business ", gPMConstants.PMELogLevel.PMLogOnError)
            End If
            ''End(Saurabh) PN59044

        ElseIf lsFormInitializing = False And Not IsSplitReceipt Then

            'txtMediaRef.Text = ""
            txtMediaRef.ReadOnly = False

        End If
        '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)

        If Not m_bReverseCashDrawerListItem Then
            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            SetMandatoryAndEnabledFields(sMediaCode:=CStr(m_vMediaResultArray(ACMediaCode, lMediaTypeIndex)))
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
        End If

        ' RDC 14112003 check again else it will bomb on the GetIsManualPaymentFromMediaArray() call
        If cmbMediaType.SelectedIndex = -1 Or m_bPopulatingLookUps Then
            Exit Sub
        End If
        '   m_lReturn = RefreshAccountDetails
        '    If m_lReturn <> PMTrue Then
        '        RaiseError "RefreshAccountDetails", "fail to call RefreshAccountDetails"
        '    End If

        ' If Credit Card selected (and was not before) then show cboIssuer if there are any issuers
        If m_sSaveMediaTypeText <> cmbMediaType.Text Then
            If sLookUpcode = ACMediaTypeCreditCard Then

                m_lReturn = LoadIssuerCombo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadIssuerCombo failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmbMediaType_Click")
                    Exit Sub
                End If

            Else
                cboIssuer.Visible = False
                lblIssuer.Visible = False

                ' For credit card payments/receipts we must let uctACTCreditCard immediately know of
                ' any changes to the Issuer as it will effect display and validation of the control.
                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    uctReceiptCC.MediaTypeIssuerID = 0
                Else
                    If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                        uctPaymentCC.MediaTypeIssuerID = 0
                        fraPaymentCreditCard.Visible = False
                        fraBank.Visible = True
                        fraPayee.Visible = True
                    End If
                End If

            End If
        End If

        'TR - Is the selected Media Type a Manual Payment Type?
        If GetIsManualPaymentFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
            txtMediaRef.Enabled = True
            lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
            '    Else
            '        If g_bChequeProduction = True And _
            ''            ((CashlistTypeID = ACTCashListTypePayments) Or _
            ''                (CashlistTypeID = ACTCashListTypeClaimPayments)) Then
            '            If sLookUpcode <> ACMediaTypeBank Then
            '                txtMediaRef.Enabled = True
            '                lblMediaRef.ForeColor = ACEnabledColor
            '            Else
            '                txtMediaRef.Enabled = False
            '                lblMediaRef.ForeColor = ACDisabledColor
            '            End If
        ElseIf m_bIsMediaRefMandatory Then
            txtMediaRef.Enabled = True
            lblMediaRef.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
        ElseIf Not m_bIsMediaRefMandatory Then
            'PN 31427 (RC)
            '    'PN 30097 As media ref is needed in every case
            ''        txtMediaRef.Enabled = False
            ''        lblMediaRef.ForeColor = ACDisabledColor
            ''        txtMediaRef.Text = ""
        End If

        '----- NOT USED YET (1.9)
        'SMJB CQ1210 04/08/03: If the payment type is manual then the payment status
        'should go straight to Issued otherwise it should be Pending Approval#
        'DD 11/09/2003: Calls from the Insurer/Agent/Tax Payment form are already
        'authorised so they should be issued
        'If CashlistTypeID = ACTCashListTypePayments Then
        '    If _
        ''            GetIsManualPaymentFromMediaArray(cmbMediaType.ItemData(cmbMediaType.ListIndex)) _
        ''            = _
        ''            True Or CashListRoadmap = ACTInsurerPaymentRoadMap Then
        '        m_lReturn& = SelectComboItem(r_ctl:=cboPaymentStatus, _
        ''                v_varID:=ACPaymentStatusIssued)
        '    Else
        '        m_lReturn& = SelectComboItem(r_ctl:=cboPaymentStatus, _
        ''                v_varID:=ACPaymentStatusPendingApproval)
        '    End If
        'End If

        If Conversion.Val(txtAmount.Text) <> 0 Then
            txtAmount_Leave(txtAmount, New EventArgs())
        End If

        m_sSaveMediaTypeText = cmbMediaType.Text

        ' Start - Sankar - PN 56728
        If (uctAccountLookup.AccountId <> 0 And sLookUpcode = ACMediaTypeCreditCard And SSTabHelper.GetTabVisible(tabMainTab, 1)) Or (uctAccountLookup.AccountId <> 0 And SSTabHelper.GetTabVisible(tabMainTab, 2)) Then
            m_lReturn = ClearFields(True)
            m_lReturn = GetPartyBanks()
            m_lReturn = RefreshAccountDetails()
            If m_lPartyBankId > 0 Then
                uctPartyBankCombo1.SelectedPaymentID = m_lPartyBankId
                m_lPartyBankId = 0
            End If
        End If
        If sLookUpcode = ACMediaTypeCreditCard And CashlistTypeID = gACTLibrary.ACTCashListTypePayments Then
            uctPaymentCC.ViewOnlyMode = True
        End If
        'End - Sankar - PN 56728

        'WPR12- Enhancement Quote Collection Process
        ChangeLabelChequeAndCreditCard()

        ' WPR 51

        If IsSplitReceipt Then
            SplitReceiptMandatoryAndEnabledFieldsFields()
            If Not IsLeadAccount Then
                uctReceiptCC.ViewOnlyMode = True
            End If
        End If
        If m_bIsMediaRefMandatory = True Then
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
        Else
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, False)
        End If
    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: SetMandatoryAndEnabledFields
    ' PURPOSE: Sets mandatory controls according to the the media and receipt type
    ' AUTHOR: Steve Watton
    ' DATE: 22 October 2002, 12:35:28
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Sub SetMandatoryAndEnabledFields(Optional ByVal sReceiptCode As String = "Zero", Optional ByVal sMediaCode As String = "Zero")

        Dim oFF As iPMFormControl.FormField
        Dim iLookUpID As Integer
        Dim sReversalCode, sBankReference As String

        Const cUndefinedReason As String = "OTHER"

        Try

            'DD 15/10/2003: Reset everything first

            oFF = m_oFormFields.Item("txtMediaRef-0")
            oFF.IsMandatory = False
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, False)

            oFF = m_oFormFields.Item("txtContactName-0")
            oFF.IsMandatory = False
            lblContactName.Font = VB6.FontChangeBold(lblContactName.Font, False)

            oFF = m_oFormFields.Item("txtFurtherDetails-0")
            oFF.IsMandatory = False
            lblFurtherDetails.Font = VB6.FontChangeBold(lblFurtherDetails.Font, False)

            oFF = m_oFormFields.Item("txtReverseDate-0")
            oFF.IsMandatory = False
            lblReverseDate.Font = VB6.FontChangeBold(lblReverseDate.Font, False)

            oFF = m_oFormFields.Item("cboReversalReason-0")
            oFF.IsMandatory = False
            lblReversalReason.Font = VB6.FontChangeBold(lblReversalReason.Font, False)

            oFF = m_oFormFields.Item("txtReason-0")
            oFF.IsMandatory = False
            lblReason.Font = VB6.FontChangeBold(lblReason.Font, False)

            oFF = m_oFormFields.Item("txtName-0")
            oFF.IsMandatory = False
            lblName.Font = VB6.FontChangeBold(lblName.Font, False)

            oFF = m_oFormFields.Item("cboBank-0")
            oFF.IsMandatory = False
            lblBank.Font = VB6.FontChangeBold(lblBank.Font, False)

            oFF = m_oFormFields.Item("txtChequeDate-0")
            oFF.IsMandatory = False
            lblChequeDate.Font = VB6.FontChangeBold(lblChequeDate.Font, False)

            'WPR12- Enhancement Quote Collection Process

            oFF = m_oFormFields.Item("txtBankLocation-0")
            oFF.IsMandatory = False
            lblBankLocation.Font = VB6.FontChangeBold(lblBankLocation.Font, False)

            oFF = m_oFormFields.Item("cboChequeType-0")
            oFF.IsMandatory = False
            lblChequeType.Font = VB6.FontChangeBold(lblChequeType.Font, False)

            oFF = m_oFormFields.Item("txtBankBranch-0")
            oFF.IsMandatory = False
            lblBankBranch.Font = VB6.FontChangeBold(lblBankBranch.Font, False)

            oFF = m_oFormFields.Item("cboChequeClearingType-0")
            oFF.IsMandatory = False
            lblChequeClearingType.Font = VB6.FontChangeBold(lblChequeClearingType.Font, False)

            oFF = m_oFormFields.Item("txtPaymentAccountCode-0")
            oFF.IsMandatory = False
            lblPaymentAccountCode.Font = VB6.FontChangeBold(lblPaymentAccountCode.Font, False)

            oFF = m_oFormFields.Item("txtBIC-0")
            oFF.IsMandatory = False
            lblBIC.Font = VB6.FontChangeBold(lblBIC.Font, False)

            oFF = m_oFormFields.Item("txtIBAN-0")
            oFF.IsMandatory = False
            lblIBAN.Font = VB6.FontChangeBold(lblIBAN.Font, False)

            oFF = m_oFormFields.Item("txtPayeeName-0")
            oFF.IsMandatory = False
            lblPayeeName.Font = VB6.FontChangeBold(lblPayeeName.Font, False)

            oFF = m_oFormFields.Item("txtChange-0")
            oFF.IsMandatory = False
            lblChange.Font = VB6.FontChangeBold(lblChange.Font, False)
            lblChange.Visible = False
            txtChange.Visible = False

            oFF = m_oFormFields.Item("txtTendered-0")
            oFF.IsMandatory = False
            lblTendered.Font = VB6.FontChangeBold(lblTendered.Font, False)
            lblTendered.Visible = False
            txtTendered.Visible = False

            'DD 21/10/2003

            oFF = m_oFormFields.Item("txtBankRef-0")
            oFF.IsMandatory = False
            lblBankRef.Font = VB6.FontChangeBold(lblBankRef.Font, False)
            txtBankRef.Enabled = False
            lblBankRef.Enabled = False

            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts And sReceiptCode <> "Zero" Then

                'need to add the recovery agreement and instalment tab to this fuction

                Select Case sReceiptCode
                    Case ACInstalmentReceiptType

                        ' PW060302 - enable these 2 fields for
                        ' receipt type of instalments: start
                        ' ISS2798
                        lblTendered.Visible = True
                        txtTendered.Visible = True
                        lblChange.Visible = True
                        txtChange.Visible = True
                        txtAmount.Enabled = True
                        txtTendered.Enabled = True

                        ' PW060302 - enable these 2 fields for
                        ' receipt type of instalments: end
                        cmdSalvage.Visible = False
                        cmdClaimDebt.Visible = False
                        EnableReceiptFrames(ACClaimsFrame, False)

                    Case ACReversalOfAnyType

                        oFF = m_oFormFields.Item("txtReverseDate-0")
                        oFF.IsMandatory = True
                        lblReverseDate.Font = VB6.FontChangeBold(lblReverseDate.Font, True)

                        oFF = m_oFormFields.Item("cboReversalReason-0")
                        oFF.IsMandatory = True
                        lblReversalReason.Font = VB6.FontChangeBold(lblReversalReason.Font, True)

                        iLookUpID = VB6.GetItemData(cboReversalReason, cboReversalReason.SelectedIndex)

                        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupReverseType, iLookUpID:=iLookUpID, sLookUpcode:=sReversalCode)

                        'set the free text to mandatory if the selected reason code is other.
                        If sReversalCode = cUndefinedReason Then

                            oFF = m_oFormFields.Item("txtReason-0")
                            oFF.IsMandatory = True
                            lblReason.Font = VB6.FontChangeBold(lblReason.Font, True)
                        Else

                            oFF = m_oFormFields.Item("txtReason-0")
                            oFF.IsMandatory = False
                            lblReason.Font = VB6.FontChangeBold(lblReason.Font, False)
                        End If

                        cmdSalvage.Visible = False
                        cmdClaimDebt.Visible = False
                        EnableReceiptFrames(ACClaimsFrame, False)

                    Case Else
                        txtAmount.Enabled = True
                        lblTendered.Visible = True
                        txtTendered.Visible = True
                        lblChange.Visible = True
                        txtChange.Visible = True
                        txtTendered.Enabled = True
                        cmdSalvage.Visible = False
                        cmdClaimDebt.Visible = False
                        EnableReceiptFrames(ACClaimsFrame, False)

                End Select

            ElseIf ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then

                'DD 03/09/2003: We're dealing with Payments
                fraBank.Enabled = True
                lblCancellationReason.ForeColor = ColorTranslator.FromOle(ACEnabledColor)

                If Not ViaClaimPayment Then
                    txtPaymentReason.Enabled = True
                End If

            End If

            'TF141103 - PN8334 - Never enable this for insurer payment
            'If (ViaInsurerPayment Or ViaClaimPayment) Or m_bAmountDisabled Then
            If (ViaInsurerPayment Or ViaClaimPayment Or ViaFinancePlan) Or m_bAmountDisabled Then
                txtAmount.Enabled = False
                TxtWoffAmt.Enabled = False
            Else
                txtAmount.Enabled = True
            End If

            If sMediaCode <> "Zero" Then

                'DD 21/10/2003: Handle the Banking Reference
                'PN 31427 (RC)
                'AR20061124 - PN30097 Enable Bank Reference for Cash Payments
                If cmbMediaType.SelectedIndex >= 0 And (CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Or CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Then
                    If GetIsBankingFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then

                        txtBankRef.Enabled = True
                        lblBankRef.Enabled = True

                        oFF = m_oFormFields.Item("txtBankRef-0")
                        oFF.IsMandatory = True
                        lblBankRef.Font = VB6.FontChangeBold(lblBankRef.Font, True)

                        'Retrieve the last value from the local Registry
                        If gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMERegSettingLevel.pmeRSLClient, "LastBankReference", sBankReference) <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Bank Reference from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryAndEnabledFields", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Else
                            txtBankRef.Text = sBankReference
                            'txtBankRef_Leave(txtBankRef, New EventArgs())
                        End If

                    End If
                End If

                If cmbMediaType.SelectedIndex >= 0 And (CashlistTypeID = gACTLibrary.ACTCashListTypePayments Or CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts) Then
                    If GetIsValidationEnabledFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then

                        If SSTabHelper.GetTabVisible(Me.tabMainTab, TAB_PAYMENT) Then
                            If sMediaCode <> ACMediaTypeCreditCard Then
                                txtPayeeName.Enabled = True

                                oFF = m_oFormFields.Item("txtPayeeName-0")
                                oFF.IsMandatory = True
                                lblPayeeName.Font = VB6.FontChangeBold(lblPayeeName.Font, True)
                            Else
                                txtPayeeName.Enabled = False

                                oFF = m_oFormFields.Item("txtPayeeName-0")
                                oFF.IsMandatory = False
                                lblPayeeName.Font = VB6.FontChangeBold(lblPayeeName.Font, True)
                            End If
                        End If

                    End If
                End If

                'DD 21/10/2003: Handle the Media Reference Mandatory setting
                If cmbMediaType.SelectedIndex >= 0 And ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then
                    If GetIsMediaRefMandatoryFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                        txtMediaRef.Enabled = True

                        oFF = m_oFormFields.Item("txtMediaRef-0")
                        oFF.IsMandatory = True
                        lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
                    End If
                End If
                If cmbMediaType.SelectedIndex >= 0 And ((CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts)) Then
                    If GetIsMediaRefMandatoryFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                        txtMediaRef.Enabled = True

                        oFF = m_oFormFields.Item("txtMediaRef-0")
                        oFF.IsMandatory = True
                        lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
                    End If
                End If
                If cmbMediaType.SelectedIndex >= 0 Then
                    If GetIsPrintedAutomaticallyFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                        Letter = True

                        chkLetter.CheckState = CheckState.Checked

                    Else
                        Letter = False

                        chkLetter.CheckState = CheckState.Unchecked
                    End If
                End If

                Select Case sMediaCode
                    Case ACMediaTypeCash, "CA"
                        'DD 10/09/2003: Fields are only mandatory for Receipts
                        If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                            'PN10087 eck 02022002

                            oFF = m_oFormFields.Item("txtAmount-0")
                            oFF.IsMandatory = True
                            lblAmount.Font = VB6.FontChangeBold(lblAmount.Font, True)
                            'PN10087End

                            oFF = m_oFormFields.Item("txtTendered-0")
                            oFF.IsMandatory = True
                            lblTendered.Font = VB6.FontChangeBold(lblTendered.Font, True)

                            oFF = m_oFormFields.Item("txtChange-0")
                            oFF.IsMandatory = True
                            lblChange.Font = VB6.FontChangeBold(lblChange.Font, True)

                            lblTendered.Visible = True
                            txtTendered.Visible = True
                            lblChange.Visible = True
                            txtChange.Visible = True
                        End If

                        cboBank.SelectedIndex = -1
                        txtChequeDate.Text = ""

                        'DD 16/10/2003: Don't reset if we are viewing
                        If CashDrawerID <> 0 And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                            chkLetter.CheckState = CheckState.Checked
                        End If
                        txtChequeDate.Text = ""

                        'WPR12- Enhancement Quote Collection Process
                        txtBankLocation.Text = ""
                        cboChequeType.SelectedIndex = -1
                        txtBankBranch.Text = ""
                        cboChequeClearingType.SelectedIndex = -1

                    Case ACMediaTypeBank, ACMediaTypeCheque, "CQ"
                        'DD 10/09/2003: Fields are only mandatory for Receipts
                        If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then

                            oFF = m_oFormFields.Item("txtName-0")
                            oFF.IsMandatory = True
                            lblName.Font = VB6.FontChangeBold(lblName.Font, True)

                            oFF = m_oFormFields.Item("txtChequeDate-0")
                            oFF.IsMandatory = True
                            lblChequeDate.Font = VB6.FontChangeBold(lblChequeDate.Font, True)

                            If Not Information.IsDate(txtChequeDate.Text) Then txtChequeDate.Text = DateTime.Today
                        End If

                        ' RDC 14112003 check that account is allowed to make electronic payments
                        m_lReturn = CheckElectronicPayments()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to check electronic payment options", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check electronic payment options", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryAndEnabledFields", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If

                    Case ACMediaTypeCreditCard

                        cboBank.SelectedIndex = -1
                        txtChequeDate.Text = ""

                        lblTendered.Visible = False
                        txtTendered.Visible = False
                        lblChange.Visible = False
                        txtChange.Visible = False
                        txtChequeDate.Text = ""

                        'WPR12- Enhancement Quote Collection Process
                        txtBankLocation.Text = ""
                        cboChequeType.SelectedIndex = -1
                        txtBankBranch.Text = ""
                        cboChequeClearingType.SelectedIndex = -1

                    Case Else
                        txtChequeDate.Text = ""
                End Select
            End If

            If IsSplitReceipt Then
                SplitReceiptMandatoryAndEnabledFieldsFields()
            End If
            If (g_bChequeProduction) AndAlso
                  ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                   (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then

                If Me.cmbMediaType.SelectedIndex > -1 Then
                    Dim iMediaId As Integer
                    Dim sPaymentMethod As String = ""
                    'now look up the media type and round if cash
                    iMediaID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

                    m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iMediaID, sLookUpcode:=sPaymentMethod)

                    If sPaymentMethod = ACMediaTypeBank Then
                        oFF = m_oFormFields.Item("txtourRef-0")
                        oFF.IsMandatory = True
                        lblOurRef.Font = VB6.FontChangeBold(lblOurRef.Font, True)
                    Else
                        oFF = m_oFormFields.Item("txtourRef-0")
                        oFF.IsMandatory = False
                        lblOurRef.Font = VB6.FontChangeBold(lblOurRef.Font, False)
                    End If
                End If
            End If
            oFF = Nothing

        Catch excep As System.Exception

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryAndEnabledFields", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End Select

        End Try

    End Sub

    Private Sub cmbMediaType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbMediaType.Enter
        m_sSaveMediaTypeText = cmbMediaType.Text

    End Sub

    Private Sub cmbMediaType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbMediaType.Leave
        'eck300401
        Dim sLookUpcode, sLookUpCaption As String

        m_sSaveMediaTypeText = cmbMediaType.Text
        If cmbMediaType.SelectedIndex = -1 Or m_bReverseCashDrawerListItem Then Exit Sub

        Dim iLookUpID As Integer = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpCaption:=sLookUpCaption, sLookUpcode:=sLookUpcode)

        Select Case sLookUpcode
            Case ACMediaTypeCreditCard
                uctPartyBankCombo2.EnableCombo = True
                lblReceiptAccountType.Enabled = True
                EnableReceiptFrames(ACCreditCardFrame, True)
                EnableReceiptFrames(ACChequeFrame, False)

                ' For credit card payments/receipts we must let uctACTCreditCard immediately know of
                ' any changes to the Media Type as it will effect display and validation of the control.
                ' For Receipts this will also enable the usercontrol and for Payments it will make it visible.
                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    uctReceiptCC.MediaTypeID = VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)
                    If ContactName.Trim() <> "" Then
                        uctReceiptCC.CCName = ContactName.Trim()
                    End If
                Else
                    If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                        fraPaymentCreditCard.Visible = True
                        uctPaymentCC.MediaTypeID = VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)
                        fraBank.Visible = False
                        fraPayee.Visible = False
                    End If
                End If

                ClearChequeDetails()
                cmbMediaType.Text = sLookUpCaption

            Case ACMediaTypeBank, ACMediaTypeCheque
                uctPartyBankCombo2.EnableCombo = False
                lblReceiptAccountType.Enabled = False
                EnableReceiptFrames(ACCreditCardFrame, False)
                EnableReceiptFrames(ACChequeFrame, True)

                ClearCreditCardDetails()

                m_lReturn = m_oFormFields.FormatControl(txtName, PaymentName)
                m_lReturn = m_oFormFields.FormatControl(txtBIC, sBIC)
                m_lReturn = m_oFormFields.FormatControl(txtIBAN, sIBAN)

                'sw front office receipting 03-12-2002
                If uctAccountLookup.Text <> "" And Replacescashlistitemid = 0 Then
                    'PSL 03/03/2003 Shouldn't overide it if there is already one type in
                    If txtName.Text = "" Then

                        m_oFormFields.FormatControl(txtName, ContactName)
                    End If
                End If
            Case ACMediaTypeCash
                uctPartyBankCombo2.EnableCombo = False
                lblReceiptAccountType.Enabled = False
                EnableReceiptFrames(ACCreditCardFrame, False)
                EnableReceiptFrames(ACChequeFrame, False)

                ClearChequeDetails()
                ClearCreditCardDetails()
                ' Gaurav Doubt
            Case ACMediaTypeBank, ACMediaTypeCheque
                EnableReceiptFrames(ACCreditCardFrame, False)
                EnableReceiptFrames(ACChequeFrame, False)

                ClearCreditCardDetails()
            Case Else
                uctPartyBankCombo2.EnableCombo = False
                lblReceiptAccountType.Enabled = False
                ClearCreditCardDetails()

        End Select

        'WPR12- Enhancement Quote Collection Process
        ChangeLabelChequeAndCreditCard()

        ' WPR 51
        If IsSplitReceipt Then
            SplitReceiptMandatoryAndEnabledFieldsFields()
        End If

        If m_bIsMediaRefMandatory = True Then
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
        Else
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, False)
        End If
    End Sub

    Private Sub cmdClaimDebt_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClaimDebt.Click
        Dim iCLMDebtAllocate As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdClaimDebt_Click
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 02 April 2003, 10:30:00
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Const ACMethod As String = "cmdClaimDebt_Click"

        Dim oClaimDebtAllocate As Object
        Dim iPtr As IntPtr = tabMainTab.Handle
        Try

            'jmf 8/7/2003
            'MsgBox "Functionality to be completed"
            'Exit Sub
            'todolist(Project not found)
            'oClaimDebtAllocate = New iCLMDebtAllocate.Interface()
            oClaimDebtAllocate = New Object()

            With oClaimDebtAllocate

                m_lReturn = .Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Unable to initialise component: " &
                                               "iCLMDebtAllocate.Interface")
                End If

                .CallingAppName = ACApp

                'Ensure the PMView is set so we enter in read only mode

                m_lErrorNumber = .SetProcessModes(vTask:=IIf(m_iTask = gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMAdd), vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set process modes")
                End If

                'Set the start up options
                Dim vKeys As Array = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 5}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})
                'NOTE - Add these to PMNavKeyConsts when module is checked back in

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "ReceiptNo"
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = Me.uctAccountLookup.Text & "/" & Me.txtMediaRef.Text
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "ReceiptAmount"
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = Me.txtAmount.Text
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "Reversed Items"
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_vReversedRBIItems
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Unsaved Items"
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_vUnsavedRBIItems
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemId
                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = CashlistitemID
                m_lErrorNumber = .SetKeys(vKeys)
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set keys")
                End If

                '.SetProcessModes Val("drqw"), 0, 0, "", Now
                '.StartMode = 1

                m_lErrorNumber = .Start()
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to set start processing")
                End If

                'Test the keys for the returned details

                m_lErrorNumber = .GetKeys(vKeys)
                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get keys")
                End If

                'TR - Unload the data from the array passed back.
                For lCount As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                    Select Case vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)
                        Case "Reversed Items"

                            m_vReversedRBIItems = vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount)
                        Case "Unsaved Items"

                            m_vUnsavedRBIItems = vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount)
                        Case "OnInstalmentPlan"

                            m_bOnInstalment = CBool(vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))
                            'TR - Show / Hide the instalments tab
                            SSTabHelper.SetTabVisible(tabMainTab, TAB_INSTALMENT, m_bOnInstalment)
                            SSTabHelper.SetTabEnabled(tabMainTab, TAB_INSTALMENT, m_bOnInstalment)
                        Case Else
                    End Select
                Next lCount

                'NOTE - this process has not been finalised - awaiting completion
                'of Tech Spec 223
            End With



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    Exit Sub

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub

            End Select

        Finally
            If Not (oClaimDebtAllocate Is Nothing) Then

                oClaimDebtAllocate.Dispose()
                oClaimDebtAllocate = Nothing
            End If



        End Try
        Exit Sub
    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        If LCase(cmdHelp.Text) = "view" Then
            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
            m_lReturn = ShowInsurerPayment
            If m_lApprovalType = kApprovalTypeInsurerPayment Then
                If m_lReturn = PMEReturnCode.PMTrue Then
                    m_lStatus = PMEReturnCode.PMOK
                Else
                    m_lStatus = PMEReturnCode.PMCancel
                End If
            End If

            iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
            Me.Hide()
        Else
            PMHelpFunc.g_sProductFamily = g_sProductFamily
            m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
        End If
    End Sub

    ''
    ''Author: Steve Watton
    ''Name: cmdSalvege_Click
    ''Method: Call the salvage form passing in neccessary parameters,
    ''        this returns a salvage array containing salvage details
    Private Sub cmdSalvage_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSalvage.Click
        Dim oSalvage As Object
        Dim lPartyCnt As Integer

        Try

            'First Check that a valid account has been selected
            If Conversion.Val(CStr(uctAccountLookup.AccountId)) = 0 Then
                MessageBox.Show("Please select a valid account", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else

                m_lReturn = m_oBusiness.GetPartyCntFromAccountID(Conversion.Val(CStr(uctAccountLookup.AccountId)), lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lPartyCnt = 0 Then
                    MessageBox.Show("Unable to proceed with this account", "Invalid " &
                                    "Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            If Conversion.Val(txtAmount.Text) = 0 Then
                MessageBox.Show("Enter a receipt amount before proceeding", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'create a new version of the salvage interface object
            Dim temp_oSalvage As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSalvage, sClassName:="ICLMSalvageReceipt.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oSalvage = temp_oSalvage

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to create the salvage interface", "Warning: Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'jmf 11/9/2003
            g_lLockedSalvagePartyId = lPartyCnt

            'call the set up method on the salvage interface, this will return the salvage array

            m_lReturn = oSalvage.SetUp(lScreenMode:=IIf(m_iTask = gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMEdit), r_vSalvageItemsArray:=SalvageArray, vPartyID:=lPartyCnt, vReceiptAmount:=txtAmount.Text, lCashListItemID:=CashlistitemID)

            oSalvage = Nothing

            ControlHelper.Print(Me)

            '
            'Method Name Setup
            '
            '
            'uctaccountlookup.
            'SetUp( _
            ''    lScreenMode As Integer, _
            ''    r_vSalvageItemsArray As Variant, _
            ''    vPartyID As Variant, _
            ''    vReceiptAmount As Variant, _
            ''    Optional lCashListItemId As Long) As Long
            '
            '
            'lCashListItemId  is only used during edit mode
            '
            'vSalvageItemsArray - list if salvage items
            '
            'vKeyArray(0, 0) = "1"   'salvage item id
            'vKeyArray(1, 0) = "2501"   'salvage actual sale
            'vKeyArray(2, 0) = "24/10/2001"   'salvage actual sale date
            'vKeyArray(3, 0) = "1.99"   'salvage actual sale GST amount
            '
            'vKeyArray(0, 1) = "2"   'salvage item id
            'vKeyArray(1, 1) = "502"   'salvage actual sale
            'vKeyArray(2, 1) = "23/10/2001"   'salvage actual sale date
            'vKeyArray(3, 1) = "4.22"   'salvage actual sale GST amount
            '
            '
            'Once this has been passed back to you, a further method is called to commit the the salvage receipt. This is on bCLMSalvageReceipt.Business  and the method is called:
            '
            'Public Function ProcessSalvageReceipts(ByRef vSalvageItemArray As Variant, _
            ''                                       ByRef lCashListItemId As Long, _
            ''                                       ByRef lPartyCnt As Long, _
            ''                                       ByRef r_vDocument_ids As Variant) As Long
            '
            '
            '
            'm_lReturn = g_oBusiness.ProcessSalvageReceipts(vKeyArray, 3, PartyId, vTransDetails)
            '
            '            vKeyArray(0, 0) = 1 - salvage item id
            '            vKeyArray(1, 0) = "5000"
            '            vKeyArray(2, 0) = "24/10/1999"
            '            vKeyArray(3, 0) = "2.2"
            '
            '            vKeyArray(0, 1) = 2
            '            vKeyArray(1, 1) = "2750"
            '            vKeyArray(2, 1) = "24/10/1999"
            '            vKeyArray(3, 1) = "1.2"
            '
            '
            '
            '
            '
            '
            'oSalvage.Mode = m_iTask
            'oSalvage.CashlistitemID = CashlistitemID
            'oSalvage.Party_cnt = 'partycnt (get from account id)
            'oSalvage.SalvageArray = SalvageArray
            'oSalvage.Amount = CLng(txtAmount.Text)
            '
            'oSalvage.GoGetEm
            'Set oSalvage = Nothing

        Catch excep As System.Exception

            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Salvage operations failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSalvage_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub frmDetails_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        lsFormInitializing = False
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Dim vOptionValue As String = ""
            Dim vUserAuthorityValue As Object

            'Float Balance and Pre-Payment (RC)
            'PN 32143 (RC)
            'Check the option for partial amounts
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=vOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_Activate", "iACTCashListItem.Form_Activate Failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            'Float Balance and Pre-Payment (RC) Gaurav Doubt
            If m_sScreenType = "PAYNOW" Then
                cboPaymentType.Enabled = False
                'PN: 61320, Let user select if multiple items are there
                'cboReceiptType.Enabled = False
                txtAmount.Enabled = False
                txtTendered.Text = txtAmount.Text
                lblAmount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                m_bAmountDisabled = True

                'If PayNow(Hidden) option is checked then Account is Editable else ReadOnly
                If gPMFunctions.NullToString(vOptionValue) = "1" Then
                    uctAccountLookup.Enabled = True
                    lblAccount.ForeColor = Color.Black
                Else
                    uctAccountLookup.Enabled = False
                    lblAccount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If

                'WPR12- Enhancement Quote Collection Process
                If m_sCashListActualCalledFrom = "iPMUQUoteCollectionProcess" Then
                    'Single Quote Selected
                    If Not m_bMultiplePoliciesSelected Then
                        If (m_sQuoteAgentType = "Intermed") Or (m_sQuoteAgentType = "Broker" And m_lQuoteClientCnt > 0) Then
                            uctAccountLookup.Enabled = True
                            lblAccount.ForeColor = Color.Black
                        Else
                            uctAccountLookup.Enabled = False
                            lblAccount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                        End If
                    End If

                    'Multiple Quote Selected
                    If m_bMultiplePoliciesSelected Then
                        uctAccountLookup.Enabled = False
                        lblAccount.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    End If
                End If

            End If

            '25/06/2008 Rahul Jaiswal 64VB Enhancement for UIIC
            'Start
            If vOptionValue = "1" And CashlistTypeID = ACReceiptType Then
                lblCollectionDate.Visible = True
                txtCollectionDate.Visible = True
                lblComments.Visible = True
                txtComments.Visible = True

                m_lReturn = m_oBusiness.GetCollectionDateOverrideAuthority(v_lUserID:=g_iUserID, r_vResults:=vUserAuthorityValue)

                txtCollectionDate.ReadOnly = Not (CStr(vUserAuthorityValue(0, 0)) = "1")
            Else
                lblCollectionDate.Visible = False
                txtCollectionDate.Visible = False
                lblComments.Visible = False
                txtComments.Visible = False
            End If

        End If
    End Sub

    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Dim oOptions As bSIROptions.Business
        Dim sValue As String = ""
        uctPartyBankCombo1.EnableAdd = True
        uctPartyBankCombo1.EnableEdit = True
        uctPartyBankCombo2.EnableAdd = True
        uctPartyBankCombo2.EnableEdit = True
        Me.cboUnderwritingYear.FirstItem = "(None)"
        If (Not UnderwritingYearID Is Nothing) And (Not IsDBNull(UnderwritingYearID)) Then
            cboUnderwritingYear.ItemId = gPMFunctions.ToSafeInteger(UnderwritingYearID)
        End If

        Dim temp_oOptions As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oOptions = temp_oOptions
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        ' Set the correct QAS database
        uctAddress.QASDatabaseID = CInt(Conversion.Val(sValue))

        If Conversion.Val(sValue) <> 0 Then
            uctAddress.PMDatabaseID = 0
        Else
            uctAddress.PMDatabaseID = 1
        End If

        uctAddress.CountryId = g_iCountryID

        m_lReturn = uctAddress.Initialise()

        oOptions.Dispose()
        oOptions = Nothing

        ' WPR 51
        If IsSplitReceipt Then
            chkIsLeadAccount.Visible = True
            lblSplitTotal.Visible = True
            txtSplitTotal.Visible = True
            lblTendered.Visible = False
        End If

        'Party Bank Details
        m_lReturn = FillPartyBankCombo()
        If m_lPartyBankId > 0 And SSTabHelper.GetTabVisible(tabMainTab, 2) Then
            uctPartyBankCombo1.SelectedPaymentID = m_lPartyBankId
            uctPartyBankCombo1.PopulateScreen()
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

    End Sub

    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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

            ' Destroy this instance of the calling object
            ' from memory.
            m_oListForm = Nothing

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

    'Private Sub frmDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
    '    Dim KeyCode As Integer = eventArgs.KeyCode
    '    Dim Shift As Integer = eventArgs.KeyData \ &H10000

    '    Dim iCtrlDown As Integer

    '    Const ACCtrlMask As Integer = 2

    '    Try

    '        ' Set the control key value.
    '        iCtrlDown = (Shift And ACCtrlMask) > 0

    '        With tabMainTab
    '            ' Check the key pressed.
    '            Select Case KeyCode
    '                Case Keys.PageUp
    '                    ' Page Up key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the first tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    '                    Else
    '                        ' Check we are not on the
    '                        ' first tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
    '                            ' Display the previous tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
    '                        End If
    '                    End If

    '                Case Keys.PageDown
    '                    ' Page Down key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the last tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
    '                    Else
    '                        ' Check we are not on the
    '                        ' last tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
    '                            ' Display the next tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
    '                        End If
    '                    End If

    '                Case Keys.Home
    '                    ' Home key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If

    '                Case Keys.End
    '                    ' End key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If
    '            End Select
    '        End With
    '        
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
    '            tabMainTab.SelectedIndex = 0
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
    '            tabMainTab.SelectedIndex = 1
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
    '            tabMainTab.SelectedIndex = 2
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
    '            tabMainTab.SelectedIndex = 3
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
    '            tabMainTab.SelectedIndex = 4
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
    '            tabMainTab.SelectedIndex = 5
    '        End If
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
    '            tabMainTab.SelectedIndex = 6
    '        End If
    '    Catch

    '        ' Error Section.

    '        Exit Sub
    '    End Try

    'End Sub

    Private Sub frmDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If
        If Not (m_oUserAuthorities Is Nothing) Then

            m_oUserAuthorities.Dispose()
            m_oUserAuthorities = Nothing
        End If

        If Not (uctPaymentCC Is Nothing) Then
            uctPaymentCC.Dispose()
        End If

        If Not (uctReceiptCC Is Nothing) Then
            uctReceiptCC.Dispose()
        End If

    End Sub

    Private Sub lvInstalments_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvInstalments.ItemCheck
        Dim Item As ListViewItem = lvInstalments.Items(eventArgs.Index)
        Dim lRow As Integer

        Try
            If m_bAddingItems Then Exit Sub

            'm_bremovingitems is set in the lost focus event of the amount field,
            'if amount has been reduced then the listitems are unchecked
            If m_bRemovingItems Then Exit Sub

            lRow = Convert.ToInt16(Item.Tag)

            'flag the item in the array as checked
            If Item.Checked Then

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue
            Else

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMFalse
            End If

            If DisplaySelectedInstalmentTotals() = gPMConstants.PMEReturnCode.PMFalse Then
                Item.Checked = gPMConstants.PMEReturnCode.PMFalse

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the lvInstalment_ItemCheck event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvInstalments_ItemCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name:     Enable Frame Controls
    ' Author:   Steve Watton
    '
    ' Description:Enable/disable the appropriate frame on the receipttab
    'Added for front office receipting
    ' ***************************************************************** '

    Private Sub EnableReceiptFrames(ByRef iFrame As Integer, ByRef bEnabled As Boolean)

        fraReceipt(iFrame).Enabled = bEnabled

        Select Case iFrame
            Case ACChequeFrame
                txtName.Enabled = bEnabled
                txtChequeDate.Enabled = bEnabled
                cboBank.Enabled = bEnabled
                'WPR12- Enhancement Quote Collection Process
                txtBankLocation.Enabled = bEnabled
                cboChequeType.Enabled = bEnabled
                txtBankBranch.Enabled = bEnabled
                cboChequeClearingType.Enabled = bEnabled

                If bEnabled Then
                    lblName.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblChequeDate.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblBank.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    'WPR12- Enhancement Quote Collection Process
                    lblBankLocation.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblChequeType.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblBankBranch.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblChequeClearingType.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                Else
                    lblName.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblChequeDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblBank.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    'WPR12- Enhancement Quote Collection Process
                    lblBankLocation.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblChequeType.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblBankBranch.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblChequeClearingType.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If
            Case ACCreditCardFrame

            Case ACClaimsFrame
                cmdSalvage.Enabled = bEnabled
                cmdClaimDebt.Enabled = bEnabled

            Case ACReversalFrame

                txtReverseDate.Enabled = bEnabled
                cboReversalReason.Enabled = bEnabled
                txtReason.Enabled = bEnabled
                If bEnabled Then
                    lblReverseDate.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblReversalReason.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                    lblReason.ForeColor = ColorTranslator.FromOle(ACEnabledColor)
                Else
                    lblReverseDate.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblReversalReason.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                    lblReason.ForeColor = ColorTranslator.FromOle(ACDisabledColor)
                End If

        End Select

    End Sub

    ' ***************************************************************** '
    ' Name:     DisplaySelectedInstalmentTotals
    ' Author:   Steve Watton
    '
    ' Description:Displays the instalment detail tlab total fields, providing the
    '             total is less than or equal to the amount.
    '             also sets amount tendered to the instalment total
    '
    'Added for front office receipting
    ' ***************************************************************** '
    Private Function DisplaySelectedInstalmentTotals() As Boolean

        Dim result As Boolean = False
        Dim curThisPlanTotal, curOverallTotal As Decimal
        Dim lRowCount As Integer
        Dim iMediaID As Integer
        Dim sPaymentMethod As String = ""
        Dim cCurrencyAmount As Decimal

        Try

            If Not Information.IsArray(InstalmentArray) Then
                Return result
            End If

            lRowCount = InstalmentArray.GetUpperBound(1)
            curThisPlanTotal = 0
            curOverallTotal = 0
            m_cInstalmentBaseTotal = 0

            'Create currency convert object if it hasn't been already.
            m_lReturn = CreateCurrencyConvert()

            Dim curOverallTotalInTran As Double = 0
            Dim curOverallTotalTranCurrency As Integer
            Dim curThisPlanTotalInTran As Double = 0
            Dim curThisPlanTotalTranCurrency As Integer

            Dim iBaseCurrencyID As Integer = 0

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=CompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)


            For lRow As Integer = 0 To lRowCount
                If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                    'Add on to the overall total
                    If ToSafeInteger(InstalmentArray(ACUseTransCurrencyInPF, lRow)) = 1 Then
                        curOverallTotalInTran += ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow))
                        curOverallTotalTranCurrency = ToSafeInteger(InstalmentArray(ACTransCurrencyCode, lRow))
                    Else
                        curOverallTotal += ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow))
                    End If



                    If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) = cboInstalment.Text Then
                        'If this is from the selected plan than add to the selected plan total

                        If ToSafeInteger(InstalmentArray(ACUseTransCurrencyInPF, lRow)) = 1 Then
                            curThisPlanTotalInTran += ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow))
                            curThisPlanTotalTranCurrency = ToSafeInteger(InstalmentArray(ACTransCurrencyCode, lRow))
                        Else
                            curThisPlanTotal += ToSafeDouble(InstalmentArray(ACInstalmentAmount, lRow))
                        End If
                    End If
                End If
            Next

            Dim dBaseAmount As Double

            curThisPlanTotalInTran = Math.Round(curThisPlanTotalInTran, 2)
            curOverallTotalInTran = Math.Round(curOverallTotalInTran, 2)

            If CurrencyID <> curOverallTotalTranCurrency Then
                If curOverallTotalInTran <> 0 Then
                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=curOverallTotalTranCurrency, lCompanyID:=CompanyID, cBaseAmount:=dBaseAmount, cCurrencyAmount:=curOverallTotalInTran, vConversionDate:=Transactiondate)
                    curOverallTotal = curOverallTotal + dBaseAmount
                End If
            Else
                curOverallTotal += curOverallTotalInTran
            End If

            dBaseAmount = 0
            If CurrencyID <> curOverallTotalTranCurrency Then
                If curThisPlanTotalInTran <> 0 Then
                    m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=curThisPlanTotalTranCurrency, lCompanyID:=CompanyID, cBaseAmount:=dBaseAmount, cCurrencyAmount:=curThisPlanTotalInTran, vConversionDate:=Transactiondate)
                    curThisPlanTotal = curThisPlanTotal + dBaseAmount
                End If
            Else
                curThisPlanTotal += curThisPlanTotalInTran
            End If

            Dim dInstalmentBaseTotal As Double
            If CurrencyID = curOverallTotalTranCurrency Then
                m_lReturn = m_oCurrencyConvert.ConvertCurrencyToBase(lCurrencyID:=curThisPlanTotalTranCurrency, lCompanyID:=CompanyID, cBaseAmount:=dInstalmentBaseTotal, cCurrencyAmount:=curOverallTotal, vConversionDate:=Transactiondate)
                m_cInstalmentBaseTotal = dInstalmentBaseTotal
            Else
                m_cInstalmentBaseTotal = curOverallTotal
            End If


            'Convert amounts to receipt currency
            If CurrencyID <> curOverallTotalTranCurrency Then
                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CurrencyID, lCompanyID:=CompanyID, cBaseAmount:=curOverallTotal, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=Transactiondate)
                curOverallTotal = cCurrencyAmount

                m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CurrencyID, lCompanyID:=CompanyID, cBaseAmount:=curThisPlanTotal, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=Transactiondate)
                curThisPlanTotal = cCurrencyAmount
            End If

            SSOveralPlanTotal.Text = StringsHelper.Format(curOverallTotal, "#,###.00")
            If SSOveralPlanTotal.Text = ".00" Then
                SSOveralPlanTotal.Text = "0.00"
            End If
            SSThisPlanTotal.Text = StringsHelper.Format(curThisPlanTotal, "#,###.00")
            If SSThisPlanTotal.Text = ".00" Then
                SSThisPlanTotal.Text = "0.00"
            End If

            result = True

            'Set tendered amount

            txtTendered.Text = CStr(curOverallTotal)

            If Not ViaInsurerPayment Then
                txtAmount.Text = CStr(curOverallTotal)
            End If

            '' AC CQ1700 25/09/03
            txtChange.Text = CStr(CDec(txtTendered.Text) - CDec(txtAmount.Text))

            m_lReturn = m_oFormFields.LostFocus(txtTendered)

            m_lReturn = m_oFormFields.LostFocus(txtChange)

            'TR - Check that the media Type has been selected
            If Me.cmbMediaType.SelectedIndex > -1 Then

                'now look up the media type and round if cash
                iMediaID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iMediaID, sLookUpcode:=sPaymentMethod)

                If sPaymentMethod = ACMediaTypeCash Then
                    ValidateMediaDetails()
                End If
            End If

            m_lReturn = m_oFormFields.LostFocus(txtTendered)

            m_lReturn = m_oFormFields.LostFocus(txtAmount)

            m_lReturn = m_oFormFields.LostFocus(txtChange)

            Return result

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process DisplaySelectedInstalmentTotals", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySelectedInstalmentTotals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateBGItem(ByRef lIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ValidateBGItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If gPMFunctions.ToSafeCurrency(lvwBGDetails.Items.Item(lIndex).SubItems.Item(kBankGuaranteeColHIndexPremiumAmount).Text, 0) = gPMFunctions.ToSafeCurrency("0", 0) Then
                MessageBox.Show("No Outstanding Premium left to allocate against this policy.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Sub lvwBGDetails_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwBGDetails.ItemCheck

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                '
                'SW 30/04/2003 CQ 917 Moved this do events to before check change of account.
                'confirm whomever wrote this above, there must be a bug in the tab control or something
                '
                Application.DoEvents()

                'sw front office receipting, dont see why this need to be called all the time
                If tabMainTabPreviousTab = 0 Then
                    CheckChangeOfAccount()
                End If

                'Party Bank Details
                If SSTabHelper.GetSelectedIndex(tabMainTab) = 1 Or SSTabHelper.GetSelectedIndex(tabMainTab) = 2 Then
                    m_lReturn = FillPartyBankCombo()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    If m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Enabled And m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Visible Then
                        m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                    End If
                End If

            End With

        Catch

            ' Error Section.

            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    ''' <summary>
    ''' cmdOK_Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object,
                            ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Const kClaimPaymentType As String = "CLP"

        Dim bValid As Boolean
        Dim bValidateUI As Boolean
        Dim oList(,) As Object
        Dim nPeriodMonths As Integer
        Dim sFailedMessage As String
        Dim sPaymentMethod As String
        Dim nLookUpID As Integer
        Dim sLookUpcode As String = ""
        Dim nMediaID As Integer
        Dim nStatusID As Integer
        Dim sStatusCode As String = ""
        Dim oOptionValue As Object
        Dim sReceiptCode As String
        Dim sMediaRef As String
        Dim nRows As Integer
        Dim bIsDeleted As Boolean
        Dim bReceiptTypeIsInstalmentBased As Boolean
        Dim bFailed As Boolean
        Dim nInsuranceFileCnt As Integer
        Dim sDocumentRef As String = ""
        Dim bValidation As Boolean
        Dim sClientAgent As String = ""
        Dim iBatchID As Integer
        Dim oPlicyNumMaint As bSIRPolicyNumMaint.Business 'PN 62476
        Dim lMediaTypeIndex As Integer 'PN 62476
        Dim bLastStep As Boolean
        Dim oCashListPost As Object
        Try
            m_bOKClicked = True
            If txtBankRef.Enabled OrElse txtBankRef.Text <> "" Then
                m_lReturn = ProcessBankBatch(txtBankRef.Text, iBatchID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    m_bOKClicked = False
                    Exit Sub
                End If
            End If
            m_ofrmList.TransactionDate = txtTransDate.Text
            'TR - See if the cmdOK button is currently supporting the "Approve" action
            If cmdOK.Text.ToLower() = "approve" Then
                iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseBusy)
                m_lReturn = m_oBusiness.GetCashlistbatchid(CashlistID, BatchID)
                m_lReturn = m_oBusiness.CheckInsurerPaymentRoadMap(nCashListItemID:=CashlistitemID,
                                                                   r_bIsInsurerPaymentRoadMap:=m_bIsInsurerePaymentRoadMap)

                m_lReturn = ApproveCashListItem(bLastStep)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Else
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    Me.Hide()
                    Exit Sub
                End If
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTAutoAllocateduringClaimPaymentWorkflow,
                                                          v_vBranch:=g_oObjectManager.SourceID,
                                                          r_vUnderwriting:=oOptionValue)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    m_bOKClicked = False
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process GetProductOptionValue.",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                If m_ofrmList.InsuranceFileCnt = 0 AndAlso gPMFunctions.NullToString(oOptionValue) = "1" Then
                    m_lReturn = m_oBusiness.GetPolicyDetailsFromClaimPayment(v_lClaimPaymentId:=m_lClaimPaymentId,
                                                                             r_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                                             r_sDocumentRef:=sDocumentRef)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("cmdOK_Click",
                                                "GetPolicyDetailsFromClaimPayment Failed",
                                                PMELogLevel.PMLogError)
                    End If
                    m_ofrmList.InsuranceFileCnt = nInsuranceFileCnt
                    m_ofrmList.DocumentRef = sDocumentRef
                End If

                If bLastStep Then
                    If m_bIsInsurerePaymentRoadMap = False Then
                        ' this is for cashlistitembatch_id not for navbatch_id
                        m_ofrmList.BatchID = BatchID
                    End If
                    m_ofrmList.IsInsurerePaymentRoadMap = m_bIsInsurerePaymentRoadMap
                    m_ofrmList.Post(CashlistitemID)
                    Dim nTaxTransDetailId As Integer
                    nTaxTransDetailId = m_ofrmList.TaxTransDetailID
                    iPMFunc.SetMousePointer(PMEMousePointerStatus.PMMouseNormal)
                    If m_lReturn = PMEReturnCode.PMTrue Then
                        m_lStatus = PMEReturnCode.PMOK
                    Else
                        m_lStatus = PMEReturnCode.PMCancel
                    End If
                    If m_bIsInsurerePaymentRoadMap Then
                        m_lReturn = AutoAllocateClick(m_lAccountID, CashlistitemID, nTaxTransDetailId)
                    End If
                End If
                Me.Hide()
            Else
                If m_iTask = PMEComponentAction.PMView Then
                    Me.Hide()
                    m_bOKClicked = False
                    Exit Sub
                End If

                bValidation = False
                If m_sCashListActualCalledFrom.ToUpper() = "IPMUQUOTECOLLECTIONPROCESS" Then
                    'Single Quote Selected
                    If Not m_bMultiplePoliciesSelected AndAlso uctAccountLookup.Enabled AndAlso m_lQuoteClientCnt > 0 AndAlso m_lQuoteAgentCnt > 0 Then
                        m_lReturn = ValidateIntermediateAndClient(r_bValidation:=bValidation, r_sClientAgent:=sClientAgent)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("cmdOk_Click", "ValidateIntermediateAndClient Failed", PMELogLevel.PMLogError)
                            Exit Sub
                        End If
                        If bValidation Then
                            'It's not an error
                            MessageBox.Show("Selected Account should be of Client Or Agent(Intermediary)." & Strings.Chr(13) & Strings.Chr(10) & sClientAgent, "Select Valid Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    End If
                End If

                ' ensure the user has selected an account before attempting to see if its deleted.
                If uctAccountLookup.AccountId <> 0 Then
                    m_lReturn = IsDeleted(uctAccountLookup.AccountId, bIsDeleted)
                    If bIsDeleted Then
                        MessageBox.Show("Account chosen has been marked as deleted." &
                                        Strings.Chr(13) & Strings.Chr(10) &
                                        "Please select another account.", "Invalid Account",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If

                If uctAccountLookup.AccountId <> 0 Then
                    m_lReturn = CheckAccount(v_lAccountId:=uctAccountLookup.AccountId,
                                             r_bFailed:=bFailed)
                    If bFailed Then
                        MessageBox.Show("Account chosen is the same as the bank account chosen." _
                                        & Strings.Chr(13) & Strings.Chr(10) &
                                        "Please select another account.", "Invalid Account",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If
                If String.Equals(m_sCallingAppName, "SplitReceiptsFromFindTransaction") AndAlso Not IsLeadAccount AndAlso LeadAccountBranchId <> 0 Then
                    If LeadAccountBranchId <> CompanyID Then
                        MessageBox.Show("Account chosen is not of the same branch as lead account branch." & Strings.Chr(13) & Strings.Chr(10) & "Please select another account.", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If

                If cmbMediaType.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a Media Type before proceeding.", "Mandatory Field - " & "Media Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bOKClicked = False
                    '<Pankaj PN:38806>
                    SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)
                    Me.cmbMediaType.Focus()
                    '</Pankaj PN:38806>
                    Exit Sub
                End If

                ' If Issuer combo is visible then it is mandatory
                If cboIssuer.Visible AndAlso cboIssuer.SelectedIndex = -1 Then
                    MessageBox.Show("Please select an Issuer before proceeding.",
                                    "Invalid " & "Issuer",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    cboIssuer.Focus()
                    m_bOKClicked = False
                    Exit Sub
                End If

                'DC290704 PN11262 must enter amount if not via Insurer Payment
                'AR20050125 - PN18271
                If gPMFunctions.ToSafeDouble(txtAmount.Text) = 0 AndAlso Not (ViaInsurerPayment OrElse ViaClaimPayment) Then
                    MessageBox.Show("Please enter Amount before proceeding.", "Mandatory Field - " & "Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bOKClicked = False
                    '<Pankaj PN:38806>
                    SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)
                    Me.txtAmount.Focus()
                    '</Pankaj PN:38806>
                    Exit Sub
                End If

                If gPMFunctions.ToSafeDouble(txtSplitTotal.Text) = 0 AndAlso IsSplitReceipt Then
                    MessageBox.Show("Please enter the Split Total before proceeding.", "Invalid " & "Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bOKClicked = False
                    SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)
                    Me.txtSplitTotal.Focus()
                    Exit Sub
                End If

                If gPMFunctions.ToSafeDouble(txtAmount.Text) + gPMFunctions.ToSafeDouble(m_dSplitReceiptRunningTotal) > gPMFunctions.ToSafeDouble(txtSplitTotal.Text) AndAlso IsSplitReceipt Then
                    MessageBox.Show("The amount you entered will exceed the total amount by " & Format((gPMFunctions.ToSafeDouble(txtAmount.Text) + gPMFunctions.ToSafeDouble(m_dSplitReceiptRunningTotal)) - gPMFunctions.ToSafeDouble(txtSplitTotal.Text), "0.00") & " . Please re-enter.", "Invalid " & "Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bOKClicked = False
                    SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)
                    Me.txtSplitTotal.Focus()
                    Exit Sub
                End If

                If cboReceiptType.SelectedIndex = -1 AndAlso CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    MessageBox.Show("Please select a Receipt Type before proceeding.", "Invalid " & "Receipt Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bOKClicked = False
                    Exit Sub
                End If

                If txtCollectionDate.Visible Then
                    If CDate(txtCollectionDate.Text) > DateTime.Today Then
                        MessageBox.Show("Collection Date is not allowed to be greater than the System Date.",
                                        "Invalid " & "Collection date",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)
                        txtCollectionDate.Focus()
                        m_bOKClicked = False
                        Exit Sub
                    End If
                    If CDate(txtCollectionDate.Text) < DateTime.Today AndAlso txtComments.Text.Trim() = "" Then
                        MessageBox.Show("Comments should be entered for a Backdated Collection Date.", "Invalid " & "Comments", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtComments.Focus()
                        m_bOKClicked = False
                        Exit Sub
                    End If

                    If txtChequeDate.Text <> "" Then

                        If CDate(txtChequeDate.Text) > DateTime.Today Then
                            MessageBox.Show("Cheque Date is not allowed to be greater than the System Date.",
                                            "Invalid " & "Cheque Date",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error)
                            txtCollectionDate.Focus()
                            m_bOKClicked = False
                            Exit Sub
                        End If

                        If CDate(txtChequeDate.Text) > CDate(txtCollectionDate.Text) Then
                            MessageBox.Show("Cheque date is not allowed to be greater than the Collection Date.",
                                            "Invalid " & "Cheque Date",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error)
                            txtCollectionDate.Focus()
                            m_bOKClicked = False
                            Exit Sub
                        End If

                        If DateAndTime.DateDiff("d", CDate(txtChequeDate.Text),
                                                DateTime.Today,
                                                FirstDayOfWeek.Sunday,
                                                FirstWeekOfYear.Jan1) >= 150 Then
                            MessageBox.Show("Cheque date more than 5 months prior to the system date is not allowed.",
                                            "Invalid " & "Cheque Date",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error)
                            txtCollectionDate.Focus()
                            m_bOKClicked = False
                            Exit Sub
                        End If

                        If DateAndTime.DateDiff("d", CDate(txtChequeDate.Text),
                                                DateTime.Today,
                                                FirstDayOfWeek.Sunday,
                                                FirstWeekOfYear.Jan1) > 90 Then
                            If MessageBox.Show("Cheque date more than 3 months prior to the system date. Do You Want To Continue?",
                                               "Invalid " & "Cheque Date",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                                txtCollectionDate.Focus()
                                m_bOKClicked = False
                                Exit Sub
                            End If
                        End If
                    End If
                End If

                'RKS PN13444
                'Validating Underwriting Year Field
                If m_bOptionUnderwritingYear Then
                    If m_bSystemOptionUWYearMandatory Then
                        If cboUnderwritingYear.ListIndex < 1 Then
                            MessageBox.Show("Please select an Underwriting Year before proceeding.",
                                            "Invalid " & "Underwriting Year",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error)
                            m_bOKClicked = False
                            '<Pankaj PN:38806>
                            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 4)
                            Me.cboUnderwritingYear.Focus()
                            '</Pankaj PN:38806>
                            Exit Sub
                        End If
                    End If
                End If 'RKS PN13444


                If cboTaxBand.SelectedIndex > 0 Then
                    If ToSafeCurrency(txtTaxAmount.Text) = 0 Then
                        MessageBox.Show("Please enter some Tax Amount..",
                                        "Invalid Tax Amount",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        txtTaxAmount.Focus()
                        Exit Sub
                    End If
                    If ToSafeCurrency(txtTaxAmount.Text) < 0 Then
                        MessageBox.Show("You cannot enter a negative amount in the Tax Amount field.",
                                        "Invalid Tax Amount",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        txtTaxAmount.Focus()
                        Exit Sub
                    End If

                    If ToSafeCurrency(txtTaxAmount.Text) > ToSafeCurrency(txtAmount.Text) Then
                        MessageBox.Show("Tax Amount should be less than or equal to Amount.",
                                        "Invalid Tax Amount",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        txtTaxAmount.Focus()
                        Exit Sub
                    End If
                Else
                    If ToSafeCurrency(txtTaxAmount.Text) <> 0 Then
                        MessageBox.Show("Please select tax band.",
                                        "Invalid Tax Amount",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                        cboTaxBand.Focus()
                        Exit Sub
                    End If

                End If

                nMediaID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)
                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType,
                                                        iLookUpID:=nMediaID,
                                                        sLookUpcode:=sPaymentMethod)

                If m_lReturn = PMEReturnCode.PMFalse Then
                    MessageBox.Show("The " & (IIf(((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)), "payment", "receipt")) &
                                    " details will not be saved", "Invalid Media Type", MessageBoxButtons.OK, MessageBoxIcon.Error Or MessageBoxIcon.Information)
                    cmdCancel_Click(cmdCancel, New EventArgs())
                    m_bOKClicked = False
                    Exit Sub
                End If

                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    'sw Added for front office receipting
                    nLookUpID = VB6.GetItemData(cboReceiptType, Me.cboReceiptType.SelectedIndex)
                    m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=nLookUpID,
                                                                  r_sReceiptTypeAlias:=sReceiptCode,
                                                                  r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
                    ' Pass all the latest values to uctACTCreditCard - except AccountID,
                    ' MediaTypeIssuerID and MediaTypeID which are passed as soon as they are set
                    ' (as they effect how the control looks and data it has loaded)

                    If IsSplitReceipt AndAlso Not IsLeadAccount Then
                        'Ignore validation for non lead account.
                        'As the values in split will be same as that of Lead
                    Else
                        With uctReceiptCC
                            .InsuranceFileCnt = m_lInsuranceFileCnt
                            .CCAmount = CDec(txtAmount.Text.Trim())
                            .CCCurrencyID = CurrencyID
                            .CCAddress1 = Address1.Trim()
                            .CCPostcode = PostalCode.Trim()
                            If .Validate() <> PMEReturnCode.PMTrue Then
                                SSTabHelper.SetSelectedIndex(tabMainTab, TAB_RECEIPT)
                                m_bOKClicked = False
                                Exit Sub
                            End If
                        End With
                    End If
                End If

                If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                    (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then

                    nLookUpID = VB6.GetItemData(Me.cboPaymentType,
                                                Me.cboPaymentType.SelectedIndex)

                    m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupPaymentTypeTable,
                                                            iLookUpID:=nLookUpID,
                                                            sLookUpcode:=sLookUpcode)
                    ' If PaymentType is CLAIM then set flag on uctACTCreditCard (for validation reasons)
                    uctPaymentCC.IsClaimPaymentType = sLookUpcode = kClaimPaymentType
                    ' Pass all the latest values to uctACTCreditCard - except AccountID,
                    ' MediaTypeIssuerID and MediaTypeID which are passed as soon as they are set
                    ' (as they effect how the control looks and data it has loaded)
                    With uctPaymentCC
                        .InsuranceFileCnt = m_lInsuranceFileCnt
                        .CCAmount = CDec(txtAmount.Text.Trim())
                        .CCCurrencyID = CurrencyID
                        .CCAddress1 = Address1.Trim()
                        .CCPostcode = PostalCode.Trim()
                        If .Validate() <> PMEReturnCode.PMTrue Then
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_PAYMENT)
                            m_bOKClicked = False
                            Exit Sub
                        End If
                    End With
                End If

                ' Set the interface status.
                m_lStatus = PMEReturnCode.PMOK
                ' Ensure any Lost Focus events have been processed
                ' prior to continuing with OK
                If Not iPMFunc.ForceLostFocus(cmdOK) Then
                    m_bOKClicked = False
                    Exit Sub
                End If

                If m_bReverseCashDrawerListItem Then
                    If ProcessReversal() <> PMEReturnCode.PMTrue Then
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If

                ' Account No is mandatory
                If (AccountID = 0) OrElse (uctAccountLookup.Text.Trim() = "") Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field.",
                                    "Mandatory Field - Account",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    m_lStatus = PMEReturnCode.PMCancel
                    uctAccountLookup.Focus()
                    SSTabHelper.SetSelectedIndex(tabMainTab, TAB_DETAILS)
                    m_bOKClicked = False
                    Exit Sub
                End If

                ' Media Ref is mandatory
                If m_bIsMediaRefMandatory AndAlso txtMediaRef.Text.Trim() = "" Then
                    lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
                    MessageBox.Show("This is a mandatory field. You must enter data in this field.", "Mandatory Field - Media Reference.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    txtMediaRef.Focus()
                    m_bOKClicked = False
                    Exit Sub
                End If

                ' Check mandatory fields

                m_lReturn = m_oFormFields.CheckMandatoryControls()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Select Case Me.ActiveControl.Name
                        Case "cmbMediaType", "txtTransDate", "txtTendered", "txtAmount"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_DETAILS)
                            If Me.ActiveControl.Name = "txtAmount" Then
                                txtAmount_Leave(txtAmount, New EventArgs())
                            End If
                        Case "txtPaymentReason"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_PAYMENT)
                            txtPaymentReason.Focus()
                        Case "txtPaymentAccountCode"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_PAYMENT)
                            txtPaymentAccountCode.Focus()
                        Case "txtName", "txtChequeDate", "txtReverseDate", "cboReversalReason", "txtReason"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_RECEIPT)
                        Case "txtFurtherDetails"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_ADDRESS)
                            txtFurtherDetails.Focus()
                        Case "txtContactName"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_ADDRESS)
                            txtContactName.Focus()
                        Case "txtPayeeName"
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_PAYMENT)
                            txtPayeeName.Focus()
                    End Select
                    m_lStatus = PMEReturnCode.PMCancel
                    m_bOKClicked = False
                    Exit Sub
                End If

                'sw front office receipting
                If Not ValidateMediaDetails() Then
                    m_bOKClicked = False
                    Exit Sub
                End If

                m_lReturn = CheckChangeOfAccount()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    m_lStatus = PMEReturnCode.PMCancel
                    m_bOKClicked = False
                    Exit Sub
                End If

                If (g_bChequeProduction) AndAlso
                    ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                     (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) AndAlso
                 sPaymentMethod = ACMediaTypeBank AndAlso
                 (txtOurRef.Text = "") Then
                    MessageBox.Show("This is a mandatory field for Cheque Production Please Enter a " &
                                    "reference.", "Mandatory Field - Our Ref", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    SSTabHelper.SetSelectedIndex(tabMainTab, TAB_DETAILS)
                    lblOurRef.Font = VB6.FontChangeBold(lblOurRef.Font, True)
                    txtOurRef.Focus()
                    m_lStatus = PMEReturnCode.PMCancel
                    m_bOKClicked = False
                    Exit Sub
                End If

                If cmbMediaType.SelectedIndex >= 0 AndAlso
                    (CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts OrElse
                     CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Then
                    If GetIsBankingFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                        ProcessBankBatch(txtBankRef.Text, iBatchID)
                        If iBatchID = 0 Then
                            Exit Sub
                        End If
                    End If
                End If
                'DD 17/05/2002
                'Validate the Media Reference by ensuring that it hasn't already
                'been used in period defined by the rules

                'DD 28/08/2003: Corrected bug to stop validating against
                'current record

                ' RVH 08/11/03 CQ2172 - START : Don't do media validation if the user is
                '                               calling stop cheque or cancel cheque.
                If (ActionKey <> gACTLibrary.ACTCancelCheque) AndAlso
                    (ActionKey <> gACTLibrary.ACTStopCheque) Then

                    sMediaRef = m_oFormFields.UnformatControl(txtMediaRef)
                    m_lReturn = m_oBusiness.ValidateMediaReference(v_lCashListID:=CashlistID,
                                                                   v_lCashlistItemID:=CashlistitemID,
                                                                   v_lMediaTypeID:=VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex),
                                                                   v_sMediaRef:=sMediaRef,
                                                                   r_bValid:=bValid,
                                                                   r_bValidateUI:=bValidateUI,
                                                                   r_iPeriodMonths:=nPeriodMonths)
                    If m_lReturn = PMEReturnCode.PMTrue Then
                        'DD 28/08/2003: Now check the list as we may have uncommitted records
                        If bValid AndAlso bValidateUI Then
                            If Information.IsArray(m_oListForm.ListData) Then
                                oList = m_oListForm.ListData
                                nRows = oList.GetUpperBound(1)
                                For lRow As Integer = 0 To nRows
                                    If CStr(oList(ACSubMediaRef, lRow)) = sMediaRef AndAlso
                                        sMediaRef.Trim() <> "" AndAlso
                                        (CDbl(oList(ACSubCashListItemID, lRow)) <> CashlistitemID OrElse
                                         (CashlistitemID = 0 AndAlso
                                          m_oListForm.lvwListDetails.Items(lRow).Tag <> lRow) OrElse
                                      m_iTask = PMEComponentAction.PMAdd) Then
                                        bValid = False
                                    End If
                                Next lRow
                            End If
                        End If

                        If Not bValid Then
                            'Its failed the validation
                            'get the message and insert the months
                            sFailedMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedValidationError,
                                                                     iDataType:=PMEResourseFileDataType.PMResString,
                                                                     bResFile:=My.Resources.ResourceManager))
                            sFailedMessage = sFailedMessage.Replace("<TYPE>",
                                                                    VB6.GetItemString(cmbMediaType, cmbMediaType.SelectedIndex))
                            sFailedMessage = sFailedMessage.Replace("<MONTHS>",
                                                                    CStr(nPeriodMonths))
                            MessageBox.Show(sFailedMessage, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID,
                                                                                    lId:=ACFailedValidationTitle,
                                                                                    iDataType:=PMEResourseFileDataType.PMResString,
                                                                                    bResFile:=My.Resources.ResourceManager)),
                                                                            MessageBoxButtons.OK,
                                                                            MessageBoxIcon.Exclamation)
                            m_lStatus = PMEReturnCode.PMCancel
                            m_bOKClicked = False
                            Exit Sub
                        End If
                    Else
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="Failed to Validate the Media Reference",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="cmdOK_Click",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        m_lStatus = PMEReturnCode.PMCancel
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If
                ' RVH 08/11/03 CQ2172 - END

                'sw added for payment maintenance
                Select Case ActionKey
                    Case gACTLibrary.ACTEditCheque
                        'update the neccessary tables
                        m_lReturn = m_oBusiness.UpdateDBForMediaRefChange(CashlistitemID,
                                                                          PMUserid, MediaRef,
                                                                          txtMediaRef.Text.Trim(),
                                                                          OurRef, TheirRef)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            MessageBox.Show("Unable to update the database with the new " &
                                            "Media Reference", "Update Media Ref",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation)
                        End If
                        Me.Hide()
                        m_bOKClicked = False
                        Exit Sub
                    Case gACTLibrary.ACTCancelCheque
                        nStatusID = VB6.GetItemData(Me.cboPaymentStatus,
                                                    Me.cboPaymentStatus.SelectedIndex)
                        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupPaymentStatus,
                                                                iLookUpID:=nStatusID,
                                                                sLookUpcode:=sStatusCode)
                        If (sStatusCode = ACStatusIssued AndAlso
                            chkInPossession.CheckState = CheckState.Checked) OrElse
                        (sStatusCode = ACStatusStopped AndAlso
                         (Information.IsDate(Stopconfirmationdate) AndAlso
                          CStr(Stopconfirmationdate) <> "00:00:00")) Then

                            m_lReturn = m_oBusiness.UpdateDBForCancelCheque(m_lTransDetailID,
                                                                            CashlistitemID,
                                                                            txtPaymentReason.Text)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                MessageBox.Show("An error occured, the payment will not be cancelled.",
                                                "Error: Unable to update database.",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information)
                            End If
                            Me.Hide()
                            m_bOKClicked = False
                            Exit Sub
                        Else
                            MessageBox.Show("This form of payment cannot be altered." &
                                            Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "Process Aborted.", "Error: Payment Details cannot be altered",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information)
                        End If
                        Me.Hide()
                        m_bOKClicked = False
                        Exit Sub

                    Case gACTLibrary.ACTStopCheque
                        nStatusID = VB6.GetItemData(Me.cboPaymentStatus, Me.cboPaymentStatus.SelectedIndex)
                        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupPaymentStatus,
                                                                iLookUpID:=nStatusID,
                                                                sLookUpcode:=sStatusCode)
                        If sStatusCode = ACStatusIssued Then
                            'stop cheque roadmap, perform initial stop
                            If txtPaymentReason.Text.Trim() <> "" Then
                                'perform the stop operations
                                m_lReturn = m_oBusiness.UpdateDBForStopCheque(CashlistitemID,
                                                                              txtPaymentReason.Text.Trim())

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    MessageBox.Show("Unable to update the database with the Stop Cheque details",
                                                    "Update Stop Cheque",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Exclamation)
                                    m_oListForm.bStopCheque = False
                                End If

                                m_oListForm.bStopCheque = True
                                Me.Hide()
                                m_bOKClicked = False
                                Exit Sub
                            Else
                                MessageBox.Show("Please enter a Stop Reason before you continue.",
                                                "Error: Stop Reason must be supplied",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Information)
                                m_bOKClicked = False
                                Exit Sub
                            End If
                            Me.Hide()
                            m_bOKClicked = False
                            Exit Sub

                        ElseIf sStatusCode = ACStatusStopRequested Then
                            'stop cheque roadmap, stop has already been requested, enter confirmation date, etc.
                            m_lReturn = m_oBusiness.UpdateDBForStopChequeConfirm(CashlistitemID,
                                                                                 CDate(txtConfirmation.Text))

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                MessageBox.Show("Unable to update the database with the Stop Cheque confirmation " &
                                                "details", "Update Confirmation Date",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Exclamation)
                            End If
                            Me.Hide()
                            m_bOKClicked = False
                            Exit Sub

                        End If
                End Select

                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then

                    If bReceiptTypeIsInstalmentBased Then
                        If Conversion.Val(txtAmount.Text) = 0 OrElse Not CheckInstalmentsChosen() Then
                            MessageBox.Show("Please select at least one instalment before you continue.",
                                            "Mandatory Field: Instalments",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Exclamation)
                            m_bOKClicked = False
                            SSTabHelper.SetSelectedIndex(tabMainTab, TAB_INSTALMENT)
                            Exit Sub
                        Else
                            ''If Not ViaInsurerPayment Then
                            m_lReturn = ProcessPartialInstalments()
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                                m_bOKClicked = False
                                SSTabHelper.SetSelectedIndex(tabMainTab, TAB_INSTALMENT)
                                Exit Sub
                            End If
                            ''End If
                        End If
                    End If
                End If
                If Not (CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts AndAlso
                        bReceiptTypeIsInstalmentBased) Then
                    'Show frmConversions form and retrieve any overridden rates.
                    m_lReturn = GetCurrencyConversions()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        m_bOKClicked = False
                        Exit Sub
                    End If
                End If

                'process next set of actions dependant on the interface task
                m_lReturn = ProcessCommand()

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    m_bOKClicked = False
                    Exit Sub
                End If

                ' Check the return value.
                If m_lReturn = PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                m_bOKClicked = False
            End If

            'Start PN: 62476
            If cmbMediaType.Text.Contains("EFT") AndAlso txtMediaRef.Text.Trim() <> "" Then
                For lCount As Integer = m_vMediaResultArray.GetLowerBound(1) To m_vMediaResultArray.GetUpperBound(1)
                    If VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex) = CDbl(m_vMediaResultArray(ACMediaTypeID, lCount)) Then
                        lMediaTypeIndex = lCount
                        Exit For
                    End If
                Next lCount

                If Strings.Len(m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex)) > 0 Then
                    If CDbl(m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex)) <> 0 AndAlso
                        CStr(m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex)) <> "" Then

                        Dim temp_oPlicyNumMaint As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oPlicyNumMaint,
                                                                 "bSIRPolicyNumMaint.Business",
                                                                 vInstanceManager:=PMGetViaClientManager)
                        oPlicyNumMaint = temp_oPlicyNumMaint

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("cmdOK_Click",
                                                    "Initilization of the Component bSIRPolicyNumMaint.Business ",
                                                    PMELogLevel.PMLogOnError)
                        End If
                    End If

                    m_lReturn = oPlicyNumMaint.InsertMediaReference(v_iSourceID:=g_iSourceID,
                                                                    v_iNumberingScheme:=m_vMediaResultArray(ACMediaNumberingScheme, lMediaTypeIndex))

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("cmdOK_Click",
                                                "The method GenerateMediaReference calling failed",
                                                PMELogLevel.PMLogOnError)
                    End If
                End If

            End If
            'End of PN: 62476

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            m_bOKClicked = False
            Exit Sub
        End Try

    End Sub
    ''' <summary>
    ''' ProcessPartialInstalments
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessPartialInstalments() As Integer

        Dim result As Integer = 0
        Dim sLookUpcode As String = ""
        Dim sInstalmentRef As String = ""
        Dim bMultiplePlans As Boolean
        Dim lInstalmentNumber, lArrayElement As Integer
        Dim cActualOverallPlanTotal As Decimal
        Dim iBaseCurrencyID As Integer
        Dim fDifference As frmDifference
        Dim iReceiptDifferenceOption As Integer
        Dim iTransCurrencyID As Integer
        Dim bAllSelected As Boolean
        Dim cDifference, cReceiptBaseAmount, cInstallmentReceivedAmount, cTotalReceivedAmount As Decimal
        Dim cTempRecpBaseAmount As Decimal
        Dim cTempInstlBaseTotal As Decimal
        Dim nWriteOffReasonId As Integer
        Dim iInstalmentCurrencyID As Integer = 0
        Dim dXRate As Double
        Dim crCurrencyAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = CreateCurrencyConvert()


            result = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=CompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Here we need to taken cActualOverallPlanTotal to store the exact overall plan amount for reciept in case of partial payment.
            cActualOverallPlanTotal = CDec(SSOveralPlanTotal.Text)

            Dim dTemp As Decimal, dTotalAmount As Decimal
            For dRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                If InstalmentArray(ACInstalmentFlagElement, dRow) Then
                    dTemp = dTotalAmount

                    If iInstalmentCurrencyID = 0 Then
                        m_oBusiness.GetSchemeCurrency(InstalmentArray(ACPremFinanceCnt, dRow), InstalmentArray(ACPremFinanceVersion, dRow), iInstalmentCurrencyID, dXRate)
                    End If
                    m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=iInstalmentCurrencyID, v_crCurrencyAmountFrom:=InstalmentArray(ACInstalmentAmount, dRow), v_lCurrencyIdTo:=CurrencyID, r_crCurrencyAmountTo:=crCurrencyAmount, dt_EffectiveDate:=Transactiondate, v_lCompanyId:=CompanyID)

                    dTotalAmount += Math.Round(crCurrencyAmount, 2)
                    If dTotalAmount > ToSafeDouble(txtAmount.Text) Then
                        If dTemp >= ToSafeDouble(txtAmount.Text) And dTotalAmount Then
                            InstalmentArray(ACInstalmentFlagElement, dRow) = gPMConstants.PMEReturnCode.PMFalse
                            cActualOverallPlanTotal -= InstalmentArray(ACInstalmentReceiptAmount, dRow)
                            If iInstalmentCurrencyID = 0 Then
                                m_oBusiness.GetSchemeCurrency(InstalmentArray(ACPremFinanceCnt, dRow), InstalmentArray(ACPremFinanceVersion, dRow), iInstalmentCurrencyID, dXRate)
                            End If
                            m_cInstalmentBaseTotal -= (InstalmentArray(ACInstalmentAmount, dRow) * dXRate)
                        End If
                    End If
                End If
            Next
            sInstalmentRef = ""
            For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                    If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) <> sInstalmentRef Then
                        If sInstalmentRef = "" Then

                            sInstalmentRef = CStr(InstalmentArray(ACInstalmentPlanRef, lRow))
                            iReceiptDifferenceOption = CInt(InstalmentArray(ACReceiptDifferenceOption, lRow))
                        Else
                            bMultiplePlans = True
                            Exit For
                        End If
                    End If
                End If
            Next

            If ToSafeCurrency(txtAmount.Text) > ToSafeCurrency(SSOveralPlanTotal.Text) AndAlso iReceiptDifferenceOption = 1 Then
                If (MsgBox("The amount entered is greater than the total value of instalments " &
                        "selected. ", vbCritical, "Receipt Processing") <> MsgBoxResult.Ok) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            If CDec(txtAmount.Text) > CDec(cActualOverallPlanTotal) Then
                If cmbMediaType.SelectedIndex <> -1 Then

                    With cmbMediaType

                        result = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode)

                    End With
                End If
                If sLookUpcode <> ACMediaTypeCash Then
                    If (MessageBox.Show("The amount entered is greater than the total value of instalments " &
                                    "selected. ", "Payment Processing", MessageBoxButtons.OK, MessageBoxIcon.Error) <> DialogResult.OK) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            ElseIf CDec(txtAmount.Text) < CDec(cActualOverallPlanTotal) Then
                If Information.IsArray(InstalmentArray) Then


                    For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                        If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                            sInstalmentRef = CStr(InstalmentArray(ACInstalmentPlanRef, lRow))
                            iTransCurrencyID = ToSafeInteger(InstalmentArray(ACTransCurrencyCode, lRow))
                            Exit For
                        End If
                    Next


                    For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                        If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                            If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) <> sInstalmentRef Then
                                bMultiplePlans = True
                                Exit For
                            End If
                        End If
                    Next
                End If
                If bMultiplePlans Then
                    If (MessageBox.Show("The amount entered is less than the total value of instalments " &
                                    "selected.  Please reselect instalments from a single instalment plan.", "Receipt Processing", MessageBoxButtons.OK, MessageBoxIcon.Error) <> DialogResult.OK) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If Not bMultiplePlans Then
                bAllSelected = True

                For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)

                    If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) = sInstalmentRef Then
                        If InstalmentArray(ACInstalmentFlagElement, lRow) <> gPMConstants.PMEReturnCode.PMTrue Then
                            bAllSelected = False
                            Exit For
                        End If
                    End If
                Next
            End If


            fDifference = New frmDifference()

            fDifference.ReceiptDifferenceOption = iReceiptDifferenceOption
            fDifference.MultiplePlans = bMultiplePlans
            fDifference.AllSelected = bAllSelected
            fDifference.ReceiptCompanyID = CompanyID
            fDifference.ReceiptCurrencyID = CurrencyID
            fDifference.ReceiptTransactionDate = Transactiondate
            fDifference.ReceiptAmount = CDec(txtAmount.Text)
            fDifference.InstallmentsAmount = m_cInstalmentBaseTotal
            fDifference.StartPosition = FormStartPosition.CenterParent

            'In Broking, this form is hidden and write-off is always chosen.
            If m_oBusiness.UnderwritingOrAgency <> "U" Then
                fDifference.Hidden = True
            Else
                fDifference.Hidden = False
            End If

            If iReceiptDifferenceOption = 1 Then
                fDifference.cboWriteOffReason.Visible = False
                fDifference.lblWOffDifference.Visible = False
            Else
                fDifference.cboWriteOffReason.Visible = True
                fDifference.lblWOffDifference.Visible = True
            End If

            fDifference.Hidden = False
            fDifference.frmDifferenceLoad()
            If fDifference.Status = gPMConstants.PMEReturnCode.PMTrue Then
                If iBaseCurrencyID <> CurrencyID Then
                    fDifference.ShowDialog()
                ElseIf iReceiptDifferenceOption <> 1 And iBaseCurrencyID = CurrencyID Then
                    fDifference.ShowDialog()
                End If
            End If

            cDifference = fDifference.DifferenceAmount
            If iReceiptDifferenceOption = 0 Then
                m_lReturn = GetUserWriteOffLimit(cDifference, iBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessPartialInstalments = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            If fDifference.Status = gPMConstants.PMEReturnCode.PMCancel Then
                If bMultiplePlans Then
                    MessageBox.Show("Multiple plans cannot be selected unless receiving the exact amount ", "Receipt Processing", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                result = gPMConstants.PMEReturnCode.PMCancel
                fDifference = Nothing
                Return result
            ElseIf fDifference.ReceiptDifferenceOption = 1 And ToSafeCurrency(txtAmount.Text) > ToSafeCurrency(cActualOverallPlanTotal) Then
                MsgBox("The amount entered is greater than the total value of instalments " & "selected. ", vbCritical, "Receipt Processing")
                ProcessPartialInstalments = gPMConstants.PMEReturnCode.PMFalse
                fDifference = Nothing
                Exit Function
            Else
                'Retrieve values from form
                cReceiptBaseAmount = fDifference.ReceiptBaseAmount
                cDifference = fDifference.DifferenceAmount
                If fDifference.ReceiptDifferenceOption = 0 Then
                    If iReceiptDifferenceOption <> 1 Then
                        result = GetUserWriteOffLimit(cDifference, iBaseCurrencyID)

                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            ProcessPartialInstalments = gPMConstants.PMEReturnCode.PMFalse
                            Exit Function
                        End If
                    End If
                End If
                iReceiptDifferenceOption = fDifference.ReceiptDifferenceOption
                nWriteOffReasonId = fDifference.WriteOffReasonID
                fDifference = Nothing
            End If

            If cDifference <> 0 And iReceiptDifferenceOption = 1 Then
                If cDifference > 0 Or bAllSelected Then

                    lInstalmentNumber = -1
                    Dim dTotal As Decimal
                    For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                        If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                            If lInstalmentNumber = -1 Then

                                lInstalmentNumber = CInt(InstalmentArray(ACInstalmentNumber, lRow))
                            End If
                            If (dTotal + CDec(InstalmentArray(ACInstalmentAmount, lRow))) <= CDec(txtAmount.Text) Then
                                dTotal += CDec(InstalmentArray(ACInstalmentAmount, lRow))
                            End If
                            If CInt(InstalmentArray(ACInstalmentNumber, lRow)) >= lInstalmentNumber Then

                                lInstalmentNumber = CInt(InstalmentArray(ACInstalmentNumber, lRow))
                                lArrayElement = lRow
                            End If
                        End If
                    Next


                    'Make highest selected installment a partial payment.
                    'If all selected and difference is less than zero then the partial payment amount will be an overpayment.
                    m_oBusiness.GetSchemeCurrency(InstalmentArray(ACPremFinanceCnt, lArrayElement), InstalmentArray(ACPremFinanceVersion, lArrayElement),
                                                  iInstalmentCurrencyID, dXRate)



                    If iInstalmentCurrencyID = CurrencyID Then
                        Dim cTransCurrDifference As Double = ToSafeDouble(cActualOverallPlanTotal) - ToSafeDouble(txtAmount.Text)
                        InstalmentArray(ACPartialPaymentAmount, lArrayElement) = CDec(InstalmentArray(ACInstalmentAmount, lArrayElement)) - cTransCurrDifference
                        InstalmentArray(ACInstalmentReceiptAmount, lArrayElement) = InstalmentArray(ACInstalmentReceiptAmount, lArrayElement) - (ToSafeDouble(cActualOverallPlanTotal) - ToSafeDouble(txtAmount.Text))
                    Else
                        cDifference = cDifference / dXRate
                        InstalmentArray(ACPartialPaymentAmount, lArrayElement) = CDec(InstalmentArray(ACInstalmentAmount, lArrayElement)) - cDifference

                        InstalmentArray(ACInstalmentReceiptAmount, lArrayElement) = InstalmentArray(ACInstalmentReceiptAmount, lArrayElement) - (ToSafeDouble(cActualOverallPlanTotal) - ToSafeDouble(txtAmount.Text))
                    End If

                    InstalmentArray(ACInstalmentBaseAmmount, lArrayElement) = InstalmentArray(ACInstalmentReceiptAmount, lArrayElement)
                    InstalmentArray(ACReceiptDifferenceOption, lArrayElement) = 1

                Else

                    lInstalmentNumber = -1

                    For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)

                        If CStr(InstalmentArray(ACInstalmentPlanRef, lRow)) = sInstalmentRef Then
                            If InstalmentArray(ACInstalmentFlagElement, lRow) <> gPMConstants.PMEReturnCode.PMTrue Then
                                If lInstalmentNumber = -1 Then

                                    lInstalmentNumber = CInt(InstalmentArray(ACInstalmentNumber, lRow))
                                End If

                                If CInt(InstalmentArray(ACInstalmentNumber, lRow)) <= lInstalmentNumber Then

                                    lInstalmentNumber = CInt(InstalmentArray(ACInstalmentNumber, lRow))
                                    lArrayElement = lRow
                                End If
                            End If
                        End If
                    Next

                    InstalmentArray(ACInstalmentFlagElement, lArrayElement) = gPMConstants.PMEReturnCode.PMTrue
                    InstalmentArray(ACPartialPaymentAmount, lArrayElement) = cDifference * -1
                End If

            End If

            If cDifference <> 0 And iReceiptDifferenceOption = 0 Then
                Dim cReceiptAmountBalance As Decimal = cReceiptBaseAmount
                For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
                    If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                        InstalmentArray(ACReceiptDifferenceOption, lRow) = iReceiptDifferenceOption
                        InstalmentArray(kWriteOffReasonId, lRow) = nWriteOffReasonId
                        If CDec(InstalmentArray(ACInstalmentAmount, lRow)) < cReceiptAmountBalance Then
                            cInstallmentReceivedAmount = InstalmentArray(ACInstalmentAmount, lRow)
                            cReceiptAmountBalance -= CDec(InstalmentArray(ACInstalmentAmount, lRow))
                        Else
                            cInstallmentReceivedAmount = cReceiptAmountBalance
                            cReceiptAmountBalance = 0
                        End If
                        cTotalReceivedAmount += cInstallmentReceivedAmount

                        InstalmentArray(ACPartialPaymentAmount, lRow) = cInstallmentReceivedAmount
                        lArrayElement = lRow
                    End If
                Next

                If cTotalReceivedAmount <> cReceiptBaseAmount AndAlso iBaseCurrencyID = CurrencyID Then

                    cInstallmentReceivedAmount = CDec(InstalmentArray(ACPartialPaymentAmount, lArrayElement)) + (cReceiptBaseAmount - cTotalReceivedAmount)

                    InstalmentArray(ACPartialPaymentAmount, lArrayElement) = cInstallmentReceivedAmount
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ProcessPartialInstalments Failed", ACApp, ACClass, "ProcessPartialInstalments", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'ProcessReversal
    'Function to process a reversal request on a given receipt
    Private Function ProcessReversal() As Integer
        Dim result As Integer = 0
        Dim iLookUpID As Integer
        Dim sReversalCode, sFailureReason As String

        Const cUndefinedReason As String = "OTHER"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboReversalReason.Text = "" Or cboReversalReason.SelectedIndex = -1 Then
                MessageBox.Show("This is a mandatory field. You must enter data in this field.", "Mandatory Field - Reversal Reason", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SSTabHelper.SetSelectedIndex(tabMainTab, TAB_RECEIPT)
                cboReversalReason.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iLookUpID = VB6.GetItemData(cboReversalReason, cboReversalReason.SelectedIndex)

            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupReverseType, iLookUpID:=iLookUpID, sLookUpcode:=sReversalCode)

            If sReversalCode = cUndefinedReason And txtReason.Text = "" Then
                MessageBox.Show("Please enter a description of the reason for the Reversal", "Warning: Insufficient Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SSTabHelper.SetSelectedIndex(tabMainTab, TAB_RECEIPT)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If MessageBox.Show("Are you sure that you want to Reverse this receipt?", "Warning: Reverse Receipt?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' KG 08/08/03 - CQ1030 - Branch/SubBranch work.Pass CashListDrawerID.

            m_lReturn = m_oBusiness.ReverseReceipt(v_lCashlistItemID:=CashlistitemID, r_sFailureReason:=sFailureReason, v_vCashListDrawerID:=CashDrawerID, sReverseCode:=sReversalCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                If sFailureReason <> "" Then
                    MessageBox.Show("The Reversal failed to process." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sFailureReason, "Warning: Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("The Reversal failed to process", "Warning: Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the ProcessReversal function", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessReversal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateMediaDetails() As Boolean
        Dim result As Boolean = False
        Dim oValidation As bSIRMediaTypeValidation.Business
        Dim vRoundedAmount As Object
        Dim vlCountryID As Object
        Dim vlMediaID As Integer
        Dim sPaymentMethod As String = ""
        Dim vValid, iLookUpID As Integer
        Dim oBusDays As Object
        Dim dMaxDate As Date

        Dim sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode As String

        'Initialising values
        sBankName = ""
        sAddress1 = ""
        sAddress2 = ""
        sAddress3 = ""
        sAddress4 = ""
        sPostalCode = ""
        Dim vValidationMessage As Object
        Dim bValidationOverridable As Boolean

        Try

            Dim iPtr As IntPtr = tabMainTab.Handle
            If txtTendered.Visible Then
                If gPMFunctions.ToSafeCurrency(txtTendered.Text) < gPMFunctions.ToSafeCurrency(txtAmount.Text) Then
                    MessageBox.Show("The amount tendered cannot be less than the amount", "Incorrect Amount Tendered", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtTendered.Text = txtAmount.Text
                    txtTendered_Leave(txtTendered, New EventArgs())
                    Return False
                End If
            End If

            If AddressCountry = 0 Then

                vlCountryID = g_oObjectManager.CountryID
            Else

                vlCountryID = AddressCountry
            End If
            vlMediaID = VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)

            Dim sStrippedString As String = ""
            Dim sMessage, IsValid As String
            iLookUpID = VB6.GetItemData(Me.cmbMediaType, cmbMediaType.SelectedIndex)

            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpcode:=sPaymentMethod)
            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then

                Select Case sPaymentMethod
                    Case ACMediaTypeCash
                        Dim temp_oValidation As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oValidation = temp_oValidation

                        'copy the original amount prior to processing
                        Originalamount = gPMFunctions.ToSafeCurrency(txtAmount.Text)

                        Amount = gPMFunctions.ToSafeCurrency(txtAmount.Text)

                        oValidation.RoundCurrency(vlMediaID, vlCountryID, Amount, vRoundedAmount)

                        txtAmount.Text = vRoundedAmount

                        If gPMFunctions.ToSafeCurrency(txtTendered.Text) = Originalamount Then
                            'tendered = original amount and can therefore be rounded in the same way
                            txtTendered.Text = StringsHelper.Format(vRoundedAmount, "0.00")
                        End If

                        Select Case CDec(txtTendered.Text)
                            Case Is = CDec(txtAmount.Text)
                                'do nothing, validation passes
                                txtChange.Text = "0"
                                result = True
                                oValidation = Nothing
                                Return result
                            Case Is < CDec(txtAmount.Text)
                                If txtTendered.Visible Then
                                    If m_bOKClicked Then
                                        MessageBox.Show("The amount tendered cannot be less than the amount", "Incorrect Amount Tendered", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                    txtChange.Text = "0"
                                    result = False
                                    oValidation = Nothing
                                    Return result
                                Else
                                    result = True
                                    oValidation = Nothing
                                    Return result
                                End If
                            Case Is > CDec(txtAmount.Text)
                                txtChange.Text = CStr(CDec(txtTendered.Text) - CDec(txtAmount.Text))
                                result = True
                                oValidation = Nothing
                                Return result
                        End Select

                    Case ACMediaTypeBank

                        'RESET THE ORIGINAL AMOUNT AS MEDIA HAS CHANGED
                        Originalamount = 0

                        If CDFutureChequeDays > 0 Then

                            Dim temp_oBusDays As Object
                            m_lReturn = g_oObjectManager.GetInstance(temp_oBusDays, "bSIRBusinessDays.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                            oBusDays = temp_oBusDays

                            'call the busdays object which will return the max possible date allowed for a cheque

                            m_lReturn = oBusDays.AddBusinessDaysToDate(DateTime.Today, CDFutureChequeDays, dMaxDate)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = False
                                oValidation = Nothing
                                oBusDays = Nothing
                                Return result
                            End If
                        Else
                            dMaxDate = DateTime.Today
                        End If

                        If Information.IsDate(txtChequeDate.Text) Then
                            If CDate(txtChequeDate.Text) > dMaxDate Then
                                MessageBox.Show("The cheque date supplied is not valid", "Warning: Invalid cheque date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                result = False
                                oValidation = Nothing
                                oBusDays = Nothing
                                Return result
                            End If
                        End If

                        oBusDays = Nothing

                        If txtTendered.Visible Then
                            If CDec(txtAmount.Text) > CDec(txtTendered.Text) Then
                                txtChange.Text = CStr(CDec(txtAmount.Text) > CDec(txtTendered.Text))
                            End If
                        End If

                        result = True
                        oValidation = Nothing
                        Return result

                    Case ACMediaTypeCreditCard

                        'RESET THE ORIGINAL AMOUNT AS MEDIA HAS CHANGED
                        Originalamount = 0

                        Return True

                    Case Else
                        'RESET THE ORIGINAL AMOUNT AS MEDIA HAS CHANGED
                        Originalamount = 0
                        SSTabHelper.SetTabVisible(tabMainTab, TAB_ADDRESS, True)
                        SSTabHelper.SetTabEnabled(tabMainTab, TAB_ADDRESS, True)
                        result = True
                        oValidation = Nothing
                        Return result

                End Select

            ElseIf ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then

                ' Only validate the Account Code if it is visible (it won't be for credit cards)
                If sPaymentMethod <> ACMediaTypeCreditCard Then

                    If AlphanumericValidation(Trim(txtBIC.Text)) <> PMEReturnCode.PMTrue Then
                        MessageBox.Show("Only alphanumeric characters allowed in BIC field.", "Bank Validation",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return PMEReturnCode.PMFalse
                    End If

                    If AlphanumericValidation(Trim(txtIBAN.Text)) <> PMEReturnCode.PMTrue Then
                        MessageBox.Show("Only alphanumeric characters allowed in IBAN field.", "Bank Validation",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return PMEReturnCode.PMFalse
                    End If

                    Dim temp_oValidation2 As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oValidation2, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oValidation = temp_oValidation2

                    sStrippedString = txtPaymentBranchCode.Text.Replace(" ", "") & "|" &
                                      txtPaymentAccountCode.Text.Replace(" ", "")

                    oValidation.ValidateNumber(vlMediaID, vlCountryID, sStrippedString, vValid,
                                               sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode,
                                               vValidationMessage, bValidationOverridable, sBIC:=Trim(txtBIC.Text), sIBAN:=Trim(txtIBAN.Text))

                    'if vValid = false then check for ValidationMessage and store all of
                    'them in a string
                    If Not vValid Then
                        If Information.IsArray(vValidationMessage) Then

                            For iErrCount As Integer = 0 To vValidationMessage.GetUpperBound(0)

                                sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & CStr(vValidationMessage(iErrCount))
                            Next
                        Else
                            'if there is no message then store the generic message
                            sMessage = "Bank details have failed validation"
                        End If

                        'if validation are overridable then show the message with vbYesNo
                        If bValidationOverridable Then
                            sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to override the bank validation?"
                            IsValid = CStr(MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                            If IsValid = System.Windows.Forms.DialogResult.Yes Then

                                If Not (Encoding.Unicode.GetByteCount(sBankName) = 0 And Encoding.Unicode.GetByteCount(sAddress1) = 0 And Encoding.Unicode.GetByteCount(sAddress2) = 0 And Encoding.Unicode.GetByteCount(sAddress3) = 0 And Encoding.Unicode.GetByteCount(sAddress4) = 0 And Encoding.Unicode.GetByteCount(sPostalCode) = 0) Then
                                    txtContactName.Text = sBankName
                                    ContactName = sBankName
                                    uctAddress.AddressLine1 = sAddress1
                                    Address1 = sAddress1
                                    uctAddress.AddressLine2 = sAddress2
                                    Address2 = sAddress2
                                    uctAddress.AddressLine3 = sAddress3
                                    Address3 = sAddress3
                                    uctAddress.AddressLine4 = sAddress4
                                    Address4 = sAddress4
                                    uctAddress.PostCode = sPostalCode
                                    PostalCode = sPostalCode

                                    uctAddress.CountryId = CInt(vlCountryID)

                                    AddressCountry = CInt(vlCountryID)
                                End If
                                result = True
                            Else
                                result = False
                            End If
                        ElseIf Not bValidationOverridable Then
                            MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            result = False
                            'cmdOK.Enabled = False
                        End If
                        '                MsgBox "The Account Code supplied is not valid.", vbOKOnly + vbCritical, _
                        ''                        "Warning: Invalid Account Code"
                        '                ValidateMediaDetails = False
                    Else

                        If Not (Encoding.Unicode.GetByteCount(sBankName) = 0 And Encoding.Unicode.GetByteCount(sAddress1) = 0 And Encoding.Unicode.GetByteCount(sAddress2) = 0 And Encoding.Unicode.GetByteCount(sAddress3) = 0 And Encoding.Unicode.GetByteCount(sAddress4) = 0 And Encoding.Unicode.GetByteCount(sPostalCode) = 0) Then
                            txtContactName.Text = sBankName
                            ContactName = sBankName
                            uctAddress.AddressLine1 = sAddress1
                            Address1 = sAddress1
                            uctAddress.AddressLine2 = sAddress2
                            Address2 = sAddress2
                            uctAddress.AddressLine3 = sAddress3
                            Address3 = sAddress3
                            uctAddress.AddressLine4 = sAddress4
                            Address4 = sAddress4
                            uctAddress.PostCode = sPostalCode
                            PostalCode = sPostalCode

                            uctAddress.CountryId = CInt(vlCountryID)

                            AddressCountry = CInt(vlCountryID)
                        End If
                        result = True
                    End If
                    oValidation = Nothing
                Else
                    result = True
                End If
                Return result
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process ValidateMediaDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMediaDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'sw front office receipting, clear CC details (media type changed)
    Private Sub ClearCreditCardDetails()

        ' Media Type or another important field has been changed (to a value other than Credit Card) so we
        ' must reset and clear all controls on uctACTCreditCard. For Receipts this will disable the controls
        ' and for Payments it will hide the user control.
        If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
            uctReceiptCC.MediaTypeID = 0
            uctReceiptCC.ViewOnlyMode = True
            uctReceiptCC.ClearControls()
        Else
            If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                uctPaymentCC.MediaTypeID = 0
                uctPaymentCC.ViewOnlyMode = True
                uctPaymentCC.ClearControls()
                fraPaymentCreditCard.Visible = False
                fraBank.Visible = True
                fraPayee.Visible = True
            End If
        End If

    End Sub

    'sw front office receipting, clear Cheque details (media type changed)
    Private Sub ClearChequeDetails()
        txtName.Text = ""
        txtName_Leave(txtName, New EventArgs())
        txtChequeDate.Text = ""
        txtChequeDate_Leave(txtChequeDate, New EventArgs())
        cboBank.SelectedIndex = -1
        cboBank_Leave(cboBank, New EventArgs())
        'WPR12- Enhancement Quote Collection Process
        txtBankLocation.Text = ""
        txtBankLocation_Leave(txtBankLocation, New EventArgs())
        cboChequeType.SelectedIndex = -1
        cboChequeType_Leave(cboChequeType, New EventArgs())
        txtBankBranch.Text = ""
        txtBankBranch_Leave(txtBankBranch, New EventArgs())
        cboChequeClearingType.SelectedIndex = -1
        cboChequeClearingType_Leave(cboChequeClearingType, New EventArgs())
    End Sub


    ''' <summary>
    ''' Approves a CashListItem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApproveCashListItem() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCashListPost As bACTCashListPost.Automated
        Dim oStepAuthorization As bACTCashlistitem.StepAuthorization
        Dim sErrorMessage As String = ""
        Dim bLastStep As Boolean
        Dim oOptionValue As Object
        Dim sAccountShortCode As String
        Dim sUserGroupCode As String

        Try
            ' START CHANGES - Changed By: AAB  - Changed On: 04-Dec-2003 10:34            
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                      v_vBranch:=g_oObjectManager.SourceID,
                                                      r_vUnderwriting:=oOptionValue)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to process GetProductOptionValue.",
                                   vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="cboPaymentType_Click",
                                   vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                Return nResult
            End If

            If gPMFunctions.NullToString(oOptionValue) = "1" Then
                Dim temp_oStepAuthorization As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization,
                                                         "bACTCashlistitem.StepAuthorization",
                                                         vInstanceManager:=PMGetViaClientManager)
                oStepAuthorization = temp_oStepAuthorization
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to Create StepAuthorization Class",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                'set the properties of the object.

                oStepAuthorization.PaymentType = ACPaymentsType
                oStepAuthorization.PaymentID = CashlistitemID
                oStepAuthorization.PaymentAmount = Amount
                oStepAuthorization.PaymentCreatorUserID = PMUserid
                m_lReturn = oStepAuthorization.ProcessApproval()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to ProcessApproval",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                ' Get the resutls of the process

                sErrorMessage = oStepAuthorization.ProcessErrorMessage
                bLastStep = oStepAuthorization.LastStep
                If sErrorMessage <> "" Then
                    nResult = PMEReturnCode.PMError
                    MessageBox.Show(sErrorMessage, "Cash List Item", MessageBoxButtons.OK)
                    oStepAuthorization.Dispose()
                    oStepAuthorization = Nothing
                    Return nResult
                Else
                    MessageBox.Show("You successfully completed this authorization step.",
                                    "Authorization",
                                    MessageBoxButtons.OK)
                End If

                m_lReturn = m_oBusiness.UpdateTransMatchCashListItemID(nCashListTransDetailsID:=0,
                                                                       nCashListItemID:=CashlistitemID,
                                                                       sProcessType:="APPROVE")
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                               sMsg:="ApproveCashListItem failed to UpdateTransMatchCashListItemID",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="ApproveCashListItem",
                               vErrNo:=Err.Number,
                               vErrDesc:=Err.Description)
                    Return nResult
                End If

                'Delete all the existing recrods we are done
                m_lReturn = m_oBusiness.ProcessWTM(v_lCashlistItemID:=CashlistitemID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to CreateWorkTaskManagerForApprovalProcess",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                m_lReturn = m_oBusiness.GetUserGroupID(v_lUserID:=PMUserid,
                                                       r_lUserGroupID:=m_lPMUserGroupId)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to GetUserGroupID",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If


                m_lReturn = m_oBusiness.GetAccountAndUserGroupCode(v_lAccountId:=AccountID,
                                                                   v_lUserGroupID:=m_lPMUserGroupId,
                                                                   r_sAccountCode:=sAccountShortCode,
                                                                   r_sUsergroupCode:=sUserGroupCode)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to GetAccountAndUserGroupCode",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                If bLastStep Then
                    Dim temp_oCashListPost As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oCashListPost,
                                                             "bACTCashListPost.Automated",
                                                             PMGetViaClientManager)
                    oCashListPost = temp_oCashListPost
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="ApproveCashListItem failed to Create bACTCashListPost.Automated object",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="ApproveCashListItem",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        Return nResult
                    End If

                    'Change the status to Issued
                    m_lReturn = SelectComboItem(r_ctl:=cboPaymentStatus, v_varID:=ACPaymentStatusIssued)
                    m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, ACPaymentStatusIssued)
                    'set to PMEdit to ensure the data is saved in the database for posting and allocation
                    m_iTask = PMEComponentAction.PMEdit
                    ProcessCommit()
                Else
                    sUserGroupCode = ""
                    m_lReturn = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="ApproveCashListItem failed to GetStepGroupCode",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="ApproveCashListItem",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        Return nResult
                    End If
                End If

                m_lReturn = CreateWTMForApproval(v_sUserGroupCode:=sUserGroupCode,
                                                 v_sAccountShortCode:=sAccountShortCode,
                                                 v_bLastStep:=bLastStep)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to CreateWorkTaskManagerForApprovalProcess",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                If Not (oStepAuthorization Is Nothing) Then
                    oStepAuthorization.Dispose()
                    oStepAuthorization = Nothing
                End If
            Else
                Dim temp_oCashListPost2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashListPost2,
                                                         "bACTCashListPost.Automated",
                                                         PMGetViaClientManager)
                oCashListPost = temp_oCashListPost2
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to Create bACTCashListPost.Automated object",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                'Change the status to Issued
                SelectComboItem(cboPaymentStatus, ACPaymentStatusIssued)
                ProcessCommit()
                m_lReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=CashlistID,
                                                              v_vCashListItemID:=CashlistitemID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="ApproveCashListItem failed to PostUnallocatedCash",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="ApproveCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If
            End If

            If Not (oCashListPost Is Nothing) Then
                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If

            If Not (oStepAuthorization Is Nothing) Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            If Not (oCashListPost Is Nothing) Then
                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If
            If Not (oStepAuthorization Is Nothing) Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="ApproveCashListItem failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="ApproveCashListItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    Private Function CreateWTMForApproval(ByVal v_sUserGroupCode As String, ByVal v_sAccountShortCode As String, ByVal v_bLastStep As Boolean) As Integer

        Dim result As Integer = 0
        Try

            Dim sTaskDesc, sTaskDescComplete As String
            Dim lTaskInstanceCnt As Integer
            Dim vKeyArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetCashListType(v_lCashListTypeID:=CashlistTypeID, r_sCashListType:=sTaskDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWTMForApproval Failed to GetCashListType", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWTMForApproval", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If v_bLastStep Then

                ReDim vKeyArray(1, 2)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CashlistitemID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CashlistID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = CashlistTypeID

                sTaskDescComplete = "Your Payment has been Approved" & New String(" "c, 1)
                sTaskDescComplete = sTaskDescComplete & "- Reference: " & OurRef.Trim() &
                                    New String(" "c, 1)
                sTaskDescComplete = sTaskDescComplete & "- The Amount: " & StringsHelper.Format(Amount, "#,##0.00")

                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=v_sAccountShortCode, v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateTime.Now, v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:=v_sUserGroupCode, v_vKeyArray:=vKeyArray, v_iUserID:=PMUserid)
            Else
                ReDim vKeyArray(1, 5)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CashlistitemID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameCashListId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CashlistID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = CashlistTypeID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameActionKey

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = gACTLibrary.ACTApprove

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameCashListItemMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = gACTLibrary.ACTUseListHidden

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameClaimPaymentId

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lClaimPaymentId

                sTaskDescComplete = sTaskDesc & " - Cash / Cheque" & New String(" "c, 1)
                sTaskDescComplete = sTaskDescComplete & "- Reference: " & OurRef.Trim() &
                                    New String(" "c, 1)
                sTaskDescComplete = sTaskDescComplete & "- The Amount: " & StringsHelper.Format(Amount, "#,##0.00")

                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:=v_sAccountShortCode, v_sDescription:=sTaskDescComplete, v_dtTaskDueDate:=DateTime.Now, v_sTaskCode:="SHOWCLITEM", v_sTaskGroupCode:="SLACS", v_sUserGroupCode:=v_sUserGroupCode, v_vKeyArray:=vKeyArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWTMForApproval Failed to " &
                                   "AddTaskToWorkManager", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWTMForApproval", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWTMForApproval Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWTMForApproval", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ''' <summary>
    ''' Declines a CashListItem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeclineCashListItem() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oNotes As Object
        Dim sAccountShortCode As String
        Dim sUserGroupCode As String
        Dim lResultingTaskID As Integer
        Dim sNotes As String = ""
        Dim oStepAuthorization As bACTCashlistitem.StepAuthorization
        Dim sErrorMessage As String = ""
        Dim bLastStep As Boolean
        Dim sGroupCode As String = ""
        Dim vOptionValue As Object
        Dim vResultArray(,) As Object

        Try

            m_lReturn = DisplayNotesScreen(sNotes)

            'TR - If the Status is OK, Update the Audit Set
            If m_lReturn = PMEReturnCode.PMOK Then

                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                          v_vBranch:=g_oObjectManager.SourceID,
                                                          r_vUnderwriting:=vOptionValue)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to process GetProductOptionValue.",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="cboPaymentType_Click")
                    Return nResult
                End If

                If gPMFunctions.NullToString(vOptionValue) = "1" Then
                    ' START CHANGES - Changed By: AAB  - Changed On: 04-Dec-2003 10:34
                    Dim temp_oStepAuthorization As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization,
                                                             "bACTCashlistitem.StepAuthorization",
                                                             vInstanceManager:=PMGetViaClientManager)
                    oStepAuthorization = temp_oStepAuthorization
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="ApproveCashListItem failed to Create StepAuthorization Class",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="ApproveCashListItem",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        Return nResult
                    End If

                    If CashlistID > 0 Then
                        m_lReturn = m_oBusiness.GetCashListDetails(nCashListId:=CashlistID,
                                                                   r_oResultArray:=vResultArray)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            nResult = m_lReturn
                            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                               sMsg:="DeclineCashListItem failed to GetCashListDetails",
                                               vApp:=ACApp, vClass:=ACClass, vMethod:="DeclineCashListItem",
                                               vErrNo:=Err.Number, vErrDesc:=Err.Description)
                            Exit Function
                        End If

                        If ToSafeLong(vResultArray(5, 0), 0) > 0 Then
                            m_lReturn = oStepAuthorization.Initialise(g_sUserName$, g_sPassword$, g_iUserID%,
                                                                                ToSafeLong(vResultArray(5, 0), 0), g_iLanguageID%,
                                                                                    g_iCurrencyID%, g_iLogLevel%, m_sCallingAppName$)
                        End If
                    End If


                    oStepAuthorization.PaymentType = ACPaymentsType
                    oStepAuthorization.PaymentID = CashlistitemID
                    oStepAuthorization.PaymentAmount = Amount
                    oStepAuthorization.PaymentCreatorUserID = PMUserid
                    m_lReturn = oStepAuthorization.ProcessDecline()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="ApproveCashListItem failed to Process User Authroization",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="ApproveCashListItem",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        Return nResult
                    End If

                    ' Get the resutls of the process
                    sErrorMessage = oStepAuthorization.ProcessErrorMessage
                    bLastStep = oStepAuthorization.LastStep
                    If sErrorMessage <> "" Then
                        nResult = PMEReturnCode.PMError
                        MessageBox.Show(sErrorMessage, "Cash List Item", MessageBoxButtons.OK)
                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                        Return nResult
                    Else
                        MessageBox.Show("You successfully completed this step.", Application.ProductName)
                    End If

                    m_lReturn = m_oBusiness.ProcessWTM(v_lCashlistItemID:=CashlistitemID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        nResult = m_lReturn
                        iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="ApproveCashListItem failed to CreateWorkTaskManagerForApprovalProcess",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:="DeclineCashListItem",
                                           vErrNo:=Information.Err().Number,
                                           vErrDesc:=Information.Err().Description)
                        Return nResult
                    End If

                    If Not (oStepAuthorization Is Nothing) Then
                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If
                Else
                    'Change the status to Declined
                    SelectComboItem(cboPaymentStatus, ACPaymentStatusDeclined)
                    ProcessCommit()
                End If

                m_lReturn = m_oBusiness.GetUserGroupID(v_lUserID:=PMUserid, r_lUserGroupID:=m_lPMUserGroupId)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="DeclineCashListItem failed to GetUserGroupID",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="DeclineCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                m_lReturn = m_oBusiness.GetAccountAndUserGroupCode(v_lAccountId:=AccountID,
                                                                   v_lUserGroupID:=m_lPMUserGroupId,
                                                                   r_sAccountCode:=sAccountShortCode,
                                                                   r_sUsergroupCode:=sUserGroupCode)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="DeclineCashListItem failed to GetAccountAndUserGroupCode",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="DeclineCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                'TR - Create a new Memo Task
                ' START CHANGES - Changed By: AAB  - Changed On: 02-Mar-2004 12:36
                ' Added the correct TaskGroupCode and the UserGroupCode
                m_lReturn = m_oBusiness.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lResultingTaskID,
                                                             v_sCustomer:=sAccountShortCode,
                                                             v_sDescription:="Pending Payment Declined - " &
                                                             sNotes.Replace(Strings.Chr(13) & Strings.Chr(10), "") &
                                                             "- The Amount: " & StringsHelper.Format(Amount, "#,##0.00"),
                                                             v_dtTaskDueDate:=DateTime.Now,
                                                             v_sTaskCode:="MEMO",
                                                             v_iUserID:=PMUserid,
                                                             v_sTaskGroupCode:="SLACS",
                                                             v_sUserGroupCode:=sUserGroupCode)
                ' END CHANGES - Changed By: AAB  - Changed On: 02-Mar-2004 12:36
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="DeclineCashListItem failed to AddTaskToWorkManager",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="DeclineCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

                'TR - Update the PaymentStatus on CLI
                m_lReturn = m_oBusiness.UpdateUserProperty(v_sPropertyName:=PMNavKeyConst.ACTKeyNameCashListItemPaymentStatusId,
                                                           v_vPropertyValue:=9)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="DeclineCashListItem failed to UpdateUserProperty",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="DeclineCashListItem",
                                       vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If
                m_lReturn = m_oBusiness.UpdateTransMatchCashListItemID(nCashListTransDetailsID:=0,
                                                                       nCashListItemID:=CashlistitemID,
                                                                       sProcessType:="DECLINE")

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="DeclineCashListItem failed to UpdateTransMatchCashListItemID",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="DeclineCashListItem",
                                       vErrNo:=Err.Number,
                                       vErrDesc:=Err.Description)
                    Return nResult
                End If

            Else
                ' MEvans : 16-09-2003 : CQ1745
                ' the decline has been cancelled so we
                ' dont want to allow the task to become completed
                nResult = PMEReturnCode.PMFalse
            End If
            oNotes = Nothing
            Return nResult
        Catch excep As System.Exception
            If Not (oStepAuthorization Is Nothing) Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If
            nResult = PMEReturnCode.PMFalse
            iPMFunc.LogMessage(PMELogLevel.PMLogOnError,
                               "DeclineCashListItem Failed",
                               ACApp, ACClass,
                               "DeclineCashListItem",
                               Information.Err().Number,
                               excep.Message,
                               excep:=excep)
            Return nResult

        End Try
    End Function

    '*************************************************************************
    'Name:          DisplayNotesScreen
    'Description:   Displays the Event Notes Screen (in Add mode)
    '               Gets PartyCnt from AccountID, and passes m_lInsuranceFileCnt
    '               and g_sUserName
    'History:       07/02/2003 - TR - Created
    '*************************************************************************
    Private Function DisplayNotesScreen(ByRef r_sNotes As String) As Integer

        Dim result As Integer = 0
        Dim oNotes As iPMBNote.Interface_Renamed
        Dim lPartyCnt As Integer

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Determine the Party_cnt from the Account ID

            m_lReturn = m_oBusiness.GetPartyCntFromAccountID(AccountID, lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Create the Notes Interface object
            oNotes = New iPMBNote.Interface_Renamed()

            m_lReturn = oNotes.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Populate the Notes form
            With oNotes
                .PartyCnt = lPartyCnt
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .AccountKey = 0 'Thinh Nguyen don't pass this in as it will load up account type which we don't support at the moment
                .NoteDate = DateTime.Now
                .UserName = g_sUserName
            End With
            m_lReturn = oNotes.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            m_lReturn = oNotes.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oNotes.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNotesScreen")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DD 29/09/2003: Get the notes entered by the user
            r_sNotes = oNotes.Description

            'TR - Set return value
            result = oNotes.Status

            'TR - Destroy the object
            oNotes.Dispose()
            oNotes = Nothing

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DisplayNotesScreen Failed", ACApp, ACClass, "DisplayNotesScreen", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          ConfirmReceipt
    'Description:   Calls the business Object bCLMDebtAllocate, to Process
    '               the Debt repayments
    'History:       TR - 14/04/03 - TS17 Recovery By Instalments changes
    '*************************************************************************
    Private Function ConfirmReceipt() As Integer
        Dim result As Integer = 0
        Dim sAccountShortCode, sUserGroupCode As String
        Dim obCLMDebtAllocate As Object

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Create the Business object
            Dim temp_obCLMDebtAllocate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_obCLMDebtAllocate, "bCLMDebtAllocate.Business", gPMConstants.PMGetViaClientManager)
            obCLMDebtAllocate = temp_obCLMDebtAllocate
            'TR - Make sure that this worked
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Get the Customer ShortName

            m_lReturn = m_oBusiness.GetAccountAndUserGroupCode(AccountID, m_lPMUserGroupId, sAccountShortCode, sUserGroupCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to Get Account and UserGroup Code" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'TR - Pass in both the arrays generated from iCLMDebtAllocate
            'Let the business object deal with them individually

            m_lReturn = obCLMDebtAllocate.ConfirmReceipt(m_vUnsavedRBIItems, m_vReversedRBIItems, sAccountShortCode, CashlistitemID, m_bOnInstalment)
            'TR - Make sure that this worked
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'TR - PmSucceed from this Method indicates Amount larger than
                'Debt & could not be handled
                If m_lReturn = gPMConstants.PMEReturnCode.PMSucceed Then
                    'TR - Todo Spec 223?
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'TR - Destroy objects
            obCLMDebtAllocate = Nothing

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "ConfirmReceipt Failed", ACApp, ACClass, "ConfirmReceipt", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '===========
    'Form Events
    '===========
    Private Sub cmdCommit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCommit.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status, so that we don't
            ' subsequently update again in List Form
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Ensure any Lost Focus events have been processed
            ' prior to continuing with OK
            If Not iPMFunc.ForceLostFocus(cmdCommit) Then
                Exit Sub
            End If

            ' Use the List Form method to update and commit the business
            m_lReturn = ProcessCommit()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Commit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCommit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Private Sub txtAmount_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAmount.KeyPress
        If Not Char.IsNumber(eventArgs.KeyChar) AndAlso Not Char.IsControl(eventArgs.KeyChar) AndAlso Not eventArgs.KeyChar = "." Then
            eventArgs.Handled = True
        End If

    End Sub

    Private Sub txtAmount_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtAmount.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If gPMFunctions.ToSafeCurrency(txtAmount.Text) = 0 Then
            txtAmount.Text = "0.00"
        End If
        eventArgs.Cancel = Cancel
    End Sub

    'WPR12- Enhancement Quote Collection Process
    Private Sub txtBankBranch_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankBranch.Enter

        m_lReturn = m_oFormFields.GotFocus(txtBankBranch)
    End Sub

    Private Sub txtBankBranch_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankBranch.Leave

        m_lReturn = m_oFormFields.LostFocus(txtBankBranch)
    End Sub

    'WPR12- Enhancement Quote Collection Process
    Private Sub txtBankLocation_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankLocation.Enter

        m_lReturn = m_oFormFields.GotFocus(txtBankLocation)
    End Sub

    Private Sub txtBankLocation_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankLocation.Leave

        m_lReturn = m_oFormFields.LostFocus(txtBankLocation)
    End Sub

    Private Sub txtBankRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankRef.Enter

        m_lReturn = m_oFormFields.GotFocus(txtBankRef)
    End Sub

    Private Sub txtBankRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBankRef.Leave

        m_lReturn = m_oFormFields.LostFocus(txtBankRef)

        'Retrieve or update the Batch ID
        'ProcessBankBatch(txtBankRef.Text)
    End Sub
    'Rahul Start
    Private Sub txtCollectionDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDate.Enter
        iPMValidate.CheckDateGotFocus(txtCollectionDate)

        m_lReturn = m_oFormFields.GotFocus(txtCollectionDate)
    End Sub

    Private Sub txtCollectionDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDate.Leave
        iPMValidate.CheckDateLostFocus(txtCollectionDate)

        m_lReturn = m_oFormFields.LostFocus(txtCollectionDate)
        If Not Information.IsDate(txtCollectionDate.Text) Then

            txtCollectionDate.Text = DateTime.Today
        End If
        If CDate(txtCollectionDate.Text) <> DateTime.Today Then
            txtComments.Enabled = True
            lblComments.Enabled = True
            'Start(Sriram P)PN 54241
            txtComments.ReadOnly = False
            'End(Sriram P)PN 54241
        Else
            txtComments.Enabled = False
            lblComments.Enabled = False
        End If
    End Sub
    'End

    Private Sub txtConfirmation_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConfirmation.Enter

        m_lReturn = m_oFormFields.GotFocus(txtConfirmation)
    End Sub
    Private Sub txtConfirmation_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConfirmation.Leave

        m_lReturn = m_oFormFields.LostFocus(txtConfirmation)
    End Sub

    Private Sub txtPaymentExpiryDate_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtPaymentExpiryDate.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        If Information.IsDate(txtPaymentExpiryDate.Text) Then
            txtPaymentExpiryDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateLong, vFieldValue:=gPMFunctions.ToSafeDate(txtPaymentExpiryDate.Text)) '''''PN68983
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Private Sub txtPaymentReason_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReason.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPaymentReason)
    End Sub
    Private Sub txtPaymentReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReason.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentReason)
    End Sub
    Private Sub txtChange_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtChange.Enter

        m_lReturn = m_oFormFields.GotFocus(txtChange)
    End Sub
    Private Sub txtChange_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtChange.Leave

        m_lReturn = m_oFormFields.LostFocus(txtChange)
    End Sub
    Private Sub txtChequeDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtChequeDate.Enter
        iPMValidate.CheckDateGotFocus(txtChequeDate) ' Gaurav Doubt

        m_lReturn = m_oFormFields.GotFocus(txtChequeDate)
    End Sub
    Private Sub txtChequeDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtChequeDate.Leave
        iPMValidate.CheckDateLostFocus(txtChequeDate) ' Gaurav Doubt

        m_lReturn = m_oFormFields.LostFocus(txtChequeDate)
        If Not Information.IsDate(txtChequeDate.Text) Then 'Gaurav Doubt

            txtChequeDate.Text = DateTime.Today
        End If
    End Sub
    Private Sub txtDatePresented_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePresented.Enter

        m_lReturn = m_oFormFields.GotFocus(txtDatePresented)
    End Sub
    Private Sub txtDatePresented_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePresented.Leave

        m_lReturn = m_oFormFields.LostFocus(txtDatePresented)
    End Sub
    Private Sub txtFurtherDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFurtherDetails.Enter

        m_lReturn = m_oFormFields.GotFocus(txtFurtherDetails)
    End Sub
    Private Sub txtFurtherDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFurtherDetails.Leave

        m_lReturn = m_oFormFields.LostFocus(txtFurtherDetails)
        Receiptdetails = gPMFunctions.ToSafeString(txtFurtherDetails)
    End Sub

    Private Sub txtMediaRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMediaRef.Enter

        m_lReturn = m_oFormFields.GotFocus(txtMediaRef)
    End Sub
    Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter

        m_lReturn = m_oFormFields.GotFocus(txtAmount)

        sAmountFieldText = txtAmount.Text

    End Sub

    Private Sub txtSplitTotal_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSplitTotal.Enter
        m_lReturn = m_oFormFields.GotFocus(txtSplitTotal)
        SplitTotal = gPMFunctions.ToSafeDecimal(txtSplitTotal.Text)
    End Sub

    Private Sub txtSplitTotal_Leave(ByVal eventSender As Object, ByVal evenArgs As EventArgs) Handles txtSplitTotal.Leave
        m_lReturn = m_oFormFields.LostFocus(txtSplitTotal)
    End Sub

    Private Sub txtSplitTotal_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtSplitTotal.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = 8 Then
            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub
        End If
        If Strings.Len(txtSplitTotal.Text) > 10 Then
            If gPMFunctions.ToSafeCurrency(CDbl(txtSplitTotal.Text) * 10) > 0 Then
            Else
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave
        Dim iLookUpID As Integer
        Dim sPaymentMethod, sLookUpcode As String
        Dim bReceiptTypeIsInstalmentBased As Boolean

        '  To support partial pay
        If cboReceiptType.SelectedIndex <> -1 Then
            iLookUpID = VB6.GetItemData(Me.cboReceiptType, Me.cboReceiptType.SelectedIndex)

            m_lReturn = m_oListForm.GetReceiptTypeDetails(v_lReceiptTypeId:=iLookUpID, r_sReceiptTypeAlias:=sLookUpcode, r_bIsInstalmentsBased:=bReceiptTypeIsInstalmentBased)
            ReceiptTypeIsInstalmentBased = bReceiptTypeIsInstalmentBased
            If bReceiptTypeIsInstalmentBased Then
                'do nothing
                Exit Sub
            Else
                If txtAmount.Text = "" Then
                    txtAmount.Text = "0.00"
                End If
            End If
        End If

        Dim dbNumericTemp As Double
        If Double.TryParse(txtAmount.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If Conversion.Val(txtAmount.Text) < 0 Then
                txtAmount.Text = CStr(CDbl(txtAmount.Text) * -1)
            End If
        Else
            'PN10543 ECK19022004
            MessageBox.Show("Please Enter a Valid Amount", "Cash List Item", MessageBoxButtons.OK)
            txtAmount.Text = "0.00"

            If txtAmount.Visible Then
                txtAmount_Enter(txtAmount, New EventArgs())
                txtAmount.Focus()
            End If

            Exit Sub
        End If

        'BB Temporary fix until we have proper validation routines
        'txtAmount.Text = FormatField( _
        ''                    iFormatType:=PMFormatCurrency, _
        ''                    vFieldValue:=txtAmount.Text)

        If Conversion.Val(txtTendered.Text) = 0 Then
            txtTendered.Text = txtAmount.Text
        End If

        If cmbMediaType.SelectedIndex <> -1 Then

            iLookUpID = VB6.GetItemData(Me.cmbMediaType, cmbMediaType.SelectedIndex)

            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpcode:=sPaymentMethod)

            If sPaymentMethod = ACMediaTypeCash Then
                If Not ValidateMediaDetails() Then
                    Exit Sub
                End If
            End If

        End If

        m_lReturn = m_oFormFields.LostFocus(txtAmount)

        'we are dealing with normal instalments
        If gPMFunctions.ToSafeCurrency(txtAmount.Text) > gPMFunctions.ToSafeCurrency(txtTendered.Text) Then
            If txtTendered.Visible Then
                MessageBox.Show("The amount tendered cannot be less than the amount", "Error: " &
                                "Incorrect Amount Tendered", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            txtTendered.Text = txtAmount.Text
            txtTendered_Leave(txtTendered, New EventArgs())
        Else
            txtChange.Text = CStr(gPMFunctions.ToSafeCurrency(txtTendered.Text) - gPMFunctions.ToSafeCurrency(txtAmount.Text))

            m_lReturn = m_oFormFields.LostFocus(txtChange)
        End If
        ''    End If

        If sAmountFieldText <> "" Then
            If gPMFunctions.ToSafeCurrency(sAmountFieldText) <> gPMFunctions.ToSafeCurrency(txtAmount.Text) Then
                Originalamount = 0
            End If
        End If

        If cboReceiptType.Text = "Bank Guarantee Debt" Then
            m_lReturn = GetAllPolicyBG()
        End If
    End Sub

    Private Sub txtMediaRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMediaRef.Leave

        m_lReturn = m_oFormFields.LostFocus(txtContactName)
    End Sub
    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter

        m_lReturn = m_oFormFields.GotFocus(txtName)
    End Sub
    Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave

        m_lReturn = m_oFormFields.LostFocus(txtName)
    End Sub
    Private Sub txtOurRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOurRef.Enter

        m_lReturn = m_oFormFields.GotFocus(txtOurRef)
    End Sub
    Private Sub txtOurRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOurRef.Leave

        m_lReturn = m_oFormFields.LostFocus(txtOurRef)
    End Sub
    Private Sub txtReason_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReason.Enter

        m_lReturn = m_oFormFields.GotFocus(txtReason)
    End Sub
    Private Sub txtReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReason.Leave

        m_lReturn = m_oFormFields.LostFocus(txtReason)
    End Sub
    Private Sub txtReverseDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReverseDate.Enter

        m_lReturn = m_oFormFields.GotFocus(txtReverseDate)
    End Sub
    Private Sub txtReverseDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReverseDate.Leave

        m_lReturn = m_oFormFields.LostFocus(txtReverseDate)
    End Sub

    Private Sub txtTendered_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTendered.Enter

        m_lReturn = m_oFormFields.GotFocus(txtTendered)
    End Sub

    Private Sub txtTendered_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTendered.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtTendered_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTendered.Leave
        If txtTendered.Text = "" Then
            txtTendered.Text = "0"
        End If
        Dim dbNumericTemp As Double
        If Double.TryParse(txtTendered.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If Conversion.Val(txtTendered.Text) < 0 Then
                txtTendered.Text = CStr(CDbl(txtTendered.Text) * -1)
            End If
        Else
            'PN10543 ECK19022004
            MessageBox.Show("Please Enter a Valid Amount", "Cash List Item", MessageBoxButtons.OK)
            txtTendered.Text = "0.00"

            If txtTendered.Visible Then
                txtTendered_Enter(txtTendered, New EventArgs())
                txtTendered.Focus()
            End If
            Exit Sub
        End If
        m_lReturn = m_oFormFields.LostFocus(txtTendered)

        If gPMFunctions.ToSafeCurrency(Convert.ToDecimal(txtTendered.Text)) < gPMFunctions.ToSafeCurrency(Convert.ToDecimal(txtAmount.Text)) Then

            MessageBox.Show("The amount tendered cannot be less than the amount", "Incorrect Amount Tendered", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtTendered.Text = txtAmount.Text
        End If
        txtChange.Text = CStr(gPMFunctions.ToSafeCurrency(Convert.ToDecimal(txtTendered.Text)) - gPMFunctions.ToSafeCurrency(Convert.ToDecimal(txtAmount.Text)))
        m_lReturn = m_oFormFields.LostFocus(txtChange)
    End Sub
    Private Sub txtTheirRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTheirRef.Enter
        iPMFunc.SelectText(txtTheirRef)
    End Sub
    Private Sub txtContactName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Enter

        m_lReturn = m_oFormFields.GotFocus(txtContactName)
    End Sub
    Private Sub txtContactName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Leave

        m_lReturn = m_oFormFields.LostFocus(txtContactName)
        ContactName = gPMFunctions.ToSafeString(txtContactName)
    End Sub
    Private Sub txtPayeeName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayeeName.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPayeeName)
    End Sub
    Private Sub txtPayeeName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPayeeName.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPayeeName)
    End Sub
    Private Sub txtPaymentAccountCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAccountCode.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPaymentAccountCode)
    End Sub
    Private Sub txtPaymentAccountCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentAccountCode.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentAccountCode)
    End Sub
    Private Sub txtBIC_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBIC.Enter

        m_lReturn = m_oFormFields.GotFocus(txtBIC)
    End Sub
    ''' <summary>
    ''' Called on KeyPress event of textox to validate only alphanumeric input
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AlphanumericValidation(sender As Object, e As KeyPressEventArgs) Handles txtBIC.KeyPress, txtIBAN.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        If (KeyAscii >= 48 AndAlso KeyAscii <= 57) OrElse
            (KeyAscii >= 65 AndAlso KeyAscii <= 90) OrElse
            (KeyAscii >= 97 And KeyAscii <= 122) _
            OrElse KeyAscii = 8 OrElse KeyAscii = 127 Then
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ''' <summary>
    ''' Called to validate alphanumeric validation via Copy/Paste 
    ''' </summary>
    ''' <param name="sInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AlphanumericValidation(ByVal sInput As String) As Integer
        If System.Text.RegularExpressions.Regex.IsMatch(sInput, "^[a-zA-Z0-9]*$") Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMFalse
        End If
    End Function

    Private Sub txtBIC_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBIC.Leave

        m_lReturn = m_oFormFields.LostFocus(txtBIC)
    End Sub
    Private Sub txtIBAN_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIBAN.Enter

        m_lReturn = m_oFormFields.GotFocus(txtIBAN)
    End Sub

    Private Sub txtIBAN_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIBAN.Leave

        m_lReturn = m_oFormFields.LostFocus(txtIBAN)
    End Sub

    Private Sub txtPaymentBranchCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentBranchCode.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPaymentBranchCode)
    End Sub
    Private Sub txtPaymentBranchCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentBranchCode.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentBranchCode)
    End Sub
    Private Sub txtPaymentExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentExpiryDate.Enter
        '''''PN68983
        iPMValidate.CheckDateGotFocus(txtPaymentExpiryDate)
        iPMFunc.SelectText(txtPaymentExpiryDate)

        m_lReturn = m_oFormFields.GotFocus(txtPaymentExpiryDate)
    End Sub
    Private Sub txtPaymentExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentExpiryDate.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentExpiryDate)
    End Sub
    Private Sub txtPaymentReference1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference1.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPaymentReference1)
    End Sub
    Private Sub txtPaymentReference1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference1.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentReference1)
    End Sub
    Private Sub txtPaymentReference2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference2.Enter

        m_lReturn = m_oFormFields.GotFocus(txtPaymentReference2)
    End Sub
    Private Sub txtPaymentReference2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPaymentReference2.Leave

        m_lReturn = m_oFormFields.LostFocus(txtPaymentReference2)
    End Sub
    Private Sub txtTransDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTransDate.Enter
        iPMValidate.CheckDateGotFocus(txtTransDate)

        iPMFunc.SelectText(txtTransDate)

        m_lReturn = m_oFormFields.GotFocus(txtTransDate)
    End Sub
    Private Sub txtTransDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTransDate.Leave
        Dim sMessage As String = ""
        'PN17203
        Dim sYear As String
        txtTransDate.Text = txtTransDate.Text.Replace("."c, "/"c)
        iPMValidate.CheckDateLostFocus(txtTransDate)
        If txtTransDate.Text.Trim().Length > 0 Then
            sYear = txtTransDate.Text.Substring(txtTransDate.Text.Length - 4)
        End If

        Dim dbNumericTemp As Double
        If Not Double.TryParse(sYear, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            sMessage = "Please enter date in valid format DD/MM/YYYY."
            txtTransDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, DateTime.Today)
            MessageBox.Show(sMessage, "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtTransDate.Focus()
            Exit Sub
        End If

        Dim eDateType As gSIRLibrary.SIREDateType = gSIRLibrary.SIRDateType(txtTransDate.Text)

        Select Case eDateType
            Case gSIRLibrary.SIREDateType.sireNullDate
                sMessage = "Date must be later than " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, gSIRLibrary.SIRSystemLowDate) & "."
            Case gSIRLibrary.SIREDateType.sireInvalidDate
                sMessage = "Please enter a valid date."
                txtTransDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, DateTime.Today)
        End Select

        If eDateType <> gSIRLibrary.SIREDateType.sireValidDate Then
            MessageBox.Show(sMessage, "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtTransDate.Focus()
        Else
            iPMValidate.CheckDateLostFocus(txtTransDate)

            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTransDate)

        End If
    End Sub

    Private Function GetAllPolicyBG() As Integer
        Dim result As Integer = 0
        Dim bSirBankGuarantee As Object
        Const kMethodName As String = "GetAllPolicyBG"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oBankGuarantee As bSIRBankGuarantee.Business
            Dim lPartyCnt As Integer
            Dim temp_oBankGuarantee As Object

            m_lReturn = g_oObjectManager.GetInstance(temp_oBankGuarantee, "bSIRBankGuarantee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBankGuarantee = temp_oBankGuarantee
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oBusiness.GetPartyCntFromAccountID(Conversion.Val(CStr(uctAccountLookup.AccountId)), lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oBusiness.GetPartyCntFromAccountID Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oBankGuarantee.GetPolicyBGForReceipt(r_vGetPoliciesForReceipt:=m_vGetBGPoliciesForReceipt, vPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "oBankGuarantee.GetPolicyBGForReceipt Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                m_lReturn = BuildBGPoliciesArray()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateBGDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = PopulateBGDetailsList()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateBGDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_cBalAmtTobeAllocated = gPMFunctions.ToSafeCurrency(txtAmount.Text, 0)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Private Function BuildBGPoliciesArray() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "BuildBGPoliciesArray"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vGetBGPoliciesForReceipt) And Information.IsArray(m_vSelectdBGPolicies) Then

                For lBGCount As Integer = m_vGetBGPoliciesForReceipt.GetLowerBound(1) To m_vGetBGPoliciesForReceipt.GetUpperBound(1)

                    For lSelPolCount As Integer = m_vSelectdBGPolicies.GetLowerBound(1) To m_vSelectdBGPolicies.GetUpperBound(1)

                        If m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.InsuranceFileCnt, lBGCount).Equals(m_vSelectdBGPolicies(MainModule.ENBankGuarantee.InsuranceFileCnt, lSelPolCount)) Then

                            m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lBGCount) = CDbl(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lBGCount)) - CDbl(m_vSelectdBGPolicies(MainModule.ENBankGuarantee.AmtTobePosted, lSelPolCount))
                        End If
                    Next
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

    Private Function PopulateBGDetailsList() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBGDetailsList"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim crBalanceAmount As Decimal
            Dim lCount As Integer
            Dim oListItem As ListViewItem

            'ReDim m_vSelBGPoliciesItemForReceipt(ENBankGuarantee.LastItem, 0)

            crBalanceAmount = gPMFunctions.ToSafeCurrency(txtAmount.Text, 0)
            lblBGPolAmtToBePosted.Text = ""
            lblBGPolOutstandingAmt.Text = ""

            If gPMFunctions.IsArrayEmpty(m_vGetBGPoliciesForReceipt) Then
                Return result
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwBGDetails.Items.Clear()
            RemoveHandler lvwBGDetails.ItemChecked, AddressOf lvwBGDetails_ItemChecked

            For i As Integer = m_vGetBGPoliciesForReceipt.GetLowerBound(1) To m_vGetBGPoliciesForReceipt.GetUpperBound(1)

                oListItem = lvwBGDetails.Items.Add(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.BGRef, i)).Trim(), "")

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexDueDate).Text = CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.DueDate, i)).Trim()

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexPolicyNo).Text = CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.InsuranceRef, i)).Trim()

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexPremiumAmount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, i)).Trim()))

                lblBGPolOutstandingAmt.Text = CStr(gPMFunctions.ToSafeCurrency(lblBGPolOutstandingAmt.Text) + gPMFunctions.ToSafeCurrency(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, i)))

            Next i

            lblBGPolAmtToBePosted.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(lblBGPolAmtToBePosted.Text))
            lblBGPolOutstandingAmt.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(lblBGPolOutstandingAmt.Text))
            AddHandler lvwBGDetails.ItemChecked, AddressOf lvwBGDetails_ItemChecked



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    '            m_lReturn = SelectedBGPoliciesForReceipt(lIndex:=i, _
    ''                                                crPostedAmount:=m_vGetBGPoliciesForReceipt(ENBankGuarantee.OutstandingAmount, lIndex))

    Private Function CalculateAmtTobeAllocated(ByRef lIndex As Integer, ByRef bIsChecked As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CalculateAmtTobeAllocated"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bIsChecked Then

                If m_cBalAmtTobeAllocated >= gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)).Trim()) And m_cBalAmtTobeAllocated > 0 And gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)).Trim()) > 0 Then

                    ListViewHelper.GetListViewSubItem(lvwBGDetails.Items.Item(lIndex), kBankGuaranteeColHIndexPostedAmount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)).Trim()))

                    m_cBalAmtTobeAllocated -= gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)).Trim())

                    lblBGPolAmtToBePosted.Text = CStr(gPMFunctions.ToSafeCurrency(lblBGPolAmtToBePosted.Text) + gPMFunctions.ToSafeCurrency(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)))

                ElseIf m_cBalAmtTobeAllocated < gPMFunctions.ToSafeCurrency(CStr(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)).Trim()) And m_cBalAmtTobeAllocated > 0 And gPMFunctions.ToSafeCurrency(m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)) > 0 Then

                    ListViewHelper.GetListViewSubItem(lvwBGDetails.Items.Item(lIndex), kBankGuaranteeColHIndexPostedAmount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(m_cBalAmtTobeAllocated))

                    lblBGPolAmtToBePosted.Text = CStr(gPMFunctions.ToSafeCurrency(lblBGPolAmtToBePosted.Text) + gPMFunctions.ToSafeCurrency(m_cBalAmtTobeAllocated))

                    'Uday
                    m_cBalAmtTobeAllocated = 0
                End If
            Else
                'm_cBalAmtTobeAllocated = m_cBalAmtTobeAllocated
                'UDAY

                m_cBalAmtTobeAllocated += gPMFunctions.ToSafeDouble(ListViewHelper.GetListViewSubItem(lvwBGDetails.Items.Item(lIndex), kBankGuaranteeColHIndexPostedAmount).Text)
                lblBGPolAmtToBePosted.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=gPMFunctions.ToSafeCurrency(gPMFunctions.ToSafeDouble(txtAmount.Text) - gPMFunctions.ToSafeCurrency(m_cBalAmtTobeAllocated)))
                ListViewHelper.GetListViewSubItem(lvwBGDetails.Items.Item(lIndex), kBankGuaranteeColHIndexPostedAmount).Text = ""
                If CInt(lblBGPolAmtToBePosted.Text) = 0 Then
                    cmdOK.Enabled = False
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

    Private Function ReBuildPolicyBgArray(ByVal crPostedAmount As Decimal) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ReBuildPolicyBgArray"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim uboundBankDetails As Integer

            m_vSelBGPoliciesItemForReceipt = Nothing

            '            If Not IsArrayEmpty(m_vSelBGPoliciesItemForReceipt) Then
            '                    uboundBankDetails = UBound(m_vSelBGPoliciesItemForReceipt, 2) + 1
            '                    ReDim Preserve m_vSelBGPoliciesItemForReceipt(ENBankGuarantee.LastItem, UBound(m_vSelBGPoliciesItemForReceipt, 2) + 1)
            '            Else
            ReDim m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.LastItem, 0)
            '            End If

            For lIndex As Integer = 0 To lvwBGDetails.Items.Count - 1

                If lvwBGDetails.Items.Item(lIndex) IsNot Nothing AndAlso lvwBGDetails.Items.Item(lIndex).Checked Then

                    uboundBankDetails = m_vSelBGPoliciesItemForReceipt.GetUpperBound(1)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.Amount, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.Amount, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.AmtTobePosted, uboundBankDetails) = lvwBGDetails.Items.Item(lIndex).SubItems.Item(kBankGuaranteeColHIndexPostedAmount - 1).Text
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.BankName, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.BankName, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.BankNameId, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.BankNameId, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.BGId, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.BGId, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.BGRef, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.BGRef, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.CoverStartDate, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.CoverStartDate, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.DueDate, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.DueDate, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.ExpiryDate, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.ExpiryDate, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.InsuranceFileCnt, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.InsuranceFileCnt, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.InsuranceRef, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.InsuranceRef, lIndex)
                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.OutstandingAmount, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.ProductCode, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.ProductCode, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.ProductDescription, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.ProductDescription, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.ProductId, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.ProductId, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.SourceCode, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.SourceCode, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.SourceDescription, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.SourceDescription, lIndex)

                    m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.SourceID, uboundBankDetails) = m_vGetBGPoliciesForReceipt(MainModule.ENBankGuarantee.SourceID, lIndex)

                    ReDim Preserve m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.LastItem, m_vSelBGPoliciesItemForReceipt.GetUpperBound(1) + 1)
                End If
            Next
            If Not gPMFunctions.IsArrayEmpty(m_vSelBGPoliciesItemForReceipt) Then

                If CBool(CDbl(m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.LastItem, m_vSelBGPoliciesItemForReceipt.GetUpperBound(1))) - 1) Then

                    If m_vSelBGPoliciesItemForReceipt.GetUpperBound(1) <> 0 Then

                        ReDim Preserve m_vSelBGPoliciesItemForReceipt(MainModule.ENBankGuarantee.LastItem, m_vSelBGPoliciesItemForReceipt.GetUpperBound(1) - 1)
                    Else
                        m_vSelBGPoliciesItemForReceipt = Nothing
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
    Private Sub uctAccountLookup_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctAccountLookup.LostFocus, uctAccountLookup.Leave

        Dim iLookUpID As Integer
        Dim sLookUpcode As String = ""

        'AAB-20-Aug-2003 09:16 - to clear the instalments
        ClearInstalmentDetails()
        GetInstalmentDetails()
        CheckChangeOfAccount()
        GetPartyPolicies()
        ' For credit card payments/receipts we must let uctACTCreditCard immediately know of
        ' any changes to the Account as it will effect display and validation of the control.
        If uctAccountLookup.Text.Trim() <> m_sSaveAccountText.Trim() Then
            If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                uctReceiptCC.AccountID = uctAccountLookup.AccountId
            Else
                If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    uctPaymentCC.AccountID = uctAccountLookup.AccountId
                End If
            End If

            '(RC) PN 31714
            If uctAccountLookup.Text.Trim() = "" Then
                GetPaymentTabData(0)
            Else
                GetPaymentTabData(uctAccountLookup.AccountId)
                txtPaymentExpiryDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, txtPaymentExpiryDate.Text) ''PN68983
            End If

        End If
        If IsLeadAccount Then
            LeadAccountBranchId = CompanyID
        End If
        'if mediatype = chq and account is selected the write account name to cheque name
        'field sw front office receipting
        If cmbMediaType.SelectedIndex <> -1 And CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then

            iLookUpID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

            m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpcode:=sLookUpcode)

            If uctAccountLookup.Text <> "" Then

                m_oFormFields.FormatControl(txtName, ContactName)
                If uctAccountLookup.Text.Trim() <> m_sSaveAccountText.Trim() Then
                    uctReceiptCC.CCName = ContactName.Trim()
                Else
                    If uctAccountLookup.AccountName.Trim() <> "" Then
                        uctReceiptCC.CCName = ContactName.Trim()
                    End If
                End If
            End If
        End If

        m_sSaveAccountText = uctAccountLookup.Text

        ' RDC 14112003 check that account is allowed to make electronic payments
        If cmbMediaType.SelectedIndex <> -1 Then

            With cmbMediaType

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=VB6.GetItemData(cmbMediaType, .SelectedIndex), sLookUpcode:=sLookUpcode)
            End With

            If sLookUpcode = ACMediaTypeBank Then
                m_lReturn = CheckElectronicPayments()
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                '
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to check electronic payment options", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check electronic payment options", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryAndEnabledFields", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

        End If
        If cboReceiptType.Text = "Bank Guarantee Debt" Then
            m_lReturn = GetAllPolicyBG()
        End If

        If SplitTotal Then
            txtName.Text = NameLead
        End If
        If IsSplitReceipt = True And IsLeadAccount = False Then

            With uctReceiptCC
                .ViewOnlyMode = True
                .CCName = CStr(CCname.Trim())
                .CCNumber = CStr(CCnumber.Trim())

                .CCExpiry = CStr(CStr(CCexpirydate).Trim())
                .CCIssue = CStr(CCissue.Trim())
                .CCPIN = CStr(CCpin.Trim())

                .CCStart = CStr(CStr(CCstartdate).Trim())
                .CCAutoAuthCode = CStr(CCauthcode.Trim())
                .CCManualAuthCode = CStr(CCmanualauthcode.Trim())
                .CCCustomerFlag = CStr(CCcustomer.Trim())
                .CCTransactionCode = CStr(CCtransactioncode.Trim())
                .MediaTypeID = MediaTypeID
                .AccountID = AccountID
                .MediaTypeIssuerID = MediaTypeIssuerID
                .CCBankId = CCBankId
                .CardTypeId = CardTypeId
                .CardTransSlipNo = CardTransSlipNo

                .SetSplitReceiptDefaults()
            End With

        End If

    End Sub


    Private Sub uctAddress_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctAddress.LostFocus
        Address1 = uctAddress.AddressLine1
        Address2 = uctAddress.AddressLine2
        Address3 = uctAddress.AddressLine3
        Address4 = uctAddress.AddressLine4
        PostalCode = uctAddress.PostCode
        AddressCountry = uctAddress.CountryId
    End Sub
    Private Sub uctAccountLookup_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctAccountLookup.GotFocus
        m_sSaveAccountText = uctAccountLookup.Text
        uctAccountLookup.CompanyId = g_oObjectManager.SourceID
    End Sub
    Private Sub cboBank_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBank.Enter

        m_lReturn = m_oFormFields.GotFocus(cboBank)
    End Sub
    Private Sub cboBank_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBank.Leave

        m_lReturn = m_oFormFields.LostFocus(cboBank)
    End Sub
    Private Sub cboReversalReason_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReversalReason.Enter

        m_lReturn = m_oFormFields.GotFocus(cboReversalReason)
    End Sub
    Private Sub cboReversalReason_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReversalReason.Leave

        m_lReturn = m_oFormFields.LostFocus(cboReversalReason)
    End Sub
    Private Sub chkInPossession_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInPossession.Enter

        m_lReturn = m_oFormFields.LostFocus(chkInPossession)
    End Sub
    Private Sub chkInPossession_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInPossession.Leave

        m_lReturn = m_oFormFields.LostFocus(chkInPossession)
    End Sub
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try

            'TR - See if the cmdOK button is currently supporting the "Approve" action
            If cmdCancel.Text.ToLower() = "decline" Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                'TR - Approve this Cash List Item
                m_lReturn = DeclineCashListItem()

                '**************
                ' MEvans : 16-09-2003 : CQ1745
                ' only return ok for the claim approval
                ' so we can update the task to complete for decline
                If m_lApprovalType = ACApprovalTypeClaimPayment Then

                    ' added check to make sure it was sucessful
                    ' before we return the ok code..
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    Else
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If

                End If
                '**************

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Me.Hide()
            Else

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
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

    '*************************************************************************
    'Name:          ProcessBankBatch
    'Description:   Checks to see if a Batch exists with reference supplied.
    '               If no batch exists then user can create a new one
    'History:       DD 21/10/2003 - Created
    '*************************************************************************
    Private Function ProcessBankBatch(ByRef sBatchRef As String, ByRef iBatchID As Integer) As Integer

        Dim result As Integer
        result = gPMConstants.PMEReturnCode.PMFalse
        Try


            If m_oBusiness.SelectBatchRecord(sBatchRef:=sBatchRef, r_lBatchID:=iBatchID) <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Select Batch Record failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If iBatchID = 0 Then
                If MessageBox.Show("This Bank Reference has not been used before." &
                                   Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to create a new Batch?", "Create New Batch", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                    'BatchStatus=1=Registered
                    'BatchType=3=Standard Receipt

                    If m_oBusiness.CreateBatchRecord(r_lBatchID:=iBatchID, v_lBatchStatusID:=1, v_lCompanyID:=g_iSourceID, v_lUserID:=g_iUserID, v_sBatchRef:=sBatchRef, v_dtCreatedDate:=DateTime.Today, v_sComment:="", v_lBatchTypeID:=3) <> gPMConstants.PMEReturnCode.PMTrue Then

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            If iBatchID <> 0 Then
                'Store the Batch in the local registry for reusing later
                If gPMFunctions.SetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMERegSettingLevel.pmeRSLClient, "LastBankReference", sBatchRef) <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to store the Bank Reference in the local Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                BatchID = iBatchID
            End If
            result = gPMConstants.PMEReturnCode.PMTrue
            Return result
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessBankBatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try

    End Function

    '*************************************************************************
    'Name:          CheckElectronicPayments
    'Description:   Checks that account holder is allowed to make electronic
    '               payments if systems option is enabled.
    '
    'History:       RDC 14112003 - created
    '*************************************************************************
    Private Function CheckElectronicPayments() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' RDC for payments only
            If ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) And uctAccountLookup.Text.Trim() <> "" Then
                ' RDC 14112003 if system option AllowElectronicPayment=true, and
                ' account property AllowElectronicPayment=false then reject.
                If m_bOptionElectronicPayment And Not (m_bAccountElectronicPayment) Then
                    ''Start(Saurabh) PN 59044
                    txtMediaRef.Text = ""
                    ''End(Saurabh) PN 59044
                    cmbMediaType.SelectedIndex = -1
                    '''PN68977
                    MessageBox.Show("This account holder is not allowed to raise electronic payments", "Account Holder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckElectronicPayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function CheckInstalmentsChosen() As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CheckInstalmentsChosen
        ' PURPOSE: Returns true if at least one instalment is chosen.
        ' AUTHOR: Danny Davis
        ' DATE: 23 March 2005, 11:52:57
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Boolean = False

        'PN25099
        If Not Information.IsArray(InstalmentArray) Then Return result

        For lRow As Integer = 0 To InstalmentArray.GetUpperBound(1)
            If InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                Return True
            End If
        Next lRow

        Return result
    End Function

    '*************************************************************************
    'Name:          GetCurrencyConversions
    'Description:   Checks that account holder is allowed to make electronic
    '               payments if systems option is enabled.
    '*************************************************************************
    Private Function GetCurrencyConversions() As Integer
        Dim result As Integer = 0
        Dim fConversions As frmConversions
        Dim vOverrideDate, vOverrideRate As Object

        Dim vHasPaymentsLimit As Object
        Dim vPaymentsAmount As Object
        Dim vPaymentsCurrencyID As Integer
        Dim bAllowOverride As Boolean
        Dim sResult As String = ""
        Dim cPaymentBaseLimit As Decimal
        Dim sPaymentBaseLimit As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get UserAuthorities business object
            m_lReturn = CreateUserAuthorities()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, CreateUserAuthorities")
            End If

            'Get authority details for current user.

            m_lReturn = m_oUserAuthorities.GetDetails(vUserId:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                           "m_oUserAuthorities.GetDetails")
            End If

            'Get override options for current user.

            m_lReturn = m_oUserAuthorities.GetNext(vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vHasPaymentsAuthority:=vHasPaymentsLimit, vPaymentsAmount:=vPaymentsAmount, vPaymentsCurrencyID:=vPaymentsCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                           "m_oUserAuthorities.GetNext")
            End If

            bAllowOverride = CInt(vOverrideDate) = 1 Or CInt(vOverrideRate) = 1

            ' option 156 (show conversion form)
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=157, r_sOptionValue:=sResult)

            fConversions = New frmConversions()

            If (bAllowOverride Or sResult.Trim() = "1") And (Not m_bSilentMultiCurrencyScreen) Then
                fConversions.DoNotShow = False
            Else
                'If user has not got the authority to override rates and show currency screen
                'is not set, then do not show the currency screen.
                fConversions.DoNotShow = True
            End If

            'Set input parameters
            fConversions.AccountID = uctAccountLookup.AccountId
            fConversions.SourceID = CompanyID
            fConversions.CurrencyID = CurrencyID

            fConversions.Amount = m_oFormFields.UnformatControl(txtAmount)

            fConversions.EffectiveDate = CDate(gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, txtTransDate.Text))
            fConversions.frmConversions_Load()
            'If base and transaction currencies are different then show form
            If fConversions.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Offset this form relative to parent
                m_lReturn = iACTFunc.SetChildFormPosition(frmParent:=Me, frmChild:=fConversions)

                fConversions.ShowDialog()

                If fConversions.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Set output parameters
            CurrencyBaseDate = fConversions.CurrencyBaseDate
            CurrencyBaseXRate = fConversions.CurrencyBaseXRate
            AccountBaseDate = fConversions.AccountBaseDate
            AccountBaseXrate = fConversions.AccountBaseXrate
            SystemBaseDate = fConversions.SystemBaseDate
            SystemBaseXrate = fConversions.SystemBaseXrate
            OverrideReason = fConversions.OverrideReason
            m_cBaseAmount = fConversions.BaseAmount

            fConversions.Close()
            fConversions = Nothing

            If result = gPMConstants.PMEReturnCode.PMTrue And vHasPaymentsLimit = 1 Then

                CreateCurrencyConvert()

                m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=vPaymentsCurrencyID, lCompanyID:=CompanyID, cBaseAmount:=cPaymentBaseLimit, cCurrencyAmount:=vPaymentsAmount, vConversionDate:=Nothing, vConversionRate:=Nothing, vIsMultiplier:=False, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=sPaymentBaseLimit, lEuro:=0, cEuroAmount:=0, vEuroCCyXrate:=Nothing, vEuroBaseXRate:=Nothing, vCCyAmountUnRounded:=Nothing, vBaseAmountUnRounded:=0)
                ' only cashlistitem payments are subject to payment authorities
                ' cashlistitem receipts should be exempt from these checks PN17327
                If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then

                    If cPaymentBaseLimit < (m_oFormFields.UnformatControl(txtAmount) * CurrencyBaseXRate) Then
                        MessageBox.Show("This Payment exceeds your limit of " & sPaymentBaseLimit & ".", "Payment limit exceeded", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyConversions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function CreateCurrencyConvert() As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oCurrencyConvert Is Nothing Then
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateCurrencyConvert", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result
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

    ' ***************************************************************** '
    '
    ' Name: LoadIssuerCombo
    '
    ' Description:  Loads the Issuer combo.
    '
    ' ***************************************************************** '
    Private Function LoadIssuerCombo() As Integer

        Dim result As Integer = 0
        Dim vMediaTypeIssuers(,) As Object
        Dim bIsClaimPayment As Boolean
        Dim iLookUpID As Integer
        Const ACClaimPaymentType As String = "CLP"
        Const ACMediaTypeIssuerID As Integer = 0
        Const ACMediaTypeIssuerDesc As Integer = 1
        Dim sLookUpcode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) And (cboPaymentType.SelectedIndex <> -1) Then
                iLookUpID = VB6.GetItemData(Me.cboPaymentType, Me.cboPaymentType.SelectedIndex)

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupPaymentTypeTable, iLookUpID:=iLookUpID, sLookUpcode:=sLookUpcode)

                bIsClaimPayment = sLookUpcode = ACClaimPaymentType
            End If

            m_lReturn = m_oBusiness.GetMediaTypeIssuer(lMediaTypeID:=VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex), iIsClaimPayment:=bIsClaimPayment, r_vOutputDetails:=vMediaTypeIssuers)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetMediaTypeIssuer", "sCallingAppName = " & ACApp)
            End If

            If Information.IsArray(vMediaTypeIssuers) Then
                With cboIssuer
                    .Visible = True
                    lblIssuer.Visible = True
                    .Items.Clear()

                    For iItem As Integer = 0 To vMediaTypeIssuers.GetUpperBound(1)
                        .Items.Add(New VB6.ListBoxItem(CStr(vMediaTypeIssuers(ACMediaTypeIssuerDesc, iItem)), CInt(vMediaTypeIssuers(ACMediaTypeIssuerID, iItem))))
                    Next
                    .SelectedIndex = -1
                End With
            Else
                cboIssuer.Visible = False
                lblIssuer.Visible = False

                ' For credit card payments/receipts we must let uctACTCreditCard immediately know of
                ' any changes to the Issuer as it will effect display and validation of the control.
                If CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    uctReceiptCC.MediaTypeIssuerID = 0
                Else
                    If (CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                        uctPaymentCC.MediaTypeIssuerID = 0
                    End If
                End If

            End If



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadIssuerCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadIssuerCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally



        End Try
        Return result
    End Function

    Private Function IsDeleted(ByVal v_lAccountId As Integer, ByRef r_bIsDeleted As Boolean) As Integer
        Dim result As Integer = 0
        Dim bACTAccount As Object

        Const kMethodName As String = "IsDeleted"

        Dim oAccount As bACTAccount.Form

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get account business object
            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAccount = temp_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTAccount.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oAccount.IsDeleted(v_lAccountId, r_bIsDeleted)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oAccount.IsDeleted", "Function failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            oAccount.Dispose()
            oAccount = Nothing



            ' This is for debugging only



        End Try
        Return result
    End Function

    ''' <summary>
    ''' SetupClaimPaymentDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetupClaimPaymentDetails() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SetupClaimPaymentDetails"
        Dim nReturn As gPMConstants.PMEReturnCode
        Try
            ' ensure there is a claim payment id before doing anything
            If m_lClaimPaymentId <> 0 Then

                ' get the claim payment details for the specified claim payment id
                nReturn = CType(GetClaimPaymentAccountsDetails(), gPMConstants.PMEReturnCode)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' save details to be defaulted
                txtPayeeName.Text = m_sClaimPaymentPayeeName
                txtPaymentAccountCode.Text = m_sClaimPaymentBankAccountNo
                txtPaymentBranchCode.Text = m_sClaimPaymentBankSortCode
                uctAddress.CountryId = m_lClaimPaymentPayeeCountryId
                txtFurtherDetails.Text = m_sClaimPaymentPayeeComments
                txtMediaRef.Text = m_sClaimPaymentMediaRef
                txtBIC.Text = m_sClaimPaymentBIC
                txtIBAN.Text = m_sClaimPaymentIBAN

                If m_sClaimPaymentAddress1.Trim() <> "" Then
                    uctAddress.AddressLine1 = m_sClaimPaymentAddress1
                    uctAddress.AddressLine2 = m_sClaimPaymentAddress2
                    uctAddress.AddressLine3 = m_sClaimPaymentAddress3
                    uctAddress.AddressLine4 = m_sClaimPaymentAddress4
                    uctAddress.PostCode = m_sClaimPaymentPostCode
                Else
                    uctAddress.AddressLine1 = Address1
                    uctAddress.AddressLine2 = Address2
                    uctAddress.AddressLine3 = Address3
                    uctAddress.AddressLine4 = Address4
                    uctAddress.PostCode = PostalCode
                    uctAddress.CountryId = AddressCountry
                End If

                txtTheirRef.Text = m_sClaimPaymentThirdPartyReference
                txtOurRef.Text = m_sClaimPaymentOurRef
                If gPMFunctions.ToSafeInteger(m_lBankPaymentTypeId) <> 0 Then
                    uctPartyBankCombo1.SelectedPaymentID = gPMFunctions.ToSafeInteger(m_lBankPaymentTypeId)
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' GetClaimPaymentAccountsDetails
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClaimPaymentAccountsDetails() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "GetClaimPaymentAccountsDetails"

        Dim nReturn As gPMConstants.PMEReturnCode
        Dim oClaimPaymentDetails As Object

        Try
            Debug.WriteLine(m_oBusiness.UnderwritingOrAgency)

            ' get claim payment details to be defaulted
            nReturn = m_oBusiness.GetClaimPaymentAccountsDetails(v_lClaimPaymentId:=m_lClaimPaymentId, r_vResults:=oClaimPaymentDetails)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentsAccountsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(oClaimPaymentDetails) Then
                ' save details to be defaulted
                m_sClaimPaymentPayeeName = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeName, 0))
                m_sClaimPaymentBankSortCode = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsBankSortCode, 0))
                m_sClaimPaymentBankAccountNo = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsBankAccountNo, 0))
                m_sClaimPaymentBIC = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsBIC, 0))
                m_sClaimPaymentIBAN = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsIBAN, 0))
                m_lClaimPaymentPayeeCountryId = gPMFunctions.ToSafeLong(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeCountry, 0), 0)
                m_sClaimPaymentPayeeComments = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeComments, 0))
                m_sClaimPaymentMediaRef = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsMediaRef, 0))
                m_sClaimPaymentAddress1 = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeAddress1, 0))
                m_sClaimPaymentAddress2 = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeAddress2, 0))
                m_sClaimPaymentAddress3 = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeAddress3, 0))
                m_sClaimPaymentAddress4 = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeeAddress4, 0))
                m_sClaimPaymentPostCode = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsPayeePostalCode, 0))
                m_sClaimPaymentThirdPartyReference = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsThirdPartyReference, 0))
                m_sClaimPaymentOurRef = CStr(oClaimPaymentDetails(kClaimPaymentAccountDetailsOurReference, 0))
                m_lBankPaymentTypeId = gPMFunctions.ToSafeLong(oClaimPaymentDetails(kClaimPaymentAccountDetailsPartyBankID, 0))
            Else
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentsAccountsDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentTabData
    '
    ' Parameters: lAccountID
    '
    ' Description: Gets data for and populates Payment Tab
    '
    ' History:
    '           Created : Rajesh Choudhary : Date : 30 Nov 2006
    ' ***************************************************************** '
    Function GetPaymentTabData(ByVal lAccountID As Integer) As Integer

        m_lReturn = AccountPropertiesToInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("AccountPropertiesToInterface failed: " & m_lReturn, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'set payment tab data

        m_lReturn = m_oFormFields.FormatControl(cboPaymentType, CashListItemPaymentTypeID)
        If uctPartyBankCombo1.SelectedPaymentID <> 0 Then

            m_lReturn = m_oFormFields.FormatControl(txtPayeeName, PaymentName)

            m_lReturn = m_oFormFields.FormatControl(txtPaymentAccountCode, PaymentAccountCode)
            m_lReturn = m_oFormFields.FormatControl(txtBIC, sBIC)
            m_lReturn = m_oFormFields.FormatControl(txtIBAN, sIBAN)
            m_lReturn = m_oFormFields.FormatControl(txtPaymentBranchCode, PaymentBranchCode)

            m_lReturn = m_oFormFields.FormatControl(txtPaymentExpiryDate, PaymentExpiryDate)

            If txtPaymentExpiryDate.Text.Trim() = "00:00:00" Then txtPaymentExpiryDate.Text = "" 'PN18882

            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference1, PaymentReference1)

            m_lReturn = m_oFormFields.FormatControl(txtPaymentReference2, PaymentReference2)

            m_lReturn = m_oFormFields.FormatControl(txtDatePresented, Datepresented)

            If txtDatePresented.Text.Trim() = "00:00:00" Then txtDatePresented.Text = ""

            m_lReturn = m_oFormFields.FormatControl(chkInPossession, Chequeinpossession)

            m_lReturn = m_oFormFields.FormatControl(txtStopRequested, Stoprequesteddate)

            If txtStopRequested.Text.Trim() = "00:00:00" Then txtStopRequested.Text = ""

            m_lReturn = m_oFormFields.FormatControl(txtConfirmation, Stopconfirmationdate)

            If txtConfirmation.Text.Trim() = "00:00:00" Then txtConfirmation.Text = ""

            If Not Me.m_bReverseCashDrawerListItem Then

                m_lReturn = m_oFormFields.FormatControl(txtPaymentReason, Reason)
            End If
        End If

        m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, CashListItemPaymentStatusID)

        m_lReturn = FillPartyBankCombo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MsgBox("FillPartyBankCombo failed: " & CStr(m_lReturn), vbCritical)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

    End Function

    Private Function CheckAccount(ByVal v_lAccountId As Integer, ByRef r_bFailed As Boolean) As Integer
        Dim result As Integer = 0
        Dim bACTCashList, bACTBankAccount As Object

        Const kMethodName As String = "CheckAccount"

        Dim oBankAccount As bACTBankAccount.Form

        Dim oCashList As bACTCashList.Form
        Dim lBankAccountID, lAccountID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oCashList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oCashList, "bACTCashList.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCashList = temp_oCashList
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTCashList.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_oBankAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBankAccount, "bACTBankAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBankAccount = temp_oBankAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:='bACTAccount.Form'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oCashList.GetDetails(vCashListID:=CashlistID)

            m_lReturn = oCashList.GetNext(vBankAccountID:=lBankAccountID)

            m_lReturn = oBankAccount.GetDetails(vBankAccountId:=lBankAccountID)

            m_lReturn = oBankAccount.GetNext(vAccountId:=lAccountID)

            'Do not allow the user to select an account that is the same as the bank account selected.
            r_bFailed = v_lAccountId = lAccountID



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            r_bFailed = True

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oBankAccount Is Nothing) Then

                oBankAccount.Dispose()
            End If
            oBankAccount = Nothing

            If Not (oCashList Is Nothing) Then

                oCashList.Dispose()
            End If
            oCashList = Nothing



            ' This is for debugging only



        End Try
        Return result
    End Function

    Public Function GetPartyBanks() As Integer
        Dim result As Integer = 0
        Dim bSIRPartyBank As Object

        Const kMethodName As String = "GetPartyBanks"

        Dim lReturn As Integer

        Dim oPartyBank As bSIRPartyBank.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If uctAccountLookup.Text <> "" Then

                m_lReturn = m_oBusiness.GetPartyCntFromAccountID(gPMFunctions.ToSafeLong(uctAccountLookup.AccountId), m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to fetch PartyCnt", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            Else
                m_lPartyCnt = 0
            End If

            Dim temp_oPartyBank As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPartyBank, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPartyBank = temp_oPartyBank

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error.
                gPMFunctions.RaiseError("FillPartyBankCombo", "Unable to get instance of  bSIRPartyBank.Business")
            End If

            m_lReturn = oPartyBank.GetPartyBankDetails(vPartyCnt:=m_lPartyCnt, vPartyBankDetails:=m_vPartyBankDetails, vAccountID:=DBNull.Value)

            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lAccountID = AccountID


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
    'Party Bank Details
    Private Function FillPartyBankCombo() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FillPartyBankCombo"

        Dim oPartyBank As Object
        Dim iLookUpID As Integer
        Dim sLookUpcode, sLookUpCaption As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Me.cmbMediaType.SelectedIndex > -1 Then
                iLookUpID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpCaption:=sLookUpCaption, sLookUpcode:=sLookUpcode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    '
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("FillPartyBankCombo", "Unable to get GetLookupSingle Media Type")
                End If
            End If
            uctPartyBankCombo1.Task = m_iTask
            If (uctAccountLookup.AccountId <> 0 And sLookUpcode = "CC" And SSTabHelper.GetTabVisible(tabMainTab, 1) And
                (ViaInsurerPayment OrElse (m_lAccountID <> AccountID))) _
               OrElse (uctAccountLookup.AccountId <> 0 And SSTabHelper.GetTabVisible(tabMainTab, 2) And (ViaInsurerPayment OrElse m_lAccountID <> AccountID)) _
               OrElse (uctAccountLookup.AccountId <> 0 And sLookUpcode = "CC" And SSTabHelper.GetTabVisible(tabMainTab, 1) AndAlso Me.cmbMediaType.SelectedIndex > -1) Then

                m_lReturn = GetPartyBanks()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetPartyBanks", "GetPartyBanks Failed")
                End If
                m_lReturn = RefreshAccountDetails()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RefreshAccountDetails", "fail to call RefreshAccountDetails")
                End If
            End If

            If uctAccountLookup.Text <> "" Then

                m_lReturn = m_oBusiness.GetPartyCntFromAccountID(gPMFunctions.ToSafeLong(uctAccountLookup.AccountId), m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to fetch PartyCnt", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            Else
                m_lPartyCnt = 0
                AccountID = m_lPartyCnt
                RefreshAccountDetails()
                If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
                    m_lReturn = ClearFields(0)
                    lblReceiptAccountType.Enabled = False
                    uctPartyBankCombo2.EnableCombo = False
                ElseIf (SSTabHelper.GetTabVisible(tabMainTab, 2)) Then
                    m_lReturn = ClearFields(1)
                    lblPaymentAccountType.Enabled = False
                    uctPartyBankCombo1.EnableCombo = False
                End If
            End If

            m_lAccountID = AccountID



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here


            ' This is for debugging only



        End Try
        Return result
    End Function

    Private Sub uctPartyBankCombo1_ComboChange(ByVal Sender As Object, ByVal e As uctPartyBankCombo.ComboChangeEventArgs) Handles uctPartyBankCombo1.ComboChange
        If e.lSelItemID > 0 Then
            FillPartyBankDetails()
        Else
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                ClearFields(1)
                EnableDisableBankControls(True)
            End If
        End If
    End Sub

    Private Sub uctPartyBankCombo2_ComboChange(ByVal Sender As Object, ByVal e As uctPartyBankCombo.ComboChangeEventArgs) Handles uctPartyBankCombo2.ComboChange
        If e.lSelItemID > 0 Then
            FillPartyBankDetails()
        Else
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
               ' ClearFields(0)
                EnableDisableBankControls(True)
            End If
        End If
    End Sub

    Private Sub uctPartyBankCombo1_AddPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBankCombo.AddPartyBankItemEventArgs) Handles uctPartyBankCombo1.AddPartyBankItem
        m_lReturn = GetPartyBanks()
        FillPartyBankDetails()
    End Sub

    Private Sub uctPartyBankCombo2_AddPartyBankItem(ByVal Sender As Object, ByVal e As uctPartyBankCombo.AddPartyBankItemEventArgs) Handles uctPartyBankCombo2.AddPartyBankItem
        m_lReturn = GetPartyBanks()
        FillPartyBankDetails()
    End Sub

    ''' <summary>
    ''' Party Bank Details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FillPartyBankDetails() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "FillPartyBankDetails"

        Dim oPartyBankId, oIsBank, oAccountId, oBankPaymentTypeId, oBankAccountTypeId, oAccountHolderName, oAccountNumber, oBankNameId, oBankName, oBankBranch, oBankBranchCode, oBankAddress1, oBankAddress2, oBankAddress3, oBankTown, oBankPostCode, oBankRegion, oBankCountry, oCardNumber, oCardStart, oCardExpiryDate, oCardIssueNumber, oCardPin, oCardIsRegistered, oCardAddress1, oCardAddress2, oCardAddress3, oCardTown, oCardPostCode, oCardCountry, oIsDeleted As Object
        Dim bMatchFound As Boolean
        Dim nBankPaymentId As Integer
        Dim oNameOnCard, oManualAuthCode As Object
        Dim sBIC As String = String.Empty
        Dim sIBAN As String = String.Empty

        Try

            If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                nBankPaymentId = uctPartyBankCombo1.SelectedPaymentID
            Else
                nBankPaymentId = uctPartyBankCombo2.SelectedPaymentID
            End If

            If (Information.IsArray(m_vPartyBankDetails) Or nBankPaymentId > 0) And m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                If Information.IsArray(m_vPartyBankDetails) Then
                    For lPaymentCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                        If CDbl(m_vPartyBankDetails(ENPartyBank.PartyBankId, lPaymentCount)) = nBankPaymentId Then

                            oPartyBankId = m_vPartyBankDetails(ENPartyBank.PartyBankId, lPaymentCount)

                            oIsBank = m_vPartyBankDetails(ENPartyBank.IsBank, lPaymentCount)

                            oAccountId = m_vPartyBankDetails(ENPartyBank.AccountID1, lPaymentCount)

                            oBankPaymentTypeId = m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, lPaymentCount)(ENPMLookups.Id)

                            oBankAccountTypeId = m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, lPaymentCount)

                            oAccountHolderName = m_vPartyBankDetails(ENPartyBank.AccountHolderName, lPaymentCount)

                            oAccountNumber = m_vPartyBankDetails(ENPartyBank.AccountNumber, lPaymentCount)
                            sBIC = m_vPartyBankDetails(ENPartyBank.BIC, lPaymentCount)
                            sIBAN = m_vPartyBankDetails(ENPartyBank.IBAN, lPaymentCount)
                            If oIsBank <> 0 Then
                                If m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount).Length = 0 Then
                                    oBankNameId = ""
                                    oBankName = ""
                                Else
                                    oBankNameId = m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount)(ENPMLookups.Id)

                                    oBankName = m_vPartyBankDetails(ENPartyBank.BankNameId, lPaymentCount)(ENPMLookups.Description)

                                End If

                                oBankBranch = m_vPartyBankDetails(ENPartyBank.BankBranch, lPaymentCount)

                                oBankBranchCode = m_vPartyBankDetails(ENPartyBank.BankBranchCode, lPaymentCount)

                                oBankAddress1 = m_vPartyBankDetails(ENPartyBank.BankAdd1, lPaymentCount)

                                oBankAddress2 = m_vPartyBankDetails(ENPartyBank.BankAdd2, lPaymentCount)

                                oBankAddress3 = m_vPartyBankDetails(ENPartyBank.BankAdd3, lPaymentCount)

                                oBankTown = m_vPartyBankDetails(ENPartyBank.BankTown, lPaymentCount)

                                oBankPostCode = m_vPartyBankDetails(ENPartyBank.BankPCode, lPaymentCount)

                                oBankRegion = m_vPartyBankDetails(ENPartyBank.BankRegion, lPaymentCount)

                                oBankCountry = m_vPartyBankDetails(ENPartyBank.BankCountry, lPaymentCount)
                            Else

                                oCardNumber = m_vPartyBankDetails(ENPartyBank.CCNum, lPaymentCount)

                                oCardStart = m_vPartyBankDetails(ENPartyBank.CCstartdate1, lPaymentCount)

                                oCardExpiryDate = m_vPartyBankDetails(ENPartyBank.CCexpirydate1, lPaymentCount)

                                oCardIssueNumber = m_vPartyBankDetails(ENPartyBank.CCIssueNum, lPaymentCount)

                                oCardPin = m_vPartyBankDetails(ENPartyBank.CCpin1, lPaymentCount)

                                oCardIsRegistered = m_vPartyBankDetails(ENPartyBank.IsRegistered, lPaymentCount)

                                oCardAddress1 = m_vPartyBankDetails(ENPartyBank.CCAdd1, lPaymentCount)

                                oCardAddress2 = m_vPartyBankDetails(ENPartyBank.CCAdd2, lPaymentCount)

                                oCardAddress3 = m_vPartyBankDetails(ENPartyBank.CCAdd3, lPaymentCount)

                                oCardTown = m_vPartyBankDetails(ENPartyBank.CCTown, lPaymentCount)

                                oCardPostCode = m_vPartyBankDetails(ENPartyBank.CCPCode, lPaymentCount)

                                oCardCountry = m_vPartyBankDetails(ENPartyBank.CCCountry, lPaymentCount)
                            End If
                            oIsDeleted = m_vPartyBankDetails(ENPartyBank.IsDeleted1, lPaymentCount)

                            oNameOnCard = m_vPartyBankDetails(ENPartyBank.CCNameOnCard, lPaymentCount)

                            oManualAuthCode = m_vPartyBankDetails(ENPartyBank.CCManualAuthNumber, lPaymentCount)
                            bMatchFound = True
                        End If
                    Next

                    If bMatchFound Then

                        If SSTabHelper.GetTabVisible(tabMainTab, 2) And CStr(oIsBank) = "1" Then
                            If (oPartyBankId <> m_lBankPaymentTypeId) Or m_lClaimPaymentId = 0 Then
                                m_lReturn = ClearFields(1)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "ClearFields Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                txtPayeeName.Text = CStr(oAccountHolderName)

                                txtPaymentAccountCode.Text = CStr(oAccountNumber)

                                txtPaymentBranchCode.Text = CStr(oBankBranchCode)
                                txtBIC.Text = CStr(sBIC)
                                txtIBAN.Text = CStr(sIBAN)
                                'Disable the Controls
                                EnableDisableBankControls(False)
                            Else
                                SetupClaimPaymentDetails()
                            End If
                        ElseIf SSTabHelper.GetTabVisible(tabMainTab, 2) And CStr(oIsBank) = "0" Then
                            uctPaymentCC.ShowPartyCreditCardScreen()

                            uctPaymentCC.CCNumber1 = CStr(oCardNumber)

                            uctPaymentCC.CCExpiry = CStr(oCardExpiryDate)

                            uctPaymentCC.CCStart = CStr(oCardStart)

                            uctPaymentCC.CCIssue = CStr(oCardIssueNumber)

                            uctPaymentCC.CCPIN = CStr(oCardPin)

                            uctPaymentCC.CCName = CStr(oNameOnCard)

                            uctPaymentCC.CCManualAuthCode = CStr(oManualAuthCode)
                            uctPaymentCC.ViewOnlyMode = True

                        ElseIf (SSTabHelper.GetTabVisible(tabMainTab, 1) And CStr(oIsBank) = "0") Then
                            m_lReturn = ClearFields(0)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "ClearFields Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            uctReceiptCC.CCNumber1 = CStr(oCardNumber)

                            uctReceiptCC.CCExpiry = CStr(oCardExpiryDate)

                            uctReceiptCC.CCStart = CStr(oCardStart)

                            uctReceiptCC.CCIssue = CStr(oCardIssueNumber)

                            uctReceiptCC.CCPIN = CStr(oCardPin)

                            uctReceiptCC.CCName = CStr(oNameOnCard)

                            uctReceiptCC.CCManualAuthCode = CStr(oManualAuthCode)
                            If ACMediaTypeCreditCard = "CC" Then
                                fraReceipt(1).Enabled = True
                            End If
                        End If
                    Else
                        EnableDisableBankControls(True)
                    End If
                End If
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        Finally




        End Try
        Return nResult
    End Function
    Private Sub EnableDisableBankControls(ByRef bEnable As Boolean)

        txtPayeeName.Enabled = bEnable
        txtPaymentAccountCode.Enabled = bEnable
        txtPaymentBranchCode.Enabled = bEnable
        txtPaymentExpiryDate.Enabled = bEnable
        txtPaymentReference1.Enabled = bEnable
        txtPaymentReference2.Enabled = bEnable
        txtBIC.Enabled = bEnable
        txtIBAN.Enabled = bEnable
        fraReceipt(1).Enabled = bEnable
        ''68983
        If Not txtPaymentReference1.Enabled Then
            txtPaymentReference1.Text = ""
        End If
        If Not txtPaymentReference2.Enabled Then
            txtPaymentReference2.Text = ""
        End If

    End Sub
    Private Function ClearFields(ByRef bClearBankFields As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ClearFields"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'For Bank
            If bClearBankFields Then

                ''''commenting next line to solve bug
                'txtPayeeName.Text = ""
                txtPaymentAccountCode.Text = ""
                txtPaymentBranchCode.Text = ""
                txtBIC.Text = ""
                txtIBAN.Text = ""
                'For Credit Card
            ElseIf (Not bClearBankFields) Then
                uctReceiptCC.CCNumber = ""
                uctReceiptCC.CCExpiry = ""
                uctReceiptCC.CCStart = ""
                uctReceiptCC.CCIssue = ""
                uctReceiptCC.CCPIN = ""
                uctReceiptCC.CCName = ""
                uctReceiptCC.CCManualAuthCode = ""
                uctReceiptCC.CCNumber1 = ""
                'WPR12- Enhancement Quote Collection Process
                uctReceiptCC.CCBankId = -1
                uctReceiptCC.CardTypeId = -1
                uctReceiptCC.CardTransSlipNo = ""
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function RefreshAccountDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "RefreshAccountDetails"
        Try

            Dim sLookUpcode As String = ""
            Dim iLookUpID As Integer
            Dim sLookUpCaption As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            If Me.cmbMediaType.SelectedIndex > -1 Then
                iLookUpID = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpCaption:=sLookUpCaption, sLookUpcode:=sLookUpcode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    '
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RefreshAccountDetails", "Unable to get GetLookupSingle Media Type")
                End If
            Else
                'Used the else part for first time loading the default value so that system would be able to fetch sLookUpcode for further use.
                iLookUpID = MediaTypeID

                m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpCaption:=sLookUpCaption, sLookUpcode:=sLookUpcode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    '
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RefreshAccountDetails", "Unable to get GetLookupSingle Media Type")
                End If
            End If

            If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                uctPartyBankCombo1.EnableCombo = True
                lblPaymentAccountType.Enabled = True
                ' Start - Sankar - PN 56728
                If sLookUpcode = ACMediaTypeCreditCard Then

                    uctPartyBankCombo1.IsBank = 0
                Else

                    uctPartyBankCombo1.IsBank = 1
                End If
                ' End - Sankar - PN 56728

                uctPartyBankCombo1.PartyCnt = m_lPartyCnt
                If m_lPartyBankId > 0 Then
                    uctPartyBankCombo1.SelectedPaymentID = m_lPartyBankId
                End If
                If CashlistTypeID <> 3 Then
                    uctPartyBankCombo1.BankPaymentTypeCode = "RECPAY"
                Else
                    uctPartyBankCombo1.BankPaymentTypeCode = "CLM"
                End If
                uctPartyBankCombo1.Task = m_iTask

                uctPartyBankCombo1.Initialise()
                uctPartyBankCombo1.PopulateScreen()
            Else
                If SSTabHelper.GetTabVisible(tabMainTab, 1) And sLookUpcode <> "CC" Then
                    m_lPartyCnt = 0
                End If
                uctPartyBankCombo2.EnableCombo = True
                lblReceiptAccountType.Enabled = True

                uctPartyBankCombo2.IsBank = 0
                If m_lPartyBankId > 0 Then
                    uctPartyBankCombo2.SelectedPaymentID = m_lPartyBankId
                End If

                uctPartyBankCombo2.PartyCnt = m_lPartyCnt
                If CashlistTypeID <> 3 Then
                    uctPartyBankCombo2.BankPaymentTypeCode = "RECPAY"
                Else
                    uctPartyBankCombo2.BankPaymentTypeCode = "CLM"
                End If
                uctPartyBankCombo2.Task = m_iTask

                uctPartyBankCombo2.Initialise()
                uctPartyBankCombo2.PopulateScreen()
            End If
            m_lAccountID = 0


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GetIsAdditionalDetailsFromMediaArray(ByRef lMediaTypeID As Integer) As Boolean
        'WPR12- Enhancement Quote Collection Process
        Dim result As Boolean = False

        For iCount As Integer = m_vMediaResultArray.GetLowerBound(0) To m_vMediaResultArray.GetUpperBound(0)
            If CInt(m_vMediaResultArray(ACMediaTypeID, iCount)) = lMediaTypeID Then
                If m_vMediaResultArray(ACIsAdditionalOptions, iCount) <> "" AndAlso CInt(m_vMediaResultArray(ACIsAdditionalOptions, iCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                    result = True
                    Exit For
                Else
                    result = False
                    Exit For
                End If
            End If
        Next iCount

        Return result
    End Function

    Private Sub ChangeLabelChequeAndCreditCard()

        Dim sLookUpcode, sLookUpCaption As String

        m_sSaveMediaTypeText = cmbMediaType.Text
        If cmbMediaType.SelectedIndex = -1 Or m_bReverseCashDrawerListItem Then Exit Sub

        Dim iLookUpID As Integer = VB6.GetItemData(Me.cmbMediaType, Me.cmbMediaType.SelectedIndex)

        m_lReturn = m_oListForm.GetLookupSingle(sLookupTable:=gACTLibrary.ACTLookupMediaType, iLookUpID:=iLookUpID, sLookUpCaption:=sLookUpCaption, sLookUpcode:=sLookUpcode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        '''''DO CAPTIONS CHANGE BACK TO NORMAL
        'For Cheque
        If lblMediaRef.Visible Then
            lblMediaRef.Text = "Media Reference:"
        End If
        If lblChequeDate.Visible Then
            lblChequeDate.Text = "Cheque Date:"
        End If
        If lblBank.Visible Then
            lblBank.Text = "Bank:"
        End If
        If lblAmount.Visible Then
            lblAmount.Text = "Amount:"
        End If

        'WPR12- Enhancement Quote Collection Process

        Dim oFF As iPMFormControl.FormField = m_oFormFields.Item("txtMediaRef-0")
        If m_bIsMediaRefMandatory Then
            txtMediaRef.Enabled = True
            oFF = m_oFormFields.Item("txtMediaRef-0")
            oFF.IsMandatory = True
            lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
        End If
        oFF = m_oFormFields.Item("cboBank-0")
        oFF.IsMandatory = False
        lblBank.Font = VB6.FontChangeBold(lblBank.Font, False)

        oFF = m_oFormFields.Item("txtBankLocation-0")
        oFF.IsMandatory = False
        lblBankLocation.Font = VB6.FontChangeBold(lblBankLocation.Font, False)

        oFF = m_oFormFields.Item("txtBankBranch-0")
        oFF.IsMandatory = False
        lblBankBranch.Font = VB6.FontChangeBold(lblBankBranch.Font, False)

        oFF = m_oFormFields.Item("cboChequeClearingType-0")
        oFF.IsMandatory = False
        lblChequeClearingType.Font = VB6.FontChangeBold(lblChequeClearingType.Font, False)

        oFF = m_oFormFields.Item("cboChequeType-0")
        oFF.IsMandatory = False
        lblChequeType.Font = VB6.FontChangeBold(lblChequeType.Font, False)

        lblBankLocation.Enabled = False
        txtBankLocation.Enabled = False

        lblChequeType.Enabled = False
        cboChequeType.Enabled = False

        lblBankBranch.Enabled = False
        txtBankBranch.Enabled = False

        lblChequeClearingType.Enabled = False
        cboChequeClearingType.Enabled = False

        'For Credit Card
        If fraReceipt(1).Enabled Then
            uctReceiptCC.IsAdditionalDetailOption = False
            uctReceiptCC.CaptionNameOnCardAdditionalOption = False
            uctReceiptCC.CaptionExpiryDateAdditionalOption = False
            uctReceiptCC.CaptionCVSPINAdditionalOption = False
        End If

        If sLookUpcode = ACMediaTypeCheque And CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts Then
            'For Cheque
            If GetIsAdditionalDetailsFromMediaArray(cmbMediaType.Items(cmbMediaType.SelectedIndex).ItemData) Then
                lblBankLocation.Enabled = True
                txtBankLocation.Enabled = True

                lblChequeType.Enabled = True
                cboChequeType.Enabled = True

                lblBankBranch.Enabled = True
                txtBankBranch.Enabled = True

                lblChequeClearingType.Enabled = True
                cboChequeClearingType.Enabled = True

                ' If lblMediaRef.Visible Then
                lblMediaRef.Text = "Instrument Number:"
                'End If
                'If lblChequeDate.Visible Then
                lblChequeDate.Text = "Instrument Date:"
                'End If
                'If lblBank.Visible Then
                lblBank.Text = "Drawee Bank Name:"
                'End If
                'If lblAmount.Visible Then
                lblAmount.Text = "Instrument Amount:"
                'End If

                oFF = m_oFormFields.Item("txtMediaRef-0")
                oFF.IsMandatory = True
                lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)

                oFF = m_oFormFields.Item("cboBank-0")
                oFF.IsMandatory = True
                lblBank.Font = VB6.FontChangeBold(lblBank.Font, True)

                oFF = m_oFormFields.Item("txtBankLocation-0")
                oFF.IsMandatory = True
                lblBankLocation.Font = VB6.FontChangeBold(lblBankLocation.Font, True)

                oFF = m_oFormFields.Item("txtBankBranch-0")
                oFF.IsMandatory = True
                lblBankBranch.Font = VB6.FontChangeBold(lblBankBranch.Font, True)

                oFF = m_oFormFields.Item("cboChequeClearingType-0")
                oFF.IsMandatory = True
                lblChequeClearingType.Font = VB6.FontChangeBold(lblChequeClearingType.Font, True)

                oFF = m_oFormFields.Item("cboChequeType-0")
                oFF.IsMandatory = True
                lblChequeType.Font = VB6.FontChangeBold(lblChequeType.Font, True)
            Else
                lblBankLocation.Enabled = False
                txtBankLocation.Enabled = False

                lblChequeType.Enabled = False
                cboChequeType.Enabled = False

                lblBankBranch.Enabled = False
                txtBankBranch.Enabled = False

                lblChequeClearingType.Enabled = False
                cboChequeClearingType.Enabled = False
            End If

        ElseIf sLookUpcode = ACMediaTypeCreditCard And CashlistTypeID = gACTLibrary.ACTCashListTypeReceipts And GetIsAdditionalDetailsFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
            'For Credit Card
            uctReceiptCC.IsAdditionalDetailOption = True
            If fraReceipt(1).Enabled Then
                uctReceiptCC.CaptionNameOnCardAdditionalOption = True
                uctReceiptCC.CaptionExpiryDateAdditionalOption = True
                uctReceiptCC.CaptionCVSPINAdditionalOption = True
            End If
        End If
        If cmbMediaType.SelectedIndex >= 0 And ((CashlistTypeID = gACTLibrary.ACTCashListTypePayments) Or (CashlistTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then
            If GetIsMediaRefMandatoryFromMediaArray(VB6.GetItemData(cmbMediaType, cmbMediaType.SelectedIndex)) Then
                txtMediaRef.Enabled = True

                oFF = m_oFormFields.Item("txtMediaRef-0")
                oFF.IsMandatory = True
                lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)
            End If
        End If

    End Sub

    Private Sub SplitReceiptMandatoryAndEnabledFieldsFields()

        'Enable / Disable Fields
        If IsLeadAccount Then
            'Lead Account
            chkIsLeadAccount.Checked = True
            txtSplitTotal.Enabled = True
            chkIsLeadAccount.Enabled = True
            txtSplitTotal.Enabled = True
            txtName.Enabled = True
            If CollectionHasLead = False Then
                chkIsLeadAccount.Enabled = False
            End If

        Else
            ' Not a lead account & collection has a lead receipt
            txtMediaRef.Enabled = False
            cmbMediaType.Enabled = False
            _fraReceipt_0.Enabled = False
            chkIsLeadAccount.Enabled = False
            txtSplitTotal.Enabled = False
            txtName.Enabled = False
            txtTransDate.Enabled = False
            If ViaInsurerPayment Then
                uctAccountLookup.Enabled = True
            End If

        End If

        If ViaInsurerPayment And Not IsLeadAccount Then
            txtAmount.Enabled = True
        End If

        If ViaInsurerPayment And IsLeadAccount Then

            chkIsLeadAccount.Enabled = False
            If CollectionHasLead = True Then
                cmbMediaType.Enabled = True
                txtSplitTotal.Enabled = True
            Else
                txtSplitTotal.Enabled = False
            End If

        End If

        If m_sCallingAppName = "SplitReceiptsFromFindTransaction" And IsLeadAccount Then
            txtMediaRef.Enabled = False
            chkIsLeadAccount.Enabled = False
            txtSplitTotal.Enabled = True
        End If

        ' Set Mandatory / Remove Mandatory
        Dim oFF As iPMFormControl.FormField = m_oFormFields.Item("txtMediaRef-0")
        oFF.IsMandatory = True
        lblMediaRef.Font = VB6.FontChangeBold(lblMediaRef.Font, True)

        oFF = m_oFormFields.Item("txtSplitTotal-0")
        oFF.IsMandatory = True
        lblSplitTotal.Font = VB6.FontChangeBold(lblSplitTotal.Font, True)

        ' Fill Values
        txtName.Text = NameLead

    End Sub

    Private Function ValidateIntermediateAndClient(ByRef r_bValidation As Boolean, ByRef r_sClientAgent As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateIntermediateAndClient"

        Dim lQuotePartyCnt As Integer
        Dim r_sShortCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sClientAgent = ""

            m_lReturn = m_oBusiness.GetPartyCntFromAccountID(gPMFunctions.ToSafeLong(uctAccountLookup.AccountId), lQuotePartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to fetch PartyCnt", "Invalid Account", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            If lQuotePartyCnt = m_lQuoteClientCnt Or lQuotePartyCnt = m_lQuoteAgentCnt Then
                r_bValidation = False
            Else

                m_lReturn = m_oBusiness.GetShortCodeFromPartyCnt(v_lPartyCnt:=m_lQuoteClientCnt, r_sShortCode:=r_sShortCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to fetch ShortCode From PartyCnt", "Failed to fetch ShortCode", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
                r_sClientAgent = "Client (" & r_sShortCode.Trim() & ")"

                r_sShortCode = ""

                m_lReturn = m_oBusiness.GetShortCodeFromPartyCnt(v_lPartyCnt:=m_lQuoteAgentCnt, r_sShortCode:=r_sShortCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to fetch ShortCode From PartyCnt", "Failed to fetch ShortCode", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
                r_sClientAgent = r_sClientAgent & " Or Agent (" & r_sShortCode.Trim() & ")"

                r_sClientAgent = r_sClientAgent & " as ShortCode"
                r_bValidation = True

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    Private Sub uctPartyBankCombo1_EditPartyBankItem(ByVal Sender As System.Object, ByVal e As uctPartyBank.uctPartyBankCombo.EditPartyBankItemEventArgs) Handles uctPartyBankCombo1.EditPartyBankItem
        m_lReturn = GetPartyBanks()
        FillPartyBankDetails()
    End Sub

    Private Sub FillCombo()
        Me.cboUnderwritingYear.FirstItem = "(None)"
    End Sub

    Private Sub frmDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = e.KeyCode
        Dim Shift As Integer = e.KeyData \ &H10000

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
            Dim i As Integer = 0

            If e.Alt And e.KeyCode = Keys.D1 And SSTabHelper.GetTabVisible(tabMainTab, 0) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage0
            End If
            If e.Alt And e.KeyCode = Keys.D2 And SSTabHelper.GetTabVisible(tabMainTab, 1) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage1
            End If
            If e.Alt And e.KeyCode = Keys.D3 And SSTabHelper.GetTabVisible(tabMainTab, 2) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage2
            End If
            If e.Alt And e.KeyCode = Keys.D4 And SSTabHelper.GetTabVisible(tabMainTab, 3) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage3
            End If
            If e.Alt And e.KeyCode = Keys.D5 And SSTabHelper.GetTabVisible(tabMainTab, 4) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage4
            End If
            If e.Alt And e.KeyCode = Keys.D6 And SSTabHelper.GetTabVisible(tabMainTab, 5) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage5
            End If
            If e.Alt And e.KeyCode = Keys.D7 And SSTabHelper.GetTabVisible(tabMainTab, 6) Then
                tabMainTab.SelectedTab = _tabMainTab_TabPage6
            End If

        Catch

            ' Error Section.

            Exit Sub
        End Try

    End Sub

    Private Sub lvInstalments_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvInstalments.ItemChecked

        Dim Item As ListViewItem = lvInstalments.Items(e.Item.Index)

        Dim lRow As Integer

        Try
            If m_bAddingItems Then Exit Sub

            'm_bremovingitems is set in the lost focus event of the amount field,
            'if amount has been reduced then the listitems are unchecked
            If m_bRemovingItems Then Exit Sub

            lRow = Convert.ToInt16(Item.Tag)

            'flag the item in the array as checked
            If Item.Checked Then

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMTrue
                InstalmentArray(ACInstalmentPlanRef, lRow) = cboInstalment.Text
            Else

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMFalse
            End If

            If DisplaySelectedInstalmentTotals() = gPMConstants.PMEReturnCode.PMFalse Then
                Item.Checked = gPMConstants.PMEReturnCode.PMFalse

                InstalmentArray(ACInstalmentFlagElement, lRow) = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the lvInstalment_ItemCheck event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvInstalments_ItemCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cboInstalment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboInstalment.SelectedIndexChanged
        GetInstalmentDetails(cboInstalment.Text)
        DisplaySelectedInstalmentTotals()

    End Sub

    Private Sub lvwBGDetails_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwBGDetails.ItemChecked
        Dim Item As ListViewItem = lvwBGDetails.Items(e.Item.Index)
        If Item.Checked Then
            m_lReturn = ValidateBGItem(Item.Index)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If m_cBalAmtTobeAllocated > 0 Then
                    m_lReturn = CalculateAmtTobeAllocated(Item.Index, IIf(Item.Checked, True, False))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("CalculateAmtTobeAllocated", "CalculateAmtTobeAllocated Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = ReBuildPolicyBgArray(gPMFunctions.ToSafeDecimal(Item.SubItems.Item(kBankGuaranteeColHIndexPostedAmount).Text))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ReBuildPolicyBgArray", "ReBuildPolicyBgArray Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    cmdOK.Enabled = True
                Else
                    Item.Checked = False

                End If
            Else
                Item.Checked = False
            End If
        ElseIf Not Item.Checked Then
            m_lReturn = CalculateAmtTobeAllocated(Item.Index, IIf(Item.Checked, True, False))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CalculateAmtTobeAllocated", "CalculateAmtTobeAllocated Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'm_lReturn = ReBuildPolicyBgArray(CDec(Item.SubItems.Item(kBankGuaranteeColHIndexPostedAmount - 2).Text))
            m_lReturn = ReBuildPolicyBgArray(gPMFunctions.ToSafeDecimal(Item.SubItems.Item(kBankGuaranteeColHIndexPostedAmount).Text))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ReBuildPolicyBgArray", "ReBuildPolicyBgArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
        'If Item.Checked = False Then
        '    Item.Checked = True
        'ElseIf Item.Checked = True Then
        '    Item.Checked = False
        'End If
    End Sub

    Private Sub uctPartyBankCombo2_EditPartyBankItem(ByVal Sender As System.Object, ByVal e As uctPartyBank.uctPartyBankCombo.EditPartyBankItemEventArgs) Handles uctPartyBankCombo2.EditPartyBankItem
        m_lReturn = GetPartyBanks()
        FillPartyBankDetails()
    End Sub

    Private Sub chkIsLeadAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsLeadAccount.Click

        If IsSplitReceipt Then
            If chkIsLeadAccount.Checked = True Then
                txtSplitTotal.Enabled = True
            Else
                txtSplitTotal.Enabled = False
            End If
        End If

    End Sub


    ''' <summary>
    ''' ShowInsurerPayment
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ShowInsurerPayment() As Integer

        Dim sSQL As String
        Dim vResultArray(,) As Object
        Dim oInsurerPayment As Object
        Dim nBatchID As Integer

        Try
            Dim oKeyArray(1, 2) As Object
            m_lReturn = m_oBusiness.GetCashlistbatchid(CashlistID, nBatchID)

            ' Assign the key array with the parameter members.

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = kPMKeyNameAllocationViewMode
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = PMEComponentAction.PMView

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeyNameBatchID
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = nBatchID

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = kPMKeyAllocationCallingAppName
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = ACTInsurerPaymentRoadMap

            If (oInsurerPayment Is Nothing) Then
                m_lReturn = g_oObjectManager.GetInstance(oObject:=oInsurerPayment,
                                                         sClassName:="iACTInsurerPaymentSFU.Interface_Renamed",
                                                         vInstanceManager:=PMGetLocalInterface)
            End If
            m_lReturn = oInsurerPayment.SetKeys(vKeyArray:=oKeyArray)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                m_lReturn = PMEReturnCode.PMFalse
                oInsurerPayment = Nothing
                Return m_lReturn
            End If


            m_lReturn = oInsurerPayment.Start()
            oInsurerPayment.Dispose()
            oInsurerPayment = Nothing

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : ShowInsurerPayment", ex)
        End Try
    End Function

    ''' <summary>
    ''' AutomaticAllocate
    ''' </summary>
    ''' <param name="nRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AutomaticAllocate(ByVal nRow As Long) As Integer
        Dim oKeys(,) As Object
        Dim nAllocationID As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim oAllocationPost As Object
        Try

            If (BatchID <> 0) Then
                ReDim oKeys(0 To 1, 0 To 4)
            Else
                ReDim oKeys(0 To 1, 0 To 3)
            End If

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameAllocationId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = nAllocationID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameCashListItemId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = CashlistitemID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCashListId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 3) = CashlistID

            If (BatchID <> 0) Then
                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 4) = PMKeyNameBatchID
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 4) = BatchID
            End If

            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oInsurerPaymentAllocateBusiness,
                                                     sClassName:="bACTInsurerPaymentAllocate.Business",
                                                     vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to get an instance of bACTInsurerPaymentAllocate.Business")
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)

            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetKeys(vKeyArray:=oKeys)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to set navigator keys")
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.Start()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to start Insurer Payment Allocate Business.")
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.GetKeys(vKeyArray:=oKeys)

            nAllocationID = CLng(oKeys(1, 0))
            m_oInsurerPaymentAllocateBusiness.Dispose()


            m_oInsurerPaymentAllocateBusiness = Nothing
            m_oListData(ACSubRealAllocationID, 0) = gACTLibrary.ACTAllocationStatusAllocated

            m_lReturn = DataToBusiness(nMode:=PMEComponentAction.PMEdit, nRow:=0)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to update the business.")
            End If

            m_lReturn = m_oBusiness.Update()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to update the business.")
            End If

            If oAllocationPost Is Nothing Then
                m_lReturn = g_oObjectManager.GetInstance(oAllocationPost,
                        "bACTAllocationPost.Automated",
                        PMGetViaClientManager)

            End If
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("ApproveCashListItem failed to Create bACTAllocationPost.Automated object")
            End If

            Dim m_vAllocationIDs As Object
            Dim sTmp As String
            Dim oKeyArray(0 To 1, 0 To 4)

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameCashListItemId
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = CashlistitemID

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameAccountID
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) = AccountID

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameAllocationId
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = nAllocationID

            m_lReturn = gACTLibrary.ParseArray(vArray:=m_vAllocationIDs,
                                               sString:=sTmp,
                                               bArrayToString:=True)
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameAllocationIDs
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 3) = sTmp

            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 4) = ACTKeyNameDocumentRef
            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sDocumentRef

            m_lReturn = oAllocationPost.SetKeys(oKeyArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("bACTInsurerPaymentAllocate.SetKeys Failed.")
            End If

            m_lReturn = oAllocationPost.Start()
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("bACTInsurerPaymentAllocate.Start Failed.")
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : AutomaticAllocate", ex)
        Finally
            If (oAllocationPost Is Nothing) = False Then
                Call oAllocationPost.Dispose()
                oAllocationPost = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' Approves a CashListItem
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApproveCashListItem(ByRef r_bLastStep As Boolean) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCashListPost As bACTCashListPost.Automated
        Dim oStepAuthorization As bACTCashlistitem.StepAuthorization
        Dim sErrorMessage As String = ""
        Dim bLastStep As Boolean
        Dim oOptionValue As Object
        Dim sAccountShortCode As String
        Dim sUserGroupCode As String

        Try
            ' START CHANGES - Changed By: AAB  - Changed On: 04-Dec-2003 10:34            
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                      v_vBranch:=g_oObjectManager.SourceID,
                                                      r_vUnderwriting:=oOptionValue)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to process GetProductOptionValue.")
            End If

            If gPMFunctions.NullToString(oOptionValue) = "1" Then
                m_lReturn = g_oObjectManager.GetInstance(oStepAuthorization,
                                                         "bACTCashlistitem.StepAuthorization",
                                                         vInstanceManager:=PMGetViaClientManager)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to Create StepAuthorization Class")
                End If

                'set the properties of the object.

                oStepAuthorization.PaymentType = ACPaymentsType
                oStepAuthorization.PaymentID = CashlistitemID
                oStepAuthorization.PaymentAmount = Amount
                oStepAuthorization.PaymentCreatorUserID = PMUserid
                m_lReturn = oStepAuthorization.ProcessApproval()
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to ProcessApproval")
                End If

                sErrorMessage = oStepAuthorization.ProcessErrorMessage
                bLastStep = oStepAuthorization.LastStep

                If sErrorMessage <> "" Then
                    nResult = PMEReturnCode.PMError
                    MessageBox.Show(sErrorMessage, "Cash List Item", MessageBoxButtons.OK)
                    oStepAuthorization.Dispose()
                    oStepAuthorization = Nothing
                    Return nResult
                Else
                    MessageBox.Show("You successfully completed this authorization step.",
                                    "Authorization",
                                    MessageBoxButtons.OK)
                End If
                r_bLastStep = bLastStep
                If bLastStep Then
                    m_lReturn = m_oBusiness.UpdateTransMatchCashListItemID(nCashListTransDetailsID:=0,
                                                                           nCashListItemID:=CashlistitemID,
                                                                           sProcessType:="APPROVE")
                End If
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to UpdateTransMatchCashListItemID")
                End If

                'Delete all the existing recrods we are done
                m_lReturn = m_oBusiness.ProcessWTM(v_lCashlistItemID:=CashlistitemID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to CreateWorkTaskManagerForApprovalProcess")
                End If

                m_lReturn = m_oBusiness.GetUserGroupID(v_lUserID:=PMUserid,
                                                       r_lUserGroupID:=m_lPMUserGroupId)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to GetUserGroupID")
                End If


                m_lReturn = m_oBusiness.GetAccountAndUserGroupCode(v_lAccountId:=AccountID,
                                                                   v_lUserGroupID:=m_lPMUserGroupId,
                                                                   r_sAccountCode:=sAccountShortCode,
                                                                   r_sUsergroupCode:=sUserGroupCode)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to GetAccountAndUserGroupCode")
                End If

                If bLastStep Then
                     sUserGroupCode = ""
                    Dim temp_oCashListPost As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oCashListPost,
                                                             "bACTCashListPost.Automated",
                                                             PMGetViaClientManager)
                    oCashListPost = temp_oCashListPost
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("ApproveCashListItem failed to Create bACTCashListPost.Automated object")
                    End If

                    'Change the status to Issued
                    m_lReturn = SelectComboItem(r_ctl:=cboPaymentStatus, v_varID:=ACPaymentStatusIssued)
                    m_lReturn = m_oFormFields.FormatControl(cboPaymentStatus, ACPaymentStatusIssued)
                    'set to PMEdit to ensure the data is saved in the database for posting and allocation
                    m_iTask = PMEComponentAction.PMEdit
                    ProcessCommit()
                Else
                    sUserGroupCode = ""
                    m_lReturn = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("ApproveCashListItem failed to GetStepGroupCode")
                    End If
                End If

                m_lReturn = CreateWTMForApproval(v_sUserGroupCode:=sUserGroupCode,
                                                 v_sAccountShortCode:=sAccountShortCode,
                                                 v_bLastStep:=bLastStep)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to CreateWorkTaskManagerForApprovalProcess")
                End If

                If Not (oStepAuthorization Is Nothing) Then
                    oStepAuthorization.Dispose()
                    oStepAuthorization = Nothing
                End If
            Else
                Dim temp_oCashListPost2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oCashListPost2,
                                                         "bACTCashListPost.Automated",
                                                         PMGetViaClientManager)
                oCashListPost = temp_oCashListPost2
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to Create bACTCashListPost.Automated object")
                End If

                'Change the status to Issued
                SelectComboItem(cboPaymentStatus, ACPaymentStatusIssued)
                ProcessCommit()
                m_lReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=CashlistID,
                                                              v_vCashListItemID:=CashlistitemID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("ApproveCashListItem failed to PostUnallocatedCash")
                End If
            End If

            If Not (oCashListPost Is Nothing) Then
                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If

            If Not (oStepAuthorization Is Nothing) Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If

            Return nResult
        Catch excep As System.Exception
            If Not (oCashListPost Is Nothing) Then
                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If
            If Not (oStepAuthorization Is Nothing) Then
                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If
            Throw New Exception("Method : ApproveCashListItem", excep)

        End Try
    End Function

    ''' <summary>
    ''' InsurerPaymentAutomaticAllocate
    ''' </summary>
    ''' <param name="nRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsurerPaymentAutomaticAllocate(ByVal nRow As Long) As Integer

        Dim oKeys(,) As Object
        Dim nAllocationID As Long

        Try
            If (BatchID <> 0) Then
                ReDim oKeys(0 To 1, 0 To 4)
            Else
                ReDim oKeys(0 To 1, 0 To 3)
            End If

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameAllocationId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 1) = nAllocationID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameCashListItemId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 2) = CashlistitemID

            oKeys(PMENavLetGetKeyColPosition.PMKeyName, 3) = ACTKeyNameCashListId
            oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 3) = CashlistID

            If (BatchID <> 0) Then
                oKeys(PMENavLetGetKeyColPosition.PMKeyName, 4) = PMKeyNameBatchID
                oKeys(PMENavLetGetKeyColPosition.PMKeyValue, 4) = BatchID
            End If

            ' Create a new instance of the Insurer Payment Allocation
            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oInsurerPaymentAllocateBusiness,
                                                     sClassName:="bACTInsurerPaymentAllocate.Business",
                                                     vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to initialise bACTInsurerPaymentAllocate.Business.")
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)

            m_lReturn = m_oInsurerPaymentAllocateBusiness.SetKeys(vKeyArray:=oKeys)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to set navigator keys.")
            End If

            m_lReturn = m_oInsurerPaymentAllocateBusiness.Start()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to start Insurer Payment Allocate Business.")
            End If
            m_lReturn = m_oInsurerPaymentAllocateBusiness.GetKeys(vKeyArray:=oKeys)

            nAllocationID = CLng(oKeys(1, 0))
            m_oInsurerPaymentAllocateBusiness.Dispose()


            m_oInsurerPaymentAllocateBusiness = Nothing
            m_lReturn = m_oBusiness.Update()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to update the business.")
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : InsurerPaymentAutomaticAllocate", ex)
        End Try

    End Function

    ''' <summary>
    ''' AutoAllocateClick
    ''' </summary>
    ''' <param name="nAccountID"></param>
    ''' <param name="nCashlistitemID"></param>
    ''' <param name="nTaxTransDetailID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoAllocateClick(ByVal nAccountID As Integer,
                                      ByVal nCashlistitemID As Integer,
                                      ByVal nTaxTransDetailID As Integer) As Integer

        Dim nRow As Integer
        Dim nTransDetailId As Integer
        Dim nPaymentTypeID As Integer
        Dim sPurchInvNo As String
        Dim oDocument As Object
        Dim nSelectedRow As Integer

        Try
            nSelectedRow = 0
            m_lReturn = GetBusiness()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to GetBusiness.")
            End If

            ' Re-Populate the data
            m_lReturn = BusinessToData()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to perform BusinessToData.")
            End If

            If m_oListData(ACSubRealAllocationID, 0) = gACTLibrary.ACTAllocationStatusAllocated Then
                MsgBox("This item has already been allocated. Please re-select.", vbExclamation, "Allocation already done")
                Exit Function
            End If

            m_lReturn = m_oBusiness.GetandUpdateBatchTransDetailID(BatchID, CLng(m_oListData(ACSubTransdetailId, 0)))
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to perform GetandUpdateBatchTransDetailID.")
            End If

            If ToSafeLong(nTaxTransDetailID, 0) <> 0 Then
                m_lReturn = m_oBusiness.GetandUpdateBatchTransDetailID(BatchID, nTaxTransDetailID)
                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    Throw New Exception("Failed to perform GetandUpdateBatchTransDetailID.")
                End If
            End If

            m_lReturn = UpdateWriteOffDocumentRef()
            If m_lReturn = PMEReturnCode.PMFalse Then
                Throw New Exception("Failed to perform UpdateWriteOffDocumentRef.")
            End If

            m_lReturn = AutomaticAllocate(0)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to allocate cash list item.")
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : AutoAllocateClick", ex)
        End Try
    End Function

    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBusiness() As Long

        Try
            ' Pass the Cash List ID to be used in selection
            ' as we only want the Items belonging to a given list
            m_lReturn = m_oBusiness.GetDetails(vCashListID:=CashlistID)

            If (m_lReturn = PMEReturnCode.PMNotFound) Then
                Return PMEReturnCode.PMNotFound
            End If

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to get details from the business object")
            End If

            Return m_lReturn
        Catch ex As Exception
            Throw New Exception("Method : GetBusiness", ex)
        End Try

    End Function

    ''' <summary>
    ''' BusinessToData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BusinessToData() As Integer
        Dim oCashListItem As Object
        Try
            If Not m_oListData Is Nothing Then
                ReDim Preserve m_oListData(ACLastItem, 0)
            Else
                ReDim m_oListData(ACLastItem, 0)
            End If

            Dim nCount As Integer
            Do While m_oBusiness.GetNext(oCashListItem) = PMEReturnCode.PMTrue

                nCount = UBound(m_oListData, 2)

                m_oListData(ACSubCashListItemID, nCount) = oCashListItem(eCashListItem.CashlistitemID)
                m_oListData(ACSubAllocationstatusID, nCount) = oCashListItem(eCashListItem.AllocationstatusID)
                m_oListData(ACSubMediatypeID, nCount) = oCashListItem(eCashListItem.MediaTypeID)
                m_oListData(ACSubCashListID, nCount) = oCashListItem(eCashListItem.CashlistID)
                m_oListData(ACSubAccountID, nCount) = oCashListItem(eCashListItem.AccountID)
                m_oListData(ACSubMediaRef, nCount) = oCashListItem(eCashListItem.MediaRef)
                m_oListData(ACSubOurRef, nCount) = oCashListItem(eCashListItem.OurRef)
                m_oListData(ACSubTheirRef, nCount) = oCashListItem(eCashListItem.TheirRef)
                m_oListData(ACSubAmount, nCount) = oCashListItem(eCashListItem.Amount)
                m_oListData(ACSubContactName, nCount) = oCashListItem(eCashListItem.ContactName)
                m_oListData(ACSubAddress1, nCount) = oCashListItem(eCashListItem.Address1)
                m_oListData(ACSubAddress2, nCount) = oCashListItem(eCashListItem.Address2)
                m_oListData(ACSubAddress3, nCount) = oCashListItem(eCashListItem.Address3)
                m_oListData(ACSubAddress4, nCount) = oCashListItem(eCashListItem.Address4)
                m_oListData(ACSubPostalCode, nCount) = oCashListItem(eCashListItem.PostalCode)
                m_oListData(ACSubAddressCountry, nCount) = oCashListItem(eCashListItem.AddressCountry)
                m_oListData(ACSubPaymentName, nCount) = oCashListItem(eCashListItem.PaymentName)
                m_oListData(ACSubPaymentAccountCode, nCount) = oCashListItem(eCashListItem.PaymentAccountCode)
                m_oListData(ACSubPaymentBranchCode, nCount) = oCashListItem(eCashListItem.PaymentBranchCode)
                m_oListData(ACSubPaymentExpiryDate, nCount) = oCashListItem(eCashListItem.PaymentExpiryDate)
                m_oListData(ACSubPaymentReference1, nCount) = oCashListItem(eCashListItem.PaymentReference1)
                m_oListData(ACSubPaymentReference2, nCount) = oCashListItem(eCashListItem.PaymentReference2)
                m_oListData(ACSubLetter, nCount) = oCashListItem(eCashListItem.Letter)
                m_oListData(ACSubTransdetailId, nCount) = oCashListItem(eCashListItem.TransdetailID)
                If m_bIsInsurerePaymentRoadMap = True And LCase(cmdOK.Text) = "approve" Then
                    m_oListData(kPMNavBatchKey, nCount) = oCashListItem(eCashListItem.Batch_id)
                Else
                    m_oListData(ACBatchid, nCount) = oCashListItem(eCashListItem.Batch_id)
                End If

                m_oListData(ACPMUserid, nCount) = oCashListItem(eCashListItem.pmuser_id)
                m_oListData(ACTransactiondate, nCount) = oCashListItem(eCashListItem.Transaction_Date)
                m_oListData(ACOriginalamount, nCount) = oCashListItem(eCashListItem.Original_Amount)
                m_oListData(ACAmounttendered, nCount) = oCashListItem(eCashListItem.Amount_Tendered)
                m_oListData(ACChange, nCount) = oCashListItem(eCashListItem.Change)
                m_oListData(ACvCashlistitemreceipttypeid, nCount) = oCashListItem(eCashListItem.CashListItem_receipt_type_id)
                m_oListData(ACCashlistitemreceiptstatusid, nCount) = oCashListItem(eCashListItem.CashListItem_receipt_status_id)
                m_oListData(ACCashlistitembankid, nCount) = oCashListItem(eCashListItem.CashListItem_bank_id)
                m_oListData(ACChequedate, nCount) = oCashListItem(eCashListItem.Cheque_Date)
                m_oListData(ACCCnumber, nCount) = oCashListItem(eCashListItem.CC_Number)
                m_oListData(ACCCexpirydate, nCount) = oCashListItem(eCashListItem.CC_Expiry_Date)
                m_oListData(ACCCstartdate, nCount) = oCashListItem(eCashListItem.CC_Start_Date)
                m_oListData(ACCCissue, nCount) = oCashListItem(eCashListItem.CC_Issue)
                m_oListData(ACCCpin, nCount) = oCashListItem(eCashListItem.CC_Pin)
                m_oListData(ACCCauthcode, nCount) = oCashListItem(eCashListItem.CC_Auth_Code)
                m_oListData(ACCCname, nCount) = oCashListItem(eCashListItem.CC_Name)
                m_oListData(ACCCcustomer, nCount) = oCashListItem(eCashListItem.CC_Customer)
                m_oListData(ACCCmanualauthcode, nCount) = oCashListItem(eCashListItem.CC_Manual_Auth_Code)
                m_oListData(ACCCtransactioncode, nCount) = oCashListItem(eCashListItem.CC_Transaction_Code)
                m_oListData(ACMediatype_IssuerID, nCount) = oCashListItem(eCashListItem.MediaTypeIssuerID)
                m_oListData(ACReceiptdetails, nCount) = oCashListItem(eCashListItem.Receipt_Details)
                m_oListData(ACCashlistitemreversepmuserid, nCount) = oCashListItem(eCashListItem.CashListItem_Reverse_PMUser_id)
                m_oListData(ACCashlistitemreversereasonid, nCount) = oCashListItem(eCashListItem.CashListItem_Reverse_Reason_id)
                m_oListData(ACCashListItemPaymentTypeID, nCount) = oCashListItem(eCashListItem.CashListItem_Payment_Type_id)
                m_oListData(ACCashListItemPaymentStatusID, nCount) = oCashListItem(eCashListItem.CashListItem_Payment_Status_id)
                m_oListData(ACDatepresented, nCount) = oCashListItem(eCashListItem.Date_Presented)
                m_oListData(ACChequeinpossession, nCount) = oCashListItem(eCashListItem.Cheque_in_Possession)
                m_oListData(ACStoprequesteddate, nCount) = oCashListItem(eCashListItem.Stop_Requested_Date)
                m_oListData(ACStopprinteddate, nCount) = oCashListItem(eCashListItem.Stop_Printed_Date)
                m_oListData(ACStopconfirmationdate, nCount) = oCashListItem(eCashListItem.Stop_Confirmation_Date)
                m_oListData(ACReason, nCount) = oCashListItem(eCashListItem.Reason)
                m_oListData(ACReplacescashlistitemid, nCount) = oCashListItem(eCashListItem.Replaces_CashListItem_id)
                m_oListData(ACCurrencyBaseDate, nCount) = oCashListItem(eCashListItem.CurrencyBaseDate)
                m_oListData(ACCurrencyBaseXrate, nCount) = oCashListItem(eCashListItem.CurrencyBaseXrate)
                m_oListData(ACAccountBaseDate, nCount) = oCashListItem(eCashListItem.AccountBaseDate)
                m_oListData(ACAccountBaseXrate, nCount) = oCashListItem(eCashListItem.AccountBaseXrate)
                m_oListData(ACSystemBaseDate, nCount) = oCashListItem(eCashListItem.SystemBaseDate)
                m_oListData(ACSystemBaseXrate, nCount) = oCashListItem(eCashListItem.SystemBaseXrate)
                m_oListData(ACOverrideReason, nCount) = oCashListItem(eCashListItem.OverrideReason)
                m_oListData(ACBankPaymentTypeId, nCount) = oCashListItem(eCashListItem.PartyBankId)
                m_oListData(ACCollectionDate, nCount) = oCashListItem(eCashListItem.CollectionDate)
                m_oListData(ACComments, nCount) = oCashListItem(eCashListItem.Comments)

                If CLng(oCashListItem(eCashListItem.AllocationstatusID)) = gACTLibrary.ACTAllocationStatusUnallocated Then
                    m_bAllocated = False
                ElseIf CLng(oCashListItem(eCashListItem.AllocationstatusID)) = gACTLibrary.ACTAllocationStatusPosted Then
                    m_bAllocated = True
                End If

                m_oListData(ACBankLocation, nCount) = oCashListItem(eCashListItem.BankLocation)
                m_oListData(ACBankBranch, nCount) = oCashListItem(eCashListItem.BankBranch)
                m_oListData(ACChequeTypeId, nCount) = oCashListItem(eCashListItem.ChequeTypeId)
                m_oListData(ACCCBankId, nCount) = oCashListItem(eCashListItem.CCBankId)
                m_oListData(ACCardTypeId, nCount) = oCashListItem(eCashListItem.CardTypeId)
                m_oListData(ACCardTransSlipNo, nCount) = oCashListItem(eCashListItem.CardTransSlipNo)
                m_oListData(ACChequeClearingTypeId, nCount) = oCashListItem(eCashListItem.ChequeClearingTypeId)
                m_oListData(kTaxBandID, nCount) = oCashListItem(eCashListItem.TaxBandId)
                m_oListData(ACTaxAmount, nCount) = oCashListItem(eCashListItem.TaxAmount)
                m_oListData(kBIC, nCount) = oCashListItem(eCashListItem.BIC)
                m_oListData(kIBAN, nCount) = oCashListItem(eCashListItem.IBAN)

                m_oListData(UBound(m_oListData, 1), nCount) = nCount + 1
                ReDim Preserve m_oListData(UBound(m_oListData, 1), nCount + 1)

            Loop

            If (IsArray(m_oListData) = True) Then
                If (nCount > 0) Then
                    ReDim Preserve m_oListData(UBound(m_oListData, 1), nCount - 1)
                End If
            End If
            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method :  BusinessToData " + " , Failed to assign the data storag", ex)
        End Try
    End Function

    ''' <summary>
    ''' DataToBusiness
    ''' </summary>
    ''' <param name="nMode"></param>
    ''' <param name="nRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToBusiness(nMode As Integer,
                                    nRow As Integer) As Integer

        Dim nBusinessDataID As Integer
        Dim oCashListItem As Object

        Try
            ReDim oCashListItem(eCashListItem.LastItem)
            oCashListItem(eCashListItem.CashlistitemID) = m_oListData(ACSubCashListItemID, nRow)
            oCashListItem(eCashListItem.AllocationstatusID) = m_oListData(ACSubRealAllocationID, nRow)
            oCashListItem(eCashListItem.MediaTypeID) = m_oListData(ACSubMediatypeID, nRow)
            oCashListItem(eCashListItem.CashlistID) = m_oListData(ACSubCashListID, nRow)
            oCashListItem(eCashListItem.AccountID) = m_oListData(ACSubAccountID, nRow)
            oCashListItem(eCashListItem.MediaRef) = m_oListData(ACSubMediaRef, nRow)
            oCashListItem(eCashListItem.OurRef) = m_oListData(ACSubOurRef, nRow)
            oCashListItem(eCashListItem.TheirRef) = m_oListData(ACSubTheirRef, nRow)
            oCashListItem(eCashListItem.Amount) = m_oListData(ACSubAmount, nRow)
            oCashListItem(eCashListItem.ContactName) = m_oListData(ACSubContactName, nRow)
            oCashListItem(eCashListItem.Address1) = m_oListData(ACSubAddress1, nRow)
            oCashListItem(eCashListItem.Address2) = m_oListData(ACSubAddress2, nRow)
            oCashListItem(eCashListItem.Address3) = m_oListData(ACSubAddress3, nRow)
            oCashListItem(eCashListItem.Address4) = m_oListData(ACSubAddress4, nRow)
            oCashListItem(eCashListItem.PostalCode) = m_oListData(ACSubPostalCode, nRow)
            oCashListItem(eCashListItem.AddressCountry) = m_oListData(ACSubAddressCountry, nRow)
            oCashListItem(eCashListItem.PaymentName) = m_oListData(ACSubPaymentName, nRow)
            oCashListItem(eCashListItem.PaymentAccountCode) = m_oListData(ACSubPaymentAccountCode, nRow)
            oCashListItem(eCashListItem.PaymentBranchCode) = m_oListData(ACSubPaymentBranchCode, nRow)
            oCashListItem(eCashListItem.PaymentExpiryDate) = m_oListData(ACSubPaymentExpiryDate, nRow)
            oCashListItem(eCashListItem.PaymentReference1) = m_oListData(ACSubPaymentReference1, nRow)
            oCashListItem(eCashListItem.PaymentReference2) = m_oListData(ACSubPaymentReference2, nRow)
            oCashListItem(eCashListItem.Letter) = m_oListData(ACSubLetter, nRow)
            If m_oListData(ACBatchid, nRow) <> 0 OrElse LCase(cmdOK.Text) = "approve" Then
                oCashListItem(eCashListItem.Batch_id) = m_oListData(ACBatchid, nRow)
            Else
                oCashListItem(eCashListItem.Batch_id) = BatchID
            End If
            oCashListItem(eCashListItem.pmuser_id) = m_oListData(ACPMUserid, nRow)
            oCashListItem(eCashListItem.Transaction_Date) = m_oListData(ACTransactiondate, nRow)
            oCashListItem(eCashListItem.Original_Amount) = m_oListData(ACOriginalamount, nRow)
            oCashListItem(eCashListItem.Amount_Tendered) = m_oListData(ACAmounttendered, nRow)
            oCashListItem(eCashListItem.Change) = m_oListData(ACChange, nRow)
            oCashListItem(eCashListItem.CashListItem_receipt_type_id) = m_oListData(ACvCashlistitemreceipttypeid, nRow)
            oCashListItem(eCashListItem.CashListItem_receipt_status_id) = m_oListData(ACCashlistitemreceiptstatusid, nRow)
            oCashListItem(eCashListItem.CashListItem_bank_id) = m_oListData(ACCashlistitembankid, nRow)
            oCashListItem(eCashListItem.Cheque_Date) = m_oListData(ACChequedate, nRow)
            oCashListItem(eCashListItem.CC_Number) = m_oListData(ACCCnumber, nRow)
            oCashListItem(eCashListItem.CC_Expiry_Date) = m_oListData(ACCCexpirydate, nRow)
            oCashListItem(eCashListItem.CC_Start_Date) = m_oListData(ACCCstartdate, nRow)
            oCashListItem(eCashListItem.CC_Issue) = m_oListData(ACCCissue, nRow)
            oCashListItem(eCashListItem.CC_Pin) = m_oListData(ACCCpin, nRow)
            oCashListItem(eCashListItem.CC_Auth_Code) = m_oListData(ACCCauthcode, nRow)
            oCashListItem(eCashListItem.CC_Name) = m_oListData(ACCCname, nRow)
            oCashListItem(eCashListItem.CC_Customer) = m_oListData(ACCCcustomer, nRow)
            oCashListItem(eCashListItem.CC_Manual_Auth_Code) = m_oListData(ACCCmanualauthcode, nRow)
            oCashListItem(eCashListItem.CC_Transaction_Code) = m_oListData(ACCCtransactioncode, nRow)
            oCashListItem(eCashListItem.MediaTypeIssuerID) = m_oListData(ACMediatype_IssuerID, nRow)
            oCashListItem(eCashListItem.Receipt_Details) = m_oListData(ACReceiptdetails, nRow)
            oCashListItem(eCashListItem.CashListItem_Reverse_PMUser_id) = m_oListData(ACCashlistitemreversepmuserid, nRow)
            oCashListItem(eCashListItem.CashListItem_Reverse_Reason_id) = m_oListData(ACCashlistitemreversereasonid, nRow)
            oCashListItem(eCashListItem.CashListItem_Payment_Type_id) = m_oListData(ACCashListItemPaymentTypeID, nRow)
            oCashListItem(eCashListItem.CashListItem_Payment_Status_id) = m_oListData(ACCashListItemPaymentStatusID, nRow)
            oCashListItem(eCashListItem.Date_Presented) = m_oListData(ACDatepresented, nRow)
            oCashListItem(eCashListItem.Cheque_in_Possession) = m_oListData(ACChequeinpossession, nRow)
            oCashListItem(eCashListItem.Stop_Requested_Date) = m_oListData(ACStoprequesteddate, nRow)
            oCashListItem(eCashListItem.Stop_Printed_Date) = m_oListData(ACStopprinteddate, nRow)
            oCashListItem(eCashListItem.Stop_Confirmation_Date) = m_oListData(ACStopconfirmationdate, nRow)
            oCashListItem(eCashListItem.Reason) = m_oListData(ACReason, nRow)
            oCashListItem(eCashListItem.Replaces_CashListItem_id) = m_oListData(ACReplacescashlistitemid, nRow)
            oCashListItem(eCashListItem.InstalmentArray) = m_oListData(ACInstalmentArray, nRow)
            oCashListItem(eCashListItem.SalvageArray) = m_oListData(ACSalvageArray, nRow)
            oCashListItem(eCashListItem.CLMUSRecoveryArray) = m_oListData(ACCLMUSRecoveryArray, nRow)
            oCashListItem(eCashListItem.CLMRVRecoveryArray) = m_oListData(ACCLMRVRecoveryArray, nRow)
            oCashListItem(eCashListItem.UnderwritingYearID) = m_oListData(ACUnderwritingYearID, nRow)
            oCashListItem(eCashListItem.CurrencyBaseDate) = m_oListData(ACCurrencyBaseDate, nRow)
            oCashListItem(eCashListItem.CurrencyBaseXrate) = m_oListData(ACCurrencyBaseXrate, nRow)
            oCashListItem(eCashListItem.AccountBaseDate) = m_oListData(ACAccountBaseDate, nRow)
            oCashListItem(eCashListItem.AccountBaseXrate) = m_oListData(ACAccountBaseXrate, nRow)
            oCashListItem(eCashListItem.SystemBaseDate) = m_oListData(ACSystemBaseDate, nRow)
            oCashListItem(eCashListItem.SystemBaseXrate) = m_oListData(ACSystemBaseXrate, nRow)
            oCashListItem(eCashListItem.OverrideReason) = m_oListData(ACOverrideReason, nRow)

            'Party Bank Details
            oCashListItem(eCashListItem.PartyBankId) = m_oListData(ACBankPaymentTypeId, nRow)
            oCashListItem(eCashListItem.CollectionDate) = m_oListData(ACCollectionDate, nRow)
            oCashListItem(eCashListItem.Comments) = m_oListData(ACComments, nRow)
            oCashListItem(eCashListItem.BGPolicies) = m_oListData(ACBGPolicies, nRow)

            'WPR12- Enhancement Quote Collection Process
            oCashListItem(eCashListItem.BankLocation) = m_oListData(ACBankLocation, nRow)
            oCashListItem(eCashListItem.BankBranch) = m_oListData(ACBankBranch, nRow)
            oCashListItem(eCashListItem.ChequeTypeId) = m_oListData(ACChequeTypeId, nRow)
            oCashListItem(eCashListItem.CCBankId) = m_oListData(ACCCBankId, nRow)
            oCashListItem(eCashListItem.CardTypeId) = m_oListData(ACCardTypeId, nRow)
            oCashListItem(eCashListItem.CardTransSlipNo) = m_oListData(ACCardTransSlipNo, nRow)
            oCashListItem(eCashListItem.ChequeClearingTypeId) = m_oListData(ACChequeClearingTypeId, nRow)

            ' E001
            oCashListItem(eCashListItem.TaxBandId) = m_oListData(kTaxBandID, nRow)
            oCashListItem(eCashListItem.TaxAmount) = m_oListData(ACTaxAmount, nRow)
            oCashListItem(eCashListItem.BIC) = m_oListData(kBIC, nRow)
            oCashListItem(eCashListItem.IBAN) = m_oListData(kIBAN, nRow)

            ' Store unique key for this row.
            nBusinessDataID = m_oListData(UBound(m_oListData, 1), nRow)

            Select Case (nMode)
                Case PMEComponentAction.PMAdd
                    m_lReturn = m_oBusiness.EditAdd(lRow:=nBusinessDataID,
                                                    r_vCashListItem:=oCashListItem)
                Case PMEComponentAction.PMEdit, PMEComponentAction.PMReverse
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=nBusinessDataID,
                                                       v_vCashListItem:=oCashListItem)
                Case PMEComponentAction.PMDelete
                    m_lReturn = m_oBusiness.EditDelete(lRow:=nBusinessDataID)
            End Select

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New Exception("Failed to assign the data details to business object")
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : DataToBusiness ", ex)
        End Try

    End Function

    ''' <summary>
    ''' GetReinsurerAndRIPaymentRecoveriesDetail
    ''' </summary>
    ''' <param name="nAccountID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetReinsurerAndRIPaymentRecoveriesDetail(ByVal nAccountID As Integer) As Integer

        Try
            If nAccountID > 0 Then
                m_lReturn = m_oBusiness.GetReinsurerAndRIPaymentRecoveriesDetail(nAccountId:=nAccountID,
                                                                                 r_bIsReinsurer:=m_bIsReinsurer,
                                                                                 r_bIsRIPaymentsAndRecovery:=m_bIsRIPaymentAndRecoveries)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("m_oBusiness.GetReinsurerAndRIPaymentRecoveriesDetail Failed")
                End If
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw New Exception("Method : GetReinsurerAndRIPaymentRecoveriesDetail", ex)
        End Try
    End Function

    ''' <summary>
    ''' UpdateWriteOffDocumentRef
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateWriteOffDocumentRef() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Try
            Dim oaResultArray(,) As Object
            Dim sDocumentRef As String = ""
            Dim nNumberRangeID As Integer
            Dim nNumber As Integer
            Dim sRangeCode As String = ""
            Dim nDocumentType As Integer
            Dim nDocumentID As Integer
            Dim sGroupCode As String = ""
            Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign

            m_lReturn = m_oBusiness.GetTransDetailsFromBatch(v_lBatchID:=BatchID, r_vResultArray:=oaResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("GetTransactionsFromBatch Failed.")
            End If

            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oPMAutoNumber,
                                                      sClassName:="bACTAutoNumber.Business",
                                                      vInstanceManager:=PMGetViaClientManager)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("GetInstance bACTAutoNumber.Business Failed.")
            End If

            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oDocumentPost,
                                                      sClassName:="bACTDocumentPost.Form",
                                                      vInstanceManager:=PMGetViaClientManager)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("GetInstance bACTDocumentPost.Form Failed.")
            End If

            If Information.IsArray(oaResultArray) Then
                For nCount As Integer = 0 To oaResultArray.GetUpperBound(1)
                    ' Create the autonumber object using component services

                    nDocumentType = gACTLibrary.ACTDocTypeWriteOff
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14

                    ' Get the number range
                    m_lReturn = m_oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode,
                                                               v_sRangeCode:=sRangeCode,
                                                               r_lNumberRangeID:=nNumberRangeID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("GetNumberRange Failed.")
                    End If

                    ''Generate the next number

                    m_lReturn = m_oPMAutoNumber.GenerateNumber(v_lNumberRangeID:=nNumberRangeID,
                                                               v_iUserID:=g_iUserID,
                                                               v_iCompanyID:=g_iSourceID,
                                                               r_lNumber:=nNumber)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("GenerateNumber Failed.")
                    End If

                    ' Format the number
                    sDocumentRef = StringsHelper.Format(nNumber, "00000000")
                    sDocumentRef = sRangeCode & sDocumentRef
                    m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=nDocumentType,
                                                            v_sDocumentRef:=sDocumentRef,
                                                            v_dtDocumentDate:=DateTime.Now,
                                                            v_sComment:="WRITEOFF",
                                                            r_vDocumentID:=nDocumentID,
                                                            r_vDocSourceID:=g_iSourceID)

                    m_lReturn = m_oBusiness.UpdateWriteOffDocumentRef(v_lOldDocumentId:=oaResultArray(1, nCount),
                                                                      v_lNewDocumentId:=nDocumentID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("UpdateWriteOffDocumentRef Failed.")
                    End If
                Next
            End If

            Return result
        Catch ex As Exception
            Throw New Exception("Method : UpdateWriteOffDocumentRef ", ex)
        Finally
            m_oDocumentPost.Dispose()
            m_oDocumentPost = Nothing
            m_oPMAutoNumber.Dispose()
            m_oPMAutoNumber = Nothing
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: WriteOffLimit
    '
    ' Description: User WriteOffLimit
    '
    ' WPR05 Development
    '
    ' ***************************************************************** '
    Private Function GetUserWriteOffLimit(ByVal dDiffAmt As Decimal, ByVal nBaseCurrencyID As Decimal) As Integer
        Dim sMsg As String
        Dim bWriteOffValid As Boolean
        Dim sCurrency As String
        Const kMethodName As String = "GetUserWriteOffLimit"
        Dim dAuthorityAmount As Decimal
        Dim UserAuthoritiesArray As Object

        Try

            CreateUserAuthorities()
            m_lReturn = m_oUserAuthorities.GetUserAuthoritiesDetails(g_iUserID, r_vResults:=UserAuthoritiesArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetUserWriteOffLimit", "Fail to get writeoff limit for the user", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If ToSafeBoolean(UserAuthoritiesArray(1, 0)) = False Then
                MessageBox.Show("You do not have write off authority.", "User Authority Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = m_oUserAuthorities.ValidateAmounts(v_iCurrencyID:=nBaseCurrencyID, v_cAmount:=dDiffAmt, v_lCompanyID:=g_iSourceID, r_vWriteOffValid:=bWriteOffValid, r_cAuthorityAmount:=dAuthorityAmount, r_sCurrency:=sCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetUserWriteOffLimit", "Fail to get writeoff limit for the user", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            If Not bWriteOffValid Then
                MessageBox.Show("Your write off limit does not allow you to write off the difference." _
                                & Environment.NewLine & Environment.NewLine _
                                & "Difference: " & String.Format("{0:0.00}", dDiffAmt) &
                                Environment.NewLine &
                                "Your write off limit: " & String.Format("{0:0.00}", dAuthorityAmount) & " (" & sCurrency.Trim & ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get User Write-Off Limit", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Private Sub GetPartyPolicies()
        Dim vPolicyArray(,) As Object
        Try
            If uctAccountLookup.AccountId = 0 Then
                cboInsuranceRef.Items.Clear()
                Exit Sub
            End If
            cboInsuranceRef.Text = ""
            m_lReturn = m_oBusiness.GetPartyPolicies(uctAccountLookup.AccountId, r_vPolicyArray:=vPolicyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cboInsuranceRef.Items.Clear()
                MessageBox.Show("GetPartyPolicies failed: " & gPMFunctions.ToSafeString(m_lReturn), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            cboInsuranceRef.Items.Clear()
            If Information.IsArray(vPolicyArray) Then
                For lCount As Integer = 0 To UBound(vPolicyArray, 2)
                    cboInsuranceRef.Items.Add(vPolicyArray(0, lCount))
                Next
            End If

        Catch excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Exit Sub
        End Try

    End Sub
End Class
