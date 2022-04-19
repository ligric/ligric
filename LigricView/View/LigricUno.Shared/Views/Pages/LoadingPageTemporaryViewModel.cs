using LigricMvvmToolkit.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LigricUno.Views.Pages
{
    internal class LoadingPageTemporaryViewModel : DispatchedBindableBase
    {
        private double _value;
        public double Value { get => _value; set => SetProperty(ref _value, value); }

        public LoadingPageTemporaryViewModel()
        {
            Navigation.PageRendering += OnPageRendering;
        }

        private int renderingPagesCount;

        private readonly object testLock = new object();
        private void OnPageRendering(Uno.CheburchayNavigation.Notifications.EventArgs.ElementRenderingEventArgs eventArgs)
        {
            renderingPagesCount++;

            double part = 100 / 48.00 * (renderingPagesCount /* login page */);
            Value = part;
            //Debug.WriteLine($"{(int)part} % _ \t{renderingPagesCount} _ \t{DateTime.Now}");
        }
    }
}
