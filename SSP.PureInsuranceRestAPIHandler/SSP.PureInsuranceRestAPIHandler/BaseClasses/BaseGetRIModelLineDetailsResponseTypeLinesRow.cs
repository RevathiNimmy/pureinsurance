namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRIModelLineDetailsResponseTypeLinesRow
    {
        //[DBCol("cede_premium_only")]
        public bool CedePremiumOnly { get; set; }

        //[DBCol("ceding_rate")]
        public float CedingRate { get; set; }

        //[DBCol("description")]
        public string Description { get; set; }

        //[DBCol("effective_date")]
        public System.DateTime EffectiveDate { get; set; }

        //[DBCol("expiry_date")]
        public System.DateTime ExpiryDate { get; set; }

        //[DBCol("line_limit")]
        public double LineLimit { get; set; }

        //[DBCol("lower_limit")]
        public double LowerLimit { get; set; }

        //[DBCol("number_of_lines")]
        public decimal NoOfLines { get; set; }

        //[DBCol("priority")]
        public int Priority { get; set; }

        //[DBCol("ri_model_id")]
        public int RIModelKey { get; set; }

        //[DBCol("ri_model_line_id")]
        public int RIModelLineKey { get; set; }

        public string ReinsuranceTypeCode { get; set; }

        //[DBCol("reinsurance_type_id")]
        public int ReinsuranceTypeKey { get; set; }

        //[DBCol("share_percent")]
        public float SharePercent { get; set; }

        public string TreatyCode { get; set; }

        //[DBCol("treaty_id")]
        public int TreatyKey { get; set; }

        public string TreatyTypeCode { get; set; }

        //[DBCol("Treaty_type_id")]
        public int TreatyTypeKey { get; set; }

        //[DBCol("ManuallyAddedTreaty")]
        public bool ManuallyAddedTreaty { get; set; }
    }
}
