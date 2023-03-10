using System.Collections.Generic;
using Ligric.Service.AuthService.Domain.Entities;

namespace Ligric.Service.AuthService.Application.Repositories
{
	public interface IRepository<TEntity, TSQLQuery> : IRepositoryBase<TEntity>
		where TEntity : EntityBase
	{
		TSQLQuery CreateSqlQueryInTransaction(string querySting);
	}
}
