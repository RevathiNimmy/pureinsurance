using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskOnKeysResponseType : BaseResponseType
    {
        public List<BaseGetWorkManagerScheduledTasksResponseTypeTasksRow> Tasks { get; set; }
    }
}
