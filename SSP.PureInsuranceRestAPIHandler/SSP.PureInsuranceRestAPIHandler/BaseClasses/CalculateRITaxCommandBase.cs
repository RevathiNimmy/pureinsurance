namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CalculateRITaxCommandBase : BaseRequestType
    {
        public double Commission { get; set; }
        public int InsuranceFileKey { get; set; }
        public int PartyKey { get; set; }
        public double Premium { get; set; }
        public int RIArrangementLineKey { get; set; }
        public bool RIArrangementLineKeySpecified { get; set; }
        public int RiskKey { get; set; }
    }
}
