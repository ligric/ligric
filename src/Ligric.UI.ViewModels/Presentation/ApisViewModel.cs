using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Ligric.Business.Apies;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Api;
using Ligric.UI.Infrastructure;
using Ligric.UI.ViewModels.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Presentation
{
	public partial class ApisViewModel
	{
		private readonly IApiesService _apiService;
		private readonly IOrdersService _ordersService;
		internal ApisViewModel(
			IApiesService apiesService,
			IOrdersService ordersService)
		{
			_apiService = apiesService;
			_ordersService = ordersService;

			ApisChangedEventSubscribe();
		}

		public ObservableCollection<ApiClientDto> Apis { get; init; } = new();

		[Reactive]
		public ApiDataViewModel AddingApi { get; } = new();

		public ReactiveCommand<Unit, Unit> SaveApiCommand => ReactiveCommand.CreateFromTask(
			execute: ct => ExecuteSaveApi(AddingApi, ct),
			canExecute: this.WhenAnyValue(x => x.AddingApi, api => CanSaveApi(api)));

		public ReactiveCommand<ApiClientDto, Unit> ShareApiCommand => ReactiveCommand.CreateFromTask<ApiClientDto>(
			execute: async (apiClient, ct) => await ExecuteShareApi(apiClient, ct));

		public ReactiveCommand<Unit, Unit> SelectAllCommand => ReactiveCommand.CreateFromTask(
			execute: ct => ExecuteSelectAll(ct));

		#region Save API
		private bool CanSaveApi(ApiDataViewModel api)
			=> api is { Name.Length: >= 1 } and { PrivateKey.Length: > 5 } and { PublicKey.Length: > 5 };

		private async Task ExecuteSaveApi(ApiDataViewModel api, CancellationToken ct)
		{
			if (api.Name != null && api.PublicKey != null && api.PrivateKey != null)
			{
				await _apiService.SaveApiAsync(api.Name, api.PublicKey, api.PrivateKey, ct);
			}
		}
		#endregion

		private async Task ExecuteSelectAll(CancellationToken ct)
		{
			foreach (var api in Apis)
			{
				if (ct.IsCancellationRequested) break;

				await _ordersService.AttachStreamAsync(api.UserApiId ?? throw new ArgumentNullException("UserId is null"));
			}
		}

		public async Task ExecuteShareApi(ApiClientDto api, CancellationToken ct)
		{
			if (api != null)
			{
				await _apiService.ShareApiAsync(api, ct);
			}
		}

		#region Api Changed Subscribtion
		private void ApisChangedEventSubscribe()
		{
			var collectionChanged = Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(handler =>
			{
				NotifyCollectionChangedEventHandler collectionChanged = (sender, e) =>
				{
					handler(e);
				};

				return collectionChanged;
			}, fsHandler => _apiService.ApiesChanged += fsHandler, fsHandler => _apiService.ApiesChanged -= fsHandler);

			collectionChanged.ObserveOn(Schedulers.Dispatcher).Subscribe(OnApiesChanged);
		}

		private void OnApiesChanged(NotifyCollectionChangedEventArgs e)
		{
			UpdateApisFromAction(e);
		}

		private void UpdateApisFromAction(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Apis.AddRange(e.NewItems);
					break;
			}
		}
		#endregion
	}

}
