namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInstalmentSettlementAmountQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public string TransactionType { get; set; }
    }
}
