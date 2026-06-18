using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SettleAllClaimPaymentsRequestType : BaseRequestType
    {
        public List<BaseSettleClaimPaymentType> ClaimPayments { get; set; }
    }
}
