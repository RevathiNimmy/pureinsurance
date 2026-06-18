
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateStandardWordingTemplateCommandBase : BaseRequestType
    {
        public byte[] DocumentTemplate { get; set; } = new byte[0];
        public string DocumentTemplateCode { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.DocumentFormatTypes DocumentTemplateFormat { get; set; }
        public bool DocumentTemplateFormatSpecified { get; set; }
        public int DocumentTemplateKey { get; set; }
        public bool DocumentTemplateKeySpecified { get; set; }
        public bool IsTXTextControlEnabled { get; set; }
    }
}
