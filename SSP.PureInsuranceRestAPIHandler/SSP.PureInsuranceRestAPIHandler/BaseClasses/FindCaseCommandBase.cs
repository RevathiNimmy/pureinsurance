using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindCaseCommandBase : BaseRequestType
    {
        public string CaseNumber { get; set; }
        public DateTime? CaseOpenDate { get; set; }
        public bool CaseOpenDateSpecified { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string ProgressStatusCode { get; set; } = string.Empty;
        public string RiskIndex { get; set; } = string.Empty;
        public string RiskType { get; set; } = string.Empty;
        public int AgentKey { get; set; }
    }
}
