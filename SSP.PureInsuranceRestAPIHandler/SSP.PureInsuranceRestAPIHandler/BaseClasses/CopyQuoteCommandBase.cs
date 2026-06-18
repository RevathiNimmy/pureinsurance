using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class CopyQuoteCommandBase : BaseRequestType
    {
        public bool CloneLivePolicy { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool IsQuoteVersioning { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool IsQuoteVersioningSpecified { get; set; }
    }
}
