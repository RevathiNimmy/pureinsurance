namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindClaimCommandBaseResponse : BaseClaimResponseType
    {
        public BaseCashListResponseType CashList { get; set; }
        public int CreditedAccountKey { get; set; }
        public int CreditedDocumentKey { get; set; }
        public int CreditedTransdetailKey { get; set; }
    }
}
