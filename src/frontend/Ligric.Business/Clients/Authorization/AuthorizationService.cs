using System;
using System.Threading.Tasks;
using Ligric.Protos;
using Grpc.Net.Client;
using static Ligric.Protos.Authorization;
using Ligric.Core.Types.User;
using Ligric.Business.Metadata;
using System.Threading;
using Ligric.Business.Authorization;

namespace Ligric.Business.Clients.Authorization
{
	public sealed class AuthorizationService : IAuthorizationService
	{
		private readonly IMetadataManager _metadata;
		private readonly AuthorizationClient _client;

		public AuthorizationService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos)
		{
			_client = new AuthorizationClient(grpcChannel);
			_metadata = metadataRepos;
		}

		public UserAuthorizationState CurrentConnectionState { get; private set; }

		public UserDto CurrentUser { get; private set; } = null!;

		public event EventHandler<UserAuthorizationState>? AuthorizationStateChanged;

		public async Task SignInAsync(string login, string password, CancellationToken ct)
		{
			//var passHashed = SecurePasswordHasher.Hash(password);
			var authReply = await _client.SignInAsync(new SignInRequest
			{
				Login = login,
				Password = password
			}, cancellationToken: ct);

			if (!authReply.Result.IsSuccess)
			{
				throw new NotImplementedException();
			}

			var metadata = new Grpc.Core.Metadata
			{
				{ "Authorization", $"Bearer {authReply.JwtToken.AccessToken}" }
			};

			_metadata.SetMetadata(metadata);

			CurrentUser = new UserDto(authReply.Id, login);
			CurrentConnectionState = UserAuthorizationState.Connected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public async Task SignUpAsync(string login, string password, CancellationToken ct)
		{
			//var passHashed = SecurePasswordHasher.Hash(password);
			var authReply = await _client.SignUpAsync(new SignUpRequest
			{
				Login = login,
				Password = password
			}, cancellationToken: ct);

			if (!authReply.Result.IsSuccess)
			{
				throw new NotImplementedException();
			}

			var metadata = new Grpc.Core.Metadata
			{
				{ "Authorization", $"Bearer {authReply.JwtToken.AccessToken}" }
			};

			_metadata.SetMetadata(metadata);

			CurrentUser = new UserDto(authReply.Id, login);
			CurrentConnectionState = UserAuthorizationState.Connected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public async Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct)
		{
			var response = await _client.IsLoginUniqueAsync(new CheckExistsRequest { Value = userName });
			return response.IsUnique;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
