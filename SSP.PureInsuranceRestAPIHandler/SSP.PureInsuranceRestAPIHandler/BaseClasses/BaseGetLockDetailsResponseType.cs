using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetLockDetailsResponseType : BaseResponseType
    {
        public System.Collections.Generic.List<BaseGetLockDetailsResponseTypeDetailsRow> Details { get; set; }
    }
}
