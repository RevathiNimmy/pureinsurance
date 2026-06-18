
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetBrokerSummaryCommandBase : BaseRequestType
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
        public SSP.PureInsuranceRestAPIHandler.Enums.QuoteType RecordType { get; set; }
        public bool RecordTypeSpecified { get; set; }
        public bool ShowUserOnly { get; set; }
        public bool ShowUserOnlySpecified { get; set; }
        public int UserKey { get; set; }
        public bool UserKeySpecified { get; set; }
        public bool RetrieveAssociates { get; set; }

        public SSP.PureInsuranceRestAPIHandler.Enums.FilterPoliciesType FilterPolicies  {get; set;}
        public string RiskIndex { get; set; }

        public SSP.PureInsuranceRestAPIHandler.Enums.FilterQuotesType FilterQuotes { get; set; }

    }
}
