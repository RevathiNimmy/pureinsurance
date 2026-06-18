namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyStatusForMediaTypeStatus
    {
        public bool IsClaimPaymentInitiated { get; set; }
        public bool IsClaimPaymentInitiatedOnLossDate { get; set; }
        public int IsPolicyCanceled { get; set; }
        public bool IsUnclearedCashListExists { get; set; }
    }
}
