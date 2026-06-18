namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentTaxGroupsResponseTypeRow
    {
        //[DBCol("code")]
        public string Code { get; set; }
        //[DBCol("description")]
        public string Description { get; set; }
        //[DBCol("is_withholding_tax")]
        public bool IsWithholdingTax { get; set; }
    }
}
