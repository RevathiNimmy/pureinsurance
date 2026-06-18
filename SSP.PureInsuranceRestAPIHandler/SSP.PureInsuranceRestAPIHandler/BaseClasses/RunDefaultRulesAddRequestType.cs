using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunDefaultRulesAddRequestType : BaseRequestType
    {
        public string ScreenCode { get; set; }

        public bool SkipSaveToDB { get; set; }

        public string XMLDataSet { get; set; }
    }
}
