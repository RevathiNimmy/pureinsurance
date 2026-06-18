using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CloneQuoteFromLivePolicyCommandBaseResponse
    {
        public int BaseInsuranceFolderKey { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public string InsuranceRef { get; set; }
        public SSP.PureInsuranceRestAPIHandler.Enums.QuoteStatusType QuoteStatusKey { get; set; }
        public int QuoteVersion { get; set; }
    }
}
