namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunBatchRenewalCommandBase : BaseRequestType
    {
        public int BatchId { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int SourceKey { get; set; }
    }
}
