using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RecalculateRIForCloneTransferCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public string ApiTimeStamp { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
