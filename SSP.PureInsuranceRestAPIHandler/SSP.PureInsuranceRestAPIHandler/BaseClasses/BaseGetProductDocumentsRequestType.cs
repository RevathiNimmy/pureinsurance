using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductDocumentsRequestType :BaseRequestType
    {
        public int DocumentTemplateKey { get; set; }

        public Boolean DocumentTemplateKeySpecified { get; set; }

        public int FunctionalArea { get; set; }

        public int ProcessTypeKey { get; set; }

        public Boolean ProcessTypeKeySpecified { get; set; }

        public string ProductCode { get; set; }
    }
}
