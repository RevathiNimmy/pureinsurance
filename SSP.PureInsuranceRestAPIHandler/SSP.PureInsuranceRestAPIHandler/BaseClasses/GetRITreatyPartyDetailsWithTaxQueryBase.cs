namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRITreatyPartyDetailsWithTaxQueryBase : BaseRequestType
    {
        public decimal Commission { get; set; }
        public string CommissionTransType { get; set; }
        public bool IgnoreTax { get; set; }
        public bool IgnoreTreatyDetails { get; set; }
        public int InsuranceFileID { get; set; }
        public decimal Premium { get; set; }
        public string PremiumTransType { get; set; }
        public int RIArrangementLineID { get; set; }
        public int RiskID { get; set; }
        public string TreatyCode { get; set; }
        public int TreatyID { get; set; }
    }
}
