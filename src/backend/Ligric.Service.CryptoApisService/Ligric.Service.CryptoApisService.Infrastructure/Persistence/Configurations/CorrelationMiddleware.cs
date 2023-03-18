using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ligric.Service.CryptoApisService.Infrastructure
{
    public class CorrelationMiddleware
    {
        public const string CorrelationHeaderKey = "CorrelationId";

        private readonly RequestDelegate _next;

        public CorrelationMiddleware(
            RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = Guid.NewGuid();

            if (context.Request != null)
            {
                context.Request.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
            }

            await this._next.Invoke(context);
        }
    }
}
