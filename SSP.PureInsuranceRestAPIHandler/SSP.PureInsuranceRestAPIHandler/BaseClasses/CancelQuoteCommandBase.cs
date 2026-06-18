
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CancelQuoteCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
