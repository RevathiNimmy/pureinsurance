using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class GetRenewalAmountToFinanceQueryBaseResponse :BaseGetReportResponseType
    {
        public double RenewalAmount { get; set; }
    }
}
