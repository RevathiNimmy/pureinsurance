using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetValidPrimaryCausesResponseType : BaseResponseType
    {
        public List<BaseGetValidPrimaryCausesResponseTypeRow> primaryCauses { get; set; }

    }
}
