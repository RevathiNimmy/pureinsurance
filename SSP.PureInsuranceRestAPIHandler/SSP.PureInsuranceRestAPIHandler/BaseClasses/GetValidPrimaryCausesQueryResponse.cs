
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetValidPrimaryCausesQueryResponse : BasePagedResponse
    {
        public List<BaseGetValidPrimaryCausesResponseTypeRow> PrimaryCauses { get; set; }
    }
}
