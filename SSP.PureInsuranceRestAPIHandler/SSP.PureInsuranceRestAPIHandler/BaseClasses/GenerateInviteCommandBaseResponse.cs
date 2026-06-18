using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateInviteCommandBaseResponse : BaseResponseType
    {
        public string MergedFilePath { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public byte[] SpooledZipFile { get; set; } = new byte[0];
    }
}
