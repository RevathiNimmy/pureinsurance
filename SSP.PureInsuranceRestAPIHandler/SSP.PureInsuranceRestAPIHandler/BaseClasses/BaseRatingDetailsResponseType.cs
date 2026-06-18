namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRatingDetailsResponseType
    {
        public string RatingSectionType { get; set; } = string.Empty;
        public string PolicySectionType { get; set; } = string.Empty;
        public string RateType { get; set; } = string.Empty;
        public decimal AnnualRate { get; set; } = 0;
        public decimal SumInsured { get; set; } = 0;
        public decimal ThisPremium { get; set; } = 0;
        public decimal AnnualPremium { get; set; } = 0;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int RatingSectionId { get; set; } = 0;
        public int RatingSectionTypeId { get; set; } = 0;
        public int PolicySectionTypeId { get; set; } = 0;
        public int RateTypeId { get; set; } = 0;
        public int OriginalFlag { get; set; } = 0;
        public int CurrencyId { get; set; } = 0;
        public int CountryId { get; set; } = 0;
        public int StateId { get; set; } = 0;
        public int IsAmended { get; set; } = 0;
        public decimal CalculatedPremium { get; set; } = 0;
        public string OverrideReason { get; set; } = string.Empty;
        public string EarningPattern { get; set; } = string.Empty;
        public int EarningPatternId { get; set; } = 0;
        public string StateCode { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string RatingTypeCode { get; set; } = string.Empty;
        public string RatingSectionTypeCode { get; set; } = string.Empty;
        public string EarningPatternCode { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
