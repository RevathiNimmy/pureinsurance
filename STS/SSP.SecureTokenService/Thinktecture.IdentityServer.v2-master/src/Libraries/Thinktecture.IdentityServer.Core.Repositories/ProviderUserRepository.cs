/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using Ssp.PureSts.Models;

namespace Thinktecture.IdentityServer.Repositories
{
    public class ProviderUserRepository : IUserRepository
    {
        [Import]
        public IClientCertificatesRepository Repository { get; set; }

        public ProviderUserRepository()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public UserAttributes ValidateUser(string userName, string password)
        {
            return ValidateUser(userName, password);
        }

        public bool ValidateUser(X509Certificate2 clientCertificate, out string userName)
        {
            return Repository.TryGetUserNameFromThumbprint(clientCertificate, out userName);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword,ref string sErrorCode,ref string sErrorMessage)
        { 
            return ChangePassword( userName,  oldPassword,  newPassword,ref sErrorCode,ref sErrorMessage);
        }

        public List<UserRole> GetUserRoles(string userName)
        {
            //var returnedRoles = new List<UserRole>();

            if (Roles.Enabled)
            {
                List<UserRole> oRoles = GetUserRoles(userName);
                var returnedRoles = (from obj in oRoles
                                     where obj.Description.StartsWith(Constants.Roles.InternalRolesPrefix)
                                     select obj).ToList();
                //var roles = Roles.GetRolesForUser(userName);
                //var returnedRoles=(from obj in roles where obj.
                //returnedRoles = roles.Where(role => role.StartsWith(Constants.Roles.InternalRolesPrefix)).ToList();    
                return returnedRoles;
            }
            else
            {
                return null;
            }
            
        }

        public string GetOptionSetting(string userName, int optionNumber)
        {
            return GetOptionSetting(userName, optionNumber);
        }
    }
}