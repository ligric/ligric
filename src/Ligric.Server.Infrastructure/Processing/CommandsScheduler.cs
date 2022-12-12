using System;
using System.Threading.Tasks;
using Dapper;
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
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [app].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command)
            });
        }
    }
}