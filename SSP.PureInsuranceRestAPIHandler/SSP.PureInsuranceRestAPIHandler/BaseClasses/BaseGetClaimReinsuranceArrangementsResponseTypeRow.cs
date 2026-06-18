namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReinsuranceArrangementsResponseTypeRow
    {
        //[DBCol("ri_arrangement_id")]
        public int ArrangementId { get; set; }
        //[DBCol("Balance")]
        public double Balance { get; set; }
        //[DBCol("ri_band_id")]
        public int BandId { get; set; }
        //[DBCol("payment")]
        public double PaymentToDate { get; set; }
        //[DBCol("RecoveryToDate")]
        public double RecoveryToDate { get; set; }
        //[DBCol("reserve")]
        public double ReserveToDate { get; set; }
        //[DBCol("sum_insured")]
        public double SumInsured { get; set; }
        //[DBCol("this_payment")]
        public double ThisPayment { get; set; }
        //[DBCol("this_reserve")]
        public double ThisReserve { get; set; }

    }
}
