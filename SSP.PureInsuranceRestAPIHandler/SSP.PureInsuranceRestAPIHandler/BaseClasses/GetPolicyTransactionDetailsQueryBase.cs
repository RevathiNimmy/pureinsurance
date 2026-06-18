namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPolicyTransactionDetails
{
    public class GetPolicyTransactionDetailsQueryBase : BaseRequestType
    {
        public System.DateTime DueByDate { get; set; }
        public bool DueByDateSpecified { get; set; }
        
        public int InsuranceFolderKey { get; set; }
        public bool OnlyOutstanding { get; set; }
    }
}
