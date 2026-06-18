using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class AddPayNowReceiptCommandBase : BaseRequestType
    {
        public int PartyKey { get; set; }
        public BaseReceiptType Receipt { get; set; }
    }
}
