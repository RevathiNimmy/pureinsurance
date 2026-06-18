namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRatingSectionTypesResponseTypeRow
    {
        //[DBCol("country_code")]
        public string CountryCode { get; set; }

        //[DBCol("country_id")]
        public int CountryId { get; set; }

        //[DBCol("currency_code")]
        public string CurrencyCode { get; set; }

        //[DBCol("currency_id")]
        public int CurrencyId { get; set; }

        //[DBCol("Description")]
        public string Description { get; set; }

        //[DBCol("Earning_Pattern_code")]
        public string EarningPatternCode { get; set; }

        //[DBCol("Rate")]
        public decimal Rate { get; set; }

        //[DBCol("rate_code")]
        public string RateTypeCode { get; set; }

        //[DBCol("Rate_Type_id")]
        public int RateTypeId { get; set; }

        //[DBCol("Code")]
        public string RatingSectionTypeCode { get; set; }

        //[DBCol("Rating_Section_Type_id")]
        public int RatingSectionTypeId { get; set; }

        //[DBCol("state_code")]
        public string StateCode { get; set; }

        //[DBCol("state_id")]
        public int StateId { get; set; }
    }
}
