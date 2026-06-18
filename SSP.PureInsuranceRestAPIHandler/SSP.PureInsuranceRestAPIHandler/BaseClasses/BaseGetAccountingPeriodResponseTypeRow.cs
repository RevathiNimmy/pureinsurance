namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAccountingPeriodResponseTypeRow
    {
        //[DBCol("period_end_date")]
        public System.DateTime PeriodEndDate { get; set; }
        //[DBCol("period_id")]
        public int PeriodKey { get; set; }
        //[DBCol("period_name")]
        public string PeriodName { get; set; }
        //[DBCol("year_name")]
        public string YearName { get; set; }
    }
}
