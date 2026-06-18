namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCalculateRITaxResponseType : BaseResponseType
    {
        public double CommissionTax { get; set; }
        public double PremiumTax { get; set; }
    }
}
