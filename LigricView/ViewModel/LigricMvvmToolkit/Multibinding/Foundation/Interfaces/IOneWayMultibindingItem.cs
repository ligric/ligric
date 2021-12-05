using System;

namespace LigricMvvmToolkit.Multibinding.Foundation.Interfaces
{
    internal interface IOneWayMultibindingItem : IMultibindingItem
    {
        object SourcePropertyValue { get; }


        event EventHandler SourcePropertyValueChanged;
    }
}