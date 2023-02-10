using Grpc.Core;
using Grpc.Dotnet.Shared.Helpers.IntegrationTests;
using Ligric.Server.Grpc;
using Microsoft.EntityFrameworkCore;

namespace Ligric.Server.IntegrationTests
{
    public class PermissionsServerTestBase<TServiceClient> : GrpcServerTestBase<Startup, DbContext, PermissionsServerApplicationFactory, TServiceClient>
        where TServiceClient : ClientBase<TServiceClient>
    {
        public PermissionsServerTestBase(PermissionsServerApplicationFactory factory) : base(factory)
        {
        }
    }
}
