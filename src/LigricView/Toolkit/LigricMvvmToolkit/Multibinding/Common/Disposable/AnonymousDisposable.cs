using System;

namespace LigricMvvmToolkit.Multibinding.Common.Disposable
{
    internal class AnonymousDisposable : IDisposable
    {
        private readonly Action _dispose;


        public AnonymousDisposable(Action dispose)
        {
            _dispose = dispose;
        }


        public void Dispose()
        {
            _dispose();
        }
    }
}