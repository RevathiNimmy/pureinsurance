using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateInviteResponseType : BaseResponseType
    {

        public string MergedFilePathField { get; set; }
        public byte[] QuoteTimeStamp { get; set; }
        public byte[] SpooledZipFile { get; set; }

    }
}
