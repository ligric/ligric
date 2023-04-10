using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Ligric.Backend.Application;
using Ligric.Backend.Application.Configuration.Commands;
using Ligric.Backend.Application.Configuration.Data;
using Ligric.Backend.Application.Configuration.Processing;

namespace Ligric.Backend.Infrastructure.Processing
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
