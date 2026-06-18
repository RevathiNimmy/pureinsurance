namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetUserPreferredColumnList
{
    public class GetUserPreferredColumnListQueryBaseResponse : BaseResponseType
    {
        public string ColumnList { get; set; }
        public string InterfaceName { get; set; }
        public string UserName { get; set; }
    }
}
