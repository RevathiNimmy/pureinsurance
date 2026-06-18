namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTransactionDetailsRequestType : BaseRequestType
    {
        public int AccountKey { get; set; }


        public bool AccountKeySpecified { get; set; }

        public BaseGetTransactionDetailsRequestTypeAllocation Allocation { get; set; }

        public System.DateTime AccountingDate { get; set; }

        public string InsuranceRef { get; set; }

        public bool IsNewPF { get; set; }

        public bool IsOutstandingOnly { get; set; }

        public string ShortCode { get; set; }

        public bool IncludeReversedTran { get; set; }

    }

}
