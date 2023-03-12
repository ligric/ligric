using Autofac;
using Ligric.Service.AuthService.Infrastructure.Logging;
using Ligric.Service.AuthService.Infrastructure.Persistence.Commands;
using Ligric.Service.AuthService.Infrastructure.Processing;
using MediatR;

namespace Ligric.Infrastructure.Processing
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
