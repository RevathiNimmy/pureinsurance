using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Models
{
    class UserAttributes
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        //public string TypeOfUser { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsLocked { get; set; }
        public bool IsInvalidPassword { get; set; }
        public bool IsTempPassword { get; set; }
        public bool IsWeakPassword { get; set; }
        public Nullable<DateTime> PasswordChangeDate { get; set; }
        public string ErrorMessage { get; set; }
    }
}
