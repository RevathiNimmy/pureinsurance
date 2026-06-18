namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTaxTypesAndBandsResponseRow
    {
        //[DBCol("allow_tax_credit")]
        public bool AllowTaxCredit { get; set; }
        //[DBCol("currency_id")]
        public int CurrencyId { get; set; }
        //[DBCol("is_deleted")]
        public int IsDeleted { get; set; }
        //[DBCol("is_value")]
        public bool IsValue { get; set; }
        //[DBCol("Rate")]
        public decimal Rate { get; set; }
        //[DBCol("sequence")]
        public int Sequence { get; set; }
        //[DBCol("DESCRIPTION")]
        public string TaxBandDescription { get; set; }
        //[DBCol("tax_band_id")]
        public int TaxBandId { get; set; }
        //[DBCol("tax_band_rate_id")]
        public int TaxBandRateId { get; set; }
        //[DBCol("code")]
        public string TaxTypeCode { get; set; }
        //[DBCol("DESCRIPTION")]
        public string TaxTypeDescription { get; set; }
        //[DBCol("tax_type_id")]
        public int TaxTypeId { get; set; }
    }
}
