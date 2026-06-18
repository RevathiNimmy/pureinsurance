namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClientDataExtractQueryBase : BaseRequestType
    {
        public string FilePassword { get; set; }
        public int PartyCnt { get; set; }
    }
}
