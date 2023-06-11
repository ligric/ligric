namespace Ligric.Core.Types
{
	public record ChainStreamingEntity<T>(Guid ChainSessionId, Guid StreamSessionId, T Entity);
}
