namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddWmTaskLogCommandBase : BaseRequestType
    {
        public string LogText { get; set; }
        public int TaskInstanceKey { get; set; }
    }
}
