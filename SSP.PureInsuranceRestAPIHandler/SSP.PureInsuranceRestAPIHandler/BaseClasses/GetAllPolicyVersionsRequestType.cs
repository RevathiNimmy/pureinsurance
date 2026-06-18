using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAllPolicyVersionsRequestType : BaseRequestType
    {
        public int InsuranceFolderKey { get; set; }
        public bool RetrieveAssociates { get; set; }
    }
}
