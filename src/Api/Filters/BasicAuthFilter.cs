using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http;
using MediatR;
using Application.Users.Queries;
using Common;

namespace Api.Filters
{
    public class BasicAuthFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return true;
            }

            var request = actionContext.Request;
            var authHeader = request.Headers.Authorization;

            if (authHeader == null || authHeader.Scheme != "Basic")
                return false;

            try
            {
                var encoded = authHeader.Parameter;
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                var parts = decoded.Split(':');
                if (parts.Length != 2) return false;

                var email = parts[0];
                var password = parts[1];

                var mediator = (IMediator)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IMediator));
                var user = mediator.Send(new ValidateUserQuery(email, password), CancellationToken.None).GetAwaiter().GetResult();
                if (user == null || user.PasswordHash != PasswordHelper.ComputeSha256Hash(password))
                    return false;

                var identity = new ClaimsIdentity("Basic");
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}