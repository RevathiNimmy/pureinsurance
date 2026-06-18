namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtReserveType
    {
        public decimal RevisionAmount { get; set; }
        public int SAMStagingReserveKey { get; set; }
        public int SiriusBaseReserveKey { get; set; }
        public string TypeCode { get; set; }
    }
}
