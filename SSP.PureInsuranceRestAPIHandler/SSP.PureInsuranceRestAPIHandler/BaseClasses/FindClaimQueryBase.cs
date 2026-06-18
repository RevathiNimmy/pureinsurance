namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindClaimQueryBase : BaseRequestType
    {
        public string CaseNumber { get; set; }
        public bool CaseNumberSpecified { get; set; }
        public string ClaimNumber { get; set; }
        public string ClientShortName { get; set; }
        public string Description { get; set; }
        public bool IncludeClosedClaim { get; set; }
        public string InsuranceFileRef { get; set; }
        public System.DateTime LossDateFrom { get; set; }
        public bool LossDateFromSpecified { get; set; }
        public System.DateTime LossDateTo { get; set; }
        public bool LossDateToSpecified { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public bool RetrieveAssociates { get; set; }
        public string RiskIndex { get; set; }
        public int RiskKey { get; set; }
        public string TPA { get; set; }


        public int AgentKey { get; set; }


        public int AgentGroupKey { get; set; }


        public int UserKey { get; set; }


        public int SourceKey { get; set; }


        public bool ExactClaimOnly { get; set; }


        public bool GenerateResultDataset { get; set; }
    }
}
