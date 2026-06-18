using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseNBQuoteResponseTypePolicy
    {
        public bool SkipNewPolicyNumber { get; set; }
        public int PolicyID { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public string QuoteRef { get; set; }
        public System.Collections.Generic.List<BaseRiskResultType> Risks { get; set; }
        public decimal PremiumDueNet { get; set; }
        public decimal PremiumDueTax { get; set; }
        public decimal PremiumDueGross { get; set; }
        public decimal TotalAnnualTax { get; set; }
        public decimal CommissionAmount { get; set; }
        public byte[] QuoteTimeStamp { get; set; }
        public STSErrorType STSError { get; set; }
    }
}
