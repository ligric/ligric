namespace Ligric.Service.AuthService.Infrastructure.Database
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }
	}
}
