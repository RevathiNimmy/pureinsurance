namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPaymentTaxesQueryBase : BasePayClaimCommandBase
    {
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
