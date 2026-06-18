namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRiskCommandBase : BaseRequestType
    {
        public bool IgnoreErrorMessage { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFolderKey field is required")]
        //
        public int InsuranceFolderKey { get; set; }

        //[Minength(1, ErrorMessage = "The QuoteTimeStamp field cannot be empty")]
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        //public string QuoteTimeStamp { get; set; } = string.Empty;

        ////(1, int.MaxValue, ErrorMessage = "The RiskKey field is required")]
        //
        public int RiskKey { get; set; }
        public bool ReturnPremiumMoreThanBilled { get; set; }

        public string RiskDescription { get; set; }

        public string ScreenCode { get; set; }
        public string SubBranchCode { get; set; }
        public string TransactionType { get; set; }

        public string XMLDataSet { get; set; }
    }
}
