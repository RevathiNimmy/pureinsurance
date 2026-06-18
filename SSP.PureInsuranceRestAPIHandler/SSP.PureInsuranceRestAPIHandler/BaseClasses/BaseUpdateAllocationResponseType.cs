namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseUpdateAllocationResponseType : BaseResponseType
    {
        public bool AllocationStatus { get; set; }
        public int AllocationId { get; set; }
        public int AllocationBatchId { get; set; }
        public bool IsWrittenOff { get; set; }
        public int WriteOffTransdetailId { get; set; }
        public int WriteOffAllocationId { get; set; }
    }

}
