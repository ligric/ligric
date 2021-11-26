using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricBoardCustomControls.Menus
{
    public partial class MenuStandard : Control
    {
        public string Trader { get => (string)GetValue(TraderProperty); set => SetValue(TraderProperty, value); }
        public static readonly DependencyProperty TraderProperty = DependencyProperty.Register(nameof(Trader), typeof(string), typeof(MenuStandard), new PropertyMetadata(string.Empty));


        public string PaymentMethod { get => (string)GetValue(PaymentMethodProperty); set => SetValue(PaymentMethodProperty, value); }
        public static readonly DependencyProperty PaymentMethodProperty = DependencyProperty.Register(nameof(PaymentMethod), typeof(string), typeof(MenuStandard), new PropertyMetadata(string.Empty));


        public string Rate { get => (string)GetValue(RateProperty); set => SetValue(RateProperty, value); }
        public static readonly DependencyProperty RateProperty = DependencyProperty.Register(nameof(Rate), typeof(string), typeof(MenuStandard), new PropertyMetadata(string.Empty));


        public string Limit { get => (string)GetValue(LimitProperty); set => SetValue(LimitProperty, value); }
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(nameof(Limit), typeof(string), typeof(MenuStandard), new PropertyMetadata(string.Empty));


        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MenuStandard), new PropertyMetadata(null));
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }


        public MenuStandard() => this.DefaultStyleKey = typeof(MenuStandard);
    }
}
