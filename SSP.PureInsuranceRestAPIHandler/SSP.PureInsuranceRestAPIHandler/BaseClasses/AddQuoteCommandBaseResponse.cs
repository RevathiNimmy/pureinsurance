using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteCommandBaseResponse : BaseResponseType
    {
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public int InsuranceFolderKey { get; set; }
        public DateTime QuoteExpiryDate { get; set; }

        public byte[] QuoteTimeStamp { get; set; }

        public int RiskKey { get; set; }
    }
}
