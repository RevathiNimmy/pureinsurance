namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CloseCaseCommandBase : BaseRequestType
    {
        public int BaseCaseKey { get; set; }
        public int CaseKey { get; set; }
        public string EventDescription { get; set; }
    }
}
