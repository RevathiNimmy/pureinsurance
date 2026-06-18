/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Ssp.PureSts.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public interface IUserRepository
    {

        UserAttributes ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage);
        string GetOptionSetting(string userName, int optionNumber);
        List<UserRole> GetUserRoles(string userName);
        bool ValidateUser(X509Certificate2 clientCertificate, out string userName);

        //bool ValidateUser(string userName, string password);
        //bool ValidateUser(X509Certificate2 clientCertificate, out string userName);
        //IEnumerable<string> GetRoles(string userName);        
    }
}