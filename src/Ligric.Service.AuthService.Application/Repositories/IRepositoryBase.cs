using System.Collections.Generic;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IRepositoryBase<TEntity>
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

		object SaveInTransactionInternal(TEntity entity);
	}
}
