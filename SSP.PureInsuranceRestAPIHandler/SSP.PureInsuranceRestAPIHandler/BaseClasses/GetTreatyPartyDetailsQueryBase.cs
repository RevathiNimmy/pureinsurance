using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTreatyPartyDetailsQueryBase : BaseRequestType
    {
        public bool IsRIDisabled { get; set; }
        public string TreatyCode { get; set; }
    }
}
