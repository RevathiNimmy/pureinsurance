namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingDetailsQueryBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }

        public int RiskKey { get; set; }
    }
}
