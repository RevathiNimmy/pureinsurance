namespace SSP.PureInsuranceRestAPIHandler.Enums
{
    public enum SAMBusinessError
    {
        // pre existing business error constants
        RecordLockedByAnotherUser = 200,
        RecordNotLockedByCurrentUser = 201,
        RecordChanged = 206,

        // new business error constants in range (1000000 - 2000000)
        DuplicateClaimExists = 1000001,
        LossFromDataInFuture,
        LossFromDateAfterReportedDate,
        LossFromDateAfterLossToDate,
        ReportedDateInFuture,
        PolicyDataMissing,
        ClaimDocumentsAlreadyProduced,
        NumberingSchemeNotFound,
        ProductsNumberingSchemesNotFound,
        AutoClaimNumberingDisabled,
        AgentRecordNotFound,
        InsuranceFileDetailsNotFound,
        AutoReinsuranceFailed,
        TransactionTypeNotFound,
        AccountsProcessingFailed,
        DMEFolderCreationFailed,
        FailedToCreateCOMComponent,
        FailedToInitialiseCOMComponent,
        COMComponentMethodFailed,
        CopyClaimFailed,
        InfoOnlyClaim,
        ClaimPaymentIsNotAllowedAgainstAnInfoOnlyClaim,
        ClaimReceiptIsNotAllowedAgainstAnInfoOnlyClaim,
        MediaTypeMandatory,
        DefferedReinsurance,
        AgentOnlyMethodUserIsNotAValidAgent,
        LessThanTwoTransactionsInTransactionArray,
        AddDocumentTransactionsFailed,
        TransactionAmountsDoNotBalance,
        PolicyNumberIsAlreadyInUse,
        UseOfAdvancedTaxScriptRequiresMandatoryInsurerTaxNumber,
        UseOfAdvancedTaxScriptRequiresMandatoryInsuredPercentage,
        UseOfAdvancedTaxScriptRequiresMandatoryInsuredPercentageAndInsurerTaxNumber,
        PostingOfPaymentToAccountsFailedMissingClassOfBusinessCode,
        PostingOfReceiptToAccountsFailedMissingClassOfBusinessCode,
        UseOfAdvanceTaxScriptSystemOptionEnforcedMandatoryAdvanceTaxDetails,
        NoSystemCurrenyHasBeenDefined,
        NoAccountCurrencyHasBeenDefined,
        NoAccountCurrencyConversionRatesHaveBeenDefined,
        NoSystemCurrencyConversionRatesHaveBeenDefined,
        DoCurrencyConversionReturnedInvalidStatus,
        ProductOptionPreventCancelledAgentsMakingClaimPaymentsEnforcedAsAgentOnThisClaimIsCancelled,
        ValidationOfScriptGeneratedTaxCalculationItemsFailed,
        CurrentIdLookupUsingBaseIdAndVersionIDFailed,
        FailedToAddTaxCalculationItem,
        FailedToAddClaimReceiptItem,
        FailedToAddClaimReceipt,
        FailedToGetCurrencyToBaseExchangeRate,
        WorkClaimPerilsInXMLDatasetDoNotMatchPerilsInCDTClaimPeril,
        GisScreenNotSetupForSpecifiedPartyType,
        DataTransferAddPartyRiskDatasetFailedAsGetDataSetFailedToReturnAnNewPartyDataset,
        FutureAdjustmentFound,
        OverlapsWithTemporaryMTA,
        NoPolicyVersionFoundForThisMTAEffectiveDate,
        ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowPermanentMTAs,
        ThisPolicyVersionIsAttachedToAClosedBranchWhichDoesntAllowTemporaryMTAs,
        MTAEffectiveDateIsBeforeOriginalCoverFromDateOfPolicy,
        BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDate,
        ClaimHasBeenLodgedAfterTheEffectiveDateOfThisMTA,
        MTAEffectiveDateIsPriorToAPreviousTransactionEffectiveDate,
        ThisPolicyIsInRenewal,
        MultiplePaymentsOrReceiptsSpecifiedAgainstASingleClaimVersion,
        NotAllSelectedRisksHaveBeenSuccessfullyQuoted,
        ReinsuranceHasNotBeenFullyAssigned,
        ThereAreNoRisksOnThePolicy,
        AtLeastOneRiskHasOutstandingQuestions,
        AtLeastOneQuotedRisksMustBeSelected,
        PolicyCannotBeMadeLiveAsTheLeadAgentsAccountHasBeenStopped,
        PolicyCannotBeMadeLiveAsTheInsuredsAccountHasBeenStopped,
        NoMatchingInstalmentQuoteFoundForSpecifiedDetails,
        TotalCommissionMoreThanPremium,
        TotalCommissionReturnIsMoreThanPremiumReturn,
        InstalmentsIsNotAValidPaymentOptionWhenCancellingAPolicy,
        ReturnPremiumIsGreaterThanBilledPremium,
        CoverNoteBookNumberNotSpecified,
        CoverNoteSheetNumberNotSpecified,
        CoverNoteBookNumberIsInvalid,
        CoverNoteSheetNumberIsOutOfRange,
        CoverNoteNumberNotAssignedToSelectedAgent,
        CoverNoteNumberNotAssignedToSelectedProduct,
        CoverNoteNumberNotAssignedToSelectedBranch,
        CoverNoteNumberEnteredIsNotAValidNumber,
        CoverNoteStatusMissingPolicyCannotBeIssued,
        CoverNoteStatusCancelledPolicyCannotBeIssued,
        CoverNoteNumberAlreadyConvertedToPolicy,
        ClaimDataTransferFailedToUpdateReserveItem,
        ClaimDataTransferFailedToUpdateRecoveryItem,
        PartyTypeNotFound,

        // Start (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.5)
        CanNotUpdateLivePolicyDetails,
        // End (Girija chokkalingam) - (Tech Spec - UIIC WR22 – Capture Quote Details – Sub Agents.doc) - (7.2.5.5)
        // Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        ManualAllocationNotPossibleInAutoAllocation,
        CashListIsEmptyButWorkFlowEnabled,
        // End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        // Start (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR01 – 64VB (SAM) -  Pre Payment Functionality)- (7.1.3.7)
        CollectionDateError,
        // End (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR01 – 64VB (SAM) -  Pre Payment Functionality)- (7.1.3.7)
        // Start (PraveenGora) - (Tech Spec - UIIC WR01 - 64VB (SAM) - Cash Receipt.doc) - (7.1.2.2.1)
        InvalidChequeDate,
        MissingComment,
        // End (PraveenGora) - (Tech Spec - UIIC WR01 - 64VB (SAM) - Cash Receipt.doc) - (7.1.2.2.1)
        // Start (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (7.1.4.6)
        InvalidOldCoverNoteSheetNumber,
        InvalidNewCoverNoteSheetNumber,
        // End (PraveenGora) - (Tech Spec - UIIC WR53 - Cover Note Maintenance - Update Cover Note Sheet.doc) - (7.1.4.6)

        // Start (Udaya Bhaskar) - Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly) -7.1.2.3.5
        BankGuaranteeNonIssued,
        BankGuaranteeNotToDelete,
        // End (Udaya Bhaskar) - Tech Spec - UIICWR6 - Update Bank Guarantee Conditionaly) -7.1.2.3.5

        // Start (Arul Stephen) - (Tech Spec - UIICWR6 - Policy Get Bank Guarantee.doc) -(7.1.5.4)
        InValidInsuranceFileCnt,
        // End (Arul Stephen) - (Tech Spec - UIICWR6 - Policy Get Bank Guarantee.doc) -(7.1.5.4)
        // Start (Arul Stephen) - (Tech Spec - UIICWR6 - New Payment Option at Policy Level.doc) -(7.1.4.4)
        InValidBankGuaranteeKey,
        InValidAvailableBalance,
        InValidProductKey,
        InValidBranchId,
        InValidCurrency,
        InValidCoverFromDate,
        RecordIsDeleted,
        InValidBankGuaranteeStatus,
        InValidPartyAgent,
        InValidUser,
        InValidProduct,
        // End (Arul Stephen) - (Tech Spec - UIICWR6 - New Payment Option at Policy Level.doc) -(7.1.4.4)
        // Start (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.4)
        BankGuaranteeSearchFailure,
        // Start (Girija chokkalingam) - (Tech Spec - UIIC WR6 Bank Guarantee – Find Bank Guarantee.doc) - (7.1.4.4)

        // Start (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR33 - Work Manager - Create Task(Additional Changes).doc)- (7.1.4.5)
        InvalidDueDate,
        // End (Vijayakumar Ramasamy) -(Tech Spec - UIIC WR33 - Work Manager - Create Task(Additional Changes).doc)- (7.1.4.5)

        // Start (Prakash C Varghese) - (This change is done to make the merged code to work. copied from version 1.12)
        RenewalStatusTypeIsInvalidForThisAction,
        InvalidTransactionType,
        InsuranceFileDetailsIsNotValidToGenerateInvite,
        NoRenewalStatusRecordExistsForTheSpecifiedPolicy,
        TrueMonthlyPolicyAnniversaryRenewalCannotBeProcessedUntilTheLastMonthlyCycleHasBeenAccepted,
        PolicyAlreadyAccepted,
        RenwalInviteAlreadyPrinted,
        PolicyNotValidTrueMonthlyPolicyUntilAnniversaryDateReachedtoGenerateInvite,
        UserDontHaveCollectionDateOverrideAccess,
        // End (Prakash C Varghese) - (This change is done to make the merged code to work. copied from version 1.12)
        LapseDateIsBeforeTheCoverStartDate,
        LapseDateIsAfterTheCoverEndDate,
        BackdatedMTANotPermitted,
        BackdatedMTAIsNotPermittedPriorToTheLastPolicyRenewalDatePlusOne,
        BackdatedMTANotPermittedWithClaimsForUser,
        // Start (Ravikumar Pasupuleti) -(Tech Spec - UIIC WR63 - Insurer payments - AddWriteOff.doc)
        FailedToAddWriteOffBecauseSystemOptionHasBeenDisabled,
        // End (Ravikumar Pasupuleti) -(Tech Spec - UIIC WR63 - Insurer payments - AddWriteOff.doc)

        // Start (Udaya Bhaskar) - (Tech Spec - UIIC WR60 - Authorise Payment - Authorise Claim Payment.doc) - (7.1.5.5)
        InvalidReferred,
        // End (Udaya Bhaskar) - (Tech Spec - UIIC WR60 - Authorise Payment - Authorise Claim Payment.doc) - (7.1.5.5)
        // Start (Girija chokkalingam) - (Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc)- (7.1.3.7)
        Invalidmediatype,
        // End (Girija chokkalingam) - (Tech Spec - PGR028 - SAM MTA Change Credit Card Details.doc)- (7.1.3.7)
        NoAccountingPeriodFound,
        // Start (Prakash C Varghese)-(PartyBank functionality)
        ErrorInAddPartyBankCollection,
        // End (Prakash C Varghese)-(PartyBank functionality)
        // Start(Saurabh Agrawal)
        InsufficentPlanBalnceToSpreadTheReducedMTA,
        // End(Saurabh Agrawal)
        ClaimNumberAlreadyExists,

        // Start - Renuka - (WPR87 Paralleling)
        NoAccountingPeriodDefinedForTheSelectedDate,
        FailedToGetNextNumberForTheSelectedAccountingPeriod,
        // End - Renuka - (WPR87 Paralleling)
        // Start - Renuka - (WPR64 Paralleling)
        InvalidCommissionRate,
        // End - Renuka - (WPR64 Paralleling)

        // Start - Prakash - WPR85 Parelleling
        PolicyCannotBeMadeLiveAsTheSelectedCashDepositAccountHasBeenStopped,
        MultiplePaymentMethodSpecified,
        CashDepositAccountDoesNotBelongToInsured,
        CashDepositAccountDoesNotBelongToAgent,
        CashDepositAccountDoesNotBelongToAgentAndInsured,
        InsufficientBalanceInCashDepositAccount,
        InsufficientRunningBalanceInCashDepositAccount,
        // End - Prakash - WPR85 Parelleling
        InvalidParty,
        InvalidCertificateYears,
        // Start  (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.4)
        WrittenNotPermittedOnProduct,

        // Added validation for write policy as per Adam's comments
        InsuranceFileTypeIsInvalidForWritePolicy,
        // End - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.4)
        UserDoesNotHaveReverseAllocationAuthority,
        CannotReverseAllocate,
        TemporaryMTAsAreNotAllowedOnThisSystem,
        InvalidCase,
        PendingClaimWithXOLLines,
        RecommendationProcessingFailed,
        ReinstatmentStartDatePriorCancelStartDate,
        ReinstatmentEndDatePriorCancelStartDate,
        ReinstatmentStartDateAfterCancelEndDate,
        ReinstatmentEndDateAfterCancelEndDate,
        FailedToAddAnExclusiveLock,
        FailedToAutoAllocateTransactions,
        ReverseAllocationFailed,
        OutOfSequenceCancellationNotAllowed,
        InstalmentPlanInvalidTransactionType,
        InstalmentPlanNoLivePlan,
        InstalmentPlanVersionNotCorrect,
        InstalmentPlanNotPreLive,
        InstalmentPlanNoOutStandingBalance,
        AccountNumberNotValid,
        AllocationAlreadyExist,
        TransactionSelectedMustBeOfSameTypeAndPolicy,
        InstalmentPlanPostMTAError,
        InvalidSortCode,
        PlanReferenceAlreadyExist,
        UserAuthorityWriteOffAuthorityDisabled,
        UserAuthorityWriteOffAuthorityAmountExceeded,
        NoPolicyVersionFoundOrAlreadyRenewed,
        StepAuthorizationProcessErrorMessage,
        AmountIsCreditType,
        FailedToRaiseClaimTransaction,
        FailedToCreateStatsFolder,
        FailedToAddInStatsDetails,
        FailedToUpdateCoinsuranceDetails,
        FailedToStoreClaimRIArrangements,
        FailedToGetRIArrangementLines,
        FailedToCalculateClaimRI,
        FailedToFinaliseClaimDetails,
        ClaimProcessingFailedToUpdateInsuranceFileSystem,
        FailedtofFinaliseStats,
        FailedToAddClaimInTransactionExportDetails,
        FailedToUpdateExportStatus,
        FailedToUpdatePaymentDocumentDetails,
        FailedToUpdateReceiptDocumentDetails,
        FailedToCreateEventForClaim,
        FailedToUpdateIsDirtyFlag,
        FailedToGetClaimStatus,
        FailedToUpdateClaimReserve,
        FailedToUpdateClaimRecovery,
        FailedToUpdateClaimStatus,
        FailedToSetupClaimGisDetails,
        FailedToGetTransactionType,
        FailedToSetPaymentReferred
    }
}
