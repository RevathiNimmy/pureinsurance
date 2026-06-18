namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateFeeCommandBase : BaseRequestType
    {

        public int FeeKey { get; set; }
        public bool IsValue { get; set; }
        public decimal Rate { get; set; }
    }
}
