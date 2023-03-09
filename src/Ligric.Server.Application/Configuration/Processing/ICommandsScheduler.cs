using System.Threading.Tasks;
using Ligric.Server.Application.Configuration.Commands;

namespace Ligric.Server.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}