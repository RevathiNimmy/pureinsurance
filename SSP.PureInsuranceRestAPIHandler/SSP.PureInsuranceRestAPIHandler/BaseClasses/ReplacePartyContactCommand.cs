namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ReplacePartyContactCommand : ReplacePartyContactCommandBase//, IRequest<ReplacePartyContactCommandResponse>
    {

        public int SourceId { get; set; }

        public int UserId { get; set; }
    }
}
