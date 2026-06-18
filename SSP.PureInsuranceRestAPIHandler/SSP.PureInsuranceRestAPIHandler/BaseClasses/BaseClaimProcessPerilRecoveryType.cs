namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessPerilRecoveryType
    {
        public string TypeCode { get; set; }
        public decimal Amount { get; set; }
        public string RecoveryPartyTypeCode { get; set; }
        public string RecoveryPartyCode { get; set; }
        public bool IsSalvageRecovery { get; set; }
        public string taxGroupCode { get; set; }
        public bool RecoveryAmountSpecified { get; set; }
        public BaseClaimProcessReceiptDetailsType RecoveryDetails { get; set; }
        public bool isReceiptToDate { get; set; }
        public bool isRecoverToDate { get; set; }
        public decimal RecoveryAmount { get; set; }
    }
}
