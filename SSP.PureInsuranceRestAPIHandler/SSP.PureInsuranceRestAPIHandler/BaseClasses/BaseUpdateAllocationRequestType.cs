namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseUpdateAllocationRequestType : BaseRequestType
    {
        public int AccountKey { get; set; }
        public int TransdetailKey { get; set; }
        public double Amount { get; set; }
        public int CashListItemKey { get; set; }
        public BaseUpdateAllocationRequestTypeAllocation[] Allocation { get; set; } = new BaseUpdateAllocationRequestTypeAllocation[0];
        public double WriteOffAmount { get; set; }
        public bool WriteOffAmountSpecified { get; set; }
        public int WriteOffReason { get; set; }
        public bool WriteOffReasonSpecified { get; set; }
        public double CurrencyDiff { get; set; }
        public bool CurrencyDiffSpecified { get; set; }
        public int TransdetailExKey { get; set; }
        public bool TransdetailExKeySpecified { get; set; }
    }

}
