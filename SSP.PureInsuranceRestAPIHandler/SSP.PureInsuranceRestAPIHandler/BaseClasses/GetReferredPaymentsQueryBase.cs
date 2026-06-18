using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetReferredPaymentsQueryBase : BaseRequestType
    {
        public string CaseNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string ClientShortName { get; set; }
        public DateTime DateOfPayment { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        public string PayeeName { get; set; }
        public string PolicyNumber { get; set; }
        public string ReferredPaymentsBranchCode { get; set; }
        public string UserCode { get; set; }
        public int AgentKey { get; set; }
    }
}
