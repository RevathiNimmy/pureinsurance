Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("bSIRPremFinConst_NET.bSIRPremFinConst")>
Public Module bSIRPremFinConst


    '================
    'PUBLIC CONSTANTS
    '================
    ' ***************************************************************** '
    ' MainArray definition
    ' Used by the following Stored Procedures(minimum list):
    '   spu_PFPremiumFinance_addnew
    '   spu_PFPremiumFinance_addnewversion
    '   spu_PFPremiumFinance_sel_latest_valid_plan
    '   spu_PFPremiumFinance_sel_single
    '   spu_PFPremiumFinance_sel_single_rec
    '   spu_PFPremiumFinance_sel_SingleFromInsuranceFile
    '   spu_PFPremiumFinance_update
    '
    ' Please ensure changes to recordsets or constants are updated correctly
    ' ***************************************************************** '
    Public Const k_PFPlanPFCnt As Integer = 0
    Public Const k_PFPlanPFVersion As Integer = 1
    Public Const k_PFPlanCompanyNo As Integer = 2
    Public Const k_PFPlanCompanyName As Integer = 3
    Public Const k_PFPlanSchemeNo As Integer = 4
    Public Const k_PFPlanSchemeName As Integer = 5
    Public Const k_PFPlanSchemeVersion As Integer = 6
    Public Const k_PFPlanStartDate As Integer = 7
    Public Const k_PFPlanEndDate As Integer = 8
    Public Const k_PFPlanProductClass As Integer = 9
    Public Const k_PFPlanTransactionType As Integer = 10
    Public Const k_PFPlanAmountToFinance As Integer = 11
    Public Const k_PFPlanAPR As Integer = 12
    Public Const k_PFPlanInterestRate As Integer = 13
    Public Const k_PFPlanDaysDelay As Integer = 14
    Public Const k_PFPlanNoOfInstalments As Integer = 15
    Public Const k_PFPlanFirstInstalment As Integer = 16
    Public Const k_PFPlanOtherInstalments As Integer = 17
    Public Const k_PFPlanCostOfProtection As Integer = 18
    Public Const k_PFPlanDeposit As Integer = 19
    Public Const k_PFPlanNetAmount As Integer = 20
    Public Const k_PFPlanTotalCost As Integer = 21
    Public Const k_PFPlanInterestCost As Integer = 22
    Public Const k_PFPlanMinFinanceCharge As Integer = 23
    Public Const k_PFPlanPayProtection As Integer = 24
    Public Const k_PFPlanQuoteDocID As Integer = 25
    Public Const k_PFPlanBankDocID As Integer = 26
    Public Const k_PFPlanCreditDocID As Integer = 27
    Public Const k_PFPlanClientName As Integer = 28
    Public Const k_PFPlanClientAddress1 As Integer = 29
    Public Const k_PFPlanClientAddress2 As Integer = 30
    Public Const k_PFPlanClientAddress3 As Integer = 31
    Public Const k_PFPlanClientTown As Integer = 32
    Public Const k_PFPlanClientAddress4 As Integer = 33
    Public Const k_PFPlanClientPostcode As Integer = 34
    Public Const k_PFPlanClientCountry As Integer = 35
    Public Const k_PFPlanClientAreaCode As Integer = 36
    Public Const k_PFPlanClientPhone As Integer = 37
    Public Const k_PFPlanClientExtn As Integer = 38
    Public Const k_PFPlanClientFaxCode As Integer = 39
    Public Const k_PFPlanClientFax As Integer = 40
    Public Const k_PFPlanBankName As Integer = 41
    Public Const k_PFPlanBankSortCode As Integer = 42
    Public Const k_PFPlanBankAccountNo As Integer = 43
    Public Const k_PFPlanBankAccountName As Integer = 44
    Public Const k_PFPlanBankBranch As Integer = 45
    Public Const k_PFPlanBankAddress1 As Integer = 46
    Public Const k_PFPlanBankAddress2 As Integer = 47
    Public Const k_PFPlanBankAddress3 As Integer = 48
    Public Const k_PFPlanBankTown As Integer = 49
    Public Const k_PFPlanBankAddress4 As Integer = 50
    Public Const k_PFPlanBankPostcode As Integer = 51
    Public Const k_PFPlanBankCountry As Integer = 52
    Public Const k_PFPlanBankAreaCode As Integer = 53
    Public Const k_PFPlanBankPhone As Integer = 54
    Public Const k_PFPlanBankExtn As Integer = 55
    Public Const k_PFPlanBankFaxCode As Integer = 56
    Public Const k_PFPlanBankFax As Integer = 57
    Public Const k_PFPlanBrokerName As Integer = 58
    Public Const k_PFPlanBrokerAddress1 As Integer = 59
    Public Const k_PFPlanBrokerAddress2 As Integer = 60
    Public Const k_PFPlanBrokerAddress3 As Integer = 61
    Public Const k_PFPlanBrokerPostcode As Integer = 62
    Public Const k_PFPlanBrokerAddress4 As Integer = 63
    Public Const k_PFPlanStatusInd As Integer = 64
    Public Const k_PFPlanNBInd As Integer = 65
    Public Const k_PFPlanMTAInd As Integer = 66
    Public Const k_PFPlanRNWLInd As Integer = 67
    Public Const k_PFPlanCNCLInd As Integer = 68
    Public Const k_PFPlanPPInd As Integer = 69
    Public Const k_PFPlanClientId As Integer = 70
    Public Const k_PFPlanClientCode As Integer = 71
    Public Const k_PFPlanSystemTag As Integer = 72
    Public Const k_PFPlanAutoGenPlanRef As Integer = 73
    Public Const k_PFPlanFinCollPlanRef As Integer = 74
    Public Const k_PFPlanPolicyCnt As Integer = 75
    Public Const k_PFPlanInterestFree As Integer = 76
    Public Const k_PFPlanIsQuote As Integer = 77
    Public Const k_PFPlanIsParentPlan As Integer = 78
    Public Const k_PFPlanParentPlanCnt As Integer = 79
    Public Const k_PFPlanParentPlanVersion As Integer = 80
    Public Const k_PFPlanPlanTransactionID As Integer = 81
    Public Const k_PFPlanTransactionCount As Integer = 82
    Public Const k_PFPlanImmediateBankDetails As Integer = 83
    Public Const k_PFPlanInsuranceFileCnt As Integer = 84
    Public Const k_PFPlanPFRF_ID As Integer = 85
    Public Const k_PFPlanPFRF_Code As Integer = 86
    Public Const k_PFPlanMediaType_ID As Integer = 87
    Public Const k_PFPlanSchemeType_ID As Integer = 88
    Public Const k_PFPlanBankCountry_ID As Integer = 89
    Public Const k_PFPlanClientCountry_ID As Integer = 90
    Public Const k_PFPlanFirstInstalmentdate As Integer = 91
    Public Const k_PFPlanNextInstalmentdate As Integer = 92
    Public Const k_PFPlanLastInstalmentdate As Integer = 93
    Public Const k_PFPlanTaxCost As Integer = 94
    Public Const k_PFPlanMediaTypeCode As Integer = 95
    Public Const k_PFPlanCCNumber As Integer = 96
    Public Const k_PFPlanCCExpiryDate As Integer = 97
    Public Const k_PFPlanCCStartDate As Integer = 98
    Public Const k_PFPlanCCIssue As Integer = 99
    Public Const k_PFPlanCCPin As Integer = 100
    Public Const k_PFPlanMediaTypeValidation As Integer = 101
    Public Const k_PFPlanBankNameMandatory As Integer = 102
    Public Const k_PFPlanBankAddressMandatory As Integer = 103
    Public Const k_PFPlanBranchNameMandatory As Integer = 104
    Public Const k_PFPlanBranchCodeMandatory As Integer = 105
    Public Const k_PFPlanCommissionTransDetail As Integer = 106
    Public Const k_PFPlanFrequencyPeriod As Integer = 107
    Public Const k_PFPlanFrequencyAmount As Integer = 108
    Public Const k_PFPlanPfFrequency_ID As Integer = 109
    Public Const k_PFPlanSource_ID As Integer = 110
    Public Const k_PFPlanProduct_ID As Integer = 111
    Public Const k_PFPlanSchemeTypeCode As Integer = 112
    Public Const k_PFPlanFinanceCharge As Integer = 113
    Public Const k_PFPlanDayOfWeekOrMonth As Integer = 114
    Public Const k_PFPlanPaymentMethod As Integer = 115
    Public Const k_PFPlanFrequency As Integer = 116
    Public Const k_PFPlanSchemePrintType As Integer = 117
    Public Const k_PFPlanOriginalAmount As Integer = 118
    Public Const k_PFPlanLastInstalment As Integer = 119
    Public Const k_PFPlanClaimDebtID As Integer = 120
    Public Const k_PFPlanUserID As Integer = 121
    Public Const k_PFPlanAgentCnt As Integer = 122
    Public Const k_PFPlanAgentRef As Integer = 123
    Public Const k_PFPlanDateCreated As Integer = 124
    Public Const k_PFPlanDateModified As Integer = 125
    Public Const k_PFPlanDateConfirmed As Integer = 126
    Public Const k_PFPlanDateReview As Integer = 127
    Public Const k_PFPlanDateLastStatement As Integer = 128
    Public Const k_PFPlanDateLastGeneration As Integer = 129
    Public Const k_PFPlanFinalStatementSet As Integer = 130
    Public Const k_PFPlanNoStatements As Integer = 131
    Public Const k_PFPlanStatementPFFrequencyID As Integer = 132
    Public Const k_PFPlanReviewPMWorkTaskInstance As Integer = 133
    Public Const k_PFPlanMediaTypeIsViaThirdParty As Integer = 134
    Public Const k_PFPlanReviewPMuserGroupID As Integer = 135
    Public Const k_PFPlanConfirmationDocID As Integer = 136
    Public Const k_PFPlanCommissionSpread As Integer = 137
    Public Const k_PFPlanTaxSpread As Integer = 138
    Public Const k_PFPlanRISpread As Integer = 139
    Public Const k_PFPlanDepositAsInstalment As Integer = 140
    Public Const k_PFPlanAuthCode As Integer = 141
    Public Const k_PFPlanClientPartyType As Integer = 142 'MKW150604 PN12507
    Public Const k_PFPlanBusinessCode As Integer = 143 'PN12594
    Public Const k_PFPlanPFRFMnemonic As Integer = 144 'PN13915
    Public Const k_PFPlanURL As Integer = 145
    Public Const k_PFPlanTimeout As Integer = 146
    Public Const k_PFPlanUsername As Integer = 147
    Public Const k_PFPlanPassword As Integer = 148
    Public Const k_PFPlanBrokerID As Integer = 149
    Public Const k_PFPlanCoverStartDate As Integer = 150
    Public Const k_PFPlanCoverEndDate As Integer = 151
    Public Const k_PFPlanTerms As Integer = 152
    Public Const k_PFPlanReference As Integer = 153
    Public Const k_PFPlanDocURL As Integer = 154
    Public Const k_PFPlanOriginalRate As Integer = 155
    Public Const k_PFPlanRefundType As Integer = 156
    Public Const k_PFPlanLimitedCompany As Integer = 157
    Public Const k_PFPlanIsCardholder As Integer = 158
    Public Const k_PfPlanCardholderName As Integer = 159
    Public Const k_PfPlanCardholderAddress1 As Integer = 160
    Public Const k_PfPlanCardholderAddress2 As Integer = 161
    Public Const k_PfPlanCardholderAddress3 As Integer = 162
    Public Const k_PfPlanCardholderAddress4 As Integer = 163
    Public Const k_PfPlanCardholderPostcode As Integer = 164
    Public Const k_PFPlanCardType As Integer = 165
    Public Const k_PFPlanProviderCollectDeposit As Integer = 166
    Public Const k_PfPlanDateBankDetailsChanged As Integer = 167
    Public Const k_PfPlanTaxGroupID As Integer = 168
    Public Const k_PfPlanInstAlign_to As Integer = 169 '0 for Align Instalment by Renewal date and 1 for Customer Pref.
    Public Const k_PFPlanSubBranchID As Integer = 170
    Public Const k_PFPlanBatchID As Integer = 171 '(RC) PLICO 9-10
    Public Const k_PFPlanDDCancelled As Integer = 172
    Public Const k_PFPlanCCCancelled As Integer = 173
    Public Const k_PFPlanPaperDD As Integer = 174

    Public Const k_PFPlanXSLCode As Integer = 171
    Public Const k_PFPlanSGSchemeType As Integer = 172
    Public Const k_PFPlanSGSchemeCode As Integer = 173

    'Party Bank Details
    Public Const k_PFPlanBankPaymentTypeId As Integer = 175
    Public Const k_PFPlanPartyBankIdSel As Integer = 178
    Public Const k_PFCancelReasonId As Integer = 179
    Public Const k_PFIsCancelPolicyRun As Integer = 180

    Public Const kFinanceNetCommission As Integer = 181
    Public Const kBIC As Integer = 182
    Public Const kIBAN As Integer = 183
    Public Const kBICMandatory As Integer = 184
    Public Const kIBANMandatory As Integer = 185


    Public Const kUseTransactionCurrency As Integer = 186

    Public Const k_PfPlanCardholderCountryID As Integer = 187

    Public Const k_PFPlanSubAgentCommissionSpread As Integer = 188
    Public Const k_PFPlanCurrBaseXRate As Integer = 189

    Public Const k_PFPlanCountOfFields As Integer = k_PFPlanCurrBaseXRate

    Public Const k_PFPlanClientCreditCardName As Integer = 0
    Public Const k_PFPlanClientcc_number As Integer = 1
    Public Const k_PFPlanClientcc_expiry_date As Integer = 2
    Public Const k_PFPlanClientcc_start_date As Integer = 3
    Public Const k_PFPlanClientcc_issue As Integer = 4
    Public Const k_PFPlanClientcc_pin As Integer = 5

    Public Const k_PFPlanClientBankName As Integer = 0
    Public Const k_PFPlanClientBankSortCode As Integer = 1
    Public Const k_PFPlanClientBankAccountNo As Integer = 2
    Public Const k_PFPlanClientBankAccountName As Integer = 3
    Public Const k_PFPlanClientBankBranch As Integer = 4
    Public Const k_PFPlanClientBankAddress1 As Integer = 5
    Public Const k_PFPlanClientBankAddress2 As Integer = 6
    Public Const k_PFPlanClientBankAddress3 As Integer = 7
    Public Const k_PFPlanClientBankTown As Integer = 8
    Public Const k_PFPlanClientBankAddress4 As Integer = 9
    Public Const k_PFPlanClientBankPostCode As Integer = 10
    Public Const k_PFPlanClientBankCountry_ID As Integer = 11
    Public Const k_PFPlanClientBankAreaCode As Integer = 12
    Public Const k_PFPlanClientBankPhone As Integer = 13
    Public Const k_PFPlanClientBankExtn As Integer = 14
    Public Const k_PFPlanClientBankFaxCode As Integer = 15
    Public Const k_PFPlanClientBankFax As Integer = 16

    'Party Bank Details
    Public Const k_PFPlanClientBankPaymentTypeId As Integer = 17

    Public Const k_PFPlanNoFinanceRate As Integer = 9999
    Public Const k_PFPlanInvalidParty As Integer = 9998

    ' ***************************************************************** '
    ' Scheme Array definition
    ' Used by the following Stored Procedures(minimum list):

    Public Const k_PFSchemeCompanyNo As Integer = 0
    Public Const k_PFSchemeSchemeNo As Integer = 1
    Public Const k_PFSchemeSchemeVersion As Integer = 2
    Public Const k_PFSchemePartyCnt As Integer = 3
    Public Const k_PFSchemeDataModelCode As Integer = 4
    Public Const k_PFSchemeStartDate As Integer = 5
    Public Const k_PFSchemeEndDate As Integer = 6
    Public Const k_PFSchemePaymentMethod As Integer = 7
    Public Const k_PFSchemeSystemTag As Integer = 8
    Public Const k_PFSchemeSchemeName As Integer = 9
    Public Const k_PFSchemeSchemeDescription As Integer = 10
    Public Const k_PFSchemeQuoteableInd As Integer = 11
    Public Const k_PFSchemeQuoteDocID As Integer = 12
    Public Const k_PFSchemeBankDocID As Integer = 13
    Public Const k_PFSchemeCreditDocID As Integer = 14
    Public Const k_PFSchemeInsrMailboxNo As Integer = 15
    Public Const k_PFSchemeEdiMessageCount As Integer = 16
    Public Const k_PFSchemeImmediateBankDetails As Integer = 17
    Public Const k_PFSchemePartyCode As Integer = 18
    Public Const k_PFSchemePartyName As Integer = 19
    Public Const k_PFSchemeMediaTypeID As Integer = 20
    Public Const k_PFSchemeCurrencyID As Integer = 21
    Public Const k_PFSchemePrintTypeID As Integer = 22
    Public Const k_PFSchemeSpreadCommission As Integer = 23
    Public Const k_PFSchemeSuspenseAccountID As Integer = 24
    Public Const k_PFSchemeInterestAccountID As Integer = 25
    Public Const k_PFSchemeAdminAccountID As Integer = 26
    Public Const k_PFSchemeProtectionAccountID As Integer = 27
    Public Const k_PFSchemeTaxGroupID As Integer = 28
    Public Const k_PFSchemeTaxSuspenseAccountID As Integer = 29
    Public Const k_PFSchemeCommissionSuspenseAccountID As Integer = 30
    Public Const k_PFSchemePFSchemeTypeID As Integer = 31
    Public Const k_PFSchemeBankNameMandatory As Integer = 32
    Public Const k_PFSchemeBankAddressMandatory As Integer = 33
    Public Const k_PFSchemeBranchNameMandatory As Integer = 34
    Public Const k_PFSchemeBranchCodeMandatory As Integer = 35
    Public Const k_PFSchemeBankAccountID As Integer = 36
    Public Const k_PFSchemeDocConfirmationID As Integer = 37
    Public Const k_PFSchemeReInsuranceSuspenseAccount As Integer = 38
    Public Const k_PFSchemeSpreadReInsurance As Integer = 39
    Public Const k_PFSchemeSpreadTaxes As Integer = 40
    Public Const k_PFSchemeDepositAsInstalment As Integer = 41
    Public Const k_PFSchemeDepositOnOtherMediaType As Integer = 42
    Public Const k_PFSchemeAgentRefMandatory As Integer = 43
    Public Const k_PFSchemePFMessage As Integer = 44
    Public Const k_PFSchemeBusinessCodeMandatory As Integer = 45
    Public Const k_PFSchemeReceiptDifference As Integer = 46
    Public Const k_PFSchemeProviderWebsite As Integer = 47
    Public Const k_PFSchemePLBrokerID As Integer = 48
    Public Const k_PFSchemeCLBrokerID As Integer = 49
    Public Const k_PFSchemeProviderUsername As Integer = 50
    Public Const k_PFSchemeProviderPassword As Integer = 51
    Public Const k_PFSchemeProviderTimeout As Integer = 52
    Public Const k_PFSchemeProviderBrokerID As Integer = 53
    Public Const k_PFSchemeFinancialInstitutionCode As Integer = 54
    Public Const k_PFSchemeDirectDebitSupplierName As Integer = 55
    Public Const k_PFSchemeDirectDebitSupplierID As Integer = 56
    Public Const k_PFSchemeRemitter As Integer = 57
    Public Const k_PFSchemeProcessingDays As Integer = 58
    Public Const k_PFSchemeAllowClientFees As Integer = 59 'PF Client Fee
    Public Const k_PFSchemeRatesForInformationOnly As Integer = 60 '(RC) PLICO 9-10
    Public Const k_PFSchemeProviderPremThreshold As Integer = 60 'Premium Threshold


    Public Const k_PFSchemeCollectionNotificationDocID As Integer = 61
    Public Const k_PFSchemeCollectionNotificationDays As Integer = 62
    Public Const k_PFSchemePlanRefEditable As Integer = 63
    Public Const kPFSchemeBIC As Integer = 64
    Public Const kPFSchemeIBAN As Integer = 65
    Public Const k_PFSchemeSubAgentSpreadCommission As Integer = 66
    Public Const k_PFSchemeSubAgentCommissionSuspenseAccountID As Integer = 67

    'Should point to the last constant..
    Public Const k_PFSchemeCountOfFields As Integer = k_PFSchemeSubAgentCommissionSuspenseAccountID

    ' Lookup array
    Public Const PFLookupID As Integer = 0
    Public Const PFLookupCode As Integer = 1
    Public Const PFLookupDescription As Integer = 2

    ' Instalment array
    Public Const PFInstFinanceCnt As Integer = 0
    Public Const PFInstFinanceVersion As Integer = 1
    Public Const PFInstInstalmentNumber As Integer = 2
    Public Const PFInstDueDate As Integer = 3
    Public Const PFInstFee As Integer = 4
    Public Const PFInstAmount As Integer = 5
    Public Const PFInstTransactionCode As Integer = 6
    Public Const PFInstStatus As Integer = 7
    Public Const PFInstBatchNumber As Integer = 8
    Public Const PFInstBatchExportDate As Integer = 9
    Public Const PFInstPostedDate As Integer = 10
    Public Const PFInstInstalmentCount As Integer = 11
    Public Const PFInstInstalmentsProcessed As Integer = 12
    Public Const PFInstTransactionID As Integer = 13
    Public Const PFInstTax As Integer = 14
    Public Const PFInstComm As Integer = 15
    Public Const PFInstResultID As Integer = 16
    Public Const PFInstBatchID As Integer = 17
    Public Const PFInstReason As Integer = 18
    Public Const PFInstId As Integer = 19
    Public Const PFInstTransactionDescription As Integer = 20
    Public Const PFInstStatusDescription As Integer = 21
    Public Const PFInstCurrencyISOCode As Integer = 22
    Public Const PFInstCurrencyID As Integer = 23
    Public Const PFInstStatusCode As Integer = 24
    Public Const kPFInstWriteOffReason As Integer = 21
    Public Const kPFInstWriteOffReasonID As Integer = 23
    Public Const kPFInstWriteOffReasonDescription As Integer = 24

    ' Status Indicator Definitions
    Public Const PFStatusIndDeleted As String = "000"
    Public Const PFStatusIndSaved As String = "010"
    Public Const PFStatusIndUpdated As String = "011"
    Public Const PFStatusIndQuotePrinted As String = "012"
    Public Const PFStatusIndLive As String = "040"
    Public Const PFStatusIndOnHold As String = "140"
    Public Const PFStatusIndCompleted As String = "900"
    Public Const PFStatusIndSuperseded As String = "990"
    Public Const PFStatusIndCancelled As String = "999"

    ' Status Constants (moved from gPFConst, required in plan maintenance)
    '(used anywhere????)
    Public Const PFStatusNew As Integer = 1
    Public Const PFStatusPending As Integer = 2
    Public Const PFStatusCollected As Integer = 3
    Public Const PFStatusManual As Integer = 4
    Public Const PFStatusRetrying As Integer = 5
    Public Const PFStatusFailed As Integer = 6
    Public Const PFStatusHold As Integer = 7
    Public Const PFStatusWriteOff As Integer = 8
    Public Const PFStatusTransferred As Integer = 9
    'PN67437
    Public Const PFStatusChargeback As Integer = 10

    ' Delay required for setting up direct debit (10 clear days = 11)
    '(used anywhere????)
    Public Const PFDirectDebitDelay As Integer = 11

    Public Const PFThirdParty As Integer = 1
    Public Const PFInHouse As Integer = 2
    Public Const PFThirdPartyViaStargate As Integer = 3
    Public Const PFThirdPartyRecovery As Integer = 4

    'TR - BackDatedMTATypes
    Public Const PFNotBDMTA As Integer = 0
    Public Const PFIntermediateBDMTA As Integer = 1
    Public Const PFLastBDMTA As Integer = 2

    ' RAW 05/11/2003 : CQ2976 : added
    Public Const m_klInstalmentMTAType_AddAndSpread As Integer = 0
    Public Const m_klInstalmentMTAType_AddToNext As Integer = 1
    Public Const m_klInstalmentMTAType_AddToNewPlan As Integer = 2
    Public Const m_klInstalmentMTAType_NoAmountChange As Integer = 3
    Public Const m_klInstalmentMTAType_SGNoProcessTransact As Integer = 1000
    ' RAW 05/11/2003 : CQ2976 : end

    '================
    'PUBLIC VARIABLES
    '================
    Public m_sClientRef As String = "" '(used anywhere?)

    'Constant for whether you can select multiple rows in the plan selection
    Public m_bAllowMultiPlanSelect As Boolean


    'ACR060606 Added for SG XML list types
    Public Const ACSGListInsurers As Integer = 0
    Public Const ACSGListBusiness As Integer = 1

    Public Const ACSGListInsurersText As String = "InsurerList.xml"
    Public Const ACSGListBusinessText As String = "BusinessList.xml"
End Module