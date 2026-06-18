using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindQuoteCommandBase : BaseRequestType
    {
        public bool AcceptRenewal { get; set; }
        public bool AcceptRenewalSpecified { get; set; }
        public BaseBankGuaranteePaymentType BankGuaranteeDetails { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public bool CoverStartDateSpecified { get; set; }
        public System.Collections.Generic.List<BaseBindQuoteRequestTypeCreditTransactionsRow> CreditTransactions { get; set; }
        public DebitAgainstType DebitAgainst { get; set; }
        public DebitAgainstAccountType DebitAgainstAccount { get; set; }
        public bool DebitAgainstAccountSpecified { get; set; }
        public bool DebitAgainstSpecified { get; set; }
        public bool EnableMasterClientAssociate { get; set; }
        public InstalmentType InstalmentType { get; set; }
        public bool InstalmentTypeSpecified { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public bool IsBackdatedMTA { get; set; }
        public bool NoTrans { get; set; }
        public string OverriddenPolicyNumber { get; set; }
        public bool PayNegativePremiumMTABalance { get; set; }
        public bool PayNegativePremiumMTABalanceSpecified { get; set; }
        public BaseReceiptType PayNowDetails { get; set; }
        public BasePaymentType PayNowPaymentDetails { get; set; }
        public bool PayTrueMonthlyPolicyMTAPremiumOnRenewal { get; set; }
        public bool PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
        public bool PaymentMethodSpecified { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public BaseSelectedCashDepositType SelectedCashDeposit { get; set; }
        public BaseSelectedInstalmentQuoteType SelectedInstalmentQuote { get; set; }
        public string TransactionType { get; set; }
        public bool WritePolicy { get; set; }
        public int PartyBankKey { get; set; }
    }
}
