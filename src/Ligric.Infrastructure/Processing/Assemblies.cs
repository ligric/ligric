using System.Reflection;
using Ligric.Application.Users.RegisterUser;

namespace Ligric.Infrastructure.Processing
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(RegisterUserCommand).Assembly;
    }
}