namespace LigricMvvmToolkit.Navigation
{
    public class PinInfo
    {
        public object Pin { get; }
        
        public object ViewModel { get; }

        public string PinKey { get; }

        public bool IsVisible { get; }

        public PinInfo(object pin, string pinKey, bool isVisible = false, object vm = null)
        {
            Pin = pin; PinKey = pinKey; IsVisible = isVisible; ViewModel = vm;
        }
    }
}
