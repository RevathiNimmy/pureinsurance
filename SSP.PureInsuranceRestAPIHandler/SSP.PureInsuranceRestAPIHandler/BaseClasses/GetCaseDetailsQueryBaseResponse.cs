
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCaseDetailsQueryBaseResponse : BasePagedResponse
    {
        public string Analyst { get; set; }
        public string Assistant { get; set; }
        public int BaseCaseKey { get; set; }
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; }
        public DateTime CaseOpenedDate { get; set; }
        public string CaseProgressDescription { get; set; }
        public string CaseProgressStatusCode { get; set; }
        public int CaseVersion { get; set; }
        public List<BaseCaseItemsResponseTypeLinkedClaimsRow> LinkedClaims { get; set; }
        public decimal TotalExcess { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalIndemnity { get; set; }
    }
}
