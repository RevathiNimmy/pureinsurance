namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPartyPoliciesResponseTypeRow
    {
        public string AssociatedClients { get; set; }
        public int BaseInsuranceFolderKey { get; set; }
        public int ClosePolicyClaims { get; set; }
        public string CorrespondenceType { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string DefaultPreferredCorrespondence { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public string InsuranceDesc { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFileSourceKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int InsuranceHolderKey { get; set; }
        public string InsuranceRef { get; set; }
        public bool IsAgentReceiveCorrespondence { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public string LastTransDesc { get; set; }
        public string LeadAgentCode { get; set; }
        public int LeadAgentKey { get; set; }
        public bool MarkedForCollection { get; set; }
        public int OpenPolicyClaims { get; set; }
        public string PolicyStatus { get; set; }
        public string PolicyTypeCode { get; set; }
        public string PolicyTypeDesc { get; set; }
        public int PolicyTypeKey { get; set; }
        public string ProductCode { get; set; }
        public int ProductKey { get; set; }
        public int QuoteStatusKey { get; set; }
        public int QuoteVersion { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public int RenewedVersion { get; set; }
        public string StatusCode { get; set; }
        public double ThisPremium { get; set; }
        public string TypeCode { get; set; }
        public bool IsAgentCorrespondence { get; set; }
        public int CorrespondenceTypeID { get; set; }
        public int DefaultPreferredCorrespondenceID { get; set; }
    }
}
