namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountingPeriod
{
    public class GetAccountingPeriodQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetAccountingPeriodResponseTypeRow> Period { get; set; }
    }
}
