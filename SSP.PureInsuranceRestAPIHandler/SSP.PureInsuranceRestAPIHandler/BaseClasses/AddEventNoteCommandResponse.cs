namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddEventNoteCommandResponse : BaseResponseType
    {
        public int EventKey { get; set; }
        public int EventPublicTextKey { get; set; }
    }
}
