using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyType
    {
        public string AccountExecutive { get; set; } = string.Empty;
        public string AccountExecutiveCode { get; set; } = string.Empty;
        public System.Collections.Generic.List<BaseAddressWithContactsType> Addresses { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public System.Collections.Generic.List<BaseContactType> Contacts { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool DomiciledForTax { get; set; }
        public bool DomiciledForTaxSpecified { get; set; }
        public string FileCode { get; set; } = string.Empty;
        public string SubBranchCode { get; set; } = string.Empty;
        public string TPIntroducer { get; set; } = string.Empty;
        public string TPUserCode { get; set; } = string.Empty;
        public bool TaxExempt { get; set; }
        public bool TaxExemptSpecified { get; set; }
        public string TaxNumber { get; set; } = string.Empty;
        public decimal TaxPercentage { get; set; }
        public bool TaxPercentageSpecified { get; set; }
        public string XMLDataset { get; set; } = string.Empty;
        public string Agent { get; set; } = string.Empty;
    }
}
