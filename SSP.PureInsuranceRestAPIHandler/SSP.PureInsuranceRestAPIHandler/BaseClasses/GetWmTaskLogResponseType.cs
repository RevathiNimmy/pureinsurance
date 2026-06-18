using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWmTaskLogResponseType : BaseResponseType
    {
       public  List<BaseGetWmTaskLogResponseTypeTaskLogRow> TaskLog { get; set; }
    }
}
