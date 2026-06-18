namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAllocationDetails
{
    public class GetAllocationDetailsQueryBase : BaseRequestType
    {
        public bool IncludeExtended { get; set; }
        
        public int TransDetailKey { get; set; }
    }
}
