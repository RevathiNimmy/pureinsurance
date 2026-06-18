using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimMaintainType : BaseClaimType
    {
        public System.Collections.Generic.List<BaseClaimPerilMaintainType> ClaimPeril { get; set; }
        public bool ExternalHandler { get; set; }
        public bool IgnoreWarnings { get; set; }
        public int SamStagingClaimKey { get; set; }
    }
}
