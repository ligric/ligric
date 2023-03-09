using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Ligric.Server.Application;
using Ligric.Server.Application.Configuration.Commands;
using Ligric.Server.Application.Configuration.Data;
using Ligric.Server.Application.Configuration.Processing;

namespace Ligric.Server.Infrastructure.Processing
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {

			throw new NotImplementedException();
        }
    }
}
