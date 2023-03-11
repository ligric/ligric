namespace Ligric.Service.AuthService.Infrastructure
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }
	}
}
