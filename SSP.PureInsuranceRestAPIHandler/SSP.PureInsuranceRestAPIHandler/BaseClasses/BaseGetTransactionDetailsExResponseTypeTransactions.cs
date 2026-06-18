using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTransactionDetailsExResponseTypeTransactions
    {
        public byte[] AllocationTimeStamp { get; set; } = new byte[0];
        public long TransDetailKey { get; set; }
        public string DocumentReference { get; set; }
        public string AccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public System.Collections.Generic.List<BaseGetTransactionDetailsExResponseTypeTransactionsExtended> Extended { get; set; }
    }

}
