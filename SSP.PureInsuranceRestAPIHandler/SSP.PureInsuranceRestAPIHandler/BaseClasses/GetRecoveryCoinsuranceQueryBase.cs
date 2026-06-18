namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRecoveryCoinsuranceQueryBase : BaseRequestType
    {
        public int ClaimPerilKey { get; set; }
        public bool IsSalvage { get; set; }
    }
}
