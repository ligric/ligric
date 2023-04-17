﻿using Ligric.UI.Helpers;

namespace Ligric.UI.Views
{
    public partial class FuturesPage : Page
    {
        public FuturesPage()
        {
            this.InitializeComponent();
			DataContextChanged += OnDataContextChanged;
        }

		//public FuturesViewModel? ViewModel { get; set; }

		private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			//ViewModel = args.NewValue as FuturesViewModel;
		}

		private async void ShareClick(object sender, RoutedEventArgs e)
		{
			//if (ViewModel != null && e.OriginalSource is FrameworkElement ui && ui.DataContext is ApiClientDto api)
			//{
			//	await ViewModel.Api.ShareApiCommand.Execute(api);
			//} 
		}

		private void OnCheckAllChecked(object sender, RoutedEventArgs e)
		{
			List<CheckBox> checkBoxes = new List<CheckBox>();
			VisualTreeHelpers.FindChildren(checkBoxes, ApisItemsRepeater, "ApiCheckBox");

			foreach (CheckBox checkBox in checkBoxes)
			{
				if (checkBox.IsChecked == null || checkBox.IsChecked == false)
				{
					checkBox.IsChecked = true;
				}
			}
		}

		private void OnApiCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			//if (ViewModel != null && e.OriginalSource is CheckBox apiCheckBox && apiCheckBox.DataContext is ApiClientDto api)
			//{
			//	ViewModel.Api.AttachApiStreamsCommand.Execute(api);
			//}
		}

		private void OnApiCheckBoxUnchecked(object sender, RoutedEventArgs e)
		{

		}
	}
}
