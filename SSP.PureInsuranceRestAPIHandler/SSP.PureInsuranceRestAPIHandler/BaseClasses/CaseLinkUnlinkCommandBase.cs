namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CaseLinkUnlinkCommandBase : BaseRequestType
    {
        public int BaseCaseKey { get; set; }
        public int ClaimKey { get; set; }
        public bool IsLinked { get; set; }
    }
}
