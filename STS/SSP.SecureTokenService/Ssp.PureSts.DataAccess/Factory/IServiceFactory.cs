using System;
using Ssp.PureSts.DataAccess.Interface;

namespace Ssp.PureSts.DataAccess.Factory
{
    public interface IServiceFactory
    {
        IUserService Get();
    }
}
