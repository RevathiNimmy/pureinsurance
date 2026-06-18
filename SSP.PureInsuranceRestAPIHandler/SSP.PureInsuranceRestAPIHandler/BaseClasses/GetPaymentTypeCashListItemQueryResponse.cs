namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentTypeCashListItem
{
    public class GetPaymentTypeCashListItemQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BasePaymentCashListType> CashList { get; set; }
    }
}
