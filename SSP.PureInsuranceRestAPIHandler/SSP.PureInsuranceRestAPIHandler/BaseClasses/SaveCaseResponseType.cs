using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveCaseResponseType : BaseResponseType
    {
        public int baseCaseKey { get; set; }

        public int caseKey { get; set; }

        public string caseNumber { get; set; }

    }
}
