using Utils;
using Action = Ligric.Protobuf.Action;

namespace Ligric.Service.CryptoApisService.Api.Helpers
{
    public static class ActionHelper
    {
        public static Action ToProtosAction(this EventAction action)
        {
            switch (action)
            {
                case EventAction.Added:
                    return Action.Added;
                case EventAction.Removed:
                    return Action.Removed;
                case EventAction.Changed:
                    return Action.Changed;
				case EventAction.Cleared:
					return Action.Cleared;
            }
            throw new NotImplementedException();
        }

		public static Action ToProtosAction(this NotifyDictionaryChangedAction action)
        {
            switch (action)
            {
                case NotifyDictionaryChangedAction.Added:
                    return Action.Added;
                case NotifyDictionaryChangedAction.Removed:
                    return Action.Removed;
                case NotifyDictionaryChangedAction.Changed:
                    return Action.Changed;
				case NotifyDictionaryChangedAction.Cleared:
					return Action.Cleared;
            }
            throw new NotImplementedException();
        }

        public static EventAction ToEventAction(this Action action)
        {
            switch (action)
            {
                case Action.Added:
                    return EventAction.Added;
                case Action.Removed:
                    return EventAction.Removed;
                case Action.Changed:
                    return EventAction.Changed;
				case Action.Cleared:
					return EventAction.Cleared;
            }
            throw new NotImplementedException();
        }
    }
}
