namespace Ligric.Backend.Data.Base
{
	public interface IConnectionSettingsProvider
	{
		string ConnectionString { get; }

		//string MongoConnectionString { get; }

		//string MongoDatabaseName { get; }
	}
}