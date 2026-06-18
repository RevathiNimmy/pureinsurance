using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SettleAllClaimPaymentsResponseType : BaseResponseType
    {
        public List<BaseSettleAllClaimPaymentSummaryResponseType> Summary { get; set; }

        public List<BaseGeneralWarningResponseType> Warnings {  get; set; }

    }
}
