
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SettleAllClaimPaymentsCommandResponse : BaseResponseType
    {
        public List<BaseSettleAllClaimPaymentSummaryResponseType> Summary { get; set; }
        public List<BaseGeneralWarningResponseType> Warnings { get; set; }
    }
}
