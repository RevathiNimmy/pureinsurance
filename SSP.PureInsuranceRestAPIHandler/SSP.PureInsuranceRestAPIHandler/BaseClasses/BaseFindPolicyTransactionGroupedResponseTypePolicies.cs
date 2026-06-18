using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindPolicyTransactionGroupedResponseTypePolicies
    {
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public double Commission { get; set; }
        public double CommissionOS { get; set; }
        public double CommissionTax { get; set; }
        public double CommissionTaxOS { get; set; }
        public System.Collections.Generic.List<BaseFindPolicyTransactionGroupedResponseTypePoliciesFees> Fees { get; set; }
        public string PolicyCurrency { get; set; }
        public int PolicyFolderId { get; set; }
        public string PolicyNumber { get; set; }
        public double Premium { get; set; }
        public double PremiumOS { get; set; }
        public double PremiumTax { get; set; }
        public double PremiumTaxOS { get; set; }
    }
}
