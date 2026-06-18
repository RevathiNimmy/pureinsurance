namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseReceiptType
    {

        public int TransactionKey { get; set; }

        public int CashListKey { get; set; }

        public string AccountShortCode { get; set; }

        public string UserName { get; set; }

        public int CCCashListItemBankKey { get; set; }

        public int CCTypeKey { get; set; }

        public int CCInsuranceFileKey { get; set; }

        public int CashListItemKey { get; set; }

        public int AccountId { get; set; }

        public int SubBranchKey { get; set; }

        public int SourceKey { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string AllocationStatusCode { get; set; }
        public double Amount { get; set; }
        public BaseBankReceiptType Bank { get; set; }
        public string BankAccountName { get; set; }
        public string CCAuthCode { get; set; }
        public string CCCashListItemBankCode { get; set; }
        public string CCCustomer { get; set; }
        public string CCExpiryDate { get; set; }
        public string CCIssue { get; set; }
        public string CCManualAuthCode { get; set; }
        public string CCName { get; set; }
        public string CCNumber { get; set; }
        public string CCPin { get; set; }
        public string CCStartDate { get; set; }
        public string CCTrackingNumber { get; set; }
        public string CCTransactionCode { get; set; }
        public string CCTransactionSlipNumber { get; set; }
        public string CCTypeCode { get; set; }
        public string CashListRef { get; set; }
        public System.DateTime ChequeDate { get; set; }
        public bool ChequeDateSpecified { get; set; }
        public string ChequeName { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public bool CollectionDateSpecified { get; set; }
        public string Comments { get; set; }
        public string ContactName { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string MediaReference { get; set; }
        public string MediaTypeCode { get; set; }
        public string MediaTypeIssuerCode { get; set; }
        public string OurReference { get; set; }
        public int PartyBankKey { get; set; }
        public string PaymentStatusCode { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PostalCode { get; set; }
        public string ReceiptTypeCode { get; set; }
        public string SubbranchCode { get; set; }
        public string TheirReference { get; set; }
        public System.DateTime TransactionDate { get; set; }

        public int CurrencyKey { get; set; }

        public int CashListItemReceiptTypeKey { get; set; }

        public int MediaTypeKey { get; set; }

        public int BankAccountKey { get; set; }

        public int MediaTypeIssuerKey { get; set; }

        public int CountryKey { get; set; }

        public int PaymentStatusKey { get; set; }

        public int PaymentTypeKey { get; set; }

        public int AllocationStatusKey { get; set; }

        public string AccountCode { get; set; }

        public int ccCashListItemBankID { get; set; }

        public int AgentKey { get; set; }

        public decimal? CurrencyBaseXrate { get; set; }
        public decimal? AccountBaseXrate { get; set; }
        public System.DateTime? CurrencyBaseDate { get; set; }
        public System.DateTime? AccountBaseDate { get; set; }
        public System.DateTime? SystemBaseDate { get; set; }
        public decimal SystemBaseXrate { get; set; }
        public int? OverrideReason { get; set; }
    }
}
