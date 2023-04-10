﻿using System.Collections.Generic;
using Ligric.Service.CryptoApisService.Domain.Entities;

namespace Ligric.Service.CryptoApisService.Application.Repositories
{
	public interface IRepository<TEntity>
		where TEntity : EntityUnit
	{
		object Save(TEntity entity);

		object SaveInTransaction(TEntity entity);

		TEntity GetEntityById(long id);

		IEnumerable<TEntity> GetAll();

		void Delete(TEntity entity);

		void Delete(long id);

		void DeleteInTransaction(long id);

		void DeleteInTransaction(TEntity entity);

		void DeleteList(IEnumerable<TEntity> entities);
	}
}
