using System;

namespace LigricMvvmToolkit.Multibinding.Foundation.Interfaces
{
    internal interface IOneWayToSourceMultibindingItem : IMultibindingItem
    {
        Type SourcePropertyType { get; }


        void OnTargetPropertyValueChanged(object newSourcePropertyValue);
    }
}