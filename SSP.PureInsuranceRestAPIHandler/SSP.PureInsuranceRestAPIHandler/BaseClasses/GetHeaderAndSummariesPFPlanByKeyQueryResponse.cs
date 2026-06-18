using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesPFPlanByKeyQueryResponse : BaseResponseType
    {
        public List<BasePremiumFinancePlanInstalmentsType> Installements { get; set; }
        public List<BasePremiumFinancePlanBankHistoryType> PFBankHistory { get; set; }
        public List<BasePremiumFinancePlanHistoryType> PFHistory { get; set; }
        public BasePremiumFinancePlanDetailsType PremiumFinanceDetails { get; set; }
        public List<BasePremiumFinancePlanTransactionsType> Transactions { get; set; }
        public BasePagedResponse PagedInstallements { get; set; }
        public BasePagedResponse PagedPFBankHistory { get; set; }
        public BasePagedResponse PagedPFHistory { get; set; }
        public BasePagedResponse PagedTransactions { get; set; }
    }
}
