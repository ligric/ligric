using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions;
using Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace Ligric.Service.CryptoApisService.Infrastructure.NHibernate.Database
{
	public class UnCommittedNhInitFactory : NhInitFactory
	{
		public UnCommittedNhInitFactory(IConnectionSettingsProvider connectionSettingsProvider,
			IInterceptor interceptor)
			: base(connectionSettingsProvider, interceptor)
		{
		}

		public override IsolationLevel IsolationLevel => IsolationLevel.ReadUncommitted;
	}

	public class NhInitFactory
	{
		private readonly string _connectionString;
		private readonly List<IConvention> _conventions;
		private readonly IInterceptor _interceptor;

		private ISessionFactory? _factory;

		//TODO: fix this later
		private readonly string[] _assemblyNames = {
			"Ligric.Service.CryptoApisService.Domain",
		};

		public ISessionFactory? Factory => _factory;

		public virtual IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;

		public NhInitFactory(IConnectionSettingsProvider connectionSettingsProvider, IInterceptor interceptor)
		{
			_interceptor = interceptor;
			_conventions = new List<IConvention>();

			_conventions.AddRange(new IConvention[] { new TableNameConvention() });

			_connectionString = connectionSettingsProvider.ConnectionString;

			Initialize();
		}

		protected void Initialize()
		{
			var configuration = new Configuration();
			configuration.DataBaseIntegration(c =>
			{
				c.Dialect<MsSql2012Dialect>();
				c.ConnectionString = _connectionString;
				c.KeywordsAutoImport = Hbm2DDLKeyWords.None;
				c.BatchSize = 100;
				c.Timeout = 0;
				c.HqlToSqlSubstitutions = "true 1, false 0";
				//c.ConnectionProvider<ContextConnectionDriver>();
				c.IsolationLevel = IsolationLevel;
#if DEBUG
				c.LogFormattedSql = true;
				c.LogSqlInConsole = true;
				c.AutoCommentSql = true;
#endif
			}).SetInterceptor(_interceptor ?? new DataInterceptor());

			_factory = Fluently.Configure(configuration)
				.ExposeConfiguration(config =>
				{
					config.SetProperty("adonet.batch_size", "100");
				})
				.Mappings(v =>
				{
					var assembliesWithMapping = GetAssembliesWithMapping();

					foreach (var assembly in assembliesWithMapping)
					{
						v.FluentMappings.AddFromAssembly(assembly).Conventions.Add(_conventions.ToArray());
					}
				})
				//.CurrentSessionContext<SessionContext>()
				.BuildSessionFactory();

#if DEBUG
			//Console.SetOut(new CustomDebugWriter());
#endif
		}

		protected List<Assembly> GetAssembliesWithMapping()
		{
			var assemblies = new List<Assembly>();

			foreach (var assemblyName in _assemblyNames)
			{
				try
				{
					var assembly = Assembly.Load(assemblyName);
					assemblies.Add(assembly);
				}
				catch (FileNotFoundException) { }
			}

			return assemblies;
		}
	}
}
