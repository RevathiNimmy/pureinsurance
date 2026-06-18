namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteWmTaskCommandBase : BaseRequestType
    {
        public int TaskInstanceKey { get; set; }
        public byte[] TaskTimeStamp { get; set; } = new byte[0];
    }
}
