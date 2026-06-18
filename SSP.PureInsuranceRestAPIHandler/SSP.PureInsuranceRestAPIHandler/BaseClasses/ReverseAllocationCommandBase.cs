using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ReverseAllocation
{
    public class ReverseAllocationCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The AllocationKey field is required")]
        
        public int AllocationKey { get; set; }
        public bool IgnoreWarnings { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The TransDetailKey field is required")]
        
        public int TransDetailKey { get; set; }
    }
}
