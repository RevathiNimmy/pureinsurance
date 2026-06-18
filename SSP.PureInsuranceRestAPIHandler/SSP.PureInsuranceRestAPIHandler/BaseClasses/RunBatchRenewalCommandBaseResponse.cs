namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunBatchRenewalCommandBaseResponse : BaseResponseType
    {
        public int InsuranceFileKey { get; set; }
        public string Message { get; set; }
        public int Result { get; set; }
    }
}
