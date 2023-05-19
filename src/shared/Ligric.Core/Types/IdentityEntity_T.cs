namespace Ligric.Core.Types
{
	public record IdentityEntity<T>(Guid id, T? Entity); 
}
