using SSP.PureInsuranceRestAPIHandler.Enums;
using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRiskStatusCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public DateTime RiskInceptionDate { get; set; }
        public bool RiskInceptionDateSpecified { get; set; }

        public int RiskKey { get; set; }
        public bool RiskKeySpecified { get; set; }
        public RiskStatusType RiskStatusCode { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
