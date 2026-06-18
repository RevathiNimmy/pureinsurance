using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIVersionQueryResponse : BasePagedResponse
    {
        public List<BaseGetRIVersionResponseTypeRIVersionsRow> ResultData { get; set; }
    }
}
