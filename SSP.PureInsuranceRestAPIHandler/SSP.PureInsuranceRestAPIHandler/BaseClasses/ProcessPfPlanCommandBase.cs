using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ProcessPfPlan
{
    public class ProcessPfPlanCommandBase : BaseRequestType
    {
        public string PartyCode { get; set; }
        public BasePremiumFinanceDetails PFQuote { get; set; }
        public System.Collections.Generic.List<BaseProcessPFPlanRequestTrans> PFTransaction { get; set; }
        public ProcessPFPlanType TransType { get; set; }
        public InstalmentType Type { get; set; }
        public bool SaveOnly { get; set; }
        public int PFPremFinanceKey { get; set; }
        public int PFPremFinanceVersion { get; set; }
        public BaseCreditCardType PFCreditCardDetails { get; set; }
        public BasePFBankDetails PFBankDetails { get; set; }
        public int SourceId { get; set; }
    }
}