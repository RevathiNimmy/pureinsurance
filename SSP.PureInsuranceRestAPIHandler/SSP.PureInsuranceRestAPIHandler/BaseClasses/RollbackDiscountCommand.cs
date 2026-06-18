namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RollbackDiscountCommand : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int ProductId { get; set; }
        public string TransactionType { get; set; }
        public int Task { get; set; }
    }
}
