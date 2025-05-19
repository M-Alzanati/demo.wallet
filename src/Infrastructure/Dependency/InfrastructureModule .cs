using System;
using Application.Wallets.Commands;
using Autofac;
using Common;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.Repositories;
using MediatR;

namespace Infrastructure.Dependency
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SerilogAdapter<>))
                .As(typeof(IAppLogger<>))
                .SingleInstance();


            builder.RegisterType<WalletContext>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WalletRepository>()
                .As<IWalletRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WalletTransactionRepository>()
                .As<IWalletTransactionRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateWalletCommandHandler).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<Func<Type, object>>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

        }
    }
}
