using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunPortfolioTransferCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InceptionDate { get; set; }

        public int InsuranceFileKey { get; set; }
        public string InsuranceFileType { get; set; }
        public short ProductKey { get; set; }
        public bool SkipPostings { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
