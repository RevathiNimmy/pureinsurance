using System;
using System.Collections.Generic;
using Ssp.PureSts.Models;

namespace Ssp.PureSts.DataAccess.Interface
{
    public interface IUserService
    {
        UserAttributes ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage);
        string GetOptionSetting( string userName,int optionNumber);
        List<UserRole> GetUserRoles(string userName);
        //bool ValidateUser(string userName, string password);
        //IEnumerable<string> GetUserRoles(string userName);
        //User GetUser(string userName);
        //IEnumerable<string> GetUserBranches(string userName);
    }
}

