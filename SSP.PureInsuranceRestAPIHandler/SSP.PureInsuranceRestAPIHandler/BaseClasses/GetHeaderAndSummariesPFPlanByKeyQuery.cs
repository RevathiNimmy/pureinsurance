using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesPFPlanByKeyQuery : GetHeaderAndSummariesPFPlanByKeyQueryBase
    {
        public int InstallementsPageNumber { get; set; }
        public int InstallementsPageSize { get; set; }
        public int PFBankHistoryPageNumber { get; set; }
        public int PFBankHistoryPageSize { get; set; }
        public int PFHistoryPageNumber { get; set; }
        public int PFHistoryPageSize { get; set; }
        public int TransactionsPageNumber { get; set; }
        public int TransactionsPageSize { get; set; }
        public string InstallementsSortBy { get; set; }
        public string PFBankHistorySortBy { get; set; }
        public string PFHistorySortBy { get; set; }
        public string TransactionsSortBy { get; set; }
        public bool ExclusiveLock { get; set; }
        public string SessionValue { get; set; }
    }
}
