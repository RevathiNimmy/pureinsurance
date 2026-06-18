using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClientDataImportResponseTypePolicyVersion
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public System.Collections.Generic.List<BaseClientDataImportResponseTypePolicyVersionClaim> Claim { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public System.Collections.Generic.List<BaseClientDataImportResponseTypePolicyVersionRisks> Risks { get; set; }
        public int SAMStagingPolicyKey { get; set; }
    }
}
