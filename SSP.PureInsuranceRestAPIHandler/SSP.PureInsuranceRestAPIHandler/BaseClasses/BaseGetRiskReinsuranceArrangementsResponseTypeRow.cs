namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRiskReinsuranceArrangementsResponseTypeRow
    {
        //[DBCol("ri_arrangement_id")]
        public int ArrangementId { get; set; }
        //[DBCol("ri_band_id")]
        public int BandId { get; set; }
        //[DBCol("extended_Limit_amount")]
        public decimal ExtendedLimitAmount { get; set; }
        //[DBCol("fac_premium_type")]
        public int FACPremiumType { get; set; }
        //[DBCol("is_extended_limit_applied")]
        public bool IsExtendedLimitApplied { get; set; }
        //[DBCol("is_modified")]
        public bool IsModified { get; set; }
        //[DBCol("original_flag")]
        public bool IsOriginal { get; set; }
        //[DBCol("ri_model_id")]
        public int ModelId { get; set; }
        //[DBCol("premium")]
        public decimal Premium { get; set; }
        //[DBCol("code")]
        public string RIModelCode { get; set; }
        //[DBCol("rioverridereasonid")]
        public int RiOverrideReasonId { get; set; }
        //[DBCol("sum_insured")]
        public decimal SumInsured { get; set; }
        //[DBCol("rmcode")]
        public string XOLRIModelCode { get; set; }
        //[DBCol("xol_ri_model_id")]
        public int XOLRIModelID { get; set; }
    }
}
