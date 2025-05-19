using Application.Users;
using Application.Users.Queries;
using MediatR;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Application.Users.Dtos;

namespace Api.Handlers
{
    public class BasicAuthHandler : DelegatingHandler
    {
        private readonly IMediator _mediator;

        public BasicAuthHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Basic")
                return UnauthorizedResponse();

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(request.Headers.Authorization.Parameter)).Split(':');
            if (credentials.Length != 2)
                return UnauthorizedResponse();

            var email = credentials[0];
            var password = credentials[1];

            var user = (await _mediator.Send(new ValidateUserQuery(email, password), cancellationToken)) as UserDto;

            if (user == null)
                return UnauthorizedResponse();

            var identity = new GenericIdentity(user.Email);
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            if (HttpContext.Current != null)
                HttpContext.Current.User = Thread.CurrentPrincipal;

            return await base.SendAsync(request, cancellationToken);
        }

        private HttpResponseMessage UnauthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.WwwAuthenticate.ParseAdd("Basic realm=\"MyApp\"");
            return response;
        }
    }

}