namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindReinsurerRequest : BaseRequestType
    {
        public string FileCode { get; set; }
        public bool IncludeClosedBranches { get; set; }
        public bool IsBroker { get; set; }
        public bool IsBrokerSpecified { get; set; }
        public bool IsFAX { get; set; }
        public bool IsFAXSpecified { get; set; }
        public bool IsRetained { get; set; }
        public bool IsRetainedSpecified { get; set; }
        public string RICode { get; set; }
        public string RIName { get; set; }
        public string RITypeCode { get; set; }
    }
}
