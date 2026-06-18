namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ChangePasswordCommandBase : BaseRequestType
    {
        public string NewPassword{ get; set; }
    }
}
