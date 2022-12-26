using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Ligric.Domain.Types.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using static Ligric.Protos.UserApies;

namespace Ligric.Business
{
    public class UserApiesService
    {
        private readonly UserApiesClient _client;
        private readonly IMetadataRepository _metadata;

        private CancellationTokenSource _chatSubscribeCancellationToken;
        private int messagesChangedSyncNumber = 0;
        public Dictionary<long, ApiDto> Apies { get; } = new Dictionary<long, ApiDto>();

        public UserApiesService(
            GrpcChannel grpcChannel,
            IMetadataRepository metadata)
        {
            _client = new UserApiesClient(grpcChannel);
            _metadata = metadata;
        }

        public event EventHandler<NotifyDictionaryChangedEventArgs<long, ApiDto>> ApiesChanged;

        public async Task SaveApiAsync(string name, string publicKey, string privatekey)
        {
            await _client.SaveAsync(new Protos.SaveApiRequest
            {
                Name = name,
                PublicKey = publicKey,
                PrivateKey = privatekey
            }, _metadata.CurrentMetadata);
        }

        public Task ChatSubscribeAsync()
        {
            if (_chatSubscribeCancellationToken is not null && !_chatSubscribeCancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            var call = _client.ApiesSubscribe(new Empty(), _metadata.CurrentMetadata);
            _chatSubscribeCancellationToken = new CancellationTokenSource();

            return call.ResponseStream
                .ToAsyncEnumerable()
                .Finally(() => call.Dispose())
                .ForEachAsync((x) =>
                {
                    ApiDto api = new ApiDto(
                        long.Parse(x.Api.Id),
                        x.Api.Name,
                        x.Api.PublicKey,
                        x.Api.PrivateKey);

                    switch (x.Action)
                    {
                        case Protos.Action.Added:
                            Apies.AddAndShout(this, ApiesChanged, (long)api.Id, api, ref messagesChangedSyncNumber);
                            break;
                        case Protos.Action.Removed:
                            Apies.RemoveAndShout(this, ApiesChanged, (long)api.Id, ref messagesChangedSyncNumber);
                            break;
                        case Protos.Action.Changed:
                            Apies.SetAndShout(this, ApiesChanged, (long)api.Id, api, ref messagesChangedSyncNumber);
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                }, _chatSubscribeCancellationToken.Token);
        }

    }
}
