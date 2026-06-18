using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveCaseRequestType :  BaseRequestType
    {
        public string analyst { get; set; }

        public string assistant { get; set; }

        public int caseKey { get; set; }

        public bool caseKeySpecified { get; set; }

        public string caseNumber { get; set; }

        public DateTime caseOpenDate { get; set; }

        public string eventDescription { get; set; }

        public string progressStatusCode { get; set; }

    }
}
