using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Api.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Trace.TraceError(context.Exception.ToString());

            HttpResponseMessage response;

            if (context.Exception is UnauthorizedAccessException)
            {
                response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized access.");
            }
            else if (context.Exception is KeyNotFoundException)
            {
                response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, context.Exception.Message);
            }
            else if (context.Exception is ArgumentException)
            {
                response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
            }
            else if (context.Exception is InvalidOperationException)
            {
                response = context.Request.CreateErrorResponse(HttpStatusCode.Conflict, context.Exception.Message);
            }
            else
            {
                response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception.Message);
            }

            context.Response = response;
        }
    }

}