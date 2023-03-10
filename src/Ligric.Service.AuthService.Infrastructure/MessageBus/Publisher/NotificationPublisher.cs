using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Contracts.Services;
using Ligric.Service.AuthService.Application.Contracts.Services;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Domain.Events.DataTypes;
using Ligric.Service.AuthService.Domain.Model.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ligric.Service.AuthService.Infrastructure.MessageBus.Publisher
{
	public class NotificationPublisher : INotificationBusPublisher
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IUserInfoPublisher _userInfoPublisher;
		private readonly ILogger<NotificationPublisher> _logger;
		private int _lockedScopes;

		private static readonly object LockObject = new();

		public NotificationPublisher(IServiceProvider serviceProvider, IConfiguration configuration,
			ILogger<NotificationPublisher> logger, IUserInfoPublisher userInfoPublisher)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
			_userInfoPublisher = userInfoPublisher;
		}

		public void StartPublish()
		{

			// Don't wait .
			Task.Run(PublishNonPublishedNotification);
		}

		private void PublishNonPublishedNotification()
		{
			_logger.LogInformation("Publishing to service bus requested.");

			if (_lockedScopes > 2)
				return;

			lock (LockObject)
			{
				_lockedScopes++;

				try
				{
					_logger.LogInformation("Publishing to service bus started.");

					using var scope = _serviceProvider.CreateScope();

					var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

					var messages = unitOfWork.OutboxMessageRepository.GetAll();

					_logger.LogInformation("Fetched Message From outbox {Count}", messages);

					PublishAndRemoveMessagesAsync(messages, unitOfWork).GetAwaiter().GetResult();
				}
				catch (Exception e)
				{
					_logger.LogCritical(e, "Notification Publisher error.");
				}
				finally
				{
					_lockedScopes--;
				}
			}
		}

		private async Task PublishAndRemoveMessagesAsync(IEnumerable<OutboxMessage> messages, IUnitOfWork unitOfWork)
		{
			foreach (var message in messages)
			{
				long id = message.Id ?? throw new ArgumentNullException($"Message Id is null");

				await SendMessageAsync(message);

				unitOfWork.OutboxMessageRepository.Delete(id);

				await unitOfWork.SaveChangesAsync();
			}

			await Task.CompletedTask;
		}


		private async Task SendMessageAsync(OutboxMessage message)
		{
			UserEntity user = message?.User ?? throw new ArgumentNullException("Message user is null");
			string id = user.Id?.ToString() ?? throw new ArgumentNullException("Message user id is null");
			string userName = user.UserName ?? throw new ArgumentNullException("Message user name is null");

			await _userInfoPublisher.SendMessageAsync(new MessageBody<UserCreatedOrUpdatedData>()
			{
				Data = new UserCreatedOrUpdatedData
				{
					Id = id,
					UniqueName = userName,
					CreatedDate = user.CreateDate,
				},
				Type = "UserCreatedOrUpdated",
				Sequence = 1,
				Version = 1,
				AggregateId = id,
				DateTime = DateTime.UtcNow
			});
		}
	}
}
