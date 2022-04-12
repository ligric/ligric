using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LigricMvvmToolkit.Navigation
{
    public class PinInfo
    {
        public object Element { get; }

        public string PinKey { get; }

        public bool IsVisible { get; }

        public IReadOnlyCollection<string> ForbiddenPageKeys { get; }
        
        public object ViewModel { get; }

        public string WrapperKey { get; }

        private static readonly ReadOnlyCollection<string> empty = new ReadOnlyCollection<string>(new string[0]);

        public PinInfo(object element, string pinKey, bool isVisible = false, object vm = null, IEnumerable<string> forbiddenPageKeys = null)
        {
            Element = element;
            PinKey = pinKey;
            IsVisible = isVisible;
            ViewModel = vm;
            ForbiddenPageKeys = forbiddenPageKeys == null
                                ? empty
                                : new ReadOnlyCollection<string>(forbiddenPageKeys.ToArray());
        }
    }
}
