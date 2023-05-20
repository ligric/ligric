using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using Ligric.Business.Apies;
using Ligric.Business.Interfaces.Futures;
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
		private readonly IFuturesCryptoManager _futuresCryptoManager;

		internal ApisViewModel(
			IDispatcher dispatcher,
			IApiesService apiesService,
			IFuturesCryptoManager futuresCryptoManager)
		{
			_dispatcher = dispatcher;
			_apiService = apiesService;
			_futuresCryptoManager = futuresCryptoManager;

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

		// TODO : Should be ReactiveCommand.Create but Executing doesnt work
		public ReactiveCommand<ApiClientViewModel, Unit> AttachApiStreamsCommand => ReactiveCommand.CreateFromTask<ApiClientViewModel>(ExecuteAttachApiStream);
		// TODO : Should be ReactiveCommand.Create but Executing doesnt work
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

		// TODO : Should be void
		private Task ExecuteAttachApiStream(ApiClientViewModel apiClient)
		{
			if (apiClient.UserApiId == null) throw new ArgumentNullException("UserId is null");
			var client = _futuresCryptoManager.Clients[(long)apiClient.UserApiId];
			client.AttachStream();

			return Task.CompletedTask;
		}

		// TODO : Should be void
		private Task ExecuteDetachApiStream(ApiClientViewModel apiClient)
		{
			if (apiClient.UserApiId == null) throw new ArgumentNullException("UserId is null");
			var client = _futuresCryptoManager.Clients[(long)apiClient.UserApiId];
			client.DetachStream();

			return Task.CompletedTask;
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
