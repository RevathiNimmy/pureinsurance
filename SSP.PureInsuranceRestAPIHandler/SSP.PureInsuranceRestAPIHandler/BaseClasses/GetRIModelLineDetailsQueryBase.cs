using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIModelLineDetailsQueryBase :BaseRequestType
    {
        public string RIModelCode { get; set; }
        public int FilterType { get; set; }
        public string TreatyTypeCode { get; set; }
        public long RIArrangementID { get; set; }
        
    }
}
