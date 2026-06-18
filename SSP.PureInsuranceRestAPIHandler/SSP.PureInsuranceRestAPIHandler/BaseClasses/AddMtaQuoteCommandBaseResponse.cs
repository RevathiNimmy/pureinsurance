using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddMtaQuoteCommandBaseResponse : BaseResponseType
    {
        public bool CanBeAddedToPfPlan { get; set; }
        public int InsuranceFileKey { get; set; }
        public DateTime QuoteExpiryDate { get; set; }

        public byte[] QuoteTimeStamp { get; set; }

        public int RiskKey { get; set; }
        public string XmlDataset { get; set; }
    }
}
