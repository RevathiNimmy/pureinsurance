namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteWmTaskCommandBaseResponse : BaseResponseType
    {
        public int TaskInstanceKey { get; set; }
        public byte[] TaskTimeStamp { get; set; } = new byte[0];
    }
}
