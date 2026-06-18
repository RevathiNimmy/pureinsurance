using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RecalculateRIForPortfolioTransferCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public DateTime TransactionDate { get; set; }
    }
}
