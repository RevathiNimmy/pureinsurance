
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveCaseCommandBaseResponse : BaseResponseType
    {
        public int BaseCaseKey { get; set; }
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; }
    }
}
