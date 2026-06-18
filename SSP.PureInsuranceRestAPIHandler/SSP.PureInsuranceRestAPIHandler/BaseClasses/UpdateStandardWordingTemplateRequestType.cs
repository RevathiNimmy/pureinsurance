using System;
using System.Collections.Generic;
using System.Text;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateStandardWordingTemplateRequestType : BaseRequestType
    {
        public byte[] DocumentTemplate{ get; set; }

        public string DocumentTemplateCode { get; set; }
            
        public SSP.PureInsuranceRestAPIHandler.Enums.DocumentFormatTypes DocumentTemplateFormat { get; set; }

        public bool DocumentTemplateFormatSpecified{  get; set; }

        public int DocumentTemplateKey { get; set; }

        public bool DocumentTemplateKeySpecified { get; set; }

        public bool IsTXTextControlEnabled { get; set; }
    }
}
