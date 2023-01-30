using Uno.CheburchayNavigation.Notifications.EventArgs;

namespace Uno.CheburchayNavigation.Notifications
{
    public enum RenderingAction
    {
        Rendering,
        Rendered
    }

    public delegate void ElementRenderingHandler(ElementRenderingEventArgs eventArgs);
}
