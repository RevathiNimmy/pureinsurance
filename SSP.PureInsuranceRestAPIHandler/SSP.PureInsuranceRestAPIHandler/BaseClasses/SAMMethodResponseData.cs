using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SamMethodResponseData
    {
        public System.Collections.Generic.List<SAMErrors> Errors { get; set; }
        public System.Guid? HandlingInstanceId { get; set; }
    }
}
