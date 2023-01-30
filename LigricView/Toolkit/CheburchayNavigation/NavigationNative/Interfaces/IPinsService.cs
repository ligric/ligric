using CheburchayNavigation.Native.Enums;
using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Notifications;
using System;
using System.Collections.Generic;

namespace CheburchayNavigation.Native.Interfaces
{
    public enum PinsAction
    {
        Pin,
        Unpin
    }

    public delegate void PinsExistenceChangedHandler(object sender, PinInfo pinInfo, PinsAction action);
    public delegate void PinVisibilityChangedHandler(object sender, PinInfo pinInfo, SwitchState newState);


    public interface IPinsService
    {
        IReadOnlyDictionary<string, PinInfo> Pins { get; }

        event PinVisibilityChangedHandler PinVisibilityChanged;

        event PinsExistenceChangedHandler PinsExistenceChanged;

        event ElementsDirectionChangedHandler<PinInfo> PinsDirectionChanged;

        void Pin(object pinElement, string pinKey, IEnumerable<string> forbiddenPageKeys = null, object viewModel = null, SwitchState defaultState = SwitchState.Visible);

        void Unpin(string pinKey);

        void TurnOffPin(string key);

        void TurnOnPin(string key);
    }
}
