using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyBankType
    {
        public string AccountHolderName { get; set; }
        public int AccountKey { get; set; }
        public string AccountType { get; set; }
        public BaseBankType Bank { get; set; }
        public string BankPaymentTypeCode { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public System.Collections.Generic.List<BasePartyBankHistoryType> History { get; set; }
        public bool IsBankItem { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPartyBankInUse { get; set; }
        public bool IsPartyBankLinkedWithInst { get; set; }
        public int PartyBankKey { get; set; }
        public int RowKey { get; set; }
        public int BankPaymentTypeKey { get; set; }

    }
}
