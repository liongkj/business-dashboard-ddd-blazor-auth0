using System.Reflection;
using Autofac;
using JomMalaysia.Framework.Configuration;
using JomMalaysia.Presentation.Gateways.WebServices;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Scope;

namespace JomMalaysia.Presentation
{
    public class PresentationModule : Autofac.Module
    {
       
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<WebServiceExecutor>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<WebServiceResponse>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ApiBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            
            builder.RegisterType<AuthorizationManagers>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<AppSetting>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HasScopeHandler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(gateway => gateway.Name.EndsWith("Gateway"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}