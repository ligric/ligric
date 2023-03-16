using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Serilog;
using NHibernate;
using Ligric.Service.AuthService.Application;
using Ligric.Service.AuthService.Infrastructure.Persistence.Repositories;
using Ligric.Service.AuthService.Infrastructure.Logging;
using Ligric.Service.AuthService.Infrastructure.Processing;
using Ligric.Infrastructure.Domain;
using Ligric.Infrastructure.Processing;
using CommonServiceLocator;
using Autofac.Extras.CommonServiceLocator;
using Ligric.Service.AuthService.Infrastructure.Quartz;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Infrastructure.Jwt;
using Ligric.Service.AuthService.Infrastructure.Nhibernate.Database;

namespace Ligric.Service.AuthService.Infrastructure
{
	public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            string connectionString,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor,
            bool runQuartz = true)
        {
            if (runQuartz)
            {
                StartQuartz(connectionString, logger, executionContextAccessor);
            }

            var serviceProvider = CreateAutofacServiceProvider(
                services,
                connectionString,
                logger,
                executionContextAccessor);

            return serviceProvider;
        }

        private static IServiceProvider CreateAutofacServiceProvider(
            IServiceCollection services,
            string connectionString,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor)
        {
			var container = new ContainerBuilder();

            container.Populate(services);

            container.RegisterType<DefaultCryptoProvider>().As<ICryptoProvider>().InstancePerLifetimeScope();
            container.RegisterType<JwtAuthManager>().As<IJwtAuthManager>().InstancePerLifetimeScope();

			container.RegisterType<QbDataInterceptor>().As<IInterceptor>().SingleInstance();
            container.RegisterType<ConnectionSettingsProvider>().As<IConnectionSettingsProvider>().SingleInstance();
            container.RegisterType<NhInitFactory>().SingleInstance();
            container.RegisterType<CoreDataProvider>().As<DataProvider>().InstancePerLifetimeScope();

			// # REPOSITORIES
			container.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

			// # OBSERVERS
			//container.RegisterType<UserApiObserver>().As<IUserApiObserver>().SingleInstance();
			//container.RegisterType<TemporaryUserFuturesObserver>().As<ITemporaryUserFuturesObserver>().SingleInstance();

			container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DomainModule());
            container.RegisterModule(new ProcessingModule());

			container.RegisterInstance(executionContextAccessor);

            var buildContainer = container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(buildContainer));

            var serviceProvider = new AutofacServiceProvider(buildContainer);

            CompositionRoot.SetContainer(buildContainer);

            return serviceProvider;
        }

        private static void StartQuartz(
            string connectionString,
            ILogger logger,
            IExecutionContextAccessor executionContextAccessor)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            var container = new ContainerBuilder();

            container.RegisterModule(new LoggingModule(logger));
            container.RegisterModule(new QuartzModule());
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new DataAccessModule(connectionString));
            container.RegisterModule(new ProcessingModule());

            container.RegisterInstance(executionContextAccessor);
            //container.Register(c =>
            //{
            //    var dbContextOptionsBuilder = new DbContextOptionsBuilder<DevPaceContext>();
            //    dbContextOptionsBuilder.UseSqlServer(connectionString);

            //    dbContextOptionsBuilder
            //        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

            //    return new DevPaceContext(dbContextOptionsBuilder.Options);
            //}).AsSelf().InstancePerLifetimeScope();

            scheduler.JobFactory = new JobFactory(container.Build());

            scheduler.Start().GetAwaiter().GetResult();

            //var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            //var trigger =
            //    TriggerBuilder
            //        .Create()
            //        .StartNow()
            //        .WithCronSchedule("0/15 * * ? * *")
            //        .Build();

            //scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

            //var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
            //var triggerCommandsProcessing =
            //    TriggerBuilder
            //        .Create()
            //        .StartNow()
            //        .WithCronSchedule("0/15 * * ? * *")
            //        .Build();
            //scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();
        }
    }
}
