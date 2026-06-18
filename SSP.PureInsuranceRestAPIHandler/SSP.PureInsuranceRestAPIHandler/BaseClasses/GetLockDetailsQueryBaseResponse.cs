using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetLockDetailsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetLockDetailsResponseTypeDetailsRow> Details { get; set; }
    }
}
