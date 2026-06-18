namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAccountBalanceResponseTypeRow
    {
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("FloatBalance")]
        public double FloatBalance { get; set; }
        //[DBCol("Overdraft")]
        public double Overdraft { get; set; }
        //[DBCol("SumAmount")]
        public double SumAmount { get; set; }
    }
}
