namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindLatestPolicyVersionsResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public int AgentKey { get; set; }
        public string AgentName { get; set; }
        public string AssociatedClients { get; set; }
        public string ClientName { get; set; }
        public string ClientShortName { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileTypeCode { get; set; }
        public string InsuranceFileTypeDescription { get; set; }
        public int InsuranceFolderKey { get; set; }
        public string InsuranceRef { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
        public bool IsReadOnly { get; set; }
        public System.DateTime IssuedDate { get; set; }
        public bool MarkedQuoteForCollection { get; set; }
        public int NoOfRenewalVersions { get; set; }
        public int NoOfVersions { get; set; }
        public int PartyKey { get; set; }
        public string PolicyStatusCode { get; set; }
        public string PolicyStatusDescription { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public System.DateTime QuoteExpiryDate { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public System.DateTime TransactionDate { get; set; }
    }
}
