namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRIAmendmentStatusCommandBase : BaseRequestType
    {
        public string ProcessType { get; set; }
        public int Status { get; set; }
        public string ApiTimeStamp { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
