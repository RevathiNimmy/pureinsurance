using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductRiskEventsRequestType : BaseRequestType
    {
        public SSP.PureInsuranceRestAPIHandler.Enums.ProductEventActionType EventType {get;set;}
        public int InsuranceFileKey { get;set;}
        public Boolean InsuranceFileKeySpecified { get; set; } 
        public string ProductCode { get; set; }
        public Boolean ProductCodeSpecified { get; set; }
    }
}
 
