namespace Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }
	}
}
