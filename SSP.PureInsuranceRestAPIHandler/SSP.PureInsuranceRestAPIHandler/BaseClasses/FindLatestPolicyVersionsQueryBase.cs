using SSP.PureInsuranceRestAPIHandler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindLatestPolicyVersionsQueryBase : BaseRequestType
    {
        public int AgentKey { get; set; }
        public DateTime CoverStartDate { get; set; }
        public bool CoverStartDateSpecified { get; set; }
        public string InsuranceRef { get; set; }
        public string InsuredName { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string ProductCode { get; set; }
        public DateTime QuoteORLiveDate { get; set; }
        public bool QuoteORLiveDateSpecified { get; set; }
        public QuoteType RecordType { get; set; }
        public bool RecordTypeSpecified { get; set; }
        public bool RetrieveAssociates { get; set; }
        public string RiskIndex { get; set; }
    }
}
