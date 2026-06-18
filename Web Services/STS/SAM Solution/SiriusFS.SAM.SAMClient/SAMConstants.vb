Imports Microsoft.VisualBasic

Public Class SAMConstants

    Public Enum SAMBusinessErrors

        ' pre existing business error constants
        RecordLockedByAnotherUser = 200
        RecordNotLockedByCurrentUser = 201
        RecordChanged = 206

        ' new business error constants in range (1000000 - 2000000)
        DuplicateClaimExists = 1000001
        LossFromDataInFuture
        LossFromDateAfterReportedDate
        LossFromDateAfterLossToDate
        ReportedDateInFuture
        PolicyDataMissing
        ClaimDocumentsAlreadyProduced
        NumberingSchemeNotFound
        ProductsNumberingSchemesNotFound
        AutoClaimNumberingDisabled
        AgentRecordNotFound
        InsuranceFileDetailsNotFound
        AutoReinsuranceFailed
        TransactionTypeNotFound
        AccountsProcessingFailed
        DMEFolderCreationFailed
        FailedToCreateCOMComponent
        FailedToInitialiseCOMComponent
        COMComponentMethodFailed
        CopyClaimFailed
        InfoOnlyClaim
        ClaimPaymentIsNotAllowedAgainstAnInfoOnlyClaim
        ClaimReceiptIsNotAllowedAgainstAnInfoOnlyClaim
        MediaTypeMandatory
        DefferedReinsurance
        AgentOnlyMethodUserIsNotAValidAgent
        LessThanTwoTransactionsInTransactionArray
        AddDocumentTransactionsFailed
        TransactionAmountsDoNotBalance
        PolicyNumberIsAlreadyInUse
        UseOfAdvancedTaxScriptRequiresMandatoryInsurerTaxNumber
        UseOfAdvancedTaxScriptRequiresMandatoryInsuredPercentage
        UseOfAdvancedTaxScriptRequiresMandatoryInsuredPercentageAndInsurerTaxNumber
        PostingOfPaymentToAccountsFailedMissingClassOfBusinessCode
        PostingOfReceiptToAccountsFailedMissingClassOfBusinessCode
        UseOfAdvanceTaxScriptSystemOptionEnforcedMandatoryAdvanceTaxDetails
        NoSystemCurrenyHasBeenDefined
        NoAccountCurrencyHasBeenDefined
        NoAccountCurrencyConversionRatesHaveBeenDefined
        NoSystemCurrencyConversionRatesHaveBeenDefined
        DoCurrencyConversionReturnedInvalidStatus
        ProductOptionPreventCancelledAgentsMakingClaimPaymentsEnforcedAsAgentOnThisClaimIsCancelled
        ValidationOfScriptGeneratedTaxCalculationItemsFailed
        CurrentIdLookupUsingBaseIdAndVersionIDFailed
        FailedToAddTaxCalculationItem
        FailedToAddClaimReceiptItem
        FailedToAddClaimReceipt
        FailedToGetCurrencyToBaseExchangeRate
        WorkClaimPerilsInXMLDatasetDoNotMatchPerilsInCDTClaimPeril
        GisScreenNotSetupForSpecifiedPartyType
        DataTransferAddPartyRiskDatasetFailedAsGetDataSetFailedToReturnAnNewPartyDataset
        FutureAdjustmentFound
        OverlapsWithTemporaryMTA
        NoPolicyVersionFoundForThisMTAEffectiveDate
        ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowPermanentMTAs
        ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowTemporaryMTAs
        MTAEffectiveDateIsBeforeOriginalCoverFromDateOfPolicy
        BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDate
        ClaimHasBeenLodgedAfterTheEffectiveDateOfThisMTA
        MTAEffectiveDateIsPriorToAPreviousTransactionEffectiveDate
        ThisPolicyIsInRenewal
        MultiplePaymentsOrReceiptsSpecifiedAgainstASingleClaimVersion
        NotAllSelectedRisksHaveBeenSuccessfullyQuoted
        ReinsuranceHasNotBeenFullyAssigned
        ThereAreNoRisksOnThePolicy
        AtLeastOneRiskHasOutstandingQuestions
        AtLeastOneQuotedRisksMustBeSelected
        PolicyCannotBeMadeLiveAsTheLeadAgentsAccountHasBeenStopped
        PolicyCannotBeMadeLiveAsTheInsuredsAccountHasBeenStopped
        NoMatchingInstalmentQuoteFoundForSpecifiedDetails
        TotalCommissionMoreThanPremium
        TotalCommissionReturnIsMoreThanPremiumReturn
        InstalmentsIsNotAValidPaymentOptionWhenCancellingAPolicy
        ReturnPremiumIsGreaterThanBilledPremium
        CoverNoteBookNumberNotSpecified
        CoverNoteSheetNumberNotSpecified
        CoverNoteBookNumberIsInvalid
        CoverNoteSheetNumberIsOutOfRange
        CoverNoteNumberNotAssignedToSelectedAgent
        CoverNoteNumberNotAssignedToSelectedProduct
        CoverNoteNumberNotAssignedToSelectedBranch
        CoverNoteNumberEnteredIsNotAValidNumber
        CoverNoteStatusMissingPolicyCannotBeIssued
        CoverNoteStatusCancelledPolicyCannotBeIssued
        CoverNoteNumberAlreadyConvertedToPolicy
        ClaimDataTransferFailedToUpdateReserveItem
        ClaimDataTransferFailedToUpdateRecoveryItem
        PartyTypeNotFound

        'Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.5)
        CanNotUpdateLivePolicyDetails
        'End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.5)
        'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        ManualAllocationNotPossibleInAutoAllocation
        CashListIsEmptyButWorkFlowEnabled
        'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        'Start (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR01 – 64VB (SAM) -  Pre Payment Functionality)- (7.1.3.7)
        CollectionDateError
        'End (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR01 – 64VB (SAM) -  Pre Payment Functionality)- (7.1.3.7)
        'Start (PraveenGora) - (Tech Spec - UIIC WR01 - 64VB (SAM) - Cash Receipt.doc) - (7.1.2.2.1)
        InvalidChequeDate
        MissingComment
        'End (PraveenGora) - (Tech Spec - UIIC WR01 - 64VB (SAM) - Cash Receipt.doc) - (7.1.2.2.1)
        'Start (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (7.1.4.6)
        InvalidOldCoverNoteSheetNumber
        InvalidNewCoverNoteSheetNumber
        'End (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (7.1.4.6)

        'Start (Udaya Bhaskar) - Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly) -7.1.2.3.5
        BankGuaranteeNonIssued
        BankGuaranteeNotToDelete
        'End (Udaya Bhaskar) - Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly) -7.1.2.3.5

        'Start (Arul Stephen) - (Tech Spec - UIICWR6 - Policy Get Bank Guarantee.doc) -(7.1.5.4)
        InValidInsuranceFileCnt
        'End (Arul Stephen) - (Tech Spec - UIICWR6 - Policy Get Bank Guarantee.doc) -(7.1.5.4)
        'Start (Arul Stephen) - (Tech Spec - UIICWR6 - New Payment Option at Policy Level.doc) -(7.1.4.4) 
        InValidBankGuaranteeKey
        InValidAvailableBalance
        InValidProductKey
        InValidBranchId
        InValidCurrency
        InValidCoverFromDate
        RecordIsDeleted
        InValidBankGuaranteeStatus
        InValidPartyAgent
        InValidUser
        InValidProduct
        'End (Arul Stephen) - (Tech Spec - UIICWR6 - New Payment Option at Policy Level.doc) -(7.1.4.4) 
        'Start (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.4)
        BankGuaranteeSearchFailure
        'Start (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.4)

        'Start (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR33 - Work Manager - Create Task(Additional Changes).doc)- (7.1.4.5)
        InvalidDueDate
        'End (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR33 - Work Manager - Create Task(Additional Changes).doc)- (7.1.4.5)

        'Start (Prakash C Varghese) - (This change is done to make the merged code to work. copied from version 1.12)
        RenewalStatusTypeIsInvalidForThisAction
        InvalidTransactionType
        InsuranceFileDetailsIsNotValidToGenerateInvite
        NoRenewalStatusRecordExistsForTheSpecifiedPolicy
        TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted
        PolicyAlreadyAccepted
        RenwalInviteAlreadyPrinted
        PolicyNotValidTrueMonthlyPolicyUntilAnniversaryDateReachedtoGenerateInvite
        UserDontHaveCollectionDateOverrideAccess
        'End (Prakash C Varghese) - (This change is done to make the merged code to work. copied from version 1.12)
        LapseDateIsBeforeTheCoverStartDate
        LapseDateIsAfterTheCoverEndDate
        BackdatedMTANotPermitted
        BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDatePlusOne
        BackdatedMTANotPermittedWithClaimsForUser
        'Start (Ravikumar Pasupuleti) -(Tech Spec - UIIC WR63 - Insurer payments - AddWriteOff.doc)
        FailedToAddWriteOffBecauseSystemOptionHasBeenDisabled
        'End (Ravikumar Pasupuleti) -(Tech Spec - UIIC WR63 - Insurer payments - AddWriteOff.doc)

        'Start (Udaya Bhaskar) - (Tech Spec - UIIC WR60 - Authorise Payment - Authorise Claim Payment.doc) - (7.1.5.5)
        InvalidReferred
        'End (Udaya Bhaskar) - (Tech Spec - UIIC WR60 - Authorise Payment - Authorise Claim Payment.doc) - (7.1.5.5)
        'Start (Girija chokkalingam) - (Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc)- (7.1.3.7)
        Invalidmediatype
        'End (Girija chokkalingam) - (Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc)- (7.1.3.7)
	NoAccountingPeriodFound
        'Start (Prakash C Varghese)-(PartyBank functionality)
        ErrorInAddPartyBankCollection
        'End (Prakash C Varghese)-(PartyBank functionality)
        'Start(Saurabh Agrawal)
        InsufficentPlanBalnceToSpreadTheReducedMTA
        'End(Saurabh Agrawal)
        ClaimNumberAlreadyExists   

 'Start - Renuka - (WPR87 Paralleling)
        NoAccountingPeriodDefinedForTheSelectedDate
        FailedToGetNextNumberForTheSelectedAccountingPeriod
        'End - Renuka - (WPR87 Paralleling)
        'Start - Renuka - (WPR64 Paralleling)
        InvalidCommissionRate
        'End - Renuka - (WPR64 Paralleling)

		'Start - Prakash - WPR85 Parelleling
        PolicyCannotBeMadeLiveAsTheSelectedCashDepositAccountHasBeenStopped
        MultiplePaymentMethodSpecified
        CashDepositAccountDoesNotBelongToInsured
        CashDepositAccountDoesNotBelongToAgent
        CashDepositAccountDoesNotBelongToAgentAndInsured
        InsufficientBalanceInCashDepositAccount
        InsufficientRunningBalanceInCashDepositAccount		
		'End - Prakash - WPR85 Parelleling

    End Enum
       

    Public Enum SAMBusinessWarnings

        RiskIsDeferred = 2000001
        PolicyIsVoid = 2000002
        LossFromDateBeforePolicyStartDate = 2000003
        LossFromDateAfterPolicyEndDate = 2000004
        PolicyIsDifferent = 2000005
        InfoOnlyClaimDataTruncated = 2000006
        LossDateChanged
        UserDoesntHaveAuthorityToMakeThisPaymentPaymentWasReferred
        MTAEffectiveDateIsPriorToAPreviousTransactionEffectiveDate

        'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        WorkFlowIsDisabledButCashListIsNotEmpty
        'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)

        'Start (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.2.4)
        'Payment done to a deleted party
        PaymentToDeletedParty
        'End (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.2.4)
        'Start (Arul Stephen A) - (Tech Spec - UIIC WR6 -Policy Get Bank Guarantee.doc) - (7.1.5.4)
        InCaseOfDirectBusinessNoAgentWillBeTheir
        InCaseIfAgentIsABrokerThenOnlyAgentBGsShouldbeRetrieved
        InCaseIfAgentIsACommAccThenOnlyClientBGSShouldBeRetrieved
        'End (Arul Stephen A) - (Tech Spec - UIIC WR6 -Policy Get Bank Guarantee.doc) - (7.1.5.4)
		'Start - Prakash - WPR85 Parelleling
        InCaseOfDirectBusinessNoAgentWillBeavailable
        InCaseIfAgentIsABrokerThenOnlyAgentCDsShouldbeRetrieved
        InCaseIfAgentIsACommAccThenOnlyClientCDSShouldBeRetrieved
        'End - Prakash - WPR85 Parelleling


        InAbsenceOfRenewalCertificateDocument
        InAbsenceOfRenewalScheduleDocument
        InAbsenceOfRenewalDebitNoteDocument

    End Enum

    Public Enum SAMComponentAction
        PMView = 0
        PMAdd = 1
        PMEdit = 2
        PMDelete = 3
        PMDummyDelete = 4
        PMAdded = 10
        PMReverse = 11
        PMReplace = 12
        PMCopy = 20
    End Enum

    Public Enum SAMInvalidData
        GeneralFailure = 0
        BackofficeComponentFailed = 11
        MandatoryInputMissing = 100
        InvalidDateFormat = 101
        InvalidLookupListValue = 102
        InvalidFormat = 103
        RecordLockedByAnotherUser = 200
        RecordNotLockedByCurrentUser = 201
        BackOfficeUnavailable = 202
        UserNotAuthorisedToActOnData = 203
        SecurityCheckFailed = 204
        TokenExpired = 205
        RecordChanged = 206
        BranchCodeInvalid = 210
        BranchMismatch = 211
        PolicyMismatch = 212
        PartyDOBIsInFuture = 213
        PartyDOBIsTooOld = 214
        PolicyRiskLinkRecordNotFound = 219
        QuoteHeaderRecordNotFound = 220
        CoverStartDateIsInThePast = 221
        CoverEndDateIsBeforeCoverStartDate = 222
        PartyRecordNotFound = 223
        PolicyRecordNotFound = 224
        FailedToLoadRiskDB = 225
        FailedToRetrievePremiumDetails = 226
        ListTypeNotFound = 227
        AddressRecordNotFound = 228
        RiskRecordNotFound = 229
        DefaultXMLFileNotAvailable = 230
        DefaultXMLFilePathNotFound = 232
        DefaultXMLFileFailedToLoad = 233
        DefaultXMLFilePathTooLong = 234
        ConfigurationFileNotAvailable = 240
        ConfigurationFilePathNotFound = 242
        ConfigurationFileFailedToLoad = 243
        ConfigurationFilePathTooLong = 244
        XMLDocumentBadlyFormed = 245
        FailedToCreateBackofficeComponent = 250
        FailedToInitialiseBackofficeComponent = 251
        BackofficeFailed = 252
        FailedToConnectToTheSiriusDatabase = 253
        SQLServerReturnedAnError = 254
        FailedToRetrieveDatamodelCodeFromXml = 260
        XmldatasetBadlyFormed = 261
        DatasetPathRegistrySettingNotFound = 262
        FailedToAddRiskRecord = 263
        FailedToMergeRiskDataset = 264
        UserAuthorityLevelsCheckFailed = 265
        ValidationRulesFailed = 266
        FailedToQuoteTheRisk = 267
        FailedToSaveRiskToDatabase = 268
        SchemeVersionNumberMissing = 269
        SchemaForVersionMissing = 270
        FileNotFound = 271
        RecordNotFound = 272
        StatusOfRiskPreventsDeletion = 273
        AgentRecordNotFound = 274
        BackofficeComponentReturnedRecordInUse = 810
        BackofficeComponentReturnedNotFound = 811
        BrokerOrSchemeInvalid = 812
        ValidationRulesReferred = 275
        ValidationRulesDeclined = 276
        UALRulesReferred = 277
        UALRulesDeclined = 278
        RatingRulesReferred = 279
        RatingRulesDeclined = 280
        LoginFailureIncorrectUsername = 290
        LoginFailureIncorrectPassword = 291
        LoginFailureLoggedInElsewhere = 292
        LoginFailureNotLinkedToAgent = 293
        UserDoesNotExist = 294
        NoEmailAddressDefinedForUser = 295
        UserUpdateFailed = 296
        StartDateIsInThePast = 297
        EndDateIsBeforeStartDate = 298
        InvalidClaimAndClaimPerilLink = 299
        TransactionAmountIsZero = 300
        OtherPartyTypeCodeDoesntStartWithOT
        SupplierBusinessShouldNotBeSpecifiedWhenThePartyIsNotASupplier
        SelectedPayeeRequiresAnIsWithholdingTaxTaxGroup
        SelectedPayeeRequiresATaxGroupWhichIsNotAnIsWithholdingTaxTaxGroup
        ClaimReceiptInvalidDataLinksProvidedNoLinkExistsBetweenClaimAndClaimPerilAndRecovery
        InvalidPartyKeySpecified
        ReceiptPartyTypeCLMRECEIVABLESpecifiedPartyKeyShouldBeZero
        PaymentPartyTypeCLMPAYABLESpecifiedPartyKeyShouldBeZero
        InvalidPartyTypeKeySpecifiedNoAgentDetailsFoundForClaim
        SpecifiedPartyTypeShouldHaveAPartyKeyofZero
        InvalidTaxGroupSpecified
        GisScreenAssociatedWithSpecifiedPartyTypeIsOfTheWrongType
        TransactionTypeIsInvalid
        SelectedInstalmentQuoteDataIsInvalid
        ValueOutOfAcceptableRange
        'Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.5)
        CodeAlreadyExists
        'End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR01 - User Access - Add Task Group.doc)-(7.1.5.5)
        'Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        InValidAllocationKeySuppliedForNewAllocation
        'End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)

        'Start (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.1.1)
        'For the time being, BaseReceiptCashListType is not handled by this application. 
        InvalidCashListType
        InvalidCashListItemType
        ReceiptCashListProcessingIsNotSupported
        ReceiptCashListItemProcessingIsNotSupported
        'For the time being, this application only supports creating a new cashlist. 
        'Updation of existing cashlist and cashlist item are not supported.
        InvalidCashListKeySpecified
        CashListUpdationIsNotSupported
        CashListItemUpdationIsNotSupported
        'Account chosen for payment and the bank account should not be the same
        PaymentAccountAndBankAccountAreSame
        'MediaReference already in use
        MediaReferenceAlreadyUsed
        'End (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.1.1)
        'Start (Arul Stephen A) - (Tech Spec - UIIC WR6 - Add Bank Guarantee.doc) - (7.1.4.5)
        InvalidExpiryDate
        'End (Arul Stephen A) - (Tech Spec - UIIC WR6 - Add Bank Guarantee.doc) - (7.1.4.5)
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (6.1.2)
        InvalidWildcardSearch
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (6.1.2)
        InvalidInsuranceFileType
        InvalidInsuranceFile
        DebtorUserGroupsAreNotSetup
    End Enum

    Public Enum SAMErrorCodes As Long

        GeneralFailure = 0
        BackofficeComponentFailed = 11
        MandatoryInputMissing = 100
        InvalidDateFormat = 101
        InvalidLookupListValue = 102
        InvalidFormat = 103
        RecordLockedByAnotherUser = 200
        RecordNotLockedByCurrentUser = 201
        BackOfficeUnavailable = 202
        UserNotAuthorisedToActOnData = 203
        SecurityCheckFailed = 204
        TokenExpired = 205
        RecordChanged = 206
        BranchCodeInvalid = 210
        BranchMismatch = 211
        PolicyMismatch = 212
        PartyDOBIsInFuture = 213
        PartyDOBIsTooOld = 214
        PolicyRiskLinkRecordNotFound = 219
        QuoteHeaderRecordNotFound = 220
        CoverStartDateIsInThePast = 221
        CoverEndDateIsBeforeCoverStartDate = 222
        PartyRecordNotFound = 223
        PolicyRecordNotFound = 224
        FailedToLoadRiskDB = 225
        FailedToRetrievePremiumDetails = 226
        ListTypeNotFound = 227
        AddressRecordNotFound = 228
        RiskRecordNotFound = 229
        DefaultXMLFileNotAvailable = 230
        DefaultXMLFilePathNotFound = 232
        DefaultXMLFileFailedToLoad = 233
        DefaultXMLFilePathTooLong = 234
        ConfigurationFileNotAvailable = 240
        ConfigurationFilePathNotFound = 242
        ConfigurationFileFailedToLoad = 243
        ConfigurationFilePathTooLong = 244
        XMLDocumentBadlyFormed = 245
        FailedToCreateBackofficeComponent = 250
        FailedToInitialiseBackofficeComponent = 251
        BackofficeFailed = 252
        FailedToConnectToTheSiriusDatabase = 253
        SQLServerReturnedAnError = 254
        FailedToRetrieveDatamodelCodeFromXml = 260
        XmldatasetBadlyFormed = 261
        DatasetPathRegistrySettingNotFound = 262
        FailedToAddRiskRecord = 263
        FailedToMergeRiskDataset = 264
        UserAuthorityLevelsCheckFailed = 265
        ValidationRulesFailed = 266
        FailedToQuoteTheRisk = 267
        FailedToSaveRiskToDatabase = 268
        SchemeVersionNumberMissing = 269
        SchemaForVersionMissing = 270
        FileNotFound = 271
        RecordNotFound = 272
        StatusOfRiskPreventsDeletion = 273
        AgentRecordNotFound = 274
        BackofficeComponentReturnedRecordInUse = 810
        BackofficeComponentReturnedNotFound = 811
        BrokerOrSchemeInvalid = 812
        ValidationRulesReferred = 275
        ValidationRulesDeclined = 276
        UALRulesReferred = 277
        UALRulesDeclined = 278
        RatingRulesReferred = 279
        RatingRulesDeclined = 280
        LoginFailureIncorrectUsername = 290
        LoginFailureIncorrectPassword = 291
        LoginFailureLoggedInElsewhere = 292
        LoginFailureNotLinkedToAgent = 293
        UserDoesNotExist = 294
        NoEmailAddressDefinedForUser = 295
        UserUpdateFailed = 296
        StartDateIsInThePast = 297
        EndDateIsBeforeStartDate = 298
    End Enum

End Class
