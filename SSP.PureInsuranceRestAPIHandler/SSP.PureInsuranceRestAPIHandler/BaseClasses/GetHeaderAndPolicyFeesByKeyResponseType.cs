using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndPolicyFeesByKeyResponseType : BaseResponseType
    {
        public string agent { get; set; }
        public string clientCode { get; set; }
        public DateTime coverStartDate { get; set; }
        public string currency { get; set; }
        public DateTime expiryDate { get; set; }
        public double feeTotal { get; set; }
        public double grossTotal { get; set; }
        public DateTime inceptionDate { get; set; }
        public int insuranceFileKey { get; set; }
        public string insuranceFileRef { get; set; }
        public double netTotal { get; set; }
        public List<BaseGetHeaderAndPolicyFeesByKeyResponseTypeRow> policyFees { get; set; }
        public double taxTotal { get; set; }
        public double totalPolicyFees { get; set; }
        public double totalPolicyFeesEligibleForFinancing { get; set; }
        public double totalPolicyFeesExcludedFromFinancing { get; set; }
        public double totalRiskFees { get; set; }
        public double totalRiskFeesEligibleForFinancing { get; set; }
        public double totalRiskFeesExcludedFromFinancing { get; set; }
    }
}
