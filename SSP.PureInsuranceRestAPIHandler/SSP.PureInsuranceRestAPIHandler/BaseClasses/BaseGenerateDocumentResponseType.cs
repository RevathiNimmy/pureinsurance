namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGenerateDocumentResponseType : BaseResponseType
    {
        public byte[] SpooledZipFile { get; set; }

        public string MergedFilePath { get; set; }
    }
}
