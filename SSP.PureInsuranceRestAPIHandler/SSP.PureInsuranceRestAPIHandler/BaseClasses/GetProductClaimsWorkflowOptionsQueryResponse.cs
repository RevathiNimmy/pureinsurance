namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductClaimsWorkflowOptionsQueryResponse : BaseResponseType
    {
        public bool CashPaymentProcess { get; set; }
        public bool CheckDeferredReinsurance { get; set; }
        public bool CheckUnpaidStatus { get; set; }
        public bool ClaimNotificationDocMessage { get; set; }
        public bool ClaimPaymentDocMessage { get; set; }
        public bool ClaimPaymentProcess { get; set; }
        public bool DescriptionForChangeInPayment { get; set; }
        public bool DescriptionForChangeInReserve { get; set; }
        public bool ExternalClaimHandling { get; set; }
        public bool FastTrackClaims { get; set; }
        public bool GenerateClaimNotificationDoc { get; set; }
        public bool GenerateClaimPaymentDoc { get; set; }
        public bool MakeFurtherPayments { get; set; }
        public bool ReinsurancePayment { get; set; }
        public bool ReinsuranceRecovery { get; set; }
        public bool SalvageRecovery { get; set; }
        public bool ThirdPartyRecovery { get; set; }
    }
}
