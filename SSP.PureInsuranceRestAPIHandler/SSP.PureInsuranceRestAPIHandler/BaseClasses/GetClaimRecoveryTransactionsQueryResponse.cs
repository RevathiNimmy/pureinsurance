using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRecoveryTransactionsQueryResponse
    {
        public List<ClaimRecoveryTransactionResponseItem> Transactions { get; set; }
    }

    public class ClaimRecoveryTransactionResponseItem
    {
        public int TransDetailKey { get; set; }
        public string DocumentReference { get; set; }
        public string ClaimNumber { get; set; }
        public int PartyKey { get; set; }
        public string PartyName { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string CurrencyCode { get; set; }
        public int InsuranceFileKey { get; set; }
        public int ClrTransactionId { get; set; }
        public string DocumentType { get; set; }
    }
}
