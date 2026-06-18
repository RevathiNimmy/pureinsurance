using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateStandardWordingTemplateResponseType:BaseResponseType
    {
        public string NewDocumentTemplateCode { get; set; }

        public string NewDocumentTemplateDescription { get; set; }

        public int NewDocumentTemplateKey { get; set; }

    }
}
