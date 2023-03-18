using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Ligric.Application;
using Ligric.Application.Configuration.Commands;
using Ligric.Application.Configuration.Data;
using Ligric.Application.Configuration.Processing;

namespace Ligric.Infrastructure.Processing
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
