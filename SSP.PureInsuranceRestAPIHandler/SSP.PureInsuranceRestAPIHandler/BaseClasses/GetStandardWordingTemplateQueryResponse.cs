namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetStandardWordingTemplateQueryResponse
    {
        public byte[] DocumentTemplate { get; set; } = new byte[0];
        public string MergedFilePath { get; set; }
    }
}
