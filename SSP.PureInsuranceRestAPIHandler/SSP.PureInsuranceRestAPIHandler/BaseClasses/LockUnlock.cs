using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class LockUnlock
    {
        public LockName LockName { get; set; }
        public int LockValue { get; set; }
        public string LockedBy { get; set; }
        public byte[] ApiTimeStamp { get; set; }
        public PMEReturnCode PMEReturnCode { get; set; }
        public bool IsTimeStampMatched { get; set; }
        public int LockedByUserID { get; set; }
        public bool CurrentlyLocked { get; set; }
        public bool IsCurrentlyLocked { get; set; }
        public string LockedByUser { get; set; }
    }
}
