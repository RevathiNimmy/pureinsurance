namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetListofUnapprovedPaymentResponseRowType : BaseResponseType
    {
        //[DBCol("amount")]
        public decimal Amount { get; set; }
        //[DBCol("Assigned_to")]
        public string Assignedto { get; set; }
        //[DBCol("Bank_Account")]
        public string BankAccount { get; set; }
        //[DBCol("base_currency_amount")]
        public decimal BaseCurrencyAmount { get; set; }
        //[DBCol("Branch")]
        public string Branch { get; set; }
        //[DBCol("cashlist_id")]
        public int CashListId { get; set; }
        //[DBCol("cashListItem_id")]
        public int CashListItemId { get; set; }
        //[DBCol("claim_ref")]
        public string ClaimRef { get; set; }
        //[DBCol("username")]
        public string CreatedBy { get; set; }
        //[DBCol("CurrencyCode")]
        public string Currency { get; set; }
        //[DBCol("Assigned_date")]
        public System.DateTime DateAssigned { get; set; }
        //[DBCol("media_ref")]
        public string MediaRef { get; set; }
        //[DBCol("media_type")]
        public string MediaType { get; set; }
        //[DBCol("account_name")]
        public string PayeeAccountName { get; set; }
        //[DBCol("Payment_type")]
        public string PaymentType { get; set; }
        //[DBCol("Policy_Ref")]
        public string PolicyRef { get; set; }
        //[DBCol("CurrentStatus")]
        public string Status { get; set; }
        //[DBCol("transaction_date")]
        public System.DateTime TransactionDate { get; set; }
    }
}
