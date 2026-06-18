using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteRiskCommandBase : BaseRequestType
    {

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //[DefaultValue(0)]
        public int InsuranceFileKey { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFolderKey field is required")]
        //[DefaultValue(0)]
        public int InsuranceFolderKey { get; set; }
        public int OrignalRiskKey { get; set; }

        //[Minength(1, ErrorMessage = "The QuoteTimeStamp field cannot be empty")]
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];

        //[Range(1, int.MaxValue, ErrorMessage = "The RiskKey field is required")]
        //[DefaultValue(0)]
        public int RiskKey { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
