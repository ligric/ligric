using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using DynamicData;
using Ligric.Business.Apies;
using Ligric.Business.Futures;
using Ligric.Core.Ligric.Core.Types.Api;
using Ligric.UI.ViewModels.Data;
using Ligric.UI.ViewModels.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Presentation
{
	public partial class ApisViewModel
	{
		private readonly IDispatcher _dispatcher;
		private readonly IApiesService _apiService;
		private readonly IFuturesOrdersService _ordersService;
		private readonly IFuturesTradesService _valuesService;
		private readonly IPositionsService _postionsService;
		private readonly ILeveragesService _leveragesService;

		internal ApisViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IFuturesOrdersService ordersService,
			IFuturesTradesService valuesService,
			IPositionsService positionsService,
			ILeveragesService leveragesService)
		{
			_dispatcher = dispatcher;
			_apiService = apiesService;
			_ordersService = ordersService;
			_valuesService = valuesService;
			_postionsService = positionsService;
			_leveragesService = leveragesService;

			_apiService.ApiesChanged += OnApiesChanged;

			lock (((ICollection)Apis).SyncRoot)
			{
				foreach (var item in _apiService.AvailableApies)
				{
					Apis.Add(item.ToApiClientViewModel());
				}
			}
		}

		public ObservableCollection<ApiClientViewModel> Apis { get; init; } = new();

		[Reactive]
		public ApiDataViewModel AddingApi { get; } = new();

		public ReactiveCommand<Unit, Unit> SaveApiCommand => ReactiveCommand.CreateFromTask(
			execute:
				ct => ExecuteSaveApi(AddingApi, ct),
			canExecute: this.AddingApi.WhenAnyValue(
				x => x.Name, x => x.PublicKey, x => x.PrivateKey, CanSaveApi));

		public ReactiveCommand<ApiClientViewModel, Unit> ShareApiCommand => ReactiveCommand.CreateFromTask<ApiClientViewModel>(
			execute: ExecuteShareApi, outputScheduler: RxApp.TaskpoolScheduler);

		public ReactiveCommand<ApiClientViewModel, Unit> AttachApiStreamsCommand => ReactiveCommand.CreateFromTask<ApiClientViewModel>(ExecuteAttachApiStream);

		public ReactiveCommand<ApiClientViewModel, Unit> DetachApiStreamsCommand => ReactiveCommand.CreateFromTask<ApiClientViewModel>(ExecuteDetachApiStream);

		#region Save API
		private bool CanSaveApi(string? name, string? publicKey, string? privateKey)
			=> name?.Length >= 1 && publicKey?.Length > 5 && privateKey?.Length > 5;

		private async Task ExecuteSaveApi(ApiDataViewModel api, CancellationToken ct)
		{
			if (api.Name != null && api.PublicKey != null && api.PrivateKey != null)
			{
				await _apiService.SaveApiAsync(api.Name, api.PrivateKey, api.PublicKey, ct);
			}
		}
		#endregion

		private async Task ExecuteAttachApiStream(ApiClientViewModel apiClient)
		{
			if (apiClient.UserApiId == null) throw new ArgumentNullException("UserId is null");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			_ordersService.AttachStreamAsync((long)apiClient.UserApiId);
			_valuesService.AttachStreamAsync((long)apiClient.UserApiId);
			_postionsService.AttachStreamAsync((long)apiClient.UserApiId);
			_leveragesService.AttachStreamAsync((long)apiClient.UserApiId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		private async Task ExecuteDetachApiStream(ApiClientViewModel apiClient)
		{
			if (apiClient.UserApiId == null) throw new ArgumentNullException("UserId is null");

			_ordersService.DetachStream((long)apiClient.UserApiId);
			_valuesService.DetachStream((long)apiClient.UserApiId);
			_postionsService.DetachStream((long)apiClient.UserApiId);
			_leveragesService.DetachStream((long)apiClient.UserApiId);
		}

		public async Task ExecuteShareApi(ApiClientViewModel api, CancellationToken ct)
		{
			if (api != null)
			{
				await _apiService.ShareApiAsync(api.ToApiClientDto(), ct);
			}
		}

		private void OnApiesChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			_dispatcher.TryEnqueue(() =>
			{
				UpdateApisFromAction(e);
			});
		}

		private void UpdateApisFromAction(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var item in e.NewItems!)
					{
						var dto = item as ApiClientDto;
						Apis.Add(dto!.ToApiClientViewModel());
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					Apis.Clear();
					break;
			}
		}
	}
}
