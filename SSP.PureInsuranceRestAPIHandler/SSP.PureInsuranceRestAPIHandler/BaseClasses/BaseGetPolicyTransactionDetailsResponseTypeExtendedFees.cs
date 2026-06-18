namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyTransactionDetailsResponseTypeExtendedFees
    {
        public decimal Amount { get; set; }
        public decimal AmountOutstanding { get; set; }
        public byte[] FeeAllocationTimeStamp { get; set; }
        public byte[] FeeTaxAllocationTimeStamp { get; set; }
        public int FeeTaxTransDetailExtendedId { get; set; }
        public int FeeTaxTransDetailId { get; set; }
        public int FeeTransDetailExtendedId { get; set; }
        public int FeeTransDetailId { get; set; }
        public string FeeType { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxAmountOutstanding { get; set; }
    }
}
