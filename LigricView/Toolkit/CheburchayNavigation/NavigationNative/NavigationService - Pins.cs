using CheburchayNavigation.Native.Enums;
using CheburchayNavigation.Native.InfoModels;
using CheburchayNavigation.Native.Interfaces;
using CheburchayNavigation.Native.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheburchayNavigation.Native
{
    public partial class NavigationService : IPinsService
    {
        private readonly HashSet<string> collapsedPins = new HashSet<string>();

        public event PinsExistenceChangedHandler PinsExistenceChanged;

        public event PinVisibilityChangedHandler PinVisibilityChanged;

        public event ElementsDirectionChangedHandler<PinInfo> PinsDirectionChanged;

        public void Pin(object pinElement, string pinKey, IEnumerable<string> forbiddenPageKeys, object viewModel, SwitchState defaultState = SwitchState.Visible)
        {
            if (pinElement == null)
                throw new NullReferenceException("Pin element is null");
           
            if (string.IsNullOrEmpty(pinKey))
                throw new NullReferenceException($"Pin key " + pinKey + " is null or empty.");

            if (pins.TryGetValue(pinKey, out PinInfo outPage))
                throw new ArgumentException($"Pin key " + pinKey + " already existence.");

            bool hasAccess = false;

            if (defaultState == SwitchState.Visible)
            {
                hasAccess = CheckAccessForCurrentPage(forbiddenPageKeys);
            }
            else
            {
                collapsedPins.Add(pinKey);
            }

            var newPin = new PinInfo(pinElement, pinKey, hasAccess ? SwitchState.Visible : Enums.SwitchState.Collapsed, vm: viewModel, forbiddenPageKeys: forbiddenPageKeys);
            pins.Add(pinKey, newPin);

            PinsExistenceChanged?.Invoke(this, newPin, PinsAction.Pin);
        }

        public void Unpin(string pinKey)
        {
            throw new NotImplementedException();
        }

        public void TurnOffPin(string key)
        {
            if (!pins.TryGetValue(key, out PinInfo pinInfo))
                throw new NullReferenceException($"Pin key {key} is not found.");

            if (!collapsedPins.Contains(key))
                collapsedPins.Add(key);

            if (pinInfo.State == SwitchState.Collapsed)
                return;

            var newPin = new PinInfo(pinInfo.Element, pinInfo.Key, SwitchState.Collapsed, pinInfo.ViewModel, pinInfo.ForbiddenPageKeys);

            pins[key] = newPin;

            PinVisibilityChanged?.Invoke(this, newPin, SwitchState.Collapsed);
        }

        public void TurnOnPin(string key)
        {
            if (!pins.TryGetValue(key, out PinInfo pinInfo))
                throw new NullReferenceException($"Pin key {key} is not found.");

            if (collapsedPins.Contains(key))
                collapsedPins.Remove(key);

            if (pinInfo.State == SwitchState.Visible)
                return;

            var newPin = new PinInfo(pinInfo.Element, pinInfo.Key, SwitchState.Visible, pinInfo.ViewModel, pinInfo.ForbiddenPageKeys);

            pins[key] = newPin;

            PinVisibilityChanged?.Invoke(this, newPin, SwitchState.Visible);
        }

        private bool CheckAccessForCurrentPage(IEnumerable<string> forbiddenPageKeys)
        {
            var currentPageKey = CurrentPage.Key;

            foreach (var item in forbiddenPageKeys)
            {
                if (string.Equals(currentPageKey, item))
                {
                    return false;
                }
            }
            return true;
        }

        private (IList<PinInfo> OldPins, IList<PinInfo> NewPins) UpdatePinsState(ElementDirection direction)
        {
            var currentPageKey = CurrentPage.Key;

            var newPins = new List<PinInfo>();
            var oldPins = new List<PinInfo>();

            foreach (var item in pins)
            {
                PinInfo pin = item.Value;

                var isOldPin = false;
                foreach (var forbiddenPageKey in pin.ForbiddenPageKeys)
                {
                    if (string.Equals(forbiddenPageKey, currentPageKey))
                    {
                        if (!collapsedPins.Contains(pin.Key))
                        {
                            var newPin = new PinInfo(pin.Element, pin.Key, SwitchState.Collapsed, pin.ViewModel, pin.ForbiddenPageKeys);
                            oldPins.Add(newPin);
                        }
                        isOldPin = true;
                        break;
                    }
                }

                if (!isOldPin)
                {
                    if (pin.State == SwitchState.Collapsed)
                    {
                        if (!collapsedPins.Contains(pin.Key))
                        {
                            var newPin = new PinInfo(pin.Element, pin.Key, SwitchState.Visible, pin.ViewModel, pin.ForbiddenPageKeys);
                            newPins.Add(newPin);
                        }
                    }
                }
            }

            foreach (var oldPin in oldPins)
                pins[oldPin.Key] = oldPin;

            foreach (var newPin in newPins)
                pins[newPin.Key] = newPin;

            PinsDirectionChanged?.Invoke(this, 
                new Notifications.EventArgs.ElementsDirectionChangedEventArgs<PinInfo>(
                    direction, newPins, oldPins));

            return (oldPins, newPins);
        }
    }
}
