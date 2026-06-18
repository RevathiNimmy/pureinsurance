namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPerilReserveType
    {

        public decimal GrossReserve { get; set; }
        public decimal RevisedGrossReserve { get; set; }
        public decimal RevisedTaxReserve { get; set; }
        public decimal RevisionAmount { get; set; }
        public decimal Tax { get; set; }
        public string TypeCode { get; set; }
        public int ReserveId { get; set; }
        public int BaseReserveKey { get; set; }


    }
}
