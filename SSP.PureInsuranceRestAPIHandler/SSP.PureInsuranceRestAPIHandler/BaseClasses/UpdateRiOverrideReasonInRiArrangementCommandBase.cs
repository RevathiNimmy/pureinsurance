namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRiOverrideReasonInRiArrangementCommandBase : BaseRequestType
    {
        public int RiArrangementId { get; set; }
        public int RiOverrideReasonId { get; set; }

    }
}
