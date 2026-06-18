using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreatePostingsForReinsuranceCommandBase : BaseRequestType
    {
        public string ProcessType { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public DateTime TransactionDate { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
