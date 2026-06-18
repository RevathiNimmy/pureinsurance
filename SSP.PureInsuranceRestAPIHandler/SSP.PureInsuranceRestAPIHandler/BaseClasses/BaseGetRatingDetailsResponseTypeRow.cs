namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRatingDetailsResponseTypeRow
    {

        //[DBCol("AnnualPremium")]
        public decimal AnnualPremium { get; set; }
        //[DBCol("AnnualRate")]
        public decimal AnnualRate { get; set; }
        //[DBCol("CalculatedPremium")]
        public decimal CalculatedPremium { get; set; }
        //[DBCol("Country")]
        public string Country { get; set; }
        //[DBCol("CountryCode")]
        public string CountryCode { get; set; }
        //[DBCol("CountryId")]
        public int CountryId { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CurrencyId")]
        public int CurrencyId { get; set; }
        //[DBCol("EarningPattern")]
        public string EarningPattern { get; set; }
        //[DBCol("EarningPatternCode")]
        public string EarningPatternCode { get; set; }
        //[DBCol("EarningPatternId")]
        public int EarningPatternId { get; set; }
        //[DBCol("IsAmended")]
        public int IsAmended { get; set; }
        //[DBCol("OriginalFlag")]
        public int OriginalFlag { get; set; }
        //[DBCol("OverrideReason")]
        public string OverrideReason { get; set; }
        //[DBCol("PolicySectionType")]
        public string PolicySectionType { get; set; }
        //[DBCol("PolicySectionTypeId")]
        public int PolicySectionTypeId { get; set; }
        //[DBCol("RateType")]
        public string RateType { get; set; }
        //[DBCol("RateTypeId")]
        public int RateTypeId { get; set; }
        //[DBCol("RatingSectionId")]
        public int RatingSectionId { get; set; }
        //[DBCol("RatingSectionType")]
        public string RatingSectionType { get; set; }
        //[DBCol("RatingSectionTypeCode")]
        public string RatingSectionTypeCode { get; set; }
        //[DBCol("RatingSectionTypeId")]
        public int RatingSectionTypeId { get; set; }
        //[DBCol("RatingTypeCode")]
        public string RatingTypeCode { get; set; }
        //[DBCol("State")]
        public string State { get; set; }
        //[DBCol("StateCode")]
        public string StateCode { get; set; }
        //[DBCol("StateId")]
        public int StateId { get; set; }
        //[DBCol("SumInsured")]
        public decimal SumInsured { get; set; }
        //[DBCol("ThisPremium")]
        public decimal ThisPremium { get; set; }
    }
}
