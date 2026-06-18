using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateStandardPolicyWordingsCommandBase : BaseRequestType
    {

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //[DefaultValue(0)]
        public int InsuranceFileKey { get; set; }      
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
       
        
            public List<BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow> PolicyStandardWordings { get; set; }
    }
}
