using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPartyQueryBase : BaseRequestType
    {
        public string AddressLine1 { get; set; }
        public string AgentGroup { get; set; }
        public PartyAgentType AgentType { get; set; }
        public bool AgentTypeSpecified { get; set; }
        public string AreaCode { get; set; }
        public string CaseNumber { get; set; }
        public bool CaseNumberSpecified { get; set; }
        public string ClaimNumber { get; set; }
        public string ClaimsRiskIndex { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public bool DateOfBirthSpecified { get; set; }
        public string FileCode { get; set; }
        public bool IncludeClosedBranches { get; set; }
        public bool IsAnySelected { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string Name { get; set; }
        public string OtherPartyTypeCode { get; set; }
        public string PartyIndex { get; set; }
        public int PartySourceId { get; set; }
        public bool PartySourceIdSpecified { get; set; }
        public PartyTypeType PartyType { get; set; }
        public bool PartyTypeSpecified { get; set; }
        public string PolicyRef { get; set; }
        public string PostCode { get; set; }
        public string RiskIndex { get; set; }
        public string RiskRequestdex { get; set; }
        public string SearchType { get; set; }
        public string Shortname { get; set; }
        public string Status { get; set; }
        public bool SupressSubAgents { get; set; }
        public bool SupressSubAgentsSpecified { get; set; }
        public string TelephoneNumber { get; set; }
        public string TransactionType { get; set; }
        public int AgentKey { get; set; }
        public int SourceId { get; set; }
        public bool GenerateResultDataset { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string AddressLine2 { get; set; }
    }
}
