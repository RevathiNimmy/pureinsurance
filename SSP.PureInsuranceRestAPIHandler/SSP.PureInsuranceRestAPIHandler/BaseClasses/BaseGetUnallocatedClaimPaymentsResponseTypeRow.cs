namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUnallocatedClaimPaymentsResponseTypeRow
    {
        //[DBCol("account_amount")]
        public double AccountAmount { get; set; }
        //[DBCol("AccountCode")]
        public string AccountCode { get; set; }
        //[DBCol("account_currency_id")]
        public int AccountCurrencyKey { get; set; }
        //[DBCol("account_id")]
        public int AccountKey { get; set; }
        //[DBCol("account_name")]
        public string AccountName { get; set; }
        //[DBCol("amount")]
        public double Amount { get; set; }
        //[DBCol("amount_currency_id")]
        public int AmountCurrencyKey { get; set; }
        //[DBCol("BankAccountCode")]
        public string BankAccountCode { get; set; }
        //[DBCol("base_claim_payment_id")]
        public int BaseClaimPaymentKey { get; set; }
        //[DBCol("base_currency_description")]
        public string BaseCurrencyDescription { get; set; }
        //[DBCol("base_format_string")]
        public string BaseCurrencyFormatString { get; set; }
        //[DBCol("CashListItem_id")]
        public int CashListItemKey { get; set; }
        //[DBCol("claim_number")]
        public string ClaimNumber { get; set; }
        //[DBCol("BranchCode")]
        public string ClaimPaymentBranchCode { get; set; }
        //[DBCol("MaxClaimPaymentKey")]
        public int ClaimPaymentKey { get; set; }
        //[DBCol("currency_amount")]
        public double CurrencyAmount { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("currency_description")]
        public string CurrencyDescription { get; set; }
        //[DBCol("currency_format_string")]
        public string CurrencyFormatString { get; set; }
        //[DBCol("currency_id")]
        public int CurrencyKey { get; set; }
        //[DBCol("date_of_payment")]
        public System.DateTime DateOfPayment { get; set; }
        //[DBCol("comment")]
        public string DocumentComment { get; set; }
        //[DBCol("document_date")]
        public System.DateTime DocumentDate { get; set; }
        //[DBCol("document_id")]
        public int DocumentKey { get; set; }
        //[DBCol("document_ref")]
        public string DocumentRef { get; set; }
        //[DBCol("claim_payment_id")]
        public int MaxClaimPaymentKey { get; set; }
        //[DBCol("MediaTypeCode")]
        public string MediaTypeCode { get; set; }
        //[DBCol("MediaTypeDesc")]
        public string MediaTypeDesc { get; set; }
        //[DBCol("our_ref")]
        public string OurRef { get; set; }
        //[DBCol("PartyBankId")]
        public int PartyBankId { get; set; }
        //[DBCol("")]
        public string PayeeAccountNo { get; set; }
        //[DBCol("payee_media_type")]
        public int PayeeMediaTypeKey { get; set; }
        //[DBCol("PayeeName")]
        public string PayeeName { get; set; }
        //[DBCol("PayeeSortCode")]
        public string PayeeShortCode { get; set; }
        //[DBCol("Status")]
        public string Status { get; set; }
        //[DBCol("ThirdPartyReference")]
        public string TheirRef { get; set; }
        //[DBCol("Media_ref")]
        public string MediaRef { get; set; }
    }
}
