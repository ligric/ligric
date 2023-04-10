using System.Threading.Tasks;
using Ligric.Backend.Application.Configuration.Commands;

namespace Ligric.Backend.Application.Configuration.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}