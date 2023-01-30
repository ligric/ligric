namespace CheburchayNavigation.Common.Notifications.EventArgs
{
    public class IndexedEventArgs : System.EventArgs
    {
        public int Index { get; }

        public IndexedEventArgs(int index)
        {
            Index= index;
        }
    }
}
