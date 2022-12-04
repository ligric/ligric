using Ligric.Server.Data.Base;
using Microsoft.Extensions.Configuration;

namespace Ligric.Infrastructure.Database
{
	public class ConnectionSettingsProvider : IConnectionSettingsProvider
	{
		public ConnectionSettingsProvider(IConfiguration configuration)
		{
			//ConnectionString = configuration.GetConnectionString("LigricConnectionString");
			ConnectionString = configuration["LigricConnectionString"];
            //MongoConnectionString = configuration.GetConnectionString("mongoDatabase");
            //MongoDatabaseName = configuration.GetConnectionString("mongoDatabaseName");
		}

		public string ConnectionString { get; }

		//public string MongoConnectionString { get; }

		//public string MongoDatabaseName { get; }
	}
}