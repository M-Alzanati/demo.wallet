using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using MediatR;
using Application;
using Application.DependencyInjection;
using Application.Wallets.Commands;
using Infrastructure.DependencyInjection;
using System.Web.Http.Dependencies;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }

}
