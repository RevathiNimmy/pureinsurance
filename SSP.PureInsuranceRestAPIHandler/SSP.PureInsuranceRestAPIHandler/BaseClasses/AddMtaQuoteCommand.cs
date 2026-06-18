using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddMtaQuoteCommand : AddMtaQuoteCommandBase
    {
        public bool IsMarketPlacePolicy { get;  set; }
        public int ClaimStatus { get;  set; }
    }
}
