namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRatingSectionByRiskTypeResponseTypeRow
    {
        //[DBCol("country_id")]
        public int CountryID { get; set; }

        //[DBCol("currency_id")]
        public int CurrencyID { get; set; }

        //[DBCol("Description")]
        public string Description { get; set; }

        //[DBCol("Earning_Pattern_id")]
        public int EarningPatternID { get; set; }

        //[DBCol("Rate")]
        public decimal Rate { get; set; }

        //[DBCol("rate_type_id")]
        public int RateTypeID { get; set; }

        //[DBCol("RatingSectionCode")]
        public string RatingSectionCode { get; set; }

        //[DBCol("RatingSectionId")]
        public int RatingSectionId { get; set; }

        //[DBCol("state_id")]
        public int StateID { get; set; }
    }
}
