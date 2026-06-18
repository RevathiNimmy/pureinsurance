namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReinsuranceArrangementLinesResponseTypeRow
    {
        //[DBCol("agreement_code")]
        public string Agreement { get; set; }
        //[DBCol("Balance")]
        public double Balance { get; set; }
        //[DBCol("DefaultPerc")]
        public double DefaultPerc { get; set; }
        //[DBCol("is_Obligatory")]
        public bool IsObligatory { get; set; }
        //[DBCol("Name")]
        public string Name { get; set; }
        //[DBCol("PaymentToDate")]
        public double PaymentToDate { get; set; }
        //[DBCol("ReserveToDate")]
        public double ReserveToDate { get; set; }
        //[DBCol("sum_insured")]
        public double SumInsured { get; set; }
        //[DBCol("this_payment")]
        public double ThisPayment { get; set; }
        //[DBCol("ThisPerc")]
        public double ThisPerc { get; set; }
        //[DBCol("this_reserve")]
        public double ThisReserve { get; set; }
        //[DBCol("type")]
        public string Type { get; set; }
    }
}
