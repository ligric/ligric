using System;
using Windows.UI.Xaml;
using WinRTMultibinding.Common.Disposable;

namespace WinRTMultibinding.Foundation
{
    internal class DisableablePropertyChangedCallback
    {
        private static readonly PropertyChangedCallback DisabledPropertyChangedCallback;


        private readonly PropertyChangedCallback _propertyChangedCallback;
        private PropertyChangedCallback _currentPropertyChangedCallback;


        static DisableablePropertyChangedCallback()
        {
            DisabledPropertyChangedCallback = (d, e) => { };
        }

        public DisableablePropertyChangedCallback(PropertyChangedCallback propertyChangedCallback)
        {
            _propertyChangedCallback = propertyChangedCallback;
            _currentPropertyChangedCallback = _propertyChangedCallback;
        }


        public void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _currentPropertyChangedCallback(d, e);
        }

        public IDisposable Disable()
        {
            _currentPropertyChangedCallback = DisabledPropertyChangedCallback;

            return new AnonymousDisposable(() => _currentPropertyChangedCallback = _propertyChangedCallback);
        }
    }
}