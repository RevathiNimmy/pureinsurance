using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckUnpaidPremiumQueryBaseResponse : BasePagedResponse
    {
        public int InstalmentOverdue { get; set; }
        public List<BaseCheckUnpaidPremiumResponseTypeRow> Transactions { get; set; }
    }
}
