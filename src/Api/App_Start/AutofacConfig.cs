using System;
using System.Reflection;
using System.Web.Http;
using Api.Handlers;
using Application.Dependency;
using Autofac;
using Autofac.Integration.WebApi;
using Infrastructure.Dependency;
using MediatR;

namespace Api
{
    public class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<ApplicationModule>();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<Func<Type, object>>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterWebApiModelBinderProvider();
            builder.RegisterType<BasicAuthHandler>().InstancePerLifetimeScope();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}