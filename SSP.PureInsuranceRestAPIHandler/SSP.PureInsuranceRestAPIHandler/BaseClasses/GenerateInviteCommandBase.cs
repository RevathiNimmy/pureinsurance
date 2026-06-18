using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateInviteCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public bool OutputAsHTML { get; set; }
        public bool OutputAsPDF { get; set; }
        //public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public string QuoteTimeStamp { get; set; }
        public bool SpoolDocumentOnly { get; set; }
        public bool SpoolDocumentOnlySpecified { get; set; }
        public int UserId { get;  set; }
    }
}
