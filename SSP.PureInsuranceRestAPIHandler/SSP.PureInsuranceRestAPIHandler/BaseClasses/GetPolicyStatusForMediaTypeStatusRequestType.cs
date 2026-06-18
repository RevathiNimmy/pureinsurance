using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyStatusForMediaTypeStatusRequestType :BaseRequestType
    {

        public int InsuranceFileKey { get; set; }
        public DateTime LossDate { get; set; }
        public bool LossDateSpecified { get; set; }

    }
}
