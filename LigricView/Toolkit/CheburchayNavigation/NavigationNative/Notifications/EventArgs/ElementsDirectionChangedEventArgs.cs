using CheburchayNavigation.Native.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CheburchayNavigation.Native.Notifications.EventArgs
{
    public class ElementsDirectionChangedEventArgs<T>
    {
        public ElementDirection Direction { get; }

        public IReadOnlyCollection<T> NewElements { get; }

        public IReadOnlyCollection<T> OldElements { get; }

        private static readonly ReadOnlyCollection<T> empty = new ReadOnlyCollection<T>(new T[0]);

        public ElementsDirectionChangedEventArgs(ElementDirection direction, IEnumerable<T> newElements, IEnumerable<T> oldElements)
        {
            Direction = direction;
            NewElements = newElements == null
                          ? empty
                          : new ReadOnlyCollection<T>(newElements.ToArray());
            OldElements = oldElements == null
                          ? empty
                          : new ReadOnlyCollection<T>(oldElements.ToArray());
        }
    }
}
