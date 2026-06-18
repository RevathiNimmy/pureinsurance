using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlanInformationQueryBaseResponse : BaseResponseType
    {
        public int OriginalInsuranceFileKey { get; set; }
        public int PremiumFinanceKey { get; set; }
        public int PremiumFinanceVersion { get; set; }
        public string ProductCode { get; set; }
    }
}
