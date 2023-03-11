using System;
using System.Collections.Generic;
using Ligric.Service.AuthService.Application.Repositories;
using Ligric.Service.AuthService.Domain.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;

namespace Ligric.Service.AuthService.Infrastructure
{
	public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
		where TEntity : EntityUnit
	{
		protected readonly DataProvider DataProvider;

		protected RepositoryBase(DataProvider dataProvider)
		{
			DataProvider = dataProvider;
		}

		public virtual object Save(TEntity entity)
		{
			DataProvider.BeginTransaction();

			var saved = SaveInTransaction(entity);

			DataProvider.CommitTransaction();

			return saved;
		}

		public virtual object SaveInTransaction(TEntity entity)
		{
			return SaveInTransactionInternal(entity);
		}

		public virtual TEntity GetEntityById(long id)
		{
			return DataProvider.Get<TEntity>(id);
		}

		public virtual IEnumerable<TEntity> GetAll()
		{
			return DataProvider.GetAll<TEntity>();
		}

		public virtual void Delete(TEntity entity)
		{
			DataProvider.BeginTransaction();
			DeleteInTransaction(entity);
			DataProvider.CommitTransaction();
		}

		public virtual void Delete(long id)
		{
			DataProvider.BeginTransaction();
			DeleteInTransaction(id);
			DataProvider.CommitTransaction();
		}

		public virtual void DeleteInTransaction(long id)
		{
			var entity = GetEntityById(id);
			if (entity == null)
			{
				return;
			}

			DeleteInTransaction(entity);
		}

		public virtual void DeleteInTransaction(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			DataProvider.Delete(entity);
		}

		public virtual void DeleteList(IEnumerable<TEntity> entities)
		{
			DataProvider.BeginTransaction();

			foreach (var entity in entities)
			{
				DeleteInTransaction(entity);
			}

			DataProvider.CommitTransaction();
		}

		protected IQueryOver<T> ToDistinctRowCount<T>(IQueryOver<T, T> query) where T : EntityBase
		{
			return query.Clone()
				.Select(Projections.CountDistinct<T>(x => x.Id))
				.ClearOrders()
				.Skip(0)
				.Take(RowSelection.NoValue);
		}

		protected object SaveInTransactionInternal(TEntity entity)
		{
			if (entity.IsNew)
			{
				return DataProvider.Save(entity);
			}

			DataProvider.Update(entity);
			return entity;
		}

		protected ISQLQuery CreateSqlQueryInTransaction(string querySting)
		{
			DataProvider.Session.Flush();

			return DataProvider.Session.CreateSQLQuery(querySting);
		}
	}
}
