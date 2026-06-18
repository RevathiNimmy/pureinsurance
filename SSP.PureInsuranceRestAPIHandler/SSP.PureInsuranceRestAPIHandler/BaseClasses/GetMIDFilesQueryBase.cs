using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetMIDFilesQueryBase : BaseRequestType
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool FailuresOnly { get; set; }
        public int MIDFileKey { get; set; }
    }
}
