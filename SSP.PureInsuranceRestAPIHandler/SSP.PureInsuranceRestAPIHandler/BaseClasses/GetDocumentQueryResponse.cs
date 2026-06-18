namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentQueryResponse : BaseResponseType
    {
        public string FileExtension { get; set; }
        public byte[] PdfDocument { get; set; } = new byte[0];
    }
}
