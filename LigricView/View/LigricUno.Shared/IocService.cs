﻿using BoardsShared.Abstractions.BoardsAbstractions.Interfaces;
using BoardsShared.BoardsCore;
using Microsoft.Extensions.DependencyInjection;

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
