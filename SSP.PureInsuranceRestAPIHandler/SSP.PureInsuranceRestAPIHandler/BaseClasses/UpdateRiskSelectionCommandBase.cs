using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRiskSelectionCommandBase : BaseRequestType
    {

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //[DefaultValue(0)]
        public int InsuranceFileKey { get; set; }      
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public int InsuranceFolderKey { get; set; }
        public int RiskKey { get; set; }
        public int IsSelected { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
