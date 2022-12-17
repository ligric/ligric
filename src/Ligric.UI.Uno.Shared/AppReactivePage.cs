using ReactiveUI;

namespace Ligric.UI.Uno
{
    /// <summary>
    /// Reactive page base class
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract partial class AppReactivePage<TViewModel> : ReactiveUI.Uno.ReactivePage<TViewModel>
          where TViewModel : class
    {
        protected AppReactivePage()
        {
            AttachHandlers();
        }

        void AttachHandlers()
        {
            DataContextChanged += (s, e) => OnDataContextChanged(e.NewValue);
            RegisterPropertyChangedCallback(ViewModelProperty, (s, p) => OnViewModelChanged());

            this.WhenActivated(_ => { });
        }

        protected virtual void OnDataContextChanged(object newValue)
        {
            if (ViewModel != newValue)
                ViewModel = newValue as TViewModel;
        }

        protected virtual void OnViewModelChanged()
        {
            if (DataContext != ViewModel)
                DataContext = ViewModel;
        }
    }
}
