
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductDocumentsQueryBase :BaseRequestType
    {
        public int DocumentTemplateKey { get; set; }
        public bool DocumentTemplateKeySpecified { get; set; }
        public int FunctionalArea { get; set; }
        public int ProcessTypeKey { get; set; }
        public bool ProcessTypeKeySpecified { get; set; }
        public string ProductCode { get; set; }
    }
}
