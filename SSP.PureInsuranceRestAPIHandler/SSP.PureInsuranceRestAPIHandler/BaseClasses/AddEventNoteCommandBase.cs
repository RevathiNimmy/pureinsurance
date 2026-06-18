namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddEventNoteCommandBase : BaseRequestType
    {
        public int EventKey { get; set; }
        public string EventText { get; set; }
    }
}
