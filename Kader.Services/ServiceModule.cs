using System;
using System.Linq;
using Autofac;
using Module = Autofac.Module;

namespace Kader.Services
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                foreach (Type item in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.EndsWith("Service") && t.IsClass))
                {
                    builder.RegisterType(item).As(item.GetInterfaces().First(t => t.Name.EndsWith("Service"))).InstancePerLifetimeScope();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
