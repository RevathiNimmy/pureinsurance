using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyTransactionDetailsResponseTypeExtended
    {
        public decimal Commission { get; set; }
        public byte[] CommissionAllocationTimeStamp { get; set; }
        public decimal CommissionOutstanding { get; set; }
        public decimal CommissionTax { get; set; }
        public byte[] CommissionTaxAllocationTimeStamp { get; set; }
        public decimal CommissionTaxOutstanding { get; set; }
        public int CommissionTaxTransDetailExtendedId { get; set; }
        public int CommissionTaxTransDetailId { get; set; }
        public int CommissionTransDetailExtendedId { get; set; }
        public int CommissionTransDetailId { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.Collections.Generic.List<BaseGetPolicyTransactionDetailsResponseTypeExtendedFees> Fees { get; set; }
        public decimal Premium { get; set; }
        public byte[] PremiumAllocationTimeStamp { get; set; }
        public decimal PremiumOutstanding { get; set; }
        public decimal PremiumTax { get; set; }
        public byte[] PremiumTaxAllocationTimeStamp { get; set; }
        public decimal PremiumTaxOutstanding { get; set; }
        public int PremiumTaxTransDetailExtendedId { get; set; }
        public int PremiumTaxTransDetailId { get; set; }
        public int PremiumTransDetailExtendedId { get; set; }
        public int PremiumTransDetailId { get; set; }
    }
}
