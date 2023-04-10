using System;
using System.Collections.Generic;
using Ligric.Backend.Domain.Base;
using NHibernate;
using NHibernate.Type;

namespace Ligric.Backend.Infrastructure.Database
{
	public class QbDataInterceptor : EmptyInterceptor
	{
		private const string UpdateDateProperty = nameof(EntityBase.UpdateDate);

		public override bool OnFlushDirty(
			object entity,
			object id,
			object[] currentState,
			object[] previousState,
			string[] propertyNames,
			IType[] types)
		{
			return SetBaseProperties(currentState, propertyNames, entity);
		}

		public override bool OnSave(
			object entity,
			object id,
			object[] state,
			string[] propertyNames,
			IType[] types)
		{
			return SetBaseProperties(state, propertyNames, entity);
		}

		private bool SetBaseProperties(IList<object> state, string[] propertyNames, object entity)
		{
			var dateTime = DateTime.UtcNow;
			var updateIndex = Array.IndexOf(propertyNames, UpdateDateProperty);

			state[updateIndex] = dateTime;

			//var syncEntity = entity as BaseSyncEntity;
			//if (syncEntity == null)
			//{
			//	return true;
			//}

			//var skipSyncedFromQb = syncEntity.SkipSyncedFromQb;

			//var syncedFromQbIndex = Array.IndexOf(propertyNames, SyncedFromQbProperty);

			//if (!skipSyncedFromQb && syncedFromQbIndex != -1)
			//{
			//	state[syncedFromQbIndex] = true;
			//}

			return true;
		}
	}
}