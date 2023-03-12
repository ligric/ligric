using System;
using NHibernate;

namespace Ligric.Service.AuthService.Infrastructure.Database
{
	public class CoreDataProvider : DataProvider, IDisposable
	{
		private readonly object _lockSession = new object();
		private ISession? _session;
		//private readonly LoggerFactory _loggerFactory;

		public CoreDataProvider(IConnectionSettingsProvider connectionSettingsProvider,
			NhInitFactory nhInitFactory)
			: base(connectionSettingsProvider)
		{
			Factory = nhInitFactory.Factory;
			//_loggerFactory = loggerFactory;
		}

#pragma warning disable CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
		public override ISession? OpenSession()
#pragma warning restore CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
		{
			// ReSharper disable once InconsistentlySynchronizedField
			if (_session != null && _session.IsOpen)
			{
				// ReSharper disable once InconsistentlySynchronizedField
				return _session;
			}

			lock (_lockSession)
			{
                // ReSharper disable once InconsistentlySynchronizedField
                if (_session != null && _session.IsOpen)
                {
                    // ReSharper disable once InconsistentlySynchronizedField
                    return _session;
                }

				if (_session != null && !_session.IsOpen)
				{
					CloseSession();
				}

				if (_session == null)
				{
					_session = Factory?.WithOptions().FlushMode(FlushMode.Auto).OpenSession();
				}

				return _session;
			}
		}

		public override void CloseSession()
		{
			try
			{
				var transaction = _session?.GetCurrentTransaction();
				if (transaction != null && transaction.IsActive == true)
				{
					transaction.Rollback();
				}
			}
			catch (Exception)
			{
				throw;
				//_loggerFactory.Logger.Error(ex, "Exception in session close method");
			}
			finally
			{
                if (_session?.IsOpen == true)
                {
                    _session?.Close();
                }

                _session = null;
			}
		}

		public void Dispose()
		{
			CloseSession();
		}
	}
}
