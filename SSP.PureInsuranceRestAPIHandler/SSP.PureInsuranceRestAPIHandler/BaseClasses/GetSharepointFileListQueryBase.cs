namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetSharepointFileListQueryBase : BaseRequestType
    {
        public string ClaimNumber { get; set; }
        public bool CreateFolder { get; set; }
        public string FolderPath { get; set; }
        public string PartyShortname { get; set; }
        public string PolicyNumber { get; set; }
    }
}
