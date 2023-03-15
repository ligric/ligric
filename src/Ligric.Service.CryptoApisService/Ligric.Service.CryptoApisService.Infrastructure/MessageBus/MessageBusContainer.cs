using Ligric.Service.CryptoApisService.Application.Services;
using Ligric.Service.CryptoApisService.Infrastructure.MessageBus.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Ligric.Service.CryptoApisService.Infrastructure.MessageBus
{
	public static class MessageBusContainer
    {
        public static IServiceCollection AddMessageBusRegistration(this IServiceCollection services,
            IConfiguration configuration)
        {
          services.AddScoped<IUserInfoPublisher, UserInfoProducer>();
          services.AddScoped<INotificationBusPublisher, NotificationPublisher>();
          services.AddSingleton(s =>
			new ConnectionFactory()
			{
				HostName = configuration["MessageBroker:HostName"]
					?? throw new System.ArgumentNullException("\"MessageBroker:HostName\" is null")
			});

            return services;
        }
    }
}
