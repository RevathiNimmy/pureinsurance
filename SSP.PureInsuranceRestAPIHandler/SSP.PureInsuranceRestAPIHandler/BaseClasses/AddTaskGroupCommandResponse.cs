namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddTaskGroupCommandResponse
    {
        public int TaskGroupKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
