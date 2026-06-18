namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunValidationRulesCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public bool ClaimKeySpecified { get; set; }
        public int ClaimPerilKey { get; set; }
        public bool ClaimPerilKeySpecified { get; set; }
        public string? ScreenCode { get; set; }
        public bool SkipSaveToDB { get; set; }
        public string TransactionType { get; set; }
        public string XMLDataSet { get; set; }
    }
}
