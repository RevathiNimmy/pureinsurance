using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyOutstandingAmountResponseType :BaseResponseType
    {
        public decimal OutstandingAmount { get; set; }
    }
}
