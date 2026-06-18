namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PolicyQueryResponse
    {
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("InsuranceFileType")]
        public string InsuranceFileType { get; set; }
        //[DBCol("ClientName")]
        public string ClientName { get; set; }
        //[DBCol("ClientShortName")]
        public string ClientShortName { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("CreatedDate")]
        public System.DateTime CreatedDate { get; set; }
        //[DBCol("LastModifiedDate")]
        public System.DateTime LastModifiedDate { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductDescription")]
        public string ProductDescription { get; set; }
        //[DBCol("RiskIndex")]
        public string RiskIndex { get; set; }
        //[DBCol("Value")]
        public string Value { get; set; }
        //[DBCol("Status")]
        public string Status { get; set; }
        //[DBCol("IsMarketPlacePolicy")]
        public int IsMarketPlacePolicy { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
    }
}
