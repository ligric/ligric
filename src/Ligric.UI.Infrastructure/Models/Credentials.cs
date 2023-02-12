namespace Ligric.UI.Infrastructure.Models
{
	public record Credentials
	{
		public string? UserName { get; init; }

		public string? Password { get; init; }
	}
}
