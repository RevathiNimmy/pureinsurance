using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPaymentHubSystemOptionsResponseType : BaseResponseType
    {
        public string AccountID { get; set; }
        public string AccountPassCode { get; set; }
        public string BrokerSCID { get; set; }
        public string CaptureMethod { get; set; }
        public string ClientName { get; set; }
        public string Customer { get; set; }
        public string Donotuseoldcarddetailsforsubsequentpayments { get; set; }
        public string LanguageTemplateID { get; set; }
        public string MarkDefaultCreditCard { get; set; }
        public string MerchantID { get; set; }
        public string MerchantTemplateID { get; set; }
        public string Password { get; set; }
        public string PaymentHubServiceUrl { get; set; }
        public string RefundPasscode { get; set; }
        public string RefundPremiumthroughInvoice { get; set; }
        public string ReturnURL { get; set; }
        public string SystemGUID { get; set; }
        public string SystemPasscode { get; set; }
        public string SystemUserName { get; set; }
        public string TransactionIPAddress { get; set; }
    }
}
