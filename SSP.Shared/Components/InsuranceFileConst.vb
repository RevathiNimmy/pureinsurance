Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("InsuranceFileConst_NET.InsuranceFileConst")>
Public Module InsuranceFileConst

    ' Constant used to Dimension Field Array
    Public Const ACFieldArraySize As Integer = 137

    ' Field Array positional constants
    Public Const ACInsuranceFileCnt As Integer = 0
    Public Const ACInsuranceFileStructureID As Integer = 1
    Public Const ACInsuranceFileTypeID As Integer = 2
    Public Const ACInsuranceFileStatusID As Integer = 3
    Public Const ACInsuranceFileID As Integer = 4
    Public Const ACSourceID As Integer = 5
    Public Const ACInsuranceFolderCnt As Integer = 6
    Public Const ACInsuranceRef As Integer = 7
    Public Const ACProductID As Integer = 8
    Public Const ACLeadInsurerCnt As Integer = 9
    Public Const ACLeadAgentCnt As Integer = 10
    Public Const ACLeadAgentPercent As Integer = 11
    Public Const ACAccountHandlerCnt As Integer = 12
    Public Const ACInsuredCnt As Integer = 13
    Public Const ACBusinessTypeID As Integer = 14
    Public Const ACCollectTypeID As Integer = 15
    Public Const ACCollectionFromCnt As Integer = 16
    'sj 19/07/2002 - start
    'Public Const ACBranchID = 17
    Public Const ACSubBranchID As Integer = 17
    'sj 19/07/2002 - end
    Public Const ACCurrencyID As Integer = 18
    Public Const ACLanguageID As Integer = 19
    Public Const ACDateIssued As Integer = 20
    Public Const ACCoverStartDate As Integer = 21
    Public Const ACExpiryDate As Integer = 22
    Public Const ACRenewalDate As Integer = 23
    Public Const ACRenewalMethodID As Integer = 24
    Public Const ACRenewalFrequencyID As Integer = 25
    Public Const ACIsReferredAtRenewal As Integer = 26
    Public Const ACLapsedReasonID As Integer = 27
    Public Const ACLapsedDate As Integer = 28
    Public Const ACLapsedDescription As Integer = 29
    Public Const ACIsReferredOnMta As Integer = 30
    Public Const ACPolicyVersion As Integer = 31
    Public Const ACGeminiPolicyStatus As Integer = 32
    Public Const ACGeminiBusinessType As Integer = 33
    Public Const ACDeferredInd As Integer = 34
    Public Const ACPolicyIgnore As Integer = 35
    Public Const ACBrokerCnt As Integer = 36
    Public Const ACRiskCodeId As Integer = 37
    Public Const ACAnalysisCodeId As Integer = 38
    Public Const ACPolicyDeductibleId As Integer = 111
    Public Const ACPolicyLimitsId As Integer = 112
    Public Const ACProposalDate As Integer = 39
    Public Const ACDiaryDate As Integer = 40
    Public Const ACReviewDate As Integer = 41
    Public Const ACRenewalDayNumber As Integer = 42
    Public Const ACPolicyTypeId As Integer = 43
    Public Const ACIndicator As Integer = 44
    Public Const ACClause As Integer = 45
    Public Const ACCover As Integer = 46
    Public Const ACArea As Integer = 47
    Public Const ACLongTermUndertakingDate As Integer = 48
    Public Const ACRenewalStopCodeID As Integer = 49
    Public Const ACVBSType As Integer = 50
    Public Const ACVBSStatus As Integer = 51
    Public Const ACIsInsurerRateTable As Integer = 52
    Public Const ACIsRelatedPolicies As Integer = 53
    Public Const ACIsRetainedDocuments As Integer = 54
    Public Const ACSchemesPostcode As Integer = 55
    Public Const ACPaidDirect As Integer = 56
    Public Const ACScheme As Integer = 57
    Public Const ACBrokerageAmount As Integer = 58
    Public Const ACIsMinimumBrokerageFlag As Integer = 59
    Public Const ACAnnualPremium As Integer = 60
    Public Const ACThisPremium As Integer = 61
    Public Const ACNetPremium As Integer = 62
    Public Const ACCommissionAmount As Integer = 63
    Public Const ACIPTableAmount As Integer = 64
    Public Const ACIPTPercentage As Integer = 65
    Public Const ACIsIPTOverridden As Integer = 66
    Public Const ACTaxAmount As Integer = 67
    Public Const ACVatableAmount As Integer = 68
    Public Const ACVatPercentage As Integer = 69
    Public Const ACVatAmount As Integer = 70
    Public Const ACPaymentMethod As Integer = 71
    Public Const ACUserDefinedDataID As Integer = 72
    Public Const ACCommissionPercentage As Integer = 73
    Public Const ACInvariantKey As Integer = 74
    Public Const ACInsuredName As Integer = 75
    Public Const ACAlternateReference As Integer = 76
    Public Const ACIsClientInvoiced As Integer = 77
    Public Const ACOldPolicyNumber As Integer = 78
    Public Const ACQuoteExpiryDate As Integer = 79
    Public Const ACAlternateAccountCnt As Integer = 80
    Public Const ACTransDescription As Integer = 81
    Public Const ACAccountExecutiveCnt As Integer = 82
    Public Const ACAnniversaryDate As Integer = 83
    Public Const ACPolicyStyleID As Integer = 84
    Public Const ACUnderwritingYearID As Integer = 85
    Public Const ACPolicyStatusID As Integer = 86
    Public Const ACInceptionTPI As Integer = 87
    'FSA Phase III
    Public Const ACFSACustomerCategoryID As Integer = 88
    Public Const ACFSAContractLocationID As Integer = 89
    Public Const ACFSAUnderwriterCnt As Integer = 90
    Public Const ACFSATypeOfSaleID As Integer = 91
    Public Const ACFSARenewalConsent As Integer = 92
    Public Const ACBaseCurrencyID As Integer = 93
    Public Const ACCountryID As Integer = 94
    Public Const ACDiscountReasonID As Integer = 95
    Public Const ACDiscountedPremium As Integer = 96
    Public Const ACDiscountPercentage As Integer = 97
    Public Const ACMatchDiscountedPremiumFlag As Integer = 98
    Public Const ACInsuranceFilePutOnNextInstalmentRenewal As Integer = 99
    Public Const ACInsuranceFileAnniversaryCopy As Integer = 100
    Public Const ACDiscountRecurringTypeId As Integer = 101
    'True Monthly Policies
    Public Const ACLeadAllowConsolidatedCommission As Integer = 102
    Public Const ACSubAllowConsolidatedCommission As Integer = 103
    Public Const ACFeesTaxes As Integer = 104

    Public Const ACCCTermsAgreed As Integer = 105
    Public Const ACCCTermsAgreedDate As Integer = 106
    Public Const ACCCInceptionDate As Integer = 107
    Public Const ACCCPolicyDocumentsIssuedDate As Integer = 108
    Public Const ACCCPolicyDocumentCorrect As Integer = 109
    Public Const ACCCErrorNotificationDate As Integer = 110
    Public Const ACFSARiskTransferAgreement As Integer = 113
    'PN38802 added new field for renewal premium
    Public Const ACRenewalPremium As Integer = 114
    '1.12 WR25
    Public Const ACRenewalProductID As Integer = 115
    Public Const ACOriginalProductID As Integer = 116

    Public Const ACFSARiskTransferEditable As Integer = 117

    Public Const ACCurrencyToBaseXRate As Integer = 118
    '--RFC-PLICO14 - Amit
    Public Const ACManualDiscountPercentage As Integer = 119

    ' WPR 63
    Public Const ACQuoteStatusID As Integer = 120
    Public Const ACQuoteVersionID As Integer = 121
    Public Const ACBaseInsuranceFolderCnt As Integer = 122

    'WPR 73-74
    Public Const ACContactuserId As Integer = 123
    Public Const kCoInsPlacement As Integer = 124
    Public Const ACMTAReasonId As Integer = 125
    Public Const ACIsMarketPlacePolicy As Integer = 126
    Public Const kCollectionFrequencyId = 127
    Public Const kPaymentTermId = 128
    Public Const kBIC As Integer = 129
    Public Const kIBAN As Integer = 130
    Public Const ACCorrespondenceType As Integer = 131
    Public Const ACDefaultPreferredCorrespondence As Integer = 132
    Public Const ACIsAgentCorrespondence As Integer = 133
    Public Const ACMediaType As Integer = 134
    Public Const ACSenderEmail As Integer = 135
    Public Const ACReceiverEmail As Integer = 136
    Public Const ACOriginalInsuranceFileTypeId As Integer = 137
    'CJR 16/1/2003
    Public Const ACPolicyClientPartyCnt As Integer = 0
    Public Const ACPolicyClientIsLead As Integer = 1
    Public Const ACPolicyClientIsCorrespondence As Integer = 2
    Public Const ACPolicyClientShortname As Integer = 3
    Public Const ACPolicyClientResolvedName As Integer = 4
    Public Const ACPolicyClientAddress1 As Integer = 5
    Public Const ACPolicyClientIsInsured As Integer = 6
    Public Const ACPolicyClientRiskCnt As Integer = 7
    Public Const ACPolicyClientLeadDtls As Integer = 8
    Public Const ACPolicyClientCorrespondenceDtls As Integer = 9
End Module