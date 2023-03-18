using Microsoft.Extensions.Configuration;

namespace Ligric.Service.AuthService.Infrastructure.Nhibernate.Database
{
	public class ConnectionSettingsProvider : IConnectionSettingsProvider
	{
		public ConnectionSettingsProvider(IConfiguration configuration)
		{
			//ConnectionString = configuration.GetConnectionString("ConnectionString");
#pragma warning disable CS8601 // Possible null reference assignment.
			ConnectionString = configuration["ConnectionString"];
#pragma warning restore CS8601
			// Possible null reference assignment.
			//MongoConnectionString = configuration.GetConnectionString("mongoDatabase");
			//MongoDatabaseName = configuration.GetConnectionString("mongoDatabaseName");
		}

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
		public string? ConnectionString { get; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

		//public string MongoConnectionString { get; }

		//public string MongoDatabaseName { get; }
	}
}
