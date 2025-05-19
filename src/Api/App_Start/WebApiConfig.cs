using System.Web.Http;
using Api.Filters;

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
            config.Filters.Add(new GlobalExceptionFilter());
            config.Filters.Add(new BasicAuthFilter());
        }
    }
}
