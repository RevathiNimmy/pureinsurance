using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateBackgroundJobCommandBase : BaseRequestType
    {
        public string Description { get; set; }
        public DateTime JobWhenToStart { get; set; }
        public string JobXML { get; set; }
    }
}
