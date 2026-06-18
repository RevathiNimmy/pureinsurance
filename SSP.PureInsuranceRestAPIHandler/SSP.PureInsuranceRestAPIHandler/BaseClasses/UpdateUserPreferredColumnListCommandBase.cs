namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateUserPreferredColumnList
{
    public class UpdateUserPreferredColumnListCommandBase : BaseRequestType
    {
        
        public string ColumnList { get; set; }
        
        public string InterfaceName { get; set; }
        
        public string UserName { get; set; }
    }
}
