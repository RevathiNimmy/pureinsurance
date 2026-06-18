using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetLockDetailsResponseTypeRow :BaseResponseType
    {
        public List<BaseGetLockDetailsResponseTypeDetailsRow> Details { get; set; }
    }
}
