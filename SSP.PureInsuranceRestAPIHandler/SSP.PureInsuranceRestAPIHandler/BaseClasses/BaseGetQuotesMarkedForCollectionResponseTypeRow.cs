namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetQuotesMarkedForCollectionResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        //[DBCol("AgentCommission")]
        public decimal AgentCommission { get; set; }
        //[DBCol("AgentKey")]
        public int AgentKey { get; set; }
        //[DBCol("AgentName")]
        public string AgentName { get; set; }
        //[DBCol("AgentTypeCode")]
        public string AgentTypeCode { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("BranchKey")]
        public int BranchKey { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CurrencyKey")]
        public int CurrencyKey { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFileRef")]
        public string InsuranceFileRef { get; set; }
        //[DBCol("InsuranceFileTypeCode")]
        public decimal InsuranceFileTypeCode { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PartyName")]
        public string PartyName { get; set; }
        //[DBCol("Premium")]
        public decimal Premium { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductKey")]
        public int ProductKey { get; set; }
    }
}
