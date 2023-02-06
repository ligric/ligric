using Grpc.Core;
using Ligric.Business;
using System;

namespace LigricUno
{
    public sealed class MetadataRepository : IMetadataRepository
    {
        public Metadata? CurrentMetadata { get; private set; }

        public event EventHandler<Metadata>? MetadataChanged;

        public void SetMetadata(Metadata metadata)
        {
            if (CurrentMetadata == metadata)
                return;

            CurrentMetadata = metadata;
            MetadataChanged?.Invoke(this, metadata);
        }
    }
}
