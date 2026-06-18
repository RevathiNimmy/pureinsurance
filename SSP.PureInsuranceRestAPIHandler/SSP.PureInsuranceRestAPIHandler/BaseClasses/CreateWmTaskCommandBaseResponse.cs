namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateWmTaskCommandBaseResponse : BaseResponseType
    {
        public string GuidPMExternalItem { get; set; }
        public int TaskInstanceKey { get; set; }
    }
}
