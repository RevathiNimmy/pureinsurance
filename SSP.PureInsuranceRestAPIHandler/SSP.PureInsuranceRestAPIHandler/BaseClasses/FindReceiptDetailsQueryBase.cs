namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindReceiptDetails
{
    public class FindReceiptDetailsQueryBase : BaseRequestType
    {
        public decimal AmountRangeFrom { get; set; }
        public bool AmountRangeFromSpecified { get; set; }
        public decimal AmountrangeTo { get; set; }
        public bool AmountrangeToSpecified { get; set; }
        public string BankAccountCode { get; set; }
        public string BatchReference { get; set; }
        public string ClientAccountNumber { get; set; }
        public string ClientCode { get; set; }
        public System.DateTime DateFrom { get; set; }
        public bool DateFromSpecified { get; set; }
        public System.DateTime DateTo { get; set; }
        public bool DateToSpecified { get; set; }
        public string DocumentReference { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string MediaReferenceFrom { get; set; }
        public string MediaReferenceTo { get; set; }
        public string MediaType { get; set; }
        public string PayeeName { get; set; }
        public string PolicyClaimNumber { get; set; }
        public string ReceiptBranch { get; set; }
        public string ReceiptStatus { get; set; }
        public int ShowOnlyOutStanding { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public int AgentKey { get; set; }
    }
}
