using System;
using Ssp.PureSts.DataAccess;
using Ssp.PureSts.DataAccess.Interface;
using Ssp.PureSts.DataAccess.ADO;

namespace Ssp.PureSts.DataAccess.Factory
{
    public class ServiceFactoryWCF : IServiceFactory
    {
        public IUserService Get()
        {
            return new UserService();
            //throw new NotImplementedException();
        }
    }
}
