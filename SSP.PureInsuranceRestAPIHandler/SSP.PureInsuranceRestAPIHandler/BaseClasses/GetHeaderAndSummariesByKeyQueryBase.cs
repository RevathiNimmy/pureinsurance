namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesByKeyQueryBase : BaseRequestType
    {
        public System.DateTime AnniversaryDate { get; set; }
        public bool AnniversaryDateSpecified { get; set; }
        public bool ExclusiveLock { get; set; }
        public bool IncludeGISRetroactiveDate { get; set; }

        public int InsuranceFileKey { get; set; }
        public int PreChangeInsuranceFileKey { get; set; }
        public string SessionValue { get; set; }
    }
}
