using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesForRenewalSelectionQueryBase : BaseRequestType
    {

        public string ProductCode { get; set; }
       
        public System.DateTime CompareDate { get; set; }
        public System.DateTime StartDate { get; set; }
   
        public bool StartDateSpecified { get; set; }



    }
}
