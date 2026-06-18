namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ReverseCollectedInstalmentCommandBase : BaseRequestType
    {
        public int PFInstalmentId { get; set; }
        public string? PFPlanStatusInd { get; set; }
    }
}
