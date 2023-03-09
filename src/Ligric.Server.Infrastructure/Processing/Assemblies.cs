using System.Reflection;
using Ligric.Server.Application.Users.RegisterUser;

namespace Ligric.Server.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(RegisterUserCommand).Assembly;
    }
}