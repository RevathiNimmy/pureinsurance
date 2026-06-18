using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CalculateTaxForClaimsCommandBase :BaseRequestType
    {
        public BaseClaimPaymentAdvancedTaxDetailsType AdvancedTaxDetails { get; set; }
        public double Amount { get; set; }
        public string CompanyCode { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsSalvageRecovery { get; set; }
        public string LossCurrencyCode { get; set; }
        public string PaymentTo { get; set; }
        public int PerilId { get; set; }
        public BaseClaimReceiptAdvancedTaxDetailsType ReceiptAdvancedTaxDetails { get; set; }
        public int ReserveKey { get; set; }
        public string ReserveType { get; set; }
        public string TaxGroupCode { get; set; }
        public string TransactionTypeCode { get; set; }
    }
}
