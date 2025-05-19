using System.Web;
using System.Web.Http;
using Serilog;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            HttpContext.Current.Response.Headers.Add("X-Frame-Options", "DENY");
            HttpContext.Current.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Log.Error(exception, "Unhandled exception");
        }
    }
}
