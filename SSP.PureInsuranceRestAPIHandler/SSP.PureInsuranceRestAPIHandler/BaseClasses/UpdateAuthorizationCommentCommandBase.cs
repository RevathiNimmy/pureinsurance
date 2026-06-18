using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateAuthorizationComment
{
    public class UpdateAuthorizationCommentCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The CashListKey field is required")]
        
        public int CashListItemId { get; set; }
        
        public string Comment { get; set; }
    }
}
