using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ssp.PureSts.Models;

namespace Ssp.PureSts.Repository.Interfaces
{
    public interface ISiriusUserRepository
    {
        UserAttributes ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage);
        string GetOptionSetting(string userName, int optionNumber);
        List<UserRole> GetUserRoles(string userName);

/*        bool ValidateUser(string userName, string password);
        bool ValidateUser(X509Certificate2 clientCertificate, out string userName);
        IEnumerable<string> GetRoles(string userName);*/
    }
}
