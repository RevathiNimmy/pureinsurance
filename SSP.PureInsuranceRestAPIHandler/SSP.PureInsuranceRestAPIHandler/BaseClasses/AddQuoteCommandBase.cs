using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteCommandBase : BaseQuoteType
    {
        public string CoInsurancePlacement { get; set; }
        public bool ConsolidatedLeadAgentCommission { get; set; }
        public bool ConsolidatedLeadAgentCommissionSpecified { get; set; }
        public bool ConsolidatedSubAgentCommission { get; set; }
        public bool ConsolidatedSubAgentCommissionSpecified { get; set; }
        public string CoverNoteBookNumber { get; set; }
        public int CoverNoteSheetNumber { get; set; }
        public bool CoverNoteSheetNumberSpecified { get; set; }
        public int PartyKey { get; set; }
        public string SubBranchCode { get; set; }
    }
}
