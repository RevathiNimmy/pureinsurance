namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetUserPreferredColumnList
{
    public class GetUserPreferredColumnListQueryBase : BaseRequestType
    {
        
        public string InterfaceName { get; set; }
        
        public string UserName { get; set; }
    }
}
