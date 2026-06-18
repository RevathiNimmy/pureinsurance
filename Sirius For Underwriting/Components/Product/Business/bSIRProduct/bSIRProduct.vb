Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  20/07/2000
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRProduct"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"




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
    Public Const ACIPaymentMethod As Integer = 59
    '------------------------------------------------------------------
    '3 Constants Defined for Float Balance and Pre-Payment
    Public Const ACICanMakeLiveInvoice As Integer = 53
    Public Const ACICanMakeLiveInstalments As Integer = 54
    Public Const ACICanMakeLivePaynow As Integer = 55

    '3 Constants Defined for Navigator Enhancement
    Public Const ACIProduceSchedule As Integer = 56
    Public Const ACIProduceCertificate As Integer = 57
    Public Const ACIProduceDebitNote As Integer = 58

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

    ' Tech Spec PGR 8.8 Renewals
    Public Const ACICPSelUseNBRenPaymentTermsAtSelection As Integer = 142

    ' End

    Public Const ACICPSelIsReservesReadOnly As Integer = 177
    Public Const ACICPSelIsRecoveriesReadOnly As Integer = 178
    Public Const ACICPSelIsPaymentsReadOnly As Integer = 179
    Public Const ACICPSelVoidTransaction As Integer = 180
    Public Const ACICPSelIsQuoteVersioning As Integer = 181
    Public Const ACICPSelDeleteQuoteAfter As Integer = 182

    'Seperate Const Required for Update as there are some Agent Renewal Values that are not required to be updated
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
    Public Const ACICPUpdIsRecommendClaimPayments As Integer = 120
    Public Const ACICPUpdMTAdateallowed As Integer = 121
    Public Const ACICPUpdMTAAllocation As Integer = 122
    Public Const ACICPUpdDefaultRenMonths As Integer = 123
    Public Const ACICPPaymentCannotExceedReserve As Integer = 124
    Public Const ACMTCRatingRulesEnabled As Integer = 125
    Public Const ACICanMakeBankGuarantee As Integer = 126

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    Public Const ACICPUpdCNDefaultPeriod As Integer = 127
    Public Const ACICPUpdCNMaxNo As Integer = 128
    Public Const ACICPUpdCNDocTemplateId As Integer = 129
    Public Const ACICPUpdCNNumberingId As Integer = 130

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.2.1)
    Public Const ACICPUpdAllowBackdatedMTAs As Integer = 131
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.2.1)

    ''Start(Saurabh Agrawal) Out of sequence MTa bug fixing
    Public Const ACICPUpdOutOfsequenceMTAUserGroup As Integer = 132
    Public Const ACICPUpdOutOfsequenceMTATaskGroup As Integer = 133
    ''End(Saurabh Agrawal) Out of sequence MTa bug fixing

    ' Tech Spec PGR 8.8 Renewals
    Public Const ACICPUpdUseNBRenPaymentTermsAtSelection As Integer = 134
    ' End

    Public Const ACIRoundOffToZero As Integer = 135

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const ACICPUpdCheckMediaTypeStatusAtClaimPayment As Integer = 136
    Public Const ACICPUpdCheckMediaTypeStatusAtPolicyRefund As Integer = 137
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    'Start - Renuka - (WPR87 Paralleling)
    Public Const ACICPUpdChangePolicyNumberAtRenewalAutomatically As Integer = 138
    'End - Renuka - (WPR87 Paralleling)
    Public Const ACICanMakeCashDeposit As Integer = 139 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling

    '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.2.2)
    Public Const ACICPUpdRIManaulPremiumAdjustment As Integer = 140

    'vivek63205
    Public Const ACICPUpdAllowBackdatedCan As Integer = 141
    Public Const ACICPUpdTMPAutoRenFac As Integer = 142


    'Start  Written Status
    Public Const ACIPUpdWrittenPolicyStatus As Integer = 143
    Public Const ACIPUpdTaskManagerDays As Integer = 144
    Public Const ACIPUpdReminderUserGroup As Integer = 145
    Public Const ACIPUpdReminderTaskGroup As Integer = 146
    Public Const ACICPUpdUsePriorTermSchemeAtRenewal As Integer = 147
    Public Const ACICPUpdBindRenewalWithoutInvitation As Integer = 148
    Public Const ACICPUpdEnablePrePayment As Integer = 149

    ' Wpr53
    Public Const ACICPUpdMandatoryRiskTypeId As Integer = 150
    Public Const ACICPUpdDoNotDeleteRenewalQuoteOnMTA As Integer = 151
    Public Const kICPUpdDefaultCoverToDateToLastDay As Integer = 152
    Public Const kICPUpdUnifiedRenewlDateIsReadOnly As Integer = 153

    Public Const ACICPUpdIsReservesReadonly As Integer = 154
    Public Const ACICPUpdIsRecoveriesReadonly As Integer = 155
    Public Const ACICPUpdIsPaymentsReadonly As Integer = 156

    'WPR05
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

    'Constants For Product Level Claim Options
    Public Const ACIsMultipleClaimsPayments As Integer = 0
    Public Const ACMaxUnauthorisedClaimValue As Integer = 1
    Public Const ACMaxUnauthorisedNoClaimPayments As Integer = 2
    Public Const ACRunAuthorisationScriptsClaimPayments As Integer = 3
    Public Const ACClaimValueForLargeLossAdvice As Integer = 4
    Public Const ACInclusionOfCoInsurersOnClaims As Integer = 5
    Public Const ACAllowNegativeReserve As Integer = 6
    Public Const ACExtClmHandlerAcknowledgedTaskAllowedTime As Integer = 7
    Public Const ACExtClmHandlerSupplyPreReportTaskAllowedTime As Integer = 8
    Public Const ACValidPolicyversionatlossdate As Integer = 9
    Public Const ACIsGrossClaimPaymentAmount As Integer = 10
    Public Const ACClaimTaskGroup As Integer = 11
    Public Const ACClaimUserGroup As Integer = 12
    Public Const ACIsDuplicateClaimcheckEnabled As Integer = 13
    Public Const ACIsadvancedTaxScriptEnabled As Integer = 14
    Public Const ACIsPaymentRefCheckEnabled As Integer = 15
    Public Const ACIsRecommendClaimPayment As Integer = 16
    Public Const ACIsPaymentCannotExceedReserve As Integer = 17
    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const ACCheckMediaTypeStatusAtClaimPayment As Integer = 18
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const ACCSuppressReserve As Integer = 19
    Public Const ACCAuthorisationThreshold As Integer = 20


    Sub Main_Renamed()


    End Sub
End Module