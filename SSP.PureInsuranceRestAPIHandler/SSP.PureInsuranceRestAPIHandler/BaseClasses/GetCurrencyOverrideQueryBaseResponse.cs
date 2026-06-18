namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCurrencyOverrideQueryBaseResponse
    {
        public bool DateOverrideAllowed { get; set; }
        public bool RateOverrideAllowed { get; set; }
        public bool PrePolicyDateOverrideAllowed { get; set; }
        public bool PrePolicyRateOverrideAllowed { get; set; }
    }
}
