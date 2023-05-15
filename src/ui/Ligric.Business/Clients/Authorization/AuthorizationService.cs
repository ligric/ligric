using Grpc.Net.Client;
using Ligric.Core.Types.User;
using Ligric.Business.Metadata;
using Ligric.Business.Authorization;
using static Ligric.Protobuf.Auth;
using Ligric.Protobuf;

namespace Ligric.Business.Clients.Authorization
{
	public sealed class AuthorizationService : IAuthorizationService
	{
		private string? refreshToken;
		private readonly IMetadataManager _metadata;
		private readonly AuthClient _client;

		private readonly System.Timers.Timer _jwtTokenExpiredTimer = new System.Timers.Timer();
		private readonly System.Timers.Timer _jwtTokenUptedTimer = new System.Timers.Timer();

		public AuthorizationService(
			GrpcChannel grpcChannel,
			IMetadataManager metadataRepos)
		{
			_client = new AuthClient(grpcChannel);
			_metadata = metadataRepos;
		}

		public UserAuthorizationState CurrentConnectionState { get; private set; }

		public UserDto? CurrentUser { get; private set; }

		public event EventHandler<UserAuthorizationState>? AuthorizationStateChanged;

		public async Task SignInAsync(string login, string password, CancellationToken ct)
		{
			var authReply = await _client.SignInAsync(new SignInRequest
			{
				Login = login,
				Password = password
			}, cancellationToken: ct);

			if (!authReply.Result.IsSuccess)
			{
				throw new NotImplementedException();
			}

			SetJwtToken(authReply.JwtToken);

			CurrentUser = new UserDto(authReply.Id, login);
			CurrentConnectionState = UserAuthorizationState.Connected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
		}

		public async Task SignUpAsync(string login, string password, CancellationToken ct)
		{
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

		public void Logout()
		{
			CleanUser();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private void SetJwtToken(JwtToken jwtToken)
		{
			refreshToken = jwtToken.RefreshToken;
			var metadata = new Grpc.Core.Metadata
			{
				{ "Authorization", $"Bearer {jwtToken.AccessToken}" }
			};
			_metadata.SetMetadata(metadata);

			JwtTokenTimerUpdater(jwtToken.ExpirationAt.ToDateTime());
		}

		private void JwtTokenTimerUpdater(DateTime expirationDateTime)
		{
			_jwtTokenExpiredTimer.Stop();
			_jwtTokenExpiredTimer.AutoReset = false;
			_jwtTokenExpiredTimer.Interval = (double)(expirationDateTime - DateTime.UtcNow).TotalMilliseconds;
			_jwtTokenExpiredTimer.Elapsed -= OnJwtTimerElapsed;
			_jwtTokenExpiredTimer.Elapsed += OnJwtTimerElapsed;
			_jwtTokenExpiredTimer.Start();

			_jwtTokenUptedTimer.Stop();
			_jwtTokenUptedTimer.AutoReset = false;
			_jwtTokenUptedTimer.Interval = (double)(expirationDateTime.AddMinutes(-1) - DateTime.UtcNow).TotalMilliseconds;
			_jwtTokenUptedTimer.Elapsed -= OnJwtTokenUptedTimerElapsed;
			_jwtTokenUptedTimer.Elapsed += OnJwtTokenUptedTimerElapsed;
			_jwtTokenUptedTimer.Start();
		}

		private async void OnJwtTokenUptedTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		 {
			var refreshResponse = await _client.RefreshTokenAsync(new RefreshTokenRequest
			{
				 RefreshToken = refreshToken
			}, headers: _metadata.CurrentMetadata);

			if(refreshResponse.Result.IsSuccess)
			{
				SetJwtToken(refreshResponse.JwtToken);
			}
			else
			{
				throw new NotImplementedException("Refresh token response was unsuccessed.");
			}
			System.Diagnostics.Debug.WriteLine("Expiration token was refreshed.");
		}

		private void OnJwtTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Access token expired");
			CleanUser();
		}

		private void CleanUser()
		{
			_metadata.CleanMetadata();
			CurrentUser = null;
			CurrentConnectionState = UserAuthorizationState.Disconnected;
			AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Disconnected);
		}
	}
}
