using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class AddPayNowReceiptCommand : AddPayNowReceiptCommandBase
    {
        public int SourceKey { get; set; }
    }
}
