namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBranchType
    {
        //[DBCol("shortname")]
        public string AgentCode { get; set; }
        //[DBCol("agentkey")]
        public int AgentKey { get; set; }
        //[DBCol("code")]
        public string BranchCode { get; set; }
        //[DBCol("source_id")]
        public int BranchKey { get; set; }
        //[DBCol("direct_business")]
        public string BusinessType { get; set; }
        //[DBCol("description")]
        public string Description { get; set; }
    }
}
