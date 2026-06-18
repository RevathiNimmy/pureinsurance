namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckReserveRecoveryQueryBaseResponse : BaseResponseType
    {
        public int ClaimStatus { get; set; }
        public decimal CurrentRecovery { get; set; }
        public decimal CurrentReserve { get; set; }
    }
}
