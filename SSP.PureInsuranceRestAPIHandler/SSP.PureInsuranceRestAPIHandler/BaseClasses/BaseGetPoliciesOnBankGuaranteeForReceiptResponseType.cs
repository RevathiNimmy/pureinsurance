using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPoliciesOnBankGuaranteeForReceiptResponseType
    {
        public int BGKey { get; set; }
        public int BankNameKey { get; set; }
        public string BankName { get; set; }
        public string BankGuaranteeRef { get; set; }
        public DateTime BGDueDate { get; set; }
        public int PolicyKey { get; set; }
        public string PolicyRef { get; set; }
        public decimal PremiumAmount { get; set; }
        public string BranchCode { get; set; }
        public string BranchDesc { get; set; }
        public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
        public decimal OutstandingPolicyAmt { get; set; }
        public DateTime CoverStartDate { get; set; }
        public DateTime CoverEndDate { get; set; }
    }
}
