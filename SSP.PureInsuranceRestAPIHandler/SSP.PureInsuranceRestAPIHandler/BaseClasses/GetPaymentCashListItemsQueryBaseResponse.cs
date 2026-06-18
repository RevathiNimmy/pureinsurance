namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentCashListItems
{
    public class GetPaymentCashListItemsQueryBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseGetPaymentCashListItemsResponseTypeRow> PaymentCashListItems { get; set; }
    }
}
