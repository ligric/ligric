using System;

namespace WinRTMultibinding.Foundation.Interfaces
{
    internal interface IOneWayMultibindingItem : IMultibindingItem
    {
        object SourcePropertyValue { get; }


        event EventHandler SourcePropertyValueChanged;
    }
}