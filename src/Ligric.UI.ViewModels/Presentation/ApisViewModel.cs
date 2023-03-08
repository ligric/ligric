using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Ligric.Business.Apies;
using Ligric.Business.Clients.Futures;
using Ligric.Business.Futures;
using Ligric.Domain.Types.Api;
using Ligric.UI.Infrastructure;
using Ligric.UI.ViewModels.Data;
using Microsoft.UI.Dispatching;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Uno.Extensions.Reactive.Dispatching;

namespace Ligric.UI.ViewModels.Presentation
{
	public partial class ApisViewModel
	{
		private readonly IDispatcher _dispatcher;
		private readonly IApiesService _apiService;
		private readonly IOrdersService _ordersService;
		private readonly IValuesService _valuesService;
		private readonly IPositionsService _postionsService;

		internal ApisViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IOrdersService ordersService,
			IValuesService valuesService,
			IPositionsService positionsService)
		{
			_dispatcher = dispatcher;
			_apiService = apiesService;
			_ordersService = ordersService;
			_valuesService = valuesService;
			_postionsService = positionsService;

			_apiService.ApiesChanged += OnApiesChanged;
		}

		public ObservableCollection<ApiClientDto> Apis { get; init; } = new();

		[Reactive]
		public ApiDataViewModel AddingApi { get; } = new();

		public ReactiveCommand<Unit, Unit> SaveApiCommand => ReactiveCommand.CreateFromTask(
			execute: ct => ExecuteSaveApi(AddingApi, ct),
			canExecute: this.WhenAnyValue(x => x.AddingApi, api => CanSaveApi(api)));

		public ReactiveCommand<ApiClientDto, Unit> ShareApiCommand => ReactiveCommand.CreateFromTask<ApiClientDto>(
			execute: async (apiClient, ct) => await ExecuteShareApi(apiClient, ct));

		public ReactiveCommand<ApiClientDto, Unit> AttachApiStreamsCommand => ReactiveCommand.CreateFromTask<ApiClientDto>(ExecuteAttachApiStream);

		#region Save API
		private bool CanSaveApi(ApiDataViewModel api)
			=> api is { Name.Length: >= 1 } and { PrivateKey.Length: > 5 } and { PublicKey.Length: > 5 };

		private async Task ExecuteSaveApi(ApiDataViewModel api, CancellationToken ct)
		{
			if (api.Name != null && api.PublicKey != null && api.PrivateKey != null)
			{
				await _apiService.SaveApiAsync(api.Name, api.PrivateKey, api.PublicKey, ct);
			}
		}
		#endregion

		private async Task ExecuteAttachApiStream(ApiClientDto apiClient)
		{
			if (apiClient.UserApiId == null) throw new ArgumentNullException("UserId is null");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			_ordersService.AttachStreamAsync((long)apiClient.UserApiId);
			_valuesService.AttachStreamAsync((long)apiClient.UserApiId);
			_postionsService.AttachStreamAsync((long)apiClient.UserApiId);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		public async Task ExecuteShareApi(ApiClientDto api, CancellationToken ct)
		{
			if (api != null)
			{
				await _apiService.ShareApiAsync(api, ct);
			}
		}

		private void OnApiesChanged(object sender, NotifyCollectionChangedEventArgs e)
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
					Apis.AddRange(e.NewItems);
					break;
			}
		}
	}
}
