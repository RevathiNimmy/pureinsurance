namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimDetailsType
    {
        public BaseGetClaimDetailsTypeClaimDetails ClaimDetails { get; set; }
        public BaseGetClaimPerilDetailsType[] ClaimPeril { get; set; }
    }
}