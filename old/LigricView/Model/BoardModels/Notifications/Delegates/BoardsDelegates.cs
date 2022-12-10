namespace BoardsCore.Notifications.Delegates
{
    public delegate void ElementChangedHandler<T>(object sender, T oldElement, T newElement);
}
