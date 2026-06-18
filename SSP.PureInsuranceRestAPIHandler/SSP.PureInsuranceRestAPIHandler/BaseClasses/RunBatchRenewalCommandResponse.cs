namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunBatchRenewalCommandResponse : BaseResponseType
    {
        public int InsuranceFileKey { get; set; }
        public string Message { get; set; }
        public int Result { get; set; }
    }
}
