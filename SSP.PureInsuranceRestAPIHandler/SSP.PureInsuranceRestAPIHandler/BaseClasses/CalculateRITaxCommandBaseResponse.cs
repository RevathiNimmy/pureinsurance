namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CalculateRITaxCommandBaseResponse : BaseResponseType
    {
        public double CommissionTax { get; set; }
        public double PremiumTax { get; set; }
    }
}
