using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIModelLineDetailsQueryResponse : BasePagedResponse
    {
        public List<BaseGetRIModelLineDetailsResponseTypeLinesRow> Lines { get; set; }
    }
}
