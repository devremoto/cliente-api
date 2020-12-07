using Api.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class RequestMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestContext = new RequestContext();

            if (context.Request.Headers.TryGetValue("Id", out var providerIdStr) &&
                Guid.TryParse(providerIdStr, out var providerId))
            {
                requestContext.UserId = providerId;
            }

            context.Features.Set(requestContext);
            await next(context);
        }
    }
}
