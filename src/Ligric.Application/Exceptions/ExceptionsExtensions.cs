﻿using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Ligric.Application.Exceptions
{
    public static class ExceptionsExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        public static void AddApplicationExceptionsHandler(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestExceptionHandler<,,>), typeof(ApplicationExceptionsHandler<,,>));
        }
    }
}
