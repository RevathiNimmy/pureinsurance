namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentCashListDetails
{
    public class GetPaymentCashListDetailsQueryBaseResponse : BasePagedResponse
    {
        public BasePaymentCashListType PaymentCashList { get; set; }
    }
}
