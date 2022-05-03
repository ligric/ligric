using BoardsCore.Abstractions.BoardsAbstractions.Interfaces;
using BoardsCore;
using Microsoft.Extensions.DependencyInjection;
using BoardsCore.Boards;

namespace LigricUno
{
    public static class IocService
    {
        private static ServiceProvider _serviceProvider;

        public static ServiceProvider ServiceProvider => _serviceProvider;

        public static void Initialize()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IBoardsService, BoardsService>()
                .BuildServiceProvider();
        }
    }
}
