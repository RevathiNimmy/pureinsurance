using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCaseDetailsResponseType :BaseResponseType
    {
        public string analyst { get; set; }

        public string assistant { get; set; }

        public int baseCaseKey { get; set; }

        public int caseKey { get; set; }

        public string caseNumber { get; set; }

        public DateTime caseOpenedDate { get; set; }

        public string caseProgressDescription { get; set; }

        public string caseProgressStatusCode { get; set; }

        public int caseVersion { get; set; }

        public List<BaseCaseItemsResponseTypeLinkedClaimsRow> linkedClaims { get; set; }

        public decimal totalExcess { get; set; }

        public decimal totalExpense { get; set; }

        public decimal totalIndemnit { get; set; }
    }
}
