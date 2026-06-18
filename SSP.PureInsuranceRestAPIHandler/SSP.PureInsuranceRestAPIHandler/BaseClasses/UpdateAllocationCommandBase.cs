using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateAllocation
{
    public class UpdateAllocationCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The AccountKey field is required")]
        
        public int AccountKey { get; set; }
        
        public System.Collections.Generic.List<BaseUpdateAllocationRequestTypeAllocation> Allocation { get; set; }
        public double Amount { get; set; }
        public int CashListItemKey { get; set; }
        public double CurrencyDiff { get; set; }
        public bool CurrencyDiffSpecified { get; set; }
        public int TransdetailExKey { get; set; }
        public bool TransdetailExKeyFieldSpecified { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The TransdetailKey field is required")]
        
        public int TransdetailKey { get; set; }
        public double WriteOffAmount { get; set; }
        public bool WriteOffAmountSpecified { get; set; }
        public int WriteOffReason { get; set; }
        public bool WriteOffReasonSpecified { get; set; }
    }
}
