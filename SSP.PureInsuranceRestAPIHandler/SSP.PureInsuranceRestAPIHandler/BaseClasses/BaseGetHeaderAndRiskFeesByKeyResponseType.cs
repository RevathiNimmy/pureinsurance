using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndRiskFeesByKeyResponseType : BaseResponseType
    {
        public string ClientCode { get; set; }
        public string Currency { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public System.Collections.Generic.List<BaseGetHeaderAndRiskFeesByKeyResponseTypeRow> RiskFees { get; set; }
    }
}
