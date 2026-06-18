namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateAllocationRequestTypeAllocation
    {
        public double AllocationAmount { get; set; }
        public bool AllocationAmountSpecified { get; set; }
        public byte[] AllocationTimeStamp { get; set; }
        public int AllocationTransdetailExKey { get; set; }
        public bool AllocationTransdetailExKeySpecified { get; set; }
        public int AllocationTransdetailKey { get; set; }
        public double WriteOffAmount { get; set; }
    }
}
