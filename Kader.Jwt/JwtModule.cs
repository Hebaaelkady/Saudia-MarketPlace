using System;
using Autofac;
using Kader.Infrastructure.Jwt.authentication;
using Kader.Infrastructure.Jwt.Interfaces;
using Module = Autofac.Module;

namespace Kader.Infrastructure.Jwt
{
    public class JwtModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
                builder.RegisterType<JwtTokenHandler>().As<IJwtTokenHandler>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
                builder.RegisterType<TokenFactory>().As<ITokenFactory>().SingleInstance();
                builder.RegisterType<JwtTokenValidator>().As<IJwtTokenValidator>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}