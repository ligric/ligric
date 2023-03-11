using Microsoft.Extensions.DependencyInjection;
using System;
using Ligric.Application.Behaviors;
using Ligric.Application.Exceptions;
using Ligric.Application.Notifications;

namespace Ligric.Application.Services
{
    public static class ApplicationExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        public static void AddBuildingBlocksApplication(this IServiceCollection services, Type validatorAssemblyMarkerType)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddApplicationExceptionsHandler();
            services.AddApplicationBehaviors(validatorAssemblyMarkerType);
            services.AddDomainNotificationHandler();
        }
    }
}
