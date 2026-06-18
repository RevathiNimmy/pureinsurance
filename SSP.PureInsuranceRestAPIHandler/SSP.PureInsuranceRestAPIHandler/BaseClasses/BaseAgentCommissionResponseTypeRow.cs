namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAgentCommissionResponseTypeRow
    {
        public string Agent { get; set; }
        public string AgentType { get; set; }
        public string CommissionBand { get; set; }
        public double CommissionRate { get; set; }
        public double CommissionValue { get; set; }
        public bool IsAmended { get; set; }
        public bool IsLeadAgent { get; set; }
        public bool IsValue { get; set; }
        public double MaximumRate { get; set; }
        public string OverRideReason { get; set; }
        public double Premium { get; set; }
        public string RiskType { get; set; }
        public string TaxGroup { get; set; }
        public string TaxGroupDescription { get; set; }
        public double TaxValue { get; set; }
        public int PerilType { get; set; }
    }
}
