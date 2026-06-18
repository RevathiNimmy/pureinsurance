using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.CancelPayment
{
    public class CancelPaymentCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The CashListItemKey field is required")]
        
        public int CashListItemKey { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The ReverseReasonKey field is required")]
        
        public int ReverseReasonKey { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The TransDetailKey field is required")]
        
        public int TransDetailKey { get; set; }
    }
}
