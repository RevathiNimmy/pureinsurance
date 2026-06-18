namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateDocumentCommandBaseResponse : BaseResponseType
    {
        public string MergedFilePath { get; set; }
        public byte[] SpooledZipFile { get; set; } = new byte[0];
    }
}
