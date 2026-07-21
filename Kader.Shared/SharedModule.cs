using System;
using Autofac;
using Kader.Infrastructure.Shared;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Module = Autofac.Module;

namespace Kader.Infrastructure.Jwt
{
    public class SharedModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                builder.RegisterType<Keys>().As<IKeys>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
                builder.RegisterType<Methods>().As<IMethods>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}