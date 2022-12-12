using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ligric.Protos;
using MediatR;
using Google.Protobuf.WellKnownTypes;
using System.Diagnostics;
using Ligric.Server.Grpc.Services.LocalTemporary;
using Ligric.Server.Grpc.Extensions;
using Ligric.Common.Types;
using Microsoft.AspNetCore.Components.Forms;
using Ligric.Common.Types.Api;

namespace Ligric.Server.Grpc.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserApiesService : UserApies.UserApiesBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly UserapiesLocalService _localApies;

    public UserApiesService(IMediator mediator, UserapiesLocalService online)
    {
        _mediator = mediator;
        _localApies = online;
    }

    private static readonly Random _random = new Random();
    [Authorize]
    public override async Task<SaveApiResponse> Save(SaveApiRequest request, ServerCallContext context)
    {
        var api = new ApiDto(_random.Next(), request.Name, request.PublicKey, request.PrivateKey);
        _localApies.AddApi(api);

        SaveApiResponse response = new SaveApiResponse
        {
            ApiId = api.Id.ToString(),
            Result = ResponseExtensions.GetSuccessResponseResult()
        };
        return response;
    }

    [Authorize]
    public override async Task ApiesSubscribe(Empty request, IServerStreamWriter<ApiesChanged> responseStream, ServerCallContext context)
    {
        var peer = context.Peer;
        Debug.WriteLine($"{peer} frame captures subscribes.");

        context.CancellationToken.Register(() => Debug.WriteLine($"{peer} cancel frame captures subscription."));

        try
        {
            await _localApies.GetUserOnlinesAsObservable()
                .ToAsyncEnumerable()
                .ForEachAwaitAsync(async (x) => await responseStream.WriteAsync(
                    new ApiesChanged
                    {
                        Action = x.Action.ToProtosAction(),
                        Api = new Protos.Api
                        {
                            Id = x.Api.Id.ToString(),
                            Name = x.Api.Name.ToString(),
                            PublicKey = x.Api.PublicKey.ToString(),
                            PrivateKey = x.Api.PrivateKey.ToString()
                        }
                    }), context.CancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            Debug.WriteLine($"{peer} frame captures unsubscribed.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"{peer} frame captures unsubscribed. Was some error: {ex}");
        }
    }
}
