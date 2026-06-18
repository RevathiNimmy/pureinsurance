using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.CancelReceipt
{
    public class CancelReceiptCommandBase : BaseRequestType
    {
        
        //(1, int.MaxValue, ErrorMessage = "The CashListItemKey field is required")]
        
        public int CashListItemKey { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        
        public string ReverseReasonCode { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The TransDetailKey field is required")]
        
        public int TransDetailKey { get; set; }
    }
}
