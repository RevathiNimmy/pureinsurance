namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingSectionTypesQueryBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }

        public int RiskKey { get; set; }
    }
}
