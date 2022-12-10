using System.Threading.Tasks;
using Ligric.Application.Configuration.Commands;

namespace Ligric.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}