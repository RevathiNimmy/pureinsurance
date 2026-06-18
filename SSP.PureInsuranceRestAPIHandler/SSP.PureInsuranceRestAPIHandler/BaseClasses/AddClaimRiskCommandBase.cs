
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddClaimRiskCommandBase : BaseRequestType
    {
        public int BaseClaimKey { get; set; }
        public int? ClaimKey { get; set; }
        public bool IgnoreIsDirty { get; set; }
        public byte[] ApiTimseStamp { get; set; } = new byte[0];
    }
}
