using System;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentSettingsQueryResponse : BaseResponseType
    {
        public int AllowConsolidatedCommission { get; set; }
        public int AlternateReferenceForEachTransaction { get; set; }
        public int AlternateReferenceMandatory { get; set; }
        public List<BaseGetAgentSettingsResponseTypeBranchesRow> Branches { get; set; }
        public int CanMakeLiveBankGuarantee { get; set; }
        public int CanMakeLiveCashDeposit { get; set; }
        public int CanMakeLiveInstalments { get; set; }
        public int CanMakeLiveInvoice { get; set; }
        public int CanMakeLivePaynow { get; set; }
        public List<BaseContactType> Contacts { get; set; }
        public string CorrespondenceType { get; set; }
        public int DaysAllowed { get; set; }
        public int DomiciledForTax { get; set; }
        public decimal ExpectedDailyPremium { get; set; }
        public decimal FloatBalanceLimit { get; set; }
        public int IsFloatBalanceAccount { get; set; }
        public int IsOverdraftAccount { get; set; }
        public int IsPrepaymentAccount { get; set; }
        public bool IsReceiveClientCorrespondence { get; set; }
        public int IsStandardAccount { get; set; }
        public DateTime OverdraftExpiry { get; set; }
        public decimal OverdraftLimit { get; set; }
        public DateTime PartyAgentDateCancelled { get; set; }
        public int UseOverrideCommissionRate { get; set; }
        public List<BaseUserDetailsType> Users { get; set; }
        public BasePagedResponse PagedBranches { get; set; }
        public BasePagedResponse PagedContacts { get; set; }
        public BasePagedResponse PagedUsers { get; set; }
    }
}
