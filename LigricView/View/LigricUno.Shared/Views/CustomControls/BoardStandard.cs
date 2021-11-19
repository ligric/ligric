using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricUno.Views.CustomControls
{
    public partial class BoardStandard : Control
    {
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BoardStandard), new PropertyMetadata(string.Empty));

        public IList<IDictionary<string,string>> Ads { get => (IList<IDictionary<string, string>>)GetValue(AdsProperty); set => SetValue(AdsProperty, value); }
        public static readonly DependencyProperty AdsProperty = DependencyProperty.Register(nameof(Ads), typeof(IList<IDictionary<string, string>>), typeof(BoardStandard), new
            PropertyMetadata(new List<IDictionary<string, string>>()));

        public BoardStandard() => this.DefaultStyleKey = typeof(BoardStandard);
    }
}
