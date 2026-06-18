namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseOpenClaimCommandBase : BaseRequestType
    {
        public BaseClaimOpenType Claim { get; set; }
        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }
    }
}
