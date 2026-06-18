namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
    {
        public double AnnualPremium { get; set; }
        public double AnnualRate { get; set; }
        public double CalculatedPremium { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string EarningPatternCode { get; set; }
        public int IsAmended { get; set; }
        public int OriginalFlag { get; set; }
        public string OverrideReason { get; set; }
        public string RateTypeCode { get; set; }
        public string RatingSectionTypeCode { get; set; }
        public string StateCode { get; set; }
        public double SumInsured { get; set; }
        public double ThisPremium { get; set; }
    }
}
