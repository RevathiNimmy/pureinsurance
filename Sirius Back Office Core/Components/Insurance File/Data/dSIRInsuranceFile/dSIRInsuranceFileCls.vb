Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRInsuranceFile_NET.SIRInsuranceFile")>
Public NotInheritable Class SIRInsuranceFile

    Implements IDisposable
#Region "Private Constants"
    Private Const ACClass As String = "SIRInsuranceFile"
#End Region


#Region "Private Variables"
    Private m_sUsername As String = ""
    ' Password.
    Private m_sPassword As String = ""
    ' User ID
    Private m_iUserID As Integer
    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    Private m_ibaseCurrencyID As Integer
    Private m_iCountryId As Integer 'Datasure
    ' LogLevel
    Private m_iLogLevel As Integer

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFileStructureID As Integer
    Private m_lInsuranceFileTypeID As Integer
    'developer guide no.101
    Private m_vInsuranceFileStatusID As Object
    Private m_lInsuranceFileID As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_sInsuranceRef As New StringsHelper.FixedLengthString(30)
    Private m_lProductID As Integer
    Private m_vLeadInsurerCnt As Object
    Private m_vLeadAgentCnt As Object
    Private m_vLeadAgentPercent As Object
    Private m_vAccountHandlerCnt As Object
    ' SJP (CMG) 04042003 PS235
    Private m_vAccountExecutiveCnt As Object
    Private m_lInsuredCnt As Integer
    Private m_iBusinessTypeID As Integer
    'developer guide no.101
    Private m_vCollectTypeID As Object
    Private m_vCollectionFromCnt As Object
    'sj 19/07/2002 - start
    'Private m_iBranchID As Integer
    Private m_lSubBranchId As Integer
    'sj 19/07/2002 - end
    Private m_vDateIssued As Object
    Private m_dtCoverStartDate As Date
    Private m_dtExpiryDate As Date
    Private m_dtRenewalDate As Date
    'developer guide no.101
    Private m_vRenewalMethodID As Object
    Private m_iRenewalFrequencyID As Integer
    Private m_iIsReferredAtRenewal As Integer
    'developer guide no.101
    Private m_vLapsedReasonID As Object
    Private m_vLapsedDate As Object
    Private m_vLapsedDescription As Object
    Private m_iIsReferredOnMta As Integer
    Private m_iPolicyVersion As Integer
    Private m_vGeminiPolicyStatus As Object
    Private m_vGeminiBusinessType As Object
    Private m_vDeferredInd As Object
    Private m_vPolicyIgnore As Object
    Private m_vBrokerCnt As Object
    Private m_vRiskCodeID As Object
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
    'ECK 14/7/99
    Private m_vCommissionPercentage As Object
    'DC 14/07/00
    Private m_vInvariantKey As Object
    'TN20000817 - Doc Ref 10 (Start)
    Private m_vInsuredName As Object
    Private m_vAlternateReference As Object
    Private m_vIsClientInvoiced As Object
    Private m_vOldPolicyNumber As Object
    Private m_vQuoteExpiryDate As Object
    Private m_vAlternateAccountCnt As Object
    'TN20000817 - Doc Ref 10 (End)
    Private m_vTransDescription As String = ""
    Private m_vAnniversaryDate As Object
    Private m_vPolicyStyleID As Object
    Private m_vUnderwritingYearID As Object
    Private m_vPolicyStatusID As Object
    Private m_vInceptionTPI As Object
    'FSA Phase III
    Private m_vFSACustomerCategoryID As Object
    Private m_vFSAContractLocationID As Object
    Private m_vFSAUnderwriterCnt As Object
    Private m_vFSATypeOfSaleID As Object
    Private m_vFSARenewalConsent As Object
    'FSA Phase IIIEnd

    'Policy Discount
    Private m_iDiscountReasonID As Integer
    'developer guide no.101
    Private m_vDiscountedPremium As Object
    'developer guide no.101
    Private m_vDiscountPercentage As Object
    Private m_iMatchDiscountedPremiumFlag As Integer


    'CT 30/11/00 added  docapi and  system options objects used to create a new documaster policy entry
    Private m_oSIRDOCAPI As bSIRDOCAPI.Form
    Private m_oSystemOption As bSIROptions.Business

    Private m_bEvent As Boolean

    Private m_lTransInsuranceFolderCnt As Integer

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lAnniversaryCopy As Integer
    Private m_lPutOnNextInstalmentRenewal As Integer
    Private m_lDiscountRecurringTypeId As Integer
    'TMP Added two Variables
    Private m_iLeadAllowConsolidatedCommission As Integer
    Private m_iSubAllowConsolidatedCommission As Integer
    'developer guide no.101
    Private m_vFeesTaxes As Object

    Private m_vCCTermsAgreed As Object
    Private m_vCCTermsAgreedDate As Object
    Private m_vCCInceptionDate As Object
    Private m_vCCPolicyDocumentsIssuedDate As Object
    Private m_vCCPolicyDocumentCorrect As Object
    Private m_vCCErrorNotificationDate As Object
    'developer guide no.101
    Private m_vRiskTransferAgreement As Object
    'PN38002 added new feild to insurance_file & event_insurance_file
    Private m_vRenewalPremium As Decimal

    '1.12 WR25
    Private m_vRenewalProductID As Object

    Private m_vOriginalProductID As Object
    'developer guide no.101
    Private m_vRiskTransferEditable As Object
    Private m_vCurrencyToBaseXRate As Double

    '--RFC-PLICO14 - Amit
    'developer guide no.101
    Private m_vManualDiscountPercentage As Object

    ' WPR 63
    Private m_vQuoteStatus As Integer
    Private m_vQuoteVersion As Integer
    Private m_vBaseInsuranceFolder As Integer

    'WPR73-74
    Private m_vContactuserId As Integer
    Private m_sCoInsPlacement As String
    Private m_iMTAReasonId As Integer
    Private m_nIsMarketPlacePolicy As Integer = 0
    Private m_oCollectionFrequencyID As Object
    Private m_oPaymentTermsID As Object

    Private m_vArray As Object = Nothing
    Private m_iCorrespondenceTypeID As Integer
    Private m_iDefaultPreferredCorrespondence As Integer
    Private m_iDefaultPreferredCorrespondenceCode As String
    Private m_bIsAgentCorrepondence As Boolean
    Private m_vSenderEmail As Object
    Private m_vReceiverEmail As Object

    Private m_sMediaType As String
	Private m_lOriginalInsuranceFileTypeID As Object

    Public Property MediaType() As String
        Get
            Return m_sMediaType
        End Get
        Set(ByVal Value As String)
            m_sMediaType = Value
        End Set
    End Property

    Public Property MTAReasonId() As Integer
        Get
            Return m_iMTAReasonId
        End Get
        Set(ByVal value As Integer)

            m_iMTAReasonId = value
        End Set
    End Property

    Public Property CoInsPlacement() As Object
        Get
            Return m_sCoInsPlacement
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vRiskTransferEditable = CByte(Value)
            m_sCoInsPlacement = Value
        End Set
    End Property
    'developer guide no.101
    Public Property RiskTransferEditable() As Object
        Get
            Return m_vRiskTransferEditable
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vRiskTransferEditable = CByte(Value)
            m_vRiskTransferEditable = Value
        End Set
    End Property

    'developer guide no.101
    Public Property RiskTransferAgreement() As Object
        Get
            Return m_vRiskTransferAgreement
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vRiskTransferAgreement = CByte(Value)
            m_vRiskTransferAgreement = Value
        End Set
    End Property

    Public Property CCTermsAgreed() As Object
        Get
            Return m_vCCTermsAgreed
        End Get
        Set(ByVal Value As Object)


            m_vCCTermsAgreed = Value
        End Set
    End Property
    Public Property CCTermsAgreedDate() As Object
        Get
            Return m_vCCTermsAgreedDate
        End Get
        Set(ByVal Value As Object)


            m_vCCTermsAgreedDate = Value
        End Set
    End Property
    Public Property CCInceptionDate() As Object
        Get
            Return m_vCCInceptionDate
        End Get
        Set(ByVal Value As Object)


            m_vCCInceptionDate = Value
        End Set
    End Property
    Public Property CCPolicyDocumentsIssuedDate() As Object
        Get
            Return m_vCCPolicyDocumentsIssuedDate
        End Get
        Set(ByVal Value As Object)


            m_vCCPolicyDocumentsIssuedDate = Value
        End Set
    End Property
    Public Property CCPolicyDocumentCorrect() As Object
        Get
            Return m_vCCPolicyDocumentCorrect
        End Get
        Set(ByVal Value As Object)


            m_vCCPolicyDocumentCorrect = Value
        End Set
    End Property
    Public Property CCErrorNotificationDate() As Object
        Get
            Return m_vCCErrorNotificationDate
        End Get
        Set(ByVal Value As Object)


            m_vCCErrorNotificationDate = Value
        End Set
    End Property

    '***********************************************************


    Public Property DiscountRecurringTypeId() As Integer
        Get
            Return m_lDiscountRecurringTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDiscountRecurringTypeId = Value
        End Set
    End Property


    '***********************************************************


    Public Property AnniversaryCopy() As Integer
        Get
            Return m_lAnniversaryCopy
        End Get
        Set(ByVal Value As Integer)
            m_lAnniversaryCopy = Value
        End Set
    End Property

    '***********************************************************


    Public Property PutOnNextInstalmentRenewal() As Integer
        Get
            Return m_lPutOnNextInstalmentRenewal
        End Get
        Set(ByVal Value As Integer)
            m_lPutOnNextInstalmentRenewal = Value
        End Set
    End Property

    '***********************************************************

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


    Public Property DiscountReasonID() As Integer
        Get
            Return m_iDiscountReasonID
        End Get
        Set(ByVal Value As Integer)
            m_iDiscountReasonID = Value
        End Set
    End Property

    'developer guide no.101
    Public Property DiscountedPremium() As Object
        Get
            Return m_vDiscountedPremium
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vDiscountedPremium = CByte(Value)
            m_vDiscountedPremium = Value
        End Set
    End Property


    'developer guide no.101
    Public Property DiscountPercentage() As Object
        Get
            Return m_vDiscountPercentage
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vDiscountPercentage = CByte(Value)
            m_vDiscountPercentage = Value
        End Set
    End Property



    Public Property MatchDiscountedPremiumFlag() As Integer
        Get
            Return m_iMatchDiscountedPremiumFlag
        End Get
        Set(ByVal Value As Integer)
            m_iMatchDiscountedPremiumFlag = Value
        End Set
    End Property


    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

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

    'DJM 12/04/2002 : Added property to hold description for DME Update.
    Public Property TransDescription() As String
        Get

            Return m_vTransDescription

        End Get
        Set(ByVal Value As String)


            m_vTransDescription = CStr(Value)

        End Set
    End Property

    Public Property InsuranceFileStructureID() As Integer
        Get

            Return m_lInsuranceFileStructureID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileStructureID = Value

        End Set
    End Property

    Public Property InsuranceFileTypeID() As Integer
        Get

            Return m_lInsuranceFileTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileTypeID = Value

        End Set
    End Property
    'developer guide no.101
    Public Property InsuranceFileStatusID() As Integer
        Get

            Return m_vInsuranceFileStatusID

        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Integer)


            'm_vInsuranceFileStatusID = CByte(Value)
            m_vInsuranceFileStatusID = Value

        End Set
    End Property

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

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

    'DN 16/08/01
    'DN 16/08/01
    Public Property TransInsuranceFolderCnt() As Integer
        Get

            Return m_lTransInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lTransInsuranceFolderCnt = Value

        End Set
    End Property

    Public Property InsuranceRef() As String
        Get

            Return m_sInsuranceRef.Value

        End Get
        Set(ByVal Value As String)

            m_sInsuranceRef.Value = Value

        End Set
    End Property

    Public Property ProductID() As Integer
        Get

            Return m_lProductID

        End Get
        Set(ByVal Value As Integer)

            m_lProductID = Value

        End Set
    End Property

    Public Property LeadInsurerCnt() As Object
        Get

            Return m_vLeadInsurerCnt

        End Get
        Set(ByVal Value As Object)



            m_vLeadInsurerCnt = Value

        End Set
    End Property

    Public Property LeadAgentCnt() As Object
        Get

            Return m_vLeadAgentCnt

        End Get
        Set(ByVal Value As Object)



            m_vLeadAgentCnt = Value

        End Set
    End Property

    Public Property LeadAgentPercent() As Object
        Get

            Return m_vLeadAgentPercent

        End Get
        Set(ByVal Value As Object)



            m_vLeadAgentPercent = Value

        End Set
    End Property

    Public Property AccountHandlerCnt() As Object
        Get

            Return m_vAccountHandlerCnt

        End Get
        Set(ByVal Value As Object)



            m_vAccountHandlerCnt = Value

        End Set
    End Property

    Public Property AccountExecutiveCnt() As Object
        Get

            Return m_vAccountExecutiveCnt

        End Get
        Set(ByVal Value As Object)



            m_vAccountExecutiveCnt = Value

        End Set
    End Property

    Public Property InsuredCnt() As Integer
        Get

            Return m_lInsuredCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuredCnt = Value

        End Set
    End Property

    Public Property BusinessTypeID() As Integer
        Get

            Return m_iBusinessTypeID

        End Get
        Set(ByVal Value As Integer)

            m_iBusinessTypeID = Value

        End Set
    End Property
    'developer guide no.101
    Public Property CollectTypeID() As Integer
        Get

            Return m_vCollectTypeID

        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Integer)


            'm_vCollectTypeID = CByte(Value)
            m_vCollectTypeID = Value

        End Set
    End Property

    Public Property CollectionFromCnt() As Object
        Get

            Return m_vCollectionFromCnt

        End Get
        Set(ByVal Value As Object)



            m_vCollectionFromCnt = Value

        End Set
    End Property

    Public Property SubBranchId() As Integer
        Get

            Return m_lSubBranchId

        End Get
        Set(ByVal Value As Integer)

            m_lSubBranchId = Value

        End Set
    End Property
    'sj 19/07/2002 - end

    Public Property CurrencyID() As Integer
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_iCurrencyID = Value

        End Set
    End Property


    Public Property LanguageID() As Integer
        Get

            Return m_iLanguageID

        End Get
        Set(ByVal Value As Integer)

            m_iLanguageID = Value

        End Set
    End Property
    Public Property BaseCurrencyID() As Integer
        Get

            Return m_ibaseCurrencyID

        End Get
        Set(ByVal Value As Integer)

            m_ibaseCurrencyID = Value

        End Set
    End Property
    'Datasure
    Public Property CountryID() As Integer
        Get

            Return m_iCountryId

        End Get
        Set(ByVal Value As Integer)

            m_iCountryId = Value

        End Set
    End Property

    Public Property DateIssued() As Object
        Get

            Return m_vDateIssued

        End Get
        Set(ByVal Value As Object)



            m_vDateIssued = Value

        End Set
    End Property

    Public Property CoverStartDate() As Date
        Get

            Return m_dtCoverStartDate

        End Get
        Set(ByVal Value As Date)

            m_dtCoverStartDate = Value

        End Set
    End Property

    Public Property ExpiryDate() As Date
        Get

            Return m_dtExpiryDate

        End Get
        Set(ByVal Value As Date)

            m_dtExpiryDate = Value

        End Set
    End Property

    Public Property RenewalDate() As Date
        Get

            Return m_dtRenewalDate

        End Get
        Set(ByVal Value As Date)

            m_dtRenewalDate = Value

        End Set
    End Property
    'developer guide no.101
    Public Property RenewalMethodID() As Integer
        Get

            Return m_vRenewalMethodID

        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Integer)


            'm_vRenewalMethodID = CByte(Value)
            m_vRenewalMethodID = Value

        End Set
    End Property

    Public Property RenewalFrequencyID() As Integer
        Get

            Return m_iRenewalFrequencyID

        End Get
        Set(ByVal Value As Integer)

            m_iRenewalFrequencyID = Value

        End Set
    End Property

    Public Property IsReferredAtRenewal() As Integer
        Get

            Return m_iIsReferredAtRenewal

        End Get
        Set(ByVal Value As Integer)

            m_iIsReferredAtRenewal = Value

        End Set
    End Property
    'developer guide no.101
    Public Property LapsedReasonID() As Integer
        Get

            Return m_vLapsedReasonID

        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Integer)


            'm_vLapsedReasonID = CByte(Value)
            m_vLapsedReasonID = Value

        End Set
    End Property

    Public Property LapsedDate() As Object
        Get

            Return m_vLapsedDate

        End Get
        Set(ByVal Value As Object)



            m_vLapsedDate = Value

        End Set
    End Property

    Public Property LapsedDescription() As Object
        Get

            Return m_vLapsedDescription

        End Get
        Set(ByVal Value As Object)



            m_vLapsedDescription = Value

        End Set
    End Property

    Public Property IsReferredOnMta() As Integer
        Get

            Return m_iIsReferredOnMta

        End Get
        Set(ByVal Value As Integer)

            m_iIsReferredOnMta = Value

        End Set
    End Property

    Public Property PolicyVersion() As Integer
        Get

            Return m_iPolicyVersion

        End Get
        Set(ByVal Value As Integer)

            m_iPolicyVersion = Value

        End Set
    End Property

    Public Property GeminiPolicyStatus() As Object
        Get

            Return m_vGeminiPolicyStatus

        End Get
        Set(ByVal Value As Object)



            m_vGeminiPolicyStatus = Value

        End Set
    End Property

    Public Property GeminiBusinessType() As Object
        Get

            Return m_vGeminiBusinessType

        End Get
        Set(ByVal Value As Object)



            m_vGeminiBusinessType = Value

        End Set
    End Property

    Public Property DeferredInd() As Object
        Get

            Return m_vDeferredInd

        End Get
        Set(ByVal Value As Object)



            m_vDeferredInd = Value

        End Set
    End Property

    Public Property PolicyIgnore() As Object
        Get

            Return m_vPolicyIgnore

        End Get
        Set(ByVal Value As Object)



            m_vPolicyIgnore = Value

        End Set
    End Property

    Public Property BrokerCnt() As Object
        Get

            Return m_vBrokerCnt

        End Get
        Set(ByVal Value As Object)



            m_vBrokerCnt = Value

        End Set
    End Property

    Public Property RiskCodeID() As Object
        Get

            Return m_vRiskCodeID

        End Get
        Set(ByVal Value As Object)



            m_vRiskCodeID = Value

        End Set
    End Property

    Public Property AnalysisCodeID() As Object
        Get

            Return m_vAnalysisCodeID

        End Get
        Set(ByVal Value As Object)



            m_vAnalysisCodeID = Value

        End Set
    End Property

    Public Property PolicyDeductiblesID() As Object
        Get

            Return m_vPolicyDeductiblesID

        End Get
        Set(ByVal Value As Object)



            m_vPolicyDeductiblesID = Value

        End Set
    End Property

    Public Property PolicyLimitsID() As Object
        Get

            Return m_vPolicyLimitsID

        End Get
        Set(ByVal Value As Object)



            m_vPolicyLimitsID = Value

        End Set
    End Property

    Public Property ProposalDate() As Object
        Get

            Return m_vProposalDate

        End Get
        Set(ByVal Value As Object)



            m_vProposalDate = Value

        End Set
    End Property

    Public Property DiaryDate() As Object
        Get

            Return m_vDiaryDate

        End Get
        Set(ByVal Value As Object)



            m_vDiaryDate = Value

        End Set
    End Property

    Public Property ReviewDate() As Object
        Get

            Return m_vReviewDate

        End Get
        Set(ByVal Value As Object)



            m_vReviewDate = Value

        End Set
    End Property

    Public Property RenewalDayNumber() As Object
        Get

            Return m_vRenewalDayNumber

        End Get
        Set(ByVal Value As Object)



            m_vRenewalDayNumber = Value

        End Set
    End Property

    Public Property PolicyTypeID() As Object
        Get

            Return m_vPolicyTypeId

        End Get
        Set(ByVal Value As Object)



            m_vPolicyTypeId = Value

        End Set
    End Property

    Public Property Indicator() As Object
        Get

            Return m_vIndicator

        End Get
        Set(ByVal Value As Object)



            m_vIndicator = Value

        End Set
    End Property

    Public Property Clause() As Object
        Get

            Return m_vClause

        End Get
        Set(ByVal Value As Object)



            m_vClause = Value

        End Set
    End Property

    Public Property Cover() As Object
        Get

            Return m_vCover

        End Get
        Set(ByVal Value As Object)



            m_vCover = Value

        End Set
    End Property

    Public Property Area() As Object
        Get

            Return m_vArea

        End Get
        Set(ByVal Value As Object)



            m_vArea = Value

        End Set
    End Property

    Public Property LongTermUndertakingDate() As Object
        Get

            Return m_vLongTermUndertakingDate

        End Get
        Set(ByVal Value As Object)



            m_vLongTermUndertakingDate = Value

        End Set
    End Property

    Public Property RenewalStopCodeID() As Object
        Get

            Return m_vRenewalStopCodeID

        End Get
        Set(ByVal Value As Object)



            m_vRenewalStopCodeID = Value

        End Set
    End Property

    Public Property UnderwritingYearID() As Object
        Get

            Return m_vUnderwritingYearID

        End Get
        Set(ByVal Value As Object)



            m_vUnderwritingYearID = Value

        End Set
    End Property

    Public Property PolicyStatusID() As Object
        Get
            Return m_vPolicyStatusID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyStatusID = Value
        End Set
    End Property

    Public Property InceptionTPI() As Object
        Get
            Return m_vInceptionTPI
        End Get
        Set(ByVal Value As Object)


            m_vInceptionTPI = Value
        End Set
    End Property
    'FSA Phase III
    Public Property FSACustomerCategoryID() As Object
        Get
            Return m_vFSACustomerCategoryID
        End Get
        Set(ByVal Value As Object)


            m_vFSACustomerCategoryID = Value
        End Set
    End Property
    Public Property FSAContractLocationID() As Object
        Get
            Return m_vFSAContractLocationID
        End Get
        Set(ByVal Value As Object)


            m_vFSAContractLocationID = Value
        End Set
    End Property
    Public Property FSAUnderwriterCnt() As Object
        Get
            Return m_vFSAUnderwriterCnt
        End Get
        Set(ByVal Value As Object)


            m_vFSAUnderwriterCnt = Value
        End Set
    End Property
    Public Property FSATypeOfSaleID() As Object
        Get
            Return m_vFSATypeOfSaleID
        End Get
        Set(ByVal Value As Object)


            m_vFSATypeOfSaleID = Value
        End Set
    End Property
    Public Property FSARenewalConsent() As Object
        Get
            Return m_vFSARenewalConsent
        End Get
        Set(ByVal Value As Object)


            m_vFSARenewalConsent = Value
        End Set
    End Property
    'TMP
    Public Property ConsolidatedLeadCommission() As Integer
        Get
            Return m_iLeadAllowConsolidatedCommission
        End Get
        Set(ByVal Value As Integer)
            m_iLeadAllowConsolidatedCommission = Value
        End Set
    End Property


    Public Property ConsolidatedSubCommission() As Integer
        Get
            Return m_iSubAllowConsolidatedCommission
        End Get
        Set(ByVal Value As Integer)
            m_iSubAllowConsolidatedCommission = Value
        End Set
    End Property

    'developer guide no.101
    Public Property FeesTaxes() As Object
        Get
            Return m_vFeesTaxes
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vFeesTaxes = CByte(Value)
            m_vFeesTaxes = Value
        End Set
    End Property
    'PN38002 add new field to hold renewal premium at point of MTA quote
    Public Property RenewalPremium() As Decimal
        Get
            Return m_vRenewalPremium
        End Get
        Set(ByVal Value As Decimal)

            m_vRenewalPremium = CDec(Value)
        End Set
    End Property


    'developer guide no.101
    Public Property ManualDiscountPercentage() As Object
        Get
            Return m_vManualDiscountPercentage
        End Get
        'Set(ByVal Value As Byte)
        Set(ByVal Value As Object)

            'm_vManualDiscountPercentage = CByte(Value)
            m_vManualDiscountPercentage = Value
        End Set
    End Property

    Public Property VBSType() As Object
        Get

            Return m_vVBSType

        End Get
        Set(ByVal Value As Object)



            m_vVBSType = Value

        End Set
    End Property

    Public Property VBSStatus() As Object
        Get

            Return m_vVBSStatus

        End Get
        Set(ByVal Value As Object)



            m_vVBSStatus = Value

        End Set
    End Property

    Public Property IsInsurerRateTable() As Object
        Get

            Return m_vIsInsurerRateTable

        End Get
        Set(ByVal Value As Object)



            m_vIsInsurerRateTable = Value

        End Set
    End Property

    Public Property IsRelatedPolicies() As Object
        Get

            Return m_vIsRelatedPolicies

        End Get
        Set(ByVal Value As Object)



            m_vIsRelatedPolicies = Value

        End Set
    End Property

    Public Property IsRetainedDocuments() As Object
        Get

            Return m_vIsRetainedDocuments

        End Get
        Set(ByVal Value As Object)



            m_vIsRetainedDocuments = Value

        End Set
    End Property

    Public Property SchemesPostcode() As Object
        Get

            Return m_vSchemesPostcode

        End Get
        Set(ByVal Value As Object)



            m_vSchemesPostcode = Value

        End Set
    End Property

    Public Property PaidDirect() As Object
        Get

            Return m_vPaidDirect

        End Get
        Set(ByVal Value As Object)



            m_vPaidDirect = Value

        End Set
    End Property

    Public Property Scheme() As Object
        Get

            Return m_vScheme

        End Get
        Set(ByVal Value As Object)



            m_vScheme = Value

        End Set
    End Property

    Public Property BrokerageAmount() As Object
        Get

            Return m_vBrokerageAmount

        End Get
        Set(ByVal Value As Object)



            m_vBrokerageAmount = Value

        End Set
    End Property

    Public Property IsMinimumBrokerageFlag() As Object
        Get

            Return m_vIsMinimumBrokerageFlag

        End Get
        Set(ByVal Value As Object)



            m_vIsMinimumBrokerageFlag = Value

        End Set
    End Property

    Public Property AnnualPremium() As Object
        Get

            Return m_vAnnualPremium

        End Get
        Set(ByVal Value As Object)



            m_vAnnualPremium = Value

        End Set
    End Property

    Public Property ThisPremium() As Object
        Get

            Return m_vThisPremium

        End Get
        Set(ByVal Value As Object)



            m_vThisPremium = Value

        End Set
    End Property

    Public Property NetPremium() As Object
        Get

            Return m_vNetPremium

        End Get
        Set(ByVal Value As Object)



            m_vNetPremium = Value

        End Set
    End Property

    Public Property CommissionAmount() As Object
        Get

            Return m_vCommissionAmount

        End Get
        Set(ByVal Value As Object)



            m_vCommissionAmount = Value

        End Set
    End Property

    Public Property IPTAbleAmount() As Object
        Get

            Return m_vIPTableAmount

        End Get
        Set(ByVal Value As Object)



            m_vIPTableAmount = Value

        End Set
    End Property

    Public Property IPTPercentage() As Object
        Get

            Return m_vIPTPercentage

        End Get
        Set(ByVal Value As Object)



            m_vIPTPercentage = Value

        End Set
    End Property

    Public Property ISIPTOverridden() As Object
        Get

            Return m_vIsIPTOverridden

        End Get
        Set(ByVal Value As Object)



            m_vIsIPTOverridden = Value

        End Set
    End Property

    Public Property TaxAmount() As Object
        Get

            Return m_vTaxAmount

        End Get
        Set(ByVal Value As Object)



            m_vTaxAmount = Value

        End Set
    End Property

    Public Property VatableAmount() As Object
        Get

            Return m_vVatableAmount

        End Get
        Set(ByVal Value As Object)



            m_vVatableAmount = Value

        End Set
    End Property

    Public Property VATPercentage() As Object
        Get

            Return m_vVatPercentage

        End Get
        Set(ByVal Value As Object)



            m_vVatPercentage = Value

        End Set
    End Property

    Public Property VATAmount() As Object
        Get

            Return m_vVatAmount

        End Get
        Set(ByVal Value As Object)



            m_vVatAmount = Value

        End Set
    End Property

    Public Property PaymentMethod() As Object
        Get

            Return m_vPaymentMethod

        End Get
        Set(ByVal Value As Object)



            m_vPaymentMethod = Value

        End Set
    End Property

    Public Property UserDefinedDataID() As Object
        Get

            Return m_vUserDefinedDataID

        End Get
        Set(ByVal Value As Object)



            m_vUserDefinedDataID = Value

        End Set
    End Property

    Public Property CommissionPercentage() As Object
        Get

            Return m_vCommissionPercentage

        End Get
        Set(ByVal Value As Object)



            m_vCommissionPercentage = Value

        End Set
    End Property
    'DC 14/07/00
    'DC 14/07/00
    Public Property InvariantKey() As Object
        Get

            Return m_vInvariantKey

        End Get
        Set(ByVal Value As Object)



            m_vInvariantKey = Value

        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    'TN20000817 - Doc Ref 10 (Start)
    Public Property InsuredName() As Object
        Get
            Return m_vInsuredName
        End Get
        Set(ByVal Value As Object)


            m_vInsuredName = Value
        End Set
    End Property

    Public Property AlternateReference() As Object
        Get
            Return m_vAlternateReference
        End Get
        Set(ByVal Value As Object)


            m_vAlternateReference = Value
        End Set
    End Property

    Property IsClientInvoiced() As Object
        Get
            Return m_vIsClientInvoiced
        End Get
        Set(ByVal Value As Object)


            m_vIsClientInvoiced = Value
        End Set
    End Property

    Property OldPolicyNumber() As Object
        Get
            Return m_vOldPolicyNumber
        End Get
        Set(ByVal Value As Object)


            m_vOldPolicyNumber = Value
        End Set
    End Property

    Public Property QuoteExpiryDate() As Object
        Get
            Return m_vQuoteExpiryDate
        End Get
        Set(ByVal Value As Object)


            m_vQuoteExpiryDate = Value
        End Set
    End Property

    Public Property AlternateAccountCnt() As Object
        Get
            Return m_vAlternateAccountCnt
        End Get
        Set(ByVal Value As Object)


            m_vAlternateAccountCnt = Value
        End Set
    End Property

    Public Property AnniversaryDate() As Object
        Get
            Return m_vAnniversaryDate
        End Get
        Set(ByVal Value As Object)


            m_vAnniversaryDate = Value
        End Set
    End Property

    Public Property PolicyStyleID() As Object
        Get
            Return m_vPolicyStyleID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyStyleID = Value
        End Set
    End Property


    Public Property CurrencyToBaseXRate() As Double
        Get
            Return m_vCurrencyToBaseXRate
        End Get
        Set(ByVal Value As Double)

            m_vCurrencyToBaseXRate = CDbl(Value)
        End Set
    End Property

    '1.12 WR25

    Public Property RenewalProductID() As Object
        Get
            Return m_vRenewalProductID
        End Get
        Set(ByVal Value As Object)


            m_vRenewalProductID = Value
        End Set
    End Property

    'Public Property OriginalProductID() As Byte
    Public Property OriginalProductID() As Object
        Get
            Return m_vOriginalProductID
        End Get
        Set(ByVal Value As Object)

            m_vOriginalProductID = Value
        End Set
    End Property


    ' WPR 63
    Public Property QuoteStatusID() As Integer
        Get
            Return m_vQuoteStatus
        End Get
        Set(ByVal value As Integer)

            m_vQuoteStatus = value
        End Set
    End Property

    Public Property QuoteVersionID() As Integer
        Get
            Return m_vQuoteVersion
        End Get
        Set(ByVal value As Integer)

            m_vQuoteVersion = value
        End Set
    End Property

    Public Property BaseInsuranceFolderCnt() As Integer
        Get
            Return m_vBaseInsuranceFolder
        End Get
        Set(ByVal value As Integer)

            m_vBaseInsuranceFolder = value
        End Set
    End Property

    Public Property ContactuserId() As Integer
        Get
            Return m_vContactuserId
        End Get
        Set(ByVal value As Integer)

            m_vContactuserId = value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property is this a Market Place policy or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketPlacePolicy() As Integer
        Get
            Return m_nIsMarketPlacePolicy
        End Get
        Set(ByVal value As Integer)
            m_nIsMarketPlacePolicy = value
        End Set
    End Property

    Public Property CollectionFrequencyID() As Object
        Get
            Return m_oCollectionFrequencyID
        End Get
        Set(ByVal value As Object)
            m_oCollectionFrequencyID = value
        End Set
    End Property
    Public Property PaymentTermsID() As Object
        Get
            Return m_oPaymentTermsID
        End Get
        Set(ByVal value As Object)
            m_oPaymentTermsID = value
        End Set
    End Property

    Public Property CorrespondenceType() As Integer
        Get
            Return m_iCorrespondenceTypeID
        End Get
        Set(ByVal value As Integer)
            m_iCorrespondenceTypeID = value
        End Set
    End Property

    Public Property DefaultPreferredCorrespondence() As Integer
        Get
            Return m_iDefaultPreferredCorrespondence
        End Get
        Set(ByVal value As Integer)
            m_iDefaultPreferredCorrespondence = value
        End Set
    End Property

    Public Property IsAgentCorrespondence() As Integer
        Get
            Return m_bIsAgentCorrepondence
        End Get
        Set(ByVal value As Integer)
            m_bIsAgentCorrepondence = value
        End Set
    End Property

    Public Property SenderEmail() As Object
        Get
            Return m_vSenderEmail
        End Get
        Set(ByVal value As Object)
            m_vSenderEmail = value
        End Set
    End Property

    Public Property ReceiverEmail() As Object
        Get
            Return m_vReceiverEmail
        End Get
        Set(ByVal value As Object)
            m_vReceiverEmail = value
        End Set
    End Property
	
	 Public Property OriginalInsuranceFileTypeID() As Object
        Get
            Return m_lOriginalInsuranceFileTypeID
        End Get
        Set(ByVal Value As Object)
            m_lOriginalInsuranceFileTypeID = Value
        End Set
    End Property
	
#End Region

#Region "Public Properties"
    'FSA Phase III End
    ' ***************************************************************** '
    ' Name: UpdateLapsed (Public)
    '
    ' Description: Updates all policies with the same insurance folder
    ' where insurance_file_type is MTAQUOTE or MTAQTETEMP.  If a lapse
    ' date is passed.
    '
    ' ***************************************************************** '
    Public Function UpdateLapsed(Optional ByVal v_dtLapseDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If Informations.IsNothing(v_dtLapseDate) Then

                    'Developer Guide 85
                    m_lReturn = .Parameters.Add(sName:="dtLapseDate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                Else

                    m_lReturn = .Parameters.Add(sName:="dtLapseDate", vValue:=CStr(v_dtLapseDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACInsuranceFileLapseSQL, sSQLName:=ACInsuranceFileLapseName, bStoredProcedure:=ACInsuranceFileLapseStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLapsed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLapsed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelPolicy (Public)
    '
    ' Description: Updates all policies with the same insurance folder
    ' to have a insurance_file_status of 'Cancelled'
    '
    ' ***************************************************************** '
    Public Function CancelPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACInsuranceFileCancelSQL, sSQLName:=ACInsuranceFileCancelName, bStoredProcedure:=ACInsuranceFileCancelStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oSystemOption IsNot Nothing Then
                    m_oSystemOption.Dispose()
                    m_oSystemOption = Nothing
                End If
                If m_oSIRDOCAPI IsNot Nothing Then
                    m_oSIRDOCAPI.Dispose()
                    m_oSIRDOCAPI = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_ibaseCurrencyID = 0 Then
                bPMFunc.GetBranchBaseCurrency(m_iSourceID, m_oDatabase, m_ibaseCurrencyID)
            End If

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=m_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="Cover_Start_Date", vValue:=CDate(m_dtCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


                m_lReturn = .SQLSelect(sSQL:=ACGetExpiryDateSQL, sSQLName:=ACGetExpiryDatesName, bStoredProcedure:=ACGetExpiryDateStored, vResultArray:=m_vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(m_vArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            'CT 30/11/00 added call to update documaster with new insurance file
            'DC270405 PN20539 set to 'add' mode rather than 'update' mode
            m_lReturn = CType(UpdateFileMaster(lMode:=1), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_ibaseCurrencyID = 0 Then 'PN 19848
                bPMFunc.GetBranchBaseCurrency(m_iSourceID, m_oDatabase, m_ibaseCurrencyID)
            End If

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If FromEvent Then
                    ' Execute SQL Statement
                    m_lReturn = .SQLAction(sSQL:=ACUpdateEventSQL, sSQLName:=ACUpdateEventName, bStoredProcedure:=ACUpdateEventStored, lRecordsAffected:=lRecordsAffected)
                Else
                    ' Execute SQL Statement
                    m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            'CT 30/11/00 added call to update documaster with updated insurance file
            m_lReturn = CType(UpdateFileMaster(lMode:=2), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRInsuranceFile
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If FromEvent Then
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleEventSQL, sSQLName:=ACSelectSingleEventName, bStoredProcedure:=ACSelectSingleEventStored, bKeepNulls:=True)
                Else
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                'developer guide no. 
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' SetPropertiesFromDB
    ''' </summary>
    ''' <param name="oFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                InsuranceFileCnt = oFields("insurance_file_cnt")
                InsuranceFileStructureID = oFields("insurance_file_structure_id")
                InsuranceFileTypeID = oFields("insurance_file_type_id")

                If Informations.IsDBNull(oFields("insurance_file_status_id")) OrElse Informations.IsNothing(oFields("insurance_file_status_id")) Then

                    InsuranceFileStatusID = Nothing
                Else
                    InsuranceFileStatusID = oFields("insurance_file_status_id")
                End If
                InsuranceFileID = oFields("insurance_file_id")
                SourceID = oFields("source_id")
                InsuranceFolderCnt = oFields("insurance_folder_cnt")
                InsuranceRef = oFields("insurance_ref")
                ProductID = oFields("product_id")

                If Informations.IsDBNull(oFields("lead_insurer_cnt")) OrElse Informations.IsNothing(oFields("lead_insurer_cnt")) Then
                    LeadInsurerCnt = Nothing
                Else
                    LeadInsurerCnt = oFields("lead_insurer_cnt")
                End If

                If Informations.IsDBNull(oFields("lead_agent_cnt")) OrElse Informations.IsNothing(oFields("lead_agent_cnt")) Then
                    LeadAgentCnt = Nothing
                Else
                    LeadAgentCnt = oFields("lead_agent_cnt")
                End If

                If Informations.IsDBNull(oFields("lead_agent_percent")) OrElse Informations.IsNothing(oFields("lead_agent_percent")) Then
                    LeadAgentPercent = Nothing
                Else
                    LeadAgentPercent = oFields("lead_agent_percent")
                End If

                If Informations.IsDBNull(oFields("account_handler_cnt")) OrElse Informations.IsNothing(oFields("account_handler_cnt")) Then
                    AccountHandlerCnt = Nothing
                Else
                    AccountHandlerCnt = oFields("account_handler_cnt")
                End If
                InsuredCnt = oFields("insured_cnt")
                BusinessTypeID = oFields("business_type_id")

                If Informations.IsDBNull(oFields("collect_type_id")) OrElse Informations.IsNothing(oFields("collect_type_id")) Then
                    CollectTypeID = Nothing
                Else
                    CollectTypeID = oFields("collect_type_id")
                End If

                If Informations.IsDBNull(oFields("collection_from_cnt")) OrElse Informations.IsNothing(oFields("collection_from_cnt")) Then
                    CollectionFromCnt = Nothing
                Else
                    CollectionFromCnt = oFields("collection_from_cnt")
                End If

                SubBranchId = oFields("branch_id")

                CurrencyID = oFields("currency_id")
                LanguageID = oFields("language_id")

                If Informations.IsDBNull(oFields("date_issued")) OrElse Informations.IsNothing(oFields("date_issued")) Then
                    DateIssued = Nothing
                Else
                    DateIssued = oFields("date_issued")
                End If
                CoverStartDate = oFields("cover_start_date")
                ExpiryDate = oFields("expiry_date")
                RenewalDate = oFields("renewal_date")

                If Informations.IsDBNull(oFields("renewal_method_id")) OrElse Informations.IsNothing(oFields("renewal_method_id")) Then
                    RenewalMethodID = Nothing
                Else
                    RenewalMethodID = oFields("renewal_method_id")
                End If
                RenewalFrequencyID = oFields("renewal_frequency_id")
                IsReferredAtRenewal = oFields("is_referred_at_renewal")

                If Informations.IsDBNull(oFields("lapsed_reason_id")) OrElse Informations.IsNothing(oFields("lapsed_reason_id")) Then
                    LapsedReasonID = Nothing
                Else
                    LapsedReasonID = oFields("lapsed_reason_id")
                End If

                If Informations.IsDBNull(oFields("lapsed_date")) OrElse Informations.IsNothing(oFields("lapsed_date")) OrElse (CDate(oFields("lapsed_date")).Year) < 1901 Then

                    LapsedDate = Nothing
                Else
                    LapsedDate = oFields("lapsed_date")
                End If

                If Informations.IsDBNull(oFields("lapsed_description")) OrElse Informations.IsNothing(oFields("lapsed_description")) Then
                    LapsedDescription = Nothing
                Else

                    LapsedDescription = oFields("lapsed_description")
                End If
                IsReferredOnMta = oFields("is_referred_on_mta")

                If Informations.IsDBNull(oFields("policy_version")) OrElse Informations.IsNothing(oFields("policy_version")) Then
                    PolicyVersion = Nothing
                Else
                    PolicyVersion = oFields("policy_version")
                End If

                If Informations.IsDBNull(oFields("gemini_policy_status")) OrElse Informations.IsNothing(oFields("gemini_policy_status")) Then
                    GeminiPolicyStatus = Nothing
                Else
                    GeminiPolicyStatus = oFields("gemini_policy_status")
                End If

                If Informations.IsDBNull(oFields("gemini_business_type")) OrElse Informations.IsNothing(oFields("gemini_business_type")) Then
                    GeminiBusinessType = Nothing
                Else
                    GeminiBusinessType = oFields("gemini_business_type")
                End If

                If Informations.IsDBNull(oFields("deferred_ind")) OrElse Informations.IsNothing(oFields("deferred_ind")) Then
                    DeferredInd = Nothing
                Else
                    DeferredInd = oFields("deferred_ind")
                End If

                If Informations.IsDBNull(oFields("policy_ignore")) OrElse Informations.IsNothing(oFields("policy_ignore")) Then
                    PolicyIgnore = Nothing
                Else
                    PolicyIgnore = oFields("policy_ignore")
                End If

                If Informations.IsDBNull(oFields("broker_cnt")) OrElse Informations.IsNothing(oFields("broker_cnt")) Then
                    BrokerCnt = Nothing
                Else
                    BrokerCnt = oFields("broker_cnt")
                End If

                If Informations.IsDBNull(oFields("risk_code_id")) OrElse Informations.IsNothing(oFields("risk_code_id")) Then
                    RiskCodeID = Nothing
                Else
                    RiskCodeID = oFields("risk_code_id")
                End If

                If Informations.IsDBNull(oFields("analysis_code_id")) OrElse Informations.IsNothing(oFields("analysis_code_id")) Then
                    AnalysisCodeID = Nothing
                Else
                    AnalysisCodeID = oFields("analysis_code_id")
                End If

                If Informations.IsDBNull(oFields("policy_deductibles_id")) OrElse Informations.IsNothing(oFields("policy_deductibles_id")) Then
                    PolicyDeductiblesID = Nothing
                Else
                    PolicyDeductiblesID = oFields("policy_deductibles_id")
                End If

                If Informations.IsDBNull(oFields("policy_limits_id")) OrElse Informations.IsNothing(oFields("policy_limits_id")) Then
                    PolicyLimitsID = Nothing
                Else
                    PolicyLimitsID = oFields("policy_limits_id")
                End If

                If Informations.IsDBNull(oFields("proposal_date")) OrElse Informations.IsNothing(oFields("proposal_date")) Then
                    ProposalDate = Nothing
                Else
                    ProposalDate = oFields("proposal_date")
                End If

                If Informations.IsDBNull(oFields("diary_date")) OrElse Informations.IsNothing(oFields("diary_date")) Then
                    DiaryDate = Nothing
                Else
                    DiaryDate = oFields("diary_date")
                End If

                If Informations.IsDBNull(oFields("review_date")) OrElse Informations.IsNothing(oFields("review_date")) Then
                    ReviewDate = Nothing
                Else
                    ReviewDate = oFields("review_date")
                End If

                If Informations.IsDBNull(oFields("renewal_day_number")) OrElse Informations.IsNothing(oFields("renewal_day_number")) Then
                    RenewalDayNumber = Nothing
                Else
                    RenewalDayNumber = oFields("renewal_day_number")
                End If

                If Informations.IsDBNull(oFields("policy_type_id")) OrElse Informations.IsNothing(oFields("policy_type_id")) Then
                    PolicyTypeID = Nothing
                Else
                    PolicyTypeID = oFields("policy_type_id")
                End If

                If Informations.IsDBNull(oFields("indicator")) OrElse Informations.IsNothing(oFields("indicator")) Then
                    Indicator = Nothing
                Else
                    Indicator = oFields("indicator")
                End If

                If Informations.IsDBNull(oFields("clause")) OrElse Informations.IsNothing(oFields("clause")) Then
                    Clause = Nothing
                Else
                    Clause = oFields("clause")
                End If

                If Informations.IsDBNull(oFields("cover")) OrElse Informations.IsNothing(oFields("cover")) Then
                    Cover = Nothing
                Else
                    Cover = oFields("cover")
                End If

                If Informations.IsDBNull(oFields("area")) OrElse Informations.IsNothing(oFields("area")) Then
                    Area = Nothing
                Else
                    Area = oFields("area")
                End If

                If Informations.IsDBNull(oFields("long_term_undertaking_date")) OrElse Informations.IsNothing(oFields("long_term_undertaking_date")) Then
                    LongTermUndertakingDate = Nothing
                Else
                    LongTermUndertakingDate = oFields("long_term_undertaking_date")
                End If

                If Informations.IsDBNull(oFields("renewal_stop_code_id")) OrElse Informations.IsNothing(oFields("renewal_stop_code_id")) Then
                    RenewalStopCodeID = Nothing
                Else
                    RenewalStopCodeID = oFields("renewal_stop_code_id")
                End If

                If Informations.IsDBNull(oFields("vbs_type")) OrElse Informations.IsNothing(oFields("vbs_type")) Then
                    VBSType = Nothing
                Else
                    VBSType = oFields("vbs_type")
                End If

                If Informations.IsDBNull(oFields("vbs_status")) OrElse Informations.IsNothing(oFields("vbs_status")) Then
                    VBSStatus = Nothing
                Else
                    VBSStatus = oFields("vbs_status")
                End If

                If Informations.IsDBNull(oFields("is_insurer_rate_table")) OrElse Informations.IsNothing(oFields("is_insurer_rate_table")) Then
                    IsInsurerRateTable = Nothing
                Else
                    IsInsurerRateTable = oFields("is_insurer_rate_table")
                End If

                If Informations.IsDBNull(oFields("is_related_policies")) OrElse Informations.IsNothing(oFields("is_related_policies")) Then
                    IsRelatedPolicies = Nothing
                Else
                    IsRelatedPolicies = oFields("is_related_policies")
                End If

                If Informations.IsDBNull(oFields("is_retained_documents")) OrElse Informations.IsNothing(oFields("is_retained_documents")) Then
                    IsRetainedDocuments = Nothing
                Else
                    IsRetainedDocuments = oFields("is_retained_documents")
                End If

                If Informations.IsDBNull(oFields("schemes_postcode")) OrElse Informations.IsNothing(oFields("schemes_postcode")) Then
                    SchemesPostcode = Nothing
                Else
                    SchemesPostcode = oFields("schemes_postcode")
                End If

                If Informations.IsDBNull(oFields("paid_direct")) OrElse Informations.IsNothing(oFields("paid_direct")) Then
                    PaidDirect = Nothing
                Else
                    PaidDirect = oFields("paid_direct")
                End If

                If Informations.IsDBNull(oFields("scheme")) OrElse Informations.IsNothing(oFields("scheme")) Then
                    Scheme = Nothing
                Else
                    Scheme = oFields("scheme")
                End If

                If Informations.IsDBNull(oFields("brokerage_amount")) OrElse Informations.IsNothing(oFields("brokerage_amount")) Then
                    BrokerageAmount = Nothing
                Else
                    BrokerageAmount = oFields("brokerage_amount")
                End If

                If Informations.IsDBNull(oFields("is_minimum_brokerage_flag")) OrElse Informations.IsNothing(oFields("is_minimum_brokerage_flag")) Then
                    IsMinimumBrokerageFlag = Nothing
                Else
                    IsMinimumBrokerageFlag = oFields("is_minimum_brokerage_flag")
                End If

                If Informations.IsDBNull(oFields("annual_premium")) OrElse Informations.IsNothing(oFields("annual_premium")) Then
                    AnnualPremium = Nothing
                Else
                    AnnualPremium = oFields("annual_premium")
                End If

                If Informations.IsDBNull(oFields("this_premium")) OrElse Informations.IsNothing(oFields("this_premium")) Then
                    ThisPremium = Nothing
                Else
                    ThisPremium = oFields("this_premium")
                End If

                If Informations.IsDBNull(oFields("net_premium")) OrElse Informations.IsNothing(oFields("net_premium")) Then
                    NetPremium = Nothing
                Else
                    NetPremium = oFields("net_premium")
                End If

                If Informations.IsDBNull(oFields("commission_amount")) OrElse Informations.IsNothing(oFields("commission_amount")) Then
                    CommissionAmount = Nothing
                Else
                    CommissionAmount = oFields("commission_amount")
                End If

                If Informations.IsDBNull(oFields("iptable_amount")) OrElse Informations.IsNothing(oFields("iptable_amount")) Then
                    IPTAbleAmount = Nothing
                Else
                    IPTAbleAmount = oFields("iptable_amount")
                End If

                If Informations.IsDBNull(oFields("ipt_percentage")) OrElse Informations.IsNothing(oFields("ipt_percentage")) Then
                    IPTPercentage = Nothing
                Else
                    IPTPercentage = oFields("ipt_percentage")
                End If

                If Informations.IsDBNull(oFields("is_ipt_overridden")) OrElse Informations.IsNothing(oFields("is_ipt_overridden")) Then
                    ISIPTOverridden = Nothing
                Else
                    ISIPTOverridden = oFields("is_ipt_overridden")
                End If

                If Informations.IsDBNull(oFields("tax_amount")) OrElse Informations.IsNothing(oFields("tax_amount")) Then
                    TaxAmount = Nothing
                Else
                    TaxAmount = oFields("tax_amount")
                End If

                If Informations.IsDBNull(oFields("vatable_amount")) OrElse Informations.IsNothing(oFields("vatable_amount")) Then
                    VatableAmount = Nothing
                Else
                    VatableAmount = oFields("vatable_amount")
                End If

                If Informations.IsDBNull(oFields("vat_percentage")) OrElse Informations.IsNothing(oFields("vat_percentage")) Then
                    VATPercentage = Nothing
                Else
                    VATPercentage = oFields("vat_percentage")
                End If

                If Informations.IsDBNull(oFields("vat_amount")) OrElse Informations.IsNothing(oFields("vat_amount")) Then
                    VATAmount = Nothing
                Else
                    VATAmount = oFields("vat_amount")
                End If

                If Informations.IsDBNull(oFields("payment_method")) OrElse Informations.IsNothing(oFields("payment_method")) Then
                    PaymentMethod = Nothing
                Else
                    PaymentMethod = oFields("payment_method")
                End If

                If Informations.IsDBNull(oFields("user_defined_data_id")) OrElse Informations.IsNothing(oFields("user_defined_data_id")) Then
                    UserDefinedDataID = Nothing
                Else
                    UserDefinedDataID = oFields("user_defined_data_id")
                End If

                If Informations.IsDBNull(oFields("commission_percentage")) OrElse Informations.IsNothing(oFields("commission_percentage")) Then
                    CommissionPercentage = Nothing
                Else

                    CommissionPercentage = oFields("commission_percentage")
                End If

                If Informations.IsDBNull(oFields("invariant_key")) OrElse Informations.IsNothing(oFields("invariant_key")) Then
                    InvariantKey = Nothing
                Else
                    InvariantKey = oFields("invariant_key")
                End If

                If Informations.IsDBNull(oFields("insured_name")) OrElse Informations.IsNothing(oFields("insured_name")) Then
                    InsuredName = Nothing
                Else
                    InsuredName = oFields("insured_name")
                End If

                If Informations.IsDBNull(oFields("alternate_reference")) OrElse Informations.IsNothing(oFields("alternate_reference")) Then
                    AlternateReference = Nothing
                Else
                    AlternateReference = oFields("alternate_reference")
                End If

                If Informations.IsDBNull(oFields("is_client_invoiced")) OrElse Informations.IsNothing(oFields("is_client_invoiced")) Then
                    IsClientInvoiced = Nothing
                Else
                    IsClientInvoiced = oFields("is_client_invoiced")
                End If

                If Informations.IsDBNull(oFields("old_policy_number")) OrElse Informations.IsNothing(oFields("old_policy_number")) Then
                    OldPolicyNumber = Nothing
                Else
                    OldPolicyNumber = oFields("old_policy_number")
                End If

                If Informations.IsDBNull(oFields("quote_expiry_date")) OrElse Informations.IsNothing(oFields("quote_expiry_date")) Then
                    QuoteExpiryDate = Nothing
                Else
                    QuoteExpiryDate = oFields("quote_expiry_date")
                End If

                If Informations.IsDBNull(oFields("alternate_account_cnt")) OrElse Informations.IsNothing(oFields("alternate_account_cnt")) Then
                    AlternateAccountCnt = Nothing
                Else
                    AlternateAccountCnt = oFields("alternate_account_cnt")
                End If

                If Informations.IsDBNull(oFields("account_executive_cnt")) OrElse Informations.IsNothing(oFields("account_executive_cnt")) Then
                    AccountExecutiveCnt = Nothing
                Else
                    AccountExecutiveCnt = oFields("account_executive_cnt")
                End If

                If Informations.IsDBNull(oFields("anniversary_date")) OrElse Informations.IsNothing(oFields("anniversary_date")) Then
                    AnniversaryDate = Nothing
                Else
                    AnniversaryDate = oFields("anniversary_date")
                End If

                If Informations.IsDBNull(oFields("policy_style_id")) OrElse Informations.IsNothing(oFields("policy_style_id")) Then
                    m_vPolicyStyleID = Nothing
                Else
                    m_vPolicyStyleID = oFields("policy_style_id")
                End If

                If Informations.IsDBNull(oFields("underwriting_year_id")) OrElse Informations.IsNothing(oFields("underwriting_year_id")) Then
                    m_vUnderwritingYearID = Nothing
                Else
                    m_vUnderwritingYearID = oFields("underwriting_year_id")
                End If

                If Informations.IsDBNull(oFields("policy_status_id")) OrElse Informations.IsNothing(oFields("policy_status_id")) Then
                    m_vPolicyStatusID = Nothing
                Else
                    m_vPolicyStatusID = oFields("policy_status_id")
                End If

                If Informations.IsDBNull(oFields("inception_date_tpi")) OrElse Informations.IsNothing(oFields("inception_date_tpi")) Then
                    m_vInceptionTPI = Nothing
                Else
                    m_vInceptionTPI = oFields("inception_date_tpi")
                End If

                If Informations.IsDBNull(oFields("fsa_customer_category_id")) OrElse Informations.IsNothing(oFields("fsa_customer_category_id")) Then
                    m_vFSACustomerCategoryID = Nothing
                Else
                    m_vFSACustomerCategoryID = oFields("fsa_customer_category_id")
                End If

                If Informations.IsDBNull(oFields("fsa_contract_location_id")) OrElse Informations.IsNothing(oFields("fsa_contract_location_id")) Then
                    m_vFSAContractLocationID = Nothing
                Else
                    m_vFSAContractLocationID = oFields("fsa_contract_location_id")
                End If

                If Informations.IsDBNull(oFields("fsa_underwriter_cnt")) OrElse Informations.IsNothing(oFields("fsa_underwriter_cnt")) Then
                    m_vFSAUnderwriterCnt = Nothing
                Else
                    m_vFSAUnderwriterCnt = oFields("fsa_underwriter_cnt")
                End If

                If Informations.IsDBNull(oFields("fsa_type_of_sale_id")) OrElse Informations.IsNothing(oFields("fsa_type_of_sale_id")) Then
                    m_vFSATypeOfSaleID = Nothing
                Else
                    m_vFSATypeOfSaleID = oFields("fsa_type_of_sale_id")
                End If

                If Informations.IsDBNull(oFields("fsa_renewal_consent")) OrElse Informations.IsNothing(oFields("fsa_renewal_consent")) Then
                    m_vFSARenewalConsent = Nothing
                Else
                    m_vFSARenewalConsent = oFields("fsa_renewal_consent")
                End If

                If Informations.IsDBNull(oFields("base_currency_id")) OrElse Informations.IsNothing(oFields("base_currency_id")) Then
                    BaseCurrencyID = 0
                Else
                    BaseCurrencyID = oFields("base_currency_id")
                End If

                If Informations.IsDBNull(oFields("country_id")) OrElse Informations.IsNothing(oFields("country_id")) Then
                    CountryID = 0
                Else
                    CountryID = oFields("country_id")
                End If

                'PN38002 add new field to hold renewal premium at point of MTA quote
                If Informations.IsDBNull(oFields("renewal_premium")) OrElse Informations.IsNothing(oFields("renewal_premium")) Then
                    RenewalPremium = Nothing
                Else
                    RenewalPremium = oFields("renewal_premium")
                End If

                m_lReturn = CType(GetPolicyDesc(m_lInsuranceFolderCnt, m_vTransDescription), gPMConstants.PMEReturnCode)

                If Informations.IsDBNull(oFields("discount_reason_id")) OrElse Informations.IsNothing(oFields("discount_reason_id")) Then
                    DiscountReasonID = 0
                Else
                    DiscountReasonID = oFields("discount_reason_id")
                End If

                If Informations.IsDBNull(oFields("discounted_premium")) OrElse Informations.IsNothing(oFields("discounted_premium")) Then
                    DiscountedPremium = 0
                Else
                    DiscountedPremium = oFields("discounted_premium")
                End If

                If Informations.IsDBNull(oFields("discount_percentage")) OrElse Informations.IsNothing(oFields("discount_percentage")) Then
                    DiscountPercentage = 0
                Else
                    DiscountPercentage = oFields("discount_percentage")
                End If

                If Informations.IsDBNull(oFields("match_discounted_premium_flag")) OrElse Informations.IsNothing(oFields("match_discounted_premium_flag")) Then
                    MatchDiscountedPremiumFlag = 0
                Else
                    MatchDiscountedPremiumFlag = oFields("match_discounted_premium_flag")
                End If

                ' put on next instalment renewal

                If Informations.IsDBNull(oFields("put_on_next_instalment_renewal")) OrElse Informations.IsNothing(oFields("put_on_next_instalment_renewal")) Then
                    m_lPutOnNextInstalmentRenewal = 0
                Else
                    m_lPutOnNextInstalmentRenewal = oFields("put_on_next_instalment_renewal")
                End If

                ' anniversary copy

                If Informations.IsDBNull(oFields("anniversary_copy")) OrElse Informations.IsNothing(oFields("anniversary_copy")) Then
                    m_lAnniversaryCopy = 0
                Else
                    m_lAnniversaryCopy = oFields("anniversary_copy")
                End If

                If Informations.IsDBNull(oFields("discount_recurring_type_id")) OrElse Informations.IsNothing(oFields("discount_recurring_type_id")) Then
                    m_lDiscountRecurringTypeId = 0
                Else
                    m_lDiscountRecurringTypeId = oFields("discount_recurring_type_id")
                End If

                'TMP lead_allow_consolidated_commission

                If Informations.IsDBNull(oFields("lead_allow_consolidated_commission")) OrElse Informations.IsNothing(oFields("lead_allow_consolidated_commission")) Then
                    m_iLeadAllowConsolidatedCommission = 0
                Else
                    m_iLeadAllowConsolidatedCommission = oFields("lead_allow_consolidated_commission")
                End If

                If Informations.IsDBNull(oFields("sub_allow_consolidated_commission")) OrElse Informations.IsNothing(oFields("sub_allow_consolidated_commission")) Then
                    m_iSubAllowConsolidatedCommission = 0
                Else
                    m_iSubAllowConsolidatedCommission = oFields("sub_allow_consolidated_commission")
                End If

                If Informations.IsDBNull(oFields("fees_taxes")) OrElse Informations.IsNothing(oFields("fees_taxes")) Then
                    FeesTaxes = 0
                Else
                    FeesTaxes = oFields("fees_taxes")
                End If

                If Informations.IsDBNull(oFields("terms_agreed")) OrElse Informations.IsNothing(oFields("terms_agreed")) Then
                    CCTermsAgreed = Nothing
                Else
                    CCTermsAgreed = oFields("terms_agreed")
                End If

                If Informations.IsDBNull(oFields("terms_agreed_date")) OrElse Informations.IsNothing(oFields("terms_agreed_date")) Then
                    CCTermsAgreedDate = Nothing
                Else
                    CCTermsAgreedDate = oFields("terms_agreed_date")
                End If

                If Informations.IsDBNull(oFields("inception_date")) OrElse Informations.IsNothing(oFields("inception_date")) Then
                    CCInceptionDate = Nothing
                Else
                    CCInceptionDate = oFields("inception_date")
                End If

                If Informations.IsDBNull(oFields("policy_documents_issued_date")) OrElse Informations.IsNothing(oFields("policy_documents_issued_date")) Then
                    CCPolicyDocumentsIssuedDate = Nothing
                Else
                    CCPolicyDocumentsIssuedDate = oFields("policy_documents_issued_date")
                End If

                If Informations.IsDBNull(oFields("policy_documents_correct")) OrElse Informations.IsNothing(oFields("policy_documents_correct")) Then
                    CCPolicyDocumentCorrect = Nothing
                Else
                    CCPolicyDocumentCorrect = oFields("policy_documents_correct")
                End If

                If Informations.IsDBNull(oFields("error_notification_date")) OrElse Informations.IsNothing(oFields("error_notification_date")) Then
                    CCErrorNotificationDate = Nothing
                Else
                    CCErrorNotificationDate = oFields("error_notification_date")
                End If

                If Informations.IsDBNull(oFields("risk_transfer_agreement")) OrElse Informations.IsNothing(oFields("risk_transfer_agreement")) Then
                    RiskTransferAgreement = Nothing
                Else
                    RiskTransferAgreement = If(oFields("risk_transfer_agreement"), 1, 0)
                End If

                If Informations.IsDBNull(oFields("Renewal_product_id")) OrElse Informations.IsNothing(oFields("Renewal_product_id")) Then
                    m_vRenewalProductID = Nothing
                Else
                    m_vRenewalProductID = oFields("Renewal_product_id")
                End If

                If Informations.IsDBNull(oFields("original_product_id")) OrElse Informations.IsNothing(oFields("original_product_id")) Then

                    m_vOriginalProductID = Nothing
                Else
                    m_vOriginalProductID = oFields("original_product_id")
                End If

                If Informations.IsDBNull(oFields("risk_transfer_editable")) OrElse Informations.IsNothing(oFields("risk_transfer_editable")) Then
                    RiskTransferEditable = 0
                Else
                    RiskTransferEditable = oFields("risk_transfer_editable")
                End If

                If Informations.IsDBNull(oFields("currency_base_xrate")) OrElse Informations.IsNothing(oFields("currency_base_xrate")) Then

                    CurrencyToBaseXRate = Nothing
                Else
                    CurrencyToBaseXRate = gPMFunctions.ToSafeDouble(oFields("currency_base_xrate"), 1)
                End If

                If Informations.IsDBNull(oFields("manual_discount_percentage")) OrElse Informations.IsNothing(oFields("manual_discount_percentage")) Then
                    ManualDiscountPercentage = 0
                Else
                    ManualDiscountPercentage = oFields("manual_discount_percentage")
                End If

                ' WPR 63
                If Informations.IsDBNull(oFields("quote_status_id")) OrElse Informations.IsNothing(oFields("quote_status_id")) Then
                    QuoteStatusID = 0
                Else
                    QuoteStatusID = oFields("quote_status_id")
                End If

                If Informations.IsDBNull(oFields("quote_version")) OrElse Informations.IsNothing(oFields("quote_version")) Then
                    QuoteVersionID = 0
                Else
                    QuoteVersionID = oFields("quote_version")
                End If

                If Informations.IsDBNull(oFields("base_insurance_folder_cnt")) OrElse Informations.IsNothing(oFields("base_insurance_folder_cnt")) Then
                    BaseInsuranceFolderCnt = 0
                Else
                    BaseInsuranceFolderCnt = oFields("base_insurance_folder_cnt")
                End If

                If Informations.IsDBNull(oFields("Contact_user_id")) OrElse Informations.IsNothing(oFields("Contact_user_id")) Then
                    ContactuserId = 0
                Else
                    ContactuserId = oFields("Contact_user_id")
                End If

                If Informations.IsDBNull(oFields("coins_placement")) OrElse Informations.IsNothing(oFields("coins_placement")) Then
                    CoInsPlacement = Nothing
                Else
                    CoInsPlacement = oFields("coins_placement")
                End If

                If Informations.IsDBNull(oFields("MTA_reason_id")) OrElse Informations.IsNothing(oFields("MTA_reason_id")) Then
                    MTAReasonId = 0
                Else
                    MTAReasonId = ToSafeInteger(oFields("MTA_reason_id"))
                End If

                If Informations.IsDBNull(oFields("is_marketplace_policy")) Or Informations.IsNothing(oFields("is_marketplace_policy")) Then
                    IsMarketPlacePolicy = 0
                Else
                    IsMarketPlacePolicy = ToSafeInteger(oFields("is_marketplace_policy"))
                End If

                If Informations.IsDBNull(oFields("DOPaymentTerms_id")) OrElse Informations.IsNothing(oFields("DOPaymentTerms_id")) Then
                    PaymentTermsID = 0
                Else
                    PaymentTermsID = ToSafeInteger(oFields("DOPaymentTerms_id"))
                End If

                If Informations.IsDBNull(oFields("CollectionFrequency_id")) OrElse Informations.IsNothing(oFields("CollectionFrequency_id")) Then
                    CollectionFrequencyID = 0
                Else
                    CollectionFrequencyID = ToSafeInteger(oFields("CollectionFrequency_id"))
                End If

                If Informations.IsDBNull(oFields("coins_placement")) OrElse Informations.IsNothing(oFields("coins_placement")) Then
                    CoInsPlacement = Nothing
                Else
                    CoInsPlacement = oFields("coins_placement")
                End If

                If Informations.IsDBNull(oFields("Correspondence_Type")) Or Informations.IsNothing(oFields("Correspondence_Type")) Then
                    CorrespondenceType = 0
                Else
                    CorrespondenceType = ToSafeInteger(oFields("Correspondence_Type"))
                End If
                If Informations.IsDBNull(oFields("Default_Preferred_Correspondence")) Or Informations.IsNothing(oFields("Default_Preferred_Correspondence")) Then
                    DefaultPreferredCorrespondence = 0
                Else
                    DefaultPreferredCorrespondence = ToSafeInteger(oFields("Default_Preferred_Correspondence"))

                End If
                If Informations.IsDBNull(oFields("Is_Agent_Correspondence")) Or Informations.IsNothing(oFields("Is_Agent_Correspondence")) Then
                    IsAgentCorrespondence = 0
                Else
                    IsAgentCorrespondence = ToSafeInteger(oFields("Is_Agent_Correspondence"))
                End If

                If Informations.IsDBNull(oFields("media_type")) Or Informations.IsNothing(oFields("Media_type")) Then

                    MediaType = Nothing
                Else

                    MediaType = oFields("Media_type")
                End If

                If Convert.IsDBNull(oFields("original_insurance_file_type_id")) OrElse Informations.IsNothing(oFields("original_insurance_file_type_id")) Then
                    OriginalInsuranceFileTypeID = Nothing
                Else
                    OriginalInsuranceFileTypeID = oFields("original_insurance_file_type_id")
                End If
				
            End With

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log ErrOrElse Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyFolderFromEvent (Public)
    '
    ' Description:Overwrites the Insurance Folder from the event table.
    '
    ' ***************************************************************** '
    Public Function CopyFolderToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(InsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyFolderToEventSQL, sSQLName:=ACCopyFolderToEventName, bStoredProcedure:=ACCopyFolderToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFolderToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolderToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyFileToEvent (Public)
    '
    ' Description: Makes a copy of the Insurance File on the event table.
    '
    ' ***************************************************************** '
    Public Function CopyFileToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyFileToEventSQL, sSQLName:=ACCopyFileToEventName, bStoredProcedure:=ACCopyFileToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFileToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFileToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopySectionsFromEvent (Public)
    '
    ' Description: Copies Sections from the event table.
    '
    ' History   : Created 30/06/2005 datasure
    ' ***************************************************************** '
    Public Function CopySectionsFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First Coinsurers
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToPolicySectionsSQL, sSQLName:=ACCopyEventToPolicySectionsName, bStoredProcedure:=ACCopyEventToPolicySectionsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopySectionsFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopySectionsFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyCoinsurerSectionsToEvent (Public)
    '
    ' Description: Makes a copy of the Coinsurer Sections to the event table.
    '
    ' History   : Created 15/09/2006 A.Robinson
    ' ***************************************************************** '
    Public Function CopyCoinsurerSectionsToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyCoinsurerSectionToEventSQL, sSQLName:=ACCopyCoinsurerSectionToEventName, bStoredProcedure:=ACCopyCoinsurerSectionToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyCoinsurerSectionsToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyCoinsurerSectionsToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyCoinsurerSectionsFromEvent (Public)
    '
    ' Description: Copies Coinsurer Sections from the event table.
    '
    ' History   : Created 15/09/2006 A.Robinson
    ' ***************************************************************** '
    Public Function CopyCoinsurerSectionsFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToCoinsurerSectionSQL, sSQLName:=ACCopyEventToCoinsurerSectionName, bStoredProcedure:=ACCopyEventToCoinsurerSectionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyCoinsurerSectionsFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyCoinsurerSectionsFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyTaxCalculationToEventSQL (Public)
    '
    ' Description: Makes a copy of the Tax Breakdown on the event table.
    '
    ' History   : Created 06/08/2005 datasure
    ' ***************************************************************** '
    Public Function CopyTaxCalculationToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First Coinsurers
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyTaxCalculationToEventSQL, sSQLName:=ACCopyTaxCalculationToEventName, bStoredProcedure:=ACCopyTaxCalculationToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyTaxCalculationToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTaxCalculationToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RecalculatetaxCalculationAfterEventSQL (Public)
    '
    ' Description: Rebuilds the tax calculation for the policy
    '
    ' History   : Created 15/08/2005 datasure
    ' ***************************************************************** '
    Public Function RecalculatetaxCalculationAfterEvent(ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First Coinsurers
            m_oDatabase.Parameters.Clear()



            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACReCalculateTaxCalculationSQL, sSQLName:=ACReCalculateTaxCalculationName, bStoredProcedure:=ACReCalculateTaxCalculationStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculatetaxCalculationAfterEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculatetaxCalculationAfterEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'EK 05/09/99 New Methods to update Insurance files from Editable Events
    ' ***************************************************************** '
    ' Name: CopyFolderFromEvent (Public)
    '
    ' Description: Overwritetes the Insurance Folder from the event table.
    '
    ' ***************************************************************** '
    Public Function CopyFolderFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFolderCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToFolderSQL, sSQLName:=ACCopyEventToFolderName, bStoredProcedure:=ACCopyEventToFolderStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFolderFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolderFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyFileFromEvent (Public)
    '
    ' Description: Overwrites the Insurance File from the event table.
    '
    ' ***************************************************************** '
    Public Function CopyFileFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object, ByVal v_lInsuranceFolderCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToFileSQL, sSQLName:=ACCopyEventToFileName, bStoredProcedure:=ACCopyEventToFileStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFileFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFileFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddFileToEvent (Public)
    '
    ' Description: Adds Event to Policy ( MTA Debits)
    ' 22082005 Method replaced for Datasure - see Old_AddFielToEvent for replaced method
    ' ***************************************************************** '
    Public Function AddFileToEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Object) As Integer
        'PN38002 add new field to hold renewal premium at point of MTA quote
        Dim result As Integer = 0
        Dim cRenewal_Premium_I, cRenewal_Premium As Decimal

        'Policy Dates
        Dim dtCoverStartDate_I, dtExpiryDate_I, dtRenewalDate_I, dtCoverStartDate, dtExpiryDate, dtRenewalDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Get Original Insurance File Values
            FromEvent = False
            InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PN38002 new field to hold renewal premium

            If Informations.IsDBNull(RenewalPremium) Or Informations.IsNothing(RenewalPremium) Then
                cRenewal_Premium_I = 0
            Else
                cRenewal_Premium_I = RenewalPremium
            End If

            dtCoverStartDate_I = CoverStartDate
            dtExpiryDate_I = ExpiryDate
            dtRenewalDate_I = RenewalDate



            If Informations.IsDBNull(RenewalPremium) Or Informations.IsNothing(RenewalPremium) Then
                cRenewal_Premium = 0
            Else
                cRenewal_Premium = RenewalPremium
            End If

            dtCoverStartDate = CoverStartDate
            dtExpiryDate = ExpiryDate
            dtRenewalDate = RenewalDate

            RenewalPremium = cRenewal_Premium

            CoverStartDate = dtCoverStartDate_I
            ExpiryDate = dtExpiryDate_I
            RenewalDate = dtRenewalDate_I

            m_lReturn = CType(Update(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add  Event & Policy together

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddFiletoEventSQL, sSQLName:=ACAddFiletoEventName, bStoredProcedure:=ACAddFiletoEventStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'Get Event File Values
            FromEvent = True
            InsuranceFileCnt = v_lEventCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN38002 add new field to hold renewal premium at point of MTA quote

            If Informations.IsDBNull(RenewalPremium) Or Informations.IsNothing(RenewalPremium) Then
                cRenewal_Premium = 0
            Else
                cRenewal_Premium = RenewalPremium
            End If
            RenewalPremium = cRenewal_Premium

            CoverStartDate = dtCoverStartDate
            ExpiryDate = dtExpiryDate
            RenewalDate = dtRenewalDate

            'Update the event
            m_lReturn = CType(Update(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddFileToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFileToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: TakeFileFromEvent (Public)
    '
    ' Description: Subtracts Event values from  Policy ( MTA Credits)
    ' 22082005 Method replaced for Datasure
    ' ***************************************************************** '
    Public Function TakeFileFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Object) As Integer
        'PN38002 add new field to hold renewal premium at point of MTA quote
        Dim result As Integer = 0
        Dim cRenewal_Premium_I, cRenewal_Premium As Decimal

        'Policy Dates
        Dim dtCoverStartDate_I, dtExpiryDate_I, dtRenewalDate_I, dtCoverStartDate, dtExpiryDate, dtRenewalDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Get Original Insurance File Values
            FromEvent = False
            InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN38002 add new field to hold renewal premium at point of MTA quote
            cRenewal_Premium_I = RenewalPremium

            dtCoverStartDate_I = CoverStartDate
            dtExpiryDate_I = ExpiryDate
            dtRenewalDate_I = RenewalDate
            'Get Event File Values
            FromEvent = True
            InsuranceFileCnt = v_lEventCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Event File Values
            FromEvent = True
            InsuranceFileCnt = v_lEventCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN38002 add new field to hold renewal premium at point of MTA quote
            RenewalPremium = cRenewal_Premium

            CoverStartDate = dtCoverStartDate_I
            ExpiryDate = dtExpiryDate_I
            RenewalDate = dtRenewalDate_I


            'Add  Event & Policy together

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTakeFileFromEventSQL, sSQLName:=ACTakeFileFromEventName, bStoredProcedure:=ACTakeFileFromEventStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'Get Event File Values
            FromEvent = True
            InsuranceFileCnt = v_lEventCnt

            m_lReturn = CType(SelectSingle(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PN38002 add new field to hold renewal premium at point of MTA quote
            RenewalPremium = cRenewal_Premium

            CoverStartDate = dtCoverStartDate
            ExpiryDate = dtExpiryDate
            RenewalDate = dtRenewalDate

            'Update the event
            m_lReturn = CType(Update(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TakeFileFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TakeFileFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateCoinsurerToEvent (Public)
    '
    ' Description:  ( MTA Debits)
    '
    ' ***************************************************************** '
    Public Function UpdateCoinsurerToEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add  Event & Policy together

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCoinsurerToEventSQL, sSQLName:=ACUpdateCoinsurerToEventName, bStoredProcedure:=ACUpdateCoinsurerToEventStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCoinsurerToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCoinsurerToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateCoinsurerFormEvent (Public)
    '
    ' Description:  ( MTA Debits)
    '
    ' ***************************************************************** '
    Public Function UpdateCoinsurerFormEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add  Event & Policy together

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCoinsurerFromEventSQL, sSQLName:=ACUpdateCoinsurerFromEventName, bStoredProcedure:=ACUpdateCoinsurerFromEventStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCoinsurerFormEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCoinsurerFormEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CopySystemFromEvent (Public)
    '
    ' Description: Overwrites the Insurance System from the event table.
    '
    ' ***************************************************************** '
    Public Function CopySystemFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToSystemSQL, sSQLName:=ACCopyEventToSystemName, bStoredProcedure:=ACCopyEventToSystemStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopySystemFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopySystemFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyExtrasFromEvent (Public)
    '
    ' Description: Overwrites the Extras from the event table.
    '
    ' ***************************************************************** '
    Public Function CopyExtrasFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First Coinsurers
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToCoinsurersSQL, sSQLName:=ACCopyEventToCoinsurersName, bStoredProcedure:=ACCopyEventToCoinsurersStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Second Fees
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToFeesSQL, sSQLName:=ACCopyEventToFeesName, bStoredProcedure:=ACCopyEventToFeesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'Third shared premiums
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToPremiumsSQL, sSQLName:=ACCopyEventToPremiumsName, bStoredProcedure:=ACCopyEventToPremiumsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'eck210601
            'Policy Agents
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyEventToPolicyAgentsSQL, sSQLName:=ACCopyEventToPolicyAgentsName, bStoredProcedure:=ACCopyEventToPolicyAgentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyExtrasFromEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyExtrasFromEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck030801 New Methods to Delete Editable Event records
    ' ***************************************************************** '
    ' Name: DeleteTransactionEvent (Public)
    '
    ' Description: Deletes record from  Event tables.
    '
    ' ***************************************************************** '
    Public Function DeleteTransactionEvent(ByVal v_lEventCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete events

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="EventCnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACDeleteTransactionEventName, sSQLName:=ACDeleteTransactionEventName, bStoredProcedure:=ACDeleteTransactionEventStored)

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTransactionEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTransactionEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 05/09/99 New Methods to Delete Editable Event records
    ' ***************************************************************** '
    ' Name: DeleteFileEvent (Public)
    '
    ' Description: Deletes record from Insurance File event table.
    '
    ' ***************************************************************** '
    Public Function DeleteFileEvent(ByVal v_lEventCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete events

            sSQL = ""
            sSQL = "DELETE from Event_Insurance_File WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTINSURANCEFILE", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFileEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFileEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteSystemEvent (Public)
    '
    ' Description: Deletes record from Insurance system event table.
    '
    ' ***************************************************************** '
    Public Function DeleteSystemEvent(ByVal v_lEventCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete events

            sSQL = ""
            sSQL = "DELETE from Event_Insurance_File_System WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTINSURANCESYSTEM", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteSystemEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteSystemEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteFolderEvent (Public)
    '
    ' Description: Deletes record from Insurance Folder event table.
    '
    ' ***************************************************************** '
    Public Function DeleteFolderEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete events

            sSQL = ""
            sSQL = "DELETE from Event_Insurance_Folder WHERE " &
                   "insurance_folder_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTINSURANCEFOLDER", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteFolderEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolderEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteExtrasEvent (Public)
    '
    ' Description: Deletes record from Insurance Extras event table.
    '
    ' ***************************************************************** '
    Public Function DeleteExtrasEvent(ByVal v_lEventCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete events

            sSQL = ""
            sSQL = "DELETE from Event_Policy_fee WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTPOLICYFEE", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = ""
            sSQL = "DELETE from Event_Policy_coinsurers WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTPOLICYCOINSURERS", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            sSQL = ""
            sSQL = "DELETE from Event_Policy_shared_premiums WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTPOLICYSHAREDPREMIUMS", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sSQL = ""
            sSQL = "DELETE from Event_policy_narrative WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTPOLICYNARRATIVE", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = ""
            sSQL = "DELETE from Event_Policy_relationship WHERE " &
                   "insurance_file_cnt = " & CStr(v_lEventCnt)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DELETEEVENTPOLICYRELATIONSHIP", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteExtrasEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteExtrasEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


#End Region

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' SJP (CMG)     04/04/2003          PS235
    ' AMB 28-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added
    'JIT    15/02/05: PN- Base Currency id is now being inserted,so added 1 Param
    ' ***************************************************************** '
    Private Function AddInputParam(Optional ByVal vArray(,) As Object = Nothing) As Integer

        Dim nResult As Integer
        Dim sValue As String = ""



        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="insurance_file_structure_id",
                                        vValue:=CStr(InsuranceFileStructureID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insurance_file_type_id", vValue:=CStr(InsuranceFileTypeID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If InsuranceFileStatusID < 1 Then

                m_lReturn = .Parameters.Add(sName:="insurance_file_status_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="insurance_file_status_id",
                                            vValue:=CStr(InsuranceFileStatusID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insurance_file_id", vValue:=CStr(InsuranceFileID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(SourceID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(InsuranceFolderCnt),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insurance_ref", vValue:=InsuranceRef,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="product_id", vValue:=CStr(ProductID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="lead_insurer_cnt", vValue:=LeadInsurerCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If (ToSafeInteger(LeadAgentCnt) = 0) Then
                LeadAgentCnt = Nothing
            End If
            m_lReturn = .Parameters.Add(sName:="lead_agent_cnt", vValue:=LeadAgentCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="lead_agent_percent", vValue:=LeadAgentPercent,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="account_handler_cnt", vValue:=AccountHandlerCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insured_cnt", vValue:=CStr(InsuredCnt),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="business_type_id", vValue:=CStr(BusinessTypeID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If CollectTypeID < 1 Then

                m_lReturn = .Parameters.Add(sName:="collect_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="collect_type_id", vValue:=CStr(CollectTypeID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="collection_from_cnt", vValue:=CollectionFromCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="branch_id", vValue:=CStr(SubBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(CurrencyID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(LanguageID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="date_issued", vValue:=DateIssued,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cover_start_date", vValue:=CoverStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lInsuranceFileTypeID = 7 AndAlso vArray IsNot Nothing AndAlso String.IsNullOrEmpty(vArray(0, 0)) = False Then

                If CDate(vArray(0, 0)) > ExpiryDate Then
                    vArray(0, 0) = ExpiryDate
                End If

                m_lReturn = .Parameters.Add(sName:="expiry_date", vValue:=CDate(vArray(0, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="expiry_date", vValue:=ExpiryDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_date", vValue:=RenewalDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If RenewalMethodID < 1 Then

                m_lReturn = .Parameters.Add(sName:="renewal_method_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="renewal_method_id", vValue:=CStr(RenewalMethodID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_frequency_id", vValue:=CStr(RenewalFrequencyID),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_referred_at_renewal", vValue:=CStr(IsReferredAtRenewal),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If LapsedReasonID < 1 Then

                m_lReturn = .Parameters.Add(sName:="lapsed_reason_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="lapsed_reason_id", vValue:=CStr(LapsedReasonID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="lapsed_date", vValue:=LapsedDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="lapsed_description", vValue:=LapsedDescription,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_referred_on_mta", vValue:=CStr(IsReferredOnMta),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_version", vValue:=CStr(PolicyVersion),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="gemini_policy_status", vValue:=GeminiPolicyStatus,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="gemini_business_type", vValue:=GeminiBusinessType,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="deferred_ind", vValue:=DeferredInd,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_ignore", vValue:=PolicyIgnore,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="broker_cnt", vValue:=BrokerCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=RiskCodeID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="analysis_code_id", vValue:=AnalysisCodeID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_deductibles_id", vValue:=PolicyDeductiblesID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_limits_id", vValue:=PolicyLimitsID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="proposal_date", vValue:=ProposalDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="diary_date", vValue:=DiaryDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="review_date", vValue:=ReviewDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_day_number", vValue:=RenewalDayNumber,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_type_id", vValue:=PolicyTypeID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="indicator", vValue:=Indicator,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="clause", vValue:=CStr(IsReferredOnMta),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cover", vValue:=Cover,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="area", vValue:=Area,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="long_term_undertaking_date", vValue:=LongTermUndertakingDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_stop_code_id", vValue:=RenewalStopCodeID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="vbs_type", vValue:=VBSType,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="vbs_status", vValue:=VBSStatus,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_insurer_rate_table", vValue:=IsInsurerRateTable,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_related_policies", vValue:=IsRelatedPolicies,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_retained_documents", vValue:=IsRetainedDocuments,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="schemes_postcode", vValue:=SchemesPostcode,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="paid_direct", vValue:=PaidDirect,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="scheme", vValue:=Scheme,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="brokerage_amount", vValue:=BrokerageAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_minimum_brokerage_flag", vValue:=IsMinimumBrokerageFlag,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="annual_premium", vValue:=AnnualPremium,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="this_premium", vValue:=ThisPremium,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="net_premium", vValue:=NetPremium,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="commission_amount", vValue:=CommissionAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="iptable_amount", vValue:=IPTAbleAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ipt_percentage", vValue:=IPTPercentage,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_ipt_overridden",
                                        vValue:=CStr(If(gPMFunctions.ToSafeBoolean(ISIPTOverridden), 1, 0)),
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="tax_amount", vValue:=TaxAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="vatable_amount", vValue:=VatableAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="vat_percentage", vValue:=VATPercentage,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="vat_amount", vValue:=VATAmount,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="payment_method", vValue:=PaymentMethod,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="user_defined_data_id", vValue:=UserDefinedDataID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="commission_percentage", vValue:=CommissionPercentage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Object.Equals(InvariantKey, Nothing) Then
                m_lReturn = .Parameters.Add(sName:="invariant_key", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            Else
                m_lReturn = .Parameters.Add(sName:="invariant_key", vValue:=InvariantKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="insured_name", vValue:=InsuredName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="alternate_reference", vValue:=AlternateReference,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="is_client_invoiced", vValue:=IsClientInvoiced,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="old_policy_number", vValue:=OldPolicyNumber,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="quote_expiry_date", vValue:=QuoteExpiryDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = .Parameters.Add(sName:="alternate_account_cnt", vValue:=AlternateAccountCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:=
                                      "getProductOptionValue Failed for Account Executive (" &
                                      gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec & ")", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="AddInputParam")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue.Length = 0 Then
                sValue = CStr(0)
            End If

            If CBool(sValue) Then
                m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=AccountExecutiveCnt,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = .Parameters.Add(sName:="anniversary_date", vValue:=AnniversaryDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If (Informations.IsDBNull(m_vPolicyStyleID) Or Informations.IsNothing(m_vPolicyStyleID)) Then
                m_lReturn = .Parameters.Add(sName:="policy_style_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="policy_style_id", vValue:=CStr(m_vPolicyStyleID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (Informations.IsDBNull(m_vUnderwritingYearID) Or Informations.IsNothing(m_vUnderwritingYearID)) Then
                m_lReturn = .Parameters.Add(sName:="underwriting_year_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="underwriting_year_id", vValue:=CStr(m_vUnderwritingYearID),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Informations.IsDBNull(m_vPolicyStatusID) Or Informations.IsNothing(m_vPolicyStatusID) Then
                m_lReturn = .Parameters.Add(sName:="policy_status_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="policy_status_id", vValue:=m_vPolicyStatusID,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vInceptionTPI) Or Informations.IsNothing(m_vInceptionTPI) Then
                m_lReturn = .Parameters.Add(sName:="inception_date_tpi", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="inception_date_tpi", vValue:=CStr(m_vInceptionTPI),
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(m_vFSACustomerCategoryID) Or Informations.IsDBNull(m_vFSACustomerCategoryID) Then
                m_lReturn = .Parameters.Add(sName:="fsa_customer_category_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="fsa_customer_category_id", vValue:=m_vFSACustomerCategoryID,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vFSAContractLocationID) Or Informations.IsNothing(m_vFSAContractLocationID) Then
                m_lReturn = .Parameters.Add(sName:="fsa_contract_location_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="fsa_contract_location_id", vValue:=m_vFSAContractLocationID,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vFSAUnderwriterCnt) Or Informations.IsNothing(m_vFSAUnderwriterCnt) Then
                m_lReturn = .Parameters.Add(sName:="fsa_underwriter_cnt", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="fsa_underwriter_cnt", vValue:=m_vFSAUnderwriterCnt,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vFSATypeOfSaleID) Or Informations.IsNothing(m_vFSATypeOfSaleID) Then
                m_lReturn = .Parameters.Add(sName:="fsa_type_of_sale_id", vValue:=DBNull.Value,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="fsa_type_of_sale_id", vValue:=m_vFSATypeOfSaleID,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vFSARenewalConsent) Or Informations.IsNothing(m_vFSARenewalConsent) Then
                m_lReturn = .Parameters.Add(sName:="fsa_renewal_consent", vValue:=m_vFSARenewalConsent,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMBoolean)
            Else
                m_lReturn = .Parameters.Add(sName:="fsa_renewal_consent", vValue:=m_vFSARenewalConsent,
                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                            iDataType:=gPMConstants.PMEDataType.PMBoolean)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="base_currency_id", vValue:=m_ibaseCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="country_id", vValue:=m_iCountryId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If DiscountReasonID < 1 Then

                m_lReturn = .Parameters.Add(sName:="discount_reason_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="discount_reason_id", vValue:=DiscountReasonID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="discounted_premium", vValue:=DiscountedPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="discount_percentage", vValue:=DiscountPercentage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="match_discounted_premium_flag", vValue:=MatchDiscountedPremiumFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="put_on_next_instalment_renewal", vValue:=m_lPutOnNextInstalmentRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="anniversary_copy", vValue:=m_lAnniversaryCopy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="discount_recurring_type_id", vValue:=m_lDiscountRecurringTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="lead_allow_consolidated_commission", vValue:=m_iLeadAllowConsolidatedCommission, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Sub_allow_consolidated_commission", vValue:=m_iSubAllowConsolidatedCommission, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="terms_agreed", vValue:=CCTermsAgreed, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="terms_agreed_date", vValue:=CCTermsAgreedDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="inception_date", vValue:=CCInceptionDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_documents_issued_date",
                                        vValue:=CCPolicyDocumentsIssuedDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="policy_documents_correct", vValue:=CCPolicyDocumentCorrect,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="error_notification_date", vValue:=CCErrorNotificationDate,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="risk_transfer_agreement", vValue:=RiskTransferAgreement, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_premium", vValue:=gPMFunctions.ToSafeDouble(m_vRenewalPremium, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="renewal_product_id", vValue:=Convert.ToString(m_vRenewalProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(m_vOriginalProductID) OrElse (CInt(m_vOriginalProductID) = 0) Then
                m_vOriginalProductID = DBNull.Value
            End If
            m_lReturn = .Parameters.Add(sName:="original_product_id", vValue:=m_vOriginalProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="risk_transfer_editable", vValue:=RiskTransferEditable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="manual_discount_percentage", vValue:=ManualDiscountPercentage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="quote_status_id", vValue:=QuoteStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="quote_version", vValue:=QuoteVersionID,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="base_insurance_folder_cnt", vValue:=BaseInsuranceFolderCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Contact_user_id", vValue:=ContactuserId,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="coins_placement", vValue:=CoInsPlacement,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="MTA_reason_id", vValue:=MTAReasonId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="bIs_Marketplace_Policy", vValue:=IsMarketPlacePolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="nDOPaymentTerms_id", vValue:=PaymentTermsID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="nCollectionFrequency_id", vValue:=CollectionFrequencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Correspondence_Type", vValue:=CorrespondenceType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="Default_Preferred_Correspondence", vValue:=DefaultPreferredCorrespondence, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="Is_Agent_Correspondence", vValue:=IsAgentCorrespondence, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = .Parameters.Add(sName:="sender_email", vValue:=SenderEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="receiver_email", vValue:=ReceiverEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
			
			  m_lReturn = .Parameters.Add(sName:="original_insurance_file_type_id", vValue:=Convert.ToString(OriginalInsuranceFileTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(InsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            InsuranceFileCnt = .Parameters.Item("insurance_file_cnt").Value

        End With

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' History: 30/11/00 created by CT based on function in dsirparty created by Tomo
    '
    ' ***************************************************************** '
    Private Function UpdateFileMaster(ByRef lMode As Integer) As Integer

        Dim result As Integer = 0
        Dim iOptionNumber As Integer
        Dim bDocumasterEnabled As Boolean
        Dim sOptionValue, sFolderDesc As String 'DN 02/04/01
        Dim sPartyName As String = String.Empty
        'DJM 12/04/2002 : Don't need follow variable as desc now a property.
        'Dim sPolicyDesc As String  'DN 02/04/01
        Dim lInsuranceFolderCnt As Integer 'DN 03/07/01
        Dim oArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        iOptionNumber = 10
        sOptionValue = ""

        m_lReturn = CType(GetOption(r_iOptionNumber:=iOptionNumber, r_sOptionValue:=sOptionValue), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        'TF160802 - Fix to use 2nd dimension

        If sOptionValue = "1" Then
            bDocumasterEnabled = True
        End If

        'Not set up - do nothing
        If Not (bDocumasterEnabled) Then
            Return result
        End If



        m_lReturn = CType(GetDocumasterPartyName(sPartyName), gPMConstants.PMEReturnCode)

        'DN 16/08/01
        If TransInsuranceFolderCnt = 0 Then
            lInsuranceFolderCnt = InsuranceFolderCnt
        Else
            lInsuranceFolderCnt = TransInsuranceFolderCnt
        End If

        'DJM 12/04/2002 : Get description from property now.
        ''DN 02/04/01 - Use InsFolderCnt as external code plus pass Policy Desc with Pol. No.
        'm_lReturn = GetPolicyDesc(lInsuranceFolderCnt, sPolicyDesc) 'DN 16/08/01
        sFolderDesc = InsuranceRef.Trim()

        'NEW CODE ADDED DUE TO PREVENT THE ERROR IF FOLDER DESC'S LENGTH IS LESS THEN 70.
        'sFolderDesc = sFolderDesc.Substring(0, 70) 'PN16037
        If sFolderDesc.Length > 70 Then
            sFolderDesc = sFolderDesc.Substring(0, 70) 'PN16037
        End If

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="sPolicyfolderCnt", vValue:=lInsuranceFolderCnt.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="sPartyCnt", vValue:=InsuredCnt.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="sSourceId", vValue:=SourceID.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="sFolderDesc", vValue:=sFolderDesc.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(
                                sSQL:=ACCheckDMEFolderSQL,
                                sSQLName:=ACCheckDMEFolderName,
                                bStoredProcedure:=ACCheckDMEFolderStored,
                                vResultArray:=oArray)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername,
                iType:=gPMConstants.PMELogLevel.PMLogError,
                sMsg:="Failed to Check DME Folder",
                vApp:=ACApp,
                vClass:=ACClass,
                vMethod:="UpdateFileMaster",
                vErrNo:=Informations.Err().Number,
                vErrDesc:=Informations.Err().Description, excep:=Nothing)
            Return result
        ElseIf Informations.IsArray(oArray) Then
            ' if folder exists for source\party\folder desc combination
            If ToSafeInteger(oArray(0, 0)) > 0 Then
                'skip updating DME
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
        End If

        If m_oSIRDOCAPI Is Nothing Then
            m_oSIRDOCAPI = New bSIRDOCAPI.Form()
            'eck010201 Pass Populated Language Id property
            m_lReturn = m_oSIRDOCAPI.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=SourceID, iLanguageID:=LanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", excep:=Nothing)

                Return result
            End If
        End If
        m_lReturn = m_oSIRDOCAPI.ProcessIndex(lMode:=lMode, iSourceID:=SourceID, lPartyId:=InsuredCnt, sPartyName:=sPartyName, lInsuranceFolderId:=lInsuranceFolderCnt, sInsuranceFileRef:=sFolderDesc, lClaimId:=0, sClaimRef:="")

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process index via DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' History: 30/11/00 created by CT based on function in dsirparty created by Tomo
    ' ***************************************************************** '
    Private Function GetOption(ByRef r_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer

        Dim result As Integer = 0
        Dim sOptionValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSystemOption Is Nothing Then
            m_oSystemOption = New bSIROptions.Business()


            m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=r_iOptionNumber, sValue:=sOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If
        r_sOptionValue = sOptionValue
        Return result

    End Function



    ' ***************************************************************** '
    ' Name: GetDocumasterPartyName (Private)
    '
    ' Description: Get an option.
    '
    ' History: 30/11/00 created by CT to get partyname for use in documaster
    ' ***************************************************************** '
    Private Function GetDocumasterPartyName(ByRef sPartyName As String) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(InsuredCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyShortNameSQL, sSQLName:=ACGetPartyShortNameName, bStoredProcedure:=ACGetPartyShortNameStored, lNumberRecords:=0, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vArray) Then
            ' No Records, return PMFalse
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        sPartyName = CStr(vArray(0, 0)).Trim()

        Return result

    End Function

    'MKW200503 PN4090 1.6.9 --> 1.8.6 Catchup
    'DJM 14/05/2003 : Re-added
    ' ***************************************************************** '
    ' Name: GetPolicyDesc (Private)
    '
    ' Description: Get Policy Description
    '
    ' History: 'DN 16/08/01 get policy Desc for use in documaster
    ' ***************************************************************** '
    Private Function GetPolicyDesc(ByRef lInsuranceFolderCnt As Integer, ByRef sPolicyDesc As String) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDescSQL, sSQLName:=ACGetPolicyDescName, bStoredProcedure:=ACGetPolicyDescStored, lNumberRecords:=0, vResultArray:=vArray) '

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vArray) Then
            ' No Records, return PMFalse
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        sPolicyDesc = CStr(vArray(0, 0)).Trim()

        Return result

    End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetContractCertainty (Public)
    ' Description: Get contract certainty details from the database.
    ' ***************************************************************** '
    Public Function GetContractCertainty(ByRef v_lInsuranceFileCnt As Integer, ByRef v_bFromEvent As Boolean, ByRef r_vArray(,) As Object) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="from_event", vValue:=CStr(v_bFromEvent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetContractCertaintySQL, sSQLName:=ACGetContractCertaintyName, bStoredProcedure:=ACGetContractCertaintyStored, lNumberRecords:=0, vResultArray:=r_vArray) '

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get contract certainty details failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContractCertainty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateContractCertainty (Public)
    ' Description: Update the contract certainty details.
    ' ***************************************************************** '
    Public Function UpdateContractCertainty(ByRef v_lInsuranceFileCnt As Integer, ByRef v_bFromEvent As Boolean, ByRef v_vTermsAgreed As Object, ByRef v_vTermsAgreedDate As Object, ByRef v_vInceptionDate As Object, ByRef v_vPolicyDocumentsIssuedDate As Object, ByRef v_vPolicyDocumentsCorrect As Object, ByRef v_vErrorNotificationDate As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="from_event", vValue:=CStr(v_bFromEvent), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="terms_agreed", vValue:=CStr(v_vTermsAgreed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsDate(v_vTermsAgreedDate) Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="terms_agreed_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else


                m_lReturn = m_oDatabase.Parameters.Add(sName:="terms_agreed_date", vValue:=CStr(v_vTermsAgreedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsDate(v_vInceptionDate) Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_Date", vValue:=CStr(v_vInceptionDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsDate(v_vPolicyDocumentsIssuedDate) Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_documents_issued_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_documents_issued_date", vValue:=CStr(v_vPolicyDocumentsIssuedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_documents_correct", vValue:=CStr(v_vPolicyDocumentsCorrect), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsDate(v_vErrorNotificationDate) Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="error_notification_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="error_notification_date", vValue:=CStr(v_vErrorNotificationDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateContractCertaintySQL, sSQLName:=ACUpdateContractCertaintyName, bStoredProcedure:=ACUpdateContractCertaintyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Updat contract certainty details failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContractCertainty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function RecalculateFeesAfterEvent(ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRecalculatePolicyFeeSQL, sSQLName:=ACRecalculatePolicyFeeName, bStoredProcedure:=ACRecalculatePolicyFeeStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculateFeesAfterEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateFeesAfterEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function RecalculateAgentsAfterEvent(ByVal v_lInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRecalculatePolicyAgentsSQL, sSQLName:=ACRecalculatePolicyAgentsName, bStoredProcedure:=ACRecalculatePolicyAgentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculateAgentsAfterEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateAgentsAfterEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class