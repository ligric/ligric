using Grpc.Core;
using System;

namespace DevPace.Core.GrpcClient;

public interface IMetadataRepository
{
    Metadata? CurrentMetadata { get; }

    event EventHandler<Metadata>? MetadataChanged;

    void SetMetadata(Metadata metadata);
}
