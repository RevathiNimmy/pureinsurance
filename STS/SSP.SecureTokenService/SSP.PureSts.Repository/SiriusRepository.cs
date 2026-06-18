using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using Ssp.PureSts.Repository.Interfaces;
using Thinktecture.IdentityServer.Repositories;
using Ssp.PureSts.Models;

namespace Ssp.PureSts.Repository
{
    public class SiriusRepository : IUserRepository 
    {
        [Import]
        public IClientCertificatesRepository Repository { get; set; }

        [Import]
        public ISiriusUserRepository IsiriusUserRepository { get; set; }

        public SiriusRepository()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        /// <summary>
        /// To validate an user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAttributes ValidateUser(string userName, string password)
        {
            return IsiriusUserRepository.ValidateUser(userName, password);;

        }

        /// <summary>
        /// To validate an user using certificate-Not in use
        /// </summary>
        /// <param name="clientCertificate"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ValidateUser(X509Certificate2 clientCertificate, out string userName)
        {
            return Repository.TryGetUserNameFromThumbprint(clientCertificate, out userName);
        }

        /// <summary>
        /// To Get associated BO user tasks groups
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<UserRole> GetUserRoles(string userName)
        {
            return IsiriusUserRepository.GetUserRoles(userName);
        }

        /// <summary>
        /// To get value for a system option
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="optionNumber"></param>
        /// <returns></returns>
        public string GetOptionSetting(string userName, int optionNumber)
        {
            return IsiriusUserRepository.GetOptionSetting(userName, optionNumber);
        }

        /// <summary>
        /// To change password for logged in user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="sErrorCode"></param>
        /// <param name="sErrorMessage"></param>
        /// <returns></returns>
        public bool ChangePassword(string userName, string oldPassword, string newPassword, ref string sErrorCode, ref string sErrorMessage)
        {
            return IsiriusUserRepository.ChangePassword(userName, oldPassword, newPassword, ref sErrorCode, ref sErrorMessage);
        }
    }
}
