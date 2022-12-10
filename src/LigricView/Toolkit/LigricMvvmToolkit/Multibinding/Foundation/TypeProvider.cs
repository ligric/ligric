using System;
using LigricMvvmToolkit.Multibinding.Foundation.Interfaces;

namespace LigricMvvmToolkit.Multibinding.Foundation
{
    public abstract class TypeProvider<T> : ITypeProvider
    {
        private Type _targetType;


        Type ITypeProvider.GetType()
            => _targetType ?? (_targetType = typeof (T));
    }
}