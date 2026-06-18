namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskQueryBase : BaseRequestType
    {
        public bool IgnoreLocking { get; set; }

        public int InsuranceFileKey { get; set; }

        public int InsuranceFolderKey { get; set; }

        //public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public string QuoteTimeStamp { get; set; }
        public int RiskKey { get; set; }
        public bool IsForEdit { get; set; }
        public string RiskLinkStatusFlag { get; set; }
    }
}
