namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateInstalmentStatus
{
    public class UpdateInstalmentStatusCommandBase : BaseRequestType
    {
        public string PFIStatusCode { get; set; }
        public int PFInstalmentKey { get; set; }
    }
}
