using SSP.PureInsuranceRestAPIHandler.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlansQueryBase : BaseRequestType
    {
        public int PartyKey { get; set; }
        public FinancePlanStatus StatusKey { get; set; }
    }
}
