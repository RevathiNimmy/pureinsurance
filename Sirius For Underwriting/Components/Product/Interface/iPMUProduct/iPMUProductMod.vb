Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")>
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 05/05/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' RKS 26/04/2005 - 354 Standard Wording Control Enchancements
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUProduct"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100

    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 316
    Public Const ACTabTitle4 As Integer = 317
    Public Const ACTabTitle5 As Integer = 318

    Public Const ACCode As Integer = 103
    Public Const ACEffectiveDate As Integer = 104
    Public Const ACRenewalWeeks As Integer = 105
    Public Const ACDescription As Integer = 106
    Public Const ACSchemeAgencyRef As Integer = 107
    Public Const ACBlockNo As Integer = 108
    Public Const ACProvClaimAutoNumberingID As Integer = 109
    Public Const ACQuoteAutoNumberingID As Integer = 110
    Public Const ACPolicyAutoNumberingID As Integer = 111
    Public Const ACFullClaimAutoNumberingID As Integer = 112
    Public Const ACMidnightRenewal As Integer = 113
    Public Const ACAutoRenewal As Integer = 114
    Public Const ACTaxSuppressed As Integer = 115
    Public Const ACShortPeriodRated As Integer = 116
    Public Const ACAccumulation As Integer = 117
    Public Const ACRIPointer As Integer = 118
    Public Const ACReportPointer As Integer = 119
    Public Const ACCover As Integer = 132


    'TN20001101 (Start) process 29
    Public Const ACClaimYearToCheck As Integer = 120
    Public Const ACMaxSingleClaimValue As Integer = 121
    Public Const ACMaxNumberOfClaim As Integer = 122
    Public Const ACMaxTotalClaimValue As Integer = 123
    'TN20001101 (Start) process 29

    'TN20010514 start
    Public Const ACNBProrata As Integer = 124
    Public Const ACMTAProrata As Integer = 125
    'TN20010514 end

    'JMK 25/07/2001 start
    Public Const ACRoundPremium As Integer = 126
    Public Const ACRoundingSection As Integer = 127
    'JMK 25/07/2001 end

    Public Const ACPolicyNumberAtQuote As Integer = 128

    ' PW311002
    Public Const ACFollowUpTimeFrame As Integer = 129
    Public Const ACGracePeriod As Integer = 130

    Public Const ACAllowStandardWordingEdit As Integer = 131
    Public Const ACApplyMandatoryRisk As Integer = 133
    Public Const kUnifiedRenewalDateIsReadOnly As Integer = 135


    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    'TN20001010
    Public Const ACApplyButton As Integer = 207
    Public Const ACRIModelButton As Integer = 208
    Public Const ACSPRButton As Integer = 209

    'JMK 23/10/2001
    Public Const ACInsurerModelButton As Integer = 210

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Alix - 13/02/2004
    Public Const ACCheckAgent As Integer = 310
    Public Const ACPositiveValues As Integer = 311
    ' /Alix

    Public Const ACCheckMediaTypeMandatory As Integer = 312
    Public Const ACPolicyStyleID As Integer = 313
    Public Const ACPolicyStyleMandatory As Integer = 314

    Public Const ACIProductid As Integer = 0
    Public Const ACICaptionId As Integer = 1
    Public Const ACICode As Integer = 2
    Public Const ACIDescription As Integer = 3
    Public Const ACIProductEffectiveDate As Integer = 4
    Public Const ACIIsDeleted As Integer = 5
    Public Const ACISchemeAgencyRef As Integer = 6
    Public Const ACIBlockNo As Integer = 7
    Public Const ACIIsTaxSuppressed As Integer = 8
    Public Const ACIQuoteAutoNumberingID As Integer = 9
    Public Const ACIIsShortPeriodRated As Integer = 10
    Public Const ACIIsMidnightRenewal As Integer = 11
    Public Const ACIIsAutoRenewable As Integer = 12
    Public Const ACIRenewalPeriod As Integer = 13
    Public Const ACIPolicyAutoNumberingID As Integer = 14
    Public Const ACIProvClaimAutoNumberingID As Integer = 15
    Public Const ACIFullClaimAutoNumberingID As Integer = 16
    Public Const ACIAccumulation As Integer = 17
    Public Const ACIRIPointer As Integer = 18
    Public Const ACIReportPointer As Integer = 19
    Public Const ACIClaimYearToCheck As Integer = 20
    Public Const ACIMaxSingleClaimValue As Integer = 21
    Public Const ACIMaxNumberOfClaim As Integer = 22
    Public Const ACIMaxTotalClaimValue As Integer = 23
    Public Const ACINBProrata As Integer = 24
    Public Const ACIMTAProrata As Integer = 25
    Public Const ACIRoundPremium As Integer = 26
    Public Const ACIRoundingSection As Integer = 27
    Public Const ACIPolicyNumberAtQuote As Integer = 28
    Public Const ACIFollowUpTimeFrame As Integer = 29
    Public Const ACIGracePeriod As Integer = 30
    Public Const ACIPreventCancelledAgents As Integer = 31
    Public Const ACIAllowPositiveValues As Integer = 32
    Public Const ACIMediaTypeMandatory As Integer = 33
    Public Const ACIPolicyStyleID As Integer = 34
    Public Const ACIPolicyStyleMandatory As Integer = 35
    Public Const ACICurrencyChange As Integer = 36
    Public Const ACILossCurrencyChange As Integer = 37
    Public Const ACIChangePolicyNumberAtRenewal As Integer = 38
    Public Const ACIAllowStandardWordingEdit As Integer = 39
    Public Const ACIHideSummaryAtRenewalAcceptance As Integer = 40
    Public Const ACIProductTrueMonthlyPolicy As Integer = 41
    Public Const ACIProductAnniversaryRenewalWeeks As Integer = 42
    Public Const ACIProductSuppressClaimTransactionsReserves As Integer = 43
    Public Const ACIProductSuppressClaimTransactionsPayments As Integer = 44
    Public Const ACIProductSuppressClaimTransactionsRecoveries As Integer = 45
    '------------------------------------------------------------------
    '7 constant defined for TMP
    '------------------------------------------------------------------
    Public Const ACIUnifiedRenewalDay As Integer = 46
    Public Const ACILeadAllowConsolidateComm As Integer = 47
    Public Const ACILeadMonthInCycle As Integer = 48
    Public Const ACILeadSuspenseAcct As Integer = 49
    Public Const ACISubAllowConsolidateComm As Integer = 50
    Public Const ACISubMonthInCycle As Integer = 51
    Public Const ACISubSuspenseAcct As Integer = 52
    '------------------------------------------------------------------
    '3 Constants Defined for Float Balance and Pre-Payment
    Public Const ACICanMakeLiveInvoice As Integer = 53
    Public Const ACICanMakeLiveInstalments As Integer = 54
    Public Const ACICanMakeLivePaynow As Integer = 55

    '3 Constants Defined for Navigator Enhancement
    Public Const ACIProduceSchedule As Integer = 56
    Public Const ACIProduceCertificate As Integer = 57
    Public Const ACIProduceDebitNote As Integer = 58
    Public Const ACIPaymentMethod As Integer = 59


    Public Const ACITradeNbOnline As Integer = 60
    Public Const ACITradeMtaOnline As Integer = 61
    Public Const ACITradeRnlOnline As Integer = 62
    Public Const ACIOnlineTradingCommencedOn As Integer = 63


    Public Const ACIIsRenewable As Integer = 64
    Public Const ACIIsRenewalSelectionEnabled As Integer = 65
    Public Const ACITrueMonthlyPolicyRenewalCommunication As Integer = 66
    Public Const ACIRenewalSelectionManReviewTemplateId As Integer = 67
    Public Const ACIRenewalSelectionManReviewAttachmentTemplateId As Integer = 68
    Public Const ACIRenewalSelectionInviteTemplateId As Integer = 69
    Public Const ACIRenewalSelectionInviteAttachmentTemplateId As Integer = 70
    Public Const ACIRenewalSelectionUpdateTemplateId As Integer = 71
    Public Const ACIRenewalSelectionUpdateAttachmentTemplateId As Integer = 72
    Public Const ACIIsRenewalInviteEnabled As Integer = 73
    Public Const ACIRenewalInviteManReviewTemplateId As Integer = 74
    Public Const ACIRenewalInviteManReviewAttachmentTemplateId As Integer = 75
    Public Const ACIRenewalInviteInviteTemplateId As Integer = 76
    Public Const ACIRenewalInviteInviteAttachmentTemplateId As Integer = 77
    Public Const ACIRenewalInviteUpdateTemplateId As Integer = 78
    Public Const ACIRenewalInviteUpdateAttachmentTemplateId As Integer = 79
    Public Const ACIIsRenewalUpdateEnabled As Integer = 80
    Public Const ACIRenewalUpdateManReviewTemplateId As Integer = 81
    Public Const ACIRenewalUpdateManReviewAttachmentTemplateId As Integer = 82
    Public Const ACIRenewalUpdateInviteTemplateId As Integer = 83
    Public Const ACIRenewalUpdateInviteAttachmentTemplateId As Integer = 84
    Public Const ACIRenewalUpdateUpdateTemplateId As Integer = 85
    Public Const ACIRenewalUpdateUpdateAttachmentTemplateId As Integer = 86
    Public Const ACIIsAgentRenewalSelectionEnabled As Integer = 87
    Public Const ACIIsAgentRenewalInviteEnabled As Integer = 88
    Public Const ACIIsAgentRenewalUpdateEnabled As Integer = 89
    Public Const ACIAgentRenewalManReviewTemplateId As Integer = 90
    Public Const ACIAgentRenewalManReviewReportId As Integer = 91
    Public Const ACIAgentRenewalInviteTemplateId As Integer = 92
    Public Const ACIAgentRenewalInviteReportId As Integer = 93
    Public Const ACIAgentRenewalUpdateTemplateId As Integer = 94
    Public Const ACIAgentRenewalUpdateReportId As Integer = 95


    Public Const ACIRenewalSelectionManReviewTemplateCode As Integer = 96
    Public Const ACIRenewalSelectionManReviewAttachmentTemplateCode As Integer = 97
    Public Const ACIRenewalSelectionInviteTemplateCode As Integer = 98
    Public Const ACIRenewalSelectionInviteAttachmentTemplateCode As Integer = 99
    Public Const ACIRenewalSelectionUpdateTemplateCode As Integer = 100
    Public Const ACIRenewalSelectionUpdateAttachmentTemplateCode As Integer = 101
    Public Const ACIRenewalInviteManReviewTemplateCode As Integer = 102
    Public Const ACIRenewalInviteManReviewAttachmentTemplateCode As Integer = 103
    Public Const ACIRenewalInviteInviteTemplateCode As Integer = 104
    Public Const ACIRenewalInviteInviteAttachmentTemplateCode As Integer = 105
    Public Const ACIRenewalInviteUpdateTemplateCode As Integer = 106
    Public Const ACIRenewalInviteUpdateAttachmentTemplateCode As Integer = 107
    Public Const ACIRenewalUpdateManReviewTemplateCode As Integer = 108
    Public Const ACIRenewalUpdateManReviewAttachmentTemplateCode As Integer = 109
    Public Const ACIRenewalUpdateInviteTemplateCode As Integer = 110
    Public Const ACIRenewalUpdateInviteAttachmentTemplateCode As Integer = 111
    Public Const ACIRenewalUpdateUpdateTemplateCode As Integer = 112
    Public Const ACIRenewalUpdateUpdateAttachmentTemplateCode As Integer = 113
    Public Const ACIAgentRenewalManReviewTemplateCode As Integer = 114
    Public Const ACIAgentRenewalManReviewReportCode As Integer = 115
    Public Const ACIAgentRenewalInviteTemplateCode As Integer = 116
    Public Const ACIAgentRenewalInviteReportCode As Integer = 117
    Public Const ACIAgentRenewalUpdateTemplateCode As Integer = 118
    Public Const ACIAgentRenewalUpdateReportCode As Integer = 119

    Public Const ACICPSelMultipleClaimPayments As Integer = 120
    Public Const ACICPSelMaxUnauthorisedClaimValue As Integer = 121
    Public Const ACICPSelMaxNoofUnauthorisedClaimPayments As Integer = 122
    Public Const ACICPSelRunAuthorisationScriptsforClaimPayments As Integer = 123


    Public Const ACICPSelBankAccountId As Integer = 124
    Public Const ACICPSelClaimValueForLargeLossAdvice As Integer = 125
    Public Const ACICPSelInclusionofCoInsurersOnClaims As Integer = 126
    Public Const ACICPSelAllowNegativeReserve As Integer = 127
    Public Const ACICPSelExtClmHandlerAcknowledgedTaskAllowedTime As Integer = 128
    Public Const ACICPSelExtClmHandlerSupplyPreReportTaskAllowedTime As Integer = 129
    Public Const ACICPSelValidPolicyVersionAtLossDate As Integer = 130
    Public Const ACICPSelIsGrossClaimPaymentAmount As Integer = 131
    Public Const ACICPSelClaimTaskGroup As Integer = 132
    Public Const ACICPSelClaimUserGroup As Integer = 133
    Public Const ACICPSelClaimsUDTA As Integer = 134
    Public Const ACICPSelClaimsUDTB As Integer = 135
    Public Const ACICPSelClaimsUDTC As Integer = 136
    Public Const ACICPSelClaimsUDTD As Integer = 137
    Public Const ACICPSelClaimsUDTE As Integer = 138
    Public Const ACICPSelIsDuplicateClaimCheckEnabled As Integer = 139
    Public Const ACICPSelIsAdvancedTaxScriptEnabled As Integer = 140
    Public Const ACICPSelIsPaymentRefCheckEnabled As Integer = 141
    Public Const ACICPSelIsRecommender As Integer = 142
    Public Const ACICPSelDateAllowed As Integer = 143
    Public Const ACICPSelAllocation As Integer = 144
    Public Const ACICPSelRenewalMonths As Integer = 145
    Public Const ACICPSelPaymentCannotExceedReserve As Integer = 146
    Public Const ACICPSelMTCRatingRulesEnabled As Integer = 147
    Public Const ACICPSelCanMakeBankGuarantee As Integer = 148

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.1.1)
    Public Const ACICPSelCNDefaultPeriod As Integer = 149
    Public Const ACICPSelCNMaxNo As Integer = 150
    Public Const ACICPSelCNDocTemplateID As Integer = 151
    Public Const ACICPSelCNNumberingId As Integer = 152
    Public Const ACICPSelCNCode As Integer = 153
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.1.1)


    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.1)
    Public Const ACICPSelAllowBackdatedMTAs As Integer = 154
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.1)

    ''Start(Saurabh Agrawal) Out of Sequence mta Bug Fixing
    Public Const ACICPSelOutOfSequencemtaUsergroup As Integer = 155
    Public Const ACICPSelOutOfSequencemtaTaskgroup As Integer = 156

    ' Tech Spec PGR 8.8 Renewals
    Public Const ACICPSelUseNBRenPaymentTermsAtSelection As Integer = 157
    ' End

    'Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    Public Const ACICPSelRoundOffToZero As Integer = 158
    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

    ''End(Saurabh Agrawal) Out of Sequence mta Bug Fixing
    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const ACICPSelCheckMediaTypeStatusAtClaimPayment As Integer = 159
    Public Const ACICPSelCheckMediaTypeStatusAtPolicyRefund As Integer = 160
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    'Start - Renuka - (WPR87 Paralleling)
    Public Const ACICPSelChangePolicyNumberAtRenewalAutomatically As Integer = 161
    'End - Renuka - (WPR87 Paralleling)
    Public Const ACICPSelCanMakeCashDeposit As Integer = 162 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

    '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.1.2.1)
    Public Const ACICPSelRIManualPremiumAdjustment As Integer = 163
    Public Const ACICPSelAllowBackdatedCan As Integer = 164 ' PN 63205

    Public Const ACICPSelTMPAutoRenFac As Integer = 165
    'Seperate Const Required for Update as there are some Agent Renewal Values that are not required to be updated
    'Start - Written Status
    Public Const ACIPSelWrittenPolicyStatus As Integer = 166
    Public Const ACIPSelTaskManagerDays As Integer = 167
    Public Const ACIPSelReminderUserGroup As Integer = 168
    Public Const ACIPSelReminderTaskGroup As Integer = 169
    'End- Written Status-
    Public Const ACICPSelUsePriorTermSchemeAtRenewal As Integer = 170
    Public Const ACICPSelBindRenewalWithoutInvitation As Integer = 171
    Public Const ACICPSelEnablePrePayment As Integer = 172
    Public Const ACICPSelMandatoryRiskTypeId As Integer = 173
    Public Const ACICPDoNotDeleteRenewalQuoteOnMTA As Integer = 174
    Public Const kICPDefaultCoverToDateToLastDay As Integer = 175
    Public Const kICPUnifiedrenewalDateIsReadOnly As Integer = 176

    Public Const ACICPSelIsReservesReadOnly As Integer = 177
    Public Const ACICPSelIsRecoveriesReadOnly As Integer = 178
    Public Const ACICPSelIsPaymentsReadOnly As Integer = 179

    Public Const ACICPDisplayRerateForQuoteAndNB As Integer = 180
    Public Const ACICPDisplayRerateForCancellationsAndReinstatments As Integer = 181
    Public Const ACICPDisplayRerateForMTA As Integer = 182
    Public Const ACICPautoRenewBackDatedMonthlyPolicy = 183
    Public Const ACICPDeleteRenewalQuoteReRunOnMTA As Integer = 184
    Public Const ACICPDisplayRerateForRenewal As Integer = 185
    Public Const ACICPSelRetainPolicyNumberonCopy As Integer = 186
    Public Const ACICPEditAnnivDate = 187
    Public Const ACICPDisableCoverStartDateOnREN As Integer = 188
    Public Const ACICPSelUsePolicyInceptionDate As Integer = 189
    Public Const ACICPSelAuthorisationThreshold As Integer = 190
	  Public Const ACICPSelVoidTransaction As Integer = 191
    Public Const ACICPSelIsQuoteVersioning As Integer = 192
    Public Const ACICPSelDeleteQuoteAfter As Integer = 193
    Public Const ACICPSelRecoveryInstalmentsEnabled As Integer = 194


    Public Const ACICPUpdMultipleClaimPayments As Integer = 98
    Public Const ACICPUpdMaxUnauthorisedClaimValue As Integer = 99
    Public Const ACICPUpdMaxNoofUnauthorisedClaimPayments As Integer = 100
    Public Const ACICPUpdRunAuthorisationScriptsforClaimPayments As Integer = 101

    Public Const ACICPUpdBankAccountId As Integer = 102
    Public Const ACICPUpdClaimValueForLargeLossAdvice As Integer = 103
    Public Const ACICPUpdInclusionofCoInsurersOnClaims As Integer = 104
    Public Const ACICPUpdAllowNegativeReserve As Integer = 105
    Public Const ACICPUpdExtClmHandlerAcknowledgedTaskAllowedTime As Integer = 106
    Public Const ACICPUpdExtClmHandlerSupplyPreReportTaskAllowedTime As Integer = 107
    Public Const ACICPUpdValidPolicyVersionAtLossDate As Integer = 108
    Public Const ACICPUpdIsGrossClaimPaymentAmount As Integer = 109
    Public Const ACICPUpdClaimTaskGroup As Integer = 110
    Public Const ACICPUpdClaimUserGroup As Integer = 111
    Public Const ACICPUpdClaimsUDTA As Integer = 112
    Public Const ACICPUpdClaimsUDTB As Integer = 113
    Public Const ACICPUpdClaimsUDTC As Integer = 114
    Public Const ACICPUpdClaimsUDTD As Integer = 115
    Public Const ACICPUpdClaimsUDTE As Integer = 116
    Public Const ACICPUpdIsDuplicateClaimCheckEnabled As Integer = 117
    Public Const ACICPUpdIsAdvancedTaxScriptEnabled As Integer = 118
    Public Const ACICPUpdIsPaymentRefCheckEnabled As Integer = 119
    Public Const ACICPUpdIsRecommender As Integer = 120
    Public Const ACICPUpdMTADateAllowed As Integer = 121
    Public Const ACICPUpdMTAAllocation As Integer = 122
    Public Const ACICPUpdDefaultRenewalMonths As Integer = 123
    Public Const ACICPUpdPaymentCannotExceedReserve As Integer = 124
    Public Const ACMTCRatingRulesEnabled As Integer = 125
    Public Const ACICanMakeBankGuarantee As Integer = 126

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.1.2)

    Public Const ACICPUpdCNDefaultPeriod As Integer = 127
    Public Const ACICPUpdCNMaxNo As Integer = 128
    Public Const ACICPUpdCNDocTemplateID As Integer = 129
    Public Const ACICPUpdCNNumberingId As Integer = 130

    'Public Const ACICPUpdMAXCount = 125
    'Public Const ACICPUpdMAXCount = 130

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section(4.3.1.2)


    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc)
    Public Const ACICPUpdAllowBackdatedMTAs As Integer = 131
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc)

    ''Start(Saurabh Agrawal) Out of sequence MTa bug fixing
    Public Const ACICPUpdOutOfsequenceMTAUserGroup As Integer = 132
    Public Const ACICPUpdOutOfsequenceMTATaskGroup As Integer = 133
    ''End(Saurabh Agrawal) Out of sequence MTa bug fixing
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.2)

    ' Tech Spec PGR 8.8 Renewals
    Public Const ACICPUpdUseNBRenPaymentTermsAtSelection As Integer = 134
    ' End


    Public Const ACICPUpdRoundOffToZero As Integer = 135

    Public Const ACICPUpdCheckMediaTypeStatusAtClaimPayment As Integer = 136
    Public Const ACICPUpdCheckMediaTypeStatusAtPolicyRefund As Integer = 137

    Public Const ACICPUpdChangePolicyNumberAtRenewalAutomatically As Integer = 138

    Public Const ACICanMakeCashDeposit As Integer = 139 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Public Const ACICPUpdRIManualPremiumAdjustment As Integer = 140
    Public Const ACICPUpdAllowBackdatedCan As Integer = 141
    Public Const ACICPUpdTMPAutoRenFac As Integer = 142

    'Start Written Status
    Public Const ACIPUpdWrittenPolicyStatus As Integer = 143
    Public Const ACIPUpdTaskManagerDays As Integer = 144
    Public Const ACIPUpdReminderUserGroup As Integer = 145
    Public Const ACIPUpdReminderTaskGroup As Integer = 146
    'End Written Status
    Public Const ACICPUpdUsePriorTermSchemeAtRenewal As Integer = 147
    Public Const ACICPUpdBindRenewalWithoutInvitation As Integer = 148
    Public Const ACICPUpdEnablePrePayment As Integer = 149
    Public Const ACICPUpdMandatoryRiskTypeId As Integer = 150
    Public Const ACICPUpdDoNotDeleteRenewalQuoteOnMTA As Integer = 151
    Public Const kICPUpdDefaultCoverToDateToLastDay As Integer = 152
    Public Const kICPUpdUnifiedRenewlDateIsReadOnly As Integer = 153

    Public Const ACICPUpdIsReservesReadonly As Integer = 154
    Public Const ACICPUpdIsRecoveriesReadonly As Integer = 155
    Public Const ACICPUpdIsPaymentsReadonly As Integer = 156
    Public Const ACICPUpdDisplayRerateForQuoteAndNB As Integer = 157
    Public Const ACICPUpdDisplayRerateForCancellationsAndReinstatments As Integer = 158
    Public Const ACICPUpdDisplayRerateForMTA As Integer = 159
    Public Const ACICPUpdAutoRenewBackDatedMonthlyPolicy As Integer = 160
    Public Const ACICPUpdDoNotDeleteRenQuote As Integer = 161
    Public Const ACICPUpdDisplayRerateForRenewal As Integer = 162
    Public Const ACICPUpdRetainPolicyNumberonCopy As Integer = 163
    Public Const ACICPUpdEditAnnivDate As Integer = 164
    Public Const ACICPUpdDisableCoverStartDateonREN As Integer = 165
    Public Const ACICPUpdUsePolicyInceptionDate As Integer = 166
    Public Const ACICPUpdAuthorisationThreshold As Integer = 167
    Public Const ACICPUpdVoidTransaction As Integer = 168
    Public Const ACICPUpdIsQuoteVersioning As Integer = 169
    Public Const ACICPUpdDeleteQuoteAfter As Integer = 170

    Public Const ACICPUpdRecoveryInstalmentsEnabled As Integer = 171
    Public Const ACICPUpdMAXCount As Integer = ACICPUpdRecoveryInstalmentsEnabled

    Public Const kSystemOptionRoundOffAccount As Integer = 5080
    Public Const systemOptionClaimsReserveAreGross As Integer = 5239
    'End - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.1.2)
    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107

    <ThreadStatic()>
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_oGIS As Object

    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenhelpID As Integer = 4089
    'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
    Public Enum ENClauseType
        ProductType = 1
        RiskType = 2
    End Enum
    'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)


    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Const PMEmptyString As String = ""

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Sub Main_Renamed()

    End Sub
End Module