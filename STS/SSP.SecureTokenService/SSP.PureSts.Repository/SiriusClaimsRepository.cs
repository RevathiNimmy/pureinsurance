using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Profile;
using System.Web.Security;
using Thinktecture.IdentityServer.TokenService;
using Thinktecture.IdentityServer.Repositories;
using Ssp.PureSts.Repository.Interfaces;
using System.ComponentModel.Composition;
using Ssp.PureSts.DataAccess;
using Ssp.PureSts.DataAccess.Interface;
using Ssp.PureSts.Models;
using Ssp.PureSts.Constants;
using System.Web;

namespace Ssp.PureSts.Repository 
{
    public class SiriusClaimsRepository : IClaimsRepository
    {
        [Import]
        ISiriusUserRepository UserRepository { get; set; }

        /// <summary>
        /// To get user claims
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="requestDetails"></param>
        /// <returns></returns>
        public virtual IEnumerable<Claim> GetClaims(ClaimsPrincipal principal, RequestDetails requestDetails)
        {
            var userName = principal.Identity.Name;
            var claims = new List<Claim>();

            // add the user's roles
            GetRolesForToken(userName).ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            UserAttributes user = (UserAttributes)HttpContext.Current.Session["User"];
            if (user != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                // add the email address
                if (!string.IsNullOrWhiteSpace(user.EmailAddress))
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
                }

            }
            else if (principal.Identity.Name != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, principal.Identity.Name));
            }

            
            return claims;
        }


        protected virtual string GetProfileClaimType(string propertyName)
        {
            if (StandardClaimTypes.Mappings.ContainsKey(propertyName))
            {
                return StandardClaimTypes.Mappings[propertyName];
            }
            else
            {
                return propertyName;
            }
        }

        /// <summary>
        /// To get supported claim types
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetSupportedClaimTypes()
        {
            var claimTypes = new List<string>
            {
                ClaimTypes.Name,
                ClaimTypes.Email,
                ClaimTypes.Role
            };

            // TODO: set the list of allowable claim types

            if (ProfileManager.Enabled)
            {
                foreach (SettingsProperty prop in ProfileBase.Properties)
                {
                    claimTypes.Add(GetProfileClaimType(prop.Name.ToLowerInvariant()));
                }
            }

            return claimTypes;
        }


        /// <summary>
        /// TO get roles for a security token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected virtual IEnumerable<string> GetRolesForToken(string userName)
        {
            var returnedRoles = new List<string>();
            GetUserService().GetUserRoles(userName).ToList().ForEach(role => returnedRoles.Add(role.Code));

            return returnedRoles;
        }

        #region Private functions
        private IUserService GetUserService()
        {
            return (new ServiceFactory()).Get();
        }
        #endregion
    }
}