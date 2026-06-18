using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunDefaultRulesEditRequestType : BaseRequestType
    {
        public int ClaimKey {  get; set; }
        
        public bool ClaimKeySpecified { get; set; }

        public int ClaimPerilKey{ get; set; }

        public bool ClaimPerilKeySpecified { get; set; }

        public string InceptionDateTPI { get; set; }

        public string ScreenCode { get; set; }

        public bool SkipSaveToDB { get;set; }

        public string TransactionType { get; set; }

        public string XMLDataSet { get; set; }
    }
}
