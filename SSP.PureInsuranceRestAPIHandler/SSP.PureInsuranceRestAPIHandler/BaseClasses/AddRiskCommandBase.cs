using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddRiskCommandBase : BaseRequestType
    {
        public string SubBranchCode { get; set; }

        public int InsuranceFileKey { get; set; }

        public int InsuranceFolderKey { get; set; }

        public string ProductCode { get; set; }

        public string RiskTypeCode { get; set; }

        public string ScreenCode { get; set; }

        public string RiskDescription { get; set; }

        public string DataModelCode { get; set; }

        public bool RunDefaultRules { get; set; }

        //[Minength(1, ErrorMessage = "The QuoteTimeStamp field is required")]
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];

        public string XMLDataSet { get; set; }


        public System.Collections.Generic.List<BaseRiskRatingSectionType> RatingSections { get; set; }

        public System.Collections.Generic.List<BaseRiskRIArrangementType> RIArrangement { get; set; }

        public bool Data_Transfer { get; set; }

        public bool IsMarketplacePolicy { get; set; }
    }
}
