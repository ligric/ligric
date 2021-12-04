using System;

namespace WinRTMultibinding.Foundation.Interfaces
{
    internal interface IOneWayToSourceMultibindingItem : IMultibindingItem
    {
        Type SourcePropertyType { get; }


        void OnTargetPropertyValueChanged(object newSourcePropertyValue);
    }
}