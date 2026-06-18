using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CancelMTAQuoteCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
