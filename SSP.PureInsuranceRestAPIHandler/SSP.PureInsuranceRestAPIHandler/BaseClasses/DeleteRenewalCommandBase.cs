namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteRenewalCommandBase : BaseRequestType
    {

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }

        //[Minength(1, ErrorMessage = "The QuoteTimeStamp field cannot be empty")]
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
