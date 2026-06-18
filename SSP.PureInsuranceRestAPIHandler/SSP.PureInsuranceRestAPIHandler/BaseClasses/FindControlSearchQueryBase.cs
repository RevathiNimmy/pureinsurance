namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindControlSearchQueryBase : BaseRequestType
    {
        public int FindControlKey { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public BaseSearchCriteriaType[] SearchCriteria { get; set; } = new BaseSearchCriteriaType[0];
    }
}
