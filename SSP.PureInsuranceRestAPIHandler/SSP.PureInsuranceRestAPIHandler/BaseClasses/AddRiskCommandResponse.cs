namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddRiskCommandResponse : BaseResponseType
    {
        public int RiskTypeId { get; set; }

        public int BranchID { get; set; }

        public int RiskFolderKey { get; set; }

        public int RiskKey { get; set; }

        public string XMLDataSet { get; set; }

        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
