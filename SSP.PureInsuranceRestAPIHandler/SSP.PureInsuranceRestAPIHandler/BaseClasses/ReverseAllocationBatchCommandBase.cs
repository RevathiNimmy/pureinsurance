using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ReverseAllocationBatch
{
    public class ReverseAllocationBatchCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The AllocationBatchKey field is required")]
        
        public int AllocationBatchKey { get; set; }
        public bool IgnoreWarnings { get; set; }
    }
}
