using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckUnpaidPremiumQueryResponse : BasePagedResponse
    {
        public CheckUnpaidPremiumQueryBaseResponse CheckUnpaidPremiumResponse { get; set; }
    }
}
