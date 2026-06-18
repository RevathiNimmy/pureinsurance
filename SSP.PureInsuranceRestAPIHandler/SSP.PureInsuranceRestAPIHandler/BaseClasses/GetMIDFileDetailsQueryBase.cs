namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetMIDFileDetailsQueryBase : BaseRequestType
    {
        public bool FailuresOnly { get; set; }
        public int MIDFileKey { get; set; }
    }
}
