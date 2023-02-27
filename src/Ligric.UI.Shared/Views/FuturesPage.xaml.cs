using System.Reactive.Linq;
using Ligric.Domain.Types.Api;
using Ligric.UI.ViewModels.Presentation;

namespace Ligric.UI.Views
{
    public sealed partial class FuturesPage : Page
    {
        public FuturesPage()
        {
            this.InitializeComponent();
			DataContextChanged += OnDataContextChanged;
        }

		public FuturesViewModel? ViewModel { get; set; }

		private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			ViewModel = args.NewValue as FuturesViewModel;
		}

		private async void ShareClick(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null && e.OriginalSource is FrameworkElement ui && ui.DataContext is ApiClientDto api)
			{
				await ViewModel.Api.ShareApiCommand.Execute(api);
			}
		}
	}
}
