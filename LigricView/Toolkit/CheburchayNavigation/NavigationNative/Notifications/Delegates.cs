using CheburchayNavigation.Native.Notifications.EventArgs;

namespace CheburchayNavigation.Native.Notifications
{
    public delegate void ElementDirectionChangedHandler<T>(object sender, ElementDirectionChangedEventArgs<T> eventArgs);

    public delegate void ElementsDirectionChangedHandler<T1, T2>(object sender, ElementDirectionChangedEventArgs<T1> eventArgs, ElementsDirectionChangedEventArgs<T2> eventArgs1);

    public delegate void ElementsDirectionChangedHandler<T>(object sender, ElementsDirectionChangedEventArgs<T> eventArgs);
}
