using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPartyBankDetailsQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public string BankPaymentTypeCode { get; set; }
        public string ISBank { get; set; }
        public bool IncludeHistory { get; set; }
        public bool IncludeLastTransactedPartyBankKey { get; set; }
        public LastTransactionTypeWithPartyBank LastTransactionType { get; set; }
        public int PartyBankKey { get; set; }
        public int PartyKey { get; set; }
        public int TransactionKey { get; set; }
    }
}
