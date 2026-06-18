using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskGroupTasksQueryBase : BaseRequestType
    {
        public DateTime EffectiveDate { get; set; }
        public int TaskGroupKey { get; set; }
    }
}
