using Grpc.Core;
using System;
using System.Threading.Tasks;
using Ligric.Protos;
using Grpc.Net.Client;
using static Ligric.Protos.Authorization;
using Ligric.Domain.Client.Base;
using Ligric.Domain.Types.User;

namespace Ligric.Business
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly IMetadataRepository _metadataRepos;

        private AuthorizationClient _client;

        public UserAuthorizationState CurrentConnectionState { get; private set; }

        public UserDto CurrentUser { get; private set; }

        public event EventHandler<UserAuthorizationState> AuthorizationStateChanged;

        public AuthorizationService(
            GrpcChannel grpcChannel, 
            IMetadataRepository metadataRepos)
        {
            _client = new AuthorizationClient(grpcChannel);
            _metadataRepos = metadataRepos;
        }

        public bool IsUserNameUnique(string userName)
        {
            return _client.IsLoginUnique(new CheckExistsRequest { Value = userName }).IsUnique;
        }

        public async Task<bool> IsUserNameUniqueAsync(string userName)
        {
            CheckExistsResponse response = await _client.IsLoginUniqueAsync(new CheckExistsRequest { Value = userName });
            return response.IsUnique;
        }

        // TODO : Need refactoring. Reason: dublicate code.

        public void SignIn(string login, string password)
        {
            var authReply = _client.SignIn(new SignInRequest { Login = login, Password = password });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken.Token}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public async Task SignInAsync(string login, string password)
        {
            var authReply = await _client.SignInAsync(new SignInRequest { Login = login, Password = password });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken.Token}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public void SignUp(string login, string password)
        {
            var authReply = _client.SignUp(new SignUpRequest { Login = login, Password = password });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken.Token}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public async Task SignUpAsync(string login, string password)
        {
            var authReply = await _client.SignUpAsync(new SignUpRequest { Login = login, Password = password });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken.Token}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(long.Parse(authReply.UserId), login);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
