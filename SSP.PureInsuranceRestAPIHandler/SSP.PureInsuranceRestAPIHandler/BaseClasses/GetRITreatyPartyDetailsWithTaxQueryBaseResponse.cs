namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRITreatyPartyDetailsWithTaxQueryBaseResponse : BaseResponseType
    {
        public string AgreementCode { get; set; }
        public decimal CommissionPercent { get; set; }
        public decimal CommissionTax { get; set; }
        public int IsRetained { get; set; }
        public decimal PremiumTax { get; set; }
    }
}
