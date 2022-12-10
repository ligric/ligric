using Ligric.Server.Domain.Entities.Users;
using System;

namespace Ligric.Application.Users.DomainServices
{
    public class UserUniquenessChecker : IUserUniquenessChecker
    {
        public UserUniquenessChecker()
        {
        }

        public bool IsLoginUnique(string userLogin)
        {
            throw new NotImplementedException();
            //var connection = this._sqlConnectionFactory.GetOpenConnection();

            //const string sql = "SELECT TOP 1 1" +
            //                   "FROM [devPace].[Users] AS [User] " +
            //                   "WHERE [User].[Login] = @Login";
            //var usersNumber = connection.QuerySingleOrDefault<int?>(sql,
            //                new
            //                {
            //                    Login = userLogin
            //                });

            //return !usersNumber.HasValue;
        }
    }
}