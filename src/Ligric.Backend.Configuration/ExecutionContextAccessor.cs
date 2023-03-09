﻿using System;
using System.Linq;
using Ligric.Backend.Application.Configuration;
using Microsoft.AspNetCore.Http;

namespace Ligric.Backend.Configuration
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CorrelationId
        {
            get
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				if (IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
				{
#pragma warning disable CS8604 // Possible null reference argument.
					return Guid.Parse(
						_httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
#pragma warning restore CS8604 // Possible null reference argument.
				}
#pragma warning restore CS8602 // Dereference of a possibly null reference.

				throw new ApplicationException("Http context and correlation id is not available");

			}
		}

        public bool IsAvailable => _httpContextAccessor?.HttpContext != null;
    }
}
