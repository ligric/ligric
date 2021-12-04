using System;

namespace WinRTMultibinding.Common.Extensions
{
    internal static class EventsExtensions
    {
        public static void RaiseEvent(this EventHandler handler, object sender)
            => handler?.Invoke(sender, EventArgs.Empty);

        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, T e) where T : EventArgs
            => handler?.Invoke(sender, e);
    }
}