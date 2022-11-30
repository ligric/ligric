using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Ligric.Application;
using Ligric.Application.Configuration.Commands;
using Ligric.Application.Configuration.Data;
using System.Reflection;
using Ligric.Infrastructure.Processing;

namespace Ligric.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInternalCommandsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[Command].[Type], " +
                               "[Command].[Data] " +
                               "FROM [app].[InternalCommands] AS [Command] " +
                               "WHERE [Command].[ProcessedDate] IS NULL";
            var commands = await connection.QueryAsync<InternalCommandDto>(sql);

            var internalCommandsList = commands.AsList();

            foreach (var internalCommand in internalCommandsList)
            {
                Type type = Assemblies.Application.GetType(internalCommand.Type);
                dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                await CommandsExecutor.Execute(commandToProcess);
            }


            return Unit.Value;
        }

        private class InternalCommandDto
        {
            public string Type { get; set; }

            public string Data { get; set; }
        }
    }
}