using CheburchayNavigation.Native.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CheburchayNavigation.Native.InfoModels
{
    public sealed class PinInfo : IModelInfo
    {
        public object Element { get; }

        public string Key { get; }

        public SwitchState State { get; }

        public IReadOnlyCollection<string> ForbiddenPageKeys { get; }
        
        public object ViewModel { get; }

        private static readonly ReadOnlyCollection<string> empty = new ReadOnlyCollection<string>(new string[0]);

        public PinInfo(object element, string pinKey, SwitchState state = SwitchState.Collapsed, object vm = null, IEnumerable<string> forbiddenPageKeys = null)
        {
            Element = element;
            Key = pinKey;
            State = state;
            ViewModel = vm;
            ForbiddenPageKeys = forbiddenPageKeys == null
                                ? empty
                                : new ReadOnlyCollection<string>(forbiddenPageKeys.ToArray());
        }
    }
}
