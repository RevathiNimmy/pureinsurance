namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckRetainedCoInsurerExistsCommandBase : BaseRequestType
    {
        public System.Collections.Generic.List<int> CoInsurerKeys { get; set; }
    }
}
