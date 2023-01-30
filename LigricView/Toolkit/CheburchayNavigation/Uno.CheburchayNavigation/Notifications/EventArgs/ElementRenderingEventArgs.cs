namespace Uno.CheburchayNavigation.Notifications.EventArgs
{
    public class ElementRenderingEventArgs : System.EventArgs
    {
        public string ElementKey { get; }
        public RenderingAction Action { get; }
        
        public ElementRenderingEventArgs(string elementKey, RenderingAction action)
        {
            ElementKey = elementKey;
            Action = action;
        }
    }
}
