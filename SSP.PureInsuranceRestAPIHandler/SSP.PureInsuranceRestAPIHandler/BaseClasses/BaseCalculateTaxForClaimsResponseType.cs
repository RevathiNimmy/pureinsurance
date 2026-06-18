namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCalculateTaxForClaimsResponseType : BaseResponseType
    {
        public double TaxCurrencyAmount { get; set; }

        public double TaxLossAmount { get; set; }

        public double TaxBaseAmount { get; set; }

        public string ReserveType { get; set; }

        public string TaxGroupCode { get; set; }

        public string TaxBandCode { get; set; }

        public decimal Percentage { get; set; }

        public decimal Amount { get; set; }

        public BaseClaimPaymentTaxItemType[] TaxItems { get; set; } = new BaseClaimPaymentTaxItemType[0];

        public BaseClaimReceiptTaxItemType[] ReceiptTaxItems { get; set; } = new BaseClaimReceiptTaxItemType[0];
    }
}
