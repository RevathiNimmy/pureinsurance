using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAllocationType
    {
        public int AccountKey { get; set; }
        public System.Collections.Generic.List<BaseAllocationDetailType> AllocatedTrans { get; set; }
        public int AllocationKey { get; set; }
        public bool AutoAllocate { get; set; }
        public string BranchCode { get; set; }
        public BaseTransDetailType LeadAllocatingTrans { get; set; }
        public System.Collections.Generic.List<BaseTransDetailType> OtherAllocatingTrans { get; set; }
        public int SourceID { get; set; }
        public bool isValidated { get; set; }
    }
}
