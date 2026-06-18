/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.ComponentModel.Composition;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityServer.Repositories;
using Ssp.PureSts.Models;

namespace Thinktecture.IdentityServer.TokenService
{
    public class RepositoryUserNameSecurityTokenHandler : GenericUserNameSecurityTokenHandler
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        protected override bool ValidateUserNameCredentialCore(string userName, string password)
        {
            Container.Current.SatisfyImportsOnce(this);
            UserAttributes objUserAttribute=UserRepository.ValidateUser(userName, password);
            return objUserAttribute.IsAuthenticated;
        }
    }
}
