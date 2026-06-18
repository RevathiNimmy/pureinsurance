namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPeriodResponseTypeRow
    {
        //[DBCol("AllocationIndicator")]
        public int AllocationIndicator { get; set; }
        //[DBCol("PeriodID")]
        public int PeriodID { get; set; }
        //[DBCol("PeriodName")]
        public string PeriodName { get; set; }
        //[DBCol("YearName")]
        public string YearName { get; set; }
    }
}
