namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseApproveCashListItemRequestType : BaseRequestType
    {
        public int CashListItemKey { get; set; }
        public string Comments { get; set; }
        public byte[] ApiTimeStamp { get; set; }
        public bool Declined { get; set; }
        public int CashListKey { get; set; }
        public decimal Amount { get; set; }
        public int CreatorUserKey { get; set; }
        public string Reference { get; set; }
        public int AccountKey { get; set; }
        public bool CheckValidationOnly { get; set; }
    }
}
