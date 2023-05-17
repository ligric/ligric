﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
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

		private readonly ObservableCollection<ApiClientViewModel> _selectedApis = new ObservableCollection<ApiClientViewModel>();

		public FuturesPage()
		{
			this.InitializeComponent();
			DataContextChanged += OnDataContextChanged;
			((INotifyPropertyChanged)_selectedApis).PropertyChanged += OnSelectedApisPropertyChanged;
		}

		public FuturesViewModel? ViewModel { get; private set; }

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
				"Limit" => isSell ? RED_COLOR : GREEN_COLOR,
				_ => SIMPLE_COLOR
			};
		}

		private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			ViewModel = args.NewValue as FuturesViewModel;
			if (ViewModel != null)
			{
				ViewModel.Api.Apis.CollectionChanged -= OnCollectionChanged;
				ViewModel.Api.Apis.CollectionChanged += OnCollectionChanged;
			}
		}

		private async void ShareClick(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null && e.OriginalSource is FrameworkElement ui && ui.DataContext is ApiClientViewModel api)
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
			if (e.OriginalSource is CheckBox apiCHeckBox && apiCHeckBox.DataContext is ApiClientViewModel api)
			{
				_selectedApis.Add(api);
			}
		}

		private void OnApiCheckBoxUnchecked(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is CheckBox apiCheckBox && apiCheckBox.DataContext is ApiClientViewModel api)
			{
				_selectedApis.Remove(api);
			}
		}

		private void OnApiToggleButtonChecked(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null && e.OriginalSource is ToggleButton apiToggleButton && apiToggleButton.DataContext is ApiClientViewModel api)
			{
				ViewModel.Api.AttachApiStreamsCommand.Execute(api);
			}
		}

		private void OnApiToggleButtonUnchecked(object sender, RoutedEventArgs e)
		{
			if (ViewModel != null && e.OriginalSource is ToggleButton apiToggleButton && apiToggleButton.DataContext is ApiClientViewModel api)
			{
				ViewModel.Api.DetachApiStreamsCommand.Execute(api);
			}
		}

		private void OnApiHeaderStopButtonClicked(object sender, RoutedEventArgs e)
		{
			List<ToggleButton> apiToggleButtons = new List<ToggleButton>();
			VisualTreeHelpers.FindChildren(apiToggleButtons, ApisItemsRepeater, "ApiToggleButton");

			foreach (var selectedApi in _selectedApis)
			{
				var apiToggleButton = apiToggleButtons.First(x => x.DataContext == selectedApi);
				apiToggleButton.IsChecked = false;
			}
		}

		private void OnApiHeaderStartButtonClicked(object sender, RoutedEventArgs e)
		{
			List<ToggleButton> apiToggleButtons = new List<ToggleButton>();
			VisualTreeHelpers.FindChildren(apiToggleButtons, ApisItemsRepeater, "ApiToggleButton");

			foreach (var selectedApi in _selectedApis)
			{
				var apiToggleButton = apiToggleButtons.First(x => x.DataContext == selectedApi);
				apiToggleButton.IsChecked = true;
			}
		}

		private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
					foreach (var oldItem in e.OldItems!)
					{
						var oldItemApi = oldItem as ApiClientViewModel;
						if (_selectedApis.FirstOrDefault(x => x == oldItemApi) != null)
						{
							_selectedApis.Remove(oldItemApi!);
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					_selectedApis.Clear();
					break;
			}
		}

		private void OnSelectedApisPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Count")
			{
				if (_selectedApis.Count == 0)
				{
					ApisCheckBox.IsChecked = false;
					ApisStartButton.IsEnabled = false;
					ApisStopButton.IsEnabled = false;
				}
				else
				{
					ApisStartButton.IsEnabled = true;
					ApisStopButton.IsEnabled = true;
				}
			}
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
