using Common.Enums;

namespace Common.Delegates
{
    public delegate void ActionStateHandler(object sender, StateEnum state);

    public delegate void CollectionEventHandler<T>(object sender, ActionCollectionEnum action, T item);
}
