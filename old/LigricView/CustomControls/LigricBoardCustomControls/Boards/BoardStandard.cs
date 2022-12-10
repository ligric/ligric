using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LigricBoardCustomControls.Boards
{
    public partial class BoardStandard : ContentControl
    {
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BoardStandard), new PropertyMetadata(string.Empty));

        public object FiltersContent
        {
            get { return GetValue(FiltersContentProperty); }
            set { SetValue(FiltersContentProperty, value); }
        }
        public static DependencyProperty FiltersContentProperty
        {
            get;
        } = DependencyProperty.Register(nameof(FiltersContent), typeof(object), typeof(BoardStandard), new PropertyMetadata(null, null));


        public object BoardOptionsContent
        {
            get { return GetValue(BoardOptionsContentProperty); }
            set { SetValue(BoardOptionsContentProperty, value); }
        }
        public static DependencyProperty BoardOptionsContentProperty
        {
            get;
        } = DependencyProperty.Register(nameof(BoardOptionsContent), typeof(object), typeof(BoardStandard), new PropertyMetadata(null, null));

        public BoardStandard() => this.DefaultStyleKey = typeof(BoardStandard);
    }
}
