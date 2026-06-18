namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountingPeriod
{
    public class GetAccountingPeriodQueryBase : BaseRequestType
    {
        public System.DateTime DateInPeriod { get; set; }
        
        public string SubBranchCode { get; set; }
        public bool SubBranchCodeSpecified { get; set; }
    }
}
