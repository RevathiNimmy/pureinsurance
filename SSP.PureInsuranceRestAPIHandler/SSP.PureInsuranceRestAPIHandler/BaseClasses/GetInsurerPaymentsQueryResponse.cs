namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetInsurerPayments
{
    public class GetInsurerPaymentsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetInsurerPaymentsResponseTypeRow> InsurerPayments { get; set; }
    }
}
