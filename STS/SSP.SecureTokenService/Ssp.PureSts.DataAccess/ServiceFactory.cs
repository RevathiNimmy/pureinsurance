using System;
using Ssp.PureSts.DataAccess.Interface;
using Ssp.PureSts.DataAccess.Factory;

namespace Ssp.PureSts.DataAccess
{
    public class ServiceFactory : IServiceFactory
    {
        public IUserService Get()
        {
            // TODO: load based on setting in config file. 
            // We Can return a factory on basic of config settings.For now we have just a option for WCF factory
            return(new ServiceFactoryWCF()).Get();            
        }
    }
}
