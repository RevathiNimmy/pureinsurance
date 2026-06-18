
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunDefaultRulesEditCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public bool ClaimKeySpecified { get; set; }
        public int ClaimPerilKey { get; set; }
        public bool ClaimPerilKeySpecified { get; set; }
        public DateTime InceptionDateTPI { get; set; } = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public string ScreenCode { get; set; }
        public bool SkipSaveToDB { get; set; }
        public string TransactionType { get; set; }
        public string XMLDataSet { get; set; }
    }
}
