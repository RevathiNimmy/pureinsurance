
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductRiskEventsQueryBase :BaseRequestType
    {
        public SSP.PureInsuranceRestAPIHandler.Enums.ProductEventActionType EventType { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public string ProductCode { get; set; }
        public bool ProductCodeSpecified { get; set; }
    }
}
