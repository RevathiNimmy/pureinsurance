using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class GetRenewalAmountToFinanceQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }
}
