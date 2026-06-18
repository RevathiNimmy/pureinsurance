namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentDefaultsQueryBase : BaseRequestType
    {
        public string DocumentTemplateCodes { get; set; }
        public string DocumentTemplateKeys { get; set; }
    }
}
