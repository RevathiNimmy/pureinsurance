namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateAllocation
{
    public class UpdateAllocationCommandBaseResponse : BaseResponseType
    {
        public int AllocationBatchId { get; set; }
        public int AllocationId { get; set; }
        public bool AllocationStatus { get; set; }
        public bool IsWrittenOff { get; set; }
        public int WriteOffAllocationId { get; set; }
        public int WriteOffTransdetailId { get; set; }
    }
}
