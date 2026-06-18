namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRecoveryReinsuranceQueryBase : BaseRequestType
    {
        public int ClaimPerilKey { get; set; }
        public bool IsSalvage { get; set; }
    }
}
