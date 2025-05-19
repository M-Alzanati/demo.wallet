using System.Web.Http;
using Api.Filters;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new GlobalExceptionFilter());

            config.MapHttpAttributeRoutes();

            // Default route (optional)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            AutofacConfig.Register(config);
        }
    }
}
