using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class MaintainLockCommandBase : BaseRequestType
    {
        public bool ClearAllLocks { get; set; }
        public List<BaseLockDetails> LockDetails { get; set; }
        public string LogOutSessionValue { get; set; }
    }
}
