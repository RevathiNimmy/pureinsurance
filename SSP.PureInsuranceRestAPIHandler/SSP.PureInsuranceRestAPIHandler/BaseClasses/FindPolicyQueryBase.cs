using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Xml;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPolicyQueryBase : RequestTypeBase
    {

        public int AgentKey { get; set; }
        public int AgentGroupKey { get; set; }
        public string InsuranceRef { get; set; }
        public string RiskIndex { get; set; }
        public string ClientShortName { get; set; }
        public InsuranceFileTypes QuoteType { get; set; }
        public bool QuoteTypeSpecified { get; set; }
        public bool ShowLapsedOnly { get; set; }
        public bool ShowLapsedOnlySpecified { get; set; }
        public bool ShowCancelledForEvents { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public bool RetrieveAssociates { get; set; }


        public bool GetResultDataset { get; set; }


        public XmlElement ResultDataset { get; set; }
    }
}
