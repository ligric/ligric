using Ligric.Backend.Application.Configuration.Data;
using Ligric.Backend.Domain.Entities.Users;
using System;

namespace Ligric.Backend.Application.Users.DomainServices
{
    public class UserUniquenessChecker : IUserUniquenessChecker
    {
		private readonly ISqlConnectionFactory _sqlConnectionFactory;
		public UserUniquenessChecker(ISqlConnectionFactory sqlConnectionFactory)
        {
			_sqlConnectionFactory = sqlConnectionFactory;
		}

        public bool IsLoginUnique(string userLogin)
        {
			//var connection = this._sqlConnectionFactory.GetOpenConnection();

			//const string sql = "SELECT TOP 1 1" +
			//				   "FROM [Users] AS [User] " +
			//				   "WHERE [User].[UserName] = @UserName";
			//var usersNumber = connection.QuerySingleOrDefault<int?>(sql,
			//	new
			//	{
			//		Login = userLogin
			//	});

			return false;
		}
    }
}
