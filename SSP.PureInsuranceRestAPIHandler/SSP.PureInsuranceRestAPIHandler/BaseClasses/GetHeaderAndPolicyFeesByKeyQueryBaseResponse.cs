
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndPolicyFeesByKeyQueryBaseResponse : BasePagedResponse
    {
        public string Agent { get; set; }
        public string ClientCode { get; set; }
        public DateTime CoverStartDate { get; set; }
        public string Currency { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double FeeTotal { get; set; }
        public double GrossTotal { get; set; }
        public DateTime InceptionDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public double NetTotal { get; set; }
        public List<BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow> PolicyFees { get; set; }
        public double TaxTotal { get; set; }
        public double TotalPolicyFees { get; set; }
        public double TotalPolicyFeesEligibleForFinancing { get; set; }
        public double TotalPolicyFeesExcludedFromFinancing { get; set; }
        public double TotalRiskFees { get; set; }
        public double TotalRiskFeesEligibleForFinancing { get; set; }
        public double TotalRiskFeesExcludedFromFinancing { get; set; }
    }
}
