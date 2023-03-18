using Ligric.Service.AuthService.Domain.Checkers;
using System;

namespace Ligric.Service.AuthService.Infrastructure.Domain
{
	public class UserUniquenessChecker : IUserUniquenessChecker
	{
		public UserUniquenessChecker()
		{

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
