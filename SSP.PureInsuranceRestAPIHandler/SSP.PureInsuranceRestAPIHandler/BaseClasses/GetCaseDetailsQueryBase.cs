using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCaseDetailsQueryBase : BaseRequestType
    {
        
        public string Analyst { get; set; }
        
        public string Assistant { get; set; }
        public int CaseKey { get; set; }
        public bool CaseKeySpecified { get; set; }
        public string CaseNumber { get; set; }
        
        public DateTime CaseOpenDate { get; set; }
        
        public string EventDescription { get; set; }
        
        public string ProgressStatusCode { get; set; }
    }
}
