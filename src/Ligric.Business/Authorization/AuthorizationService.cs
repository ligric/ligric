using System;
using System.Threading.Tasks;
using Ligric.Protos;
using Grpc.Net.Client;
using static Ligric.Protos.Authorization;
using Ligric.Domain.Types.User;
using Ligric.Business.Metadata;
using System.Threading;

namespace Ligric.Business.Authorization
{
	public sealed class AuthorizationService : IAuthorizationService
	{
		private readonly IMetadataManager _metadataRepos;
		private AuthorizationClient _client;

		public UserAuthorizationState CurrentConnectionState { get; private set; }

		public UserDto CurrentUser { get; private set; } = null!;

		public event EventHandler<UserAuthorizationState>? AuthorizationStateChanged;

		public AuthorizationService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos)
		{
			_client = new AuthorizationClient(grpcChannel);
			_metadataRepos = metadataRepos;
		}

		public bool IsUserNameUnique(string userName)
		{
			return _client.IsLoginUnique(new CheckExistsRequest { Value = userName }).IsUnique;
		}

		public async Task<bool> IsUserNameUniqueAsync(string userName, CancellationToken ct)
		{
			var response = await _client.IsLoginUniqueAsync(new CheckExistsRequest { Value = userName });
			return response.IsUnique;
		}

		// TODO : Need refactoring. Reason: dublicate code.

		public void SignIn(string login, string password)
		{
			var authReply = _client.SignIn(new SignInRequest { Login = login, Password = password });
			var metadata = new Grpc.Core.Metadata();
			metadata.Add("Authorization", $"Bearer {authReply.JwtToken.AccessToken}");
			_metadataRepos.SetMetadata(metadata);

			//CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
			//CurrentConnectionState = UserAuthorizationState.Connected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public async Task SignInAsync(string login, string password, CancellationToken ct)
		{
			var authReply = await _client.SignInAsync(new SignInRequest
			{
				Login = login,
				Password = password
			}, cancellationToken: ct);

			var metadata = new Grpc.Core.Metadata
			{
				{ "Authorization", $"Bearer {authReply.JwtToken.AccessToken}" }
			};

			_metadataRepos.SetMetadata(metadata);

			CurrentUser = new UserDto(login);
			CurrentConnectionState = UserAuthorizationState.Connected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public void SignUp(string login, string password)
		{
			var authReply = _client.SignUp(new SignUpRequest { Login = login, Password = password });
			var metadata = new Grpc.Core.Metadata();
			metadata.Add("Authorization", $"Bearer {authReply.JwtToken.AccessToken}");
			_metadataRepos.SetMetadata(metadata);

			//CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
			//CurrentConnectionState = UserAuthorizationState.Connected;
			//AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public async Task SignUpAsync(string login, string password, CancellationToken ct)
		{
			var authReply = await _client.SignUpAsync(new SignUpRequest { Login = login, Password = password });
			var metadata = new Grpc.Core.Metadata();
			metadata.Add("Authorization", $"Bearer {authReply.JwtToken.AccessToken}");
			_metadataRepos.SetMetadata(metadata);

			//CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
			//CurrentConnectionState = UserAuthorizationState.Connected;
			//AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
