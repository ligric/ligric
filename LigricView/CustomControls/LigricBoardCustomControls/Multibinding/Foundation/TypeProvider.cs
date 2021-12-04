using System;
using WinRTMultibinding.Foundation.Interfaces;

namespace WinRTMultibinding.Foundation
{
    public abstract class TypeProvider<T> : ITypeProvider
    {
        private Type _targetType;


        Type ITypeProvider.GetType()
            => _targetType ?? (_targetType = typeof (T));
    }
}