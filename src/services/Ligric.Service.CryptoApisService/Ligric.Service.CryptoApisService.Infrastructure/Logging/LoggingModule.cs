﻿using Autofac;
using Serilog;

namespace Ligric.Service.CryptoApisService.Infrastructure.Logging
{
	public class LoggingModule : Autofac.Module
    {
        private readonly ILogger _logger;

		public LoggingModule(ILogger logger)
        {
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_logger)
                .As<ILogger>()
                .SingleInstance();
        }
    }
}
