namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTransactionDetailsExResponseTypeTransactionsExtended
    {
        public System.DateTime DueDate { get; set; }
        public int ExtendedId { get; set; }
        public decimal DueAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
    }

}
