namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTransDetailType
    {
        public decimal Amount { get; set; }
        public int CashListItemKey { get; set; }
        public int TransDetailKey { get; set; }

        public int AmountCurrencyId { get; set; }
    }
}
