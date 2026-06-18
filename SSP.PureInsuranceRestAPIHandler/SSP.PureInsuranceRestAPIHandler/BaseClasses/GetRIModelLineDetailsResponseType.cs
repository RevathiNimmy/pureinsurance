using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIModelLineDetailsResponseType : BaseResponseType
    {

        public  List<BaseGetRIModelLineDetailsResponseTypeLinesRow> Lines { get; set; }

    }
}
