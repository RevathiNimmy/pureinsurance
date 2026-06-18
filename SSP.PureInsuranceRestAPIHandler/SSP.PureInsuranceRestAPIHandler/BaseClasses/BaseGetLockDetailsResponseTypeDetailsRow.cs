namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetLockDetailsResponseTypeDetailsRow
    {
        //[DBCol("IsExclusiveLock")]
        public int IsExclusiveLock { get; set; }
        //[DBCol("IsSystemLock")]
        public int IsSystemLock { get; set; }
        //[DBCol("Lock2Value")]
        public int Lock2Value { get; set; }
        //[DBCol("LockName")]
        public string LockName { get; set; }
        //[DBCol("LockValue")]
        public int LockValue { get; set; }
        //[DBCol("LockedByID")]
        public int LockedByID { get; set; }
        //[DBCol("LockedTime")]
        public System.DateTime LockedTime { get; set; }
        //[DBCol("UserName")]
        public string UserName { get; set; }
        //[DBCol("Lock3Value")]
        public string Lock3Value { get; set; }
    }
}
