namespace Ligric.Business.Metadata;

public interface IMetadataManager
{
	Grpc.Core.Metadata? CurrentMetadata { get; }

	event EventHandler<Grpc.Core.Metadata?>? MetadataChanged;

	void SetMetadata(Grpc.Core.Metadata metadata);

	void CleanMetadata();
}
