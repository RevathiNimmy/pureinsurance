using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses 
{
    public class GetListofUnapprovedPaymentQueryBase : BaseRequestType
    {
        public string AssignedTo { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string CashListItemKey { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string PayeeName { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string ShowAllOtherPayments { get; set; } = string.Empty;
    }
}
