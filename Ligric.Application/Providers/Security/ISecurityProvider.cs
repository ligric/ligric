using System;
using Ligric.Common.Types;

namespace Ligric.Application.Providers.Security
{
    public interface ISecurityProvider
    {
        UserDto Login(string login, string password);

        UserDto QuickLogin(Guid key);

        bool LogOut(string token);

        string AuthenticateRequest(string token);

        void ThrownUnauthorized(string message = null);
    }
}