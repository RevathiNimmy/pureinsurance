using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndRiskFeesByKeyQueryBaseResponse : BasePagedResponse
    {
        public string ClientCode { get; set; }
        public string Currency { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public List<BaseGetHeaderAndRiskFeesByKeyResponseTypeRow> RiskFees { get; set; }
    }
}
