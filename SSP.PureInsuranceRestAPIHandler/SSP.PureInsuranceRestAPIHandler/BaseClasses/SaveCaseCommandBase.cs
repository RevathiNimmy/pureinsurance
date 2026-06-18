
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveCaseCommandBase : BaseRequestType
    {
        public int CaseKey { get; set; }
        public bool CaseKeySpecified { get; set; }
        public string CaseNumber { get; set; }
        public DateTime CaseOpenDate { get; set; }
        public string Assistant { get; set; }
        public string Analyst { get; set; }
        public string ProgressStatusCode { get; set; }
        public string EventDescription { get; set; }
    }
}
