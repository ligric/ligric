using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ligric.Domain.Base;
using NHibernate;
using NHibernate.Type;

namespace Ligric.Infrastructure.Database
{
	public class DataInterceptor : EmptyInterceptor
	{
		private const string UpdateDateProperty = nameof(EntityBase.UpdateDate);
		//private const string SyncedFromQbProperty = nameof(BaseSyncEntity.SyncedFromQb);

		public DataInterceptor()
		{

		}

		public override bool OnFlushDirty(
			object entity,
			object id,
			object[] currentState,
			object[] previousState,
			string[] propertyNames,
			IType[] types)
		{
			var entityObj = entity as EntityBase;
			if (entityObj == null)
			{
				throw new InvalidOperationException();
			}
			var skipSyncedFromQb = GetSkipSyncedFromQb(entity);
			return SetBaseProperties(currentState, propertyNames, id, entityObj.SkipUpdateDate, skipSyncedFromQb);
		}

		public override bool OnSave(
			object entity,
			object id,
			object[] state,
			string[] propertyNames,
			IType[] types)
		{
			var entityObj = entity as EntityBase;
			if (entityObj == null)
			{
				throw new NotImplementedException();
			}
			var skipSyncedFromQb = GetSkipSyncedFromQb(entity);
			return SetBaseProperties(state, propertyNames, id, entityObj.SkipUpdateDate, skipSyncedFromQb);
		}

		private bool SetBaseProperties(IList<object> state, string[] propertyNames, object id, bool skipUpdateDate, bool skipSyncedFromQb = false)
		{
			if (id != null)
			{
				var dateTime = DateTime.UtcNow;
				var updateIndex = Array.IndexOf(propertyNames, UpdateDateProperty);
				//var syncedFromQbIndex = Array.IndexOf(propertyNames, SyncedFromQbProperty);

				if (!skipUpdateDate)
				{
					state[updateIndex] = dateTime;
				}

				//if (!skipSyncedFromQb && syncedFromQbIndex != -1)
				//{
				// state[syncedFromQbIndex] = false;
				//}
			}

			return true;
		}

		public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
		{
#if DEBUG
			Trace.WriteLine(sql.ToString());
#endif
			return sql;
		}

		private bool GetSkipSyncedFromQb(object entity)
		{
			throw new NotImplementedException();
			//var skipSyncedFromQb = (entity as BaseSyncEntity)?.SkipSyncedFromQb ?? false;
			//return skipSyncedFromQb;
		}
	}
}
