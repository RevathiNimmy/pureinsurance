namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteUndeleteUserGroupCommandBase : BaseRequestType
    {
        public bool Deleted { get; set; }
        public string UserGroupCode { get; set; }
    }
}
