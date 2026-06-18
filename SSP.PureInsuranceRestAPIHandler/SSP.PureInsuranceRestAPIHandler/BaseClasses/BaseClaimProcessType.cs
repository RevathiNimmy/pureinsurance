using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessType : BaseClaimType
    {
        public int ClaimKey { get; set; }
        public new string CurrencyCode { get; set; }
        public string GisScreenCode { get; set; }
        public new int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public string UnderwritingYearCode { get; set; }
        public System.Collections.Generic.List<BaseClaimProcessPerilType> ClaimPeril { get; set; }
        public bool IgnoreWarnings { get; set; }
        public bool ExternalHandler { get; set; }
        public System.Collections.Generic.List<BaseClaimProcessBuilderRiskType> ClaimBuilderDetail { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public bool IsTPASettleDirectly { get; set; }

        public bool IsTPASpecified { get; set; }

        public BaseClaimReceiptPayeeType ReceiptPayee { get; set; }
    }
}
