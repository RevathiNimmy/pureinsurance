
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskOnKeysQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetWorkManagerScheduledTasksResponseTypeTasksRow> Tasks { get; set; }
    }
}
