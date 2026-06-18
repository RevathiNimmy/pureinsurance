namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetailsEx
{
    public class GetTransactionDetailsExQueryBase : BaseRequestType
    {
        public System.Collections.Generic.List<BaseGetTransactionDetailsExRequestTypeAllocation> Allocation { get; set; }
    }
}
