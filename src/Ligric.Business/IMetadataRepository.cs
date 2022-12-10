using Grpc.Core;
using System;

namespace Ligric.Business;

public interface IMetadataRepository
{
    Metadata? CurrentMetadata { get; }

    event EventHandler<Metadata>? MetadataChanged;

    void SetMetadata(Metadata metadata);
}
