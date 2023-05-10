﻿using System.Reactive.Linq;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.UI.Helpers;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Presentation;
using Microsoft.UI;

namespace Ligric.UI.Views
{
	public partial class FuturesPage : Page
    {
		private static readonly SolidColorBrush
			RED_COLOR = ToBrush("#FF5C5C"),
			GREEN_COLOR = ToBrush("#5CFF94"),
			SIMPLE_COLOR = new SolidColorBrush(Colors.White);

		public FuturesPage()
        {
            this.InitializeComponent();
			DataContextChanged += OnDataContextChanged;
        }

		public FuturesViewModel? ViewModel { get; private set; }

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
			if (ViewModel != null && e.OriginalSource is CheckBox apiCheckBox && apiCheckBox.DataContext is ApiClientDto api)
			{
				ViewModel.Api.AttachApiStreamsCommand.Execute(api);
			}
		}

		private void OnApiCheckBoxUnchecked(object sender, RoutedEventArgs e)
		{

		}

		public static string GetOrderSideFromOrderViewModel(OrderViewModel orderVm)
		{
			const string closeShort = "Close Short", closeLong = "Close Long",
						 openShort = "Open Short", openLong = "Open Long";

			bool isSell = orderVm.Side is "Sell";
			
			return orderVm.Type switch
			{
				"TakeProfitMarket" => isSell ? closeShort : closeLong,
				"StopMarket" => isSell ? closeShort : closeLong,
				"Limit" => isSell ? openShort : openLong,
				_ => "-"
			};
		}


		public static Brush SideRectangleBrushFromOrderViewModel(OrderViewModel orderVm)
			=> orderVm.Side is "Sell" ? RED_COLOR : GREEN_COLOR;

		public static Brush SideTextBlockForegroundFromOrderViewModel(OrderViewModel orderVm)
		{
			bool isSell = orderVm.Side is "Sell";

			return orderVm.Type switch
			{
				"TakeProfitMarket" => isSell ? GREEN_COLOR : RED_COLOR,
				"StopMarket" => isSell ? GREEN_COLOR : RED_COLOR,
				"Limit" => isSell ? GREEN_COLOR : RED_COLOR,
				_ => SIMPLE_COLOR
			};
		}

		private static SolidColorBrush ToBrush(string hex)
		{
			hex = hex.Replace("#", string.Empty);
			byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
			byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
			byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
			return new SolidColorBrush(Windows.UI.Color.FromArgb(255, r, g, b));
		}
	}
}
