using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricBoardCustomControls.Items
{
    public partial class ItemStandard : Control
    {
        public IDictionary<string, string> Properties { get => (IDictionary<string, string>)GetValue(PropertiesProperty); set => SetValue(PropertiesProperty, value); }
        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(nameof(Properties), typeof(IDictionary<string, string>), typeof(ItemStandard), new PropertyMetadata(new Dictionary<string, string>(), OnPropertiesChanged));
        
        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dict = (IDictionary<string, string>)e.NewValue;

            if (dict.TryGetValue("Trader", out string trader))
                ((ItemStandard)d).Trader = trader;
            if (dict.TryGetValue("PaymentMethod", out string paymentMethod))
                ((ItemStandard)d).PaymentMethod = paymentMethod;
            if (dict.TryGetValue("Rate", out string rate))
                ((ItemStandard)d).Rate = rate;
            if (dict.TryGetValue("Limit", out string limit))
                ((ItemStandard)d).Limit = limit;
        }

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
