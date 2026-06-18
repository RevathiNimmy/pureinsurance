namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPaymentItemType
    {

        public int BaseReserveKey { get; set; }
        public bool IsTaxOverridden { get; set; }
        public decimal OverriddedTaxAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool ReverseExcess { get; set; }
        public string TaxGroupCode { get; set; }

        public int TaxGroupId { get; set; }

        public int ReserveId { get; set; }

        public bool IsWHTTax { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TaxAmountWHT { get; set; }

        public int ClaimPaymentItemId { get; set; }


        public decimal ExcessAmount { get; set; }

        public bool IsExcess { get; set; }

        public decimal PaymentAdjustment { get; set; }

        public object[,] AdvancedTaxDetails { get; set; }

        public decimal CurrencyAmount { get; set; }

        public decimal LCurrencyAmount { get; set; }

        public decimal LCurrencyTaxWHT { get; set; }

        public decimal LCurrencyTax { get; set; }

        public decimal CurrentReserve { get; set; }

        public int RecoveryId { get; set; }

        public int RecoveryTypeId { get; set; }

        public decimal RevisionAmount { get; set; }

        public bool ReserveExcess { get; set; }

        public decimal CurrencyTax { get; set; }
    }
}
