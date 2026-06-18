namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAllocationDetailType
    {
        public int AllocationDetailKey { get; set; }
        public BaseTransDetailType Transaction { get; set; }
    }
}
