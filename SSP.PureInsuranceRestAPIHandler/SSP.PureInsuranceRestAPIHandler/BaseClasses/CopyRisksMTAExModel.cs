namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CopyRisksMtaExModel
    {
        public int InsuranceFileCount { get; set; }
        public int LastNewRiskCount { get; set; }
        public bool FromSAM { get; set; }
        public long CreateLinkType { get; set; }
        public bool IsSAMCopyQuote { get; set; }
        public int OnlyRiskCount { get; set; }
    }
}
