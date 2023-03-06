﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Future;
using Ligric.Protos;
using Ligric.UI.Infrastructure;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using Utils;

namespace Ligric.UI.ViewModels.Presentation
{
	public class FutureOrdersViewModel
	{
		private readonly IValuesService _valuesService;
		private readonly IOrdersService _ordersService;

		internal FutureOrdersViewModel(
			IOrdersService ordersService,
			IValuesService valuesService)
		{
			_ordersService = ordersService;
			_valuesService = valuesService;

			Subscribtions();
		}

		public ObservableCollection<OrderViewModel> Orders { get; } = new ObservableCollection<OrderViewModel>();

		private void Subscribtions()
		{
			var ordersCollectionChanged = Observable.FromEvent<EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>, NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>>(handler =>
			{
				EventHandler<NotifyDictionaryChangedEventArgs<long, FuturesOrderDto>> collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _ordersService.OpenOrdersChanged += fsHandler, fsHandler => _ordersService.OpenOrdersChanged -= fsHandler);

			var valuesCollectionChanged = Observable.FromEvent<EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>>, NotifyDictionaryChangedEventArgs<string, decimal>>(handler =>
			{
				EventHandler<NotifyDictionaryChangedEventArgs<string, decimal>> collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _valuesService.ValuesChanged += fsHandler, fsHandler => _valuesService.ValuesChanged -= fsHandler);

			ordersCollectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnOpenOrdersChanged);
			valuesCollectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnValuesChanged);
		}

		private void OnOpenOrdersChanged(NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> obj)
		{
			UpdateOrdersFromAction(obj);
		}

		private void OnValuesChanged(NotifyDictionaryChangedEventArgs<string, decimal> obj)
		{
			UpdateOrdersFromAction(obj);
		}

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<long, FuturesOrderDto> obj)
		{
			switch (obj.Action)
			{
				case NotifyDictionaryChangedAction.Added:
					var addedOrder = obj.NewValue ?? throw new ArgumentException("Order is null");
					Orders.Add(addedOrder.ToOrderViewModel());
					break;
				case NotifyDictionaryChangedAction.Removed:
					var removedOrder = Orders.FirstOrDefault(x => x.Id == obj.Key.ToString());
					if (removedOrder == null) break;
					Orders.Remove(removedOrder);
					break;
				case NotifyDictionaryChangedAction.Changed:
					var changedOrder = obj.NewValue ?? throw new ArgumentException("Order is null");
					var stringId = changedOrder.Id.ToString();
					for (int i = 0; i < Orders.Count; i++)
					{
						if (Orders[i].Id == stringId)
						{
							Orders[i] = changedOrder.ToOrderViewModel();
							break;
						}
					}
					break;
			}
		}

		private void UpdateOrdersFromAction(NotifyDictionaryChangedEventArgs<string, decimal> e)
		{
			if (e.Action == NotifyDictionaryChangedAction.Added
				|| e.Action == NotifyDictionaryChangedAction.Changed)
			{
				for (int i = 0; i < Orders.Count; i++)
				{
					if (Orders[i].Symbol == e.Key)
					{
						Orders[i].Value = e.NewValue.ToString();
					}
				}
			}
		}
	}

}
