namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentQueryBase : BaseRequestType
    {
        public bool Compress { get; set; }
        public bool ConvertPdf { get; set; }
        public int DocNum { get; set; }
        public string FileExtension { get; set; }
    }
}
