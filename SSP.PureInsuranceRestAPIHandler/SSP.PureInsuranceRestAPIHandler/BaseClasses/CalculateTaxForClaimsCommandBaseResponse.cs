using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CalculateTaxForClaimsCommandBaseResponse :BaseResponseType
    {
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public List<BaseClaimReceiptTaxItemType> ReceiptTaxItems { get; set; }
        public string ReserveType { get; set; }
        public string TaxBandCode { get; set; }
        public double TaxBaseAmount { get; set; }
        public double TaxCurrencyAmount { get; set; }
        public string TaxGroupCode { get; set; }
        public List<BaseClaimPaymentTaxItemType> TaxItems { get; set; }
        public double TaxLossAmount { get; set; }
    }
}
