namespace Ligric.Service.CryptoApisService.Infrastructure.Database
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }
	}
}
