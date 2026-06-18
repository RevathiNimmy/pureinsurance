using System;
using System.Collections.Generic;

namespace Ssp.PureSts.Models
{
    [Serializable]
    public class UserAttributes
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsLocked { get; set; }
        public bool IsInvalidPassword { get; set; }
        public bool IsTempPassword { get; set; }
        public bool IsWeakPassword { get; set; }
        public bool IsSystemUpgradeChangePasswordRequired { get; set; }
        public Nullable<DateTime> PasswordChangeDate { get; set; }
        public Nullable<DateTime> PasswordExpiryDate { get; set; }
        public string ErrorMessage { get; set; }
    }

    [Serializable]
    public class UserRole
    {
        public string Code { get; set; }   
        public string Description { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystemAdmin { get; set; }
        public int UserGroupKey { get; set; }
    }
}




