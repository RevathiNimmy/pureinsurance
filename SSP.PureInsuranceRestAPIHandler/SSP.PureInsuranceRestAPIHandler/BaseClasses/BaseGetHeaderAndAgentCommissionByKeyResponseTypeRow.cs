namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndAgentCommissionByKeyResponseTypeRow
    {
        //[DBCol("Agent")]
        public string Agent { get; set; }
        //[DBCol("AgentType")]
        public string AgentType { get; set; }
        //[DBCol("CommissionBand")]
        public string CommissionBand { get; set; }
        //[DBCol("CommissionRate")]
        public double CommissionRate { get; set; }
        //[DBCol("CommissionValue")]
        public double CommissionValue { get; set; }
        //[DBCol("IsLeadAgent")]
        public bool IsLeadAgent { get; set; }
        //[DBCol("IsValue")]
        public bool IsValue { get; set; }
        //[DBCol("Premium")]
        public double Premium { get; set; }
        //[DBCol("RiskType")]
        public string RiskType { get; set; }
        //[DBCol("TaxGroup")]
        public string TaxGroup { get; set; }
        //[DBCol("TaxValue")]
        public double TaxValue { get; set; }
    }
}
