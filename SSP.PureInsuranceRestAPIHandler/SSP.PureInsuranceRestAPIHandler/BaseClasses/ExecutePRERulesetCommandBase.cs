using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ExecutePRERulesetCommandBase : BaseRequestType
    {
        public DateTime CoverStartDate { get; set; }
        public bool IgnoreErrorMessage { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public string PRERuleAssemblyName { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public string RiskDescription { get; set; }
        public int RiskKey { get; set; }
        public bool RunPostPRERule { get; set; }
        public bool RunPrePRERule { get; set; }
        public string ScreenCode { get; set; }
        public string SubBranchCode { get; set; }
        public string TransactionType { get; set; }
        public string XMLDataSet { get; set; }
    }
}
