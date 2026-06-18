using SSP.PureInsuranceRestAPIHandler.Enums;
using System;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateWmTaskCommandBase : BaseRequestType
    {
        public WMActionType? ActionType { get; set; }
        public string Client { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public List<BaseUpdateWmTaskRequestTypeRow> KeyData { get; set; }
        public int TaskInstanceKey { get; set; }
        public int TaskStatusKey { get; set; }
        public byte[] TaskTimeStamp { get; set; } = new byte[0];
        public int Urgent { get; set; }
        public string UserCode { get; set; }
        public string UserGroupCode { get; set; } = string.Empty;
    }
}
