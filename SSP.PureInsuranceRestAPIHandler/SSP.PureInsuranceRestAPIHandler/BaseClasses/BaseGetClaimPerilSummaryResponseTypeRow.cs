namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPerilSummaryResponseTypeRow
    {
        //[DBCol("Average")]
        public decimal Average { get; set; }
        //[DBCol("CurrentReserve")]
        public decimal CurrentReserve { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("InitialReserve")]
        public decimal InitialReserve { get; set; }
        //[DBCol("PaidAmount")]
        public decimal PaidAmount { get; set; }
        //[DBCol("RevisedReserve")]
        public decimal RevisedReserve { get; set; }
        //[DBCol("SumInsured")]
        public decimal SumInsured { get; set; }
    }
}
