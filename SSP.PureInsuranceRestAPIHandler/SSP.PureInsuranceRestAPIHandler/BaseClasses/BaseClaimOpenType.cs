using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimOpenType : BaseClaimType
    {
        public System.Collections.Generic.List<BaseClaimPerilType> ClaimPeril { get; set; }
        public string DuplicateClaimOverrideUserName { get; set; }
        public string DuplicateClaimOverrideUserPassword { get; set; }
        public bool IgnoreWarnings { get; set; }
        public string UnderwritingYearCode { get; set; }
        public bool IgnoreClaimNumberValidation { get; set; }
        public int InsurerAddressTypeId { get; set; }

        public bool ExternalHandler { get; set; }
        public int SamStagingClaimKey { get; set; }
    }
}
