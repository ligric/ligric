﻿using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Ligric.Application.Notifications
{
    public static class NotificationsExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        public static void AddDomainNotificationHandler(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler, NotificationHandler>();
        }
    }
}
