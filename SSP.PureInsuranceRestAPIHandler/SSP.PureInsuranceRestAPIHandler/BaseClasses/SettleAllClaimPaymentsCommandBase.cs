
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SettleAllClaimPaymentsCommandBase : BaseRequestType
    {
        public List<BaseSettleClaimPaymentType> ClaimPayments { get; set; }
    }
}
