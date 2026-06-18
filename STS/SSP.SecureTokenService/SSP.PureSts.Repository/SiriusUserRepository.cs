using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Thinktecture.IdentityServer;
using Thinktecture.IdentityServer.Repositories;
using System.ComponentModel.Composition;
using Ssp.PureSts.DataAccess;
using Ssp.PureSts.DataAccess.Interface;
using Ssp.PureSts.DataAccess.Factory;
using Ssp.PureSts.Models;

namespace Ssp.PureSts.Repository
{
    public class SiriusUserRepository : IUserRepository
    {
        [Import]
        public IClientCertificatesRepository Repository { get; set; }

        public SiriusUserRepository()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        #region IUserRepository interface members
        public UserAttributes ValidateUser(string userName, string password)
        {
            return GetUserService().ValidateUser(userName, password);
        }

        public bool ValidateUser(System.Security.Cryptography.X509Certificates.X509Certificate2 clientCertificate, out string userName)
        {
            return Repository.TryGetUserNameFromThumbprint(clientCertificate, out userName);
        }

        //public List<UserRole> GetRoles(string userName)
        //{
        //    return GetUserRoles(userName);
        //}

        public string GetOptionSetting(string userName, int optionNumber)
        {
            return GetUserService().GetOptionSetting(userName, optionNumber);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage)
        {
            return GetUserService().ChangePassword(userName,  oldPassword,  newPassword,ref sErrorCode,ref sErrorMessage);
        }

        #endregion

        public List<UserRole> GetUserRoles(string userName)
        {
            List<UserRole> roles = GetUserService().GetUserRoles(userName).ToList();

            var IsSysAdmin = (from obj in roles
                              where obj.IsSystemAdmin == true
                              select obj).ToList();
            // add the default Thinktecture roles
            UserRole oRole;
            if (IsSysAdmin.Count > 0)
            {
                oRole = new UserRole();
                oRole.Code = "Admin";
                oRole.Description = Thinktecture.IdentityServer.Constants.Roles.IdentityServerAdministrators;
                roles.Add(oRole);
                //roles.Add(Thinktecture.IdentityServer.Constants.Roles.IdentityServerAdministrators);
            }

            oRole = new UserRole();
            oRole.Code = "User";
            oRole.Description = Thinktecture.IdentityServer.Constants.Roles.IdentityServerUsers;
            roles.Add(oRole);

            //if (roles.Contains("Administrator"))
            //{
            //    roles.Add(Thinktecture.IdentityServer.Constants.Roles.IdentityServerAdministrators);
            //}

            //if (roles.Contains("User"))
            //{
            //    roles.Add(Thinktecture.IdentityServer.Constants.Roles.IdentityServerUsers);
            //}

            return roles;
        }

        #region Private functions
        private IUserService GetUserService()
        {
            return (new ServiceFactory()).Get();
        }

        private string EncryptSiriusPassword(string password)
        {
            return password;
        }
        #endregion
    }
}