namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseSettleClaimPaymentType
    {

        public string AccountCode { get; set; }

        public string BankAccountCode { get; set; }

        ////(1, double.MaxValue, ErrorMessage = "The ClaimPaymentAmount field is required")]
        //
        public decimal ClaimPaymentAmount { get; set; }

        public string ClaimPaymentBranchCode { get; set; }
        public int ClaimPaymentKey { get; set; }

        public string CurrencyCode { get; set; }
        public int DocumentKey { get; set; }
        public string DocumentRef { get; set; }

        public string MediaTypeCode { get; set; }
        public string OurRef { get; set; }
        public string PayeeName { get; set; }

        public int ClaimPaymentBranchKey { get; set; }

        public int CurrencyKey { get; set; }

        public int AccountKey { get; set; }
        public string LoginUserName { get; set; }
    }
}
