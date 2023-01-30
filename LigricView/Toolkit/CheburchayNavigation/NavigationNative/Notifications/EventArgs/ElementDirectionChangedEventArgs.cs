using CheburchayNavigation.Native.Enums;

namespace CheburchayNavigation.Native.Notifications.EventArgs
{
    public class ElementDirectionChangedEventArgs<T>
    {
        public ElementDirection Direction { get; }

        public T NewElement { get; }

        public T OldElement { get; }

        public ElementDirectionChangedEventArgs(ElementDirection direction, T newElement, T oldElement)
        {
            Direction = direction;
            NewElement = newElement;
            OldElement = oldElement;
        }
    }
}
