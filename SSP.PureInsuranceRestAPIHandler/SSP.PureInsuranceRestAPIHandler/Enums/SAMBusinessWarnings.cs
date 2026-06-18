namespace SSP.PureInsuranceRestAPIHandler.Enums
{
    public enum SAMBusinessWarnings
    {
        RiskIsDeferred = 2000001,
        PolicyIsVoid = 2000002,
        LossFromDateBeforePolicyStartDate = 2000003,
        LossFromDateAfterPolicyEndDate = 2000004,
        PolicyIsDifferent = 2000005,
        InfoOnlyClaimDataTruncated = 2000006,
        LossDateChanged,
        UserDoesntHaveAuthorityToMakeThisPaymentPaymentWasReferred,
        UserHaveAuthorityToMakeThisPaymentPayment = 2000007,
        MTAEffectiveDateIsPriorToAPreviousTransactionEffectiveDate,

        // Start (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)
        WorkFlowIsDisabledButCashListIsNotEmpty,
        // End (Arul Stephen)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)

        // Start (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.2.4)
        // Payment done to a deleted party
        PaymentToDeletedParty,
        // End (Prakash C Varghese)-(Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc)-(11.6.2.4)

        // Start (Arul Stephen A) - (Tech Spec - UIIC WR6 -Policy Get Bank Guarantee.doc) - (7.1.5.4)
        InCaseOfDirectBusinessNoAgentWillBeTheir,
        InCaseIfAgentIsABrokerThenOnlyAgentBGsShouldbeRetrieved,
        InCaseIfAgentIsACommAccThenOnlyClientBGSShouldBeRetrieved,
        // End (Arul Stephen A) - (Tech Spec - UIIC WR6 -Policy Get Bank Guarantee.doc) - (7.1.5.4)

        // Start - Prakash - WPR85 Parelleling
        InCaseOfDirectBusinessNoAgentWillBeavailable,
        InCaseIfAgentIsABrokerThenOnlyAgentCDsShouldbeRetrieved,
        InCaseIfAgentIsACommAccThenOnlyClientCDSShouldBeRetrieved,
        // End - Prakash - WPR85 Parelleling

        InAbsenceOfRenewalCertificateDocument,
        InAbsenceOfRenewalScheduleDocument,
        InAbsenceOfRenewalDebitNoteDocument,
        ClaimIsAlreadyLinkedToOneCase,
        GeneralFailure,
        DocumentIsAlreadyReversed
    }

}
