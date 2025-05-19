using System.Net.Http;
using System.Web.Http;
using Api.Filters;
using Api.Handlers;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            AutofacConfig.Register(config);

            if (config.DependencyResolver.GetService(typeof(BasicAuthHandler)) is DelegatingHandler handler)
            {
                config.MessageHandlers.Add(handler);
            }
            config.Filters.Add(new GlobalExceptionFilter());
        }
    }
}
