namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClientDataExtractQueryBaseResponse : BaseResponseType
    {
        public byte[] ClientDataFile { get; set; } = new byte[0];
    }
}
