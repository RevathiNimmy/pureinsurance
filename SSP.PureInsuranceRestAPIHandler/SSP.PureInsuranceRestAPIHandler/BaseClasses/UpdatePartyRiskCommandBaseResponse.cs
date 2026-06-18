namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePartyRiskCommandBaseResponse : BaseResponseType
    {
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public string XMLDataSet { get; set; }
    }
}
