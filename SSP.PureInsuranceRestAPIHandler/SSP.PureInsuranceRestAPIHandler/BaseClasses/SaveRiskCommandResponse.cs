using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveRiskCommandResponse : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public string XMLDataSet { get; set; }
    }
}