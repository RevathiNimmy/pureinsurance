using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndPolicyTaxByKeyResponseType : BaseResponseType
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
        public double NetSumInsured { get; set; }
        public double NetTotal { get; set; }
        public List<BaseGetHeaderAndPolicyTaxByKeyResponseTypeRow> PolicyTaxes { get; set; }
        public double TaxTotal { get; set; }
        public double TotalCommissionLeadAgent { get; set; }
        public double TotalPolicyTax { get; set; }
        public double TotalPolicyTaxEligibleForFinancing { get; set; }
        public double TotalPolicyTaxExcludedFromFinancing { get; set; }
        public double TotalRiskTax { get; set; }
        public double TotalRiskTaxEligibleForFinancing { get; set; }
        public double TotalRiskTaxExcludedFromFinancing { get; set; }
        public double TotalTaxLeadAgent { get; set; }
    }
}
