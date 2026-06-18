using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ExecutePRERulesetCommandBaseResponse : BaseResponseType
    {
        public string XMLDataSet { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
