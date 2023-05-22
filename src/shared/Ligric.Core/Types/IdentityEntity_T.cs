namespace Ligric.Core.Types
{
	public record IdentityEntity<T>(Guid Id, T? Entity); 
}
