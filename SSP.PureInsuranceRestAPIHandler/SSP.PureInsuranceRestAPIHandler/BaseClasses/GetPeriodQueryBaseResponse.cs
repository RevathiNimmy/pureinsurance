namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPeriod
{
    public class GetPeriodQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetPeriodResponseTypeRow> Periods { get; set; }
    }
}
