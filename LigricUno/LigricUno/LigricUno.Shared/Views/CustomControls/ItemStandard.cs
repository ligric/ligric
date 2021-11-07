using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.CustomControls
{
    public sealed class ItemStandard : Control
    {
        public string Trader { get => (string)GetValue(TraderProperty); set => SetValue(TraderProperty, value); }
        public static readonly DependencyProperty TraderProperty = DependencyProperty.Register(nameof(Trader), typeof(string), typeof(ItemStandard), new PropertyMetadata(string.Empty));


        public string PaymentMethod { get => (string)GetValue(PaymentMethodProperty); set => SetValue(PaymentMethodProperty, value); }
        public static readonly DependencyProperty PaymentMethodProperty = DependencyProperty.Register(nameof(PaymentMethod), typeof(string), typeof(ItemStandard), new PropertyMetadata(string.Empty));


        public string Rate { get => (string)GetValue(RateProperty); set => SetValue(RateProperty, value); }
        public static readonly DependencyProperty RateProperty = DependencyProperty.Register(nameof(Rate), typeof(string), typeof(ItemStandard), new PropertyMetadata(string.Empty));


        public string Limit { get => (string)GetValue(LimitProperty); set => SetValue(LimitProperty, value); }
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(nameof(Limit), typeof(string), typeof(ItemStandard), new PropertyMetadata(string.Empty));
        

        public ItemStandard() => this.DefaultStyleKey = typeof(ItemStandard);
    }
}
