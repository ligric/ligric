//using System;
//using System.Reflection;
//using System.Threading.Tasks;
//using Ligric.Infrastructure.Database;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;

//namespace Ligric.Infrastructure.Processing.InternalCommands
//{
//    public class CommandsDispatcher : ICommandsDispatcher
//    {
//        private readonly IMediator _mediator;
//        private readonly DevPaceContext _devPaceContext;

//        public CommandsDispatcher(
//            IMediator mediator,
//            DevPaceContext devPaceContext)
//        {
//            this._mediator = mediator;
//            this._devPaceContext = devPaceContext;
//        }

//        public async Task DispatchCommandAsync(Guid id)
//        {
//            var internalCommand = await this._devPaceContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);

//            Type type = Assembly.GetAssembly(typeof(MarkCustomerAsWelcomedCommand)).GetType(internalCommand.Type);
//            dynamic command = JsonConvert.DeserializeObject(internalCommand.Data, type);

//            internalCommand.ProcessedDate = DateTime.UtcNow;

//            await this._mediator.Send(command);
//        }
//    }
//}