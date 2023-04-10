using Autofac;

namespace Ligric.Service.CryptoApisService.IoC
{
    public static class CompositionRoot
    {
        private static IContainer? _container;

        public static void SetContainer(IContainer container)
        {
            _container = container;
        }

        internal static ILifetimeScope? BeginLifetimeScope()
        {
            return _container?.BeginLifetimeScope();
        }
    }
}
