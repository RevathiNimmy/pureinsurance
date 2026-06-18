namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RequestTypeBase
    {
        public string BranchCode { get; set; }


        public int SourceId { get; set; }
        public bool SkipPolicyTypeCheck { get; set; }


        public string LoginUserName { get; set; } = string.Empty;


        public string Route { get; set; }
    }
}
