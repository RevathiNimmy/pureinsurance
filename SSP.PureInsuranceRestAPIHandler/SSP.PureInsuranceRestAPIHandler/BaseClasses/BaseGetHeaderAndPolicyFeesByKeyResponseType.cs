using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndPolicyFeesByKeyResponseType
    {
        public string Agent { get; set; }
        public string ClientCode { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public string Currency { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public double FeeTotal { get; set; }
        public double GrossTotal { get; set; }
        public System.DateTime InceptionDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public double NetTotal { get; set; }
        public System.Collections.Generic.List<BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow> PolicyFees { get; set; }
        public double TaxTotal { get; set; }
        public double TotalPolicyFees { get; set; }
        public double TotalPolicyFeesEligibleForFinancing { get; set; }
        public double TotalPolicyFeesExcludedFromFinancing { get; set; }
        public double TotalRiskFees { get; set; }
        public double TotalRiskFeesEligibleForFinancing { get; set; }
        public double TotalRiskFeesExcludedFromFinancing { get; set; }
    }
}
