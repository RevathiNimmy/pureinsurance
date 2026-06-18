namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTaxGroupsForClaimsResponseTypeRow
    {
        //[DBCol("advanced_tax_script")]
        public string AdvanceTaxScript { get; set; }
        //[DBCol("code")]
        public string Code { get; set; }
        //[DBCol("description")]
        public string Description { get; set; }
        //[DBCol("is_tax_amount_editable")]
        public bool IsTaxAmountEditable { get; set; }
        //[DBCol("is_withholding_tax")]
        public bool IsWithHoldingTax { get; set; }
        //[DBCol("tax_group_id")]
        public int TaxGroupKey { get; set; }
    }
}
