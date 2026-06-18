using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckDocumentTemplateExistsQueryBase : BaseRequestType
    {
        public int DocumentTemplateKey { get; set; }
    }
}
