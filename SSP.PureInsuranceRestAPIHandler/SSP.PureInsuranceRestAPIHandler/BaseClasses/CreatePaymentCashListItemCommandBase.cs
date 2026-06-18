using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.CreatePaymentCashListItem
{
    public class CreatePaymentCashListItemCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The CashListKey field is required")]
        
        public int CashListKey { get; set; }
        public System.Collections.Generic.List<BasePaymentCashListItemType> PaymentItem { get; set; }
    }
}
