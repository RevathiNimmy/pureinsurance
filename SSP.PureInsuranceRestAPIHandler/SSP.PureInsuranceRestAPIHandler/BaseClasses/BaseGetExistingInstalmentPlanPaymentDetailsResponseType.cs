using System;
using System.Collections.Generic;
using System.Text;
using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetExistingInstalmentPlanPaymentDetailsResponseType : BaseResponseType
    {
        public string BIC { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public BaseAddressType BankAddress { get; set; }
        public string BankAreaCode { get; set; }
        public string BankBranch { get; set; }
        public string BankExtn { get; set; }
        public string BankFax { get; set; }
        public string BankFaxCode { get; set; }
        public string BankName { get; set; }
        public string BankPhone { get; set; }
        public string BankSortCode { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public string IBAN { get; set; }
        public int PFRF_ID { get; set; }
    }
}
