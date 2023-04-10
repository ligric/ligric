using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Ligric.Core.Types.User;
using Ligric.Business.Metadata;
using System.Threading;
using Ligric.Business.Authorization;
using static Ligric.Protobuf.Auth;
using Ligric.Protobuf;

namespace Ligric.Business.Clients.Authorization
{
	public sealed class AuthorizationService : IAuthorizationService
	{
		private readonly IMetadataManager _metadata;
		private readonly AuthClient _client;

		public AuthorizationService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos)
		{
			_client = new AuthClient(grpcChannel);
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
