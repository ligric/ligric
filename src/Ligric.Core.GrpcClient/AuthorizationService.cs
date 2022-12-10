using Grpc.Core;
using System;
using System.Threading.Tasks;
using DevPace.Core;
using DevPace.Core.DataTypes;
using DevPace.Protos;
using static DevPace.Protos.Authorization;
using Grpc.Net.Client;

namespace DevPace.Core.GrpcClient
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

        public bool IsLoginUnique(string login)
        {
            return _client.IsLoginUnique(new CheckExistsRequest { Value = login }).IsUnique;
        }

        public async Task<bool> IsLoginUniqueAsync(string login)
        {
            CheckExistsResponse response = await _client.IsLoginUniqueAsync(new CheckExistsRequest { Value = login });
            return response.IsUnique;
        }

        public bool IsEmailUnique(string login)
        {
            return _client.IsEmailUnique(new CheckExistsRequest { Value = login }).IsUnique;
        }

        public async Task<bool> IsEmailUniqueAsync(string login)
        {
            CheckExistsResponse response = await _client.IsEmailUniqueAsync(new CheckExistsRequest { Value = login });
            return response.IsUnique;
        }

        public void SignUp(string login, string password, string email)
        {
            var passHash = SecurePasswordHasher.Hash(password);
            var authReply = _client.SignUp(new SignUpRequest { Login = login, Password = passHash, Email = email });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(Guid.Parse(authReply.UserGuid), login, email);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public async Task SignUpAsync(string login, string password, string email)
        {
            var passHash = SecurePasswordHasher.Hash(password);
            var authReply = await _client.SignUpAsync(new SignUpRequest { Login = login, Password = passHash, Email = email });
            var metadata = new Metadata();
            metadata.Add("Authorization", $"Bearer {authReply.JwtToken}");
            _metadataRepos.SetMetadata(metadata);

            CurrentUser = new UserDto(Guid.Parse(authReply.UserGuid), login, email);
            CurrentConnectionState = UserAuthorizationState.Connected;
            AuthorizationStateChanged?.Invoke(this, UserAuthorizationState.Connected);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
