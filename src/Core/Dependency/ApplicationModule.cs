using System;
using System.Reflection;
using Application.Common.Behaviors;
using Application.Wallets.Commands;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Module = Autofac.Module;

namespace Application.Dependency
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = MediatRConfigurationBuilder
                .Create(typeof(CreateWalletCommand).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();

            builder.RegisterMediatR(configuration);

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(ResilienceBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.Register<Func<Type, object>>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();
        }
    }
}
