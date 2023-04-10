using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using FluentNHibernate.Conventions;
using Ligric.Service.AuthService.Domain.Entities;
using Ligric.Service.AuthService.Infrastructure.Nhibernate.Conventions;
using NHibernate;
using NHibernate.Criterion;

namespace Ligric.Service.AuthService.Infrastructure.Nhibernate.Database
{
	public abstract class DataProvider
	{
		protected readonly string ConnectionString;
		protected readonly List<IConvention> Conventions;

		protected ISessionFactory? Factory;

		protected DataProvider(IConnectionSettingsProvider connectionSettingsProvider)
		{
			Conventions = new List<IConvention>();

			Conventions.AddRange(new IConvention[] { new TableNameConvention() });

			ConnectionString = connectionSettingsProvider.ConnectionString;
		}

		public virtual ISession Session => OpenSession();

		public abstract ISession OpenSession();

		public abstract void CloseSession();

		public void BeginTransaction(IsolationLevel isolationLevel)
		{
			if (Session != null)
			{
				Session.BeginTransaction(isolationLevel);
			}
			else
			{
				throw new NullReferenceException("Session is null here.");
			}
		}

		public void BeginTransaction()
		{
			BeginTransaction(IsolationLevel.ReadCommitted);
		}

		public void CommitTransaction()
		{
			try
			{
				var transaction = Session?.GetCurrentTransaction();
				if (transaction != null && transaction.IsActive)
				{
					transaction.Commit();
				}
			}
			catch (Exception)
			{
				RollbackTransaction();
				throw;
			}
		}

		public void Clear()
		{
			Session.Clear();
		}

		public ICriteria CreateCriteria<T>()
			where T : EntityUnit
		{
			return Session.CreateCriteria(typeof(T));
		}

		public object Save<T>(T entity)
			where T : EntityUnit
		{
			return Session.Save(entity);
		}

		public void Delete<T>(T entity)
			where T : EntityUnit
		{
			if (Session.Contains(entity))
			{
				Session.Delete(entity);
			}
		}

		public IQueryOver<T, T> QueryOver<T>() where T : EntityUnit
		{
			var query = Session.QueryOver<T>();
			return query;
		}

		public ISQLQuery SQLQuery(string sql)
		{
			var query = Session.CreateSQLQuery(sql);
			return query;
		}

		public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : EntityUnit
		{
			var query = Session.QueryOver(alias);
			return query;
		}

		public IQueryOver<T, T> QueryOver<T>(QueryOver<T> detachedQuery) where T : EntityUnit
		{
			var query = detachedQuery.GetExecutableQueryOver(Session);
			return query;
		}

		public T? Unproxy<T>(T enity) where T : EntityUnit
		{
			return Session.GetSessionImplementation().PersistenceContext.Unproxy(enity) as T;
		}

		public T Get<T>(object id)
			where T : EntityUnit
		{
			return Session.Get<T>(id);
		}

		public IEnumerable<T> GetAll<T>()
			where T : EntityUnit
		{
			return Session.QueryOver<T>().List();
		}

		public IStatelessSession? OpenStatelessSession()
		{
			return Factory?.OpenStatelessSession();
		}

		public ISQLQuery? CreateSqlQuery(string query)
		{
			return Session?.CreateSQLQuery(query);
		}

		public void SaveOrUpdate<T>(T entity)
			where T : EntityUnit
		{
			//TODO: check and fix that
			try
			{
				Session.SaveOrUpdate(entity);
			}
			catch (NonUniqueObjectException)
			{
				var sessionEnt = Session.Load<T>(entity.Id);

				if (sessionEnt.Id == entity.Id)
				{
					Session.Evict(sessionEnt);
				}
				Session.SaveOrUpdate(entity);
			}
		}

		public void Update<T>(T entity)
			where T : EntityUnit
		{
			//TODO: check and fix that
			try
			{
				Session.Update(entity);
			}
			catch (NonUniqueObjectException)
			{
				var sessionEnt = Session.Load<T>(entity.Id);

				if (sessionEnt.Id == entity.Id)
				{
					Session.Evict(sessionEnt);
				}

				Session.Update(entity);
			}
		}

		public void Merge<T>(T entity)
			where T : EntityUnit
		{
			Session.Merge(entity);
		}

		public void RollbackTransaction()
		{
			try
			{
				var transaction = Session?.GetCurrentTransaction();
				if (transaction != null && transaction.IsActive)
				{
					transaction.Rollback();
				}
			}
			finally
			{
				CloseSession();
			}
		}
	}
}
