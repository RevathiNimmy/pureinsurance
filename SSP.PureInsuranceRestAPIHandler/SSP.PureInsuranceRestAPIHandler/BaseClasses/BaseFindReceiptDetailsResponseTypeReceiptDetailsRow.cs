namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindReceiptDetailsResponseTypeReceiptDetailsRow
    {
        //[DBCol("AccountNumber")]
        public int AccountNumber { get; set; }
        //[DBCol("Allocated")]
        public bool Allocated { get; set; }
        //[DBCol("AllowReverseAllocation")]
        public int AllowReverseAllocation { get; set; }
        //[DBCol("Amount")]
        public decimal Amount { get; set; }
        //[DBCol("BankAccount")]
        public string BankAccount { get; set; }
        //[DBCol("BankAccountID")]
        public int BankAccountID { get; set; }
        //[DBCol("BatchReference")]
        public string BatchReference { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("CancellationDate")]
        public System.DateTime CancellationDate { get; set; }
        //[DBCol("CancellationReason")]
        public string CancellationReason { get; set; }
        //[DBCol("CashListItemKey")]
        public int CashListItemKey { get; set; }
        //[DBCol("ClaimKey")]
        public int ClaimKey { get; set; }
        //[DBCol("ClientCode")]
        public string ClientCode { get; set; }
        //[DBCol("CurrencyKey")]
        public int CurrencyKey { get; set; }
        //[DBCol("DocumentReference")]
        public string DocumentReference { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("MediaReference")]
        public string MediaReference { get; set; }
        //[DBCol("MediaType")]
        public string MediaType { get; set; }
        //[DBCol("OurReference")]
        public string OurReference { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PayeeName")]
        public string PayeeName { get; set; }

        //[DBCol("PolicyClaimNumber")]
        public string PolicyClaimNumber { get; set; }
        //[DBCol("PolicyHolder")]
        public string PolicyHolder { get; set; }
        //[DBCol("ReceiptAccountCode")]
        public string ReceiptAccountCode { get; set; }
        //[DBCol("ReceiptBranchCode")]
        public string ReceiptBranchCode { get; set; }
        //[DBCol("ReceiptCode")]
        public string ReceiptCode { get; set; }
        //[DBCol("ReceiptDate")]
        public System.DateTime ReceiptDate { get; set; }
        //[DBCol("ReceiptStatus")]
        public string ReceiptStatus { get; set; }
        //[DBCol("ReceiptStatusCode")]
        public string ReceiptStatusCode { get; set; }
        //[DBCol("ReceiptType")]
        public string ReceiptType { get; set; }
        //[DBCol("ReverseAllocationDays")]
        public int ReverseAllocationDays { get; set; }
        //[DBCol("ReverseReasonKey")]
        public int ReverseReasonKey { get; set; }
        //[DBCol("TheirReference")]
        public string TheirReference { get; set; }
        //[DBCol("TransDetailKey")]
        public int TransDetailKey { get; set; }
        //[DBCol("User")]
        public string User { get; set; }
    }
}
