using Autofac;
using Ligric.Infrastructure.Processing;
using Ligric.Service.CryptoApisService.Infrastructure.Logging;
using Ligric.Service.CryptoApisService.Infrastructure.Persistence.Commands;
using MediatR;

namespace Ligric.Service.CryptoApisService.IoC
{
	public class ProcessingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<DomainEventsDispatcher>()
            //   .As<IDomainEventsDispatcher>()
            //   .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
                typeof(INotificationHandler<>));

            //builder.RegisterGenericDecorator(
            //    typeof(UnitOfWorkCommandHandlerDecorator<>),
            //    typeof(ICommandHandler<>));

            //builder.RegisterGenericDecorator(
            //    typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            //    typeof(ICommandHandler<,>));

            //builder.RegisterType<CommandsDispatcher>()
            //    .As<ICommandsDispatcher>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<CommandsScheduler>()
            //    .As<ICommandsScheduler>()
            //    .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerWithResultDecorator<,>),
                typeof(ICommandHandler<,>));
        }
    }
}
