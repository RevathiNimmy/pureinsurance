namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ApplyDiscountCommandResponse
    {
        public ApplyDiscountCommandBaseResponse ApplyDiscountResponse { get; set; }
    }

    public class ApplyDiscountCommandBaseResponse : BaseResponseType
    {
        public string FailureReason { get; set; }
    }
}
