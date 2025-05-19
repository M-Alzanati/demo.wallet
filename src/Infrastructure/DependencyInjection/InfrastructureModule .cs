using Application.Wallets.Commands;
using Autofac;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MediatR;
using System;

namespace Infrastructure.DependencyInjection
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
