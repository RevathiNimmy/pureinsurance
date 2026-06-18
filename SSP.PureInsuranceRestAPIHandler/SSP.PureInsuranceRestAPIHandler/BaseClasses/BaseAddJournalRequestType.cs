using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddJournalRequestType : BaseRequestType
    {

        public string JournalTypeCode { get; set; }
        public System.DateTime JournalDate { get; set; }
        public bool JournalDateSpecified { get; set; }
        public string Comment { get; set; }
        public BaseAddJournalRecurringDetails RecurringDetails { get; set; }
        public BaseAddJournalReversalDetails ReversalDetails { get; set; }

        public System.Collections.Generic.List<BaseAddJournalTransaction> Transactions { get; set; }
        public string JournalBranchCode { get; set; }
        public string JournalSubBranchCode { get; set; }
        public int isApproved { get; set; }
        public int is_reffered { get; set; }
        public int ManualJournalId { get; set; }
    }
}