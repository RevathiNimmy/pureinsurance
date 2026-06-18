namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetBrokerSummaryResponseTypeRow
    {
        //[DBCol("AgentKey")]
        public int AgentKey { get; set; }
        //[DBCol("AgentName")]
        public string AgentName { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("BaseInsuranceFileKey")]
        public int BaseInsuranceFileKey { get; set; }
        //[DBCol("BaseInsuranceFolderKey")]
        public int BaseInsuranceFolderKey { get; set; }
        //[DBCol("ClientName")]
        public string ClientName { get; set; }
        //[DBCol("ClientShortName")]
        public string ClientShortName { get; set; }
        //[DBCol("ExpiryDate")]
        public System.DateTime ExpiryDate { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFileTypeCode")]
        public string InsuranceFileTypeCode { get; set; }
        //[DBCol("InsuranceFileTypeDescription")]
        public string InsuranceFileTypeDescription { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("IsCurrent")]
        public bool IsCurrent { get; set; }
        //[DBCol("IsMarketPlacePolicy")]
        public bool IsMarketPlacePolicy { get; set; }
        //[DBCol("IsReinstateLinkVersion")]
        public bool IsReinstateLinkVersion { get; set; }
        //[DBCol("IssuedDate")]
        public System.DateTime IssuedDate { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PolicyStatusCode")]
        public string PolicyStatusCode { get; set; }
        //[DBCol("PolicyStatusDescription")]
        public string PolicyStatusDescription { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductDescription")]
        public string ProductDescription { get; set; }
        //[DBCol("QuoteExpiryDate")]
        public System.DateTime QuoteExpiryDate { get; set; }
        //[DBCol("QuoteStatusKey")]
        public int QuoteStatusKey { get; set; }
        //[DBCol("QuoteVersion")]
        public int QuoteVersion { get; set; }
        //[DBCol("RenewedVersion")]
        public int RenewedVersion { get; set; }
        //[DBCol("RiskStatus")]
        public string RiskStatus { get; set; }
        //[DBCol("StartDate")]
        public System.DateTime StartDate { get; set; }
        //[DBCol("RiskNumber")]
        public int RiskNumber { get; set; }
        //[DBCol("RiskDescription")]
        public string RiskDescription { get; set; }

    }
}