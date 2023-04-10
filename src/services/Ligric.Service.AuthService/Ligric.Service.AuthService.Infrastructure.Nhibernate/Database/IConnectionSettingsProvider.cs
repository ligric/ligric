namespace Ligric.Service.AuthService.Infrastructure.Nhibernate.Database
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }
	}
}
