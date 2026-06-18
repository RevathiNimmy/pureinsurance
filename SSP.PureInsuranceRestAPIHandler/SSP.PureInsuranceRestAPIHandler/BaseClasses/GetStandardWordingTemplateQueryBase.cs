using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetStandardWordingTemplateQueryBase : BaseRequestType
    {
        public string DocumentTemplateCode { get; set; }
        public DocumentFormatTypes DocumentTemplateFormat { get; set; }
        public bool DocumentTemplateFormatSpecified { get; set; }
        public int DocumentTemplateKey { get; set; }
        public bool DocumentTemplateKeySpecified { get; set; }
        public bool IsTXTextControlEnabled { get; set; }
    }
}
