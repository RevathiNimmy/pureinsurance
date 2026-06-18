using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseMaintainLockRequestType : BaseRequestType
    {
        public Boolean ClearAllLocks { get; set; }

        public List<BaseLockDetails> LockDetails { get;set;}

        public String LogOutSessionValue { get; set; } 
    }
}
